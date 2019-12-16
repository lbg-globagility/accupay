Option Strict On

Imports AccuPay
Imports AccuPay.Entity
Imports AccuPay.Utils
Imports PayrollSys

Namespace Global.Globagility.AccuPay.Government

    Public Class PhilHealthCalculator

        Private _philHealthDeductionType As String

        Private _philHealthContributionRate As Decimal

        Private _philHealthMinimumContribution As Decimal

        Private _philHealthMaximumContribution As Decimal

        Private _philHealthBrackets As IList(Of PhilHealthBracket)

        Public Sub New(listOfValues As IEnumerable(Of ListOfValue), philHealthBrackets As IEnumerable(Of PhilHealthBracket))
            listOfValues = listOfValues.
                Where(Function(l) l.Type = "PhilHealth").
                ToList()

            Dim values = New ListOfValueCollection(listOfValues)

            _philHealthDeductionType = If(values.GetValue("DeductionType"), "Bracket")
            _philHealthContributionRate = values.GetDecimal("Rate")
            _philHealthMinimumContribution = values.GetDecimal("MinimumContribution")
            _philHealthMaximumContribution = values.GetDecimal("MaximumContribution")

            _philHealthBrackets = philHealthBrackets.ToList()
        End Sub

        Public Function Calculate(monthlyRate As Decimal) As Decimal
            Dim philHealthContribution = 0D

            If _philHealthDeductionType = "Formula" Then
                philHealthContribution = monthlyRate * (_philHealthContributionRate / 100)

                philHealthContribution = {philHealthContribution, _philHealthMinimumContribution}.Max()
                philHealthContribution = {philHealthContribution, _philHealthMaximumContribution}.Min()
                philHealthContribution = AccuMath.Truncate(philHealthContribution, 2)
            Else
                Dim philHealthBracket = _philHealthBrackets?.FirstOrDefault(
                    Function(p) p.SalaryRangeFrom <= monthlyRate And monthlyRate <= p.SalaryRangeTo)

                philHealthContribution = If(philHealthBracket?.TotalMonthlyPremium, 0)
            End If

            Return philHealthContribution
        End Function

    End Class

End Namespace