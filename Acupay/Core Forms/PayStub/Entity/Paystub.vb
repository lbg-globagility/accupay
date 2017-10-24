Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("paystub")>
    Public Class Paystub

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer?

        Public Property PayPeriodID As Integer?

        Public Property EmployeeID As Integer?

        Public Property TimeEntryID As Integer?

        Public Property PayFromdate As Date

        Public Property PayToDate As Date

        Public Property RegularHours As Decimal

        Public Property RegularPay As Decimal

        Public Property OvertimeHours As Decimal

        Public Property OvertimePay As Decimal

        Public Property NightDiffHours As Decimal

        Public Property NightDiffPay As Decimal

        Public Property NightDiffOvertimeHours As Decimal

        Public Property NightDiffOvertimePay As Decimal

        Public Property RestDayHours As Decimal

        Public Property RestDaypay As Decimal

        Public Property LeavePay As Decimal

        Public Property HolidayPay As Decimal

        Public Property LateHours As Decimal

        Public Property LateDeduction As Decimal

        Public Property UndertimeHours As Decimal

        Public Property UndertimeDeduction As Decimal

        Public Property AbsenceDeduction As Decimal

        Public Property WorkPay As Decimal

        Public Property TotalGrossSalary As Decimal

        Public Property TotalNetSalary As Decimal

        Public Property TotalTaxableSalary As Decimal

        Public Property TotalEmpSSS As Decimal

        Public Property TotalEmpWithholdingTax As Decimal

        Public Property TotalCompPhilhealth As Decimal

        Public Property TotalEmpHDMF As Decimal

        Public Property TotalCompHDMF As Decimal

        Public Property TotalVacationDaysLeft As Decimal

        Public Property TotalUndeclaredSalary As Decimal

        Public Property TotalLoans As Decimal

        Public Property TotalBonus As Decimal

        Public Property TotalAllowance As Decimal

        Public Property TotalAdjustments As Decimal

        Public Property ThirteenthMonthInclusion As Boolean

        Public Property FirstTimeSalary As Boolean

    End Class

End Namespace