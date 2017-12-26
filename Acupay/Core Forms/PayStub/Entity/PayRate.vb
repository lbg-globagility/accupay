Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("payrate")>
    Public Class PayRate

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property DayBefore As Date?

        <Column("Date")>
        Public Property RateDate As Date

        Public Property PayType As String

        Public Property Description As String

        <Column("PayRate")>
        Public Property CommonRate As Decimal

        Public Property OvertimeRate As Decimal

        Public Property NightDifferentialRate As Decimal

        Public Property NightDifferentialOTRate As Decimal

        Public Property RestDayRate As Decimal

        Public Property RestDayOvertimeRate As Decimal

    End Class

End Namespace
