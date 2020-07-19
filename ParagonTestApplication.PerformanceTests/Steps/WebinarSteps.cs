namespace ParagonTestApplication.PerformanceTests.Steps
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using NBomber.Contracts;
    using NBomber.CSharp;
    using ParagonTestApplication.ApiClient.ApiHelpers;
    using ParagonTestApplication.ApiClient.ClientWrapper;
    using ParagonTestApplication.Models.ApiModels.Series;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using ParagonTestApplication.Models.Common;

    /// <summary>
    /// Steps.
    /// </summary>
    public class WebinarSteps
    {
        private readonly WebinarHelper webinarHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebinarSteps"/> class.
        /// </summary>
        /// <param name="client">HttpClientWrapper.</param>
        public WebinarSteps(HttpClientWrapper client)
        {
            this.webinarHelper = new WebinarHelper(client);
        }

        /// <summary>
        /// Get webinar.
        /// </summary>
        /// <returns>Step.</returns>
        public IStep GetWebinar()
        {
            var step = Step.Create("get_webinar", context =>
            {
                var webinarId = (int)context.Data["webinar_id"];
                var response = this.webinarHelper.GetWebinar(webinarId).Result;
                return Task.FromResult(response.StatusCode == HttpStatusCode.OK ? Response.Ok() : Response.Fail());
            });
            return step;
        }

        /// <summary>
        /// Get webinars.
        /// </summary>
        /// <param name="webinarFilter">Webinar filter.</param>
        /// <param name="paginationFilter">Pagination filter.</param>
        /// <returns>Step.</returns>
        public IStep GetWebinarList(WebinarFilter webinarFilter = null, PaginationFilter paginationFilter = null)
        {
            var step = Step.Create("get_webinar_list", async context =>
            {
                var response = await this.webinarHelper.GetWebinarList(webinarFilter, paginationFilter);
                return response.StatusCode == HttpStatusCode.OK ? Response.Ok() : Response.Fail();
            });
            return step;
        }

        /// <summary>
        /// Create webinar.
        /// </summary>
        /// <returns>Step.</returns>
        public IStep CreateWebinar()
        {
            var step = Step.Create("create_webinar", async context =>
            {
                var response = await this.webinarHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
                {
                    Name = Guid.NewGuid().ToString(),
                    Duration = 60,
                    StartDateTime = "2020-09-01T12:00",
                    Series = new CreateOrUpdateSeriesRequest
                    { Name = Guid.NewGuid().ToString() },
                });
                context.Data.Add("webinar_id", response.Data.Id);
                return response.StatusCode == HttpStatusCode.Created ? Response.Ok() : Response.Fail();
            });
            return step;
        }

        /// <summary>
        /// Update webinar.
        /// </summary>
        /// <returns>Step.</returns>
        public IStep UpdateWebinar()
        {
            var step = Step.Create("update_webinar", async context =>
            {
                var webinarId = (int)context.Data["webinar_id"];
                var response = await this.webinarHelper.UpdateWebinar(webinarId, new CreateOrUpdateWebinarRequest
                {
                    Name = Guid.NewGuid().ToString(),
                    Duration = 120,
                    StartDateTime = "2020-10-01T09:00",
                    Series = new CreateOrUpdateSeriesRequest { Name = Guid.NewGuid().ToString() },
                });
                return response.StatusCode == HttpStatusCode.Created ? Response.Ok() : Response.Fail();
            });
            return step;
        }

        /// <summary>
        /// Delet webinar.
        /// </summary>
        /// <returns>Step.</returns>
        public IStep DeleteWebinar()
        {
            var step = Step.Create("delete_webinar", async context =>
            {
                var webinarId = (int)context.Data["webinar_id"];
                await this.webinarHelper.DeleteWebinar(webinarId);
                return Response.Ok();
            });
            return step;
        }
    }
}