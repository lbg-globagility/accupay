Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories

Public Class SelectBranchForm

    Public Property SelectedBranch As Branch

    Private _branches As List(Of Branch)

    Private ReadOnly _branchRepository As BranchRepository

    Sub New(branchRepository As BranchRepository)

        InitializeComponent()

        _branchRepository = branchRepository
    End Sub

    Private Async Sub SelectBranchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SelectedBranch = Nothing

        Await LoadBranch()

    End Sub

    Private Async Function LoadBranch() As Task

        _branches = (Await _branchRepository.GetAllAsync()).ToList()

        BranchComboBox.DisplayMember = "Name"
        BranchComboBox.DataSource = _branches

    End Function

    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click

        SelectedBranch = DirectCast(BranchComboBox.SelectedValue, Branch)
        Me.Close()
    End Sub

End Class