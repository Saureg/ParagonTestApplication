namespace ParagonTestApplication.ApiTests.Common
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Server for API tests.
    /// </summary>
    public class ApiServer : IDisposable
    {
        private static readonly object Lock = new object();

        private static ApiServer apiServer;

        private ApiServer()
        {
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            this.Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            this.Client = this.Server.CreateClient();
        }

        /// <summary>
        /// Gets httpClient.
        /// </summary>
        public HttpClient Client { get; private set; }

        private TestServer Server { get; set; }

        /// <summary>
        /// Get server instance.
        /// </summary>
        /// <returns>Server instance.</returns>
        public static ApiServer GetInstance()
        {
            if (apiServer != null)
            {
                return apiServer;
            }

            lock (Lock)
            {
                apiServer ??= new ApiServer();
            }

            return apiServer;
        }

        /// <summary>
        /// Dispose server.
        /// </summary>
        public void Dispose()
        {
            if (this.Client != null)
            {
                this.Client.Dispose();
                this.Client = null;
            }

            if (this.Server == null)
            {
                return;
            }

            this.Server.Dispose();
            this.Server = null;
        }
    }
}