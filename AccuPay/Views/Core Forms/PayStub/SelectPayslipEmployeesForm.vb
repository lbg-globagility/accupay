Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.CrystalReports
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Win32

Public Class SelectPayslipEmployeesForm

    Public Const StatusSent As String = "SENT"

    Private ReadOnly _currentPayPeriodId As Integer

    Private ReadOnly _isEmail As Boolean

    Private _employeeModels As List(Of EmployeeModel)

    Private _currentPayPeriod As PayPeriod

    Private ReadOnly _payslipCreator As PayslipBuilder

    Private ReadOnly _policyHelper As PolicyHelper

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private ReadOnly _paystubRepository As PaystubRepository

    Private ReadOnly _paystubEmailRepository As PaystubEmailRepository

    Private ReadOnly _paystubEmailHistoryRepository As PaystubEmailHistoryRepository

    Sub New(currentPayPeriodId As Integer, isEmail As Boolean)

        InitializeComponent()

        _isEmail = isEmail

        _currentPayPeriodId = currentPayPeriodId

        _employeeModels = New List(Of EmployeeModel)

        _payslipCreator = MainServiceProvider.GetRequiredService(Of PayslipBuilder)

        _policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

        _paystubRepository = MainServiceProvider.GetRequiredService(Of PaystubRepository)

        _paystubEmailRepository = MainServiceProvider.GetRequiredService(Of PaystubEmailRepository)

        _paystubEmailHistoryRepository = MainServiceProvider.GetRequiredService(Of PaystubEmailHistoryRepository)

    End Sub

    Private Async Sub SelectPayslipEmployeesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        EmployeeGridView.AutoGenerateColumns = False

        SendEmailToolStripDropDownButton.Visible = _isEmail
        SendEmailToolStripButton.Visible = _isEmail

        ManageEmailToolStripDropDownButton.Visible = _isEmail
        RefreshEmailServiceToolStripButton.Visible = _isEmail

        EmailAddressColumn.Visible = _isEmail
        PayslipTypeColumn.Visible = _isEmail
        EmailStatusColumn.Visible = _isEmail
        ResetEmailButtonColumn.Visible = _isEmail
        ErrorLogMessageColumn.Visible = _isEmail

        _currentPayPeriod = Await _payPeriodRepository.GetByIdAsync(_currentPayPeriodId)

        Await ShowEmployees()

        If _policyHelper.ShowActual Then

            PreviewToolStripButton.Visible = False
            SendEmailToolStripButton.Visible = False
        Else

            PreviewToolStripDropDownButton.Visible = False
            SendEmailToolStripDropDownButton.Visible = False

            PayslipTypeColumn.Visible = False
        End If

    End Sub

    Private Async Function ShowEmployees() As Task

        Dim employees = Await _paystubRepository.
            GetByPayPeriodWithEmployeeDivisionAsync(_currentPayPeriodId)

        Dim employeeModels As New List(Of EmployeeModel)

        For Each paystub In employees
            employeeModels.Add(New EmployeeModel With {
                .EmployeeId = paystub.EmployeeID.Value,
                .EmailAddress = If(
                    String.IsNullOrWhiteSpace(paystub.Employee.EmailAddress),
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
                .PayslipType = "-",
                .EmailStatus = "-"
            })
        Next

        _employeeModels = employeeModels.OrderBy(Function(e) e.FullName).ToList

        EmployeeGridView.DataSource = _employeeModels

        Await RefreshEmailStatus()

        EnableDisableActionButtons()

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

            employee.PayslipType = "-"
            employee.EmailStatus = "-"
            employee.ErrorLogMessage = Nothing

            If history IsNot Nothing Then

                employee.IsSelected = False
                employee.PayslipType = SetPayslipType(history.IsActual)
                employee.EmailStatus = StatusSent
                employee.SentDateTime = history.SentDateTime
                employee.EmailAddress = history.EmailAddress

                row.DefaultCellStyle.BackColor = Color.Green
                row.DefaultCellStyle.ForeColor = Color.White
                checkBoxCell.Style.BackColor = Color.Black
                checkBoxCell.ReadOnly = True

            ElseIf queue IsNot Nothing Then

                employee.IsSelected = False
                employee.PayslipType = SetPayslipType(queue.IsActual)
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

    Private Shared Function SetPayslipType(isActual As Boolean) As String
        Return If(isActual, "Actual", "Declared")
    End Function

    Private Sub EnableDisableActionButtons()
        Dim selectedEmployeesCount = _employeeModels.Where(Function(e) e.IsSelected).Count

        Dim enabled = selectedEmployeesCount > 0

        PreviewToolStripDropDownButton.Enabled = enabled
        PreviewToolStripButton.Enabled = enabled

        SendEmailToolStripDropDownButton.Enabled = enabled
        SendEmailToolStripButton.Enabled = enabled

        StatusLabel.Text = $"{selectedEmployeesCount} employee(s) selected."
    End Sub

    Private Sub CloseDialogButton_Click(sender As Object, e As EventArgs) Handles CloseDialogButton.Click
        Me.Close()
    End Sub

    Private Sub PreviewButton_Click(sender As Object, e As EventArgs) Handles _
        PreviewToolStripButton.Click,
        PreviewDeclaredToolStripMenuItem.Click,
        PreviewActualToolStripMenuItem.Click

        Dim isActual = sender Is PreviewActualToolStripMenuItem

        DisableAllButtons()

        FunctionUtils.TryCatchFunction("Print Payslip",
            Sub()
                Dim employeeIds = _employeeModels.
                    Where(Function(m) m.IsSelected).
                    Select(Function(m) m.EmployeeId).
                    ToArray()

                Dim reportDocument = _payslipCreator.CreateReportDocument(
                    payPeriodId:=_currentPayPeriod.RowID.Value,
                    isActual:=Convert.ToSByte(isActual),
                    employeeIds:=employeeIds)

                Dim crvwr As New CrysRepForm
                crvwr.crysrepvwr.ReportSource = reportDocument.GetReportDocument()
                crvwr.Show()
            End Sub)

        DisableAllButtons(disable:=False)

    End Sub

    Private Async Sub SendEmailButton_Click(sender As Object, e As EventArgs) Handles _
        SendEmailToolStripButton.Click,
        SendEmailActualToolStripMenuItem.Click,
        SendEmailDeclaredToolStripMenuItem.Click

        Dim employees = _employeeModels.Where(Function(m) m.IsSelected).ToList

        If employees.Count = 0 Then Return

        Dim isActual = sender Is SendEmailActualToolStripMenuItem

        Dim payslipType = ""
        If _policyHelper.ShowActual Then

            payslipType = If(isActual, " Actual", " Declared")
        End If

        Const messageTitle As String = "Send Email Payslips"
        Dim confirm = MessageBoxHelper.Confirm(Of Boolean)(
            $"Send{payslipType} payslips to {employees.Count} employee(s) through email?",
            messageTitle)

        If Not confirm Then Return

        Dim paystubEmails As New List(Of PaystubEmail)

        For Each employee In employees
            paystubEmails.Add(New PaystubEmail() With {
                .CreatedBy = z_User,
                .PaystubID = employee.PaystubId,
                .Status = PaystubEmail.StatusWaiting,
                .IsActual = isActual
            })
        Next

        DisableAllButtons()

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()

                Await _paystubEmailRepository.CreateManyAsync(paystubEmails)

                Await RefreshEmailStatus()

                EnableDisableActionButtons()

                MessageBoxHelper.Information($"{employees.Count} email(s) were added to queue. You can click 'Refresh Status' in 'Manage Emails' to check the updated status of the emails.")

            End Function)

        DisableAllButtons(disable:=False)

    End Sub

    Private Async Sub RefreshEmailStatusButton_Click(sender As Object, e As EventArgs) Handles RefreshEmailStatusToolStripMenuItem.Click
        Await RefreshEmailStatus()

        EnableDisableActionButtons()
    End Sub

    Private Sub EmployeeGridView_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) _
        Handles EmployeeGridView.CellMouseUp

        EmployeeGridView.EndEdit()
        EmployeeGridView.Refresh()
        EnableDisableActionButtons()
    End Sub

    Private Async Sub RefreshEmailServiceToolStripButton_Click(sender As Object, e As EventArgs) Handles _
        RefreshEmailServiceToolStripButton.Click

        Dim serviceName = StringConfig.AccupayEmailServiceName
        Dim globagilityHelpDescription = $"Please restart the {serviceName} manually or contact Globagility Inc. for assistance."

        Try
            DisableAllButtons()
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
        Finally
            DisableAllButtons(disable:=False)

        End Try

    End Sub

    Private Sub DisableAllButtons(Optional disable As Boolean = True)
        ToolStrip1.Enabled = Not disable
        UncheckAllButton.Enabled = Not disable
        CloseDialogButton.Enabled = Not disable

        If disable Then

            Me.Cursor = Cursors.WaitCursor
        Else

            Me.Cursor = Cursors.Default

        End If

    End Sub

    Private Sub UncheckAllButton_Click(sender As Object, e As EventArgs) Handles UncheckAllButton.Click
        For Each model In _employeeModels
            model.IsSelected = False
        Next

        EmployeeGridView.EndEdit()
        EmployeeGridView.Refresh()
        EnableDisableActionButtons()

    End Sub

    Private Async Sub ResetEmailsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetEmailsToolStripMenuItem.Click

        Const messageTitle As String = "Reset All Email Status"
        Dim confirm = MessageBoxHelper.Confirm(Of Boolean)($"Reset ALL email status back to unsent?", messageTitle)

        If Not confirm Then Return

        DisableAllButtons()

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of PaystubDataService)

                Await dataService.DeletePaystubEmailsByPeriodAsync(
                    payPeriodId:=_currentPayPeriod.RowID.Value,
                    organizationId:=z_OrganizationID
                )

                Await RefreshEmailStatus()
            End Function)

        DisableAllButtons(disable:=False)

    End Sub

    Private Async Sub EmployeeGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles EmployeeGridView.CellContentClick

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then Return

        If e.ColumnIndex = ResetEmailButtonColumn.Index Then

            Dim employee = CType(EmployeeGridView.CurrentRow.DataBoundItem, EmployeeModel)

            If employee Is Nothing OrElse _currentPayPeriod?.RowID Is Nothing Then
                Return
            End If

            Const messageTitle As String = "Reset Email Status"
            Dim confirm = MessageBoxHelper.Confirm(Of Boolean)(
                $"Reset email status of employee [{employee.EmployeeNumber}] back to unsent?",
                messageTitle)

            If Not confirm Then Return

            DisableAllButtons()

            Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                Async Function()
                    Dim dataService = MainServiceProvider.GetRequiredService(Of PaystubDataService)

                    Await dataService.DeletePaystubEmailsByEmployeeAndPeriodAsync(
                        employeeId:=employee.EmployeeId,
                        payPeriodId:=_currentPayPeriod.RowID.Value,
                        organizationId:=z_OrganizationID
                    )

                    Await RefreshEmailStatus()
                End Function
            )

            DisableAllButtons(disable:=False)
        End If

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
        Public Property PayslipType As String
        Public Property EmailStatus As String
        Public Property SentDateTime As Date?
        Public Property ErrorLogMessage As String
    End Class

End Class