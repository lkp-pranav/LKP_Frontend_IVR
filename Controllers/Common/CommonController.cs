using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Utils;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using LKP_Frontend_MVC.Models.Response.ClientDealer;
using System.Reflection;

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

        //[HttpPost]
        public async Task<IActionResult> GetZones()
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

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
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            var user = new CommonModel{ user_id = sessionUser.user_id, user_type = sessionUser.user_type };

            var response = await LoginHelper.SendHttpRequest(
                _httpClient,
                $"https://localhost:7121/api/BranchCNT/GetDealerBasisZone?Zone={Zone}",
                user,
                "Bearer",
                sessionUser.accessToken
            );
            var dealerList = (response.data as JArray)?.ToObject<List<DealerResponse>>();
            response.data = dealerList;
            
            return Json(response);
        }

        public async Task<IActionResult> FetchBranch(string Zone)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            var user = new CommonModel { user_id = sessionUser.user_id, user_type = sessionUser.user_type };

            var response = await LoginHelper.SendHttpRequest(
                _httpClient,
                $"https://localhost:7121/api/CustomGroup/FetchBranch?zone={Zone}",
                user,
                "Bearer",
                sessionUser.accessToken
            );

            var branchList = (response.data as JArray)?.ToObject<List<string>>();
            response.data = branchList;

            return Json(response);
        }

        public async Task<IActionResult> FetchDealerByBranch(string branchCode)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            var user = new CommonModel { user_id = sessionUser.user_id, user_type = sessionUser.user_type };

            var response = await LoginHelper.SendHttpRequest(
                _httpClient,
                $"https://localhost:7121/api/CustomGroup/FetchDealerByBranch?branchCode={branchCode}",
                user,
                "Bearer",
                sessionUser.accessToken
            );

            var dealerList = (response.data as JArray)?.ToObject<List<DealerResponse>>();
            response.data = dealerList;

            return Json(response);
        }

        public async Task<IActionResult> FetchDealerByGroup(string groupCode)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            var user = new CommonModel { user_id = sessionUser.user_id, user_type = sessionUser.user_type };

            var response = await LoginHelper.SendHttpRequest(
                _httpClient,
                $"https://localhost:7121/api/CustomGroup/FetchDealerByGroup?groupCode={groupCode}",
                user,
                "Bearer",
                sessionUser.accessToken
            );

            var dealerList = (response.data as JArray)?.ToObject<List<GroupDealerInfo>>();
            response.data = dealerList;

            return Json(response);
        }

        public async Task<IActionResult> GetDealerSegment(string dealer)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            var user = new CommonModel { user_id = sessionUser.user_id, user_type = sessionUser.user_type };

            ResponsePayLoad payLoad = new ResponsePayLoad();

            var response = await LoginHelper.SendHttpRequest(
                _httpClient,
                $"https://localhost:7121/api/DealerCNT/GetDealerSegment?dealer={dealer}",
                user,
                "Bearer",
                sessionUser.accessToken
            );
            var list = (response.data as JArray)?.ToObject<List<DealerSegmentResponse>>();
            response.data = list;


            return Json(response);
        }
    }
}
