using System;
using ParagonTestApplication.Models.ApiModels.Common;
using ParagonTestApplication.Models.Common;

namespace ParagonTestApplication.Models.ApiModels.Webinars
{
    public class WebinarFilter
    {
        public string MinDateTime { get; set; }
        public string MaxDateTime { get; set; }
        public string MinDuration { get; set; }
        public string MaxDuration { get; set; }
        public string SeriesId { get; set; }
    }
}