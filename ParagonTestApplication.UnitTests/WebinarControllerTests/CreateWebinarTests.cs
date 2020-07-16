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

namespace ParagonTestApplication.UnitTests.WebinarControllerTests
{
    public class CreateWebinarTests : WebinarControllerBaseTests
    {
        [Test]
        public async Task CreateWebinar_WithValidModel_ReturnsCreatedResult()
        {
            var createOrUpdateRequest = new CreateOrUpdateWebinarRequest
            {
                Name = "new_webinar",
                Duration = 35,
                StartDateTime = "2020-09-01T12:00",
                Series = new CreateOrUpdateSeriesRequest
                {
                    Name = "new_series"
                }
            };
            var webinarForCreate = new Webinar
            {
                Id = 100,
                Name = createOrUpdateRequest.Name,
                Duration = createOrUpdateRequest.Duration,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddMinutes(createOrUpdateRequest.Duration),
                Series = new Series
                {
                    Id = 100,
                    Name = "new_series"
                }
            };
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.Create(It.IsAny<Webinar>(), createOrUpdateRequest.Series.Name))
                .ReturnsAsync(webinarForCreate);
            var webinarController = new WebinarController(MockMapper, mock.Object);

            var result = await webinarController.CreateWebinar(createOrUpdateRequest);

            var createdAtActionResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
            var model = createdAtActionResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions
            (
                () => model.StatusCode.ShouldBe(HttpStatusCode.Created),
                () => model.Message.ShouldBe("Success"),
                () => model.Data.Id.ShouldBe(webinarForCreate.Id),
                () => model.Data.StartDateTime.ShouldBe(webinarForCreate.StartDateTime),
                () => model.Data.EndDateTime.ShouldBe(webinarForCreate.EndDateTime),
                () => model.Data.Duration.ShouldBe(webinarForCreate.Duration),
                () => model.Data.Series.Id.ShouldBe(webinarForCreate.Series.Id),
                () => model.Data.Series.Name.ShouldBe(webinarForCreate.Series.Name)
            );
        }

        [Test]
        public async Task CreateWebinar_WithInvalidModel_ReturnsBadRequestResult()
        {
            var mock = new Mock<IAllWebinars>();
            var webinarController = new WebinarController(MockMapper, mock.Object);

            var result = await webinarController.CreateWebinar(new CreateOrUpdateWebinarRequest());

            var badRequestObjectResult = result.Result.ShouldBeOfType<BadRequestObjectResult>();
            var model = badRequestObjectResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions
            (
                () => model.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => model.Data.ShouldBeNull(),
                () => model.Message.ShouldContain("Name is required"),
                () => model.Message.ShouldContain("Series is required"),
                () => model.Message.ShouldContain("StartDateTime is required"),
                () => model.Message.ShouldContain("Duration must be equal or greater than 1 minute")
            );
        }

        [Test]
        public async Task CreateWebinar_WithNonUniqueWebinarName_ReturnsBadRequestResult()
        {
            var testWebinarName = TestWebinars.First().Name;
            var request = new CreateOrUpdateWebinarRequest
            {
                Name = testWebinarName,
                Duration = 1,
                Series = new CreateOrUpdateSeriesRequest {Name = "123"},
                StartDateTime = "2020-09-01T12:00"
            };
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.GetAll())
                .ReturnsAsync(TestWebinars);
            var webinarController = new WebinarController(MockMapper, mock.Object);

            var result = await webinarController.CreateWebinar(request);

            var badRequestObjectResult = result.Result.ShouldBeOfType<BadRequestObjectResult>();
            var model = badRequestObjectResult.Value.ShouldBeAssignableTo<Response<WebinarDto>>();
            model.ShouldSatisfyAllConditions
            (
                () => model.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => model.Data.ShouldBeNull(),
                () => model.Message.ShouldContain("Name must be unique")
            );
        }
    }
}