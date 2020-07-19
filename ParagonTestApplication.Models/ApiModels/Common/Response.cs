namespace ParagonTestApplication.Models.ApiModels.Common
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    using System.Net;

    /// <summary>
    /// Common response model.
    /// </summary>
    /// <typeparam name="T">Object model in common response.</typeparam>
    public class Response<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}"/> class.
        /// </summary>
        /// <param name="statusCode">Status code.</param>
        /// <param name="data">Data.</param>
        /// <param name="message">Response message.</param>
        public Response(HttpStatusCode statusCode, T data, string message = null)
        {
            this.StatusCode = statusCode;
            this.Message = message ?? "Success";
            this.Data = data;
        }

        /// <summary>
        /// Gets or sets data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets response message.
        /// </summary>
        public string Message { get; set; }
    }
}