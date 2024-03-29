﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PalindromicPrimeNumber.Properties;
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
                try
                {
                    var toBase = GetTargetBaseFromUser();

                    var palindromicPrimeNumbersInTargetBase
                        = palindromicPrimeNumberProvider.GetPalindromicPrimeNumbers(1000, toBase);

                    Display(palindromicPrimeNumbersInTargetBase);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (true);
        }

        private static void Display(IEnumerable<string> palindromicPrimeNumbersInTargetBae)
        {
            foreach (var palPrime in palindromicPrimeNumbersInTargetBae)
                Console.WriteLine($"--> {palPrime}");
        }

        private static int GetTargetBaseFromUser()
        {
            Console.Write(Messages.EnterBaseNumber);
            var input = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(input))
                throw new ArgumentException(Messages.BaseNumberIsRequired);

            if (!int.TryParse(input, out var toBase))
                throw new ArgumentException(Messages.InvalidInputForBaseNumber, input);

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