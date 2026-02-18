using Hangfire;
using VOCENuOrderApp.Data;
using VOCENuOrderApp.Models;
using VOCENuOrderApp.Models.NUORDER;
using VOCENuOrderApp.Models.XoroModels;
using VOCENuOrderApp.Utilities;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace VOCENuOrderApp.Services
{
    public class VOCE_NuOrderProductSync
    {
        private readonly XoroApiServiceVOCE _xoroApiService;
        // Database context
        private readonly ApplicationDbContext _context;
        private readonly VOCE_NuOrderReplenishmentSync _productReplenSync;
        private readonly IBackgroundJobClient _backgroundJobs;

        // Fix for CS0103: The name 'xoroApiService' does not exist in the current context
        // Update the constructor to accept XoroApiService as a parameter and assign it to the field.

        public VOCE_NuOrderProductSync(
            ApplicationDbContext context,
            VOCE_NuOrderReplenishmentSync productReplenSync,
            XoroApiServiceVOCE xoroApiServiceVOCE,
            IBackgroundJobClient backgroundJobs
        )
        {
            _xoroApiService = xoroApiServiceVOCE; // <-- Now this assignment is valid
            _context = context;
            _productReplenSync = productReplenSync;
            _backgroundJobs = backgroundJobs;
        }

        public async Task VOCE_GetXoroProductList()
        {
            try
            {
                /////Here you will call you api
                int sec = DateTime.Now.Second;
                int min = DateTime.Now.Minute;
                int hour = DateTime.Now.Hour;
                int day = DateTime.Now.Day;
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;

                string from_sec, to_sec, from_min, to_min, from_hour, to_hour, from_day, to_day, from_month, to_month, from_year, to_year;

                DateTime original = new DateTime(year, month, day, hour, min, sec);
                //original = original.Add(new TimeSpan(3, 0, 0)); // For Eastern Time zone systems
                DateTime updated = original.Add(new TimeSpan(0, -10, 0)); // hour, min, sec - how much time you want to take back

                from_year = CheckDigit(updated.Year);
                from_month = CheckDigit(updated.Month);
                from_day = CheckDigit(updated.Day);
                from_hour = CheckDigit(updated.Hour);
                from_min = CheckDigit(updated.Minute);
                from_sec = CheckDigit(updated.Second);

                to_year = CheckDigit(original.Year);
                to_month = CheckDigit(original.Month);
                to_day = CheckDigit(original.Day);
                to_hour = CheckDigit(original.Hour);
                to_min = CheckDigit(original.Minute);
                to_sec = CheckDigit(original.Second);

                string from_date = from_year + "-" + from_month + "-" + from_day + "T" + from_hour + ":" + from_min + ":" + from_sec;  // "2022-05-24T11:50:00";
                string to_date = to_year + "-" + to_month + "-" + to_day + "T" + to_hour + ":" + to_min + ":" + to_sec;  // "2022-05-24T11:55:00";

                //string from_date = "2022-05-28T13:01:00";
                //string to_date = "2022-05-28T17:59:00";

                await VOCE_GetProductsXoro(from_date, to_date);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // NEW: CSV/Pipe list of base parts -> fetch products by base part and sync to NuORDER
        // Endpoint used: /api/xerp/product/getproduct?base_part_number={basepart}&tag={tags}&page={page}
        public Task VOCE_GetXoroProductList_CSVLIST(string basePartsCsvOrPipe)
        {
            if (string.IsNullOrWhiteSpace(basePartsCsvOrPipe))
            {
                return Task.CompletedTask;
            }

            var baseParts = System.Text.RegularExpressions.Regex
                .Split(basePartsCsvOrPipe, @"[|,]")
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (!baseParts.Any())
            {
                Console.WriteLine("⚠️ No valid base parts were supplied for scheduling.");
                return Task.CompletedTask;
            }

            var minuteOffset = 1;

            foreach (var basePart in baseParts)
            {
                try
                {
                    var delay = TimeSpan.FromMinutes(minuteOffset);
                    _backgroundJobs.Schedule<VOCE_NuOrderProductSync>(
                        svc => svc.VOCE_GetProductsByBasePart(basePart),
                        delay);

                    Console.WriteLine($"⏱ Scheduled {basePart} for sync in {delay.TotalMinutes} minute(s).");
                    minuteOffset++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Failed to schedule base part {basePart}: {ex.Message}");
                }
            }

            return Task.CompletedTask;
        }


        // Method to Get updated Products from one XORO Instance - GET PRODUCT REQUEST XORO
        public async Task VOCE_GetProductsXoro(string frm_dt, string to_dt)
        {
            try
            {

                HttpClient _httpClient = new HttpClient();

                //Client Voce Enterprises
                var clientId = "5e232c5ebed443dfbb4ed4cc5579b13a";
                var clientSecret = "3fc2016c2ceb4cac86e647124b812d79";

                var authenticationString = $"{clientId}:{clientSecret}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

                _httpClient.BaseAddress = new Uri("https://res.xorosoft.io");
                _httpClient.Timeout = TimeSpan.FromSeconds(30);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var tags = "B2B";
                // 🔁 START PAGINATION
                int page = 1;
                bool morePages = true;
                int pageSize = 100; // 🆕 adjust this if Xoro returns fewer/more than 100

                while (morePages)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"/api/xerp/product/getproduct?updated_at_min={frm_dt}&updated_at_max={to_dt}&tag={tags}&page={page}");

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

                    XoroGetProductRoot xoroProducts = JsonSerializer.Deserialize<XoroGetProductRoot>(content, settings);
                    // 🔁 END PAGINATION

                    if (xoroProducts.Data?.Count > 0)
                    {
                        int i = 1;

                        foreach (var BaseProductRecord in xoroProducts.Data)
                        {
                            // ✅ Skip if ModifiedBy = "NUORDER_APP" or contains "API"
                            if (!string.IsNullOrEmpty(BaseProductRecord.ModifySource) &&
                                BaseProductRecord.ModifySource.Contains("NuOrder App (API)", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"⏩ Skipping product {BaseProductRecord.BasePartNumber} — Modified by NuOrder App (API).");
                                continue;
                            }

                            var mergedProduct = BaseProductRecord;

                            var colorOptions = BaseProductRecord.Options?
                                .Where(opt => opt.Name.Equals("Color", StringComparison.OrdinalIgnoreCase) ||
                                              opt.Name.Equals("Colour", StringComparison.OrdinalIgnoreCase))
                                .SelectMany(opt => opt.Values)
                                .ToList();

                            if (colorOptions == null || !colorOptions.Any())
                                continue;

                            foreach (var optionValue in colorOptions)
                            {
                                if (!string.IsNullOrEmpty(optionValue.Name))
                                {
                                    // ✅ Check for "B2B" tag in variants of this color
                                    bool hasB2BTaggedVariant = BaseProductRecord.Variants
                                    .Any(v => v.Option1Value == optionValue.Name
                                     && !string.IsNullOrWhiteSpace(v.Tags)
                                     && v.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                    .Any(t => t.Equals("B2B", StringComparison.OrdinalIgnoreCase)));

                                    if (!hasB2BTaggedVariant)
                                    {
                                        string skipMessage = $"Skipped color '{optionValue.Name}' for '{BaseProductRecord.BasePartNumber}' — No variant tagged with 'B2B'.";
                                        Console.WriteLine($"⏩ {skipMessage}");

                                        // ✅ Log skipped case as "Skipped"
                                        await LogProductSyncStatus(
                                            BaseProductRecord.BasePartNumber,
                                            optionValue.Name,
                                            "Skipped",
                                            skipMessage,
                                            "Voce Ent"
                                        );

                                        continue; // ❌ Skip this color if no variant has the "NUORDER" tag
                                    }

                                    //only run for certain color and style check 
                                    //if (optionValue.Code == "FNY" && BaseProductRecord.BasePartNumber == "STC-251-7012")
                                    //{

                                    //Console.WriteLine(colorName);
                                    Console.WriteLine($"BasePart: {BaseProductRecord.BasePartNumber} | Color: {optionValue.Code}");

                                    // ✅ Proceed with product + color sync
                                    var updatedColorProduct = await VOCE_NuOrder_CreateProduct(BaseProductRecord, optionValue.Name, optionValue.Code); // Instead of Color Name we will send Color Code for Brand id

                                    // Log the sync status in SQL Server
                                    //string syncStatus = (updatedColorProduct != null) ? "Success" : "Failed";
                                    //string message = (updatedColorProduct != null) ? "Product synced successfully." : $"Error syncing color {optionValue.Name}";

                                    //await LogProductSyncStatus(BaseProductRecord.BasePartNumber, optionValue.Name, syncStatus, message, "PaperLabel");

                                    if (updatedColorProduct?.Variants == null)
                                    {
                                        Console.WriteLine($"⚠️ UpdatedColorProduct or its Variants is null for color {optionValue.Name} and product {BaseProductRecord.BasePartNumber}. Skipping...");
                                        continue;
                                    }

                                    if (updatedColorProduct != null)
                                    {
                                        Console.WriteLine($"✅ Synced color {optionValue.Name} for product {BaseProductRecord.BasePartNumber}.");
                                        await LogProductSyncStatus(
                                            BaseProductRecord.BasePartNumber,
                                            optionValue.Name,
                                            "Success",
                                            "Product synced successfully.",
                                            "Voce Ent"
                                        );
                                    }
                                    else
                                    {
                                        Console.WriteLine($"⚠️ Skipped color {optionValue.Name} for product {BaseProductRecord.BasePartNumber} due to error.");
                                        await LogProductSyncStatus(
                                            BaseProductRecord.BasePartNumber,
                                            optionValue.Name,
                                            "Error",
                                            "Product is null.",
                                            "Voce Ent"
                                        );
                                        continue;
                                    }

                                    // Merge back the NuORDER Size ID (_id) from returned product
                                    //foreach (var updatedVariant in updatedColorProduct.Variants)
                                    //{
                                    //    var matching = mergedProduct.Variants.FirstOrDefault(v => v.ItemNumber == updatedVariant.ItemNumber);
                                    //    if (matching != null)
                                    //    {
                                    //        matching.CustomFieldH3 = updatedVariant.CustomFieldH3; // NuOrder Size Id
                                    //        matching.CustomFieldH12 = updatedVariant.CustomFieldH12; // NuOrder Style Id
                                    //    }
                                    //}

                                    foreach (var updatedVariant in updatedColorProduct.Variants)
                                    {
                                        var matching = mergedProduct.Variants.FirstOrDefault(v => v.ItemNumber == updatedVariant.ItemNumber);
                                        if (matching != null)
                                        {
                                            matching.CustomFieldH2 = updatedVariant.CustomFieldH2;  // style-color id
                                            matching.CustomFieldH3 = updatedVariant.CustomFieldH3;  // size _id
                                            matching.CustomFieldH12 = updatedVariant.CustomFieldH12; // style _id
                                        }
                                    }


                                    //} // End of the color if 
                                    //}// END OF THE IF LOOP FOR COLOR CODE
                                }
                            }

                            if (mergedProduct.Variants.Any(v =>
                            !string.IsNullOrWhiteSpace(v.CustomFieldH2)  // brand_id (style-color)
                            || !string.IsNullOrWhiteSpace(v.CustomFieldH3)  // size _id
                            || !string.IsNullOrWhiteSpace(v.CustomFieldH12) // style _id
                            ))
                            {
                                // Fetch the latest product from Xoro before updating
                                var latestProduct = await _xoroApiService.GetProductByBasePartAsync(mergedProduct.BasePartNumber);
                                if (latestProduct != null)
                                {
                                    // First preserve critical fields like season, upc or option codes
                                    PreserveCriticalFields(mergedProduct, latestProduct);
                                }
                                else
                                {
                                    // Fallback to previous logic if fetch fails
                                    PreserveCriticalFields(mergedProduct, BaseProductRecord);
                                }
                                // Then update xoro with complete product and nuorder id's
                                //await UpdateXoroProductWithVariants(mergedProduct);
                                // Use raw JSON round-trip patch to preserve schema and only set NuOrder IDs
                                await XoroRoundTripPatchNuOrderIdsAsync(mergedProduct);
                            }
                            else
                            {
                                Console.WriteLine($"❌ Skipping Xoro update for {mergedProduct.BasePartNumber}, no updated variants.");
                            }

                            // Make the inventory request 
                            await _productReplenSync.VOCE_NuOrder_Replen_CSVLIST(mergedProduct.BasePartNumber);


                        }

                        // 🔁 START: Pagination Progress
                        morePages = xoroProducts.Data.Count == pageSize; // continue if full page returned
                        page++; // move to next page
                        await Task.Delay(500); // ⏳ Throttle: wait 500ms to respect API rate limits
                        // 🔁 END
                    }
                    else
                    {
                        Console.WriteLine("Could not fetch products !");
                        // 🔁 START: Pagination Exit
                        morePages = false; // stop looping
                        Console.WriteLine("✅ All products processed or no data returned.");
                        // 🔁 END
                    }
                }// 🧠 end while
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // NEW: Get products from XORO by base part number (paginated) and sync to NuORDER
        public async Task VOCE_GetProductsByBasePart(string basePartNumber)
        {
            try
            {
                using var _httpClient = new HttpClient();

                // Client Voce Enterprises
                var clientId = "5e232c5ebed443dfbb4ed4cc5579b13a";
                var clientSecret = "3fc2016c2ceb4cac86e647124b812d79";

                var authenticationString = $"{clientId}:{clientSecret}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

                _httpClient.BaseAddress = new Uri("https://res.xorosoft.io");
                _httpClient.Timeout = TimeSpan.FromSeconds(30);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var tags = "B2B";
                int page = 1;
                bool morePages = true;
                int pageSize = 100;

                while (morePages)
                {
                    var url = $"/api/xerp/product/getproduct?base_part_number={Uri.EscapeDataString(basePartNumber)}&tag={tags}&page={page}";
                    using var request = new HttpRequestMessage(HttpMethod.Get, url);
                    using var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                    var xoroProducts = JsonSerializer.Deserialize<XoroGetProductRoot>(content, settings);

                    if (xoroProducts?.Data != null && xoroProducts.Data.Count > 0)
                    {
                        foreach (var baseProductRecord in xoroProducts.Data)
                        {
                            await ProcessVoceProductRecord(baseProductRecord);
                        }

                        morePages = xoroProducts.Data.Count == pageSize; // crude page check
                        page++;
                        await Task.Delay(300);
                    }
                    else
                    {
                        morePages = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ VOCE_GetProductsByBasePart error for {basePartNumber}: {ex.Message}");
            }
        }

        // Helper: process a single base product record (mirrors existing date-range logic)
        private async Task ProcessVoceProductRecord(XoroProductDatum BaseProductRecord)
        {
            // ✅ Skip if ModifiedBy = "NuOrder App (API)"
            if (!string.IsNullOrEmpty(BaseProductRecord.ModifySource) &&
                BaseProductRecord.ModifySource.Contains("NuOrder App (API)", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"⏩ Skipping product {BaseProductRecord.BasePartNumber} — Modified by NuOrder App (API).");
                return;
            }

            var colorOptions = BaseProductRecord.Options?
                .Where(opt => opt.Name.Equals("Color", StringComparison.OrdinalIgnoreCase) ||
                              opt.Name.Equals("Colour", StringComparison.OrdinalIgnoreCase))
                .SelectMany(opt => opt.Values)
                .ToList();

            if (colorOptions == null || !colorOptions.Any()) return;

            foreach (var optionValue in colorOptions)
            {
                if (string.IsNullOrEmpty(optionValue.Name)) continue;

                // ✅ Only process colors having a variant tagged B2B
                bool hasB2BTaggedVariant = BaseProductRecord.Variants
                    .Any(v => v.Option1Value == optionValue.Name &&
                              !string.IsNullOrWhiteSpace(v.Tags) &&
                              v.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                   .Any(t => t.Equals("B2B", StringComparison.OrdinalIgnoreCase)));

                if (!hasB2BTaggedVariant)
                {
                    string skipMessage = $"Skipped color '{optionValue.Name}' for '{BaseProductRecord.BasePartNumber}' — No variant tagged with 'B2B'.";
                    Console.WriteLine($"⏩ {skipMessage}");
                    await LogProductSyncStatus(BaseProductRecord.BasePartNumber, optionValue.Name, "Skipped", skipMessage, "Voce Ent");
                    continue;
                }

                Console.WriteLine($"BasePart: {BaseProductRecord.BasePartNumber} | Color: {optionValue.Code}");

                var updatedColorProduct = await VOCE_NuOrder_CreateProduct(BaseProductRecord, optionValue.Name, optionValue.Code);
                if (updatedColorProduct == null || updatedColorProduct.Variants == null)
                {
                    Console.WriteLine($"⚠️ Skipped color {optionValue.Name} for product {BaseProductRecord.BasePartNumber} due to error.");
                    await LogProductSyncStatus(BaseProductRecord.BasePartNumber, optionValue.Name, "Error", "Product is null.", "Voce Ent");
                    continue;
                }

                Console.WriteLine($"✅ Synced color {optionValue.Name} for product {BaseProductRecord.BasePartNumber}.");
                await LogProductSyncStatus(BaseProductRecord.BasePartNumber, optionValue.Name, "Success", "Product synced successfully.", "Voce Ent");

                // Persist the returned NuORDER IDs in Xoro
                var mergedProduct = BaseProductRecord;
                foreach (var updatedVariant in updatedColorProduct.Variants)
                {
                    var matching = mergedProduct.Variants.FirstOrDefault(v => v.ItemNumber == updatedVariant.ItemNumber);
                    if (matching != null)
                    {
                        matching.CustomFieldH2 = updatedVariant.CustomFieldH2;
                        matching.CustomFieldH3 = updatedVariant.CustomFieldH3;
                        matching.CustomFieldH12 = updatedVariant.CustomFieldH12;
                    }
                }

                var latest = await _xoroApiService.GetProductByBasePartAsync(mergedProduct.BasePartNumber);
                if (latest != null) PreserveCriticalFields(mergedProduct, latest);
                else PreserveCriticalFields(mergedProduct, BaseProductRecord);

                await XoroRoundTripPatchNuOrderIdsAsync(mergedProduct);

                // Kick replenishment for this base part
                await _productReplenSync.VOCE_NuOrder_Replen_CSVLIST(mergedProduct.BasePartNumber);
            }
        }



        //Method to POST Products to selected Xoro Instance - https://voce-ent.xoro.one/
        public async Task<XoroProductDatum?> VOCE_NuOrder_CreateProduct(XoroProductDatum XoroProductObj, string ColorName, string ColorCode) // Sending Color Code for Brand id
        {
            HttpClient _httpClient1 = new HttpClient();

            // Client Voce Production
            NuOrderConfig config = new NuOrderConfig();
            config.ConsumerKey = "EYzMg2mDv9Paz9TKsaPs52Bf";    // Retrieve this value from NuORDER > Admin > API Management
            config.ConsumerSecret = "JfMbNWa54xBYQ8RJnyyhjy4eEXkMMfXfmMVNFYscw6nCNzr7vH8afN2JSwGStp3m"; // Retrieve this value from NuORDER > Admin > API Management
            config.Token = "MAbs9sDHF952gdSfEtBGA3vM";          // Retrieve this value from NuORDER > Admin > API Management, or the token response body
            config.TokenSecret = "2kxA5fh9Tc68qbJ3ucPtrmxW2Bss9fENnab5mN7gN7DQ9NuJDpRKvr2mpZYZvYWB";    // Retrieve this value from NuORDER > Admin > API Management, or the token response body
            config.Version = "1.0";
            config.SignatureMethod = "HMAC-SHA1";

            NuOrderWebService ws = new NuOrderWebService(config);

            //string url = "https://sandbox1.nuorder.com/api/product/new/force"; // sandbox 
            string url = "https://next.nuorder.com/api/product/new/force"; // production 

            const int maxRetries = 3;
            int delayMs = 1000;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                var error_message = "";

                try
                {
                    var NuProduct = MakeJsonObject_NuOrderProduct(XoroProductObj, ColorName, ColorCode, "Voce"); // Sending Color Code for Brand id

                    // Log if the product is null
                    if (NuProduct == null)
                    {
                        string failMsg = $"Failed to generate NuOrder product object for {ColorName} ({XoroProductObj.BasePartNumber}).";
                        Console.WriteLine($"⚠️ {failMsg}");

                        await LogProductSyncStatus(
                            XoroProductObj.BasePartNumber,
                            ColorName,
                            "Failed",
                            failMsg,
                            "Voce Ent"
                        );

                        return null;
                    }

                    var serializedProduct = JsonSerializer.Serialize(NuProduct);

                    // Submitted log
                    await LogProductSyncStatus(
                        XoroProductObj.BasePartNumber,
                        ColorName,
                        "Submitted",
                        "Product sync request submitted",
                        "Voce Ent",
                        serializedProduct,
                        null
                    );

                    //using var httpClient = new HttpClient { BaseAddress = new Uri("https://sandbox1.nuorder.com/") }; // sandbox
                    using var httpClient = new HttpClient { BaseAddress = new Uri("https://next.nuorder.com/") };  // production

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GenerateOAuthHeader(config, HttpMethod.Put, url));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                    using var response = await httpClient.PutAsync(url, content);

                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseData);
                    Console.WriteLine($"Attempt {attempt} Response: {response.StatusCode} - {responseData}"); // Logging should happen here

                    if (response.IsSuccessStatusCode)
                    {
                        var NuProductResponse = JsonSerializer.Deserialize<SAMPLE_NUORDER_PRODUCT_UPDATE_RESPONSE>(responseData, new JsonSerializerOptions
                        {
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        });

                        error_message = NuProductResponse?.message ?? "No Message";

                        string nuOrderProductId = NuProductResponse?.brand_id ?? "Unknown";

                        await LogProductSyncStatus(
                            XoroProductObj.BasePartNumber,
                            ColorName,
                            "Success",
                            $"Product synced successfully to NuOrder. NuOrder Brand ID: {nuOrderProductId} + Message: {error_message}",
                            "Voce Ent",
                            serializedProduct,
                            responseData
                        );

                        if (NuProductResponse != null)
                        {
                            // Style-level IDs: apply to every variant of the selected color
                            foreach (var v in XoroProductObj.Variants.Where(v => v.Option1Value == ColorName))
                            {
                                v.CustomFieldH2 = NuProductResponse.brand_id; // style-color key you chose brand id
                                v.CustomFieldH12 = NuProductResponse._id;      // NuORDER product (style) _id
                            }

                            // Size-level IDs: map by UPC when available
                            if (NuProductResponse.sizes != null && NuProductResponse.sizes.Any())
                            {
                                var map = NuProductResponse.sizes
                                    .Where(s => !string.IsNullOrWhiteSpace(s.upc))
                                    .ToDictionary(s => s.upc, s => s._id);

                                foreach (var v in XoroProductObj.Variants.Where(v => v.Option1Value == ColorName))
                                {
                                    if (!string.IsNullOrWhiteSpace(v.ItemUpc) && map.TryGetValue(v.ItemUpc, out var sizeId))
                                        v.CustomFieldH3 = sizeId; // NuORDER size _id
                                }
                            }
                        }

                        //if (NuProductResponse?.sizes != null && NuProductResponse.sizes.Any())
                        //{
                        //    foreach (var size in NuProductResponse.sizes)
                        //    {
                        //        var matchingVariant = XoroProductObj.Variants.FirstOrDefault(v => v.ItemUpc == size.upc);
                        //        if (matchingVariant != null)
                        //        {
                        //            matchingVariant.CustomFieldH2 = NuProductResponse.brand_id;
                        //            matchingVariant.CustomFieldH12 = NuProductResponse._id; // ✅ Save NuORDER Product ID in CustomFieldH12
                        //            matchingVariant.CustomFieldH3 = size._id; // ✅ Save NuORDER Size ID in CustomFieldH3
                        //        }
                        //    }
                        //}

                        return XoroProductObj;
                    }
                    else if ((int)response.StatusCode == 429 || (int)response.StatusCode >= 500)
                    {
                        Console.WriteLine($"🔁 Retryable error ({(int)response.StatusCode}). Retrying...");
                        await Task.Delay(delayMs * attempt); // Exponential backoff
                    }
                    else
                    {
                        try
                        {
                            var errorObj = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData);
                            error_message = errorObj.ContainsKey("message") ? errorObj["message"]?.ToString() : responseData;
                        }
                        catch
                        {
                            error_message = responseData;
                        }

                        // Logging should happen here
                        // ❌ NuOrder rejected the product — log full details
                        string failMsg = $"NuOrder Error ({(int)response.StatusCode}): {responseData} and {error_message}";

                        Console.WriteLine($"❌ {failMsg}");

                        await LogProductSyncStatus(
                            XoroProductObj.BasePartNumber,
                            ColorName,
                            "Failed",
                            failMsg,
                            "Voce Ent",
                            serializedProduct,
                            responseData
                        );

                        return null; // ⚠️ Return null to prevent falsely treating as success
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = $"❌ Exception in VOCE_NuOrder_CreateProduct for {XoroProductObj.BasePartNumber} ({ColorName}): {ex.Message} NuOrder Response : {error_message}";
                    Console.WriteLine(errorMsg);

                    await LogProductSyncStatus(
                        XoroProductObj.BasePartNumber,
                        ColorName,
                        "Failed",
                        errorMsg,
                        "Voce Ent",
                        null,
                        null
                    );

                    return null;
                }
            }

            // Log failed sync after retries
            await LogProductSyncStatus(XoroProductObj.BasePartNumber, ColorName, "Failed", "All retry attempts failed", "Voce Ent", null, null);
            return null;
        }


        // Function to Map Xoro Product to NuOrder Style mapping
        public NUORDER_PRODUCT_UPDATE_VOCE MakeJsonObject_NuOrderProduct(XoroProductDatum XoroProductObj, string ColorName, string ColorCode, string Instance)
        {
            try
            {
                if (XoroProductObj.Variants == null || !XoroProductObj.Variants.Any())
                {
                    Console.WriteLine("No variants found for the product.");
                    return null;
                }

                // Use UTC dates in NuORDER's yyyy-MM-dd format
                var todayStr = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
                var plus30Str = DateTime.UtcNow.Date.AddDays(30).ToString("yyyy-MM-dd");

                // Extract season values and select the first one if multiple exist
                var firstseason = !string.IsNullOrEmpty(XoroProductObj.Variants[0].SeasonValues)
                    ? XoroProductObj.Variants[0].SeasonValues.Split(',')[0].Trim() // Take first season value
                    : string.Empty;

                var firstVariant = XoroProductObj.Variants.FirstOrDefault(v => v.Option1Value == ColorName);

                string descriptionName = firstVariant?.Description ?? XoroProductObj.Title;

                // Create NuOrder Create/Update Product Object
                NUORDER_PRODUCT_UPDATE_VOCE NuProduct = new NUORDER_PRODUCT_UPDATE_VOCE
                {
                    style_number = XoroProductObj.BasePartNumber,
                    season = firstseason,
                    color = ColorName,  // Color Name
                    name = descriptionName, // Style Name or item description
                    brand_id = XoroProductObj.BasePartNumber + "_" + ColorCode,    // Color Code instead of Color Name  - CTR-212-122_GREY MIX_SS22  - + "_" + firstseason
                    unique_key = "",
                    schema_id = "",
                    sizes = new List<Size_VOCE>(),

                    // Custom Fields
                    category = XoroProductObj.ProductCategoryName,
                    available_now = true,
                    archived = false,
                    active = XoroProductObj.ActiveFlag // Product Active Flag
                    //cancelled = !XoroProductObj.ActiveFlag;
                };

                XoroProductObj.Variants.ForEach(delegate (ProductVariant XoroProductVariant)
                {
                    if (XoroProductVariant.Option1Value == ColorName) // Correct check
                    {
                        var combinedSize = BuildCombinedSize(XoroProductVariant);

                        // Only add sizes when there is something to send
                        if (!string.IsNullOrWhiteSpace(combinedSize))
                        {
                            var sz = new Size_VOCE
                            {
                                size = combinedSize,                           // <-- key line (e.g., "2B", "2.5B")
                                upc = XoroProductVariant.ItemUpc,              // keep your UPC mapping
                                xoro_item_number = XoroProductVariant.ItemNumber,      // keep your SKU mapping
                            };

                            NuProduct.sizes.Add(sz);
                        }

                        //if (XoroProductVariant.Option2Name == "Size")
                        //{
                        //    Size_VOCE sz = new Size_VOCE
                        //    {
                        //        size = XoroProductVariant.Option2ValueCode,
                        //        upc = XoroProductVariant.ItemUpc,
                        //        xoro_sku = XoroProductVariant.ItemNumber,
                        //    };

                        //    NuProduct.sizes.Add(sz);
                        //}

                        // (keep the rest of your per-variant mapping)
                        NuProduct.active = XoroProductVariant.ActiveFlag; // Variant Active Flag
                        NuProduct.cancelled = !XoroProductVariant.ActiveFlag;
                        NuProduct.color_code = XoroProductVariant.Option1ValueCode;
                        NuProduct.available_now = true;

                        // Is Immediate Logic
                        if (string.Equals(XoroProductVariant.CustomFieldH10, "Y", StringComparison.OrdinalIgnoreCase))
                        {
                            // immediate window = today … today+30
                            NuProduct.available_from = todayStr;
                            NuProduct.available_until = plus30Str;
                            NuProduct.available_now = true;
                        }
                        // Is Future Date Window
                        else if (string.Equals(XoroProductVariant.CustomFieldH11, "Y", StringComparison.OrdinalIgnoreCase))
                        {
                            // Future window (leave as-is if you still use these fields)
                            NuProduct.available_from = FormatDateOrNull(XoroProductVariant.CustomFieldH8);
                            NuProduct.available_until = FormatDateOrNull(XoroProductVariant.CustomFieldH9);
                            NuProduct.available_now = false;
                        }
                        // else: leave dates null; product will not be flagged available_now
                        //else if (XoroProductVariant.CustomFieldH11 == "Y") // Else it should be future date
                        //{
                        //    NuProduct.available_from = FormatDateOrNull(XoroProductVariant.CustomFieldH8);
                        //    NuProduct.available_until = FormatDateOrNull(XoroProductVariant.CustomFieldH9);
                        //    NuProduct.available_now = false;
                        //}
                        //else if (XoroProductVariant.IsPreSellFlag)
                        //{
                        //    NuProduct.available_from = FormatDateOrNull(XoroProductVariant.CustomFieldH13);
                        //    NuProduct.available_until = FormatDateOrNull(XoroProductVariant.CustomFieldH14);
                        //}

                        NuProduct.description = XoroProductVariant.Description;

                        // Season Logic
                        var variantSeason = !string.IsNullOrEmpty(XoroProductVariant.SeasonValues)
                        ? XoroProductVariant.SeasonValues.Split(',')[0].Trim()
                        : "";

                        NuProduct.season = variantSeason;
                        NuProduct.seasons = XoroProductVariant.SeasonValues?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>();

                        NuProduct.images = new List<string> { "" }; //XoroProductVariant.ImagePath

                        // Custom fields - detail fields
                        NuProduct.subcategory = XoroProductObj.ProductCategoryName;
                        NuProduct.division = XoroProductVariant.BrandName;
                        NuProduct.department = MapDepartmentFromBrand(XoroProductVariant.BrandName);
                        NuProduct.fabric_description = XoroProductVariant.MaterialName;
                        NuProduct.country_of_origin = XoroProductVariant.CooCodeIso2;

                        // Initialize Pricing object
                        NuProduct.pricing = new PricingVOCE
                        {
                            USD = new USD(),
                            CAD = new CAD(),
                            AUD = new AUD()
                        };

                        NuProduct.pricing.CAD = new CAD
                        {
                            wholesale = ConvertToDecimal(XoroProductVariant.StandardUnitPrice),
                            retail = ConvertToDecimal(XoroProductVariant.CustomPrice1),
                            disabled = false
                        };

                        // Iterate over the currency price list
                        foreach (var priceItem in XoroProductVariant.CurrencyPriceList)
                        {
                            if (priceItem.CurrencyCode == "USD")
                            {
                                NuProduct.pricing.USD = new USD
                                {
                                    wholesale = ConvertToDecimal(priceItem.StandardUnitPrice),
                                    retail = ConvertToDecimal(priceItem.CustomPrice1),
                                    disabled = false
                                };
                            }
                            else if (priceItem.CurrencyCode == "AUD")
                            {
                                NuProduct.pricing.AUD = new AUD
                                {
                                    wholesale = ConvertToDecimal(priceItem.StandardUnitPrice),
                                    retail = ConvertToDecimal(priceItem.CustomPrice1),
                                    disabled = false
                                };
                            }
                        }
                    }
                });

                return NuProduct;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        //Method to POST Products to selected Xoro Instance - https://voce-ent.xoro.one/
        public async Task UpdateXoroProductWithVariants(XoroProductDatum xoroProduct)
        {
            try
            {
                // Client Voce Enterprises
                var clientId = "5e232c5ebed443dfbb4ed4cc5579b13a";
                var clientSecret = "3fc2016c2ceb4cac86e647124b812d79";
                var authString = $"{clientId}:{clientSecret}";
                var base64Auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(authString));

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://res.xorosoft.io"),
                    Timeout = TimeSpan.FromSeconds(30)
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Get the latest product data from Xoro ERP
                var latestProduct = await _xoroApiService.GetProductByBasePartAsync(xoroProduct.BasePartNumber);
                if (latestProduct != null)
                {
                    // overlay IDs from xoroProduct (with newly set fields) onto latestProduct
                    foreach (var v in latestProduct.Variants)
                    {
                        var inc = xoroProduct.Variants.FirstOrDefault(iv => iv.ItemNumber == v.ItemNumber);
                        if (inc != null)
                        {
                            if (!string.IsNullOrWhiteSpace(inc.CustomFieldH2)) v.CustomFieldH2 = inc.CustomFieldH2;   // style-color id
                            if (!string.IsNullOrWhiteSpace(inc.CustomFieldH3)) v.CustomFieldH3 = inc.CustomFieldH3;   // size _id
                            if (!string.IsNullOrWhiteSpace(inc.CustomFieldH12)) v.CustomFieldH12 = inc.CustomFieldH12;  // style _id
                        }
                    }
                    xoroProduct = latestProduct;
                }

                // Transform to BasePartA for compatibility with API model
                var productRecord = MakeJsonObject_ProductRecordSAME(xoroProduct);
                var productRecordArray = new List<BasePartA> { productRecord };

                // Serialize as a single object for update endpoint
                var payload = JsonSerializer.Serialize(productRecordArray, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                });

                // Log payload for debugging
                Console.WriteLine("📦 Serialized Product Update Payload:\n" + payload);

                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                // Call the update endpoint
                var response = await httpClient.PostAsync("/api/xerp/product/create", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Log response
                Console.WriteLine("📥 Xoro Response:\n" + responseContent);

                response.EnsureSuccessStatusCode();

                // Parse and log result
                var resultObj = JsonSerializer.Deserialize<XoroProductRootobject>(responseContent);

                Console.WriteLine($"✅ Xoro Update Result: {resultObj?.Result}, Message: {resultObj?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to update product in Xoro: {ex.Message}");
            }
        }


        // Round-trip a product, apply a surgical JSON patch, push back to Xoro.
        // Nothing else is touched; all niche fields (tax profiles, province maps, etc.) remain intact.


        // Helper: case-insensitive property getter for JsonObject
        private static bool TryGetCaseInsensitive(JsonObject obj, string key, out JsonNode? value)
        {
            foreach (var kv in obj)
            {
                if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
                {
                    value = kv.Value;
                    return true;
                }
            }
            value = null;
            return false;
        }

        // Helper: truncate long strings for logging
        private static string Truncate(string? s, int max)
            => string.IsNullOrEmpty(s) ? string.Empty : (s.Length > max ? s.Substring(0, max) + "…" : s);

        // === DROP-IN REPLACEMENT ===
        public async Task<bool> XoroRoundTripPatchAsync(
            string basePartNumber,
            Action<JsonObject> patch // mutate only what you need
        )
        {
            var corrId = Guid.NewGuid().ToString("n"); // tie logs together

            try
            {
                // 1) Fetch authoritative RAW JSON
                string? raw;
                try
                {
                    raw = await _xoroApiService.GetProductByBasePartRawAsync(basePartNumber);
                }
                catch (Exception exFetch)
                {
                    Console.WriteLine($"❌ [{corrId}] Fetch failed for {basePartNumber}: {exFetch.Message}");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(raw))
                {
                    Console.WriteLine($"⚠️  [{corrId}] No JSON returned for {basePartNumber}.");
                    return false;
                }

                // 2) Parse & normalize product object
                JsonObject productObj;
                try
                {
                    var root = JsonNode.Parse(raw) as JsonObject
                        ?? throw new InvalidOperationException("Root JSON is not an object.");

                    // Handles { Data:[{...}] } or bare { ... }
                    productObj = ExtractProductObject(root)
                        ?? throw new InvalidOperationException("Product object not found in payload.");
                }
                catch (JsonException jx)
                {
                    Console.WriteLine($"❌ [{corrId}] JSON parse error ({basePartNumber}): {jx.Message}");
                    return false;
                }
                catch (Exception exParse)
                {
                    Console.WriteLine($"❌ [{corrId}] Extract error ({basePartNumber}): {exParse.Message}");
                    return false;
                }

                // 3) Apply your surgical patch
                try
                {
                    patch(productObj);
                }
                catch (Exception exPatch)
                {
                    Console.WriteLine($"❌ [{corrId}] Patch step threw ({basePartNumber}): {exPatch.Message}");
                    return false;
                }

                // 4) Serialize via detached clone to allow adding to JsonArray
                string json;
                try
                {
                    var serOpts = new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                        WriteIndented = false
                    };

                    // Clone by re-parsing so the node has no parent
                    var cloned = JsonNode.Parse(productObj.ToJsonString(serOpts))!.AsObject();
                    var payloadArray = new JsonArray { cloned };

                    json = payloadArray.ToJsonString(serOpts);
                }
                catch (Exception exSer)
                {
                    Console.WriteLine($"❌ [{corrId}] Serialize error ({basePartNumber}): {exSer.Message}");
                    return false;
                }

                Console.WriteLine($"📦 [{corrId}] Outgoing payload preview ({basePartNumber}): {Truncate(json, 4000)}");

                // 5) POST back to Xoro
                bool ok;
                string body;
                try
                {
                    (ok, body) = await _xoroApiService.PostProductUpdateRawAsync(json);
                }
                catch (Exception exPost)
                {
                    Console.WriteLine($"❌ [{corrId}] Post failed ({basePartNumber}): {exPost.Message}");
                    return false;
                }

                Console.WriteLine($"📥 [{corrId}] Xoro response ({basePartNumber}): {Truncate(body, 4000)}");

                // 6) If Xoro returns { result: bool, message: string }, surface it
                try
                {
                    var resp = JsonNode.Parse(body) as JsonObject;
                    if (resp != null)
                    {
                        bool hasResult = TryGetCaseInsensitive(resp, "Result", out var rNode) ||
                                         TryGetCaseInsensitive(resp, "result", out rNode);

                        if (hasResult && rNode is not null)
                        {
                            var svcOk = rNode.GetValue<bool>();
                            ok = ok && svcOk;

                            if (!ok &&
                                (TryGetCaseInsensitive(resp, "Message", out var mNode) ||
                                 TryGetCaseInsensitive(resp, "message", out mNode)))
                            {
                                Console.WriteLine($"❌ [{corrId}] Xoro error ({basePartNumber}): {mNode?.GetValue<string>()}");
                            }
                        }
                    }
                }
                catch
                {
                    // response wasn’t JSON; keep original ok
                }

                return ok;
            }
            catch (Exception ex)
            {
                // final safety net
                Console.WriteLine($"❌ [{corrId}] Unhandled error in XoroRoundTripPatchAsync({basePartNumber}): {ex}");
                return false;
            }
        }



        // Helper: handles { Data: [ {...} ] } or bare { ... }
        private static JsonObject? ExtractProductObject(JsonObject root)
        {
            if (root.TryGetPropertyValue("Data", out var data) && data is JsonArray arr && arr.Count > 0 && arr[0] is JsonObject obj)
                return obj;
            return root;
        }



        public Task<bool> XoroRoundTripPatchNuOrderIdsAsync(XoroProductDatum sourceWithIds)
        {
            return XoroRoundTripPatchAsync(sourceWithIds.BasePartNumber, productObj =>
            {
                if (!productObj.TryGetPropertyValue("Variants", out var vNode) || vNode is not JsonArray vArr) return;

                var byItem = sourceWithIds.Variants
                    .Where(v => !string.IsNullOrWhiteSpace(v.ItemNumber))
                    .ToDictionary(v => v.ItemNumber!, v => v);

                foreach (var vn in vArr.OfType<JsonObject>())
                {
                    var itemNo = vn["ItemNumber"]?.GetValue<string>();
                    if (string.IsNullOrWhiteSpace(itemNo) || !byItem.TryGetValue(itemNo!, out var src)) continue;

                    SetIfNotEmpty(vn, "CustomFieldH2", src.CustomFieldH2);  // style-color key
                    SetIfNotEmpty(vn, "CustomFieldH3", src.CustomFieldH3);  // size _id
                    SetIfNotEmpty(vn, "CustomFieldH12", src.CustomFieldH12); // style _id
                }
            });
        }

        private static void SetIfNotEmpty(JsonObject obj, string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value)) obj[key] = value;
        }


        // Method to log product sync status in SQL Server
        public async Task LogProductSyncStatus(string basePart, string color, string status, string message, string brand, string? requestJson = null, string? responseJson = null)
        {
            var log = new ProductSyncLog
            {
                BasePartNumber = basePart,
                ColorName = color,
                SyncStatus = status,
                Message = message,
                Brand = brand,
                Timestamp = DateTime.UtcNow,
                RequestJson = requestJson,
                ResponseJson = responseJson
            };

            _context.ProductSyncLogs.Add(log);
            await _context.SaveChangesAsync();
        }


        // Function to update in CAD prices in Vitaly Holdings INSTANCE
        public BasePartA MakeJsonObject_ProductRecordSAME(XoroProductDatum ProductRecord)
        {

            BasePartA xoroProductObject = new BasePartA();

            try
            {

                // Data translation
                xoroProductObject.ActiveFlag = ProductRecord.ActiveFlag;
                xoroProductObject.BasePartNumber = CheckNull(ProductRecord.BasePartNumber);
                xoroProductObject.BodyHtml = CheckNull(ProductRecord.BodyHtml);
                xoroProductObject.ProductCategoryName = CheckNull(ProductRecord.ProductCategoryName);
                xoroProductObject.Title = CheckNull(ProductRecord.Title);
                xoroProductObject.Handle = CheckNull(ProductRecord.Handle);

                xoroProductObject.Options = new List<ProductOptionA>();
                xoroProductObject.Variants = new List<ProductVariantA>();

                ProductRecord.Options.ForEach(delegate (ProductOption POptions)
                {
                    ProductOptionA optionA = new ProductOptionA();
                    optionA.Name = CheckNull(POptions.Name);
                    optionA.Position = CheckNullDigit(POptions.Position);
                    optionA.Values = new List<ProductValueA>();

                    POptions.Values.ForEach(delegate (ProductValue PValue)
                    {
                        ProductValueA valueA = new ProductValueA();
                        valueA.Code = CheckNull(PValue.Code);
                        valueA.Name = CheckNull(PValue.Name);
                        valueA.Seq = CheckNullDigit(PValue.Seq);

                        optionA.Values.Add(valueA);
                    });

                    optionA.ValueArr = new List<string>();

                    xoroProductObject.Options.Add(optionA);
                });

                ProductRecord.Variants.ForEach(delegate (ProductVariant PVariant)
                {
                    ProductVariantA variantA = new ProductVariantA();
                    variantA.ActiveFlag = PVariant.ActiveFlag;
                    if (PVariant.ActiveFlag) { variantA.ActiveFlagStr = "Y"; } else { variantA.ActiveFlagStr = "N"; }
                    variantA.AlternativeItemNumber1 = CheckNull(PVariant.AlternativeItemNumber1);
                    variantA.AlternativeItemNumber2 = CheckNull(PVariant.AlternativeItemNumber2);
                    variantA.AlternativeItemNumber3 = CheckNull(PVariant.AlternativeItemNumber3);
                    variantA.AdjAccountName = CheckNull(PVariant.AdjAccountName);
                    variantA.AlertNote = CheckNull(PVariant.AlertNote);
                    variantA.AssetAccountName = CheckNull(PVariant.AssetAccountName);
                    variantA.BrandName = CheckNull(PVariant.BrandName);
                    variantA.CategoryName = CheckNull(PVariant.CategoryName);
                    variantA.CogsAccountName = CheckNull(PVariant.CogsAccountName);
                    variantA.CooCodeIso2 = CheckNull(PVariant.CooCodeIso2);
                    variantA.Description = CheckNull(PVariant.Description);
                    variantA.ExpenseAccountName = CheckNull(PVariant.ExpenseAccountName);
                    variantA.GroupName = CheckNull(PVariant.GroupName);
                    variantA.HazmatName = CheckNull(PVariant.HazmatName);
                    variantA.Height = CheckNull(PVariant.Height);
                    variantA.HSCode = CheckNull(PVariant.HSCode);
                    variantA.ImagePath = CheckNull(PVariant.ImagePath);
                    variantA.IncomeAccountName = CheckNull(PVariant.IncomeAccountName);
                    variantA.IncomeReturnAccountName = CheckNull(PVariant.IncomeReturnAccountName);
                    variantA.IsBomItemFlag = PVariant.IsBomItemFlag;
                    variantA.IsPurchasableFlag = PVariant.IsPurchasableFlag;
                    if (PVariant.IsPurchasableFlag) { variantA.IsPurchasableFlagStr = "Y"; } else { variantA.IsPurchasableFlagStr = "N"; }
                    variantA.IsRawMaterialFlag = PVariant.IsRawMaterialFlag;
                    variantA.IsSellableFlag = PVariant.IsSellableFlag;
                    if (PVariant.IsSellableFlag) { variantA.IsSellableFlagStr = "Y"; } else { variantA.IsSellableFlagStr = "N"; }
                    variantA.IsTaxableFlag = PVariant.IsTaxableOnSale;
                    variantA.IsTaxableOnPurchase = PVariant.IsTaxableOnPurchase;
                    if (PVariant.IsTaxableOnPurchase) { variantA.IsTaxableOnPurchaseStr = "Y"; } else { variantA.IsTaxableOnPurchaseStr = "N"; }
                    variantA.IsTaxableOnSale = PVariant.IsTaxableOnSale;
                    if (PVariant.IsTaxableOnSale) { variantA.IsTaxableOnSaleStr = "Y"; } else { variantA.IsTaxableOnSaleStr = "N"; }
                    variantA.IsTransferableFlag = PVariant.IsTransferableFlag;
                    if (PVariant.IsTransferableFlag) { variantA.IsTransferableFlagStr = "Y"; } else { variantA.IsTransferableFlagStr = "N"; }
                    variantA.IsPreSellFlag = PVariant.IsPreSellFlag;
                    if (PVariant.IsPreSellFlag) { variantA.IsPreSellFlagStr = "Y"; } else { variantA.IsPreSellFlagStr = "N"; }
                    variantA.ItemBarcode = CheckNull(PVariant.ItemBarcode);
                    variantA.ItemNumber = CheckNull(PVariant.ItemNumber);
                    variantA.ItemTypeName = CheckNull(PVariant.ItemTypeName);
                    variantA.ItemUpc = CheckNull(PVariant.ItemUpc);
                    variantA.Length = CheckNull(PVariant.Length);
                    variantA.Option1Value = CheckNull(PVariant.Option1Value);
                    variantA.Option2Value = CheckNull(PVariant.Option2Value);
                    variantA.Option3Value = CheckNull(PVariant.Option3Value);
                    variantA.PurchaseNotes = CheckNull(PVariant.PurchaseNotes);
                    //variantA.ReOrderPointQty = CheckNullDigit(PVariant.ReOrderPointQty);
                    //variantA.ReOrderQty = CheckNullDigit(PVariant.ReOrderQty);
                    variantA.ReturnableFlag = PVariant.ReturnableFlag;
                    variantA.SalesNotes = CheckNull(PVariant.SalesNotes);
                    variantA.SaleTaxCode = CheckNull(PVariant.SaleTaxCode);
                    variantA.SeasonValues = CheckNull(PVariant.SeasonValues);
                    variantA.PurchaseTaxCode = CheckNull(PVariant.PurchaseTaxCode);
                    variantA.SellPkgQty = CheckNullDigit(PVariant.SellPkgQty);
                    variantA.SellUomName = CheckNull(PVariant.SellUomName);
                    variantA.SizeUomName = CheckNull(PVariant.SizeUomName);
                    variantA.Tags = CheckNull(PVariant.Tags);
                    variantA.WebUrl = CheckNull(PVariant.WebUrl);
                    variantA.Weight = PVariant.Weight;
                    variantA.WeightUomName = CheckNull(PVariant.WeightUomName);
                    variantA.Width = CheckNull(PVariant.Width);
                    variantA.WipAccountName = CheckNull(PVariant.WipAccountName);
                    variantA.DefaultVendorName = CheckNull(PVariant.DefaultVendorName);
                    variantA.MaterialName = CheckNull(PVariant.MaterialName);


                    // Item Custom fields
                    variantA.CustomFieldH1 = CheckNull(PVariant.CustomFieldH1);
                    variantA.CustomFieldH2 = CheckNull(PVariant.CustomFieldH2);
                    variantA.CustomFieldH3 = CheckNull(PVariant.CustomFieldH3);
                    variantA.CustomFieldH4 = CheckNull(PVariant.CustomFieldH4);
                    variantA.CustomFieldH5 = CheckNull(PVariant.CustomFieldH5);
                    variantA.CustomFieldH6 = CheckNull(PVariant.CustomFieldH6);
                    variantA.CustomFieldH7 = CheckNull(PVariant.CustomFieldH7);
                    variantA.CustomFieldH8 = CheckNull(PVariant.CustomFieldH8);
                    variantA.CustomFieldH9 = CheckNull(PVariant.CustomFieldH9);
                    variantA.CustomFieldH10 = CheckNull(PVariant.CustomFieldH10);
                    variantA.CustomFieldH11 = CheckNull(PVariant.CustomFieldH11);
                    variantA.CustomFieldH12 = CheckNull(PVariant.CustomFieldH12);
                    variantA.CustomFieldH13 = CheckNull(PVariant.CustomFieldH13);
                    variantA.CustomFieldH14 = CheckNull(PVariant.CustomFieldH14);
                    variantA.CustomFieldH15 = CheckNull(PVariant.CustomFieldH15);
                    variantA.CustomFieldH16 = CheckNull(PVariant.CustomFieldH16);
                    variantA.CustomFieldH17 = CheckNull(PVariant.CustomFieldH17);
                    variantA.CustomFieldH18 = CheckNull(PVariant.CustomFieldH18);
                    variantA.CustomFieldH19 = CheckNull(PVariant.CustomFieldH19);
                    variantA.CustomFieldH20 = CheckNull(PVariant.CustomFieldH20);
                    variantA.CustomFieldH21 = CheckNull(PVariant.CustomFieldH21);
                    variantA.CustomFieldH22 = CheckNull(PVariant.CustomFieldH22);
                    variantA.CustomFieldH23 = CheckNull(PVariant.CustomFieldH23);
                    variantA.CustomFieldH24 = CheckNull(PVariant.CustomFieldH24);
                    variantA.CustomFieldH25 = CheckNull(PVariant.CustomFieldH25);
                    variantA.CustomFieldH26 = CheckNull(PVariant.CustomFieldH26);
                    variantA.CustomFieldH27 = CheckNull(PVariant.CustomFieldH27);
                    variantA.CustomFieldH28 = CheckNull(PVariant.CustomFieldH28);
                    variantA.CustomFieldH29 = CheckNull(PVariant.CustomFieldH29);
                    variantA.CustomFieldH30 = CheckNull(PVariant.CustomFieldH30);
                    variantA.CustomFieldH31 = CheckNull(PVariant.CustomFieldH31);
                    variantA.CustomFieldH32 = CheckNull(PVariant.CustomFieldH32);
                    variantA.CustomFieldH33 = CheckNull(PVariant.CustomFieldH33);
                    variantA.CustomFieldH34 = CheckNull(PVariant.CustomFieldH34);
                    variantA.CustomFieldH35 = CheckNull(PVariant.CustomFieldH35);
                    variantA.CustomFieldH36 = CheckNull(PVariant.CustomFieldH36);
                    variantA.CustomFieldH37 = CheckNull(PVariant.CustomFieldH37);
                    variantA.CustomFieldH38 = CheckNull(PVariant.CustomFieldH38);
                    variantA.CustomFieldH39 = CheckNull(PVariant.CustomFieldH39);
                    variantA.CustomFieldH40 = CheckNull(PVariant.CustomFieldH40);

                    // Price columns
                    variantA.StandardUnitCost = PVariant.StandardUnitCost;
                    variantA.StandardUnitPrice = PVariant.StandardUnitPrice;
                    variantA.CustomPrice1 = PVariant.CustomPrice1;
                    variantA.CustomPrice2 = PVariant.CustomPrice2;
                    variantA.CustomPrice3 = PVariant.CustomPrice3;
                    variantA.CustomPrice4 = PVariant.CustomPrice4;
                    variantA.CustomPrice5 = PVariant.CustomPrice5;
                    variantA.CustomPrice6 = PVariant.CustomPrice6;
                    variantA.CustomPrice7 = PVariant.CustomPrice7;
                    variantA.CustomPrice8 = PVariant.CustomPrice8;
                    variantA.CustomPrice9 = PVariant.CustomPrice9;
                    variantA.CustomPrice10 = PVariant.CustomPrice10;
                    variantA.CustomPrice11 = PVariant.CustomPrice11;
                    variantA.CustomPrice12 = PVariant.CustomPrice12;
                    variantA.CustomPrice13 = PVariant.CustomPrice13;
                    variantA.CustomPrice14 = PVariant.CustomPrice14;

                    xoroProductObject.Variants.Add(variantA);
                });

                return xoroProductObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return xoroProductObject;
            }
        }



        public void PreserveCriticalFields(XoroProductDatum mergedProduct, XoroProductDatum originalProduct)
        {
            foreach (var variant in mergedProduct.Variants)
            {
                var original = originalProduct.Variants
                    .FirstOrDefault(v => v.ItemNumber == variant.ItemNumber);

                if (original != null)
                {
                    // Season
                    if (string.IsNullOrWhiteSpace(variant.SeasonValues) && !string.IsNullOrWhiteSpace(original.SeasonValues))
                        variant.SeasonValues = original.SeasonValues;

                    // UPC
                    if (string.IsNullOrWhiteSpace(variant.ItemUpc) && !string.IsNullOrWhiteSpace(original.ItemUpc))
                        variant.ItemUpc = original.ItemUpc;

                    // Option1Value (Color)
                    //if (string.IsNullOrWhiteSpace(variant.Option1Value) && !string.IsNullOrWhiteSpace(original.Option1Value))
                    //    variant.Option1Value = original.Option1Value;

                    // Option2Value (Size)
                    //if (string.IsNullOrWhiteSpace(variant.Option2Value) && !string.IsNullOrWhiteSpace(original.Option2Value))
                    //    variant.Option2Value = original.Option2Value;

                    // Add other fields here as needed (e.g., Active, CountryOfOrigin)
                }
            }
        }



        // Helper method to configure the HttpClient to avoid duplication:
        private HttpClient CreateHttpClient(string baseAddress, string clientId, string clientSecret)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
                Timeout = TimeSpan.FromSeconds(30)
            };
            var authString = $"{clientId}:{clientSecret}";
            var base64Auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(authString));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }


        // Function to check Null values before assignment
        public string CheckNull(object a)
        {
            if (a is null)
            {
                return "";
            }
            else
            {
                return a.ToString();
            }
        }



        // Function to check Null DIGIT values before assignment
        public int CheckNullDigit(object a)
        {
            if (a is null)
            {
                return 0;
            }
            else if (a.ToString() == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(a);
            }
        }



        private static decimal ConvertToDecimal(object value)
        {
            if (value is decimal decimalValue)
                return decimalValue;

            if (value is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.Number)
                    return jsonElement.GetDecimal();

                if (jsonElement.ValueKind == JsonValueKind.String && decimal.TryParse(jsonElement.GetString(), out decimal parsedValue))
                    return parsedValue;
            }

            return 0m; // Default fallback value
        }


        // Combine option 2 size and option 3 width
        private static string BuildCombinedSize(ProductVariant v)
        {
            // Prefer normalized “Code” fields; fall back to Value
            string s2 = string.IsNullOrWhiteSpace(v.Option2ValueCode) ? v.Option2Value : v.Option2ValueCode;  // e.g., "2.5"
            string s3 = string.IsNullOrWhiteSpace(v.Option3ValueCode) ? v.Option3Value : v.Option3ValueCode;  // e.g., "B"

            s2 = s2?.Trim();
            s3 = s3?.Trim()?.ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(s2) && string.IsNullOrWhiteSpace(s3)) return string.Empty;
            if (string.IsNullOrWhiteSpace(s3)) return s2 ?? string.Empty;

            // Combine without separator: "2.5" + "B" => "2.5B"
            return $"{s2}{s3}";
        }


        // Two-letter Brand -> NuORDER Department
        private static readonly Dictionary<string, string> BrandCodeToDepartment =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["AT"] = "ARTESANDS",
                ["BE"] = "BOND-EYE BOUND SWIMWEAR",
                ["BL"] = "BLOCH",
                ["FL"] = "FLO DANCEWEAR",
                ["KR"] = "KOY BRANDS",
                ["LE"] = "LEO'S DANCEWEAR",
                ["MI"] = "MIMY",
                ["ML"] = "MIRELLA",
                ["NT"] = "NIPTUCK",
                ["RP"] = "RP COLLECTION",
                ["SE"] = "SEA LEVEL SWIMWEAR",
                ["SU"] = "SUNNYLIFE"
            };

        private static string MapDepartmentFromBrand(string? brandCodeOrName)
        {
            if (string.IsNullOrWhiteSpace(brandCodeOrName)) return null!;
            var key = brandCodeOrName.Trim();
            // If we got a 2-letter code, map it; if it’s already a full name, just return it
            return BrandCodeToDepartment.TryGetValue(key, out var dept)
                ? dept
                : brandCodeOrName;
        }

        private string GenerateOAuthHeader(NuOrderConfig config, HttpMethod method, string url)
        {
            var oauthParameters = new Dictionary<string, string>
            {
                { "oauth_consumer_key", config.ConsumerKey },
                { "oauth_token", config.Token },
                { "oauth_nonce", Guid.NewGuid().ToString("N") },
                { "oauth_timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
                { "oauth_signature_method", config.SignatureMethod },
                { "oauth_version", config.Version }
            };

            // Add other OAuth parameters as needed

            // Create base string
            var baseString = $"{method.Method.ToUpper()}&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(string.Join("&", oauthParameters.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value}")))}";

            // Create signing key
            var signingKey = $"{Uri.EscapeDataString(config.ConsumerSecret)}&{Uri.EscapeDataString(config.TokenSecret)}";

            // Calculate signature
            using (var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(signingKey)))
            {
                var hashBytes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(baseString));
                oauthParameters["oauth_signature"] = Convert.ToBase64String(hashBytes);
            }

            // Construct OAuth header
            var oauthHeader = $"OAuth {string.Join(", ", oauthParameters.OrderBy(p => p.Key).Select(p => $"{p.Key}=\"{Uri.EscapeDataString(p.Value)}\""))}";

            return oauthHeader;
        }


        private string FormatDateOrNull(string dateStr)
        {
            return DateTime.TryParse(dateStr, out var dt) ? dt.ToString("yyyy-MM-dd") : null;
        }



        // Function to check if the minute is single digit or multi digit
        public string CheckDigit(int m)
        {
            string string_m;
            if (m < 10)
            {
                string_m = "0" + m.ToString();
            }
            else
            {
                string_m = m.ToString();
            }
            return string_m;
        }
    }
}
