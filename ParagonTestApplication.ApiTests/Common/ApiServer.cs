using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace ParagonTestApplication.ApiTests.Common
{
    public class ApiServer : IDisposable
    {
        private IConfigurationRoot _config;

        public ApiServer()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        public HttpClient Client { get; private set; }

        public TestServer Server { get; private set; }

        private static ApiServer _apiServer;

        private static readonly object _lock = new object();

        public static ApiServer GetInstance()
        {
            if (_apiServer != null) return _apiServer;
            lock (_lock)
            {
                _apiServer ??= new ApiServer();
            }

            return _apiServer;
        }

        public void Dispose()
        {
            if (Client != null)
            {
                Client.Dispose();
                Client = null;
            }

            if (Server != null)
            {
                Server.Dispose();
                Server = null;
            }
        }
    }
}