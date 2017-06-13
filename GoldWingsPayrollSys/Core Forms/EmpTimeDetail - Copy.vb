Imports MySql.Data.MySqlClient
Imports Microsoft.Win32

Public Class EmpTimeDetail

    Dim dattabLogs As New DataTable

    Dim dtTimeLogs As New DataTable

    Dim dtImport As New DataTable

    Dim thefilepath As String

    Dim view_ID As Object

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        'view_ID = VIEW_privilege(Me.Text, orgztnID)

        With dattabLogs.Columns
            .Add("logging")

            .Add("EmpNum")
            .Add("TI")
            .Add("TO")
            .Add("Date")
            .Add("Type")

        End With

        With dtTimeLogs.Columns

            .Add("EmpID", Type.GetType("System.String"))
            .Add("DateLog", Type.GetType("System.String"))
            .Add("TimeLog", Type.GetType("System.String"))
            .Add("TypeLog", Type.GetType("System.String"))

            .Add("DateTimeLog", Type.GetType("System.String"))

            .Add("BranchCode", Type.GetType("System.String"))

            .Add("Tag", Type.GetType("System.String"))

        End With

        With dtImport.Columns

            .Add("EmploID", Type.GetType("System.String"))
            .Add("TIn", Type.GetType("System.String"))
            .Add("TOut", Type.GetType("System.String"))
            .Add("LogDate", Type.GetType("System.String"))

            .Add("DateTimeLogIn", Type.GetType("System.String"))

            .Add("DateTimeLogOut", Type.GetType("System.String"))

            .Add("BranchCode", Type.GetType("System.String"))

        End With

        loademployeetimeentrydetails()

        AddHandler dgvetentd.SelectionChanged, AddressOf dgvetentd_SelectionChanged

        dgvetentd_SelectionChanged(sender, e)

        view_ID = VIEW_privilege("Employee Time Entry logs", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then

            tsbtnNew.Visible = 0
            tsbtnSave.Visible = 0
            tsbtndel.Visible = 0

            dontUpdate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    tsbtnNew.Visible = 0
                    tsbtnSave.Visible = 0
                    tsbtndel.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        tsbtnNew.Visible = 0
                    Else
                        tsbtnNew.Visible = 1
                    End If

                    If drow("Deleting").ToString = "N" Then
                        tsbtndel.Visible = 0
                    Else
                        tsbtndel.Visible = 1
                    End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

        dgvetentd.Focus()

        TabPage1.Focus()

    End Sub

    Sub loademployeetimeentrydetails(Optional pagination As Integer = 0)

        Static once As Integer = -1
        'filltable
        'dgvRowAdder("SELECT DATE_FORMAT(etdet.Created,'%m/%d/%Y %h:%i %p') 'Created'" & _
        '            ",DATE_FORMAT(etdet.Created,'%Y-%m-%d %H:%i:%s') 'createdmilit'" & _
        '          ",COALESCE(CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))),'') 'Created by'" & _
        '          ",COALESCE(DATE_FORMAT(etdet.LastUpd,'%b-%d-%Y'),'') 'Last Update'" & _
        '          ",COALESCE((SELECT CONCAT(CONCAT(UCASE(LEFT(FirstName, 1)), SUBSTRING(FirstName, 2)),' ',CONCAT(UCASE(LEFT(LastName, 1)), SUBSTRING(LastName, 2))) FROM user WHERE RowID=etdet.LastUpd),'') 'Last update by'" & _
        '          " FROM employeetimeentrydetails etdet" & _
        '          " LEFT JOIN user u ON etdet.CreatedBy=u.RowID" & _
        '          " WHERE etdet.OrganizationID=" & orgztnID & _
        '          " GROUP BY etdet.Created " & _
        '          " ORDER BY etdet.RowID DESC LIMIT " & pagination & ",100;", _
        '          dgvetentd) 'DATE_FORMAT(etdet.Created,'%b-%d-%Y %H:%m%:%s')
        'If once <> 1 Then
        '    once = 1
        '    With dgvetentd
        '        .Columns("Created by").Visible = False

        '        .Columns("Last Update").Visible = False
        '        .Columns("Last Update by").Visible = False
        '    End With
        'End If
        dgvetentd.Rows.Clear()
        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL `VIEW_timeentrydetails`('" & orgztnID & "', '" & pagination & "');")
        Dim catchdt As New DataTable
        catchdt = n_SQLQueryToDatatable.ResultTable
        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvetentd.Rows.Add(row_array)
        Next
        catchdt.Dispose()
    End Sub

    Sub VIEWemployeetimeentrydetails(ByVal date_created As Object,
                                     Optional EmployeeNumber As String = Nothing)

        Dim param(2, 2) As Object

        param(0, 0) = "etentd_Created"
        param(1, 0) = "etentd_OrganizationID"
        param(2, 0) = "etd_EmployeeNumber"

        param(0, 1) = date_created
        param(1, 1) = orgztnID
        param(2, 1) = EmployeeNumber

        EXEC_VIEW_PROCEDURE(param, _
                           "VIEW_employeetimeentrydetails", _
                           dgvetentdet)

    End Sub

    Private Sub tsbtnNew_Click(sender As Object, e As EventArgs) Handles tsbtnNew.Click
        Static employeeleaveRowID As Integer = -1

        Try
            Dim browsefile As OpenFileDialog = New OpenFileDialog()
            browsefile.Filter = "Text Documents (*.txt)|*.txt" & _
                                "|All files (*.*)|*.*"

            If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

                thefilepath = browsefile.FileName

                tsbtnNew.Enabled = False

                Dim balloon_x = lblforballoon.Location.X

                lblforballoon.Location = New Point(TabControl1.Location.X, lblforballoon.Location.Y)

                InfoBalloon("Please wait a few moments.", _
                          "Importing file...", lblforballoon, 0, -69)

                lblforballoon.Location = New Point(balloon_x, lblforballoon.Location.Y)

                RemoveHandler dgvetentd.SelectionChanged, AddressOf dgvetentd_SelectionChanged

                Panel1.Enabled = False

                ToolStripProgressBar1.Visible = True

                bgworkImport.RunWorkerAsync()

            Else

            End If
        Catch ex As Exception
            MsgBox(ex.Message & " Error on file initialization")
        Finally
            'AddHandler dgvetentdet.SelectionChanged, AddressOf dgvetentdet_SelectionChanged
        End Try
    End Sub

    Dim pre_empnum, _
        pre_timin, _
        pre_timout, _
        pre_datelog, _
        pre_schedtyp As String

    Sub filldattab(ByVal thefilepath As String)

        dattabLogs.Rows.Clear()

        Dim objReader As New System.IO.StreamReader(thefilepath)

        Dim empnum, _
            timin, _
            timout, _
            datelog, _
            schedtyp As String

        Dim rowindx As Integer = 0

        Do While objReader.Peek() >= 0

            Dim logval = objReader.ReadLine()
            'Dim insRow = dattabLogs.Rows.Add(objReader.ReadLine())

            'With insRow
            If Trim(logval) <> "" Then
                empnum = getStrBetween(Trim(logval), _
                                           "", _
                                           ":")

                empnum = Trim(empnum.Substring(0, empnum.Length - 2))

                Dim prevlength = getStrBetween(Trim(logval), _
                                           "", _
                                           ":").Length

                datelog = StrReverse(getStrBetween(StrReverse(logval), _
                                                       "", _
                                                       "M"))
                datelog = datelog.Substring(0, 8)

                '01102014    09:27 AM20150203TI F
                Dim Y = datelog.Substring(0, 4)

                Dim MM = datelog.Substring(4, 2)

                Dim dd = datelog.Substring(6, 2)

                datelog = CObj(Y & "-" & MM & "-" & dd)

                schedtyp = getStrBetween(StrReverse(logval), _
                                             "", _
                                             " ")
                'MsgBox(logval)

                If logval.Contains("I") Then
                    timin = logval.Substring(prevlength - 2, _
                                                                logval.Length - prevlength)
                    timin = MilitTime(timin)

                    timout = ""

                Else
                    timin = ""

                    timout = logval.Substring(prevlength - 2, _
                                                                logval.Length - prevlength)
                    timout = MilitTime(timout)

                End If

                'pre_empnum = empnum
                'pre_timin = timin
                'pre_timout = timout
                'pre_datelog = datelog
                'pre_schedtyp = schedtyp

                Dim isTimeIn As SByte = If(timin <> "", 1, 0)

                If isTimeIn = 1 Then

                    pre_empnum = empnum
                    pre_timin = timin
                    pre_timout = timout
                    pre_datelog = datelog
                    pre_schedtyp = schedtyp

                    'Dim etentRowID = _
                    '    INSUPD_employeetimeentrydetails(, _
                    '                                    4, _
                    '                                    timin, _
                    '                                    timout, _
                    '                                    datelog, _
                    '                                    schedtyp) 'drow("EmpNum")

                    'logval'etentRowID
                    dattabLogs.Rows.Add(logval, _
                                        empnum, _
                                        timin, _
                                        timout, _
                                        datelog, _
                                        schedtyp)

                    'dgvetentdet.Rows.Add(Nothing, _
                    '                       empnum, _
                    '                       timin, _
                    '                       timout, _
                    '                       datelog, _
                    '                       schedtyp)

                    'For Each drow As DataRow In dattabLogs.Rows
                    '    'etentd_RowID = _
                    '    INSUPD_employeetimeentrydetails(, _
                    '                                    4, _
                    '                                    drow("TI"), _
                    '                                    drow("TO"), _
                    '                                    drow("Date"), _
                    '                                    drow("Type")) 'drow("EmpNum")

                    'Next

                Else

                    If pre_empnum = empnum _
                        And pre_datelog = datelog Then

                        dattabLogs.Rows(dattabLogs.Rows.Count - 1)("TO") = timout
                        'dattabLogs.Rows(dattabLogs.Rows.Count - 1)("") = 1

                    End If
                End If

                ''*******************************************
                'If rowindx = 0 Then
                'Else
                '    With dattabLogs.Rows(rowindx - 1)
                '        .Item("TI") = If(.Item("TI").ToString = "", _
                '                         pre_timin, _
                '                         .Item("TI"))

                '        .Item("TO") = If(.Item("TO").ToString = "", _
                '                         pre_timout, _
                '                         .Item("TI"))
                '    End With

                '    pre_timin = timin
                '    pre_timout = timout

                'End If

                'dattabLogs.Rows.Add(logval, _
                '                    empnum, _
                '                    timin, _
                '                    timout, _
                '                    datelog, _
                '                    schedtyp)

                'rowindx += 1
                ''*******************************************

            Else
                '.Item("EmpNum") = Nothing
                '.Item("TI") = Nothing
                '.Item("TO") = Nothing
                '.Item("Date") = Nothing
                '.Item("Type") = Nothing
            End If

            'End With

        Loop

        objReader.Close()
        objReader.Dispose()

    End Sub

    Sub fillDTTimeLog(Optional pathoffile As String = Nothing)

        'With dtTimeLogs.Columns

        '.Add("EmpID", Type.GetType("System.String"))
        '.Add("TimeLog", Type.GetType("System.String"))
        '.Add("DateLog", Type.GetType("System.String"))
        '.Add("TypeLog", Type.GetType("System.String"))

        'End With
        Dim timelogImportID = ValNoComma(New ExecuteQuery("SELECT MAX(ImportID) FROM timeentrylog;").Result) + 1
        If pathoffile <> Nothing Then

            Dim objReader As New System.IO.StreamReader(thefilepath)

            dtTimeLogs.Rows.Clear()

            Do While objReader.Peek() >= 0

                Dim redline = objReader.ReadLine()

                Dim red_line = Trim(redline).Replace(vbTab, _
                                                " ")

                Dim strings = Split(red_line, _
                                    " ") 'vbTab

                Dim ii = containCollection(strings)

                Dim Employee_ID = ii.Item(0)
                Dim DateTimeLogStamp = ii.Item(1) & " " & ii.Item(2)
                ''For Each strval As String In ii
                'MsgBox(ii.Item(0) & vbNewLine & _
                '        getDataInList(ii, "date") & vbNewLine & _
                '        getDataInList(ii, "time"))
                ''Next
                'Dim INSUPD_timeentrylog As New ExecuteQuery("SELECT INSUPD_timeentrylog('" & orgztnID & "','" & Employee_ID & "','" & DateTimeLogStamp & "','" & timelogImportID & "');")
                If redline.Contains("TI") _
                    Or redline.Contains("TO") Then

                    Dim result1 = ii.Item(1)

                    Dim result2 = ii.Item(ii.Count - 1)

                    Dim _array = Split(redline, vbTab)

                    Dim fsdfsdf = _array(1)

                    Dim array_maxcount = _array.GetUpperBound(0)

                    Dim iouioui = _array(array_maxcount)

                    dtTimeLogs.Rows.Add(Employee_ID, _
                                        getDataInList(ii, "date"), _
                                        getDataInList(ii, "time"), _
                                        String.Empty,
                                        _array(1),
                                        _array(array_maxcount),
                                        If(redline.Contains("TI"), 1, 0))
                    '1 as time in
                    '0 as time out
                Else

                    dtTimeLogs.Rows.Add(Employee_ID, _
                                        getDataInList(ii, "date"), _
                                        getDataInList(ii, "time"), _
                                        String.Empty,
                                        Nothing,
                                        Nothing,
                                        Nothing)

                End If

            Loop

            objReader.Close()
            objReader.Dispose()

        End If

    End Sub

    Function containCollection(ByVal lists() As String, _
                               Optional SplitDelimiter As String = " ") As List(Of String)

        Dim listContainer As New List(Of String)

        For Each strval As String In lists

            If strval.Trim <> String.Empty Then

                Dim pluckBetweenSpace = Split(strval, SplitDelimiter)

                For Each strvalue As String In pluckBetweenSpace

                    If strvalue.Trim <> String.Empty Then

                        listContainer.Add(Trim(strvalue))

                    End If

                Next

            End If

        Next

        Return listContainer

    End Function

    Function getDataInList(ByVal listofstr As List(Of String), _
                           ByVal NameOfDataType As String) As Object

        Dim returnval = Nothing

        If NameOfDataType.ToLower = "date" Then

            For Each strval As String In listofstr
                Try
                    'returnval = CDate(strval)

                    If strval.Length = 10 Then

                        If strval.Contains("-") Then
                            returnval = Format(CDate(strval), "yyyy-MM-dd")
                            Exit For
                        ElseIf strval.Contains("/") Then
                            returnval = Format(CDate(strval), "yyyy-MM-dd")
                            Exit For
                        Else
                            Continue For
                        End If

                    End If

                Catch ex As Exception
                    returnval = Nothing
                    Continue For
                End Try
            Next

        ElseIf NameOfDataType.ToLower = "time" Then

            For Each strval As String In listofstr
                Try
                    If strval.Contains(":") Then
                        If strval.Length = 8 Then
                            returnval = Format(CDate(strval), "HH:mm:ss")
                            Exit For
                        ElseIf strval.Length = 5 Then
                            returnval = Format(CDate(strval), "HH:mm:ss")
                            Exit For
                        End If
                    Else
                        Continue For
                    End If

                Catch ex As Exception
                    returnval = Nothing
                    Continue For
                End Try
            Next

        End If

        Return returnval

    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) 'Handles Button2.Click
        If Not bgworkImport.IsBusy Then
            'DataGridView1.Rows.Clear()
            'DataGridView1.Columns.Clear()

            'dgvetentdet.DataSource = dattabLogs

            'BackgroundWorker1.RunWorkerAsync()

            'For Each drow As DataRow In dattabLogs.Rows
            '    For Each c As DataColumn In dattabLogs.Columns

            '    Next
            'Next
            MsgBox("not busy")
        Else
            MsgBox("busy")
            'Button2_Click(sender, e)
        End If

    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        'If e.Error IsNot Nothing Then
        '    MessageBox.Show("Error: " & e.Error.Message)
        'ElseIf e.Cancelled Then
        '    'MessageBox.Show("Word counting canceled.") '& vbNewLine & e.Result.ToString)
        '    MessageBox.Show("Background work cancelled.") '& vbNewLine & e.Result.ToString)
        'Else
        '    'MessageBox.Show("Finished counting words.") ' & vbNewLine & e.Result.ToString)
        '    MessageBox.Show("Importing finished successfully.") ' & vbNewLine & e.Result.ToString)
        'End If

    End Sub

    Function MilitTime(ByVal timeval As Object) As Object

        Dim retrnObj As Object

        retrnObj = New Object

        If timeval = Nothing Then
            retrnObj = Nothing
        Else

            Dim endtime As Object = timeval

            If endtime.ToString.Contains("P") Then

                Dim hrs As String = If(Val(getStrBetween(endtime, "", ":")) = 12, 12, Val(getStrBetween(endtime, "", ":")) + 12)

                Dim mins As String = StrReverse(getStrBetween(StrReverse(endtime.ToString), "", ":"))

                mins = getStrBetween(mins, "", " ")

                retrnObj = hrs & ":" & mins

            ElseIf endtime.ToString.Contains("A") Then

                Dim i As Integer = StrReverse(endtime).ToString.IndexOf(" ")

                endtime = endtime.ToString.Replace("A", "")

                'Dim i As Integer = StrReverse("3:15 AM").ToString.IndexOf(" ")

                ''endtime = endtime.ToString.Replace("A", "")

                'MsgBox(Trim(StrReverse(StrReverse("3:15 AM").ToString.Substring(i, ("3:15 AM").ToString.Length - i))).Length)

                Dim amTime As String = Trim(StrReverse(StrReverse(endtime.ToString).Substring(i, _
                                                                                  endtime.ToString.Length - i)
                                          )
                               )

                amTime = If(getStrBetween(amTime, "", ":") = "12", _
                            24 & ":" & StrReverse(getStrBetween(StrReverse(amTime), "", ":")), _
                            amTime)

                retrnObj = amTime

            End If

        End If

        Return retrnObj

    End Function

    Dim emp_num, _
        t_in, _
        t_out, _
        logdate, _
        typ As Object

    Dim emp_numb As String

    Private Sub Button3_Click(sender As Object, e As EventArgs) ' Handles Button3.Click

        'Dim etentd_RowID As Integer = Nothing

        'Dim distinctlogdate As New List(Of String)

        'Dim issamedate As String = Nothing
        'For Each drow As DataRow In dattabLogs.Rows
        '    If issamedate <> drow("Date").ToString Then
        '        distinctlogdate.Add(drow("Date").ToString)
        '    End If
        'Next

        'For Each d_date In distinctlogdate

        '    Dim selrow = dattabLogs.Select("Date='" & d_date & "'")

        '    For Each drw In selrow
        '        For Each c As DataColumn In dattabLogs.Columns
        '            'MsgBox(drw(c).ToString)
        '            If emp_numb <> drw("EmpNum").ToString Then
        '                emp_numb = drw("EmpNum").ToString

        '                If t_out <> drw("TO").ToString Then
        '                    t_out = drw("TO").ToString

        '                End If

        '            End If
        '        Next
        '    Next

        'Next

        'For Each drow As DataRow In dattabLogs.Rows

        '    'etentd_RowID = _
        '    INSUPD_employeetimeentrydetails(, _
        '                                    4, _
        '                                    drow("TI"), _
        '                                    drow("TO"), _
        '                                    drow("Date"), _
        '                                    drow("Type")) 'drow("EmpNum")

        '    'If emp_num <> drow("EmpNum").ToString _
        '    '        And logdate <> drow("Date").ToString Then

        '    '    emp_num = drow("EmpNum").ToString
        '    '    t_in = drow("TI").ToString
        '    '    t_out = drow("TO").ToString
        '    '    logdate = drow("Date").ToString
        '    '    typ = drow("Type").ToString

        '    '    If drow("TO").ToString = "" Then
        '    '        'etentd_RowID = _
        '    '        INSUPD_employeetimeentrydetails(etentd_RowID, _
        '    '                                        4, _
        '    '                                        drow("TI"), _
        '    '                                        drow("TO"), _
        '    '                                        drow("Date"), _
        '    '                                        drow("Type")) 'drow("EmpNum")
        '    '    End If

        '    'Else
        '    '    If drow("EmpNum").ToString = emp_num _
        '    '        And drow("Date").ToString = logdate Then

        '    '        If drow("TO").ToString = "" Then
        '    '            'etentd_RowID = _
        '    '            INSUPD_employeetimeentrydetails(etentd_RowID, _
        '    '                                            4, _
        '    '                                            drow("TI"), _
        '    '                                            drow("TO"), _
        '    '                                            drow("Date"), _
        '    '                                            drow("Type")) 'drow("EmpNum")
        '    '        End If

        '    '    End If

        '    'End If

        'Next
    End Sub

    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Function INSUPD_employeetimeentrydetails(Optional etentd_RowID As Object = Nothing, _
                                             Optional etentd_EmployeeID As Object = Nothing, _
                                             Optional etentd_TimeIn As Object = Nothing, _
                                             Optional etentd_TimeOut As Object = Nothing, _
                                             Optional etentd_Date As Object = Nothing, _
                                             Optional etentd_TimeScheduleType As Object = Nothing, _
                                             Optional etentd_Created As Object = Nothing, _
                                             Optional etentd_TimeEntryStatus As Object = Nothing,
                                             Optional EditAsUnique As String = "0",
                                             Optional Branch_Code As Object = Nothing,
                                             Optional DateTimeLogIn As Object = Nothing,
                                             Optional DateTimeLogOut As Object = Nothing) As Object

        Static mysql_date_format As String = String.Empty

        Static vb_date_format As String = String.Empty

        Static machineShortTime As String = String.Empty

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            mysql_date_format = New ExecuteQuery("SELECT @@date_format;").Result
            
            vb_date_format = mysql_date_format.Replace("%", "")
            
            vb_date_format = vb_date_format.Replace("Y", "y")

            vb_date_format = vb_date_format.Replace("m", "M")

            machineShortTime = RegKey.GetValue("sTimeFormat").ToString 'sShortTime
            
            machineShortTime = machineShortTime.Replace("t", "").Trim

            machineShortTime = machineShortTime.Replace("h", "H").Trim

        End If

        Dim params(9, 2) As Object

        'params(0, 0) = "etentd_RowID"
        'params(1, 0) = "etentd_OrganizationID"
        'params(2, 0) = "etentd_CreatedBy"
        'params(3, 0) = "etentd_LastUpdBy"
        'params(4, 0) = "etentd_EmployeeID"
        'params(5, 0) = "etentd_TimeIn"
        'params(6, 0) = "etentd_TimeOut"
        'params(7, 0) = "etentd_Date"
        'params(8, 0) = "etentd_TimeScheduleType"

        'params(0, 1) = If(etentd_RowID = Nothing, DBNull.Value, etentd_RowID)
        'params(1, 1) = orgztnID
        'params(2, 1) = 2 'CreatedBy
        'params(3, 1) = 2 'LastUpdBy
        'params(4, 1) = If(etentd_EmployeeID = Nothing, DBNull.Value, etentd_EmployeeID)
        'params(5, 1) = If(etentd_TimeIn = Nothing, DBNull.Value, etentd_TimeIn)
        'params(6, 1) = If(etentd_TimeOut = Nothing, DBNull.Value, etentd_TimeOut)
        'params(7, 1) = etentd_Date
        'params(8, 1) = If(etentd_TimeScheduleType = Nothing, DBNull.Value, etentd_TimeScheduleType)

        'INSUPD_employeetimeentrydetails = _
        '    EXEC_INSUPD_PROCEDURE(params, _
        '                          "INSUPD_employeetimeentrydetails", _
        '                          "etentdID")
        Dim return_value = Nothing
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("INSUPD_employeetimeentrydetails", conn)
            conn.Open()
            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("etentdID", MySqlDbType.Int32)

                'Dim rowid = If(etentd_RowID = Nothing, DBNull.Value, etentd_RowID)

                'MsgBox(rowid.ToString)

                .Parameters.AddWithValue("etentd_RowID", If(etentd_RowID = Nothing, DBNull.Value, etentd_RowID))
                .Parameters.AddWithValue("etentd_OrganizationID", orgztnID)
                .Parameters.AddWithValue("etentd_CreatedBy", z_User)
                .Parameters.AddWithValue("etentd_Created", etentd_Created)
                .Parameters.AddWithValue("etentd_LastUpdBy", z_User)
                .Parameters.AddWithValue("etentd_EmployeeID", If(etentd_EmployeeID = Nothing, DBNull.Value, etentd_EmployeeID))

                If IsDBNull(etentd_TimeIn) Then
                    .Parameters.AddWithValue("etentd_TimeIn", etentd_TimeIn)
                Else
                    .Parameters.AddWithValue("etentd_TimeIn", If(etentd_TimeIn = Nothing, DBNull.Value, etentd_TimeIn))
                End If

                If IsDBNull(etentd_TimeOut) Then
                    .Parameters.AddWithValue("etentd_TimeOut", etentd_TimeOut)
                Else
                    .Parameters.AddWithValue("etentd_TimeOut", If(etentd_TimeOut = Nothing, DBNull.Value, etentd_TimeOut))
                End If

                .Parameters.AddWithValue("etentd_Date", etentd_Date)

                .Parameters.AddWithValue("etentd_TimeScheduleType", If(etentd_TimeScheduleType = Nothing, String.Empty, Trim(etentd_TimeScheduleType)))

                .Parameters.AddWithValue("etentd_TimeEntryStatus", If(etentd_TimeEntryStatus = Nothing, String.Empty, etentd_TimeEntryStatus))

                .Parameters.AddWithValue("EditAsUnique", EditAsUnique)


                If IsDBNull(Branch_Code) Then
                    .Parameters.AddWithValue("Branch_Code", Branch_Code)

                Else
                    .Parameters.AddWithValue("Branch_Code", If(Branch_Code = Nothing, DBNull.Value, Branch_Code))

                End If

                Dim custom_datetimeformat = String.Concat(vb_date_format, " ", machineShortTime)

                If IsDBNull(DateTimeLogIn) Then
                    .Parameters.AddWithValue("DateTimeLogIn", DateTimeLogIn)

                Else

                    .Parameters.AddWithValue("DateTimeLogIn", If(DateTimeLogIn = Nothing, DBNull.Value,
                                                                 String.Concat(Format(CDate(DateTimeLogIn),
                                                                                      custom_datetimeformat))))

                End If

                If IsDBNull(DateTimeLogOut) Then
                    .Parameters.AddWithValue("DateTimeLogOut", DateTimeLogOut)

                Else
                    .Parameters.AddWithValue("DateTimeLogOut", If(DateTimeLogOut = Nothing, DBNull.Value,
                                                                 String.Concat(Format(CDate(DateTimeLogOut),
                                                                                      custom_datetimeformat))))

                End If

                .Parameters("etentdID").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader() '.ExecuteScalar()
                'INSUPD_employeetimeentrydetails
                return_value = datread(0) 'Return value'CType(.ExecuteScalar(), Integer)

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " " & "INSUPD_employeetimeentrydetails", , "Error")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return return_value
    End Function

    Private Sub bgworkImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkImport.DoWork

        backgroundworking = 1

        'filldattab(thefilepath)

        'For Each drow As DataRow In dattabLogs.Rows
        '    'etentd_RowID = _
        '    INSUPD_employeetimeentrydetails(, _
        '                                    4, _
        '                                    drow("TI"), _
        '                                    drow("TO"), _
        '                                    drow("Date"), _
        '                                    drow("Type")) 'drow("EmpNum")

        'Next

        '**************************************************

        'With dtTimeLogs.Columns

        '.Add("EmpID", Type.GetType("System.String"))
        '.Add("TimeLog", Type.GetType("System.String"))
        '.Add("DateLog", Type.GetType("System.String"))
        '.Add("TypeLog", Type.GetType("System.String"))

        'End With

        fillDTTimeLog(thefilepath)

        dtImport.Rows.Clear()

        '**************************************************

        Dim distinctID = dtTimeLogs.DefaultView.ToTable(True, "EmpID")

        Dim distinctDateLog = dtTimeLogs.DefaultView.ToTable(True, "DateLog")

        Dim EmpIDAndDate As String = String.Empty

        Dim timeOne As String = String.Empty

        Dim timeTwo As String = String.Empty

        Dim distinctID_RowCount = distinctID.Rows.Count

        Dim loop_indx = 1

        For Each drow As DataRow In distinctID.Rows

            If IsDBNull(drow(0)) Then
                Continue For
            ElseIf drow(0) = Nothing Then
                Continue For
            Else

                For Each d_row As DataRow In distinctDateLog.Rows

                    Dim empsel_dtTimeLogs = dtTimeLogs.Select("EmpID = '" & drow(0) & "' AND DateLog = '" & d_row(0) & "'")

                    If empsel_dtTimeLogs.Count = 0 Then
                        Continue For
                    ElseIf empsel_dtTimeLogs.Count = 1 Then
                        
                        If empsel_dtTimeLogs(0)("Tag") Is Nothing Then

                            timeOne = If(IsDBNull(empsel_dtTimeLogs(0)("TimeLog")), "", empsel_dtTimeLogs(0)("TimeLog"))

                            dtImport.Rows.Add(drow(0), _
                                              empsel_dtTimeLogs(0)("TimeLog"), _
                                              Nothing, _
                                              d_row(0))

                        Else

                            Dim fsdfsd = empsel_dtTimeLogs(0)("DateTimeLog")

                            If ValNoComma(empsel_dtTimeLogs(0)("Tag")) = 1 Then 'Time in

                                dtImport.Rows.Add(drow(0), _
                                                  empsel_dtTimeLogs(0)("TimeLog"), _
                                                  Nothing, _
                                                  d_row(0),
                                                  empsel_dtTimeLogs(0)("DateTimeLog"),
                                                  Nothing, _
                                                  empsel_dtTimeLogs(0)("BranchCode"))

                            ElseIf ValNoComma(empsel_dtTimeLogs(0)("Tag")) = 0 Then 'Time out

                                dtImport.Rows.Add(drow(0), _
                                                  Nothing, _
                                                  empsel_dtTimeLogs(0)("TimeLog"), _
                                                  d_row(0),
                                                  Nothing, _
                                                  empsel_dtTimeLogs(0)("DateTimeLog"),
                                                  empsel_dtTimeLogs(0)("BranchCode"))

                            End If

                        End If

                    ElseIf empsel_dtTimeLogs.Count > 1 Then

                            Dim lastrow_indx = empsel_dtTimeLogs.Count - 1

                            timeOne = If(IsDBNull(empsel_dtTimeLogs(0)("TimeLog")), "", empsel_dtTimeLogs(0)("TimeLog"))

                            timeTwo = If(IsDBNull(empsel_dtTimeLogs(lastrow_indx)("TimeLog")), "", empsel_dtTimeLogs(lastrow_indx)("TimeLog"))

                            dtImport.Rows.Add(drow(0), _
                                              timeOne, _
                                              timeTwo, _
                                              d_row(0),
                                              empsel_dtTimeLogs(0)("DateTimeLog"),
                                              empsel_dtTimeLogs(lastrow_indx)("DateTimeLog"),
                                              empsel_dtTimeLogs(lastrow_indx)("BranchCode"))

                            Dim fsdfsd = empsel_dtTimeLogs(0)("DateTimeLog")

                            Dim fsdfsdff = empsel_dtTimeLogs(lastrow_indx)("DateTimeLog")

                    End If

                Next

            End If

            bgworkImport.ReportProgress(CInt(50 * loop_indx / distinctID_RowCount), "")

            loop_indx += 1

        Next

        'For Each drow As DataRow In distinctID.Rows

        '    Dim myselect = From selrow In dtTimeLogs
        '                   Where selrow.Field(Of String)("EmpID") = drow(0)
        '                   Order By selrow.Field(Of String)("DateLog") Ascending

        '    Dim successtwo = 0

        '    Dim prev_date = Nothing

        '    For Each drw As DataRow In myselect

        '        If EmpIDAndDate <> drw(0).ToString & drw(1).ToString Then

        '            EmpIDAndDate = drw(0).ToString & drw(1).ToString

        '            If successtwo = 1 Then

        '                successtwo = 1

        '                'Try
        '                '    timeOne = If(CDate(drw(2)) > CDate(timeOne), timeOne, drw(2))
        '                'Catch ex As Exception
        '                '    timeOne = Nothing
        '                'End Try

        '                'Try
        '                '    timeTwo = If(CDate(drw(2)) > CDate(timeOne), drw(2), timeOne)
        '                'Catch ex As Exception
        '                '    timeTwo = timeOne
        '                'End Try

        '                'Dim selectexist = dtImport.Select("EmploID='" & drw(0).ToString & "' AND LogDate='" & drw(1).ToString & "'")

        '                dtImport.Rows.Add(drw(0).ToString, _
        '                                  timeOne, _
        '                                  timeTwo, _
        '                                  drw(1).ToString)

        '            ElseIf successtwo = 2 Then

        '                successtwo = 0

        '                dtImport.Rows.Add(drw(0).ToString, _
        '                                  timeOne, _
        '                                  timeTwo, _
        '                                  prev_date) 'drw(1).ToString

        '            Else
        '                successtwo = 1

        '            End If

        '            prev_date = drw(1)

        '            If IsDBNull(drw(2)) Then
        '                timeOne = String.Empty
        '            Else
        '                timeOne = drw(2).ToString
        '            End If

        '        Else

        '            'timeOne = If(CDate(drw(2)) > CDate(timeOne), timeOne, drw(2))

        '            'timeTwo = If(CDate(drw(2)) > CDate(timeOne), drw(2), timeOne)

        '            If IsDBNull(drw(2)) Then
        '                timeTwo = String.Empty
        '            Else
        '                timeTwo = drw(2).ToString
        '            End If

        '            'dtImport.Rows.Add(drw(0).ToString, _
        '            '                  timeOne, _
        '            '                  timeTwo, _
        '            '                  drw(1).ToString)

        '            'With dtImport.Columns

        '            '    .Add("EmploID", Type.GetType("System.String"))
        '            '    .Add("TIn", Type.GetType("System.String"))
        '            '    .Add("TOut", Type.GetType("System.String"))
        '            '    .Add("LogDate", Type.GetType("System.String"))

        '            'End With

        '            successtwo = 2

        '        End If

        '        'MsgBox(drw(0).ToString & vbNewLine & _
        '        '       drw(1).ToString & vbNewLine & _
        '        '       drw(2).ToString)

        '    Next

        '    'MsgBox("NEXT")

        'Next

        '**************************************************

        'Dim names = From row In dtTimeLogs.AsEnumerable()
        '            Select row.Field(Of String)("EmpID") Distinct

        ''For Each strval In names.ToList
        'MsgBox(names.ToList.Count)

        ''Next  

        '**************************************************

        'Dim GroupedSum = From userentry In dtTimeLogs
        '                Group userentry By key = userentry.Field(Of String)("EmpID") Into Group _
        '                Select ProductID = key, SumVal = Group.Sum(Function(p) p("Value"))

        'For Each strr In GroupedSum.ToList
        '    MsgBox(strr.ToString)
        '    '    'MsgBox(strr.ProductID & vbNewLine & strr.SumVal)

        '    '    INSUPD_paystubitem(, paystubID, strr.ProductID, strr.SumVal)

        '    '    includedallowance.Add(strr.ProductID)

        'Next

    End Sub

    Private Sub bgworkImport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkImport.ProgressChanged

        Threading.Thread.Sleep(0)

        ToolStripProgressBar1.Value = CType(e.ProgressPercentage, Integer)

    End Sub

    Dim progress_value = 0

    Private Sub bgworkImport_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkImport.RunWorkerCompleted

        Dim balloon_x = lblforballoon.Location.X

        If e.Error IsNot Nothing Then
            
            'MessageBox.Show("Error: " & e.Error.Message)
            MessageBox.Show(getErrExcptn(e.Error, Me.Name))

            progress_value = ToolStripProgressBar1.Value

            tsbtnNew.Enabled = True

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")

            tsbtnNew.Enabled = True

        Else

            lblforballoon.Location = New Point(TabControl1.Location.X, lblforballoon.Location.Y)

            'If dgvetentd.RowCount = 0 Then
            '    dgvetentd.Rows.Add(Format(CDate(dbnow), "MMM-dd-yyyy"))
            'Else
            '    If dgvetentd.RowCount >= 10 Then
            '        dgvetentd.Rows.Remove(dgvetentd.Rows(9))
            '        dgvetentd.Rows.Insert(0, 1)
            '        dgvetentd.Item(0, 0).Value = Format(CDate(dbnow), "MMM-dd-yyyy")
            '    Else
            '        dgvetentd.Rows.Insert(0, 1)
            '        dgvetentd.Item(0, 0).Value = Format(CDate(dbnow), "MMM-dd-yyyy")
            '    End If
            'End If

            dgvetentdet.Rows.Clear()

            progress_value = ToolStripProgressBar1.Value

            bgworkInsertImport.RunWorkerAsync()

        End If

        backgroundworking = 0

    End Sub

    Private Sub bgworkInsertImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkInsertImport.DoWork

        backgroundworking = 1

        'With dtTimeLogs.Columns

        '.Add("EmpID", Type.GetType("System.String"))
        '.Add("TimeLog", Type.GetType("System.String"))
        '.Add("DateLog", Type.GetType("System.String"))
        '.Add("TypeLog", Type.GetType("System.String"))

        'End With

        Dim currtimestamp = Format(CDate(EXECQUER("SELECT CURRENT_TIMESTAMP();")), "yyyy-MM-dd HH:mm:ss")


        'With dtImport.Columns

        '    .Add("EmploID", Type.GetType("System.String"))
        '    .Add("TIn", Type.GetType("System.String"))
        '    .Add("TOut", Type.GetType("System.String"))
        '    .Add("LogDate", Type.GetType("System.String"))

        'End With
        Dim lastbound = dtImport.Rows.Count 'dattabLogs

        Dim indx = 1

        For Each drow As DataRow In dtImport.Rows 'dattabLogs

            'INSUPD_employeetimeentrydetails(, _
            '                                drow("EmpNum"), _
            '                                drow("TI"), _
            '                                drow("TO"), _
            '                                drow("Date"), _
            '                                drow("Type"), _
            '                                currtimestamp) 'drow("EmpNum")

            'MsgBox(drow("EmploID") & vbNewLine & _
            '        drow("TIn") & vbNewLine & _
            '        drow("TOut") & vbNewLine & _
            '        drow("LogDate"))

            'With dtImport.Columns

            '    .Add("EmploID", Type.GetType("System.String"))
            '    .Add("TIn", Type.GetType("System.String"))
            '    .Add("TOut", Type.GetType("System.String"))
            '    .Add("LogDate", Type.GetType("System.String"))

            '    .Add("DateTimeLogIn", Type.GetType("System.String"))

            '    .Add("DateTimeLogOut", Type.GetType("System.String"))

            '    .Add("BranchCode", Type.GetType("System.String"))

            'End With

            INSUPD_employeetimeentrydetails(, _
                                            drow("EmploID"), _
                                            drow("TIn"), _
                                            drow("TOut"), _
                                            drow("LogDate"), _
                                            "", _
                                            currtimestamp, , "1",
                                            drow("BranchCode"),
                                            drow("DateTimeLogIn"),
                                            drow("DateTimeLogOut"))

            'IO.File.AppendAllText(IO.Path.GetTempPath() & "aaa.txt", _
            '                      drow("EmploID").ToString & " " & _
            '                      drow("TIn").ToString & " " & _
            '                      drow("TOut").ToString & " " & _
            '                      drow("LogDate").ToString & Environment.NewLine)

            Dim progressvalue = CInt((indx / lastbound) * 50)

            bgworkInsertImport.ReportProgress(progressvalue)

            indx += 1

        Next

        EXECQUER("UPDATE employeetimeentrydetails SET TimeScheduleType='' WHERE TimeScheduleType IS NULL AND OrganizationID='" & orgztnID & "';")

        'For Each drow As DataRow In dattabLogs.Rows
        '    'drow("logging").ToString
        '    dgvetentdet.Rows.Add(Nothing, _
        '                         drow("EmpNum").ToString, _
        '                         drow("TI").ToString, _
        '                         drow("TO").ToString, _
        '                         Format(CDate(drow("Date")), "MM-dd-yyyy"),    _
        '                         drow("Type").ToString)

        '    'INSUPD_employeetimeentrydetails(, _
        '    '                                4, _
        '    '                                drow("TI"), _
        '    '                                drow("TO"), _
        '    '                                drow("Date"), _
        '    '                                drow("Type")) 'drow("EmpNum")
        'Next

    End Sub

    Private Sub bgworkInsertImport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkInsertImport.ProgressChanged

        ToolStripProgressBar1.Value = progress_value + CType(e.ProgressPercentage, Integer)

    End Sub

    Private Sub bgworkInsertImport_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkInsertImport.RunWorkerCompleted

        Dim balloon_x = lblforballoon.Location.X

        If e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message)

            tsbtnNew.Enabled = True

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")

            tsbtnNew.Enabled = True

        Else

            loademployeetimeentrydetails(0)

            InfoBalloon(, , lblforballoon, , , 1)

            InfoBalloon(IO.Path.GetFileName(thefilepath) & " imported successfully.", _
                      "Importing file finished", lblforballoon, 0, -69)

        End If

        ToolStripProgressBar1.Visible = False

        ToolStripProgressBar1.Value = 0

        progress_value = 0

        tsbtnNew.Enabled = True

        Panel1.Enabled = True

        backgroundworking = 0

        dgvetentd_SelectionChanged(sender, e)

        AddHandler dgvetentd.SelectionChanged, AddressOf dgvetentd_SelectionChanged

        lblforballoon.Location = New Point(balloon_x, lblforballoon.Location.Y)

    End Sub

    Private Sub dgvetentdet_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvetentdet.CellContentClick

    End Sub

    Dim haserrinput As SByte

    Dim listofEditRow As New AutoCompleteStringCollection

    Dim reset_static As SByte = -1

    Private Sub dgvetentdet_CellEndEdit1(sender As Object, e As DataGridViewCellEventArgs) 'Handles dgvetentdet.CellEndEdit

        dgvetentdet.ShowCellErrors = True
        Dim colName As String = dgvetentdet.Columns(e.ColumnIndex).Name
        Dim rowindx = e.RowIndex

        Static num As Integer = If(reset_static = -1, _
                                   -1, _
                                   num)

        If dgvetentdet.RowCount <> 0 Then
            With dgvetentdet

                If Val(dgvetentdet.Item("Column1", e.RowIndex).Value) <> 0 Then
                    'If num <> Val(dgvetentdet.Item("Column1", e.RowIndex).Value) Then
                    '    num = Val(dgvetentdet.Item("Column1", e.RowIndex).Value)
                    listofEditRow.Add(dgvetentdet.Item("Column1", e.RowIndex).Value)
                    'End If
                Else

                End If

                'Column3'Column4'Column5

                If (colName = "Column5") Then

                    If Trim(dgvetentdet.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvetentdet.Item(colName, rowindx).Value)
                        Try
                            dgvetentdet.Item(colName, rowindx).Value = Format(CDate(dateobj), "M/dd/yyyy")

                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            haserrinput = 1
                            dgvetentdet.Item(colName, rowindx).ErrorText = "     Invalid date value"
                        End Try
                    Else
                        haserrinput = 0

                        dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                    End If

                ElseIf (colName = "Column3" Or colName = "Column4") Then

                    If Trim(dgvetentdet.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvetentdet.Item(colName, rowindx).Value).Replace(" ", ":")

                        Dim dateobj_len = dateobj.ToString.Length

                        Dim ampm As String = Nothing

                        Try
                            If dateobj.ToString.Contains("A") Or _
                                dateobj.ToString.Contains("P") Or _
                                dateobj.ToString.Contains("M") Then

                                ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))

                                dateobj_len -= ampm.Length

                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, dateobj_len)) 'dateobj.ToString.Substring(0, 4)
                                dateobj = dateobj.ToString.Replace(" ", ":")

                                dateobj &= ampm

                            End If
                            '    dateobj = getStrBetween(dateobj.ToString, "", " ")
                            '    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm")
                            '    dgvempleave.Item(colName, rowIndx).Value = valtime.ToLongTimeString
                            'Else

                            Dim modified_format = If(dateobj_len = 5, "h:mm tt", "hh:mm:ss tt")

                            Dim valtime As DateTime = DateTime.Parse(dateobj).ToString(modified_format)
                            If ampm = Nothing Then
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToLongTimeString
                            Else
                                dgvetentdet.Item(colName, rowindx).Value = Trim(valtime.ToLongTimeString.Substring(0, (dateobj_len - 1))) & ampm
                            End If
                            'End If
                            'valtime = DateTime.Parse(e.FormattedValue)
                            'valtime = valtime.ToLongTimeString
                            'Format(valtime, "hh:mm tt")
                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            Try
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, dateobj_len))
                                dateobj = dateobj.ToString.Replace(" ", ":")

                                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm:ss")
                                'valtime = DateTime.Parse(e.FormattedValue)
                                'valtime = valtime.ToLongTimeString
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToLongTimeString
                                'Format(valtime, "hh:mm tt")
                                haserrinput = 0

                                dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                            Catch ex_1 As Exception
                                haserrinput = 1
                                dgvetentdet.Item(colName, rowindx).ErrorText = "     Invalid time value"
                            End Try
                        End Try
                    Else
                        haserrinput = 0

                        dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                    End If
                    'Else 'Column6
                    '    haserrinput = 0
                    '    dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                End If

            End With
        End If

        'dgvetentdet.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells
        'dgvetentdet.AutoResizeRow(e.RowIndex)
        'dgvetentdet.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None

    End Sub

    Private Sub dgvetentdet_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvetentdet.CellEndEdit

        dgvetentdet.ShowCellErrors = True

        Dim colName As String = dgvetentdet.Columns(e.ColumnIndex).Name
        Dim rowindx = e.RowIndex

        Static num As Integer = If(reset_static = -1, _
                                   -1, _
                                   num)

        If dgvetentdet.RowCount <> 0 Then
            With dgvetentdet

                If Val(dgvetentdet.Item("Column1", e.RowIndex).Value) <> 0 Then
                    'If num <> Val(dgvetentdet.Item("Column1", e.RowIndex).Value) Then
                    '    num = Val(dgvetentdet.Item("Column1", e.RowIndex).Value)
                    listofEditRow.Add(dgvetentdet.Item("Column1", e.RowIndex).Value)
                    'End If
                Else

                End If

                'Column3'Column4'Column5

                If (colName = "Column5") Then

                    If Trim(dgvetentdet.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvetentdet.Item(colName, rowindx).Value)
                        Try
                            dgvetentdet.Item(colName, rowindx).Value = Format(CDate(dateobj), "M/dd/yyyy")

                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            haserrinput = 1
                            dgvetentdet.Item(colName, rowindx).ErrorText = "     Invalid date value"
                        End Try
                    Else
                        haserrinput = 0

                        dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                    End If

                ElseIf (colName = "Column3" Or colName = "Column4") Then

                    If Trim(dgvetentdet.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvetentdet.Item(colName, rowindx).Value).Replace(" ", ":")

                        Dim ampm As String = Nothing

                        Try
                            If dateobj.ToString.Contains("A") Or _
                        dateobj.ToString.Contains("P") Or _
                        dateobj.ToString.Contains("M") Then

                                ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                                ampm = Microsoft.VisualBasic.Left(ampm, 2) & "M"
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                                dateobj = dateobj.ToString.Replace(" ", ":")

                            End If
                            '    dateobj = getStrBetween(dateobj.ToString, "", " ")
                            '    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm")
                            '    dgvempleave.Item(colName, rowIndx).Value = valtime.ToShortTimeString
                            'Else
                            Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")
                            If ampm = Nothing Then
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToShortTimeString
                            Else
                                dgvetentdet.Item(colName, rowindx).Value = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                            End If
                            'End If
                            'valtime = DateTime.Parse(e.FormattedValue)
                            'valtime = valtime.ToShortTimeString
                            'Format(valtime, "hh:mm tt")
                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            Try
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, 5))
                                dateobj = dateobj.ToString.Replace(" ", ":")

                                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")
                                'valtime = DateTime.Parse(e.FormattedValue)
                                'valtime = valtime.ToShortTimeString
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToShortTimeString
                                'Format(valtime, "hh:mm tt")
                                haserrinput = 0

                                dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                            Catch ex_1 As Exception
                                haserrinput = 1
                                dgvetentdet.Item(colName, rowindx).ErrorText = "     Invalid time value"
                            End Try
                        End Try
                    Else
                        haserrinput = 0

                        dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                    End If
                    'Else 'Column6
                    '    haserrinput = 0
                    '    dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                End If

            End With
        End If

        'dgvetentdet.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells
        'dgvetentdet.AutoResizeRow(e.RowIndex)
        'dgvetentdet.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None

    End Sub

    Dim pagination As Integer

    Private Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked, _
                                                                                                Nxt.LinkClicked, Last.LinkClicked

        RemoveHandler dgvetentd.SelectionChanged, AddressOf dgvetentd_SelectionChanged

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "First" Then
            pagination = 0
        ElseIf sendrname = "Prev" Then
            If pagination - 100 < 0 Then
                pagination = 0
            Else : pagination -= 100
            End If
        ElseIf sendrname = "Nxt" Then
            pagination += 100
        ElseIf sendrname = "Last" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(DISTINCT(Created)) / 100 FROM employeetimeentrydetails WHERE OrganizationID=" & orgztnID & ";"))



            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * 100

            If pagination - 100 < 100 Then
                'pagination = 0

            End If

            'pagination = If(lastpage - 100 >= 100, _
            '                lastpage - 100, _
            '                lastpage)

        End If

        loademployeetimeentrydetails(pagination)

        dgvetentd_SelectionChanged(sender, e)

        AddHandler dgvetentd.SelectionChanged, AddressOf dgvetentd_SelectionChanged

    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Me.Close()
    End Sub

    Private Sub Form8_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If backgroundworking = 1 Then

            e.Cancel = True

            'Dim prompt = MessageBox.Show("Do you want to log out ?", "Confirm logging out", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

            'If prompt = MsgBoxResult.Yes Then

            '    MDIPrimaryForm.Showmainbutton.Enabled = True

            '    If bgworkImport.IsBusy Then
            '        bgworkImport.CancelAsync()
            '    End If

            '    If bgworkInsertImport.IsBusy Then
            '        bgworkInsertImport.CancelAsync()
            '    End If

            '    InfoBalloon(, , lblforballoon, , , 1)

            '    If previousForm IsNot Nothing Then
            '        If previousForm.Name = Me.Name Then
            '            previousForm = Nothing
            '        End If
            '    End If

            '    showAuditTrail.Close()
            '    SelectFromEmployee.Close()

            '    TimeAttendForm.listTimeAttendForm.Remove(Me.Name)

            '    e.Cancel = False

            'Else

            '    e.Cancel = True

            'End If

        Else

            InfoBalloon(, , lblforballoon, , , 1)

            If previousForm IsNot Nothing Then
                If previousForm.Name = Me.Name Then
                    previousForm = Nothing
                End If
            End If

            showAuditTrail.Close()

            SelectFromEmployee.Close()

            TimeAttendForm.listTimeAttendForm.Remove(Me.Name)

        End If

    End Sub

    Dim newRowID

    Dim dontUpdate As SByte = 0

    Private Sub tsbtnSave_Click(sender As Object, e As EventArgs) Handles tsbtnSave.Click

        dgvetentdet.EndEdit(True)

        If dontUpdate = 1 Then
            listofEditRow.Clear()
        End If

        If haserrinput = 1 Then
            WarnBalloon("Please input a valid date or time.", "Invalid Date or Time", lblforballoon, 0, -69)
            Exit Sub
        ElseIf dgvetentdet.RowCount = 1 Then
            Exit Sub
        End If

        Dim currtimestamp = Nothing

        If dgvetentd.RowCount <> 0 Then

            currtimestamp = Format(CDate(dgvetentd.CurrentRow.Cells("createdmilit").Value), "yyyy-MM-dd HH:mm:ss")

        Else

            currtimestamp = Format(CDate(EXECQUER("SELECT CURRENT_TIMESTAMP();")), "yyyy-MM-dd HH:mm:ss")

        End If
        ', _
        '                                    currtimestamp
        For Each dgrow As DataGridViewRow In dgvetentdet.Rows
            With dgrow
                If .IsNewRow = False Then
                    Dim RowID = Nothing

                    Dim time_i = If(.Cells("Column3").Value = Nothing, _
                                    Nothing, _
                                    Format(CDate(Trim(.Cells("Column3").Value)), "HH:mm:ss"))

                    Dim time_o = If(.Cells("Column4").Value = Nothing, _
                                    Nothing, _
                                    Format(CDate(Trim(.Cells("Column4").Value)), "HH:mm:ss"))

                    If listofEditRow.Contains(.Cells("Column1").Value) Then
                        Dim etent_date = Format(CDate(.Cells("Column5").Value), "yyyy-MM-dd")

                        RowID = .Cells("Column1").Value
                        INSUPD_employeetimeentrydetails(RowID, _
                                                        .Cells("Column2").Value, _
                                                        time_i, _
                                                        time_o, _
                                                        Trim(etent_date), _
                                                        .Cells("Column6").Value)
                    Else
                        If .Cells("Column1").Value = Nothing Then
                            newRowID = _
                            INSUPD_employeetimeentrydetails(, _
                                                            .Cells("Column2").Value, _
                                                            time_i, _
                                                            time_o, _
                                                            Format(CDate(.Cells("Column5").Value), "yyyy-MM-dd"), _
                                                            .Cells("Column6").Value, _
                                                            currtimestamp)

                            .Cells("Column1").Value = newRowID
                        End If

                    End If

                    'Dim afsfa = CDate("").DayOfWeek

                    'If .Cells("Column1").Value = Nothing Then
                    '    .Cells("Column1").Value = newRowID
                    'End If

                End If
            End With

        Next

        listofEditRow.Clear()

        reset_static = -1

        InfoBalloon("Successfully saved.", _
                  "Successfully saved.", lblforballoon, 0, -69)

        tsbtnCancel_Click(sender, e)

    End Sub

    Private Sub dgvetentd_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvetentd.CellContentClick

    End Sub

    Public originalTimeEntryCount As Integer = Nothing

    Private Sub dgvetentd_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvetentd.SelectionChanged
        haserrinput = 0
        dgvetentdet.ShowCellErrors = False
        listofEditRow.Clear()

        originalTimeEntryCount = 0

        With dgvetentd
            If .RowCount <> 0 Then
                'If backgroundworking = 1 Then
                If backgroundworking = 0 Then 'ToolStripProgressBar1.Visible = False 

                    VIEWemployeetimeentrydetails(.CurrentRow.Cells("createdmilit").Value,
                                                 TextBox1.Text.Trim)

                    originalTimeEntryCount = dgvetentdet.RowCount - 1

                End If

            Else
                originalTimeEntryCount = 0

                tsbtndel.Enabled = False
                dgvetentdet.Rows.Clear()
            End If
        End With
    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        TabControlColor(TabControl1, e)
    End Sub

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click

        Dim r_indx, c_indx

        If dgvetentdet.RowCount <> 1 Then

            r_indx = dgvetentdet.CurrentRow.Index
            c_indx = dgvetentdet.CurrentCell.ColumnIndex

            dgvetentd_SelectionChanged(sender, e)

            If (dgvetentdet.RowCount - 2) >= r_indx Then
                dgvetentdet.Item(c_indx, r_indx).Selected = True
            End If
        Else
            dgvetentd_SelectionChanged(sender, e)
        End If

    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        'MsgBox(DateDiff(DateInterval.Minute, CDate("18:02:00"), CDate("18:00:00")))

        If Button3.Image.Tag = 1 Then
            Button3.Image = Nothing
            Button3.Image = My.Resources.r_arrow
            Button3.Image.Tag = 0

            TabControl1.Show()
            dgvetentd.Width = 350

            dgvetentd_SelectionChanged(sender, e)
        Else
            Button3.Image = Nothing
            Button3.Image = My.Resources.l_arrow
            Button3.Image.Tag = 1

            TabControl1.Hide()
            Dim pointX As Integer = Width_resolution - (Width_resolution * 0.15)

            dgvetentd.Width = pointX
        End If

        'Dim inputval = InputBox("Please input time", "")

        'MsgBox(MilitTime(inputval.ToString).ToString)

    End Sub

    Private Sub dgvetentd_GotFocus(sender As Object, e As EventArgs) Handles dgvetentd.GotFocus

        If dgvetentd.RowCount <> 0 Then

            If backgroundworking = 0 Then

                tsbtndel.Enabled = True

            End If

        Else

            tsbtndel.Enabled = False

        End If

    End Sub

    Private Sub dgvetentd_LostFocus(sender As Object, e As EventArgs) Handles dgvetentd.LostFocus

        tsbtndel.Enabled = False
    End Sub

    Private Sub tsbtndel_Click(sender As Object, e As EventArgs) Handles tsbtndel.Click
        With dgvetentd
            If .RowCount <> 0 Then

                Dim result = MessageBox.Show("Are you sure you want to delete ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

                If result = DialogResult.Yes Then

                    EXECQUER("DELETE FROM employeetimeentrydetails WHERE Created='" & .CurrentRow.Cells("createdmilit").Value & "';" & _
                             "ALTER TABLE employeetimeentrydetails AUTO_INCREMENT = 0;")

                    dgvetentd.Rows.Remove(.CurrentRow)

                End If

            End If

        End With

    End Sub

    Private Sub dgvetentdet_RowMinimumHeightChanged(sender As Object, e As DataGridViewRowEventArgs) Handles dgvetentdet.RowMinimumHeightChanged

    End Sub

    Private Sub dgvetentdet_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvetentdet.Scroll

        myEllipseButton(dgvetentdet, _
                        "Column2", _
                        btnEmpID)

    End Sub

    Private Sub dgvetentdet_SelectionChanged(sender As Object, e As EventArgs) Handles dgvetentdet.SelectionChanged
        If dgvetentdet.RowCount = 1 Then

        Else
            With dgvetentdet.CurrentRow
                .Cells("Column2").ReadOnly = True

                If .IsNewRow Then
                    .Cells("Column2").ReadOnly = False
                Else
                    myEllipseButton(dgvetentdet, "Column2", btnEmpID)

                End If

            End With

        End If

        myEllipseButton(dgvetentdet, _
                        "Column2", _
                        btnEmpID)

    End Sub

    Private Sub btnEmpID_Click(sender As Object, e As EventArgs) Handles btnEmpID.Click

        With SelectFromEmployee

            .Show()

            .BringToFront()

        End With

    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click

        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        RemoveHandler dgvetentd.SelectionChanged, AddressOf dgvetentd_SelectionChanged

        Dim selrowindx As Integer = Nothing
        If dgvetentd.RowCount <> 0 Then
            selrowindx = dgvetentd.CurrentRow.Index
        End If

        loademployeetimeentrydetails()

        If dgvetentd.RowCount <> 0 Then
            If selrowindx < dgvetentd.RowCount Then
                dgvetentd.Item("Createds", selrowindx).Selected = True
            End If
        End If

        dgvetentd_SelectionChanged(sender, e)

        AddHandler dgvetentd.SelectionChanged, AddressOf dgvetentd_SelectionChanged

    End Sub

    Private Sub ToolStripProgressBar1_Click(sender As Object, e As EventArgs) Handles ToolStripProgressBar1.Click

    End Sub

    Private Sub ToolStripProgressBar1_VisibleChanged(sender As Object, e As EventArgs) Handles ToolStripProgressBar1.VisibleChanged

        Dim boolVisib = Not ToolStripProgressBar1.Visible


        tsbtnSave.Enabled = boolVisib

        tsbtndel.Enabled = boolVisib

        tsbtnCancel.Enabled = boolVisib

        MDIPrimaryForm.Showmainbutton.Enabled = boolVisib

        TimeAttendForm.MenuStrip1.Enabled = boolVisib

        Button4.Enabled = boolVisib

        tsbtnAudittrail.Enabled = boolVisib

        First.Enabled = boolVisib

        Prev.Enabled = boolVisib

        Nxt.Enabled = boolVisib

        Last.Enabled = boolVisib

        btnEmpID.Enabled = boolVisib


    End Sub

    Private Sub ContextMenuStrip2_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip2.Opening

        'DeleteRowToolStripMenuItem

        If dgvetentdet.RowCount = 1 Then

            DeleteRowToolStripMenuItem.Enabled = False

        ElseIf dgvetentdet.CurrentRow.IsNewRow Then

            DeleteRowToolStripMenuItem.Enabled = False

        Else
            DeleteRowToolStripMenuItem.Enabled = True

        End If

    End Sub

    Private Sub DeleteRowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteRowToolStripMenuItem.Click

        Dim result = MessageBox.Show("Are you sure you want to delete this item ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

        If result = DialogResult.Yes Then

            With dgvetentdet

                .Focus()

                .EndEdit(True)

                EXECQUER("DELETE FROM employeetimeentrydetails WHERE RowID='" & .CurrentRow.Cells("Column1").Value & "';" & _
                          "ALTER TABLE employeetimeentrydetails AUTO_INCREMENT = 0;")

                .Rows.Remove(.CurrentRow)

            End With

        End If

    End Sub

    Private Sub Search_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox7.KeyPress, TextBox1.KeyPress,
                                                                                    ComboBox8.KeyPress, TextBox15.KeyPress,
                                                                                    ComboBox9.KeyPress, TextBox16.KeyPress,
                                                                                    ComboBox10.KeyPress, TextBox17.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then
            Button4_Click(sender, e)

        End If

    End Sub

    Private Sub cboxsearchpayperiod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboxsearchpayperiod.SelectedIndexChanged

    End Sub

    Private Sub cboxsearchmonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboxsearchmonth.SelectedIndexChanged

        If cboxsearchmonth.SelectedIndex > -1 Then

            Dim month_index = cboxsearchmonth.SelectedIndex + 1

            Dim n_SQLQueryToDatatable As _
                New SQLQueryToDatatable("SELECT * " &
                                        " FROM payperiod" &
                                        " WHERE OrganizationID=''" &
                                        " AND TotalGrossSalary=1" &
                                        " AND `Month`='" & month_index & "'" &
                                        " AND `Year`='';")

        End If

    End Sub

    Private Sub tsbtnExportReportTimeLogs_Click(sender As Object, e As EventArgs) Handles tsbtnExportReportTimeLogs.Click

    End Sub

End Class