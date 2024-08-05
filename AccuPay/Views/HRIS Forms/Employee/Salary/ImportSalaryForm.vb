Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Globagility.AccuPay.Salaries
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportSalaryForm

    Private _salaries As IList(Of Salary)

    Public Property IsSaved As Boolean

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Sub New()

        InitializeComponent()

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        Me.IsSaved = False
    End Sub

    Private Async Sub BrowseButton_Click(sender As Object, e As EventArgs) Handles BrowseButton.Click
        Dim browseFile = New OpenFileDialog With {
            .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"
        }

        If browseFile.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim fileName = browseFile.FileName

        Dim records As New List(Of SalaryRowRecord)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
                                Sub()
                                    records = ExcelService(Of SalaryRowRecord).Read(fileName, "Employee Salary").ToList
                                End Sub)

        If parsedSuccessfully = False Then Return

        If records Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read the template.")

            Return
        End If

        Dim rejectedRecords As New List(Of SalaryRowRecord)

        Dim salaryViewModels = New List(Of SalaryViewModel)
        _salaries = New List(Of Salary)

        Dim employeeNos = records.Select(Function(s) s.EmployeeNo).ToArray()

        Dim employees = Await _employeeRepository.GetByMultipleEmployeeNumberAsync(employeeNos, z_OrganizationID)

        For Each record In records

            Dim employee = employees.FirstOrDefault(Function(t) CBool(t.EmployeeNo = record.EmployeeNo))

            If employee Is Nothing Then
                record.ErrorMessage = "Employee does not exist!"
                rejectedRecords.Add(record)
                Continue For
            End If

            If record.EffectiveFrom IsNot Nothing AndAlso
                record.EffectiveFrom.Value < PayrollTools.SqlServerMinimumDate Then
                record.ErrorMessage = "Dates cannot be earlier than January 1, 1753."
                rejectedRecords.Add(record)
                Continue For
            End If

            Dim lastSalary = Await _employeeRepository.GetCurrentSalaryAsync(employee.RowID.Value)

            Dim doPaySSSContribution = True

            If lastSalary IsNot Nothing Then
                doPaySSSContribution = lastSalary.DoPaySSSContribution
            End If

            If CheckIfRecordIsValid(record, rejectedRecords) = False Then

                Continue For

            End If

            Dim salary = New Salary With {
                .OrganizationID = z_OrganizationID,
                .EmployeeID = employee.RowID,
                .PositionID = employee.PositionID,
                .EffectiveFrom = record.EffectiveFrom.Value,
                .BasicSalary = record.BasicSalary.Value,
                .AllowanceSalary = record.AllowanceSalary,
                .DoPaySSSContribution = If(record.SSSAuto = "YES", True, False),
                .AutoComputeHDMFContribution = If(record.HDMF_Auto = "YES", True, False),
                .AutoComputePhilHealthContribution = If(record.PhilhealthAuto = "YES", True, False),
                .HDMFAmount = record.HDMF,
                .PhilHealthDeduction = record.Philhealth
            }

            salary.UpdateTotalSalary()

            _salaries.Add(salary)
            salaryViewModels.Add(New SalaryViewModel(salary, employee))
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({salaryViewModels.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _salaries.Count > 0

        SalaryDataGrid.DataSource = salaryViewModels
        RejectedRecordsGrid.AutoGenerateColumns = False
        RejectedRecordsGrid.DataSource = rejectedRecords
    End Sub

    Private Function CheckIfRecordIsValid(record As SalaryRowRecord, rejectedRecords As List(Of SalaryRowRecord)) As Boolean

        If record.EffectiveFrom Is Nothing Then

            record.ErrorMessage = "Effective from cannot be empty."
            rejectedRecords.Add(record)
            Return False
        End If

        If record.BasicSalary Is Nothing Then

            record.ErrorMessage = "Basic salary cannot be empty."
            rejectedRecords.Add(record)
            Return False
        End If

        Return True

    End Function

    Private Sub UpdateStatusLabel(errorCount As Integer)
        If errorCount > 0 Then

            If errorCount = 1 Then
                lblStatus.Text = "There is 1 error."
            Else
                lblStatus.Text = $"There are {errorCount} errors."

            End If

            lblStatus.Text += " Failed records will not be saved."
            lblStatus.BackColor = Color.Red
        Else
            lblStatus.Text = "No errors found."
            lblStatus.BackColor = Color.Green
        End If
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        'TODO: there Is a database error when Unique Constraint is violated.
        'We should add a proper error message when that happens

        If _salaries Is Nothing Then
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Import Salary",
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of ISalaryDataService)
                Await dataService.SaveManyAsync(_salaries.ToList(), z_User)

                Me.IsSaved = True
            End Function)

        Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Close()
    End Sub

    ''' <summary>
    ''' ViewModel that bounds the salary entites to the data grid view
    ''' </summary>
    Private Class SalaryViewModel

        Private ReadOnly _salary As Salary

        Private ReadOnly _employee As Employee

        Public Sub New(salary As Salary, employee As Employee)
            _salary = salary
            _employee = employee
        End Sub

        Public ReadOnly Property EmployeeNo As String
            Get
                Return _employee.EmployeeNo
            End Get
        End Property

        Public ReadOnly Property FullName As String
            Get
                Return _employee.FullNameWithMiddleInitialLastNameFirst
            End Get
        End Property

        Public ReadOnly Property EffectiveFrom As Date
            Get
                Return _salary.EffectiveFrom
            End Get
        End Property

        Public ReadOnly Property BasicSalary As Decimal
            Get
                Return _salary.BasicSalary
            End Get
        End Property

        Public ReadOnly Property AllowanceSalary As Decimal
            Get
                Return _salary.AllowanceSalary
            End Get
        End Property

        Public ReadOnly Property SSSAuto As Boolean
            Get
                Return _salary.DoPaySSSContribution
            End Get
        End Property
        Public ReadOnly Property HDMF_Auto As Boolean
            Get
                Return _salary.AutoComputeHDMFContribution
            End Get
        End Property
        Public ReadOnly Property PhilhealthAuto As Boolean
            Get
                Return _salary.AutoComputePhilHealthContribution
            End Get
        End Property
        Public ReadOnly Property Philhealth As Decimal
            Get
                Return _salary.PhilHealthDeduction
            End Get
        End Property
        Public ReadOnly Property HDMF As Decimal
            Get
                Return _salary.HDMFAmount
            End Get
        End Property

    End Class

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click
        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.Salary)
    End Sub

End Class
