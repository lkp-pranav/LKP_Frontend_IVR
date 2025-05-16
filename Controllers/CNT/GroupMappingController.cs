using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Request.GroupMapping;
using LKP_Frontend_MVC.Models.Response.ClientDealer;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.GroupCNT;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Policy;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class GroupMappingController : Controller
    {
        #region Fields
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private readonly string baseURL = "";
        #endregion

        #region Constructor
        public GroupMappingController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            baseURL = _Configuration["ApiSettings:BaseUrl"];
        }
        #endregion

        #region Methods
        // CREATE: Group Mapping
        public async Task<IActionResult> Index(GroupFilterModel inputModel)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            var responsePayLoad = await RequestHelper.SendHttpRequest(
                _httpClient,
                $"{baseURL}/api/GroupCNT/GetAllMapping",
                inputModel,
                "Bearer",
                sessionUser.accessToken
            );

            if (responsePayLoad == null || !responsePayLoad.isSuccess)
            {
                return View(new List<GroupCNTResponse>());
            }

            var model = JsonConvert.DeserializeObject<List<GroupCNTResponse>>(responsePayLoad.data.ToString());
            responsePayLoad.data = model;
            responsePayLoad.message = inputModel;

            return View(responsePayLoad);
        }
        #endregion
    }
}
