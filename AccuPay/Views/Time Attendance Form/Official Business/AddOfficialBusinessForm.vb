Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddOfficialBusinessForm
    Public Property IsSaved As Boolean
    Public Property ShowBalloonSuccess As Boolean

    Private _currentEmployee As Employee

    Private _newOfficialBusiness As New OfficialBusiness()

    Private ReadOnly _officialBusinessRepository As IOfficialBusinessRepository

    Sub New(employee As Employee)

        InitializeComponent()

        _currentEmployee = employee

        _officialBusinessRepository = MainServiceProvider.GetRequiredService(Of IOfficialBusinessRepository)

        Me.IsSaved = False

    End Sub

    Private Sub AddLeaveForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        PopulateEmployeeData()

        LoadStatusList()

        ResetForm()

    End Sub

    Private Sub PopulateEmployeeData()

        EmployeeNameTextBox.Text = _currentEmployee?.FullNameWithMiddleInitial

        EmployeeNumberTextBox.Text = _currentEmployee?.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(_currentEmployee.Image)

    End Sub

    Private Sub LoadStatusList()

        StatusComboBox.DataSource = _officialBusinessRepository.GetStatusList()

    End Sub

    Private Sub ResetForm()

        Me._newOfficialBusiness = New OfficialBusiness
        Me._newOfficialBusiness.EmployeeID = _currentEmployee.RowID
        Me._newOfficialBusiness.OrganizationID = z_OrganizationID

        Me._newOfficialBusiness.StartDate = Date.Now
        Me._newOfficialBusiness.EndDate = Date.Now
        Me._newOfficialBusiness.StartTime = Date.Now.TimeOfDay
        Me._newOfficialBusiness.EndTime = Date.Now.TimeOfDay

        Me._newOfficialBusiness.Reason = String.Empty
        Me._newOfficialBusiness.Comments = String.Empty
        Me._newOfficialBusiness.Status = Nothing

        CreateDataBindings()
    End Sub

    Private Sub CreateDataBindings()

        StartDatePicker.DataBindings.Clear()
        StartDatePicker.DataBindings.Add("Value", Me._newOfficialBusiness, "ProperStartDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndDatePicker.DataBindings.Clear()
        EndDatePicker.DataBindings.Add("Value", Me._newOfficialBusiness, "ProperEndDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        StartTimePicker.DataBindings.Clear()
        StartTimePicker.DataBindings.Add("Value", Me._newOfficialBusiness, "StartTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        EndTimePicker.DataBindings.Clear()
        EndTimePicker.DataBindings.Add("Value", Me._newOfficialBusiness, "EndTimeFull", True) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        ReasonTextBox.DataBindings.Clear()
        ReasonTextBox.DataBindings.Add("Text", Me._newOfficialBusiness, "Reason")

        CommentTextBox.DataBindings.Clear()
        CommentTextBox.DataBindings.Add("Text", Me._newOfficialBusiness, "Comments")

        StatusComboBox.SelectedIndex = -1
        StatusComboBox.DataBindings.Clear()
        StatusComboBox.DataBindings.Add("Text", Me._newOfficialBusiness, "Status")

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
        If Me._newOfficialBusiness Is Nothing Then Return

        If EndTimePicker.Value.TimeOfDay < StartTimePicker.Value.TimeOfDay Then

            Me._newOfficialBusiness.EndDate = Me._newOfficialBusiness.StartDate.Value.AddDays(1)
        Else
            Me._newOfficialBusiness.EndDate = Me._newOfficialBusiness.StartDate.Value

        End If

        If Me._newOfficialBusiness.EndDate.HasValue Then
            EndDatePicker.Value = Me._newOfficialBusiness.EndDate.Value
        End If
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAddAndNew.Click, btnAddAndClose.Click

        ForceDataBindingsCommit()

        Await FunctionUtils.TryCatchFunctionAsync("New Official Business",
            Async Function()

                Dim dataService = MainServiceProvider.GetRequiredService(Of IOfficialBusinessDataService)
                Await dataService.SaveAsync(Me._newOfficialBusiness, z_User)

                Me.IsSaved = True

                If sender Is btnAddAndNew Then
                    ShowBalloonInfo("Official Business Successfully Added", "Saved")

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
