using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Aigang.Contracts.Executor.Api.Client.DTO;
using Aigang.Contracts.Executor.Api.Client.Requests;
using Aigang.Contracts.Executor.Api.Client.Responses;
using Aigang.Platform.Domain.DeviceData;
using Aigang.Platform.Domain.Insurance;
using Aigang.Platform.Utils;
using log4net;
using Newtonsoft.Json;

namespace Aigang.Contracts.Executor.Api.Client
{
    public static class ContractsExecutorClient
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ContractsExecutorClient));
        private static readonly HttpClient _httpClient = GetHttpClient();
        
        public static async Task<Product> GetProductDetails(string productAddress, int productTypeId)
        {
            var requestPath = $"insurance/product/{productTypeId}/{productAddress}";

            var apiHttpResponse = await _httpClient.GetAsync(requestPath);
            var apiResponseContent = await apiHttpResponse.Content.ReadAsStringAsync();

            if (!apiHttpResponse.IsSuccessStatusCode)
            {
                if (apiHttpResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    _logger.Error($"[GetProductDetails] Not success response staus code: " +
                                  $"Request path: {requestPath} \n" +
                                  $"StatusCode: {(int)apiHttpResponse.StatusCode} {apiHttpResponse.StatusCode} \n" +
                                  $"Content: {apiResponseContent}");
                }
                
                HttpClientService.ThrowHandledException(apiHttpResponse.StatusCode, apiResponseContent);
            } 

            _logger.Info($"Received data from api: {apiResponseContent}");

            Product result =  new Product();

            switch (apiHttpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        result = JsonConvert.DeserializeObject<Product>(apiResponseContent);
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"Cant deserialize: {apiResponseContent} to Product \n{e}");
                        throw;
                    }

                    break;
                default:
                    _logger.Error($"[GetProductDetails] Not expected response staus code: " +
                                  $"Request path: {requestPath} \n" +
                                  $"StatusCode: {(int)apiHttpResponse.StatusCode} {apiHttpResponse.StatusCode} \n" +
                                  $"Content: {apiResponseContent}");
                    throw new ArgumentOutOfRangeException();
            }
            
            return result;
        }
        
        
        public static async Task<ProductStats> GetProductStats(string productAddress, int productTypeId)
        {
            var requestPath = $"insurance/product/{productTypeId}/{productAddress}/stats";

            var apiHttpResponse = await _httpClient.GetAsync(requestPath);
            var apiResponseContent = await apiHttpResponse.Content.ReadAsStringAsync();

            if (!apiHttpResponse.IsSuccessStatusCode)
            {
                if (apiHttpResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    _logger.Error($"[GetProductStats] Not success response staus code: " +
                                  $"Request path: {requestPath} \n" +
                                  $"StatusCode: {(int)apiHttpResponse.StatusCode} {apiHttpResponse.StatusCode} \n" +
                                  $"Content: {apiResponseContent}");
                }
                
                HttpClientService.ThrowHandledException(apiHttpResponse.StatusCode, apiResponseContent);
            } 

            _logger.Info($"Received data from api: {apiResponseContent}");

            ProductStats result =  new ProductStats();
            
            switch (apiHttpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        result = JsonConvert.DeserializeObject<ProductStats>(apiResponseContent);
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"Cant deserialize: {apiResponseContent} to ProductStats \n{e}");
                        throw;
                    }

                    break;
                default:
                    _logger.Error($"[GetProductStats] Not expected response staus code: " +
                                  $"Request path: {requestPath} \n" +
                                  $"StatusCode: {(int)apiHttpResponse.StatusCode} {apiHttpResponse.StatusCode} \n" +
                                  $"Content: {apiResponseContent}");
                    throw new ArgumentOutOfRangeException();
            }
            
            return result;
        }
        
        
        public static async Task<Policy> GetPolicy(string productAddress, int productTypeId, string policyId)
        {
            var requestPath = $"insurance/policy/{productTypeId}/{productAddress}/{policyId}";

            var apiHttpResponse = await _httpClient.GetAsync(requestPath);
            var apiResponseContent = await apiHttpResponse.Content.ReadAsStringAsync();

            if (!apiHttpResponse.IsSuccessStatusCode)
            {
                if (apiHttpResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    _logger.Error($"[GetProduct] Not success response staus code: " +
                                  $"Request path: {requestPath} \n" +
                                  $"StatusCode: {(int)apiHttpResponse.StatusCode} {apiHttpResponse.StatusCode} \n" +
                                  $"Content: {apiResponseContent}");
                }
                
                HttpClientService.ThrowHandledException(apiHttpResponse.StatusCode, apiResponseContent);
            } 

            _logger.Info($"Received data from api: {apiResponseContent}");

            Policy result =  new Policy();

            switch (apiHttpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        var dto = JsonConvert.DeserializeObject<GetPolicyResponse>(apiResponseContent);
                        result = dto.Policy;
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"Cant deserialize: {apiResponseContent} to Policy \n{e}");
                        throw;
                    }

                    break;
                default:
                    _logger.Error($"[GetPolicy] Not expected response staus code: " +
                                  $"Request path: {requestPath} \n" +
                                  $"StatusCode: {(int)apiHttpResponse.StatusCode} {apiHttpResponse.StatusCode} \n" +
                                  $"Content: {apiResponseContent}");
                    throw new ArgumentOutOfRangeException();
            }
            
            return result;
        }
        
        
        public static async Task<decimal> CalculatePremiumAsync(int productTypeId, string productAddress, MobileData mobileData)
        {
            var requestPath = $"insurance/calculatepremium";

            _logger.Info($"Requesting to calculate premium from Aigang.Contracts.Executor.Api");

            var responseMessage = await MobileDataGetAsync(productTypeId, productAddress, mobileData, requestPath);
            var response = JsonConvert.DeserializeObject<CalculatePremiumResponse>(await responseMessage.Content.ReadAsStringAsync());

            return response.Premium;
        }
        
        public static async Task<string> ValidateDataAsync(int productTypeId, string productAddress, MobileData mobileData)
        {
            var requestPath = "insurance/validatedata";

            _logger.Info($"Requesting to calculate premium from Aigang.Contracts.Executor.Api");

            var responseMessage = await MobileDataGetAsync(productTypeId, productAddress, mobileData, requestPath);
            var response = JsonConvert.DeserializeObject<ValidateDataResponse>(await responseMessage.Content.ReadAsStringAsync());

            return response.ValidationResultCode;
        }


        public static async Task<bool> IsClaimableAsync(int productTypeId, string productAddress, MobileData mobileData)
        {
            var requestPath = $"insurance/isclaimable";

            _logger.Info($"Requesting to calculate premium from Aigang.Contracts.Executor.Api");

            var responseMessage = await MobileDataGetAsync(productTypeId, productAddress, mobileData, requestPath);
            var response = JsonConvert.DeserializeObject<IsClaimableResponse>(await responseMessage.Content.ReadAsStringAsync());

            return response.IsClaimable;
        }
 
        
        private static async Task<HttpResponseMessage> MobileDataGetAsync(int productTypeId, string productAddress, MobileData mobileData,
            string requestPath)
        {
            var request = new MobileDataRequest();
            
            request.ProductTypeId = productTypeId;
            request.ProductAddress = productAddress;

            request.MobileData = new MobileDataDto
            {
                WearLevel = mobileData.WearLevel.ToString("D"),
                BatteryDesignCapacity = mobileData.BatteryDesignCapacity,
                ChargeLevel = mobileData.ChargeLevel,
                AgeInMonths = mobileData.AgeInMonths,
                Region = mobileData.Region,
                Brand = mobileData.Brand
            };

            var json = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(_httpClient.BaseAddress + requestPath),
                Method = HttpMethod.Get,
                Content = stringContent
            };

            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await _httpClient.SendAsync(httpRequestMessage);
            var response = JsonConvert.DeserializeObject<BaseContractsExecutorResponse>(await result.Content.ReadAsStringAsync());

            if (!result.IsSuccessStatusCode)
            {
                _logger.Error($"Communication failed with Aigang.Contracts.Executor.Api: Request path: {requestPath} StatusCode: {result.StatusCode} Reason: {response.Message}");
                HttpClientService.ThrowHandledException(result.StatusCode, response.Message, response.Reason);
            }

            return result;
        }


        private static HttpClient GetHttpClient()
        {
            var client = new HttpClient();

            var baseAddress = ConfigurationManager.GetString("ContractsExecutor:Address");
            var apiSecret = ConfigurationManager.GetString("ContractsExecutor:Secret");

            var bytes = Encoding.ASCII.GetBytes(apiSecret);
            string value = Convert.ToBase64String(bytes);

            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.TryAddWithoutValidation(HttpRequestHeader.Authorization.ToString(), value);
  
            return client;
        }
    }
}