using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class WithholdingTaxCalculator
    {
        private readonly IReadOnlyCollection<FilingStatusType> _filingStatuses;

        private readonly IReadOnlyCollection<WithholdingTaxBracket> _withholdingTaxBrackets;

        private readonly IReadOnlyCollection<DivisionMinimumWage> _divisionMinimumWages;

        private readonly ListOfValueCollection _settings;

        public WithholdingTaxCalculator(ListOfValueCollection settings, IReadOnlyCollection<FilingStatusType> filingStatuses, IReadOnlyCollection<WithholdingTaxBracket> withholdingTaxBrackets, IReadOnlyCollection<DivisionMinimumWage> divisionMinimumWages)
        {
            _filingStatuses = filingStatuses;
            _withholdingTaxBrackets = withholdingTaxBrackets;
            _divisionMinimumWages = divisionMinimumWages;
            _settings = settings;
        }

        public void Calculate(Paystub paystub, Paystub previousPaystub, Employee employee, PayPeriod payperiod, Salary salary)
        {
            // Reset the tax value before starting
            paystub.DeferredTaxableIncome = 0;
            paystub.TaxableIncome = 0;
            paystub.WithholdingTax = 0;

            var currentTaxableIncome = 0M;

            if (employee.EmployeeType == SalaryType.Fixed)
                currentTaxableIncome = paystub.BasicPay;
            else if (employee.EmployeeType == SalaryType.Monthly)
            {
                var taxablePolicy = _settings.GetString("Payroll Policy.paystub.taxableincome") ?? "Basic Pay";

                if (taxablePolicy == "Gross Income")
                    // Adds those taxable allowances to the taxable income
                    currentTaxableIncome = paystub.TotalEarnings + paystub.TotalTaxableAllowance;
                else
                    currentTaxableIncome = paystub.BasicPay;
            }

            // Government contributions are tax deductible
            currentTaxableIncome -= paystub.GovernmentDeductions;
            // Taxable income should not be less than zero
            paystub.TaxableIncome = new decimal[] { currentTaxableIncome, 0 }.Max();

            var deductionSchedule = employee.WithholdingTaxSchedule;
            // Check if the current pay period is scheduled for taxation. If not, put the
            // taxable income as `Deferred` to be added on the taxable income in the next period.
            if (!IsScheduledForTaxation(deductionSchedule, payperiod))
            {
                paystub.DeferredTaxableIncome = paystub.TaxableIncome;
                paystub.TaxableIncome = 0;

                return;
            }

            if (IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod) | IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod))
                paystub.TaxableIncome += previousPaystub?.DeferredTaxableIncome ?? 0;
            else if (IsWithholdingTaxPaidPerPayPeriod(deductionSchedule))
            {
            }

            var dailyRate = employee.IsDaily ? salary.BasicSalary :
                                            salary.BasicSalary / (employee.WorkDaysPerYear / 12);

            // Round the daily rate to two decimal places since amounts in the 3rd decimal place
            // isn't significant enough to warrant the employee to be taxable.
            dailyRate = AccuMath.CommercialRound(dailyRate, 2);

            // If the employee is earning below the minimum wage, then remove the taxable income.
            var minimumWage = GetCurrentMinimumWage(employee);
            if (dailyRate <= minimumWage)
                paystub.TaxableIncome = 0;

            // If the employee has no taxable income, then there's no need to compute for tax withheld.
            if (paystub.TaxableIncome <= 0)
                return;

            int? payFrequencyId = null;
            if (IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod) | IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod))
                payFrequencyId = PayFrequency.Monthly;
            else if (IsWithholdingTaxPaidPerPayPeriod(deductionSchedule))
                payFrequencyId = PayFrequency.SemiMonthly;

            var filingStatusId = GetFilingStatusID(employee.MaritalStatus, employee.NoOfDependents);
            var taxBracket = GetTaxBracket(payFrequencyId, filingStatusId, paystub, payperiod);

            paystub.WithholdingTax = GetTaxWithheld(taxBracket, paystub.TaxableIncome);
        }

        private decimal GetCurrentMinimumWage(Employee employee)
        {
            var divisionMinimumWage = _divisionMinimumWages?.FirstOrDefault(t => Nullable.Equals(t.DivisionID, employee.Position?.DivisionID));
            var minimumWage = divisionMinimumWage?.Amount ?? 0;

            return minimumWage;
        }

        private decimal GetTaxWithheld(WithholdingTaxBracket bracket, decimal taxableIncome)
        {
            if (bracket == null)
                return 0;

            var excessAmount = taxableIncome - bracket.TaxableIncomeFromAmount;
            var taxWithheld = bracket.ExemptionAmount + (excessAmount * bracket.ExemptionInExcessAmount);

            return AccuMath.CommercialRound(taxWithheld);
        }

        private int GetFilingStatusID(string maritalStatus, int? noOfDependents)
        {
            var filingStatus = _filingStatuses.
                                Where(x => x.MaritalStatus == maritalStatus).
                                Where(x => x.Dependents <= (noOfDependents ?? 0)).
                                OrderByDescending(x => x.Dependents).
                                FirstOrDefault();

            var filingStatusID = 1;
            if (filingStatus != null)
                filingStatusID = filingStatus.RowID;

            return filingStatusID;
        }

        private WithholdingTaxBracket GetTaxBracket(int? payFrequencyID, int? filingStatusID, Paystub _paystub, PayPeriod _payperiod)
        {
            var taxEffectivityDate = new DateTime(_payperiod.Year, _payperiod.Month, 1);

            var possibleBrackets = _withholdingTaxBrackets.Where(w => w.PayFrequencyID.Value == payFrequencyID.Value).Where(w => w.EffectiveDateFrom <= taxEffectivityDate & taxEffectivityDate <= w.EffectiveDateTo).Where(w => w.TaxableIncomeFromAmount < _paystub.TaxableIncome & _paystub.TaxableIncome <= w.TaxableIncomeToAmount).ToList();

            // If there are more than one tax brackets that matches the previous list, filter by
            // the tax filing status.
            if (possibleBrackets.Count > 1)
                return possibleBrackets.Where(b => Nullable.Equals(b.FilingStatusID, filingStatusID)).FirstOrDefault();
            else if (possibleBrackets.Count == 1)
                return possibleBrackets.First();

            return null/* TODO Change to default(_) if this is not a reference type */;
        }

        private bool IsWithholdingTaxPaidOnFirstHalf(string deductionSchedule, PayPeriod payperiod)
        {
            return payperiod.IsFirstHalf & (deductionSchedule == ContributionSchedule.FIRST_HALF);
        }

        private bool IsWithholdingTaxPaidOnEndOfTheMonth(string deductionSchedule, PayPeriod payperiod)
        {
            return payperiod.IsEndOfTheMonth & (deductionSchedule == ContributionSchedule.END_OF_THE_MONTH);
        }

        private bool IsWithholdingTaxPaidPerPayPeriod(string deductionSchedule)
        {
            return deductionSchedule == ContributionSchedule.PER_PAY_PERIOD;
        }

        private bool IsScheduledForTaxation(string deductionSchedule, PayPeriod payperiod)
        {
            return (payperiod.IsFirstHalf & IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod)) | (payperiod.IsEndOfTheMonth & IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod)) | IsWithholdingTaxPaidPerPayPeriod(deductionSchedule);
        }

        private class PayFrequency
        {
            public const int SemiMonthly = 1;
            public const int Monthly = 2;
        }

        private class SalaryType
        {
            public const string Fixed = "Fixed";
            public const string Monthly = "Monthly";
            public const string Daily = "Daily";
        }
    }
}