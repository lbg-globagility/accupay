Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Services
Imports AccuPay.Entity

Public Class SssCalculator

    Private ReadOnly _settings As ListOfValueCollection

    Private ReadOnly _socialSecurityBrackets As IReadOnlyCollection(Of Entities.SocialSecurityBracket)

    Public Sub New(settings As ListOfValueCollection, socialSecurityBrackets As IReadOnlyCollection(Of Entities.SocialSecurityBracket))
        _settings = settings
        _socialSecurityBrackets = socialSecurityBrackets
    End Sub

    Public Sub Calculate(paystub As Paystub, previousPaystub As Entities.Paystub, salary As Entities.Salary, employee As Entities.Employee, payperiod As Entities.PayPeriod)
        ' Reset SSS values to zero
        paystub.SssEmployeeShare = 0
        paystub.SssEmployerShare = 0

        ' If salary is is set not to pay sss, return.
        If Not salary.DoPaySSSContribution Then
            Return
        End If

        ' Get the social security bracket based on the amount earned.
        Dim amount = GetSocialSecurityAmount(paystub, previousPaystub, salary, employee)
        Dim socialSecurityBracket = FindMatchingBracket(amount)

        ' If no bracket was matched/found, then there's nothing to compute.
        If socialSecurityBracket Is Nothing Then
            Return
        End If

        Dim employeeShare = socialSecurityBracket.EmployeeContributionAmount
        Dim employerShare = socialSecurityBracket.EmployerContributionAmount + socialSecurityBracket.EmployeeECAmount

        If employee.IsWeeklyPaid Then
            Dim shouldDeduct = If(employee.IsUnderAgency, payperiod.SSSWeeklyAgentContribSched, payperiod.SSSWeeklyContribSched)

            If shouldDeduct Then
                paystub.SssEmployeeShare = employeeShare
                paystub.SssEmployerShare = employerShare
            End If
        Else
            Dim deductionSchedule = employee.SssSchedule

            If IsSssPaidOnFirstHalf(payperiod, deductionSchedule) Or IsSssPaidOnEndOfTheMonth(payperiod, deductionSchedule) Then
                paystub.SssEmployeeShare = employeeShare
                paystub.SssEmployerShare = employerShare
            ElseIf IsSssPaidPerPayPeriod(deductionSchedule) Then
                paystub.SssEmployeeShare = employeeShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
                paystub.SssEmployerShare = employerShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
            End If
        End If
    End Sub

    Private Function FindMatchingBracket(amount As Decimal) As Entities.SocialSecurityBracket
        Return _socialSecurityBrackets.FirstOrDefault(Function(s) s.RangeFromAmount <= amount And s.RangeToAmount >= amount)
    End Function

    Private Function GetSocialSecurityAmount(paystub As Paystub,
                                             previousPaystub As Entities.Paystub,
                                             salary As Entities.Salary,
                                             employee As Entities.Employee) As Decimal

        Dim policyByOrganization = _settings.GetBoolean("Policy.ByOrganization", False)

        Dim calculationBasis = _settings.GetEnum("SocialSecuritySystem.CalculationBasis", SssCalculationBasis.BasicSalary, policyByOrganization)

        Select Case calculationBasis
            Case SssCalculationBasis.BasicSalary

                Return Data.Helpers.PayrollTools.GetEmployeeMonthlyRate(employee, salary)

            Case SssCalculationBasis.Earnings

                Return If(previousPaystub?.TotalEarnings, 0) + paystub.TotalEarnings

            Case SssCalculationBasis.GrossPay

                Return If(previousPaystub?.GrossPay, 0) + paystub.GrossPay

            Case SssCalculationBasis.BasicMinusDeductions

                Return If(previousPaystub?.TotalDaysPayWithOutOvertimeAndLeave, 0) + paystub.TotalDaysPayWithOutOvertimeAndLeave

            Case SssCalculationBasis.BasicMinusDeductionsWithoutPremium

                Dim totalHours = If(previousPaystub?.TotalWorkedHoursWithoutOvertimeAndLeave, 0) + paystub.TotalWorkedHoursWithoutOvertimeAndLeave

                If (New SystemOwnerService()).GetCurrentSystemOwner() = SystemOwnerService.Benchmark AndAlso employee.IsPremiumInclusive Then

                    totalHours = If(previousPaystub?.RegularHoursAndTotalRestDay, 0) + paystub.RegularHoursAndTotalRestDay
                End If

                Dim monthlyRate = Data.Helpers.PayrollTools.GetEmployeeMonthlyRate(employee, salary)
                Dim dailyRate = Data.Helpers.PayrollTools.GetDailyRate(monthlyRate, employee.WorkDaysPerYear)
                Dim hourlyRate = Data.Helpers.PayrollTools.GetHourlyRateByDailyRate(dailyRate)

                Return totalHours * hourlyRate

            Case Else

                Return 0
        End Select
    End Function

    Private Function IsSssPaidOnFirstHalf(payperiod As Entities.PayPeriod, deductionSchedule As String) As Boolean
        Return payperiod.IsFirstHalf And (deductionSchedule = ContributionSchedule.FIRST_HALF)
    End Function

    Private Function IsSssPaidOnEndOfTheMonth(payperiod As Entities.PayPeriod, deductionSchedule As String) As Boolean
        Return payperiod.IsEndOfTheMonth And (deductionSchedule = ContributionSchedule.END_OF_THE_MONTH)
    End Function

    Private Function IsSssPaidPerPayPeriod(deductionSchedule As String) As Boolean
        Return deductionSchedule = ContributionSchedule.PER_PAY_PERIOD
    End Function

    Private Class ContributionSchedule
        Public Const FIRST_HALF As String = "First half"
        Public Const END_OF_THE_MONTH As String = "End of the month"
        Public Const PER_PAY_PERIOD As String = "Per pay period"
    End Class

End Class