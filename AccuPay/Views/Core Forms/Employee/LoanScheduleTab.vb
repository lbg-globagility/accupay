Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Loans
Imports Microsoft.EntityFrameworkCore

Public Class LoanScheduleTab

    Dim empNo As Integer = 1
    Dim orgId As Integer = 1

    Private _mode As FormMode = FormMode.Empty

    Private _employee As Employee
    Private _loanschedules As List(Of LoanSchedule)
    Private _productlist As List(Of Product)

    Private _currentLoanschedule As LoanSchedule

    Public Sub New()
        InitializeComponent()
        dgvLoanList.AutoGenerateColumns = False
    End Sub

    Private Sub ChangeMode(mode As FormMode)
        _mode = mode

        Select Case _mode
            Case FormMode.Disabled
                btnNew.Enabled = False
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case FormMode.Empty
                btnNew.Enabled = True
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case FormMode.Creating
                btnNew.Enabled = False
                btnSave.Enabled = True
                btnDelete.Enabled = False
                btnCancel.Enabled = True
            Case FormMode.Editing
                btnNew.Enabled = True
                btnSave.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
        End Select
    End Sub

    Private Sub LoadLoanSched()
        'If _employee Is Nothing Then
        '    Return
        'End If

        'Using context = New PayrollContext()
        '    _loanschedules = (From s In context.LoanSchedules
        '                      Where CBool(s.EmployeeID = _employee.RowID)
        '                      Order By s.DedEffectiveDateFrom Descending).
        '                 ToList()
        'End Using

        Using context = New PayrollContext()
            _loanschedules = (From s In context.LoanSchedules
                              Where CBool(s.EmployeeID = empNo)
                              Order By s.DedEffectiveDateFrom Descending).
                              ToList()
        End Using

        RemoveHandler dgvLoanList.SelectionChanged, AddressOf dgvLoanList_SelectionChanged
        dgvLoanList.DataSource = _loanschedules

        If _currentLoanschedule IsNot Nothing Then
            Dim oldLoanSchedule = _currentLoanschedule
            _currentLoanschedule = Nothing

            For Each row As DataGridViewRow In dgvLoanList.Rows
                Dim loansched = DirectCast(row.DataBoundItem, LoanSchedule)
                If oldLoanSchedule.RowID = loansched.RowID Then
                    _currentLoanschedule = oldLoanSchedule
                    dgvLoanList.CurrentCell = row.Cells(0)
                    row.Selected = True
                    Exit For
                End If
            Next
        End If

        If _currentLoanschedule Is Nothing Then
            SelectLoanSchedule(_loanschedules.FirstOrDefault())
        End If

        AddHandler dgvLoanList.SelectionChanged, AddressOf dgvLoanList_SelectionChanged
    End Sub

    Private Sub SelectLoanSchedule(loansched As LoanSchedule)
        _currentLoanschedule = loansched

        If _currentLoanschedule Is Nothing Then
            ClearForm()
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
            DisplayLoan()
        End If
    End Sub

    Private Sub dgvLoanList_SelectionChanged(sender As Object, e As EventArgs) Handles dgvLoanList.SelectionChanged
        Dim loansched = DirectCast(dgvLoanList.CurrentRow?.DataBoundItem, LoanSchedule)

        If loansched Is Nothing Then
            Return
        End If

        SelectLoanSchedule(loansched)
    End Sub

    'Not use for now
    Public Sub SetEmployee(employee As Employee)
        If _mode = FormMode.Creating Then
            EnableLoanScheduleGrid()
        End If

        _employee = employee
        txtFNameLoan.Text = $"{employee.FirstName} {employee.LastName}"
        txtEmpIDLoan.Text = $"ID# {employee.EmployeeNo}, {employee?.Position.Name}, {employee.EmployeeType} Salary"
        pbEmpPicLoan.Image = ConvByteToImage(employee.Image)

        ChangeMode(FormMode.Empty)
        LoadLoanSched()

    End Sub

    Private Sub DisableLoanScheduleGrid()
        RemoveHandler dgvLoanList.SelectionChanged, AddressOf dgvLoanList_SelectionChanged
        dgvLoanList.ClearSelection()
        dgvLoanList.CurrentCell = Nothing
    End Sub

    Private Sub EnableLoanScheduleGrid()
        AddHandler dgvLoanList.SelectionChanged, AddressOf dgvLoanList_SelectionChanged

        If dgvLoanList.Rows.Count > 0 Then
            dgvLoanList.Item(0, 0).Selected = True
            SelectLoanSchedule(DirectCast(dgvLoanList.CurrentRow.DataBoundItem, LoanSchedule))
        End If
    End Sub

    Private Sub ClearForm()
        datefrom.Value = Date.Today
        dateto.Value = Date.Today
        cboloantype.SelectedValue = String.Empty
        cmbStatus.SelectedValue = String.Empty
        cmbdedsched.SelectedValue = String.Empty
        txtloannumber.Text = String.Empty
        txtbal.Text = String.Empty
        txtnoofpayper.Text = String.Empty
        txtnoofpayperleft.Text = String.Empty
        txtloaninterest.Text = String.Empty
        TextBox6.Text = String.Empty
        txtdedpercent.Text = String.Empty
        txtdedamt.Text = String.Empty
        chkboxChargeToBonus.Checked = False
        rdbpercent.Checked = False

    End Sub

    Private Sub DisplayLoan()

        cboloantype.SelectedItem = _currentLoanschedule.LoanTypeID

        datefrom.Value = _currentLoanschedule.DedEffectiveDateFrom
        'dateto.Value = If(_currentLoanschedule.DedEffectiveDateTo, _currentLoanschedule.DedEffectiveDateFrom.AddYears(1))
        cmbStatus.SelectedItem = _currentLoanschedule.Status
        cmbdedsched.SelectedItem = _currentLoanschedule.DeductionSchedule
        cboloantype.SelectedItem = _currentLoanschedule.LoanName
        txtloannumber.Text = CStr(_currentLoanschedule.LoanNumber)
        txtbal.Text = CStr(_currentLoanschedule.TotalBalanceLeft)
        txtnoofpayper.Text = CStr(_currentLoanschedule.NoOfPayPeriod)
        txtnoofpayperleft.Text = CStr(_currentLoanschedule.LoanPayPeriodLeft)
        txtloanamt.Text = CStr(_currentLoanschedule.TotalLoanAmount)
        txtloaninterest.Text = CStr(_currentLoanschedule.DeductionPercentage)
        TextBox6.Text = CStr(_currentLoanschedule.Comments)
        'txtdedpercent.Text = CStr(_currentLoanschedule.DeductionPercentage)
        txtdedamt.Text = CStr(_currentLoanschedule.DeductionAmount)
        chkboxChargeToBonus.Checked = False
        rdbpercent.Checked = False

    End Sub

    Private Enum FormMode
        Disabled
        Empty
        Creating
        Editing
    End Enum

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Dim latestloan = _loanschedules.
          OrderBy(Function(s) s.DedEffectiveDateTo).
          LastOrDefault()

        _currentLoanschedule = New LoanSchedule() With {
            .OrganizationID = z_OrganizationID,
            .EmployeeID = _employee.RowID,
            .DedEffectiveDateFrom = CDate(datefrom.Value),
            .DedEffectiveDateTo = .DedEffectiveDateFrom.AddYears(1)
        }

        DisableLoanScheduleGrid()
        ChangeMode(FormMode.Creating)
        DisplayLoan()
    End Sub

    Private Sub pbEmpPicLoan_Click(sender As Object, e As EventArgs) Handles pbEmpPicLoan.Click

        cmbStatus.Items.Clear()
        cmbStatus.Items.Add("Complete")
        cmbStatus.Items.Add("In Progress")
        cmbStatus.Items.Add("Cancelled")
        cmbStatus.Items.Add("On Hold")

        cmbdedsched.Items.Clear()
        cmbdedsched.Items.Add("Per pay period")
        cmbdedsched.Items.Add("First half")
        cmbdedsched.Items.Add("End of the month")

        cboloantype.Items.Clear()
        Using context = New PayrollContext()
            _productlist = (From pro In context.Products
                            Where pro.OrganizationID = orgId And pro.CategoryID = 10).ToList()

        End Using

        For Each strval In _productlist
            cboloantype.Items.Add(strval.PartNo)
        Next

        Using context = New PayrollContext()
            Dim listOfValues = (From emp In context.Employees.
                                    Include(Function(emp) emp.Position)
                                Where CBool(emp.RowID = empNo)).
                               FirstOrDefault()

            txtFNameLoan.Text = $"{listOfValues.FirstName} {listOfValues.LastName}"
            txtEmpIDLoan.Text = $"ID# {listOfValues.EmployeeNo}, {listOfValues?.Position.Name}, {listOfValues.EmployeeType} Salary"
            pbEmpPicLoan.Image = ConvByteToImage(listOfValues.Image)

        End Using

        LoadLoanSched()

    End Sub

End Class