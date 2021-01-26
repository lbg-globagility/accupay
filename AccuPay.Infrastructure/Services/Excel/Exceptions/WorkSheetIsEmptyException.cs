namespace AccuPay.Infrastructure.Services.Excel
{
    public class WorkSheetIsEmptyException : ExcelException
    {
        public WorkSheetIsEmptyException(string message = "WorkSheet is empty.") : base(message)
        {
        }
    }
}