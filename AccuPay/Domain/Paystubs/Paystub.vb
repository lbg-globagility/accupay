Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Acupay

Namespace Global.AccuPay.Entity

    <Table("paystub")>
    Public Class Paystub

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property PayPeriodID As Integer?

        Public Property EmployeeID As Integer?

        Public Property TimeEntryID As Integer?

        Public Property PayFromdate As Date

        Public Property PayToDate As Date

        Public Property BasicHours As Decimal

        Public Property BasicPay As Decimal

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

        Public Property RestDayOTHours As Decimal

        Public Property RestDayOTPay As Decimal

        Public Property LeaveHours As Decimal

        Public Property LeavePay As Decimal

        Public Property SpecialHolidayHours As Decimal

        Public Property SpecialHolidayPay As Decimal

        Public Property SpecialHolidayOTHours As Decimal

        Public Property SpecialHolidayOTPay As Decimal

        Public Property RegularHolidayHours As Decimal

        Public Property RegularHolidayPay As Decimal

        Public Property RegularHolidayOTHours As Decimal

        Public Property RegularHolidayOTPay As Decimal

        Public Property HolidayPay As Decimal

        Public Property LateHours As Decimal

        Public Property LateDeduction As Decimal

        Public Property UndertimeHours As Decimal

        Public Property UndertimeDeduction As Decimal

        Public Property AbsentHours As Decimal

        Public Property AbsenceDeduction As Decimal

        <Column("WorkPay")>
        Public Property TotalEarnings As Decimal

        Public Property TotalBonus As Decimal

        Public Property TotalAllowance As Decimal

        Public Property TotalTaxableAllowance As Decimal

        <Column("TotalGrossSalary")>
        Public Property GrossPay As Decimal

        <Column("DeferredTaxableIncome")>
        Public Property DeferredTaxableIncome As Decimal

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

        Public Overridable Property ActualAdjustments As ICollection(Of ActualAdjustment)

        Public Overridable Property PaystubItems As ICollection(Of PaystubItem)

        Public Overridable Property AllowanceItems As ICollection(Of AllowanceItem)

        Public Overridable Property ThirteenthMonthPay As ThirteenthMonthPay

        Public ReadOnly Property AdditionalPay As Decimal
            Get
                Return (
                    OvertimePay +
                    NightDiffPay +
                    NightDiffOvertimePay +
                    RestDayPay +
                    RestDayOTPay +
                    SpecialHolidayPay +
                    SpecialHolidayOTPay +
                    RegularHolidayPay +
                    RegularHolidayOTPay)
            End Get
        End Property

        Public ReadOnly Property BasicDeductions As Decimal
            Get
                Return LateDeduction + UndertimeDeduction + AbsenceDeduction
            End Get
        End Property

        Public ReadOnly Property GovernmentDeductions As Decimal
            Get
                Return SssEmployeeShare + PhilHealthEmployeeShare + HdmfEmployeeShare
            End Get
        End Property

        Public ReadOnly Property NetDeductions As Decimal
            Get
                Return GovernmentDeductions + TotalLoans + WithholdingTax
            End Get
        End Property

        Public Sub New()
            Adjustments = New List(Of Adjustment)
            ActualAdjustments = New List(Of ActualAdjustment)
            PaystubItems = New List(Of PaystubItem)
            AllowanceItems = New List(Of AllowanceItem)
        End Sub

    End Class

End Namespace
