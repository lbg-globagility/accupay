Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.AccuPay.Desktop.Helpers
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection
Imports MySql.Data.MySqlClient
Imports File = System.IO.File

Public Class EmployeeForm

    Private Const EmployeeEntityName As String = "Employee"

    Private str_ms_excel_file_extensn As String =
        String.Concat("Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|",
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls")

    Private if_sysowner_is_benchmark As Boolean

    Private _currentSystemOwner As String

    Private threadArrayList As New List(Of Thread)

    Private _branches As New List(Of Branch)

    Private ReadOnly _policy As IPolicyHelper

    Private ReadOnly _systemOwnerService As ISystemOwnerService

    Private _laGlobalEmployeeReports As New Dictionary(Of String, LaGlobalEmployeeReportName)

    Private _currentRolePermission As RolePermission

    Sub New()

        InitializeComponent()

        _policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of ISystemOwnerService)

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        SplitContainer2.SplitterWidth = 7
        MyBase.OnLoad(e)
    End Sub

    Dim paytypestring As String

    Private Async Sub Employee_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _currentSystemOwner = _systemOwnerService.GetCurrentSystemOwner()

        if_sysowner_is_benchmark = _currentSystemOwner = SystemOwner.Benchmark

        Dim userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)
        Dim user = Await userRepository.GetByIdAsync(z_User)
        u_nem = user?.FullName

        PrepareForm(user)

        chkGracePeriodAsBuffer.Visible = _policy.UseGracePeriodAsBuffer
        chkOvertimeOverride.Visible = _policy.OverrideOvertimeRateEligibility

        previousForm = Me

        Await LoadEmployee()

        paytypestring = EXECQUER("SELECT PayFrequencyType FROM payfrequency pfq LEFT JOIN organization org ON org.PayFrequencyID=pfq.RowID WHERE org.RowID='" & orgztnID & "' LIMIT 1;")

        AddHandler tbpEmployee.Enter, AddressOf tbpEmployee_Enter
        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
    End Sub

    Private Sub PrepareForm(currentUser As AspNetUser)

        If Not _policy.UseUserLevel Then

            CheckRolePermissions()
        End If

        PrepareFormForUserLevelAuthorizations(currentUser)
        PrepareFormForBenchmark()
        PreperateFormForLaGlobal()

        ShowAgencyComboBox()
    End Sub

    Private Sub ShowAgencyComboBox()
        Dim show = _policy.UseAgency
        AgencyLabel.Visible = show
        cboAgency.Visible = show
    End Sub

    Private Sub CheckRolePermissions()

        Dim employeePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.EMPLOYEE).FirstOrDefault()
        Dim salaryPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.SALARY).FirstOrDefault()

        If employeePermission Is Nothing OrElse employeePermission.Read = False Then

            RemoveTab(tbpEmployee)

        End If

        If employeePermission Is Nothing OrElse
            employeePermission.Read = False OrElse
            employeePermission.Update = False Then

            RemoveTab(tbpempchklist)
            RemoveTab(tbpAwards)
            RemoveTab(tbpCertifications)
            RemoveTab(tbpEducBG)
            RemoveTab(tbpPrevEmp)
            RemoveTab(tbpDiscipAct)
            RemoveTab(tbpBonus)
            RemoveTab(tbpAttachment)

        End If

        If salaryPermission Is Nothing OrElse salaryPermission.Read = False Then

            RemoveTab(tbpSalary)
        End If

        If Not _policy.UseBonus Then

            RemoveTab(tbpBonus)
        End If

    End Sub

    Private Sub RemoveTab(page As TabPage)

        If tabctrlemp.TabPages.Contains(page) Then

            tabctrlemp.TabPages.Remove(page)
        End If

    End Sub

    Private Sub PrepareFormForUserLevelAuthorizations(currentUser As AspNetUser)

        If currentUser Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
            Return
        End If

        If Not _policy.UseUserLevel Then Return

        If currentUser.UserLevel = UserLevel.Four OrElse currentUser.UserLevel = UserLevel.Five Then

            RemoveTab(tbpempchklist)
            RemoveTab(tbpAwards)
            RemoveTab(tbpCertifications)
            RemoveTab(tbpEducBG)
            RemoveTab(tbpPrevEmp)
            RemoveTab(tbpDiscipAct)
            RemoveTab(tbpSalary)
            RemoveTab(tbpBonus)
            RemoveTab(tbpAttachment)

        End If

    End Sub

    Private Sub PrepareFormForBenchmark()
        If if_sysowner_is_benchmark Then

            'only salary and employee tabs should be visible

            RemoveTab(tbpempchklist)
            RemoveTab(tbpAwards)
            RemoveTab(tbpCertifications)
            RemoveTab(tbpEducBG)
            RemoveTab(tbpPrevEmp)
            RemoveTab(tbpDiscipAct)
            RemoveTab(tbpBonus)
            RemoveTab(tbpAttachment)

            TabControl3.Visible = False
            LeaveGroupBox.Visible = True

        End If

        If dbnow = Nothing Then
            dbnow = EXECQUER(CURDATE_MDY)
        End If
    End Sub

    Private Sub PreperateFormForLaGlobal()

        If _currentSystemOwner <> SystemOwner.LAGlobal Then
            ActiveEmployeeChecklistReportToolStripMenuItem.Visible = False
            BPIInsuranceAmountReportToolStripMenuItem.Visible = False
            EmploymentContractToolStripMenuItem.Visible = False
            EndOfContractReportToolStripMenuItem.Visible = False
            MonthlyBirthdayReportToolStripMenuItem.Visible = False
            DeploymentEndorsementToolStripMenuItem.Visible = False
            WorkOrderToolStripMenuItem.Visible = False
        End If

        _laGlobalEmployeeReports = New Dictionary(Of String, LaGlobalEmployeeReportName) From {
            {ActiveEmployeeChecklistReportToolStripMenuItem.Name, LaGlobalEmployeeReportName.ActiveEmployeeChecklistReport},
            {BPIInsuranceAmountReportToolStripMenuItem.Name, LaGlobalEmployeeReportName.BpiInsurancePaymentReport},
            {EmploymentContractToolStripMenuItem.Name, LaGlobalEmployeeReportName.EmploymentContractPage},
            {EndOfContractReportToolStripMenuItem.Name, LaGlobalEmployeeReportName.MonthlyEndofContractReport},
            {MonthlyBirthdayReportToolStripMenuItem.Name, LaGlobalEmployeeReportName.MonthlyBirthdayReport},
            {DeploymentEndorsementToolStripMenuItem.Name, LaGlobalEmployeeReportName.SmDeploymentEndorsement},
            {WorkOrderToolStripMenuItem.Name, LaGlobalEmployeeReportName.WorkOrder}}
    End Sub

#Region "Employee Check list"

    Private Async Sub tbpempchklist_Enter(sender As Object, e As EventArgs) Handles tbpempchklist.Enter
        InfoBalloon(, , txtTIN, , , 1)
        InfoBalloon(, , txtPIN, , , 1)
        InfoBalloon(, , txtHDMF, , , 1)
        InfoBalloon(, , txtSSS, , , 1)

        UpdateTabPageText()
        tbpempchklist.Text = "CHECK LIST               "
        Label25.Text = "CHECK LIST"

        imglstchklist.Images.Item(0).Tag = 0
        imglstchklist.Images.Item(1).Tag = 1

        tabIndx = GetCheckListTabPageIndex()

        Await PopulateEmployeeData()
    End Sub

    Dim chkliststring As New AutoCompleteStringCollection

    Sub VIEW_employeechecklist(ByVal emp_rowid As Object)

        'Fix this to handle if there is no employee checklist record.

        Static once As Integer = -1
        Static emp_row_id As String = Nothing
        chkliststring.Clear()
        panelchklist.Controls.Clear()

        If emp_row_id <> -1 Then

            Dim field_count As Integer = 0
            Dim text_indx As Integer = 2

            Try
                If conn.State = ConnectionState.Open Then : conn.Close() : End If

                cmd = New MySqlCommand("VIEW_employeechecklist", conn)
                conn.Open()
                With cmd
                    .Parameters.Clear()
                    .CommandType = CommandType.StoredProcedure

                    .Parameters.AddWithValue("echk_EmployeeID", emp_rowid)
                    .Parameters.AddWithValue("echk_OrganizationID", orgztnID)

                    Dim datread As MySqlDataReader

                    datread = .ExecuteReader()

                    field_count = (datread.FieldCount / 2) - 2

                    If datread.Read Then
                        For i = 0 To field_count
                            chkliststring.Add(datread.GetString(text_indx).ToString & "@" &
                                              datread.GetString(text_indx + 1).ToString)
                            text_indx += 2
                        Next
                    End If

                End With
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            Finally
                conn.Close()
                cmd.Dispose()

                Dim lbllink_text As String = Nothing
                Dim lbllink_imgindx As Integer = 0

                Dim dyna_Y As Integer = 0

                Dim prev_x, prev_y, prev_width As Integer
                prev_x = 3
                prev_y = 6
                prev_width = -1

                Dim ii As Integer = 0

                For Each strval In chkliststring
                    lbllink_text = getStrBetween(strval, "", "@")
                    lbllink_imgindx = CInt(StrReverse(getStrBetween(StrReverse(strval), "", "@")))

                    Dim chklistlinklbl As New LinkLabel

                    With chklistlinklbl
                        .AutoSize = True
                        .Name = "chklistlinklbl_" & ii
                        .Text = lbllink_text
                        .LinkArea = New LinkArea(6, .Text.Length)

                        If dyna_Y < 3 Then
                            .Location = New Point(prev_x, prev_y)
                            prev_x += (.Width + 192)
                        Else
                            dyna_Y = 0
                            prev_y = prev_y + 48
                            prev_x = 3
                            .Location = New Point(prev_x, prev_y)
                            prev_x += (.Width + 192)

                        End If

                        dyna_Y += 1

                        .ImageList = imglstchklist
                        .ImageAlign = ContentAlignment.MiddleLeft
                        .ImageIndex = lbllink_imgindx
                        'Segoe UI Semibold, 9.75pt, style=Bold
                        .Font = New Font("Segoe UI Semibold", 11.0!)
                        .LinkColor = Color.FromArgb(0, 155, 255)
                        'Me.txtEmpID.Font = New Font("Microsoft Sans Serif", 9.75!, FontStyle.Bold)

                        panelchklist.Controls.Add(chklistlinklbl)
                    End With

                    ii += 1
                Next
                chkliststring.Clear()

                If once <> dgvEmp.CurrentRow.Index Then

                    For Each objlbllink As Control In panelchklist.Controls
                        If TypeOf objlbllink Is LinkLabel Then
                            With DirectCast(objlbllink, LinkLabel)
                                RemoveHandler .LinkClicked, AddressOf chklistlinklbl_LinkClicked
                                AddHandler .LinkClicked, AddressOf chklistlinklbl_LinkClicked
                            End With
                        Else
                            Continue For
                        End If
                    Next
                End If
            End Try
        Else

        End If
    End Sub

    Sub chklistlinklbl_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

        Dim link_lablesender As New LinkLabel

        link_lablesender = DirectCast(sender, LinkLabel)

        With link_lablesender
            If .Name = "chklistlinklbl_0" Then 'Performance appraisal = 2
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)
                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "Performance appraisal"
                    ' dgvempatta.Item("Column38", indx).Value = "Performance appraisal"
                End If
            ElseIf .Name = "chklistlinklbl_1" Then 'BIR TIN = 4
                tabctrlemp.SelectedIndex = GetEmployeeProfileTabPageIndex()
                txtTIN.Focus()
                If .ImageIndex = 0 Then
                    InfoBalloon("Please supply this field", Trim(.Text) & " checklist", txtTIN, txtTIN.Width - 16, -69)
                End If

            ElseIf .Name = "chklistlinklbl_2" Then 'Diploma = 6
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "Diploma"
                    ' dgvempatta.Item("Column38", indx).Value = "Diploma"
                End If
            ElseIf .Name = "chklistlinklbl_3" Then 'ID Info slip = 8
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "ID Info slip"
                    ' dgvempatta.Item("Column38", indx).Value = "ID Info slip"
                End If
            ElseIf .Name = "chklistlinklbl_4" Then 'Philhealth ID = 10
                tabctrlemp.SelectedIndex = GetEmployeeProfileTabPageIndex()
                txtPIN.Focus()
                If .ImageIndex = 0 Then
                    InfoBalloon("Please supply this field", Trim(.Text) & " checklist", txtPIN, txtPIN.Width - 16, -70)
                End If
            ElseIf .Name = "chklistlinklbl_5" Then 'HDMF ID = 12
                tabctrlemp.SelectedIndex = GetEmployeeProfileTabPageIndex()
                txtHDMF.Focus()
                If .ImageIndex = 0 Then
                    InfoBalloon("Please supply this field", Trim(.Text) & " checklist", txtHDMF, txtHDMF.Width - 16, -70)
                End If
            ElseIf .Name = "chklistlinklbl_6" Then 'SSS No = 14
                tabctrlemp.SelectedIndex = GetEmployeeProfileTabPageIndex()
                txtSSS.Focus()
                If .ImageIndex = 0 Then
                    InfoBalloon("Please supply this field", Trim(.Text) & " checklist", txtSSS, txtSSS.Width - 16, -70)
                End If
            ElseIf .Name = "chklistlinklbl_7" Then 'Transcript of record = 16
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "Transcript of record"
                    ' dgvempatta.Item("Column38", indx).Value = "Transcript of record"
                End If
            ElseIf .Name = "chklistlinklbl_8" Then 'Birth certificate = 18
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "Birth certificate"
                    ' dgvempatta.Item("Column38", indx).Value = "Birth certificate"
                End If
            ElseIf .Name = "chklistlinklbl_9" Then 'Employee contract = 20
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "Employee contract"
                    ' dgvempatta.Item("Column38", indx).Value = "Employee contract"
                End If
            ElseIf .Name = "chklistlinklbl_10" Then 'Medical exam = 22
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "Medical exam"
                    ' dgvempatta.Item("Column38", indx).Value = "Medical exam"
                End If
            ElseIf .Name = "chklistlinklbl_11" Then 'NBI clearance = 24

                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "NBI clearance"
                    ' dgvempatta.Item("Column38", indx).Value = "NBI clearance"
                End If
            ElseIf .Name = "chklistlinklbl_12" Then 'COE employer = 26
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "COE employer"
                    ' dgvempatta.Item("Column38", indx).Value = "COE employer"
                End If
            ElseIf .Name = "chklistlinklbl_13" Then 'Marriage contract = 28
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "Marriage contract"
                    ' dgvempatta.Item("Column38", indx).Value = "Marriage contract"
                End If
            ElseIf .Name = "chklistlinklbl_14" Then 'House sketch = 30
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "House sketch"
                    ' dgvempatta.Item("Column38", indx).Value = "House sketch"
                End If
            ElseIf .Name = "chklistlinklbl_15" Then '2305 = 32 'Training agreement = 32
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "2305" '"Training agreement"
                    ' dgvempatta.Item("Column38", indx).Value = "2305"
                End If
            ElseIf .Name = "chklistlinklbl_16" Then 'Health permit = 34
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "Health permit"
                    ' dgvempatta.Item("Column38", indx).Value = "Health permit"
                End If
            ElseIf .Name = "chklistlinklbl_17" Then 'SSS loan certificate = 36 'Valid ID = 36
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "SSS loan certificate" '"Valid ID"
                    ' dgvempatta.Item("Column38", indx).Value = "SSS loan certificate"
                End If
            ElseIf .Name = "chklistlinklbl_18" Then 'Resume = 38
                tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    'TODO: check this
                    ' tsbtnNewAtta_Click(sender, e)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = "Resume"
                    ' dgvempatta.Item("Column38", indx).Value = "Resume"
                End If
            Else
                ctrlAttachment(link_lablesender)
            End If
        End With
    End Sub

    Public Function GetCheckListTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpempchklist)
    End Function

    Public Function GetEmployeeProfileTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpEmployee)
    End Function

    Public Function GetAwardsTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpAwards)
    End Function

    Public Function GetCertificationTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpCertifications)
    End Function

    Public Function GetDisciplinaryActionTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpDiscipAct)
    End Function

    Public Function GetEducationalBackgroundTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpEducBG)
    End Function

    Public Function GetPreviousEmployerTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpPrevEmp)
    End Function

    Public Function GetBonusTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpBonus)
    End Function

    Public Function GetAttachmentTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpAttachment)
    End Function

    Public Function GetSalaryTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpSalary)
    End Function

    Sub ctrlAttachment(ByVal lnk_lablesender As LinkLabel)

        If lnk_lablesender IsNot Nothing Then

            tabctrlemp.SelectedIndex = GetAttachmentTabPageIndex()
            tbpAttachment.Focus()

            With lnk_lablesender

                If .ImageIndex = 0 Then

                    'TODO: check this
                    ' tsbtnNewAtta_Click(lnk_lablesender, New EventArgs)

                    ' Dim indx = dgvempatta.CurrentRow.Index

                    ' dgvempatta.Rows.Add()
                    ' dgvempatta.Item("eatt_Type", indx).Selected = True
                    ' dgvempatta.Item("eatt_Type", indx).Value = .Text.Trim
                    ' dgvempatta.Item("Column38", indx).Value = .Text.Trim
                End If
            End With
        End If
    End Sub

