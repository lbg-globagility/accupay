Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("paystubactual")>
    Public Class PaystubActual

        <Key>
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

        <Obsolete>
        Public Property HolidayPay As Decimal

        Public Property LateDeduction As Decimal

        Public Property UndertimeDeduction As Decimal

        Public Property AbsenceDeduction As Decimal

        <NotMapped>
        Public Property TotalEarnings As Decimal

        <Column("TotalGrossSalary")>
        Public Property GrossPay As Decimal

        Public Property TotalAdjustments As Decimal

        <Column("TotalNetSalary")>
        Public Property NetPay As Decimal

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

        <ForeignKey("RowID")>
        Public Overridable Property Paystub As Paystub

        Public Property RestDayNightDiffPay As Decimal
        Public Property RestDayNightDiffOTPay As Decimal

        Public Property SpecialHolidayNightDiffPay As Decimal
        Public Property SpecialHolidayNightDiffOTPay As Decimal
        Public Property SpecialHolidayRestDayPay As Decimal
        Public Property SpecialHolidayRestDayOTPay As Decimal
        Public Property SpecialHolidayRestDayNightDiffPay As Decimal
        Public Property SpecialHolidayRestDayNightDiffOTPay As Decimal

        Public Property RegularHolidayNightDiffPay As Decimal
        Public Property RegularHolidayNightDiffOTPay As Decimal
        Public Property RegularHolidayRestDayPay As Decimal
        Public Property RegularHolidayRestDayOTPay As Decimal
        Public Property RegularHolidayRestDayNightDiffPay As Decimal
        Public Property RegularHolidayRestDayNightDiffOTPay As Decimal

        Public ReadOnly Property AdditionalPay As Decimal
            Get
                Dim original =
                    OvertimePay +
                    NightDiffPay +
                    NightDiffOvertimePay +
                    RestDayPay +
                    RestDayOTPay +
                    SpecialHolidayPay +
                    SpecialHolidayOTPay +
                    RegularHolidayPay +
                    RegularHolidayOTPay

                Dim newBreakdowns =
                    RestDayNightDiffPay +
                    RestDayNightDiffOTPay +
                    SpecialHolidayNightDiffPay +
                    SpecialHolidayNightDiffOTPay +
                    SpecialHolidayRestDayPay +
                    SpecialHolidayRestDayOTPay +
                    SpecialHolidayRestDayNightDiffPay +
                    SpecialHolidayRestDayNightDiffOTPay +
                    RegularHolidayNightDiffPay +
                    RegularHolidayNightDiffOTPay +
                    RegularHolidayRestDayPay +
                    RegularHolidayRestDayOTPay +
                    RegularHolidayRestDayNightDiffPay +
                    RegularHolidayRestDayNightDiffOTPay

                Return original + newBreakdowns
            End Get
        End Property

        Public ReadOnly Property BasicDeductions As Decimal
            Get
                Return LateDeduction + UndertimeDeduction + AbsenceDeduction
            End Get
        End Property

    End Class

End Namespace