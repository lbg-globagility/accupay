using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Utilities;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class PaystubActualCalculator
    {
        public void Compute(
            Employee employee,
            Salary salary,
            ListOfValueCollection settings,
            PayPeriod payperiod,
            Paystub paystub,
            string currentSystemOwner)
        {
            decimal totalEarnings = 0;

            // TODO: move this code to BasePaystub class so paystub and paystubactual
            // can both use this formula
            if (employee.IsDaily || currentSystemOwner == SystemOwner.Benchmark)
            {
                totalEarnings = paystub.Actual.TotalEarningForDaily;
            }
            else if (employee.IsFixed)
            {
                var monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary, isActual: true);
                var basicPay = monthlyRate / 2;

                totalEarnings = basicPay + paystub.Actual.AdditionalPay;
            }
            else if (employee.IsMonthly)
            {
                var isFirstPayAsDailyRule = settings.GetBoolean("Payroll Policy", "isfirstsalarydaily");

                if (employee.IsFirstPay(payperiod) && isFirstPayAsDailyRule)
                {
                    totalEarnings = paystub.Actual.TotalEarningForDaily;
                }
                else
                {
                    var monthlyRate = PayrollTools
                        .GetEmployeeMonthlyRate(employee, salary, isActual: true);

                    var basicPay = monthlyRate / 2;

                    paystub.Actual.RegularPay = basicPay - paystub.Actual.LeavePay;

                    totalEarnings =
                        paystub.Actual.RegularPay +
                        paystub.Actual.LeavePay +
                        paystub.Actual.AdditionalPay -
                        paystub.Actual.BasicDeductions;
                }
            }

            paystub.Actual.GrossPay = AccuMath.CommercialRound(totalEarnings + paystub.TotalBonus + paystub.GrandTotalAllowance);
            paystub.Actual.TotalAdjustments = AccuMath.CommercialRound(paystub.TotalAdjustments + paystub.ActualAdjustments.Sum(a => a.Amount));
            paystub.Actual.NetPay = AccuMath.CommercialRound(paystub.Actual.GrossPay - paystub.NetDeductions + paystub.Actual.TotalAdjustments);
        }
    }
}
