using Hangfire;
using VOCENuOrderApp.Data;
using VOCENuOrderApp.Models.NUORDER;
using VOCENuOrderApp.Models.XORO;
using System.Text.RegularExpressions;
using System.Text.Json; // added

namespace VOCENuOrderApp.Services
{
    public class VOCE_NuOrderReplenishmentSync
    {
        private readonly XoroApiServiceVOCE _xoroApiService;
        private readonly NuOrderApiServiceVOCE _nuOrderApiService;
        private readonly ILogger<VOCE_NuOrderReplenishmentSync> _logger;
        private readonly ApplicationDbContext _context;

        // private const string WarehouseId = "67881a879b9810346496f60b"; // Sandbox
        private const string WarehouseId = "628d4fa01f9aa012b5f1a1a5";  // Production

        public VOCE_NuOrderReplenishmentSync(
            XoroApiServiceVOCE xoroApiService,
            NuOrderApiServiceVOCE nuOrderApiService,
            ILogger<VOCE_NuOrderReplenishmentSync> logger,
            ApplicationDbContext context)
        {
            _xoroApiService = xoroApiService;
            _nuOrderApiService = nuOrderApiService;
            _logger = logger;
            _context = context;
        }

        public async Task VOCE_NuOrder_Replen_CSVLIST(string basePartsCsvOrPipe)
        {
            try
            {
                var baseParts = Regex
                    .Split(basePartsCsvOrPipe, @"[|,]")
                    .Select(bp => bp.Trim())
                    .Where(bp => !string.IsNullOrWhiteSpace(bp))
                    .ToList();

                if (!baseParts.Any())
                    throw new ArgumentException("No valid base parts provided.");

                var stores = new[] { "Main Warehouse" }; // add more if needed: "NRI", "NRI - LA", "HQ", "PRE BOOK"
                int delaySeconds = 0;

                foreach (var basePart in baseParts)
                {
                    var scheduledDelay = TimeSpan.FromSeconds(delaySeconds);
                    BackgroundJob.Schedule(
                        () => SyncReplenishmentForBasePart(basePart, stores),
                        scheduledDelay);

                    delaySeconds += 20; // throttle between jobs
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VOCE_NuOrder_Replen_CSVLIST");
                throw;
            }
        }

        // Example single-run
        public async Task VOCE_NuOrder_Replen()
        {
            await SyncReplenishmentForBasePart("S0284L", new[] { "Main Warehouse" });
        }

        // Main sync for one base part - Replenishment Mapping
        public async Task SyncReplenishmentForBasePart(string basePartNumber, string[] stores)
        {
            // 1) Inventory per item across stores
            var inventoryVariants = await _xoroApiService.GetInventoryByBasePartAsync(basePartNumber, stores);
            if (inventoryVariants == null || !inventoryVariants.Any())
            {
                _logger.LogWarning("No inventory variants for {BasePart}", basePartNumber);
                return;
            }

            // Aggregate per item number
            var aggregatedInventory = AggregateInventoryByItem(basePartNumber, inventoryVariants);

            // Items that actually have any PO qty (so we don't pull POs for everything)
            var itemNumbersWithPO = aggregatedInventory
                .Where(inv => inv.TotalQtyOnPO > 0)
                .Select(inv => inv.ItemNumber)
                .Distinct()
                .ToList();

            // 2) Pull incoming PO deliveries for those items
            var incomingPOs = new List<IncomingPOSummary>();
            if (itemNumbersWithPO.Any())
            {
                var incomingPOData = await _xoroApiService.GetIncomingPODeliveriesAsync(itemNumbersWithPO, stores);
                incomingPOs = incomingPOData.Select(po => new IncomingPOSummary
                {
                    ItemNumber = po.ItemNumber,
                    AvailablePOLineQty = po.AvailablePOLineQty,
                    ExpectedDeliveryDate = po.ExpectedDeliveryDate,
                    PONumber = po.PONumber,
                    LinkedSOQty = po.LinkedSOQty
                }).ToList();
            }

            // 3) Pull product for NuORDER IDs
            var xoroProduct = await _xoroApiService.GetProductByBasePartAsync(basePartNumber);
            if (xoroProduct == null || xoroProduct.Variants == null || !xoroProduct.Variants.Any())
            {
                _logger.LogWarning("No product/variants found for {BasePart} when building replenishments.", basePartNumber);
                return;
            }

            // 4) Group by color code; only variants that have NuORDER size id (H3)
            var colorGroups = xoroProduct.Variants
                .Where(v => !string.IsNullOrWhiteSpace(v.Option1ValueCode) &&
                            !string.IsNullOrWhiteSpace(v.CustomFieldH3))     // H3 = NuORDER SKU/size _id
                .GroupBy(v => v.Option1ValueCode);

            foreach (var colorGroup in colorGroups)
            {
                var colorCode = colorGroup.Key;
                _logger.LogInformation("Processing color group {ColorCode} for {BasePart}.", colorCode, basePartNumber);

                var replenishmentList = new List<NuOrderReplenishment>();
                var productIdsForColorGroup = new HashSet<string>(); // NuORDER product (style) _id(s) to refresh

                // business flags (if any variant says Immediate, we’ll send immediate rows)
                bool hasImmediateFlag = colorGroup.Any(v => v.CustomFieldH10 == "Y");

                foreach (var variant in colorGroup)
                {
                    string itemNumber = variant.ItemNumber;
                    string nuOrderSizeId = variant.CustomFieldH3;   // size(SKU) _id
                    string nuOrderProductId = variant.CustomFieldH12;  // product(style) _id

                    if (!string.IsNullOrWhiteSpace(nuOrderProductId))
                        productIdsForColorGroup.Add(nuOrderProductId);

                    if (string.IsNullOrWhiteSpace(itemNumber) || string.IsNullOrWhiteSpace(nuOrderSizeId))
                    {
                        var msg = $"Skipping variant with missing ItemNumber/H3. ItemNumber='{itemNumber}', H3='{nuOrderSizeId}'";
                        _logger.LogWarning(msg);
                        await LogReplenishmentSync(basePartNumber, colorCode, nuOrderSizeId, null, null, false, "Failed", msg, null, null, "VOCE");
                        continue;
                    }

                    var inv = aggregatedInventory.FirstOrDefault(i => i.ItemNumber == itemNumber);
                    //float? atsQty = inv != null && inv.TotalATSQty > 0 ? inv.TotalATSQty : (float?)null;
                    //float? atsQty = inv != null && inv.TotalATSQty > 0 ? inv.TotalATSQty : 0f;
                    float? atsQty = inv?.TotalATSQty;

                    // ---- Immediate (from ATS) ----
                    if (hasImmediateFlag)
                    {
                        replenishmentList.Add(new NuOrderReplenishment
                        {
                            sku_id = nuOrderSizeId,
                            warehouse_id = WarehouseId,
                            display_name = "immediate",
                            prebook = false,
                            period_start = DateTime.UtcNow.ToString("MM/dd/yyyy"),
                            period_end = null,
                            quantity = atsQty,
                            active = true
                        });

                        await LogReplenishmentSync(basePartNumber, colorCode, nuOrderSizeId, atsQty, null, false,
                            "Success", "Immediate replenishment staged", null, null, "VOCE");
                    }
                    else
                    {
                        _logger.LogInformation("Skipping immediate for SKU {SkuId} (CustomFieldH10 != 'Y').", nuOrderSizeId);
                    }

                    // ---- Prebook (unchanged) ----
                    if (variant.IsPreSellFlag)
                    {
                        replenishmentList.Add(new NuOrderReplenishment
                        {
                            sku_id = nuOrderSizeId,
                            warehouse_id = WarehouseId,
                            display_name = "Prebook",
                            prebook = true,
                            period_start = DateTime.Now.ToString("MM/dd/yyyy"),
                            period_end = null,
                            quantity = null,
                            active = true
                        });
                        if (DateTime.TryParse(variant.CustomFieldH13, out var preStart) &&
                            DateTime.TryParse(variant.CustomFieldH14, out var preEnd))
                        {
                            await LogReplenishmentSync(basePartNumber, colorCode, nuOrderSizeId, null, null, true,
                                "Success", "Prebook replenishment staged", null, null, "VOCE");
                        }
                        else
                        {
                            var msg = $"Invalid prebook period for {itemNumber}: start='{variant.CustomFieldH13}', end='{variant.CustomFieldH14}'";
                            _logger.LogWarning(msg);
                            await LogReplenishmentSync(basePartNumber, colorCode, nuOrderSizeId, null, null, true, "Failed", msg, null, null, "VOCE");
                        }
                    }
                } // variants in color

                // ---- Future: full size range per ETA ----
                var futurePoLinesForColor = incomingPOs
                    .Where(po => po.AvailablePOLineQty.HasValue && po.AvailablePOLineQty.Value > 0)
                    .Where(po => colorGroup.Any(v => v.ItemNumber == po.ItemNumber))
                    .ToList();

                var futureDates = futurePoLinesForColor
                    .Select(po => NormalizeEtaDate(po.ExpectedDeliveryDate))
                    .Where(d => d.HasValue)
                    .Select(d => d.Value)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();

                foreach (var eta in futureDates)
                {
                    foreach (var variant in colorGroup)
                    {
                        var itemNumber = variant.ItemNumber;
                        var nuOrderSizeId = variant.CustomFieldH3;

                        if (string.IsNullOrWhiteSpace(itemNumber) || string.IsNullOrWhiteSpace(nuOrderSizeId))
                        {
                            continue;
                        }

                        float qty = futurePoLinesForColor
                            .Where(po => po.ItemNumber == itemNumber && NormalizeEtaDate(po.ExpectedDeliveryDate) == eta)
                            .Sum(po => po.AvailablePOLineQty ?? 0f);

                        replenishmentList.Add(new NuOrderReplenishment
                        {
                            sku_id = nuOrderSizeId,
                            warehouse_id = WarehouseId,
                            display_name = "future",
                            prebook = false,
                            period_start = eta.ToString("MM/dd/yyyy"),
                            period_end = null, //eta.ToString("MM/dd/yyyy"),
                            quantity = qty,
                            active = true
                        });

                        await LogReplenishmentSync(
                            basePartNumber, colorCode, nuOrderSizeId,
                            null, qty, false, "Success",
                            $"Future staged for ETA {eta:MM/dd/yyyy} (full size range)", null, null, "VOCE");
                    }
                }

                // build one row per PO line for this SKU/item
                //var poLinesForItem = incomingPOs
                //    .Where(po => po.ItemNumber == itemNumber &&
                //                 po.AvailablePOLineQty.HasValue &&
                //                 po.AvailablePOLineQty.Value > 0 &&
                //                 !string.IsNullOrWhiteSpace(po.ExpectedDeliveryDate))
                //    .ToList();

                //foreach (var po in poLinesForItem)
                //{
                //    if (!DateTime.TryParse(po.ExpectedDeliveryDate, out var eta))
                //    {
                //        var msg = $"Invalid PO ETA for {itemNumber} / PO {po.PONumber}: '{po.ExpectedDeliveryDate}'";
                //        _logger.LogWarning(msg);
                //        await LogReplenishmentSync(basePartNumber, colorCode, nuOrderSizeId, null, po.AvailablePOLineQty, false, "Failed", msg);
                //        continue;
                //    }

                //    // If you want to subtract linked SO qty from the PO quantity, uncomment:
                //    // var netQty = Math.Max(0, (po.AvailablePOLineQty ?? 0) - (po.LinkedSOQty ?? 0));
                //    var netQty = po.AvailablePOLineQty ?? 0;

                //    replenishmentList.Add(new NuOrderReplenishment
                //    {
                //        sku_id = nuOrderSizeId,
                //        warehouse_id = WarehouseId,
                //        display_name = "Future",
                //        prebook = false,
                //        period_start = eta.ToString("MM/dd/yyyy"),
                //        period_end = eta.ToString("MM/dd/yyyy"),
                //        quantity = netQty,
                //        active = true
                //    });

                //    await LogReplenishmentSync(basePartNumber, colorCode, nuOrderSizeId, null, netQty, false,
                //        "Success", $"Future replenishment staged for PO {po.PONumber} (ETA {eta:MM/dd/yyyy})");
                //}

                // 5) Submit one request per color group
                if (replenishmentList.Any())
                {
                    // Delete existing replenishments per SKU once
                    var skuIds = replenishmentList
                        .Select(r => r.sku_id)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct()
                        .ToList();

                    foreach (var sku in skuIds)
                    {
                        _logger.LogInformation("Deleting existing replenishment for SKU: {Sku}", sku);
                        await _nuOrderApiService.DeleteReplenishmentAsync(WarehouseId, sku);
                    }

                    var bulkPayload = new NUORDER_REPLENISHMENT_BULK { replenishments = replenishmentList };

                    Console.Write(basePartNumber + " " + colorCode);

                    _logger.LogInformation("Submitting {Count} replenishment rows for {BasePart} / {Color}.",
                        replenishmentList.Count, basePartNumber, colorCode);

                    var requestJson = JsonSerializer.Serialize(bulkPayload, new JsonSerializerOptions { WriteIndented = false });

                    // THIS IS THE CREATE REPLENISHMENT PROCESS
                    var (result, responseBody) = await _nuOrderApiService.CreateOrUpdateReplenishmentsWithBodyAsync(WarehouseId, bulkPayload);
                    _logger.LogInformation("✅ Submitted replenishments for {BasePart} / {Color} | Result: {Result}",
                        basePartNumber, colorCode, result);

                    await LogReplenishmentSync(
                        basePartNumber,
                        colorCode,
                        "*bulk*",
                        null,
                        null,
                        false,
                        result ? "Submitted" : "Failed",
                        result ? "Bulk replenishments submitted" : "Bulk replenishments failed",
                        requestJson,
                        responseBody,
                        "VOCE");

                    // Refresh inventory flags for each product (style) represented in this color group
                    var updated = await _nuOrderApiService.UpdateInventoryFlagsAsync(productIdsForColorGroup.ToList());
                    if (updated)
                        _logger.LogInformation("✅ Inventory flags updated for {BasePart} / {Color}.", basePartNumber, colorCode);
                    else
                        _logger.LogError("❌ Failed to update inventory flags for {BasePart} / {Color}.", basePartNumber, colorCode);
                }
                else
                {
                    _logger.LogInformation("❌ No replenishments built for {BasePart} / {Color}. Skipping.", basePartNumber, colorCode);
                }
            } // each color group
        }


        // ==== Helpers / Models ====

        // Combined totals per item (variant) across the selected stores
        public class CombinedInventoryTotals
        {
            public string BasePartNumber { get; set; } = string.Empty;
            public string ItemNumber { get; set; } = string.Empty;
            public float TotalAvailableQty { get; set; }
            public float TotalATSQty { get; set; }
            public float TotalOnHandQty { get; set; }
            public float TotalQtyOnPO { get; set; }
            public float TotalATSIncPO { get; set; }
        }

        public List<CombinedInventoryTotals> AggregateInventoryByItem(string basePartNumber, List<XoroInvDatum> variants)
        {
            return variants
                .GroupBy(v => v.ItemNumber)
                .Select(g => new CombinedInventoryTotals
                {
                    BasePartNumber = basePartNumber,
                    ItemNumber = g.Key,
                    TotalAvailableQty = g.Sum(v => v.AvailableQty),
                    TotalATSQty = g.Sum(v => v.ATSQty),
                    TotalOnHandQty = g.Sum(v => v.OnHandQty),
                    TotalQtyOnPO = g.Sum(v => v.QtyOnPO),
                    TotalATSIncPO = g.Sum(v => v.ATSQtyIncPO)
                })
                .ToList();
        }

        public class IncomingPOSummary
        {
            public string ItemNumber { get; set; } = string.Empty;
            public float? AvailablePOLineQty { get; set; }
            public string? ExpectedDeliveryDate { get; set; }
            public string? PONumber { get; set; } = string.Empty;
            public float? LinkedSOQty { get; set; }
        }

        public long StringToUnix(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return -1;

            if (DateTime.TryParse(dateStr, out DateTime parsedDate))
                return new DateTimeOffset(parsedDate).ToUnixTimeMilliseconds();

            return -1;
        }

        private static DateTime? NormalizeEtaDate(string? dateStr)
        {
            if (!DateTime.TryParse(dateStr, out var etaRaw))
            {
                return null;
            }

            var eta = etaRaw.Date;
            return eta < DateTime.UtcNow.Date ? DateTime.UtcNow.Date : eta;
        }

        private async Task LogReplenishmentSync(
            string basePart,
            string color,
            string skuId,
            float? ats,
            float? poQty,
            bool prebook,
            string status,
            string message,
            string? requestJson = null,
            string? responseJson = null,
            string? client = null)
        {
            var log = new ReplenishmentSyncLog
            {
                Timestamp = DateTime.UtcNow,
                BasePartNumber = basePart,
                Color = color,
                SkuId = skuId,
                ATS = ats,
                POQty = poQty,
                Prebook = prebook,
                Status = status,
                Message = message,
                Client = client,
                RequestJson = requestJson,
                ResponseJson = responseJson
            };
            _context.ReplenishmentSyncLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
