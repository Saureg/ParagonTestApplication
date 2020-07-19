namespace ParagonTestApplication.PerformanceTests.Scenarios
{
    using System;
    using NBomber.Contracts;
    using NBomber.CSharp;
    using ParagonTestApplication.ApiClient.ClientWrapper;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using ParagonTestApplication.Models.Common;
    using ParagonTestApplication.PerformanceTests.Config;
    using ParagonTestApplication.PerformanceTests.Steps;

    /// <summary>
    /// Webinar scenarios.
    /// </summary>
    public class WebinarScenarios : CommonScenarios
    {
        private readonly int duration;
        private readonly int minPause;
        private readonly int maxPause;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebinarScenarios"/> class.
        /// </summary>
        /// <param name="client">HttpClientWrapper.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="minPause">MinPause.</param>
        /// <param name="maxPause">MaxPause.</param>
        public WebinarScenarios(HttpClientWrapper client, int duration, int minPause, int maxPause)
            : base(client)
        {
            this.duration = duration;
            this.minPause = minPause;
            this.maxPause = maxPause;
        }

        /// <summary>
        /// Action with webinar scenario.
        /// </summary>
        /// <param name="injectionRate">Injection rate.</param>
        /// <returns>Scenario.</returns>
        public Scenario ActionsWithWebinarsScenario(InjectionRate injectionRate)
        {
            var webinarSteps = new WebinarSteps(this.Client);

            var scenario = ScenarioBuilder
                .CreateScenario(
                    "create_webinar_scenario",
                    webinarSteps.CreateWebinar(),
                    CommonSteps.Pause(this.minPause, this.maxPause),
                    webinarSteps.GetWebinar(),
                    CommonSteps.Pause(this.minPause, this.maxPause),
                    webinarSteps.UpdateWebinar(),
                    CommonSteps.Pause(this.minPause, this.maxPause),
                    webinarSteps.DeleteWebinar(),
                    CommonSteps.Pause(injectionRate.Pause))
                .WithWarmUpDuration(TimeSpan.FromMinutes(2))
                .WithLoadSimulations(
                    LoadSimulation.NewRampConcurrentScenarios(
                        injectionRate.ThreadCount,
                        TimeSpan.FromMinutes(this.duration)));

            return scenario;
        }

        /// <summary>
        /// GetWebinarList scenario.
        /// </summary>
        /// <param name="injectionRate">Injection rate.</param>
        /// <returns>Scenario.</returns>
        public Scenario GetWebinarListScenario(InjectionRate injectionRate)
        {
            var webinarSteps = new WebinarSteps(this.Client);

            var scenario = ScenarioBuilder
                .CreateScenario(
                    "get_webinar_list_scenario",
                    webinarSteps.GetWebinarList(),
                    CommonSteps.Pause(injectionRate.Pause))
                .WithWarmUpDuration(TimeSpan.FromMinutes(2))
                .WithLoadSimulations(
                    LoadSimulation.NewRampConcurrentScenarios(
                        injectionRate.ThreadCount,
                        TimeSpan.FromMinutes(this.duration)));

            return scenario;
        }

        /// <summary>
        /// GetWebinarListWithFilters scenario.
        /// </summary>
        /// <param name="injectionRate">Injection rate.</param>
        /// <returns>Scenario.</returns>
        public Scenario GetWebinarListWithFiltersScenario(InjectionRate injectionRate)
        {
            var webinarSteps = new WebinarSteps(this.Client);

            var scenario = ScenarioBuilder
                .CreateScenario(
                    "get_webinar_list_with_filters_scenario",
                    webinarSteps.GetWebinarList(new WebinarFilter
                    {
                        MaxDuration = "240",
                        MinDuration = "10",
                        MaxDateTime = "2020-12-01T09:00",
                        MinDateTime = "2020-01-01T09:00",
                    }), CommonSteps.Pause(injectionRate.Pause))
                .WithWarmUpDuration(TimeSpan.FromMinutes(2))
                .WithLoadSimulations(
                    LoadSimulation.NewRampConcurrentScenarios(
                        injectionRate.ThreadCount,
                        TimeSpan.FromMinutes(this.duration)));

            return scenario;
        }

        /// <summary>
        /// GetWebinarLargeList scenario.
        /// </summary>
        /// <param name="injectionRate">Injection rate.</param>
        /// <returns>Scenario.</returns>
        public Scenario GetWebinarLargeListScenario(InjectionRate injectionRate)
        {
            var webinarSteps = new WebinarSteps(this.Client);

            var scenario = ScenarioBuilder
                .CreateScenario(
                    "get_webinar_large_list_scenario",
                    webinarSteps.GetWebinarList(
                        new WebinarFilter { MinDuration = "200" },
                        new PaginationFilter { PageNumber = 1, PageSize = 1000 }),
                    CommonSteps.Pause(injectionRate.Pause))
                .WithWarmUpDuration(TimeSpan.FromMinutes(2))
                .WithLoadSimulations(
                    LoadSimulation.NewRampConcurrentScenarios(
                        injectionRate.ThreadCount,
                        TimeSpan.FromMinutes(this.duration)));

            return scenario;
        }
    }
}