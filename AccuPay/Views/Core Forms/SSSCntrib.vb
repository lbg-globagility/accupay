Imports System.Data.OleDb
Imports MySql.Data.MySqlClient

'Imports Microsoft.Office.Interop

Public Class SSSCntrib
    Public q_paysocialsecurity As String = "SELECT sss.RowID," &
    "COALESCE(sss.RangeFromAmount,0) 'Range of Compensation'," &
    "COALESCE(sss.RangeToAmount,0)," &
    "COALESCE(sss.MonthlySalaryCredit,0) 'Monthly Salary Credit'," &
    "COALESCE(sss.EmployerContributionAmount,0) 'Employer Contribution Amount'," &
    "COALESCE(sss.EmployeeContributionAmount,0) 'Employee Contribution Amount'," &
    "COALESCE(sss.EmployeeECAmount,0) 'EC\/ER Amount'," &
    "COALESCE(sss.EmployerContributionAmount,0) + COALESCE(sss.EmployeeECAmount,0) 'Employer Total Contribution'," &
    "COALESCE(sss.EmployeeContributionAmount,0) 'Employee Total Contribution'," &
    "COALESCE(sss.EmployerContributionAmount,0) + COALESCE(sss.EmployeeContributionAmount,0) + COALESCE(sss.EmployeeECAmount,0) 'EC\/ER Total'," &
    "DATE_FORMAT(sss.Created,'%m-%d-%Y') 'Creation Date'," &
    "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by'," &
    "COALESCE(DATE_FORMAT(sss.LastUpd,'%m-%d-%Y'),'') 'Last Update'," &
    "(SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM user WHERE RowID=sss.LastUpdBy) 'LastUpdate by' " &
    "FROM paysocialsecurity sss " &
    "INNER JOIN user u ON sss.CreatedBy=u.RowID" &
    " WHERE sss.MonthlySalaryCredit!=0" &
    " AND sss.HiddenData='0'" &
    " AND EffectiveDateFrom='2019-04-01' AND EffectiveDateTo='2022-12-01'"

    Dim _editRowID As New List(Of String)
    Dim e_rindx, e_cindx, charcnt, SS_rcount As Integer
    Dim _cellVal, _now, u_nem As String
    Sub loadSSSCntrib()
        dgvPaySSS.Rows.Clear()
        For Each r As DataRow In retAsDatTbl(q_paysocialsecurity & " ORDER BY sss.MonthlySalaryCredit").Rows 'ORDER BY sss.MonthlySalaryCredit
            dgvPaySSS.Rows.Add(r(0), r(1), r(2), r(3), r(4), r(5), r(6), r(7), r(8), r(9), r(10), r(11), r(12))
        Next
        SS_rcount = dgvPaySSS.RowCount - 1
    End Sub

    Private Sub SSSCntrib_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        InfoBalloon(, , lblforballoon, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        'If FormLeft.Contains("SSS Contribution Table") Then
        '    FormLeft.Remove("SSS Contribution Table")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.Text = "Welcome"
        'Else
        '    MDIPrimaryForm.Text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

        showAuditTrail.Close()

        GeneralForm.listGeneralForm.Remove(Me.Name)
    End Sub

    Dim view_ID As Integer = Nothing

    Dim dontUpdate As SByte = 0

    Dim dontCreate As SByte = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        'view_ID = VIEW_privilege(Me.Text, orgztnID)

        loadSSSCntrib()

        AddHandler dgvPaySSS.EditingControlShowing, AddressOf dgvPaySSS_EditingControlShowing

        _now = EXECQUER(CURDATE_MDY)

        u_nem = EXECQUER(USERNameStrPropr & 1)

        view_ID = VIEW_privilege("SSS Contribution Table", orgztnID)

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
    Private Sub dgvPaySSS_CellKeyPress(sender As Object, e As KeyPressEventArgs)
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
    Private Sub dgvPaySSS_TextChanged(sender As Object, e As EventArgs)
        _txtCell = DirectCast(sender, TextBox)
        Try
            With dgvPaySSS.CurrentRow
                If _curCol = 4 Then
                    .Cells("Column7").Value = Val(_txtCell.Text) _
                                            + Val(.Cells("Column6").Value)

                    .Cells("Column9").Value = Val(_txtCell.Text) _
                                            + Val(.Cells("Column5").Value) _
                                            + Val(.Cells("Column6").Value)
                ElseIf _curCol = 5 Then
                    .Cells("Column8").Value = Val(_txtCell.Text)

                    .Cells("Column9").Value = Val(.Cells("Column4").Value) _
                                            + Val(_txtCell.Text) _
                                            + Val(.Cells("Column6").Value)
                ElseIf _curCol = 6 Then
                    .Cells("Column7").Value = Val(_txtCell.Text) _
                                            + Val(.Cells("Column4").Value)

                    .Cells("Column9").Value = Val(.Cells("Column4").Value) _
                                            + Val(.Cells("Column5").Value) _
                                            + Val(_txtCell.Text)
                End If
            End With
        Catch ex As Exception

        Finally

        End Try
    End Sub

    Private Sub dgvPaySSS_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvPaySSS.CellBeginEdit
        e_rindx = e.RowIndex
        e_cindx = e.ColumnIndex
        If e_rindx < SS_rcount Then
            _cellVal = dgvPaySSS.Item(e_cindx, e_rindx).Value
        End If
    End Sub

    Private Sub dgvPaySSS_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPaySSS.CellEndEdit

        If e_rindx < SS_rcount Then
            If _cellVal = dgvPaySSS.Item(e_cindx, e_rindx).Value Then
            Else
                _editRowID.Add(dgvPaySSS.Item("Column1", e_rindx).Value & "@" & dgvPaySSS.CurrentRow.Index)
            End If
        End If

        If dgvTxtBx IsNot Nothing Then
            RemoveHandler dgvTxtBx.KeyDown, AddressOf dgvPaySSS_KeyDown
        End If
    End Sub

    Private Sub dgvPaySSS_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPaySSS.CellContentClick

    End Sub

    Private Sub dgvPaySSS_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) 'Handles dgvPaySSS.CellValueChanged

    End Sub
    Dim _curCol As Integer
    Dim dgvTxtBx As New TextBox
    Private Sub dgvPaySSS_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) 'Handles dgvPaySSS.EditingControlShowing

        e.Control.ContextMenu = New ContextMenu

        Static r_indx, c_indx As Integer
        r_indx = -1
        c_indx = -1
        dgvTxtBx = DirectCast(e.Control, TextBox)
        With dgvTxtBx
            Try
                If r_indx <> dgvPaySSS.CurrentRow.Index And c_indx <> dgvPaySSS.CurrentCell.ColumnIndex Then
                    r_indx = dgvPaySSS.CurrentRow.Index
                    c_indx = dgvPaySSS.CurrentCell.ColumnIndex
                    _curCol = c_indx
                    RemoveHandler .TextChanged, AddressOf dgvPaySSS_TextChanged
                    RemoveHandler .KeyPress, AddressOf dgvPaySSS_CellKeyPress
                    RemoveHandler .KeyDown, AddressOf dgvPaySSS_KeyDown
                Else
                End If

                AddHandler .TextChanged, AddressOf dgvPaySSS_TextChanged
                AddHandler .KeyPress, AddressOf dgvPaySSS_CellKeyPress

                AddHandler .KeyDown, AddressOf dgvPaySSS_KeyDown
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            Finally
            End Try
        End With
    End Sub

    Sub SaveSSS_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click

        dgvPaySSS.EndEdit(True)

        Static rIndx, cIndx As Integer

        If dontUpdate = 1 Then
            _editRowID.Clear()
        End If

        If dgvPaySSS.RowCount >= 2 Then

            rIndx = dgvPaySSS.CurrentRow.Index
            cIndx = dgvPaySSS.CurrentCell.ColumnIndex

            Dim _rID, _indx As String

            For Each rID In _editRowID

                _rID = getStrBetween(rID, "", "@")

                _indx = StrReverse(getStrBetween(StrReverse(rID), "", "@"))

                With dgvPaySSS.Rows(Val(_indx))

                    EXECQUER("UPDATE paysocialsecurity SET " & _
                    "RangeFromAmount=" & Val(.Cells("Column2").Value) & _
                    ",RangeToAmount=" & Val(.Cells("Column14").Value) & _
                    ",MonthlySalaryCredit=" & Val(.Cells("Column3").Value) & _
                    ",EmployeeContributionAmount=" & Val(.Cells("Column5").Value) & _
                    ",EmployerContributionAmount=" & Val(.Cells("Column4").Value) & _
                    ",EmployeeECAmount=" & Val(.Cells("Column6").Value) & _
                    ",LastUpd=CURRENT_TIMESTAMP()" & _
                    ",LastUpdBy=" & z_User & _
                    " WHERE RowID='" & _rID & "';")

                    .Cells("Column12").Value = _now
                    .Cells("Column13").Value = u_nem

                    .Cells("Column2").Value = FormatNumber(Val(.Cells("Column2").Value), 2).Replace(",", "")
                    'Dim rangeTo = If(Val(.Cells("Column14").Value) = 0, 1000000000, Val(.Cells("Column14").Value))
                    '                                       'rangeTo
                    .Cells("Column14").Value = FormatNumber(Val(.Cells("Column14").Value), 2).Replace(",", "")
                    .Cells("Column3").Value = FormatNumber(.Cells("Column3").Value, 2).Replace(",", "")
                    .Cells("Column4").Value = FormatNumber(.Cells("Column4").Value, 2).Replace(",", "")
                    .Cells("Column5").Value = FormatNumber(.Cells("Column5").Value, 2).Replace(",", "")
                    .Cells("Column6").Value = FormatNumber(.Cells("Column6").Value, 2).Replace(",", "")

                End With
            Next

            If dontUpdate = 1 Then

            Else
                InfoBalloon(, , lblforballoon, , , 1)
                'InfoBalloon("SSS Contribution were successfully updated.", "Update Successful", lblforballoon, lblforballoon.Width - 16, -69)
                _editRowID.Clear()
            End If

            If dontCreate = 0 Then

                For Each row As DataGridViewRow In dgvPaySSS.Rows
                    With row
                        If .Cells("Column1").Value = Nothing And .IsNewRow = False Then
                            Dim _RowID = INS_paysocialsecurity(Val(.Cells("Column2").Value), _
                                                              Val(.Cells("Column14").Value),
                                                              Val(.Cells("Column3").Value), _
                                                              Val(.Cells("Column5").Value), _
                                                              Val(.Cells("Column4").Value), _
                                                              Val(.Cells("Column6").Value))

                            .Cells("Column10").Value = _now
                            .Cells("Column1").Value = _RowID
                            .Cells("Column11").Value = u_nem

                            .Cells("Column2").Value = FormatNumber(Val(.Cells("Column2").Value), 2).Replace(",", "")
                            .Cells("Column14").Value = FormatNumber(Val(.Cells("Column14").Value), 2).Replace(",", "")
                            .Cells("Column3").Value = FormatNumber(.Cells("Column3").Value, 2).Replace(",", "")
                            .Cells("Column4").Value = FormatNumber(.Cells("Column4").Value, 2).Replace(",", "")
                            .Cells("Column5").Value = FormatNumber(.Cells("Column5").Value, 2).Replace(",", "")
                            .Cells("Column6").Value = FormatNumber(.Cells("Column6").Value, 2).Replace(",", "")

                            InfoBalloon(, , lblforballoon, , , 1)
                            InfoBalloon("Successfully created SSS contribution.", "Created SSS Contribution Successful", lblforballoon, lblforballoon.Width - 16, -69)
                        End If
                    End With
                Next

            End If

            'For i = SS_rcount To dgvPaySSS.RowCount - 2
            '    With dgvPaySSS.Rows(i)
            '        Dim _RowID = INS_paysocialsecurity(Val(.Cells("Column2").Value), _
            '                                          Val(.Cells("Column14").Value),
            '                                          Val(.Cells("Column3").Value), _
            '                                          Val(.Cells("Column5").Value), _
            '                                          Val(.Cells("Column4").Value), _
            '                                          Val(.Cells("Column6").Value))

            '        .Cells("Column10").Value = _now
            '        .Cells("Column1").Value = _RowID
            '        .Cells("Column11").Value = u_nem

            '        .Cells("Column2").Value = FormatNumber(Val(.Cells("Column2").Value), 2).Replace(",", "")
            '        .Cells("Column14").Value = FormatNumber(Val(.Cells("Column14").Value), 2).Replace(",", "")
            '        .Cells("Column3").Value = FormatNumber(.Cells("Column3").Value, 2).Replace(",", "")
            '        .Cells("Column4").Value = FormatNumber(.Cells("Column4").Value, 2).Replace(",", "")
            '        .Cells("Column5").Value = FormatNumber(.Cells("Column5").Value, 2).Replace(",", "")
            '        .Cells("Column6").Value = FormatNumber(.Cells("Column6").Value, 2).Replace(",", "")
            '    End With

            'Next

            SS_rcount = EXECQUER("SELECT COUNT(RowID) FROM paysocialsecurity")

            dgvPaySSS.Item(cIndx, rIndx).Selected = True

            Dim min_range = _
                EXECQUER("SELECT MIN(RangeFromAmount) FROM paysocialsecurity;")

            min_range = ValNoComma(min_range)

            EXECQUER("INSERT INTO paysocialsecurity (Created,CreatedBy,LastUpdBy,RangeFromAmount,RangeToAmount,MonthlySalaryCredit,EmployeeContributionAmount,EmployerContributionAmount,EmployeeECAmount) VALUES " & _
                                                "(CURRENT_TIMESTAMP(),'" & z_User & "','" & z_User & "',0,(" & min_range & " - 1),0,0,0,0) ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP(),LastUpdBy='" & z_User & "',RangeFromAmount=0,RangeToAmount=(" & min_range & " - 1),MonthlySalaryCredit=0,EmployeeContributionAmount=0,EmployerContributionAmount=0,EmployeeECAmount=0;")

        End If

    End Sub

    Private Sub dgvPaySSS_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgvPaySSS.CurrentCellChanged
        If dgvTxtBx IsNot Nothing Then
            RemoveHandler dgvTxtBx.TextChanged, AddressOf dgvPaySSS_TextChanged
            RemoveHandler dgvTxtBx.KeyDown, AddressOf dgvPaySSS_KeyDown

        End If
        'RemoveHandler dgvTxtBx.KeyPress, AddressOf dgvPaySSS_CellKeyPress
    End Sub

    Private Sub dgvPaySSS_KeyDown(sender As Object, e As KeyEventArgs) ' Handles dgvPaySSS.KeyDown
        If (e.Control AndAlso Keys.S) Then
            'SaveSSS_Click(sender, e)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Sub DeleteSSS_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click

        Dim result = MessageBox.Show("Do you want to Delete this Item ?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)
        If result = DialogResult.No Then
            Exit Sub
        ElseIf result = DialogResult.Yes Then
            If dgvPaySSS.RowCount <> 0 Then
                dgvPaySSS.EndEdit(True)
                With dgvPaySSS.CurrentRow
                    If .Cells("Column1").Value <> "" Then

                        INS_audittrail("RangeFromAmount", _
                                        .Cells("Column1").Value, _
                                        .Cells("Column2").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("RangeToAmount", _
                                        .Cells("Column1").Value, _
                                        .Cells("Column14").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("MonthlySalaryCredit", _
                                        .Cells("Column1").Value, _
                                        .Cells("Column3").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("EmployeeContributionAmount", _
                                        .Cells("Column1").Value, _
                                        .Cells("Column5").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("EmployerContributionAmount", _
                                       .Cells("Column1").Value, _
                                       .Cells("Column4").Value, _
                                       "", _
                                       "Delete")

                        INS_audittrail("EmployeeECAmount", _
                                       .Cells("Column1").Value, _
                                       .Cells("Column6").Value, _
                                       "", _
                                       "Delete")


                        EXECQUER("DELETE FROM paysocialsecurity WHERE RowID=" & .Cells("Column1").Value)

                    End If
                    dgvPaySSS.Rows.Remove(dgvPaySSS.CurrentRow)
                End With
            End If
        End If
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Me.Close()
    End Sub

    Private Sub dgvPaySSS_KeyDown1(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Delete Then
            If dgvPaySSS.CurrentRow.IsNewRow = False Then
                e.Handled = If(dgvPaySSS.CurrentRow.Index >= SS_rcount, False, True)

                If e.Handled = False Then
                    dgvPaySSS.Rows.Remove(dgvPaySSS.CurrentRow)
                End If
            End If
        End If
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        _editRowID.Clear()

        loadSSSCntrib()

    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click

        'Dim promptAuditTrail As New showAuditTrail

        'promptAuditTrail.ShowDialog()

        'promptAuditTrail.loadAudTrail(view_ID)


        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Dim dt_importcatcher As New DataTable

    Dim filepath As String = Nothing

    Private Sub tsbtnSSSImport_Click(sender As Object, e As EventArgs) Handles tsbtnSSSImport.Click

        'For Each c As DataGridViewColumn In dgvPaySSS.Columns
        '    IO.File.AppendAllText(IO.Path.GetTempPath() & "dgvPaySSS.txt", c.Name & "@" & c.HeaderText & "&" & c.Visible.ToString & Environment.NewLine)
        'Next

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

    Function INSUPD_paysocialsecurity(Optional sss_RowID = Nothing, _
                                      Optional sss_RangeFromAmount = Nothing, _
                                      Optional sss_RangeToAmount = Nothing, _
                                      Optional sss_MonthlySalaryCredit = Nothing, _
                                      Optional sss_EmployeeContributionAmount = Nothing, _
                                      Optional sss_EmployerContributionAmount = Nothing, _
                                      Optional sss_EmployeeECAmount = Nothing) As Object

        Dim returnval = Nothing

        Dim params(8, 2) As Object

        params(0, 0) = "sss_RowID"
        params(1, 0) = "sss_CreatedBy"
        params(2, 0) = "sss_LastUpdBy"
        params(3, 0) = "sss_RangeFromAmount"
        params(4, 0) = "sss_RangeToAmount"
        params(5, 0) = "sss_MonthlySalaryCredit"
        params(6, 0) = "sss_EmployeeContributionAmount"
        params(7, 0) = "sss_EmployerContributionAmount"
        params(8, 0) = "sss_EmployeeECAmount"

        params(0, 1) = If(sss_RowID = Nothing, DBNull.Value, sss_RowID)
        params(1, 1) = z_User
        params(2, 1) = z_User
        params(3, 1) = Val(sss_RangeFromAmount)
        params(4, 1) = Val(sss_RangeToAmount)
        params(5, 1) = Val(sss_MonthlySalaryCredit)
        params(6, 1) = Val(sss_EmployeeContributionAmount)
        params(7, 1) = Val(sss_EmployerContributionAmount)
        params(8, 1) = Val(sss_EmployeeECAmount)

        returnval = _
        EXEC_INSUPD_PROCEDURE(params, _
                               "INSUPD_paysocialsecurity", _
                               "returnval")

        Return returnval

    End Function

    Function ExcelToCVS(ByVal opfiledir As String, _
                        ByVal arrlist As ArrayList) As Object

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

        'For Each dgvrow As DataGridViewRow In dgvPaySSS.Rows

        '    'SELECT RowID,Created,CreatedBy,LastUpd,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed audittrail;

        '    'SELECT  `Created`,  `RowID`,  `CreatedBy`,  `LastUpd`,  `LastUpdBy`,  `RangeFromAmount`,  `RangeToAmount`,  `MonthlySalaryCredit`
        '    ',  `EmployeeContributionAmount`,  `EmployerContributionAmount`,  `EmployeeECAmount` FROM `globaldb`.`paysocialsecurity`

        '    If dgvrow.IsNewRow = False Then

        '        INS_audittrail("RangeFromAmount", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column2").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("RangeToAmount", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column14").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("MonthlySalaryCredit", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column3").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("EmployeeContributionAmount", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column5").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("EmployerContributionAmount", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column4").Value, _
        '                       "", _
        '                       "Delete")

        '        INS_audittrail("EmployeeECAmount", _
        '                       dgvrow.Cells("Column1").Value, _
        '                       dgvrow.Cells("Column6").Value, _
        '                       "", _
        '                       "Delete")

        '    End If

        '    bgworkImportSSS.ReportProgress(CInt(50 * (bgworkindx / dgvPaySSS.RowCount)), "")

        '    bgworkindx += 1

        'Next

        'dgvPaySSS.Rows.Clear()

        'bgworkindx = 0

        'Dim maxsssrowid = EXECQUER("SELECT MAX(RowID) FROM paysocialsecurity;")

        'EXECQUER("DELETE FROM paysocialsecurity;" & _
        '         "ALTER TABLE paysocialsecurity AUTO_INCREMENT = " & Val(maxsssrowid) & ";")

        Dim arrayrow As New ArrayList

        dt_importcatcher = _
        ExcelToCVS(filepath, arrayrow)

        Dim rowindx As Integer = 0

        If dt_importcatcher Is Nothing Then

        Else

            Dim lastbound = dt_importcatcher.Rows.Count 'dgvPaySSS.RowCount + 

            For Each drow As DataRow In dt_importcatcher.Rows

                'For Each dcol As DataColumn In dt_importcatcher.Columns

                '    Dim dataval = drow(dcol.ColumnName)

                '    MsgBox(dataval.ToString)

                Dim sssrowid = _
                INSUPD_paysocialsecurity(, _
                                        Val(drow(0)), _
                                        Val(drow(1)), _
                                        Val(drow(2)), _
                                        Val(drow(4)), _
                                        Val(drow(3)), _
                                        Val(drow(5)))

                'Next

                Dim progressvalue = CInt(100 * bgworkindx / lastbound)

                bgworkImportSSS.ReportProgress(progressvalue, "")

                bgworkindx += 1

            Next

            'If Val(sssrowid) = 0 Then
            'EXECQUER("UPDATE paysocialsecurity SET RowID=1 WHERE RowID='0';")
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

        loadSSSCntrib()

        backgroundworking = 0

        ToolStrip1.Enabled = True

        ToolStripProgressBar1.Visible = False

        ToolStripProgressBar1.Value = 0

    End Sub

End Class