using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.Results;
using JustEatTest.Api.Controllers;
using JustEatTest.Api.Validation;
using JustEatTest.Domain.Restaurants;
using JustEatTest.Domain.Restaurants.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Serilog;

namespace JustEatTest.Api.Tests.Controllers
{
    [TestFixture]
    public class RestaurantsControllerTests
    {
        private RestaurantsController _restaurantController;
        private Mock<IPostcodeValidator> _postcodeValidatorMock;
        private Mock<IRestaurantService> _restaurantServiceMock;
        private Mock<ILogger> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _postcodeValidatorMock = new Mock<IPostcodeValidator>();
            _restaurantServiceMock = new Mock<IRestaurantService>();
            _loggerMock = new Mock<ILogger>();

            _postcodeValidatorMock
                .Setup(x => x.Validate(It.IsAny<string>()))
                .Returns(new ValidationResult());

            _loggerMock
                .Setup(x => x.ForContext(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(_loggerMock.Object);

            _restaurantController = new RestaurantsController(_postcodeValidatorMock.Object, _restaurantServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Given_invalid_model_should_return_422()
        {
            var errorResult = new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("InvalidPostcode",
                    "Postcode is invalid")
            });
            _postcodeValidatorMock
                .Setup(x => x.Validate(It.IsAny<string>()))
                .Returns(errorResult);

            _restaurantServiceMock
                .Setup(x => x.GetByPostcodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Restaurant>());

            var result = await _restaurantController.GetAsync("postcode").ConfigureAwait(false);
            var badResult = result as ObjectResult;

            badResult.Should().NotBeNull();
            badResult.StatusCode.Should().Be(422);
        }

        [Test]
        public async Task Given_valid_model_should_return_200()
        {
            _restaurantServiceMock
                .Setup(x => x.GetByPostcodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Restaurant>());

            var result = await _restaurantController.GetAsync("postcode").ConfigureAwait(false);
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }
    }
}