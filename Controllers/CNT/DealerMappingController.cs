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

        public async Task<IActionResult> CreateMapping(DealerCNTModel dealerCNTModel)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            //string[] primary = dealerCNTModel.PrimaryDealer.Split("--");
            //string[] secondary = dealerCNTModel.SecondaryDealer.Split("--");
            //string primaryDealerID = primary[0].Trim();
            //string primaryDealerName = primary[1].Trim();
            //string secondaryDealerID = primary[0].Trim();
            //string secondaryDealerName = primary[1].Trim();

            var payload = new DealerCNTInputModel
            {
                User_id = sessionUser.User_id,
                User_type = sessionUser.User_type,
                Zone = dealerCNTModel.Zone,
                PrimaryDealer = dealerCNTModel.PrimaryDealer,
                PrimaryDealerCTCLLogin = dealerCNTModel.PrimaryCTCL,
                SecondaryDealer = dealerCNTModel.SecondaryDealer,
                SecondaryDealerCTCLLogin = dealerCNTModel.SecondaryCTCL
            };

            ResponsePayLoad response = await LoginHelper.SendHttpRequest(_httpClient, "https://localhost:7121/api/DealerCNT/CreateDealerCNTMapping", payload, "Bearer", sessionUser.accessToken);
            Console.WriteLine(payload);
            return RedirectToAction("Index");
        }
    }
}
