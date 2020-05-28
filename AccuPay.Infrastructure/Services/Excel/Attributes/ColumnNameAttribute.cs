using System;

namespace AccuPay.Infrastracture.Services.Excel
{
    public class ColumnNameAttribute : Attribute
    {
        public string Value { get; set; }

        public ColumnNameAttribute(string value) => Value = value;
    }
}