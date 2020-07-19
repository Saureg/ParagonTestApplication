namespace ParagonTestApplication.PerformanceTests.Config
{
    using Microsoft.Extensions.Configuration;
    using NUnit.Framework;

    /// <summary>
    /// Configuration.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Get config.
        /// </summary>
        /// <returns>ConfigModel.</returns>
        public static ConfigModel GetConfig()
        {
            var confPath = TestContext.CurrentContext.WorkDirectory;

            var config = new ConfigurationBuilder()
                .SetBasePath(confPath)
                .AddJsonFile("config.json", false, false)
                .Build()
                .Get<ConfigModel>();

            return config;
        }
    }
}