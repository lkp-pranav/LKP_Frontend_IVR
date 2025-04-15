namespace LKP_Frontend_MVC.Models.Request.Common
{
    public class PageInputModel : CommonModel
    {
        public int Start { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
