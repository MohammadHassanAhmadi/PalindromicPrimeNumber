//namespace PalindromicPrimeNumber.Services.Convertors
//{
//    public class ThreeBaseConvertor : IBaseConvertor
//    {
//        public int Base { get; } = 3;

//        public string Convert(int numberInDecimalBase)
//        {
//            var temp = numberInDecimalBase;
//            var result = string.Empty;
//            do
//            {
//                result += temp % Base;
//                temp /= Base;
//            } while (temp > 0);

//            return result;
//        }
//    }
//}