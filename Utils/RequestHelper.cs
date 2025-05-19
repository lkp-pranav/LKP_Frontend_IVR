using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Response.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using LKP_Frontend_MVC.Models.Response.User;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

namespace LKP_Frontend_MVC.Utils
{
    public class RequestHelper
    {
        [HttpPost]
        public static async Task<ResponsePayLoad?> SendHttpRequest<T>(HttpClient httpClient, string url, T data, string authType, string authToken)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrWhiteSpace(authToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
                }

                using var response = await httpClient.SendAsync(request);

                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ResponsePayLoad
                    {
                        isSuccess = false,
                        message = "Login Failed",
                        errorMessages = "Please enter the correct credentials",
                        statusCode = HttpStatusCode.Unauthorized
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponsePayLoad
                    {
                        isSuccess = false,
                        message = $"Error: {response.ReasonPhrase}",
                        errorMessages = responseJson,
                        statusCode = response.StatusCode
                    };
                }

                var payload = JsonConvert.DeserializeObject<ResponsePayLoad>(responseJson);
                payload.statusCode = response.StatusCode;
                return payload;
            }
            catch (Exception ex)
            {
                
                return new ResponsePayLoad
                {
                    isSuccess = false,
                    message = "Exception occurred during API request.",
                    errorMessages = ex.Message,
                    statusCode = HttpStatusCode.InternalServerError
                };
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
                return null;
            }
        }
    }
}
