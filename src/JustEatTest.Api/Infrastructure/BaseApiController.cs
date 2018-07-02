using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using JustEatTest.Api.Model;
using Serilog;

namespace JustEatTest.Api.Infrastructure
{
    public abstract class BaseApiController : ControllerBase
    {
        private const string RequestInvalidErrorType = "request_invalid";

        private readonly ILogger _logger;

        public BaseApiController(ILogger logger)
        {
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        protected IActionResult ValidateModelState<TValidator, TModel>(TValidator validator, TModel model)
                    where TModel : class
                    where TValidator : IValidator<TModel>
        {
            var validationResult = validator.Validate(model);

            if (validationResult?.Errors != null && validationResult.Errors.Any())
            {
                _logger
                    .ForContext("Errors", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)))
                    .Information("Validation error");
                return CreateErrorResponse(Request?.HttpContext?.TraceIdentifier, validationResult);
            }
            return null;
        }

        private IActionResult CreateErrorResponse(string requestId, ValidationResult validationResult)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
            var errorResponse = new ErrorResponse(requestId, RequestInvalidErrorType, errorMessages);
            return new ObjectResult(errorResponse) { StatusCode = StatusCodes.Status422UnprocessableEntity };
        }
    }
}