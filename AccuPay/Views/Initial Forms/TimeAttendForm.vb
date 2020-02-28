Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class TimeAttendForm

    Public emp_ft_col As String = New ExecuteQuery("SELECT GROUP_CONCAT(CONCAT('e.',ii.COLUMN_NAME)) AS Result FROM information_schema.STATISTICS ii WHERE ii.INDEX_TYPE='FULLTEXT' AND ii.TABLE_SCHEMA='" & sys_db & "' AND ii.TABLE_NAME='employee' ORDER BY ii.SEQ_IN_INDEX;").Result

    Public listTimeAttendForm As New List(Of String)

    Private sys_ownr As New SystemOwner

    Private lRepo As ListOfValueRepository

    Private Sub ChangeForm(ByVal Formname As Form, Optional ViewName As String = Nothing)

        reloadViewPrivilege()

        Dim view_ID = ValNoComma(VIEW_privilege(ViewName, orgztnID))

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If PayrollTools.CheckIfUsingUserLevel() = True OrElse formuserprivilege.Count > 0 Then

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
            Dim FName As String = Formname.Name 'Formname.KeyPreview = False 'True
            Formname.TopLevel = False
            Formname.Enabled = True
            If listTimeAttendForm.Contains(FName) Then
                'Formname.Show()
                'Formname.BringToFront()
                'Formname.Focus()
            Else
                PanelTimeAttend.Controls.Add(Formname)
                listTimeAttendForm.Add(Formname.Name)
                Formname.Refresh()
                'Formname.Location = New Point((PanelTimeAttend.Width / 2) - (Formname.Width / 2), (PanelTimeAttend.Height / 2) - (Formname.Height / 2))
                'Formname.Anchor = AnchorStyles.Top And AnchorStyles.Bottom And AnchorStyles.Right And AnchorStyles.Left
                'Formname.WindowState = FormWindowState.Maximized
                Formname.Dock = DockStyle.Fill
            End If
            Formname.Show()
            Formname.BringToFront()
            Formname.Focus()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            Dim listOfForms = PanelTimeAttend.Controls.Cast(Of Form).Where(Function(i) i.Name <> Formname.Name)
            For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
                'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
                pb.Enabled = False
            Next
        End Try
    End Sub

    Private Sub TimeEntToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TimeEntToolStripMenuItem.Click
        'ChangeForm(AttendanceTimeEntryForm)
        'EmployeeShiftEntryForm.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        ChangeForm(EmployeeShiftEntryForm, "Employee Shift")
        previousForm = EmployeeShiftEntryForm
    End Sub

    Private Sub TimeAttendForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        For Each objctrl As Control In PanelTimeAttend.Controls
            If TypeOf objctrl Is Form Then
                DirectCast(objctrl, Form).Close()

            End If
        Next

    End Sub

    Private Sub TimeAttendForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim checker = FeatureListChecker.Instance
        MassOvertimeToolStripMenuItem.Visible = checker.HasAccess(Feature.MassOvertime)

        LoadShiftSchedulePolicyAsync()

        PrepareFormForUserLevelAuthorizations()
    End Sub

    Private Async Sub LoadShiftSchedulePolicyAsync()
        lRepo = New ListOfValueRepository
        Dim shiftPolicies = Await lRepo.GetShiftPolicies()

        If Not shiftPolicies.Any() Then
            ShiftScheduleToolStripMenuItem.Visible = False
            Return
        End If
        Dim settings = New ListOfValueCollection(shiftPolicies)
        Dim _policy = New TimeEntryPolicy(settings)

        Dim _bool = _policy.UseShiftSchedule
        ShiftScheduleToolStripMenuItem.Visible = _bool
        TimeEntToolStripMenuItem.Visible = Not _bool
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

                LeaveToolStripMenuItem.Visible = False
                OfficialBusinessToolStripMenuItem.Visible = False

                If user.UserLevel = UserLevel.Five Then

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

    Private Sub PanelTimeAttend_ControlRemoved(sender As Object, e As ControlEventArgs) Handles PanelTimeAttend.ControlRemoved
        Dim listOfForms = PanelTimeAttend.Controls.Cast(Of Form)()
        For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
            'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
            pb.Enabled = True
            Exit For
        Next
    End Sub

    Private Sub SummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SummaryToolStripMenuItem.Click
        ChangeForm(TimeEntrySummaryForm, "Employee Time Entry logs")
        previousForm = TimeEntrySummaryForm
    End Sub

    Private Sub MassOvertimeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MassOvertimeToolStripMenuItem.Click
        ChangeForm(MassOvertimeForm, "Employee Time Entry Logs")
        previousForm = MassOvertimeForm
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ShiftScheduleToolStripMenuItem.Click
        ChangeForm(ShiftScheduleForm, "Employee Time Entry Logs")
        previousForm = ShiftScheduleForm
    End Sub

    Private Sub ToolStripMenuItem1_Click_1(sender As Object, e As EventArgs) Handles TimeLogsToolStripMenuItem.Click
        ChangeForm(TimeLogsForm2, "Employee Time Entry logs")
        previousForm = TimeLogsForm2
    End Sub

    Private Sub LeaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LeaveToolStripMenuItem.Click
        ChangeForm(EmployeeLeavesForm, "Employee Leave")
        previousForm = EmployeeLeavesForm
    End Sub

    Private Sub OfficialBusinessToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OfficialBusinessToolStripMenuItem.Click
        ChangeForm(OfficialBusinessForm, "Official Business filing")
        previousForm = OfficialBusinessForm
    End Sub

    Private Sub OvertimeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OvertimeToolStripMenuItem.Click
        ChangeForm(EmployeeOvertimeForm, "Employee Overtime")
        previousForm = EmployeeOvertimeForm
    End Sub

End Class