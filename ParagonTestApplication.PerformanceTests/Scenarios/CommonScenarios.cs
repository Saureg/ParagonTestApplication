namespace ParagonTestApplication.PerformanceTests.Scenarios
{
    using ParagonTestApplication.ApiClient.ClientWrapper;

    /// <summary>
    /// Common scenario.
    /// </summary>
    public class CommonScenarios
    {
        /// <summary>
        /// Client.
        /// </summary>
        protected readonly HttpClientWrapper Client;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonScenarios"/> class.
        /// </summary>
        /// <param name="client">HttpClientWrapper.</param>
        protected CommonScenarios(HttpClientWrapper client)
        {
            this.Client = client;
        }
    }
}