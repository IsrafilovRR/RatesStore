using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Utils
{
    public class HttpHelper
    {
        public static T Get<T>(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    var responseData = JsonConvert.DeserializeObject<T>(content);
                    return responseData;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbLogger.LogMessage("The request has been processed correctly");
            }
        }
        public async static Task<T> GetAsync<T>(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<T>(content);
                    return responseData;
                }
            }
            catch (InvalidOperationException ex)
            {
                var message = "The request message was already sent by the HttpClient instance";
                DbLogger.LogError(ex, message);
                return default;
            }
            catch (HttpRequestException ex)
            {
                var message = @"The request failed due to an underlying issue such as network connectivity,
                    DNS failure, server certificate validation or timeout.";
                DbLogger.LogError(ex, message);
                return default;
            }
            catch (TaskCanceledException ex)
            {
                var message = "The request timed-out or the user canceled the request's Task.";
                DbLogger.LogError(ex, message);
                return default;
            }
            finally
            {
                DbLogger.LogMessage("The request has been processed correctly");
            }
        }
    }
}
