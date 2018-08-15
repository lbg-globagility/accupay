Option Strict On

Imports AccuPay.Entity
Imports PayrollSys

Public Class SssCalculator

    Private Class ContributionSchedule
        Public Const FirstHalf As String = "First half"
        Public Const EndOfTheMonth As String = "End of the month"
        Public Const PerPayPeriod As String = "Per pay period"
    End Class

    Private ReadOnly _socialSecurityBrackets As IList(Of SocialSecurityBracket)

    Public Sub New(socialSecurityBrackets As IList(Of SocialSecurityBracket))
        _socialSecurityBrackets = socialSecurityBrackets
    End Sub

    Public Sub Calculate(settings As ListOfValueCollection, paystub As Paystub, previousPaystub As Paystub, salary As Salary, employee As DataRow, employee2 As Employee, payperiod As PayPeriod)
        Dim deductionSchedule = employee("SSSDeductSched").ToString

        Dim isWeekly As Boolean = Convert.ToBoolean(Convert.ToInt16(employee("IsWeeklyPaid")))

        Dim sssCalculation = settings.GetEnum("SocialSecuritySystem.CalculationBasis", SssCalculationBasis.BasicSalary)

        Dim isSssProrated =
            (sssCalculation = SssCalculationBasis.Earnings) Or
            (sssCalculation = SssCalculationBasis.GrossPay)

        Dim employeeSssPerMonth = 0D
        Dim employerSssPerMonth = 0D

        If isSssProrated Then
            Dim amount = GetSocialSecurityAmount(paystub, previousPaystub, sssCalculation)
            Dim socialSecurityBracket = FindMatchingBracket(amount)

            employeeSssPerMonth = If(socialSecurityBracket?.EmployeeContributionAmount, 0)
            employerSssPerMonth = If(socialSecurityBracket?.EmployerContributionAmount, 0)
        ElseIf sssCalculation = SssCalculationBasis.BasicSalary Then
            Dim socialSecurityId = salary.PaySocialSecurityID

            Dim socialSecurityBracket = _socialSecurityBrackets.FirstOrDefault(Function(s) Nullable.Equals(s.RowID, socialSecurityId))

            employeeSssPerMonth = If(socialSecurityBracket?.EmployeeContributionAmount, 0)
            employerSssPerMonth = If(socialSecurityBracket?.EmployerContributionAmount + socialSecurityBracket?.EmployeeECAmount, 0)
        End If

        If isWeekly Then
            Dim shouldDeduct = If(employee2.IsUnderAgency, payperiod.SSSWeeklyAgentContribSched, payperiod.SSSWeeklyContribSched)

            If shouldDeduct Then
                paystub.SssEmployeeShare = employeeSssPerMonth
                paystub.SssEmployerShare = employerSssPerMonth
            Else
                paystub.SssEmployeeShare = 0
                paystub.SssEmployerShare = 0
            End If
        Else
            If IsSssPaidOnFirstHalf(payperiod, deductionSchedule) Or IsSssPaidOnEndOfTheMonth(payperiod, deductionSchedule) Then
                paystub.SssEmployeeShare = employeeSssPerMonth
                paystub.SssEmployerShare = employerSssPerMonth
            ElseIf IsSssPaidPerPayPeriod(deductionSchedule) Then
                Dim payPeriodsPerMonth = CDec(employee("PAYFREQUENCY_DIVISOR"))

                paystub.SssEmployeeShare = employeeSssPerMonth / payPeriodsPerMonth
                paystub.SssEmployerShare = employerSssPerMonth / payPeriodsPerMonth
            Else
                paystub.SssEmployeeShare = 0
                paystub.SssEmployerShare = 0
            End If
        End If
    End Sub

    Private Function FindMatchingBracket(amount As Decimal) As SocialSecurityBracket
        Return _socialSecurityBrackets.FirstOrDefault(Function(s) s.RangeFromAmount <= amount And s.RangeToAmount >= amount)
    End Function

    Private Function GetSocialSecurityAmount(paystub As Paystub, previousPaystub As Paystub, sssCalculation As SssCalculationBasis) As Decimal
        If sssCalculation = SssCalculationBasis.Earnings Then
            Return If(previousPaystub?.TotalEarnings, 0) + paystub.TotalEarnings
        ElseIf sssCalculation = SssCalculationBasis.GrossPay Then
            Return If(previousPaystub?.GrossPay, 0) + paystub.GrossPay
        Else
            Return 0
        End If
    End Function

    Private Function IsSssPaidOnFirstHalf(payperiod As PayPeriod, deductionSchedule As String) As Boolean
        Return payperiod.IsFirstHalf And (deductionSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsSssPaidOnEndOfTheMonth(payperiod As PayPeriod, deductionSchedule As String) As Boolean
        Return payperiod.IsEndOfTheMonth And (deductionSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsSssPaidPerPayPeriod(deductionSchedule As String) As Boolean
        Return deductionSchedule = ContributionSchedule.PerPayPeriod
    End Function

End Class
