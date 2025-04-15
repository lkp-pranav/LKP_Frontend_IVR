using LKP_Frontend_MVC.Models.Request.BranchCNT;
using LKP_Frontend_MVC.Models.Response.BranchCNT;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using LKP_Frontend_MVC.Models.Response.User;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class BranchMappingController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private string _encKey = "";

        public BranchMappingController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            _encKey = _Configuration.GetSection("encKey").Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 50)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            int start = ((page - 1) * pageSize) + 1;

            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<BranchCNTResponse> model = new List<BranchCNTResponse>();

            string url = $"https://localhost:7121/api/BranchCNT/GetAllBranchCNTMapping?start={start}&pageSize={pageSize}";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, "Bearer", sessionUser.accessToken);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            model = JsonConvert.DeserializeObject<List<BranchCNTResponse>>(responsePayLoad.data.ToString());

            responsePayLoad.data = model;
            
            return View(responsePayLoad);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMapping(BranchCNTInputModel branchCNTInput)
        {
            
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            int IsHOCNT = branchCNTInput.Zone == "H.O." ? 1 : 0;

            branchCNTInput.User_id = sessionUser.User_id;
            branchCNTInput.User_type = sessionUser.User_type;
            branchCNTInput.IsHOCNT = IsHOCNT;

            ResponsePayLoad response = await LoginHelper.SendHttpRequest(_httpClient,"https://localhost:7121/api/BranchCNT/CreateBranchCNTMapping", branchCNTInput, "Bearer", sessionUser.accessToken);

            if (response == null || !response.isSuccess)
            {
                return View("Error");
            }

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
                $"https://localhost:7121/api/BranchCNT/DeleteBranchCNTMapping?rowId={rowId}"
            );

            if (responsePayload == null || !responsePayload.isSuccess)
            {
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateMapping(BranchCNTInputModel branchCNTInput)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            int IsHOCNT = branchCNTInput.Zone == "H.O." ? 1 : 0;

            branchCNTInput.User_id = sessionUser.User_id;
            branchCNTInput.User_type = sessionUser.User_type;
            branchCNTInput.IsHOCNT = IsHOCNT;

            ResponsePayLoad response = await LoginHelper.SendHttpRequest(_httpClient, "https://localhost:7121/api/BranchCNT/UpdateBranchCNTMapping", branchCNTInput, "Bearer", sessionUser.accessToken);

            if (response == null || !response.isSuccess)
            {
                return View("Error");
            }

            return RedirectToAction("Index");

        }

    }
}
