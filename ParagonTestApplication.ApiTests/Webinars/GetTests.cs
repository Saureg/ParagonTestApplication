using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using ParagonTestApplication.ApiTests.Common;
using ParagonTestApplication.ApiTests.Helpers;
using ParagonTestApplication.Models.ApiModels.Series;
using ParagonTestApplication.Models.ApiModels.Webinars;
using Shouldly;

namespace ParagonTestApplication.ApiTests.Webinars
{
    public class GetTests
    {
        private readonly WebinarHelper _helper;

        public GetTests()
        {
            var apiServer = ApiServer.GetInstance();
            var client = new HttpClientWrapper(apiServer.Client);
            _helper = new WebinarHelper(client);
        }

        private WebinarDto _webinar;

        [OneTimeSetUp]
        public async Task PrepareTests()
        {
            var createdOrUpdateWebinar = new CreateOrUpdateWebinarRequest
            {
                Duration = 10,
                Name = Guid.NewGuid().ToString(),
                StartDateTime = "2020-09-01T12:00",
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = "zxc"
                }
            };

            var result = await _helper.CreateWebinar(createdOrUpdateWebinar);

            _webinar = result.Data;
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _helper.DeleteWebinar(_webinar.Id);
        }

        [Test]
        public async Task GetWebinarTest()
        {
            var response = await _helper.GetWebinar(_webinar.Id);

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Message.ShouldBe("Success"),
                () => response.Data.Id.ShouldBe(_webinar.Id),
                () => response.Data.Name.ShouldBe(_webinar.Name),
                () => response.Data.Duration.ShouldBe(_webinar.Duration),
                () => response.Data.StartDateTime.ShouldBe(_webinar.StartDateTime),
                () => response.Data.EndDateTime.ShouldBe(_webinar.EndDateTime),
                () => response.Data.Series.Id.ShouldBe(_webinar.Series.Id),
                () => response.Data.Series.Name.ShouldBe(_webinar.Series.Name)
            );
        }

        [Test]
        public async Task GetWebinarWithUnknownIdTest()
        {
            const int testWebinarId = 0;
            var response = await _helper.GetWebinar(testWebinarId);

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.NotFound),
                () => response.Message.ShouldBe($"Webinar with id={testWebinarId} not found"),
                () => response.Data.ShouldBeNull()
            );
        }

        [Test]
        public async Task GetWebinarWithInvalidIdTest()
        {
            const string invalidWebinarId = "test";
            var response = await _helper.GetWebinarWithBadRequestResponse(invalidWebinarId);

            response.ShouldSatisfyAllConditions
            (
                () => response.Status.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Title.ShouldBe("One or more validation errors occurred."),
                () => response.Errors.Count.ShouldBe(1),
                () => response.Errors["id"].ShouldBe(new List<string> {$"The value '{invalidWebinarId}' is not valid."})
            );
        }
    }
}