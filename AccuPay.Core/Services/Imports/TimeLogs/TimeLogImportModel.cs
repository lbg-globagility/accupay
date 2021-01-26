using AccuPay.Core.Entities;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services.Imports
{
    public class TimeLogImportModel
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
        public Shift Shift { get; set; }
        public DateTime ShiftTimeInBounds { get; set; }
        public DateTime ShiftTimeOutBounds { get; set; }
        public string WarningMessage { get; set; }

        public TimeLogImportModel()
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
                if (Shift != null)
                    return $"{Shift.StartTime.ToStringFormat("hh:mm tt")} - {Shift.EndTime.ToStringFormat("hh:mm tt")}";

                return "-";
            }
        }

        public static List<IGrouping<string, TimeLogImportModel>> GroupByEmployee(IList<TimeLogImportModel> logs)
        {
            return logs.GroupBy(l => l.EmployeeNumber).ToList();
        }
    }
}
