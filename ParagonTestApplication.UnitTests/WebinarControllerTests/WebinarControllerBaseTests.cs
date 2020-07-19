namespace ParagonTestApplication.UnitTests.WebinarControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using NUnit.Framework;
    using ParagonTestApplication.Mappings;
    using ParagonTestApplication.Models.DataModels;

    /// <summary>
    /// Base class for webinar controller tests.
    /// </summary>
    [Category("Unit.WebinarController")]
    public abstract class WebinarControllerBaseTests
    {
        protected readonly IMapper MockMapper;
        protected readonly List<Webinar> TestWebinars;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebinarControllerBaseTests"/> class.
        /// </summary>
        protected WebinarControllerBaseTests()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new MapperProfile()); });
            this.MockMapper = mapperConfiguration.CreateMapper();

            this.TestWebinars = new List<Webinar>
            {
                new Webinar
                {
                    Id = 1,
                    Name = "first_webinar",
                    StartDateTime = DateTime.Now,
                    Duration = 15,
                    Series = new Series
                    {
                        Id = 1,
                        Name = "series"
                    }
                },
                new Webinar
                {
                    Id = 2,
                    Name = "second_webinar",
                    StartDateTime = DateTime.Now.AddDays(30),
                    Duration = 120,
                    Series = new Series
                    {
                        Id = 1,
                        Name = "series"
                    }
                }
            };
            this.TestWebinars.ForEach(x => x.CalculateEndDateTime());
        }
    }
}