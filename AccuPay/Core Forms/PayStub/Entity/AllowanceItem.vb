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

        Public Overridable Property PaystubID As Integer?

        Public Overridable Property AllowanceID As Integer?

        Public Overridable Property PayPeriodID As Integer?

        Public Overridable Property Amount As Decimal

        <ForeignKey("AllowanceID")>
        Public Overridable Property Allowance As Allowance

        <ForeignKey("PaystubID")>
        Public Overridable Property Paystub As Paystub

        <ForeignKey("PayPeriodID")>
        Public Overridable Property PayPeriod As PayPeriod

        Public Overridable Property AllowancesPerDay As IList(Of AllowancePerDay)

    End Class

End Namespace