using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class PayrollGenerator : IPayrollGenerator
    {
        private readonly IPaystubDataService _paystubDataService;

        //private static ILog logger = LogManager.GetLogger("PayrollLogger");

        public PayrollGenerator(IPaystubDataService paystubDataService)
        {
            _paystubDataService = paystubDataService;
        }

        public async Task<PaystubEmployeeResult> Start(
            int employeeId,
            IPayrollResources resources,
            int organizationId,
            int currentlyLoggedInUserId)
        {
            // we use the employee data from resources.Employees instead of just passing the employee
            // entity in the Start method because we can be sure that the data in resources.Employees
            // are complete employee data (ex. with Position, Divisition) that are needed by PayrollGenerator.
            var employee = resources.Employees.Where(x => x.RowID == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new Exception("Employee was not loaded.");
            }

            var currentSystemOwner = resources.CurrentSystemOwner;

            var settings = resources.ListOfValueCollection;

            var payPeriod = resources.PayPeriod;

            var bpiInsuranceProduct = resources.BpiInsuranceProduct;

            var sickLeaveProduct = resources.SickLeaveProduct;

            var vacationLeaveProduct = resources.VacationLeaveProduct;

            var singleParentLeaveProduct = resources.SingleParentLeaveProduct;

            var calendarCollection = resources.CalendarCollection;

            var salary = resources.Salaries.FirstOrDefault(s => s.EmployeeID == employee.RowID);

            var paystub = resources.Paystubs.FirstOrDefault(p => p.EmployeeID == employee.RowID);

            var previousPaystub = resources.PreviousPaystubs.FirstOrDefault(p => p.EmployeeID == employee.RowID);

            var loans = resources.Loans
                .Where(l => l.EmployeeID == employee.RowID)
                .ToList();

            var previousTimeEntries = resources.TimeEntries
                .Where(t => t.EmployeeID == employee.RowID)
                .ToList();

            var timeEntries = resources.TimeEntries
                .Where(t => t.EmployeeID == employee.RowID)
                .Where(t => payPeriod.PayFromDate <= t.Date)
                .Where(t => t.Date <= payPeriod.PayToDate)
                .OrderBy(t => t.Date)
                .ToList();

            var actualTimeEntries = resources.ActualTimeEntries
                .Where(t => t.EmployeeID == employee.RowID)
                .ToList();

            var allowances = resources.Allowances
                .Where(a => a.EmployeeID == employee.RowID)
                .ToList();

            var bonuses = resources.Bonuses
                .Where(a => a.EmployeeID == employee.RowID)
                .ToList();

            var leaves = resources.Leaves
                .Where(a => a.EmployeeID == employee.RowID)
                .ToList();

            var shifts = resources.Shifts
                .Where(a => a.EmployeeID == employee.RowID)
                .ToList();

            var allowanceSalaryTimeEntries = resources.AllowanceSalaryTimeEntries
                .Where(t => t.EmployeeID == employee.RowID)
                .ToList();

            try
            {
                var result = await GeneratePayStub(
                    resources,
                    organizationId: organizationId,
                    currentlyLoggedInUserId: currentlyLoggedInUserId,
                    currentSystemOwner,
                    settings,
                    calendarCollection,
                    payPeriod,
                    paystub: paystub,
                    employee: employee,
                    salary: salary,
                    previousPaystub: previousPaystub,
                    bpiInsuranceProduct: bpiInsuranceProduct,
                    sickLeaveProduct: sickLeaveProduct,
                    vacationLeaveProduct: vacationLeaveProduct,
                    singleParentLeaveProduct: singleParentLeaveProduct,
                    loans: loans,
                    previousTimeEntries: previousTimeEntries,
                    timeEntries: timeEntries,
                    actualTimeEntries: actualTimeEntries,
                    allowances: allowances,
                    leaves: leaves,
                    bonuses: bonuses,
                    shifts: shifts,
                    policy: resources.Policy,
                    allowanceSalaryTimeEntries: allowanceSalaryTimeEntries);

                return result;
            }
            catch (PayrollException ex)
            {
                return PaystubEmployeeResult.Error(employee, ex.Message);
            }
            catch (Exception ex)
            {
                //logger.Error("DoProcess", ex);
                return PaystubEmployeeResult.Error(employee, $"Failure to generate paystub for employee {employee.EmployeeNo} {ex.Message}.");
            }
        }

        private async Task<PaystubEmployeeResult> GeneratePayStub(
            IPayrollResources resources,
            int organizationId,
            int currentlyLoggedInUserId,
            string currentSystemOwner,
            ListOfValueCollection settings,
            CalendarCollection calendarCollection,
            PayPeriod payPeriod,
            Paystub paystub,
            Employee employee,
            Salary salary,
            Paystub previousPaystub,
            Product bpiInsuranceProduct,
            Product sickLeaveProduct,
            Product vacationLeaveProduct,
            Product singleParentLeaveProduct,
            IReadOnlyCollection<Loan> loans,
            IReadOnlyCollection<TimeEntry> previousTimeEntries,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<ActualTimeEntry> actualTimeEntries,
            IReadOnlyCollection<Allowance> allowances,
            IReadOnlyCollection<Leave> leaves,
            IReadOnlyCollection<Bonus> bonuses,
            IPolicyHelper policy,
            IReadOnlyCollection<Shift> shifts,
            IReadOnlyCollection<AllowanceSalaryTimeEntry> allowanceSalaryTimeEntries = null)
        {
            if (salary == null)
                return PaystubEmployeeResult.Error(employee, "Employee has no salary for this cutoff.");

            if ((!timeEntries.Any()) && (employee.IsDaily || employee.IsMonthly))
                return PaystubEmployeeResult.Error(employee, "No time entries.");

            if (employee.Position == null)
                return PaystubEmployeeResult.Error(employee, "Employee has no job position set.");

            if (paystub == null)
            {
                paystub = new Paystub()
                {
                    OrganizationID = organizationId,
                    Created = DateTime.Now,
                    CreatedBy = currentlyLoggedInUserId,
                    LastUpdBy = currentlyLoggedInUserId,
                    EmployeeID = employee.RowID,
                    PayPeriodID = payPeriod.RowID,
                    PayFromDate = payPeriod.PayFromDate,
                    PayToDate = payPeriod.PayToDate
                };
            }

            if (paystub.Actual == null)
            {
                paystub.Actual = new PaystubActual()
                {
                    OrganizationID = organizationId,
                    EmployeeID = employee.RowID,
                    PayPeriodID = payPeriod.RowID,
                    PayFromDate = payPeriod.PayFromDate,
                    PayToDate = payPeriod.PayToDate
                };
            }

            ResetLoans(loans, paystub, currentlyLoggedInUserId);

            var allowanceItems = paystub.CreateAllowanceItems(
                currentlyLoggedInUserId,
                settings,
                calendarCollection,
                payPeriod,
                employee,
                previousTimeEntries: previousTimeEntries,
                timeEntries: timeEntries,
                allowances,
                shifts: shifts);

            var loanTransactions = paystub.CreateLoanTransactions(
                payPeriod,
                loans,
                bonuses: bonuses,
                policy: policy,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            paystub.ComputePayroll(
                resources,
                currentlyLoggedInUserId: currentlyLoggedInUserId,
                currentSystemOwner,
                settings,
                calendarCollection,
                payPeriod,
                employee: employee,
                salary: salary,
                previousPaystub: previousPaystub,
                loanTransactions: loanTransactions,
                timeEntries: timeEntries,
                actualTimeEntries: actualTimeEntries,
                allowanceItems: allowanceItems.ToList(),
                bonuses: bonuses,
                allowanceSalaryTimeEntries: allowanceSalaryTimeEntries);

            await _paystubDataService.SaveAsync(
                currentlyLoggedInUserId: currentlyLoggedInUserId,
                currentSystemOwner,
                policy,
                payPeriod,
                paystub,
                employee,
                bpiInsuranceProduct: bpiInsuranceProduct,
                sickLeaveProduct: sickLeaveProduct,
                vacationLeaveProduct: vacationLeaveProduct,
                singleParentLeaveProduct: singleParentLeaveProduct,
                loans: loans,
                allowanceItems: allowanceItems,
                loanTransactions: loanTransactions,
                timeEntries: timeEntries,
                leaves: leaves,
                bonuses: bonuses);

            return PaystubEmployeeResult.Success(employee, paystub);
        }

        private void ResetLoans(IReadOnlyCollection<Loan> loans, Paystub paystub, int currentlyLoggedInUserId)
        {
            if (paystub?.LoanTransactions == null || paystub.LoanTransactions.Count == 0) return;

            foreach (var loan in loans)
            {
                var loanTransactions = paystub.LoanTransactions.Where(x => x.LoanID == loan.RowID);

                if (loanTransactions.Any())
                {
                    loan.RetractLoanTransactions(loanTransactions);
                    loan.LastUpdBy = currentlyLoggedInUserId;
                }
            }
        }
    }
}
