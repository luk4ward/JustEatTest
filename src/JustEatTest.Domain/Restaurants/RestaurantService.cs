using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using JustEatTest.Domain.Http;
using JustEatTest.Domain.Infrastructure;
using JustEatTest.Domain.Restaurants.Model;
using JustEatTest.Domain.Restaurants.Model.JustEat;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Serilog;
using SerilogTimings.Extensions;

namespace JustEatTest.Domain.Restaurants
{
    public class RestaurantService : IRestaurantService
    {
        private const string AuthType = "Basic";
        private const string PostcodeQuery = "q";

        private readonly HttpClient _httpClient;
        private readonly JustEatApiOptions _options;
        private readonly ILogger _logger;

        public RestaurantService(
            HttpClient httpClient,
            JustEatApiOptions options,
            ILogger logger)
        {
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options
                ?? throw new ArgumentNullException(nameof(options));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IEnumerable<Restaurant>> GetByPostcodeAsync(string postcode)
        {
            if (string.IsNullOrEmpty(postcode))
                throw new ArgumentException("Postcode is required", nameof(postcode));

            var queries = new Dictionary<string, string> { { PostcodeQuery, postcode } };

            var request = BuildRequest(HttpMethod.Get, queries);

            return InvokeAsync(request);
        }

        private HttpRequestMessage BuildRequest(HttpMethod method, Dictionary<string, string> queries)
        {
            var requestUri = QueryHelpers.AddQueryString(_options.RestaurantsUri, queries);

            var httpRequest = new HttpRequestMessage(method, requestUri);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(AuthType, _options.Authorization);
            httpRequest.Headers.Add(HttpHelper.AcceptLanguageHeader, _options.Language);
            httpRequest.Headers.Add(HttpHelper.HostHeader, _options.Host);
            httpRequest.Headers.Add(HttpHelper.AcceptTenantHeader, _options.Tenant);

            return httpRequest;
        }

        private async Task<IEnumerable<Restaurant>> InvokeAsync(HttpRequestMessage request)
        {
            var cancellationTokenSource = new CancellationTokenSource(_options.RequestTimeout);

            try
            {
                HttpResponseMessage httpResponse = null;

                using (var op = _logger.BeginOperation("Just Eat API Restaurants Lookup"))
                {
                    httpResponse = await
                         _httpClient.SendAsync(request, cancellationTokenSource.Token).ConfigureAwait(false);

                    if (!httpResponse.IsSuccessStatusCode && httpResponse.StatusCode != HttpStatusCode.NotFound)
                        httpResponse.EnsureSuccessStatusCode();

                    op.Complete();
                }

                if (!httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
                    return null;

                var responseJson = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                httpResponse.Dispose();

                var restaurants = JsonConvert.DeserializeObject<RestaurantResponse>(responseJson);
                return restaurants?.Restaurants;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error calling Just Eat API");
                return null;
            }
        }
    }
}
