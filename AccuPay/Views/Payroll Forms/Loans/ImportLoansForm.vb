Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Interfaces.Excel
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Helpers
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class ImportLoansForm

    Private Const FormEntityName As String = "Loan"

    Private _loans As List(Of LoanSchedule)

    Private _deductionSchedulesList As List(Of String)

    Private _loanTypeList As List(Of Product)

    Public IsSaved As Boolean

    Private _employeeRepository As EmployeeRepository

    Private _listOfValueRepository As ListOfValueRepository

    Private _loanService As LoanDataService

    Private _productRepository As ProductRepository

    Private _userActivityRepository As UserActivityRepository

    Sub New()

        InitializeComponent()

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _listOfValueRepository = MainServiceProvider.GetRequiredService(Of ListOfValueRepository)

        _loanService = MainServiceProvider.GetRequiredService(Of LoanDataService)

        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

    Private Async Sub ImportLoansForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        Me._deductionSchedulesList = _listOfValueRepository.
            ConvertToStringList(Await _listOfValueRepository.GetDeductionSchedulesAsync())

        Me._loanTypeList = New List(Of Product) _
                (Await _productRepository.GetLoanTypesAsync(z_OrganizationID))

        LoansDataGrid.AutoGenerateColumns = False
        RejectedRecordsGrid.AutoGenerateColumns = False

        SaveButton.Enabled = False

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

        Dim records As New List(Of LoanRowRecord)
        Dim parser = MainServiceProvider.GetRequiredService(Of IExcelParser(Of LoanRowRecord))

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
            Sub()
                records = parser.Read(fileName).ToList
            End Sub)

        If parsedSuccessfully = False Then Return

        _loans = New List(Of LoanSchedule)

        Dim acceptedRecords As New List(Of LoanRowRecord)
        Dim rejectedRecords As New List(Of LoanRowRecord)

        For Each record In records

            'TODO: this is an N+1 query problem. Refactor this
            Dim employee = Await _employeeRepository.GetByEmployeeNumberAsync(record.EmployeeNumber, z_OrganizationID)

            If employee?.RowID Is Nothing Then

                record.ErrorMessage = "Employee number does not exists in the database."

                rejectedRecords.Add(record)

                Continue For
            End If

            'For displaying on datagrid view; placed here in case record is rejected soon
            record.EmployeeFullName = employee.FullNameWithMiddleInitialLastNameFirst
            record.EmployeeNumber = employee.EmployeeNo

            Dim loan = Await ConvertToLoan(record, employee.RowID.Value)

            If loan Is Nothing Then

                If String.IsNullOrWhiteSpace(record.ErrorMessage) Then
                    record.ErrorMessage = "Cannot parse data."

                End If
                rejectedRecords.Add(record)
                Continue For
            End If

            Dim validationErrorMessage = ValidateLoan(loan)

            If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then

                record.ErrorMessage = validationErrorMessage
                rejectedRecords.Add(record)
                Continue For
            End If

            _loans.Add(loan)
            acceptedRecords.Add(record)
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({acceptedRecords.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _loans.Count > 0

        LoansDataGrid.DataSource = acceptedRecords
        RejectedRecordsGrid.DataSource = rejectedRecords

    End Sub

    Private Function ValidateLoan(loan As LoanSchedule) As String

        If loan.Status = LoanSchedule.STATUS_COMPLETE Then Return "Loan schedule is already completed."

        If loan.OrganizationID Is Nothing Then Return "Organization is required."

        If loan.EmployeeID Is Nothing Then Return "Employee is required."

        If loan.LoanTypeID Is Nothing Then Return "Loan type is required."

        If loan.TotalLoanAmount < 0 Then Return "Total loan amount cannot be less than 0."

        If loan.DeductionAmount < 0 Then Return "Deduction amount cannot be less than 0."

        If String.IsNullOrWhiteSpace(loan.DeductionSchedule) Then Return "Deduction schedule is required."

        Return Nothing
    End Function

    Private Async Function ConvertToLoan(record As LoanRowRecord, employeeId As Integer) As Task(Of LoanSchedule)

        If record.StartDate Is Nothing Then

            record.ErrorMessage = "Start Date cannot be empty."
            Return Nothing
        End If

        If record.StartDate < Data.Helpers.PayrollTools.SqlServerMinimumDate Then

            record.ErrorMessage = "Dates cannot be earlier than January 1, 1753."
            Return Nothing
        End If

        'Type
        If String.IsNullOrWhiteSpace(record.LoanName) Then

            record.ErrorMessage = "Loan Type/Name cannot be blank."
            Return Nothing
        End If

        Dim loanType = Await Me._productRepository.GetOrCreateLoanTypeAsync(
            record.LoanName,
            organizationId:=z_OrganizationID,
            userId:=z_User)

        If loanType Is Nothing Then

            record.ErrorMessage = "Cannot get or create loan type. Please contact " & My.Resources.AppCreator
            Return Nothing

        End If

        record.LoanName = loanType.PartNo
        record.LoanType = loanType

        'Total Balance
        If record.TotalBalanceLeft Is Nothing Then

            record.ErrorMessage = "Loan balance cannot be empty."
            Return Nothing
        End If

        'Deduction Schedule
        Dim deductionSchedule = Me._deductionSchedulesList.
            FirstOrDefault(Function(d) d.Equals(record.DeductionSchedule, StringComparison.InvariantCultureIgnoreCase))

        If deductionSchedule Is Nothing Then

            record.ErrorMessage = "Selected Deduction frequency is not in the choices."
            Return Nothing

        End If

        record.DeductionSchedule = deductionSchedule

        'Deduction Amount
        If record.TotalLoanAmount Is Nothing Then

            record.ErrorMessage = "Total loan amount cannot be blank."
            Return Nothing

        End If

        'Amount
        If record.DeductionAmount Is Nothing Then

            record.ErrorMessage = "Deduction amount cannot be blank."
            Return Nothing

        End If

        Return record.ToLoan(employeeId)

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

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Me.Cursor = Cursors.WaitCursor

        Dim messageTitle = "Import Loans"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Await _loanService.SaveManyAsync(_loans)

                Dim importList = New List(Of UserActivityItem)
                For Each item In _loans

                    Dim suffixIdentifier = $"with type '{item.LoanName}' and start date '{item.DedEffectiveDateFrom.ToShortDateString()}'."

                    importList.Add(New UserActivityItem() With
                    {
                        .Description = $"Created a new loan {suffixIdentifier}",
                        .EntityId = item.RowID.Value,
                        .ChangedEmployeeId = item.EmployeeID.Value
                    })
                Next

                _userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeImport, importList)

                Me.IsSaved = True

                Me.Close()
            End Function)

        Me.Cursor = Cursors.Default

    End Sub

    Private Async Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        Dim fileInfo = Await DownloadTemplateHelper.DownloadExcelWithData(ExcelTemplates.Loan)

        If fileInfo Is Nothing Then Return

        Using package As New ExcelPackage(fileInfo)
            Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets("Options")
            Dim loanTypes = _productRepository.
                ConvertToStringList(Me._loanTypeList).
                OrderBy(Function(t) t).
                ToList()

            For index = 0 To loanTypes.Count - 1
                worksheet.Cells(index + 2, 2).Value = loanTypes(index)
            Next

            package.Save()

            Process.Start(fileInfo.FullName)
        End Using
    End Sub

End Class