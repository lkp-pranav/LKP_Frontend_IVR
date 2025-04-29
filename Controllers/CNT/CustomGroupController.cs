using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Request.CustomGroup;
using LKP_Frontend_MVC.Models.Response.BranchCNT;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.CustomGroup;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class CustomGroupController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private string _encKey = "";

        public CustomGroupController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            _encKey = _Configuration.GetSection("encKey").Value;
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
            List<CustomGroupResponse> model = new List<CustomGroupResponse>();
            string url = $"https://localhost:7121/api/CustomGroup/GetAllCustomGroups";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, inputModel, "Bearer", sessionUser.accessToken);

            if (responsePayLoad == null || !responsePayLoad.isSuccess)
            {
                return View("Error");
            }

            ViewBag.CurrentPage = inputModel.Start;
            ViewBag.PageSize = inputModel.PageSize;

            model = JsonConvert.DeserializeObject<List<CustomGroupResponse>>(responsePayLoad.data.ToString());
            responsePayLoad.data = model;
            responsePayLoad.message = inputModel;

            return View(responsePayLoad);
        }

        public async Task<IActionResult> CreateCustomGroup([FromBody] CustomGroupInputModel inputModel)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;
            ResponsePayLoad response = await LoginHelper.SendHttpRequest(
                _httpClient,
                "https://localhost:7121/api/CustomGroup/CreateCustomGroup", 
                inputModel, "Bearer", 
                sessionUser.accessToken
            );

            if (response == null || !response.isSuccess)
            {
                TempData["ErrorMessage"] = response?.errorMessages ?? "An unexpected error occurred.";
                TempData["ShowToast"] = true;
                return Json(new { success = false, message = response?.errorMessages ?? "An unexpected error occurred." });
            }

            return Json(new { success = true, message = "Mapping created successfully." });
        }

        public async Task<IActionResult> UpdateCustomGroup([FromBody] CustomGroupInputModel inputModel)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;
            ResponsePayLoad response = await LoginHelper.SendHttpRequest(
                _httpClient,
                "https://localhost:7121/api/CustomGroup/UpdateCustomGroup",
                inputModel, "Bearer",
                sessionUser.accessToken
            );

            if (response == null || !response.isSuccess)
            {
                return Json(new { success = false, message = response?.errorMessages ?? "An unexpected error occurred." });
            }

            return Json(new { success = true, message = "Mapping created successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomGroup(string groupCode)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            var user = new CommonModel { user_id = sessionUser.user_id, user_type = sessionUser.user_type };

            var responsePayload = await LoginHelper.SendHttpRequest(
                _httpClient,
                $"https://localhost:7121/api/CustomGroup/DeleteCustomGroup?groupCode={groupCode}",
                user,
                "Bearer",
                sessionUser.accessToken
            );

            if (responsePayload == null || !responsePayload.isSuccess)
            {
                return View("Error");
            }

            return RedirectToAction("Index");
        }
    }
}
