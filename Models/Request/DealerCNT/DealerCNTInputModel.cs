using LKP_Frontend_MVC.Models.Request.Common;
using System.ComponentModel.DataAnnotations;

namespace LKP_Frontend_MVC.Models.Request.DealerCNT
{
    public class DealerCNTInputModel : CommonModel
    {
        public int RowId { get; set; }
        [Required]
        public string Zone { get; set; }

        [Required]
        public string PrimaryDealer { get; set; }

        [Required]
        public string PrimaryDealerName { get; set; }

        [Required]
        public string PrimaryDealerCTCLLogin { get; set; }

        [Required]
        public string SecondaryDealer { get; set; }

        [Required]
        public string SecondaryDealerName { get; set; }

        [Required]
        public string SecondaryDealerCTCLLogin { get; set; }
    }
}
