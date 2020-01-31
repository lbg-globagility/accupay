Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class EmployeeOvertimeForm

    Private _employees As New List(Of Employee)

    Private _allEmployees As New List(Of Employee)

    Private _currentOvertime As Overtime

    Private _currentOvertimes As New List(Of Overtime)

    Private _changedOvertimes As New List(Of Overtime)

    Private _overtimeRepository As New OvertimeRepository

    Private _employeeRepository As New EmployeeRepository

    Private _productRepository As New ProductRepository

    Private _textBoxDelayedAction As New DelayedAction(Of Boolean)

    Private Async Sub EmployeeOvertimeForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeComponentSettings()

        LoadStatusList()

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

    Private Sub EmployeeOvertimeForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        InfoBalloon(, , FormTitleLabel, , , 1)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click
        Using form = New ImportOvertimeForm()
            form.ShowDialog()

            If form.IsSaved Then
                myBalloon("Overtimes Successfully Imported", "Import Overtimes", EmployeePictureBox, 100, -20)
                'Refresh list
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

        StatusComboBox.DataSource = _overtimeRepository.GetStatusList()

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

        AttachmentPictureBox.Image = Nothing

    End Sub

    Private Async Sub ShowAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowAllCheckBox.CheckedChanged
        Await ShowEmployeeList()
    End Sub

    Private Function GetSelectedEmployee() As Employee
        If EmployeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(EmployeesDataGridView.CurrentRow.DataBoundItem, Employee)
    End Function

    Private Async Function LoadOvertimes(currentEmployee As Employee) As Task
        If currentEmployee Is Nothing Then Return

        Me._currentOvertimes = (Await _overtimeRepository.GetByEmployeeAsync(currentEmployee.RowID)).
                                OrderByDescending(Function(a) a.OTStartDate).
                                ToList

        Me._changedOvertimes = Me._currentOvertimes.CloneListJson()

        OvertimeListBindingSource.DataSource = Me._currentOvertimes

        OvertimeGridView.DataSource = OvertimeListBindingSource

    End Function

    Private Async Sub EmployeesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles EmployeesDataGridView.SelectionChanged

        ResetForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        EmployeeNameTextBox.Text = currentEmployee.FullNameWithMiddleInitial
        EmployeeNumberTextBox.Text = currentEmployee.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(currentEmployee.Image)

        Await LoadOvertimes(currentEmployee)

    End Sub

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
        StartTimePicker.DataBindings.Add("Value", Me._currentOvertime, "OTStartTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndTimePicker.DataBindings.Clear()
        EndTimePicker.DataBindings.Add("Value", Me._currentOvertime, "OTEndTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        ReasonTextBox.DataBindings.Clear()
        ReasonTextBox.DataBindings.Add("Text", Me._currentOvertime, "Reason")

        CommentTextBox.DataBindings.Clear()
        CommentTextBox.DataBindings.Add("Text", Me._currentOvertime, "Comments")

        StatusComboBox.DataBindings.Clear()
        StatusComboBox.DataBindings.Add("Text", Me._currentOvertime, "Status")

        DetailsTabLayout.Enabled = True

    End Sub

    Private Sub OvertimeGridView_SelectionChanged(sender As Object, e As EventArgs) Handles OvertimeGridView.SelectionChanged
        ResetForm()

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

End Class