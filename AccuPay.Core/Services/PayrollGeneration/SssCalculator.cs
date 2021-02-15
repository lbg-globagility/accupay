using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class SssCalculator
    {
        private readonly IPolicyHelper _policy;
        private readonly PayPeriod _payPeriod;
        private readonly IReadOnlyCollection<SocialSecurityBracket> _socialSecurityBrackets;

        public SssCalculator(IPolicyHelper policy, IReadOnlyCollection<SocialSecurityBracket> socialSecurityBrackets, PayPeriod payPeriod)
        {
            _policy = policy;
            _payPeriod = payPeriod;

            // brackets are already filtered in PayrollResources
            // but still better to filter here so SSSCalculator
            // can be sure that it has the right data
            _socialSecurityBrackets = socialSecurityBrackets
                .Where(x => payPeriod.DateMonth >= x.EffectiveDateFrom)
                .Where(x => payPeriod.DateMonth <= x.EffectiveDateTo)
                .ToList();
        }

        public void Calculate(
            Paystub paystub,
            Paystub previousPaystub,
            Salary salary,
            Employee employee,
            string currentSystemOwner)
        {
            // Reset SSS values to zero
            paystub.SssEmployeeShare = 0;
            paystub.SssEmployerShare = 0;

            // If salary is set not to pay sss, return.
            if (!salary.DoPaySSSContribution)
                return;

            // Get the social security bracket based on the amount earned.
            var amount = GetSocialSecurityAmount(
                paystub,
                previousPaystub,
                salary,
                employee,
                currentSystemOwner);
            var socialSecurityBracket = FindMatchingBracket(amount);

            // If no bracket was matched/found, then there's nothing to compute.
            if (socialSecurityBracket == null)
                return;

            var employeeShare = socialSecurityBracket.EmployeeContributionAmount + socialSecurityBracket.EmployeeMPFAmount;
            var employerShare = socialSecurityBracket.EmployerContributionAmount + socialSecurityBracket.EmployerECAmount + socialSecurityBracket.EmployerMPFAmount;

            if (employee.IsWeeklyPaid)
            {
                var shouldDeduct = employee.IsUnderAgency ? _payPeriod.SSSWeeklyAgentContribSched : _payPeriod.SSSWeeklyContribSched;

                if (shouldDeduct)
                {
                    paystub.SssEmployeeShare = employeeShare;
                    paystub.SssEmployerShare = employerShare;
                }
            }
            else
            {
                var deductionSchedule = employee.SssSchedule;

                if (IsSssPaidOnFirstHalf(_payPeriod, deductionSchedule) ||
                    IsSssPaidOnEndOfTheMonth(_payPeriod, deductionSchedule))
                {
                    paystub.SssEmployeeShare = employeeShare;
                    paystub.SssEmployerShare = employerShare;
                }
                else if (IsSssPaidPerPayPeriod(deductionSchedule))
                {
                    paystub.SssEmployeeShare =
                        employeeShare / CalendarConstant.SemiMonthlyPayPeriodsPerMonth;
                    paystub.SssEmployerShare =
                        employerShare / CalendarConstant.SemiMonthlyPayPeriodsPerMonth;
                }
            }
        }

        private SocialSecurityBracket FindMatchingBracket(decimal amount)
        {
            return _socialSecurityBrackets
                .FirstOrDefault(s => s.RangeFromAmount <= amount && s.RangeToAmount >= amount);
        }

        private decimal GetSocialSecurityAmount(
            Paystub paystub,
            Paystub previousPaystub,
            Salary salary,
            Employee employee,
            string currentSystemOwner)
        {
            switch (_policy.SssCalculationBasis(employee.OrganizationID.Value))
            {
                case SssCalculationBasis.BasicSalary:
                    return PayrollTools.GetEmployeeMonthlyRate(employee, salary);

                case SssCalculationBasis.BasicMinusDeductions:

                    if (employee.IsFixed)
                    {
                        return PayrollTools.GetEmployeeMonthlyRate(employee, salary);
                    }
                    else
                    {
                        return (previousPaystub?.TotalDaysPayWithOutOvertimeAndLeave(employee.IsMonthly) ?? 0) +
                            paystub.TotalDaysPayWithOutOvertimeAndLeave(employee.IsMonthly);
                    }

                case SssCalculationBasis.BasicMinusDeductionsWithoutPremium:

                    if (employee.IsFixed)
                    {
                        return PayrollTools.GetEmployeeMonthlyRate(employee, salary);
                    }
                    else
                    {
                        var totalHours = (previousPaystub?.TotalWorkedHoursWithoutOvertimeAndLeave(employee.IsMonthly) ?? 0) +
                            paystub.TotalWorkedHoursWithoutOvertimeAndLeave(employee.IsMonthly);

                        if (currentSystemOwner == SystemOwner.Benchmark && employee.IsPremiumInclusive)
                        {
                            totalHours = (previousPaystub?.RegularHoursAndTotalRestDay ?? 0) +
                                paystub.RegularHoursAndTotalRestDay;
                        }

                        var monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary);
                        var dailyRate = PayrollTools.GetDailyRate(monthlyRate, employee.WorkDaysPerYear);
                        var hourlyRate = PayrollTools.GetHourlyRateByDailyRate(dailyRate);

                        return totalHours * hourlyRate;
                    }

                case SssCalculationBasis.Earnings:
                    return (previousPaystub?.TotalEarnings ?? 0) + paystub.TotalEarnings;

                case SssCalculationBasis.GrossPay:
                    return (previousPaystub?.GrossPay ?? 0) + paystub.GrossPay;

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
