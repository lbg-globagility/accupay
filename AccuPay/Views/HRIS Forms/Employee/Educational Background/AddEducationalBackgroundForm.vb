Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddEducationalBackgroundForm

    Public Property IsSaved As Boolean
    Public Property ShowBalloon As Boolean

    Private _employee As Employee

    Private _newEducBg As EducationalBackground

    Public Sub New(employee As Employee)

        InitializeComponent()

        _employee = employee
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
                    .Type = cboType.Text
                    .School = txtSchool.Text
                    .Degree = txtDegree.Text
                    .Course = txtCourse.Text
                    .Major = txtMajor.Text
                    .DateFrom = dtpDateFrom.Value
                    .DateTo = dtpDateTo.Value
                    .Remarks = txtRemarks.Text
                    .OrganizationID = z_OrganizationID
                    .EmployeeID = _employee.RowID.Value
                End With

                Dim dataService = MainServiceProvider.GetRequiredService(Of IEducationalBackgroundDataService)
                Await dataService.SaveAsync(_newEducBg, z_User)

                succeed = True
            End Function)

        If succeed Then
            IsSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Educational Background successfully added.", "Saved")
                ClearForm()
            Else
                ShowBalloon = True
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
