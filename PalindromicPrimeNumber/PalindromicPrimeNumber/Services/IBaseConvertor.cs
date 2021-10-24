namespace PalindromicPrimeNumber.Services
{
    public interface IBaseConvertor
    {
        int Base { get; }

        string Convert(int numberInDecimalBase);
    }
}