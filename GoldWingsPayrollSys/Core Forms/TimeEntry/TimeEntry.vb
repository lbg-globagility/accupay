Option Strict On

Public Class TimeEntry

    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property EmployeeID As Integer?

    Public Property EmployeShiftID As Integer?

    Public Property EmployeeSalaryID As Integer?

    Public Property PayRateID As Integer?

    Public Property EntryDate As Date

    Public Property RegularHours As Decimal

    Public Property RegularPay As Decimal

    Public Property OvertimeHours As Decimal

    Public Property OvertimePay As Decimal

    Public Property NightDiffHours As Decimal

    Public Property NightDiffPay As Decimal

    Public Property NightDiffOvertimeHours As Decimal

    Public Property NightDiffOvertimePay As Decimal

    Public Property RestDayHours As Decimal

    Public Property RestDayPay As Decimal

    Public Property LeavePay As Decimal

    Public Property HolidayPay As Decimal

    Public Property LateHours As Decimal

    Public Property LateDeduction As Decimal

    Public Property UndertimeHours As Decimal

    Public Property UndertimePay As Decimal

    Public Property AbsenceDeduction As Decimal

    Public Property TotalDayPay As Decimal

    Public Property DutyStart As Date

    Public Property DutyEnd As Date

    Public Sub New(timeLog As TimeLog, shiftToday As ShiftToday)
        Dim timeIn = timeLog.FullTimeIn
        Dim timeOut = timeLog.FullTimeOut

        DutyStart = {timeIn, shiftToday.RangeStart}.Max
        DutyEnd = {timeOut, shiftToday.RangeEnd}.Min
    End Sub

End Class
