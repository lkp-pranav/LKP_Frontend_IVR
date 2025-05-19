using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace LKP_Frontend_MVC.Controllers.Excel
{
    public class ExcelController : Controller
    {
        #region Fields
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private readonly string baseURL = "";
        #endregion

        #region Constructor
        public ExcelController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            baseURL = _Configuration["ApiSettings:BaseUrl"];
        }
        #endregion

        #region Methods
        [HttpPost] // EXPORT: Client Dealer Excel
        public async Task<IActionResult> ExportClientDealerExcel()
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            var user = new PageInputModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type,
                Start = 1,
                PageSize = 1000000
            };

            var stream = await RequestHelper.CreateExcel(_httpClient, $"{baseURL}/api/Excel/ExportClientDealerExcel", user, sessionUser.accessToken);

            if (stream == null) return Content("Failed to export data.");

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClientDealerList.xlsx");
        }


        [HttpPost] // EXPORT: Group Mapping Excel
        public async Task<IActionResult> ExportGroupMappingExcel()
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            var user = new PageInputModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type,
                Start = 1,
                PageSize = 1000000
            };

            var stream = await RequestHelper.CreateExcel(_httpClient, $"{baseURL}/api/Excel/ExportGroupMappingExcel", user, sessionUser.accessToken);

            if (stream == null) return Content("Failed to export data.");

            return File( stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GroupMapping.xlsx");
        }

        [HttpPost] // EXPORT: Dealer Creation Excel
        public async Task<IActionResult> ExportDealerCreationFile()
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            var user = new CommonModel
            {
                user_id = sessionUser.user_id,
                user_type = sessionUser.user_type
            };

            var stream = await RequestHelper.CreateExcel(_httpClient, $"{baseURL}/api/Excel/ExportDealerCreationFile", user, sessionUser.accessToken);

            if (stream == null) return Content("Failed to export data.");

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Dealer_Creation.xlsx");
        }
        #endregion
    }
}
