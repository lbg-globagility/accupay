using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class SssCalculator
    {
        private readonly ListOfValueCollection _settings;

        private readonly IReadOnlyCollection<SocialSecurityBracket> _socialSecurityBrackets;

        public SssCalculator(ListOfValueCollection settings, IReadOnlyCollection<SocialSecurityBracket> socialSecurityBrackets)
        {
            _settings = settings;
            _socialSecurityBrackets = socialSecurityBrackets;
        }

        public void Calculate(Paystub paystub,
                            Paystub previousPaystub,
                            Salary salary,
                            Employee employee,
                            PayPeriod payperiod,
                            string currentSystemOwner)
        {
            // Reset SSS values to zero
            paystub.SssEmployeeShare = 0;
            paystub.SssEmployerShare = 0;

            // If salary is is set not to pay sss, return.
            if (!salary.DoPaySSSContribution)
                return;

            // Get the social security bracket based on the amount earned.
            var amount = GetSocialSecurityAmount(paystub,
                                                previousPaystub,
                                                salary,
                                                employee,
                                                currentSystemOwner);
            var socialSecurityBracket = FindMatchingBracket(amount);

            // If no bracket was matched/found, then there's nothing to compute.
            if (socialSecurityBracket == null)
                return;

            var employeeShare = socialSecurityBracket.EmployeeContributionAmount;
            var employerShare = socialSecurityBracket.EmployerContributionAmount + socialSecurityBracket.EmployeeECAmount;

            if (employee.IsWeeklyPaid)
            {
                var shouldDeduct = employee.IsUnderAgency ? payperiod.SSSWeeklyAgentContribSched : payperiod.SSSWeeklyContribSched;

                if (shouldDeduct)
                {
                    paystub.SssEmployeeShare = employeeShare;
                    paystub.SssEmployerShare = employerShare;
                }
            }
            else
            {
                var deductionSchedule = employee.SssSchedule;

                if (IsSssPaidOnFirstHalf(payperiod, deductionSchedule) ||
                    IsSssPaidOnEndOfTheMonth(payperiod, deductionSchedule))
                {
                    paystub.SssEmployeeShare = employeeShare;
                    paystub.SssEmployerShare = employerShare;
                }
                else if (IsSssPaidPerPayPeriod(deductionSchedule))
                {
                    paystub.SssEmployeeShare = employeeShare /
                                                CalendarConstants.SemiMonthlyPayPeriodsPerMonth;
                    paystub.SssEmployerShare = employerShare /
                                                CalendarConstants.SemiMonthlyPayPeriodsPerMonth;
                }
            }
        }

        private SocialSecurityBracket FindMatchingBracket(decimal amount)
        {
            return _socialSecurityBrackets.FirstOrDefault(s => s.RangeFromAmount <= amount &&
                                                                s.RangeToAmount >= amount);
        }

        private decimal GetSocialSecurityAmount(Paystub paystub,
                                                Paystub previousPaystub,
                                                Salary salary,
                                                Employee employee,
                                                string currentSystemOwner)
        {
            var policyByOrganization = _settings.GetBoolean("Policy.ByOrganization", false);

            var calculationBasis = _settings.GetEnum("SocialSecuritySystem.CalculationBasis", SssCalculationBasis.BasicSalary, policyByOrganization);

            switch (calculationBasis)
            {
                case SssCalculationBasis.BasicSalary:
                    return PayrollTools.GetEmployeeMonthlyRate(employee, salary);

                case SssCalculationBasis.Earnings:
                    return (previousPaystub?.TotalEarnings ?? 0) + paystub.TotalEarnings;

                case SssCalculationBasis.GrossPay:
                    return (previousPaystub?.GrossPay ?? 0) + paystub.GrossPay;

                case SssCalculationBasis.BasicMinusDeductions:
                    return (previousPaystub?.TotalDaysPayWithOutOvertimeAndLeave ?? 0) +
                            paystub.TotalDaysPayWithOutOvertimeAndLeave;

                case SssCalculationBasis.BasicMinusDeductionsWithoutPremium:
                    var totalHours = (previousPaystub?.TotalWorkedHoursWithoutOvertimeAndLeave ?? 0) +
                                        paystub.TotalWorkedHoursWithoutOvertimeAndLeave;

                    if (currentSystemOwner == SystemOwnerService.Benchmark && employee.IsPremiumInclusive)
                    {
                        totalHours = (previousPaystub?.RegularHoursAndTotalRestDay ?? 0) +
                                        paystub.RegularHoursAndTotalRestDay;
                    }

                    var monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary);
                    var dailyRate = PayrollTools.GetDailyRate(monthlyRate, employee.WorkDaysPerYear);
                    var hourlyRate = PayrollTools.GetHourlyRateByDailyRate(dailyRate);

                    return totalHours * hourlyRate;

                default:
                    return 0;
            }
        }

        private bool IsSssPaidOnFirstHalf(PayPeriod payperiod, string deductionSchedule)
        {
            return payperiod.IsFirstHalf && deductionSchedule == ContributionSchedule.FIRST_HALF;
        }

        private bool IsSssPaidOnEndOfTheMonth(PayPeriod payperiod, string deductionSchedule)
        {
            return payperiod.IsEndOfTheMonth && deductionSchedule == ContributionSchedule.END_OF_THE_MONTH;
        }

        private bool IsSssPaidPerPayPeriod(string deductionSchedule)
        {
            return deductionSchedule == ContributionSchedule.PER_PAY_PERIOD;
        }
    }
}