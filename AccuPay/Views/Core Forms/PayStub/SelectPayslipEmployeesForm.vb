Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Payslip
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore

Public Class SelectPayslipEmployeesForm

    Private ReadOnly _currentPayPeriodId As Integer

    Private ReadOnly _isEmail As Boolean

    Private Declared As String = "Declared"

    Private Actual As String = "Actual"

    Private PayslipTypes As New List(Of String) From {Declared, Actual}

    Private _employeeModels As New List(Of EmployeeModel)

    Private _currentPayPeriod As PayPeriod

    Sub New(currentPayPeriodId As Integer, isEmail As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _isEmail = isEmail
        _currentPayPeriodId = currentPayPeriodId

    End Sub

    Private Async Sub SelectPayslipEmployeesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        EmployeesDataGrid.AutoGenerateColumns = False

        PayslipTypeComboBox.DataSource = PayslipTypes

        SendEmailsButton.Visible = _isEmail
        RefreshEmailStatusButton.Visible = _isEmail

        EmailAddressColumn.Visible = _isEmail
        EmailStatusColumn.Visible = _isEmail
        ErrorLogMessageColumn.Visible = _isEmail

        _currentPayPeriod = Await GetCurrentPayPeriod()

        Await ShowEmployees()

        Using context As New PayrollContext

            Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

            Dim showActual = (settings.GetBoolean("Policy.ShowActual", True) = True)

            If showActual = False Then

                PayslipTypePanel.Visible = False
                PayslipTypeComboBox.Text = Declared

            End If

        End Using

    End Sub

    Private Async Function GetCurrentPayPeriod() As Task(Of PayPeriod)
        Using context As New PayrollContext

            Return Await context.PayPeriods.
                FirstOrDefaultAsync(Function(p) p.RowID.Value = _currentPayPeriodId)

        End Using
    End Function

    Private Async Function ShowEmployees() As Task

        Using context As New PayrollContext

            Dim employees = Await context.Paystubs.
                                Include(Function(p) p.Employee.Position.Division).
                                Where(Function(p) p.PayPeriodID.Value = _currentPayPeriodId).
                                ToListAsync

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

            EmployeesDataGrid.DataSource = _employeeModels

            Await RefreshEmailStatus()

            EnableDisableButtons()

        End Using

    End Function

    Private Async Function RefreshEmailStatus() As Task

        If Not _isEmail Then Return

        If _employeeModels.Count = 0 Then Return

        Using context As New PayrollContext

            Dim paystubIds = _employeeModels.Select(Function(e) e.PaystubId).ToArray

            Dim paystubEmails = Await context.PaystubEmails.
                                    Where(Function(p) paystubIds.Contains(p.PaystubID)).
                                    ToListAsync

            Dim paystubEmailHistories = Await context.PaystubEmailHistories.
                                                Where(Function(p) paystubIds.Contains(p.PaystubID)).
                                                ToListAsync

            Dim index = 0
            For Each employee In _employeeModels

                Dim row = EmployeesDataGrid.Rows(index)
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
                    employee.EmailStatus = AccuPay.Data.Entities.PaystubEmailHistory.StatusSent
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
                        Case AccuPay.Data.Entities.PaystubEmail.StatusWaiting
                            row.DefaultCellStyle.BackColor = Color.LightGreen
                            row.DefaultCellStyle.ForeColor = Color.Black
                            checkBoxCell.Style.BackColor = Color.Black
                            checkBoxCell.ReadOnly = True

                        Case AccuPay.Data.Entities.PaystubEmail.StatusProcessing
                            row.DefaultCellStyle.BackColor = Color.YellowGreen
                            row.DefaultCellStyle.ForeColor = Color.Black
                            checkBoxCell.Style.BackColor = Color.Black
                            checkBoxCell.ReadOnly = True

                        Case AccuPay.Data.Entities.PaystubEmail.StatusFailed
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

        End Using

    End Function

    Private Sub EnableDisableButtons()
        Dim selectedEmployeesCount = _employeeModels.Where(Function(e) e.IsSelected).Count

        PreviewButton.Enabled = selectedEmployeesCount > 0
        SendEmailsButton.Enabled = selectedEmployeesCount > 0

        SendEmailsButton.Text = $"&Send Emails ({selectedEmployeesCount})"

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

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Sub PreviewButton_Click(sender As Object, e As EventArgs) Handles PreviewButton.Click

        Dim isActual = PayslipTypeComboBox.Text = Actual

        Dim payslipCreator As New PayslipCreator(_currentPayPeriod, Convert.ToSByte(isActual))

        Dim nextPayPeriod = PayrollTools.GetNextPayPeriod(_currentPayPeriod.RowID.Value)

        Dim employeeIds = _employeeModels.
                            Where(Function(m) m.IsSelected).
                            Select(Function(m) m.EmployeeId).
                            ToArray()

        Dim reportDocument = payslipCreator.CreateReportDocument(z_OrganizationID, nextPayPeriod, employeeIds)

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = reportDocument.GetReportDocument()
        crvwr.Show()
    End Sub

    Private Async Sub ProceedButton_Click(sender As Object, e As EventArgs) Handles SendEmailsButton.Click

        Dim employees = _employeeModels.Where(Function(m) m.IsSelected).ToList

        If employees.Count = 0 Then Return

        Using context As New PayrollContext

            For Each employee In employees
                context.Add(New PaystubEmail() With {
                    .CreatedBy = z_User,
                    .PaystubID = employee.PaystubId,
                    .Status = AccuPay.Data.Entities.PaystubEmail.StatusWaiting
                })

                Await context.SaveChangesAsync
            Next

        End Using

        Await RefreshEmailStatus()

        EnableDisableButtons()

        MessageBoxHelper.Information($"{employees.Count} email(s) were added to queue. You can click refresh to check the updated status of the emails.")

    End Sub

    Private Async Sub RefreshEmailStatusButton_Click(sender As Object, e As EventArgs) Handles RefreshEmailStatusButton.Click
        Await RefreshEmailStatus()

        EnableDisableButtons()
    End Sub

    Private Sub EmployeesDataGrid_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles EmployeesDataGrid.CellMouseUp

        EnableDisableButtons()
    End Sub

End Class