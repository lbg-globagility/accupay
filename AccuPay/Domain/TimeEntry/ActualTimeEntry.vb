Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeetimeentryactual")>
    Public Class ActualTimeEntry

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property EmployeeID As Integer?

        Public Property EmployeeShiftID As Integer?

        Public Property EmployeeSalaryID As Integer?

        Public Property PayRateID As Integer?

        <Column("Date")>
        Public Property [Date] As Date

        <Column("RegularHoursAmount")>
        Public Property RegularPay As Decimal

        <Column("OvertimeHoursAmount")>
        Public Property OvertimePay As Decimal

        <Column("NightDiffHoursAmount")>
        Public Property NightDiffPay As Decimal

        <Column("NightDiffOTHoursAmount")>
        Public Property NightDiffOTPay As Decimal

        <Column("RestDayAmount")>
        Public Property RestDayPay As Decimal

        Public Property RestDayOTPay As Decimal

        <Column("Leavepayment")>
        Public Property LeavePay As Decimal

        Public Property SpecialHolidayPay As Decimal

        Public Property SpecialHolidayOTPay As Decimal

        Public Property RegularHolidayPay As Decimal

        Public Property RegularHolidayOTPay As Decimal

        <Column("HolidayPayAmount")>
        Public Property HolidayPay As Decimal

        <Column("HoursLateAmount")>
        Public Property LateDeduction As Decimal

        <Column("UndertimeHoursAmount")>
        Public Property UndertimeDeduction As Decimal

        <Column("Absent")>
        Public Property AbsentDeduction As Decimal

        <Column("BasicDayPay")>
        Public Property BasicDayPay As Decimal

        <Column("TotalDayPay")>
        Public Property TotalDayPay As Decimal

        <ForeignKey("EmployeeShiftID")>
        Public Overridable Property ShiftSchedule As ShiftSchedule

    End Class

End Namespace
