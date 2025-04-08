using LKP_Frontend_MVC.Models.Response.BranchCNT;
using LKP_Frontend_MVC.Models.Response.ClientDealer;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class BranchMappingController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;

        public BranchMappingController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 50)
        {
            if (HttpContext.Session.GetString("encrypted2FAData") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int start = ((page - 1) * pageSize) + 1;

            string bearerKey = HttpContext.Session.GetString("bearerKey");
            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<BranchCNTResponse> model = new List<BranchCNTResponse>();

            string url = $"https://localhost:7121/api/BranchCNT/GetAllBranchCNTMapping?start={start}&pageSize={pageSize}";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, "Bearer", bearerKey);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            model = JsonConvert.DeserializeObject<List<BranchCNTResponse>>(responsePayLoad.data.ToString());

            responsePayLoad.data = model;
            
            return View(responsePayLoad);
        }


        public async Task<IActionResult> GetZones()
        {
            string bearerKey = HttpContext.Session.GetString("bearerKey");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerKey);
            var response = await _httpClient.GetFromJsonAsync<ResponsePayLoad>("https://localhost:7121/api/BranchCNT/GetZoneList");
            return Json(response);
        }

        public async Task<IActionResult> GetDealerByZone(string Zone)
        {
            string bearerKey = HttpContext.Session.GetString("bearerKey");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerKey);
            var response = await _httpClient.GetFromJsonAsync<ResponsePayLoad>($"https://localhost:7121/api/BranchCNT/GetDealerBasisZone?Zone={Zone}");
            return Json(response);
        }

    }
}
