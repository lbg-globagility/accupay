using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace AccuPay.Core.Services
{
    public class TimeInTimeOutParser
    {
        private const string DateDashFormat = "M-d-yyyy";
        private const string DateSlashFormat = "M/d/yyyy";

        private const string DateValidationFormat = @"^(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{1,4})$";

        public Collection<FixedFormatTimeEntry> Parse(string filename)
        {
            var timeEntries = new Collection<FixedFormatTimeEntry>();

            using (StreamReader reader = new StreamReader(filename))
            {
                string currentLine;

                do
                {
                    currentLine = reader.ReadLine();
                    ParseLine(currentLine, timeEntries);
                }
                while (currentLine != null);
            }

            return timeEntries;
        }

        public Collection<ConventionalTimeLogs> ParseConventionalTimeLogs(string filename)
        {
            var timeEntries = new Collection<ConventionalTimeLogs>();

            using (StreamReader reader = new StreamReader(filename))
            {
                string currentLine = string.Empty;

                while (reader.Peek() >= 0)
                {
                    currentLine = reader.ReadLine();

                    if (currentLine.Length > 0)
                    {
                        var values = Regex.Split(currentLine, @"\t");

                        timeEntries.Add(new ConventionalTimeLogs(values[0], values[1]));
                    }
                }
            }

            return timeEntries;
        }

        private void ParseLine(string line, Collection<FixedFormatTimeEntry> timeEntries)
        {
            try
            {
                if (string.IsNullOrEmpty(line))
                    return;

                // Collapse and trim the whitespaces in the line
                line = Regex.Replace(line, @"\s+", " ")?.Trim();

                var parts = Regex.Split(line, @"\s+");

                if (parts.Length < 3)
                    return;

                var employeeNo = parts[0]?.Trim();

                var logDate = parts[1]?.Trim();

                // Do a sanity check on the date, skip line if it's invalid
                if (!Regex.IsMatch(logDate, DateValidationFormat))
                    return;

                var dateFormat = string.Empty;
                if (logDate.Contains("-"))
                    dateFormat = DateDashFormat;
                else if (logDate.Contains("/"))
                    dateFormat = DateSlashFormat;

                var dateOccurred = DateTime.ParseExact(logDate, dateFormat, CultureInfo.InvariantCulture);

                var timeIn = parts[2].Trim();
                var timeOut = parts.Length > 3 ? parts[3].Trim() : null;

                if (string.IsNullOrEmpty(timeIn) && string.IsNullOrEmpty(timeOut))
                    return;

                timeEntries.Add(new FixedFormatTimeEntry(employeeNo, dateOccurred, timeIn, timeOut));
            }
            catch (Exception ex)
            {
                throw new ParseTimeLogException(line, ex);
            }
        }
    }

    internal class ParseTimeLogException : Exception
    {
        private static string FormatMessage(string line)
        {
            return $"Cannot parse the following line: {line}";
        }

        public ParseTimeLogException(string line) : base(FormatMessage(line))
        {
        }

        public ParseTimeLogException(string line, Exception ex) : base(FormatMessage(line), ex)
        {
        }
    }

    public class FixedFormatTimeEntry
    {
        public string EmployeeNo { get; set; }
        public DateTime DateOccurred { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }

        public FixedFormatTimeEntry(string employeeNo, DateTime dateOccurred, string timeIn, string timeOut)
        {
            this.EmployeeNo = employeeNo;
            this.DateOccurred = dateOccurred;
            this.TimeIn = timeIn;
            this.TimeOut = timeOut;
        }
    }

    public class ConventionalTimeLogs
    {
        public ConventionalTimeLogs(string employee_uniq_key, object date_andtime)
        {
            this.EmployeUniqueKey = employee_uniq_key;
            this.DateAndTime = date_andtime;
        }

        public string EmployeUniqueKey { get; set; } = string.Empty;

        public object DateAndTime { get; set; } = null;
    }
}