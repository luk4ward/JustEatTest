using FluentValidation;

namespace JustEatTest.Api.Validation
{
    public class PostcodeValidator : AbstractValidator<string>, IPostcodeValidator
    {
        private const string _ukPostCodeRegex =
            @"^([Gg][Ii][Rr] 0[Aa]{0,2})|^((([A-Za-z][0-9]{0,2})|^(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{0,2})|^(([A-Za-z][0-9][A-Za-z])|^([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{0,2}$)";

        public PostcodeValidator()
        {
            RuleFor(x => x)
                .Matches(_ukPostCodeRegex)
                .WithMessage("Invalid postcode");
        }
    }
}