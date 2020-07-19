namespace ParagonTestApplication.ApiTests.Tests.Webinars
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using ParagonTestApplication.ApiTests.Models.TestDataModels.Webinars;
    using ParagonTestApplication.ApiTests.TestData;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using ParagonTestApplication.Models.Common;
    using Shouldly;

    /// <summary>
    /// Get webinar list tests.
    /// </summary>
    public class GetWebinarListTests : BaseWebinarsTests
    {
        /// <summary>
        /// Check webinar from list.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarFromListTest()
        {
            var createdWebinarResponse =
                await this.WebinarApiHelper.CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest());
            var webinar = createdWebinarResponse.Data;

            var response = await this.WebinarApiHelper.GetWebinarList(paginationFilter: new PaginationFilter
            {
                PageSize = 100500
            });

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Message.ShouldBe("Success"),
                () => response.Data.Items.ShouldContain(x => x.Id == webinar.Id));

            var foundedWebinar = response.Data.Items.First(x => x.Id == webinar.Id);
            foundedWebinar.ShouldSatisfyAllConditions(
                () => foundedWebinar.Name.ShouldBe(webinar.Name),
                () => foundedWebinar.Duration.ShouldBe(webinar.Duration),
                () => foundedWebinar.StartDateTime.ShouldBe(webinar.StartDateTime),
                () => foundedWebinar.EndDateTime.ShouldBe(webinar.EndDateTime),
                () => foundedWebinar.Series.Id.ShouldBe(webinar.Series.Id),
                () => foundedWebinar.Series.Name.ShouldBe(webinar.Series.Name));
        }

        /// <summary>
        /// Check webinar list without params.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarListWithoutParamsTest()
        {
            var response = await this.WebinarApiHelper.GetWebinarList();

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Message.ShouldBe("Success"),
                () => response.Data.CurrentPage.ShouldBe(1),
                () => response.Data.PageSize.ShouldBe(50),
                () => response.Data.HasPrevious.ShouldBeFalse(),
                () => response.Data.Items.Count.ShouldBePositive());
        }

        /// <summary>
        /// Check webinar list with pagination filter.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarListWithPaginationFilterTest()
        {
            await this.WebinarApiHelper.CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest());
            await this.WebinarApiHelper.CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest());

            const int pageNumber = 2;
            const int pageSize = 1;

            var response =
                await this.WebinarApiHelper.GetWebinarList(paginationFilter: new PaginationFilter(pageNumber, pageSize));

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Data.Items.Count.ShouldBe(1),
                () => response.Data.CurrentPage.ShouldBe(pageNumber),
                () => response.Data.PageSize.ShouldBe(pageSize),
                () => response.Data.HasPrevious.ShouldBeTrue(),
                () => response.Data.HasNext.ShouldBeTrue(),
                () => response.Data.TotalCount.ShouldBeGreaterThanOrEqualTo(3),
                () => response.Data.TotalPages.ShouldBeGreaterThanOrEqualTo(3));
        }

        /// <summary>
        /// Check webinar list with date filter.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarListWithDateFilterTest()
        {
            var webinarResponse1 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(
                    startDateTime: "2020-12-12T12:00",
                    duration: 60));
            var webinarResponse2 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(
                    startDateTime: "2020-12-12T11:00",
                    duration: 61));
            var webinarResponse3 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(
                    startDateTime: "2020-12-12T16:00",
                    duration: 60));

            var response =
                await this.WebinarApiHelper.GetWebinarList(
                    new WebinarFilter
                    {
                        MinDateTime = "2020-12-12T12:00",
                        MaxDateTime = "2020-12-12T15:00"
                    },
                    new PaginationFilter
                    {
                        PageSize = 100500
                    });

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Data.Items.ShouldContain(x => x.Id == webinarResponse1.Data.Id),
                () => response.Data.Items.ShouldContain(x => x.Id == webinarResponse2.Data.Id),
                () => response.Data.Items.ShouldNotContain(x => x.Id == webinarResponse3.Data.Id));
        }

        /// <summary>
        /// Check webinar list with duration filter.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarListWithDurationFilterTest()
        {
            var webinarResponse1 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(duration: 100));
            var webinarResponse2 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(duration: 200));
            var webinarResponse3 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(duration: 300));

            var response =
                await this.WebinarApiHelper.GetWebinarList(
                    new WebinarFilter
                    {
                        MinDuration = "100",
                        MaxDuration = "250"
                    },
                    new PaginationFilter
                    {
                        PageSize = 100500
                    });

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Data.Items.ShouldContain(x => x.Id == webinarResponse1.Data.Id),
                () => response.Data.Items.ShouldContain(x => x.Id == webinarResponse2.Data.Id),
                () => response.Data.Items.ShouldNotContain(x => x.Id == webinarResponse3.Data.Id));
        }

        /// <summary>
        /// Check webinar list with series id filter.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarListWithSeriesIdFilterTest()
        {
            var webinarResponse1 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(seriesName: Guid.NewGuid().ToString()));
            var webinarResponse2 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(seriesName: Guid.NewGuid().ToString()));

            var response =
                await this.WebinarApiHelper.GetWebinarList(
                    new WebinarFilter
                    {
                        SeriesId = webinarResponse1.Data.Series.Id.ToString()
                    },
                    new PaginationFilter
                    {
                        PageSize = 100500
                    });

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Data.Items.ShouldContain(x => x.Id == webinarResponse1.Data.Id),
                () => response.Data.Items.ShouldNotContain(x => x.Id == webinarResponse2.Data.Id));
        }

        /// <summary>
        /// Check webinar list with all filters.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarListWithAllFilterTest()
        {
            var seriesName = Guid.NewGuid().ToString();
            var webinarResponse1 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(
                    startDateTime: "2020-12-12T11:00",
                    duration: 60,
                    seriesName: seriesName));
            var webinarResponse2 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(
                    startDateTime: "2020-12-12T11:00",
                    duration: 80,
                    seriesName: seriesName));
            var webinarResponse3 = await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(
                    startDateTime: "2020-12-13T11:00",
                    duration: 60,
                    seriesName: Guid.NewGuid().ToString()));

            var response =
                await this.WebinarApiHelper.GetWebinarList(
                    new WebinarFilter
                    {
                        MinDateTime = "2020-12-12T11:30",
                        MaxDateTime = "2020-12-13T10:30",
                        MinDuration = "50",
                        MaxDuration = "70",
                        SeriesId = webinarResponse1.Data.Series.Id.ToString()
                    },
                    new PaginationFilter
                    {
                        PageSize = 10
                    });

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Data.TotalCount.ShouldBe(1),
                () => response.Data.TotalPages.ShouldBe(1),
                () => response.Data.Items.ShouldContain(x => x.Id == webinarResponse1.Data.Id),
                () => response.Data.Items.ShouldNotContain(x => x.Id == webinarResponse2.Data.Id),
                () => response.Data.Items.ShouldNotContain(x => x.Id == webinarResponse3.Data.Id));
        }

        /// <summary>
        /// Check webinar list with incorrect range in filters.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarListWithIncorrectRangeInFiltersTest()
        {
            await this.WebinarApiHelper
                .CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest(
                    startDateTime: "2020-12-12T11:00",
                    duration: 60,
                    seriesName: Guid.NewGuid().ToString()));

            var response =
                await this.WebinarApiHelper.GetWebinarList(new WebinarFilter
                {
                    MinDateTime = "2020-12-12T12:00",
                    MaxDateTime = "2020-12-12T10:00",
                    MinDuration = "70",
                    MaxDuration = "50"
                });

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.OK),
                () => response.Data.Items.Count.ShouldBe(0));
        }

        /// <summary>
        /// Check webinar list validation.
        /// </summary>
        /// <param name="validationTestData">Test data.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task GetWebinarListValidationTest(
            [ValueSource(typeof(GetListWebinarTestData), nameof(GetListWebinarTestData.GenerateValidationTestDataList))]
            GetListTestDataModel validationTestData)
        {
            var response = await this.WebinarApiHelper
                .GetWebinarList(validationTestData.WebinarFilter, validationTestData.PaginationFilter);

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Data.ShouldBeNull(),
                () => response.Message.ShouldBe(validationTestData.ExpectedValidationMessage));
        }
    }
}