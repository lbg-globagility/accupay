Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeetimeentry")>
    Public Class TimeEntry

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer?

        <Column("Date")>
        Public Property EntryDate As Date

        Public Property EmployeeShiftID As Integer?

        Public Property EmployeeID As Integer?

        Public Property EmployeeSalaryID As Integer?

        Public Property EmployeeFixedSalaryFlag As Char

        Public Property RegularHoursWorked As Decimal

        Public Property RegularHoursAmount As Decimal

        Public Property TotalHoursWorked As Decimal

        Public Property OvertimeHoursWorked As Decimal

        Public Property OvertimeHoursAmount As Decimal

        Public Property UndertimeHours As Decimal

        Public Property UndertimeHoursAmount As Decimal

        Public Property NightDifferentialHours As Decimal

        Public Property NightDiffHoursAmount As Decimal

        Public Property NightDifferentialOTHours As Decimal

        Public Property NightDiffOTHoursAmount As Decimal

        Public Property HoursLate As Decimal

        Public Property HoursLateAmount As Decimal

        Public Property LateFlag As String

        Public Property PayRateID As Integer?

        Public Property VacationLeaveHours As Decimal

        Public Property SickLeaveHours As Decimal

        Public Property MaternityLeaveHours As Decimal

        Public Property OtherLeaveHours As Decimal

        Public Property TotalDayPay As Decimal

        Public Property Absent As Decimal

        Public Property ChargeToDivisionID As Integer?

        Public Property TaxableDailyAllowance As Decimal

        Public Property HolidayPayAmount As Decimal

        Public Property TaxableDailyBonus As Decimal

        Public Property NonTaxableDailyBonus As Decimal

        Public Property Leavepayment As Decimal

        Public Property BasicDayPay As Decimal

        Public Property RestDayHours As Decimal

        Public Property RestDayAmount As Decimal

    End Class

End Namespace