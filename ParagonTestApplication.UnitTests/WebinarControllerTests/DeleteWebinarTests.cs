namespace ParagonTestApplication.UnitTests.WebinarControllerTests
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
    /// Delete webinar tests.
    /// </summary>
    public class DeleteWebinarTests : WebinarControllerBaseTests
    {
        /// <summary>
        /// Delete webinar with existing id test.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task DeleteWebinar_WithExistingId_ReturnsNoContentResult()
        {
            var testWebinarId = this.TestWebinars.First().Id;
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.Get(testWebinarId))
                .ReturnsAsync(this.TestWebinars.First);
            var webinarController = new WebinarController(this.MockMapper, mock.Object);

            var result = await webinarController.DeleteWebinar(testWebinarId);

            var noContentResult = result.ShouldBeOfType<NoContentResult>();
            noContentResult.StatusCode.ShouldBe(204);
        }

        /// <summary>
        /// Delete webinar with unknown id test.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task DeleteWebinar_WithUnknownId_ReturnsNotFoundResult()
        {
            const int testWebinarId = 22;
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.GetAll())
                .ReturnsAsync(this.TestWebinars);
            var webinarController = new WebinarController(this.MockMapper, mock.Object);

            var result = await webinarController.DeleteWebinar(testWebinarId);

            var notFoundObjectResult = result.ShouldBeOfType<NotFoundObjectResult>();
            var model = notFoundObjectResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions(
                () => model.StatusCode.ShouldBe(HttpStatusCode.NotFound),
                () => model.Data.ShouldBeNull(),
                () => model.Message.ShouldContain($"Webinar with id={testWebinarId} not found"));
        }
    }
}