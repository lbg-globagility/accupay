Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Utils

Public Class AddEducationalBackgroundForm

    Private Const FormEntityName As String = "Educational Background"
    Public Property isSaved As Boolean
    Public Property showBalloon As Boolean

    Private _employee As Employee

    Private _newEducBg As EducationalBackground

    Private _educBgRepo As EducationalBackgroundRepository

    Private _userActivityRepo As UserActivityRepository

    Public Sub New(employee As Employee,
                   educBgRepo As EducationalBackgroundRepository,
                   userActivityRepo As UserActivityRepository)

        InitializeComponent()

        _employee = employee

        _educBgRepo = educBgRepo

        _userActivityRepo = userActivityRepo
    End Sub

    Private Sub AddEducationalBackgroundForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFullName.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)
        ClearForm()
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

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 70, -74)
    End Sub

    Private Async Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddAndCloseButton.Click, AddAndNewButton.Click
        pbEmployee.Focus() 'To lose focus on current control
        Dim succeed As Boolean
        Dim messageBody = ""
        If cboType.SelectedItem Is Nothing Then
            messageBody = "Education Type empty."
        ElseIf String.IsNullOrWhiteSpace(txtSchool.Text) Then
            messageBody = "School empty."
        End If

        If messageBody <> "" Then
            ShowBalloonInfo(messageBody, "Invalid Input")
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("New Educational Background",
            Async Function()
                _newEducBg = New EducationalBackground
                With _newEducBg
                    .Type = cboType.SelectedItem
                    .School = txtSchool.Text
                    .Degree = txtDegree.Text
                    .Course = txtCourse.Text
                    .Major = txtMajor.Text
                    .DateFrom = dtpDateFrom.Value
                    .DateTo = dtpDateTo.Value
                    .Remarks = txtRemarks.Text
                    .OrganizationID = z_OrganizationID
                    .CreatedBy = z_User
                    .EmployeeID = _employee.RowID.Value
                End With

                Await _educBgRepo.CreateAsync(_newEducBg)

                _userActivityRepo.RecordAdd(z_User, FormEntityName, CInt(_newEducBg.RowID), z_OrganizationID)
                succeed = True
            End Function)

        If succeed Then
            isSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Educational Background successfully added.", "Saved")
                ClearForm()
            Else
                showBalloon = True
                Me.Close()
            End If
        End If
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Sub Dates_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateFrom.ValueChanged, dtpDateTo.ValueChanged
        If dtpDateTo.Value < dtpDateFrom.Value Then
            dtpDateTo.Value = dtpDateFrom.Value
        End If
    End Sub

End Class