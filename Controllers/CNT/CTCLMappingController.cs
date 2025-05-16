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
        #region Fields
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private readonly string baseURL = "";
        #endregion

        #region Constructor
        public CTCLMappingController(IConfiguration configuration,HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            baseURL = _Configuration["ApiSettings:BaseUrl"];
        }
        #endregion

        #region Methods
        // GET: CTCL Mappings
        public async Task<IActionResult> Index(CTCLMappingFilterModel inputModel)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            inputModel.Category = inputModel.Category ?? "ALL";
            
            var responsePayLoad = await RequestHelper.SendHttpRequest(
                _httpClient,
                $"{baseURL}/api/CTCLMapping/GetAllMapping", 
                inputModel, 
                "Bearer", 
                sessionUser.accessToken
            );

            if (responsePayLoad == null || !responsePayLoad.isSuccess)
            {
                return View(new List<CTCLMappingResponse>());
            }

            var model = JsonConvert.DeserializeObject<List<CTCLMappingResponse>>(responsePayLoad.data.ToString());
            responsePayLoad.data = model;
            responsePayLoad.message = inputModel;

            return View(responsePayLoad);
        }
        #endregion
    }
}
