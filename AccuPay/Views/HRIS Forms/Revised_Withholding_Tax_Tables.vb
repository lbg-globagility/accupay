Imports System.Data.OleDb

Public Class Revised_Withholding_Tax_Tables
    Dim getpf As Integer

    Private Sub fillpaytype()
        Dim strQuery2 As String = "select PayFrequencyType from payfrequency"
        cmbPayType.Items.Clear()
        cmbPayType.Items.Add("")
        cmbPayType.Items.AddRange(CType(SQL_ArrayList(strQuery2).ToArray(GetType(String)), String()))
        cmbPayType.SelectedIndex = 0
    End Sub
    Private Sub fillFilingStatus()
        Dim dt As New DataTable
        dt = getDataTableForSQL("select * from filingstatus")

        dgvfilingstatus.Rows.Clear()

        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvfilingstatus.Rows.Add()
            With drow
                dgvfilingstatus.Rows.Item(n).Cells(c_FilingStatus.Index).Value = .Item("FilingStatus").ToString
                dgvfilingstatus.Rows.Item(n).Cells(c_maritalStatus.Index).Value = .Item("MaritalStatus").ToString
                dgvfilingstatus.Rows.Item(n).Cells(c_Dependent.Index).Value = .Item("Dependent").ToString
                dgvfilingstatus.Rows.Item(n).Cells(c_FID.Index).Value = .Item("RowID").ToString
            End With
        Next
    End Sub

    Private Sub selectfilingstatus()
        If dgvfilingstatus.RowCount <> 0 Then

            Dim dt As New DataTable
            dt = getDataTableForSQL("select * from paywithholdingtax where filingstatusid = '" & dgvfilingstatus.CurrentRow.Cells(c_FID.Index).Value & "' and payfrequencyid = '" & getpf & "'")
            dgvlisttaxableamt.Rows.Clear()

            For Each drow As DataRow In dt.Rows
                Dim n As Integer = dgvlisttaxableamt.Rows.Add()
                With drow
                    dgvlisttaxableamt.Rows.Item(n).Cells(c_ExemptionAmount.Index).Value = FormatNumber(.Item("ExemptionAmount"), 2)
                    dgvlisttaxableamt.Rows.Item(n).Cells(c_ExemptionExcessAmount.Index).Value = FormatPercent(.Item("ExemptioninExcessAmount"), 2, TriState.True)
                    dgvlisttaxableamt.Rows.Item(n).Cells(c_Taxincomeframt.Index).Value = FormatNumber(.Item("Taxableincomefromamount"), 2)
                    dgvlisttaxableamt.Rows.Item(n).Cells(c_taxincometoamt.Index).Value = FormatNumber(.Item("Taxableincometoamount"), 2)
                    dgvlisttaxableamt.Rows.Item(n).Cells(c_rowID.Index).Value = .Item("RowID").ToString
                End With
            Next

        Else
            dgvlisttaxableamt.Rows.Clear()

        End If

    End Sub

    Private Sub Revised_Withholding_Tax_Tables_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        InfoBalloon(, , cmbPayType, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        'If FormLeft.Contains("Withholding tax table") Then
        '    FormLeft.Remove("Withholding tax table")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.Text = "Welcome"
        'Else
        '    MDIPrimaryForm.Text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

        GeneralForm.listGeneralForm.Remove(Me.Name)
    End Sub

    Dim view_ID As Object

    Dim dontUpdate As SByte = 0

    Dim dontCreate As SByte = 0

    Private Sub Revised_Withholding_Tax_Tables_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillpaytype()
        fillFilingStatus()

        view_ID = VIEW_privilege("Withholding Tax Table", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then
            btnSave.Visible = 0
            tsbtnNewTax.Visible = 0
            tsbtnSaveTax.Visible = 0
            dontUpdate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    btnSave.Visible = 0
                    tsbtnNewTax.Visible = 0
                    tsbtnSaveTax.Visible = 0
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

                    'If drow("Deleting").ToString = "N" Then
                    '    ToolStripButton3.Visible = 0
                    'Else
                    '    ToolStripButton3.Visible = 1
                    'End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

    End Sub


    Private Sub dgvfilingstatus_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvfilingstatus.CellContentClick

    End Sub

    Private Sub dgvfilingstatus_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvfilingstatus.CellClick
        If cmbPayType.SelectedIndex = -1 Or _
            cmbPayType.SelectedIndex = 0 Then

            InfoBalloon("Please select a pay frequency.", "No pay frequency is selected", cmbPayType, 128, -69)
        Else

            selectfilingstatus()

        End If

    End Sub

    Private Sub cmbPayType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPayType.SelectedIndexChanged
        dgvlisttaxableamt.Rows.Clear()
        Dim pf As String = getStringItem("select RoWID from payfrequency where PayFrequencyType = '" & cmbPayType.Text & "'")
        getpf = Val(pf)

        If cmbPayType.SelectedIndex = -1 Or _
            cmbPayType.SelectedIndex = 0 Then

        Else

            selectfilingstatus()

        End If

    End Sub




    Private Sub tsbtnNewTax_Click(sender As Object, e As EventArgs) Handles tsbtnNewTax.Click

    End Sub

    Private Sub tsbtnSaveTax_Click(sender As Object, e As EventArgs) Handles tsbtnSaveTax.Click

        dgvlisttaxableamt.EndEdit(True)

        For Each drow As DataGridViewRow In dgvlisttaxableamt.Rows
            Dim examt, excessamt, taxfrom, taxto As String
            examt = FormatPercent(drow.Cells(c_ExemptionAmount.Index).Value)
            excessamt = drow.Cells(c_ExemptionExcessAmount.Index).Value
            taxfrom = drow.Cells(c_Taxincomeframt.Index).Value
            taxto = drow.Cells(c_taxincometoamt.Index).Value

            Dim getexamt, gettaxfrom, gettaxto As Double
            'Dim str As String = percentlabel.Text
            ' Get rid of percentage sign
            'str = str.Replace("%", "")
            ' Check if you can convert

            excessamt = excessamt.Replace("%", "")
            Dim dbl As Double
            If Not (Double.TryParse(excessamt, dbl)) Then Exit Sub
            ' Successfully converted


            ' divide by 100 to get percent
            dbl = dbl / 100




            If Double.TryParse(examt, getexamt) Then
                getexamt = CDec(examt)
            Else
                getexamt = 0
            End If

            If Double.TryParse(taxfrom, gettaxfrom) Then
                gettaxfrom = CDec(taxfrom)
            Else
                gettaxfrom = 0
            End If

            If Double.TryParse(taxto, gettaxto) Then
                gettaxto = CDec(taxto)
            Else
                gettaxto = 0
            End If

            DirectCommand("UPDATE paywithholdingtax SET " & _
                          "ExemptionAmount = '" & getexamt & "', " & _
                          "ExemptioninExcessAmount = '" & dbl & "', " & _
                          "Taxableincomefromamount = '" & gettaxfrom & "', " & _
                          "Taxableincometoamount = '" & gettaxto & "' " & _
                          "WHERE RowID = '" & drow.Cells(c_rowID.Index).Value & "'")
        Next
        MsgBox("Save Successfully!", MsgBoxStyle.Information, "Saved.")

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

    End Sub

    Private Sub tsbtnCancelTax_Click(sender As Object, e As EventArgs) Handles tsbtnCancelTax.Click

        If dgvfilingstatus.RowCount <> 0 Then
            dgvfilingstatus_CellClick(sender, New DataGridViewCellEventArgs(c_FilingStatus.Index, 0))
        End If

    End Sub

    Private Sub tsbtnCloseTax_Click(sender As Object, e As EventArgs) Handles tsbtnCloseTax.Click
        Me.Close()
    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub dgvlisttaxableamt_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvlisttaxableamt.CellContentClick

    End Sub

    Private Sub dgvlisttaxableamt_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvlisttaxableamt.EditingControlShowing
        e.Control.ContextMenu = New ContextMenu

    End Sub

    Dim imp_PayFreqID = Nothing

    Dim imp_FilingstatID = Nothing

    Dim imp_FilePath = Nothing



    Private Sub tsbtnimportwtax_Click(sender As Object, e As EventArgs) Handles tsbtnimportwtax.Click

        'Dim n_payFreqSelectn As New payFreqSelectn

        'If n_payFreqSelectn.ShowDialog = Windows.Forms.DialogResult.OK Then

        'End If

        imp_PayFreqID = Nothing

        imp_FilingstatID = Nothing

        imp_FilePath = Nothing

        Dim n_importWithholdingTax As New importWithholdingTax

        If n_importWithholdingTax.ShowDialog = Windows.Forms.DialogResult.OK Then

            imp_PayFreqID = n_importWithholdingTax.PayFrequencyID

            imp_FilingstatID = n_importWithholdingTax.FilingStatusID

            imp_FilePath = n_importWithholdingTax.ImportFilePath


            ToolStrip1.Enabled = False

            ToolStripProgressBar1.Value = 0

            ToolStripProgressBar1.Visible = True

            bgworkimporttax.RunWorkerAsync()

        End If

    End Sub

    Private Sub bgworkimporttax_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkimporttax.DoWork

        Dim dtimporttax As DataTable

        dtimporttax = _
            ExcelToCVS(imp_FilePath)

        Dim indx = 1

        If dtimporttax Is Nothing Then

        Else

            Dim importrecordcount = dtimporttax.Rows.Count

            For Each drow As DataRow In dtimporttax.Rows

                INSUPD_paywithholdingtax(, _
                                         drow(2), _
                                         drow(3), _
                                         drow(0), _
                                         drow(1))

                bgworkimporttax.ReportProgress(CInt(100 * indx / importrecordcount), "")

                indx += 1

            Next

        End If

    End Sub

    Private Sub bgworkimporttax_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkimporttax.ProgressChanged

        ToolStripProgressBar1.Value = CType(e.ProgressPercentage, Integer)

    End Sub

    Private Sub bgworkimporttax_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkimporttax.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Error: " & vbNewLine & e.Error.Message)

        ElseIf e.Cancelled Then

        Else

            If dgvfilingstatus.RowCount <> 0 Then

                dgvfilingstatus_CellClick(sender, New DataGridViewCellEventArgs(c_FilingStatus.Index, 0))

            End If

        End If

        ToolStrip1.Enabled = True

        ToolStripProgressBar1.Visible = False

        ToolStripProgressBar1.Value = 0

    End Sub

    Sub INSUPD_paywithholdingtax(Optional wtx_RowID = Nothing, _
                                 Optional wtx_ExemptionAmount = Nothing, _
                                 Optional wtx_ExemptionInExcessAmount = Nothing, _
                                 Optional wtx_TaxableIncomeFromAmount = Nothing, _
                                 Optional wtx_TaxableIncomeToAmount = Nothing)

        Dim params(10, 2) As Object

        params(0, 0) = "wtx_RowID"
        params(1, 0) = "wtx_CreatedBy"
        params(2, 0) = "wtx_LastUpdBy"
        params(3, 0) = "wtx_PayFrequencyID"
        params(4, 0) = "wtx_FilingStatusID"
        params(5, 0) = "wtx_EffectiveDateFrom"
        params(6, 0) = "wtx_EffectiveDateTo"
        params(7, 0) = "wtx_ExemptionAmount"
        params(8, 0) = "wtx_ExemptionInExcessAmount"
        params(9, 0) = "wtx_TaxableIncomeFromAmount"
        params(10, 0) = "wtx_TaxableIncomeToAmount"

        params(0, 1) = If(wtx_RowID = Nothing, DBNull.Value, wtx_RowID)
        params(1, 1) = z_User
        params(2, 1) = z_User
        params(3, 1) = If(imp_PayFreqID = Nothing, DBNull.Value, imp_PayFreqID)
        params(4, 1) = If(imp_FilingstatID = Nothing, DBNull.Value, imp_FilingstatID)
        params(5, 1) = DBNull.Value '"wtx_EffectiveDateFrom"
        params(6, 1) = DBNull.Value '"wtx_EffectiveDateTo"
        params(7, 1) = ValNoComma(wtx_ExemptionAmount)
        params(8, 1) = ValNoComma(wtx_ExemptionInExcessAmount)
        params(9, 1) = ValNoComma(wtx_TaxableIncomeFromAmount)
        params(10, 1) = ValNoComma(wtx_TaxableIncomeToAmount)

        EXEC_INSUPD_PROCEDURE(params, _
                               "INSUPD_paywithholdingtax", _
                               "returnval")

    End Sub

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

End Class