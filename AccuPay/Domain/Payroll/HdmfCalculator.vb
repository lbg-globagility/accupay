Option Strict On

Imports AccuPay.Entity
Imports PayrollSys

Namespace Global.AccuPay.Payroll

    Public Class HdmfCalculator

        Private Class ContributionSchedule
            Public Const FirstHalf As String = "First half"
            Public Const EndOfTheMonth As String = "End of the month"
            Public Const PerPayPeriod As String = "Per pay period"
        End Class

        Private Const PagibigEmployerAmount As Decimal = 100

        Public Sub Calculate(salary As Salary, paystub As Paystub, _employee As DataRow, employee2 As Employee, payperiod As PayPeriod)
            Dim deductionSchedule = _employee("HDMFDeductSched").ToString

            Dim employeeHdmfPerMonth = salary.HDMFAmount
            Dim employerHdmfPerMonth = If(employeeHdmfPerMonth = 0, 0, PagibigEmployerAmount)

            If deductionSchedule = ContributionSchedule.PerPayPeriod Then
                Dim payPeriodsPerMonth = CDec(ValNoComma(_employee("PAYFREQUENCY_DIVISOR")))

                employeeHdmfPerMonth = payPeriodsPerMonth
                employerHdmfPerMonth = payPeriodsPerMonth
            End If

            Dim isWeekly As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsWeeklyPaid")))

            If isWeekly Then
                Dim isOnScheduleForDeduction = If(
                    employee2.IsUnderAgency,
                    payperiod.HDMFWeeklyAgentContribSched,
                    payperiod.HDMFWeeklyContribSched)

                If isOnScheduleForDeduction Then
                    paystub.HdmfEmployeeShare = employeeHdmfPerMonth
                    paystub.HdmfEmployerShare = employerHdmfPerMonth
                Else
                    paystub.HdmfEmployeeShare = 0
                    paystub.HdmfEmployerShare = 0
                End If
            Else
                If IsHdmfPaidOnFirstHalf(deductionSchedule, payperiod) Or
                    IsHdmfPaidOnEndOfTheMonth(deductionSchedule, payperiod) Or
                    IsHdmfPaidPerPayPeriod(deductionSchedule) Then

                    paystub.HdmfEmployeeShare = employeeHdmfPerMonth
                    paystub.HdmfEmployerShare = employerHdmfPerMonth
                Else
                    paystub.HdmfEmployeeShare = 0
                    paystub.HdmfEmployerShare = 0
                End If
            End If
        End Sub

        Private Function IsHdmfPaidOnFirstHalf(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsFirstHalf And (deductionSchedule = ContributionSchedule.FirstHalf)
        End Function

        Private Function IsHdmfPaidOnEndOfTheMonth(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsEndOfTheMonth And (deductionSchedule = ContributionSchedule.EndOfTheMonth)
        End Function

        Private Function IsHdmfPaidPerPayPeriod(deductionSchedule As String) As Boolean
            Return deductionSchedule = ContributionSchedule.PerPayPeriod
        End Function

    End Class

End Namespace
