Imports System.IO
Imports AccuPay.Utils

Public Class HRISForm

    Public listHRISForm As New List(Of String)

    Dim sys_ownr As New SystemOwner

    Private curr_sys_owner_name As String = sys_ownr.CurrentSystemOwner

    Private Sub ChangeForm(ByVal Formname As Form, Optional ViewName As String = Nothing)

        reloadViewPrivilege()

        Dim view_ID = ValNoComma(VIEW_privilege(ViewName, orgztnID))

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count > 0 Then

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

    Sub PositionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PositionToolStripMenuItem.Click

        Dim n_UserAccessRights As New UserAccessRights(EmpPosition.ViewIdentification)

        'If n_UserAccessRights.ResultValue(AccessRightName.HasReadOnly) Then

        ChangeForm(EmpPosition, "Position")
        previousForm = EmpPosition

        'End If

        'ChangeForm(Positn)
        'previousForm = Positn

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
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Personal Profile")
        EmployeeForm.tbpEmployee.Focus()
        '    File.AppendAllText(Path.GetTempPath() & "dgvetent.txt", c.Name & "@" & c.HeaderText & "&" & c.Visible.ToString & Environment.NewLine)
        'Next

        'Employee.tbpEmployee_Enter(sender, e)
    End Sub

    Private Sub EmpSalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmpSalToolStripMenuItem.Click

        Dim index = EmployeeForm.GetSalaryTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Salary")
        EmployeeForm.tbpNewSalary.Focus()
        'Employee.tbpSalary_Enter(sender, e)
    End Sub

    Private Sub AwardsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AwardsToolStripMenuItem.Click

        Dim index = EmployeeForm.GetAwardsTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Award")
        EmployeeForm.tbpAwards.Focus()
        'Employee.tbpAwards_Enter(sender, e)
    End Sub

    Private Sub CertificatesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CertificatesToolStripMenuItem.Click

        Dim index = EmployeeForm.GetCertificationTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Certification")
        EmployeeForm.tbpCertifications.Focus()
        'Employee.tbpCertifications_Enter(sender, e)
    End Sub

    Private Sub LeaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LeaveToolStripMenuItem.Click

        Dim index = EmployeeForm.GetLeaveTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Leave")
        EmployeeForm.tbpLeave.Focus()
        'Employee.tbpLeave_Enter(sender, e)
    End Sub

    Private Sub DisciplinaryActionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisciplinaryActionToolStripMenuItem.Click

        Dim index = EmployeeForm.GetDisciplinaryActionTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Disciplinary Action")
        EmployeeForm.tbpDiscipAct.Focus()
        'Employee.tbpDiscipAct_Enter(sender, e)
    End Sub

    Private Sub EducBGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EducBGToolStripMenuItem.Click

        Dim index = EmployeeForm.GetEducationalBackgroundTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Educational Background")
        EmployeeForm.tbpEducBG.Focus()
        'Employee.tbpEducBG_Enter(sender, e)
    End Sub

    Private Sub PrevEmplyrToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrevEmplyrToolStripMenuItem.Click

        Dim index = EmployeeForm.GetPreviousEmployerTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Previous Employer")
        EmployeeForm.tbpPrevEmp.Focus()
        'Employee.tbpPrevEmp_Enter(sender, e)
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles PromotionToolStripMenuItem.Click

        Dim index = EmployeeForm.GetPromotionTabPageIndex

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Promotion")
        EmployeeForm.tbpPromotion.Focus()
        'Employee.tbpPromotion_Enter(sender, e)
    End Sub

    Private Sub ToolStripMenuItem8_Click(sender As Object, e As EventArgs) Handles OvertimeToolStripMenuItem.Click

        Dim index = EmployeeForm.tabctrlemp.TabPages.IndexOf(EmployeeForm.tbpEmpOT)

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Overtime")
        EmployeeForm.tbpEmpOT.Focus()
        'Employee.tbpEmpOT_Enter(sender, e)
    End Sub

    Private Sub ToolStripMenuItem9_Click(sender As Object, e As EventArgs) Handles OfficialBusinessToolStripMenuItem.Click

        Dim index = EmployeeForm.tabctrlemp.TabPages.IndexOf(EmployeeForm.tbpOBF)

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Official Business filing")
        EmployeeForm.tbpOBF.Focus()
        'Employee.tbpOBF_Enter(sender, e)
    End Sub

    Private Sub ToolStripMenuItem10_Click(sender As Object, e As EventArgs) Handles BonusToolStripMenuItem.Click

        Dim index = EmployeeForm.tabctrlemp.TabPages.IndexOf(EmployeeForm.tbpBonus)

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Bonus")
        EmployeeForm.tbpBonus.Focus()
        'Employee.tbpBonus_Enter(sender, e)
    End Sub

    Private Sub AttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AttachmentToolStripMenuItem.Click

        Dim index = EmployeeForm.tabctrlemp.TabPages.IndexOf(EmployeeForm.tbpAttachment)

        EmployeeForm.tabctrlemp.SelectedIndex = index
        EmployeeForm.tabIndx = index
        ChangeForm(EmployeeForm, "Employee Attachment")
        EmployeeForm.tbpAttachment.Focus()
        'Employee.tbpAttachment_Enter(sender, e)
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
        If sys_ownr.CurrentSystemOwner <> SystemOwner.Hyundai Then
            JobLevelToolStripMenuItem.Visible = False
            JobCategoryToolStripMenuItem.Visible = False
            PointsToolStripMenuItem.Visible = False
        End If

        If Not Debugger.IsAttached Then
            EmployeeExperimentalToolStripMenuItem.Visible = False
        End If

        PrepareFormForUserLevelAuthorizations()

    End Sub

    Private Sub PrepareFormForUserLevelAuthorizations()

        Using context As New PayrollContext

            Dim user = context.Users.FirstOrDefault(Function(u) u.RowID.Value = z_User)

            If user Is Nothing Then

                MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
            End If

            Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

            If settings.GetBoolean("User Policy.UseUserLevel", False) = False Then

                Return

            End If

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
                LeaveToolStripMenuItem.Visible = False
                OfficialBusinessToolStripMenuItem.Visible = False
                AttachmentToolStripMenuItem.Visible = False
                OffSetToolStripMenuItem.Visible = False

                If user.UserLevel = UserLevel.Five Then

                    LoansToolStripMenuItem.Visible = False
                    AllowanceToolStripMenuItem.Visible = False
                    OvertimeToolStripMenuItem.Visible = False

                End If

            End If

        End Using

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
            (curr_sys_owner_name = SystemOwner.Cinema2000)

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

    Private Sub HrisToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoansToolStripMenuItem.Click
        ChangeForm(EmployeeLoansForm, "Employee Loan Schedule")
        previousForm = EmployeeLoansForm
    End Sub

    Private Sub AllowanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllowanceToolStripMenuItem.Click
        ChangeForm(EmployeeAllowanceForm, "Employee Allowance")
        previousForm = EmployeeAllowanceForm
    End Sub

End Class