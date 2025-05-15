using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Request.CTCLMapping;
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
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private readonly string baseURL = "";

        public CTCLMappingController(IConfiguration configuration,HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            baseURL = _Configuration["ApiSettings:BaseUrl"];
        }

        public async Task<IActionResult> Index(CTCLMappingFilterModel inputModel)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<CTCLMappingResponse> model = new List<CTCLMappingResponse>();
            inputModel.Category = inputModel.Category ?? "ALL";
            string url = $"{baseURL}/api/CTCLMapping/GetAllMapping";
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
