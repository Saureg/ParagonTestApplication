using System.Net;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace ParagonTestApplication.Models.ApiModels.Common
{
    public class Response<T>
    {
        public Response(HttpStatusCode statusCode, T data, string message = null)
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