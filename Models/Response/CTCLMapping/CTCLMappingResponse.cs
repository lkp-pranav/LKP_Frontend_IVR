namespace LKP_Frontend_MVC.Models.Response.CTCLMapping
{
    public class CTCLMappingResponse
    {
        public string? Clientcode { get; set; } = "";
        public string? PrimaryDealer { get; set; }
        public string? PrimaryDealerName { get; set; }
        public string? PrimaryDealerCTCLLogin { get; set; }
        public string? PrimaryDealerWorkStatus { get; set; }
        public string? SecondaryDealer { get; set; }
        public string? SecondaryDealerName { get; set; }
        public string? SecondaryDealerCTCLLogin { get; set; }
        public string? SecondaryDealerWorkStatus { get; set; }
        public string? Zone { get; set; }
        public string? ZoneDealerID { get; set; }
        public string? ZoneDealerName { get; set; }
        public string? ZoneCtclLoginID { get; set; }
        public string? ZoneWorkStatus { get; set; }
        public int IsHOCNT { get; set; }
        public string? HO_DealerID { get; set; }
        public string? HO_DealerName { get; set; }
        public string? HO_CTCLLogin { get; set; }
        public string? HO_WorkStatus { get; set; }
        public string? Category { get; set; }
        public int Flag { get; set; }
    }
}
