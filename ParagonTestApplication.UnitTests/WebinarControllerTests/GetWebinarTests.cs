﻿namespace ParagonTestApplication.UnitTests.WebinarControllerTests
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
    using ParagonTestApplication.Controllers;
    using ParagonTestApplication.Data.Contracts;
    using ParagonTestApplication.Models.ApiModels.Common;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using Shouldly;

    /// <summary>
    /// Get webinar tests.
    /// </summary>
    public class GetWebinarTests : WebinarControllerBaseTests
    {
        /// <summary>
        /// Check get webinar with existing id.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinar_WithExistingId_ReturnsOkResult()
        {
            const int testWebinarId = 1;
            var expectedWebinar = this.TestWebinars.First(x => x.Id == testWebinarId);
            var webinarController = this.InitializeWebinarControllerForGetTests(testWebinarId);

            var result = await webinarController.GetWebinar(testWebinarId);

            var okObjectResult = result.Result.ShouldBeOfType<OkObjectResult>();
            var model = okObjectResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions(
                () => model.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => model.Message.ShouldBe("Success"),
                () => model.Data.Id.ShouldBe(expectedWebinar.Id),
                () => model.Data.Name.ShouldBe(expectedWebinar.Name),
                () => model.Data.Duration.ShouldBe(expectedWebinar.Duration),
                () => model.Data.StartDateTime.ShouldBe(expectedWebinar.StartDateTime),
                () => model.Data.EndDateTime.ShouldBe(expectedWebinar.EndDateTime),
                () => model.Data.Series.Id.ShouldBe(expectedWebinar.Series.Id),
                () => model.Data.Series.Name.ShouldBe(expectedWebinar.Series.Name));
        }

        /// <summary>
        /// Check get webinar with unknown id.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinar_WithUnknownId_ReturnsNotFoundResult()
        {
            var webinarController = this.InitializeWebinarControllerForGetTests(1);

            const int unknownId = 0;
            var result = await webinarController.GetWebinar(unknownId);

            var notFoundObjectResult = result.Result.ShouldBeOfType<NotFoundObjectResult>();
            var model = notFoundObjectResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions(
                () => model.StatusCode.ShouldBe(HttpStatusCode.NotFound),
                () => model.Message.ShouldBe($"Webinar with id={unknownId} not found"),
                () => model.Data.ShouldBeNull());
        }

        private WebinarController InitializeWebinarControllerForGetTests(int testWebinarId)
        {
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.Get(testWebinarId))
                .ReturnsAsync(this.TestWebinars.First(x => x.Id == testWebinarId));
            var webinarController = new WebinarController(this.MockMapper, mock.Object);
            return webinarController;
        }
    }
}