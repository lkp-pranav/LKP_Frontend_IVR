using LKP_Frontend_MVC.Models.Response.Common;
using LKP_Frontend_MVC.Models.Response.User;
using Newtonsoft.Json;
using System.Net;

namespace LKP_Frontend_MVC.Middleware
{
    public class LoginCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public LoginCheckMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            ResponsePayLoad payLoad = new ResponsePayLoad();

            var excludedPaths = new HashSet<string>
            {
                "/Login",
                "/VerifyPan",
                "/AuthenticatePAN",
                "/SSOLogin"
            };

            if (excludedPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
            {
                await _next(context);
                return;
            }

            try
            {
                var sessionUserJson = context.Session.GetString("sessionUser");

                if (string.IsNullOrEmpty(sessionUserJson))
                {
                    context.Response.Redirect("/Login/Index");
                    return;
                }

                var sessionUser = JsonConvert.DeserializeObject<SessionUser>(sessionUserJson);
                context.Items["SessionUser"] = sessionUser;

                await _next(context);
            }
            catch(Exception ex)
            {
                payLoad.errorMessages = ex.Message;
                payLoad.isSuccess = false;
                payLoad.message = "Error in Login Check Middleware";
                payLoad.data = null;
                payLoad.statusCode = HttpStatusCode.InternalServerError;
            }
        }
    }
}
