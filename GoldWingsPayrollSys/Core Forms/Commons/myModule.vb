Imports MySql.Data.MySqlClient
Imports MySql.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO

Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Data.OleDb
Imports Microsoft.Win32

Module myModule
    Public conn As New MySqlConnection
    Public da As New MySqlDataAdapter
    Public cmd As New MySqlCommand

    Public firstchar_requiredforparametername As String = "?"

    Public n_DataBaseConnection As New DataBaseConnection
    Public mysql_conn_text As String = n_DataBaseConnection.GetStringMySQLConnectionString
    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Public machineShortDateFormat As String = RegKey.GetValue("sShortDate").ToString

    Public custom_mysqldateformat As String = String.Empty

    Public defRowCount As Integer
    Public scAutoComplete As New AutoCompleteStringCollection
    Public autcompORD_TYPES As New AutoCompleteStringCollection
    Public simpleSearchAutoComp As New AutoCompleteStringCollection

    Public isgetFromProd As Boolean = False
    Public sys_servername, sys_userid, sys_password, sys_db, sys_apppath As String
    Public prodImage As New DataTable
    Public orgztnID As String
    Public orgNam As String
    Public me_Name As String

    Public TimeTick As SByte = 0

    Public viewProdCaller As String
    Public newPLCaller As String

    Public prodImagequer As String = "SELECT PartNo,Image FROM product WHERE Image IS NOT NULL AND OrganizationID='"

    Public PO_STATS As String = "SELECT DisplayValue FROM listofval WHERE Type='PO_Status'" ' ORDER BY OrderBy

    Public PO_STATS_MRF As String = "SELECT DisplayValue FROM listofval WHERE Type='PO_Status' OR Type='PO_Status2' OR Type='PO_Status1' ORDER BY OrderBy"

    Public DR_STATS As String = "SELECT DisplayValue FROM listofval WHERE Type='DR Status' ORDER BY DisplayValue"

    Public PO_TYPE_ As String = "SELECT displayvalue FROM listofval WHERE type='Order Type' ORDER BY orderby"

    Public SYS_ORGZTN_ID As String = "SELECT COALESCE(RowID,'') FROM organization WHERE Name='" '& orgztn_name & "'" 'Ikhea Lighting Inc

    Public SYS_MAIN_BRNCH_ID As String = "SELECT RowID FROM inventorylocation WHERE Type='Main' AND Status='Active'"

    Public ORDER_TYPES As String = "SELECT DisplayValue FROM listofval WHERE Type='Order Type' ORDER BY DisplayValue"

    Public CURDATE_MDY As String = "SELECT CURDATE();" '"SELECT DATE_FORMAT(CURDATE(),'%m-%d-%Y')"

    Public USERNameStrPropr As String = "SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) FROM user u WHERE RowID="

    Public Width_resolution As Integer = My.Computer.Screen.Bounds.Width

    Public Height_resolution As Integer = My.Computer.Screen.Bounds.Height

    Public dbnow

    Public numofdaysthisyear As Integer

    Public previousForm As Form = Nothing

    Public FormLeft As New List(Of String)

    Public FormLeftHRIS As New List(Of String)

    Public FormLeftPayroll As New List(Of String)

    Public FormLeftTimeAttend As New List(Of String)

    Public position_view_table As New DataTable

    Public userFirstName As String = Nothing

    Public userLastName As String = Nothing

    Public backgroundworking As SByte = 0

    Public db_connectinstring = ""

    Public MachineLocalization As New DataTable

    Public AppFilePath As String = Application.StartupPath

    Public Function getConn() As MySqlConnection
        Return conn
    End Function

    Public Sub dbconn()

        Static write As SByte = 0

        Try

            Dim n_DataBaseConnection As New DataBaseConnection

            conn = New MySqlConnection

            conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString

            hasERR = 0

        Catch ex As Exception

            hasERR = 1

            MsgBox(ex.Message & " ERR_NO 77-10 : dbconn", MsgBoxStyle.Critical, "Server Connection")

        Finally
            'REG_EDIT_DBCONNECTION()

        End Try

    End Sub

    Public Function getErrExcptn(ByVal ex As Exception, Optional FormNam As String = Nothing) As String
        Dim st As StackTrace = New StackTrace(ex, True)
        Dim sf As StackFrame = st.GetFrame(st.FrameCount - 1)

        Dim op_FrmNam As String = If(FormNam = Nothing, "", FormNam & ".")
        'Form Name '.' Method Name '@Line' Code Line Number

        Dim mystr As String = ex.Message & vbNewLine & vbNewLine &
                        "ERROR occured in " & op_FrmNam &
                        st.GetFrame(st.FrameCount - 1).GetMethod.Name &
                        " " & sf.GetFileLineNumber() &
                        " " & ex.ToString()
        '               'ito ung line number sa code editor
        'ErrorLog(mystr)

        Return mystr

        'Return MsgBox(mystr, , "Unexpected Message")
    End Function
    Sub filltable(ByVal datgrid As Object, _
                         Optional _quer As String = Nothing, _
                         Optional Params As Array = Nothing, _
                         Optional CommandType As Object = Nothing)
        'Optional ParamValue As Object = Nothing, _
        Dim publictable As New DataTable
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()
            cmd = New MySqlCommand
            cmd.Connection = conn

            cmd.CommandText = _quer

            Select Case Val(CommandType)
                Case 0
                    cmd.CommandType = Data.CommandType.Text
                Case 1
                    'cmd = New MySqlCommand(_quer, conn)
                    cmd.Parameters.Clear()
                    cmd.CommandType = Data.CommandType.StoredProcedure
                    '.Parameters.AddWithValue(ParamName, ParamValue)
                    For indx = 0 To Params.GetUpperBound(0) - 1
                        Dim paramName As String = Params(indx, 0)
                        Dim paramVal = Params(indx, 1)
                        cmd.Parameters.AddWithValue(paramName, paramVal)

                    Next
            End Select

            da.SelectCommand = cmd
            da.Fill(publictable)
            datgrid.DataSource = publictable

            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " ERR_NO 77-10 : filltable", MsgBoxStyle.Critical, "Unexpected Message")
        Finally
            conn.Close()
            da.Dispose()
            cmd.Dispose()
        End Try

    End Sub

    Function retAsDatTbl(ByVal _quer As String, _
                         Optional dgv As DataGridView = Nothing) As Object

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(_quer)

        Return n_SQLQueryToDatatable.ResultTable

    End Function

    Sub dgvRowAdder(ByVal sqlcmd As String, _
                    ByVal dgvlistcatcher As DataGridView, _
                    Optional xtraCatcher As AutoCompleteStringCollection = Nothing, _
                    Optional asStringYes As SByte = 0)
        Dim dr As MySqlDataReader
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()
            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = sqlcmd
                dr = .ExecuteReader()
            End With

            dgvlistcatcher.Rows.Clear()

            If xtraCatcher Is Nothing Then
                If asStringYes = 0 Then
                    Do While dr.Read
                        Dim r = dgvlistcatcher.Rows.Add()
                        For c = 0 To dr.FieldCount - 1
                            Dim dr_val = If(IsDBNull(dr(c)), "", dr.GetString(c))
                            dgvlistcatcher.Rows(r).Cells(c).Value = dr_val
                        Next
                    Loop
                Else
                    Do While dr.Read
                        Dim r = dgvlistcatcher.Rows.Add()
                        For c = 0 To dr.FieldCount - 1
                            dgvlistcatcher.Rows(r).Cells(c).Value = dr(c)
                        Next
                    Loop
                End If
            Else
                If asStringYes = 0 Then
                    Do While dr.Read
                        Dim r = dgvlistcatcher.Rows.Add()
                        For c = 0 To dr.FieldCount - 1
                            Dim dr_val = If(IsDBNull(dr(c)), "", dr.GetString(c))
                            dgvlistcatcher.Rows(r).Cells(c).Value = dr_val

                            If dgvlistcatcher.Rows(r).Cells(c).Visible Then
                                xtraCatcher.Add(dr_val) 'dgvlistcatcher.Rows(r).Cells(c).ColumnIndex.ToString & "@" & 
                            End If
                        Next
                    Loop
                Else
                    Do While dr.Read
                        Dim r = dgvlistcatcher.Rows.Add()
                        For c = 0 To dr.FieldCount - 1
                            dgvlistcatcher.Rows(r).Cells(c).Value = dr(c)

                            If dgvlistcatcher.Rows(r).Cells(c).Visible Then
                                xtraCatcher.Add(dr(c)) 'dgvlistcatcher.Rows(r).Cells(c).ColumnIndex.ToString & "@" & 
                            End If
                        Next
                    Loop
                End If
            End If


            dr.Close()
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " ERR_NO 77-10 : dgvRowAdder", MsgBoxStyle.Critical, "Unexpected Message")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
    End Sub

    Public hasERR As SByte

    Public Function EXECQUER(ByVal cmdsql As String, _
                             Optional errorthrower As String = Nothing) As Object 'String

        Dim n_ExecuteQuery As New ExecuteQuery(cmdsql)

        Return n_ExecuteQuery.Result

    End Function

    Public Function EXECQUERByte(ByVal cmdsql As String, Optional makeItByte As Byte = Nothing) As Object
        Dim theObj As New Object
        Dim dr As MySqlDataReader
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            Try
                conn.Open()
                hasERR = 0
            Catch ex As Exception
                hasERR = 1
                MsgBox(ex.Message)
            End Try
            cmd = New MySqlCommand
            With cmd
                .CommandType = CommandType.Text
                .Connection = conn
                .CommandText = cmdsql
                dr = .ExecuteReader()

            End With
            If makeItByte = Nothing Then
                theObj = If(dr.Read = True, dr(0), Nothing)
            Else

            End If
            dr.Close()
            conn.Close()
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " ERR_NO : EXECQUERByte", , "Unexpected Message")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try

        Return theObj
    End Function
    Sub enlistTheLists(ByVal sqlcmd As String, ByVal listcatcher As AutoCompleteStringCollection, Optional isClear As SByte = Nothing)
        Dim dr As MySqlDataReader
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()
            cmd = New MySqlCommand
            With cmd
                .CommandType = CommandType.Text
                .Connection = conn
                .CommandText = sqlcmd
            End With
            dr = cmd.ExecuteReader()
            If isClear = Nothing Then : listcatcher.Clear() : End If
            Do While dr.Read
                listcatcher.Add(dr(0)) 'GetString
            Loop
            dr.Close()
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " ERR_NO 77-10 : enlistTheLists", MsgBoxStyle.Critical, "Unexpected Message")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        conn.Close()
    End Sub
    Sub enlistToCboBox(ByVal sqlcmd As String, ByVal listcatcher As ComboBox, Optional isClear As SByte = Nothing)
        Dim dr As MySqlDataReader
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()
            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = sqlcmd
            End With
            dr = cmd.ExecuteReader()
            If isClear = Nothing Then : listcatcher.Items.Clear() : End If
            Do While dr.Read
                If dr.GetString(0) <> "" Then
                    listcatcher.Items.Add(dr(0)) 'GetString
                End If
            Loop
            dr.Close()
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " ERR_NO 77-10 : enlistToCboBox", MsgBoxStyle.Critical, "Unexpected Message")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        conn.Close()
    End Sub

    Public Function TrapDecimKey(ByVal KCode As String, Optional _hasdecpt As SByte = 0) As Boolean    '//textbox keypress event insert number ONLY
        Static isdecpt As SByte = 0
        If (KCode >= 48 And KCode <= 57) Or KCode = 8 Or KCode = 46 Then
            'If KCode = 46 Then
            '    isdecpt += 1
            'Else : isdecpt = 0
            'End If
            'If isdecpt = 1 Then
            '    TrapDecimKey = True : isdecpt = 0
            'Else
            TrapDecimKey = False
            'End If
        Else
            TrapDecimKey = True
        End If
    End Function
    Public Function TrapNumKey(ByVal KCode As String) As Boolean    '//textbox keypress event insert number ONLY
        If (KCode >= 48 And KCode <= 57) Or KCode = 8 Then
            TrapNumKey = False
        Else
            TrapNumKey = True
        End If
    End Function

    Public Function TrapCharKey(ByVal KCode As String) As Boolean       '//textbox keypress event insert alphabet & space ONLY
        If (KCode >= 1 And KCode <= 7) Or (KCode >= 9 And KCode <= 31) Or (KCode >= 33 And KCode <= 45) Or (KCode >= 48 And KCode <= 57) Or (KCode >= 58 And KCode <= 64) Or (KCode >= 91 And KCode <= 96 Or (KCode >= 123 And KCode <= 126)) Or KCode = 94 Or KCode = 47 Or KCode = 46 Then
            TrapCharKey = True
        Else
            TrapCharKey = False
        End If
    End Function

    Public Function TrapSpaceKey(ByVal KCode As String) As Boolean      '//textbox keypress event disable space key
        If KCode = 32 Then
            TrapSpaceKey = True
        Else
            TrapSpaceKey = False
        End If
    End Function

    Sub rptParam(ByVal sender As Object, ByVal param As String, ByVal rptdoc As ReportDocument)
        Dim crParamFldDeftns As ParameterFieldDefinitions
        Dim crParamFldDeftn As ParameterFieldDefinition
        Dim crParamrVals As New ParameterValues
        Dim crParamDiscVal As New ParameterDiscreteValue
        If TypeOf sender Is TextBox Or TypeOf sender Is ComboBox Or TypeOf sender Is Label Then
            crParamDiscVal.Value = sender.Text
            crParamFldDeftns = rptdoc.DataDefinition.ParameterFields
            crParamFldDeftn = crParamFldDeftns.Item(param)
            crParamrVals.Add(crParamDiscVal)
            crParamFldDeftn.ApplyCurrentValues(crParamrVals)
        ElseIf TypeOf sender Is DateTimePicker Then
            crParamDiscVal.Value = Format(sender.Value, "MMM-dd-yyyy")
            crParamFldDeftns = rptdoc.DataDefinition.ParameterFields
            crParamFldDeftn = crParamFldDeftns.Item(param)
            crParamrVals.Add(crParamDiscVal)
            crParamFldDeftn.ApplyCurrentValues(crParamrVals)
        ElseIf TypeOf sender Is String Then
            crParamDiscVal.Value = sender
            crParamFldDeftns = rptdoc.DataDefinition.ParameterFields
            crParamFldDeftn = crParamFldDeftns.Item(param)
            crParamrVals.Add(crParamDiscVal)
            crParamFldDeftn.ApplyCurrentValues(crParamrVals)
        End If
    End Sub
    Sub rptDefntn(obj As Object, _
                  Optional nemofRptDefntn As String = Nothing, _
                  Optional rptdocu As ReportDocument = Nothing)

        Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = _
        rptdocu.ReportDefinition.ReportObjects.Item(nemofRptDefntn)

        If TypeOf obj Is TextBox Or TypeOf obj Is ComboBox Or TypeOf obj Is Label Then
            objText.Text = obj.Text
        ElseIf TypeOf obj Is DateTimePicker Then
            objText.Text = Format(obj.Value, "MMM-dd-yyyy")
        ElseIf TypeOf obj Is String Then
            objText.Text = obj
        End If
    End Sub

    Public Function convertFileToByte(ByVal filePath As String) As Byte()
        Dim fs As FileStream
        fs = New FileStream(filePath, FileMode.Open, FileAccess.Read)
        Dim fileByte As Byte() = New Byte(fs.Length - 1) {}
        fs.Read(fileByte, 0, System.Convert.ToInt32(fs.Length))
        fs.Close()

        Return fileByte

    End Function

    Sub qty_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        Dim txtlngth As New TextBox
        txtlngth = DirectCast(sender, TextBox)
        txtlngth.MaxLength = 11
        e.Handled = TrapNumKey(Asc(e.KeyChar))

        AddHandler txtlngth.TextChanged, AddressOf qty_TextChanged
    End Sub
    Sub qty_TextChanged(sender As Object, e As EventArgs)
        Dim txtlngth As New TextBox
        txtlngth = DirectCast(sender, TextBox)
        Try
            If txtlngth.Text <> "" Then
                If CInt(txtlngth.Text) < Integer.MaxValue Then '2147483647
                    'MsgBox("Mali ang Quantity mo!")
                End If
            End If
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & vbNewLine & "Please input an appropriate value.", MsgBoxStyle.Critical, "Too much Quantity")
        Finally
            RemoveHandler txtlngth.TextChanged, AddressOf qty_TextChanged
        End Try
    End Sub
    Sub rmks_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        Dim txtlngth As New TextBox
        txtlngth = DirectCast(sender, TextBox)
        txtlngth.MaxLength = 2000
        e.Handled = False 'TrapCharKey(Asc(e.KeyChar))
    End Sub

    Sub TabControlColor(ByVal TabCntrl As TabControl, _
                        ByVal ee As System.Windows.Forms.DrawItemEventArgs, _
                        Optional formColor As Color = Nothing)

        Dim g As Graphics = ee.Graphics
        Dim tp As TabPage = TabCntrl.TabPages(ee.Index)
        Dim br As Brush
        Dim sf As New StringFormat

        Dim r As New RectangleF(ee.Bounds.X, ee.Bounds.Y + 7, ee.Bounds.Width, ee.Bounds.Height - 7) '

        If formColor <> Nothing Then

            'ee.Graphics.FillRectangle(BackBrush, myTabRect)

            'Dim transparBackBrush = New SolidBrush(Color.Red)

            'BackBrush.Dispose()
            'transparBackBrush.Dispose()

            '====ito yung pagkulay sa puwang ng Items ng Tabcontrol
            'Dim custPen = New Pen(Color.Transparent, 2)

            '====ito yung pagkulay sa Border ng Tabcontrol
            If TabCntrl.Alignment = TabAlignment.Top Then
                Dim _myPen As New Pen(formColor, 7) 'Color.Red
                '- ((TabCntrl.Bounds.X * 0.01) + 2)
                'TabCntrl.Bounds.X - ((TabCntrl.Bounds.X * 0.05))
                ' + 2
                Dim myTabRect As Rectangle = New Rectangle(0, 0, TabCntrl.Width - ((TabCntrl.Width * 0.01)), TabCntrl.Height - 3)
                'Dim myTabRect As Rectangle = New Rectangle(0, 0, TabCntrl.Width, TabCntrl.Height)

                'Dim BackBrush = New SolidBrush(formColor)

                ee.Graphics.DrawRectangle(_myPen, myTabRect)

                Dim custBr = New SolidBrush(formColor)

                Dim x = 0
                For i = 0 To TabCntrl.TabCount - 1
                    x += ee.Bounds.Width
                Next
                '                                                                                             '+ x - (x * 0.08)
                Dim myCustRect = New Rectangle(x - (x * 0.08), 0, TabCntrl.Width - ((TabCntrl.Width * 0.02) - 2), ee.Bounds.Height)

                'ee.Graphics.DrawRectangle(custPen, myCustRect)

                ee.Graphics.FillRectangle(custBr, myCustRect)
                '====ito yung pagkulay sa puwang ng Items ng Tabcontrol

            ElseIf TabCntrl.Alignment = TabAlignment.Bottom Then

                '====ito yung pagkulay sa puwang ng Items ng Tabcontrol

                'Dim custBrBot = New SolidBrush(Color.Red) 'formColor

                'Dim x = 0
                'For i = 0 To TabCntrl.TabCount - 1
                '    x += ee.Bounds.Width
                'Next

                ''Dim _myPen As New Pen(Color.Red, 7) 'Color.Red'formColor
                ' ''- ((TabCntrl.Bounds.X * 0.01) + 2)
                ' ''TabCntrl.Bounds.X - ((TabCntrl.Bounds.X * 0.05))
                ' '' + 2
                ''Dim myTabRect As Rectangle = New Rectangle(x + (x * 0.01), ee.Bounds.Y, TabCntrl.Width - x, TabCntrl.Height - 3)
                ' ''Dim myTabRect As Rectangle = New Rectangle(0, 0, TabCntrl.Width, TabCntrl.Height)

                ' ''Dim BackBrush = New SolidBrush(formColor)

                ''ee.Graphics.DrawRectangle(_myPen, myTabRect)


                'Dim myCustRectBot = New Rectangle(x, ee.Bounds.Y, TabCntrl.Width - x, ee.Bounds.Height) 'TabCntrl.Width 

                ''ee.Graphics.DrawRectangle(custPen, myCustRect)

                'ee.Graphics.FillRectangle(custBrBot, myCustRectBot)
                '====ito yung pagkulay sa puwang ng Items ng Tabcontrol

            End If
            '====ito yung pagkulay sa Border ng Tabcontrol

        End If

        'Dim TabTextBrush As Brush = New SolidBrush(Color.White)
        Dim TabTextBrush As Brush = New SolidBrush(Color.FromArgb(142, 33, 11))
        'Dim TabBackBrush As Brush = New SolidBrush(Color.FromArgb(255, 242, 157))'255, 200, 80
        Dim TabBackBrush As Brush = New SolidBrush(Color.FromArgb(255, 245, 160)) '255, 255, 64
        '                                                       255, 245, 160 & 255, 255, 85
        'Dim TabTextBrush As Brush = New SolidBrush(Color.Black) 'FromArgb(142, 33, 11)
        'Dim TabBackBrush As Brush = New SolidBrush(Color.FromArgb(255, 242, 157))

        sf.Alignment = StringAlignment.Center

        Dim strTitle As String = tp.Text
        'If the current index is the Selected Index, change the color
        If TabCntrl.SelectedIndex = ee.Index Then
            'this is the background color of the tabpage
            'you could make this a standard color for the selected page
            'br = New SolidBrush(tp.BackColor)

            br = TabBackBrush
            'br = New SolidBrush(Color.PowderBlue)

            'this is the background color of the tab page
            g.FillRectangle(br, ee.Bounds)
            'this is the background color of the tab page
            'you could make this a stndard color for the selected page
            'br = New SolidBrush(tp.ForeColor)
            ' I changed to specific color
            br = TabTextBrush
            ' Tried bold, didn't like it
            Dim ff As Font
            ff = New Font(TabCntrl.Font, FontStyle.Bold)
            g.DrawString(strTitle, ff, br, r, sf)
            'g.DrawString("TAB PAGE 1", TabCntrl.Font, br, r, sf)
        Else
            'these are the standard colors for the unselected tab pages

            'br = New SolidBrush(Color.WhiteSmoke)

            'Dim small_rect As Rectangle = New Rectangle(ee.Bounds.X, _
            '                                            ee.Bounds.Y + 7, _
            '                                            ee.Bounds.Width, _
            '                                            ee.Bounds.Height - 7)

            'Color.FromArgb(formColor.ToArgb)
            br = New SolidBrush(Color.WhiteSmoke) 'formColor
            g.FillRectangle(br, ee.Bounds)
            br = New SolidBrush(Color.Black)
            Dim ff As Font
            ff = New Font(TabCntrl.Font, FontStyle.Regular)
            g.DrawString(strTitle, TabCntrl.Font, br, r, sf)

        End If

    End Sub

    Public infohint As ToolTip
    Public Sub InfoBalloon(Optional ToolTipStringContent As String = Nothing, Optional ToolTipStringTitle As String = Nothing, Optional objct As System.Windows.Forms.IWin32Window = Nothing, Optional x As Integer = 0, Optional y As Integer = 0, Optional dispo As SByte = 0, Optional duration As Integer = 3000)
        Try
            If dispo = 1 Then
                infohint.Active = False
                infohint.Hide(objct)
                infohint.Dispose()
            Else
                infohint = New ToolTip
                infohint.IsBalloon = True
                infohint.ToolTipTitle = ToolTipStringTitle
                infohint.ToolTipIcon = ToolTipIcon.Info
                infohint.Show(ToolTipStringContent, objct, x, y, duration)
            End If
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            'MsgBox(ex.Message & " ERR_NO 77-10 : InfoBalloon")

        End Try
    End Sub

    Public hintWarn As ToolTip 'New ToolTip

    Public Sub WarnBalloon(Optional ToolTipStringContent As String = Nothing, Optional ToolTipStringTitle As String = Nothing, Optional objct As System.Windows.Forms.IWin32Window = Nothing, Optional x As Integer = 0, Optional y As Integer = 0, Optional dispo As Byte = 0, Optional duration As Integer = 2275)

        'Dim hint As New ToolTip
        Try
            If dispo = 1 Then
                hintWarn.Hide(objct)
                hintWarn.Dispose()
                'Exit Try
                'Exit Sub
            Else
                hintWarn = New ToolTip
                hintWarn.IsBalloon = True
                hintWarn.ToolTipTitle = ToolTipStringTitle
                hintWarn.ToolTipIcon = ToolTipIcon.Warning
                hintWarn.Show(ToolTipStringContent, objct, x, y, duration)
            End If
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            'MsgBox(ex.Message & " ERR_NO 77-10 : WarnBalloon")
        End Try

    End Sub

    Sub myEllipseButton(ByVal dgv As DataGridView, ByVal colName As String, ByVal btn As Button, Optional isVisb As SByte = Nothing)
        'dgv = New DataGridView
        'colName = New String(colName)
        'btn = New Button
        Try
            If dgv.CurrentRow.Cells(colName).Selected Then

                If dgv.Columns(colName).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells Then
                    Dim wid As Integer = dgv.Columns(colName).Width

                    Dim x As Integer = dgv.Columns(colName).Width + 25
                    dgv.Columns(colName).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                    dgv.Columns(colName).Width = x
                End If

                Dim rect As Rectangle = dgv.GetCellDisplayRectangle(dgv.CurrentRow.Cells(colName).ColumnIndex, dgv.CurrentRow.Cells(colName).RowIndex, True)
                btn.Parent = dgv
                btn.Location = New Point(rect.Right - btn.Width, rect.Top)
                btn.Visible = If(isVisb = 0, True, False)
            Else
                btn.Visible = False
            End If
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            'MsgBox(ex.Message & " ERR_NO 77-10 : myEllipseButton")
        End Try
    End Sub
    Sub myAutoChk(ByVal dgv As DataGridView, ByVal colName As String, ByVal chkbx As CheckBox)
        Try
            Dim rect As Rectangle = dgv.GetCellDisplayRectangle(dgv.Rows(0).Cells(colName).ColumnIndex, dgv.Rows(0).Cells(colName).RowIndex, True)
            chkbx.Parent = dgv
            chkbx.Location = New Point(rect.Right - chkbx.Width, rect.Top - ((dgv.ColumnHeadersHeight / 2) + 5))
            chkbx.Visible = True
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " ERR_NO 77-10 : myAutoChk")
        End Try
    End Sub
    Sub previewImage(ByVal colName As String, ByVal dgv As DataGridView, ByVal pb As PictureBox)
        Try
            For Each r As DataRow In prodImage.Rows
                If dgv.CurrentRow.IsNewRow = False Then
                    If r(0).ToString = Trim(dgv.CurrentRow.Cells(colName).Value) Then
                        pb.Image = ConvertByteToImage(DirectCast(r(1), Byte()))
                        pb.SizeMode = PictureBoxSizeMode.StretchImage
                        Exit For
                    Else
                        pb.Image = Nothing
                    End If
                Else
                    pb.Image = Nothing
                    Exit For
                End If
            Next
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            'MsgBox(getErrExcptn(ex) & " myModule")
        End Try
    End Sub
    Public Function ConvByteToImage(ByVal ImgByte As Byte()) As Image
        Try
            Dim stream As System.IO.MemoryStream
            Dim img As Image
            stream = New System.IO.MemoryStream(ImgByte)
            img = Image.FromStream(stream)
            Return img
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            Return Nothing
        End Try
    End Function
    Public tsb_shmabut As New ToolStripButton

    Public Function AdjustComboBoxWidth(ByVal sender As Object, ByVal e As EventArgs)
        Dim senderComboBox = DirectCast(sender, ComboBox)
        Dim width As Integer = senderComboBox.DropDownWidth
        Dim g As Graphics = senderComboBox.CreateGraphics()
        Dim font As Font = senderComboBox.Font

        Dim vertScrollBarWidth As Integer = If((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0)

        Dim newWidth As Integer
        For Each s As String In DirectCast(sender, ComboBox).Items
            newWidth = CInt(g.MeasureString(s, font).Width) + vertScrollBarWidth
            If width < newWidth Then
                width = newWidth
            End If
        Next

        senderComboBox.DropDownWidth = width
        Return False
    End Function

    Sub clearObjControl(ByVal obj As Object)
        Try
            For Each r As Control In obj.Controls
                If TypeOf r Is System.Windows.Forms.TextBox Then
                    DirectCast(r, TextBox).Text = ""
                ElseIf TypeOf r Is System.Windows.Forms.ComboBox Then
                    DirectCast(r, ComboBox).Text = ""
                    If DirectCast(r, ComboBox).DropDownStyle = ComboBoxStyle.DropDownList Then
                        DirectCast(r, ComboBox).SelectedIndex = -1
                    End If
                ElseIf TypeOf r Is System.Windows.Forms.CheckBox Then
                    DirectCast(r, CheckBox).Checked = False
                ElseIf TypeOf r Is System.Windows.Forms.MaskedTextBox Then
                    DirectCast(r, MaskedTextBox).Text = ""
                ElseIf TypeOf r Is System.Windows.Forms.PictureBox Then
                    DirectCast(r, PictureBox).Image = Nothing
                ElseIf TypeOf r Is System.Windows.Forms.DataGridView Then
                    DirectCast(r, DataGridView).Rows.Clear()

                End If
            Next
            hasERR = 0
        Catch ex As Exception
            hasERR = 1

        End Try

    End Sub

    Public txtSimpleSearch As New TextBox

    Sub txtSimpleSearchProp(ByVal tbPage As TabPage)

        'tbPage.Controls.Add(txtSimpleSearch)

        With txtSimpleSearch
            .Name = "txtSimpleSearch"
            .Width = tbPage.Width - ((tbPage.Width / 10)) ' * 2

            .Location = New Point(tbPage.Width / 5, tbPage.Height / 4) ', subParent.Height / 2
            .BringToFront()
        End With
    End Sub

    Dim dgvleft As New DataGridView

    Dim formFunctn, cmdSearch, s_query As String
    Public Function myAutoComp(ByVal dgv As DataGridView) As AutoCompleteStringCollection
        'If simpleSearchAutoComp.Count >= 1 Then
        '    simpleSearchAutoComp.Clear()
        'End If
        Dim paramAutoComp As New AutoCompleteStringCollection
        'Dim valAdd As String
        If dgv.RowCount >= 1 Then
            For Each r As DataGridViewRow In dgv.Rows
                For i = 0 To dgv.ColumnCount - 1
                    If r.Cells(i).Visible = True Then
                        paramAutoComp.Add(r.Cells(i).Value.ToString)

                        'If TypeOf r.Cells(i).Value Is Integer Then
                        '    valAdd = CStr(r.Cells(i).Value)
                        'Else
                        '    valAdd = r.Cells(i).Value.ToString
                        'End If

                        'paramAutoComp.Add(valAdd)
                    End If
                Next
            Next

            Return paramAutoComp

        Else
            Return Nothing
        End If

    End Function

    Sub mytxtSimpleSearchProp(ByVal theTxtBx As TextBox, _
                              ByVal dgv As DataGridView, _
                              ByVal formFunc As String, _
                              Optional s_quer As String = Nothing, _
                               Optional dgvAutoComp As AutoCompleteStringCollection = Nothing, _
                               Optional withKeyPress As SByte = Nothing)
        formFunctn = formFunc

        s_query = s_quer

        dgvleft = dgv

        txtSimpleSearch = theTxtBx

        With theTxtBx

            .AutoCompleteCustomSource = If(dgvAutoComp Is Nothing, simpleSearchAutoComp, dgvAutoComp)

            .AutoCompleteMode = AutoCompleteMode.Suggest
            .AutoCompleteSource = AutoCompleteSource.CustomSource

            '.Show()

            '.Width = 220
            '.Location = point
            'AddHandler theTxtBx.TextChanged, AddressOf txtSimpleSearch_event
            If withKeyPress = Nothing Then
                AddHandler theTxtBx.KeyDown, AddressOf txtSimpleSearch_KeyEventArgs
            End If

        End With

    End Sub

    'Sub txtSimpleSearch_event(sender As Object, e As EventArgs)
    '    'With txtSimpleSearch
    '    '    MsgBox("cmdSearch")
    '    '    'cmdSearch = txtSimpleSearch.Text

    '    cmdSearch = DirectCast(sender, TextBox).Text

    '    'End With
    'End Sub

    Public kEnter As New KeyEventArgs(Keys.Enter)

    Public dgvEvArgs As New DataGridViewCellEventArgs(0, 0)

    Sub txtSimpleSearch_KeyEventArgs(sender As Object, e As KeyEventArgs)
        Try
            cmdSearch = DirectCast(sender, TextBox).Text

            With txtSimpleSearch
                If e.KeyCode = Keys.Enter And cmdSearch <> "" Then
                    'MsgBox("txtSimpleSearch_KeyEventArgs")

                    'order Number OR Related order Number
                    If IsNumeric(.Text) = True Then
                        If formFunctn = "Delivery Report" Then

                            fillDGV(s_query & " o.OrderNumber = (SELECT OrderNumber FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='" & formFunctn & "' AND OrganizationID=" & orgztnID & ")" & _
                                " AND o.Type='" & formFunctn & "'" & _
                                " OR o.RelatedMRFId = (SELECT RowID FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='Merchandise Requisition' AND OrganizationID=" & orgztnID & ")" & _
                                " AND o.Type='" & formFunctn & "'" & _
                                " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                        ElseIf formFunctn = "Merchandise Requisition" Then

                            fillDGV(s_query & " o.OrderNumber = (SELECT OrderNumber FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='" & formFunctn & "' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " OR o.RelatedDRId = (SELECT RowID FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='Delivery Report' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                        ElseIf formFunctn = "Packing List" Then

                            fillDGV(s_query & " o.OrderNumber = (SELECT OrderNumber FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='" & formFunctn & "' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " OR o.RelatedPRSId = (SELECT RowID FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='Purchase Requisition' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                        ElseIf formFunctn = "Purchase Requisition" Then

                            fillDGV(s_query & " o.OrderNumber = (SELECT OrderNumber FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='" & formFunctn & "' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " OR o.RelatedPLId = (SELECT RowID FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='Packing List' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                        ElseIf formFunctn = "Receiving Report" Then

                            fillDGV(s_query & " o.OrderNumber = (SELECT OrderNumber FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='" & formFunctn & "' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " OR o.RelatedPRSId = (SELECT RowID FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='Purchase Requisition' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " OR o.RelatedPLId = (SELECT RowID FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='Packing List' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                        ElseIf formFunctn = "Repairs" Then

                            fillDGV(s_query & " r.RepairNo = (SELECT RepairNo FROM repairs WHERE RepairNo=" & Trim(.Text) & " AND OrganizationID=" & orgztnID & " GROUP BY RepairNo)" & _
                                    " OR r.ReferenceNumber = (SELECT RowID FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND RowID LIKE r.ReferenceNumber LIMIT 1)" & _
                                    " AND r.OrganizationID=" & orgztnID & " GROUP BY r.RepairNo ORDER BY r.RepairNo DESC", dgvleft)

                            'DI MO PA NAGAGAWA UNG SA ReferenceNumber sa repairs Simple Search

                            'End If

                        ElseIf formFunctn = "Supplier" Then

                            'kung Cellphone Number ba?
                            'kung FaxNumber ba?

                            fillDGV(s_query & " ac.MainPhone = '" & .Text & "'" & _
                                    " AND ac.OrganizationID='" & orgztnID & "' AND ac.AccountType='Supplier' ORDER BY ac.CompanyName ASC", dgvleft)

                        ElseIf formFunctn = "Stock Adjustment" Then

                            fillDGV(s_query & " o.OrderNumber=(SELECT OrderNumber FROM `order` WHERE OrderNumber=" & Trim(.Text) & " AND Type='" & formFunctn & "' AND OrganizationID=" & orgztnID & ")" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft)

                        End If
                        'order Date OR Creation
                    ElseIf IsDate(.Text) = True Then

                        If formFunctn = "Repairs" Then

                            'Dim rprType As String = If(Repairs.ComboBox1.SelectedIndex = -1, "", _
                            '                           If(Repairs.ComboBox1.SelectedIndex = 0, " AND r.Type='Customer Issue'", _
                            '                                " AND r.Type='Shipment Issue'"
                            '                             )
                            '                          )

                            fillDGV(s_query & " r.Created LIKE '%" & .Text & "%'" & _
                                    " OR r.LastUpd LIKE '%" & .Text & "%'" & _
                                    " AND r.OrganizationID=" & orgztnID & " GROUP BY r.RepairNo ORDER BY r.RepairNo DESC", dgvleft)
                        ElseIf formFunctn = "Supplier" Then

                            fillDGV(s_query & " ac.Created LIKE '" & .Text & "'" & _
                                    " AND ac.AccountType='Supplier'" & _
                                    " OR ac.LastUpd LIKE '" & .Text & "'" & _
                                    " AND ac.OrganizationID='" & orgztnID & "' AND ac.AccountType='Supplier' ORDER BY ac.CompanyName ASC", dgvleft)

                        Else

                            Dim theDate As Date = Date.Parse(.Text)
                            Dim _dat = Format(theDate, "yyyy-MM-dd")

                            fillDGV(s_query & " o.OrderDate LIKE '%" & _dat & "%'" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                            '" OR o.Created LIKE '%" & _dat & "%'" & _
                            '" AND o.Type='" & formFunctn & "'" & _
                        End If

                        'user
                    ElseIf EXECQUER("SELECT EXISTS(SELECT RowID FROM user WHERE OrganizationID=" & orgztnID & _
                                " AND CONCAT(LastName,', ',FirstName) = '" & .Text & "')") = 1 Then
                        'tinignan ulit kung may match sa contact
                        If EXECQUER("SELECT EXISTS(SELECT RowID FROM contact WHERE OrganizationID=" & orgztnID & _
                                    " AND CONCAT(LastName,', ',FirstName) = '" & .Text & "')") = 1 Then

                            If formFunctn = "Repairs" Then

                                fillDGV(s_query & " r.CreatedBy = (SELECT RowID FROM user WHERE CONCAT(LastName,', ',FirstName) = '" & .Text & "')" & _
                                        " OR r.LastUpdBy = (SELECT RowID FROM user WHERE CONCAT(LastName,', ',FirstName) = '" & .Text & "')" & _
                                        " AND r.OrganizationID=" & orgztnID & " GROUP BY r.RepairNo ORDER BY r.RepairNo DESC", dgvleft)

                            Else

                                fillDGV(s_query & " o.ContactID=(SELECT RowID FROM contact WHERE CONCAT(LastName,', ',FirstName) = '" & .Text & "')" & _
                                        " AND o.Type='" & formFunctn & "'" & _
                                        " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                            End If

                        Else 'dito 'True' nga na galing sa user table
                            If formFunctn = "Repairs" Then

                                fillDGV(s_query & " CONCAT(u.LastName ,',',u.FirstName) = '" & .Text & "'" & _
                                        " OR ac.LastUpdBy = (SELECT RowID FROM user WHERE CONCAT(LastName,', ',FirstName) = '" & .Text & "')" & _
                                        "' AND ac.OrganizationID='" & orgztnID & "' AND ac.AccountType='Supplier' ORDER BY ac.CompanyName ASC", dgvleft)

                            Else

                                fillDGV(s_query & " o.CreatedBy = (SELECT RowID FROM user WHERE CONCAT(LastName,', ',FirstName) = '" & .Text & "')" & _
                                        " AND o.Type='" & formFunctn & "'" & _
                                        " OR o.LastUpdBy = (SELECT RowID FROM user WHERE CONCAT(LastName,', ',FirstName) = '" & .Text & "')" & _
                                        " AND o.Type='" & formFunctn & "'" & _
                                        " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                            End If
                        End If

                        'contact
                    ElseIf EXECQUER("SELECT EXISTS(SELECT RowID FROM contact WHERE OrganizationID=" & orgztnID & _
                                    " AND CONCAT(LastName,', ',FirstName) = '" & .Text & "')") = 1 Then

                        If formFunctn = "Repairs" Then

                        ElseIf formFunctn = "Supplier" Then

                            fillDGV(s_query & " CONCAT(c.LastName ,', ',c.FirstName) = '" & Trim(.Text) & _
                                    "' AND ac.OrganizationID='" & orgztnID & "' AND ac.AccountType='Supplier' ORDER BY ac.CompanyName ASC", dgvleft)

                        Else

                            fillDGV(s_query & " o.ContactID=(SELECT RowID FROM contact WHERE CONCAT(LastName,', ',FirstName) = '" & .Text & "')" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                        End If

                        'inventorylocation
                    ElseIf EXECQUER("SELECT EXISTS(SELECT RowID FROM inventorylocation WHERE OrganizationID=" & orgztnID & _
                                " AND Name = '" & .Text & "')") = 1 Then

                        If formFunctn = "Repairs" Then

                            fillDGV(s_query & " r.InventoryLocationID=(SELECT RowID FROM inventorylocation WHERE Name = '" & .Text & "' AND Status='Active' AND Type='Main')" & _
                                    " AND r.OrganizationID=" & orgztnID & " GROUP BY r.RepairNo ORDER BY r.RepairNo DESC", dgvleft)

                        Else

                            'AND Type!='Main'
                            fillDGV(s_query & " o.InventoryLocationID=(SELECT RowID FROM inventorylocation WHERE Name = '" & .Text & "' AND Status='Active')" & _
                                    " AND o.Type='" & formFunctn & "'" & _
                                    " AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                        End If

                    Else 'order Status

                        If formFunctn = "Repairs" Then

                            fillDGV(s_query & " r.Type = '" & Trim(.Text) & _
                                    "' AND r.OrganizationID=" & orgztnID & " GROUP BY r.RepairNo ORDER BY r.RepairNo DESC", dgvleft)

                        ElseIf formFunctn = "Supplier" Then

                            If .Text.Contains("@") = True Then
                                'kung Email Address ba?     'If str.Contains("TOP") = True Then

                                fillDGV(s_query & " ac.EmailAddress = '" & .Text & "'" & _
                                        " AND ac.OrganizationID='" & orgztnID & "' AND ac.AccountType='Supplier' ORDER BY ac.CompanyName ASC", dgvleft)

                            Else
                                'kung Address ba?
                                'CONCAT(COALESCE(StreetAddress1,''),', ',COALESCE(StreetAddress2,''),', ',COALESCE(Barangay,''),', ',COALESCE(CityTown,''),', ',COALESCE(State,''),', ',COALESCE(Country,''),', ',COALESCE(ZipCode,''))

                                fillDGV(s_query & " ac.CompanyName = '" & Trim(.Text) & "'" & _
                                    " AND ac.OrganizationID='" & orgztnID & "' AND ac.AccountType='Supplier' ORDER BY ac.CompanyName ASC", dgvleft)

                            End If

                        Else

                            fillDGV(s_query & " o.`Status` = '" & Trim(.Text) & _
                                "' AND o.Type='" & formFunctn & "'" & _
                                " OR o.StatusAsOf = '" & Trim(.Text) & _
                                "' AND o.Type='" & formFunctn & "'" & _
                                " AND o.Type='" & formFunctn & "' AND o.OrganizationID=" & orgztnID & " ORDER BY CAST(o.OrderNumber AS INT) DESC", dgvleft, 1)

                        End If

                    End If

                End If

            End With

            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " ERR_NO 77-10 : txtSimpleSearch_KeyEventArgs", MsgBoxStyle.Critical, "Unexpected Message")
        End Try

    End Sub

    Sub fillDGV(ByVal cmdstrSearch As String, ByVal dgv As DataGridView, Optional hideCol As Byte = Nothing)

        If conn.State = ConnectionState.Open Then : conn.Close() : End If
        conn.Open()
        cmd = New MySqlCommand
        With cmd
            .CommandType = CommandType.Text
            .Connection = conn
            .CommandText = cmdstrSearch

            'MsgBox(cmdstrSearch)

        End With

        filltable(dgv)

        If hideCol <> Nothing Then
            For i = 0 To hideCol - 1
                dgv.Columns(i).Visible = False

                dgv.Columns(i).SortMode = If(i <= hideCol, DataGridViewColumnSortMode.Automatic, DataGridViewColumnSortMode.NotSortable)
            Next
        Else
            For i = 0 To hideCol - 1
                dgv.Columns(i).Visible = False

                dgv.Columns(i).SortMode = If(i < hideCol, DataGridViewColumnSortMode.Automatic, DataGridViewColumnSortMode.NotSortable)
            Next
        End If

        cmd.Dispose()
    End Sub

    Public tsbtnExportToExcel As New ToolStripButton

    Public mySaveFileDialog As New SaveFileDialog

    Dim savePath As String

    Sub tsbtnExportToExcel_Click(sender As Object, e As EventArgs)
        Try

            Static l As SByte = 0
            If l <> 1 Then
                l = 1
                mySaveFileDialog.RestoreDirectory = True
                mySaveFileDialog.Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" & _
                                          "Microsoft Excel Documents 97-2003 (*.xls)|*.xls|" & _
                                          "OpenDocument Spreadsheet (*.ods)|*.ods" ' & _
            End If

            If dgvtargetforExport.RowCount = 0 Then ' - 1
                Exit Sub
            End If

            '"All files (*.*)|*.*"

            If mySaveFileDialog.ShowDialog() = DialogResult.OK Then

                Dim mainBranchID = EXECQUER(SYS_MAIN_BRNCH_ID & " AND OrganizationID=" & orgztnID)

                savePath = Path.GetFullPath(mySaveFileDialog.FileName)

                Try

                    Dim dat_tab As New DataTable
                    dat_tab = getDataTableForSQL("SELECT COALESCE(p.PartNo,'') PartNo" & _
                                                 ",COALESCE(p.Name,'') Name" & _
                                                 ",COALESCE(p.Category,'') Category" & _
                                                 ",COALESCE(p.Catalog,'') Catalog" & _
                                                 ",IF(SUBSTRING_INDEX(FORMAT(p.UnitPrice,2),'.',-1)<1,FORMAT(p.UnitPrice,0),FORMAT(p.UnitPrice,2)) UnitPrice" & _
                                                 ",COALESCE(p.TotalShipmentCount,0) TotalShipmentCount" & _
                                                 ",COALESCE((SELECT SUM(TotalAvailbleItemQty) FROM productinventorylocation WHERE ProductID=p.RowID),'') Total_Available_Qty" & _
                                                 ",COALESCE(DATE_FORMAT(p.LastRcvdFromShipmentDate,'%m/%d/%Y'),'') Last_Shipment_Date" & _
                                                 ",COALESCE(p.LastRcvdFromShipmentCount ,0) Last_Shipment_Count" & _
                                                 ",COALESCE(p.ReOrderPoint ,0) Re_Order_Point" & _
                                                 ",COALESCE(p.BookPageNo ,'') Book_Page_No" & _
                                                 ",COALESCE(p.BrandName ,'') BrandName" & _
                                                 ",COALESCE(p.UnitOfMeasure ,'') UnitOfMeasure" & _
                                                 " FROM product p " & _
                                                 "JOIN productinventorylocation pil " & _
                                                 "ON pil.ProductID=p.RowID" & _
                                                 " WHERE p.OrganizationID=" & orgztnID & " AND pil.InventoryLocationID='" & mainBranchID & _
                                                 "' GROUP BY p.RowID ORDER BY p.PartNo ASC;")

                    If mySaveFileDialog.FilterIndex = 1 Then
                        Dim xlApp As Excel.Application
                        Dim xlWorkBook As Excel.Workbook
                        Dim xlWorkSheet As Excel.Worksheet
                        Dim misValue As Object = System.Reflection.Missing.Value

                        xlApp = New Excel.Application
                        xlWorkBook = xlApp.Workbooks.Add(misValue)
                        xlWorkSheet = xlWorkBook.Sheets("Sheet1")
                        Dim i As Integer
                        Dim xclstr As String
                        With xlWorkSheet
                            i = 2
                            .Cells(1, 1) = "Item Code"
                            .Cells(1, 2) = "Name"
                            .Cells(1, 3) = "Category"
                            .Cells(1, 4) = "Catalog"
                            .Cells(1, 5) = "SRP"
                            .Cells(1, 6) = "Total_Available_Qty"
                            .Cells(1, 7) = "Total_Shipment_Count"
                            .Cells(1, 8) = "Last_Shipment_Date"
                            .Cells(1, 9) = "Last_Shipment_Count"
                            .Cells(1, 10) = "Re_Order_Point"
                            .Cells(1, 11) = "Book_Page_No"
                            .Cells(1, 12) = "Brand_Name"
                            .Cells(1, 13) = "Unit_of_Measure"

                            For Each r As DataRow In dat_tab.Rows
                                .Cells(i, 1) = r("PartNo").ToString
                                .Cells(i, 2) = r("Name").ToString
                                .Cells(i, 3) = r("Category").ToString
                                .Cells(i, 4) = r("Catalog").ToString
                                .Cells(i, 5) = r("UnitPrice").ToString

                                xclstr = If(IsDBNull(r("Total_Available_Qty")), "", _
                                             r("Total_Available_Qty"))

                                .Cells(i, 6) = xclstr
                                .Cells(i, 7) = r("TotalShipmentCount").ToString
                                .Cells(i, 8) = r("Last_Shipment_Date").ToString
                                .Cells(i, 9) = r("Last_Shipment_Count").ToString
                                .Cells(i, 10) = r("Re_Order_Point").ToString
                                .Cells(i, 11) = r("Book_Page_No").ToString
                                .Cells(i, 12) = r("BrandName").ToString
                                .Cells(i, 13) = r("UnitOfMeasure").ToString

                                i = i + 1
                            Next
                        End With

                        xlWorkSheet.SaveAs(savePath)

                        'Dim ws As Excel.Worksheet
                        'Dim lRow As Long
                        'ws = xlWorkBook.Sheets("Sheet1")
                        'lRow = ws.Range("A" & ws.Rows.Count).End(Excel.XlDirection.xlUp).Row'Gets the row count of excel file
                        'MsgBox("The last row which has data in Col A of Sheet1 is " & lRow)
                        'lRow = ws.Range(1, ws.Columns.Count).End(Excel.XlDirection.xlToLeft).Column'Gets the column count of excel file
                        'MsgBox("The last Column which has data in Row 1 of Sheet1 is " & lRow)

                        xlWorkBook.Close()
                        xlApp.Quit()

                        releaseObject(xlApp)
                        releaseObject(xlWorkBook)
                        releaseObject(xlWorkSheet)
                    Else

                        Dim stream = New FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite) ', FileAccess.ReadWrite

                        Dim writer = New ExcelWriter(stream)

                        Dim cellStr As String
                        With writer

                            .BeginWrite()

                            .WriteCell(0, 0, "Item Code")
                            .WriteCell(0, 1, "Name")
                            .WriteCell(0, 2, "Category")
                            .WriteCell(0, 3, "Catalog")
                            .WriteCell(0, 4, "SRP")
                            .WriteCell(0, 5, "Total_Available_Qty")
                            .WriteCell(0, 6, "Total_Shipment_Count")
                            .WriteCell(0, 7, "Last_Shipment_Date")
                            .WriteCell(0, 8, "Last_Shipment_Count")
                            .WriteCell(0, 9, "Re_Order_Point")
                            .WriteCell(0, 10, "Book_Page_No")
                            .WriteCell(0, 11, "Brand_Name")
                            .WriteCell(0, 12, "Unit_of_Measure")

                            Dim n = 1
                            For Each r As DataRow In dat_tab.Rows
                                .WriteCell(n, 0, r("PartNo").ToString)
                                .WriteCell(n, 1, r("Name").ToString)
                                .WriteCell(n, 2, r("Category").ToString)
                                .WriteCell(n, 3, r("Catalog").ToString)
                                .WriteCell(n, 4, r("UnitPrice").ToString)

                                'cellStr = If(IsDBNull(r("Total_Available_Qty")), "", _
                                '                     r("Total_Available_Qty"))

                                cellStr = r("Total_Available_Qty")

                                .WriteCell(n, 5, cellStr)

                                .WriteCell(n, 6, r("TotalShipmentCount").ToString)
                                .WriteCell(n, 7, r("Last_Shipment_Date").ToString)
                                .WriteCell(n, 8, r("Last_Shipment_Count").ToString)
                                .WriteCell(n, 9, r("Re_Order_Point").ToString)
                                .WriteCell(n, 10, r("Book_Page_No").ToString)
                                .WriteCell(n, 11, r("BrandName").ToString)
                                .WriteCell(n, 12, r("UnitOfMeasure").ToString)

                                n = n + 1
                            Next

                            .EndWrite()
                        End With

                    End If

                    Dim res As MsgBoxResult

                    res = MsgBox("Process completed!" & vbNewLine & "Would you like to Open " & Path.GetFileName(savePath) & " ?", MsgBoxStyle.YesNo, "Done Exporting")

                    If (res = MsgBoxResult.Yes) Then
                        Process.Start(savePath)
                    End If

                    hasERR = 0
                Catch ex As Exception
                    hasERR = 1
                    MsgBox(ex.Message & " ERR_NO  77-10 : tsbtnExportToExcel_Click", , "Unexepected Message")
                Finally

                End Try

            End If

            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message, "Error code tsbtnExportToExcel_Click")
        End Try
    End Sub

    'Public Function getDataTableForSQL(ByVal COMMD As String)
    '    Dim command As MySqlCommand = New MySqlCommand(COMMD, conn)

    '    Try
    '        Dim DataReturn As New DataTable
    '        '    Dim sql As String = COMMD

    '        command.Connection.Open()


    '        Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(command)
    '        adapter.Fill(DataReturn)
    '        command.Connection.Close()
    '        Return DataReturn
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '        Return Nothing
    '    Finally
    '        command.Connection.Close()
    '    End Try
    'End Function

    Dim dgvtargetforExport As DataGridView
    Sub callExcelExportBtn(Optional tlstrp As ToolStrip = Nothing, Optional dgv As DataGridView = Nothing, Optional grupby As String = Nothing)

        tsbtnExportToExcel.Name = "tsbtnExportToExcel"
        tsbtnExportToExcel.Text = "E&xport Products"

        tlstrp.Items.Add(tsbtnExportToExcel)

        AddHandler tsbtnExportToExcel.Click, AddressOf tsbtnExportToExcel_Click

        dgvtargetforExport = New DataGridView
        dgvtargetforExport = dgv

    End Sub

    Public Delegate Sub delegateCaller(sender As Object, e As EventArgs)

    Event AnEvent As delegateCaller

    Public mydele As delegateCaller

    'Public _Sub

    Public Event An_Event()

    Sub INS_LoL(Optional DisplayValue As Object = Nothing, _
                Optional LIC As Object = Nothing, _
                Optional Type As Object = Nothing, _
                Optional ParentLIC As Object = Nothing, _
                Optional Active As Object = Nothing, _
                Optional Description As Object = Nothing, _
                Optional Created As Object = Nothing, _
                Optional CreatedBy As Object = Nothing, _
                Optional LastUpd As Object = Nothing, _
                Optional OrderBy As Object = Nothing, _
                Optional LastUpdBy As Object = Nothing)

        EXECQUER("INSERT INTO listofval (DisplayValue,LIC,Type,ParentLIC,Active,Description,Created,CreatedBy,LastUpd,OrderBy," & _
                             "LastUpdBy) VALUES (" & _
                             "'" & DisplayValue & "'" & _
                             "," & If(LIC = Nothing, "NULL", "'" & LIC & "'") & _
                             "," & If(Type = Nothing, "NULL", "'" & Type & "'") & _
                             "," & If(ParentLIC = Nothing, "NULL", "'" & ParentLIC & "'") & _
                             "," & If(Active = Nothing, "NULL", "'" & Active & "'") & _
                             "," & If(Description = Nothing, "NULL", "'" & Description & "'") & _
                             ",DATE_FORMAT(NOW(),'%Y-%m-%d %h:%i:%s')" & _
                             "," & CreatedBy & _
                             ",DATE_FORMAT(NOW(),'%Y-%m-%d %h:%i:%s')" & _
                             "," & If(OrderBy = Nothing, "NULL", OrderBy) & _
                             "," & CreatedBy & _
                             ");")


    End Sub

    Function INS_paysocialsecurity(Optional RangeFromAmount As Object = Nothing, _
                Optional RangeToAmount As Object = Nothing, _
                Optional MonthlySalaryCredit As Object = Nothing, _
                Optional EmployeeContributionAmount As Object = Nothing, _
                Optional EmployerContributionAmount As Object = Nothing, _
                Optional EmployeeECAmount As Object = Nothing) As String

        'RangeFromAmount = If(RangeFromAmount = Nothing, "NULL", RangeFromAmount)
        'RangeToAmount = If(RangeToAmount = Nothing, "NULL", RangeToAmount)
        MonthlySalaryCredit = If(MonthlySalaryCredit = Nothing, "NULL", MonthlySalaryCredit)
        EmployeeContributionAmount = If(EmployeeContributionAmount = Nothing, "NULL", EmployeeContributionAmount)
        EmployerContributionAmount = If(EmployerContributionAmount = Nothing, "NULL", EmployerContributionAmount)
        EmployeeECAmount = If(EmployeeECAmount = Nothing, "NULL", EmployeeECAmount)

        Dim getpaysocialsecurity = EXECQUER("INSERT INTO paysocialsecurity (CreatedBy,LastUpdBy,RangeFromAmount,RangeToAmount,MonthlySalaryCredit,EmployeeContributionAmount," & _
        "EmployerContributionAmount,EmployeeECAmount) VALUES (" & _
        "" & z_User & _
        "," & z_User & _
        "," & RangeFromAmount & _
        "," & RangeToAmount & _
        "," & MonthlySalaryCredit & _
        "," & EmployeeContributionAmount & _
        "," & EmployerContributionAmount & _
        "," & EmployeeECAmount & _
        ");") ' & _
        '"SELECT COALESCE(RowID,'') FROM paysocialsecurity " & _
        '"WHERE Createdby=" & 1 & _
        '" AND LastUpdBy=" & 1 & _
        '" AND RangeFromAmount=" & RangeFromAmount & _
        '" AND RangeToAmount=" & RangeToAmount & _
        '" AND MonthlySalaryCredit=" & MonthlySalaryCredit & _
        '" AND EmployeeContributionAmount=" & EmployeeContributionAmount & _
        '" AND EmployerContributionAmount=" & EmployerContributionAmount & _
        '" AND EmployeeECAmount=" & EmployeeECAmount & ";"
        Return getpaysocialsecurity

    End Function

    Function INS_payphilhealth(Optional SalaryBracket As Object = Nothing, _
                Optional SalaryRangeFrom As Object = Nothing, _
                Optional SalaryRangeTo As Object = Nothing, _
                Optional SalaryBase As Object = Nothing, _
                Optional TotalMonthlyPremium As Object = Nothing, _
                Optional EmployeeShare As Object = Nothing, _
                Optional EmployerShare As Object = Nothing) As String

        SalaryBracket = If(SalaryBracket = Nothing, "NULL", SalaryBracket)
        SalaryRangeFrom = If(SalaryRangeFrom = Nothing, "NULL", SalaryRangeFrom)
        SalaryRangeTo = If(SalaryRangeTo = Nothing, "NULL", SalaryRangeTo)
        SalaryBase = If(SalaryBase = Nothing, "NULL", SalaryBase)
        TotalMonthlyPremium = If(TotalMonthlyPremium = Nothing, "NULL", TotalMonthlyPremium)
        EmployeeShare = If(EmployeeShare = Nothing, "NULL", EmployeeShare)
        EmployerShare = If(EmployerShare = Nothing, "NULL", EmployerShare)
        'TotalMonthlyPremium,
        Dim getpaysocialsecurity = EXECQUER("INSERT INTO payphilhealth (CreatedBy,LastUpdBy,SalaryBracket,SalaryRangeFrom,SalaryRangeTo,SalaryBase," & _
        "EmployeeShare,EmployerShare) VALUES (" & _
        "" & z_User & _
        "," & z_User & _
        "," & SalaryBracket & _
        "," & SalaryRangeFrom & _
        "," & SalaryRangeTo & _
        "," & SalaryBase & _
        "," & EmployeeShare & _
        "," & EmployerShare & _
        ");" & _
        "SELECT RowID " & _
        "FROM payphilhealth " & _
        "WHERE SalaryBracket=" & SalaryBracket & _
        " AND SalaryRangeFrom=" & SalaryRangeFrom & _
        " AND SalaryRangeTo=" & SalaryRangeTo & _
        " AND SalaryBase=" & SalaryBase & _
        " AND EmployeeShare=" & EmployeeShare & _
        " AND EmployerShare=" & EmployerShare & _
        " AND CreatedBy=" & z_User)

        Return getpaysocialsecurity
    End Function

    Public Function INS_employee(Optional EmployeeID As Object = Nothing, Optional EmploymentStatus As Object = Nothing, Optional Gender As Object = Nothing, _
                Optional JobTitle As Object = Nothing, Optional PositionID As Object = Nothing, Optional Salutation As Object = Nothing, _
                Optional FirstName As Object = Nothing, Optional MiddleName As Object = Nothing, Optional LastName As Object = Nothing, _
                Optional Nickname As Object = Nothing, Optional Birthdate As Object = Nothing, Optional TINNo As Object = Nothing, _
                Optional SSSNo As Object = Nothing, Optional HDMFNo As Object = Nothing, Optional PhilHealthNo As Object = Nothing, _
                Optional EmailAddress As Object = Nothing, Optional WorkPhone As Object = Nothing, Optional HomePhone As Object = Nothing, _
                Optional MobilePhone As Object = Nothing, Optional HomeAddress As Object = Nothing, Optional PayFrequencyID As Object = Nothing, _
                Optional UndertimeOverride As Object = Nothing, Optional OvertimeOverride As Object = Nothing, Optional Surname As Object = Nothing, _
                Optional MaritalStatus As Object = Nothing, Optional NoOfDependents As String = Nothing, Optional LeavePerPayPeriod As String = Nothing, _
                Optional EmployeeType As String = Nothing) As String '

        JobTitle = If(JobTitle = Nothing, "NULL", "'" & JobTitle & "'") : Salutation = If(Salutation = Nothing, "NULL", "'" & Salutation & "'")
        Nickname = If(Nickname = Nothing, "NULL", "'" & Nickname & "'") : TINNo = If(TINNo = Nothing, "NULL", "'" & TINNo & "'")
        SSSNo = If(SSSNo = Nothing, "NULL", "'" & SSSNo & "'") : HDMFNo = If(HDMFNo = Nothing, "NULL", "'" & HDMFNo & "'")
        PhilHealthNo = If(PhilHealthNo = Nothing, "NULL", "'" & PhilHealthNo & "'") : EmailAddress = If(EmailAddress = Nothing, "NULL", "'" & EmailAddress & "'")
        WorkPhone = If(WorkPhone = Nothing, "NULL", "'" & WorkPhone & "'") : HomePhone = If(HomePhone = Nothing, "NULL", "'" & HomePhone & "'")
        MobilePhone = If(MobilePhone = Nothing, "NULL", "'" & MobilePhone & "'") : HomeAddress = If(HomeAddress = Nothing, "NULL", "'" & HomeAddress & "'")
        PayFrequencyID = If(PayFrequencyID = Nothing, "NULL", PayFrequencyID) : UndertimeOverride = If(UndertimeOverride = Nothing, "0", "'" & UndertimeOverride & "'")
        OvertimeOverride = If(OvertimeOverride = Nothing, "0", "'" & OvertimeOverride & "'") : PositionID = If(PositionID = Nothing, "NULL", PositionID)
        FirstName = If(FirstName = Nothing, "NULL", "'" & FirstName & "'") : MiddleName = If(MiddleName = Nothing, "NULL", "'" & MiddleName & "'")
        LastName = If(LastName = Nothing, "NULL", "'" & LastName & "'") : EmployeeID = If(EmployeeID = Nothing, "NULL", "'" & EmployeeID & "'")

        Surname = If(Surname = Nothing, "NULL", "'" & Surname & "'")
        MaritalStatus = If(MaritalStatus = Nothing, "NULL", "'" & MaritalStatus & "'")
        EmployeeType = If(EmployeeType = Nothing, "NULL", "'" & EmployeeType & "'")

        Dim getpaysocialsecurity = EXECQUER("INSERT INTO employee (CreatedBy,LastUpdBy,OrganizationID,EmployeeID,EmploymentStatus,Gender,JobTitle," & _
        "PositionID,Salutation,FirstName,MiddleName,LastName,Nickname,Birthdate,TINNo,SSSNo,HDMFNo,PhilHealthNo,EmailAddress,WorkPhone,HomePhone,MobilePhone,HomeAddress," & _
        "PayFrequencyID,UndertimeOverride,OvertimeOverride,Surname,MaritalStatus,NoOfDependents,LeavePerPayPeriod,EmployeeType) VALUES (" & _
        "" & z_User & _
        "," & z_User & _
        "," & orgztnID & _
        "," & EmployeeID & _
        ",'" & EmploymentStatus & _
        "','" & Gender & _
        "'," & JobTitle & _
        "," & PositionID & _
        "," & Salutation & _
        "," & FirstName & _
        "," & MiddleName & _
        "," & LastName & _
        "," & Nickname & _
        ",'" & Birthdate & _
        "'," & TINNo & _
        "," & SSSNo & _
        "," & HDMFNo & _
        "," & PhilHealthNo & _
        "," & EmailAddress & _
        "," & WorkPhone & _
        "," & HomePhone & _
        "," & MobilePhone & _
        "," & HomeAddress & _
        "," & PayFrequencyID & _
        "," & UndertimeOverride & _
        "," & OvertimeOverride & _
        "," & Surname & _
        "," & MaritalStatus & _
        "," & NoOfDependents & _
        "," & LeavePerPayPeriod & _
        "," & EmployeeType & _
        ");" & _
        "SELECT RowID " & _
        "FROM employee " & _
        "WHERE CreatedBy=" & z_User & _
        " AND LastUpdBy=" & z_User & _
        " AND OrganizationID=" & orgztnID & _
        " AND EmployeeID" & If(EmployeeID = "NULL", " IS NULL", "=" & EmployeeID) & _
        " AND EmploymentStatus='" & EmploymentStatus & _
        "' AND Gender='" & Gender & _
        "' AND JobTitle" & If(JobTitle = "NULL", " IS NULL", "=" & JobTitle) & _
        " AND PositionID" & If(PositionID = "NULL", " IS NULL", "=" & PositionID) & _
        " AND Salutation" & If(Salutation = "NULL", " IS NULL", "=" & Salutation) & _
        " AND FirstName" & If(FirstName = "NULL", " IS NULL", "=" & FirstName) & _
        " AND MiddleName" & If(MiddleName = "NULL", " IS NULL", "=" & MiddleName) & _
        " AND LastName" & If(LastName = "NULL", " IS NULL", "=" & LastName) & _
        " AND Nickname" & If(Nickname = "NULL", " IS NULL", "=" & Nickname) & _
        " AND Birthdate='" & Birthdate & _
        "' AND TINNo" & If(TINNo = "NULL", " IS NULL", "=" & TINNo) & _
        " AND SSSNo" & If(SSSNo = "NULL", " IS NULL", "=" & SSSNo) & _
        " AND HDMFNo" & If(HDMFNo = "NULL", " IS NULL", "=" & HDMFNo) & _
        " AND PhilHealthNo" & If(PhilHealthNo = "NULL", " IS NULL", "=" & PhilHealthNo) & _
        " AND EmailAddress" & If(EmailAddress = "NULL", " IS NULL", "=" & EmailAddress) & _
        " AND WorkPhone" & If(WorkPhone = "NULL", " IS NULL", "=" & WorkPhone) & _
        " AND HomePhone" & If(HomePhone = "NULL", " IS NULL", "=" & HomePhone) & _
        " AND MobilePhone" & If(MobilePhone = "NULL", " IS NULL", "=" & MobilePhone) & _
        " AND HomeAddress" & If(HomeAddress = "NULL", " IS NULL", "=" & HomeAddress) & _
        " AND PayFrequencyID" & If(PayFrequencyID = "NULL", " IS NULL", "=" & PayFrequencyID) & _
        " AND PayFrequencyID" & If(PayFrequencyID = "NULL", " IS NULL", "=" & PayFrequencyID) & _
        " AND UndertimeOverride=" & UndertimeOverride & _
        " AND OvertimeOverride=" & OvertimeOverride & _
        " AND Surname" & If(Surname = "NULL", " IS NULL", "=" & Surname) & _
        " AND MaritalStatus" & If(MaritalStatus = "NULL", " IS NULL", "=" & MaritalStatus) & _
        " AND NoOfDependents=" & NoOfDependents & _
        " AND LeavePerPayPeriod=" & LeavePerPayPeriod & _
        " AND EmployeeType" & If(EmployeeType = "NULL", " IS NULL", "=" & EmployeeType) & ";")

        Return getpaysocialsecurity
    End Function
    Function INS_employeedepen(Optional Salutation As Object = Nothing, Optional FirstName As Object = Nothing, _
                               Optional MiddleName As Object = Nothing, Optional LastName As Object = Nothing, _
                               Optional Surname As Object = Nothing, Optional ParentEmployeeID As Object = Nothing, _
                               Optional TINNo As Object = Nothing, Optional SSSNo As Object = Nothing, _
                               Optional HDMFNo As Object = Nothing, Optional PhilHealthNo As Object = Nothing, _
                               Optional EmailAddress As Object = Nothing, Optional WorkPhone As Object = Nothing, _
                               Optional HomePhone As Object = Nothing, Optional MobilePhone As Object = Nothing, _
                               Optional HomeAddress As Object = Nothing, Optional Nickname As Object = Nothing, _
                               Optional JobTitle As Object = Nothing, Optional Gender As Object = Nothing, _
                               Optional RelationToEmployee As Object = Nothing, Optional ActiveFlag As Object = Nothing, _
                               Optional Birthdate As Object = Nothing) As String

        Salutation = If(Salutation = Nothing, "NULL", "'" & Salutation & "'") : FirstName = If(FirstName = Nothing, "NULL", "'" & FirstName & "'")
        MiddleName = If(MiddleName = Nothing, "NULL", "'" & MiddleName & "'") : LastName = If(LastName = Nothing, "NULL", "'" & LastName & "'")
        Surname = If(Surname = Nothing, "NULL", "'" & Surname & "'") : TINNo = If(TINNo = Nothing, "NULL", "'" & TINNo & "'")
        SSSNo = If(SSSNo = Nothing, "NULL", "'" & SSSNo & "'") : HDMFNo = If(HDMFNo = Nothing, "NULL", "'" & HDMFNo & "'")
        PhilHealthNo = If(PhilHealthNo = Nothing, "NULL", "'" & PhilHealthNo & "'") : EmailAddress = If(EmailAddress = Nothing, "NULL", "'" & EmailAddress & "'")
        WorkPhone = If(WorkPhone = Nothing, "NULL", "'" & WorkPhone & "'") : HomePhone = If(HomePhone = Nothing, "NULL", "'" & HomePhone & "'")
        MobilePhone = If(MobilePhone = Nothing, "NULL", "'" & MobilePhone & "'") : HomeAddress = If(HomeAddress = Nothing, "NULL", "'" & HomeAddress & "'")
        Nickname = If(Nickname = Nothing, "NULL", "'" & Nickname & "'") : JobTitle = If(JobTitle = Nothing, "NULL", "'" & JobTitle & "'")
        RelationToEmployee = If(RelationToEmployee = Nothing, "NULL", "'" & RelationToEmployee & "'")

        Dim getemployeedepen = EXECQUER("INSERT INTO employeedependents (CreatedBy,OrganizationID,Salutation,FirstName,MiddleName,LastName," & _
                                        "Surname,ParentEmployeeID,TINNo,SSSNo,HDMFNo,PhilHealthNo,EmailAddress,WorkPhone,HomePhone,MobilePhone,HomeAddress," & _
                                        "Nickname,JobTitle,Gender,RelationToEmployee,ActiveFlag,Birthdate) VALUES (" & _
                                        "" & z_User & _
                                        "," & orgztnID & _
                                        "," & Salutation & _
                                        "," & FirstName & _
                                        "," & MiddleName & _
                                        "," & LastName & _
                                        "," & Surname & _
                                        "," & ParentEmployeeID & _
                                        "," & TINNo & _
                                        "," & SSSNo & _
                                        "," & HDMFNo & _
                                        "," & PhilHealthNo & _
                                        "," & EmailAddress & _
                                        "," & WorkPhone & _
                                        "," & HomePhone & _
                                        "," & MobilePhone & _
                                        "," & HomeAddress & _
                                        "," & Nickname & _
                                        "," & JobTitle & _
                                        ",'" & Gender & _
                                        "'," & RelationToEmployee & _
                                        ",'" & ActiveFlag & _
                                        "','" & Birthdate & _
                                        "');SELECT RowID FROM employeedependents WHERE " & _
                                        "CreatedBy=" & z_User & _
                                        " AND OrganizationID=" & orgztnID & _
                                        " AND Salutation" & If(Salutation = "NULL", " IS NULL", "=" & Salutation) & _
                                        " AND FirstName" & If(FirstName = "NULL", " IS NULL", "=" & FirstName) & _
                                        " AND MiddleName" & If(MiddleName = "NULL", " IS NULL", "=" & MiddleName) & _
                                        " AND LastName" & If(LastName = "NULL", " IS NULL", "=" & LastName) & _
                                        " AND Surname" & If(Surname = "NULL", " IS NULL", "=" & Surname) & _
                                        " AND ParentEmployeeID" & If(ParentEmployeeID = "NULL", " IS NULL", "=" & ParentEmployeeID) & _
                                        " AND TINNo" & If(TINNo = "NULL", " IS NULL", "=" & TINNo) & _
                                        " AND SSSNo" & If(SSSNo = "NULL", " IS NULL", "=" & SSSNo) & _
                                        " AND HDMFNo" & If(HDMFNo = "NULL", " IS NULL", "=" & HDMFNo) & _
                                        " AND PhilHealthNo" & If(PhilHealthNo = "NULL", " IS NULL", "=" & PhilHealthNo) & _
                                        " AND EmailAddress" & If(EmailAddress = "NULL", " IS NULL", "=" & EmailAddress) & _
                                        " AND WorkPhone" & If(WorkPhone = "NULL", " IS NULL", "=" & WorkPhone) & _
                                        " AND HomePhone" & If(HomePhone = "NULL", " IS NULL", "=" & HomePhone) & _
                                        " AND MobilePhone" & If(MobilePhone = "NULL", " IS NULL", "=" & MobilePhone) & _
                                        " AND HomeAddress" & If(HomeAddress = "NULL", " IS NULL", "=" & HomeAddress) & _
                                        " AND Nickname" & If(Nickname = "NULL", " IS NULL", "=" & Nickname) & _
                                        " AND JobTitle" & If(JobTitle = "NULL", " IS NULL", "=" & JobTitle) & _
                                        " AND Gender='" & Gender & "' " & _
                                        " AND RelationToEmployee" & If(RelationToEmployee = "NULL", " IS NULL", "=" & RelationToEmployee) & _
                                        " AND ActiveFlag='" & ActiveFlag & "' " & _
                                        " AND Birthdate='" & Birthdate & "';")

        Return getemployeedepen
    End Function
    Public pshRowID As String

    Sub UnChk(ByVal chkbx As CheckBox)
        chkbx.Checked = False 'If(chkbx.Checked, False, True)
    End Sub


    'Dim collofObj(Byte.MaxValue) As Object
    'Dim i As Byte = 0
    '        For Each ctl As Control In Me.Controls
    '            If TypeOf ctl Is ToolStrip Then
    '                For Each tl_item As ToolStripItem In Repairs.ToolStrip1.Items

    '                    If TypeOf tl_item Is ToolStripButton Then

    '                        If DirectCast(tl_item, ToolStripButton).Text.ToString.Replace("&", "") = "New" Or _
    '                            DirectCast(tl_item, ToolStripButton).Text.ToString.Replace("&", "") = "Save" Then

    '                            collofObj(i) = tl_item

    '                            i = i + 1
    '                        End If

    '                    End If
    '                Next
    '            Else
    '                Exit For
    '            End If
    '        Next

    '        For Each itm As ToolStripButton In collofObj
    '            If itm IsNot Nothing Then
    '                itm.Dispose()
    '            End If
    '        Next
    Function getStrBetween(ByVal myStr As String, ByVal startIndx As Char, ByVal lastIndx As Char) As String
        Dim _mystr As String = myStr

        _mystr = _mystr.Substring(_mystr.IndexOf(startIndx) + 1)
        _mystr = _mystr.Substring(0, _mystr.IndexOf(lastIndx))

        Return _mystr

    End Function
    Public Function INSGet_View(ByVal ViewName As String) As String '                             ' & orgztnID 
        Dim _str = EXECQUER("INSERT INTO view (ViewName,OrganizationID) VALUES('" & ViewName & "',1" & _
                                            ");SELECT RowID FROM view WHERE ViewName='" & ViewName & _
                                                "' AND OrganizationID='" & orgztnID & "' LIMIT 1;") '" & orgztnID & "
        Return _str
    End Function

    Public Function INS_position(ByVal PositionName As String, _
                                 Optional ParentPositionID As String = Nothing, _
                                 Optional ParentDivisionID As String = Nothing) As String
        'orgztnID
        ParentPositionID = If(ParentPositionID = Nothing, "NULL", ParentPositionID)
        ParentDivisionID = If(ParentDivisionID = Nothing, "NULL", ParentDivisionID)

        Dim _str = EXECQUER("INSERT INTO position (CreatedBy,OrganizationID,PositionName,ParentPositionID,DivisionId) " & _
        "VALUES(" & 1 & "," & orgztnID & ",'" & PositionName & _
        "'," & ParentPositionID & "," & ParentDivisionID & ");" & _
        "SELECT RowID FROM position WHERE PositionName='" & PositionName & _
        "' AND OrganizationID=" & orgztnID & _
        " AND ParentPositionID" & If(ParentPositionID = "NULL", " IS NULL", "=" & ParentPositionID) & _
        " AND DivisionId" & If(ParentDivisionID = "NULL", " IS NULL", "=" & ParentDivisionID) & _
        " AND CreatedBy=" & z_User & ";")
        Return _str

    End Function


    '=========================================my ERROR NUMBER=====================================
    'MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
    'Catch ex As Exception

    'CrysVwr                    67-01
    'DR                         68-02
    'ExcelWriter                69-03
    'gainStkfromRepaired        71-04
    'ItemCodeforthisStkAdj      73-05
    'itemforRepairs             73-06
    'MRFOffStaff                77-07
    'MRFSalesCoord              77-08
    'MRFWarehseMngr             77-09
    'myModule                   77-10
    'MySQLConn                  77-11
    'newDR                      78-12
    'newPL                      78-13
    'newRepair                  78-14
    'newRR                      78-15
    'Pack                       80-16
    'Packinglist                80-17
    'PRS                        80-18
    'PRS_CEO                    80-19

    '======================*Hanggang dito ka pa lang*==============

    'PR_CEO_PL                 80-20
    'PR_CEO_RR                 80-21
    'PRS_GM                     80-22
    'PR_GM_PL                  80-23
    'PR_GM_RR                  80-24
    'PRS_SalesCoor              80-25
    'PRS_SalesCoor_PL           80-26
    'PRS_SalesCoor_RR           80-27
    'PRS_SC                     80-28
    'PRSWarehseMngr             80-29
    'QtyReserving               81-30
    'RecRep                     82-31
    'Repairs                    82-32
    'SupplierMgmt               83-33
    'viewproducts               86-34
    'viewsupplier               86-35
    'zoomImg                    90-36

    '==============================================================
    Private Sub ErrorLog(ByVal s As String)
        Dim noww As String = Format(Now, "yyyy-MM-dd hh:mm:ss t")
        ''Application.StartupPath = C:\Users\GLOBAL-D\Desktop\Ikhea Lights - Updated System\IkheaLightingInc\bin\x86\Release
        'File.AppendAllText(Application.StartupPath & "\ErrLog.txt", noww & " → " & s & Environment.NewLine)
    End Sub
    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
    Dim cnString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source=" & Application.StartupPath & "\dat.mdb"
    Dim dafile As OleDbDataAdapter
    Sub fileMaker(ByVal id As String)
        Try
            Static x As SByte = 0
            'If x <> id Then 'If x <> 0 Then
            '    x = id
            Dim CN As New OleDbConnection(cnString)
            Dim dtable As New DataTable
            CN.Open()
            dafile = New OleDbDataAdapter()
            dafile.SelectCommand = New OleDbCommand("SELECT File FROM Images WHERE FileID=" & id, CN)
            dafile.Fill(dtable)
            CN.Close()
            CN.Dispose()
            Dim fspdf As New FileStream(Path.GetTempPath & "file.pdf", FileMode.Create)
            Dim blob As Byte() = DirectCast(dtable.Rows(0)("File"), Byte())
            fspdf.Write(blob, 0, blob.Length)
            fspdf.Close()
            fspdf = Nothing
            '    '    Process.Start(Application.StartupPath & "\file.pdf")
            '    'Else
            'End If
            '
            Process.Start(Path.GetTempPath & "file.pdf")

            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error code fileMaker")
        End Try
    End Sub

    Public OrgPic As Byte()

    Sub txtDecimalPoint(ByVal _charcnt As Integer, _
                        ByVal txtdecim As TextBox)
        Dim _txtdecim As New TextBox
        _txtdecim = txtdecim
        Dim pointpos As Integer = _txtdecim.Text.IndexOf(".")
        If pointpos < 0 Then
            _txtdecim.Text = _txtdecim.Text.Substring(0, _charcnt).Replace(".", "")
        Else
            _txtdecim.Text = _txtdecim.Text.Replace(".", "")
            _txtdecim.Text = _txtdecim.Text.Insert(pointpos, ".")
            '_txtdecim.Text = Left(_txtdecim.Text, _charcnt)
        End If
        _txtdecim.Select(_charcnt, 0)
    End Sub

    Function INSUPD_category(Optional cat_RowID As Object = Nothing, _
                        Optional cat_CategoryName As Object = Nothing, _
                        Optional cat_CatalogID As Object = Nothing) As Object
        Dim return_value = Nothing
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            Dim cmdquer As MySqlCommand

            cmdquer = New MySqlCommand("INSUPD_category", conn)

            conn.Open()

            With cmdquer

                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("cat_ID", MySqlDbType.Int32)

                .Parameters.AddWithValue("cat_RowID", If(cat_RowID = Nothing, DBNull.Value, cat_RowID))
                .Parameters.AddWithValue("cat_CategoryName", If(cat_CategoryName = Nothing, DBNull.Value, cat_CategoryName))
                .Parameters.AddWithValue("cat_OrganizationID", orgztnID) 'orgztnID 
                .Parameters.AddWithValue("cat_CatalogID", If(cat_CatalogID = Nothing, DBNull.Value, cat_CatalogID)) 'orgztnID 

                .Parameters("cat_ID").Direction = ParameterDirection.ReturnValue
                Dim datRead As MySqlDataReader
                datRead = .ExecuteReader
                If datRead.Read Then
                    return_value = datRead(0)
                End If
            End With

            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " INSUPD_category")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return return_value
    End Function

    Sub EXEC_VIEW_PROCEDURE(Optional ParamsCollection As Array = Nothing, _
                      Optional ProcedureName As String = Nothing, _
                      Optional DGVCatcher As DataGridView = Nothing, _
                      Optional isUsualDGVPopulateFALSE As SByte = 0, _
                      Optional isAutoresizeRow As SByte = 0)

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand(ProcedureName, conn)
            conn.Open()
            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                For e = 0 To ParamsCollection.GetUpperBound(0) ' - 1
                    Dim paramName As String = ParamsCollection(e, 0)
                    Dim paramVal As Object = ParamsCollection(e, 1)

                    .Parameters.AddWithValue(paramName, paramVal)

                Next

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

                DGVCatcher.Rows.Clear()

                If isUsualDGVPopulateFALSE = 0 Then
                    If isAutoresizeRow = 0 Then
                        Do While datread.Read

                            Dim rcnt = DGVCatcher.Rows.Add()

                            For Each c As DataGridViewColumn In DGVCatcher.Columns
                                DGVCatcher.Item(c.Name, rcnt).Value = datread(c.Index)
                            Next

                        Loop
                    Else
                        Do While datread.Read

                            Dim rcnt = DGVCatcher.Rows.Add()

                            For Each c As DataGridViewColumn In DGVCatcher.Columns
                                DGVCatcher.Item(c.Name, rcnt).Value = datread(c.Index)
                            Next

                            DGVCatcher.AutoResizeRow(rcnt)
                        Loop
                        DGVCatcher.PerformLayout()
                    End If
                Else
                    If isAutoresizeRow = 0 Then
                        Do While datread.Read

                            Dim rcnt = DGVCatcher.Rows.Add()

                            For i = 0 To datread.FieldCount - 1
                                DGVCatcher.Item(i, rcnt).Value = datread(i)
                            Next
                            'DGVCatcher.AutoResizeRow(rcnt)
                        Loop
                    Else
                        Do While datread.Read

                            Dim rcnt = DGVCatcher.Rows.Add()

                            For i = 0 To datread.FieldCount - 1
                                DGVCatcher.Item(i, rcnt).Value = datread(i)
                            Next
                            DGVCatcher.AutoResizeRow(rcnt)
                        Loop
                        DGVCatcher.PerformLayout()
                    End If
                End If



            End With
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " " & ProcedureName, MsgBoxStyle.Critical, "Error")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
    End Sub

    Function EXEC_INSUPD_PROCEDURE(Optional ParamsCollection As Array = Nothing, _
                      Optional ProcedureName As String = Nothing, _
                      Optional returnName As String = Nothing, _
                     Optional MySql_DbType As MySqlDbType = MySqlDbType.Int32) As Object
        Dim return_value = Nothing
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand(ProcedureName, conn)
            conn.Open()
            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add(returnName, MySql_DbType)

                For e = 0 To ParamsCollection.GetUpperBound(0) ' - 1
                    Dim paramName As String = ParamsCollection(e, 0)
                    Dim paramVal As Object = ParamsCollection(e, 1)

                    .Parameters.AddWithValue(paramName, paramVal)

                Next

                .Parameters(returnName).Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader
                datread = .ExecuteReader()
                If datread.Read Then
                    return_value = datread(0)
                End If
            End With
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " " & ProcedureName, MsgBoxStyle.Critical, "ERROR")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return return_value
    End Function

    Function fillDattbl(Optional _quer As String = Nothing, _
                         Optional Params As Array = Nothing, _
                         Optional CommandType As Object = Nothing) As Object
        'Optional ParamValue As Object = Nothing, _
        Dim publictable As New DataTable
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()
            cmd = New MySqlCommand
            cmd.Connection = conn

            cmd.CommandText = _quer

            Select Case Val(CommandType)
                Case 0
                    cmd.CommandType = Data.CommandType.Text
                Case 1
                    'cmd = New MySqlCommand(_quer, conn)
                    cmd.Parameters.Clear()
                    cmd.CommandType = Data.CommandType.StoredProcedure
                    '.Parameters.AddWithValue(ParamName, ParamValue)
                    For indx = 0 To Params.GetUpperBound(0) - 1
                        Dim paramName As String = Params(indx, 0)
                        Dim paramVal = Params(indx, 1)
                        cmd.Parameters.AddWithValue(paramName, paramVal)

                    Next
            End Select

            da.SelectCommand = cmd
            da.Fill(publictable)

            'Return publictable
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(ex.Message & " fillDattbl", MsgBoxStyle.Critical, "Unexpected Message")
        Finally
            conn.Close()
            da.Dispose()
            cmd.Dispose()
        End Try
        Return publictable
    End Function

    Function makefileGetPath(ByVal blobobj As Object) As String

        Dim retrnPath As String = Path.GetTempPath & "tmpfileEmployeeImage.jpg"

        Dim fsmakefile As New FileStream(retrnPath, FileMode.Create)
        Dim blob As Byte() = DirectCast(blobobj, Byte()) 'DirectCast(dtable.Rows(0)("File"), Byte())
        fsmakefile.Write(blob, 0, blob.Length)
        fsmakefile.Close()
        fsmakefile = Nothing

        Return retrnPath

    End Function

    Function VIEW_privilege(ByVal vw_ViewName As String, _
                            ByVal vw_org As String) As Object

        'Dim params(2, 2) As Object

        'params(0, 0) = "vw_ViewName"
        'params(1, 0) = "vw_OrganizationID"

        'params(0, 1) = vw_ViewName
        'params(1, 1) = vw_org

        'Dim view_RowID As Object

        'view_RowID = EXEC_INSUPD_PROCEDURE(params, _
        '                       "VIEW_privilege", _
        '                       "view_RowID")

        Return New ReadSQLFunction("VIEW_privilege", "view_RowID", vw_ViewName, orgztnID).ReturnValue

    End Function

#Region "Reminders"
    '**********need to know**********
    '1.) User
    '   - RowID
    '   - First Name & Last Name
    '   - Privilege (is Add, is Edit, is Delete, is Read only)

    '2.) Organization
    '   - RowID
    '   - Organization Name

    '3.) Employee TabControl - Name 

    '4.) how to call INSERT Row employeesalary

    '5.) how to load employee salary

    '6.) names of columns in Employee - DataGridView

    '7.) name of DataGridView in Employee - DataGridView

#End Region

    Public Enum DGVHeaderImageAlignments As Int32
        [Default] = 0
        FillCell = 1
        SingleCentered = 2
        SingleLeft = 3
        SingleRight = 4
        Stretch = [Default]
        Tile = 5
    End Enum

    'Sub EnterKeyAsTabKey(Optional enter_key As String = Nothing, _
    '                  Optional nextobjfield As Object = Nothing)

    '    If enter_key = 13 Then

    '        nextobjfield.Focus()

    '    End If

    'End Sub

    Dim dataread As MySqlDataReader

    Function GetAsDataTable(ByVal _quer As String, _
                         Optional CmdType As CommandType = CommandType.Text, _
                         Optional ParamsCollection As Array = Nothing) As Object

        Dim pubDatTbl As New DataTable

        Try
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            cmd = New MySqlCommand(_quer, conn)

            conn.Open()

            With cmd
                'If CmdType = CommandType.StoredProcedure Then

                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                '.Parameters.Add("", MySqlDbType.Int32)

                For e = 0 To ParamsCollection.GetUpperBound(0)
                    Dim paramName As String = ParamsCollection(e, 0)
                    Dim paramVal As Object = ParamsCollection(e, 1)

                    .Parameters.AddWithValue(paramName, paramVal)

                Next

                '.Parameters("").Direction = ParameterDirection.ReturnValue

                'da.SelectCommand = cmd

                dataread = .ExecuteReader()

                'End If

            End With

            Dim rownew As DataRow = Nothing

            For c = 0 To dataread.FieldCount - 1
                pubDatTbl.Columns.Add("DatRowCol" & c)

            Next

            Do While dataread.Read
                rownew = pubDatTbl.NewRow

                For c = 0 To dataread.FieldCount - 1
                    rownew(c) = If(IsDBNull(dataread(c)), "", dataread(c).ToString)

                Next

                pubDatTbl.Rows.Add(rownew)

            Loop

            'da.Fill(pubDatTbl)

            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(getErrExcptn(ex, "myModule"), MsgBoxStyle.Critical)

        Finally
            da.Dispose()
            conn.Close()
            cmd.Dispose()

        End Try

        Return pubDatTbl

    End Function

    Function callProcAsDatTab(Optional ParamsCollection As Array = Nothing, _
                              Optional ProcedureName As String = Nothing) As Object

        Dim returnvalue = Nothing

        Dim mysqlda As New MySqlDataAdapter()

        Try

            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            conn.Open()

            Dim ds As New DataSet()

            With mysqlda

                .SelectCommand = New MySqlCommand(ProcedureName, conn)

                .SelectCommand.CommandType = CommandType.StoredProcedure

                .SelectCommand.Parameters.Clear()

                For e = 0 To ParamsCollection.GetUpperBound(0) ' - 1

                    Dim paramName As String = ParamsCollection(e, 0)

                    Dim paramVal As Object = ParamsCollection(e, 1)

                    .SelectCommand.Parameters.AddWithValue(paramName, paramVal)

                Next

                .Fill(ds, "Table0")

            End With

            Dim dt As DataTable = ds.Tables("Table0")

            returnvalue = dt

            hasERR = 0
        Catch ex As Exception
            hasERR = 1

            MsgBox(getErrExcptn(ex, ProcedureName), MsgBoxStyle.Critical)

            returnvalue = Nothing

        Finally

            mysqlda.Dispose()

        End Try

        Return returnvalue

    End Function

    Function GetWorkBookAsDataInText(ByVal opfiledir As String, Optional FormName As String = "") As Object
        Dim adapter As New OleDbDataAdapter
        Dim dataset As New DataSet

        Dim connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & opfiledir & ";Extended Properties='Excel 12.0;IMEX=1;HDR=No;TypeGuessRows=0;ImportMixedTypes=Text;';"

        Dim result = Nothing

        Try
            Dim connection As New OleDbConnection(connectionString)
            connection.Open()

            If connection.State = ConnectionState.Closed Then
                Console.Write("Connection cannot be opened")
            Else
                Console.Write("Welcome")
            End If

            'New Object() {Nothing, Nothing, Nothing, "TABLE"}
            Dim schemaTable As DataTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)

            For Each excelTable As DataRow In schemaTable.Rows
                Dim command As New OleDbCommand("SELECT * FROM [" & excelTable(2).ToString & "]", connection)

                adapter.SelectCommand = command
                adapter.Fill(dataset, excelTable(2).ToString)
            Next

            result = dataset
            connection.Close()
        Catch ex As Exception
            result = Nothing
            MsgBox(getErrExcptn(ex, FormName), MsgBoxStyle.Critical)
        Finally
            adapter.Dispose()
            dataset.Dispose()
        End Try

        Return result
    End Function

    Function getWorkBookAsDataSet(ByVal opfiledir As String, _
                             Optional FormName As String = "") As Object

        Dim StrConn As String

        Dim DA As New OleDbDataAdapter

        Dim DS As New DataSet

        Dim Str As String = Nothing

        Dim ColumnCount As Integer = 0

        Dim OuterCount As Integer = 0

        Dim InnerCount As Integer = 0

        Dim RowCount As Integer = 0

        StrConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & opfiledir & ";Extended Properties='Excel 12.0;';"

        Dim returnvalue = Nothing

        Try
            Dim objConn As New OleDbConnection(StrConn)

            objConn.Open()

            If objConn.State = ConnectionState.Closed Then

                Console.Write("Connection cannot be opened")

            Else

                Console.Write("Welcome")

            End If

            'Dim datasheet As DataTable = GetSchemaTable(StrConn)

            'returnvalue = datasheet


            Dim schemaTable As DataTable = _
                objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, _
                                            Nothing) 'New Object() {Nothing, Nothing, Nothing, "TABLE"}

            Dim i = 0

            'Dim datset As DataSet = New DataSet("dsImport")

            For Each drow As DataRow In schemaTable.Rows

                Dim objCmd As New OleDbCommand("Select * from [" & drow(2).ToString & "]", objConn)

                'objCmd.CommandType = CommandType.Text

                DA.SelectCommand = objCmd

                DA.Fill(DS, drow(2).ToString)

                'Dim dtimport As New DataTable

                'dtimport = DS.Tables(i)

                'Dim row_count = dtimport.Rows.Count

                'datset.Tables.Add(DS.Tables(i))

                i += 1

            Next

            returnvalue = DS

            objConn.Close()

            hasERR = 0
        Catch ex As Exception
            hasERR = 1

            returnvalue = Nothing

            MsgBox(getErrExcptn(ex, FormName), MsgBoxStyle.Critical)

        Finally

            DA.Dispose()
            'DS.Dispose()

        End Try

        Return returnvalue

    End Function


    Public Function GetSchemaTable(ByVal connectionString As String) _
        As DataTable

        Using connection As New OleDbConnection(connectionString)

            connection.Open()

            Dim schemaTable As DataTable = _
                connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, _
                New Object() {Nothing, Nothing, Nothing, "TABLE"})

            Return schemaTable

        End Using

    End Function

    Function IntVal(ByVal ObjectValue As Object) As Integer
        Dim catchval = If(IsDBNull(ObjectValue), 0, ObjectValue)
        Return CInt(Val(catchval))
    End Function

    Function ValNoComma(ByVal ObjectValue As Object) As Double

        Dim catchval = Nothing
        'If(IsDBNull(ObjectValue), 0, FormatNumber(Val(ObjectValue), 2))
        If IsDBNull(ObjectValue) Then
            catchval = Val(0)
        ElseIf ObjectValue = Nothing Then
            catchval = Val(0)
        Else
            catchval = ObjectValue.ToString.Replace(",", "")
        End If

        Return Val(catchval.ToString.Replace(",", ""))

    End Function

    Sub PopulateDGVwithDatTbl(Optional dgv As DataGridView = Nothing, _
                              Optional dt As DataTable = Nothing)

        dgv.Rows.Clear()

        If dt IsNot Nothing Then
            Try

                For Each drow As DataRow In dt.Rows

                    Dim rowindx = _
                                dgv.Rows.Add()

                    For Each dgvcol As DataGridViewColumn In dgv.Columns

                        dgv.Item(dgvcol.Name, rowindx).Value = CObj(drow(dgvcol.Index))

                    Next

                Next

            Catch ex As Exception

                MsgBox(getErrExcptn(ex, "myModule"))

            Finally

                dgv.PerformLayout()

            End Try

        End If

    End Sub

    Function InstantiateDatatable(Optional DT As DataTable = Nothing) As DataTable

        Dim returnval As New DataTable

        returnval = CType(DT, DataTable)

        Return returnval

    End Function

    Function ObjectCopyToClipBoard(Optional KeyEvArgs As KeyEventArgs = Nothing, _
                                   Optional objObject As Object = Nothing) As Boolean

        Dim returnvalue As Boolean = False

        If KeyEvArgs.Control AndAlso KeyEvArgs.KeyCode = Keys.C Then

            returnvalue = True

            objObject.Copy()

        Else

            returnvalue = False


        End If

        Return returnvalue

    End Function


    Function GetDatatable(Query As String) As DataTable
        Dim dt As New DataTable
        Try
            da = New MySqlDataAdapter(Query, conn)
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Sub OjbAssignNoContextMenu(ByVal obj As Object)

        obj.ContextMenu = New ContextMenu

    End Sub

    Function StringToArray(ByVal ParamString As String) As String()

        Dim returnvalue(0) As String

        Dim paramlength = ParamString.Length

        Dim new_listofStr As New List(Of String)

        Try

            If paramlength = 3 Then

                ReDim returnvalue(0)

                returnvalue(0) = ParamString

                If new_listofStr.Contains(ParamString) = False Then

                    new_listofStr.Add(ParamString)

                End If

            Else

                For combicount = 2 To 5

                    Dim limitstrindx = paramlength - combicount

                    If limitstrindx < 0 Then

                        ReDim returnvalue(0)

                        returnvalue(0) = ParamString

                        If new_listofStr.Contains(ParamString) = False Then

                            new_listofStr.Add(ParamString)

                        End If

                        Exit For

                    Else

                        Dim splitDispName = Split(ParamString, " ")

                        Dim newsizearray = limitstrindx + (splitDispName.GetUpperBound(0))

                        ReDim Preserve returnvalue(newsizearray) 'limitstrindx

                        Dim i_indx = 0

                        For i = 0 To limitstrindx

                            Dim strval = ParamString.Substring(i, combicount)

                            'strlooparray(i) = strval

                            new_listofStr.Add(strval)

                            i_indx = i

                        Next

                        Dim ix = 0

                        If combicount = 5 Then

                            'ReDim returnvalue(new_listofStr.Count - 1)

                            'Dim indx = 0

                            'For Each strval In new_listofStr
                            '    returnvalue(indx) = strval
                            '    indx += 1
                            'Next

                            'For ii = i_indx To newsizearray

                            '    'strlooparray(ii) = Trim(splitDispName(ix))

                            '    Dim n_value = splitDispName(ix)

                            '    new_listofStr.Add(n_value)

                            '    ix += 1

                            'Next

                            For Each strvalue In splitDispName

                                Dim n_value = strvalue

                                If new_listofStr.Contains(n_value) Then
                                    Continue For
                                Else

                                    new_listofStr.Add(n_value)

                                End If

                            Next

                        End If

                    End If

                Next

            End If

        Catch ex As Exception

            MsgBox(getErrExcptn(ex, "myModule"))

        End Try

        Return new_listofStr.ToArray

    End Function

    Public installerpath As String = String.Empty

    Sub REG_EDIT_DBCONNECTION()

        Dim regKey As RegistryKey

        Dim ver = Nothing

        regKey = Registry.LocalMachine.OpenSubKey("Software\Globagility\DBConn\MTI", True)

        If regKey Is Nothing Then

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
            regKey.CreateSubKey("Globagility\DBConn\MTI")

            regKey = Registry.LocalMachine.OpenSubKey("Software\Globagility\DBConn\MTI", True)

            regKey.SetValue("server", "127.0.0.1")
            regKey.SetValue("user id", "root")
            regKey.SetValue("password", "globagility")
            regKey.SetValue("database", "GoldWingsPayrollSys")

        Else

            ver = regKey.GetValue("server") & vbNewLine & _
                regKey.GetValue("user id") & vbNewLine & _
                regKey.GetValue("password") & vbNewLine & _
                regKey.GetValue("database")

            installerpath = regKey.GetValue("installerpath")

            'MsgBox(ver)

        End If

        regKey.Close()

    End Sub

    Function MYSQLDateFormat(ParamDate As Date) As Object

        Dim returnvalue = Nothing

        Try
            Dim strmonthlen = Trim(ParamDate.Month).Length

            Dim strdaylen = Trim(ParamDate.Day).Length

            Dim strmonth, strday As String
            
            If strmonthlen = 1 Then
                strmonth = "0" & Trim(ParamDate.Month)
            Else
                strmonth = Trim(ParamDate.Month)
            End If

            If strdaylen = 1 Then
                strday = "0" & Trim(ParamDate.Day)
            Else
                strday = Trim(ParamDate.Day)
            End If

            returnvalue = ParamDate.Year & "-" & strmonth & "-" & strday

        Catch ex As Exception
            returnvalue = Nothing
            MsgBox(getErrExcptn(ex, "MYSQLDateFormat"), , CStr(ParamDate))
        End Try

        Return returnvalue

    End Function

    Function VBDateTimePickerValueFormat(ParamDate As Date) As Object

        Dim returnvalue = Nothing

        'returnvalue = Format(ParamDate, "dd/MM/yyyy")
        returnvalue = Format(ParamDate, machineShortDateFormat)

        Return returnvalue

    End Function

    Function FindingWordsInString(ByVal FullStringToCompare As String, ParamArray AnArrayOfWords() As String) As Boolean

        Dim returnvalue As Boolean = False

        FullStringToCompare = FullStringToCompare.ToUpper

        For Each strval In AnArrayOfWords

            If FullStringToCompare.Contains(strval.ToUpper) Then
                returnvalue = True
                Exit For
            End If

        Next

        Return returnvalue

    End Function

End Module