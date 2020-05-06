Option Strict On

Imports AccuPay.Data.ValueObjects
Imports AccuPay.Utilities

Namespace Benchmark

    Public Class OvertimeInput

        Public Property OvertimeType As Rate

        Public Property Input As Decimal
        Public Property IsDay As Boolean
        Public Property IsHolidayInclusive As Boolean

        'premium days that can include the regular pay
        'RegularHoliday, SpecialHoliday
        Public Property IsHolidayThatIsNotRestDay As Boolean

        Private Property _payPerHour As Decimal

        Public Shared NotRestDayHolidays As String() = {
            OvertimeRate.RegularHolidayDescription,
            OvertimeRate.SpecialHolidayDescription
        }

        Sub New(overtimeType As Rate, input As Decimal, isDay As Boolean, payperHour As Decimal, Optional isHolidayInclusive As Boolean = False)

            Me.OvertimeType = overtimeType
            Me.Input = input
            Me.IsDay = isDay
            Me.IsHolidayInclusive = isHolidayInclusive

            _payPerHour = payperHour

            Me.IsHolidayThatIsNotRestDay = NotRestDayHolidays.Contains(overtimeType.Name)

        End Sub

        Public ReadOnly Property Description As String
            Get
                Return OvertimeType?.Name
            End Get
        End Property

        Public ReadOnly Property Hours As Decimal
            Get
                Return If(IsDay, Input * BenchmarkPaystubRate.WorkHoursPerDay, Input)
            End Get
        End Property

        Public ReadOnly Property Amount As Decimal
            Get
                Dim overtimeRate = If(IsHolidayInclusive AndAlso IsHolidayThatIsNotRestDay, OvertimeType.CurrentRate - 1, OvertimeType.CurrentRate)
                Dim rate = If(OvertimeType.BaseRate Is Nothing, overtimeRate, overtimeRate - OvertimeType.BaseRate.CurrentRate)

                Return AccuMath.CommercialRound((Hours * rate * _payPerHour))
            End Get
        End Property

    End Class

End Namespace