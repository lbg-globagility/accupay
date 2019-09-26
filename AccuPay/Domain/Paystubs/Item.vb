Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("paystubitem")>
    Public Class PaystubItem

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

        Public Overridable Property PayStubID As Integer?

        Public Overridable Property ProductID As Integer?

        Public Overridable Property PayAmount As Decimal

        'this might throw an error when accessed.
        'it thrown an error when used in Gotesco project.
        'currently no code references this property so this is not an issue.
        'String is the recommended data type for Undeclared
        Public Overridable Property Undeclared As Char

        <ForeignKey("ProductID")>
        Public Overridable Property Product As Product

        <ForeignKey("PayStubID")>
        Public Overridable Property Paystub As Paystub

    End Class

End Namespace