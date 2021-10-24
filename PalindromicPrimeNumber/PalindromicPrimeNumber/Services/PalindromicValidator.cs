using System.Linq;

namespace PalindromicPrimeNumber.Services
{
    public class PalindromicValidator : IPalindromicValidator
    {
        public bool Validate(string value)
        {
            if (value.Length == 1) return true;

            return value.SequenceEqual(value.Reverse());
        }
    }
}