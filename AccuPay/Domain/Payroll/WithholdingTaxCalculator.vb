Option Strict On

Imports AccuPay.Entity
Imports PayrollSys

Namespace Global.AccuPay.Payroll

    Public Class WithholdingTaxCalculator

        Private ReadOnly _filingStatuses As DataTable

        Private ReadOnly _withholdingTaxBrackets As ICollection(Of WithholdingTaxBracket)

        Private ReadOnly _divisionMinimumWages As ICollection(Of DivisionMinimumWage)

        Public Sub New(filingStatuses As DataTable, withholdingTaxBrackets As ICollection(Of WithholdingTaxBracket), divisionMinimumWages As ICollection(Of DivisionMinimumWage))
            _filingStatuses = filingStatuses
            _withholdingTaxBrackets = withholdingTaxBrackets
            _divisionMinimumWages = divisionMinimumWages
        End Sub

        Public Sub Calculate(settings As ListOfValueCollection, paystub As Paystub, previousPaystub As Paystub, employee2 As Employee, payperiod As PayPeriod, salary As Salary)
            Dim currentTaxableIncome = 0D

            If employee2.EmployeeType = SalaryType.Fixed Then
                currentTaxableIncome = paystub.BasicPay
            ElseIf employee2.EmployeeType = SalaryType.Monthly Then
                Dim taxablePolicy = If(settings.GetString("Payroll Policy.paystub.taxableincome"), "Basic Pay")

                If taxablePolicy = "Gross Income" Then
                    currentTaxableIncome = paystub.TotalEarnings

                    'Adds those taxable allowances to the taxable income
                    currentTaxableIncome += paystub.TotalTaxableAllowance
                Else
                    currentTaxableIncome = paystub.BasicPay
                End If

            End If


            currentTaxableIncome = currentTaxableIncome - paystub.GovernmentDeductions

            Dim deductionSchedule = employee2.WithholdingTaxSchedule

            If IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod) Or IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod) Then
                paystub.TaxableIncome = currentTaxableIncome + If(previousPaystub?.TaxableIncome, 0)
            ElseIf IsWithholdingTaxPaidPerPayPeriod(deductionSchedule) Then
                paystub.TaxableIncome = currentTaxableIncome
            Else
                paystub.TaxableIncome = currentTaxableIncome
            End If

            Dim dailyRate = If(
                employee2.IsDaily,
                salary.BasicSalary,
                salary.BasicSalary / (employee2.WorkDaysPerYear / 12))

            ' Round the daily rate to two decimal places since amounts in the 3rd decimal place
            ' isn't significant enough to warrant the employee to be taxable.
            dailyRate = Math.Round(dailyRate, 2)

            Dim divisionMinimumWage = _divisionMinimumWages?.
                FirstOrDefault(Function(t) CBool(t.DivisionID = employee2.Position?.DivisionID))

            Dim minimumWage = If(divisionMinimumWage?.Amount, 0)
            Dim isMinimumWageEarner = dailyRate <= minimumWage

            If isMinimumWageEarner Then
                paystub.TaxableIncome = 0
            End If

            If Not (paystub.TaxableIncome > 0D And IsScheduledForTaxation(deductionSchedule, payperiod)) Then
                paystub.WithholdingTax = 0
                Return
            End If

            Dim payFrequencyID As Integer
            If IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod) Or IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod) Then
                payFrequencyID = PayFrequency.Monthly
            ElseIf IsWithholdingTaxPaidPerPayPeriod(deductionSchedule) Then
                payFrequencyID = PayFrequency.SemiMonthly
            End If

            Dim filingStatusID = GetFilingStatusID(employee2.MaritalStatus, employee2.NoOfDependents)

            Dim bracket = GetMatchingTaxBracket(payFrequencyID, filingStatusID, paystub, payperiod)

            If bracket Is Nothing Then
                paystub.TaxableIncome = 0
                Return
            End If

            Dim exemptionAmount = bracket.ExemptionAmount
            Dim taxableIncomeFromAmount = bracket.TaxableIncomeFromAmount
            Dim exemptionInExcessAmount = bracket.ExemptionInExcessAmount

            Dim excessAmount = paystub.TaxableIncome - taxableIncomeFromAmount

            Dim taxAmount = AccuMath.CommercialRound(exemptionAmount + (excessAmount * exemptionInExcessAmount))

            If employee2.IsWeeklyPaid Then
                Dim is_deduct_sched_to_thisperiod = If(
                    employee2.IsUnderAgency,
                    payperiod.WTaxWeeklyAgentContribSched,
                    payperiod.WTaxWeeklyContribSched)

                If is_deduct_sched_to_thisperiod Then
                    paystub.WithholdingTax = taxAmount
                Else
                    paystub.WithholdingTax = 0
                End If
            Else
                paystub.WithholdingTax = taxAmount
            End If
        End Sub

        Private Function GetFilingStatusID(maritalStatus As String, noOfDependents As Integer?) As Integer
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

            Return filingStatusID
        End Function

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

        Private Class SalaryType
            Public Const Fixed As String = "Fixed"
            Public Const Monthly As String = "Monthly"
            Public Const Daily As String = "Daily"
        End Class

    End Class

End Namespace
