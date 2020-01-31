Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class EmployeeLeavesForm

    Private _employees As New List(Of Employee)

    Private _allEmployees As New List(Of Employee)

    Private _currentLeave As Leave

    Private _currentLeaves As New List(Of Leave)

    Private _changedLeaves As New List(Of Leave)

    Private _leaveRepository As New LeaveRepository

    Private _employeeRepository As New EmployeeRepository

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
            'Refresh list
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

        Me._allEmployees = (Await _employeeRepository.GetAllWithPositionAsync()).
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

        AttachmentPictureBox.Image = Nothing

    End Sub

    Private Async Sub ShowAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowAllCheckBox.CheckedChanged
        Await ShowEmployeeList()
    End Sub

    Private Function GetSelectedEmployee() As Employee
        If EmployeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(EmployeesDataGridView.CurrentRow.DataBoundItem, Employee)
    End Function

    Private Async Function LoadLeaves(currentEmployee As Employee) As Task
        If currentEmployee Is Nothing Then Return

        Me._currentLeaves = (Await _leaveRepository.GetByEmployeeAsync(currentEmployee.RowID)).
                                OrderByDescending(Function(a) a.StartDate).
                                ToList

        Me._changedLeaves = Me._currentLeaves.CloneListJson()

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
        EndDatePicker.DataBindings.Add("Value", Me._currentLeave, "EndDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        StartTimePicker.DataBindings.Clear()
        StartTimePicker.DataBindings.Add("Value", Me._currentLeave, "StartTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndTimePicker.DataBindings.Clear()
        EndTimePicker.DataBindings.Add("Value", Me._currentLeave, "EndTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

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

End Class