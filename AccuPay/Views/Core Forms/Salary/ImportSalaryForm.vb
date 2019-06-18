Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Utils
Imports Globagility.AccuPay
Imports Globagility.AccuPay.Government
Imports Globagility.AccuPay.Salaries
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class ImportSalaryForm

    Private _salaries As IList(Of Salary)

    Public Property IsSaved As Boolean

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Me.IsSaved = False

    End Sub

    Private Sub BrowseButton_Click(sender As Object, e As EventArgs) Handles BrowseButton.Click
        Dim browseFile = New OpenFileDialog With {
            .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"
        }

        If browseFile.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim fileName = browseFile.FileName

        Dim records As New List(Of SalaryRowRecord)
        Dim parser = New ExcelParser(Of SalaryRowRecord)("Employee Salary")

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunctionAsync(
                                Sub()
                                    records = parser.Read(fileName).ToList
                                End Sub)

        If parsedSuccessfully = False Then Return

        If records Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read the template.")

            Return
        End If

        Dim rejectedRecords As New List(Of SalaryRowRecord)

        Dim salaryViewModels = New List(Of SalaryViewModel)
        _salaries = New List(Of Salary)

        Using context = New PayrollContext()
            For Each record In records

                Dim employee = context.Employees.
                    FirstOrDefault(Function(t) CBool(t.EmployeeNo = record.EmployeeNo AndAlso t.OrganizationID = z_OrganizationID))

                If employee Is Nothing Then
                    record.ErrorMessage = "Employee does not exist!"
                    rejectedRecords.Add(record)
                    Continue For
                End If

                Dim lastSalary = context.Salaries.
                    Where(Function(s) Nullable.Equals(employee.RowID, s.EmployeeID)).
                    OrderByDescending(Function(s) s.EffectiveTo).
                    FirstOrDefault

                Dim doPaySSSContribution = True

                If lastSalary IsNot Nothing Then
                    doPaySSSContribution = lastSalary.DoPaySSSContribution
                End If

                If CheckIfRecordIsValid(record, rejectedRecords) = False Then

                    Continue For

                End If

                Dim salary = New Salary With {
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .EmployeeID = employee.RowID,
                    .PositionID = employee.PositionID,
                    .EffectiveFrom = record.EffectiveFrom.Value,
                    .EffectiveTo = record.EffectiveTo,
                    .BasicSalary = record.BasicSalary.Value,
                    .AllowanceSalary = record.AllowanceSalary,
                    .DoPaySSSContribution = If(lastSalary?.DoPaySSSContribution, True)
                }

                _salaries.Add(salary)
                salaryViewModels.Add(New SalaryViewModel(salary, employee))
            Next
        End Using

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({salaryViewModels.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _salaries.Count > 0

        SalaryDataGrid.DataSource = salaryViewModels
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

        Try

            Using context = New PayrollContext()
                Dim philHealthBrackets = Await context.PhilHealthBrackets.ToListAsync()
                Dim philHealthConfig = Await context.ListOfValues.Where(Function(t) t.Type = "PhilHealth").ToListAsync()
                Dim philHealthCalculator = New PhilHealthCalculator(philHealthConfig, philHealthBrackets)

                For Each salary In _salaries
                    Dim employee = Await context.Employees.
                        FirstOrDefaultAsync(Function(emp) Nullable.Equals(emp.RowID, salary.EmployeeID))

                    salary.TotalSalary = salary.BasicSalary + salary.AllowanceSalary

                    Dim monthlyRate = 0D
                    If employee.EmployeeType = "Monthly" Or employee.EmployeeType = "Fixed" Then
                        monthlyRate = salary.BasicSalary
                    ElseIf employee.EmployeeType = "Daily" Then
                        Dim workDaysPerMonth = PayrollTools.GetWorkDaysPerMonth(employee.WorkDaysPerYear)

                        monthlyRate = salary.BasicSalary * workDaysPerMonth
                    End If

                    salary.PhilHealthDeduction = philHealthCalculator.Calculate(monthlyRate)

                    ' Set the pagibig amount to the default
                    salary.HDMFAmount = 100D

                    context.Salaries.Add(salary)
                Next

                Await context.SaveChangesAsync()

                Me.IsSaved = True

            End Using
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage("Import Salary", ex)
        End Try

        Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
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

        Public ReadOnly Property EffectiveTo As Date?
            Get
                Return _salary.EffectiveTo
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

    End Class

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click
        DownloadTemplateHelper.Download(ExcelTemplates.Salary)
    End Sub

End Class