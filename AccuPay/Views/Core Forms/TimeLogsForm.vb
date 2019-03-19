Imports System.IO
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Helper.TimeLogsReader
Imports AccuPay.Utils
Imports log4net
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports OfficeOpenXml


Public Class TimeLogsForm

    Public Enum TimeLogsFormat
        Optimized = 0
        Conventional = 1
    End Enum

    Private _logger As ILog = LogManager.GetLogger("TimeLogsLogger")

    Dim dattabLogs As New DataTable

    Dim dtTimeLogs As New DataTable

    Dim dtImport As New DataTable

    Dim thefilepath As String

    Dim view_ID As Object

    Private sys_ownr As New SystemOwner

    Private str_query_insupd_timeentrylogs As String =
        "SELECT INSUPD_timeentrylogs(?og_id, ?emp_unique_key, ?timestamp_log, ?max_importid);"

    'do this habang di pa 100% stable ang new import
    Private _showNewImport As Boolean

    Private _useShiftSchedulePolicy As Boolean

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _useShiftSchedulePolicy = GetShiftSchedulePolicy()

        _showNewImport = True

        If _showNewImport Then
            tsbtnNew.Visible = 0
            tsbtnNewExperimental.Visible = 1
        Else
            tsbtnNew.Visible = 1
            tsbtnNewExperimental.Visible = 0
        End If

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

            If _showNewImport Then
                tsbtnNewExperimental.Visible = 0
            Else
                tsbtnNew.Visible = 0
            End If

            tsbtnSave.Visible = 0
            tsbtndel.Visible = 0

            dontUpdate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    If _showNewImport Then
                        tsbtnNewExperimental.Visible = 0
                    Else
                        tsbtnNew.Visible = 0
                    End If

                    tsbtnSave.Visible = 0
                    tsbtndel.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then


                        If _showNewImport Then
                            tsbtnNewExperimental.Visible = 0
                        Else
                            tsbtnNew.Visible = 0
                        End If
                    Else

                        If _showNewImport Then
                            tsbtnNewExperimental.Visible = 1
                        Else
                            tsbtnNew.Visible = 1
                        End If
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

    Private Function GetShiftSchedulePolicy() As Boolean
        Using context = New PayrollContext()

            Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

            Dim policy = New TimeEntryPolicy(settings)

            Return policy.UseShiftSchedule
        End Using
    End Function

    Sub loademployeetimeentrydetails(Optional pagination As Integer = 0)
        Static once As Integer = -1

        dgvetentd.Rows.Clear()
        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL `VIEW_timeentrydetails`('" & orgztnID & "','" & pagination & "');")
        Dim catchdt As New DataTable
        catchdt = n_SQLQueryToDatatable.ResultTable
        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvetentd.Rows.Add(row_array)
        Next

        catchdt.Dispose()
    End Sub

    Sub VIEWemployeetimeentrydetails(ByVal timeentrylogsImportID As Object,
                                     EmployeeNumber As String,
                                     firstName As String,
                                     lastName As String)

        Dim param(4, 4) As Object

        param(0, 0) = "etentd_TimeentrylogsImportID"
        param(1, 0) = "etentd_OrganizationID"
        param(2, 0) = "etd_EmployeeNumber"
        param(3, 0) = "e_FirstName"
        param(4, 0) = "e_LastName"

        param(0, 1) = timeentrylogsImportID
        param(1, 1) = orgztnID
        param(2, 1) = EmployeeNumber
        param(3, 1) = firstName
        param(4, 1) = lastName

        EXEC_VIEW_PROCEDURE(param,
                           "VIEW_employeetimeentrydetails",
                           dgvetentdet)
    End Sub

    Private Sub tsbtnNew_Click(sender As Object, e As EventArgs) _
        Handles tsbtnNew.Click, tsbtnNewExperimental.Click

        Static employeeleaveRowID As Integer = -1

        Dim timeLogsFormat_ As TimeLogsFormat? = TimeLogsImportOption()

        'They chose Cancel or used the close button
        If timeLogsFormat_ Is Nothing Then Return

        Try
            Dim browsefile As OpenFileDialog = New OpenFileDialog()
            browsefile.Filter = "Text Documents (*.txt)|*.txt" &
                                "|All files (*.*)|*.*"

            If browsefile.ShowDialog = Windows.Forms.DialogResult.OK Then

                thefilepath = browsefile.FileName
                Dim balloon_x = lblforballoon.Location.X

                lblforballoon.Location = New Point(TabControl1.Location.X, lblforballoon.Location.Y)

                InfoBalloon("Please wait a few moments.",
                          "Importing file...", lblforballoon, 0, -69)

                lblforballoon.Location = New Point(balloon_x, lblforballoon.Location.Y)

                If timeLogsFormat_ = TimeLogsFormat.Conventional Then

                    If sender Is tsbtnNewExperimental Then
                        NewTimeEntryAlternateLineImport()

                    Else
                        HouseKeepingBeforeStartAlternateLineBackgroundWork()
                        bgworkTypicalImport.RunWorkerAsync(sender)
                    End If

                Else
                    HouseKeepingBeforeStartAlternateLineBackgroundWork()
                    bgworkImport.RunWorkerAsync()
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message & " Error on file initialization")
        Finally
        End Try
    End Sub

    Private Async Sub NewTimeEntryAlternateLineImport()
        Dim importer = New TimeLogsReader()
        Dim importOutput = importer.Import(thefilepath)

        If importOutput.IsImportSuccess = False Then
            MessageBox.Show(importOutput.ErrorMessage)
            Return
        End If

        Dim logs = importOutput.Logs

        If logs.Count = 0 Then
            MessageBox.Show("No logs were parsed. Please make sure the log files follows the right format.")
            Return
        End If

        Dim timeAttendanceHelper As ITimeAttendanceHelper = Await GetTimeAttendanceHelper(logs)

        'determines the IstimeIn, LogDate, and Employee values
        logs = timeAttendanceHelper.Analyze()
        Dim validLogs = logs.Where(Function(l) l.HasError = False).ToList()
        Dim invalidLogs = logs.Where(Function(l) l.HasError = True).ToList()

        invalidLogs.AddRange(importOutput.Errors)


        'preview the logs here
        Dim previewDialog As New _
            TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog(validLogs, invalidLogs)

        With previewDialog
            .ShowDialog()
            .BringToFront()
        End With

        If previewDialog.Cancelled Then
            Return
        End If

        HouseKeepingBeforeStartAlternateLineBackgroundWork()
        bgworkTypicalImport.RunWorkerAsync(timeAttendanceHelper)
    End Sub

    Private Async Function GetTimeAttendanceHelper(logs As IList(Of ImportTimeAttendanceLog)) _
                            As Threading.Tasks.Task(Of ITimeAttendanceHelper)

        Dim logsGroupedByEmployee = ImportTimeAttendanceLog.GroupByEmployee(logs)
        Dim employees As List(Of Employee) = Await GetEmployeesFromLogGroup(logsGroupedByEmployee)

        Dim firstDate = logs.FirstOrDefault.DateTime.ToMinimumHourValue
        Dim lastDate = logs.LastOrDefault.DateTime.ToMaximumHourValue

        Dim timeAttendanceHelper As ITimeAttendanceHelper

        If _useShiftSchedulePolicy Then

            Dim employeeShifts As List(Of EmployeeDutySchedule) =
                    Await GetEmployeeDutyShifts(firstDate, lastDate)

            timeAttendanceHelper = New TimeAttendanceHelperNew(logs, employees, employeeShifts)

        Else

            Dim employeeShifts As List(Of ShiftSchedule) =
                    Await GetEmployeeShifts(firstDate, lastDate)

            timeAttendanceHelper = New TimeAttendanceHelper(logs, employees, employeeShifts)

        End If

        Return timeAttendanceHelper
    End Function

    Private Sub HouseKeepingBeforeStartAlternateLineBackgroundWork()
        tsbtnNew.Enabled = False
        tsbtnNewExperimental.Enabled = False

        RemoveHandler dgvetentd.SelectionChanged, AddressOf dgvetentd_SelectionChanged

        Panel1.Enabled = False

        ToolStripProgressBar1.Visible = True
    End Sub

    Private Function TimeLogsImportOption() As TimeLogsFormat?

        Dim time_logformat As TimeLogsFormat?

        MessageBoxManager.Yes = "Alternating line"
        MessageBoxManager.No = "Same line"

        MessageBoxManager.Register()

        Dim custom_prompt =
            MessageBox.Show("Which format are you going to import ?",
                            "", MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1)

        If custom_prompt = Windows.Forms.DialogResult.Yes Then
            time_logformat = TimeLogsFormat.Conventional
        ElseIf custom_prompt = Windows.Forms.DialogResult.No Then
            time_logformat = TimeLogsFormat.Optimized
        ElseIf custom_prompt = Windows.Forms.DialogResult.Cancel Then
            time_logformat = Nothing
        End If

        MessageBoxManager.Unregister()

        Return time_logformat

    End Function

    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Function INSUPD_employeetimeentrydetails(Optional etentd_RowID As Object = Nothing,
                                             Optional etentd_EmployeeID As Object = Nothing,
                                             Optional etentd_TimeIn As Object = Nothing,
                                             Optional etentd_TimeOut As Object = Nothing,
                                             Optional etentd_Date As Object = Nothing,
                                             Optional etentd_TimeScheduleType As Object = Nothing,
                                             Optional etentd_Created As Object = Nothing,
                                             Optional etentd_TimeEntryStatus As Object = Nothing,
                                             Optional EditAsUnique As String = "0",
                                             Optional Branch_Code As Object = Nothing,
                                             Optional DateTimeLogIn As Object = Nothing,
                                             Optional DateTimeLogOut As Object = Nothing,
                                             Optional timeentrylogsImportID As String = "") As Object

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

            machineShortTime = RegKey.GetValue("sTimeFormat").ToString

            machineShortTime = machineShortTime.Replace("t", "").Trim

            machineShortTime = machineShortTime.Replace("h", "H").Trim

        End If

        Dim params(9, 2) As Object

        Dim return_value = Nothing
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("INSUPD_employeetimeentrydetails", conn)
            conn.Open()
            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("etentdID", MySqlDbType.Int32)

                .Parameters.AddWithValue("etentd_RowID", If(etentd_RowID = Nothing, DBNull.Value, etentd_RowID))
                .Parameters.AddWithValue("etentd_OrganizationID", orgztnID)
                .Parameters.AddWithValue("etentd_CreatedBy", z_User)
                .Parameters.AddWithValue("etentd_Created", etentd_Created)
                .Parameters.AddWithValue("etentd_LastUpdBy", z_User)
                .Parameters.AddWithValue("etentd_EmployeeID", If(etentd_EmployeeID = Nothing, DBNull.Value, etentd_EmployeeID))

                .Parameters.AddWithValue("etentd_TimeentrylogsImportID", timeentrylogsImportID)


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

                datread = .ExecuteReader()
                return_value = datread(0)

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

        Dim parser = New TimeInTimeOutParser()
        Dim timeEntries = parser.Parse(thefilepath)

        Dim timeLogsByEmployee = timeEntries.
            GroupBy(Function(t) t.EmployeeNo).
            ToList()

        Using context = New PayrollContext()
            Dim dateCreated = Date.Now

            For Each timelogs In timeLogsByEmployee
                Dim employee = context.Employees.
                    Where(Function(et) et.EmployeeNo = timelogs.Key).
                    Where(Function(et) Nullable.Equals(et.OrganizationID, z_OrganizationID)).
                    FirstOrDefault()

                If employee Is Nothing Then
                    Continue For
                End If

                For Each timeLog In timelogs
                    Dim t = New TimeLog() With {
                        .OrganizationID = z_OrganizationID,
                        .EmployeeID = employee.RowID,
                        .Created = dateCreated,
                        .CreatedBy = z_User,
                        .LogDate = timeLog.DateOccurred
                    }

                    If Not String.IsNullOrWhiteSpace(timeLog.TimeIn) Then
                        t.TimeIn = TimeSpan.Parse(timeLog.TimeIn)
                    End If

                    If Not String.IsNullOrWhiteSpace(timeLog.TimeOut) Then
                        t.TimeOut = TimeSpan.Parse(timeLog.TimeOut)
                    End If

                    context.TimeLogs.Add(t)
                Next
            Next

            context.SaveChanges()

        End Using

        bgworkImport.ReportProgress(100)
        Return

        dtImport.Rows.Clear()
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

            tsbtnNewExperimental.Enabled = True

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")

            tsbtnNew.Enabled = True

            tsbtnNewExperimental.Enabled = True

        Else

            lblforballoon.Location = New Point(TabControl1.Location.X, lblforballoon.Location.Y)

            dgvetentdet.Rows.Clear()

            progress_value = ToolStripProgressBar1.Value

            bgworkInsertImport.RunWorkerAsync()

        End If

        backgroundworking = 0

    End Sub

    Private Sub bgworkInsertImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkInsertImport.DoWork
        backgroundworking = 1

        Dim currtimestamp = Format(CDate(EXECQUER("SELECT CURRENT_TIMESTAMP();")), "yyyy-MM-dd HH:mm:ss")

        Dim lastbound = dtImport.Rows.Count 'dattabLogs

        Dim indx = 1

        For Each drow As DataRow In dtImport.Rows 'dattabLogs

            INSUPD_employeetimeentrydetails(,
                                            drow("EmploID"),
                                            drow("TIn"),
                                            drow("TOut"),
                                            drow("LogDate"),
                                            "",
                                            currtimestamp, , "1",
                                            drow("BranchCode"),
                                            drow("DateTimeLogIn"),
                                            drow("DateTimeLogOut"))

            Dim progressvalue = CInt((indx / lastbound) * 50)

            bgworkInsertImport.ReportProgress(progressvalue)

            indx += 1

        Next

        EXECQUER("UPDATE employeetimeentrydetails SET TimeScheduleType='' WHERE TimeScheduleType IS NULL AND OrganizationID='" & orgztnID & "';")
    End Sub

    Private Sub bgworkInsertImport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkInsertImport.ProgressChanged
        ToolStripProgressBar1.Value = progress_value + CType(e.ProgressPercentage, Integer)
    End Sub

    Private Sub bgworkInsertImport_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) _
        Handles bgworkInsertImport.RunWorkerCompleted,
                bgworkTypicalImport.RunWorkerCompleted

        Dim balloon_x = lblforballoon.Location.X

        If e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message)

            tsbtnNew.Enabled = True

            tsbtnNewExperimental.Enabled = True

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")

            tsbtnNew.Enabled = True

            tsbtnNewExperimental.Enabled = True
        Else

            loademployeetimeentrydetails(0)

            InfoBalloon(, , lblforballoon, , , 1)

            InfoBalloon(IO.Path.GetFileName(thefilepath) & " imported successfully.",
                      "Importing file finished", lblforballoon, 0, -69)

        End If

        ToolStripProgressBar1.Visible = False

        ToolStripProgressBar1.Value = 0

        progress_value = 0

        tsbtnNew.Enabled = True

        tsbtnNewExperimental.Enabled = True

        Panel1.Enabled = True

        backgroundworking = 0

        dgvetentd_SelectionChanged(sender, e)

        AddHandler dgvetentd.SelectionChanged, AddressOf dgvetentd_SelectionChanged

        lblforballoon.Location = New Point(balloon_x, lblforballoon.Location.Y)

        'Refresh Gridview
        Button4.PerformClick()
    End Sub

    Dim haserrinput As SByte

    Dim listofEditRow As New AutoCompleteStringCollection

    Dim reset_static As SByte = -1

    Private Sub dgvetentdet_CellEndEdit1(sender As Object, e As DataGridViewCellEventArgs) 'Handles dgvetentdet.CellEndEdit
        dgvetentdet.ShowCellErrors = True
        Dim colName As String = dgvetentdet.Columns(e.ColumnIndex).Name
        Dim rowindx = e.RowIndex

        Static num As Integer = If(reset_static = -1,
                                   -1,
                                   num)

        If dgvetentdet.RowCount <> 0 Then
            With dgvetentdet

                If Val(dgvetentdet.Item("Column1", e.RowIndex).Value) <> 0 Then
                    listofEditRow.Add(dgvetentdet.Item("Column1", e.RowIndex).Value)
                End If

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
                            If dateobj.ToString.Contains("A") Or
                                dateobj.ToString.Contains("P") Or
                                dateobj.ToString.Contains("M") Then

                                ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))

                                dateobj_len -= ampm.Length

                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, dateobj_len)) 'dateobj.ToString.Substring(0, 4)
                                dateobj = dateobj.ToString.Replace(" ", ":")

                                dateobj &= ampm

                            End If

                            Dim modified_format = If(dateobj_len = 5, "h:mm tt", "hh:mm:ss tt")

                            Dim valtime As DateTime = DateTime.Parse(dateobj).ToString(modified_format)
                            If ampm = Nothing Then
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToLongTimeString
                            Else
                                dgvetentdet.Item(colName, rowindx).Value = Trim(valtime.ToLongTimeString.Substring(0, (dateobj_len - 1))) & ampm
                            End If

                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            Try
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, dateobj_len))
                                dateobj = dateobj.ToString.Replace(" ", ":")

                                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm:ss")
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToLongTimeString
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
                End If

            End With
        End If
    End Sub

    Private Sub dgvetentdet_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvetentdet.CellEndEdit

        dgvetentdet.ShowCellErrors = True

        Dim colName As String = dgvetentdet.Columns(e.ColumnIndex).Name
        Dim rowindx = e.RowIndex

        Static num As Integer = If(reset_static = -1,
                                   -1,
                                   num)

        If dgvetentdet.RowCount <> 0 Then
            With dgvetentdet

                If Val(dgvetentdet.Item("Column1", e.RowIndex).Value) <> 0 Then
                    listofEditRow.Add(dgvetentdet.Item("Column1", e.RowIndex).Value)
                End If

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
                            If dateobj.ToString.Contains("A") Or
                        dateobj.ToString.Contains("P") Or
                        dateobj.ToString.Contains("M") Then

                                ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                                ampm = Microsoft.VisualBasic.Left(ampm, 2) & "M"
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                                dateobj = dateobj.ToString.Replace(" ", ":")

                            End If

                            Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")
                            If ampm = Nothing Then
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToShortTimeString
                            Else
                                dgvetentdet.Item(colName, rowindx).Value = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                            End If

                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            Try
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, 5))
                                dateobj = dateobj.ToString.Replace(" ", ":")

                                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToShortTimeString
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
                End If

            End With
        End If
    End Sub

    Dim pagination As Integer

    Private Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked,
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

            End If

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
        Dim currentImportId = ""

        If dgvetentd.RowCount <> 0 Then
            currtimestamp = Format(CDate(dgvetentd.CurrentRow.Cells("createdmilit").Value), "yyyy-MM-dd HH:mm:ss")
            currentImportId = dgvetentd.CurrentRow.Cells("TimeentrylogsImportID").Value
        Else
            currtimestamp = Format(CDate(EXECQUER("SELECT CURRENT_TIMESTAMP();")), "yyyy-MM-dd HH:mm:ss")
        End If

        For Each dgrow As DataGridViewRow In dgvetentdet.Rows
            With dgrow
                If .IsNewRow = False Then
                    Dim RowID = Nothing

                    Dim time_i = If(.Cells("Column3").Value = Nothing,
                                    Nothing,
                                    Format(CDate(Trim(.Cells("Column3").Value)), "HH:mm:ss"))

                    Dim time_o = If(.Cells("Column4").Value = Nothing,
                                    Nothing,
                                    Format(CDate(Trim(.Cells("Column4").Value)), "HH:mm:ss"))

                    If listofEditRow.Contains(.Cells("Column1").Value) Then
                        Dim etent_date = Format(CDate(.Cells("Column5").Value), "yyyy-MM-dd")

                        Dim dateout = Nothing

                        If IsDBNull(.Cells("Column13").Value) Then

                            dateout = Nothing
                        Else
                            If IsDBNull(.Cells("Column11")) Then
                                dateout = Nothing
                            Else

                                dateout = Format(CDate(.Cells("Column13").Value), "yyyy-MM-dd")

                            End If

                        End If

                        Dim timeout = Format(CDate(Trim(.Cells("Column4").Value)), "HH:mm:ss")
                        Dim timestampinout = dateout & " " & timeout

                        Dim datein = Format(CDate(.Cells("Column5").Value), "yyyy-MM-dd")
                        Dim timein = Format(CDate(Trim(.Cells("Column3").Value)), "HH:mm:ss")
                        Dim timestampin = datein & " " & timein

                        RowID = .Cells("Column1").Value
                        INSUPD_employeetimeentrydetails(RowID,
                                                    .Cells("Column2").Value,
                                                    time_i,
                                                    time_o,
                                                    Trim(etent_date),
                                                    .Cells("Column6").Value,,,,, timestampin, timestampinout,
                                                            timeentrylogsImportID:=currentImportId)
                    Else
                        If .Cells("Column1").Value = Nothing Then
                            Dim newRowID =
                            INSUPD_employeetimeentrydetails(,
                                                            .Cells("Column2").Value,
                                                            time_i,
                                                            time_o,
                                                            Format(CDate(.Cells("Column5").Value), "yyyy-MM-dd"),
                                                            .Cells("Column6").Value,
                                                            currtimestamp,
                                                            timeentrylogsImportID:=currentImportId)

                            .Cells("Column1").Value = newRowID
                        End If

                    End If

                End If
            End With

        Next

        listofEditRow.Clear()

        reset_static = -1

        InfoBalloon("Successfully saved.",
                  "Successfully saved.", lblforballoon, 0, -69)

        tsbtnCancel_Click(sender, e)
    End Sub

    Public originalTimeEntryCount As Integer = Nothing

    Private Sub dgvetentd_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvetentd.SelectionChanged
        haserrinput = 0
        dgvetentdet.ShowCellErrors = False
        listofEditRow.Clear()

        originalTimeEntryCount = 0

        With dgvetentd
            If .RowCount <> 0 Then
                If backgroundworking = 0 Then
                    VIEWemployeetimeentrydetails(.CurrentRow.Cells("TimeentrylogsImportID").Value,
                                                 TextBox1.Text.Trim,
                                                 txtFirstName.Text.Trim,
                                                 txtLastName.Text.Trim)

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

    Private Async Sub tsbtndel_Click(sender As Object, e As EventArgs) Handles tsbtndel.Click

        If dgvetentd.RowCount = 0 Then Return

        Dim importId = dgvetentd.CurrentRow.Cells("TimeentrylogsImportID").Value

        If importId Is Nothing Then Return

        Dim result = MessageBox.Show($"Are you sure you want to delete import: {importId}?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

        If result = DialogResult.Yes Then

            Using context As New PayrollContext

                context.TimeAttendanceLogs.
                    RemoveRange(context.TimeAttendanceLogs.
                                    Where(Function(t) t.ImportNumber = importId))

                context.TimeLogs.
                    RemoveRange(context.TimeLogs.
                                    Where(Function(t) t.TimeentrylogsImportID = importId))

                Await context.SaveChangesAsync()

                dgvetentd.Rows.Remove(dgvetentd.CurrentRow)

            End Using

        End If
    End Sub

    Private Sub dgvetentdet_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvetentdet.Scroll
        myEllipseButton(dgvetentdet,
                        "Column2",
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

        myEllipseButton(dgvetentdet,
                        "Column2",
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

                EXECQUER("DELETE FROM employeetimeentrydetails WHERE RowID='" & .CurrentRow.Cells("Column1").Value & "';")
                .Rows.Remove(.CurrentRow)
            End With
        End If
    End Sub

    Private Sub Search_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox7.KeyPress, TextBox1.KeyPress,
                                                                                    ComboBox8.KeyPress, txtFirstName.KeyPress,
                                                                                    ComboBox9.KeyPress, txtLastName.KeyPress,
                                                                                    ComboBox10.KeyPress, TextBox17.KeyPress
        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then
            Button4_Click(sender, e)
        End If
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
        Dim saveFileDialog = New SaveFileDialog()
        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx"

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            Dim fileName = saveFileDialog.FileName
            Dim file = New FileInfo(fileName)

            Using excelPackage = New ExcelPackage(file)
                Dim worksheet = excelPackage.Workbook.Worksheets.Add("Time logs")
                worksheet.Column(5).Style.Numberformat.Format = "mm/dd/yyyy"
                worksheet.Column(7).Style.Numberformat.Format = "mm/dd/yyyy"

                worksheet.Cells("A1").Value = "Employee ID"
                worksheet.Cells("B1").Value = "Name"
                worksheet.Cells("C1").Value = "Shift"
                worksheet.Cells("D1").Value = "Time In"
                worksheet.Cells("E1").Value = "Date In"
                worksheet.Cells("F1").Value = "Time Out"
                worksheet.Cells("G1").Value = "Date Out"
                worksheet.Cells("H1").Value = "Schedule Type"

                Dim i = 2
                For Each row As DataGridViewRow In dgvetentdet.Rows
                    worksheet.Cells($"A{i}").Value = row.Cells(2).Value
                    worksheet.Cells($"B{i}").Value = row.Cells(3).Value
                    worksheet.Cells($"C{i}").Value = row.Cells(4).Value
                    worksheet.Cells($"D{i}").Value = row.Cells(5).Value
                    worksheet.Cells($"E{i}").Value = row.Cells(6).Value
                    worksheet.Cells($"F{i}").Value = row.Cells(7).Value
                    worksheet.Cells($"G{i}").Value = row.Cells(8).Value
                    worksheet.Cells($"H{i}").Value = row.Cells(9).Value

                    i += 1
                Next

                excelPackage.Save()
                MsgBox("Time logs has been exported.", MsgBoxStyle.OkOnly, "Exported time logs")
            End Using
        End If
    End Sub

    Private Function ImportConventionalFormatTimeLogs() As Integer
        Dim return_value As Integer = 0

        Dim max_importid = New SQL(String.Concat("SELECT MAX(ImportID) FROM timeentrylogs WHERE OrganizationID=", orgztnID, ";")).GetFoundRow
        max_importid = (Convert.ToDouble(max_importid) + 1)

        Dim emp_unique_key, datetime_attended As Object

        Dim parser = New TimeInTimeOutParser()
        Dim timeEntries = parser.ParseConventionalTimeLogs(thefilepath)

        Dim i = 1

        Dim line_content_count As Integer = timeEntries.Count

        For Each timeEntry In timeEntries

            emp_unique_key =
                timeEntry.EmployeUniqueKey

            datetime_attended =
                timeEntry.DateAndTime

            Dim param_values =
                New Object() {orgztnID,
                              emp_unique_key,
                              Convert.ToString(datetime_attended),
                              max_importid}

            Dim sql As New SQL(str_query_insupd_timeentrylogs,
                               param_values)
            sql.ExecuteQuery()

            If sql.HasError Then
                MsgBox(sql.ErrorMessage)
            End If

            return_value = ((i / line_content_count) * 100)

            bgworkTypicalImport.
                ReportProgress(return_value)

            i += 1
        Next

        Return max_importid
    End Function

    Private Sub bgworkTypicalImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkTypicalImport.DoWork
        If e.Argument Is tsbtnNew Then
            OldTimeEntryAlternateLineImport()

        Else
            Dim args = CType(e.Argument, ITimeAttendanceHelper)

            NewTimeEntryAlternateLineImportSave(args)

        End If
    End Sub

    Private Async Sub NewTimeEntryAlternateLineImportSave(timeAttendanceHelper As ITimeAttendanceHelper)

        Try
            Dim timeLogs = timeAttendanceHelper.GenerateTimeLogs()
            Dim timeAttendanceLogs = timeAttendanceHelper.GenerateTimeAttendanceLogs()

            Using context = New PayrollContext()

                Dim importId = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
                Dim originalImportId = importId

                Dim counter As Integer = 0

                While context.TimeLogs.FirstOrDefault(Function(t) t.TimeentrylogsImportID = importId) IsNot Nothing OrElse
                        context.TimeAttendanceLogs.FirstOrDefault(Function(t) t.ImportNumber = importId) IsNot Nothing
                    counter += 1

                    importId = originalImportId & "_" & counter

                End While

                For Each timeLog In timeLogs

                    timeLog.TimeentrylogsImportID = importId

                    context.TimeLogs.Add(timeLog)
                Next

                For Each timeAttendanceLog In timeAttendanceLogs

                    timeAttendanceLog.ImportNumber = importId

                    context.TimeAttendanceLogs.Add(timeAttendanceLog)
                Next

                Await context.SaveChangesAsync()

            End Using


        Catch ex As Exception

            _logger.Error("NewTimeEntryAlternateLineImport", ex)

            MessageBoxHelper.DefaultErrorMessage("Import Logs")

            Throw ex

        End Try

    End Sub

    Private Async Function GetEmployeeShifts(firstDate As Date, lastDate As Date) As Threading.Tasks.Task(Of List(Of ShiftSchedule))

        Using context = New PayrollContext()
            Return Await context.ShiftSchedules.
                           Include(Function(s) s.Shift).
                           Where(Function(s) s.OrganizationID = z_OrganizationID).
                           Where(Function(s) s.EffectiveFrom >= firstDate).
                           Where(Function(s) s.EffectiveTo <= lastDate).
                           ToListAsync()
        End Using

    End Function

    'new shift table
    Private Async Function GetEmployeeDutyShifts(firstDate As Date, lastDate As Date) As Threading.Tasks.Task(Of List(Of EmployeeDutySchedule))

        Using context = New PayrollContext()
            Return Await context.EmployeeDutySchedules.
                           Where(Function(s) s.OrganizationID = z_OrganizationID).
                           Where(Function(s) s.DateSched >= firstDate).
                           Where(Function(s) s.DateSched <= lastDate).
                           ToListAsync()
        End Using

    End Function

    Private Async Function GetEmployeesFromLogGroup(logsGroupedByEmployee As List(Of IGrouping(Of String, ImportTimeAttendanceLog))) As Threading.Tasks.Task(Of List(Of Employee))

        Using context As New PayrollContext
            If logsGroupedByEmployee.Count < 1 Then
                Return New List(Of Employee)
            End If

            Dim employeeNumbersArray(logsGroupedByEmployee.Count - 1) As String

            For index = 0 To logsGroupedByEmployee.Count - 1
                employeeNumbersArray(index) = logsGroupedByEmployee(index).Key
            Next

            Return Await context.Employees.
                            Where(Function(e) employeeNumbersArray.Contains(e.EmployeeNo)).
                            Where(Function(e) Nullable.Equals(e.OrganizationID, z_OrganizationID)).
                            ToListAsync
        End Using

    End Function

    Private Sub OldTimeEntryAlternateLineImport()
        Dim import_id = ImportConventionalFormatTimeLogs()

        Dim param_values =
            New Object() {orgztnID,
                         z_User,
                          DBNull.Value,
                          DBNull.Value,
                          import_id}

        Dim sql As New SQL("CALL BULK_INSUPD_employeetimeentrydetails(?og_id, ?user_id, ?from_date, ?to_date, ?id_import);",
                           param_values)
        sql.ExecuteQuery()

        Try
            If sql.HasError Then
                Throw sql.ErrorException
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Name))
        End Try
    End Sub

    Private Sub bgworkTypicalImport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkTypicalImport.ProgressChanged
        ToolStripProgressBar1.Value = e.ProgressPercentage
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        Static _bool As Boolean =
            (sys_ownr.CurrentSystemOwner <> SystemOwner.Cinema2000)

        tsbtnExportReportTimeLogs.Visible = _bool

        MyBase.OnLoad(e)
    End Sub

    Private Class NewTimeLogsArgument
        Public Property Logs As IList(Of ImportTimeAttendanceLog)
        Public Property IsChangeable As Boolean

        Sub New(logs As IList(Of ImportTimeAttendanceLog), isChangeable As Boolean)
            Me.Logs = logs
            Me.IsChangeable = isChangeable
        End Sub
    End Class
End Class
