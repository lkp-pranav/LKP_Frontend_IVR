using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Models.Request.CustomGroup
{
    public class CustomGroupInputModel : CommonModel
    {
        public int RowId { get; set; }
        public string GroupCode { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public string DealerID { get; set; }
        public string DealerName { get; set; }
        public string CTCLLoginId { get; set; }
        public string Active { get; set; } = "Y";
    }
}
