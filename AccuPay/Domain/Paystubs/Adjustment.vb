Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("paystubadjustment")>
    Public Class Adjustment

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

        Public Property ProductID As Integer?

        Public Property PaystubID As Integer?

        <Column("PayAmount")>
        Public Property Amount As Decimal

        Public Property Comment As String

        Public Property IsActual As Boolean

        <ForeignKey("PaystubID")>
        Public Overridable Property Paystub As Paystub

        <ForeignKey("ProductID")>
        Public Overridable Property Product As Product

    End Class

End Namespace
