using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services.Policies;
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

        public ShiftImportModel(Employee employee)
        {
            AssignEmployee(employee);
        }

        private void AssignEmployee(Employee employee)
        {
            EmployeeId = employee?.RowID;
            EmployeeNo = employee?.EmployeeNo;
            FullName = employee?.FullNameLastNameFirst;
        }

        // used in DataGridView as well as TimeToDisplay and BreakFromDisplay
        public DateTime? TimeFromDisplay => TimeUtility.ToDateTime(StartTime);

        public DateTime? TimeToDisplay => TimeUtility.ToDateTime(EndTime);

        public DateTime? BreakFromDisplay => TimeUtility.ToDateTime(BreakTime);

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

                if (_isShiftBasedAutoOvertimePolicyEnabled)
                    if (!_shiftBasedAutoOvertimePolicy.IsValidDefaultShiftPeriod(TimeFromDisplay, TimeToDisplay, BreakLength))
                    {
                        var endTime = _shiftBasedAutoOvertimePolicy.GetDefaultShiftPeriodEndTime(TimeFromDisplay, BreakLength);
                        if (endTime.HasValue)
                        {
                            var minOvertimeMinutes = Convert.ToDouble(_shiftBasedAutoOvertimePolicy.Minimum);
                            var expectedEndTime = endTime.Value;
                            var reason = $"End Time should be {expectedEndTime.ToShortTimeString()} or greater";
                            if (minOvertimeMinutes > 0)
                            {
                                var expectedOvertime = endTime.Value.AddMinutes(minOvertimeMinutes);
                                reason = $"End Time should be {expectedEndTime.ToShortTimeString()}, or {expectedOvertime.ToShortTimeString()} or greater";
                            }
                            reasons.Add(reason);
                        }
                    }

                var message = string.Join("; ", reasons.ToArray());

                if (!string.IsNullOrWhiteSpace(message))
                    message += ".";

                return message;
            }
        }

        internal void SetShiftBasedAutoOvertimePolict(ShiftBasedAutomaticOvertimePolicy shiftBasedAutoOvertimePolicy)
        {
            _shiftBasedAutoOvertimePolicy = shiftBasedAutoOvertimePolicy;
            _isShiftBasedAutoOvertimePolicyEnabled = _shiftBasedAutoOvertimePolicy != null ? _shiftBasedAutoOvertimePolicy.Enabled : false;
        }
    }
}