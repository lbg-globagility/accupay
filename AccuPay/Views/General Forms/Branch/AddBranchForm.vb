Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Repositories
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class AddBranchForm

    Private _branches As IEnumerable(Of Branch)

    Private _calendars As IEnumerable(Of PayCalendar)

    Private _currentBranch As Branch

    Private _currentFormType As FormMode
    Public Property HasChanges As Boolean

    Public Property LastAddedBranchId As Integer?

    Private ReadOnly _calendarRepository As CalendarRepository

    Private ReadOnly _employeeRepository As EmployeeRepository

    Sub New()

        InitializeComponent()

        _calendarRepository = MainServiceProvider.GetRequiredService(Of CalendarRepository)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
    End Sub

    Private Async Sub AddBranchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.HasChanges = False

        Me.LastAddedBranchId = Nothing

        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.BRANCH)

        AddButton.Visible = False
        EditButton.Visible = False
        DeleteButton.Visible = False

        If role.Success Then

            If role.RolePermission.Create Then
                AddButton.Visible = True

            End If

            If role.RolePermission.Update Then
                EditButton.Visible = True
            End If

            If role.RolePermission.Delete Then
                DeleteButton.Visible = True

            End If

        End If

        Await RefreshForm()

        PopulateForm()

        AddHandler BranchGrid.SelectionChanged, AddressOf BranchGrid_SelectionChanged

    End Sub

    Private Async Function RefreshForm() As Task

        Dim branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)

        _branches = (Await branchRepository.GetAllAsync()).
            OrderBy(Function(b) b.Name).
            ToList

        _calendars = (Await _calendarRepository.GetAllAsync()).
            OrderBy(Function(c) c.Name).
            ToList

        NameTextBox.Clear()
        DetailsGroupBox.Enabled = False

        BranchGrid.AutoGenerateColumns = False
        BranchGrid.DataSource = _branches

        CalendarComboBox.DataSource = _calendars

        _currentFormType = FormMode.Empty
    End Function

    Private Function GetSelectedBranch() As Branch

        If BranchGrid.CurrentRow Is Nothing Then

            Return Nothing

        End If

        If CheckIfGridViewHasValue(BranchGrid) = False Then Return Nothing

        Return CType(BranchGrid.CurrentRow?.DataBoundItem, Branch)

    End Function

    Private Function CheckIfGridViewHasValue(gridView As DataGridView) As Boolean
        Return gridView.Rows.
            Cast(Of DataGridViewRow).
            Any(Function(r) r.Cells.Cast(Of DataGridViewCell).
                Any(Function(c) c.Value IsNot Nothing))
    End Function

    Private Function GetControlWithError() As Control
        Dim errorControl As Control = Nothing

        If String.IsNullOrWhiteSpace(NameTextBox.Text) Then

            Return NameTextBox

        End If

        If CalendarComboBox.SelectedItem Is Nothing Then

            Return CalendarComboBox

        End If

        Return errorControl
    End Function

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click

        DetailsGroupBox.Text = "New Branch"
        DetailsGroupBox.Enabled = True

        NameTextBox.Clear()
        CalendarComboBox.SelectedIndex = -1

        _currentFormType = FormMode.Creating

        NameTextBox.Focus()

    End Sub

    Private Sub EditButton_Click(sender As Object, e As EventArgs) Handles EditButton.Click

        Dim branch = GetSelectedBranch()

        If branch Is Nothing Then

            MessageBoxHelper.ErrorMessage("No Branch selected.")
            Return

        End If

        _currentFormType = FormMode.Editing

        DetailsGroupBox.Text = "Edit Branch"
        DetailsGroupBox.Enabled = True

        NameTextBox.Focus()

    End Sub

    Private Sub PopulateForm()
        Dim branch = GetSelectedBranch()

        If branch Is Nothing Then Return

        _currentBranch = branch

        NameTextBox.Text = _currentBranch.Name

        If _currentBranch.CalendarID Is Nothing Then
            CalendarComboBox.SelectedIndex = -1
        Else
            CalendarComboBox.SelectedValue = _currentBranch.CalendarID

        End If
    End Sub

    Private Async Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        Dim branch = GetSelectedBranch()

        Dim branchId = branch?.RowID

        If branchId Is Nothing Then

            MessageBoxHelper.ErrorMessage("No branch selected.")
            Return

        End If

        If (Await _employeeRepository.GetByBranchAsync(branchId.Value)).Any() Then

            MessageBoxHelper.ErrorMessage("Branch already has employee therefore cannot be deleted.")
            Return

        End If

        Dim confirmMessage = $"Are you sure you want to delete branch: '{branch.Name}'?"

        If MessageBoxHelper.Confirm(Of Boolean) _
            (confirmMessage, "Delete Branch", messageBoxIcon:=MessageBoxIcon.Warning) = False Then Return

        Await FunctionUtils.TryCatchFunctionAsync("Delete Branch",
            Async Function()

                Dim branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)
                Await branchRepository.DeleteAsync(branch)

                Await RefreshForm()

                Me.HasChanges = True
                Me.LastAddedBranchId = Nothing

                MessageBoxHelper.Information($"Branch: '{branch.Name}' successfully deleted.")
            End Function)

    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        If _currentFormType = FormMode.Empty Then Return

        Dim errorControl As Control = GetControlWithError()

        If errorControl IsNot Nothing Then

            MessageBoxHelper.Warning("Please provide input for the required fields.")
            errorControl.Focus()
            Return

        End If

        If _currentFormType = FormMode.Editing AndAlso _currentBranch?.RowID Is Nothing Then

            MessageBoxHelper.ErrorMessage("There was a problem in updating the branch. Please reopen the form and try again.")
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save branch",
            Async Function()

                Dim branchName = NameTextBox.Text.Trim
                Dim calendar = DirectCast(CalendarComboBox.SelectedItem, PayCalendar)

                Me.LastAddedBranchId = Await SaveBranch(branchName, calendar)
                Dim successMesage = ""

                If Me.LastAddedBranchId IsNot Nothing Then

                    Me.HasChanges = True

                    If _currentFormType = FormMode.Creating Then

                        successMesage = $"Branch: '{branchName}' successfully added."
                    Else

                        Me.LastAddedBranchId = Nothing
                        successMesage = $"Branch: '{branchName}' successfully updated."
                    End If

                End If

                Await RefreshForm()

                MessageBoxHelper.Information(successMesage)

            End Function)

    End Sub

    Private Async Function SaveBranch(branchName As String, calendar As PayCalendar) As Task(Of Integer?)

        Dim branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)

        Dim branch As New Branch
        If _currentFormType = FormMode.Creating Then

            branch.CreatedBy = z_User
            branch.Code = NameTextBox.Text
            branch.Name = NameTextBox.Text
            branch.CalendarID = calendar?.RowID

            branch.RowID = Await branchRepository.CreateAsync(branch)

        ElseIf _currentFormType = FormMode.Editing Then

            branch = _currentBranch.CloneJson()
            branch.Code = branchName
            branch.Name = branchName
            branch.CalendarID = calendar?.RowID
            branch.LastUpdBy = z_User

            Await branchRepository.UpdateAsync(branch)

        End If

        Return branch.RowID

    End Function

    Private Sub BranchGrid_SelectionChanged(sender As Object, e As EventArgs)

        PopulateForm()

    End Sub

End Class
