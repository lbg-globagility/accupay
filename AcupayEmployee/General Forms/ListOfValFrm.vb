Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Linq
'Imports System.DirectoryServices
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Public Class ListOfValFrm
    'Dim manager As New sqlModule.Manager
    Dim conn As New MySqlConnection(db_connectinstring) 'Manager.GetConnString
    Dim sqlquery As String
    Dim sqlcmd As MySqlCommand
    Dim sqlrd As MySqlDataReader
    Dim fs As FileStream
    Dim br As BinaryReader
    Dim parentval As New AutoCompleteStringCollection
    Dim listofvtype As New AutoCompleteStringCollection
    Dim cue As String
    Dim ImageData() As Byte
    Dim AttachedFile As Object
    Dim legit As Boolean = True
    Dim fraud As Boolean = False
    Dim itemno, rowscount As Integer
    Dim thefilepath As String = Nothing
    Dim FileName, FileExtension As String
    Dim listofvaltype, listofvaldisplayvalue As Integer
    Dim nowDate = Date.Now.ToString("yyyy/MM/dd HH:mm:ss")
    Dim listofvalsystemaccntflg, listofvaldisplayaccntflg As Char
    Private Sub ListOfValueForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Cursor = Cursors.WaitCursor
        Try
            errProvider.Clear()
            clearfields()
            callAutoCompleteFunctions()
            callAutoPopulateFunctions()
            'enlistTheLists("SELECT displayvalue FROM listofval WHERE displayaccountFlg = 'Y' AND status = 'Active' GROUP BY displayvalue ", parentval)
            'enlistTheLists("SELECT type FROM listofval WHERE displayaccountFlg = 'Y' GROUP BY type ", listofvtype)
            enlistTheLists("SELECT displayvalue FROM listofval GROUP BY displayvalue ", parentval)
            enlistTheLists("SELECT type FROM listofval GROUP BY type ", listofvtype)
            lv_parentvalue.AutoCompleteCustomSource = parentval
            lv_type.AutoCompleteCustomSource = listofvtype
            displayListOfValueDetails()
            tabMain.SelectedTab = tabDetails
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ListOfValueForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'Me.Cursor = Cursors.WaitCursor
        'Try
        myBalloon(, , lblsavemsg, , , 1)
        'Catch ex As Exception
        '    MsgBox(getErrExcptn(ex, Me.Name))
        'Finally
        '    conn.Close()
        'End Try
        'Me.Cursor = Cursors.Default
    End Sub
#Region "Functions"
    Sub callAutoCompleteFunctions()
        SearchCompleteLOVType()
        SearchCompleteParentValue()
    End Sub
    Sub callAutoPopulateFunctions()
        populateLOVType()
        populateParentValue()
        'populateStatus()
    End Sub
