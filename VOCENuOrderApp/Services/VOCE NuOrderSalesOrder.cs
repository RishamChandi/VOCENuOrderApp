using Hangfire;
using Microsoft.Data.SqlClient;
using VOCENuOrderApp.Models.NUORDER;
using VOCENuOrderApp.Models.XORO;
using VOCENuOrderApp.Data;

namespace VOCENuOrderApp.Services
{
    public class VOCE_NuOrderSalesOrder
    {
        private readonly NuOrderApiServiceVOCE _nuOrderApiService;
        private readonly XoroApiServiceVOCE _xoroApiService;
        private readonly ILogger<VOCE_NuOrderSalesOrder> _logger;
        private readonly ApplicationDbContext _db;

        public VOCE_NuOrderSalesOrder(NuOrderApiServiceVOCE nuOrderApiService, XoroApiServiceVOCE xoroApiService, ILogger<VOCE_NuOrderSalesOrder> logger, ApplicationDbContext db)
        {
            _nuOrderApiService = nuOrderApiService;
            _xoroApiService = xoroApiService;
            _logger = logger;
            _db = db;
        }

        // Entry point: fetch Approved orders and process each
        public async Task FetchApprovedOrdersAsync()
        {
            try
            {
                var orderIds = await _nuOrderApiService.GetOrderIdsByStatusAsync("Approved");

                if (orderIds == null || !orderIds.Any())
                {
                    _logger.LogInformation("No approved orders found in NuOrder.");
                    return;
                }

                int delaySeconds = 0;
                foreach (var orderId in orderIds)
                {
                    var scheduledDelay = TimeSpan.FromSeconds(delaySeconds);
                    BackgroundJob.Schedule(() => ProcessNuOrderToXoroAsync(orderId), scheduledDelay);
                    delaySeconds += 10; // stagger requests
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching approved orders.");
            }
        }

        // Process one NuOrder id
        public async Task ProcessNuOrderToXoroAsync(string orderId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderId))
                {
                    _logger.LogWarning("Skipped processing empty order id.");
                    return;
                }

                _logger.LogInformation("Fetching NuOrder for OrderId: {OrderId}", orderId);
                var nuOrder = await _nuOrderApiService.GetOrderByIdAsync(orderId);
                if (nuOrder == null)
                {
                    _logger.LogWarning("NuOrder not found for OrderId: {OrderId}", orderId);
                    return;
                }

                //var xoroOrderPayload = MapNuOrderToXoroSalesOrder(nuOrder);
                //if (xoroOrderPayload?.Data == null || !xoroOrderPayload.Data.Any())
                //{
                //    _logger.LogWarning("Mapped Xoro payload is empty for OrderId: {OrderId}", orderId);
                //    return;
                //}

                var prevStatus = nuOrder.status ?? string.Empty;
                var xoroOrder = MapNuOrderToXoroSalesOrder(nuOrder);

                _logger.LogInformation("Posting Sales Order to Xoro. OrderNumber: {OrderNumber}", nuOrder.order_number);
                var result = await _xoroApiService.PostXoroSalesOrderAsync(xoroOrder);

                if (result)
                {
                    _logger.LogInformation("Xoro Sales Order created for NuOrder {OrderNumber}", nuOrder.order_number);
                    var targetStatus = "processed";
                    var res = await _nuOrderApiService.UpdateOrderStatusByNumberPathWithResultAsync(nuOrder.order_number!, targetStatus);
                    var updateResult = res.Success;
                    var endpointUsed = "number/path";
                    if (!updateResult)
                    {
                        // Fallback by ID if number path failed
                        var idResSuccess = await _nuOrderApiService.UpdateOrderStatusByNumberPathAsync(orderId, targetStatus);
                        endpointUsed = "id/path";
                        updateResult = idResSuccess;
                    }
                    _db.OrderStatusSyncLogs.Add(new OrderStatusSyncLog
                    {
                        OrderNumber = nuOrder.order_number ?? string.Empty,
                        NuOrderInternalId = orderId,
                        PreviousStatus = prevStatus,
                        TargetStatus = targetStatus,
                        Success = updateResult,
                        EndpointUsed = endpointUsed,
                        ErrorMessage = updateResult ? string.Empty : "Status update failed after Xoro success",
                        RequestUrl = res.RequestUrl,
                        RequestJson = string.Empty,
                        ResponseBody = res.ResponseBody,
                        ResponseStatusCode = res.StatusCode,
                        Client = "VOCE"
                    });
                    await _db.SaveChangesAsync();
                }
                else
                {
                    _logger.LogError("Failed to create Xoro Sales Order for NuOrder {OrderNumber}", nuOrder.order_number);
                    _db.OrderStatusSyncLogs.Add(new OrderStatusSyncLog
                    {
                        OrderNumber = nuOrder.order_number ?? string.Empty,
                        NuOrderInternalId = orderId,
                        PreviousStatus = prevStatus,
                        TargetStatus = "processed",
                        Success = false,
                        EndpointUsed = "number/path",
                        ErrorMessage = "Xoro Sales Order create failed",
                        RequestUrl = string.Empty,
                        RequestJson = string.Empty,
                        ResponseBody = string.Empty,
                        ResponseStatusCode = string.Empty,
                        Client = "VOCE"
                    });
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing OrderId {OrderId}", orderId);
                _db.OrderStatusSyncLogs.Add(new OrderStatusSyncLog
                {
                    OrderNumber = orderId,
                    NuOrderInternalId = orderId,
                    PreviousStatus = string.Empty,
                    TargetStatus = "processed",
                    Success = false,
                    EndpointUsed = "number/path",
                    ErrorMessage = ex.Message,
                    Client = "VOCE"
                });
                await _db.SaveChangesAsync();
            }
        }

