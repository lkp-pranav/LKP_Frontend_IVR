using LKP_Frontend_MVC.Models.Request.Common;
using LKP_Frontend_MVC.Models.Request.Login;
using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.User;
using LKP_Frontend_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace LKP_Frontend_MVC.Controllers.Login
{
    public class LoginController : Controller
    {
        #region fields
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        private readonly string _key;
        private readonly string _encKey;
        #endregion

        #region constructor
        public LoginController(IConfiguration configuration, HttpClient httpClient)
        {
            _Configuration = configuration;
            _httpClient = httpClient;
            _key = _Configuration.GetSection("SecurityKey").Value;
            _encKey = _Configuration.GetSection("encKey").Value;
        }
        #endregion

        #region Login
        // Render Login Page
        [HttpGet]
        public IActionResult Index() => View();

        // Verify Login and redirect to 2FA
        [HttpPost]
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

            ResponsePayLoad? responsePayload = await LoginHelper.SendHttpRequest(_httpClient, "https://localhost:7121/api/Login/Login", requestData, "Basic", base64Credentials);
            Console.WriteLine("Response after login", responsePayload);

            if (responsePayload == null || !responsePayload.isSuccess)
            {
                TempData["ErrorMessage"] = responsePayload?.errorMessages ?? "An unexpected error occurred.";
                TempData["ShowToast"] = true;
                ViewBag.IsSuccess = false;
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
                Console.WriteLine("Missing Bearer Key or Encrypted Data");
                return RedirectToAction("VerifyPan");
            }

            var user = LoginHelper.DeserializeEncryptedData<TwoFactorAuthInputModel>(encryptedData, _encKey);
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
           
            ResponsePayLoad? responsePayload = await LoginHelper.SendHttpRequest(_httpClient, "https://localhost:7121/api/Login/ValidateTwoFactorAuthentication", requestData, "Bearer", bearerKey);

            if (responsePayload == null || !responsePayload.isSuccess)
            {
                return RedirectToAction("VerifyPan");
            }
            Console.WriteLine(responsePayload.data);
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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            // Redirect to login page
            return RedirectToAction("Index");
        }
    }
}
