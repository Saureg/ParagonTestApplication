// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ParagonTestApplication.ApiClient.Models
{
    using System.Collections.Generic;
    using System.Net;

    // ReSharper disable once ClassNeverInstantiated.Global

    /// <summary>
    /// Response with FluentValidation error.
    /// </summary>
    public class ValidationErrorResponse
    {
        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets statusCode.
        /// </summary>
        public HttpStatusCode Status { get; set; }

        // ReSharper disable once CollectionNeverUpdated.Global

        /// <summary>
        /// Gets or sets errors.
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}