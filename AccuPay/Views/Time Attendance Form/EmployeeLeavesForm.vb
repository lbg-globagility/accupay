Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection

Public Class EmployeeLeavesForm

    Private Const FormEntityName As String = "Leave"

    Private _currentLeave As Leave

    Private _employees As List(Of Employee)

    Private _allEmployees As List(Of Employee)

    Private _currentLeaves As List(Of Leave)

    Private _changedLeaves As List(Of Leave)

    Private _employeeRepository As EmployeeRepository

    Private _productRepository As ProductRepository

    Private _userActivityRepository As UserActivityRepository

    Private _textBoxDelayedAction As DelayedAction(Of Boolean)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _employees = New List(Of Employee)

        _allEmployees = New List(Of Employee)

        _currentLeaves = New List(Of Leave)

        _changedLeaves = New List(Of Leave)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

        _textBoxDelayedAction = New DelayedAction(Of Boolean)

    End Sub

    Private Async Sub EmployeeLeavesForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeComponentSettings()

        LoadStatusList()
        Await LoadLeaveTypes()

        Await LoadEmployees()
        Await ShowEmployeeList()

        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs)

        _textBoxDelayedAction.ProcessAsync(Async Function()
                                               Await FilterEmployees(SearchTextBox.Text.ToLower())

                                               Return True
                                           End Function)
    End Sub

    Private Sub EmployeeLeavesForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        myBalloon(, , EmployeePictureBox, , , 1)
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

        Dim leaveRepository = MainServiceProvider.GetRequiredService(Of LeaveRepository)
        StatusComboBox.DataSource = leaveRepository.GetStatusList()

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

        Await ShowEmployeeLeaves()

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

        'leave grid view
        RemoveHandler LeaveGridView.SelectionChanged, AddressOf LeaveGridView_SelectionChanged

        LeaveGridView.DataSource = Nothing

        AddHandler LeaveGridView.SelectionChanged, AddressOf LeaveGridView_SelectionChanged

        'leave details
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

    Private Async Function LoadLeaves(currentEmployee As Employee) As Task
        If currentEmployee?.RowID Is Nothing Then Return

        Dim leaveRepository = MainServiceProvider.GetRequiredService(Of LeaveRepository)
        Me._currentLeaves = (Await leaveRepository.GetByEmployeeAsync(currentEmployee.RowID.Value)).
                                OrderByDescending(Function(a) a.StartDate).
                                ToList

        Me._changedLeaves = Me._currentLeaves.CloneListJson()

        'cloning TimeSpans objects that are Nothing(null) results to default(T) which is {00:00:00}
        For index As Integer = 0 To Me._changedLeaves.Count - 1
            Me._changedLeaves(index).StartTime = Me._currentLeaves(index).StartTime
            Me._changedLeaves(index).EndTime = Me._currentLeaves(index).EndTime
        Next

        PopulateLeaveGridView()

    End Function

    Private Async Sub EmployeesDataGridView_SelectionChanged(sender As Object, e As EventArgs)

        Await ShowEmployeeLeaves()

    End Sub

    Private Async Function ShowEmployeeLeaves() As Task

        ResetForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee?.RowID Is Nothing Then Return

        EmployeeNameTextBox.Text = currentEmployee.FullNameWithMiddleInitial
        EmployeeNumberTextBox.Text = currentEmployee.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(currentEmployee.Image)

        VacationLeaveAllowanceTextBox.Text = currentEmployee.VacationLeaveAllowance.ToString()
        SickLeaveAllowanceTextBox.Text = currentEmployee.SickLeaveAllowance.ToString()

        VacationLeaveBalanceTextBox.Text = (Await _employeeRepository.
                                            GetVacationLeaveBalance(currentEmployee.RowID.Value)).
                                            ToString()
        SickLeaveBalanceTextBox.Text = (Await _employeeRepository.
                                            GetSickLeaveBalance(currentEmployee.RowID.Value)).
                                            ToString()

        Await LoadLeaves(currentEmployee)
    End Function

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

        Dim leaveList = New List(Of Product)(Await _productRepository.GetLeaveTypesAsync(z_OrganizationID))

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

    Private Function RecordUpdate(newLeave As Leave) As Boolean

        Dim oldLeave =
            Me._changedLeaves.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newLeave.RowID))

        If oldLeave Is Nothing Then Return False

        Dim changes = New List(Of UserActivityItem)

        Dim entityName = FormEntityName.ToLower()

        If newLeave.StartDate <> oldLeave.StartDate Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = oldLeave.RowID.Value,
                        .Description = $"Updated {entityName} start date from '{oldLeave.StartDate.ToShortDateString}' to '{newLeave.StartDate.ToShortDateString}'."
                        })
        End If
        If newLeave.EndDate <> oldLeave.EndDate Then
            changes.Add(New UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID.Value,
                       .Description = $"Updated {entityName} end date from '{oldLeave.EndDate?.ToShortDateString}' to '{newLeave.EndDate?.ToShortDateString}'."
                       })
        End If
        If newLeave.StartTime.ToString <> oldLeave.StartTime.ToString Then
            changes.Add(New UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID.Value,
                       .Description = $"Updated {entityName} start time from '{oldLeave.StartTime.StripSeconds.ToString}' to '{newLeave.StartTime.StripSeconds.ToString}'."
                       })
        End If
        If newLeave.EndTime.ToString <> oldLeave.EndTime.ToString Then
            changes.Add(New UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID.Value,
                       .Description = $"Updated {entityName} end time from '{oldLeave.EndTime.StripSeconds.ToString}' to '{newLeave.EndTime.StripSeconds.ToString}'."
                       })
        End If
        If newLeave.Reason <> oldLeave.Reason Then
            changes.Add(New UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID.Value,
                       .Description = $"Updated {entityName} reason from '{oldLeave.Reason}' to '{newLeave.Reason}'."
                       })
        End If
        If newLeave.Comments <> oldLeave.Comments Then
            changes.Add(New UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID.Value,
                       .Description = $"Updated {entityName} comments from '{oldLeave.Comments}' to '{newLeave.Comments}'."
                       })
        End If
        If newLeave.LeaveType <> oldLeave.LeaveType Then
            changes.Add(New UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID.Value,
                       .Description = $"Updated {entityName} type from '{oldLeave.LeaveType}' to '{newLeave.LeaveType}'."
                       })
        End If
        If newLeave.Status <> oldLeave.Status Then
            changes.Add(New UserActivityItem() With
                       {
                       .EntityId = oldLeave.RowID.Value,
                       .Description = $"Updated {entityName} status from '{oldLeave.Status}' to '{newLeave.Status}'."
                       })
        End If

        If changes.Count > 0 Then

            _userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)
            Return True
        End If

        Return True
    End Function

    Private Async Function DeleteLeave(currentEmployee As Employee, messageTitle As String) As Task

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                                            Async Function()
                                                Dim leaveRepository = MainServiceProvider.GetRequiredService(Of LeaveRepository)
                                                Await leaveRepository.DeleteAsync(Me._currentLeave.RowID.Value)

                                                _userActivityRepository.RecordDelete(z_User, FormEntityName, Me._currentLeave.RowID.Value, z_OrganizationID)

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

        If Me._currentLeave.EndDate.HasValue Then
            EndDatePicker.Value = Me._currentLeave.EndDate.Value

        End If

    End Sub

    Private Sub LeaveGridView_SelectionChanged(sender As Object, e As EventArgs)

        ShowLeaveDetails()

    End Sub

    Private Sub ShowLeaveDetails()

        If LeaveGridView.CurrentRow Is Nothing Then Return

        Dim currentLeave As Leave = GetSelectedLeave()

        Dim currentEmployee = GetSelectedEmployee()
        If currentLeave IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentLeave.EmployeeID, currentEmployee.RowID) Then

            PopulateLeaveForm(currentLeave)
        End If
    End Sub

    Private Async Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

        Dim employee As Employee = GetSelectedEmployee()

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

        PopulateLeaveGridView()
    End Sub

    Private Sub PopulateLeaveGridView()

        RemoveHandler LeaveGridView.SelectionChanged, AddressOf LeaveGridView_SelectionChanged

        LeaveListBindingSource.DataSource = Me._currentLeaves

        LeaveGridView.DataSource = LeaveListBindingSource

        ShowLeaveDetails()

        AddHandler LeaveGridView.SelectionChanged, AddressOf LeaveGridView_SelectionChanged

    End Sub

    Private Async Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        Const messageTitle As String = "Delete Leave"

        If Me._currentLeave?.RowID Is Nothing Then
            MessageBoxHelper.Warning("No leave selected!")

            Return
        End If

        Dim leaveRepository = MainServiceProvider.GetRequiredService(Of LeaveRepository)
        Dim currentLeave = Await leaveRepository.GetByIdAsync(Me._currentLeave.RowID.Value)

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

                item.LastUpdBy = z_User
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
                                            Dim leaveService = MainServiceProvider.GetRequiredService(Of LeaveService)
                                            Await leaveService.SaveManyAsync(changedLeaves,
                                                                            z_OrganizationID)

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
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Sub LeaveHistoryToolStripButton_Click(sender As Object, e As EventArgs) Handles LeaveHistoryToolStripButton.Click
        Dim dialog = New ViewLeaveLedgerDialog(GetSelectedEmployee())
        dialog.ShowDialog()
    End Sub

End Class