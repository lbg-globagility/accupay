using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Linq;
using System.Text.RegularExpressions;

namespace AccuPay.Core.Services
{
    public class HdmfCalculator
    {
        public const decimal StandardEmployeeContribution = 100;

        private const decimal StandardEmployerContribution = 100;

        public void Calculate(Salary salary, Paystub paystub, Employee employee, ListOfValueCollection settings, PayPeriod payperiod)
        {
            // Reset HDMF contribution
            paystub.HdmfEmployeeShare = 0;
            paystub.HdmfEmployerShare = 0;

            decimal employeeShare;

            // If HDMF autocomputation is true, employee share is the Standard contribution.
            // Otherwise, use whatever is set in the hdmf contribution in the salary
            if (salary.AutoComputeHDMFContribution)
                employeeShare = settings.GetDecimal(name: "Default Government Contribution.HDMF", @default: StandardEmployeeContribution);
            else
                employeeShare = salary.HDMFAmount;

            var employerShare = employeeShare == 0 ? 0 : settings.GetDecimal(name: "Default Government Contribution.HDMF", @default: StandardEmployerContribution);

            if (employee.IsWeeklyPaid)
            {
                var isOnScheduleForDeduction = employee.IsUnderAgency ? payperiod.HDMFWeeklyAgentContribSched : payperiod.HDMFWeeklyContribSched;

                if (isOnScheduleForDeduction)
                {
                    paystub.HdmfEmployeeShare = employeeShare;
                    paystub.HdmfEmployerShare = employerShare;
                }
            }
            else
            {
                var deductionSchedule = employee.PagIBIGSchedule;

                if (IsHdmfPaidOnFirstHalf(deductionSchedule, payperiod) ||
                    IsHdmfPaidOnEndOfTheMonth(deductionSchedule, payperiod))
                {
                    paystub.HdmfEmployeeShare = employeeShare;
                    paystub.HdmfEmployerShare = employerShare;
                }
                else if (IsHdmfPaidPerPayPeriod(deductionSchedule))
                {
                    paystub.HdmfEmployeeShare = employeeShare /
                                                CalendarConstant.SemiMonthlyPayPeriodsPerMonth;
                    paystub.HdmfEmployerShare = employerShare /
                                                CalendarConstant.SemiMonthlyPayPeriodsPerMonth;
                }
            }

            // Override employer share
            // make its value matching to the employee share
            var hdmfPolicy = "HDMF.EmployeesMatchingEmployerShare";
            var employeesMatchingEmployerShare = settings.GetStringOrNull(hdmfPolicy);

            if (!string.IsNullOrWhiteSpace(employeesMatchingEmployerShare))
            {
                var employees = Regex.Split(employeesMatchingEmployerShare, ",");

                if (employees.Where(e => e.Trim() == employee.EmployeeNo).Any())
                    paystub.HdmfEmployerShare = paystub.HdmfEmployeeShare;
            }
        }

        private bool IsHdmfPaidOnFirstHalf(string deductionSchedule, PayPeriod payperiod)
        {
            return payperiod.IsFirstHalf &&
                deductionSchedule == ContributionSchedule.FIRST_HALF;
        }

        private bool IsHdmfPaidOnEndOfTheMonth(string deductionSchedule, PayPeriod payperiod)
        {
            return payperiod.IsEndOfTheMonth &&
                deductionSchedule == ContributionSchedule.END_OF_THE_MONTH;
        }

        private bool IsHdmfPaidPerPayPeriod(string deductionSchedule)
        {
            return deductionSchedule == ContributionSchedule.PER_PAY_PERIOD;
        }
    }
}
