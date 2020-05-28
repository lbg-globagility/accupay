using System;

namespace AccuPay.Infrastracture.Services.Excel
{
    public class WorkSheetNotFoundException : Exception
    {
        public WorkSheetNotFoundException(string message) : base(message)
        {
        }
    }
}