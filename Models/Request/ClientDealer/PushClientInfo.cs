using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Models.Request.ClientDealer
{
    public class PushClientInfo : CommonModel
    {
        public string Mode { get; set; } = "Incremental";
        public string Zone { get; set; } = "ALL";
        public string Branch { get; set; } = "ALL";
        public string DealerCode { get; set; } = "ALL";
        public string ClientCode { get; set; } = "ALL";
    }
}
