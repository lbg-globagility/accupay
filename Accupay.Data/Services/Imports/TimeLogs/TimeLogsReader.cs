using AccuPay.Data.Services.Imports;
using AccuPay.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AccuPay.Data.Services
{
    public class TimeLogsReader
    {
        public const string FileNotFoundError = "Import file not found. It may have been deleted or moved.";

        public const string PreferredExtension = ".txt";

        public ImportOutput Read(string filename)
        {
            var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            return Read(fileStream);
        }

        public ImportOutput Read(FileStream fileStream)
        {
            ImportOutput output = new ImportOutput();

            var logs = new List<TimeLogImportModel>();

            int lineNumber = 0;
            string lineContent;

            try
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
            catch (FileNotFoundException)
            {
                output.IsImportSuccess = false;
                output.ErrorMessage = FileNotFoundError;
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

        private TimeLogImportModel ParseLine(string lineContent, int lineNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(lineContent))
                    // not considered error if it's just an empty line
                    // but it won't still be added to TimeAttendanceLog
                    return null;

                var parts = Regex.Split(lineContent, @"\t");

                if (parts.Length < 2)
                    return (new TimeLogImportModel()
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
                    return (new TimeLogImportModel()
                    {
                        LineContent = lineContent,
                        LineNumber = lineNumber,
                        ErrorMessage = "Date log must consist of Date (1 space) Time."
                    });

                var logDate = ObjectUtils.ToNullableDateTime(parts[1]);

                if (logDate == null)
                    return (new TimeLogImportModel()
                    {
                        LineContent = lineContent,
                        LineNumber = lineNumber,
                        ErrorMessage = "Second column must be a valid Date Time."
                    });

                return (new TimeLogImportModel()
                {
                    EmployeeNumber = employeeNo,
                    DateTime = Convert.ToDateTime(logDate),
                    LineContent = lineContent,
                    LineNumber = lineNumber
                });
            }
            catch (Exception)
            {
                return (new TimeLogImportModel()
                {
                    LineContent = lineContent,
                    LineNumber = lineNumber,
                    ErrorMessage = "Error reading the line. Please check the template."
                });
            }
        }

        public class ImportOutput
        {
            public IList<TimeLogImportModel> Logs { get; set; }
            public IList<TimeLogImportModel> Errors { get; set; }

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
                this.Logs = new List<TimeLogImportModel>();
                this.Errors = new List<TimeLogImportModel>();
                this.IsImportSuccess = true;

                this.ErrorMessage = null;
            }
        }
    }
}