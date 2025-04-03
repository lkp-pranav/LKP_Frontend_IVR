using System.Net;

namespace LKP_Frontend_MVC.Models.Response.Common
{
    public class ResponsePayLoad
    {
        public HttpStatusCode statusCode { get; set; }
        public bool isSuccess { get; set; } = true;
        public string? errorMessages { get; set; } = string.Empty;
        public object? data { get; set; } = new object();
        public object? message { get; set; } = string.Empty;
    }
}
