using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PalindromicPrimeNumber.Exceptions;
using PalindromicPrimeNumber.Properties;
using PalindromicPrimeNumber.Services;
using Xunit;

namespace PalindromicPrimeNumber.IntegrationTest
{
    public class PalindromicPrimeNumbersProviderTest
    {
        private readonly int[] decimalPalPrimesLessThan1000 =
        {
            2, 3, 5, 7, 11, 101, 131, 151, 181, 191, 313,
            353, 373, 383, 727, 757, 787, 797, 919, 929
        };

        private readonly string[] binaryPalPrimesLessThan1000 =
        {
            "11", "101", "111", "10001", "11111", "1001001", "1101011",
            "1111111", "100000001", "100111001", "110111011"
        };
        
        private static IPalindromicPrimeNumberProvider GetPalindromicPrimeNumberProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.InstallPalindromicPrimeServices();

            return serviceCollection
                .BuildServiceProvider()
                .GetRequiredService<IPalindromicPrimeNumberProvider>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(16)]
        [InlineData(3)]
        [InlineData(5)]
        public void TestGetPalindromicPrimeNumbers_WhenBaseIsUndefined_ThrowsUnsupportedBaseException(int targetBase)
        {
            var palindromicPrimeNumberProvider = GetPalindromicPrimeNumberProvider();

            Action action = () => palindromicPrimeNumberProvider.GetPalindromicPrimeNumbers(1000, targetBase).ToArray();

            action.Should().Throw<UnsupportedBaseException>()
                .WithMessage(string.Format(Messages.UnsuppotedTargetBase, targetBase));
        }

        [Fact]
        public void TestGetPalindromicPrimeNumbers_WhenBaseIsBinary_ResultMustBeAsExpected()
        {
            var palindromicPrimeNumberProvider = GetPalindromicPrimeNumberProvider();
            var palPrimesLessThan1000 = palindromicPrimeNumberProvider.GetPalindromicPrimeNumbers(1000, 2);

            palPrimesLessThan1000.Should().Equal(binaryPalPrimesLessThan1000);
        }

        [Fact]
        public void GetPalindromicPrimeNumbers_WhenBaseIsDecimal_ResultMustBeAsExpected()
        {
            var palindromicPrimeNumberProvider = GetPalindromicPrimeNumberProvider();
            var palPrimesLessThan1000 = palindromicPrimeNumberProvider.GetPalindromicPrimeNumbers(1000, 10);

            palPrimesLessThan1000.Should().Equal(decimalPalPrimesLessThan1000.Select(i => i.ToString()));
        }
    }
}