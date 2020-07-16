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

namespace ParagonTestApplication.ApiTests.Tests.Webinars
{
    public class UpdateWebinarTests : BaseWebinarsTests
    {
        private async Task<int> CreateWebinar()
        {
            var response = await WebinarApiHelper.CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest());
            return response.Data.Id;
        }

        [Test]
        public async Task UpdateWebinarTest()
        {
            var existingWebinarId = await CreateWebinar();
            var startDateTime = DateTime.Now.RemoveSecondsAndMilliseconds();
            var updatedWebinarRequest = new CreateOrUpdateWebinarRequest
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 120,
                StartDateTime = startDateTime.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = Guid.NewGuid().ToString()
                }
            };
            var endDateTime = CalculateWebinarEndDateTime(startDateTime, updatedWebinarRequest.Duration);

            var updatedWebinarResponse = await WebinarApiHelper.UpdateWebinar(existingWebinarId, updatedWebinarRequest);

            updatedWebinarResponse.ShouldSatisfyAllConditions(
                () => updatedWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.Created),
                () => updatedWebinarResponse.Message.ShouldBe("Success"),
                () => updatedWebinarResponse.Data.Id.ShouldBe(existingWebinarId),
                () => updatedWebinarResponse.Data.Name.ShouldBe(updatedWebinarRequest.Name),
                () => updatedWebinarResponse.Data.Duration.ShouldBe(updatedWebinarRequest.Duration),
                () => updatedWebinarResponse.Data.StartDateTime.ShouldBe(startDateTime),
                () => updatedWebinarResponse.Data.EndDateTime.ShouldBe(endDateTime),
                () => updatedWebinarResponse.Data.Series.Id.ShouldBePositive(),
                () => updatedWebinarResponse.Data.Series.Name.ShouldBe(updatedWebinarRequest.Series.Name)
            );

            var getWebinarResponse = await WebinarApiHelper.GetWebinar(updatedWebinarResponse.Data.Id);

            getWebinarResponse.Data.Name.ShouldBe(updatedWebinarResponse.Data.Name);
        }

        [Test]
        public async Task UpdateWebinarWithNewSeriesNameTest()
        {
            var existingWebinarId = await CreateWebinar();
            var seriesName = Guid.NewGuid().ToString();
            var updatedWebinarResponse = await WebinarApiHelper.UpdateWebinar(existingWebinarId,
                new CreateOrUpdateWebinarRequest
                {
                    Name = Guid.NewGuid().ToString(),
                    Duration = 10,
                    StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                    Series = new CreateOrUpdateSeriesRequest
                    {
                        Name = seriesName
                    }
                });

            updatedWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var getSeriesResponse = await WebinarApiHelper.GetSeries();

            getSeriesResponse.Data.Count(x => x.Name == seriesName).ShouldBe(1);
        }

        [Test]
        public async Task UpdateWebinarWithExistingSeriesNameTest()
        {
            var seriesName = Guid.NewGuid().ToString();
            var createdWebinarResponse = await WebinarApiHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 10,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = seriesName
                }
            });

            var updatedWebinarResponse = await WebinarApiHelper.UpdateWebinar(createdWebinarResponse.Data.Id,
                new CreateOrUpdateWebinarRequest
                {
                    Name = Guid.NewGuid().ToString(),
                    Duration = 140,
                    StartDateTime = DateTime.Now.AddDays(30).ToDateTimeWithMinutesString(),
                    Series = new CreateOrUpdateSeriesRequest
                    {
                        Name = seriesName
                    }
                });

            updatedWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var getSeriesResponse = await WebinarApiHelper.GetSeries();

            getSeriesResponse.Data.Count(x => x.Name == seriesName).ShouldBe(1);
        }

        [Test]
        public async Task UpdateWebinarWithNonUniqueNameTest()
        {
            var existingWebinarId = await CreateWebinar();

            var webinarName = Guid.NewGuid().ToString();
            await WebinarApiHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
            {
                Name = webinarName,
                Duration = 10,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = Guid.NewGuid().ToString()
                }
            });

            var updateWebinarResponse = await WebinarApiHelper.UpdateWebinar(existingWebinarId,
                new CreateOrUpdateWebinarRequest
                {
                    Name = webinarName,
                    Duration = 250,
                    StartDateTime = DateTime.Now.AddMonths(2).ToDateTimeWithMinutesString(),
                    Series = new CreateOrUpdateSeriesRequest
                    {
                        Name = Guid.NewGuid().ToString()
                    }
                });

            updateWebinarResponse.ShouldSatisfyAllConditions
            (
                () => updateWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => updateWebinarResponse.Data.ShouldBe(null),
                () => updateWebinarResponse.Message.ShouldBe("Name must be unique")
            );
        }

        [Test]
        public async Task UpdateWebinarWithCurrentNameTest()
        {
            var createdWebinarResponse = await WebinarApiHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 10,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = Guid.NewGuid().ToString()
                }
            });

            var updateWebinarResponse = await WebinarApiHelper.UpdateWebinar(createdWebinarResponse.Data.Id,
                new CreateOrUpdateWebinarRequest
                {
                    Name = createdWebinarResponse.Data.Name,
                    Duration = 250,
                    StartDateTime = DateTime.Now.AddMonths(2).ToDateTimeWithMinutesString(),
                    Series = new CreateOrUpdateSeriesRequest
                    {
                        Name = Guid.NewGuid().ToString()
                    }
                });

            updateWebinarResponse.ShouldSatisfyAllConditions
            (
                () => updateWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.Created),
                () => updateWebinarResponse.Data.Name.ShouldBe(createdWebinarResponse.Data.Name),
                () => updateWebinarResponse.Message.ShouldBe("Success")
            );
        }

        [Test]
        public async Task UpdateWebinarWithUnknownIdTest()
        {
            const int unknownId = 0;

            var updateWebinarResponse =
                await WebinarApiHelper.UpdateWebinar(unknownId, CreateOrUpdateWebinarTestData.GenerateRequest());

            updateWebinarResponse.ShouldSatisfyAllConditions
            (
                () => updateWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound),
                () => updateWebinarResponse.Data.ShouldBeNull(),
                () => updateWebinarResponse.Message.ShouldBe($"Webinar with id={unknownId} not found")
            );
        }

        [Test]
        public async Task UpdateWebinarValidationTest(
            [ValueSource(typeof(CreateOrUpdateWebinarTestData),
                nameof(CreateOrUpdateWebinarTestData.GenerateValidationTestDataList))]
            CreateTestDataModel validationTestData)
        {
            var existingWebinarId = await CreateWebinar();

            var updatedWebinarResponse =
                await WebinarApiHelper.UpdateWebinar(existingWebinarId, validationTestData.CreateWebinarRequest);

            updatedWebinarResponse.ShouldSatisfyAllConditions
            (
                () => updatedWebinarResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => updatedWebinarResponse.Data.ShouldBeNull(),
                () => updatedWebinarResponse.Message.ShouldBe(validationTestData.ExpectedValidationMessage)
            );
        }

        [Test]
        public async Task UpdateWebinarWithInvalidDurationTest()
        {
            var existingWebinarId = await CreateWebinar();

            var response = await WebinarApiHelper.UpdateWebinarWithInvalidData($"{existingWebinarId}", new
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 1.1,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = Guid.NewGuid().ToString()
                }
            });

            response.ShouldSatisfyAllConditions
            (
                () => response.Status.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Title.ShouldBe("One or more validation errors occurred."),
                () => response.Errors.Count.ShouldBe(1)
            );
        }

        [Test]
        public async Task UpdateWebinarWithoutRequestTest()
        {
            var existingWebinarId = await CreateWebinar();

            var response = await WebinarApiHelper.UpdateWebinarWithInvalidData($"{existingWebinarId}", null);

            response.ShouldSatisfyAllConditions
            (
                () => response.Status.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Title.ShouldBe("One or more validation errors occurred."),
                () => response.Errors.Count.ShouldBe(1)
            );
        }

        [Test]
        public async Task UpdateWebinarWithInvalidIdTest()
        {
            var response = await WebinarApiHelper.UpdateWebinarWithInvalidData("first", new CreateOrUpdateWebinarRequest
            {
                Name = Guid.NewGuid().ToString(),
                Duration = 11,
                StartDateTime = DateTime.Now.ToDateTimeWithMinutesString(),
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = Guid.NewGuid().ToString()
                }
            });

            response.ShouldSatisfyAllConditions
            (
                () => response.Status.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Title.ShouldBe("One or more validation errors occurred."),
                () => response.Errors.Count.ShouldBe(1)
            );
        }
    }
}