Imports AccuPay
Imports AccuPay.Data.Repositories
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class AddOfficialBusinessForm
    Public Property IsSaved As Boolean
    Public Property ShowBalloonSuccess As Boolean

    Private _officialBusinessRepository As New OfficialBusinessRepository

    Private _currentEmployee As Employee

    Private _newOfficialBusiness As New OfficialBusiness

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

    Private Function ValidateSave(newOfficialBusiness As OfficialBusiness) As Boolean

        Dim validationErrorMessage = newOfficialBusiness.Validate()

        If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then
            MessageBoxHelper.ErrorMessage(validationErrorMessage)
            Return False
        End If

        Return True

    End Function

    Private Sub UpdateEndDateDependingOnStartAndEndTimes()
        If Me._newOfficialBusiness Is Nothing Then Return

        If EndTimePicker.Value.TimeOfDay < StartTimePicker.Value.TimeOfDay Then

            Me._newOfficialBusiness.EndDate = Me._newOfficialBusiness.StartDate.Value.AddDays(1)
        Else
            Me._newOfficialBusiness.EndDate = Me._newOfficialBusiness.StartDate.Value

        End If

        EndDatePicker.Value = Me._newOfficialBusiness.EndDate
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAddAndNew.Click, btnAddAndClose.Click

        ForceDataBindingsCommit()

        If ValidateSave(Me._newOfficialBusiness) = False Then Return

        Await FunctionUtils.TryCatchFunctionAsync("New Official Business",
            Async Function()
                Await _officialBusinessRepository.SaveAsync(Me._newOfficialBusiness)

                Dim repo As New UserActivityRepository
                repo.RecordAdd(z_User, "Official Business", Me._newOfficialBusiness.RowID, z_OrganizationID)

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