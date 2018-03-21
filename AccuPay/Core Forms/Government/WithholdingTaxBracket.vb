Option Strict On

Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Namespace Global.AccuPay.Entity

    <Table("paywithholdingtax")>
    Public Class WithholdingTaxBracket

        <Key>
        Public Property RowID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property PayFrequencyID As Integer?

        Public Property FilingStatusID As Integer?

        Public Property EffectiveDateFrom As Date

        Public Property EffectiveDateTo As Date

        Public Property ExemptionAmount As Decimal

        Public Property ExemptionInExcessAmount As Decimal

        Public Property TaxableIncomeFromAmount As Decimal

        Public Property TaxableIncomeToAmount As Decimal

    End Class

End Namespace
