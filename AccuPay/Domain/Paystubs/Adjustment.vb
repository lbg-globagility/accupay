Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("paystubadjustment")>
    Public Class Adjustment
        Implements IAdjustment

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer? Implements IAdjustment.RowID

        Public Property OrganizationID As Integer? Implements IAdjustment.OrganizationID

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date Implements IAdjustment.Created

        Public Property CreatedBy As Integer? Implements IAdjustment.CreatedBy

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date? Implements IAdjustment.LastUpd

        Public Property LastUpdBy As Integer? Implements IAdjustment.LastUpdBy

        Public Property ProductID As Integer? Implements IAdjustment.ProductID

        Public Property PaystubID As Integer? Implements IAdjustment.PayStubID

        <Column("PayAmount")>
        Public Property Amount As Decimal Implements IAdjustment.PayAmount

        Public Property Comment As String Implements IAdjustment.Comment

        Public Property IsActual As Boolean Implements IAdjustment.IsActual

        <ForeignKey("PaystubID")>
        Public Overridable Property Paystub As Paystub Implements IAdjustment.Paystub

        <ForeignKey("ProductID")>
        Public Overridable Property Product As Product Implements IAdjustment.Product

    End Class

End Namespace