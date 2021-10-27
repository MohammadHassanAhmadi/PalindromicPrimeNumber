namespace PalindromicPrimeNumber.Services.Convertors
{
    public interface IBaseConvertor
    {
        int Base { get; }

        string Convert(int numberInDecimalBase);
    }
}