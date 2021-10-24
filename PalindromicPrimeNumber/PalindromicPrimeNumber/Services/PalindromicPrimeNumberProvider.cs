using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PalindromicPrimeNumber.Services
{
    public class PalindromicPrimeNumberProvider : IPalindromicPrimeNumberProvider
    {
        private readonly IPalindromicValidator palindromicValidator;
        private readonly IPrimeNumberValidator primeNumberValidator;
        private readonly IServiceProvider serviceProvider;

        public PalindromicPrimeNumberProvider(
            IPrimeNumberValidator primeNumberValidator,
            IPalindromicValidator palindromicValidator,
            IServiceProvider serviceProvider)
        {
            this.primeNumberValidator = primeNumberValidator;
            this.palindromicValidator = palindromicValidator;
            this.serviceProvider = serviceProvider;
        }

        public IEnumerable<string> GetPalindromicPrimeNumbers(int max, int inBase)
        {
            var targetBaseConvertor =
                serviceProvider.GetServices<IBaseConvertor>().SingleOrDefault(i => i.Base == inBase);
            if (targetBaseConvertor == null)
                throw new NotSupportedException($"Not supported target base, base='{inBase}'.");

            for (var number = 2; number <= max; number++)
                if (primeNumberValidator.Validate(number))
                {
                    var numberInTargetBase = targetBaseConvertor.Convert(number);
                    if (palindromicValidator.Validate(numberInTargetBase))
                        yield return numberInTargetBase;
                }
        }
    }
}