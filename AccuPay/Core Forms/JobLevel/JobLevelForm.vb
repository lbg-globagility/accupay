Imports System.ComponentModel
Imports AccuPay.JobLevels
Imports System.Data.Entity

Public Class JobLevelForm

    Private _context As PayrollContext

    Private _category As JobCategory

    Private WithEvents _jobLevelsSource As BindingSource

    Private Sub JobLevelForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeComponents()
        LoadJobCategories()
    End Sub

    Private Sub InitializeComponents()
        _jobLevelsSource = New BindingSource()
        JobLevelsDataGridView.DataSource = _jobLevelsSource
        JobCategoriesDataGridView.AutoGenerateColumns = False
        JobLevelsDataGridView.AutoGenerateColumns = False
    End Sub

    Private Sub LoadJobCategories()
        Using context = New PayrollContext()
            Dim jobCategories = context.JobCategories.ToList()
            RemoveHandler JobCategoriesDataGridView.SelectionChanged, AddressOf JobCategoriesDataGridView_SelectionChanged
            JobCategoriesDataGridView.DataSource = jobCategories
            SelectJobCategory(_category)
            AddHandler JobCategoriesDataGridView.SelectionChanged, AddressOf JobCategoriesDataGridView_SelectionChanged
        End Using
    End Sub

    Private Sub SelectJobCategory(jobCategory As JobCategory)
        For Each row As DataGridViewRow In JobCategoriesDataGridView.Rows
            Dim currentCategory = DirectCast(row.DataBoundItem, JobCategory)

            If currentCategory.RowID = jobCategory?.RowID Then
                JobCategoriesDataGridView.ClearSelection()
                JobCategoriesDataGridView.Item(1, row.Index).Selected = True
            End If
        Next
    End Sub

    Private Sub LoadJobCategory(jobCategory As JobCategory)
        _category = jobCategory
        CategoryNameTextBox.Text = _category.Name
        _jobLevelsSource.DataSource = _category.JobLevels
    End Sub

    Private Sub NewCategoryButton_Click(sender As Object, e As EventArgs) Handles NewCategoryButton.Click
        Dim category = New JobCategory() With {
            .OrganizationID = z_OrganizationID,
            .Created = Date.Now,
            .CreatedBy = z_User
        }

        _context?.Dispose()
        _context = New PayrollContext()
        _context.JobCategories.Add(category)
        LoadJobCategory(category)
    End Sub

    Private Sub SaveCategoryButton_Click(sender As Object, e As EventArgs) Handles SaveCategoryButton.Click
        JobLevelsDataGridView.EndEdit()
        _category.Name = CategoryNameTextBox.Text
        _context.SaveChanges()
        JobLevelsDataGridView.Refresh()

        LoadJobCategories()
    End Sub

    Private Sub OnNewJobLevel(sender As Object, e As AddingNewEventArgs) Handles _jobLevelsSource.AddingNew
        Dim jobLevel = New JobLevel() With {
            .OrganizationID = z_OrganizationID,
            .Created = Date.Now,
            .CreatedBy = z_User
        }

        e.NewObject = jobLevel
    End Sub

    Private Sub JobCategoriesDataGridView_SelectionChanged(sender As Object, e As EventArgs) 'Handles JobCategoriesDataGridView.SelectionChanged
        Dim jobCategory = DirectCast(JobCategoriesDataGridView.CurrentRow.DataBoundItem, JobCategory)

        _context?.Dispose()
        _context = New PayrollContext()

        Dim category = _context.JobCategories.Find(jobCategory.RowID)
        LoadJobCategory(category)
    End Sub

    Private Sub JobLevelsDataGridView_KeyPress(sender As Object, e As KeyEventArgs) Handles JobLevelsDataGridView.KeyDown
        If Not (e.KeyCode = Keys.Delete) Then
            Return
        End If

        Dim jobLevel = DirectCast(JobLevelsDataGridView.CurrentRow?.DataBoundItem, JobLevel)

        If jobLevel Is Nothing Then
            JobLevelsDataGridView.CurrentCell = Nothing
        End If

        _category.JobLevels.Remove(jobLevel)

        If jobLevel?.RowID IsNot Nothing Then
            _context.Entry(jobLevel).State = EntityState.Deleted
        End If

        ' Make sure to synchronize the binding list to make sure no error happens.
        _jobLevelsSource.CurrencyManager.Refresh()
    End Sub

End Class
