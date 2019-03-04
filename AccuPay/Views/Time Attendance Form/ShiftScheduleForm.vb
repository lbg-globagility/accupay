Imports AccuPay.Tools

Public Class ShiftScheduleForm
    Private organizationId As Integer

    Private machineCulture As Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture

    Private sunDayName = DayNames.Sun.ToString

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        LoadChanges()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

    End Sub

    Private Sub ColorizeSundays(gridRow As DataGridViewRow)
        Dim shortDayName =
            Convert.ToDateTime(gridRow.Cells(eshDayName.Name).Value).ToString(eshDayName.DefaultCellStyle.Format)

        If shortDayName = sunDayName Then
            gridRow.DefaultCellStyle.ForeColor = Color.Red
        Else
            Return
        End If
    End Sub

    Private Sub ZebraliseEmployeeRows()
        Dim ebonyStyle = Color.Gainsboro
        Dim ivoryStyle = Color.White

        Dim eIDs = EmployeeTreeView1.GetTickedEmployees.Select(Function(e) e.RowID.Value).ToList

        Dim isEven = False
        Dim i = 1

        For Each eID In eIDs
            isEven = i Mod 2 = 0

            If isEven Then
                ColorEmployeeRows(eID, ivoryStyle)
            Else
                ColorEmployeeRows(eID, ebonyStyle)
            End If

            i += 1
        Next

    End Sub

    Private Sub ColorEmployeeRows(employeePrimKey As Integer, backColor As Color)
        Dim employeeGridRows =
                grid.Rows.OfType(Of DataGridViewRow).
                Where(Function(row) _
                          Equals(Convert.ToInt32(row.Cells(eshEmployeeId.Name).Value),
                                 employeePrimKey))

        For Each eGridRow In employeeGridRows
            eGridRow.DefaultCellStyle.BackColor = backColor

            ColorizeSundays(eGridRow)
        Next
    End Sub

    Private Sub EmployeeTreeView1_TickedEmployee(s As Object, e As EventArgs) Handles EmployeeTreeView1.TickedEmployee
        LoadChanges()
    End Sub

    Private Sub LoadChanges()
        Dim dates =
            Calendar.EachDay(dtpDateFrom.Value.ToShortDateString,
                             dtpDateTo.Value.ToShortDateString)

        Dim employees = EmployeeTreeView1.GetTickedEmployees
        Dim employeeList = From emp In employees
                           Order By String.Concat(emp.LastName, emp.FirstName)
                           Let ee = emp
                           Select New ShiftScheduleModel With {
                               .EmployeeId = emp.RowID,
                               .EmployeeNo = emp.EmployeeNo,
                               .FullName = String.Join(", ", emp.LastName, emp.FirstName)}

        Dim _models As New List(Of ShiftScheduleModel)

        Dim dateList = dates.ToList

        For Each ee In employeeList.ToList
            For Each dateVal In dateList
                CollectShiftSchedModel(ee,
                                       dateVal.ToShortDateString,
                                       _models)
            Next
        Next

        grid.DataSource = _models
    End Sub

    Private Sub CollectShiftSchedModel(ee As ShiftScheduleModel, dateVal As Date?, modelList As ICollection(Of ShiftScheduleModel))
        Dim newEe As New ShiftScheduleModel With {
            .EmployeeId = ee.EmployeeId,
            .EmployeeNo = ee.EmployeeNo,
            .FullName = ee.FullName,
            .DateValue = dateVal,
            .DayName = GetDayName(dateVal.Value),
            .TimeFrom = TimeUtility.ToDateTime(TimeTextBox1.Value),
            .TimeTo = TimeUtility.ToDateTime(TimeTextBox2.Value),
            .BreakFrom = TimeUtility.ToDateTime(TimeTextBox3.Value),
            .BreakTo = TimeUtility.ToDateTime(TimeTextBox4.Value)}

        modelList.Add(newEe)
    End Sub

    Private Function GetDayName(dateValue As Date) As String
        Dim dayOfWeek As DayOfWeek = machineCulture.Calendar.GetDayOfWeek(dateValue)
        Return machineCulture.DateTimeFormat.GetDayName(dayOfWeek)
    End Function

    Private Sub EmployeeTreeView1_Load(sender As Object, e As EventArgs) Handles EmployeeTreeView1.Load

    End Sub

    Private Sub dtpDateFrom_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateFrom.ValueChanged
        Dim start As Date = dtpDateFrom.Value.ToShortDateString
        Dim finish As Date = dtpDateTo.Value.ToShortDateString

        If start > finish Then
            dtpDateTo.Value = start
        Else
            LoadChanges()
        End If
    End Sub

    Private Sub dtpDateTo_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateTo.ValueChanged
        Dim start As Date = dtpDateFrom.Value.ToShortDateString
        Dim finish As Date = dtpDateTo.Value.ToShortDateString

        If start > finish Then
            dtpDateFrom.Value = finish
        Else
            LoadChanges()
        End If
    End Sub

    Private Sub ShiftScheduleForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
    End Sub

    Private Sub ShiftScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        organizationId = z_OrganizationID

        grid.AutoGenerateColumns = False

        EmployeeTreeView1.SwitchOrganization(organizationId)

    End Sub

    Private Class ShiftScheduleModel
        Public Property EmployeeId As Integer?
        Public Property EmployeeNo As String
        Public Property FullName As String
        Public Property DateValue As Date?
        Public Property DayName As String
        Public Property TimeFrom As DateTime?
        Public Property TimeTo As DateTime?
        Public Property BreakFrom As DateTime?
        Public Property BreakTo As DateTime?
    End Class

    Private Enum DayNames
        Sun
        Mon
        Tue
        Wed
        Thur
        Fri
        Sat
    End Enum

    Private Sub grid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellContentClick

    End Sub

    Private Sub grid_DataSourceChanged(sender As Object, e As EventArgs) Handles grid.DataSourceChanged
        ZebraliseEmployeeRows()
    End Sub

    Private Sub grid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles grid.DataError
        Console.WriteLine("grid_DataError : {0}, {1}", e.Cancel, e.Exception.Message)

        'Dim rowItem = grid.Item(e.ColumnIndex, e.RowIndex)

        'rowItem.ErrorText = String.Empty

        'If e.Exception IsNot Nothing Then rowItem.ErrorText = e.Exception.Message

        'e.Cancel = False
    End Sub

    Private Sub grid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellEndEdit
        Console.WriteLine("grid_CellEndEdit")

        'Dim rowItem = grid.Item(e.ColumnIndex, e.RowIndex)

        'If rowItem.Value Is Nothing Then rowItem.ErrorText = String.Empty

    End Sub

    Private Sub grid_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles grid.CellBeginEdit
        Console.WriteLine("grid_CellBeginEdit")
        'Dim rowItem = grid.Item(e.ColumnIndex, e.RowIndex)
        'rowItem.ErrorText = String.Empty
    End Sub

    Private Sub grid_CellParsing(sender As Object, e As DataGridViewCellParsingEventArgs) Handles grid.CellParsing
        Console.WriteLine("grid_CellParsing")

        If e.ColumnIndex = eshTimeFrom.Index Then
            Dim _value As DateTime? = TimeUtility.ToDateTime(Calendar.ToTimespan(e.Value))
            'If _value.HasValue Then
            e.Value = _value
            '    'Else
            '    '    e.Value = String.Empty
            'End If

            e.ParsingApplied = _value.HasValue
        Else
            e.ParsingApplied = False
        End If

    End Sub

End Class