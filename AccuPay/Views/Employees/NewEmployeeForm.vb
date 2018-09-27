Option Strict On

Imports AccuPay.Entity

Public Class NewEmployeeForm

    Public Event Init()
    Public Event EmployeeSelected(employeeId As Integer?)
    Public Event EmployeeRefresh()
    Public Event ActiveChanged()
    Public Event Search()

    Public ReadOnly Property IsActive As Boolean
        Get
            Return ActiveCheckBox.Checked
        End Get
    End Property

    Public ReadOnly Property Term As String
        Get
            Return SearchTextBox.Text
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
        Dim presenter = New NewEmployeePresenter(Me)
    End Sub

    Private Sub NewEmployeeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitComponents()
        RaiseEvent Init()
    End Sub

    Private Sub InitComponents()
        EmployeeDataGridView.AutoGenerateColumns = False
        ActiveCheckBox.Checked = True
    End Sub

    Public Sub SetEmployees(employees As IList(Of Employee))
        EmployeeDataGridView.DataSource = employees
    End Sub

    Private Sub EmployeeDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles EmployeeDataGridView.SelectionChanged
        Dim employee = DirectCast(EmployeeDataGridView.CurrentRow.DataBoundItem, Employee)

        If employee Is Nothing Then
            Return
        End If

        RaiseEvent EmployeeSelected(employee.RowID)
    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs) Handles SearchTextBox.TextChanged
        RaiseEvent Search()
    End Sub

    Private Sub ActiveCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ActiveCheckBox.CheckedChanged
        RaiseEvent ActiveChanged()
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        RaiseEvent EmployeeRefresh()
    End Sub

End Class