Imports System.Configuration
Imports System.Threading
Imports Indigo
Imports MySql.Data.MySqlClient

'Imports System
'Imports System.Threading

Public Class MDIPrimaryForm

    Dim DefaultFontStyle = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim ExemptedForms As New List(Of String)

    'Dim ctlMDI As MdiClient

    Private versionNo As String

    Private str_pending_leave As String =
        String.Concat("SELECT e.EmployeeID",
                      ", CONCAT_WS(', ', e.LastName, e.FirstName) `FullName`",
                      ", CONCAT_WS(' to ', CONCAT(TIME_FORMAT(elv.LeaveStartTime, '%l:%i'), LEFT(TIME_FORMAT(elv.LeaveEndTime, '%p'), 1))",
                      "                  , CONCAT(TIME_FORMAT(elv.LeaveEndTime, '%l:%i'), LEFT(TIME_FORMAT(elv.LeaveEndTime, '%p'), 1))) `LeaveTime`",
                      ", DATE_FORMAT(elv.LeaveStartDate, '%c/%e/%Y') `LeaveStartDate`",
                      ", (DATEDIFF(elv.LeaveEndDate, elv.LeaveStartDate) + 1) `LeaveDays`",
                      " FROM employeeleave elv",
                      " INNER JOIN employee e ON e.RowID=elv.EmployeeID",
                      " WHERE elv.OrganizationID=?og_rowid",
                      " And elv.`Status`=?lv_status",
                      " ORDER BY elv.Created DESC;")

    Private if_sysowner_is_hyundai As Boolean =
        Convert.ToInt16(New SQL("SELECT EXISTS(SELECT RowID FROM systemowner WHERE IsCurrentOwner='1' AND Name='Hyundai' LIMIT 1) `Result`;").GetFoundRow)

    Private if_sysowner_is_cinema2k As Boolean =
        Convert.ToInt16(New SQL("SELECT EXISTS(SELECT RowID FROM systemowner WHERE IsCurrentOwner='1' AND Name='Cinema 2000' LIMIT 1) `Result`;").GetFoundRow)

    Protected Overrides Sub OnLoad(e As EventArgs)

        'For Each ctl As Control In Me.Controls
        '    Try
        '        ctlMDI = DirectCast(ctl, MdiClient)
        '        ctlMDI.BackColor = Color.White
        '    Catch ex As Exception
        '        Continue For
        '    End Try
        'Next

        'Me.IsMdiContainer = True

        'ToolStripButton0_Click(ToolStripButton0, New EventArgs)

        With ExemptedForms
            .Add("MDIPrimaryForm")
            .Add("MetroLogin")
            .Add("FormReports")
            .Add("GeneralForm")
            .Add("HomeForm")
            .Add("HRISForm")
            .Add("PayrollForm")
            .Add("TimeAttendForm")
        End With

        SplitContainer1.SplitterWidth = 6

        SplitContainer2.SplitterWidth = 6

        Panel2.Font = DefaultFontStyle

        Panel3.Font = DefaultFontStyle

        Panel12.Font = DefaultFontStyle

        Panel11.Font = DefaultFontStyle

        Panel13.Font = DefaultFontStyle

        Panel14.Font = DefaultFontStyle

        Panel6.Font = DefaultFontStyle

        Panel5.Font = DefaultFontStyle

        Panel7.Font = DefaultFontStyle

        CollapsibleGroupBox6.Visible = if_sysowner_is_hyundai
        Panel15.Font = DefaultFontStyle

        setProperDashBoardAccordingToSystemOwner()

        Panel1.Focus()
        BackgroundWorker1.RunWorkerAsync()
        MyBase.OnLoad(e)

        'LoginForm.Hide()
        MetroLogin.Hide()

        'MsgBox("Done")

    End Sub

    Public listofGroup As New AutoCompleteStringCollection

    Public Sub ChangeForm(ByVal Formname As Form)

        Try
            Application.DoEvents()
            Dim FName As String = Formname.Name
            Formname.TopLevel = False

            If listofGroup.Contains(FName) Then
                Formname.Show()
                Formname.BringToFront()
            Else
                Me.Panel1.Controls.Add(Formname)
                'Formname.MdiParent = Me
                listofGroup.Add(Formname.Name)

                Formname.Show()
                Formname.BringToFront()

                'Formname.Location = New Point((Panel1.Width / 2) - (Formname.Width / 2), (Panel1.Height / 2) - (Formname.Height / 2))
                'Formname.Anchor = AnchorStyles.Top And AnchorStyles.Bottom And AnchorStyles.Right And AnchorStyles.Left
                'Formname.WindowState = FormWindowState.Maximized
                Formname.Dock = DockStyle.Fill

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblTime.Text = TimeOfDay
    End Sub

    Dim ClosingForm As Form = Nothing 'New

    Dim busy_bgworks(1) As System.ComponentModel.BackgroundWorker

    Private Sub MDIPrimaryForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'Dim prompt = MsgBox("Do you want to log out ?", MsgBoxStyle.YesNo, "Confirmation")

        busy_bgworks(0) = BackgroundWorker1
        busy_bgworks(1) = bgDashBoardReloader

        Dim busy_bgworker = busy_bgworks.Cast(Of System.ComponentModel.BackgroundWorker).Where(Function(x) x.IsBusy)

        'Console.WriteLine(fdsfsd)

        e.Cancel = (busy_bgworker.Count > 0)

        LockTime()

        'If backgroundworking = 1 Then
        '    'e.Cancel = True

        'Else

        'End If

        If e.Cancel = False Then

            Dim prompt = MessageBox.Show("Do you want to log out ?", "Confirming log out", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

            If prompt = MsgBoxResult.Yes Then

                position_view_table = Nothing

                e.Cancel = False

                ''Close all forms that remains open

                Dim listofExtraFrm As Form()

                Dim listofExtraForm As New List(Of String)

                listofExtraForm.Add("CrysVwr")
                listofExtraForm.Add("dutyshift")
                listofExtraForm.Add("leavtyp")
                listofExtraForm.Add("LoanType")
                listofExtraForm.Add("newEmpStat")
                listofExtraForm.Add("newEmpType")
                listofExtraForm.Add("newPostion")
                listofExtraForm.Add("newProdAllowa")
                listofExtraForm.Add("newProdBonus")
                listofExtraForm.Add("SelectFromEmployee")
                listofExtraForm.Add("selectPayPeriod")
                listofExtraForm.Add("viewtotallow")
                listofExtraForm.Add("viewtotbon")
                listofExtraForm.Add("viewtotloan")
                listofExtraForm.Add("FindingForm")

                listofExtraForm.Add("AddListOfValueForm")
                listofExtraForm.Add("AddPostionForm")

                listofExtraForm.Add("GeneralForm")
                listofExtraForm.Add("HRISForm")
                listofExtraForm.Add("PayrollForm")
                listofExtraForm.Add("TimeAttendForm")

                ReDim listofExtraFrm(My.Application.OpenForms.Count - 1)

                Dim itemindex = 0

                Dim open_forms = My.Application.OpenForms

                For Each f As Form In open_forms

                    'If f.Name = "MDIPrimaryForm" Or _
                    '    f.Name = "MetroLogin" Or _
                    '    f.Name = "FormReports" Or _
                    '    f.Name = "GeneralForm" Or _
                    '    f.Name = "HomeForm" Or _
                    '    f.Name = "HRISForm" Or _
                    '    f.Name = "PayrollForm" Or _
                    '    f.Name = "TimeAttendForm" Then

                    Dim frmName = f.Name

                    If ExemptedForms.Contains(frmName) Then
                        Continue For
                    Else

                        If listofExtraForm.Contains(frmName) Then
                            Continue For
                            'ReDim Preserve listofExtraFrm(itemindex + 1)

                            'f.Close()
                        Else
                            If frmName.Trim.Length > 0 Then
                                listofExtraFrm(itemindex) = f
                                itemindex += 1

                            End If

                        End If

                    End If
                Next

                Dim openform_count = listofExtraFrm.GetUpperBound(0)

                For ii = 0 To openform_count

                    If listofExtraFrm(ii) Is Nothing Then
                        Continue For
                    Else
                        'ClosingForm = New Form

                        ClosingForm = listofExtraFrm(ii)

                        'Dim frmName = ClosingForm.Name

                        'ClosingForm.Dispose()

                        ClosingForm.Close()

                    End If

                Next

                'GeneralForm.Close()
                'HRISForm.Close()
                'PayrollForm.Close()
                'TimeAttendForm.Close()

                Dim n_ExecuteQuery As _
                    New ExecuteQuery("UPDATE user" &
                                     " SET InSession='0'" &
                                     ",LastUpd=CURRENT_TIMESTAMP()" &
                                     ",LastUpdBy='" & z_User & "'" &
                                     " WHERE RowID='" & z_User & "';")

                If openform_count >= 5 Then
                    Thread.Sleep(1175)
                End If

                ''LoginForm.Show()

                ''LoginForm.fillPosition()

                ''LoginForm.UsernameTextBox.Clear()
                ''LoginForm.PasswordTextBox.Clear()

                ''LoginForm.UsernameTextBox.Focus()

                ''LoginForm.loadImage(LoginForm.cmbBranchName.Text)

                With MetroLogin

                    .Show()

                    .txtbxUserID.Clear()

                    .txtbxPword.Clear()

                    .txtbxUserID.Focus()

                    .PhotoImages.Image = Nothing

                    .cbxorganiz.SelectedIndex = -1

                    .ReloadOrganization()

                End With

            ElseIf prompt = MsgBoxResult.No Then
                e.Cancel = True
            Else
                e.Cancel = True
            End If

            Dim e_CloseReason = New String() {1, 2, 3, 4, 5, 6}

            If e_CloseReason.Contains(e.CloseReason) Then

                'n_UserLog.Out()

            End If
        Else

        End If

    End Sub

    'Dim n_UserLog As New UserLog

    Private Sub MDIPrimaryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            If dbnow = Nothing Then
                dbnow = EXECQUER(CURDATE_MDY)
            End If

            ToolStripButton3.Text = "Time &&" & vbNewLine & "Attendance"

            ToolStripButton3.ToolTipText = "Time & Attendance"

            '123, 24

            lblTime.Text = TimeOfDay
            'lblUser.Text = Z_UserName
            lblUser.Text = userFirstName &
                           If(userLastName = Nothing, "", " " & userLastName)

            lblPosition.Text = z_postName

            ToolStripButton0_Click(sender, e)

            PictureBox1.Image = ImageList1.Images(1)
            LoadVersionNo()
            'Me.Text = orgNam

            'n_UserLog.Out()

            'n_UserLog.Inn()
        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))
        Finally

        End Try

    End Sub

    Private Sub LoadVersionNo()
        Dim appSettings = ConfigurationManager.AppSettings
        Me.versionNo = appSettings.Get("payroll.version")

        If versionNo IsNot Nothing Then
            lblVersionValue.Text = Me.versionNo
        Else
            lblVersionValue.Text = "Version no is missing."
        End If
    End Sub

    'Trebuchet MS
    'Segoe UI

    Dim selectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim isHome As SByte = 0

    Private Sub ToolStripButton0_Click(sender As Object, e As EventArgs) Handles ToolStripButton0.Click

        isHome = 1

        UnlockTime()

        GeneralForm.Hide()
        HRISForm.Hide()
        PayrollForm.Hide()
        TimeAttendForm.Hide()

        FormReports.Hide()

        ToolStripButton0.BackColor = Color.FromArgb(255, 255, 255)

        ToolStripButton1.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton2.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton3.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton4.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton5.BackColor = Color.FromArgb(194, 228, 255)

        ToolStripButton0.Font = selectedButtonFont

        ToolStripButton1.Font = unselectedButtonFont
        ToolStripButton2.Font = unselectedButtonFont
        ToolStripButton3.Font = unselectedButtonFont
        ToolStripButton4.Font = unselectedButtonFont
        ToolStripButton5.Font = unselectedButtonFont

        Static once As SByte = 0
        If once = 0 Then
            once = 1
            'Me.Text = "Welcome"
            Me.Text = orgNam
        Else
            'Me.Text = ""

        End If

    End Sub

    Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        isHome = 0

        LockTime()

        ChangeForm(GeneralForm)

        HRISForm.Hide()
        PayrollForm.Hide()
        TimeAttendForm.Hide()

        FormReports.Hide()

        ToolStripButton0.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton2.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton3.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton4.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton5.BackColor = Color.FromArgb(194, 228, 255)

        ToolStripButton1.BackColor = Color.FromArgb(255, 255, 255)

        ToolStripButton1.Font = selectedButtonFont

        ToolStripButton0.Font = unselectedButtonFont
        ToolStripButton2.Font = unselectedButtonFont
        ToolStripButton3.Font = unselectedButtonFont
        ToolStripButton4.Font = unselectedButtonFont
        ToolStripButton5.Font = unselectedButtonFont

        refresh_previousForm(0, sender, e)

        'If FormLeft.Count = 0 Then
        '    Me.Text = "Welcome"
        'Else
        '    Me.Text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click

        isHome = 0

        LockTime()

        ChangeForm(TimeAttendForm)

        GeneralForm.Hide()
        HRISForm.Hide()
        PayrollForm.Hide()

        FormReports.Hide()

        ToolStripButton0.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton1.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton2.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton4.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton5.BackColor = Color.FromArgb(194, 228, 255)

        ToolStripButton3.BackColor = Color.FromArgb(255, 255, 255)

        ToolStripButton3.Font = selectedButtonFont

        ToolStripButton0.Font = unselectedButtonFont
        ToolStripButton1.Font = unselectedButtonFont
        ToolStripButton2.Font = unselectedButtonFont
        ToolStripButton4.Font = unselectedButtonFont
        ToolStripButton5.Font = unselectedButtonFont

        refresh_previousForm(2, sender, e)
    End Sub

    Private Sub Panel1_Resize(sender As Object, e As EventArgs) Handles Panel1.Resize
        'For Each panelcontrl As Control In Panel1.Controls
        '    If TypeOf panelcontrl Is Form Then
        '        panelcontrl.Width = Panel1.Width
        '    End If
        'Next
    End Sub

    Dim theemployeetable As New DataTable

    Sub refresh_previousForm(Optional groupindex As Object = 0,
                             Optional sndr As Object = 0,
                             Optional ee As EventArgs = Nothing)

        Static once As SByte = 0

        If once = 0 Then
            'once = 1

            Exit Sub

        End If

        Static countchanges As Integer = -1
        'SELECT RowID,CreatedBy,Created,LastUpdBy,LastUpd,OrganizationID,Salutation,FirstName,MiddleName,LastName,Surname,EmployeeID,TINNo,SSSNo,HDMFNo,PhilHealthNo,EmploymentStatus,EmailAddress,WorkPhone,HomePhone,MobilePhone,HomeAddress,Nickname,JobTitle,Gender,EmployeeType,MaritalStatus,Birthdate,StartDate,TerminationDate,PositionID,PayFrequencyID,NoOfDependents,UndertimeOverride,OvertimeOverride,NewEmployeeFlag,LeaveBalance,SickLeaveBalance,MaternityLeaveBalance,LeaveAllowance,SickLeaveAllowance,MaternityLeaveAllowance,LeavePerPayPeriod,SickLeavePerPayPeriod,MaternityLeavePerPayPeriod FROM employee;

        'theemployeetable = retAsDatTbl("SELECT RowID" & _
        '                               ",CreatedBy" & _
        '                               ",Created" & _
        '                               ",LastUpdBy" & _
        '                               ",LastUpd" & _
        '                               ",OrganizationID" & _
        '                               ",Salutation" & _
        '                               ",FirstName" & _
        '                               ",MiddleName" & _
        '                               ",LastName" & _
        '                               ",Surname" & _
        '                               ",EmployeeID" & _
        '                               ",TINNo" & _
        '                               ",SSSNo" & _
        '                               ",HDMFNo" & _
        '                               ",PhilHealthNo" & _
        '                               ",EmploymentStatus" & _
        '                               ",EmailAddress" & _
        '                               ",WorkPhone" & _
        '                               ",HomePhone" & _
        '                               ",MobilePhone" & _
        '                               ",HomeAddress" & _
        '                               ",Nickname" & _
        '                               ",JobTitle" & _
        '                               ",Gender" & _
        '                               ",EmployeeType" & _
        '                               ",MaritalStatus" & _
        '                               ",Birthdate" & _
        '                               ",StartDate" & _
        '                               ",TerminationDate" & _
        '                               ",PositionID" & _
        '                               ",PayFrequencyID" & _
        '                               ",NoOfDependents" & _
        '                               ",UndertimeOverride" & _
        '                               ",OvertimeOverride" & _
        '                               ",NewEmployeeFlag" & _
        '                               ",LeaveBalance" & _
        '                               ",SickLeaveBalance" & _
        '                               ",MaternityLeaveBalance" & _
        '                               ",LeaveAllowance" & _
        '                               ",SickLeaveAllowance" & _
        '                               ",MaternityLeaveAllowance" & _
        '                               ",LeavePerPayPeriod" & _
        '                               ",SickLeavePerPayPeriod" & _
        '                               ",MaternityLeavePerPayPeriod" & _
        '                               " FROM employee" & _
        '                               " WHERE DATE_FORMAT(LastUpd,'%Y-%m-%d')=CURRENT_DATE();")

        'If theemployeetable.Rows.Count <> 0 Then

        'Else

        'End If

        If previousForm IsNot Nothing Then
            'previousForm = Nothing

            If groupindex = 0 Then 'General

                If previousForm.Name = "UsersFrom" Then

                ElseIf previousForm.Name = "ListOfValueForm" Then

                ElseIf previousForm.Name = "OrganizatinoForm" Then

                ElseIf previousForm.Name = "UserPrivilegeForm" Then

                ElseIf previousForm.Name = "PhilHealht" Then

                ElseIf previousForm.Name = "SSSCntrib" Then

                ElseIf previousForm.Name = "Payrate" Then

                ElseIf previousForm.Name = "ShiftEntryForm" Then

                ElseIf previousForm.Name = "userprivil" Then

                ElseIf previousForm.Name = "Revised_Withholding_Tax_Tables" Then

                End If

            ElseIf groupindex = 1 Then 'HRIS

                If previousForm.Name = "Employee" Then

                    With EmployeeForm

                        Select Case .tabIndx
                            Case 0 'Checklist

                            Case 1 'Personal Profile
                                If .listofEditDepen.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 2 'Awards
                                If .listofEditRowAward.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 3 'Certification
                                If .listofEditRowCert.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 4 'Leave
                                If .listofEditRowleave.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 5 'Medicla Profile
                                If .listofEditedmedprod.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 6 'Disciplinary action

                            Case 7 'Educational Background

                            Case 8 'Previous Employer

                            Case 9 'Promotion

                            Case 10 'Loan Schedule

                            Case 11 'Loan History

                            Case 12 'Salary
                                If .listofEditEmpSal.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 13 'Pay slip
                                If .listofEditedmedprod.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 14 'Allowance
                                If .listofEditEmpAllow.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 15 'Overtime
                                If .listofEditRowEmpOT.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 16 'Official business
                                If .listofEditRowOBF.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 17 'Bonus
                                If .listofEditRowBon.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                            Case 18 'Attachment
                                If .listofEditRoweatt.Count = 0 Then
                                    .SearchEmoloyee_Click(sndr, ee)
                                Else

                                End If

                        End Select

                    End With

                ElseIf previousForm.Name = "Positn" Then

                ElseIf previousForm.Name = "EmpPosition" Then

                ElseIf previousForm.Name = "DivisionForm" Then

                End If

            ElseIf groupindex = 2 Then 'Time Attendance

                If previousForm.Name = "ShiftEntryForm" Then

                ElseIf previousForm.Name = "EmployeeShiftEntryForm" Then

                ElseIf previousForm.Name = "Payrate" Then 'ShiftEntryForm

                ElseIf previousForm.Name = "EmpTimeDetail" Then

                ElseIf previousForm.Name = "EmpTimeEntry" Then

                    If Application.OpenForms().OfType(Of TimEntduration).Any Then
                        If TimEntduration.bgWork.IsBusy Then
                        Else
                            EmpTimeEntry.btnRerfresh_Click(sndr, ee)
                        End If

                    End If

                End If

            ElseIf groupindex = 3 Then 'Payroll
                If previousForm.Name = "Paystub" Then
                    With PayStub
                        .btnrefresh_Click(sndr, ee)
                    End With
                End If
            End If

            'previousForm = UsersFrom
            'previousForm = ListOfValueForm
            'previousForm = OrganizatinoForm
            'previousForm = UserPrivilegeForm
            'previousForm = PhilHealht
            'previousForm = SSSCntrib
            'previousForm = Revised_Withholding_Tax_Tables
            'previousForm = Positn
            'previousForm = DivisionForm
            'previousForm = Paystub
            'previousForm = ShiftEntryForm
            'previousForm = EmployeeShiftEntryForm
            'previousForm = Payrate
            'previousForm = EmpTimeDetail
            'previousForm = EmpTimeEntry

            countchanges = theemployeetable.Rows.Count
        End If
    End Sub

    Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click

        isHome = 0

        LockTime()

        ChangeForm(PayrollForm)

        GeneralForm.Hide()
        HRISForm.Hide()
        TimeAttendForm.Hide()

        FormReports.Hide()

        ToolStripButton0.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton1.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton2.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton3.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton5.BackColor = Color.FromArgb(194, 228, 255)

        ToolStripButton4.BackColor = Color.FromArgb(255, 255, 255)

        ToolStripButton4.Font = selectedButtonFont

        ToolStripButton0.Font = unselectedButtonFont
        ToolStripButton1.Font = unselectedButtonFont
        ToolStripButton2.Font = unselectedButtonFont
        ToolStripButton3.Font = unselectedButtonFont
        ToolStripButton5.Font = unselectedButtonFont

        refresh_previousForm(3, sender, e)

    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click

        isHome = 0

        LockTime()

        ChangeForm(HRISForm)

        GeneralForm.Hide()
        PayrollForm.Hide()
        TimeAttendForm.Hide()

        FormReports.Hide()

        ToolStripButton0.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton1.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton3.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton4.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton5.BackColor = Color.FromArgb(194, 228, 255)

        ToolStripButton2.BackColor = Color.FromArgb(255, 255, 255)

        ToolStripButton2.Font = selectedButtonFont

        ToolStripButton0.Font = unselectedButtonFont
        ToolStripButton1.Font = unselectedButtonFont
        ToolStripButton3.Font = unselectedButtonFont
        ToolStripButton4.Font = unselectedButtonFont
        ToolStripButton5.Font = unselectedButtonFont

        If HRISForm.listHRISForm.Count < 0 Then

            Dim indx = HRISForm.listHRISForm.Count - 1

            If HRISForm.listHRISForm.Item(indx) = "Employee" Then
                Select Case EmployeeForm.tabctrlemp.SelectedIndex
                    Case 0

                    Case 1
                        With EmployeeForm
                            If .tsbtnNewEmp.Enabled = True Then
                                Dim isTableChange = EXECQUER("SELECT EXISTS(SELECT RowID" &
                                                             " FROM position" &
                                                             " WHERE CURRENT_DATE()" &
                                                             " IN (DATE_FORMAT(Created,'%Y-%m-%d'),DATE_FORMAT(LastUpd,'%Y-%m-%d'))" &
                                                             " AND OrganizationID=" & orgztnID & " LIMIT 1);")

                                If isTableChange = 1 Then

                                End If

                            End If

                        End With

                    Case 2

                    Case 3

                    Case 4

                    Case 5

                    Case 6

                    Case 7

                    Case 8

                    Case 9

                    Case 10

                    Case 11

                    Case 12

                    Case 13

                    Case 14

                    Case 15

                    Case 16

                    Case 17

                    Case 18

                End Select

            ElseIf HRISForm.listHRISForm.Item(indx) = "EmpPosition" Then
                'MsgBox("EmpPosition")
            ElseIf HRISForm.listHRISForm.Item(indx) = "DivisionForm" Then
                'MsgBox("DivisionForm")
            End If

        End If

        refresh_previousForm(1, sender, e)
    End Sub

    'Toggling pin status

    Private Sub Pin_UnPin(sender As Object, e As EventArgs)

        Static once As SByte = 0

        If once = 0 Then
            once = 1
            'ToolStripButton5.Image.Tag = 1
        End If

        ImageList1.Images(0).Tag = 1
        ImageList1.Images(1).Tag = 2

        'If ToolStripButton5.Image.Tag = 1 Then 'Unhide toolstrip
        '    ToolStripButton5.Image = ImageList1.Images(1)
        '    ToolStripButton5.Image.Tag = 0

        'Else '                                  'Hide toolstrip
        '    ToolStripButton5.Image = ImageList1.Images(0)
        '    ToolStripButton5.Image.Tag = 1

        'End If

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

        Static once As SByte = 0

        If once = 0 Then
            once = 1
            PictureBox1.Image.Tag = 1
        End If

        ImageList1.Images(0).Tag = 1
        ImageList1.Images(1).Tag = 2

        If PictureBox1.Image.Tag = 1 Then 'Hide toolstrip
            PictureBox1.Image = ImageList1.Images(0)
            PictureBox1.Image.Tag = 0

            Showmainbutton.Dock = DockStyle.None
        Else '                             'Show toolstrip
            PictureBox1.Image = ImageList1.Images(1)
            PictureBox1.Image.Tag = 1

            Showmainbutton.Dock = DockStyle.Left

        End If

    End Sub

    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

    '    If keyData = Keys.RControlKey And Keys.D1 Then

    '        ToolStripButton0_Click(ToolStripButton0, New EventArgs)

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    ElseIf keyData = Keys.F2 Then
    '        ToolStripButton1_Click(ToolStripButton1, New EventArgs)

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    ElseIf keyData = Keys.F3 Then
    '        ToolStripButton4_Click(ToolStripButton2, New EventArgs)

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    ElseIf keyData = Keys.F4 Then
    '        ToolStripButton3_Click(ToolStripButton3, New EventArgs)

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    ElseIf keyData = Keys.F5 Then
    '        ToolStripButton5_Click(ToolStripButton4, New EventArgs)

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    Else

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    End If

    'End Function

    'Protected Overrides Function ProcessKeyPreview(ByRef m As Message) As Boolean

    '    Return MyBase.ProcessKeyPreview(m)

    'End Function

    Private Sub ToolStripButton5_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton5.Click

        isHome = 0

        LockTime()

        ChangeForm(FormReports)

        GeneralForm.Hide()
        HRISForm.Hide()
        TimeAttendForm.Hide()

        ToolStripButton0.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton1.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton2.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton3.BackColor = Color.FromArgb(194, 228, 255)
        ToolStripButton4.BackColor = Color.FromArgb(194, 228, 255)

        ToolStripButton5.BackColor = Color.FromArgb(255, 255, 255)

        ToolStripButton5.Font = selectedButtonFont

        ToolStripButton0.Font = unselectedButtonFont
        ToolStripButton1.Font = unselectedButtonFont
        ToolStripButton2.Font = unselectedButtonFont
        ToolStripButton3.Font = unselectedButtonFont
        ToolStripButton4.Font = unselectedButtonFont

    End Sub

    Sub LockTime()
        Timer2.Stop()
        bgDashBoardReloader.CancelAsync()
        Timer2.Enabled = False
    End Sub

    Sub UnlockTime()

        Timer2.Enabled = True

        Timer2.Start()

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Timer2_Tick(Timer2, New EventArgs)

        End If

    End Sub

    Private Sub Timer2_Tick(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Timer2.Tick

        TimeTick += 1

        If TimeTick = 1 Then ' the timer now succeeds 60 seconds

            TimeTick = 0

            LockTime()

            If bgDashBoardReloader.IsBusy = False Then

                bgDashBoardReloader.RunWorkerAsync()

            End If

        End If

    End Sub

    Dim n_bgwAge21Dependents = Nothing

    Dim n_bgwBDayCelebrant = Nothing

    Dim n_bgwOBPending = Nothing

    Dim n_bgwOTPending = Nothing

    Dim n_bgwLoanBalances = Nothing

    Dim n_bgwNegaPaySlips = Nothing

    Dim n_bgwForRegularization = Nothing

    Dim dt_pend_leave As New DataTable

    Private Sub bgDashBoardReloader_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgDashBoardReloader.DoWork

        Dim params(0, 1) As Object

        params(0, 0) = "OrganizID"

        params(0, 1) = orgztnID

        n_bgwAge21Dependents = New DashBoardDataExtractor(params,
                                                          "DBoard_Age21Dependents")

        n_bgwAge21Dependents = n_bgwAge21Dependents.getDataTable

        n_bgwBDayCelebrant = New DashBoardDataExtractor(params,
                                                        "DBoard_BirthdayCelebrantThisMonth")

        n_bgwBDayCelebrant = n_bgwBDayCelebrant.getDataTable

        n_bgwOBPending = New DashBoardDataExtractor(params,
                                                        "DBoard_OBPending")

        n_bgwOBPending = n_bgwOBPending.getDataTable

        n_bgwOTPending = New DashBoardDataExtractor(params,
                                                        "DBoard_OTPending")

        n_bgwOTPending = n_bgwOTPending.getDataTable

        n_bgwLoanBalances = New DashBoardDataExtractor(params,
                                                        "DBoard_LoanBalances")

        n_bgwLoanBalances = n_bgwLoanBalances.getDataTable

        n_bgwNegaPaySlips = New DashBoardDataExtractor(params,
                                                        "DBoard_NegativePaySlips")

        n_bgwNegaPaySlips = n_bgwNegaPaySlips.getDataTable

        n_bgwForRegularization = New DashBoardDataExtractor(params,
                                                        "DBoard_ForRegularization")

        n_bgwForRegularization = n_bgwForRegularization.getDataTable

        dgvfrequentabsent.Tag = New SQLQueryToDatatable("CALL `FREQUENT_absent`('" & orgztnID & "');").ResultTable

        dgvfrequentleave.Tag = New SQLQueryToDatatable("CALL `FREQUENT_leave`('" & orgztnID & "');").ResultTable

        'dgvfrequentleave

        If if_sysowner_is_hyundai Then

            Dim pend_leave As New SQL(str_pending_leave,
                                  New Object() {orgztnID, "Pending"})

            dt_pend_leave = pend_leave.GetFoundRows.Tables(0)

        End If

    End Sub

    Private Sub bgDashBoardReloader_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgDashBoardReloader.ProgressChanged

    End Sub

    Private Sub bgDashBoardReloader_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgDashBoardReloader.RunWorkerCompleted

        If e.Error IsNot Nothing Then

            MessageBox.Show("ERROR : " & e.Error.Message)

        ElseIf e.Cancelled Then

            MessageBox.Show("CANCELLED" & vbNewLine & e.Error.Message)
        Else

        End If

        UnlockTime()

        Static once As SByte = 0

        Dim dattbl = InstantiateDatatable(n_bgwAge21Dependents)

        PopulateDGVwithDatTbl(dgvAge21Depen,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwBDayCelebrant)

        PopulateDGVwithDatTbl(dgvBDayCeleb,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwLoanBalances)

        PopulateDGVwithDatTbl(dgvLoanBalance,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwOBPending)

        PopulateDGVwithDatTbl(dgvOBPending,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwOTPending)

        PopulateDGVwithDatTbl(dgvOTPending,
                              dattbl)

        'dgvnegaPaySlip, n_bgwNegaPaySlips
        dattbl = InstantiateDatatable(n_bgwNegaPaySlips)

        PopulateDGVwithDatTbl(dgvnegaPaySlip,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwForRegularization)

        PopulateDGVwithDatTbl(dgvRegularization,
                              dattbl)

        Dim new_dt As New DataTable
        new_dt = DirectCast(dgvfrequentabsent.Tag, DataTable)
        PopulateDGVwithDatTbl(dgvfrequentabsent,
                              new_dt)

        Dim n_dt As New DataTable
        n_dt = DirectCast(dgvfrequentleave.Tag, DataTable)
        PopulateDGVwithDatTbl(dgvfrequentleave,
                              n_dt)

        If if_sysowner_is_hyundai Then
            dgvpendingleave.Rows.Clear()
            For Each drow As DataRow In dt_pend_leave.Rows
                dgvpendingleave.Rows.Add(drow.ItemArray)
            Next
            dgvpendingleave.Enabled = True
        End If

        dgvAge21Depen.Enabled = True

        dgvBDayCeleb.Enabled = True

        dgvLoanBalance.Enabled = True

        dgvOBPending.Enabled = True

        dgvOTPending.Enabled = True

        dgvnegaPaySlip.Enabled = True

        dgvRegularization.Enabled = True

        dgvfrequentabsent.Enabled = True

        dgvfrequentleave.Enabled = True

        If once = 0 Then

            once = 1

            dgvAge21Depen.Enabled = True

            dgvBDayCeleb.Enabled = True

            AddHandler NotifyIcon1.DoubleClick, AddressOf NotifyIcon1_Click
        Else

            NotifyIcon1.Visible = True

            NotifyIcon1.ShowBalloonTip(30000)

        End If

        'LoginForm.Hide()

    End Sub

    Private Sub NotifyIcon1_Click(sender As Object, e As EventArgs)

        ToolStripButton0_Click(sender, e)

    End Sub

    Private Sub dgvEmp_CellClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub dgvEmp_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.F5 And isHome = 1 Then

            'DashBoardReloader()

            Timer2_Tick(Timer2, New EventArgs)

            Return True

        ElseIf keyData = Keys.F1 Then

            PictureBox1_Click(PictureBox1, New EventArgs)

            Return True

        ElseIf keyData = Keys.Oem5 Then

            Static thrice As Integer = -1

            thrice += 1

            If thrice = 5 Then

                thrice = 0

                'Dim n_BlankTimeEntryLogs As New BlankTimeEntryLogs("2015-11-01", "2015-11-15")

                'If n_BlankTimeEntryLogs.ShowDialog("") = Windows.Forms.DialogResult.OK Then

                '    MsgBox(n_BlankTimeEntryLogs.DialogResult.ToString)

                'End If

                'Dim n_ShiftTemplater As _
                '    New ShiftTemplater

                'n_ShiftTemplater.Show()

            End If

            Return False
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    'Sub DashBoardReloader()

    '    If bgDashBoardReloader.IsBusy = False Then

    '        Dim ii = 0 'n_bgwAge21Dependents.getDataTable.Rows.Count

    '        'MsgBox(ii, , "DashBoardReloader")

    '        'Timer2_Tick(Timer2, New EventArgs)

    '    End If

    'End Sub

    Private Sub MDIPrimaryForm_TextChanged(sender As Object, e As EventArgs) Handles Me.TextChanged

        CenterMe()

    End Sub

    Private Sub MDIPrimaryForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        'Me.Text = Me.Width & " is width" & Panel1.Width & " is panel width"

        CenterMe()

        Width_resolution = Me.Width

        Height_resolution = Me.Height

        'If Me.Size = Me.MinimumSize Then
        '    dgvBDayCeleb.Size = New Size(350, 210)
        'Else
        '    dgvBDayCeleb.Size = New Size(350, 296)
        'End If

    End Sub

    Private Const strtoConcat = " "

    Private Sub CenterMe()
        'Dim g As Graphics = Me.CreateGraphics()
        'Dim startingPoint As Double = (Me.Width / 2) - (g.MeasureString(Me.Text.Trim, Me.Font).Width / 2)
        'Dim widthOfASpace As Double = g.MeasureString(strtoConcat, Me.Font).Width
        'Dim tmp As String = strtoConcat
        'Dim tmpWidth As Double = 0
        'Do
        '    tmp += strtoConcat
        '    tmpWidth += widthOfASpace
        'Loop While (tmpWidth + widthOfASpace) < startingPoint

        'Me.Text = tmp & Me.Text.Trim & tmp

        Me.Refresh()

    End Sub

    Private Sub ctxtmenNothing_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ctxtmenNothing.Opening

    End Sub

    Private Sub SplitContainer1_Panel1_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Panel1.Paint

    End Sub

    Protected Overrides Sub OnActivated(e As EventArgs)

        MyBase.OnActivated(e)

    End Sub

    Protected Overrides Sub OnDeactivate(e As EventArgs)

        MyBase.OnDeactivate(e)

    End Sub

    Dim bgwork_errormsg As String = String.Empty

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        If e.Cancel = False Then
            Dim n_ExecuteQuery As New ExecuteQuery("CALL EXEC_userupdateleavebalancelog('" & orgztnID & "','" & z_User & "');")
            bgwork_errormsg = n_ExecuteQuery.ErrorMessage
        End If
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged

    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            'MsgBox(getErrExcptn(e.Error, Me.Name))
            MsgBox(bgwork_errormsg)
        ElseIf e.Cancelled Then
            MsgBox("Background work cancelled.",
                   MsgBoxStyle.Information)
        Else

        End If

    End Sub

    Private Sub setProperDashBoardAccordingToSystemOwner()

        If if_sysowner_is_cinema2k Then
            setVisiblePropertyDashBoardBaseOnCinema2K(Panel8)
            setVisiblePropertyDashBoardBaseOnCinema2K(Panel9)
            setVisiblePropertyDashBoardBaseOnCinema2K(Panel10)

        End If

    End Sub

    Private Sub setVisiblePropertyDashBoardBaseOnCinema2K(pnl As Panel)

        Dim _list =
            pnl.Controls.OfType(Of CollapsibleGroupBox)()

        For Each collapgpbox In _list
            Dim _bool As Boolean =
                (collapgpbox.AccessibleDescription = SystemOwner.Cinema2000)

            collapgpbox.Visible = _bool

        Next

    End Sub

End Class

Public Class DashBoardDataExtractor

    Dim datatab As New DataTable

    Sub New(Optional ParamsCollection As Array = Nothing,
            Optional ProcedureName As String = Nothing)

        'Dim n_callProcAsDatTable As New callProcAsDatTable

        'datatab = New DataTable

        datatab = callProcAsDatTbl(ParamsCollection,
                                   ProcedureName)

    End Sub

    Public ReadOnly Property getDataTable As DataTable

        Get
            Return datatab

        End Get

    End Property

    Function callProcAsDatTbl(Optional ParamsCollection As Array = Nothing,
                                      Optional ProcedureName As String = Nothing) As Object

        Dim returnvalue = Nothing

        Dim mysqlda As New MySqlDataAdapter()

        Dim new_conn As New MySqlConnection

        new_conn.ConnectionString = db_connectinstring

        Try

            If new_conn.State = ConnectionState.Open Then : new_conn.Close() : End If

            new_conn.Open()

            Dim ds As New DataSet()

            With mysqlda

                .SelectCommand = New MySqlCommand(ProcedureName, new_conn)
                .SelectCommand.CommandTimeout = 999999
                .SelectCommand.CommandType = CommandType.StoredProcedure

                .SelectCommand.Parameters.Clear()

                For e = 0 To ParamsCollection.GetUpperBound(0) ' - 1

                    Dim paramName As String = ParamsCollection(e, 0)

                    Dim paramVal As Object = ParamsCollection(e, 1)

                    .SelectCommand.Parameters.AddWithValue(paramName, paramVal)

                Next

                .Fill(ds, "Table0")

            End With

            Dim dt As DataTable = ds.Tables("Table0")

            returnvalue = dt

            hasERR = 0
        Catch ex As Exception
            hasERR = 1

            MsgBox(getErrExcptn(ex, ProcedureName), MsgBoxStyle.Critical)

            returnvalue = Nothing
        Finally

            mysqlda.Dispose()

        End Try

        Return returnvalue

    End Function

End Class

Public Class UserLog

    Dim syslogViewID = Nothing

    Dim new_conn As New MySqlConnection

    Sub New()

        new_conn.ConnectionString = db_connectinstring

        syslogViewID = EXECQUER("SELECT RowID FROM `view` WHERE ViewName='Login Form' AND OrganizationID='" & orgztnID & "';")

    End Sub

    Sub Inn()

        INS_audittrail("System Log",
                       "",
                       "IN",
                       "",
                       "Log")

    End Sub

    Sub Out()

        EXECQUER("UPDATE `audittrail`" &
                 " SET NewValue='OUT'" &
                 " WHERE CreatedBy='" & z_User & "'" &
                 " AND OrganizationID='" & orgztnID & "'" &
                 " AND NewValue=''" &
                 " AND ViewID='" & syslogViewID & "';")

    End Sub

    Sub INS_audittrail(Optional au_FieldChanged = Nothing,
                       Optional au_ChangedRowID = Nothing,
                       Optional au_OldValue = Nothing,
                       Optional au_NewValue = Nothing,
                       Optional au_ActionPerformed = Nothing)

        Try
            If new_conn.State = ConnectionState.Open Then : new_conn.Close() : End If

            cmd = New MySqlCommand("INS_audittrail", new_conn)

            new_conn.Open()

            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.AddWithValue("au_CreatedBy", z_User)

                .Parameters.AddWithValue("au_LastUpdBy", z_User)

                .Parameters.AddWithValue("au_OrganizationID", orgztnID)

                .Parameters.AddWithValue("au_ViewID", syslogViewID)

                .Parameters.AddWithValue("au_FieldChanged", Trim(au_FieldChanged))

                .Parameters.AddWithValue("au_ChangedRowID", au_ChangedRowID)

                .Parameters.AddWithValue("au_OldValue", Trim(au_OldValue))

                .Parameters.AddWithValue("au_NewValue", Trim(au_NewValue))

                .Parameters.AddWithValue("au_ActionPerformed", Trim(au_ActionPerformed))

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " " & "INS_audittrail", , "Error")
        Finally
            new_conn.Close()
            cmd.Dispose()

        End Try

    End Sub

End Class
