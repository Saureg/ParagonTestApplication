namespace ParagonTestApplication.UnitTests.WebinarControllerTests
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
    using ParagonTestApplication.Controllers;
    using ParagonTestApplication.Data.Contracts;
    using ParagonTestApplication.Models.ApiModels.Common;
    using ParagonTestApplication.Models.ApiModels.Series;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using ParagonTestApplication.Models.DataModels;
    using Shouldly;

    /// <summary>
    /// Update webinar tests.
    /// </summary>
    public class UpdateWebinarTests : WebinarControllerBaseTests
    {
        /// <summary>
        /// Check updating webinar with valid data.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task UpdateWebinar_WithValidData_ReturnsCreatedResult()
        {
            const int testWebinarId = 100;
            var createOrUpdateRequest = new CreateOrUpdateWebinarRequest
            {
                Name = "updated_webinar_name",
                Duration = 120,
                StartDateTime = "2020-09-01T12:00",
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = "updated_series_name"
                }
            };
            var updatedWebinar = new Webinar
            {
                Id = testWebinarId,
                Name = "updated_webinar_name",
                Duration = 120,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddMinutes(120),
                Series = new Series
                {
                    Id = 120,
                    Name = "updated_series_name"
                }
            };
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.Get(testWebinarId))
                .ReturnsAsync(this.TestWebinars.First);
            mock
                .Setup(x => x.Update(testWebinarId, It.IsAny<Webinar>()))
                .ReturnsAsync(updatedWebinar);
            var webinarController = new WebinarController(this.MockMapper, mock.Object);

            var result = await webinarController.UpdateWebinar(testWebinarId, createOrUpdateRequest);

            var createdAtActionResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
            var model = createdAtActionResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions(
                () => model.StatusCode.ShouldBe(HttpStatusCode.Created),
                () => model.Message.ShouldContain("Success"),
                () => model.Data.Id.ShouldBe(testWebinarId),
                () => model.Data.Duration.ShouldBe(updatedWebinar.Duration),
                () => model.Data.StartDateTime.ShouldBe(updatedWebinar.StartDateTime),
                () => model.Data.EndDateTime.ShouldBe(updatedWebinar.EndDateTime),
                () => model.Data.Series.Id.ShouldBe(updatedWebinar.Series.Id),
                () => model.Data.Series.Name.ShouldBe(updatedWebinar.Series.Name));
        }

        /// <summary>
        /// Check updating webinar with current name.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task UpdateWebinar_WithCurrentName_ReturnsCreatedResult()
        {
            var testWebinarId = this.TestWebinars.First().Id;
            var testWebinarName = this.TestWebinars.First().Name;
            var createOrUpdateRequest = new CreateOrUpdateWebinarRequest
            {
                Name = testWebinarName,
                Duration = 120,
                StartDateTime = "2020-09-01T12:00",
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = "updated_series_name"
                }
            };
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.Get(testWebinarId))
                .ReturnsAsync(this.TestWebinars.First);
            mock
                .Setup(x => x.GetAll())
                .ReturnsAsync(this.TestWebinars);
            mock
                .Setup(x => x.Update(testWebinarId, It.IsAny<Webinar>()))
                .ReturnsAsync(this.TestWebinars.First);
            var webinarController = new WebinarController(this.MockMapper, mock.Object);

            var result = await webinarController.UpdateWebinar(testWebinarId, createOrUpdateRequest);

            var createdAtActionResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
            var model = createdAtActionResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions(
                () => model.StatusCode.ShouldBe(HttpStatusCode.Created),
                () => model.Message.ShouldContain("Success"),
                () => model.Data.Id.ShouldBe(testWebinarId));
        }

        /// <summary>
        /// Check updating webinar with unknown id.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task UpdateWebinar_WithUnknownId_ReturnsNotFoundResult()
        {
            const int testWebinarId = 0;
            var mock = new Mock<IAllWebinars>();
            var webinarController = new WebinarController(this.MockMapper, mock.Object);

            var result = await webinarController.UpdateWebinar(testWebinarId, new CreateOrUpdateWebinarRequest
            {
                Name = "new_webinar",
                Duration = 35,
                StartDateTime = "2020-09-01T12:00",
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = "new_series"
                }
            });

            var notFoundObjectResult = result.Result.ShouldBeOfType<NotFoundObjectResult>();
            var model = notFoundObjectResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions(
                () => model.StatusCode.ShouldBe(HttpStatusCode.NotFound),
                () => model.Message.ShouldContain($"Webinar with id={testWebinarId} not found"),
                () => model.Data.ShouldBeNull());
        }

        /// <summary>
        /// Check updating webinar with invalid data.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task UpdateWebinar_WithInvalidData_ReturnsBadRequestResult()
        {
            const int testWebinarId = 99;
            var mock = new Mock<IAllWebinars>();
            var webinarController = new WebinarController(this.MockMapper, mock.Object);

            var result = await webinarController.UpdateWebinar(testWebinarId, new CreateOrUpdateWebinarRequest());

            var badRequestObjectResult = result.Result.ShouldBeOfType<BadRequestObjectResult>();
            var model = badRequestObjectResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions(
                () => model.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => model.Data.ShouldBeNull(),
                () => model.Message.ShouldContain("Name is required"),
                () => model.Message.ShouldContain("Series is required"),
                () => model.Message.ShouldContain("StartDateTime is required"),
                () => model.Message.ShouldContain("Duration must be equal or greater than 1 minute"));
        }

        /// <summary>
        /// Check updating webinar with non unique name.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task UpdateWebinar_WithNonUniqueName_ReturnsBadRequestResult()
        {
            const int testWebinarId = 99;
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.GetAll())
                .ReturnsAsync(this.TestWebinars);
            var webinarController = new WebinarController(this.MockMapper, mock.Object);

            var result = await webinarController.UpdateWebinar(testWebinarId, new CreateOrUpdateWebinarRequest
            {
                Name = this.TestWebinars.First().Name,
                Duration = 12,
                StartDateTime = "2020-09-01T12:00",
                Series = new CreateOrUpdateSeriesRequest { Name = "series" }
            });

            var badRequestObjectResult = result.Result.ShouldBeOfType<BadRequestObjectResult>();
            var model = badRequestObjectResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions(
                () => model.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => model.Data.ShouldBeNull(),
                () => model.Message.ShouldContain("Name must be unique"));
        }
    }
}