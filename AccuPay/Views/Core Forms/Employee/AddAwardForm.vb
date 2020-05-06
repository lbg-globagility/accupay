Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Utils

Public Class AddAwardForm
    Public Property isSaved As Boolean
    Public Property showBalloon As Boolean

    Private Const FormEntityName As String = "Award"

    Private _employee As Employee

    Private _newAward As Award

    Private _awardRepo As New AwardRepository

    Private _userActivityRepo As New UserActivityRepository

    Public Sub New(employee As Employee)
        InitializeComponent()
        _employee = employee
    End Sub

    Private Sub AddAwardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFullName.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)

        ClearForm()
    End Sub

    Private Async Sub AddAndCloseButton_Click(sender As Object, e As EventArgs) Handles AddAndCloseButton.Click, AddAndNewButton.Click
        pbEmployee.Focus() 'To lose focus on current control
        Dim succeed As Boolean
        Dim messageTitle = ""
        If String.IsNullOrWhiteSpace(txtAwardType.Text) Then
            messageTitle = "Invalid Input"
            ShowBalloonInfo("Award Type empty.", messageTitle)
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("New Award",
            Async Function()
                _newAward = New Award
                With _newAward
                    .AwardType = txtAwardType.Text
                    .AwardDescription = txtDescription.Text
                    .AwardDate = dtpAwardDate.Value
                    .OrganizationID = z_OrganizationID
                    .CreatedBy = z_User
                    .EmployeeID = _employee.RowID.Value
                End With

                Await _awardRepo.CreateAsync(_newAward)

                _userActivityRepo.RecordAdd(z_User, FormEntityName, CInt(_newAward.RowID), z_OrganizationID)
                succeed = True
            End Function)

        If succeed Then
            isSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Bonus successfully added.", "Saved")
                ClearForm()
            Else
                showBalloon = True
                Me.Close()
            End If
        End If

    End Sub

    Private Sub ClearForm()
        txtAwardType.Text = ""
        txtDescription.Text = ""
        dtpAwardDate.Value = Today
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 70, -74)
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

End Class