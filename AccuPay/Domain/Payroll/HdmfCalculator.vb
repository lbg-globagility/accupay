Option Strict On

Imports AccuPay.Entity
Imports PayrollSys

Namespace Global.AccuPay.Payroll

    Public Class HdmfCalculator

        Private Const StandardEmployeeContribution As Decimal = 100

        Private Const StandardEmployerContribution As Decimal = 100

        Public Sub Calculate(salary As Salary, paystub As Paystub, employee As Employee, payperiod As PayPeriod)
            ' Reset HDMF contribution
            paystub.HdmfEmployeeShare = 0
            paystub.HdmfEmployerShare = 0

            Dim employeeShare As Decimal

            ' If HDMF autocomputation is true, employee share is the Standard contribution.
            ' Otherwise, use whatever is set in the hdmf contribution in the salary
            If salary.AutoComputeHDMFContribution Then
                employeeShare = StandardEmployeeContribution
            Else
                employeeShare = salary.HDMFAmount
            End If

            Dim employerShare = If(employeeShare = 0, 0, StandardEmployerContribution)

            If employee.IsWeeklyPaid Then
                Dim isOnScheduleForDeduction = If(
                    employee.IsUnderAgency,
                    payperiod.HDMFWeeklyAgentContribSched,
                    payperiod.HDMFWeeklyContribSched)

                If isOnScheduleForDeduction Then
                    paystub.HdmfEmployeeShare = employeeShare
                    paystub.HdmfEmployerShare = employerShare
                End If
            Else
                Dim deductionSchedule = employee.PagIBIGSchedule

                If IsHdmfPaidOnFirstHalf(deductionSchedule, payperiod) Or
                    IsHdmfPaidOnEndOfTheMonth(deductionSchedule, payperiod) Then

                    paystub.HdmfEmployeeShare = employeeShare
                    paystub.HdmfEmployerShare = employerShare
                ElseIf IsHdmfPaidPerPayPeriod(deductionSchedule) Then
                    paystub.HdmfEmployeeShare = employeeShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
                    paystub.HdmfEmployerShare = employerShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
                End If
            End If
        End Sub

        Private Function IsHdmfPaidOnFirstHalf(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsFirstHalf And (deductionSchedule = ContributionSchedule.FIRST_HALF)
        End Function

        Private Function IsHdmfPaidOnEndOfTheMonth(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsEndOfTheMonth And (deductionSchedule = ContributionSchedule.END_OF_THE_MONTH)
        End Function

        Private Function IsHdmfPaidPerPayPeriod(deductionSchedule As String) As Boolean
            Return deductionSchedule = ContributionSchedule.PER_PAY_PERIOD
        End Function

    End Class

End Namespace
