Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class OfficialBusinessForm

    Private _employees As New List(Of Employee)

    Private _allEmployees As New List(Of Employee)

    Private _currentOfficialBusiness As OfficialBusiness

    Private _currentOfficialBusinesses As New List(Of OfficialBusiness)

    Private _changedOfficialBusinesses As New List(Of OfficialBusiness)

    Private _officialBusinessRepository As New OfficialBusinessRepository

    Private _employeeRepository As New EmployeeRepository

    Private _productRepository As New ProductRepository

    Private _textBoxDelayedAction As New DelayedAction(Of Boolean)

    Private Async Sub OfficialBusinessForm_Load(sender As Object, e As EventArgs) Handles Me.Load

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

    Private Sub OfficialBusinessForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        InfoBalloon(, , FormTitleLabel, , , 1)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click
        Using form = New ImportOBForm()
            form.ShowDialog()

            If form.IsSaved Then
                myBalloon("Official businesses Successfully Imported", "Import Official Businesses", EmployeePictureBox, 100, -20)
                'Refresh list
            End If
        End Using
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, FormTitleLabel, 400)
    End Sub

    Private Sub InitializeComponentSettings()
        OfficialBusinessGridView.AutoGenerateColumns = False
        EmployeesDataGridView.AutoGenerateColumns = False
    End Sub

    Private Sub LoadStatusList()

        StatusComboBox.DataSource = _officialBusinessRepository.GetStatusList()

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

        Me._currentOfficialBusiness = Nothing

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

    Private Async Function LoadOfficialBusinesses(currentEmployee As Employee) As Task
        If currentEmployee Is Nothing Then Return

        Me._currentOfficialBusinesses = (Await _officialBusinessRepository.GetByEmployeeAsync(currentEmployee.RowID)).
                                OrderByDescending(Function(a) a.StartDate).
                                ToList

        Me._changedOfficialBusinesses = Me._currentOfficialBusinesses.CloneListJson()

        OfficialBusinessListBindingSource.DataSource = Me._currentOfficialBusinesses

        OfficialBusinessGridView.DataSource = OfficialBusinessListBindingSource

    End Function

    Private Async Sub EmployeesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles EmployeesDataGridView.SelectionChanged

        ResetForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        EmployeeNameTextBox.Text = currentEmployee.FullNameWithMiddleInitial
        EmployeeNumberTextBox.Text = currentEmployee.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(currentEmployee.Image)

        Await LoadOfficialBusinesses(currentEmployee)

    End Sub

    Private Function GetSelectedOfficialBusiness() As OfficialBusiness
        Return CType(OfficialBusinessGridView.CurrentRow.DataBoundItem, OfficialBusiness)
    End Function

    Private Sub PopulateOfficialBusinessForm(officialBusiness As OfficialBusiness)
        Me._currentOfficialBusiness = officialBusiness

        StartDatePicker.DataBindings.Clear()
        StartDatePicker.DataBindings.Add("Value", Me._currentOfficialBusiness, "StartDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndDatePicker.DataBindings.Clear()
        EndDatePicker.DataBindings.Add("Value", Me._currentOfficialBusiness, "EndDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        StartTimePicker.DataBindings.Clear()
        StartTimePicker.DataBindings.Add("Value", Me._currentOfficialBusiness, "StartTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndTimePicker.DataBindings.Clear()
        EndTimePicker.DataBindings.Add("Value", Me._currentOfficialBusiness, "EndTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        ReasonTextBox.DataBindings.Clear()
        ReasonTextBox.DataBindings.Add("Text", Me._currentOfficialBusiness, "Reason")

        CommentTextBox.DataBindings.Clear()
        CommentTextBox.DataBindings.Add("Text", Me._currentOfficialBusiness, "Comments")

        StatusComboBox.DataBindings.Clear()
        StatusComboBox.DataBindings.Add("Text", Me._currentOfficialBusiness, "Status")

        DetailsTabLayout.Enabled = True

    End Sub

    Private Sub OfficialBusinessGridView_SelectionChanged(sender As Object, e As EventArgs) Handles OfficialBusinessGridView.SelectionChanged
        ResetForm()

        If OfficialBusinessGridView.CurrentRow Is Nothing Then Return

        Dim currentOfficialBusiness As OfficialBusiness = GetSelectedOfficialBusiness()

        Dim currentEmployee = GetSelectedEmployee()
        If currentOfficialBusiness IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentOfficialBusiness.EmployeeID, currentEmployee.RowID) Then

            PopulateOfficialBusinessForm(currentOfficialBusiness)
        End If
    End Sub

    Private Async Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

        Dim employee As Employee = GetSelectedEmployee()

        If employee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")

            Return
        End If

        Dim form As New AddOfficialBusinessForm(employee)
        form.ShowDialog()

        If form.IsSaved Then

            Await LoadOfficialBusinesses(employee)

            If form.ShowBalloonSuccess Then
                ShowBalloonInfo("Official Business Successfully Added", "Saved")
            End If
        End If

    End Sub

End Class