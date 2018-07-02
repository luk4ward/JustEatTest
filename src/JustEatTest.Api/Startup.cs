using JustEatTest.Api.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using StructureMap;
using JustEatTest.Api.Infrastructure;
using System;

namespace JustEatTest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.WithProperty("Version", ReflectionUtils.GetAssemblyVersion<Startup>())
                .CreateLogger();

            if (env.IsDevelopment())
                Logger.Debug(Configuration.Dump());
        }

        public IConfiguration Configuration { get; }
        public Serilog.ILogger Logger { get; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore(options =>
                {
                })
                .AddJsonFormatters(serializerOptions =>
                {
                    // Snake casing by default
                    serializerOptions.NullValueHandling = NullValueHandling.Ignore;
                    serializerOptions.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });

            var container = new Container(cfg =>
                {
                    cfg.AddRegistry(new AppRegistry(Configuration, Logger));
                    cfg.Populate(services);
                }
            );
            services.Configure<RouteOptions>(options =>
                options.LowercaseUrls = true);
                
            return container.GetInstance<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog(Logger);

            var pathBase = Configuration.GetValue<string>("PathBase");
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseMiddleware<RequestLoggingMiddleware>(Logger);
            app.UseMvc();
        }
    }
}
