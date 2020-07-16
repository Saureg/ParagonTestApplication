using System;
using ParagonTestApplication.ApiTests.Common;
using ParagonTestApplication.ApiTests.Helpers;

namespace ParagonTestApplication.ApiTests.Tests.Webinars
{
    public abstract class BaseWebinarsTests
    {
        protected readonly WebinarHelper WebinarApiHelper;

        protected BaseWebinarsTests()
        {
            var apiServer = ApiServer.GetInstance();
            var client = new HttpClientWrapper(apiServer.Client);
            WebinarApiHelper = new WebinarHelper(client);
        }

        protected static DateTime CalculateWebinarEndDateTime(DateTime startDateTime, int duration)
        {
            var endDateTime = startDateTime.AddMinutes(duration);
            return endDateTime;
        }
    }
}