        private XoroOrder MapNuOrderToXoroSalesOrder(NuOrderSORootobject nuOrder)
        {
            if (nuOrder == null) throw new ArgumentNullException(nameof(nuOrder));

            var header = new Soestimateheader
            {
                ThirdPartyRefNo = nuOrder.order_number,
                ThirdPartyRefName = nuOrder.order_number,
                ThirdPartySource = "NuOrder API",
                ThirdPartyDisplayName = "NuOrder",
                ThirdPartyIconUrl = "https://cdn2.xorosoft.io/Resources/Images/nuorder.png",

                RefNo = nuOrder.order_number,
                OrderDate = (nuOrder.created_on == default ? DateTime.UtcNow : nuOrder.created_on).ToString("MM/dd/yyyy"),
                DateToBeShipped = (nuOrder.start_ship == default ? DateTime.UtcNow : nuOrder.start_ship).ToString("MM/dd/yyyy"),
                DateToBeCancelled = (nuOrder.end_ship == default ? DateTime.UtcNow.AddMonths(1) : nuOrder.end_ship).ToString("MM/dd/yyyy"),

                CustomerName = nuOrder.retailer?.retailer_name ?? "",
                CustomerPO = nuOrder.customer_po_number ?? "",
                CurrencyCode = nuOrder.currency_code ?? "USD",

                SaleStoreName = "Main Warehouse",
                StoreName = "Main Warehouse",
                OrderTypeName = MapOrderType(nuOrder.order_type),

                ReCalcTaxesFlag = true,
                ReCalculateTaxes = true,
                ReCalcShippingTaxesFlag = true,

                ShipMethodName = "Delivery (Third Party)",
                CarrierName = "SH SHOP RATES",
                ShipServiceName = "Ground",
                ShippingTermsName = "Prepaid & Billed",
                SalesRepId = !string.IsNullOrWhiteSpace(nuOrder.rep_code) ? nuOrder.rep_code : (nuOrder.rep_name ?? ""),

                CustomerMessage = nuOrder.notes ?? string.Empty,
                Memo = "", //nuOrder.notes ?? string.Empty,
                Tags = "NuOrder",

                // Bill To
                BillToAddr = nuOrder.billing_address?.line_1 ?? "",
                BillToCity = nuOrder.billing_address?.city ?? "",
                BillToState = nuOrder.billing_address?.state ?? "",
                BillToCountry = nuOrder.billing_address?.country ?? "",
                BillToZpCode = (nuOrder.billing_address as Billing_AddressNuOrder)?.zip ?? "",
                BillToFirstName = nuOrder.retailer?.buyer_name ?? "",
                BillToEmail = nuOrder.retailer?.buyer_email ?? "",

                // Ship To
                ShipToAddr = nuOrder.shipping_address?.line_1 ?? nuOrder.billing_address?.line_1 ?? "",
                ShipToCity = nuOrder.shipping_address?.city ?? nuOrder.billing_address?.city ?? "",
                ShipToState = nuOrder.shipping_address?.state ?? nuOrder.billing_address?.state ?? "",
                ShipToCountry = nuOrder.shipping_address?.country ?? nuOrder.billing_address?.country ?? "",
                ShipToZpCode = (nuOrder.shipping_address as Shipping_AddressNuOrder)?.zip ?? (nuOrder.billing_address as Billing_AddressNuOrder)?.zip ?? "",
                ShipToFirstName = nuOrder.retailer?.buyer_name ?? "",
                ShipToEmail = nuOrder.retailer?.buyer_email ?? "",
            };

            var lineArr = new List<Soestimateitemlinearr>();
            if (nuOrder.line_items != null)
            {
                foreach (var li in nuOrder.line_items)
                {
                    if (li?.sizes == null || li.sizes.Length == 0) continue;

                    foreach (var sz in li.sizes)
                    {
                        if (sz == null || sz.quantity <= 0) continue;

                        var itemNumber = ComposeXoroItemNumber(li.product?.style_number, li.product?.color_code, sz.size);
                        var desc = $"{li.product?.style_number}-{li.product?.color}-{sz.size}".Trim('-');
                        var upc = sz.upc;
                        header.BrandName = li.product.division;

                        lineArr.Add(new Soestimateitemlinearr
                        {
                            ItemTypeName = "Inventory",
                            ItemNumber = upc, //itemNumber,
                            ItemIdentifierCode = 2,
                            Description = desc,
                            UnitPrice = sz.original_price, //sz.price,
                            Qty = sz.quantity,
                            DateToBeShipped = (li.ship_start == default ? header.DateToBeShipped : li.ship_start.ToString("MM/dd/yyyy")),
                            Notes = desc,
                            ThirdPartyRefNo2 = nuOrder.order_number,
                            Discount = nuOrder.discount, 
                            DiscountTypeName = "percentage"  //Hardcoded to percentage discount
                        });
                    }
                }
            }

            var xoroOrder = new XoroOrder
            {
                SoEstimateHeader = header,
                SoEstimateItemLineArr = lineArr
            };

            var payload = new XoroSalesOrder
            {
                ConfirmFlag = false,
                ConfirmValue = null,
                ConfirmValueData = null,
                Data = new List<XoroOrder> { xoroOrder },
                EndRequest = true,
                ErrorCode = 0,
                IsForwardRequest = false,
                LogFlag = true,
                Message = string.Empty,
                MessageTitle = null,
                MetaData = string.Empty,
                Result = true
            };

            return xoroOrder;
        }