#Region "Clear/Enable/Visible Functions"
    Sub clearfields()
        Try
            clearListOfValueInformation()
            enableGB(legit, fraud, fraud)
            enableANDvisibleMS(legit, fraud, fraud)
            dgUnknown.Rows.Clear()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Sub clearListOfValueInformation()
        Try
            txtDisplayValue.Text = ""
            txtComments.Text = ""
            cboLOVType.Text = ""
            cboParentValue.Text = ""
            cboStatus.SelectedItem = Nothing
            cboLOVType.SelectedItem = Nothing
            cboParentValue.SelectedItem = Nothing
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Sub enableGB(ByVal enable1 As Boolean, ByVal enable2 As Boolean, ByVal enable3 As Boolean)
        Try
            gbListOfValList.Enabled = enable1
            gbListOfVal.Enabled = enable2
            gbUnknown.Enabled = enable3
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Sub enableANDvisibleMS(ByVal enable1 As Boolean, ByVal enable2 As Boolean, ByVal visible1 As Boolean)
        Try
            msNew.Enabled = enable1
            msSave.Enabled = enable2
            msClear.Enabled = legit
            msCancel.Visible = visible1
            msAdd.Visible = fraud
            cmdFirst.Visible = fraud
            cmdPrev.Visible = fraud
            cmdNext.Visible = fraud
            cmdLast.Visible = fraud
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Sub datagridReadOnlySetup(ByVal readonly1 As Boolean, ByVal readonly2 As Boolean)
        Try
            dgUnknown.CommitEdit(True)
            lv_no.ReadOnly = readonly2
            lv_displayvalue.ReadOnly = readonly1
            lv_parentvalue.ReadOnly = readonly1
            lv_comments.ReadOnly = readonly1
            lv_status.ReadOnly = readonly1
            'lv_type.ReadOnly = readonly1
            lv_systemaccount.ReadOnly = readonly2
            dgUnknown.CommitEdit(True)
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
#End Region
#Region "Click Functions"
    Sub tsrefreshperformclick()
        Try
            errProvider.Clear()
            clearfields()
            callAutoCompleteFunctions()
            callAutoPopulateFunctions()
            displayListOfValueDetails()
            tabMain.SelectedTab = tabDetails
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
#End Region
#Region "Display Functions"
#Region "AutoComplete Functions"
    Sub SearchCompleteLOVType()
        Try
            Dim lovtype As New AutoCompleteStringCollection
            Dim cmd As New MySqlCommand("SELECT type FROM listofval GROUP BY type ", conn) 'WHERE displayaccountFlg = 'Y' 
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter(cmd)
            da.Fill(ds, "list")
            Dim i As Integer
            For i = 0 To ds.Tables(0).Rows.Count - 1
                lovtype.Add(ds.Tables(0).Rows(i)("type").ToString())
            Next
            cboLOVType.AutoCompleteSource = AutoCompleteSource.CustomSource
            cboLOVType.AutoCompleteCustomSource = lovtype
            cboLOVType.AutoCompleteMode = AutoCompleteMode.Suggest
        Catch ex As Exception
        End Try
        conn.Close()
    End Sub
    Sub SearchCompleteParentValue()
        Try
            Dim parentvalue As New AutoCompleteStringCollection
            Dim cmd As New MySqlCommand("SELECT displayvalue FROM listofval GROUP BY displayvalue ", conn) 'WHERE displayaccountFlg = 'Y' AND status = 'Active' 
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter(cmd)
            da.Fill(ds, "list")
            Dim i As Integer
            For i = 0 To ds.Tables(0).Rows.Count - 1
                parentvalue.Add(ds.Tables(0).Rows(i)("displayvalue").ToString())
            Next
            cboParentValue.AutoCompleteSource = AutoCompleteSource.CustomSource
            cboParentValue.AutoCompleteCustomSource = parentvalue
            cboParentValue.AutoCompleteMode = AutoCompleteMode.Suggest
        Catch ex As Exception
        End Try
        conn.Close()
    End Sub
#End Region
#Region "AutoPopulate Functions"
    Sub populateLOVType()
        Try
            cboLOVType.Items.Clear()
            If conn.State = ConnectionState.Open Then conn.Close()
            Dim sql1 As String = "SELECT type FROM listofval GROUP BY type ORDER BY type " 'WHERE displayaccountFlg = 'Y' 
            If conn.State = ConnectionState.Closed Then conn.Open()
            Dim cmd1 As New MySqlCommand(sql1, conn)
            Dim reader1 As MySqlDataReader = cmd1.ExecuteReader()
            While reader1.Read()
                cboLOVType.Items.Add(reader1(0).ToString())
            End While
            cboLOVType.Items.Add("")
            reader1.Close()
            conn.Close()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Sub populateParentValue()
        Try
            cboParentValue.Items.Clear()
            If conn.State = ConnectionState.Open Then conn.Close()
            Dim sql1 As String = "SELECT displayvalue FROM listofval GROUP BY displayvalue ORDER BY displayvalue " 'WHERE displayaccountFlg = 'Y' AND status = 'Active' 
            If conn.State = ConnectionState.Closed Then conn.Open()
            Dim cmd1 As New MySqlCommand(sql1, conn)
            Dim reader1 As MySqlDataReader = cmd1.ExecuteReader()
            While reader1.Read()
                cboParentValue.Items.Add(reader1(0).ToString())
            End While
            cboParentValue.Items.Add("")
            reader1.Close()
            conn.Close()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Sub populateStatus()
        Try
            cboStatus.Items.Clear() : lv_status.Items.Clear()
            If conn.State = ConnectionState.Open Then conn.Close()
            Dim sql1 As String = "SELECT displayvalue FROM listofval ORDER BY displayvalue" 'WHERE type = 'Status' AND status = 'Active' 
            If conn.State = ConnectionState.Closed Then conn.Open()
            Dim cmd1 As New MySqlCommand(sql1, conn)
            Dim reader1 As MySqlDataReader = cmd1.ExecuteReader()
            While reader1.Read()
                cboStatus.Items.Add(reader1(0).ToString())
                lv_status.Items.Add(reader1(0).ToString())
            End While
            reader1.Close()
            conn.Close()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
