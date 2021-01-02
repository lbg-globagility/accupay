Imports System.IO
Imports AccuPay.Core.Entities
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient

Module myModule
    Public conn As New MySqlConnection
    Public cmd As New MySqlCommand

    Public MainServiceProvider As ServiceProvider

    Public firstchar_requiredforparametername As String = "?"

    Public n_DataBaseConnection As New DataBaseConnection
    Public mysql_conn_text As String = n_DataBaseConnection.GetStringMySQLConnectionString
    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Public machineShortDateFormat As String = RegKey.GetValue("sShortDate").ToString

    Public custom_mysqldateformat As String = String.Empty

    Public sys_servername, sys_userid, sys_password, sys_db, sys_apppath As String
    Public orgztnID As String
    Public orgNam As String

    Public TimeTick As SByte = 0

    Public CURDATE_MDY As String = "SELECT CURDATE();" '"SELECT DATE_FORMAT(CURDATE(),'%m-%d-%Y')"

    Public Width_resolution As Integer = My.Computer.Screen.Bounds.Width

    Public Height_resolution As Integer = My.Computer.Screen.Bounds.Height

    Public dbnow

    Public previousForm As Form = Nothing

    Public USER_ROLE As AspNetRole

    Public userFirstName As String = Nothing

    Public userLastName As String = Nothing

    Public db_connectinstring = ""

    Public MachineLocalization As New DataTable

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

    Function retAsDatTbl(ByVal _quer As String,
                         Optional dgv As DataGridView = Nothing) As Object

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(_quer)

        Return n_SQLQueryToDatatable.ResultTable

    End Function

    Public hasERR As SByte

    Public Function EXECQUER(ByVal cmdsql As String,
                             Optional errorthrower As String = Nothing) As Object 'String

        Dim n_ExecuteQuery As New ExecuteQuery(cmdsql)

        Return n_ExecuteQuery.Result

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

    Public Function convertFileToByte(ByVal filePath As String) As Byte()
        Dim fs As FileStream
        fs = New FileStream(filePath, FileMode.Open, FileAccess.Read)
        Dim fileByte As Byte() = New Byte(fs.Length - 1) {}
        fs.Read(fileByte, 0, System.Convert.ToInt32(fs.Length))
        fs.Close()

        Return fileByte

    End Function

    Sub TabControlColor(ByVal TabCntrl As TabControl,
                        ByVal ee As System.Windows.Forms.DrawItemEventArgs,
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

    Public infohint As New ToolTip

    Public Sub InfoBalloon(Optional ToolTipStringContent As String = Nothing, Optional ToolTipStringTitle As String = Nothing, Optional objct As System.Windows.Forms.IWin32Window = Nothing, Optional x As Integer = 0, Optional y As Integer = 0, Optional dispo As SByte = 0, Optional duration As Integer = 3000)

        If infohint Is Nothing Then Return

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

    Public hintWarn As New ToolTip 'New ToolTip

    Public Sub WarnBalloon(Optional ToolTipStringContent As String = Nothing, Optional ToolTipStringTitle As String = Nothing, Optional objct As System.Windows.Forms.IWin32Window = Nothing, Optional x As Integer = 0, Optional y As Integer = 0, Optional dispo As Byte = 0, Optional duration As Integer = 2275)

        If hintWarn Is Nothing Then Return

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

    Function INS_employeedepen(Optional Salutation As Object = Nothing, Optional FirstName As Object = Nothing,
                               Optional MiddleName As Object = Nothing, Optional LastName As Object = Nothing,
                               Optional Surname As Object = Nothing, Optional ParentEmployeeID As Object = Nothing,
                               Optional TINNo As Object = Nothing, Optional SSSNo As Object = Nothing,
                               Optional HDMFNo As Object = Nothing, Optional PhilHealthNo As Object = Nothing,
                               Optional EmailAddress As Object = Nothing, Optional WorkPhone As Object = Nothing,
                               Optional HomePhone As Object = Nothing, Optional MobilePhone As Object = Nothing,
                               Optional HomeAddress As Object = Nothing, Optional Nickname As Object = Nothing,
                               Optional JobTitle As Object = Nothing, Optional Gender As Object = Nothing,
                               Optional RelationToEmployee As Object = Nothing, Optional ActiveFlag As Object = Nothing,
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

        Dim getemployeedepen = EXECQUER("INSERT INTO employeedependents (CreatedBy,OrganizationID,Salutation,FirstName,MiddleName,LastName," &
                                        "Surname,ParentEmployeeID,TINNo,SSSNo,HDMFNo,PhilHealthNo,EmailAddress,WorkPhone,HomePhone,MobilePhone,HomeAddress," &
                                        "Nickname,JobTitle,Gender,RelationToEmployee,ActiveFlag,Birthdate) VALUES (" &
                                        "" & z_User &
                                        "," & orgztnID &
                                        "," & Salutation &
                                        "," & FirstName &
                                        "," & MiddleName &
                                        "," & LastName &
                                        "," & Surname &
                                        "," & ParentEmployeeID &
                                        "," & TINNo &
                                        "," & SSSNo &
                                        "," & HDMFNo &
                                        "," & PhilHealthNo &
                                        "," & EmailAddress &
                                        "," & WorkPhone &
                                        "," & HomePhone &
                                        "," & MobilePhone &
                                        "," & HomeAddress &
                                        "," & Nickname &
                                        "," & JobTitle &
                                        ",'" & Gender &
                                        "'," & RelationToEmployee &
                                        ",'" & ActiveFlag &
                                        "','" & Birthdate &
                                        "');SELECT RowID FROM employeedependents WHERE " &
                                        "CreatedBy=" & z_User &
                                        " AND OrganizationID=" & orgztnID &
                                        " AND Salutation" & If(Salutation = "NULL", " IS NULL", "=" & Salutation) &
                                        " AND FirstName" & If(FirstName = "NULL", " IS NULL", "=" & FirstName) &
                                        " AND MiddleName" & If(MiddleName = "NULL", " IS NULL", "=" & MiddleName) &
                                        " AND LastName" & If(LastName = "NULL", " IS NULL", "=" & LastName) &
                                        " AND Surname" & If(Surname = "NULL", " IS NULL", "=" & Surname) &
                                        " AND ParentEmployeeID" & If(ParentEmployeeID = "NULL", " IS NULL", "=" & ParentEmployeeID) &
                                        " AND TINNo" & If(TINNo = "NULL", " IS NULL", "=" & TINNo) &
                                        " AND SSSNo" & If(SSSNo = "NULL", " IS NULL", "=" & SSSNo) &
                                        " AND HDMFNo" & If(HDMFNo = "NULL", " IS NULL", "=" & HDMFNo) &
                                        " AND PhilHealthNo" & If(PhilHealthNo = "NULL", " IS NULL", "=" & PhilHealthNo) &
                                        " AND EmailAddress" & If(EmailAddress = "NULL", " IS NULL", "=" & EmailAddress) &
                                        " AND WorkPhone" & If(WorkPhone = "NULL", " IS NULL", "=" & WorkPhone) &
                                        " AND HomePhone" & If(HomePhone = "NULL", " IS NULL", "=" & HomePhone) &
                                        " AND MobilePhone" & If(MobilePhone = "NULL", " IS NULL", "=" & MobilePhone) &
                                        " AND HomeAddress" & If(HomeAddress = "NULL", " IS NULL", "=" & HomeAddress) &
                                        " AND Nickname" & If(Nickname = "NULL", " IS NULL", "=" & Nickname) &
                                        " AND JobTitle" & If(JobTitle = "NULL", " IS NULL", "=" & JobTitle) &
                                        " AND Gender='" & Gender & "' " &
                                        " AND RelationToEmployee" & If(RelationToEmployee = "NULL", " IS NULL", "=" & RelationToEmployee) &
                                        " AND ActiveFlag='" & ActiveFlag & "' " &
                                        " AND Birthdate='" & Birthdate & "';")

        Return getemployeedepen
    End Function

    Function getStrBetween(ByVal myStr As String, ByVal startIndx As Char, ByVal lastIndx As Char) As String
        Dim _mystr As String = myStr

        _mystr = _mystr.Substring(_mystr.IndexOf(startIndx) + 1)
        _mystr = _mystr.Substring(0, _mystr.IndexOf(lastIndx))

        Return _mystr

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

    Sub EXEC_VIEW_PROCEDURE(Optional ParamsCollection As Array = Nothing,
                      Optional ProcedureName As String = Nothing,
                      Optional DGVCatcher As DataGridView = Nothing,
                      Optional isUsualDGVPopulateFALSE As SByte = 0,
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

    Function EXEC_INSUPD_PROCEDURE(Optional ParamsCollection As Array = Nothing,
                      Optional ProcedureName As String = Nothing,
                      Optional returnName As String = Nothing,
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
                'If datread.Read Then
                return_value = datread(0)
                'End If
            End With
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            MsgBox(String.Concat(ex.Message, Environment.NewLine, Environment.NewLine, ProcedureName),
                   MsgBoxStyle.Critical, "ERROR")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return return_value
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

    Function callProcAsDatTab(Optional ParamsCollection As Array = Nothing,
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

    Sub PopulateDGVwithDatTbl(Optional dgv As DataGridView = Nothing,
                              Optional dt As DataTable = Nothing)

        dgv.Rows.Clear()

        If dt IsNot Nothing Then
            Try

                For Each drow As DataRow In dt.Rows

                    Dim rowindx =
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
