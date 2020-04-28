Option Strict On

Imports AccuPay.Data.Services

Public Class ActualTimeEntryPolicy

    Private ReadOnly _settings As ListOfValueCollection

    Public Sub New(settings As ListOfValueCollection)
        _settings = settings
    End Sub

    Public ReadOnly Property AllowanceForOvertime As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForOvertime", True)
        End Get
    End Property

    Public ReadOnly Property AllowanceForNightDiff As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForNightDifferential", True)
        End Get
    End Property

    Public ReadOnly Property AllowanceForNightDiffOT As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForNightDifferentialOvertime", True)
        End Get
    End Property

    Public ReadOnly Property AllowanceForRestDay As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForRestDay", True)
        End Get
    End Property

    Public ReadOnly Property AllowanceForRestDayOT As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForRestDayOT", True)
        End Get
    End Property

    Public ReadOnly Property AllowanceForHoliday As Boolean
        Get
            Return _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForHolidayPay", True)
        End Get
    End Property

End Class