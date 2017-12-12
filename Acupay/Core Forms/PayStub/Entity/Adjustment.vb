Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("paystubadjustmentactual")>
    Public Class Adjustment

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer?

        Public Property PayStubID As Integer?

        Public Property ProductID As Integer?

        Public Property PayAmount As Decimal

        Public Property Comment As String

        Public Property IsActual As Boolean

        <ForeignKey("PayStubID")>
        Public Overridable Property Paystub As Paystub

    End Class

End Namespace