namespace ParagonTestApplication.Models.ApiModels.Webinars
{
    using System;
    using ParagonTestApplication.Models.ApiModels.Series;

    /// <summary>
    /// Webinar Data Transfer Object.
    /// </summary>
    public class WebinarDto
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets start datetime of webinar.
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets end datetime of webinar.
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets duration.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets series dto.
        /// </summary>
        public SeriesDto Series { get; set; }
    }
}