using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services.Policies;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities;
using System;
using System.Collections.Generic;

namespace AccuPay.Data.Services.Imports
{
    public class ShiftImportModel : ShiftModel
    {
        private ShiftBasedAutomaticOvertimePolicy _shiftBasedAutoOvertimePolicy;
        private bool _isShiftBasedAutoOvertimePolicyEnabled;

        public string EmployeeNo { get; set; }
        public string FullName { get; set; }

        public ShiftImportModel(Employee employee, ShiftBasedAutomaticOvertimePolicy shiftBasedAutoOvertimePolicy)
        {
            _shiftBasedAutoOvertimePolicy = shiftBasedAutoOvertimePolicy;
            _isShiftBasedAutoOvertimePolicyEnabled = _shiftBasedAutoOvertimePolicy != null ? _shiftBasedAutoOvertimePolicy.Enabled : false;

            AssignEmployee(employee);
        }

        private void AssignEmployee(Employee employee)
        {
            EmployeeId = employee?.RowID;
            EmployeeNo = employee?.EmployeeNo;
            FullName = employee?.FullNameLastNameFirst;
        }

        // used in DataGridView as well as TimeToDisplay and BreakFromDisplay
        public DateTime? TimeFromDisplay => TimeUtility.ToDateTime(StartTime, Date);

        public DateTime? TimeToDisplay => TimeUtility.ToDateTime(EndTime, Date);

        public DateTime? BreakFromDisplay => TimeUtility.ToDateTime(BreakTime, Date);

        public bool IsValidToSave => string.IsNullOrWhiteSpace(Remarks);

        public string IsRestDayText => IsRestDay ? "Yes" : "No";

        public string Remarks
        {
            get
            {
                List<string> reasons = new List<string>();

                if (EmployeeId == null)
                    reasons.Add("Employee does not exists");

                if (Date < PayrollTools.SqlServerMinimumDate)
                    reasons.Add("Date cannot be earlier than January 1, 1753");

                if (StartTime == null)
                    reasons.Add("Start Time is required");

                if (EndTime == null)
                    reasons.Add("End Time is required");

                if (_isShiftBasedAutoOvertimePolicyEnabled) ValidateExpectedEndTimeOrMinimumOvertime(reasons);

                var message = string.Join("; ", reasons.ToArray());

                if (!string.IsNullOrWhiteSpace(message))
                    message += ".";

                return message;
            }
        }

        private void ValidateExpectedEndTimeOrMinimumOvertime(List<string> reasons)
        {
            if (!_shiftBasedAutoOvertimePolicy.IsValidDefaultShiftPeriod(TimeFromDisplay, EndTime, BreakLength))
            {
                var importedShiftTimePeriod = TimePeriod.FromTime(StartTime.Value, EndTime.Value, Date);
                var expectedEndTime = _shiftBasedAutoOvertimePolicy.GetExpectedEndTime(TimeFromDisplay, BreakLength).Value;
                var isLessThanExpectedEndTime = importedShiftTimePeriod.End < expectedEndTime;
                var reason = isLessThanExpectedEndTime ? $"End Time should be {expectedEndTime.ToShortTimeString()}" : string.Empty;

                if (!isLessThanExpectedEndTime)
                {
                    var minOvertimeMinutes = Convert.ToDouble(_shiftBasedAutoOvertimePolicy.Minimum);
                    var isEqualOrMoreThanMinimumOverTime = expectedEndTime.AddMinutes(minOvertimeMinutes) <= importedShiftTimePeriod.End;

                    if (!isEqualOrMoreThanMinimumOverTime)
                    {
                        var expectedOvertime = expectedEndTime.AddMinutes(minOvertimeMinutes);
                        reason = $"End Time should be {expectedOvertime.ToShortTimeString()} or greater";
                    }
                }
                reasons.Add(reason);
            }
        }
    }
}