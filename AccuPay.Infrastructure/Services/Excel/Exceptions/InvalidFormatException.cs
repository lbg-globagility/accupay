using System;

namespace AccuPay.Infrastracture.Services.Excel
{
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException(string message = "Only .xlsx files are supported.") : base(message)
        {
        }
    }
}