using System;
using NBomber.Contracts;
using NBomber.CSharp;
using ParagonTestApplication.ApiClient.ClientWrapper;
using ParagonTestApplication.Models.ApiModels.Webinars;
using ParagonTestApplication.Models.Common;
using ParagonTestApplication.PerformanceTests.Config;
using ParagonTestApplication.PerformanceTests.Steps;

namespace ParagonTestApplication.PerformanceTests.Scenarios
{
    public class WebinarScenarios : CommonScenarios
    {
        private readonly int _duration;
        private readonly int _minPause;
        private readonly int _maxPause;

        public WebinarScenarios(HttpClientWrapper client, int duration, int minPause, int maxPause) : base(client)
        {
            _duration = duration;
            _minPause = minPause;
            _maxPause = maxPause;
        }

        public Scenario ActionsWithWebinarsScenario(InjectionRate injectionRate)
        {
            var webinarSteps = new WebinarSteps(Client);

            var scenario = ScenarioBuilder
                .CreateScenario(
                    "create_webinar_scenario",
                    webinarSteps.CreateWebinar(),
                    CommonSteps.Pause(_minPause, _maxPause),
                    webinarSteps.GetWebinar(),
                    CommonSteps.Pause(_minPause, _maxPause),
                    webinarSteps.UpdateWebinar(),
                    CommonSteps.Pause(_minPause, _maxPause),
                    webinarSteps.DeleteWebinar(),
                    CommonSteps.Pause(injectionRate.Pause)
                )
                .WithWarmUpDuration(TimeSpan.FromMinutes(2))
                .WithLoadSimulations(
                    LoadSimulation.NewRampConcurrentScenarios(injectionRate.ThreadCount,
                        TimeSpan.FromMinutes(_duration)));

            return scenario;
        }

        public Scenario GetWebinarListScenario(InjectionRate injectionRate)
        {
            var webinarSteps = new WebinarSteps(Client);

            var scenario = ScenarioBuilder
                .CreateScenario(
                    "get_webinar_list_scenario",
                    webinarSteps.GetWebinarList(),
                    CommonSteps.Pause(injectionRate.Pause)
                )
                .WithWarmUpDuration(TimeSpan.FromMinutes(2))
                .WithLoadSimulations(
                    LoadSimulation.NewRampConcurrentScenarios(injectionRate.ThreadCount,
                        TimeSpan.FromMinutes(_duration)));

            return scenario;
        }

        public Scenario GetWebinarListWithFiltersScenario(InjectionRate injectionRate)
        {
            var webinarSteps = new WebinarSteps(Client);

            var scenario = ScenarioBuilder
                .CreateScenario(
                    "get_webinar_list_with_filters_scenario",
                    webinarSteps.GetWebinarList(new WebinarFilter
                    {
                        MaxDuration = "240",
                        MinDuration = "10",
                        MaxDateTime = "2020-12-01T09:00",
                        MinDateTime = "2020-01-01T09:00"
                    }),
                    CommonSteps.Pause(injectionRate.Pause)
                )
                .WithWarmUpDuration(TimeSpan.FromMinutes(2))
                .WithLoadSimulations(
                    LoadSimulation.NewRampConcurrentScenarios(injectionRate.ThreadCount,
                        TimeSpan.FromMinutes(_duration)));

            return scenario;
        }

        public Scenario GetWebinarLargeListScenario(InjectionRate injectionRate)
        {
            var webinarSteps = new WebinarSteps(Client);

            var scenario = ScenarioBuilder
                .CreateScenario(
                    "get_webinar_large_list_scenario",
                    webinarSteps.GetWebinarList(
                        new WebinarFilter
                        {
                            MinDuration = "200"
                        },
                        new PaginationFilter
                        {
                            PageNumber = 1,
                            PageSize = 1000
                        }),
                    CommonSteps.Pause(injectionRate.Pause)
                )
                .WithWarmUpDuration(TimeSpan.FromMinutes(2))
                .WithLoadSimulations(
                    LoadSimulation.NewRampConcurrentScenarios(injectionRate.ThreadCount,
                        TimeSpan.FromMinutes(_duration)));

            return scenario;
        }
    }
}