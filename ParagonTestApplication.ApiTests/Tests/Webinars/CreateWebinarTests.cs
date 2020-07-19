namespace ParagonTestApplication.ApiTests.Tests.Webinars
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using ParagonTestApplication.ApiTests.Helpers;
    using ParagonTestApplication.ApiTests.Models.TestDataModels.Webinars;
    using ParagonTestApplication.ApiTests.TestData;
    using ParagonTestApplication.Models.ApiModels.Series;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using Shouldly;

    /// <summary>
    /// Create webinar tests.
    /// </summary>
    public class CreateWebinarTests : BaseWebinarsTests
    {
        /// <summary>
        /// Check creating webinar.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task CreateWebinarTest()
        {
            var startDateTime = DateTime.Now.RemoveSecondsAndMilliseconds();
            var newWebinarRequest = new CreateOrUpdateWebinarRequest
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 10,
                StartDateTime = startDateTime.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = Guid.NewGuid().ToString()
                }
            };
            var endDateTime = CalculateWebinarEndDateTime(startDateTime, newWebinarRequest.Duration);

            var createWebinarResponse = await this.WebinarApiHelper.CreateWebinar(newWebinarRequest);

            createWebinarResponse.ShouldSatisfyAllConditions(
                () => createWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.Created),
                () => createWebinarResponse.Message.ShouldBe("Success"),
                () => createWebinarResponse.Data.Id.ShouldBePositive(),
                () => createWebinarResponse.Data.Name.ShouldBe(newWebinarRequest.Name),
                () => createWebinarResponse.Data.Duration.ShouldBe(newWebinarRequest.Duration),
                () => createWebinarResponse.Data.StartDateTime.ShouldBe(startDateTime),
                () => createWebinarResponse.Data.EndDateTime.ShouldBe(endDateTime),
                () => createWebinarResponse.Data.Series.Id.ShouldBePositive(),
                () => createWebinarResponse.Data.Series.Name.ShouldBe(newWebinarRequest.Series.Name));

            var getWebinarResponse = await this.WebinarApiHelper.GetWebinar(createWebinarResponse.Data.Id);

            getWebinarResponse.Data.Name.ShouldBe(createWebinarResponse.Data.Name);
        }

        /// <summary>
        /// Check creating webinar with new series name.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task CreateWebinarWithNewSeriesNameTest()
        {
            var seriesName = Guid.NewGuid().ToString();

            var createWebinarResponse = await this.WebinarApiHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 10,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = seriesName
                }
            });

            createWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var getSeriesResponse = await this.WebinarApiHelper.GetSeries();

            getSeriesResponse.Data.Count(x => x.Name == seriesName).ShouldBe(1);
        }

        /// <summary>
        /// Check creating webinar with existing series name.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task CreateWebinarWithExistingSeriesNameTest()
        {
            var seriesName = Guid.NewGuid().ToString();

            await this.WebinarApiHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 10,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = seriesName
                }
            });

            var createWebinarResponse = await this.WebinarApiHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 10,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = seriesName
                }
            });

            createWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var getSeriesResponse = await this.WebinarApiHelper.GetSeries();

            getSeriesResponse.Data.Count(x => x.Name == seriesName).ShouldBe(1);
        }

        /// <summary>
        /// Check creating webinar with non unique name.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task CreateWebinarWithNonUniqueNameTest()
        {
            var webinarName = Guid.NewGuid().ToString();

            await this.WebinarApiHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
            {
                Name = webinarName,
                Duration = 10,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = Guid.NewGuid().ToString()
                }
            });

            var createWebinarResponse = await this.WebinarApiHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
            {
                Name = webinarName,
                Duration = 10,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = Guid.NewGuid().ToString()
                }
            });

            createWebinarResponse.ShouldSatisfyAllConditions(
                () => createWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => createWebinarResponse.Data.ShouldBe(null),
                () => createWebinarResponse.Message.ShouldBe("Name must be unique"));
        }

        /// <summary>
        /// Check validation.
        /// </summary>
        /// <param name="validationTestData">Validation test data.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task CreateWebinarValidationTest(
            [ValueSource(
                typeof(CreateOrUpdateWebinarTestData),
                nameof(CreateOrUpdateWebinarTestData.GenerateValidationTestDataList))]
            CreateTestDataModel validationTestData)
        {
            var createWebinarResponse = await this.WebinarApiHelper.CreateWebinar(validationTestData.CreateWebinarRequest);

            createWebinarResponse.ShouldSatisfyAllConditions(
                () => createWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => createWebinarResponse.Data.ShouldBeNull(),
                () => createWebinarResponse.Message.ShouldBe(validationTestData.ExpectedValidationMessage));
        }

        /// <summary>
        /// Check creating webinar with invalid duration.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task CreateWebinarWithInvalidDurationTest()
        {
            var response = await this.WebinarApiHelper.CreateWebinarWithInvalidData(new
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 1.1,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = Guid.NewGuid().ToString()
                }
            });

            response.ShouldSatisfyAllConditions(
                () => response.Status.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Title.ShouldBe("One or more validation errors occurred."),
                () => response.Errors.Count.ShouldBe(1));
        }

        /// <summary>
        /// Check creating webinar without request.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task CreateWebinarWithoutRequestTest()
        {
            var response = await this.WebinarApiHelper.CreateWebinarWithInvalidData(null);

            response.ShouldSatisfyAllConditions(
                () => response.Status.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Title.ShouldBe("One or more validation errors occurred."),
                () => response.Errors.Count.ShouldBe(1));
        }
    }
}