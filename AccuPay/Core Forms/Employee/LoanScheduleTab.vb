Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys


Public Class LoanScheduleTab
    Private _mode As FormMode = FormMode.Empty

    Private _employee As Employee
    Private _loanschedules As List(Of LoanSchedule)

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
        If _employee Is Nothing Then
            Return
        End If

        Using context = New PayrollContext()
            _loanschedules = (From s In context.LoanSchedules
                              Where CBool(s.EmployeeID = _employee.RowID)
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

        'SelectLoanSchedule(loansched)
    End Sub

    Public Sub SetEmployee(employee As Employee)
        If _mode = FormMode.Creating Then
            EnableLoanScheduleGrid()
        End If

        _employee = employee
        'txtPayFrequency.Text = employee.PayFrequency.Type
        'txtSalaryType.Text = employee.EmployeeType
        txtFNameLoan.Text = $"{employee.FirstName} {employee.LastName}"
        txtEmpIDLoan.Text = $"ID# {employee.EmployeeNo}, {employee?.Position.Name}, {employee.EmployeeType} Salary"
        pbEmpPicLoan.Image = ConvByteToImage(employee.Image)

        ChangeMode(FormMode.Empty)
        LoadLoanSched()

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
        'RemoveHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
        'dtpEffectiveFrom.Value = _currentLoanschedule.EffectiveFrom

        'dtpEffectiveTo.Value = If(_currentLoanschedule.EffectiveTo, _currentLoanschedule.EffectiveFrom.AddYears(100))
        'txtAmount.Text = CStr(_currentLoanschedule.BasicSalary)
        'txtAllowance.Text = CStr(_currentLoanschedule.AllowanceSalary)
        'txtTotalSalary.Text = CStr(_currentLoanschedule.TotalSalary)
        'txtBasicPay.Text = CStr(_currentLoanschedule.BasicPay)
        'txtSss.Text = CStr(If(_currentLoanschedule.SocialSecurityBracket?.EmployeeContributionAmount, 0))
        'txtSss.Tag = _currentLoanschedule.SocialSecurityBracket
        'txtPhilHealth.Text = CStr(_currentLoanschedule.PhilHealthDeduction)
        'txtPagIbig.Text = CStr(_currentLoanschedule.HDMFAmount)
        'AddHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged


        datefrom.Value = _currentLoanschedule.DedEffectiveDateFrom

        dateto.Value = If(_currentLoanschedule.DedEffectiveDateTo, _currentLoanschedule.DedEffectiveDateFrom.AddYears(100))

        cboloantype.SelectedValue = String.Empty
        cmbStatus.SelectedValue = String.Empty
        cmbdedsched.SelectedValue = String.Empty

        txtloannumber.Text = CStr(_currentLoanschedule.LoanNumber)
        txtbal.Text = CStr(_currentLoanschedule.TotalBalanceLeft)
        txtnoofpayper.Text = CStr(_currentLoanschedule.NoOfPayPeriod)
        txtnoofpayperleft.Text = CStr(_currentLoanschedule.LoanPayPeriodLeft)

        txtloaninterest.Text = CStr(_currentLoanschedule.TotalBalanceLeft)

        TextBox6.Text = CStr(_currentLoanschedule.Comments)

        txtdedpercent.Text = CStr(_currentLoanschedule.TotalBalanceLeft)
        txtdedamt.Text = CStr(_currentLoanschedule.TotalBalanceLeft)
        chkboxChargeToBonus.Checked = False
        rdbpercent.Checked = False

    End Sub

    Private Enum FormMode
        Disabled
        Empty
        Creating
        Editing
    End Enum


End Class
