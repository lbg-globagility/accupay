Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("calendarday")>
    Public Class CalendarDay
        Implements IPayrate

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

        Public ReadOnly Property RegularRate As Decimal Implements IPayrate.RegularRate
            Get
                Return DayType.RegularRate
            End Get
        End Property

        Public ReadOnly Property OvertimeRate As Decimal Implements IPayrate.OvertimeRate
            Get
                Return DayType.OvertimeRate
            End Get
        End Property

        Public ReadOnly Property NightDiffRate As Decimal Implements IPayrate.NightDiffRate
            Get
                Return DayType.NightDiffRate
            End Get
        End Property

        Public ReadOnly Property NightDiffOTRate As Decimal Implements IPayrate.NightDiffOTRate
            Get
                Return DayType.NightDiffOTRate
            End Get
        End Property

        Public ReadOnly Property RestDayRate As Decimal Implements IPayrate.RestDayRate
            Get
                Return DayType.RestDayRate
            End Get
        End Property

        Public ReadOnly Property RestDayOTRate As Decimal Implements IPayrate.RestDayOTRate
            Get
                Return DayType.RestDayOTRate
            End Get
        End Property

        Public ReadOnly Property RestDayNDRate As Decimal Implements IPayrate.RestDayNDRate
            Get
                Return DayType.RestDayNDRate
            End Get
        End Property

        Public ReadOnly Property RestDayNDOTRate As Decimal Implements IPayrate.RestDayNDOTRate
            Get
                Return DayType.RestDayNDOTRate
            End Get
        End Property

        Public ReadOnly Property IsRegularDay As Boolean Implements IPayrate.IsRegularDay
            Get
                Return DayType.Name = "Regular Day"
            End Get
        End Property

        Public ReadOnly Property IsRegularHoliday As Boolean Implements IPayrate.IsRegularHoliday
            Get
                Return DayType.Name = "Regular Holiday"
            End Get
        End Property

        Public ReadOnly Property IsSpecialNonWorkingHoliday As Boolean Implements IPayrate.IsSpecialNonWorkingHoliday
            Get
                Return DayType.Name = "Special Non-Working Holiday"
            End Get
        End Property

        Public ReadOnly Property IsHoliday As Boolean Implements IPayrate.IsHoliday
            Get
                Return IsRegularHoliday OrElse IsSpecialNonWorkingHoliday
            End Get
        End Property

        Private ReadOnly Property IPayrate_Date As Date Implements IPayrate.Date
            Get
                Return [Date]
            End Get
        End Property

    End Class

End Namespace
