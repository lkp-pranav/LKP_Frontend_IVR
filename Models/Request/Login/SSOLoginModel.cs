namespace LKP_Frontend_MVC.Models.Request.Login
{
    public class SSOLoginModel : TwoFactorAuthInputModel
    {
        public string accessToken { get; set; }
    }
}
