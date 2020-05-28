using System;

namespace AccuPay.Infrastracture.Services.Excel
{
    public class WorkSheetRowParseValueException : Exception
    {
        public int RowNumber { get; }

        public string ColumnName { get; }

        public new string Message { get; }

        public new Exception InnerException { get; }

        public WorkSheetRowParseValueException(Exception innerException, string columnName, int rowNumber)
        {
            // We needed to overload the base properties Message and InnerException since we can't call the base constructor.
            // We can't call the base constructor because we need to create the message first before calling the base constructor and that is prohibited.

            ColumnName = columnName;
            RowNumber = rowNumber;

            Message = $"Cannot parse value of column '{columnName}' on row {rowNumber}";
            InnerException = innerException;
        }
    }
}