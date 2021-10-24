using System.Linq;

namespace PalindromicPrimeNumber.Services
{
    public class PalindromeValidator : IPalindromeValidator
    {
        public bool Validate(string value)
        {
            if (value.Length == 1) return true;

            return value.SequenceEqual(value.Reverse());
        }
    }
}