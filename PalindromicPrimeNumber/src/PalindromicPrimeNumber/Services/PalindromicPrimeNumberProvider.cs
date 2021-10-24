using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PalindromicPrimeNumber.Exceptions;
using PalindromicPrimeNumber.Properties;

namespace PalindromicPrimeNumber.Services
{
    public class PalindromicPrimeNumberProvider : IPalindromicPrimeNumberProvider
    {
        private readonly IPalindromeValidator palindromeValidator;
        private readonly IPrimeNumberValidator primeNumberValidator;
        private readonly IServiceProvider serviceProvider;

        public PalindromicPrimeNumberProvider(
            IPrimeNumberValidator primeNumberValidator,
            IPalindromeValidator palindromeValidator,
            IServiceProvider serviceProvider)
        {
            this.primeNumberValidator = primeNumberValidator;
            this.palindromeValidator = palindromeValidator;
            this.serviceProvider = serviceProvider;
        }

        public IEnumerable<string> GetPalindromicPrimeNumbers(int max, int inBase)
        {
            var targetBaseConvertor = GetTargetBaseConvertor(inBase);

            for (var number = 2; number <= max; number++)
                if (primeNumberValidator.Validate(number))
                {
                    var numberInTargetBase = targetBaseConvertor.Convert(number);
                    if (palindromeValidator.Validate(numberInTargetBase))
                        yield return numberInTargetBase;
                }
        }

        private IBaseConvertor GetTargetBaseConvertor(int inBase)
        {
            var targetBaseConvertor =
                serviceProvider.GetServices<IBaseConvertor>().SingleOrDefault(i => i.Base == inBase);

            if (targetBaseConvertor == null)
                throw new UnsupportedBaseException(Messages.UnsuppotedTargetBase, inBase);

            return targetBaseConvertor;
        }
    }
}