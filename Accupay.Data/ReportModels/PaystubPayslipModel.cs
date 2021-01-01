using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccuPay.Data.ReportModels
{
    public class PaystubPayslipModel : IPaystubPayslipModel
    {
        private const string MoneyFormat = "#,##0.00";

        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public decimal RegularPay { get; set; }

        public decimal BasicHours { get; set; }

        public decimal BasicPay { get; private set; }

        public decimal Allowance { get; set; }
        public decimal Ecola { get; set; }
        public decimal AbsentHours { get; set; }

        private decimal _absentAmount;

        public decimal AbsentAmount
        {
            set => _absentAmount = Negative(value);
            get => _absentAmount;
        }

        public decimal LateAndUndertimeHours { get; set; }

        private decimal _lateAndUndertimeAmount;

        public decimal LateAndUndertimeAmount
        {
            set => _lateAndUndertimeAmount = Negative(value);
            get => _lateAndUndertimeAmount;
        }

        public decimal GrossPay { get; set; }

        private decimal _sssAmount;

        public decimal SSSAmount
        {
            set => _sssAmount = Negative(value);
            get => _sssAmount;
        }

        private decimal _philHealthAmount;

        public decimal PhilHealthAmount
        {
            set => _philHealthAmount = Negative(value);
            get => _philHealthAmount;
        }

        private decimal _pagibigAmount;

        public decimal PagibigAmount
        {
            set => _pagibigAmount = Negative(value);
            get => _pagibigAmount;
        }

        private decimal _taxWithheldAmount;

        public decimal TaxWithheldAmount
        {
            set => _taxWithheldAmount = Negative(value);
            get => _taxWithheldAmount;
        }

        public decimal LeaveHours { get; set; }
        public decimal LeavePay { get; set; }

        public decimal NetPay { get; set; }

        public Employee Employee { get; }

        public PaystubPayslipModel(Employee employee) => this.Employee = employee;

        private decimal Negative(decimal num)
        {
            if (num > 0)
                return num * -1;

            return num;
        }

        public decimal ComputeBasicPay(decimal salary, decimal workHours)
        {
            if (Employee.IsMonthly || Employee.IsFixed)
            {
                if (Employee.PayFrequencyID == (int)PayFrequencyType.Monthly)
                    return salary;
                else if (Employee.PayFrequencyID == (int)PayFrequencyType.SemiMonthly)
                    return salary / PayrollTools.SemiMonthlyPayPeriodsPerMonth;
                else
                    throw new Exception("GetBasicPay is implemented on monthly and semimonthly only");
            }
            else if (Employee.IsDaily)
                return workHours * (salary / PayrollTools.WorkHoursPerDay);

            return 0;
        }

        public PaystubPayslipModel CreateSummaries(decimal salary, decimal workHours)
        {
            BasicPay = ComputeBasicPay(salary, workHours);

            return this.CreateOvertimeSummaryColumns().
                        CreateLoanSummaryColumns().
                        CreateAdjustmentSummaryColumns();
        }

        public decimal TotalDeductions => Negative(SSSAmount +
                                                    PhilHealthAmount +
                                                    PagibigAmount +
                                                    TaxWithheldAmount +
                                                    TotalLoans);

        public List<Loan> Loans { get; set; }
        public List<Adjustment> Adjustments { get; set; }

        public decimal TotalLoans => Negative(Loans.Sum(l => l.Amount));

        public decimal TotalAdjustments => Adjustments.Sum(l => l.Amount);

        public string LoanNamesSummary { get; private set; }

        public string LoanAmountsSummary { get; private set; }

        public string LoanBalancesSummary { get; private set; }

        public string AdjustmentNamesSummary { get; private set; }

        public string AdjustmentAmountsSummary { get; private set; }

        public PaystubPayslipModel CreateLoanSummaryColumns()
        {
            LoanNamesSummary = "";
            LoanAmountsSummary = "";
            LoanBalancesSummary = "";

            StringBuilder loanNamesSummaryBuilder = new StringBuilder();
            StringBuilder loanAmountsSummaryBuilder = new StringBuilder();
            StringBuilder loanBalancesSummaryBuilder = new StringBuilder();

            var rightSideSummaryMaxCharacters = 15;

            foreach (var loan in Loans)
            {
                if (loan.Amount != 0)
                {
                    loanNamesSummaryBuilder.AppendLine(loan.Name.Ellipsis(rightSideSummaryMaxCharacters));
                    loanAmountsSummaryBuilder.AppendLine(loan.Amount.ToString(MoneyFormat));
                    loanBalancesSummaryBuilder.AppendLine(loan.Balance.ToString(MoneyFormat));
                }
            }

            LoanNamesSummary = loanNamesSummaryBuilder.ToString();
            LoanAmountsSummary = loanAmountsSummaryBuilder.ToString();
            LoanBalancesSummary = loanBalancesSummaryBuilder.ToString();

            return this;
        }

        public PaystubPayslipModel CreateAdjustmentSummaryColumns()
        {
            AdjustmentNamesSummary = "";
            AdjustmentAmountsSummary = "";

            StringBuilder adjustmentNamesSummaryBuilder = new StringBuilder();
            StringBuilder adjustmentAmountsSummaryBuilder = new StringBuilder();

            var rightSideSummaryMaxCharacters = 25;

            foreach (var adjustment in Adjustments)
            {
                if (adjustment.Amount != 0)
                {
                    adjustmentNamesSummaryBuilder.AppendLine(adjustment.Name.Ellipsis(rightSideSummaryMaxCharacters));
                    adjustmentAmountsSummaryBuilder.AppendLine(adjustment.Amount.ToString(MoneyFormat));
                }
            }

            AdjustmentNamesSummary = adjustmentNamesSummaryBuilder.ToString();
            AdjustmentAmountsSummary = adjustmentAmountsSummaryBuilder.ToString();

            return this;
        }

        public decimal TotalOvertimeHours { get; private set; }

        public decimal TotalOvertimePay { get; private set; }

        public string OvertimeNamesSummary { get; private set; }

        public string OvertimeHoursSummary { get; private set; }

        public string OvertimeAmountsSummary { get; private set; }

        public PaystubPayslipModel CreateOvertimeSummaryColumns()
        {
            TotalOvertimeHours = 0;
            TotalOvertimePay = 0;

            StringBuilder overtimeNamesSummaryBuilder = new StringBuilder();
            StringBuilder overtimeHoursSummaryBuilder = new StringBuilder();
            StringBuilder overtimeAmountsSummaryBuilder = new StringBuilder();

            if (OvertimePay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Overtime");
                overtimeHoursSummaryBuilder.AppendLine(OvertimeHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(OvertimePay.ToString(MoneyFormat));
                TotalOvertimeHours += OvertimeHours;
                TotalOvertimePay += OvertimePay;
            }
            if (NightDiffPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Night Diff");
                overtimeHoursSummaryBuilder.AppendLine(NightDiffHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(NightDiffPay.ToString(MoneyFormat));
                TotalOvertimeHours += NightDiffHours;
                TotalOvertimePay += NightDiffPay;
            }
            if (NightDiffOvertimePay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Night Diff OT");
                overtimeHoursSummaryBuilder.AppendLine(NightDiffOvertimeHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(NightDiffOvertimePay.ToString(MoneyFormat));
                TotalOvertimeHours += NightDiffOvertimeHours;
                TotalOvertimePay += NightDiffOvertimePay;
            }
            if (RestDayPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Rest Day");
                overtimeHoursSummaryBuilder.AppendLine(RestDayHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RestDayPay.ToString(MoneyFormat));
                TotalOvertimeHours += RestDayHours;
                TotalOvertimePay += RestDayPay;
            }
            if (RestDayOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Rest Day OT");
                overtimeHoursSummaryBuilder.AppendLine(RestDayOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RestDayOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += RestDayOTHours;
                TotalOvertimePay += RestDayOTPay;
            }
            if (SpecialHolidayPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Special Holiday");
                overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayPay.ToString(MoneyFormat));
                TotalOvertimeHours += SpecialHolidayHours;
                TotalOvertimePay += SpecialHolidayPay;
            }
            if (SpecialHolidayOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Special Holiday OT");
                overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += SpecialHolidayOTHours;
                TotalOvertimePay += SpecialHolidayOTPay;
            }
            if (RegularHolidayPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Regular Holiday");
                overtimeHoursSummaryBuilder.AppendLine(RegularHolidayHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayPay.ToString(MoneyFormat));
                TotalOvertimeHours += RegularHolidayHours;
                TotalOvertimePay += RegularHolidayPay;
            }
            if (RegularHolidayOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Regular Holiday OT");
                overtimeHoursSummaryBuilder.AppendLine(RegularHolidayOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += RegularHolidayOTHours;
                TotalOvertimePay += RegularHolidayOTPay;
            }
            if (RestDayNightDiffPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Rest Day ND");
                overtimeHoursSummaryBuilder.AppendLine(RestDayNightDiffHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RestDayNightDiffPay.ToString(MoneyFormat));
                TotalOvertimeHours += RestDayNightDiffHours;
                TotalOvertimePay += RestDayNightDiffPay;
            }
            if (RestDayNightDiffOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("Rest Day ND OT");
                overtimeHoursSummaryBuilder.AppendLine(RestDayNightDiffOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RestDayNightDiffOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += RestDayNightDiffOTHours;
                TotalOvertimePay += RestDayNightDiffOTPay;
            }
            if (SpecialHolidayNightDiffPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("S. Holi ND");
                overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayNightDiffHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayNightDiffPay.ToString(MoneyFormat));
                TotalOvertimeHours += SpecialHolidayNightDiffHours;
                TotalOvertimePay += SpecialHolidayNightDiffPay;
            }
            if (SpecialHolidayNightDiffOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("S. Holi ND OT");
                overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayNightDiffOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayNightDiffOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += SpecialHolidayNightDiffOTHours;
                TotalOvertimePay += SpecialHolidayNightDiffOTPay;
            }
            if (SpecialHolidayRestDayPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("S. Holi RD");
                overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayPay.ToString(MoneyFormat));
                TotalOvertimeHours += SpecialHolidayRestDayHours;
                TotalOvertimePay += SpecialHolidayRestDayPay;
            }
            if (SpecialHolidayRestDayOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("S. Holi RD OT");
                overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += SpecialHolidayRestDayOTHours;
                TotalOvertimePay += SpecialHolidayRestDayOTPay;
            }
            if (SpecialHolidayRestDayNightDiffPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("S. Holi RD ND");
                overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffPay.ToString(MoneyFormat));
                TotalOvertimeHours += SpecialHolidayRestDayNightDiffHours;
                TotalOvertimePay += SpecialHolidayRestDayNightDiffPay;
            }
            if (SpecialHolidayRestDayNightDiffOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("S. Holi RD ND OT");
                overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += SpecialHolidayRestDayNightDiffOTHours;
                TotalOvertimePay += SpecialHolidayRestDayNightDiffOTPay;
            }
            if (RegularHolidayNightDiffPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("R. Holi. ND");
                overtimeHoursSummaryBuilder.AppendLine(RegularHolidayNightDiffHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayNightDiffPay.ToString(MoneyFormat));
                TotalOvertimeHours += RegularHolidayNightDiffHours;
                TotalOvertimePay += RegularHolidayNightDiffPay;
            }
            if (RegularHolidayNightDiffOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("R. Holi. ND OT");
                overtimeHoursSummaryBuilder.AppendLine(RegularHolidayNightDiffOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayNightDiffOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += RegularHolidayNightDiffOTHours;
                TotalOvertimePay += RegularHolidayNightDiffOTPay;
            }
            if (RegularHolidayRestDayPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD");
                overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayPay.ToString(MoneyFormat));
                TotalOvertimeHours += RegularHolidayRestDayHours;
                TotalOvertimePay += RegularHolidayRestDayPay;
            }
            if (RegularHolidayRestDayOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD OT");
                overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += RegularHolidayRestDayOTHours;
                TotalOvertimePay += RegularHolidayRestDayOTPay;
            }
            if (RegularHolidayRestDayNightDiffPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD ND");
                overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffPay.ToString(MoneyFormat));
                TotalOvertimeHours += RegularHolidayRestDayNightDiffHours;
                TotalOvertimePay += RegularHolidayRestDayNightDiffPay;
            }
            if (RegularHolidayRestDayNightDiffOTPay != 0)
            {
                overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD ND OT");
                overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffOTHours.ToString(MoneyFormat));
                overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffOTPay.ToString(MoneyFormat));
                TotalOvertimeHours += RegularHolidayRestDayNightDiffOTHours;
                TotalOvertimePay += RegularHolidayRestDayNightDiffOTPay;
            }

            OvertimeNamesSummary = overtimeNamesSummaryBuilder.ToString();
            OvertimeHoursSummary = overtimeHoursSummaryBuilder.ToString();
            OvertimeAmountsSummary = overtimeAmountsSummaryBuilder.ToString();

            return this;
        }

        public decimal OvertimeHours { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal NightDiffHours { get; set; }
        public decimal NightDiffPay { get; set; }
        public decimal NightDiffOvertimeHours { get; set; }
        public decimal NightDiffOvertimePay { get; set; }
        public decimal RestDayHours { get; set; }
        public decimal RestDayPay { get; set; }
        public decimal RestDayOTHours { get; set; }
        public decimal RestDayOTPay { get; set; }
        public decimal SpecialHolidayHours { get; set; }
        public decimal SpecialHolidayPay { get; set; }
        public decimal SpecialHolidayOTHours { get; set; }
        public decimal SpecialHolidayOTPay { get; set; }
        public decimal RegularHolidayHours { get; set; }
        public decimal RegularHolidayPay { get; set; }
        public decimal RegularHolidayOTHours { get; set; }
        public decimal RegularHolidayOTPay { get; set; }
        public decimal RestDayNightDiffHours { get; set; }
        public decimal RestDayNightDiffPay { get; set; }
        public decimal RestDayNightDiffOTHours { get; set; }
        public decimal RestDayNightDiffOTPay { get; set; }
        public decimal SpecialHolidayNightDiffHours { get; set; }
        public decimal SpecialHolidayNightDiffPay { get; set; }
        public decimal SpecialHolidayNightDiffOTHours { get; set; }
        public decimal SpecialHolidayNightDiffOTPay { get; set; }
        public decimal SpecialHolidayRestDayHours { get; set; }
        public decimal SpecialHolidayRestDayPay { get; set; }
        public decimal SpecialHolidayRestDayOTHours { get; set; }
        public decimal SpecialHolidayRestDayOTPay { get; set; }
        public decimal SpecialHolidayRestDayNightDiffHours { get; set; }
        public decimal SpecialHolidayRestDayNightDiffPay { get; set; }
        public decimal SpecialHolidayRestDayNightDiffOTHours { get; set; }
        public decimal SpecialHolidayRestDayNightDiffOTPay { get; set; }
        public decimal RegularHolidayNightDiffHours { get; set; }
        public decimal RegularHolidayNightDiffPay { get; set; }
        public decimal RegularHolidayNightDiffOTHours { get; set; }
        public decimal RegularHolidayNightDiffOTPay { get; set; }
        public decimal RegularHolidayRestDayHours { get; set; }
        public decimal RegularHolidayRestDayPay { get; set; }
        public decimal RegularHolidayRestDayOTHours { get; set; }
        public decimal RegularHolidayRestDayOTPay { get; set; }
        public decimal RegularHolidayRestDayNightDiffHours { get; set; }
        public decimal RegularHolidayRestDayNightDiffPay { get; set; }
        public decimal RegularHolidayRestDayNightDiffOTHours { get; set; }
        public decimal RegularHolidayRestDayNightDiffOTPay { get; set; }

        public class Overtime
        {
            public string Name { get; set; }
            public decimal Hours { get; set; }
            public decimal Amount { get; set; }
        }

        public class Loan
        {
            public string Name { get; set; }

            public decimal Amount { get; set; }

            public decimal Balance { get; set; }

            public Loan(LoanTransaction loan) : this(loan.Loan?.LoanType?.PartNo, loan.DeductionAmount, loan.TotalBalance)
            {
            }

            public Loan(string name, decimal amount, decimal balance)
            {
                this.Name = name;

                this.Amount = amount;

                this.Balance = balance;
            }
        }

        public class Adjustment
        {
            public string Name { get; set; }

            public decimal Amount { get; set; }

            public Adjustment(IAdjustment adjustment) : this(adjustment.Product?.PartNo, adjustment.Amount)
            {
            }

            public Adjustment(string name, decimal amount)
            {
                this.Name = name;

                this.Amount = amount;
            }
        }
    }
}