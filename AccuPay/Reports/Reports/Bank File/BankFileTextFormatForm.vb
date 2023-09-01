Option Strict On

Imports System.IO
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class BankFileTextFormatForm
    Private Const THOUSAND_VALUE As Integer = 1000
    Private ReadOnly DATETIME_PICKER_MINDATE As Date = New Date(1753, 1, 1)
    Private ReadOnly DATETIME_PICKER_MAXDATE As Date = New Date(9998, 12, 31)
    Private ReadOnly _paystubRepository As IPaystubRepository
    Private ReadOnly _payPeriodRepository As IPayPeriodRepository
    Private ReadOnly _organizationRepository As IOrganizationRepository
    Private _organization As Organization
    Private bankFileHeaderDataManager As BankFileHeaderDataManager = New BankFileHeaderDataManager(z_OrganizationID)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _paystubRepository = MainServiceProvider.GetRequiredService(Of IPaystubRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)

    End Sub

    Private Async Sub BankFileTextFormatForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gridPayroll.AutoGenerateColumns = False
        numCompanyCode.Controls(0).Hide()
        numFundingAccountNo.Controls(0).Hide()
        numCeilingAmount.Controls(0).Hide()
        numPresentingOffice.Controls(0).Hide()
        numBatchNo.Controls(0).Hide()
        chkSelectAll.ThreeState = True

        Dim bankFileHeaderModel = bankFileHeaderDataManager.GetBankFileHeaderModel
        If bankFileHeaderModel IsNot Nothing Then
            numCompanyCode.Text = bankFileHeaderModel.CompanyCode
            numFundingAccountNo.Text = bankFileHeaderModel.FundingAccountNo
            numPresentingOffice.Text = bankFileHeaderModel.PresentingOfficeNo
            numBatchNo.Text = bankFileHeaderModel.BatchNo
        End If

        _organization = Await _organizationRepository.GetByIdWithAddressAsync(z_OrganizationID)

        Dim payPeriod = Await _payPeriodRepository.GetCurrentOpenAsync(organization:=_organization)

        If payPeriod Is Nothing Then
            Dim payPeriods = Await _payPeriodRepository.GetYearlyPayPeriodsOfWeeklyAsync(organization:=_organization, year:=Date.Now.Year, currentUserId:=z_User)
            payPeriod = payPeriods.
                Where(Function(p) p.PayFromDate <= Date.Now.Date).
                Where(Function(p) p.PayToDate >= Date.Now.Date).
                FirstOrDefault()
        End If
        dtpPayrollDate.Value = payPeriod.PayFromDate.Date
        dtpPayrollDate.MinDate = payPeriod.PayFromDate.Date
        dtpPayrollDate.MaxDate = payPeriod.PayToDate.Date

        Await LoadPaystubs(payPeriod)

        AddHandler chkSelectAll.CheckedChanged, AddressOf chkSelectAll_CheckedChanged

        chkSelectAll.Checked = True
        chkSelectAll.CheckState = CheckState.Checked

        'AddHandler gridPayroll.CellValueChanged, AddressOf gridPayroll_CellValueChanged
    End Sub

    Private Async Function LoadPaystubs(payPeriod As PayPeriod) As Task
        Dim paystubs = Enumerable.Empty(Of Paystub)
        If payPeriod.RowID IsNot Nothing Then
            paystubs = Await _paystubRepository.GetByPayPeriodWithEmployeesOfSamePayFrequencyAsync(payPeriodId:=payPeriod.RowID.Value)
        End If

        Dim models = paystubs.Select(Function(p) New BankFileModel(p)).
            OrderBy(Function(t) t.CompanyName).
            ThenBy(Function(t) String.Concat(t.LastName, t.FirstName)).
            ToList()

        Dim summaryModel As BankFileModel = BankFileModel.BankFileModelSummary(models:=models)

        Dim append = New List(Of BankFileModel) From {summaryModel}
        models = models.Concat(append).ToList()

        gridPayroll.DataSource = models
    End Function

    Private Sub chkSelectAll_CheckedChanged_1(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged

    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs)
        RemoveHandler gridPayroll.CellValueChanged, AddressOf gridPayroll_CellValueChanged
        Dim rows = gridPayroll.Rows.OfType(Of DataGridViewRow).ToList()

        If Not chkSelectAll.CheckState = CheckState.Indeterminate Then
            For Each item In rows
                item.Cells(Column1.Name).Value = chkSelectAll.Checked
            Next
        End If

        UpdateCeiling()
        UpdateTriStateCheckBox()
        AddHandler gridPayroll.CellValueChanged, AddressOf gridPayroll_CellValueChanged
    End Sub

    Private Sub UpdateCeiling()
        Dim rows = gridPayroll.Rows.OfType(Of DataGridViewRow)
        Dim models = rows.
            Select(Function(r) DirectCast(r.DataBoundItem, BankFileModel)).
            Where(Function(p) p.IsSelected).
            ToList()

        Dim amountList = models?.Select(Function(p) p.Amount)
        Dim given = Math.Ceiling(If(amountList.Any(), amountList.Max(), 0))
        Dim caught = THOUSAND_VALUE - (given Mod THOUSAND_VALUE)
        Dim result = given + caught
        numCeilingAmount.Value = result
        If result <= THOUSAND_VALUE Then numCeilingAmount.Value = 0
    End Sub

    Private Sub UpdateTriStateCheckBox()
        Dim rows = gridPayroll.Rows.OfType(Of DataGridViewRow)
        Dim models = rows.
            Select(Function(r) DirectCast(r.DataBoundItem, BankFileModel)).
            Where(Function(p) p.IsSelected).
            ToList()

        Dim totalCount = rows.Count - 1
        Dim selectedCount = models.Count
        chkSelectAll.Text = $"Select All ({selectedCount}/{totalCount})"
        If Not selectedCount = 0 AndAlso selectedCount < totalCount Then
            chkSelectAll.CheckState = CheckState.Indeterminate
        ElseIf Not selectedCount = 0 AndAlso selectedCount = totalCount Then
            chkSelectAll.CheckState = CheckState.Checked
        ElseIf selectedCount = 0 Then
            chkSelectAll.CheckState = CheckState.Unchecked
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not numCompanyCode.Value > 0 OrElse Not numCompanyCode.Value.ToString("00000").Length = 5 Then
            ValidationMessagePrompt($"Invalid Company Code.{Environment.NewLine}It is the 5 digit code.")
            numCompanyCode.Focus()
            Return
        End If

        If Not numFundingAccountNo.Value > 0 OrElse Not numFundingAccountNo.Value.ToString("0000000000").Length = 10 Then
            ValidationMessagePrompt($"Invalid Funding Account No.{Environment.NewLine}It is the 10 digit code.")
            numFundingAccountNo.Focus()
            Return
        End If

        If Not numPresentingOffice.Value > 0 OrElse Not numPresentingOffice.Value.ToString("000").Length = 3 Then
            ValidationMessagePrompt($"Invalid Presenting Office.{Environment.NewLine}It is the 3 digit code.")
            numPresentingOffice.Focus()
            Return
        End If

        If Not numBatchNo.Value > 0 OrElse Not numBatchNo.Value.ToString("00").Length = 2 Then
            ValidationMessagePrompt($"Invalid Batch No.{Environment.NewLine}It is the 2 digit code.")
            numBatchNo.Focus()
            Return
        End If

        Dim models = gridPayroll.Rows.OfType(Of DataGridViewRow).
            Select(Function(r) DirectCast(r.DataBoundItem, BankFileModel)).
            Where(Function(p) p.IsSelected).
            ToList()

        Dim header As String = GetHeader(models:=models)

        Dim companyCode = numCompanyCode.Value.ToString("00000")
        Dim payrollDate = dtpPayrollDate.Value.ToString("MMddyy")
        Dim batchNo = numBatchNo.Value.ToString("00")
        Dim employeeDetails = models.
            Select(Function(t) t.GetDetails(companyCode:=companyCode, payrollDate:=payrollDate, batchNo:=batchNo)).
            ToArray()
        Dim details = String.Join(Environment.NewLine, employeeDetails)

        Dim trailer As String = GetTrailer(models:=models)

        Dim defaultFileName = $"{companyCode}-{payrollDate}-{batchNo}.01"
        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName:=defaultFileName,
                                defaultExtension:="01",
                                filter:="Bank File|*.01;")

        If saveFileDialogHelperOutPut.IsSuccess = False Then Return

        Dim pathAndFileName = saveFileDialogHelperOutPut.FileInfo.FullName 'Path.Combine(Path.GetTempPath, )
        Using sw As New StreamWriter(pathAndFileName)
            sw.WriteLine(header)
            sw.WriteLine(details)
            sw.WriteLine(trailer)
        End Using

        bankFileHeaderDataManager.Save(companyCode:=numCompanyCode.Value.ToString("00000"),
            fundingAccountNo:=numFundingAccountNo.Value.ToString("0000000000"),
            presentingOfficeNo:=numPresentingOffice.Value.ToString("000"),
            batchNo:=numBatchNo.Value.ToString("00"))

        Process.Start("explorer.exe", $"/select,""{pathAndFileName}""")
    End Sub

    Private Function GetHeader(models As List(Of BankFileModel)) As String
        Dim totalSummary = Math.Round(models.Sum(Function(p) p.Amount), 2).ToString().Replace(".", String.Empty)
        Return String.Concat("H",
            numCompanyCode.Value.ToString("00000"),
            dtpPayrollDate.Value.ToString("MMddyy"),
            numBatchNo.Value.ToString("00"),
            "1",
            numFundingAccountNo.Value.ToString("0000000000"),
            numPresentingOffice.Value.ToString("000"),
            numCeilingAmount.Value.ToString("0000000000"),
            CInt(totalSummary).ToString("00000000000000"),
            "1",
            Space(75))
    End Function

    Private Shared Sub ValidationMessagePrompt(warningMessage As String)
        MessageBox.Show(warningMessage, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    End Sub

    Private Function GetTrailer(models As List(Of BankFileModel)) As String
        Dim totalSummary = Math.Round(models.Sum(Function(p) p.Amount), 2).ToString().Replace(".", String.Empty)
        Return String.Concat("T",
            numCompanyCode.Value.ToString("00000"),
            dtpPayrollDate.Value.ToString("MMddyy"),
            numBatchNo.Value.ToString("00"),
            "2",
            numFundingAccountNo.Value.ToString("0000000000"),
            models.Sum(Function(p) p.AccountNumberDecimal).ToString("000000000000000"),
            CInt(totalSummary).ToString("000000000000000"),
            models.Sum(Function(p) p.DataHashInt).ToString("000000000000000000"),
            models.Count.ToString("00000"),
            Space(50))
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub gridPayroll_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridPayroll.CellContentClick

    End Sub

    Private Sub gridPayroll_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.ColumnIndex = Column1.Index Then
            UpdateCeiling()
            UpdateTriStateCheckBox()
        End If
    End Sub

    Private Function GetPayrollSelector() As MultiplePayPeriodSelectionDialog
        Dim payrollSelector = New MultiplePayPeriodSelectionDialog With {
            .ShowPayrollSummaryPanel = False,
            .ShowDeclaredOrActualOptionsPanel = False
        }

        If Not payrollSelector.ShowDialog() = DialogResult.OK Then
            Return Nothing
        End If

        Return payrollSelector
    End Function

    Private Async Sub lnkSelectPeriod_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkSelectPeriod.LinkClicked
        Dim payrollSelector = GetPayrollSelector()
        If payrollSelector Is Nothing Then Return

        Dim firstSelectedPeriod = payrollSelector.PayPeriodFrom
        If firstSelectedPeriod Is Nothing Then Return

        dtpPayrollDate.MinDate = DATETIME_PICKER_MINDATE
        dtpPayrollDate.MaxDate = DATETIME_PICKER_MAXDATE

        Dim payPeriod = firstSelectedPeriod
        dtpPayrollDate.Value = payPeriod.PayFromDate.Date
        dtpPayrollDate.MinDate = payPeriod.PayFromDate.Date
        dtpPayrollDate.MaxDate = payPeriod.PayToDate.Date

        chkSelectAll.Checked = False

        Await LoadPaystubs(payPeriod)

        chkSelectAll.Checked = True

    End Sub

    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click

        Dim models = gridPayroll.Rows.OfType(Of DataGridViewRow).
            Select(Function(r) DirectCast(r.DataBoundItem, BankFileModel)).
            Where(Function(p) p.IsSelected).
            ToList()

        Dim groupCompany = models.GroupBy(Function(t) t.CompanyName).ToList()
        Dim companyNames = groupCompany.Select(Function(t) t.FirstOrDefault().CompanyName).ToArray()

        If companyNames.Any() AndAlso companyNames.Count > 1 Then
            Dim form = New BankFileCompanySelector(companyNames:=companyNames)
            If form.ShowDialog() = DialogResult.OK Then
                If Not form.IsAll Then
                    models = models.
                        Where(Function(t) t.CompanyName = form.SelectedCompanyName).
                        ToList()
                End If
            ElseIf form.ShowDialog() = DialogResult.Cancel Then
                Return
            End If
        End If

        Dim now = DateTime.Now.ToString("HHmm")
        Dim payrollDate = dtpPayrollDate.Value.ToString("MMddyy")

        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName:=$"BankFile_{payrollDate}~{now}", ".xlsx")

        If saveFileDialogHelperOutPut.IsSuccess = False Then
            Return
        End If

        Using excel = New ExcelPackage(newFile:=saveFileDialogHelperOutPut.FileInfo)
            Dim defaultWorksheet = excel.Workbook.
                Worksheets.
                OfType(Of ExcelWorksheet).
                FirstOrDefault()
            If defaultWorksheet Is Nothing Then defaultWorksheet = excel.Workbook.Worksheets.Add(Name:="Sheet1")

            Dim initialRowIndex = 1
            defaultWorksheet.Cells(initialRowIndex, 1).Value = "Company Code"
            defaultWorksheet.Cells(initialRowIndex, 2).Value = numCompanyCode.Value.ToString("00000")
            defaultWorksheet.Cells(initialRowIndex, 4).Value = "Payroll Date"
            defaultWorksheet.Cells(initialRowIndex, 5).Value = dtpPayrollDate.Value.ToShortDateString()
            initialRowIndex += 1

            defaultWorksheet.Cells(initialRowIndex, 1).Value = "Funding Account No."
            defaultWorksheet.Cells(initialRowIndex, 2).Value = numFundingAccountNo.Value.ToString("0000000000")
            defaultWorksheet.Cells(initialRowIndex, 4).Value = "Presenting Office"
            defaultWorksheet.Cells(initialRowIndex, 5).Value = numPresentingOffice.Value.ToString("000")
            initialRowIndex += 1

            defaultWorksheet.Cells(initialRowIndex, 1).Value = "Ceiling Amount"
            defaultWorksheet.Cells(initialRowIndex, 2).Value = numCeilingAmount.Value.ToString("N")
            defaultWorksheet.Cells(initialRowIndex, 4).Value = "Batch No."
            defaultWorksheet.Cells(initialRowIndex, 5).Value = numBatchNo.Value.ToString("00")
            initialRowIndex += 2

            defaultWorksheet.Cells(initialRowIndex, 1).Value = "Company Name"
            defaultWorksheet.Cells(initialRowIndex, 2).Value = "Account No."
            defaultWorksheet.Cells(initialRowIndex, 3).Value = "Last Name"
            defaultWorksheet.Cells(initialRowIndex, 4).Value = "First Name"
            defaultWorksheet.Cells(initialRowIndex, 5).Value = "Amount"
            initialRowIndex += 1

            Dim rowIndex = initialRowIndex
            For Each model In models
                defaultWorksheet.Cells(rowIndex, 1).Value = model.CompanyName
                defaultWorksheet.Cells(rowIndex, 2).Value = model.AccountNumber
                defaultWorksheet.Cells(rowIndex, 3).Value = model.LastName
                defaultWorksheet.Cells(rowIndex, 4).Value = model.FirstName
                defaultWorksheet.Cells(rowIndex, 5).Value = model.Amount.ToString("N")

                rowIndex += 1
            Next

            excel.Save()
        End Using

        Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)
    End Sub

End Class
