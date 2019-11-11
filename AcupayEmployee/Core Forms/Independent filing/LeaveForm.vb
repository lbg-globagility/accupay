Imports System.Threading
Imports Femiani.Forms.UI.Input
Imports AccuPay.Entity
Imports System.Data.Entity

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

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        btnApply.Focus()

        Try
            Using context = New PayrollContext()
                Dim employee = context.Employees.
                    FirstOrDefault(Function(emp) emp.RowID = e_rowid)

                Dim leave = New Leave() With {
                    .OrganizationID = employee.OrganizationID,
                    .CreatedBy = 0,
                    .LeaveType = cboleavetypes.Tag(1),
                    .EmployeeID = employee.RowID,
                    .StartTime = dtpstarttime.Value.TimeOfDay,
                    .EndTime = dtpendtime.Value.TimeOfDay,
                    .StartDate = dtpstartdate.Value,
                    .EndDate = dtpendate.Value,
                    .Reason = txtreason.Text,
                    .Comments = txtcomments.Text,
                    .Status = "Pending"
                }

                Dim ledger = context.LeaveLedgers.Include(Function(l) l.LastTransaction).
                    Where(Function(l) l.EmployeeID = e_rowid).
                    Where(Function(l) l.Product.Name = leave.LeaveType).
                    FirstOrDefault()

                If leave.IsPaid Then
                    If ledger Is Nothing Then
                        MsgBox($"Sorry, but we can't find your {leave.LeaveType} leave ledger. Please contact management.")
                        Return
                    End If

                    If ledger.LastTransaction Is Nothing Or ledger.LastTransaction.Balance <= 0 Then
                        MsgBox($"Sorry, but you don't have anymore remaining balance for {leave.LeaveType} leaves. Please contact management.")
                        Return
                    End If
                End If

                context.Leaves.Add(leave)
                context.SaveChanges()

                MsgBox("Leave has been submitted.", MsgBoxStyle.OkOnly, "Leave Submitted")
                Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Failed to save leave")
        End Try
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
