using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Response.ClientDealer;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.CTCLMapping;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class CTCLMappingController : Controller
    {
        private readonly HttpClient _httpClient;

        public CTCLMappingController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(PageInputModel inputModel)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<CTCLMappingResponse> model = new List<CTCLMappingResponse>();
            inputModel.Category = inputModel.Category ?? "ALL";
            string url = "https://localhost:7121/api/CTCLMapping/GetAllMapping";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, inputModel, "Bearer", sessionUser.accessToken);

            if (responsePayLoad == null || !responsePayLoad.isSuccess)
            {
                return View(new List<CTCLMappingResponse>());
            }

            model = JsonConvert.DeserializeObject<List<CTCLMappingResponse>>(responsePayLoad.data.ToString());
            responsePayLoad.data = model;
            responsePayLoad.message = inputModel;

            return View(responsePayLoad);
        }
    }
}
