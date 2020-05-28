using AccuPay.Utilities;
using AccuPay.Utilities.Extensions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AccuPay.Infrastracture.Services.Excel
{
    public class ExcelParser<T> where T : IExcelRowRecord, new()
    {
        private readonly string _worksheetName;

        public ExcelParser()
        {
        }

        public ExcelParser(string worksheetName)
        {
            _worksheetName = worksheetName;
        }

        public IList<T> Read(string filePath)
        {
            // we can probably support other file format unless it's .xls
            if (Path.GetExtension(filePath) != ".xlsx")
                throw new InvalidFormatException();

            var stream = GetFileContents(filePath);

            return Read(stream);
        }

        /// <summary>
        /// Parses a stream into a list of models. This only supports .xlsx files.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public IList<T> Read(Stream fileStream)
        {
            List<T> records = new List<T>();

            using (var excel = new ExcelPackage(fileStream))
            {
                ExcelWorksheet worksheet;

                if (_worksheetName == null)
                    worksheet = excel.Workbook.Worksheets[1];
                else
                    worksheet = excel.Workbook.Worksheets[_worksheetName];

                if (worksheet == null)
                    throw new WorkSheetNotFoundException($"Cannot find the worksheet {(string.IsNullOrWhiteSpace(_worksheetName) ? "" : $"`{ _worksheetName }`")}.");

                var tprops = typeof(T).GetProperties().ToList();

                var groups = worksheet.Cells.GroupBy(g => g.Start.Row).ToList();

                if (groups.Count < 1)
                    throw new WorkSheetIsEmptyException();

                var columns = groups.First().Select((col, index) => new Column(col, index)).ToList();

                var rows = GetRows(worksheet, columns);

                var rowIndex = 1; // starts at 1 because of the headers
                foreach (var row in rows)
                {
                    rowIndex += 1;

                    var newRow = ParseRow(row, columns, tprops, rowIndex);
                    if (newRow == null)
                        continue;

                    newRow.LineNumber = rowIndex;
                    records.Add(newRow);
                }
            }

            return records;
        }

        private T ParseRow(List<object> row, IList<Column> colNames, List<PropertyInfo> tprops, int lineNumber)
        {
            var newRecord = new T();

            bool rowIsBlank = true;

            foreach (var column in colNames)
            {
                if (column.Index >= row.Count)
                    continue;

                var originalValue = row[column.Index];

                if (string.IsNullOrWhiteSpace(ObjectUtils.ToStringOrNull(originalValue)) == false)
                    rowIsBlank = false;

                PropertyInfo prop;

                // Check by Property Attribute Name
                prop = tprops.FirstOrDefault(t =>
                {
                    if (Attribute.IsDefined(t, typeof(ColumnNameAttribute)) == false)
                        return false;

                    var attr = (ColumnNameAttribute[])t.GetCustomAttributes(typeof(ColumnNameAttribute), false);

                    return attr[0].Value.ToPascal() == column.Name.ToPascal();
                });

                if (prop == null)
                {
                    // Check by Property Name
                    prop = tprops.FirstOrDefault(t =>
                    {
                        return t.Name.ToPascal() == column.Name.ToPascal();
                    });
                }

                if (prop != null)
                {
                    if (Attribute.IsDefined(prop, typeof(IgnoreAttribute)))
                        continue;

                    try
                    {
                        ParseValue(newRecord, prop, originalValue);
                    }
                    catch (Exception ex)
                    {
                        throw new WorkSheetRowParseValueException(ex, column.Name, lineNumber);
                    }
                }
            }

            return rowIsBlank ? default : newRecord;
        }

        private List<List<object>> GetRows(ExcelWorksheet worksheet, ICollection<Column> columns)
        {
            var firstRow = worksheet.Dimension.Start.Row + 1;
            var endRow = worksheet.Dimension.End.Row;

            var rows = new List<List<object>>();

            for (var i = firstRow; i <= endRow; i++)
            {
                var row = new List<object>();

                foreach (var column in columns)
                    row.Add(worksheet.Cells[$"{column.Letter}{i}"].Value);

                rows.Add(row);
            }

            return rows;
        }

        private Stream GetFileContents(string filePath)
        {
            var contents = new MemoryStream();

            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fileStream.CopyTo(contents);
            }

            return contents;
        }

        private void ParseValue(T newRecord, PropertyInfo prop, object originalValue)
        {
            if (originalValue is double)
            {
                var value = (double)originalValue;

                if (prop.PropertyType == typeof(double))
                    prop.SetValue(newRecord, value);
                else if (prop.PropertyType == typeof(double?))
                    prop.SetValue(newRecord, ObjectUtils.ToNullableDouble(value));
                else if (prop.PropertyType == typeof(decimal))
                    prop.SetValue(newRecord, (decimal)value);
                else if (prop.PropertyType == typeof(decimal?))
                    prop.SetValue(newRecord, ObjectUtils.ToNullableDecimal(value));
                else if (prop.PropertyType == typeof(int))
                    prop.SetValue(newRecord, (int)value);
                else if (prop.PropertyType == typeof(int?))
                    prop.SetValue(newRecord, ObjectUtils.ToNullableInteger(value));
                else if (prop.PropertyType == typeof(DateTime) | prop.PropertyType == typeof(DateTime?))
                    prop.SetValue(newRecord, DateTime.FromOADate(value));
                else if (prop.PropertyType == typeof(string))
                    prop.SetValue(newRecord, value.ToString());
                else if (prop.PropertyType == typeof(TimeSpan) | prop.PropertyType == typeof(TimeSpan?))
                    prop.SetValue(newRecord, DateTime.FromOADate(value).TimeOfDay);
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                var dateInput = ObjectUtils.ToDateTime(originalValue);
                if (dateInput != DateTime.MinValue && DateTime.MinValue.ToString() != originalValue?.ToString())
                    prop.SetValue(newRecord, dateInput);
            }
            else if (prop.PropertyType == typeof(DateTime?))
                prop.SetValue(newRecord, ObjectUtils.ToNullableDateTime(originalValue));
            else if (prop.PropertyType == typeof(TimeSpan))
                prop.SetValue(newRecord, ObjectUtils.ToTimeSpan(originalValue));
            else if (prop.PropertyType == typeof(TimeSpan?))
                prop.SetValue(newRecord, ObjectUtils.ToNullableTimeSpan(originalValue));
            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                prop.SetValue(newRecord, ObjectUtils.ToBoolean(originalValue));
            else
                prop.SetValue(newRecord, originalValue);
        }

        private class Column
        {
            public string Letter { get; }

            public string Name { get; }

            public int Index { get; }

            public Column(ExcelRangeBase cell, int index)
            {
                this.Letter = GetLettersOnly(cell.Address);
                this.Name = cell.Value?.ToString();
                this.Index = index;
            }

            private string GetLettersOnly(string address)
            {
                return Regex.Replace(address, @"[\d-]", string.Empty);
            }
        }
    }
}