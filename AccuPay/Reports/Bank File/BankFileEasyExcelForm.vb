Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class BankFileEasyExcelForm
    Public Const POLICY_TYPE_NAME As String = "BankFileEasyExcelPolicy"
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
        Dim bankFilePolicy = (Await _listOfValueRepository.GetAllAsync()).
            Where(Function(t) t.Type = POLICY_TYPE_NAME).
            Where(Function(t) t.DisplayValue.Split({","c}).Contains($"{_organizationId}")).
            FirstOrDefault()

        If bankFilePolicy Is Nothing Then Return

        Dim fileName = bankFilePolicy.LIC

        Dim models = gridPayroll.Rows.OfType(Of DataGridViewRow).
            Select(Function(r) DirectCast(r.DataBoundItem, BankFileModel)).
            Where(Function(p) p.IsSelected).
            ToList()

        Dim postDate = dtpPostingDate.Value.ToString("MMddyy")

        Dim defaultExtension = fileName.Split({"."c}).LastOrDefault()

        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(
            defaultFileName:=$"{orgNam} {postDate}.{defaultExtension}",
            defaultExtension:=$".{defaultExtension}")

        If saveFileDialogHelperOutPut.IsSuccess = False Then
            Return
        End If

        IO.File.Copy(sourceFileName:=$"BankTemplates/{fileName}",
            destFileName:=saveFileDialogHelperOutPut.FileInfo.FullName,
            overwrite:=True)

        Using excel = New ExcelPackage(newFile:=saveFileDialogHelperOutPut.FileInfo)
            Dim defaultWorksheet = excel.Workbook.
                Worksheets.
                OfType(Of ExcelWorksheet).
                FirstOrDefault()

            If defaultWorksheet Is Nothing Then defaultWorksheet = excel.Workbook.Worksheets.Add("Sheet1")

            Dim rowIndex = 1
            For Each model In models
                defaultWorksheet.Cells(rowIndex, 1).Value = model.FullNameBeginningWithLastName
                defaultWorksheet.Cells(rowIndex, 2).Value = model.AccountNumber
                defaultWorksheet.Cells(rowIndex, 3).Value = model.Amount.ToString("N")

                rowIndex += 1
            Next

            excel.Save()
        End Using

        Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)
    End Sub

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
