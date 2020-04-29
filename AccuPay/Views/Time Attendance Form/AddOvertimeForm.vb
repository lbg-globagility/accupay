Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Utils

Public Class AddOvertimeForm
    Public Property IsSaved As Boolean
    Public Property ShowBalloonSuccess As Boolean

    Private Const FormEntityName As String = "Overtime"

    Private _overtimeRepository As New OvertimeRepository()

    Private _currentEmployee As Employee

    Private _newOvertime As New Overtime()

    Sub New(employee As Employee)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _currentEmployee = employee

        Me.IsSaved = False

    End Sub

    Private Sub AddLeaveForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        PopulateEmployeeData()

        LoadStatusList()

        ResetForm()

    End Sub

    Private Sub LoadStatusList()

        StatusComboBox.DataSource = _overtimeRepository.GetStatusList()

    End Sub

    Private Sub PopulateEmployeeData()

        EmployeeNameTextBox.Text = _currentEmployee?.FullNameWithMiddleInitial

        EmployeeNumberTextBox.Text = _currentEmployee?.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(_currentEmployee.Image)

    End Sub

    Private Sub ResetForm()

        Me._newOvertime = New Overtime
        Me._newOvertime.EmployeeID = _currentEmployee.RowID
        Me._newOvertime.OrganizationID = z_OrganizationID
        Me._newOvertime.CreatedBy = z_User

        Me._newOvertime.OTStartDate = Date.Now
        Me._newOvertime.OTEndDate = Date.Now
        Me._newOvertime.OTStartTime = Date.Now.TimeOfDay
        Me._newOvertime.OTEndTime = Date.Now.TimeOfDay

        Me._newOvertime.Reason = String.Empty
        Me._newOvertime.Comments = String.Empty
        Me._newOvertime.Status = Nothing

        CreateDataBindings()
    End Sub

    Private Sub CreateDataBindings()

        StartDatePicker.DataBindings.Clear()
        StartDatePicker.DataBindings.Add("Value", Me._newOvertime, "OTStartDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndDatePicker.DataBindings.Clear()
        EndDatePicker.DataBindings.Add("Value", Me._newOvertime, "OTEndDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        StartTimePicker.DataBindings.Clear()
        StartTimePicker.DataBindings.Add("Value", Me._newOvertime, "OTStartTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndTimePicker.DataBindings.Clear()
        EndTimePicker.DataBindings.Add("Value", Me._newOvertime, "OTEndTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        ReasonTextBox.DataBindings.Clear()
        ReasonTextBox.DataBindings.Add("Text", Me._newOvertime, "Reason")

        CommentTextBox.DataBindings.Clear()
        CommentTextBox.DataBindings.Add("Text", Me._newOvertime, "Comments")

        StatusComboBox.SelectedIndex = -1
        StatusComboBox.DataBindings.Clear()
        StatusComboBox.DataBindings.Add("Text", Me._newOvertime, "Status")

        DetailsTabLayout.Enabled = True

    End Sub

    Private Sub ForceDataBindingsCommit()
        'Workaround. Focus other control to lose focus on current control
        EmployeePictureBox.Focus()
        UpdateEndDateDependingOnStartAndEndTimes()
    End Sub

    Private Function ValidateSave(newOvertime As Overtime) As Boolean

        Dim validationErrorMessage = newOvertime.Validate()

        If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then
            MessageBoxHelper.ErrorMessage(validationErrorMessage)
            Return False
        End If

        Return True

    End Function

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, EmployeeInfoTabLayout, 400)
    End Sub

    Private Sub UpdateEndDateDependingOnStartAndEndTimes()
        If Me._newOvertime Is Nothing Then Return

        If EndTimePicker.Value.TimeOfDay < StartTimePicker.Value.TimeOfDay Then

            Me._newOvertime.OTEndDate = Me._newOvertime.OTStartDate.AddDays(1)
        Else
            Me._newOvertime.OTEndDate = Me._newOvertime.OTStartDate

        End If

        EndDatePicker.Value = Me._newOvertime.OTEndDate
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Async Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddAndNewButton.Click, AddAndCloseButton.Click

        ForceDataBindingsCommit()

        If ValidateSave(Me._newOvertime) = False Then Return

        Await FunctionUtils.TryCatchFunctionAsync("New Overtime",
            Async Function()
                Await _overtimeRepository.SaveAsync(Me._newOvertime)

                Dim repo As New UserActivityRepository
                repo.RecordAdd(z_User, FormEntityName, Me._newOvertime.RowID.Value, z_OrganizationID)

                Me.IsSaved = True

                If sender Is AddAndNewButton Then
                    ShowBalloonInfo("Overtime Successfully Added", "Saved")

                    ResetForm()
                Else

                    Me.ShowBalloonSuccess = True
                    Me.Close()
                End If
            End Function)

    End Sub

    Private Sub TimePicker_Leave(sender As Object, e As EventArgs) _
        Handles StartTimePicker.Leave, EndTimePicker.Leave, StartDatePicker.Leave

        UpdateEndDateDependingOnStartAndEndTimes()

    End Sub

End Class