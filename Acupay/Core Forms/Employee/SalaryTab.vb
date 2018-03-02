Option Strict On

Imports System.Linq
Imports AccuPay.Entity
Imports PayrollSys

Public Class SalaryTab

    Private _mode As FormMode = FormMode.NoneSelected

    Private _context As PayrollContext

    Private _employee As Employee

    Private _salaries As List(Of Salary)

    Private _currentSalary As Salary

    Public Sub SetEmployee(employee As Employee)
        _employee = employee
        txtFullname.Text = employee.LastName
        pbEmployee.Image = ConvByteToImage(employee.Image)
    End Sub

    Private Sub SalaryTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvSalaries.AutoGenerateColumns = False
    End Sub

    Private Sub ChangeMode(mode As FormMode)
        _mode = mode

        If _mode = FormMode.NoneSelected Then
            btnNew.Enabled = True
            btnSave.Enabled = False
            btnCancel.Enabled = False
        ElseIf _mode = FormMode.Creating Then
            btnNew.Enabled = False
            btnSave.Enabled = True
            btnCancel.Enabled = True
        ElseIf _mode = FormMode.Editing Then
            btnNew.Enabled = True
            btnSave.Enabled = True
            btnCancel.Enabled = True
        End If
    End Sub

    Private Sub LoadSalaries()
        _salaries = _context.
            Salaries.Where(Function(s) Nullable.Equals(s.EmployeeID, _employee.RowID)).
            ToList()

        dgvSalaries.DataSource = _salaries
    End Sub

    Private Sub DisplaySalary()
        txtAmount.Text = CStr(_currentSalary.Amount)
        txtAllowance.Text = CStr(_currentSalary.AllowanceAmount)
        txtBasicPay.Text = CStr(_currentSalary.BasicPay)
        txtPagIbig.Text = CStr(_currentSalary.HDMFAmount)
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        _currentSalary = New Salary()

        DisplaySalary()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Throw New NotImplementedException()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Throw New NotImplementedException()
    End Sub

    Private Enum FormMode
        NoneSelected
        Creating
        Editing
    End Enum

End Class