        // Order Type Mapping Function
        public static string MapOrderType(string orderType)
        {
            var orderTypeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Immediate Order", "Immediate" },
                { "Booking", "Booking" }
                // Add more mappings as needed
            };

            return orderTypeMap.TryGetValue(orderType, out var mappedValue) ? mappedValue : "Unknown";
        }


        private static string ComposeXoroItemNumber(string? styleNumber, string? colorCode, string? size)
        {
            styleNumber ??= string.Empty;
            var cc = string.IsNullOrWhiteSpace(colorCode) ? DeriveColorCodeFromName(null) : colorCode.ToUpperInvariant();
            var sc = MapSizeToCode(size);
            if (string.IsNullOrWhiteSpace(styleNumber) || string.IsNullOrWhiteSpace(cc) || string.IsNullOrWhiteSpace(sc))
                return styleNumber; // fallback best effort
            return $"{styleNumber}-{cc}-{sc}";
        }

        private static string DeriveColorCodeFromName(string? colorName)
        {
            if (string.IsNullOrWhiteSpace(colorName)) return string.Empty;
            // fallback: take first letters of words up to 3 chars, e.g. "Bright White" => BWH
            var parts = colorName.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length == 1)
            {
                return parts[0].Length <= 3 ? parts[0].ToUpperInvariant() : parts[0].Substring(0, 3).ToUpperInvariant();
            }
            return string.Join(string.Empty, parts.Select(p => p[0])).ToUpperInvariant();
        }

        private static string MapSizeToCode(string? size)
        {
            if (string.IsNullOrWhiteSpace(size)) return string.Empty;
            return size.Trim().ToLowerInvariant() switch
            {
                "xs" or "x small" or "x-small" or "extra small" => "XS",
                "s" or "small" => "S",
                "m" or "medium" => "M",
                "l" or "large" => "L",
                "xl" or "x large" or "x-large" or "extra large" => "XL",
                "xxl" or "2xl" or "xx large" or "xx-large" => "XXL",
                _ => size.ToUpperInvariant()
            };
        }
    }
}