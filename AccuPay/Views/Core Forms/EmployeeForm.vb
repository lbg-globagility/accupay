Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection
Imports MySql.Data.MySqlClient

Public Class EmployeeForm

    Private Const EmployeeEntityName As String = "Employee"

    Private Const DisciplinaryActionEntityName As String = "Disciplinary Action"

    Private Const PromotionEntityName As String = "Promotion"

    Private str_ms_excel_file_extensn As String =
        String.Concat("Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|",
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls")

    Private if_sysowner_is_benchmark As Boolean

    Private if_sysowner_is_laglobal As Boolean

    Private threadArrayList As New List(Of Thread)

    Private _branches As New List(Of Branch)

    Private _payFrequencies As New List(Of PayFrequency)

    Private _policy As PolicyHelper

    Private _systemOwnerService As SystemOwnerService

    Sub New()

        InitializeComponent()

        _policy = MainServiceProvider.GetRequiredService(Of PolicyHelper)
        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        SplitContainer2.SplitterWidth = 7
        MyBase.OnLoad(e)
    End Sub

#Region "Employee Check list"

    Dim empchklist_columns As New AutoCompleteStringCollection

    Sub tbpempchklist_Enter(sender As Object, e As EventArgs) Handles tbpempchklist.Enter
        InfoBalloon(, , txtTIN, , , 1)
        InfoBalloon(, , txtPIN, , , 1)
        InfoBalloon(, , txtHDMF, , , 1)
        InfoBalloon(, , txtSSS, , , 1)

        UpdateTabPageText()
        tbpempchklist.Text = "CHECK LIST               "
        Label25.Text = "CHECK LIST"
        Static once As SByte = 0

        If once = 0 Then
            once = 1

            imglstchklist.Images.Item(0).Tag = 0
            imglstchklist.Images.Item(1).Tag = 1

        End If

        tabIndx = GetCheckListTabPageIndex()
        dgvEmp_SelectionChanged(sender, e)
    End Sub

    Dim chkliststring As New AutoCompleteStringCollection

    Sub VIEW_employeechecklist(ByVal emp_rowid As Object)
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
                    Else
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
                        .Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!)
                        .LinkColor = Color.FromArgb(0, 155, 255)
                        'Me.txtEmpID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold)

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

    Public Function GetPromotionTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpPromotion)
    End Function

    Public Function GetBonusTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpBonus)
    End Function

    Public Function GetAttachmentTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpAttachment)
    End Function

    Public Function GetSalaryTabPageIndex() As Integer
        Return tabctrlemp.TabPages.IndexOf(tbpNewSalary)
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
    Public Simple As New AutoCompleteStringCollection
    Public SimpleDummy As New AutoCompleteStringCollection
    Dim payFreq As New AutoCompleteStringCollection
    Dim dbnow, u_nem, positID, payFreqID, yrold18 As String
    Dim emp_rcount, salutn_count, viewid As Integer

    Public q_employee As String = "SELECT e.RowID," &
        "e.EmployeeID 'Employee ID'," &
        "e.FirstName 'First Name'," &
        "e.MiddleName 'Middle Name'," &
        "e.LastName 'Last Name'," &
        "e.Surname," &
        "e.Nickname," &
        "e.MaritalStatus 'Marital Status'," &
        "COALESCE(e.NoOfDependents,0) 'No. Of Dependents'," &
        "e.Birthdate," &
        "e.Startdate," &
        "IFNULL(d.Name,'') `Job Title`," &
        "COALESCE(pos.PositionName,'') 'Position'," &
        "e.Salutation," &
        "e.TINNo 'TIN'," &
        "e.SSSNo 'SSS No.'," &
        "e.HDMFNo 'PAGIBIG No.'," &
        "e.PhilHealthNo 'PhilHealth No.'," &
        "e.WorkPhone 'Work Phone No.'," &
        "e.HomePhone 'Home Phone No.'," &
        "e.MobilePhone 'Mobile Phone No.'," &
        "e.HomeAddress 'Home address'," &
        "e.EmailAddress 'Email address'," &
        "IF(e.Gender='M','Male','Female') 'Gender'," &
        "e.EmploymentStatus 'Employment Status'," &
        "IFNULL(pf.PayFrequencyType,'') 'Pay Frequency'," &
        "e.UndertimeOverride," &
        "e.OvertimeOverride," &
        "DATE_FORMAT(e.Created,'%m-%d-%Y') 'Creation Date'," &
        "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by'," &
        "COALESCE(DATE_FORMAT(e.LastUpd,'%m-%d-%Y'),'') 'Last Update'," &
        "IFNULL((SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM user WHERE RowID=e.LastUpdBy),'') 'LastUpdate by'" &
        ",COALESCE(pos.RowID,'') 'PositionID'" &
        ",IFNULL(e.PayFrequencyID,'') 'PayFrequencyID'" &
        ",e.EmployeeType" &
        ",e.LeaveBalance" &
        ",e.SickLeaveBalance" &
        ",e.MaternityLeaveBalance" &
        ",e.LeaveAllowance" &
        ",e.SickLeaveAllowance" &
        ",e.MaternityLeaveAllowance" &
        ",e.LeavePerPayPeriod" &
        ",e.SickLeavePerPayPeriod" &
        ",e.MaternityLeavePerPayPeriod" &
        ",COALESCE(fstat.RowID,3) 'fstatRowID'" &
        ",e.AlphaListExempted" &
        ",e.WorkDaysPerYear" &
        ",CHAR_TO_DAYOFWEEK(e.DayOfRest) 'DayOfRest'" &
        ",e.ATMNo" &
        ",e.BankName" &
        ",IFNULL(e.OtherLeavePerPayPeriod,0) 'OtherLeavePerPayPeriod'" &
        ",IFNULL(e.OtherLeaveAllowance,0) 'OtherLeaveAllowance'" &
        ",IFNULL(e.OtherLeaveBalance,0) 'OtherLeaveBalance'" &
        ",e.CalcHoliday" &
        ",e.CalcSpecialHoliday" &
        ",e.CalcNightDiff" &
        ",e.CalcNightDiffOT" &
        ",e.CalcRestDay" &
        ",e.CalcRestDayOT" &
        ",IFNULL(e.LateGracePeriod,0) AS LateGracePeriod" &
        ",IFNULL(e.RevealInPayroll,1) AS RevealInPayroll" &
        ",IFNULL(e.OffsetBalance,0) AS OffsetBalance" &
        ",IFNULL(ag.AgencyName,'') AS AgencyName" &
        ",IFNULL(ag.RowID,'') AS ag_RowID" &
        ",e.DateEvaluated" &
        ",e.DateRegularized" &
        " " &
        "FROM employee e " &
        "LEFT JOIN user u ON e.CreatedBy=u.RowID " &
        "LEFT JOIN position pos ON e.PositionID=pos.RowID " &
        "LEFT JOIN division d ON d.RowID=pos.DivisionID " &
        "LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID " &
        "LEFT JOIN filingstatus fstat ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents " &
        "LEFT JOIN agency ag ON ag.RowID=e.AgencyID " &
        "WHERE e.OrganizationID=" & orgztnID

    Dim _by As String = "(SELECT CONCAT(CONCAT(UCASE(LEFT(FirstName, 1)), SUBSTRING(FirstName, 2)),' ',CONCAT(UCASE(LEFT(LastName, 1)), SUBSTRING(LastName, 2)))  FROM user WHERE RowID="

    Public q_empldependents As String = "SELECT  edep.RowID, edep.ParentEmployeeID, COALESCE(edep.Salutation,''),  `FirstName`,  COALESCE(edep.MiddleName,''),  edep.LastName,  COALESCE(edep.Surname,''),  edep.RelationToEmployee,  COALESCE(edep.TINNo,''),  COALESCE(edep.SSSNo,''),  COALESCE(edep.HDMFNo,''),  COALESCE(PhilHealthNo,''),  COALESCE(EmailAddress,''),  COALESCE(edep.WorkPhone,''),  COALESCE(edep.HomePhone,''),  COALESCE(edep.MobilePhone,''),  COALESCE(HomeAddress,''),  COALESCE(Nickname,''),  COALESCE(edep.JobTitle,''), COALESCE(edep.Gender,''),  IF(edep.ActiveFlag='Y','TRUE','FALSE'),  COALESCE(DATE_FORMAT(edep.Birthdate,'%m-%d-%Y'),'')," &
        _by & "edep.CreatedBy), DATE_FORMAT(edep.Created,'%m-%d-%Y')," & _by & "edep.LastUpdBy),  COALESCE(DATE_FORMAT(edep.LastUpd,'%m-%d-%Y'),'') FROM employeedependents edep WHERE edep.OrganizationID=" & orgztnID & " AND edep.ParentEmployeeID="

    Public q_salut As String = "SELECT DisplayValue FROM listofval lov WHERE lov.Type='Salutation' AND Active='Yes'"

    Public q_empstat As String = "SELECT DisplayValue FROM listofval lov WHERE lov.Type='Employment Status' AND Active='Yes'"

    Public q_emptype As String = "SELECT DisplayValue" &
        " FROM listofval lov" &
        " WHERE lov.`Type`='Employee Type'" &
        " AND lov.Active='Yes'" &
        " AND lov.DisplayValue IN ('Daily','Monthly','Fixed')" &
        " ORDER BY FIELD(lov.DisplayValue,'Daily','Monthly','Fixed');"

    Public q_maritstat As String = "SELECT DisplayValue FROM listofval lov WHERE lov.Type='Marital Status' AND Active='Yes'"

    Dim employeepix As New DataTable

    Sub loademployee(Optional q_empsearch As String = Nothing)
        PopulateEmployeeGrid()

        emp_rcount = dgvEmp.RowCount

        Static x As SByte = 0
        If x = 0 Then
            x = 1

            Dim dt_indxwithvalue As New DataTable

            Task.Factory.StartNew(Sub()

                                      Dim all_emp As New SQLQueryToDatatable(q_employee)

                                      Dim catchdt As New DataTable
                                      catchdt = all_emp.ResultTable

                                      dt_indxwithvalue.Columns.Add("IndexVal", Type.GetType("System.Int32"))
                                      dt_indxwithvalue.Columns.Add("OutputVal", Type.GetType("System.String"))

                                      Dim colname, empid_str, lname_str As String
                                      colname = "DataColumnName"
                                      Dim i As Integer = 0

                                      For Each dr As DataRow In catchdt.Rows

                                          i = 0

                                          empid_str = Convert.ToString(dr(1).ToString)
                                          lname_str = Convert.ToString(dr(4).ToString)

                                          Simple.Add(empid_str)
                                          Simple.Add(lname_str)

                                          dt_indxwithvalue.Rows.Add(0, empid_str)

                                          dt_indxwithvalue.Rows.Add(3, lname_str)

                                      Next

                                      txtSimple.AutoCompleteCustomSource = Simple

                                      catchdt.Dispose()

                                      ComboBox1.ValueMember = "IndexVal"
                                      ComboBox1.DisplayMember = "OutputVal"
                                      ComboBox1.DataSource = dt_indxwithvalue

                                  End Sub).ContinueWith(Sub()
                                                            txtSimple.Enabled = True
                                                        End Sub, TaskScheduler.FromCurrentSynchronizationContext)
        End If
    End Sub

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
    Dim dontUpdateEmp As SByte = 0

    Async Sub INSUPD_employee_01(sender As Object, e As EventArgs) Handles tsbtnSaveEmp.Click

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        'dgvEmp.CurrentRow.Cells("RowID").Value
        If (tsbtnNewEmp.Enabled = True AndAlso
            (dgvEmp.CurrentRow Is Nothing OrElse
            String.IsNullOrWhiteSpace(dgvEmp.CurrentRow.Cells("RowID").Value))) Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            WarnBalloon("Please select an employee to update.", "No employee selected", lblforballoon, 0, -69)
            Return

        ElseIf (tsbtnNewEmp.Enabled = False AndAlso
        EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE EmployeeID='" &
            Trim(txtEmpID.Text) & "' AND OrganizationID=" & orgztnID & ");") = 1) OrElse
       (tsbtnNewEmp.Enabled = True AndAlso
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
            WarnBalloon("Please input an Employee status.", "Invalid Employee status", lblforballoon, 0, -69)
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
            WarnBalloon("Please input marital status.", "Invalid marital status", lblforballoon, 0, -69)
            Exit Sub
        ElseIf Trim(cboPayFreq.Text) = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            cboPayFreq.Focus()
            WarnBalloon("Please input a pay frequency.", "Invalid Pay Frequency", lblforballoon, 0, -69)
            Exit Sub
        ElseIf String.IsNullOrWhiteSpace(txtWorkDaysPerYear.Text) OrElse ObjectUtils.ToDecimal(txtWorkDaysPerYear.Text) <= 0 Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            txtWorkDaysPerYear.Focus()
            WarnBalloon("Please input a valid work days per year.", "Invalid Work Days per Year", lblforballoon, 0, -69)
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

        If tsbtnNewEmp.Enabled = False Then
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

            If tsbtnNewEmp.Enabled = True Then 'Means update and oldEmployee is needed for UserActivity
                oldEmployee = GetOldEmployee(employee_RowID)

            End If

            new_eRowID =
            INSUPDemployee(employee_RowID,
                           z_User,
                           orgztnID,
                           cboSalut.Text.Trim,
                           txtFName.Text.Trim,
                           txtMName.Text.Trim,
                           txtLName.Text.Trim,
                           txtSName.Text.Trim,
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
                           cboPayFreq.SelectedValue,
                           ValNoComma(txtNumDepen.Text),
                           image_object,
                           0,'ValNoComma(txtvlpayp.Text),
                           0,'ValNoComma(txtslpayp.Text),
                           0,'ValNoComma(txtmlpayp.Text),
                           0,'ValNoComma(txtothrpayp.Text),
                           Convert.ToInt16(chkutflag.Checked),
                           Convert.ToInt16(chkotflag.Checked),
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
                           Convert.ToInt16(chkcalcNightDiffOT.Checked),
                           Convert.ToInt16(chkcalcRestDay.Checked),
                           Convert.ToInt16(chkcalcRestDayOT.Checked),
                           regularizationDate,
                           evaluationDate,
                           (Not chkbxRevealInPayroll.Checked),
                           ValNoComma(txtUTgrace.Text),
                           agensi_rowid,
                           0,
                           GetSelectedBranch()?.RowID,
                           ValNoComma(BPIinsuranceText.Text))
            succeed = new_eRowID IsNot Nothing

            Dim employeeId = If(tsbtnNewEmp.Enabled = False, new_eRowID, employee_RowID)

            'this is during edit
            If if_sysowner_is_benchmark AndAlso employeeId IsNot Nothing Then

                Dim leaveService = MainServiceProvider.GetRequiredService(Of LeaveService)
                Dim newleaveBalance = Await leaveService.
                                            ForceUpdateLeaveAllowanceAsync(
                                                        employeeId:=employeeId,
                                                        organizationId:=z_OrganizationID,
                                                        userId:=z_User,
                                                        selectedLeaveType:=Data.Enums.LeaveType.Vacation,
                                                        newAllowance:=LeaveAllowanceTextBox.Text.ToDecimal)

                LeaveBalanceTextBox.Text = newleaveBalance.ToString("#0.00")
            End If
        Catch ex As Exception
            succeed = False
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

        Dim dgvEmp_RowIndex = 0
        If tsbtnNewEmp.Enabled = False Then 'INSERT employee

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

                Dim repo = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
                repo.RecordAdd(z_User, EmployeeEntityName, employee_RowID, z_OrganizationID)
                InfoBalloon("Employee ID '" & txtEmpID.Text & "' has been created successfully.", "New Employee successfully created", lblforballoon, 0, -69, , 5000)

            End If
        Else 'UPDATE employee

            If dgvEmp.CurrentRow Is Nothing Then Exit Sub

            If dontUpdateEmp = 1 Then
                tsbtnNewEmp.Enabled = True
                AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
                Exit Sub
            End If

            For Each drow As DataRow In employeepix.Rows
                If drow("RowID").ToString = dgvEmp.CurrentRow.Cells("RowID").Value Then
                    drow("Image") = Nothing
                    drow("Image") = image_object

                    Exit For
                End If
            Next

            dgvEmp_RowIndex = dgvEmp.CurrentRow.Index

            If succeed Then
                RecordUpdateEmployee(oldEmployee)
                InfoBalloon("Employee ID '" & txtEmpID.Text & "' has been updated successfully.", "Employee Update Successful", lblforballoon, 0, -69)
            End If

        End If

        With dgvEmp.Rows(dgvEmp_RowIndex)

            If tsbtnNewEmp.Enabled = False Then

                .Cells("RowID").Value = employee_RowID

            End If

            .Cells("Column1").Value = txtEmpID.Text : .Cells("Column2").Value = txtFName.Text
            .Cells("Column3").Value = txtMName.Text : .Cells("Column4").Value = txtLName.Text
            .Cells("Column5").Value = txtNName.Text
            .Cells("Column6").Value = Format(dtpempbdate.Value, machineShortDateFormat) 'dtpBDate.Value

            .Cells("Column8").Value = If(cboPosit.SelectedIndex = -1, "",
                                         If(cboPosit.SelectedIndex = (cboPosit.Items.Count - 1), Nothing, Trim(cboPosit.Text)))

            .Cells("Column9").Value = cboSalut.Text : .Cells("Column10").Value = txtTIN.Text
            .Cells("Column11").Value = txtSSS.Text : .Cells("Column12").Value = txtHDMF.Text
            .Cells("Column13").Value = txtPIN.Text : .Cells("Column15").Value = txtWorkPhne.Text
            .Cells("Column16").Value = txtHomePhne.Text : .Cells("Column17").Value = txtMobPhne.Text
            .Cells("Column18").Value = txtHomeAddr.Text : .Cells("Column14").Value = txtemail.Text
            .Cells("Column19").Value = If(rdMale.Checked, "Male", "Female")
            .Cells("Column20").Value = cboEmpStat.Text : .Cells("Column21").Value = txtSName.Text
            .Cells("Column25").Value = dbnow : .Cells("Column26").Value = u_nem

            .Cells("Column29").Value = cboPosit.SelectedValue

            .Cells("Column22").Value = cboPayFreq.Text

            .Cells("Column31").Value = cboMaritStat.Text : .Cells("Column32").Value = Val(txtNumDepen.Text)
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

            .Cells("Column23").Value = If(chkutflag.Checked, 1, 0)
            .Cells("Column24").Value = If(chkotflag.Checked, 1, 0)

            .Cells("Column1").Selected = True

            .Cells("AlphaExempted").Value = If(chkAlphaListExempt.Checked = True, "0", "1")

            .Cells("WorkDaysPerYear").Value = txtWorkDaysPerYear.Text

            .Cells("DayOfRest").Value = cboDayOfRest.Text

            .Cells("ATMNo").Value = txtATM.Text

            .Cells("BankName").Value = cbobank.Text

            .Cells("OtherLeaveAllowance").Value = txtothrallow.Text

            .Cells("OtherLeaveBalance").Value = txtothrbal.Text

            .Cells("CalcHoliday").Value = Convert.ToInt16(chkcalcHoliday.Checked)
            .Cells("CalcSpecialHoliday").Value = Convert.ToInt16(chkcalcSpclHoliday.Checked)
            .Cells("CalcNightDiff").Value = Convert.ToInt16(chkcalcNightDiff.Checked)
            .Cells("CalcNightDiffOT").Value = Convert.ToInt16(chkcalcNightDiffOT.Checked)
            .Cells("CalcRestDay").Value = Convert.ToInt16(chkcalcRestDay.Checked)
            .Cells("CalcRestDayOT").Value = Convert.ToInt16(chkcalcRestDayOT.Checked)

            .Cells("LateGracePeriod").Value = txtUTgrace.Text
            .Cells("AgencyName").Value = cboAgency.Text

            .Cells("BranchID").Value = GetSelectedBranch()?.RowID
            .Cells("BPIInsuranceColumn").Value = BPIinsuranceText.Text

            SetEmployeeGridDataRow(dgvEmp_RowIndex)

        End With
        tsbtnNewEmp.Enabled = True

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        tsbtnSaveEmp.Enabled = True
    End Sub

    Private Shared Function GetOldEmployee(employee_RowID As Integer?) As Employee

        If employee_RowID.HasValue = False Then Return Nothing

        Dim employeeBuilder = MainServiceProvider.GetRequiredService(Of EmployeeQueryBuilder)

        Return employeeBuilder.
                    IncludePayFrequency().
                    IncludePosition().
                    IncludeBranch().
                    GetById(employee_RowID.Value, z_OrganizationID)

    End Function

    Private Function RecordUpdateEmployee(oldEmployee As Employee) As Boolean

        If oldEmployee Is Nothing Then Return False

        Dim changes = New List(Of UserActivityItem)

        Dim entityName = EmployeeEntityName.ToLower()

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
                        .Description = $"Updated {entityName} ID from '{oldEmployee.EmployeeNo}' to '{txtEmpID.Text}'."
                        })
        End If
        If oldEmployee.EmployeeType <> cboEmpType.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} type from '{oldEmployee.EmployeeType}' to '{cboEmpType.Text}'."
                        })
        End If
        If oldEmployee.EmploymentStatus <> cboEmpStat.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} status from '{oldEmployee.EmploymentStatus}' to '{cboEmpStat.Text}'."
                        })
        End If
        If oldEmployee.StartDate <> dtpempstartdate.Value Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} start date from '{oldEmployee.StartDate.ToShortDateString}' to '{dtpempstartdate.Text}'."
                        })
        End If
        If oldEmployee.Salutation <> cboSalut.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} salutation from '{oldEmployee.Salutation}' to '{cboSalut.Text}'."
                        })
        End If
        If oldEmployee.Gender <> gender Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} gender from '{oldEmployee.Gender}' to '{gender}'."
                        })
        End If
        If oldEmployee.FirstName <> txtFName.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} first name from '{oldEmployee.FirstName}' to '{txtFName.Text}'."
                        })
        End If
        If oldEmployee.MiddleName <> txtMName.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} middle name from '{oldEmployee.MiddleName}' to '{txtMName.Text}'."
                        })
        End If
        If oldEmployee.LastName <> txtLName.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} last name from '{oldEmployee.LastName}' to '{txtLName.Text}'."
                        })
        End If
        If oldEmployee.Surname <> txtSName.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} surname from '{oldEmployee.Surname}' to '{txtSName.Text}'."
                        })
        End If
        If oldEmployee.Nickname <> txtNName.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} nickname from '{oldEmployee.Nickname}' to '{txtNName.Text}'."
                        })
        End If
        If oldEmployee.MaritalStatus <> cboMaritStat.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} marital status from '{oldEmployee.MaritalStatus}' to '{cboMaritStat.Text}'."
                        })
        End If
        If oldEmployee.NoOfDependents.ToString <> txtNumDepen.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} number of dependents from '{oldEmployee.NoOfDependents.ToString}' to '{txtNumDepen.Text}'."
                        })
        End If
        If oldEmployee.PayFrequency.Type <> cboPayFreq.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} pay frequency from '{oldEmployee.PayFrequency.Type}' to '{cboPayFreq.Text}'."
                        })
        End If
        If oldEmployee.DayOfRest Is Nothing And cboDayOfRest.Text <> "" Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} rest day from '' to  '{cboDayOfRest.Text}'."
                        })
        ElseIf oldEmployee.DayOfRest IsNot Nothing And cboDayOfRest.Text <> "" Then
            If WeekdayName(oldEmployee.DayOfRest, False, FirstDayOfWeek.Sunday) <> cboDayOfRest.Text Then
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} rest day from '{WeekdayName(oldEmployee.DayOfRest, False, FirstDayOfWeek.Sunday)}' to '{cboDayOfRest.Text}'."
                        })
            End If
        ElseIf oldEmployee.DayOfRest IsNot Nothing And cboDayOfRest.Text = "" Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} rest day from '{WeekdayName(oldEmployee.DayOfRest, False, FirstDayOfWeek.Sunday)}' to ''."
                        })
        End If
        If oldEmployee.DateEvaluated <> dtpEvaluationDate.Value Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} evaluation date from '{oldEmployee.DateEvaluated?.ToShortDateString}' to '{dtpEvaluationDate.Text}'."
                        })
        End If
        If (oldEmployee.DateEvaluated Is Nothing And dtpEvaluationDate.Checked) Or
            (oldEmployee.DateEvaluated IsNot Nothing And dtpEvaluationDate.Checked = False) Then
            If dtpEvaluationDate.Checked = False Then
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} evaluation date from '{oldEmployee.DateEvaluated?.ToShortDateString}' to ''."
                        })
            Else
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} evaluation date from '' to '{dtpEvaluationDate.Text}'."
                        })
            End If
        End If
        If oldEmployee.DateRegularized <> dtpRegularizationDate.Value Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} regularization date from '{oldEmployee.DateRegularized?.ToShortDateString}' to '{dtpRegularizationDate.Text}'."
                        })
        End If
        If (oldEmployee.DateRegularized Is Nothing And dtpRegularizationDate.Checked) Or
            (oldEmployee.DateRegularized IsNot Nothing And dtpRegularizationDate.Checked = False) Then
            If dtpRegularizationDate.Checked = False Then
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} regularization date from '{oldEmployee.DateRegularized?.ToShortDateString}' to ''."
                        })
            Else
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} regularization date from '' to '{dtpRegularizationDate.Text}'."
                        })
            End If
        End If
        If oldEmployee.AtmNo Is Nothing And txtATM.Text <> "" Then 'change to deposit
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} salary distribution from 'Cash / Check' to 'Direct Deposit'."
                        })
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} ATM number from '' to '{txtATM.Text}'."
                        })
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} bank name from '' to '{cbobank.Text}'."
                        })

        ElseIf oldEmployee.AtmNo <> Nothing And txtATM.Text Is Nothing Then ' change to cash / check
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} salary distribution from 'Direct Deposit' to 'Cash / Check'."
                        })
        Else
            If oldEmployee.AtmNo <> txtATM.Text Then 'change ATM number and Bank Name
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} ATM number from '{oldEmployee.AtmNo}' to '{txtATM.Text}'."
                        })
            End If
            If oldEmployee.BankName <> cbobank.Text Then
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} bank name from '{oldEmployee.BankName}' to '{cbobank.Text}'."
                        })
            End If
        End If
        If (oldEmployee.Branch Is Nothing And BranchComboBox.Text <> "") Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} branch from '' to '{BranchComboBox.Text}'."
                        })
        ElseIf oldEmployee.Branch IsNot Nothing And BranchComboBox.Text <> "" Then
            If oldEmployee.Branch.Name <> BranchComboBox.Text Then
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} branch from '{oldEmployee.Branch.Name}' to '{BranchComboBox.Text}'."
                        })
            End If
        End If
        If oldEmployee.BPIInsurance <> BPIinsuranceText.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} BPI insurance from '{oldEmployee.BPIInsurance.ToString}' to '{BPIinsuranceText.Text}'."
                        })
        End If
        'If oldEmployee.Position.Division.Name <> txtDivisionName.Text Then
        '    changes.Add(New UserActivityItem() With
        '                {
        '                .EntityId = oldEmployee.RowID,
        '                .Description = $"Updated employee division from '{oldEmployee.Position.Division.Name}' to '{txtDivisionName.Text}'."
        '                })
        'End If
        If oldEmployee.Position.Name <> cboPosit.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} position from '{oldEmployee.Position.Name}' to '{cboPosit.Text}'."
                        })
        End If
        'If oldEmployee.Agency.Name <> cboAgency.Text Then
        '    changes.Add(New UserActivityItem() With
        '                {
        '                .EntityId = oldEmployee.RowID,
        '                .Description = $"Updated employee agency from '{oldEmployee.Agency.Name'} to '{cboAgency.Text}'."
        '                })
        'End If
        If oldEmployee.BirthDate <> dtpempbdate.Value Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} birthday from '{oldEmployee.BirthDate.ToShortDateString}' to '{dtpempbdate.Text}'."
                        })
        End If
        If oldEmployee.EmailAddress <> txtemail.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} email address from '{oldEmployee.EmailAddress}' to '{txtemail.Text}'."
                        })
        End If
        If oldEmployee.TinNo <> txtTIN.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} TIN from '{oldEmployee.TinNo}' to '{txtTIN.Text}'."
                        })
        End If
        If oldEmployee.SssNo <> txtSSS.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} SSS from '{oldEmployee.SssNo}' to '{txtSSS.Text}'."
                        })
        End If
        If oldEmployee.PhilHealthNo <> txtPIN.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} PhilHealth from '{oldEmployee.PhilHealthNo}' to '{txtPIN.Text}'."
                        })
        End If
        If oldEmployee.HdmfNo <> txtHDMF.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} PagIbig from '{oldEmployee.HdmfNo}' to '{txtHDMF.Text}'."
                        })
        End If
        If oldEmployee.HomeAddress <> txtHomeAddr.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} home address from '{oldEmployee.HomeAddress}' to '{txtHomeAddr.Text}'."
                        })
        End If
        If oldEmployee.WorkPhone <> txtWorkPhne.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} work phone from '{oldEmployee.WorkPhone}' to '{txtWorkPhne.Text}'."
                        })
        End If
        If oldEmployee.HomePhone <> txtHomePhne.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} home phone from '{oldEmployee.HomePhone}' to '{txtHomePhne.Text}'."
                        })
        End If
        If oldEmployee.MobilePhone <> txtMobPhne.Text Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} mobile phone from '{oldEmployee.MobilePhone}' to '{txtMobPhne.Text}'."
                        })
        End If
        If oldEmployee.OvertimeOverride <> chkotflag.Checked Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} calculate overtime from '{oldEmployee.OvertimeOverride.ToString}' to '{chkotflag.Checked.ToString}'."
                        })
        End If
        If oldEmployee.UndertimeOverride <> chkutflag.Checked Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} calculate undertime from '{oldEmployee.UndertimeOverride.ToString}' to '{chkutflag.Checked.ToString}'."
                        })
        End If
        If oldEmployee.LateGracePeriod <> txtUTgrace.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} grace period from '{oldEmployee.LateGracePeriod.ToString}' to '{txtUTgrace.Text}'."
                        })
        End If
        'If oldEmployee.AlphalistExempted <> chkAlphaListExempt.Checked Then
        '    changes.Add(New UserActivityItem() With
        '                {
        '                .EntityId = oldEmployee.RowID,
        '                .Description = $"Updated employee alpha list exemption from '{oldEmployee.AlphalistExempted.ToString}' to '{chkAlphaListExempt.Checked.ToString}'."
        '                })
        'End If
        If oldEmployee.WorkDaysPerYear <> txtWorkDaysPerYear.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} work days per year from '{oldEmployee.WorkDaysPerYear.ToString}' to '{txtWorkDaysPerYear.Text}'."
                        })
        End If
        If oldEmployee.CalcHoliday <> chkcalcHoliday.Checked Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} calculate holiday from '{oldEmployee.CalcHoliday.ToString}' to '{chkcalcHoliday.Checked.ToString}'."
                        })
        End If
        If oldEmployee.CalcSpecialHoliday <> chkcalcSpclHoliday.Checked Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} calculate special holiday from '{oldEmployee.CalcSpecialHoliday.ToString}' to '{chkcalcSpclHoliday.Checked.ToString}'."
                        })
        End If
        If oldEmployee.CalcNightDiff <> chkcalcNightDiff.Checked Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} calculate night differential from '{oldEmployee.CalcNightDiff.ToString}' to '{chkcalcNightDiff.Checked.ToString}'."
                        })
        End If
        If oldEmployee.CalcRestDay <> chkcalcRestDay.Checked Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} calculate rest day from '{oldEmployee.CalcRestDay.ToString}' to '{chkcalcRestDay.Checked.ToString}'."
                        })
        End If
        'If oldEmployee.CalcNightDiffOT <> chkcalcNightDiffOT.Checked Then
        '    changes.Add(New UserActivityItem() With
        '                {
        '                .EntityId = oldEmployee.RowID,
        '                .Description = $"Updated employee calculate night diferential overtime from '{oldEmployee.CalcNightDiffOT.ToString}' to '{chkcalcNightDiffOT.Checked.ToString}'."
        '                })
        'End If
        'If oldEmployee.CalcRestDayOT <> chkcalcRestDayOT.Checked Then
        '    changes.Add(New UserActivityItem() With
        '                {
        '                .EntityId = oldEmployee.RowID,
        '                .Description = $"Updated employee calculate rest day overtime from '{oldEmployee.CalcRestDayOT.ToString}' to '{chkcalcRestDayOT.Checked.ToString}'."
        '                })
        'End If
        If oldEmployee.VacationLeaveAllowance <> txtvlallow.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} vacation leave allowance from '{oldEmployee.VacationLeaveAllowance.ToString("#0")}' to '{txtvlallow.Text}'."
                        })
        End If
        If oldEmployee.SickLeaveAllowance <> txtslallow.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} sick leave allowance from '{oldEmployee.SickLeaveAllowance.ToString}' to '{txtslallow.Text}'."
                        })
        End If
        If oldEmployee.MaternityLeaveAllowance <> txtmlallow.Text.ToDecimal Then
            If gender = "F" Then
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} maternity leave allowance from '{oldEmployee.MaternityLeaveAllowance.ToString}' to '{txtmlallow.Text}'."
                        })
            ElseIf gender = "M" Then
                changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} paternity leave allowance from '{oldEmployee.MaternityLeaveAllowance.ToString}' to '{txtmlallow.Text}'."
                        })
            End If
        End If
        If oldEmployee.OtherLeaveAllowance <> txtothrallow.Text.ToDecimal Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} other leave allowance from '{oldEmployee.OtherLeaveAllowance.ToString}' to '{txtothrallow.Text}'."
                        })
        End If
        If oldEmployee.RevealInPayroll <> Not chkbxRevealInPayroll.Checked Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldEmployee.RowID,
                        .Description = $"Updated {entityName} hide in payroll from '{(Not oldEmployee.RevealInPayroll).ToString}' to '{chkbxRevealInPayroll.Checked.ToString}'."
                        })
        End If

        If changes.Count > 0 Then
            Dim repo = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
            repo.CreateRecord(z_User, EmployeeEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)
            Return True
        End If

        Return False
    End Function

    Private Async Sub SetEmployeeGridDataRow(rowIndex As Integer)
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
    End Sub

    Sub tsbtnSaveEmp_Click(sender As Object, e As EventArgs) 'Handles tsbtnSaveEmp.Click
        If tsbtnSaveEmp.Visible = False Then : Exit Sub : End If
        Static isDupEmpID As SByte
        If isDupEmpID = 0 And tsbtnNewEmp.Enabled = False Then
            For Each r As DataGridViewRow In dgvEmp.Rows
                If txtEmpID.Text?.ToUpper() = r.Cells("Column1").Value.ToString()?.ToUpper() Then : isDupEmpID = 1 : Exit For : Else : isDupEmpID = 0 : End If
            Next
        End If

        If isDupEmpID = 1 And tsbtnNewEmp.Enabled = False Then
            txtEmpID.Focus()
            WarnBalloon("Employee ID " & txtEmpID.Text & " is already exist, please try another.", "Invalid Employee ID", txtEmpID, txtEmpID.Width - 16, -69) : isDupEmpID = 0 : Exit Sub
        ElseIf txtFName.Text.Trim() = "" Then
            txtFName.Focus()
            WarnBalloon("Please input First name", "Invalid First name", txtFName, txtFName.Width - 16, -69) : Exit Sub
        ElseIf txtLName.Text.Trim() = "" Then
            txtLName.Focus()
            WarnBalloon("Please input Last name", "Invalid Last name", txtLName, txtLName.Width - 16, -69) : Exit Sub
        ElseIf cboMaritStat.Text = "" Or cboMaritStat.SelectedIndex = -1 Then
            cboMaritStat.Focus()
            WarnBalloon("Please input a Marital Status", "Invalid Marital Status", cboMaritStat, cboMaritStat.Width - 16, -69) : Exit Sub
        End If

        Dim _gend = If(rdMale.Checked, "M", "F")

        'If EXECQUER("SELECT EXISTS(SELECT RowID FROM listofval lov WHERE lov.Type='Salutation' AND Active='Yes' AND DisplayValue='" & cboSalut.Text & "')") = 0 And cboSalut.Text <> "" Then
        '    INS_LoL(cboSalut.Text, cboSalut.Text, "Salutation", , "Yes", , , 1) : cboSalut.Items.Add(cboSalut.Text)
        'End If

        If tsbtnNewEmp.Enabled = True Then
            If dgvEmp.RowCount <> 0 Then
                With dgvEmp.CurrentRow
                    Dim _positID = If(positID = "", ",PositionID=NULL", ",PositionID=" & positID)
                    Dim _freqID = If(payFreqID = "", ",PayFrequencyID=NULL", ",PayFrequencyID='" & payFreqID & "'")

                    Dim prevMaritStat As String = .Cells("Column31").Value 'cboMaritStat.Text
                    Dim prevNoOfDepen As String = Val(.Cells("Column32").Value) 'txtNumDepen.Text
                    'Format(dtpBDate.Value, "yyyy-MM-dd")
                    EXECQUER("UPDATE employee SET " &
                    "EmployeeID='" & Trim(txtEmpID.Text) &
                    "',FirstName='" & txtFName.Text &
                    "',MiddleName='" & txtMName.Text &
                    "',LastName='" & txtLName.Text &
                    "',Surname='" & txtSName.Text &
                    "',Nickname='" & txtNName.Text &
                    "',Birthdate='" & empBDate &
                    "',JobTitle='" & Trim(txtDivisionName.Text) &
                    "',Salutation='" & cboSalut.Text &
                    "',TINNo='" & txtTIN.Text &
                    "',SSSNo='" & txtSSS.Text &
                    "',HDMFNo='" & txtHDMF.Text &
                    "',PhilHealthNo='" & txtPIN.Text &
                    "',WorkPhone='" & txtWorkPhne.Text &
                    "',HomePhone='" & txtHomePhne.Text &
                    "',MobilePhone='" & txtMobPhne.Text &
                    "',HomeAddress='" & txtHomeAddr.Text &
                    "',EmailAddress='" & txtemail.Text &
                    "',Gender='" & _gend &
                    "',EmploymentStatus='" & cboEmpStat.Text &
                    "',MaritalStatus='" & cboMaritStat.Text &
                    "',NoOfDependents=" & Val(txtNumDepen.Text) &
                    ",EmployeeType='" & cboEmpType.Text &
                    "',LastUpd=CURRENT_TIMESTAMP()" &
                    ",LastUpdBy=1" & _positID & _freqID &
                    " WHERE RowID='" & .Cells("RowID").Value & "'")

                    If hasERR = 0 Then 'ito yung error sa EXECQUER() dun sa myModule.vb
                        .Cells("Column1").Value = txtEmpID.Text : .Cells("Column2").Value = txtFName.Text
                        .Cells("Column3").Value = txtMName.Text : .Cells("Column4").Value = txtLName.Text
                        .Cells("Column5").Value = txtNName.Text : .Cells("Column6").Value = Format(CDate(empBDate), machineShortDateFormat) 'dtpBDate.Value
                        .Cells("Column7").Value = Trim(txtDivisionName.Text) : .Cells("Column8").Value = cboPosit.Text
                        .Cells("Column9").Value = cboSalut.Text : .Cells("Column10").Value = txtTIN.Text
                        .Cells("Column11").Value = txtSSS.Text : .Cells("Column12").Value = txtHDMF.Text
                        .Cells("Column13").Value = txtPIN.Text : .Cells("Column15").Value = txtWorkPhne.Text
                        .Cells("Column16").Value = txtHomePhne.Text : .Cells("Column17").Value = txtMobPhne.Text
                        .Cells("Column18").Value = txtHomeAddr.Text : .Cells("Column14").Value = txtemail.Text
                        .Cells("Column19").Value = If(_gend = "M", "Male", "Female")
                        .Cells("Column20").Value = cboEmpStat.Text : .Cells("Column21").Value = txtSName.Text
                        .Cells("Column27").Value = dbnow : .Cells("Column28").Value = u_nem

                        .Cells("Column29").Value = If(cboPosit.SelectedIndex = -1 Or cboPosit.Text = "", "",
                                                      getStrBetween(positn.Item(cboPosit.SelectedIndex), "", "@"))

                        .Cells("Column31").Value = cboMaritStat.Text : .Cells("Column32").Value = Val(txtNumDepen.Text)
                        .Cells("Column34").Value = cboEmpType.Text

                        If prevMaritStat <> .Cells("Column31").Value And
                           prevNoOfDepen <> .Cells("Column32").Value Then

                            MsgBox("INSERT Row employeesalary")
                            'INSERT Row employeesalary
                        Else
                            If prevMaritStat <> .Cells("Column31").Value Or
                               prevNoOfDepen <> .Cells("Column32").Value Then

                                MsgBox("INSERT Row employeesalary")
                                'INSERT Row employeesalary
                            End If
                        End If
                    End If

                    'enlistToCboBox(q_salut, cboSalut) : salutn_count = cboSalut.Items.Count
                    '"Surname" = "Column21" : "PayFrequency" = "Column22"
                    '"UndertimeOverride" = "Column23" : "OvertimeOverride" = "Column24"
                End With
                InfoBalloon("Employee ID '" & txtEmpID.Text & "' has successfully updated.", "Employee Update Successful", lblforballoon, 0, -69)
            End If
        Else 'Format(dtpBDate.Value, "yyyy-MM-dd")

            Dim _RowID = INS_employee(txtEmpID.Text,
                                cboEmpStat.Text, _gend, Trim(txtDivisionName.Text), positID, cboSalut.Text, txtFName.Text,
                                txtMName.Text,
                                txtLName.Text, txtNName.Text,
                                empBDate,
                                txtTIN.Text, txtSSS.Text, txtHDMF.Text, txtPIN.Text, txtemail.Text,
                                txtWorkPhne.Text, txtHomePhne.Text,
                                txtMobPhne.Text, txtHomeAddr.Text, payFreqID, , , txtSName.Text,
                                cboMaritStat.Text, Val(txtNumDepen.Text), 0, cboEmpType.Text)
            _EmpRowID = _RowID
            RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            'enlistToCboBox(q_salut, cboSalut) : salutn_count = cboSalut.Items.Count

            If hasERR = 0 Then
                If emp_rcount = 0 Then
                    dgvEmp.Rows.Add()
                Else : dgvEmp.Rows.Insert(0, 1)
                End If

                emp_rcount += 1 : isDupEmpID = 0

                With dgvEmp.Rows(0)
                    .Cells("RowID").Value = _RowID
                    .Cells("Column1").Value = txtEmpID.Text : .Cells("Column2").Value = txtFName.Text
                    .Cells("Column3").Value = txtMName.Text : .Cells("Column4").Value = txtLName.Text
                    .Cells("Column5").Value = txtNName.Text : .Cells("Column6").Value = Format(CDate(empBDate), machineShortDateFormat) 'dtpBDate.Value
                    .Cells("Column7").Value = Trim(txtDivisionName.Text) : .Cells("Column8").Value = cboPosit.Text
                    .Cells("Column9").Value = cboSalut.Text : .Cells("Column10").Value = txtTIN.Text
                    .Cells("Column11").Value = txtSSS.Text : .Cells("Column12").Value = txtHDMF.Text
                    .Cells("Column13").Value = txtPIN.Text : .Cells("Column15").Value = txtWorkPhne.Text
                    .Cells("Column16").Value = txtHomePhne.Text : .Cells("Column17").Value = txtMobPhne.Text
                    .Cells("Column18").Value = txtHomeAddr.Text : .Cells("Column14").Value = txtemail.Text
                    .Cells("Column19").Value = If(_gend = "M", "Male", "Female")
                    .Cells("Column20").Value = cboEmpStat.Text : .Cells("Column21").Value = txtSName.Text
                    .Cells("Column25").Value = dbnow : .Cells("Column26").Value = u_nem

                    .Cells("Column29").Value = If(cboPosit.SelectedIndex = -1 Or cboPosit.Text = "", "",
                                                  getStrBetween(positn.Item(cboPosit.SelectedIndex), "", "@"))

                    .Cells("Column31").Value = cboMaritStat.Text : .Cells("Column32").Value = Val(txtNumDepen.Text)
                    .Cells("Column34").Value = cboEmpType.Text

                End With

                InfoBalloon("Employee ID '" & txtEmpID.Text & "' has successfully created.", "New Employee successfully created", lblforballoon, 0, -69)
            End If

            dgvEmp_SelectionChanged(sender, e) : AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
        End If

        cboEmpStat.Enabled = True
        tsbtnNewEmp.Enabled = True : loadPositName()

    End Sub

    Private Sub Employee_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim result = Windows.Forms.DialogResult.Yes

        Dim prompt = Nothing

        Select Case tabIndx
            Case GetEmployeeProfileTabPageIndex()
                If tsbtnNewEmp.Enabled = False Or
                    listofEditDepen.Count <> 0 Then
                End If
            Case GetDisciplinaryActionTabPageIndex()
                If btnNew.Enabled = False Then
                End If
            Case GetEducationalBackgroundTabPageIndex()
                If btnNewEduc.Enabled = False Then
                End If
            Case GetPreviousEmployerTabPageIndex()
                If btnNewPrevEmp.Enabled = False Then
                End If

        End Select

        If result = DialogResult.Cancel Then
            e.Cancel = True
        ElseIf result = DialogResult.No Then
            e.Cancel = True
        ElseIf result = DialogResult.Yes Then
            e.Cancel = False

            RemoveHandler txtBDate.Leave, AddressOf txtBDate_Leave
            InfoBalloon(, , lblforballoon1, , , 1)

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

        End If

        If e.Cancel = False Then

            Dim liveThreads = threadArrayList.Cast(Of Thread).Where(Function(i) i.IsAlive)

            For Each ilist As Thread In liveThreads
                'If ilist.IsAlive Then
                ilist.Abort()
                'End If
            Next

            threadArrayList.Clear()

        End If
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

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT RowID,PositionName" &
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

    Function payp_count(Optional PayFreqRowID As Object = Nothing) As Integer
        Dim params(1, 2) As Object

        If dgvEmp.RowCount <> 0 Then
            PayFreqRowID = dgvEmp.CurrentRow.Cells("Column30").Value
        Else
            PayFreqRowID = 1
        End If

        params(0, 0) = "organization_ID"
        params(1, 0) = "PayFrequencyID"

        params(0, 1) = orgztnID
        params(1, 1) = PayFreqRowID

        Dim _divisor = EXEC_INSUPD_PROCEDURE(params,
                                              "COUNT_payperiodthisyear",
                                              "payp_count")
        Return CInt(_divisor)
    End Function

    Dim view_ID As Object
    Dim paytypestring As String

    Private Sub Employee_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        if_sysowner_is_benchmark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark
        if_sysowner_is_laglobal = _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.LAGlobal

        If if_sysowner_is_benchmark Then

            'only salary and employee tabs should be visible

            tabctrlemp.TabPages.Remove(tbpempchklist)
            tabctrlemp.TabPages.Remove(tbpAwards)
            tabctrlemp.TabPages.Remove(tbpCertifications)
            tabctrlemp.TabPages.Remove(tbpDiscipAct)
            tabctrlemp.TabPages.Remove(tbpEducBG)
            tabctrlemp.TabPages.Remove(tbpPrevEmp)
            tabctrlemp.TabPages.Remove(tbpPromotion)
            tabctrlemp.TabPages.Remove(tbpBonus)
            tabctrlemp.TabPages.Remove(tbpAttachment)

            TabControl3.Visible = False
            LeaveGroupBox.Visible = True

        End If

        If dbnow = Nothing Then
            dbnow = EXECQUER(CURDATE_MDY)
        End If

        previousForm = Me
        'dbconn()
        view_ID = VIEW_privilege("Employee Personal Profile", orgztnID)

        loademployee()

        u_nem = EXECQUER(USERNameStrPropr & z_User)

        paytypestring = EXECQUER("SELECT PayFrequencyType FROM payfrequency pfq LEFT JOIN organization org ON org.PayFrequencyID=pfq.RowID WHERE org.RowID='" & orgztnID & "' LIMIT 1;")

        PrepareFormForUserLevelAuthorizations()

        InitializeLaGlobalReportList()

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
    End Sub

    Private Async Sub PrepareFormForUserLevelAuthorizations()

        Dim userRepository = MainServiceProvider.GetRequiredService(Of UserRepository)
        Dim user = Await userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
        End If

        If _policy.UseUserLevel = False Then

            Return

        End If

        If user.UserLevel = UserLevel.Four OrElse user.UserLevel = UserLevel.Five Then

            tabctrlemp.TabPages.Remove(tbpempchklist)
            tabctrlemp.TabPages.Remove(tbpAwards)
            tabctrlemp.TabPages.Remove(tbpCertifications)
            tabctrlemp.TabPages.Remove(tbpEducBG)
            tabctrlemp.TabPages.Remove(tbpPrevEmp)
            tabctrlemp.TabPages.Remove(tbpPromotion)
            tabctrlemp.TabPages.Remove(tbpDiscipAct)
            tabctrlemp.TabPages.Remove(tbpNewSalary)
            tabctrlemp.TabPages.Remove(tbpBonus)
            tabctrlemp.TabPages.Remove(tbpAttachment)

        End If

    End Sub

    Private Sub Employee_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd

        InfoBalloon(, , lblforballoon1, , , 1)

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

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click, tsbtnCloseempawar.Click, tsbtnCloseempcert.Click,
                                                                    btnClose.Click, ToolStripButton5.Click, ToolStripButton13.Click,
                                                                   ToolStripButton18.Click,
                                                                   ToolStripButton2.Click,
                                                                   ToolStripButton11.Click,
                                                                   ToolStripButton16.Click 'ToolStripButton12.Click
        Me.Close()
    End Sub

    Private Sub txt_Leave(sender As Object, e As EventArgs) Handles txtFName.Leave, txtFName.Leave, txtHomeAddr.Leave,
                                                                    txtLName.Leave, txtMName.Leave, txtNName.Leave, txtSName.Leave ', txtJTtle.Leave
        'With DirectCast(sender, TextBox)
        '    .Text = StrConv(.Text, VbStrConv.ProperCase)
        'End With
    End Sub

    Dim PositE_asc As String

    Private Sub cboPosit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboPosit.KeyPress
        e.Handled = True
    End Sub

    Private Async Sub cboPosit_SelectedIndexChanged_1Async(sender As Object, e As EventArgs) Handles cboPosit.SelectedIndexChanged
        If Not tsbtnNewEmp.Enabled Then
            Dim positionId = cboPosit.SelectedValue
            Dim divisionName = String.Empty

            Dim positionRepository = MainServiceProvider.GetRequiredService(Of PositionRepository)

            Dim position = Await positionRepository.GetByIdWithDivisionAsync(positionId)
            If position IsNot Nothing Then
                divisionName = position.Division.Name
            End If
            txtDivisionName.Text = divisionName
        End If
    End Sub

    Private Sub cboPosit_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cboPosit.SelectedIndexChanged ', cboPosit.SelectedValueChanged

        positID = cboPosit.SelectedValue
        Dim n_ExecuteQuery As _
            New ExecuteQuery("SELECT d.GracePeriod FROM `division` d INNER JOIN position pos ON pos.RowID='" & positID & "' AND d.RowID=pos.DivisionId;")
        txtUTgrace.Text = ValNoComma(n_ExecuteQuery.Result)

    End Sub

    Dim noCurrCellChange As SByte
    Dim EmployeeImage As Image

    Dim employeefullname As String = Nothing
    Dim subdetails As String = Nothing
    Dim sameEmpID As Integer = -1
    Dim LastFirstMidName As String = Nothing
    Dim publicEmpRowID = Nothing

    Async Sub dgvEmp_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvEmp.SelectionChanged

        RemoveHandler cboPosit.SelectedIndexChanged, AddressOf cboPosit_SelectedIndexChanged
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
                    txtSName.Text = If(IsDBNull(.Cells("Column21").Value), "", .Cells("Column21").Value)

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
                    employeefullname = employeefullname & If(IsDBNull(.Cells("Column21").Value),
                                                             "",
                                                             "-" & StrConv(.Cells("Column21").Value,
                                                                           VbStrConv.ProperCase))
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

                If selectedTab Is tbpempchklist Then

                    txtEmpIDChk.Text = subdetails '"ID# " & .Cells("Column1").Value

                    txtFNameChk.Text = employeefullname
                    pbEmpPicChk.Image = Nothing
                    pbEmpPicChk.Image = EmployeeImage
                    lblyourrequirement.Text = .Cells("Column2").Value & "'s requirements"
                    VIEW_employeechecklist(.Cells("RowID").Value)

                ElseIf selectedTab Is tbpEmployee Then 'Employee

                    SetEmployee()

                ElseIf selectedTab Is tbpAwards Then
                    Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                    Dim employee = GetCurrentEmployeeEntity(employeeID)

                    Await AwardTab.SetEmployee(employee)

                ElseIf selectedTab Is tbpCertifications Then
                    Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                    Dim employee = GetCurrentEmployeeEntity(employeeID)

                    Await CertificationTab.SetEmployee(employee)

                ElseIf selectedTab Is tbpDiscipAct Then
                    Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                    Dim employee = GetCurrentEmployeeEntity(employeeID)

                    Await DisciplinaryActionTab.SetEmployee(employee)

                ElseIf selectedTab Is tbpEducBG Then
                    Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                    Dim employee = GetCurrentEmployeeEntity(employeeID)

                    Await EducationalBackgroundTab.SetEmployee(employee)

                ElseIf selectedTab Is tbpPrevEmp Then
                    Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                    Dim employee = GetCurrentEmployeeEntity(employeeID)

                    Await PreviousEmployerTab.SetEmployee(employee)

                ElseIf selectedTab Is tbpPromotion Then 'Promotion
                    Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                    Dim employee = GetCurrentEmployeeEntity(employeeID)

                    Await PromotionTab.SetEmployee(employee)

                ElseIf selectedTab Is tbpBonus Then 'Bonus
                    Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                    Dim employee = GetCurrentEmployeeEntity(employeeID)

                    Await BonusTab.SetEmployee(employee)

                ElseIf selectedTab Is tbpAttachment Then 'Attachment
                    Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                    Dim employee = GetCurrentEmployeeEntity(employeeID)

                    Await AttachmentTab.SetEmployee(employee, Me)

                ElseIf selectedTab Is tbpNewSalary Then

                    'transferred here so this function will not fetch data from database
                    'even if the other tabs dont need the employee entity
                    Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                    Dim employee = GetCurrentEmployeeEntity(employeeID)

                    Await SalaryTab.SetEmployee(employee)
                End If

                If txtdgvDepen IsNot Nothing Then
                    RemoveHandler txtdgvDepen.KeyDown, AddressOf dgvDepen_KeyDown
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
                    dgvDepen.Rows.Clear()

                    clearObjControl(SplitContainer2.Panel1)
                    clearObjControl(tbpleaveallow)
                    clearObjControl(tbpleavebal)
                    chkutflag.Checked = 0
                    chkotflag.Checked = 0
                    listofEditDepen.Clear()

            End Select
        End If
        listofEditDepen.Clear()
    End Sub

    Private Sub SetEmployee()
        txtNName.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column5").Value), "", dgvEmp.CurrentRow.Cells("Column5").Value)
        txtDivisionName.Text = dgvEmp.CurrentRow.Cells("Column7").Value

        If dgvEmp.CurrentRow.Cells("Column6").Value Is Nothing Then
            dtpempbdate.Value = Format(CDate(dbnow), machineShortDateFormat)
        Else
            dtpempbdate.Value = Format(CDate(dgvEmp.CurrentRow.Cells("Column6").Value), machineShortDateFormat)
        End If

        txtTIN.Text = dgvEmp.CurrentRow.Cells("Column10").Value : txtSSS.Text = dgvEmp.CurrentRow.Cells("Column11").Value
        txtHDMF.Text = dgvEmp.CurrentRow.Cells("Column12").Value : txtPIN.Text = dgvEmp.CurrentRow.Cells("Column13").Value
        txtWorkPhne.Text = dgvEmp.CurrentRow.Cells("Column15").Value : txtHomePhne.Text = dgvEmp.CurrentRow.Cells("Column16").Value
        txtMobPhne.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column17").Value), "", dgvEmp.CurrentRow.Cells("Column17").Value) : txtHomeAddr.Text = dgvEmp.CurrentRow.Cells("Column18").Value
        txtemail.Text = dgvEmp.CurrentRow.Cells("Column14").Value

        Dim payFrequency = _payFrequencies.Where(Function(p) p.Type = dgvEmp.CurrentRow.Cells("Column22").Value).FirstOrDefault
        If payFrequency Is Nothing Then
            cboPayFreq.SelectedIndex = -1
        Else
            cboPayFreq.SelectedIndex = _payFrequencies.IndexOf(payFrequency)
        End If

        RemoveHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged

        If dgvEmp.CurrentRow.Cells("Column20").Value = "" Then
            cboEmpStat.SelectedIndex = -1
            cboEmpStat.Text = ""
        Else
            cboEmpStat.Text = dgvEmp.CurrentRow.Cells("Column20").Value
        End If

        reloadPositName(dgvEmp.CurrentRow.Cells("Column29").Value)  ': cboPosit.Text = dgvEmp.CurrentRow.Cells("Column8").Value

        cboPosit.Text = dgvEmp.CurrentRow.Cells("Column8").Value

        AddHandler cboPosit.SelectedIndexChanged, AddressOf cboPosit_SelectedIndexChanged

        If IsDBNull(dgvEmp.CurrentRow.Cells("Column9").Value) OrElse dgvEmp.CurrentRow.Cells("Column9").Value = "" Then
            cboSalut.SelectedIndex = -1
            cboSalut.Text = ""
        Else
            cboSalut.Text = dgvEmp.CurrentRow.Cells("Column9").Value
        End If
        '"UndertimeOverride" = "Column23" : "OvertimeOverride" = "Column24"
        '"Creation date" = "Column25" : "Created by" = "Column26"
        '"Last update" = "Column27" : "Last Update by" = "Column28"
        If dgvEmp.CurrentRow.Cells("Column31").Value = "" Then
            cboMaritStat.SelectedIndex = -1
            cboMaritStat.Text = ""
        Else
            cboMaritStat.Text = dgvEmp.CurrentRow.Cells("Column31").Value
        End If

        cboEmpType.SelectedIndex = -1
        cboEmpType.Text = ""

        cboEmpType.Text = dgvEmp.CurrentRow.Cells("Column34").Value

        txtNumDepen.Text = Val(dgvEmp.CurrentRow.Cells("Column32").Value)

        Dim radioGender As RadioButton
        If dgvEmp.CurrentRow.Cells("Column19").Value = "Male" Then
            rdMale.Checked = True
            radioGender = rdMale
        Else
            rdFMale.Checked = True
            radioGender = rdFMale
        End If

        Gender_CheckedChanged(radioGender, New EventArgs)

        noCurrCellChange = 0
        dtpempstartdate.Value = CDate(dgvEmp.CurrentRow.Cells("colstartdate").Value) '.ToString.Replace("-", "/")

        pbemppic.Image = Nothing
        pbemppic.Image = EmployeeImage
        txtEmpID.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column1").Value), "", dgvEmp.CurrentRow.Cells("Column1").Value)
        txtFName.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column2").Value), "", dgvEmp.CurrentRow.Cells("Column2").Value)
        txtMName.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column3").Value), "", dgvEmp.CurrentRow.Cells("Column3").Value)
        txtLName.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column4").Value), "", dgvEmp.CurrentRow.Cells("Column4").Value)
        txtSName.Text = If(IsDBNull(dgvEmp.CurrentRow.Cells("Column21").Value), "", dgvEmp.CurrentRow.Cells("Column21").Value)

        Dim case_one As Integer = -1
        If case_one <> sameEmpID Then
            case_one = sameEmpID
            VIEW_employeedependents(dgvEmp.CurrentRow.Cells("RowID").Value)
            dependentitemcount = dgvDepen.RowCount - 1
        ElseIf case_one = sameEmpID Then
            VIEW_employeedependents(dgvEmp.CurrentRow.Cells("RowID").Value)
        Else
            If dgvDepen.RowCount = 1 Then
            Else

                If dgvDepen.CurrentRow.Index >= dependentitemcount Then
                    ''MsgBox("If")
                    If dependentitemcount = -1 Then
                        VIEW_employeedependents(dgvEmp.CurrentRow.Cells("RowID").Value)
                        dependentitemcount = dgvDepen.RowCount - 1
                    End If
                Else
                    ''MsgBox("else")
                    dgvDepen.Rows.Clear()
                    VIEW_employeedependents(dgvEmp.CurrentRow.Cells("RowID").Value)
                    dependentitemcount = dgvDepen.RowCount - 1
                End If

            End If
        End If

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

        chkutflag.Checked = If(dgvEmp.CurrentRow.Cells("Column23").Value = 1, True, False)
        chkotflag.Checked = If(dgvEmp.CurrentRow.Cells("Column24").Value = 1, True, False)

        chkAlphaListExempt.Checked = If(dgvEmp.CurrentRow.Cells("AlphaExempted").Value = 0, True, False)
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
        chkcalcNightDiffOT.Checked = Convert.ToInt16(dgvEmp.CurrentRow.Cells("CalcNightDiffOT").Value) 'If(dgvEmp.CurrentRow.Cells("CalcNightDiffOT").Value = "Y", True, False)
        chkcalcRestDay.Checked = Convert.ToInt16(dgvEmp.CurrentRow.Cells("CalcRestDay").Value) 'If(dgvEmp.CurrentRow.Cells("CalcRestDay").Value = "Y", True, False)
        chkcalcRestDayOT.Checked = Convert.ToInt16(dgvEmp.CurrentRow.Cells("CalcRestDayOT").Value) 'If(dgvEmp.CurrentRow.Cells("CalcRestDayOT").Value = "Y", True, False)

        chkbxRevealInPayroll.Checked =
            (Not CBool(Convert.ToInt16(dgvEmp.CurrentRow.Cells("RevealInPayroll").Value)))

        txtUTgrace.Text = dgvEmp.CurrentRow.Cells("LateGracePeriod").Value 'AgencyName
        cboAgency.Text = dgvEmp.CurrentRow.Cells("AgencyName").Value

        Dim dataRow = DirectCast(dgvEmp.CurrentRow.Tag, DataRow)
        If dataRow IsNot Nothing Then
            Dim hasDateEvaluated = Not IsDBNull(dataRow("DateEvaluated"))
            dtpEvaluationDate.Value = If(hasDateEvaluated, dataRow("DateEvaluated"), dtpEvaluationDate.MinDate)
            dtpEvaluationDate.Checked = hasDateEvaluated

            Dim hasDateRegularized = Not IsDBNull(dataRow("DateRegularized"))
            dtpRegularizationDate.Value = If(hasDateRegularized, dataRow("DateRegularized"), dtpRegularizationDate.MinDate)
            dtpRegularizationDate.Checked = hasDateRegularized
        End If

        Dim branchId = dgvEmp.CurrentRow.Cells("BranchID").Value
        Dim branch = _branches.
            Where(Function(b) Nullable.Equals(b.RowID, branchId)).
            FirstOrDefault

        Dim branchIndex As Integer = -1

        If branch IsNot Nothing Then
            branchIndex = _branches.IndexOf(branch)
        End If

        BranchComboBox.SelectedIndex = branchIndex

        BPIinsuranceText.Text = dgvEmp.CurrentRow.Cells("BPIInsuranceColumn").Value

        AddHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged
    End Sub

    Private Shared Function GetCurrentEmployeeEntity(employeeID As Integer?) As Employee

        Dim employeeBuilder = MainServiceProvider.GetRequiredService(Of EmployeeQueryBuilder)

        Return employeeBuilder.
                    IncludePayFrequency().
                    IncludePosition().
                    GetById(employeeID, z_OrganizationID)

    End Function

    Dim currDepenCount As Integer

    Sub tsbtnNewEmp_Click(sender As Object, e As EventArgs) Handles tsbtnNewEmp.Click
        If tsbtnNewEmp.Visible = False Then : Exit Sub : End If

        txtEmpID.Focus()
        tsbtnNewEmp.Enabled = False
        clearObjControl(SplitContainer2.Panel1)
        clearObjControl(SplitContainer2.Panel2)

        rdMale.Checked = True : rdFMale.Checked = False

        pbemppic.Image = Nothing
        File.Delete(Path.GetTempPath & "tmpfileEmployeeImage.jpg")

        LeaveBalanceTextBox.Text = 0
        txtNumDepen.Text = 0
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
        chkbxRevealInPayroll.Checked = False

        BPIinsuranceText.Text = _policy.DefaultBPIInsurance
    End Sub

    Dim PayFreqE_asc As String

    Private Sub cboPayFreq_KeyPress(sender As Object, e As KeyPressEventArgs) 'Handles cboPayFreq.KeyPress
        PayFreqE_asc = Asc(e.KeyChar)
        If PayFreqE_asc = 8 Then
            e.Handled = False
            cboPayFreq.SelectedIndex = -1
        End If
    End Sub

    Private Sub cboPayFreq_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cboPayFreq.SelectedIndexChanged, cboPayFreq.SelectedValueChanged
        If cboPayFreq.Text = "" Then
            payFreqID = ""
            If PayFreqE_asc = 8 Then : payFreqID = "" : End If
        Else : payFreqID = getStrBetween(payFreq.Item(cboPayFreq.SelectedIndex), "", "@")
        End If
    End Sub

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click
        cboEmpStat.Enabled = True
        tsbtnNewEmp.Enabled = True

        dependentitemcount = -1
        dgvEmp_SelectionChanged(sender, e)
    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles tabctrlemp.DrawItem
        TabControlColor(tabctrlemp, e)
    End Sub

    Private Sub TabControl2_DrawItem(sender As Object, e As DrawItemEventArgs) 'Handles TabControl2.DrawItem
        TabControlColor(TabControl2, e, Me.BackColor)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        'Removed the old position form
        'When this button is visible again, add code to show the new add position form
    End Sub

    Private Sub cboEmpStat_TextChanged(sender As Object, e As EventArgs) 'Handles cboEmpStat.TextChanged
        If publicEmpRowID Is Nothing Then
        Else
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
                            First_LinkClicked(First, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))
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

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If Button3.Image.Tag = 1 Then
            Button3.Image = Nothing
            Button3.Image = My.Resources.r_arrow
            Button3.Image.Tag = 0

            tabctrlemp.Show()
            dgvEmp.Width = 350
            dgvEmp_SelectionChanged(sender, e)
        Else
            Button3.Image = Nothing
            Button3.Image = My.Resources.l_arrow
            Button3.Image.Tag = 1

            tabctrlemp.Hide()
            Dim pointX As Integer = Width_resolution - (Width_resolution * 0.15)
            dgvEmp.Width = pointX
        End If
    End Sub

    Dim dgvR_indx As Integer
    Dim dgvDepenPrevVal, dgvC_indx As String
    Dim dgvDepenActiv As Object

    Private Sub dgvDepen_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvDepen.CellBeginEdit 'te Sub dgvDepen_
        dgvR_indx = e.RowIndex : dgvC_indx = dgvDepen.Columns(dgvDepen.CurrentCell.ColumnIndex).Name
        c_Editing = dgvDepen.Columns(dgvDepen.CurrentCell.ColumnIndex).Name
        If Val(dgvDepen.Item("Colmn0", dgvR_indx).Value) <> 0 Then
            If dgvC_indx = "Colmn20" Then
                dgvDepenActiv = DirectCast(dgvDepen.Item(dgvC_indx, dgvR_indx), DataGridViewCheckBoxCell).Value
            Else
                dgvDepenPrevVal = dgvDepen.Item(dgvC_indx, dgvR_indx).Value
            End If
        End If
        noCurrCellChange = 1
    End Sub

    Public listofEditDepen As New List(Of String)

    Private Sub dgvDepen_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDepen.CellEndEdit
        Try
            dgvC_indx = dgvDepen.Columns(e.ColumnIndex).Name
            dgvR_indx = e.RowIndex

            If dgvC_indx = "Colmn20" Then
                If dgvDepenActiv = DirectCast(dgvDepen.Item(dgvC_indx, dgvR_indx), DataGridViewCheckBoxCell).Value Then
                Else
                    If Val(dgvDepen.Item("Colmn0", dgvR_indx).Value) <> 0 Then
                        listofEditDepen.Add(dgvDepen.Item("Colmn0", dgvR_indx).Value)
                    End If
                End If
            ElseIf dgvC_indx = "Colmn2" Then
                dgvDepen.Item("Colmn24", e.RowIndex).Value = dgvDepen.Item("Colmn2", e.RowIndex).Value
                dgvDepen.Item("Colmn2", e.RowIndex).Value = dgvDepen.Item("Colmn24", e.RowIndex).Value

                If Val(dgvDepen.Item("Colmn0", dgvR_indx).Value) <> 0 Then
                    listofEditDepen.Add(dgvDepen.Item("Colmn0", dgvR_indx).Value)
                End If
                dgvDepen.Item("Colmn25", e.RowIndex).Value = dgvDepen.Item("Colmn7", e.RowIndex).Value
            ElseIf dgvC_indx = "Colmn7" Then
                dgvDepen.Item("Colmn25", e.RowIndex).Value = dgvDepen.Item("Colmn7", e.RowIndex).Value
                If Val(dgvDepen.Item("Colmn0", dgvR_indx).Value) <> 0 Then
                    listofEditDepen.Add(dgvDepen.Item("Colmn0", dgvR_indx).Value)
                End If
                dgvDepen.Item("Colmn24", e.RowIndex).Value = dgvDepen.Item("Colmn2", e.RowIndex).Value
            Else
                If dgvDepen.Item(dgvC_indx, dgvR_indx).Value = dgvDepenPrevVal Then
                Else 'Colmn21 is Column for dgvDepen - Birthdate
                    If Val(dgvDepen.Item("Colmn0", dgvR_indx).Value) <> 0 Then
                        listofEditDepen.Add(dgvDepen.Item("Colmn0", dgvR_indx).Value)
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            If txtdgvDepen IsNot Nothing Then
                RemoveHandler txtdgvDepen.KeyDown, AddressOf dgvDepen_KeyDown
            End If
            dgvDepen.AutoResizeRow(e.RowIndex)
            dgvDepen.PerformLayout()
        End Try
    End Sub

    Dim dependentitemcount As Integer = 0

    Sub substituteCell(ByVal dgv As DataGridView, ByVal colName As String, ByVal Obj As Object, Optional isVisb As SByte = Nothing)

        Try
            Obj.Visible = If(isVisb = 0, True, False)
            If dgv.Columns(colName).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells Then
                Dim wid As Integer = dgv.Columns(colName).Width

                Dim x As Integer = dgv.Columns(colName).Width + 25
                dgv.Columns(colName).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                dgv.Columns(colName).Width = x
            End If

            Dim rect As Rectangle = dgv.GetCellDisplayRectangle(dgv.CurrentRow.Cells(colName).ColumnIndex, dgv.CurrentRow.Cells(colName).RowIndex, True)
            Obj.Parent = dgv : Obj.Width = dgv.Columns(colName).Width
            Obj.Location = New Point(rect.Right - Obj.Width, rect.Top)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub dgvDepen_ColumnDisplayIndexChanged(sender As Object, e As DataGridViewColumnEventArgs) 'Handles dgvDepen.ColumnDisplayIndexChanged, dgvDepen.ColumnWidthChanged
        If dgvDepen.RowCount <> 0 Then
            With dgvDepen.CurrentRow

            End With
        Else
        End If
    End Sub

    Private Sub dgvDepen_Scroll(sender As Object, e As ScrollEventArgs) 'Handles dgvDepen.Scroll
        If dgvDepen.RowCount <> 0 Then
            With dgvDepen.CurrentRow

            End With
        Else
        End If
    End Sub

    Dim currDgvIndx, r_Editing As Integer
    Dim c_Editing As String
    Dim txtdgvDepen As TextBox

    Private Sub dgvDepen_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvDepen.EditingControlShowing

        Try
            e.Control.ContextMenu = New ContextMenu
            txtdgvDepen = New TextBox
            txtdgvDepen = DirectCast(e.Control, TextBox)
            AddHandler txtdgvDepen.KeyDown, AddressOf dgvDepen_KeyDown
        Catch ex As Exception
            txtdgvDepen = Nothing
        End Try

        r_Editing = dgvDepen.CurrentRow.Index
        c_Editing = dgvDepen.Columns(dgvDepen.CurrentCell.ColumnIndex).Name

    End Sub

    Private Sub dgvDepen_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvDepen.SelectionChanged
        If dgvDepen.RowCount <> 0 Then
            With dgvDepen.CurrentRow
                If .IsNewRow Then
                Else
                    currDgvIndx = .Index
                    Dim currdgvDepenCellName = dgvDepen.Columns(dgvDepen.CurrentCell.ColumnIndex).Name
                    substituteCell(dgvDepen, currdgvDepenCellName, txtCell, 1)

                End If
            End With
        Else
            txtCell.Visible = False
        End If
    End Sub

    Dim curr_empColm As String
    Dim curr_empRow As Integer

    Sub SearchEmployee_Click(sender As Object, e As EventArgs) Handles Button4.Click
        dependentitemcount = -1

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
        RemoveHandler dgvDepen.SelectionChanged, AddressOf dgvDepen_SelectionChanged

        If dgvEmp.RowCount <> 0 Then
            curr_empRow = dgvEmp.CurrentRow.Index
            curr_empColm = dgvEmp.Columns(dgvEmp.CurrentCell.ColumnIndex).Name
            dgvEmp.Item(curr_empColm, curr_empRow).Selected = True
        End If

        If TabControl2.SelectedIndex = 0 Then
            PopulateEmployeeGrid()
        Else
            Dim sql = $"{q_employee} ORDER BY e.RowID DESC LIMIT {pagination},100;"
            If isKPressSimple = 1 Then
                If txtSimple.Text.Trim.Length = 0 Then
                    SimpleEmployeeFilter(sql)
                Else
                    SearchEmpSimple()
                End If
            Else
                If txtSimple.Text.Trim.Length = 0 Then
                    SimpleEmployeeFilter(sql)
                Else
                    SearchEmpSimple()
                End If
            End If
        End If

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
            dgvEmp_SelectionChanged(sender, e)
        End If

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
        AddHandler dgvDepen.SelectionChanged, AddressOf dgvDepen_SelectionChanged
    End Sub

    Private Sub PopulateEmployeeGrid()
        Dim param_array = New Object() {orgztnID,
                                        TextBox1.Text,
                                        TextBox15.Text,
                                        TextBox16.Text,
                                        pagination}

        Dim n_ReadSQLProcedureToDatatable As New _
            ReadSQLProcedureToDatatable("SEARCH_employeeprofile",
                                        param_array)
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
        dgvEmp_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Async Sub SimpleEmployeeFilter(queryText As String, Optional dictionary As Dictionary(Of String, Object) = Nothing)
        Using command = New MySqlCommand(queryText, New MySqlConnection(mysql_conn_text))
            Try
                dgvEmp.Rows.Clear()
                Dim hasParameters = dictionary.Any
                If hasParameters Then
                    For Each dict In dictionary
                        command.Parameters.AddWithValue(dict.Key, dict.Value)
                    Next
                End If
                Await command.Connection.OpenAsync()
                Dim da As New MySqlDataAdapter
                da.SelectCommand = command
                Dim ds As New DataSet
                da.Fill(ds)
                Dim dt = ds.Tables.OfType(Of DataTable).FirstOrDefault
                For Each drow In dt.Rows.OfType(Of DataRow).ToList()
                    Dim rowArray = drow.ItemArray()
                    Dim index = dgvEmp.Rows.Add(rowArray)
                    dgvEmp.Rows(index).Tag = drow
                Next
            Catch ex As Exception
                Throw New Exception("SimpleEmployeeFilter")
            End Try
        End Using
    End Sub

    Private Sub dgvDepen_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvDepen.DataError
        e.ThrowException = False

        If (e.Context = DataGridViewDataErrorContexts.Commit) _
            Then
            MessageBox.Show("Commit error")
        End If
        If (e.Context = DataGridViewDataErrorContexts _
            .CurrentCellChange) Then
            MessageBox.Show("Cell change")
        End If
        If (e.Context = DataGridViewDataErrorContexts.Parsing) _
            Then
            MessageBox.Show("parsing error")
        End If
        If (e.Context =
            DataGridViewDataErrorContexts.LeaveControl) Then
            MessageBox.Show("leave control error")
        End If

        If (TypeOf (e.Exception) Is ConstraintException) Then
            Dim view As DataGridView = CType(sender, DataGridView)
            view.Rows(e.RowIndex).ErrorText = "     an error"
            view.Rows(e.RowIndex).Cells(e.ColumnIndex) _
                .ErrorText = "an error"

            e.ThrowException = False
        End If
    End Sub

    Private Sub dgvDepen_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgvDepen.CurrentCellChanged

        If c_Editing Is Nothing Then
        Else
            If noCurrCellChange = 1 Then
                If c_Editing = "Colmn12" Or c_Editing = "Colmn20" Then
                ElseIf c_Editing = "Colmn21" _
                    And dgvDepen.Item("Colmn21", r_Editing).Value <> Nothing Then
                    Try
                        dgvDepen.Item("Colmn21", r_Editing).Value = Format(CDate(dgvDepen.Item("Colmn21", r_Editing).Value), machineShortDateFormat)
                    Catch ex As Exception
                        dgvDepen.EndEdit(True)
                        WarnBalloon(, , txtCell, , , 1)

                        dgvDepen.Focus()
                        dgvDepen_SelectionChanged(sender, e)
                        WarnBalloon("Please input an appropriate Birth date", "Invalid Birth date", txtCell, txtCell.Width - 16, -69, , 3000)
                        Exit Sub
                    End Try
                End If
            End If

        End If
        If txtdgvDepen IsNot Nothing Then
            ''RemoveHandler dgvDepenTxtBox.Leave, AddressOf CellStrProperT rim_Leave
            RemoveHandler txtdgvDepen.KeyDown, AddressOf dgvDepen_KeyDown
        End If
    End Sub

    Private Sub cboSearchCommon_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox7.KeyPress, ComboBox8.KeyPress, ComboBox9.KeyPress,
                                                                                          ComboBox10.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)
        If e_asc = 8 Then
            e.Handled = False
            DirectCast(sender, ComboBox).SelectedIndex = -1
        ElseIf e_asc = 13 Then
            SearchEmployee_Click(sender, e)
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

    Private Sub Search_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress, TextBox15.KeyPress, TextBox16.KeyPress,
                                                                                  TextBox17.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 13 Then
            SearchEmployee_Click(sender, e)
        End If
    End Sub

    Dim colName As String

    Sub SearchEmpSimple() ' As String
        Static s As SByte

        Try
            search_selIndx = ValNoComma(ComboBox1.SelectedValue)
            Console.WriteLine(String.Concat("@@@@@@@@@@@@@@@@@ ", search_selIndx))
        Catch ex As Exception
            search_selIndx = 0
            Console.WriteLine(String.Concat("@@@@@ ERROR @@@@@ ", getErrExcptn(ex, Me.Name)))
        End Try
        s = 0
        Select Case search_selIndx
            Case 0 : colName = "e.EmployeeID='"
            Case 1 : colName = "e.FirstName='"
            Case 2 : colName = "e.MiddleName='"
            Case 3 : colName = "e.LastName='"
            Case 4 : colName = "e.Surname='"
            Case 5 : colName = "e.Nickname='"
            Case 6 : colName = "e.MaritalStatus='"
            Case 7 : colName = "e.EmployeeID='" 'NoOfDependents
            Case 8 : colName = "e.Birthdate='"
                s = 1
                'dgvRowAdder(q_employee & " AND " & colName & Format(CDate(txtSimple.Text), "yyyy-MM-dd") & "' ORDER BY e.RowID DESC", dgvEmp)
                Dim dict = New Dictionary(Of String, Object) From {{"@birthDate", Format(CDate(txtSimple.Text), "yyyy-MM-dd")}}
                SimpleEmployeeFilter($"{q_employee} AND e.Birthdate=@birthDate ORDER BY e.RowID DESC", dict)
            Case 9 : colName = "e.Startdate='"
                s = 1
                'dgvRowAdder(q_employee & " AND " & colName & Format(CDate(txtSimple.Text), "yyyy-MM-dd") & "' ORDER BY e.RowID DESC", dgvEmp)
                Dim dict = New Dictionary(Of String, Object) From {{"@startdate", Format(CDate(txtSimple.Text), "yyyy-MM-dd")}}
                SimpleEmployeeFilter($"{q_employee} AND e.Startdate=@startdate ORDER BY e.RowID DESC", dict)
            Case 10 : colName = "e.JobTitle='"
            Case 11 : colName = "pos.PositionName='" 'e.PositionID
            Case 12 : colName = "e.Salutation='"
            Case 13 : colName = "e.TINNo='"
            Case 14 : colName = "e.SSSNo='"
            Case 15 : colName = "e.HDMFNo='"
            Case 16 : colName = "e.PhilHealthNo='"
            Case 17 : colName = "e.WorkPhone='"
            Case 18 : colName = "e.HomePhone='"
            Case 19 : colName = "e.MobilePhone='" '19
            Case 20 : colName = "e.HomeAddress='"
            Case 21 : colName = "e.EmailAddress='"
            Case 22 : colName = "e.Gender=LEFT('"
                s = 1
                'dgvRowAdder(q_employee & " AND " & colName & txtSimple.Text & "',1) ORDER BY e.RowID DESC", dgvEmp)
                Dim dict = New Dictionary(Of String, Object) From {{"@gender", txtSimple.Text}}
                SimpleEmployeeFilter($"{q_employee} AND e.Gender=@gender ORDER BY e.RowID DESC", dict)
            Case 23 : colName = "e.EmploymentStatus='"
            Case 24 : colName = "pf.PayFrequencyType='"

            Case 25 : colName = "DATE_FORMAT(e.Created,'%m-%d-%Y')='"
            Case 26 : colName = "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))='" 'e.CreatedBy
            Case 27 : colName = "DATE_FORMAT(e.LastUpd,'%m-%d-%Y')='"
            Case 28 : colName = "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))='" 'e.CreatedBy

            Case 29 : colName = "e.EmployeeType='"
            Case 30 : colName = "e.EmployeeID='"

        End Select

        If s = 0 Then
            'dgvRowAdder(q_employee & " AND " & colName & txtSimple.Text & "' ORDER BY e.RowID DESC", dgvEmp)
            Dim sql = $"{q_employee} ORDER BY e.RowID DESC LIMIT {pagination},100;"
            SimpleEmployeeFilter(sql)
        End If
    End Sub

    Dim isKPressSimple As SByte
    Dim colSearchSimple As String

    Private Sub txtSimple_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSimple.KeyDown ',
        'ComboBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            isKPressSimple = 1
            SearchEmployee_Click(sender, e)
            isKPressSimple = 0
        End If
    End Sub

    Private Sub txtSimple_TextChanged(sender As Object, e As EventArgs) Handles txtSimple.TextChanged
        ComboBox1.Text = txtSimple.Text
    End Sub

    Sub tsbtnNewDepen_Click(sender As Object, e As EventArgs) Handles tsbtnNewDepen.Click
        RemoveHandler dgvDepen.SelectionChanged, AddressOf dgvDepen_SelectionChanged

        dgvDepen.EndEdit(True)

        dgvDepen.Focus()
        For Each r As DataGridViewRow In dgvDepen.Rows

            If r.IsNewRow Then
                r.Cells("Colmn2").Selected = True
                r.Cells("Colmn20").Value = True
                dgvDepen.Focus()
                Exit For
            End If
        Next

        AddHandler dgvDepen.SelectionChanged, AddressOf dgvDepen_SelectionChanged
    End Sub

    Dim _EmpRowID As String

    Sub tsbtnSaveDepen_Click(sender As Object, e As EventArgs) 'Handles tsbtnSaveDepen.Click
        Static finUpd As Integer = -1

        If tsbtnNewEmp.Enabled = False Then
        Else

        End If

        dgvDepen.EndEdit(True)

        If noCurrCellChange = 1 Then
            If c_Editing <> Nothing Then
                If c_Editing = "Colmn12" Or c_Editing = "Colmn20" Or c_Editing = "Colmn21" Then
                    dgvDepen.Item("Colmn21", r_Editing).Selected = True
                End If
            End If
        End If

        Dim numActivDepen As Integer = 0
        Dim depenCount = 0
        For Each r As DataGridViewRow In dgvDepen.Rows
            If Trim(r.Cells("Colmn3").Value) = "" Then
                WarnBalloon(, , txtCell, , , 1)
                dgvDepen.Item("Colmn3", r.Index).Selected = True
                dgvDepen.Focus()
                dgvDepen_SelectionChanged(sender, e)
                WarnBalloon("Please input First Name", "Invalid First Name", txtCell, txtCell.Width - 16, -69, , 3000)
                Exit Sub
            ElseIf Trim(r.Cells("Colmn5").Value) = "" Then
                WarnBalloon(, , txtCell, , , 1)
                dgvDepen.Item("Colmn5", r.Index).Selected = True
                dgvDepen.Focus()
                dgvDepen_SelectionChanged(sender, e)
                WarnBalloon("Please input Last Name", "Invalid Last Name", txtCell, txtCell.Width - 16, -69, , 3000)
                Exit Sub
            ElseIf Trim(r.Cells("Colmn19").Value) = "" Then
                WarnBalloon(, , txtCell, , , 1)
                dgvDepen.Item("Colmn19", r.Index).Selected = True
                dgvDepen.Focus()
                dgvDepen_SelectionChanged(sender, e)
                WarnBalloon("Please select a Gender", "Invalid Gender", txtCell, txtCell.Width - 16, -69, , 3000)
                Exit Sub
            End If
            Try
                dgvDepen.Item("Colmn21", r.Index).Value = Format(CDate(dgvDepen.Item("Colmn21", r.Index).Value), machineShortDateFormat)
            Catch ex As Exception
                WarnBalloon(, , txtCell, , , 1)
                dgvDepen.Item("Colmn21", r.Index).Selected = True
                dgvDepen.Focus()
                dgvDepen_SelectionChanged(sender, e)
                WarnBalloon("Please input an appropriate Birth date", "Invalid Birth date", txtCell, txtCell.Width - 16, -69, , 3000)
                Exit Sub
            End Try

            If r.Cells("Colmn0").Value <> finUpd Then
                finUpd = r.Cells("Colmn0").Value
                Dim isActivCell = If(DirectCast(r.Cells("Colmn20"), DataGridViewCheckBoxCell).Value = True, "Y", "N")
                For Each rlst In listofEditDepen
                    If r.Cells("Colmn0").Value = rlst Then
                        Dim _bdate = Format(Date.Parse(r.Cells("Colmn21").Value), "yyyy-MM-dd")
                        EXECQUER("UPDATE employeedependents SET " &
                        "Salutation='" & r.Cells("Colmn2").Value &
                        "',FirstName='" & r.Cells("Colmn3").Value &
                        "',MiddleName='" & r.Cells("Colmn4").Value &
                        "',LastName='" & r.Cells("Colmn5").Value &
                        "',Surname='" & r.Cells("Colmn6").Value &
                        "',RelationToEmployee='" & r.Cells("Colmn7").Value &
                        "',TINNo='" & r.Cells("Colmn8").Value &
                        "',SSSNo='" & r.Cells("Colmn9").Value &
                        "',HDMFNo='" & r.Cells("Colmn10").Value &
                        "',PhilHealthNo='" & r.Cells("Colmn11").Value &
                        "',EmailAddress='" & r.Cells("Colmn12").Value &
                        "',WorkPhone='" & r.Cells("Colmn13").Value &
                        "',HomePhone='" & r.Cells("Colmn14").Value &
                        "',MobilePhone='" & r.Cells("Colmn15").Value &
                        "',HomeAddress='" & r.Cells("Colmn16").Value &
                        "',Nickname='" & r.Cells("Colmn17").Value &
                        "',JobTitle='" & r.Cells("Colmn18").Value &
                        "',Gender='" & r.Cells("Colmn19").Value &
                        "',ActiveFlag='" & isActivCell &
                        "',Birthdate='" & _bdate &
                        "',LastUpd=CURRENT_TIMESTAMP()" &
                        ",LastUpdBy=1" &
                        " WHERE RowID='" & rlst & "'")
                        finUpd = rlst

                        InfoBalloon("Dependent " & r.Cells("Colmn3").Value & " " & r.Cells("Colmn5").Value & " has successfully updated.",
                                  "Dependent Update Successful", lblforballoon1, 0, -69)
                        Exit For
                    End If
                Next
            End If

            If r.Cells("Colmn0").Value Is Nothing And dgvEmp.RowCount <> 0 Then

                Dim bdate = Format(Date.Parse(r.Cells("Colmn21").Value), "yyyy-MM-dd").Replace("/", "-")
                Dim depenRowID = INS_employeedepen(r.Cells("Colmn2").Value, r.Cells("Colmn3").Value,
                                  r.Cells("Colmn4").Value, r.Cells("Colmn5").Value,
                                  r.Cells("Colmn6").Value, dgvEmp.CurrentRow.Cells("RowID").Value,
                                  r.Cells("Colmn8").Value, r.Cells("Colmn9").Value,
                                  r.Cells("Colmn10").Value, r.Cells("Colmn11").Value,
                                  r.Cells("Colmn12").Value, r.Cells("Colmn13").Value,
                                  r.Cells("Colmn14").Value, r.Cells("Colmn15").Value,
                                  r.Cells("Colmn16").Value, r.Cells("Colmn17").Value,
                                  r.Cells("Colmn18").Value, r.Cells("Colmn19").Value,
                                  r.Cells("Colmn7").Value, "Y",
                                  bdate)

                r.Cells("Colmn0").Value = depenRowID
                r.Cells("Colmn22").Value = u_nem
                r.Cells("Colmn23").Value = dbnow
                r.Cells("Colmn1").Value = dgvEmp.CurrentRow.Cells("RowID").Value

                InfoBalloon(, , lblforballoon1, , , 1)
                InfoBalloon("Dependent " & r.Cells("Colmn3").Value & " " & r.Cells("Colmn5").Value & " has successfully created.",
                          "New Dependent successfully created", lblforballoon1, 0, -69)
            End If

            depenCount += 1

            If r.Cells("Colmn20").Value = True Then
                numActivDepen += 1
            End If
        Next

        If dgvDepen.RowCount <> 0 And dgvEmp.RowCount <> 0 Then
            Dim numDepens = numActivDepen 'dgvDepen.RowCount
            If numDepens <> Val(txtNumDepen.Text) Then
                EXECQUER("UPDATE employee SET NoOfDependents=" & numDepens & " WHERE RowID='" & dgvEmp.CurrentRow.Cells("RowID").Value & "'")
                txtNumDepen.Text = numDepens
                dgvEmp.CurrentRow.Cells("Column32").Value = numDepens

                MsgBox("INSERT Row employeesalary will take effect here", , "TRIGGER FUNCTION")

                'TRIGGERS the INSERT Row employeesalary it is - global_trunc.EMP_MaritalNumChild_UPD()
            End If
        End If

        tsbtnNewDepen.Enabled = True
        listofEditDepen.Clear()
        finUpd = -1 : noCurrCellChange = 0

        If c_Editing <> Nothing And r_Editing <> Nothing Then
            dgvDepen.Item(c_Editing, r_Editing).Selected = True
        End If
    End Sub

    Private Sub btnsavr_Click(sender As Object, e As EventArgs) Handles tsbtnSaveDepen.Click

        ToolStrip4.Focus()

        dgvDepen.EndEdit(True)

        For Each dgvrow As DataGridViewRow In dgvDepen.Rows
            dgvrow.Cells("Colmn2").Selected = True
            Exit For
        Next

        If dgvEmp.RowCount = 0 Then
            Exit Sub
        End If

        If dontUpdateEmp = 1 Then
            listofEditDepen.Clear()
        End If

        Dim _naw As Object = EXECQUER("SELECT DATE_FORMAT(NOW(),'%Y-%m-%d %T');")

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            Dim cmdquer As MySqlCommand
            cmdquer = New MySqlCommand("INSUPD_employeedependents", conn)
            conn.Open()

            With cmdquer

                Dim numActivDepen As Integer = 0
                'Dim depenCount = 0

                For Each r As DataGridViewRow In dgvDepen.Rows

                    Dim datread As MySqlDataReader

                    .Parameters.Clear()

                    .CommandType = CommandType.StoredProcedure

                    If listofEditDepen.Contains(Trim(r.Cells("Colmn0").Value)) Then
                        'UPDATE FUNCTION

                        .Parameters.Add("empdepenID", MySqlDbType.Int32)

                        .Parameters.AddWithValue("emp_RowID", If(Val(r.Cells("Colmn0").Value) <> 0, r.Cells("Colmn0").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_CreatedBy", z_User) 'Z_User
                        .Parameters.AddWithValue("emp_LastUpdBy", z_User) 'Z_User
                        .Parameters.AddWithValue("emp_LastUpd", _naw)
                        .Parameters.AddWithValue("emp_OrganizationID", orgztnID) 'orgztnID
                        .Parameters.AddWithValue("emp_Salutation", If(Trim(r.Cells("Colmn24").Value) <> "", r.Cells("Colmn24").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_FirstName", If(Trim(r.Cells("Colmn3").Value) <> "", r.Cells("Colmn3").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_MiddleName", If(Trim(r.Cells("Colmn4").Value) <> "", r.Cells("Colmn4").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_LastName", If(Trim(r.Cells("Colmn5").Value) <> "", r.Cells("Colmn5").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_SurName", If(Trim(r.Cells("Colmn6").Value) <> "", r.Cells("Colmn6").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_ParentEmployeeID", dgvEmp.CurrentRow.Cells("RowID").Value)
                        .Parameters.AddWithValue("emp_TINNo", If(Trim(r.Cells("Colmn8").Value) <> "", r.Cells("Colmn8").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_SSSNo", If(Trim(r.Cells("Colmn9").Value) <> "", r.Cells("Colmn9").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_HDMFNo", If(Trim(r.Cells("Colmn10").Value) <> "", r.Cells("Colmn10").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_PhilHealthNo", If(Trim(r.Cells("Colmn11").Value) <> "", r.Cells("Colmn11").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_EmailAddress", If(Trim(r.Cells("Colmn12").Value) <> "", r.Cells("Colmn12").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_WorkPhone", If(Trim(r.Cells("Colmn13").Value) <> "", r.Cells("Colmn13").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_HomePhone", If(Trim(r.Cells("Colmn14").Value) <> "", r.Cells("Colmn14").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_MobilePhone", If(Trim(r.Cells("Colmn15").Value) <> "", r.Cells("Colmn15").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_HomeAddress", If(Trim(r.Cells("Colmn16").Value) <> "", r.Cells("Colmn16").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_Nickname", If(Trim(r.Cells("Colmn17").Value) <> "", r.Cells("Colmn17").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_JobTitle", If(Trim(r.Cells("Colmn18").Value) <> "", r.Cells("Colmn18").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_Gender", If(Trim(r.Cells("Colmn19").Value) <> "", r.Cells("Colmn19").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_RelationToEmployee", If(Trim(r.Cells("Colmn25").Value) <> "", r.Cells("Colmn25").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_ActiveFlag", If(CBool(DirectCast(r.Cells("Colmn20"), DataGridViewCheckBoxCell).Value) = True, "Y", "N"))
                        .Parameters.AddWithValue("emp_Birthdate", If(Trim(r.Cells("Colmn21").Value) <> "", Format(CDate(r.Cells("Colmn21").Value), "yyyy-MM-dd"), DBNull.Value))
                        .Parameters.AddWithValue("emp_IsDoneByImporting", "0")

                        .Parameters("empdepenID").Direction = ParameterDirection.ReturnValue

                        .ExecuteScalar() 'ExecuteNonQuery

                    End If

                    If Val(r.Cells("Colmn0").Value) = 0 And
                        r.IsNewRow = False And
                        tsbtnNewDepen.Visible = True Then
                        'INSERT FUNCTION

                        .Parameters.Add("empdepenID", MySqlDbType.Int32)

                        .Parameters.AddWithValue("emp_RowID", If(Val(r.Cells("Colmn0").Value) <> 0, r.Cells("Colmn0").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_CreatedBy", z_User) 'Z_User
                        .Parameters.AddWithValue("emp_LastUpdBy", z_User) 'Z_User
                        .Parameters.AddWithValue("emp_LastUpd", _naw)
                        .Parameters.AddWithValue("emp_OrganizationID", orgztnID) 'orgztnID
                        .Parameters.AddWithValue("emp_Salutation", If(Trim(r.Cells("Colmn24").Value) <> "", r.Cells("Colmn24").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_FirstName", If(Trim(r.Cells("Colmn3").Value) <> "", r.Cells("Colmn3").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_MiddleName", If(Trim(r.Cells("Colmn4").Value) <> "", r.Cells("Colmn4").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_LastName", If(Trim(r.Cells("Colmn5").Value) <> "", r.Cells("Colmn5").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_SurName", If(Trim(r.Cells("Colmn6").Value) <> "", r.Cells("Colmn6").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_ParentEmployeeID", dgvEmp.CurrentRow.Cells("RowID").Value)
                        .Parameters.AddWithValue("emp_TINNo", If(Trim(r.Cells("Colmn8").Value) <> "", r.Cells("Colmn8").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_SSSNo", If(Trim(r.Cells("Colmn9").Value) <> "", r.Cells("Colmn9").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_HDMFNo", If(Trim(r.Cells("Colmn10").Value) <> "", r.Cells("Colmn10").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_PhilHealthNo", If(Trim(r.Cells("Colmn11").Value) <> "", r.Cells("Colmn11").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_EmailAddress", If(Trim(r.Cells("Colmn12").Value) <> "", r.Cells("Colmn12").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_WorkPhone", If(Trim(r.Cells("Colmn13").Value) <> "", r.Cells("Colmn13").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_HomePhone", If(Trim(r.Cells("Colmn14").Value) <> "", r.Cells("Colmn14").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_MobilePhone", If(Trim(r.Cells("Colmn15").Value) <> "", r.Cells("Colmn15").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_HomeAddress", If(Trim(r.Cells("Colmn16").Value) <> "", r.Cells("Colmn16").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_Nickname", If(Trim(r.Cells("Colmn17").Value) <> "", r.Cells("Colmn17").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_JobTitle", If(Trim(r.Cells("Colmn18").Value) <> "", r.Cells("Colmn18").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_Gender", If(Trim(r.Cells("Colmn19").Value) <> "", r.Cells("Colmn19").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_RelationToEmployee", If(Trim(r.Cells("Colmn25").Value) <> "", r.Cells("Colmn25").Value, DBNull.Value))
                        .Parameters.AddWithValue("emp_ActiveFlag", If(DirectCast(r.Cells("Colmn20"), DataGridViewCheckBoxCell).Value = True, "Y", "N"))
                        .Parameters.AddWithValue("emp_Birthdate", If(Trim(r.Cells("Colmn21").Value) <> "", Format(CDate(r.Cells("Colmn21").Value), "yyyy-MM-dd"), DBNull.Value))
                        .Parameters.AddWithValue("emp_IsDoneByImporting", "0")

                        .Parameters("empdepenID").Direction = ParameterDirection.ReturnValue

                        datread = .ExecuteReader()

                        r.Cells("Colmn0").Value = datread.GetString(0).ToString()

                        datread.Dispose()

                    End If

                    'depenCount += 1

                    If CBool(r.Cells("Colmn20").Value) = True Then
                        numActivDepen += 1
                    End If

                Next

                If dgvDepen.RowCount <> 0 And dgvEmp.RowCount <> 0 Then
                    Dim numDepens = numActivDepen 'dgvDepen.RowCount
                    If numDepens <> Val(txtNumDepen.Text) Then
                        EXECQUER("UPDATE employee SET NoOfDependents=" & numDepens & " WHERE RowID='" & dgvEmp.CurrentRow.Cells("RowID").Value & "';")
                        txtNumDepen.Text = numDepens
                        dgvEmp.CurrentRow.Cells("Column32").Value = numDepens

                        'TRIGGERS the INSERT Row employeesalary it is - global_trunc.EMP_MaritalNumChild_UPD()
                    End If
                End If

            End With
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        Finally
            conn.Close()
            listofEditDepen.Clear()

        End Try

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        listofEditDepen.Clear()

        dependentitemcount = -1

        If tsbtnNewEmp.Enabled = True Then
            tsbtnNewDepen.Enabled = True : listofEditDepen.Clear()

            dgvEmp_SelectionChanged(sender, e)
        Else
            dgvDepen.Rows.Clear()
        End If
    End Sub

    Private Sub TabControl2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl2.SelectedIndexChanged
        If TabControl2.SelectedIndex = 0 Then
            TextBox1.Focus()
        Else
            txtSimple.Focus()
        End If
    End Sub

    Dim search_selIndx As Integer

    Dim empcolcount As Integer

    Private Sub SplitContainer2_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer2.SplitterMoved
        InfoBalloon(, , lblforballoon1, , , 1)
    End Sub

    Private Sub dgvDepen_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvDepen.KeyDown
        If (e.Control AndAlso e.KeyCode = Keys.S) Then
            tsbtnSaveDepen_Click(sender, e)
        ElseIf e.KeyCode = Keys.Escape Then
            ToolStripButton1_Click(sender, e)
        End If
    End Sub

    Sub spltr_Panel1_KeyDown(sender As Object, e As KeyEventArgs)
        If (e.Control AndAlso e.KeyCode = Keys.S) Then
            tsbtnSaveEmp_Click(sender, e)
        ElseIf (e.Control AndAlso e.KeyCode = Keys.N) Then
            tsbtnNewEmp_Click(sender, e)
        ElseIf e.KeyCode = Keys.Escape Then
            tsbtnCancel_Click(sender, e)
        End If
    End Sub

    Private Sub txtBDate_TextChanged(sender As Object, e As EventArgs) Handles txtBDate.TextChanged
        'dtpempenddate
    End Sub

    Sub VIEW_employeedependents(ByVal ParentEmployeeID As Object)

        Dim param(1, 2) As Object

        param(0, 0) = "edep_ParentEmployeeID"
        param(1, 0) = "edep_OrganizationID"

        param(0, 1) = ParentEmployeeID
        param(1, 1) = orgztnID

        EXEC_VIEW_PROCEDURE(param,
                           "VIEW_employeedependents",
                           dgvDepen, , 1)

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

    Private Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked,
                                                                                                Nxt.LinkClicked, Last.LinkClicked

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

                'pagination -= 50

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
                'pagination = 0

            End If

        End If

        If (Trim(TextBox1.Text) <> "" Or
            Trim(TextBox15.Text) <> "" Or
            Trim(TextBox16.Text) <> "" Or
            Trim(TextBox17.Text) <> "") And
            TabControl2.SelectedIndex = 0 Then

            SearchEmployee_Click(sender, e)

        ElseIf Trim(txtSimple.Text) <> "" And
        TabControl2.SelectedIndex = 1 Then

            SearchEmployee_Click(sender, e)
        Else

            loademployee()

        End If

        dgvEmp_SelectionChanged(sender, e)

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabctrlemp.SelectedIndexChanged
        Label25.Text = Trim(tabctrlemp.SelectedTab.Text)
    End Sub

    Dim emp_ralation As New AutoCompleteStringCollection

    Async Sub tbpEmployee_Enter(sender As Object, e As EventArgs) Handles tbpEmployee.Enter

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

            'enlistToCboBox(q_salut, cboSalut)
            'salutn_count = cboSalut.Items.Count

            enlistToCboBox(q_empstat, cboEmpStat) '"SELECT DISTINCT(COALESCE(DisplayValue,'')) FROM listofval WHERE Type='Status' AND Active='Yes'"

            enlistToCboBox(q_emptype, cboEmpType)

            enlistToCboBox(q_maritstat, cboMaritStat)

            enlistToCboBox("SELECT DisplayValue FROM listofval WHERE `Type`='Bank Names';", cbobank)

            ShowBranch()

            ShowBPIInsurance()

            Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT '' AS RowID, '' AS AgencyName" &
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

            view_ID = VIEW_privilege("Employee Personal Profile", orgztnID)

            'For Each strval In cboSalut.Items
            '    Colmn2.Items.Add(strval)
            'Next

            enlistTheLists("SELECT DisplayValue FROM listofval WHERE Type='Employee Relationship' ORDER BY OrderBy;",
                           emp_ralation)

            Dim payFrequencyRepository = MainServiceProvider.GetRequiredService(Of PayFrequencyRepository)
            Dim payFrequencies = Await payFrequencyRepository.GetAllAsync()
            _payFrequencies = payFrequencies.
                                Where(Function(p) p.RowID = PayFrequencyType.SemiMonthly OrElse
                                    p.RowID = PayFrequencyType.Weekly).ToList

            cboPayFreq.ValueMember = "RowID"
            cboPayFreq.DisplayMember = "Type"

            cboPayFreq.DataSource = _payFrequencies

            For Each strval In emp_ralation
                Colmn7.Items.Add(strval)
                'MsgBox(strval)
            Next

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

            If formuserprivilege.Count = 0 Then

                tsbtnNewEmp.Visible = 0
                tsbtnSaveEmp.Visible = 0

                tsbtnNewDepen.Visible = 0
                tsbtnSaveDepen.Visible = 0

                dontUpdateEmp = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        'ToolStripButton2.Visible = 0
                        tsbtnNewEmp.Visible = 0
                        tsbtnSaveEmp.Visible = 0

                        tsbtnNewDepen.Visible = 0
                        tsbtnSaveDepen.Visible = 0

                        dontUpdateEmp = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            tsbtnNewEmp.Visible = 0
                            tsbtnNewDepen.Visible = 0
                        Else
                            tsbtnNewEmp.Visible = 1
                            tsbtnNewDepen.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateEmp = 1
                        Else
                            dontUpdateEmp = 0
                        End If

                    End If

                Next

            End If

            AddHandler dgvDepen.SelectionChanged, AddressOf dgvDepen_SelectionChanged

            Panel1.Visible =
                (Panel1.AccessibleDescription = _systemOwnerService.GetCurrentSystemOwner())

        End If

        tabIndx = GetEmployeeProfileTabPageIndex()

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub ShowBranch()

        _branches = New List(Of Branch)

        If _policy.ShowBranch = False Then

            BranchComboBox.Visible = False
            BranchLabel.Visible = False
            AddBranchLinkButton.Visible = False

            Return

        End If

        Dim branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)
        _branches = branchRepository.GetAll()

        BranchComboBox.Visible = True
        BranchLabel.Visible = True
        AddBranchLinkButton.Visible = True

        BranchComboBox.DisplayMember = "Name"
        BranchComboBox.DataSource = _branches
    End Sub

    Private Sub ShowBPIInsurance()

        If _policy.ShowBranch = False Then

            BPIinsuranceText.Visible = False
            BPIinsuranceLabel.Visible = False

            Return

        End If

        BPIinsuranceText.Visible = True
        BPIinsuranceLabel.Visible = True

    End Sub

    Private Function GetSelectedBranch() As Branch

        If BranchComboBox.SelectedIndex >= 0 AndAlso BranchComboBox.SelectedIndex < _branches.Count Then

            Return _branches(BranchComboBox.SelectedIndex)

        End If

        Return Nothing

    End Function

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

    Private Sub txtOTgrace_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOTgrace.KeyPress
        e.Handled = TrapNumKey(Asc(e.KeyChar))
    End Sub

    Private Sub txtUTgrace_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUTgrace.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, txtUTgrace.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

    End Sub

    Private Sub chkotflag_CheckedChanged(sender As Object, e As EventArgs) Handles chkotflag.CheckedChanged
        If chkotflag.Checked Then
            txtOTgrace.Enabled = 1
        Else
            txtOTgrace.Enabled = 0
        End If
    End Sub

    Private Sub Panel1_VisibleChanged(sender As Object, e As EventArgs) Handles Panel1.VisibleChanged

        Dim _bool As Boolean = Panel1.Visible

        If _bool Then

            GroupBox1.Location =
                New Point(GroupBox1.Location.X, 557)

        End If

    End Sub

#End Region 'Personal Profile

#Region "Awards"

    Sub tbpAwards_Enter(sender As Object, e As EventArgs) Handles tbpAwards.Enter

        UpdateTabPageText()

        tbpAwards.Text = "AWARDS               "
        Label25.Text = "AWARDS"

        dgvEmp_SelectionChanged(sender, e)

    End Sub

#End Region 'Awards

#Region "Certifications"

    Sub tbpCertifications_Enter(sender As Object, e As EventArgs) Handles tbpCertifications.Enter

        UpdateTabPageText()

        tbpCertifications.Text = "CERTIFICATIONS               "
        Label25.Text = "CERTIFICATIONS"

        dgvEmp_SelectionChanged(sender, e)

    End Sub

#End Region 'Certifications

#Region "Medical Profile"

    Dim view_IDMed As Integer

    Dim categMedRec As String = 0

    Dim dontUpdateMed As SByte = 0

    Dim hasErrondateMedRec As SByte = -1

    Dim medrecordEdited As New AutoCompleteStringCollection

    Dim prevsRowMedRec As Integer

    Dim dateColumn As String

    Function VIEW_employeemedrecordID(Optional emedrecord_EmployeeID As Object = Nothing,
                                      Optional emedrecord_DateFrom As Object = Nothing,
                                      Optional emedrecord_DateTo As Object = Nothing,
                                      Optional emedrecord_ProductID As Object = Nothing) As Object
        Dim param(4, 2) As Object

        param(0, 0) = "emedrecord_EmployeeID"
        param(1, 0) = "emedrecord_DateFrom"
        param(2, 0) = "emedrecord_DateTo"
        param(3, 0) = "emedrecord_ProductID"
        param(4, 0) = "emedrecord_OrganizationID"

        param(0, 1) = If(emedrecord_EmployeeID Is Nothing, DBNull.Value, CInt(emedrecord_EmployeeID))
        param(1, 1) = If(emedrecord_DateFrom Is Nothing, DBNull.Value, Format(CDate(emedrecord_DateFrom), "yyyy-MM-dd"))
        param(2, 1) = If(emedrecord_DateTo Is Nothing, DBNull.Value, Format(CDate(emedrecord_DateTo), "yyyy-MM-dd"))
        param(3, 1) = If(emedrecord_ProductID Is Nothing, DBNull.Value, CInt(emedrecord_ProductID))
        param(4, 1) = orgztnID

        Dim returnval = EXEC_INSUPD_PROCEDURE(param,
                                              "VIEW_employeemedrecordID",
                                              "empmedrecordID")

        Return returnval

    End Function

    Sub INS_employeemedicalrecord(Optional emedrec_RowID As Object = Nothing,
                                     Optional emedrec_EmployeeID As Object = Nothing,
                                     Optional emedrec_DateFrom As Object = Nothing,
                                     Optional emedrec_DateTo As Object = Nothing,
                                     Optional emedrec_ProductID As Object = Nothing,
                                     Optional emedrec_Finding As Object = Nothing)

        'INSERT LANG ITO SA employeemedicalrecord

        Dim _naw As Object = EXECQUER("SELECT DATE_FORMAT(NOW(),'%Y-%m-%d %T');")

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            Dim cmdquer As MySqlCommand
            cmdquer = New MySqlCommand("INSUPD_employeemedicalrecord", conn)
            conn.Open()

            With cmdquer

                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("emedrecID", MySqlDbType.Int32)

                .Parameters.AddWithValue("emedrec_RowID", If(emedrec_RowID Is Nothing, DBNull.Value, emedrec_RowID))
                .Parameters.AddWithValue("emedrec_OrganizationID", orgztnID) 'orgztnID
                .Parameters.AddWithValue("emedrec_Created", _naw)
                .Parameters.AddWithValue("emedrec_LastUpd", _naw)
                .Parameters.AddWithValue("emedrec_CreatedBy", z_User)
                .Parameters.AddWithValue("emedrec_LastUpdBy", z_User)
                .Parameters.AddWithValue("emedrec_EmployeeID", If(emedrec_EmployeeID Is Nothing, DBNull.Value, emedrec_EmployeeID))
                .Parameters.AddWithValue("emedrec_DateFrom", If(emedrec_DateFrom Is Nothing, DBNull.Value, Format(CDate(emedrec_DateFrom), "yyyy-MM-dd")))
                .Parameters.AddWithValue("emedrec_DateTo", If(emedrec_DateTo Is Nothing, DBNull.Value, Format(CDate(emedrec_DateTo), "yyyy-MM-dd")))
                .Parameters.AddWithValue("emedrec_ProductID", If(emedrec_ProductID Is Nothing, DBNull.Value, emedrec_ProductID))
                .Parameters.AddWithValue("emedrec_Finding", emedrec_Finding)

                .Parameters("emedrecID").Direction = ParameterDirection.ReturnValue

                .ExecuteScalar() 'ExecuteNonQuery

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " INS_employeemedicalrecord")
        Finally
            conn.Close()
        End Try
    End Sub

    Function INS_product(Optional p_Name As Object = Nothing,
                         Optional p_PartNo As Object = Nothing,
                         Optional p_CategName As Object = Nothing,
                         Optional p_Status As Object = "Active",
                         Optional p_IsFixed As Boolean = False) As Object

        Dim return_value = Nothing
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            Dim cmdquer As MySqlCommand

            cmdquer = New MySqlCommand("INSUPD_product", conn)

            conn.Open()

            With cmdquer

                Dim datrd As MySqlDataReader

                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("prod_RowID", MySqlDbType.Int32)

                .Parameters.AddWithValue("p_RowID", DBNull.Value)
                .Parameters.AddWithValue("p_Name", p_Name)
                .Parameters.AddWithValue("p_OrganizationID", orgztnID) 'orgztnID
                .Parameters.AddWithValue("p_PartNo", p_PartNo)
                .Parameters.AddWithValue("p_LastUpd", DBNull.Value)
                .Parameters.AddWithValue("p_CreatedBy", z_User)
                .Parameters.AddWithValue("p_LastUpdBy", z_User)
                .Parameters.AddWithValue("p_Category", p_CategName)
                .Parameters.AddWithValue("p_CategoryID", DBNull.Value) 'KELANGAN MA-RETRIEVE KO UNG ROWID SA CATEGORY WHERE CATEGORYNAME = 'MEDICAL RECORD'
                .Parameters.AddWithValue("p_Status", p_Status)
                .Parameters.AddWithValue("p_UnitPrice", 0.0)
                .Parameters.AddWithValue("p_UnitOfMeasure", 0)
                .Parameters.AddWithValue("p_IsFixed", p_IsFixed)
                .Parameters.AddWithValue("p_IsIncludedIn13th", 0)
                .Parameters("prod_RowID").Direction = ParameterDirection.ReturnValue

                datrd = .ExecuteReader()
                If datrd.Read Then
                    return_value = datrd(0)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message & " INSUPD_product")
        Finally
            conn.Close()
        End Try
        Return return_value
    End Function

    Public listofEditedmedprod As New AutoCompleteStringCollection

    Dim prevsRowpMed As Integer

    Private Sub tsbtnCancelmedrecord_Click(sender As Object, e As EventArgs)
        listofEditedmedprod.Clear()
        dgvEmp_SelectionChanged(sender, e)
    End Sub

#End Region 'Medical Profile

#Region "Disciplinary Action"

    Sub tbpDiscipAct_Enter(sender As Object, e As EventArgs) Handles tbpDiscipAct.Enter

        UpdateTabPageText()

        tbpDiscipAct.Text = "DISCIPLINARY ACTION               "

        Label25.Text = "DISCIPLINARY ACTION"

        dgvEmp_SelectionChanged(sender, e)

    End Sub

#End Region 'Disciplinary Action

#Region "Educational Background"

    Sub tbpEducBG_Enter(sender As Object, e As EventArgs) Handles tbpEducBG.Enter
        UpdateTabPageText()

        tbpEducBG.Text = "EDUCATIONAL BACKGROUND               "
        Label25.Text = "EDUCATIONAL BACKGROUND"

        dgvEmp_SelectionChanged(sender, e)
    End Sub

#End Region 'Educational Background

#Region "Previous Employer"

    Sub tbpPrevEmp_Enter(sender As Object, e As EventArgs) Handles tbpPrevEmp.Enter

        UpdateTabPageText()

        tbpPrevEmp.Text = "PREVIOUS EMPLOYER               "
        Label25.Text = "PREVIOUS EMPLOYER"

        dgvEmp_SelectionChanged(sender, e)

    End Sub

#End Region 'Previous Employer

#Region "Promotion"

    Sub tbpPromotion_Enter(sender As Object, e As EventArgs) Handles tbpPromotion.Enter

        UpdateTabPageText()

        tbpPromotion.Text = "PROMOTION               "

        Label25.Text = "PROMOTION"

        dgvEmp_SelectionChanged(sender, e)

    End Sub

#End Region 'Promotion

#Region "Salary"

    Sub tbpNewSalary_Enter(sender As Object, e As EventArgs) Handles tbpNewSalary.Enter

        UpdateTabPageText()

        tbpNewSalary.Text = "SALARY               "
        Label25.Text = "SALARY"

        dgvEmp_SelectionChanged(sender, e)

    End Sub

#End Region 'Salary

#Region "Bonus"

    Sub tbpBonus_Enter(sender As Object, e As EventArgs) Handles tbpBonus.Enter

        UpdateTabPageText()

        tbpBonus.Text = "EMPLOYEE BONUS               "
        Label25.Text = "EMPLOYEE BONUS"

        dgvEmp_SelectionChanged(sender, e)

    End Sub

#End Region 'Bonus

#Region "Attachment"

    Sub tbpAttachment_Enter(sender As Object, e As EventArgs) Handles tbpAttachment.Enter

        UpdateTabPageText()

        tbpAttachment.Text = "ATTACHMENT               "

        Label25.Text = "ATTACHMENT"

        dgvEmp_SelectionChanged(sender, e)

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
        tbpPromotion.Text = "PROMOT"
        tbpBonus.Text = "BONUS"
        tbpAttachment.Text = "ATTACH"
        tbpNewSalary.Text = "SALARY"

    End Sub

    Private Sub AddBranchLinkButton_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles AddBranchLinkButton.LinkClicked

        Dim form As New AddBranchForm
        form.ShowDialog()

        If form.HasChanges Then

            Dim branchId = GetSelectedBranch()?.RowID

            If form.LastAddedBranchId IsNot Nothing Then

                branchId = form.LastAddedBranchId

            End If

            Dim branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)
            _branches = branchRepository.GetAll()
            BranchComboBox.DataSource = _branches

            Dim currentBranch = _branches.Where(Function(b) Nullable.Equals(b.RowID, branchId)).FirstOrDefault

            If currentBranch Is Nothing Then

                BranchComboBox.SelectedIndex = -1
            Else

                BranchComboBox.SelectedIndex = _branches.IndexOf(currentBranch)
            End If

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

                    SearchEmployee_Click(Button4, New EventArgs)
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

    Private Sub Gender_CheckedChanged(sender As RadioButton, e As EventArgs) Handles rdMale.CheckedChanged,
                                                                                rdFMale.CheckedChanged
        If Not sender.Checked Then Return

        Dim label_gender = ""

        If sender.Name = rdMale.Name Then

            label_gender = "Paternity"

            LoadSalutation(Gender.Male)

            Label148.Text = label_gender
            Label149.Text = label_gender
        ElseIf sender.Name = rdFMale.Name Then

            label_gender = "Maternity"

            LoadSalutation(Gender.Female)

            Label148.Text = label_gender
            Label149.Text = label_gender
        End If

    End Sub

    Private Async Sub DisplayLeaveHistoryButton_Click(sender As Object, e As EventArgs) Handles DisplayLeaveHistoryButton.Click
        Dim employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
        Dim employee = Await employeeRepository.GetByIdAsync(CInt(publicEmpRowID))
        Dim dialog = New ViewLeaveLedgerDialog(employee)
        dialog.ShowDialog()
    End Sub

    Private Sub UserActivityEmployeeToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityEmployeeToolStripButton.Click
        Dim userActivity As New UserActivityForm(EmployeeEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Sub Print201ReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Print201ReportToolStripMenuItem.Click
        Print201Report()
    End Sub

    Private Sub tabctrlemp_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles tabctrlemp.Selecting

        Dim view_name As String = String.Empty

        Try
            view_name = e.TabPage.AccessibleDescription.Trim
        Catch ex As Exception
            view_name = String.Empty
        Finally
            If view_name.Length > 0 Then

                Dim view_row_id = VIEW_privilege(view_name, orgztnID)

                Dim formuserprivilege = position_view_table.Select("ViewID = " & view_row_id)

                Dim bool_result As Boolean = False

                bool_result = (formuserprivilege.Count = 0)

                If bool_result = False Then

                    For Each drow In formuserprivilege
                        bool_result = (drow("ReadOnly").ToString = "Y")

                        If bool_result = False Then

                            bool_result = (drow("Creates").ToString = "N" _
                                           And drow("Deleting").ToString = "N" _
                                           And drow("Updates").ToString = "N")

                        End If

                    Next

                End If

                e.Cancel = bool_result

            End If
        End Try

    End Sub

    Dim indentifyGender As Dictionary(Of Gender, String) =
        New Dictionary(Of Gender, String) From {{Gender.Male, Gender.Male.ToString()}, {Gender.Female, Gender.Female.ToString()}}

    Private Async Sub LoadSalutation(gender As Gender)
        Dim genderList = {"Neutral", indentifyGender(gender)}

        Dim listOfValueRepository = MainServiceProvider.GetRequiredService(Of ListOfValueRepository)

        Dim salutationList = Await listOfValueRepository.
                            GetFilteredListOfValuesAsync(Function(l) l.Type = "Salutation" AndAlso
                                                                genderList.Contains(l.ParentLIC))

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
                If Not String.IsNullOrWhiteSpace(.Cells(Column9.Name).Value) Then

                    cboSalut.Text = CStr(.Cells(Column9.Name).Value)
                End If

                If CStr(.Cells(Column19.Name).Value) <> gender.ToString() Then
                    cboSalut.SelectedIndex = 0
                    cboSalut.Text = String.Empty
                End If
            End With
        End If

        Colmn2.Items.Clear()
        Colmn2.Items.Add(String.Empty)
        Colmn2.Items.AddRange(salutations)

    End Sub

    Private Enum Gender
        Male
        Female
    End Enum

    Private _laGlobalEmployeeReports As New Dictionary(Of String, LaGlobalEmployeeReportName)

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

        Dim employeeBuilder = MainServiceProvider.GetRequiredService(Of EmployeeQueryBuilder)

        employee = Await employeeBuilder.IncludePosition().
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

    Private Sub InitializeLaGlobalReportList()

        If if_sysowner_is_laglobal = False Then
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

End Class