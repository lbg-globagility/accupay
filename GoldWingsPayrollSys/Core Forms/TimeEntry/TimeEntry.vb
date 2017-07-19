Public Class TimeEntry

    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property EmployeeID As Integer?

    Public Property EmployeShiftID As Integer?

    Public Property EmployeeSalaryID As Integer?

    Public Property PayRateID As Integer?

    Public Property EntryDate As Date

    Public Property TimeIn As TimeSpan?

    Public Property TimeOut As TimeSpan?

    Public Property RegularHours As Decimal

    Public Property RegularPay As Decimal

    Public Property OvertimeHours As Decimal

    Public Property OvertimePay As Decimal

    Public Property NightDiffHours As Decimal

    Public Property NightDiffPay As Decimal

    Public Property NightDiffOTHours As Decimal

    Public Property NightDiffOTPay As Decimal

    Public Property RestDayHours As Decimal

    Public Property RestDayPay As Decimal

    Public Property LeavePay As Decimal

    Public Property HolidayPay As Decimal

    Public Property LateHours As Decimal

    Public Property LateDeduction As Decimal

    Public Property UndertimeHours As Decimal

    Public Property UndertimePay As Decimal

    Public Property AbsentDeduction As Decimal

    Public Property TotalDayPay As Decimal

    Public Function DateTomorrow() As DateTime
        Return EntryDate.AddDays(1)
    End Function

End Class
