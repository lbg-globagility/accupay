Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Acupay

Namespace Global.AccuPay.Entity

    <Table("paystub")>
    Public Class Paystub

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

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

        Public Property RestDayPay As Decimal

        Public Property LeavePay As Decimal

        Public Property HolidayPay As Decimal

        Public Property LateHours As Decimal

        Public Property LateDeduction As Decimal

        Public Property UndertimeHours As Decimal

        Public Property UndertimeDeduction As Decimal

        Public Property AbsenceDeduction As Decimal

        Public Property WorkPay As Decimal

        Public Property TotalBonus As Decimal

        Public Property TotalAllowance As Decimal

        <Column("TotalGrossSalary")>
        Public Property GrossPay As Decimal

        <Column("TotalTaxableSalary")>
        Public Property TaxableIncome As Decimal

        <Column("TotalEmpWithholdingTax")>
        Public Property WithholdingTax As Decimal

        <Column("TotalEmpSSS")>
        Public Property SssEmployeeShare As Decimal

        <Column("TotalCompSSS")>
        Public Property SssEmployerShare As Decimal

        <Column("TotalEmpPhilhealth")>
        Public Property PhilHealthEmployeeShare As Decimal

        <Column("TotalCompPhilhealth")>
        Public Property PhilHealthEmployerShare As Decimal

        <Column("TotalEmpHDMF")>
        Public Property HdmfEmployeeShare As Decimal

        <Column("TotalCompHDMF")>
        Public Property HdmfEmployerShare As Decimal

        Public Property TotalVacationDaysLeft As Decimal

        Public Property TotalUndeclaredSalary As Decimal

        Public Property TotalLoans As Decimal

        Public Property TotalAdjustments As Decimal

        <Column("TotalNetSalary")>
        Public Property NetPay As Decimal

        Public Property ThirteenthMonthInclusion As Boolean

        Public Property FirstTimeSalary As Boolean

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

        Public Overridable Property Adjustments As ICollection(Of Adjustment)

    End Class

End Namespace