Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeetimeentry")>
    Public Class TimeEntry

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property EmployeeID As Integer?

        Public Property EmployeeShiftID As Integer?

        Public Property EmployeeSalaryID As Integer?

        Public Property PayRateID As Integer?

        <Column("Date")>
        Public Property [Date] As Date

        <Column("RegularHoursWorked")>
        Public Property RegularHours As Decimal

        <Column("RegularHoursAmount")>
        Public Property RegularPay As Decimal

        <Column("OvertimeHoursWorked")>
        Public Property OvertimeHours As Decimal

        <Column("OvertimeHoursAmount")>
        Public Property OvertimePay As Decimal

        <Column("NightDifferentialHours")>
        Public Property NightDiffHours As Decimal

        <Column("NightDiffHoursAmount")>
        Public Property NightDiffPay As Decimal

        <Column("NightDifferentialOTHours")>
        Public Property NightDiffOTHours As Decimal

        <Column("NightDiffOTHoursAmount")>
        Public Property NightDiffOTPay As Decimal

        Public Property RestDayHours As Decimal

        <Column("RestDayAmount")>
        Public Property RestDayPay As Decimal

        Public Property RestDayOTHours As Decimal

        Public Property RestDayOTPay As Decimal

        Public Property VacationLeaveHours As Decimal

        Public Property SickLeaveHours As Decimal

        Public Property MaternityLeaveHours As Decimal

        Public Property OtherLeaveHours As Decimal

        <Column("Leavepayment")>
        Public Property LeavePay As Decimal

        Public Property SpecialHolidayHours As Decimal

        Public Property SpecialHolidayPay As Decimal

        Public Property SpecialHolidayOTHours As Decimal

        Public Property SpecialHolidayOTPay As Decimal

        Public Property RegularHolidayHours As Decimal

        Public Property RegularHolidayPay As Decimal

        <NotMapped>
        Public Property BasicRegularHolidayPay As Decimal

        Public Property RegularHolidayOTHours As Decimal

        Public Property RegularHolidayOTPay As Decimal

        <Column("HolidayPayAmount")>
        Public Property HolidayPay As Decimal

        <Column("HoursLate")>
        Public Property LateHours As Decimal

        <Column("HoursLateAmount")>
        Public Property LateDeduction As Decimal

        Public Property UndertimeHours As Decimal

        <Column("UndertimeHoursAmount")>
        Public Property UndertimeDeduction As Decimal

        Public Property AbsentHours As Decimal

        <Column("Absent")>
        Public Property AbsentDeduction As Decimal

        <NotMapped>
        Public Property BasicHours As Decimal

        <Column("BasicDayPay")>
        Public Property BasicDayPay As Decimal

        <Column("TotalHoursWorked")>
        Public Property TotalHours As Decimal

        <Column("TotalDayPay")>
        Public Property TotalDayPay As Decimal

        <NotMapped>
        Public Property DutyStart As Date

        <NotMapped>
        Public Property DutyEnd As Date

        Public Property WorkHours As Decimal

        Public Property ShiftHours As Decimal

        Public Property IsRestDay As Boolean

        Public Property HasShift As Boolean

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

        Public Sub New()
        End Sub

        Public Sub SetLeaveHours(type As String, leaveHours As Decimal)
            Select Case type
                Case LeaveType.Sick
                    SickLeaveHours = leaveHours
                Case LeaveType.Vacation
                    VacationLeaveHours = leaveHours
                Case LeaveType.Maternity
                    MaternityLeaveHours = leaveHours
                Case LeaveType.Others
                    OtherLeaveHours = leaveHours
            End Select
        End Sub

        Public ReadOnly Property TotalLeaveHours As Decimal
            Get
                Return VacationLeaveHours + SickLeaveHours + MaternityLeaveHours + OtherLeaveHours
            End Get
        End Property

        Public Sub ComputeTotalHours()
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

        Public Sub ComputeTotalPay()
            TotalDayPay = GetTotalDayPay()
        End Sub

        Public Function GetTotalDayPay() As Decimal
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

        Public Sub Reset()

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