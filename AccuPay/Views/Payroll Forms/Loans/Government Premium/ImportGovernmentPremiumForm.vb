Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.AccuPay.Desktop.Helpers
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Core.Services.Imports.Policy
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportGovernmentPremiumForm
    Private Const _loanTypeGrouping As LoanTypeGroupingEnum = LoanTypeGroupingEnum.Government
    Private _loans As List(Of Loan)

    Private _deductionSchedulesList As List(Of String)

    Private _loanTypeList As List(Of Product)
    Private _organization As Organization
    Private _currentSystemOwner As SystemOwner
    Public IsSaved As Boolean

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Private ReadOnly _listOfValueRepository As IListOfValueRepository

    Private ReadOnly _productRepository As IProductRepository
    Private ReadOnly _organizationRepository As IOrganizationRepository
    Private ReadOnly _payPeriodRepository As IPayPeriodRepository
    Private ReadOnly _systemOwnerService As ISystemOwnerService
    Private ReadOnly _importPolicy As ImportPolicy
    Private ReadOnly _loanService As ILoanDataService

    Sub New()

        InitializeComponent()

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _listOfValueRepository = MainServiceProvider.GetRequiredService(Of IListOfValueRepository)

        _loanService = MainServiceProvider.GetRequiredService(Of ILoanDataService)

        _productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)

        _organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of ISystemOwnerService)

        Dim policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _importPolicy = policyHelper.ImportPolicy
    End Sub

    Private Async Sub ImportGovernmentPremiumForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _organization = Await _organizationRepository.GetByIdWithAddressAsync(z_OrganizationID)
        _currentSystemOwner = Await _systemOwnerService.GetCurrentSystemOwnerEntityAsync()

        Me.IsSaved = False

        Me._deductionSchedulesList = _listOfValueRepository.
            ConvertToStringList(Await _listOfValueRepository.GetDeductionSchedulesAsync())
        If _organization.IsWeekly Then
            Me._deductionSchedulesList = LookUpStringItem.
                Convert(New List(Of String), hasDefaultItem:=True, ContributionSchedule.PER_PAY_PERIOD).
                Select(Function(t) t.Item).
                ToList()
        End If

        If Not _importPolicy.IsOpenToAllImportMethod Then
            Me._loanTypeList = New List(Of Product) _
                (Await _productRepository.GetLoanTypesAsync(organizationId:=z_OrganizationID, _loanTypeGrouping))
        ElseIf _importPolicy.IsOpenToAllImportMethod Then
            Me._loanTypeList = New List(Of Product) _
                (Await _productRepository.GetLoanTypesAllOrganizationsAsync(_loanTypeGrouping))
        End If

        LoansDataGrid.AutoGenerateColumns = False
        RejectedRecordsGrid.AutoGenerateColumns = False

        SaveButton.Enabled = False

    End Sub

    Private Async Function GetPayPeriods(year As Integer) As Task(Of ICollection(Of PayPeriod))
        If _currentSystemOwner.IsMorningSun AndAlso
            _organization.IsWeekly Then

            Return Await _payPeriodRepository.GetYearlyPayPeriodsOfWeeklyAsync(
                organization:=_organization,
                year:=year,
                currentUserId:=z_User)
        End If

        Return Await _payPeriodRepository.GetYearlyPayPeriodsAsync(
                organizationId:=z_OrganizationID,
                year:=year,
                currentUserId:=z_User)
    End Function

    Private Async Sub BrowseButton_Click(sender As Object, e As EventArgs) Handles BrowseButton.Click
        Dim browseFile = New OpenFileDialog With {
            .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"
        }

        If browseFile.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim selectMonthForm As New selectMonth
        Dim selectedMonthDate As Date = DateTime.Now
        If selectMonthForm.ShowDialog() = DialogResult.OK Then
            selectedMonthDate = CType(selectMonthForm.MonthValue, Date)
        Else
            Return
        End If

        Dim fileName = browseFile.FileName

        Dim records As New List(Of GovernmentPremiumRowRecord)
        Dim parser = MainServiceProvider.GetRequiredService(Of IExcelParser(Of GovernmentPremiumRowRecord))

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
            Sub()
                records = parser.Read(fileName).ToList
            End Sub)

        If parsedSuccessfully = False Then Return

        _loans = New List(Of Loan)

        Dim acceptedRecords As New List(Of GovernmentPremiumRowRecord)
        Dim rejectedRecords As New List(Of GovernmentPremiumRowRecord)

        Dim productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)

        Dim payPeriodsOfMonth = (Await GetPayPeriods(year:=selectedMonthDate.Year)).
            Where(Function(p) p.Month = selectedMonthDate.Month).
            ToList()
        Dim payPeriods = payPeriodsOfMonth.
            OrderBy(Function(p) p.PayFromDate).
            ThenBy(Function(p) p.PayToDate).
            ToList()
        Dim startDate = payPeriods.FirstOrDefault().PayFromDate

        Dim loanTypes As ICollection(Of Product) = New List(Of Product)

        If Not _importPolicy.IsOpenToAllImportMethod Then
            loanTypes = Await productRepository.GetLoanTypesAsync(z_OrganizationID, _loanTypeGrouping)
            Await StandardImport(records, rejectedRecords, loanTypes, payPeriods, startDate)
        ElseIf _importPolicy.IsOpenToAllImportMethod Then
            loanTypes = Await productRepository.GetLoanTypesAllOrganizationsAsync(_loanTypeGrouping)
            Await StandardImport2(records, rejectedRecords, loanTypes, payPeriods, startDate)
        End If

        Dim rejectedRecordLineNumbers = rejectedRecords.Select(Function(r) r.LineNumber).ToArray()

        acceptedRecords.AddRange(records.Where(Function(r) Not rejectedRecordLineNumbers.Contains(r.LineNumber)).ToList())

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({acceptedRecords.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = acceptedRecords.Count > 0

        LoansDataGrid.DataSource = acceptedRecords
        RejectedRecordsGrid.DataSource = rejectedRecords

    End Sub

    Private Async Function StandardImport2(records As List(Of GovernmentPremiumRowRecord),
        rejectedRecords As List(Of GovernmentPremiumRowRecord),
        loanTypes As ICollection(Of Product),
        payPeriods As List(Of PayPeriod),
        startDate As Date) As Task

        Dim employeeIdList = records.Select(Function(t) t.EmployeeRowId).ToArray()
        Dim employees = (Await _employeeRepository.GetByMultipleIdAsync(employeeIdList:=employeeIdList)).ToList()

        For Each record In records
            Dim employee = employees.FirstOrDefault(Function(e) Integer.Equals(e.RowID, record.EmployeeRowId))

            If employee?.RowID Is Nothing Then

                record.ErrorMessage = "Employee number does not exists in the database."

                rejectedRecords.Add(record)

                Continue For
            End If

            'For displaying on datagrid view; placed here in case record is rejected soon
            record.EmployeeFullName = employee.FullNameWithMiddleInitialLastNameFirst
            record.EmployeeNumber = employee.EmployeeNo

            Dim convertedLoans = ConvertToLoans(rejectedRecords:=rejectedRecords,
                record,
                employee:=employee,
                loanTypes:=loanTypes,
                startDate:=startDate,
                payPeriods:=payPeriods)
            If rejectedRecords.Any(Function(r) r.LineNumber = record.LineNumber) Then Continue For
            For Each loan In convertedLoans
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
            Next
        Next
    End Function

    Private Async Function StandardImport(records As List(Of GovernmentPremiumRowRecord),
        rejectedRecords As List(Of GovernmentPremiumRowRecord),
        loanTypes As ICollection(Of Product),
        payPeriods As List(Of PayPeriod),
        startDate As Date) As Task

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

            Dim convertedLoans = ConvertToLoans(rejectedRecords:=rejectedRecords,
                record,
                employee:=employee,
                loanTypes:=loanTypes,
                startDate:=startDate,
                payPeriods:=payPeriods)
            If rejectedRecords.Any(Function(r) r.LineNumber = record.LineNumber) Then Continue For
            For Each loan In convertedLoans
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
            Next
        Next
    End Function

    Private Function ValidateLoan(loan As Loan) As String

        If loan.Status = Loan.STATUS_COMPLETE Then Return "Loan schedule is already completed."

        If loan.OrganizationID Is Nothing Then Return "Organization is required."

        If loan.EmployeeID Is Nothing Then Return "Employee is required."

        If loan.LoanTypeID Is Nothing Then Return "Loan type is required."

        If loan.TotalLoanAmount < 0 Then Return "Total loan amount cannot be less than 0."

        If loan.DeductionAmount < 0 Then Return "Deduction amount cannot be less than 0."

        If String.IsNullOrWhiteSpace(loan.DeductionSchedule) Then Return "Deduction schedule is required."

        Return Nothing
    End Function

    Private Function ConvertToLoans(ByRef rejectedRecords As List(Of GovernmentPremiumRowRecord),
        record As GovernmentPremiumRowRecord,
        employee As Employee,
        loanTypes As ICollection(Of Product),
        startDate As Date,
        payPeriods As List(Of PayPeriod)) As List(Of Loan)

        Dim collectedErrorMessage As New List(Of String)

        If If(record.SssEcEmployerShare, 0) <= -1 Then
            collectedErrorMessage.Add("Leave SSS EC Employer Share as blank or zero(0")
        End If

        If If(record.SssWispEmployeeShare, 0) <= -1 Then
            collectedErrorMessage.Add("Leave SSS WISP Employee Share as blank or zero(0")
        End If

        If If(record.SssWispEmployerShare, 0) <= -1 Then
            collectedErrorMessage.Add("Leave SSS WISP Employer Share as blank or zero(0")
        End If

        Dim validSss = If(record.SssEmployeeShare, 0) >= 0 AndAlso If(record.SssEmployerShare, 0) >= 0

        Dim validPhilHealth = If(record.PhilHealthEmployeeShare, 0) >= 0 AndAlso If(record.PhilHealthEmployerShare, 0) >= 0

        Dim validHdmf = If(record.HdmfEmployeeShare, 0) >= 0 AndAlso If(record.HdmfEmployerShare, 0) >= 0

        If Not validSss And
            (validPhilHealth Or validHdmf) Then
            collectedErrorMessage.Add("Both SSS Employee & Employer Share should have value")
        End If

        If Not validPhilHealth And
            (validSss Or validHdmf) Then
            collectedErrorMessage.Add("Both PhilHealth Employee & Employer Share should have value")
        End If

        If Not validHdmf And
            (validSss Or validPhilHealth) Then
            collectedErrorMessage.Add("Both HDMF Employee & Employer Share should have value")
        End If

        If collectedErrorMessage.Any() Then
            record.ErrorMessage = String.Concat(record.ErrorMessage, String.Join("; ", collectedErrorMessage.ToArray()))
            rejectedRecords.Add(record)
        End If

        Return record.ToLoans(employee:=employee, loanTypes:=loanTypes, startDate:=startDate, payPeriods:=payPeriods)
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
                Await _loanService.SaveManyAsync(_loans, z_User)

                Me.IsSaved = True

                Me.Close()
            End Function)

        Me.Cursor = Cursors.Default

    End Sub

    Private Async Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        Dim fileInfo = Await DownloadTemplateHelper.DownloadExcelWithData(ExcelTemplates.GovernmentPremium)

        If fileInfo Is Nothing Then Return

        'Using package As New ExcelPackage(fileInfo)
        '    Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets("Sheet1")
        '    Dim validation = worksheet.DataValidations.AddListValidation("A2:A999")

        '    validation.ErrorStyle = ExcelDataValidationWarningStyle.stop
        '    validation.ErrorTitle = "Invalid Employee ID"
        '    validation.Error = "Employee ID doesn't exists."
        '    validation.ShowErrorMessage = True
        '    validation.Formula.ExcelFormula = "Options!$A$2:$A$999"

        '    package.Save()

        Process.Start(fileInfo.FullName)
        'End Using
    End Sub

End Class
