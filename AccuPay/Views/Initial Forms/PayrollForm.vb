Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class PayrollForm

    Public listPayrollForm As New List(Of String)

    Private if_sysowner_is_benchmark As Boolean

    Private ReadOnly _systemOwnerService As SystemOwnerService

    Private ReadOnly _policyHelper As PolicyHelper

    Private ReadOnly _roleRepository As RoleRepository

    Sub New()

        InitializeComponent()

        _policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        _roleRepository = MainServiceProvider.GetRequiredService(Of RoleRepository)
    End Sub

    Private Async Sub PayrollForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetProperInterfaceBaseOnCurrentSystemOwner()

        If Not _policyHelper.UseUserLevel Then

            Await CheckRolePermissions()
        End If

        If Not Debugger.IsAttached Then
            PaystubExperimentalToolStripMenuItem.Visible = False
            WithholdingTaxToolStripMenuItem.Visible = False
        End If
    End Sub

    Private Async Function CheckRolePermissions() As Task
        USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)

        Dim allowancePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.ALLOWANCE).FirstOrDefault()
        Dim loanPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.LOAN).FirstOrDefault()
        Dim payPeriodPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.PAYPERIOD).FirstOrDefault()

        AllowanceToolStripMenuItem.Visible = If(allowancePermission?.Read, False)
        LoanToolStripMenuItem.Visible = If(loanPermission?.Read, False)

        'Payroll only overrides the visibility if Read is False
        'since they are already checked by other policies above
        If payPeriodPermission Is Nothing OrElse payPeriodPermission.Read = False Then

            PayrollToolStripMenuItem.Visible = False
            BenchmarkPaystubToolStripMenuItem.Visible = False
            BonusToolStripMenuItem.Visible = False
            WithholdingTaxToolStripMenuItem.Visible = False
        End If

    End Function

    Private Sub SetProperInterfaceBaseOnCurrentSystemOwner()

        Dim showBonusForm As Boolean =
            (_systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Goldwings)

        ' no AccuPay clients are using bonus and other features are outdated and might be buggy
        ' just like deleting Paystub should also delete it's bonuses
        showBonusForm = False

        BonusToolStripMenuItem.Visible = showBonusForm

        if_sysowner_is_benchmark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark

        If if_sysowner_is_benchmark Then

            BenchmarkPaystubToolStripMenuItem.Visible = True

            BonusToolStripMenuItem.Visible = False
            WithholdingTaxToolStripMenuItem.Visible = False
            PaystubExperimentalToolStripMenuItem.Visible = False
            AllowanceToolStripMenuItem.Visible = False
        Else
            BenchmarkPaystubToolStripMenuItem.Visible = False
        End If

    End Sub

    Private Async Function ChangeForm(ByVal passedForm As Form, Optional permissionName As String = Nothing) As Task

        If permissionName IsNot Nothing Then

            If Await PermissionHelper.DoesAllowReadAsync(permissionName, policyHelper:=_policyHelper) = False Then
                MessageBoxHelper.DefaultUnauthorizedFormMessage()
                Return
            End If

        End If

        ShowForm(passedForm)

    End Function

    Private Sub ShowForm(passedForm As Form)
        Try
            Application.DoEvents()
            Dim FName As String = passedForm.Name
            passedForm.KeyPreview = True
            passedForm.TopLevel = False
            passedForm.Enabled = True
            If listPayrollForm.Contains(FName) Then
            Else
                PanelPayroll.Controls.Add(passedForm)
                listPayrollForm.Add(passedForm.Name)
                passedForm.Refresh()

                passedForm.Dock = DockStyle.Fill
            End If
            passedForm.Show()
            passedForm.BringToFront()
            passedForm.Focus()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

    End Sub

    Private Async Sub PayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PayrollToolStripMenuItem.Click
        If if_sysowner_is_benchmark Then

            Await ChangeForm(BenchmarkPayrollForm, PermissionConstant.PAYPERIOD)
            previousForm = BenchmarkPayrollForm
        Else

            Await ChangeForm(PayStubForm, PermissionConstant.PAYPERIOD)
            previousForm = PayStubForm

        End If

    End Sub

    Private Async Sub BonusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BonusToolStripMenuItem.Click
        Await ChangeForm(BonusGenerator, PermissionConstant.PAYPERIOD)
        previousForm = BonusGenerator

    End Sub

    Private Async Sub WithholdingTaxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WithholdingTaxToolStripMenuItem.Click
        Await ChangeForm(WithholdingTax, PermissionConstant.PAYPERIOD)
        previousForm = WithholdingTax
    End Sub

    Private Async Sub PaystubExperimentalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PaystubExperimentalToolStripMenuItem.Click
        Await ChangeForm(PaystubView, PermissionConstant.PAYPERIOD)
        previousForm = PaystubView
    End Sub

    Private Async Sub AllowanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllowanceToolStripMenuItem.Click
        Await ChangeForm(EmployeeAllowanceForm, PermissionConstant.ALLOWANCE)
        previousForm = EmployeeAllowanceForm
    End Sub

    Private Async Sub LoanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoanToolStripMenuItem.Click
        Await ChangeForm(EmployeeLoansForm, PermissionConstant.LOAN)
        previousForm = EmployeeLoansForm
    End Sub

    Private Async Sub BenchmarkPaystubToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BenchmarkPaystubToolStripMenuItem.Click
        Await ChangeForm(BenchmarkPaystubForm, PermissionConstant.PAYPERIOD)
        previousForm = BenchmarkPaystubForm
    End Sub

    Private Sub PayrollForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        For Each objctrl As Control In PanelPayroll.Controls
            If TypeOf objctrl Is Form Then
                DirectCast(objctrl, Form).Close()

            End If
        Next

    End Sub

    Private Sub PanelPayroll_ControlRemoved(sender As Object, e As ControlEventArgs) Handles PanelPayroll.ControlRemoved
        Dim listOfForms = PanelPayroll.Controls.Cast(Of Form)()
        For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
            'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
            pb.Enabled = True
            Exit For
        Next
    End Sub

    Private Sub PanelPayroll_Paint(sender As Object, e As PaintEventArgs) Handles PanelPayroll.Paint

    End Sub

End Class