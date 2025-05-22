using LKP_Frontend_MVC.Models.Request.Common;
using System.ComponentModel.DataAnnotations;

namespace LKP_Frontend_MVC.Models.Request.CustomGroup
{
    public class CustomGroupInputModel : CommonModel
    {
        [Required]
        public string GroupCode { get; set; }
        [Required]
        public string Zone { get; set; }
        [Required]
        public string Branch { get; set; }
        [Required]
        public string DealerID { get; set; }
        [Required]
        public string DealerName { get; set; }
        [Required]
        public string CTCLLoginId { get; set; }
        [Required]
        public string Active { get; set; }
    }
}
