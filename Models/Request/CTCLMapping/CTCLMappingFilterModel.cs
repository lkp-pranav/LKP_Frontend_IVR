using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Models.Request.CTCLMapping
{
    public class CTCLMappingFilterModel : PageInputModel
    {
        public string? ClientCode { get; set; } = "";
        public string? PrimaryDealer { get; set; } = "";
        public string? SecondaryDealer { get; set; } = "";
        public string? Zone { get; set; } = "";
        public string? Category { get; set; } = "ALL";
    }
}
