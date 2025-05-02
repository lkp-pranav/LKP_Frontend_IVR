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

        public async Task<IActionResult> Index(CustomGroupFilterModel inputModel)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            inputModel.Zone = inputModel.Zone?.Trim() ?? "";
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

            model = JsonConvert.DeserializeObject<List<CustomGroupResponse>>(responsePayLoad.data.ToString());
            responsePayLoad.data = model;
            responsePayLoad.message = inputModel;

            return View(responsePayLoad);
        }

        public async Task<IActionResult> CreateCustomGroup([FromBody] CustomGroupInputModel inputModel)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

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
                TempData["ToastMessage"] = response?.errorMessages ?? "Error in creating Custom Group!!.";
                TempData["ToastType"] = "danger";
                return Json(new { success = false, message = response?.errorMessages ?? "An unexpected error occurred." });
            }

            TempData["ToastMessage"] = response.message ?? "Custom Group created successfully!!.";
            TempData["ToastType"] = "success";
            return Json(new { success = true, message = "Mapping created successfully." });
        }

        public async Task<IActionResult> UpdateCustomGroup([FromBody] CustomGroupInputModel inputModel)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

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
                TempData["ToastMessage"] = response?.errorMessages ?? "Error in updating Custom Group!!";
                TempData["ToastType"] = "danger";
                return Json(new { success = false, message = response?.errorMessages ?? "An unexpected error occurred." });
            }

            TempData["ToastMessage"] = response?.errorMessages ?? "Custom Group Updated Successfully!!";
            TempData["ToastType"] = "success";
            return Json(new { success = true, message = "Mapping created successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomGroup(string groupCode)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

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
                TempData["ToastMessage"] = responsePayload?.errorMessages ?? "Error in updating Custom Group!!";
                TempData["ToastType"] = "danger";
                return View("Error");
            }

            TempData["ToastMessage"] = responsePayload.errorMessages ?? "Custom Group deleted Successfully!!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index");
        }
    }
}
