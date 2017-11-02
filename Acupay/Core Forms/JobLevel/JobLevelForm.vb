Imports System.ComponentModel
Imports AccuPay.JobLevel

Public Class JobLevelForm

    Private _context As PayrollContext

    Private _category As JobCategory

    Private WithEvents _jobLevelsSource As BindingSource

    Private Sub JobLevelForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        JobLevelDataGridView.AutoGenerateColumns = False
        _jobLevelsSource = New BindingSource()
        JobLevelDataGridView.DataSource = _jobLevelsSource
    End Sub

    Private Sub NewCategoryButton_Click(sender As Object, e As EventArgs) Handles NewCategoryButton.Click
        _context = New PayrollContext()

        _category = New JobCategory() With {
            .OrganizationID = z_OrganizationID,
            .Created = Date.Now,
            .CreatedBy = z_User
        }

        _context.JobCategories.Add(_category)

        _jobLevelsSource.DataSource = _category.JobLevels
    End Sub

    Private Sub SaveCategoryButton_Click(sender As Object, e As EventArgs) Handles SaveCategoryButton.Click
        JobLevelDataGridView.EndEdit()

        _category.Name = CategoryNameTextBox.Text
        _context.SaveChanges()
    End Sub

    Private Sub OnNewJobLevel(sender As Object, e As AddingNewEventArgs) Handles _jobLevelsSource.AddingNew
        Dim jobLevel = New JobLevel() With {
            .OrganizationID = z_OrganizationID,
            .Created = Date.Now,
            .CreatedBy = z_User
        }

        e.NewObject = jobLevel
    End Sub

End Class
