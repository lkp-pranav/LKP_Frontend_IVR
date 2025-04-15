using LKP_Frontend_MVC.Models.Request.BranchCNT;
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

        public async Task<IActionResult> Index(int page = 1, int pageSize = 50)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson== null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            int start = ((page - 1) * pageSize) + 1;

           
            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<DealerCNTResponse> model = new List<DealerCNTResponse>();

            string url = $"https://localhost:7121/api/DealerCNT/GetAllDealerCNTMapping?start={start}&pageSize={pageSize}";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, "Bearer", sessionUser.accessToken);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            model = JsonConvert.DeserializeObject<List<DealerCNTResponse>>(responsePayLoad.data.ToString());

            responsePayLoad.data = model;

            return View(responsePayLoad);
        }

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
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMapping(int rowId)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionUser.accessToken);

            var responsePayload = await _httpClient.DeleteFromJsonAsync<ResponsePayLoad>(
                $"https://localhost:7121/api/DealerCNT/DeleteDealerCNTMapping?rowId={rowId}"
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
            return RedirectToAction("Index");
        }
    }
}
