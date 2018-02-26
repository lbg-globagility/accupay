Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Acupay

Namespace Global.AccuPay.Entity

    <Table("paystubactual")>
    Public Class PaystubActual

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property PayPeriodID As Integer?

        Public Property EmployeeID As Integer?

        Public Property PayFromDate As Date

        Public Property PayToDate As Date

        Public Property RegularPay As Decimal

        Public Property OvertimePay As Decimal

        Public Property NightDiffPay As Decimal

        Public Property NightDiffOvertimePay As Decimal

        Public Property RestDayPay As Decimal

        Public Property RestDayOTPay As Decimal

        Public Property LeavePay As Decimal

        Public Property SpecialHolidayPay As Decimal

        Public Property SpecialHolidayOTPay As Decimal

        Public Property RegularHolidayPay As Decimal

        Public Property RegularHolidayOTPay As Decimal

        Public Property HolidayPay As Decimal

        Public Property LateDeduction As Decimal

        Public Property UndertimeDeduction As Decimal

        Public Property AbsenceDeduction As Decimal

        <Column("TotalGrossSalary")>
        Public Property GrossPay As Decimal

        Public Property TotalAdjustments As Decimal

        <Column("TotalNetSalary")>
        Public Property NetPay As Decimal

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

    End Class

End Namespace