Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("payrate")>
    Public Class PayRate

        Private Const RegularDay As String = "Regular Day"

        Private Const SpecialNonWorkingHoliday As String = "Special Non-Working Holiday"

        Private Const RegularHoliday As String = "Regular Holiday"

        Private Const DoubleHoliday As String = "Double Holiday"

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

        Public Property DayBefore As Date?

        <Column("Date")>
        Public Property [Date] As Date

        Public Property PayType As String

        Public Property Description As String

        <Column("PayRate")>
        Public Property CommonRate As Decimal

        Public Property OvertimeRate As Decimal

        Public Property NightDifferentialRate As Decimal

        Public Property NightDifferentialOTRate As Decimal

        Public Property RestDayRate As Decimal

        Public Property RestDayOvertimeRate As Decimal

        Public Property RestDayNDRate As Decimal

        Public Property RestDayNDOTRate As Decimal

        Public ReadOnly Property IsRegularDay As Boolean
            Get
                Return PayType = RegularDay
            End Get
        End Property

        Public ReadOnly Property IsHoliday As Boolean
            Get
                Return IsSpecialNonWorkingHoliday Or IsRegularHoliday
            End Get
        End Property

        Public ReadOnly Property IsSpecialNonWorkingHoliday As Boolean
            Get
                Return PayType = SpecialNonWorkingHoliday
            End Get
        End Property

        Public ReadOnly Property IsRegularHoliday As Boolean
            Get
                Return PayType = RegularHoliday OrElse PayType = DoubleHoliday
            End Get
        End Property

        Public ReadOnly Property IsDoubleHoliday As Boolean
            Get
                Return PayType = DoubleHoliday
            End Get
        End Property

    End Class

End Namespace
