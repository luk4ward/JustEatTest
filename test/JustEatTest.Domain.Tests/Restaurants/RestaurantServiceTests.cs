using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JustEatTest.Domain.Infrastructure;
using JustEatTest.Domain.Restaurants;
using Moq;
using NUnit.Framework;
using Serilog;

namespace JustEatTest.Domain.Tests.Controllers
{
    [TestFixture]
    public class RestaurantServiceTests
    {
        private RestaurantService _restaurantService;
        private TestHttpHandler _handler;
        private JustEatApiOptions _options;
        private Mock<ILogger> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _handler = new TestHttpHandler();
            var httpClient = new HttpClient(_handler);

            _options = new JustEatApiOptions
            {
                RestaurantsUri = "https://public.je-apis.com/restaurants",
                Host = "public.je-apis.com",
                Language = "en-GB",
                Tenant = "uk"
            };

            _loggerMock = new Mock<ILogger>();
            _loggerMock
                .Setup(x => x.ForContext(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(_loggerMock.Object);

            _restaurantService = new RestaurantService(httpClient, _options, _loggerMock.Object);
        }

        [Test]
        public void Given_empty_postcode_should_thrown_exception()
        {
            Assert.That(async () => await _restaurantService.GetByPostcodeAsync(string.Empty), Throws.ArgumentException);
        }

        [Test]
        public async Task Given_valid_postcode_should_return_restaurants()
        {
            var result = await _restaurantService.GetByPostcodeAsync("SW12 8PW");

            result.Count().Should().Be(2);
        }

        [Test]
        public async Task Given_valid_postcode_should_set_headers()
        {
            await _restaurantService.GetByPostcodeAsync("SW12 8PW");

            _handler.Request.Headers.Host.Should().Be(_options.Host);
            _handler.Request.Headers.AcceptLanguage.ToString().Should().Be(_options.Language);
            _handler.Request.Headers.Where(x => x.Key == "Accept-Tenant").FirstOrDefault().Value.FirstOrDefault().Should().Be(_options.Tenant);
        }

        [Test]
        public async Task Given_valid_postcode_should_invoke_api()
        {
            await _restaurantService.GetByPostcodeAsync("SW12 8PW");

            _handler.HitCount.Should().Be(1);
        }

        private class TestHttpHandler : HttpMessageHandler
        {
            public Func<HttpResponseMessage> HttpResponseFactory { get; set; } = () => new HttpResponseMessage();
            public int HitCount { get; set; }
            public HttpRequestMessage Request { get; set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                HitCount++;
                Request = request;
                var response = HttpResponseFactory.Invoke();
                var content = "{\"Restaurants\": [{\"Name\": \"Just Eat\"}, {\"Name\": \"PizzaHut\"}]}";
                response.Content = new StringContent(content);
                return Task.FromResult(response);
            }
        }
    }
}