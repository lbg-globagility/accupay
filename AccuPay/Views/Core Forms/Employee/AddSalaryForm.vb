Option Strict On
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Utils
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class AddSalaryForm

    Public Property isSaved As Boolean
    Public Property showBalloon As Boolean

    Private Const FormEntityName As String = "Salary"

    Private _employee As Employee

    Private _salaryRepo As SalaryRepository

    Private _userActivityRepo As UserActivityRepository

    Public Sub New(employee As Employee)
        InitializeComponent()

        _employee = employee

        _salaryRepo = MainServiceProvider.GetRequiredService(Of SalaryRepository)

        _userActivityRepo = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
    End Sub

    Private Sub AddSalaryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFullName.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)

        txtPayFrequency.Text = _employee.PayFrequency.Type
        txtSalaryType.Text = _employee.EmployeeType

        ClearForm()
    End Sub

    Private Sub ClearForm()

        dtpEffectiveFrom.Value = Date.Today
        dtpEffectiveTo.Value = Date.Today
        txtAmount.Text = "0.00"
        txtAllowance.Text = "0.00"
        txtTotalSalary.Text = "0.00"
        txtPhilHealth.Text = String.Empty
        txtPagIbig.Text = String.Empty
        chkPaySSS.Checked = True
        chkPayPhilHealth.Checked = True
        ChkPagIbig.Checked = True
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
                    '.EffectiveTo = If(dtpEffectiveTo.Checked, dtpEffectiveTo.Value, New DateTime?)
                    .BasicSalary = txtAmount.Text.ToDecimal
                    .AllowanceSalary = txtAllowance.Text.ToDecimal
                    .DoPaySSSContribution = chkPaySSS.Checked
                    .AutoComputePhilHealthContribution = chkPayPhilHealth.Checked
                    .PhilHealthDeduction = txtPhilHealth.Text.ToDecimal
                    .AutoComputeHDMFContribution = ChkPagIbig.Checked
                    .HDMFAmount = txtPagIbig.Text.ToDecimal
                    .PositionID = _employee.PositionID
                    .EmployeeID = _employee.RowID
                    .OrganizationID = z_OrganizationID
                    .CreatedBy = z_User
                End With

                Await _salaryRepo.SaveAsync(newSalary)

                _userActivityRepo.RecordAdd(z_User, FormEntityName, CInt(newSalary.RowID), z_OrganizationID)

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