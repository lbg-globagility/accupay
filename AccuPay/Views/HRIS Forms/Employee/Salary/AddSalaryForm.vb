Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.AccuPay.Desktop.Helpers
Imports AccuPay.Benchmark
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class AddSalaryForm

    Public Property isSaved As Boolean
    Public Property showBalloon As Boolean

    Private _employee As Employee

    Private _isSystemOwnerBenchMark As Boolean

    Private _ecolaAllowance As Allowance

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Private ReadOnly _systemOwnerService As ISystemOwnerService

    Public Sub New(employee As Employee)
        InitializeComponent()

        _employee = employee

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of ISystemOwnerService)
    End Sub

    Private Async Sub AddSalaryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFullName.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)

        txtPayFrequency.Text = _employee.PayFrequency?.Type
        txtSalaryType.Text = _employee.EmployeeType

        _isSystemOwnerBenchMark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwner.Benchmark

        ToggleBenchmarkEcola()

        Await InitializeBenchmarkData()

        ClearForm()
    End Sub

    Private Async Function InitializeBenchmarkData() As Task(Of Boolean)
        If _isSystemOwnerBenchMark Then

            Dim employeeId = _employee?.RowID

            Dim errorMessage = "Cannot retrieve ECOLA data. Please contact Globagility Inc. to fix this."

            Dim currentPayPeriod = Await _payPeriodRepository.GetOrCreateCurrentPayPeriodAsync(
                organizationId:=z_OrganizationID,
                currentUserId:=z_User)

            If currentPayPeriod Is Nothing OrElse employeeId Is Nothing Then
                MessageBoxHelper.ErrorMessage(errorMessage)

                Return False
            End If

            'If we are going to enable this as a policy, check its employee type.
            'If Daily then its allowance frequency should also be Daily.
            'Else allowance frequency should be semi-monthly (for Fixed and Monthly)
            'If _employee.IsDaily Then
            'End If
            _ecolaAllowance = Await BenchmarkPayrollHelper.GetEcola(
                employeeId.Value,
                payDateFrom:=currentPayPeriod.PayFromDate,
                payDateTo:=currentPayPeriod.PayToDate)

            If _ecolaAllowance Is Nothing Then
                MessageBoxHelper.ErrorMessage(errorMessage)
                Return False
            End If

        End If

        Return True

    End Function

    Private Sub ClearForm()

        dtpEffectiveFrom.Value = Date.Today
        txtAmount.Text = "0.00"
        txtAllowance.Text = "0.00"
        txtTotalSalary.Text = "0.00"
        txtPhilHealth.Text = String.Empty
        txtPagIbig.Text = String.Empty
        chkPaySSS.Checked = True
        chkPayPhilHealth.Checked = True
        ChkPagIbig.Checked = True
    End Sub

    Private Sub ToggleBenchmarkEcola()

        lblTotalSalary.Visible = Not _isSystemOwnerBenchMark
        lblTotalSalaryPeroSign.Visible = Not _isSystemOwnerBenchMark
        txtTotalSalary.Visible = Not _isSystemOwnerBenchMark

        lblEcola.Visible = _isSystemOwnerBenchMark
        lblEcolaPeroSign.Visible = _isSystemOwnerBenchMark
        txtEcola.Visible = _isSystemOwnerBenchMark

    End Sub

    Private Sub ChkPayPhilHealth_CheckedChanged(sender As Object, e As EventArgs) Handles chkPayPhilHealth.CheckedChanged
        txtPhilHealth.Enabled = Not chkPayPhilHealth.Checked
    End Sub

    Private Sub ChkPagIbig_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPagIbig.CheckedChanged
        txtPagIbig.Enabled = Not ChkPagIbig.Checked
    End Sub

    Private Async Sub AddAndCloseButton_Click(sender As Object, e As EventArgs) Handles AddAndCloseButton.Click, AddAndNewButton.Click
        pbEmployee.Focus() 'To lose focus on current control
        Dim succeed As Boolean
        Dim messageTitle = ""

        Await FunctionUtils.TryCatchFunctionAsync("New Salary",
            Async Function()
                Dim newSalary = New Salary
                With newSalary
                    .EffectiveFrom = dtpEffectiveFrom.Value
                    .IsMinimumWage = chkIsMinimumWage.Checked
                    .BasicSalary = txtAmount.Text.ToDecimal
                    .AllowanceSalary = txtAllowance.Text.ToDecimal
                    .DoPaySSSContribution = chkPaySSS.Checked
                    .AutoComputePhilHealthContribution = chkPayPhilHealth.Checked
                    .PhilHealthDeduction = txtPhilHealth.Text.ToDecimal
                    .AutoComputeHDMFContribution = ChkPagIbig.Checked
                    .HDMFAmount = txtPagIbig.Text.ToDecimal
                    .EmployeeID = _employee.RowID
                    .OrganizationID = z_OrganizationID
                End With

                Dim dataService = MainServiceProvider.GetRequiredService(Of ISalaryDataService)
                Await dataService.SaveAsync(newSalary, z_User)

                If _isSystemOwnerBenchMark AndAlso _ecolaAllowance?.RowID IsNot Nothing Then

                    Await EcolaHelper.SaveEcola(_ecolaAllowance.RowID.Value, txtEcola.Text.ToDecimal)

                End If

                succeed = True
            End Function)

        If succeed Then
            isSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Salary successfully added.", "Saved")
                ClearForm()
            Else
                showBalloon = True
                Me.Close()
            End If
        End If

    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 70, -74)
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Sub txtAllowance_txtAmount_TextChanged(sender As Object, e As EventArgs) Handles txtAmount.TextChanged, txtAllowance.TextChanged
        txtTotalSalary.Text = Decimal.Round((txtAllowance.Text.ToDecimal + txtAmount.Text.ToDecimal), 2).ToString("#.##")
    End Sub

End Class
