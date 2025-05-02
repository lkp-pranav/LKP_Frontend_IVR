using LKP_Frontend_MVC.Models.Request.ClientDealer;
using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Response.ClientDealer;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class ClientDealerController : Controller
    {

        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;

        public ClientDealerController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
        }
   
        public async Task<IActionResult> Index(ClientDealerFilterModel inputModel)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<ClientDealerResponse> model = new List<ClientDealerResponse>();

            string url = $"https://localhost:7121/api/ClientDealer/GetAllMapping";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, inputModel,"Bearer", sessionUser.accessToken);

            if (responsePayLoad == null || !responsePayLoad.isSuccess)
            {
               
                return View(new List<ClientDealerResponse>());
            }
            model = JsonConvert.DeserializeObject<List<ClientDealerResponse>>(responsePayLoad.data.ToString());
            inputModel.Category = inputModel.Category ?? "ALL";
            responsePayLoad.data = model;
            responsePayLoad.message = inputModel;

            return View(responsePayLoad);
        }

        [HttpPost]
        public async Task<IActionResult> GetClientGroups(string clientCode)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;
            var user = new CommonModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type
            };

            ClientGroupResponse groupList = new ClientGroupResponse();

            string url = $"https://localhost:7121/api/ClientDealer/GetClientGroups?clientCode={clientCode}";
            var responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, user, "Bearer", sessionUser.accessToken);

            groupList = JsonConvert.DeserializeObject<ClientGroupResponse>(responsePayLoad.data.ToString());
            responsePayLoad.data = groupList;

            return Json(responsePayLoad);
        }
    }
}
