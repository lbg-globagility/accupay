using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Utilities;

namespace AccuPay.Core.ReportModels
{
    public class LeaveLedgerReportModel : ILeaveLedgerReportModel
    {
        public string EmployeeNumber { get; set; }

        public string FullName { get; set; }

        public LeaveType LeaveType { get; set; }

        public LeaveLedgerReportModel(string employeeNumber,
                                        string fullName,
                                        LeaveType leaveType,
                                        decimal beginningBalance,
                                        decimal availedLeave)
        {
            this.EmployeeNumber = employeeNumber;
            this.FullName = fullName;
            this.LeaveType = leaveType;
            this.BeginningBalance = beginningBalance;
            this.AvailedLeave = availedLeave;
        }

        public string LeaveTypeDescription
        {
            get
            {
                switch (LeaveType)
                {
                    case LeaveType.Sick:
                        return "SL";

                    case LeaveType.Vacation:
                        return "VL";

                    default:
                        return "";
                }
            }
        }

        private decimal _beginningBalance;

        public decimal BeginningBalance
        {
            get => _beginningBalance;
            set => _beginningBalance = AccuMath.CommercialRound(value);
        }

        private decimal _availedLeave;

        public decimal AvailedLeave
        {
            get => _availedLeave;
            set => _availedLeave = AccuMath.CommercialRound(value);
        }

        public decimal BeginningBalanceInDays => BeginningBalance / PayrollTools.WorkHoursPerDay;

        public decimal AvailedLeaveInDays => AvailedLeave / PayrollTools.WorkHoursPerDay;

        public decimal EndingBalance => BeginningBalance - AvailedLeave;

        public decimal EndingBalanceInDays => EndingBalance / 8;
    }
}