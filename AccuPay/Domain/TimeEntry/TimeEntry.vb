Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Data.Helpers
Imports AccuPay.Data

Namespace Global.AccuPay.Entity

    <Table("employeetimeentry")>
    Public Class TimeEntry
        Implements ITimeEntry

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer? Implements ITimeEntry.RowID

        Public Property OrganizationID As Integer? Implements ITimeEntry.OrganizationID

        Public Property EmployeeID As Integer? Implements ITimeEntry.EmployeeID

        Public Property BranchID As Integer? Implements ITimeEntry.BranchID

        Public Property EmployeeShiftID As Integer? Implements ITimeEntry.EmployeeShiftID

        Public Property EmployeeSalaryID As Integer? Implements ITimeEntry.EmployeeSalaryID

        Public Property PayRateID As Integer? Implements ITimeEntry.PayRateID

        <Column("Date")>
        Public Property [Date] As Date Implements ITimeEntry.Date

        <Column("RegularHoursWorked")>
        Public Property RegularHours As Decimal Implements ITimeEntry.RegularHours

        <Column("RegularHoursAmount")>
        Public Property RegularPay As Decimal Implements ITimeEntry.RegularPay

        <Column("OvertimeHoursWorked")>
        Public Property OvertimeHours As Decimal Implements ITimeEntry.OvertimeHours

        <Column("OvertimeHoursAmount")>
        Public Property OvertimePay As Decimal Implements ITimeEntry.OvertimePay

        <Column("NightDifferentialHours")>
        Public Property NightDiffHours As Decimal Implements ITimeEntry.NightDiffHours

        <Column("NightDiffHoursAmount")>
        Public Property NightDiffPay As Decimal Implements ITimeEntry.NightDiffPay

        <Column("NightDifferentialOTHours")>
        Public Property NightDiffOTHours As Decimal Implements ITimeEntry.NightDiffOTHours

        <Column("NightDiffOTHoursAmount")>
        Public Property NightDiffOTPay As Decimal Implements ITimeEntry.NightDiffOTPay

        Public Property RestDayHours As Decimal Implements ITimeEntry.RestDayHours

        <Column("RestDayAmount")>
        Public Property RestDayPay As Decimal Implements ITimeEntry.RestDayPay

        Public Property RestDayOTHours As Decimal Implements ITimeEntry.RestDayOTHours

        Public Property RestDayOTPay As Decimal Implements ITimeEntry.RestDayOTPay

        Public Property VacationLeaveHours As Decimal Implements ITimeEntry.VacationLeaveHours

        Public Property SickLeaveHours As Decimal Implements ITimeEntry.SickLeaveHours

        Public Property MaternityLeaveHours As Decimal Implements ITimeEntry.MaternityLeaveHours

        Public Property OtherLeaveHours As Decimal Implements ITimeEntry.OtherLeaveHours

        <Column("Leavepayment")>
        Public Property LeavePay As Decimal Implements ITimeEntry.LeavePay

        Public Property SpecialHolidayHours As Decimal Implements ITimeEntry.SpecialHolidayHours

        Public Property SpecialHolidayPay As Decimal Implements ITimeEntry.SpecialHolidayPay

        Public Property SpecialHolidayOTHours As Decimal Implements ITimeEntry.SpecialHolidayOTHours

        Public Property SpecialHolidayOTPay As Decimal Implements ITimeEntry.SpecialHolidayOTPay

        Public Property RegularHolidayHours As Decimal Implements ITimeEntry.RegularHolidayHours

        Public Property RegularHolidayPay As Decimal Implements ITimeEntry.RegularHolidayPay

        <NotMapped>
        Public Property BasicRegularHolidayPay As Decimal Implements ITimeEntry.BasicRegularHolidayPay

        Public Property RegularHolidayOTHours As Decimal Implements ITimeEntry.RegularHolidayOTHours

        Public Property RegularHolidayOTPay As Decimal Implements ITimeEntry.RegularHolidayOTPay

        <Column("HolidayPayAmount")>
        Public Property HolidayPay As Decimal Implements ITimeEntry.HolidayPay

        <Column("HoursLate")>
        Public Property LateHours As Decimal Implements ITimeEntry.LateHours

        <Column("HoursLateAmount")>
        Public Property LateDeduction As Decimal Implements ITimeEntry.LateDeduction

        Public Property UndertimeHours As Decimal Implements ITimeEntry.UndertimeHours

        <Column("UndertimeHoursAmount")>
        Public Property UndertimeDeduction As Decimal Implements ITimeEntry.UndertimeDeduction

        Public Property AbsentHours As Decimal Implements ITimeEntry.AbsentHours

        <Column("Absent")>
        Public Property AbsentDeduction As Decimal Implements ITimeEntry.AbsentDeduction

        <NotMapped>
        Public Property BasicHours As Decimal Implements ITimeEntry.BasicHours

        <Column("BasicDayPay")>
        Public Property BasicDayPay As Decimal Implements ITimeEntry.BasicDayPay

        <Column("TotalHoursWorked")>
        Public Property TotalHours As Decimal Implements ITimeEntry.TotalHours

        <Column("TotalDayPay")>
        Public Property TotalDayPay As Decimal Implements ITimeEntry.TotalDayPay

        <NotMapped>
        Public Property DutyStart As Date Implements ITimeEntry.DutyStart

        <NotMapped>
        Public Property DutyEnd As Date Implements ITimeEntry.DutyEnd

        Public Property WorkHours As Decimal Implements ITimeEntry.WorkHours

        Public Property ShiftHours As Decimal Implements ITimeEntry.ShiftHours

        Public Property IsRestDay As Boolean Implements ITimeEntry.IsRestDay

        Public Property HasShift As Boolean Implements ITimeEntry.HasShift

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

        Public Sub New()
        End Sub

        Public Sub SetLeaveHours(type As String, leaveHours As Decimal) Implements ITimeEntry.SetLeaveHours
            Select Case type
                Case ProductConstant.SICK_LEAVE
                    SickLeaveHours = leaveHours
                Case ProductConstant.VACATION_LEAVE
                    VacationLeaveHours = leaveHours
                Case ProductConstant.MATERNITY_LEAVE
                    MaternityLeaveHours = leaveHours
                Case ProductConstant.OTHERS_LEAVE
                    OtherLeaveHours = leaveHours
            End Select
        End Sub

        Public ReadOnly Property TotalLeaveHours As Decimal Implements ITimeEntry.TotalLeaveHours
            Get
                Return VacationLeaveHours + SickLeaveHours + MaternityLeaveHours + OtherLeaveHours
            End Get
        End Property

        Public Sub ComputeTotalHours() Implements ITimeEntry.ComputeTotalHours
            TotalHours =
                RegularHours +
                OvertimeHours +
                RestDayHours +
                RestDayOTHours +
                SpecialHolidayHours +
                SpecialHolidayOTHours +
                RegularHolidayHours +
                RegularHolidayOTHours
        End Sub

        Public Sub ComputeTotalPay() Implements ITimeEntry.ComputeTotalPay
            TotalDayPay = GetTotalDayPay()
        End Sub

        Public Function GetTotalDayPay() As Decimal Implements ITimeEntry.GetTotalDayPay
            Return RegularPay +
                    OvertimePay +
                    NightDiffPay +
                    NightDiffOTPay +
                    RestDayPay +
                    RestDayOTPay +
                    SpecialHolidayPay +
                    SpecialHolidayOTPay +
                    RegularHolidayPay +
                    RegularHolidayOTPay +
                    LeavePay
        End Function

        Public Sub Reset() Implements ITimeEntry.Reset

            IsRestDay = False
            HasShift = False
            ResetHours()
            ResetPay()

        End Sub

        Private Sub ResetHours()
            BasicHours = 0
            RegularHours = 0
            OvertimeHours = 0
            NightDiffHours = 0
            NightDiffOTHours = 0
            RestDayHours = 0
            RestDayOTHours = 0
            VacationLeaveHours = 0
            SickLeaveHours = 0
            MaternityLeaveHours = 0
            OtherLeaveHours = 0
            SpecialHolidayHours = 0
            SpecialHolidayOTHours = 0
            RegularHolidayHours = 0
            RegularHolidayOTHours = 0
            LateHours = 0
            UndertimeHours = 0
            AbsentHours = 0
            TotalHours = 0

            ShiftHours = 0
            WorkHours = 0
        End Sub

        Private Sub ResetPay()
            BasicDayPay = 0
            RegularPay = 0
            OvertimePay = 0
            NightDiffPay = 0
            NightDiffOTPay = 0
            RestDayPay = 0
            RestDayOTPay = 0
            LeavePay = 0
            SpecialHolidayPay = 0
            SpecialHolidayOTPay = 0
            RegularHolidayPay = 0
            RegularHolidayOTPay = 0
            LateDeduction = 0
            UndertimeDeduction = 0
            AbsentDeduction = 0
            TotalDayPay = 0
        End Sub

    End Class

End Namespace