#End Region
#Region "Display Datagrids"
    Public Function displayListOfValueDetails()
        If conn.State = ConnectionState.Open Then conn.Close()
        sqlquery = "SELECT type FROM listofval GROUP BY type ORDER BY type " 'WHERE displayaccountFlg = 'Y' 
        dgListOfVal.Rows.Clear()
        Dim sqlcmd As New MySqlCommand(sqlquery, conn)
        conn.Open()
        Dim sqlrd As MySqlDataReader = sqlcmd.ExecuteReader
        sqlcmd = Nothing
        If sqlrd.HasRows Then
            Try
                Dim n As Integer = 0
                Dim seqno As Integer = 1
                While sqlrd.Read
                    dgListOfVal.Rows.Add()
                    dgListOfVal.Item(l_no.Index, n).Value = seqno
                    dgListOfVal.Item(l_type.Index, n).Value = sqlrd(0)
                    seqno = seqno + 1
                    n = n + 1
                End While
                Return True
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name))
                sqlrd.Close()
                conn.Close()
                Return False
            Finally
                If dgListOfVal.Rows.Count <> 0 Then
                    dgListOfVal.CurrentRow.Selected = False
                End If
                sqlrd.Close()
                conn.Close()
            End Try
            Return True
        End If
        conn.Close()
        Return Nothing
    End Function
    Public Function displayListOfValueItems(ByVal slovtype As String)
        If conn.State = ConnectionState.Open Then conn.Close()
        sqlquery = "SELECT lv.rowid,lv.displayvalue,lv.parentlic,lv.description,lv.type,lv.Active FROM listofval lv WHERE lv.type = '" & slovtype & "' ORDER BY lv.displayvalue "

        'lv.systemaccountflg,

        'lv.status,

        'lv.displayaccountFlg = 'Y' AND 

        dgUnknown.Rows.Clear()
        Dim sqlcmd As New MySqlCommand(sqlquery, conn)
        conn.Open()
        Dim sqlrd As MySqlDataReader = sqlcmd.ExecuteReader
        sqlcmd = Nothing
        If sqlrd.HasRows Then
            Try
                Dim n As Integer = 0
                Dim seqno As Integer = 1
                While sqlrd.Read
                    dgUnknown.Rows.Add()
                    dgUnknown.Item(lv_rowid.Index, n).Value = sqlrd("rowid")
                    'dgUnknown.Item(lv_systemaccountflg.Index, n).Value = sqlrd(1)
                    dgUnknown.Item(lv_no.Index, n).Value = seqno
                    dgUnknown.Item(lv_displayvalue.Index, n).Value = sqlrd("displayvalue")
                    dgUnknown.Item(lv_parentvalue.Index, n).Value = sqlrd("parentlic")
                    dgUnknown.Item(lv_comments.Index, n).Value = sqlrd("description")
                    'dgUnknown.Item(lv_status.Index, n).Value = sqlrd(5)
                    dgUnknown.Item(lv_status.Index, n).Value = sqlrd("Active")
                    dgUnknown.Item(lv_type.Index, n).Value = sqlrd("type")
                    If sqlrd(1) = "Y" Then
                        dgUnknown.Item(lv_systemaccount.Index, n).Value = legit
                    Else
                        dgUnknown.Item(lv_systemaccount.Index, n).Value = fraud
                    End If
                    seqno = seqno + 1
                    n = n + 1
                End While
                Return True
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name))
                sqlrd.Close()
                conn.Close()
                Return False
            Finally
                If dgUnknown.Rows.Count <> 0 Then
                    dgUnknown.CurrentRow.Selected = False
                End If
                sqlrd.Close()
                conn.Close()
            End Try
            Return True
        End If
        conn.Close()
        Return Nothing
    End Function
