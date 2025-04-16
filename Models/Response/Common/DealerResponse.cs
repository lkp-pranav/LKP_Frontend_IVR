namespace LKP_Frontend_MVC.Models.Response.Common
{
    public class DealerResponse
    {
        public string DealerID { get; set; }
        public string DealerName { get; set; }
        public string CTCLLoginid { get; set; }
        public string? Segment { get; set; } = null;
    }
}
