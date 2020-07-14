using System.Net;

namespace ParagonTestApplication.Models.ApiModels.Common
{
    public class Response<T>
    {
        public Response()
        {
        }
        
        public Response(T data)
        {
            Message = string.Empty;
            Data = data;
        }
        
        public Response(T data, HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message ?? "Success";
            Data = data;
        }
        
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}