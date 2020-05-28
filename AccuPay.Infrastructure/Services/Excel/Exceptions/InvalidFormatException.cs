namespace AccuPay.Infrastructure.Services.Excel
{
    public class InvalidFormatException : ExcelException
    {
        public InvalidFormatException(string message = "Only .xlsx files are supported.") : base(message)
        {
        }
    }
}