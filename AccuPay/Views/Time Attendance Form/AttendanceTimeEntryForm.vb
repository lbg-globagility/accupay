Public Class AttendanceTimeEntryForm
    Dim datefrom, dateto As DateTime
    Dim PathReg As String = My.Application.Info.DirectoryPath

    Public zAttens_DT As New System.Data.DataTable
    Public StrForExport As New ArrayList

    Dim zOpenExelFile As New System.Diagnostics.Process

    Dim S_BInder As New BindingSource

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        If dtDateTo.Value < dtFromDate.Value Then
            MsgBox("To Date should be greater than From Date", MsgBoxStyle.Exclamation, "Not Allowed")
            Exit Sub
        End If

        datefrom = dtFromDate.Value
        dateto = dtDateTo.Value

        Dim Osdcard As New OpenFileDialog
        Osdcard.Filter = "USB File - NU2088|*.txt"
        Osdcard.ShowDialog()
        If Osdcard.FileName = Nothing Then
            Exit Sub
        End If

        Dim GetAttendanceFile() As String = IO.File.ReadAllLines(Osdcard.FileName)
        dgvAttendanceList.Rows.Clear()

        ShowWaitForm("Please wait while loading...")
        Application.DoEvents()
        MainForm.MainProgressBar.Maximum = GetAttendanceFile.Count + 2

        Dim empid, timen, daten, ampm, stat As String
        Dim yyyy, mm, dd, ymd As String

        Dim str As String
        For s As Integer = 1 To GetAttendanceFile.Length - 1
            If GetAttendanceFile(s) = Nothing Then
                Continue For
            End If
            Dim GetSplitedDetailed() As String = Split(GetAttendanceFile(s), vbTab)


            str = GetSplitedDetailed(0)
            empid = str.Substring(0, 12)
            daten = str.Substring(20, 8)
            ampm = str.Substring(18, 2)
            stat = str.Substring(28, 2)
            timen = str.Substring(12, 5)

            yyyy = daten.Substring(0, 4)
            mm = daten.Substring(4, 2)
            dd = daten.Substring(6, 2)
            ymd = mm + "/" + dd + "/" + yyyy

            Dim D As Date
            D = timen

            Dim TB() As String = Split(D, ":")
            Dim result As String
            result = TB(0) + ((TB(1) * 100) / 60) / 100

            Dim timeampm As String = timen + ":00 " + ampm
            Dim get_datetime As String = ymd + " " + timeampm


            Dim dt As DateTime = timeampm
            Dim dtoffset As DateTime
            dtoffset = Format(dt.ToUniversalTime, "HH:mm")



            ' Dim dt As DateTime = get_datetime.ToString("HH:mm:ss")


            'dt = DateTime.ParseExact(get_datetime, "HHmmss", Nothing)

            'Dim sTheTimeFormatYouWant As String = String.Empty

            'sTheTimeFormatYouWant = dt.ToString("h:mm tt")

            'thisTime = row.TimeStamp.DateTime
            'currTime = Format(thisTime, "yyyy/M/dd HH:mm:ss")
            'Dim ename As String = getStringItem("Select concat(lastname, ' ', Firstname, MiddleName) From Employee Where rowid = '" & empn & "'")
            'empnames = ename
            Application.DoEvents()
            MainForm.MainProgressBar.Value += 1

         
            dgvAttendanceList.Rows.Add(empid, "", ymd, timen)
            
        Next
        dgvAttendanceList.AllowUserToAddRows = False
        dgvAttendanceList.AllowUserToDeleteRows = False
        dgvAttendanceList.AllowUserToOrderColumns = True
        dgvAttendanceList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvAttendanceList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        dgvAttendanceList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAttendanceList.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        CloseWaitForm()
    End Sub
End Class