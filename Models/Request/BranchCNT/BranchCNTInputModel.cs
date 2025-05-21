using LKP_Frontend_MVC.Models.Request.Common;
using System.ComponentModel.DataAnnotations;

namespace LKP_Frontend_MVC.Models.Request.BranchCNT
{
    public class BranchCNTInputModel : CommonModel
    {
        public int RowId { get; set; }

        [Required]
        public string Zone { get; set; }

        [Required]
        public string DealerID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Dealer Name can't exceed 100 characters.")]
        public string DealerName { get; set; }

        [Required]
        public string CtclLoginId { get; set; }
    }
}
