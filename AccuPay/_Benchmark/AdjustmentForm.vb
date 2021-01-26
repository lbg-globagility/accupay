Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AdjustmentForm

    Private ReadOnly _adjustmentType As AdjustmentType

    Private ReadOnly _productRepository As IProductRepository

    Private _adjustments As IEnumerable(Of Product)

    Private _currentAdjustment As Product

    Private _currentFormType As FormMode

    Sub New(Optional adjustmentType As AdjustmentType = AdjustmentType.Blank)

        InitializeComponent()

        _adjustmentType = adjustmentType

        _productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)

    End Sub

    Private Async Sub AdjustmentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Await RefreshForm()

    End Sub

    Private Async Function RefreshForm() As Task
        If _adjustmentType = AdjustmentType.Deduction Then

            _adjustments = Await _productRepository.GetDeductionAdjustmentTypesAsync(z_OrganizationID)

            Me.Text = "Deduction Adjustments"

        ElseIf _adjustmentType = AdjustmentType.OtherIncome Then

            _adjustments = Await _productRepository.GetAdditionAdjustmentTypesAsync(z_OrganizationID)

            Me.Text = "Other Income Adjustments"
        Else

            _adjustments = Await _productRepository.GetAdjustmentTypesAsync(z_OrganizationID)

        End If

        CodeTextBox.Clear()
        DescriptionTextBox.Clear()
        DetailsGroupBox.Enabled = False

        AdjustmentGridView.AutoGenerateColumns = False
        AdjustmentGridView.DataSource = _adjustments

        _currentFormType = FormMode.Empty
    End Function

    Private Function GetSelectedAdjustment() As Product

        If AdjustmentGridView.CurrentRow Is Nothing Then

            Return Nothing

        End If

        If CheckIfGridViewHasValue(AdjustmentGridView) = False Then Return Nothing

        Return CType(AdjustmentGridView.CurrentRow?.DataBoundItem, Product)

    End Function

    Private Function CheckIfGridViewHasValue(gridView As DataGridView) As Boolean
        Return gridView.Rows.
            Cast(Of DataGridViewRow).
            Any(Function(r) r.Cells.Cast(Of DataGridViewCell).
                Any(Function(c) c.Value IsNot Nothing))
    End Function

    Private Function GetTextBoxWithError() As TextBox
        Dim errorTextBox As TextBox = Nothing

        If String.IsNullOrWhiteSpace(CodeTextBox.Text) Then

            errorTextBox = CodeTextBox

        ElseIf String.IsNullOrWhiteSpace(DescriptionTextBox.Text) Then

            errorTextBox = DescriptionTextBox

        End If

        Return errorTextBox
    End Function

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click

        DetailsGroupBox.Text = "New Adjustment"
        DetailsGroupBox.Enabled = True

        CodeTextBox.Clear()
        DescriptionTextBox.Clear()

        _currentFormType = FormMode.Creating

        CodeTextBox.Focus()

    End Sub

    Private Sub EditButton_Click(sender As Object, e As EventArgs) Handles EditButton.Click

        Dim adjustment = GetSelectedAdjustment()

        If adjustment Is Nothing Then

            MessageBoxHelper.ErrorMessage("No adjustment selected.")
            Return

        End If

        _currentAdjustment = adjustment

        DetailsGroupBox.Text = "Edit Adjustment"
        DetailsGroupBox.Enabled = True

        CodeTextBox.Text = _currentAdjustment.Comments
        DescriptionTextBox.Text = _currentAdjustment.PartNo

        _currentFormType = FormMode.Editing

        CodeTextBox.Focus()

    End Sub

    Private Async Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        Dim adjustment = GetSelectedAdjustment()

        Dim adjustmentId = adjustment?.RowID

        If adjustmentId Is Nothing Then

            MessageBoxHelper.ErrorMessage("No adjustment selected.")
            Return

        End If

        Dim confirmMessage = $"Are you sure you want to delete adjustment: '{adjustment.PartNo}'?"

        If MessageBoxHelper.Confirm(Of Boolean) _
               (confirmMessage, "Delete Adjustment", messageBoxIcon:=MessageBoxIcon.Warning) = False Then Return

        Await FunctionUtils.TryCatchFunctionAsync("Delete Adjustment",
            Async Function()

                Await _productRepository.DeleteAsync(adjustmentId.Value)

                Await RefreshForm()
                MessageBoxHelper.Information($"Adjustment: '{adjustment.PartNo}' successfully deleted.")
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

        If _currentFormType = FormMode.Editing AndAlso _currentAdjustment?.RowID Is Nothing Then

            MessageBoxHelper.ErrorMessage("There was a problem in updating the adjustment type. Please reopen the form and try again.")
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save adjustment",
            Async Function()

                Dim successMessage = ""

                Dim adjustmentName = DescriptionTextBox.Text.Trim

                If _currentFormType = FormMode.Creating Then

                    Await _productRepository.AddAdjustmentTypeAsync(
                                    organizationId:=z_OrganizationID,
                                    userId:=z_User,
                                    adjustmentName:=adjustmentName,
                                    comments:=CodeTextBox.Text.Trim,
                                    adjustmentType:=_adjustmentType)

                    successMessage = $"Adjustment: '{adjustmentName}' successfully added."

                ElseIf _currentFormType = FormMode.Editing Then

                    Dim currentAdjustmentId = _currentAdjustment.RowID.Value

                    Await _productRepository.UpdateAdjustmentTypeAsync(
                                                id:=currentAdjustmentId,
                                                userId:=z_User,
                                                adjustmentName:=adjustmentName,
                                                code:=CodeTextBox.Text.Trim)

                    successMessage = $"Adjustment: '{adjustmentName}' successfully updated."
                End If

                Await RefreshForm()

                MessageBoxHelper.Information(successMessage)

            End Function)

    End Sub

End Class
