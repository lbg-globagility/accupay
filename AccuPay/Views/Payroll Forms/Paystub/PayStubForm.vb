Option Strict On

Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.CrystalReports
Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Exceptions
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Repositories.PaystubRepository
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports Castle.Components.DictionaryAdapter
Imports log4net
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class PayStubForm

    Private Const FormEntityName As String = "Payroll"

    Private _logger As ILog = LogManager.GetLogger("PayrollLogger")

    Public Property CurrentYear As Integer = CDate(dbnow).Year

    Private Const ItemsPerPage As Integer = 20

    Private _pageNo As Integer = 0

    Private _lastPaystubPageNo As Integer = 0

    Private _totalPaystubs As Integer = 0

    Dim employeepicture As New DataTable

    Private _currentPayFromDate As Date? = Nothing
    Private _currentPayToDate As Date? = Nothing
    Private _currentPayperiodId As Integer? = Nothing

    Dim currentEmployeeNumber As String = Nothing

    Dim selectedButtonFont As New Font("Trebuchet MS", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont As New Font("Trebuchet MS", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))

    Private _originalActualAdjustments As List(Of ActualAdjustment)

    Private _originalAdjustments As List(Of Adjustment)

    Private _currentSystemOwner As String

    Private ReadOnly _policy As PolicyHelper

    Private ReadOnly _systemOwnerService As SystemOwnerService

    Private ReadOnly _agencyFeeRepository As AgencyFeeRepository

    Private ReadOnly _employeeRepository As EmployeeRepository

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private ReadOnly _paystubRepository As PaystubRepository

    Private ReadOnly _productRepository As ProductRepository

    Sub New()

        InitializeComponent()

        _policy = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        _agencyFeeRepository = MainServiceProvider.GetRequiredService(Of AgencyFeeRepository)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

        _paystubRepository = MainServiceProvider.GetRequiredService(Of PaystubRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        SplitContainer1.SplitterWidth = 6

        Dim dgvVisibleRows = PayPeriodGridView.Columns.Cast(Of DataGridViewColumn).Where(Function(ii) ii.Visible = True)

        Dim scrollbarwidth = 19

        Dim mincolwidth As Integer = CInt((PayPeriodGridView.Width - (PayPeriodGridView.RowHeadersWidth + scrollbarwidth)) / dgvVisibleRows.Count)

        For Each dgvcol In dgvVisibleRows
            dgvcol.Width = mincolwidth
            dgvcol.SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        _currentSystemOwner = _systemOwnerService.GetCurrentSystemOwner()
        SetProperInterfaceBaseOnCurrentSystemOwner()

        For Each txtbx In Panel6.Controls.OfType(Of TextBox)
            AddHandler txtbx.TextChanged, AddressOf NumberFieldFormatter
        Next

        MyBase.OnLoad(e)

    End Sub

    Private Async Sub PayStub_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        AdjustmentGridView.AutoGenerateColumns = False
        PayPeriodGridView.AutoGenerateColumns = False

        PayPeriodGridView.Focus()

        linkPrev.Text = "← " & (CurrentYear - 1)
        linkNxt.Text = (CurrentYear + 1) & " →"

        If dgvemployees.RowCount <> 0 Then
            dgvemployees.Item("EmployeeID", 0).Selected = True
        End If

        cboProducts.ValueMember = "RowID"
        cboProducts.DisplayMember = "DisplayName"

        Await PopulateAdjustmentTypeComboBox()

        ShowOrHideActual()
        ShowOrHideEmailPayslip()
        ShowOrHideCostCenterReport()

        Await RefreshForm()

        AddHandler DeclaredTabPage.Enter, AddressOf DeclaredTabPage_Enter
        AddHandler ActualTabPage.Enter, AddressOf ActualTabPage_Enter
        AddHandler PayPeriodGridView.SelectionChanged, AddressOf dgvpayper_SelectionChanged
        AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    Private Async Function PopulateAdjustmentTypeComboBox() As Task
        Dim items = (Await _productRepository.GetAdjustmentTypesAsync(z_OrganizationID)).
            OrderBy(Function(a) a.PartNo).
            ToList()

        cboProducts.DataSource = items
    End Function

    Private Sub PayStub_EnabledChanged(sender As Object, e As EventArgs) Handles Me.EnabledChanged

        Dim _bool As Boolean = Me.Enabled

        ''###### CONTROL DISABLER ######

        PayrollForm.MenuStrip1.Enabled = _bool
        MDIPrimaryForm.Showmainbutton.Enabled = _bool
        ToolStrip1.Enabled = _bool

        Panel5.Enabled = _bool

    End Sub

    Private Sub ShowOrHideCostCenterReport()
        CostCenterReportToolStripMenuItem.Visible = _policy.UseCostCenter
    End Sub

    Private Sub ShowOrHideActual()

        Dim showActual = _policy.ShowActual

        PayslipDeclaredToolStripMenuItem.Visible = showActual
        PayslipActualToolStripMenuItem.Visible = showActual

        CostCenterReportAllDeclaredToolStripMenuItem.Visible = showActual
        CostCenterReportByBranchDeclaredToolStripMenuItem.Visible = showActual
        CostCenterReportAllActualToolStripMenuItem.Visible = showActual
        CostCenterReportByBranchActualToolStripMenuItem.Visible = showActual

        ExportNetPayDeclaredAllToolStripMenuItem.Visible = showActual
        ExportNetPayDeclaredCashToolStripMenuItem.Visible = showActual
        ExportNetPayDeclaredDirectDepositToolStripMenuItem.Visible = showActual
        ExportNetPayActualAllToolStripMenuItem.Visible = showActual
        ExportNetPayActualCashToolStripMenuItem.Visible = showActual
        ExportNetPayActualDirectDepositToolStripMenuItem.Visible = showActual

        If showActual = False Then

            Dim str_empty As String = String.Empty

            DeclaredTabPage.Text = str_empty

            ActualTabPage.Text = str_empty

            AddHandler tabEarned.Selecting, AddressOf tabEarned_Selecting
        Else

            RemoveHandler CostCenterReportAllToolStripMenuItem.Click, AddressOf CostCenterReportToolStripMenuItem_Click
            RemoveHandler CostCenterReportByBranchToolStripMenuItem.Click, AddressOf CostCenterReportToolStripMenuItem_Click

            RemoveHandler ExportNetPayAllToolStripMenuItem.Click, AddressOf ExportNetPayDetailsToolStripMenuItem_Click
            RemoveHandler ExportNetPayCashToolStripMenuItem.Click, AddressOf ExportNetPayDetailsToolStripMenuItem_Click
            RemoveHandler ExportNetPayDirectDepositToolStripMenuItem.Click, AddressOf ExportNetPayDetailsToolStripMenuItem_Click

            RemoveHandler PrintPaySlipToolStripMenuItem.Click, AddressOf PrintAllPaySlip_Click

        End If

    End Sub

    Private Sub ShowOrHideEmailPayslip()

        Dim emailPayslip = _policy.UseEmailPayslip

        ManagePayslipsToolStripMenuItem.Visible = emailPayslip
        PrintPaySlipToolStripMenuItem.Visible = Not emailPayslip

    End Sub

    Public Async Function VIEW_payperiodofyear() As Task
        ClosePayrollToolStripMenuItem.Visible = False
        ReopenPayrollToolStripMenuItem.Visible = False
        GeneratePayrollToolStripButton.Visible = False

        RemoveHandler PayPeriodGridView.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        Dim payPeriodList = Await _payPeriodRepository.GetPaginatedListAsync(
            PageOptions.AllData,
            organizationId:=z_OrganizationID,
            year:=CurrentYear)

        Dim payPeriods = payPeriodList.Items

        PayPeriodGridView.DataSource = payPeriods
        Dim index As Integer = 0
        For Each payPeriod In payPeriods
            HighlightOpenPayPeriod(index, payPeriod)

            index += 1
        Next

        Await UpdatePayPeriodDetails()

        AddHandler PayPeriodGridView.SelectionChanged, AddressOf dgvpayper_SelectionChanged
    End Function

    Private Sub HighlightOpenPayPeriod(index As Integer, payPeriod As PayPeriod)

        Dim currentRow = PayPeriodGridView.Rows(index)

        If payPeriod.Status = PayPeriodStatus.Open Then

            currentRow.DefaultCellStyle.SelectionForeColor = Color.Green

            Dim defaultFont = currentRow.DefaultCellStyle.Font
            If defaultFont Is Nothing Then

                If PayPeriodGridView.DefaultCellStyle.Font IsNot Nothing Then

                    defaultFont = PayPeriodGridView.DefaultCellStyle.Font

                ElseIf defaultFont IsNot Nothing Then

                    defaultFont = defaultFont
                Else

                    defaultFont = New Font("Microsoft Sans Serif", 8.25)
                End If

                currentRow.DefaultCellStyle.Font = New Font(defaultFont, FontStyle.Bold)

            End If

        End If

    End Sub

    Private Async Function LoadEmployeesAsync() As Task

        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        dgvemployees.Rows.Clear()

        If _currentPayperiodId IsNot Nothing Then

            Dim offset = _pageNo * ItemsPerPage
            Dim limit = ItemsPerPage

            Dim parameters = New Object() {
                orgztnID,
                tsSearch.Text,
                _currentPayperiodId,
                offset,
                limit
            }

            Dim n_ReadSQLProcedureToDatatable =
            New SQL("CALL SEARCH_employee_paystub(?1, ?2, ?3, ?4, ?5);",
                    parameters)

            Dim catchdt = n_ReadSQLProcedureToDatatable.GetFoundRows.Tables(0)

            For Each drow As DataRow In catchdt.Rows
                Dim row_array = drow.ItemArray
                dgvemployees.Rows.Add(row_array)
            Next

            _totalPaystubs = CInt(EXECQUER($"SELECT COUNT(RowID) FROM paystub WHERE OrganizationID={orgztnID} AND PayPeriodID = {_currentPayperiodId};"))
            _lastPaystubPageNo = CInt({Math.Ceiling(_totalPaystubs / ItemsPerPage) - 1, 0}.Max())

            With dgvemployees
                .Columns("RowID").Visible = False

            End With
            If dgvemployees.RowCount > 0 Then
                dgvemployees.Item("EmployeeID", 0).Selected = True
            End If

            employeepicture = New SQLQueryToDatatable("SELECT RowID,Image FROM employee WHERE Image IS NOT NULL AND OrganizationID=" & orgztnID & ";").ResultTable 'retAsDatTbl("SELECT RowID,Image FROM employee WHERE OrganizationID=" & orgztnID & ";")

        End If

        Await UpdateEmployeeDetails()

        AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

    End Function

    Private Async Sub dgvpayper_SelectionChanged(sender As Object, e As EventArgs)
        Await UpdatePayPeriodDetails()
    End Sub

    Private Async Function UpdatePayPeriodDetails() As Task

        If PayPeriodGridView.CurrentRow Is Nothing OrElse
            CType(PayPeriodGridView.CurrentRow.DataBoundItem, PayPeriod) Is Nothing Then

            _currentPayperiodId = Nothing
            _currentPayFromDate = Nothing
            _currentPayToDate = Nothing

            ShowPayrollActions(False)
        Else

            Dim currentPayPeriod = CType(PayPeriodGridView.CurrentRow.DataBoundItem, PayPeriod)

            _currentPayperiodId = currentPayPeriod.RowID
            _currentPayFromDate = currentPayPeriod.PayFromDate
            _currentPayToDate = currentPayPeriod.PayToDate

            Dim isOpen = currentPayPeriod.Status = PayPeriodStatus.Open
            ShowPayrollActions(isOpen, currentPayPeriod.Status)
        End If

        Await LoadEmployeesAsync()
    End Function

    Private Sub ShowPayrollActions(showPayrollActions As Boolean, Optional payrollStatus As PayPeriodStatus? = Nothing)

        DeletePaystubColumn.Visible = showPayrollActions

        If payrollStatus Is Nothing Then

            DisableAllPayrollActions()
        Else

            Select Case payrollStatus
                Case PayPeriodStatus.Pending

                    DisableAllPayrollActions()
                    ClosePayrollToolStripMenuItem.Visible = True

                Case PayPeriodStatus.Open

                    EnablePayrollToolStripItem()
                    ReopenPayrollToolStripMenuItem.Visible = False

                    EnableAdjustmentsInput()

                Case PayPeriodStatus.Closed

                    DisableAllPayrollActions()
                    ReopenPayrollToolStripMenuItem.Visible = True

                Case Else
                    DisableAllPayrollActions()

            End Select

        End If
    End Sub

    Private Sub DisableAllPayrollActions()
        EnablePayrollToolStripItem(enable:=False)
        EnableAdjustmentsInput(enable:=False)
    End Sub

    Private Sub EnablePayrollToolStripItem(Optional enable As Boolean = True)
        GeneratePayrollToolStripButton.Visible = enable
        RecalculateThirteenthMonthPayToolStripMenuItem.Visible = enable
        CancelPayrollToolStripMenuItem.Visible = enable
        DeletePaystubsToolStripMenuItem.Visible = enable
        ReopenPayrollToolStripMenuItem.Visible = enable
        ClosePayrollToolStripMenuItem.Visible = enable
        'OthersToolStripMenuItem.Visible = enable
    End Sub

    Private Sub EnableAdjustmentsInput(Optional enable As Boolean = True)
        AdjustmentGridView.Enabled = enable
        btnSaveAdjustments.Visible = enable
        btnDiscardChanges.Visible = enable
    End Sub

    Private Async Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        CurrentYear = CurrentYear - 1

        linkPrev.Text = "← " & (CurrentYear - 1)
        linkNxt.Text = (CurrentYear + 1) & " →"

        Await VIEW_payperiodofyear()
    End Sub

    Private Async Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        CurrentYear = CurrentYear + 1

        linkNxt.Text = (CurrentYear + 1) & " →"
        linkPrev.Text = "← " & (CurrentYear - 1)

        Await VIEW_payperiodofyear()
    End Sub

    Private Async Sub EmployeeGridViewPaginationChanged(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles _
        First.LinkClicked, Prev.LinkClicked,
        Nxt.LinkClicked, Last.LinkClicked,
        LinkLabel2.LinkClicked, LinkLabel1.LinkClicked

        Await UpdateEmployeeListByPagination(sender)
    End Sub

    Private Async Function UpdateEmployeeListByPagination(sender As Object) As Task
        Dim action As PaginationAction

        If sender Is First OrElse sender Is LinkLabel1 Then
            action = PaginationAction.First
        ElseIf sender Is Prev OrElse sender Is LinkLabel2 Then
            action = PaginationAction.Previous
        ElseIf sender Is Nxt OrElse sender Is LinkLabel4 Then
            action = PaginationAction.Next
        ElseIf sender Is Last OrElse sender Is LinkLabel3 Then
            action = PaginationAction.Last
        End If

        Select Case action
            Case PaginationAction.First
                _pageNo = 0
            Case PaginationAction.Previous
                _pageNo = If(1 < _pageNo, _pageNo - 1, 0)
            Case PaginationAction.Next
                _pageNo = If(_pageNo < _lastPaystubPageNo, _pageNo + 1, _lastPaystubPageNo)
            Case PaginationAction.Last
                _pageNo = _lastPaystubPageNo
        End Select

        Await LoadEmployeesAsync()
    End Function

    Private Async Sub dgvemployees_SelectionChanged(sender As Object, e As EventArgs)

        Await UpdateEmployeeDetails()

    End Sub

    Private Async Function UpdateEmployeeDetails() As Task

        EnableAdjustmentButtons(False)
        ResetNumberTextBox()
        AdjustmentGridView.DataSource = Nothing

        dgvemployees.Tag = Nothing

        If dgvemployees.RowCount > 0 Then 'And dgvemployees.CurrentRow IsNot Nothing
            With dgvemployees.CurrentRow

                dgvemployees.Tag = .Cells("RowID").Value

                txtFName.Text = .Cells("FirstName").Value.ToString()

                Dim addtlWord As String = Nothing

                If Not IsDBNull(.Cells("MiddleName").Value) AndAlso Not String.IsNullOrWhiteSpace(.Cells("MiddleName").Value.ToString()) Then

                    Dim midNameTwoWords = Split(.Cells("MiddleName").Value.ToString, " ")

                    addtlWord = " "

                    For Each s In midNameTwoWords

                        addtlWord &= (StrConv(Microsoft.VisualBasic.Left(s, 1), VbStrConv.ProperCase) & ".")

                    Next

                    txtFName.Text &= addtlWord

                End If

                txtFName.Text = txtFName.Text & " " & .Cells("LastName").Value.ToString()

                currentEmployeeNumber = .Cells("EmployeeID").Value.ToString()

                txtEmpID.Text = "ID# " & .Cells("EmployeeID").Value.ToString() &
                    If(IsDBNull(.Cells("Position")),
                        "",
                        ", " & .Cells("Position").Value.ToString()) &
                    If(.Cells("EmployeeType").Value.ToString() = Nothing,
                        "",
                        ", " & .Cells("EmployeeType").Value.ToString() & " salary")

                pbEmpPicChk.Image = Nothing
                Try
                    Dim selemppic = employeepicture.Select("RowID=" & ValNoComma(.Cells("RowID").Value))

                    For Each drow In selemppic
                        If IsDBNull(drow("Image")) Then
                            pbEmpPicChk.Image = Nothing
                        Else
                            pbEmpPicChk.Image = ConvByteToImage(DirectCast(drow("Image"), Byte()))
                        End If
                        Exit For
                    Next
                Catch ex As Exception

                End Try

                'End If

                txtAbsenceDeduction.Text = "0.00"
                txtLateDeduction.Text = "0.00"
                txtUndertimeDeduction.Text = "0.00"

                If _currentPayperiodId IsNot Nothing Then
                    '# ####################################################### #
                    Try
                        For Each txtbxctrl In SplitContainer1.Panel2.Controls.OfType(Of TextBox).ToList()
                            txtbxctrl.Text = "0.00"
                        Next
                    Catch ex As Exception
                        MsgBox(getErrExcptn(ex, Me.Name))
                    End Try
                    If tabEarned.SelectedIndex = 0 Then
                        Await ShowDeclaredData()
                    ElseIf tabEarned.SelectedIndex = 1 Then
                        Await ShowActualData()
                    End If
                Else

                    txtGrossPay.Text = ""
                    txtNetPay.Text = ""
                    txtTotalTaxableSalary.Text = ""

                    Try
                        For Each txtbxctrl In SplitContainer1.Panel2.Controls.OfType(Of TextBox).ToList()
                            txtbxctrl.Text = "0.00"
                        Next
                    Catch ex As Exception
                        MsgBox(getErrExcptn(ex, Me.Name))
                    End Try
                    If tabEarned.SelectedIndex = 0 Then
                        Await ShowDeclaredData()
                    ElseIf tabEarned.SelectedIndex = 1 Then
                        Await ShowActualData()
                    End If
                End If

            End With
        Else
            currentEmployeeNumber = Nothing

            pbEmpPicChk.Image = Nothing
            txtFName.Text = ""
            txtEmpID.Text = ""

            txtBasicRate.Text = ""

            txttotreghrs.Text = ""
            txttotregamt.Text = ""

            txtOvertimeHours.Text = ""
            txtOvertimePay.Text = ""

            txtNightDiffHours.Text = ""
            txtNightDiffPay.Text = ""

            txtNightDiffOvertimeHours.Text = ""
            txtNightDiffOvertimePay.Text = ""

            txtHolidayHours.Text = ""
            txtHolidayPay.Text = ""

            txtRegularHours.Text = ""
            txtRegularPay.Text = ""

            lblSubtotal.Text = ""

            txtTotalNonTaxableAllowance.Text = ""
            txtTotalTaxableAllowance.Text = ""

            txtGrossPay.Text = ""

            txtAbsentHours.Text = ""
            txtAbsenceDeduction.Text = ""

            txtLateHours.Text = ""
            txtLateDeduction.Text = ""

            txtUndertimeHours.Text = ""
            txtUndertimeDeduction.Text = ""

            lblsubtotmisc.Text = ""

            txtSssEmployeeShare.Text = ""
            txtPhilHealthEmployeeShare.Text = ""
            txtHdmfEmployeeShare.Text = ""

            txtTotalLoans.Text = ""
            txtTotalBonus.Text = ""

            txtTotalTaxableSalary.Text = ""
            txtWithholdingTax.Text = ""
            txtNetPay.Text = ""

            txtLeavePay.Text = ""
            txtThirteenthMonthPay.Text = ""
            txtTotalNetPay.Text = ""

        End If
    End Function

    Async Function GeneratePayroll() As Task

        If _currentPayperiodId Is Nothing Then
            Return
        End If

        Dim payPeriod = _payPeriodRepository.GetById(_currentPayperiodId.Value)
        If payPeriod Is Nothing OrElse payPeriod?.RowID Is Nothing OrElse payPeriod?.OrganizationID Is Nothing Then
            Return
        End If

        If payPeriod.Status <> PayPeriodStatus.Open Then
            MessageBoxHelper.Warning("Only ""Open"" pay periods can be computed.")
            Return
        End If

        'We are using a fresh instance of EmployeeRepository
        Dim repository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
        'later, we can let the user choose the employees that they want to generate.
        Dim employees = Await repository.GetAllActiveAsync(z_OrganizationID)

        Dim generator As New PayrollGeneration(employees, additionalProgressCount:=1)
        Dim progressDialog = New ProgressDialog(generator, "Generating payroll...")

        MDIPrimaryForm.Enabled = False
        progressDialog.Show()

        generator.SetCurrentMessage("Loading resources...")
        GetResources(
            progressDialog,
            Sub(resourcesTask)

                If resourcesTask Is Nothing Then

                    HandleErrorLoadingResources(progressDialog)
                    Return
                End If

                generator.IncreaseProgress("Finished loading resources.")

                Dim generationTask = Task.Run(
                    Async Function()
                        Await generator.Start(resourcesTask.Result, New TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate))
                    End Function
                )

                generationTask.ContinueWith(
                    Sub() GeneratePayrollOnSuccess(generator.Results, progressDialog),
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext
                )

                generationTask.ContinueWith(
                    Sub(t) GeneratePayrollOnError(t, progressDialog),
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnFaulted,
                    TaskScheduler.FromCurrentSynchronizationContext
                )
            End Sub)

    End Function

    Private Async Sub GeneratePayrollOnSuccess(results As IReadOnlyCollection(Of ProgressGenerator.IResult), progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        Dim saveResults = results.Select(Function(r) CType(r, PaystubEmployeeResult)).ToList()

        Dim dialog = New EmployeeResultsDialog(
            saveResults,
            title:="Payroll Results",
            generationDescription:="Payroll generation",
            entityDescription:="paystubs") With {
            .Owner = Me
        }

        dialog.ShowDialog()
        Await RefreshForm()

        Await TimeEntrySummaryForm.LoadPayPeriods()

    End Sub

    Private Sub GeneratePayrollOnError(t As Task, progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        _logger.Error("Error on generating payroll.", t.Exception)
        MsgBox("Something went wrong while generating the payroll . Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Generation")

    End Sub

    Private Sub GetResources(progressDialog As ProgressDialog, callBackAfterLoadResources As Action(Of Task(Of PayrollResources)))
        Dim resources = MainServiceProvider.GetRequiredService(Of PayrollResources)

        Dim loadTask = Task.Run(
            Function()
                Dim resourcesTask = resources.Load(
                    payPeriodId:=_currentPayperiodId.Value,
                    organizationId:=z_OrganizationID,
                    userId:=z_User)

                resourcesTask.Wait()

                Return resources
            End Function)

        loadTask.ContinueWith(
            callBackAfterLoadResources,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        loadTask.ContinueWith(
            Sub(t) LoadingResourcesOnError(t, progressDialog),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Sub LoadingResourcesOnError(t As Task, progressDialog As ProgressDialog)

        _logger.Error("Error loading one of the payroll data.", t.Exception)

        HandleErrorLoadingResources(progressDialog)

    End Sub

    Private Shared Sub HandleErrorLoadingResources(progressDialog As ProgressDialog)
        CloseProgressDialog(progressDialog)

        MsgBox("Something went wrong while loading the payroll data needed for computation. Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Resources")
    End Sub

    Private Shared Sub CloseProgressDialog(progressDialog As ProgressDialog)

        MDIPrimaryForm.Enabled = True

        If progressDialog Is Nothing Then Return

        progressDialog.Close()
        progressDialog.Dispose()
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Me.Close()
    End Sub

    Private Async Sub btnrefresh_Click(sender As Object, e As EventArgs) Handles btnrefresh.Click
        Await VIEW_payperiodofyear()
    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        TabControlColor(TabControl1, e)
    End Sub

    Private Async Sub ShowAllowanceButton_Click(sender As Object, e As EventArgs) Handles ShowNonTaxableAllowanceButton.Click, ShowTaxableAllowanceButton.Click
        Dim isTaxable = sender Is ShowTaxableAllowanceButton

        Dim employeeId = ObjectUtils.ToNullableInteger(dgvemployees.Tag)
        If employeeId Is Nothing OrElse _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected paystub.")
            Return

        End If

        Dim paystub = Await _paystubRepository.GetByCompositeKeyAsync(
            New EmployeeCompositeKey(employeeId:=employeeId.Value, payPeriodId:=_currentPayperiodId.Value))

        If paystub Is Nothing Then

            MessageBoxHelper.Warning("No selected paystub.")
            Return

        End If

        Dim dialog As New AllowanceBreakdownDialog(paystub.RowID.Value, isTaxable)
        dialog.ShowDialog()
    End Sub

    Private Async Sub btntotloan_Click(sender As Object, e As EventArgs) Handles btntotloan.Click
        Dim employeeId = ObjectUtils.ToNullableInteger(dgvemployees.Tag)
        If employeeId Is Nothing OrElse _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected paystub.")
            Return

        End If

        Dim paystub = Await _paystubRepository.GetByCompositeKeyAsync(
            New EmployeeCompositeKey(employeeId:=employeeId.Value, payPeriodId:=_currentPayperiodId.Value))

        If paystub Is Nothing Then

            MessageBoxHelper.Warning("No selected paystub.")
            Return

        End If

        Dim form As New LoanBreakdownDialog(paystub.RowID.Value)
        form.ShowDialog()
    End Sub

    Private Async Sub btntotbon_Click(sender As Object, e As EventArgs) Handles btntotbon.Click
        Dim employeeId = ObjectUtils.ToNullableInteger(dgvemployees.Tag)
        If employeeId Is Nothing OrElse _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected paystub.")
            Return

        End If

        Dim paystub = Await _paystubRepository.GetByCompositeKeyAsync(
            New EmployeeCompositeKey(employeeId:=employeeId.Value, payPeriodId:=_currentPayperiodId.Value))

        If paystub Is Nothing Then

            MessageBoxHelper.Warning("No selected paystub.")
            Return

        End If

        Dim datePeriod = New TimePeriod(paystub.PayFromDate, paystub.PayToDate)
        Dim form As New BonusBreakdownDialog(employeeId:=employeeId.Value, datePeriod:=datePeriod)
        form.ShowDialog()
    End Sub

    Private Sub PayStub_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = False

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        Dim open_forms = My.Application.OpenForms.Cast(Of Form).Where(Function(i) i.Name = "CrysVwr")
        If open_forms.Count > 0 Then
            Dim array_open_forms = open_forms.ToArray
            For Each frm In array_open_forms
                frm.Close()
            Next
        End If

        PayrollForm.listPayrollForm.Remove(Me.Name)
    End Sub

    Private Async Sub tsSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tsSearch.KeyPress
        Dim e_asc As Integer = Asc(e.KeyChar)

        Dim userPressedEnterKey = (e_asc = 13)
        If userPressedEnterKey Then
            Await SearchEmployeeAsync()
        End If
    End Sub

    Private Async Sub tsbtnSearch_Click(sender As Object, e As EventArgs) Handles tsbtnSearch.Click
        Await SearchEmployeeAsync()
    End Sub

    Public Async Function SearchEmployeeAsync() As Task
        _pageNo = 0

        If tsSearch.Text.Trim.Length = 0 Then
            Await UpdateEmployeeListByPagination(First)
        Else
            Await LoadEmployeesAsync()
        End If
    End Function

    Private Sub PayrollSummaryDeclaredAndActualToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles _
        PrintPayrollSummaryToolStripButton.Click

        Dim reportProvider As New PayrollSummaryExcelFormatReportProvider()
        reportProvider.Run()

    End Sub

    Private Async Sub dgvemployees_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployees.CellContentClick

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then Return

        If e.ColumnIndex = DeletePaystubColumn.Index Then

            Await DeletePaystub()

        End If

    End Sub

    Private Async Sub dgvAdjustments_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles AdjustmentGridView.CellContentClick

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 OrElse AdjustmentGridView.CurrentRow Is Nothing Then Return

        Dim adjustments = CType(AdjustmentGridView.DataSource, BindingSource)
        Dim currentAdjustment = CType(AdjustmentGridView.CurrentRow.DataBoundItem, IAdjustment)

        If currentAdjustment Is Nothing Then Return

        If currentAdjustment.RowID Is Nothing OrElse currentAdjustment.RowID <= 0 Then

            adjustments.Remove(currentAdjustment)
            Return
        End If

        If e.ColumnIndex = DeleteAdjustmentColumn.Index Then

            Dim adjustmentTypes = CType(cboProducts.DataSource, List(Of Product))
            Dim adjustmentType As Product = If(currentAdjustment.ProductID Is Nothing, Nothing,
                adjustmentTypes.FirstOrDefault(Function(a) a.RowID.Value = currentAdjustment.ProductID.Value))

            Dim adjustmentName = adjustmentType?.Name

            Dim message = "Are you sure you want to delete '" & adjustmentName & "'" & If(adjustmentName.ToLower.Contains("adjustment"), "", " adjustment") & " ?"

            If MessageBoxHelper.Confirm(Of Boolean)(message, "Delete Adjustment") Then

                adjustments.Remove(currentAdjustment)
                Await SaveAdjustmentsAsync()
            End If
        End If
    End Sub

    Private Async Sub btnSaveAdjustments_Click(sender As Object, e As EventArgs) Handles btnSaveAdjustments.Click
        Await SaveAdjustmentsAsync()
    End Sub

    Private Async Function SaveAdjustmentsAsync() As Task
        AdjustmentGridView.EndEdit()
        Const messageTitle As String = "Save Adjustments"

        Dim employeeId = ObjectUtils.ToNullableInteger(dgvemployees.Tag)
        If employeeId Is Nothing OrElse _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected paystub.", messageTitle)
            Return

        End If

        Dim paystub = Await _paystubRepository.GetByCompositeKeyAsync(
                        New EmployeeCompositeKey(
                            employeeId:=employeeId.Value,
                            payPeriodId:=_currentPayperiodId.Value))

        If paystub Is Nothing OrElse paystub.RowID Is Nothing Then

            MessageBoxHelper.Warning("Paystub no longer exists.", messageTitle)
            Return

        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
         Async Function()

             Dim allAjustments = CType(AdjustmentGridView.DataSource, BindingSource)

             Dim isActual = IsActualSelected()
             If isActual Then

                 Await UpdateAdjustments(Of ActualAdjustment)(paystub, allAjustments)
                 Await RecordAdjustmentUserActivity(Of ActualAdjustment)(allAjustments, paystub, isActual)
             Else

                 Await UpdateAdjustments(Of Adjustment)(paystub, allAjustments)
                 Await RecordAdjustmentUserActivity(Of Adjustment)(allAjustments, paystub, isActual)
             End If

             Await UpdateEmployeeDetails()

         End Function)
    End Function

    Private Async Function RecordAdjustmentUserActivity(Of T As IAdjustment)(
        allAjustments As BindingSource,
        paystub As Paystub,
        isActual As Boolean) As Task

        Dim result = CreateAdjustmentUserActivityItems(Of T)(allAjustments, paystub, isActual)

        Await SaveUserAdjustmentUserActivity(result)

    End Function

    Private Shared Async Function SaveUserAdjustmentUserActivity(result As (added As List(Of UserActivityItem), updated As List(Of UserActivityItem), deleted As List(Of UserActivityItem))) As Task
        Dim added As List(Of UserActivityItem) = result.added
        Dim updated As List(Of UserActivityItem) = result.updated
        Dim deleted As List(Of UserActivityItem) = result.deleted

        If added?.Any() Then

            Dim userActivityService = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
            Await userActivityService.CreateRecordAsync(
              z_User,
              FormEntityName,
              z_OrganizationID,
              UserActivityRepository.RecordTypeAdd,
              added)

        End If

        If updated?.Any() Then

            Dim userActivityService = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
            Await userActivityService.CreateRecordAsync(
              z_User,
              FormEntityName,
              z_OrganizationID,
              UserActivityRepository.RecordTypeAdd,
              updated)

        End If

        If deleted?.Any() Then

            Dim userActivityService = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
            Await userActivityService.CreateRecordAsync(
              z_User,
              FormEntityName,
              z_OrganizationID,
              UserActivityRepository.RecordTypeDelete,
              deleted)

        End If
    End Function

    Private Function CreateAdjustmentUserActivityItems(Of T As IAdjustment)(
        allAjustments As BindingSource,
        paystub As Paystub,
        isActual As Boolean) As _
        (added As List(Of UserActivityItem), updated As List(Of UserActivityItem), deleted As List(Of UserActivityItem))

        Dim suffixIdentifier = $"for payroll { GetPayPeriodString()}."
        Dim entityName = If(isActual, "actual adjustment", "adjustment")

        Dim added As New List(Of UserActivityItem)
        Dim updated As New List(Of UserActivityItem)
        Dim deleted As New List(Of UserActivityItem)

        Dim adjustmentTypes = CType(cboProducts.DataSource, List(Of Product))

        Dim modifiedAdjustments As New List(Of T)
        For Each adjustment In allAjustments
            modifiedAdjustments.Add(CType(adjustment, T))
        Next

        Dim originalAdjustments = GetOriginalAdjustments(Of T)(isActual)

        For Each adjustment In originalAdjustments

            Dim updatedAdjustment = modifiedAdjustments.
                Where(Function(a) a.RowID.HasValue).
                Where(Function(a) a.RowID.Value = adjustment.RowID.Value).
                FirstOrDefault()

            If updatedAdjustment IsNot Nothing Then

                Dim oldType = GetAdjustmentTypeName(adjustmentTypes, adjustment.ProductID)

                If Not adjustment.ProductID.NullableEquals(updatedAdjustment.ProductID) Then

                    Dim newType = GetAdjustmentTypeName(adjustmentTypes, updatedAdjustment.ProductID)

                    updated.Add(New UserActivityItem() With {
                        .EntityId = adjustment.RowID.Value,
                        .Description = $"Updated type from '{ oldType }' to '{ newType }' of {entityName} with type {oldType} {suffixIdentifier}",
                        .ChangedEmployeeId = paystub.EmployeeID.Value
                    })

                ElseIf adjustment.Amount <> updatedAdjustment.Amount Then

                    updated.Add(New UserActivityItem() With {
                        .EntityId = adjustment.RowID.Value,
                        .Description = $"Updated amount from '{ adjustment.Amount }' to '{ updatedAdjustment.Amount }' of {entityName} with type {oldType} {suffixIdentifier}",
                        .ChangedEmployeeId = paystub.EmployeeID.Value
                    })

                ElseIf adjustment.Comment <> updatedAdjustment.Comment Then

                    updated.Add(New UserActivityItem() With {
                        .EntityId = adjustment.RowID.Value,
                        .Description = $"Updated comment from '{ adjustment.Comment }' to '{ updatedAdjustment.Comment }' of {entityName} with type {oldType} {suffixIdentifier}",
                        .ChangedEmployeeId = paystub.EmployeeID.Value
                    })

                End If
            Else
                Dim adjustmentType = GetAdjustmentTypeName(adjustmentTypes, adjustment.ProductID)

                deleted.Add(New UserActivityItem() With {
                    .EntityId = adjustment.RowID.Value,
                    .Description = $"Deleted an {entityName} with type {adjustmentType} {suffixIdentifier}",
                    .ChangedEmployeeId = paystub.EmployeeID.Value
                })
            End If

        Next

        For Each adjustment In modifiedAdjustments

            Dim originalAdjustment = originalAdjustments.
                Where(Function(a) a.RowID.HasValue).
                Where(Function(a) a.RowID.Value = adjustment.RowID.Value).
                FirstOrDefault()

            If originalAdjustment Is Nothing Then
                Dim adjustmentType = GetAdjustmentTypeName(adjustmentTypes, adjustment.ProductID)

                added.Add(New UserActivityItem() With {
                    .EntityId = adjustment.RowID.Value,
                    .Description = $"Created a new {entityName} with type {adjustmentType} {suffixIdentifier}",
                    .ChangedEmployeeId = paystub.EmployeeID.Value
                 })

            End If
        Next

        Return (added, updated, deleted)
    End Function

    Private Shared Function GetAdjustmentTypeName(adjustmentTypes As List(Of Product), productId As Integer?) As String

        Dim oldType = String.Empty

        If productId IsNot Nothing Then

            oldType = adjustmentTypes.Where(Function(a) a.RowID.Value = productId.Value).FirstOrDefault()?.PartNo

        End If

        Return oldType
    End Function

    Private Function GetOriginalAdjustments(Of T As IAdjustment)(isActual As Boolean) As List(Of T)

        Dim list As New List(Of T)

        If isActual Then

            _originalActualAdjustments.ForEach(
                Sub(a)
                    list.Add(CType(CType(a, Object), T))
                End Sub)
        Else

            _originalAdjustments.ForEach(
                Sub(a)
                    list.Add(CType(CType(a, Object), T))
                End Sub)

        End If

        Return list
    End Function

    Private Async Function UpdateAdjustments(Of T As IAdjustment)(paystub As Paystub, allAjustments As BindingSource) As Task
        Dim adjustments As New List(Of T)

        Dim isActual = GetType(ActualAdjustment).IsAssignableFrom(GetType(T))

        For Each item As T In allAjustments

            If item.ProductID Is Nothing Then

                Throw New BusinessLogicException("Adjustment type is required!")

            End If

            If item.Amount = 0 Then

                Throw New BusinessLogicException("Adjustment amount must be not be equal to zero.")

            End If

            item.IsActual = isActual
            item.OrganizationID = z_OrganizationID
            item.PaystubID = paystub.RowID.Value
            adjustments.Add(item)
        Next

        If adjustments.GroupBy(Function(a) a.ProductID).Any(Function(a) a.Count() > 1) Then

            Throw New BusinessLogicException("Multiple adjustment with the same adjustment type is not allowed.")
        End If

        Dim dataService = MainServiceProvider.GetRequiredService(Of PaystubDataService)
        Await dataService.UpdateAdjustmentsAsync(paystub.RowID.Value, adjustments.ToList(), z_User)
    End Function

    Private Async Function LoadAdjustmentsAsync() As Task

        Dim employeeId = ObjectUtils.ToNullableInteger(dgvemployees.Tag)
        If employeeId Is Nothing OrElse _currentPayperiodId Is Nothing Then Return

        Dim isActual = IsActualSelected()

        Dim paystub = Await _paystubRepository.GetByCompositeKeyAsync(
            New EmployeeCompositeKey(
                employeeId:=employeeId.Value,
                payPeriodId:=_currentPayperiodId.Value))

        If paystub Is Nothing OrElse paystub.RowID Is Nothing Then Return

        If isActual Then

            Dim adjustments = (Await _paystubRepository.GetActualAdjustmentsAsync(paystub.RowID.Value)).ToList()
            CreateOriginalActualAdjustments(adjustments)

            Dim bindingList As New BindingList(Of ActualAdjustment)(adjustments)
            AdjustmentGridView.DataSource = New BindingSource(bindingList, Nothing)
            AdjustmentTitleLabel.Text = $"Actual Adjustments (Declared: {FormatNumber(paystub.TotalAdjustments, 2)})"
        Else

            Dim adjustments = (Await _paystubRepository.GetAdjustmentsAsync(paystub.RowID.Value)).ToList()
            CreateOriginalAdjustments(adjustments)

            Dim bindingList As New BindingList(Of Adjustment)(adjustments)
            AdjustmentGridView.DataSource = New BindingSource(bindingList, Nothing)
            AdjustmentTitleLabel.Text = If(_policy.ShowActual, "Declared Adjustments", "Adjustments")
        End If
    End Function

    Private Sub CreateOriginalAdjustments(adjustments As List(Of Adjustment))

        If adjustments Is Nothing Then Return

        _originalAdjustments = New List(Of Adjustment)

        adjustments.ForEach(
            Sub(adjustment)
                _originalAdjustments.Add(New Adjustment() With {
                    .RowID = adjustment.RowID,
                    .OrganizationID = adjustment.OrganizationID,
                    .Created = adjustment.Created,
                    .CreatedBy = adjustment.CreatedBy,
                    .LastUpd = adjustment.LastUpd,
                    .LastUpdBy = adjustment.LastUpdBy,
                    .ProductID = adjustment.ProductID,
                    .PaystubID = adjustment.PaystubID,
                    .Amount = adjustment.Amount,
                    .Comment = adjustment.Comment,
                    .IsActual = adjustment.IsActual
                })
            End Sub)
    End Sub

    Private Sub CreateOriginalActualAdjustments(adjustments As List(Of ActualAdjustment))

        If adjustments Is Nothing Then Return

        _originalActualAdjustments = New List(Of ActualAdjustment)

        adjustments.ForEach(
            Sub(adjustment)
                _originalActualAdjustments.Add(New ActualAdjustment() With {
                    .RowID = adjustment.RowID,
                    .OrganizationID = adjustment.OrganizationID,
                    .Created = adjustment.Created,
                    .CreatedBy = adjustment.CreatedBy,
                    .LastUpd = adjustment.LastUpd,
                    .LastUpdBy = adjustment.LastUpdBy,
                    .ProductID = adjustment.ProductID,
                    .PaystubID = adjustment.PaystubID,
                    .Amount = adjustment.Amount,
                    .Comment = adjustment.Comment,
                    .IsActual = adjustment.IsActual
                })
            End Sub)
    End Sub

    Private Function IsActualSelected() As Boolean
        Return tabEarned.SelectedTab Is ActualTabPage
    End Function

    Private Sub dgvAdjustments_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles AdjustmentGridView.DataError
        EnableAdjustmentButtons(False)
    End Sub

    Private Sub dgvAdjustments_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles AdjustmentGridView.CellEndEdit
        EnableAdjustmentButtons(True)
    End Sub

    Private Sub dgvAdjustments_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles AdjustmentGridView.UserDeletedRow
        EnableAdjustmentButtons(True)
    End Sub

    Private Sub EnableAdjustmentButtons(enabled As Boolean)
        btnSaveAdjustments.Enabled = enabled
        btnDiscardChanges.Enabled = enabled

    End Sub

    Private Sub tabEarned_DrawItem(sender As Object, e As DrawItemEventArgs) Handles tabEarned.DrawItem
        TabControlColor(tabEarned, e)
    End Sub

    Private Sub ResetNumberTextBox()
        For Each txtbx In Panel6.Controls.OfType(Of TextBox)
            txtbx.Text = "0.00"
        Next
    End Sub

    Private Sub NumberFieldFormatter(sender As Object, e As EventArgs)
        If TypeOf sender Is TextBox Then
            Dim txtboxSender = DirectCast(sender, TextBox)
            txtboxSender.Text = FormatNumber(txtboxSender.Text, 2)
        End If
    End Sub

    Private Async Sub DeclaredTabPage_Enter(sender As Object, e As EventArgs)
        Await ShowDeclaredData()
    End Sub

    Private Async Function ShowDeclaredData() As Task
        DeclaredTabPage.Text = DeclaredTabPage.Text.Trim
        DeclaredTabPage.Text = DeclaredTabPage.Text & Space(15)
        ActualTabPage.Text = ActualTabPage.Text.Trim

        For Each txtbxctrl In DeclaredTabPage.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag

        Dim employeeId = ObjectUtils.ToNullableInteger(EmployeeRowID)
        If employeeId Is Nothing OrElse _currentPayperiodId Is Nothing Then
            Return
        End If

        Dim employee = Await _employeeRepository.GetByIdAsync(employeeId.Value)
        If employee Is Nothing Then Return

        Dim paystub = Await GetPaystubAsync(employee.RowID)
        If paystub Is Nothing Then Return

        Dim datePeriod = GetPayPeriodDates()
        If datePeriod Is Nothing Then Return

        Dim salary = Await _employeeRepository.GetCurrentSalaryAsync(employee.RowID.Value, datePeriod.End)
        If salary Is Nothing Then Return

        Dim basicRate = If(employee.IsDaily, salary.BasicSalary, salary.BasicSalary / 2)

        txtBasicRate.Text = FormatNumber(basicRate, 2)

        txtRegularHours.Text = FormatNumber(paystub.RegularHours, 2)
        txtOvertimeHours.Text = FormatNumber(paystub.OvertimeHours, 2)
        txtNightDiffHours.Text = FormatNumber(paystub.NightDiffHours, 2)
        txtNightDiffOvertimeHours.Text = FormatNumber(paystub.NightDiffOvertimeHours, 2)

        txtRegularPay.Text = FormatNumber(paystub.RegularPay, 2)
        txtOvertimePay.Text = FormatNumber(paystub.OvertimePay, 2)
        txtNightDiffPay.Text = FormatNumber(paystub.NightDiffPay, 2)
        txtNightDiffOvertimePay.Text = FormatNumber(paystub.NightDiffOvertimePay, 2)

        'Deductions
        txtAbsentHours.Text = FormatNumber(paystub.AbsentHours, 2)
        txtLateHours.Text = FormatNumber(paystub.LateHours, 2)
        txtUndertimeHours.Text = FormatNumber(paystub.UndertimeHours, 2)

        txtAbsenceDeduction.Text = FormatNumber(paystub.AbsenceDeduction, 2)
        txtLateDeduction.Text = FormatNumber(paystub.LateDeduction, 2)
        txtUndertimeDeduction.Text = FormatNumber(paystub.UndertimeDeduction, 2)

        'Holidays and leave
        txtRegularHolidayHours.Text = FormatNumber(paystub.RegularHolidayHours, 2)
        txtRegularHolidayOTHours.Text = FormatNumber(paystub.RegularHolidayOTHours, 2)
        txtSpecialHolidayHours.Text = FormatNumber(paystub.SpecialHolidayHours, 2)
        txtSpecialHolidayOTHours.Text = FormatNumber(paystub.SpecialHolidayOTHours, 2)
        txtRestDayHours.Text = FormatNumber(paystub.RestDayHours, 2)
        txtRestDayOtHours.Text = FormatNumber(paystub.RestDayOTHours, 2)
        txtLeaveHours.Text = FormatNumber(paystub.LeaveHours, 2)

        txtRegularHolidayPay.Text = FormatNumber(paystub.RegularHolidayPay, 2)
        txtRegularHolidayOTPay.Text = FormatNumber(paystub.RegularHolidayOTPay, 2)
        txtSpecialHolidayPay.Text = FormatNumber(paystub.SpecialHolidayPay, 2)
        txtSpecialHolidayOTPay.Text = FormatNumber(paystub.SpecialHolidayOTPay, 2)
        txtRestDayPay.Text = FormatNumber(paystub.RestDayPay, 2)
        txtRestDayOtPay.Text = FormatNumber(paystub.RestDayOTPay, 2)
        txtLeavePay.Text = FormatNumber(paystub.LeavePay, 2)

        'Allowance, Bonus and Gross Pay
        txtTotalNonTaxableAllowance.Text = FormatNumber(paystub.TotalNonTaxableAllowance, 2)
        txtTotalTaxableAllowance.Text = FormatNumber(paystub.TotalTaxableAllowance, 2)
        txtGrandTotalAllowance.Text = FormatNumber(paystub.GrandTotalAllowance, 2)

        txtTotalBonus.Text = FormatNumber(paystub.TotalBonus, 2)
        txtGrossPay.Text = FormatNumber(paystub.GrossPay, 2)

        'Government Deductions
        txtSssEmployeeShare.Text = FormatNumber(paystub.SssEmployeeShare, 2)
        txtPhilHealthEmployeeShare.Text = FormatNumber(paystub.PhilHealthEmployeeShare, 2)
        txtHdmfEmployeeShare.Text = FormatNumber(paystub.HdmfEmployeeShare, 2)

        'Other Deductions
        txtTotalTaxableSalary.Text = FormatNumber(paystub.TaxableIncome, 2)
        txtWithholdingTax.Text = FormatNumber(paystub.WithholdingTax, 2)

        txtTotalLoans.Text = FormatNumber(paystub.TotalLoans, 2)
        txtTotalAdjustments.Text = FormatNumber(paystub.TotalAdjustments, 2)

        Dim totalAgencyFee = _agencyFeeRepository.GetPaystubAmount(
            organizationId:=z_OrganizationID,
            timePeriod:=datePeriod,
            employeeId:=employee.RowID.Value)

        txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

        '13th month and Net Pay
        Dim thirteenthMonthPay = If(paystub.ThirteenthMonthPay?.Amount, 0)
        txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

        Dim totalNetSalary = paystub.NetPay + totalAgencyFee
        txtNetPay.Text = FormatNumber(totalNetSalary, 2)
        txtTotalNetPay.Text = FormatNumber(totalNetSalary + thirteenthMonthPay, 2)

        Await LoadAdjustmentsAsync()
    End Function

    Private Async Sub ActualTabPage_Enter(sender As Object, e As EventArgs)
        Await ShowActualData()
    End Sub

    Private Async Function ShowActualData() As Task
        ActualTabPage.Text = ActualTabPage.Text.Trim
        ActualTabPage.Text = ActualTabPage.Text & Space(15)
        DeclaredTabPage.Text = DeclaredTabPage.Text.Trim

        For Each txtbxctrl In ActualTabPage.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag
        Dim employeeId = ObjectUtils.ToNullableInteger(EmployeeRowID)
        If employeeId Is Nothing OrElse _currentPayperiodId Is Nothing Then
            Return
        End If

        Dim employee = Await _employeeRepository.GetByIdAsync(employeeId.Value)
        If employee Is Nothing Then Return

        Dim paystub = Await GetPaystubAsync(employee.RowID)
        If paystub Is Nothing Then Return

        Dim datePeriod = GetPayPeriodDates()
        If datePeriod Is Nothing Then Return

        Dim salary = Await _employeeRepository.GetCurrentSalaryAsync(employee.RowID.Value, datePeriod.End)
        If salary Is Nothing Then Return

        Dim basicRate = If(employee.IsDaily, salary.TotalSalary, salary.TotalSalary / 2)

        txtBasicRateActual.Text = FormatNumber(basicRate, 2)

        txtRegularHoursActual.Text = FormatNumber(paystub.RegularHours, 2)
        txtOvertimeHoursActual.Text = FormatNumber(paystub.OvertimeHours, 2)
        txtNightDiffHoursActual.Text = FormatNumber(paystub.NightDiffHours, 2)
        NightDiffOvertimeHoursActual.Text = FormatNumber(paystub.NightDiffOvertimeHours, 2)

        txtRegularPayActual.Text = FormatNumber(paystub.Actual?.RegularPay, 2)
        txtOvertimePayActual.Text = FormatNumber(paystub.Actual?.OvertimePay, 2)
        txtNightDiffPayActual.Text = FormatNumber(paystub.Actual?.NightDiffPay, 2)
        txtNightDiffOvertimePayActual.Text = FormatNumber(paystub.Actual?.NightDiffOvertimePay, 2)

        'Deductions
        txtAbsentHoursActual.Text = FormatNumber(paystub.AbsentHours, 2)
        txtLateHoursActual.Text = FormatNumber(paystub.LateHours, 2)
        txtUndertimeHoursActual.Text = FormatNumber(paystub.UndertimeHours, 2)

        txtAbsenceDeductionActual.Text = FormatNumber(paystub.Actual?.AbsenceDeduction, 2)
        txtLateDeductionActual.Text = FormatNumber(paystub.Actual?.LateDeduction, 2)
        txtUndertimeDeductionActual.Text = FormatNumber(paystub.Actual?.UndertimeDeduction, 2)

        'Holidays and leave
        txtRegularHolidayHours.Text = FormatNumber(paystub.RegularHolidayHours, 2)
        txtRegularHolidayOTHours.Text = FormatNumber(paystub.RegularHolidayOTHours, 2)
        txtSpecialHolidayHours.Text = FormatNumber(paystub.SpecialHolidayHours, 2)
        txtSpecialHolidayOTHours.Text = FormatNumber(paystub.SpecialHolidayOTHours, 2)
        txtRestDayHours.Text = FormatNumber(paystub.RestDayHours, 2)
        txtRestDayOtHours.Text = FormatNumber(paystub.RestDayOTHours, 2)
        txtLeaveHours.Text = FormatNumber(paystub.LeaveHours, 2)

        txtRegularHolidayPay.Text = FormatNumber(paystub.Actual?.RegularHolidayPay, 2)
        txtRegularHolidayOTPay.Text = FormatNumber(paystub.Actual?.RegularHolidayOTPay, 2)
        txtSpecialHolidayPay.Text = FormatNumber(paystub.Actual?.SpecialHolidayPay, 2)
        txtSpecialHolidayOTPay.Text = FormatNumber(paystub.Actual?.SpecialHolidayOTPay, 2)
        txtRestDayPay.Text = FormatNumber(paystub.Actual?.RestDayPay, 2)
        txtRestDayOtPay.Text = FormatNumber(paystub.Actual?.RestDayOTPay, 2)
        txtLeavePay.Text = FormatNumber(paystub.Actual?.LeavePay, 2)

        'Allowance, Bonus and Gross Pay
        txtTotalNonTaxableAllowance.Text = FormatNumber(paystub.TotalNonTaxableAllowance, 2)
        txtTotalTaxableAllowance.Text = FormatNumber(paystub.TotalTaxableAllowance, 2)
        txtGrandTotalAllowance.Text = FormatNumber(paystub.GrandTotalAllowance, 2)

        txtTotalBonus.Text = FormatNumber(paystub.TotalBonus, 2)
        txtGrossPay.Text = FormatNumber(paystub.Actual?.GrossPay, 2)

        'Government Deductions
        txtSssEmployeeShare.Text = FormatNumber(paystub.SssEmployeeShare, 2)
        txtPhilHealthEmployeeShare.Text = FormatNumber(paystub.PhilHealthEmployeeShare, 2)
        txtHdmfEmployeeShare.Text = FormatNumber(paystub.HdmfEmployeeShare, 2)

        'Other Deductions
        txtTotalTaxableSalary.Text = FormatNumber(paystub.TaxableIncome, 2)
        txtWithholdingTax.Text = FormatNumber(paystub.WithholdingTax, 2)

        txtTotalLoans.Text = FormatNumber(paystub.TotalLoans, 2)
        txtTotalAdjustments.Text = FormatNumber(paystub.Actual?.TotalAdjustments, 2)

        Dim totalAgencyFee = _agencyFeeRepository.GetPaystubAmount(
            organizationId:=z_OrganizationID,
            timePeriod:=datePeriod,
            employeeId:=employee.RowID.Value)

        txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

        '13th month and Net Pay
        Dim thirteenthMonthPay = If(paystub.ThirteenthMonthPay?.Amount, 0)
        txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

        Dim totalNetSalary = paystub.Actual?.NetPay + totalAgencyFee
        txtNetPay.Text = FormatNumber(totalNetSalary, 2)
        txtTotalNetPay.Text = FormatNumber(totalNetSalary + thirteenthMonthPay, 2)

        Dim totalNetPay = totalNetSalary + thirteenthMonthPay
        txtTotalNetPay.Text = FormatNumber(totalNetPay, 2)

        Await LoadAdjustmentsAsync()
    End Function

    Private Async Function GetPaystubAsync(EmployeeRowID As Object) As Task(Of Paystub)
        Dim employeeId = ObjectUtils.ToNullableInteger(EmployeeRowID)

        If employeeId Is Nothing OrElse _currentPayperiodId Is Nothing Then
            Return Nothing
        End If

        Return Await _paystubRepository.GetByCompositeKeyWithActualAndThirteenthMonthAsync(
            New EmployeeCompositeKey(employeeId:=employeeId.Value, payPeriodId:=_currentPayperiodId.Value))
    End Function

    Private Function GetPayPeriodDates() As TimePeriod
        Dim dateTo = ObjectUtils.ToNullableDateTime(_currentPayToDate)

        If _currentPayFromDate Is Nothing OrElse dateTo Is Nothing Then
            Return Nothing
        End If

        Return New TimePeriod(_currentPayFromDate.Value, dateTo.Value)
    End Function

    Private Async Sub btnDiscardChanges_Click(sender As Object, e As EventArgs) Handles btnDiscardChanges.Click
        Await LoadAdjustmentsAsync()

        EnableAdjustmentButtons(False)
    End Sub

    Private Async Sub AddNewAdjustmentTypeLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles AddNewAdjustmentTypeLinkLabel.LinkClicked

        Dim n_ProdCtrlForm As New ProdCtrlForm

        With n_ProdCtrlForm

            .Status.HeaderText = "Taxable Flag"

            .PartNo.HeaderText = "Item Name"

            .NameOfCategory = "Adjustment Type"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                Await PopulateAdjustmentTypeComboBox()

            End If

        End With

    End Sub

    Private Async Function DeletePaystub() As Task
        Dim employeeId = ObjectUtils.ToNullableInteger(dgvemployees.Tag)
        If employeeId Is Nothing OrElse _currentPayperiodId Is Nothing OrElse currentEmployeeNumber = Nothing Then

            MessageBoxHelper.Warning("No selected paystub.", "Delete Paystub")
            Return

        End If

        Dim toBeDeletedEmployeeNumber = currentEmployeeNumber
        Dim toBeDeletedPaypFrom = _currentPayFromDate
        Dim toBeDeletedPaypTo = _currentPayToDate

        Dim payPeriodString As String = GetPayPeriodString()

        Dim prompt = MessageBox.Show(
            "Are you sure you want to delete the " & payPeriodString &
            " payroll of employee '" & toBeDeletedEmployeeNumber & "' ?",
            "Delete employee payroll",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If prompt = Windows.Forms.DialogResult.Yes Then

            Me.Cursor = Cursors.WaitCursor

            Await FunctionUtils.TryCatchFunctionAsync("Delete Paystub",
                Async Function()

                    Dim paystub = Await _paystubRepository.GetByCompositeKeyAsync(
                        New EmployeeCompositeKey(
                            employeeId:=employeeId.Value,
                            payPeriodId:=_currentPayperiodId.Value))

                    Dim paystubDataService = MainServiceProvider.GetRequiredService(Of PaystubDataService)
                    Await paystubDataService.DeleteAsync(
                        paystub,
                        userId:=z_User,
                        organizationId:=z_OrganizationID)

                    Await RecordDeletePaystubUserActivity(payPeriodString, paystub)

                    Await RefreshForm()

                    Await TimeEntrySummaryForm.LoadPayPeriods()

                    MessageBoxHelper.Information($"Paystub of employee {toBeDeletedEmployeeNumber} for payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}' was successfully deleted.")
                End Function)

            Me.Cursor = Cursors.Default
        End If

    End Function

    Private Async Function RecordDeletePaystubUserActivity(payPeriodString As String, paystub As Paystub) As Task

        Dim activityItem = New List(Of UserActivityItem) From {
            New UserActivityItem() With
            {
                .EntityId = paystub.RowID.Value,
                .Description = $"Deleted a paystub for payroll {payPeriodString}.",
                .ChangedEmployeeId = paystub.EmployeeID.Value
            }
        }

        Dim userActivityService = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
        Await userActivityService.CreateRecordAsync(
          z_User,
          FormEntityName,
          z_OrganizationID,
          UserActivityRepository.RecordTypeDelete,
          activityItem)
    End Function

    Private Sub SetProperInterfaceBaseOnCurrentSystemOwner()
        If Not _policy.ShowActual Then
            Dim str_empty As String = String.Empty
            DeclaredTabPage.Text = str_empty
            ActualTabPage.Text = str_empty
            AddHandler tabEarned.Selecting, AddressOf tabEarned_Selecting
        End If

        If Not _policy.UseAgency Then
            lblAgencyFee.Visible = False
            lblAgencyFeePesoSign.Visible = False
            txtAgencyFee.Visible = False
        End If

        If Not _policy.UseBonus Then
            lblTotalBonus.Visible = False
            lblTotalBonusPesoSign.Visible = False
            txtTotalBonus.Visible = False

            btntotbon.Visible = False
        End If

    End Sub

    Private Sub tabEarned_Selecting(sender As Object, e As TabControlCancelEventArgs)

        e.Cancel = Not _policy.ShowActual

    End Sub

    Private Enum PaginationAction
        First
        Last
        [Next]
        Previous
    End Enum

    Private Async Sub ClosePayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClosePayrollToolStripMenuItem.Click

        Await FunctionUtils.TryCatchFunctionAsync("Close Payroll",
            Async Function()
                Await UpdatePayrollStatus(close:=True)
            End Function)

    End Sub

    Private Async Sub ReopenPayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReopenPayrollToolStripMenuItem.Click

        Await FunctionUtils.TryCatchFunctionAsync("Reopen Payroll",
            Async Function()
                Await UpdatePayrollStatus(close:=False)
            End Function)

    End Sub

    Private Async Function UpdatePayrollStatus(close As Boolean) As Task(Of Boolean)

        If _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("Please select a pay period first.")
            Return False
        End If

        Dim payPeriod = Await _payPeriodRepository.GetByIdAsync(_currentPayperiodId.Value)

        If payPeriod Is Nothing Then

            MessageBoxHelper.Warning("Pay period does not exists. Please refresh the form.")
            Return False
        End If

        Dim dataService = MainServiceProvider.GetRequiredService(Of PayPeriodDataService)
        If close Then

            Await dataService.CloseAsync(payPeriod.RowID.Value, z_User)
            Await RecordPayrollStatusUpdateAsync(payPeriod.RowID.Value, $"Closed the payroll {GetPayPeriodString()}.")
        Else

            Await dataService.ReopenAsync(payPeriod.RowID.Value, z_User)
            Await RecordPayrollStatusUpdateAsync(payPeriod.RowID.Value, $"Reopened the payroll {GetPayPeriodString()}.")
        End If

        Await RefreshForm()

        Await TimeEntrySummaryForm.LoadPayPeriods()

        If close Then

            MessageBoxHelper.Information("Pay period was closed successfully.")
        Else
            MessageBoxHelper.Information("Pay period was reopened successfully.")

        End If

        Return True

    End Function

    Private Async Function RecordPayrollStatusUpdateAsync(payPeriodId As Integer, description As String) As Task

        Dim payPeriodString = GetPayPeriodString()

        Dim activityItem = New List(Of UserActivityItem) From {
            New UserActivityItem() With
            {
                .EntityId = payPeriodId,
                .Description = description,
                .ChangedEmployeeId = Nothing
            }
        }

        Dim userActivityService = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
        Await userActivityService.CreateRecordAsync(
          z_User,
          FormEntityName,
          z_OrganizationID,
          UserActivityRepository.RecordTypeEdit,
          activityItem)
    End Function

    Private Async Function RefreshForm() As Task
        Await VIEW_payperiodofyear()
    End Function

    Private Async Sub GeneratePayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GeneratePayrollToolStripButton.Click

        If _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected pay period.", "Delete Payroll")
            Return

        End If

        Dim payPeriodString As String = GetPayPeriodString()

        Dim confirm = MessageBoxHelper.Confirm(Of Boolean)(
            $"Generate payroll for {payPeriodString}?",
            "Generate Payroll")

        If Not confirm Then Return

        Await GeneratePayroll()

    End Sub

    Private Sub CashOutUnusedLeavesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CashOutUnusedLeavesToolStripMenuItem.Click
        If _currentPayperiodId Is Nothing Then
            MsgBox("Please select a generated payroll.", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim payPeriodSelector = New MultiplePayPeriodSelectionDialog()

        If payPeriodSelector.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim dateFromId = payPeriodSelector.PayPeriodFromID
        Dim dateToId = payPeriodSelector.PayPeriodToID

        ' Code not tested
        'Dim cashOut = New CashOutUnusedLeave(dateFromId, dateToId, paypRowID, z_OrganizationID, z_User)
        'cashOut.Execute()
    End Sub

    Private Async Sub DeletePayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeletePaystubsToolStripMenuItem.Click
        DeletePaystubsToolStripMenuItem.Enabled = False

        Dim toBeDeletedPaypFrom = _currentPayFromDate
        Dim toBeDeletedPaypTo = _currentPayToDate

        If _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected pay period.", "Delete Payroll")
            Return

        End If

        Dim payPeriodString As String = GetPayPeriodString()

        Dim prompt = MessageBox.Show(
            $"Are you sure you want to delete ALL paystubs of employees for payroll {payPeriodString}?",
            "Delete all employee paystubs",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If prompt = Windows.Forms.DialogResult.Yes Then

            Await FunctionUtils.TryCatchFunctionAsync("Delete ALL Pastubs",
                Async Function()

                    Dim paystubs = Await _paystubRepository.GetByPayPeriodWithEmployeeDivisionAsync(_currentPayperiodId.Value)

                    Dim paystubDataService = MainServiceProvider.GetRequiredService(Of PaystubDataService)
                    Await paystubDataService.DeleteByPeriodAsync(
                    payPeriodId:=_currentPayperiodId.Value,
                    userId:=z_User,
                    organizationId:=z_OrganizationID)

                    Dim recordMessage = $"Deleted a paystub for payroll {payPeriodString}."
                    Await RecordMultiplePaystubUserActivity(recordMessage, paystubs)

                    Await RefreshForm()

                    Await TimeEntrySummaryForm.LoadPayPeriods()

                    MessageBoxHelper.Information($"All paystubs for payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}' were successfully deleted.")
                End Function)

        End If

        DeletePaystubsToolStripMenuItem.Enabled = True
    End Sub

    Private Function GetPayPeriodString() As String
        Return $"'{CDate(_currentPayFromDate).ToShortDateString()}' to '{CDate(_currentPayToDate).ToShortDateString()}'"
    End Function

    Private Shared Async Function RecordMultiplePaystubUserActivity(
        description As String,
        paystubs As ICollection(Of Paystub)) As Task

        Dim changes = New List(Of UserActivityItem)

        For Each paystub In paystubs
            changes.Add(New UserActivityItem() With
            {
                .EntityId = paystub.RowID.Value,
                .Description = description,
                .ChangedEmployeeId = paystub.EmployeeID.Value
            })
        Next

        If changes.Any() Then

            Dim repo = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
            Await repo.CreateRecordAsync(
              z_User,
              FormEntityName,
              z_OrganizationID,
              UserActivityRepository.RecordTypeDelete,
              changes)

        End If
    End Function

    Private Async Sub CancelPayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CancelPayrollToolStripMenuItem.Click
        CancelPayrollToolStripMenuItem.Enabled = False

        Dim toBeDeletedPaypFrom = _currentPayFromDate
        Dim toBeDeletedPaypTo = _currentPayToDate

        If _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected pay period.", "Delete Payroll")
            Return

        End If

        Dim payPeriodString As String = GetPayPeriodString()

        Dim prompt = MessageBox.Show(
            $"Are you sure you want to cancel the payroll and delete ALL paystubs and time entries of employees for payroll {payPeriodString}?",
            "Cancel payroll",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If prompt = Windows.Forms.DialogResult.Yes Then

            Await FunctionUtils.TryCatchFunctionAsync("Cancel Payroll",
                Async Function()

                    Dim paystubs = Await _paystubRepository.GetByPayPeriodWithEmployeeDivisionAsync(_currentPayperiodId.Value)

                    Dim payperiodDataService = MainServiceProvider.GetRequiredService(Of PayPeriodDataService)
                    Await payperiodDataService.CancelAsync(_currentPayperiodId.Value, z_User)

                    Dim recordMessage = $"Deleted paystub and time entries for payroll {payPeriodString}."

                    Await RecordMultiplePaystubUserActivity(recordMessage, paystubs)
                    Await RecordPayrollStatusUpdateAsync(_currentPayperiodId.Value, $"Cancelled the payroll {GetPayPeriodString()}.")

                    Await RefreshForm()
                    Await TimeEntrySummaryForm.LoadPayPeriods()

                    MessageBoxHelper.Information($"Payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}' was successfully cancelled.")
                End Function)

        End If

        CancelPayrollToolStripMenuItem.Enabled = True
    End Sub

    Private Async Sub PrintAllPaySlip_Click(sender As Object, e As EventArgs) Handles _
        PayslipDeclaredToolStripMenuItem.Click,
        PayslipActualToolStripMenuItem.Click,
        PrintPaySlipToolStripMenuItem.Click

        Dim isActual = sender Is PayslipActualToolStripMenuItem

        Await PrintPayslip(isActual)
    End Sub

    Private Async Function PrintPayslip(isActual As Boolean) As Task

        If _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("Please select a pay period first.")
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Print Payslip",
            Async Function()
                Dim payslipBuilder = MainServiceProvider.GetRequiredService(Of PayslipBuilder)

                Dim reportDocument = Await payslipBuilder.CreateReportDocumentAsync(
                    payPeriodId:=_currentPayperiodId.Value,
                    isActual:=isActual)

                Dim crvwr As New CrysRepForm
                crvwr.crysrepvwr.ReportSource = reportDocument.GetReportDocument()
                crvwr.Show()

            End Function,
            "Error generating payslips.")
    End Function

    Private Sub ManageEmailPayslipsToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles ManagePrintPayslipsToolStripMenuItem.Click,
                ManageEmailPayslipsToolStripMenuItem.Click

        Dim form As SelectPayslipEmployeesForm

        If _currentPayperiodId Is Nothing Then

            MessageBoxHelper.Warning("Please select a pay period first.")
            Return
        End If

        If sender Is ManageEmailPayslipsToolStripMenuItem Then
            form = New SelectPayslipEmployeesForm(_currentPayperiodId.Value, isEmail:=True)
        Else
            form = New SelectPayslipEmployeesForm(_currentPayperiodId.Value, isEmail:=False)
        End If

        form.ShowDialog()

    End Sub

    Private Sub CostCenterReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles _
        CostCenterReportAllToolStripMenuItem.Click,
        CostCenterReportAllDeclaredToolStripMenuItem.Click,
        CostCenterReportAllActualToolStripMenuItem.Click,
        CostCenterReportByBranchToolStripMenuItem.Click,
        CostCenterReportByBranchDeclaredToolStripMenuItem.Click,
        CostCenterReportByBranchActualToolStripMenuItem.Click

        Dim provider = New CostCenterReportProvider()

        provider.IsActual = sender Is CostCenterReportAllActualToolStripMenuItem OrElse sender Is CostCenterReportByBranchActualToolStripMenuItem

        provider.SelectedReportType = If(
            sender Is CostCenterReportByBranchToolStripMenuItem OrElse
            sender Is CostCenterReportByBranchDeclaredToolStripMenuItem OrElse
            sender Is CostCenterReportByBranchActualToolStripMenuItem,
            CostCenterReportProvider.ReportType.Branch,
            CostCenterReportProvider.ReportType.All)

        provider.Owner = Me

        provider.Run()

    End Sub

    Private Sub RecalculateThirteenthMonthPayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecalculateThirteenthMonthPayToolStripMenuItem.Click

        If _currentPayperiodId Is Nothing Then Return

        Dim form As New SelectRecalculateThirteenthMonthEmployeesForm(_currentPayperiodId.Value)

        form.ShowDialog()

    End Sub

    Private Async Sub ReleaseThirteenthMonthPayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Include13thMonthPayToolStripMenuItem.Click

        If _currentPayperiodId Is Nothing Then Return

        Dim form As New SelectReleaseThirteenthMonthEmployeesForm(_currentPayperiodId.Value)

        form.ShowDialog()

        If form.HasChanges Then

            Dim adjustmentTypes = CType(cboProducts.DataSource, List(Of Product))

            If Not adjustmentTypes.Any(Function(a) a.PartNo = ProductConstant.THIRTEENTH_MONTH_PAY_ADJUSTMENT) Then

                Dim productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

                Dim thirteenthMonthPayAdjustment = Await productRepository.GetOrCreateAdjustmentTypeAsync(
                ProductConstant.THIRTEENTH_MONTH_PAY_ADJUSTMENT,
                organizationId:=z_OrganizationID,
                userId:=z_User)

                Await PopulateAdjustmentTypeComboBox()

            End If

            Await UpdateEmployeeDetails()
        End If

    End Sub

    Private Async Sub ExportNetPayDetailsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles _
        ExportNetPayAllToolStripMenuItem.Click,
        ExportNetPayCashToolStripMenuItem.Click,
        ExportNetPayDirectDepositToolStripMenuItem.Click,
 _
        ExportNetPayDeclaredAllToolStripMenuItem.Click,
        ExportNetPayDeclaredCashToolStripMenuItem.Click,
        ExportNetPayDeclaredDirectDepositToolStripMenuItem.Click,
 _
        ExportNetPayActualAllToolStripMenuItem.Click,
        ExportNetPayActualCashToolStripMenuItem.Click,
        ExportNetPayActualDirectDepositToolStripMenuItem.Click

        Dim datePeriod = GetPayPeriodDates()
        If datePeriod Is Nothing Then

            MessageBoxHelper.Warning("No pay period selected.")
            Return
        End If

        Dim paystubDateKey = New DateCompositeKey(
            z_OrganizationID,
            payFromDate:=datePeriod.Start,
            payToDate:=datePeriod.End)

        Dim paystubs = Await _paystubRepository.GetAllWithEmployeeAsync(paystubDateKey)

        If paystubs?.Count = 0 Then

            MessageBoxHelper.Warning($"No paystub for payroll {datePeriod.Start.ToShortDateString()} to {datePeriod.End.ToShortDateString()} yet.")
            Return
        End If

        Dim paystubFilter = paystubs.
            OrderBy(Function(p) p.Employee.LastName).
            ThenBy(Function(p) p.Employee.FirstName).
            AsQueryable()

        Dim payrollCategory = PayrollSummaryCategory.All
        Dim isActual = True

        If sender Is ExportNetPayDeclaredAllToolStripMenuItem OrElse sender Is ExportNetPayAllToolStripMenuItem Then
            payrollCategory = PayrollSummaryCategory.All
            isActual = False

        ElseIf sender Is ExportNetPayDeclaredCashToolStripMenuItem OrElse sender Is ExportNetPayCashToolStripMenuItem Then
            payrollCategory = PayrollSummaryCategory.Cash
            isActual = False
            paystubFilter = paystubFilter.Where(Function(p) String.IsNullOrWhiteSpace(p.Employee.AtmNo))

        ElseIf sender Is ExportNetPayDeclaredDirectDepositToolStripMenuItem OrElse sender Is ExportNetPayDirectDepositToolStripMenuItem Then
            payrollCategory = PayrollSummaryCategory.DirectDeposit
            isActual = False
            paystubFilter = paystubFilter.Where(Function(p) Not String.IsNullOrWhiteSpace(p.Employee.AtmNo))

        ElseIf sender Is ExportNetPayActualAllToolStripMenuItem Then
            payrollCategory = PayrollSummaryCategory.All
            isActual = True

        ElseIf sender Is ExportNetPayActualCashToolStripMenuItem Then
            payrollCategory = PayrollSummaryCategory.Cash
            isActual = True
            paystubFilter = paystubFilter.Where(Function(p) String.IsNullOrWhiteSpace(p.Employee.AtmNo))

        ElseIf sender Is ExportNetPayActualDirectDepositToolStripMenuItem Then
            payrollCategory = PayrollSummaryCategory.DirectDeposit
            isActual = True
            paystubFilter = paystubFilter.Where(Function(p) Not String.IsNullOrWhiteSpace(p.Employee.AtmNo))

        End If

        paystubs = paystubFilter.ToList()

        Try

            ExportNetPayDetails(datePeriod, paystubs, payrollCategory, isActual)
        Catch ex As IOException

            MessageBoxHelper.ErrorMessage(ex.Message)
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage()

        End Try

    End Sub

    Private Shared Sub ExportNetPayDetails(datePeriod As TimePeriod, paystubs As ICollection(Of Paystub), payrollCategory As String, isActual As Boolean)
        Dim saveFileDialog = New SaveFileDialog()
        saveFileDialog.FileName = $"{z_CompanyName}NetPay{payrollCategory}Report{datePeriod.Start.ToShortDateString().Replace("/", "-")}TO{datePeriod.End.ToShortDateString().Replace("/", "-")}"
        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx"

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            Dim fileName = saveFileDialog.FileName
            Dim file = New FileInfo(fileName)

            If file.Exists() Then

                file.Delete()
            End If

            Using excelPackage = New ExcelPackage(file)
                Dim worksheet = excelPackage.Workbook.Worksheets.Add("NetPay")

                Dim i = 1
                For Each paystub In paystubs
                    worksheet.Cells($"A{i}").Value = $"{paystub.Employee.LastName}, {paystub.Employee.FirstName}"
                    worksheet.Cells($"B{i}").Value = paystub.Employee.AtmNo
                    worksheet.Cells($"C{i}").Value = If(isActual, paystub.Actual.NetPay, paystub.NetPay)

                    worksheet.Cells($"C{i}").Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)"
                    worksheet.Cells($"C{i}").Style.HorizontalAlignment = ExcelHorizontalAlignment.Right

                    i += 1

                Next

                excelPackage.Save()
                Process.Start(fileName)
            End Using
        End If
    End Sub

    Private Sub UserActivityToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityToolStripButton.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

End Class
