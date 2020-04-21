Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories

Public Class SelectBranchForm

    Private _branches As List(Of Branch)

    Public Property SelectedBranch As Branch

    Private Async Sub SelectBranchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SelectedBranch = Nothing

        Await LoadBranch()

    End Sub

    Private Async Function LoadBranch() As Task

        _branches = Await New BranchRepository().GetAllAsync()

        BranchComboBox.DisplayMember = "Name"
        BranchComboBox.DataSource = _branches

    End Function

    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click

        SelectedBranch = DirectCast(BranchComboBox.SelectedValue, Branch)
        Me.Close()
    End Sub

End Class