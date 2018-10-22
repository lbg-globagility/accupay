Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Views.Employees
Imports PayrollSys

Public Class SalaryTab2

    Public Event Init()

    Public Event SelectEmployee(employee As Employee)

    Public Event NewSalary()

    Public Event SaveSalary()

    Public Event DeleteSalary()

    Public Event SelectSalary(salary As Salary)

    Public Event SalaryChanged(amount As Decimal)

    Public Event CancelChanges()

    Private _mode As Mode = Mode.Empty

    Public ReadOnly Property EffectiveFrom As Date
        Get
            Return dtpEffectiveFrom.Value
        End Get
    End Property

    Public ReadOnly Property EffectiveTo As Date?
        Get
            Return If(dtpEffectiveTo.Checked, dtpEffectiveTo.Value, Nothing)
        End Get
    End Property

    Public Property BasicSalary As Decimal
        Get
            Return TypeTools.ParseDecimal(txtAmount.Text)
        End Get
        Set(value As Decimal)
            txtAmount.Text = CStr(value)
        End Set
    End Property

    Public Property AllowanceSalary As Decimal
        Get
            Return TypeTools.ParseDecimal(txtAllowance.Text)
        End Get
        Set(value As Decimal)
            txtAllowance.Text = CStr(value)
        End Set
    End Property

    Public Property Sss As Decimal?
        Get
            Return TypeTools.ParseDecimalOrNull(txtSss.Text)
        End Get
        Set(value As Decimal?)
            txtSss.Text = If(value.HasValue, CStr(value), String.Empty)
        End Set
    End Property

    Public Property PhilHealth As Decimal?
        Get
            Return TypeTools.ParseDecimalOrNull(txtPhilHealth.Text)
        End Get
        Set(value As Decimal?)
            txtPhilHealth.Text = If(value.HasValue, CStr(value), String.Empty)
        End Set
    End Property

    Public Property PagIBIG As Decimal
        Get
            Return TypeTools.ParseDecimal(txtPagIbig.Text)
        End Get
        Set(value As Decimal)
            txtPagIbig.Text = CStr(value)
        End Set
    End Property

    Public Sub New()
        Dim presenter = New SalaryPresenter(Me)
        InitializeComponent()
        dgvSalaries.AutoGenerateColumns = False
    End Sub

    Private Sub SalaryTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If DesignMode Then
            Return
        End If

        RaiseEvent Init()
    End Sub

    Public Sub SetEmployee(employee As Employee)
        If _mode = Mode.Creating Then
            EnableSalaryGrid()
        End If

        RaiseEvent SelectEmployee(employee)
        ChangeMode(Mode.Empty)
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
        RemoveHandler dgvSalaries.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
        dgvSalaries.DataSource = salaries
        AddHandler dgvSalaries.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
    End Sub

    Public Sub ActivateSelectedSalary(selectedSalary As Salary)
        If selectedSalary Is Nothing Then
            Return
        End If

        For Each row As DataGridViewRow In dgvSalaries.Rows
            Dim comparedSalary = DirectCast(row.DataBoundItem, Salary)

            If selectedSalary.RowID = comparedSalary.RowID Then
                dgvSalaries.CurrentCell = row.Cells(0)
                row.Selected = True
                Exit For
            End If
        Next
    End Sub

    Private Sub ClearForm()
        dtpEffectiveFrom.Value = Date.Today
        dtpEffectiveTo.Checked = False
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

        dtpEffectiveTo.Checked = salary.EffectiveTo.HasValue
        If salary.EffectiveTo.HasValue Then
            dtpEffectiveTo.Value = salary.EffectiveTo.Value
        End If

        txtAmount.Text = CStr(salary.BasicSalary)
        txtAllowance.Text = CStr(salary.AllowanceSalary)
        txtTotalSalary.Text = CStr(salary.TotalSalary)
        txtBasicPay.Text = CStr(salary.BasicPay)
        txtSss.Text = CStr(If(salary.SocialSecurityBracket?.EmployeeContributionAmount, 0))
        txtSss.Tag = salary.SocialSecurityBracket
        txtPhilHealth.Text = CStr(salary.PhilHealthDeduction)
        txtPagIbig.Text = CStr(salary.HDMFAmount)
        AddHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        RaiseEvent NewSalary()
    End Sub

    Public Sub DisableSalarySelection()
        RemoveHandler dgvSalaries.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
        dgvSalaries.ClearSelection()
        dgvSalaries.CurrentCell = Nothing
    End Sub

    Private Sub EnableSalaryGrid()
        AddHandler dgvSalaries.SelectionChanged, AddressOf dgvSalaries_SelectionChanged

        If dgvSalaries.Rows.Count > 0 Then
            dgvSalaries.Item(0, 0).Selected = True
            'SelectSalary2(DirectCast(dgvSalaries.CurrentRow.DataBoundItem, Salary))
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        RaiseEvent SaveSalary()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        Dim result = MsgBox("Are you sure you want to delete this salary?", MsgBoxStyle.YesNo, "Delete Salary")

        If result = MsgBoxResult.Yes Then
            RaiseEvent DeleteSalary()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs)
        RaiseEvent CancelChanges()

        'If _mode = SalaryViewMode.Creating Then
        '    EnableSalaryGrid()
        'End If

        'If _currentSalary Is Nothing Then
        '    ChangeMode(SalaryViewMode.Empty)
        'Else
        '    ChangeMode(SalaryViewMode.Editing)
        'End If
    End Sub

    Private Sub dgvSalaries_SelectionChanged(sender As Object, e As EventArgs)
        Dim salary = DirectCast(dgvSalaries.CurrentRow?.DataBoundItem, Salary)

        If salary Is Nothing Then
            Return
        End If

        RaiseEvent SelectSalary(salary)
    End Sub

    Private Sub txtAmount_TextChanged(sender As Object, e As EventArgs)
        Dim amount = TypeTools.ParseDecimal(txtAmount.Text)
        RaiseEvent SalaryChanged(amount)
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Using dialog = New ImportSalaryForm()
            dialog.ShowDialog()
        End Using
    End Sub

    Private Sub TxtSss_KeyDown(sender As Object, e As KeyEventArgs)
        e.Handled = True

        Select Case e.KeyCode
            Case Keys.Back, Keys.D0, Keys.NumPad0
                txtSss.Tag = Nothing
                txtSss.Text = ""
        End Select
    End Sub

    Public Enum Mode
        Disabled
        Empty
        Creating
        Editing
    End Enum

End Class
