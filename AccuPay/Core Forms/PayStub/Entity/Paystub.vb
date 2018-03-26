Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Acupay

Namespace Global.AccuPay.Entity

    <Table("paystub")>
    Public Class Paystub

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property RowID As Integer?

        Public Overridable Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property Created As Date

        Public Overridable Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Overridable Property LastUpd As Date?

        Public Overridable Property LastUpdBy As Integer?

        Public Overridable Property PayPeriodID As Integer?

        Public Overridable Property EmployeeID As Integer?

        Public Overridable Property TimeEntryID As Integer?

        Public Overridable Property PayFromdate As Date

        Public Overridable Property PayToDate As Date

        Public Overridable Property RegularHours As Decimal

        Public Overridable Property RegularPay As Decimal

        Public Overridable Property OvertimeHours As Decimal

        Public Overridable Property OvertimePay As Decimal

        Public Overridable Property NightDiffHours As Decimal

        Public Overridable Property NightDiffPay As Decimal

        Public Overridable Property NightDiffOvertimeHours As Decimal

        Public Overridable Property NightDiffOvertimePay As Decimal

        Public Overridable Property RestDayHours As Decimal

        Public Overridable Property RestDayPay As Decimal

        Public Overridable Property RestDayOTHours As Decimal

        Public Overridable Property RestDayOTPay As Decimal

        Public Overridable Property LeaveHours As Decimal

        Public Overridable Property LeavePay As Decimal

        Public Overridable Property SpecialHolidayHours As Decimal

        Public Overridable Property SpecialHolidayPay As Decimal

        Public Overridable Property SpecialHolidayOTHours As Decimal

        Public Overridable Property SpecialHolidayOTPay As Decimal

        Public Overridable Property RegularHolidayHours As Decimal

        Public Overridable Property RegularHolidayPay As Decimal

        Public Overridable Property RegularHolidayOTHours As Decimal

        Public Overridable Property RegularHolidayOTPay As Decimal

        Public Overridable Property HolidayPay As Decimal

        Public Overridable Property LateHours As Decimal

        Public Overridable Property LateDeduction As Decimal

        Public Overridable Property UndertimeHours As Decimal

        Public Overridable Property UndertimeDeduction As Decimal

        Public Overridable Property AbsentHours As Decimal

        Public Overridable Property AbsenceDeduction As Decimal

        <Column("WorkPay")>
        Public Overridable Property TotalEarnings As Decimal

        Public Overridable Property TotalBonus As Decimal

        Public Overridable Property TotalAllowance As Decimal

        <Column("TotalGrossSalary")>
        Public Overridable Property GrossPay As Decimal

        <Column("TotalTaxableSalary")>
        Public Overridable Property TaxableIncome As Decimal

        <Column("TotalEmpWithholdingTax")>
        Public Overridable Property WithholdingTax As Decimal

        <Column("TotalEmpSSS")>
        Public Overridable Property SssEmployeeShare As Decimal

        <Column("TotalCompSSS")>
        Public Overridable Property SssEmployerShare As Decimal

        <Column("TotalEmpPhilhealth")>
        Public Overridable Property PhilHealthEmployeeShare As Decimal

        <Column("TotalCompPhilhealth")>
        Public Overridable Property PhilHealthEmployerShare As Decimal

        <Column("TotalEmpHDMF")>
        Public Overridable Property HdmfEmployeeShare As Decimal

        <Column("TotalCompHDMF")>
        Public Overridable Property HdmfEmployerShare As Decimal

        Public Overridable Property TotalVacationDaysLeft As Decimal

        Public Overridable Property TotalUndeclaredSalary As Decimal

        Public Overridable Property TotalLoans As Decimal

        Public Overridable Property TotalAdjustments As Decimal

        <Column("TotalNetSalary")>
        Public Overridable Property NetPay As Decimal

        Public Overridable Property ThirteenthMonthInclusion As Boolean

        Public Overridable Property FirstTimeSalary As Boolean

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

        Public Overridable Property Adjustments As ICollection(Of Adjustment)

        Public Overridable Property ActualAdjustments As ICollection(Of ActualAdjustment)

        Public Overridable Property PaystubItems As ICollection(Of PaystubItem)

        Public Overridable Property AllowanceItems As IList(Of AllowanceItem)

        <NotMapped>
        Public Overridable Property ThirteenthMonthPay As ThirteenthMonthPay

        Public Sub New()
            Adjustments = New List(Of Adjustment)
            ActualAdjustments = New List(Of ActualAdjustment)
            PaystubItems = New List(Of PaystubItem)
            AllowanceItems = New List(Of AllowanceItem)
        End Sub

    End Class

End Namespace