Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeeadjustments")>
    Public Class Adjustment

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        Public Property ProductID As Integer?

        Public Property EffectiveStartDate As Date

        Public Property AllowanceFrequency As String

        Public Property EffectiveEndDate As Date

        Public Property TaxableFlag As Char

        Public Property AllowanceAmount As Decimal

        Public Property Comments As Char

    End Class

End Namespace