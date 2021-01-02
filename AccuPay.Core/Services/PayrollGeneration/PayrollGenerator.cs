using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class PayrollGenerator
    {
        private readonly PayrollContext _context;
        private readonly PaystubDataService _paystubDataService;

        //private static ILog logger = LogManager.GetLogger("PayrollLogger");

        public PayrollGenerator(PayrollContext context, PaystubDataService paystubDataService)
        {
            _context = context;
            _paystubDataService = paystubDataService;
        }

        public async Task<PaystubEmployeeResult> Start(
            int employeeId,
            PayrollResources resources,
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
            PayrollResources resources,
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

            var loanTransactions = CreateLoanTransactions(
                paystub,
                payPeriod,
                loans,
                bonuses: bonuses,
                policy: policy,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            ComputePayroll(
                resources,
                currentlyLoggedInUserId: currentlyLoggedInUserId,
                currentSystemOwner,
                settings,
                calendarCollection,
                payPeriod,
                paystub: paystub,
                employee: employee,
                salary: salary,
                previousPaystub: previousPaystub,
                loanTransactions: loanTransactions,
                timeEntries: timeEntries,
                actualTimeEntries: actualTimeEntries,
                allowances: allowances,
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

            if (EligibleForNewBPIInsurance(paystub, employee, settings, payPeriod))
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

            if (currentSystemOwner != SystemOwnerService.Benchmark)
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
                var loanPaymentFromBonuses = GetLoanPaymentFromBonuses(bonuses, loan, payPeriod: payPeriod);

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

        private static IEnumerable<LoanPaymentFromBonus> GetLoanPaymentFromBonuses(IReadOnlyCollection<Bonus> bonuses, Loan loan, PayPeriod payPeriod)
        {
            if (loan.LoanPaymentFromBonuses == null || !loan.LoanPaymentFromBonuses.Any())
            {
                return new List<LoanPaymentFromBonus>();
            }

            var bonusIds = bonuses
                .Select(b => b.RowID.Value)
                .ToArray();
            if (!payPeriod.IsEndOfTheMonth)
            {
                bonusIds = bonuses
                    .Where(b => b.AllowanceFrequency != Bonus.FREQUENCY_MONTHLY)
                    .Select(b => b.RowID.Value)
                    .ToArray();
            }

            var loanPaymentFromBonuses = loan
                .LoanPaymentFromBonuses
                .Where(l => l.LoanId == loan.RowID.Value)
                .Where(l => bonusIds.Contains(l.BonusId));

            return loanPaymentFromBonuses;
        }

        private void ComputePayroll(
            PayrollResources resources,
            int currentlyLoggedInUserId,
            string currentSystemOwner,
            ListOfValueCollection settings,
            CalendarCollection calendarCollection,
            PayPeriod payPeriod,
            Paystub paystub,
            Employee employee,
            Salary salary,
            Paystub previousPaystub,
            ICollection<LoanTransaction> loanTransactions,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<ActualTimeEntry> actualTimeEntries,
            IReadOnlyCollection<Allowance> allowances,
            IReadOnlyCollection<AllowanceItem> allowanceItems,
            IReadOnlyCollection<Bonus> bonuses)
        {
            if (currentSystemOwner != SystemOwnerService.Benchmark)
            {
                ComputeBasicHoursAndPay(paystub, employee, salary, calendarCollection, timeEntries: timeEntries, actualTimeEntries: actualTimeEntries);

                ComputeHours(paystub, employee, salary, timeEntries: timeEntries, actualTimeEntries: actualTimeEntries);

                ComputeTotalEarnings(paystub, employee, settings, payPeriod);
            }

            // Allowances
            paystub.TotalTaxableAllowance = AccuMath.CommercialRound(allowanceItems.Where(a => a.IsTaxable).Sum(a => a.Amount));
            paystub.TotalNonTaxableAllowance = AccuMath.CommercialRound(allowanceItems.Where(a => !a.IsTaxable).Sum(a => a.Amount));

            // Bonuses
            paystub.TotalBonus = AccuMath.CommercialRound(bonuses.Sum(b => b.BonusAmount));

            // Loans
            paystub.TotalLoans = loanTransactions.Sum(t => t.DeductionAmount);

            // gross pay and total earnings should be higher than the goverment deduction calculators
            // since it is sometimes used in computing the basis pay for the deductions
            // depending on the organization's policy
            if (paystub.TotalEarnings < 0)
                paystub.TotalEarnings = 0;

            paystub.GrossPay = paystub.TotalEarnings + paystub.TotalBonus + paystub.GrandTotalAllowance;

            paystub.TotalAdjustments = paystub.Adjustments.Sum(a => a.Amount);
            // BPI Insurance feature, currently used by LA Global
            if (EligibleForNewBPIInsurance(paystub, employee, settings, payPeriod))
            {
                paystub.TotalAdjustments -= employee.BPIInsurance;
            }

            var socialSecurityCalculator = new SssCalculator(settings, resources.SocialSecurityBrackets);
            socialSecurityCalculator.Calculate(paystub, previousPaystub, salary, employee, payPeriod, currentSystemOwner);

            var philHealthCalculator = new PhilHealthCalculator(new PhilHealthPolicy(settings));
            philHealthCalculator.Calculate(salary, paystub, previousPaystub, employee, payPeriod, allowances, currentSystemOwner);

            var hdmfCalculator = new HdmfCalculator();
            hdmfCalculator.Calculate(salary, paystub, employee, settings, payPeriod);

            var withholdingTaxCalculator = new WithholdingTaxCalculator(settings, resources.WithholdingTaxBrackets);
            withholdingTaxCalculator.Calculate(paystub, previousPaystub, employee, payPeriod, salary);

            paystub.NetPay = AccuMath.CommercialRound(paystub.GrossPay - paystub.NetDeductions + paystub.TotalAdjustments);

            var actualCalculator = new PaystubActualCalculator();
            actualCalculator.Compute(employee, salary, settings, payPeriod, paystub, currentSystemOwner, actualTimeEntries);

            var thirteenthMonthPayCalculator = new ThirteenthMonthPayCalculator(
                organizationId: paystub.OrganizationID.Value,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            thirteenthMonthPayCalculator.Calculate(employee, paystub, timeEntries, actualTimeEntries, salary, settings, allowanceItems.ToList(), currentSystemOwner);
        }

        private bool EligibleForNewBPIInsurance(Paystub paystub, Employee employee, ListOfValueCollection settings, PayPeriod payPeriod)
        {
            return settings.GetBoolean("Employee Policy.UseBPIInsurance", false) &&
                    employee.IsFirstPay(payPeriod) &&
                    employee.BPIInsurance > 0 &&
                    !paystub.Adjustments.Any(a => a.Product?.PartNo == ProductConstant.BPI_INSURANCE_ADJUSTMENT);
        }

        private void ComputeTotalEarnings(Paystub paystub, Employee employee, ListOfValueCollection settings, PayPeriod payPeriod)
        {
            if (employee.IsFixed)
            {
                paystub.TotalEarnings = paystub.BasicPay + paystub.AdditionalPay;
            }
            else if (employee.IsMonthly)
            {
                var isFirstPayAsDailyRule = settings.GetBoolean("Payroll Policy", "isfirstsalarydaily");

                if (employee.IsFirstPay(payPeriod) && isFirstPayAsDailyRule)
                {
                    paystub.TotalEarnings = paystub.RegularPay +
                                            paystub.LeavePay +
                                            paystub.AdditionalPay;
                }
                else
                {
                    paystub.RegularHours = paystub.BasicHours - paystub.LeaveHours;

                    paystub.RegularPay = paystub.BasicPay - paystub.LeavePay;

                    paystub.TotalEarnings = (paystub.BasicPay + paystub.AdditionalPay) -
                                            paystub.BasicDeductions;
                }
            }
            else if (employee.IsDaily)
            {
                paystub.TotalEarnings = paystub.RegularPay +
                                        paystub.LeavePay +
                                        paystub.AdditionalPay;
            }
        }

        private void ComputeBasicHoursAndPay(Paystub paystub,
                                            Employee employee,
                                            Salary salary,
                                            CalendarCollection calendarCollection,
                                            IReadOnlyCollection<TimeEntry> timeEntries,
                                            IReadOnlyCollection<ActualTimeEntry> actualTimeEntries)
        {
            // Basic Hours
            if (employee.IsPremiumInclusive)
            {
                if (employee.WorkDaysPerYear > 0)
                {
                    var workDaysPerPayPeriod = employee.WorkDaysPerYear /
                                                CalendarConstant.MonthsInAYear /
                                                CalendarConstant.SemiMonthlyPayPeriodsPerMonth;

                    paystub.BasicHours = workDaysPerPayPeriod * 8;
                }
            }
            else if (employee.IsDaily)
            {
                paystub.BasicHours = ComputeBasicHoursForDaily(calendarCollection, timeEntries);
            }

            // Basic Pay
            paystub.ComputeBasicPay(employee.IsDaily, salary.BasicSalary, timeEntries);

            paystub.Actual.ComputeBasicPay(employee.IsDaily, salary.TotalSalary, actualTimeEntries);
        }

        private decimal ComputeBasicHoursForDaily(CalendarCollection calendarCollection, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            var basicHours = 0M;

            foreach (var timeEntry in timeEntries)
            {
                var payrateCalendar = calendarCollection.GetCalendar(timeEntry.BranchID);
                var payrate = payrateCalendar.Find(timeEntry.Date);

                if (!(timeEntry.IsRestDay ||
                    (timeEntry.TotalLeaveHours > 0) ||
                    payrate.IsRegularHoliday ||
                    payrate.IsSpecialNonWorkingHoliday))

                    basicHours += timeEntry.WorkHours;
            }

            return basicHours;
        }

        private void ComputeHours(Paystub paystub,
                                Employee employee,
                                Salary salary,
                                IReadOnlyCollection<TimeEntry> timeEntries,
                                IReadOnlyCollection<ActualTimeEntry> actualTimeEntries)
        {
            PaystubRate paystubRate = new PaystubRate();
            paystubRate.Compute(timeEntries, salary, employee, actualTimeEntries);

            paystub.RegularHours = paystubRate.RegularHours;
            paystub.RegularPay = paystubRate.RegularPay;
            paystub.Actual.RegularPay = paystubRate.ActualRegularPay;

            paystub.OvertimeHours = paystubRate.OvertimeHours;
            paystub.OvertimePay = paystubRate.OvertimePay;
            paystub.Actual.OvertimePay = paystubRate.ActualOvertimePay;

            paystub.NightDiffHours = paystubRate.NightDiffHours;
            paystub.NightDiffPay = paystubRate.NightDiffPay;
            paystub.Actual.NightDiffPay = paystubRate.ActualNightDiffPay;

            paystub.NightDiffOvertimeHours = paystubRate.NightDiffOvertimeHours;
            paystub.NightDiffOvertimePay = paystubRate.NightDiffOvertimePay;
            paystub.Actual.NightDiffOvertimePay = paystubRate.ActualNightDiffOvertimePay;

            paystub.RestDayHours = paystubRate.RestDayHours;
            paystub.RestDayPay = paystubRate.RestDayPay;
            paystub.Actual.RestDayPay = paystubRate.ActualRestDayPay;

            paystub.RestDayOTHours = paystubRate.RestDayOTHours;
            paystub.RestDayOTPay = paystubRate.RestDayOTPay;
            paystub.Actual.RestDayOTPay = paystubRate.ActualRestDayOTPay;

            paystub.SpecialHolidayHours = paystubRate.SpecialHolidayHours;
            paystub.SpecialHolidayPay = paystubRate.SpecialHolidayPay;
            paystub.Actual.SpecialHolidayPay = paystubRate.ActualSpecialHolidayPay;

            paystub.SpecialHolidayOTHours = paystubRate.SpecialHolidayOTHours;
            paystub.SpecialHolidayOTPay = paystubRate.SpecialHolidayOTPay;
            paystub.Actual.SpecialHolidayOTPay = paystubRate.ActualSpecialHolidayOTPay;

            paystub.RegularHolidayHours = paystubRate.RegularHolidayHours;
            paystub.RegularHolidayPay = paystubRate.RegularHolidayPay;
            paystub.Actual.RegularHolidayPay = paystubRate.ActualRegularHolidayPay;

            paystub.RegularHolidayOTHours = paystubRate.RegularHolidayOTHours;
            paystub.RegularHolidayOTPay = paystubRate.RegularHolidayOTPay;
            paystub.Actual.RegularHolidayOTPay = paystubRate.ActualRegularHolidayOTPay;

            paystub.LeaveHours = paystubRate.LeaveHours;
            paystub.LeavePay = paystubRate.LeavePay;
            paystub.Actual.LeavePay = paystubRate.ActualLeavePay;

            paystub.LateHours = paystubRate.LateHours;
            paystub.LateDeduction = paystubRate.LateDeduction;
            paystub.Actual.LateDeduction = paystubRate.ActualLateDeduction;

            paystub.UndertimeHours = paystubRate.UndertimeHours;
            paystub.UndertimeDeduction = paystubRate.UndertimeDeduction;
            paystub.Actual.UndertimeDeduction = paystubRate.ActualUndertimeDeduction;

            paystub.AbsentHours = paystubRate.AbsentHours;
            paystub.AbsenceDeduction = paystubRate.AbsenceDeduction;
            paystub.Actual.AbsenceDeduction = paystubRate.ActualAbsenceDeduction;
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
                    var timeEntry = timeEntries.Where(t => leave.StartDate == t.Date);

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

        private List<LoanTransaction> CreateLoanTransactions(
            Paystub paystub,
            PayPeriod payPeriod,
            IReadOnlyCollection<Loan> loans,
            IReadOnlyCollection<Bonus> bonuses,
            IPolicyHelper policy,
            int currentlyLoggedInUserId)
        {
            var loanTransactions = new List<LoanTransaction>();
            var currentLoans = loans.Where(x => x.Status == Loan.STATUS_IN_PROGRESS);

            var calculator = new LoanDeductionAmountCalculator(policy);

            foreach (var loan in currentLoans)
            {
                if (loan.LoanPayPeriodLeft == 0) continue;

                var previousLoanTransactions = loan
                    .LoanTransactions?
                    .Where(x => x.PayPeriod.PayFromDate < payPeriod.PayFromDate)
                    .ToList();

                var yearlyLoanInterest = loan.YearlyLoanInterests?
                    .Where(x => x.IsWithInPayPeriod(payPeriod))
                    .OrderBy(x => x.Year)
                    .LastOrDefault();

                var bonusLoanPayments = GetLoanPaymentFromBonuses(bonuses, loan, payPeriod).ToList();

                (decimal deductionAmount, decimal interestAmount, YearlyLoanInterest newYearlyLoanInterest) =
                    calculator.Calculate(
                        loan, payPeriod,
                        yearlyLoanInterest: yearlyLoanInterest,
                        previousLoanTransactions: previousLoanTransactions,
                        bonusLoanPayments: bonusLoanPayments,
                        thirteenthMonthPayLoanPayments: paystub.LoanPaymentFromThirteenthMonthPays?.ToList());

                var loanTransaction = new LoanTransaction()
                {
                    Created = DateTime.Now,
                    LastUpd = DateTime.Now,
                    Paystub = paystub,
                    OrganizationID = paystub.OrganizationID,
                    EmployeeID = paystub.EmployeeID,
                    PayPeriodID = payPeriod.RowID,
                    LoanID = loan.RowID.Value,
                    DeductionAmount = deductionAmount,
                    InterestAmount = interestAmount
                };

                loanTransactions.Add(loanTransaction);

                if (policy.UseGoldwingsLoanInterest)
                {
                    if (newYearlyLoanInterest != null)
                    {
                        newYearlyLoanInterest.CreatedBy = currentlyLoggedInUserId;
                        loan.YearlyLoanInterests.Add(newYearlyLoanInterest);
                    }
                }

                loan.TotalBalanceLeft -= loanTransaction.PrincipalAmount;
                loan.RecomputePayPeriodLeft();

                loanTransaction.LoanPayPeriodLeft = loan.LoanPayPeriodLeft;
                loanTransaction.TotalBalance = loan.TotalBalanceLeft;
            }

            return loanTransactions;
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

            var vacationLeaveUsed = timeEntries.Sum(t => t.VacationLeaveHours);
            var newBalance = employee.LeaveBalance - vacationLeaveUsed;

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

            var sickLeaveUsed = timeEntries.Sum(t => t.SickLeaveHours);
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

        private class PaystubRate : IPaystubRate
        {
            public decimal RegularHours { get; set; }
            public decimal RegularPay { get; set; }
            public decimal ActualRegularPay { get; set; }

            public decimal OvertimeHours { get; set; }
            public decimal OvertimePay { get; set; }
            public decimal ActualOvertimePay { get; set; }

            public decimal NightDiffHours { get; set; }
            public decimal NightDiffPay { get; set; }
            public decimal ActualNightDiffPay { get; set; }

            public decimal NightDiffOvertimeHours { get; set; }
            public decimal NightDiffOvertimePay { get; set; }
            public decimal ActualNightDiffOvertimePay { get; set; }

            public decimal RestDayHours { get; set; }
            public decimal RestDayPay { get; set; }
            public decimal ActualRestDayPay { get; set; }

            public decimal RestDayOTHours { get; set; }
            public decimal RestDayOTPay { get; set; }
            public decimal ActualRestDayOTPay { get; set; }

            public decimal SpecialHolidayHours { get; set; }
            public decimal SpecialHolidayPay { get; set; }
            public decimal ActualSpecialHolidayPay { get; set; }

            public decimal SpecialHolidayOTHours { get; set; }
            public decimal SpecialHolidayOTPay { get; set; }
            public decimal ActualSpecialHolidayOTPay { get; set; }

            public decimal RegularHolidayHours { get; set; }
            public decimal RegularHolidayPay { get; set; }
            public decimal ActualRegularHolidayPay { get; set; }

            public decimal RegularHolidayOTHours { get; set; }
            public decimal RegularHolidayOTPay { get; set; }
            public decimal ActualRegularHolidayOTPay { get; set; }

            public decimal HolidayPay { get; set; }

            public decimal LeaveHours { get; set; }
            public decimal LeavePay { get; set; }
            public decimal ActualLeavePay { get; set; }

            public decimal LateHours { get; set; }
            public decimal LateDeduction { get; set; }
            public decimal ActualLateDeduction { get; set; }

            public decimal UndertimeHours { get; set; }
            public decimal UndertimeDeduction { get; set; }
            public decimal ActualUndertimeDeduction { get; set; }

            public decimal AbsentHours { get; set; }
            public decimal AbsenceDeduction { get; set; }
            public decimal ActualAbsenceDeduction { get; set; }

            public void Compute(IReadOnlyCollection<TimeEntry> timeEntries,
                                Salary salary,
                                Employee employee,
                                IReadOnlyCollection<ActualTimeEntry> actualtimeentries)
            {
                var totalTimeEntries = TotalTimeEntryCalculator.Calculate(timeEntries, salary, employee, actualtimeentries);

                this.RegularHours = totalTimeEntries.RegularHours;
                this.RegularPay = totalTimeEntries.RegularPay;
                this.ActualRegularPay = totalTimeEntries.ActualRegularPay;

                this.OvertimeHours = totalTimeEntries.OvertimeHours;
                this.OvertimePay = totalTimeEntries.OvertimePay;
                this.ActualOvertimePay = totalTimeEntries.ActualOvertimePay;

                this.NightDiffHours = totalTimeEntries.NightDifferentialHours;
                this.NightDiffPay = totalTimeEntries.NightDiffPay;
                this.ActualNightDiffPay = totalTimeEntries.ActualNightDiffPay;

                this.NightDiffOvertimeHours = totalTimeEntries.NightDifferentialOvertimeHours;
                this.NightDiffOvertimePay = totalTimeEntries.NightDiffOvertimePay;
                this.ActualNightDiffOvertimePay = totalTimeEntries.ActualNightDiffOvertimePay;

                this.RestDayHours = totalTimeEntries.RestDayHours;
                this.RestDayPay = totalTimeEntries.RestDayPay;
                this.ActualRestDayPay = totalTimeEntries.ActualRestDayPay;

                this.RestDayOTHours = totalTimeEntries.RestDayOTHours;
                this.RestDayOTPay = totalTimeEntries.RestDayOTPay;
                this.ActualRestDayOTPay = totalTimeEntries.ActualRestDayOTPay;

                this.SpecialHolidayHours = totalTimeEntries.SpecialHolidayHours;
                this.SpecialHolidayPay = totalTimeEntries.SpecialHolidayPay;
                this.ActualSpecialHolidayPay = totalTimeEntries.ActualSpecialHolidayPay;

                this.SpecialHolidayOTHours = totalTimeEntries.SpecialHolidayOTHours;
                this.SpecialHolidayOTPay = totalTimeEntries.SpecialHolidayOTPay;
                this.ActualSpecialHolidayOTPay = totalTimeEntries.ActualSpecialHolidayOTPay;

                this.RegularHolidayHours = totalTimeEntries.RegularHolidayHours;
                this.RegularHolidayPay = totalTimeEntries.RegularHolidayPay;
                this.ActualRegularHolidayPay = totalTimeEntries.ActualRegularHolidayPay;

                this.RegularHolidayOTHours = totalTimeEntries.RegularHolidayOTHours;
                this.RegularHolidayOTPay = totalTimeEntries.RegularHolidayOTPay;
                this.ActualRegularHolidayOTPay = totalTimeEntries.ActualRegularHolidayOTPay;

                this.HolidayPay = totalTimeEntries.HolidayPay;

                this.LeaveHours = totalTimeEntries.LeaveHours;
                this.LeavePay = totalTimeEntries.LeavePay;
                this.ActualLeavePay = totalTimeEntries.ActualLeavePay;

                this.LateHours = totalTimeEntries.LateHours;
                this.LateDeduction = totalTimeEntries.LateDeduction;
                this.ActualLateDeduction = totalTimeEntries.ActualLateDeduction;

                this.UndertimeHours = totalTimeEntries.UndertimeHours;
                this.UndertimeDeduction = totalTimeEntries.UndertimeDeduction;
                this.ActualUndertimeDeduction = totalTimeEntries.ActualUndertimeDeduction;

                this.AbsentHours = totalTimeEntries.AbsentHours;
                this.AbsenceDeduction = totalTimeEntries.AbsenceDeduction;
                this.ActualAbsenceDeduction = totalTimeEntries.ActualAbsenceDeduction;
            }
        }
    }
}
