Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("calendarday")>
    Public Class CalendarDay

        <Key>
        Public Property RowID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property Updated As Date?

        Public Property UpdatedBy As Integer?

        Public Property CalendarID As Integer?

        Public Property DayTypeID As Integer?

        Public Property [Date] As Date

        Public Property Description As String

        Public Overridable Property DayType As DayType

    End Class

End Namespace
