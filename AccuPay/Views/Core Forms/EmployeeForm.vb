'Imports Emgu.Util
'Imports Emgu.CV
'Imports Emgu.CV.OCR
'Imports Emgu.CV.
'Imports Tesseract.Interop
Imports System.Text.RegularExpressions
Imports System.Data.Entity
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports Microsoft.EntityFrameworkCore

Public Class EmployeeForm

    Private str_ms_excel_file_extensn As String =
        String.Concat("Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|",
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls")

    Dim sys_ownr As New SystemOwner

    Protected Overrides Sub OnLoad(e As EventArgs)
        SplitContainer2.SplitterWidth = 7
        MyBase.OnLoad(e)
    End Sub

#Region "Employee Check list"

    Dim empchklist_columns As New AutoCompleteStringCollection

    Dim view_IDEmpLoan As Integer = Nothing

    Sub tbpempchklist_Enter(sender As Object, e As EventArgs) Handles tbpempchklist.Enter

        InfoBalloon(, , txtTIN, , , 1)
        InfoBalloon(, , txtPIN, , , 1)
        InfoBalloon(, , txtHDMF, , , 1)
        InfoBalloon(, , txtSSS, , , 1)

        tabpageText(tabIndx)
        tbpempchklist.Text = "CHECK LIST               "
        Label25.Text = "CHECK LIST"
        Static once As SByte = 0

        If once = 0 Then
            once = 1

            imglstchklist.Images.Item(0).Tag = 0
            imglstchklist.Images.Item(1).Tag = 1

        End If

        view_IDEmpLoan = VIEW_privilege("Employee Loan History", orgztnID)
        tabIndx = 0 'TabControl1.SelectedIndex
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
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)
                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "Performance appraisal"
                    dgvempatta.Item("Column38", indx).Value = "Performance appraisal"
                End If
            ElseIf .Name = "chklistlinklbl_1" Then 'BIR TIN = 4
                tabctrlemp.SelectedIndex = 1
                txtTIN.Focus()
                If .ImageIndex = 0 Then
                    InfoBalloon("Please supply this field", Trim(.Text) & " checklist", txtTIN, txtTIN.Width - 16, -69)
                End If

            ElseIf .Name = "chklistlinklbl_2" Then 'Diploma = 6
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "Diploma"
                    dgvempatta.Item("Column38", indx).Value = "Diploma"
                End If
            ElseIf .Name = "chklistlinklbl_3" Then 'ID Info slip = 8
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "ID Info slip"
                    dgvempatta.Item("Column38", indx).Value = "ID Info slip"
                End If
            ElseIf .Name = "chklistlinklbl_4" Then 'Philhealth ID = 10
                tabctrlemp.SelectedIndex = 1
                txtPIN.Focus()
                If .ImageIndex = 0 Then
                    InfoBalloon("Please supply this field", Trim(.Text) & " checklist", txtPIN, txtPIN.Width - 16, -70)
                End If
            ElseIf .Name = "chklistlinklbl_5" Then 'HDMF ID = 12
                tabctrlemp.SelectedIndex = 1
                txtHDMF.Focus()
                If .ImageIndex = 0 Then
                    InfoBalloon("Please supply this field", Trim(.Text) & " checklist", txtHDMF, txtHDMF.Width - 16, -70)
                End If
            ElseIf .Name = "chklistlinklbl_6" Then 'SSS No = 14
                tabctrlemp.SelectedIndex = 1
                txtSSS.Focus()
                If .ImageIndex = 0 Then
                    InfoBalloon("Please supply this field", Trim(.Text) & " checklist", txtSSS, txtSSS.Width - 16, -70)
                End If
            ElseIf .Name = "chklistlinklbl_7" Then 'Transcript of record = 16
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "Transcript of record"
                    dgvempatta.Item("Column38", indx).Value = "Transcript of record"
                End If
            ElseIf .Name = "chklistlinklbl_8" Then 'Birth certificate = 18
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "Birth certificate"
                    dgvempatta.Item("Column38", indx).Value = "Birth certificate"
                End If
            ElseIf .Name = "chklistlinklbl_9" Then 'Employee contract = 20
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "Employee contract"
                    dgvempatta.Item("Column38", indx).Value = "Employee contract"
                End If
            ElseIf .Name = "chklistlinklbl_10" Then 'Medical exam = 22
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "Medical exam"
                    dgvempatta.Item("Column38", indx).Value = "Medical exam"
                End If
            ElseIf .Name = "chklistlinklbl_11" Then 'NBI clearance = 24

                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "NBI clearance"
                    dgvempatta.Item("Column38", indx).Value = "NBI clearance"
                End If
            ElseIf .Name = "chklistlinklbl_12" Then 'COE employer = 26
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "COE employer"
                    dgvempatta.Item("Column38", indx).Value = "COE employer"
                End If
            ElseIf .Name = "chklistlinklbl_13" Then 'Marriage contract = 28
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "Marriage contract"
                    dgvempatta.Item("Column38", indx).Value = "Marriage contract"
                End If
            ElseIf .Name = "chklistlinklbl_14" Then 'House sketch = 30
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "House sketch"
                    dgvempatta.Item("Column38", indx).Value = "House sketch"
                End If
            ElseIf .Name = "chklistlinklbl_15" Then '2305 = 32 'Training agreement = 32
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "2305" '"Training agreement"
                    dgvempatta.Item("Column38", indx).Value = "2305"
                End If
            ElseIf .Name = "chklistlinklbl_16" Then 'Health permit = 34
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "Health permit"
                    dgvempatta.Item("Column38", indx).Value = "Health permit"
                End If
            ElseIf .Name = "chklistlinklbl_17" Then 'SSS loan certificate = 36 'Valid ID = 36
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "SSS loan certificate" '"Valid ID"
                    dgvempatta.Item("Column38", indx).Value = "SSS loan certificate"
                End If
            ElseIf .Name = "chklistlinklbl_18" Then 'Resume = 38
                tabctrlemp.SelectedIndex = 17
                tbpAttachment.Focus()
                If .ImageIndex = 0 Then
                    tsbtnNewAtta_Click(sender, e)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = "Resume"
                    dgvempatta.Item("Column38", indx).Value = "Resume"
                End If
            Else
                ctrlAttachment(link_lablesender)
            End If
        End With
    End Sub

    Sub ctrlAttachment(ByVal lnk_lablesender As LinkLabel)

        If lnk_lablesender IsNot Nothing Then

            tabctrlemp.SelectedIndex = 17
            tbpAttachment.Focus()

            With lnk_lablesender

                If .ImageIndex = 0 Then

                    tsbtnNewAtta_Click(lnk_lablesender, New EventArgs)

                    Dim indx = dgvempatta.CurrentRow.Index

                    dgvempatta.Rows.Add()
                    dgvempatta.Item("eatt_Type", indx).Selected = True
                    dgvempatta.Item("eatt_Type", indx).Value = .Text.Trim
                    dgvempatta.Item("Column38", indx).Value = .Text.Trim
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
        'If q_empsearch = Nothing Then

        '    dgvRowAdder(q_employee & " ORDER BY e.LastName, e.FirstName " &
        '                ",FIELD(e.EmploymentStatus,'Resigned','Terminated')" &
        '                ",FIELD(e.RevealInPayroll,'1','0') LIMIT " & pagination & ",100;", dgvEmp)
        '    ''e.RowID DESC
        'Else
        '    dgvRowAdder(q_employee & q_empsearch & " ORDER BY e.LastName, e.FirstName DESC" &
        '                ",FIELD(e.EmploymentStatus,'Resigned','Terminated')" &
        '                ",FIELD(e.RevealInPayroll,'1','0')", dgvEmp) ', Simple)
        '    ''e.RowID DESC
        'End If

        'Dim q_search = searchCommon(ComboBox7, TextBox1,
        '                                ComboBox8, TextBox15,
        '                                ComboBox9, TextBox16,
        '                                ComboBox10, TextBox17)

        Dim param_array =
            New Object() {orgztnID,
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

        For Each drow As DataRow In dtemployee.Rows
            Dim rowArray = drow.ItemArray()
            dgvEmp.Rows.Add(rowArray)
        Next
        dtemployee.Dispose()

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

    Public Sub Print201(sender As Object, e As EventArgs) Handles ToolStripButton22.Click
        Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
        Dim provider = New Employee201ReportProvider(employeeID)
        provider.Run()
    End Sub

    Dim empBDate As String
    Dim dontUpdateEmp As SByte = 0

    Sub INSUPD_employee_01(sender As Object, e As EventArgs) Handles tsbtnSaveEmp.Click
        MaskedTextBox1.Focus()
        pbemppic.Focus()
        MaskedTextBox2.Focus()
        pbemppic.Focus()

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        If tsbtnNewEmp.Enabled = False And
            EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE EmployeeID='" & Trim(txtEmpID.Text) & "' AND OrganizationID=" & orgztnID & ");") = 1 Then
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

        ElseIf Trim(cboEmpStat.Text) = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            cboEmpStat.Focus()
            WarnBalloon("Please input an Employee status.", "Invalid Employee status", lblforballoon, 0, -69)
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
        Static null_index() As Integer = {-1, 0}
        Dim new_eRowID = Nothing
        Try
            Dim employee_restday = If(null_index.Contains(cboDayOfRest.SelectedIndex), DBNull.Value, cboDayOfRest.SelectedIndex)

            Dim agensi_rowid = If(cboAgency.SelectedValue = Nothing, DBNull.Value, cboAgency.SelectedValue)
            positID = cboPosit.SelectedValue
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
                           ValNoComma(txtvlpayp.Text),
                           ValNoComma(txtslpayp.Text),
                           ValNoComma(txtmlpayp.Text),
                           ValNoComma(txtothrpayp.Text),
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
                           If(MaskedTextBox2.Tag = Nothing, DBNull.Value, MaskedTextBox2.Tag),
                           If(MaskedTextBox1.Tag = Nothing, DBNull.Value, MaskedTextBox1.Tag),
                           (Not chkbxRevealInPayroll.Checked),
                           ValNoComma(txtUTgrace.Text),
                           agensi_rowid,
                           0)
        Catch ex As Exception
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
            InfoBalloon("Employee ID '" & txtEmpID.Text & "' has been created successfully." & vbNewLine &
                        If(rdMale.Checked, "His", "Her") & " salary was created also, you may now proceed to 'SALARY' tab and update it.", "New Employee successfully created", lblforballoon, 0, -69, , 5000)
        Else 'UPDATE employee

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

            InfoBalloon("Employee ID '" & txtEmpID.Text & "' has been updated successfully.", "Employee Update Successful", lblforballoon, 0, -69)

        End If

        With dgvEmp.Rows(dgvEmp_RowIndex)

            If tsbtnNewEmp.Enabled = False Then

                .Cells("RowID").Value = employee_RowID

            End If

            .Cells("Column1").Value = strTrimProper(txtEmpID.Text) : .Cells("Column2").Value = strTrimProper(txtFName.Text)
            .Cells("Column3").Value = strTrimProper(txtMName.Text) : .Cells("Column4").Value = strTrimProper(txtLName.Text)
            .Cells("Column5").Value = strTrimProper(txtNName.Text)
            .Cells("Column6").Value = Format(dtpempbdate.Value, machineShortDateFormat) 'dtpBDate.Value

            .Cells("Column8").Value = If(cboPosit.SelectedIndex = -1, "",
                                         If(cboPosit.SelectedIndex = (cboPosit.Items.Count - 1), Nothing, Trim(cboPosit.Text)))

            .Cells("Column9").Value = strTrimProper(cboSalut.Text) : .Cells("Column10").Value = txtTIN.Text
            .Cells("Column11").Value = strTrimProper(txtSSS.Text) : .Cells("Column12").Value = txtHDMF.Text
            .Cells("Column13").Value = strTrimProper(txtPIN.Text) : .Cells("Column15").Value = txtWorkPhne.Text
            .Cells("Column16").Value = strTrimProper(txtHomePhne.Text) : .Cells("Column17").Value = txtMobPhne.Text
            .Cells("Column18").Value = strTrimProper(txtHomeAddr.Text) : .Cells("Column14").Value = txtemail.Text
            .Cells("Column19").Value = If(rdMale.Checked, "Male", "Female")
            .Cells("Column20").Value = cboEmpStat.Text : .Cells("Column21").Value = strTrimProper(txtSName.Text)
            .Cells("Column25").Value = dbnow : .Cells("Column26").Value = u_nem

            .Cells("Column29").Value = cboPosit.SelectedValue

            .Cells("Column22").Value = paytypestring

            .Cells("Column31").Value = cboMaritStat.Text : .Cells("Column32").Value = Val(txtNumDepen.Text)
            .Cells("Column34").Value = cboEmpType.Text

            .Cells("colstartdate").Value = dtpempstartdate.Value
            .Cells("Column35").Value = txtvlallow.Text

            .Cells("Column36").Value = txtvlallow.Text
            .Cells("slallowance").Value = txtslallow.Text
            .Cells("mlallowance").Value = txtmlallow.Text

            .Cells("Column35").Value = txtvlbal.Text
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

            .Cells("OtherPayP").Value = txtothrpayp.Text

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

        End With
        tsbtnNewEmp.Enabled = True

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        tsbtnSaveEmp.Enabled = True
    End Sub

    Sub SaveEmployee(sender As Object, e As EventArgs) 'Handles tsbtnSaveEmp.Click

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        If tsbtnNewEmp.Enabled = False And
            EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE EmployeeID='" & Trim(txtEmpID.Text) & "' AND OrganizationID=" & orgztnID & ");") = 1 Then
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

        ElseIf Trim(cboEmpStat.Text) = "" Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            cboEmpStat.Focus()
            WarnBalloon("Please input an Employee status.", "Invalid Employee status", lblforballoon, 0, -69)
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

        End If

        Dim params(54, 2) As Object

        params(0, 0) = "RID"
        params(1, 0) = "UserRowID"
        params(2, 0) = "OrganizID"
        params(3, 0) = "Salutat"
        params(4, 0) = "FName"
        params(5, 0) = "MName"
        params(6, 0) = "LName"
        params(7, 0) = "Surname"
        params(8, 0) = "EmpID"
        params(9, 0) = "TIN"
        params(10, 0) = "SSS"
        params(11, 0) = "HDMF"
        params(12, 0) = "PhH"
        params(13, 0) = "EmpStatus"
        params(14, 0) = "EmailAdd"
        params(15, 0) = "WorkNo"
        params(16, 0) = "HomeNo"
        params(17, 0) = "MobileNo"
        params(18, 0) = "HAddress"
        params(19, 0) = "Nick"
        params(20, 0) = "JTitle"
        params(21, 0) = "Gend"
        params(22, 0) = "EmpType"
        params(23, 0) = "MaritStat"
        params(24, 0) = "BDate"
        params(25, 0) = "Start_Date"
        params(26, 0) = "TerminatDate"
        params(27, 0) = "PositID"
        params(28, 0) = "PayFreqID"
        params(29, 0) = "NumDependent"
        params(30, 0) = "UTOverride"
        params(31, 0) = "OTOverride"
        params(32, 0) = "NewEmpFlag"
        params(33, 0) = "LeaveBal"
        params(34, 0) = "SickBal"
        params(35, 0) = "MaternBal"
        params(36, 0) = "LeaveAllow"
        params(37, 0) = "SickAllow"
        params(38, 0) = "MaternAllow"
        params(39, 0) = "Imag"
        params(40, 0) = "LeavePayPer"
        params(41, 0) = "SickPayPer"
        params(42, 0) = "MaternPayPer"
        params(43, 0) = "IsExemptAlphaList"
        params(44, 0) = "Work_DaysPerYear"
        params(45, 0) = "Day_Rest"

        params(46, 0) = "ATM_No"
        params(47, 0) = "OtherLeavePayPer"
        params(48, 0) = "Bank_Name"
        params(49, 0) = "Calc_Holiday"

        params(50, 0) = "Calc_SpecialHoliday"
        params(51, 0) = "Calc_NightDiff"
        params(52, 0) = "Calc_NightDiffOT"
        params(53, 0) = "Calc_RestDay"
        params(54, 0) = "Calc_RestDayOT"
        '**********************
        Dim dgvEmp_RowIndex = 0
        Try
            txtEmpID.Focus()
            'dgvEmp

            If tsbtnNewEmp.Enabled = True Then
                params(0, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
            Else
                params(0, 1) = DBNull.Value
            End If

            params(1, 1) = z_User 'CreaBy
            params(2, 1) = orgztnID 'OrganizID
            params(4, 1) = Trim(cboSalut.Text) 'Salutat
            params(4, 1) = Trim(txtFName.Text) 'FName
            params(5, 1) = Trim(txtMName.Text) 'MName
            params(6, 1) = Trim(txtLName.Text) 'LName
            params(7, 1) = Trim(txtSName.Text) 'Surname
            params(8, 1) = Trim(txtEmpID.Text) 'EmpID
            params(9, 1) = txtTIN.Text 'TIN
            params(10, 1) = txtSSS.Text 'SSS
            params(11, 1) = txtHDMF.Text 'HDMF
            params(12, 1) = txtPIN.Text 'PhH
            params(13, 1) = cboEmpStat.Text 'EmpStatus
            params(14, 1) = Trim(txtemail.Text) 'EmailAdd
            params(15, 1) = Trim(txtWorkPhne.Text) 'WorkNo
            params(16, 1) = Trim(txtHomePhne.Text) 'HomeNo
            params(17, 1) = Trim(txtMobPhne.Text) 'MobileNo
            params(18, 1) = Trim(txtHomeAddr.Text) 'HAddress
            params(19, 1) = Trim(txtNName.Text) 'Nick
            params(20, 1) = Trim(txtDivisionName.Text) 'JTitle
            params(21, 1) = If(rdMale.Checked, "M", "F") 'Gend
            params(22, 1) = cboEmpType.Text 'EmpType
            params(23, 1) = cboMaritStat.Text 'MaritStat
            params(24, 1) = Format(CDate(dtpempbdate.Value), "yyyy-MM-dd") 'BDate
            params(25, 1) = Format(CDate(dtpempstartdate.Value), "yyyy-MM-dd") 'Start_Date

            params(26, 1) = DBNull.Value 'If(termdate = "", DBNull.Value, Format(CDate(termdate), "yyyy-MM-dd"))

            Dim positn_ID = If(cboPosit.SelectedIndex = -1 Or cboPosit.Text = "",
                               DBNull.Value,
                               If(getStrBetween(positn.Item(cboPosit.SelectedIndex).ToString, "", "@") = "NULL",
                                  DBNull.Value,
                                  getStrBetween(positn.Item(cboPosit.SelectedIndex).ToString, "", "@")))

            params(27, 1) = positID 'positn_ID 'PositID

            Dim pay_freqID = EXECQUER("SELECT RowID FROM payfrequency WHERE PayFrequencyType='" & cboPayFreq.Text & "';")
            params(28, 1) = cboPayFreq.SelectedValue 'If(Val(pay_freqID) = 0, DBNull.Value, pay_freqID) 'PayFreqID

            Dim Employee_RowID_ID = Nothing

            If dgvEmp.RowCount <> 0 Then
                Employee_RowID_ID = dgvEmp.CurrentRow.Cells("RowID").Value
            End If

            Dim count_Dependents = EXECQUER("SELECT COUNT(edep.RowID)" &
                                     " FROM employeedependents edep" &
                                     " LEFT JOIN employee e ON e.RowID=edep.ParentEmployeeID AND e.OrganizationID=edep.OrganizationID" &
                                     " WHERE e.RowID='" & Employee_RowID_ID & "'" &
                                     " AND edep.ActiveFlag='Y'" &
                                     " AND edep.OrganizationID='" & orgztnID & "';") 'NumDependent

            count_Dependents = IntVal(count_Dependents)

            params(29, 1) = count_Dependents
            params(30, 1) = If(chkutflag.Checked, "1", "0") 'UTOverride
            params(31, 1) = If(chkotflag.Checked, "1", "0") 'UTOverride
            params(32, 1) = If(cboEmpStat.Text = "Probationary", "1", "0") 'NewEmpFlag
            params(33, 1) = txtvlbal.Text 'LeaveBal
            params(34, 1) = txtslbal.Text 'SickBal
            params(35, 1) = txtmlbal.Text 'MaternBal
            params(36, 1) = txtvlallow.Text 'LeaveAllow
            params(37, 1) = txtslallow.Text 'SickAllow
            params(38, 1) = txtmlallow.Text 'MaternAllow
            '*********IMAGE
            If File.Exists(Path.GetTempPath & "tmpfileEmployeeImage.jpg") Then 'pbemppic.Image = Nothing
                params(39, 1) = convertFileToByte(Path.GetTempPath & "tmpfileEmployeeImage.jpg") 'ang gawin mo from image, convert into Byte()
            ElseIf empPic = "" Then
                params(39, 1) = DBNull.Value
            ElseIf empPic <> "" Then
                If File.Exists(Path.GetTempPath & "tmpfileEmployeeImage.jpg") Then
                    params(39, 1) = convertFileToByte(empPic)
                Else
                    params(39, 1) = DBNull.Value
                End If
            End If
            '*********IMAGE
            params(40, 1) = ValNoComma(txtvlpayp.Text) 'LeavePayPer
            params(41, 1) = ValNoComma(txtslpayp.Text) 'SickPayPer
            params(42, 1) = ValNoComma(txtmlpayp.Text) 'MaternPayPer
            params(43, 1) = If(chkAlphaListExempt.Checked, "0", "1")
            params(44, 1) = ValNoComma(txtWorkDaysPerYear.Text)
            If cboDayOfRest.SelectedIndex = -1 Then
                params(45, 1) = "1"
            Else
                params(45, 1) = cboDayOfRest.SelectedIndex + 1
            End If
            params(46, 1) = txtATM.Text.Trim
            params(47, 1) = ValNoComma(txtothrpayp.Text)
            params(48, 1) = cbobank.Text
            params(49, 1) = If(chkcalcHoliday.Checked, "Y", "N")
            params(50, 1) = If(chkcalcSpclHoliday.Checked, "Y", "N")
            params(51, 1) = If(chkcalcNightDiff.Checked, "Y", "N")
            params(52, 1) = If(chkcalcNightDiffOT.Checked, "Y", "N")
            params(53, 1) = If(chkcalcRestDay.Checked, "Y", "N")
            params(54, 1) = If(chkcalcRestDayOT.Checked, "Y", "N")

            Dim emplo_RowID =
                EXEC_INSUPD_PROCEDURE(params,
                                       "INSUPD_employee_01",
                                       "returnval")

            If tsbtnNewEmp.Enabled = False Then 'INSERT employee

                Dim new_drow As DataRow
                new_drow = employeepix.NewRow
                new_drow("RowID") = emplo_RowID
                new_drow("Image") = params(40, 1) 'If(empPic = Nothing, "", convertFileToByte(empPic))
                employeepix.Rows.Add(new_drow)

                'emp_rcount
                If dgvEmp.RowCount = 0 Then
                    dgvEmp.Rows.Add()
                Else : dgvEmp.Rows.Insert(0, 1)

                End If

                emp_rcount += 1 ': isDupEmpID = 0

                dgvEmp_RowIndex = 0

                InfoBalloon("Employee ID '" & txtEmpID.Text & "' has been created successfully." & vbNewLine &
                            If(rdMale.Checked, "His", "Her") & " salary was created also, you may now proceed to 'SALARY' tab and update it.", "New Employee successfully created", lblforballoon, 0, -69, , 5000)
            Else 'UPDATE employee

                If dontUpdateEmp = 1 Then
                    tsbtnNewEmp.Enabled = True
                    AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
                    Exit Sub
                End If

                For Each drow As DataRow In employeepix.Rows
                    If drow("RowID").ToString = dgvEmp.CurrentRow.Cells("RowID").Value Then
                        drow("Image") = Nothing
                        drow("Image") = params(40, 1) 'If(empPic = Nothing, "", convertFileToByte(empPic))
                        'employeepix.Rows.Remove(drow)
                        Exit For
                    End If
                Next

                dgvEmp_RowIndex = dgvEmp.CurrentRow.Index
                InfoBalloon("Employee ID '" & txtEmpID.Text & "' has been updated successfully.", "Employee Update Successful", lblforballoon, 0, -69)
            End If

            With dgvEmp.Rows(dgvEmp_RowIndex)
                If tsbtnNewEmp.Enabled = False Then
                    .Cells("RowID").Value = emplo_RowID
                End If

                .Cells("Column1").Value = strTrimProper(txtEmpID.Text) : .Cells("Column2").Value = strTrimProper(txtFName.Text)
                .Cells("Column3").Value = strTrimProper(txtMName.Text) : .Cells("Column4").Value = strTrimProper(txtLName.Text)
                .Cells("Column5").Value = strTrimProper(txtNName.Text)
                .Cells("Column6").Value = Format(dtpempbdate.Value, machineShortDateFormat) 'dtpBDate.Value
                .Cells("Column7").Value = Trim(txtDivisionName.Text)

                .Cells("Column8").Value = If(cboPosit.SelectedIndex = -1, "",
                                             If(cboPosit.SelectedIndex = (cboPosit.Items.Count - 1), Nothing, Trim(cboPosit.Text)))

                .Cells("Column9").Value = strTrimProper(cboSalut.Text) : .Cells("Column10").Value = txtTIN.Text
                .Cells("Column11").Value = strTrimProper(txtSSS.Text) : .Cells("Column12").Value = txtHDMF.Text
                .Cells("Column13").Value = strTrimProper(txtPIN.Text) : .Cells("Column15").Value = txtWorkPhne.Text
                .Cells("Column16").Value = strTrimProper(txtHomePhne.Text) : .Cells("Column17").Value = txtMobPhne.Text
                .Cells("Column18").Value = strTrimProper(txtHomeAddr.Text) : .Cells("Column14").Value = txtemail.Text
                .Cells("Column19").Value = If(rdMale.Checked, "Male", "Female")
                .Cells("Column20").Value = cboEmpStat.Text : .Cells("Column21").Value = strTrimProper(txtSName.Text)
                .Cells("Column25").Value = dbnow : .Cells("Column26").Value = u_nem

                .Cells("Column29").Value = If(cboPosit.SelectedIndex = -1 Or cboPosit.Text = "", "",
                                             If(cboPosit.SelectedIndex = (cboPosit.Items.Count - 1), Nothing, getStrBetween(positn.Item(cboPosit.SelectedIndex), "", "@")))

                .Cells("Column22").Value = paytypestring

                .Cells("Column31").Value = cboMaritStat.Text : .Cells("Column32").Value = Val(txtNumDepen.Text)
                .Cells("Column34").Value = cboEmpType.Text

                .Cells("colstartdate").Value = dtpempstartdate.Value '.ToString.Replace("/", "-")

                .Cells("Column35").Value = txtvlallow.Text

                .Cells("Column36").Value = txtvlallow.Text
                .Cells("slallowance").Value = txtslallow.Text
                .Cells("mlallowance").Value = txtmlallow.Text

                .Cells("Column35").Value = txtvlbal.Text
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
                .Cells("OtherPayP").Value = txtothrpayp.Text
                .Cells("OtherLeaveAllowance").Value = txtothrallow.Text
                .Cells("OtherLeaveBalance").Value = txtothrbal.Text

                .Cells("CalcHoliday").Value = If(chkcalcHoliday.Checked, "Y", "N")
                .Cells("CalcSpecialHoliday").Value = If(chkcalcSpclHoliday.Checked, "Y", "N")
                .Cells("CalcNightDiff").Value = If(chkcalcNightDiff.Checked, "Y", "N")
                .Cells("CalcNightDiffOT").Value = If(chkcalcNightDiffOT.Checked, "Y", "N")
                .Cells("CalcRestDay").Value = If(chkcalcRestDay.Checked, "Y", "N")
                .Cells("CalcRestDayOT").Value = If(chkcalcRestDayOT.Checked, "Y", "N")
            End With
        Catch ex As Exception

            Dim catch_errstr = getErrExcptn(ex, Me.Name)

            If catch_errstr.Contains("Object reference not set to an instance of an object.") Then
                WarnBalloon("No employee is selected", "Please select an employee.", lblforballoon, 0, -69)
            Else
                WarnBalloon(catch_errstr, catch_errstr, lblforballoon, 0, -69)
            End If
        Finally
            tsbtnNewEmp.Enabled = True
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
        End Try
    End Sub

    Sub tsbtnSaveEmp_Click(sender As Object, e As EventArgs) 'Handles tsbtnSaveEmp.Click
        If tsbtnSaveEmp.Visible = False Then : Exit Sub : End If
        Static isDupEmpID As SByte
        If isDupEmpID = 0 And tsbtnNewEmp.Enabled = False Then
            For Each r As DataGridViewRow In dgvEmp.Rows
                If strTrimProper(txtEmpID.Text) = r.Cells("Column1").Value Then : isDupEmpID = 1 : Exit For : Else : isDupEmpID = 0 : End If
            Next
        End If

        If isDupEmpID = 1 And tsbtnNewEmp.Enabled = False Then
            txtEmpID.Focus()
            WarnBalloon("Employee ID " & txtEmpID.Text & " is already exist, please try another.", "Invalid Employee ID", txtEmpID, txtEmpID.Width - 16, -69) : isDupEmpID = 0 : Exit Sub
        ElseIf strTrimProper(txtFName.Text) = "" Then
            txtFName.Focus()
            WarnBalloon("Please input First name", "Invalid First name", txtFName, txtFName.Width - 16, -69) : Exit Sub
        ElseIf strTrimProper(txtLName.Text) = "" Then
            txtLName.Focus()
            WarnBalloon("Please input Last name", "Invalid Last name", txtLName, txtLName.Width - 16, -69) : Exit Sub
        ElseIf cboMaritStat.Text = "" Or cboMaritStat.SelectedIndex = -1 Then
            cboMaritStat.Focus()
            WarnBalloon("Please input a Marital Status", "Invalid Marital Status", cboMaritStat, cboMaritStat.Width - 16, -69) : Exit Sub
        End If

        Dim _gend = If(rdMale.Checked, "M", "F")

        If EXECQUER("SELECT EXISTS(SELECT RowID FROM listofval lov WHERE lov.Type='Salutation' AND Active='Yes' AND DisplayValue='" & cboSalut.Text & "')") = 0 And cboSalut.Text <> "" Then
            INS_LoL(cboSalut.Text, cboSalut.Text, "Salutation", , "Yes", , , 1) : cboSalut.Items.Add(cboSalut.Text)
        End If

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
                    "',FirstName='" & strTrimProper(txtFName.Text) &
                    "',MiddleName='" & strTrimProper(txtMName.Text) &
                    "',LastName='" & strTrimProper(txtLName.Text) &
                    "',Surname='" & txtSName.Text &
                    "',Nickname='" & strTrimProper(txtNName.Text) &
                    "',Birthdate='" & empBDate &
                    "',JobTitle='" & Trim(txtDivisionName.Text) &
                    "',Salutation='" & strTrimProper(cboSalut.Text) &
                    "',TINNo='" & txtTIN.Text &
                    "',SSSNo='" & txtSSS.Text &
                    "',HDMFNo='" & txtHDMF.Text &
                    "',PhilHealthNo='" & txtPIN.Text &
                    "',WorkPhone='" & strTrimProper(txtWorkPhne.Text) &
                    "',HomePhone='" & strTrimProper(txtHomePhne.Text) &
                    "',MobilePhone='" & strTrimProper(txtMobPhne.Text) &
                    "',HomeAddress='" & strTrimProper(txtHomeAddr.Text) &
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
                        .Cells("Column1").Value = strTrimProper(txtEmpID.Text) : .Cells("Column2").Value = strTrimProper(txtFName.Text)
                        .Cells("Column3").Value = strTrimProper(txtMName.Text) : .Cells("Column4").Value = strTrimProper(txtLName.Text)
                        .Cells("Column5").Value = strTrimProper(txtNName.Text) : .Cells("Column6").Value = Format(CDate(empBDate), machineShortDateFormat) 'dtpBDate.Value
                        .Cells("Column7").Value = Trim(txtDivisionName.Text) : .Cells("Column8").Value = cboPosit.Text
                        .Cells("Column9").Value = strTrimProper(cboSalut.Text) : .Cells("Column10").Value = txtTIN.Text
                        .Cells("Column11").Value = strTrimProper(txtSSS.Text) : .Cells("Column12").Value = txtHDMF.Text
                        .Cells("Column13").Value = strTrimProper(txtPIN.Text) : .Cells("Column15").Value = txtWorkPhne.Text
                        .Cells("Column16").Value = strTrimProper(txtHomePhne.Text) : .Cells("Column17").Value = txtMobPhne.Text
                        .Cells("Column18").Value = strTrimProper(txtHomeAddr.Text) : .Cells("Column14").Value = txtemail.Text
                        .Cells("Column19").Value = If(_gend = "M", "Male", "Female")
                        .Cells("Column20").Value = cboEmpStat.Text : .Cells("Column21").Value = strTrimProper(txtSName.Text)
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

                    enlistToCboBox(q_salut, cboSalut) : salutn_count = cboSalut.Items.Count
                    '"Surname" = "Column21" : "PayFrequency" = "Column22"
                    '"UndertimeOverride" = "Column23" : "OvertimeOverride" = "Column24"
                End With
                InfoBalloon("Employee ID '" & txtEmpID.Text & "' has successfully updated.", "Employee Update Successful", lblforballoon, 0, -69)
            End If
        Else 'Format(dtpBDate.Value, "yyyy-MM-dd")

            Dim _RowID = INS_employee(txtEmpID.Text,
                                cboEmpStat.Text, _gend, Trim(txtDivisionName.Text), positID, strTrimProper(cboSalut.Text), strTrimProper(txtFName.Text),
                                strTrimProper(txtMName.Text),
                                strTrimProper(txtLName.Text), strTrimProper(txtNName.Text),
                                empBDate,
                                txtTIN.Text, txtSSS.Text, txtHDMF.Text, txtPIN.Text, txtemail.Text,
                                txtWorkPhne.Text, txtHomePhne.Text,
                                txtMobPhne.Text, strTrimProper(txtHomeAddr.Text), payFreqID, , , strTrimProper(txtSName.Text),
                                cboMaritStat.Text, Val(txtNumDepen.Text), 0, strTrimProper(cboEmpType.Text))
            _EmpRowID = _RowID
            RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            enlistToCboBox(q_salut, cboSalut) : salutn_count = cboSalut.Items.Count

            If hasERR = 0 Then
                If emp_rcount = 0 Then
                    dgvEmp.Rows.Add()
                Else : dgvEmp.Rows.Insert(0, 1)
                End If

                emp_rcount += 1 : isDupEmpID = 0

                With dgvEmp.Rows(0)
                    .Cells("RowID").Value = _RowID
                    .Cells("Column1").Value = strTrimProper(txtEmpID.Text) : .Cells("Column2").Value = strTrimProper(txtFName.Text)
                    .Cells("Column3").Value = strTrimProper(txtMName.Text) : .Cells("Column4").Value = strTrimProper(txtLName.Text)
                    .Cells("Column5").Value = strTrimProper(txtNName.Text) : .Cells("Column6").Value = Format(CDate(empBDate), machineShortDateFormat) 'dtpBDate.Value
                    .Cells("Column7").Value = Trim(txtDivisionName.Text) : .Cells("Column8").Value = cboPosit.Text
                    .Cells("Column9").Value = strTrimProper(cboSalut.Text) : .Cells("Column10").Value = txtTIN.Text
                    .Cells("Column11").Value = strTrimProper(txtSSS.Text) : .Cells("Column12").Value = txtHDMF.Text
                    .Cells("Column13").Value = strTrimProper(txtPIN.Text) : .Cells("Column15").Value = txtWorkPhne.Text
                    .Cells("Column16").Value = strTrimProper(txtHomePhne.Text) : .Cells("Column17").Value = txtMobPhne.Text
                    .Cells("Column18").Value = strTrimProper(txtHomeAddr.Text) : .Cells("Column14").Value = txtemail.Text
                    .Cells("Column19").Value = If(_gend = "M", "Male", "Female")
                    .Cells("Column20").Value = cboEmpStat.Text : .Cells("Column21").Value = strTrimProper(txtSName.Text)
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

        newEmpType.Close()
        newEmpStat.Close()
        newPostion.Close()
    End Sub

    Private Sub Employee_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim result = Windows.Forms.DialogResult.Yes

        Dim prompt = Nothing

        Select Case tabIndx
            Case 0
            Case 1 'PERSONAL PROFILE
                If tsbtnNewEmp.Enabled = False Or
                    listofEditDepen.Count <> 0 Then
                End If
            Case 2 'SALARY
                'btnNewSal
            Case 3 'AWARDS
                If listofEditRowAward.Count <> 0 Then
                End If
            Case 4 'CERTIFICATIONS
                If listofEditRowCert.Count <> 0 Then
                End If
            Case 5 'LEAVE
                If listofEditRowleave.Count <> 0 Then
                End If
            Case 6 'MEDICAL PROFILE
            Case 7 'DISCIPLINARY ACTION
                If btnNew.Enabled = False Then
                End If
            Case 8 'EDUCATIONAL BACKGROUND
                If btnNewEduc.Enabled = False Then
                End If
            Case 9 'PREVIOUS EMPLOYER
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

            InfoBalloon(, , txtstartdate, , , 1)
            InfoBalloon(, , txtstarttime, , , 1)

            InfoBalloon(, , Label232, , , 1)
            InfoBalloon(, , Label233, , , 1)
            InfoBalloon(, , Label234, , , 1)
            InfoBalloon(, , Label235, , , 1)

            InfoBalloon(, , Label367, , , 1)

            WarnBalloon(, , cboloantype, , , 1)

            newPostion.Close()
            newEmpStat.Close()
            newEmpType.Close()
            leavtyp.Close()

            showAuditTrail.Close()

            MDIPrimaryForm.lblCreatedBy.Text = Nothing
            MDIPrimaryForm.lblCreatedDate.Text = Nothing
            MDIPrimaryForm.lblUpdatedBy.Text = Nothing
            MDIPrimaryForm.lblUpdatedDate.Text = Nothing

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

    Sub loadPayFreqType()
        enlistTheLists("SELECT CONCAT(RowID,'@',PayFrequencyType) FROM payfrequency", payFreq)
        For Each r In payFreq
            cboPayFreq.Items.Add(StrReverse(getStrBetween(StrReverse(r), "", "@")))
        Next
    End Sub

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
        cboPosit.DataSource = n_SQLQueryToDatatable.ResultTable
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
        cboPosit.DataSource = n_SQLQueryToDatatable.ResultTable
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

        If dbnow = Nothing Then
            dbnow = EXECQUER(CURDATE_MDY)
        End If

        previousForm = Me
        'dbconn()
        view_ID = VIEW_privilege("Employee Personal Profile", orgztnID)

        loademployee()
        'loadPayFreqType()

        u_nem = EXECQUER(USERNameStrPropr & z_User)

        paytypestring = EXECQUER("SELECT PayFrequencyType FROM payfrequency pfq LEFT JOIN organization org ON org.PayFrequencyID=pfq.RowID WHERE org.RowID='" & orgztnID & "' LIMIT 1;")

        employeepix = retAsDatTbl("SELECT e.RowID,COALESCE(e.Image,'') 'Image' FROM employee e WHERE e.OrganizationID=" & orgztnID & " ORDER BY e.RowID DESC;")

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
    End Sub

    Private Sub Employee_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd

        InfoBalloon(, , lblforballoon1, , , 1)

        InfoBalloon(, , lblforballoon, , , 1)
        WarnBalloon(, , lblforballoon, , , 1)

        myBalloon(, , lblforballoon, , , 1)

        Select Case tabIndx
            Case 0
                'tbpempchklist.Text = "CHECKLIST"
            Case 1
                'tbpEmployee.Text = "PERSON"
                WarnBalloon(, , txtEmpID, , , 1)
                WarnBalloon(, , txtFName, , , 1)
                WarnBalloon(, , txtLName, , , 1)
            Case 2
                'tbpSalary.Text = "SALARY"
            Case 3
                'tbpAwards.Text = "AWARD"
            Case 4
                'tbpCertifications.Text = "CERTI"
            Case 5
                'tbpLeave.Text = "LEAVE"
                InfoBalloon(, , txtstartdate, , , 1)
                InfoBalloon(, , txtstarttime, , , 1)
            Case 6
                'tbpMedRec.Text = "MEDIC"
            Case 7
                'tbpDiscipAct.Text = "DISCIP"
            Case 8
                'tbpEducBG.Text = "EDUC"
            Case 9
                'tbpPrevEmp.Text = "PREV EMP"
            Case 10
                'tbpPromotion.Text = "PROMOT"
            Case 11
                'tbpLoans.Text = "LOAN SCH"
            Case 12
                'tbpLoanHist.Text = "LOAN H"
            Case 13
                'tbpPayslip.Text = "PAYSLIP"
            Case 14
                'tbpempallow.Text = "ALLOW"
            Case 15
                'tbpEmpOT.Text = "EMP OT"
            Case 16
                'tbpOBF.Text = "OFFBUSI"
            Case 17
                'tbpBonus.Text = "BONUS"
            Case 18
                'tbpAttachment.Text = "ATTACH"
                WarnBalloon(, , cboattatype, , , 1)
        End Select
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click, tsbtnCloseempawar.Click, tsbtnCloseempcert.Click, ToolStripButton4.Click,
                                                                    btnClose.Click, ToolStripButton5.Click, ToolStripButton13.Click,
                                                                   ToolStripButton18.Click, ToolStripButton24.Click, ToolStripButton30.Click, ToolStripButton32.Click,
                                                                   ToolStripButton2.Click, ToolStripButton7.Click,
                                                                   ToolStripButton11.Click, tsbtnCloseOBF.Click, ToolStripButton9.Click, ToolStripButton7.Click,
                                                                   ToolStripButton16.Click, ToolStripButton12.Click 'ToolStripButton12.Click
        Me.Close()
    End Sub

    Private Sub txt_Leave(sender As Object, e As EventArgs) Handles txtFName.Leave, txtFName.Leave, txtHomeAddr.Leave,
                                                                    txtLName.Leave, txtMName.Leave, txtNName.Leave, txtSName.Leave ', txtJTtle.Leave
        With DirectCast(sender, TextBox)
            .Text = StrConv(.Text, VbStrConv.ProperCase)
        End With
    End Sub

    Dim PositE_asc As String

    Private Sub cboPosit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboPosit.KeyPress
        e.Handled = True
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

    Sub dgvEmp_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvEmp.SelectionChanged

        RemoveHandler cboPosit.SelectedIndexChanged, AddressOf cboPosit_SelectedIndexChanged
        RemoveHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged

        If tsbtnNewEmp.Enabled = 0 Then
            cboEmpStat.Enabled = 1
            tsbtnNewEmp.Enabled = 1
        End If

        is_NewEducBG = 0 'Educational Background
        IsNewDiscip = 0 'Disciplinary Action
        IsNewPrevEmp = 0 'Previous Employer
        IsNewPromot = 0 'Promotion
        interest_charging_amt = 0
        publicEmpRowID = String.Empty

        If dgvEmp.RowCount <> 0 Then
            With dgvEmp.CurrentRow
                Dim empPix() As DataRow
                publicEmpRowID = .Cells("RowID").Value
                If sameEmpID <> .Cells("RowID").Value Then

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

                    txtFName.Text = .Cells("Column2").Value
                    txtMName.Text = .Cells("Column3").Value
                    txtLName.Text = .Cells("Column4").Value
                    txtSName.Text = .Cells("Column21").Value

                    employeefullname = .Cells("Column2").Value

                    Dim addtlWord = Nothing

                    If .Cells("Column3").Value = Nothing Then
                    Else

                        Dim midNameTwoWords = Split(.Cells("Column3").Value.ToString, " ")
                        addtlWord = " "
                        For Each s In midNameTwoWords
                            addtlWord &= (StrConv(Microsoft.VisualBasic.Left(s, 1), VbStrConv.ProperCase) & ".")
                        Next
                    End If

                    employeefullname = employeefullname & addtlWord
                    employeefullname = employeefullname & " " & .Cells("Column4").Value
                    employeefullname = employeefullname & If(.Cells("Column21").Value = Nothing,
                                                             "",
                                                             "-" & StrConv(.Cells("Column21").Value,
                                                                           VbStrConv.ProperCase))
                    '
                    LastFirstMidName = .Cells("Column4").Value & ", " & .Cells("Column2").Value &
                        If(Trim(addtlWord) = Nothing, "", If(Trim(addtlWord) = ".", "", ", " & addtlWord))

                    subdetails = "ID# " & .Cells("Column1").Value &
                                If(.Cells("Column8").Value = Nothing,
                                                                   "",
                                                                   ", " & .Cells("Column8").Value) &
                                If(.Cells("Column34").Value = Nothing,
                                                                   "",
                                                                   ", " & .Cells("Column34").Value & " salary")

                End If

                Dim employeeID = ConvertToType(Of Integer?)(publicEmpRowID)
                Dim employee As Employee = Nothing
                Using context = New PayrollContext()
                    employee = (From emp In context.Employees.
                                    Include(Function(emp) emp.PayFrequency).
                                    Include(Function(emp) emp.Position)
                                Where CBool(emp.RowID = employeeID)).
                               FirstOrDefault()
                End Using

                Dim selectedTab = tabctrlemp.SelectedTab

                If selectedTab Is tbpempchklist Then

                    txtEmpIDChk.Text = subdetails '"ID# " & .Cells("Column1").Value

                    txtFNameChk.Text = employeefullname
                    pbEmpPicChk.Image = Nothing
                    pbEmpPicChk.Image = EmployeeImage
                    lblyourrequirement.Text = .Cells("Column2").Value & "'s requirements"
                    VIEW_employeechecklist(.Cells("RowID").Value)

                ElseIf selectedTab Is tbpEmployee Then 'Employee

                    txtNName.Text = .Cells("Column5").Value
                    txtDivisionName.Text = .Cells("Column7").Value

                    If .Cells("Column6").Value = Nothing Then
                        dtpempbdate.Value = Format(CDate(dbnow), machineShortDateFormat)
                    Else
                        dtpempbdate.Value = Format(CDate(.Cells("Column6").Value), machineShortDateFormat)
                    End If

                    txtTIN.Text = .Cells("Column10").Value : txtSSS.Text = .Cells("Column11").Value
                    txtHDMF.Text = .Cells("Column12").Value : txtPIN.Text = .Cells("Column13").Value
                    txtWorkPhne.Text = .Cells("Column15").Value : txtHomePhne.Text = .Cells("Column16").Value
                    txtMobPhne.Text = .Cells("Column17").Value : txtHomeAddr.Text = .Cells("Column18").Value
                    txtemail.Text = .Cells("Column14").Value

                    If .Cells("Column22").Value = "" Then : cboPayFreq.SelectedIndex = -1 : cboPayFreq.Text = ""
                    Else : cboPayFreq.Text = .Cells("Column22").Value
                    End If

                    RemoveHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged

                    If .Cells("Column20").Value = "" Then
                        cboEmpStat.SelectedIndex = -1
                        cboEmpStat.Text = ""
                    Else
                        cboEmpStat.Text = .Cells("Column20").Value
                    End If

                    reloadPositName(.Cells("Column29").Value)  ': cboPosit.Text = .Cells("Column8").Value

                    cboPosit.Text = .Cells("Column8").Value

                    AddHandler cboPosit.SelectedIndexChanged, AddressOf cboPosit_SelectedIndexChanged

                    If .Cells("Column9").Value = "" Then
                        cboSalut.SelectedIndex = -1
                        cboSalut.Text = ""
                    Else
                        cboSalut.Text = .Cells("Column9").Value
                    End If
                    '"UndertimeOverride" = "Column23" : "OvertimeOverride" = "Column24"
                    '"Creation date" = "Column25" : "Created by" = "Column26"
                    '"Last update" = "Column27" : "Last Update by" = "Column28"
                    If .Cells("Column31").Value = "" Then
                        cboMaritStat.SelectedIndex = -1
                        cboMaritStat.Text = ""
                    Else
                        cboMaritStat.Text = .Cells("Column31").Value
                    End If

                    cboEmpType.SelectedIndex = -1
                    cboEmpType.Text = ""

                    cboEmpType.Text = .Cells("Column34").Value

                    txtNumDepen.Text = Val(.Cells("Column32").Value)
                    If .Cells("Column19").Value = "Male" Then
                        rdMale.Checked = True
                    Else
                        rdFMale.Checked = True
                    End If
                    noCurrCellChange = 0
                    dtpempstartdate.Value = CDate(.Cells("colstartdate").Value) '.ToString.Replace("-", "/")

                    pbemppic.Image = Nothing
                    pbemppic.Image = EmployeeImage
                    txtEmpID.Text = .Cells("Column1").Value
                    txtFName.Text = .Cells("Column2").Value
                    txtMName.Text = .Cells("Column3").Value
                    txtLName.Text = .Cells("Column4").Value
                    txtSName.Text = .Cells("Column21").Value

                    Static case_one As Integer = -1
                    If case_one <> sameEmpID Then
                        case_one = sameEmpID
                        VIEW_employeedependents(.Cells("RowID").Value)
                        dependentitemcount = dgvDepen.RowCount - 1
                    ElseIf case_one = sameEmpID Then
                        VIEW_employeedependents(.Cells("RowID").Value)
                    Else
                        If dgvDepen.RowCount = 1 Then
                        Else

                            If dgvDepen.CurrentRow.Index >= dependentitemcount Then
                                ''MsgBox("If")
                                If dependentitemcount = -1 Then
                                    VIEW_employeedependents(.Cells("RowID").Value)
                                    dependentitemcount = dgvDepen.RowCount - 1
                                End If
                            Else
                                ''MsgBox("else")
                                dgvDepen.Rows.Clear()
                                VIEW_employeedependents(.Cells("RowID").Value)
                                dependentitemcount = dgvDepen.RowCount - 1
                            End If

                        End If
                    End If

                    txtvlallow.Text = .Cells("Column36").Value
                    txtslallow.Text = .Cells("slallowance").Value
                    txtmlallow.Text = .Cells("mlallowance").Value

                    txtvlbal.Text = .Cells("Column35").Value
                    txtslbal.Text = .Cells("slbalance").Value
                    txtmlbal.Text = .Cells("mlbalance").Value

                    txtvlpayp.Text = .Cells("Column33").Value
                    txtslpayp.Text = .Cells("slpayp").Value
                    txtmlpayp.Text = .Cells("mlpayp").Value

                    chkutflag.Checked = If(.Cells("Column23").Value = 1, True, False)
                    chkotflag.Checked = If(.Cells("Column24").Value = 1, True, False)

                    chkAlphaListExempt.Checked = If(.Cells("AlphaExempted").Value = 0, True, False)
                    txtWorkDaysPerYear.Text = .Cells("WorkDaysPerYear").Value
                    cboDayOfRest.Text = String.Empty
                    cboDayOfRest.Text = .Cells("DayOfRest").Value
                    txtATM.Text = .Cells("ATMNo").Value
                    txtothrpayp.Text = .Cells("OtherPayP").Value
                    txtothrallow.Text = .Cells("OtherLeaveAllowance").Value
                    txtothrbal.Text = .Cells("OtherLeaveBalance").Value
                    If .Cells("ATMNo").Value = Nothing Then
                        rdbCash.Checked = True
                        rdbDirectDepo.Checked = False
                    Else
                        rdbCash.Checked = False
                        rdbDirectDepo.Checked = True
                    End If

                    rdbDirectDepo_CheckedChanged(rdbDirectDepo, New EventArgs)

                    chkcalcHoliday.Checked = Convert.ToInt16(.Cells("CalcHoliday").Value) 'If(.Cells("CalcHoliday").Value = "Y", True, False)
                    chkcalcSpclHoliday.Checked = Convert.ToInt16(.Cells("CalcSpecialHoliday").Value) 'If(.Cells("CalcSpecialHoliday").Value = "Y", True, False)
                    chkcalcNightDiff.Checked = Convert.ToInt16(.Cells("CalcNightDiff").Value) 'If(.Cells("CalcNightDiff").Value = "Y", True, False)
                    chkcalcNightDiffOT.Checked = Convert.ToInt16(.Cells("CalcNightDiffOT").Value) 'If(.Cells("CalcNightDiffOT").Value = "Y", True, False)
                    chkcalcRestDay.Checked = Convert.ToInt16(.Cells("CalcRestDay").Value) 'If(.Cells("CalcRestDay").Value = "Y", True, False)
                    chkcalcRestDayOT.Checked = Convert.ToInt16(.Cells("CalcRestDayOT").Value) 'If(.Cells("CalcRestDayOT").Value = "Y", True, False)

                    chkbxRevealInPayroll.Checked =
                        (Not CBool(Convert.ToInt16(.Cells("RevealInPayroll").Value)))

                    txtUTgrace.Text = .Cells("LateGracePeriod").Value 'AgencyName
                    cboAgency.Text = .Cells("AgencyName").Value

                    Dim n_fdsfd As _
                        New ExecuteQuery("SELECT CONCAT(IFNULL(DATE_FORMAT(DateEvaluated,@@date_format),''),',',IFNULL(DATE_FORMAT(DateRegularized,@@date_format),'')) FROM employee WHERE RowID='" & publicEmpRowID & "';")
                    If n_fdsfd.Result = Nothing Then
                    Else
                        Dim date_value_string = Split(CStr(n_fdsfd.Result), ",")
                        If date_value_string(0).Length > 0 Then
                            MaskedTextBox1.Text = CDate(date_value_string(0)).ToShortDateString
                        End If
                        If date_value_string(1).Length > 0 Then
                            MaskedTextBox2.Text = CDate(date_value_string(1)).ToShortDateString
                        End If
                    End If
                    AddHandler cboEmpStat.TextChanged, AddressOf cboEmpStat_TextChanged
                ElseIf selectedTab Is tbpSalary Then 'Salary
                    'filingid, mStat, noofdepd

                    filingid = .Cells("fstatRowID").Value
                    mStat = .Cells("Column31").Value
                    noofdepd = .Cells("Column32").Value
                    'dgvemployeesalary.Focus()
                    If dgvemployeesalary.RowCount <> 0 Then
                        dgvemployeesalary.Item("c_fromdate", 0).Selected = True
                    End If

                    txtFNameSal.Text = employeefullname
                    txtEmpIDSal.Text = subdetails '"ID# " & .Cells("Column1").Value
                    txtEmpIDSal.Tag = publicEmpRowID

                    pbEmpPicSal.Image = Nothing
                    txtpaytype.Text = .Cells("Column22").Value
                    txtEmp_type.Text = .Cells("Column34").Value
                    pbEmpPicSal.Image = EmployeeImage

                ElseIf selectedTab Is tbpAwards Then
                    txtEmpIDAwar.Text = subdetails '"ID# " & .Cells("Column1").Value

                    txtFNameAwar.Text = employeefullname
                    pbEmpPicAwar.Image = Nothing
                    pbEmpPicAwar.Image = EmployeeImage
                    listofEditRowAward.Clear()
                    VIEW_employeeawards(.Cells("RowID").Value)
                ElseIf selectedTab Is tbpCertifications Then
                    txtFNameCert.Text = employeefullname
                    txtEmpIDCert.Text = subdetails '"ID# " & .Cells("Column1").Value

                    pbEmpPicCert.Image = Nothing
                    pbEmpPicCert.Image = EmployeeImage
                    listofEditRowCert.Clear()
                    VIEW_employeecertification(.Cells("RowID").Value)
                ElseIf selectedTab Is tbpLeave Then
                    txtFNameLeave.Text = employeefullname
                    txtEmpIDLeave.Text = subdetails '"ID# " & .Cells("Column1").Value

                    If .Cells("Column19").Value = "Male" Then
                        rdMale.Checked = True
                    Else
                        rdFMale.Checked = True

                    End If

                    txtvlallowLeave.Text = .Cells("Column36").Value
                    txtslallowLeave.Text = .Cells("slallowance").Value
                    txtmlallowleave.Text = .Cells("mlallowance").Value

                    txtvlbalLeave.Text = .Cells("Column35").Value
                    txtslbalLeave.Text = .Cells("slbalance").Value
                    txtmlbalLeave.Text = .Cells("mlbalance").Value

                    txtvlpaypLeave.Text = .Cells("Column33").Value
                    txtslpaypLeave.Text = .Cells("slpayp").Value
                    txtmlpaypLeave.Text = .Cells("mlpayp").Value

                    pbEmpPicLeave.Image = Nothing
                    pbEmpPicLeave.Image = EmployeeImage
                    listofEditRowleave.Clear()
                    VIEW_employeeleave(.Cells("RowID").Value)
                    dgvempleave_SelectionChanged(sender, e)
                ElseIf selectedTab Is tbpDiscipAct Then
                    controlclear()
                    controlfalseDiscipAct()
                    fillempdisciplinary()
                    txtFNameDiscip.Text = employeefullname
                    txtEmpIDDiscip.Text = subdetails '"ID# " & .Cells("Column1").Value

                    pbEmpPicDiscip.Image = Nothing
                    pbEmpPicDiscip.Image = EmployeeImage
                ElseIf selectedTab Is tbpEducBG Then
                    fillselectRowID()
                    fillselecteducback()
                    txtFNameEduc.Text = employeefullname
                    txtEmpIDEduc.Text = subdetails '"ID# " & .Cells("Column1").Value

                    pbEmpPicEduc.Image = Nothing
                    pbEmpPicEduc.Image = EmployeeImage
                ElseIf selectedTab Is tbpPrevEmp Then
                    txtFNamePrevEmp.Text = employeefullname
                    txtEmpIDPrevEmp.Text = subdetails
                    pbEmpPicPrevEmp.Image = Nothing
                    pbEmpPicPrevEmp.Image = EmployeeImage
                    cleartextboxPrevEmp()
                    fillemployerlist()
                ElseIf selectedTab Is tbpPromotion Then 'Promotion
                    'controlfalsePromot()
                    fillpromotions()
                    'fillselectedpromotions()
                    txtFNamePromot.Text = employeefullname
                    txtEmpIDPromot.Text = subdetails '"ID# " & .Cells("Column1").Value

                    pbEmpPicPromot.Image = Nothing
                    pbEmpPicPromot.Image = EmployeeImage
                    txtpositfrompromot.Text = ""
                    cmbto.Text = ""
                    txtempcurrbasicpay.Text = "0"
                    txtReasonPromot.Text = ""
                    cmbfrom.Text = txtpositfrompromot.Text
                    cmbfrom_SelectedIndexChanged(sender, e)

                    cmbto.Enabled = 0
                    dtpEffectivityDate.Enabled = 0
                    cmbflg.Enabled = 0

                    Label82.Visible = False
                    lblpeso.Visible = False
                    txtbasicpay.Visible = False
                    RemoveHandler cmbflg.SelectedIndexChanged, AddressOf cmbflg_SelectedIndexChanged
                    Label142.Text = "Current salary"
                ElseIf selectedTab Is tbpLoans Then 'Loan Schedule
                    txtFNameLoan.Text = employeefullname
                    txtEmpIDLoan.Text = subdetails
                    pbEmpPicLoan.Image = Nothing
                    pbEmpPicLoan.Image = EmployeeImage
                    TextBox7.Text = .Cells("Column1").Value
                    dgvLoanList.Rows.Clear()
                    fillloadsched()
                    fillloadschedselected()
                ElseIf selectedTab Is tbpLoanHist Then 'Loan History

                    txtFNameLoanhist.Text = employeefullname
                    txtEmpIDLoanhist.Text = subdetails
                    pbEmpPicLoanhist.Image = Nothing
                    pbEmpPicLoanhist.Image = EmployeeImage
                    VIEW_employeeloanhistory(.Cells("RowID").Value)
                    dgvloanhisto_SelectionChanged(sender, e)

                ElseIf selectedTab Is tbpPayslip Then 'Pay slip history
                    dgvpayper_SelectionChanged(sender, e)
                ElseIf selectedTab Is tbpempallow Then 'Employee Allowance
                    RemoveHandler dgvempallowance.SelectionChanged, AddressOf dgvempallowance_SelectionChanged
                    pbEmpPicAllow.Image = Nothing
                    txtFNameAllow.Text = employeefullname
                    txtEmpIDAllow.Text = subdetails
                    pbEmpPicAllow.Image = EmployeeImage
                    VIEW_employeeallowance(.Cells("RowID").Value)
                    dgvempallowance_SelectionChanged(sender, e)

                    AddHandler dgvempallowance.SelectionChanged, AddressOf dgvempallowance_SelectionChanged
                    dgvempallowance_SelectionChanged1(sender, New EventArgs)
                ElseIf selectedTab Is tbpEmpOT Then 'Employee Overtime
                    txtFNameEmpOT.Text = employeefullname
                    txtEmpIDEmpOT.Text = subdetails '"ID# " & .Cells("Column1").Value

                    pbEmpPicEmpOT.Image = Nothing
                    pbEmpPicEmpOT.Image = EmployeeImage
                    listofEditRowEmpOT.Clear()
                    VIEW_employeeOT(.Cells("RowID").Value)
                    dgvEmpOT_SelectionChanged(sender, e)
                ElseIf selectedTab Is tbpOBF Then 'Official Business filing
                    RemoveHandler cboOBFstatus.SelectedIndexChanged, AddressOf cboOBFstatus_SelectedIndexChanged
                    txtFNameOBF.Text = employeefullname
                    txtEmpIDOBF.Text = subdetails '"ID# " & .Cells("Column1").Value
                    pbEmpPicOBF.Image = Nothing
                    pbEmpPicOBF.Image = EmployeeImage
                    listofEditRowOBF.Clear()
                    dgvOBF.Rows.Clear()
                    VIEW_employeeoffbusi(.Cells("RowID").Value)
                    dgvOBF_SelectionChanged(sender, e)
                    AddHandler cboOBFstatus.SelectedIndexChanged, AddressOf cboOBFstatus_SelectedIndexChanged
                ElseIf selectedTab Is tbpBonus Then 'Bonus
                    txtFNameBon.Text = employeefullname
                    txtEmpIDBon.Text = subdetails '"ID# " & .Cells("Column1").Value
                    pbEmpPicBon.Image = Nothing
                    pbEmpPicBon.Image = EmployeeImage
                    listofEditRowBon.Clear()
                    VIEW_employeebonus(.Cells("RowID").Value)
                    dgvempbon_SelectionChanged(sender, e)
                ElseIf selectedTab Is tbpAttachment Then 'Attachment
                    txtFNameAtta.Text = employeefullname
                    txtEmpIDAtta.Text = subdetails '"ID# " & .Cells("Column1").Value
                    pbEmpPicAtta.Image = Nothing
                    pbEmpPicAtta.Image = EmployeeImage
                    listofEditRoweatt.Clear()
                    VIEW_employeeattachments(.Cells("RowID").Value)
                    dgvempatta_SelectionChanged(sender, e)
                ElseIf selectedTab Is tbpNewSalary Then
                    SalaryTab.SetEmployee(employee)
                End If

                MDIPrimaryForm.lblCreatedBy.Text = .Cells("Column26").Value
                MDIPrimaryForm.lblCreatedDate.Text = .Cells("Column25").Value
                MDIPrimaryForm.lblUpdatedBy.Text = .Cells("Column28").Value
                MDIPrimaryForm.lblUpdatedDate.Text = .Cells("Column27").Value

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
                Case 0 'Employee

                    For Each panel_ctrl As Control In panelchklist.Controls
                        If TypeOf panel_ctrl Is LinkLabel Then
                            DirectCast(panel_ctrl, LinkLabel).ImageIndex = 0
                        End If
                    Next
                    lblyourrequirement.Text = ""
                Case 1 'Employee
                    dgvDepen.Rows.Clear()

                    clearObjControl(SplitContainer2.Panel1)
                    clearObjControl(tbpleaveallow)
                    clearObjControl(tbpleavebal)
                    clearObjControl(tbpleavepayp)
                    chkutflag.Checked = 0
                    chkotflag.Checked = 0
                    listofEditDepen.Clear()
                Case 2 'Salary
                    filingid = Nothing
                    mStat = Nothing
                    noofdepd = Nothing
                    txtEmpIDSal.Text = ""
                    txtEmpIDSal.Tag = Nothing
                    txtFNameSal.Text = ""
                    txtEmp_type.Text = ""
                    pbEmpPicSal.Image = Nothing

                Case 3 'Awards
                    txtEmpIDAwar.Text = ""
                    txtFNameAwar.Text = ""
                    pbEmpPicAwar.Image = Nothing
                    listofEditRowAward.Clear()
                    dgvempawar.Rows.Clear()
                Case 4 'Certifications
                    txtEmpIDCert.Text = ""
                    txtFNameCert.Text = ""
                    pbEmpPicCert.Image = Nothing
                    listofEditRowCert.Clear()
                    dgvempcert.Rows.Clear()
                Case 5 'Leave
                    txtEmpIDLeave.Text = ""
                    txtFNameLeave.Text = ""
                    pbEmpPicLeave.Image = Nothing
                    listofEditRowleave.Clear()
                    cboleavetypes.SelectedIndex = -1
                    txtstarttime.Text = ""
                    txtendtime.Text = ""
                    txtstartdate.Text = ""
                    txtendate.Text = ""
                    txtreason.Text = ""
                    txtcomments.Text = ""
                    txtvlallowLeave.Text = ""
                    txtslallowLeave.Text = ""
                    txtmlallowleave.Text = ""
                    txtvlbalLeave.Text = ""
                    txtslbalLeave.Text = ""
                    txtmlbalLeave.Text = ""
                    txtvlpaypLeave.Text = ""
                    txtslpaypLeave.Text = ""
                    txtmlpaypLeave.Text = ""
                    dgvempleave.Rows.Clear()
                Case 6 'Disciplinary Action
                    controlclear()
                    controlfalseDiscipAct()
                    fillempdisciplinary()
                    txtEmpIDDiscip.Text = ""
                    txtFNameDiscip.Text = ""
                    pbEmpPicDiscip.Image = Nothing

                Case 7 'Educational Background

                    dgvEducback.Rows.Clear()
                    fillselecteducback()
                    txtEmpIDEduc.Text = ""
                    txtFNameEduc.Text = ""
                    pbEmpPicEduc.Image = Nothing
                Case 8 'Previous Employer
                    cleartextboxPrevEmp()
                    dgvListCompany.Rows.Clear()
                Case 9 'Promotion

                    cmbfrom.SelectedIndex = -1
                    txtpositfrompromot.Text = ""
                    cmbto.Text = ""
                    txtEmpIDPromot.Text = ""
                    txtFNamePromot.Text = ""
                    pbEmpPicPromot.Image = Nothing
                    dgvPromotionList.Rows.Clear()
                    txtempcurrbasicpay.Text = 0
                    txtReasonPromot.Text = ""
                    cmbto.Enabled = 0
                    dtpEffectivityDate.Enabled = 0
                    cmbflg.Enabled = 0
                    Label82.Visible = False
                    lblpeso.Visible = False
                    txtbasicpay.Visible = False

                    RemoveHandler cmbflg.SelectedIndexChanged, AddressOf cmbflg_SelectedIndexChanged
                Case 10 'Loan Schedule
                    TextBox7.Text = ""
                    dgvLoanList.Rows.Clear()
                Case 11 'Loan History
                    txtFNameLoanhist.Text = ""
                    txtEmpIDLoanhist.Text = ""
                    pbEmpPicLoanhist.Image = Nothing
                    dateded.Value = Format(CDate(dbnow), machineShortDateFormat)
                    ComboBox2.SelectedIndex = -1
                    ComboBox2.Text = ""
                    TextBox11.Text = ""
                    txtamount.Text = ""
                Case 12 'Pay slip history
                    paypFrom = Nothing
                    paypTo = Nothing
                    paypRowID = Nothing
                    txtempbasicpay.Text = "0.00"
                    txttotreghrs.Text = "0.00"
                    txttotregamt.Text = "0.00"
                    txttotothrs.Text = "0.00"
                    txttototamt.Text = "0.00"
                    txttotnightdiffhrs.Text = "0.00"
                    txttotnightdiffamt.Text = "0.00"
                    txttotnightdiffothrs.Text = "0.00"
                    txttotnightdiffotamt.Text = "0.00"
                    txttotholidayhrs.Text = "0.00"
                    txttotholidayamt.Text = "0.00"
                    txthrswork.Text = "0.00"
                    txthrsworkamt.Text = "0.00"
                    lblsubtot.Text = "0.00"
                    txtemptotallow.Text = "0.00"
                    txtemptotbon.Text = "0.00"
                    txtgrosssal.Text = "0.00"
                    txttotabsent.Text = "0.00"
                    txttotabsentamt.Text = "0.00"
                    txttottardi.Text = "0.00"
                    txttottardiamt.Text = "0.00"
                    txttotut.Text = "0.00"
                    txttotutamt.Text = "0.00"
                    lblsubtotmisc.Text = "0.00"
                    txtempsss.Text = "0.00"
                    txtempphh.Text = "0.00"
                    txtemphdmf.Text = "0.00"
                    txttaxabsal.Text = "0.00"
                    txtempwtax.Text = "0.00"
                    txtemptotloan.Text = "0.00"
                    txtnetsal.Text = "0.00"
                    vlbal.Text = "0"
                    slbal.Text = "0"
                    mlbal.Text = "0"
                    TextBox8.Text = "0"
                    TextBox9.Text = "0"
                    TextBox5.Text = "0"
                    TextBox13.Text = "0"
                    TextBox14.Text = "0"
                    TextBox10.Text = "0"
                Case 13 'Employee Allowance
                    dgvempallowance.Tag = Nothing
                    txtFNameAllow.Text = ""
                    txtEmpIDAllow.Text = ""
                    pbEmpPicAllow.Image = Nothing
                    dgvempallowance.Rows.Clear()
                    listofEditEmpAllow.Clear()
                Case 14 'Employee Overtime
                    txtFNameEmpOT.Text = ""
                    txtEmpIDEmpOT.Text = "" '"ID# " & .Cells("Column1").Value

                    pbEmpPicEmpOT.Image = Nothing
                    listofEditRowEmpOT.Clear()
                    dgvempOT.Rows.Clear()
                Case 15 'Official Business filing
                    dgvOBF.Rows.Clear()
                    txtFNameOBF.Text = ""
                    txtEmpIDOBF.Text = ""
                    pbEmpPicOBF.Image = Nothing
                    listofEditRowOBF.Clear()
                Case 16 'Bonus
                    txtFNameBon.Text = ""
                    txtEmpIDBon.Text = ""
                    pbEmpPicBon.Image = Nothing
                    listofEditRowBon.Clear()

                    dgvempbon.Rows.Clear()
                Case 17 'Attachment
                    dgvempatta.Rows.Clear()
                    cboattatype.SelectedIndex = -1
                    txtFNameAtta.Text = ""
                    txtEmpIDAtta.Text = ""
                    pbEmpPicAtta.Image = Nothing
                    listofEditRoweatt.Clear()
            End Select
            MDIPrimaryForm.lblCreatedBy.Text = Nothing
            MDIPrimaryForm.lblCreatedDate.Text = Nothing
            MDIPrimaryForm.lblUpdatedBy.Text = Nothing
            MDIPrimaryForm.lblUpdatedDate.Text = Nothing
        End If
        listofEditDepen.Clear()
    End Sub

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
            txtvlallow.Text = If(IsDBNull(drow("vl_allowance")), 0.0, drow("vl_allowance"))
            txtslallow.Text = If(IsDBNull(drow("sl_allowance")), 0.0, drow("sl_allowance"))
            txtmlallow.Text = If(IsDBNull(drow("ml_allowance")), 0.0, drow("ml_allowance"))

            txtvlpayp.Text = If(IsDBNull(drow("vl_payp")), 0.0, drow("vl_payp"))
            txtslpayp.Text = If(IsDBNull(drow("sl_payp")), 0.0, drow("sl_payp"))
            txtmlpayp.Text = If(IsDBNull(drow("ml_payp")), 0.0, drow("ml_payp"))
        Next
        dtpempstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
        dtpempbdate.Value = Format(CDate(dbnow), machineShortDateFormat)
        chkbxRevealInPayroll.Checked = False
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

    Function strTrimProper(ByVal exprssn As String) As String
        Return StrConv(Trim(exprssn), VbStrConv.ProperCase)
    End Function

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked

        HRISForm.PositionToolStripMenuItem_Click(HRISForm.PositionToolStripMenuItem, New EventArgs)
    End Sub

    Private Sub cboEmpStat_TextChanged(sender As Object, e As EventArgs) 'Handles cboEmpStat.TextChanged
        If publicEmpRowID = Nothing Then
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        newPostion.Close()
        newEmpType.Close()
        newEmpStat.Show() : newEmpStat.BringToFront()
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

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        'Regular Exempt, Regular Non Exempt, Contractor
        newPostion.Close() : newEmpStat.Close()
        newEmpType.Show() : newEmpType.BringToFront()
    End Sub

    Dim curr_empColm As String
    Dim curr_empRow As Integer

    Sub SearchEmoloyee_Click(sender As Object, e As EventArgs) Handles Button4.Click
        dependentitemcount = -1

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
        RemoveHandler dgvDepen.SelectionChanged, AddressOf dgvDepen_SelectionChanged

        If dgvEmp.RowCount <> 0 Then
            curr_empRow = dgvEmp.CurrentRow.Index
            curr_empColm = dgvEmp.Columns(dgvEmp.CurrentCell.ColumnIndex).Name
            dgvEmp.Item(curr_empColm, curr_empRow).Selected = True
        End If

        If TabControl2.SelectedIndex = 0 Then
            Dim q_search = searchCommon(ComboBox7, TextBox1,
                                        ComboBox8, TextBox15,
                                        ComboBox9, TextBox16,
                                        ComboBox10, TextBox17)

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

            For Each drow As DataRow In dtemployee.Rows
                Dim rowArray = drow.ItemArray()
                dgvEmp.Rows.Add(rowArray)
            Next
            dtemployee.Dispose()
        Else
            If isKPressSimple = 1 Then
                If txtSimple.Text.Trim.Length = 0 Then
                    dgvRowAdder(q_employee & " ORDER BY e.RowID DESC LIMIT " & pagination & ",100;", dgvEmp)
                Else
                    searchEmpSimple()
                End If
            Else
                If txtSimple.Text.Trim.Length = 0 Then
                    dgvRowAdder(q_employee & " ORDER BY e.RowID DESC LIMIT " & pagination & ",100;", dgvEmp)
                Else
                    searchEmpSimple()
                End If
            End If
        End If

        If dgvEmp.RowCount <> 0 Then
            If curr_empRow <= dgvEmp.RowCount - 1 Then
                dgvEmp.Item(curr_empColm, curr_empRow).Selected = True
            Else
                dgvEmp.Item(curr_empColm, 0).Selected = True
            End If
            dgvEmp_SelectionChanged(sender, e)
        End If

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
        AddHandler dgvDepen.SelectionChanged, AddressOf dgvDepen_SelectionChanged
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

        If r_Editing = Nothing And c_Editing = Nothing Then
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
                Else
                    dgvDepen.Item(c_Editing, r_Editing).Value = strTrimProper(dgvDepen.Item(c_Editing, r_Editing).Value)
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
            SearchEmoloyee_Click(sender, e)
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
            SearchEmoloyee_Click(sender, e)
        End If
    End Sub

    Function searchCommon(Optional cbox1 As ComboBox = Nothing, Optional search1 As Object = Nothing,
                          Optional cbox2 As ComboBox = Nothing, Optional search2 As Object = Nothing,
                          Optional cbox3 As ComboBox = Nothing, Optional search3 As Object = Nothing,
                          Optional cbox4 As ComboBox = Nothing, Optional search4 As Object = Nothing,
                          Optional cbox5 As ComboBox = Nothing, Optional search5 As Object = Nothing) As String

        Dim _search1, _search2, _search3, _search4, _search5 As String ', ordate, credate

        Select Case cbox1.SelectedIndex
            Case 0
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID LIKE '" & search1.Text & "%'")
            Case 1
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID LIKE '%" & search1.Text & "%'")
            Case 2
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID = '" & search1.Text & "'")
            Case 3
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID NOT LIKE '%" & search1.Text & "%'")
            Case 4
                _search1 = " e.EmployeeID IS NULL"
            Case 5
                _search1 = " e.EmployeeID IS NOT NULL"
            Case Else
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID = '" & search1.Text & "'")
        End Select

        Select Case cbox2.SelectedIndex
            Case 0
                _search2 = If(search2.Text = "", Nothing, " e.FirstName LIKE '" & search2.Text & "%'")
            Case 1
                _search2 = If(search2.Text = "", Nothing, " e.FirstName LIKE '%" & search2.Text & "%'")
            Case 2
                _search2 = If(search2.Text = "", Nothing, " e.FirstName = '" & search2.Text & "'")
            Case 3
                _search2 = If(search2.Text = "", Nothing, " e.FirstName NOT LIKE '%" & search2.Text & "%'")
            Case 4
                _search2 = " e.FirstName IS NULL"
            Case 5
                _search2 = " e.FirstName IS NOT NULL"
            Case Else
                _search2 = If(search2.Text = "", Nothing, " e.FirstName = '" & search2.Text & "'")
        End Select

        If _search1 <> "" And _search2 <> "" Then
            _search2 = " AND" & _search2
        End If

        Select Case cbox3.SelectedIndex
            Case 0
                _search3 = If(search3.Text = "", Nothing, " e.LastName LIKE '" & search3.Text & "%'")
            Case 1
                _search3 = If(search3.Text = "", Nothing, " e.LastName LIKE '%" & search3.Text & "%'")
            Case 2
                _search3 = If(search3.Text = "", Nothing, " e.LastName = '" & search3.Text & "'")
            Case 3
                _search3 = If(search3.Text = "", Nothing, " e.LastName NOT LIKE '%" & search3.Text & "%'")
            Case 4
                _search3 = " e.LastName IS NULL"
            Case 5
                _search3 = " e.LastName IS NOT NULL"
            Case Else
                _search3 = If(search3.Text = "", Nothing, " e.LastName = '" & search3.Text & "'")
        End Select

        If (_search1 <> "" Or _search2 <> "") And _search3 <> "" Then
            _search3 = " AND" & _search3
        End If

        If cbox4 Is Nothing Then
            _search4 = Nothing
        Else
            Select Case cbox4.SelectedIndex
                Case 0
                    _search4 = If(search4.Text = "", Nothing, " e.Surname LIKE '" & search4.Text & "%'")
                Case 1
                    _search4 = If(search4.Text = "", Nothing, " e.Surname LIKE '%" & search4.Text & "%'")
                Case 2
                    _search4 = If(search4.Text = "", Nothing, " e.Surname = '" & search4.Text & "'")
                Case 3
                    _search4 = If(search4.Text = "", Nothing, " e.Surname NOT LIKE '%" & search4.Text & "%'")
                Case 4
                    _search4 = " e.Surname IS NULL"
                Case 5
                    _search4 = " e.Surname IS NOT NULL"
                Case Else
                    _search4 = If(search4.Text = "", Nothing, " e.Surname = '" & search4.Text & "'")
            End Select

            If (_search1 <> "" Or _search2 <> "" Or _search3 <> "") And _search4 <> "" Then
                _search4 = " AND" & _search4
            End If
        End If

        If cbox5 Is Nothing Then
            _search5 = Nothing
        Else
            Select Case cbox5.SelectedIndex
                Case 0
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName LIKE '" & search5.Text & "%'")
                Case 1
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName LIKE '%" & search5.Text & "%'")
                Case 2
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName = '" & search5.Text & "'")
                Case 3
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName NOT LIKE '%" & search5.Text & "%'")
                Case 4
                    _search5 = " e.MiddleName IS NULL"
                Case 5
                    _search5 = " e.MiddleName IS NOT NULL"
                Case Else
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName = '" & search5.Text & "'")
            End Select

            If (_search1 <> "" Or _search2 <> "" Or _search3 <> "" Or _search4 <> "") And _search5 <> "" Then
                _search5 = " AND" & _search5
            End If
        End If

        Return _search1 & _search2 & _search3 & _search4 & _search5
    End Function

    Dim colName As String

    Sub searchEmpSimple() ' As String
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
                dgvRowAdder(q_employee & " AND " & colName & Format(CDate(txtSimple.Text), "yyyy-MM-dd") & "' ORDER BY e.RowID DESC", dgvEmp)

            Case 9 : colName = "e.Startdate='"
                s = 1

                dgvRowAdder(q_employee & " AND " & colName & Format(CDate(txtSimple.Text), "yyyy-MM-dd") & "' ORDER BY e.RowID DESC", dgvEmp)

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
                dgvRowAdder(q_employee & " AND " & colName & txtSimple.Text & "',1) ORDER BY e.RowID DESC", dgvEmp)

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
            dgvRowAdder(q_employee & " AND " & colName & txtSimple.Text & "' ORDER BY e.RowID DESC", dgvEmp)
        End If
    End Sub

    Dim isKPressSimple As SByte
    Dim colSearchSimple As String

    Private Sub txtSimple_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSimple.KeyDown ',
        'ComboBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            isKPressSimple = 1
            SearchEmoloyee_Click(sender, e)
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
                Else
                    dgvDepen.Item(c_Editing, r_Editing).Value = strTrimProper(dgvDepen.Item(c_Editing, r_Editing).Value)
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

            If r.Cells("Colmn0").Value = Nothing And dgvEmp.RowCount <> 0 Then

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
            newEmpType.Close()
            newEmpStat.Close()
            newPostion.Close()

            'EXECQUER("")

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
                        drow("Image") = If(empPic = Nothing,
                                           Nothing,
                                           convertFileToByte(empPic))

                        If empPic = Nothing Then
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

            SearchEmoloyee_Click(sender, e)

        ElseIf Trim(txtSimple.Text) <> "" And
        TabControl2.SelectedIndex = 1 Then

            SearchEmoloyee_Click(sender, e)
        Else

            loademployee()

        End If

        dgvEmp_SelectionChanged(sender, e)

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabctrlemp.SelectedIndexChanged
        Label25.Text = Trim(tabctrlemp.SelectedTab.Text)

        If tabctrlemp.SelectedTab Is tbpNewSalary Then
            dgvEmp_SelectionChanged(sender, e)
        End If
    End Sub

    Dim emp_ralation As New AutoCompleteStringCollection

    Sub tbpEmployee_Enter(sender As Object, e As EventArgs) Handles tbpEmployee.Enter

        tabpageText(tabIndx)

        tbpEmployee.Text = "PERSONAL PROFILE               "

        Label25.Text = "PERSONAL PROFILE"
        Static once As SByte = 0
        If once = 0 Then
            once = 1

            cboPosit.ContextMenu = New ContextMenu

            txtUTgrace.ContextMenu = New ContextMenu

            txtWorkDaysPerYear.ContextMenu = New ContextMenu

            loadPositName()

            enlistToCboBox(q_salut, cboSalut)
            salutn_count = cboSalut.Items.Count

            enlistToCboBox(q_empstat, cboEmpStat) '"SELECT DISTINCT(COALESCE(DisplayValue,'')) FROM listofval WHERE Type='Status' AND Active='Yes'"

            enlistToCboBox(q_emptype, cboEmpType)

            enlistToCboBox(q_maritstat, cboMaritStat)

            enlistToCboBox("SELECT DisplayValue FROM listofval WHERE `Type`='Bank Names';", cbobank)

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

            If dbnow = Nothing Then
                dbnow = EXECQUER(CURDATE_MDY)
            End If

            dtpempstartdate.Value = dbnow 'Format(CDate(dbnow), machineShortDateFormat)

            view_ID = VIEW_privilege("Employee Personal Profile", orgztnID)

            For Each strval In cboSalut.Items
                Colmn2.Items.Add(strval)
            Next

            enlistTheLists("SELECT DisplayValue FROM listofval WHERE Type='Employee Relationship' ORDER BY OrderBy;",
                           emp_ralation)

            Dim dt_payfreq As New DataTable

            dt_payfreq = retAsDatTbl("SELECT RowID,PayFrequencyType FROM payfrequency WHERE RowID IN (1,4) ORDER BY RowID DESC;")

            cboPayFreq.ValueMember = "RowID"
            cboPayFreq.DisplayMember = "PayFrequencyType"

            cboPayFreq.DataSource = dt_payfreq

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
                (Panel1.AccessibleDescription = sys_ownr.CurrentSystemOwner)

        End If

        tabIndx = 1 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub tbpEmployee_Leave(sender As Object, e As EventArgs) 'Handles tbpEmployee.Leave
        tbpEmployee.Text = "PERSON"
    End Sub

    Private Sub tbpleavepayp_Enter(sender As Object, e As EventArgs) Handles tbpleavepayp.Enter

        Static once As SByte = 0

        If once = 0 Then
            once = 1

            OjbAssignNoContextMenu(txtvlpayp)

            OjbAssignNoContextMenu(txtslpayp)

            OjbAssignNoContextMenu(txtmlpayp)

            OjbAssignNoContextMenu(txtothrpayp)

        End If

    End Sub

    Private Sub txtvlpayp_Leave(sender As Object, e As EventArgs) Handles txtvlpayp.Leave

        Dim count_payp = payp_count()

        Dim calc_result = Val(txtvlpayp.Text) * count_payp

        txtvlallow.Text = ValNoComma(calc_result)

    End Sub

    Private Sub txtslpayp_Leave(sender As Object, e As EventArgs) Handles txtslpayp.Leave

        Dim count_payp = payp_count()

        Dim calc_result = Val(txtslpayp.Text) * count_payp

        txtslallow.Text = ValNoComma(calc_result)

    End Sub

    Private Sub txtmlpayp_Leave(sender As Object, e As EventArgs) Handles txtmlpayp.Leave

        Dim count_payp = payp_count()

        Dim calc_result = Val(txtmlpayp.Text) * count_payp

        txtmlallow.Text = ValNoComma(calc_result)

    End Sub

    Private Sub txtothrpayp_Leave(sender As Object, e As EventArgs) Handles txtothrpayp.Leave

        Dim count_payp = payp_count()

        Dim calc_result = Val(txtothrpayp.Text) * count_payp

        txtothrallow.Text = ValNoComma(calc_result)

    End Sub

    Private Sub txtvlallow_Leave(sender As Object, e As EventArgs) Handles txtvlallow.Leave

        Dim count_payp = payp_count()

        Dim calc_result = Val(txtvlallow.Text) / count_payp

        txtvlpayp.Text = ValNoComma(calc_result)

    End Sub

    Private Sub txtvlallow_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtvlallow.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtvlallow.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtvlallow.Text.Contains(".") Then
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

    Private Sub txtslallow_Leave(sender As Object, e As EventArgs) Handles txtslallow.Leave

        Dim count_payp = payp_count()

        Dim calc_result = Val(txtslallow.Text) / count_payp

        txtslpayp.Text = ValNoComma(calc_result)

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

    Private Sub txtmlallow_Leave(sender As Object, e As EventArgs) Handles txtmlallow.Leave

        Dim count_payp = payp_count()

        Dim calc_result = Val(txtmlallow.Text) / count_payp

        txtmlpayp.Text = ValNoComma(calc_result)

    End Sub

    Private Sub txtothrallow_Leave(sender As Object, e As EventArgs) Handles txtothrallow.Leave

        Dim count_payp = payp_count()

        Dim calc_result = Val(txtothrallow.Text) / count_payp

        txtothrpayp.Text = ValNoComma(calc_result)

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

    Private Sub txtvlpayp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtvlpayp.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtvlpayp.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtvlpayp.Text.Contains(".") Then
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

    Private Sub txtslpayp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtslpayp.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtslpayp.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtslpayp.Text.Contains(".") Then
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

    Private Sub txtmlpayp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtmlpayp.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtmlpayp.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtmlpayp.Text.Contains(".") Then
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

    'Sub tbpNewSalary_Enter(sender As Object, e As EventArgs) Handles tbpNewSalary.Enter
    '    dgvEmp_SelectionChanged(sender, e)
    'End Sub

#Region "Awards"

    Dim view_IDAwar As Integer

    Sub tbpAwards_Enter(sender As Object, e As EventArgs) Handles tbpAwards.Enter
        tabpageText(tabIndx)

        tbpAwards.Text = "AWARDS              "

        Label25.Text = "AWARDS"
        Static once As SByte = 0
        If once = 0 Then
            once = 1
            view_IDAwar = VIEW_privilege("Employee Award", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDAwar)

            If formuserprivilege.Count = 0 Then

                tsbtnNewempawar.Visible = 0
                tsbtnSaveempawar.Visible = 0

                dontUpdateAwar = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        'ToolStripButton2.Visible = 0
                        tsbtnNewempawar.Visible = 0
                        tsbtnSaveempawar.Visible = 0

                        dontUpdateAwar = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            tsbtnNewempawar.Visible = 0
                        Else
                            tsbtnNewempawar.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateAwar = 1
                        Else
                            dontUpdateAwar = 0
                        End If

                    End If
                Next
            End If
        End If

        tabIndx = 3 'TabControl1.SelectedIndex
        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub tbpAwards_Leave(sender As Object, e As EventArgs) 'Handles tbpAwards.Leave
        tbpAwards.Text = "AWARD"
    End Sub

    Sub VIEW_employeeawards(ByVal EmployeeID As Object)

        Dim param(1, 2) As Object

        param(0, 0) = "eawar_EmployeeID"
        param(1, 0) = "eawar_OrganizationID"

        param(0, 1) = EmployeeID
        param(1, 1) = orgztnID

        EXEC_VIEW_PROCEDURE(param,
                           "VIEW_employeeawards",
                           dgvempawar)

    End Sub

    Sub tsbtnNewempawar_Click(sender As Object, e As EventArgs) Handles tsbtnNewempawar.Click
        For Each r As DataGridViewRow In dgvempawar.Rows
            If r.IsNewRow Then
                r.Cells("eawar_Type").Selected = True

            End If
        Next
        dgvempawar.Focus()
    End Sub

    Dim dontUpdateAwar As SByte = 0

    Sub SaveEmployeeAward(sender As Object, e As EventArgs) Handles tsbtnSaveempawar.Click

        dgvempawar.EndEdit(True)

        If dontUpdateAwar = 1 Then
            listofEditRowAward.Clear()
        End If

        If dgvEmp.RowCount = 0 Then
            Exit Sub
        End If

        Dim dbnow As Object = EXECQUER("SELECT DATE_FORMAT(NOW(),'%Y-%m-%d %T');")

        Dim param(9, 2) As Object

        param(0, 0) = "eawa_RowID"
        param(1, 0) = "eawa_OrganizationID"
        param(2, 0) = "eawa_Created"
        param(3, 0) = "eawa_CreatedBy"
        param(4, 0) = "eawa_LastUpd"
        param(5, 0) = "eawa_LastUpdBy"
        param(6, 0) = "eawa_EmployeeID"
        param(7, 0) = "eawa_AwardType"
        param(8, 0) = "eawa_AwardDescription"
        param(9, 0) = "eawa_AwardDate"

        For Each r As DataGridViewRow In dgvempawar.Rows
            If Val(r.Cells("eawar_RowID").Value) = 0 And
                tsbtnNewempawar.Visible = True Then

                If r.IsNewRow = False Then
                    param(0, 1) = DBNull.Value
                    param(1, 1) = orgztnID
                    param(2, 1) = dbnow
                    param(3, 1) = z_User  'CreatedBy
                    param(4, 1) = dbnow 'Created
                    param(5, 1) = z_User 'LastUpdBy
                    param(6, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                    param(7, 1) = If(r.Cells("eawar_Type").Value = Nothing, DBNull.Value, r.Cells("eawar_Type").Value)
                    param(8, 1) = If(r.Cells("eawar_Description").Value = Nothing, DBNull.Value, r.Cells("eawar_Description").Value)
                    param(9, 1) = If(r.Cells("eawar_DateAwarded").Value = Nothing, DBNull.Value, r.Cells("eawar_DateAwarded").Value)

                    r.Cells("eawar_RowID").Value = EXEC_INSUPD_PROCEDURE(param, "INSUPD_employeeawards", "eawa_int")
                End If
            Else
                'eawar_RowID@RowID&True
                'eawar_EmployeeID@EmployeeID&False
                'eawar_Type@Award Type&True
                'eawar_Description@Award Description&True
                'eawar_DateAwarded@Date awarded&True
                'DataGridViewTextBoxColumn69@Column6&False
                'DataGridViewTextBoxColumn70@Column7&False
                'DataGridViewTextBoxColumn71@Column8&False
                'DataGridViewTextBoxColumn72@Column9&False

                If listofEditRowAward.Contains(r.Cells("eawar_RowID").Value) Then
                    param(0, 1) = r.Cells("eawar_RowID").Value
                    param(1, 1) = orgztnID
                    param(2, 1) = dbnow
                    param(3, 1) = z_User 'CreatedBy
                    param(4, 1) = dbnow 'Created
                    param(5, 1) = z_User 'LastUpdBy
                    param(6, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                    param(7, 1) = If(r.Cells("eawar_Type").Value = Nothing, DBNull.Value, r.Cells("eawar_Type").Value)
                    param(8, 1) = If(r.Cells("eawar_Description").Value = Nothing, DBNull.Value, r.Cells("eawar_Description").Value)
                    param(9, 1) = If(r.Cells("eawar_DateAwarded").Value = Nothing, DBNull.Value, r.Cells("eawar_DateAwarded").Value)

                    EXEC_INSUPD_PROCEDURE(param, "INSUPD_employeeawards", "eawa_int")
                End If
            End If
        Next

        listofEditRowAward.Clear()
        '                                           'dgvEmp                   'Employee ID
        InfoBalloon("Changes made in Employee ID '" & dgvEmp.CurrentRow.Cells("Column1").Value & "' has successfully saved.", "Changes successfully save", lblforballoon, 0, -69)

    End Sub

    Dim prevsvalueaward As Object
    Dim prevsRowaward As Integer
    Dim prevsColaward As Integer

    Private Sub dgvempawar_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvempawar.CellBeginEdit

        If dgvempawar.RowCount <> 0 Then

            prevsvalueaward = dgvempawar.Item(e.ColumnIndex, e.RowIndex).Value

        End If

    End Sub

    Public listofEditRowAward As New AutoCompleteStringCollection

    Private Sub dgvempawar_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempawar.CellEndEdit

        prevsRowaward = e.RowIndex
        prevsColaward = e.ColumnIndex

        If dgvempawar.RowCount <> 0 Then

            If dgvempawar.Item("eawar_RowID", prevsRowaward).Value <> Nothing Then

                If dgvempawar.Item(prevsColaward, prevsRowaward).Value <> prevsvalueaward Then
                    listofEditRowAward.Add(dgvempawar.Item("eawar_RowID", prevsRowaward).Value)
                End If
            End If

        End If

        dgvempawar.AutoResizeRow(e.RowIndex)
        dgvempawar.PerformLayout()
    End Sub

    Private Sub tsbtnCancelempawar_Click(sender As Object, e As EventArgs) Handles tsbtnCancelempawar.Click
        listofEditRowAward.Clear()
        dgvEmp_SelectionChanged(sender, e)
    End Sub

#End Region 'Awards

#Region "Certifications"

    Dim view_IDCert As Integer

    Private Sub tbpCertifications_Click(sender As Object, e As EventArgs) Handles tbpCertifications.Click

    End Sub

    Sub tbpCertifications_Enter(sender As Object, e As EventArgs) Handles tbpCertifications.Enter

        tabpageText(tabIndx)

        tbpCertifications.Text = "CERTIFICATIONS               "

        Label25.Text = "CERTIFICATIONS"
        Static once As SByte = 0
        If once = 0 Then
            once = 1
            view_IDCert = VIEW_privilege("Employee Certification", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDCert)

            If formuserprivilege.Count = 0 Then

                tsbtnNewempcert.Visible = 0
                tsbtnSaveempcert.Visible = 0

                dontUpdateCert = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        'ToolStripButton2.Visible = 0
                        tsbtnNewempcert.Visible = 0
                        tsbtnSaveempcert.Visible = 0

                        dontUpdateCert = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            tsbtnNewempcert.Visible = 0
                        Else
                            tsbtnNewempcert.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateCert = 1
                        Else
                            dontUpdateCert = 0
                        End If

                    End If

                Next

            End If

        End If

        tabIndx = 4 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub TabPage5_Leave(sender As Object, e As EventArgs) 'Handles tbpCertifications.Leave
        tbpCertifications.Text = "CERTI"
    End Sub

    Sub VIEW_employeecertification(ByVal EmployeeID As Object)

        Dim param(1, 2) As Object

        param(0, 0) = "ecert_EmployeeID"
        param(1, 0) = "ecert_OrganizationID"

        param(0, 1) = EmployeeID
        param(1, 1) = orgztnID

        EXEC_VIEW_PROCEDURE(param,
                            "VIEW_employeecertification",
                            dgvempcert, , 1)

    End Sub

    Private Sub tsbtnNewempcert_Click(sender As Object, e As EventArgs) Handles tsbtnNewempcert.Click
        For Each r As DataGridViewRow In dgvempcert.Rows
            If r.IsNewRow Then
                r.Cells("ecert_Type").Selected = True
            End If
        Next
        dgvempcert.Focus()
    End Sub

    Dim dontUpdateCert As SByte = 0

    Sub SaveEmployeeCertif(sender As Object, e As EventArgs) Handles tsbtnSaveempcert.Click

        dgvempcert.EndEdit(True)

        If dontUpdateCert = 1 Then
            listofEditRowCert.Clear()
        End If

        If hasDateErrCert = 1 Then
            WarnBalloon("Please input a valid date.", "Invalid Date issued or Date of expiration", lblforballoon, 0, -69)
            Exit Sub
        ElseIf dgvEmp.RowCount = 0 Then
            Exit Sub
        End If

        Dim dbnow As Object = EXECQUER("SELECT DATE_FORMAT(NOW(),'%Y-%m-%d %T');")

        Dim param(12, 2) As Object

        param(0, 0) = "ecer_RowID"
        param(1, 0) = "ecer_OrganizationID"
        param(2, 0) = "ecer_Created"
        param(3, 0) = "ecer_CreatedBy"
        param(4, 0) = "ecer_LastUpd"
        param(5, 0) = "ecer_LastUpdBy"
        param(6, 0) = "ecer_EmployeeID"
        param(7, 0) = "ecer_CertificationType"
        param(8, 0) = "ecer_IssuingAuthority"
        param(9, 0) = "ecer_CertificationNo"
        param(10, 0) = "ecer_IssueDate"
        param(11, 0) = "ecer_ExpirationDate"
        param(12, 0) = "ecer_Comments"

        For Each r As DataGridViewRow In dgvempcert.Rows

            If Val(r.Cells("ecert_RowID").Value) = 0 And
                tsbtnNewempcert.Visible = True Then

                If r.IsNewRow = False Then
                    param(0, 1) = DBNull.Value
                    param(1, 1) = orgztnID
                    param(2, 1) = dbnow
                    param(3, 1) = z_User
                    param(4, 1) = DBNull.Value
                    param(5, 1) = z_User
                    param(6, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                    param(7, 1) = If(r.Cells("ecert_Type").Value = Nothing, DBNull.Value, Trim(r.Cells("ecert_Type").Value))
                    param(8, 1) = If(r.Cells("ecert_IssuingAuth").Value = Nothing, DBNull.Value, Trim(r.Cells("ecert_IssuingAuth").Value))
                    param(9, 1) = If(r.Cells("ecert_CertNum").Value = Nothing, DBNull.Value, Trim(r.Cells("ecert_CertNum").Value))
                    param(10, 1) = If(r.Cells("ecert_DateIssued").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("ecert_DateIssued").Value), "yyyy-MM-dd"))
                    param(11, 1) = If(r.Cells("ecert_Expiration").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("ecert_Expiration").Value), "yyyy-MM-dd"))
                    param(12, 1) = If(r.Cells("ecert_Comments").Value = Nothing, DBNull.Value, Trim(r.Cells("ecert_Comments").Value))

                    r.Cells("ecert_RowID").Value = EXEC_INSUPD_PROCEDURE(param, "INSUPD_employeecertification", "ecer_int")
                End If
            Else

                If listofEditRowCert.Contains(r.Cells("ecert_RowID").Value) Then
                    param(0, 1) = r.Cells("ecert_RowID").Value
                    param(1, 1) = orgztnID
                    param(2, 1) = dbnow
                    param(3, 1) = z_User
                    param(4, 1) = DBNull.Value
                    param(5, 1) = z_User
                    param(6, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                    param(7, 1) = If(r.Cells("ecert_Type").Value = Nothing, DBNull.Value, Trim(r.Cells("ecert_Type").Value))
                    param(8, 1) = If(r.Cells("ecert_IssuingAuth").Value = Nothing, DBNull.Value, Trim(r.Cells("ecert_IssuingAuth").Value))
                    param(9, 1) = If(r.Cells("ecert_CertNum").Value = Nothing, DBNull.Value, Trim(r.Cells("ecert_CertNum").Value))
                    param(10, 1) = If(r.Cells("ecert_DateIssued").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("ecert_DateIssued").Value), "yyyy-MM-dd"))
                    param(11, 1) = If(r.Cells("ecert_Expiration").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("ecert_Expiration").Value), "yyyy-MM-dd"))
                    param(12, 1) = If(r.Cells("ecert_Comments").Value = Nothing, DBNull.Value, Trim(r.Cells("ecert_Comments").Value))

                    EXEC_INSUPD_PROCEDURE(param, "INSUPD_employeecertification", "ecer_int")
                End If
            End If
        Next

        listofEditRowCert.Clear()

        InfoBalloon("Changes made in Employee ID '" & dgvEmp.CurrentRow.Cells("Column1").Value & "' has successfully saved.", "Changes successfully save", lblforballoon, 0, -69)

    End Sub

    Private Sub dgvempcert_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempcert.CellContentClick
        'ecert_DateIssued
    End Sub

    Public listofEditRowCert As New AutoCompleteStringCollection

    Dim hasDateErrCert As SByte = -1

    Dim prevsRowCert As Integer

    Private Sub dgvempcert_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempcert.CellEndEdit
        prevsRowCert = e.RowIndex

        Dim colName As String = dgvempcert.Columns(e.ColumnIndex).Name

        dgvempcert.ShowCellErrors = True

        Static num As Integer = 0

        If dgvempcert.RowCount <> 0 Then
            listofEditRowCert.Add(dgvempcert.Item("ecert_RowID", prevsRowCert).Value)

            If colName = "ecert_DateIssued" _
                And dgvempcert.Item("ecert_DateIssued", prevsRowCert).Value <> Nothing Then 'e.ColumnIndex
                Try
                    dgvempcert.Item("ecert_DateIssued", prevsRowCert).Value = Format(CDate(dgvempcert.Item("ecert_DateIssued", prevsRowCert).Value), machineShortDateFormat)
                    hasDateErrCert = 0
                    dgvempcert.Item("ecert_DateIssued", prevsRowCert).ErrorText = Nothing
                Catch ex As Exception
                    hasDateErrCert = 1
                    dgvempcert.Item("ecert_DateIssued", prevsRowCert).ErrorText = "     Invalid date value"
                    'Return
                End Try
                ' And dgvempcert.Columns("Column6").Index = e.ColumnIndex
            ElseIf colName = "ecert_Expiration" _
                And dgvempcert.Item("ecert_Expiration", prevsRowCert).Value <> Nothing Then 'e.ColumnIndex
                Try
                    dgvempcert.Item("ecert_Expiration", prevsRowCert).Value = Format(CDate(dgvempcert.Item("ecert_Expiration", prevsRowCert).Value), machineShortDateFormat)
                    hasDateErrCert = 0
                    dgvempcert.Item("ecert_Expiration", prevsRowCert).ErrorText = Nothing
                Catch ex As Exception
                    hasDateErrCert = 1
                    dgvempcert.Item("ecert_Expiration", prevsRowCert).ErrorText = "     Invalid date value"
                    'Return
                End Try
            Else
                hasDateErrCert = 0
                dgvempcert.Item(colName, prevsRowCert).ErrorText = Nothing
            End If

        End If

        dgvempcert.AutoResizeRow(e.RowIndex)
        dgvempcert.PerformLayout()
    End Sub

    Private Sub tsbtnCancelempcert_Click(sender As Object, e As EventArgs) Handles tsbtnCancelempcert.Click
        listofEditRowCert.Clear()

        dgvEmp_SelectionChanged(sender, e)
    End Sub

#End Region 'Certifications

#Region "Leave"

    Dim view_IDLeave As Integer

    Public leavetype As New AutoCompleteStringCollection

    Private Sub tbpLeave_Click(sender As Object, e As EventArgs) Handles tbpLeave.Click

    End Sub

    Dim categleavID As String = Nothing

    Dim leave_type As New AutoCompleteStringCollection

    Dim curr_leave_dgvrow, curr_ot_dgvrow, curr_ob_dgvrow As DataGridViewRow

    Sub tbpLeave_Enter(sender As Object, e As EventArgs) Handles tbpLeave.Enter

        tabpageText(tabIndx)

        tbpLeave.Text = "LEAVE               "

        Label25.Text = "LEAVE"

        Static once As SByte = 0

        If once = 0 Then
            once = 1

            enlistToCboBox("SELECT DisplayValue FROM listofval WHERE Type='Employee Leave Status' AND Active='Yes' ORDER BY OrderBy;",
                           cboleavestatus)

            view_IDLeave = VIEW_privilege("Employee Leave", orgztnID)

            '48
            categleavID = EXECQUER("SELECT RowID FROM category WHERE OrganizationID=" & orgztnID & " AND CategoryName='" & "Leave Type" & "' LIMIT 1;")

            If Val(categleavID) = 0 Then
                categleavID = INSUPD_category(, "Leave Type")
            End If

            enlistTheLists("SELECT CONCAT(COALESCE(PartNo,''),'@',RowID) FROM product WHERE CategoryID='" & categleavID & "' AND OrganizationID=" & orgztnID & ";",
                           leave_type) 'cboallowtype

            cboleavetypes.Items.Clear()

            For Each strval In leave_type
                cboleavetypes.Items.Add(getStrBetween(strval, "", "@"))
            Next

            AddHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDLeave)

            If formuserprivilege.Count = 0 Then

                tsbtnNewLeave.Visible = 0
                tsbtnSaveLeave.Visible = 0

                dontUpdateLeave = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        'ToolStripButton2.Visible = 0
                        tsbtnNewLeave.Visible = 0
                        tsbtnSaveLeave.Visible = 0
                        tsbtnDeletLeave.Visible = False

                        dontUpdateLeave = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            tsbtnNewLeave.Visible = 0
                        Else
                            tsbtnNewLeave.Visible = 1
                        End If

                        tsbtnDeletLeave.Visible = (drow("Deleting").ToString = "Y")

                        If drow("Updates").ToString = "N" Then
                            dontUpdateLeave = 1
                        Else
                            dontUpdateLeave = 0
                        End If

                    End If

                Next

            End If

        End If

        tabIndx = 5 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub TabPage6_Leave(sender As Object, e As EventArgs) 'Handles tbpLeave.Leave
        tbpLeave.Text = "LEAVE"
    End Sub

    Sub VIEW_employeeleave(ByVal EmployeeID As Object) '3722

        RemoveHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged

        Dim param(1, 2) As Object

        param(0, 0) = "elv_EmployeeID"
        param(1, 0) = "elv_OrganizationID"

        param(0, 1) = EmployeeID
        param(1, 1) = orgztnID

        EXEC_VIEW_PROCEDURE(param,
                           "VIEW_employeeleave",
                           dgvempleave, 1, 1)

        AddHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged

    End Sub

    Sub tsbtnNewLeave_Click(sender As Object, e As EventArgs) Handles tsbtnNewLeave.Click

        dgvempleave.Focus()

        RemoveHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged

        For Each r As DataGridViewRow In dgvempleave.Rows
            If r.IsNewRow Then
                dgvleaveRowindx = r.Index
                r.Cells("elv_Type").Selected = True
                Exit For
            End If
        Next
        dgvempleave_SelectionChanged(sender, e)

        cboleavetypes.Focus()

        AddHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged

    End Sub

    Dim dontUpdateLeave As SByte = 0

    Sub SaveLeave_Click(sender As Object, e As EventArgs) Handles tsbtnSaveLeave.Click

        pbEmpPicLeave.Focus()

        cboleavetypes.Focus()

        pbEmpPicLeave.Focus()

        dtpstartdate.Focus()

        pbEmpPicLeave.Focus()

        dtpendate.Focus()

        pbEmpPicLeave.Focus()

        txtstarttime.Focus()

        pbEmpPicLeave.Focus()

        txtendtime.Focus()

        pbEmpPicLeave.Focus()

        cboleavestatus.Focus()

        pbEmpPicLeave.Focus()

        txtreason.Focus()

        pbEmpPicLeave.Focus()

        txtcomments.Focus()

        pbEmpPicLeave.Focus()

        If dontUpdateLeave = 1 Then
            listofEditRowleave.Clear()
        End If

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        dgvempleave.EndEdit(True)

        If haserrinputleave = 1 Then
            '"Invalid Date issued or Date of expiration"

            'MsgBox(colName & vbNewLine & rowIndxleave)

            If dgvempleave.Item(colNameleave, rowIndxleave).ErrorText <> Nothing Then
                Dim invalids As String

                invalids = StrReverse(getStrBetween(StrReverse(dgvempleave.Item(colNameleave, rowIndxleave).ErrorText), "", " "))

                WarnBalloon("Please input a valid " & invalids & ".",
                              StrConv(dgvempleave.Item(colNameleave, rowIndxleave).ErrorText, VbStrConv.ProperCase),
                              lblforballoon, 0, -69)
            Else
                WarnBalloon("Please input a valid and complete leave.",
                            "Invalid employee leave",
                            lblforballoon, 0, -69)
            End If

            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            Exit Sub

        ElseIf dgvEmp.RowCount = 0 Then

            Exit Sub
        End If

        Dim param(13, 2) As Object

        param(0, 0) = "elv_RowID"
        param(1, 0) = "elv_OrganizationID"
        param(2, 0) = "elv_LeaveStartTime"
        param(3, 0) = "elv_LeaveType"
        param(4, 0) = "elv_CreatedBy"
        param(5, 0) = "elv_LastUpdBy"
        param(6, 0) = "elv_EmployeeID"
        param(7, 0) = "elv_LeaveEndTime"
        param(8, 0) = "elv_LeaveStartDate"
        param(9, 0) = "elv_LeaveEndDate"
        param(10, 0) = "elv_Reason"
        param(11, 0) = "elv_Comments"
        param(12, 0) = "elv_Image"
        param(13, 0) = "elv_Status"

        For Each r As DataGridViewRow In dgvempleave.Rows

            Dim elv_rowid = If(ValNoComma(r.Cells("elv_RowID").Value) = 0, DBNull.Value, r.Cells("elv_RowID").Value)

            If Val(r.Cells("elv_RowID").Value) = 0 And
                tsbtnNewLeave.Visible = True Then

                If r.IsNewRow = False Then

                    If r.Cells("elv_StartDate").Value <> Nothing And r.Cells("elv_EndDate").Value <> Nothing Then

                        param(0, 1) = DBNull.Value
                        param(1, 1) = orgztnID
                        param(2, 1) = MilitTime(r.Cells("elv_StartTime").Value) 'If(r.Cells("Column3").Value = Nothing, DBNull.Value, r.Cells("Column3").Value) 'Start time
                        param(3, 1) = If(r.Cells("elv_Type").Value = Nothing, DBNull.Value, r.Cells("elv_Type").Value) 'Leave type
                        param(4, 1) = z_User 'CreatedBy
                        param(5, 1) = z_User 'LastUpdBy
                        param(6, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                        param(7, 1) = MilitTime(r.Cells("elv_EndTime").Value) 'If(r.Cells("Column4").Value = Nothing, DBNull.Value, r.Cells("Column4").Value) 'End time
                        param(8, 1) = If(r.Cells("elv_StartDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("elv_StartDate").Value), "yyyy-MM-dd")) 'Start date
                        param(9, 1) = If(r.Cells("elv_EndDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("elv_EndDate").Value), "yyyy-MM-dd")) 'End date
                        param(10, 1) = If(r.Cells("elv_Reason").Value = Nothing, "", r.Cells("elv_Reason").Value) 'Reason
                        param(11, 1) = If(r.Cells("elv_Comment").Value = Nothing, "", r.Cells("elv_Comment").Value) 'Comments

                        Dim imageobj As Object = If(r.Cells("elv_Image").Value Is Nothing,
                                                    DBNull.Value,
                                                    r.Cells("elv_Image").Value) 'Image

                        param(12, 1) = imageobj

                        param(13, 1) = If(r.Cells("elv_Status").Value = Nothing, "", r.Cells("elv_Status").Value)

                        r.Cells("elv_RowID").Value =
                            New ReadSQLFunction("INSUPD_employeeleave",
                                                    "empleaveID",
                                                elv_rowid,
                                                orgztnID,
                                                MilitTime(r.Cells("elv_StartTime").Value),
                                                If(r.Cells("elv_Type").Value = Nothing, DBNull.Value, r.Cells("elv_Type").Value),
                                                z_User,
                                                z_User,
                                                dgvEmp.CurrentRow.Cells("RowID").Value,
                                                MilitTime(r.Cells("elv_EndTime").Value),
                                                If(r.Cells("elv_StartDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("elv_StartDate").Value), "yyyy-MM-dd")),
                                                If(r.Cells("elv_EndDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("elv_EndDate").Value), "yyyy-MM-dd")),
                                                If(r.Cells("elv_Reason").Value = Nothing, "", r.Cells("elv_Reason").Value),
                                                If(r.Cells("elv_Comment").Value = Nothing, "", r.Cells("elv_Comment").Value),
                                                DBNull.Value,
                                                If(r.Cells("elv_Status").Value = Nothing, "", r.Cells("elv_Status").Value)).ReturnValue

                    End If

                End If
            Else

                If listofEditRowleave.Contains(r.Cells("elv_RowID").Value) Then
                    If r.Cells("elv_StartTime").Value <> Nothing And r.Cells("elv_EndTime").Value <> Nothing _
                        And r.Cells("elv_StartDate").Value <> Nothing And r.Cells("elv_EndDate").Value <> Nothing Then

                        param(0, 1) = r.Cells("elv_RowID").Value
                        param(1, 1) = orgztnID
                        param(2, 1) = MilitTime(r.Cells("elv_StartTime").Value) 'If(r.Cells("Column3").Value = Nothing, DBNull.Value, r.Cells("Column3").Value) 'Start time
                        param(3, 1) = If(r.Cells("elv_Type").Value = Nothing, DBNull.Value, r.Cells("elv_Type").Value) 'Leave type
                        param(4, 1) = z_User 'CreatedBy
                        param(5, 1) = z_User 'LastUpdBy
                        param(6, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                        param(7, 1) = MilitTime(r.Cells("elv_EndTime").Value)  'End time
                        param(8, 1) = If(r.Cells("elv_StartDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("elv_StartDate").Value), "yyyy-MM-dd")) 'Start date
                        param(9, 1) = If(r.Cells("elv_EndDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("elv_EndDate").Value), "yyyy-MM-dd")) 'End date
                        param(10, 1) = If(r.Cells("elv_Reason").Value = Nothing, "", r.Cells("elv_Reason").Value) 'Reason
                        param(11, 1) = If(r.Cells("elv_Comment").Value = Nothing, "", r.Cells("elv_Comment").Value) 'Comments
                        param(12, 1) = If(r.Cells("elv_Image").Value Is Nothing, DBNull.Value, r.Cells("elv_Image").Value)

                        param(13, 1) = If(r.Cells("elv_Status").Value = Nothing, "", r.Cells("elv_Status").Value)

                        'EXEC_INSUPD_PROCEDURE(param, "INSUPD_employeeleave", "empleaveID")

                        Dim n_ReadSQLFunction As _
                            New ReadSQLFunction("INSUPD_employeeleave",
                                                    "empleaveID",
                                                elv_rowid,
                                                orgztnID,
                                                MilitTime(r.Cells("elv_StartTime").Value),
                                                If(r.Cells("elv_Type").Value = Nothing, DBNull.Value, r.Cells("elv_Type").Value),
                                                z_User,
                                                z_User,
                                                dgvEmp.CurrentRow.Cells("RowID").Value,
                                                MilitTime(r.Cells("elv_EndTime").Value),
                                                If(r.Cells("elv_StartDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("elv_StartDate").Value), "yyyy-MM-dd")),
                                                If(r.Cells("elv_EndDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("elv_EndDate").Value), "yyyy-MM-dd")),
                                                If(r.Cells("elv_Reason").Value = Nothing, "", r.Cells("elv_Reason").Value),
                                                If(r.Cells("elv_Comment").Value = Nothing, "", r.Cells("elv_Comment").Value),
                                                DBNull.Value,
                                                If(r.Cells("elv_Status").Value = Nothing, "", r.Cells("elv_Status").Value))

                    End If
                End If
            End If
        Next

        listofEditRowleave.Clear()

        If hasERR = 0 Then

            '                                           'dgvEmp
            InfoBalloon("Changes made in Employee ID '" & dgvEmp.CurrentRow.Cells("Column1").Value & "' has successfully saved.", "Changes successfully save", lblforballoon, 0, -69)
        Else
            dgvEmp_SelectionChanged(sender, e)

        End If

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

    End Sub

    Sub INSUPD_employeeattachments(Optional eatta_RowID As Object = Nothing,
                                   Optional eatta_EmployeeID As Object = Nothing,
                                   Optional eatta_Type As Object = Nothing,
                                   Optional eatta_FileType As Object = Nothing,
                                   Optional eatta_FileName As Object = Nothing)

        Dim params(7, 2) As Object

        params(0, 0) = "eatta_RowID"
        params(1, 0) = "eatta_CreatedBy"
        params(2, 0) = "eatta_LastUpdBy"
        params(3, 0) = "eatta_EmployeeID"
        params(4, 0) = "eatta_Type"
        params(5, 0) = "eatta_AttachedFile"
        params(6, 0) = "eatta_FileType"
        params(7, 0) = "eatta_FileName"

        params(0, 1) = If(eatta_RowID = Nothing, DBNull.Value, eatta_RowID)
        params(1, 1) = z_User
        params(2, 1) = z_User
        params(3, 1) = eatta_EmployeeID
        params(4, 1) = eatta_Type '"Employee Leave@RowIDemployeeleave"
        params(5, 1) = DBNull.Value
        params(6, 1) = eatta_FileType
        params(7, 1) = eatta_FileName

        EXEC_INSUPD_PROCEDURE(params,
                              "INSUPD_employeeattachments",
                              "eatta_ID")
    End Sub

    Dim isdlleave As SByte = 0

    Private Sub btndlleavefile_Click(sender As Object, e As EventArgs) Handles btndlleavefile.Click
        isdlleave = 1

        If dgvempleave.RowCount <> 1 Then
            dgvempleave.Item("elv_viewimage",
                             dgvempleave.CurrentRow.Index).Selected = True

            Dim dgvceleventarg As New DataGridViewCellEventArgs(elv_viewimage.Index,
                                                                dgvempleave.CurrentRow.Index)

            dgvempleave_CellContentClick(sender, dgvceleventarg)
        End If
    End Sub

    Dim promptresult As Object

    Private Sub dgvempleave_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempleave.CellContentClick

        If dgvempleave.CurrentCell.ColumnIndex = dgvempleave.Columns("elv_viewimage").Index Then
            If dgvempleave.CurrentRow.Cells("elv_Image").Value IsNot Nothing Then

                If isdlleave = 1 Then
                    promptresult = Windows.Forms.DialogResult.Yes
                    isdlleave = 0
                Else
                    promptresult = Windows.Forms.DialogResult.No
                End If

                If promptresult = Windows.Forms.DialogResult.No Then

                    Dim _attafileextensn,
                        _attafilename As String

                    _attafilename = dgvempleave.CurrentRow.Cells("elv_attafilename").Value
                    _attafileextensn = dgvempleave.CurrentRow.Cells("elv_attafileextensn").Value

                    Dim tmp_path = Path.GetTempPath &
                                                _attafilename & _attafileextensn

                    If _attafileextensn = Nothing Then
                    Else
                        If Trim(_attafilename) = Nothing Then
                            dgvempleave.CurrentRow.Cells("elv_attafilename").Selected = 1
                            dgvempleave.BeginEdit(1)
                            InfoBalloon("Please input a file name.", "Attachment has no file name", Label233, 0, -69)
                        Else
                            Dim file_stream As New FileStream(tmp_path, FileMode.Create)
                            Dim blob As Byte() = DirectCast(dgvempleave.CurrentRow.Cells("elv_Image").Value, Byte())
                            file_stream.Write(blob, 0, blob.Length)
                            file_stream.Close()
                            file_stream = Nothing

                            Process.Start(tmp_path)
                        End If

                    End If
                Else 'If promptresult = Windows.Forms.DialogResult.Yes Then

                    Dim dlImage As SaveFileDialog = New SaveFileDialog
                    dlImage.RestoreDirectory = True

                    If dlImage.ShowDialog = Windows.Forms.DialogResult.OK Then

                        Dim savefilepath As String =
                            Path.GetFullPath(dlImage.FileName) &
                            dgvempleave.CurrentRow.Cells("elv_attafileextensn").Value

                        Dim fs As New FileStream(savefilepath, FileMode.Create)
                        Dim blob As Byte() = DirectCast(dgvempleave.CurrentRow.Cells("elv_Image").Value, Byte())
                        fs.Write(blob, 0, blob.Length)
                        fs.Close()
                        fs = Nothing

                        Process.Start(savefilepath)

                    End If
                End If
            Else
                MsgBox("Nothing to view", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Dim prev_elv_Type,
        prev_elv_StartTime,
        prev_elv_EndTime,
        prev_elv_StartDate,
        prev_elv_EndDate,
        prev_elv_Reason,
        prev_elv_Comment,
        prev_elv_Status As String

    Private Sub dgvempleave_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvempleave.SelectionChanged
        Try

            If dgvempleave.RowCount > 1 Then

                With dgvempleave.CurrentRow

                    If .IsNewRow = False Then

                        dgvleaveRowindx = .Index

                        prev_elv_Type = .Cells("elv_Type").Value

                        prev_elv_StartTime = If(CStr(.Cells("elv_StartTime").Value) = "", "", CDate(.Cells("elv_StartTime").Value).ToShortTimeString)

                        prev_elv_EndTime = If(CStr(.Cells("elv_EndTime").Value) = "", "", CDate(.Cells("elv_EndTime").Value).ToShortTimeString)

                        prev_elv_StartDate = Format(CDate(.Cells("elv_StartDate").Value), machineShortDateFormat)

                        prev_elv_EndDate = Format(CDate(.Cells("elv_EndDate").Value), machineShortDateFormat)

                        prev_elv_Reason = CStr(.Cells("elv_Reason").Value)

                        prev_elv_Comment = CStr(.Cells("elv_Comment").Value)

                        prev_elv_Status = CStr(.Cells("elv_Status").Value)

                        cboleavetypes.Text = prev_elv_Type '.Cells("elv_Type").Value

                        txtstarttime.Text = prev_elv_StartTime '.Cells("elv_StartTime").Value
                        txtendtime.Text = prev_elv_EndTime '.Cells("elv_EndTime").Value

                        If prev_elv_StartDate = "1/1/0001" Then
                            dtpstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
                        Else
                            dtpstartdate.Value = Format(CDate(prev_elv_StartDate), machineShortDateFormat) '.Cells("elv_StartDate").Value
                        End If

                        If prev_elv_EndDate = "1/1/0001" Then
                            dtpendate.Value = Format(CDate(dbnow), machineShortDateFormat)
                        Else
                            dtpendate.Value = Format(CDate(prev_elv_EndDate), machineShortDateFormat) '.Cells("elv_StartDate").Value
                        End If

                        txtreason.Text = prev_elv_Reason '.Cells("elv_Reason").Value
                        txtcomments.Text = prev_elv_Comment '.Cells("elv_Comment").Value

                        pbempleave.Image = Nothing

                        pbempleave.Image = ConvByteToImage(DirectCast(.Cells("elv_Image").Value, Byte()))

                        cboleavestatus.Text = prev_elv_Status
                    Else
                        clear()
                    End If
                End With
            Else
                dgvleaveRowindx = 0
                'ObjectFields(dgvempleave, 1)
                clear()
                'clearObjControl(TabPage1)
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))

        End Try
        myEllipseButton(dgvempleave, "elv_Type", btnleavtyp)
    End Sub

    Private Sub dgvempleave_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvempleave.Scroll
        myEllipseButton(dgvempleave, "elv_Type", btnleavtyp)
    End Sub

    Sub clear()

        cboleavetypes.SelectedIndex = -1
        txtstarttime.Text = ""
        txtendate.Text = ""
        txtstartdate.Text = ""
        txtendtime.Text = ""
        txtreason.Text = ""
        txtcomments.Text = ""

        pbempleave.Image = Nothing

        cboleavestatus.SelectedIndex = -1
        cboleavestatus.Text = ""

    End Sub

    Public listofEditRowleave As New AutoCompleteStringCollection

    Private Sub dgvempleave_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvempleave.CellBeginEdit

        RemoveHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged

    End Sub

    Dim colNameleave As String
    Dim rowIndxleave As Integer

    Private Sub dgvempleave_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempleave.CellEndEdit

        dgvempleave.ShowCellErrors = True

        Static num As Integer = -1

        colNameleave = dgvempleave.Columns(e.ColumnIndex).Name
        rowIndxleave = e.RowIndex

        If Val(dgvempleave.Item("elv_RowID", e.RowIndex).Value) <> 0 Then

            listofEditRowleave.Add(dgvempleave.Item("elv_RowID", e.RowIndex).Value)
        Else
        End If

        If (colNameleave = "elv_StartDate" Or colNameleave = "elv_EndDate") Then
            Dim dateobj As Object = Trim(dgvempleave.Item(colNameleave, rowIndxleave).Value)
            Try
                dgvempleave.Item(colNameleave, rowIndxleave).Value = Format(CDate(dateobj), machineShortDateFormat)

                haserrinputleave = 0

                dgvempleave.Item(colNameleave, rowIndxleave).ErrorText = Nothing

                If dgvempleave.Item("elv_StartDate", rowIndxleave).Value <> Nothing _
                    And dgvempleave.Item("elv_EndDate", rowIndxleave).Value <> Nothing Then

                    Dim date_differ = DateDiff(DateInterval.Day,
                                                CDate(dgvempleave.Item("elv_StartDate", rowIndxleave).Value),
                                                CDate(dgvempleave.Item("elv_EndDate", rowIndxleave).Value))

                    If date_differ < 0 Then
                        haserrinputleave = 1
                        dgvempleave.Item(colNameleave, rowIndxleave).ErrorText = "     Invalid date value"
                    Else
                        dgvempleave.Item("elv_StartDate", rowIndxleave).ErrorText = Nothing
                        dgvempleave.Item("elv_EndDate", rowIndxleave).ErrorText = Nothing
                    End If

                    Dim _from = Format(CDate(dgvempleave.Item("elv_StartDate", rowIndxleave).Value), "yyyy-MM-dd")
                    Dim _to = Format(CDate(dgvempleave.Item("elv_EndDate", rowIndxleave).Value), "yyyy-MM-dd")

                    Dim invalidleave = 0

                    If dgvEmp.RowCount <> 0 Then
                        invalidleave = EXECQUER("SELECT EXISTS(SELECT RowID" &
                                                    " FROM employeetimeentry" &
                                                    " WHERE DATE BETWEEN '" & _from & "'" &
                                                    " AND '" & _to & "'" &
                                                    " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                                    " AND OrganizationID=" & orgztnID & ");")

                        If invalidleave = 0 Then
                            invalidleave = EXECQUER("SELECT EXISTS(SELECT RowID" &
                                                    " FROM employeeleave" &
                                                    " WHERE " &
                                                    " ('" & _from & "' IN (LeaveStartDate,LeaveEndDate) OR '" & _to & "' IN (LeaveStartDate,LeaveEndDate))" &
                                                    " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                                    " AND OrganizationID=" & orgztnID & ");")
                        End If
                    End If

                    If listofEditRowleave.Contains(dgvempleave.Item("elv_RowID", e.RowIndex).Value) Then
                        haserrinputleave = 0
                        dgvempleave.Item(colNameleave, rowIndxleave).ErrorText = Nothing
                    Else

                        If invalidleave = 1 Then
                            haserrinputleave = 1
                            dgvempleave.Item(colNameleave, rowIndxleave).ErrorText = "     The employee has already a record on this date"
                        End If
                    End If
                End If
            Catch ex As Exception
                haserrinputleave = 1
                dgvempleave.Item(colNameleave, rowIndxleave).ErrorText = "     Invalid date value"
            End Try

        ElseIf (colNameleave = "elv_StartTime" Or colNameleave = "elv_EndTime") Then

            Dim dateobj As Object = Trim(dgvempleave.Item(colNameleave, rowIndxleave).Value).Replace(" ", ":")

            Dim ampm As String = Nothing

            Try
                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If

                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")
                If ampm = Nothing Then
                    dgvempleave.Item(colNameleave, rowIndxleave).Value = valtime.ToShortTimeString
                Else
                    dgvempleave.Item(colNameleave, rowIndxleave).Value = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If

                haserrinputleave = 0

                dgvempleave.Item(colNameleave, rowIndxleave).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")

                    dgvempleave.Item(colNameleave, rowIndxleave).Value = valtime.ToShortTimeString

                    haserrinputleave = 0

                    dgvempleave.Item(colNameleave, rowIndxleave).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputleave = 1
                    dgvempleave.Item(colNameleave, rowIndxleave).ErrorText = "     Invalid time value"
                End Try
            End Try

        ElseIf colNameleave = "elv_Type" Then
            If dgvempleave.Item("elv_Type", rowIndxleave).Value = Nothing Then
                haserrinputleave = 1
                dgvempleave.Item("elv_Type", rowIndxleave).ErrorText = "     Invalid leave type"
            Else
                haserrinputleave = 0
                dgvempleave.Item("elv_Type", rowIndxleave).ErrorText = Nothing
            End If
        End If

        dgvempleave.Item("elv_viewimage", rowIndxleave).Value = "view this"

        dgvempleave.AutoResizeRow(rowIndxleave)
        dgvempleave.PerformLayout()

        AddHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged

    End Sub

    Dim haserrinputleave As SByte

    Function MilitTime(ByVal timeval As Object) As Object

        Dim retrnObj As Object

        retrnObj = New Object

        If timeval = Nothing Then
            retrnObj = DBNull.Value
        Else

            Dim endtime As Object = timeval

            If endtime.ToString.Contains("P") Then

                Dim hrs As String = If(Val(getStrBetween(endtime, "", ":")) = 12, 12, Val(getStrBetween(endtime, "", ":")) + 12)

                Dim mins As String = StrReverse(getStrBetween(StrReverse(endtime.ToString), "", ":"))

                mins = getStrBetween(mins, "", " ")

                retrnObj = hrs & ":" & mins

            ElseIf endtime.ToString.Contains("A") Then

                Dim i As Integer = StrReverse(endtime).ToString.IndexOf(" ")

                endtime = endtime.ToString.Replace("A", "")

                Dim amTime As String = Trim(StrReverse(StrReverse(endtime.ToString).Substring(i,
                                                                                  endtime.ToString.Length - i)
                                          )
                               )

                amTime = If(getStrBetween(amTime, "", ":") = "12",
                            24 & ":" & StrReverse(getStrBetween(StrReverse(amTime), "", ":")),
                            amTime)

                retrnObj = amTime
            End If
        End If
        Return retrnObj

    End Function

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        listofEditRowleave.Clear()
        dgvEmp_SelectionChanged(sender, e)
    End Sub

    'elv_RowID@RowID&False
    'elv_Type@Leave type&True
    'elv_StartTime@Start time&True
    'elv_EndTime@End time&True
    'elv_StartDate@Start date&True
    'elv_EndDate@End date&True
    'elv_Reason@Reason&True
    'elv_Comment@Comments&True
    'elv_Image@Image&False

    Dim currleavcolmn As String

    Private Sub dgvempleave_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvempleave.EditingControlShowing
        currleavcolmn = dgvempleave.Columns(dgvempleave.CurrentCell.ColumnIndex).Name

        e.Control.ContextMenu = New ContextMenu

        If currleavcolmn = "elv_Type" Then

            With DirectCast(e.Control, TextBox)
                .AutoCompleteCustomSource = leavetype
                .AutoCompleteMode = AutoCompleteMode.Suggest
                .AutoCompleteSource = AutoCompleteSource.CustomSource
            End With
        Else
        End If
    End Sub

    Dim thefilepath As String

    Dim atta_name,
        atta_extensn As String

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        'RemoveHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged
        atta_name = Nothing
        atta_extensn = Nothing

        Static employeeleaveRowID As Integer = -1
        Try
            Dim browsefile As OpenFileDialog = New OpenFileDialog()
            'browsefile.Filter = "JPEG(*.jpg)|*.jpg"
            browsefile.Filter = "All files (*.*)|*.*" &
                                "|JPEG (*.jpg)|*.jpg" &
                                "|PNG (*.PNG)|*.png" &
                                "|MS Word 97-2003 Document (*.doc)|*.doc" &
                                "|MS Word Document (*.docx)|*.docx" &
                                "|MS Excel 97-2003 Workbook (*.xls)|*.xls" &
                                "|MS Excel Workbook (*.xlsx)|*.xlsx"

            '|" & _
            '"PNG(*.PNG)|*.png|" & _
            '"Bitmap(*.BMP)|*.bmp"
            If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then
                With dgvempleave
                    .Focus()

                    thefilepath = browsefile.FileName
                    atta_name = Path.GetFileNameWithoutExtension(thefilepath)
                    atta_extensn = Path.GetExtension(thefilepath)

                    If .CurrentRow.IsNewRow Then
                        Dim e_rowindx As Integer = .CurrentRow.Index
                        Dim currcol As String = .Columns(.CurrentCell.ColumnIndex).Name
                        .Rows.Add()
                        .Item("elv_Image", e_rowindx).Value = Nothing
                        .Item("elv_Image", e_rowindx).Value = convertFileToByte(thefilepath)

                        .Item("elv_attafilename", e_rowindx).Value = atta_name
                        .Item("elv_attafileextensn", e_rowindx).Value = atta_extensn

                        .Item(currcol, e_rowindx).Selected = True
                    Else
                        .CurrentRow.Cells("elv_Image").Value = Nothing
                        .CurrentRow.Cells("elv_Image").Value = convertFileToByte(thefilepath)

                        .CurrentRow.Cells("elv_attafilename").Value = atta_name
                        .CurrentRow.Cells("elv_attafileextensn").Value = atta_extensn

                        If employeeleaveRowID <> Val(dgvempleave.Item("elv_RowID", .CurrentRow.Index).Value) Then
                            listofEditRowleave.Add(dgvempleave.Item("elv_RowID", .CurrentRow.Index).Value)
                        End If
                    End If
                    .Focus()
                End With
            Else

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        Finally
            dgvempleave_SelectionChanged(sender, e)
            'AddHandler dgvempleave.SelectionChanged, AddressOf dgvempleave_SelectionChanged
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Static employeeleaveRowID As Integer = -1

        pbempleave.Image = Nothing

        If dgvempleave.RowCount = 1 Then
            dgvempleave.Item("elv_Image", 0).Value = Nothing
        Else
            If dgvempleave.CurrentRow.IsNewRow = False Then
                dgvempleave.CurrentRow.Cells("elv_Image").Value = Nothing
                If employeeleaveRowID <> Val(dgvempleave.CurrentRow.Cells("elv_RowID").Value) Then
                    listofEditRowleave.Add(dgvempleave.CurrentRow.Cells("elv_RowID").Value)
                End If
            End If
        End If
    End Sub

    Private Sub cboleavetypes_GotFocus(sender As Object, e As EventArgs) Handles cboleavetypes.GotFocus

        If dgvempleave.RowCount = 1 Then
        Else
            dgvempleave.Item("elv_Type", dgvleaveRowindx).Selected = True
        End If
    End Sub

    Dim dgvleaveRowindx As Integer

    Private Sub cboleavetypes_Leave(sender As Object, e As EventArgs) Handles cboleavetypes.Leave

        colNameleave = "elv_Type"

        Dim thegetval = Trim(cboleavetypes.Text) 'If(sendr_name = "txtleavetype", Trim(txtleavetype.Text), Trim(cboleavetypes.Text))

        If dgvempleave.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempleave.Rows.Add()
                dgvleaveRowindx = dgvempleave.RowCount - 2
            End If
        Else
            If dgvempleave.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempleave.Rows.Add()
                    dgvleaveRowindx = dgvempleave.RowCount - 2
                End If
            Else
                dgvleaveRowindx = dgvempleave.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempleave.Item("elv_Type", dgvleaveRowindx).Value = thegetval

            cboleavetypes.Text = thegetval

            dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

            If thegetval <> prev_elv_Type _
                    And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
                listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
            End If
        End If
    End Sub

    Private Sub Label225_Click(sender As Object, e As EventArgs) Handles Label225.Click

        InfoBalloon(, , txtstartdate, , , 1)

        txtstarttime.Focus()

        InfoBalloon("Ex. The time is 08:30:15 am" & vbNewLine &
                    "     just type '8 30'(don't include the apostrophe(')) and press Tab key or Enter key" & vbNewLine &
                    "Ex. The time is 06:15:15 pm" & vbNewLine &
                    "     if it is 'pm', get the hour and then plus 12(twelve)" & vbNewLine &
                    "     the hour is 6 so, 6 + 12 = 18" & vbNewLine &
                    "     just type '18 15'" & vbNewLine &
                    "     or '18:15 pm'(don't include the apostrophe(')) and press Tab key or Enter key" & vbNewLine &
                    "Ex. The time is 12:12:12 pm" & vbNewLine &
                    "     if it is 'pm', and the hour is equal to twelve(12)" & vbNewLine &
                    "     no need to add 12" & vbNewLine &
                    "     just type '12 12'" & vbNewLine &
                    "     or '12:12 pm'(don't include the apostrophe(')) and press Tab key or Enter key" & vbNewLine,
                    "How to input Time ?",
                    txtstarttime, 82, -240, , 3600000)

    End Sub

    Private Sub txtstarttime_GotFocus(sender As Object, e As EventArgs) Handles txtstarttime.GotFocus

        If dgvempleave.RowCount = 1 Then
        Else
            dgvempleave.Item("elv_StartTime", dgvleaveRowindx).Selected = True
        End If
    End Sub

    Private Sub txtstarttime_Leave(sender As Object, e As EventArgs) Handles txtstarttime.Leave

        InfoBalloon(, , txtstarttime, , , 1)

        colNameleave = "elv_StartTime"

        Dim thegetval = Trim(txtstarttime.Text)

        Dim theretval = ""

        If dgvempleave.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempleave.Rows.Add()
                dgvleaveRowindx = dgvempleave.RowCount - 2
            End If
        Else

            If dgvempleave.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempleave.Rows.Add()
                    dgvleaveRowindx = dgvempleave.RowCount - 2
                End If
            Else
                dgvleaveRowindx = dgvempleave.CurrentRow.Index
            End If

        End If

        Dim dateobj As Object = thegetval.Replace(" ", ":")
        Dim ampm As String = Nothing

        If thegetval <> "" Then
            Try

                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If
                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")

                If ampm = Nothing Then
                    theretval = valtime.ToShortTimeString
                Else
                    theretval = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If
                haserrinputleave = 0

                dgvempleave.Item("elv_StartTime", dgvleaveRowindx).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")
                    'valtime = DateTime.Parse(e.FormattedValue)
                    'valtime = valtime.ToShortTimeString
                    theretval = valtime.ToShortTimeString
                    'Format(valtime, "hh:mm tt")

                    haserrinputleave = 0

                    dgvempleave.Item("elv_StartTime", dgvleaveRowindx).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputleave = 1
                    dgvempleave.Item("elv_StartTime", dgvleaveRowindx).ErrorText = "     Invalid time value"
                Finally
                    If thegetval <> "" Then
                        dgvempleave.Item("elv_StartTime", dgvleaveRowindx).Value = theretval
                        txtstarttime.Text = theretval
                        dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

                        If theretval <> prev_elv_StartTime _
                        And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
                            listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
                        End If

                    End If
                End Try
            Finally
                If thegetval <> "" Then
                    dgvempleave.Item("elv_StartTime", dgvleaveRowindx).Value = theretval
                    txtstarttime.Text = theretval
                    dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

                    If theretval <> prev_elv_StartTime _
                        And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
                        listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
                    End If

                End If
            End Try
        End If

    End Sub

    Private Sub txtendtime_TextChanged(sender As Object, e As EventArgs) Handles txtendtime.TextChanged

    End Sub

    Private Sub txtendtime_GotFocus(sender As Object, e As EventArgs) Handles txtendtime.GotFocus

        If dgvempleave.RowCount = 1 Then
        Else
            dgvempleave.Item("elv_EndTime", dgvleaveRowindx).Selected = True
        End If

    End Sub

    Private Sub txtendtime_Leave(sender As Object, e As EventArgs) Handles txtendtime.Leave

        colNameleave = "elv_EndTime"

        Dim thegetval = Trim(txtendtime.Text)

        Dim theretval = ""

        If dgvempleave.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempleave.Rows.Add()
                dgvleaveRowindx = dgvempleave.RowCount - 2
            End If
        Else
            If dgvempleave.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempleave.Rows.Add()
                    dgvleaveRowindx = dgvempleave.RowCount - 2
                End If
            Else
                dgvleaveRowindx = dgvempleave.CurrentRow.Index
            End If
        End If

        Dim dateobj As Object = thegetval.Replace(" ", ":")
        Dim ampm As String = Nothing

        If thegetval <> "" Then
            Try

                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If

                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")

                If ampm = Nothing Then
                    theretval = valtime.ToShortTimeString
                Else
                    theretval = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If

                haserrinputleave = 0

                dgvempleave.Item("elv_EndTime", dgvleaveRowindx).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")

                    theretval = valtime.ToShortTimeString

                    haserrinputleave = 0

                    dgvempleave.Item("elv_EndTime", dgvleaveRowindx).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputleave = 1
                    dgvempleave.Item("elv_EndTime", dgvleaveRowindx).ErrorText = "     Invalid time value"
                Finally
                    If thegetval <> "" Then
                        dgvempleave.Item("elv_EndTime", dgvleaveRowindx).Value = theretval
                        txtendtime.Text = theretval
                        dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

                        If theretval <> prev_elv_EndTime _
                        And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
                            listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
                        End If

                    End If
                End Try
            Finally
                If thegetval <> "" Then
                    dgvempleave.Item("elv_EndTime", dgvleaveRowindx).Value = theretval
                    txtendtime.Text = theretval
                    dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

                    If theretval <> prev_elv_EndTime _
                        And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
                        listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
                    End If

                End If
            End Try
        End If

    End Sub

    Private Sub cboleavestatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboleavestatus.SelectedIndexChanged

    End Sub

    Private Sub cboleavestatus_GotFocus(sender As Object, e As EventArgs) Handles cboleavestatus.GotFocus

        If dgvempleave.RowCount = 1 Then
        Else
            dgvempleave.Item("elv_Status", dgvleaveRowindx).Selected = True
        End If

    End Sub

    Private Sub cboleavestatus_Leave(sender As Object, e As EventArgs) Handles cboleavestatus.Leave

        colNameleave = "elv_Status"

        'Dim sendr_name As String = CType(sender, Object).Name

        Dim thegetval = Trim(cboleavestatus.Text) 'If(sendr_name = "txtleavetype", Trim(txtleavetype.Text), Trim(cboleavestatus.Text))

        If dgvempleave.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempleave.Rows.Add()
                dgvleaveRowindx = dgvempleave.RowCount - 2
                'dgvempleave.Item("elv_Status", dgvleaveRowindx).Selected = True
            End If
        Else
            If dgvempleave.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempleave.Rows.Add()
                    dgvleaveRowindx = dgvempleave.RowCount - 2
                    'dgvempleave.Item("elv_Status", dgvleaveRowindx).Selected = True
                End If
            Else
                dgvleaveRowindx = dgvempleave.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempleave.Item("elv_Status", dgvleaveRowindx).Value = thegetval

            cboleavestatus.Text = thegetval

            dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

            If thegetval <> prev_elv_Status _
                    And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
                listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
            End If

        End If

    End Sub

    Private Sub dtpstartdate_ValueChanged(sender As Object, e As EventArgs) Handles dtpstartdate.ValueChanged

        Dim has_valid_dgvrow As Boolean = False

        Try
            curr_leave_dgvrow = dgvempleave.CurrentRow
            has_valid_dgvrow = (curr_leave_dgvrow IsNot Nothing _
                                And curr_leave_dgvrow.Cells("elv_RowID").Value = Nothing)
        Catch ex As Exception
            has_valid_dgvrow = False
        Finally
            If has_valid_dgvrow Then
                Dim date_value = dtpstartdate.Value
                dtpendate.Value = date_value
                curr_leave_dgvrow.Cells("elv_EndDate").Value = date_value

            End If

        End Try

    End Sub

    Private Sub txtstartdate_TextChanged(sender As Object, e As EventArgs) Handles txtstartdate.TextChanged

    End Sub

    Private Sub Label224_Click(sender As Object, e As EventArgs) Handles Label224.Click

        InfoBalloon(, , txtstarttime, , , 1)

        txtstartdate.Focus()

        InfoBalloon("Ex. The date is June 1 2015" & vbNewLine &
                    "     just type '6 1 15'(don't include the apostrophe(')) and press Tab key or Enter key" & vbNewLine & vbNewLine &
                    "Ex. The date is 2015-07-02" & vbNewLine &
                    "     just type '7 2 15'(don't include the apostrophe(')) and press Tab key or Enter key" & vbNewLine,
                    "How to input Date ?",
                    txtstartdate, 82, -135, , 3600000)

    End Sub

    Private Sub Label224_MouseEnter(sender As Object, e As EventArgs) Handles Label224.MouseEnter

    End Sub

    Private Sub Label224_MouseLeave(sender As Object, e As EventArgs) Handles Label224.MouseLeave

    End Sub

    '
    Private Sub dtpstartdate_GotFocus(sender As Object, e As EventArgs) Handles dtpstartdate.GotFocus

        If dgvempleave.RowCount = 1 Then
        Else
            dgvempleave.Item("elv_StartDate", dgvleaveRowindx).Selected = True
        End If

    End Sub

    Private Sub txtstartdate_GotFocus(sender As Object, e As EventArgs) Handles txtstartdate.GotFocus

    End Sub

    Private Sub dtpstartdate_Leave(sender As Object, e As EventArgs) Handles dtpstartdate.Leave

        InfoBalloon(, , txtstartdate, , , 1)

        colNameleave = "elv_StartDate"

        Dim thegetval = Trim(dtpstartdate.Value)

        Dim theretval = ""

        If dgvempleave.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempleave.Rows.Add()
                dgvleaveRowindx = dgvempleave.RowCount - 2
                'dgvempleave.Item("elv_StartDate", dgvleaveRowindx).Selected = True
            End If
        Else
            If dgvempleave.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempleave.Rows.Add()
                    dgvleaveRowindx = dgvempleave.RowCount - 2
                    'dgvempleave.Item("elv_StartDate", dgvleaveRowindx).Selected = True
                End If
            Else
                dgvleaveRowindx = dgvempleave.CurrentRow.Index
            End If
        End If

        Dim dateobj As Object = Trim(thegetval)

        If thegetval <> "" Then
            Try
                theretval = Format(CDate(dateobj), machineShortDateFormat)

                haserrinputleave = 0

                dgvempleave.Item("elv_StartDate", dgvleaveRowindx).ErrorText = Nothing

                If thegetval <> Nothing _
                    And Trim(txtendate.Text) <> Nothing Then

                    Dim date_differ = DateDiff(DateInterval.Day,
                                                CDate(thegetval),
                                                CDate(Trim(txtendate.Text)))

                    If date_differ < 0 Then
                        haserrinputleave = 1
                        dgvempleave.Item("elv_StartDate", dgvleaveRowindx).ErrorText = "     Invalid date value"
                    Else
                        dgvempleave.Item("elv_StartDate", dgvleaveRowindx).ErrorText = Nothing
                        dgvempleave.Item("elv_EndDate", dgvleaveRowindx).ErrorText = Nothing
                    End If

                    Dim _from = Format(CDate(theretval), "yyyy-MM-dd")
                    Dim _to = Format(CDate(dgvempleave.Item("elv_EndDate", dgvleaveRowindx).Value), "yyyy-MM-dd")

                    Dim invalidleave = 0

                    If dgvEmp.RowCount <> 0 Then
                        invalidleave = EXECQUER("SELECT EXISTS(SELECT RowID" &
                                                    " FROM employeetimeentry" &
                                                    " WHERE DATE BETWEEN '" & _from & "'" &
                                                    " AND '" & _to & "'" &
                                                    " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                                    " AND OrganizationID=" & orgztnID & ");")

                        If invalidleave = 0 Then
                            invalidleave = EXECQUER("SELECT EXISTS(SELECT RowID" &
                                                    " FROM employeeleave" &
                                                    " WHERE " &
                                                    " ('" & _from & "' IN (LeaveStartDate,LeaveEndDate) OR '" & _to & "' IN (LeaveStartDate,LeaveEndDate))" &
                                                    " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                                    " AND OrganizationID=" & orgztnID & ");")
                        End If
                    End If

                    If invalidleave = 1 Then
                        haserrinputleave = 1
                        dgvempleave.Item("elv_StartDate", dgvleaveRowindx).ErrorText = "     The employee has already a leave record on this date"
                    End If

                End If
            Catch ex As Exception
                haserrinputleave = 1
                dgvempleave.Item("elv_StartDate", dgvleaveRowindx).ErrorText = "     Invalid date value"
            Finally
                If thegetval <> "" Then
                    dgvempleave.Item("elv_StartDate", dgvleaveRowindx).Value = theretval
                    dtpstartdate.Value = Format(CDate(theretval), machineShortDateFormat)
                    dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

                    If theretval <> prev_elv_StartDate _
                        And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
                        listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
                    End If

                End If
            End Try

        End If

    End Sub

    Private Sub txtstartdate_Leave(sender As Object, e As EventArgs) Handles txtstartdate.Leave
    End Sub

    Private Sub dtpendate_ValueChanged(sender As Object, e As EventArgs) Handles dtpendate.ValueChanged
    End Sub

    Private Sub txtendate_TextChanged(sender As Object, e As EventArgs) Handles txtendate.TextChanged
    End Sub

    Private Sub dtpendate_GotFocus(sender As Object, e As EventArgs) Handles dtpendate.GotFocus

        If dgvempleave.RowCount = 1 Then
        Else
            dgvempleave.Item("elv_EndDate", dgvleaveRowindx).Selected = True
        End If

    End Sub

    Private Sub txtendate_GotFocus(sender As Object, e As EventArgs) Handles txtendate.GotFocus
    End Sub

    Private Sub dtpendate_Leave(sender As Object, e As EventArgs) Handles dtpendate.Leave

        colNameleave = "elv_EndDate"

        Dim thegetval = Trim(dtpendate.Value)

        Dim theretval = ""

        If dgvempleave.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempleave.Rows.Add()
                dgvleaveRowindx = dgvempleave.RowCount - 2
            End If
        Else
            If dgvempleave.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempleave.Rows.Add()
                    dgvleaveRowindx = dgvempleave.RowCount - 2
                End If
            Else
                dgvleaveRowindx = dgvempleave.CurrentRow.Index
            End If
        End If

        Dim dateobj As Object = Trim(thegetval)

        If thegetval <> "" Then
            Try
                theretval = Format(CDate(dateobj), machineShortDateFormat)

                haserrinputleave = 0

                dgvempleave.Item("elv_EndDate", dgvleaveRowindx).ErrorText = Nothing

                If thegetval <> Nothing _
                    And Trim(txtstartdate.Text) <> Nothing Then

                    Dim date_differ = DateDiff(DateInterval.Day,
                                                CDate(Trim(txtstartdate.Text)),
                                                CDate(thegetval))

                    If date_differ < 0 Then
                        haserrinputleave = 1
                        dgvempleave.Item("elv_EndDate", dgvleaveRowindx).ErrorText = "     Invalid date value"
                    Else
                        dgvempleave.Item("elv_StartDate", dgvleaveRowindx).ErrorText = Nothing
                        dgvempleave.Item("elv_EndDate", dgvleaveRowindx).ErrorText = Nothing
                    End If

                    Dim _from = Format(CDate(Trim(txtstartdate.Text)), "yyyy-MM-dd")
                    Dim _to = Format(CDate(theretval), "yyyy-MM-dd")

                    Dim invalidleave = 0

                    If dgvEmp.RowCount <> 0 Then
                        invalidleave = EXECQUER("SELECT EXISTS(SELECT RowID" &
                                                    " FROM employeetimeentry" &
                                                    " WHERE DATE BETWEEN '" & _from & "'" &
                                                    " AND '" & _to & "'" &
                                                    " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                                    " AND OrganizationID=" & orgztnID & ");")

                        If invalidleave = 0 Then
                            invalidleave = EXECQUER("SELECT EXISTS(SELECT RowID" &
                                                    " FROM employeeleave" &
                                                    " WHERE " &
                                                    " ('" & _from & "' IN (LeaveStartDate,LeaveEndDate) OR '" & _to & "' IN (LeaveStartDate,LeaveEndDate))" &
                                                    " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                                    " AND OrganizationID=" & orgztnID & ");")
                        End If
                    End If

                    If invalidleave = 1 Then
                        haserrinputleave = 1
                        dgvempleave.Item("elv_EndDate", dgvleaveRowindx).ErrorText = "     The employee has already a leave record on this date"
                    End If

                End If
            Catch ex As Exception
                haserrinputleave = 1
                dgvempleave.Item("elv_EndDate", dgvleaveRowindx).ErrorText = "     Invalid date value"
            Finally
                If thegetval <> "" Then
                    dgvempleave.Item("elv_EndDate", dgvleaveRowindx).Value = theretval
                    dtpendate.Value = Format(CDate(theretval), machineShortDateFormat)
                    dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

                    If theretval <> prev_elv_EndDate _
                        And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
                        listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
                    End If

                End If
            End Try
        End If

    End Sub

    Private Sub txtendate_Leave(sender As Object, e As EventArgs) Handles txtendate.Leave
    End Sub

    Private Sub txtreason_TextChanged(sender As Object, e As EventArgs) Handles txtreason.TextChanged
    End Sub

    Private Sub txtreason_GotFocus(sender As Object, e As EventArgs) Handles txtreason.GotFocus

        If dgvempleave.RowCount = 1 Then
        Else
            dgvempleave.Item("elv_Reason", dgvleaveRowindx).Selected = True
        End If

    End Sub

    Private Sub txtreason_Leave(sender As Object, e As EventArgs) Handles txtreason.Leave

        colNameleave = "elv_Reason"

        Dim thegetval = Trim(txtreason.Text)

        If dgvempleave.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempleave.Rows.Add()
                dgvleaveRowindx = dgvempleave.RowCount - 2
            End If
        Else
            If dgvempleave.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempleave.Rows.Add()
                    dgvleaveRowindx = dgvempleave.RowCount - 2
                End If
            Else
                dgvleaveRowindx = dgvempleave.CurrentRow.Index
            End If
        End If

        dgvempleave.Item("elv_Reason", dgvleaveRowindx).Value = thegetval
        txtreason.Text = thegetval
        dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

        If thegetval <> prev_elv_Reason _
                    And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
            listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
        End If

    End Sub

    Private Sub txtcomments_GotFocus(sender As Object, e As EventArgs) Handles txtcomments.GotFocus

        If dgvempleave.RowCount = 1 Then
        Else
            dgvempleave.Item("elv_Comment", dgvleaveRowindx).Selected = True
        End If

    End Sub

    Private Sub txtcomments_Leave(sender As Object, e As EventArgs) Handles txtcomments.Leave

        colNameleave = "elv_Comment"

        Dim thegetval = Trim(txtcomments.Text)

        If dgvempleave.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempleave.Rows.Add()
                dgvleaveRowindx = dgvempleave.RowCount - 2

            End If
        Else
            If dgvempleave.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempleave.Rows.Add()
                    dgvleaveRowindx = dgvempleave.RowCount - 2

                End If
            Else
                dgvleaveRowindx = dgvempleave.CurrentRow.Index
            End If
        End If

        dgvempleave.Item("elv_Comment", dgvleaveRowindx).Value = thegetval
        txtcomments.Text = thegetval
        dgvempleave.Item("elv_viewimage", dgvleaveRowindx).Value = "view this"

        If thegetval <> prev_elv_Comment _
                    And dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value <> Nothing Then
            listofEditRowleave.Add(dgvempleave.Item("elv_RowID", dgvleaveRowindx).Value)
        End If

    End Sub

    Private Sub btnleavtyp_Click(sender As Object, e As EventArgs) Handles btnleavtyp.Click

        With leavtyp
            .Show()
            .BringToFront()

            .lstbxleavtyp.Focus()
        End With

    End Sub

#End Region 'Leave

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

        param(0, 1) = If(emedrecord_EmployeeID = Nothing, DBNull.Value, CInt(emedrecord_EmployeeID))
        param(1, 1) = If(emedrecord_DateFrom = Nothing, DBNull.Value, Format(CDate(emedrecord_DateFrom), "yyyy-MM-dd"))
        param(2, 1) = If(emedrecord_DateTo = Nothing, DBNull.Value, Format(CDate(emedrecord_DateTo), "yyyy-MM-dd"))
        param(3, 1) = If(emedrecord_ProductID = Nothing, DBNull.Value, CInt(emedrecord_ProductID))
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

                .Parameters.AddWithValue("emedrec_RowID", If(emedrec_RowID = Nothing, DBNull.Value, emedrec_RowID))
                .Parameters.AddWithValue("emedrec_OrganizationID", orgztnID) 'orgztnID
                .Parameters.AddWithValue("emedrec_Created", _naw)
                .Parameters.AddWithValue("emedrec_LastUpd", _naw)
                .Parameters.AddWithValue("emedrec_CreatedBy", z_User)
                .Parameters.AddWithValue("emedrec_LastUpdBy", z_User)
                .Parameters.AddWithValue("emedrec_EmployeeID", If(emedrec_EmployeeID = Nothing, DBNull.Value, emedrec_EmployeeID))
                .Parameters.AddWithValue("emedrec_DateFrom", If(emedrec_DateFrom = Nothing, DBNull.Value, Format(CDate(emedrec_DateFrom), "yyyy-MM-dd")))
                .Parameters.AddWithValue("emedrec_DateTo", If(emedrec_DateTo = Nothing, DBNull.Value, Format(CDate(emedrec_DateTo), "yyyy-MM-dd")))
                .Parameters.AddWithValue("emedrec_ProductID", If(emedrec_ProductID = Nothing, DBNull.Value, emedrec_ProductID))
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

    Dim IsNew As Integer
    Dim view_IDDiscip As Integer

    Public categDiscipID As String

    Private Sub tbpDiscipAct_Click(sender As Object, e As EventArgs) Handles tbpDiscipAct.Click
    End Sub

    Dim empdiscippenal As New DataTable

    Sub tbpDiscipAct_Enter(sender As Object, e As EventArgs) Handles tbpDiscipAct.Enter

        tabpageText(tabIndx)

        tbpDiscipAct.Text = "DISCIPLINARY ACTION               "

        Label25.Text = "DISCIPLINARY ACTION"
        Static once As SByte = 0
        If once = 0 Then
            once = 1

            cboAction.ContextMenu = New ContextMenu

            dtpFrom.Value = Format(CDate(dbnow), machineShortDateFormat)
            dtpTo.Value = Format(CDate(dbnow), machineShortDateFormat)

            categDiscipID = EXECQUER("SELECT RowID FROM category WHERE OrganizationID=" & orgztnID & " AND CategoryName='" & "Employee Disciplinary" & "' LIMIT 1;")

            If Val(categDiscipID) = 0 Then
                categDiscipID = INSUPD_category(, "Employee Disciplinary")
            End If

            fillfindingcombobox()

            fillempdisciplinary()
            If Not dgvDisciplinaryList.Rows.Count = 0 Then
                fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
            End If

            'Employee Disciplinary Penalty
            empdiscippenal = retAsDatTbl("SELECT * FROM listofval WHERE Type='Employee Disciplinary Penalty' AND Active='Yes' ORDER BY OrderBy;")

            For Each drow As DataRow In empdiscippenal.Rows
                cboAction.Items.Add(Trim(drow("DisplayValue").ToString))
            Next

            view_IDDiscip = VIEW_privilege("Employee Disciplinary Action", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDDiscip)

            If formuserprivilege.Count = 0 Then

                btnNew.Visible = 0
                btnSave.Visible = 0
                btnDelete.Visible = 0

                dontUpdateDiscip = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        'ToolStripButton2.Visible = 0
                        btnNew.Visible = 0
                        btnSave.Visible = 0
                        btnDelete.Visible = 0
                        dontUpdateDiscip = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            btnNew.Visible = 0
                        Else
                            btnNew.Visible = 1
                        End If

                        If drow("Deleting").ToString = "N" Then
                            btnDelete.Visible = 0
                        Else
                            btnDelete.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateDiscip = 1
                        Else
                            dontUpdateDiscip = 0
                        End If

                    End If

                Next

            End If

        End If

        tabIndx = 6 'TabControl1.SelectedIndex

        If btnNew.Enabled = False Then
        Else
            dgvEmp_SelectionChanged(sender, e)
        End If

    End Sub

    Dim discipenalty = Nothing

    Private Sub cboAction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAction.SelectedIndexChanged
        discipenalty = Nothing

        For Each drow As DataRow In empdiscippenal.Rows
            If Trim(drow("DisplayValue").ToString) = Trim(cboAction.Text) Then
                discipenalty = drow("LIC").ToString
                Exit For
            End If
        Next
        'empdiscippenal
    End Sub

    Private Sub dtpFrom_ValueChanged(sender As Object, e As EventArgs) Handles dtpFrom.ValueChanged
    End Sub

    Private Sub dtpTo_ValueChanged(sender As Object, e As EventArgs) Handles dtpTo.ValueChanged
    End Sub

    Private Sub dgvDisciplinaryList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDisciplinaryList.CellClick

        If Not dgvDisciplinaryList.Rows.Count = 0 Then
            controltrue()
            fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
        End If

    End Sub

    Private Sub dgvDisciplinaryList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDisciplinaryList.CellContentClick
    End Sub

    Private Sub TabPage8_Leave(sender As Object, e As EventArgs) 'Handles tbpDiscipAct.Leave
        tbpDiscipAct.Text = "DISCIP"
    End Sub

    Private Sub lblAddFindingname_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddFindingname.LinkClicked

        Dim p As New ProdCtrlForm

        With p

            .Status.HeaderText = "Taxable Flag"

            .PartNo.HeaderText = "Item Name"

            .NameOfCategory = "Employee Disciplinary"

            Dim dgv_cols =
                .dgvproducts.Columns.Cast(Of DataGridViewColumn).Where(Function(dgcol) dgcol.Name <> "PartNo")

            For Each dcol In dgv_cols
                dcol.Visible = False
            Next

            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                fillfindingcombobox()

            End If

        End With

    End Sub

    Private Sub fillempdisciplinary()
        If Not dgvEmp.Rows.Count = 0 Then
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select *,ed.Comments 'ed_Comments' From employeedisciplinaryaction ed inner join product p on ed.FindingID = p.RowID " &
                                    "Where ed.OrganizationID = '" & z_OrganizationID & "' And ed.EmployeeID = '" & dgvEmp.CurrentRow.Cells("RowID").Value & "' Order by ed.RowID DESC")

            dgvDisciplinaryList.Rows.Clear()
            If dt.Rows.Count > 0 Then
                For Each drow As DataRow In dt.Rows
                    Dim n As Integer = dgvDisciplinaryList.Rows.Add()
                    With drow

                        dgvDisciplinaryList.Rows.Item(n).Cells(c_action.Index).Value = .Item("Action").ToString
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_datefrom.Index).Value = CDate(.Item("DateFrom")).ToString(machineShortDateFormat)
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_dateto.Index).Value = CDate(.Item("DateTo")).ToString(machineShortDateFormat)
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_FindingName.Index).Value = .Item("PartNo").ToString
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_comment.Index).Value = .Item("ed_Comments").ToString
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_desc.Index).Value = .Item("FindingDescription").ToString
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID").ToString
                    End With
                Next
            End If
        Else
            dgvDisciplinaryList.Rows.Clear()
        End If

    End Sub

    Private Function fillempdisciplinaryselected(ByVal dID As Integer) As Boolean

        If Not dgvDisciplinaryList.Rows.Count = 0 Then
            Dim dt As New DataTable

            dt = getDataTableForSQL("Select * From employeedisciplinaryaction ed inner join product p on ed.FindingID = p.RowID " &
                                    "Where ed.OrganizationID = '" & z_OrganizationID & "' And ed.RowID = '" & dID & "'")

            If dt.Rows.Count > 0 Then
                For Each drow As DataRow In dt.Rows

                    With drow

                        cboAction.Text = .Item("Action").ToString
                        dtpFrom.Text = CDate(.Item("DateFrom")).ToString(machineShortDateFormat)
                        dtpTo.Text = CDate(.Item("DateTo")).ToString(machineShortDateFormat)
                        txtDesc.Text = .Item("FindingDescription").ToString
                        txtdiscipcomment.Text = .Item("Comments").ToString
                        cmbFinding.Text = .Item("PartNo").ToString
                    End With
                Next
            End If
        End If
        Return True

    End Function

    Private Sub controltrue()
        cmbFinding.Enabled = True
        cboAction.Enabled = True
        txtdiscipcomment.Enabled = True
        txtDesc.Enabled = True
        dtpFrom.Enabled = True
        dtpTo.Enabled = True
    End Sub

    Private Sub controlfalseDiscipAct()
        cmbFinding.Enabled = 0
        cboAction.Enabled = 0
        txtdiscipcomment.Enabled = 0
        txtDesc.Enabled = 0
        dtpFrom.Enabled = 0
        dtpTo.Enabled = 0
    End Sub

    Private Sub controlclear()
        cmbFinding.SelectedIndex = -1
        cboAction.Text = ""
        txtdiscipcomment.Clear()
        txtDesc.Clear()
        dtpFrom.Value = Date.Now
        dtpTo.Value = Date.Now
    End Sub

    Sub fillfindingcombobox()
        Dim strQuery As String = "select partno from product Where OrganizationID = '" & z_OrganizationID & "' AND CategoryID='" & categDiscipID & "';"
        cmbFinding.Items.Clear()
        cmbFinding.Items.Add("-Please select one-")
        cmbFinding.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbFinding.SelectedIndex = 0
    End Sub

    Dim IsNewDiscip As SByte = 0

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        IsNewDiscip = 1
        btnNew.Enabled = False
        dgvEmp.Enabled = False
        controltrue()
        controlclear()

    End Sub

    Dim dontUpdateDiscip As SByte = 0

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If IsNewDiscip = 1 Then
            If dgvEmp.RowCount = 0 Then

                Exit Sub
            Else

                Dim fID As String = getStringItem("Select RowID From product where PartNo = '" & cmbFinding.Text & "' And organizationID = '" & z_OrganizationID & "'")
                Dim getfID As Integer = Val(fID)

                sp_employeedisciplinaryaction(z_datetime,
                                              z_User,
                                              z_datetime,
                                              z_OrganizationID,
                                              z_User,
                                              Trim(cboAction.Text),
                                              txtcomments.Text,
                                              txtDesc.Text,
                                              getfID, dgvEmp.CurrentRow.Cells("RowID").Value,
                                              dtpFrom.Value.ToString("yyyy-MM-dd"),
                                              dtpTo.Value.ToString("yyyy-MM-dd"),
                                              discipenalty)

                fillempdisciplinary()
                fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
                myBalloon("Successfully Save", "Saving...", lblforballoon, , -100)
            End If

            dgvEmp.Enabled = True
            btnNew.Enabled = True
            IsNewDiscip = 0
        Else
            If dontUpdateDiscip = 1 Then
                Exit Sub
            ElseIf dgvEmp.RowCount = 0 Then
                Exit Sub
            End If

            If dgvDisciplinaryList.RowCount <> 0 Then
                Dim fID As String = getStringItem("Select RowID From product where PartNo = '" & cmbFinding.Text & "' And organizationID = '" & z_OrganizationID & "'")
                Dim getfID As Integer = Val(fID)
                Dim penaltyUpd = If(discipenalty = Nothing, Nothing, ",Penalty='" & discipenalty & "'")

                DirectCommand("UPDATE employeedisciplinaryaction SET Action = '" & cboAction.Text & "', DateFrom = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' " &
                              ", DateTo = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "', FindingDescription = '" & txtDesc.Text & "', Comments = '" & txtdiscipcomment.Text & "', " &
                              "FindingID = '" & getfID & "'" & penaltyUpd & " Where RowID = '" & dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value & "';")

                fillempdisciplinary()
                fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
                myBalloon("Successfully Save", "Saving...", lblforballoon, , -100)
            End If
            IsNewDiscip = 0

        End If

        IsNewDiscip = 0

        fillempdisciplinary()
        controlfalseDiscipAct()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        Dim selected_rows =
            dgvDisciplinaryList.Rows.OfType(Of DataGridViewRow).Where(Function(dgvr) dgvr.Selected)

        Dim selected_rowids = New List(Of String)

        For Each dgvr As DataGridViewRow In selected_rows
            selected_rowids.Add(Convert.ToString(dgvr.Cells("c_rowid").Value))
        Next

        If selected_rows.Count > 0 Then

            Console.WriteLine("HERE")
            Console.WriteLine(String.Join(", ", selected_rowids.ToArray))

            Dim str_quer =
                String.Concat("DELETE FROM employeedisciplinaryaction",
                              " WHERE OrganizationID = ", orgztnID,
                              " AND RowID IN (", String.Join(", ", selected_rowids.ToArray), ");")

            Dim sql As New SQL(str_quer)
            sql.ExecuteQuery()

            If sql.HasError = False Then

                For Each dgvr As DataGridViewRow In selected_rows
                    dgvDisciplinaryList.Rows.Remove(dgvr)
                Next

            End If

        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            controlclear()

            If Not dgvDisciplinaryList.Rows.Count = 0 Then
                fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        End Try
        btnNew.Enabled = True
        dgvEmp.Enabled = True
        IsNewDiscip = 0
        controlfalseDiscipAct()

    End Sub

    Private Sub cboAction_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboAction.KeyPress
        e.Handled = True
    End Sub

#End Region 'Disciplinary Action

#Region "Educational Background"

    Dim is_New As Integer = 0
    Dim view_IDEduc As Integer

    Sub tbpEducBG_Enter(sender As Object, e As EventArgs) Handles tbpEducBG.Enter
        tabpageText(tabIndx)

        tbpEducBG.Text = "EDUCATIONAL BACKGROUND               "

        Label25.Text = "EDUCATIONAL BACKGROUND"
        Static once As SByte = 0
        If once = 0 Then
            once = 1
            cmbEducType.Text = "College"

            DateTimePicker2.Value = Format(CDate(dbnow), machineShortDateFormat)
            DateTimePicker1.Value = Format(CDate(dbnow), machineShortDateFormat)

            view_IDEduc = VIEW_privilege("Employee Educational Background", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDEduc)

            If formuserprivilege.Count = 0 Then

                btnNewEduc.Visible = 0
                btnSaveEduc.Visible = 0
                btnDeleteEduc.Visible = 0

                dontUpdateEduc = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        'ToolStripButton2.Visible = 0
                        btnNewEduc.Visible = 0
                        btnSaveEduc.Visible = 0
                        btnDeleteEduc.Visible = 0

                        dontUpdateEduc = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            btnNewEduc.Visible = 0
                        Else
                            btnNewEduc.Visible = 1
                        End If

                        If drow("Deleting").ToString = "N" Then
                            btnDeleteEduc.Visible = 0
                        Else
                            btnDeleteEduc.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateEduc = 1
                        Else
                            dontUpdateEduc = 0
                        End If
                    End If
                Next
            End If
        End If

        tabIndx = 7 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)
    End Sub

    Private Sub TabPage9_Leave(sender As Object, e As EventArgs) 'Handles tbpEducBG.Leave
        tbpEducBG.Text = "EDUC"
    End Sub

    Private Sub filleducback()
        If dgvEmp.Rows.Count = 0 Then
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * from employeeeducation ed inner join employee ee on ed.EmployeeID = ee.RowID " &
                                    "where ee.OrganizationID = '" & z_OrganizationID & "' and ee.EmployeeID = '" & dgvEmp.CurrentRow.Cells(c_empID.Index).Value & "'")

            dgvEducback.Rows.Clear()
            For Each drow As DataRow In dt.Rows
                Dim n As Integer = dgvEducback.Rows.Add()
                With drow
                    dgvEducback.Rows.Item(n).Cells(c_EmplyeeID.Index).Value = .Item("EmployeeID").ToString
                    dgvEducback.Rows.Item(n).Cells(c_name.Index).Value = .Item("Name").ToString
                    dgvEducback.Rows.Item(n).Cells(c_school.Index).Value = .Item("School").ToString
                    dgvEducback.Rows.Item(n).Cells(c_degree.Index).Value = .Item("Degree").ToString
                    dgvEducback.Rows.Item(n).Cells(c_course.Index).Value = .Item("Course").ToString
                    dgvEducback.Rows.Item(n).Cells(c_minor.Index).Value = .Item("Minor").ToString
                    dgvEducback.Rows.Item(n).Cells(c_EducationalType.Index).Value = .Item("EducationType").ToString
                    dgvEducback.Rows.Item(n).Cells(c_datefrom.Index).Value = CDate(.Item("DateFrom")).ToString(machineShortDateFormat)
                    dgvEducback.Rows.Item(n).Cells(c_dateto.Index).Value = CDate(.Item("DateTo")).ToString(machineShortDateFormat)
                    dgvEducback.Rows.Item(n).Cells(c_Remarks.Index).Value = .Item("Remarks").ToString
                    dgvEducback.Rows.Item(n).Cells(c_RowID1.Index).Value = .Item("RowID").ToString
                End With
            Next
        End If
    End Sub

    Private Sub cleartextbox()
        txtCourse.Clear()
        txtDegree.Clear()
        cmbEducType.SelectedIndex = -1

        txtMinor.Clear()

        txtRemarks.Clear()
        txtSchool.Clear()

    End Sub

    Dim is_NewEducBG As SByte = 1

    Private Sub btnNewEduc_Click(sender As Object, e As EventArgs) Handles btnNewEduc.Click
        btnNewEduc.Enabled = False
        cleartextbox()
        btnDelete.Enabled = False
        dgvEducback.Enabled = False

        is_NewEducBG = 1
        cmbEducType.Focus()
    End Sub

    Dim dontUpdateEduc As SByte = 0

    Private Sub btnSaveEduc_Click(sender As Object, e As EventArgs) Handles btnSaveEduc.Click
        If dgvEmp.RowCount <> 0 Then

            If is_NewEducBG = 1 Then
            Else

            End If
            If btnNewEduc.Enabled = False Then

                SP_EducBackGround(z_datetime, z_User, z_datetime, z_User, z_OrganizationID, dgvEmp.CurrentRow.Cells("RowID").Value,
                                  dtpFrom.Value.ToString(machineShortDateFormat), dtpTo.Value.ToString(machineShortDateFormat),
                                  txtCourse.Text, txtSchool.Text, txtDegree.Text, txtMinor.Text, cmbEducType.Text, txtRemarks.Text)

                myBalloon("Successfully Save", "Saved", lblforballoon, , -100)
                filleducback()

                is_NewEducBG = 0
                btnNewEduc.Enabled = True
                dgvEducback.Enabled = True
            Else
                If dontUpdateEduc = 1 Then
                    Exit Sub
                End If
                If dgvEducback.RowCount <> 0 Then
                    SP_employeeeducationUpdate(dtpFrom.Value.ToString(machineShortDateFormat), dtpTo.Value.ToString(machineShortDateFormat),
                                  txtCourse.Text, txtSchool.Text, txtDegree.Text, txtMinor.Text, cmbEducType.Text, txtRemarks.Text,
                                  dgvEducback.CurrentRow.Cells(c_RowID1.Index).Value)

                    myBalloon("Successfully Save", "Saved", lblforballoon, , -100)
                    filleducback()
                End If

                btnNewEduc.Enabled = True
                dgvEducback.Enabled = True
                is_NewEducBG = 0
            End If

            is_NewEducBG = 0
            fillselectRowID()
            'fillselecteducback()
        End If
    End Sub

    Private Sub btnCancelEduc_Click(sender As Object, e As EventArgs) Handles btnCancelEduc.Click
        btnNewEduc.Enabled = True
        dgvEducback.Enabled = True
        is_NewEducBG = 0
    End Sub

    Private Sub dgvEducback_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEducback.CellClick
        btnSaveEduc.Enabled = True
        btnDeleteEduc.Enabled = True
        fillselecteducback()
    End Sub

    Private Sub fillselectRowID()

        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * from employeeeducation ed inner join employee ee on ed.EmployeeID = ee.RowID " &
                                "where ee.OrganizationID = '" & z_OrganizationID & "' And ee.RowID = '" & dgvEmp.CurrentRow.Cells("RowID").Value & "'")

        dgvEducback.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvEducback.Rows.Add()
            With drow
                dgvEducback.Rows.Item(n).Cells(c_EmplyeeID.Index).Value = .Item("EmployeeID").ToString
                dgvEducback.Rows.Item(n).Cells(c_name.Index).Value = .Item("Name").ToString
                dgvEducback.Rows.Item(n).Cells(c_school.Index).Value = .Item("School").ToString
                dgvEducback.Rows.Item(n).Cells(c_degree.Index).Value = .Item("Degree").ToString
                dgvEducback.Rows.Item(n).Cells(c_course.Index).Value = .Item("Course").ToString
                dgvEducback.Rows.Item(n).Cells(c_minor.Index).Value = .Item("Minor").ToString
                dgvEducback.Rows.Item(n).Cells(c_EducationalType.Index).Value = .Item("EducationType").ToString
                dgvEducback.Rows.Item(n).Cells(DataGridViewTextBoxColumn108.Index).Value = CDate(.Item("DateFrom")).ToString(machineShortDateFormat)
                dgvEducback.Rows.Item(n).Cells(DataGridViewTextBoxColumn109.Index).Value = CDate(.Item("DateTo")).ToString(machineShortDateFormat)
                dgvEducback.Rows.Item(n).Cells(c_Remarks.Index).Value = .Item("Remarks").ToString
                dgvEducback.Rows.Item(n).Cells(c_RowID1.Index).Value = .Item("RowID").ToString
            End With
        Next
    End Sub

    Private Sub fillselecteducback()
        If dgvEducback.Rows.Count = 0 Then
            cleartextbox()
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * from employeeeducation ed inner join employee ee on ed.EmployeeID = ee.RowID " &
                                    "where ee.OrganizationID = '" & z_OrganizationID & "' And ed.RowID = '" & dgvEducback.CurrentRow.Cells(c_RowID1.Index).Value & "'")
            cleartextbox()
            For Each drow As DataRow In dt.Rows
                With drow

                    txtSchool.Text = .Item("School").ToString
                    txtDegree.Text = .Item("Degree").ToString
                    txtCourse.Text = .Item("Course").ToString
                    txtMinor.Text = .Item("Minor").ToString
                    cmbEducType.Text = .Item("EducationType").ToString
                    dtpFrom.Value = CDate(.Item("DateFrom")).ToString(machineShortDateFormat)
                    dtpTo.Value = CDate(.Item("DateTo")).ToString(machineShortDateFormat)
                    txtRemarks.Text = .Item("Remarks").ToString

                End With
            Next
        End If
    End Sub

    Private Sub cmbEducType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEducType.SelectedIndexChanged
        lblSchool.Text = cmbEducType.Text
    End Sub

#End Region 'Educational Background

#Region "Previous Employer"

    Dim IsNewPrevEmp As Integer = 0

    Dim view_IDPrevEmp As Integer

    Sub tbpPrevEmp_Enter(sender As Object, e As EventArgs) Handles tbpPrevEmp.Enter

        tabpageText(tabIndx)

        tbpPrevEmp.Text = "PREVIOUS EMPLOYER               "
        Label25.Text = "PREVIOUS EMPLOYER"

        Static once As SByte = 0

        If once = 0 Then
            once = 1
            view_IDPrevEmp = VIEW_privilege("Employee Previous Employer", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDPrevEmp)

            If formuserprivilege.Count = 0 Then

                btnNewPrevEmp.Visible = 0
                btnSavePrevEmp.Visible = 0
                btnDelPrevEmp.Visible = 0

                dontUpdatePrevEmp = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        btnNewPrevEmp.Visible = 0
                        btnSavePrevEmp.Visible = 0
                        btnDelPrevEmp.Visible = 0

                        dontUpdatePrevEmp = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            btnNewPrevEmp.Visible = 0
                        Else
                            btnNewPrevEmp.Visible = 1
                        End If

                        If drow("Deleting").ToString = "N" Then
                            btnDelPrevEmp.Visible = 0
                        Else
                            btnDelPrevEmp.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdatePrevEmp = 1
                        Else
                            dontUpdatePrevEmp = 0
                        End If
                    End If
                Next
            End If
        End If

        tabIndx = 8 'TabControl1.SelectedIndex
        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub TabPage10_Leave(sender As Object, e As EventArgs) 'Handles tbpPrevEmp.Leave
        tbpPrevEmp.Text = "PREV EMP"
    End Sub

    Private Sub btnNewPrevEmp_Click(sender As Object, e As EventArgs) Handles btnNewPrevEmp.Click
        txtCompanyName.Focus()
        IsNewPrevEmp = 1
        cleartextboxPrevEmp()
        btnSavePrevEmp.Enabled = True
        btnNewPrevEmp.Enabled = False
        grpDetails.Enabled = True
        dgvListCompany.Enabled = False
        dgvEmp.Enabled = False

        IsNewPrevEmp = 1
    End Sub

    Dim dontUpdatePrevEmp As SByte = 0

    Private Sub btnSavePrevEmp_Click(sender As Object, e As EventArgs) Handles btnSavePrevEmp.Click

        If dgvEmp.RowCount = 0 Then
            btnNewPrevEmp.Enabled = True
            IsNewPrevEmp = 0
            Exit Sub
        End If

        Dim dateExpTo = Format(CDate(dtpExpto.Value), "yyyy-MM-dd")

        If btnNewPrevEmp.Enabled = False Then 'IsNewPrevEmp = 1
            Z_ErrorProvider.Dispose()

            If txtCompanyName.Text = Nothing Or txtContactName.Text = Nothing Or txtMainPhone.Text = Nothing _
                Or txtCompAddr.Text = Nothing Or txtEmailAdd.Text = Nothing Then
                If Not SetWarningIfEmpty(txtCompanyName) And SetWarningIfEmpty(txtContactName) _
                    And SetWarningIfEmpty(txtCompAddr) And SetWarningIfEmpty(txtEmailAdd) _
                     And SetWarningIfEmpty(txtMainPhone) Then

                End If
            Else

                SP_employeepreviousemployer(txtCompanyName.Text, txtTradeName.Text, z_OrganizationID, txtMainPhone.Text, txtFaxNo.Text, txtJobTitle.Text,
                                       Format(CDate(dtpExfromto.Value), "yyyy-MM-dd") & "@" & Trim(dateExpTo), txtCompAddr.Text, txtContactName.Text, txtEmailAdd.Text, txtAltEmailAdd.Text, txtAltPhone.Text,
                                      txtUrl.Text, Trim(txtTinNo.Text), txtJobFunction.Text, z_datetime, z_User, z_datetime, z_User, txtOrganizationType.Text,
                                      dgvEmp.CurrentRow.Cells("RowID").Value)
                fillemployerlist()

                myBalloon("Successfully Save", "Saved", lblforballoon, , -100)
                dgvListCompany.Enabled = True
                btnNewPrevEmp.Enabled = True
                dgvEmp.Enabled = True
                IsNewPrevEmp = 0
            End If
        Else
            If dontUpdatePrevEmp = 1 Then
                Exit Sub
            End If
            Z_ErrorProvider.Dispose()
            If txtCompanyName.Text = Nothing Or txtContactName.Text = Nothing Or txtMainPhone.Text = Nothing _
                Or txtCompAddr.Text = Nothing Or txtEmailAdd.Text = Nothing Then
                If Not SetWarningIfEmpty(txtCompanyName) And SetWarningIfEmpty(txtContactName) _
                    And SetWarningIfEmpty(txtCompAddr) And SetWarningIfEmpty(txtEmailAdd) _
                     And SetWarningIfEmpty(txtMainPhone) Then

                End If
            Else
                'dtpExpto
                SP_EmployeePreviousEmployerUpdate(txtCompanyName.Text, txtTradeName.Text, txtMainPhone.Text, txtFaxNo.Text, txtJobTitle.Text,
                                Format(CDate(dtpExfromto.Value), "yyyy-MM-dd") & "@" & Trim(dateExpTo), txtCompAddr.Text, txtContactName.Text, txtEmailAdd.Text, txtAltEmailAdd.Text, txtAltPhone.Text,
                               txtUrl.Text, Trim(txtTinNo.Text), txtJobFunction.Text, txtOrganizationType.Text,
                               dgvListCompany.CurrentRow.Cells(c_rowidPrevEmp.Index).Value)
                fillemployerlist()
                btnNewPrevEmp.Enabled = True
                dgvEmp.Enabled = True
                myBalloon("Successfully Save", "Saved", lblforballoon, , -100)
            End If

        End If

        SetWarningIfEmpty(txtCompanyName, "Hide this error provider")
        SetWarningIfEmpty(txtContactName, "Hide this error provider")

        SetWarningIfEmpty(txtCompAddr, "Hide this error provider")
        SetWarningIfEmpty(txtEmailAdd, "Hide this error provider")

    End Sub

    Private Sub btnDelPrevEmp_Click(sender As Object, e As EventArgs) Handles btnDelPrevEmp.Click
        If MsgBox("Are you sure you want to remove this employer " & txtCompanyName.Text & "?", MsgBoxStyle.YesNo, "Removing...") = MsgBoxResult.Yes Then
            DirectCommand("Delete From employeepreviousemployer where RowID = '" & dgvListCompany.CurrentRow.Cells(c_rowidPrevEmp.Index).Value & "'")
            'fillemployerlist()
            btnDelPrevEmp.Enabled = False
            btnNewPrevEmp.Enabled = True
            dgvEmp.Enabled = True
        End If
    End Sub

    Private Sub btnCancelPrevEmp_Click(sender As Object, e As EventArgs) Handles btnCancelPrevEmp.Click
        cleartextboxPrevEmp()

        btnDelPrevEmp.Enabled = False
        dgvListCompany.Enabled = True
        btnNewPrevEmp.Enabled = True
        dgvEmp.Enabled = True
        dgvEmp.Focus()
        IsNewPrevEmp = 0
    End Sub

    Sub cleartextboxPrevEmp()
        txtAltEmailAdd.Clear()
        txtAltPhone.Clear()
        txtCompAddr.Clear()
        txtCompanyName.Clear()
        txtContactName.Clear()
        txtEmailAdd.Clear()
        txtFaxNo.Clear()
        txtJobFunction.Clear()
        txtJobTitle.Clear()
        txtMainPhone.Clear()
        txtOrganizationType.Clear()
        txtTinNo.Clear()
        txtTradeName.Clear()
        txtUrl.Clear()
        txtExfromto.Clear()

    End Sub

    Private Sub dgvListCompany_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvListCompany.CellContentClick

    End Sub

    Private Sub dgvListCompany_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvListCompany.CellClick
        fillemployerOneByone()
        btnSave.Enabled = True
        btnDelete.Enabled = True
    End Sub

    Private Sub dgvEmplist_CellClick1(sender As Object, e As DataGridViewCellEventArgs) ' Handles dgvEmp.CellClick
        fillemployerOneByone()
    End Sub

    Private Sub fillemployerlist()

        If dgvEmp.Rows.Count = 0 Then
            dgvListCompany.Rows.Clear()
            cleartextboxPrevEmp()
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * From employeepreviousemployer where EmployeeID = '" & dgvEmp.CurrentRow.Cells("RowID").Value & "' " &
                                    "And OrganizationID = '" & z_OrganizationID & "' ")

            dgvListCompany.Rows.Clear()
            cleartextboxPrevEmp()

            For Each drow As DataRow In dt.Rows
                Dim n As Integer = dgvListCompany.Rows.Add()

                With drow

                    dgvListCompany.Item(c_compname.Index, n).Value = .Item("Name").ToString
                    dgvListCompany.Item(c_trade.Index, n).Value = .Item("TradeName").ToString
                    dgvListCompany.Item(c_contname.Index, n).Value = .Item("ContactName").ToString
                    dgvListCompany.Item(c_mainphone.Index, n).Value = .Item("MainPHone").ToString
                    dgvListCompany.Item(c_altphone.Index, n).Value = .Item("AltPhone").ToString
                    dgvListCompany.Item(c_faxno.Index, n).Value = .Item("FaxNumber").ToString
                    dgvListCompany.Item(c_emailaddr.Index, n).Value = .Item("EmailAddress").ToString
                    dgvListCompany.Item(c_altemailaddr.Index, n).Value = .Item("AltEmailAddress").ToString
                    dgvListCompany.Item(c_url.Index, n).Value = .Item("URL").ToString
                    dgvListCompany.Item(c_tinno.Index, n).Value = .Item("TINNo").ToString
                    dgvListCompany.Item(c_jobtitle.Index, n).Value = .Item("JobTitle").ToString
                    dgvListCompany.Item(c_jobfunction.Index, n).Value = .Item("JobFunction").ToString
                    dgvListCompany.Item(c_orgtype.Index, n).Value = .Item("OrganizationType").ToString

                    If IsDBNull(.Item("ExperienceFromTo")) Then
                        dgvListCompany.Item(c_experience.Index, n).Value = Nothing
                    Else
                        If Trim(.Item("ExperienceFromTo").ToString) <> "" Then
                            Dim date_From = EXECQUER("SELECT SUBSTRING_INDEX('" & .Item("ExperienceFromTo").ToString & "', '@', 1);")

                            Dim date_To = EXECQUER("SELECT SUBSTRING_INDEX('" & .Item("ExperienceFromTo").ToString & "', '@', -1);")

                            dgvListCompany.Item(c_experience.Index, n).Value = Format(CDate(date_From), machineShortDateFormat) '.Item("ExperienceFromTo")
                            dgvListCompany.Item(c_expdateto.Index, n).Value = Format(CDate(date_To), machineShortDateFormat)

                        End If

                    End If

                    dgvListCompany.Item(c_compaddr.Index, n).Value = .Item("BusinessAddress").ToString
                    dgvListCompany.Item(c_rowidPrevEmp.Index, n).Value = .Item("RowID").ToString

                End With
            Next
        End If
    End Sub

    Private Sub fillemployerOneByone()
        If dgvListCompany.Rows.Count = 0 Then
            dtpExfromto.Value = Format(CDate(dbnow), machineShortDateFormat)
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * From employeepreviousemployer where RowID = '" & dgvListCompany.CurrentRow.Cells(c_rowidPrevEmp.Index).Value & "' " &
                                    "And OrganizationID = '" & z_OrganizationID & "'")
            If dt.Rows.Count > 0 Then
                cleartextboxPrevEmp()
                For Each drow As DataRow In dt.Rows
                    With drow

                        txtCompanyName.Text = .Item("Name").ToString
                        txtTradeName.Text = .Item("TradeName").ToString
                        txtContactName.Text = .Item("ContactName").ToString
                        txtMainPhone.Text = .Item("MainPhone").ToString
                        txtAltPhone.Text = .Item("AltPhone").ToString
                        txtFaxNo.Text = .Item("FaxNumber").ToString
                        txtEmailAdd.Text = .Item("EmailAddress").ToString
                        txtAltEmailAdd.Text = .Item("AltEmailAddress").ToString
                        txtUrl.Text = .Item("URL").ToString
                        txtTinNo.Text = .Item("TINNo").ToString
                        txtJobTitle.Text = .Item("JobTitle").ToString
                        txtJobFunction.Text = .Item("JobFunction").ToString
                        txtOrganizationType.Text = .Item("OrganizationType").ToString
                        txtCompAddr.Text = .Item("BusinessAddress").ToString

                        If .Item("ExperienceFromTo").ToString = "" Then
                            dtpExfromto.Value = Format(CDate(dbnow), machineShortDateFormat)
                        Else
                            Dim date_From = EXECQUER("SELECT SUBSTRING_INDEX('" & .Item("ExperienceFromTo").ToString & "', '@', 1);")

                            dtpExfromto.Value = Format(CDate(date_From), machineShortDateFormat)
                        End If

                        If .Item("ExperienceFromTo").ToString = "" Then
                            dtpExpto.Value = Format(CDate(dbnow), machineShortDateFormat)
                        Else
                            Dim date_To = EXECQUER("SELECT SUBSTRING_INDEX('" & .Item("ExperienceFromTo").ToString & "', '@', -1);")

                            dtpExpto.Value = Format(CDate(date_To), machineShortDateFormat) '.Item("ExperienceFromTo")
                        End If

                    End With
                Next
            Else
                cleartextboxPrevEmp()
            End If
        End If
    End Sub

#End Region 'Previous Employer

#Region "Promotion"

    Dim IsNewPromot As SByte = 0
    Dim view_IDPromot As Integer
    Dim rowidPromot As Integer

    Sub tbpPromotion_Enter(sender As Object, e As EventArgs) Handles tbpPromotion.Enter

        tabpageText(tabIndx)

        tbpPromotion.Text = "PROMOTION               "

        Label25.Text = "PROMOTION"

        Static once As SByte = 0

        If once = 0 Then
            once = 1

            txtbasicpay.ContextMenu = New ContextMenu

            cmbto.ContextMenu = New ContextMenu

            dtpEffectivityDate.Value = Format(CDate(dbnow), machineShortDateFormat)

            cmbSalaryChanged.Visible = False

            Dim n_SQLQueryToDatatable As _
                New SQLQueryToDatatable("SELECT '' AS RowID, '' AS PositionName" &
                                        " UNION SELECT pos.RowID,pos.PositionName" &
                                        " FROM position pos" &
                                        " LEFT JOIN employee e ON e.PositionID!=pos.RowID AND e.RowID='" & sameEmpID & "'" &
                                        " WHERE pos.OrganizationID='" & orgztnID & "';")

            With n_SQLQueryToDatatable

                cmbto.ValueMember = .ResultTable.Columns(0).ColumnName

                cmbto.DisplayMember = .ResultTable.Columns(1).ColumnName

                cmbto.DataSource = .ResultTable

            End With

            fillemplyeelistselected()
            fillPositionFrom()

            view_IDPromot = VIEW_privilege("Employee Promotion", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDPromot)

            If formuserprivilege.Count = 0 Then

                btnNewPromot.Visible = 0
                btnSavePromot.Visible = 0
                btnDelPromot.Visible = 0

                dontUpdatePromot = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        btnNewPromot.Visible = 0
                        btnSavePromot.Visible = 0
                        btnDelPromot.Visible = 0

                        dontUpdatePromot = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            btnNewPromot.Visible = 0
                        Else
                            btnNewPromot.Visible = 1
                        End If

                        If drow("Deleting").ToString = "N" Then
                            btnDelPromot.Visible = 0
                        Else
                            btnDelPromot.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdatePromot = 1
                        Else
                            dontUpdatePromot = 0
                        End If
                    End If
                Next
            End If
        End If
        tabIndx = 9 'TabControl1.SelectedIndex

        If btnNewPromot.Enabled = True Then
            dgvEmp_SelectionChanged(sender, e)
        End If

    End Sub

    Private Sub TabPage11_Leave(sender As Object, e As EventArgs) 'Handles tbpPromotion.Leave
        tbpPromotion.Text = "PROMOT"
    End Sub

    Private Sub controltruePromot()
        cmbfrom.Enabled = True
        cmbto.Enabled = True
        cmbflg.Enabled = True
        dtpEffectivityDate.Enabled = True
        txtbasicpay.Enabled = True
        txtReasonPromot.Enabled = True
    End Sub

    Private Sub controlclearPromot()
        cmbfrom.SelectedIndex = -1
        cmbto.SelectedIndex = -1
        cmbto.Text = ""
        cmbflg.SelectedIndex = -1
        dtpEffectivityDate.Value = Date.Now
        txtbasicpay.Clear()
        txtReasonPromot.Text = ""
    End Sub

    Private Sub controlfalsePromot()
        cmbfrom.Enabled = False
        cmbto.Enabled = False
        cmbflg.Enabled = False
        dtpEffectivityDate.Enabled = False
        txtbasicpay.Enabled = False
        txtReasonPromot.Enabled = False
    End Sub

    Private Sub btnNewPromot_EnabledChanged(sender As Object, e As EventArgs) Handles btnNewPromot.EnabledChanged
        dgvEmp.Enabled = btnNewPromot.Enabled
    End Sub

    Private Sub MakeNewPromotion(sender As Object, e As EventArgs) Handles btnNewPromot.Click

        btnNewPromot.Enabled = False

        dgvEmp.Enabled = False

        controlclearPromot()

        controltruePromot()

        IsNewPromot = 1

        dgvPromotionList_CellClick(sender, New DataGridViewCellEventArgs(c_promotRowID.Index, 0))

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("SELECT '' AS RowID, '' AS PositionName" &
                                    " UNION SELECT pos.RowID,pos.PositionName" &
                                    " FROM position pos" &
                                    " LEFT JOIN employee e ON e.PositionID!=pos.RowID AND e.RowID='" & sameEmpID & "'" &
                                    " WHERE pos.OrganizationID='" & orgztnID & "';")

        With n_SQLQueryToDatatable

            cmbto.ValueMember = .ResultTable.Columns(0).ColumnName

            cmbto.DisplayMember = .ResultTable.Columns(1).ColumnName

            cmbto.DataSource = .ResultTable

        End With

        n_SQLQueryToDatatable =
            New SQLQueryToDatatable("SELECT es.*,CURDATE() AS Curdate" &
                                    ",(CURDATE() > es.EffectiveDateFrom) AS CurrDateIsGreater" &
                                    ",pos.PositionName" &
                                    " FROM employeesalary es" &
                                    " INNER JOIN employee e ON e.RowID=es.EmployeeID" &
                                    " LEFT JOIN position pos ON pos.RowID=e.PositionID" &
                                    " WHERE es.OrganizationID='" & orgztnID & "'" &
                                    " AND es.EmployeeID='" & sameEmpID & "'" &
                                    " ORDER BY es.EffectiveDateFrom DESC" &
                                    " LIMIT 1;")

        For Each drow As DataRow In n_SQLQueryToDatatable.ResultTable.Rows

            Dim ii = DateDiff(DateInterval.Day, CDate(drow("EffectiveDateFrom")), CDate(drow("Curdate")))

            Dim min_date As Date

            If ii <= 0 Then
                If drow("CurrDateIsGreater") > "1" Then
                    min_date = DateAdd(DateInterval.Day, 1, CDate(drow("Curdate")))
                Else
                    min_date = DateAdd(DateInterval.Day, 1, CDate(drow("EffectiveDateFrom")))
                End If
            Else
                If drow("CurrDateIsGreater") > "1" Then
                    min_date = DateAdd(DateInterval.Day, 1, CDate(drow("Curdate")))
                Else
                    min_date = DateAdd(DateInterval.Day, 1, CDate(drow("EffectiveDateFrom")))
                End If

            End If

            dtpEffectivityDate.MinDate = min_date

            txtpositfrompromot.Text = If(IsDBNull(drow("PositionName")), "", drow("PositionName"))

            Exit For

        Next

        txtempcurrbasicpay.Text =
            New ExecuteQuery("SELECT Salary" &
                                " FROM employeesalary" &
                                " WHERE OrganizationID='" & orgztnID & "'" &
                                " AND EmployeeID='" & sameEmpID & "'" &
                                " ORDER BY EffectiveDateFrom DESC" &
                                " LIMIT 1;").Result

    End Sub

    Private Sub btnNewPromot_Click(sender As Object, e As EventArgs) 'Handles btnNewPromot.Click
        RemoveHandler cmbflg.SelectedIndexChanged, AddressOf cmbflg_SelectedIndexChanged

        btnNewPromot.Enabled = False
        dgvEmp.Enabled = False
        controlclearPromot()
        controltruePromot()

        dtpEffectivityDate.Value = Now

        dtpEffectivityDate.MinDate = Format(CDate(dbnow), machineShortDateFormat)

        IsNewPromot = 1

        dgvPromotionList_CellClick(sender, New DataGridViewCellEventArgs(c_promotRowID.Index, 0))

        If dgvEmp.RowCount <> 0 Then
            txtpositfrompromot.Text = dgvEmp.CurrentRow.Cells("Column8").Value
            Try
                cmbto.Items.Clear()

                fillCombobox("SELECT PositionName from Position Where OrganizationID = '" & orgztnID &
                             "' And RowID NOT IN (SELECT PositionID FROM employee WHERE OrganizationID=" & orgztnID &
                             " AND PositionID IS NOT NULL GROUP BY PositionID" &
                             " UNION SELECT PositionID FROM user WHERE OrganizationID='" & orgztnID &
                             "' GROUP BY PositionID);",
                             cmbto)
            Catch ex As Exception
                MsgBox(ex.Message & vbNewLine & "Error in 'Position to'.")
            End Try

            Dim getsalarnearnow = EXECQUER("SELECT COALESCE(Salary,0)" &
                                   " FROM employeesalary" &
                                   " WHERE EmployeeID='" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                   "' AND OrganizationID='" & orgztnID &
                                   "' AND EffectiveDateTo IS NULL" &
                                   " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveDateFrom)" &
                                   " LIMIT 1;")

            txtempcurrbasicpay.Text = Val(getsalarnearnow)

            Dim EffDateBeforCurrent As New DataTable

            EffDateBeforCurrent = retAsDatTbl("SELECT EffectiveDateFrom" &
                                   ",EffectiveDateTo" &
                                   " FROM employeesalary" &
                                   " WHERE EmployeeID='" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                   "' AND OrganizationID='" & orgztnID &
                                   "' ORDER BY EffectiveDateFrom DESC;")

            If EffDateBeforCurrent.Rows.Count = 1 Then

                Dim empEffDateFrom = Format(CDate(EffDateBeforCurrent.Rows(0)("EffectiveDateFrom").ToString), "yyyy-MM-dd")

                Dim dbCurrDate = EXECQUER("SELECT DATE_FORMAT(CURRENT_DATE(),'%Y-%m-%d');")

                If CDate(dbCurrDate) = CDate(empEffDateFrom) Then
                    Dim thevaliddate = CDate(dbCurrDate).AddDays(2)

                    dtpEffectivityDate.MinDate = Format(CDate(thevaliddate), machineShortDateFormat)

                ElseIf CDate(dbCurrDate) > CDate(empEffDateFrom) Then

                    Dim dateDiffer = DateDiff(DateInterval.Day, CDate(empEffDateFrom), CDate(dbCurrDate))

                    Dim thevaliddate = Nothing

                    If Val(dateDiffer) <= 2 Then
                        thevaliddate = CDate(dbCurrDate).AddDays(2)
                    Else
                        thevaliddate = CDate(dbCurrDate)
                    End If

                    dtpEffectivityDate.MinDate = Format(CDate(thevaliddate), machineShortDateFormat)

                ElseIf CDate(dbCurrDate) < CDate(empEffDateFrom) Then

                    Dim dateDiffer = DateDiff(DateInterval.Day, CDate(dbCurrDate), CDate(empEffDateFrom))

                    Dim thevaliddate = Nothing

                    If Val(dateDiffer) <= 2 Then
                        thevaliddate = CDate(empEffDateFrom).AddDays(2)
                    Else
                        thevaliddate = CDate(empEffDateFrom)
                    End If

                    dtpEffectivityDate.MinDate = Format(CDate(thevaliddate), machineShortDateFormat)

                End If

            ElseIf EffDateBeforCurrent.Rows.Count >= 2 Then

                Dim dateDiffer = DateDiff(DateInterval.Day,
                                          CDate(EffDateBeforCurrent.Rows(1)("EffectiveDateTo").ToString),
                                          CDate(EffDateBeforCurrent.Rows(0)("EffectiveDateFrom").ToString))

                Dim thevaliddate = Nothing

                If Val(dateDiffer) <= 2 Then
                    thevaliddate = CDate(EffDateBeforCurrent.Rows(0)("EffectiveDateFrom")).AddDays(2)
                Else
                    thevaliddate = CDate(EffDateBeforCurrent.Rows(0)("EffectiveDateFrom"))
                End If

                dtpEffectivityDate.MinDate = Format(CDate(thevaliddate), machineShortDateFormat)

            End If

        End If

        cmbto.Text = ""
        cmbflg.SelectedIndex = -1
        cmbflg.Text = ""

        Label142.Text = "Current salary"

    End Sub

    Private Sub fillemplyeelistselected()

        If dgvEmp.RowCount <> 0 Then
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select concat(COALESCE(Lastname, ' '),' ', COALESCE(Firstname, ' '), ' ', COALESCE(MiddleName, ' ')) as name, EmployeeID, rowid from employee where organizationID = '" & z_OrganizationID & "' And rowid = '" & dgvEmp.CurrentRow.Cells("RowID").Value & "'")

            For Each drow As DataRow In dt.Rows

                With drow

                    txtEmpID.Text = .Item("EmployeeID").ToString

                    rowidPromot = .Item("RowID").ToString
                End With
            Next
        End If
    End Sub

    Private Sub fillpromotions()
        dgvPromotionList.Rows.Clear()
        If Not dgvEmp.Rows.Count = 0 Then
            Dim dt As New DataTable
            dt = getDataTableForSQL("select concat(COALESCE(e.Lastname, ' '),' ', COALESCE(e.Firstname, ' '), ' ', COALESCE(e.MiddleName, ' ')) as name " &
                                    ",ep.EffectiveDate, ep.CompensationChange, es.BasicPay, ep.PositionFrom, ep.PositionTo, e.EmployeeID, ep.RowID " &
                                    ", IFNULL(ep.Reason,'') 'Reason'" &
                                    "from employeepromotions ep LEFT join employee e on ep.EmployeeID = e.RowID " &
                                    "LEFT join employeesalary es on ep.EmployeeSalaryID = es.RowID " &
                                    "where ep.OrganizationID = '" & z_OrganizationID & "' And e.RowID = '" & dgvEmp.CurrentRow.Cells("RowID").Value & "'" &
                                    " ORDER BY RowID DESC;")

            If dt.Rows.Count > 0 Then

                For Each drow As DataRow In dt.Rows
                    With drow
                        Dim flg As Integer = .Item("CompensationChange").ToString
                        Dim getflg As String
                        If flg = 1 Then
                            getflg = "Yes"
                        Else
                            getflg = "No"
                        End If

                        dgvPromotionList.Rows.Add(.Item("EmployeeID").ToString,
                                                  .Item("Name").ToString,
                                                  .Item("RowID").ToString,
                                                  .Item("PositionFrom").ToString,
                                                  .Item("PositionTo").ToString,
                                                  CDate(.Item("EffectiveDate")).ToString(machineShortDateFormat),
                                                  getflg,
                                                  .Item("BasicPay").ToString,
                                                  .Item("Reason").ToString)

                    End With
                Next
            End If
        End If

    End Sub

    Private Sub fillselectedpromotions()

        txtbasicpay.Text = ""
        cmbflg.SelectedIndex = -1
        cmbfrom.SelectedIndex = -1
        cmbto.SelectedIndex = -1
        cmbSalaryChanged.SelectedIndex = -1
        txtpositfrompromot.Text = ""
        txtReasonPromot.Text = "" 'c_reasonpromot

        txtempcurrbasicpay.Text = "0"

        Label142.Text = "Current salary"

        If Not dgvPromotionList.Rows.Count = 0 Then
            Dim dt As New DataTable
            dt = getDataTableForSQL("select concat(COALESCE(e.Lastname, ' '),' ', COALESCE(e.Firstname, ' '), ' ', COALESCE(e.MiddleName, ' ')) as name " &
                                    ",ep.EffectiveDate, ep.CompensationChange, es.BasicPay, ep.PositionFrom, ep.PositionTo, e.EmployeeID, ep.RowID, " &
                                    "concat('Php', ' ', Format(es.BasicPay,2), ' ', DATE_FORMAT(es.EffectiveDatefrom, '%m/%d/%Y'), ' ', 'To', DATE_FORMAT(es.EffectiveDateTo, '%m/%d/%Y')) As SalaryDate " &
                                    ", IFNULL(ep.Reason,'') 'Reason'" &
                                    "from employeepromotions ep LEFT join employee e on ep.EmployeeID = e.RowID " &
                                    "LEFT join employeesalary es on ep.EmployeeSalaryID = es.RowID " &
                                    "where ep.OrganizationID = '" & z_OrganizationID & "' And e.RowID = '" & rowidPromot & "' And ep.RowID = '" & dgvPromotionList.CurrentRow.Cells(c_promotRowID.Index).Value & "'")
            controlclearPromot()
            If dt.Rows.Count > 0 Then

                For Each drow As DataRow In dt.Rows

                    With drow
                        Dim flg As Integer = .Item("CompensationChange").ToString
                        Dim getflg As String
                        If flg = 1 Then
                            getflg = "Yes"
                        Else
                            getflg = "No"
                        End If

                        cmbflg.Text = getflg
                        cmbfrom.Text = .Item("PositionFrom").ToString
                        cmbto.Text = .Item("PositionTo").ToString
                        dtpEffectivityDate.Text = CDate(.Item("EffectiveDate")).ToString(machineShortDateFormat)

                    End With
                Next
            End If

            With dgvPromotionList.CurrentRow

                cmbflg.Text = .Cells("c_compensation").Value
                cmbfrom.Text = .Cells("c_PostionFrom").Value
                cmbto.Text = .Cells("c_positionto").Value

                txtpositfrompromot.Text = .Cells("c_PostionFrom").Value

                dtpEffectivityDate.Text = Format(CDate(.Cells("c_effecDate").Value), machineShortDateFormat)

                txtempcurrbasicpay.Text = .Cells("c_basicpay").Value

                txtReasonPromot.Text = .Cells("c_reasonpromot").Value

            End With
        End If
    End Sub

    Private Sub fillPositionFrom()
        Dim strQuery As String = "select PositionName from Position Where OrganizationID = '" & z_OrganizationID & "'"
        cmbfrom.Items.Clear()
        cmbfrom.Items.Add("")
        cmbfrom.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbfrom.SelectedIndex = 0
    End Sub

    Private Sub fillSalaryDate()
        Dim strQuery As String = "Select concat('Php', ' ', Format(BasicPay,2), ' ', DATE_FORMAT(EffectiveDatefrom, '%m/%d/%Y'), ' ', 'To ', COALESCE(DATE_FORMAT(EffectiveDateTo, '%m/%d/%Y'),DATE_FORMAT(ADDDATE(CURRENT_DATE(), INTERVAL 100 YEAR),'%m/%d/%Y'))) as salarydate from employeesalary " &
                                 "where EmployeeID = '" & rowidPromot & "' And OrganizationID = '" & z_OrganizationID & "'"
        cmbSalaryChanged.Items.Clear()
        cmbSalaryChanged.Items.Add("-Please Select One-")
        cmbSalaryChanged.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbSalaryChanged.SelectedIndex = 0
    End Sub

    Dim posit_RowID As String = Nothing

    Private Sub cmbto_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbto.KeyPress
        e.Handled = True
    End Sub

    Private Sub cmbto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbto.SelectedIndexChanged

        posit_RowID = Nothing

        For Each r In positn
            posit_RowID = StrReverse(getStrBetween(StrReverse(r), "", "@"))

            If cmbto.Text = posit_RowID Then
                If dgvEmp.RowCount <> 0 Then
                    posit_RowID = getStrBetween(r, "", "@")
                End If

                posit_RowID = getStrBetween(r, "", "@")
                Exit For
            End If
        Next

    End Sub

    Private Sub INSUPD_employeepromotion(sender As Object, e As EventArgs) Handles btnSavePromot.Click

        Dim paramValues(10)

        paramValues(1) = orgztnID
        paramValues(2) = z_User
        paramValues(3) = sameEmpID
        paramValues(4) = txtpositfrompromot.Text
        paramValues(5) = cmbto.Text
        paramValues(6) = Format(CDate(dtpEffectivityDate.Value), "yyyy-MM-dd")

        Dim c_char = Nothing

        If cmbflg.SelectedIndex = -1 Then
            btnSavePromot.Enabled = True
            InfoBalloon("Please select either Yes or No.", "Is compensation changed ?", cmbflg, cmbflg.Width - 16, -70)
            cmbflg.Focus()
            Exit Sub

        ElseIf cmbflg.SelectedIndex = 0 Then
            c_char = "1"

        ElseIf cmbflg.SelectedIndex = 1 Then
            c_char = "0"

        End If

        paramValues(7) = c_char
        paramValues(8) = CDbl(ValNoComma(txtbasicpay.Text))
        paramValues(9) = DBNull.Value
        paramValues(10) = txtReasonPromot.Text.Trim

        If btnNewPromot.Enabled Then
            paramValues(0) = dgvPromotionList.CurrentRow.Cells("c_promotRowID").Value
        Else
            paramValues(0) = DBNull.Value

        End If

        Dim n_ReadSQLFunction As _
            New ReadSQLFunction("INSUPD_employeepromotion",
                                "returnvalue",
                                paramValues)

        If btnNewPromot.Enabled = False Then

            paramValues(0) = n_ReadSQLFunction.ReturnValue

            dgvPromotionList.Rows.Add(dgvEmp.CurrentRow.Cells("Column1").Value,
                                      dgvEmp.CurrentRow.Cells("Column2").Value,
                                      paramValues(0),
                                      paramValues(4),
                                      paramValues(5),
                                      paramValues(6),
                                      If(c_char = "1", "Yes", "No"),
                                      paramValues(8),
                                      paramValues(10))
        Else
            With dgvPromotionList.CurrentRow

                .Cells(3).Value = paramValues(4)
                .Cells(4).Value = paramValues(5)
                .Cells(5).Value = paramValues(6)
                .Cells(6).Value = If(c_char = "1", "Yes", "No")
                .Cells(7).Value = paramValues(8)
                .Cells(8).Value = paramValues(10)

            End With

        End If

        dtpEffectivityDate.MinDate = "1/1/1900"

        btnNewPromot.Enabled = True

        btnSavePromot.Enabled = True

    End Sub

    Dim dontUpdatePromot As SByte = 0

    Private Sub btnSavePromot_Click(sender As Object, e As EventArgs) 'Handles btnSavePromot.Click

        If btnNewPromot.Enabled = False Then 'If IsNewPromot = 1 Then
            If cmbflg.Text = "Yes" Then
                If cmbSalaryChanged.Text = "-Please Select One-" Then
                    myBalloonWarn("Please select one", "System Message", cmbSalaryChanged, , -65)
                    Exit Sub
                End If
            End If

            Dim sID As String = getStringItem("Select rowidPromot from employeesalary " &
                                              "where concat('Php', ' ', Format(BasicPay,2), ' ', DATE_FORMAT(EffectiveDatefrom, '%m/%d/%Y'), ' ', " &
                                              "'To', DATE_FORMAT(EffectiveDateTo, '%m/%d/%Y')) = '" & cmbSalaryChanged.Text & "' " &
                                              "And EmployeeID = '" & rowidPromot & "' And OrganizationID = '" & z_OrganizationID & "'")
            Dim getsID As Integer = Val(sID)

            Dim flg As Integer
            If cmbflg.Text = "Yes" Then
                flg = 1
                If dgvEmp.RowCount <> 0 Then

                    posit_RowID = If(cmbto.SelectedIndex = -1 Or Trim(cmbto.Text) = "",
                                     If(cmbto.SelectedIndex = (cmbto.Items.Count - 1), Nothing, dgvEmp.CurrentRow.Cells("Column29").Value),
                                     posit_RowID)

                End If
            Else
                flg = 0
            End If

            If dgvEmp.RowCount <> 0 Then
                cmbto.Text = If(cmbto.SelectedIndex = -1 Or Trim(cmbto.Text) = "", dgvEmp.CurrentRow.Cells("Column8").Value, Trim(cmbto.Text))

                Dim latest_salaryID = EXECQUER("SELECT RowID FROM employeesalary WHERE EmployeeID='" & dgvEmp.CurrentRow.Cells("RowID").Value & "' AND OrganizationID='" & orgztnID & "' AND EffectiveDateTo IS NULL LIMIT 1;")

                getsID = Val(latest_salaryID)

                sp_promotion(z_datetime, z_User, z_datetime, z_OrganizationID, z_User, dtpEffectivityDate.Value.ToString("yyyy-MM-dd"), txtpositfrompromot.Text, cmbto.Text,
                             getsID, flg, dgvEmp.CurrentRow.Cells("RowID").Value,
                             txtReasonPromot.Text, ValNoComma(txtbasicpay.Text))

                DirectCommand("UPDATE employeesalary SET PaysocialSecurityID = '" & z_ssid & "', PayPhilHealthID = '" & z_phID & "', BasicPay = '" & txtbasicpay.Text.Replace(",", "") & "' Where rowid = '" & getsID & "'")

                fillpromotions()

            End If
        Else
            If dontUpdatePromot = 1 Then
                Exit Sub
            End If

            If dgvEmp.RowCount <> 0 Then
                cmbto.Text = If(cmbto.SelectedIndex = -1 Or Trim(cmbto.Text) = "", dgvEmp.CurrentRow.Cells("Column8").Value, Trim(cmbto.Text))
            End If

            If cmbflg.Text = "Yes" Then
                If cmbSalaryChanged.Text = "-Please Select One-" Then
                    myBalloonWarn("Please select one", "System Message", cmbSalaryChanged, , -65)
                    Exit Sub
                End If
                Dim flg As Integer
                If cmbflg.Text = "Yes" Then
                    flg = 1
                Else
                    flg = 0
                End If
                Dim sID As String = getStringItem("Select rowidPromot from employeesalary " &
                                           "where concat('Php', ' ', Format(BasicPay,2), ' ', DATE_FORMAT(EffectiveDatefrom, '%m/%d/%Y'), ' ', " &
                                           "'To', DATE_FORMAT(EffectiveDateTo, '%m/%d/%Y')) = '" & cmbSalaryChanged.Text & "' " &
                                           "And EmployeeID = '" & rowidPromot & "' And OrganizationID = '" & z_OrganizationID & "'")
                Dim getsID As Integer = Val(sID)
                Dim basicpay As Double
                If Double.TryParse(txtbasicpay.Text, basicpay) Then
                    basicpay = CDec(txtbasicpay.Text)
                Else
                    basicpay = 0

                End If
                ', EmployeeSalaryID = '" & getsID & "'
                DirectCommand("UPDATE employeepromotions SET Effectivedate = '" & dtpEffectivityDate.Value.ToString("yyyy-MM-dd") & "', " &
                              "LastUpd = '" & z_datetime & "', lastupdby = '" & z_User & "', PositionFrom = '" & cmbfrom.Text & "', PositionTo = '" & cmbto.Text &
                              "', CompensationChange = '" & flg & "', Reason = '" & txtReasonPromot.Text & "' Where rowid = '" & dgvPromotionList.CurrentRow.Cells(c_promotRowID.Index).Value & "'")
            Else

                Dim flg As Integer
                If cmbflg.Text = "Yes" Then
                    flg = 1
                Else
                    flg = 0
                End If

                DirectCommand("UPDATE employeepromotions SET Effectivedate = '" & dtpEffectivityDate.Value.ToString("yyyy-MM-dd") & "', LastUpd = '" & z_datetime & "', lastupdby = '" & z_User & "'," &
                       "PositionFrom = '" & cmbfrom.Text & "', PositionTo = '" & cmbto.Text &
                       "', CompensationChange = '" & flg & "', Reason = '" & txtReasonPromot.Text & "' Where rowid = '" & dgvPromotionList.CurrentRow.Cells(c_promotRowID.Index).Value & "'")

            End If
            fillpromotions()
            dgvEmp.Enabled = True
            myBalloon("Successfully Save", "Saving...", lblforballoon, , -69)
        End If

        controlfalsePromot()
        btnNewPromot.Enabled = True
        dgvEmp.Enabled = True

        dtpEffectivityDate.MinDate = Format(CDate("1/1/1900"), machineShortDateFormat)
        posit_RowID = Nothing

        If Trim(txtpositfrompromot.Text) <> Trim(cmbto.Text) Then
            sameEmpID = -1
            Dim row_index As Integer = Nothing

            If dgvEmp.RowCount <> 0 Then
                row_index = dgvEmp.CurrentRow.Index
            End If

            RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            loademployee()

            If dgvEmp.RowCount <> 0 Then
                dgvEmp.Item("Column1", row_index).Selected = True
            End If

            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            dgvEmp_SelectionChanged(sender, e)

        End If

        dtpEffectivityDate.MinDate = Format(CDate("1/1/1900"), machineShortDateFormat)

    End Sub

    Private Sub dgvPromotionList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPromotionList.CellClick
        RemoveHandler cmbflg.SelectedIndexChanged, AddressOf cmbflg_SelectedIndexChanged
        Try
            'fillpromotions()
            controltruePromot()
            fillselectedpromotions()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        End Try

        AddHandler cmbflg.SelectedIndexChanged, AddressOf cmbflg_SelectedIndexChanged
    End Sub

    Private Sub cmbfrom_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cmbfrom.SelectedIndexChanged 'cmbfrom_SelectedIndexChanged
        Try
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        End Try
    End Sub

    Private Sub txtbasicpay_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbasicpay.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtbasicpay.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtbasicpay.Text.Contains(".") Then
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

    Private Sub cmbflg_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cmbflg.SelectedIndexChanged

        If cmbflg.Text = "Yes" Then

            If dgvEmp.RowCount <> 0 Then
            Else
                txtempcurrbasicpay.Text = "0"
            End If

            Label82.SuspendLayout()
            Label82.Visible = True
            Label82.PerformLayout()

            lblpeso.SuspendLayout()
            lblpeso.Visible = True
            lblpeso.PerformLayout()

            Panel11.AutoScrollPosition = New Point(0, 0)

            With txtbasicpay
                .SuspendLayout()

                .BringToFront()
                .Enabled = True
                .Location = New Point(149, 213)
                .Visible = True
                .Focus()

                .PerformLayout()

            End With
        Else
            cmbSalaryChanged.Visible = False

            txtbasicpay.Visible = 0

            Label82.Visible = 0
            lblpeso.Visible = 0

            InfoBalloon(, , txtbasicpay, , , 1)

        End If

        If txtbasicpay.Visible = True Then
            InfoBalloon("Please input the new basic pay.", "New Basic pay", txtbasicpay, txtbasicpay.Width - 16, -70)

        End If
    End Sub

    Private Sub cmbSalaryChanged_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSalaryChanged.SelectedIndexChanged
        Try
            Dim sID As String = getStringItem("Select basicpay from employeesalary " &
                                         "where concat('Php', ' ', Format(BasicPay,2), ' ', DATE_FORMAT(EffectiveDatefrom, '%m/%d/%Y'), ' ', " &
                                         "'To', DATE_FORMAT(EffectiveDateTo, '%m/%d/%Y')) = '" & cmbSalaryChanged.Text & "' " &
                                         "And EmployeeID = '" & rowidPromot & "' And OrganizationID = '" & z_OrganizationID & "'")
            Dim getsID As Double = sID

            txtbasicpay.Text = FormatNumber(getsID, 2)
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        End Try
    End Sub

    Private Sub dgvPromotionList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPromotionList.CellContentClick
        If e.ColumnIndex = c_basicpay.Index Then

        ElseIf e.ColumnIndex = c_empname2.Index Then

        End If
    End Sub

    Private Sub btnCancelPromot_Click(sender As Object, e As EventArgs) Handles btnCancePromot.Click

        dtpEffectivityDate.MinDate = Format(CDate("1/1/1900"), machineShortDateFormat)

        InfoBalloon(, , txtbasicpay, , , 1)

        RemoveHandler cmbflg.SelectedIndexChanged, AddressOf cmbflg_SelectedIndexChanged

        Try
            IsNewPromot = 0
            btnNewPromot.Enabled = True
            dgvEmp.Enabled = True
            controlclearPromot()
            controlfalsePromot()
            fillemplyeelistselected()
            fillpromotions()
            fillselectedpromotions()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        Finally
            txtbasicpay.Visible = 0
            Label82.Visible = 0
            lblpeso.Visible = 0

            dtpEffectivityDate.MinDate = "1/1/1900"

            dgvEmp_SelectionChanged(sender, e)
            posit_RowID = Nothing
        End Try

        Label142.Text = "Current salary"

        'AddHandler cmbflg.SelectedIndexChanged, AddressOf cmbflg_SelectedIndexChanged
    End Sub

#End Region 'Promotion

#Region "Loan Schedule"

    Private view_IDLoan As Integer

    Public loan_type As New AutoCompleteStringCollection

    Private categloantypeID As String = Nothing

    Public loandeducsched As New AutoCompleteStringCollection

    Private cancell_status = New ExecuteQuery("SELECT ii.COLUMN_COMMENT FROM information_schema.`COLUMNS` ii WHERE ii.TABLE_SCHEMA='" & sys_db & "' AND ii.COLUMN_NAME='Status' AND ii.TABLE_NAME='employeeloanschedule';").Result

    Private status_last_index() As String = Split(cancell_status, ",") 'Fully Paid,In Progress,On hold,Cancelled
    Private numb_er = status_last_index.GetUpperBound(0)
    Private str_LoanCancelledStauts As String = status_last_index(numb_er)
    Private IsNewLoan As SByte = 0
    Private dontUpdateLoan As SByte = 0
    Private loantypeID As String = Nothing

    Private interest_charging As SByte = 0

    Private interest_charging_amt As Double = 0
    Private threadArrayList As New List(Of Thread)

    Const six_months_semi_annual = 12

    Sub tbpLoans_Enter(sender As Object, e As EventArgs) Handles tbpLoans.Enter

        tabpageText(tabIndx)

        tbpLoans.Text = "LOAN SCHEDULE               "

        Label25.Text = "LOAN SCHEDULE"
        Static once As SByte = 0
        If once = 0 Then
            once = 1

            OjbAssignNoContextMenu(txtnoofpayper)

            OjbAssignNoContextMenu(txtdedamt)

            OjbAssignNoContextMenu(cboloantype)

            OjbAssignNoContextMenu(cmbStatus)

            OjbAssignNoContextMenu(cmbdedsched)

            view_IDLoan = VIEW_privilege("Employee Loan Schedule", orgztnID)

            categloantypeID = EXECQUER("SELECT RowID FROM category WHERE OrganizationID=" & orgztnID & " AND CategoryName='" & "Loan Type" & "' LIMIT 1;")

            If Val(categloantypeID) = 0 Then
                categloantypeID = INSUPD_category(, "Loan Type")
            End If

            enlistTheLists("SELECT DisplayValue FROM listofval WHERE `Type`='Government deduction schedule' AND Active='Yes' ORDER BY OrderBy;", loandeducsched)

            cmbdedsched.Items.Clear()

            For Each strval In loandeducsched
                cmbdedsched.Items.Add(strval)
            Next
            cmbStatus.Items.Add(str_LoanCancelledStauts)
            enlistTheLists("SELECT CONCAT(COALESCE(PartNo,''),'@',RowID) FROM product WHERE CategoryID='" & categloantypeID & "' AND OrganizationID='" & orgztnID & "' AND PartNo IN ('Calamity', 'Cash Advance', 'PAGIBIG', 'PhilHealth', 'SSS')" &
                           " UNION SELECT CONCAT(COALESCE(PartNo,''),'@',RowID) FROM product WHERE CategoryID='" & categloantypeID & "' AND OrganizationID='" & orgztnID & "';",
                           loan_type)

            For Each strval In loan_type
                cboloantype.Items.Add(getStrBetween(strval, "", "@"))
            Next

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDLoan)

            If formuserprivilege.Count = 0 Then

                tsbtnNewLoan.Visible = 0
                tsbtnSaveLoan.Visible = 0
                DeleteLoanScheduleButton.Visible = 0

                dontUpdateLoan = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        tsbtnNewLoan.Visible = 0
                        tsbtnSaveLoan.Visible = 0
                        DeleteLoanScheduleButton.Visible = 0

                        dontUpdateLoan = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            tsbtnNewLoan.Visible = 0
                        Else
                            tsbtnNewLoan.Visible = 1
                        End If

                        If drow("Deleting").ToString = "N" Then
                            DeleteLoanScheduleButton.Visible = 0
                        Else
                            DeleteLoanScheduleButton.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateLoan = 1
                        Else
                            dontUpdateLoan = 0
                        End If

                    End If
                Next
            End If
        End If

        chkboxChargeToBonus.Visible = (sys_ownr.CurrentSystemOwner = SystemOwner.Goldwings)

        tabIndx = 10 'TabControl1.SelectedIndex
        dgvEmp_SelectionChanged(sender, e)
    End Sub

    Private Sub fillloadsched()
        If dgvEmp.Rows.Count = 0 Then
            Exit Sub
        End If
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select *,COALESCE((SELECT PartNo FROM product WHERE RowID=LoanTypeID),'') 'Loan Type'" &
                                " from employeeloanschedule Where EmployeeID = '" & dgvEmp.CurrentRow.Cells("RowID").Value & "' And OrganizationID = '" & z_OrganizationID & "'" &
                                " ORDER BY DedEffectiveDateFrom DESC;")

        dgvLoanList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvLoanList.Rows.Add()
            With drow

                dgvLoanList.Item(c_loanno.Index, n).Value = .Item("LoanNumber").ToString
                dgvLoanList.Item(c_totloanamt.Index, n).Value = FormatNumber(.Item("totalloanAmount").ToString, 2)
                dgvLoanList.Item(c_totballeft.Index, n).Value = FormatNumber(.Item("TotalBalanceLeft").ToString, 2)
                dgvLoanList.Item(c_dedamt.Index, n).Value = FormatNumber(.Item("DeductionAmount").ToString, 2)
                dgvLoanList.Item(c_DedPercent.Index, n).Value = FormatNumber(.Item("DeductionPercentage").ToString)
                dgvLoanList.Item(c_dedsched.Index, n).Value = .Item("DeductionSchedule").ToString
                dgvLoanList.Item(c_noofpayperiod.Index, n).Value = .Item("Noofpayperiod").ToString
                dgvLoanList.Item(c_RemarksLoan.Index, n).Value = .Item("Comments").ToString
                dgvLoanList.Item(c_RowIDLoan.Index, n).Value = .Item("RowID").ToString
                dgvLoanList.Item(c_status.Index, n).Value = .Item("Status").ToString
                dgvLoanList.Item(c_loantype.Index, n).Value = .Item("Loan Type").ToString
                dgvLoanList.Item(c_noofpayperiodleft.Index, n).Value = .Item("LoanPayPeriodLeft").ToString
                dgvLoanList.Item(c_dedeffectivedatefrom.Index, n).Value =
                    If(IsDBNull(.Item("DedEffectiveDateFrom")),
                       Format(CDate(dbnow), machineShortDateFormat),
                       Format(CDate(.Item("DedEffectiveDateFrom")), machineShortDateFormat))
                dgvLoanList.Item(LoanHasBonus.Index, n).Value = Not IsDBNull(.Item("BonusID"))
            End With
        Next
    End Sub

    Private Sub fillloadschedselected()
        dgvLoanList.Tag = Nothing
        rdbpercent.Checked = 0
        rdbamount.Checked = 0
        chkboxChargeToBonus.Tag = Nothing
        chkboxChargeToBonus.Checked = 0
        TextBox7.Text = ""
        If tsbtnNewLoan.Enabled = True Then
            txtloannumber.Text = ""
        End If
        txtloanamt.Text = ""
        txtbal.Text = ""
        txtdedamt.Text = ""
        txtdedpercent.Text = ""
        cmbdedsched.Text = ""
        txtnoofpayper.Text = ""
        TextBox6.Text = ""
        datefrom.Value = Format(CDate(dbnow), machineShortDateFormat)
        dateto.Value = Format(CDate(dbnow), machineShortDateFormat)
        cmbStatus.Text = ""
        cboloantype.SelectedIndex = -1
        txtnoofpayperleft.Text = 0
        txtloaninterest.Text = ""

        If dgvLoanList.Rows.Count = 0 Then
            Exit Sub
        End If
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select *,(BonusID IS NOT NULL) AS LoanHasBonus,COALESCE((SELECT PartNo FROM product WHERE RowID=LoanTypeID),'') 'Loan Type'" &
                                " from employeeloanschedule Where RowID = '" & dgvLoanList.CurrentRow.Cells(c_RowIDLoan.Index).Value & "' And OrganizationID = '" & z_OrganizationID & "'")
        cleartextbox()
        For Each drow As DataRow In dt.Rows
            With drow
                Dim empid As Integer = .Item("EmployeeID").ToString
                Dim eID As String = getStringItem("Select EmployeeID From Employee Where RowID = '" & empid & "'")
                Dim geteID As Integer = Val(eID)
                TextBox7.Text = geteID
                txtloannumber.Text = Val(.Item("LoanNumber").ToString)
                txtloanamt.Text = FormatNumber(.Item("totalloanAmount").ToString, 2)
                txtbal.Text = FormatNumber(.Item("TotalBalanceLeft").ToString, 2)
                txtdedamt.Text = FormatNumber(.Item("DeductionAmount").ToString, 2)
                cmbdedsched.Text = .Item("DeductionSchedule").ToString
                txtnoofpayper.Text = .Item("Noofpayperiod").ToString
                TextBox6.Text = .Item("Comments").ToString
                datefrom.Value = CDate(.Item("DedEffectiveDateFrom")).ToString(machineShortDateFormat)
                dateto.Value = CDate(.Item("DedEffectiveDateTo")).ToString(machineShortDateFormat)
                cmbStatus.Text = .Item("Status").ToString
                cboloantype.Text = .Item("Loan Type").ToString
                txtnoofpayperleft.Text = If(IsDBNull(.Item("LoanPayPeriodLeft")), 0, .Item("LoanPayPeriodLeft").ToString)
                txtloaninterest.Text = .Item("DeductionPercentage").ToString

                If Val(txtdedamt.Text) <> Nothing Then
                    rdbamount.Checked = 1
                    rdbpercent.Checked = 0
                Else
                    rdbpercent.Checked = 1
                    rdbamount.Checked = 0
                End If
                dgvLoanList.Tag = .Item("RowID")
                chkboxChargeToBonus.Checked = CBool(.Item("LoanHasBonus"))
                chkboxChargeToBonus.Tag = If(IsDBNull(.Item("BonusID")), Nothing, .Item("BonusID"))
            End With
        Next
    End Sub

    Private Sub dgvLoanList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLoanList.CellClick
        fillloadschedselected()
    End Sub

    Private Sub tsbtnNewLoan_Click(sender As Object, e As EventArgs) Handles tsbtnNewLoan.Click
        tsbtnNewLoan.Enabled = False
        IsNewLoan = 1
        Dim loanno As String = 0

        txtloanamt.Text = ""
        cboloantype.Focus() 'txtloanamt
        txtbal.Text = ""
        txtdedamt.Text = ""
        txtdedpercent.Text = ""
        cmbdedsched.SelectedIndex = -1
        txtnoofpayper.Text = ""
        cmbStatus.Text = "In Progress"
        datefrom.Value = Date.Now
        dateto.Value = Date.Now
        txtRemarks.Text = ""
        dgvEmp.Enabled = False

        If dgvEmp.RowCount <> 0 Then
            loanno = getStringItem("Select COALESCE(MAX(LoanNumber),0) from employeeloanschedule where OrganizationID = '" &
                                                 z_OrganizationID & "' And EmployeeID = '" & dgvEmp.CurrentRow.Cells("RowID").Value & "';")
        End If

        Dim getloanno As Integer = Val(loanno) + 1
        txtloannumber.Text = getloanno
        chkboxChargeToBonus.Checked = tsbtnNewLoan.Enabled
    End Sub

    Private Sub tsbtnSaveLoan_Click(sender As Object, e As EventArgs) Handles tsbtnSaveLoan.Click
        pbEmpPicLoan.Focus() 'And txtdedpercent.Text = ""

        txtloanamt.Focus()

        pbEmpPicLoan.Focus()

        txtdedamt.Focus()

        pbEmpPicLoan.Focus()

        txtnoofpayper.Focus()

        pbEmpPicLoan.Focus()

        If loantypeID = Nothing Then
            WarnBalloon("Please select a type of loan.", "Invalid Type of loan", cboloantype, cboloantype.Width - 16, -70)
            Exit Sub
        ElseIf dgvEmp.RowCount = 0 Then
            IsNewLoan = 0
            dgvEmp.Enabled = True
            tsbtnNewLoan.Enabled = True

            Exit Sub
        ElseIf cmbdedsched.Text.Length = 0 Then
            WarnBalloon("Please select a loan deduction schedule.", "Invalid Loan Deduction Schedule", lblforballoon, , -100)
            Exit Sub
        ElseIf cmbdedsched.SelectedIndex = -1 Then
            WarnBalloon("Please select a loan deduction schedule.", "Invalid Loan Deduction Schedule", lblforballoon, , -100)
            Exit Sub
        End If

        If txtloannumber.Text = "" And txtbal.Text = "" And txtdedamt.Text = "" _
            And cmbdedsched.Text = "" And txtnoofpayper.Text = "" And cmbStatus.Text = "" Then
            If SetWarningIfEmpty(txtloannumber) And SetWarningIfEmpty(txtbal) And SetWarningIfEmpty(txtdedamt) And SetWarningIfEmpty(txtdedpercent) _
                And SetWarningIfEmpty(cmbdedsched) And SetWarningIfEmpty(txtnoofpayper) And SetWarningIfEmpty(cmbStatus) Then
                Exit Sub
            End If
            Exit Sub
        End If

        Dim empid As Integer = dgvEmp.CurrentRow.Cells("RowID").Value

        If IsNewLoan = 1 Then

            Dim LoanPayPeriodToDate = PAYTODATE_OF_NoOfPayPeriod(datefrom.Value.ToString("yyyy-MM-dd"),
                                                                 ValNoComma(txtnoofpayper.Text),
                                                                 dgvEmp.CurrentRow.Cells("RowID").Value,
                                                                 cmbdedsched.Text.Trim)

            SP_LoadSchedule(z_User, z_User, z_datetime, z_datetime, Val(txtloannumber.Text.Replace(",", "")), datefrom.Value.ToString("yyyy-MM-dd"), Format(CDate(LoanPayPeriodToDate), "yyyy-MM-dd"),
                            z_OrganizationID, Val(empid), Val(txtloanamt.Text.Replace(",", "")), Trim(cmbdedsched.Text), Val(txtbal.Text.Replace(",", "")), Val(txtdedamt.Text.Replace(",", "")),
                            Val(txtnoofpayper.Text.Replace(",", "")), txtRemarks.Text, cmbStatus.Text, 0, loantypeID,
                            cmbdedsched.Text, chkboxChargeToBonus.Tag) 'Val(txtdedpercent.Text)
            fillloadsched()
            fillloadschedselected()
            myBalloon("Successfully Save", "Saved", lblforballoon, , -100)
        Else
            If dontUpdateLoan = 1 Then
                Exit Sub
            End If
            SP_UpdateLoadSchedule(z_User, z_datetime, Val(txtloannumber.Text), datefrom.Value.ToString("yyyy-MM-dd"), dateto.Value.ToString("yyyy-MM-dd"),
                                 Val(txtloanamt.Text.Replace(",", "")), "", Val(txtdedamt.Text.Replace(",", "")),
                                 Val(txtnoofpayper.Text), Trim(TextBox6.Text), cmbStatus.Text, 0, dgvLoanList.CurrentRow.Cells(c_RowIDLoan.Index).Value,
                                 loantypeID,
                                 cmbdedsched.Text, chkboxChargeToBonus.Tag) 'CDec(txtdedpercent.Text)'cmbdedsched.Text
            fillloadsched()
            fillloadschedselected()
            SaveBonusCommentsRegardsToLoan()
            myBalloon("Successfully Save", "Saved", lblforballoon, , -100)

        End If

        IsNewLoan = 0
        dgvEmp.Enabled = True
        tsbtnNewLoan.Enabled = True

        SetWarningIfEmpty(txtloannumber, "Hide this error provider")
        SetWarningIfEmpty(txtbal, "Hide this error provider")
        SetWarningIfEmpty(txtdedamt, "Hide this error provider")
        SetWarningIfEmpty(txtdedpercent, "Hide this error provider")
        SetWarningIfEmpty(cmbdedsched, "Hide this error provider")
        SetWarningIfEmpty(txtnoofpayper, "Hide this error provider")
        SetWarningIfEmpty(cmbStatus, "Hide this error provider")
    End Sub

    Private Sub ToolStripButton23_Click(sender As Object, e As EventArgs) Handles ToolStripButton23.Click
        cleartextboxLoan()
        IsNewLoan = 0
        dgvEmp.Enabled = True
        fillloadsched()
        fillloadschedselected()
        tsbtnNewLoan.Enabled = 1
    End Sub

    Private Sub lblAdd_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAdd.LinkClicked
        AddListOfValueForm.lblName.Text = "Deduction Schedule"
        AddListOfValueForm.ShowDialog()

    End Sub

    Sub cleartextboxLoan()
        txtloanamt.Clear()
        txtbal.Clear()
        txtdedamt.Clear()
        txtdedpercent.Clear()
        cmbdedsched.SelectedIndex = -1
        txtnoofpayper.Clear()
        cmbStatus.SelectedIndex = -1
        datefrom.Value = Date.Now
        dateto.Value = Date.Now
        txtRemarks.Clear()
    End Sub

    Private Sub cboloantype_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboloantype.KeyPress
        e.Handled = True
    End Sub

    Private Sub cboloantype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboloantype.SelectedIndexChanged

        Dim loanType_Defualt_DeductSched = String.Empty

        loanType_Defualt_DeductSched =
            EXECQUER("SELECT DisplayValue" &
                     " FROM listofval" &
                     " WHERE `Type`='Government deduction schedule'" &
                     " AND Active='Yes'" &
                     " AND Description='" & cboloantype.Text.Trim & "'" &
                     " ORDER BY OrderBy;")

        If loanType_Defualt_DeductSched <> String.Empty Then

            cmbdedsched.Text = loanType_Defualt_DeductSched.ToString
        End If
    End Sub

    Private Sub cboloantype_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboloantype.SelectedValueChanged
        loantypeID = Nothing

        If Trim(cboloantype.Text) = "" Or cboloantype.SelectedIndex = -1 Then
        Else
            For Each strval In loan_type
                loantypeID = getStrBetween(strval, "", "@")
                If loantypeID = Trim(cboloantype.Text) Then
                    loantypeID = StrReverse(getStrBetween(StrReverse(strval), "", "@"))
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub lnklblloantype_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblloantype.LinkClicked
        With LoanType
            .Show()
            .BringToFront()
            .TextBox1.Focus()
        End With
    End Sub

    Private Sub btnloantype_Click(sender As Object, e As EventArgs) 'Handles btnloantype.Click, lnklblloantype.Click
        With LoanType
            .Show()
            .BringToFront()
            .TextBox1.Focus()
        End With
    End Sub

    Private Sub txtloanamt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtloanamt.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtloanamt.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtloanamt.Text.Contains(".") Then
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

    Private Sub txtloanamt_Leave(sender As Object, e As EventArgs) Handles txtloanamt.Leave

        txtloanamt.Text = txtloanamt.Text.Replace(",", "")

        If Val(txtloaninterest.Text) = 0.06 Then

            Dim nochangeval = interest_charging_amt + (interest_charging_amt * Val(txtloaninterest.Text))

            If Val(txtloanamt.Text) = nochangeval Then
            Else
                interest_charging_amt = Val(txtloanamt.Text)

                txtnoofpayper_Leave(sender, e)
            End If
        Else
            interest_charging_amt = Val(txtloanamt.Text)
        End If
    End Sub

    Private Sub txtdedamt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtdedamt.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtdedamt.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtdedamt.Text.Contains(".") Then
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

    Private Sub txtdedamt_TextChanged(sender As Object, e As EventArgs) Handles txtdedamt.TextChanged
        txtdedamt.Text = If(Val(txtdedamt.Text) = 0, 0, txtdedamt.Text)
    End Sub

    Private Sub txtdedamt_Leave(sender As Object, e As EventArgs) Handles txtdedamt.Leave

        txtdedamt.Text = txtdedamt.Text.Replace(",", "")

        Dim loanamt, dedamt, totalded As Double
        If Double.TryParse(txtdedamt.Text, dedamt) Then
            dedamt = Val(txtdedamt.Text.Replace(",", ""))
            loanamt = Val(txtloanamt.Text.Replace(",", ""))
            totalded = loanamt / dedamt
        End If

        Dim decimaldigit = Microsoft.VisualBasic.Right(FormatNumber(totalded, 1).ToString, 1)

        totalded = Val(totalded.ToString.Replace(",", ""))

        txtnoofpayper.Text = FormatNumber(totalded, 0).Replace(",", "")

        If tsbtnNewLoan.Enabled = False Then
            txtnoofpayperleft.Text = txtnoofpayper.Text
        End If
    End Sub

    Private Sub txtnoofpayper_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnoofpayper.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)
        e.Handled = TrapNumKey(e_KAsc)
    End Sub

    Private Sub txtnoofpayper_Leave(sender As Object, e As EventArgs) Handles txtnoofpayper.Leave

        If tsbtnNewLoan.Enabled = False Then

            txtnoofpayperleft.Text = txtnoofpayper.Text

            Dim numpayp = Val(txtnoofpayper.Text)

            Dim loan_interest As Decimal = 0

            loan_interest = ValNoComma(txtloaninterest.Text)

            Dim bool As Boolean =
                If((numpayp > six_months_semi_annual) = True,
                sys_ownr.CurrentSystemOwner = SystemOwner.Cinema2000,
                True)

            If bool Then

                If txtloaninterest.Text.Trim.Length = 0 Then

                    txtloaninterest.Text = loan_interest

                End If

                loan_interest = ValNoComma(txtloaninterest.Text)

                txtloanamt.Text = FormatNumber(Val(interest_charging_amt + (interest_charging_amt * loan_interest)), 2).Replace(",", "")

                Dim loan_amt = interest_charging_amt / numpayp
                Dim tot_loan = (ValNoComma(FormatNumber(loan_amt, 2)) * numpayp)
                Dim roundoff_decim = Math.Round(tot_loan, 2)


                loan_amt = (loan_amt + (loan_amt * loan_interest))

                loan_amt = FormatNumber(loan_amt, 2).ToString.Replace(",", "")

                txtdedamt.Text = loan_amt

                txtdedamt_Leave(sender, e)
            Else
                If sys_ownr.CurrentSystemOwner = SystemOwner.Cinema2000 Then
                    InfoBalloon("Interest discarded if number of pay period is 6 months or lesser.",
                                "Loan interest validation", Label367, (Label367.Width - 15), -60)
                End If

                loan_interest = 0

                txtloaninterest.Text = 0

                txtloanamt.Text = interest_charging_amt

                Dim loan_amt = interest_charging_amt / numpayp

                Dim tot_loan = (ValNoComma(FormatNumber(loan_amt, 2)) * numpayp)

                Dim roundoff_decim = Math.Round(tot_loan, 2)


                loan_amt = FormatNumber(loan_amt, 2).ToString.Replace(",", "")

                txtdedamt.Text = loan_amt

            End If

        End If

    End Sub

    Private Sub txtloaninterest_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtloaninterest.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtloaninterest.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtloaninterest.Text.Contains(".") Then
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

    Private Sub txtloaninterest_Leave(sender As Object, e As EventArgs) Handles txtloaninterest.Leave
        txtnoofpayper_Leave(txtnoofpayper,
                            New EventArgs)
    End Sub

    Private Sub txtdedpercent_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtdedpercent.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtdedpercent.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtdedpercent.Text.Contains(".") Then
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

    Private Sub cmbStatus_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbStatus.KeyPress
        e.Handled = True
    End Sub

    Private Sub rdbpercent_CheckedChanged(sender As Object, e As EventArgs) Handles rdbpercent.CheckedChanged
        Dim rdbcheckd = If(rdbpercent.Checked, 1, 0)
        txtdedpercent.Enabled = rdbcheckd
    End Sub

    Private Sub rdbamount_CheckedChanged(sender As Object, e As EventArgs) Handles rdbamount.CheckedChanged
        Dim rdbcheckd = If(rdbamount.Checked, 1, 0)
        txtdedamt.Enabled = True 'rdbcheckd
    End Sub

    Function PAYTODATE_OF_NoOfPayPeriod(Optional EmpLoanEffectiveDateFrom As Object = Nothing,
                                        Optional EmpLoanNoOfPayPeriod As Object = Nothing,
                                        Optional Employee_RowID As Object = Nothing,
                                        Optional LoanDeductSched As Object = Nothing) As Object

        Dim params(3, 2) As Object

        params(0, 0) = "EmpLoanEffectiveDateFrom"
        params(1, 0) = "EmpLoanNoOfPayPeriod"
        params(2, 0) = "Employee_RowID"
        params(3, 0) = "LoanDeductSched"

        params(0, 1) = EmpLoanEffectiveDateFrom

        Dim newValInt = CInt(EmpLoanNoOfPayPeriod)

        params(1, 1) = newValInt

        params(2, 1) = Employee_RowID

        params(3, 1) = LoanDeductSched

        PAYTODATE_OF_NoOfPayPeriod =
            EXEC_INSUPD_PROCEDURE(params,
                                  "PAYTODATE_OF_NoOfPayPeriod",
                                  "ReturnDate",
                                  MySqlDbType.Date)
    End Function

    Private Sub cmbdedsched_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbdedsched.KeyPress
        e.Handled = True
    End Sub

    Private Sub tsbtnImportLoans_Click(sender As Object, e As EventArgs) Handles tsbtnImportLoans.Click
        Dim browsefile As New OpenFileDialog()

        browsefile.Filter = str_ms_excel_file_extensn

        If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

            filepath = IO.Path.GetFullPath(browsefile.FileName)

            Dim catchDatSet =
                getWorkBookAsDataSet(filepath,
                                     Me.Name)

            If (catchDatSet Is Nothing) = False And Trim(filepath).Length > 0 Then

                Dim n_ImportLoans As New ImportLoans(catchDatSet, Me)

                Dim objNewThread As New Thread(AddressOf n_ImportLoans.StartProcess)

                Static indx As Integer = 0

                indx += 1

                objNewThread.Name = String.Concat("ImportLoans", indx)

                objNewThread.IsBackground = True

                objNewThread.Start()

                threadArrayList.Add(objNewThread)

            End If
        End If
    End Sub

    Private Sub SaveBonusCommentsRegardsToLoan()
        For Each dict In ebonus_rowid_comment
            Dim str_comment As String = String.Concat("'", dict.Value(0), "'")
            Dim bool_bonus_potent As Short = Convert.ToInt16(dict.Value(1))
            Dim row_id = dict.Key

            Dim str_quer As String = String.Empty

            If bool_bonus_potent = 0 Then

                str_quer =
                String.Concat("UPDATE employeebonus eb",
                              " LEFT JOIN employeeloanschedule els ON els.BonusID=eb.RowID",
                              " SET eb.`Remarks`=", str_comment,
                              ",els.Comments=", str_comment,
                              ",els.BonusPotentialPaymentForLoan=", bool_bonus_potent,
                              ",els.LoanPayPeriodLeftForBonus=IF(IFNULL(els.LoanPayPeriodLeftForBonus, 0) = 0, els.LoanPayPeriodLeft, els.LoanPayPeriodLeftForBonus)",
                              ",eb.RemainingBalance = (eb.RemainingBalance - els.DeductionAmount)",
                              " WHERE eb.RowID='", row_id, "';")
            Else 'If bool_bonus_potent = 1 Then

                str_quer =
                String.Concat("UPDATE employeebonus eb",
                              " LEFT JOIN employeeloanschedule els ON els.BonusID=eb.RowID",
                              " SET eb.`Remarks`=", str_comment,
                              ",els.Comments=", str_comment,
                              ",els.BonusPotentialPaymentForLoan=", bool_bonus_potent,
                              ",els.LoanPayPeriodLeftForBonus=IF(IFNULL(els.LoanPayPeriodLeftForBonus, 0) = 0, els.LoanPayPeriodLeft, els.LoanPayPeriodLeftForBonus)",
                              ",eb.RemainingBalance = (eb.RemainingBalance - (els.DeductionAmount * els.LoanPayPeriodLeft))",
                              " WHERE eb.RowID='", row_id, "';")

            End If

            Dim exec_quer As New ExecuteQuery(str_quer)
            Dim exec_result = exec_quer.Result
        Next
    End Sub

    ''' <summary>
    ''' Deletes the loan schedule the user has requested.
    ''' </summary>
    Private Sub DeleteLoanSchedule(sender As Object, e As EventArgs) Handles DeleteLoanScheduleButton.Click
        Dim loanScheduleID As Integer

        If Not Integer.TryParse(dgvLoanList.CurrentRow.Cells(c_RowIDLoan.Index).Value, loanScheduleID) Then
            MsgBox("Sorry, but something has gone awry. Please contact Globagility, Inc. if you see this error message.")
            Return
        End If

        Try
            loanScheduleID = Convert.ToInt32(dgvLoanList.CurrentRow.Cells(c_RowIDLoan.Index).Value)
            Dim prompt = MessageBox.Show(
                "Do you want to delete this loan ?",
                "Confirm deletion",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2)

            If prompt = DialogResult.Yes Then
                Using context = New PayrollContext()
                    Dim loanSchedule = context.LoanSchedules.Find(loanScheduleID)

                    If loanSchedule.LoanTransactions.Count() > 0 Then
                        Dim secondPrompt = MessageBox.Show(
                            "This loan has already started, are you sure you want to delete this loan? Doing this might affect previous cutoffs.",
                            "Confirm deletion",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2)

                        If secondPrompt <> DialogResult.Yes Then
                            Return
                        End If
                    End If

                    context.LoanSchedules.Remove(loanSchedule)
                    context.SaveChanges()
                End Using

                Dim curr_row = dgvLoanList.CurrentRow
                dgvLoanList.Rows.Remove(curr_row)
            End If
        Catch ex As Exception
            Throw New Exception($"Failed to delete loan schedule #{loanScheduleID}.", ex)
        End Try
    End Sub

#End Region 'Loan Schedule

#Region "Loan History"

    Dim view_IDHisto As Integer

    Sub tbpLoanHist_Enter(sender As Object, e As EventArgs) Handles tbpLoanHist.Enter

        tabpageText(tabIndx)

        tbpLoanHist.Text = "LOAN HISTORY               "

        Label25.Text = "LOAN HISTORY"
        Static once As SByte = 0
        If once = 0 Then
            once = 1
            view_IDHisto = VIEW_privilege("Employee Loan History", orgztnID)

            AddHandler dgvloanhisto.SelectionChanged, AddressOf dgvloanhisto_SelectionChanged

        End If

        tabIndx = 11 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)
    End Sub

    Private Sub dgvloanhisto_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvloanhisto.CellClick

        ComboBox2.SelectedIndex = -1
        ComboBox2.Text = ""
        dateded.Value = CDate(Format(CDate(dbnow), machineShortDateFormat))
        TextBox11.Text = ""
        txtamount.Text = "0"

        If dgvloanhisto.RowCount <> 0 Then
            With dgvloanhisto.CurrentRow

                ComboBox2.Text = .Cells("DataGridViewTextBoxColumn116").Value
                dateded.Value = CDate(Format(CDate(.Cells("c_dateded").Value), machineShortDateFormat))
                TextBox11.Text = .Cells("DataGridViewTextBoxColumn117").Value
                txtamount.Text = .Cells("c_Amount").Value

            End With
        Else
        End If
    End Sub

    Private Sub dgvloanhisto_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvloanhisto.SelectionChanged
        If dgvloanhisto.RowCount <> 0 Then
            With dgvloanhisto.CurrentRow
                If .IsNewRow Then
                    dateded.Value = Format(CDate(dbnow), machineShortDateFormat)
                    ComboBox2.SelectedIndex = -1
                    ComboBox2.Text = ""
                    TextBox11.Text = ""
                    txtamount.Text = ""
                Else
                    If .Cells("c_dateded").Value = Nothing Then
                        dateded.Value = Format(CDate(dbnow), machineShortDateFormat)
                    Else
                        dateded.Value = Format(CDate(.Cells("c_dateded").Value), machineShortDateFormat)
                    End If
                    ComboBox2.Text = .Cells("DataGridViewTextBoxColumn116").Value 'Status
                    TextBox11.Text = .Cells("DataGridViewTextBoxColumn117").Value 'Remarks
                    txtamount.Text = .Cells("c_Amount").Value 'Amount
                End If
            End With
        Else

            dateded.Value = Format(CDate(dbnow), machineShortDateFormat)
            ComboBox2.SelectedIndex = -1
            ComboBox2.Text = ""
            TextBox11.Text = ""
            txtamount.Text = ""
        End If
    End Sub

    Sub VIEW_employeeloanhistory(ByVal EmployeeRowID As Object)
        Dim params = New Object(,) {
            {"ehist_EmployeeID", EmployeeRowID},
            {"ehist_OrganizationID", orgztnID},
            {"ehist_LoanType", Nothing}
        }

        EXEC_VIEW_PROCEDURE(params, "VIEW_employeeloanhistory", dgvloanhisto)
    End Sub

    Private Sub GroupBox3_Enter(sender As Object, e As EventArgs) Handles GroupBox3.Enter

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            cbohistoloantype.Enabled = False

            enlistToCboBox("SELECT p.PartNo" &
                           " FROM product p" &
                           " INNER JOIN category c ON c.OrganizationID='" & orgztnID & "' AND c.CategoryName='Loan Type'" &
                           " WHERE p.CategoryID=c.RowID" &
                           " AND p.OrganizationID=" & orgztnID & ";",
                           cbohistoloantype)

            cbohistoloantype.Enabled = True

            AddHandler cbohistoloantype.SelectedIndexChanged, AddressOf cbohistoloantype_SelectedIndexChanged
        End If
    End Sub

    Private Sub cbohistoloantype_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cbohistoloantype.SelectedIndexChanged
        Dim params = New Object(,) {
            {"ehist_EmployeeID", dgvEmp.CurrentRow.Cells("RowID").Value},
            {"ehist_OrganizationID", orgztnID},
            {"ehist_LoanType", cbohistoloantype.Text}
        }

        EXEC_VIEW_PROCEDURE(params, "VIEW_employeeloanhistory", dgvloanhisto)
    End Sub

#End Region 'Loan History

#Region "Salary"

    Dim noofdepd, view_IDSal As Integer
    Dim mStat As String
    Dim filingid As Integer

    Dim isorgPHHdeductsched As SByte
    Dim isorgSSSdeductsched As SByte
    Dim isorgHDMFdeductsched As SByte

    Public listofEditEmpSal As New List(Of String)

    Dim objGotFoc As Object

    Dim is_user_override_phh As Boolean
    Dim is_user_override_sss As Boolean
    Dim payfreqdivisor = Val(0)

    'Function
    Function INSUPD_employeesalary(Optional esal_RowID As Object = Nothing,
                              Optional esal_EmployeeID As Object = Nothing,
                              Optional esal_BasicPay As Object = Nothing,
                              Optional esal_Salary As Object = Nothing,
                              Optional esal_NoofDependents As Object = Nothing,
                              Optional esal_MaritalStatus As Object = Nothing,
                              Optional esal_PositionID As Object = Nothing,
                              Optional esal_EffectiveDateFrom As Object = Nothing,
                              Optional esal_EffectiveDateTo As Object = Nothing,
                              Optional esal_TrueSalary As Object = Nothing,
                              Optional esal_IsDoneByImporting As Object = "0",
                              Optional esal_PaySocialSecurityID As Integer? = Nothing,
                              Optional esal_PayPhilHealthID As Integer? = Nothing,
                              Optional esal_PhilHealthDeduction As Decimal? = Nothing) As Object

        Dim date_to = If(esal_EffectiveDateTo = Nothing, DBNull.Value, Format(CDate(esal_EffectiveDateTo), "yyyy-MM-dd"))

        Dim _params =
            New Object() {If(esal_RowID = Nothing, DBNull.Value, esal_RowID),
            esal_EmployeeID,
            z_User,
            z_User,
            orgztnID,
            esal_BasicPay,
            esal_Salary,
            esal_NoofDependents,
            esal_MaritalStatus,
            If(esal_PositionID = Nothing, DBNull.Value, esal_PositionID),
            Format(CDate(esal_EffectiveDateFrom), "yyyy-MM-dd"),
            date_to,
            (ValNoComma(txtPagibig.Text) * payfreqdivisor),
            (ValNoComma(txtPagibig.Text) * payfreqdivisor),
            If(esal_TrueSalary = Nothing, Val(esal_Salary), Val(esal_TrueSalary)),
            esal_IsDoneByImporting,
            Convert.ToInt16(is_user_override_sss),
            Convert.ToInt16(is_user_override_phh),
            esal_PaySocialSecurityID,
            esal_PayPhilHealthID,
            esal_PhilHealthDeduction}

        Dim str_query As String =
            String.Concat("SELECT INSUPD_employeesalary(",
                          "?esal_RowID",
                          ", ?esal_EmployeeID",
                          ", ?esal_CreatedBy",
                          ", ?esal_LastUpdBy",
                          ", ?esal_OrganizationID",
                          ", ?esal_BasicPay",
                          ", ?esal_Salary",
                          ", ?esal_NoofDependents",
                          ", ?esal_MaritalStatus",
                          ", ?esal_PositionID",
                          ", ?esal_EffectiveDateFrom",
                          ", ?esal_EffectiveDateTo",
                          ", ?esal_HDMFAmount",
                          ", ?esal_PAGIBIGAmout",
                          ", ?esal_TrueSalary",
                          ", ?esal_IsDoneByImporting",
                          ", ?esal_DiscardSSS",
                          ", ?esal_DiscardPhH",
                          ", ?esal_PaySocialSecurityID",
                          ", ?esal_PayPhilHealthID",
                          ", ?esal_PhilHealthDeduction",
                          ") `Result`;")

        Dim sql As New SQL(str_query, _params)

        Dim returnvalue As Object = Nothing

        returnvalue = sql.GetFoundRow

        If sql.HasError Then
            Try
                Throw sql.ErrorException
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Name))
            End Try
        End If

        Return returnvalue

    End Function

#End Region 'Salary

#Region "Pay slip"

    Dim paypyearnow

    Dim viewIDPaySlip As Integer = Nothing

    Sub tbpPayslip_Enter(sender As Object, e As EventArgs) Handles tbpPayslip.Enter
        tabpageText(tabIndx)

        tbpPayslip.Text = "PAY SLIP               "

        Label25.Text = "PAY SLIP"

        Static once As SByte = 0

        If once = 0 Then
            once = 1

            VIEW_payperiodofyear()

            paypyearnow = Format(CDate(dbnow), "yyyy")

            linkPrev.Text = "← " & (Val(paypyearnow) - 1)
            linkNxt.Text = (Val(paypyearnow) + 1) & " →"

            viewIDPaySlip = VIEW_privilege("Employee Pay Slip", orgztnID)

            AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        End If

        tabIndx = 12 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)
    End Sub

    Private Sub tbpPayslip_Leave(sender As Object, e As EventArgs) 'Handles tbpPayslip.Leave
        tbpPayslip.Text = "PAYSLIP"
    End Sub

    Public paypFrom As String = Nothing
    Public paypTo As String = Nothing
    Public paypRowID As String = Nothing
    Public isEndOfMonth As String = 0

    Dim numofweekdays As Integer = Nothing

    Private Sub dgvpayper_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvpayper.SelectionChanged

        If dgvpayper.RowCount <> 0 Then
            If dgvEmp.RowCount <> 0 Then
                Dim selEmpID = dgvEmp.CurrentRow.Cells("RowID").Value

                With dgvpayper.CurrentRow
                    paypFrom = Format(CDate(.Cells("payp_from").Value), "yyyy-MM-dd")
                    paypTo = Format(CDate(.Cells("payp_to").Value), "yyyy-MM-dd")

                    paypRowID = .Cells("payp_RowID").Value

                    isEndOfMonth = Trim(.Cells("payp_endofmonth").Value)

                    Dim date_diff = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

                    numofweekdays = 0

                    For i = 0 To date_diff

                        Dim DayOfWeek = CDate(paypFrom).AddDays(i)

                        If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                            'numofweekends += 1

                        ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                            'numofweekends += 1
                        Else
                            numofweekdays += 1
                        End If
                    Next
                End With

                txtempbasicpay.Text = "0.00"

                txttotreghrs.Text = "0.00"
                txttotregamt.Text = "0.00"

                txttotothrs.Text = "0.00"
                txttototamt.Text = "0.00"

                txttotnightdiffhrs.Text = "0.00"
                txttotnightdiffamt.Text = "0.00"

                txttotnightdiffothrs.Text = "0.00"
                txttotnightdiffotamt.Text = "0.00"

                txttotholidayhrs.Text = "0.00"
                txttotholidayamt.Text = "0.00"

                txthrswork.Text = "0.00"
                txthrsworkamt.Text = "0.00"

                lblsubtot.Text = "0.00"

                txtemptotallow.Text = "0.00"

                txtemptotbon.Text = "0.00"

                txtgrosssal.Text = "0.00"

                txttotabsent.Text = "0.00"
                txttotabsentamt.Text = "0.00"

                txttottardi.Text = "0.00"
                txttottardiamt.Text = "0.00"

                txttotut.Text = "0.00"
                txttotutamt.Text = "0.00"

                lblsubtotmisc.Text = "0.00"

                txtempsss.Text = "0.00"
                txtempphh.Text = "0.00"
                txtemphdmf.Text = "0.00"

                txttaxabsal.Text = "0.00"

                txtempwtax.Text = "0.00"

                txtemptotloan.Text = "0.00"

                txtnetsal.Text = "0.00"

                vlbal.Text = "0"
                slbal.Text = "0"
                mlbal.Text = "0"

                TextBox8.Text = dgvEmp.CurrentRow.Cells("Column36").Value
                TextBox9.Text = dgvEmp.CurrentRow.Cells("slallowance").Value
                TextBox5.Text = dgvEmp.CurrentRow.Cells("mlallowance").Value

                TextBox13.Text = dgvEmp.CurrentRow.Cells("Column33").Value
                TextBox14.Text = dgvEmp.CurrentRow.Cells("slpayp").Value
                TextBox10.Text = dgvEmp.CurrentRow.Cells("mlpayp").Value

                VIEW_paystub(selEmpID,
                             paypRowID)

                For Each dgvrow As DataGridViewRow In dgvpaystub.Rows
                    With dgvrow
                        txtgrosssal.Text = FormatNumber(Val(.Cells("paystb_TotalGrossSalary").Value), 2)

                        txtnetsal.Text = FormatNumber(Val(.Cells("paystb_TotalNetSalary").Value), 2)

                        txttaxabsal.Text = FormatNumber(Val(.Cells("paystb_TotalTaxableSalary").Value), 2)

                        txtempwtax.Text = FormatNumber(Val(.Cells("paystb_TotalEmpWithholdingTax").Value), 2)

                        txtempsss.Text = FormatNumber(Val(.Cells("paystb_TotalEmpSSS").Value), 2)
                        txtempphh.Text = FormatNumber(Val(.Cells("paystb_TotalEmpPhilhealth").Value), 2)
                        txtemphdmf.Text = FormatNumber(Val(.Cells("paystb_TotalEmpHDMF").Value), 2)

                        txtemptotallow.Text = FormatNumber(Val(.Cells("paystb_TotalAllowance").Value), 2)

                        txtemptotloan.Text = FormatNumber(Val(.Cells("paystb_TotalLoans").Value), 2)
                        txtemptotbon.Text = FormatNumber(Val(.Cells("paystb_TotalBonus").Value), 2)

                        VIEW_paystubitem(.Cells("paystb_RowID").Value)

                        For Each dgvrw As DataGridViewRow In dgvpaystubitm.Rows
                            With dgvrw
                                If .Cells("Item").Value.ToString = "Vacation leave" Then
                                    vlbal.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                                ElseIf .Cells("Item").Value.ToString = "Sick leave" Then
                                    slbal.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                                ElseIf .Cells("Item").Value.ToString = "Maternity/paternity leave" Then
                                    mlbal.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                                End If
                            End With
                        Next
                    End With
                Next
                VIEW_specificemployeesalary(selEmpID,
                                            paypTo)

                For Each dgvrow As DataGridViewRow In dgvempsal.Rows
                    txtempbasicpay.Text = "0.00"

                    With dgvrow
                        txtempbasicpay.Text = FormatNumber(Val(.Cells("esal_BasicPay").Value), 2)

                        If isorgSSSdeductsched = 1 Then
                            If isEndOfMonth = "0" Then
                                txtempsss.Text = "0.00"
                            Else
                                txtempsss.Text = Val(.Cells("esal_EmployeeContributionAmount").Value)
                                txtempsss.Text = FormatNumber(txtempsss.Text, 2).ToString '.Replace(",", "")
                            End If
                        Else
                            txtempsss.Text = Val(.Cells("esal_EmployeeContributionAmount").Value) / 2
                            txtempsss.Text = FormatNumber(txtempsss.Text, 2).ToString '.Replace(",", "")

                        End If

                        If isorgPHHdeductsched = 1 Then
                            If isEndOfMonth = "0" Then
                                txtempphh.Text = "0.00"
                            Else
                                txtempphh.Text = .Cells("esal_EmployeeShare").Value
                                txtempphh.Text = FormatNumber(txtempphh.Text, 2).ToString '.Replace(",", "")
                            End If
                        Else
                            txtempphh.Text = Val(.Cells("esal_EmployeeShare").Value) / 2
                            txtempphh.Text = FormatNumber(txtempphh.Text, 2).ToString '.Replace(",", "")

                        End If

                        If isorgHDMFdeductsched = 1 Then
                            If isEndOfMonth = "0" Then
                                txtemphdmf.Text = "0.00"
                            Else
                                txtemphdmf.Text = .Cells("esal_HDMFAmount").Value
                                txtemphdmf.Text = FormatNumber(txtemphdmf.Text, 2).ToString '.Replace(",", "")
                            End If
                        Else
                            txtemphdmf.Text = Val(.Cells("esal_HDMFAmount").Value) / 2
                            txtemphdmf.Text = FormatNumber(txtemphdmf.Text, 2).ToString '.Replace(",", "")

                        End If
                        Exit For
                    End With
                Next
                VIEW_employeetimeentry_SUM(selEmpID,
                                            paypFrom,
                                            paypTo)

                For Each dgvrow As DataGridViewRow In dgvetent.Rows

                    txttotreghrs.Text = "0.00"
                    txttotregamt.Text = "0.00"

                    txttotothrs.Text = "0.00"
                    txttototamt.Text = "0.00"

                    txttotnightdiffhrs.Text = "0.00"
                    txttotnightdiffamt.Text = "0.00"

                    txttotnightdiffothrs.Text = "0.00"
                    txttotnightdiffotamt.Text = "0.00"

                    txttotholidayhrs.Text = "0.00"
                    txttotholidayamt.Text = "0.00"

                    txthrswork.Text = "0.00"
                    txthrsworkamt.Text = "0.00"

                    lblsubtot.Text = "0.00"

                    Dim employeetype As String = dgvEmp.CurrentRow.Cells("Column34").Value

                    With dgvrow
                        If employeetype = "Fixed" Then
                            If dgvEmp.CurrentRow.Cells("Column30").Value = 1 Then
                                If txtgrosssal.Text = "0.00" Then
                                    txtgrosssal.Text = FormatNumber(Val(txtempbasicpay.Text.Replace(",", "")) +
                                    (.Cells("etent_OvertimeHoursAmount").Value) +
                                    (.Cells("etent_NightDiffOTHoursAmount").Value),
                                                                2)
                                End If

                                lblsubtot.Text = txtempbasicpay.Text 'txtgrosssal

                                txthrsworkamt.Text = lblsubtot.Text
                            Else
                                If txtgrosssal.Text = "0.00" Then
                                    txtgrosssal.Text = FormatNumber(Val(txtemptotallow.Text.Replace(",", "")) +
                                    (.Cells("etent_OvertimeHoursAmount").Value) +
                                    (.Cells("etent_NightDiffOTHoursAmount").Value), 2)
                                End If

                                lblsubtot.Text = txtempbasicpay.Text 'txtgrosssal'.Replace(",", "")
                                txthrsworkamt.Text = lblsubtot.Text

                            End If
                        Else
                            lblsubtot.Text = FormatNumber(Val(.Cells("etent_TotalDayPay").Value), 2)

                            txthrsworkamt.Text = FormatNumber(Val(.Cells("etent_TotalDayPay").Value), 2)

                        End If

                        txttotreghrs.Text = Val(.Cells("etent_RegularHoursWorked").Value)
                        txttotregamt.Text = If(IsDBNull(.Cells("etent_RegularHoursAmount").Value), "0.00", FormatNumber(Val(.Cells("etent_RegularHoursAmount").Value), 2))

                        txttotothrs.Text = .Cells("etent_OvertimeHoursWorked").Value
                        txttototamt.Text = FormatNumber(Val(.Cells("etent_OvertimeHoursAmount").Value), 2)

                        txttotnightdiffhrs.Text = .Cells("etent_NightDifferentialHours").Value
                        txttotnightdiffamt.Text = FormatNumber(Val(.Cells("etent_NightDiffHoursAmount").Value), 2)

                        txttotnightdiffothrs.Text = .Cells("etent_NightDifferentialOTHours").Value
                        txttotnightdiffotamt.Text = FormatNumber(Val(.Cells("etent_NightDiffOTHoursAmount").Value), 2)

                        txttotut.Text = .Cells("etent_UndertimeHours").Value
                        txttotutamt.Text = FormatNumber(Val(.Cells("etent_UndertimeHoursAmount").Value), 2)

                        txthrswork.Text = .Cells("etent_TotalHoursWorked").Value

                        txttottardi.Text = .Cells("etent_HoursLate").Value
                        txttottardiamt.Text = FormatNumber(Val(.Cells("etent_HoursLateAmount").Value), 2)

                        Exit For
                    End With
                Next

                COUNT_employeeabsent(selEmpID,
                                    dgvEmp.CurrentRow.Cells("colstartdate").Value,
                                    paypFrom,
                                    paypTo)
            Else

            End If
        Else
            paypFrom = Nothing
            paypTo = Nothing
            paypRowID = Nothing

            txtempbasicpay.Text = "0.00"

            txttotreghrs.Text = "0.00"
            txttotregamt.Text = "0.00"

            txttotothrs.Text = "0.00"
            txttototamt.Text = "0.00"

            txttotnightdiffhrs.Text = "0.00"
            txttotnightdiffamt.Text = "0.00"

            txttotnightdiffothrs.Text = "0.00"
            txttotnightdiffotamt.Text = "0.00"

            txttotholidayhrs.Text = "0.00"
            txttotholidayamt.Text = "0.00"

            txthrswork.Text = "0.00"
            txthrsworkamt.Text = "0.00"

            lblsubtot.Text = "0.00"

            txtemptotallow.Text = "0.00"

            txtemptotbon.Text = "0.00"

            txtgrosssal.Text = "0.00"

            txttotabsent.Text = "0.00"
            txttotabsentamt.Text = "0.00"

            txttottardi.Text = "0.00"
            txttottardiamt.Text = "0.00"

            txttotut.Text = "0.00"
            txttotutamt.Text = "0.00"

            lblsubtotmisc.Text = "0.00"

            txtempsss.Text = "0.00"
            txtempphh.Text = "0.00"
            txtemphdmf.Text = "0.00"

            txttaxabsal.Text = "0.00"

            txtempwtax.Text = "0.00"

            txtemptotloan.Text = "0.00"

            txtnetsal.Text = "0.00"

            vlbal.Text = "0"
            slbal.Text = "0"
            mlbal.Text = "0"

            If dgvEmp.RowCount <> 0 Then

                TextBox8.Text = dgvEmp.CurrentRow.Cells("Column36").Value
                TextBox9.Text = dgvEmp.CurrentRow.Cells("slallowance").Value
                TextBox5.Text = dgvEmp.CurrentRow.Cells("mlallowance").Value

                TextBox13.Text = dgvEmp.CurrentRow.Cells("Column33").Value
                TextBox14.Text = dgvEmp.CurrentRow.Cells("slpayp").Value
                TextBox10.Text = dgvEmp.CurrentRow.Cells("mlpayp").Value
            Else

                TextBox8.Text = "0"
                TextBox9.Text = "0"
                TextBox5.Text = "0"

                TextBox13.Text = "0"
                TextBox14.Text = "0"
                TextBox10.Text = "0"

            End If
        End If
    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked

        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        paypyearnow = paypyearnow + 1

        linkNxt.Text = (Val(paypyearnow) + 1) & " →"
        linkPrev.Text = "← " & (Val(paypyearnow) - 1)

        VIEW_payperiodofyear(paypyearnow)

        AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        dgvpayper_SelectionChanged(sender, e)

    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked

        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        paypyearnow = paypyearnow - 1

        linkPrev.Text = "← " & (Val(paypyearnow) - 1)
        linkNxt.Text = (Val(paypyearnow) + 1) & " →"

        VIEW_payperiodofyear(paypyearnow)

        AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
        dgvpayper_SelectionChanged(sender, e)
    End Sub

    Sub VIEW_payperiodofyear(Optional param_Date As Object = Nothing)
        Dim params(2, 2) As Object

        params(0, 0) = "payp_OrganizationID"
        params(1, 0) = "param_Date"

        params(0, 1) = orgztnID
        params(1, 1) = If(param_Date = Nothing, DBNull.Value, param_Date & "-01-01")

        EXEC_VIEW_PROCEDURE(params,
                            "VIEW_payperiodofyear",
                            dgvpayper)

    End Sub

    Sub VIEW_paystub(Optional EmpID As Object = Nothing,
                     Optional PayPeriodID As Object = Nothing)

        Dim params(2, 2) As Object

        params(0, 0) = "paystb_OrganizationID"
        params(1, 0) = "paystb_EmployeeID"
        params(2, 0) = "paystb_PayPeriodID"

        params(0, 1) = orgztnID
        params(1, 1) = EmpID
        params(2, 1) = PayPeriodID

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_paystub",
                             dgvpaystub, , 1)

    End Sub

    Sub VIEW_paystubitem(ByVal paystitm_PayStubID As Object)

        Dim params(2, 2) As Object

        params(0, 0) = "paystitm_PayStubID"

        params(0, 1) = paystitm_PayStubID

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_paystubitem",
                             dgvpaystubitm, , 1)

    End Sub

    Sub VIEW_specificemployeesalary(Optional esal_EmployeeID As Object = Nothing,
                                    Optional esal_Date As Object = Nothing)

        Dim params(2, 2) As Object

        params(0, 0) = "esal_EmployeeID"
        params(1, 0) = "esal_OrganizationID"
        params(2, 0) = "esal_Date"

        params(0, 1) = esal_EmployeeID
        params(1, 1) = orgztnID
        params(2, 1) = esal_Date

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_specificemployeesalary",
                             dgvempsal, , 1)

    End Sub

    Sub VIEW_employeetimeentry_SUM(Optional etent_EmployeeID As Object = Nothing,
                                   Optional etent_Date As Object = Nothing,
                                   Optional etent_DateTo As Object = Nothing)

        Dim params(3, 2) As Object

        params(0, 0) = "etent_OrganizationID"
        params(1, 0) = "etent_EmployeeID"
        params(2, 0) = "etent_Date"
        params(3, 0) = "etent_DateTo"

        params(0, 1) = orgztnID
        params(1, 1) = etent_EmployeeID
        params(2, 1) = etent_Date
        params(3, 1) = etent_DateTo

        EXEC_VIEW_PROCEDURE(params,
                            "VIEW_employeetimeentry_SUM",
                            dgvetent, , 1)

    End Sub

    Function COUNT_employeeabsent(Optional EmpID As Object = Nothing,
                                  Optional EmpStartDate As Object = Nothing,
                                  Optional payperiodDateFrom As Object = Nothing,
                                  Optional payperiodDateTo As Object = Nothing) As Object

        Dim returnval As Object = Nothing

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("COUNT_employeeabsent", conn)
            conn.Open()
            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("absentcount", MySqlDbType.Decimal)

                .Parameters.AddWithValue("EmpID", EmpID)
                .Parameters.AddWithValue("OrgID", orgztnID)
                .Parameters.AddWithValue("EmpStartDate", Format(CDate(EmpStartDate), "yyyy-MM-dd"))
                .Parameters.AddWithValue("payperiodDateFrom", Format(CDate(payperiodDateFrom), "yyyy-MM-dd"))
                .Parameters.AddWithValue("payperiodDateTo", Format(CDate(payperiodDateTo), "yyyy-MM-dd"))

                .Parameters("absentcount").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

                returnval = If(datread.Read = True, If(IsDBNull(datread.GetString(0)), "0.00", datread.GetString(0).ToString), "0.00") 'dr.GetString(0).ToString

            End With
        Catch ex As Exception
            MsgBox(ex.Message, , "Error : COUNT_employeeabsent")

        End Try

        Return returnval

    End Function

    Sub SplitContainer3_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer3.SplitterMoved

        PanelPayslip.Focus()
    End Sub

    'tsbtnprintpayslip.Click

    Private Sub tsbtnprintpayslip_Click(sender As Object, e As EventArgs) Handles tsbtnprintpayslip.Click
        Dim papy_str As String = Nothing

        Try

            Dim rptdoc As New HalfPaySlip

            With rptdoc.ReportDefinition.Sections(2)

                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtempbasicpay")
                objText.Text = " ₱ " & txtempbasicpay.Text

                objText = .ReportObjects("OrgName")
                objText.Text = orgNam

                objText = .ReportObjects("OrgAddress")
                objText.Text = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" &
                                        ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" &
                                        ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" &
                                        ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" &
                                        ",IF(Country IS NULL,'',CONCAT(', ',Country))" &
                                        ",IF(State IS NULL,'',CONCAT(', ',State)))" &
                                        " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                        " WHERE o.RowID=" & orgztnID & ";")

                Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                        ",',',COALESCE(FaxNumber,'')" &
                                        ",',',COALESCE(EmailAddress,'')" &
                                        ",',',COALESCE(TINNo,''))" &
                                        " FROM organization WHERE RowID=" & orgztnID & ";")

                Dim contactdet = Split(contactdetails, ",")

                objText = .ReportObjects("OrgContact")
                If Trim(contactdet(0).ToString) = "" Then
                Else
                    objText.Text = "Contact No. " & contactdet(0).ToString
                End If

                objText = .ReportObjects("payperiod")
                papy_str = "Payroll slip for the period of   " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))
                objText.Text = papy_str

                objText = .ReportObjects("txtFName")
                objText.Text = StrConv(LastFirstMidName, VbStrConv.Uppercase) 'txtFName.Text

                objText = .ReportObjects("txtEmpID")
                objText.Text = dgvEmp.CurrentRow.Cells("Column1").Value 'txtEmpID.Text

                objText = .ReportObjects("txttotreghrs")
                objText.Text = txttotreghrs.Text

                objText = .ReportObjects("txttotregamt")
                objText.Text = "₱ " & txttotregamt.Text

                objText = .ReportObjects("txttotothrs")
                objText.Text = txttotothrs.Text

                objText = .ReportObjects("txttototamt")
                objText.Text = "₱ " & txttototamt.Text

                objText = .ReportObjects("txttotnightdiffhrs")
                objText.Text = txttotnightdiffhrs.Text

                objText = .ReportObjects("txttotnightdiffamt")
                objText.Text = "₱ " & txttotnightdiffamt.Text

                objText = .ReportObjects("txttotnightdiffothrs")
                objText.Text = txttotnightdiffothrs.Text

                objText = .ReportObjects("txttotnightdiffotamt")
                objText.Text = "₱ " & txttotnightdiffotamt.Text

                objText = .ReportObjects("txttotholidayhrs")
                objText.Text = txttotholidayhrs.Text

                objText = .ReportObjects("txttotholidayamt")
                objText.Text = "₱ " & txttotholidayamt.Text
                '₱
                objText = .ReportObjects("txthrswork")
                objText.Text = txthrswork.Text

                objText = .ReportObjects("txthrsworkamt")
                objText.Text = "₱ " & txthrsworkamt.Text

                objText = .ReportObjects("lblsubtot")
                objText.Text = "₱ " & lblsubtot.Text

                objText = .ReportObjects("txtemptotallow")
                objText.Text = "₱ " & txtemptotallow.Text

                objText = .ReportObjects("txtgrosssal")
                objText.Text = "₱ " & txtgrosssal.Text

                objText = .ReportObjects("txtvlbal")
                objText.Text = vlbal.Text

                objText = .ReportObjects("txtslbal")
                objText.Text = slbal.Text

                objText = .ReportObjects("txtmlbal")
                objText.Text = mlbal.Text

                objText = .ReportObjects("txtothlbal")
                objText.Text = 0

                For Each dgvrow As DataGridViewRow In dgvpaystubitm.Rows

                    If dgvrow.Cells("Item").Value = "Others" Then

                        objText.Text = Val(dgvrow.Cells("PayAmount").Value)

                        Exit For
                    End If
                Next

                objText = .ReportObjects("txttotabsent")
                objText.Text = txttotabsent.Text

                objText = .ReportObjects("txttotabsentamt")
                objText.Text = "₱ " & txttotabsentamt.Text

                objText = .ReportObjects("txttottardi")
                objText.Text = txttottardi.Text

                objText = .ReportObjects("txttottardiamt")
                objText.Text = "₱ " & txttottardiamt.Text

                objText = .ReportObjects("txttotut")
                objText.Text = txttotut.Text

                objText = .ReportObjects("txttotutamt")
                objText.Text = "₱ " & txttotutamt.Text

                Dim misc_subtot = Val(txttottardiamt.Text) + Val(txttotutamt.Text)

                objText = .ReportObjects("lblsubtotmisc")
                objText.Text = "₱ " & FormatNumber(Val(misc_subtot), 2).ToString.Replace(",", "")

                objText = .ReportObjects("txtempsss")
                objText.Text = "₱ " & txtempsss.Text

                objText = .ReportObjects("txtempphh")
                objText.Text = "₱ " & txtempphh.Text

                objText = .ReportObjects("txtemphdmf")
                objText.Text = "₱ " & txtemphdmf.Text

                objText = .ReportObjects("txtemptotloan")
                objText.Text = "₱ " & txtemptotloan.Text

                objText = .ReportObjects("txtemptotbon")
                objText.Text = "₱ " & txtemptotbon.Text

                objText = .ReportObjects("txttaxabsal")
                objText.Text = "₱ " & txttaxabsal.Text

                objText = .ReportObjects("txtempwtax")
                objText.Text = "₱ " & txtempwtax.Text

                objText = .ReportObjects("txtnetsal")
                objText.Text = "₱ " & txtnetsal.Text

                objText = .ReportObjects("allowsubdetails")

                If dgvEmp.RowCount <> 0 Then

                    VIEW_eallow_indate(dgvEmp.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                    VIEW_eloan_indate(dgvEmp.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                    VIEW_ebon_indate(dgvEmp.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                    Dim allowvalues As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("allowvalues")

                    For Each dgvrow As DataGridViewRow In dgvempallowans.Rows
                        If dgvrow.Index = 0 Then
                            objText.Text = dgvrow.Cells("eallw_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                            allowvalues.Text = "₱ " & FormatNumber(Val(dgvrow.Cells("eallw_Amount").Value), 2)
                        Else
                            objText.Text &= vbNewLine & dgvrow.Cells("eallw_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                            allowvalues.Text &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("eallw_Amount").Value), 2)

                            Dim strtxt = dgvrow.Cells("eallw_Type").Value & vbTab & "₱ " & dgvrow.Cells("eallw_Amount").Value

                            If strtxt.ToString.Length < objText.Text.Length Then
                                Dim lengthdiff = strtxt.ToString.Length - objText.Text.Length
                            Else

                            End If

                        End If
                    Next

                    objText = .ReportObjects("loansubdetails")

                    Dim loanvalues As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("loanvalues")

                    For Each dgvrow As DataGridViewRow In dgvemploan.Rows
                        If dgvrow.Index = 0 Then
                            objText.Text = dgvrow.Cells("cloan_loantype").Value ' & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

                            loanvalues.Text = "₱ " & FormatNumber(Val(dgvrow.Cells("cloan_dedamt").Value), 2)
                        Else
                            objText.Text &= vbNewLine & dgvrow.Cells("cloan_loantype").Value ' & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

                            loanvalues.Text &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("cloan_dedamt").Value), 2)

                            Dim strtxt = dgvrow.Cells("cloan_loantype").Value & vbTab & "₱ " & dgvrow.Cells("cloan_dedamt").Value

                            If strtxt.ToString.Length < objText.Text.Length Then
                                Dim lengthdiff = strtxt.ToString.Length - objText.Text.Length
                            Else

                            End If

                        End If
                    Next

                    objText = .ReportObjects("bonsubdetails")

                    Dim bonvalues As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("bonvalues")

                    For Each dgvrow As DataGridViewRow In dgvempbonus.Rows
                        If dgvrow.Index = 0 Then
                            objText.Text = dgvrow.Cells("bons_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                            bonvalues.Text = "₱ " & FormatNumber(Val(dgvrow.Cells("bons_Amount").Value), 2)
                        Else
                            objText.Text &= vbNewLine & dgvrow.Cells("bons_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                            bonvalues.Text &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("bons_Amount").Value), 2)

                            Dim strtxt = dgvrow.Cells("bons_Type").Value & vbTab & "₱ " & dgvrow.Cells("bons_Amount").Value

                            If strtxt.ToString.Length < objText.Text.Length Then
                                Dim lengthdiff = strtxt.ToString.Length - objText.Text.Length
                            Else

                            End If

                        End If
                    Next
                End If
            End With

            Dim crvwr As New CrysVwr
            crvwr.CrystalReportViewer1.ReportSource = rptdoc

            crvwr.Text = papy_str & ", ID# " & dgvEmp.CurrentRow.Cells("RowID").Value & ", " & txtFName.Text
            crvwr.Refresh()
            crvwr.Show() '
            'TINNo
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        End Try
    End Sub

    Private Sub tsbtnprintall_Click(sender As Object, e As EventArgs) Handles tsbtnprintall.Click
        Try

            Dim pay_stbitem As New DataTable 'this is for leave balances

            pay_stbitem = retAsDatTbl("SELECT" &
                                      " pi.PayStubID" &
                                      ",pi.ProductID" &
                                      ",p.PartNo" &
                                      ",pi.PayAmount" &
                                      " FROM paystubitem pi" &
                                      " LEFT JOIN product p ON p.RowID = pi.ProductID" &
                                      " LEFT JOIN paystub ps ON ps.RowID = pi.PayStubID" &
                                      " WHERE p.Category='Leave Type'" &
                                      " AND p.OrganizationID=" & orgztnID &
                                      " AND ps.PayPeriodID='" & paypRowID & "';") 'this is for leave balances

            Dim rptdattab As New DataTable

            With rptdattab.Columns

                .Add("Column1", Type.GetType("System.Int32"))
                .Add("Column2", Type.GetType("System.String"))
                .Add("Column3") 'Employee Full Name

                .Add("Column4") 'Gross Income

                .Add("Column5") 'Net Income
                .Add("Column6") 'Taxable salary

                .Add("Column7") 'Withholding Tax
                .Add("Column8") 'Total Allowance

                .Add("Column9") 'Total Loans
                .Add("Column10") 'Total Bonuses

                .Add("Column11") 'Basic Pay
                .Add("Column12") 'SSS Amount

                .Add("Column13") 'PhilHealth Amount
                .Add("Column14") 'PAGIBIG Amount

                .Add("Column15") 'Sub Total - Right side
                .Add("Column16") 'txthrsworkamt

                .Add("Column17") 'Regular hours worked

                .Add("Column18") 'Regular hours amount

                .Add("Column19") 'Overtime hours worked

                .Add("Column20") 'Overtime hours amount
                .Add("Column21") 'Night differential hours worked
                .Add("Column22") 'Night differential hours amount

                .Add("Column23") 'Night differential OT hours worked
                .Add("Column24") 'Night differential OT hours amount

                .Add("Column25") 'Total hours worked

                .Add("Column26") 'Undertime hours

                .Add("Column27") 'Undertime amount
                .Add("Column28") 'Late hours

                .Add("Column29") 'Late amount

                .Add("Column30") 'Leave type
                .Add("Column31") 'Leave count
                .Add("Column32")

                .Add("Column33")

                .Add("Column34") 'Allowance type
                .Add("Column35") 'Loan type
                .Add("Column36") 'Bonus type

                .Add("Column37") 'Allowance amount
                .Add("Column38") 'Loan amount
                .Add("Column39") 'Bonus amount

            End With

            Dim employee_dattab As New DataTable

            employee_dattab = retAsDatTbl("SELECT e.* FROM" &
                                          " employee e LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID" &
                                          " WHERE e.OrganizationID=" & orgztnID &
                                          " AND '" & paypTo & "' BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,'" & paypTo & "')" &
                                          " GROUP BY e.RowID" &
                                          " ORDER BY e.RowID DESC;")

            Dim newdatrow As DataRow

            For Each drow As DataRow In employee_dattab.Rows
                newdatrow = rptdattab.NewRow

                newdatrow("Column1") = drow("RowID") 'Employee RowID
                newdatrow("Column2") = drow("EmployeeID") 'Employee ID

                Dim full_name = drow("FirstName").ToString & If(drow("MiddleName").ToString = Nothing,
                                                            "",
                                                            " " & StrConv(Microsoft.VisualBasic.Left(drow("MiddleName").ToString, 1),
                                                            VbStrConv.ProperCase) & ".")

                full_name = full_name & " " & drow("LastName").ToString

                full_name = full_name & If(drow("Surname").ToString = Nothing,
                                        "",
                                        "-" & StrConv(Microsoft.VisualBasic.Left(drow("Surname").ToString, 1),
                                        VbStrConv.ProperCase))

                newdatrow("Column3") = full_name 'Employee Full Name

                VIEW_paystub(drow("RowID").ToString,
                                     paypRowID)

                Dim totamountallow = 0.0
                Dim totamountbon = 0.0

                For Each dgvrow As DataGridViewRow In dgvpaystub.Rows
                    With dgvrow

                        newdatrow("Column4") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalGrossSalary").Value), 2) 'Gross Income

                        Dim gros_inc = Val(newdatrow("Column4"))

                        newdatrow("Column5") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalNetSalary").Value), 2) 'Net Income

                        newdatrow("Column6") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalTaxableSalary").Value), 2) 'Taxable salary

                        newdatrow("Column7") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalEmpWithholdingTax").Value), 2) 'Withholding Tax

                        newdatrow("Column8") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalAllowance").Value), 2) 'Total Allowance

                        totamountallow = Val(.Cells("paystb_TotalAllowance").Value)

                        newdatrow("Column9") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalLoans").Value), 2) 'Total Loans
                        newdatrow("Column10") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalBonus").Value), 2) 'Total Bonuses

                        totamountbon = Val(.Cells("paystb_TotalBonus").Value)

                        Dim selpay_stbitem = pay_stbitem.Select("PayStubID = " & .Cells("paystb_RowID").Value)

                        Dim firstRow = 0

                        Dim isStrListed As New List(Of String)

                        For Each datrow In selpay_stbitem 'this is for leave balances

                            Dim leavebalance = Val(datrow("PayAmount"))

                            If firstRow = 0 Then
                                If isStrListed.Contains(datrow("PartNo")) Then
                                Else
                                    newdatrow("Column30") = datrow("PartNo")

                                    newdatrow("Column31") = Val(datrow("PayAmount"))

                                    isStrListed.Add(datrow("PartNo"))
                                End If
                            Else
                                If isStrListed.Contains(datrow("PartNo")) Then
                                Else
                                    newdatrow("Column30") &= vbNewLine & datrow("PartNo")

                                    newdatrow("Column31") &= vbNewLine & Val(datrow("PayAmount"))

                                    isStrListed.Add(datrow("PartNo"))
                                End If
                            End If

                            firstRow += 1

                        Next 'this is for leave balances
                        isStrListed.Clear()
                    End With
                    Exit For
                Next

                VIEW_specificemployeesalary(drow("RowID").ToString,
                                            paypTo)

                Dim theEmpBasicPayFix = 0.0

                For Each dgvrow As DataGridViewRow In dgvempsal.Rows
                    With dgvrow
                        newdatrow("Column11") = "₱ " & FormatNumber(Val(.Cells("esal_BasicPay").Value), 2) 'Basic Pay

                        theEmpBasicPayFix = Val(.Cells("esal_BasicPay").Value) 'Basic Pay

                        If isorgSSSdeductsched = 1 Then
                            newdatrow("Column12") = "₱ " & FormatNumber((Val(.Cells("esal_EmployeeContributionAmount").Value) / 2), 2)
                        Else
                            If isEndOfMonth = "0" Then
                                newdatrow("Column12") = "₱ " & "0.00" 'SSS Amount
                            Else
                                newdatrow("Column12") = "₱ " & FormatNumber(Val(.Cells("esal_EmployeeContributionAmount").Value), 2) 'SSS Amount
                            End If
                            'SSS Amount
                        End If

                        If isorgPHHdeductsched = 1 Then
                            newdatrow("Column13") = "₱ " & FormatNumber((Val(.Cells("esal_EmployeeShare").Value) / 2), 2) 'PhilHealth Amount
                        Else
                            If isEndOfMonth = "0" Then
                                newdatrow("Column13") = "₱ " & "0.00" 'PhilHealth Amount
                            Else
                                newdatrow("Column13") = "₱ " & FormatNumber(Val(.Cells("esal_EmployeeShare").Value), 2) 'PhilHealth Amount
                            End If
                        End If

                        If isorgHDMFdeductsched = 1 Then
                            newdatrow("Column14") = "₱ " & FormatNumber((Val(.Cells("esal_HDMFAmount").Value) / 2), 2) 'PAGIBIG Amount
                        Else
                            If isEndOfMonth = "0" Then
                                newdatrow("Column14") = "₱ " & "0.00" 'PAGIBIG Amount
                            Else
                                newdatrow("Column14") = "₱ " & FormatNumber((Val(.Cells("esal_HDMFAmount").Value)), 2) 'PAGIBIG Amount
                            End If

                        End If
                        Exit For
                    End With
                Next

                VIEW_employeetimeentry_SUM(drow("RowID").ToString,
                                            paypFrom,
                                            paypTo)

                For Each dgvrow As DataGridViewRow In dgvetent.Rows
                    With dgvrow

                        If drow("EmployeeType").ToString = "Fixed" Then

                            Dim validgrossinc = Val(newdatrow("Column4"))

                            If Val(.Cells("etent_TotalDayPay").Value) = 0 Then
                                If drow("PayFrequencyID").ToString = 1 Then
                                    ''------------------------------------------------ITO UNG BASIC PAY

                                    'newdatrow("Column4") = "₱ " & FormatNumber(Val(newdatrow("Column11")) + _
                                    '(.Cells("etent_OvertimeHoursAmount").Value) + _
                                    '(.Cells("etent_NightDiffOTHoursAmount").Value), _
                                    '2)

                                    newdatrow("Column4") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") '.ToString.Replace(",", "") 'Gross Income

                                    newdatrow("Column15") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'Sub Total - Right side

                                    newdatrow("Column16") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'txthrsworkamt
                                Else
                                    Dim totbasicpay = Val(totamountallow) +
                                    Val(.Cells("etent_OvertimeHoursAmount").Value) +
                                    Val(.Cells("etent_NightDiffOTHoursAmount").Value)

                                    newdatrow("Column4") = "₱ " & FormatNumber(totbasicpay, 2) 'newdatrow("Column4") '.ToString.Replace(",", "") 'Gross Income

                                    newdatrow("Column16") = "₱ " & FormatNumber(totbasicpay, 2) 'newdatrow("Column4") 'txthrsworkamt
                                End If
                            Else

                                newdatrow("Column4") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") '.ToString.Replace(",", "") 'Gross Income

                                newdatrow("Column15") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'Sub Total - Right side

                                newdatrow("Column16") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'txthrsworkamt

                            End If
                        Else
                            newdatrow("Column15") = "₱ " & FormatNumber(Val(.Cells("etent_TotalDayPay").Value), 2) 'Sub Total - Right side

                            newdatrow("Column16") = "₱ " & FormatNumber(Val(.Cells("etent_TotalDayPay").Value), 2) 'txthrsworkamt

                        End If

                        newdatrow("Column17") = FormatNumber(Val(.Cells("etent_RegularHoursWorked").Value), 2) 'Regular hours worked
                        newdatrow("Column18") = "₱ " & FormatNumber(Val(.Cells("etent_RegularHoursAmount").Value), 2) 'Regular hours amount

                        newdatrow("Column19") = FormatNumber(Val(.Cells("etent_OvertimeHoursWorked").Value), 2) 'Overtime hours worked
                        newdatrow("Column20") = "₱ " & FormatNumber(Val(.Cells("etent_OvertimeHoursAmount").Value), 2) 'Overtime hours amount

                        newdatrow("Column21") = FormatNumber(Val(.Cells("etent_NightDifferentialHours").Value), 2) 'Night differential hours worked
                        newdatrow("Column22") = "₱ " & FormatNumber(Val(.Cells("etent_NightDiffHoursAmount").Value), 2) 'Night differential hours amount

                        newdatrow("Column23") = FormatNumber(Val(.Cells("etent_NightDifferentialOTHours").Value), 2) 'Night differential OT hours worked
                        newdatrow("Column24") = "₱ " & FormatNumber(Val(.Cells("etent_NightDiffOTHoursAmount").Value), 2) 'Night differential OT hours amount

                        newdatrow("Column25") = FormatNumber(Val(.Cells("etent_TotalHoursWorked").Value), 2) 'Total hours worked

                        newdatrow("Column26") = "₱ " & FormatNumber(Val(.Cells("etent_UndertimeHours").Value), 2) 'Undertime hours
                        newdatrow("Column27") = "₱ " & FormatNumber(Val(.Cells("etent_UndertimeHoursAmount").Value), 2) 'Undertime amount

                        txttotabsent.Text = COUNT_employeeabsent(drow("RowID").ToString,
                                                                 drow("StartDate").ToString,
                                                                 paypFrom,
                                                                 paypTo)

                        newdatrow("Column28") = "₱ " & FormatNumber(Val(.Cells("etent_HoursLate").Value), 2)
                        newdatrow("Column29") = "₱ " & FormatNumber(Val(.Cells("etent_HoursLateAmount").Value), 2)

                        Dim misc_subtot = Val(newdatrow("Column29")) + Val(newdatrow("Column27"))

                        Exit For
                    End With
                Next

                VIEW_eallow_indate(drow("RowID"),
                                    paypFrom,
                                    paypTo)

                VIEW_eloan_indate(drow("RowID"),
                                    paypFrom,
                                    paypTo)

                VIEW_ebon_indate(drow("RowID"),
                                    paypFrom,
                                    paypTo)

                For Each dgvrow As DataGridViewRow In dgvempallowance.Rows 'Allowances
                    If dgvrow.Index = 0 Then
                        newdatrow("Column34") = dgvrow.Cells("eall_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                        newdatrow("Column37") = "₱ " & FormatNumber(Val(dgvrow.Cells("eall_Amount").Value), 2)
                    Else
                        newdatrow("Column34") &= vbNewLine & dgvrow.Cells("eall_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                        newdatrow("Column37") &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("eall_Amount").Value), 2)

                    End If
                Next

                For Each dgvrow As DataGridViewRow In dgvLoanList.Rows 'Loans
                    If dgvrow.Index = 0 Then
                        newdatrow("Column35") = dgvrow.Cells("c_loantype").Value ' & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

                        newdatrow("Column38") = "₱ " & FormatNumber(Val(dgvrow.Cells("c_dedamt").Value), 2)
                    Else
                        newdatrow("Column35") &= vbNewLine & dgvrow.Cells("c_loantype").Value ' & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

                        newdatrow("Column38") &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("c_dedamt").Value), 2)

                    End If
                Next

                For Each dgvrow As DataGridViewRow In dgvempbon.Rows 'Bonuses
                    If dgvrow.Index = 0 Then
                        newdatrow("Column36") = dgvrow.Cells("bon_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                        newdatrow("Column39") = "₱ " & FormatNumber(Val(dgvrow.Cells("bon_Amount").Value), 2)
                    Else
                        newdatrow("Column36") &= vbNewLine & dgvrow.Cells("bon_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                        newdatrow("Column39") &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("bon_Amount").Value), 2)

                    End If
                Next

                rptdattab.Rows.Add(newdatrow)

            Next

            Dim rptdoc As New prntAllPaySlip

            With rptdoc.ReportDefinition.Sections(2)
                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("OrgName1")

                objText.Text = orgNam

                objText = .ReportObjects("OrgName")

                objText.Text = orgNam

                objText = .ReportObjects("OrgAddress1")

                Dim orgaddress = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" &
                                        ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" &
                                        ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" &
                                        ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" &
                                        ",IF(Country IS NULL,'',CONCAT(', ',Country))" &
                                        ",IF(State IS NULL,'',CONCAT(', ',State)))" &
                                        " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                        " WHERE o.RowID=" & orgztnID & ";")

                objText.Text = orgaddress

                objText = .ReportObjects("OrgAddress")

                objText.Text = orgaddress

                Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                        ",',',COALESCE(FaxNumber,'')" &
                                        ",',',COALESCE(EmailAddress,'')" &
                                        ",',',COALESCE(TINNo,''))" &
                                        " FROM organization WHERE RowID=" & orgztnID & ";")

                Dim contactdet = Split(contactdetails, ",")

                objText = .ReportObjects("OrgContact1")

                Dim contactdets As String = Nothing

                If Trim(contactdet(0).ToString) = "" Then
                    contactdets = ""
                Else
                    contactdets = "Contact No. " & contactdet(0).ToString
                End If

                objText.Text = contactdets

                objText = .ReportObjects("OrgContact")

                objText.Text = contactdets

                objText = .ReportObjects("payperiod1")

                Dim papy_str = "Payroll slip for the period of   " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))

                objText.Text = papy_str

                objText = .ReportObjects("payperiod")

                objText.Text = papy_str

            End With

            rptdoc.SetDataSource(rptdattab)

            Dim crvwr As New CrysVwr
            crvwr.CrystalReportViewer1.ReportSource = rptdoc

            Dim papy_string = "Print all pay slip for the period of " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))

            crvwr.Text = papy_string
            crvwr.Refresh()
            crvwr.Show()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        End Try

    End Sub

    Sub VIEW_eallow_indate(Optional eallow_EmployeeID As Object = Nothing,
                               Optional datefrom As Object = Nothing,
                               Optional dateto As Object = Nothing)

        Dim param(4, 2) As Object

        param(0, 0) = "eallow_EmployeeID"
        param(1, 0) = "eallow_OrganizationID"
        param(2, 0) = "effectivedatefrom"
        param(3, 0) = "effectivedateto"
        param(4, 0) = "numweekdays"

        param(0, 1) = eallow_EmployeeID
        param(1, 1) = orgztnID
        param(2, 1) = datefrom
        param(3, 1) = If(dateto = Nothing, DBNull.Value, dateto)
        param(4, 1) = Val(numofweekdays)

        EXEC_VIEW_PROCEDURE(param,
                           "VIEW_employeeallowance_indate",
                           dgvempallowans, , 1)

    End Sub

    Sub VIEW_eloan_indate(Optional eloan_EmployeeID As Object = Nothing,
                               Optional datefrom As Object = Nothing,
                               Optional dateto As Object = Nothing)

        Dim params(3, 2) As Object

        params(0, 0) = "eloan_EmployeeID"
        params(1, 0) = "eloan_OrganizationID"
        params(2, 0) = "effectivedatefrom"
        params(3, 0) = "effectivedateto"

        params(0, 1) = eloan_EmployeeID
        params(1, 1) = orgztnID
        params(2, 1) = datefrom
        params(3, 1) = dateto

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_employeeloan_indate",
                             dgvemploan)

    End Sub

    Sub VIEW_ebon_indate(Optional ebon_EmployeeID As Object = Nothing,
                               Optional datefrom As Object = Nothing,
                               Optional dateto As Object = Nothing)

        Dim params(3, 2) As Object

        params(0, 0) = "ebon_EmployeeID"
        params(1, 0) = "ebon_OrganizationID"
        params(2, 0) = "effectivedatefrom"
        params(3, 0) = "effectivedateto"

        params(0, 1) = ebon_EmployeeID
        params(1, 1) = orgztnID
        params(2, 1) = datefrom
        params(3, 1) = dateto

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_employeebonus_indate",
                             dgvempbonus)

    End Sub

#End Region 'Pay slip

#Region "Employee Allowance"

    Public categallowID As Object = Nothing

    Private Sub tbpempallow_Click(sender As Object, e As EventArgs) Handles tbpempallow.Click

    End Sub

    Public allowance_type As New AutoCompleteStringCollection

    Dim view_IDAllow As Integer = Nothing

    Sub tbpempallow_Enter(sender As Object, e As EventArgs) Handles tbpempallow.Enter

        tabpageText(tabIndx)

        tbpempallow.Text = "ALLOWANCE               "

        Label25.Text = "ALLOWANCE"
        Static once As SByte = 0

        If once = 0 Then
            once = 1

            txtallowamt.ContextMenu = New ContextMenu

            cboallowfreq.ContextMenu = New ContextMenu

            cboallowtype.ContextMenu = New ContextMenu

            categallowID = EXECQUER("SELECT RowID FROM category WHERE OrganizationID=" & orgztnID & " AND CategoryName='" & "Allowance Type" & "' LIMIT 1;")

            If Val(categallowID) = 0 Then
                categallowID = INSUPD_category(, "Allowance Type")
            End If

            enlistTheLists("SELECT CONCAT(COALESCE(p.PartNo,''),'@',p.RowID)" &
                           " FROM product p" &
                           " INNER JOIN category c ON c.RowID=p.CategoryID" &
                           " WHERE c.CategoryName='Allowance Type'" &
                           " AND p.OrganizationID='" & orgztnID & "' AND p.ActiveData='1';",
                           allowance_type) 'cboallowtype

            For Each strval In allowance_type
                cboallowtype.Items.Add(getStrBetween(strval, "", "@"))
                eall_Type.Items.Add(getStrBetween(strval, "", "@"))
            Next

            enlistToCboBox("SELECT DisplayValue FROM listofval WHERE Type='Allowance Frequency' AND Active='Yes' ORDER BY OrderBy;",
                           cboallowfreq)

            For Each strval In cboallowfreq.Items
                eall_Frequency.Items.Add(strval)
            Next

            AddHandler dgvempallowance.SelectionChanged, AddressOf dgvempallowance_SelectionChanged

            view_IDAllow = VIEW_privilege("Employee Allowance", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDAllow)

            If formuserprivilege.Count = 0 Then

                tsbtnNewAllowance.Visible = 0
                tsbtnSaveAllowance.Visible = 0
                tsbtnDelAllowance.Visible = False
                dontUpdateAllow = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        tsbtnNewAllowance.Visible = 0
                        tsbtnSaveAllowance.Visible = 0
                        tsbtnDelAllowance.Visible = False
                        dontUpdateAllow = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            tsbtnNewAllowance.Visible = 0
                        Else
                            tsbtnNewAllowance.Visible = 1
                        End If

                        If drow("Deleting").ToString = "N" Then
                            tsbtnDelAllowance.Visible = 0
                        Else
                            tsbtnDelAllowance.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateAllow = 1
                        Else
                            dontUpdateAllow = 0
                        End If

                    End If

                Next

            End If

        End If

        tabIndx = 13 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub tsbtnNewAllowance_Click(sender As Object, e As EventArgs) Handles tsbtnNewAllowance.Click
        dgvempallowance.EndEdit(1)
        dgvempallowance.Focus()

        For Each dgvrow As DataGridViewRow In dgvempallowance.Rows
            If dgvrow.IsNewRow Then
                dgvallowRowindx = dgvrow.Index
                dgvrow.Cells("eall_Type").Selected = 1
                Exit For
            End If
        Next

        dgvempallowance_SelectionChanged(sender, e)

    End Sub

    Dim dontUpdateAllow As SByte = 0

    Private Sub tsbtnSaveAllowance_Click(sender As Object, e As EventArgs) Handles tsbtnSaveAllowance.Click
        pbEmpPicAllow.Focus()

        cboallowtype.Focus()

        pbEmpPicAllow.Focus()

        cboallowfreq.Focus()

        pbEmpPicAllow.Focus()

        dtpallowstartdate.Focus()

        pbEmpPicAllow.Focus()

        dtpallowenddate.Focus()

        pbEmpPicAllow.Focus()

        txtallowamt.Focus()

        pbEmpPicAllow.Focus()

        dgvempallowance.EndEdit(1)

        If dontUpdateAllow = 1 Then
            listofEditEmpAllow.Clear()
        End If

        If dgvEmp.RowCount = 0 Then
            Exit Sub
        ElseIf empallow_daterangehasrecord = 1 Then
            dtpallowstartdate.Focus()
            WarnBalloon("Please supply a valid date range.", "Invalid date range", lblforballoon, 0, -69)
            Exit Sub
        End If

        Static once As SByte = 0

        For Each dgvrow As DataGridViewRow In dgvempallowance.Rows
            With dgvrow
                If .IsNewRow = 0 Then
                    If listofEditEmpAllow.Contains(.Cells("eall_RowID").Value) Then

                        INSUPD_employeeallowance(.Cells("eall_RowID").Value,
                                                 dgvEmp.CurrentRow.Cells("RowID").Value,
                                                 .Cells("eall_Frequency").Value,
                                                 .Cells("eall_Start").Value,
                                                 .Cells("eall_End").Value,
                                                 .Cells("eall_Amount").Value,
                                                 .Cells("eall_ProdID").Value)
                    Else
                        If .Cells("eall_RowID").Value = Nothing And
                            tsbtnNewAllowance.Visible = True Then

                            .Cells("eall_RowID").Value = INSUPD_employeeallowance(,
                                                     dgvEmp.CurrentRow.Cells("RowID").Value,
                                                     .Cells("eall_Frequency").Value,
                                                     .Cells("eall_Start").Value,
                                                     .Cells("eall_End").Value,
                                                     .Cells("eall_Amount").Value,
                                                     .Cells("eall_ProdID").Value)
                        End If

                    End If
                End If

            End With
        Next

        listofEditEmpAllow.Clear()

        If hasERR = 0 Then

            InfoBalloon("Changes made in Employee ID '" & dgvEmp.CurrentRow.Cells("Column1").Value & "' has successfully saved.", "Changes successfully save", lblforballoon, 0, -69)
        Else
            dgvEmp_SelectionChanged(sender, e)

        End If

    End Sub

    Function INSUPD_employeeallowance(Optional eall_RowID As Object = Nothing,
                                      Optional eall_EmployeeID As Object = Nothing,
                                      Optional eall_AllowanceFrequency As Object = Nothing,
                                      Optional eall_EffectiveStartDate As Object = Nothing,
                                      Optional eall_EffectiveEndDate As Object = Nothing,
                                      Optional eall_Amount As Object = Nothing,
                                      Optional eall_ProductID As Object = Nothing) As Object

        Dim params(9, 2) As Object

        params(0, 0) = "eall_RowID"
        params(1, 0) = "eall_OrganizationID"
        params(2, 0) = "eall_EmployeeID"
        params(3, 0) = "eall_CreatedBy"
        params(4, 0) = "eall_LastUpdBy"
        params(5, 0) = "eall_ProductID"
        params(6, 0) = "eall_AllowanceFrequency"
        params(7, 0) = "eall_EffectiveStartDate"
        params(8, 0) = "eall_EffectiveEndDate"
        params(9, 0) = "eall_Amount"

        params(0, 1) = If(eall_RowID = Nothing, DBNull.Value, eall_RowID)
        params(1, 1) = orgztnID
        params(2, 1) = eall_EmployeeID
        params(3, 1) = z_User
        params(4, 1) = z_User
        params(5, 1) = eall_ProductID
        params(6, 1) = eall_AllowanceFrequency
        params(7, 1) = Format(CDate(eall_EffectiveStartDate), "yyyy-MM-dd")
        params(8, 1) = If(eall_EffectiveEndDate = Nothing, DBNull.Value, Format(CDate(eall_EffectiveEndDate), "yyyy-MM-dd"))
        params(9, 1) = eall_Amount

        INSUPD_employeeallowance = EXEC_INSUPD_PROCEDURE(params,
                                                          "INSUPD_employeeallowance",
                                                          "eallow_RowID")

    End Function

    Private Sub ToolStripButton10_Click(sender As Object, e As EventArgs) Handles ToolStripButton10.Click
        listofEditEmpAllow.Clear()

        RemoveHandler dtpallowstartdate.ValueChanged, AddressOf dtpallowstartdate_ValueChanged
        RemoveHandler dtpallowenddate.ValueChanged, AddressOf dtpallowenddate_ValueChanged

        dgvEmp_SelectionChanged(sender, e)
    End Sub

    Private Sub cboallowfreq_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboallowfreq.KeyPress, cboallowtype.KeyPress

        e.Handled = True

    End Sub

    Private Sub cboallowfreq_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboallowfreq.SelectedIndexChanged ', cboallowfreq.SelectedIndexChanged

    End Sub

    Private Sub cboallowfreq_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboallowfreq.SelectedValueChanged  ', cboallowfreq.SelectedIndexChanged

        dtpallowstartdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]

        Select Case cboallowfreq.SelectedIndex

            Case 0 To 1 'Daily & Monthly
                dtpallowstartdate.Enabled = 1
                dtpallowenddate.Enabled = 1

                lblreqstartdate.Visible = 1
                lblreqenddate.Visible = 1

            Case 2 'One time
                dtpallowstartdate.Enabled = 1
                dtpallowenddate.Enabled = 0

                lblreqstartdate.Visible = 1
                lblreqenddate.Visible = 0
            Case 3 To 4 'Semi-monthly & Weekly
                dtpallowstartdate.Enabled = 1
                dtpallowenddate.Enabled = 1

                lblreqstartdate.Visible = 1
                lblreqenddate.Visible = 1

            Case Else 'Nothing
                dtpallowstartdate.Enabled = 0
                dtpallowenddate.Enabled = 0

                lblreqstartdate.Visible = 0
                lblreqenddate.Visible = 0

        End Select

    End Sub

    Private Sub cboallowfreq_GotFocus(sender As Object, e As EventArgs) Handles cboallowfreq.GotFocus

        If dgvempallowance.RowCount = 1 Then
        Else
            dgvempallowance.Item("eall_Frequency", dgvallowRowindx).Selected = 1
        End If

    End Sub

    Private Sub cboallowfreq_Leave(sender As Object, e As EventArgs) Handles cboallowfreq.Leave ', cboallowfreq.SelectedIndexChanged

        Dim thegetval = Trim(cboallowfreq.Text)

        If dgvempallowance.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempallowance.Rows.Add()
                dgvallowRowindx = dgvempallowance.RowCount - 2

            End If
        Else
            If dgvempallowance.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempallowance.Rows.Add()
                    dgvallowRowindx = dgvempallowance.RowCount - 2

                End If
            Else
                dgvallowRowindx = dgvempallowance.CurrentRow.Index

            End If
        End If

        If thegetval <> "" Then
            dgvempallowance.Item("eall_Frequency", dgvallowRowindx).Value = thegetval

            cboallowfreq.Text = thegetval

        End If

        With dgvempallowance.Rows(dgvallowRowindx)
            If Val(.Cells("eall_RowID").Value) <> 0 Then
                If thegetval <> allowance_prevval(1) Then
                    listofEditEmpAllow.Add(.Cells("eall_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Dim dgvallowRowindx As Integer = 0

    Dim allowProductID As String = Nothing

    Dim statictabindxempallow As SByte = -1

    Private Sub cboallowtype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboallowtype.SelectedIndexChanged

    End Sub

    Private Sub cboallowtype_GotFocus(sender As Object, e As EventArgs) Handles cboallowtype.GotFocus

        If dgvempallowance.RowCount = 1 Then
        Else
            dgvempallowance.Item("eall_type", dgvallowRowindx).Selected = 1
        End If

    End Sub

    Private Sub cboallowtype_Leave(sender As Object, e As EventArgs) Handles cboallowtype.Leave 'cboallowtype.SelectedIndexChanged

        For Each strval In allowance_type
            If Trim(cboallowtype.Text) = getStrBetween(strval, "", "@") Then
                allowProductID = StrReverse(getStrBetween(StrReverse(strval), "", "@"))
                Exit For
            End If
        Next

        Dim thegetval = Trim(cboallowtype.Text)

        If dgvempallowance.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempallowance.Rows.Add()
                dgvallowRowindx = dgvempallowance.RowCount - 2

            End If
        Else
            If dgvempallowance.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempallowance.Rows.Add()
                    dgvallowRowindx = dgvempallowance.RowCount - 2

                End If
            Else
                dgvallowRowindx = dgvempallowance.CurrentRow.Index

            End If
        End If

        If thegetval <> "" Then
            dgvempallowance.Item("eall_type", dgvallowRowindx).Value = thegetval
            dgvempallowance.Item("eall_ProdID", dgvallowRowindx).Value = allowProductID

            cboallowtype.Text = thegetval

        End If

        With dgvempallowance.Rows(dgvallowRowindx)
            If Val(.Cells("eall_RowID").Value) <> 0 Then
                If thegetval <> allowance_prevval(0) Then
                    listofEditEmpAllow.Add(.Cells("eall_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Private Sub dtpallowstartdate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtpallowstartdate.KeyPress ', dtpallowenddate.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        Static j As SByte = 0
        Static a As SByte = 0
        Static m As SByte = 0

        Dim MM = Format(CDate(dtpallowstartdate.Value), "MMM")
        Dim dd = CDate(dtpallowstartdate.Value).Day
        dd = If(dd.ToString.Length = 1, "0" & dd, dd)
        Dim yyyy = CDate(dtpallowstartdate.Value).Year

        If e_asc = 74 Or e_asc = 106 Then 'j

            Select Case j
                Case 0
                    dtpallowstartdate.Value = CDate("Jan-" & dd & "-" & yyyy)
                Case 1
                    dtpallowstartdate.Value = CDate("Jun-" & dd & "-" & yyyy)
                Case 2
                    dtpallowstartdate.Value = CDate("Jul-" & dd & "-" & yyyy)
            End Select

            j += 1

            If j >= 3 Then
                j = 0
            End If

        ElseIf e_asc = 65 Or e_asc = 97 Then 'a

            Select Case a
                Case 0
                    dtpallowstartdate.Value = CDate("Apr-" & dd & "-" & yyyy)
                Case 1
                    dtpallowstartdate.Value = CDate("Aug-" & dd & "-" & yyyy)
            End Select

            a += 1

            If a >= 2 Then
                a = 0
            End If

        ElseIf e_asc = 77 Or e_asc = 109 Then 'm
            Select Case m
                Case 0
                    dtpallowstartdate.Value = CDate("Mar-" & dd & "-" & yyyy)
                Case 1
                    dtpallowstartdate.Value = CDate("May-" & dd & "-" & yyyy)
            End Select

            m += 1

            If m >= 2 Then
                m = 0
            End If

            'ElseIf e_asc = 70 Or e_asc = 102 Then 'f
            '    dtpallowstartdate.Value = CDate("Feb-" & dd & "-" & yyyy)
        ElseIf e_asc = 83 Or e_asc = 115 Then 's
            dtpallowstartdate.Value = CDate("Sep-" & dd & "-" & yyyy)
        ElseIf e_asc = 78 Or e_asc = 110 Then 'n
            dtpallowstartdate.Value = CDate("Nov-" & dd & "-" & yyyy)
        ElseIf e_asc = 68 Or e_asc = 100 Then 'd
            dtpallowstartdate.Value = CDate("Dec-" & dd & "-" & yyyy)
        ElseIf e_asc = 79 Or e_asc = 111 Then 'o
            dtpallowstartdate.Value = CDate("Oct-" & dd & "-" & yyyy)
        Else
            dtpallowstartdate.Value = CDate(MM & "-" & dd & "-" & yyyy)
        End If
    End Sub

    Private Sub dtpallowenddate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtpallowenddate.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        Static j As SByte = 0
        Static a As SByte = 0
        Static m As SByte = 0

        Dim MM = Format(CDate(dtpallowenddate.Value), "MMM")
        Dim dd = CDate(dtpallowenddate.Value).Day
        dd = If(dd.ToString.Length = 1, "0" & dd, dd)
        Dim yyyy = CDate(dtpallowenddate.Value).Year

        If e_asc = 74 Or e_asc = 106 Then 'j

            Select Case j
                Case 0
                    dtpallowenddate.Value = CDate("Jan-" & dd & "-" & yyyy)
                Case 1
                    dtpallowenddate.Value = CDate("Jun-" & dd & "-" & yyyy)
                Case 2
                    dtpallowenddate.Value = CDate("Jul-" & dd & "-" & yyyy)
            End Select

            j += 1

            If j >= 3 Then
                j = 0
            End If

        ElseIf e_asc = 65 Or e_asc = 97 Then 'a

            Select Case a
                Case 0
                    dtpallowenddate.Value = CDate("Apr-" & dd & "-" & yyyy)
                Case 1
                    dtpallowenddate.Value = CDate("Aug-" & dd & "-" & yyyy)
            End Select

            a += 1

            If a >= 2 Then
                a = 0
            End If

        ElseIf e_asc = 77 Or e_asc = 109 Then 'm
            Select Case m
                Case 0
                    dtpallowenddate.Value = CDate("Mar-" & dd & "-" & yyyy)
                Case 1
                    dtpallowenddate.Value = CDate("May-" & dd & "-" & yyyy)
            End Select

            m += 1

            If m >= 2 Then
                m = 0
            End If

            'ElseIf e_asc = 70 Or e_asc = 102 Then 'f
            '    dtpallowenddate.Value = CDate("Feb-" & dd & "-" & yyyy)
        ElseIf e_asc = 83 Or e_asc = 115 Then 's
            dtpallowenddate.Value = CDate("Sep-" & dd & "-" & yyyy)
        ElseIf e_asc = 78 Or e_asc = 110 Then 'n
            dtpallowenddate.Value = CDate("Nov-" & dd & "-" & yyyy)
        ElseIf e_asc = 68 Or e_asc = 100 Then 'd
            dtpallowenddate.Value = CDate("Dec-" & dd & "-" & yyyy)
        ElseIf e_asc = 79 Or e_asc = 111 Then 'o
            dtpallowenddate.Value = CDate("Oct-" & dd & "-" & yyyy)
        Else
            dtpallowenddate.Value = CDate(MM & "-" & dd & "-" & yyyy)
        End If
    End Sub

    Private Sub Label167_Click(sender As Object, e As EventArgs) Handles Label167.Click

    End Sub

    Dim dtpallowstartdateval As Object = Nothing

    Dim empallow_daterangehasrecord

    Private Sub dtpallowstartdate_ValueChanged(sender As Object, e As EventArgs) 'Handles dtpallowstartdate.ValueChanged
        'dtpallowstartdateval = dtpallowstartdate.Value
        empallow_daterangehasrecord = 0
        Dim date_range = DateDiff(DateInterval.Day, CDate(dtpallowstartdate.Value), CDate(dtpallowenddate.Value))

        If date_range < 0 And cboallowfreq.SelectedIndex <> 2 And cboallowfreq.SelectedIndex <> -1 Then
            empallow_daterangehasrecord = 1
            WarnBalloon("Please supply a valid date range.", "Invalid date range", dtpallowstartdate, dtpallowstartdate.Width - 16, -69)
        End If

    End Sub

    Private Sub dtpallowstartdate_TextChanged(sender As Object, e As EventArgs) Handles dtpallowstartdate.TextChanged
        dtpallowstartdateval = dtpallowstartdate.Value
    End Sub

    Private Sub dtpallowstartdate_GotFocus(sender As Object, e As EventArgs) Handles dtpallowstartdate.GotFocus

        If dgvempallowance.RowCount = 1 Then
        Else
            dgvempallowance.Item("eall_Start", dgvallowRowindx).Selected = 1
        End If

    End Sub

    Private Sub dtpallowstartdate_Leave(sender As Object, e As EventArgs) Handles dtpallowstartdate.Leave
        'dtpallowstartdateval.ToString
        Dim thegetval = If(dtpallowstartdateval = Nothing, Trim(dtpallowstartdate.Value), Trim(dtpallowstartdateval))

        If dgvempallowance.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempallowance.Rows.Add()
                dgvallowRowindx = dgvempallowance.RowCount - 2

            End If
        Else
            If dgvempallowance.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempallowance.Rows.Add()
                    dgvallowRowindx = dgvempallowance.RowCount - 2

                End If
            Else
                dgvallowRowindx = dgvempallowance.CurrentRow.Index

            End If
        End If

        If thegetval <> "" Then
            dgvempallowance.Item("eall_Start", dgvallowRowindx).Value = Format(CDate(thegetval), machineShortDateFormat)

            dtpallowstartdate.Value = Format(CDate(thegetval), machineShortDateFormat)

            If dtpallowenddate.Enabled = False Then
                dgvempallowance.Item("eall_End", dgvallowRowindx).Value = Nothing
            End If
        End If

        With dgvempallowance.Rows(dgvallowRowindx)
            If Val(.Cells("eall_RowID").Value) <> 0 Then
                If thegetval <> allowance_prevval(2) Then
                    listofEditEmpAllow.Add(.Cells("eall_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Private Sub Label168_Click(sender As Object, e As EventArgs) Handles Label168.Click

    End Sub

    Dim dtpallowenddateval As Object = Nothing

    Private Sub dtpallowenddate_ValueChanged(sender As Object, e As EventArgs) 'Handles dtpallowenddate.ValueChanged
        'dtpallowenddateval = dtpallowenddate.Value
        empallow_daterangehasrecord = 0
        Dim date_range = DateDiff(DateInterval.Day, CDate(dtpallowstartdate.Value), CDate(dtpallowenddate.Value))

        If date_range < 0 And cboallowfreq.SelectedIndex <> 2 And cboallowfreq.SelectedIndex <> -1 Then
            empallow_daterangehasrecord = 1
            WarnBalloon("Please supply a valid date range.", "Invalid date range", dtpallowenddate, dtpallowenddate.Width - 16, -69)
        End If

    End Sub

    Private Sub dtpallowenddate_TextChanged(sender As Object, e As EventArgs) Handles dtpallowenddate.TextChanged
        dtpallowenddateval = dtpallowenddate.Value
    End Sub

    Private Sub dtpallowenddate_GotFocus(sender As Object, e As EventArgs) Handles dtpallowenddate.GotFocus

        If dgvempallowance.RowCount = 1 Then
        Else
            dgvempallowance.Item("eall_End", dgvallowRowindx).Selected = 1
        End If

    End Sub

    Private Sub dtpallowenddate_Leave(sender As Object, e As EventArgs) Handles dtpallowenddate.Leave

        Dim thegetval = If(dtpallowenddateval = Nothing, Trim(dtpallowenddate.Value), Trim(dtpallowenddateval))

        If dgvempallowance.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempallowance.Rows.Add()
                dgvallowRowindx = dgvempallowance.RowCount - 2

            End If
        Else
            If dgvempallowance.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempallowance.Rows.Add()
                    dgvallowRowindx = dgvempallowance.RowCount - 2

                End If
            Else
                dgvallowRowindx = dgvempallowance.CurrentRow.Index

            End If
        End If

        If thegetval <> "" Then
            dgvempallowance.Item("eall_End", dgvallowRowindx).Value = Format(CDate(thegetval), machineShortDateFormat)

            dtpallowenddate.Value = Format(CDate(thegetval), machineShortDateFormat)

        End If

        With dgvempallowance.Rows(dgvallowRowindx)
            If Val(.Cells("eall_RowID").Value) <> 0 Then
                If thegetval <> allowance_prevval(3) Then
                    listofEditEmpAllow.Add(.Cells("eall_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Private Sub txtallowamt_TextChanged(sender As Object, e As EventArgs) Handles txtallowamt.TextChanged

    End Sub

    Private Sub txtallowamt_GotFocus(sender As Object, e As EventArgs) Handles txtallowamt.GotFocus

        If dgvempallowance.RowCount = 1 Then
        Else
            dgvempallowance.Item("eall_Amount", dgvallowRowindx).Selected = 1
        End If

    End Sub

    Private Sub txtallowamt_Leave(sender As Object, e As EventArgs) Handles txtallowamt.Leave

        Dim thegetval = Trim(txtallowamt.Text)

        If dgvempallowance.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempallowance.Rows.Add()
                dgvallowRowindx = dgvempallowance.RowCount - 2

            End If
        Else
            If dgvempallowance.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempallowance.Rows.Add()
                    dgvallowRowindx = dgvempallowance.RowCount - 2

                End If
            Else
                dgvallowRowindx = dgvempallowance.CurrentRow.Index

            End If
        End If

        If thegetval <> "" Then
            dgvempallowance.Item("eall_Amount", dgvallowRowindx).Value = thegetval

            txtallowamt.Text = thegetval

        End If

        With dgvempallowance.Rows(dgvallowRowindx)
            If Val(.Cells("eall_RowID").Value) <> 0 Then
                If thegetval <> allowance_prevval(4) Then
                    listofEditEmpAllow.Add(.Cells("eall_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Private Sub lnklbaddallowtype_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklbaddallowtype.LinkClicked

        Dim n_ProductControlForm As New ProductControlForm

        With n_ProductControlForm

            .Status.HeaderText = "Taxable Flag"

            .PartNo.HeaderText = "Allowance name"

            .NameOfCategory = "Allowance Type"

            If n_ProductControlForm.ShowDialog = Windows.Forms.DialogResult.OK Then

                enlistTheLists("SELECT CONCAT(COALESCE(p.PartNo,''),'@',p.RowID)" &
                               " FROM product p" &
                               " INNER JOIN category c ON c.RowID=p.CategoryID" &
                               " WHERE c.CategoryName='Allowance Type'" &
                               " AND p.OrganizationID='" & orgztnID & "' AND p.ActiveData='1';",
                               allowance_type) 'cboallowtype

                cboallowtype.Items.Clear()
                eall_Type.Items.Clear()

                For Each strval In allowance_type
                    cboallowtype.Items.Add(getStrBetween(strval, "", "@"))
                    eall_Type.Items.Add(getStrBetween(strval, "", "@"))
                Next

            End If

        End With

    End Sub

    Private Sub dgvempallowance_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvempallowance.DataError
        e.ThrowException = False

    End Sub

    Private Sub dgvempallowance_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempallowance.CellContentClick

    End Sub

    Public listofEditEmpAllow As New List(Of String)

    Private Sub dgvempallowance_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempallowance.CellEndEdit

        dgvempallowance.ShowCellErrors = 1

        Dim colname = dgvempallowance.Columns(e.ColumnIndex).Name

        With dgvempallowance.Rows(e.RowIndex)
            If Val(.Cells("eall_RowID").Value) <> 0 Then
                listofEditEmpAllow.Add(.Cells("eall_RowID").Value)
            Else
            End If

            If colname = "eall_Type" Then

                For Each strval In allowance_type
                    Dim strcompare = Trim(dgvempallowance.Item("eall_Type", e.RowIndex).Value)
                    If strcompare = getStrBetween(strval, "", "@") Then
                        dgvempallowance.Item("eall_ProdID", e.RowIndex).Value = StrReverse(getStrBetween(StrReverse(strval), "", "@"))
                        Exit For
                    End If
                Next

            End If

        End With

    End Sub

    Dim allowance_prevval(5) As Object

    Private Sub dgvempallowance_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvempallowance.SelectionChanged

        RemoveHandler dtpallowstartdate.ValueChanged, AddressOf dtpallowstartdate_ValueChanged
        RemoveHandler dtpallowenddate.ValueChanged, AddressOf dtpallowenddate_ValueChanged

        If dgvempallowance.RowCount <> 1 Then
            With dgvempallowance.CurrentRow
                dgvallowRowindx = .Index
                If .IsNewRow = False Then
                    cboallowtype.Text = .Cells("eall_Type").Value
                    cboallowfreq.Text = .Cells("eall_Frequency").Value

                    If .Cells("eall_Start").Value = Nothing Then
                        dtpallowstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
                    Else
                        dtpallowstartdate.Value = Format(CDate(.Cells("eall_Start").Value), machineShortDateFormat)
                    End If

                    If .Cells("eall_End").Value = Nothing Then
                        dtpallowenddate.Value = Format(CDate(dbnow), machineShortDateFormat)
                    Else
                        dtpallowenddate.Value = Format(CDate(.Cells("eall_End").Value), machineShortDateFormat)
                    End If

                    txtallowamt.Text = .Cells("eall_Amount").Value

                    allowance_prevval(0) = .Cells("eall_Type").Value
                    allowance_prevval(1) = .Cells("eall_Frequency").Value
                    allowance_prevval(2) = dtpallowstartdate.Value
                    allowance_prevval(3) = dtpallowenddate.Value
                    allowance_prevval(4) = .Cells("eall_Amount").Value

                    cboallowfreq_SelectedValueChanged(sender, e)
                Else
                    cboallowtype.Text = ""
                    cboallowfreq.Text = ""
                    dtpallowstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
                    dtpallowenddate.Value = Format(CDate(dbnow), machineShortDateFormat)
                    txtallowamt.Text = ""

                    allowance_prevval(0) = ""
                    allowance_prevval(1) = ""
                    allowance_prevval(2) = dtpallowstartdate.Value
                    allowance_prevval(3) = dtpallowenddate.Value
                    allowance_prevval(4) = ""

                End If
            End With
        Else
            dgvallowRowindx = 0

            cboallowtype.Text = ""
            cboallowfreq.Text = ""
            dtpallowstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
            dtpallowenddate.Value = Format(CDate(dbnow), machineShortDateFormat)
            txtallowamt.Text = ""

            allowance_prevval(0) = ""
            allowance_prevval(1) = ""
            allowance_prevval(2) = dtpallowstartdate.Value
            allowance_prevval(3) = dtpallowenddate.Value
            allowance_prevval(4) = ""

        End If

        AddHandler dtpallowstartdate.ValueChanged, AddressOf dtpallowstartdate_ValueChanged
        AddHandler dtpallowenddate.ValueChanged, AddressOf dtpallowenddate_ValueChanged

    End Sub

    Sub VIEW_employeeallowance(Optional eallow_EmployeeID As Object = Nothing)

        Dim param(1, 2) As Object

        param(0, 0) = "eallow_EmployeeID"
        param(1, 0) = "eallow_OrganizationID"

        param(0, 1) = eallow_EmployeeID
        param(1, 1) = orgztnID

        EXEC_VIEW_PROCEDURE(param,
                           "VIEW_employeeallowance",
                           dgvempallowance, , 1)

    End Sub

    Private Sub txtallowamt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtallowamt.KeyPress

        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedash As SByte = 0

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 45 Or e_KAsc = 46 Then

            If e_KAsc = 45 Then

                onedash += 1
                If onedash >= 2 Then
                    If txtallowamt.Text.Contains("-") Then
                        e.Handled = True
                        onedash = 2
                    Else
                        e.Handled = False
                        onedash = 0
                    End If
                Else
                    If txtallowamt.Text.Contains("-") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If

            ElseIf e_KAsc = 46 Then

                onedot += 1
                If onedot >= 2 Then
                    If txtallowamt.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtallowamt.Text.Contains(".") Then
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

    Private Sub tsbtnimportallowance_Click(sender As Object, e As EventArgs) Handles tsbtnimportallowance.Click
        Dim browsefile As New OpenFileDialog()

        browsefile.Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                                  "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"

        If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

            filepath = IO.Path.GetFullPath(browsefile.FileName)

            Dim catchDatSet =
                getWorkBookAsDataSet(filepath,
                                     Me.Name)

            If (catchDatSet Is Nothing) = False And Trim(filepath).Length > 0 Then

                Dim n_importallowance As New ImportAllowance(catchDatSet, Me)

                Dim objNewThread As New Thread(AddressOf n_importallowance.DoImport)

                objNewThread.IsBackground = True

                objNewThread.Start()

                threadArrayList.Add(objNewThread)

            End If

        End If

    End Sub

#End Region

#Region "Employee Overtime"

    Dim view_IDEmpOT As Integer

    Public EmpOTtype As New AutoCompleteStringCollection

    Private Sub tbpEmpOT_Click(sender As Object, e As EventArgs) Handles tbpEmpOT.Click

    End Sub

    Sub tbpEmpOT_Enter(sender As Object, e As EventArgs) Handles tbpEmpOT.Enter

        tabpageText(tabIndx)

        tbpEmpOT.Text = "EMPLOYEE OVERTIME               "

        Label25.Text = "EMPLOYEE OVERTIME"
        Static once As SByte = 0

        If once = 0 Then
            once = 1

            enlistTheLists("SELECT DisplayValue FROM listofval WHERE Type='Employee OT Type' AND Active='Yes';",
                           EmpOTtype)

            cboEmpOTtypes.Items.Clear()

            For Each strval In EmpOTtype
                cboEmpOTtypes.Items.Add(strval)
                eot_Type.Items.Add(strval)
            Next

            enlistToCboBox("SELECT DisplayValue FROM listofval WHERE Type='Employee Overtime Status' AND Active='Yes' ORDER BY OrderBy;",
                           cboStatusEmpOT)

            view_IDEmpOT = VIEW_privilege("Employee Overtime", orgztnID)

            AddHandler dgvempOT.SelectionChanged, AddressOf dgvEmpOT_SelectionChanged

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDEmpOT)

            If formuserprivilege.Count = 0 Then

                tsbtnNewEmpOT.Visible = 0
                tsbtnSaveEmpOT.Visible = 0

                dontUpdateEmpOT = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        tsbtnNewEmpOT.Visible = 0
                        tsbtnSaveEmpOT.Visible = 0

                        dontUpdateEmpOT = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            tsbtnNewEmpOT.Visible = 0
                        Else
                            tsbtnNewEmpOT.Visible = 1
                        End If

                        If drow("Deleting").ToString = "N" Then
                            tsbtnDeleteEmpOT.Visible = 0
                        Else
                            tsbtnDeleteEmpOT.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateEmpOT = 1
                        Else
                            dontUpdateEmpOT = 0
                        End If

                    End If

                Next

            End If

        End If

        tabIndx = 14 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub tbpEmpOT_Leave(sender As Object, e As EventArgs) 'Handles tbpEmpOT.Leave
        tbpEmpOT.Text = "Emp OT"
    End Sub

    Dim pagenumberOT As Integer = 0

    Sub VIEW_employeeOT(ByVal EmployeeID As Object) '3722

        RemoveHandler dgvempOT.SelectionChanged, AddressOf dgvEmpOT_SelectionChanged

        Dim param(2, 2) As Object

        param(0, 0) = "eot_EmployeeID"
        param(1, 0) = "eot_OrganizationID"
        param(2, 0) = "pagenumber"

        param(0, 1) = EmployeeID
        param(1, 1) = orgztnID
        param(2, 1) = pagenumberOT

        EXEC_VIEW_PROCEDURE(param,
                           "VIEW_employeeOT",
                           dgvempOT, 1, 1)

        AddHandler dgvempOT.SelectionChanged, AddressOf dgvEmpOT_SelectionChanged

    End Sub

    Sub tsbtnNewEmpOT_Click(sender As Object, e As EventArgs) Handles tsbtnNewEmpOT.Click

        dgvempOT.Focus()

        RemoveHandler dgvempOT.SelectionChanged, AddressOf dgvEmpOT_SelectionChanged

        For Each r As DataGridViewRow In dgvempOT.Rows
            If r.IsNewRow Then
                dgvEmpOTRowindx = r.Index
                r.Cells("eot_Type").Selected = True
                Exit For
            End If
        Next

        dgvEmpOT_SelectionChanged(sender, e)

        cboEmpOTtypes.Focus()

        AddHandler dgvempOT.SelectionChanged, AddressOf dgvEmpOT_SelectionChanged

    End Sub

    Dim dontUpdateEmpOT As SByte = 0

    Sub SaveEmpOT_Click(sender As Object, e As EventArgs) Handles tsbtnSaveEmpOT.Click

        pbEmpPicEmpOT.Focus()

        cboEmpOTtypes.Focus()

        pbEmpPicEmpOT.Focus()

        dtpstartdateEmpOT.Focus()

        pbEmpPicEmpOT.Focus()

        dtpendateEmpOT.Focus()

        pbEmpPicEmpOT.Focus()

        txtstarttimeEmpOT.Focus()

        pbEmpPicEmpOT.Focus()

        txtreasonEmpOT.Focus()

        pbEmpPicEmpOT.Focus()

        txtcommentsEmpOT.Focus()

        pbEmpPicEmpOT.Focus()

        cboStatusEmpOT.Focus()

        pbEmpPicEmpOT.Focus()

        If dontUpdateEmpOT = 1 Then
            listofEditRowEmpOT.Clear()
        End If

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        dgvempOT.EndEdit(True)

        If haserrinputEmpOT = 1 Then
            '"Invalid Date issued or Date of expiration"

            'MsgBox(colName & vbNewLine & rowIndxEmpOT)

            If dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText <> Nothing Then
                Dim invalids As String

                invalids = StrReverse(getStrBetween(StrReverse(dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText), "", " "))

                WarnBalloon("Please input a valid " & invalids & ".",
                              StrConv(dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText, VbStrConv.ProperCase),
                              lblforballoon, 0, -69)
            Else
                WarnBalloon("Please input a valid and complete Employee Overtime.",
                            "Invalid employee Overtime",
                            lblforballoon, 0, -69)
            End If

            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            Exit Sub
        End If

        If dgvEmp.RowCount = 0 Then
            RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            Exit Sub
        End If

        Dim param(13, 2) As Object

        param(0, 0) = "eot_RowID"
        param(1, 0) = "eot_OrganizationID"
        param(2, 0) = "eot_CreatedBy"
        param(3, 0) = "eot_LastUpdBy"
        param(4, 0) = "eot_EmployeeID"
        param(5, 0) = "eot_OTType"
        param(6, 0) = "eot_OTStartTime"
        param(7, 0) = "eot_OTEndTime"
        param(8, 0) = "eot_OTStartDate"
        param(9, 0) = "eot_OTEndDate"
        param(10, 0) = "eot_Reason"
        param(11, 0) = "eot_Comments"
        param(12, 0) = "eot_Image"
        param(13, 0) = "eot_OTStatus"

        'cboStatusEmpOT

        For Each r As DataGridViewRow In dgvempOT.Rows
            If Val(r.Cells("eot_RowID").Value) = 0 And
                tsbtnNewEmpOT.Visible = True Then

                If r.IsNewRow = False Then

                    If r.Cells("eot_StartDate").Value <> Nothing Then

                        param(0, 1) = DBNull.Value
                        param(1, 1) = orgztnID
                        param(2, 1) = z_User 'CreatedBy
                        param(3, 1) = z_User 'LastUpdBy
                        param(4, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                        param(5, 1) = If(r.Cells("eot_Type").Value = Nothing, DBNull.Value, r.Cells("eot_Type").Value) 'EmpOT type
                        param(6, 1) = MilitTime(r.Cells("eot_StartTime").Value) 'If(r.Cells("Column3").Value = Nothing, DBNull.Value, r.Cells("Column3").Value) 'Start time
                        param(7, 1) = MilitTime(r.Cells("eot_EndTime").Value) 'If(r.Cells("Column4").Value = Nothing, DBNull.Value, r.Cells("Column4").Value) 'End time
                        param(8, 1) = If(r.Cells("eot_StartDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("eot_StartDate").Value), "yyyy-MM-dd")) 'Start date
                        param(9, 1) = If(r.Cells("eot_EndDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("eot_EndDate").Value), "yyyy-MM-dd")) 'End date
                        param(10, 1) = If(r.Cells("eot_Reason").Value = Nothing, DBNull.Value, r.Cells("eot_Reason").Value) 'Reason
                        param(11, 1) = If(r.Cells("eot_Comment").Value = Nothing, DBNull.Value, r.Cells("eot_Comment").Value) 'Comments

                        Dim imageobj As Object = If(r.Cells("eot_Image").Value Is Nothing,
                                                    DBNull.Value,
                                                    r.Cells("eot_Image").Value) 'Image

                        param(12, 1) = imageobj
                        param(13, 1) = r.Cells("eot_Status").Value

                        r.Cells("eot_RowID").Value = EXEC_INSUPD_PROCEDURE(param, "INSUPD_employeeOT", "eot_ID")

                        INSUPD_employeeattachments(, dgvEmp.CurrentRow.Cells("RowID").Value,
                                                    "Employee Overtime@" & r.Cells("eot_RowID").Value,
                                                    r.Cells("eot_attafileextensn").Value,
                                                    r.Cells("eot_attafilename").Value)

                    End If

                End If
            Else

                If listofEditRowEmpOT.Contains(r.Cells("eot_RowID").Value) Then
                    If r.Cells("eot_StartTime").Value <> Nothing And r.Cells("eot_EndTime").Value <> Nothing _
                        And r.Cells("eot_StartDate").Value <> Nothing And r.Cells("eot_EndDate").Value <> Nothing Then

                        param(0, 1) = r.Cells("eot_RowID").Value
                        param(1, 1) = orgztnID
                        param(2, 1) = z_User 'CreatedBy
                        param(3, 1) = z_User 'LastUpdBy
                        param(4, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                        param(5, 1) = If(r.Cells("eot_Type").Value = Nothing, DBNull.Value, r.Cells("eot_Type").Value) 'EmpOT type
                        param(6, 1) = MilitTime(r.Cells("eot_StartTime").Value) 'If(r.Cells("Column3").Value = Nothing, DBNull.Value, r.Cells("Column3").Value) 'Start time
                        param(7, 1) = MilitTime(r.Cells("eot_EndTime").Value) 'If(r.Cells("Column4").Value = Nothing, DBNull.Value, r.Cells("Column4").Value) 'End time
                        param(8, 1) = If(r.Cells("eot_StartDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("eot_StartDate").Value), "yyyy-MM-dd")) 'Start date
                        param(9, 1) = If(r.Cells("eot_EndDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("eot_EndDate").Value), "yyyy-MM-dd")) 'End date
                        param(10, 1) = If(r.Cells("eot_Reason").Value = Nothing, DBNull.Value, r.Cells("eot_Reason").Value) 'Reason
                        param(11, 1) = If(r.Cells("eot_Comment").Value = Nothing, DBNull.Value, r.Cells("eot_Comment").Value) 'Comments
                        param(12, 1) = If(r.Cells("eot_Image").Value Is Nothing, DBNull.Value, r.Cells("eot_Image").Value)
                        param(13, 1) = r.Cells("eot_Status").Value

                        EXEC_INSUPD_PROCEDURE(param, "INSUPD_employeeOT", "eot_ID")

                        INSUPD_employeeattachments(, dgvEmp.CurrentRow.Cells("RowID").Value,
                                                    "Employee Overtime@" & r.Cells("eot_RowID").Value,
                                                    r.Cells("eot_attafileextensn").Value,
                                                    r.Cells("eot_attafilename").Value)

                    End If

                End If
            End If

        Next

        listofEditRowEmpOT.Clear()
        '                                           'dgvEmp
        InfoBalloon("Changes made in Employee ID '" & dgvEmp.CurrentRow.Cells("Column1").Value & "' has successfully saved.", "Changes successfully save", lblforballoon, 0, -69)

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Dim isdlEmpOT As SByte = 0

    Private Sub btndlEmpOTfile_Click(sender As Object, e As EventArgs) Handles btndlEmpOTfile.Click
        isdlEmpOT = 1

        If dgvempOT.RowCount <> 1 Then
            dgvempOT.Item("eot_viewimage",
                             dgvempOT.CurrentRow.Index).Selected = True

            Dim dgvceleventarg As New DataGridViewCellEventArgs(eot_viewimage.Index,
                                                                dgvempOT.CurrentRow.Index)

            dgvEmpOT_CellContentClick(sender, dgvceleventarg)

        End If

    End Sub

    Private Sub dgvempOT_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvempOT.DataError
    End Sub

    Dim promptresultEOT As Object

    Private Sub dgvEmpOT_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempOT.CellContentClick

        If dgvempOT.CurrentCell.ColumnIndex = dgvempOT.Columns("eot_viewimage").Index Then
            If dgvempOT.CurrentRow.Cells("eot_Image").Value IsNot Nothing Then

                If isdlEmpOT = 1 Then
                    promptresultEOT = Windows.Forms.DialogResult.Yes
                    isdlEmpOT = 0
                Else
                    promptresultEOT = Windows.Forms.DialogResult.No
                    'promptresultEOT = MessageBox.Show("Do you want to download and open this file ?", "View file", MessageBoxButtons.YesNo)
                End If

                If promptresultEOT = Windows.Forms.DialogResult.No Then

                    Dim _attafileextensn,
                        _attafilename As String

                    _attafilename = dgvempOT.CurrentRow.Cells("eot_attafilename").Value
                    _attafileextensn = dgvempOT.CurrentRow.Cells("eot_attafileextensn").Value

                    If _attafileextensn <> "" Then
                        If Trim(_attafilename) = Nothing Then
                            dgvempOT.CurrentRow.Cells("eot_attafilename").Selected = 1
                            dgvempOT.BeginEdit(1)
                            InfoBalloon("Please input a file name.", "Attachment has no file name", Label234, 0, -69)
                        Else
                            Dim tmp_path = Path.GetTempPath &
                                                _attafilename & _attafileextensn

                            Dim file_stream As New FileStream(tmp_path, FileMode.Create)
                            Dim blob As Byte() = DirectCast(dgvempOT.CurrentRow.Cells("eot_Image").Value, Byte())
                            file_stream.Write(blob, 0, blob.Length)
                            file_stream.Close()
                            file_stream = Nothing

                            Process.Start(tmp_path)
                        End If
                    Else
                        MsgBox("Nothing to view", MsgBoxStyle.Information)
                    End If
                Else 'If promptresultEOT = Windows.Forms.DialogResult.Yes Then

                    Dim dlImage As SaveFileDialog = New SaveFileDialog
                    dlImage.RestoreDirectory = True

                    'dlImage.Filter = "JPEG(*.jpg)|*.jpg"

                    'dlImage.Filter = "All files (*.*)|*.*" & _
                    '                 "|JPEG (*.jpg)|*.jpg" & _
                    '                 "|PNG (*.PNG)|*.png" & _
                    '                 "|MS Word 97-2003 Document (*.doc)|*.doc" & _
                    '                 "|MS Word Document (*.docx)|*.docx" & _
                    '                 "|MS Excel 97-2003 Workbook (*.xls)|*.xls" & _
                    '                 "|MS Excel Workbook (*.xlsx)|*.xlsx"

                    If dlImage.ShowDialog = Windows.Forms.DialogResult.OK Then

                        Dim savefilepath As String =
                            Path.GetFullPath(dlImage.FileName) &
                            dgvempOT.CurrentRow.Cells("eot_attafileextensn").Value

                        Dim fs As New FileStream(savefilepath, FileMode.Create)
                        Dim blob As Byte() = DirectCast(dgvempOT.CurrentRow.Cells("eot_Image").Value, Byte())
                        fs.Write(blob, 0, blob.Length)
                        fs.Close()
                        fs = Nothing

                        Process.Start(savefilepath)

                    End If

                End If
            Else
                MsgBox("Nothing to view", MsgBoxStyle.Information)

            End If

        End If

    End Sub

    Dim prev_eot_Type,
        prev_eot_StartTime,
        prev_eot_EndTime,
        prev_eot_StartDate,
        prev_eot_EndDate,
        prev_eot_Reason,
        prev_eot_Comment,
        prev_eot_Status As String

    Private Sub dgvEmpOT_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvEmpOT.SelectionChanged
        RemoveHandler cboStatusEmpOT.SelectedIndexChanged, AddressOf cboStatusEmpOT_SelectedIndexChanged
        cboStatusEmpOT.SelectedIndex = -1
        If dgvempOT.RowCount <> 1 Then

            With dgvempOT.CurrentRow
                If .IsNewRow = False Then

                    dgvEmpOTRowindx = .Index

                    prev_eot_Type = .Cells("eot_Type").Value

                    prev_eot_StartTime = If(CStr(.Cells("eot_StartTime").Value) = "", "", CDate(.Cells("eot_StartTime").Value).ToShortTimeString)

                    prev_eot_EndTime = If(CStr(.Cells("eot_EndTime").Value) = "", "", CDate(.Cells("eot_EndTime").Value).ToShortTimeString)

                    prev_eot_StartDate = Format(CDate(.Cells("eot_StartDate").Value), machineShortDateFormat)

                    prev_eot_EndDate = Format(CDate(.Cells("eot_EndDate").Value), machineShortDateFormat)

                    prev_eot_Reason = CStr(.Cells("eot_Reason").Value)

                    prev_eot_Comment = CStr(.Cells("eot_Comment").Value)

                    prev_eot_Status = CStr(.Cells("eot_Status").Value)

                    cboEmpOTtypes.Text = prev_eot_Type '.Cells("eot_Type").Value

                    txtstarttimeEmpOT.Text = prev_eot_StartTime '.Cells("eot_StartTime").Value
                    txtendtimeEmpOT.Text = prev_eot_EndTime '.Cells("eot_EndTime").Value

                    If prev_eot_StartDate = "1/1/0001" Then
                        dtpstartdateEmpOT.Value = Format(CDate(dbnow), machineShortDateFormat) '.Cells("eot_StartDate").Value
                    Else
                        dtpstartdateEmpOT.Value = Format(CDate(prev_eot_StartDate), machineShortDateFormat) '.Cells("eot_StartDate").Value
                    End If

                    If prev_eot_EndDate = "1/1/0001" Then
                        dtpendateEmpOT.Value = Format(CDate(dbnow), machineShortDateFormat) '.Cells("eot_StartDate").Value
                    Else
                        dtpendateEmpOT.Value = Format(CDate(prev_eot_EndDate), machineShortDateFormat) '.Cells("eot_EndDate").Value
                    End If

                    txtreasonEmpOT.Text = prev_eot_Reason '.Cells("eot_Reason").Value
                    txtcommentsEmpOT.Text = prev_eot_Comment '.Cells("eot_Comment").Value

                    cboStatusEmpOT.Text = prev_eot_Status
                    pbempEmpOT.Image = Nothing

                    pbempEmpOT.Image = ConvByteToImage(DirectCast(.Cells("eot_Image").Value, Byte()))

                    'makefileGetPath(DirectCast(.Cells("Column9").Value)
                Else
                    clearEOT()

                    dtpstartdateEmpOT.Value = Format(CDate(dbnow), machineShortDateFormat) '.Cells("eot_StartDate").Value
                    dtpendateEmpOT.Value = Format(CDate(dbnow), machineShortDateFormat) '.Cells("eot_EndDate").Value

                End If
            End With
        Else
            dgvEmpOTRowindx = 0
            'ObjectFields(dgvEmpOT, 1)
            clearEOT()
            'clearObjControl(TabPage1)

            dtpstartdateEmpOT.Value = Format(CDate(dbnow), machineShortDateFormat) '.Cells("eot_StartDate").Value
            dtpendateEmpOT.Value = Format(CDate(dbnow), machineShortDateFormat) '.Cells("eot_EndDate").Value

        End If

        AddHandler cboStatusEmpOT.SelectedIndexChanged, AddressOf cboStatusEmpOT_SelectedIndexChanged
    End Sub

    Private Sub dgvEmpOT_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvempOT.Scroll
    End Sub

    Sub clearEOT()

        cboEmpOTtypes.Text = ""
        cboEmpOTtypes.SelectedIndex = -1
        txtstarttimeEmpOT.Text = ""
        txtendateEmpOT.Text = ""
        txtendtimeEmpOT.Text = ""
        txtstartdateEmpOT.Text = ""
        txtcommentsEmpOT.Text = ""

        pbempEmpOT.Image = Nothing

    End Sub

    Public listofEditRowEmpOT As New AutoCompleteStringCollection

    Private Sub dgvEmpOT_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvempOT.CellBeginEdit
    End Sub

    Dim colNameEmpOT As String
    Dim rowIndxEmpOT As Integer

    Private Sub dgvEmpOT_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempOT.CellEndEdit

        dgvempOT.ShowCellErrors = True

        Static num As Integer = -1

        colNameEmpOT = dgvempOT.Columns(e.ColumnIndex).Name
        rowIndxEmpOT = e.RowIndex

        If Val(dgvempOT.Item("eot_RowID", e.RowIndex).Value) <> 0 Then
            listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", e.RowIndex).Value)
        Else

        End If

        If (colNameEmpOT = "eot_StartDate" Or colNameEmpOT = "eot_EndDate") Then
            Dim dateobj As Object = Trim(dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).Value)
            Try
                dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).Value = Format(CDate(dateobj), machineShortDateFormat)

                haserrinputEmpOT = 0

                dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText = Nothing

                If dgvempOT.Item("eot_StartDate", rowIndxEmpOT).Value <> Nothing _
                    And dgvempOT.Item("eot_EndDate", rowIndxEmpOT).Value <> Nothing Then

                    Dim date_differ = DateDiff(DateInterval.Day,
                                                CDate(dgvempOT.Item("eot_StartDate", rowIndxEmpOT).Value),
                                                CDate(dgvempOT.Item("eot_EndDate", rowIndxEmpOT).Value))

                    If date_differ < 0 Then
                        haserrinputEmpOT = 1
                        dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText = "     Invalid date value"
                    Else
                        dgvempOT.Item("eot_StartDate", rowIndxEmpOT).ErrorText = Nothing
                        dgvempOT.Item("eot_EndDate", rowIndxEmpOT).ErrorText = Nothing
                    End If

                    Dim _from = Format(CDate(dgvempOT.Item("eot_StartDate", rowIndxEmpOT).Value), "yyyy-MM-dd")
                    Dim _to = Format(CDate(dgvempOT.Item("eot_EndDate", rowIndxEmpOT).Value), "yyyy-MM-dd")

                    Dim invalidEmpOT = 0

                    If dgvEmp.RowCount <> 0 Then
                        invalidEmpOT = EXECQUER("SELECT EXISTS(SELECT RowID" &
                                                    " FROM employeetimeentry" &
                                                    " WHERE DATE BETWEEN '" & _from & "'" &
                                                    " AND '" & _to & "'" &
                                                    " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                                    " AND OrganizationID=" & orgztnID & ");")

                        If invalidEmpOT = 0 Then
                            invalidEmpOT = EXECQUER("SELECT EXISTS(SELECT RowID" &
                                                    " FROM employeeEmpOT" &
                                                    " WHERE " &
                                                    " ('" & _from & "' IN (EmpOTStartDate,EmpOTEndDate) OR '" & _to & "' IN (EmpOTStartDate,EmpOTEndDate))" &
                                                    " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value &
                                                    " AND OrganizationID=" & orgztnID & ");")
                        End If
                    End If

                    If listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", e.RowIndex).Value) Then
                        haserrinputEmpOT = 0
                        dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText = Nothing
                    Else

                        If invalidEmpOT = 1 Then
                            haserrinputEmpOT = 1
                            dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText = "     The employee has already a record on this date"
                        End If

                    End If

                End If
            Catch ex As Exception
                haserrinputEmpOT = 1
                dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText = "     Invalid date value"
            End Try

        ElseIf (colNameEmpOT = "eot_StartTime" Or colNameEmpOT = "eot_EndTime") Then

            Dim dateobj As Object = Trim(dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).Value).Replace(" ", ":")

            Dim ampm As String = Nothing

            Try

                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If

                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")
                If ampm = Nothing Then
                    dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).Value = valtime.ToShortTimeString
                Else
                    dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).Value = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If

                haserrinputEmpOT = 0

                dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")

                    dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).Value = valtime.ToShortTimeString

                    haserrinputEmpOT = 0

                    dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputEmpOT = 1
                    dgvempOT.Item(colNameEmpOT, rowIndxEmpOT).ErrorText = "     Invalid time value"
                End Try
            End Try

        ElseIf colNameEmpOT = "eot_Type" Then
            If dgvempOT.Item("eot_Type", rowIndxEmpOT).Value = Nothing Then
                haserrinputEmpOT = 1
                dgvempOT.Item("eot_Type", rowIndxEmpOT).ErrorText = "     Invalid EmpOT type"
            Else
                haserrinputEmpOT = 0
                dgvempOT.Item("eot_Type", rowIndxEmpOT).ErrorText = Nothing
            End If
        End If

        dgvempOT.Item("eot_viewimage", rowIndxEmpOT).Value = "view this"

        dgvempOT.AutoResizeRow(rowIndxEmpOT)
        dgvempOT.PerformLayout()
    End Sub

    Dim haserrinputEmpOT As SByte

    Private Sub ToolStripButton8_Click(sender As Object, e As EventArgs) Handles ToolStripButton8.Click
        listofEditRowEmpOT.Clear()

        Dim rowindx, colindx As Integer

        If dgvempOT.RowCount = 1 Then
            rowindx = -1
            colindx = -1
        Else
            If dgvempOT.CurrentRow.IsNewRow Then
                rowindx = -1
                colindx = -1
            Else
                rowindx = dgvempOT.CurrentRow.Index
                colindx = dgvempOT.CurrentCell.ColumnIndex
            End If

        End If

        dgvEmp_SelectionChanged(sender, e)

        If rowindx = -1 Then
        Else
            dgvempOT.Item(colindx, rowindx).Selected = True

        End If

    End Sub

    Private Sub tsbtnDeleteEmpOT_Click(sender As Object, e As EventArgs) Handles tsbtnDeleteEmpOT.Click

        dgvempOT.Focus()

        If Not dgvempOT.CurrentRow.IsNewRow Then

            Dim result = MessageBox.Show("Are you sure you want to delete this Overtime ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

            If result = DialogResult.Yes Then

                dgvempOT.EndEdit(True)

                EXECQUER("DELETE FROM employeeovertime WHERE RowID = '" & dgvempOT.CurrentRow.Cells("eot_RowID").Value & "';")

                dgvempOT.Rows.Remove(dgvempOT.CurrentRow)

            End If

        End If

    End Sub

    'eot_RowID@RowID&False
    'eot_Type@EmpOT type&True
    'eot_StartTime@Start time&True
    'eot_EndTime@End time&True
    'eot_StartDate@Start date&True
    'eot_EndDate@End date&True
    'eot_Reason@Reason&True
    'eot_Comment@Comments&True
    'eot_Image@Image&False

    Dim currleavcolmnEOT As String

    Private Sub dgvEmpOT_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvempOT.EditingControlShowing
        currleavcolmnEOT = dgvempOT.Columns(dgvempOT.CurrentCell.ColumnIndex).Name

        If currleavcolmnEOT = "eot_Type" Then
            With DirectCast(e.Control, ComboBox)
                .AutoCompleteCustomSource = EmpOTtype
                .AutoCompleteMode = AutoCompleteMode.Suggest
                .AutoCompleteSource = AutoCompleteSource.CustomSource

            End With
        Else
        End If

    End Sub

    Dim thefilepathEOT As String

    Dim atta_nameEOT,
        atta_extensnEOT As String

    Private Sub btnbrowseimageEmpOT_Click(sender As Object, e As EventArgs) Handles btnBrowseEmpOT.Click
        'RemoveHandler dgvEmpOT.SelectionChanged, AddressOf dgvEmpOT_SelectionChanged
        atta_nameEOT = Nothing
        atta_extensnEOT = Nothing

        Static employeeEmpOTRowID As Integer = -1
        Try
            Dim browsefile As OpenFileDialog = New OpenFileDialog()

            'browsefile.Filter = "JPEG(*.jpg)|*.jpg"

            browsefile.Filter = "All files (*.*)|*.*" &
                                "|JPEG (*.jpg)|*.jpg" &
                                "|PNG (*.PNG)|*.png" &
                                "|MS Word 97-2003 Document (*.doc)|*.doc" &
                                "|MS Word Document (*.docx)|*.docx" &
                                "|MS Excel 97-2003 Workbook (*.xls)|*.xls" &
                                "|MS Excel Workbook (*.xlsx)|*.xlsx"

            '|" & _
            '"PNG(*.PNG)|*.png|" & _
            '"Bitmap(*.BMP)|*.bmp"
            If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then
                With dgvempOT
                    '.ClearSelection()
                    .Focus()

                    thefilepathEOT = browsefile.FileName
                    atta_nameEOT = Path.GetFileNameWithoutExtension(thefilepathEOT)
                    atta_extensnEOT = Path.GetExtension(thefilepathEOT)

                    If .CurrentRow.IsNewRow Then
                        Dim e_rowindx As Integer = .CurrentRow.Index
                        Dim currcol As String = .Columns(.CurrentCell.ColumnIndex).Name
                        .Rows.Add()
                        .Item("eot_Image", e_rowindx).Value = Nothing
                        .Item("eot_Image", e_rowindx).Value = convertFileToByte(thefilepathEOT)

                        .Item("eot_attafilename", e_rowindx).Value = atta_nameEOT
                        .Item("eot_attafileextensn", e_rowindx).Value = atta_extensnEOT

                        .Item(currcol, e_rowindx).Selected = True
                    Else
                        .CurrentRow.Cells("eot_Image").Value = Nothing
                        .CurrentRow.Cells("eot_Image").Value = convertFileToByte(thefilepathEOT)

                        .CurrentRow.Cells("eot_attafilename").Value = atta_nameEOT
                        .CurrentRow.Cells("eot_attafileextensn").Value = atta_extensnEOT

                        If employeeEmpOTRowID <> Val(dgvempOT.Item("eot_RowID", .CurrentRow.Index).Value) Then
                            listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", .CurrentRow.Index).Value)
                        End If
                    End If

                    .Focus()
                End With
            Else

            End If
        Catch ex As Exception
            MsgBox(ex.Message & " Error on Image")
        Finally
            dgvEmpOT_SelectionChanged(sender, e)
            'AddHandler dgvEmpOT.SelectionChanged, AddressOf dgvEmpOT_SelectionChanged
        End Try
    End Sub

    Private Sub btnClearEmpOT_Click(sender As Object, e As EventArgs) Handles btnClearEmpOT.Click
        Static employeeEmpOTRowID As Integer = -1

        pbempEmpOT.Image = Nothing

        If dgvempOT.RowCount = 1 Then
            dgvempOT.Item("eot_Image", 0).Value = Nothing
        Else
            If dgvempOT.CurrentRow.IsNewRow = False Then
                dgvempOT.CurrentRow.Cells("eot_Image").Value = Nothing
                If employeeEmpOTRowID <> Val(dgvempOT.CurrentRow.Cells("eot_RowID").Value) Then
                    listofEditRowEmpOT.Add(dgvempOT.CurrentRow.Cells("eot_RowID").Value)
                End If
            End If
        End If

    End Sub

    Private Sub cboEmpOTtypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEmpOTtypes.SelectedIndexChanged

    End Sub

    Private Sub cboEmpOTtypes_GotFocus(sender As Object, e As EventArgs) Handles cboEmpOTtypes.GotFocus

        If dgvempOT.RowCount = 1 Then
        Else
            dgvempOT.Item("eot_Type", dgvEmpOTRowindx).Selected = True
        End If
    End Sub

    Dim dgvEmpOTRowindx As Integer

    Private Sub cboEmpOTtypes_Leave(sender As Object, e As EventArgs) Handles cboEmpOTtypes.Leave

        colNameEmpOT = "eot_Type"

        Dim thegetval = Trim(cboEmpOTtypes.Text) 'If(sendr_name = "txtEmpOTtype", Trim(txtEmpOTtype.Text), Trim(cboEmpOTtypes.Text))

        If dgvempOT.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempOT.Rows.Add()
                dgvEmpOTRowindx = dgvempOT.RowCount - 2
            End If
        Else
            If dgvempOT.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempOT.Rows.Add()
                    dgvEmpOTRowindx = dgvempOT.RowCount - 2
                End If
            Else
                dgvEmpOTRowindx = dgvempOT.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempOT.Item("eot_Type", dgvEmpOTRowindx).Value = thegetval

            cboEmpOTtypes.Text = thegetval

            dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

            If thegetval <> prev_eot_Type _
                    And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
            End If

        End If

    End Sub

    Private Sub txtstarttimeEmpOT_TextChanged(sender As Object, e As EventArgs) Handles txtstarttimeEmpOT.TextChanged
    End Sub

    Private Sub txtstarttimeEmpOT_GotFocus(sender As Object, e As EventArgs) Handles txtstarttimeEmpOT.GotFocus

        If dgvempOT.RowCount = 1 Then
        Else
            dgvempOT.Item("eot_StartTime", dgvEmpOTRowindx).Selected = True
        End If

    End Sub

    Private Sub txtstarttimeEmpOT_Leave(sender As Object, e As EventArgs) Handles txtstarttimeEmpOT.Leave

        colNameEmpOT = "eot_StartTime"

        Dim thegetval = Trim(txtstarttimeEmpOT.Text)

        Dim theretval = ""

        If dgvempOT.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempOT.Rows.Add()
                dgvEmpOTRowindx = dgvempOT.RowCount - 2
            End If
        Else

            If dgvempOT.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempOT.Rows.Add()
                    dgvEmpOTRowindx = dgvempOT.RowCount - 2
                End If
            Else
                dgvEmpOTRowindx = dgvempOT.CurrentRow.Index
            End If

        End If

        Dim dateobj As Object = thegetval.Replace(" ", ":")
        Dim ampm As String = Nothing

        If thegetval <> "" Then
            Try

                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If

                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")

                If ampm = Nothing Then
                    theretval = valtime.ToShortTimeString
                Else
                    theretval = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If

                haserrinputEmpOT = 0

                dgvempOT.Item("eot_StartTime", dgvEmpOTRowindx).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")

                    theretval = valtime.ToShortTimeString
                    'Format(valtime, "hh:mm tt")

                    haserrinputEmpOT = 0

                    dgvempOT.Item("eot_StartTime", dgvEmpOTRowindx).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputEmpOT = 1
                    dgvempOT.Item("eot_StartTime", dgvEmpOTRowindx).ErrorText = "     Invalid time value"
                Finally
                    If thegetval <> "" Then
                        dgvempOT.Item("eot_StartTime", dgvEmpOTRowindx).Value = theretval
                        txtstarttimeEmpOT.Text = theretval
                        dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

                        If theretval <> prev_eot_StartTime _
                        And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                            listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
                        End If

                    End If
                End Try
            Finally
                If thegetval <> "" Then
                    dgvempOT.Item("eot_StartTime", dgvEmpOTRowindx).Value = theretval
                    txtstarttimeEmpOT.Text = theretval
                    dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

                    If theretval <> prev_eot_StartTime _
                        And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                        listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
                    End If

                End If
            End Try
        End If

    End Sub

    Private Sub txtendtimeEmpOT_TextChanged(sender As Object, e As EventArgs) Handles txtendtimeEmpOT.TextChanged
    End Sub

    Private Sub txtendtimeEmpOT_GotFocus(sender As Object, e As EventArgs) Handles txtendtimeEmpOT.GotFocus

        If dgvempOT.RowCount = 1 Then
        Else
            dgvempOT.Item("eot_EndTime", dgvEmpOTRowindx).Selected = True
        End If

    End Sub

    Private Sub txtendtimeEmpOT1_Leave(sender As Object, e As EventArgs) Handles txtendtimeEmpOT.Leave

    End Sub

    Private Sub txtendtimeEmpOT_Leave(sender As Object, e As EventArgs) Handles txtendtimeEmpOT.Leave

        Static custom_date = Nothing

        Static Dim onceonly As SByte = 0

        If onceonly = 0 Then

            onceonly = 1

            custom_date = EXECQUER("SELECT CURDATE();")

        End If

        If custom_date = Nothing Then
            Exit Sub
        End If

        colNameEmpOT = "eot_EndTime"

        Dim thegetval = Trim(txtendtimeEmpOT.Text)

        Dim theretval = ""

        If dgvempOT.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempOT.Rows.Add()
                dgvEmpOTRowindx = dgvempOT.RowCount - 2
                'dgvEmpOT.Item("eot_EndTime", dgvEmpOTRowindx).Selected = True
            End If
        Else
            If dgvempOT.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempOT.Rows.Add()
                    dgvEmpOTRowindx = dgvempOT.RowCount - 2
                    'dgvEmpOT.Item("eot_EndTime", dgvEmpOTRowindx).Selected = True
                End If
            Else
                dgvEmpOTRowindx = dgvempOT.CurrentRow.Index
            End If
        End If

        Dim dateobj As Object = thegetval.Replace(" ", ":")
        Dim ampm As String = Nothing

        If thegetval <> "" Then
            Try

                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If

                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")

                If ampm = Nothing Then
                    theretval = valtime.ToShortTimeString
                Else
                    theretval = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If
                'Format(valtime, "hh:mm tt")

                '**************************************************************

                Static previous_starttime As String = String.Empty

                Static previous_endtime As String = String.Empty

                If previous_starttime <> txtstarttimeEmpOT.Text _
                    Or previous_endtime <> txtendtimeEmpOT.Text Then

                    previous_starttime = theretval

                    Dim starttimevalue = Format(CDate("2015-01-01 " & txtstarttimeEmpOT.Text), "hh:mm tt")

                    Dim endtimevalue = Format(CDate("2015-01-01 " & theretval), "hh:mm tt")

                    Dim starttimeMilit = MilitTime(starttimevalue)

                    Dim endtimeMilit = MilitTime(endtimevalue)

                    txtendtimeEmpOT.Text = txtendtimeEmpOT.Text

                    previous_endtime = txtendtimeEmpOT.Text

                End If

                '**************************************************************

                haserrinputEmpOT = 0

                dgvempOT.Item("eot_EndTime", dgvEmpOTRowindx).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")

                    theretval = valtime.ToShortTimeString
                    'Format(valtime, "hh:mm tt")

                    haserrinputEmpOT = 0

                    dgvempOT.Item("eot_EndTime", dgvEmpOTRowindx).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputEmpOT = 1
                    dgvempOT.Item("eot_EndTime", dgvEmpOTRowindx).ErrorText = "     Invalid time value"
                Finally
                    If thegetval <> "" Then
                        dgvempOT.Item("eot_EndTime", dgvEmpOTRowindx).Value = theretval
                        txtendtimeEmpOT.Text = theretval
                        dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

                        If theretval <> prev_eot_EndTime _
                        And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                            listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
                        End If

                    End If
                End Try
            Finally
                If thegetval <> "" Then
                    dgvempOT.Item("eot_EndTime", dgvEmpOTRowindx).Value = theretval
                    txtendtimeEmpOT.Text = theretval
                    dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

                    If theretval <> prev_eot_EndTime _
                        And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                        listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
                    End If

                End If
            End Try
        End If

    End Sub

    Private Sub dtpstartdateEmpOT_ValueChanged(sender As Object, e As EventArgs) Handles dtpstartdateEmpOT.ValueChanged

        Dim has_valid_dgvrow As Boolean = False

        Try
            curr_ot_dgvrow = dgvempOT.CurrentRow
            has_valid_dgvrow = (curr_ot_dgvrow IsNot Nothing _
                                And curr_ot_dgvrow.Cells("eot_RowID").Value = Nothing)
        Catch ex As Exception
            has_valid_dgvrow = False
        Finally
            If has_valid_dgvrow Then
                Dim date_value = dtpstartdateEmpOT.Value
                dtpendateEmpOT.Value = date_value
                curr_ot_dgvrow.Cells("eot_EndDate").Value = date_value

            End If

        End Try

    End Sub

    Private Sub txtstartdateEmpOT_TextChanged(sender As Object, e As EventArgs) Handles txtstartdateEmpOT.TextChanged

    End Sub

    Private Sub dtpstartdateEmpOT_GotFocus(sender As Object, e As EventArgs) Handles dtpstartdateEmpOT.GotFocus

        If dgvempOT.RowCount = 1 Then
        Else
            dgvempOT.Item("eot_StartDate", dgvEmpOTRowindx).Selected = True
        End If

    End Sub

    Private Sub txtstartdateEmpOT_GotFocus(sender As Object, e As EventArgs) Handles txtstartdateEmpOT.GotFocus

    End Sub

    Private Sub dtpstartdateEmpOT_Leave(sender As Object, e As EventArgs) Handles dtpstartdateEmpOT.Leave

        colNameEmpOT = "eot_StartDate"

        Dim thegetval = Trim(dtpstartdateEmpOT.Value)

        Dim theretval = ""

        If dgvempOT.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempOT.Rows.Add()
                dgvEmpOTRowindx = dgvempOT.RowCount - 2
                'dgvEmpOT.Item("eot_StartDate", dgvEmpOTRowindx).Selected = True
            End If
        Else
            If dgvempOT.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempOT.Rows.Add()
                    dgvEmpOTRowindx = dgvempOT.RowCount - 2
                    'dgvEmpOT.Item("eot_StartDate", dgvEmpOTRowindx).Selected = True
                End If
            Else
                dgvEmpOTRowindx = dgvempOT.CurrentRow.Index
            End If
        End If

        Dim dateobj As Object = Trim(thegetval)

        If thegetval <> "" Then
            Try
                theretval = Format(CDate(dateobj), machineShortDateFormat)

                haserrinputEmpOT = 0

                dgvempOT.Item("eot_StartDate", dgvEmpOTRowindx).ErrorText = Nothing

                If thegetval <> Nothing _
                    And Trim(txtendateEmpOT.Text) <> Nothing Then

                    Dim date_differ = DateDiff(DateInterval.Day,
                                                CDate(thegetval),
                                                CDate(Trim(txtendateEmpOT.Text)))

                    If date_differ < 0 Then
                        haserrinputEmpOT = 1
                        dgvempOT.Item("eot_StartDate", dgvEmpOTRowindx).ErrorText = "     Invalid date value"
                    Else
                        dgvempOT.Item("eot_StartDate", dgvEmpOTRowindx).ErrorText = Nothing
                        dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).ErrorText = Nothing
                    End If

                    Dim _from = Format(CDate(theretval), "yyyy-MM-dd")
                    Dim _to = Format(CDate(dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).Value), "yyyy-MM-dd")

                    Dim invalidEmpOT = 0

                End If
            Catch ex As Exception
                haserrinputEmpOT = 1
                dgvempOT.Item("eot_StartDate", dgvEmpOTRowindx).ErrorText = "     Invalid date value"
            Finally
                If thegetval <> "" Then
                    dgvempOT.Item("eot_StartDate", dgvEmpOTRowindx).Value = theretval
                    dtpstartdateEmpOT.Value = Format(CDate(theretval), machineShortDateFormat)
                    dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

                    If theretval <> prev_eot_StartDate _
                        And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                        listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
                    End If

                End If
            End Try

        End If

    End Sub

    Private Sub txtstartdateEmpOT_Leave(sender As Object, e As EventArgs) Handles txtstartdateEmpOT.Leave
    End Sub

    Private Sub dtpendateEmpOT_ValueChanged(sender As Object, e As EventArgs) Handles dtpendateEmpOT.ValueChanged
    End Sub

    Private Sub txtendateEmpOT_TextChanged(sender As Object, e As EventArgs) Handles txtendateEmpOT.TextChanged
    End Sub

    Private Sub dtpendateEmpOT_GotFocus(sender As Object, e As EventArgs) Handles dtpendateEmpOT.GotFocus

        If dgvempOT.RowCount = 1 Then
        Else
            dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).Selected = True
        End If

    End Sub

    Private Sub txtendateEmpOT_GotFocus(sender As Object, e As EventArgs) Handles txtendateEmpOT.GotFocus
    End Sub

    Private Sub dtpendateEmpOT_Leave(sender As Object, e As EventArgs) Handles dtpendateEmpOT.Leave

        colNameEmpOT = "eot_EndDate"

        Dim thegetval = Trim(dtpendateEmpOT.Value)

        Dim theretval = ""

        If dgvempOT.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempOT.Rows.Add()
                dgvEmpOTRowindx = dgvempOT.RowCount - 2
                'dgvEmpOT.Item("eot_EndDate", dgvEmpOTRowindx).Selected = True
            End If
        Else
            If dgvempOT.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempOT.Rows.Add()
                    dgvEmpOTRowindx = dgvempOT.RowCount - 2
                    'dgvEmpOT.Item("eot_EndDate", dgvEmpOTRowindx).Selected = True
                End If
            Else
                dgvEmpOTRowindx = dgvempOT.CurrentRow.Index
            End If
        End If

        Dim dateobj As Object = Trim(thegetval)

        If thegetval <> "" Then
            Try
                theretval = Format(CDate(dateobj), machineShortDateFormat)

                haserrinputEmpOT = 0

                dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).ErrorText = Nothing

                If thegetval <> Nothing _
                    And Trim(txtstartdateEmpOT.Text) <> Nothing Then

                    Dim date_differ = DateDiff(DateInterval.Day,
                                                CDate(Trim(txtstartdateEmpOT.Text)),
                                                CDate(thegetval))

                    If date_differ < 0 Then
                        haserrinputEmpOT = 1
                        dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).ErrorText = "     Invalid date value"
                    Else
                        dgvempOT.Item("eot_StartDate", dgvEmpOTRowindx).ErrorText = Nothing
                        dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).ErrorText = Nothing
                    End If

                    Dim _from = Format(CDate(Trim(txtstartdateEmpOT.Text)), "yyyy-MM-dd")
                    Dim _to = Format(CDate(theretval), "yyyy-MM-dd")

                    Dim invalidEmpOT = 0

                    'If dgvEmp.RowCount <> 0 Then
                    '    invalidEmpOT = EXECQUER("SELECT EXISTS(SELECT RowID" & _
                    '                                " FROM employeetimeentry" & _
                    '                                " WHERE DATE BETWEEN '" & _from & "'" & _
                    '                                " AND '" & _to & "'" & _
                    '                                " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value & _
                    '                                " AND OrganizationID=" & orgztnID & ");")

                    '    If invalidEmpOT = 0 Then
                    '        invalidEmpOT = EXECQUER("SELECT EXISTS(SELECT RowID" & _
                    '                                " FROM employeeovertime" & _
                    '                                " WHERE " & _
                    '                                " ('" & _from & "' IN (OTStartDate,OTEndDate) OR '" & _to & "' IN (OTStartDate,OTEndDate))" & _
                    '                                " AND EmployeeID=" & dgvEmp.CurrentRow.Cells("RowID").Value & _
                    '                                " AND OrganizationID=" & orgztnID & ");")
                    '    End If
                    'End If

                    'If invalidEmpOT = 1 Then
                    '    haserrinputEmpOT = 1
                    '    dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).ErrorText = "     The employee has already a record on this date"
                    'End If

                End If
            Catch ex As Exception
                haserrinputEmpOT = 1
                dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).ErrorText = "     Invalid date value"
            Finally
                If thegetval <> "" Then
                    dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).Value = theretval
                    dtpendateEmpOT.Value = Format(CDate(theretval), machineShortDateFormat)
                    dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

                    If theretval <> prev_eot_EndDate _
                        And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                        listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
                    End If

                End If
            End Try
        Else
            dgvempOT.Item("eot_EndDate", dgvEmpOTRowindx).Value = Nothing
        End If

    End Sub

    Private Sub txtendateEmpOT_Leave(sender As Object, e As EventArgs) Handles txtendateEmpOT.Leave
    End Sub

    Private Sub txtreasonEmpOT_TextChanged(sender As Object, e As EventArgs) Handles txtreasonEmpOT.TextChanged
    End Sub

    Private Sub txtreasonEmpOT_GotFocus(sender As Object, e As EventArgs) Handles txtreasonEmpOT.GotFocus

        If dgvempOT.RowCount = 1 Then
        Else
            dgvempOT.Item("eot_Reason", dgvEmpOTRowindx).Selected = True
        End If

    End Sub

    Private Sub txtreasonEmpOT_Leave(sender As Object, e As EventArgs) Handles txtreasonEmpOT.Leave

        colNameEmpOT = "eot_Reason"

        Dim thegetval = Trim(txtreasonEmpOT.Text)

        If dgvempOT.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempOT.Rows.Add()
                dgvEmpOTRowindx = dgvempOT.RowCount - 2
                'dgvEmpOT.Item("eot_EndDate", dgvEmpOTRowindx).Selected = True
            End If
        Else
            If dgvempOT.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempOT.Rows.Add()
                    dgvEmpOTRowindx = dgvempOT.RowCount - 2
                    'dgvEmpOT.Item("eot_EndDate", dgvEmpOTRowindx).Selected = True
                End If
            Else
                dgvEmpOTRowindx = dgvempOT.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempOT.Item("eot_Reason", dgvEmpOTRowindx).Value = thegetval
            txtreasonEmpOT.Text = thegetval
            dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

            If thegetval <> prev_eot_Reason _
                    And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
            End If

        End If

    End Sub

    Private Sub txtcommentsEmpOT_TextChanged(sender As Object, e As EventArgs) Handles txtcommentsEmpOT.TextChanged
    End Sub

    Private Sub txtcommentsEmpOT_GotFocus(sender As Object, e As EventArgs) Handles txtcommentsEmpOT.GotFocus

        If dgvempOT.RowCount = 1 Then
        Else
            dgvempOT.Item("eot_Comment", dgvEmpOTRowindx).Selected = True
        End If

    End Sub

    Private Sub txtcommentsEmpOT_Leave(sender As Object, e As EventArgs) Handles txtcommentsEmpOT.Leave

        colNameEmpOT = "eot_Comment"

        Dim thegetval = Trim(txtcommentsEmpOT.Text)

        If dgvempOT.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempOT.Rows.Add()
                dgvEmpOTRowindx = dgvempOT.RowCount - 2
                'dgvEmpOT.Item("eot_EndDate", dgvEmpOTRowindx).Selected = True
            End If
        Else
            If dgvempOT.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempOT.Rows.Add()
                    dgvEmpOTRowindx = dgvempOT.RowCount - 2
                    'dgvEmpOT.Item("eot_EndDate", dgvEmpOTRowindx).Selected = True
                End If
            Else
                dgvEmpOTRowindx = dgvempOT.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempOT.Item("eot_Comment", dgvEmpOTRowindx).Value = thegetval
            txtcommentsEmpOT.Text = thegetval
            dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

            If thegetval <> prev_eot_Comment _
                    And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
            End If

        End If

    End Sub

    Private Sub Label186_Click(sender As Object, e As EventArgs) Handles Label186.Click
    End Sub

    Private Sub cboStatusEmpOT_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cboStatusEmpOT.SelectedIndexChanged
        cboStatusEmpOT_Leave(sender, e)
    End Sub

    Private Sub cboStatusEmpOT_GotFocus(sender As Object, e As EventArgs) Handles cboStatusEmpOT.GotFocus

        If dgvempOT.RowCount = 1 Then
        Else
            dgvempOT.Item("eot_Status", dgvEmpOTRowindx).Selected = True
        End If

    End Sub

    Private Sub cboStatusEmpOT_Leave(sender As Object, e As EventArgs) Handles cboStatusEmpOT.Leave

        colNameEmpOT = "eot_Status"

        Dim thegetval = Trim(cboStatusEmpOT.Text)

        If dgvempOT.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempOT.Rows.Add()
                dgvEmpOTRowindx = dgvempOT.RowCount - 2
                'dgvEmpOT.Item("eot_EndDate", dgvEmpOTRowindx).Selected = True
            End If
        Else
            If dgvempOT.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempOT.Rows.Add()
                    dgvEmpOTRowindx = dgvempOT.RowCount - 2
                    'dgvEmpOT.Item("eot_EndDate", dgvEmpOTRowindx).Selected = True
                End If
            Else
                dgvEmpOTRowindx = dgvempOT.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempOT.Item("eot_Status", dgvEmpOTRowindx).Value = thegetval
            cboStatusEmpOT.Text = thegetval
            dgvempOT.Item("eot_viewimage", dgvEmpOTRowindx).Value = "view this"

            If thegetval <> prev_eot_Status _
                    And dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value <> Nothing Then
                listofEditRowEmpOT.Add(dgvempOT.Item("eot_RowID", dgvEmpOTRowindx).Value)
            End If

        End If

    End Sub

    Private Sub btnEmpOTtyp_Click(sender As Object, e As EventArgs) Handles btnEmpOTtyp.Click
    End Sub

    Private Sub OTImport_Click(sender As Object, e As EventArgs) Handles OTImport.Click
        Dim browsefile As New OpenFileDialog()

        browsefile.Filter = str_ms_excel_file_extensn

        Try

            If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

                filepath = IO.Path.GetFullPath(browsefile.FileName)

                Dim catchDatSet =
                    getWorkBookAsDataSet(filepath,
                                         Me.Name)

                If (catchDatSet Is Nothing) = False And Trim(filepath).Length > 0 Then

                    Dim n_overtime As New ImportOvertime(catchDatSet, Me)

                    Dim objNewThread As New Thread(AddressOf n_overtime.DoImport)

                    Static indx As Integer = 0

                    indx += 1

                    objNewThread.Name = String.Concat("ImportOvertime", indx)

                    objNewThread.IsBackground = True

                    objNewThread.Start()

                    threadArrayList.Add(objNewThread)

                End If

                'MsgBox("Overtime Successfully Imported !")

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Overtime Import Failed")
        End Try

    End Sub

#End Region

#Region "Official Business Filing"

    Dim view_IDOBF As Integer

    Public OBFtype As New AutoCompleteStringCollection

    Private Sub tbpOBF_Click(sender As Object, e As EventArgs) Handles tbpOBF.Click

    End Sub

    Sub tbpOBF_Enter(sender As Object, e As EventArgs) Handles tbpOBF.Enter

        tabpageText(tabIndx)

        tbpOBF.Text = "OFFICIAL BUSINESS FILING               "

        Label25.Text = "OFFICIAL BUSINESS FILING"
        Static once As SByte = 0

        If once = 0 Then
            once = 1

            enlistTheLists("SELECT DisplayValue FROM listofval WHERE Type='Official Business Type' AND Active='Yes';",
                           OBFtype)

            cboOBFtypes.Items.Clear()

            For Each strval In OBFtype
                cboOBFtypes.Items.Add(strval)
                obf_Type.Items.Add(strval)
            Next

            enlistToCboBox("SELECT DisplayValue FROM listofval WHERE Type='Employee Overtime Status' AND Active='Yes' ORDER BY OrderBy;",
                           cboOBFstatus)

            view_IDOBF = VIEW_privilege("Official Business filing", orgztnID)

            AddHandler dgvOBF.SelectionChanged, AddressOf dgvOBF_SelectionChanged

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDOBF)

            If formuserprivilege.Count = 0 Then

                tsbtnNewOBF.Visible = 0
                tsbtnSaveOBF.Visible = 0

                dontUpdateOBF = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        tsbtnNewOBF.Visible = 0
                        tsbtnSaveOBF.Visible = 0
                        tsbtnDelOffBusi.Visible = 0

                        dontUpdateOBF = 1
                        Exit For
                    Else
                        tsbtnNewOBF.Visible = (drow("Creates").ToString = "Y")

                        tsbtnDelOffBusi.Visible = (drow("Deleting").ToString = "Y")

                        dontUpdateOBF = (drow("Updates").ToString = "N")
                    End If
                Next
            End If
        End If

        tabIndx = 15 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Sub VIEW_employeeoffbusi(ByVal Employee_ID As String)
        Dim params(2, 2) As Object

        params(0, 0) = "obf_EmployeeID"
        params(1, 0) = "obf_OrganizationID"

        params(0, 1) = Employee_ID
        params(1, 1) = orgztnID

        EXEC_VIEW_PROCEDURE(params,
                            "VIEW_employeeoffbusi",
                            dgvOBF, 1, 1)

    End Sub

    Sub tsbtnNewOBF_Click(sender As Object, e As EventArgs) Handles tsbtnNewOBF.Click

        dgvempOT.Focus()

        RemoveHandler dgvOBF.SelectionChanged, AddressOf dgvOBF_SelectionChanged

        For Each r As DataGridViewRow In dgvOBF.Rows
            If r.IsNewRow Then
                dgvOBFRowindx = r.Index
                r.Cells("obf_Type").Selected = True
                Exit For
            End If
        Next

        dgvOBF_SelectionChanged(sender, e)

        AddHandler dgvOBF.SelectionChanged, AddressOf dgvOBF_SelectionChanged

        'txtEmpOTtype.Focus()
        cboOBFtypes.Focus()
    End Sub

    Dim dontUpdateOBF As SByte = 0

    Private Sub tsbtnSaveOBF_Click(sender As Object, e As EventArgs) Handles tsbtnSaveOBF.Click

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        pbEmpPicOBF.Focus()

        cboOBFtypes.Focus()

        pbEmpPicOBF.Focus()

        dtpstartdateOBF.Focus()

        pbEmpPicOBF.Focus()

        dtpendateOBF.Focus()

        pbEmpPicOBF.Focus()

        txtstarttimeOBF.Focus()

        pbEmpPicOBF.Focus()

        txtendtimeOBF.Focus()

        pbEmpPicOBF.Focus()

        txtreasonOBF.Focus()

        pbEmpPicOBF.Focus()

        txtcommentsOBF.Focus()

        pbEmpPicOBF.Focus()

        cboOBFstatus.Focus()

        pbEmpPicOBF.Focus()

        dgvOBF.EndEdit(True)

        If dontUpdateOBF = 1 Then
            listofEditRowOBF.Clear()
        End If

        If haserrinputEmpOT = 1 Then
            '"Invalid Date issued or Date of expiration"

            'MsgBox(colName & vbNewLine & rowIndxEmpOT)

            If dgvOBF.Item(colNameEmpOT, rowIndxEmpOT).ErrorText <> Nothing Then
                Dim invalids As String

                invalids = StrReverse(getStrBetween(StrReverse(dgvOBF.Item(colNameEmpOT, rowIndxEmpOT).ErrorText), "", " "))

                WarnBalloon("Please input a valid " & invalids & ".",
                              StrConv(dgvOBF.Item(colNameEmpOT, rowIndxEmpOT).ErrorText, VbStrConv.ProperCase),
                              lblforballoon, 0, -69)
            Else
                WarnBalloon("Please input a valid and complete Employee Overtime.",
                            "Invalid employee Employee Overtime",
                            lblforballoon, 0, -69)
            End If

            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

            Exit Sub
        End If

        If dgvEmp.RowCount = 0 Then
            AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged
            Exit Sub
        End If

        Dim param(13, 2) As Object

        param(0, 0) = "obf_RowID"
        param(1, 0) = "obf_OrganizationID"
        param(2, 0) = "obf_CreatedBy"
        param(3, 0) = "obf_LastUpdBy"
        param(4, 0) = "obf_EmployeeID"
        param(5, 0) = "obf_Type"
        param(6, 0) = "obf_StartTime"
        param(7, 0) = "obf_EndTime"
        param(8, 0) = "obf_StartDate"
        param(9, 0) = "obf_EndDate"
        param(10, 0) = "obf_Reason"
        param(11, 0) = "obf_Comments"
        param(12, 0) = "obf_Image"
        param(13, 0) = "obf_OffBusStatus"

        For Each r As DataGridViewRow In dgvOBF.Rows
            If Val(r.Cells("obf_RowID").Value) = 0 And
                tsbtnNewOBF.Visible = True Then

                If r.IsNewRow = False Then

                    param(0, 1) = DBNull.Value
                    param(1, 1) = orgztnID
                    param(2, 1) = z_User 'CreatedBy
                    param(3, 1) = z_User 'LastUpdBy
                    param(4, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                    param(5, 1) = If(r.Cells("obf_Type").Value = Nothing, DBNull.Value, r.Cells("obf_Type").Value) 'EmpOT type
                    param(6, 1) = MilitTime(r.Cells("obf_StartTime").Value) 'If(r.Cells("Column3").Value = Nothing, DBNull.Value, r.Cells("Column3").Value) 'Start time
                    param(7, 1) = MilitTime(r.Cells("obf_EndTime").Value) 'If(r.Cells("Column4").Value = Nothing, DBNull.Value, r.Cells("Column4").Value) 'End time
                    param(8, 1) = If(r.Cells("obf_StartDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("obf_StartDate").Value), "yyyy-MM-dd")) 'Start date
                    param(9, 1) = If(r.Cells("obf_EndDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("obf_EndDate").Value), "yyyy-MM-dd")) 'End date
                    param(10, 1) = If(r.Cells("obf_Reason").Value = Nothing, DBNull.Value, r.Cells("obf_Reason").Value) 'Reason
                    param(11, 1) = If(r.Cells("obf_Comment").Value = Nothing, DBNull.Value, r.Cells("obf_Comment").Value) 'Comments

                    Dim imageobj As Object = If(r.Cells("obf_Image").Value Is Nothing,
                                                DBNull.Value,
                                                r.Cells("obf_Image").Value) 'Image

                    param(12, 1) = imageobj

                    param(13, 1) = r.Cells("obf_Status").Value

                    r.Cells("obf_RowID").Value = EXEC_INSUPD_PROCEDURE(param, "INSUPD_employeeoffbusi", "obf_ID")

                    INSUPD_employeeattachments(, dgvEmp.CurrentRow.Cells("RowID").Value,
                                                "Official Business@" & r.Cells("obf_RowID").Value,
                                                r.Cells("obf_attafileextensn").Value,
                                                r.Cells("obf_attafilename").Value)

                End If
            Else

                If listofEditRowOBF.Contains(r.Cells("obf_RowID").Value) Then

                    param(0, 1) = r.Cells("obf_RowID").Value
                    param(1, 1) = orgztnID
                    param(2, 1) = z_User 'CreatedBy
                    param(3, 1) = z_User 'LastUpdBy
                    param(4, 1) = dgvEmp.CurrentRow.Cells("RowID").Value
                    param(5, 1) = If(r.Cells("obf_Type").Value = Nothing, DBNull.Value, r.Cells("obf_Type").Value) 'EmpOT type
                    param(6, 1) = MilitTime(r.Cells("obf_StartTime").Value) 'If(r.Cells("Column3").Value = Nothing, DBNull.Value, r.Cells("Column3").Value) 'Start time
                    param(7, 1) = MilitTime(r.Cells("obf_EndTime").Value) 'If(r.Cells("Column4").Value = Nothing, DBNull.Value, r.Cells("Column4").Value) 'End time
                    param(8, 1) = If(r.Cells("obf_StartDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("obf_StartDate").Value), "yyyy-MM-dd")) 'Start date
                    param(9, 1) = If(r.Cells("obf_EndDate").Value = Nothing, DBNull.Value, Format(CDate(r.Cells("obf_EndDate").Value), "yyyy-MM-dd")) 'End date
                    param(10, 1) = If(r.Cells("obf_Reason").Value = Nothing, DBNull.Value, r.Cells("obf_Reason").Value) 'Reason
                    param(11, 1) = If(r.Cells("obf_Comment").Value = Nothing, DBNull.Value, r.Cells("obf_Comment").Value) 'Comments
                    param(12, 1) = If(r.Cells("obf_Image").Value Is Nothing, DBNull.Value, r.Cells("obf_Image").Value)
                    param(13, 1) = If(r.Cells("obf_Status").Value = Nothing, DBNull.Value, r.Cells("obf_Status").Value)

                    EXEC_INSUPD_PROCEDURE(param, "INSUPD_employeeoffbusi", "obf_ID")

                    INSUPD_employeeattachments(, dgvEmp.CurrentRow.Cells("RowID").Value,
                                                "Official Business@" & r.Cells("obf_RowID").Value,
                                                r.Cells("obf_attafileextensn").Value,
                                                r.Cells("obf_attafilename").Value)

                End If

            End If

        Next

        listofEditRowOBF.Clear()
        '                                           'dgvEmp
        InfoBalloon("Changes made in Employee ID '" & dgvEmp.CurrentRow.Cells("Column1").Value & "' has successfully saved.", "Changes successfully save", lblforballoon, 0, -69)

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

    End Sub

    Dim haserrinputOBF As SByte

    Private Sub tsbtnCancelOBF_Click(sender As Object, e As EventArgs) Handles tsbtnCancelOBF.Click
        listofEditRowOBF.Clear()
        dgvEmp_SelectionChanged(sender, e)
    End Sub

    Private Sub dgvOBF_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvOBF.DataError

    End Sub

    Dim promptresultobf As Object

    Private Sub dgvOBF_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOBF.CellContentClick
        RemoveHandler cboOBFstatus.SelectedIndexChanged, AddressOf cboOBFstatus_SelectedIndexChanged
        If dgvOBF.CurrentCell.ColumnIndex = dgvOBF.Columns("obf_view").Index Then
            If dgvOBF.CurrentRow.Cells("obf_Image").Value IsNot Nothing Then

                If isdlEmpOT = 1 Then
                    promptresultobf = Windows.Forms.DialogResult.Yes
                    isdlEmpOT = 0
                Else
                    promptresultobf = Windows.Forms.DialogResult.No
                    'promptresultobf = MessageBox.Show("Do you want to download and open this file ?", "View file", MessageBoxButtons.YesNo)
                End If

                If promptresultobf = Windows.Forms.DialogResult.No Then

                    Dim _attafileextensn,
                        _attafilename As String

                    _attafilename = dgvOBF.CurrentRow.Cells("obf_attafilename").Value
                    _attafileextensn = dgvOBF.CurrentRow.Cells("obf_attafileextensn").Value

                    Dim tmp_path = Path.GetTempPath &
                                                _attafilename & _attafileextensn

                    If _attafileextensn <> "" Then
                        If Trim(_attafilename) = "" Then
                            dgvOBF.CurrentRow.Cells("obf_attafilename").Selected = 1
                            dgvOBF.BeginEdit(1)
                            InfoBalloon("Please input a file name.", "Attachment has no file name", Label232, 0, -69)
                        Else
                            Dim file_stream As New FileStream(tmp_path, FileMode.Create)
                            Dim blob As Byte() = DirectCast(dgvOBF.CurrentRow.Cells("obf_Image").Value, Byte())
                            file_stream.Write(blob, 0, blob.Length)
                            file_stream.Close()
                            file_stream = Nothing

                            Process.Start(tmp_path)

                        End If
                    Else
                        MsgBox("Nothing to view", MsgBoxStyle.Information)
                    End If
                Else

                    Dim dlImage As SaveFileDialog = New SaveFileDialog
                    dlImage.RestoreDirectory = True

                    'dlImage.Filter = "JPEG(*.jpg)|*.jpg"

                    'dlImage.Filter = "All files (*.*)|*.*" & _
                    '                 "|JPEG (*.jpg)|*.jpg" & _
                    '                 "|PNG (*.PNG)|*.png" & _
                    '                 "|MS Word 97-2003 Document (*.doc)|*.doc" & _
                    '                 "|MS Word Document (*.docx)|*.docx" & _
                    '                 "|MS Excel 97-2003 Workbook (*.xls)|*.xls" & _
                    '                 "|MS Excel Workbook (*.xlsx)|*.xlsx"

                    If dlImage.ShowDialog = Windows.Forms.DialogResult.OK Then

                        'dlImage.FileName = dgvOBF.CurrentRow.Cells("obf_attafilename").Value & _
                        '                   dgvOBF.CurrentRow.Cells("obf_attafileextensn").Value

                        Dim savefilepath As String =
                            Path.GetFullPath(dlImage.FileName) &
                            dgvOBF.CurrentRow.Cells("obf_attafileextensn").Value

                        Dim fs As New FileStream(savefilepath, FileMode.Create)
                        Dim blob As Byte() = DirectCast(dgvOBF.CurrentRow.Cells("obf_Image").Value, Byte())
                        fs.Write(blob, 0, blob.Length)
                        fs.Close()
                        fs = Nothing

                        Process.Start(savefilepath)

                    End If

                End If
            Else
                MsgBox("Nothing to view", MsgBoxStyle.Information)

            End If
        End If

        AddHandler cboOBFstatus.SelectedIndexChanged, AddressOf cboOBFstatus_SelectedIndexChanged
    End Sub

    Dim prev_obf_Type,
        prev_obf_StartTime,
        prev_obf_EndTime,
        prev_obf_StartDate,
        prev_obf_EndDate,
        prev_obf_Reason,
        prev_obf_Comment _
        As String

    Private Sub dgvOBF_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvOBF.SelectionChanged

        If dgvOBF.RowCount <> 1 Then
            With dgvOBF.CurrentRow

                pbempOBF.Image = Nothing

                If .IsNewRow = 0 Then

                    dgvOBFRowindx = .Index

                    prev_obf_Type = .Cells("obf_Type").Value
                    prev_obf_StartTime = .Cells("obf_StartTime").Value
                    prev_obf_EndTime = .Cells("obf_EndTime").Value
                    prev_obf_StartDate = .Cells("obf_StartDate").Value
                    prev_obf_EndDate = .Cells("obf_EndDate").Value
                    prev_obf_Reason = .Cells("obf_Reason").Value
                    prev_obf_Comment = .Cells("obf_Comment").Value

                    cboOBFtypes.Text = .Cells("obf_Type").Value
                    txtstarttimeOBF.Text = .Cells("obf_StartTime").Value
                    txtendtimeOBF.Text = .Cells("obf_EndTime").Value

                    If Trim(.Cells("obf_StartDate").Value) = Nothing Then
                        dtpstartdateOBF.Value = Format(CDate(dbnow), machineShortDateFormat)
                    Else
                        dtpstartdateOBF.Value = Format(CDate(.Cells("obf_StartDate").Value), machineShortDateFormat)
                    End If

                    If Trim(.Cells("obf_EndDate").Value) = Nothing Then
                        dtpendateOBF.Value = Format(CDate(dbnow), machineShortDateFormat)
                    Else
                        dtpendateOBF.Value = Format(CDate(.Cells("obf_EndDate").Value), machineShortDateFormat)
                    End If

                    txtreasonOBF.Text = .Cells("obf_Reason").Value
                    txtcommentsOBF.Text = .Cells("obf_Comment").Value

                    cboOBFstatus.Text = .Cells("obf_Status").Value

                    pbempOBF.Image = ConvByteToImage(.Cells("obf_Image").Value)
                Else
                    prev_obf_Type = ""
                    prev_obf_StartTime = ""
                    prev_obf_EndTime = ""
                    prev_obf_StartDate = ""
                    prev_obf_EndDate = ""
                    prev_obf_Reason = ""
                    prev_obf_Comment = ""

                    cboOBFtypes.SelectedIndex = -1
                    txtstarttimeOBF.Text = ""
                    txtendtimeOBF.Text = ""
                    txtstartdateOBF.Text = ""
                    txtendateOBF.Text = ""
                    txtreasonOBF.Text = ""
                    txtcommentsOBF.Text = ""

                    cboOBFstatus.SelectedIndex = -1
                    dtpstartdateOBF.Value = Format(CDate(dbnow), machineShortDateFormat)
                    dtpendateOBF.Value = Format(CDate(dbnow), machineShortDateFormat)
                End If
            End With
        Else
            dgvOBFRowindx = 0

            prev_obf_Type = ""
            prev_obf_StartTime = ""
            prev_obf_EndTime = ""
            prev_obf_StartDate = ""
            prev_obf_EndDate = ""
            prev_obf_Reason = ""
            prev_obf_Comment = ""

            cboOBFtypes.SelectedIndex = -1
            txtstarttimeOBF.Text = ""
            txtendtimeOBF.Text = ""
            txtstartdateOBF.Text = ""
            txtendateOBF.Text = ""
            txtreasonOBF.Text = ""
            txtcommentsOBF.Text = ""

            pbempOBF.Image = Nothing

            dtpstartdateOBF.Value = Format(CDate(dbnow), machineShortDateFormat)
            dtpendateOBF.Value = Format(CDate(dbnow), machineShortDateFormat)
        End If
    End Sub

    Public listofEditRowOBF As New AutoCompleteStringCollection

    Private Sub dgvOBF_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOBF.CellEndEdit
    End Sub

    Dim dgvOBFRowindx As Integer
    Dim colNameOBF As String

    Private Sub cboOBFtypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOBFtypes.SelectedIndexChanged
    End Sub

    Private Sub cboOBFtypes_GotFocus(sender As Object, e As EventArgs) Handles cboOBFtypes.GotFocus

        If dgvOBF.RowCount = 1 Then
        Else
            dgvOBF.Item("obf_Type", dgvOBFRowindx).Selected = True
        End If
    End Sub

    Private Sub cboOBFtypes_Leave(sender As Object, e As EventArgs) Handles cboOBFtypes.Leave

        colNameOBF = "obf_Type"

        Dim thegetval = Trim(cboOBFtypes.Text) 'If(sendr_name = "txtOBFtype", Trim(txtOBFtype.Text), Trim(cboOBFtypes.Text))

        If dgvOBF.RowCount = 1 Then
            If thegetval <> "" Then
                dgvOBF.Rows.Add()
                dgvOBFRowindx = dgvOBF.RowCount - 2
            End If
        Else
            If dgvOBF.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvOBF.Rows.Add()
                    dgvOBFRowindx = dgvOBF.RowCount - 2
                End If
            Else
                dgvOBFRowindx = dgvOBF.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvOBF.Item("obf_Type", dgvOBFRowindx).Value = thegetval

            cboOBFtypes.Text = thegetval

            dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

            If thegetval <> prev_obf_Type _
                    And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
            End If

        End If

    End Sub

    Private Sub txtstarttimeOBF_TextChanged(sender As Object, e As EventArgs) Handles txtstarttimeOBF.TextChanged

    End Sub

    Private Sub Label227_Click(sender As Object, e As EventArgs) Handles Label227.Click

        InfoBalloon(, , txtstarttimeOBF, , , 1)

        txtstarttimeOBF.Focus()

        InfoBalloon("Ex. The time is 08:30:15 am" & vbNewLine &
                    "     just type '8 30'(don't include the apostrophe(')) and press Tab key or Enter key" & vbNewLine &
                    "Ex. The time is 06:15:15 pm" & vbNewLine &
                    "     if it is 'pm', get the hour and then plus 12(twelve)" & vbNewLine &
                    "     the hour is 6 so, 6 + 12 = 18" & vbNewLine &
                    "     just type '18 15'" & vbNewLine &
                    "     or '18:15 pm'(don't include the apostrophe(')) and press Tab key or Enter key" & vbNewLine &
                    "Ex. The time is 12:12:12 pm" & vbNewLine &
                    "     if it is 'pm', and the hour is equal to twelve(12)" & vbNewLine &
                    "     no need to add 12" & vbNewLine &
                    "     just type '12 12'" & vbNewLine &
                    "     or '12:12 pm'(don't include the apostrophe(')) and press Tab key or Enter key" & vbNewLine,
                    "How to input Time ?",
                    txtstarttimeOBF, 82, -240, , 3600000)

    End Sub

    Private Sub txtstarttimeOBF_GotFocus(sender As Object, e As EventArgs) Handles txtstarttimeOBF.GotFocus

        If dgvOBF.RowCount = 1 Then
        Else
            dgvOBF.Item("obf_StartTime", dgvOBFRowindx).Selected = True
        End If

    End Sub

    Private Sub txtstarttimeOBF_Leave(sender As Object, e As EventArgs) Handles txtstarttimeOBF.Leave

        InfoBalloon(, , txtstarttimeOBF, , , 1)

        colNameOBF = "obf_StartTime"

        Dim thegetval = Trim(txtstarttimeOBF.Text)

        Dim theretval = ""

        If dgvOBF.RowCount = 1 Then
            If thegetval <> "" Then
                dgvOBF.Rows.Add()
                dgvOBFRowindx = dgvOBF.RowCount - 2
            End If
        Else

            If dgvOBF.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvOBF.Rows.Add()
                    dgvOBFRowindx = dgvOBF.RowCount - 2
                End If
            Else
                dgvOBFRowindx = dgvOBF.CurrentRow.Index
            End If

        End If

        Dim dateobj As Object = thegetval.Replace(" ", ":")
        Dim ampm As String = Nothing

        If thegetval <> "" Then
            Try

                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If

                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")

                If ampm = Nothing Then
                    theretval = valtime.ToShortTimeString
                Else
                    theretval = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If

                'Format(valtime, "hh:mm tt")
                haserrinputOBF = 0

                dgvOBF.Item("obf_StartTime", dgvOBFRowindx).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")

                    theretval = valtime.ToShortTimeString
                    'Format(valtime, "hh:mm tt")

                    haserrinputOBF = 0

                    dgvOBF.Item("obf_StartTime", dgvOBFRowindx).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputOBF = 1
                    dgvOBF.Item("obf_StartTime", dgvOBFRowindx).ErrorText = "     Invalid time value"
                Finally
                    If thegetval <> "" Then
                        dgvOBF.Item("obf_StartTime", dgvOBFRowindx).Value = theretval
                        txtstarttimeOBF.Text = theretval
                        dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

                        If theretval <> prev_obf_StartTime _
                        And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                            listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
                        End If

                    End If
                End Try
            Finally
                If thegetval <> "" Then
                    dgvOBF.Item("obf_StartTime", dgvOBFRowindx).Value = theretval
                    txtstarttimeOBF.Text = theretval
                    dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

                    If theretval <> prev_obf_StartTime _
                        And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                        listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
                    End If

                End If
            End Try
        End If

    End Sub

    Private Sub txtendtimeOBF_TextChanged(sender As Object, e As EventArgs) Handles txtendtimeOBF.TextChanged
    End Sub

    Private Sub txtendtimeOBF_GotFocus(sender As Object, e As EventArgs) Handles txtendtimeOBF.GotFocus

        If dgvOBF.RowCount = 1 Then
        Else
            dgvOBF.Item("obf_EndTime", dgvOBFRowindx).Selected = True
        End If

    End Sub

    Private Sub txtendtimeOBF_Leave(sender As Object, e As EventArgs) Handles txtendtimeOBF.Leave

        colNameOBF = "obf_EndTime"

        Dim thegetval = Trim(txtendtimeOBF.Text)

        Dim theretval = ""

        If dgvOBF.RowCount = 1 Then
            If thegetval <> "" Then
                dgvOBF.Rows.Add()
                dgvOBFRowindx = dgvOBF.RowCount - 2
            End If
        Else
            If dgvOBF.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvOBF.Rows.Add()
                    dgvOBFRowindx = dgvOBF.RowCount - 2
                End If
            Else
                dgvOBFRowindx = dgvOBF.CurrentRow.Index
            End If
        End If

        Dim dateobj As Object = thegetval.Replace(" ", ":")
        Dim ampm As String = Nothing

        If thegetval <> "" Then
            Try

                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If

                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")

                If ampm = Nothing Then
                    theretval = valtime.ToShortTimeString
                Else
                    theretval = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If
                'Format(valtime, "hh:mm tt")
                haserrinputOBF = 0

                dgvOBF.Item("obf_EndTime", dgvOBFRowindx).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")

                    theretval = valtime.ToShortTimeString
                    'Format(valtime, "hh:mm tt")

                    haserrinputOBF = 0

                    dgvOBF.Item("obf_EndTime", dgvOBFRowindx).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputOBF = 1
                    dgvOBF.Item("obf_EndTime", dgvOBFRowindx).ErrorText = "     Invalid time value"
                Finally
                    If thegetval <> "" Then
                        dgvOBF.Item("obf_EndTime", dgvOBFRowindx).Value = theretval
                        txtendtimeOBF.Text = theretval
                        dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

                        If theretval <> prev_obf_EndTime _
                        And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                            listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
                        End If

                    End If
                End Try
            Finally
                If thegetval <> "" Then
                    dgvOBF.Item("obf_EndTime", dgvOBFRowindx).Value = theretval
                    txtendtimeOBF.Text = theretval
                    dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

                    If theretval <> prev_obf_EndTime _
                        And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                        listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
                    End If

                End If
            End Try
        End If

    End Sub

    Private Sub dtpstartdateOBF_ValueChanged(sender As Object, e As EventArgs) Handles dtpstartdateOBF.ValueChanged

        Dim has_valid_dgvrow As Boolean = False

        Try
            curr_ob_dgvrow = dgvOBF.CurrentRow
            has_valid_dgvrow = (curr_ob_dgvrow IsNot Nothing _
                                And curr_ob_dgvrow.Cells("obf_RowID").Value = Nothing)
        Catch ex As Exception
            has_valid_dgvrow = False
        Finally
            If has_valid_dgvrow Then
                Dim date_value = dtpstartdateOBF.Value
                dtpendateOBF.Value = date_value
                curr_ob_dgvrow.Cells("obf_EndDate").Value = date_value

            End If
        End Try
    End Sub

    Private Sub txtstartdateOBF_TextChanged(sender As Object, e As EventArgs) Handles txtstartdateOBF.TextChanged
    End Sub

    Private Sub dtpstartdateOBF_GotFocus(sender As Object, e As EventArgs) Handles dtpstartdateOBF.GotFocus

        If dgvOBF.RowCount = 1 Then
        Else
            dgvOBF.Item("obf_StartDate", dgvOBFRowindx).Selected = True
        End If

    End Sub

    Private Sub txtstartdateOBF_GotFocus(sender As Object, e As EventArgs) Handles txtstartdateOBF.GotFocus
    End Sub

    Private Sub dtpstartdateOBF_Leave(sender As Object, e As EventArgs) Handles dtpstartdateOBF.Leave

        colNameOBF = "obf_StartDate"

        Dim thegetval = Trim(dtpstartdateOBF.Value)

        Dim theretval = ""

        If dgvOBF.RowCount = 1 Then
            If thegetval <> "" Then
                dgvOBF.Rows.Add()
                dgvOBFRowindx = dgvOBF.RowCount - 2
            End If
        Else
            If dgvOBF.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvOBF.Rows.Add()
                    dgvOBFRowindx = dgvOBF.RowCount - 2
                End If
            Else
                dgvOBFRowindx = dgvOBF.CurrentRow.Index
            End If
        End If

        Dim dateobj As Object = Trim(thegetval)

        If thegetval <> "" Then
            Try
                theretval = Format(CDate(dateobj), machineShortDateFormat)

                haserrinputOBF = 0

                dgvOBF.Item("obf_StartDate", dgvOBFRowindx).ErrorText = Nothing

                If thegetval <> Nothing _
                    And Trim(txtendateOBF.Text) <> Nothing Then

                    Dim date_differ = DateDiff(DateInterval.Day,
                                                CDate(thegetval),
                                                CDate(Trim(txtendateOBF.Text)))

                    If date_differ < 0 Then
                        haserrinputOBF = 1
                        dgvOBF.Item("obf_StartDate", dgvOBFRowindx).ErrorText = "     Invalid date value"
                    Else
                        dgvOBF.Item("obf_StartDate", dgvOBFRowindx).ErrorText = Nothing
                        dgvOBF.Item("obf_EndDate", dgvOBFRowindx).ErrorText = Nothing
                    End If

                    Dim _from = Format(CDate(theretval), "yyyy-MM-dd")
                    Dim _to = Format(CDate(dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Value), "yyyy-MM-dd")

                    Dim invalidOBF = 0

                End If
            Catch ex As Exception
                haserrinputOBF = 1
                dgvOBF.Item("obf_StartDate", dgvOBFRowindx).ErrorText = "     Invalid date value"
            Finally
                If thegetval <> "" Then
                    dgvOBF.Item("obf_StartDate", dgvOBFRowindx).Value = theretval
                    dtpstartdateOBF.Value = Format(CDate(theretval), machineShortDateFormat)
                    dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

                    If theretval <> prev_obf_StartDate _
                        And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                        listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
                    End If

                End If
            End Try

        End If

    End Sub

    Private Sub txtstartdateOBF_Leave(sender As Object, e As EventArgs) Handles txtstartdateOBF.Leave
    End Sub

    Private Sub dtpendateOBF_ValueChanged(sender As Object, e As EventArgs) Handles dtpendateOBF.ValueChanged
    End Sub

    Private Sub txtendateOBF_TextChanged(sender As Object, e As EventArgs) Handles txtendateOBF.TextChanged
    End Sub

    Private Sub dtpendateOBF_GotFocus(sender As Object, e As EventArgs) Handles dtpendateOBF.GotFocus

        If dgvOBF.RowCount = 1 Then
        Else
            dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Selected = True
        End If

    End Sub

    Private Sub txtendateOBF_GotFocus(sender As Object, e As EventArgs) Handles txtendateOBF.GotFocus
    End Sub

    Private Sub dtpendateOBF_Leave(sender As Object, e As EventArgs) Handles dtpendateOBF.Leave

        colNameOBF = "obf_EndDate"

        Dim thegetval = Trim(dtpendateOBF.Value)

        Dim theretval = ""

        If dgvOBF.RowCount = 1 Then
            If thegetval <> "" Then
                dgvOBF.Rows.Add()
                dgvOBFRowindx = dgvOBF.RowCount - 2
                'dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Selected = True
            End If
        Else
            If dgvOBF.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvOBF.Rows.Add()
                    dgvOBFRowindx = dgvOBF.RowCount - 2
                    'dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Selected = True
                End If
            Else
                dgvOBFRowindx = dgvOBF.CurrentRow.Index
            End If
        End If

        Dim dateobj As Object = Trim(thegetval)

        If thegetval <> "" Then
            Try
                theretval = Format(CDate(dateobj), machineShortDateFormat)

                haserrinputOBF = 0

                dgvOBF.Item("obf_EndDate", dgvOBFRowindx).ErrorText = Nothing

                If thegetval <> Nothing _
                    And Trim(txtstartdateOBF.Text) <> Nothing Then

                    Dim date_differ = DateDiff(DateInterval.Day,
                                                CDate(Trim(txtstartdateOBF.Text)),
                                                CDate(thegetval))

                    If date_differ < 0 Then
                        haserrinputOBF = 1
                        dgvOBF.Item("obf_EndDate", dgvOBFRowindx).ErrorText = "     Invalid date value"
                    Else
                        dgvOBF.Item("obf_StartDate", dgvOBFRowindx).ErrorText = Nothing
                        dgvOBF.Item("obf_EndDate", dgvOBFRowindx).ErrorText = Nothing
                    End If

                    Dim _from = Format(CDate(Trim(txtstartdateOBF.Text)), "yyyy-MM-dd")
                    Dim _to = Format(CDate(theretval), "yyyy-MM-dd")

                    Dim invalidOBF = 0

                End If
            Catch ex As Exception
                haserrinputOBF = 1
                dgvOBF.Item("obf_EndDate", dgvOBFRowindx).ErrorText = "     Invalid date value"
            Finally
                If thegetval <> "" Then
                    dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Value = theretval
                    dtpendateOBF.Value = Format(CDate(theretval), machineShortDateFormat)
                    dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

                    If theretval <> prev_obf_EndDate _
                        And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                        listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
                    End If

                End If
            End Try
        Else
            dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Value = Nothing
        End If

    End Sub

    Private Sub txtendateOBF_Leave(sender As Object, e As EventArgs) Handles txtendateOBF.Leave
    End Sub

    Private Sub txtreasonOBF_TextChanged(sender As Object, e As EventArgs) Handles txtreasonOBF.TextChanged
    End Sub

    Private Sub txtreasonOBF_GotFocus(sender As Object, e As EventArgs) Handles txtreasonOBF.GotFocus

        If dgvOBF.RowCount = 1 Then
        Else
            dgvOBF.Item("obf_Reason", dgvOBFRowindx).Selected = True
        End If

    End Sub

    Private Sub txtreasonOBF_Leave(sender As Object, e As EventArgs) Handles txtreasonOBF.Leave

        colNameOBF = "obf_Reason"

        Dim thegetval = Trim(txtreasonOBF.Text)

        If dgvOBF.RowCount = 1 Then
            If thegetval <> "" Then
                dgvOBF.Rows.Add()
                dgvOBFRowindx = dgvOBF.RowCount - 2
                'dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Selected = True
            End If
        Else
            If dgvOBF.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvOBF.Rows.Add()
                    dgvOBFRowindx = dgvOBF.RowCount - 2
                    'dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Selected = True
                End If
            Else
                dgvOBFRowindx = dgvOBF.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvOBF.Item("obf_Reason", dgvOBFRowindx).Value = thegetval
            txtreasonOBF.Text = thegetval
            dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

            If thegetval <> prev_obf_Reason _
                    And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
            End If

        End If

    End Sub

    Private Sub txtcommentsOBF_TextChanged(sender As Object, e As EventArgs) Handles txtcommentsOBF.TextChanged
    End Sub

    Private Sub txtcommentsOBF_GotFocus(sender As Object, e As EventArgs) Handles txtcommentsOBF.GotFocus

        If dgvOBF.RowCount = 1 Then
        Else
            dgvOBF.Item("obf_Comment", dgvOBFRowindx).Selected = True
        End If

    End Sub

    Private Sub txtcommentsOBF_Leave(sender As Object, e As EventArgs) Handles txtcommentsOBF.Leave

        colNameOBF = "obf_Comment"

        Dim thegetval = Trim(txtcommentsOBF.Text)

        If dgvOBF.RowCount = 1 Then
            If thegetval <> "" Then
                dgvOBF.Rows.Add()
                dgvOBFRowindx = dgvOBF.RowCount - 2
                'dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Selected = True
            End If
        Else
            If dgvOBF.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvOBF.Rows.Add()
                    dgvOBFRowindx = dgvOBF.RowCount - 2
                    'dgvOBF.Item("obf_EndDate", dgvOBFRowindx).Selected = True
                End If
            Else
                dgvOBFRowindx = dgvOBF.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvOBF.Item("obf_Comment", dgvOBFRowindx).Value = thegetval
            txtcommentsOBF.Text = thegetval
            dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

            If thegetval <> prev_obf_Comment _
                    And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
            End If
        End If
    End Sub

    Private Sub Label228_Click(sender As Object, e As EventArgs) Handles Label228.Click
    End Sub

    Private Sub cboOBFstatus_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cboOBFstatus.SelectedIndexChanged
        cboOBFstatus_Leave(sender, e)
    End Sub

    Private Sub cboOBFstatus_GotFocus(sender As Object, e As EventArgs) Handles cboOBFstatus.GotFocus
        '
        If dgvOBF.RowCount = 1 Then
        Else
            dgvOBF.Item("obf_Status", dgvOBFRowindx).Selected = True
        End If

    End Sub

    Private Sub cboOBFstatus_Leave(sender As Object, e As EventArgs) Handles cboOBFstatus.Leave

        colNameOBF = "obf_Status"

        Dim thegetval = Trim(cboOBFstatus.Text)

        If dgvOBF.RowCount = 1 Then
            If thegetval <> "" Then
                dgvOBF.Rows.Add()
                dgvOBFRowindx = dgvOBF.RowCount - 2
            End If
        Else
            If dgvOBF.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvOBF.Rows.Add()
                    dgvOBFRowindx = dgvOBF.RowCount - 2
                End If
            Else
                dgvOBFRowindx = dgvOBF.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvOBF.Item("obf_Status", dgvOBFRowindx).Value = thegetval
            cboOBFstatus.Text = thegetval
            dgvOBF.Item("obf_view", dgvOBFRowindx).Value = "view this"

            If thegetval <> prev_obf_Comment _
                    And dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value <> Nothing Then
                listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", dgvOBFRowindx).Value)
            End If

        End If

    End Sub

    Dim atta_nameobf,
        atta_extensnobf,
        thefilepathobf As String

    Private Sub btnBrowseOBF_Click(sender As Object, e As EventArgs) Handles btnBrowseOBF.Click
        'RemoveHandler dgvobf.SelectionChanged, AddressOf dgvobf_SelectionChanged
        atta_nameobf = Nothing
        atta_extensnobf = Nothing

        Static employeeobfRowID As Integer = -1
        Try
            Dim browsefile As OpenFileDialog = New OpenFileDialog()

            'browsefile.Filter = "JPEG(*.jpg)|*.jpg"

            browsefile.Filter = "All files (*.*)|*.*" &
                                "|JPEG (*.jpg)|*.jpg" &
                                "|PNG (*.PNG)|*.png" &
                                "|MS Word 97-2003 Document (*.doc)|*.doc" &
                                "|MS Word Document (*.docx)|*.docx" &
                                "|MS Excel 97-2003 Workbook (*.xls)|*.xls" &
                                "|MS Excel Workbook (*.xlsx)|*.xlsx"

            '|" & _
            '"PNG(*.PNG)|*.png|" & _
            '"Bitmap(*.BMP)|*.bmp"
            If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then
                With dgvOBF
                    '.ClearSelection()
                    .Focus()

                    thefilepathobf = browsefile.FileName
                    atta_nameobf = Path.GetFileNameWithoutExtension(thefilepathobf)
                    atta_extensnobf = Path.GetExtension(thefilepathobf)

                    If .CurrentRow.IsNewRow Then
                        Dim e_rowindx As Integer = .CurrentRow.Index
                        Dim currcol As String = .Columns(.CurrentCell.ColumnIndex).Name
                        .Rows.Add()
                        .Item("obf_Image", e_rowindx).Value = Nothing
                        .Item("obf_Image", e_rowindx).Value = convertFileToByte(thefilepathobf)

                        .Item("obf_attafilename", e_rowindx).Value = atta_nameobf
                        .Item("obf_attafileextensn", e_rowindx).Value = atta_extensnobf

                        .Item(currcol, e_rowindx).Selected = True
                    Else
                        .CurrentRow.Cells("obf_Image").Value = Nothing
                        .CurrentRow.Cells("obf_Image").Value = convertFileToByte(thefilepathobf)

                        .CurrentRow.Cells("obf_attafilename").Value = atta_nameobf
                        .CurrentRow.Cells("obf_attafileextensn").Value = atta_extensnobf

                        If employeeobfRowID <> Val(dgvOBF.Item("obf_RowID", .CurrentRow.Index).Value) Then
                            listofEditRowOBF.Add(dgvOBF.Item("obf_RowID", .CurrentRow.Index).Value)
                        End If
                    End If

                    .Focus()
                End With
            Else

            End If
        Catch ex As Exception
            MsgBox(ex.Message & " Error on Image")
        Finally
            dgvOBF_SelectionChanged(sender, e)
            'AddHandler dgvobf.SelectionChanged, AddressOf dgvobf_SelectionChanged
        End Try
    End Sub

    Private Sub btnClearOBF_Click(sender As Object, e As EventArgs) Handles btnClearOBF.Click
        Static employeeEmpOTRowID As Integer = -1

        pbempOBF.Image = Nothing

        If dgvOBF.RowCount = 1 Then
            dgvOBF.Item("obf_Image", 0).Value = Nothing
        Else
            If dgvOBF.CurrentRow.IsNewRow = False Then
                dgvOBF.CurrentRow.Cells("obf_Image").Value = Nothing
                If employeeEmpOTRowID <> Val(dgvOBF.CurrentRow.Cells("obf_RowID").Value) Then
                    listofEditRowOBF.Add(dgvOBF.CurrentRow.Cells("obf_RowID").Value)
                End If
            End If
        End If

    End Sub

    Dim isdlOBF As SByte = 0

    Private Sub btndlOBF_Click(sender As Object, e As EventArgs) Handles btndlOBF.Click
        isdlOBF = 1

        If dgvOBF.RowCount <> 1 Then
            dgvOBF.Item("obf_view",
                             dgvOBF.CurrentRow.Index).Selected = True

            Dim dgvceleventarg As New DataGridViewCellEventArgs(obf_view.Index,
                                                                dgvOBF.CurrentRow.Index)

            dgvOBF_CellContentClick(sender, dgvceleventarg)

        End If

    End Sub

    Private Sub tsbtnDelOffBusi_Click(sender As Object, e As EventArgs) Handles tsbtnDelOffBusi.Click

        tsbtnDelOffBusi.Enabled = False

        dgvOBF.Focus()

        If Not dgvOBF.CurrentRow.IsNewRow Then

            Dim result = MessageBox.Show("Are you sure you want to delete O.B. ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

            If result = DialogResult.Yes Then

                dgvOBF.EndEdit(True)

                Dim n_ExecuteQuery As _
                    New ExecuteQuery("DELETE FROM employeeofficialbusiness WHERE RowID = '" & dgvOBF.CurrentRow.Cells("obf_RowID").Value & "';")

                dgvOBF.Rows.Remove(dgvOBF.CurrentRow)

            End If

        End If

        tsbtnDelOffBusi.Enabled = True

    End Sub

    Private Sub OBImport_Click(sender As Object, e As EventArgs) Handles OBImport.Click
        Dim browsefile As New OpenFileDialog()

        browsefile.Filter = str_ms_excel_file_extensn

        Try

            If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

                filepath = IO.Path.GetFullPath(browsefile.FileName)

                Dim catchDatSet =
                    getWorkBookAsDataSet(filepath,
                                         Me.Name)

                If (catchDatSet Is Nothing) = False And Trim(filepath).Length > 0 Then

                    Dim n_ob As New ImportOB(catchDatSet, Me)

                    Dim objNewThread As New Thread(AddressOf n_ob.DoImport)

                    Static indx As Integer = 0

                    indx += 1

                    objNewThread.Name = String.Concat("ImportOB", indx)

                    objNewThread.IsBackground = True

                    objNewThread.Start()

                    threadArrayList.Add(objNewThread)

                End If

                'MsgBox("Official Business Successfully Imported !")

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Official Business Import Failed")
        End Try

    End Sub

#End Region

#Region "Bonus"

    Public categBonusID As String

    Public bonus_type As New AutoCompleteStringCollection

    Private Sub tbpBonus_Click(sender As Object, e As EventArgs) Handles tbpBonus.Click

    End Sub

    Dim view_IDBon As Integer = Nothing

    Sub tbpBonus_Enter(sender As Object, e As EventArgs) Handles tbpBonus.Enter

        tabpageText(tabIndx)

        tbpBonus.Text = "EMPLOYEE BONUS               "

        Label25.Text = "EMPLOYEE BONUS"
        Static once As SByte = 0

        If once = 0 Then
            once = 1

            txtbonamt.ContextMenu = New ContextMenu

            cbobontype.ContextMenu = New ContextMenu

            cbobonfreq.ContextMenu = New ContextMenu

            categBonusID = EXECQUER("SELECT RowID FROM category WHERE OrganizationID=" & orgztnID & " AND CategoryName='" & "Bonus" & "' LIMIT 1;")

            If Val(categBonusID) = 0 Then
                categBonusID = INSUPD_category(, "Bonus")
            End If

            enlistTheLists("SELECT CONCAT(COALESCE(PartNo,''),'@',RowID) FROM product WHERE CategoryID='" & categBonusID & "' AND OrganizationID=" & orgztnID & ";",
                           bonus_type) 'cboallowtype

            For Each strval In bonus_type
                cbobontype.Items.Add(getStrBetween(strval, "", "@"))
                bon_Type.Items.Add(getStrBetween(strval, "", "@"))
            Next

            enlistToCboBox("SELECT DisplayValue FROM listofval WHERE Type='Allowance Frequency' AND Active='Yes' AND OrderBy=3;",
                           cbobonfreq)

            'enlistToCboBox("SELECT DisplayValue FROM listofval WHERE Type='Allowance Frequency' AND Active='Yes' ORDER BY OrderBy;", _
            '               cbobonfreq)

            For Each strval In cbobonfreq.Items
                bon_Frequency.Items.Add(strval)
            Next

            AddHandler dgvempbon.SelectionChanged, AddressOf dgvempbon_SelectionChanged

            view_IDBon = VIEW_privilege("Employee Bonus", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDBon)

            If formuserprivilege.Count = 0 Then

                tsbtnNewBon.Visible = 0
                tsbtnSaveBon.Visible = 0
                tsbtnDelBon.Visible = False
                dontUpdateBon = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        tsbtnNewBon.Visible = 0
                        tsbtnSaveBon.Visible = 0
                        tsbtnDelBon.Visible = False
                        dontUpdateBon = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            tsbtnNewBon.Visible = 0
                        Else
                            tsbtnNewBon.Visible = 1
                        End If

                        If drow("Deleting").ToString = "N" Then
                            tsbtnDelBon.Visible = False
                        Else
                            tsbtnDelBon.Visible = True
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateBon = 1
                        Else
                            dontUpdateBon = 0
                        End If

                    End If

                Next

            End If

        End If

        tabIndx = 16 'TabControl1.SelectedIndex

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Sub VIEW_employeebonus(ByVal bon_EmployeeID As Object)

        Dim param(1, 2) As Object

        param(0, 0) = "ebon_EmployeeID"
        param(1, 0) = "ebon_OrganizationID"

        param(0, 1) = bon_EmployeeID
        param(1, 1) = orgztnID

        EXEC_VIEW_PROCEDURE(param,
                           "VIEW_employeebonus",
                           dgvempbon, , 1)

    End Sub

    Function INSUPD_employeebonus(Optional bon_RowID As Object = Nothing,
                                      Optional bon_EmployeeID As Object = Nothing,
                                      Optional bon_AllowanceFrequency As Object = Nothing,
                                      Optional bon_EffectiveStartDate As Object = Nothing,
                                      Optional bon_EffectiveEndDate As Object = Nothing,
                                      Optional bon_BonusAmount As Object = Nothing,
                                      Optional bon_ProductID As Object = Nothing) As Object

        Dim params(9, 2) As Object

        params(0, 0) = "bon_RowID"
        params(1, 0) = "bon_OrganizationID"
        params(2, 0) = "bon_EmployeeID"
        params(3, 0) = "bon_CreatedBy"
        params(4, 0) = "bon_LastUpdBy"
        params(5, 0) = "bon_ProductID"
        params(6, 0) = "bon_AllowanceFrequency"
        params(7, 0) = "bon_EffectiveStartDate"
        params(8, 0) = "bon_EffectiveEndDate"
        params(9, 0) = "bon_BonusAmount"

        params(0, 1) = If(bon_RowID = Nothing, DBNull.Value, bon_RowID)
        params(1, 1) = orgztnID
        params(2, 1) = bon_EmployeeID
        params(3, 1) = z_User
        params(4, 1) = z_User
        params(5, 1) = bon_ProductID
        params(6, 1) = bon_AllowanceFrequency
        params(7, 1) = Format(CDate(bon_EffectiveStartDate), "yyyy-MM-dd")
        params(8, 1) = If(bon_EffectiveEndDate = Nothing, DBNull.Value, Format(CDate(bon_EffectiveEndDate), "yyyy-MM-dd"))
        params(9, 1) = bon_BonusAmount

        INSUPD_employeebonus = EXEC_INSUPD_PROCEDURE(params,
                                                    "INSUPD_employeebonus",
                                                    "bon_ID")

    End Function

    Private Sub tsbtnNewBon_Click(sender As Object, e As EventArgs) Handles tsbtnNewBon.Click
        dgvempbon.EndEdit(1)
        dgvempbon.Focus()

        For Each dgvrow As DataGridViewRow In dgvempbon.Rows
            If dgvrow.IsNewRow Then
                dgvbonRowindx = dgvrow.Index
                dgvrow.Cells("bon_Type").Selected = 1
                Exit For
            End If
        Next

        cbobontype.SelectedIndex = -1
        cbobontype.Text = ""
        cbobonfreq.SelectedIndex = -1
        cbobonfreq.Text = ""
        dtpbonstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
        dtpbonenddate.Value = Format(CDate(dbnow), machineShortDateFormat)
        txtbonamt.Text = ""

        dgvempbon_SelectionChanged(sender, e)

    End Sub

    Dim dontUpdateBon As SByte = 0

    Private Sub tsbtnSaveBon_Click(sender As Object, e As EventArgs) Handles tsbtnSaveBon.Click
        pbEmpPicBon.Focus()

        cbobontype.Focus()

        pbEmpPicBon.Focus()

        cbobonfreq.Focus()

        pbEmpPicBon.Focus()

        dtpbonstartdate.Focus()

        pbEmpPicBon.Focus()

        dtpbonenddate.Focus()

        pbEmpPicBon.Focus()

        txtbonamt.Focus()

        pbEmpPicBon.Focus()

        dgvempbon.EndEdit(1)

        If dontUpdateBon = 1 Then
            listofEditRowBon.Clear()
        End If

        If dgvEmp.RowCount = 0 Then
            Exit Sub
        End If
        'RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        Static once As SByte = 0

        For Each dgvrow As DataGridViewRow In dgvempbon.Rows
            With dgvrow
                If .IsNewRow = 0 Then
                    If listofEditRowBon.Contains(dgvrow.Cells("bon_RowID").Value) Then

                        INSUPD_employeebonus(.Cells("bon_RowID").Value,
                                                 dgvEmp.CurrentRow.Cells("RowID").Value,
                                                 .Cells("bon_Frequency").Value,
                                                 .Cells("bon_Start").Value,
                                                 .Cells("bon_End").Value,
                                                 .Cells("bon_Amount").Value,
                                                 .Cells("bon_ProdID").Value)
                    Else
                        If .Cells("bon_RowID").Value = Nothing And
                            tsbtnNewBon.Visible = True Then

                            .Cells("bon_RowID").Value = INSUPD_employeebonus(,
                                                     dgvEmp.CurrentRow.Cells("RowID").Value,
                                                     .Cells("bon_Frequency").Value,
                                                     .Cells("bon_Start").Value,
                                                     .Cells("bon_End").Value,
                                                     .Cells("bon_Amount").Value,
                                                     .Cells("bon_ProdID").Value)
                        End If
                    End If
                End If

            End With
        Next

        listofEditRowBon.Clear()

        InfoBalloon("Changes made in Employee ID '" & dgvEmp.CurrentRow.Cells("Column1").Value & "' has successfully saved.", "Changes successfully save", lblforballoon, 0, -69)

        'AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

    End Sub

    Private Sub tsbtnCancelBon_Click(sender As Object, e As EventArgs) Handles tsbtnCancelBon.Click
        listofEditRowBon.Clear()
        dgvEmp_SelectionChanged(sender, e)
    End Sub

    Private Sub tsbtnDelBon_Click(sender As Object, e As EventArgs) Handles tsbtnDelBon.Click

        Dim bonus_RowID = dgvempbon.Tag

        If bonus_RowID = Nothing Then
        Else

            Dim result = MessageBox.Show("Are you sure you want to delete bonus ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

            If result = DialogResult.Yes Then
                dgvempbon.Focus()
                Dim n_ExecuteQuery As _
                    New ExecuteQuery("CALL DEL_employeebonus('" & bonus_RowID & "');")

                dgvempbon.Rows.Remove(dgvempbon.CurrentRow)

            End If

        End If

    End Sub

    Private Sub dgvempbon_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempbon.CellContentClick
    End Sub

    Dim bon_prevval(4) As Object

    Private Sub dgvempbon_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvempbon.SelectionChanged
        If dgvempbon.RowCount > 1 Then
            With dgvempbon.CurrentRow

                dgvbonRowindx = .Index
                If .IsNewRow = 0 Then
                    bon_prevval(0) = .Cells("bon_Type").Value
                    bon_prevval(1) = .Cells("bon_Amount").Value
                    bon_prevval(2) = .Cells("bon_Frequency").Value
                    bon_prevval(3) = .Cells("bon_Start").Value
                    bon_prevval(4) = .Cells("bon_End").Value

                    cbobontype.Text = .Cells("bon_Type").Value
                    cbobonfreq.Text = .Cells("bon_Frequency").Value

                    If Trim(.Cells("bon_Start").Value) <> Nothing Then
                        dtpbonstartdate.Value = Format(CDate(.Cells("bon_Start").Value), machineShortDateFormat)
                    Else
                        dtpbonstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
                    End If

                    If Trim(.Cells("bon_End").Value) <> Nothing Then
                        dtpbonenddate.Value = Format(CDate(.Cells("bon_Start").Value), machineShortDateFormat)
                    Else
                        dtpbonenddate.Value = Format(CDate(dbnow), machineShortDateFormat)
                    End If

                    txtbonamt.Text = .Cells("bon_Amount").Value
                    dgvempbon.Tag = .Cells("bon_RowID").Value
                Else

                    bon_prevval(0) = ""
                    bon_prevval(1) = ""
                    bon_prevval(2) = ""
                    bon_prevval(3) = ""
                    bon_prevval(4) = ""

                    cbobontype.Text = ""
                    cbobonfreq.Text = ""
                    dtpbonstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
                    dtpbonenddate.Value = Format(CDate(dbnow), machineShortDateFormat)
                    txtbonamt.Text = ""
                    dgvempbon.Tag = Nothing
                End If

            End With
        Else
            dgvbonRowindx = 0
            dgvempbon.Tag = Nothing
            bon_prevval(0) = ""
            bon_prevval(1) = ""
            bon_prevval(2) = ""
            bon_prevval(3) = ""
            bon_prevval(4) = ""

            dtpbonstartdate.Value = Format(CDate(dbnow), machineShortDateFormat)
            dtpbonenddate.Value = Format(CDate(dbnow), machineShortDateFormat)
        End If
    End Sub

    Public listofEditRowBon As New List(Of String)

    Private Sub dgvempbon_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempbon.CellEndEdit

        dgvempbon.ShowCellErrors = 1

        Dim colname = dgvempbon.Columns(e.ColumnIndex).Name

        With dgvempbon.Rows(e.RowIndex)
            If Val(.Cells("bon_RowID").Value) <> 0 Then
                listofEditRowBon.Add(.Cells("bon_RowID").Value)
            Else
            End If

            If colname = "bon_Start" Or colname = "bon_End" Then

            ElseIf colname = "bon_Type" Then

                For Each strval In bonus_type
                    Dim strcompare = Trim(dgvempbon.Item("bon_Type", e.RowIndex).Value.ToString)
                    If strcompare = getStrBetween(strval, "", "@") Then
                        dgvempbon.Item("bon_ProdID", e.RowIndex).Value = StrReverse(getStrBetween(StrReverse(strval), "", "@"))
                        Exit For
                    End If
                Next

            End If

        End With

    End Sub

    Dim dgvbonRowindx As Integer = 0

    Dim bonProductID As String

    Private Sub cbobontype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbobontype.SelectedIndexChanged

    End Sub

    Private Sub cbobontype_GotFocus(sender As Object, e As EventArgs) Handles cbobontype.GotFocus

        If dgvempbon.RowCount = 1 Then
        Else
            dgvempbon.Item("bon_Type", dgvbonRowindx).Selected = 1
        End If

    End Sub

    Private Sub cbobontype_Leave(sender As Object, e As EventArgs) Handles cbobontype.Leave

        For Each strval In bonus_type
            If Trim(cbobontype.Text) = getStrBetween(strval, "", "@") Then
                bonProductID = StrReverse(getStrBetween(StrReverse(strval), "", "@"))
                Exit For
            End If
        Next

        Dim thegetval = Trim(cbobontype.Text)

        If dgvempbon.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempbon.Rows.Add()
                dgvbonRowindx = dgvempbon.RowCount - 2
            End If
        Else
            If dgvempbon.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempbon.Rows.Add()
                    dgvbonRowindx = dgvempbon.RowCount - 2
                End If
            Else
                dgvbonRowindx = dgvempbon.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempbon.Item("bon_Type", dgvbonRowindx).Value = thegetval
            dgvempbon.Item("bon_ProdID", dgvbonRowindx).Value = bonProductID

            cbobontype.Text = thegetval

        End If

        With dgvempbon.Rows(dgvbonRowindx)
            If Val(.Cells("bon_RowID").Value) <> 0 Then
                If thegetval <> bon_prevval(0) Then
                    listofEditRowBon.Add(.Cells("bon_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Private Sub cbobonfreq_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbobonfreq.SelectedIndexChanged

    End Sub

    Dim IsBonusOneTimeFreq As Boolean = False

    Private Sub cbobonfreq_SelectedValueChanged(sender As Object, e As EventArgs) Handles cbobonfreq.SelectedValueChanged

        dtpbonstartdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]

        Select Case cbobonfreq.SelectedIndex
            Case -1 'Nothing
                dtpbonstartdate.Enabled = 0
                dtpbonenddate.Enabled = 0

                lblbonstartdate.Visible = 0
                lblbonenddate.Visible = 0
            Case 0 'Daily

                IsBonusOneTimeFreq = 1
                dtpbonstartdate.Enabled = 1
                dtpbonenddate.Enabled = 0

                lblbonstartdate.Visible = 1
                lblbonenddate.Visible = 0
            Case 1 'Monthly

                dtpbonstartdate.Enabled = 1
                dtpbonenddate.Enabled = 1

                lblbonstartdate.Visible = 1
                lblbonenddate.Visible = 1
            Case 2 'One time
                dtpbonstartdate.Enabled = 1
                dtpbonenddate.Enabled = 0
                IsBonusOneTimeFreq = 1
                lblbonstartdate.Visible = 1
                lblbonenddate.Visible = 0
            Case 3 To 4 'Semi-monthly & Weekly
                dtpbonstartdate.Enabled = 1
                dtpbonenddate.Enabled = 1

                lblbonstartdate.Visible = 1
                lblbonenddate.Visible = 1

        End Select

    End Sub

    Private Sub cbobonfreq_GotFocus(sender As Object, e As EventArgs) Handles cbobonfreq.GotFocus

        If dgvempbon.RowCount = 1 Then
        Else
            dgvempbon.Item("bon_Frequency", dgvbonRowindx).Selected = 1
        End If

    End Sub

    Private Sub cbobonfreq_Leave(sender As Object, e As EventArgs) Handles cbobonfreq.Leave

        Dim thegetval = Trim(cbobonfreq.Text)

        If dgvempbon.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempbon.Rows.Add()
                dgvbonRowindx = dgvempbon.RowCount - 2
            End If
        Else
            If dgvempbon.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempbon.Rows.Add()
                    dgvbonRowindx = dgvempbon.RowCount - 2
                End If
            Else
                dgvbonRowindx = dgvempbon.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempbon.Item("bon_Frequency", dgvbonRowindx).Value = thegetval

            cbobonfreq.Text = thegetval

        End If

        With dgvempbon.Rows(dgvbonRowindx)
            If Val(.Cells("bon_RowID").Value) <> 0 Then
                If thegetval <> bon_prevval(2) Then
                    listofEditRowBon.Add(.Cells("bon_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Private Sub dtpbonstartdate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtpbonstartdate.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        Static j As SByte = 0
        Static a As SByte = 0
        Static m As SByte = 0

        Dim MM As Object = Format(CDate(dtpbonstartdate.Value), "MMM")
        Dim dd As Object = CDate(dtpbonstartdate.Value).Day
        dd = If(dd.ToString.Length = 1, "0" & dd, dd)
        Dim yyyy As Object = CDate(dtpbonstartdate.Value).Year

        If e_asc = 74 Or e_asc = 106 Then 'j

            Select Case j
                Case 0
                    dtpbonstartdate.Value = CDate("Jan-" & dd & "-" & yyyy)
                Case 1
                    dtpbonstartdate.Value = CDate("Jun-" & dd & "-" & yyyy)
                Case 2
                    dtpbonstartdate.Value = CDate("Jul-" & dd & "-" & yyyy)
            End Select

            j += 1

            If j >= 3 Then
                j = 0
            End If

        ElseIf e_asc = 65 Or e_asc = 97 Then 'a

            Select Case a
                Case 0
                    dtpbonstartdate.Value = CDate("Apr-" & dd & "-" & yyyy)
                Case 1
                    dtpbonstartdate.Value = CDate("Aug-" & dd & "-" & yyyy)
            End Select

            a += 1

            If a >= 2 Then
                a = 0
            End If

        ElseIf e_asc = 77 Or e_asc = 109 Then 'm
            Select Case m
                Case 0
                    dtpbonstartdate.Value = CDate("Mar-" & dd & "-" & yyyy)
                Case 1
                    dtpbonstartdate.Value = CDate("May-" & dd & "-" & yyyy)
            End Select

            m += 1

            If m >= 2 Then
                m = 0
            End If

        ElseIf e_asc = 83 Or e_asc = 115 Then 's
            dtpbonstartdate.Value = CDate("Sep-" & dd & "-" & yyyy)
        ElseIf e_asc = 78 Or e_asc = 110 Then 'n
            dtpbonstartdate.Value = CDate("Nov-" & dd & "-" & yyyy)
        ElseIf e_asc = 68 Or e_asc = 100 Then 'd
            dtpbonstartdate.Value = CDate("Dec-" & dd & "-" & yyyy)
        ElseIf e_asc = 79 Or e_asc = 111 Then 'o
            dtpbonstartdate.Value = CDate("Oct-" & dd & "-" & yyyy)
        Else
            dtpbonstartdate.Value = CDate(MM & "-" & dd & "-" & yyyy)
        End If
    End Sub

    Private Sub dtpbonstartdate_ValueChanged(sender As Object, e As EventArgs) Handles dtpbonstartdate.ValueChanged

        If DateDiff(DateInterval.Day, CDate(dtpbonstartdate.Value), CDate(dtpbonenddate.Value)) < 0 Or IsBonusOneTimeFreq Then
            dtpbonenddate.Value = dtpbonstartdate.Value
        End If

    End Sub

    Dim dtpbonstartdateval As Object = Nothing

    Private Sub dtpbonstartdate_TextChanged(sender As Object, e As EventArgs) Handles dtpbonstartdate.TextChanged
        dtpbonstartdateval = dtpbonstartdate.Value
    End Sub

    Private Sub dtpbonstartdate_GotFocus(sender As Object, e As EventArgs) Handles dtpbonstartdate.GotFocus

        If dgvempbon.RowCount = 1 Then
        Else
            dgvempbon.Item("bon_Start", dgvbonRowindx).Selected = 1
        End If

    End Sub

    Private Sub dtpbonstartdate_Leave(sender As Object, e As EventArgs) Handles dtpbonstartdate.Leave

        Dim thegetval = If(dtpbonstartdateval = Nothing, Trim(dtpbonstartdate.Value), Trim(dtpbonstartdateval))

        If dgvempbon.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempbon.Rows.Add()
                dgvbonRowindx = dgvempbon.RowCount - 2
            End If
        Else
            If dgvempbon.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempbon.Rows.Add()
                    dgvbonRowindx = dgvempbon.RowCount - 2
                End If
            Else
                dgvbonRowindx = dgvempbon.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempbon.Item("bon_Start", dgvbonRowindx).Value = Format(CDate(thegetval), machineShortDateFormat)

            dtpbonstartdate.Value = Format(CDate(thegetval), machineShortDateFormat)

        End If

        With dgvempbon.Rows(dgvbonRowindx)
            If Val(.Cells("bon_RowID").Value) <> 0 Then
                If thegetval <> bon_prevval(3) Then
                    listofEditRowBon.Add(.Cells("bon_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Private Sub dtpbonenddate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtpbonenddate.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        Static j As SByte = 0
        Static a As SByte = 0
        Static m As SByte = 0

        Dim MM = Format(CDate(dtpbonenddate.Value), "MMM")
        Dim dd = CDate(dtpbonenddate.Value).Day
        dd = If(dd.ToString.Length = 1, "0" & dd, dd)
        Dim yyyy = CDate(dtpbonenddate.Value).Year

        If e_asc = 74 Or e_asc = 106 Then 'j

            Select Case j
                Case 0
                    dtpbonenddate.Value = CDate("Jan-" & dd & "-" & yyyy)
                Case 1
                    dtpbonenddate.Value = CDate("Jun-" & dd & "-" & yyyy)
                Case 2
                    dtpbonenddate.Value = CDate("Jul-" & dd & "-" & yyyy)
            End Select

            j += 1

            If j >= 3 Then
                j = 0
            End If

        ElseIf e_asc = 65 Or e_asc = 97 Then 'a

            Select Case a
                Case 0
                    dtpbonenddate.Value = CDate("Apr-" & dd & "-" & yyyy)
                Case 1
                    dtpbonenddate.Value = CDate("Aug-" & dd & "-" & yyyy)
            End Select

            a += 1

            If a >= 2 Then
                a = 0
            End If

        ElseIf e_asc = 77 Or e_asc = 109 Then 'm
            Select Case m
                Case 0
                    dtpbonenddate.Value = CDate("Mar-" & dd & "-" & yyyy)
                Case 1
                    dtpbonenddate.Value = CDate("May-" & dd & "-" & yyyy)
            End Select

            m += 1

            If m >= 2 Then
                m = 0
            End If

        ElseIf e_asc = 83 Or e_asc = 115 Then 's
            dtpbonenddate.Value = CDate("Sep-" & dd & "-" & yyyy)
        ElseIf e_asc = 78 Or e_asc = 110 Then 'n
            dtpbonenddate.Value = CDate("Nov-" & dd & "-" & yyyy)
        ElseIf e_asc = 68 Or e_asc = 100 Then 'd
            dtpbonenddate.Value = CDate("Dec-" & dd & "-" & yyyy)
        ElseIf e_asc = 79 Or e_asc = 111 Then 'o
            dtpbonenddate.Value = CDate("Oct-" & dd & "-" & yyyy)
        Else
            dtpbonenddate.Value = CDate(MM & "-" & dd & "-" & yyyy)
        End If
    End Sub

    Private Sub dtpbonenddate_ValueChanged(sender As Object, e As EventArgs) Handles dtpbonenddate.ValueChanged

        If DateDiff(DateInterval.Day, CDate(dtpbonstartdate.Value), CDate(dtpbonenddate.Value)) < 0 Or IsBonusOneTimeFreq Then
            dtpbonenddate.Value = dtpbonstartdate.Value
        End If

    End Sub

    Dim dtpbonenddateval As Object = Nothing

    Private Sub dtpbonenddate_TextChanged(sender As Object, e As EventArgs) Handles dtpbonenddate.TextChanged
        dtpbonenddateval = dtpbonenddate.Value
    End Sub

    Private Sub dtpbonenddate_GotFocus(sender As Object, e As EventArgs) Handles dtpbonenddate.GotFocus

        If dgvempbon.RowCount = 1 Then
        Else
            dgvempbon.Item("bon_End", dgvbonRowindx).Selected = 1
        End If

    End Sub

    Private Sub dtpbonenddate_Leave(sender As Object, e As EventArgs) Handles dtpbonenddate.Leave

        Dim thegetval = If(dtpbonenddateval = Nothing, Trim(dtpbonenddate.Value), Trim(dtpbonenddateval))

        If dgvempbon.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempbon.Rows.Add()
                dgvbonRowindx = dgvempbon.RowCount - 2
            End If
        Else
            If dgvempbon.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempbon.Rows.Add()
                    dgvbonRowindx = dgvempbon.RowCount - 2
                End If
            Else
                dgvbonRowindx = dgvempbon.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempbon.Item("bon_End", dgvbonRowindx).Value = Format(CDate(thegetval), machineShortDateFormat)

            dtpbonenddate.Value = Format(CDate(thegetval), machineShortDateFormat)

        End If

        With dgvempbon.Rows(dgvbonRowindx)
            If Val(.Cells("bon_RowID").Value) <> 0 Then
                If thegetval <> bon_prevval(4) Then
                    listofEditRowBon.Add(.Cells("bon_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Private Sub txtbonamt_TextChanged(sender As Object, e As EventArgs) Handles txtbonamt.TextChanged
    End Sub

    Private Sub txtbonamt_GotFocus(sender As Object, e As EventArgs) Handles txtbonamt.GotFocus

        If dgvempbon.RowCount = 1 Then
        Else
            dgvempbon.Item("bon_Amount", dgvbonRowindx).Selected = 1
        End If

    End Sub

    Private Sub txtbonamt_Leave(sender As Object, e As EventArgs) Handles txtbonamt.Leave

        Dim thegetval = Trim(txtbonamt.Text)

        If dgvempbon.RowCount = 1 Then
            If thegetval <> "" Then
                dgvempbon.Rows.Add()
                dgvbonRowindx = dgvempbon.RowCount - 2
            End If
        Else
            If dgvempbon.CurrentRow.IsNewRow Then
                If thegetval <> "" Then
                    dgvempbon.Rows.Add()
                    dgvbonRowindx = dgvempbon.RowCount - 2
                End If
            Else
                dgvbonRowindx = dgvempbon.CurrentRow.Index
            End If
        End If

        If thegetval <> "" Then
            dgvempbon.Item("bon_Amount", dgvbonRowindx).Value = thegetval

            txtbonamt.Text = thegetval

        End If

        With dgvempbon.Rows(dgvbonRowindx)
            If Val(.Cells("bon_RowID").Value) <> 0 Then
                If thegetval <> bon_prevval(1) Then
                    listofEditRowBon.Add(.Cells("bon_RowID").Value)
                End If
            Else
            End If
        End With

    End Sub

    Private Sub txtbonamt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbonamt.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtbonamt.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtbonamt.Text.Contains(".") Then
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

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        With newProdBonus
            .Show()
            .BringToFront()
            .TextBox1.Focus()
        End With
    End Sub

    Private Sub cbobonfreq_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbobonfreq.KeyPress, cbobontype.KeyPress
        e.Handled = True
    End Sub

#End Region

#Region "Attachment"

    Private Sub tbpAttachment_Click(sender As Object, e As EventArgs) Handles tbpAttachment.Click
    End Sub

    Dim computertemppath = Nothing

    Dim view_IDAttach As Integer = Nothing

    Sub tbpAttachment_Enter(sender As Object, e As EventArgs) Handles tbpAttachment.Enter

        tabpageText(tabIndx)

        tbpAttachment.Text = "ATTACHMENT               "

        Label25.Text = "ATTACHMENT"

        Static once As SByte = 0

        If once = 0 Then
            once = 1

            computertemppath = Path.GetTempPath

            enlistToCboBox("SELECT DisplayValue FROM listofval WHERE Type='Employee Checklist' ORDER BY OrderBy;",
                            cboattatype)

            For Each strval In cboattatype.Items
                eatt_Type.Items.Add(strval)
            Next

            AddHandler dgvempatta.SelectionChanged, AddressOf dgvempatta_SelectionChanged

            dgvempatta.Focus()

            view_IDAttach = VIEW_privilege("Employee Attachment", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDAttach)

            If formuserprivilege.Count = 0 Then

                tsbtnNewAtta.Visible = 0
                tsbtnSaveAtta.Visible = 0

                dontUpdateAtta = 1
            Else
                For Each drow In formuserprivilege
                    If drow("ReadOnly").ToString = "Y" Then
                        tsbtnNewAtta.Visible = 0
                        tsbtnSaveAtta.Visible = 0

                        dontUpdateAtta = 1
                        Exit For
                    Else
                        If drow("Creates").ToString = "N" Then
                            tsbtnNewAtta.Visible = 0
                        Else
                            tsbtnNewAtta.Visible = 1
                        End If

                        If drow("Updates").ToString = "N" Then
                            dontUpdateAtta = 1
                        Else
                            dontUpdateAtta = 0
                        End If

                    End If

                Next

            End If

        End If

        tabIndx = 17

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub cboattatype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboattatype.SelectedIndexChanged
    End Sub

    Private Sub cboattatype_Leave(sender As Object, e As EventArgs) Handles cboattatype.Leave
    End Sub

    Private Sub dgvempatta_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempatta.CellEndEdit

        Dim rowindx = e.RowIndex

        Dim viewthis = dgvempatta.Columns(e.ColumnIndex).Name

        If viewthis = "eatt_Type" Then

            For Each strval In cboattatype.Items
                If strval = Trim(dgvempatta.Item("eatt_Type", rowindx).Value) Then
                    dgvempatta.Item("Column38", rowindx).Value = strval
                    dgvempatta.Item("eatt_viewthis", rowindx).Value = "view this"
                    Exit For
                End If
            Next

        End If

        listofEditRoweatt.Add(dgvempatta.Item("eatt_RowID", rowindx).Value)

    End Sub

    Private Sub dgvempatta_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvempatta.DataError
    End Sub

    Private Sub dgvempatta_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempatta.CellContentClick

        If dgvempatta.RowCount = 1 Then
            MsgBox("Nothing to view.", MsgBoxStyle.Information)
        Else
            Dim viewthis = dgvempatta.Columns(dgvempatta.CurrentCell.ColumnIndex).Name

            If viewthis = "eatt_viewthis" Then
                If dgvempatta.CurrentRow.Cells("eatt_AttachedFile").Value IsNot Nothing Then

                    Dim savefilepath = Path.GetTempPath &
                                             dgvempatta.CurrentRow.Cells("eatt_FileName").Value &
                                             dgvempatta.CurrentRow.Cells("eatt_FileType").Value

                    If Trim(dgvempatta.CurrentRow.Cells("eatt_FileType").Value) = Nothing Then
                    Else
                        If Trim(dgvempatta.CurrentRow.Cells("eatt_FileName").Value) = Nothing Then
                            dgvempatta.CurrentRow.Cells("eatt_FileName").Selected = 1
                            dgvempatta.BeginEdit(1)
                            InfoBalloon("Please input a file name.", "Attachment has no file name", Label235, 0, -69)
                        Else
                            Dim fs As New FileStream(savefilepath,
                                                     FileMode.Create)

                            Dim blob As Byte() = DirectCast(dgvempatta.CurrentRow.Cells("eatt_AttachedFile").Value, Byte())
                            fs.Write(blob, 0, blob.Length)
                            fs.Close()
                            fs = Nothing

                            Process.Start(savefilepath)
                        End If

                    End If
                Else
                    MsgBox("Nothing to view", MsgBoxStyle.Information)

                End If
            End If
        End If
    End Sub

    Private Sub dgvempatta_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvempatta.SelectionChanged
        If dgvempatta.RowCount = 1 Then
            pbatta.Image = Nothing
        Else
            With dgvempatta.CurrentRow
                If .IsNewRow Then
                    pbatta.Image = Nothing
                Else
                    If .Cells("eatt_AttachedFile").Value Is Nothing Then
                        pbatta.Image = Nothing
                    Else
                        pbatta.Image = ConvByteToImage(.Cells("eatt_AttachedFile").Value)

                        Dim eattachment_type = .Cells("eatt_Type").Value

                    End If
                End If
            End With
        End If
    End Sub

    Sub VIEW_employeeattachments(ByVal EmpID As Object)
        Dim params(1, 2) As Object

        params(0, 0) = "EmpID"

        params(0, 1) = EmpID

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_employeeattachments",
                             dgvempatta)

    End Sub

    Function INSUPD_employeeattachment(Optional eatta_RowID As Object = Nothing,
                                       Optional eatta_EmployeeID As Object = Nothing,
                                       Optional eatta_Type As Object = Nothing,
                                       Optional eatta_FileName As Object = Nothing,
                                       Optional eatta_FileType As Object = Nothing,
                                       Optional eatta_AttachedFile As Object = Nothing) As Object

        Dim params(7, 2) As Object

        'eatt_RowID
        'eatt_Type
        'eatt_FileName
        'eatt_FileType
        'eatt_EmployeeID
        'eatt_Created
        'eatt_CreatedBy
        'eatt_LastUpd
        'eatt_LastUpdBy
        'eatt_AttachedFile
        'eatt_viewthis

        params(0, 0) = "eatta_RowID"
        params(1, 0) = "eatta_EmployeeID"
        params(2, 0) = "eatta_CreatedBy"
        params(3, 0) = "eatta_LastUpdBy"
        params(4, 0) = "eatta_Type"
        params(5, 0) = "eatta_FileName"
        params(6, 0) = "eatta_FileType"
        params(7, 0) = "eatta_AttachedFile"

        params(0, 1) = If(eatta_RowID = Nothing, DBNull.Value, eatta_RowID)
        params(1, 1) = eatta_EmployeeID
        params(2, 1) = z_User
        params(3, 1) = z_User
        params(4, 1) = eatta_Type
        params(5, 1) = Trim(eatta_FileName)
        params(6, 1) = Trim(eatta_FileType)
        params(7, 1) = If(eatta_AttachedFile Is Nothing, DBNull.Value, eatta_AttachedFile)

        INSUPD_employeeattachment =
            EXEC_INSUPD_PROCEDURE(params,
                                   "INSUPD_employeeattachment",
                                   "empattaID")

    End Function

    Private Sub btnattaclear_Click(sender As Object, e As EventArgs) Handles btnattaclear.Click
        dgvempatta.Focus()

        pbatta.Image = Nothing

        With dgvempatta.CurrentRow
            .Cells("eatt_AttachedFile").Value = Nothing
            .Cells("eatt_FileName").Value = Nothing
            .Cells("eatt_FileType").Value = Nothing
            .Cells("eatt_viewthis").Value = Nothing

            listofEditRoweatt.Add(dgvempatta.Item("eatt_RowID", .Index).Value)

        End With

    End Sub

    Public listofEditRoweatt As New AutoCompleteStringCollection

    Dim atta_nameatt As String = Nothing
    Dim atta_extensnatt As String = Nothing
    Dim thefilepathatt As String = Nothing

    Private Sub btnattabrowse_Click(sender As Object, e As EventArgs) Handles btnattabrowse.Click
        dgvempatta.Focus()
        'RemoveHandler dgvEmpOT.SelectionChanged, AddressOf dgvEmpOT_SelectionChanged
        atta_nameatt = Nothing
        atta_extensnatt = Nothing

        Static employeeEmpOTRowID As Integer = -1
        Try
            Dim browsefile As OpenFileDialog = New OpenFileDialog()

            'browsefile.Filter = "JPEG(*.jpg)|*.jpg"

            browsefile.Filter = "All files (*.*)|*.*" &
                                "|JPEG (*.jpg)|*.jpg" &
                                "|PNG (*.PNG)|*.png" &
                                "|MS Word 97-2003 Document (*.doc)|*.doc" &
                                "|MS Word Document (*.docx)|*.docx" &
                                "|MS Excel 97-2003 Workbook (*.xls)|*.xls" &
                                "|MS Excel Workbook (*.xlsx)|*.xlsx"

            '|" & _
            '"PNG(*.PNG)|*.png|" & _
            '"Bitmap(*.BMP)|*.bmp"
            If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then
                With dgvempatta
                    '.ClearSelection()
                    .Focus()
                    'If .RowCount = 1 Then
                    '    .Rows.Add()
                    '    .Item("Column9", 0).Value = convertFileToByte(browsefile.FileName)
                    '    '.Item(.CurrentCell.ColumnIndex, 0).Selected = True
                    'Else

                    thefilepathatt = browsefile.FileName
                    atta_nameatt = Path.GetFileNameWithoutExtension(thefilepathatt)
                    atta_extensnatt = Path.GetExtension(thefilepathatt)

                    If .CurrentRow.IsNewRow Then
                        Dim e_rowindx As Integer = .CurrentRow.Index
                        Dim currcol As String = .Columns(.CurrentCell.ColumnIndex).Name
                        .Rows.Add()
                        .Item("eatt_AttachedFile", e_rowindx).Value = Nothing
                        .Item("eatt_AttachedFile", e_rowindx).Value = convertFileToByte(thefilepathatt)

                        .Item("eatt_FileName", e_rowindx).Value = atta_nameatt
                        .Item("eatt_FileType", e_rowindx).Value = atta_extensnatt
                        .Item("eatt_viewthis", e_rowindx).Value = "view this"

                        .CurrentRow.Cells("Column38").Value = .CurrentRow.Cells("eatt_Type").Value

                        .Item(currcol, e_rowindx).Selected = True
                    Else
                        .CurrentRow.Cells("eatt_AttachedFile").Value = Nothing
                        .CurrentRow.Cells("eatt_AttachedFile").Value = convertFileToByte(thefilepathatt)

                        .CurrentRow.Cells("eatt_FileName").Value = atta_nameatt
                        .CurrentRow.Cells("eatt_FileType").Value = atta_extensnatt
                        .CurrentRow.Cells("eatt_viewthis").Value = "view this"

                        .CurrentRow.Cells("Column38").Value = .CurrentRow.Cells("eatt_Type").Value

                        If employeeEmpOTRowID <> Val(dgvempatta.Item("eatt_RowID", .CurrentRow.Index).Value) Then
                            listofEditRoweatt.Add(dgvempatta.Item("eatt_RowID", .CurrentRow.Index).Value)
                        End If

                    End If
                    .Focus()
                End With
            Else

            End If
        Catch ex As Exception
            MsgBox(ex.Message & " Error on File")
        Finally
            dgvempatta_SelectionChanged(sender, e)
            'AddHandler dgvEmpOT.SelectionChanged, AddressOf dgvEmpOT_SelectionChanged
        End Try
    End Sub

    Private Sub btnattadl_Click(sender As Object, e As EventArgs) Handles btnattadl.Click
        dgvempatta.Focus()

        If dgvempatta.CurrentRow.Cells("eatt_AttachedFile").Value Is Nothing Then
            MsgBox("Nothing to view.", MsgBoxStyle.Information)
        Else

            Dim dlImage As SaveFileDialog = New SaveFileDialog
            dlImage.RestoreDirectory = True

            'dlImage.Filter = "JPEG(*.jpg)|*.jpg"

            'dlImage.Filter = "All files (*.*)|*.*" & _
            '                 "|JPEG (*.jpg)|*.jpg" & _
            '                 "|PNG (*.PNG)|*.png" & _
            '                 "|MS Word 97-2003 Document (*.doc)|*.doc" & _
            '                 "|MS Word Document (*.docx)|*.docx" & _
            '                 "|MS Excel 97-2003 Workbook (*.xls)|*.xls" & _
            '                 "|MS Excel Workbook (*.xlsx)|*.xlsx"

            If dlImage.ShowDialog = Windows.Forms.DialogResult.OK Then

                'dlImage.FileName = dgvOBF.CurrentRow.Cells("obf_attafilename").Value & _
                '                   dgvOBF.CurrentRow.Cells("obf_attafileextensn").Value

                Dim savefilepath As String =
                    Path.GetFullPath(dlImage.FileName) &
                    dgvempatta.CurrentRow.Cells("eatt_FileType").Value

                Dim fs As New FileStream(savefilepath, FileMode.Create)
                Dim blob As Byte() = DirectCast(dgvempatta.CurrentRow.Cells("eatt_AttachedFile").Value, Byte())
                fs.Write(blob, 0, blob.Length)
                fs.Close()
                fs = Nothing

                Dim prompt = MessageBox.Show("Do you want to open the saved file ?", "Show file", MessageBoxButtons.YesNoCancel)

                If prompt = Windows.Forms.DialogResult.Yes Then
                    Process.Start(savefilepath)
                End If

            End If

        End If

    End Sub

    Private Sub tsbtnNewAtta_Click(sender As Object, e As EventArgs) Handles tsbtnNewAtta.Click
        dgvempatta.EndEdit(1)

        For Each dgvrow As DataGridViewRow In dgvempatta.Rows
            If dgvrow.IsNewRow Then
                dgvrow.Cells("eatt_Type").Selected = 1
                dgvempatta.Focus()
                Exit For
            End If
        Next

        dgvempatta_SelectionChanged(sender, e)

    End Sub

    Dim dontUpdateAtta As SByte = 0

    Private Sub tsbtnSaveAtta_Click(sender As Object, e As EventArgs) Handles tsbtnSaveAtta.Click

        dgvempatta.EndEdit(1)

        If dontUpdateAtta = 1 Then
            listofEditRoweatt.Clear()
        End If

        If dgvEmp.RowCount = 0 Then
            Exit Sub
        End If

        For Each dgvrow As DataGridViewRow In dgvempatta.Rows
            With dgvrow

                'eatt_RowID
                'eatt_Type
                'eatt_FileName
                'eatt_FileType
                'eatt_EmployeeID
                'eatt_Created
                'eatt_CreatedBy
                'eatt_LastUpd
                'eatt_LastUpdBy
                'eatt_AttachedFile
                'eatt_viewthis

                If .IsNewRow Then
                Else
                    'MsgBox(("LambertBGalit").Substring(0, If(("LambertBGalit").ToString.Length >= 200, 200, ("LambertBGalit").ToString.Length)))

                    Dim filenameLength = If(Trim(.Cells("eatt_FileName").Value).Length >= 200, 200, Trim(.Cells("eatt_FileName").Value).Length)
                    Dim fileextnsnLength = If(Trim(.Cells("eatt_FileType").Value).Length >= 200, 200, Trim(.Cells("eatt_FileType").Value).Length)

                    If .Cells("eatt_RowID").Value = Nothing And
                        tsbtnNewAtta.Visible = True Then

                        If .Cells("Column38").Value = Nothing Then

                            WarnBalloon("The file '" & .Cells("eatt_FileName").Value & .Cells("eatt_FileType").Value & "' has no Attachment type." & vbNewLine &
                                        "Please supply it's attachment type.",
                                        "An attachment has no Type", cboattatype, 0, -69)

                            Exit Sub
                        Else
                            .Cells("eatt_RowID").Value = INSUPD_employeeattachment(,
                                                                               dgvEmp.CurrentRow.Cells("RowID").Value,
                                                                               .Cells("Column38").Value,
                                                                               Trim(.Cells("eatt_FileName").Value).Substring(0, filenameLength),
                                                                               Trim(.Cells("eatt_FileType").Value).Substring(0, fileextnsnLength),
                                                                               .Cells("eatt_AttachedFile").Value)

                        End If
                    Else
                        If listofEditRoweatt.Contains(.Cells("eatt_RowID").Value) Then

                            INSUPD_employeeattachment(.Cells("eatt_RowID").Value,
                                                      dgvEmp.CurrentRow.Cells("RowID").Value,
                                                      .Cells("Column38").Value,
                                                      Trim(.Cells("eatt_FileName").Value).Substring(0, filenameLength),
                                                      Trim(.Cells("eatt_FileType").Value).Substring(0, fileextnsnLength),
                                                      .Cells("eatt_AttachedFile").Value)

                        End If

                    End If

                End If

            End With

        Next

        listofEditRoweatt.Clear()

        InfoBalloon("Changes made in Employee ID '" & dgvEmp.CurrentRow.Cells("Column1").Value & "' has successfully saved.", "Changes successfully save", lblforballoon, 0, -69)

    End Sub

    Private Sub tsbtnCancelAtta_Click(sender As Object, e As EventArgs) Handles tsbtnCancelAtta.Click
        listofEditRoweatt.Clear()
        dgvEmp_SelectionChanged(sender, e)
    End Sub

#End Region

    Sub tabpageText(ByVal tbpIndex As Integer)
        Static once As SByte = 0

        If once = 0 Then
            once = 1
            Exit Sub
        End If

        Select Case tbpIndex
            Case 0
                tbpempchklist.Text = "CHECKLIST"
            Case 1
                tbpEmployee.Text = "PERSON"
            Case 2
                tbpSalary.Text = "SALARY"
            Case 3
                tbpAwards.Text = "AWARD"
            Case 4
                tbpCertifications.Text = "CERTI"
            Case 5
                tbpLeave.Text = "LEAVE"
            Case 6
                tbpDiscipAct.Text = "DISCIP"
            Case 7
                tbpEducBG.Text = "EDUC"
            Case 8
                tbpPrevEmp.Text = "PREV EMP"
            Case 9
                tbpPromotion.Text = "PROMOT"
            Case 10
                tbpLoans.Text = "LOAN SCH"
            Case 11
                tbpLoanHist.Text = "LOAN HIST"
            Case 12
                tbpPayslip.Text = "PAYSLIP"
            Case 13
                tbpempallow.Text = "ALLOW"
            Case 14
                tbpEmpOT.Text = "EMP OT"
            Case 15
                tbpOBF.Text = "OFFBUSI"
            Case 16
                tbpBonus.Text = "BONUS"
            Case 17
                tbpAttachment.Text = "ATTACH"
            Case Else

        End Select

    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click

        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Dim view_IDDependents As String = Nothing

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        Static once As SByte = 0
        If once = 0 Then
            once = 1

            view_IDDependents = VIEW_privilege("Employee Dependents", orgztnID)

        End If

        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_IDDependents)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub ToolStripButton31_Click(sender As Object, e As EventArgs) Handles ToolStripButton31.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_IDSal)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub ToolStripButton14_Click(sender As Object, e As EventArgs) Handles ToolStripButton14.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_IDAwar)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub ToolStripButton15_Click(sender As Object, e As EventArgs) Handles ToolStripButton15.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_IDCert)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub ToolStripButton17_Click(sender As Object, e As EventArgs) Handles ToolStripButton17.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDLeave)
        showAuditTrail.BringToFront()

    End Sub

    Private Sub ToolStripButton20_Click(sender As Object, e As EventArgs)
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDMed)
        showAuditTrail.BringToFront()

    End Sub

    Private Sub btnAudittrail_Click(sender As Object, e As EventArgs) Handles btnAudittrail.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDDiscip)
        showAuditTrail.BringToFront()

    End Sub

    Private Sub tsAudittrail_Click(sender As Object, e As EventArgs) Handles tsAudittrail.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDEduc)
        showAuditTrail.BringToFront()

    End Sub

    Private Sub ToolStripButton21_Click(sender As Object, e As EventArgs) Handles ToolStripButton21.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDPrevEmp)
        showAuditTrail.BringToFront()

    End Sub

    Private Sub ToolStripButton19_Click(sender As Object, e As EventArgs) Handles ToolStripButton19.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDPromot)
        showAuditTrail.BringToFront()

    End Sub

    Private Sub ToolStripButton25_Click(sender As Object, e As EventArgs) Handles ToolStripButton25.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDLoan)
        showAuditTrail.BringToFront()

    End Sub

    Private Sub ToolStripButton33_Click(sender As Object, e As EventArgs) Handles ToolStripButton33.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDHisto)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub ToolStripButton26_Click(sender As Object, e As EventArgs) Handles ToolStripButton26.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDAllow)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub ToolStripButton27_Click(sender As Object, e As EventArgs) Handles ToolStripButton27.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDEmpOT)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub ToolStripButton28_Click(sender As Object, e As EventArgs) Handles ToolStripButton28.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDOBF)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub ToolStripButton29_Click(sender As Object, e As EventArgs) Handles ToolStripButton29.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDBon)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub ToolStripButton34_Click(sender As Object, e As EventArgs) Handles ToolStripButton34.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_IDAttach)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub dgvempawar_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempawar.CellContentClick
    End Sub

    Private Sub dgvempawar_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvempawar.EditingControlShowing
        e.Control.ContextMenu = New ContextMenu
    End Sub

    Private Sub dgvempcert_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvempcert.EditingControlShowing
        e.Control.ContextMenu = New ContextMenu
    End Sub

    Private Sub dgvmedrec_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs)
        e.Control.ContextMenu = New ContextMenu
    End Sub

    Private Sub dgvempatta_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvempatta.EditingControlShowing
        e.Control.ContextMenu = New ContextMenu
    End Sub

    Dim filepath As String = Nothing

    Private Sub EmployeeImporter_Click(sender As Object, e As EventArgs) Handles tsbtnImportEmployee.Click,
                                                                                 tsbtnImportDependents.Click,
                                                                                 tsbtnImportSalary.Click

        tsbtnImportEmployee.Enabled = False

        tsbtnImportDependents.Enabled = False

        tsbtnImportSalary.Enabled = False

        Dim browsefile As OpenFileDialog = New OpenFileDialog()

        browsefile.Filter = str_ms_excel_file_extensn

        If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

            filepath = IO.Path.GetFullPath(browsefile.FileName)

            tabctrlemp.Enabled = False

            MDIPrimaryForm.Showmainbutton.Enabled = False

            Panel7.Enabled = False

            sender_Name = sender.Name

            If sender_Name = "tsbtnImportEmployee" Then

                tsprogbarempimport.Value = 0

                tsprogbarempimport.Visible = True

            ElseIf sender_Name = "tsbtnImportDependents" Then

                ToolStripProgressBar1.Value = 0

                ToolStripProgressBar1.Visible = True

            ElseIf sender_Name = "tsbtnImportSalary" Then

                ToolStripProgressBar2.Value = 0

                ToolStripProgressBar2.Visible = True

            End If

            bgworkImporting.RunWorkerAsync()

        End If

        tsbtnImportEmployee.Enabled = True

        tsbtnImportDependents.Enabled = True

        tsbtnImportSalary.Enabled = True

    End Sub

    Dim sender_Name = ""

    Private Sub bgworkImporting_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkImporting.DoWork

        RemoveHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        Dim catchDT =
        getWorkBookAsDataSet(filepath,
                             Me.Name)

        If catchDT Is Nothing Then
        Else

            Dim tbltoUse As String = Nothing

            If sender_Name = "tsbtnImportEmployee" Then
                tbltoUse = "Employees$"

                Dim tblpayfreq = New SQLQueryToDatatable("SELECT * FROM payfrequency;").ResultTable

                Dim dtEmpWorkSheet = catchDT.Tables("Employees$") 'Employees(2)

                If dtEmpWorkSheet Is Nothing Then
                Else

                    Dim emp_import_count = dtEmpWorkSheet.Rows.Count

                    Dim work_indx = 1

                    Dim emp_positName As String = ""

                    For Each drow As DataRow In dtEmpWorkSheet.Rows

                        Dim emp_positID = Nothing

                        If IsDBNull(drow(12)) Then
                            emp_positID = DBNull.Value
                        Else
                            Dim positName = CStr(drow(12)).Trim
                            If positName = Nothing Then
                                emp_positID = DBNull.Value
                            Else
                                emp_positName = positName
                                emp_positID = DBNull.Value

                            End If

                        End If

                        Dim payfreq_rowID = tblpayfreq.Select("PayFrequencyType='" & drow(18).ToString & "'")

                        Dim payfreqrow_id = Nothing

                        For Each prow In payfreq_rowID
                            payfreqrow_id = prow("RowID")
                        Next

                        INSUPD_employee(DBNull.Value,
                                        z_User,
                                        orgztnID,
                                        drow(9),
                                        drow(2),
                                        drow(3),
                                        drow(1),
                                        drow(4),
                                        drow(0),
                                        drow(13),
                                        drow(14),
                                        drow(16),
                                        drow(15),
                                        drow(20),
                                        String.Empty,
                                        String.Empty,
                                        String.Empty,
                                        drow(11),
                                        drow(10),
                                        drow(7),
                                        String.Empty,
                                        drow(6),
                                        drow(19),
                                        drow(8),
                                        drow(5),
                                        drow(17),
                                        DBNull.Value,
                                        emp_positID,
                                        If(payfreqrow_id = Nothing, DBNull.Value, payfreqrow_id),
                                        0,
                                        "1",
                                        "1",
                                        String.Empty,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        DBNull.Value,
                                        0,
                                        0,
                                        0,
                                        "0",
                                        If(ValNoComma(drow(23)) > 0, drow(23), 313),
                                        "1",
                                        drow(21),
                                        0,
                                        drow(22),
                                        "1", "1", "1", "1", "1", "1",
                                        emp_positName)

                        Dim progressvalue = CInt((work_indx / emp_import_count) * 100)

                        bgworkImporting.ReportProgress(progressvalue)

                        work_indx += 1

                    Next

                End If

            ElseIf sender_Name = "tsbtnImportDependents" Then
                tbltoUse = "'Employee Dependents$'"

                Dim dtEmpDepenWorkSheet = catchDT.Tables("'Employee Dependents$'") 'Employee Depedents(0)

                If dtEmpDepenWorkSheet IsNot Nothing Then

                    Dim emp_import_count = dtEmpDepenWorkSheet.Rows.Count

                    Dim work_indx = 1

                    For Each drow As DataRow In dtEmpDepenWorkSheet.Rows

                        INSUPD_employeedependents(,
                                                  drow(10),
                                                  drow(3),
                                                  drow(4),
                                                  drow(2),
                                                  drow(5),
                                                  drow(0),
                                                  drow(13),
                                                  drow(14),
                                                  drow(16),
                                                  drow(15), , ,
                                                  drow(12),
                                                  drow(12),
                                                  drow(11),
                                                  drow(8), ,
                                                  drow(7),
                                                  drow(1),
                                                  "Y",
                                                  drow(6),
                                                  "1")

                        Dim progressvalue = CInt((work_indx / emp_import_count) * 100)

                        bgworkImporting.ReportProgress(progressvalue)

                        work_indx += 1

                    Next

                    EXECQUER("DELETE FROM listofval WHERE `Type`='EmployeeDependent' AND Active='No';")

                End If

            ElseIf sender_Name = "tsbtnImportSalary" Then
                tbltoUse = "'Employee Salary$'"

                Dim dtEmpSalWorkSheet = catchDT.Tables("'Employee Salary$'") 'Employee Salary(1)

                If dtEmpSalWorkSheet IsNot Nothing Then

                    Dim emp_import_count = dtEmpSalWorkSheet.Rows.Count

                    Dim work_indx = 1

                    Dim emp_RowID = Nothing

                    Dim sheetEmpRowID = Nothing

                    Dim catchEmpRow = Nothing

                    Dim EmpRowID = Nothing

                    Dim EmpNumDepen = Nothing

                    Dim EmpMaritStat = Nothing

                    Dim EmpPositID = Nothing

                    Dim PAYFREQUENCY_DIVISOR = ValNoComma(0)

                    For Each drow As DataRow In dtEmpSalWorkSheet.Rows

                        sheetEmpRowID = If(IsDBNull(drow(0)), Nothing, drow(0))

                        If emp_RowID <> sheetEmpRowID Then

                            emp_RowID = sheetEmpRowID

                            Dim dtrowrecord As New DataTable

                            dtrowrecord =
                                New SQLQueryToDatatable("SELECT " &
                                                        "e.RowID" &
                                                        ",e.MaritalStatus" &
                                                        ",e.PositionID" &
                                                        ",IF(e.EmployeeType='Monthly', PAYFREQUENCY_DIVISOR(pf.PayFrequencyType), PAYFREQUENCY_DIVISOR(e.EmployeeType)) AS PAYFREQUENCY_DIVISOR" &
                                                        " FROM employee e" &
                                                        " INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID" &
                                                        " WHERE e.EmployeeID='" & emp_RowID & "'" &
                                                        " AND e.OrganizationID='" & orgztnID & "';").ResultTable
                            Dim new_blankarr() As Object ' = New Object()
                            ReDim new_blankarr(0)
                            catchEmpRow = new_blankarr

                            For Each erow As DataRow In dtrowrecord.Rows
                                catchEmpRow = erow.ItemArray()
                            Next

                            'catchEmpRow = Split(catchEmpRow, ",")

                            If catchEmpRow.GetUpperBound(0) = 0 Then

                                Continue For
                            Else

                                EmpRowID = catchEmpRow(0)

                                EmpNumDepen = EXECQUER("SELECT COUNT(RowID) FROM employeedependents WHERE ParentEmployeeID='" & EmpRowID & "' AND OrganizationID=" & orgztnID & " AND ActiveFlag='Y';")

                                EmpMaritStat = catchEmpRow(1)

                                EmpPositID = catchEmpRow(2)

                                PAYFREQUENCY_DIVISOR = ValNoComma(catchEmpRow(3))

                            End If
                        Else

                            Continue For

                        End If

                        Dim declared_Sal = ValNoComma(drow(1))

                        Dim undeclared_Sal = ValNoComma(drow(2))

                        Dim EffDateFrom = If(IsDBNull(drow(3)),
                                             "1900-01-01",
                                             MYSQLDateFormat(CDate(drow(3))))

                        Dim EffDateTo = If(IsDBNull(drow(4)),
                                           Nothing,
                                            If(drow(4) Is Nothing,
                                               Nothing,
                                               MYSQLDateFormat(CDate(drow(4)))))

                        INSUPD_employeesalary(,
                                                EmpRowID,
                                                declared_Sal / PAYFREQUENCY_DIVISOR,
                                                declared_Sal,
                                                EmpNumDepen,
                                                EmpMaritStat,
                                                EmpPositID,
                                                EffDateFrom,
                                                EffDateTo,
                                                declared_Sal + (undeclared_Sal),
                                                "1")

                        Dim progressvalue = CInt((work_indx / emp_import_count) * 100)

                        bgworkImporting.ReportProgress(progressvalue)

                        work_indx += 1

                    Next

                End If

            End If

        End If

        catchDT.Dispose()

    End Sub

    Private Sub bgworkImporting_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkImporting.ProgressChanged

        If sender_Name = "tsbtnImportEmployee" Then

            tsprogbarempimport.Value = CType(e.ProgressPercentage, Integer)

        ElseIf sender_Name = "tsbtnImportDependents" Then

            ToolStripProgressBar1.Value = CType(e.ProgressPercentage, Integer)

        ElseIf sender_Name = "tsbtnImportSalary" Then

            ToolStripProgressBar2.Value = CType(e.ProgressPercentage, Integer)

        End If

    End Sub

    Private Sub bgworkImporting_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkImporting.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox(getErrExcptn(e.Error, Me.Name))
        ElseIf e.Cancelled Then
            MessageBox.Show("Background work cancelled.")
        Else
            MsgBox("Done importing", MsgBoxStyle.Information)

            loademployee()

        End If

        tsprogbarempimport.Visible = False

        ToolStripProgressBar1.Visible = False

        ToolStripProgressBar2.Visible = False

        tabctrlemp.Enabled = True

        MDIPrimaryForm.Showmainbutton.Enabled = True

        Panel7.Enabled = True

        sameEmpID = -1

        AddHandler dgvEmp.SelectionChanged, AddressOf dgvEmp_SelectionChanged

        dgvEmp_SelectionChanged(sender, New EventArgs)

    End Sub

    Function INSUPD_employee(ParamArray paramSetValue() As Object) As Object

        Dim str_quer As String =
            String.Concat("SELECT INSUPD_employee_01(",
                          "?RID",
                          ",?UserRowID",
                          ",?OrganizID",
                          ",?Salutat",
                          ",?FName",
                          ",?MName",
                          ",?LName",
                          ",?Surname",
                          ",?EmpID",
                          ",?TIN",
                          ",?SSS",
                          ",?HDMF",
                          ",?PhH",
                          ",?EmpStatus",
                          ",?EmailAdd",
                          ",?WorkNo",
                          ",?HomeNo",
                          ",?MobileNo",
                          ",?HAddress",
                          ",?Nick",
                          ",?JTitle",
                          ",?Gend",
                          ",?EmpType",
                          ",?MaritStat",
                          ",?BDate",
                          ",?Start_Date",
                          ",?TerminatDate",
                          ",?PositID",
                          ",?PayFreqID",
                          ",?NumDependent",
                          ",?UTOverride",
                          ",?OTOverride",
                          ",?NewEmpFlag",
                          ",?LeaveBal",
                          ",?SickBal",
                          ",?MaternBal",
                          ",?LeaveAllow",
                          ",?SickAllow",
                          ",?MaternAllow",
                          ",?Imag",
                          ",?LeavePayPer",
                          ",?SickPayPer",
                          ",?MaternPayPer",
                          ",?IsExemptAlphaList",
                          ",?Work_DaysPerYear",
                          ",?Day_Rest",
                          ",?ATM_No",
                          ",?OtherLeavePayPer",
                          ",?Bank_Name",
                          ",?Calc_Holiday",
                          ",?Calc_SpecialHoliday",
                          ",?Calc_NightDiff",
                          ",?Calc_NightDiffOT",
                          ",?Calc_RestDay",
                          ",?Calc_RestDayOT",
                          ",?PositionTextName",
                          ");")

        Dim sql As New SQL(str_quer, paramSetValue)

        Dim returnvalue As Object = Nothing

        sql.ExecuteQuery()

        Return returnvalue

    End Function

    Private Sub Label25_Click(sender As Object, e As EventArgs) Handles Label25.Click

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub dgvEmp_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmp.CellContentClick

    End Sub

    Private Sub tbpLoans_Click(sender As Object, e As EventArgs) Handles tbpLoans.Click

    End Sub

    Private Sub cboDayOfRest_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDayOfRest.SelectedIndexChanged

    End Sub

    Private Sub txtloaninterest_TextChanged(sender As Object, e As EventArgs) Handles txtloaninterest.TextChanged

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Function INSUPDemployee(ParamArray paramSetValue() As Object) As Object

        'RowID,CreatedBy,Created,LastUpdBy,LastUpd,OrganizationID,Salutation,FirstName,MiddleName,LastName,Surname,EmployeeID,TINNo,SSSNo,HDMFNo,PhilHealthNo,EmploymentStatus,EmailAddress,WorkPhone,HomePhone,MobilePhone,HomeAddress,Nickname,JobTitle,Gender,EmployeeType,MaritalStatus,Birthdate,StartDate,TerminationDate,PositionID,PayFrequencyID,NoOfDependents,UndertimeOverride,OvertimeOverride,NewEmployeeFlag,LeaveBalance,SickLeaveBalance,MaternityLeaveBalance,OtherLeaveBalance,LeaveAllowance,SickLeaveAllowance,MaternityLeaveAllowance,OtherLeaveAllowance,Image,LeavePerPayPeriod,SickLeavePerPayPeriod,MaternityLeavePerPayPeriod,OtherLeavePerPayPeriod,AlphaListExempted,WorkDaysPerYear,DayOfRest,ATMNo,BankName,CalcHoliday,CalcSpecialHoliday,CalcNightDiff,CalcNightDiffOT,CalcRestDay,CalcRestDayOT,DateRegularized,DateEvaluated,RevealInPayroll,LateGracePeriod,AgencyID,OffsetBalance

        'emplo_RowID,emplo_UserID,emplo_OrganizationID,emplo_Salutation,emplo_FirstName,emplo_MiddleName,emplo_LastName,emplo_Surname,emplo_EmployeeID,emplo_TINNo,emplo_SSSNo,emplo_HDMFNo,emplo_PhilHealthNo,emplo_EmploymentStatus,emplo_EmailAddress,emplo_WorkPhone,emplo_HomePhone,emplo_MobilePhone,emplo_HomeAddress,emplo_Nickname,emplo_JobTitle,emplo_Gender,emplo_EmployeeType,emplo_MaritalStatus,emplo_Birthdate,emplo_Startdate,emplo_TerminationDate,emplo_PositionID,emplo_PayFrequencyID,emplo_NoOfDependents,emplo_Image,emplo_LeavePerPayPeriod,emplo_SickLeavePerPayPeriod,emplo_MaternityLeavePerPayPeriod,emplo_OtherLeavePerPayPeriod,emplo_UndertimeOverride,emplo_OvertimeOverride,emplo_LeaveBalance,emplo_SickLeaveBalance,emplo_MaternityLeaveBalance,emplo_OtherLeaveBalance,emplo_LeaveAllowance,emplo_SickLeaveAllowance,emplo_MaternityLeaveAllowance,emplo_OtherLeaveAllowance,emplo_AlphaListExempted,emplo_WorkDaysPerYear,emplo_DayOfRest,emplo_ATMNo,emplo_BankName,emplo_CalcHoliday,emplo_CalcSpecialHoliday,emplo_CalcNightDiff
        'emplo_CalcNightDiffOT,emplo_CalcRestDay,emplo_CalcRestDayOT,emplo_DateRegularized
        'emplo_DateEvaluated,emplo_RevealInPayroll,emplo_LateGracePeriod,emplo_AgencyID,emplo_OffsetBalance

        Dim n_ReadSQLFunction As New ReadSQLFunction("INSUPD_employee", "returnval", paramSetValue)

        Return n_ReadSQLFunction.ReturnValue

    End Function

    Function INSUPD_employeedependents(Optional emp_RowID = Nothing,
                                       Optional emp_Salutation = Nothing,
                                       Optional emp_FirstName = Nothing,
                                       Optional emp_MiddleName = Nothing,
                                       Optional emp_LastName = Nothing,
                                       Optional emp_SurName = Nothing,
                                       Optional emp_ParentEmployeeID = Nothing,
                                       Optional emp_TINNo = Nothing,
                                       Optional emp_SSSNo = Nothing,
                                       Optional emp_HDMFNo = Nothing,
                                       Optional emp_PhilHealthNo = Nothing,
                                       Optional emp_EmailAddress = Nothing,
                                       Optional emp_WorkPhone = Nothing,
                                       Optional emp_HomePhone = Nothing,
                                       Optional emp_MobilePhone = Nothing,
                                       Optional emp_HomeAddress = Nothing,
                                       Optional emp_Nickname = Nothing,
                                       Optional emp_JobTitle = Nothing,
                                       Optional emp_Gender = Nothing,
                                       Optional emp_RelationToEmployee = Nothing,
                                       Optional emp_ActiveFlag = Nothing,
                                       Optional emp_Birthdate = Nothing,
                                       Optional emp_IsDoneByImporting = "0")

        Dim params(26, 2) As Object

        params(0, 0) = "emp_RowID"
        params(1, 0) = "emp_CreatedBy"
        params(2, 0) = "emp_LastUpdBy"
        params(3, 0) = "emp_LastUpd"
        params(4, 0) = "emp_OrganizationID"
        params(5, 0) = "emp_Salutation"
        params(6, 0) = "emp_FirstName"
        params(7, 0) = "emp_MiddleName"
        params(8, 0) = "emp_LastName"
        params(9, 0) = "emp_SurName"
        params(10, 0) = "emp_ParentEmployeeID"
        params(11, 0) = "emp_TINNo"
        params(12, 0) = "emp_SSSNo"
        params(13, 0) = "emp_HDMFNo"
        params(14, 0) = "emp_PhilHealthNo"
        params(15, 0) = "emp_EmailAddress"
        params(16, 0) = "emp_WorkPhone"
        params(17, 0) = "emp_HomePhone"
        params(18, 0) = "emp_MobilePhone"
        params(19, 0) = "emp_HomeAddress"
        params(20, 0) = "emp_Nickname"
        params(21, 0) = "emp_JobTitle"
        params(22, 0) = "emp_Gender"
        params(23, 0) = "emp_RelationToEmployee"
        params(24, 0) = "emp_ActiveFlag"
        params(25, 0) = "emp_Birthdate"
        params(26, 0) = "emp_IsDoneByImporting"

        params(0, 1) = If(emp_RowID = Nothing, DBNull.Value, emp_RowID)
        params(1, 1) = z_User
        params(2, 1) = z_User
        params(3, 1) = 0
        params(4, 1) = orgztnID
        params(5, 1) = If(IsDBNull(emp_Salutation), "", Trim(emp_Salutation))
        params(6, 1) = If(IsDBNull(emp_FirstName), "", Trim(emp_FirstName))
        params(7, 1) = If(IsDBNull(emp_MiddleName), "", Trim(emp_MiddleName))
        params(8, 1) = If(IsDBNull(emp_LastName), "", Trim(emp_LastName))
        params(9, 1) = If(IsDBNull(emp_SurName), "", Trim(emp_SurName))

        If IsDBNull(emp_ParentEmployeeID) Then

            params(10, 1) = DBNull.Value
        Else

            Dim Emp_RID = EXECQUER("SELECT RowID FROM employee WHERE EmployeeID='" & emp_ParentEmployeeID & "' AND OrganizationID=" & orgztnID & " LIMIT 1;")

            Dim ParentEmpRID = emp_ParentEmployeeID

            If Emp_RID <> emp_ParentEmployeeID Then
                ParentEmpRID = Emp_RID
            End If

            params(10, 1) = ParentEmpRID

        End If

        params(11, 1) = If(IsDBNull(emp_TINNo), "   -   -   -", Trim(emp_TINNo))
        params(12, 1) = If(IsDBNull(emp_SSSNo), "  -       -", Trim(emp_SSSNo))
        params(13, 1) = If(IsDBNull(emp_HDMFNo), "    -    -", Trim(emp_HDMFNo))
        params(14, 1) = If(IsDBNull(emp_PhilHealthNo), "    -    -", Trim(emp_PhilHealthNo))
        params(15, 1) = If(IsDBNull(emp_EmailAddress), "", Trim(emp_EmailAddress))
        params(16, 1) = If(IsDBNull(emp_WorkPhone), "", Trim(emp_WorkPhone))
        params(17, 1) = If(IsDBNull(emp_HomePhone), "", Trim(emp_HomePhone))
        params(18, 1) = If(IsDBNull(emp_MobilePhone), "", Trim(emp_MobilePhone))
        params(19, 1) = If(IsDBNull(emp_HomeAddress), "", Trim(emp_HomeAddress))
        params(20, 1) = If(IsDBNull(emp_Nickname), "", Trim(emp_Nickname))
        params(21, 1) = If(IsDBNull(emp_JobTitle), "", Trim(emp_JobTitle))
        params(22, 1) = If(IsDBNull(emp_Gender), "M", Trim(emp_Gender))
        params(23, 1) = If(IsDBNull(emp_RelationToEmployee), "", Trim(emp_RelationToEmployee))
        params(24, 1) = If(IsDBNull(emp_ActiveFlag), "N", Trim(emp_ActiveFlag))
        params(25, 1) = If(IsDBNull(emp_Birthdate), DBNull.Value, Format(CDate(emp_Birthdate), "yyyy-MM-dd"))
        params(26, 1) = emp_IsDoneByImporting

        Dim ret_val =
        EXEC_INSUPD_PROCEDURE(params,
                              "INSUPD_employeedependents",
                              "empdepenID")

        Return ret_val

    End Function

    Private Sub tsbtnNewEmp_EnabledChanged(sender As Object, e As EventArgs) Handles tsbtnNewEmp.EnabledChanged

    End Sub

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

    Function INSUPD_position(Optional pos_RowID As Object = Nothing,
                             Optional pos_PositionName As Object = Nothing,
                             Optional pos_ParentPositionID As Object = Nothing,
                             Optional pos_DivisionId As Object = Nothing) As Object

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("INSUPD_position", conn)

            conn.Open()

            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("positID", MySqlDbType.Int32)

                .Parameters.AddWithValue("pos_RowID", If(pos_RowID = Nothing, DBNull.Value, pos_RowID))
                .Parameters.AddWithValue("pos_PositionName", Trim(pos_PositionName))
                .Parameters.AddWithValue("pos_CreatedBy", z_User)
                .Parameters.AddWithValue("pos_OrganizationID", orgztnID)
                .Parameters.AddWithValue("pos_LastUpdBy", z_User)
                .Parameters.AddWithValue("pos_ParentPositionID", If(pos_ParentPositionID = Nothing, DBNull.Value, pos_ParentPositionID))
                .Parameters.AddWithValue("pos_DivisionId", If(pos_DivisionId = Nothing, DBNull.Value, pos_DivisionId))

                .Parameters("positID").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

                INSUPD_position = datread(0)

            End With
        Catch ex As Exception
            INSUPD_position = Nothing
            MsgBox(ex.Message & " " & "INSUPD_position", , "Error")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try

        Return INSUPD_position

    End Function

    Private Sub txtWorkHoursPerWeek_KeyPress(sender As Object, e As KeyPressEventArgs) Handles _
                                                                                                txtWorkDaysPerYear.KeyPress
        'e.Handled = TrapNumKey(Asc(e.KeyChar))
        e.Handled = New TrapDecimalKey(Asc(e.KeyChar), txtWorkDaysPerYear.Text).ResultTrap

    End Sub

    Private Sub txtWorkHoursPerWeek_TextChanged(sender As Object, e As EventArgs)
    End Sub

    Private Sub tsbtnDeletLeave_Click(sender As Object, e As EventArgs) Handles tsbtnDeletLeave.Click

        dgvempleave.Focus()

        If Not dgvempleave.CurrentRow.IsNewRow Then

            Dim result = MessageBox.Show("Are you sure you want to delete leave ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

            If result = DialogResult.Yes Then

                dgvempleave.EndEdit(True)

                EXECQUER(String.Concat("CALL DEL_employeeleave('", ValNoComma(dgvempleave.CurrentRow.Cells("elv_RowID").Value), "');"))

                dgvempleave.Rows.Remove(dgvempleave.CurrentRow)

            End If

        End If

    End Sub

    Private Sub txtothrpayp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtothrpayp.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtothrpayp.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtothrpayp.Text.Contains(".") Then
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

    Private Sub rdbDirectDepo_CheckedChanged(sender As Object, e As EventArgs) Handles rdbDirectDepo.CheckedChanged

        Dim checkstate = rdbDirectDepo.Checked

        txtATM.Enabled = checkstate

        cbobank.Enabled = checkstate

        lnklblAddBank.Enabled = checkstate

        Label359.Visible = checkstate

        Label360.Visible = checkstate

        If cbobank.Enabled Then

            If dgvEmp.RowCount <> 0 Then

                cbobank.Text = dgvEmp.CurrentRow.Cells("BankName").Value

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

    Private Sub txtATM_TextChanged(sender As Object, e As EventArgs) Handles txtATM.TextChanged

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

    Private Sub cbobank_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbobank.SelectedIndexChanged
    End Sub

    Private Sub rdbCash_CheckedChanged(sender As Object, e As EventArgs) Handles rdbCash.CheckedChanged
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

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        RemoveHandler MaskedTextBox1.Leave, AddressOf MaskedTextBox1_Leave
        If CheckBox1.Checked Then
            AddHandler MaskedTextBox1.Leave, AddressOf MaskedTextBox1_Leave
            MaskedTextBox1.ReadOnly = False
            MaskedTextBox1.Focus()
        Else
            MaskedTextBox1.ReadOnly = True
        End If

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        RemoveHandler MaskedTextBox2.Leave, AddressOf MaskedTextBox2_Leave
        If CheckBox2.Checked Then
            AddHandler MaskedTextBox2.Leave, AddressOf MaskedTextBox2_Leave
            MaskedTextBox2.ReadOnly = False
            MaskedTextBox2.Focus()
        Else
            MaskedTextBox2.ReadOnly = True
        End If

    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked

        Dim n_DiscipAction As New DiscipAction

        'n_DiscipAction.call_asdialog = 1

        If n_DiscipAction.ShowDialog() = Windows.Forms.DialogResult.OK Then

            cboAction.Items.Clear()

            empdiscippenal = retAsDatTbl("SELECT * FROM listofval WHERE Type='Employee Disciplinary Penalty' AND Active='Yes' ORDER BY OrderBy;")

            For Each drow As DataRow In empdiscippenal.Rows
                cboAction.Items.Add(Trim(drow("DisplayValue").ToString))
            Next

        End If

    End Sub

    Private Sub cboPayFreq_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles cboPayFreq.SelectedIndexChanged

    End Sub

    Private Sub PageNumerOvertime(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkFirstOT.LinkClicked,
                                                                                                lnkPrevOT.LinkClicked,
                                                                                                lnkNxtOT.LinkClicked,
                                                                                                lnkLastOT.LinkClicked

        'pagenumberOTOT

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "lnkFirstOT" Then
            pagenumberOT = 0
        ElseIf sendrname = "lnkPrevOT" Then

            Dim modcent = pagenumberOT Mod 10

            If modcent = 0 Then

                pagenumberOT -= 10
            Else

                pagenumberOT -= modcent

            End If

            If pagenumberOT < 0 Then

                pagenumberOT = 0

            End If

        ElseIf sendrname = "lnkNxtOT" Then

            Dim modcent = pagenumberOT Mod 10

            If modcent = 0 Then
                pagenumberOT += 10
            Else
                pagenumberOT -= modcent

                pagenumberOT += 10

            End If
        ElseIf sendrname = "lnkLastOT" Then

            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 10 FROM employeeovertime WHERE OrganizationID='" & orgztnID & "' AND EmployeeID='" & publicEmpRowID & "';"))

            Dim remender = lastpage Mod 1

            pagenumberOT = (lastpage - remender) * 10

            If pagenumberOT - 10 < 10 Then
                'pagination = 0

            End If

        End If

        dgvEmp_SelectionChanged(sender, e)

    End Sub

    Private Sub Gender_CheckedChanged(sender As Object, e As EventArgs) Handles rdMale.CheckedChanged,
                                                                                rdFMale.CheckedChanged

        Dim obj_sender = DirectCast(sender, RadioButton)

        Dim label_gender = ""

        If obj_sender.Name = "rdMale" _
            And obj_sender.Checked Then

            label_gender = "Paternity"

        ElseIf obj_sender.Name = "rdFMale" _
            And obj_sender.Checked Then

            label_gender = "Maternity"

        End If

        Label148.Text = label_gender
        Label149.Text = label_gender
        Label152.Text = label_gender

        Label104.Text = label_gender
        Label105.Text = label_gender
        Label119.Text = label_gender

    End Sub

    Private Sub cboOBFstatus_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles cboOBFstatus.SelectedIndexChanged

    End Sub

    Private Sub tsbtnDelAllowance_Click(sender As Object, e As EventArgs) Handles tsbtnDelAllowance.Click

        Dim allowance_RowID = dgvempallowance.Tag

        If allowance_RowID = Nothing Then
        Else

            Dim result = MessageBox.Show("Are you sure you want to delete allowance ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

            If result = DialogResult.Yes Then

                Dim n_ExecuteQuery As _
                    New ExecuteQuery("CALL DEL_employeeallowance('" & dgvempallowance.Tag & "');")

                dgvempallowance.Rows.Remove(dgvempallowance.CurrentRow)

            End If

        End If

    End Sub

    Private Sub dgvempallowance_SelectionChanged1(sender As Object, e As EventArgs) Handles dgvempallowance.SelectionChanged

        dgvempallowance.Tag = Nothing

        Try

            Dim curr_row = dgvempallowance.CurrentRow

            If curr_row IsNot Nothing Then

                dgvempallowance.Tag = curr_row.Cells("eall_RowID").Value

            End If
        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))

        End Try

    End Sub

    Private Sub MaskedTextBox2_Leave(sender As Object, e As EventArgs) 'Handles MaskedTextBox2.Leave
        Try
            MaskedTextBox2.Text = CDate(MaskedTextBox2.Text).ToShortDateString
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try
    End Sub

    Private Sub MaskedTextBox2_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles MaskedTextBox2.MaskInputRejected

    End Sub

    Private Sub MaskedTextBox2_TextChanged(sender As Object, e As EventArgs) Handles MaskedTextBox2.TextChanged
        Try
            MaskedTextBox2.Tag = MYSQLDateFormat(CDate(MaskedTextBox2.Text))
        Catch ex As Exception
            MaskedTextBox2.Tag = String.Empty
        Finally
            If CheckBox2.Checked = False Then
                CheckBox2.Checked = (MaskedTextBox2.Tag.ToString.Length > 0)
            End If
        End Try
    End Sub

    Private Sub MaskedTextBox1_Leave(sender As Object, e As EventArgs) 'Handles MaskedTextBox1.Leave
        Try
            MaskedTextBox1.Text = CDate(MaskedTextBox1.Text).ToShortDateString
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

    End Sub

    Private Sub MaskedTextBox1_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles MaskedTextBox1.MaskInputRejected

    End Sub

    Private Sub MaskedTextBox1_TextChanged(sender As Object, e As EventArgs) Handles MaskedTextBox1.TextChanged
        Try
            MaskedTextBox1.Tag = MYSQLDateFormat(CDate(MaskedTextBox1.Text))
        Catch ex As Exception
            MaskedTextBox1.Tag = String.Empty
        Finally
            If CheckBox1.Checked = False Then
                CheckBox1.Checked = (MaskedTextBox1.Tag.ToString.Length > 0)
            End If
        End Try
    End Sub

    Private Sub chkboxChargeToBonus_CheckedChanged(sender As Object, e As EventArgs) Handles chkboxChargeToBonus.CheckedChanged

    End Sub

    Private ebonus_rowid_comment As New Dictionary(Of Object, String())

    Private Sub chkboxChargeToBonus_Click(sender As Object, e As EventArgs) Handles chkboxChargeToBonus.Click

        If dgvLoanList.RowCount > 0 _
            And tsbtnNewLoan.Enabled Then
            chkboxChargeToBonus.Checked = Convert.ToInt16(CBool(dgvLoanList.CurrentRow.Cells("LoanHasBonus").Value))
        End If

        Dim bool_result As Boolean = False

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT EmployeeID,BonusID,DedEffectiveDateFrom,DedEffectiveDateTo FROM employeeloanschedule WHERE RowID='" & If(tsbtnNewLoan.Enabled, dgvLoanList.Tag, 0) & "';")

        Dim catchdt As New DataTable

        catchdt = n_SQLQueryToDatatable.ResultTable

        Dim DedEffectiveDateFrom, DedEffectiveDateTo As Object

        DedEffectiveDateFrom = MYSQLDateFormat(CDate(datefrom.Value))
        DedEffectiveDateTo = MYSQLDateFormat(DedEffectiveDateFrom)

        Dim BonusRefID, EmpRow_ID As Integer

        For Each drow As DataRow In catchdt.Rows

            DedEffectiveDateFrom = MYSQLDateFormat(CDate(drow("DedEffectiveDateFrom")))
            DedEffectiveDateTo = MYSQLDateFormat(CDate(drow("DedEffectiveDateTo")))
            BonusRefID = ValNoComma(drow("BonusID"))
            EmpRow_ID = drow("EmployeeID")

        Next

        If tsbtnNewLoan.Enabled = False _
            And ValNoComma(txtnoofpayper.Text) > 0 _
            And cmbdedsched.Text.Length > 0 Then

            EmpRow_ID = dgvEmp.CurrentRow.Cells("RowID").Value

            DedEffectiveDateTo = New ExecuteQuery("SELECT PAYTODATE_OF_NoOfPayPeriod('" & DedEffectiveDateFrom & "'" &
                                                  ",'" & ValNoComma(txtnoofpayper.Text) & "','" & EmpRow_ID & "','" & cmbdedsched.Text & "') 'Result';").Result
            DedEffectiveDateTo = MYSQLDateFormat(CDate(DedEffectiveDateTo))
        End If

        Dim els_rowid As Object = Nothing

        Try
            els_rowid =
                dgvLoanList.CurrentRow.Cells("c_RowIDLoan").Value
        Catch ex As Exception
            els_rowid = Nothing
        End Try

        Dim n_EmployeeBonusControl As New EmployeeBonusControl(EmpRow_ID, BonusRefID, DedEffectiveDateFrom, DedEffectiveDateTo)

        With n_EmployeeBonusControl

            .EmployeeLoanRowID = els_rowid

            .ShowAsDialog = True

            .StartPosition = FormStartPosition.CenterScreen

            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                bool_result = True
                chkboxChargeToBonus.Checked = bool_result
                chkboxChargeToBonus.Tag = .EmployeeBonusRowID
                ebonus_rowid_comment = .BonusComments
            Else
                Dim this_bool_result As Boolean = tsbtnNewLoan.Enabled
                If this_bool_result = False Then
                    chkboxChargeToBonus.Checked = this_bool_result
                Else

                End If
            End If

        End With
    End Sub

    Private Sub cmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus.SelectedIndexChanged
    End Sub

    Private Sub cmbStatus_TextChanged(sender As Object, e As EventArgs) Handles cmbStatus.TextChanged

        cmbStatus.Enabled = Not (cmbStatus.Text = str_LoanCancelledStauts)

    End Sub

    Private Sub tabctrlemp_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles tabctrlemp.Selecting

        Dim is_tabpage_payslip As Boolean = (e.TabPage.Name = tbpPayslip.Name)

        Dim isOldSalaryTab = (e.TabPage.Name = tbpSalary.Name)

        If is_tabpage_payslip Then

            e.Cancel = is_tabpage_payslip

            MDIPrimaryForm.ToolStripButton5_Click(sender, New EventArgs)

            PayrollForm.PayrollToolStripMenuItem_Click(sender, New EventArgs)
        ElseIf isOldSalaryTab Then
            Dim index = 0
            For Each tabPage In tabctrlemp.TabPages
                If tabPage Is tbpNewSalary Then
                    Exit For
                End If
                index += 1
            Next

            tabctrlemp.SelectedIndex = index
        Else
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

        End If

    End Sub

End Class
