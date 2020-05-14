Option Strict On
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Enums
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils

Public Class PromotionTab

    Private Const FormEntityName As String = "Promotion"

    Private _mode As FormMode = FormMode.Empty

    Private _employee As Employee

    Private _promotions As IEnumerable(Of Promotion)

    Private _positions As IEnumerable(Of Position)

    Private _currentPromotion As Promotion

    Private _promotionRepo As PromotionRepository

    Private _salaryRepo As SalaryRepository

    Private _positionRepo As PositionRepository

    Private _employeeRepo As EmployeeRepository

    Private _userActivityRepo As UserActivityRepository

    Public Sub New()
        InitializeComponent()
        dgvPromotions.AutoGenerateColumns = False

        _promotionRepo = New PromotionRepository

        _salaryRepo = New SalaryRepository

        _positionRepo = New PositionRepository

        _employeeRepo = New EmployeeRepository

        _userActivityRepo = New UserActivityRepository
    End Sub
    Friend Async Function SetEmployee(employee As Employee) As Task
        pbEmployee.Focus()
        _employee = employee

        txtFullname.Text = employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(employee.Image)

        Await LoadPromotions()
    End Function

    Private Async Function LoadPromotions() As Task
        If _employee?.RowID Is Nothing Then Return

        _promotions = Await _promotionRepo.GetListByEmployeeAsync(_employee.RowID.Value)
        _promotions = _promotions.OrderByDescending(Function(x) x.EffectiveDate).ToList()

        _positions = Await _positionRepo.GetAllAsync(z_OrganizationID)
        _positions = _positions.OrderBy(Function(x) x.Name).ToList()

        RemoveHandler dgvPromotions.SelectionChanged, AddressOf dgvPromotions_SelectionChanged
        dgvPromotions.DataSource = _promotions
        cboPositionTo.DataSource = _positions

        If _promotions.Count > 0 Then
            SelectPromotion(DirectCast(dgvPromotions.CurrentRow?.DataBoundItem, Promotion))
            ChangeMode(FormMode.Editing)
            FormToolsControl(True)
        Else
            SelectPromotion(Nothing)
            _currentPromotion = New Promotion
            ChangeMode(FormMode.Empty)
            FormToolsControl(False)
        End If

        AddHandler dgvPromotions.SelectionChanged, AddressOf dgvPromotions_SelectionChanged
    End Function

    Private Sub FormToolsControl(control As Boolean)
        cboPositionTo.Enabled = control
        dtpEffectivityDate.Enabled = control
        cboCompensationChange.Enabled = control
        txtNewSalary.Enabled = control
        txtReason.Enabled = control
    End Sub

    Private Sub SelectPromotion(promotion As Promotion)
        If promotion IsNot Nothing Then
            _currentPromotion = promotion

            With _currentPromotion
                lblPositionFrom.Text = .PositionFrom
                cboPositionTo.Text = .PositionTo
                dtpEffectivityDate.Value = .EffectiveDate.Value
                cboCompensationChange.Text = .CompensationToYesNo

                If .SalaryEntity IsNot Nothing Then
                    lblCurrentSalary.Text = .SalaryEntity.BasicSalary.ToString
                    txtNewSalary.Text = .SalaryEntity.BasicSalary.ToString
                Else
                    lblCurrentSalary.Text = "0"
                    txtNewSalary.Text = "0"
                End If

                txtReason.Text = .Reason
            End With
        Else
            ClearForm()
        End If
    End Sub

    Private Sub ClearForm()
        lblPositionFrom.Clear()
        cboPositionTo.SelectedItem = Nothing
        dtpEffectivityDate.Value = Today
        cboCompensationChange.Text = ""
        lblCurrentSalary.Text = "0.00"
        txtNewSalary.Clear()
        txtReason.Clear()
    End Sub

    Private Sub dgvPromotions_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPromotions.SelectionChanged
        If _promotions.Count > 0 Then
            Dim promotion = DirectCast(dgvPromotions.CurrentRow?.DataBoundItem, Promotion)
            SelectPromotion(promotion)

        End If
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

    Private Async Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddPromotionForm(_employee)
        form.ShowDialog()

        If form.isSaved Then
            Await LoadPromotions()
            txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
            If form.showBalloon Then
                ShowBalloonInfo("Promotion successfuly added.", "Saved")
            End If

        End If
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 47, -103)
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        pbEmployee.Focus()
        If Await SavePromotion() Then
            Await LoadPromotions()
        End If
    End Sub

    Private Async Function SavePromotion() As Task(Of Boolean)
        Dim messageTitle = ""
        Dim messageBody = ""
        Dim succeed As Boolean = False

        If String.IsNullOrWhiteSpace(cboPositionTo.Text) Then
            messageBody = "Position To is empty."
        ElseIf String.IsNullOrEmpty(cboCompensationChange.Text) Then
            messageBody = "Compensation Change is empty."
        ElseIf CompensationToString10() = "1" Then
            If String.IsNullOrWhiteSpace(txtNewSalary.Text) Then
                messageBody = "New Salary is empty."
            ElseIf Not Regex.IsMatch(txtNewSalary.text, "^(\d*\.)?\d+$") Then
                messageBody = "New Salary is invalid."
            End If
        End If

        If messageBody <> "" Then
            ShowBalloonInfo(messageBody, "Invalid Input")
            Return False
        End If


        Await FunctionUtils.TryCatchFunctionAsync("Save Promotion",
            Async Function()
                If isChanged() Then
                    Dim oldPromotion = _currentPromotion.CloneJson()

                    With _currentPromotion
                        .PositionFrom = lblPositionFrom.Text
                        .PositionTo = cboPositionTo.Text
                        .EffectiveDate = dtpEffectivityDate.Value
                        .CompensationChange = CompensationToString10()
                        .CompensationValue = CType(txtNewSalary.Text, Decimal?)
                        .Reason = txtReason.Text
                        .LastUpdBy = z_User
                    End With

                    With _employee
                        .Position = CType(cboPositionTo.SelectedItem, Position)
                        .PositionID = .Position.RowID.Value
                    End With

                    Await SavePromotionSalary(oldPromotion)

                    Await _promotionRepo.UpdateAsync(_currentPromotion)
                    Await _employeeRepo.SaveAsync(_employee)

                    RecordUpdateAward(oldPromotion)

                    messageTitle = "Update Promotion"
                    succeed = True
                Else
                    MessageBoxHelper.Warning("No value changed")
                End If
            End Function)

        If succeed Then
            ShowBalloonInfo("Promotion successfuly saved.", messageTitle)
            txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
            Return True
        End If
        Return False
    End Function

    Private Async Function SavePromotionSalary(oldPromotion As Promotion) As Task

        Dim promotionSalary = New Salary

        If oldPromotion.CompensationToYesNo = "Yes" And
            _currentPromotion.CompensationToYesNo = "No" Then

            Await _salaryRepo.DeleteAsync(_currentPromotion.EmployeeSalaryID.Value)

            Return

        ElseIf oldPromotion.CompensationToYesNo = "No" And
            _currentPromotion.CompensationToYesNo = "Yes" Then

            With promotionSalary
                .BasicSalary = CDec(txtNewSalary.Text)
                .AllowanceSalary = 0
                .TotalSalary = .BasicSalary + .AllowanceSalary
                .MaritalStatus = _employee.MaritalStatus
                .PositionID = _employee.PositionID
                .EffectiveFrom = dtpEffectivityDate.Value
                .DoPaySSSContribution = True
                .AutoComputeHDMFContribution = True
                .AutoComputePhilHealthContribution = True
                .EmployeeID = _employee.RowID.Value
                .CreatedBy = z_User
                .OrganizationID = z_OrganizationID
            End With

        ElseIf oldPromotion.CompensationToYesNo = "Yes" And
            _currentPromotion.CompensationToYesNo = "Yes" Then

            promotionSalary = _currentPromotion.SalaryEntity

            With promotionSalary
                .BasicSalary = CDec(txtNewSalary.Text)
                .TotalSalary = .BasicSalary + .AllowanceSalary
                .EffectiveFrom = dtpEffectivityDate.Value
                .LastUpdBy = z_User
            End With

        End If

        Await _salaryRepo.SaveAsync(promotionSalary)

        With _currentPromotion
            .EmployeeSalaryID = promotionSalary.RowID.Value
        End With

    End Function

    Private Function CompensationChange() As Decimal
        If CompensationToString10() = "1" Then
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

    Private Sub RecordUpdateAward(oldPromotion As Promotion)
        Dim changes = New List(Of UserActivityItem)

        Dim entityName = FormEntityName.ToLower()

        If _currentPromotion.PositionTo <> oldPromotion.PositionTo Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldPromotion.RowID),
                        .Description = $"Updated {entityName} position to from '{oldPromotion.PositionTo}' to '{_currentPromotion.PositionTo}'."
                        })
        End If
        If _currentPromotion.EffectiveDate <> oldPromotion.EffectiveDate Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldPromotion.RowID),
                        .Description = $"Updated {entityName} effective date from '{oldPromotion.EffectiveDate.Value.ToShortDateString}' to '{_currentPromotion.EffectiveDate.Value.ToShortDateString}'."
                        })
        End If
        If _currentPromotion.CompensationToYesNo <> oldPromotion.CompensationToYesNo Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldPromotion.RowID),
                        .Description = $"Updated {entityName} compensation change from '{oldPromotion.CompensationToYesNo}' to '{_currentPromotion.CompensationToYesNo}'."
                        })
        End If
        If _currentPromotion.BasicPay <> oldPromotion.BasicPay Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldPromotion.RowID),
                        .Description = $"Updated {entityName} new salary from '{oldPromotion.BasicPay}' to '{_currentPromotion.BasicPay}'."
                        })
        End If
        If _currentPromotion.Reason <> oldPromotion.Reason Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldPromotion.RowID),
                        .Description = $"Updated {entityName} reason from '{oldPromotion.Reason}' to '{_currentPromotion.Reason}'."
                        })
        End If

        _userActivityRepo.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)
    End Sub

    Private Function isChanged() As Boolean

        With _currentPromotion
            If .PositionTo <> cboPositionTo.Text OrElse
                    .EffectiveDate <> dtpEffectivityDate.Value OrElse
                    .CompensationToYesNo <> cboCompensationChange.Text OrElse
                    .BasicPay <> CDec(txtNewSalary.Text) OrElse
                    .Reason <> txtReason.Text Then
                Return True
            End If
            Return False
        End With
    End Function

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim result = MsgBox("Are you sure you want to delete this Promotion?", MsgBoxStyle.YesNo, "Delete Promotion")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Promotion",
                Async Function()

                    Await _promotionRepo.DeleteAsync(_currentPromotion)

                    _userActivityRepo.RecordDelete(z_User, FormEntityName, CInt(_currentPromotion.RowID), z_OrganizationID)

                    Await LoadPromotions()
                End Function)

        End If
    End Sub

    Private Sub btnUserActivity_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Async Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _mode = FormMode.Creating Then
            SelectPromotion(Nothing)
        ElseIf _mode = FormMode.Editing Then
            Await LoadPromotions()
        End If

        If _currentPromotion Is Nothing Then
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
        End If
    End Sub

    Private Sub btnCloseForm_Click(sender As Object, e As EventArgs) Handles btnCloseForm.Click
        EmployeeForm.Close()
    End Sub
End Class
