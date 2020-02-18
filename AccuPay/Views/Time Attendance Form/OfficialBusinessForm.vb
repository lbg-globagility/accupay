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

        'cloning TimeSpans objects that are Nothing(null) results to default(T) which is {00:00:00}
        For index As Integer = 0 To Me._changedOfficialBusinesses.Count - 1
            Me._changedOfficialBusinesses(index).StartTime = Me._currentOfficialBusinesses(index).StartTime
            Me._changedOfficialBusinesses(index).EndTime = Me._currentOfficialBusinesses(index).EndTime
        Next

        OfficialBusinessListBindingSource.DataSource = Me._currentOfficialBusinesses

        OfficialBusinessGridView.DataSource = OfficialBusinessListBindingSource

    End Function

    Private Function CheckIfBothNullorBothHaveValue(object1 As Object, object2 As Object) As Boolean

        Return (object1 Is Nothing AndAlso object2 Is Nothing) OrElse
            (object1 IsNot Nothing AndAlso object2 IsNot Nothing)

    End Function

    Private Function CheckIfOfficialBusinessIsChanged(newOfficialBusiness As OfficialBusiness) As Boolean
        If Me._currentOfficialBusiness Is Nothing Then Return False

        Dim oldOfficialBusiness =
            Me._changedOfficialBusinesses.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newOfficialBusiness.RowID))

        If oldOfficialBusiness Is Nothing Then Return False

        Dim hasChanged = False

        If _
            newOfficialBusiness.StartDate.Date <> oldOfficialBusiness.StartDate.Date OrElse
            newOfficialBusiness.EndDate.Date <> oldOfficialBusiness.EndDate.Date OrElse
            Not CheckIfBothNullorBothHaveValue(newOfficialBusiness.StartTime, oldOfficialBusiness.StartTime) OrElse
            newOfficialBusiness.StartTime.StripSeconds <> oldOfficialBusiness.StartTime.StripSeconds OrElse
            Not CheckIfBothNullorBothHaveValue(newOfficialBusiness.EndTime, oldOfficialBusiness.EndTime) OrElse
            newOfficialBusiness.EndTime.StripSeconds <> oldOfficialBusiness.EndTime.StripSeconds OrElse
            newOfficialBusiness.Reason <> oldOfficialBusiness.Reason OrElse
            newOfficialBusiness.Comments <> oldOfficialBusiness.Comments OrElse
            newOfficialBusiness.Status <> oldOfficialBusiness.Status Then

            hasChanged = True
        End If

        Return hasChanged

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
        StartTimePicker.DataBindings.Add("Value", Me._currentOfficialBusiness, "StartTimeFull", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

        EndTimePicker.DataBindings.Clear()
        EndTimePicker.DataBindings.Add("Value", Me._currentOfficialBusiness, "EndTimeFull", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

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

    Private Sub OfficialBusinessListBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) Handles OfficialBusinessListBindingSource.CurrentItemChanged

        Dim currentRow = OfficialBusinessGridView.CurrentRow

        If currentRow Is Nothing Then Return

        If Me._currentOfficialBusiness Is Nothing Then Return

        Dim hasChanged = CheckIfOfficialBusinessIsChanged(Me._currentOfficialBusiness)

        If hasChanged Then
            currentRow.DefaultCellStyle.BackColor = Color.Yellow
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Red
        Else
            currentRow.DefaultCellStyle.BackColor = Color.White
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Black

        End If

    End Sub

End Class