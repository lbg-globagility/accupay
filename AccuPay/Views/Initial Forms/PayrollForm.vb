Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class PayrollForm

    Public listPayrollForm As New List(Of String)

    Private if_sysowner_is_benchmark As Boolean

    Private ReadOnly _systemOwnerService As ISystemOwnerService

    Private ReadOnly _policyHelper As IPolicyHelper

    Private ReadOnly _roleRepository As IRoleRepository

    Private ReadOnly _userRepository As IAspNetUserRepository

    Sub New()

        InitializeComponent()

        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of ISystemOwnerService)

        _roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)

        _userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)
    End Sub

    Private Async Sub PayrollForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetProperInterfaceBaseOnCurrentSystemOwner()

        If _policyHelper.UseUserLevel Then

            Await RestrictByUserLevel()
        Else

            Await CheckRolePermissions()
        End If

        If Not Debugger.IsAttached Then
            PaystubExperimentalToolStripMenuItem.Visible = False
            WithholdingTaxToolStripMenuItem.Visible = False
        End If
    End Sub

    Private Async Function RestrictByUserLevel() As Task

        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
        End If

        If Not _policyHelper.UseUserLevel Then Return

        If user.UserLevel = UserLevel.Four Then
            PayrollToolStripMenuItem.Visible = False
            WithholdingTaxToolStripMenuItem.Visible = False
            PaystubExperimentalToolStripMenuItem.Visible = False
            BenchmarkPaystubToolStripMenuItem.Visible = False
        End If
    End Function

    Private Async Function CheckRolePermissions() As Task
        USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)

        Dim allowancePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.ALLOWANCE).FirstOrDefault()
        Dim loanPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.LOAN).FirstOrDefault()
        Dim payPeriodPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.PAYPERIOD).FirstOrDefault()

        LoanToolStripMenuItem.Visible = If(loanPermission?.Read, False)

        'Payroll only overrides the visibility if Read is False
        'since they are already checked by other policies above
        If payPeriodPermission Is Nothing OrElse payPeriodPermission.Read = False Then

            AllowanceToolStripMenuItem.Visible = False
            PayrollToolStripMenuItem.Visible = False
            BenchmarkPaystubToolStripMenuItem.Visible = False
            WithholdingTaxToolStripMenuItem.Visible = False
        End If

    End Function

    Private Sub SetProperInterfaceBaseOnCurrentSystemOwner()

        if_sysowner_is_benchmark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwner.Benchmark

        If if_sysowner_is_benchmark Then

            BenchmarkPaystubToolStripMenuItem.Visible = True

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
