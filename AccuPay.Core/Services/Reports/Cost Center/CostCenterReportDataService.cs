using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class CostCenterReportDataService
    {
        private Branch _selectedBranch;
        private int _userId;
        private ICostCenterReportResources _resources;

        public List<PayPeriodModel> GetData(
            ICostCenterReportResources resources,
            Branch selectedBranch,
            int userId,
            bool isActual)
        {
            _resources = resources;
            _selectedBranch = selectedBranch;
            _userId = userId;

            if (_selectedBranch?.RowID == null)
                throw new Exception("Branch does not exists.");

            List<PayPeriodModel> payPeriodModels = new List<PayPeriodModel>();

            var dateForCheckingLastWorkingDay = PayrollTools.
                GetPreviousCutoffDateForCheckingLastWorkingDay(resources.ReportPeriod.Start);

            var allTimeEntries = resources.TimeEntries;

            var payPeriodTimeEntries = allTimeEntries
                .Where(t => t.Date >= resources.ReportPeriod.Start)
                .Where(t => t.Date <= resources.ReportPeriod.End)
                .ToList();

            // Get all the employee in the branch
            // Also get the employees that has at least 1 timelogs on the branch
            List<Employee> employees = GetEmployeeFromSelectedBranch(
                resources.Employees,
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

            var branchActualTimeEntries = GetBranchActualTimeEntries(branchTimeEntries);

            var salaries = resources.Salaries
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToList();

            var employeePaystubs = resources.Paystubs
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToList();

            var employeeMonthlyDeductions = GenerateMonthlyDeductionList(employeeIds, employeePaystubs, resources.SocialSecurityBrackets);

            var hmoLoans = resources.HmoLoans;

            hmoLoans = hmoLoans
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToList();

            var dailyAllowances = _resources.DailyAllowances.Where(x => employeeIds.Contains(x.EmployeeID.Value)).ToList();

            payPeriodModels = CreatePayPeriodModels(
                isActual,
                resources.ReportTimePeriods,
                employees,
                salaries,
                employeePaystubs,
                employeeMonthlyDeductions,
                hmoLoans,
                dailyAllowances,
                resources.PayPeriods,
                allTimeEntries: allTimeEntries,
                branchTimeEntries: branchTimeEntries,
                branchActualTimeEntries: branchActualTimeEntries,
                selectedBranch: _selectedBranch);

            return payPeriodModels;
        }

        private List<ActualTimeEntry> GetBranchActualTimeEntries(List<TimeEntry> branchTimeEntries)
        {
            var firstDate = branchTimeEntries.OrderBy(x => x.Date).FirstOrDefault()?.Date;
            var lastDate = branchTimeEntries.OrderBy(x => x.Date).LastOrDefault()?.Date;

            if (firstDate == null || lastDate == null)
            {
                return new List<ActualTimeEntry>();
            }

            var actualTimeEntries = _resources.ActualTimeEntries
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

        private List<Employee> GetEmployeeFromSelectedBranch(
            List<Employee> dailyEmployees,
            List<TimeEntry> timeEntries,
            int branchId)
        {
            var employeeIdsWithTimeEntriesInBranch = timeEntries
                .Where(t => t.BranchID != null)
                .Where(t => t.BranchID == branchId)
                .GroupBy(t => t.EmployeeID)
                .Select(t => t.Key)
                .ToArray();

            var employeesWithTimeEntriesInBranch = dailyEmployees
                .Where(e => employeeIdsWithTimeEntriesInBranch.Contains(e.RowID.Value))
                .ToList();

            var employeesFromBranch = dailyEmployees
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

        private List<MonthlyDeduction> GenerateMonthlyDeductionList(int[] employeeIds, List<Paystub> allPaystubs, List<SocialSecurityBracket> sssBrackets)
        {
            List<MonthlyDeduction> employeeMonthlyDeductions = new List<MonthlyDeduction>();

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
            var ecAmount = sssBracket?.EmployerECAmount ?? 0;

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
            List<PayPeriod> allPayPeriods,
            List<TimeEntry> allTimeEntries,
            List<TimeEntry> branchTimeEntries,
            List<ActualTimeEntry> branchActualTimeEntries,
            Branch selectedBranch)
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
                    allPayPeriods,
                    allTimeEntries: allTimeEntries,
                    branchTimeEntries: branchTimeEntries,
                    branchActualTimeEntries: branchActualTimeEntries);

                payPeriodModels.Add(new PayPeriodModel(payPeriod, selectedBranch, paystubModels));
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
                    .Where(s => s.EffectiveFrom <= payPeriod.End)
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
                    _resources.Settings,
                    _resources.CalendarCollection,
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
            public TimePeriod PayPeriod { get; }
            public Branch Branch { get; }

            public List<CostCenterPaystubModel> Paystubs { get; set; }

            public PayPeriodModel(TimePeriod payPeriod, Branch branch, List<CostCenterPaystubModel> paystubs)
            {
                PayPeriod = payPeriod;
                Branch = branch;
                Paystubs = paystubs;
            }
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
