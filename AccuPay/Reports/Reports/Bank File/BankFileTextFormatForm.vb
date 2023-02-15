Option Strict On

Imports System.IO
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

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
            dtpPayrollDate.Value = Date.Now
        Else
            dtpPayrollDate.Value = payPeriod.PayFromDate.Date
            dtpPayrollDate.MinDate = payPeriod.PayFromDate.Date
            dtpPayrollDate.MaxDate = payPeriod.PayToDate.Date
        End If

        Await LoadPaystubs(payPeriod)

        AddHandler chkSelectAll.CheckedChanged, AddressOf chkSelectAll_CheckedChanged

        chkSelectAll.Checked = True

        AddHandler gridPayroll.CellValueChanged, AddressOf gridPayroll_CellValueChanged
    End Sub

    Private Async Function LoadPaystubs(payPeriod As PayPeriod) As Task
        Dim paystubs = Await _paystubRepository.GetByPayPeriodWithEmployeeAsync(payPeriodId:=payPeriod.RowID.Value)
        Dim models = paystubs.Select(Function(p) New BankFileModel(p)).ToList()

        Dim summaryModel As BankFileModel = BankFileModel.BankFileModelSummary(models:=models)

        Dim append = New List(Of BankFileModel) From {summaryModel}
        models = models.Concat(append).ToList()

        gridPayroll.DataSource = models
    End Function

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs)
        Dim rows = gridPayroll.Rows.OfType(Of DataGridViewRow).ToList()
        For Each item In rows
            item.Cells(Column1.Name).Value = chkSelectAll.Checked
        Next

        UpdateCeiling()
    End Sub

    Private Sub UpdateCeiling()
        Dim models = gridPayroll.Rows.OfType(Of DataGridViewRow).
            Select(Function(r) DirectCast(r.DataBoundItem, BankFileModel)).
            Where(Function(p) p.IsSelected).
            ToList()

        Dim given = Math.Ceiling(models.Sum(Function(p) p.Amount))
        Dim caught = THOUSAND_VALUE - (given Mod THOUSAND_VALUE)
        Dim result = given + caught
        numCeilingAmount.Value = result
        If result <= THOUSAND_VALUE Then numCeilingAmount.Value = 0
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not numCompanyCode.Value > 0 OrElse Not numCompanyCode.Value.ToString("00000").Length = 5 Then
            ValidationMessagePrompt($"Invalid Company Code.{Environment.NewLine}It is the 5 digit code.")
            Return
        End If

        If Not numFundingAccountNo.Value > 0 OrElse Not numFundingAccountNo.Value.ToString("0000000000").Length = 10 Then
            ValidationMessagePrompt($"Invalid Funding Account No.{Environment.NewLine}It is the 10 digit code.")
            Return
        End If

        If Not numPresentingOffice.Value > 0 OrElse Not numPresentingOffice.Value.ToString("000").Length = 3 Then
            ValidationMessagePrompt($"Invalid Presenting Office.{Environment.NewLine}It is the 3 digit code.")
            Return
        End If

        If Not numBatchNo.Value > 0 OrElse Not numBatchNo.Value.ToString("00").Length = 2 Then
            ValidationMessagePrompt($"Invalid Batch No.{Environment.NewLine}It is the 2 digit code.")
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

        Dim pathAndFileName = Path.Combine(Path.GetTempPath, $"{companyCode}-{payrollDate}-{batchNo}.01")
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
        Return String.Concat("H",
            numCompanyCode.Value.ToString("00000"),
            dtpPayrollDate.Value.ToString("MMddyy"),
            numBatchNo.Value.ToString("00"),
            "1",
            numFundingAccountNo.Value.ToString("0000000000"),
            numPresentingOffice.Value.ToString("000"),
            numCeilingAmount.Value.ToString("0000000000"),
            models.Sum(Function(p) p.Amount).ToString("00000000000000"),
            "1",
            Space(75))
    End Function

    Private Shared Sub ValidationMessagePrompt(warningMessage As String)
        MessageBox.Show(warningMessage, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    End Sub

    Private Function GetTrailer(models As List(Of BankFileModel)) As String
        Return String.Concat("T",
            numCompanyCode.Value.ToString("00000"),
            dtpPayrollDate.Value.ToString("MMddyy"),
            numBatchNo.Value.ToString("00"),
            "2",
            numFundingAccountNo.Value.ToString("0000000000"),
            models.Sum(Function(p) p.AccountNumberDecimal).ToString("000000000000000"),
            models.Sum(Function(p) p.Amount).ToString("000000000000000"),
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
        If e.ColumnIndex = Column1.Index Then UpdateCeiling()
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

End Class
