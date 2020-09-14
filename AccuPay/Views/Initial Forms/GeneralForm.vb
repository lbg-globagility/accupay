Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class GeneralForm

    Public listGeneralForm As New List(Of String)

    Private ReadOnly _systemOwnerService As SystemOwnerService

    Private ReadOnly _policyHelper As PolicyHelper

    Private ReadOnly _roleRepository As RoleRepository

    Private ReadOnly _userRepository As UserRepository

    Sub New()

        InitializeComponent()

        _policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        _roleRepository = MainServiceProvider.GetRequiredService(Of RoleRepository)

        _userRepository = MainServiceProvider.GetRequiredService(Of UserRepository)

    End Sub

    Private Async Sub GeneralForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
        End If

        CheckPolicyPermissions()

        If _policyHelper.UseUserLevel = True Then
            CheckUserLevelPermissions(user)
        Else

            Await CheckRolePermissions()
        End If

    End Sub

    Private Sub CheckPolicyPermissions()
        If _policyHelper.PayRateCalculationBasis = PayRateCalculationBasis.Branch Then

            CalendarsToolStripMenuItem.Visible = True
            PayRateToolStripMenuItem.Visible = False
        Else

            CalendarsToolStripMenuItem.Visible = False
            PayRateToolStripMenuItem.Visible = True

        End If

        If _policyHelper.ShowBranch = False Then
            BranchToolStripMenuItem.Visible = False
        End If

        If _policyHelper.UseShiftSchedule = True Then
            DutyShiftingToolStripMenuItem.Visible = False
        End If
    End Sub

    Private Sub CheckUserLevelPermissions(user As Data.Entities.User)
        If user.UserLevel = UserLevel.Two OrElse user.UserLevel = UserLevel.Three Then

            UserToolStripMenuItem.Visible = False
            OrganizationToolStripMenuItem.Visible = False

        End If

        UserPrivilegeToolStripMenuItem.Visible = False
    End Sub

    Private Async Function CheckRolePermissions() As Task
        Await ReloadRolePermissions()

        Dim userPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.USER).FirstOrDefault()
        Dim organizationPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.ORGANIZATION).FirstOrDefault()
        Dim branchPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.BRANCH).FirstOrDefault()
        Dim rolePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.ROLE).FirstOrDefault()
        Dim shiftPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.SHIFT).FirstOrDefault()
        Dim calendarPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.CALENDAR).FirstOrDefault()

        UserToolStripMenuItem.Visible = If(userPermission?.Read, False)
        OrganizationToolStripMenuItem.Visible = If(organizationPermission?.Read, False)
        UserPrivilegeToolStripMenuItem.Visible = If(rolePermission?.Read, False)

        'Branch, Calendar and Duty Shift will override the visibility only if Read is False
        'since they are already checked by other policies above
        If branchPermission Is Nothing OrElse branchPermission.Read = False Then
            BranchToolStripMenuItem.Visible = False
        End If

        If calendarPermission Is Nothing OrElse calendarPermission.Read = False Then
            CalendarsToolStripMenuItem.Visible = False
            PayRateToolStripMenuItem.Visible = False
        End If

        If shiftPermission Is Nothing OrElse shiftPermission.Read = False Then
            DutyShiftingToolStripMenuItem.Visible = False
        End If
    End Function

    Async Function ChangeForm(passedForm As Form, Optional permissionName As String = Nothing) As Task

        If permissionName IsNot Nothing Then

            If Await PermissionHelper.AllowRead(permissionName, policyHelper:=_policyHelper) = False Then
                MessageBoxHelper.DefaultUnauthorizedFormMessage()
                Return
            End If

        End If

        ShowForm(passedForm)

    End Function

    Private Function ShowForm(passedForm As Form) As Form
        Try
            Application.DoEvents()
            Dim FName As String = passedForm.Name
            passedForm.KeyPreview = True
            passedForm.TopLevel = False
            passedForm.Enabled = True
            If listGeneralForm.Contains(FName) Then
                passedForm.Show()
                passedForm.BringToFront()
                passedForm.Focus()
            Else
                PanelGeneral.Controls.Add(passedForm)
                listGeneralForm.Add(passedForm.Name)

                passedForm.Show()
                passedForm.BringToFront()
                passedForm.Focus()
                passedForm.Dock = DockStyle.Fill
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            Dim listOfForms = PanelGeneral.Controls.Cast(Of Form).Where(Function(i) i.Name <> passedForm.Name)
            For Each pb As Form In listOfForms
                pb.Enabled = False
            Next
        End Try

        Return passedForm
    End Function

    Private Sub GeneralForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        For Each objctrl As Control In PanelGeneral.Controls
            If TypeOf objctrl Is Form Then
                DirectCast(objctrl, Form).Close()

            End If
        Next

    End Sub

    Private Async Sub UserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserToolStripMenuItem.Click

        Await ChangeForm(UsersForm2, PermissionConstant.USER)
        previousForm = UsersForm2

    End Sub

    Private Async Sub OrganizationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrganizationToolStripMenuItem.Click

        Await ChangeForm(OrganizationForm, PermissionConstant.ORGANIZATION)
        previousForm = OrganizationForm

    End Sub

    Private Async Sub SupplierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserPrivilegeToolStripMenuItem.Click

        Await ChangeForm(UserPrivilegeForm, PermissionConstant.ROLE)
        previousForm = UserPrivilegeForm

    End Sub

    Private Async Sub SSSTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SSSTableToolStripMenuItem.Click

        Await ChangeForm(SSSCntrib)
        previousForm = SSSCntrib

    End Sub

    Private Async Sub DutyShiftingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DutyShiftingToolStripMenuItem.Click

        Await ChangeForm(ShiftEntryForm, PermissionConstant.SHIFT)
        previousForm = ShiftEntryForm

    End Sub

    Private Async Sub PayRateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PayRateToolStripMenuItem.Click

        Await ChangeForm(PayRateForm, PermissionConstant.CALENDAR)
        previousForm = PayRateForm

    End Sub

    Private Async Sub CalendarsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CalendarsToolStripMenuItem.Click
        Await ChangeForm(CalendarsForm, PermissionConstant.CALENDAR)
        previousForm = CalendarsForm
    End Sub

    Private Async Sub AgencyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AgencyToolStripMenuItem.Click

        Await ChangeForm(AgencyForm)
        previousForm = AgencyForm

    End Sub

    Private Async Sub BranchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BranchToolStripMenuItem.Click

        If Await PermissionHelper.AllowRead(PermissionConstant.BRANCH, policyHelper:=_policyHelper) = False Then
            MessageBoxHelper.DefaultUnauthorizedFormMessage()
            Return
        End If

        Dim form As New AddBranchForm
        form.ShowDialog()

    End Sub

    Private Sub PanelGeneral_ControlRemoved(sender As Object, e As ControlEventArgs) Handles PanelGeneral.ControlRemoved
        Dim listOfForms = PanelGeneral.Controls.Cast(Of Form)()
        For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
            'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
            pb.Enabled = True
            Exit For
        Next
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim ownr() As String =
            Split(AgencyToolStripMenuItem.AccessibleDescription, ";")

        AgencyToolStripMenuItem.Visible =
            ownr.Contains(_systemOwnerService.GetCurrentSystemOwner())

        MyBase.OnLoad(e)

    End Sub

    Private Async Function ReloadRolePermissions() As Task

        USER_ROLE = Await _roleRepository.GetByUserAndOrganization(userId:=z_User, organizationId:=z_OrganizationID)

    End Function

End Class