namespace ParagonTestApplication.Models.DataModels
{
    using System;

    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global

    /// <summary>
    /// Webinar model.
    /// </summary>
    public class Webinar
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
        /// Gets or sets start datetime.
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets end datetime.
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets duration.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isDeleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets series id.
        /// </summary>
        public int SeriesId { get; set; }

        /// <summary>
        /// Gets or sets series.
        /// </summary>
        public virtual Series Series { get; set; }

        /// <summary>
        /// Calculate end datetime for webinar.
        /// </summary>
        public void CalculateEndDateTime()
        {
            this.EndDateTime = this.StartDateTime.AddMinutes(this.Duration);
        }
    }
}