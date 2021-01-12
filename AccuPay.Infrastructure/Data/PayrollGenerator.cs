using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public partial class PayrollGenerator : IPayrollGenerator
    {
        private readonly PayrollContext _context;
        private readonly IPaystubDataService _paystubDataService;

        //private static ILog logger = LogManager.GetLogger("PayrollLogger");

        public PayrollGenerator(PayrollContext context, IPaystubDataService paystubDataService)
        {
            _context = context;
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
                    loans: loans,
                    previousTimeEntries: previousTimeEntries,
                    timeEntries: timeEntries,
                    actualTimeEntries: actualTimeEntries,
                    allowances: allowances,
                    leaves: leaves,
                    bonuses: bonuses,
                    policy: resources.Policy);

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
            IReadOnlyCollection<Loan> loans,
            IReadOnlyCollection<TimeEntry> previousTimeEntries,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<ActualTimeEntry> actualTimeEntries,
            IReadOnlyCollection<Allowance> allowances,
            IReadOnlyCollection<Leave> leaves,
            IReadOnlyCollection<Bonus> bonuses,
            IPolicyHelper policy)
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

            var allowanceItems = CreateAllowanceItems(
                currentlyLoggedInUserId,
                settings,
                calendarCollection,
                payPeriod,
                paystub,
                employee,
                previousTimeEntries: previousTimeEntries,
                timeEntries: timeEntries,
                allowances);

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
                bonuses: bonuses);

            await SavePayroll(
                currentlyLoggedInUserId: currentlyLoggedInUserId,
                currentSystemOwner,
                settings,
                policy,
                payPeriod,
                paystub,
                employee,
                bpiInsuranceProduct: bpiInsuranceProduct,
                sickLeaveProduct: sickLeaveProduct,
                vacationLeaveProduct: vacationLeaveProduct,
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

        public async Task SavePayroll(
            int currentlyLoggedInUserId,
            string currentSystemOwner,
            ListOfValueCollection settings,
            IPolicyHelper policy,
            PayPeriod payPeriod,
            Paystub paystub,
            Employee employee,
            Product bpiInsuranceProduct,
            Product sickLeaveProduct,
            Product vacationLeaveProduct,
            IReadOnlyCollection<Loan> loans,
            ICollection<AllowanceItem> allowanceItems,
            ICollection<LoanTransaction> loanTransactions,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<Leave> leaves,
            IReadOnlyCollection<Bonus> bonuses)
        {
            foreach (var loan in loans)
            {
                loan.LastUpdBy = currentlyLoggedInUserId;
                _context.Entry(loan).State = EntityState.Modified;

                SaveLoanPaymentFromBonusItems(
                    payPeriod: payPeriod,
                    paystub: paystub,
                    loan: loan,
                    bonuses: bonuses,
                    useLoanDeductFromBonus: policy.UseLoanDeductFromBonus);

                SaveYearlyLoanInterest(policy, loan);
            }

            bool isNew = true;
            if (paystub.RowID.HasValue)
            {
                isNew = false;

                paystub.LastUpdBy = currentlyLoggedInUserId;
                _context.Entry(paystub).State = EntityState.Modified;
                _context.Entry(paystub.Actual).State = EntityState.Modified;

                if (paystub.ThirteenthMonthPay != null)
                {
                    _context.Entry(paystub.ThirteenthMonthPay).State = EntityState.Modified;
                }
            }
            else
            {
                _context.Paystubs.Add(paystub);
            }

            bool isEligibleForBPIInsurance = employee.IsEligibleForNewBPIInsurance(
                useBPIInsurancePolicy: settings.GetBoolean("Employee Policy.UseBPIInsurance", false),
                paystub.Adjustments,
                payPeriod);

            if (isEligibleForBPIInsurance)
            {
                _context.Adjustments.Add(new Adjustment()
                {
                    OrganizationID = paystub.OrganizationID,
                    CreatedBy = currentlyLoggedInUserId,
                    Paystub = paystub,
                    ProductID = bpiInsuranceProduct.RowID,
                    Amount = -employee.BPIInsurance
                });
            }

            if (paystub.AllowanceItems != null)
            {
                _context.AllowanceItems.RemoveRange(paystub.AllowanceItems);
            }
            paystub.AllowanceItems = allowanceItems;

            if (paystub.LoanTransactions != null)
            {
                _context.LoanTransactions.RemoveRange(paystub.LoanTransactions);
            }
            paystub.LoanTransactions = loanTransactions;

            if (currentSystemOwner != SystemOwner.Benchmark)
            {
                await UpdateLeaveLedger(paystub, employee, payPeriod, timeEntries, leaves);

                await UpdatePaystubItems(
                    currentlyLoggedInUserId,
                    paystub,
                    employee,
                    sickLeaveProduct: sickLeaveProduct,
                    vacationLeaveProduct: vacationLeaveProduct,
                    timeEntries);
            }
            else
            {
                await UpdateBenchmarkLeaveLedger(paystub, employee, payPeriod);
            }

            // TODO: move this to a repository and data service, then move the useractivity to that data service
            await _context.SaveChangesAsync();

            if (isNew)
            {
                await _paystubDataService.RecordCreate(currentlyLoggedInUserId, paystub, payPeriod);
            }
            else
            {
                await _paystubDataService.RecordEdit(currentlyLoggedInUserId, paystub, payPeriod);
            }
        }

        private void SaveYearlyLoanInterest(IPolicyHelper policy, Loan loan)
        {
            if (policy.UseGoldwingsLoanInterest && loan.YearlyLoanInterests != null && loan.YearlyLoanInterests.Any())
            {
                loan.YearlyLoanInterests.ToList().ForEach(yearlyLoanInterest =>
                {
                    // Save new yearly loan interest.
                    // Don't save updated yearlyLoanInterests since they should not be edited after it was created.
                    if (yearlyLoanInterest.IsNewEntity)
                    {
                        _context.Entry(yearlyLoanInterest).State = EntityState.Added;
                    }
                });
            }
        }

        private void SaveLoanPaymentFromBonusItems(
            PayPeriod payPeriod,
            Paystub paystub,
            Loan loan,
            IReadOnlyCollection<Bonus> bonuses,
            bool useLoanDeductFromBonus)
        {
            if (useLoanDeductFromBonus)
            {
                var loanPaymentFromBonuses = paystub.GetLoanPaymentFromBonuses(bonuses, loan, payPeriod: payPeriod);

                foreach (var lb in loanPaymentFromBonuses)
                {
                    var existingItem = lb.Items
                        .Where(i => i.LoanPaidBonusId == lb.Id)
                        .Where(i => i.PaystubId == paystub.RowID)
                        .FirstOrDefault();

                    if (existingItem != null) continue;

                    var lbItem = LoanPaymentFromBonusItem.CreateNew(lb.Id, paystub);

                    lb.Items.Add(lbItem);

                    _context.Entry(lbItem).State = EntityState.Added;
                }
            }
        }

        private ICollection<AllowanceItem> CreateAllowanceItems(
            int currentlyLoggedInUserId,
            ListOfValueCollection settings,
            CalendarCollection calendarCollection,
            PayPeriod payPeriod,
            Paystub paystub,
            Employee employee,
            IReadOnlyCollection<TimeEntry> previousTimeEntries,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<Allowance> allowances)
        {
            var dailyCalculator = new DailyAllowanceCalculator(
                settings,
                calendarCollection,
                previousTimeEntries,
                organizationId: paystub.OrganizationID.Value,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            var semiMonthlyCalculator = new SemiMonthlyAllowanceCalculator(
                new AllowancePolicy(settings),
                employee,
                paystub,
                payPeriod,
                calendarCollection,
                timeEntries,
                organizationId: paystub.OrganizationID.Value,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            var allowanceItems = new List<AllowanceItem>();

            foreach (var allowance in allowances)
            {
                var item = AllowanceItem.Create(
                    paystub: paystub,
                    product: allowance.Product,
                    payperiodId: payPeriod.RowID.Value,
                    allowanceId: allowance.RowID.Value,
                    organizationId: paystub.OrganizationID.Value,
                    currentlyLoggedInUserId: currentlyLoggedInUserId);

                if (allowance.IsOneTime)
                {
                    item.Amount = allowance.Amount;
                }
                else if (allowance.IsDaily)
                {
                    item = dailyCalculator.Compute(payPeriod, allowance, employee, paystub, timeEntries);
                }
                else if (allowance.IsSemiMonthly)
                {
                    item = semiMonthlyCalculator.Calculate(allowance);
                }
                else if (allowance.IsMonthly)
                {
                    // TODO: monthly payrolls are only for Fixed allowances only.
                    if (allowance.Product.Fixed && payPeriod.IsEndOfTheMonth)
                    {
                        item.Amount = allowance.Amount;
                    }
                }
                else
                {
                    item = null;
                }

                allowanceItems.Add(item);
            }

            return allowanceItems;
        }

        private async Task UpdateBenchmarkLeaveLedger(Paystub paystub, Employee employee, PayPeriod payPeriod)
        {
            var vacationLedger = await _context.LeaveLedgers
                .Include(l => l.Product)
                .Include(l => l.LastTransaction)
                .Where(l => l.EmployeeID == employee.RowID)
                .Where(l => l.Product.IsVacationLeave)
                .FirstOrDefaultAsync();

            vacationLedger.LeaveTransactions = new List<LeaveTransaction>();

            if (vacationLedger == null)
                throw new Exception($"Vacation ledger for Employee No.: {employee.EmployeeNo}");

            UpdateLedgerTransaction(
                paystub: paystub,
                payPeriod: payPeriod,
                leaveId: null, ledger:
                vacationLedger, totalLeaveHours:
                paystub.LeaveHours,
                transactionDate: payPeriod.PayToDate);
        }

        private async Task UpdateLeaveLedger(
            Paystub paystub,
            Employee employee,
            PayPeriod payPeriod,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<Leave> leaves)
        {
            // use LeaveLedgerRepository
            var employeeLeaves = leaves
                .Where(l => l.EmployeeID == employee.RowID)
                .OrderBy(l => l.StartDate)
                .ToList();

            var leaveIds = employeeLeaves.Select(l => l.RowID).ToArray();

            List<LeaveTransaction> transactions = new List<LeaveTransaction>() { };
            if (leaveIds.Any())
            {
                transactions =
                    await (from t in _context.LeaveTransactions
                           where leaveIds.Contains(t.ReferenceID)
                           select t).ToListAsync();
            }

            var employeeId = employee.RowID;
            var ledgers = await _context.LeaveLedgers
                .Include(x => x.Product)
                .Include(x => x.LeaveTransactions)
                .Include(x => x.LastTransaction)
                .Where(x => x.EmployeeID == employeeId)
                .ToListAsync();

            var newLeaveTransactions = new List<LeaveTransaction>();
            foreach (var leave in employeeLeaves)
            {
                // If a transaction has already been made for the current leave, skip the current leave.
                if (transactions.Any(t => t.ReferenceID == leave.RowID))
                {
                    continue;
                }
                else
                {
                    var ledger = ledgers.FirstOrDefault(l => l.Product.PartNo == leave.LeaveType);

                    // retrieves the time entries within leave date range
                    var timeEntry = timeEntries?.Where(t => leave.StartDate == t.Date);

                    if (timeEntry == null)
                        continue;

                    // summate the leave hours
                    var totalLeaveHours = timeEntry.Sum(t => t.TotalLeaveHours);

                    if (totalLeaveHours == 0)
                        continue;

                    var transactionDate = leave.EndDate ?? leave.StartDate;

                    UpdateLedgerTransaction(
                        paystub: paystub,
                        payPeriod: payPeriod,
                        leaveId: leave.RowID,
                        ledger: ledger,
                        totalLeaveHours: totalLeaveHours,
                        transactionDate: transactionDate);
                }
            }
        }

        private void UpdateLedgerTransaction(
            Paystub paystub,
            PayPeriod payPeriod,
            int? leaveId,
            LeaveLedger ledger,
            decimal totalLeaveHours,
            DateTime transactionDate)
        {
            var newTransaction = new LeaveTransaction()
            {
                OrganizationID = paystub.OrganizationID,
                Created = DateTime.Now,
                EmployeeID = paystub.EmployeeID,
                PayPeriodID = payPeriod.RowID,
                ReferenceID = leaveId,
                TransactionDate = transactionDate,
                Type = LeaveTransactionType.Debit,
                Amount = totalLeaveHours,
                Paystub = paystub,
                Balance = (ledger?.LastTransaction?.Balance ?? 0) - totalLeaveHours
            };

            ledger.LeaveTransactions.Add(newTransaction);
            ledger.LastTransaction = newTransaction;
        }

        private async Task UpdatePaystubItems(
            int currentlyLoggedInUserId,
            Paystub paystub,
            Employee employee,
            Product sickLeaveProduct,
            Product vacationLeaveProduct,
            IReadOnlyCollection<TimeEntry> timeEntries)
        {
            _context.Entry(paystub).Collection(p => p.PaystubItems).Load();
            _context.Set<PaystubItem>().RemoveRange(paystub.PaystubItems);

            var vacationLeaveBalance = await _context.PaystubItems
                .Where(p => p.Product.PartNo == ProductConstant.VACATION_LEAVE)
                .Where(p => p.Paystub.RowID == paystub.RowID)
                .FirstOrDefaultAsync();

            decimal vacationLeaveUsed = timeEntries?.Sum(t => t.VacationLeaveHours) ?? 0;
            decimal newBalance = employee.LeaveBalance - vacationLeaveUsed;

            vacationLeaveBalance = new PaystubItem()
            {
                OrganizationID = paystub.OrganizationID,
                Created = DateTime.Now,
                CreatedBy = currentlyLoggedInUserId,
                ProductID = vacationLeaveProduct.RowID,
                PayAmount = newBalance,
                Paystub = paystub
            };

            paystub.PaystubItems.Add(vacationLeaveBalance);

            var sickLeaveBalance = await _context.PaystubItems
                .Where(p => p.Product.PartNo == ProductConstant.SICK_LEAVE)
                .Where(p => p.Paystub.RowID == paystub.RowID)
                .FirstOrDefaultAsync();

            var sickLeaveUsed = timeEntries?.Sum(t => t.SickLeaveHours) ?? 0;
            var newBalance2 = employee.SickLeaveBalance - sickLeaveUsed;

            sickLeaveBalance = new PaystubItem()
            {
                OrganizationID = paystub.OrganizationID,
                Created = DateTime.Now,
                CreatedBy = currentlyLoggedInUserId,
                ProductID = sickLeaveProduct.RowID,
                PayAmount = newBalance2,
                Paystub = paystub
            };

            paystub.PaystubItems.Add(sickLeaveBalance);
        }
    }
}
