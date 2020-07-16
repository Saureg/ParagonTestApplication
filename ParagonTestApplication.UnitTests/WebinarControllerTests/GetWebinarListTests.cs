using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ParagonTestApplication.Controllers;
using ParagonTestApplication.Data.Contracts;
using ParagonTestApplication.Models.ApiModels.Common;
using ParagonTestApplication.Models.ApiModels.Webinars;
using ParagonTestApplication.Models.Common;
using ParagonTestApplication.Models.DataModels;
using Shouldly;

namespace ParagonTestApplication.UnitTests.WebinarControllerTests
{
    public class GetWebinarListTests : WebinarControllerBaseTests
    {
        [Test]
        public async Task GetWebinarList_WithoutParams_ReturnsOkResult()
        {
            var paginationFilter = new PaginationFilter();
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.GetFilteredList(It.IsAny<WebinarParameters>(), It.IsAny<PaginationFilter>()))
                .ReturnsAsync(new PagedList<Webinar>(TestWebinars, TestWebinars.Count, paginationFilter.PageNumber,
                    paginationFilter.PageSize));
            var webinarController = new WebinarController(MockMapper, mock.Object);

            var result = await webinarController.GetWebinarList(new WebinarFilter(), paginationFilter);

            var okObjectResult = result.Result.ShouldBeOfType<OkObjectResult>();
            var model = okObjectResult.Value.ShouldBeAssignableTo<Response<PagedList<WebinarDto>>>();
            model.ShouldSatisfyAllConditions
            (
                () => model.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => model.Message.ShouldContain("Success"),
                () => model.Data.CurrentPage.ShouldBe(paginationFilter.PageNumber),
                () => model.Data.PageSize.ShouldBe(paginationFilter.PageSize),
                () => model.Data.TotalCount.ShouldBe(TestWebinars.Count),
                () => model.Data.TotalPages.ShouldBe(1),
                () => model.Data.HasNext.ShouldBeFalse(),
                () => model.Data.HasPrevious.ShouldBeFalse(),
                () => model.Data.Items.Count.ShouldBe(TestWebinars.Count)
            );
        }

        [Test]
        public async Task GetWebinarList_WithParams_ReturnsOkResult()
        {
            var paginationFilter = new PaginationFilter
            {
                PageSize = 12,
                PageNumber = 123
            };
            var webinarFilter = new WebinarFilter
            {
                MinDateTime = "2010-01-01T00:00",
                MaxDateTime = "2020-09-01T12:00",
                MinDuration = "1",
                MaxDuration = "100",
                SeriesId = "1"
            };
            var mock = new Mock<IAllWebinars>();
            mock
                .Setup(x => x.GetFilteredList(It.IsAny<WebinarParameters>(), It.IsAny<PaginationFilter>()))
                .ReturnsAsync(new PagedList<Webinar>(TestWebinars, TestWebinars.Count, paginationFilter.PageNumber,
                    paginationFilter.PageSize));
            var webinarController = new WebinarController(MockMapper, mock.Object);

            var result = await webinarController.GetWebinarList(webinarFilter, paginationFilter);

            var okObjectResult = result.Result.ShouldBeOfType<OkObjectResult>();
            var model = okObjectResult.Value.ShouldBeAssignableTo<Response<PagedList<WebinarDto>>>();
            model.ShouldSatisfyAllConditions
            (
                () => model.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => model.Data.ShouldNotBeNull(),
                () => model.Message.ShouldContain("Success")
            );
        }

        [Test]
        public async Task GetWebinarList_WithInvalidParams_ReturnsBadRequestResult()
        {
            var paginationFilter = new PaginationFilter
            {
                PageSize = -1,
                PageNumber = int.MaxValue
            };
            var webinarFilter = new WebinarFilter
            {
                MinDateTime = "22-01-2010 15:43",
                MaxDateTime = "22 december",
                MinDuration = "zero",
                MaxDuration = "ten",
                SeriesId = "twelve"
            };
            var mock = new Mock<IAllWebinars>();
            var webinarController = new WebinarController(MockMapper, mock.Object);

            var result = await webinarController.GetWebinarList(webinarFilter, paginationFilter);

            var badRequestObjectResult = result.Result.ShouldBeOfType<BadRequestObjectResult>();
            var model = badRequestObjectResult.Value.ShouldBeAssignableTo<Response<PagedList<WebinarDto>>>();
            model.ShouldSatisfyAllConditions
            (
                () => model.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => model.Data.ShouldBeNull(),
                () => model.Message.ShouldContain("PageNumber must be less than 12147483647"),
                () => model.Message.ShouldContain("PageSize must be equal or greater than 1"),
                () => model.Message.ShouldContain("MinDateTime must be in format 2010-01-11T11:41"),
                () => model.Message.ShouldContain("MaxDateTime must be in format 2010-01-11T11:41"),
                () => model.Message.ShouldContain("MinDuration must be a positive valid integer"),
                () => model.Message.ShouldContain("MaxDuration must be a positive valid integer"),
                () => model.Message.ShouldContain("SeriesId must be a positive valid integer")
            );
        }
    }
}