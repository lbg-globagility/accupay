Imports System.Data.OleDb
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports Excel = Microsoft.Office.Interop.Excel

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

    Sub filltable(ByVal datgrid As Object,
                         Optional _quer As String = Nothing,
                         Optional Params As Array = Nothing,
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
                    cmd.CommandType = CommandType.Text
                Case 1
                    'cmd = New MySqlCommand(_quer, conn)
                    cmd.Parameters.Clear()
                    cmd.CommandType = CommandType.StoredProcedure
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

    Public hasERR As SByte

    Public Function EXECQUER(ByVal cmdsql As String,
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

    Public Function convertFileToByte(ByVal filePath As String) As Byte()
        Dim fs As FileStream
        fs = New FileStream(filePath, FileMode.Open, FileAccess.Read)
        Dim fileByte As Byte() = New Byte(fs.Length - 1) {}
        fs.Read(fileByte, 0, System.Convert.ToInt32(fs.Length))
        fs.Close()

        Return fileByte

    End Function

    Dim formFunctn, cmdSearch, s_query As String

    Dim savePath As String

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

    Public Delegate Sub delegateCaller(sender As Object, e As EventArgs)

    Event AnEvent As delegateCaller

    Public mydele As delegateCaller

    'Public _Sub

    Public Event An_Event()

    Sub INS_LoL(Optional DisplayValue As Object = Nothing,
                Optional LIC As Object = Nothing,
                Optional Type As Object = Nothing,
                Optional ParentLIC As Object = Nothing,
                Optional Active As Object = Nothing,
                Optional Description As Object = Nothing,
                Optional Created As Object = Nothing,
                Optional CreatedBy As Object = Nothing,
                Optional LastUpd As Object = Nothing,
                Optional OrderBy As Object = Nothing,
                Optional LastUpdBy As Object = Nothing)

        EXECQUER("INSERT INTO listofval (DisplayValue,LIC,Type,ParentLIC,Active,Description,Created,CreatedBy,LastUpd,OrderBy," &
                             "LastUpdBy) VALUES (" &
                             "'" & DisplayValue & "'" &
                             "," & If(LIC = Nothing, "NULL", "'" & LIC & "'") &
                             "," & If(Type = Nothing, "NULL", "'" & Type & "'") &
                             "," & If(ParentLIC = Nothing, "NULL", "'" & ParentLIC & "'") &
                             "," & If(Active = Nothing, "NULL", "'" & Active & "'") &
                             "," & If(Description = Nothing, "NULL", "'" & Description & "'") &
                             ",DATE_FORMAT(NOW(),'%Y-%m-%d %h:%i:%s')" &
                             "," & CreatedBy &
                             ",DATE_FORMAT(NOW(),'%Y-%m-%d %h:%i:%s')" &
                             "," & If(OrderBy = Nothing, "NULL", OrderBy) &
                             "," & CreatedBy &
                             ");")

    End Sub

    Public pshRowID As String

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

    Dim dafile As OleDbDataAdapter

    Public OrgPic As Byte()

    Function INSUPD_category(Optional cat_RowID As Object = Nothing,
                        Optional cat_CategoryName As Object = Nothing,
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

    Function fillDattbl(Optional _quer As String = Nothing,
                         Optional Params As Array = Nothing,
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
                    cmd.CommandType = CommandType.Text
                Case 1
                    'cmd = New MySqlCommand(_quer, conn)
                    cmd.Parameters.Clear()
                    cmd.CommandType = CommandType.StoredProcedure
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

    Function VIEW_privilege(ByVal vw_ViewName As String,
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

    Function GetAsDataTable(ByVal _quer As String,
                         Optional CmdType As CommandType = CommandType.Text,
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

    Function getWorkBookAsDataSet(ByVal opfiledir As String,
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

            Dim schemaTable As DataTable =
                objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
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

            Dim schemaTable As DataTable =
                connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
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

    Function InstantiateDatatable(Optional DT As DataTable = Nothing) As DataTable

        Dim returnval As New DataTable

        returnval = CType(DT, DataTable)

        Return returnval

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

            ver = regKey.GetValue("server") & vbNewLine &
                regKey.GetValue("user id") & vbNewLine &
                regKey.GetValue("password") & vbNewLine &
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

End Module