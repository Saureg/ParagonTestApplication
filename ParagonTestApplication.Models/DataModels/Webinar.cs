using System;

namespace ParagonTestApplication.Models.DataModels
{
    public class Webinar
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int Duration { get; set; }

        public bool IsDeleted { get; set; }

        public int SeriesId { get; set; }

        public virtual Series Series { get; set; }

        public void CalculateEndDateTime()
        {
            EndDateTime = StartDateTime.AddMinutes(Duration);
        }
    }
}