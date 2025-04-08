using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Response.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace LKP_Frontend_MVC.Utils
{
    public class LoginHelper
    {
        public static async Task<ResponsePayLoad?> SendHttpRequest(HttpClient _httpClient, string url, EncryptedDataInput data, string authType, string authToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authType, authToken);

                var requestBody = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponsePayLoad>(responseJson);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"HTTP Request Failed: {ex.Message}");
                return null;
            }
        }

        public static async Task<ResponsePayLoad?> SendHttpRequest(HttpClient _httpClient, string url, string authType, string authToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authType, authToken);
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponsePayLoad>(responseJson);
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"HTTP Request Failed: {ex.Message}");
                return null;
            }
            
        }


        public static T? DeserializeEncryptedData<T>(string encryptedData, string _encKey) where T : class
        {
            try
            {
                string decodedData = CommonHelper.Decrypt(encryptedData, true, _encKey);
                return JsonConvert.DeserializeObject<T>(decodedData);
            }
            catch
            {
                Console.WriteLine("Failed to decrypt/deserialize data.");
                return null;
            }
        }
    }
}
