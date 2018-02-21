Imports Femiani.Forms.UI.Input
Imports MySql.Data.MySqlClient

Public Class OffSetting

    Protected Overrides Sub OnLoad(e As EventArgs)

        eosStatus.ValueMember = "DisplayValue"
        eosStatus.DisplayMember = "DisplayValue"
        eosStatus.DataSource = SQL_GetDataTable("SELECT DisplayValue FROM listofval WHERE `Type`='Leave Status';")

        btnRefresh_Click(btnRefresh, e)

        MyBase.OnLoad(e)

    End Sub

    Private Sub OffSetting_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        InfoBalloon(, , lblforballoon, , , 1)

        HRISForm.listHRISForm.Remove(Me.Name)

    End Sub

    Private Sub OffSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dgvemployees_SelectionChanged(sender, e)

        bgworkSearchAutoComplete.RunWorkerAsync()

    End Sub

    Private Sub tsbtnNew_Click(sender As Object, e As EventArgs) Handles tsbtnNew.Click

        dgvempoffset.EndEdit(True)

        dgvempoffset.ClearSelection()

        If dgvempoffset.RowCount > max_row_count Then

            lnk = New LinkLabel.Link

            lnk.Name = "Nxt"

            'Pagination_Link(First, New LinkLabelLinkClickedEventArgs(lnk))
            lnkFirst_LinkClicked(lnkNxt, New LinkLabelLinkClickedEventArgs(lnk))

        End If

        dgvempoffset.Focus()

        For Each dgvrow As DataGridViewRow In dgvempoffset.Rows
            If dgvrow.IsNewRow Then

                dgvrow.Cells("eosStartTime").Selected = True

                Exit For

            End If

        Next

    End Sub

    Private Sub tsbtnSave_Click(sender As Object, e As EventArgs) Handles tsbtnSave.Click

        tsbtnSave.Enabled = False

        dgvempoffset.EndEdit(True)

        If publicEmpRowID = Nothing Then

        Else

            For Each dgvrow As DataGridViewRow In dgvempoffset.Rows

                With dgvrow

                    If .IsNewRow = False Then

                        Dim eosRowID = Nothing

                        If .Cells("eosRowID").Value = Nothing Then

                            .Cells("eosRowID").Value =
                                INSUPD_employeeoffset(,
                                                    publicEmpRowID, .Cells("eosRowID").Value,
                                                    .Cells("eosStartTime").Value,
                                                    .Cells("eosEndTime").Value,
                                                    .Cells("eosStartDate").Value,
                                                    .Cells("eosEndDate").Value,
                                                    .Cells("eosStatus").Value,
                                                    .Cells("eosReason").Value,
                                                    .Cells("eosComment").Value)

                        Else

                            If listofEditRow.Contains(.Cells("eosRowID").Value) Then
                                eosRowID = .Cells("eosRowID").Value

                                INSUPD_employeeoffset(eosRowID,
                                                    publicEmpRowID, .Cells("eosRowID").Value,
                                                    .Cells("eosStartTime").Value,
                                                    .Cells("eosEndTime").Value,
                                                    .Cells("eosStartDate").Value,
                                                    .Cells("eosEndDate").Value,
                                                    .Cells("eosStatus").Value,
                                                    .Cells("eosReason").Value,
                                                    .Cells("eosComment").Value)

                            End If

                        End If

                    End If

                End With

            Next

            listofEditRow.Clear()

            btnRefresh_Click(btnRefresh, e)

        End If

        tsbtnSave.Enabled = True

        InfoBalloon("Successfully saved.",
                  "Successfully saved.", lblforballoon, 0, -69)

        lnk = New LinkLabel.Link

        lnk.Name = "First"

        Pagination_Link(First, New LinkLabelLinkClickedEventArgs(lnk))

    End Sub

    Function INSUPD_employeeoffset(Optional eosRowID As Object = Nothing,
                              Optional eosEmployeeID As Object = Nothing,
                              Optional eosType As Object = Nothing,
                              Optional eosStartTime As Object = Nothing,
                              Optional eosEndTime As Object = Nothing,
                              Optional eosStartDate As Object = Nothing,
                              Optional eosEndDate As Object = Nothing,
                              Optional eosStatus As Object = Nothing,
                              Optional eosReason As Object = Nothing,
                              Optional eosComments As Object = Nothing) As Object

        'eosRowID,eosOrganizationID,eosEmployeeID,eosUserRowID,eosType,eosStartTime,eosEndTime,eosStartDate,eosEndDate,eosStatus,eosReason,eosComments

        'INSUPD_employeeoffset

        Dim returnval = Nothing

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("INSUPD_employeeoffset", conn)
            conn.Open()
            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("returnvalue", MySqlDbType.Int32)

                .Parameters.AddWithValue("eosRowID", If(eosRowID = Nothing, DBNull.Value, eosRowID))

                .Parameters.AddWithValue("eosOrganizationID", orgztnID)

                .Parameters.AddWithValue("eosEmployeeID", eosEmployeeID)

                .Parameters.AddWithValue("eosUserRowID", z_User)

                .Parameters.AddWithValue("eosType", eosType)

                If IsDBNull(eosStartTime) Then

                    .Parameters.AddWithValue("eosStartTime", DBNull.Value)

                ElseIf eosStartTime = Nothing Then

                    .Parameters.AddWithValue("eosStartTime", DBNull.Value)

                ElseIf eosStartTime.ToString.Trim = String.Empty Then

                    .Parameters.AddWithValue("eosStartTime", DBNull.Value)

                Else
                    Dim last_time =
                              Format(CDate(eosStartTime), "HH:mm")

                    .Parameters.AddWithValue("eosStartTime", last_time)

                End If

                If IsDBNull(eosEndTime) Then

                    .Parameters.AddWithValue("eosEndTime", DBNull.Value)

                ElseIf eosEndTime = Nothing Then

                    .Parameters.AddWithValue("eosEndTime", DBNull.Value)

                ElseIf eosEndTime.ToString.Trim = String.Empty Then

                    .Parameters.AddWithValue("eosEndTime", DBNull.Value)

                Else
                    Dim last_time =
                              Format(CDate(eosEndTime), "HH:mm")

                    .Parameters.AddWithValue("eosEndTime", last_time)

                End If

                .Parameters.AddWithValue("eosStartDate", If(eosStartDate = Nothing, DBNull.Value, Format(CDate(eosStartDate), "yyyy-MM-dd")))

                .Parameters.AddWithValue("eosEndDate", If(eosEndDate = Nothing, DBNull.Value, Format(CDate(eosEndDate), "yyyy-MM-dd")))


                If eosStatus Is Nothing Then
                    .Parameters.AddWithValue("eosStatus", String.Empty)

                ElseIf IsDBNull(eosStatus) Then
                    .Parameters.AddWithValue("eosStatus", String.Empty)

                Else
                    .Parameters.AddWithValue("eosStatus", eosStatus.ToString.Trim)

                End If


                If eosReason Is Nothing Then
                    .Parameters.AddWithValue("eosReason", String.Empty)

                ElseIf IsDBNull(eosReason) Then
                    .Parameters.AddWithValue("eosReason", String.Empty)

                Else
                    .Parameters.AddWithValue("eosReason", eosReason.ToString.Trim)

                End If


                If eosComments Is Nothing Then
                    .Parameters.AddWithValue("eosComments", String.Empty)

                ElseIf IsDBNull(eosComments) Then
                    .Parameters.AddWithValue("eosComments", String.Empty)

                Else
                    .Parameters.AddWithValue("eosComments", eosComments.ToString.Trim)

                End If


                .Parameters("returnvalue").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

                If datread.Read = True Then
                    returnval = If(IsDBNull(datread(0)), Nothing, datread(0))

                End If

            End With

        Catch ex As Exception
            MsgBox(ex.Message, , "Error : INSUPD_employeeoffset")

        End Try

        Return returnval

    End Function

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click

        dgvemployees_SelectionChanged(tsbtnCancel, e)

    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Me.Close()

    End Sub

    Private Sub dgvemployees_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployees.CellContentClick

    End Sub

    Private Sub dgvemployees_SelectionChanged(sender As Object, e As EventArgs)
        haserrinput = 0
        dgvempoffset.ShowCellErrors = False
        listofEditRow.Clear()

        If dgvemployees.RowCount <> 0 Then

            With dgvemployees.CurrentRow

                txtFName.Text = .Cells("eFullName").Value

                txtEmpID.Text = .Cells("eDetails").Value

                lblOffSetBal.Text = .Cells("eOffsetBal").Value & " hour(s)"


                VIEW_employeeoffset(.Cells("eRowID").Value)

            End With

        Else

            publicEmpRowID = Nothing

            dgvempoffset.Rows.Clear()

            txtFName.Text = String.Empty

            txtEmpID.Text = txtFName.Text

            lblOffSetBal.Text = txtEmpID.Text

        End If

    End Sub

    Dim publicEmpRowID = Nothing

    Sub VIEW_employeeoffset(ByVal EmpRowID As Object)

        publicEmpRowID = EmpRowID

        dgvempoffset.Rows.Clear()

        Dim dt_eos As New DataTable

        dt_eos = retAsDatTbl("CALL VIEW_employeeoffset('" & orgztnID & "','" & EmpRowID & "', '" & pagenumber & "');")

        For Each erow As DataRow In dt_eos.Rows

            Dim row_array = erow.ItemArray

            dgvempoffset.Rows.Add(row_array)

        Next

        dt_eos.Dispose()

    End Sub

    Const max_row_count = 20

    Private Sub dgvempoffset_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvempoffset.CellBeginEdit

        e.Cancel = (e.RowIndex >= max_row_count)

    End Sub

    Private Sub dgvempoffset_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempoffset.CellContentClick

    End Sub

    Dim haserrinput As SByte

    Dim listofEditRow As New AutoCompleteStringCollection

    Dim reset_static As SByte = -1

    Private Sub dgvempoffset_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempoffset.CellEndEdit

        dgvempoffset.ShowCellErrors = True

        'eosStartTime

        'eosEndTime

        Dim colName As String = dgvempoffset.Columns(e.ColumnIndex).Name
        Dim rowindx = e.RowIndex

        Static num As Integer = If(reset_static = -1,
                                   -1,
                                   num)

        If dgvempoffset.RowCount <> 0 Then
            With dgvempoffset

                If Val(dgvempoffset.Item("eosRowID", e.RowIndex).Value) <> 0 Then

                    listofEditRow.Add(dgvempoffset.Item("eosRowID", e.RowIndex).Value)

                Else

                End If

                If (colName = "f5a1ds5f1a5ds1f5ad1f1asd") Then

                    If Trim(dgvempoffset.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvempoffset.Item(colName, rowindx).Value)
                        Try
                            dgvempoffset.Item(colName, rowindx).Value = Format(CDate(dateobj), "M/dd/yyyy")

                            haserrinput = 0

                            dgvempoffset.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            haserrinput = 1
                            dgvempoffset.Item(colName, rowindx).ErrorText = "     Invalid date value"

                        End Try

                    Else
                        haserrinput = 0

                        dgvempoffset.Item(colName, rowindx).ErrorText = Nothing

                    End If

                ElseIf (colName = "eosStartTime" Or colName = "eosEndTime") Then

                    If Trim(dgvempoffset.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvempoffset.Item(colName, rowindx).Value).Replace(" ", ":")

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
                                dgvempoffset.Item(colName, rowindx).Value = valtime.ToShortTimeString
                            Else
                                dgvempoffset.Item(colName, rowindx).Value = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                            End If

                            haserrinput = 0

                            dgvempoffset.Item(colName, rowindx).ErrorText = Nothing

                        Catch ex As Exception
                            Try
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, 5))
                                dateobj = dateobj.ToString.Replace(" ", ":")

                                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")

                                dgvempoffset.Item(colName, rowindx).Value = valtime.ToShortTimeString

                                haserrinput = 0

                                dgvempoffset.Item(colName, rowindx).ErrorText = Nothing
                            Catch ex_1 As Exception
                                haserrinput = 1
                                dgvempoffset.Item(colName, rowindx).ErrorText = "     Invalid time value"

                            End Try

                        End Try

                    Else
                        haserrinput = 0

                        dgvempoffset.Item(colName, rowindx).ErrorText = Nothing

                    End If

                End If

            End With

        End If

    End Sub

    Dim dt_emp As New DataTable

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        If sender.Name = "btnRefresh" Then

            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        End If

        btnRefresh.Enabled = False

        Static prev_rowindx As Integer = -1

        If dgvemployees.RowCount <> 0 Then
            prev_rowindx = dgvemployees.CurrentRow.Index

        End If

        dt_emp = New DataTable

        Dim searchstring = String.Empty

        If AutoCompleteTextBox1.Text.Trim.Length <> 0 Then
            searchstring = AutoCompleteTextBox1.Text

        End If

        dt_emp = retAsDatTbl("CALL VIEW_employee1('" & orgztnID & "', '" & pagination & "', '" & searchstring & "');")

        dgvemployees.Rows.Clear()

        For Each drow As DataRow In dt_emp.Rows

            Dim row_array = drow.ItemArray

            dgvemployees.Rows.Add(row_array)

        Next

        dt_emp.Dispose()

        If dgvemployees.RowCount <> 0 _
            And prev_rowindx <> -1 Then

            If prev_rowindx < dgvemployees.RowCount Then
                dgvemployees.Item("eEmpID", prev_rowindx).Selected = True

            End If

        End If

        btnRefresh.Enabled = True

        If sender.Name = "btnRefresh" Then

            AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        End If

    End Sub

    Dim pagination As Integer = 0

    Dim lnk As LinkLabel.Link

    Private Sub Pagination_Link(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked,
                                                                                                Prev.LinkClicked,
                                                                                                Nxt.LinkClicked,
                                                                                                Last.LinkClicked

        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "First" Then
            pagination = 0
        ElseIf sendrname = "Prev" Then

            Dim modcent = pagination Mod 20

            If modcent = 0 Then

                pagination -= 20

            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sendrname = "Nxt" Then

            Dim modcent = pagination Mod 20

            If modcent = 0 Then
                pagination += 20

            Else
                pagination -= modcent

                pagination += 20

            End If
        ElseIf sendrname = "Last" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 20 FROM employee WHERE OrganizationID=" & orgztnID & ";"))

            Dim remender = lastpage Mod 1

            pagination = lastpage - remender

            If pagination - 20 < 20 Then
                'pagination = 0

            End If

            'pagination = If(lastpage - 20 >= 20, _
            '                lastpage - 20, _
            '                lastpage)

        End If

        btnRefresh_Click(sender, e)

        dgvemployees_SelectionChanged(sender, e)

        AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem

        TabControlColor(TabControl1, e)

    End Sub

    Dim pagenumber As Integer = 0

    Private Sub lnkFirst_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkFirst.LinkClicked,
                                                                                                    lnkPrev.LinkClicked,
                                                                                                    lnkNxt.LinkClicked,
                                                                                                    lnkLast.LinkClicked

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "lnkFirst" Then
            pagenumber = 0
        ElseIf sendrname = "lnkPrev" Then

            Dim modcent = pagenumber Mod 20

            If modcent = 0 Then

                pagenumber -= 20

            Else

                pagenumber -= modcent

            End If

            If pagenumber < 0 Then

                pagenumber = 0

            End If

        ElseIf sendrname = "lnkNxt" Then

            Dim modcent = pagenumber Mod 20

            If modcent = 0 Then
                pagenumber += 20

            Else
                pagenumber -= modcent

                pagenumber += 20

            End If
        ElseIf sendrname = "lnkLast" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 20 FROM employeeoffset WHERE OrganizationID='" & orgztnID & "' AND EmployeeID='" & publicEmpRowID & "';"))

            Dim remender = lastpage Mod 1

            pagenumber = lastpage - remender

            If pagenumber - 20 < 20 Then
                'pagenumber = 0

            End If

            'pagenumber = If(lastpage - 20 >= 20, _
            '                lastpage - 20, _
            '                lastpage)

        End If

        dgvemployees_SelectionChanged(sender, e)

    End Sub

    Private Sub tsbtnDelete_Click(sender As Object, e As EventArgs) Handles tsbtnDelete.Click

        dgvempoffset.EndEdit(True)

        tsbtnDelete.Enabled = False

        dgvempoffset.Focus()

        Dim sel_dgvrows =
            dgvempoffset.Rows.OfType(Of DataGridViewRow).Where(Function(dgv) dgv.IsNewRow = False And dgv.Selected)

        Dim selected_row_count = sel_dgvrows.Count

        Dim sel_count = (selected_row_count - 1)

        If sel_count > -1 Then

            Dim result = MessageBox.Show("Are you sure you want to delete the selected offset ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

            If result = DialogResult.Yes Then

                For i = 0 To sel_count

                    Dim seldgv = sel_dgvrows(i)


                    Dim str_quer As String =
                        String.Concat("DELETE FROM employeeoffset WHERE RowID = ?offset_rowid;")

                    Dim _params = New Object() {seldgv.Cells("eosRowID").Value}

                    Dim sql As New SQL(str_quer,
                                       _params)

                    sql.ExecuteQuery()

                    dgvempoffset.Rows.Remove(seldgv)

                Next

                lnk = New LinkLabel.Link

                lnk.Name = "First"

                Pagination_Link(First, New LinkLabelLinkClickedEventArgs(lnk))

            End If

        End If

        'InfoBalloon("Successfully saved.", _
        '          "Successfully saved.", lblforballoon, 0, -69)


        tsbtnDelete.Enabled = True

    End Sub

    Private Sub bgworkSearchAutoComplete_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkSearchAutoComplete.DoWork

        Dim EmpIDFNameLName As New DataTable

        EmpIDFNameLName = retAsDatTbl("SELECT EmployeeID, FirstName, LastName FROM employee WHERE OrganizationID='" & orgztnID & "' AND RevealInPayroll='1';")

        For Each drow As DataRow In EmpIDFNameLName.Rows

            With AutoCompleteTextBox1

                .Items.Add(New AutoCompleteEntry(CStr(drow("EmployeeID")), StringToArray(CStr(drow("EmployeeID")))))

                .Items.Add(New AutoCompleteEntry(CStr(drow("FirstName")), StringToArray(CStr(drow("FirstName")))))

                .Items.Add(New AutoCompleteEntry(CStr(drow("LastName")), StringToArray(CStr(drow("LastName")))))

            End With

        Next

        EmpIDFNameLName.Dispose()

    End Sub

    Private Sub bgworkSearchAutoComplete_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkSearchAutoComplete.ProgressChanged

    End Sub

    Private Sub bgworkSearchAutoComplete_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkSearchAutoComplete.RunWorkerCompleted

        AutoCompleteTextBox1.Enabled = True

    End Sub

    Private Sub AutoCompleteTextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles AutoCompleteTextBox1.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then

            btnRefresh_Click(btnRefresh, New EventArgs)

        End If

    End Sub

    Private Sub AutoCompleteTextBox1_TextChanged(sender As Object, e As EventArgs) Handles AutoCompleteTextBox1.TextChanged

    End Sub

End Class