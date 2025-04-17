using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Response.ClientDealer;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.User;
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
   
        public async Task<IActionResult> Index(PageInputModel inputModel)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            int start = ((inputModel.Start - 1) * inputModel.PageSize) + 1;

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<ClientDealerResponse> model = new List<ClientDealerResponse>();

            string url = $"https://localhost:7121/api/ClientDealer/GetAllMapping";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, inputModel,"Bearer", sessionUser.accessToken);

            if (responsePayLoad == null || !responsePayLoad.isSuccess)
            {
                return View("Error");
            }

            ViewBag.CurrentPage = inputModel.Start;
            ViewBag.PageSize = inputModel.PageSize;

            model = JsonConvert.DeserializeObject<List<ClientDealerResponse>>(responsePayLoad.data.ToString());
            responsePayLoad.data = model;
            Console.WriteLine(responsePayLoad.data.ToString());


            return View(responsePayLoad);
        }
    }
}
