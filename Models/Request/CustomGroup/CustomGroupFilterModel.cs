using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Models.Request.CustomGroup
{
    public class CustomGroupFilterModel : PageInputModel
    {
        public string? GroupCode { get; set; } = "";
        public string? Zone { get; set; } = "";
        public string? branch { get; set; } = "";
        public string? Active { get; set; } = "";
    }
}
