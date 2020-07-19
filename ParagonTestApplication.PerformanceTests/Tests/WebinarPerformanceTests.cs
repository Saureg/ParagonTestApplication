namespace ParagonTestApplication.PerformanceTests.Tests
{
    using System.Linq;
    using System.Net.Http;
    using NBomber.CSharp;
    using NUnit.Framework;
    using ParagonTestApplication.ApiClient.ClientWrapper;
    using ParagonTestApplication.PerformanceTests.Scenarios;

    /// <summary>
    /// Webinar performance tests.
    /// </summary>
    public class WebinarPerformanceTests
    {
        /// <summary>
        /// Performance test.
        /// </summary>
        [Test]
        public void WebinarTest()
        {
            var config = Config.Config.GetConfig();
            var actionsInjectRate = config.InjectionRates.First(x => x.Name == "Actions");
            var listInjectRate = config.InjectionRates.First(x => x.Name == "List");

            var client = new HttpClient(new HttpClientHandler()) { BaseAddress = config.Url };
            var clientWrapper = new HttpClientWrapper(client);

            var webinarScenarios =
                new WebinarScenarios(clientWrapper, config.Duration, config.MinPause, config.MaxPause);
            NBomberRunner
                .RegisterScenarios(
                    webinarScenarios.ActionsWithWebinarsScenario(actionsInjectRate),
                    webinarScenarios.GetWebinarListScenario(listInjectRate),
                    webinarScenarios.GetWebinarListWithFiltersScenario(listInjectRate),
                    webinarScenarios.GetWebinarLargeListScenario(listInjectRate))
                .Run();
        }
    }
}