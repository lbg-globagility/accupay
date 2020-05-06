using AccuPay.Data.Entities;
using AccuPay.Utilities;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AccuPay.Data.Services
{
    public class TimeLogsReader
    {
        public const string FILE_NOT_FOUND_ERROR = "Import file not found. It may have been deleted or moved.";

        public ImportOutput Import(string filename)
        {
            ImportOutput output = new ImportOutput();

            var logs = new List<ImportTimeAttendanceLog>();

            int lineNumber = 0;
            string lineContent;

            try
            {
                using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var stream = new StreamReader(fileStream))
                    {
                        do
                        {
                            lineNumber += 1;
                            lineContent = stream.ReadLine();
                            var log = ParseLine(lineContent, lineNumber);
                            if (log == null)
                                continue;
                            else
                                logs.Add(log);
                        }
                        while (lineContent != null);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                output.IsImportSuccess = false;
                output.ErrorMessage = FILE_NOT_FOUND_ERROR;
            }
            catch (Exception)
            {
                output.IsImportSuccess = false;
                output.ErrorMessage = "An error occured. Please try again or contact Globagility.";
            }

            output.Logs = logs.Where(l => l.HasError == false).ToList();
            output.Errors = logs.Where(l => l.HasError == true).ToList();

            return output;
        }

        private ImportTimeAttendanceLog ParseLine(string lineContent, int lineNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(lineContent))
                    // not considered error if it's just an empty line
                    // but it won't still be added to TimeAttendanceLog
                    return null;

                var parts = Regex.Split(lineContent, @"\t");

                if (parts.Length < 2)
                    return (new ImportTimeAttendanceLog()
                    {
                        LineContent = lineContent,
                        LineNumber = lineNumber,
                        ErrorMessage = "Needs at least 2 items in one line separated by a tab."
                    });

                if (string.IsNullOrEmpty(parts[0]) && string.IsNullOrEmpty(parts[1]))
                    // happens when blank lines was still read
                    // usually because it received an input like tab
                    return null;

                var employeeNo = parts[0].Trim();

                if ((Regex.Split(parts[1], " ").Count() < 2))
                    return (new ImportTimeAttendanceLog()
                    {
                        LineContent = lineContent,
                        LineNumber = lineNumber,
                        ErrorMessage = "Date log must consist of Date (1 space) Time."
                    });

                var logDate = ObjectUtils.ToNullableDateTime(parts[1]);

                if (logDate == null)
                    return (new ImportTimeAttendanceLog()
                    {
                        LineContent = lineContent,
                        LineNumber = lineNumber,
                        ErrorMessage = "Second column must be a valid Date Time."
                    });

                return (new ImportTimeAttendanceLog()
                {
                    EmployeeNumber = employeeNo,
                    DateTime = Convert.ToDateTime(logDate),
                    LineContent = lineContent,
                    LineNumber = lineNumber
                });
            }
            catch (Exception)
            {
                return (new ImportTimeAttendanceLog()
                {
                    LineContent = lineContent,
                    LineNumber = lineNumber,
                    ErrorMessage = "Error reading the line. Please check the template."
                });
            }
        }

        public class ImportOutput
        {
            public IList<ImportTimeAttendanceLog> Logs { get; set; }
            public IList<ImportTimeAttendanceLog> Errors { get; set; }

            /// <summary>
            /// True if the file was read successfully. Even if there are errors parsing
            /// some lines, as long as the file was read, this is still True.
            /// Posible reason for this to become False is when it did not find the chosen
            /// file.
            /// </summary>
            public bool IsImportSuccess { get; set; }

            public string ErrorMessage { get; set; }

            public ImportOutput()
            {
                this.Logs = new List<ImportTimeAttendanceLog>();
                this.Errors = new List<ImportTimeAttendanceLog>();
                this.IsImportSuccess = true;

                this.ErrorMessage = null;
            }
        }
    }

    public class ImportTimeAttendanceLog
    {
        // - Contents and error message
        public string LineContent { get; set; }

        public int LineNumber { get; set; }
        public string ErrorMessage { get; set; }

        // - Extracted contents
        public string EmployeeNumber { get; set; }

        public DateTime DateTime { get; set; }

        // - analyzed data
        public bool? IsTimeIn { get; set; }

        public DateTime? LogDate { get; set; }
        public Employee Employee { get; set; }
        public ShiftSchedule ShiftSchedule { get; set; }
        public EmployeeDutySchedule EmployeeDutySchedule { get; set; }
        public DateTime ShiftTimeInBounds { get; set; }
        public DateTime ShiftTimeOutBounds { get; set; }
        public string WarningMessage { get; set; }

        public ImportTimeAttendanceLog()
        {
            this.IsTimeIn = null;
            this.LogDate = null;

            this.EmployeeNumber = null;
        }

        public override string ToString() => DateTime.ToString("MM/dd/yyyy hh:mm tt");

        public string Type => IsTimeIn == null ? "" : (IsTimeIn.Value ? "Time-in" : "Time-out");

        public string EmployeeFullName => Employee == null ? "" : Employee.FullNameWithMiddleInitial;

        public bool HasError => ErrorMessage != null;

        public bool HasWarning => WarningMessage != null;

        public string ShiftDescription
        {
            get
            {
                if (ShiftSchedule != null && ShiftSchedule.Shift != null)
                    return $"{ShiftSchedule.Shift.TimeFrom.ToStringFormat("hh:mm tt")} - {ShiftSchedule.Shift.TimeTo.ToStringFormat("hh:mm tt")}";
                else if (EmployeeDutySchedule != null)
                    return $"{EmployeeDutySchedule.StartTime.ToStringFormat("hh:mm tt")} - {EmployeeDutySchedule.EndTime.ToStringFormat("hh:mm tt")}";

                return "-";
            }
        }

        public static List<IGrouping<string, ImportTimeAttendanceLog>> GroupByEmployee(IList<ImportTimeAttendanceLog> logs)
        {
            return logs.GroupBy(l => l.EmployeeNumber).ToList();
        }
    }
}