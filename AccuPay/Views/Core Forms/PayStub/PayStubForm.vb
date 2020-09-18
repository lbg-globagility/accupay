Imports System.Collections.Concurrent
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.CrystalReports
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Repositories.PaystubRepository
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports log4net
Imports Microsoft.Extensions.DependencyInjection
Imports MySql.Data.MySqlClient
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class PayStubForm

    Private _logger As ILog = LogManager.GetLogger("PayrollLogger")

    Public Property CurrentYear As String = CDate(dbnow).Year

    Private Const ItemsPerPage As Integer = 20

    Private _pageNo As Integer = 0

    Private _lastPageNo As Integer = 0

    Private _totalItems As Integer = 0

    Dim employeepicture As New DataTable

    Public paypFrom As String = Nothing
    Public paypTo As String = Nothing
    Public paypRowID As String = Nothing
    Public paypPayFreqID As String = Nothing
    Public isEndOfMonth As String = 0

    Private _totalPaystubs As Integer = 0
    Private _finishedPaystubs As Integer = 0

    Dim currentEmployeeNumber As String = Nothing

    Public withthirteenthmonthpay As SByte = 0

    Dim IsUserPressEnterToSearch As Boolean = False

    Dim selectedButtonFont = New Font("Trebuchet MS", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont = New Font("Trebuchet MS", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))

    Dim quer_empPayFreq = ""

    Dim dtJosh As DataTable
    Dim da As New MySqlDataAdapter()

    Private _results As BlockingCollection(Of PayrollGeneration.Result)

    Private _policy As PolicyHelper

    Private _systemOwnerService As SystemOwnerService

    Private _payPeriodRepository As PayPeriodRepository

    Private _paystubRepository As PaystubRepository

    Private _agencyFeeRepository As AgencyFeeRepository

    Private _employeeRepository As EmployeeRepository

    Sub New()

        InitializeComponent()

        _results = New BlockingCollection(Of PayrollGeneration.Result)()

        _policy = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

        _paystubRepository = MainServiceProvider.GetRequiredService(Of PaystubRepository)

        _agencyFeeRepository = MainServiceProvider.GetRequiredService(Of AgencyFeeRepository)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        SplitContainer1.SplitterWidth = 6

        Dim dgvVisibleRows = dgvpayper.Columns.Cast(Of DataGridViewColumn).Where(Function(ii) ii.Visible = True)

        Dim scrollbarwidth = 19

        Dim mincolwidth As Integer = (dgvpayper.Width - (dgvpayper.RowHeadersWidth + scrollbarwidth)) / dgvVisibleRows.Count

        For Each dgvcol In dgvVisibleRows
            dgvcol.Width = mincolwidth
            dgvcol.SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        setProperInterfaceBaseOnCurrentSystemOwner()

        For Each txtbx In Panel6.Controls.OfType(Of TextBox)
            AddHandler txtbx.TextChanged, AddressOf NumberFieldFormatter
        Next

        MyBase.OnLoad(e)

    End Sub

    Private Sub PayStub_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dgvAdjustments.AutoGenerateColumns = False

        RefreshForm()

        dgvpayper.Focus()

        linkPrev.Text = "← " & (CurrentYear - 1)
        linkNxt.Text = (CurrentYear + 1) & " →"

        If dgvemployees.RowCount <> 0 Then
            dgvemployees.Item("EmployeeID", 0).Selected = 1
        End If

        cboProducts.ValueMember = "ProductID"
        cboProducts.DisplayMember = "ProductName"

        cboProducts.DataSource = New SQLQueryToDatatable("SELECT RowID AS 'ProductID', Name AS 'ProductName', Category FROM product WHERE Category IN ('Allowance Type', 'Bonus', 'Adjustment Type')" &
                                                  " AND OrganizationID='" & orgztnID & "' ORDER BY Name;").ResultTable

        ShowOrHideActual()
        ShowOrHideEmailPayslip()
        ShowOrHidePayrollSummaryByBranch()

    End Sub

    Private Sub PayStub_EnabledChanged(sender As Object, e As EventArgs) Handles Me.EnabledChanged

        Dim _bool As Boolean = Me.Enabled

        ''###### CONTROL DISABLER ######

        PayrollForm.MenuStrip1.Enabled = _bool
        MDIPrimaryForm.Showmainbutton.Enabled = _bool
        ToolStrip1.Enabled = _bool

        Panel5.Enabled = _bool

        MDIPrimaryForm.systemprogressbar.Value = 0
        MDIPrimaryForm.systemprogressbar.Visible = CBool(Not _bool)

    End Sub

    Private Sub ShowOrHidePayrollSummaryByBranch()
        CostCenterReportToolStripMenuItem.Visible = _policy.PayRateCalculationBasis =
                                                            PayRateCalculationBasis.Branch
    End Sub

    Private Sub ShowOrHideActual()

        Dim showActual = _policy.ShowActual

        PayrollSummaryDeclaredToolStripMenuItem.Visible = showActual
        PayrollSummaryActualToolStripMenuItem.Visible = showActual

        PayslipDeclaredToolStripMenuItem.Visible = showActual
        PayslipActualToolStripMenuItem.Visible = showActual

        ExportNetPayDeclaredAllToolStripMenuItem.Visible = showActual
        ExportNetPayDeclaredCashToolStripMenuItem.Visible = showActual
        ExportNetPayDeclaredDirectDepositToolStripMenuItem.Visible = showActual
        ExportNetPayActualAllToolStripMenuItem.Visible = showActual
        ExportNetPayActualCashToolStripMenuItem.Visible = showActual
        ExportNetPayActualDirectDepositToolStripMenuItem.Visible = showActual

        If showActual = False Then

            Dim str_empty As String = String.Empty

            TabPage1.Text = str_empty

            TabPage4.Text = str_empty

            AddHandler tabEarned.Selecting, AddressOf tabEarned_Selecting

            PrintPayrollSummaryToolStripDropDownButton.Visible = False
            PrintPayrollSummaryToolStripButton.Visible = True
        Else

            PrintPayrollSummaryToolStripDropDownButton.Visible = True
            PrintPayrollSummaryToolStripButton.Visible = False

            RemoveHandler ExportNetPayAllToolStripMenuItem.Click, AddressOf ExportNetPayDetailsToolStripMenuItem_Click
            RemoveHandler ExportNetPayCashToolStripMenuItem.Click, AddressOf ExportNetPayDetailsToolStripMenuItem_Click
            RemoveHandler ExportNetPayDirectDepositToolStripMenuItem.Click, AddressOf ExportNetPayDetailsToolStripMenuItem_Click

        End If

    End Sub

    Private Sub ShowOrHideEmailPayslip()

        Dim emailPayslip = _policy.UseEmailPayslip

        ManagePayslipsToolStripMenuItem.Visible = emailPayslip
        PrintPaySlipToolStripMenuItem.Visible = Not emailPayslip

    End Sub

    Sub VIEW_payperiodofyear(Optional param_Date As Object = Nothing)
        ClosePayrollToolStripMenuItem.Visible = False
        ReopenPayrollToolStripMenuItem.Visible = False
        GeneratePayrollToolStripButton.Visible = False

        Dim hasValue = (MDIPrimaryForm.systemprogressbar.Value > 0)
        If hasValue Then
            For i = 0 To 5
                RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
            Next
        End If

        Dim date_param = If(param_Date = Nothing, "NULL", "'" & param_Date & "-01-01'")
        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL VIEW_payperiodofyear('" & orgztnID & "'," & date_param & ",'0');")
        Dim catchdt As New DataTable : catchdt = n_SQLQueryToDatatable.ResultTable

        dgvpayper.Rows.Clear()
        Dim index As Integer = 0
        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvpayper.Rows.Add(row_array)

            HighlightOpenPayPeriod(index, drow)

            index += 1
        Next
        catchdt.Dispose()
        If hasValue Then
            AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
        End If
    End Sub

    Private Sub HighlightOpenPayPeriod(index As Integer, drow As DataRow)

        Dim currentRow = dgvpayper.Rows(index)

        Dim status = Enums(Of PayPeriodStatus).Parse(drow("Status"))
        If status = PayPeriodStatus.Open Then

            currentRow.DefaultCellStyle.SelectionForeColor = Color.Green

            Dim defaultFont = currentRow.DefaultCellStyle.Font
            If defaultFont Is Nothing Then

                If dgvpayper.DefaultCellStyle.Font IsNot Nothing Then

                    defaultFont = dgvpayper.DefaultCellStyle.Font

                ElseIf defaultFont IsNot Nothing Then

                    defaultFont = defaultFont
                Else

                    defaultFont = New Font("Microsoft Sans Serif", 8.25)
                End If

                currentRow.DefaultCellStyle.Font = New Font(defaultFont, FontStyle.Bold)

            End If

        End If

    End Sub

    Sub loademployee(Optional q_empsearch As String = Nothing)
        If paypRowID Is Nothing Then
            Return
        End If

        Dim offset = _pageNo * ItemsPerPage
        Dim limit = ItemsPerPage

        Dim parameters = New Object() {
            orgztnID,
            tsSearch.Text,
            paypRowID,
            offset,
            limit
        }

        Dim n_ReadSQLProcedureToDatatable =
            New SQL("CALL SEARCH_employee_paystub(?1, ?2, ?3, ?4, ?5);",
                    parameters)

        Dim catchdt = n_ReadSQLProcedureToDatatable.GetFoundRows.Tables(0)

        dgvemployees.Rows.Clear()

        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvemployees.Rows.Add(row_array)
        Next

        _totalItems = Val(EXECQUER($"SELECT COUNT(RowID) FROM paystub WHERE OrganizationID={orgztnID} AND PayPeriodID = {paypRowID};"))
        _lastPageNo = {Math.Ceiling(_totalItems / ItemsPerPage) - 1, 0}.Max()

        Static x As SByte = 0

        If x = 0 Then
            x = 1

            With dgvemployees
                .Columns("RowID").Visible = False

            End With
            If dgvemployees.RowCount > 0 Then
                dgvemployees.Item("EmployeeID", 0).Selected = True
            End If

            employeepicture = New SQLQueryToDatatable("SELECT RowID,Image FROM employee WHERE Image IS NOT NULL AND OrganizationID=" & orgztnID & ";").ResultTable 'retAsDatTbl("SELECT RowID,Image FROM employee WHERE OrganizationID=" & orgztnID & ";")
        End If
    End Sub

    Private Sub dgvpayper_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvpayper.SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        Static str_pay_freq_sched As String = String.Empty

        Static _year As Integer = 0

        If dgvpayper.RowCount > 0 Then
            With dgvpayper.CurrentRow

                paypPayFreqID = .Cells("Column4").Value

                paypRowID = .Cells("Column1").Value
                paypFrom = MYSQLDateFormat(CDate(.Cells("Column2").Value)) 'Format(CDate(.Cells("Column2").Value), "yyyy-MM-dd")

                paypTo = MYSQLDateFormat(CDate(.Cells("Column3").Value)) 'Format(CDate(.Cells("Column3").Value), "yyyy-MM-dd")

                isEndOfMonth = Trim(.Cells("Column14").Value)

                Dim str_sched_payfreq As String = Convert.ToString(.Cells("Column12").Value)

                str_pay_freq_sched = str_sched_payfreq

                _year = CurrentYear

                Dim select_cutoff_payfrequency =
                    tstrip.Items.OfType(Of ToolStripButton).Where(Function(tsbtn) tsbtn.Text = str_sched_payfreq)

                For Each _tsbtn In select_cutoff_payfrequency
                    PayFreq_Changed(_tsbtn, New EventArgs)
                Next

                If .Index < 0 Then Return

                Dim status = Enums(Of PayPeriodStatus).Parse(.Cells("StatusColumn").Value)

                Dim isModified = status = PayPeriodStatus.Open

                ShowPayrollActions(isModified, status)

            End With
        Else

            paypRowID = Nothing
            paypFrom = Nothing
            paypTo = Nothing
            isEndOfMonth = 0
            paypPayFreqID = String.Empty

            ShowPayrollActions(False)

            dgvemployees_SelectionChanged(sender, e)

        End If
        AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

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
        dgvAdjustments.Enabled = enable
        btnSaveAdjustments.Visible = enable
        btndiscardchanges.Visible = enable
    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        CurrentYear = CurrentYear - 1

        linkPrev.Text = "← " & (CurrentYear - 1)
        linkNxt.Text = (CurrentYear + 1) & " →"

        VIEW_payperiodofyear(CurrentYear)

        AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        dgvpayper_SelectionChanged(sender, e)
    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        CurrentYear = CurrentYear + 1

        linkNxt.Text = (CurrentYear + 1) & " →"
        linkPrev.Text = "← " & (CurrentYear - 1)

        VIEW_payperiodofyear(CurrentYear)

        AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        dgvpayper_SelectionChanged(sender, e)
    End Sub

    Private Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked,
                                                                                                Nxt.LinkClicked, Last.LinkClicked,
                                                                                                LinkLabel4.LinkClicked, LinkLabel3.LinkClicked,
                                                                                                LinkLabel2.LinkClicked, LinkLabel1.LinkClicked
        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        Dim action As PaginationAction

        If sendrname = "First" Or sendrname = "LinkLabel1" Then
            action = PaginationAction.First
        ElseIf sendrname = "Prev" Or sendrname = "LinkLabel2" Then
            action = PaginationAction.Previous
        ElseIf sendrname = "Nxt" Or sendrname = "LinkLabel4" Then
            action = PaginationAction.Next
        ElseIf sendrname = "Last" Or sendrname = "LinkLabel3" Then
            action = PaginationAction.Last
        End If

        Select Case action
            Case PaginationAction.First
                _pageNo = 0
            Case PaginationAction.Previous
                _pageNo = If(1 < _pageNo, _pageNo - 1, 0)
            Case PaginationAction.Next
                _pageNo = If(_pageNo < _lastPageNo, _pageNo + 1, _lastPageNo)
            Case PaginationAction.Last
                _pageNo = _lastPageNo
        End Select

        Dim pay_freqString = String.Empty

        For Each ctrl In tstrip.Items
            If TypeOf ctrl Is ToolStripButton Then

                If DirectCast(ctrl, ToolStripButton).BackColor =
                        Color.FromArgb(194, 228, 255) Then

                    pay_freqString =
                            DirectCast(ctrl, ToolStripButton).Text

                    Exit For
                Else
                    Continue For
                End If
            Else
                Continue For
            End If

        Next

        quer_empPayFreq = " And pf.PayFrequencyType='" & pay_freqString & "' "

        loademployee(quer_empPayFreq)

        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        dgvemployees_SelectionChanged(sender, e)

        AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    Private Sub dgvemployees_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvemployees.SelectionChanged

        btnSaveAdjustments.Enabled = False

        Dim employeetype = ""

        Static sameEmpID As String = 0

        dgvemployees.Tag = Nothing

        ResetNumberTextBox()

        If dgvemployees.RowCount > 0 Then 'And dgvemployees.CurrentRow IsNot Nothing
            With dgvemployees.CurrentRow

                employeetype = Trim(.Cells("EmployeeType").Value)
                sameEmpID = .Cells("RowID").Value

                dgvemployees.Tag = .Cells("RowID").Value

                txtFName.Text = .Cells("FirstName").Value

                Dim addtlWord = Nothing

                If .Cells("MiddleName").Value = Nothing Then
                Else

                    Dim midNameTwoWords = Split(.Cells("MiddleName").Value.ToString, " ")

                    addtlWord = " "

                    For Each s In midNameTwoWords

                        addtlWord &= (StrConv(Microsoft.VisualBasic.Left(s, 1), VbStrConv.ProperCase) & ".")

                    Next

                    txtFName.Text &= addtlWord

                End If

                txtFName.Text = txtFName.Text & " " & .Cells("LastName").Value

                currentEmployeeNumber = .Cells("EmployeeID").Value

                txtEmpID.Text = "ID# " & .Cells("EmployeeID").Value &
                            If(IsDBNull(.Cells("Position")),
                                                               "",
                                                               ", " & .Cells("Position").Value) &
                            If(.Cells("EmployeeType").Value = Nothing,
                                                               "",
                                                               ", " & .Cells("EmployeeType").Value & " salary")

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

                If ValNoComma(paypRowID) > 0 Then
                    '# ####################################################### #
                    Try
                        For Each txtbxctrl In SplitContainer1.Panel2.Controls.OfType(Of TextBox).ToList()
                            txtbxctrl.Text = "0.00"
                        Next
                    Catch ex As Exception
                        MsgBox(getErrExcptn(ex, Me.Name))
                    End Try
                    If tabEarned.SelectedIndex = 0 Then
                        TabPage1_Enter1(TabPage1, New EventArgs)
                    ElseIf tabEarned.SelectedIndex = 1 Then
                        TabPage4_Enter1(TabPage4, New EventArgs)
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
                        TabPage1_Enter1(TabPage1, New EventArgs)
                    ElseIf tabEarned.SelectedIndex = 1 Then
                        TabPage4_Enter1(TabPage4, New EventArgs)
                    End If
                End If

            End With
        Else
            sameEmpID = -1

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

            txtTotalAllowance.Text = ""
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

    End Sub

    Sub GeneratePayroll()

        Dim payPeriodId = ObjectUtils.ToNullableInteger(paypRowID)
        If payPeriodId Is Nothing Then
            GeneratePayrollToolStripButton.Enabled = True
            Return
        End If

        Dim payPeriod = _payPeriodRepository.GetById(payPeriodId)
        If payPeriod Is Nothing OrElse payPeriod?.RowID Is Nothing OrElse payPeriod?.OrganizationID Is Nothing Then
            GeneratePayrollToolStripButton.Enabled = True
            Return
        End If

        If payPeriod.Status <> PayPeriodStatus.Open Then
            MessageBoxHelper.Warning("Only ""Open"" pay periods can be computed.")
            GeneratePayrollToolStripButton.Enabled = True
            Return
        End If

        Dim resources = MainServiceProvider.GetRequiredService(Of PayrollResources)

        Dim loadTask = Task.Factory.StartNew(
            Function()

                Dim resourcesTask = resources.Load(payPeriodId:=payPeriodId,
                                                   organizationId:=z_OrganizationID,
                                                   userId:=z_User)

                resourcesTask.Wait()

                Return resources
            End Function,
            0
        )

        loadTask.ContinueWith(
            AddressOf LoadingPayrollDataOnSuccess,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        loadTask.ContinueWith(
            AddressOf LoadingPayrollDataOnError,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Sub LoadingPayrollDataOnSuccess(t As Task(Of PayrollResources))
        GeneratePayrollToolStripButton.Enabled = True
        ThreadingPayrollGeneration(t.Result)
    End Sub

    Private Sub LoadingPayrollDataOnError(t As Task)
        GeneratePayrollToolStripButton.Enabled = True
        _logger.Error("Error loading one of the payroll data.", t.Exception)
        MsgBox("Something went wrong while loading the payroll data needed for computation. Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Resources")
        Me.Enabled = True
    End Sub

    Private Sub ThreadingPayrollGeneration(resources As PayrollResources)
        Me.Enabled = False

        Try
            ProgressTimer.Start()

            _finishedPaystubs = 0
            _totalPaystubs = resources.Employees.Count

            _results = New BlockingCollection(Of PayrollGeneration.Result)()

            Dim generator = MainServiceProvider.GetRequiredService(Of PayrollGeneration)

            Dim generationTask = Task.Run(
                Sub()
                    'TEMPORARY set to synchronous since there is a race condition issue
                    'that is hard to debug
                    'Parallel.ForEach(
                    '        resources.Employees,
                    '        Sub(employee)

                    '            _results.Add(generator.DoProcess(organizationId:=z_OrganizationID,
                    '                                            userId:=z_User,
                    '                                            employee:=employee,
                    '                                            resources:=resources))

                    '            Interlocked.Increment(_finishedPaystubs)
                    '        End Sub)
                    resources.Employees.ToList().ForEach(
                            Sub(employee)

                                _results.Add(generator.DoProcess(organizationId:=z_OrganizationID,
                                                                userId:=z_User,
                                                                employee:=employee,
                                                                resources:=resources))

                                Interlocked.Increment(_finishedPaystubs)
                            End Sub)
                End Sub
            )

            generationTask.ContinueWith(
                    AddressOf GeneratingPayrollOnSuccess,
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext
            )

            generationTask.ContinueWith(
                AddressOf GeneratingPayrollOnError,
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext
            )
        Catch ex As Exception
            _logger.Error("Error loading the employees", ex)
        End Try
    End Sub

    Private Sub ProgressTimer_Tick(sender As Object, e As EventArgs) Handles ProgressTimer.Tick

        Dim percentComplete As Integer = (_finishedPaystubs / _totalPaystubs) * 100
        MDIPrimaryForm.systemprogressbar.Value = percentComplete

    End Sub

    Private Async Sub GeneratingPayrollOnSuccess()

        Dim dialog = New PayrollResultDialog(_results.ToList()) With {
                .Owner = Me
            }
        dialog.ShowDialog()

        ProgressTimer.Stop()

        RefreshForm()

        Await TimeEntrySummaryForm.LoadPayPeriods()

        Me.Enabled = True
        dgvpayper_SelectionChanged(dgvpayper, New EventArgs)
    End Sub

    Private Sub GeneratingPayrollOnError(t As Task)
        _logger.Error("Error on generating payroll.", t.Exception)
        MsgBox("Something went wrong while generating the payroll . Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Generation")
        Me.Enabled = True
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Me.Close()
    End Sub

    Public genpayselyear As String = Nothing

    Sub btnrefresh_Click(sender As Object, e As EventArgs) Handles btnrefresh.Click
        VIEW_payperiodofyear()
    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        TabControlColor(TabControl1, e)
    End Sub

    Private Sub btntotallow_Click(sender As Object, e As EventArgs) Handles btntotallow.Click, btnTotalTaxabAllowance.Click
        viewtotloan.Close()
        viewtotbon.Close()

        With viewtotallow
            .Show()
            .BringToFront()
            If dgvemployees.RowCount > 0 Then

                .VIEW_allowanceperday(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With
    End Sub

    Private Sub btntotbon_Click(sender As Object, e As EventArgs) Handles btntotbon.Click
        viewtotallow.Close()
        viewtotloan.Close()

        With viewtotbon
            .Show()
            .BringToFront()
            If dgvemployees.RowCount <> 0 Then
                .VIEW_employeebonus_indate(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With

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
        viewtotallow.Close()
        viewtotloan.Close()
        viewtotbon.Close()

        PayrollForm.listPayrollForm.Remove(Me.Name)
    End Sub

    Private Sub tsSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tsSearch.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        IsUserPressEnterToSearch = (e_asc = 13)
        If IsUserPressEnterToSearch Then
            Dim search_string_len = tsSearch.Text.Trim.Length
            IsUserPressEnterToSearch = (search_string_len > 0)
            tsbtnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub tsbtnSearch_Click(sender As Object, e As EventArgs) Handles tsbtnSearch.Click
        IsUserPressEnterToSearch = False

        If tsSearch.Text.Trim.Length = 0 Then
            First_LinkClicked(First, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))
        Else
            Dim dattabsearch As New DataTable

            'pagination = 0

            _pageNo = 0

            Dim offset = _pageNo * ItemsPerPage
            Dim limit = ItemsPerPage

            Dim param_array = New Object() {
                orgztnID,
                tsSearch.Text,
                paypRowID,
                offset,
                limit
            }

            Dim n_ReadSQLProcedureToDatatable = New SQL(
                "CALL SEARCH_employee_paystub(?1, ?2, ?3, ?4, ?5);",
                param_array)

            dattabsearch = n_ReadSQLProcedureToDatatable.GetFoundRows.Tables(0)

            dgvemployees.Rows.Clear()

            _totalItems = Val(EXECQUER($"SELECT COUNT(RowID) FROM paystub WHERE OrganizationID={orgztnID} AND PayPeriodID = {paypRowID};"))
            _lastPageNo = {Math.Ceiling(_totalItems / ItemsPerPage) - 1, 0}.Max()

            For Each drow As DataRow In dattabsearch.Rows
                Dim row_array = drow.ItemArray
                dgvemployees.Rows.Add(row_array)
            Next

            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            dgvemployees_SelectionChanged(sender, e)

            AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        End If
    End Sub

    Private Sub SplitContainer1_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        SplitContainer1.Panel2.Focus()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Image.Tag = 1 Then
            Button3.Image = Nothing
            Button3.Image = My.Resources.r_arrow
            Button3.Image.Tag = 0

            TabControl1.Show()
            dgvpayper.Width = 350

            dgvpayper_SelectionChanged(sender, e)
        Else
            Button3.Image = Nothing
            Button3.Image = My.Resources.l_arrow
            Button3.Image.Tag = 1

            TabControl1.Hide()
            Dim pointX As Integer = Width_resolution - (Width_resolution * 0.15)

            dgvpayper.Width = pointX
        End If
    End Sub

    Private Sub PayrollSummaryDeclaredAndActualToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles _
        PrintPayrollSummaryToolStripButton.Click,
        PayrollSummaryDeclaredToolStripMenuItem.Click,
        PayrollSummaryActualToolStripMenuItem.Click

        Dim isActual = sender Is PayrollSummaryActualToolStripMenuItem
        ShowPayrollSummary(isActual)

    End Sub

    Private Sub tbppayroll_Enter(sender As Object, e As EventArgs) Handles tbppayroll.Enter

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Dim payfrqncy As New AutoCompleteStringCollection

            Dim sel_query = ""

            Dim hasAnEmployee = EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE OrganizationID=" & orgztnID & " LIMIT 1);")

            If hasAnEmployee = 1 Then
                sel_query = "SELECT pp.PayFrequencyType FROM payfrequency pp INNER JOIN employee e ON e.PayFrequencyID=pp.RowID GROUP BY pp.RowID;"
            Else
                sel_query = "SELECT PayFrequencyType FROM payfrequency WHERE PayFrequencyType IN ('SEMI-MONTHLY','WEEKLY');"
            End If

            enlistTheLists(sel_query, payfrqncy)

            Dim first_sender As New ToolStripButton

            Dim indx = 0

            For Each strval In payfrqncy

                If strval Is Nothing OrElse strval.ToString().Trim().ToUpper() <> "SEMI-MONTHLY" Then
                    Continue For
                End If

                Dim new_tsbtn As New ToolStripButton

                With new_tsbtn

                    .AutoSize = False
                    .BackColor = Color.FromArgb(255, 255, 255)
                    .ImageTransparentColor = Color.Magenta
                    .Margin = New Padding(0, 1, 0, 1)
                    .Name = String.Concat("tsbtn" & strval)
                    .Overflow = ToolStripItemOverflow.Never
                    .Size = New Size(110, 30)
                    .Text = strval
                    .TextAlign = ContentAlignment.MiddleLeft
                    .TextImageRelation = TextImageRelation.ImageBeforeText
                    .ToolTipText = strval

                End With

                tstrip.Items.Add(new_tsbtn)

                If indx = 0 Then
                    indx = 1
                    first_sender = new_tsbtn
                End If

                AddHandler new_tsbtn.Click, AddressOf PayFreq_Changed

            Next

            tstrip.PerformLayout()

            If first_sender IsNot Nothing Then
                PayFreq_Changed(first_sender, New EventArgs)
            End If

            For Each tsItem As ToolStripItem In tstrip.Items
                tsItem.PerformClick()
                Exit For
            Next

            dgvpayper_SelectionChanged(dgvpayper, New EventArgs)

            AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        End If

    End Sub

    Sub PayFreq_Changed(sender As Object, e As EventArgs)

        quer_empPayFreq = ""

        Dim senderObj As New ToolStripButton

        Static prevObj As New ToolStripButton

        Static once As SByte = 0

        senderObj = DirectCast(sender, ToolStripButton)

        If once = 0 Then
            once += 1

            prevObj = senderObj

            senderObj.BackColor = Color.FromArgb(194, 228, 255)

            senderObj.Font = selectedButtonFont

            quer_empPayFreq = " AND pf.PayFrequencyType='" & senderObj.Text & "' "

            loademployee(quer_empPayFreq)

            Exit Sub
        End If

        If prevObj.Name = Nothing Then
        Else

            If prevObj.Name <> senderObj.Name Then

                prevObj.BackColor = Color.FromArgb(255, 255, 255)

                prevObj.Font = unselectedButtonFont

                prevObj = senderObj

            End If

        End If

        senderObj.BackColor = Color.FromArgb(194, 228, 255)

        senderObj.Font = selectedButtonFont

        senderObj.Tag = 1

        Dim select_cutoff_payfrequency =
            tstrip.Items.OfType(Of ToolStripButton).Where(Function(tsbtn) tsbtn.Name <> senderObj.Name)

        For Each _tsbtn In select_cutoff_payfrequency
            _tsbtn.Tag = 0
        Next

        Dim prev_selRowIndex = -1

        If dgvemployees.RowCount <> 0 Then
            Try
                prev_selRowIndex = dgvemployees.CurrentRow.Index
            Catch ex As Exception
                prev_selRowIndex = -1
            End Try
        End If

        quer_empPayFreq = String.Empty

        tsbtnSearch_Click(sender, e)

    End Sub

    Private Async Sub dgvemployees_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployees.CellContentClick

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then Return

        If e.ColumnIndex = DeletePaystubColumn.Index Then

            Await DeletePaystub()

        End If

    End Sub

    Private Sub dgAdjustments_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAdjustments.CellContentClick

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then Return

        If TypeOf dgvAdjustments.Columns(e.ColumnIndex) Is DataGridViewLinkColumn Then
            Dim n_ExecuteQuery As New ExecuteQuery("SELECT PartNo FROM product WHERE RowID='" & dgvAdjustments.Item("cboProducts", e.RowIndex).Value & "' LIMIT 1;")
            Dim item_name As String = n_ExecuteQuery.Result
            Dim prompt = MessageBox.Show("Are you sure you want to delete '" & item_name & "'" & If(item_name.ToLower.Contains("adjustment"), "", " adjustment") & " ?",
                                         "Delete adjustment", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If prompt = Windows.Forms.DialogResult.Yes Then

                If Not IsDBNull(dgvAdjustments.Item("IsAdjustmentActual", e.RowIndex).Value) Then

                    Dim SQLTableName As String = If(dgvAdjustments.Item("IsAdjustmentActual", e.RowIndex).Value = 0, "paystubadjustment", "paystubadjustmentactual")
                    Dim del_quer As String =
                        "DELETE FROM " & SQLTableName & " WHERE RowID='" & dgvAdjustments.Item("psaRowID", e.RowIndex).Value & "';"
                    n_ExecuteQuery = New ExecuteQuery(del_quer)

                End If

                dgvAdjustments.Rows.Remove(dgvAdjustments.Rows(e.RowIndex))
                dgvemployees_SelectionChanged(dgvemployees, New EventArgs)
                btnSaveAdjustments.Enabled = False
            End If
        End If
    End Sub

    Sub btnSaveAdjustments_Click(sender As Object, e As EventArgs) Handles btnSaveAdjustments.Click
        dgvAdjustments.EndEdit(True)

        Dim hasError As Boolean = False
        Dim errorRow As New DataGridViewRow
        Dim lastRow As Integer = dgvAdjustments.Rows.Count

        Dim rowCount As Integer = 0

        Dim comment_columnname = "DataGridViewTextBoxColumn64"
        If Not hasError Then
            Try
                Dim SQLFunctionName As String = If(ValNoComma(tabEarned.SelectedIndex) = 0, "I_paystubadjustment", "I_paystubadjustmentactual")
                For Each dgvRow As DataGridViewRow In dgvAdjustments.Rows
                    Dim productRowID = dgvRow.Cells("cboProducts").Value
                    If productRowID IsNot Nothing And dgvRow.IsNewRow = False Then 'If Not dgvRow.Cells(0).Value Is Nothing AndAlso Not dgvRow.Cells(1).Value Is Nothing AndAlso IsNumeric(dgvRow.Cells(1).Value) Then
                        If TypeOf dgvRow.Cells("cboProducts").Value Is String Then
                            productRowID =
                            EXECQUER("SELECT RowID FROM product WHERE OrganizationID='" & orgztnID & "' AND PartNo='" & dgvRow.Cells("cboProducts").Value & "' LIMIT 1;")
                        End If
                        Dim n_ReadSQLFunction As _
                            New ReadSQLFunction(SQLFunctionName,
                                                "returnvalue",
                                                orgztnID,
                                                z_User,
                                                productRowID,
                                                ValNoComma(dgvRow.Cells("DataGridViewTextBoxColumn66").Value),
                                                dgvRow.Cells("DataGridViewTextBoxColumn64").Value,
                                                Me.currentEmployeeNumber,
                                                paypRowID,
                                                dgvRow.Cells("psaRowID").Value)
                        If ValNoComma(dgvRow.Cells("psaRowID").Value) = 0 And n_ReadSQLFunction.HasError = False Then
                            dgvRow.Cells("psaRowID").Value = n_ReadSQLFunction.ReturnValue
                        End If
                    Else : Continue For
                    End If
                Next
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name))
            Finally
                UpdatePaystubAdjustmentColumn()
                MsgBox("Adjustments were saved!", MsgBoxStyle.Information)
                dgvemployees_SelectionChanged(sender, e)
            End Try
        End If
    End Sub

    Private Sub btntotloan_Click(sender As Object, e As EventArgs) Handles btntotloan.Click
        viewtotallow.Close()
        viewtotbon.Close()

        With viewtotloan
            .Show()
            .BringToFront()

            If dgvemployees.RowCount <> 0 Then
                .VIEW_employeeloan_indate(dgvemployees.CurrentRow.Cells("RowID").Value, paypFrom, paypTo)
                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With
    End Sub

    Private Sub UpdatePaystubAdjustmentColumn()
        Dim _conn As New MySqlConnection
        _conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString
        Try
            If _conn.State = ConnectionState.Open Then : _conn.Close() : End If
            _conn.Open()

            cmd = New MySqlCommand("SP_UpdatePaystubAdjustment", _conn)

            With cmd
                .Parameters.Clear()
                .CommandType = CommandType.StoredProcedure

                .Parameters.AddWithValue("pa_EmployeeID", Me.currentEmployeeNumber)
                .Parameters.AddWithValue("pa_PayPeriodID", dgvpayper.SelectedRows(0).Cells(0).Value)
                .Parameters.AddWithValue("User_RowID", z_User)
                .Parameters.AddWithValue("Og_RowID", z_OrganizationID)
            End With

            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            _conn.Close()
            cmd.Dispose()
        End Try
    End Sub

    Private Sub UpdateAdjustmentDetails(Optional IsActual As Boolean = False)
        Try
            dtJosh = New DataTable
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()

            cmd = New MySqlCommand("VIEW_paystubadjustment", conn)
            With cmd
                .Parameters.Clear()
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("pa_EmployeeID", Me.currentEmployeeNumber)
                If dgvpayper.RowCount > 0 And dgvpayper.SelectedRows.Count > 0 Then
                    .Parameters.AddWithValue("pa_PayPeriodID", dgvpayper.SelectedRows(0).Cells(0).Value)
                Else
                    .Parameters.AddWithValue("pa_PayPeriodID", DBNull.Value)
                End If
                .Parameters.AddWithValue("pa_OrganizationID", orgztnID)
                .Parameters.AddWithValue("pa_IsActual", Convert.ToInt16(IsActual))
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(dtJosh)
            dgvAdjustments.DataSource = dtJosh
        Catch ex As Exception
            dgvAdjustments.Rows.Clear()
        Finally
            conn.Close()
            cmd.Dispose()
            da.Dispose()
        End Try
    End Sub

    Private Sub dgvAdjustments_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvAdjustments.DataError
        btnSaveAdjustments.Enabled = False
    End Sub

    Private Sub dgvAdjustments_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAdjustments.CellEndEdit
        UpdateSaveAdjustmentButtonDisable()
    End Sub

    Private Sub UpdateSaveAdjustmentButtonDisable()
        Dim hasError As Boolean = False

        btnSaveAdjustments.Enabled = Not hasError
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

    Private Sub TabPage1_Enter1(sender As Object, e As EventArgs) Handles TabPage1.Enter 'DECLARED

        TabPage1.Text = TabPage1.Text.Trim
        TabPage1.Text = TabPage1.Text & Space(15)
        TabPage4.Text = TabPage4.Text.Trim

        For Each txtbxctrl In TabPage1.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag

        If ObjectUtils.ToNullableInteger(EmployeeRowID) Is Nothing OrElse
            ObjectUtils.ToNullableInteger(paypRowID) Is Nothing Then
            Return
        End If

        Dim employee = _employeeRepository.GetById(EmployeeRowID)
        If employee Is Nothing Then Return

        Dim paystub = GetPaystub(employee.RowID)
        If paystub Is Nothing Then Return

        Dim datePeriod = GetPayPeriodDates()
        If datePeriod Is Nothing Then Return

        Dim salary = _employeeRepository.GetCurrentSalary(employee.RowID, datePeriod.Start)
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
        txtLeaveHours.Text = FormatNumber(paystub.LeaveHours, 2)

        txtRegularHolidayPay.Text = FormatNumber(paystub.RegularHolidayPay, 2)
        txtRegularHolidayOTPay.Text = FormatNumber(paystub.RegularHolidayOTPay, 2)
        txtSpecialHolidayPay.Text = FormatNumber(paystub.SpecialHolidayPay, 2)
        txtSpecialHolidayOTPay.Text = FormatNumber(paystub.SpecialHolidayOTPay, 2)
        txtRestDayPay.Text = FormatNumber(paystub.RestDayPay, 2)
        txtLeavePay.Text = FormatNumber(paystub.LeavePay, 2)

        'Allowance, Bonus and Gross Pay
        txtTotalAllowance.Text = FormatNumber(paystub.TotalAllowance, 2)
        txtTotalTaxableAllowance.Text = FormatNumber(paystub.TotalTaxableAllowance, 2)

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

        Dim totalAgencyFee = _agencyFeeRepository.GetPaystubAmount(organizationId:=z_OrganizationID,
                                                                   timePeriod:=datePeriod,
                                                                   employeeId:=employee.RowID)
        txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

        '13th month and Net Pay
        Dim thirteenthMonthPay = If(paystub.ThirteenthMonthPay?.Amount, 0)
        txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

        Dim totalNetSalary = paystub.NetPay + totalAgencyFee
        txtNetPay.Text = FormatNumber(totalNetSalary, 2)
        txtTotalNetPay.Text = FormatNumber(totalNetSalary + thirteenthMonthPay, 2)

        UpdateAdjustmentDetails(Convert.ToInt16(TabPage1.Tag))
    End Sub

    Private Sub TabPage4_Enter1(sender As Object, e As EventArgs) Handles TabPage4.Enter 'UNDECLARED / ACTUAL
        TabPage4.Text = TabPage4.Text.Trim
        TabPage4.Text = TabPage4.Text & Space(15)
        TabPage1.Text = TabPage1.Text.Trim

        For Each txtbxctrl In TabPage4.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag

        If ObjectUtils.ToNullableInteger(EmployeeRowID) Is Nothing OrElse
            ObjectUtils.ToNullableInteger(paypRowID) Is Nothing Then
            Return
        End If

        Dim employee = _employeeRepository.GetById(EmployeeRowID)
        If employee Is Nothing Then Return

        Dim paystub = GetPaystub(employee.RowID)
        If paystub Is Nothing Then Return

        Dim datePeriod = GetPayPeriodDates()
        If datePeriod Is Nothing Then Return

        Dim salary = _employeeRepository.GetCurrentSalary(employee.RowID, datePeriod.Start)
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
        txtLeaveHours.Text = FormatNumber(paystub.LeaveHours, 2)

        txtRegularHolidayPay.Text = FormatNumber(paystub.Actual?.RegularHolidayPay, 2)
        txtRegularHolidayOTPay.Text = FormatNumber(paystub.Actual?.RegularHolidayOTPay, 2)
        txtSpecialHolidayPay.Text = FormatNumber(paystub.Actual?.SpecialHolidayPay, 2)
        txtSpecialHolidayOTPay.Text = FormatNumber(paystub.Actual?.SpecialHolidayOTPay, 2)
        txtRestDayPay.Text = FormatNumber(paystub.Actual?.RestDayPay, 2)
        txtLeavePay.Text = FormatNumber(paystub.Actual?.LeavePay, 2)

        'Allowance, Bonus and Gross Pay
        txtTotalAllowance.Text = FormatNumber(paystub.TotalAllowance, 2)
        txtTotalTaxableAllowance.Text = FormatNumber(paystub.TotalTaxableAllowance, 2)

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

        Dim totalAgencyFee = _agencyFeeRepository.GetPaystubAmount(organizationId:=z_OrganizationID,
                                                                   timePeriod:=datePeriod,
                                                                   employeeId:=employee.RowID)
        txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

        '13th month and Net Pay
        Dim thirteenthMonthPay = If(paystub.ThirteenthMonthPay?.Amount, 0)
        txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

        Dim totalNetSalary = paystub.Actual?.NetPay + totalAgencyFee
        txtNetPay.Text = FormatNumber(totalNetSalary, 2)
        txtTotalNetPay.Text = FormatNumber(totalNetSalary + thirteenthMonthPay, 2)

        Dim totalNetPay = totalNetSalary + thirteenthMonthPay
        txtTotalNetPay.Text = FormatNumber(totalNetPay, 2)

    End Sub

    Private Function GetPaystub(EmployeeRowID As Object) As Paystub
        If ObjectUtils.ToNullableInteger(EmployeeRowID) Is Nothing OrElse
            ObjectUtils.ToNullableInteger(paypRowID) Is Nothing Then
            Return Nothing
        End If

        Return _paystubRepository.GetByCompositeKeyWithActualAndThirteenthMonth(
                    New EmployeeCompositeKey(employeeId:=EmployeeRowID, payPeriodId:=paypRowID))
    End Function

    Private Function GetPayPeriodDates() As TimePeriod
        Dim dateFrom = ObjectUtils.ToNullableDateTime(paypFrom)
        Dim dateTo = ObjectUtils.ToNullableDateTime(paypTo)

        If dateFrom Is Nothing OrElse dateTo Is Nothing Then
            Return Nothing
        End If

        Return New TimePeriod(dateFrom, dateTo)
    End Function

    Private Sub btndiscardchanges_Click(sender As Object, e As EventArgs) Handles btndiscardchanges.Click
        UpdateAdjustmentDetails(Convert.ToInt16(tabEarned.SelectedIndex)) 'Josh

        btnSaveAdjustments.Enabled = False
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked

        Dim n_ProdCtrlForm As New ProdCtrlForm

        With n_ProdCtrlForm

            .Status.HeaderText = "Taxable Flag"

            .PartNo.HeaderText = "Item Name"

            .NameOfCategory = "Adjustment Type"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                cboProducts.DataSource = Nothing

                cboProducts.ValueMember = "ProductID"
                cboProducts.DisplayMember = "ProductName"

                cboProducts.DataSource = New SQLQueryToDatatable("SELECT RowID AS 'ProductID', Name AS 'ProductName', Category FROM product WHERE Category IN ('Allowance Type', 'Bonus', 'Adjustment Type')" &
                                                                 " AND OrganizationID='" & orgztnID & "' ORDER BY Name;").ResultTable

            End If

        End With

    End Sub

    Private Async Function DeletePaystub() As Task
        Dim employeeId = ObjectUtils.ToNullableInteger(dgvemployees.Tag)
        Dim payPeriodId = ObjectUtils.ToNullableInteger(paypRowID)
        If employeeId Is Nothing OrElse payPeriodId Is Nothing OrElse currentEmployeeNumber = Nothing Then

            MessageBoxHelper.Warning("No selected paystub.", "Delete Paystub")
            Return

        End If

        Dim toBeDeletedEmployeeNumber = currentEmployeeNumber
        Dim toBeDeletedPaypFrom = paypFrom
        Dim toBeDeletedPaypTo = paypTo

        Dim prompt = MessageBox.Show("Are you sure you want to delete the '" & CDate(toBeDeletedPaypFrom).ToShortDateString &
                                     "' to '" & CDate(toBeDeletedPaypTo).ToShortDateString &
                                     "' payroll of employee '" & toBeDeletedEmployeeNumber & "' ?",
                                     "Delete employee payroll",
                                     MessageBoxButtons.YesNoCancel,
                                     MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If prompt = Windows.Forms.DialogResult.Yes Then

            Me.Cursor = Cursors.WaitCursor

            Await FunctionUtils.TryCatchFunctionAsync("Delete Paystub",
                Async Function()

                    Dim paystubDataService = MainServiceProvider.GetRequiredService(Of PaystubDataService)

                    Await paystubDataService.DeleteAsync(
                        New EmployeeCompositeKey(
                            employeeId:=employeeId.Value,
                            payPeriodId:=payPeriodId.Value),
                        userId:=z_User,
                        organizationId:=z_OrganizationID)

                    RefreshForm()

                    Await TimeEntrySummaryForm.LoadPayPeriods()

                    MessageBoxHelper.Information($"Paystub of employee {toBeDeletedEmployeeNumber} for payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}' was successfully deleted.")
                End Function)

            Me.Cursor = Cursors.Default
        End If

    End Function

    Private Sub dgvemployees_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvemployees.RowsRemoved
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    Private Sub setProperInterfaceBaseOnCurrentSystemOwner()
        If _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Cinema2000 Then
            Dim str_empty As String = String.Empty
            TabPage1.Text = str_empty
            TabPage4.Text = str_empty
            AddHandler tabEarned.Selecting, AddressOf tabEarned_Selecting

        End If

        If _systemOwnerService.GetCurrentSystemOwner() <> SystemOwnerService.Goldwings Then
            Label13.Visible = False
            Label43.Visible = False
            txtTotalBonus.Visible = False
            btntotbon.Visible = False
            lblGrossIncomeDivider.Visible = False

            Dim newPositionY = 496
            lblGrossIncome.Location = New Point(lblGrossIncome.Location.X, newPositionY)
            lblGrossIncomePesoSign.Location = New Point(lblGrossIncomePesoSign.Location.X, newPositionY)
            txtGrossPay.Location = New Point(txtGrossPay.Location.X, newPositionY)
        End If
    End Sub

    Private Sub tabEarned_Selecting(sender As Object, e As TabControlCancelEventArgs)

        Static isCinemaUser As Boolean =
            (_systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Cinema2000)

        e.Cancel = isCinemaUser

    End Sub

    Private Function GetCurrentPayFrequencyType() As String

        Dim select_cutoff_payfrequency =
                tstrip.Items.OfType(Of ToolStripButton).Where(Function(tsbtn) tsbtn.Tag = 1)

        Dim str_payfrequency_sched As String = "SEMI-MONTHLY"

        For Each _tsbtn In select_cutoff_payfrequency
            str_payfrequency_sched = _tsbtn.Text
        Next

        Return str_payfrequency_sched

    End Function

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

        Dim payPeriodId = ObjectUtils.ToNullableInteger(paypRowID)

        If payPeriodId Is Nothing Then

            MessageBoxHelper.Warning("Please select a pay period first.")
            Return False
        End If

        Dim payPeriod = Await _payPeriodRepository.GetByIdAsync(payPeriodId)

        If payPeriod Is Nothing Then

            MessageBoxHelper.Warning("Pay period does not exists. Please refresh the form.")
            Return False
        End If

        Dim dataService = MainServiceProvider.GetRequiredService(Of PayPeriodDataService)
        If close Then

            Await dataService.CloseAsync(payPeriod.RowID.Value, z_User)
        Else
            Await dataService.ReopenAsync(payPeriod.RowID.Value, z_User)

        End If

        RefreshForm()

        Await TimeEntrySummaryForm.LoadPayPeriods()

        If close Then

            MessageBoxHelper.Information("Pay period was closed successfully.")
        Else
            MessageBoxHelper.Information("Pay period was reopened successfully.")

        End If

        Return True

    End Function

    Private Sub RefreshForm()
        VIEW_payperiodofyear()
        dgvpayper_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub GeneratePayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GeneratePayrollToolStripButton.Click

        Dim payperiodId As Integer? = ObjectUtils.ToNullableInteger(paypRowID)

        If payperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected pay period.", "Delete Payroll")
            Return

        End If

        Dim confirm = MessageBoxHelper.Confirm(Of Boolean)(
            $"Generate payroll for '{CDate(paypFrom).ToShortDateString}' to '{CDate(paypTo).ToShortDateString}'?",
            "Generate Payroll")

        If Not confirm Then Return

        GeneratePayrollToolStripButton.Enabled = False

        GeneratePayroll()

    End Sub

    Private Sub Include13thMonthPayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Include13thMonthPayToolStripMenuItem.Click
        Dim payPeriodSelector = New PayrollSummaDateSelection()

        If payPeriodSelector.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim dateFrom = payPeriodSelector.DateFrom
        Dim dateTo = payPeriodSelector.DateTo

        ' Not tested
        'Dim realse = New ReleaseThirteenthMonthPay(dateFrom, dateTo, paypRowID)
    End Sub

    Private Sub CashOutUnusedLeavesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CashOutUnusedLeavesToolStripMenuItem.Click
        If paypRowID = 0 Then
            MsgBox("Please select a generated payroll.", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim payPeriodSelector = New PayrollSummaDateSelection()

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

        Dim toBeDeletedPaypFrom = paypFrom
        Dim toBeDeletedPaypTo = paypTo

        Dim payperiodId As Integer? = ObjectUtils.ToNullableInteger(paypRowID)

        If payperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected pay period.", "Delete Payroll")
            Return

        End If

        Dim prompt = MessageBox.Show($"Are you sure you want to delete ALL paystubs of employees for payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}'?",
                                     "Delete all employee payroll",
                                     MessageBoxButtons.YesNoCancel,
                                     MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If prompt = Windows.Forms.DialogResult.Yes Then
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            Await FunctionUtils.TryCatchFunctionAsync("Delete Payroll",
                  Async Function()

                      Dim paystubDataService = MainServiceProvider.GetRequiredService(Of PaystubDataService)
                      Await paystubDataService.DeleteByPeriodAsync(
                        payPeriodId:=payperiodId.Value,
                        userId:=z_User,
                        organizationId:=z_OrganizationID)

                      RefreshForm()

                      Await TimeEntrySummaryForm.LoadPayPeriods()

                      MessageBoxHelper.Information($"All paystubs for payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}' were successfully deleted.")
                  End Function)

        End If

        DeletePaystubsToolStripMenuItem.Enabled = True
    End Sub

    Private Async Sub CancelPayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CancelPayrollToolStripMenuItem.Click
        CancelPayrollToolStripMenuItem.Enabled = False

        Dim toBeDeletedPaypFrom = paypFrom
        Dim toBeDeletedPaypTo = paypTo

        Dim payperiodId As Integer? = ObjectUtils.ToNullableInteger(paypRowID)

        If payperiodId Is Nothing Then

            MessageBoxHelper.Warning("No selected pay period.", "Delete Payroll")
            Return

        End If

        Dim prompt = MessageBox.Show($"Are you sure you want to cancel the payroll and delete ALL paystubs and time entries of employees for payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}'?",
                                     "Cancel payroll",
                                     MessageBoxButtons.YesNoCancel,
                                     MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If prompt = Windows.Forms.DialogResult.Yes Then
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            Await FunctionUtils.TryCatchFunctionAsync("Cancel Payroll",
                  Async Function()

                      Dim payperiodDataService = MainServiceProvider.GetRequiredService(Of PayPeriodDataService)
                      Await payperiodDataService.CancelAsync(payperiodId.Value, z_User)

                      RefreshForm()

                      Await TimeEntrySummaryForm.LoadPayPeriods()

                      MessageBoxHelper.Information($"Payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}' was successfully cancelled.")
                  End Function)

        End If

        CancelPayrollToolStripMenuItem.Enabled = True
    End Sub

    Private Sub PrintPaySlipToolStripMenuItem_Click(sender As Object, e As EventArgs)

        If _policy.ShowActual = False Then

            PrintPayslip(isActual:=0)

        End If
    End Sub

    Private Sub PrintAllPaySlip_Click(sender As Object, e As EventArgs) Handles PayslipDeclaredToolStripMenuItem.Click, PayslipActualToolStripMenuItem.Click
        Dim IsActualFlag = Convert.ToInt16(DirectCast(sender, ToolStripMenuItem).Tag)

        PrintPayslip(IsActualFlag)
    End Sub

    Private Sub PrintPayslip(isActual As SByte)

        FunctionUtils.TryCatchFunction("Print Payslip",
            Sub()
                Dim payslipBuilder = MainServiceProvider.GetRequiredService(Of PayslipBuilder)

                Dim payPeriodId = ValNoComma(paypRowID)

                Dim reportDocument = payslipBuilder.CreateReportDocument(
                    payPeriodId:=payPeriodId,
                    isActual:=isActual)

                Dim crvwr As New CrysRepForm
                crvwr.crysrepvwr.ReportSource = reportDocument.GetReportDocument()
                crvwr.Show()

            End Sub,
            "Error generating payslips.")
    End Sub

    Private Shared Sub ShowPayrollSummary(isActual As Boolean)
        Dim reportProvider As New PayrollSummaryExcelFormatReportProvider With {
            .IsActual = isActual
        }

        reportProvider.Run()
    End Sub

    Private Sub ManageEmailPayslipsToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles ManagePrintPayslipsToolStripMenuItem.Click,
                ManageEmailPayslipsToolStripMenuItem.Click

        Dim form As SelectPayslipEmployeesForm

        Dim payPeriodId = ValNoComma(paypRowID)

        If sender Is ManageEmailPayslipsToolStripMenuItem Then
            form = New SelectPayslipEmployeesForm(payPeriodId, isEmail:=True)
        Else
            form = New SelectPayslipEmployeesForm(payPeriodId, isEmail:=False)
        End If

        form.ShowDialog()

    End Sub

    Private Sub PayrollSummaryByBranchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CostCenterReportToolStripMenuItem.Click

        Dim provider = New CostCenterReportProvider()

        provider.Run()

    End Sub

    Private Sub RecalculateThirteenthMonthPayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecalculateThirteenthMonthPayToolStripMenuItem.Click

        Dim payPeriodId = ObjectUtils.ToNullableInteger(ValNoComma(paypRowID))

        If payPeriodId Is Nothing Then Return

        Dim form As New SelectThirteenthMonthEmployeesForm(payPeriodId)

        form.ShowDialog()

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
        If datePeriod Is Nothing Then Return

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

End Class