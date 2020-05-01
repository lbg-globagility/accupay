Option Strict On

Imports System.Text.RegularExpressions
Imports AccuPay.Data
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services
Imports AccuPay.Entity
Imports PayrollSys

Namespace Global.AccuPay.Payroll

    Public Class HdmfCalculator

        Public Const StandardEmployeeContribution As Decimal = 100

        Private Const StandardEmployerContribution As Decimal = 100

        Public Sub Calculate(salary As Entities.Salary, paystub As Paystub, employee As Entities.Employee, payperiod As PayPeriod, settings As ListOfValueCollection)
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

            'Override employer share
            'make its value matching to the employee share
            Dim hdmfPolicy = "HDMF.EmployeesMatchingEmployerShare"
            Dim employeesMatchingEmployerShare = settings.GetStringOrNull(hdmfPolicy)

            If Not String.IsNullOrWhiteSpace(employeesMatchingEmployerShare) Then

                Dim employees = Regex.Split(employeesMatchingEmployerShare, ",")

                If employees.Where(Function(e) e.Trim = employee.EmployeeNo).
                    Any Then

                    paystub.HdmfEmployerShare = paystub.HdmfEmployeeShare

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