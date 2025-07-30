Option Strict On

Imports System.IO
Imports System.Text
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Repositories
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Infrastructure.Data
Imports Castle.Components.DictionaryAdapter.Xml
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Office.Interop.Excel
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class BankFileTextFormatForm
    Private Const THOUSAND_VALUE As Integer = 1000
    Private ReadOnly DATETIME_PICKER_MINDATE As Date = New Date(1753, 1, 1)
    Private ReadOnly DATETIME_PICKER_MAXDATE As Date = New Date(9998, 12, 31)
    Private ReadOnly _paystubRepository As IPaystubRepository
    Private ReadOnly _payPeriodRepository As IPayPeriodRepository
    Private ReadOnly _organizationRepository As IOrganizationRepository
    Private ReadOnly _bankFileHeaderRepository As IBankFileHeaderRepository
    Private ReadOnly _organizationId As Integer
    Private ReadOnly _userId As Integer
    Private _organization As Organization
    Private bankFileHeaderDataManager As BankFileHeaderDataManager

    Public Sub New(organizationId As Integer)

        _organizationId = organizationId
        _userId = z_User
        bankFileHeaderDataManager = New BankFileHeaderDataManager(organizationId)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _paystubRepository = MainServiceProvider.GetRequiredService(Of IPaystubRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)

        _bankFileHeaderRepository = MainServiceProvider.GetRequiredService(Of IBankFileHeaderRepository)

    End Sub

    Private Async Sub BankFileTextFormatForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gridPayroll.AutoGenerateColumns = False
        numCompanyCode.Controls(0).Hide()
        numBatchNo.Controls(0).Hide()
        chkSelectAll.ThreeState = True

        Dim bankFileHeaderModel = Await _bankFileHeaderRepository.GetByOrganizationOrCreateAsync(_organizationId, z_User)
        If bankFileHeaderModel IsNot Nothing Then
            numCompanyCode.Text = bankFileHeaderModel.CompanyCode
            numBatchNo.Text = bankFileHeaderModel.BatchNo
        End If

        _organization = Await _organizationRepository.GetByIdWithAddressAsync(_organizationId)

        Dim payPeriod = Await _payPeriodRepository.GetCurrentOpenAsync(organization:=_organization)

        If payPeriod Is Nothing Then
            Dim payPeriods = Await _payPeriodRepository.GetByYearAndPayPrequencyAsync(
                organizationId:=1,
                year:=1,
                payFrequencyId:=1)
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
            Dim payPeriodId = If(payPeriod.RowID, 0)
            paystubs = Await _paystubRepository.GetByPayPeriodWithEmployeeAsync(payPeriodId)
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
        'If Not selectedCount = 0 AndAlso selectedCount < totalCount Then
        '    chkSelectAll.CheckState = CheckState.Indeterminate
        'ElseIf Not selectedCount = 0 AndAlso selectedCount = totalCount Then
        '    chkSelectAll.CheckState = CheckState.Checked
        'ElseIf selectedCount = 0 Then
        '    chkSelectAll.CheckState = CheckState.Unchecked
        'End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs)
        If Not numCompanyCode.Value > 0 OrElse Not numCompanyCode.Value.ToString("00000").Length = 5 Then
            ValidationMessagePrompt($"Invalid Company Code.{Environment.NewLine}It is the 5 digit code.")
            numCompanyCode.Focus()
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

        Dim bankFileExtension = If(Not numBatchNo.Value.ToString("00") = "00" And Not String.IsNullOrEmpty(numBatchNo.Value.ToString("00")),
            numBatchNo.Value.ToString("00"),
            "01")
        Dim defaultFileName = $"{companyCode}-{payrollDate}-{batchNo}.{bankFileExtension}"
        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName:=defaultFileName,
                                defaultExtension:=bankFileExtension,
                                filter:=$"Bank File|*.{bankFileExtension};")

        If saveFileDialogHelperOutPut.IsSuccess = False Then Return

        Dim pathAndFileName = saveFileDialogHelperOutPut.FileInfo.FullName 'Path.Combine(Path.GetTempPath, )
        Using sw As New StreamWriter(pathAndFileName)
            sw.WriteLine(header)
            sw.WriteLine(details)
            sw.WriteLine(trailer)
        End Using

        bankFileHeaderDataManager.Save(companyCode:=numCompanyCode.Value.ToString("00000"), batchNo:=numBatchNo.Value.ToString("00"))

        Process.Start("explorer.exe", $"/select,""{pathAndFileName}""")
    End Sub

    Private Function GetHeader(models As List(Of BankFileModel)) As String
        Dim totalSummary = Math.Round(models.Sum(Function(p) p.Amount), 2).ToString().Replace(".", String.Empty)
        Return String.Concat("H",
            numCompanyCode.Value.ToString("00000"),
            dtpPayrollDate.Value.ToString("MMddyy"),
            numBatchNo.Value.ToString("00"),
            "1",
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

    Private Sub btnExportTxtFile_Click(sender As Object, e As EventArgs) Handles btnExportTxtFile.Click

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

        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName:=$"BankFile_{payrollDate}~{now}", ".txt")

        If saveFileDialogHelperOutPut.IsSuccess = False Then
            Return
        End If
        Dim output As New StringBuilder()

        For Each model In models
            output.AppendLine($"{model.AccountNumber}" & vbTab & $"{model.Amount}")
        Next

        IO.File.WriteAllText(saveFileDialogHelperOutPut.FileInfo.FullName, output.ToString())

        SaveBankFileHeader()

        Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)
    End Sub

    Private Sub gridPayroll_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles gridPayroll.CellFormatting
        If e.RowIndex >= 0 Then gridPayroll.Rows(e.RowIndex).HeaderCell.Value = $"{e.RowIndex + 1}"

    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles btnExportBDO.Click

        Dim models = gridPayroll.Rows.OfType(Of DataGridViewRow).
            Select(Function(r) DirectCast(r.DataBoundItem, BankFileModel)).
            Where(Function(p) p.IsSelected).
            ToList()

        Dim now = Date.Now.ToString("HHmm")
        Dim payrollDate = dtpPayrollDate.Value.ToString("MMddyy")

        Dim defaultExtension = "xlsm"

        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(
            defaultFileName:=$"BDO_{payrollDate}~{now}.{defaultExtension}",
            defaultExtension:=$".{defaultExtension}")

        If saveFileDialogHelperOutPut.IsSuccess = False Then
            Return
        End If


        IO.File.Copy(sourceFileName:=$"BankTemplates/BDO-XPRESS-LINK.{defaultExtension}",
            destFileName:=saveFileDialogHelperOutPut.FileInfo.FullName,
            overwrite:=True)

        Using excel = New ExcelPackage(newFile:=saveFileDialogHelperOutPut.FileInfo)

            Dim defaultWorksheet = excel.Workbook.
                Worksheets.
                OfType(Of ExcelWorksheet).
                FirstOrDefault()
            If defaultWorksheet Is Nothing Then defaultWorksheet = excel.Workbook.Worksheets.FirstOrDefault()

            'Company Code
            defaultWorksheet.Cells("B3").Value = numCompanyCode.Value.ToString("000")

            'Payroll Date
            'defaultWorksheet.Cells("D6").Value = $"{dtpPayrollDate.Value:MM/dd/yyyy}"

            'Funding Account No.
            'defaultWorksheet.Cells("B7").Value = numFundingAccountNo.Value.ToString("0000000000")

            'Batch No.
            defaultWorksheet.Cells("B5").Value = numBatchNo.Value.ToString("00")

            Dim rowIndex = 7
            For Each model In models
                defaultWorksheet.Cells(rowIndex, 1).Value = model.AccountNumber
                defaultWorksheet.Cells(rowIndex, 2).Value = model.Amount.ToString("N")
                defaultWorksheet.Cells(rowIndex, 3).Value = $"{model.LastName}, {model.FirstName} {model.MiddleInitial}."
                defaultWorksheet.Cells(rowIndex, 4).Value = ""

                defaultWorksheet.Cells(rowIndex, 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                defaultWorksheet.Cells(rowIndex, 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                defaultWorksheet.Cells(rowIndex, 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left

                defaultWorksheet.Cells(rowIndex, 3).Style.Font.Color.SetColor(Color.Black)
                defaultWorksheet.Cells(rowIndex, 2).Style.Font.Color.SetColor(Color.Black)
                defaultWorksheet.Cells(rowIndex, 1).Style.Font.Color.SetColor(Color.Black)
                rowIndex += 1
            Next
            rowIndex += 10
            defaultWorksheet.Cells(rowIndex, 2).Value = "Approved By:"
            defaultWorksheet.Cells(rowIndex, 3).Value = "Ken Chua / Rod Chua"
            SaveBankFileHeader()
            excel.Save()
        End Using

        Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)
    End Sub
    Private Async Function SaveBankFileHeader() As Task
        Dim bankFileHeaderRepository = MainServiceProvider.GetRequiredService(Of IBankFileHeaderRepository)
        Dim bankFileHeader = Await bankFileHeaderRepository.GetByOrganizationOrCreateAsync(_organizationId, userId:=_userId)

        bankFileHeader.CompanyCode = numCompanyCode.Value.ToString("000")
        bankFileHeader.BatchNo = numBatchNo.Value.ToString("00")
        bankFileHeader.LastUpdBy = _userId

        Await bankFileHeaderRepository.SaveAsync(bankFileHeader)
    End Function

    Private Function GetModel(row As DataGridViewRow) As BankFileModel
        Return CType(row.DataBoundItem, BankFileModel)
    End Function

    Private Function GetModels() As List(Of BankFileModel)
        Return gridPayroll.Rows.OfType(Of DataGridViewRow).
            Select(Function(t) GetModel(t)).
            ToList()
    End Function

End Class
