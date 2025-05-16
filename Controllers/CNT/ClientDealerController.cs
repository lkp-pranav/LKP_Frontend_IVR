using LKP_Frontend_MVC.Models.Request.ClientDealer;
using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Response.ClientDealer;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class ClientDealerController : Controller
    {
        #region Fields
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private readonly string baseURL = "";
        #endregion

        #region Constructor
        public ClientDealerController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            baseURL = _Configuration["ApiSettings:BaseUrl"];
        }
        #endregion

        #region Methods

        // GET: Client-Dealer Mapping
        public async Task<IActionResult> Index(ClientDealerFilterModel inputModel)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            var responsePayLoad = await RequestHelper.SendHttpRequest(
                _httpClient,
                $"{baseURL}/api/ClientDealer/GetAllMapping", 
                inputModel,
                "Bearer", 
                sessionUser.accessToken
            );

            if (responsePayLoad == null || !responsePayLoad.isSuccess)
            {
                return View(new List<ClientDealerResponse>());
            }

            var model = JsonConvert.DeserializeObject<List<ClientDealerResponse>>(responsePayLoad.data.ToString());

            inputModel.Category = inputModel.Category ?? "ALL";
            responsePayLoad.data = model;
            responsePayLoad.message = inputModel;
            return View(responsePayLoad);
        }

        [HttpPost] // GET: Client Group Info
        public async Task<IActionResult> GetClientGroups(string clientCode)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;
            var user = new CommonModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type
            };

            var responsePayLoad = await RequestHelper.SendHttpRequest(
                _httpClient,
                $"{baseURL}/api/ClientDealer/GetClientGroups?clientCode={clientCode}",
                user, 
                "Bearer", 
                sessionUser.accessToken
            );

            var groupList = JsonConvert.DeserializeObject<ClientGroupResponse>(responsePayLoad.data.ToString());
            responsePayLoad.data = groupList;

            return Json(responsePayLoad);
        }

        [HttpPost] // EXPORT: Add Dealer Client CSV
        public async Task<IActionResult> AddDealerClientCSV()
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;
            var user = new CommonModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type
            };

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionUser.accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

            var response = await _httpClient.PostAsync($"{baseURL}/api/ClientDealer/GetUploadMapping?option=DealerAdd", content);

            if (!response.IsSuccessStatusCode)
                return Content("Failed to export data.");

            var stream = await response.Content.ReadAsStreamAsync();

            return File(stream, "application/octet-stream", "Add_ClientDealerMapping.csv");
        }

        [HttpPost] // EXPORT: Delete Dealer Client CSV
        public async Task<IActionResult> DeleteDealerClientCSV()
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;
            var user = new CommonModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type
            };

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionUser.accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

            var response = await _httpClient.PostAsync($"{baseURL}/api/ClientDealer/GetUploadMapping?option=DealerDelete", content);

            if (!response.IsSuccessStatusCode)
                return Content("Failed to export data.");

            var stream = await response.Content.ReadAsStreamAsync();

            return File(stream, "application/octet-stream", "Delete_ClientDealerMapping.csv");
        }
        #endregion
    }
}
