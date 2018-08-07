Option Strict On

Imports AccuPay.Entity

Namespace Global.AccuPay.Payroll

    Public Class WithholdingTaxCalculator

        Private ReadOnly _filingStatuses As DataTable

        Private ReadOnly _withholdingTaxBrackets As IList(Of WithholdingTaxBracket)

        Public Sub New(filingStatuses As DataTable, withholdingTaxBrackets As IList(Of WithholdingTaxBracket))
            _filingStatuses = filingStatuses
            _withholdingTaxBrackets = withholdingTaxBrackets
        End Sub

        Public Sub Calculate(deductionSchedule As String, employee As DataRow, paystub As Paystub, employee2 As Employee, payperiod As PayPeriod)
            Dim payFrequencyID As Integer

            If IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod) Or IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod) Then
                payFrequencyID = PayFrequency.Monthly
            ElseIf IsWithholdingTaxPaidPerPayPeriod(deductionSchedule) Then
                payFrequencyID = PayFrequency.SemiMonthly
            End If

            ' Round the daily rate to two decimal places since amounts in the 3rd decimal place
            ' isn't significant enough to warrant the employee to be taxable.
            Dim dailyRate = Math.Round(ValNoComma(employee("EmpRatePerDay")), 2)

            Dim minimumWage = ValNoComma(employee("MinimumWageAmount"))
            Dim isMinimumWageEarner = dailyRate <= minimumWage

            If isMinimumWageEarner Then
                paystub.TaxableIncome = 0D
            End If

            If Not (paystub.TaxableIncome > 0D And IsScheduledForTaxation(deductionSchedule, payperiod)) Then
                paystub.WithholdingTax = 0
                Return
            End If

            Dim maritalStatus = employee2.MaritalStatus
            Dim noOfDependents = employee2.NoOfDependents

            Dim filingStatus = _filingStatuses.
                Select($"
                    MaritalStatus = '{maritalStatus}' AND
                    Dependent <= '{noOfDependents}'
                ").
                OrderByDescending(Function(f) CInt(f("Dependent"))).
                FirstOrDefault()

            Dim filingStatusID = 1
            If filingStatus IsNot Nothing Then
                filingStatusID = CInt(filingStatus("RowID"))
            End If

            Dim bracket = GetMatchingTaxBracket(payFrequencyID, filingStatusID, paystub, payperiod)

            If bracket Is Nothing Then
                Return
            End If

            Dim exemptionAmount = bracket.ExemptionAmount
            Dim taxableIncomeFromAmount = bracket.TaxableIncomeFromAmount
            Dim exemptionInExcessAmount = bracket.ExemptionInExcessAmount

            Dim excessAmount = paystub.TaxableIncome - taxableIncomeFromAmount

            Dim is_weekly As Boolean = Convert.ToBoolean(Convert.ToInt16(employee("IsWeeklyPaid")))

            If is_weekly Then
                Dim is_deduct_sched_to_thisperiod = If(
                    employee2.IsUnderAgency,
                    payperiod.WTaxWeeklyAgentContribSched,
                    payperiod.WTaxWeeklyContribSched)

                If is_deduct_sched_to_thisperiod Then
                    paystub.WithholdingTax = AccuMath.CommercialRound(exemptionAmount + (excessAmount * exemptionInExcessAmount))
                Else
                    paystub.WithholdingTax = 0
                End If
            Else
                paystub.WithholdingTax = AccuMath.CommercialRound(exemptionAmount + (excessAmount * exemptionInExcessAmount))
            End If
        End Sub

        Private Function GetMatchingTaxBracket(payFrequencyID As Integer?, filingStatusID As Integer?, _paystub As Paystub, _payperiod As PayPeriod) As WithholdingTaxBracket
            Dim taxEffectivityDate = New Date(_payperiod.Year, _payperiod.Month, 1)

            Dim possibleBrackets =
                (From w In _withholdingTaxBrackets
                 Where w.PayFrequencyID = payFrequencyID And
                     w.TaxableIncomeFromAmount <= _paystub.TaxableIncome And
                     _paystub.TaxableIncome <= w.TaxableIncomeToAmount And
                     w.EffectiveDateFrom <= taxEffectivityDate And
                     taxEffectivityDate <= w.EffectiveDateTo
                 Select w).
                 ToList()

            ' If there are more than one tax brackets that matches the previous list, filter by
            ' the tax filing status.
            If possibleBrackets.Count > 1 Then
                Return possibleBrackets.
                    Where(Function(b) Nullable.Equals(b.FilingStatusID, filingStatusID)).
                    FirstOrDefault()
            ElseIf possibleBrackets.Count = 1 Then
                Return possibleBrackets.First()
            End If

            Return Nothing
        End Function

        Private Function IsWithholdingTaxPaidOnFirstHalf(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsFirstHalf And (deductionSchedule = ContributionSchedule.FirstHalf)
        End Function

        Private Function IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsEndOfTheMonth And (deductionSchedule = ContributionSchedule.EndOfTheMonth)
        End Function

        Private Function IsWithholdingTaxPaidPerPayPeriod(deductionSchedule As String) As Boolean
            Return deductionSchedule = ContributionSchedule.PerPayPeriod
        End Function

        Private Function IsScheduledForTaxation(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return (payperiod.IsFirstHalf And IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod)) Or
                (payperiod.IsEndOfTheMonth And IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod)) Or
                IsWithholdingTaxPaidPerPayPeriod(deductionSchedule)
        End Function

        Private Class PayFrequency
            Public Const SemiMonthly As Integer = 1
            Public Const Monthly As Integer = 2
        End Class

        Private Class ContributionSchedule
            Public Const FirstHalf As String = "First half"
            Public Const EndOfTheMonth As String = "End of the month"
            Public Const PerPayPeriod As String = "Per pay period"
        End Class

    End Class

End Namespace
