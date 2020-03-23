Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("daytype")>
    Public Class DayType

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property Updated As Date?

        Public Property UpdatedBy As Integer?

        Public Property Name As String

        Public Property RegularRate As Decimal

        Public Property OvertimeRate As Decimal

        Public Property NightDiffRate As Decimal

        Public Property NightDiffOTRate As Decimal

        Public Property RestDayRate As Decimal

        Public Property RestDayOTRate As Decimal

        Public Property RestDayNDRate As Decimal

        Public Property RestDayNDOTRate As Decimal

        Public Property DayConsideredAs As String

    End Class

End Namespace
