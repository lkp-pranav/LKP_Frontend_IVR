using System.ComponentModel.DataAnnotations;

namespace LKP_Frontend_MVC.Models.Request.Login
{
    public class TwoFactorAuthInputModel
    {
        [Display(Name = "User Id")]
        public string? User_id { get; set; }

        [Display(Name = "User Type")]
        public string User_type { get; set; }

        [Display(Name = "Auth Type")]
        public string Auth_type { get; set; } = "PAN";

        [Display(Name = "Auth Value")]
        public string Auth_value { get; set; }
    }
}
