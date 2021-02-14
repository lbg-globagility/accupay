using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class PhilHealthCalculator
    {
        private readonly IPhilHealthPolicy _policy;

        public PhilHealthCalculator(IPhilHealthPolicy policy)
        {
            _policy = policy;
        }

        public void Calculate(
            Salary salary,
            Paystub paystub,
            Paystub previousPaystub,
            Employee employee,
            PayPeriod payperiod,
            string currentSystemOwner)
        {
            // Reset the PhilHealth to zero
            paystub.PhilHealthEmployeeShare = 0;
            paystub.PhilHealthEmployerShare = 0;

            decimal totalContribution;

            // If auto compute the PhilHealth is true, then we use the available formulas to compute the total contribution.
            // Otherwise, we use whatever amount is set in the salary.
            if (salary.AutoComputePhilHealthContribution)
            {
                totalContribution = GetTotalContribution(
                    salary,
                    paystub,
                    previousPaystub,
                    employee,
                    currentSystemOwner);
            }
            else
            {
                totalContribution = salary.PhilHealthDeduction;
            }

            // If totalContribution is zero, then the employee has no PhilHealth to pay
            if (totalContribution <= 0)
                return;

            var halfContribution = AccuMath.Truncate(totalContribution / 2, 2);

            var employeeShare = halfContribution;
            var employerShare = halfContribution;

            // Account for any division loss by putting the missing value to the employer's share
            if (_policy.OddCentDifference)
            {
                var expectedTotal = halfContribution * 2;
                decimal remainder = 0;

                if (expectedTotal < totalContribution)
                    remainder = totalContribution - expectedTotal;

                employerShare += remainder;
            }

            if (employee.IsWeeklyPaid)
            {
                var is_deduct_sched_to_thisperiod =
                        employee.IsUnderAgency ?
                        payperiod.PhHWeeklyAgentContribSched :
                        payperiod.PhHWeeklyContribSched;

                if (is_deduct_sched_to_thisperiod)
                {
                    paystub.PhilHealthEmployeeShare = employeeShare;
                    paystub.PhilHealthEmployerShare = employerShare;
                }
            }
            else
            {
                var deductionSchedule = employee.PhilHealthSchedule;

                if (IsPhilHealthPaidOnFirstHalf(deductionSchedule, payperiod) ||
                    IsPhilHealthPaidOnEndOfTheMonth(deductionSchedule, payperiod))
                {
                    paystub.PhilHealthEmployeeShare = employeeShare;
                    paystub.PhilHealthEmployerShare = employerShare;
                }
                else if (IsPhilHealthPaidPerPayPeriod(deductionSchedule))
                {
                    paystub.PhilHealthEmployeeShare =
                        employeeShare / CalendarConstant.SemiMonthlyPayPeriodsPerMonth;
                    paystub.PhilHealthEmployerShare =
                        employerShare / CalendarConstant.SemiMonthlyPayPeriodsPerMonth;
                }
            }
        }

        private decimal GetTotalContribution(
            Salary salary,
            Paystub paystub,
            Paystub previousPaystub,
            Employee employee,
            string currentSystemOwner)
        {
            var calculationBasis = _policy.CalculationBasis(employee.OrganizationID.Value);

            var basisPay = 0M;

            switch (calculationBasis)
            {
                case PhilHealthCalculationBasis.BasicSalary:
                    basisPay = PayrollTools.GetEmployeeMonthlyRate(employee, salary);
                    break;

                case PhilHealthCalculationBasis.BasicMinusDeductions:

                    if (employee.IsFixed)
                    {
                        basisPay = PayrollTools.GetEmployeeMonthlyRate(employee, salary);
                    }
                    else
                    {
                        basisPay = (previousPaystub?.TotalDaysPayWithOutOvertimeAndLeave ?? 0) +
                        paystub.TotalDaysPayWithOutOvertimeAndLeave;
                    }
                    break;

                case PhilHealthCalculationBasis.BasicMinusDeductionsWithoutPremium:

                    if (employee.IsFixed)
                    {
                        basisPay = PayrollTools.GetEmployeeMonthlyRate(employee, salary);
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

                        basisPay = totalHours * hourlyRate;
                    }
                    break;

                case PhilHealthCalculationBasis.Earnings:
                    basisPay = (previousPaystub?.TotalEarnings ?? 0) + paystub.TotalEarnings;
                    break;

                case PhilHealthCalculationBasis.GrossPay:
                    basisPay = (previousPaystub?.GrossPay ?? 0) + paystub.GrossPay;
                    break;

                default:
                    break;
            }

            var totalContribution = ComputePhilHealth(basisPay);

            return totalContribution;
        }

        private decimal ComputePhilHealth(decimal basis)
        {
            var minimum = _policy.MinimumContribution;
            var maximum = _policy.MaximumContribution;
            var rate = _policy.Rate / 100;

            // Contribution should be bounded by the minimum and maximum
            var contribution = new decimal[] {
                new decimal[] { basis * rate, minimum }.Max(),
                maximum
            }.Min();
            // Round to the nearest cent
            contribution = AccuMath.CommercialRound(contribution);

            return contribution;
        }

        private bool IsPhilHealthPaidOnFirstHalf(string deductionSchedule, PayPeriod payperiod)
        {
            return payperiod.IsFirstHalf &&
                deductionSchedule == ContributionSchedule.FIRST_HALF;
        }

        private bool IsPhilHealthPaidOnEndOfTheMonth(string deductionSchedule, PayPeriod payperiod)
        {
            return payperiod.IsEndOfTheMonth &&
                deductionSchedule == ContributionSchedule.END_OF_THE_MONTH;
        }

        private bool IsPhilHealthPaidPerPayPeriod(string deductionSchedule)
        {
            return deductionSchedule == ContributionSchedule.PER_PAY_PERIOD;
        }
    }
}
