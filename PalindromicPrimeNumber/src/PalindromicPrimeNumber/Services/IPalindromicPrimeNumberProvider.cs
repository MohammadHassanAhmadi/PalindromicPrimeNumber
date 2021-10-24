using System.Collections.Generic;

namespace PalindromicPrimeNumber.Services
{
    public interface IPalindromicPrimeNumberProvider
    {
        IEnumerable<string> GetPalindromicPrimeNumbers(int max, int inBase);
    }
}