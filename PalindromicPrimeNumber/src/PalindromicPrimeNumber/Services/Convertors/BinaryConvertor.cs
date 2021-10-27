namespace PalindromicPrimeNumber.Services.Convertors
{
    public class BinaryConvertor : IBaseConvertor
    {
        public int Base { get; } = 2;

        public string Convert(int numberInDecimalBase)
        {
            return System.Convert.ToString(numberInDecimalBase, 2);
        }
    }

}