Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Enums
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection

Public Class AddBranchForm

    Private _calendarRepository As CalendarRepository

    Private _employeeRepository As EmployeeRepository

    Private _branches As IEnumerable(Of Branch)

    Private _calendars As IEnumerable(Of PayCalendar)

    Private _currentBranch As Branch

    Private _currentFormType As FormMode

    Private _payrateCalculationBasis As PayRateCalculationBasis
    Public Property HasChanges As Boolean

    Public Property LastAddedBranchId As Integer?

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _calendarRepository = New CalendarRepository()

        _employeeRepository = New EmployeeRepository()
    End Sub

    Private Async Sub AddBranchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.HasChanges = False

        Me.LastAddedBranchId = Nothing

        ShowCalendar()

        Await RefreshForm()

    End Sub

    Private Sub ShowCalendar()

        Dim settings = ListOfValueCollection.Create()

        _payrateCalculationBasis = settings.GetEnum("Pay rate.CalculationBasis",
                                            PayRateCalculationBasis.Organization)

        If _payrateCalculationBasis <> PayRateCalculationBasis.Branch Then

            CalendarPanel.Visible = False

        End If

    End Sub

    Private Async Function RefreshForm() As Task

        Dim _branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)

        _branches = Await _branchRepository.GetAllAsync()

        _calendars = Await _calendarRepository.GetAllAsync()

        NameTextBox.Clear()
        DetailsGroupBox.Enabled = False

        BranchGridView.AutoGenerateColumns = False
        BranchGridView.DataSource = _branches

        CalendarComboBox.DataSource = _calendars

        _currentFormType = FormMode.Empty
    End Function

    Private Function GetSelectedBranch() As Branch

        If BranchGridView.CurrentRow Is Nothing Then

            Return Nothing

        End If

        If CheckIfGridViewHasValue(BranchGridView) = False Then Return Nothing

        Return CType(BranchGridView.CurrentRow?.DataBoundItem, Branch)

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

        If _payrateCalculationBasis = PayRateCalculationBasis.Branch AndAlso
                CalendarComboBox.SelectedItem Is Nothing Then

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

        _currentBranch = branch

        DetailsGroupBox.Text = "Edit Branch"
        DetailsGroupBox.Enabled = True

        NameTextBox.Text = _currentBranch.Name

        If _currentBranch.CalendarID Is Nothing Then
            CalendarComboBox.SelectedIndex = -1
        Else
            CalendarComboBox.SelectedValue = _currentBranch.CalendarID

        End If

        _currentFormType = FormMode.Editing

        NameTextBox.Focus()

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

                    Dim _branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)
                    Await _branchRepository.DeleteAsync(branch)

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

        Dim _branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)

        Dim branch As New Branch
        If _currentFormType = FormMode.Creating Then

            branch.CreatedBy = z_User
            branch.Code = NameTextBox.Text
            branch.Name = NameTextBox.Text
            branch.CalendarID = calendar?.RowID

            branch.RowID = Await _branchRepository.CreateAsync(branch)

        ElseIf _currentFormType = FormMode.Editing Then

            branch = _currentBranch.CloneJson()
            branch.Code = branchName
            branch.Name = branchName
            branch.CalendarID = calendar?.RowID
            branch.LastUpdBy = z_User

            Await _branchRepository.UpdateAsync(branch)

        End If

        Return branch.RowID

    End Function

End Class