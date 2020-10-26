using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class CostCenterReportDataService
    {
        private readonly CalendarService _calendarService;
        private readonly ListOfValueService _listOfValueService;
        private readonly AllowanceRepository _allowanceRepository;
        private readonly PayrollContext _context;

        private DateTime _selectedMonth;
        private Branch _selectedBranch;
        private int _userId;

        public CostCenterReportDataService(
            CalendarService calendarService,
            ListOfValueService listOfValueService,
            AllowanceRepository allowanceRepository,
            PayrollContext context)
        {
            _calendarService = calendarService;
            _listOfValueService = listOfValueService;
            _allowanceRepository = allowanceRepository;
            _context = context;
        }

        public List<PayPeriodModel> GetData(
            DateTime selectedMonth,
            Branch selectedBranch,
            int userId,
            bool isActual)
        {
            _selectedMonth = selectedMonth;
            _selectedBranch = selectedBranch;
            _userId = userId;

            if (_selectedBranch?.RowID == null)
                throw new Exception("Branch does not exists.");

            List<PayPeriodModel> payPeriodModels = new List<PayPeriodModel>();

            var payPeriods = GetPayPeriod(_context);
            DateTime startDate = new DateTime[] { payPeriods[0].Start, payPeriods[1].Start }.Min();
            DateTime endDate = new DateTime[] { payPeriods[0].End, payPeriods[1].End }.Max();

            var reportPeriod = new TimePeriod(startDate, endDate);

            var dateForCheckingLastWorkingDay = PayrollTools.
                GetPreviousCutoffDateForCheckingLastWorkingDay(reportPeriod.Start);

            var allTimeEntries = _context.TimeEntries
                .Include(t => t.Employee)
                .Where(t => t.Date >= dateForCheckingLastWorkingDay)
                .Where(t => t.Date <= reportPeriod.End)
                .ToList();

            var payPeriodTimeEntries = allTimeEntries
                .Where(t => t.Date >= reportPeriod.Start)
                .Where(t => t.Date <= reportPeriod.End)
                .ToList();

            // Get all the employee in the branch
            // Also get the employees that has at least 1 timelogs on the branch
            List<Employee> employees = GetEmployeeFromSelectedBranch(
                _context,
                payPeriodTimeEntries,
                _selectedBranch.RowID.Value);

            var employeeIds = employees
                .GroupBy(x => x.RowID.Value)
                .Select(x => x.Key)
                .ToArray();
            var organizationids = employees
                .GroupBy(x => x.OrganizationID.Value)
                .Select(x => x.Key)
                .ToArray();

            // if timeEntry's BranchID is Nothing, set it to
            // employee's BranchID for easier querying
            AddBranchToTimeEntries(payPeriodTimeEntries, employees);

            var branchTimeEntries = payPeriodTimeEntries
                .Where(t => t.BranchID != null)
                .Where(t => t.BranchID == _selectedBranch.RowID)
                .ToList();

            var branchActualTimeEntries = GetBranchActualTimeEntries(branchTimeEntries, _context);

            var salaries = _context.Salaries
                .Where(s => s.EffectiveFrom <= reportPeriod.Start)
                .ToList();

            salaries = salaries
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToList();

            var employeePaystubs = _context.Paystubs
                .Include(p => p.ThirteenthMonthPay)
                .Include(p => p.PayPeriod)
                .Where(p => p.PayPeriod.PayFromDate >= reportPeriod.Start)
                .Where(p => p.PayPeriod.PayToDate <= reportPeriod.End)
                .ToList();

            employeePaystubs = employeePaystubs
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToList();

            var employeeMonthlyDeductions = GenerateMonthlyDeductionList(employeeIds, employeePaystubs);

            var hmoLoans = _context.LoanTransactions
                .Include(l => l.Paystub)
                .Include(p => p.PayPeriod)
                .Include(p => p.LoanSchedule.LoanType)
                .Where(p => p.PayPeriod.PayFromDate >= reportPeriod.Start)
                .Where(p => p.PayPeriod.PayToDate <= reportPeriod.End)
                .Where(p => p.LoanSchedule.LoanType.Name == ProductConstant.HMO_LOAN)
                .ToList();

            hmoLoans = hmoLoans
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToList();

            var settings = _listOfValueService.Create();

            var calendarCollection = _calendarService.GetCalendarCollection(reportPeriod);

            var dailyAllowances = GetDailyAllowances(organizationids, reportPeriod, employeeIds);

            var allPayPeriods = _context.PayPeriods
                .Where(x => organizationids.Contains(x.OrganizationID.Value))
                .Where(x => x.Year == _selectedMonth.Year)
                .Where(x => x.Month == _selectedMonth.Month)
                .ToList();
            payPeriodModels = CreatePayPeriodModels(
                isActual,
                payPeriods,
                employees,
                salaries,
                employeePaystubs,
                employeeMonthlyDeductions,
                hmoLoans,
                dailyAllowances,
                calendarCollection,
                settings,
                allPayPeriods,
                allTimeEntries: allTimeEntries,
                branchTimeEntries: branchTimeEntries,
                branchActualTimeEntries: branchActualTimeEntries);

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

            var actualTimeEntries = context.ActualTimeEntries
                .Where(x => x.Date >= firstDate)
                .Where(x => x.Date <= lastDate)
                .ToList();

            var branchActualTimeEntries = new List<ActualTimeEntry>();
            foreach (var actualTimeEntry in actualTimeEntries)
            {
                if (branchTimeEntries
                        .Where(x => x.Date == actualTimeEntry.Date)
                        .Where(x => x.EmployeeID == actualTimeEntry.EmployeeID)
                        .Any())
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
            var payPeriods = context.PayPeriods
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.IsSemiMonthly)
                .Where(p => p.Year == _selectedMonth.Year)
                .Where(p => p.Month == _selectedMonth.Month)
                .ToList();

            if (payPeriods.Count != 2)
                throw new Exception($"Pay periods on the selected month was {payPeriods.Count} instead of 2 (First half, End of the month)");

            return new List<TimePeriod>()
            {
                new TimePeriod(payPeriods[0].PayFromDate, payPeriods[0].PayToDate),
                new TimePeriod(payPeriods[1].PayFromDate, payPeriods[1].PayToDate)
            };
        }

        private List<Employee> GetEmployeeFromSelectedBranch(
            PayrollContext context,
            List<TimeEntry> timeEntries,
            int branchId)
        {
            var employeeIdsWithTimeEntriesInBranch = timeEntries
                .Where(t => t.BranchID != null)
                .Where(t => t.BranchID == branchId)
                .GroupBy(t => t.EmployeeID)
                .Select(t => t.Key)
                .ToArray();

            var employeesWithTimeEntriesInBranch = context.Employees
                .Where(e => e.IsDaily)
                .Where(e => employeeIdsWithTimeEntriesInBranch.Contains(e.RowID.Value))
                .ToList();

            var employeesFromBranch = context.Employees
                .Where(e => e.IsDaily)
                .Where(e => e.BranchID != null)
                .Where(e => e.BranchID == _selectedBranch.RowID)
                .ToList();

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
            var dailyAllowances = new List<Allowance>();
            foreach (var organizationId in organizationids)
            {
                var allowances = _allowanceRepository.GetByPayPeriodWithProduct(
                        organizationId: organizationId,
                        timePeriod: reportPeriod)
                    .ToList();

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

            var taxEffectivityDate = new DateTime(_selectedMonth.Year, _selectedMonth.Month, 1);
            sssBrackets = _context.SocialSecurityBrackets
                .Where(s => taxEffectivityDate >= s.EffectiveDateFrom)
                .Where(s => taxEffectivityDate <= s.EffectiveDateTo)
                .ToList();

            foreach (var employeeId in employeeIds)
            {
                var employeePaystubs = allPaystubs
                    .Where(p => p.EmployeeID == employeeId)
                    .ToList();

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

                    var sssPayables = GetEmployerSSSPayables(
                        sssBrackets,
                        employeePaystubs[0].SssEmployeeShare);

                    sssAmount = sssPayables.EmployerShare;
                    ecAmount = sssPayables.ECamount;

                    // check if there is a 2nd paystub (could be only 1 paystub for the month)
                    if (employeePaystubs.Count == 2)
                    {
                        sssPayables = GetEmployerSSSPayables(
                            sssBrackets,
                            employeePaystubs[1].SssEmployeeShare);

                        sssAmount += sssPayables.EmployerShare;
                        ecAmount += sssPayables.ECamount;
                    }
                }

                employeeMonthlyDeductions.Add(
                    MonthlyDeduction.Create(
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
            var sssBracket = sssBrackets
                .Where(s => s.EmployeeContributionAmount == employeeShare)
                .FirstOrDefault();

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

                timeEntry.BranchID = employees.FirstOrDefault(e => e.RowID == timeEntry.EmployeeID)?.BranchID;
            }
        }

        private List<PayPeriodModel> CreatePayPeriodModels(
            bool isActual,
            List<TimePeriod> payPeriods,
            List<Employee> employees,
            List<Salary> salaries,
            List<Paystub> paystubs,
            List<MonthlyDeduction> monthlyDeductions,
            List<LoanTransaction> hmoLoans,
            List<Allowance> dailyAllowances,
            CalendarCollection calendarCollection,
            ListOfValueCollection settings,
            List<PayPeriod> allPayPeriods,
            List<TimeEntry> allTimeEntries,
            List<TimeEntry> branchTimeEntries,
            List<ActualTimeEntry> branchActualTimeEntries)
        {
            List<PayPeriodModel> payPeriodModels = new List<PayPeriodModel>();
            foreach (var payPeriod in payPeriods)
            {
                var payPeriodPaystubs = paystubs
                    .Where(p => p.PayPeriod.PayFromDate >= payPeriod.Start)
                    .Where(p => p.PayPeriod.PayToDate <= payPeriod.End)
                    .ToList();

                var payPeriodHmoLoans = hmoLoans
                    .Where(p => p.PayPeriod.PayFromDate >= payPeriod.Start)
                    .Where(p => p.PayPeriod.PayToDate <= payPeriod.End)
                    .ToList();

                var paystubModels = CreatePaystubModels(
                    isActual,
                    employees,
                    salaries,
                    payPeriod,
                    payPeriodPaystubs: payPeriodPaystubs,
                    allPaystubs: paystubs,
                    monthlyDeductions,
                    payPeriodHmoLoans,
                    dailyAllowances,
                    calendarCollection,
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

        private List<CostCenterPaystubModel> CreatePaystubModels(
            bool isActual,
            List<Employee> employees,
            List<Salary> salaries,
            TimePeriod payPeriod,
            List<Paystub> payPeriodPaystubs,
            List<Paystub> allPaystubs,
            List<MonthlyDeduction> monthlyDeductions,
            List<LoanTransaction> hmoLoans,
            List<Allowance> dailyAllowances,
            CalendarCollection calendarCollection,
            ListOfValueCollection settings,
            List<PayPeriod> payPeriods,
            List<TimeEntry> allTimeEntries,
            List<TimeEntry> branchTimeEntries,
            List<ActualTimeEntry> branchActualTimeEntries)
        {
            List<CostCenterPaystubModel> paystubModels = new List<CostCenterPaystubModel>();

            foreach (var employee in employees)
            {
                var earliestAllTimeEntryDate = allTimeEntries
                    .Select(x => x.Date)
                    .OrderBy(x => x.Date)
                    .FirstOrDefault();
                var employeeAllTimeEntries = allTimeEntries
                    .Where(t => t.Date >= earliestAllTimeEntryDate)
                    .Where(t => t.Date <= payPeriod.End)
                    .Where(t => t.EmployeeID == employee.RowID)
                    .ToList();

                var employeeBranchTimeEntries = branchTimeEntries
                    .Where(t => t.Date >= payPeriod.Start)
                    .Where(t => t.Date <= payPeriod.End)
                    .Where(t => t.EmployeeID == employee.RowID)
                    .ToList();

                var employeeBranchActualTimeEntries = branchActualTimeEntries
                    .Where(t => t.Date >= payPeriod.Start)
                    .Where(t => t.Date <= payPeriod.End)
                    .Where(t => t.EmployeeID == employee.RowID)
                    .ToList();

                var employeeAllowances = dailyAllowances
                    .Where(x => x.EmployeeID == employee.RowID)
                    .ToList();

                var salary = salaries
                    .Where(s => s.EmployeeID == employee.RowID)
                    .Where(s => s.EffectiveFrom <= payPeriod.Start)
                    .OrderByDescending(s => s.EffectiveFrom)
                    .FirstOrDefault();

                var currentPaystub = payPeriodPaystubs
                    .Where(p => p.EmployeeID == employee.RowID)
                    .FirstOrDefault();

                var monthlyPaystubs = allPaystubs
                    .Where(d => d.EmployeeID == employee.RowID)
                    .ToList();

                var monthlyDeduction = monthlyDeductions
                    .Where(d => d.EmployeeID == employee.RowID)
                    .FirstOrDefault();

                var hmoLoan = hmoLoans
                    .Where(h => h.EmployeeID == employee.RowID)
                    .FirstOrDefault();

                var organizationId = employee.OrganizationID.Value;

                var currentPayPeriod = payPeriods
                    .Where(x => x.OrganizationID == organizationId)
                    .Where(x => x.PayFromDate == payPeriod.Start)
                    .Where(x => x.PayToDate == payPeriod.End)
                    .FirstOrDefault();

                var createdPaystubModel = CostCenterPaystubModel.Create(
                    employee,
                    salary,
                    currentPaystub,
                    monthlyPaystubs,
                    isActual,
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

            public List<CostCenterPaystubModel> Paystubs { get; set; }
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

        #endregion Custom Classes
    }
}