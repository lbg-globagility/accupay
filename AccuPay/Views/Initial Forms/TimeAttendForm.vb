Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class TimeAttendForm

    Public listTimeAttendForm As New List(Of String)

    Private ReadOnly _policyHelper As IPolicyHelper

    Private ReadOnly _roleRepository As IRoleRepository

    Private ReadOnly _userRepository As IAspNetUserRepository
    Private ReadOnly _systemOwnerService As ISystemOwnerService

    Sub New()

        InitializeComponent()

        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)

        _userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of ISystemOwnerService)

    End Sub

    Private Async Sub TimeAttendForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MassOvertimeToolStripMenuItem.Visible = _policyHelper.UseMassOvertime
        ResetLeaveCreditsToolStripMenuItem.Visible = (Await _systemOwnerService.GetCurrentSystemOwnerAsync()) = SystemOwner.Cinema2000

        If Not _policyHelper.UseUserLevel Then

            Await CheckRolePermissions()
        End If

        PrepareFormForUserLevelAuthorizations()
    End Sub

    Private Async Function CheckRolePermissions() As Task
        USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)

        Dim leavePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.LEAVE).FirstOrDefault()
        Dim officialBusinessPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.OFFICIALBUSINESS).FirstOrDefault()
        Dim overtimePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.OVERTIME).FirstOrDefault()
        Dim shiftPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.SHIFT).FirstOrDefault()
        Dim timeLogPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.TIMELOG).FirstOrDefault()
        Dim timeEntryPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.TIMEENTRY).FirstOrDefault()
        Dim tripTicketPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.TRIPTICKET).FirstOrDefault()

        LeaveToolStripMenuItem.Visible = If(leavePermission?.Read, False)
        OfficialBusinessToolStripMenuItem.Visible = If(officialBusinessPermission?.Read, False)
        OvertimeToolStripMenuItem.Visible = If(overtimePermission?.Read, False)
        TimeLogsToolStripMenuItem.Visible = If(timeLogPermission?.Read, False)
        SummaryToolStripMenuItem.Visible = If(timeEntryPermission?.Read, False)
        TripTicketsToolStripMenuItem.Visible = If(tripTicketPermission?.Read, False)

        'Shift and overtime only overrides the visibility if Read is False
        'since they are already checked by other policies above
        If shiftPermission Is Nothing OrElse shiftPermission.Read = False Then

            ShiftToolStripMenuItem.Visible = False
        End If

        If overtimePermission Is Nothing OrElse overtimePermission.Read = False Then
            MassOvertimeToolStripMenuItem.Visible = False
        End If

    End Function

    Private Async Sub PrepareFormForUserLevelAuthorizations()

        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
            Return
        End If

        If Not _policyHelper.UseUserLevel Then Return

        If user.UserLevel = UserLevel.Four OrElse user.UserLevel = UserLevel.Five Then

            LeaveToolStripMenuItem.Visible = False
            OfficialBusinessToolStripMenuItem.Visible = False

            If user.UserLevel = UserLevel.Five Then

                OvertimeToolStripMenuItem.Visible = False

            End If

        End If

    End Sub

    Private Async Function ChangeForm(ByVal passedForm As Form, Optional permissionName As String = Nothing) As Task

        If permissionName IsNot Nothing Then

            If Await PermissionHelper.DoesAllowReadAsync(permissionName, policyHelper:=_policyHelper) = False Then
                MessageBoxHelper.DefaultUnauthorizedFormMessage()
                Return
            End If

        End If

        ShowForm(passedForm)
    End Function

    Private Sub ShowForm(passedForm As Form)
        Try
            Application.DoEvents()
            Dim FName As String = passedForm.Name
            passedForm.TopLevel = False
            passedForm.Enabled = True
            If listTimeAttendForm.Contains(FName) Then
            Else
                PanelTimeAttend.Controls.Add(passedForm)
                listTimeAttendForm.Add(passedForm.Name)
                passedForm.Refresh()
                passedForm.Dock = DockStyle.Fill
            End If
            passedForm.Show()
            passedForm.BringToFront()
            passedForm.Focus()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            Dim listOfForms = PanelTimeAttend.Controls.Cast(Of Form).Where(Function(i) i.Name <> passedForm.Name)
            For Each pb As Form In listOfForms
                pb.Enabled = False
            Next
        End Try

    End Sub

    Private Sub TimeAttendForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        For Each objctrl As Control In PanelTimeAttend.Controls
            If TypeOf objctrl Is Form Then
                DirectCast(objctrl, Form).Close()

            End If
        Next

    End Sub

    Private Sub PanelTimeAttend_ControlRemoved(sender As Object, e As ControlEventArgs) Handles PanelTimeAttend.ControlRemoved
        Dim listOfForms = PanelTimeAttend.Controls.Cast(Of Form)()
        For Each pb As Form In listOfForms
            pb.Enabled = True
            Exit For
        Next
    End Sub

    Private Async Sub SummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SummaryToolStripMenuItem.Click
        Await ChangeForm(TimeEntrySummaryForm, PermissionConstant.TIMEENTRY)
        previousForm = TimeEntrySummaryForm
    End Sub

    Private Async Sub MassOvertimeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MassOvertimeToolStripMenuItem.Click

        Dim helper = Await PermissionHelper.GetRoleAsync(PermissionConstant.OVERTIME, policyHelper:=_policyHelper)
        If helper.Success = False OrElse
            helper.RolePermission.Create = False OrElse
            helper.RolePermission.Update = False OrElse
            helper.RolePermission.Delete = False Then
            MessageBoxHelper.Warning("You need Read, Create, Update and Delete permissions to access this form.", "Unauthorized Action")
            Return
        End If

        Await ChangeForm(MassOvertimeForm, PermissionConstant.OVERTIME)
        previousForm = MassOvertimeForm
    End Sub

    Private Async Sub ShiftToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShiftToolStripMenuItem.Click
        Await ChangeForm(ShiftForm, PermissionConstant.SHIFT)
        previousForm = ShiftForm
    End Sub

    Private Async Sub TimeLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TimeLogsToolStripMenuItem.Click
        Await ChangeForm(TimeLogsForm, PermissionConstant.TIMELOG)
        previousForm = TimeLogsForm
    End Sub

    Private Async Sub LeaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LeaveToolStripMenuItem.Click
        Await ChangeForm(EmployeeLeavesForm, PermissionConstant.LEAVE)
        previousForm = EmployeeLeavesForm
    End Sub

    Private Async Sub OfficialBusinessToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OfficialBusinessToolStripMenuItem.Click
        Await ChangeForm(OfficialBusinessForm, PermissionConstant.OFFICIALBUSINESS)
        previousForm = OfficialBusinessForm
    End Sub

    Private Async Sub OvertimeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OvertimeToolStripMenuItem.Click
        Await ChangeForm(EmployeeOvertimeForm, PermissionConstant.OVERTIME)
        previousForm = EmployeeOvertimeForm
    End Sub

    Private Async Sub TripTicketsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TripTicketsToolStripMenuItem.Click
        Await ChangeForm(TripTicketForm, PermissionConstant.TRIPTICKET)
        previousForm = TripTicketForm
    End Sub

    Private Async Sub ResetLeaveCreditsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetLeaveCreditsToolStripMenuItem.Click
        Await ChangeForm(ResetLeaveCreditsForm, PermissionConstant.RESET_LEAVE_CREDIT)
        previousForm = ResetLeaveCreditsForm
    End Sub
End Class
