Imports System.Threading
Imports Femiani.Forms.UI.Input

Public Class LeaveForm

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        LeaveForm()

    End Sub

    Sub LeaveForm()

        dtpstarttime.CustomFormat = machineShortTimeFormat

        dtpendtime.CustomFormat = machineShortTimeFormat

        dtpstartdate.Tag = Format(CDate(dtpstartdate.Value), "yyyy-MM-dd")
        dtpendate.Tag = Format(CDate(dtpendate.Value), "yyyy-MM-dd")

    End Sub

    Dim n_LeaveRowID As String = String.Empty

    Public Property LeaveRowID As String

        Get

            Return n_LeaveRowID

        End Get

        Set(value As String)

            n_LeaveRowID = value

        End Set

    End Property

    Private Sub LeaveForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If Panel1.Enabled = True Then

            e.Cancel = False
        Else

            e.Cancel = True

        End If

    End Sub

    Private previous_orgID As String = String.Empty

    Private Sub LeaveForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'enlistToCboBox("SELECT DISTINCT(DisplayValue) FROM listofval WHERE Type='Leave Type' AND Active='Yes';", _
        '               cboleavetypes)

        enlistToCboBox("SELECT p.PartNo" &
                        " FROM product p" &
                        " INNER JOIN category c ON c.RowID=p.CategoryID" &
                        " WHERE c.CategoryName='Leave Type'" &
                        " AND p.OrganizationID='" & orgztnID & "';",
                       cboleavetypes)

        enlistToCboBox("SELECT Name FROM organization WHERE NoPurpose='0' ORDER BY Name;",
                       cboOrganization)

        If n_LeaveRowID <> Nothing Then

            Dim dtLeaveRow As New DataTable

            dtLeaveRow =
            New SQLQueryToDatatable("SELECT el.*" &
                        ",e.EmployeeID AS EmpID" &
                        ",IFNULL(og.Name,'') AS OrgName" &
                        ",TIME_FORMAT(el.LeaveStartTime,'%r') AS LvStartTime" &
                        ",TIME_FORMAT(el.LeaveEndTime,'%r') AS LvEndTime" &
                        " FROM employeeleave el" &
                        " LEFT JOIN employee e ON e.RowID=el.EmployeeID" &
                        " LEFT JOIN organization og ON og.RowID=el.OrganizationID" &
                        " WHERE el.RowID='" & n_LeaveRowID & "';").ResultTable

            If dtLeaveRow IsNot Nothing Then

                For Each drow As DataRow In dtLeaveRow.Rows

                    TxtEmployeeNumber1.Text = If(IsDBNull(drow("EmpID")), "", CStr(drow("EmpID")))

                    cboleavetypes.Text = CStr(drow("LeaveType"))

                    Dim ii = Format(CDate(drow("LeaveStartDate")), machineShortDateFormat)

                    dtpstartdate.Text = ii

                    ii = Format(CDate(drow("LeaveEndDate")), machineShortDateFormat)

                    dtpendate.Text = ii

                    Dim iii = Format(CDate(drow("LvStartTime")), "hh:mm tt")

                    dtpstarttime.Value = dtpstarttime.MinDate & " " & iii

                    iii = Format(CDate(drow("LvEndTime")), "hh:mm tt")

                    dtpendtime.Value = dtpendtime.MinDate & " " & iii

                    txtreason.Text = If(IsDBNull(drow("Reason")), "", CStr(drow("Reason")))

                    txtcomments.Text = If(IsDBNull(drow("Comments")), "", CStr(drow("Comments")))

                    CboListOfValue1.Text = If(IsDBNull(drow("Status")), "", CStr(drow("Status")))

                    cboOrganization.Text = CStr(drow("OrgName"))

                Next

            End If

        End If

        With TxtEmployeeFullName1

            .Text = String.Empty
            .Enabled = False
            .EmployeeTableColumnName = "CONCAT(LastName,', ',FirstName,IF(MiddleName = '', '', CONCAT(', ',MiddleName)))"
            '"CONCAT(LastName,', ',FirstName,', ',MiddleName)"

        End With

        Panel1.Focus()

        bgwEmpNames.RunWorkerAsync()

    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click

        'TxtEmployeeNumber1.Focus()

        btnApply.Focus()

        'cboOrganization.Focus()

        'btnApply.Focus()

        If TxtEmployeeNumber1.Exists _
            And TxtEmployeeFullName1.Exists Then

            Panel1.Enabled = False
            bgSaving.RunWorkerAsync()

        End If

    End Sub

    Function INSUPD_employeeleave() As Object

        Dim param(14, 2) As Object

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
        param(14, 0) = "elv_OverrideLeaveBal"

        param(0, 1) = If(n_LeaveRowID = Nothing, DBNull.Value, n_LeaveRowID)
        param(1, 1) = EXECQUER("SELECT OrganizationID FROM employee WHERE RowID='" & e_rowid & "';") 'TxtEmployeeNumber1.RowIDValue
        'previous_orgID
        Dim iii = Format(CDate(dtpstarttime.Value), "hh:mm tt")

        param(2, 1) = MilitTime(iii) 'Start time

        'Dim leave_type_str = cboleavetypes.Text

        'param(3, 1) = LeaveTypeValue 'Leave type
        param(3, 1) = cboleavetypes.Tag(1) 'Leave type
        param(4, 1) = If(z_User = 0, DBNull.Value, z_User)
        param(5, 1) = param(4, 1) 'z_User
        param(6, 1) = e_rowid 'TxtEmployeeNumber1.RowIDValue 'n_EmployeeRowID

        iii = Format(CDate(dtpendtime.Value), "hh:mm tt")

        param(7, 1) = MilitTime(iii) 'End time
        param(8, 1) = dtpstartdate.Tag 'Start date
        param(9, 1) = dtpendate.Tag 'End date
        param(10, 1) = Trim(txtreason.Text) 'Reason
        param(11, 1) = Trim(txtcomments.Text) 'Comments

        Dim imageobj As Object = DBNull.Value

        'If(r.Cells("elv_Image").Value Is Nothing, _
        '                                    DBNull.Value, _
        '                                    r.Cells("elv_Image").Value) 'Image

        param(12, 1) = imageobj

        param(13, 1) = If(LeaveStatusValue = Nothing, AccuPay.Entity.Leave.StatusPending, LeaveStatusValue) 'CboListOfValue1.Text

        Dim leavetypeRowID = If(cboleavetypes.Tag(0) Is Nothing, DBNull.Value,
                                If(cboleavetypes.Tag(0) = Nothing, DBNull.Value, cboleavetypes.Tag(0)))

        'param(14, 1) = leavetypeRowID
        param(14, 1) = 0

        Return _
                EXEC_INSUPD_PROCEDURE(param,
                                      "INSUPD_employeeleave_indepen",
                                      "empleaveID")

    End Function

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

                'Dim i As Integer = StrReverse("3:15 AM").ToString.IndexOf(" ")

                ''endtime = endtime.ToString.Replace("A", "")

                'MsgBox(Trim(StrReverse(StrReverse("3:15 AM").ToString.Substring(i, ("3:15 AM").ToString.Length - i))).Length)

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

    Private Sub bgSaving_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgSaving.DoWork

        'Dim leave_type_str = cboleavetypes.Text

        'leave_type_str = CboListOfValue2.Text

        'LeaveRowID
        If n_LeaveRowID = Nothing Then
            n_LeaveRowID =
                INSUPD_employeeleave()
        Else
            INSUPD_employeeleave()
        End If

    End Sub

    Private Sub bgSaving_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgSaving.ProgressChanged

    End Sub

    Private Sub bgSaving_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgSaving.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Background work cancelled.")
        Else

            If TxtEmployeeNumber1.Exists Then
                MsgBox("Leave successfully saved", MsgBoxStyle.Information)
                Me.Close()
            End If

        End If

        Panel1.Enabled = True

    End Sub

    Private Sub TxtEmployeeNumber1_GotFocus(sender As Object, e As EventArgs) Handles TxtEmployeeNumber1.GotFocus

        If TxtEmployeeFullName1.RowIDValue.Length = 0 Then
        Else

            'Dim selectEmpID = EXECQUER("SELECT EmployeeID FROM employee WHERE RowID='" & TxtEmployeeFullName1.RowIDValue & "' LIMIT 1;")

            'TxtEmployeeNumber1.Text = selectEmpID.ToString

        End If

    End Sub

    Dim appropriategenderleave As String = String.Empty

    Dim ogleavetype As New DataTable

    Private Sub TxtEmployeeNumber1_Leave(sender As Object, e As EventArgs) Handles TxtEmployeeNumber1.Leave

        RemoveHandler cboleavetypes.SelectedIndexChanged, AddressOf cboleavetypes_SelectedIndexChanged
        RemoveHandler cboleavetypes.SelectedValueChanged, AddressOf cboleavetypes_SelectedIndexChanged

        If orgztnID = Nothing Then
            'SQLQueryToDatatable

            Dim dtEmp As New DataTable

            dtEmp = New SQLQueryToDatatable("SELECT OrganizationID,Gender FROM employee WHERE RowID='" & TxtEmployeeNumber1.RowIDValue & "';").ResultTable

            Dim org_rowid = Nothing 'New ExecuteQuery("SELECT OrganizationID FROM employee WHERE RowID='" & TxtEmployeeNumber1.RowIDValue & "';").Result

            For Each erow As DataRow In dtEmp.Rows
                org_rowid = erow("OrganizationID")

                Dim sex = erow("Gender").ToString

                If sex = "M" Then
                    appropriategenderleave = "Paternity"
                Else
                    appropriategenderleave = "Maternity"
                End If

            Next

            'ogleavetype.Dispose()

            'ogleavetype = New DataTable

            ogleavetype = New SQLQueryToDatatable("SELECT p.RowID" &
                                                  ",IF(p.PartNo LIKE '%aternity%', '" & appropriategenderleave & "', p.PartNo) AS PartNo" &
                                                  " FROM product p" &
                                                  " INNER JOIN category c ON c.RowID=p.CategoryID" &
                                                  " WHERE c.CategoryName='Leave Type'" &
                                                  " AND p.OrganizationID='" & org_rowid & "';").ResultTable

            With cboleavetypes
                .ValueMember = ogleavetype.Columns(0).ColumnName
                .DisplayMember = ogleavetype.Columns(1).ColumnName
                .DataSource = ogleavetype
            End With

            'enlistToCboBox("SELECT IF(p.PartNo LIKE '%aternity%', '" & appropriategenderleave & "', p.PartNo)" &
            '               " FROM product p" &
            '               " INNER JOIN category c ON c.RowID=p.CategoryID" &
            '               " WHERE c.CategoryName='Leave Type'" &
            '               " AND p.OrganizationID='" & org_rowid & "';",
            '               cboleavetypes)

        End If

        AddHandler cboleavetypes.SelectedIndexChanged, AddressOf cboleavetypes_SelectedIndexChanged
        AddHandler cboleavetypes.SelectedValueChanged, AddressOf cboleavetypes_SelectedIndexChanged

    End Sub

    Private Sub TxtEmployeeNumber1_TextChanged(sender As Object, e As EventArgs) Handles TxtEmployeeNumber1.TextChanged

    End Sub

    Dim LeaveTypeValue As String = String.Empty

    Private Sub cboleavetypes_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cboleavetypes.SelectedIndexChanged, cboleavetypes.SelectedValueChanged

        LeaveTypeValue = ""

        If ogleavetype Is Nothing Then
        Else

            If ogleavetype.Columns.Count > 0 Then

                If ogleavetype.Rows.Count > 0 Then

                    Dim sel_ogleavetype = ogleavetype.Select("RowID = " & ValNoComma(cboleavetypes.SelectedValue))

                    For Each lvrow In sel_ogleavetype
                        LeaveTypeValue = lvrow("PartNo").ToString 'cboleavetypes.Text
                    Next

                End If

            End If

        End If

    End Sub

    Dim LeaveStatusValue As String = String.Empty

    Private Sub CboListOfValue1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboListOfValue1.SelectedIndexChanged, CboListOfValue1.SelectedValueChanged
        LeaveStatusValue = CboListOfValue1.Text
    End Sub

    Private Sub cboOrganization_Leave(sender As Object, e As EventArgs) Handles cboOrganization.Leave

        previous_orgID = EXECQUER("SELECT RowID FROM organization WHERE Name='" & cboOrganization.Text & "';")

    End Sub

    Private Sub cboOrganization_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOrganization.SelectedIndexChanged

    End Sub

    Dim catchdt As New DataTable

    Private Sub bgwEmpNames_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwEmpNames.DoWork

        Thread.Sleep(750)

        Dim dtempfullname As New DataTable
        'CONCAT(e.LastName,', ',e.FirstName, IF(e.MiddleName = '', '', CONCAT(', ',e.MiddleName)))
        dtempfullname = New SQLQueryToDatatable("SELECT e.RowID, CONCAT_WS(', ', e.LastName, e.FirstName, IF(LENGTH(TRIM(e.MiddleName)) = 0, NULL, e.MiddleName)) `EmpFullName`" &
                                    " FROM employee e INNER JOIN organization og ON og.RowID=e.OrganizationID" &
                                    " WHERE og.NoPurpose='0'" &
                                    " GROUP BY e.OrganizationID,e.EmployeeID;").ResultTable

        If dtempfullname IsNot Nothing Then

            For Each drow As DataRow In dtempfullname.Rows

                If CStr(drow("EmpFullName")) <> Nothing Then
                    'autcoEmpID.Items.Add(New AutoCompleteEntry(CStr(drow("EmployeeID"))))

                    TxtEmployeeFullName1.Items.Add(New AutoCompleteEntry(CStr(drow("EmpFullName")), StringToArray(CStr(drow("EmpFullName")))))

                End If

            Next

            catchdt = dtempfullname

        End If

    End Sub

    Private Sub bgwEmpNames_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwEmpNames.ProgressChanged

    End Sub

    Private Sub bgwEmpNames_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwEmpNames.RunWorkerCompleted

        Panel1.Focus()

        If e.Error IsNot Nothing Then

            MessageBox.Show("Error: " & e.Error.Message)

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")
        Else

            TxtEmployeeFullName1.Enabled = True

            TxtEmployeeFullName1.Text = ""

            cboxEmployees.DisplayMember = "EmpFullName"
            cboxEmployees.ValueMember = "RowID"
            cboxEmployees.DataSource = catchdt

        End If

        TxtEmployeeFullName1.Focus()

        cboxEmployees.Text = String.Empty
    End Sub

    Private Sub dtpstartdate_ValueChanged(sender As Object, e As EventArgs) Handles dtpstartdate.ValueChanged
        dtpstartdate.Tag = Format(CDate(dtpstartdate.Value), "yyyy-MM-dd")
    End Sub

    Private Sub dtpendate_ValueChanged(sender As Object, e As EventArgs) Handles dtpendate.ValueChanged
        dtpendate.Tag = Format(CDate(dtpendate.Value), "yyyy-MM-dd")
    End Sub

    Private Sub cboleavetypes_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles cboleavetypes.SelectedIndexChanged
        Dim str_values() As String = {cboleavetypes.SelectedValue, cboleavetypes.Text}

        cboleavetypes.Tag = str_values

    End Sub

    Dim e_rowid As Integer = 0

    Private Sub cboxEmployees_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboxEmployees.SelectedIndexChanged,
                                                                                             cboxEmployees.SelectedValueChanged

        e_rowid = Convert.ToInt32(cboxEmployees.SelectedValue)

        Label9.Text = e_rowid

        Dim selectEmpID = EXECQUER("SELECT EmployeeID FROM employee WHERE RowID='" & e_rowid & "' LIMIT 1;")

        TxtEmployeeNumber1.Text = Convert.ToString(selectEmpID)

        TxtEmployeeNumber1.RowIDValue = e_rowid

    End Sub

    Private Sub TxtEmployeeFullName1_Leave(sender As Object, e As EventArgs) Handles TxtEmployeeFullName1.Leave

        If TxtEmployeeFullName1.Text.Trim.Length = 0 Then

            cboxEmployees.Text = String.Empty
        Else

            cboxEmployees.Text = TxtEmployeeFullName1.Text

        End If

    End Sub

    Private Sub TxtEmployeeFullName1_Load(sender As Object, e As EventArgs) Handles TxtEmployeeFullName1.Load

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class