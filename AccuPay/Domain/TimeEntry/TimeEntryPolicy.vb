Option Strict On

Public Class TimeEntryPolicy

    Private ReadOnly _settings As ListOfValueCollection

    Public Sub New(settings As ListOfValueCollection)
        _settings = settings
    End Sub

    Public ReadOnly Property LateHoursRoundingUp As Boolean
        Get
            Return _settings.GetBoolean("LateHours.RoundingUp")
        End Get
    End Property

    Public ReadOnly Property NightDiffBreakTime As Boolean
        Get
            Return _settings.GetBoolean("NightDiffPolicy.HasBreakTime")
        End Get
    End Property

    Public ReadOnly Property LateSkipCountRounding As Boolean
        Get
            Return _settings.GetBoolean("LateHours.SkipCountRounding")
        End Get
    End Property

    Public ReadOnly Property ComputeBreakTimeLate As Boolean
        Get
            Return _settings.GetBoolean("LateHours.ComputeBreakTimeLate")
        End Get
    End Property

    Public ReadOnly Property OvertimeSkipCountRounding As Boolean
        Get
            Return _settings.GetBoolean("OvertimeHours.SkipCountRounding")
        End Get
    End Property

    Public ReadOnly Property AbsencesOnHoliday As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.holiday.allowabsence")
        End Get
    End Property

    Public ReadOnly Property RequiredToWorkLastDay As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.HolidayLastWorkingDayOrAbsent")
        End Get
    End Property

    Public ReadOnly Property RequiredToWorkLastDayForHolidayPay As Boolean
        Get
            Return _settings.GetBoolean("HolidayPolicy.WorkLastDayForHolidayPay")
        End Get
    End Property

    Public ReadOnly Property HasNightBreaktime As Boolean
        Get
            Return _settings.GetBoolean("OvertimePolicy.NightBreaktime")
        End Get
    End Property

    Public ReadOnly Property RespectDefaultRestDay As Boolean
        Get
            Return _settings.GetBoolean("RestDayPolicy.RespectDefaultRestDay")
        End Get
    End Property

    Public ReadOnly Property IgnoreShiftOnRestDay As Boolean
        Get
            Return _settings.GetBoolean("RestDayPolicy.IgnoreShiftOnRestDay")
        End Get
    End Property

    Public ReadOnly Property RestDayInclusive As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.restday.inclusiveofbasicpay")
        End Get
    End Property

End Class
