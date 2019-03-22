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

        Public Sub Calculate(salary As Salary, paystub As Paystub, employee As Employee, payperiod As PayPeriod)
            Dim deductionSchedule = employee.PagIBIGSchedule

            Dim employeeShare = salary.HDMFAmount
            Dim employerShare = If(employeeShare = 0, 0, PagibigEmployerAmount)

            If employee.IsWeeklyPaid Then
                Dim isOnScheduleForDeduction = If(
                    employee.IsUnderAgency,
                    payperiod.HDMFWeeklyAgentContribSched,
                    payperiod.HDMFWeeklyContribSched)

                If isOnScheduleForDeduction Then
                    paystub.HdmfEmployeeShare = employeeShare
                    paystub.HdmfEmployerShare = employerShare
                Else
                    paystub.HdmfEmployeeShare = 0
                    paystub.HdmfEmployerShare = 0
                End If
            Else
                If IsHdmfPaidOnFirstHalf(deductionSchedule, payperiod) Or
                    IsHdmfPaidOnEndOfTheMonth(deductionSchedule, payperiod) Then

                    paystub.HdmfEmployeeShare = employeeShare
                    paystub.HdmfEmployerShare = employerShare
                ElseIf IsHdmfPaidPerPayPeriod(deductionSchedule) Then
                    paystub.HdmfEmployeeShare = employeeShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
                    paystub.HdmfEmployerShare = employerShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
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
