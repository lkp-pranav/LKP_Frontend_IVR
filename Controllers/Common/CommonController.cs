using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Utils;
using Newtonsoft.Json.Linq;

namespace LKP_Frontend_MVC.Controllers.Common
{
    public class CommonController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;

        public CommonController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
        }

        //public async Task<IActionResult> GetZones()
        //{
        //    string sessionUserJson = HttpContext.Session.GetString("sessionUser");
        //    if (sessionUserJson == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
        //    var user = new CommonModel
        //    {
        //        user_id = sessionUser.user_id,
        //        user_type = sessionUser.user_type
        //    };

        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionUser.accessToken);
        //    var response = await _httpClient.GetFromJsonAsync<ResponsePayLoad>("https://localhost:7121/api/BranchCNT/GetZoneList");
        //    return Json(response);
        //}


        [HttpPost]
        public async Task<IActionResult> GetZones()
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            var user = new CommonModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type
            };

            var response = await LoginHelper.SendHttpRequest(
                _httpClient,
                "https://localhost:7121/api/BranchCNT/GetZoneList",
                user,
                "Bearer",
                sessionUser.accessToken
            );
            var zoneList = (response.data as JArray)?.ToObject<List<string>>();
            response.data = zoneList;

            return Json(response);
        }


        public async Task<IActionResult> GetDealerByZone(string Zone)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionUser.accessToken);
            var response = await _httpClient.GetFromJsonAsync<ResponsePayLoad>($"https://localhost:7121/api/BranchCNT/GetDealerBasisZone?Zone={Zone}");
            return Json(response);
        }

        public async Task<IActionResult> GetDealerSegment(string dealer)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionUser.accessToken);
            var response = await _httpClient.GetFromJsonAsync<ResponsePayLoad>($"https://localhost:7121/api/DealerCNT/GetDealerSegment?dealer={dealer}");
            return Json(response);
        }
    }
}
