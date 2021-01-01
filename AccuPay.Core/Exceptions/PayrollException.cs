using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Core.Exceptions
{
    public class PayrollException : Exception
    {
        public PayrollException(string message) : base(message)
        {
        }
    }
}