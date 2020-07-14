using System;
using ParagonTestApplication.Models.ApiModels.Series;

namespace ParagonTestApplication.Models.ApiModels.Webinars
{
    public class WebinarDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public DateTime StartDateTime { get; set; }
        
        public DateTime EndDateTime { get; set; }
        
        public int Duration { get; set; }
        
        public SeriesDto Series { get; set; }
    }
}