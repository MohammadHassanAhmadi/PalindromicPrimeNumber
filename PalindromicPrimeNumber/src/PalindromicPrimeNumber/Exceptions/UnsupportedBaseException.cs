using System;

namespace PalindromicPrimeNumber.Exceptions
{
    public class UnsupportedBaseException : ApplicationException
    {
        public UnsupportedBaseException()
        {
        }

        public UnsupportedBaseException(string message)
            : base(message)
        {
        }

        public UnsupportedBaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public UnsupportedBaseException(string message, params object[] messageArgs)
            : this(string.Format(message, messageArgs))
        {
        }
    }
}