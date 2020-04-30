Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services
Imports AccuPay.Entity
Imports AccuPay.Utilities
Imports PayrollSys

Namespace Global.AccuPay.Payroll

    Public Class WithholdingTaxCalculator

        Private ReadOnly _filingStatuses As DataTable

        Private ReadOnly _withholdingTaxBrackets As ICollection(Of Entities.WithholdingTaxBracket)

        Private ReadOnly _divisionMinimumWages As ICollection(Of Entities.DivisionMinimumWage)

        Private ReadOnly _settings As ListOfValueCollection

        Public Sub New(settings As ListOfValueCollection, filingStatuses As DataTable, withholdingTaxBrackets As ICollection(Of Entities.WithholdingTaxBracket), divisionMinimumWages As ICollection(Of Entities.DivisionMinimumWage))
            _filingStatuses = filingStatuses
            _withholdingTaxBrackets = withholdingTaxBrackets
            _divisionMinimumWages = divisionMinimumWages
            _settings = settings
        End Sub

        Public Sub Calculate(paystub As Paystub, previousPaystub As Paystub, employee As Entities.Employee, payperiod As PayPeriod, salary As Salary)
            ' Reset the tax value before starting
            paystub.DeferredTaxableIncome = 0
            paystub.TaxableIncome = 0
            paystub.WithholdingTax = 0

            Dim currentTaxableIncome = 0D

            If employee.EmployeeType = SalaryType.Fixed Then
                currentTaxableIncome = paystub.BasicPay
            ElseIf employee.EmployeeType = SalaryType.Monthly Then
                Dim taxablePolicy = If(_settings.GetString("Payroll Policy.paystub.taxableincome"), "Basic Pay")

                If taxablePolicy = "Gross Income" Then
                    'Adds those taxable allowances to the taxable income
                    currentTaxableIncome = paystub.TotalEarnings + paystub.TotalTaxableAllowance
                Else
                    currentTaxableIncome = paystub.BasicPay
                End If
            End If

            ' Government contributions are tax deductible
            currentTaxableIncome -= paystub.GovernmentDeductions
            ' Taxable income should not be less than zero
            paystub.TaxableIncome = {currentTaxableIncome, 0}.Max()

            Dim deductionSchedule = employee.WithholdingTaxSchedule
            ' Check if the current pay period is scheduled for taxation. If not, put the
            ' taxable income as `Deferred` to be added on the taxable income in the next period.
            If Not IsScheduledForTaxation(deductionSchedule, payperiod) Then
                paystub.DeferredTaxableIncome = paystub.TaxableIncome
                paystub.TaxableIncome = 0

                Return
            End If

            If IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod) Or
                IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod) Then

                paystub.TaxableIncome += If(previousPaystub?.DeferredTaxableIncome, 0)
            ElseIf IsWithholdingTaxPaidPerPayPeriod(deductionSchedule) Then
                ' Nothing to do here for now
            End If

            Dim dailyRate = If(
                employee.IsDaily,
                salary.BasicSalary,
                salary.BasicSalary / (employee.WorkDaysPerYear / 12))

            ' Round the daily rate to two decimal places since amounts in the 3rd decimal place
            ' isn't significant enough to warrant the employee to be taxable.
            dailyRate = AccuMath.CommercialRound(dailyRate, 2)

            ' If the employee is earning below the minimum wage, then remove the taxable income.
            Dim minimumWage = GetCurrentMinimumWage(employee)
            If dailyRate <= minimumWage Then
                paystub.TaxableIncome = 0
            End If

            ' If the employee has no taxable income, then there's no need to compute for tax withheld.
            If paystub.TaxableIncome <= 0 Then
                Return
            End If

            Dim payFrequencyID As Integer
            If IsWithholdingTaxPaidOnFirstHalf(deductionSchedule, payperiod) Or
                IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule, payperiod) Then

                payFrequencyID = PayFrequency.Monthly
            ElseIf IsWithholdingTaxPaidPerPayPeriod(deductionSchedule) Then
                payFrequencyID = PayFrequency.SemiMonthly
            End If

            Dim filingStatusID = GetFilingStatusID(employee.MaritalStatus, employee.NoOfDependents)
            Dim taxBracket = GetTaxBracket(payFrequencyID, filingStatusID, paystub, payperiod)

            paystub.WithholdingTax = GetTaxWithheld(taxBracket, paystub.TaxableIncome)
        End Sub

        Private Function GetCurrentMinimumWage(employee As Entities.Employee) As Decimal
            Dim divisionMinimumWage = _divisionMinimumWages?.
                FirstOrDefault(Function(t) Nullable.Equals(t.DivisionID, employee.Position?.DivisionID))
            Dim minimumWage = If(divisionMinimumWage?.Amount, 0)

            Return minimumWage
        End Function

        Private Function GetTaxWithheld(bracket As Entities.WithholdingTaxBracket, taxableIncome As Decimal) As Decimal
            If bracket Is Nothing Then
                Return 0
            End If

            Dim excessAmount = taxableIncome - bracket.TaxableIncomeFromAmount
            Dim taxWithheld = bracket.ExemptionAmount + (excessAmount * bracket.ExemptionInExcessAmount)

            Return AccuMath.CommercialRound(taxWithheld)
        End Function

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

        Private Function GetTaxBracket(payFrequencyID As Integer?, filingStatusID As Integer?, _paystub As Paystub, _payperiod As PayPeriod) As Entities.WithholdingTaxBracket
            Dim taxEffectivityDate = New Date(_payperiod.Year, _payperiod.Month, 1)

            Dim possibleBrackets = _withholdingTaxBrackets.
                Where(Function(w) w.PayFrequencyID.Value = payFrequencyID.Value).
                Where(Function(w) w.EffectiveDateFrom <= taxEffectivityDate And taxEffectivityDate <= w.EffectiveDateTo).
                Where(Function(w) w.TaxableIncomeFromAmount < _paystub.TaxableIncome And _paystub.TaxableIncome <= w.TaxableIncomeToAmount).
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
            Return payperiod.IsFirstHalf And (deductionSchedule = ContributionSchedule.FIRST_HALF)
        End Function

        Private Function IsWithholdingTaxPaidOnEndOfTheMonth(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsEndOfTheMonth And (deductionSchedule = ContributionSchedule.END_OF_THE_MONTH)
        End Function

        Private Function IsWithholdingTaxPaidPerPayPeriod(deductionSchedule As String) As Boolean
            Return deductionSchedule = ContributionSchedule.PER_PAY_PERIOD
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

        Private Class SalaryType
            Public Const Fixed As String = "Fixed"
            Public Const Monthly As String = "Monthly"
            Public Const Daily As String = "Daily"
        End Class

    End Class

End Namespace