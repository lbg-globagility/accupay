Imports System.Threading
Imports Femiani.Forms.UI.Input

Public Class LeaveForm

    Private LeaveRowID As String = String.Empty

    Private previous_orgID As String = String.Empty

    Private appropriategenderleave As String = String.Empty

    Private ogleavetype As New DataTable

    Private LeaveTypeValue As String = String.Empty

    Private e_rowid As Integer = 0

    Private LeaveStatusValue As String = String.Empty

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        LeaveForm()
    End Sub

    Sub LeaveForm()
        dtpstarttime.CustomFormat = machineShortTimeFormat
        dtpendtime.CustomFormat = machineShortTimeFormat
    End Sub

    Private Sub LeaveForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Panel1.Enabled = True Then
            e.Cancel = False
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub LeaveForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        enlistToCboBox("SELECT Name FROM organization WHERE NoPurpose='0' ORDER BY Name;",
                       cboOrganization)

        With TxtEmployeeFullName1
            .Text = String.Empty
            .Enabled = False
            .EmployeeTableColumnName = "CONCAT(LastName,', ',FirstName,IF(MiddleName = '', '', CONCAT(', ',MiddleName)))"
        End With

        Panel1.Focus()

        bgwEmpNames.RunWorkerAsync()
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        btnApply.Focus()

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

        param(0, 1) = If(LeaveRowID = Nothing, DBNull.Value, LeaveRowID)
        param(1, 1) = EXECQUER("SELECT OrganizationID FROM employee WHERE RowID='" & e_rowid & "';") 'TxtEmployeeNumber1.RowIDValue
        'previous_orgID

        param(2, 1) = dtpstarttime.Value.ToString("HH:mm:00")

        'param(3, 1) = LeaveTypeValue 'Leave type
        param(3, 1) = cboleavetypes.Tag(1) 'Leave type
        param(4, 1) = If(z_User = 0, DBNull.Value, z_User)
        param(5, 1) = param(4, 1) 'z_User
        param(6, 1) = e_rowid 'TxtEmployeeNumber1.RowIDValue 'n_EmployeeRowID

        param(7, 1) = dtpendtime.Value.ToString("HH:mm:00")
        param(8, 1) = dtpstartdate.Value.ToString("yyyy-MM-dd") 'Start date
        param(9, 1) = dtpendate.Value.ToString("yyyy-MM-dd") 'End date
        param(10, 1) = Trim(txtreason.Text) 'Reason
        param(11, 1) = Trim(txtcomments.Text) 'Comments

        Dim imageobj As Object = DBNull.Value
        param(12, 1) = imageobj
        param(13, 1) = If(LeaveStatusValue = Nothing, "Pending", LeaveStatusValue) 'CboListOfValue1.Text

        Dim leavetypeRowID = If(cboleavetypes.Tag(0) Is Nothing, DBNull.Value,
                                If(cboleavetypes.Tag(0) = Nothing, DBNull.Value, cboleavetypes.Tag(0)))

        param(14, 1) = 0

        Return EXEC_INSUPD_PROCEDURE(param,
                                      "INSUPD_employeeleave_indepen",
                                      "empleaveID")
    End Function

    Private Sub bgSaving_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgSaving.DoWork
        'LeaveRowID
        If LeaveRowID = Nothing Then
            LeaveRowID =
                INSUPD_employeeleave()
        Else
            INSUPD_employeeleave()
        End If
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

    Private Sub TxtEmployeeNumber1_Leave(sender As Object, e As EventArgs) Handles TxtEmployeeNumber1.Leave
        RemoveHandler cboleavetypes.SelectedIndexChanged, AddressOf cboleavetypes_SelectedIndexChanged
        RemoveHandler cboleavetypes.SelectedValueChanged, AddressOf cboleavetypes_SelectedIndexChanged

        If orgztnID = Nothing Then
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
        End If

        AddHandler cboleavetypes.SelectedIndexChanged, AddressOf cboleavetypes_SelectedIndexChanged
        AddHandler cboleavetypes.SelectedValueChanged, AddressOf cboleavetypes_SelectedIndexChanged
    End Sub

    Private Sub cboleavetypes_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cboleavetypes.SelectedIndexChanged, cboleavetypes.SelectedValueChanged
        LeaveTypeValue = ""

        If ogleavetype Is Nothing Then
        Else
            If ogleavetype.Columns.Count > 0 Then
                If ogleavetype.Rows.Count > 0 Then

                    Dim sel_ogleavetype = ogleavetype.Select("RowID = " & ValNoComma(cboleavetypes.SelectedValue))

                    For Each lvrow In sel_ogleavetype
                        LeaveTypeValue = lvrow("PartNo").ToString
                    Next

                End If
            End If
        End If
    End Sub

    Private Sub CboListOfValue1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboListOfValue1.SelectedIndexChanged, CboListOfValue1.SelectedValueChanged
        LeaveStatusValue = CboListOfValue1.Text
    End Sub

    Private Sub cboOrganization_Leave(sender As Object, e As EventArgs) Handles cboOrganization.Leave
        previous_orgID = EXECQUER("SELECT RowID FROM organization WHERE Name='" & cboOrganization.Text & "';")
    End Sub

    Dim catchdt As New DataTable

    Private Sub bgwEmpNames_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwEmpNames.DoWork
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

    Private Sub cboleavetypes_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles cboleavetypes.SelectedIndexChanged
        Dim str_values() As String = {cboleavetypes.SelectedValue, cboleavetypes.Text}

        cboleavetypes.Tag = str_values
    End Sub

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

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class