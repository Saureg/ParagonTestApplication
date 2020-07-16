using System.Collections.Generic;
using System.Net;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ParagonTestApplication.ApiTests.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ValidationErrorResponse
    {
        public string Title { get; set; }
        public HttpStatusCode Status { get; set; }
        // ReSharper disable once CollectionNeverUpdated.Global
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}