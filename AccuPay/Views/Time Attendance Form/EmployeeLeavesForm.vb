Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Entity
Imports AccuPay.ModelData
Imports AccuPay.Repository
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils

Public Class EmployeeLeavesForm

    Private _employees As New List(Of Entities.Employee)

    Private _allEmployees As New List(Of Entities.Employee)

    Private _currentLeave As Leave

    Private _currentLeaves As New List(Of Leave)

    Private _changedLeaves As New List(Of Leave)

    Private _leaveRepository As New LeaveRepository

    Private _employeeRepository As New Repositories.EmployeeRepository

    Private _productRepository As New ProductRepository

    Private _textBoxDelayedAction As New DelayedAction(Of Boolean)

    Private Async Sub EmployeeLeavesForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeComponentSettings()

        LoadStatusList()
        Await LoadLeaveTypes()

        Await LoadEmployees()
        Await ShowEmployeeList()

        ResetForm()

    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs) Handles SearchTextBox.TextChanged

        _textBoxDelayedAction.ProcessAsync(Async Function()
                                               Await FilterEmployees(SearchTextBox.Text.ToLower())

                                               Return True
                                           End Function)
    End Sub

    Private Sub EmployeeLeavesForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        InfoBalloon(, , FormTitleLabel, , , 1)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Async Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click
        Dim importForm As New ImportLeaveForm()
        If Not importForm.ShowDialog() = DialogResult.OK Then Return

        Dim succeed = Await importForm.SaveAsync()

        If succeed Then
            Dim currentEmployee = GetSelectedEmployee()

            If currentEmployee IsNot Nothing Then

                Await LoadLeaves(currentEmployee)

            End If
            InfoBalloon("Imported successfully.", "Done Importing Employee Leave(s)", EmployeePictureBox, 0, -69)
        End If
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, FormTitleLabel, 400)
    End Sub

    Private Sub InitializeComponentSettings()
        LeaveGridView.AutoGenerateColumns = False
        EmployeesDataGridView.AutoGenerateColumns = False
    End Sub

    Private Sub LoadStatusList()

        StatusComboBox.DataSource = _leaveRepository.GetStatusList()

    End Sub

    Private Async Function FilterEmployees(Optional searchValue As String = "") As Task
        Dim filteredEmployees As New List(Of Employee)

        If String.IsNullOrEmpty(searchValue) Then
            EmployeesDataGridView.DataSource = Me._employees
        Else
            EmployeesDataGridView.DataSource =
                Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If
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

        Me._currentLeave = Nothing

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

        LeaveTypeComboBox.SelectedIndex = -1
        LeaveTypeComboBox.DataBindings.Clear()

        StatusComboBox.SelectedIndex = -1
        StatusComboBox.DataBindings.Clear()
    End Sub

    Private Async Sub ShowAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowAllCheckBox.CheckedChanged
        Await ShowEmployeeList()
    End Sub

    Private Function GetSelectedEmployee() As Data.Entities.Employee
        If EmployeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(EmployeesDataGridView.CurrentRow.DataBoundItem, Data.Entities.Employee)
    End Function

    Private Sub ForceGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        EmployeeInfoTabLayout.Focus()
        UpdateEndDateDependingOnStartAndEndTimes()
    End Sub

    Private Async Function LoadLeaves(currentEmployee As Data.Entities.Employee) As Task
        If currentEmployee Is Nothing Then Return

        Me._currentLeaves = (Await _leaveRepository.GetByEmployeeAsync(currentEmployee.RowID)).
                                OrderByDescending(Function(a) a.StartDate).
                                ToList

        Me._changedLeaves = Me._currentLeaves.CloneListJson()

        'cloning TimeSpans objects that are Nothing(null) results to default(T) which is {00:00:00}
        For index As Integer = 0 To Me._changedLeaves.Count - 1
            Me._changedLeaves(index).StartTime = Me._currentLeaves(index).StartTime
            Me._changedLeaves(index).EndTime = Me._currentLeaves(index).EndTime
        Next

        LeaveListBindingSource.DataSource = Me._currentLeaves

        LeaveGridView.DataSource = LeaveListBindingSource

    End Function

    Private Async Sub EmployeesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles EmployeesDataGridView.SelectionChanged

        ResetForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        EmployeeNameTextBox.Text = currentEmployee.FullNameWithMiddleInitial
        EmployeeNumberTextBox.Text = currentEmployee.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(currentEmployee.Image)

        VacationLeaveAllowanceTextBox.Text = currentEmployee.VacationLeaveAllowance
        SickLeaveAllowanceTextBox.Text = currentEmployee.SickLeaveAllowance

        VacationLeaveBalanceTextBox.Text = Await EmployeeData.GetVacationLeaveBalance(currentEmployee.RowID)
        SickLeaveBalanceTextBox.Text = Await EmployeeData.GetSickLeaveBalance(currentEmployee.RowID)

        Await LoadLeaves(currentEmployee)

    End Sub

    Private Function GetSelectedLeave() As Leave
        Return CType(LeaveGridView.CurrentRow.DataBoundItem, Leave)
    End Function

    Private Sub PopulateLeaveForm(leave As Leave)
        Me._currentLeave = leave

        StartDatePicker.DataBindings.Clear()
        StartDatePicker.DataBindings.Add("Value", Me._currentLeave, "StartDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndDatePicker.DataBindings.Clear()
        EndDatePicker.DataBindings.Add("Value", Me._currentLeave, "ProperEndDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        StartTimePicker.DataBindings.Clear()
        StartTimePicker.DataBindings.Add("Value", Me._currentLeave, "StartTimeFull", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

        EndTimePicker.DataBindings.Clear()
        EndTimePicker.DataBindings.Add("Value", Me._currentLeave, "EndTimeFull", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

        ReasonTextBox.DataBindings.Clear()
        ReasonTextBox.DataBindings.Add("Text", Me._currentLeave, "Reason")

        CommentTextBox.DataBindings.Clear()
        CommentTextBox.DataBindings.Add("Text", Me._currentLeave, "Comments")

        LeaveTypeComboBox.DataBindings.Clear()
        LeaveTypeComboBox.DataBindings.Add("Text", Me._currentLeave, "LeaveType")

        StatusComboBox.DataBindings.Clear()
        StatusComboBox.DataBindings.Add("Text", Me._currentLeave, "Status")

        DetailsTabLayout.Enabled = True

    End Sub

    Private Async Function LoadLeaveTypes() As Task

        Dim leaveList = New List(Of Product)(Await _productRepository.GetLeaveTypes())

        leaveList = leaveList.Where(Function(a) a.PartNo IsNot Nothing).
                                                Where(Function(a) a.PartNo.Trim <> String.Empty).
                                                ToList

        Dim leaveTypes = _productRepository.ConvertToStringList(leaveList)

        LeaveTypeComboBox.DataSource = leaveTypes

    End Function

    Private Function CheckIfBothNullorBothHaveValue(object1 As Object, object2 As Object) As Boolean

        Return (object1 Is Nothing AndAlso object2 Is Nothing) OrElse
            (object1 IsNot Nothing AndAlso object2 IsNot Nothing)

    End Function

    Private Function CheckIfLeaveIsChanged(newLeave As Leave) As Boolean
        If Me._currentLeave Is Nothing Then Return False

        Dim oldLeave =
            Me._changedLeaves.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newLeave.RowID))

        If oldLeave Is Nothing Then Return False

        Dim hasChanged = False

        If _
            newLeave.StartDate.Date <> oldLeave.StartDate.Date OrElse
            Not CheckIfBothNullorBothHaveValue(newLeave.EndDate, oldLeave.EndDate) OrElse
            newLeave.EndDate?.Date <> oldLeave.EndDate?.Date OrElse
            Not CheckIfBothNullorBothHaveValue(newLeave.StartTime, oldLeave.StartTime) OrElse
            newLeave.StartTime.StripSeconds <> oldLeave.StartTime.StripSeconds OrElse
            Not CheckIfBothNullorBothHaveValue(newLeave.EndTime, oldLeave.EndTime) OrElse
            newLeave.EndTime.StripSeconds <> oldLeave.EndTime.StripSeconds OrElse
            newLeave.Reason <> oldLeave.Reason OrElse
            newLeave.Comments <> oldLeave.Comments OrElse
            newLeave.LeaveType <> oldLeave.LeaveType OrElse
            newLeave.Status <> oldLeave.Status Then

            hasChanged = True
        End If

        Return hasChanged

    End Function

    Private Function RecordUpdate(newLeave As Leave)

        Dim oldLeave =
            Me._changedLeaves.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newLeave.RowID))

        If oldLeave Is Nothing Then Return False

        Dim changes = New List(Of Data.Entities.UserActivityItem)

        If newLeave.StartDate <> oldLeave.StartDate Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = oldLeave.RowID,
                        .Description = $"Update leave start date from '{oldLeave.StartDate.ToShortDateString}' to '{newLeave.StartDate.ToShortDateString}'"
                        })
        End If
        If newLeave.EndDate <> oldLeave.EndDate Then
            changes.Add(New Data.Entities.UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID,
                       .Description = $"Update leave end date from '{oldLeave.EndDate?.ToShortDateString}' to '{newLeave.EndDate?.ToShortDateString}'"
                       })
        End If
        If newLeave.StartTime.ToString <> oldLeave.StartTime.ToString Then
            changes.Add(New Data.Entities.UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID,
                       .Description = $"Update leave start time from '{oldLeave.StartTime.StripSeconds.ToString}' to '{newLeave.StartTime.StripSeconds.ToString}'"
                       })
        End If
        If newLeave.EndTime.ToString <> oldLeave.EndTime.ToString Then
            changes.Add(New Data.Entities.UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID,
                       .Description = $"Update leave end time from '{oldLeave.EndTime.StripSeconds.ToString}' to '{newLeave.EndTime.StripSeconds.ToString}'"
                       })
        End If
        If newLeave.Reason <> oldLeave.Reason Then
            changes.Add(New Data.Entities.UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID,
                       .Description = $"Update leave reason from '{oldLeave.Reason}' to '{newLeave.Reason}'"
                       })
        End If
        If newLeave.Comments <> oldLeave.Comments Then
            changes.Add(New Data.Entities.UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID,
                       .Description = $"Update leave comments from '{oldLeave.Comments}' to '{newLeave.Comments}'"
                       })
        End If
        If newLeave.LeaveType <> oldLeave.LeaveType Then
            changes.Add(New Data.Entities.UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID,
                       .Description = $"Update leave type from '{oldLeave.LeaveType}' to '{newLeave.LeaveType}'"
                       })
        End If
        If newLeave.Status <> oldLeave.Status Then
            changes.Add(New Data.Entities.UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID,
                       .Description = $"Update leave status from '{oldLeave.Status}' to '{newLeave.Status}'"
                       })
        End If

        If changes.Count > 0 Then
            Dim repo = New Data.Repositories.UserActivityRepository
            repo.CreateRecord(z_User, "Leave", z_OrganizationID, Data.Repositories.UserActivityRepository.RecordTypeEdit, changes)
            Return True
        End If

        Return True
    End Function

    Private Async Function DeleteLeave(currentEmployee As Data.Entities.Employee, messageTitle As String) As Task

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                                            Async Function()
                                                Await _leaveRepository.DeleteAsync(Me._currentLeave.RowID)

                                                Dim repo As New Data.Repositories.UserActivityRepository
                                                repo.RecordDelete(z_User, "Leave", Me._currentLeave.RowID, z_OrganizationID)

                                                Await LoadLeaves(currentEmployee)

                                                ShowBalloonInfo("Successfully Deleted.", messageTitle)

                                            End Function)
    End Function

    Private Sub UpdateEndDateDependingOnStartAndEndTimes()
        If Me._currentLeave Is Nothing Then Return

        If StartTimePicker.Checked AndAlso EndTimePicker.Checked AndAlso
            EndTimePicker.Value.Value.TimeOfDay < StartTimePicker.Value.Value.TimeOfDay Then

            Me._currentLeave.EndDate = Me._currentLeave.StartDate.AddDays(1)
        Else
            Me._currentLeave.EndDate = Me._currentLeave.StartDate

        End If

        EndDatePicker.Value = Me._currentLeave.EndDate
    End Sub

    Private Sub LeaveGridView_SelectionChanged(sender As Object, e As EventArgs) Handles LeaveGridView.SelectionChanged
        ResetForm()

        If LeaveGridView.CurrentRow Is Nothing Then Return

        Dim currentLeave As Leave = GetSelectedLeave()

        Dim currentEmployee = GetSelectedEmployee()
        If currentLeave IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentLeave.EmployeeID, currentEmployee.RowID) Then

            PopulateLeaveForm(currentLeave)
        End If
    End Sub

    Private Async Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

        Dim employee As Data.Entities.Employee = GetSelectedEmployee()

        If employee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")

            Return
        End If

        Dim form As New AddLeaveForm(employee)
        form.ShowDialog()

        If form.IsSaved Then

            Await LoadLeaves(employee)

            If form.ShowBalloonSuccess Then
                ShowBalloonInfo("Leave Successfully Added", "Saved")
            End If
        End If

    End Sub

    Private Sub LeaveListBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) Handles LeaveListBindingSource.CurrentItemChanged

        Dim currentRow = LeaveGridView.CurrentRow

        If currentRow Is Nothing Then Return

        If Me._currentLeave Is Nothing Then Return

        Dim hasChanged = CheckIfLeaveIsChanged(Me._currentLeave)

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

        If Me._currentLeaves Is Nothing Then
            MessageBoxHelper.Warning("No changed leaves!")
            Return
        End If

        Me._currentLeaves = Me._changedLeaves.CloneListJson()

        'cloning TimeSpans objects that are Nothing(null) results to default(T) which is {00:00:00}
        For index As Integer = 0 To Me._currentLeaves.Count - 1
            Me._currentLeaves(index).StartTime = Me._changedLeaves(index).StartTime
            Me._currentLeaves(index).EndTime = Me._changedLeaves(index).EndTime
        Next

        LeaveListBindingSource.DataSource = Me._currentLeaves

        LeaveGridView.DataSource = LeaveListBindingSource
    End Sub

    Private Async Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        Const messageTitle As String = "Delete Leave"

        If Me._currentLeave Is Nothing OrElse
            Me._currentLeave.RowID Is Nothing Then
            MessageBoxHelper.Warning("No leave selected!")

            Return
        End If

        Dim currentLeave = Await _leaveRepository.GetByIdAsync(Me._currentLeave.RowID)

        If currentLeave Is Nothing Then

            MessageBoxHelper.Warning("Leave not found in database! Please close this form then open again.")

            Return
        End If

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete this?", "Confirm Deletion") = False Then

            Return
        End If

        'TODO: check if this is used in a closed payroll. If it is, prevent this from being deleted.
        Await DeleteLeave(currentEmployee, messageTitle)

    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        ForceGridViewCommit()

        Dim changedLeaves As New List(Of Leave)

        Dim messageTitle = "Update Leaves"

        For Each item In Me._currentLeaves
            If CheckIfLeaveIsChanged(item) Then

                Dim validationErrorMessage = item.Validate()
                If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then
                    MessageBoxHelper.ErrorMessage(validationErrorMessage)
                    Return
                End If

                changedLeaves.Add(item)
            End If
        Next

        If changedLeaves.Count < 1 Then

            MessageBoxHelper.Warning("No changed leaves!", messageTitle)
            Return

        ElseIf changedLeaves.Count > 1 AndAlso MessageBoxHelper.Confirm(Of Boolean) _
            ($"You are about to update multiple leaves. Do you want to proceed?", "Confirm Multiple Updates") = False Then

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                                        Async Function()
                                            Await _leaveRepository.SaveManyAsync(changedLeaves)

                                            For Each item In changedLeaves
                                                RecordUpdate(item)
                                            Next

                                            ShowBalloonInfo($"{changedLeaves.Count} Leave(s) Successfully Updated.", messageTitle)

                                            Dim currentEmployee = GetSelectedEmployee()

                                            If currentEmployee IsNot Nothing Then

                                                Await LoadLeaves(currentEmployee)

                                            End If

                                        End Function)
    End Sub

    Private Sub TimePicker_Leave(sender As Object, e As EventArgs) _
        Handles StartTimePicker.Leave, EndTimePicker.Leave, StartDatePicker.Leave

        UpdateEndDateDependingOnStartAndEndTimes()

    End Sub

    Private Sub UserActivityToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityToolStripButton.Click
        Dim userActivity As New UserActivityForm("Leave")
        userActivity.ShowDialog()
    End Sub

End Class