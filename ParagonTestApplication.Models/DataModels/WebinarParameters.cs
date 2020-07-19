namespace ParagonTestApplication.Models.DataModels
{
    using System;

    /// <summary>
    /// Webinar parameters model.
    /// </summary>
    public class WebinarParameters
    {
        /// <summary>
        /// Gets or sets minimum datetime.
        /// </summary>
        public DateTime? MinDateTime { get; set; }

        /// <summary>
        /// Gets or sets maximum datetime.
        /// </summary>
        public DateTime? MaxDateTime { get; set; }

        /// <summary>
        /// Gets or sets minimum duration.
        /// </summary>
        public int? MinDuration { get; set; }

        /// <summary>
        /// Gets or sets maximum duration.
        /// </summary>
        public int? MaxDuration { get; set; }

        /// <summary>
        /// Gets or sets series id.
        /// </summary>
        public int? SeriesId { get; set; }
    }
}