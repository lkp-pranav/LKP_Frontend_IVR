using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Models.Request.DealerCNT
{
    public class DealerCNTInputModel : CommonModel
    {
        
        public string Zone { get; set; }

        
        public string PrimaryDealer { get; set; }

        
        public string PrimaryDealerName { get; set; }

        
        public string PrimaryDealerCTCLLogin { get; set; }

        
        public string SecondaryDealer { get; set; }

        
        public string SecondaryDealerName { get; set; }

        
        public string SecondaryDealerCTCLLogin { get; set; }
    }
}
