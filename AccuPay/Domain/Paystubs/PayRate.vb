Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Data

Namespace Global.AccuPay.Entity

    <Table("payrate")>
    Public Class PayRate
        Implements IPayrate

        Public Const RegularDay As String = "Regular Day"

        Public Const SpecialNonWorkingHoliday As String = "Special Non-Working Holiday"

        Public Const RegularHoliday As String = "Regular Holiday"

        Public Const DoubleHoliday As String = "Double Holiday"

        Public Const RegularDayAndSpecialHoliday As String = "Regular + Special Holiday"

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

        Public ReadOnly Property IsRegularDay As Boolean Implements IPayrate.IsRegularDay
            Get
                Return PayType = RegularDay
            End Get
        End Property

        Public ReadOnly Property IsHoliday As Boolean Implements IPayrate.IsHoliday
            Get
                Return IsSpecialNonWorkingHoliday Or IsRegularHoliday
            End Get
        End Property

        Public ReadOnly Property IsSpecialNonWorkingHoliday As Boolean Implements IPayrate.IsSpecialNonWorkingHoliday
            Get
                Return PayType = SpecialNonWorkingHoliday
            End Get
        End Property

        Public ReadOnly Property IsRegularHoliday As Boolean Implements IPayrate.IsRegularHoliday
            Get
                Return PayType = RegularHoliday OrElse PayType = DoubleHoliday
            End Get
        End Property

        Public ReadOnly Property IPayrate_Date As Date Implements IPayrate.Date
            Get
                Return [Date]
            End Get
        End Property

        Public ReadOnly Property IPayrate_RegularRate As Decimal Implements IPayrate.RegularRate
            Get
                Return CommonRate
            End Get
        End Property

        Private ReadOnly Property IPayrate_OvertimeRate As Decimal Implements IPayrate.OvertimeRate
            Get
                Return OvertimeRate
            End Get
        End Property

        Public ReadOnly Property IPayrate_NightDiffRate As Decimal Implements IPayrate.NightDiffRate
            Get
                Return NightDifferentialRate
            End Get
        End Property

        Public ReadOnly Property IPayrate_NightDiffOTRate As Decimal Implements IPayrate.NightDiffOTRate
            Get
                Return NightDifferentialOTRate
            End Get
        End Property

        Private ReadOnly Property IPayrate_RestDayRate As Decimal Implements IPayrate.RestDayRate
            Get
                Return RestDayRate
            End Get
        End Property

        Public ReadOnly Property IPayrate_RestDayOTRate As Decimal Implements IPayrate.RestDayOTRate
            Get
                Return RestDayOvertimeRate
            End Get
        End Property

        Private ReadOnly Property IPayrate_RestDayNDRate As Decimal Implements IPayrate.RestDayNDRate
            Get
                Return RestDayNDRate
            End Get
        End Property

        Private ReadOnly Property IPayrate_RestDayNDOTRate As Decimal Implements IPayrate.RestDayNDOTRate
            Get
                Return RestDayNDOTRate
            End Get
        End Property

        Public ReadOnly Property IsDoubleHoliday As Boolean Implements IPayrate.IsDoubleHoliday
            Get
                Return PayType = DoubleHoliday
            End Get
        End Property

    End Class

End Namespace