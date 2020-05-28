using System;

namespace AccuPay.Infrastructure.Services.Excel
{
    public class ExcelException : Exception
    {
        public ExcelException(string message) : base(message)
        {
        }
    }
}