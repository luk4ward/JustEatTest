using StructureMap;
using JustEatTest.Api.Validation;
using System.Net.Http;
using JustEatTest.Domain.Restaurants;
using Serilog;
using Microsoft.Extensions.Configuration;
using JustEatTest.Domain.Infrastructure;

namespace JustEatTest.Api.Infrastructure
{
    public class AppRegistry : Registry
    {
        public AppRegistry(IConfiguration configuration, ILogger logger)
        {
            ForSingletonOf<JustEatApiOptions>()
                .Use(Bind<JustEatApiOptions>(configuration, "JustEat"));
            For<ILogger>()
                .Use(ctx => logger.ForContext(ctx.ParentType ?? ctx.RootType))
                .AlwaysUnique();
            For<IPostcodeValidator>()
                .Use<PostcodeValidator>();
            For<IRestaurantService>()
                .Use<RestaurantService>();
            For<HttpClient>()
                .Use(new HttpClient());
        }

        private static TSettings Bind<TSettings>(IConfiguration configuration, string key) where TSettings : new()
        {
            var settings = new TSettings();
            configuration.Bind(key, settings);
            return settings;
        }
    }
}