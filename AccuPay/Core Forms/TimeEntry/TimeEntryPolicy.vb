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

    Public ReadOnly Property RestDayInclusive As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.restday.inclusiveofbasicpay")
        End Get
    End Property

End Class
