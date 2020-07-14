using ParagonTestApplication.Models.ApiModels.Series;

namespace ParagonTestApplication.Models.ApiModels.Webinars
{
    public class CreateOrUpdateWebinarRequest
    {
        public string Name { get; set; }
        
        public string StartDateTime { get; set; }
        
        public int Duration { get; set; }
        
        public CreateOrUpdateSeriesRequest Series { get; set; }
    }
}