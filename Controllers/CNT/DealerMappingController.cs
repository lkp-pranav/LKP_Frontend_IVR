using LKP_Frontend_MVC.Models.Request.BranchCNT;
using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Request.DealerCNT;
using LKP_Frontend_MVC.Models.Response.BranchCNT;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.DealerCNT;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class DealerMappingController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;

        public DealerMappingController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
        }

       
        public async Task<IActionResult> Index(PageInputModel inputModel)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson== null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            int start = ((inputModel.Start - 1) * inputModel.PageSize) + 1;

            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<DealerCNTResponse> model = new List<DealerCNTResponse>();

            string url = $"https://localhost:7121/api/DealerCNT/GetAllDealerCNTMapping";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, inputModel, "Bearer", sessionUser.accessToken);

            if (responsePayLoad == null || !responsePayLoad.isSuccess)
            {
                return View("Error");
            }

            ViewBag.CurrentPage = inputModel.Start;
            ViewBag.PageSize = inputModel.PageSize;

            model = JsonConvert.DeserializeObject<List<DealerCNTResponse>>(responsePayLoad.data.ToString());

            responsePayLoad.data = model;

            return View(responsePayLoad);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMapping(DealerCNTInputModel dealerCNTModel)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            dealerCNTModel.user_id = sessionUser.user_id;
            dealerCNTModel.user_type = sessionUser.user_type;   
            
            ResponsePayLoad response = await LoginHelper.SendHttpRequest(_httpClient, "https://localhost:7121/api/DealerCNT/CreateDealerCNTMapping", dealerCNTModel, "Bearer", sessionUser.accessToken);
            if (response == null || !response.isSuccess)
            {
                TempData["ErrorMessage"] = response?.errorMessages ?? "An unexpected error occurred.";
                TempData["ShowToast"] = true;
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Mapping created successfully.";
            TempData["ShowToast"] = true;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMapping(int rowId)
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
                $"https://localhost:7121/api/DealerCNT/DeleteDealerCNTMapping?rowId={rowId}",
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

        [HttpPost]
        public async  Task<IActionResult> UpdateMapping(DealerCNTInputModel dealerCNTModel)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            dealerCNTModel.user_id = sessionUser.user_id;
            dealerCNTModel.user_type = sessionUser.user_type;

            ResponsePayLoad response = await LoginHelper.SendHttpRequest(_httpClient, "https://localhost:7121/api/DealerCNT/UpdateDealerCNTMapping", dealerCNTModel, "Bearer", sessionUser.accessToken);

            if (response == null || !response.isSuccess)
            {
                TempData["ErrorMessage"] = response?.errorMessages ?? "An unexpected error occurred.";
                TempData["ShowToast"] = true;
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Mapping created successfully.";
            TempData["ShowToast"] = true;
            return RedirectToAction("Index");
        }
    }
}
