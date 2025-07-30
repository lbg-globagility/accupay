Option Strict On

Imports System.IO
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class BankFileTextFormatSecurityBankForm
    Private Const THOUSAND_VALUE As Integer = 1000
    Public Const POLICY_TYPE_NAME As String = "BankFileSecurityBankPolicy"
    Private ReadOnly DATETIME_PICKER_MINDATE As Date = New Date(1753, 1, 1)
    Private ReadOnly DATETIME_PICKER_MAXDATE As Date = New Date(9998, 12, 31)
    Private ReadOnly _paystubRepository As IPaystubRepository
    Private ReadOnly _payPeriodRepository As IPayPeriodRepository
    Private ReadOnly _organizationRepository As IOrganizationRepository
    Private ReadOnly _listOfValueRepository As IListOfValueRepository
    Private ReadOnly _organizationId As Integer
    Private ReadOnly _userId As Integer
    Private _organization As Organization

    Public Sub New(organizationId As Integer, userId As Integer)

        _organizationId = organizationId
        _userId = userId

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _paystubRepository = MainServiceProvider.GetRequiredService(Of IPaystubRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)

        _listOfValueRepository = MainServiceProvider.GetRequiredService(Of IListOfValueRepository)

    End Sub

    Private Async Sub BankFileTextFormatForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gridPayroll.AutoGenerateColumns = False
        numFundingAccountNo.Controls(0).Hide()
        chkSelectAll.ThreeState = True

        _organization = Await _organizationRepository.GetByIdWithAddressAsync(_organizationId)

        Dim payPeriod = Await _payPeriodRepository.GetCurrentOpenAsync(organization:=_organization)

        If payPeriod Is Nothing Then
            Dim payPeriods = Await _payPeriodRepository.GetByYearAndPayPrequencyAsync(
                organizationId:=_organizationId,
                year:=Now.Year,
                payFrequencyId:=PayFrequency.SemiMonthlyTypeId)
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

        Dim bankFileHeaderRepository = MainServiceProvider.GetRequiredService(Of IBankFileHeaderRepository)
        Dim fundingAccountNo = (Await bankFileHeaderRepository.GetByOrganizationOrCreateAsync(_organizationId, userId:=_userId))?.FundingAccountNo

        Decimal.TryParse(fundingAccountNo, numFundingAccountNo.Value)
    End Sub

    Private Async Function LoadPaystubs(payPeriod As PayPeriod) As Task
        Dim paystubs = Enumerable.Empty(Of Paystub)
        If payPeriod.RowID IsNot Nothing Then
            Dim payPeriodId = If(payPeriod.RowID, 0)
            paystubs = Await _paystubRepository.GetByPayPeriodWithEmployeeAsync(payPeriodId)
        End If

        Dim models = paystubs.Select(Function(p) New BankFileModel(p)).
            OrderBy(Function(t) t.CompanyName).
            ThenBy(Function(t) t.FullNameBeginningWithLastName).
            ToList()

        Dim summaryModel As BankFileModel = BankFileModel.BankFileModelSummary(models:=models)

        Dim append = New List(Of BankFileModel) From {summaryModel}
        models = models.Concat(append).ToList()

        gridPayroll.DataSource = models
    End Function

    Private Sub chkSelectAll_CheckedChanged_1(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged

    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub chkSelectAll_CheckStateChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckStateChanged
        RemoveHandler gridPayroll.CellValueChanged, AddressOf gridPayroll_CellValueChanged
        Dim rows = gridPayroll.Rows.OfType(Of DataGridViewRow).ToList()

        If Not chkSelectAll.CheckState = CheckState.Indeterminate Then
            For Each item In rows
                item.Cells(Column1.Name).Value = chkSelectAll.Checked
            Next
        ElseIf chkSelectAll.CheckState = CheckState.Indeterminate Then
            For Each item In rows
                item.Cells(Column1.Name).Value = Not GetModel(item).HasError
            Next
        End If

        gridPayroll.EndEdit()
        gridPayroll.Refresh()

        UpdateTriStateCheckBox()
        AddHandler gridPayroll.CellValueChanged, AddressOf gridPayroll_CellValueChanged
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
    End Sub

    Private Async Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim models = gridPayroll.Rows.OfType(Of DataGridViewRow).
            Select(Function(r) DirectCast(r.DataBoundItem, BankFileModel)).
            Where(Function(p) p.IsSelected).
            ToList()

        Dim fileExtension = "txt"

        Dim postDate = dtpPostingDate.Value.ToString("MMddyy")

        Dim companyInitials As String = String.Join(String.Empty, orgNam.Split(" "c).Select(Function(word) word.Substring(0, 1).ToUpper()))
        Dim defaultFileName = $"{companyInitials}-{postDate}.{fileExtension}"

        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName:=defaultFileName,
            defaultExtension:=fileExtension,
            filter:=$"Bank File|*.{fileExtension};")

        If saveFileDialogHelperOutPut.IsSuccess = False Then Return

        Dim pathAndFileName = saveFileDialogHelperOutPut.FileInfo.FullName
        Using sw As New StreamWriter(pathAndFileName)
            Dim header = BankFileModel.GetFormattedSecurityBankHeader(fundingAccountNo:=CInt(numFundingAccountNo.Value),
                postDate:=postDate,
                models:=models)

            sw.WriteLine(header)

            For Each model In models
                sw.WriteLine(model.SecurityBankFormat)
            Next
        End Using

        Await SaveBankFileHeaderChangesAsync()

        Process.Start("explorer.exe", $"/select,""{pathAndFileName}""")
    End Sub

    Private Async Function SaveBankFileHeaderChangesAsync() As Task
        Dim bankFileHeaderRepository = MainServiceProvider.GetRequiredService(Of IBankFileHeaderRepository)
        Dim bankFileHeader = Await bankFileHeaderRepository.GetByOrganizationOrCreateAsync(_organizationId, userId:=_userId)

        bankFileHeader.FundingAccountNo = $"{numFundingAccountNo.Value}"
        bankFileHeader.LastUpdBy = _userId

        Await bankFileHeaderRepository.SaveAsync(bankFileHeader)
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub gridPayroll_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridPayroll.CellContentClick

    End Sub

    Private Sub gridPayroll_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.ColumnIndex = Column1.Index Then
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

    Private Async Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        Dim bankFilePolicy = (Await _listOfValueRepository.GetAllAsync()).
            Where(Function(t) t.Type = POLICY_TYPE_NAME).
            Where(Function(t) t.DisplayValue.Split({","c}).Contains($"{_organizationId}")).
            FirstOrDefault()

        If bankFilePolicy Is Nothing Then Return

        Dim fileVersionName = bankFilePolicy.LIC

        Dim models = gridPayroll.Rows.OfType(Of DataGridViewRow).
            Select(Function(r) DirectCast(r.DataBoundItem, BankFileModel)).
            Where(Function(p) p.IsSelected).
            ToList()

        Dim companyInitials As String = String.Join(String.Empty, orgNam.Split(" "c).Select(Function(word) word.Substring(0, 1).ToUpper()))
        Dim payrollDate = dtpPostingDate.Value.ToString("MMddyy")

        Dim defaultExtension = fileVersionName.Split({"."c}).LastOrDefault()

        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(
            defaultFileName:=$"SBC_PAYROLL_EPOST {companyInitials} {payrollDate}.{defaultExtension}",
            defaultExtension:=$".{defaultExtension}")

        If saveFileDialogHelperOutPut.IsSuccess = False Then
            Return
        End If

        IO.File.Copy(sourceFileName:=$"BankTemplates/SecurityBank/{fileVersionName}",
            destFileName:=saveFileDialogHelperOutPut.FileInfo.FullName,
            overwrite:=True)

        Using excel = New ExcelPackage(newFile:=saveFileDialogHelperOutPut.FileInfo)
            Dim defaultWorksheet = excel.Workbook.
                Worksheets.
                OfType(Of ExcelWorksheet).
                FirstOrDefault()

            If defaultWorksheet Is Nothing Then defaultWorksheet = excel.Workbook.Worksheets.Add("DATA")

            'Posting Date
            defaultWorksheet.Cells("B3").Value = $"{dtpPostingDate.Value:MM/dd/yyyy}"

            'Funding Account No.
            defaultWorksheet.Cells("B2").Value = numFundingAccountNo.Value.ToString("0000000000000")

            'Total Amount
            defaultWorksheet.Cells("B5").Value = If(models?.Sum(Function(t) t.Amount), 0)

            'Total Record
            defaultWorksheet.Cells("B6").Value = If(models?.Count(), 0)

            Dim rowIndex = 9
            For Each model In models
                defaultWorksheet.Cells(rowIndex, 1).Value = model.FullNameBeginningWithLastName
                defaultWorksheet.Cells(rowIndex, 2).Value = model.AccountNumberSecurityBankCompliant
                defaultWorksheet.Cells(rowIndex, 3).Value = model.Amount.ToString("N")

                rowIndex += 1
            Next

            excel.Save()
        End Using

        Await SaveBankFileHeaderChangesAsync()

        Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)
    End Sub

    Private Sub gridPayroll_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles gridPayroll.CellFormatting
        If e.RowIndex >= 0 Then gridPayroll.Rows(e.RowIndex).HeaderCell.Value = $"{e.RowIndex + 1}"

    End Sub

    Private Function GetModel(row As DataGridViewRow) As BankFileModel
        Return CType(row.DataBoundItem, BankFileModel)
    End Function

    Private Function GetModels() As List(Of BankFileModel)
        Return gridPayroll.Rows.OfType(Of DataGridViewRow).
            Select(Function(t) GetModel(t)).
            ToList()
    End Function

End Class
