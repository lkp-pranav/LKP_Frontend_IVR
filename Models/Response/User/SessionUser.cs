using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Models.Response.User
{
    public class SessionUser : CommonModel
    {
        public string accessToken { get; set; }
    }
}
