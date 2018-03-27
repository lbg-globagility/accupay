Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("allowanceitem")>
    Public Class AllowanceItem

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property RowID As Integer?

        Public Overridable Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property Created As Date

        Public Overridable Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Overridable Property LastUpd As Date?

        Public Overridable Property LastUpdBy As Integer?

        Public Overridable Property AllowanceID As Integer?

        Public Overridable Property PayPeriodID As Integer?

        Public Overridable Property Amount As Decimal

        <ForeignKey("AllowanceID")>
        Public Overridable Property Allowance As Allowance

        Public Overridable Property Paystub As Paystub

        <ForeignKey("PayPeriodID")>
        Public Overridable Property PayPeriod As PayPeriod

        Public Overridable Property AllowancesPerDay As IList(Of AllowancePerDay)

        Public Overridable Sub AddPerDay([date] As Date, amount As Decimal)
            Dim perDay = New AllowancePerDay([date], amount)
            perDay.AllowanceItem = Me
            AllowancesPerDay.Add(perDay)

            Me.Amount += perDay.Amount
        End Sub

    End Class

End Namespace