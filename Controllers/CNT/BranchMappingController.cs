﻿using LKP_Frontend_MVC.Models.Request.BranchCNT;
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
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private string _encKey = "";

        public BranchMappingController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            _encKey = _Configuration.GetSection("encKey").Value;
        }

        
        public async Task<IActionResult> Index(PageInputModel inputModel)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            inputModel.user_id = sessionUser.user_id;
            inputModel.user_type = sessionUser.user_type;

            int start = ((inputModel.Start - 1) * inputModel.PageSize) + 1;

            ResponsePayLoad responsePayLoad = new ResponsePayLoad();
            List<BranchCNTResponse> model = new List<BranchCNTResponse>();

            string url = $"https://localhost:7121/api/BranchCNT/GetAllBranchCNTMapping";
            responsePayLoad = await LoginHelper.SendHttpRequest(_httpClient, url, inputModel,"Bearer", sessionUser.accessToken);

            if(responsePayLoad == null || !responsePayLoad.isSuccess)
            {
                return View("Error");
            }

            ViewBag.CurrentPage = inputModel.Start;
            ViewBag.PageSize = inputModel.PageSize;

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

            branchCNTInput.user_id = sessionUser.user_id;
            branchCNTInput.user_type = sessionUser.user_type;
            branchCNTInput.IsHOCNT = IsHOCNT;

            ResponsePayLoad response = await LoginHelper.SendHttpRequest(_httpClient,"https://localhost:7121/api/BranchCNT/CreateBranchCNTMapping", branchCNTInput, "Bearer", sessionUser.accessToken);

            if (response == null || !response.isSuccess)
            {
                TempData["ErrorMessage"] = response?.errorMessages ?? "An unexpected error occurred.";
                TempData["ShowToast"] = true;
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Mapping created successfully.";
            TempData["ShowToast"] = true;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMapping(int rowId)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionUser.accessToken);

            var user = new CommonModel { user_id = sessionUser.user_id, user_type = sessionUser.user_type };

            var responsePayload = await LoginHelper.SendHttpRequest(
                _httpClient,
                $"https://localhost:7121/api/BranchCNT/DeleteBranchCNTMapping?rowId={rowId}",
                user,
                "Bearer",
                sessionUser.accessToken
            );

            if (responsePayload == null || !responsePayload.isSuccess)
            {
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMapping(BranchCNTInputModel branchCNTInput)
        {
            string sessionUserJson = HttpContext.Session.GetString("sessionUser");
            if (sessionUserJson == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);

            int IsHOCNT = branchCNTInput.Zone == "H.O." ? 1 : 0;

            branchCNTInput.user_id = sessionUser.user_id;
            branchCNTInput.user_type = sessionUser.user_type;
            branchCNTInput.IsHOCNT = IsHOCNT;

            ResponsePayLoad response = await LoginHelper.SendHttpRequest(_httpClient, "https://localhost:7121/api/BranchCNT/UpdateBranchCNTMapping", branchCNTInput, "Bearer", sessionUser.accessToken);

            if (response == null || !response.isSuccess)
            {
                TempData["ErrorMessage"] = response?.errorMessages ?? "An unexpected error occurred.";
                TempData["ShowToast"] = true;
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = response.message ?? "Mapping updated successfully";
            TempData["ShowToast"] = true;

            return RedirectToAction("Index");

        }

    }
}
