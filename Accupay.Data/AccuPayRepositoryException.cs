using System;

namespace AccuPay.Data
{
    public class AccuPayRepositoryException : Exception
    {
        public AccuPayRepositoryException(string message) : base(message)
        {
        }
    }
}