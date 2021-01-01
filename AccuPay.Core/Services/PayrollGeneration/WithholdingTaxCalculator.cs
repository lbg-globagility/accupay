using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class WithholdingTaxCalculator
    {
        private readonly IReadOnlyCollection<WithholdingTaxBracket> _withholdingTaxBrackets;

        private readonly ListOfValueCollection _settings;

        public WithholdingTaxCalculator(ListOfValueCollection settings, IReadOnlyCollection<WithholdingTaxBracket> withholdingTaxBrackets)
        {
            _withholdingTaxBrackets = withholdingTaxBrackets;
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

            if (IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod) ||
                IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod))
            {
                paystub.TaxableIncome += (previousPaystub?.DeferredTaxableIncome ?? 0);
            }
            else if (IsWithholdingTaxPaidPerPayPeriod(deductionSchedule))
            {
                // Nothing to do here for now
            }

            // If the employee is earning minimum wage, then remove the taxable income.
            if (salary.IsMinimumWage)
                paystub.TaxableIncome = 0;

            // If the employee has no taxable income, then there's no need to compute for tax withheld.
            if (paystub.TaxableIncome <= 0)
                return;

            int? payFrequencyId = null;
            if (IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod) ||
                IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod))
            {
                payFrequencyId = PayFrequency.Monthly;
            }
            else if (IsWithholdingTaxPaidPerPayPeriod(deductionSchedule))
            {
                payFrequencyId = PayFrequency.SemiMonthly;
            }

            var taxBracket = GetTaxBracket(payFrequencyId, paystub, payperiod);

            paystub.WithholdingTax = GetTaxWithheld(taxBracket, paystub.TaxableIncome);
        }

        private decimal GetTaxWithheld(WithholdingTaxBracket bracket, decimal taxableIncome)
        {
            if (bracket == null)
                return 0;

            var excessAmount = taxableIncome - bracket.TaxableIncomeFromAmount;
            var taxWithheld = bracket.ExemptionAmount + (excessAmount * bracket.ExemptionInExcessAmount);

            return AccuMath.CommercialRound(taxWithheld);
        }

        private WithholdingTaxBracket GetTaxBracket(int? payFrequencyID, Paystub _paystub, PayPeriod _payperiod)
        {
            var taxEffectivityDate = new DateTime(_payperiod.Year, _payperiod.Month, 1);

            var possibleBrackets = _withholdingTaxBrackets.
                                        Where(w => w.PayFrequencyID == payFrequencyID).
                                        Where(w => w.EffectiveDateFrom <= taxEffectivityDate).
                                        Where(w => taxEffectivityDate <= w.EffectiveDateTo).
                                        Where(w => w.TaxableIncomeFromAmount < _paystub.TaxableIncome).
                                        Where(w => _paystub.TaxableIncome <= w.TaxableIncomeToAmount).
                                        ToList();

            return possibleBrackets.FirstOrDefault();
        }

        private bool IsWithholdingTaxPaidOnFirstHalf(string deductionSchedule, PayPeriod payperiod)
        {
            return payperiod.IsFirstHalf && deductionSchedule == ContributionSchedule.FIRST_HALF;
        }

        private bool IsWithholdingTaxPaidOnEndOfTheMonth(string deductionSchedule, PayPeriod payperiod)
        {
            return payperiod.IsEndOfTheMonth && deductionSchedule == ContributionSchedule.END_OF_THE_MONTH;
        }

        private bool IsWithholdingTaxPaidPerPayPeriod(string deductionSchedule)
        {
            return deductionSchedule == ContributionSchedule.PER_PAY_PERIOD;
        }

        private bool IsScheduledForTaxation(string deductionSchedule, PayPeriod payperiod)
        {
            return (payperiod.IsFirstHalf &&
                        IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod)) ||
                    (payperiod.IsEndOfTheMonth &&
                        IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod)) ||
                    IsWithholdingTaxPaidPerPayPeriod(deductionSchedule);
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