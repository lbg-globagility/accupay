Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class EmployeeOvertimeForm

    Private _currentOvertime As Overtime

    Private _employees As List(Of Employee)

    Private _allEmployees As List(Of Employee)

    Private _currentOvertimes As List(Of Overtime)

    Private _changedOvertimes As List(Of Overtime)

    Private ReadOnly _textBoxDelayedAction As DelayedAction(Of Boolean)

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Private ReadOnly _policyHelper As IPolicyHelper

    Private _currentRolePermission As RolePermission

    Sub New()

        InitializeComponent()

        _employees = New List(Of Employee)

        _allEmployees = New List(Of Employee)

        _currentOvertimes = New List(Of Overtime)

        _changedOvertimes = New List(Of Overtime)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _textBoxDelayedAction = New DelayedAction(Of Boolean)

    End Sub

    Private Async Sub EmployeeOvertimeForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        StartTimePicker.ShowCheckBox = _policyHelper.UseMassOvertime

        InitializeComponentSettings()

        Await CheckRolePermissions()

        LoadStatusList()

        Await LoadEmployees()
        Await ShowEmployeeList()

        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

        HideControlsBaseOnShiftOvertimePolicy()
    End Sub

    Private Sub HideControlsBaseOnShiftOvertimePolicy()
        If _policyHelper.ShiftBasedAutomaticOvertimePolicy.Enabled Then
            NewToolStripButton.Visible = False
            SaveToolStripButton.Visible = False
            DeleteToolStripButton.Visible = False
            CancelToolStripButton.Visible = False
            ImportToolStripButton.Visible = False
        End If
    End Sub

    Private Async Function CheckRolePermissions() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.OVERTIME)

        NewToolStripButton.Visible = False
        ImportToolStripButton.Visible = False
        SaveToolStripButton.Visible = False
        CancelToolStripButton.Visible = False
        DeleteToolStripButton.Visible = False
        DetailsTabLayout.Enabled = False

        If role.Success Then

            _currentRolePermission = role.RolePermission

            If _currentRolePermission.Create Then
                NewToolStripButton.Visible = True
                ImportToolStripButton.Visible = True

            End If

            If _currentRolePermission.Update Then
                SaveToolStripButton.Visible = True
                CancelToolStripButton.Visible = True
                DetailsTabLayout.Enabled = True
            End If

            If _currentRolePermission.Delete Then
                DeleteToolStripButton.Visible = True

            End If

        End If
    End Function

    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs)

        _textBoxDelayedAction.ProcessAsync(
            Async Function()
                Await FilterEmployees(SearchTextBox.Text.ToLower())

                Return True
            End Function)
    End Sub

    Private Sub EmployeeOvertimeForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        myBalloon(, , EmployeePictureBox, , , 1)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Async Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click
        Using form = New ImportOvertimeForm()
            form.ShowDialog()

            If form.IsSaved Then

                Dim currentEmployee = GetSelectedEmployee()
                If currentEmployee IsNot Nothing Then

                    Await LoadOvertimes(currentEmployee)

                End If

                myBalloon("Overtimes Successfully Imported", "Import Overtimes", EmployeePictureBox, 100, -20)
            End If
        End Using
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, FormTitleLabel, 400)
    End Sub

    Private Sub InitializeComponentSettings()
        OvertimeGridView.AutoGenerateColumns = False
        EmployeesDataGridView.AutoGenerateColumns = False
    End Sub

    Private Sub LoadStatusList()

        Dim repository = MainServiceProvider.GetRequiredService(Of IOvertimeRepository)
        StatusComboBox.DataSource = repository.GetStatusList()

    End Sub

    Private Async Function FilterEmployees(Optional searchValue As String = "") As Task
        Dim filteredEmployees As New List(Of Employee)

        RemoveHandler EmployeesDataGridView.SelectionChanged, AddressOf EmployeesDataGridView_SelectionChanged

        If String.IsNullOrEmpty(searchValue) Then
            EmployeesDataGridView.DataSource = Me._employees
        Else
            EmployeesDataGridView.DataSource =
                Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If

        Await ShowEmployeeOvertimes()

        AddHandler EmployeesDataGridView.SelectionChanged, AddressOf EmployeesDataGridView_SelectionChanged
    End Function

    Private Async Function LoadEmployees() As Task

        Me._allEmployees = (Await _employeeRepository.GetAllWithPositionAsync(z_OrganizationID)).
            OrderBy(Function(e) e.LastName).
            ToList

    End Function

    Private Async Function ShowEmployeeList() As Task

        If ShowAllCheckBox.Checked Then

            Me._employees = Me._allEmployees
        Else

            Me._employees = Me._allEmployees.Where(Function(e) e.IsActive).ToList

        End If

        Await FilterEmployees()
    End Function

    Private Sub ResetForm()

        'employee details

        EmployeeNameTextBox.Text = String.Empty
        EmployeeNumberTextBox.Text = String.Empty

        EmployeePictureBox.Image = Nothing

        'overtime grid view
        RemoveHandler OvertimeGridView.SelectionChanged, AddressOf OvertimeGridView_SelectionChanged

        OvertimeGridView.DataSource = Nothing

        AddHandler OvertimeGridView.SelectionChanged, AddressOf OvertimeGridView_SelectionChanged

        'overtime details

        Me._currentOvertime = Nothing

        DetailsTabLayout.Enabled = False

        StartDatePicker.ResetText()
        StartDatePicker.DataBindings.Clear()

        EndDatePicker.ResetText()
        EndDatePicker.DataBindings.Clear()

        StartTimePicker.ResetText()
        StartTimePicker.DataBindings.Clear()

        EndTimePicker.ResetText()
        EndTimePicker.DataBindings.Clear()

        ReasonTextBox.Clear()
        ReasonTextBox.DataBindings.Clear()

        CommentTextBox.Clear()
        CommentTextBox.DataBindings.Clear()

        StatusComboBox.SelectedIndex = -1
        StatusComboBox.DataBindings.Clear()
    End Sub

    Private Async Sub ShowAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowAllCheckBox.CheckedChanged

        RemoveHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

        SearchTextBox.Clear()
        Await ShowEmployeeList()

        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged
    End Sub

    Private Function GetSelectedEmployee() As Employee
        If EmployeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(EmployeesDataGridView.CurrentRow.DataBoundItem, Employee)
    End Function

    Private Sub ForceGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        EmployeeInfoTabLayout.Focus()
        UpdateEndDateDependingOnStartAndEndTimes()
    End Sub

    Private Async Function LoadOvertimes(currentEmployee As Employee) As Task
        If currentEmployee?.RowID Is Nothing Then Return

        Dim repository = MainServiceProvider.GetRequiredService(Of IOvertimeRepository)
        Me._currentOvertimes = (Await repository.GetByEmployeeAsync(currentEmployee.RowID.Value)).
            OrderByDescending(Function(a) a.OTStartDate).
            ToList

        Me._changedOvertimes = Me._currentOvertimes.CloneListJson()

        'cloning TimeSpans objects that are Nothing(null) results to default(T) which is {00:00:00}
        For index As Integer = 0 To Me._changedOvertimes.Count - 1
            Me._changedOvertimes(index).OTStartTime = Me._currentOvertimes(index).OTStartTime
            Me._changedOvertimes(index).OTEndTime = Me._currentOvertimes(index).OTEndTime
        Next

        PopulateOvertimeGridView()

    End Function

    Private Function CheckIfBothNullorBothHaveValue(object1 As Object, object2 As Object) As Boolean

        Return (object1 Is Nothing AndAlso object2 Is Nothing) OrElse
            (object1 IsNot Nothing AndAlso object2 IsNot Nothing)

    End Function

    Private Function CheckIfOvertimeIsChanged(newOvertime As Overtime) As Boolean
        If Me._currentOvertime Is Nothing Then Return False

        Dim oldOvertime =
            Me._changedOvertimes.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newOvertime.RowID))

        If oldOvertime Is Nothing Then Return False

        Dim hasChanged = False

        If _
            newOvertime.OTStartDate.Date <> oldOvertime.OTStartDate.Date OrElse
            Not CheckIfBothNullorBothHaveValue(newOvertime.OTStartTime, oldOvertime.OTStartTime) OrElse
            newOvertime.OTStartTime.StripSeconds <> oldOvertime.OTStartTime.StripSeconds OrElse
            Not CheckIfBothNullorBothHaveValue(newOvertime.OTEndTime, oldOvertime.OTEndTime) OrElse
            newOvertime.OTEndTime.StripSeconds <> oldOvertime.OTEndTime.StripSeconds OrElse
            newOvertime.Reason <> oldOvertime.Reason OrElse
            newOvertime.Comments <> oldOvertime.Comments OrElse
            newOvertime.Status <> oldOvertime.Status Then

            hasChanged = True
        End If

        Return hasChanged

    End Function

    Private Function GetSelectedOvertime() As Overtime
        Return CType(OvertimeGridView.CurrentRow.DataBoundItem, Overtime)
    End Function

    Private Sub PopulateOvertimeForm(overtime As Overtime)
        Me._currentOvertime = overtime

        StartDatePicker.DataBindings.Clear()
        StartDatePicker.DataBindings.Add("Value", Me._currentOvertime, "OTStartDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndDatePicker.DataBindings.Clear()
        EndDatePicker.DataBindings.Add("Value", Me._currentOvertime, "OTEndDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        StartTimePicker.DataBindings.Clear()

        'If _policyHelper.UseMassOvertime Then StartTimePicker.Checked = _currentOvertime.OTStartTime.HasValue

        StartTimePicker.DataBindings.Add("Value", Me._currentOvertime, "OTStartTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndTimePicker.DataBindings.Clear()
        EndTimePicker.DataBindings.Add("Value", Me._currentOvertime, "OTEndTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        ReasonTextBox.DataBindings.Clear()
        ReasonTextBox.DataBindings.Add("Text", Me._currentOvertime, "Reason")

        CommentTextBox.DataBindings.Clear()
        CommentTextBox.DataBindings.Add("Text", Me._currentOvertime, "Comments")

        StatusComboBox.DataBindings.Clear()
        StatusComboBox.DataBindings.Add("Text", Me._currentOvertime, "Status")

        If _currentRolePermission.Update Then

            DetailsTabLayout.Enabled = True
        End If

    End Sub

    Private Async Function DeleteOvertime(currentEmployee As Employee, messageTitle As String) As Task

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of IOvertimeDataService)
                Await dataService.DeleteAsync(
                    id:=Me._currentOvertime.RowID.Value,
                    currentlyLoggedInUserId:=z_User)

                Await LoadOvertimes(currentEmployee)

                ShowBalloonInfo("Successfully Deleted.", messageTitle)

            End Function)
    End Function

    Private Sub UpdateEndDateDependingOnStartAndEndTimes()
        If Me._currentOvertime Is Nothing Then Return

        If StartTimePicker.Value.HasValue AndAlso EndTimePicker.Value.TimeOfDay < StartTimePicker.Value.Value.TimeOfDay Then

            Me._currentOvertime.OTEndDate = Me._currentOvertime.OTStartDate.AddDays(1)
        Else
            Me._currentOvertime.OTEndDate = Me._currentOvertime.OTStartDate

        End If

        EndDatePicker.Value = Me._currentOvertime.OTEndDate

    End Sub

    Private Async Sub EmployeesDataGridView_SelectionChanged(sender As Object, e As EventArgs)

        Await ShowEmployeeOvertimes()

    End Sub

    Private Async Function ShowEmployeeOvertimes() As Task

        ResetForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        EmployeeNameTextBox.Text = currentEmployee.FullNameWithMiddleInitial
        EmployeeNumberTextBox.Text = currentEmployee.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(currentEmployee.Image)

        Await LoadOvertimes(currentEmployee)

    End Function

    Private Sub OvertimeGridView_SelectionChanged(sender As Object, e As EventArgs)

        ShowOvertimeDetails()

    End Sub

    Private Sub ShowOvertimeDetails()

        If OvertimeGridView.CurrentRow Is Nothing Then Return

        Dim currentOvertime As Overtime = GetSelectedOvertime()

        Dim currentEmployee = GetSelectedEmployee()
        If currentOvertime IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentOvertime.EmployeeID, currentEmployee.RowID) Then

            PopulateOvertimeForm(currentOvertime)
        End If
    End Sub

    Private Async Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

        Dim employee As Employee = GetSelectedEmployee()

        If employee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")

            Return
        End If

        Dim form As New AddOvertimeForm(employee)
        form.ShowDialog()

        If form.IsSaved Then

            Await LoadOvertimes(employee)

            If form.ShowBalloonSuccess Then
                ShowBalloonInfo("Overtime Successfully Added", "Saved")
            End If
        End If

    End Sub

    Private Sub OvertimeListBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) Handles OvertimeListBindingSource.CurrentItemChanged

        Dim currentRow = OvertimeGridView.CurrentRow

        If currentRow Is Nothing Then Return

        If Me._currentOvertime Is Nothing Then Return

        Dim hasChanged = CheckIfOvertimeIsChanged(Me._currentOvertime)

        If hasChanged Then
            currentRow.DefaultCellStyle.BackColor = Color.Yellow
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Red
        Else
            currentRow.DefaultCellStyle.BackColor = Color.White
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Black

        End If

    End Sub

    Private Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click
        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        If Me._currentOvertimes Is Nothing Then
            MessageBoxHelper.Warning("No changed overtimes!")
            Return
        End If

        Me._currentOvertimes = Me._changedOvertimes.CloneListJson()

        'cloning TimeSpans objects that are Nothing(null) results to default(T) which is {00:00:00}
        For index As Integer = 0 To Me._currentOvertimes.Count - 1
            Me._currentOvertimes(index).OTStartTime = Me._changedOvertimes(index).OTStartTime
            Me._currentOvertimes(index).OTEndTime = Me._changedOvertimes(index).OTEndTime
        Next

        PopulateOvertimeGridView()
    End Sub

    Private Sub PopulateOvertimeGridView()

        RemoveHandler OvertimeGridView.SelectionChanged, AddressOf OvertimeGridView_SelectionChanged

        OvertimeListBindingSource.DataSource = Me._currentOvertimes

        OvertimeGridView.DataSource = OvertimeListBindingSource

        ShowOvertimeDetails()

        AddHandler OvertimeGridView.SelectionChanged, AddressOf OvertimeGridView_SelectionChanged

    End Sub

    Private Async Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        Const messageTitle As String = "Delete Overtime"

        If Me._currentOvertime?.RowID Is Nothing Then
            MessageBoxHelper.Warning("No overtime selected!")

            Return
        End If

        Dim repository = MainServiceProvider.GetRequiredService(Of IOvertimeRepository)
        Dim currentOvertime = Await repository.GetByIdAsync(Me._currentOvertime.RowID.Value)

        If currentOvertime Is Nothing Then

            MessageBoxHelper.Warning("Overtime not found in database! Please close this form then open again.")

            Return
        End If

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete this?", "Confirm Deletion") = False Then

            Return
        End If

        Await DeleteOvertime(currentEmployee, messageTitle)

    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        ForceGridViewCommit()

        Dim overlappingOvertimes = ScrutinizedConflictingOvertime(_currentOvertimes)
        Dim hasWorry = overlappingOvertimes.Any()

        Dim messageTitle = "Update Overtimes"

        If hasWorry Then
            Dim overtime1 = overlappingOvertimes.FirstOrDefault
            Dim overtime2 = overlappingOvertimes.LastOrDefault
            Dim timeFormat = "hh\:mm"
            MessageBoxHelper.
                Warning($"An overtime overlaps another overtime.
                {overtime1.OTStartTime.Value.ToString(timeFormat)}-{overtime1.OTEndTime.Value.ToString(timeFormat)} {overtime1.OTStartDate.ToShortDateString}-{overtime1.OTEndDate.ToShortDateString}
                {overtime2.OTStartTime.Value.ToString(timeFormat)}-{overtime2.OTEndTime.Value.ToString(timeFormat)} {overtime2.OTStartDate.ToShortDateString}-{overtime2.OTEndDate.ToShortDateString}",
                        messageTitle)

            SetCurrentOvertimeRow(overtime2)
            Return
        End If

        Dim changedOvertimes As New List(Of Overtime)

        For Each item In Me._currentOvertimes

            If CheckIfOvertimeIsChanged(item) Then
                changedOvertimes.Add(item)
            End If
        Next

        If changedOvertimes.Count < 1 Then

            MessageBoxHelper.Warning("No changed overtime!", messageTitle)
            Return

        ElseIf changedOvertimes.Count > 1 AndAlso MessageBoxHelper.Confirm(Of Boolean) _
            ($"You are about to update multiple overtimes. Do you want to proceed?", "Confirm Multiple Updates") = False Then

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of IOvertimeDataService)

                Await dataService.SaveManyAsync(changedOvertimes, z_User)

                ShowBalloonInfo($"{changedOvertimes.Count} Overtime(s) Successfully Updated.", messageTitle)

                Dim currentEmployee = GetSelectedEmployee()

                If currentEmployee IsNot Nothing Then

                    Await LoadOvertimes(currentEmployee)

                End If

            End Function)
    End Sub

    Public Function ScrutinizedConflictingOvertime(otList As List(Of Overtime), Optional otStatus As String = "") As List(Of Overtime)
        Dim employeeIDs = otList.GroupBy(Function(ot) ot.EmployeeID).Select(Function(id) id.Key).ToList()

        Dim overlappingOvertime As New List(Of Overtime)

        Dim approved = If(String.IsNullOrEmpty(otStatus), Overtime.StatusApproved, otStatus)

        For Each employeeID In employeeIDs

            Dim overtimeList = otList.
                Where(Function(ot) ot.EmployeeID.Value = employeeID.Value).
                GroupBy(Function(ot) ot.OTStartDate).
                GroupBy(Function(ot) ot.FirstOrDefault.OTEndDate).
                Select(Function(ot) New With {ot.Key, ot.FirstOrDefault.FirstOrDefault.OTEndDate}).
                ToList()

            Dim itHas = False 'overtimeList.Any()

            For Each ot In overtimeList
                Dim otStartDate = ot.Key
                Dim overtimes = otList.
                    Where(Function(o) o.OTStartDate = otStartDate AndAlso o.OTEndDate = ot.OTEndDate).
                    Where(Function(o) o.Status = approved).
                    OrderBy(Function(o) o.OTStartTime).
                    ToList()
                'OrderBy(Function(o) o.OTEndTime).

                Dim isMoreThanOne = If(overtimes.Any(), overtimes.Count() > 1, False)
                If Not isMoreThanOne Then Continue For

                Dim count = overtimes.Count - 1
                Dim preceedingOvertime = overtimes.FirstOrDefault
                'Dim thisIsIt = preceedingOvertime.EmployeeID.Value = 75 And preceedingOvertime.OTStartDate.Date = New Date(2020, 2, 18)
                'If thisIsIt Then Console.WriteLine("This is it...")
                For i = 1 To count
                    Dim proceedingOvertime = overtimes(i)

                    If preceedingOvertime.OTStartTime.Value < proceedingOvertime.OTStartTime.Value _
                        AndAlso proceedingOvertime.OTStartTime.Value < preceedingOvertime.OTEndTime.Value Then

                        itHas = True

                        overlappingOvertime.Add(preceedingOvertime)
                        overlappingOvertime.Add(proceedingOvertime)
                        Exit For
                    End If

                    preceedingOvertime = proceedingOvertime
                Next

                If itHas Then Exit For
            Next

            If itHas Then Exit For
        Next

        Return overlappingOvertime
    End Function

    Private Sub SetCurrentOvertimeRow(overtime As Overtime)
        Dim gridRow = OvertimeGridView.Rows.OfType(Of DataGridViewRow).
            Where(Function(r) ObjectUtils.ToNullableTimeSpan(r.Cells(Column2.Name).Value).Value = overtime.OTStartTime.Value).
            Where(Function(r) ObjectUtils.ToNullableTimeSpan(r.Cells(Column4.Name).Value).Value = overtime.OTEndTime.Value).
            Where(Function(r) ObjectUtils.ToNullableDateTime(r.Cells(Column1.Name).Value).Value = overtime.OTStartDate).
            Where(Function(r) ObjectUtils.ToNullableDateTime(r.Cells(Column3.Name).Value).Value = overtime.OTEndDate).
            Where(Function(r) r.Cells(Column7.Name).Value.ToString() = overtime.Status).
            Where(Function(r) r.Cells(Column5.Name).Value.ToString() = overtime.Reason).
            Where(Function(r) r.Cells(Column6.Name).Value.ToString() = overtime.Comments).
            FirstOrDefault

        If gridRow Is Nothing Then Return

        Dim selectedCells = gridRow.Cells
        Dim selectedCell = selectedCells.OfType(Of DataGridViewCell).FirstOrDefault
        OvertimeGridView.CurrentCell = selectedCell

        gridRow.Selected = True
        OvertimeGridView.CurrentRow.Selected = True

        Dim selectedOvertime = DirectCast(gridRow.DataBoundItem, Overtime)
        PopulateOvertimeForm(selectedOvertime)
    End Sub

    Private Sub TimePicker_Leave(sender As Object, e As EventArgs) _
        Handles StartTimePicker.Leave, EndTimePicker.Leave, StartDatePicker.Leave

        UpdateEndDateDependingOnStartAndEndTimes()

    End Sub

    Private Sub UserActivityToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityToolStripButton.Click

        Dim formEntityName As String = "Overtime"

        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

End Class
