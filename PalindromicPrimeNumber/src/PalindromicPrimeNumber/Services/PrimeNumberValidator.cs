namespace PalindromicPrimeNumber.Services
{
    public class PrimeNumberValidator : IPrimeNumberValidator
    {
        public bool Validate(int number)
        {
            if (number == 1) return false;

            for (var i = 2; i <= number / 2; i++)
                if (number % i == 0)
                    return false;

            return true;
        }
    }
}