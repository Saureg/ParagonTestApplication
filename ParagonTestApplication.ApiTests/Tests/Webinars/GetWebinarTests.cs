namespace ParagonTestApplication.ApiTests.Tests.Webinars
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using ParagonTestApplication.Models.ApiModels.Series;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using Shouldly;

    /// <summary>
    /// Get webinar tests.
    /// </summary>
    public class GetWebinarTests : BaseWebinarsTests
    {
        private WebinarDto webinar;

        /// <summary>
        /// Prepate test data.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [OneTimeSetUp]
        public async Task PrepareTestData()
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

            var result = await this.WebinarApiHelper.CreateWebinar(createdOrUpdateWebinar);

            this.webinar = result.Data;
        }

        /// <summary>
        /// Check get webinar.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarTest()
        {
            var response = await this.WebinarApiHelper.GetWebinar(this.webinar.Id);

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Message.ShouldBe("Success"),
                () => response.Data.Id.ShouldBe(this.webinar.Id),
                () => response.Data.Name.ShouldBe(this.webinar.Name),
                () => response.Data.Duration.ShouldBe(this.webinar.Duration),
                () => response.Data.StartDateTime.ShouldBe(this.webinar.StartDateTime),
                () => response.Data.EndDateTime.ShouldBe(this.webinar.EndDateTime),
                () => response.Data.Series.Id.ShouldBe(this.webinar.Series.Id),
                () => response.Data.Series.Name.ShouldBe(this.webinar.Series.Name));
        }

        /// <summary>
        /// Check get webinar with unknown id.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarWithUnknownIdTest()
        {
            const int testWebinarId = 0;
            var response = await this.WebinarApiHelper.GetWebinar(testWebinarId);

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.NotFound),
                () => response.Message.ShouldBe($"Webinar with id={testWebinarId} not found"),
                () => response.Data.ShouldBeNull());
        }

        /// <summary>
        /// Check get webinar with invalid id.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarWithInvalidIdTest()
        {
            const string invalidWebinarId = "test";
            var response = await this.WebinarApiHelper.GetWebinarWithInvalidData(invalidWebinarId);

            response.ShouldSatisfyAllConditions(
                () => response.Status.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Title.ShouldBe("One or more validation errors occurred."),
                () => response.Errors.Count.ShouldBe(1),
                () => response.Errors["id"].ShouldBe(new List<string> { $"The value '{invalidWebinarId}' is not valid." }));
        }
    }
}