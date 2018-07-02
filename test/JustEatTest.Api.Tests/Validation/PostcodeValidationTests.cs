using FluentAssertions;
using JustEatTest.Api.Validation;
using NUnit.Framework;

namespace JustEatTest.Api.Tests.Validation
{
    [TestFixture]
    public class PostcodeValidationTests
    {
        [TestFixture]
        public class PhoneValidatorTests
        {
            [Test]
            [TestCase("")]
            [TestCase("Test")]
            [TestCase("99501")]
            [TestCase("n4     ")]
            [TestCase("SW12 8PWxxxx")]
            [TestCase("576SW128PW")]
            [TestCase("ASD SW12 8PW")]
            public void Given_invalid_postcode_should_be_false(string postcode)
            {
                var validator = new PostcodeValidator();
                var result = validator.Validate(postcode);

                result.IsValid.Should().BeFalse();
            }

            [Test]
            [TestCase("N4")]
            [TestCase("sw12")]
            [TestCase("N4 2NE")]
            [TestCase("N42NE")]
            [TestCase("n42ne")]
            [TestCase("Sw12 8pW")]
            public void Given_valid_postcode_should_be_true(string postcode)
            {
                var validator = new PostcodeValidator();
                var result = validator.Validate(postcode);

                result.IsValid.Should().BeTrue();
            }
        }
    }
}