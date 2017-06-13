Imports System.Data.OleDb
Imports MySql.Data.MySqlClient

Public Class PhiHealth
    Public q_payphhealth As String = "SELECT phh.RowID," & _
    "COALESCE(phh.SalaryBracket,0) 'Salary Bracket'," & _
    "COALESCE(phh.SalaryRangeFrom,0) 'Salary Range From'," & _
    "COALESCE(phh.SalaryRangeTo,0) 'Salary Range To'," & _
    "COALESCE(phh.SalaryBase,0) 'Salary Base'," & _
    "COALESCE(phh.EmployeeShare,0) 'Employee Share'," & _
    "COALESCE(phh.EmployerShare ,0) 'Employer Share'," & _
    "IFNULL(TotalMonthlyPremium,COALESCE(phh.EmployeeShare ,0) + COALESCE(phh.EmployerShare ,0)) 'Total Monthly Premium'," & _
    "DATE_FORMAT(phh.Created,'%m-%d-%Y') 'Creation Date'," & _
    "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by'," & _
    "COALESCE(DATE_FORMAT(phh.LastUpd,'%m-%d-%Y'),'') 'Last Update'," & _
    "(SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM user WHERE RowID=phh.LastUpdBy) 'LastUpdate by'" & _
    " FROM payphilhealth phh" & _
    " INNER JOIN user u ON phh.CreatedBy=u.RowID" &
    " WHERE phh.HiddenData='0'"

    Dim _editRowID As New List(Of String)
    Dim e_rindx, e_cindx, charcnt, _curCol, PhHlth_rcount As Integer
    Dim _cellVal, _now, u_nem As String
    Dim view_ID As Object
    Sub loadPhiHealth()
        dgvPhHlth.Rows.Clear()
        For Each r As DataRow In retAsDatTbl(q_payphhealth & " ORDER BY phh.SalaryBase").Rows 'ORDER BY phh.SalaryBracket
            dgvPhHlth.Rows.Add(r(0), r(1), r(2), r(3), r(4), r(5), r(6), r(7), r(8), r(9), r(10), r(11))
        Next
        PhHlth_rcount = dgvPhHlth.RowCount - 1
    End Sub

    Private Sub PhiHealth_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        InfoBalloon(, , lblforballoon, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        'If FormLeft.Contains("PhilHealth Contribution Table") Then
        '    FormLeft.Remove("PhilHealth Contribution Table")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.Text = "Welcome"
        'Else
        '    MDIPrimaryForm.Text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

        showAuditTrail.Close()

        GeneralForm.listGeneralForm.Remove(Me.Name)
    End Sub

    Dim dontUpdate As SByte = 0

    Dim dontCreate As SByte = 0

    Private Sub PhiHealth_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        'view_ID = VIEW_privilege(Me.Text, orgztnID)

        loadPhiHealth()

        AddHandler dgvPhHlth.EditingControlShowing, AddressOf dgvPhHlth_EditingControlShowing

        _now = EXECQUER(CURDATE_MDY)

        u_nem = EXECQUER(USERNameStrPropr & 1)

        view_ID = VIEW_privilege("PhilHealth Contribution Table", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then
            ToolStripButton2.Visible = 0
            ToolStripButton3.Visible = 0
            dontUpdate = 1
            dontCreate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    ToolStripButton3.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        'ToolStripButton2.Visible = 0
                        dontCreate = 1
                    Else
                        dontCreate = 0
                        'ToolStripButton2.Visible = 1
                    End If

                    If drow("Deleting").ToString = "N" Then
                        ToolStripButton3.Visible = 0
                    Else
                        ToolStripButton3.Visible = 1
                    End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

    End Sub
    Private Sub dgvPhHlth_CellKeyPress(sender As Object, e As KeyPressEventArgs)
        Dim e_asc As String = Asc(e.KeyChar)
        e.Handled = TrapDecimKey(e_asc)
        Dim _txtbox = DirectCast(sender, TextBox)
        charcnt = _txtbox.TextLength
        If e_asc = 46 Then
            txtDecimalPoint(charcnt, _txtbox)
        End If

        Try
            If _txtbox.Text <> "" Then
                If CInt(_txtbox.Text) < Integer.MaxValue Then '2147483647
                    'MsgBox("Mali ang Quantity mo!")
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & "Please input an appropriate value.", MsgBoxStyle.Exclamation, "Too much Numeric Value")
        End Try
    End Sub
    Dim _txtCell As New TextBox
    Private Sub dgvPhHlth_TextChanged(sender As Object, e As EventArgs)
        _txtCell = DirectCast(sender, TextBox)
        Try
            With dgvPhHlth.CurrentRow
                If _curCol = 5 Then
                    .Cells("Column8").Value = Val(_txtCell.Text) _
                                            + Val(.Cells("Column7").Value)

                ElseIf _curCol = 6 Then
                    .Cells("Column8").Value = Val(_txtCell.Text) _
                                            + Val(.Cells("Column6").Value)
                End If
            End With
        Catch ex As Exception

        Finally

        End Try
    End Sub

    Private Sub dgvPhHlth_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvPhHlth.CellBeginEdit
        e_rindx = e.RowIndex
        e_cindx = e.ColumnIndex
        If e_rindx < PhHlth_rcount Then
            _cellVal = dgvPhHlth.Item(e_cindx, e_rindx).Value
        End If
    End Sub

    Private Sub dgvPhHlth_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPhHlth.CellEndEdit
        If e_rindx < PhHlth_rcount Then
            If _cellVal = dgvPhHlth.Item(e_cindx, e_rindx).Value Then
            Else
                _editRowID.Add(dgvPhHlth.Item("Column1", e_rindx).Value & "@" & dgvPhHlth.CurrentRow.Index)
            End If
        End If

        If dgvTxtBx IsNot Nothing Then
            RemoveHandler dgvTxtBx.KeyDown, AddressOf dgvPhHlth_KeyDown
        End If
    End Sub

    Private Sub dgvPhHlth_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPhHlth.CellContentClick

    End Sub
    Dim dgvTxtBx As New TextBox
    Private Sub dgvPhHlth_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) 'Handles dgvPhHlth.EditingControlShowing

        e.Control.ContextMenu = New ContextMenu

        Static r_indx, c_indx As Integer
        r_indx = -1
        c_indx = -1
        dgvTxtBx = DirectCast(e.Control, TextBox)
        With dgvTxtBx
            Try
                If r_indx <> dgvPhHlth.CurrentRow.Index And c_indx <> dgvPhHlth.CurrentCell.ColumnIndex Then
                    r_indx = dgvPhHlth.CurrentRow.Index
                    c_indx = dgvPhHlth.CurrentCell.ColumnIndex
                    _curCol = c_indx
                    RemoveHandler .TextChanged, AddressOf dgvPhHlth_TextChanged
                    RemoveHandler .KeyPress, AddressOf dgvPhHlth_CellKeyPress
                    RemoveHandler .KeyDown, AddressOf dgvPhHlth_KeyDown
                Else
                End If


                AddHandler .TextChanged, AddressOf dgvPhHlth_TextChanged
                AddHandler .KeyPress, AddressOf dgvPhHlth_CellKeyPress
                AddHandler .KeyDown, AddressOf dgvPhHlth_KeyDown

            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            Finally

            End Try
        End With
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click

        dgvPhHlth.EndEdit(True)

        Static rIndx, cIndx As Integer

        If dontUpdate = 1 Then
            _editRowID.Clear()
        End If

        If dgvPhHlth.RowCount >= 2 Then

            rIndx = dgvPhHlth.CurrentRow.Index
            cIndx = dgvPhHlth.CurrentCell.ColumnIndex

            Dim _rID, _indx As String

            For Each rID In _editRowID
                _rID = getStrBetween(rID, "", "@")
                _indx = StrReverse(getStrBetween(StrReverse(rID), "", "@"))
                With dgvPhHlth.Rows(Val(_indx))
                    EXECQUER("UPDATE payphilhealth SET " & _
                    "SalaryBracket=" & Val(.Cells("Column2").Value) & _
                    ",SalaryRangeFrom=" & Val(.Cells("Column3").Value) & _
                    ",SalaryRangeTo=" & Val(.Cells("Column4").Value) & _
                    ",SalaryBase=" & Val(.Cells("Column5").Value) & _
                    ",EmployeeShare=" & Val(.Cells("Column6").Value) & _
                    ",EmployerShare=" & Val(.Cells("Column7").Value) & _
                    ",LastUpd=CURRENT_TIMESTAMP()" & _
                    ",LastUpdBy=" & z_User & _
                    " WHERE RowID='" & _rID & "'")

                    .Cells("Column11").Value = _now
                    .Cells("Column12").Value = u_nem

                    .Cells("Column3").Value = FormatNumber(Val(.Cells("Column3").Value), 2).Replace(",", "")
                    .Cells("Column4").Value = FormatNumber(Val(.Cells("Column4").Value), 2).Replace(",", "")
                    .Cells("Column5").Value = FormatNumber(.Cells("Column5").Value, 2).Replace(",", "")
                    .Cells("Column6").Value = FormatNumber(.Cells("Column6").Value, 2).Replace(",", "")
                    .Cells("Column7").Value = FormatNumber(.Cells("Column7").Value, 2).Replace(",", "")
                End With
            Next


            If dontUpdate = 1 Then

            Else
                InfoBalloon(, , lblforballoon, , , 1)
                InfoBalloon("PhilHealth Contribution were successfully updated.", "Update Successful", lblforballoon, lblforballoon.Width - 16, -69)
                _editRowID.Clear()
            End If

            If dontCreate = 0 Then

                For Each drow As DataGridViewRow In dgvPhHlth.Rows
                    With drow
                        If .Cells("Column1").Value = Nothing And .IsNewRow = False Then
                            Dim _RowID = INS_payphilhealth(Val(.Cells("Column2").Value), _
                                                           Val(.Cells("Column3").Value),
                                                           Val(.Cells("Column4").Value), _
                                                           Val(.Cells("Column5").Value), _
                                                           , _
                                                           Val(.Cells("Column6").Value), _
                                                           Val(.Cells("Column7").Value))

                            .Cells("Column9").Value = _now
                            .Cells("Column1").Value = _RowID
                            .Cells("Column10").Value = u_nem

                            .Cells("Column3").Value = FormatNumber(Val(.Cells("Column3").Value), 2).Replace(",", "")
                            .Cells("Column4").Value = FormatNumber(Val(.Cells("Column4").Value), 2).Replace(",", "")
                            .Cells("Column5").Value = FormatNumber(.Cells("Column5").Value, 2).Replace(",", "")
                            .Cells("Column6").Value = FormatNumber(.Cells("Column6").Value, 2).Replace(",", "")
                            .Cells("Column7").Value = FormatNumber(.Cells("Column7").Value, 2).Replace(",", "")

                            InfoBalloon(, , lblforballoon, , , 1)
                            InfoBalloon("PhilHealth Contribution were successfully created.", "Created PhilHealth Contribution Successful", lblforballoon, lblforballoon.Width - 16, -69)
                        End If
                    End With
                Next

            End If

            For i = PhHlth_rcount To dgvPhHlth.RowCount - 2
                With dgvPhHlth.Rows(i)

                End With
            Next

            PhHlth_rcount = EXECQUER("SELECT COUNT(RowID) FROM payphilhealth")

            dgvPhHlth.Item(cIndx, rIndx).Selected = True

        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub dgvPhHlth_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgvPhHlth.CurrentCellChanged
        If dgvTxtBx IsNot Nothing Then
            RemoveHandler dgvTxtBx.TextChanged, AddressOf dgvPhHlth_TextChanged
            RemoveHandler dgvTxtBx.KeyDown, AddressOf dgvPhHlth_KeyDown
        End If
    End Sub

    Private Sub dgvPhHlth_KeyDown(sender As Object, e As KeyEventArgs) 'Handles dgvPhHlth.KeyDown
        If (e.Control AndAlso Keys.S) Then
            Button1_Click(sender, e)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Dim result = MessageBox.Show("Do you want to Delete this Item ?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)
        If result = DialogResult.No Then
            Exit Sub
        ElseIf result = DialogResult.Yes Then
            If dgvPhHlth.RowCount <> 0 Then
                dgvPhHlth.EndEdit(True)
                With dgvPhHlth.CurrentRow
                    If .Cells("Column1").Value <> "" Then
                        
                        INS_audittrail("SalaryRangeFrom", _
                                       .Cells("Column1").Value, _
                                       .Cells("Column3").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("SalaryRangeTo", _
                                       .Cells("Column1").Value, _
                                       .Cells("Column4").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("SalaryBase", _
                                       .Cells("Column1").Value, _
                                       .Cells("Column5").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("TotalMonthlyPremium", _
                                       .Cells("Column1").Value, _
                                       .Cells("Column8").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("EmployeeShare", _
                                       .Cells("Column1").Value, _
                                       .Cells("Column6").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("EmployerShare", _
                                       .Cells("Column1").Value, _
                                       .Cells("Column7").Value, _
                                       "", _
                                       "Delete")

                        EXECQUER("DELETE FROM payphilhealth WHERE RowID=" & .Cells("Column1").Value)

                    End If
                    dgvPhHlth.Rows.Remove(dgvPhHlth.CurrentRow)
                End With
            End If
        End If
    End Sub
    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Me.Close()
    End Sub

    Private Sub dgvPhHlth_KeyDown1(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Delete Then
            If dgvPhHlth.CurrentRow.IsNewRow = False Then
                e.Handled = If(dgvPhHlth.CurrentRow.Index >= PhHlth_rcount, False, True)
                If e.Handled = False Then
                    dgvPhHlth.Rows.Remove(dgvPhHlth.CurrentRow)
                End If
            End If
        End If
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        _editRowID.Clear()

        loadPhiHealth()

    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Dim dt_importcatcher As New DataTable

    Dim filepath As String = Nothing '= IO.Path.GetFullPath(browsefile.FileName) 'browsefile.FileName

    Private Sub tsbtnPhilHealthImport_Click(sender As Object, e As EventArgs) Handles tsbtnPhilHealthImport.Click

        'For Each c As DataGridViewColumn In dgvPhHlth.Columns
        '    IO.File.AppendAllText(IO.Path.GetTempPath() & "dgvPhHlth.txt", c.Name & "@" & c.HeaderText & "&" & c.Visible.ToString & Environment.NewLine)
        'Next

        'Static once As SByte = 0

        'If once = 0 Then
        '    Exit Sub

        'End If


        Dim browsefile As OpenFileDialog = New OpenFileDialog()

        browsefile.Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" & _
                                  "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"

        If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

            filepath = IO.Path.GetFullPath(browsefile.FileName)

            ToolStripProgressBar1.Visible = True

            ToolStrip1.Enabled = False

            bgworkImportSSS.RunWorkerAsync()

        End If

    End Sub

    Function INSUPD_payphilhealth(Optional phh_RowID = Nothing, _
                                      Optional phh_SalaryRangeFrom = Nothing, _
                                      Optional phh_SalaryRangeTo = Nothing, _
                                      Optional phh_SalaryBase = Nothing, _
                                      Optional phh_EmployeeShare = Nothing, _
                                      Optional phh_EmployerShare = Nothing, _
                                      Optional phh_TotalMonthlyPremium = Nothing, _
                                      Optional phh_SalaryBracket = Nothing) As Object

        Dim returnval = Nothing

        Dim params(9, 2) As Object

        params(0, 0) = "phh_RowID"
        params(1, 0) = "phh_CreatedBy"
        params(2, 0) = "phh_LastUpdBy"
        params(3, 0) = "phh_SalaryRangeFrom"
        params(4, 0) = "phh_SalaryRangeTo"
        params(5, 0) = "phh_SalaryBase"
        params(6, 0) = "phh_TotalMonthlyPremium"
        params(7, 0) = "phh_EmployeeShare"
        params(8, 0) = "phh_EmployerShare"
        params(9, 0) = "phh_SalaryBracket"

        params(0, 1) = If(phh_RowID = Nothing, DBNull.Value, phh_RowID)
        params(1, 1) = z_User
        params(2, 1) = z_User
        params(3, 1) = Val(phh_SalaryRangeFrom)
        params(4, 1) = Val(phh_SalaryRangeTo)
        params(5, 1) = Val(phh_SalaryBase)

        params(6, 1) = If(Val(phh_TotalMonthlyPremium) = 0, _
                          Val(phh_EmployeeShare) + Val(phh_EmployerShare), _
                          Val(phh_TotalMonthlyPremium)) 'phh_TotalMonthlyPremium

        params(7, 1) = Val(phh_EmployeeShare)
        params(8, 1) = Val(phh_EmployerShare)
        params(9, 1) = Val(phh_SalaryBracket)

        returnval = _
        EXEC_INSUPD_PROCEDURE(params, _
                               "INSUPD_payphilhealth", _
                               "returnval")

        Return returnval

    End Function

    Function ExcelToCVS(ByVal opfiledir As String) As Object

        Dim StrConn As String
        Dim DA As New OleDbDataAdapter
        Dim DS As New DataSet
        Dim Str As String = Nothing
        Dim ColumnCount As Integer = 0
        Dim OuterCount As Integer = 0
        Dim InnerCount As Integer = 0
        Dim RowCount As Integer = 0

        Dim opfile As New OpenFileDialog

        '                          "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" & _
        '                          "Microsoft Excel Documents 97-2003 (*.xls)|*.xls|" & _
        '                          "OpenDocument Spreadsheet (*.ods)|*.ods"

        opfile.Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" & _
                                  "Microsoft Excel Documents 97-2003 (*.xls)|*.xls|" & _
                                  "OpenDocument Spreadsheet (*.ods)|*.ods"

        'D_CanteenDed("Delete All", z_divno, Z_YYYY, Z_PayNo, Z_PayrollType, Z_YYYYMM, 0, 0)

        'opfiledir = opfile.FileName

        'Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;";
        'StrConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & opfile.FileName & ";Extended Properties=Excel 8.0;"
        StrConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & opfiledir & ";Extended Properties=Excel 12.0;"

        'Dim cnString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source=" & Application.StartupPath & "\dat.mdb"

        Dim objConn As New OleDbConnection(StrConn)

        Try
            objConn.Open()

            If objConn.State = ConnectionState.Closed Then

                Console.Write("Connection cannot be opened")

            Else

                Console.Write("Welcome")

            End If

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))

        End Try

        Dim objCmd As New OleDbCommand("Select * from [Sheet1$]", objConn)

        objCmd.CommandType = CommandType.Text


        Dim Count As Integer

        Count = 0

        Try
            DA.SelectCommand = objCmd

            DA.Fill(DS, "XLData")

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))

        End Try

        Dim returnvalue = Nothing

        Try

            'RowCount = DS.Tables(0).Rows.Count

            'ColumnCount = DS.Tables(0).Columns.Count

            'For OuterCount = 0 To RowCount - 1

            '    Str = ""

            '    For InnerCount = 0 To ColumnCount - 1

            '        Str &= DS.Tables(0).Rows(OuterCount).Item(InnerCount) & ","

            '    Next

            '    arrlist.Add(Str)

            'Next

            returnvalue = DS.Tables(0)

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))

            returnvalue = Nothing

        Finally

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
            objConn.Dispose()
            objConn = Nothing

        End Try

        Return returnvalue

    End Function

    Sub INS_audittrail(Optional au_FieldChanged = Nothing, _
                       Optional au_ChangedRowID = Nothing, _
                       Optional au_OldValue = Nothing, _
                       Optional au_NewValue = Nothing, _
                       Optional au_ActionPerformed = Nothing)

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("INS_audittrail", conn)

            conn.Open()

            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                '.Parameters.Add(returnName, MySql_DbType)

                .Parameters.AddWithValue("au_CreatedBy", z_User)

                .Parameters.AddWithValue("au_LastUpdBy", z_User)

                .Parameters.AddWithValue("au_OrganizationID", orgztnID)

                .Parameters.AddWithValue("au_ViewID", view_ID)

                .Parameters.AddWithValue("au_FieldChanged", Trim(au_FieldChanged))

                .Parameters.AddWithValue("au_ChangedRowID", au_ChangedRowID)

                .Parameters.AddWithValue("au_OldValue", Trim(au_OldValue))

                .Parameters.AddWithValue("au_NewValue", Trim(au_NewValue))

                .Parameters.AddWithValue("au_ActionPerformed", Trim(au_ActionPerformed))

                '.Parameters(returnName).Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

            End With

        Catch ex As Exception
            MsgBox(ex.Message & " " & "INS_audittrail", , "Error")
        Finally
            conn.Close()
            cmd.Dispose()

        End Try

    End Sub

    Private Sub bgworkImportSSS_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkImportSSS.DoWork

        backgroundworking = 1

        Dim bgworkindx = 0

        'For Each dgvrow As DataGridViewRow In dgvPhHlth.Rows

        '    'SELECT  `RowID`,  `Created`,  `LastUpd`,  `CreatedBy`,  `LastUpdBy`,  `SalaryBracket`,  `SalaryRangeFrom`,  `SalaryRangeTo`,  `SalaryBase`
        '    ',  `TotalMonthlyPremium`,  `EmployeeShare`,  `EmployerShare` FROM `payroll`.`payphilhealth`

        '    If dgvrow.IsNewRow = False Then

        '        INS_audittrail("SalaryRangeFrom", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column3").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("SalaryRangeTo", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column4").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("SalaryBase", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column5").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("TotalMonthlyPremium", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column8").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("EmployeeShare", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column6").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("EmployerShare", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column7").Value, _
        '                       "", _
        '                       "Delete")

        '    End If

        '    bgworkImportSSS.ReportProgress(CInt(50 * (bgworkindx / dgvPhHlth.RowCount)), "")

        '    bgworkindx += 1

        'Next

        'Dim maxsssrowid = EXECQUER("SELECT MAX(RowID) FROM payphilhealth;")

        'EXECQUER("DELETE FROM payphilhealth;" & _
        '         "ALTER TABLE payphilhealth AUTO_INCREMENT = " & Val(maxsssrowid) & ";")

        Dim arrayrow As New ArrayList

        dt_importcatcher = _
        ExcelToCVS(filepath) 'filepath

        Dim rowindx As Integer = 0

        If dt_importcatcher Is Nothing Then

        Else

            Dim lastbound = dt_importcatcher.Rows.Count 'dgvPhHlth.RowCount +

            For Each drow As DataRow In dt_importcatcher.Rows

                'For Each dcol As DataColumn In dt_importcatcher.Columns

                '    Dim dataval = drow(dcol.ColumnName)

                '    MsgBox(dataval.ToString)

                INSUPD_payphilhealth(, _
                                        Val(drow(1)), _
                                        Val(drow(2)), _
                                        Val(drow(3)), _
                                        Val(drow(5)), _
                                        Val(drow(4)),
                                        Val(drow(6)), _
                                        Val(drow(0))) 'Val(drow(5))

                'Next

                Dim progressvalue = CInt(100 * bgworkindx / lastbound)

                bgworkImportSSS.ReportProgress(progressvalue, "")

                bgworkindx += 1

            Next

            'If Val(sssrowid) = 0 Then
            'EXECQUER("UPDATE payphilhealth SET RowID=1 WHERE RowID='0';")
            'End If

            bgworkImportSSS.ReportProgress(100, "")

        End If


    End Sub

    Private Sub bgworkImportSSS_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkImportSSS.ProgressChanged

        ToolStripProgressBar1.Value = CType(e.ProgressPercentage, Integer)

    End Sub

    Private Sub bgworkImportSSS_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkImportSSS.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Background work cancelled.")
        Else
            'MessageBox.Show("Background work finish successfully.")
        End If

        'If dt_importcatcher Is Nothing Then

        'Else

        'End If

        loadPhiHealth()

        ToolStrip1.Enabled = True

        ToolStripProgressBar1.Visible = False

        ToolStripProgressBar1.Value = 0

        backgroundworking = 0

    End Sub

End Class