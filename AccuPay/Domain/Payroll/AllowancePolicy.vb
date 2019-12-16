Option Strict On

Public Class AllowancePolicy

    Private ReadOnly _settings As ListOfValueCollection

    Public Sub New(settings As ListOfValueCollection)
        _settings = settings
    End Sub

    Public ReadOnly Property IsLeavePaid As Boolean
        Get
            Return _settings.GetBoolean("AllowancePolicy.IsLeavePaid")
        End Get
    End Property

    Public ReadOnly Property IsOvertimePaid As Boolean
        Get
            Return _settings.GetBoolean("AllowancePolicy.IsOvertimePaid")
        End Get
    End Property

    Public ReadOnly Property IsRestDayPaid As Boolean
        Get
            Return _settings.GetBoolean("AllowancePolicy.IsRestDayPaid")
        End Get
    End Property

    Public ReadOnly Property IsRestDayOTPaid As Boolean
        Get
            Return _settings.GetBoolean("AllowancePolicy.IsRestDayOTPaid")
        End Get
    End Property

    Public ReadOnly Property IsSpecialHolidayPaid As Boolean
        Get
            Return _settings.GetBoolean("AllowancePolicy.IsSpecialHolidayPaid")
        End Get
    End Property

    Public ReadOnly Property IsRegularHolidayPaid As Boolean
        Get
            Return _settings.GetBoolean("AllowancePolicy.IsRegularHolidayPaid")
        End Get
    End Property

End Class
