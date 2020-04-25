Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.Services
Imports AccuPay.Entity
Imports AccuPay.Utilities
Imports PayrollSys

Namespace Global.AccuPay.Payroll

    Public Class PaystubActualCalculator

        Public Sub Compute(employee As Entities.Employee,
                           salary As Salary,
                           settings As ListOfValueCollection,
                           payperiod As PayPeriod,
                           paystub As Paystub)
            Dim totalEarnings As Decimal

            If employee.IsDaily OrElse (New SystemOwner).CurrentSystemOwner = SystemOwner.Benchmark Then

                totalEarnings = paystub.Actual.RegularPay + paystub.Actual.LeavePay + paystub.Actual.AdditionalPay

            ElseIf employee.IsFixed Then

                Dim monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary, isActual:=True)
                Dim basicPay = monthlyRate / 2

                totalEarnings = basicPay + paystub.Actual.AdditionalPay

            ElseIf employee.IsMonthly Then

                Dim isFirstPayAsDailyRule = settings.GetBoolean("Payroll Policy", "isfirstsalarydaily")

                Dim isFirstPay =
                    payperiod.PayFromDate <= employee.StartDate And
                    employee.StartDate <= payperiod.PayToDate

                If isFirstPay And isFirstPayAsDailyRule Then

                    totalEarnings = paystub.Actual.RegularPay + paystub.Actual.LeavePay + paystub.Actual.AdditionalPay
                Else

                    Dim monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary, isActual:=True)
                    Dim basicPay = monthlyRate / 2

                    paystub.Actual.RegularPay = basicPay - paystub.Actual.LeavePay

                    totalEarnings = paystub.Actual.RegularPay + paystub.Actual.LeavePay + paystub.Actual.AdditionalPay - paystub.Actual.BasicDeductions
                End If
            End If

            paystub.Actual.TotalEarnings = AccuMath.CommercialRound(totalEarnings)
            paystub.Actual.GrossPay = AccuMath.CommercialRound(totalEarnings + paystub.TotalAllowance)
            paystub.Actual.TotalAdjustments = AccuMath.CommercialRound(paystub.TotalAdjustments + paystub.ActualAdjustments.Sum(Function(a) a.PayAmount))
            paystub.Actual.NetPay = AccuMath.CommercialRound(paystub.Actual.GrossPay - paystub.NetDeductions + paystub.Actual.TotalAdjustments)
        End Sub

    End Class

End Namespace