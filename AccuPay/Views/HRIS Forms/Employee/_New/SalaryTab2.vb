Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Utilities
Imports AccuPay.Views.Employees

Public Class SalaryTab2

    Public Event Init()

    Public Event SelectEmployee(employee As Employee)

    Public Event NewSalary()

    Public Event SaveSalary()

    Public Event DeleteSalary()

    Public Event DeleteSss()

    Public Event SelectSalary(salary As Salary)

    Public Event SalaryChanged(amount As Decimal)

    Public Event CancelChanges()

    Private _mode As Mode = Mode.Empty

    Public ReadOnly Property CurrentMode As Mode
        Get
            Return _mode
        End Get
    End Property

    Public ReadOnly Property EffectiveFrom As Date
        Get
            Return dtpEffectiveFrom.Value
        End Get
    End Property

    Public Property BasicSalary As Decimal
        Get
            Return ObjectUtils.ToDecimal(txtAmount.Text)
        End Get
        Set(value As Decimal)
            txtAmount.Text = CStr(value)
        End Set
    End Property

    Public Property AllowanceSalary As Decimal
        Get
            Return ObjectUtils.ToDecimal(txtAllowance.Text)
        End Get
        Set(value As Decimal)
            txtAllowance.Text = CStr(value)
        End Set
    End Property

    Public Property BasicPay As Decimal
        Get
            Return ObjectUtils.ToDecimal(txtBasicPay.Text)
        End Get
        Set(value As Decimal)
            txtBasicPay.Text = CStr(value)
        End Set
    End Property

    Public Property Sss As Decimal?
        Get
            Return ObjectUtils.ToNullableDecimal(txtSss.Text)
        End Get
        Set(value As Decimal?)
            txtSss.Text = If(value.HasValue, CStr(value), String.Empty)
        End Set
    End Property

    Public Property PhilHealth As Decimal?
        Get
            Return ObjectUtils.ToNullableDecimal(txtPhilHealth.Text)
        End Get
        Set(value As Decimal?)
            txtPhilHealth.Text = If(value.HasValue, CStr(value), String.Empty)
        End Set
    End Property

    Public Property PagIBIG As Decimal
        Get
            Return ObjectUtils.ToDecimal(txtPagIbig.Text)
        End Get
        Set(value As Decimal)
            txtPagIbig.Text = CStr(value)
        End Set
    End Property

    Public Sub New()
        Dim presenter = New SalaryPresenter(Me)
        InitializeComponent()
        DataGridView1.AutoGenerateColumns = False
    End Sub

    Private Sub SalaryTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If DesignMode Then
            Return
        End If

        RaiseEvent Init()
    End Sub

    Public Sub SetEmployee(employee As Employee)
        If _mode = Mode.Creating Then
            EnableSalarySelection()
        End If

        RaiseEvent SelectEmployee(employee)
    End Sub

    Public Sub ShowEmployee(employee As Employee)
        txtPayFrequency.Text = employee.PayFrequency?.Type
        txtSalaryType.Text = employee.EmployeeType
        txtFullname.Text = $"{employee.FirstName} {employee.LastName}"
        txtEmployeeID.Text = $"ID# {employee.EmployeeNo}, {employee?.Position?.Name}, {employee.EmployeeType} Salary"
        pbEmployee.Image = ConvByteToImage(employee.Image)
    End Sub

    Public Sub ChangeMode(mode As Mode)
        _mode = mode

        Select Case _mode
            Case Mode.Disabled
                btnNew.Enabled = False
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case Mode.Empty
                btnNew.Enabled = True
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case Mode.Creating
                btnNew.Enabled = False
                btnSave.Enabled = True
                btnDelete.Enabled = False
                btnCancel.Enabled = True
            Case Mode.Editing
                btnNew.Enabled = True
                btnSave.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
        End Select
    End Sub

    Public Sub DisplaySalaries(salaries As IList(Of Salary))
        RemoveHandler DataGridView1.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
        DataGridView1.DataSource = salaries
        DataGridView1.DataSource = salaries
        AddHandler DataGridView1.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
    End Sub

    Public Sub ActivateSelectedSalary(selectedSalary As Salary)
        If selectedSalary Is Nothing Then
            Return
        End If

        For Each row As DataGridViewRow In DataGridView1.Rows
            Dim comparedSalary = DirectCast(row.DataBoundItem, Salary)

            If selectedSalary.RowID = comparedSalary.RowID Then
                RemoveHandler DataGridView1.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
                DataGridView1.CurrentCell = row.Cells(0)
                row.Selected = True
                AddHandler DataGridView1.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
                Exit For
            End If
        Next
    End Sub

    Private Sub ClearForm()
        dtpEffectiveFrom.Value = Date.Today
        txtAmount.Text = String.Empty
        txtAllowance.Text = String.Empty
        txtTotalSalary.Text = String.Empty
        txtBasicPay.Text = String.Empty
        txtSss.Text = String.Empty
        txtPhilHealth.Text = String.Empty
        txtPagIbig.Text = String.Empty
    End Sub

    Public Sub DisplaySalary(salary As Salary)
        RemoveHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
        dtpEffectiveFrom.Value = salary.EffectiveFrom

        txtAmount.Text = CStr(salary.BasicSalary)
        txtAllowance.Text = CStr(salary.AllowanceSalary)
        txtTotalSalary.Text = CStr(salary.TotalSalary)
        txtPhilHealth.Text = CStr(salary.PhilHealthDeduction)
        txtPagIbig.Text = CStr(salary.HDMFAmount)
        AddHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        RaiseEvent NewSalary()
    End Sub

    Public Sub DisableSalarySelection()
        RemoveHandler DataGridView1.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
        DataGridView1.ClearSelection()
        DataGridView1.CurrentCell = Nothing
    End Sub

    Public Sub EnableSalarySelection()
        AddHandler DataGridView1.SelectionChanged, AddressOf dgvSalaries_SelectionChanged

        If DataGridView1.Rows.Count > 0 Then
            DataGridView1.Item(0, 0).Selected = True
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Me.Focus()

        RaiseEvent SaveSalary()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim result = MsgBox("Are you sure you want to delete this salary?", MsgBoxStyle.YesNo, "Delete Salary")

        If result = MsgBoxResult.Yes Then
            RaiseEvent DeleteSalary()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        RaiseEvent CancelChanges()
    End Sub

    Private Sub dgvSalaries_SelectionChanged(sender As Object, e As EventArgs)
        Dim salary = DirectCast(DataGridView1.CurrentRow?.DataBoundItem, Salary)

        If salary Is Nothing Then
            Return
        End If

        RaiseEvent SelectSalary(salary)
    End Sub

    Private Sub txtAmount_TextChanged(sender As Object, e As EventArgs)
        Dim amount = ObjectUtils.ToDecimal(txtAmount.Text)
        RaiseEvent SalaryChanged(amount)
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Using dialog = New ImportSalaryForm()
            dialog.ShowDialog()
        End Using
    End Sub

    Private Sub TxtSss_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSss.KeyDown
        e.Handled = True

        Select Case e.KeyCode
            Case Keys.Back, Keys.D0, Keys.NumPad0
                RaiseEvent DeleteSss()
        End Select
    End Sub

    Public Enum Mode
        Disabled
        Empty
        Creating
        Editing
    End Enum

End Class