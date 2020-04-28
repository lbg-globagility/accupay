using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class CostCenterReportDataService
    {
        private readonly DateTime _selectedMonth;
        private readonly Branch _selectedBranch;
        private readonly int _organizationId;
        private readonly int _userId;

        public CostCenterReportDataService(DateTime selectedMonth,
                                            Branch selectedBranch,
                                            int organizationId,
                                            int userId)
        {
            this._selectedMonth = selectedMonth;
            this._selectedBranch = selectedBranch;
            this._organizationId = organizationId;
            this._userId = userId;

            if (_selectedBranch?.RowID == null)
                throw new Exception("Branch does not exists.");
        }

        public List<PayPeriodModel> GetData()
        {
            List<PayPeriodModel> payPeriodModels = new List<PayPeriodModel>();

            using (PayrollContext context = new PayrollContext())
            {
                var payPeriods = context.PayPeriods.
                                            Where(p => p.OrganizationID == _organizationId).
                                            Where(p => p.IsSemiMonthly).
                                            Where(p => p.Year == _selectedMonth.Year).
                                            Where(p => p.Month == _selectedMonth.Month).
                                            ToList();

                if (payPeriods.Count != 2)
                    throw new Exception($"Pay periods on the selected month was {payPeriods.Count} instead of 2 (First half, End of the month)");

                DateTime startDate = new DateTime[] { payPeriods[0].PayFromDate, payPeriods[1].PayFromDate }.Min();
                DateTime endDate = new DateTime[] { payPeriods[0].PayToDate, payPeriods[1].PayToDate }.Max();

                var timeEntries = context.TimeEntries.
                                            Include(t => t.Employee).
                                            Where(t => t.Employee.OrganizationID == _organizationId).
                                            Where(t => t.Date >= startDate && t.Date <= endDate).
                                            ToList();

                var employeesWithTimeEntriesInBranch = timeEntries.
                                                            Where(t => t.BranchID != null).
                                                            Where(t => t.BranchID == _selectedBranch.RowID).
                                                            GroupBy(t => t.EmployeeID).
                                                            Select(t => t.Key).
                                                            ToArray();

                // Get all the employee in the branch
                // Also get the employees that has at least 1 timelogs on the branch
                var employees = context.Employees.
                                            Where(e => e.IsDaily).
                                            Where(e => (e.BranchID.HasValue &&
                                                            e.BranchID == _selectedBranch.RowID.Value) ||
                                                        employeesWithTimeEntriesInBranch.Contains(e.RowID.Value)).
                                            ToList();

                // if timeEntry's BranchID is Nothing, set it to
                // employee's BranchID for easier querying
                AddBranchToTimeEntries(timeEntries, employees);

                timeEntries = timeEntries.
                                Where(t => t.BranchID != null).
                                Where(t => t.BranchID == _selectedBranch.RowID).
                                ToList();

                var salaries = context.Salaries.
                                        Where(s => s.OrganizationID == _organizationId).
                                        Where(s => s.EffectiveFrom <= startDate).
                                        ToList();

                var employeePaystubs = context.Paystubs.
                                                Include(p => p.ThirteenthMonthPay).
                                                Where(p => p.PayPeriodID == payPeriods[0].RowID ||
                                                                p.PayPeriodID == payPeriods[1].RowID).
                                                ToList();

                var employeeMonthlyDeductions = GenerateMonthlyDeductionList(payPeriods, employees, employeePaystubs);

                var hmoLoanType = new ProductRepository().
                                        GetOrCreateAdjustmentTypeAsync(ProductConstant.HMO_LOAN,
                                                                    organizationId: _organizationId,
                                                                    userId: _userId);

                var hmoLoans = context.LoanTransactions.
                                        Include(l => l.Paystub).
                                        Where(l => l.PayPeriodID == payPeriods[0].RowID
                                                        || l.PayPeriodID == payPeriods[1].RowID).
                                        ToList();

                payPeriodModels = CreatePayPeriodModels(payPeriods, timeEntries, employees, salaries, employeePaystubs, employeeMonthlyDeductions, hmoLoans);
            }

            return payPeriodModels;
        }

        private List<MonthlyDeduction> GenerateMonthlyDeductionList(List<PayPeriod> payPeriods, List<Employee> employees, List<Paystub> allPaystubs)
        {
            List<MonthlyDeduction> employeeMonthlyDeductions = new List<MonthlyDeduction>();

            List<SocialSecurityBracket> sssBrackets;

            using (PayrollContext context = new PayrollContext())
            {
                var taxEffectivityDate = new DateTime(payPeriods[0].Year, payPeriods[0].Month, 1);
                sssBrackets = context.SocialSecurityBrackets.
                                        Where(s => taxEffectivityDate >= s.EffectiveDateFrom).
                                        Where(s => taxEffectivityDate <= s.EffectiveDateTo).
                                        ToList();
            }

            foreach (var employee in employees)
            {
                if (employee.RowID == null)
                    throw new Exception("Employee does not exists");

                var employeePaystubs = allPaystubs.
                                        Where(p => p.EmployeeID == employee.RowID).
                                        ToList();

                if (employeePaystubs.Count > 2)
                    throw new Exception("Only up to 2 paystubs should be computed per employee. First half and end of the month paystubs.");

                decimal sssAmount = 0;
                decimal ecAmount = 0;
                decimal hdmfAmount = 0;
                decimal philhealthAmount = 0;
                decimal thirteenthMonthPay = 0;

                if (employeePaystubs.Any())
                {
                    hdmfAmount = employeePaystubs.Sum(p => p.HdmfEmployerShare);
                    philhealthAmount = employeePaystubs.Sum(p => p.PhilHealthEmployerShare);
                    thirteenthMonthPay = employeePaystubs.Sum(p => p.ThirteenthMonthPay.Amount);

                    var sssPayables = GetEmployerSSSPayables(sssBrackets,
                                                            employeePaystubs[0].SssEmployeeShare);
                    sssAmount = sssPayables.EmployerShare;
                    ecAmount = sssPayables.ECamount;

                    // check if there is a 2nd paystub (could be only 1 paystub for the month)
                    if (employeePaystubs.Count == 2)
                    {
                        sssPayables = GetEmployerSSSPayables(sssBrackets,
                                                    employeePaystubs[1].SssEmployeeShare);
                        sssAmount += sssPayables.EmployerShare;
                        ecAmount += sssPayables.ECamount;
                    }
                }

                employeeMonthlyDeductions.Add(MonthlyDeduction.
                                            Create(employeeId: employee.RowID.Value,
                                                    sssAmount: sssAmount,
                                                    ecAmount: ecAmount,
                                                    hdmfAmount: hdmfAmount,
                                                    philhealthAmount: philhealthAmount,
                                                    thirteenthMonthPay: thirteenthMonthPay));
            }

            return employeeMonthlyDeductions;
        }

        private static SSSEmployerShare GetEmployerSSSPayables(List<SocialSecurityBracket> sssBrackets, decimal employeeShare)
        {
            if (employeeShare == 0)
                return SSSEmployerShare.Zero;

            // SSS employer share and EC are not saved in the database. To get those data
            // we need to query the SSS bracket and get by employee contribution amount
            var sssBracket = sssBrackets.Where(s => s.EmployeeContributionAmount == employeeShare).
                                            FirstOrDefault();

            var sssAmount = sssBracket?.EmployerContributionAmount ?? 0;
            var ecAmount = sssBracket?.EmployeeECAmount ?? 0;

            return new SSSEmployerShare(employerShare: sssAmount, ECamount: ecAmount);
        }

        private void AddBranchToTimeEntries(List<TimeEntry> timeEntries, List<Employee> employees)
        {
            foreach (var timeEntry in timeEntries)
            {
                if (timeEntry.BranchID.HasValue)
                    continue;

                timeEntry.BranchID = employees.
                            FirstOrDefault(e => e.RowID == timeEntry.EmployeeID)?.BranchID;
            }
        }

        private static List<PayPeriodModel> CreatePayPeriodModels(List<PayPeriod> payPeriods, List<TimeEntry> allTimeEntries, List<Employee> employees, List<Salary> salaries, List<Paystub> paystubs, List<MonthlyDeduction> monthlyDeductions, List<LoanTransaction> hmoLoans)
        {
            List<PayPeriodModel> payPeriodModels = new List<PayPeriodModel>();
            foreach (var payPeriod in payPeriods)
            {
                var payPeriodPaystubs = paystubs.
                                            Where(p => p.PayPeriodID == payPeriod.RowID).
                                            ToList();

                var payPeriodHmoLoans = hmoLoans.
                                            Where(p => p.PayPeriodID == payPeriod.RowID).
                                            ToList();

                var paystubModels = CreatePaystubModels(allTimeEntries,
                                                        employees,
                                                        salaries,
                                                        payPeriod,
                                                        payPeriodPaystubs,
                                                        monthlyDeductions,
                                                        payPeriodHmoLoans);

                payPeriodModels.Add(new PayPeriodModel()
                {
                    PayPeriod = payPeriod,
                    Paystubs = paystubModels
                });
            }

            return payPeriodModels;
        }

        private static List<PaystubModel> CreatePaystubModels(List<TimeEntry> allTimeEntries, List<Employee> employees, List<Salary> salaries, PayPeriod payPeriod, List<Paystub> paystubs, List<MonthlyDeduction> monthlyDeductions, List<LoanTransaction> hmoLoans)
        {
            List<PaystubModel> paystubModels = new List<PaystubModel>();

            foreach (var employee in employees)
            {
                var timeEntries = allTimeEntries.
                                    Where(t => t.Date >= payPeriod.PayFromDate).
                                    Where(t => t.Date <= payPeriod.PayToDate).
                                    Where(t => t.EmployeeID == employee.RowID).
                                    ToList();

                var salary = salaries.
                                Where(s => s.EmployeeID == employee.RowID).
                                Where(s => s.EffectiveFrom <= payPeriod.PayFromDate).
                                OrderByDescending(s => s.EffectiveFrom).
                                FirstOrDefault();

                var paystub = paystubs.
                                Where(p => p.EmployeeID == employee.RowID).
                                FirstOrDefault();

                var monthlyDeduction = monthlyDeductions.
                                        Where(d => d.EmployeeID == employee.RowID).
                                        FirstOrDefault();

                var hmoLoan = hmoLoans.
                                Where(h => h.EmployeeID == employee.RowID).
                                FirstOrDefault();

                var createdPaystubModel = PaystubModel.Create(employee,
                                                                salary,
                                                                timeEntries,
                                                                paystub,
                                                                monthlyDeduction,
                                                                hmoLoan);

                if (createdPaystubModel != null && createdPaystubModel.GrossPay > 0)
                    paystubModels.Add(createdPaystubModel);
            }

            return paystubModels;
        }

        #region Custom Classes

        public class PayPeriodModel
        {
            public PayPeriod PayPeriod { get; set; }

            public List<PaystubModel> Paystubs { get; set; }
        }

        public class SSSEmployerShare
        {
            public SSSEmployerShare(decimal employerShare, decimal ECamount)
            {
                this.EmployerShare = employerShare;
                this.ECamount = ECamount;
            }

            public decimal EmployerShare { get; }
            public decimal ECamount { get; }

            internal static SSSEmployerShare Zero => new SSSEmployerShare(0, 0);
        }

        public class PaystubModel
        {
            public static PaystubModel Create(Employee employee,
                                                Salary salary,
                                                List<TimeEntry> timeEntries,
                                                Paystub currentPaystub,
                                                MonthlyDeduction monthlyDeduction,
                                                LoanTransaction hmoLoan)
            {
                if (employee == null)
                    return null;
                if (timeEntries == null || timeEntries.Any() == false)
                    return null;

                PaystubModel paystubModel = new PaystubModel();
                paystubModel.Employee = employee;

                if (salary != null)
                    paystubModel = ComputeHoursAndPay(paystubModel, employee, salary, timeEntries);

                if (currentPaystub != null)
                {
                    // Check the percentage of work hours the employee worked in this branch
                    // If employee worked for 100 hours in total, and he worked 40 hours in this branch,
                    // then he worked 40% of his total worked hours in this branch.
                    var workedPercentage = AccuMath.CommercialRound(paystubModel.RegularHours / currentPaystub.RegularHours); // 40 / 100
                    paystubModel = ComputeGovernmentDeductions(hmoLoan, monthlyDeduction, paystubModel, workedPercentage);
                }

                return paystubModel;
            }

            private static PaystubModel ComputeGovernmentDeductions(LoanTransaction hmoLoan, MonthlyDeduction monthlyDeduction, PaystubModel paystubModel, decimal workedPercentage)
            {
                // Test hmoLoan
                paystubModel.HMOAmount = MonthlyDeductionAmount.ComputeBranchPercentage(hmoLoan?.Amount ?? 0, workedPercentage);

                paystubModel.SSSAmount = monthlyDeduction.SSSAmount.GetBranchPercentage(workedPercentage);

                paystubModel.ECAmount = monthlyDeduction.ECAmount.GetBranchPercentage(workedPercentage);

                paystubModel.HDMFAmount = monthlyDeduction.HDMFAmount.GetBranchPercentage(workedPercentage);

                paystubModel.PhilHealthAmount = monthlyDeduction.PhilHealthAmount.GetBranchPercentage(workedPercentage);

                paystubModel.ThirteenthMonthPay = monthlyDeduction.ThirteenthMonthPay.GetBranchPercentage(workedPercentage);

                return paystubModel;
            }

            private PaystubModel()
            {
            }

            private static PaystubModel ComputeHoursAndPay(PaystubModel paystubModel, Employee employee, Salary salary, List<TimeEntry> timeEntries)
            {
                var totalTimeEntries = TotalTimeEntryCalculator.Calculate(timeEntries,
                                                                            salary,
                                                                            employee,
                                                                            new List<ActualTimeEntry>());

                paystubModel.RegularHours = totalTimeEntries.RegularHours;
                paystubModel.HourlyRate = totalTimeEntries.HourlyRate;

                paystubModel.OvertimeHours = AccuMath.CommercialRound(totalTimeEntries.OvertimeHours);
                paystubModel.OvertimePay = totalTimeEntries.OvertimePay;

                paystubModel.NightDiffHours = AccuMath.CommercialRound(totalTimeEntries.NightDiffHours);
                paystubModel.NightDiffPay = totalTimeEntries.NightDiffPay;

                paystubModel.NightDiffOvertimeHours = AccuMath.CommercialRound(totalTimeEntries.NightDiffOvertimeHours);
                paystubModel.NightDiffOvertimePay = totalTimeEntries.NightDiffOvertimePay;

                paystubModel.SpecialHolidayHours = AccuMath.CommercialRound(totalTimeEntries.SpecialHolidayHours);
                paystubModel.SpecialHolidayPay = totalTimeEntries.SpecialHolidayPay;

                paystubModel.SpecialHolidayOTHours = AccuMath.CommercialRound(totalTimeEntries.SpecialHolidayOTHours);
                paystubModel.SpecialHolidayOTPay = totalTimeEntries.SpecialHolidayOTPay;

                paystubModel.RegularHolidayHours = AccuMath.CommercialRound(totalTimeEntries.RegularHolidayHours);
                paystubModel.RegularHolidayPay = totalTimeEntries.RegularHolidayPay;

                paystubModel.RegularHolidayOTHours = AccuMath.CommercialRound(totalTimeEntries.RegularHolidayOTHours);
                paystubModel.RegularHolidayOTPay = totalTimeEntries.RegularHolidayOTPay;
                return paystubModel;
            }

            private Employee Employee { get; set; }

            private int EmployeeId => Employee.RowID.Value;

            private string EmployeeName => Employee.FullNameWithMiddleInitialLastNameFirst.ToUpper() +
                                            " " +
                                            Employee.EmployeeNo;

            private decimal RegularDays => RegularHours / PayrollTools.WorkHoursPerDay;

            private decimal RegularHours { get; set; }

            private decimal DailyRate => AccuMath.CommercialRound(_hourlyRate *
                                                                PayrollTools.WorkHoursPerDay);

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
            private decimal SpecialHolidayHours { get; set; }
            private decimal SpecialHolidayPay { get; set; }
            private decimal SpecialHolidayOTHours { get; set; }
            private decimal SpecialHolidayOTPay { get; set; }
            private decimal RegularHolidayHours { get; set; }
            private decimal RegularHolidayPay { get; set; }
            private decimal RegularHolidayOTHours { get; set; }
            private decimal RegularHolidayOTPay { get; set; }

            public decimal GrossPay => AccuMath.CommercialRound(RegularPay +
                                                    OvertimePay +
                                                    NightDiffPay +
                                                    NightDiffOvertimePay +
                                                    SpecialHolidayPay +
                                                    SpecialHolidayOTPay +
                                                    RegularHolidayPay +
                                                    RegularHolidayOTPay);

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
                    int daysPerCutoff = 13; // this should probably be retrieved from employee.WorkDaysPerYear / 12 / 2

                    var vacationLeavePerYearInDays = Employee.VacationLeaveAllowance / PayrollTools.WorkHoursPerDay;

                    var basicRate = DailyRate * daysPerCutoff; // this should probably be retrieved from PayrollTools.GetEmployeeMonthlyRate / 2

                    return AccuMath.CommercialRound(basicRate * vacationLeavePerYearInDays / PayrollTools.MonthsPerYear / daysPerCutoff);
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
                    // Net pay is how much they will per employee
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
                    _lookUp["SpecialHolidayHours"] = this.SpecialHolidayHours.ToString();
                    _lookUp["SpecialHolidayPay"] = this.SpecialHolidayPay.ToString();
                    _lookUp["SpecialHolidayOTHours"] = this.SpecialHolidayOTHours.ToString();
                    _lookUp["SpecialHolidayOTPay"] = this.SpecialHolidayOTPay.ToString();
                    _lookUp["RegularHolidayHours"] = this.RegularHolidayHours.ToString();
                    _lookUp["RegularHolidayPay"] = this.RegularHolidayPay.ToString();
                    _lookUp["RegularHolidayOTHours"] = this.RegularHolidayOTHours.ToString();
                    _lookUp["RegularHolidayOTPay"] = this.RegularHolidayOTPay.ToString();
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
            public static MonthlyDeduction Create(int employeeId, decimal sssAmount, decimal ecAmount, decimal hdmfAmount, decimal philhealthAmount, decimal thirteenthMonthPay)
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

            public decimal SemiMonthlyAmount => AccuMath.CommercialRound(MonthlyAmount / 2);

            public decimal Amount => MonthlyAmount;

            public decimal GetBranchPercentage(decimal branchPercentage) =>
                                    ComputeBranchPercentage(SemiMonthlyAmount, branchPercentage);

            public static decimal ComputeBranchPercentage(decimal amount, decimal branchPercentage) =>
                                    AccuMath.CommercialRound(amount * branchPercentage);
        }

        #endregion Custom Classes
    }
}