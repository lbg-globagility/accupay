Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection

Public Class HRISForm

    Public listHRISForm As New List(Of String)

    Private curr_sys_owner_name As String = ""

    Private if_sysowner_is_benchmark As Boolean

    Private ReadOnly _policyHelper As PolicyHelper

    Dim _systemOwnerService As SystemOwnerService

    Private ReadOnly _productRepository As ProductRepository

    Private ReadOnly _userRepository As UserRepository

    Sub New(policyHelper As PolicyHelper,
            systemOwnerService As SystemOwnerService,
            productRepository As ProductRepository,
            userRepository As UserRepository)

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

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of NewDivisionPositionForm)()

            ChangeForm(form, "Division")

            previousForm = form

        End Using
    End Sub

    Private Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs) Handles CheckListToolStripMenuItem.Click
        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpempchklist)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Personal Profile")
            form.tbpempchklist.Focus()
        End Using
    End Sub

    Private Sub PersonalinfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PersonalinfoToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpEmployee)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Personal Profile")
            form.tbpEmployee.Focus()
        End Using
    End Sub

    Private Sub EmpSalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmpSalToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpNewSalary)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Salary")
            form.tbpNewSalary.Focus()
        End Using
    End Sub

    Private Sub AwardsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AwardsToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpAwards)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Award")
            form.tbpAwards.Focus()
        End Using
    End Sub

    Private Sub CertificatesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CertificatesToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpCertifications)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Certification")
            form.tbpCertifications.Focus()
        End Using
    End Sub

    Private Sub DisciplinaryActionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisciplinaryActionToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpDiscipAct)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Disciplinary Action")
            form.tbpDiscipAct.Focus()
        End Using
    End Sub

    Private Sub EducBGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EducBGToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpEducBG)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Educational Background")
            form.tbpEducBG.Focus()
        End Using
    End Sub

    Private Sub PrevEmplyrToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrevEmplyrToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpPrevEmp)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Previous Employer")
            form.tbpPrevEmp.Focus()
        End Using
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles PromotionToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpPromotion)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Promotion")
            form.tbpPromotion.Focus()
        End Using
    End Sub

    Private Sub ToolStripMenuItem10_Click(sender As Object, e As EventArgs) Handles BonusToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpBonus)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Bonus")
            form.tbpBonus.Focus()
        End Using

    End Sub

    Private Sub AttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AttachmentToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeForm)()

            Dim index = form.tabctrlemp.TabPages.IndexOf(form.tbpAttachment)

            form.tabctrlemp.SelectedIndex = index
            form.tabIndx = index
            ChangeForm(form, "Employee Attachment")
            form.tbpAttachment.Focus()
        End Using
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

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of JobLevelForm)()

            ChangeForm(form, "Position")
            previousForm = form
        End Using
    End Sub

    Private Sub PointsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PointsToolStripMenuItem.Click
        ChangeForm(JobPointsForm, "Position")
        previousForm = JobPointsForm
    End Sub

    Private Sub EmployeeExperimentalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeeExperimentalToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of NewEmployeeForm)()

            ChangeForm(form, "Position")
            previousForm = form
        End Using
    End Sub

    Private Sub DeductionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeductionsToolStripMenuItem.Click

        Dim form As New AdjustmentForm(_productRepository, AdjustmentType.Deduction)
        form.ShowDialog()

    End Sub

    Private Sub OtherIncomeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OtherIncomeToolStripMenuItem.Click

        Dim form As New AdjustmentForm(_productRepository, AdjustmentType.OtherIncome)
        form.ShowDialog()

    End Sub

End Class