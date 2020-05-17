Option Strict On

Imports System.Text.RegularExpressions
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection

Public Class AddPromotionForm

    Private Const FormEntityName As String = "Promotion"
    Public Property isSaved As Boolean
    Public Property showBalloon As Boolean

    Private _newPromotion As Promotion

    Private _positions As IEnumerable(Of Position)

    Private _newSalary As Salary

    Private _currentSalary As Salary

    Private _employee As Employee

    Private _latestPromotion As Promotion

    Private _employeeRepo As EmployeeRepository

    Private _promotionRepo As PromotionRepository

    Private _positionRepo As PositionRepository

    Private _salaryRepo As SalaryRepository

    Private _userActivityRepo As UserActivityRepository

    Public Sub New(employee As Employee)

        InitializeComponent()

        _employee = employee

        _employeeRepo = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _positionRepo = MainServiceProvider.GetRequiredService(Of PositionRepository)

        _promotionRepo = MainServiceProvider.GetRequiredService(Of PromotionRepository)

        _salaryRepo = MainServiceProvider.GetRequiredService(Of SalaryRepository)

        _userActivityRepo = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
    End Sub

    Private Async Sub AddPromotionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFullName.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)

        Dim promotions = Await _promotionRepo.GetListByEmployeeAsync(_employee.RowID.Value)
        _latestPromotion = promotions.LastOrDefault

        _positions = Await _positionRepo.GetAllAsync(z_OrganizationID)
        _positions = _positions.OrderBy(Function(x) x.Name).ToList()
        cboPositionTo.DataSource = _positions

        _currentSalary = Await _employeeRepo.GetCurrentSalaryAsync(_employee.RowID.Value)

        If _currentSalary IsNot Nothing Then
            lblCurrentSalary.Text = _currentSalary.BasicSalary.ToString
        End If

        ClearForm()
    End Sub

    Private Async Sub AddAndCloseButton_Click(sender As Object, e As EventArgs) Handles AddAndCloseButton.Click, AddAndNewButton.Click

        pbEmployee.Focus() 'To lose focus on current control
        Dim succeed As Boolean
        Dim messageBody = ""

        If String.IsNullOrWhiteSpace(cboPositionTo.Text) Then
            messageBody = "Position To is empty."
        ElseIf String.IsNullOrEmpty(cboCompensationChange.Text) Then
            messageBody = "Compensation Change is empty."
        ElseIf CompensationToString10() = "1" Then
            If String.IsNullOrWhiteSpace(txtNewSalary.Text) Then
                messageBody = "New Salary is empty."
            ElseIf Not Regex.IsMatch(txtNewSalary.Text, "^(\d*\.)?\d+$") Then
                messageBody = "New Salary is invalid."
            End If
        End If

        If messageBody <> "" Then
            ShowBalloonInfo(messageBody, "Invalid input")
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("New Promotion",
            Async Function()
                If CompensationToString10() = "1" Then
                    _newSalary = New Salary

                    With _newSalary
                        .BasicSalary = CDec(txtNewSalary.Text)
                        .AllowanceSalary = 0
                        .TotalSalary = .BasicSalary + .AllowanceSalary
                        .MaritalStatus = _employee.MaritalStatus
                        .PositionID = CType(cboPositionTo.SelectedValue, Position).RowID.Value
                        .EffectiveFrom = dtpEffectivityDate.Value
                        .DoPaySSSContribution = True
                        .AutoComputeHDMFContribution = True
                        .AutoComputePhilHealthContribution = True
                        .EmployeeID = _employee.RowID.Value
                        .CreatedBy = z_User
                        .OrganizationID = z_OrganizationID
                    End With

                    Await _salaryRepo.SaveAsync(_newSalary)
                End If

                _newPromotion = New Promotion
                With _newPromotion
                    .PositionFrom = lblPositionFrom.Text
                    .PositionTo = cboPositionTo.Text
                    .EffectiveDate = dtpEffectivityDate.Value
                    .CompensationChange = CompensationToString10()
                    .CompensationValue = CompensationChange()
                    .Reason = txtReason.Text
                    .NewAmount = 0
                    .EmployeeID = _employee.RowID.Value
                    .OrganizationID = z_OrganizationID
                    .CreatedBy = z_User

                    If CompensationToString10() = "1" Then
                        .EmployeeSalaryID = _newSalary.RowID.Value
                    ElseIf CompensationToString10() = "0" And _currentSalary IsNot Nothing Then
                        .EmployeeSalaryID = _currentSalary.RowID.Value
                    End If
                End With

                Await _promotionRepo.CreateAsync(_newPromotion)

                _userActivityRepo.RecordAdd(z_User, FormEntityName, CInt(_newPromotion.RowID), z_OrganizationID)

                With _employee
                    .Position = CType(cboPositionTo.SelectedItem, Position)
                    .PositionID = .Position.RowID.Value
                End With

                Await _employeeRepo.SaveAsync(_employee)

                succeed = True
            End Function)

        If succeed Then
            isSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Promotion successfully added.", "Saved")
                ClearForm()
            Else
                showBalloon = True
                Me.Close()
            End If
        End If

    End Sub

    Private Function CompensationChange() As Decimal
        If cboCompensationChange.Text = "Yes" Then
            Return CDec(txtNewSalary.Text)
        Else
            Return CDec(lblCurrentSalary.Text)
        End If

    End Function

    Private Function CompensationToString10() As String
        If cboCompensationChange.Text = "Yes" Then
            Return "1"
        Else
            Return "0"
        End If
    End Function

    Private Sub ClearForm()
        If _latestPromotion Is Nothing Then
            lblPositionFrom.Text = _employee.Position.Name
        Else
            lblPositionFrom.Text = _latestPromotion.PositionTo
        End If
        cboPositionTo.SelectedItem = Nothing
        dtpEffectivityDate.Value = Today
        cboCompensationChange.Text = ""
        txtNewSalary.Clear()
        txtReason.Clear()
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 70, -74)
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Sub cboCompensationChange_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCompensationChange.SelectedIndexChanged
        If cboCompensationChange.Text = "Yes" Then
            lblNewSalary.Visible = True
            lblPeso.Visible = True
            txtNewSalary.Visible = True
            lblReqAsterisk.Visible = True
        Else
            lblNewSalary.Visible = False
            lblPeso.Visible = False
            txtNewSalary.Visible = False
            lblReqAsterisk.Visible = False
        End If
    End Sub

End Class