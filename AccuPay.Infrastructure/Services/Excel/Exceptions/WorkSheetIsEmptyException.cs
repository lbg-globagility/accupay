using System;

namespace AccuPay.Infrastracture.Services.Excel
{
    public class WorkSheetIsEmptyException : Exception
    {
        public WorkSheetIsEmptyException(string message = "WorkSheet is empty.") : base(message)
        {
        }
    }
}