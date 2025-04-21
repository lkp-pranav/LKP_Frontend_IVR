namespace LKP_Frontend_MVC.Models.Request.Common
{
    public class PageInputModel : CommonModel
    {
        public int Start { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        public string? ClientCode { get; set; } = "";
        public string? ClientName { get; set; } = "";
        public string? Zone { get; set; } = "";
        public string? DealerID { get; set; } = "";
        public string? DealerName { get; set; } = "";
        public string? branch { get; set; } = "";
        public string? Category { get; set; } = "ALL";
        public string GroupCode { get; set; } = "";
    }
}
