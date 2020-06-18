using System;

namespace AccuPay.Utilities.Attributes
{
    public class ColumnNameAttribute : Attribute
    {
        public string Value { get; set; }

        public ColumnNameAttribute(string value) => Value = value;
    }
}