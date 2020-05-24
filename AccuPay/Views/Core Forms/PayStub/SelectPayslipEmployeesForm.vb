Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Payslip
Imports AccuPay.Utilities
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Win32

Public Class SelectPayslipEmployeesForm

    Private ReadOnly _currentPayPeriodId As Integer

    Private ReadOnly _isEmail As Boolean

    Private Const Declared As String = "Declared"

    Private Const Actual As String = "Actual"

    Private _payslipTypes As List(Of String)

    Private _employeeModels As List(Of EmployeeModel)

    Private _currentPayPeriod As PayPeriod

    Private _payslipCreator As PayslipCreator

    Private _policyHelper As PolicyHelper

    Private _payPeriodRepository As PayPeriodRepository

    Private _paystubRepository As PaystubRepository

    Private _paystubEmailRepository As PaystubEmailRepository

    Private _paystubEmailHistoryRepository As PaystubEmailHistoryRepository

    Sub New(currentPayPeriodId As Integer, isEmail As Boolean)

        InitializeComponent()

        _isEmail = isEmail
        _currentPayPeriodId = currentPayPeriodId

        _employeeModels = New List(Of EmployeeModel)

        _payslipTypes = New List(Of String) From {Declared, Actual}

        _payslipCreator = MainServiceProvider.GetRequiredService(Of PayslipCreator)

        _policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

        _paystubRepository = MainServiceProvider.GetRequiredService(Of PaystubRepository)

        _paystubEmailRepository = MainServiceProvider.GetRequiredService(Of PaystubEmailRepository)

        _paystubEmailHistoryRepository = MainServiceProvider.GetRequiredService(Of PaystubEmailHistoryRepository)

    End Sub

    Private Async Sub SelectPayslipEmployeesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        EmployeeGridView.AutoGenerateColumns = False

        PayslipTypeComboBox.DataSource = _payslipTypes

        SendEmailsButton.Visible = _isEmail
        RefreshEmailStatusButton.Visible = _isEmail
        RefreshEmailServiceButton.Visible = _isEmail

        EmailAddressColumn.Visible = _isEmail
        EmailStatusColumn.Visible = _isEmail
        ErrorLogMessageColumn.Visible = _isEmail

        _currentPayPeriod = Await _payPeriodRepository.GetByIdAsync(_currentPayPeriodId)

        Await ShowEmployees()

        Dim showActual = _policyHelper.ShowActual

        If showActual = False Then

            PayslipTypePanel.Visible = False
            PayslipTypeComboBox.Text = Declared

        End If

    End Sub

    Private Async Function ShowEmployees() As Task

        Dim employees = Await _paystubRepository.
                                GetByPayPeriodWithEmployeeDivisionAsync(_currentPayPeriodId)

        Dim employeeModels As New List(Of EmployeeModel)

        For Each paystub In employees
            employeeModels.Add(New EmployeeModel With {
                .EmployeeId = paystub.EmployeeID.Value,
                .EmailAddress = If(String.IsNullOrWhiteSpace(paystub.Employee.EmailAddress),
                                    Nothing,
                                    paystub.Employee.EmailAddress),
                .PaystubId = paystub.RowID.Value,
                .IsSelected = If(Not _isEmail, True, (Not String.IsNullOrWhiteSpace(paystub.Employee.EmailAddress))),
                .EmployeeNumber = paystub.Employee.EmployeeNo,
                .FirstName = paystub.Employee.FirstName,
                .MiddleName = paystub.Employee.MiddleName,
                .LastName = paystub.Employee.LastName,
                .EmployeeType = paystub.Employee.EmployeeType,
                .PositionName = paystub.Employee.Position?.Name,
                .DivisionName = paystub.Employee.Position?.Division?.Name,
                .FullName = paystub.Employee.FullNameWithMiddleInitialLastNameFirst,
                .EmailStatus = "-"
            })
        Next

        _employeeModels = employeeModels.OrderBy(Function(e) e.FullName).ToList

        EmployeeGridView.DataSource = _employeeModels

        Await RefreshEmailStatus()

        EnableDisableButtons()

    End Function

    Private Async Function RefreshEmailStatus() As Task

        If Not _isEmail Then Return

        If _employeeModels.Count = 0 Then Return

        Dim paystubIds = _employeeModels.Select(Function(e) e.PaystubId).ToArray()

        Dim paystubEmails = Await _paystubEmailRepository.GetByPaystubIdsAsync(paystubIds)
        Dim paystubEmailHistories = Await _paystubEmailHistoryRepository.GetByPaystubIdsAsync(paystubIds)

        Dim index = 0
        For Each employee In _employeeModels

            Dim row = EmployeeGridView.Rows(index)
            Dim checkBoxCell = row.Cells(SelectedCheckBoxColumn.Index)

            Dim history = paystubEmailHistories.
                            FirstOrDefault(Function(h) h.PaystubID = employee.PaystubId)

            Dim queue = paystubEmails.
                            OrderByDescending(Function(h) h.Created).
                            FirstOrDefault(Function(h) h.PaystubID = employee.PaystubId)

            checkBoxCell.Style.BackColor = Color.White
            checkBoxCell.ReadOnly = False

            employee.EmailStatus = "-"
            employee.ErrorLogMessage = Nothing

            If history IsNot Nothing Then

                employee.IsSelected = False
                employee.EmailStatus = PaystubEmailHistory.StatusSent
                employee.SentDateTime = history.SentDateTime
                employee.EmailAddress = history.EmailAddress

                row.DefaultCellStyle.BackColor = Color.Green
                row.DefaultCellStyle.ForeColor = Color.White
                checkBoxCell.Style.BackColor = Color.Black
                checkBoxCell.ReadOnly = True

            ElseIf queue IsNot Nothing Then

                employee.IsSelected = False
                employee.EmailStatus = queue.Status

                Select Case employee.EmailStatus
                    Case PaystubEmail.StatusWaiting
                        row.DefaultCellStyle.BackColor = Color.LightGreen
                        row.DefaultCellStyle.ForeColor = Color.Black
                        checkBoxCell.Style.BackColor = Color.Black
                        checkBoxCell.ReadOnly = True

                    Case PaystubEmail.StatusProcessing
                        row.DefaultCellStyle.BackColor = Color.YellowGreen
                        row.DefaultCellStyle.ForeColor = Color.Black
                        checkBoxCell.Style.BackColor = Color.Black
                        checkBoxCell.ReadOnly = True

                    Case PaystubEmail.StatusFailed
                        row.DefaultCellStyle.BackColor = Color.Red
                        row.DefaultCellStyle.ForeColor = Color.White

                        employee.ErrorLogMessage = queue.ErrorLogMessage

                        If String.IsNullOrWhiteSpace(employee.EmailAddress) Then

                            employee.IsSelected = False
                            checkBoxCell.Style.BackColor = Color.Black
                            checkBoxCell.ReadOnly = True

                        End If

                End Select
            Else

                row.DefaultCellStyle.BackColor = Color.White
                row.DefaultCellStyle.ForeColor = Color.Black

                If String.IsNullOrWhiteSpace(employee.EmailAddress) Then

                    employee.IsSelected = False
                    checkBoxCell.Style.BackColor = Color.Black
                    checkBoxCell.ReadOnly = True

                End If
            End If

            index += 1

        Next

    End Function

    Private Sub EnableDisableButtons()
        Dim selectedEmployeesCount = _employeeModels.Where(Function(e) e.IsSelected).Count

        PreviewButton.Enabled = selectedEmployeesCount > 0
        SendEmailsButton.Enabled = selectedEmployeesCount > 0

        SendEmailsButton.Text = $"&Send Emails ({selectedEmployeesCount})"

    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Sub PreviewButton_Click(sender As Object, e As EventArgs) Handles PreviewButton.Click

        Dim isActual = PayslipTypeComboBox.Text = Actual

        Dim employeeIds = _employeeModels.
                            Where(Function(m) m.IsSelected).
                            Select(Function(m) m.EmployeeId).
                            ToArray()

        Dim reportDocument = _payslipCreator.CreateReportDocument(organizationId:=z_OrganizationID,
                                                                 payPeriodId:=_currentPayPeriod.RowID.Value,
                                                                 Convert.ToSByte(isActual),
                                                                 employeeIds)

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = reportDocument.GetReportDocument()
        crvwr.Show()
    End Sub

    Private Async Sub ProceedButton_Click(sender As Object, e As EventArgs) Handles SendEmailsButton.Click

        Dim employees = _employeeModels.Where(Function(m) m.IsSelected).ToList

        If employees.Count = 0 Then Return

        Dim paystubEmails As New List(Of PaystubEmail)

        For Each employee In employees
            paystubEmails.Add(New PaystubEmail() With {
                    .CreatedBy = z_User,
                    .PaystubID = employee.PaystubId,
                    .Status = PaystubEmail.StatusWaiting
                })
        Next

        Await _paystubEmailRepository.CreateManyAsync(paystubEmails)

        Await RefreshEmailStatus()

        EnableDisableButtons()

        MessageBoxHelper.Information($"{employees.Count} email(s) were added to queue. You can click refresh to check the updated status of the emails.")

    End Sub

    Private Async Sub RefreshEmailStatusButton_Click(sender As Object, e As EventArgs) Handles RefreshEmailStatusButton.Click
        Await RefreshEmailStatus()

        EnableDisableButtons()
    End Sub

    Private Sub EmployeesDataGrid_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles EmployeeGridView.CellMouseUp

        EnableDisableButtons()
    End Sub

    Private Async Sub RefreshEmailServiceButton_Click(sender As Object, e As EventArgs) Handles RefreshEmailServiceButton.Click

        Dim serviceName = StringConfig.AccupayEmailServiceName
        Dim globagilityHelpDescription = $"Please restart the {serviceName} manually or contact Globagility Inc. for assistance."

        Try
            'TODO: getting the IP Address should be from a static class and also will be used by other
            'functions that are needing the values from registry

            Dim regKey = Registry.LocalMachine.OpenSubKey("Software\Globagility\DBConn\GoldWings", True)
            Dim serverIpAddress = regKey.GetValue("server").ToString

            Dim service = New WSMService(serverIpAddress, serviceName)
            Dim result = Await service.StartOrRestart()

            If result.IsSuccessStatusCode Then
                MessageBoxHelper.Information("Service successfully restarted.")
            Else
                MessageBoxHelper.Information($"Cannot restart the service at this time. {globagilityHelpDescription}")

            End If

            ''TODO: AccuPay Email Service should be stored in a static class that will also be used
            ''by the AccuPayWindowService project.
        Catch ex As Exception
            MessageBoxHelper.ErrorMessage($"An error occured trying to restart the service. {globagilityHelpDescription}")

        End Try

    End Sub

    Private Class EmployeeModel
        Public Property EmployeeId As Integer
        Public Property EmailAddress As String
        Public Property PaystubId As Integer
        Public Property IsSelected As Boolean
        Public Property EmployeeNumber As String
        Public Property FirstName As String
        Public Property MiddleName As String
        Public Property LastName As String
        Public Property FullName As String
        Public Property EmployeeType As String
        Public Property PositionName As String
        Public Property DivisionName As String
        Public Property EmailStatus As String
        Public Property SentDateTime As Date?
        Public Property ErrorLogMessage As String
    End Class

End Class