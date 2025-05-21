using LKP_Frontend_MVC.Models.Request.Common;

namespace LKP_Frontend_MVC.Models.Request.BranchCNT
{
    public class BranchCNTInputModel : CommonModel
    {
        public int RowId { get; set; }  
        public string Zone { get; set; }
        public string DealerID { get; set; }
        public string DealerName { get; set; }
        public string CtclLoginId { get; set; }
    }
}
