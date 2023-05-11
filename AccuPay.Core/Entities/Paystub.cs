using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities
{
    [Table("paystub")]
    public class Paystub : BasePaystub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public decimal BasicHours { get; set; }

        public decimal RegularHours { get; set; }

        public decimal OvertimeHours { get; set; }

        public decimal NightDiffHours { get; set; }

        public decimal NightDiffOvertimeHours { get; set; }

        public decimal RestDayHours { get; set; }

        public decimal RestDayOTHours { get; set; }

        public decimal LeaveHours { get; set; }

        public decimal SpecialHolidayHours { get; set; }

        public decimal SpecialHolidayOTHours { get; set; }

        public decimal RegularHolidayHours { get; set; }

        public decimal RegularHolidayOTHours { get; set; }

        public decimal LateHours { get; set; }

        public decimal UndertimeHours { get; set; }

        public decimal AbsentHours { get; set; }

        public decimal RestDayNightDiffHours { get; set; }
        public decimal RestDayNightDiffOTHours { get; set; }

        public decimal SpecialHolidayNightDiffHours { get; set; }
        public decimal SpecialHolidayNightDiffOTHours { get; set; }
        public decimal SpecialHolidayRestDayHours { get; set; }
        public decimal SpecialHolidayRestDayOTHours { get; set; }
        public decimal SpecialHolidayRestDayNightDiffHours { get; set; }
        public decimal SpecialHolidayRestDayNightDiffOTHours { get; set; }

        public decimal RegularHolidayNightDiffHours { get; set; }
        public decimal RegularHolidayNightDiffOTHours { get; set; }
        public decimal RegularHolidayRestDayHours { get; set; }
        public decimal RegularHolidayRestDayOTHours { get; set; }
        public decimal RegularHolidayRestDayNightDiffHours { get; set; }
        public decimal RegularHolidayRestDayNightDiffOTHours { get; set; }

        [Column("WorkPay")]
        public decimal TotalEarnings { get; set; }

        public decimal TotalBonus { get; set; }

        [Column("TotalAllowance")]
        public decimal TotalNonTaxableAllowance { get; set; }

        public decimal TotalTaxableAllowance { get; set; }

        [Column("DeferredTaxableIncome")]
        public decimal DeferredTaxableIncome { get; set; }

        [Column("TotalTaxableSalary")]
        public decimal TaxableIncome { get; set; }

        [Column("TotalEmpWithholdingTax")]
        public decimal WithholdingTax { get; set; }

        [Column("TotalEmpSSS")]
        public decimal SssEmployeeShare { get; set; }

        [Column("TotalCompSSS")]
        public decimal SssEmployerShare { get; set; }

        [Column("TotalEmpPhilhealth")]
        public decimal PhilHealthEmployeeShare { get; set; }

        [Column("TotalCompPhilhealth")]
        public decimal PhilHealthEmployerShare { get; set; }

        [Column("TotalEmpHDMF")]
        public decimal HdmfEmployeeShare { get; set; }

        [Column("TotalCompHDMF")]
        public decimal HdmfEmployerShare { get; set; }

        public decimal TotalLoans { get; set; }

        [ForeignKey("PayPeriodID")]
        public virtual PayPeriod PayPeriod { get; set; }

        public virtual ICollection<Adjustment> Adjustments { get; set; }

        public virtual ICollection<ActualAdjustment> ActualAdjustments { get; set; }

        public virtual ICollection<AllowanceItem> AllowanceItems { get; set; }

        public virtual ICollection<LeaveTransaction> LeaveTransactions { get; set; }

        public virtual ICollection<LoanPaymentFromThirteenthMonthPay> LoanPaymentFromThirteenthMonthPays { get; set; }

        public virtual ICollection<LoanTransaction> LoanTransactions { get; set; }

        public virtual ICollection<PaystubEmail> PaystubEmails { get; set; }

        public virtual ICollection<PaystubEmailHistory> PaystubEmailHistories { get; set; }

        public virtual ICollection<PaystubItem> PaystubItems { get; set; }

        public virtual ThirteenthMonthPay ThirteenthMonthPay { get; set; }

        public virtual PaystubActual Actual { get; set; }

        // TODO: try to remove this
        [NotMapped]
        public decimal Ecola { get; set; }

        public decimal GrandTotalAllowance => TotalNonTaxableAllowance + TotalTaxableAllowance;

        public decimal NetDeductions => GovernmentDeductions + TotalLoans + WithholdingTax;

        public decimal GovernmentDeductions => SssEmployeeShare + PhilHealthEmployeeShare + HdmfEmployeeShare;

        public decimal TotalRestDayHours => RestDayHours + SpecialHolidayRestDayHours + RegularHolidayRestDayHours;

        public decimal TotalRestDayPay => RestDayPay + SpecialHolidayRestDayPay + RegularHolidayRestDayPay;

        public decimal RegularHoursAndTotalRestDay => RegularHours + TotalRestDayHours;

        public decimal RegularPayAndTotalRestDay => RegularPay + TotalRestDayPay;

        public decimal TotalOvertimeHours =>
            OvertimeHours +
            RestDayOTHours +
            SpecialHolidayOTHours +
            RegularHolidayOTHours +
            SpecialHolidayRestDayOTHours +
            RegularHolidayRestDayOTHours;

        // needed to be virtual to be overriden in unit test
        public virtual decimal TotalWorkedHoursWithoutOvertimeAndLeave(bool isMonthly)
        {
            decimal totalHours =
                RegularHoursAndTotalRestDay +
                SpecialHolidayHours +
                RegularHolidayHours;

            if (isMonthly)
            {
                // RegularHours of monthly employees is inclusive of AbsentHours,
                // LateHours, and UndertimeHours. To get the true total worked hours,
                // AbsentHours, LateHours, and UndertimeHours needs to be deducted to
                // the RegularHours if the employee is monthly.
                // This difference between monthly and daily RegularHours can be seen
                // in the payroll summary. RegularHours of Daily is exclusive of
                // AbsentHours, LateHours, and UndertimeHours while Monthly is inclusive.
                totalHours = totalHours - AbsentHours - LateHours - UndertimeHours;
            }

            return totalHours;
        }

        public decimal TotalWorkedHoursWithoutLeave(bool isMonthly)
        {
            return TotalWorkedHoursWithoutOvertimeAndLeave(isMonthly) + TotalOvertimeHours;
        }

        // needed to be virtual to be overriden in unit test
        public virtual decimal TotalDaysPayWithOutOvertimeAndLeave(bool isMonthly)
        {
            decimal totalPay =
                RegularPayAndTotalRestDay +
                SpecialHolidayPay +
                RegularHolidayPay;

            if (isMonthly)
            {
                // RegularPay of monthly employees is inclusive of AbsenceDeduction,
                // LateDeduction, and UndertimeDeduction. To get the true total worked hours,
                // AbsenceDeduction, LateDeduction, and UndertimeDeduction needs to be deducted to
                // the RegularPay if the employee is monthly.
                // This difference between monthly and daily RegularPay can be seen
                // in the payroll summary. RegularPay of Daily is exclusive of
                // AbsenceDeduction, LateDeduction, and UndertimeDeduction while Monthly is inclusive.
                totalPay = totalPay - AbsenceDeduction - LateDeduction - UndertimeDeduction;
            }

            return totalPay;
        }

        public decimal TotalDeductionAdjustments =>
            Adjustments.Where(a => a.Amount < 0).Sum(a => a.Amount) +
            ActualAdjustments.Where(a => a.Amount < 0).Sum(a => a.Amount);

        public decimal TotalAdditionAdjustments =>
            Adjustments.Where(a => a.Amount > 0).Sum(a => a.Amount) +
            ActualAdjustments.Where(a => a.Amount > 0).Sum(a => a.Amount);

        public bool IsNewEntity => BaseEntity.CheckIfNewEntity(RowID);

        public Paystub()
        {
            Adjustments = new List<Adjustment>();
            ActualAdjustments = new List<ActualAdjustment>();
            PaystubItems = new List<PaystubItem>();

            AllowanceItems = new List<AllowanceItem>();
            LeaveTransactions = new List<LeaveTransaction>();
            LoanTransactions = new List<LoanTransaction>();
        }

        #region Payroll

        public ICollection<AllowanceItem> CreateAllowanceItems(
            int currentlyLoggedInUserId,
            ListOfValueCollection settings,
            CalendarCollection calendarCollection,
            PayPeriod payPeriod,
            Employee employee,
            IReadOnlyCollection<TimeEntry> previousTimeEntries,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<Allowance> allowances,
            IReadOnlyCollection<Shift> shifts)
        {
            var dailyCalculator = new DailyAllowanceCalculator(
                new AllowancePolicy(settings),
                employee,
                this,
                payPeriod,
                calendarCollection,
                timeEntries: timeEntries,
                previousTimeEntries: previousTimeEntries,
                shifts: shifts,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            var semiMonthlyCalculator = new SemiMonthlyAllowanceCalculator(
                new AllowancePolicy(settings),
                employee,
                this,
                payPeriod,
                calendarCollection,
                timeEntries,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            var allowanceItems = new List<AllowanceItem>();

            foreach (var allowance in allowances)
            {
                var item = AllowanceItem.Create(
                    paystub: this,
                    product: allowance.Product,
                    payperiodId: payPeriod.RowID.Value,
                    allowanceId: allowance.RowID.Value,
                    currentlyLoggedInUserId: currentlyLoggedInUserId);

                if (allowance.IsOneTime)
                {
                    item.Amount = allowance.Amount;
                }
                else if (allowance.IsDaily)
                {
                    item = dailyCalculator.Compute(allowance);
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

        public List<LoanTransaction> CreateLoanTransactions(
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

                var yearlyLoanInterest = loan.YearlyLoanInterests?
                    .Where(x => x.IsWithInPayPeriod(payPeriod))
                    .OrderBy(x => x.Year)
                    .LastOrDefault();

                var previousLoanTransactions = loan
                    .LoanTransactions?
                    .Where(x => x.PayPeriod.PayFromDate < payPeriod.PayFromDate)
                    .ToList();

                var bonusLoanPayments = GetLoanPaymentFromBonuses(bonuses, loan, payPeriod)
                    .ToList();

                (decimal deductionAmount, decimal interestAmount, YearlyLoanInterest newYearlyLoanInterest) =
                    calculator.Calculate(
                        loan, payPeriod,
                        yearlyLoanInterest: yearlyLoanInterest,
                        previousLoanTransactions: previousLoanTransactions,
                        bonusLoanPayments: bonusLoanPayments,
                        thirteenthMonthPayLoanPayments: LoanPaymentFromThirteenthMonthPays?.ToList());

                var loanTransaction = new LoanTransaction()
                {
                    Created = DateTime.Now,
                    LastUpd = DateTime.Now,
                    Paystub = this,
                    OrganizationID = OrganizationID,
                    EmployeeID = EmployeeID,
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

        public ICollection<LoanPaymentFromBonus> GetLoanPaymentFromBonuses(IReadOnlyCollection<Bonus> bonuses, Loan loan, PayPeriod payPeriod)
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

            return loan
                .LoanPaymentFromBonuses
                .Where(l => l.LoanId == loan.RowID.Value)
                .Where(l => bonusIds.Contains(l.BonusId))
                .ToList();
        }

        public void ComputePayroll(
            IPayrollResources resources,
            int currentlyLoggedInUserId,
            string currentSystemOwner,
            ListOfValueCollection settings,
            CalendarCollection calendarCollection,
            PayPeriod payPeriod,
            Employee employee,
            Salary salary,
            Paystub previousPaystub,
            ICollection<LoanTransaction> loanTransactions,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<ActualTimeEntry> actualTimeEntries,
            IReadOnlyCollection<AllowanceItem> allowanceItems,
            IReadOnlyCollection<Bonus> bonuses)
        {
            if (currentSystemOwner != SystemOwner.Benchmark)
            {
                ComputeBasicHoursAndPay(employee, salary, calendarCollection, timeEntries: timeEntries, actualTimeEntries: actualTimeEntries);

                ComputeHours(employee, salary, timeEntries: timeEntries, actualTimeEntries: actualTimeEntries);

                var isFirstPayAsDailyRule = settings.GetBoolean("Payroll Policy", "isfirstsalarydaily");
                ComputeTotalEarnings(employee, isFirstPayAsDailyRule, payPeriod, currentSystemOwner);
            }

            // Allowances
            TotalTaxableAllowance = AccuMath.CommercialRound(allowanceItems.Where(a => a.IsTaxable).Sum(a => a.Amount));
            TotalNonTaxableAllowance = AccuMath.CommercialRound(allowanceItems.Where(a => !a.IsTaxable).Sum(a => a.Amount));

            // Bonuses
            TotalBonus = AccuMath.CommercialRound(bonuses.Sum(b => b.BonusAmount));

            // Loans
            TotalLoans = loanTransactions.Sum(t => t.DeductionAmount);

            // gross pay and total earnings should be higher than the goverment deduction calculators
            // since it is sometimes used in computing the basis pay for the deductions
            // depending on the organization's policy
            if (TotalEarnings < 0)
                TotalEarnings = 0;

            GrossPay = TotalEarnings + TotalBonus + GrandTotalAllowance;

            TotalAdjustments = Adjustments.Sum(a => a.Amount);

            bool isEligibleForBPIInsurance = employee.IsEligibleForNewBPIInsurance(
                useBPIInsurancePolicy: settings.GetBoolean("Employee Policy.UseBPIInsurance", false),
                Adjustments,
                payPeriod);

            // BPI Insurance feature, currently used by LA Global
            if (isEligibleForBPIInsurance)
            {
                TotalAdjustments -= employee.BPIInsurance;
            }

            var socialSecurityCalculator = new SssCalculator(resources.Policy, resources.SocialSecurityBrackets, payPeriod);
            socialSecurityCalculator.Calculate(this, previousPaystub, salary, employee, currentSystemOwner);

            var philHealthCalculator = new PhilHealthCalculator(new PhilHealthPolicy(settings));
            philHealthCalculator.Calculate(salary, this, previousPaystub, employee, payPeriod, currentSystemOwner);

            var hdmfCalculator = new HdmfCalculator();
            hdmfCalculator.Calculate(salary, this, employee, settings, payPeriod);

            var withholdingTaxCalculator = new WithholdingTaxCalculator(settings, resources.WithholdingTaxBrackets);
            withholdingTaxCalculator.Calculate(this, previousPaystub, employee, payPeriod, salary);

            NetPay = AccuMath.CommercialRound(GrossPay - NetDeductions + TotalAdjustments);

            var actualCalculator = new PaystubActualCalculator();
            actualCalculator.Compute(employee, salary, settings, payPeriod, this, currentSystemOwner);

            var thirteenthMonthPayCalculator = new ThirteenthMonthPayCalculator(
                organizationId: OrganizationID.Value,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            thirteenthMonthPayCalculator.Calculate(employee, this, timeEntries, actualTimeEntries, salary, settings, allowanceItems.ToList(), currentSystemOwner);
        }

        public void ComputeTotalEarnings(Employee employee, bool isFirstPayAsDailyRule, PayPeriod payPeriod, string currentSystemOwner)
        {
            if (employee.IsFixed)
            {
                TotalEarnings = BasicPay + AdditionalPay;
            }
            else if (employee.IsDaily || currentSystemOwner == SystemOwner.Benchmark)
            {
                TotalEarnings = TotalEarningForDaily;
            }
            else if (employee.IsMonthly)
            {
                if (employee.IsFirstPay(payPeriod) && isFirstPayAsDailyRule)
                {
                    TotalEarnings = TotalEarningForDaily;
                }
                else
                {
                    RegularHours = BasicHours - LeaveHours;

                    RegularPay = BasicPay - LeavePay;

                    TotalEarnings = (BasicPay + AdditionalPay) - BasicDeductions;
                }
            }
        }

        private void ComputeBasicHoursAndPay(
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
                    var workDaysPerPayPeriod =
                        employee.WorkDaysPerYear /
                        CalendarConstant.MonthsInAYear /
                        CalendarConstant.SemiMonthlyPayPeriodsPerMonth;

                    BasicHours = workDaysPerPayPeriod * 8;
                }
            }
            else if (employee.IsDaily)
            {
                BasicHours = ComputeBasicHoursForDaily(calendarCollection, timeEntries);
            }

            // Basic Pay
            ComputeBasicPay(employee.IsDaily, salary.BasicSalary, timeEntries);

            Actual.ComputeBasicPay(employee.IsDaily, salary.TotalSalary, actualTimeEntries);
        }

        private void ComputeHours(
            Employee employee,
            Salary salary,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<ActualTimeEntry> actualTimeEntries)
        {
            var totalTimeEntries = TotalTimeEntryCalculator.Calculate(timeEntries, salary, employee, actualTimeEntries);

            RegularHours = totalTimeEntries.RegularHours;
            RegularPay = totalTimeEntries.RegularPay;
            Actual.RegularPay = totalTimeEntries.ActualRegularPay;

            OvertimeHours = totalTimeEntries.OvertimeHours;
            OvertimePay = totalTimeEntries.OvertimePay;
            Actual.OvertimePay = totalTimeEntries.ActualOvertimePay;

            NightDiffHours = totalTimeEntries.NightDifferentialHours;
            NightDiffPay = totalTimeEntries.NightDiffPay;
            Actual.NightDiffPay = totalTimeEntries.ActualNightDiffPay;

            NightDiffOvertimeHours = totalTimeEntries.NightDifferentialOvertimeHours;
            NightDiffOvertimePay = totalTimeEntries.NightDiffOvertimePay;
            Actual.NightDiffOvertimePay = totalTimeEntries.ActualNightDiffOvertimePay;

            RestDayHours = totalTimeEntries.RestDayHours;
            RestDayPay = totalTimeEntries.RestDayPay;
            Actual.RestDayPay = totalTimeEntries.ActualRestDayPay;

            RestDayOTHours = totalTimeEntries.RestDayOTHours;
            RestDayOTPay = totalTimeEntries.RestDayOTPay;
            Actual.RestDayOTPay = totalTimeEntries.ActualRestDayOTPay;

            SpecialHolidayHours = totalTimeEntries.SpecialHolidayHours;
            SpecialHolidayPay = totalTimeEntries.SpecialHolidayPay;
            Actual.SpecialHolidayPay = totalTimeEntries.ActualSpecialHolidayPay;

            SpecialHolidayOTHours = totalTimeEntries.SpecialHolidayOTHours;
            SpecialHolidayOTPay = totalTimeEntries.SpecialHolidayOTPay;
            Actual.SpecialHolidayOTPay = totalTimeEntries.ActualSpecialHolidayOTPay;

            RegularHolidayHours = totalTimeEntries.RegularHolidayHours;
            RegularHolidayPay = totalTimeEntries.RegularHolidayPay;
            Actual.RegularHolidayPay = totalTimeEntries.ActualRegularHolidayPay;

            RegularHolidayOTHours = totalTimeEntries.RegularHolidayOTHours;
            RegularHolidayOTPay = totalTimeEntries.RegularHolidayOTPay;
            Actual.RegularHolidayOTPay = totalTimeEntries.ActualRegularHolidayOTPay;

            LeaveHours = totalTimeEntries.LeaveHours;
            LeavePay = totalTimeEntries.LeavePay;
            Actual.LeavePay = totalTimeEntries.ActualLeavePay;

            LateHours = totalTimeEntries.LateHours;
            LateDeduction = totalTimeEntries.LateDeduction;
            Actual.LateDeduction = totalTimeEntries.ActualLateDeduction;

            UndertimeHours = totalTimeEntries.UndertimeHours;
            UndertimeDeduction = totalTimeEntries.UndertimeDeduction;
            Actual.UndertimeDeduction = totalTimeEntries.ActualUndertimeDeduction;

            AbsentHours = totalTimeEntries.AbsentHours;
            AbsenceDeduction = totalTimeEntries.AbsenceDeduction;
            Actual.AbsenceDeduction = totalTimeEntries.ActualAbsenceDeduction;
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

                {
                    basicHours += timeEntry.WorkHours;
                }
            }

            return basicHours;
        }

        #endregion Payroll

        #region Composite Keys

        public class EmployeeCompositeKey
        {
            public int EmployeeId { get; set; }
            public int PayPeriodId { get; set; }

            public EmployeeCompositeKey(int employeeId, int payPeriodId)
            {
                EmployeeId = employeeId;
                PayPeriodId = payPeriodId;
            }
        }

        public class DateCompositeKey
        {
            public int OrganizationId { get; set; }
            public DateTime PayFromDate { get; set; }
            public DateTime PayToDate { get; set; }

            public DateCompositeKey(int organizationId, DateTime payFromDate, DateTime payToDate)
            {
                OrganizationId = organizationId;
                PayFromDate = payFromDate;
                PayToDate = payToDate;
            }
        }

        #endregion Composite Keys
    }
}
