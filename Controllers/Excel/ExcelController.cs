using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Response.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LKP_Frontend_MVC.Controllers.Excel
{
    public class ExcelController : Controller
    {
        private readonly HttpClient _httpClient;

        public ExcelController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> ExportClientDealerExcel()
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            var user = new PageInputModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type,
                Start = 1,
                PageSize = 1000000
            };

            // Serialize CommonModel to JSON string
            var json = JsonConvert.SerializeObject(user);

            // Create StringContent with JSON data
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Send POST request with CommonModel data
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionUser.accessToken);
            var response = await _httpClient.PostAsync("https://localhost:7121/api/Excel/ExportClientDealerExcel", content);

            if (!response.IsSuccessStatusCode)
                return Content("Failed to export data.");

            var stream = await response.Content.ReadAsStreamAsync();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClientDealerList.xlsx");
        }


        [HttpPost]
        public async Task<IActionResult> ExportGroupMappingExcel()
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            var user = new PageInputModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type,
                Start = 1,
                PageSize = 1000000
            };

            // Serialize CommonModel to JSON string
            var json = JsonConvert.SerializeObject(user);

            // Create StringContent with JSON data
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Send POST request with CommonModel data
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionUser.accessToken);
            var response = await _httpClient.PostAsync("https://localhost:7121/api/Excel/ExportGroupMappingExcel", content);

            if (!response.IsSuccessStatusCode)
                return Content("Failed to export data.");

            var stream = await response.Content.ReadAsStreamAsync();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GroupMappingList.xlsx");
        }

    }
}
