Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class EmailDashboardForm

    Private ReadOnly _paystubEmailRepository As PaystubEmailRepository
    Private ReadOnly _wsmService As WSMService

    Sub New()

        InitializeComponent()

        _paystubEmailRepository = MainServiceProvider.GetRequiredService(Of PaystubEmailRepository)

        Dim connectionString = ConnectionStringRegistry.GetCurrent()
        _wsmService = New WSMService(connectionString.ServerName, StringConfig.AccupayEmailServiceName)
    End Sub

    Private Async Sub EmailDashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            ShowLoadingBar()
            SetDefaultTexts()

            Await Task.Delay(1000)
            Await UpdateDashboardData()
            ShowLoadingBar(False)

            RefreshTimer.Start()
        Catch ex As Exception

            MessageBoxHelper.Warning("Cannot access Email Service.")
            Me.Close()

        End Try
    End Sub

    Private Sub SetDefaultTexts()
        StatusLabel.Text = "-"
        NumberOfQueueLabel.Text = "-"
        CurrentProcessingLabel.Text = "NONE"
        ProcessingDateLabel.Text = "-"
        NextOnQueueLabel.Text = "NONE"
    End Sub

    Private Async Function UpdateOnlineLabels() As Task
        Dim status = Await _wsmService.GetStatus()

        Dim isOnline = status = ServiceProcess.ServiceControllerStatus.Running
        StatusLabel.Text = $"Status: {If(isOnline, "ONLINE", "OFFLINE")}"

        DashboardPanel.BackColor = If(isOnline, Color.DarkGreen, Color.DarkRed)
    End Function

    Private Async Function UpdateEmailLabels() As Task

        Dim nextOnQueue = _paystubEmailRepository.FirstOnQueueWithPaystubDetails()
        Dim allInQueue = Await _paystubEmailRepository.GetAllOnQueueAsync()

        If allInQueue.Any() Then

            NumberOfQueueLabel.Text = $"{allInQueue.Count} email(s) on queue."

            Dim employeeCurrentlyProcessing = allInQueue.FirstOrDefault(Function(q) q.Status = PaystubEmail.StatusProcessing)

            If employeeCurrentlyProcessing IsNot Nothing Then

                CurrentProcessingLabel.Text = GetEmployeeDescription(employeeCurrentlyProcessing)

                If employeeCurrentlyProcessing.ProcessingStarted IsNot Nothing Then

                    ProcessingDateLabel.Text = employeeCurrentlyProcessing.ProcessingStarted.Value.ToShortDateString() & " " &
                        employeeCurrentlyProcessing.ProcessingStarted.Value.ToLongTimeString()
                End If

            End If
        Else
            NumberOfQueueLabel.Text = "No queued emails."
        End If

        If nextOnQueue IsNot Nothing Then

            NextOnQueueLabel.Text = GetEmployeeDescription(nextOnQueue)

        End If
    End Function

    Private Shared Function GetEmployeeDescription(paystubEmail As PaystubEmail) As String
        Return $"{paystubEmail.Paystub.Employee.FullNameWithMiddleInitial} ({paystubEmail.Paystub.Employee.EmailAddress})"
    End Function

    Private Async Sub RestartEmailServiceLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles RestartEmailServiceLinkLabel.LinkClicked

        ShowLoadingBar()

        Dim serviceName = StringConfig.AccupayEmailServiceName
        Dim globagilityHelpDescription = $"Please restart the {serviceName} manually or contact Globagility Inc. for assistance."

        Await FunctionUtils.TryCatchFunctionAsync("Restart Email Service",
            Async Function()

                Dim result = Await _wsmService.StopIfRunning()

                If Not result.IsSuccessStatusCode Then
                    MessageBoxHelper.Information($"Cannot restart the service at this time. {globagilityHelpDescription}")
                    Return Nothing
                End If

                Dim dataService = MainServiceProvider.GetRequiredService(Of PaystubEmailDataService)
                Await dataService.ResetAllProcessingAsync()

                result = Await _wsmService.StartOrRestart()

                If result.IsSuccessStatusCode Then
                    MessageBoxHelper.Information("Service successfully restarted.")
                Else
                    MessageBoxHelper.Information($"Cannot restart the service at this time. {globagilityHelpDescription}")
                    Return Nothing
                End If

                Return Nothing
            End Function)

        ShowLoadingBar(False)

    End Sub

    Private Async Sub RefreshTimer_Tick(sender As Object, e As EventArgs) Handles RefreshTimer.Tick
        Await UpdateDashboardData()

    End Sub

    Private Async Function UpdateDashboardData() As Task
        SetDefaultTexts()

        Await UpdateEmailLabels()
        Await UpdateOnlineLabels()
    End Function

    Private Sub ShowLoadingBar(Optional show As Boolean = True)
        LoadingPictureBox.Visible = show
        DashboardPanel.Visible = Not show
    End Sub

End Class
