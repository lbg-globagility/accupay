Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddOvertimeForm
    Public Property IsSaved As Boolean
    Public Property ShowBalloonSuccess As Boolean

    Private _currentEmployee As Employee

    Private _newOvertime As Overtime

    Private ReadOnly _overtimeRepository As OvertimeRepository

    Sub New(employee As Employee)

        InitializeComponent()

        _currentEmployee = employee

        _overtimeRepository = MainServiceProvider.GetRequiredService(Of OvertimeRepository)

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

        Me._newOvertime = Overtime.NewOvertime(
            organizationId:=z_OrganizationID,
            employeeId:=_currentEmployee.RowID.Value,
            startDate:=Date.Now,
            startTime:=Date.Now.TimeOfDay,
            endTime:=Date.Now.TimeOfDay,
            reason:=String.Empty,
            comments:=String.Empty,
            status:=Nothing)

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

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Async Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddAndNewButton.Click, AddAndCloseButton.Click

        ForceDataBindingsCommit()

        Await FunctionUtils.TryCatchFunctionAsync("New Overtime",
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of OvertimeDataService)
                Await dataService.SaveAsync(Me._newOvertime, z_User)

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
