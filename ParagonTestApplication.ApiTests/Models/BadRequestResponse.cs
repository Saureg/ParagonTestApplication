using System.Collections.Generic;
using System.Net;

namespace ParagonTestApplication.ApiTests.Models
{
    public class BadRequestResponse
    {
        public string Title { get; set; }
        public HttpStatusCode Status { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}