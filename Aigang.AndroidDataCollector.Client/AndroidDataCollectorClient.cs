using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Aigang.AndroidDataCollector.Client.Responses;
using Aigang.Platform.Utils;
using log4net;
using Newtonsoft.Json;

namespace Aigang.AndroidDataCollector.Client
{
    public class AndroidDataCollectorClient : IAndroidDataCollectorClient
    {
        private static readonly string _dataCollectorApiAddress = ConfigurationManager.GetString("AndroidDataCollector:Address");
        private static readonly string _dataCollectorApiSecret = ConfigurationManager.GetString("AndroidDataCollector:Secret");
        private readonly ILog _logger = LogManager.GetLogger(typeof(AndroidDataCollectorClient));

        private static readonly HttpClient _httpClient = GetHttpClient(_dataCollectorApiAddress);


        public async Task<FetchDataResponse> RequestForDeviceData(string deviceId)
        {
            var requestPath = $"device/data/task/{deviceId}";

            HttpResponseMessage apiHttpResponse = await _httpClient.GetAsync(requestPath);

            var apiResponseContent = await apiHttpResponse.Content.ReadAsStringAsync();

            if (!apiHttpResponse.IsSuccessStatusCode)
            {
                if (apiHttpResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    _logger.Error($"[RequestForDeviceData] Not success response staus code: " +
                                  $"Request path: {requestPath} \n" +
                                  $"StatusCode: {(int) apiHttpResponse.StatusCode} {apiHttpResponse.StatusCode} \n" +
                                  $"Content: {apiResponseContent}");
                }

                HttpClientService.ThrowHandledException(apiHttpResponse.StatusCode, apiResponseContent);
            }

            _logger.Info($"Received data from api: {apiResponseContent}");

            FetchDataResponse result = new FetchDataResponse();

            try
            {
                result = JsonConvert.DeserializeObject<FetchDataResponse>(apiResponseContent);
            }
            catch (Exception e)
            {
                _logger.Error($"Cant deserialize: {apiResponseContent} to FetchDataResponse \n{e}");
                throw;
            }


            result.IsResponseAccepted = apiHttpResponse.StatusCode == HttpStatusCode.Accepted;
            return result;
        }

        public async Task<MobileDataResponse> GetDeviceDataAsync(string taskId)
        {
            var requestPath = $"device/data/{taskId}";

            var apiHttpResponse = await _httpClient.GetAsync(requestPath);
            var apiResponseContent = await apiHttpResponse.Content.ReadAsStringAsync();

            if (!apiHttpResponse.IsSuccessStatusCode)
            {
                if (apiHttpResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    _logger.Error($"[GetDeviceDataAsync] Not success response staus code: " +
                                  $"Request path: {requestPath} \n" +
                                  $"StatusCode: {(int)apiHttpResponse.StatusCode} {apiHttpResponse.StatusCode} \n" +
                                  $"Content: {apiResponseContent}");
                }
                
                HttpClientService.ThrowHandledException(apiHttpResponse.StatusCode, apiResponseContent);
            } 

            _logger.Info($"Received data from api: {apiResponseContent}");

            MobileDataResponse result =  new MobileDataResponse();

            switch (apiHttpResponse.StatusCode)
            {
                case HttpStatusCode.Accepted:
                    result.IsResponseAccepted = true;
                    break;
                case HttpStatusCode.OK:
                    try
                    {
                        result = JsonConvert.DeserializeObject<MobileDataResponse>(apiResponseContent);
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"Cant deserialize: {apiResponseContent} to FetchDataResponse \n{e}");
                        throw;
                    }

                    break;
                default:
                    _logger.Error($"[GetDeviceDataAsync] Not expected response staus code: " +
                                  $"Request path: {requestPath} \n" +
                                  $"StatusCode: {(int)apiHttpResponse.StatusCode} {apiHttpResponse.StatusCode} \n" +
                                  $"Content: {apiResponseContent}");
                    throw new ArgumentOutOfRangeException();
            }
            
            return result;
        }

        private static HttpClient GetHttpClient(string baseAddress)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(_dataCollectorApiSecret);

            return client;
        }
    }

    public interface IAndroidDataCollectorClient
    {
        Task<FetchDataResponse> RequestForDeviceData(string deviceId);

        Task<MobileDataResponse> GetDeviceDataAsync(string fetchRequestId);
    }
}