#End Region
#End Region
#Region "Saving Functions"
    Sub getListOfValTypeID(ByVal lovType As String)
        Try
            listofvaltype = 0
            If conn.State = ConnectionState.Open Then conn.Close()
            Dim dtLOVid As New DataTable
            dtLOVid = getDataTableForSQL("SELECT rowid FROM listofval WHERE type = '" & lovType & "' ")
            If dtLOVid.Rows.Count <> 0 Then
                listofvaltype = dtLOVid.Rows(0)(0)
            Else
                listofvaltype = 0
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Sub getListOfValDetails(ByVal lovLIC As String, ByVal lovType As String)
        Try
            listofvaldisplayvalue = 0 : listofvalsystemaccntflg = "" : listofvaldisplayaccntflg = ""
            If conn.State = ConnectionState.Open Then conn.Close()
            Dim dtLOVid As New DataTable
            dtLOVid = getDataTableForSQL("SELECT rowid,systemaccountflg,displayaccountflg FROM listofval WHERE lic = '" & lovLIC & "' AND type = '" & lovType & "' ")
            If dtLOVid.Rows.Count <> 0 Then
                listofvaldisplayvalue = dtLOVid.Rows(0)(0)
                listofvalsystemaccntflg = dtLOVid.Rows(0)(1)
                listofvaldisplayaccntflg = dtLOVid.Rows(0)(2)
            Else
                listofvaldisplayvalue = 0 : listofvalsystemaccntflg = "" : listofvaldisplayaccntflg = ""
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
#End Region
#End Region
    Private Sub tabMain_DrawItem(sender As Object, e As DrawItemEventArgs) Handles tabMain.DrawItem
        Try
            TabControlColor(tabMain, e)
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Private Sub dgUnknown_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgUnknown.EditingControlShowing
        Try
            Dim dgvColName = dgUnknown.Columns(dgUnknown.CurrentCell.ColumnIndex).Name
            If dgvColName = "lv_parentvalue" Then
            ElseIf dgvColName = "lv_status" Then
            ElseIf dgvColName = "lv_type" Then
            Else
                Dim cellTextBox = DirectCast(e.Control, TextBox)
                cellTextBox.AutoCompleteCustomSource = Nothing
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Private Sub dgUnknown_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgUnknown.CellClick
        Try
            If dgUnknown.CurrentRow.Cells("lv_systemaccountflg").Value = "Y" Then
                dgUnknown.ReadOnly = legit
                datagridReadOnlySetup(legit, legit)
            Else
                dgUnknown.ReadOnly = fraud
                datagridReadOnlySetup(fraud, legit)
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Private Sub dgUnknown_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgUnknown.CellEndEdit
        Me.Cursor = Cursors.WaitCursor
        Try
            dgUnknown.CommitEdit(True)
            If dgUnknown.CurrentRow.Cells("lv_displayvalue").Value <> "" Then
                dgUnknown.Item("lv_displayvalue", e.RowIndex).ErrorText = Nothing
            Else
                dgUnknown.Item("lv_displayvalue", e.RowIndex).ErrorText = "Please enter the display name of this value"
            End If
            If dgUnknown.CurrentRow.Cells("lv_type").Value <> "" Then
                dgUnknown.Item("lv_type", e.RowIndex).ErrorText = Nothing
            Else
                dgUnknown.Item("lv_type", e.RowIndex).ErrorText = "Please enter the type of this value"
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub pbClose_Click(sender As Object, e As EventArgs) Handles pbClose.Click
        'Me.Cursor = Cursors.WaitCursor
        'Try
        '    'PrimaryForm.LstOfValForm = False
        Me.Close()
        'Catch ex As Exception
        '    MsgBox(getErrExcptn(ex, Me.Name))
        'Finally
        '    conn.Close()
        'End Try
        'Me.Cursor = Cursors.Default
    End Sub
    Private Sub cboStatus_Leave(sender As Object, e As EventArgs) Handles cboStatus.Leave
        Try
            cboLOVType.Focus()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
    End Sub
    Private Sub tsRefresh_Click(sender As Object, e As EventArgs) Handles tsRefresh.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            tsrefreshperformclick()
            myBalloon("Successfully refresh", "Refresh", lblsavemsg, -15, -65)
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub msClear_Click(sender As Object, e As EventArgs) Handles msClear.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            errProvider.Clear()
            If cue = "New" Then
                clearListOfValueInformation()
                dgUnknown.Rows.Clear()
                cboLOVType.Focus()
            Else
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub msNew_Click(sender As Object, e As EventArgs) Handles msNew.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            errProvider.Clear()
            tabMain.SelectedTab = tabDetails
            clearListOfValueInformation()
            dgUnknown.Rows.Clear()
            enableGB(fraud, legit, fraud)
            enableANDvisibleMS(fraud, legit, legit)
            cue = "New"
            If dgListOfVal.Rows.Count <> 0 Then
                dgListOfVal.CurrentRow.Selected = False
            End If
            cboLOVType.Focus()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub msCancel_Click(sender As Object, e As EventArgs) Handles msCancel.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            errProvider.Clear()
            tabMain.SelectedTab = tabDetails
            clearListOfValueInformation()
            enableGB(legit, fraud, legit)
            enableANDvisibleMS(legit, legit, fraud)
            cue = "Edit"
            If dgListOfVal.Rows.Count <> 0 Then
                dgListOfVal.CurrentRow.Selected = True
                displayListOfValueItems(dgListOfVal.CurrentRow.Cells("l_type").Value)
            Else
                tsrefreshperformclick()
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub dgListOfVal_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgListOfVal.CellContentClick

    End Sub

    Private Sub dgUnknown_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgUnknown.CellContentClick

    End Sub

    Private Sub dgListOfVal_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgListOfVal.CellClick
        Me.Cursor = Cursors.WaitCursor
        Try
            errProvider.Clear()
            tabMain.SelectedTab = tabDetails
            clearListOfValueInformation()
            enableGB(legit, fraud, legit)
            enableANDvisibleMS(legit, legit, fraud)
            cue = "Edit"
            If dgListOfVal.Rows.Count <> 0 Then
                displayListOfValueItems(dgListOfVal.CurrentRow.Cells("l_type").Value)
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub msSave_Click(sender As Object, e As EventArgs) Handles msSave.Click
        Try
            errProvider.Clear()
            dgUnknown.CommitEdit(True)
            'myModule.systemerrorfound = False
            If cue = "New" Then
                If cboLOVType.Text = "" AndAlso txtDisplayValue.Text = "" AndAlso cboStatus.Text = "" Then
                    errProvider.SetError(cboLOVType, "Please fill-up these fields")
                    errProvider.SetError(txtDisplayValue, "Please fill-up these fields")
                    errProvider.SetError(cboStatus, "Please fill-up these fields")
                    cboLOVType.Focus()
                ElseIf cboLOVType.Text = "" Then
                    errProvider.SetError(cboLOVType, "Please choose the type of this value")
                    cboLOVType.Focus()
                ElseIf txtDisplayValue.Text = "" Then
                    errProvider.SetError(txtDisplayValue, "Please enter the display name of this value")
                    txtDisplayValue.Focus()
                ElseIf cboStatus.Text = "" Then
                    errProvider.SetError(cboStatus, "Please choose the status of this value")
                    cboStatus.Focus()
                Else
                    If MessageBox.Show("Would you like to save the changes on this page? ", "Saving", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        getListOfValTypeID(cboLOVType.Text)
                        If listofvaltype = 0 Then
                            I_ListOfVal(nowDate, z_User, nowDate, z_User, txtDisplayValue.Text, txtDisplayValue.Text, cboLOVType.Text, cboParentValue.Text, cboStatus.Text, txtComments.Text, "N", "Y", DBNull.Value)

                        Else
                            getListOfValDetails(txtDisplayValue.Text, cboLOVType.Text)
                            If listofvaldisplayvalue = 0 Then
                                I_ListOfVal(nowDate, z_User, nowDate, z_User, txtDisplayValue.Text, txtDisplayValue.Text, cboLOVType.Text, cboParentValue.Text, cboStatus.Text, txtComments.Text, "N", "Y", DBNull.Value)
                            Else
                                If listofvalsystemaccntflg = "Y" Then
                                    U_ListOfVal(listofvaldisplayvalue, nowDate, z_User, txtDisplayValue.Text, txtDisplayValue.Text, cboLOVType.Text, cboParentValue.Text, cboStatus.Text, txtComments.Text, listofvalsystemaccntflg, listofvaldisplayaccntflg, DBNull.Value)
                                Else
                                    U_ListOfVal(listofvaldisplayvalue, nowDate, z_User, txtDisplayValue.Text, txtDisplayValue.Text, cboLOVType.Text, cboParentValue.Text, cboStatus.Text, txtComments.Text, "N", "Y", DBNull.Value)
                                End If
                            End If
                        End If
                        'If myModule.systemerrorfound = False Then
                        '    myBalloon("Successfully Save", "Save", lblsavemsg, -15, -65)
                        '    tsrefreshperformclick()
                        'End If
                    End If
                End If
            ElseIf cue = "Edit" Then
                If dgUnknown.Rows.Count <> 0 Then
                    rowscount = dgUnknown.Rows.Count - 1
                    For i = 0 To dgUnknown.Rows.Count - 1
                        dgUnknown.Rows(i).Cells("lv_displayvalue").ErrorText = Nothing
                        dgUnknown.Rows(i).Cells("lv_type").ErrorText = Nothing
                        If dgUnknown.Rows(i).Cells("lv_displayvalue").Value = "" Then
                            dgUnknown.Rows(i).Cells("lv_displayvalue").ErrorText = "Please enter the display name of this value"
                            Exit For
                        ElseIf dgUnknown.Rows(i).Cells("lv_type").Value = "" Then
                            dgUnknown.Rows(i).Cells("lv_type").ErrorText = "Please enter the type of this value"
                            Exit For
                        ElseIf rowscount = 0 Then
                            If MessageBox.Show("Would you like to save the changes on this page? ", "Saving", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                For a = 0 To dgUnknown.Rows.Count - 1
                                    If dgUnknown.Rows(a).Cells("lv_systemaccountflg").Value <> "Y" Then
                                        getListOfValTypeID(dgUnknown.Rows(a).Cells("lv_type").Value)
                                        If listofvaltype = 0 Then
                                            I_ListOfVal(nowDate, z_User, nowDate, z_User, dgUnknown.Rows(a).Cells("lv_displayvalue").Value, dgUnknown.Rows(a).Cells("lv_displayvalue").Value, dgUnknown.Rows(a).Cells("lv_type").Value, dgUnknown.Rows(a).Cells("lv_parentvalue").Value, dgUnknown.Rows(a).Cells("lv_status").Value, dgUnknown.Rows(a).Cells("lv_comments").Value, "N", "Y", DBNull.Value)
                                        Else
                                            getListOfValDetails(dgUnknown.Rows(a).Cells("lv_displayvalue").Value, dgUnknown.Rows(a).Cells("lv_type").Value)
                                            If listofvaldisplayvalue = 0 Then
                                                I_ListOfVal(nowDate, z_User, nowDate, z_User, dgUnknown.Rows(a).Cells("lv_displayvalue").Value, dgUnknown.Rows(a).Cells("lv_displayvalue").Value, dgUnknown.Rows(a).Cells("lv_type").Value, dgUnknown.Rows(a).Cells("lv_parentvalue").Value, dgUnknown.Rows(a).Cells("lv_status").Value, dgUnknown.Rows(a).Cells("lv_comments").Value, "N", "Y", DBNull.Value)
                                            Else
                                                If listofvalsystemaccntflg <> "Y" Then
                                                    U_ListOfVal(listofvaldisplayvalue, nowDate, z_User, dgUnknown.Rows(a).Cells("lv_displayvalue").Value, dgUnknown.Rows(a).Cells("lv_displayvalue").Value, dgUnknown.Rows(a).Cells("lv_type").Value, dgUnknown.Rows(a).Cells("lv_parentvalue").Value, dgUnknown.Rows(a).Cells("lv_status").Value, dgUnknown.Rows(a).Cells("lv_comments").Value, "N", "Y", DBNull.Value)
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                                'If myModule.systemerrorfound = False Then
                                '    myBalloon("Successfully Updated", "Update", lblsavemsg, -15, -65)
                                '    tsrefreshperformclick()
                                'End If
                            End If
                        End If
                        rowscount = rowscount - 1
                    Next
                End If
            Else
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub dgListOfVal_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgListOfVal.DataError
        Me.Cursor = Cursors.WaitCursor
        Try
            'MessageBox.Show("Error:  " & e.Context.ToString())
            If (e.Context = DataGridViewDataErrorContexts.Commit) _
                Then
                'MessageBox.Show("Commit error")
            End If
            If (e.Context = DataGridViewDataErrorContexts.CurrentCellChange) Then
                MessageBox.Show("Cell change")
            End If
            If (e.Context = DataGridViewDataErrorContexts.Parsing) Then
                MessageBox.Show("parsing error")
            End If
            If (e.Context = DataGridViewDataErrorContexts.LeaveControl) Then
                ' MessageBox.Show("leave control error")
            End If
            If (e.Context = DataGridViewDataErrorContexts.Formatting) Then
                'MessageBox.Show("leave control error")
            End If
            If (TypeOf (e.Exception) Is ConstraintException) Then
                Dim view As DataGridView = CType(sender, DataGridView)
                view.Rows(e.RowIndex).ErrorText = "an error"
                view.Rows(e.RowIndex).Cells(e.ColumnIndex) _
                    .ErrorText = "an error"
                MsgBox("error")
                e.ThrowException = False
            End If
            If StrComp(e.Exception.Message, "Input string was not in a correct format.") = 0 Then
                MessageBox.Show("Please Enter a numeric Value")
                'This will change the number back to original
                dgListOfVal.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = " "
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub dgUnknown_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgUnknown.DataError
        Me.Cursor = Cursors.WaitCursor
        Try
            'MessageBox.Show("Error:  " & e.Context.ToString())
            If (e.Context = DataGridViewDataErrorContexts.Commit) _
                Then
                'MessageBox.Show("Commit error")
            End If
            If (e.Context = DataGridViewDataErrorContexts.CurrentCellChange) Then
                MessageBox.Show("Cell change")
            End If
            If (e.Context = DataGridViewDataErrorContexts.Parsing) Then
                MessageBox.Show("parsing error")
            End If
            If (e.Context = DataGridViewDataErrorContexts.LeaveControl) Then
                ' MessageBox.Show("leave control error")
            End If
            If (e.Context = DataGridViewDataErrorContexts.Formatting) Then
                'MessageBox.Show("leave control error")
            End If
            If (TypeOf (e.Exception) Is ConstraintException) Then
                Dim view As DataGridView = CType(sender, DataGridView)
                view.Rows(e.RowIndex).ErrorText = "an error"
                view.Rows(e.RowIndex).Cells(e.ColumnIndex) _
                    .ErrorText = "an error"
                MsgBox("error")
                e.ThrowException = False
            End If
            If StrComp(e.Exception.Message, "Input string was not in a correct format.") = 0 Then
                MessageBox.Show("Please Enter a numeric Value")
                'This will change the number back to original
                dgUnknown.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = " "
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            conn.Close()
        End Try
        Me.Cursor = Cursors.Default
    End Sub

End Class