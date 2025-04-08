using LKP_Frontend_MVC.Models.Response.ClientDealer;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class ClientDealerController : Controller
    {

        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;

        public ClientDealerController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 50)
        {
            if(HttpContext.Session.GetString("encrypted2FAData") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int start = ((page - 1) * pageSize) + 1;

            string bearerKey = HttpContext.Session.GetString("bearerKey");
            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<ClientDealerResponse> model = new List<ClientDealerResponse>();

            string url = $"https://localhost:7121/api/ClientDealer/GetAllMapping?start={start}&pageSize={pageSize}";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, "Bearer", bearerKey);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            model = JsonConvert.DeserializeObject<List<ClientDealerResponse>>(responsePayLoad.data.ToString());
            responsePayLoad.data = model;
            Console.WriteLine(responsePayLoad.data.ToString());


            return View(responsePayLoad);
        }
    }
}
