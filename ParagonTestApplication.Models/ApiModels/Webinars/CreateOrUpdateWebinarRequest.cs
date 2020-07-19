namespace ParagonTestApplication.Models.ApiModels.Webinars
{
    using ParagonTestApplication.Models.ApiModels.Series;

    /// <summary>
    /// Webinar request model for creating or updating.
    /// </summary>
    public class CreateOrUpdateWebinarRequest
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets star date time.
        /// </summary>
        public string StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets duration.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets Series.
        /// </summary>
        public CreateOrUpdateSeriesRequest Series { get; set; }
    }
}