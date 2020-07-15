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

namespace ParagonTestApplication.UnitTests.WebinarControllerTests
{
    public class DeleteWebinarTests : WebinarControllerBaseTests
    {
        [Test]
        public async Task DeleteWebinar_WithExistingId_ReturnsNoContentResult()
        {
            var testWebinarId = TestWebinars.First().Id;
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.Get(testWebinarId))
                .ReturnsAsync(TestWebinars.First);
            var webinarController = new WebinarController(MockMapper, mock.Object);

            var result = await webinarController.DeleteWebinar(testWebinarId);

            var noContentResult = result.ShouldBeOfType<NoContentResult>();
            noContentResult.StatusCode.ShouldBe(204);
        }

        [Test]
        public async Task DeleteWebinar_WithUnknownId_ReturnsNotFoundResult()
        {
            const int testWebinarId = 22;
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.GetAll())
                .ReturnsAsync(TestWebinars);
            var webinarController = new WebinarController(MockMapper, mock.Object);

            var result = await webinarController.DeleteWebinar(testWebinarId);

            var notFoundObjectResult = result.ShouldBeOfType<NotFoundObjectResult>();
            var model = notFoundObjectResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions
            (
                () => model.StatusCode.ShouldBe(HttpStatusCode.NotFound),
                () => model.Data.ShouldBeNull(),
                () => model.Message.ShouldContain($"Webinar with id={testWebinarId} not found")
            );
        }
    }
}