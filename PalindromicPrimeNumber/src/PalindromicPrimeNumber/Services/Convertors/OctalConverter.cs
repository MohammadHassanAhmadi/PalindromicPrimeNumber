namespace PalindromicPrimeNumber.Services.Convertors
{
    public class OctalConverter:IBaseConvertor
    {
        public int Base { get; } = 8;
        
        public string Convert(int numberInDecimalBase)
        {
            return System.Convert.ToString(numberInDecimalBase, 8);
        }
    }
}