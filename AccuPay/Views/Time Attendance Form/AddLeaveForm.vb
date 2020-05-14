Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection

Public Class AddLeaveForm
    Public Property IsSaved As Boolean
    Public Property ShowBalloonSuccess As Boolean

    Private Const FormEntityName As String = "Leave"

    Private _currentEmployee As Employee

    Private _newLeave As New Leave

    Private _leaveService As LeaveService

    Private _leaveRepository As LeaveRepository

    Private _productRepository As ProductRepository

    Private _userActivityRepository As UserActivityRepository

    Sub New(employee As Employee)

        InitializeComponent()

        _currentEmployee = employee

        _leaveService = MainServiceProvider.GetRequiredService(Of LeaveService)

        _leaveRepository = MainServiceProvider.GetRequiredService(Of LeaveRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

        Me.IsSaved = False

    End Sub

    Private Async Sub AddLeaveForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        PopulateEmployeeData()

        LoadStatusList()

        Await LoadLeaveTypes()

        ResetForm()

    End Sub

    Private Sub PopulateEmployeeData()

        EmployeeNameTextBox.Text = _currentEmployee?.FullNameWithMiddleInitial

        EmployeeNumberTextBox.Text = _currentEmployee?.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(_currentEmployee.Image)

    End Sub

    Private Sub LoadStatusList()

        StatusComboBox.DataSource = _leaveRepository.GetStatusList()

    End Sub

    Private Async Function LoadLeaveTypes() As Task

        Dim leaveList = New List(Of Product)(Await _productRepository.GetLeaveTypesAsync(z_OrganizationID))

        leaveList = leaveList.Where(Function(a) a.PartNo IsNot Nothing).
                                                Where(Function(a) a.PartNo.Trim <> String.Empty).
                                                ToList

        Dim leaveTypes = _productRepository.ConvertToStringList(leaveList)

        LeaveTypeComboBox.DataSource = leaveTypes
    End Function

    Private Sub ResetForm()

        Me._newLeave = New Leave
        Me._newLeave.EmployeeID = _currentEmployee.RowID
        Me._newLeave.OrganizationID = z_OrganizationID
        Me._newLeave.CreatedBy = z_User

        Me._newLeave.StartDate = Date.Now
        Me._newLeave.EndDate = Date.Now
        Me._newLeave.StartTime = Date.Now.TimeOfDay
        Me._newLeave.EndTime = Date.Now.TimeOfDay

        Me._newLeave.Reason = String.Empty
        Me._newLeave.Comments = String.Empty
        Me._newLeave.Status = Nothing
        Me._newLeave.LeaveType = Nothing

        CreateDataBindings()
    End Sub

    Private Sub CreateDataBindings()

        StartDatePicker.DataBindings.Clear()
        StartDatePicker.DataBindings.Add("Value", Me._newLeave, "StartDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndDatePicker.DataBindings.Clear()
        EndDatePicker.DataBindings.Add("Value", Me._newLeave, "ProperEndDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        StartTimePicker.DataBindings.Clear()
        StartTimePicker.DataBindings.Add("Value", Me._newLeave, "StartTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndTimePicker.DataBindings.Clear()
        EndTimePicker.DataBindings.Add("Value", Me._newLeave, "EndTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        ReasonTextBox.DataBindings.Clear()
        ReasonTextBox.DataBindings.Add("Text", Me._newLeave, "Reason")

        CommentTextBox.DataBindings.Clear()
        CommentTextBox.DataBindings.Add("Text", Me._newLeave, "Comments")

        LeaveTypeComboBox.SelectedIndex = -1
        LeaveTypeComboBox.DataBindings.Clear()
        LeaveTypeComboBox.DataBindings.Add("Text", Me._newLeave, "LeaveType")

        StatusComboBox.SelectedIndex = -1
        StatusComboBox.DataBindings.Clear()
        StatusComboBox.DataBindings.Add("Text", Me._newLeave, "Status")

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

    Private Function ValidateSave(newLeave As Leave) As Boolean

        Dim validationErrorMessage = newLeave.Validate()

        If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then
            MessageBoxHelper.ErrorMessage(validationErrorMessage)
            Return False
        End If

        Return True

    End Function

    Private Sub UpdateEndDateDependingOnStartAndEndTimes()
        If Me._newLeave Is Nothing Then Return

        If StartTimePicker.Checked AndAlso EndTimePicker.Checked AndAlso
            EndTimePicker.Value.TimeOfDay < StartTimePicker.Value.TimeOfDay Then

            Me._newLeave.EndDate = Me._newLeave.StartDate.AddDays(1)
        Else
            Me._newLeave.EndDate = Me._newLeave.StartDate

        End If

        If Me._newLeave.EndDate.HasValue Then
            EndDatePicker.Value = Me._newLeave.EndDate.Value

        End If

    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Async Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddAndNewButton.Click, AddAndCloseButton.Click

        ForceDataBindingsCommit()

        If StartTimePicker.Checked = False Then
            Me._newLeave.StartTime = Nothing
        End If

        If EndTimePicker.Checked = False Then
            Me._newLeave.EndTime = Nothing
        End If

        If ValidateSave(Me._newLeave) = False Then Return

        Await FunctionUtils.TryCatchFunctionAsync("New Leave",
            Async Function()
                Dim list As New List(Of Leave)
                list.Add(Me._newLeave)

                'Temporarily use SaveMany to validate the leave
                'TODO: use SaveAsync
                Await _leaveService.SaveManyAsync(list, z_OrganizationID)

                _userActivityRepository.RecordAdd(z_User, FormEntityName, Me._newLeave.RowID.Value, z_OrganizationID)

                Me.IsSaved = True

                If sender Is AddAndNewButton Then
                    ShowBalloonInfo("Leave Successfully Added", "Saved")

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