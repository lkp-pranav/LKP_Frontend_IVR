using LKP_Frontend_MVC.Models.Request.Common;
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

        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;

        public GroupMappingController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
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
            List<GroupCNTResponse> model = new List<GroupCNTResponse>();

            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient,"https://localhost:7121/api/GroupCNT/GetAllMapping",inputModel,"Bearer",sessionUser.accessToken);

            if (responsePayLoad == null || !responsePayLoad.isSuccess)
            {

                return View(new List<GroupCNTResponse>());
            }

            model = JsonConvert.DeserializeObject<List<GroupCNTResponse>>(responsePayLoad.data.ToString());
            responsePayLoad.data = model;
            responsePayLoad.message = inputModel;

            return View(responsePayLoad);
        }
    }
}
