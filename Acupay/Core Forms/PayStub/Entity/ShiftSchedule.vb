Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeeshift")>
    Public Class ShiftSchedule

        <Key>
        Public Property RowID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        Public Property ShiftID As Integer?

        Public Property EffectiveFrom As Date

        Public Property EffectiveTo As Date

        <Column("NightShift")>
        Public Property IsNightShift As Boolean

        <Column("RestDay")>
        Public Property IsRestDay As Boolean

        <ForeignKey("ShiftID")>
        Public Overridable Property Shift As Shift

    End Class

End Namespace
