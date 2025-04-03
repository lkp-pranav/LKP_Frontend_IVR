using System.ComponentModel.DataAnnotations;

namespace LKP_Frontend_MVC.Models.Request.Login
{
    public class LoginInputModel
    {
        [Display(Name = "User Type")]
        public string User_type { get; set; }

        [Display(Name = "User Id")]
        public string User_id { get; set; }

        [Display(Name = "User Password")]
        public string User_password { get; set; }
    }
}
