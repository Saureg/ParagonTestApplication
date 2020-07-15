using System;
using System.Collections.Generic;
using AutoMapper;
using NUnit.Framework;
using ParagonTestApplication.Mappings;
using ParagonTestApplication.Models.DataModels;

namespace ParagonTestApplication.UnitTests.WebinarControllerTests
{
    [Category("WebinarController")]
    public class WebinarControllerBaseTests
    {
        protected readonly IMapper MockMapper;
        protected readonly List<Webinar> TestWebinars;

        protected WebinarControllerBaseTests()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new MapperProfile()); });
            MockMapper = mapperConfiguration.CreateMapper();

            TestWebinars = new List<Webinar>
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
            TestWebinars.ForEach(x => x.CalculateEndDateTime());
        }
    }
}