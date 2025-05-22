using LKP_Frontend_MVC.Models.Request.BranchCNT;
using LKP_Frontend_MVC.Models.Response.BranchCNT;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Controllers.CNT
{
    public class BranchMappingController : Controller
    {
        #region Fields
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private readonly string baseURL = "";
        private string _encKey = "";
        #endregion

        #region Constructor
        public BranchMappingController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            _encKey = _Configuration.GetSection("encKey").Value;
            baseURL = _Configuration["ApiSettings:BaseUrl"];
        }
        #endregion

        #region Methods
        // GET: Branch CNT Mapping
        public async Task<IActionResult> Index(BranchCNTFilterModel inputModel)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            inputModel.Zone = inputModel.Zone?.Trim() ?? "";
            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            var responsePayLoad = await RequestHelper.SendHttpRequest(
                _httpClient,
                $"{baseURL}/api/BranchCNT/GetAllBranchCNTMapping", 
                inputModel,
                "Bearer",
                sessionUser.accessToken
            );

            if(responsePayLoad == null || !responsePayLoad.isSuccess)
            {
                return View("Error");
            }

            var model = JsonConvert.DeserializeObject<List<BranchCNTResponse>>(responsePayLoad.data.ToString());
            responsePayLoad.data = model;
            responsePayLoad.message = inputModel;
            
            return View(responsePayLoad);
        }

        [HttpPost] // CREATE: Branch CNT Mapping
        public async Task<IActionResult> CreateMapping(BranchCNTInputModel branchCNTInput)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            branchCNTInput.user_id = sessionUser.user_id;
            branchCNTInput.user_type = sessionUser.user_type;

            var response = await RequestHelper.SendHttpRequest(
                _httpClient,
                $"{baseURL}/api/BranchCNT/CreateBranchCNTMapping", 
                branchCNTInput,
                "Bearer", 
                sessionUser.accessToken
            );

            if (response == null || !response.isSuccess)
            {
                TempData["ToastMessage"] = response?.errorMessages ?? "Error in creating Branch Mapping!!.";
                TempData["ToastType"] = "danger";
                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = response.message ?? "Branch mapping created successfully!!.";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost] // DELETE: Branch CNT Mapping
        public async Task<IActionResult> DeleteMapping(int rowId)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            var responsePayload = await RequestHelper.SendHttpRequest(
                _httpClient,
                $"{baseURL}/api/BranchCNT/DeleteBranchCNTMapping?rowId={rowId}",
                sessionUser,
                "Bearer",
                sessionUser.accessToken
            );

            if (responsePayload == null || !responsePayload.isSuccess)
            {
                TempData["ToastMessage"] = responsePayload?.errorMessages ?? "Error in deleting Branch Mapping!!.";
                TempData["ToastType"] = "danger";
                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = responsePayload.message ?? "Branch mapping deleted successfully!!.";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost] // UPDATE: Branch CNT Mapping
        public async Task<IActionResult> UpdateMapping(BranchCNTInputModel branchCNTInput)
        {
            var sessionUser = HttpContext.Items["SessionUser"] as SessionUser;

            branchCNTInput.user_id = sessionUser.user_id;
            branchCNTInput.user_type = sessionUser.user_type;

            var response = await RequestHelper.SendHttpRequest(
                _httpClient,
                $"{baseURL}/api/BranchCNT/UpdateBranchCNTMapping",
                branchCNTInput, 
                "Bearer", 
                sessionUser.accessToken
            );

            if (response == null || !response.isSuccess)
            {
                TempData["ToastMessage"] = response?.errorMessages ?? "Error in updating Branch Mapping!!.";
                TempData["ToastType"] = "danger";
                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = response.message ?? "Branch mapping Updated successfully!!.";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index");
        }
        #endregion

    }
}
