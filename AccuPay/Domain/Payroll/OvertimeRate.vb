Option Strict On

Public Class OvertimeRate
    Public Property BasePay As Rate
    Public Property Overtime As Rate
    Public Property NightDifferential As Rate
    Public Property NightDifferentialOvertime As Rate

    Public Property RestDay As Rate
    Public Property RestDayOvertime As Rate
    Public Property RestDayNightDifferential As Rate
    Public Property RestDayNightDifferentialOvertime As Rate

    Public Property SpecialHoliday As Rate
    Public Property SpecialHolidayOvertime As Rate
    Public Property SpecialHolidayNightDifferential As Rate
    Public Property SpecialHolidayNightDifferentialOvertime As Rate
    Public Property SpecialHolidayRestDay As Rate
    Public Property SpecialHolidayRestDayOvertime As Rate
    Public Property SpecialHolidayRestDayNightDifferential As Rate
    Public Property SpecialHolidayRestDayNightDifferentialOvertime As Rate

    Public Property RegularHoliday As Rate
    Public Property RegularHolidayOvertime As Rate
    Public Property RegularHolidayNightDifferential As Rate
    Public Property RegularHolidayNightDifferentialOvertime As Rate
    Public Property RegularHolidayRestDay As Rate
    Public Property RegularHolidayRestDayOvertime As Rate
    Public Property RegularHolidayRestDayNightDifferential As Rate
    Public Property RegularHolidayRestDayNightDifferentialOvertime As Rate

    Public Const BasePayDescription As String = "Base Pay"
    Public Const OvertimeDescription As String = "Overtime"
    Public Const NightDifferentialDescription As String = "Night Differential"
    Public Const NightDifferentialOvertimeDescription As String = "Night Differential OT"

    Public Const RestDayDescription As String = "Rest Day"
    Public Const RestDayOvertimeDescription As String = "Rest Day OT"
    Public Const RestDayNightDifferentialDescription As String = "Rest Day Night Differential"
    Public Const RestDayNightDifferentialOvertimeDescription As String = "Rest Day Night Differential OT"

    Public Const SpecialHolidayDescription As String = "Special Holiday"
    Public Const SpecialHolidayOvertimeDescription As String = "Special Holiday OT"
    Public Const SpecialHolidayNightDifferentialDescription As String = "Special Holiday Night Differential"
    Public Const SpecialHolidayNightDifferentialOvertimeDescription As String = "Special Holiday Night Differential OT"
    Public Const SpecialHolidayRestDayDescription As String = "Special Holiday Rest Day"
    Public Const SpecialHolidayRestDayOvertimeDescription As String = "Special Holiday Rest Day OT"
    Public Const SpecialHolidayRestDayNightDifferentialDescription As String = "Special Holiday Rest Day Night Differential"
    Public Const SpecialHolidayRestDayNightDifferentialOvertimeDescription As String = "Special Holiday Rest Day Night Differential OT"

    Public Const RegularHolidayDescription As String = "Regular Holiday"
    Public Const RegularHolidayOvertimeDescription As String = "Regular Holiday OT"
    Public Const RegularHolidayNightDifferentialDescription As String = "Regular Holiday Night Differential"
    Public Const RegularHolidayNightDifferentialOvertimeDescription As String = "Regular Holiday Night Differential OT"
    Public Const RegularHolidayRestDayDescription As String = "Regular Holiday Rest Day"
    Public Const RegularHolidayRestDayOvertimeDescription As String = "Regular Holiday Rest Day OT"
    Public Const RegularHolidayRestDayNightDifferentialDescription As String = "Regular Holiday Rest Day Night Differential"
    Public Const RegularHolidayRestDayNightDifferentialOvertimeDescription As String = "Regular Holiday Rest Day Night Differential OT"

    Private _overtimeRateList As List(Of Rate)

    Public ReadOnly Property OvertimeRateList() As List(Of Rate)
        Get
            If _overtimeRateList Is Nothing Then

                _overtimeRateList = New List(Of Rate)

                '_rateList.Add(Me.BasePay)
                _overtimeRateList.Add(Me.Overtime)
                _overtimeRateList.Add(Me.NightDifferential)
                _overtimeRateList.Add(Me.NightDifferentialOvertime)
                _overtimeRateList.Add(Me.RestDay)
                _overtimeRateList.Add(Me.RestDayOvertime)
                _overtimeRateList.Add(Me.RestDayNightDifferential)
                _overtimeRateList.Add(Me.RestDayNightDifferentialOvertime)

                _overtimeRateList.Add(Me.SpecialHoliday)
                _overtimeRateList.Add(Me.SpecialHolidayOvertime)
                _overtimeRateList.Add(Me.SpecialHolidayNightDifferential)
                _overtimeRateList.Add(Me.SpecialHolidayNightDifferentialOvertime)
                _overtimeRateList.Add(Me.SpecialHolidayRestDay)
                _overtimeRateList.Add(Me.SpecialHolidayRestDayOvertime)
                _overtimeRateList.Add(Me.SpecialHolidayRestDayNightDifferential)
                _overtimeRateList.Add(Me.SpecialHolidayRestDayNightDifferentialOvertime)

                _overtimeRateList.Add(Me.RegularHoliday)
                _overtimeRateList.Add(Me.RegularHolidayOvertime)
                _overtimeRateList.Add(Me.RegularHolidayNightDifferential)
                _overtimeRateList.Add(Me.RegularHolidayNightDifferentialOvertime)
                _overtimeRateList.Add(Me.RegularHolidayRestDay)
                _overtimeRateList.Add(Me.RegularHolidayRestDayOvertime)
                _overtimeRateList.Add(Me.RegularHolidayRestDayNightDifferential)
                _overtimeRateList.Add(Me.RegularHolidayRestDayNightDifferentialOvertime)

            End If

            Return _overtimeRateList
        End Get
    End Property

    Public ReadOnly Property RegularRateGroup() As RateGroup
        Get
            Return New RateGroup() With {
                .BasePay = Me.BasePay,
                .Overtime = Me.Overtime,
                .NightDifferential = Me.NightDifferential,
                .NightDifferentialOvertime = Me.NightDifferentialOvertime,
                .RestDay = Me.RestDay,
                .RestDayOvertime = Me.RestDayOvertime,
                .RestDayNightDifferential = Me.RestDayNightDifferential,
                .RestDayNightDifferentialOvertime = Me.RestDayNightDifferentialOvertime
            }
        End Get
    End Property

    Public ReadOnly Property SpecialHolidayRateGroup() As RateGroup
        Get
            Return New RateGroup() With {
                .BasePay = Me.SpecialHoliday,
                .Overtime = Me.SpecialHolidayOvertime,
                .NightDifferential = Me.SpecialHolidayNightDifferential,
                .NightDifferentialOvertime = Me.SpecialHolidayNightDifferentialOvertime,
                .RestDay = Me.SpecialHolidayRestDay,
                .RestDayOvertime = Me.SpecialHolidayRestDayOvertime,
                .RestDayNightDifferential = Me.SpecialHolidayRestDayNightDifferential,
                .RestDayNightDifferentialOvertime = Me.SpecialHolidayRestDayNightDifferentialOvertime
            }
        End Get
    End Property

    Public ReadOnly Property RegularHolidayRateGroup() As RateGroup
        Get
            Return New RateGroup() With {
                .BasePay = Me.RegularHoliday,
                .Overtime = Me.RegularHolidayOvertime,
                .NightDifferential = Me.RegularHolidayNightDifferential,
                .NightDifferentialOvertime = Me.RegularHolidayNightDifferentialOvertime,
                .RestDay = Me.RegularHolidayRestDay,
                .RestDayOvertime = Me.RegularHolidayRestDayOvertime,
                .RestDayNightDifferential = Me.RegularHolidayRestDayNightDifferential,
                .RestDayNightDifferentialOvertime = Me.RegularHolidayRestDayNightDifferentialOvertime
            }
        End Get
    End Property

    Public Sub New(basePay As Decimal,
        overtime As Decimal,
        nightDifferential As Decimal,
        nightDifferentialOvertime As Decimal,
        restDay As Decimal,
        restDayOvertime As Decimal,
        restDayNightDifferential As Decimal,
        restDayNightDifferentialOvertime As Decimal,
        specialHoliday As Decimal,
        specialHolidayOvertime As Decimal,
        specialHolidayNightDifferential As Decimal,
        specialHolidayNightDifferentialOvertime As Decimal,
        specialHolidayRestDay As Decimal,
        specialHolidayRestDayOvertime As Decimal,
        specialHolidayRestDayNightDifferential As Decimal,
        specialHolidayRestDayNightDifferentialOvertime As Decimal,
        regularHoliday As Decimal,
        regularHolidayOvertime As Decimal,
        regularHolidayNightDifferential As Decimal,
        regularHolidayNightDifferentialOvertime As Decimal,
        regularHolidayRestDay As Decimal,
        regularHolidayRestDayOvertime As Decimal,
        regularHolidayRestDayNightDifferential As Decimal,
        regularHolidayRestDayNightDifferentialOvertime As Decimal)

        Me.BasePay = New Rate("Base Pay", basePay)
        Me.Overtime = New Rate("Overtime", overtime)
        Me.NightDifferential = New Rate("Night Differential", nightDifferential, Me.BasePay)
        Me.NightDifferentialOvertime = New Rate("Night Differential OT", nightDifferentialOvertime)

        Me.RestDay = New Rate("Rest Day", restDay)
        Me.RestDayOvertime = New Rate("Rest Day OT", restDayOvertime)
        Me.RestDayNightDifferential = New Rate("Rest Day Night Differential", restDayNightDifferential, Me.RestDay)
        Me.RestDayNightDifferentialOvertime = New Rate("Rest Day Night Differential OT", restDayNightDifferentialOvertime)

        Me.SpecialHoliday = New Rate("Special Holiday", specialHoliday)
        Me.SpecialHolidayOvertime = New Rate("Special Holiday OT", specialHolidayOvertime)
        Me.SpecialHolidayNightDifferential = New Rate("Special Holiday Night Differential", specialHolidayNightDifferential, Me.SpecialHoliday)
        Me.SpecialHolidayNightDifferentialOvertime = New Rate("Special Holiday Night Differential OT", specialHolidayNightDifferentialOvertime)
        Me.SpecialHolidayRestDay = New Rate("Special Holiday Rest Day", specialHolidayRestDay)
        Me.SpecialHolidayRestDayOvertime = New Rate("Special Holiday Rest Day OT", specialHolidayRestDayOvertime)
        Me.SpecialHolidayRestDayNightDifferential = New Rate("Special Holiday Rest Day Night Differential", specialHolidayRestDayNightDifferential, Me.SpecialHolidayRestDay)
        Me.SpecialHolidayRestDayNightDifferentialOvertime = New Rate("Special Holiday Rest Day Night Differential OT", specialHolidayRestDayNightDifferentialOvertime)

        Me.RegularHoliday = New Rate("Regular Holiday", regularHoliday)
        Me.RegularHolidayOvertime = New Rate("Regular Holiday OT", regularHolidayOvertime)
        Me.RegularHolidayNightDifferential = New Rate("Regular Holiday Night Differential", regularHolidayNightDifferential, Me.RegularHoliday)
        Me.RegularHolidayNightDifferentialOvertime = New Rate("Regular Holiday Night Differential OT", regularHolidayNightDifferentialOvertime)
        Me.RegularHolidayRestDay = New Rate("Regular Holiday Rest Day", regularHolidayRestDay)
        Me.RegularHolidayRestDayOvertime = New Rate("Regular Holiday Rest Day OT", regularHolidayRestDayOvertime)
        Me.RegularHolidayRestDayNightDifferential = New Rate("Regular Holiday Rest Day Night Differential", regularHolidayRestDayNightDifferential, Me.RegularHolidayRestDay)
        Me.RegularHolidayRestDayNightDifferentialOvertime = New Rate("Regular Holiday Rest Day Night Differential OT", regularHolidayRestDayNightDifferentialOvertime)

    End Sub

    Public Class RateGroup
        Public Property BasePay As Rate
        Public Property Overtime As Rate
        Public Property NightDifferential As Rate
        Public Property NightDifferentialOvertime As Rate

        Public Property RestDay As Rate
        Public Property RestDayOvertime As Rate
        Public Property RestDayNightDifferential As Rate
        Public Property RestDayNightDifferentialOvertime As Rate

    End Class

End Class