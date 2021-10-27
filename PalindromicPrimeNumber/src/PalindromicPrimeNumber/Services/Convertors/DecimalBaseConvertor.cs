namespace PalindromicPrimeNumber.Services.Convertors
{
    public class DecimalBaseConvertor : IBaseConvertor
    {
        public int Base { get; } = 10;

        public string Convert(int numberInDecimalBase)
        {
            return numberInDecimalBase.ToString();
        }
    }
}