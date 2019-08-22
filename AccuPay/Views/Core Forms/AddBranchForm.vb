Imports System.Threading.Tasks
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

    Private _currentBranch As Branch

    Private _currentFormType As FormMode

    Public Property HasChanges As Boolean

    Public Property LastBranchId As Integer?

    Private Async Sub AddBranchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.HasChanges = False

        Me.LastBranchId = Nothing

        Await RefreshForm()

    End Sub

    Private Async Function RefreshForm() As Task

        Using context As New PayrollContext

            _branches = Await context.Branches.
                            Where(Function(b) b.OrganizationID = z_OrganizationID).
                            ToListAsync

        End Using

        NameTextBox.Clear()
        DetailsGroupBox.Enabled = False

        BranchGridView.AutoGenerateColumns = False
        BranchGridView.DataSource = _branches

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

    Private Function GetTextBoxWithError() As TextBox
        Dim errorTextBox As TextBox = Nothing

        If String.IsNullOrWhiteSpace(NameTextBox.Text) Then

            errorTextBox = NameTextBox

        End If

        Return errorTextBox
    End Function

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click

        DetailsGroupBox.Text = "New Branch"
        DetailsGroupBox.Enabled = True

        NameTextBox.Clear()

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

                        Dim existingBranch = context.Branches.FirstOrDefault(Function(b) b.RowID.Value = branchId)

                        If existingBranch IsNot Nothing Then

                            context.Branches.Remove(existingBranch)

                            Await context.SaveChangesAsync

                        End If

                    End Using

                    Await RefreshForm()

                    Me.HasChanges = True
                    Me.LastBranchId = Nothing

                    MessageBoxHelper.Information($"Branch: '{branch.Name}' successfully deleted.")
                End Function)

    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Dim errorTextBox As TextBox = GetTextBoxWithError()

        If _currentFormType = FormMode.Empty Then Return

        If errorTextBox IsNot Nothing Then

            MessageBoxHelper.Warning("Please provide input for the required fields.")
            errorTextBox.Focus()
            Return

        End If

        If _currentFormType = FormMode.Editing AndAlso _currentBranch?.RowID Is Nothing Then

            MessageBoxHelper.ErrorMessage("There was a problem in updating the branch. Please reopen the form and try again.")
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save branch",
                                Async Function()

                                    Dim branchName = NameTextBox.Text.Trim

                                    Dim successMessage = Await SaveBranch(branchName)

                                    Dim isNotInsert = _currentFormType <> FormMode.Creating

                                    Await RefreshForm()

                                    Using context As New PayrollContext

                                        Dim lastBranch = Await context.Branches.FirstOrDefaultAsync(Function(b) b.Code = branchName AndAlso b.OrganizationID = z_OrganizationID)
                                        Me.LastBranchId = lastBranch?.RowID

                                        If Me.LastBranchId IsNot Nothing Then

                                            Me.HasChanges = True

                                            If isNotInsert Then

                                                Me.LastBranchId = Nothing

                                            End If

                                        End If

                                    End Using

                                    MessageBoxHelper.Information(successMessage)

                                End Function)

    End Sub

    Private Async Function SaveBranch(branchName As String) As Task(Of String)

        Dim successMessage = ""

        Using context As New PayrollContext

            If _currentFormType = FormMode.Creating Then

                Dim branch As New Branch
                branch.OrganizationID = z_OrganizationID
                branch.CreatedBy = z_User
                branch.Code = NameTextBox.Text
                branch.Name = NameTextBox.Text

                context.Branches.Add(branch)

                successMessage = $"Branch: '{branchName}' successfully added."

            ElseIf _currentFormType = FormMode.Editing Then

                Dim currentBranchId = _currentBranch.RowID.Value

                Dim branch = Await context.Branches.FirstOrDefaultAsync(Function(b) b.RowID.Value = currentBranchId)

                If branch Is Nothing Then

                    Throw New ArgumentException("Branch has already been deleted. Please reopen the form.")

                End If

                If Await context.Branches.AnyAsync(Function(b) b.Code = branchName AndAlso b.OrganizationID = z_OrganizationID) Then

                    Throw New ArgumentException("Branch already exists.")

                End If

                branch.Code = branchName
                branch.Name = branchName
                branch.LastUpdBy = z_User

                successMessage = $"Branch: '{branchName}' successfully updated."
            End If

            Await context.SaveChangesAsync

            Return successMessage

        End Using

    End Function

End Class