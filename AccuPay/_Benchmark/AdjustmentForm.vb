Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class AdjustmentForm

    Private ReadOnly _adjustmentType As AdjustmentType

    Private _productRepository As ProductRepository

    Private _adjustments As IEnumerable(Of Product)

    Public Enum AdjustmentType

        Deduction
        OtherIncome
        Blank

    End Enum

    Sub New(Optional adjustmentType As AdjustmentType = AdjustmentType.Blank)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _productRepository = New ProductRepository
        _adjustmentType = adjustmentType

    End Sub

    Private Async Sub AdjustmentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Await RefreshForm()

    End Sub

    Private Async Function RefreshForm() As Task
        If _adjustmentType = AdjustmentType.Deduction Then

            _adjustments = Await _productRepository.GetDeductionAdjustmentTypes()

            Me.Text = "Deduction Adjustments"

        ElseIf _adjustmentType = AdjustmentType.OtherIncome Then

            _adjustments = Await _productRepository.GetAdditionAdjustmentTypes()

            Me.Text = "Other Income Adjustments"
        Else

            _adjustments = Await _productRepository.GetAdjustmentTypes()

        End If

        AdjustmentGridView.AutoGenerateColumns = False
        AdjustmentGridView.DataSource = _adjustments
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

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click

        DetailsGroupBox.Text = "New Adjustment"

        CodeTextBox.Clear()
        DescriptionTextBox.Clear()

    End Sub

    Private Sub EditButton_Click(sender As Object, e As EventArgs) Handles EditButton.Click

        Dim adjustment = GetSelectedAdjustment()

        If adjustment Is Nothing Then

            MessageBoxHelper.ErrorMessage("No adjustment selected.")
            Return

        End If

        DetailsGroupBox.Text = "Edit Adjustment"

        CodeTextBox.Text = adjustment.Comments
        DescriptionTextBox.Text = adjustment.PartNo

    End Sub

End Class