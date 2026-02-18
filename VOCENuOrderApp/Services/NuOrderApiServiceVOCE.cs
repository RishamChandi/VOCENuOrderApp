using Microsoft.Extensions.Options;
using VOCENuOrderApp.Models.NUORDER;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VOCENuOrderApp.Services
{
    public class NuOrderApiServiceVOCE
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NuOrderApiServiceVOCE> _logger;
        private readonly NuOrderVOCEConfig _config;
        string env = "next"; // Sandbox is sandbox1 and for production is next

        public NuOrderApiServiceVOCE(HttpClient httpClient, ILogger<NuOrderApiServiceVOCE> logger, IOptions<NuOrderVOCEConfig> config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config.Value;
        }

        // Body-based status update using NuOrder internal id
        public async Task<bool> UpdateOrderStatusByIdAsync(string orderId, string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderId))
                {
                    _logger.LogError("❌ Order ID empty for status update.");
                    return false;
                }
                var normalized = status?.Trim().ToUpperInvariant();
                if (string.IsNullOrWhiteSpace(normalized))
                {
                    _logger.LogError("❌ Status empty for update. OrderId: {OrderId}", orderId);
                    return false;
                }
                var url = $"https://{env}.nuorder.com/api/order/{orderId}/status";
                var payloadObj = new { status = normalized };
                var payloadJson = JsonSerializer.Serialize(payloadObj);
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(payloadJson, Encoding.UTF8, "application/json")
                };
                OAuthHelper.SignRequest(request, _config);
                _logger.LogDebug("📤 Status Update(ID) URL: {Url} Payload: {Payload}", url, payloadJson);
                var response = await _httpClient.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Status updated (ID). OrderId: {OrderId} -> {Status}", orderId, normalized);
                    return true;
                }
                _logger.LogWarning("⚠️ Status update failed (ID). OrderId: {OrderId} Http: {Code} Body: {Body}", orderId, response.StatusCode, body);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception updating status (ID). OrderId: {OrderId}", orderId);
                return false;
            }
        }

        // Body-based status update using order number
        public async Task<bool> UpdateOrderStatusByNumberAsync(string orderNumber, string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderNumber))
                {
                    _logger.LogError("❌ Order number empty for status update.");
                    return false;
                }
                var normalized = status?.Trim().ToUpperInvariant();
                if (string.IsNullOrWhiteSpace(normalized))
                {
                    _logger.LogError("❌ Status empty for update. OrderNumber: {OrderNumber}", orderNumber);
                    return false;
                }
                var url = $"https://{env}.nuorder.com/api/order/number/{orderNumber}/status";
                var payloadObj = new { status = normalized };
                var payloadJson = JsonSerializer.Serialize(payloadObj);
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(payloadJson, Encoding.UTF8, "application/json")
                };
                OAuthHelper.SignRequest(request, _config);
                _logger.LogDebug("📤 Status Update(Number) URL: {Url} Payload: {Payload}", url, payloadJson);
                var response = await _httpClient.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Status updated (Number). OrderNumber: {OrderNumber} -> {Status}", orderNumber, normalized);
                    return true;
                }
                _logger.LogWarning("⚠️ Status update failed (Number). OrderNumber: {OrderNumber} Http: {Code} Body: {Body}", orderNumber, response.StatusCode, body);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception updating status (Number). OrderNumber: {OrderNumber}", orderNumber);
                return false;
            }
        }

        public record StatusUpdateResult(bool Success, string StatusCode, string ResponseBody, string RequestUrl);

        // Detailed variant returning request/response for logging
        public async Task<StatusUpdateResult> UpdateOrderStatusByNumberPathWithResultAsync(string orderNumber, string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderNumber))
                {
                    var msg = "Order number empty for path status update.";
                    _logger.LogError("❌ {Msg}", msg);
                    return new StatusUpdateResult(false, string.Empty, msg, string.Empty);
                }
                var normalized = status?.Trim().ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(normalized))
                {
                    var msg = $"Status empty for path update. OrderNumber: {orderNumber}";
                    _logger.LogError("❌ {Msg}", msg);
                    return new StatusUpdateResult(false, string.Empty, msg, string.Empty);
                }
                var safeNum = Uri.EscapeDataString(orderNumber);
                var safeStatus = Uri.EscapeDataString(normalized);
                var url = $"https://{env}.nuorder.com/api/order/number/{safeNum}/{safeStatus}";
                var request = new HttpRequestMessage(HttpMethod.Put, url);
                OAuthHelper.SignRequest(request, _config);
                _logger.LogDebug("📤 Path Status Update URL: {Url}", url);
                var response = await _httpClient.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();
                var code = ((int)response.StatusCode).ToString();
                return new StatusUpdateResult(response.IsSuccessStatusCode, code, body, url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception during path status update. OrderNumber: {OrderNumber}", orderNumber);
                return new StatusUpdateResult(false, string.Empty, ex.Message, string.Empty);
            }
        }

        // Path-based status update using order number and lowercase status, no body.
        public async Task<bool> UpdateOrderStatusByNumberPathAsync(string orderNumber, string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderNumber))
                {
                    _logger.LogError("❌ Order number empty for path status update.");
                    return false;
                }
                var normalized = status?.Trim().ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(normalized))
                {
                    _logger.LogError("❌ Status empty for path update. OrderNumber: {OrderNumber}", orderNumber);
                    return false;
                }
                var safeNum = Uri.EscapeDataString(orderNumber);
                var safeStatus = Uri.EscapeDataString(normalized);
                var url = $"https://{env}.nuorder.com/api/order/number/{safeNum}/{safeStatus}"; // path style
                var request = new HttpRequestMessage(HttpMethod.Put, url); // no body
                OAuthHelper.SignRequest(request, _config);
                _logger.LogDebug("📤 Path Status Update URL: {Url}", url);
                var response = await _httpClient.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Status updated (path). OrderNumber: {OrderNumber} -> {Status}", orderNumber, normalized);
                    _logger.LogDebug("✅ NuOrder status update response: {StatusCode} {Body}", (int)response.StatusCode, body);
                    return true;
                }
                _logger.LogWarning("⚠️ Path status update failed. OrderNumber: {OrderNumber} Http: {Code} Body: {Body}", orderNumber, response.StatusCode, body);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception during path status update. OrderNumber: {OrderNumber}", orderNumber);
                return false;
            }
        }

        public async Task<bool> CreateSingleReplenishmentAsync(NuOrderReplenishment payload)
        {
            try
            {
                if (payload == null)
                {
                    _logger.LogError("❌ Payload is null");
                    return false;
                }
                var warehouse_id = payload.warehouse_id;
                var sku_id = payload.sku_id;
                if (string.IsNullOrEmpty(warehouse_id) || string.IsNullOrEmpty(sku_id))
                {
                    _logger.LogError("❌ Warehouse ID or SKU ID is null or empty");
                    return false;
                }

                // Construct the request URL
                var requestUrl = $"https://{env}.nuorder.com/api/v3.0/warehouse/{warehouse_id}/sku/{sku_id}/replenishments";
                var jsonPayload = JsonSerializer.Serialize(payload);
                var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = requestContent
                };

                // 🔐 Sign request using OAuth 1.0a
                OAuthHelper.SignRequest(request, _config);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Replenishment created successfully for {PartNumber}", payload?.display_name);
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("❌ Failed to create replenishment. Status: {Status}. Error: {Error}", response.StatusCode, error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception while creating NuOrder replenishment request");
                return false;
            }
        }


        // Bulk Create Replenishments Scheduled
        public async Task CreateOrUpdateReplenishmentsScheduled(string warehouseId, NUORDER_REPLENISHMENT_BULK payload, string colorName)
        {
            var result = await CreateOrUpdateReplenishments(warehouseId, payload);
            _logger.LogInformation("✅ [Scheduled] Submitted replenishments for Color: {ColorName} | Result: {Result}", colorName, result);
        }



        // Bulk Create Replenishments
        public async Task<bool> CreateOrUpdateReplenishments(string warehouseId, NUORDER_REPLENISHMENT_BULK bulkPayload)
        {
            var (ok, _) = await CreateOrUpdateReplenishmentsWithBodyAsync(warehouseId, bulkPayload);
            return ok;
        }

        // Bulk Create Replenishments returning response body for logging
        public async Task<(bool Success, string ResponseBody)> CreateOrUpdateReplenishmentsWithBodyAsync(string warehouseId, NUORDER_REPLENISHMENT_BULK bulkPayload)
        {
            try
            {
                if (bulkPayload == null || bulkPayload.replenishments == null || !bulkPayload.replenishments.Any())
                {
                    _logger.LogError("❌ Bulk payload is null or empty");
                    return (false, string.Empty);
                }
                // Validate warehouse_id and sku_id for each replenishment
                foreach (var replenishment in bulkPayload.replenishments)
                {
                    if (string.IsNullOrEmpty(replenishment.warehouse_id) || string.IsNullOrEmpty(replenishment.sku_id))
                    {
                        _logger.LogError("❌ Warehouse ID or SKU ID is null or empty for replenishment");
                        return (false, string.Empty);
                    }
                }
                var warehouse_id = warehouseId;
                // Construct the request URL
                var requestUrl = $"https://{env}.nuorder.com/api/v3.0/warehouse/{warehouse_id}/replenishments"; // Ensure this is correct for bulk
                var jsonPayload = JsonSerializer.Serialize(bulkPayload);
                var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Put, requestUrl)
                {
                    Content = requestContent
                };

                // 🔐 Sign request using OAuth 1.0a
                OAuthHelper.SignRequest(request, _config);

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Bulk replenishments posted successfully. Total: {Count}", bulkPayload.replenishments.Count);
                    return (true, responseBody);
                }

                _logger.LogError("❌ Failed to post bulk replenishments. Status: {Status}. Error: {Error}", response.StatusCode, responseBody);
                return (false, responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception while posting NuOrder replenishment bulk request");
                return (false, string.Empty);
            }
        }


        // Delete Replenishement by SKU id and Warehouse id
        public async Task<bool> DeleteReplenishmentAsync(string warehouseId, string skuId)
        {
            try
            {
                var url = $"https://{env}.nuorder.com/api/v3.0/warehouse/{warehouseId}/sku/{skuId}/replenishments";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);

                // 🔐 Sign request using OAuth 1.0a
                OAuthHelper.SignRequest(request, _config);

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Deleted existing replenishment for SKU: {SkuId}", skuId);
                    var deserializedResponse = JsonSerializer.Deserialize<NuOrderReplenResponse>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("⚠️ Failed to delete replenishment for SKU: {SkuId}. Error: {Error}", skuId, error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception during replenishment delete for SKU: {SkuId}", skuId);
                return false;
            }
        }



        // Refersh Replenishment Request by NuOrder Product ID
        public async Task<bool> UpdateInventoryFlagsAsync(List<string> productIds)
        {
            try
            {
                if (productIds == null || !productIds.Any())
                {
                    _logger.LogError("❌ Product IDs list is null or empty.");
                    return false;
                }

                // Construct the request URL
                var requestUrl = $"https://{env}.nuorder.com/api/inventory/flags";

                // Create the payload
                var payload = new
                {
                    product_ids = productIds
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Put, requestUrl)
                {
                    Content = requestContent
                };

                // 🔐 Sign request using OAuth 1.0a
                OAuthHelper.SignRequest(request, _config);

                var response = await _httpClient.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Inventory flags updated successfully for Product IDs: {ProductIds}", string.Join(", ", productIds));
                    return true;
                }
                else
                {
                    _logger.LogError("❌ Failed to update inventory flags. Status: {Status}, Error: {Error}", response.StatusCode, responseText);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception while updating inventory flags.");
                return false;
            }
        }


        // NOT IN USE - UNTIL WE KNOW THE PRICESHEET NAME
        public async Task<bool> AssignPriceSheetBulkAsync(NUORDER_PRICESHEET payload)
        {
            try
            {
                if (payload == null || payload.pricing == null || !payload.pricing.Any())
                {
                    _logger.LogError("❌ Price sheet payload is null or empty.");
                    return false;
                }

                string url = "https://{env}.nuorder.com/api/pricesheet/bulk/assign/DBP%20USD"; // DBP USD Replace `{env}` with your target env
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                // 🔐 OAuth signing
                OAuthHelper.SignRequest(request, _config);

                var response = await _httpClient.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Price sheet bulk assignment successful.");
                    return true;
                }
                else
                {
                    _logger.LogError("❌ Price sheet bulk assignment failed. Status: {Status}, Error: {Error}", response.StatusCode, responseText);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception during NuORDER price sheet bulk assignment.");
                return false;
            }
        }


        // Post price sheet to NuOrder system - Date Based Pricesheet
        public async Task<bool> SendPriceSheetRequestAsync(string priceSheetName, NUORDER_PRICESHEET payload)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(priceSheetName))
                {
                    _logger.LogError("❌ Price sheet name is empty.");
                    return false;
                }

                if (payload == null || payload.pricing == null || !payload.pricing.Any())
                {
                    _logger.LogError("❌ Price sheet payload is null or empty.");
                    return false;
                }

                string url = $"https://{env}.nuorder.com/api/pricesheet/bulk/assign/{Uri.EscapeDataString(priceSheetName)}";

                var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                _logger.LogInformation("📤 Sending PriceSheet to NuOrder ({PriceSheet}): {Payload}", priceSheetName, jsonPayload);

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
                };

                OAuthHelper.SignRequest(request, _config);

                var response = await _httpClient.SendAsync(request);
                var responseData = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ PriceSheet '{PriceSheet}' posted successfully.", priceSheetName);
                    return true;
                }
                else
                {
                    _logger.LogError("❌ NuOrder PriceSheet post failed. Status: {StatusCode}, Response: {Response}", response.StatusCode, responseData);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception while posting PriceSheet '{PriceSheet}'", priceSheetName);
                return false;
            }
        }



        // Function to NuOrder sales order by status 
        public async Task<List<string>> GetOrderIdsByStatusAsync(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    _logger.LogError("❌ Status is null or empty.");
                    return new List<string>();
                }

                // Construct the request URL
                var requestUrl = $"https://{env}.nuorder.com/api/orders/{status}/list";

                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                // 🔐 Sign request using OAuth 1.0a
                OAuthHelper.SignRequest(request, _config);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var orderIds = JsonSerializer.Deserialize<List<string>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("✅ Successfully fetched order IDs for status '{Status}'. Total Orders: {Count}", status, orderIds?.Count ?? 0);
                    return orderIds ?? new List<string>();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("❌ Failed to fetch order IDs for status '{Status}'. Status: {StatusCode}, Error: {Error}", status, response.StatusCode, error);
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception while fetching order IDs for status '{Status}'", status);
                return new List<string>();
            }
        }


        // Function to get order details by Order ID
        public async Task<NuOrderSORootobject?> GetOrderByIdAsync(string orderId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderId))
                {
                    _logger.LogError("❌ Order ID is null or empty.");
                    return null;
                }

                // Construct the request URL
                var requestUrl = $"https://{env}.nuorder.com/api/order/{orderId}";

                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                // 🔐 Sign request using OAuth 1.0a
                OAuthHelper.SignRequest(request, _config);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var orderDetails = JsonSerializer.Deserialize<NuOrderSORootobject>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("✅ Successfully fetched order details for Order ID '{OrderId}'.", orderId);
                    return orderDetails;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("❌ Failed to fetch order details for Order ID '{OrderId}'. Status: {StatusCode}, Error: {Error}", orderId, response.StatusCode, error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception while fetching order details for Order ID '{OrderId}'", orderId);
                return null;
            }
        }




        // Helper class for OAuth 1.0a signing
        public static class OAuthHelper
        {
            public static void SignRequest(HttpRequestMessage request, NuOrderVOCEConfig config)
            {
                // Generate OAuth 1.0a signature
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var nonce = Guid.NewGuid().ToString("N");

                var signatureBaseString = GenerateSignatureBaseString(request, config, timestamp, nonce);
                var signature = GenerateSignature(signatureBaseString, config.ConsumerSecret, config.TokenSecret);

                // Add OAuth headers
                var authHeader = $"OAuth oauth_consumer_key=\"{config.ConsumerKey}\", " +
                                 $"oauth_token=\"{config.Token}\", " +
                                 $"oauth_signature_method=\"{config.SignatureMethod}\", " +
                                 $"oauth_timestamp=\"{timestamp}\", " +
                                 $"oauth_nonce=\"{nonce}\", " +
                                 $"oauth_version=\"{config.Version}\", " +
                                 $"oauth_signature=\"{Uri.EscapeDataString(signature)}\"";

                request.Headers.Add("Authorization", authHeader);
            }

            private static string GenerateSignatureBaseString(HttpRequestMessage request, NuOrderVOCEConfig config, string timestamp, string nonce)
            {
                var method = request.Method.Method.ToUpper();
                var url = request.RequestUri.GetLeftPart(UriPartial.Path);
                var parameters = new SortedDictionary<string, string>
            {
                { "oauth_consumer_key", config.ConsumerKey },
                { "oauth_token", config.Token },
                { "oauth_signature_method", config.SignatureMethod },
                { "oauth_timestamp", timestamp },
                { "oauth_nonce", nonce },
                { "oauth_version", config.Version }
            };

                // Include query parameters
                if (request.RequestUri.Query.Length > 0)
                {
                    var queryParameters = request.RequestUri.Query.TrimStart('?').Split('&');
                    foreach (var param in queryParameters)
                    {
                        var keyValue = param.Split('=');
                        parameters.Add(Uri.EscapeDataString(keyValue[0]), Uri.EscapeDataString(keyValue[1]));
                    }
                }

                // Include body parameters for POST requests
                if (request.Content != null && request.Content.Headers.ContentType.MediaType == "application/x-www-form-urlencoded")
                {
                    var bodyParameters = request.Content.ReadAsStringAsync().Result.Split('&');
                    foreach (var param in bodyParameters)
                    {
                        var keyValue = param.Split('=');
                        parameters.Add(Uri.EscapeDataString(keyValue[0]), Uri.EscapeDataString(keyValue[1]));
                    }
                }

                var parameterString = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                return $"{method}&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(parameterString)}";
            }





            private static string GenerateSignature(string baseString, string consumerSecret, string tokenSecret)
            {
                var key = $"{Uri.EscapeDataString(consumerSecret)}&{Uri.EscapeDataString(tokenSecret)}";
                using var hasher = new HMACSHA1(Encoding.UTF8.GetBytes(key));
                var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(baseString));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
