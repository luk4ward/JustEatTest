using FluentValidation;

namespace JustEatTest.Api.Validation
{
    public interface IPostcodeValidator : IValidator<string>
    {
    }
}