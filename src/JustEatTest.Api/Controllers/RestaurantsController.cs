using System;
using System.Threading.Tasks;
using JustEatTest.Api.Infrastructure;
using JustEatTest.Api.Validation;
using JustEatTest.Domain.Restaurants;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace JustEatTest.Api.Controllers
{
    [Route("[controller]")]
    public class RestaurantsController : BaseApiController
    {
        private readonly IPostcodeValidator _restaurantPostcodeValidator;
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(
            IPostcodeValidator restaurantPostcodeValidator,
            IRestaurantService restaurantService,
            ILogger logger) : base(logger)
        {
            _restaurantPostcodeValidator = restaurantPostcodeValidator
                ?? throw new ArgumentNullException(nameof(restaurantPostcodeValidator));
            _restaurantService = restaurantService
                ?? throw new ArgumentNullException(nameof(restaurantService));
        }

        [HttpGet]
        [Route("{postcode}")]
        public async Task<IActionResult> GetAsync(string postcode)
        {
            var validationResponse = ValidateModelState(_restaurantPostcodeValidator, postcode);

            if (validationResponse != null)
                return validationResponse;
            
            var restaurants = await _restaurantService.GetByPostcodeAsync(postcode).ConfigureAwait(false);

            return Ok(restaurants);
        }

    }
}