Option Strict On
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Enums
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils

Public Class EducationalBackgroundTab

    Private _employee As Employee

    Private _educationalBackgrounds As IEnumerable(Of EducationalBackground)

    Private _currentEducBg As EducationalBackground

    Private _mode As FormMode = FormMode.Empty

    Public Sub New()
        InitializeComponent()
        dgvEducBgs.AutoGenerateColumns = False

    End Sub

    Public Async Function SetEmployee(employee As Employee) As Task
        _employee = employee

        txtFullname.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)
        pbEmployee.Focus()  'To focus on EducationalBackgroundTab

        Await LoadEducationalBackgrounds()
    End Function

    Private Async Function LoadEducationalBackgrounds() As Task
        If _employee?.RowID Is Nothing Then Return

        Dim educbgRepo = New EducationalBackgroundRepository
        _educationalBackgrounds = Await educbgRepo.GetListByEmployeeAsync(_employee.RowID.Value)

        RemoveHandler dgvEducBgs.SelectionChanged, AddressOf dgvEducBgs_SelectionChanged
        dgvEducBgs.DataSource = _educationalBackgrounds

        If _educationalBackgrounds.Count > 0 Then
            SelectEducationBackground(DirectCast(dgvEducBgs.CurrentRow?.DataBoundItem, EducationalBackground))
            ChangeMode(FormMode.Editing)
        Else
            SelectEducationBackground(Nothing)
            _currentEducBg = New EducationalBackground
            ChangeMode(FormMode.Empty)
        End If

        AddHandler dgvEducBgs.SelectionChanged, AddressOf dgvEducBgs_SelectionChanged
    End Function

    Private Sub SelectEducationBackground(educBg As EducationalBackground)
        If educBg IsNot Nothing Then
            _currentEducBg = educBg

            With _currentEducBg
                cboType.SelectedItem = .Type
                txtSchool.Text = .School
                txtDegree.Text = .Degree
                txtCourse.Text = .Course
                txtMajor.Text = .Major
                dtpDateFrom.Value = .DateFrom
                dtpDateTo.Value = .DateTo
                txtRemarks.Text = .Remarks
            End With
        Else
            ClearForm()
        End If
    End Sub

    Private Sub ClearForm()
        cboType.SelectedItem = Nothing
        txtSchool.Text = ""
        txtDegree.Text = ""
        txtCourse.Text = ""
        txtMajor.Text = ""
        dtpDateFrom.Value = Today
        dtpDateTo.Value = Today
        txtRemarks.Text = ""
    End Sub

    Private Sub dgvEducBgs_SelectionChanged(sender As Object, e As EventArgs) Handles dgvEducBgs.SelectionChanged
        If _educationalBackgrounds.Count > 0 Then
            Dim educBg = DirectCast(dgvEducBgs.CurrentRow?.DataBoundItem, EducationalBackground)
            SelectEducationBackground(educBg)

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

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        EmployeeForm.Close()
    End Sub

    Private Sub Date_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateFrom.ValueChanged, dtpDateTo.ValueChanged
        If dtpDateTo.Value < dtpDateFrom.Value Then
            dtpDateTo.Value = dtpDateFrom.Value
        End If
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim result = MsgBox("Are you sure you want to delete this Educational Background?", MsgBoxStyle.YesNo, "Delete Educational Background")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Educational Background",
                Async Function()
                    Dim repo = New EducationalBackgroundRepository
                    Await repo.DeleteAsync(_currentEducBg)

                    Dim userActivityRepo = New UserActivityRepository
                    userActivityRepo.RecordDelete(z_User, "Educational Background", CInt(_currentEducBg.RowID), z_OrganizationID)

                    Await LoadEducationalBackgrounds()
                End Function)

        End If
    End Sub

    Private Async Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _mode = FormMode.Creating Then
            SelectEducationBackground(Nothing)
        ElseIf _mode = FormMode.Editing Then
            Await LoadEducationalBackgrounds()
        End If

        If _currentEducBg Is Nothing Then
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
        End If
    End Sub

    Private Async Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddEducationalBackgroundForm(_employee)
        form.ShowDialog()

        If form.isSaved Then
            Await LoadEducationalBackgrounds()

            If form.showBalloon Then
                ShowBalloonInfo("Educational Background successfuly added.", "Saved")
            End If

        End If
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 47, -103)
    End Sub

    Private Sub btnUserActivity_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click
        Dim userActivity As New UserActivityForm("Educational Background")
        userActivity.ShowDialog()
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        pbEmployee.Focus()
        If Await SaveEducBg() Then
            Await LoadEducationalBackgrounds()
        End If
    End Sub

    Private Async Function SaveEducBg() As Task(Of Boolean)
        Dim messageTitle = ""
        Dim succeed As Boolean = False

        If cboType.SelectedItem Is Nothing Then
            ShowBalloonInfo("Type is empty.", "Invalid Input")
            Return False
        ElseIf String.IsNullOrWhiteSpace(txtSchool.Text) Then
            ShowBalloonInfo("School is empty", "Invalid Input")
            Return False
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save Educational Background",
            Async Function()
                If isChanged() Then
                    Dim oldEducBg = _currentEducBg.CloneJson()

                    With _currentEducBg
                        .Type = cboType.SelectedItem.ToString
                        .School = txtSchool.Text
                        .Degree = txtDegree.Text
                        .Course = txtCourse.Text
                        .Major = txtMajor.Text
                        .DateFrom = dtpDateFrom.Value
                        .DateTo = dtpDateTo.Value
                        .Remarks = txtRemarks.Text
                        .LastUpdBy = z_User
                    End With

                    Dim educBgRepo = New EducationalBackgroundRepository
                    Await educBgRepo.UpdateAsync(_currentEducBg)

                    RecordUpdateEducBg(oldEducBg)

                    messageTitle = "Update Award"
                    succeed = True
                Else
                    MessageBoxHelper.Warning("No value changed")
                End If
            End Function)
        If succeed Then
            ShowBalloonInfo("Educational Background successfuly saved.", messageTitle)
            Return True
        End If
        Return False
    End Function

    Private Sub RecordUpdateEducBg(oldEducBg As EducationalBackground)
        Dim changes = New List(Of UserActivityItem)

        If _currentEducBg.Type <> oldEducBg.Type Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldEducBg.RowID),
                        .Description = $"Update education type from '{oldEducBg.Type}' to '{_currentEducBg.Type}'"
                        })
        End If
        If _currentEducBg.School <> oldEducBg.School Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldEducBg.RowID),
                        .Description = $"Update educational background school from '{oldEducBg.School}' to '{_currentEducBg.School}'"
                        })
        End If
        If _currentEducBg.Degree <> oldEducBg.Degree Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldEducBg.RowID),
                        .Description = $"Update educational background degree from '{oldEducBg.Degree}' to '{_currentEducBg.Degree}'"
                        })
        End If
        If _currentEducBg.Course <> oldEducBg.Course Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldEducBg.RowID),
                        .Description = $"Update educational background course from '{oldEducBg.Course}' to '{_currentEducBg.Course}'"
                        })
        End If
        If _currentEducBg.Major <> oldEducBg.Major Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldEducBg.RowID),
                        .Description = $"Update educational background major from '{oldEducBg.Major}' to '{_currentEducBg.Major}'"
                        })
        End If
        If _currentEducBg.DateFrom <> oldEducBg.DateFrom Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldEducBg.RowID),
                        .Description = $"Update education start date from '{oldEducBg.DateFrom.ToShortDateString}' to '{_currentEducBg.DateFrom.ToShortDateString}'"
                        })
        End If
        If _currentEducBg.DateTo <> oldEducBg.DateTo Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldEducBg.RowID),
                        .Description = $"Update education end date from '{oldEducBg.DateTo.ToShortDateString}' to '{_currentEducBg.DateTo.ToShortDateString}'"
                        })
        End If
        If _currentEducBg.Remarks <> oldEducBg.Remarks Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldEducBg.RowID),
                        .Description = $"Update educational background remarks from '{oldEducBg.Remarks}' to '{_currentEducBg.Remarks}'"
                        })
        End If

        Dim repo = New UserActivityRepository
        repo.CreateRecord(z_User, "Educational Background", z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)
    End Sub

    Private Function isChanged() As Boolean
        With _currentEducBg

            If .Type <> cboType.SelectedItem.ToString OrElse
                .School <> txtSchool.Text OrElse
                .Degree <> txtDegree.Text OrElse
                .Course <> txtCourse.Text OrElse
                .Major <> txtMajor.Text OrElse
                .DateFrom <> dtpDateFrom.Value OrElse
                .DateTo <> dtpDateTo.Value OrElse
                .Remarks <> txtRemarks.Text Then
                Return True
            End If
        End With
        Return False
    End Function
End Class
