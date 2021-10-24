using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PalindromicPrimeNumber.Services;

namespace PalindromicPrimeNumber
{
    public class Program
    {
        private static void Main()
        {
            var palindromicPrimeNumberProvider = GetServiceProvider()
                .GetRequiredService<IPalindromicPrimeNumberProvider>();

            do
            {
                var toBase = GetTargetBaseFromUser();

                var palindromicPrimeNumbersInTargetBae
                    = palindromicPrimeNumberProvider.GetPalindromicPrimeNumbers(1000, toBase);

                Display(palindromicPrimeNumbersInTargetBae);
            } while (true);
        }

        private static void Display(IEnumerable<string> palindromicPrimeNumbersInTargetBae)
        {
            foreach (var palPrime in palindromicPrimeNumbersInTargetBae)
                Console.WriteLine($"--> {palPrime}");
        }

        private static int GetTargetBaseFromUser()
        {
            Console.Write("Enter base: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out var toBase))
                Console.WriteLine("[Error] Invalid inout for base number!");

            return toBase;
        }

        private static IServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.InstallPalindromicPrimeServices();

            return serviceCollection.BuildServiceProvider();
        }
    }
}