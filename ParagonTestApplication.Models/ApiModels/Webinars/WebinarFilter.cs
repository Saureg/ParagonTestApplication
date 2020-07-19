namespace ParagonTestApplication.Models.ApiModels.Webinars
{
    /// <summary>
    /// Webinar filter model.
    /// </summary>
    public class WebinarFilter
    {
        /// <summary>
        /// Gets or sets minimum datetime.
        /// </summary>
        public string MinDateTime { get; set; }

        /// <summary>
        /// Gets or sets maximum datetime.
        /// </summary>
        public string MaxDateTime { get; set; }

        /// <summary>
        /// Gets or sets minimum duration.
        /// </summary>
        public string MinDuration { get; set; }

        /// <summary>
        /// Gets or sets maximum duration.
        /// </summary>
        public string MaxDuration { get; set; }

        /// <summary>
        /// Gets or sets series id.
        /// </summary>
        public string SeriesId { get; set; }
    }
}