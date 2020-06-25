using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class PaystubActualCalculator
    {
        public void Compute(
            Employee employee,
            Salary salary,
            ListOfValueCollection settings,
            PayPeriod payperiod,
            Paystub paystub,
            string currentSystemOwner,
            IReadOnlyCollection<ActualTimeEntry> actualTimeEntries)
        {
            decimal totalEarnings = 0;

            if (employee.IsDaily || currentSystemOwner == SystemOwnerService.Benchmark)
            {
                totalEarnings = paystub.Actual.RegularPay +
                                paystub.Actual.LeavePay +
                                paystub.Actual.AdditionalPay;
            }
            else if (employee.IsFixed)
            {
                var monthlyRate = PayrollTools.
                                    GetEmployeeMonthlyRate(employee, salary, isActual: true);
                var basicPay = monthlyRate / 2;

                totalEarnings = basicPay + paystub.Actual.AdditionalPay;
            }
            else if (employee.IsMonthly)
            {
                var isFirstPayAsDailyRule = settings.
                                GetBoolean("Payroll Policy", "isfirstsalarydaily");

                var isFirstPay = payperiod.PayFromDate <= employee.StartDate &&
                                employee.StartDate <= payperiod.PayToDate;

                if (isFirstPay && isFirstPayAsDailyRule)
                {
                    totalEarnings = paystub.Actual.RegularPay +
                                    paystub.Actual.LeavePay +
                                    paystub.Actual.AdditionalPay;
                }
                else
                {
                    var monthlyRate = PayrollTools.
                                    GetEmployeeMonthlyRate(employee, salary, isActual: true);
                    var basicPay = monthlyRate / 2;

                    paystub.Actual.RegularPay = basicPay - paystub.Actual.LeavePay;

                    totalEarnings = paystub.Actual.RegularPay +
                                    paystub.Actual.LeavePay +
                                    paystub.Actual.AdditionalPay -
                                    paystub.Actual.BasicDeductions;
                }
            }

            paystub.Actual.ComputeBasicPay(employee.IsDaily, salary.TotalSalary, actualTimeEntries);
            paystub.Actual.GrossPay = AccuMath.CommercialRound(totalEarnings + paystub.TotalAllowance);
            paystub.Actual.TotalAdjustments = AccuMath.CommercialRound(paystub.TotalAdjustments + paystub.ActualAdjustments.Sum(a => a.Amount));
            paystub.Actual.NetPay = AccuMath.CommercialRound(paystub.Actual.GrossPay - paystub.NetDeductions + paystub.Actual.TotalAdjustments);
        }
    }
}