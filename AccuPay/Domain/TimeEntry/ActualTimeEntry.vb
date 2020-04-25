Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Data

Namespace Global.AccuPay.Entity

    <Table("employeetimeentryactual")>
    Public Class ActualTimeEntry
        Implements IActualTimeEntry

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer? Implements IActualTimeEntry.RowID

        Public Property OrganizationID As Integer? Implements IActualTimeEntry.OrganizationID

        Public Property EmployeeID As Integer? Implements IActualTimeEntry.EmployeeID

        Public Property EmployeeShiftID As Integer? Implements IActualTimeEntry.EmployeeShiftID

        Public Property EmployeeSalaryID As Integer? Implements IActualTimeEntry.EmployeeSalaryID

        Public Property PayRateID As Integer? Implements IActualTimeEntry.PayRateID

        <Column("Date")>
        Public Property [Date] As Date Implements IActualTimeEntry.Date

        <Column("RegularHoursAmount")>
        Public Property RegularPay As Decimal Implements IActualTimeEntry.RegularPay

        <Column("OvertimeHoursAmount")>
        Public Property OvertimePay As Decimal Implements IActualTimeEntry.OvertimePay

        <Column("NightDiffHoursAmount")>
        Public Property NightDiffPay As Decimal Implements IActualTimeEntry.NightDiffPay

        <Column("NightDiffOTHoursAmount")>
        Public Property NightDiffOTPay As Decimal Implements IActualTimeEntry.NightDiffOTPay

        <Column("RestDayAmount")>
        Public Property RestDayPay As Decimal Implements IActualTimeEntry.RestDayPay

        Public Property RestDayOTPay As Decimal Implements IActualTimeEntry.RestDayOTPay

        <Column("Leavepayment")>
        Public Property LeavePay As Decimal Implements IActualTimeEntry.LeavePay

        Public Property SpecialHolidayPay As Decimal Implements IActualTimeEntry.SpecialHolidayPay

        Public Property SpecialHolidayOTPay As Decimal Implements IActualTimeEntry.SpecialHolidayOTPay

        Public Property RegularHolidayPay As Decimal Implements IActualTimeEntry.RegularHolidayPay

        Public Property RegularHolidayOTPay As Decimal Implements IActualTimeEntry.RegularHolidayOTPay

        <Column("HolidayPayAmount")>
        Public Property HolidayPay As Decimal Implements IActualTimeEntry.HolidayPay

        <Column("HoursLateAmount")>
        Public Property LateDeduction As Decimal Implements IActualTimeEntry.LateDeduction

        <Column("UndertimeHoursAmount")>
        Public Property UndertimeDeduction As Decimal Implements IActualTimeEntry.UndertimeDeduction

        <Column("Absent")>
        Public Property AbsentDeduction As Decimal Implements IActualTimeEntry.AbsentDeduction

        <Column("BasicDayPay")>
        Public Property BasicDayPay As Decimal Implements IActualTimeEntry.BasicDayPay

        <Column("TotalDayPay")>
        Public Property TotalDayPay As Decimal Implements IActualTimeEntry.TotalDayPay

        <ForeignKey("EmployeeShiftID")>
        Public Overridable Property ShiftSchedule As ShiftSchedule

    End Class

End Namespace