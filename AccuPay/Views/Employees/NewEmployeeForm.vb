Option Strict On

Imports AccuPay.Entity

Public Class NewEmployeeForm

    Public Event Init()
    Public Event EmployeeSelected(employeeId As Integer?)
    Public Event Search(term As String)

    Public Sub New()
        InitializeComponent()
        Dim presenter = New NewEmployeePresenter(Me)
    End Sub

    Private Sub NewEmployeeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitComponents()
        RaiseEvent Init()
    End Sub

    Private Sub InitComponents()
        DataGridView1.AutoGenerateColumns = False
    End Sub

    Public Sub SetEmployees(employees As IList(Of Employee))
        DataGridView1.DataSource = employees
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        Dim employee = DirectCast(DataGridView1.CurrentRow.DataBoundItem, Employee)

        If employee Is Nothing Then
            Return
        End If

        RaiseEvent EmployeeSelected(employee.RowID)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        RaiseEvent Search(TextBox1.Text)
    End Sub

End Class