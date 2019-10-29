Option Strict On

Namespace Benchmark

    Public Class OvertimeInput

        Public Property OvertimeType As Rate

        Public Property Input As Decimal
        Public Property IsDay As Boolean
        Public Property IsHolidayInclusive As Boolean

        Private Property _payPerHour As Decimal

        Sub New(overtimeType As Rate, input As Decimal, isDay As Boolean, payperHour As Decimal, Optional isHolidayInclusive As Boolean = False)

            Me.OvertimeType = overtimeType
            Me.Input = input
            Me.IsDay = isDay
            Me.IsHolidayInclusive = isHolidayInclusive

            _payPerHour = payperHour

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
                Dim overtimeRate = If(IsHolidayInclusive, OvertimeType.Rate - 1, OvertimeType.Rate)
                Dim rate = If(OvertimeType.BaseRate Is Nothing, overtimeRate, overtimeRate - OvertimeType.BaseRate.Rate)

                Return AccuMath.CommercialRound((Hours * rate * _payPerHour))
            End Get
        End Property

    End Class

End Namespace