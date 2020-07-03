Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class HRISForm

    Public listHRISForm As New List(Of String)

    Private curr_sys_owner_name As String = ""

    Private if_sysowner_is_benchmark As Boolean

    Private _policyHelper As PolicyHelper

    Dim _systemOwnerService As SystemOwnerService

    Private _userRepository As UserRepository

    Sub New()

        InitializeComponent()

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        _policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _userRepository = MainServiceProvider.GetRequiredService(Of UserRepository)

        curr_sys_owner_name = _systemOwnerService.GetCurrentSystemOwner()
        if_sysowner_is_benchmark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark

        PrepareFormForBenchmark()
    End Sub

    Private Sub PrepareFormForBenchmark()

        If if_sysowner_is_benchmark Then
            CheckListToolStripMenuItem.Visible = False
            AwardsToolStripMenuItem.Visible = False
            CertificatesToolStripMenuItem.Visible = False
            EducBGToolStripMenuItem.Visible = False
            PrevEmplyrToolStripMenuItem.Visible = False
            PromotionToolStripMenuItem.Visible = False
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

    Private Sub ChangeForm(ByVal Formname As Form, Optional ViewName As String = Nothing)

        reloadViewPrivilege()

        Dim view_ID = ValNoComma(VIEW_privilege(ViewName, orgztnID))

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If _policyHelper.UseUserLevel OrElse formuserprivilege.Count > 0 Then

            For Each drow In formuserprivilege
                'If drow("ReadOnly").ToString = "Y" Then
                If drow("AllowedToAccess").ToString = "N" Then

                    'ChangeForm(Formname)
                    'previousForm = Formname

                    'Exit For
                    Exit Sub
                Else
                    If drow("Creates").ToString = "Y" _
                        Or drow("Updates").ToString = "Y" _
                        Or drow("Deleting").ToString = "Y" _
                        Or drow("ReadOnly").ToString = "Y" Then
                        'And drow("Updates").ToString = "Y" Then

                        'ChangeForm(Formname)
                        'previousForm = Formname
                        Exit For
                    Else
                        Exit Sub
                    End If

                End If

            Next
        Else
            Exit Sub
        End If

        Try
            Application.DoEvents()
            Dim FName As String = Formname.Name
            Formname.TopLevel = False
            Formname.KeyPreview = True
            Formname.Enabled = True
            If listHRISForm.Contains(FName) Then
                Formname.Show()
                Formname.BringToFront()
                Formname.Focus()
            Else
                Me.PanelHRIS.Controls.Add(Formname)
                listHRISForm.Add(Formname.Name)

                Formname.Show()
                Formname.BringToFront()
                Formname.Focus()
                'Formname.Location = New Point((PanelHRIS.Width / 2) - (Formname.Width / 2), (PanelHRIS.Height / 2) - (Formname.Height / 2))
                'Formname.Anchor = AnchorStyles.Top And AnchorStyles.Bottom And AnchorStyles.Right And AnchorStyles.Left
                'Formname.WindowState = FormWindowState.Maximized
                Formname.Dock = DockStyle.Fill
                'PerformLayout
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            Dim listOfForms = PanelHRIS.Controls.Cast(Of Form).Where(Function(i) i.Name <> Formname.Name)
            For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
                'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
                pb.Enabled = False
            Next
        End Try
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DivisionToolStripMenuItem.Click
        ChangeForm(NewDivisionPositionForm, "Division")

        previousForm = NewDivisionPositionForm
    End Sub

    Private Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs) Handles CheckListToolStripMenuItem.Click

        Dim index = EmployeeForm.GetCheckListTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        'TODO: this should be 'Employee Check List' but there is a bug on user priviledge.
        'this is needed to be fixed!
        ChangeForm(EmployeeForm, "Employee Personal Profile")
        EmployeeForm.tbpempchklist.Focus()
        'Employee.tbpempchklist_Enter(sender, e)
    End Sub

    Private Sub PersonalinfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PersonalinfoToolStripMenuItem.Click

        Dim index = EmployeeForm.GetEmployeeProfileTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tbpEmployee_Enter(sender, e)
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Personal Profile")
        EmployeeForm.tbpEmployee.Focus()
    End Sub

    Private Sub EmpSalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmpSalToolStripMenuItem.Click

        Dim index = EmployeeForm.GetSalaryTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Salary")
        EmployeeForm.tbpNewSalary.Focus()
    End Sub

    Private Sub AwardsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AwardsToolStripMenuItem.Click

        Dim index = EmployeeForm.GetAwardsTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Award")
        EmployeeForm.tbpAwards.Focus()
    End Sub

    Private Sub CertificatesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CertificatesToolStripMenuItem.Click

        Dim index = EmployeeForm.GetCertificationTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Certification")
        EmployeeForm.tbpCertifications.Focus()
    End Sub

    Private Sub DisciplinaryActionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisciplinaryActionToolStripMenuItem.Click

        Dim index = EmployeeForm.GetDisciplinaryActionTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Disciplinary Action")
        EmployeeForm.tbpDiscipAct.Focus()
    End Sub

    Private Sub EducBGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EducBGToolStripMenuItem.Click

        Dim index = EmployeeForm.GetEducationalBackgroundTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Educational Background")
        EmployeeForm.tbpEducBG.Focus()
    End Sub

    Private Sub PrevEmplyrToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrevEmplyrToolStripMenuItem.Click

        Dim index = EmployeeForm.GetPreviousEmployerTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Previous Employer")
        EmployeeForm.tbpPrevEmp.Focus()
    End Sub

    Private Sub ToolStripMenuItem10_Click(sender As Object, e As EventArgs) Handles BonusToolStripMenuItem.Click

        Dim index = EmployeeForm.GetBonusTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Bonus")
        EmployeeForm.tbpBonus.Focus()
    End Sub

    Private Sub AttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AttachmentToolStripMenuItem.Click

        Dim index = EmployeeForm.tabctrlemp.TabPages.IndexOf(EmployeeForm.tbpAttachment)

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Attachment")
        EmployeeForm.tbpAttachment.Focus()
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

    Private Sub HRISForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _systemOwnerService.GetCurrentSystemOwner() <> SystemOwnerService.Hyundai Then
            JobLevelToolStripMenuItem.Visible = False
            JobCategoryToolStripMenuItem.Visible = False
            PointsToolStripMenuItem.Visible = False
        End If

        If Not Debugger.IsAttached Then
            EmployeeExperimentalToolStripMenuItem.Visible = False
        End If

        PrepareFormForUserLevelAuthorizations()

    End Sub

    Private Async Sub PrepareFormForUserLevelAuthorizations()
        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
        End If

        If _policyHelper.UseUserLevel = False Then Return

        If user.UserLevel = UserLevel.Four OrElse user.UserLevel = UserLevel.Five Then

            DivisionToolStripMenuItem.Visible = False

            CheckListToolStripMenuItem.Visible = False
            AwardsToolStripMenuItem.Visible = False
            CertificatesToolStripMenuItem.Visible = False
            EducBGToolStripMenuItem.Visible = False
            PrevEmplyrToolStripMenuItem.Visible = False
            PromotionToolStripMenuItem.Visible = False
            DisciplinaryActionToolStripMenuItem.Visible = False
            EmpSalToolStripMenuItem.Visible = False
            BonusToolStripMenuItem.Visible = False
            AttachmentToolStripMenuItem.Visible = False
            OffSetToolStripMenuItem.Visible = False

        End If

    End Sub

    Sub reloadViewPrivilege()

        Dim hasPositionViewUpdate = EXECQUER("SELECT EXISTS(SELECT" &
                                             " RowID" &
                                             " FROM position_view" &
                                             " WHERE OrganizationID='" & orgztnID & "'" &
                                             " AND (DATE_FORMAT(Created,@@date_format) = CURDATE()" &
                                             " OR DATE_FORMAT(LastUpd,@@date_format) = CURDATE()));")

        If hasPositionViewUpdate = "1" Then

            position_view_table = retAsDatTbl("SELECT *" &
                                              " FROM position_view" &
                                              " WHERE PositionID=(SELECT PositionID FROM user WHERE RowID=" & z_User & ")" &
                                              " AND OrganizationID='" & orgztnID & "';")

        End If

    End Sub

    Private Sub OffSetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OffSetToolStripMenuItem.Click

        'Dim n_OffSetting As New OffSetting

        ChangeForm(OffSetting, "Employee Leave")

        previousForm = OffSetting

    End Sub

    Private Sub PanelHRIS_ControlRemoved(sender As Object, e As ControlEventArgs) Handles PanelHRIS.ControlRemoved
        Dim listOfForms = PanelHRIS.Controls.Cast(Of Form)()
        For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
            'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
            pb.Enabled = True
            Exit For
        Next
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        OffSetToolStripMenuItem.Visible =
            (curr_sys_owner_name = SystemOwnerService.Cinema2000)

        MyBase.OnLoad(e)

    End Sub

    Private Sub JobCategoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JobCategoryToolStripMenuItem.Click
        ChangeForm(JobLevelForm, "Position")
        previousForm = JobLevelForm
    End Sub

    Private Sub PointsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PointsToolStripMenuItem.Click
        ChangeForm(JobPointsForm, "Position")
        previousForm = JobPointsForm
    End Sub

    Private Sub EmployeeExperimentalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeeExperimentalToolStripMenuItem.Click
        ChangeForm(NewEmployeeForm, "Position")
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

End Class