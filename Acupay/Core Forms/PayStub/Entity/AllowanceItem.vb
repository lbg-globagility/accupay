Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("allowanceitem")>
    Public Class AllowanceItem

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property PaystubID As Integer?

        Public Property AllowanceID As Integer?

        Public Property PayPeriodID As Integer?

        Public Property Amount As Decimal

        <ForeignKey("AllowanceID")>
        Public Overridable Property Allowance As Allowance

        <ForeignKey("PaystubID")>
        Public Overridable Property Paystub As Paystub

        <ForeignKey("PayPeriodID")>
        Public Overridable Property PayPeriod As PayPeriod

    End Class

End Namespace