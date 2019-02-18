Option Strict On

Imports AccuPay.Repository
Imports Simplified = AccuPay.SimplifiedEntities.GridView

Public Class EmployeeLoansForm

    Private _employeeRepository As EmployeeRepository

    Private _employees As List(Of Simplified.Employee)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _employeeRepository = New EmployeeRepository
        _employees = New List(Of Simplified.Employee)
    End Sub

    Private Async Sub EmployeeLoansForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        employeesDataGridView.AutoGenerateColumns = False

        Dim list = Await _employeeRepository.GetAll(Of Simplified.Employee)()

        Me._employees = CType(list, List(Of Simplified.Employee))

        employeesDataGridView.DataSource = Me._employees
    End Sub

    Private Async Sub searchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
        Dim searchValue = searchTextBox.Text.ToLower()

        Dim filteredEmployees As New List(Of Simplified.Employee)

        If String.IsNullOrEmpty(searchValue) Then
            employeesDataGridView.DataSource = Me._employees
        Else
            employeesDataGridView.DataSource = Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class