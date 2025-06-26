using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Models.Request.ClientDealer
{
    public class ClientDealerFilterModel : PageInputModel
    {
        public string? ClientCode { get; set; } = "";
        public string? ClientName { get; set; } = "";
        public string? ClientType { get; set; } = "LKP";
        public string? Zone { get; set; } = "";
        public string? DealerID { get; set; } = "";
        public string? DealerName { get; set; } = "";
        public string? Branch { get; set; } = "";
        public string? Category { get; set; } = "ALL";
    }
}
