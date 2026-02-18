using VOCENuOrderApp.Models.XORO;
using VOCENuOrderApp.Models.XoroModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace VOCENuOrderApp.Services
{
    //PAPER LABEL SERVICE
    public class XoroApiServiceVOCE
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<XoroApiServiceVOCE> _logger;

        public XoroApiServiceVOCE(HttpClient httpClient, ILogger<XoroApiServiceVOCE> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<List<XoroInvDatum>?> GetInventoryByBasePartAsync(string basePartNumber, string[] storeNames)
        {
            try
            {
                var allData = new List<XoroInvDatum>();
                var storeList = string.Join(",", storeNames);
                int currentPage = 1;
                int totalPages = int.MaxValue; // set initial high value to enter loop

                while (currentPage <= totalPages)
                {
                    string endpoint = $"https://res.xorosoft.io/api/xerp/inventory/getinventorybyitem?base_part_number={Uri.EscapeDataString(basePartNumber)}&store={Uri.EscapeDataString(storeList)}&page={currentPage}";

                    var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetEncodedAuth());
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<XORO_INVENTORY>(json);

                    if (result?.Data != null)
                        allData.AddRange(result.Data);

                    // update total pages after first fetch
                    totalPages = result?.TotalPages ?? currentPage;

                    currentPage++;
                }

                return allData;
            }
            catch (Exception ex)
            {
                // Optional: log error ex.Message
                return null;
            }
        }


        public async Task<XoroProductDatum?> GetProductByBasePartAsync(string basePartNumber)
        {
            try
            {
                var url = $"https://res.xorosoft.io/api/xerp/product/getproduct?base_part_number={HttpUtility.UrlEncode(basePartNumber)}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetEncodedAuth());
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<XoroApiResponseVOCE<XoroProductDatum>>(json);
                return result?.Data?.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }


        // Function to post a sales order to Xoro
        public async Task<bool> PostXoroSalesOrderAsync(XoroOrder salesOrder)
        {
            try
            {
                if (salesOrder == null)
                {
                    _logger.LogError("❌ Sales order object is null.");
                    return false;
                }

                // Construct the request URL
                var url = "https://res.xorosoft.io/api/xerp/salesorder/create";

                // Serialize the sales order object to JSON
                var jsonPayload = JsonSerializer.Serialize(salesOrder, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = requestContent
                };

                // Add authorization and headers
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetEncodedAuth());
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Send the request
                var response = await _httpClient.SendAsync(request);

                var message = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Successfully posted sales order to Xoro.");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("❌ Failed to post sales order to Xoro. Status: {StatusCode}, Error: {Error}", response.StatusCode, error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception while posting sales order to Xoro.");
                return false;
            }
        }


        // Function to get incoming PO deliveries for specific items and stores
        public async Task<List<IncomingPODatum>> GetIncomingPODeliveriesAsync(List<string> itemNumbers, string[] storeNames)
        {
            try
            {
                var storeList = string.Join(",", storeNames);
                var itemList = string.Join(",", itemNumbers.Distinct());
                var url = $"https://res.xorosoft.io/api/xerp/purchaseorder/getincomingpodeliveries?item_numbers={itemList}&store_names={storeList}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetEncodedAuth());
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<IncomingPODatum>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonSerializer.Deserialize<XoroResponseWrapperVOCE<List<IncomingPODatum>>>(content);

                return json?.Data ?? new List<IncomingPODatum>();
            }
            catch
            {
                return new List<IncomingPODatum>();
            }
        }


        // RAW JSON getter (authoritative product) — preserves all niche fields like province taxes
        public async Task<string?> GetProductByBasePartRawAsync(string basePartNumber)
        {
            try
            {
                var url = $"https://res.xorosoft.io/api/xerp/product/getproduct?base_part_number={Uri.EscapeDataString(basePartNumber)}";

                using var req = new HttpRequestMessage(HttpMethod.Get, url);
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetEncodedAuth());
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var res = await _httpClient.SendAsync(req);
                if (!res.IsSuccessStatusCode) return null;

                return await res.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProductByBasePartRawAsync failed for {BasePart}", basePartNumber);
                return null;
            }
        }

        // RAW JSON poster for product create/update — expects a JSON ARRAY string (e.g., [ { product... } ])
        public async Task<(bool ok, string body)> PostProductUpdateRawAsync(string jsonArrayPayload)
        {
            try
            {
                var url = "https://res.xorosoft.io/api/xerp/product/create";

                using var req = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(jsonArrayPayload, Encoding.UTF8, "application/json")
                };
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetEncodedAuth());
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var res = await _httpClient.SendAsync(req);
                var body = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogError("❌ PostProductUpdateRawAsync failed. Status: {Status} Body: {Body}", res.StatusCode, body);
                    return (false, body);
                }

                _logger.LogInformation("✅ PostProductUpdateRawAsync succeeded.");
                return (true, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception in PostProductUpdateRawAsync");
                return (false, ex.Message);
            }
        }


        private static string GetEncodedAuth()
        {
            // Voce Enterprise credentials
            var clientId = "5e232c5ebed443dfbb4ed4cc5579b13a";
            var clientSecret = "3fc2016c2ceb4cac86e647124b812d79";
            var authenticationString = $"{clientId}:{clientSecret}";
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
        }
    }

    public class XoroApiResponseVOCE<T>
    {
        public bool Result { get; set; }
        public List<T> Data { get; set; } = new();
    }

    public class XoroResponseWrapperVOCE<T>
    {
        public bool Result { get; set; }
        public T Data { get; set; } = default!;
        public string Message { get; set; } = string.Empty;
    }
}
