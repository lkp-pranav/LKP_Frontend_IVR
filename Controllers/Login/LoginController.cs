using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Request.Login;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace LKP_Frontend_MVC.Controllers.Login
{
    public class LoginController : Controller
    {
        #region Fields
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private readonly string _key;
        private readonly string _encKey;
        private readonly string baseURL = "";
        private readonly string lkpConnectURL = "";
        #endregion

        #region Constructor
        public LoginController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            _key = _Configuration.GetSection("SecurityKey").Value;
            _encKey = _Configuration.GetSection("encKey").Value;
            baseURL = _Configuration["ApiSettings:BaseUrl"];
            lkpConnectURL = _Configuration["ApiSettings:LkpConnect"];
        }
        #endregion

        #region Login
        [HttpGet] // Render Login Page
        public IActionResult Index() => View();


        [HttpPost] // Verify Login and redirect to 2FA
        public async Task<IActionResult> Login(LoginInputModel inputModel)
        {
            var resultJson = JsonConvert.SerializeObject(new
            {
                inputModel.User_type,
                inputModel.User_id,
                inputModel.User_password
            });

            string encryptedData = CommonHelper.Encrypt(resultJson, true, _encKey);
            HttpContext.Session.SetString("encryptedData", encryptedData);

            string base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"admin:admin"));
            var requestData = new EncryptedDataInput { Data = encryptedData };

            var responsePayload = await RequestHelper.SendHttpRequest(_httpClient, $"{baseURL}/api/Login/Login", requestData, "Basic", base64Credentials);

            if (responsePayload == null || !responsePayload.isSuccess)
            {
                TempData["ToastMessage"] = responsePayload?.errorMessages ?? "Error occured while loggin in !!";
                TempData["ToastType"] = "danger";
                return RedirectToAction("Index");
            }

            JObject jsonData = JObject.FromObject(responsePayload.data);
            string bearerKey = jsonData?["key"]?.ToString();
            HttpContext.Session.SetString("bearerKey", bearerKey);

            return RedirectToAction("VerifyPan");
        }
        #endregion

        #region 2FA-PAN
        // Render 2FA - PAN Page
        [HttpGet]
        public IActionResult VerifyPan()
        {
            if (HttpContext.Session.GetString("encryptedData") == null)
            {
                return RedirectToAction("Index");
            }


            return View();
        }

        // Verify 2FA - PAN and redirect to Home Page
        [HttpPost]
        public async Task<IActionResult> AuthenticatePAN(string pan)
        {
            string bearerKey = HttpContext.Session.GetString("bearerKey");
            string encryptedData = HttpContext.Session.GetString("encryptedData");

            // Remove base64 token after storing
            HttpContext.Session.Remove("bearerKey");
            HttpContext.Session.Remove("encryptedData");

            if (string.IsNullOrEmpty(bearerKey) || string.IsNullOrEmpty(encryptedData))
            {
                return RedirectToAction("VerifyPan");
            }

            var user = RequestHelper.DeserializeEncryptedData<TwoFactorAuthInputModel>(encryptedData, _encKey);
            if (user == null)
            {
                return RedirectToAction("VerifyPan");
            }

            user.User_id = user.User_type.ToLower() switch
            {
                "client" => $"CLI-{user.User_id}",
                "employee" => $"EMP-{user.User_id}",
                _ => $"APN-{user.User_id}"
            };
            user.Auth_value = pan;

            var resultJson = JsonConvert.SerializeObject(new
            {
                user.User_id,
                user.User_type,
                user.Auth_type,
                user.Auth_value
            });

            string encrypted2FAData = CommonHelper.Encrypt(resultJson, true, _encKey);
            var requestData = new EncryptedDataInput { Data = encrypted2FAData };
           
            var responsePayload = await RequestHelper.SendHttpRequest(_httpClient, $"{baseURL}/api/Login/ValidateTwoFactorAuthentication", requestData, "Bearer", bearerKey);

            if (responsePayload == null || !responsePayload.isSuccess || responsePayload.statusCode == HttpStatusCode.Unauthorized)
            {
                TempData["ToastMessage"] = responsePayload?.errorMessages ?? "Error in creating Branch Mapping!!.";
                TempData["ToastType"] = "danger";
                return RedirectToAction("VerifyPan");
            }
            
            JObject jsonData = JObject.FromObject(responsePayload.data);
            string username = jsonData?["name"]?.ToString();

            var sessionUser = new SessionUser
            {
                user_id = jsonData?["user_id"]?.ToString(),
                user_type = jsonData?["user_type"]?.ToString(),
                accessToken = jsonData?["accessToken"]?.ToString()
            };
            string sessionUserJson = JsonConvert.SerializeObject(sessionUser);
            HttpContext.Session.SetString("username", username);
            HttpContext.Session.SetString("sessionUser", sessionUserJson);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region SSO Login
        [HttpGet]
        public IActionResult SSOLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SSOLogin(SSOLoginModel inputModel)
        {

            var origin = Request.Headers["Origin"].ToString();

            if (string.IsNullOrEmpty(origin) || !origin.StartsWith(lkpConnectURL, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("Invalid origin.");
            }

            inputModel.User_id = inputModel.User_type.ToLower() switch
            {
                "client" => $"CLI-{inputModel.User_id}",
                "employee" => $"EMP-{inputModel.User_id}",
                _ => $"APN-{inputModel.User_id}"
            };

            var resultJson = JsonConvert.SerializeObject(inputModel);
            string encrypted2FAData = CommonHelper.Encrypt(resultJson, true, _encKey);
            var requestData = new EncryptedDataInput { Data = encrypted2FAData };

            var responsePayload = await RequestHelper.SendHttpRequest(_httpClient, $"{baseURL}/api/Login/ValidateTwoFactorAuthentication", requestData, "Bearer", inputModel.accessToken);

            if (responsePayload == null || !responsePayload.isSuccess || responsePayload.statusCode == HttpStatusCode.Unauthorized)
            {
                TempData["ToastMessage"] = responsePayload?.errorMessages ?? "Error in creating Branch Mapping!!.";
                TempData["ToastType"] = "danger";
                return RedirectToAction("VerifyPan");
            }
            
            JObject jsonData = JObject.FromObject(responsePayload.data);
            string username = jsonData?["name"]?.ToString();

            var sessionUser = new SessionUser
            {
                user_id = jsonData?["user_id"]?.ToString(),
                user_type = jsonData?["user_type"]?.ToString(),
                accessToken = jsonData?["accessToken"]?.ToString()
            };
            string sessionUserJson = JsonConvert.SerializeObject(sessionUser);
            HttpContext.Session.SetString("username", username);
            HttpContext.Session.SetString("sessionUser", sessionUserJson);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            // Redirect to login page
            return Redirect(lkpConnectURL);
        }
        #endregion
    }
}
