using LKP_Frontend_MVC.Models.Response.BranchCNT;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.DealerCNT;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

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
            if (HttpContext.Session.GetString("encrypted2FAData") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int start = ((page - 1) * pageSize) + 1;

            string bearerKey = HttpContext.Session.GetString("bearerKey");
            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<DealerCNTResponse> model = new List<DealerCNTResponse>();

            string url = $"https://localhost:7121/api/DealerCNT/GetAllDealerCNTMapping?start={start}&pageSize={pageSize}";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, "Bearer", bearerKey);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            model = JsonConvert.DeserializeObject<List<DealerCNTResponse>>(responsePayLoad.data.ToString());

            responsePayLoad.data = model;

            return View(responsePayLoad);
        }
    }
}
