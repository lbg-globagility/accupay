using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class CostCenterPaystubModel
    {
        public static CostCenterPaystubModel Create(
            Employee employee,
            Salary salary,
            Paystub currentPaystub,
            List<Paystub> monthlyPaystubs,
            bool isActual,
            MonthlyDeduction monthlyDeduction,
            LoanTransaction hmoLoan,
            List<Allowance> dailyAllowances,
            ListOfValueCollection settings,
            CalendarCollection calendarCollection,
            int userId,
            PayPeriod payPeriod,
            List<TimeEntry> allTimeEntries,
            List<TimeEntry> branchTimeEntries,
            List<ActualTimeEntry> branchActualTimeEntries)
        {
            if (employee == null)
                return null;
            if (branchTimeEntries == null || branchTimeEntries.Any() == false)
                return null;

            CostCenterPaystubModel paystubModel = new CostCenterPaystubModel();
            paystubModel.Employee = employee;

            if (salary != null)
                paystubModel = ComputeHoursAndPay(
                    paystubModel,
                    employee,
                    salary,
                    isActual,
                    timeEntries: branchTimeEntries,
                    actualTimeEntries: branchActualTimeEntries);

            if (currentPaystub != null && currentPaystub.RegularHours > 0)
            {
                // Check the percentage of work hours the employee worked in this branch this cut off
                // If the employee worked for 200 hours in total for the whole month, and he worked 40 hours
                // in this branch this cutoff, then he worked 40% of his total worked hours
                // in this branch this cutoff.
                var totalWorhkedHoursThisMonth = monthlyPaystubs
                    .Sum(x => x.TotalWorkedHoursWithoutLeave(employee.IsMonthly));
                var workedPercentage = AccuMath
                    .CommercialRound(paystubModel.TotalWorkedHoursWithoutLeave / totalWorhkedHoursThisMonth); // 40 / 200

                paystubModel = ComputeGovernmentDeductions(
                    hmoLoan,
                    monthlyDeduction,
                    paystubModel,
                    workedPercentage);
            }

            paystubModel.TotalAllowance = ComputeTotalAllowance(
                dailyAllowances,
                settings,
                calendarCollection,
                userId,
                currentPaystub,
                paystubModel.Employee,
                payPeriod,
                allTimeEntries: allTimeEntries,
                branchTimeEntries: branchTimeEntries);

            paystubModel.ThirteenthMonthPay = ComputeThirteenthMonthPay(
                employee,
                branchTimeEntries: branchTimeEntries,
                branchActualTimeEntries: branchActualTimeEntries);
            return paystubModel;
        }

        private static decimal ComputeThirteenthMonthPay(
            Employee employee,
            List<TimeEntry> branchTimeEntries,
            List<ActualTimeEntry> branchActualTimeEntries)
        {
            var thirteenthMonthPay = ThirteenthMonthPayCalculator
                .ComputeByRegularPayAndAllowanceDaily(
                    employee,
                    branchTimeEntries,
                    branchActualTimeEntries);

            return AccuMath.CommercialRound(thirteenthMonthPay / CalendarConstant.MonthsInAYear);
        }

        private static decimal ComputeTotalAllowance(
            List<Allowance> dailyAllowances,
            ListOfValueCollection settings,
            CalendarCollection calendarCollection,
            int userId,
            Paystub currentPaystub,
            Employee employee,
            PayPeriod payPeriod,
            List<TimeEntry> allTimeEntries,
            List<TimeEntry> branchTimeEntries)
        {
            var allowanceCalculator = new DailyAllowanceCalculator(
                new AllowancePolicy(settings),
                employee,
                currentPaystub,
                payPeriod,
                calendarCollection,
                previousTimeEntries: allTimeEntries,
                timeEntries: branchTimeEntries,
                currentlyLoggedInUserId: userId);

            decimal totalAllowance = 0;

            foreach (var allowance in dailyAllowances)
            {
                var allowanceItem = allowanceCalculator.Compute(allowance);

                if (allowanceItem != null)
                {
                    totalAllowance += allowanceItem.Amount;
                }
            }

            return totalAllowance;
        }

        private static CostCenterPaystubModel ComputeGovernmentDeductions(
            LoanTransaction hmoLoan,
            MonthlyDeduction monthlyDeduction,
            CostCenterPaystubModel paystubModel,
            decimal workedPercentage)
        {
            paystubModel.HMOAmount = MonthlyDeductionAmount
                .ComputeBranchPercentage(
                    amount: hmoLoan?.DeductionAmount ?? 0,
                    branchPercentage: workedPercentage);

            paystubModel.SSSAmount = monthlyDeduction.SSSAmount.GetBranchPercentage(workedPercentage);

            paystubModel.ECAmount = monthlyDeduction.ECAmount.GetBranchPercentage(workedPercentage);

            paystubModel.HDMFAmount = monthlyDeduction.HDMFAmount.GetBranchPercentage(workedPercentage);

            paystubModel.PhilHealthAmount = monthlyDeduction.PhilHealthAmount.GetBranchPercentage(workedPercentage);

            return paystubModel;
        }

        private CostCenterPaystubModel()
        {
        }

        private static CostCenterPaystubModel ComputeHoursAndPay(
            CostCenterPaystubModel paystubModel,
            Employee employee,
            Salary salary,
            bool isActual,
            List<TimeEntry> timeEntries,
            List<ActualTimeEntry> actualTimeEntries)
        {
            var totalTimeEntries = TotalTimeEntryCalculator.Calculate(
                timeEntries,
                salary,
                employee,
                actualTimeEntries);

            paystubModel.RegularHours = totalTimeEntries.RegularHours;
            paystubModel.OvertimeHours = AccuMath.CommercialRound(totalTimeEntries.OvertimeHours);
            paystubModel.NightDiffHours = AccuMath.CommercialRound(totalTimeEntries.NightDifferentialHours);
            paystubModel.NightDiffOvertimeHours = AccuMath.CommercialRound(totalTimeEntries.NightDifferentialOvertimeHours);

            paystubModel.RestDayHours = AccuMath.CommercialRound(totalTimeEntries.RestDayHours);
            paystubModel.RestDayOTHours = AccuMath.CommercialRound(totalTimeEntries.RestDayOTHours);

            /* Not yet supported by TimeEntry
            paystubModel.SpecialHolidayRestDayHours = AccuMath.CommercialRound(totalTimeEntries.SpecialHolidayRestDayHours);
            paystubModel.RegularHolidayRestDayHours = AccuMath.CommercialRound(totalTimeEntries.RegularHolidayRestDayHours);
            paystubModel.SpecialHolidayRestDayOTHours = AccuMath.CommercialRound(totalTimeEntries.SpecialHolidayRestDayOTHours);
            paystubModel.RegularHolidayRestDayOTHours = AccuMath.CommercialRound(totalTimeEntries.RegularHolidayRestDayOTHours);
            */

            paystubModel.SpecialHolidayHours = AccuMath.CommercialRound(totalTimeEntries.SpecialHolidayHours);
            paystubModel.SpecialHolidayOTHours = AccuMath.CommercialRound(totalTimeEntries.SpecialHolidayOTHours);
            paystubModel.RegularHolidayHours = AccuMath.CommercialRound(totalTimeEntries.RegularHolidayHours);
            paystubModel.RegularHolidayOTHours = AccuMath.CommercialRound(totalTimeEntries.RegularHolidayOTHours);

            if (!isActual)
            {
                paystubModel.HourlyRate = totalTimeEntries.HourlyRate;
                paystubModel.OvertimePay = totalTimeEntries.OvertimePay;
                paystubModel.NightDiffPay = totalTimeEntries.NightDiffPay;
                paystubModel.NightDiffOvertimePay = totalTimeEntries.NightDiffOvertimePay;

                paystubModel.RestDayPay = totalTimeEntries.RestDayPay;
                paystubModel.RestDayOTPay = totalTimeEntries.RestDayOTPay;

                paystubModel.SpecialHolidayPay = totalTimeEntries.SpecialHolidayPay;
                paystubModel.SpecialHolidayOTPay = totalTimeEntries.SpecialHolidayOTPay;
                paystubModel.RegularHolidayPay = totalTimeEntries.RegularHolidayPay;
                paystubModel.RegularHolidayOTPay = totalTimeEntries.RegularHolidayOTPay;
            }
            else
            {
                paystubModel.HourlyRate = totalTimeEntries.ActualHourlyRate;
                paystubModel.OvertimePay = totalTimeEntries.ActualOvertimePay;
                paystubModel.NightDiffPay = totalTimeEntries.ActualNightDiffPay;
                paystubModel.NightDiffOvertimePay = totalTimeEntries.ActualNightDiffOvertimePay;

                paystubModel.RestDayPay = totalTimeEntries.ActualRestDayPay;
                paystubModel.RestDayOTPay = totalTimeEntries.ActualRestDayOTPay;

                paystubModel.SpecialHolidayPay = totalTimeEntries.ActualSpecialHolidayPay;
                paystubModel.SpecialHolidayOTPay = totalTimeEntries.ActualSpecialHolidayOTPay;
                paystubModel.RegularHolidayPay = totalTimeEntries.ActualRegularHolidayPay;
                paystubModel.RegularHolidayOTPay = totalTimeEntries.ActualRegularHolidayOTPay;
            }

            return paystubModel;
        }

        private Employee Employee { get; set; }

        private int EmployeeId => Employee.RowID.Value;

        private string EmployeeName => Employee.FullNameWithMiddleInitialLastNameFirst.ToUpper()/* +
                                            " " +
                                            Employee.EmployeeNo*/;

        private decimal RegularDays => RegularHours / PayrollTools.WorkHoursPerDay;

        private decimal RegularHours { get; set; }

        private decimal DailyRate => AccuMath.CommercialRound(_hourlyRate * PayrollTools.WorkHoursPerDay);

        private decimal _hourlyRate;

        private decimal HourlyRate
        {
            get => AccuMath.CommercialRound(_hourlyRate);
            set => _hourlyRate = value;
        }

        private decimal RegularPay => AccuMath.CommercialRound(_hourlyRate * RegularHours);

        private decimal OvertimeHours { get; set; }
        private decimal OvertimePay { get; set; }
        private decimal NightDiffHours { get; set; }
        private decimal NightDiffPay { get; set; }
        private decimal NightDiffOvertimeHours { get; set; }
        private decimal NightDiffOvertimePay { get; set; }
        private decimal RestDayHours { get; set; }
        private decimal RestDayPay { get; set; }
        private decimal RestDayOTHours { get; set; }
        private decimal RestDayOTPay { get; set; }
        private decimal SpecialHolidayHours { get; set; }
        private decimal SpecialHolidayPay { get; set; }
        private decimal SpecialHolidayOTHours { get; set; }
        private decimal SpecialHolidayOTPay { get; set; }
        private decimal RegularHolidayHours { get; set; }
        private decimal RegularHolidayPay { get; set; }
        private decimal RegularHolidayOTHours { get; set; }
        private decimal RegularHolidayOTPay { get; set; }

        #region Not supported by TimeEntry yet

        private decimal SpecialHolidayRestDayHours { get; set; }
        private decimal RegularHolidayRestDayHours { get; set; }
        private decimal SpecialHolidayRestDayOTHours { get; set; }
        private decimal RegularHolidayRestDayOTHours { get; set; }

        #endregion Not supported by TimeEntry yet

        private decimal TotalAllowance { get; set; }

        #region same formula in Paystub.cs

        public decimal TotalRestDayHours => RestDayHours + SpecialHolidayRestDayHours + RegularHolidayRestDayHours;

        public decimal RegularHoursAndTotalRestDay => RegularHours + TotalRestDayHours;

        public decimal TotalOvertimeHours =>
            OvertimeHours +
            RestDayOTHours +
            SpecialHolidayOTHours +
            RegularHolidayOTHours +
            SpecialHolidayRestDayOTHours +
            RegularHolidayRestDayOTHours;

        public decimal TotalWorkedHoursWithoutOvertimeAndLeave =>
            RegularHoursAndTotalRestDay +
            SpecialHolidayHours +
            RegularHolidayHours;

        public decimal TotalWorkedHoursWithoutLeave => TotalWorkedHoursWithoutOvertimeAndLeave + TotalOvertimeHours;

        #endregion same formula in Paystub.cs

        public decimal GrossPay => AccuMath.CommercialRound(
            RegularPay +
            OvertimePay +
            NightDiffPay +
            NightDiffOvertimePay +
            RestDayPay +
            RestDayOTPay +
            SpecialHolidayPay +
            SpecialHolidayOTPay +
            RegularHolidayPay +
            RegularHolidayOTPay +
            TotalAllowance);

        private decimal SSSAmount { get; set; }
        private decimal ECAmount { get; set; }
        private decimal HDMFAmount { get; set; }
        private decimal PhilHealthAmount { get; set; }
        private decimal HMOAmount { get; set; }
        private decimal ThirteenthMonthPay { get; set; }

        private decimal FiveDaySilpAmount
        {
            get
            {
                if (Employee.WorkDaysPerYear == 0 || Employee.VacationLeaveAllowance == 0) return 0;

                var vacationLeavePerYearInDays = Employee.VacationLeaveAllowance / PayrollTools.WorkHoursPerDay;
                var daysPerMonths = Employee.WorkDaysPerYear / PayrollTools.MonthsPerYear;

                return AccuMath.CommercialRound(this.RegularPay * vacationLeavePerYearInDays / PayrollTools.MonthsPerYear / daysPerMonths);
            }
        }

        private decimal TotalDeductions
        {
            get
            {
                return SSSAmount + ECAmount + HDMFAmount + PhilHealthAmount + HMOAmount + ThirteenthMonthPay + FiveDaySilpAmount;
            }
        }

        private decimal NetPay
        {
            get
            {
                // Total deductions are added since cost center report is for
                // franchise/branch owners. They will pay the employer deductions.
                // Net pay is how much they will pay per employee
                return GrossPay + TotalDeductions;
            }
        }

        private Dictionary<string, string> _lookUp;

        public Dictionary<string, string> LookUp
        {
            get
            {
                if (_lookUp != null)
                    return _lookUp;

                _lookUp = new Dictionary<string, string>();
                _lookUp["EmployeeId"] = this.EmployeeId.ToString();
                _lookUp["EmployeeName"] = this.EmployeeName;
                _lookUp["TotalDays"] = this.RegularDays.ToString();
                _lookUp["TotalHours"] = this.RegularHours.ToString();
                _lookUp["DailyRate"] = this.DailyRate.ToString();
                _lookUp["HoulyRate"] = this.HourlyRate.ToString();
                _lookUp["BasicPay"] = this.RegularPay.ToString();
                _lookUp["OvertimeHours"] = this.OvertimeHours.ToString();
                _lookUp["OvertimePay"] = this.OvertimePay.ToString();
                _lookUp["NightDiffHours"] = this.NightDiffHours.ToString();
                _lookUp["NightDiffPay"] = this.NightDiffPay.ToString();
                _lookUp["NightDiffOvertimeHours"] = this.NightDiffOvertimeHours.ToString();
                _lookUp["NightDiffOvertimePay"] = this.NightDiffOvertimePay.ToString();
                _lookUp["RestDayHours"] = this.RestDayHours.ToString();
                _lookUp["RestDayPay"] = this.RestDayPay.ToString();
                _lookUp["RestDayOTHours"] = this.RestDayOTHours.ToString();
                _lookUp["RestDayOTPay"] = this.RestDayOTPay.ToString();
                _lookUp["SpecialHolidayHours"] = this.SpecialHolidayHours.ToString();
                _lookUp["SpecialHolidayPay"] = this.SpecialHolidayPay.ToString();
                _lookUp["SpecialHolidayOTHours"] = this.SpecialHolidayOTHours.ToString();
                _lookUp["SpecialHolidayOTPay"] = this.SpecialHolidayOTPay.ToString();
                _lookUp["RegularHolidayHours"] = this.RegularHolidayHours.ToString();
                _lookUp["RegularHolidayPay"] = this.RegularHolidayPay.ToString();
                _lookUp["RegularHolidayOTHours"] = this.RegularHolidayOTHours.ToString();
                _lookUp["RegularHolidayOTPay"] = this.RegularHolidayOTPay.ToString();
                _lookUp["TotalAllowanceKey"] = this.TotalAllowance.ToString();
                _lookUp["GrossPay"] = this.GrossPay.ToString();
                _lookUp["SSSAmount"] = this.SSSAmount.ToString();
                _lookUp["ECAmount"] = this.ECAmount.ToString();
                _lookUp["HDMFAmount"] = this.HDMFAmount.ToString();
                _lookUp["PhilHealthAmount"] = this.PhilHealthAmount.ToString();
                _lookUp["HMOAmount"] = this.HMOAmount.ToString();
                _lookUp["ThirteenthMonthPay"] = this.ThirteenthMonthPay.ToString();
                _lookUp["FiveDaySilpAmount"] = this.FiveDaySilpAmount.ToString();
                _lookUp["NetPay"] = this.NetPay.ToString();

                return _lookUp;
            }
        }
    }

    public class MonthlyDeduction
    {
        public static MonthlyDeduction Create(
            int employeeId,
            decimal sssAmount,
            decimal ecAmount,
            decimal hdmfAmount,
            decimal philhealthAmount,
            decimal thirteenthMonthPay)
        {
            return new MonthlyDeduction()
            {
                EmployeeID = employeeId,
                SSSAmount = MonthlyDeductionAmount.Create(sssAmount),
                ECAmount = MonthlyDeductionAmount.Create(ecAmount),
                HDMFAmount = MonthlyDeductionAmount.Create(hdmfAmount),
                PhilHealthAmount = MonthlyDeductionAmount.Create(philhealthAmount),
                ThirteenthMonthPay = MonthlyDeductionAmount.Create(thirteenthMonthPay)
            };
        }

        private MonthlyDeduction()
        {
        }

        public int EmployeeID { get; set; }

        public MonthlyDeductionAmount SSSAmount { get; set; }
        public MonthlyDeductionAmount ECAmount { get; set; }
        public MonthlyDeductionAmount HDMFAmount { get; set; }
        public MonthlyDeductionAmount PhilHealthAmount { get; set; }
        public MonthlyDeductionAmount ThirteenthMonthPay { get; set; }
    }

    public class MonthlyDeductionAmount
    {
        public static MonthlyDeductionAmount Create(decimal amount)
        {
            return new MonthlyDeductionAmount(amount);
        }

        private MonthlyDeductionAmount(decimal amount)
        {
            MonthlyAmount = AccuMath.CommercialRound(amount);
        }

        public decimal MonthlyAmount { get; }

        public decimal GetBranchPercentage(decimal branchPercentage) =>
            ComputeBranchPercentage(
                amount: MonthlyAmount,
                branchPercentage: branchPercentage);

        public static decimal ComputeBranchPercentage(decimal amount, decimal branchPercentage) =>
            AccuMath.CommercialRound(amount * branchPercentage);
    }
}
