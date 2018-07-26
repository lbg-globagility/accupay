Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("paystubadjustment")>
    Public Class Adjustment

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

        Public Overridable Property ProductID As Integer?

        Public Overridable Property PaystubID As Integer?

        <Column("PayAmount")>
        Public Overridable Property Amount As Decimal

        Public Overridable Property Comment As String

        Public Overridable Property IsActual As Boolean

        <ForeignKey("PaystubID")>
        Public Overridable Property Paystub As Paystub

    End Class

End Namespace
