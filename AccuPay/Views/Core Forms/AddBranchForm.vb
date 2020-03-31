Option Strict On

Imports System.Threading.Tasks
Imports AccuPay
Imports AccuPay.Entity
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore

Public Class AddBranchForm

    Public Enum FormMode
        Disabled
        Empty
        Creating
        Editing
    End Enum

    Private _branches As IEnumerable(Of Branch)

    Private _calendars As IEnumerable(Of PayCalendar)

    Private _currentBranch As Branch

    Private _currentFormType As FormMode

    Private _payrateCalculationBasis As PayRateCalculationBasis
    Public Property HasChanges As Boolean

    Public Property LastAddedBranchId As Integer?

    Private Async Sub AddBranchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.HasChanges = False

        Me.LastAddedBranchId = Nothing

        ShowBranch()

        Await RefreshForm()

    End Sub

    Private Sub ShowBranch()

        Using context As New PayrollContext

            Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

            _payrateCalculationBasis = settings.GetEnum("Pay rate.CalculationBasis",
                                            AccuPay.PayRateCalculationBasis.Organization)

            If _payrateCalculationBasis <> PayRateCalculationBasis.Branch Then

                CalendarPanel.Visible = False

            End If

        End Using

    End Sub

    Private Async Function RefreshForm() As Task

        Using context As New PayrollContext

            _branches = Await context.Branches.ToListAsync

            _calendars = Await context.Calendars.ToListAsync()

        End Using

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

        Dim confirmMessage = $"Are you sure you want to delete branch: '{branch.Name}'?"

        If MessageBoxHelper.Confirm(Of Boolean) _
               (confirmMessage, "Delete Branch", messageBoxIcon:=MessageBoxIcon.Warning) = False Then Return

        Await FunctionUtils.TryCatchFunctionAsync("Delete Branch",
                Async Function()

                    Using context As New PayrollContext

                        Dim existingBranch = context.Branches.FirstOrDefault(Function(b) b.RowID.Value = branchId.Value)

                        If existingBranch IsNot Nothing Then

                            context.Branches.Remove(existingBranch)

                            Await context.SaveChangesAsync

                        End If

                    End Using

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

        Using context As New PayrollContext

            Dim branch As New Branch

            Dim branchNameValidationQuery = context.Branches.
                Where(Function(b) b.Code.Trim.ToUpper = branchName.Trim.ToUpper)

            If _currentFormType = FormMode.Creating Then

                branch.CreatedBy = z_User
                branch.Code = NameTextBox.Text
                branch.Name = NameTextBox.Text
                branch.CalendarID = calendar?.RowID

                context.Branches.Add(branch)

            ElseIf _currentFormType = FormMode.Editing Then

                Dim currentBranchId = _currentBranch.RowID.Value

                branch = Await context.Branches.FirstOrDefaultAsync(Function(b) b.RowID.Value = currentBranchId)

                If branch Is Nothing Then

                    Throw New ArgumentException("Branch has already been deleted. Please reopen the form.")

                End If

                branchNameValidationQuery = branchNameValidationQuery.Where(Function(b) b.RowID.Value <> currentBranchId)

                branch.Code = branchName
                branch.Name = branchName
                branch.CalendarID = calendar?.RowID
                branch.LastUpdBy = z_User

            End If

            If Await branchNameValidationQuery.AnyAsync Then

                Throw New ArgumentException("Branch already exists.")

            End If

            Await context.SaveChangesAsync

            Return branch.RowID

        End Using

    End Function

End Class