#End Region

#Region "Personal Profile"

    Public positn As New AutoCompleteStringCollection
    Dim payFreq As New AutoCompleteStringCollection
    Dim dbnow, u_nem, positID, payFreqID As String
    Dim emp_rcount As Integer

    Public q_empstat As String = "SELECT DisplayValue FROM listofval lov WHERE lov.Type='Employment Status' AND Active='Yes'"

    Public q_emptype As String = "SELECT DisplayValue" &
        " FROM listofval lov" &
        " WHERE lov.`Type`='Employee Type'" &
        " AND lov.Active='Yes'" &
        " AND lov.DisplayValue IN ('Daily','Monthly','Fixed')" &
        " ORDER BY FIELD(lov.DisplayValue,'Daily','Monthly','Fixed');"

    Public q_maritstat As String = "SELECT DisplayValue FROM listofval lov WHERE lov.Type='Marital Status' AND Active='Yes'"

    Dim employeepix As New DataTable

    Private Async Function LoadEmployee() As Task
        Await PopulateEmployeeGrid()
    End Function

    Private Sub Print201Report()
        Dim employeeID = ObjectUtils.ToNullableInteger(publicEmpRowID)

        If employeeID Is Nothing Then

            MessageBoxHelper.Warning("No selected employee.")
            Return

        End If

        Dim provider = New Employee201ReportProvider(employeeID)
        provider.Run()
    End Sub

    Dim empBDate As String

    Private Async Sub INSUPD_employee_01(sender As Object, e As EventArgs) Handles tsbtnSaveEmp.Click

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        Dim isNew = tsbtnNewEmp.Enabled = False

        If (isNew AndAlso Not _currentRolePermission.Create) OrElse
                (Not isNew AndAlso Not _currentRolePermission.Update) Then

            MessageBoxHelper.DefaultUnauthorizedActionMessage()
            Return
        End If

        If (Not isNew AndAlso
            (dgvEmp.CurrentRow Is Nothing OrElse
            String.IsNullOrWhiteSpace(dgvEmp.CurrentRow.Cells("RowID").Value))) Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            WarnBalloon("Please select an employee to update.", "No employee selected", lblforballoon, 0, -69)
            Return

        ElseIf (isNew AndAlso
        EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE EmployeeID='" &
            Trim(txtEmpID.Text) & "' AND OrganizationID=" & orgztnID & ");") = 1) OrElse
       (Not isNew AndAlso
        EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE EmployeeID='" &
            Trim(txtEmpID.Text) & "' AND OrganizationID=" & orgztnID & " AND RowID <> " & dgvEmp.CurrentRow.Cells("RowID").Value & "); ") = 1) Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            WarnBalloon("Employee ID has already exist.", "Invalid employee ID", lblforballoon, 0, -69)
            Exit Sub
        ElseIf Trim(txtEmpID.Text) = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            txtEmpID.Focus()
            WarnBalloon("Please input an employee ID.", "Invalid employee ID", lblforballoon, 0, -69)
            Exit Sub
        ElseIf cboEmpType.Text = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            cboEmpType.Focus()
            WarnBalloon("Please select an employee type.", "Invalid employee type", lblforballoon, 0, -69)
            Exit Sub
        ElseIf Trim(cboEmpStat.Text) = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            cboEmpStat.Focus()
            WarnBalloon("Please select an employee status.", "Invalid employee status", lblforballoon, 0, -69)
            Exit Sub
        ElseIf Trim(txtFName.Text) = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            txtFName.Focus()
            WarnBalloon("Please input first name.", "Invalid first name", lblforballoon, 0, -69)
            Exit Sub
        ElseIf Trim(txtLName.Text) = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            txtLName.Focus()
            WarnBalloon("Please input last name.", "Invalid last name", lblforballoon, 0, -69)
            Exit Sub
        ElseIf Trim(cboMaritStat.Text) = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            cboMaritStat.Focus()
            WarnBalloon("Please select a marital status.", "Invalid marital status", lblforballoon, 0, -69)
            Exit Sub
        ElseIf Trim(cboPosit.Text) = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            cboPosit.Focus()
            WarnBalloon("Please select a position.", "Invalid Position", lblforballoon, 0, -69)
            Exit Sub
        ElseIf String.IsNullOrWhiteSpace(txtWorkDaysPerYear.Text) OrElse ObjectUtils.ToDecimal(txtWorkDaysPerYear.Text) <= 0 Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            txtWorkDaysPerYear.Focus()
            WarnBalloon("Please input a valid work days per year.", "Invalid work days per year", lblforballoon, 0, -69)
            Exit Sub

        ElseIf rdbDirectDepo.Checked Then

            If txtATM.Text.Trim = String.Empty Then
                AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
                txtATM.Focus()
                WarnBalloon("Please input an ATM No.", "Invalid ATM No.", lblforballoon, 0, -69)
                Exit Sub
            ElseIf cbobank.Text.Trim = String.Empty Then
                AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
                cbobank.Focus()
                WarnBalloon("Please input a Bank Name", "Invalid Bank Name", lblforballoon, 0, -69)
                Exit Sub
            End If

        ElseIf positID = "" Then

            'this fucking elseif block skips over the next elseif
            'don't put another elseif at the bottom

            positID = EXECQUER("SELECT RowID FROM position WHERE PositionName='" & cboPosit.Text & "' AND OrganizationID='" & orgztnID & "';")

            If positID = "" Then

                AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
                cboPosit.Focus()
                WarnBalloon("Please input employee position.", "Invalid Position", lblforballoon, 0, -69)
                Exit Sub
            End If
        End If

        Dim employee_RowID = Nothing

        If isNew Then
            employee_RowID = DBNull.Value
        Else
            If dgvEmp.RowCount <> 0 Then
                employee_RowID = dgvEmp.CurrentRow.Cells("RowID").Value
            Else
                employee_RowID = DBNull.Value
            End If
        End If

        Dim image_object = Nothing

        If File.Exists(Path.GetTempPath & "tmpfileEmployeeImage.jpg") Then 'pbemppic.Image = Nothing
            image_object = convertFileToByte(Path.GetTempPath & "tmpfileEmployeeImage.jpg") 'ang gawin mo from image, convert into Byte()
        ElseIf empPic = "" Then
            image_object = DBNull.Value
        ElseIf empPic <> "" Then
            If File.Exists(Path.GetTempPath & "tmpfileEmployeeImage.jpg") Then
                image_object = convertFileToByte(empPic)
            Else
                image_object = DBNull.Value
            End If
        End If
        Dim null_index() As Integer = {-1, 0}
        Dim new_eRowID = Nothing
        Dim oldEmployee As Employee = Nothing

        Dim succeed As Boolean = False
        Try
            Dim employee_restday = If(null_index.Contains(cboDayOfRest.SelectedIndex), DBNull.Value, cboDayOfRest.SelectedIndex)

            Dim agensi_rowid = If(String.IsNullOrWhiteSpace(cboAgency.SelectedValue), DBNull.Value, cboAgency.SelectedValue)
            positID = cboPosit.SelectedValue

            Dim regularizationDate = If(dtpRegularizationDate.Checked, dtpRegularizationDate.Value, DBNull.Value)
            Dim evaluationDate = If(dtpEvaluationDate.Checked, dtpEvaluationDate.Value, DBNull.Value)

            If Not isNew Then 'Means update and oldEmployee is needed for UserActivity
                oldEmployee = Await GetOldEmployee(employee_RowID)

            End If

            new_eRowID =
            INSUPDemployee(employee_RowID,
                           z_User,
                           orgztnID,
                           cboSalut.Text.Trim,
                           txtFName.Text.Trim,
                           txtMName.Text.Trim,
                           txtLName.Text.Trim,
                           String.Empty,
                           txtEmpID.Text.Trim,
                           txtTIN.Text.Trim,
                           txtSSS.Text.Trim,
                           txtHDMF.Text.Trim,
                           txtPIN.Text.Trim,
                           cboEmpStat.Text.Trim,
                           txtemail.Text.Trim,
                           txtWorkPhne.Text.Trim,
                           txtHomePhne.Text.Trim,
                           txtMobPhne.Text.Trim,
                           txtHomeAddr.Text.Trim,
                           txtNName.Text.Trim,
                           String.Empty,
                           If(rdMale.Checked, "M", "F"),
                           cboEmpType.Text.Trim,
                           cboMaritStat.Text.Trim,
                           Format(CDate(dtpempbdate.Value), "yyyy-MM-dd"),
                           Format(CDate(dtpempstartdate.Value), "yyyy-MM-dd"),
                           DBNull.Value,
                           positID,
                           image_object,
                           ValNoComma(txtvlbal.Text),
                           ValNoComma(txtslbal.Text),
                           ValNoComma(txtmlbal.Text),
                           ValNoComma(txtothrbal.Text),
                           ValNoComma(txtvlallow.Text),
                           ValNoComma(txtslallow.Text),
                           ValNoComma(txtmlallow.Text),
                           ValNoComma(txtothrallow.Text),
                           "0",
                           ValNoComma(txtWorkDaysPerYear.Text),
                           employee_restday,
                           txtATM.Text.Trim,
                           cbobank.Text,
                           Convert.ToInt16(chkcalcHoliday.Checked),
                           Convert.ToInt16(chkcalcSpclHoliday.Checked),
                           Convert.ToInt16(chkcalcNightDiff.Checked),
                           True,
                           Convert.ToInt16(chkcalcRestDay.Checked),
                           True,
                           regularizationDate,
                           evaluationDate,
                           False,
                           ValNoComma(txtUTgrace.Text),
                           agensi_rowid,
                           BranchComboBox.SelectedValue,
                           ValNoComma(BPIinsuranceText.Text),
                           chkGracePeriodAsBuffer.Checked,
                           chkOvertimeOverride.Checked)
            succeed = new_eRowID IsNot Nothing

            Dim employeeId = If(isNew, new_eRowID, employee_RowID)

            'this is during edit
            If if_sysowner_is_benchmark AndAlso employeeId IsNot Nothing Then

                Dim leaveService = MainServiceProvider.GetRequiredService(Of ILeaveDataService)
                Dim newleaveBalance = Await leaveService.
                    ForceUpdateLeaveAllowanceAsync(
                        employeeId:=employeeId,
                        organizationId:=z_OrganizationID,
                        userId:=z_User,
                        selectedLeaveType:=LeaveType.Vacation,
                        newAllowance:=LeaveAllowanceTextBox.Text.ToDecimal)

                LeaveBalanceTextBox.Text = newleaveBalance.ToString("#0.00")
            End If
        Catch ex As Exception
            succeed = False
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

        Dim dgvEmp_RowIndex = 0
        If isNew Then 'INSERT employee

            employee_RowID = new_eRowID

            Dim new_drow As DataRow
            new_drow = employeepix.NewRow
            new_drow("RowID") = employee_RowID
            new_drow("Image") = image_object
            employeepix.Rows.Add(new_drow)

            If dgvEmp.RowCount = 0 Then
                dgvEmp.Rows.Add()
            Else : dgvEmp.Rows.Insert(0, 1)

            End If

            emp_rcount += 1
            dgvEmp_RowIndex = 0
            If succeed Then

                Dim repo = MainServiceProvider.GetRequiredService(Of IUserActivityRepository)
                Await repo.RecordAddAsync(
                    z_User,
                    EmployeeEntityName,
                    entityId:=employee_RowID,
                    organizationId:=z_OrganizationID,
                    changedEmployeeId:=employee_RowID)
                InfoBalloon("Employee ID '" & txtEmpID.Text & "' has been created successfully.", "New Employee successfully created", lblforballoon, 0, -69, , 5000)

            End If
        Else 'UPDATE employee

            If dgvEmp.CurrentRow Is Nothing Then Exit Sub

            For Each drow As DataRow In employeepix.Rows
                If drow("RowID").ToString = dgvEmp.CurrentRow.Cells("RowID").Value Then
                    drow("Image") = Nothing
                    drow("Image") = image_object

                    Exit For
                End If
            Next

            dgvEmp_RowIndex = dgvEmp.CurrentRow.Index

            If succeed Then
                Await RecordUpdateEmployee(oldEmployee)
                InfoBalloon("Employee ID '" & txtEmpID.Text & "' has been updated successfully.", "Employee Update Successful", lblforballoon, 0, -69)
            End If

        End If

        With dgvEmp.Rows(dgvEmp_RowIndex)

            If isNew Then

                .Cells("RowID").Value = employee_RowID

            End If

            .Cells("Column1").Value = txtEmpID.Text : .Cells("Column2").Value = txtFName.Text
            .Cells("Column3").Value = txtMName.Text : .Cells("Column4").Value = txtLName.Text
            .Cells("Column5").Value = txtNName.Text
            .Cells("Column6").Value = Format(dtpempbdate.Value, machineShortDateFormat) 'dtpBDate.Value

            .Cells("Column8").Value =
                If(cboPosit.SelectedIndex = -1, "",
                    If(String.IsNullOrWhiteSpace(cboPosit.Text), Nothing, Trim(cboPosit.Text)))

            .Cells("Column9").Value = cboSalut.Text : .Cells("Column10").Value = txtTIN.Text
            .Cells("Column11").Value = txtSSS.Text : .Cells("Column12").Value = txtHDMF.Text
            .Cells("Column13").Value = txtPIN.Text : .Cells("Column15").Value = txtWorkPhne.Text
            .Cells("Column16").Value = txtHomePhne.Text : .Cells("Column17").Value = txtMobPhne.Text
            .Cells("Column18").Value = txtHomeAddr.Text : .Cells("Column14").Value = txtemail.Text
            .Cells("Column19").Value = If(rdMale.Checked, "Male", "Female")
            .Cells("Column20").Value = cboEmpStat.Text
            .Cells("Column25").Value = dbnow : .Cells("Column26").Value = u_nem

            .Cells("Column29").Value = cboPosit.SelectedValue

            .Cells("Column31").Value = cboMaritStat.Text
            .Cells("Column34").Value = cboEmpType.Text

            .Cells("colstartdate").Value = dtpempstartdate.Value

            If if_sysowner_is_benchmark Then

                .Cells("Column36").Value = LeaveAllowanceTextBox.Text
                .Cells("Column35").Value = LeaveBalanceTextBox.Text
            Else

                .Cells("Column36").Value = txtvlallow.Text
                .Cells("Column35").Value = txtvlbal.Text

            End If

            .Cells("slallowance").Value = txtslallow.Text
            .Cells("mlallowance").Value = txtmlallow.Text

            .Cells("slbalance").Value = txtslbal.Text
            .Cells("mlbalance").Value = txtmlbal.Text

            .Cells("Column1").Selected = True

            .Cells("WorkDaysPerYear").Value = txtWorkDaysPerYear.Text

            .Cells("DayOfRest").Value = cboDayOfRest.Text

            .Cells("ATMNo").Value = txtATM.Text

            .Cells("BankName").Value = cbobank.Text

            .Cells("OtherLeaveAllowance").Value = txtothrallow.Text

            .Cells("OtherLeaveBalance").Value = txtothrbal.Text

            .Cells("CalcHoliday").Value = Convert.ToInt16(chkcalcHoliday.Checked)
            .Cells("CalcSpecialHoliday").Value = Convert.ToInt16(chkcalcSpclHoliday.Checked)
            .Cells("CalcNightDiff").Value = Convert.ToInt16(chkcalcNightDiff.Checked)
            .Cells("CalcRestDay").Value = Convert.ToInt16(chkcalcRestDay.Checked)

            .Cells("LateGracePeriod").Value = txtUTgrace.Text
            .Cells(GracePeriodAsBuffer.Name).Value = Convert.ToInt16(chkGracePeriodAsBuffer.Checked)
            .Cells(OvertimeOverride.Name).Value = Convert.ToInt16(chkOvertimeOverride.Checked)
            .Cells("AgencyName").Value = cboAgency.Text

            .Cells("BranchID").Value = BranchComboBox.SelectedValue
            .Cells("BPIInsuranceColumn").Value = BPIinsuranceText.Text

            Await SetEmployeeGridDataRow(dgvEmp_RowIndex)

        End With
        tsbtnNewEmp.Enabled = True

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        tsbtnSaveEmp.Enabled = True
    End Sub

    Private Shared Async Function GetOldEmployee(employee_RowID As Integer?) As Task(Of Employee)

        If employee_RowID.HasValue = False Then Return Nothing

        Dim employeeBuilder = MainServiceProvider.GetRequiredService(Of IEmployeeQueryBuilder)

        Return Await employeeBuilder.
            IncludePayFrequency().
            IncludePosition().
            IncludeBranch().
            IncludeAgency().
            GetByIdAsync(employee_RowID.Value, z_OrganizationID)

    End Function

    Private Async Function RecordUpdateEmployee(oldEmployee As Employee) As Task(Of Boolean)

        If oldEmployee Is Nothing Then Return False

        Dim changes = New List(Of UserActivityItem)

        Dim gender = Nothing
        If rdFMale.Checked Then
            gender = "F"
        ElseIf rdMale.Checked Then
            gender = "M"
        End If

        If oldEmployee.EmployeeNo <> txtEmpID.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated employee ID from '{oldEmployee.EmployeeNo}' to '{txtEmpID.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.EmployeeType <> cboEmpType.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated employee type from '{oldEmployee.EmployeeType}' to '{cboEmpType.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.EmploymentStatus <> cboEmpStat.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated employee status from '{oldEmployee.EmploymentStatus}' to '{cboEmpStat.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.StartDate <> dtpempstartdate.Value Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated start date from '{oldEmployee.StartDate.ToShortDateString}' to '{dtpempstartdate.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.Salutation <> cboSalut.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated salutation from '{oldEmployee.Salutation}' to '{cboSalut.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.Gender <> gender Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated gender from '{oldEmployee.Gender}' to '{gender}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.FirstName <> txtFName.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated first name from '{oldEmployee.FirstName}' to '{txtFName.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.MiddleName <> txtMName.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated middle name from '{oldEmployee.MiddleName}' to '{txtMName.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.LastName <> txtLName.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated last name from '{oldEmployee.LastName}' to '{txtLName.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.Nickname <> txtNName.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated nickname from '{oldEmployee.Nickname}' to '{txtNName.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.MaritalStatus <> cboMaritStat.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated marital status from '{oldEmployee.MaritalStatus}' to '{cboMaritStat.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.DayOfRest Is Nothing And cboDayOfRest.Text <> "" Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated rest day from '' to  '{cboDayOfRest.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        ElseIf oldEmployee.DayOfRest IsNot Nothing And cboDayOfRest.Text <> "" Then
            If WeekdayName(oldEmployee.DayOfRest, False, FirstDayOfWeek.Sunday) <> cboDayOfRest.Text Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = oldEmployee.RowID,
                    .Description = $"Updated rest day from '{WeekdayName(oldEmployee.DayOfRest, False, FirstDayOfWeek.Sunday)}' to '{cboDayOfRest.Text}' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
            End If
        ElseIf oldEmployee.DayOfRest IsNot Nothing And cboDayOfRest.Text = "" Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated rest day from '{WeekdayName(oldEmployee.DayOfRest, False, FirstDayOfWeek.Sunday)}' to '' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.DateEvaluated <> dtpEvaluationDate.Value Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated evaluation date from '{oldEmployee.DateEvaluated?.ToShortDateString}' to '{dtpEvaluationDate.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If (oldEmployee.DateEvaluated Is Nothing And dtpEvaluationDate.Checked) Or
            (oldEmployee.DateEvaluated IsNot Nothing And dtpEvaluationDate.Checked = False) Then
            If dtpEvaluationDate.Checked = False Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = oldEmployee.RowID,
                    .Description = $"Updated evaluation date from '{oldEmployee.DateEvaluated?.ToShortDateString}' to '' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
            Else
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = oldEmployee.RowID,
                    .Description = $"Updated evaluation date from '' to '{dtpEvaluationDate.Text}' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
            End If
        End If
        If oldEmployee.DateRegularized <> dtpRegularizationDate.Value Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated regularization date from '{oldEmployee.DateRegularized?.ToShortDateString}' to '{dtpRegularizationDate.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If (oldEmployee.DateRegularized Is Nothing And dtpRegularizationDate.Checked) Or
            (oldEmployee.DateRegularized IsNot Nothing And dtpRegularizationDate.Checked = False) Then
            If dtpRegularizationDate.Checked = False Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = oldEmployee.RowID,
                    .Description = $"Updated regularization date from '{oldEmployee.DateRegularized?.ToShortDateString}' to '' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
            Else
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = oldEmployee.RowID,
                    .Description = $"Updated regularization date from '' to '{dtpRegularizationDate.Text}' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
            End If
        End If
        If oldEmployee.AtmNo Is Nothing And txtATM.Text <> "" Then 'change to deposit
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated salary distribution from 'Cash / Check' to 'Direct Deposit' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated ATM number/account number from '' to '{txtATM.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated bank from '' to '{cbobank.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })

        ElseIf oldEmployee.AtmNo <> Nothing And txtATM.Text Is Nothing Then ' change to cash / check
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated salary distribution from 'Direct Deposit' to 'Cash / Check' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        Else
            If oldEmployee.AtmNo <> txtATM.Text Then 'change ATM number and Bank Name
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = oldEmployee.RowID,
                    .Description = $"Updated ATM number/account number from '{oldEmployee.AtmNo}' to '{txtATM.Text}' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
            End If
            If oldEmployee.BankName <> cbobank.Text Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = oldEmployee.RowID,
                    .Description = $"Updated bank from '{oldEmployee.BankName}' to '{cbobank.Text}' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
            End If
        End If

        If Not Nullable.Equals(oldEmployee.BranchID, ObjectUtils.ToNullableInteger(BranchComboBox.SelectedValue)) Then

            Dim oldBranch = _branches.FirstOrDefault(Function(b) Nullable.Equals(b.RowID, oldEmployee.BranchID))
            Dim newBranch = _branches.FirstOrDefault(Function(b) Nullable.Equals(b.RowID, ObjectUtils.ToNullableInteger(BranchComboBox.SelectedValue)))

            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated branch from '{oldBranch?.Name}' to '{newBranch?.Name}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })

        End If
        If _policy.UseBPIInsurance AndAlso oldEmployee.BPIInsurance <> BPIinsuranceText.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated BPI insurance from '{oldEmployee.BPIInsurance.ToString}' to '{BPIinsuranceText.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.Position?.Name <> cboPosit.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated position from '{oldEmployee.Position?.Name}' to '{cboPosit.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If _policy.UseAgency AndAlso oldEmployee.Agency?.Name <> cboAgency.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated employee agency from '{oldEmployee.Agency?.Name}' to '{cboAgency.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.BirthDate <> dtpempbdate.Value Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated birthday from '{oldEmployee.BirthDate.ToShortDateString}' to '{dtpempbdate.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.EmailAddress <> txtemail.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated email address from '{oldEmployee.EmailAddress}' to '{txtemail.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.TinNo <> txtTIN.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated TIN from '{oldEmployee.TinNo}' to '{txtTIN.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.SssNo <> txtSSS.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated SSS from '{oldEmployee.SssNo}' to '{txtSSS.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.PhilHealthNo <> txtPIN.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated PhilHealth from '{oldEmployee.PhilHealthNo}' to '{txtPIN.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.HdmfNo <> txtHDMF.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated PAGIBIG from '{oldEmployee.HdmfNo}' to '{txtHDMF.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.HomeAddress <> txtHomeAddr.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated home address from '{oldEmployee.HomeAddress}' to '{txtHomeAddr.Text}' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
        End If
        If oldEmployee.WorkPhone <> txtWorkPhne.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated work phone from '{oldEmployee.WorkPhone}' to '{txtWorkPhne.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.HomePhone <> txtHomePhne.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated home phone from '{oldEmployee.HomePhone}' to '{txtHomePhne.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.MobilePhone <> txtMobPhne.Text Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated mobile phone from '{oldEmployee.MobilePhone}' to '{txtMobPhne.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.LateGracePeriod <> txtUTgrace.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated grace period from '{oldEmployee.LateGracePeriod}' to '{txtUTgrace.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If _policy.UseGracePeriodAsBuffer AndAlso oldEmployee.GracePeriodAsBuffer <> chkGracePeriodAsBuffer.Checked Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated `GracePeriodAsBuffer` from '{oldEmployee.GracePeriodAsBuffer}' to '{chkGracePeriodAsBuffer.Checked}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.WorkDaysPerYear <> txtWorkDaysPerYear.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated work days per year from '{oldEmployee.WorkDaysPerYear}' to '{txtWorkDaysPerYear.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.CalcHoliday <> chkcalcHoliday.Checked Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated calculate holiday from '{oldEmployee.CalcHoliday}' to '{chkcalcHoliday.Checked}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.CalcSpecialHoliday <> chkcalcSpclHoliday.Checked Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated calculate special holiday from '{oldEmployee.CalcSpecialHoliday}' to '{chkcalcSpclHoliday.Checked}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.CalcNightDiff <> chkcalcNightDiff.Checked Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated calculate night differential from '{oldEmployee.CalcNightDiff}' to '{chkcalcNightDiff.Checked}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.CalcRestDay <> chkcalcRestDay.Checked Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated calculate rest day from '{oldEmployee.CalcRestDay}' to '{chkcalcRestDay.Checked}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.VacationLeaveAllowance <> txtvlallow.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated vacation leave allowance from '{oldEmployee.VacationLeaveAllowance:#0}' to '{txtvlallow.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.SickLeaveAllowance <> txtslallow.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated sick leave allowance from '{oldEmployee.SickLeaveAllowance}' to '{txtslallow.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If oldEmployee.MaternityLeaveAllowance <> txtmlallow.Text.ToDecimal Then
            If gender = "F" Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = oldEmployee.RowID,
                    .Description = $"Updated maternity leave allowance from '{oldEmployee.MaternityLeaveAllowance}' to '{txtmlallow.Text}' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
            ElseIf gender = "M" Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = oldEmployee.RowID,
                    .Description = $"Updated paternity leave allowance from '{oldEmployee.MaternityLeaveAllowance}' to '{txtmlallow.Text}' of employee.",
                    .ChangedEmployeeId = oldEmployee.RowID.Value
                })
            End If
        End If
        If oldEmployee.OtherLeaveAllowance <> txtothrallow.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated other leave allowance from '{oldEmployee.OtherLeaveAllowance}' to '{txtothrallow.Text}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If
        If _policy.OverrideOvertimeRateEligibility AndAlso oldEmployee.OvertimeOverride <> chkOvertimeOverride.Checked Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldEmployee.RowID,
                .Description = $"Updated `OvertimeRateEligibility` from '{oldEmployee.OvertimeOverride}' to '{chkOvertimeOverride.Checked}' of employee.",
                .ChangedEmployeeId = oldEmployee.RowID.Value
            })
        End If

        If changes.Any() Then
            Dim repo = MainServiceProvider.GetRequiredService(Of IUserActivityRepository)
            Await repo.CreateRecordAsync(
                z_User,
                EmployeeEntityName,
                z_OrganizationID,
                UserActivity.RecordTypeEdit,
                changes)
            Return True
        End If

        Return False
    End Function

    Private Async Function SetEmployeeGridDataRow(rowIndex As Integer) As Task
        Dim gridRow = dgvEmp.Rows(rowIndex)
        Using command = New MySqlCommand("CALL `SEARCH_employeeprofile`(@organizationID, @employeeID, '', '', 0);",
                                         New MySqlConnection(mysql_conn_text))
            With command.Parameters
                .AddWithValue("@organizationID", orgztnID)
                .AddWithValue("@employeeID", gridRow.Cells(Column1.Name).Value)
            End With

            Await command.Connection.OpenAsync()

            Dim da As New MySqlDataAdapter
            da.SelectCommand = command

            Dim ds As New DataSet
            da.Fill(ds)

            Dim dt = ds.Tables.OfType(Of DataTable).FirstOrDefault

            If dt IsNot Nothing Then dgvEmp.Rows(rowIndex).Tag = dt.Rows.OfType(Of DataRow).FirstOrDefault
        End Using
    End Function

    Private Sub Employee_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = False

        RemoveHandler txtBDate.Leave, AddressOf txtBDate_Leave

        InfoBalloon(, , lblforballoon, , , 1)
        WarnBalloon(, , lblforballoon, , , 1)

        WarnBalloon(, , txtEmpID, , , 1)
        WarnBalloon(, , txtFName, , , 1)
        WarnBalloon(, , txtLName, , , 1)
        WarnBalloon(, , cboattatype, , , 1)

        myBalloon(, , lblforballoon, , , 1)
        InfoBalloon(, , Label235, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        HRISForm.listHRISForm.Remove(Me.Name)

        Dim liveThreads = threadArrayList.Cast(Of Thread).Where(Function(i) i.IsAlive)

        For Each ilist As Thread In liveThreads
            'If ilist.IsAlive Then
            ilist.Abort()
            'End If
        Next

        threadArrayList.Clear()

    End Sub

    Private Sub txtBDate_Leave(sender As Object, e As EventArgs)
        Throw New NotImplementedException()
    End Sub

    Dim dt_cboPosit As New DataTable

    Sub loadPositName()

        Dim str_quer_positions As String =
            String.Concat("SELECT pos.RowID",
                          ",pos.PositionName",
                          " FROM `position` pos",
                          " INNER JOIN division dv ON dv.RowID=pos.DivisionId AND dv.OrganizationID=pos.OrganizationID",
                          " WHERE pos.OrganizationID=", orgztnID,
                          " AND LENGTH(TRIM(pos.PositionName)) > 0",
                          " ORDER BY pos.PositionName;")

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(str_quer_positions)

        Static once As SByte = 0
        If once = 0 Then
            once = 1
            cboPosit.ValueMember = n_SQLQueryToDatatable.ResultTable.Columns(0).ColumnName
            cboPosit.DisplayMember = n_SQLQueryToDatatable.ResultTable.Columns(1).ColumnName
        End If
        n_SQLQueryToDatatable.ResultTable.DefaultView.Sort = cboPosit.DisplayMember + " ASC"
        dt_cboPosit = n_SQLQueryToDatatable.ResultTable.DefaultView.ToTable

        cboPosit.DataSource = dt_cboPosit
    End Sub

    Sub reloadPositName(ByVal e_positID As String)

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(
            "SELECT RowID,PositionName" &
            " FROM position" &
            " WHERE OrganizationID=" & orgztnID &
            " AND RowID!='" & String.Empty & "';")

        Static once As SByte = 0
        If once = 0 Then
            once = 1
            cboPosit.ValueMember = n_SQLQueryToDatatable.ResultTable.Columns(0).ColumnName
            cboPosit.DisplayMember = n_SQLQueryToDatatable.ResultTable.Columns(1).ColumnName
        End If
        n_SQLQueryToDatatable.ResultTable.DefaultView.Sort = cboPosit.DisplayMember + " ASC"
        dt_cboPosit = n_SQLQueryToDatatable.ResultTable.DefaultView.ToTable

        cboPosit.DataSource = dt_cboPosit
    End Sub

    Private Sub Employee_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd

        InfoBalloon(, , lblforballoon, , , 1)
        WarnBalloon(, , lblforballoon, , , 1)

        myBalloon(, , lblforballoon, , , 1)

        Select Case tabIndx
            Case GetEmployeeProfileTabPageIndex()
                WarnBalloon(, , txtEmpID, , , 1)
                WarnBalloon(, , txtFName, , , 1)
                WarnBalloon(, , txtLName, , , 1)
            Case GetAttachmentTabPageIndex()
                WarnBalloon(, , cboattatype, , , 1)
        End Select
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles _
        tsbtnClose.Click, tsbtnCloseempawar.Click, tsbtnCloseempcert.Click,
        btnClose.Click, ToolStripButton5.Click, ToolStripButton13.Click,
        ToolStripButton18.Click,
        ToolStripButton2.Click,
        ToolStripButton11.Click,
        ToolStripButton16.Click

        Me.Close()
    End Sub

    Private Async Sub cboPosit_SelectedIndexChanged_1Async(sender As Object, e As EventArgs) Handles cboPosit.SelectedIndexChanged
        Dim divisionName As String = Await GetDivisionName(cboPosit.Text)
        txtDivisionName.Text = divisionName
    End Sub

    Private Shared Async Function GetDivisionName(positionName As String) As Task(Of String)
        Dim divisionName = String.Empty

        Dim repository = MainServiceProvider.GetRequiredService(Of IPositionRepository)

        Dim position = Await repository.GetByNameWithDivisionAsync(z_OrganizationID, positionName)
        If position?.Division IsNot Nothing Then
            divisionName = position.Division.Name
        End If

        Return divisionName
    End Function

    Dim noCurrCellChange As SByte
    Dim EmployeeImage As Image

    Dim employeefullname As String = Nothing
    Dim subdetails As String = Nothing
    Dim sameEmpID As Integer = -1
    Dim LastFirstMidName As String = Nothing
    Dim publicEmpRowID = Nothing

    Async Sub dgvEmp_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvEmp.SelectionChanged

        Await PopulateEmployeeData()
    End Sub

    Public Async Function PopulateEmployeeData() As Task
        RemoveHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged

        If tsbtnNewEmp.Enabled = 0 Then
            cboEmpStat.Enabled = 1
            tsbtnNewEmp.Enabled = 1
        End If

        publicEmpRowID = String.Empty

        If dgvEmp.RowCount <> 0 Then
            With dgvEmp.CurrentRow
                Dim empPix() As DataRow
                publicEmpRowID = .Cells("RowID").Value
                If IsDBNull(.Cells("RowID").Value) = False AndAlso sameEmpID <> .Cells("RowID").Value Then

                    sameEmpID = .Cells("RowID").Value

                    empPix = employeepix.Select("RowID=" & .Cells("RowID").Value)
                    For Each drow In empPix
                        If IsDBNull(drow("Image")) = 0 Then
                            EmployeeImage = ConvByteToImage(DirectCast(drow("Image"), Byte()))
                            makefileGetPath(drow("Image"))
                        Else
                            EmployeeImage = Nothing
                        End If
                    Next

                    txtFName.Text = If(IsDBNull(.Cells("Column2").Value), "", .Cells("Column2").Value)
                    txtMName.Text = If(IsDBNull(.Cells("Column3").Value), "", .Cells("Column3").Value)
                    txtLName.Text = If(IsDBNull(.Cells("Column4").Value), "", .Cells("Column4").Value)

                    employeefullname = If(IsDBNull(.Cells("Column2").Value), "", .Cells("Column2").Value)

                    Dim addtlWord = Nothing

                    If IsDBNull(.Cells("Column3").Value) OrElse .Cells("Column3").Value Is Nothing OrElse .Cells("Column3").Value Is Nothing Then
                    Else

                        Dim midNameTwoWords = Split(If(IsDBNull(.Cells("Column3").Value), "", .Cells("Column3").Value).ToString, " ")
                        addtlWord = " "
                        For Each s In midNameTwoWords
                            addtlWord &= (StrConv(Microsoft.VisualBasic.Left(s, 1), VbStrConv.ProperCase) & ".")
                        Next
                    End If

                    employeefullname = employeefullname & addtlWord
                    employeefullname = employeefullname & " " & If(IsDBNull(.Cells("Column4").Value), "", .Cells("Column4").Value)
                    '
                    LastFirstMidName = If(IsDBNull(.Cells("Column4").Value), "", .Cells("Column4").Value) & ", " & If(IsDBNull(.Cells("Column2").Value), "", .Cells("Column2").Value) &
                        If(Trim(addtlWord) Is Nothing, "", If(Trim(addtlWord) = ".", "", ", " & addtlWord))

                    subdetails = "ID# " & .Cells("Column1").Value &
                                If(.Cells("Column8").Value Is Nothing,
                                                                   "",
                                                                   ", " & .Cells("Column8").Value) &
                                If(.Cells("Column34").Value Is Nothing,
                                                                   "",
                                                                   ", " & .Cells("Column34").Value & " salary")

                End If

                Dim selectedTab = tabctrlemp.SelectedTab
                Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                Dim employee = Await GetCurrentEmployeeEntity(employeeID)

                If selectedTab Is tbpempchklist Then

                    txtEmpIDChk.Text = subdetails '"ID# " & .Cells("Column1").Value

                    txtFNameChk.Text = employeefullname
                    pbEmpPicChk.Image = Nothing
                    pbEmpPicChk.Image = EmployeeImage
                    lblyourrequirement.Text = .Cells("Column2").Value & "'s requirements"
                    VIEW_employeechecklist(.Cells("RowID").Value)

                ElseIf selectedTab Is tbpEmployee Then 'Employee

                    Await SetEmployee()

                ElseIf selectedTab Is tbpAwards Then

                    RemoveHandler tbpAwards.Enter, AddressOf tbpAwards_Enter
                    Await AwardTab.SetEmployee(employee)
                    AddHandler tbpAwards.Enter, AddressOf tbpAwards_Enter

                ElseIf selectedTab Is tbpCertifications Then

                    RemoveHandler tbpCertifications.Enter, AddressOf tbpCertifications_Enter
                    Await CertificationTab.SetEmployee(employee)
                    AddHandler tbpCertifications.Enter, AddressOf tbpCertifications_Enter

                ElseIf selectedTab Is tbpDiscipAct Then

                    RemoveHandler tbpDiscipAct.Enter, AddressOf tbpDiscipAct_Enter
                    Await DisciplinaryActionTab.SetEmployee(employee)
                    AddHandler tbpDiscipAct.Enter, AddressOf tbpDiscipAct_Enter

                ElseIf selectedTab Is tbpEducBG Then

                    RemoveHandler tbpEducBG.Enter, AddressOf tbpEducBG_Enter
                    Await EducationalBackgroundTab.SetEmployee(employee)
                    AddHandler tbpEducBG.Enter, AddressOf tbpEducBG_Enter

                ElseIf selectedTab Is tbpPrevEmp Then

                    RemoveHandler tbpPrevEmp.Enter, AddressOf tbpPrevEmp_Enter
                    Await PreviousEmployerTab.SetEmployee(employee)
                    AddHandler tbpPrevEmp.Enter, AddressOf tbpPrevEmp_Enter

                ElseIf selectedTab Is tbpBonus Then

                    RemoveHandler tbpBonus.Enter, AddressOf tbpBonus_Enter
                    Await BonusTab.SetEmployee(employee)
                    AddHandler tbpBonus.Enter, AddressOf tbpBonus_Enter

                ElseIf selectedTab Is tbpAttachment Then

                    RemoveHandler tbpAttachment.Enter, AddressOf tbpAttachment_Enter
                    Await AttachmentTab.SetEmployee(employee)
                    AddHandler tbpAttachment.Enter, AddressOf tbpAttachment_Enter

                ElseIf selectedTab Is tbpSalary Then

                    RemoveHandler tbpSalary.Enter, AddressOf tbpNewSalary_Enter
                    Await SalaryTab.SetEmployee(employee)
                    AddHandler tbpSalary.Enter, AddressOf tbpNewSalary_Enter
                End If

                Dim filetmppath As String = Path.GetTempPath & "tmpfile.jpg"
                empPic = filetmppath
            End With
        Else
            LastFirstMidName = Nothing
            sameEmpID = -1
            Select Case tabIndx
                Case GetCheckListTabPageIndex()

                    For Each panel_ctrl As Control In panelchklist.Controls
                        If TypeOf panel_ctrl Is LinkLabel Then
                            DirectCast(panel_ctrl, LinkLabel).ImageIndex = 0
                        End If
                    Next
                    lblyourrequirement.Text = ""

                Case GetEmployeeProfileTabPageIndex() 'Employee

                    clearObjControl(SplitContainer2.Panel1)
                    clearObjControl(tbpleaveallow)
                    clearObjControl(tbpleavebal)

            End Select
        End If
    End Function

    Private Function GetDbString(input As Object) As String

        If IsDBNull(input) OrElse input Is Nothing Then
            Return Nothing
        Else
            Return input.ToString()
        End If

    End Function

    Private Function GetDbDate(input As Object) As Date

        Dim defaultOutput = Format(CDate(dbnow), machineShortDateFormat)

        If IsDBNull(input) Then Return defaultOutput

        Dim dateOutput = ObjectUtils.ToNullableDateTime(input)

        If dateOutput Is Nothing Then Return defaultOutput

        If dateOutput < PayrollTools.SqlServerMinimumDate Then Return defaultOutput

        Return dateOutput
    End Function

    Private Async Function SetEmployee() As Task

        Dim positionName = dgvEmp.CurrentRow.Cells("Column8").Value.ToString()

        reloadPositName(dgvEmp.CurrentRow.Cells("Column29").Value)

        txtDivisionName.Text = Await GetDivisionName(positionName)
        cboPosit.Text = positionName

        txtNName.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column5").Value), "", dgvEmp.CurrentRow.Cells("Column5").Value)

        dtpempbdate.Value = GetDbDate(dgvEmp.CurrentRow.Cells("Column6").Value)
        dtpempstartdate.Value = GetDbDate(dgvEmp.CurrentRow.Cells("colstartdate").Value)

        txtTIN.Text = GetDbString(dgvEmp.CurrentRow.Cells("Column10").Value)
        txtSSS.Text = GetDbString(dgvEmp.CurrentRow.Cells("Column11").Value)
        txtHDMF.Text = GetDbString(dgvEmp.CurrentRow.Cells("Column12").Value)
        txtPIN.Text = GetDbString(dgvEmp.CurrentRow.Cells("Column13").Value)
        txtWorkPhne.Text = GetDbString(dgvEmp.CurrentRow.Cells("Column15").Value)
        txtHomePhne.Text = GetDbString(dgvEmp.CurrentRow.Cells("Column16").Value)
        txtMobPhne.Text = GetDbString(dgvEmp.CurrentRow.Cells("Column17").Value)
        txtHomeAddr.Text = GetDbString(dgvEmp.CurrentRow.Cells("Column18").Value)
        txtemail.Text = GetDbString(dgvEmp.CurrentRow.Cells("Column14").Value)

        RemoveHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged

        If dgvEmp.CurrentRow.Cells("Column20").Value = "" Then
            cboEmpStat.SelectedIndex = -1
            cboEmpStat.Text = ""
        Else
            cboEmpStat.Text = dgvEmp.CurrentRow.Cells("Column20").Value
        End If

        SetComboBoxValue(dgvEmp.CurrentRow.Cells("Column9").Value, cboSalut)

        SetComboBoxValue(dgvEmp.CurrentRow.Cells("Column31").Value, cboMaritStat)

        SetComboBoxValue(dgvEmp.CurrentRow.Cells("Column34").Value, cboEmpType)

        Dim radioGender As RadioButton
        If dgvEmp.CurrentRow.Cells("Column19").Value = "Male" Then
            rdMale.Checked = True
            radioGender = rdMale
        Else
            rdFMale.Checked = True
            radioGender = rdFMale
        End If

        Await GenderChanged(radioGender)

        noCurrCellChange = 0

        pbemppic.Image = Nothing
        pbemppic.Image = EmployeeImage
        txtEmpID.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column1").Value), "", dgvEmp.CurrentRow.Cells("Column1").Value)
        txtFName.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column2").Value), "", dgvEmp.CurrentRow.Cells("Column2").Value)
        txtMName.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column3").Value), "", dgvEmp.CurrentRow.Cells("Column3").Value)
        txtLName.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column4").Value), "", dgvEmp.CurrentRow.Cells("Column4").Value)

        If if_sysowner_is_benchmark Then

            LeaveAllowanceTextBox.Text = dgvEmp.CurrentRow.Cells("Column36").Value
            LeaveBalanceTextBox.Text = dgvEmp.CurrentRow.Cells("Column35").Value

        End If

        txtvlallow.Text = dgvEmp.CurrentRow.Cells("Column36").Value
        txtslallow.Text = dgvEmp.CurrentRow.Cells("slallowance").Value
        txtmlallow.Text = dgvEmp.CurrentRow.Cells("mlallowance").Value

        txtvlbal.Text = dgvEmp.CurrentRow.Cells("Column35").Value
        txtslbal.Text = dgvEmp.CurrentRow.Cells("slbalance").Value
        txtmlbal.Text = dgvEmp.CurrentRow.Cells("mlbalance").Value

        txtWorkDaysPerYear.Text = dgvEmp.CurrentRow.Cells("WorkDaysPerYear").Value
        cboDayOfRest.Text = String.Empty
        cboDayOfRest.Text = dgvEmp.CurrentRow.Cells("DayOfRest").Value
        txtATM.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("ATMNo").Value), "", dgvEmp.CurrentRow.Cells("ATMNo").Value)
        txtothrallow.Text = dgvEmp.CurrentRow.Cells("OtherLeaveAllowance").Value
        txtothrbal.Text = dgvEmp.CurrentRow.Cells("OtherLeaveBalance").Value
        If String.IsNullOrWhiteSpace(txtATM.Text) Then
            rdbCash.Checked = True
            rdbDirectDepo.Checked = False
        Else
            rdbCash.Checked = False
            rdbDirectDepo.Checked = True
        End If

        rdbDirectDepo_CheckedChanged(rdbDirectDepo, New EventArgs)

        chkcalcHoliday.Checked = Convert.ToInt16(dgvEmp.CurrentRow.Cells("CalcHoliday").Value) 'If(dgvEmp.CurrentRow.Cells("CalcHoliday").Value = "Y", True, False)
        chkcalcSpclHoliday.Checked = Convert.ToInt16(dgvEmp.CurrentRow.Cells("CalcSpecialHoliday").Value) 'If(dgvEmp.CurrentRow.Cells("CalcSpecialHoliday").Value = "Y", True, False)
        chkcalcNightDiff.Checked = Convert.ToInt16(dgvEmp.CurrentRow.Cells("CalcNightDiff").Value) 'If(dgvEmp.CurrentRow.Cells("CalcNightDiff").Value = "Y", True, False)
        chkcalcRestDay.Checked = Convert.ToInt16(dgvEmp.CurrentRow.Cells("CalcRestDay").Value) 'If(dgvEmp.CurrentRow.Cells("CalcRestDay").Value = "Y", True, False)

        txtUTgrace.Text = dgvEmp.CurrentRow.Cells("LateGracePeriod").Value 'AgencyName
        cboAgency.Text = dgvEmp.CurrentRow.Cells("AgencyName").Value

        chkGracePeriodAsBuffer.Checked = Convert.ToInt16(Convert.ToInt16(dgvEmp.CurrentRow.Cells(GracePeriodAsBuffer.Name).Value))
        chkOvertimeOverride.Checked = Convert.ToInt16(Convert.ToInt16(dgvEmp.CurrentRow.Cells(OvertimeOverride.Name).Value))

        Dim dataRow = DirectCast(dgvEmp.CurrentRow.Tag, DataRow)
        If dataRow IsNot Nothing Then
            Dim hasDateEvaluated = Not IsDBNull(dataRow("DateEvaluated"))
            dtpEvaluationDate.Value = If(hasDateEvaluated, dataRow("DateEvaluated"), dtpEvaluationDate.MinDate)
            dtpEvaluationDate.Checked = hasDateEvaluated

            Dim hasDateRegularized = Not IsDBNull(dataRow("DateRegularized"))
            dtpRegularizationDate.Value = If(hasDateRegularized, dataRow("DateRegularized"), dtpRegularizationDate.MinDate)
            dtpRegularizationDate.Checked = hasDateRegularized
        End If

        Dim branchId = ObjectUtils.ToNullableInteger(dgvEmp.CurrentRow.Cells("BranchID").Value)
        Dim currentBranch As LookUpItem = GetCurrentBranch(branchId)

        BranchComboBox.SelectedItem = currentBranch

        BPIinsuranceText.Text = dgvEmp.CurrentRow.Cells("BPIInsuranceColumn").Value

        AddHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged
    End Function

    Private Function GetCurrentBranch(branchId As Integer?) As LookUpItem
        Dim branchLookUpItems = CType(BranchComboBox.DataSource, List(Of LookUpItem))

        Dim currentBranch = branchLookUpItems?.Where(Function(b) Nullable.Equals(b.Id, branchId)).FirstOrDefault()

        If currentBranch Is Nothing Then

            currentBranch = branchLookUpItems?.Where(Function(b) Nullable.Equals(b.Id, Nothing)).FirstOrDefault()
        End If

        Return currentBranch
    End Function

    Private Sub SetComboBoxValue(dbValue As Object, comboBox As ComboBox)
        If IsDBNull(dbValue) OrElse dbValue = "" Then
            comboBox.SelectedIndex = -1
            comboBox.Text = ""
        Else
            comboBox.Text = dbValue
        End If
    End Sub

    Private Shared Async Function GetCurrentEmployeeEntity(employeeID As Integer?) As Task(Of Employee)

        Dim employeeBuilder = MainServiceProvider.GetRequiredService(Of IEmployeeQueryBuilder)

        Return Await employeeBuilder.
            IncludePayFrequency().
            IncludePosition().
            GetByIdAsync(employeeID, z_OrganizationID)

    End Function

    Sub tsbtnNewEmp_Click(sender As Object, e As EventArgs) Handles tsbtnNewEmp.Click

        RemoveHandler tbpEmployee.Enter, AddressOf tbpEmployee_Enter

        If tsbtnNewEmp.Visible = False Then

            AddHandler tbpEmployee.Enter, AddressOf tbpEmployee_Enter
            Return

        End If

        txtEmpID.Focus()
        tsbtnNewEmp.Enabled = False
        clearObjControl(SplitContainer2.Panel1)
        clearObjControl(SplitContainer2.Panel2)

        rdMale.Checked = True : rdFMale.Checked = False

        pbemppic.Image = Nothing
        File.Delete(Path.GetTempPath & "tmpfileEmployeeImage.jpg")

        LeaveBalanceTextBox.Text = 0
        txtvlbal.Text = 0
        txtslbal.Text = 0
        txtmlbal.Text = 0

        Dim leavedefaults As New DataTable
        leavedefaults = retAsDatTbl("SELECT CAST(VacationLeaveDays AS DECIMAL(11,2)) 'vl_allowance'" &
                                        ",CAST(SickLeaveDays AS DECIMAL(11,2)) 'sl_allowance'" &
                                        ",CAST(MaternityLeaveDays AS DECIMAL(11,2)) 'ml_allowance'" &
                                        ",CAST((VacationLeaveDays) / 26 AS DECIMAL(11,2)) 'vl_payp'" &
                                        ",CAST((SickLeaveDays) / 26 AS DECIMAL(11,2)) 'sl_payp'" &
                                        ",CAST((MaternityLeaveDays) / 26 AS DECIMAL(11,2)) 'ml_payp'" &
                                        " FROM organization" &
                                        " WHERE RowID=" & orgztnID & ";")
        For Each drow As DataRow In leavedefaults.Rows

            If if_sysowner_is_benchmark Then

                LeaveAllowanceTextBox.Text = If(IsDBNull(drow("vl_allowance")), 0.0, drow("vl_allowance"))

            End If

            txtvlallow.Text = If(IsDBNull(drow("vl_allowance")), 0.0, drow("vl_allowance"))
            txtslallow.Text = If(IsDBNull(drow("sl_allowance")), 0.0, drow("sl_allowance"))
            txtmlallow.Text = If(IsDBNull(drow("ml_allowance")), 0.0, drow("ml_allowance"))

        Next
        dtpempstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
        dtpempbdate.Value = Format(CDate(dbnow), machineShortDateFormat)

        BPIinsuranceText.Text = _policy.DefaultBPIInsurance

        chkcalcHoliday.Checked = True
        chkcalcNightDiff.Checked = True
        chkcalcSpclHoliday.Checked = True
        chkcalcRestDay.Checked = True

        AddHandler tbpEmployee.Enter, AddressOf tbpEmployee_Enter
    End Sub

    Private Async Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click
        cboEmpStat.Enabled = True
        tsbtnNewEmp.Enabled = True

        Await PopulateEmployeeData()
    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles tabctrlemp.DrawItem
        TabControlColor(tabctrlemp, e)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        'Removed the old position form
        'When this button is visible again, add code to show the new add position form
    End Sub

    Private Async Sub cboEmpStat_TextChanged(sender As Object, e As EventArgs)
        Await ChangeEmployeeStatus()

    End Sub

    Private Async Function ChangeEmployeeStatus() As Task
        If publicEmpRowID IsNot Nothing Then
            If tsbtnNewEmp.Enabled Then
                If (cboEmpStat.Text.Contains("Terminat") Or cboEmpStat.Text.Contains("Resign")) Then

                    Dim n_SetEmployeeEndDate As _
                        New SetEmployeeEndDate(publicEmpRowID)

                    If n_SetEmployeeEndDate.ShowDialog = Windows.Forms.DialogResult.OK Then

                        Dim n_ExecuteQuery As _
                            New ExecuteQuery("UPDATE employee" &
                                             " SET EmploymentStatus='" & cboEmpStat.Text.Trim & "'" &
                                             ",TerminationDate='" & n_SetEmployeeEndDate.ReturnDateValue & "'" &
                                             ",LastUpdBy='" & z_User & "'" &
                                             " WHERE RowID='" & publicEmpRowID & "';")
                        If n_ExecuteQuery.HasError = False Then
                            Await EmployeeGridViewPaginationChanged(First)
                            InfoBalloon("Employee ID '" & txtEmpID.Text & "' has been updated successfully.", "Employee Update Successful", lblforballoon, 0, -69)
                        End If
                    Else
                        RemoveHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged
                        cboEmpStat.Text = New ExecuteQuery("SELECT EmploymentStatus FROM employee WHERE RowID='" & publicEmpRowID & "';").Result
                        AddHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged

                    End If

                End If
            End If
        End If
    End Function

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If Button3.Image.Tag = 1 Then
            Button3.Image = Nothing
            Button3.Image = My.Resources.r_arrow
            Button3.Image.Tag = 0

            tabctrlemp.Show()
            dgvEmp.Width = 350

            Await PopulateEmployeeData()
        Else
            Button3.Image = Nothing
            Button3.Image = My.Resources.l_arrow
            Button3.Image.Tag = 1

            tabctrlemp.Hide()
            Dim pointX As Integer = Width_resolution - (Width_resolution * 0.15)
            dgvEmp.Width = pointX
        End If
    End Sub

    Dim curr_empColm As String
    Dim curr_empRow As Integer

    Private Async Sub SearchEmployee_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Await SearchEmployee()
    End Sub

    Public Async Function SearchEmployee() As Task

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        If dgvEmp.RowCount <> 0 Then
            curr_empRow = dgvEmp.CurrentRow.Index
            curr_empColm = dgvEmp.Columns(dgvEmp.CurrentCell.ColumnIndex).Name
            dgvEmp.Item(curr_empColm, curr_empRow).Selected = True
        End If

        Await PopulateEmployeeGrid()

        If dgvEmp.RowCount <> 0 Then
            If curr_empRow <= dgvEmp.RowCount - 1 Then

                If curr_empColm Is Nothing Then

                    dgvEmp.Item(Column1.Name, curr_empRow).Selected = True
                Else

                    dgvEmp.Item(curr_empColm, curr_empRow).Selected = True
                End If
            Else
                dgvEmp.Item(curr_empColm, 0).Selected = True
            End If

            Await PopulateEmployeeData()
        End If

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
    End Function

    Private Async Function PopulateEmployeeGrid() As Task
        Dim param_array = New Object() {
            orgztnID,
            TextBox1.Text,
            TextBox15.Text,
            TextBox16.Text,
            pagination}

        Dim n_ReadSQLProcedureToDatatable As New _
            ReadSQLProcedureToDatatable("SEARCH_employeeprofile", param_array)
        Dim dtemployee As New DataTable
        dtemployee = n_ReadSQLProcedureToDatatable.ResultTable
        dgvEmp.Rows.Clear()

        For Each drow In dtemployee.Rows.OfType(Of DataRow).ToList()
            Dim rowArray = drow.ItemArray()
            Dim index = dgvEmp.Rows.Add(rowArray)
            dgvEmp.Rows(index).Tag = drow
        Next
        dtemployee.Dispose()
        employeepix = retAsDatTbl("SELECT e.RowID,COALESCE(e.Image,'') 'Image' FROM employee e WHERE e.OrganizationID=" & orgztnID & " ORDER BY e.RowID DESC;")

        Await PopulateEmployeeData()
    End Function

    Private Async Sub cboSearchCommon_KeyPress(sender As Object, e As KeyPressEventArgs) Handles _
        ComboBox7.KeyPress,
        ComboBox8.KeyPress,
        ComboBox9.KeyPress,
        ComboBox10.KeyPress

        Dim e_asc As String = Asc(e.KeyChar)
        If e_asc = 8 Then
            e.Handled = False
            DirectCast(sender, ComboBox).SelectedIndex = -1
        ElseIf e_asc = 13 Then
            Await SearchEmployee()
        Else : e.Handled = True
        End If
    End Sub

    Private Sub ComboBox7_TextChanged(sender As Object, e As EventArgs) Handles ComboBox7.TextChanged, ComboBox8.TextChanged, ComboBox9.TextChanged,
                                                                                           ComboBox10.TextChanged
        Dim cboSearch_nem As String = DirectCast(sender, ComboBox).Name

        If cboSearch_nem = "ComboBox7" Then
            TextBox1.ReadOnly = If(ComboBox7.Text = "is empty null" Or ComboBox7.Text = "is not empty", True, False)
            If TextBox1.ReadOnly Then
                TextBox1.BackColor = Color.FromArgb(255, 255, 255)
            End If
        ElseIf cboSearch_nem = "ComboBox8" Then
            TextBox15.ReadOnly = If(ComboBox8.Text = "is empty null" Or ComboBox8.Text = "is not empty", True, False)
            If TextBox15.ReadOnly Then
                TextBox15.BackColor = Color.FromArgb(255, 255, 255)
            End If
        ElseIf cboSearch_nem = "ComboBox9" Then
            TextBox16.ReadOnly = If(ComboBox9.Text = "is empty null" Or ComboBox9.Text = "is not empty", True, False)
            If TextBox16.ReadOnly Then
                TextBox16.BackColor = Color.FromArgb(255, 255, 255)
            End If
        ElseIf cboSearch_nem = "ComboBox10" Then
            TextBox17.ReadOnly = If(ComboBox10.Text = "is empty null" Or ComboBox10.Text = "is not empty", True, False)
            If TextBox17.ReadOnly Then
                TextBox17.BackColor = Color.FromArgb(255, 255, 255)
            End If
        End If
    End Sub

    Private Async Sub Search_KeyPress(sender As Object, e As KeyPressEventArgs) Handles _
        TextBox1.KeyPress,
        TextBox15.KeyPress,
        TextBox16.KeyPress,
        TextBox17.KeyPress

        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 13 Then
            Await SearchEmployee()
        End If
    End Sub

    Private Sub txtBDate_TextChanged(sender As Object, e As EventArgs) Handles txtBDate.TextChanged
        'dtpempenddate
    End Sub

    Dim empPic As String

    Private Sub btnbrowse_Click(sender As Object, e As EventArgs) Handles btnbrowse.Click
        Static employeeleaveRowID As Integer = -1
        Try
            Dim browsefile As OpenFileDialog = New OpenFileDialog()
            browsefile.Filter = "JPEG(*.jpg)|*.jpg" '& _
            '"PNG(*.PNG)|*.png|" & _
            '"Bitmap(*.BMP)|*.bmp"
            If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

                empPic = browsefile.FileName

                pbemppic.Image = Image.FromFile(browsefile.FileName)

                For Each drow As DataRow In employeepix.Rows
                    If drow("RowID").ToString = dgvEmp.CurrentRow.Cells("RowID").Value Then
                        drow("Image") = If(empPic Is Nothing,
                                           Nothing,
                                           convertFileToByte(empPic))

                        If empPic Is Nothing Then
                        Else
                            makefileGetPath(drow("Image"))
                        End If

                        Exit For

                    End If
                Next
            Else
                empPic = Nothing

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        Finally
            'dgvempleave_SelectionChanged(sender, e)
            'AddHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged
        End Try
    End Sub

    Private Sub btnclearimage_Click(sender As Object, e As EventArgs) Handles btnclearimage.Click
        empPic = Nothing

        pbemppic.Image = Nothing

        For Each drow As DataRow In employeepix.Rows
            If drow("RowID").ToString = dgvEmp.CurrentRow.Cells("RowID").Value Then
                drow("Image") = Nothing

                File.Delete(Path.GetTempPath & "tmpfileEmployeeImage.jpg")

                Exit For
            End If
        Next
    End Sub

    Public tabIndx As Integer = -1

    Dim pagination As Integer = 0

    Const emp_page_limiter As Integer = 50

    Private Async Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles _
        First.LinkClicked,
        Prev.LinkClicked,
        Nxt.LinkClicked,
        Last.LinkClicked

        Await EmployeeGridViewPaginationChanged(sender)

    End Sub

    Private Async Function EmployeeGridViewPaginationChanged(sender As Object) As Task
        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "First" Then
            pagination = 0
        ElseIf sendrname = "Prev" Then

            Dim modcent = pagination Mod emp_page_limiter

            If modcent = 0 Then

                pagination -= emp_page_limiter
            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sendrname = "Nxt" Then

            Dim modcent = pagination Mod emp_page_limiter

            If modcent = 0 Then
                pagination += emp_page_limiter
            Else
                pagination -= modcent

                pagination += emp_page_limiter

            End If
        ElseIf sendrname = "Last" Then
            Dim lastpage = Val(EXECQUER(String.Concat("SELECT COUNT(RowID) / ", emp_page_limiter, " FROM employee WHERE OrganizationID=", orgztnID, ";")))

            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * emp_page_limiter

            If pagination - emp_page_limiter < emp_page_limiter Then

            End If

        End If

        If (Trim(TextBox1.Text) <> "" Or
            Trim(TextBox15.Text) <> "" Or
            Trim(TextBox16.Text) <> "" Or
            Trim(TextBox17.Text) <> "") Then

            Await SearchEmployee()
        Else

            Await LoadEmployee()

        End If

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
    End Function

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabctrlemp.SelectedIndexChanged
        Label25.Text = Trim(tabctrlemp.SelectedTab.Text)
    End Sub

    Dim emp_ralation As New AutoCompleteStringCollection

    Async Sub tbpEmployee_Enter(sender As Object, e As EventArgs)
        Await OpenEmployeeTab()

    End Sub

    Public Async Function OpenEmployeeTab() As Task
        UpdateTabPageText()

        tbpEmployee.Text = "PERSONAL PROFILE               "

        Label25.Text = "PERSONAL PROFILE"
        Dim once As SByte = 0
        If once = 0 Then
            once = 1

            cboPosit.ContextMenu = New ContextMenu

            txtUTgrace.ContextMenu = New ContextMenu

            txtWorkDaysPerYear.ContextMenu = New ContextMenu

            loadPositName()

            enlistToCboBox(q_empstat, cboEmpStat) '"SELECT DISTINCT(COALESCE(DisplayValue,'')) FROM listofval WHERE Type='Status' AND Active='Yes'"

            enlistToCboBox(q_emptype, cboEmpType)

            enlistToCboBox(q_maritstat, cboMaritStat)

            enlistToCboBox("SELECT DisplayValue FROM listofval WHERE `Type`='Bank Names';", cbobank)

            Await ShowBranch()

            ShowBPIInsurance()

            Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(
                "SELECT '' AS RowID, '' AS AgencyName" &
                " UNION" &
                " SELECT RowID,AgencyName FROM agency WHERE OrganizationID='" & orgztnID & "' AND IsActive=1;")

            Dim dt_agency As New DataTable

            dt_agency = n_SQLQueryToDatatable.ResultTable

            With cboAgency

                .DisplayMember = dt_agency.Columns(1).ColumnName

                .ValueMember = dt_agency.Columns(0).ColumnName

                .DataSource = dt_agency

            End With

            If dbnow Is Nothing Then
                dbnow = EXECQUER(CURDATE_MDY)
            End If

            dtpempstartdate.Value = dbnow 'Format(CDate(dbnow), machineShortDateFormat)

            enlistTheLists(
                "SELECT DisplayValue FROM listofval WHERE Type='Employee Relationship' ORDER BY OrderBy;",
                emp_ralation)

            Dim role = PermissionHelper.GetRole(PermissionConstant.EMPLOYEE)

            tsbtnNewEmp.Visible = False
            tsbtnImport.Visible = False
            tsbtnSaveEmp.Visible = False
            tsbtnCancel.Visible = False

            If role.Success Then
                _currentRolePermission = role.RolePermission

                If _currentRolePermission.Create Then
                    tsbtnNewEmp.Visible = True
                    tsbtnImport.Visible = True

                End If

                If _currentRolePermission.Update OrElse _currentRolePermission.Create Then
                    tsbtnSaveEmp.Visible = True
                    tsbtnCancel.Visible = True
                End If

            End If

        End If

        tabIndx = GetEmployeeProfileTabPageIndex()

        Await PopulateEmployeeData()
    End Function

    Private Async Function ShowBranch() As Task

        _branches = New List(Of Branch)

        BranchComboBox.Visible = True
        BranchLabel.Visible = True
        AddBranchLinkButton.Visible = True

        BranchComboBox.ValueMember = "Id"
        BranchComboBox.DisplayMember = "DisplayMember"

        Await PopulateBranchComboBox()
    End Function

    Private Async Function PopulateBranchComboBox() As Task
        Dim branchRepository = MainServiceProvider.GetRequiredService(Of IBranchRepository)
        _branches = (Await branchRepository.GetAllAsync()).
            OrderBy(Function(b) b.Name).
            ToList()

        Dim branchLookUpItems = LookUpItem.Convert(
            _branches,
            idPropertyName:="RowID",
            displayMemberPropertyName:="Name",
            hasDefaultItem:=True)

        BranchComboBox.DataSource = branchLookUpItems
    End Function

    Private Sub ShowBPIInsurance()

        If _policy.UseBPIInsurance = False Then

            BPIinsuranceText.Visible = False
            BPIinsuranceLabel.Visible = False

            Return

        End If

        BPIinsuranceText.Visible = True
        BPIinsuranceLabel.Visible = True

    End Sub

    Private Sub tbpEmployee_Leave(sender As Object, e As EventArgs) 'Handles tbpEmployee.Leave
        tbpEmployee.Text = "PERSON"
    End Sub

    Private Sub txtvlallow_KeyPress(sender As Object, e As KeyPressEventArgs) _
        Handles txtvlallow.KeyPress, LeaveAllowanceTextBox.KeyPress

        Dim e_KAsc As String = Asc(e.KeyChar)

        Dim textbox = CType(sender, TextBox)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If textbox.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If textbox.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtslallow_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtslallow.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtslallow.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtslallow.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtmlallow_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtmlallow.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtmlallow.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtmlallow.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtothrallow_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtothrallow.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtothrallow.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtothrallow.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtvlbal_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtvlbal.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtvlbal.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtvlbal.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtslbal_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtslbal.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtslbal.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtslbal.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtmlbal_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtmlbal.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtmlbal.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtmlbal.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtUTgrace_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUTgrace.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, txtUTgrace.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

    End Sub

#End Region 'Personal Profile

#Region "Awards"

    Private Async Sub tbpAwards_Enter(sender As Object, e As EventArgs) Handles tbpAwards.Enter

        UpdateTabPageText()

        tbpAwards.Text = "AWARDS               "
        Label25.Text = "AWARDS"

        Await PopulateEmployeeData()

    End Sub

#End Region 'Awards

#Region "Certifications"

    Private Async Sub tbpCertifications_Enter(sender As Object, e As EventArgs) Handles tbpCertifications.Enter

        UpdateTabPageText()

        tbpCertifications.Text = "CERTIFICATIONS               "
        Label25.Text = "CERTIFICATIONS"

        Await PopulateEmployeeData()

    End Sub

#End Region 'Certifications

#Region "Disciplinary Action"

    Private Async Sub tbpDiscipAct_Enter(sender As Object, e As EventArgs) Handles tbpDiscipAct.Enter

        UpdateTabPageText()

        tbpDiscipAct.Text = "DISCIPLINARY ACTION               "

        Label25.Text = "DISCIPLINARY ACTION"

        Await PopulateEmployeeData()

    End Sub

#End Region 'Disciplinary Action

#Region "Educational Background"

    Private Async Sub tbpEducBG_Enter(sender As Object, e As EventArgs) Handles tbpEducBG.Enter
        UpdateTabPageText()

        tbpEducBG.Text = "EDUCATIONAL BACKGROUND               "
        Label25.Text = "EDUCATIONAL BACKGROUND"

        Await PopulateEmployeeData()
    End Sub

#End Region 'Educational Background

#Region "Previous Employer"

    Private Async Sub tbpPrevEmp_Enter(sender As Object, e As EventArgs) Handles tbpPrevEmp.Enter

        UpdateTabPageText()

        tbpPrevEmp.Text = "PREVIOUS EMPLOYER               "
        Label25.Text = "PREVIOUS EMPLOYER"

        Await PopulateEmployeeData()

    End Sub

#End Region 'Previous Employer

#Region "Salary"

    Private Async Sub tbpNewSalary_Enter(sender As Object, e As EventArgs) Handles tbpSalary.Enter

        UpdateTabPageText()

        tbpSalary.Text = "SALARY               "
        Label25.Text = "SALARY"

        Await PopulateEmployeeData()

    End Sub

#End Region 'Salary

#Region "Bonus"

    Private Async Sub tbpBonus_Enter(sender As Object, e As EventArgs) Handles tbpBonus.Enter

        UpdateTabPageText()

        tbpBonus.Text = "EMPLOYEE BONUS               "
        Label25.Text = "EMPLOYEE BONUS"

        Await PopulateEmployeeData()

    End Sub

#End Region 'Bonus

#Region "Attachment"

    Private Async Sub tbpAttachment_Enter(sender As Object, e As EventArgs) Handles tbpAttachment.Enter

        UpdateTabPageText()

        tbpAttachment.Text = "ATTACHMENT               "

        Label25.Text = "ATTACHMENT"

        Await PopulateEmployeeData()

    End Sub

#End Region

    Sub UpdateTabPageText()
        Static once As SByte = 0

        If once = 0 Then
            once = 1
            Exit Sub
        End If

        tbpempchklist.Text = "CHECKLIST"
        tbpEmployee.Text = "PERSON"
        tbpAwards.Text = "AWARD"
        tbpCertifications.Text = "CERTI"
        tbpDiscipAct.Text = "DISCIP"
        tbpEducBG.Text = "EDUC"
        tbpPrevEmp.Text = "PREV EMP"
        tbpBonus.Text = "BONUS"
        tbpAttachment.Text = "ATTACH"
        tbpSalary.Text = "SALARY"

    End Sub

    Private Async Sub AddBranchLinkButton_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles AddBranchLinkButton.LinkClicked

        Dim form As New AddBranchForm
        form.ShowDialog()

        If form.HasChanges Then

            Dim branchId = ObjectUtils.ToNullableInteger(BranchComboBox.SelectedValue)

            If form.LastAddedBranchId IsNot Nothing Then

                branchId = form.LastAddedBranchId

            End If

            Await PopulateBranchComboBox()

            BranchComboBox.SelectedItem = GetCurrentBranch(branchId)

        End If

    End Sub

    Private Async Sub ToolStripButton35_ClickAsync(sender As Object, e As EventArgs) Handles tsbtnImport.Click
        Using importForm = New ImportEmployeeForm()
            If Not importForm.ShowDialog() = DialogResult.OK Then
                Return
            End If

            Await FunctionUtils.TryCatchFunctionAsync("Import Employee",
                Async Function()

                    Await importForm.SaveAsync()

                    Await SearchEmployee()
                    InfoBalloon("Imported successfully.", "Done Importing Employee Profiles", lblforballoon, 0, -69)

                End Function)
        End Using
    End Sub

    Function INSUPDemployee(ParamArray paramSetValue() As Object) As Object
        Dim n_ReadSQLFunction As New ReadSQLFunction("INSUPD_employee", "returnval", paramSetValue)

        Return n_ReadSQLFunction.ReturnValue
    End Function

    Private Sub btnPrintMemo_Click(sender As Object, e As EventArgs) Handles btnPrintMemo.Click
        If dgvDisciplinaryList.SelectedRows.Count > 0 Then
            Dim frm As New Josh_CrysRepForm

            frm.employeeName = txtFNameDiscip.Text

            Dim dgvRow As DataGridViewRow = dgvDisciplinaryList.SelectedRows(0)

            frm.disciplinaryAction = dgvRow.Cells(1).Value.ToString().Trim
            frm.infraction = dgvRow.Cells(0).Value.ToString().Trim
            frm.comments = dgvRow.Cells(5).Value.ToString().Trim

            frm.Show()
        Else : MsgBox("Choose a disciplinary action first")
        End If
    End Sub

    Private Sub txtWorkHoursPerWeek_KeyPress(sender As Object, e As KeyPressEventArgs) Handles _
                                                                                                txtWorkDaysPerYear.KeyPress
        'e.Handled = TrapNumKey(Asc(e.KeyChar))
        e.Handled = New TrapDecimalKey(Asc(e.KeyChar), txtWorkDaysPerYear.Text).ResultTrap

    End Sub

    Private Sub rdbDirectDepo_CheckedChanged(sender As Object, e As EventArgs) Handles rdbDirectDepo.CheckedChanged

        Dim checkstate = rdbDirectDepo.Checked

        txtATM.Enabled = checkstate

        cbobank.Enabled = checkstate

        lnklblAddBank.Enabled = checkstate

        Label359.Visible = checkstate

        Label360.Visible = checkstate

        If cbobank.Enabled Then

            If dgvEmp.RowCount <> 0 Then

                cbobank.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("BankName").Value), "", dgvEmp.CurrentRow.Cells("BankName").Value)

            End If
        Else
            cbobank.SelectedIndex = -1

        End If

    End Sub

    Private Sub txtATM_EnabledChanged(sender As Object, e As EventArgs) Handles txtATM.EnabledChanged
        If txtATM.Enabled = False Then
            txtATM.Text = String.Empty
        End If
    End Sub

    Private Sub cbobank_EnabledChanged(sender As Object, e As EventArgs) Handles cbobank.EnabledChanged
        If cbobank.Enabled = False Then
            cbobank.Text = String.Empty
        End If
    End Sub

    Private Sub cbobank_Leave(sender As Object, e As EventArgs) Handles cbobank.Leave

        If cbobank.Items.Count <> 0 Then

        End If

    End Sub

    Private Sub lnklblAddBank_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblAddBank.LinkClicked

        Dim message, title, defaultValue As String

        Dim myValue As Object
        ' Set prompt.
        message = "Please input a Bank Name"
        ' Set title.
        title = "Add Bank Name"
        defaultValue = String.Empty    ' Set default value.

        ' Display message, title, and default value.
        myValue = InputBox(message, title, defaultValue)
        ' If user has clicked Cancel, set myValue to defaultValue
        If myValue Is "" Then myValue = defaultValue

        If myValue <> String.Empty Then

            If myValue.ToString.Trim.Length > 50 Then
                myValue = myValue.ToString.Trim.Substring(0, 50)
            Else
                myValue = myValue.ToString.Trim
            End If

            EXECQUER("SELECT `INSUPD_listofval`('" & myValue.ToString.Trim & "'" &
                     ", '" & myValue.ToString.Trim & "'" &
                     ", 'Bank Names'" &
                     ", '" & myValue.ToString.Trim & "'" &
                     ", 'Yes'" &
                     ", '" & myValue.ToString.Trim & "'" &
                     ", '" & z_User & "'" &
                     ", '1');")

            cbobank.Items.Add(myValue.ToString.Trim)

            cbobank.Text = myValue.ToString.Trim

        End If

    End Sub

    Private Async Sub Gender_CheckedChanged(sender As RadioButton, e As EventArgs) Handles rdMale.CheckedChanged, rdFMale.CheckedChanged
        Await GenderChanged(sender)

    End Sub

    Private Async Function GenderChanged(sender As RadioButton) As Task
        If Not sender.Checked Then Return

        Dim label_gender As String

        If sender.Name = rdMale.Name Then

            label_gender = "Paternity"

            Await LoadSalutation(Gender.Male)

            Label148.Text = label_gender
            Label149.Text = label_gender
        ElseIf sender.Name = rdFMale.Name Then

            label_gender = "Maternity"

            Await LoadSalutation(Gender.Female)

            Label148.Text = label_gender
            Label149.Text = label_gender
        End If
    End Function

    Private Sub UserActivityEmployeeToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityEmployeeToolStripButton.Click
        Dim userActivity As New UserActivityForm(EmployeeEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Sub Print201ReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Print201ReportToolStripMenuItem.Click
        Print201Report()
    End Sub

    Dim indentifyGender As Dictionary(Of Gender, String) =
        New Dictionary(Of Gender, String) From {{Gender.Male, Gender.Male.ToString()}, {Gender.Female, Gender.Female.ToString()}}

    Private Async Function LoadSalutation(gender As Gender) As Task
        Dim genderList = {"Neutral", indentifyGender(gender)}

        Dim listOfValueRepository = MainServiceProvider.GetRequiredService(Of IListOfValueRepository)

        Dim salutationList = Await listOfValueRepository.
            GetFilteredListOfValuesAsync(Function(l) l.Type = "Salutation" AndAlso genderList.Contains(l.ParentLIC))

        salutationList = salutationList.OrderBy(Function(l) l.DisplayValue).ToList()

        Dim salutations = salutationList.
                GroupBy(Function(l) l.DisplayValue).
                Select(Function(l) l.FirstOrDefault.DisplayValue).
                ToArray()

        cboSalut.Text = String.Empty
        cboSalut.Items.Clear()
        cboSalut.Items.Add(String.Empty)
        cboSalut.Items.AddRange(salutations)

        Dim currentRow = dgvEmp.CurrentRow
        If currentRow IsNot Nothing Then
            With currentRow
                If Not IsDBNull(.Cells(Column9.Name).Value) AndAlso Not String.IsNullOrWhiteSpace(.Cells(Column9.Name).Value) Then

                    cboSalut.Text = CStr(.Cells(Column9.Name).Value)
                End If

                If IsDBNull(.Cells(Column9.Name).Value) OrElse CStr(.Cells(Column19.Name).Value) <> gender.ToString() Then
                    cboSalut.SelectedIndex = 0
                    cboSalut.Text = String.Empty
                End If
            End With
        End If

    End Function

    Private Enum Gender
        Male
        Female
    End Enum

    Private Async Sub LaGlobalEmployeeReportMenu_Click(sender As ToolStripMenuItem, e As EventArgs) Handles ActiveEmployeeChecklistReportToolStripMenuItem.Click,
        BPIInsuranceAmountReportToolStripMenuItem.Click,
        EmploymentContractToolStripMenuItem.Click,
        EndOfContractReportToolStripMenuItem.Click,
        MonthlyBirthdayReportToolStripMenuItem.Click,
        DeploymentEndorsementToolStripMenuItem.Click,
        WorkOrderToolStripMenuItem.Click

        Dim employeeRow = dgvEmp.CurrentRow
        If employeeRow Is Nothing Then

            MessageBoxHelper.Warning("No selected employee.")
            Return

        End If

        Dim employeeNumber = employeeRow.Cells(Column1.Name).Value

        Dim employee As Employee

        Dim employeeBuilder = MainServiceProvider.GetRequiredService(Of IEmployeeQueryBuilder)

        employee = Await employeeBuilder.
            IncludePosition().
            IncludeBranch().
            ByEmployeeNumber(employeeNumber).
            FirstOrDefaultAsync(z_OrganizationID)

        If employee Is Nothing Then

            MessageBoxHelper.Warning("No selected employee.")
            Return

        ElseIf sender Is EmploymentContractToolStripMenuItem AndAlso
            (String.IsNullOrWhiteSpace(employee.SssNo) OrElse
            String.IsNullOrWhiteSpace(employee.PhilHealthNo) OrElse
            String.IsNullOrWhiteSpace(employee.HdmfNo) OrElse
            String.IsNullOrWhiteSpace(employee.TinNo)) Then

            MessageBoxHelper.Warning("Employee needs to have SSS, Philhealth, PAGIBIG and TIN numbers encoded in his profile.")
            Return

        End If

        Dim selectedReport = _laGlobalEmployeeReports(sender.Name)

        Dim report = New LaGlobalEmployeeReports(employee)
        report.Print(selectedReport)
    End Sub

End Class
