namespace AccuPay.Infrastructure.Services.Excel
{
    public class WorkSheetNotFoundException : ExcelException
    {
        public WorkSheetNotFoundException(string message) : base(message)
        {
        }
    }
}