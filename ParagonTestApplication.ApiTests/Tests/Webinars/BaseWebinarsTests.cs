namespace ParagonTestApplication.ApiTests.Tests.Webinars
{
    using System;
    using ParagonTestApplication.ApiClient.ApiHelpers;
    using ParagonTestApplication.ApiClient.ClientWrapper;
    using ParagonTestApplication.ApiTests.Common;

    /// <summary>
    /// Base class for webinar tests.
    /// </summary>
    public abstract class BaseWebinarsTests
    {
        protected readonly WebinarHelper WebinarApiHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWebinarsTests"/> class.
        /// </summary>
        protected BaseWebinarsTests()
        {
            var apiServer = ApiServer.GetInstance();
            var client = new HttpClientWrapper(apiServer.Client);
            this.WebinarApiHelper = new WebinarHelper(client);
        }

        /// <summary>
        /// Calculate webinar end datetime.
        /// </summary>
        /// <param name="startDateTime">Start datetime.</param>
        /// <param name="duration">Duration.</param>
        /// <returns>DateTime.</returns>
        protected static DateTime CalculateWebinarEndDateTime(DateTime startDateTime, int duration)
        {
            var endDateTime = startDateTime.AddMinutes(duration);
            return endDateTime;
        }
    }
}