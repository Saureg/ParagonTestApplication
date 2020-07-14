using System;

namespace ParagonTestApplication.Models.DataModels
{
    public class WebinarParameters
    {
        public DateTime? MinDateTime { get; set; }
        public DateTime? MaxDateTime { get; set; }
        public int? MinDuration { get; set; }
        public int? MaxDuration { get; set; }
        public int? SeriesId { get; set; } 
    }
}