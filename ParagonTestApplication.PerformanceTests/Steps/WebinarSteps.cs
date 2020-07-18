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

namespace ParagonTestApplication.PerformanceTests.Steps
{
    public class WebinarSteps
    {
        private readonly WebinarHelper _webinarHelper;

        public WebinarSteps(HttpClientWrapper client)
        {
            _webinarHelper = new WebinarHelper(client);
        }

        public IStep GetWebinar()
        {
            var step = Step.Create("get_webinar", context =>
            {
                var webinarId = (int) context.Data["webinar_id"];
                var response = _webinarHelper.GetWebinar(webinarId).Result;
                return Task.FromResult(response.StatusCode == HttpStatusCode.OK ? Response.Ok() : Response.Fail());
            });
            return step;
        }

        public IStep GetWebinarList(WebinarFilter webinarFilter = null, PaginationFilter paginationFilter = null)
        {
            var step = Step.Create("get_webinar_list", context =>
            {
                var response = _webinarHelper.GetWebinarList(webinarFilter, paginationFilter).Result;
                return Task.FromResult(response.StatusCode == HttpStatusCode.OK ? Response.Ok() : Response.Fail());
            });
            return step;
        }

        public IStep CreateWebinar()
        {
            var step = Step.Create("create_webinar", context =>
            {
                var response = _webinarHelper.CreateWebinar(new CreateOrUpdateWebinarRequest
                {
                    Name = Guid.NewGuid().ToString(),
                    Duration = 60,
                    StartDateTime = "2020-09-01T12:00",
                    Series = new CreateOrUpdateSeriesRequest {Name = Guid.NewGuid().ToString()}
                }).Result;
                context.Data.Add("webinar_id", response.Data.Id);
                return Task.FromResult(response.StatusCode == HttpStatusCode.Created ? Response.Ok() : Response.Fail());
            });
            return step;
        }

        public IStep UpdateWebinar()
        {
            var step = Step.Create("update_webinar", context =>
            {
                var webinarId = (int) context.Data["webinar_id"];
                var response = _webinarHelper.UpdateWebinar(webinarId, new CreateOrUpdateWebinarRequest
                {
                    Name = Guid.NewGuid().ToString(),
                    Duration = 120,
                    StartDateTime = "2020-10-01T09:00",
                    Series = new CreateOrUpdateSeriesRequest {Name = Guid.NewGuid().ToString()}
                }).Result;
                return Task.FromResult(response.StatusCode == HttpStatusCode.Created ? Response.Ok() : Response.Fail());
            });
            return step;
        }

        public IStep DeleteWebinar()
        {
            var step = Step.Create("delete_webinar", context =>
            {
                var webinarId = (int) context.Data["webinar_id"];
                _webinarHelper.DeleteWebinar(webinarId);
                return Task.FromResult(Response.Ok());
            });
            return step;
        }
    }
}