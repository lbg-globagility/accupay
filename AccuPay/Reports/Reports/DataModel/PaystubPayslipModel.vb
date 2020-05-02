Option Strict On

Imports AccuPay.Data

''' <summary>
''' Anemic implementation of IPaystubPayslipModel just for Crystal Report data source
''' </summary>
Public Class PaystubPayslipModel
    Implements IPaystubPayslipModel

    Public ReadOnly Property EmployeeId As Integer Implements IPaystubPayslipModel.EmployeeId
    Public ReadOnly Property EmployeeNumber As String Implements IPaystubPayslipModel.EmployeeNumber
    Public ReadOnly Property EmployeeName As String Implements IPaystubPayslipModel.EmployeeName
    Public ReadOnly Property RegularPay As Decimal Implements IPaystubPayslipModel.RegularPay
    Public ReadOnly Property BasicHours As Decimal Implements IPaystubPayslipModel.BasicHours
    Public ReadOnly Property BasicPay As Decimal Implements IPaystubPayslipModel.BasicPay
    Public ReadOnly Property Allowance As Decimal Implements IPaystubPayslipModel.Allowance
    Public ReadOnly Property Ecola As Decimal Implements IPaystubPayslipModel.Ecola
    Public ReadOnly Property AbsentHours As Decimal Implements IPaystubPayslipModel.AbsentHours
    Public ReadOnly Property AbsentAmount As Decimal Implements IPaystubPayslipModel.AbsentAmount
    Public ReadOnly Property LateAndUndertimeHours As Decimal Implements IPaystubPayslipModel.LateAndUndertimeHours
    Public ReadOnly Property LateAndUndertimeAmount As Decimal Implements IPaystubPayslipModel.LateAndUndertimeAmount
    Public ReadOnly Property GrossPay As Decimal Implements IPaystubPayslipModel.GrossPay
    Public ReadOnly Property SSSAmount As Decimal Implements IPaystubPayslipModel.SSSAmount
    Public ReadOnly Property PhilHealthAmount As Decimal Implements IPaystubPayslipModel.PhilHealthAmount
    Public ReadOnly Property PagibigAmount As Decimal Implements IPaystubPayslipModel.PagibigAmount
    Public ReadOnly Property TaxWithheldAmount As Decimal Implements IPaystubPayslipModel.TaxWithheldAmount
    Public ReadOnly Property LeaveHours As Decimal Implements IPaystubPayslipModel.LeaveHours
    Public ReadOnly Property LeavePay As Decimal Implements IPaystubPayslipModel.LeavePay
    Public ReadOnly Property NetPay As Decimal Implements IPaystubPayslipModel.NetPay
    Public ReadOnly Property TotalDeductions As Decimal Implements IPaystubPayslipModel.TotalDeductions

#Region "Loans and Adjustments"

    Public ReadOnly Property TotalLoans As Decimal Implements IPaystubPayslipModel.TotalLoans
    Public ReadOnly Property TotalAdjustments As Decimal Implements IPaystubPayslipModel.TotalAdjustments
    Public ReadOnly Property LoanNamesSummary As String Implements IPaystubPayslipModel.LoanNamesSummary
    Public ReadOnly Property LoanAmountsSummary As String Implements IPaystubPayslipModel.LoanAmountsSummary
    Public ReadOnly Property LoanBalancesSummary As String Implements IPaystubPayslipModel.LoanBalancesSummary
    Public ReadOnly Property AdjustmentNamesSummary As String Implements IPaystubPayslipModel.AdjustmentNamesSummary
    Public ReadOnly Property AdjustmentAmountsSummary As String Implements IPaystubPayslipModel.AdjustmentAmountsSummary

#End Region

#Region "Overtimes Breakdowns and Summary"

    Public ReadOnly Property TotalOvertimeHours As Decimal Implements IPaystubPayslipModel.TotalOvertimeHours
    Public ReadOnly Property TotalOvertimePay As Decimal Implements IPaystubPayslipModel.TotalOvertimePay
    Public ReadOnly Property OvertimeNamesSummary As String Implements IPaystubPayslipModel.OvertimeNamesSummary
    Public ReadOnly Property OvertimeHoursSummary As String Implements IPaystubPayslipModel.OvertimeHoursSummary
    Public ReadOnly Property OvertimeAmountsSummary As String Implements IPaystubPayslipModel.OvertimeAmountsSummary
    Public ReadOnly Property OvertimeHours As Decimal Implements IPaystubPayslipModel.OvertimeHours
    Public ReadOnly Property OvertimePay As Decimal Implements IPaystubPayslipModel.OvertimePay
    Public ReadOnly Property NightDiffHours As Decimal Implements IPaystubPayslipModel.NightDiffHours
    Public ReadOnly Property NightDiffPay As Decimal Implements IPaystubPayslipModel.NightDiffPay
    Public ReadOnly Property NightDiffOvertimeHours As Decimal Implements IPaystubPayslipModel.NightDiffOvertimeHours
    Public ReadOnly Property NightDiffOvertimePay As Decimal Implements IPaystubPayslipModel.NightDiffOvertimePay
    Public ReadOnly Property RestDayHours As Decimal Implements IPaystubPayslipModel.RestDayHours
    Public ReadOnly Property RestDayPay As Decimal Implements IPaystubPayslipModel.RestDayPay
    Public ReadOnly Property RestDayOTHours As Decimal Implements IPaystubPayslipModel.RestDayOTHours
    Public ReadOnly Property RestDayOTPay As Decimal Implements IPaystubPayslipModel.RestDayOTPay
    Public ReadOnly Property SpecialHolidayHours As Decimal Implements IPaystubPayslipModel.SpecialHolidayHours
    Public ReadOnly Property SpecialHolidayPay As Decimal Implements IPaystubPayslipModel.SpecialHolidayPay
    Public ReadOnly Property SpecialHolidayOTHours As Decimal Implements IPaystubPayslipModel.SpecialHolidayOTHours
    Public ReadOnly Property SpecialHolidayOTPay As Decimal Implements IPaystubPayslipModel.SpecialHolidayOTPay
    Public ReadOnly Property RegularHolidayHours As Decimal Implements IPaystubPayslipModel.RegularHolidayHours
    Public ReadOnly Property RegularHolidayPay As Decimal Implements IPaystubPayslipModel.RegularHolidayPay
    Public ReadOnly Property RegularHolidayOTHours As Decimal Implements IPaystubPayslipModel.RegularHolidayOTHours
    Public ReadOnly Property RegularHolidayOTPay As Decimal Implements IPaystubPayslipModel.RegularHolidayOTPay
    Public ReadOnly Property RestDayNightDiffHours As Decimal Implements IPaystubPayslipModel.RestDayNightDiffHours
    Public ReadOnly Property RestDayNightDiffPay As Decimal Implements IPaystubPayslipModel.RestDayNightDiffPay
    Public ReadOnly Property RestDayNightDiffOTHours As Decimal Implements IPaystubPayslipModel.RestDayNightDiffOTHours
    Public ReadOnly Property RestDayNightDiffOTPay As Decimal Implements IPaystubPayslipModel.RestDayNightDiffOTPay
    Public ReadOnly Property SpecialHolidayNightDiffHours As Decimal Implements IPaystubPayslipModel.SpecialHolidayNightDiffHours
    Public ReadOnly Property SpecialHolidayNightDiffPay As Decimal Implements IPaystubPayslipModel.SpecialHolidayNightDiffPay
    Public ReadOnly Property SpecialHolidayNightDiffOTHours As Decimal Implements IPaystubPayslipModel.SpecialHolidayNightDiffOTHours
    Public ReadOnly Property SpecialHolidayNightDiffOTPay As Decimal Implements IPaystubPayslipModel.SpecialHolidayNightDiffOTPay
    Public ReadOnly Property SpecialHolidayRestDayHours As Decimal Implements IPaystubPayslipModel.SpecialHolidayRestDayHours
    Public ReadOnly Property SpecialHolidayRestDayPay As Decimal Implements IPaystubPayslipModel.SpecialHolidayRestDayPay
    Public ReadOnly Property SpecialHolidayRestDayOTHours As Decimal Implements IPaystubPayslipModel.SpecialHolidayRestDayOTHours
    Public ReadOnly Property SpecialHolidayRestDayOTPay As Decimal Implements IPaystubPayslipModel.SpecialHolidayRestDayOTPay
    Public ReadOnly Property SpecialHolidayRestDayNightDiffHours As Decimal Implements IPaystubPayslipModel.SpecialHolidayRestDayNightDiffHours
    Public ReadOnly Property SpecialHolidayRestDayNightDiffPay As Decimal Implements IPaystubPayslipModel.SpecialHolidayRestDayNightDiffPay
    Public ReadOnly Property SpecialHolidayRestDayNightDiffOTHours As Decimal Implements IPaystubPayslipModel.SpecialHolidayRestDayNightDiffOTHours
    Public ReadOnly Property SpecialHolidayRestDayNightDiffOTPay As Decimal Implements IPaystubPayslipModel.SpecialHolidayRestDayNightDiffOTPay
    Public ReadOnly Property RegularHolidayNightDiffHours As Decimal Implements IPaystubPayslipModel.RegularHolidayNightDiffHours
    Public ReadOnly Property RegularHolidayNightDiffPay As Decimal Implements IPaystubPayslipModel.RegularHolidayNightDiffPay
    Public ReadOnly Property RegularHolidayNightDiffOTHours As Decimal Implements IPaystubPayslipModel.RegularHolidayNightDiffOTHours
    Public ReadOnly Property RegularHolidayNightDiffOTPay As Decimal Implements IPaystubPayslipModel.RegularHolidayNightDiffOTPay
    Public ReadOnly Property RegularHolidayRestDayHours As Decimal Implements IPaystubPayslipModel.RegularHolidayRestDayHours
    Public ReadOnly Property RegularHolidayRestDayPay As Decimal Implements IPaystubPayslipModel.RegularHolidayRestDayPay
    Public ReadOnly Property RegularHolidayRestDayOTHours As Decimal Implements IPaystubPayslipModel.RegularHolidayRestDayOTHours
    Public ReadOnly Property RegularHolidayRestDayOTPay As Decimal Implements IPaystubPayslipModel.RegularHolidayRestDayOTPay
    Public ReadOnly Property RegularHolidayRestDayNightDiffHours As Decimal Implements IPaystubPayslipModel.RegularHolidayRestDayNightDiffHours
    Public ReadOnly Property RegularHolidayRestDayNightDiffPay As Decimal Implements IPaystubPayslipModel.RegularHolidayRestDayNightDiffPay
    Public ReadOnly Property RegularHolidayRestDayNightDiffOTHours As Decimal Implements IPaystubPayslipModel.RegularHolidayRestDayNightDiffOTHours
    Public ReadOnly Property RegularHolidayRestDayNightDiffOTPay As Decimal Implements IPaystubPayslipModel.RegularHolidayRestDayNightDiffOTPay

#End Region

End Class