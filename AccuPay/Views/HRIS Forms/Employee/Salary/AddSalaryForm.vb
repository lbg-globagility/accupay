Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.AccuPay.Desktop.Helpers
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class AddSalaryForm

    Public Property IsSaved As Boolean
    Public Property ShowBalloon As Boolean

    Private _employee As Employee

    Private _isSystemOwnerBenchMark As Boolean

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Private ReadOnly _systemOwnerService As ISystemOwnerService

    Public Sub New(employee As Employee)
        InitializeComponent()

        _employee = employee

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of ISystemOwnerService)
    End Sub

    Private Sub AddSalaryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFullName.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)

        txtPayFrequency.Text = _employee.PayFrequency?.Type
        txtSalaryType.Text = _employee.EmployeeType

        _isSystemOwnerBenchMark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwner.Benchmark

        ToggleBenchmarkEcola()

        ClearForm()
    End Sub

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
                    .EffectiveFrom = dtpEffectiveFrom.Value.Date
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

                If _isSystemOwnerBenchMark Then

                    Await EcolaHelper.CreateEcola(
                        employeeId:=_employee.RowID.Value,
                        ecolaAmount:=txtEcola.Text.ToDecimal,
                        startDate:=newSalary.EffectiveFrom)
                End If

                succeed = True
            End Function)

        If succeed Then
            IsSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Salary successfully added.", "Saved")
                ClearForm()
            Else
                ShowBalloon = True
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

    Private Async Function ShowOrHideGovernmentFields() As Task
        Dim currentSystemOwner = Await _systemOwnerService.GetCurrentSystemOwnerEntityAsync()
        If Not currentSystemOwner.IsMorningSun Then Return

        Dim listOfValueService = MainServiceProvider.GetRequiredService(Of IListOfValueService)
        Dim settings = Await listOfValueService.CreateAsync()

        Dim hdmfPolicy = New Core.Services.Policies.HdmfPolicy(settings)
        Dim hdmfBool = Not hdmfPolicy.HdmfCalculationBasis(z_OrganizationID) = Core.Enums.HdmfCalculationBasis.BasedOnLoan
        Label11.Visible = hdmfBool
        ChkPagIbig.Visible = hdmfBool
        Label217.Visible = hdmfBool
        txtPagIbig.Visible = hdmfBool

        Dim philHealthPolicy = New Core.Services.PhilHealthPolicy(settings)
        Dim philHealthBool = Not philHealthPolicy.CalculationBasis(z_OrganizationID) = Core.Enums.PhilHealthCalculationBasis.BasedOnLoan
        Label8.Visible = philHealthBool
        chkPayPhilHealth.Visible = philHealthBool
        Label215.Visible = philHealthBool
        txtPhilHealth.Visible = philHealthBool

        Dim sssPolicy = New Core.Services.Policies.SssPolicy(settings)
        Dim sssBool = Not sssPolicy.SssCalculationBasis(z_OrganizationID) = Core.Enums.SssCalculationBasis.BasedOnLoan
        Label9.Visible = sssBool
        chkPaySSS.Visible = sssBool
    End Function

End Class
