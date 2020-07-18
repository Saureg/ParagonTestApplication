using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace ParagonTestApplication.PerformanceTests.Config
{
    public static class Config
    {
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