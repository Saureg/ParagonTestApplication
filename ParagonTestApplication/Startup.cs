namespace ParagonTestApplication
{
    using System;
    using System.IO;
    using System.Reflection;
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using ParagonTestApplication.Data;
    using ParagonTestApplication.Data.Contracts;
    using ParagonTestApplication.Data.Repositories;

    /// <summary>
    /// Startup class.
    /// </summary>
    public class Startup
    {
        private readonly IConfigurationRoot configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="hostEnv">Environment.</param>
        public Startup(IHostEnvironment hostEnv)
        {
            this.configuration = new ConfigurationBuilder()
                .SetBasePath(hostEnv.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MainDbContext>(options =>
                options.UseNpgsql(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IAllWebinars, WebinarRepository>();
            services.AddTransient<IAllSeries, SeriesRepository>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Paragon test application", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        // ReSharper disable once UnusedMember.Global

        /// <summary>
        /// Configure.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Environment.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/api/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            loggerFactory.AddFile("logs/log-{Date}.txt");
        }
    }
}