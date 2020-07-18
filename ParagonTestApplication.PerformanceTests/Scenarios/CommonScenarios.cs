using ParagonTestApplication.ApiClient.ClientWrapper;

namespace ParagonTestApplication.PerformanceTests.Scenarios
{
    public class CommonScenarios
    {
        protected readonly HttpClientWrapper Client;

        protected CommonScenarios(HttpClientWrapper client)
        {
            Client = client;
        }
    }
}