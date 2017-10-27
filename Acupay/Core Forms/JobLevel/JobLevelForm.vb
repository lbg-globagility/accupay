Imports System.ComponentModel
Imports AccuPay.JobLevel

Public Class JobLevelForm

    Private _category As JobCategory

    Private _jobLevelsBinding As BindingSource

    Private Sub JobLevelForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _jobLevelsBinding = New BindingSource()
        JobLevelDataGridView.DataSource = _jobLevelsBinding
    End Sub

    Private Sub NewCategoryButton_Click(sender As Object, e As EventArgs) Handles NewCategoryButton.Click
        _category = New JobCategory()
        _category.JobLevels.Add(New JobLevel() With {.Name = "Test"})

        _jobLevelsBinding.DataSource = _category.JobLevels
    End Sub

    Private Sub SaveCategoryButton_Click(sender As Object, e As EventArgs) Handles SaveCategoryButton.Click
        _category.Name = CategoryNameTextBox.Text

        Using context = New PayrollContext()
            'context.JobCategories.Add(_category)
        End Using
    End Sub

    Private Sub CategoryNameTextBox_KeyUp(sender As Object, e As KeyEventArgs) Handles CategoryNameTextBox.KeyUp
        If e.KeyCode = Keys.Enter Then
            Dim level = New JobLevel() With {
                .Name = CategoryNameTextBox.Text
            }

            _category.JobLevels.Add(level)
            _jobLevelsBinding.ResetBindings(False)
        End If
    End Sub

End Class
