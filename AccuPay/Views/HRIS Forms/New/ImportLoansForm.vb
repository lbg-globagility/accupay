Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Loans
Imports AccuPay.Repository
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports Globagility.AccuPay
Imports Globagility.AccuPay.Loans
Imports OfficeOpenXml

Public Class ImportLoansForm

    Private _loans As List(Of LoanSchedule)

    Private _employeeRepository As New EmployeeRepository

    Private _productRepository As New ProductRepository

    Private _loanScheduleRepository As New LoanScheduleRepository

    Private _listOfValueRepository As New ListOfValueRepository

    Private _deductionSchedulesList As List(Of String)

    Private _loanTypeList As List(Of Product)

    Public IsSaved As Boolean

    Private Async Sub ImportLoansForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        Me._deductionSchedulesList = _listOfValueRepository.
            ConvertToStringList(Await _listOfValueRepository.GetDeductionSchedules())

        Me._loanTypeList = New List(Of Product) _
                (Await _productRepository.GetLoanTypes())

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
        Dim parser = New ExcelParser(Of LoanRowRecord)()

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunctionAsync(
            Sub()
                records = parser.Read(fileName).ToList
            End Sub)

        If parsedSuccessfully = False Then Return

        _loans = New List(Of LoanSchedule)

        Dim rejectedRecords As New List(Of LoanRowRecord)

        For Each record In records

            Dim employee = Await _employeeRepository.GetByEmployeeNumberAsync(record.EmployeeNumber)

            If employee Is Nothing Then

                record.ErrorMessage = "Employee number does not exists in the database."

                rejectedRecords.Add(record)

                Continue For
            End If

            If String.IsNullOrWhiteSpace(record.LoanName) Then

                record.ErrorMessage = "Loan Type/Name cannot be blank."

                rejectedRecords.Add(record)

                Continue For
            End If

            Dim loanType = Await Me._productRepository.GetOrCreateLoanType(record.LoanName)

            If loanType Is Nothing Then

                record.ErrorMessage = "Cannot get or create loan type. Please contact " & My.Resources.AppCreator

                rejectedRecords.Add(record)

                Continue For

            End If

            If CheckIfRecordIsValid(record, rejectedRecords) = False Then

                Continue For

            End If

            Dim deductionSchedule = Me._deductionSchedulesList.
                FirstOrDefault(Function(d) d.Equals(record.DeductionSchedule, StringComparison.InvariantCultureIgnoreCase))

            If deductionSchedule Is Nothing Then

                record.ErrorMessage = "Selected Deduction frequency is not in the choices."

                rejectedRecords.Add(record)

                Continue For

            End If

            Dim loanSchedule = New LoanSchedule With {
                .RowID = Nothing,
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .EmployeeID = employee.RowID,
                .Employee = employee,
                .LoanNumber = record.LoanNumber,
                .Comments = record.Comments,
                .TotalLoanAmount = record.TotalLoanAmount,
                .TotalBalanceLeft = record.TotalBalanceLeft,
                .DedEffectiveDateFrom = record.DedEffectiveDateFrom,
                .DeductionAmount = record.DeductionAmount,
                .DeductionPercentage = 0,
                .LoanName = record.LoanName,
                .LoanTypeID = loanType.RowID,
                .Status = LoanScheduleRepository.STATUS_IN_PROGRESS,
                .DeductionSchedule = deductionSchedule,
                .NoOfPayPeriod = Me._loanScheduleRepository.ComputeNumberOfPayPeriod(record.TotalLoanAmount, record.DeductionAmount),
                .LoanPayPeriodLeft = Me._loanScheduleRepository.ComputeNumberOfPayPeriod(record.TotalBalanceLeft, record.DeductionAmount)
            }

            _loans.Add(loanSchedule)
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({Me._loans.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _loans.Count > 0

        LoansDataGrid.DataSource = _loans
        RejectedRecordsGrid.DataSource = rejectedRecords

    End Sub

    Private Function CheckIfRecordIsValid(record As LoanRowRecord, rejectedRecords As List(Of LoanRowRecord)) As Boolean

        If record.DedEffectiveDateFrom Is Nothing Then

            record.ErrorMessage = "Start date cannot be empty."
            rejectedRecords.Add(record)
            Return False
        End If

        If record.TotalLoanAmount Is Nothing Then

            record.ErrorMessage = "Total loan amount cannot be empty."
            rejectedRecords.Add(record)
            Return False
        End If

        If record.TotalBalanceLeft Is Nothing Then

            record.ErrorMessage = "Loan balance cannot be empty."
            rejectedRecords.Add(record)
            Return False
        End If

        If record.DeductionAmount Is Nothing Then

            record.ErrorMessage = "Deduction amount cannot be empty."
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

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Me.Cursor = Cursors.WaitCursor

        Dim messageTitle = "Import Loans"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim loansWithOutEmployeeObject = _loans.CloneListJson()

                For Each loan In loansWithOutEmployeeObject
                    loan.Employee = Nothing
                Next

                Await _loanScheduleRepository.SaveManyAsync(loansWithOutEmployeeObject, Me._loanTypeList)

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