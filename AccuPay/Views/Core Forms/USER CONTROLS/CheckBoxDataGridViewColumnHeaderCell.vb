Public Class CheckBoxDataGridViewColumnHeaderCell
    Inherits DataGridViewColumnHeaderCell
    Dim l As New Point(6, 5)
    Dim s As New Size(12, 12)

    Private _isChecked As Boolean

    Public Property isChecked() As Boolean
        Get
            Return _isChecked
        End Get
        Set(value As Boolean)
            _isChecked = value
            DataGridView.EndEdit()
            For x As Integer = 0 To DataGridView.Rows.Count - 1
                If DataGridView.Rows(x).IsNewRow Then Continue For
                DataGridView.Rows(x).Cells(OwningColumn.Index).Value = value
            Next
        End Set
    End Property

    Protected Overrides Sub Paint(
          graphics As Graphics,
          clipBounds As Rectangle,
          cellBounds As Rectangle,
          rowIndex As Integer,
          dataGridViewElementState As DataGridViewElementStates,
          value As Object,
          formattedValue As Object,
          errorText As String,
          cellStyle As DataGridViewCellStyle,
          advancedBorderStyle As DataGridViewAdvancedBorderStyle,
          paintParts As DataGridViewPaintParts)
        Dim spaces As New String(" "c, 6)
        MyBase.Paint(
                graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState,
                spaces & value.ToString.Trim, spaces & formattedValue.ToString.Trim, errorText, cellStyle, advancedBorderStyle, paintParts)
        CheckBoxRenderer.DrawCheckBox(graphics, New Point(cellBounds.X + 6, 5), If(isChecked, VisualStyles.CheckBoxState.CheckedNormal, VisualStyles.CheckBoxState.UncheckedNormal))
    End Sub

    Protected Overrides Sub OnMouseClick(e As DataGridViewCellMouseEventArgs)
        If New Rectangle(l, s).Contains(e.Location) Then
            isChecked = Not isChecked
        End If
        MyBase.OnMouseClick(e)
    End Sub

End Class
