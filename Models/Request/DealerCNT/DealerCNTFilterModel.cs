using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Models.Request.DealerCNT
{
    public class DealerCNTFilterModel : PageInputModel
    {
        public string? Zone { get; set; } = "";
        public string? PrimaryDealer { get; set; } = "";
        public string? PrimaryCTCL { get; set; } = "";
        public string? SecondaryDealer { get; set; } = "";
        public string? SecondaryCTCL { get; set; } = "";
    }
}
