using FluentAssertions;
using PalindromicPrimeNumber;
using PalindromicPrimeNumber.Services;
using Xunit;

namespace PalindromicPrimeNumbers.Test
{
    public class PalindromicValidatorUnitTest
    {
        [Theory]
        [InlineData("A")]
        [InlineData("c")]
        [InlineData("1")]
        [InlineData("121")]
        [InlineData("131")]
        [InlineData("929")]
        [InlineData("AbA")]
        [InlineData("aba")]
        [InlineData("aBa")]
        [InlineData("1221")]
        [InlineData("12ABc%%cBA21")]
        [InlineData("12ABc%%%cBA21")]
        [InlineData("12ABc%2%cBA21")]
        public void TestIsPalindromic_WhenValueIsPalindromic_ReturnsTrue(string value)
        {
            new PalindromicValidator().Validate(value).Should().BeTrue();
        }

        [Theory]
        [InlineData("Ab")]
        [InlineData("34")]
        [InlineData("1c")]
        [InlineData("389")]
        [InlineData("12c1")]
        [InlineData("1341")]
        [InlineData("AbAc")]
        [InlineData("aba4")]
        [InlineData("aBaB")]
        [InlineData("12212")]
        [InlineData("12ABcc%%cBA21")]
        [InlineData("12ABc%c%%cBA21")]
        [InlineData("12ABc%2c%cBA21")]
        public void TestIsPalindromic_WhenValueIsNotPalindromic_ReturnsTrue(string value)
        {
            new PalindromicValidator().Validate(value).Should().BeFalse();
        }
    }
}