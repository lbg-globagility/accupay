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

    Public machineShortTimeFormat As String = RegKey.GetValue("sShortTime").ToString

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

    Public Function TrapSpaceKey(ByVal KCode As String) As Boolean      '//textbox keypress event disable space key
        If KCode = 32 Then
            TrapSpaceKey = True
        Else
            TrapSpaceKey = False
        End If
    End Function

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

    Public txtSimpleSearch As New TextBox

    Dim dgvleft As New DataGridView

    Dim formFunctn, cmdSearch, s_query As String

    Public kEnter As New KeyEventArgs(Keys.Enter)

    Public dgvEvArgs As New DataGridViewCellEventArgs(0, 0)

    Public tsbtnExportToExcel As New ToolStripButton

    Public mySaveFileDialog As New SaveFileDialog

    Dim savePath As String

    Dim dgvtargetforExport As DataGridView

    Public Delegate Sub delegateCaller(sender As Object, e As EventArgs)

    Event AnEvent As delegateCaller

    Public mydele As delegateCaller

    'Public _Sub

    Public Event An_Event()

    Public pshRowID As String

    Function getStrBetween(ByVal myStr As String, ByVal startIndx As Char, ByVal lastIndx As Char) As String
        Dim _mystr As String = myStr

        _mystr = _mystr.Substring(_mystr.IndexOf(startIndx) + 1)
        _mystr = _mystr.Substring(0, _mystr.IndexOf(lastIndx))

        Return _mystr

    End Function

    Public Function INSGet_View(ByVal ViewName As String) As String '                             ' & orgztnID
        Dim _str = EXECQUER("INSERT INTO view (ViewName,OrganizationID) VALUES('" & ViewName & "',1" &
                                            ");SELECT RowID FROM view WHERE ViewName='" & ViewName &
                                                "' AND OrganizationID='" & orgztnID & "' LIMIT 1;") '" & orgztnID & "
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

    Public OrgPic As Byte()

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
                If datread.Read Then
                    return_value = datread(0)
                End If
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

    Dim dataread As MySqlDataReader

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

    Function ObjectCopyToClipBoard(Optional KeyEvArgs As KeyEventArgs = Nothing,
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