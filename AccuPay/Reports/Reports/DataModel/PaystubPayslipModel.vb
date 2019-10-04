Imports System.Text

Public Class PaystubPayslipModel

    Private Const MoneyFormat As String = "#,##0.00"

    Public Property EmployeeId As Integer
    Public Property EmployeeNumber As String
    Public Property EmployeeName As String
    Public Property RegularPay As Decimal
    Public Property BasicHours As Decimal
    Public Property BasicPay As Decimal
    Public Property Allowance As Decimal
    Public Property Ecola As Decimal
    Public Property AbsentHours As Decimal
    Public Property AbsentAmount As Decimal
    Public Property LateAndUndertimeHours As Decimal
    Public Property LateAndUndertimeAmount As Decimal
    Public Property GrossPay As Decimal
    Public Property SSSAmount As Decimal
    Public Property PhilHealthAmount As Decimal
    Public Property PagibigAmount As Decimal
    Public Property TaxWithheldAmount As Decimal
    Public Property LeaveHours As Decimal
    Public Property LeavePay As Decimal

    'total overtime and night differential
    'total loan
    'total deduction (Formula)

    Public Property NetPay As Decimal

    Public Property TotalOvertimeHours As Decimal
    Public Property TotalOvertimePay As Decimal

#Region "Overtimes Breakdowns and Summary"

    Private _overtimeNamesSummary As String

    Public ReadOnly Property OvertimeNamesSummary As String
        Get
            Return _overtimeNamesSummary
        End Get
    End Property

    Private _overtimeHoursSummary As String

    Public ReadOnly Property OvertimeHoursSummary As String
        Get
            Return _overtimeHoursSummary
        End Get
    End Property

    Private _overtimeAmountsSummary As String

    Public ReadOnly Property OvertimeAmountsSummary As String
        Get
            Return _overtimeAmountsSummary
        End Get
    End Property

    Public Function CreateOvertimeSummaryColumns() As PaystubPayslipModel

        TotalOvertimeHours = 0
        TotalOvertimePay = 0

        Dim overtimeNamesSummaryBuilder As New StringBuilder
        Dim overtimeHoursSummaryBuilder As New StringBuilder
        Dim overtimeAmountsSummaryBuilder As New StringBuilder

        If OvertimePay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Overtime")
            overtimeHoursSummaryBuilder.AppendLine(OvertimeHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(OvertimePay.ToString(MoneyFormat))
            TotalOvertimeHours += OvertimeHours
            TotalOvertimePay += OvertimePay
        End If
        If NightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Night Diff")
            overtimeHoursSummaryBuilder.AppendLine(NightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(NightDiffPay.ToString(MoneyFormat))
            TotalOvertimeHours += NightDiffHours
            TotalOvertimePay += NightDiffPay
        End If
        If NightDiffOvertimePay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Night Diff OT")
            overtimeHoursSummaryBuilder.AppendLine(NightDiffOvertimeHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(NightDiffOvertimePay.ToString(MoneyFormat))
            TotalOvertimeHours += NightDiffOvertimeHours
            TotalOvertimePay += NightDiffOvertimePay
        End If
        If RestDayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Rest Day")
            overtimeHoursSummaryBuilder.AppendLine(RestDayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RestDayPay.ToString(MoneyFormat))
            TotalOvertimeHours += RestDayHours
            TotalOvertimePay += RestDayPay
        End If
        If RestDayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Rest Day OT")
            overtimeHoursSummaryBuilder.AppendLine(RestDayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RestDayOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += RestDayOTHours
            TotalOvertimePay += RestDayOTPay
        End If
        If SpecialHolidayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Special Holiday")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayPay.ToString(MoneyFormat))
            TotalOvertimeHours += SpecialHolidayHours
            TotalOvertimePay += SpecialHolidayPay
        End If
        If SpecialHolidayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Special Holiday OT")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += SpecialHolidayOTHours
            TotalOvertimePay += SpecialHolidayOTPay
        End If
        If RegularHolidayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Regular Holiday")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayPay.ToString(MoneyFormat))
            TotalOvertimeHours += RegularHolidayHours
            TotalOvertimePay += RegularHolidayPay
        End If
        If RegularHolidayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Regular Holiday OT")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += RegularHolidayOTHours
            TotalOvertimePay += RegularHolidayOTPay
        End If
        If RestDayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Rest Day ND")
            overtimeHoursSummaryBuilder.AppendLine(RestDayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RestDayNightDiffPay.ToString(MoneyFormat))
            TotalOvertimeHours += RestDayNightDiffHours
            TotalOvertimePay += RestDayNightDiffPay
        End If
        If RestDayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Rest Day ND OT")
            overtimeHoursSummaryBuilder.AppendLine(RestDayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RestDayNightDiffOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += RestDayNightDiffOTHours
            TotalOvertimePay += RestDayNightDiffOTPay
        End If
        If SpecialHolidayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi ND")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayNightDiffPay.ToString(MoneyFormat))
            TotalOvertimeHours += SpecialHolidayNightDiffHours
            TotalOvertimePay += SpecialHolidayNightDiffPay
        End If
        If SpecialHolidayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi ND OT")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayNightDiffOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += SpecialHolidayNightDiffOTHours
            TotalOvertimePay += SpecialHolidayNightDiffOTPay
        End If
        If SpecialHolidayRestDayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi RD")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayPay.ToString(MoneyFormat))
            TotalOvertimeHours += SpecialHolidayRestDayHours
            TotalOvertimePay += SpecialHolidayRestDayPay
        End If
        If SpecialHolidayRestDayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi RD OT")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += SpecialHolidayRestDayOTHours
            TotalOvertimePay += SpecialHolidayRestDayOTPay
        End If
        If SpecialHolidayRestDayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi RD ND")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffPay.ToString(MoneyFormat))
            TotalOvertimeHours += SpecialHolidayRestDayNightDiffHours
            TotalOvertimePay += SpecialHolidayRestDayNightDiffPay
        End If
        If SpecialHolidayRestDayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi RD ND OT")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += SpecialHolidayRestDayNightDiffOTHours
            TotalOvertimePay += SpecialHolidayRestDayNightDiffOTPay
        End If
        If RegularHolidayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. ND")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayNightDiffPay.ToString(MoneyFormat))
            TotalOvertimeHours += RegularHolidayNightDiffHours
            TotalOvertimePay += RegularHolidayNightDiffPay
        End If
        If RegularHolidayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. ND OT")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayNightDiffOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += RegularHolidayNightDiffOTHours
            TotalOvertimePay += RegularHolidayNightDiffOTPay
        End If
        If RegularHolidayRestDayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayPay.ToString(MoneyFormat))
            TotalOvertimeHours += RegularHolidayRestDayHours
            TotalOvertimePay += RegularHolidayRestDayPay
        End If
        If RegularHolidayRestDayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD OT")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += RegularHolidayRestDayOTHours
            TotalOvertimePay += RegularHolidayRestDayOTPay
        End If
        If RegularHolidayRestDayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD ND")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffPay.ToString(MoneyFormat))
            TotalOvertimeHours += RegularHolidayRestDayNightDiffHours
            TotalOvertimePay += RegularHolidayRestDayNightDiffPay
        End If
        If RegularHolidayRestDayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD ND OT")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffOTPay.ToString(MoneyFormat))
            TotalOvertimeHours += RegularHolidayRestDayNightDiffOTHours
            TotalOvertimePay += RegularHolidayRestDayNightDiffOTPay
        End If

        _overtimeNamesSummary = overtimeNamesSummaryBuilder.ToString
        _overtimeHoursSummary = overtimeHoursSummaryBuilder.ToString
        _overtimeAmountsSummary = overtimeAmountsSummaryBuilder.ToString

        Return Me
    End Function

    Public Property OvertimeHours As Decimal
    Public Property OvertimePay As Decimal
    Public Property NightDiffHours As Decimal
    Public Property NightDiffPay As Decimal
    Public Property NightDiffOvertimeHours As Decimal
    Public Property NightDiffOvertimePay As Decimal
    Public Property RestDayHours As Decimal
    Public Property RestDayPay As Decimal
    Public Property RestDayOTHours As Decimal
    Public Property RestDayOTPay As Decimal
    Public Property SpecialHolidayHours As Decimal
    Public Property SpecialHolidayPay As Decimal
    Public Property SpecialHolidayOTHours As Decimal
    Public Property SpecialHolidayOTPay As Decimal
    Public Property RegularHolidayHours As Decimal
    Public Property RegularHolidayPay As Decimal
    Public Property RegularHolidayOTHours As Decimal
    Public Property RegularHolidayOTPay As Decimal
    Public Property RestDayNightDiffHours As Decimal
    Public Property RestDayNightDiffPay As Decimal
    Public Property RestDayNightDiffOTHours As Decimal
    Public Property RestDayNightDiffOTPay As Decimal
    Public Property SpecialHolidayNightDiffHours As Decimal
    Public Property SpecialHolidayNightDiffPay As Decimal
    Public Property SpecialHolidayNightDiffOTHours As Decimal
    Public Property SpecialHolidayNightDiffOTPay As Decimal
    Public Property SpecialHolidayRestDayHours As Decimal
    Public Property SpecialHolidayRestDayPay As Decimal
    Public Property SpecialHolidayRestDayOTHours As Decimal
    Public Property SpecialHolidayRestDayOTPay As Decimal
    Public Property SpecialHolidayRestDayNightDiffHours As Decimal
    Public Property SpecialHolidayRestDayNightDiffPay As Decimal
    Public Property SpecialHolidayRestDayNightDiffOTHours As Decimal
    Public Property SpecialHolidayRestDayNightDiffOTPay As Decimal
    Public Property RegularHolidayNightDiffHours As Decimal
    Public Property RegularHolidayNightDiffPay As Decimal
    Public Property RegularHolidayNightDiffOTHours As Decimal
    Public Property RegularHolidayNightDiffOTPay As Decimal
    Public Property RegularHolidayRestDayHours As Decimal
    Public Property RegularHolidayRestDayPay As Decimal
    Public Property RegularHolidayRestDayOTHours As Decimal
    Public Property RegularHolidayRestDayOTPay As Decimal
    Public Property RegularHolidayRestDayNightDiffHours As Decimal
    Public Property RegularHolidayRestDayNightDiffPay As Decimal
    Public Property RegularHolidayRestDayNightDiffOTHours As Decimal
    Public Property RegularHolidayRestDayNightDiffOTPay As Decimal

#End Region

    Public Class Overtime

        Public Property Name As String
        Public Property Hours As Decimal
        Public Property Amount As Decimal

    End Class

End Class