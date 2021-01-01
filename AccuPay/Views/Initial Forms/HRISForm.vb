Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Repositories
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class HRISForm

    Public listHRISForm As New List(Of String)

    Private if_sysowner_is_benchmark As Boolean

    Private ReadOnly _policyHelper As IPolicyHelper

    Private ReadOnly _systemOwnerService As SystemOwnerService

    Private ReadOnly _roleRepository As RoleRepository

    Private ReadOnly _userRepository As AspNetUserRepository

    Sub New()

        InitializeComponent()

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _roleRepository = MainServiceProvider.GetRequiredService(Of RoleRepository)

        _userRepository = MainServiceProvider.GetRequiredService(Of AspNetUserRepository)

        if_sysowner_is_benchmark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark
    End Sub

    Private Async Sub HRISForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        OffSetToolStripMenuItem.Visible = _policyHelper.UseOffset

        BonusToolStripMenuItem.Visible = _policyHelper.UseBonus

        If Not _policyHelper.UseJobLevel Then
            JobLevelToolStripMenuItem.Visible = False
            JobCategoryToolStripMenuItem.Visible = False
            PointsToolStripMenuItem.Visible = False
        End If

        If Not _policyHelper.UseUserLevel Then

            Await CheckRolePermissions()
        End If

        PrepareFormForUserLevelAuthorizations()

        PrepareFormForBenchmark()

        If Not Debugger.IsAttached Then
            EmployeeExperimentalToolStripMenuItem.Visible = False
        End If

    End Sub

    Private Async Function CheckRolePermissions() As Task
        USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)

        Dim employeePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.EMPLOYEE).FirstOrDefault()
        Dim salaryPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.SALARY).FirstOrDefault()
        Dim divisionPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.DIVISION).FirstOrDefault()
        Dim positionPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.POSITION).FirstOrDefault()

        EmployeeToolStripMenuItem.Visible = If(employeePermission?.Read, False) OrElse If(salaryPermission?.Read, False)

        PersonalInfoToolStripMenuItem.Visible = If(employeePermission?.Read, False)

        Dim employeeUpdatable = If(employeePermission?.Read, False) AndAlso If(employeePermission?.Update, False)

        CheckListToolStripMenuItem.Visible = employeeUpdatable
        AwardsToolStripMenuItem.Visible = employeeUpdatable
        CertificatesToolStripMenuItem.Visible = employeeUpdatable
        EducBGToolStripMenuItem.Visible = employeeUpdatable
        PrevEmplyrToolStripMenuItem.Visible = employeeUpdatable
        DisciplinaryActionToolStripMenuItem.Visible = employeeUpdatable
        AttachmentToolStripMenuItem.Visible = employeeUpdatable

        'Bonus, Job, Points, Offset only overrides the visibility if Read is False
        'since they are already checked by other policies above
        If Not employeeUpdatable Then
            OffSetToolStripMenuItem.Visible = False
        End If

        If Not employeeUpdatable Then
            BonusToolStripMenuItem.Visible = False
        End If

        If positionPermission Is Nothing OrElse positionPermission.Read = False Then
            JobLevelToolStripMenuItem.Visible = False
            JobCategoryToolStripMenuItem.Visible = False
            PointsToolStripMenuItem.Visible = False
        End If

        EmpSalToolStripMenuItem.Visible = If(salaryPermission?.Read, False)

        DivisionToolStripMenuItem.Visible = If(divisionPermission?.Read, False) OrElse If(positionPermission?.Read, False)
    End Function

    Private Async Sub PrepareFormForUserLevelAuthorizations()
        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
            Return
        End If

        If Not _policyHelper.UseUserLevel Then Return

        If user.UserLevel = UserLevel.Four OrElse user.UserLevel = UserLevel.Five Then

            DivisionToolStripMenuItem.Visible = False

            CheckListToolStripMenuItem.Visible = False
            AwardsToolStripMenuItem.Visible = False
            CertificatesToolStripMenuItem.Visible = False
            EducBGToolStripMenuItem.Visible = False
            PrevEmplyrToolStripMenuItem.Visible = False
            DisciplinaryActionToolStripMenuItem.Visible = False
            EmpSalToolStripMenuItem.Visible = False
            BonusToolStripMenuItem.Visible = False
            AttachmentToolStripMenuItem.Visible = False
            OffSetToolStripMenuItem.Visible = False

        End If

    End Sub

    Private Sub PrepareFormForBenchmark()

        If if_sysowner_is_benchmark Then
            CheckListToolStripMenuItem.Visible = False
            AwardsToolStripMenuItem.Visible = False
            CertificatesToolStripMenuItem.Visible = False
            EducBGToolStripMenuItem.Visible = False
            PrevEmplyrToolStripMenuItem.Visible = False
            DisciplinaryActionToolStripMenuItem.Visible = False
            BonusToolStripMenuItem.Visible = False
            AttachmentToolStripMenuItem.Visible = False
            OffSetToolStripMenuItem.Visible = False
            JobLevelToolStripMenuItem.Visible = False
            EmployeeExperimentalToolStripMenuItem.Visible = False

            DeductionsToolStripMenuItem.Visible = True
            OtherIncomeToolStripMenuItem.Visible = True
        End If

    End Sub

    Private Async Function ChangeForm(ByVal passedForm As Form, Optional permissionNames As String() = Nothing) As Task

        Dim isAllowed As Boolean = False

        If permissionNames IsNot Nothing Then

            For Each permissionName In permissionNames

                If Await PermissionHelper.DoesAllowReadAsync(permissionName, policyHelper:=_policyHelper) Then

                    isAllowed = True
                End If

            Next
        End If

        If Not isAllowed Then
            MessageBoxHelper.DefaultUnauthorizedFormMessage()
            Return
        End If

        ShowForm(passedForm)
    End Function

    Private Sub ShowForm(passedForm As Form)
        Try
            Application.DoEvents()
            Dim FName As String = passedForm.Name
            passedForm.TopLevel = False
            passedForm.KeyPreview = True
            passedForm.Enabled = True
            If listHRISForm.Contains(FName) Then
                passedForm.Show()
                passedForm.BringToFront()
                passedForm.Focus()
            Else
                Me.PanelHRIS.Controls.Add(passedForm)
                listHRISForm.Add(passedForm.Name)

                passedForm.Show()
                passedForm.BringToFront()
                passedForm.Focus()
                passedForm.Dock = DockStyle.Fill
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            Dim listOfForms = PanelHRIS.Controls.Cast(Of Form).Where(Function(i) i.Name <> passedForm.Name)
            For Each pb As Form In listOfForms
                pb.Enabled = False
            Next
        End Try
    End Sub

    Private Async Sub DivisionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DivisionToolStripMenuItem.Click
        Await ChangeForm(NewDivisionPositionForm, {PermissionConstant.DIVISION, PermissionConstant.POSITION})

        previousForm = NewDivisionPositionForm
    End Sub

    Private Async Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs) Handles CheckListToolStripMenuItem.Click

        Dim index = EmployeeForm.GetCheckListTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.EMPLOYEE})
        EmployeeForm.tbpempchklist.Focus()
    End Sub

    Private Async Sub PersonalinfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PersonalInfoToolStripMenuItem.Click

        Dim index = EmployeeForm.GetEmployeeProfileTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        Await EmployeeForm.OpenEmployeeTab()
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.EMPLOYEE})
        EmployeeForm.tbpEmployee.Focus()
    End Sub

    Private Async Sub EmpSalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmpSalToolStripMenuItem.Click

        Dim index = EmployeeForm.GetSalaryTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.SALARY})
        EmployeeForm.tbpSalary.Focus()
    End Sub

    Private Async Sub AwardsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AwardsToolStripMenuItem.Click

        Dim index = EmployeeForm.GetAwardsTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.EMPLOYEE})
        EmployeeForm.tbpAwards.Focus()
    End Sub

    Private Async Sub CertificatesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CertificatesToolStripMenuItem.Click

        Dim index = EmployeeForm.GetCertificationTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.EMPLOYEE})
        EmployeeForm.tbpCertifications.Focus()
    End Sub

    Private Async Sub DisciplinaryActionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisciplinaryActionToolStripMenuItem.Click

        Dim index = EmployeeForm.GetDisciplinaryActionTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.EMPLOYEE})
        EmployeeForm.tbpDiscipAct.Focus()
    End Sub

    Private Async Sub EducBGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EducBGToolStripMenuItem.Click

        Dim index = EmployeeForm.GetEducationalBackgroundTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.EMPLOYEE})
        EmployeeForm.tbpEducBG.Focus()
    End Sub

    Private Async Sub PrevEmplyrToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrevEmplyrToolStripMenuItem.Click

        Dim index = EmployeeForm.GetPreviousEmployerTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.EMPLOYEE})
        EmployeeForm.tbpPrevEmp.Focus()
    End Sub

    Private Async Sub ToolStripMenuItem10_Click(sender As Object, e As EventArgs) Handles BonusToolStripMenuItem.Click

        Dim index = EmployeeForm.GetBonusTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.EMPLOYEE})
        EmployeeForm.tbpBonus.Focus()
    End Sub

    Private Async Sub AttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AttachmentToolStripMenuItem.Click

        Dim index = EmployeeForm.tabctrlemp.TabPages.IndexOf(EmployeeForm.tbpAttachment)

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        Await ChangeForm(EmployeeForm, {PermissionConstant.EMPLOYEE})
        EmployeeForm.tbpAttachment.Focus()
    End Sub

    Private Async Sub OffSetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OffSetToolStripMenuItem.Click

        Await ChangeForm(OffSetting, {PermissionConstant.EMPLOYEE})
        previousForm = OffSetting

    End Sub

    Private Async Sub JobCategoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JobCategoryToolStripMenuItem.Click
        Await ChangeForm(JobLevelForm, {PermissionConstant.POSITION})
        previousForm = JobLevelForm
    End Sub

    Private Async Sub PointsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PointsToolStripMenuItem.Click
        Await ChangeForm(JobPointsForm, {PermissionConstant.POSITION})
        previousForm = JobPointsForm
    End Sub

    Private Async Sub EmployeeExperimentalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeeExperimentalToolStripMenuItem.Click
        Await ChangeForm(NewEmployeeForm, {PermissionConstant.EMPLOYEE})
        previousForm = NewEmployeeForm
    End Sub

    Private Sub DeductionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeductionsToolStripMenuItem.Click

        Dim form As New AdjustmentForm(AdjustmentType.Deduction)
        form.ShowDialog()

    End Sub

    Private Sub OtherIncomeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OtherIncomeToolStripMenuItem.Click

        Dim form As New AdjustmentForm(AdjustmentType.OtherIncome)
        form.ShowDialog()

    End Sub

    Private Sub HRISForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        For Each objctrl As Control In PanelHRIS.Controls
            If TypeOf objctrl Is Form Then
                DirectCast(objctrl, Form).Close()

            End If
        Next

    End Sub

    Private Sub HRISForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        'Label1.Text = Me.Width & " PanelHRIS " & PanelHRIS.Width
    End Sub

    Private Sub PanelHRIS_ControlRemoved(sender As Object, e As ControlEventArgs) Handles PanelHRIS.ControlRemoved
        Dim listOfForms = PanelHRIS.Controls.Cast(Of Form)()
        For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
            'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
            pb.Enabled = True
            Exit For
        Next
    End Sub

End Class
