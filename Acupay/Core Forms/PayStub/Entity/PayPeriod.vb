Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("payperiod")>
    Public Class PayPeriod

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property PayFromDate As Date

        Public Property PayToDate As Date

    End Class

End Namespace