using System;

namespace AccuPay.Core.Exceptions
{
    public class LeaveResetException: Exception
    {
        public LeaveResetException(string message) : base(message)
        {
        }
    }
}
