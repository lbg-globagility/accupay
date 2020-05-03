using System;

namespace AccuPay.Data.Exceptions
{
    public class AccuPayRepositoryException : Exception
    {
        public AccuPayRepositoryException(string message) : base(message)
        {
        }
    }
}