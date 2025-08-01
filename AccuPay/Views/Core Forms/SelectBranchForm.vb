Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Public Class SelectBranchForm

    Private _branches As List(Of Branch)

    Public Property SelectedBranch As Branch

    Private _branchRepository As IBranchRepository

    Sub New()

        InitializeComponent()

        _branchRepository = MainServiceProvider.GetRequiredService(Of IBranchRepository)

    End Sub

    Private Async Sub SelectBranchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SelectedBranch = Nothing

        Await LoadBranch()

    End Sub

    Private Async Function LoadBranch() As Task

        _branches = (Await _branchRepository.GetAllAsync()).
                        OrderBy(Function(b) b.Name).
                        ToList

        BranchComboBox.DisplayMember = "Name"
        BranchComboBox.DataSource = _branches

    End Function

    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click

        SelectedBranch = DirectCast(BranchComboBox.SelectedValue, Branch)
    End Sub

End Class
