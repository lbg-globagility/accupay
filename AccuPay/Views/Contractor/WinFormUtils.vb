Imports Microsoft.Extensions.DependencyInjection
Imports SergeUtils

Module WinFormUtils
    Public Function GetRequiredService(Of T)() As T
        Return MainServiceProvider.GetRequiredService(Of T)
    End Function

    Public Sub ClearDataBindings(ctrl As Control,
        Optional allTextBox As Boolean = True,
        Optional allComboBox As Boolean = True,
        Optional allDateTimePicker As Boolean = True)

        If allTextBox Then
            For Each textBox In ctrl.Controls.OfType(Of Control).OfType(Of TextBox)
                textBox.DataBindings.Clear()
                textBox.Clear()
            Next
        End If

        If allComboBox Then
            For Each comboBox In ctrl.Controls.OfType(Of Control).OfType(Of ComboBox)
                comboBox.DataBindings.Clear()
                comboBox.SelectedIndex = -1
            Next

            For Each comboBox In ctrl.Controls.OfType(Of Control).OfType(Of EasyCompletionComboBox)
                comboBox.DataBindings.Clear()
                comboBox.SelectedIndex = -1
            Next
        End If

        If allDateTimePicker Then
            For Each dateTimePicker In ctrl.Controls.OfType(Of Control).OfType(Of DateTimePicker)
                dateTimePicker.DataBindings.Clear()
                If dateTimePicker.Checked Then dateTimePicker.Value = Date.Now
            Next
        End If

    End Sub

End Module
