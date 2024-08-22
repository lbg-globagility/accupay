Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class OfficialBusinessForm

    Private _currentOfficialBusiness As OfficialBusiness

    Private _employees As List(Of Employee)

    Private _allEmployees As List(Of Employee)

    Private _currentOfficialBusinesses As List(Of OfficialBusiness)

    Private _changedOfficialBusinesses As List(Of OfficialBusiness)

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Private ReadOnly _textBoxDelayedAction As DelayedAction(Of Boolean)

    Private _currentRolePermission As RolePermission

    Sub New()

        InitializeComponent()

        _employees = New List(Of Employee)

        _allEmployees = New List(Of Employee)

        _currentOfficialBusinesses = New List(Of OfficialBusiness)

        _changedOfficialBusinesses = New List(Of OfficialBusiness)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _textBoxDelayedAction = New DelayedAction(Of Boolean)
    End Sub

    Private Async Sub OfficialBusinessForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeComponentSettings()

        Await CheckRolePermissions()

        LoadStatusList()

        Await LoadEmployees()
        Await ShowEmployeeList()

        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

    End Sub

    Private Async Function CheckRolePermissions() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.OFFICIALBUSINESS)

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

    Private Sub OfficialBusinessForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        myBalloon(, , EmployeePictureBox, , , 1)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Async Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click
        Using form = New ImportOBForm()
            form.ShowDialog()

            If form.IsSaved Then
                Dim currentEmployee = GetSelectedEmployee()

                If currentEmployee IsNot Nothing Then

                    Await LoadOfficialBusinesses(currentEmployee)

                End If
                myBalloon("Official businesses Successfully Imported", "Import Official Businesses", EmployeePictureBox, 100, -20)
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

        Dim repository = MainServiceProvider.GetRequiredService(Of IOfficialBusinessRepository)
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

        Await ShowEmployeeOfficialBusinesses()

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

        'official business grid view
        RemoveHandler OfficialBusinessGridView.SelectionChanged, AddressOf OfficialBusinessGridView_SelectionChanged

        OfficialBusinessGridView.DataSource = Nothing

        AddHandler OfficialBusinessGridView.SelectionChanged, AddressOf OfficialBusinessGridView_SelectionChanged

        'official business details
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

    Private Async Function LoadOfficialBusinesses(currentEmployee As Employee) As Task
        If currentEmployee?.RowID Is Nothing Then Return

        Dim repository = MainServiceProvider.GetRequiredService(Of IOfficialBusinessRepository)
        Me._currentOfficialBusinesses = (Await repository.GetByEmployeeAsync(currentEmployee.RowID.Value)).
            OrderByDescending(Function(a) a.StartDate).
            ToList

        Me._changedOfficialBusinesses = Me._currentOfficialBusinesses.CloneListJson()

        'cloning TimeSpans objects that are Nothing(null) results to default(T) which is {00:00:00}
        For index As Integer = 0 To Me._changedOfficialBusinesses.Count - 1
            Me._changedOfficialBusinesses(index).StartTime = Me._currentOfficialBusinesses(index).StartTime
            Me._changedOfficialBusinesses(index).EndTime = Me._currentOfficialBusinesses(index).EndTime
        Next

        PopulateOfficialBusinessGridView()

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
            Not CheckIfBothNullorBothHaveValue(newOfficialBusiness.StartDate, oldOfficialBusiness.StartDate) OrElse
           (newOfficialBusiness.StartDate.HasValue AndAlso newOfficialBusiness.StartDate.Value.Date <> oldOfficialBusiness.StartDate.Value.Date) OrElse
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

    Private Async Sub EmployeesDataGridView_SelectionChanged(sender As Object, e As EventArgs)

        Await ShowEmployeeOfficialBusinesses()

    End Sub

    Private Async Function ShowEmployeeOfficialBusinesses() As Task

        ResetForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        EmployeeNameTextBox.Text = currentEmployee.FullNameWithMiddleInitial
        EmployeeNumberTextBox.Text = currentEmployee.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(currentEmployee.Image)

        Await LoadOfficialBusinesses(currentEmployee)

    End Function

    Private Function GetSelectedOfficialBusiness() As OfficialBusiness
        Return CType(OfficialBusinessGridView.CurrentRow.DataBoundItem, OfficialBusiness)
    End Function

    Private Sub PopulateOfficialBusinessForm(officialBusiness As OfficialBusiness)
        Me._currentOfficialBusiness = officialBusiness

        StartDatePicker.DataBindings.Clear()
        StartDatePicker.DataBindings.Add("Value", Me._currentOfficialBusiness, "ProperStartDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndDatePicker.DataBindings.Clear()
        EndDatePicker.DataBindings.Add("Value", Me._currentOfficialBusiness, "ProperEndDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

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

        If _currentRolePermission.Update Then

            DetailsTabLayout.Enabled = True
        End If

    End Sub

    Private Async Function DeleteOfficialBusiness(currentEmployee As Employee, messageTitle As String) As Task

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of IOfficialBusinessDataService)
                Await dataService.DeleteAsync(
                    Me._currentOfficialBusiness.RowID.Value,
                    currentlyLoggedInUserId:=z_User)

                Await LoadOfficialBusinesses(currentEmployee)

                ShowBalloonInfo("Successfully Deleted.", messageTitle)

            End Function)
    End Function

    Private Sub UpdateEndDateDependingOnStartAndEndTimes()
        If Me._currentOfficialBusiness Is Nothing Then Return

        If EndTimePicker.Value.TimeOfDay < StartTimePicker.Value.TimeOfDay Then

            Me._currentOfficialBusiness.EndDate = Me._currentOfficialBusiness.StartDate.Value.AddDays(1)
        Else
            Me._currentOfficialBusiness.EndDate = Me._currentOfficialBusiness.StartDate.Value

        End If

        If Me._currentOfficialBusiness.EndDate.HasValue Then
            EndDatePicker.Value = Me._currentOfficialBusiness.EndDate.Value

        End If

    End Sub

    Private Sub OfficialBusinessGridView_SelectionChanged(sender As Object, e As EventArgs)

        ShowOfficialBusinessDetails()

    End Sub

    Private Sub ShowOfficialBusinessDetails()

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

    Private Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click
        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        If Me._currentOfficialBusinesses Is Nothing Then
            MessageBoxHelper.Warning("No changed official business!")
            Return
        End If

        Me._currentOfficialBusinesses = Me._changedOfficialBusinesses.CloneListJson()

        'cloning TimeSpans objects that are Nothing(null) results to default(T) which is {00:00:00}
        For index As Integer = 0 To Me._currentOfficialBusinesses.Count - 1
            Me._currentOfficialBusinesses(index).StartTime = Me._changedOfficialBusinesses(index).StartTime
            Me._currentOfficialBusinesses(index).EndTime = Me._changedOfficialBusinesses(index).EndTime
        Next

        PopulateOfficialBusinessGridView()
    End Sub

    Private Sub PopulateOfficialBusinessGridView()

        RemoveHandler OfficialBusinessGridView.SelectionChanged, AddressOf OfficialBusinessGridView_SelectionChanged

        OfficialBusinessListBindingSource.DataSource = Me._currentOfficialBusinesses

        OfficialBusinessGridView.DataSource = OfficialBusinessListBindingSource

        ShowOfficialBusinessDetails()

        AddHandler OfficialBusinessGridView.SelectionChanged, AddressOf OfficialBusinessGridView_SelectionChanged
    End Sub

    Private Async Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        Const messageTitle As String = "Delete Official Business"

        If Me._currentOfficialBusiness?.RowID Is Nothing Then
            MessageBoxHelper.Warning("No official business selected!")

            Return
        End If

        Dim repository = MainServiceProvider.GetRequiredService(Of IOfficialBusinessRepository)
        Dim currentOfficialBusiness = Await repository.GetByIdAsync(Me._currentOfficialBusiness.RowID.Value)

        If currentOfficialBusiness Is Nothing Then

            MessageBoxHelper.Warning("Official Business not found in database! Please close this form then open again.")

            Return
        End If

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete this?", "Confirm Deletion") = False Then

            Return
        End If

        Await DeleteOfficialBusiness(currentEmployee, messageTitle)

    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        ForceGridViewCommit()

        Dim changedOfficialBusinesses As New List(Of OfficialBusiness)

        Dim messageTitle = "Update Official Business"

        For Each item In Me._currentOfficialBusinesses

            If CheckIfOfficialBusinessIsChanged(item) Then
                changedOfficialBusinesses.Add(item)
            End If
        Next

        If changedOfficialBusinesses.Count < 1 Then

            MessageBoxHelper.Warning("No changed official business!", messageTitle)
            Return

        ElseIf changedOfficialBusinesses.Count > 1 AndAlso MessageBoxHelper.Confirm(Of Boolean) _
            ($"You are about to update multiple official businesses. Do you want to proceed?", "Confirm Multiple Updates") = False Then

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of IOfficialBusinessDataService)
                Await dataService.SaveManyAsync(changedOfficialBusinesses, z_User)

                ShowBalloonInfo($"{changedOfficialBusinesses.Count} OfficialBusiness(es) Successfully Updated.", messageTitle)

                Dim currentEmployee = GetSelectedEmployee()

                If currentEmployee IsNot Nothing Then

                    Await LoadOfficialBusinesses(currentEmployee)

                End If

            End Function)
    End Sub

    Private Sub TimePicker_Leave(sender As Object, e As EventArgs) _
        Handles StartTimePicker.Leave, EndTimePicker.Leave, StartDatePicker.Leave

        UpdateEndDateDependingOnStartAndEndTimes()

    End Sub

    Private Sub UserActivityToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityToolStripButton.Click

        Dim formEntityName As String = "Official Business"

        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Sub Approval_Click(sender As Object, e As EventArgs) Handles Approval.Click
        Dim form As New OfficialBusinessApprovalForm
        form.ShowDialog()
    End Sub
End Class
