using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
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
        private readonly int _userId;

        public CostCenterReportDataService(DateTime selectedMonth,
                                            Branch selectedBranch,
                                            int userId)
        {
            this._selectedMonth = selectedMonth;
            this._selectedBranch = selectedBranch;
            this._userId = userId;

            if (_selectedBranch?.RowID == null)
                throw new Exception("Branch does not exists.");
        }

        public List<PayPeriodModel> GetData()
        {
            List<PayPeriodModel> payPeriodModels = new List<PayPeriodModel>();

            using (PayrollContext context = new PayrollContext())
            {
                var payPeriods = GetPayPeriod(context);
                DateTime startDate = new DateTime[] { payPeriods[0].Start, payPeriods[1].Start }.Min();
                DateTime endDate = new DateTime[] { payPeriods[0].End, payPeriods[1].End }.Max();

                var reportPeriod = new TimePeriod(startDate, endDate);

                var dateForCheckingLastWorkingDay = PayrollTools.
                                GetPreviousCutoffDateForCheckingLastWorkingDay(reportPeriod.Start);

                var allTimeEntries = context.TimeEntries.
                                            Include(t => t.Employee).
                                            Where(t => t.Date >= dateForCheckingLastWorkingDay).
                                            Where(t => t.Date <= reportPeriod.End).
                                            ToList();

                var payPeriodTimeEntries = allTimeEntries.
                                            Where(t => t.Date >= reportPeriod.Start).
                                            Where(t => t.Date <= reportPeriod.End).
                                            ToList();

                // Get all the employee in the branch
                // Also get the employees that has at least 1 timelogs on the branch
                List<Employee> employees = GetEmployeeFromSelectedBranch(context,
                                                                        payPeriodTimeEntries,
                                                                        _selectedBranch.RowID.Value);

                var employeeIds = employees.
                                        GroupBy(x => x.RowID.Value).
                                        Select(x => x.Key).
                                        ToArray();
                var organizationids = employees.
                                        GroupBy(x => x.OrganizationID.Value).
                                        Select(x => x.Key).
                                        ToArray();

                // if timeEntry's BranchID is Nothing, set it to
                // employee's BranchID for easier querying
                AddBranchToTimeEntries(payPeriodTimeEntries, employees);

                var branchTimeEntries = payPeriodTimeEntries.
                                        Where(t => t.BranchID != null).
                                        Where(t => t.BranchID == _selectedBranch.RowID).
                                        ToList();

                var branchActualTimeEntries = GetBranchActualTimeEntries(branchTimeEntries, context);

                var salaries = context.Salaries.
                                        Where(s => s.EffectiveFrom <= reportPeriod.Start).
                                        ToList();

                salaries = salaries.
                                Where(x => employeeIds.Contains(x.EmployeeID.Value)).
                                ToList();

                var employeePaystubs = context.Paystubs.
                                        Include(p => p.ThirteenthMonthPay).
                                        Include(p => p.PayPeriod).
                                        Where(p => p.PayPeriod.PayFromDate >= reportPeriod.Start).
                                        Where(p => p.PayPeriod.PayToDate <= reportPeriod.End).
                                        ToList();

                employeePaystubs = employeePaystubs.
                                        Where(x => employeeIds.Contains(x.EmployeeID.Value)).
                                        ToList();

                var employeeMonthlyDeductions = GenerateMonthlyDeductionList(employeeIds, employeePaystubs);

                var hmoLoans = context.LoanTransactions.
                                        Include(l => l.Paystub).
                                        Include(p => p.PayPeriod).
                                        Include(p => p.LoanSchedule.LoanType).
                                        Where(p => p.PayPeriod.PayFromDate >= reportPeriod.Start).
                                        Where(p => p.PayPeriod.PayToDate <= reportPeriod.End).
                                        Where(p => p.LoanSchedule.LoanType.Name == ProductConstant.HMO_LOAN).
                                        ToList();

                hmoLoans = hmoLoans.
                                Where(x => employeeIds.Contains(x.EmployeeID.Value)).
                                ToList();

                var settings = ListOfValueCollection.Create();

                var calendarCollections = GetCalendarCollections(organizationids, reportPeriod, settings);

                var dailyAllowances = GetDailyAllowances(organizationids, reportPeriod, employeeIds);

                var allPayPeriods = context.PayPeriods.
                                                Where(x => organizationids.Contains(x.OrganizationID.Value)).
                                                Where(x => x.Year == _selectedMonth.Year).
                                                Where(x => x.Month == _selectedMonth.Month).
                                                ToList();
                payPeriodModels = CreatePayPeriodModels(payPeriods,
                                                        employees,
                                                        salaries,
                                                        employeePaystubs,
                                                        employeeMonthlyDeductions,
                                                        hmoLoans,
                                                        dailyAllowances,
                                                        calendarCollections,
                                                        settings,
                                                        allPayPeriods,
                                                        allTimeEntries: allTimeEntries,
                                                        branchTimeEntries: branchTimeEntries,
                                                        branchActualTimeEntries: branchActualTimeEntries);
            }

            return payPeriodModels;
        }

        private List<ActualTimeEntry> GetBranchActualTimeEntries(List<TimeEntry> branchTimeEntries, PayrollContext context)
        {
            var firstDate = branchTimeEntries.OrderBy(x => x.Date).FirstOrDefault()?.Date;
            var lastDate = branchTimeEntries.OrderBy(x => x.Date).LastOrDefault()?.Date;

            if (firstDate == null || lastDate == null)
            {
                return new List<ActualTimeEntry>();
            }

            var actualTimeEntries = context.ActualTimeEntries.
                                        Where(x => x.Date >= firstDate).
                                        Where(x => x.Date <= lastDate).
                                        ToList();

            var branchActualTimeEntries = new List<ActualTimeEntry>();
            foreach (var actualTimeEntry in actualTimeEntries)
            {
                if (branchTimeEntries.
                        Where(x => x.Date == actualTimeEntry.Date).
                        Where(x => x.EmployeeID == actualTimeEntry.EmployeeID).
                        Any())
                {
                    branchActualTimeEntries.Add(actualTimeEntry);
                }
            }

            return branchActualTimeEntries;
        }

        private List<TimePeriod> GetPayPeriod(PayrollContext context)
        {
            // get a random organizationId just to get a random payperiodId
            var organizationId = context.Organizations.Select(x => x.RowID).FirstOrDefault();
            if (organizationId == null) return null;
            // get a random payperiod just to get the first day and last day of the payroll month
            var payPeriods = context.PayPeriods.
                                        Where(p => p.OrganizationID == organizationId).
                                        Where(p => p.IsSemiMonthly).
                                        Where(p => p.Year == _selectedMonth.Year).
                                        Where(p => p.Month == _selectedMonth.Month).
                                        ToList();

            if (payPeriods.Count != 2)
                throw new Exception($"Pay periods on the selected month was {payPeriods.Count} instead of 2 (First half, End of the month)");

            return new List<TimePeriod>()
            {
                new TimePeriod(payPeriods[0].PayFromDate, payPeriods[0].PayToDate),
                new TimePeriod(payPeriods[1].PayFromDate, payPeriods[1].PayToDate)
            };
        }

        private List<CalendarCollection> GetCalendarCollections(int[] organizationids,
                                                                TimePeriod timePeriod,
                                                                ListOfValueCollection settings)
        {
            var calendarCollections = new List<CalendarCollection>();
            foreach (var organizationId in organizationids)
            {
                // maybe in the future pay rate calculation basis is configurable per organization
                var calculationBasis = settings.GetEnum("Pay rate.CalculationBasis",
                                            Enums.PayRateCalculationBasis.Organization);

                var calendarCollection = PayrollTools.
                                    GetCalendarCollection(timePeriod,
                                                        calculationBasis,
                                                        organizationId);

                if (calendarCollection != null)
                {
                    calendarCollections.Add(calendarCollection);
                }
            }

            return calendarCollections;
        }

        private List<Employee> GetEmployeeFromSelectedBranch(PayrollContext context,
                                                            List<TimeEntry> timeEntries,
                                                            int branchId)
        {
            var employeeIdsWithTimeEntriesInBranch = timeEntries.
                                                        Where(t => t.BranchID != null).
                                                        Where(t => t.BranchID == branchId).
                                                        GroupBy(t => t.EmployeeID).
                                                        Select(t => t.Key).
                                                        ToArray();

            var employeesWithTimeEntriesInBranch = context.Employees.
                                    Where(e => e.IsDaily).
                                    Where(e => employeeIdsWithTimeEntriesInBranch.Contains(e.RowID.Value)).
                                    ToList();

            var employeesFromBranch = context.Employees.
                                        Where(e => e.IsDaily).
                                        Where(e => e.BranchID != null).
                                        Where(e => e.BranchID == _selectedBranch.RowID).
                                        ToList();

            var employees = new List<Employee>();
            employees.AddRange(employeesFromBranch);

            foreach (var employee in employeesWithTimeEntriesInBranch)
            {
                if (employeesFromBranch.Where(x => x.RowID == employee.RowID).Any() == false)
                {
                    employees.Add(employee);
                }
            }

            return employees;
        }

        private List<Allowance> GetDailyAllowances(int[] organizationids, TimePeriod reportPeriod, int[] employeeIds)
        {
            var allowanceRepo = new AllowanceRepository();

            var dailyAllowances = new List<Allowance>();
            foreach (var organizationId in organizationids)
            {
                var allowances = allowanceRepo.GetByPayPeriodWithProduct(
                                                                organizationId: organizationId,
                                                                timePeriod: reportPeriod).
                                                       ToList();

                if (allowances.Where(x => x.IsDaily).Any())
                {
                    dailyAllowances.AddRange(allowances.Where(x => x.IsDaily).ToList());
                }
            }

            return dailyAllowances.Where(x => employeeIds.Contains(x.EmployeeID.Value)).ToList();
        }

        private List<MonthlyDeduction> GenerateMonthlyDeductionList(int[] employeeIds, List<Paystub> allPaystubs)
        {
            List<MonthlyDeduction> employeeMonthlyDeductions = new List<MonthlyDeduction>();

            List<SocialSecurityBracket> sssBrackets;

            using (PayrollContext context = new PayrollContext())
            {
                var taxEffectivityDate = new DateTime(_selectedMonth.Year, _selectedMonth.Month, 1);
                sssBrackets = context.SocialSecurityBrackets.
                                        Where(s => taxEffectivityDate >= s.EffectiveDateFrom).
                                        Where(s => taxEffectivityDate <= s.EffectiveDateTo).
                                        ToList();
            }

            foreach (var employeeId in employeeIds)
            {
                var employeePaystubs = allPaystubs.
                                        Where(p => p.EmployeeID == employeeId).
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

                var divideTheDeductions = employeePaystubs.Count == 2;

                employeeMonthlyDeductions.Add(MonthlyDeduction.
                                            Create(divideTheDeductions,
                                                    employeeId: employeeId,
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

        private List<PayPeriodModel> CreatePayPeriodModels(List<TimePeriod> payPeriods,
                                                            List<Employee> employees,
                                                            List<Salary> salaries,
                                                            List<Paystub> paystubs,
                                                            List<MonthlyDeduction> monthlyDeductions,
                                                            List<LoanTransaction> hmoLoans,
                                                            List<Allowance> dailyAllowances,
                                                            List<CalendarCollection> calendarCollections,
                                                            ListOfValueCollection settings,
                                                            List<PayPeriod> allPayPeriods,
                                                            List<TimeEntry> allTimeEntries,
                                                            List<TimeEntry> branchTimeEntries,
                                                            List<ActualTimeEntry> branchActualTimeEntries)
        {
            List<PayPeriodModel> payPeriodModels = new List<PayPeriodModel>();
            foreach (var payPeriod in payPeriods)
            {
                var payPeriodPaystubs = paystubs.
                                         Where(p => p.PayPeriod.PayFromDate >= payPeriod.Start).
                                         Where(p => p.PayPeriod.PayToDate <= payPeriod.End).
                                         ToList();

                var payPeriodHmoLoans = hmoLoans.
                                            Where(p => p.PayPeriod.PayFromDate >= payPeriod.Start).
                                            Where(p => p.PayPeriod.PayToDate <= payPeriod.End).
                                            ToList();

                var paystubModels = CreatePaystubModels(employees,
                                                        salaries,
                                                        payPeriod,
                                                        payPeriodPaystubs,
                                                        monthlyDeductions,
                                                        payPeriodHmoLoans,
                                                        dailyAllowances,
                                                        calendarCollections,
                                                        settings,
                                                        allPayPeriods,
                                                        allTimeEntries: allTimeEntries,
                                                        branchTimeEntries: branchTimeEntries,
                                                        branchActualTimeEntries: branchActualTimeEntries);

                payPeriodModels.Add(new PayPeriodModel()
                {
                    PayPeriod = payPeriod,
                    Paystubs = paystubModels
                });
            }

            return payPeriodModels;
        }

        private List<PaystubModel> CreatePaystubModels(List<Employee> employees,
                                                            List<Salary> salaries,
                                                            TimePeriod payPeriod,
                                                            List<Paystub> paystubs,
                                                            List<MonthlyDeduction> monthlyDeductions,
                                                            List<LoanTransaction> hmoLoans,
                                                            List<Allowance> dailyAllowances,
                                                            List<CalendarCollection> calendarCollections,
                                                            ListOfValueCollection settings,
                                                            List<PayPeriod> payPeriods,
                                                            List<TimeEntry> allTimeEntries,
                                                            List<TimeEntry> branchTimeEntries,
                                                            List<ActualTimeEntry> branchActualTimeEntries)
        {
            List<PaystubModel> paystubModels = new List<PaystubModel>();

            foreach (var employee in employees)
            {
                var earliestAllTimeEntryDate = allTimeEntries.Select(x => x.Date).
                                                            OrderBy(x => x.Date).
                                                            FirstOrDefault();
                var employeeAllTimeEntries = allTimeEntries.
                                    Where(t => t.Date >= earliestAllTimeEntryDate).
                                    Where(t => t.Date <= payPeriod.End).
                                    Where(t => t.EmployeeID == employee.RowID).
                                    ToList();

                var employeeBranchTimeEntries = branchTimeEntries.
                                    Where(t => t.Date >= payPeriod.Start).
                                    Where(t => t.Date <= payPeriod.End).
                                    Where(t => t.EmployeeID == employee.RowID).
                                    ToList();

                var employeeBranchActualTimeEntries = branchActualTimeEntries.
                                    Where(t => t.Date >= payPeriod.Start).
                                    Where(t => t.Date <= payPeriod.End).
                                    Where(t => t.EmployeeID == employee.RowID).
                                    ToList();

                var employeeAllowances = dailyAllowances.
                                            Where(x => x.EmployeeID == employee.RowID).
                                            ToList();

                var salary = salaries.
                                Where(s => s.EmployeeID == employee.RowID).
                                Where(s => s.EffectiveFrom <= payPeriod.Start).
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

                var organizationId = employee.OrganizationID.Value;
                var calendarCollection = calendarCollections.
                                            Where(x => x.OrganizationId == organizationId).
                                            FirstOrDefault();

                var currentPayPeriod = payPeriods.Where(x => x.OrganizationID == organizationId).
                                                    Where(x => x.PayFromDate == payPeriod.Start).
                                                    Where(x => x.PayToDate == payPeriod.End).
                                                    FirstOrDefault();

                var createdPaystubModel = PaystubModel.Create(employee,
                                                                salary,
                                                                paystub,
                                                                monthlyDeduction,
                                                                hmoLoan,
                                                                employeeAllowances,
                                                                settings,
                                                                calendarCollection,
                                                                _userId,
                                                                currentPayPeriod,
                                                                allTimeEntries: employeeAllTimeEntries,
                                                                branchTimeEntries: employeeBranchTimeEntries,
                                                                branchActualTimeEntries: employeeBranchActualTimeEntries);

                if (createdPaystubModel != null && createdPaystubModel.GrossPay > 0)
                    paystubModels.Add(createdPaystubModel);
            }

            return paystubModels;
        }

        #region Custom Classes

        public class PayPeriodModel
        {
            public TimePeriod PayPeriod { get; set; }

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
                                                Paystub currentPaystub,
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

                PaystubModel paystubModel = new PaystubModel();
                paystubModel.Employee = employee;

                if (salary != null)
                    paystubModel = ComputeHoursAndPay(paystubModel, employee, salary, branchTimeEntries);

                if (currentPaystub != null)
                {
                    // Check the percentage of work hours the employee worked in this branch
                    // If employee worked for 100 hours in total, and he worked 40 hours in this branch,
                    // then he worked 40% of his total worked hours in this branch.
                    var workedPercentage = AccuMath.CommercialRound(paystubModel.RegularHours / currentPaystub.RegularHours); // 40 / 100
                    paystubModel = ComputeGovernmentDeductions(hmoLoan,
                                                                monthlyDeduction,
                                                                paystubModel,
                                                                workedPercentage);
                }

                paystubModel.TotalAllowance = ComputeTotalAllowance(dailyAllowances,
                                                                    settings,
                                                                    calendarCollection,
                                                                    userId,
                                                                    currentPaystub,
                                                                    paystubModel.Employee,
                                                                    payPeriod,
                                                                    allTimeEntries: allTimeEntries,
                                                                    branchTimeEntries: branchTimeEntries);

                paystubModel.ThirteenthMonthPay = ThirteenthMonthPayCalculator.
                                                    ComputeByRegularPayAndAllowanceDaily(employee,
                                                                                        branchTimeEntries,
                                                                                        branchActualTimeEntries);
                return paystubModel;
            }

            private static decimal ComputeTotalAllowance(List<Allowance> dailyAllowances,
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
                                                        settings,
                                                        calendarCollection,
                                                        allTimeEntries,
                                                        organizationId: employee.OrganizationID.Value,
                                                        userId: userId);

                decimal totalAllowance = 0;

                foreach (var allowance in dailyAllowances)
                {
                    var allowanceItem = allowanceCalculator.Compute(payPeriod,
                                                                    allowance,
                                                                    employee,
                                                                    currentPaystub,
                                                                    branchTimeEntries);

                    if (allowanceItem != null)
                    {
                        totalAllowance += allowanceItem.Amount;
                    }
                }

                return totalAllowance;
            }

            private static PaystubModel ComputeGovernmentDeductions(LoanTransaction hmoLoan,
                                                                    MonthlyDeduction monthlyDeduction,
                                                                    PaystubModel paystubModel,
                                                                    decimal workedPercentage)
            {
                paystubModel.HMOAmount = MonthlyDeductionAmount.
                                            ComputeBranchPercentage(hmoLoan?.Amount ?? 0, workedPercentage);

                paystubModel.SSSAmount = monthlyDeduction.SSSAmount.
                                            GetBranchPercentage(workedPercentage);

                paystubModel.ECAmount = monthlyDeduction.ECAmount.
                                            GetBranchPercentage(workedPercentage);

                paystubModel.HDMFAmount = monthlyDeduction.HDMFAmount.
                                            GetBranchPercentage(workedPercentage);

                paystubModel.PhilHealthAmount = monthlyDeduction.PhilHealthAmount.
                                            GetBranchPercentage(workedPercentage);

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
            private decimal TotalAllowance { get; set; }

            public decimal GrossPay => AccuMath.CommercialRound(RegularPay +
                                                    OvertimePay +
                                                    NightDiffPay +
                                                    NightDiffOvertimePay +
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
                    var vacationLeavePerYearInDays = Employee.VacationLeaveAllowance / PayrollTools.WorkHoursPerDay;
                    var daysPerMonths = Employee.WorkDaysPerYear / PayrollTools.MonthsPerYear;
                    var daysPerCutoff = daysPerMonths / PayrollTools.SemiMonthlyPayPeriodsPerMonth;

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
            public static MonthlyDeduction Create(bool divideTheDeductions,
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
                    SSSAmount = MonthlyDeductionAmount.Create(divideTheDeductions, sssAmount),
                    ECAmount = MonthlyDeductionAmount.Create(divideTheDeductions, ecAmount),
                    HDMFAmount = MonthlyDeductionAmount.Create(divideTheDeductions, hdmfAmount),
                    PhilHealthAmount = MonthlyDeductionAmount.Create(divideTheDeductions, philhealthAmount),
                    ThirteenthMonthPay = MonthlyDeductionAmount.Create(divideTheDeductions, thirteenthMonthPay)
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
            private readonly bool _divideTheDeductions;

            public static MonthlyDeductionAmount Create(bool divideTheDeductions, decimal amount)
            {
                return new MonthlyDeductionAmount(divideTheDeductions, amount);
            }

            private MonthlyDeductionAmount(bool divideTheDeductions, decimal amount)
            {
                MonthlyAmount = AccuMath.CommercialRound(amount);
                _divideTheDeductions = divideTheDeductions;
            }

            public decimal MonthlyAmount { get; }

            public decimal SemiMonthlyAmount => AccuMath.CommercialRound(MonthlyAmount / 2);

            public decimal Amount => MonthlyAmount;

            public decimal GetBranchPercentage(decimal branchPercentage) =>
                     ComputeBranchPercentage(_divideTheDeductions ? SemiMonthlyAmount : MonthlyAmount
                                            , branchPercentage);

            public static decimal ComputeBranchPercentage(decimal amount, decimal branchPercentage) =>
                                    AccuMath.CommercialRound(amount * branchPercentage);
        }

        #endregion Custom Classes
    }
}