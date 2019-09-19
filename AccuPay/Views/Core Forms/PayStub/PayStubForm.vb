Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Threading.Tasks
Imports log4net
Imports System.Collections.Concurrent
Imports Microsoft.EntityFrameworkCore
Imports AccuPay.Views.Payroll
Imports AccuPay.Utils
Imports AccuPay.Entity

Public Class PayStubForm

    Private _logger As ILog = LogManager.GetLogger("PayrollLogger")

    Public current_year As String = CDate(dbnow).Year

    Private Const ItemsPerPage As Integer = 20

    Private _pageNo As Integer = 0

    Private _lastPageNo As Integer = 0

    Private _totalItems As Integer = 0

    Dim employeepicture As New DataTable

    Dim viewID As Integer = Nothing

    Public paypFrom As String = Nothing
    Public paypTo As String = Nothing
    Public paypRowID As String = Nothing
    Public paypPayFreqID As String = Nothing
    Public isEndOfMonth As String = 0

    Private _totalPaystubs As Integer = 0
    Private _finishedPaystubs As Integer = 0
    Private _successfulPaystubs As Integer = 0
    Private _failedPaystubs As Integer = 0

    Dim currentEmployeeID As String = Nothing

    Public withthirteenthmonthpay As SByte = 0

    Dim IsUserPressEnterToSearch As Boolean = False

    Dim selectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim quer_empPayFreq = ""

    Dim dtJosh As DataTable
    Dim da As New MySqlDataAdapter()

    Private sys_ownr As New SystemOwner

    Private _results As BlockingCollection(Of PayrollGeneration.Result)

    Private _payPeriodDataList As List(Of PayPeriodStatusData)

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

    Private Sub PayStub_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        viewID = VIEW_privilege("Employee Pay Slip", orgztnID)

        VIEW_payperiodofyear()

        dgvpayper.Focus()

        linkPrev.Text = "← " & (current_year - 1)
        linkNxt.Text = (current_year + 1) & " →"

        If dgvemployees.RowCount <> 0 Then
            dgvemployees.Item("EmployeeID", 0).Selected = 1
        End If

        Dim formuserprivilege = position_view_table.Select("ViewID = " & viewID)

        If formuserprivilege.Count = 0 Then

            ManagePayrollToolStripDropDownButton.Visible = 0
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    ManagePayrollToolStripDropDownButton.Visible = 0

                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        ManagePayrollToolStripDropDownButton.Visible = 0
                    Else
                        ManagePayrollToolStripDropDownButton.Visible = 1
                    End If

                End If

            Next

        End If

        cboProducts.ValueMember = "ProductID"
        cboProducts.DisplayMember = "ProductName"

        cboProducts.DataSource = New SQLQueryToDatatable("SELECT RowID AS 'ProductID', Name AS 'ProductName', Category FROM product WHERE Category IN ('Allowance Type', 'Bonus', 'Adjustment Type')" &
                                                  " AND OrganizationID='" & orgztnID & "' ORDER BY Name;").ResultTable

        dgvAdjustments.AutoGenerateColumns = False

    End Sub

    Sub VIEW_payperiodofyear(Optional param_Date As Object = Nothing)
        ClosePayrollToolStripMenuItem.Visible = False
        ReopenPayrollToolStripMenuItem.Visible = False

        Dim hasValue = (MDIPrimaryForm.systemprogressbar.Value > 0)
        If hasValue Then
            For i = 0 To 5
                RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
            Next
        End If

        Dim date_param = If(param_Date = Nothing, "NULL", "'" & param_Date & "-01-01'")
        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL VIEW_payperiodofyear('" & orgztnID & "'," & date_param & ",'0');")
        Dim catchdt As New DataTable : catchdt = n_SQLQueryToDatatable.ResultTable

        Dim payPeriodsWithPaystubCount = PayPeriodStatusData.GetPeriodsWithPaystubCount()
        _payPeriodDataList = New List(Of PayPeriodStatusData)

        dgvpayper.Rows.Clear()
        Dim index As Integer = 0
        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvpayper.Rows.Add(row_array)

            Dim payPeriodData = CreatePayPeriodData(payPeriodsWithPaystubCount, index, drow) 'drow("ppRowID").ToString
            If payPeriodData IsNot Nothing Then
                _payPeriodDataList.Add(payPeriodData)
            End If

            index += 1
        Next
        catchdt.Dispose()
        If hasValue Then
            AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
        End If
    End Sub

    Private Function CreatePayPeriodData(payPeriodsWithPaystubCount As List(Of PayPeriod), index As Integer, drow As DataRow) As PayPeriodStatusData
        Dim payPeriodData As New PayPeriodStatusData

        payPeriodData.Index = index
        Dim currentRow = dgvpayper.Rows(payPeriodData.Index)
        payPeriodData.Status = PayPeriodStatusData.PayPeriodStatus.Open

        If drow("IsClosed") <> 0 Then
            'the payperiods here are closed
            'currentRow.DefaultCellStyle.ForeColor = Color.Gray
            payPeriodData.Status = PayPeriodStatusData.PayPeriodStatus.Closed
        Else

            'check if this open payperiod is already modified
            If payPeriodsWithPaystubCount.Any(Function(p) p.RowID.Value = drow("ppRowID")) Then

                currentRow.DefaultCellStyle.SelectionForeColor = Color.Green

                Dim defaultFont = currentRow.DefaultCellStyle.Font
                If defaultFont Is Nothing Then

                    If dgvpayper.DefaultCellStyle.Font IsNot Nothing Then

                        defaultFont = dgvpayper.DefaultCellStyle.Font

                    ElseIf PayStubForm.DefaultFont IsNot Nothing Then

                        defaultFont = PayStubForm.DefaultFont
                    Else

                        defaultFont = New Font("Microsoft Sans Serif", 8.25)
                    End If

                    currentRow.DefaultCellStyle.Font = New Font(defaultFont, FontStyle.Bold)

                End If

                payPeriodData.Status = PayPeriodStatusData.PayPeriodStatus.Processing

            End If

        End If

        Return payPeriodData
    End Function

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
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
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

                _year = current_year

                Dim select_cutoff_payfrequency =
                    tstrip.Items.OfType(Of ToolStripButton).Where(Function(tsbtn) tsbtn.Text = str_sched_payfreq)

                For Each _tsbtn In select_cutoff_payfrequency
                    PayFreq_Changed(_tsbtn, New EventArgs)
                Next

                If .Index < 0 OrElse .Index >= _payPeriodDataList.Count Then Return

                Dim currentPayperiod = _payPeriodDataList(.Index)
                Dim isModified = currentPayperiod.Status = PayPeriodStatusData.PayPeriodStatus.Processing

                ShowPayrollActions(isModified,
                    New PayPeriodStatusData.PayPeriodStatusReference(currentPayperiod.Status))

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

    Private Sub ShowPayrollActions(showPayrollActions As Boolean, Optional payrollStatus As PayPeriodStatusData.PayPeriodStatusReference = Nothing)

        DeleteToolStripDropDownButton.Visible = showPayrollActions

        If payrollStatus Is Nothing Then

            DisableAllPayrollActions()
        Else

            Select Case (payrollStatus.PayPeriodStatus)
                Case PayPeriodStatusData.PayPeriodStatus.Open

                    DisableAllPayrollActions()
                    ClosePayrollToolStripMenuItem.Visible = True

                Case PayPeriodStatusData.PayPeriodStatus.Processing

                    EnablePayrollToolStripItem()
                    ReopenPayrollToolStripMenuItem.Visible = False

                    EnableAdjustmentsInput()

                Case PayPeriodStatusData.PayPeriodStatus.Closed

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
        'RegeneratePayrollToolStripMenuItem.Visible = enable
        DeletePayrollToolStripMenuItem.Visible = enable
        ReopenPayrollToolStripMenuItem.Visible = enable
        ClosePayrollToolStripMenuItem.Visible = enable
        OthersToolStripMenuItem.Visible = enable
    End Sub

    Private Sub EnableAdjustmentsInput(Optional enable As Boolean = True)
        dgvAdjustments.Enabled = enable
        btnSaveAdjustments.Visible = enable
        btndiscardchanges.Visible = enable
    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        current_year = current_year - 1

        linkPrev.Text = "← " & (current_year - 1)
        linkNxt.Text = (current_year + 1) & " →"

        VIEW_payperiodofyear(current_year)

        AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        dgvpayper_SelectionChanged(sender, e)
    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        current_year = current_year + 1

        linkNxt.Text = (current_year + 1) & " →"
        linkPrev.Text = "← " & (current_year - 1)

        VIEW_payperiodofyear(current_year)

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
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
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

                currentEmployeeID = .Cells("EmployeeID").Value

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

                txttotabsentamt.Text = "0.00"
                txttottardiamt.Text = "0.00"
                txttotutamt.Text = "0.00"

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
                        TabPage4_Enter(TabPage4, New EventArgs)
                    End If
                Else

                    txtgrosssal.Text = ""
                    txtnetsal.Text = ""
                    txttaxabsal.Text = ""

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
                        TabPage4_Enter(TabPage4, New EventArgs)
                    End If
                End If

            End With
        Else
            sameEmpID = -1

            currentEmployeeID = Nothing

            pbEmpPicChk.Image = Nothing
            txtFName.Text = ""
            txtEmpID.Text = ""

            txtBasicPay.Text = ""

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

            txtemptotallow.Text = ""
            txtTotTaxabAllow.Text = ""

            txtgrosssal.Text = ""

            txttotabsent.Text = ""
            txttotabsentamt.Text = ""

            txttottardi.Text = ""
            txttottardiamt.Text = ""

            txttotut.Text = ""
            txttotutamt.Text = ""

            lblsubtotmisc.Text = ""

            txtempsss.Text = ""
            txtempphh.Text = ""
            txtemphdmf.Text = ""

            txtemptotloan.Text = ""
            txtemptotbon.Text = ""

            txttaxabsal.Text = ""
            txtempwtax.Text = ""
            txtnetsal.Text = ""

            txtPaidLeave.Text = ""
            txtThirteenthMonthPay.Text = ""
            txtTotalNetPay.Text = ""

        End If

    End Sub

    Sub genpayroll(Optional PayFreqRowID As Object = Nothing)
        Dim loadTask = Task.Factory.StartNew(
            Function()
                If paypFrom = Nothing And paypTo = Nothing Then
                    Return Nothing
                End If

                Dim resources = New PayrollResources(Integer.Parse(paypRowID), CDate(paypFrom), CDate(paypTo))
                Dim resourcesTask = resources.Load()
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

    Private Async Function LoadingPayrollDataOnSuccess(t As Task(Of PayrollResources)) As Task
        Await ThreadingPayrollGeneration(t.Result)
    End Function

    Private Sub LoadingPayrollDataOnError(t As Task)
        _logger.Error("Error loading one of the payroll data.", t.Exception)
        MsgBox("Something went wrong while loading the payroll data needed for computation. Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Resources")
        Me.Enabled = True
    End Sub

    Private Async Function ThreadingPayrollGeneration(resources As PayrollResources) As Task
        _finishedPaystubs = 0

        Try
            Me.Enabled = False

            _totalPaystubs = resources.Employees.Count
            _successfulPaystubs = 0
            _failedPaystubs = 0
            _results = New BlockingCollection(Of PayrollGeneration.Result)()

            Await Task.Run(
                Sub()
                    Parallel.ForEach(
                        resources.Employees,
                        Sub(employee)
                            Dim generator = New PayrollGeneration(
                                employee,
                                resources,
                                Me
                            )

                            generator.DoProcess()
                        End Sub)
                End Sub)

            RefreshForm()

            Await TimeEntrySummaryForm.LoadPayPeriods()
        Catch ex As Exception
            _logger.Error("Error loading the employees", ex)
        End Try
    End Function

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

        showAuditTrail.Close()
        selectPayPeriod.Close()
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

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(viewID)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub ActualToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles DeclaredToolStripMenuItem3.Click, ActualToolStripMenuItem3.Click

        Dim psefr As New PayrollSummaryExcelFormatReportProvider

        Dim is_actual = Convert.ToInt16(DirectCast(sender, ToolStripMenuItem).Tag)

        psefr.IsActual = is_actual

        psefr.Run()

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

                Dim new_tsbtn As New ToolStripButton

                With new_tsbtn

                    .AutoSize = False
                    .BackColor = Color.FromArgb(255, 255, 255)
                    .ImageTransparentColor = System.Drawing.Color.Magenta
                    .Margin = New System.Windows.Forms.Padding(0, 1, 0, 1)
                    .Name = String.Concat("tsbtn" & strval)
                    .Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
                    .Size = New System.Drawing.Size(110, 30)
                    .Text = strval
                    .TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                    .TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
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

    Private Sub dgAdjustments_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAdjustments.CellContentClick
        If TypeOf dgvAdjustments.Columns(e.ColumnIndex) Is DataGridViewLinkColumn _
            AndAlso e.RowIndex >= 0 Then
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
                                                Me.currentEmployeeID,
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

                .Parameters.AddWithValue("pa_EmployeeID", Me.currentEmployeeID)
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
                .Parameters.AddWithValue("pa_EmployeeID", Me.currentEmployeeID)
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
        For Each txtbxctrl In TabPage1.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag

        Dim _params = New Object() {orgztnID,
                                    EmployeeRowID,
                                    paypFrom,
                                    paypTo}

        Dim n_SQLQueryToDatatable As _
            New SQL("CALL VIEW_paystubitem_declared(?OrganizID, ?EmpRowID, ?pay_date_from, ?pay_date_to);",
                    _params)

        Dim paystubactual = n_SQLQueryToDatatable.GetFoundRows.Tables(0)

        Dim txtbxField = Panel6.Controls.OfType(Of TextBox).Where(Function(txbx) String.IsNullOrWhiteSpace(txbx.AccessibleDescription) = False)

        For Each drow As DataRow In paystubactual.Rows

            Dim psaItems = New SQL("CALL VIEW_paystubitem('" & ValNoComma(drow("RowID")) & "');").GetFoundRows.Tables(0)

            Dim strdouble = ValNoComma(drow("TrueSalary")) / ValNoComma(drow("PAYFREQUENCYDIVISOR")) 'BasicPay

            txtBasicPay.Text = FormatNumber(ValNoComma(strdouble), 2)

            txtRegularHours.Text = ValNoComma(drow("RegularHours"))

            If drow("EmployeeType").ToString = "Fixed" Then
                txtRegularPay.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then
                Dim basicPay = ValNoComma(drow("BasicPay"))
                Dim deductions = 0.0

                If drow("FirstTimeSalary").ToString = "True" Then
                    basicPay = ValNoComma(drow("RegularPay"))
                Else
                    basicPay = ValNoComma(drow("BasicPay"))
                    deductions = ValNoComma(drow("LateDeduction")) +
                    ValNoComma(drow("UndertimeDeduction")) +
                    ValNoComma(drow("Absent")) '+
                    'ValNoComma(drow("HolidayPay"))
                End If

                txtRegularPay.Text = FormatNumber(basicPay - deductions, 2)
            ElseIf drow("EmployeeType").ToString = "Daily" Then
                txtRegularPay.Text = FormatNumber(ValNoComma(drow("RegularPay")), 2)
            End If

            txtOvertimeHours.Text = ValNoComma(drow("OvertimeHours"))
            txtOvertimePay.Text = FormatNumber(ValNoComma(drow("OvertimePay")), 2)

            txtNightDiffHours.Text = ValNoComma(drow("NightDiffHours"))
            txtNightDiffPay.Text = FormatNumber(ValNoComma(drow("NightDiffPay")), 2)

            txtNightDiffOvertimeHours.Text = ValNoComma(drow("NightDiffOvertimeHours"))
            txtNightDiffOvertimePay.Text = FormatNumber(ValNoComma(drow("NightDiffOvertimePay")), 2)

            txtRestDayHours.Text = ValNoComma(drow("RestDayHours"))
            txtRestDayAmount.Text = FormatNumber(ValNoComma(drow("RestDayPay")), 2)

            txtHolidayHours.Text = 0.0
            txtHolidayPay.Text = FormatNumber(ValNoComma(drow("HolidayPay")), 2)

            Dim sumallbasic = ValNoComma(drow("RegularPay")) +
                          ValNoComma(drow("OvertimePay")) +
                          ValNoComma(drow("NightDiffPay")) +
                          ValNoComma(drow("NightDiffOvertimePay")) +
                          ValNoComma(drow("HolidayPay"))

            If drow("EmployeeType").ToString = "Fixed" Then
                lblSubtotal.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then
                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "True" Then
                    thebasicpay = ValNoComma(drow("RegularPay"))
                    lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction")) + ValNoComma(drow("Absent"))
                    'Dim all_regular = (thebasicpay - (thelessamounts + ValNoComma(drow("HolidayPay"))))
                    Dim all_regular = (thebasicpay - thelessamounts)
                    lblSubtotal.Text =
                    FormatNumber(all_regular + ValNoComma(drow("HolidayPay")) +
                                 ValNoComma(drow("OvertimePay")) +
                                 ValNoComma(drow("NightDiffPay")) +
                                 ValNoComma(drow("NightDiffOvertimePay")), 2)
                End If
            Else
                lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)

            End If

            'Absent
            txttotabsent.Text = FormatNumber(ValNoComma((drow("AbsentHours"))), 2)
            txttotabsentamt.Text = FormatNumber(ValNoComma((drow("Absent"))), 2)
            'Tardiness / late
            txttottardi.Text = ValNoComma(drow("LateHours"))
            txttottardiamt.Text = FormatNumber(ValNoComma((drow("LateDeduction"))), 2)
            'Undertime
            txttotut.Text = ValNoComma(drow("UndertimeHours"))
            txttotutamt.Text = FormatNumber(ValNoComma((drow("UndertimeDeduction"))), 2)

            Dim miscsubtotal = ValNoComma(drow("Absent")) + ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction"))
            lblsubtotmisc.Text = FormatNumber(ValNoComma((miscsubtotal)), 2)

            'Allowance
            txtemptotallow.Text = FormatNumber(ValNoComma((drow("TotalAllowance"))), 2)

            'Taxable Allowance
            txtTotTaxabAllow.Text = FormatNumber(ValNoComma((drow("TotalTaxableAllowance"))), 2)

            txtGrandTotalAllow.Text = FormatNumber(ValNoComma(drow("TotalAllowance")) + ValNoComma(drow("TotalTaxableAllowance")), 2)

            'Bonus
            txtemptotbon.Text = FormatNumber(ValNoComma((drow("TotalBonus"))), 2)
            'Gross
            txtgrosssal.Text = FormatNumber(ValNoComma((drow("TotalGrossSalary"))), 2)

            'SSS
            txtempsss.Text = FormatNumber(ValNoComma((drow("TotalEmpSSS"))), 2)
            'PhilHealth
            txtempphh.Text = FormatNumber(ValNoComma((drow("TotalEmpPhilhealth"))), 2)
            'PAGIBIG
            txtemphdmf.Text = FormatNumber(ValNoComma((drow("TotalEmpHDMF"))), 2)

            'Taxable salary
            txttaxabsal.Text = FormatNumber(ValNoComma((drow("TotalTaxableSalary"))), 2)
            'Withholding taxS
            txtempwtax.Text = FormatNumber(ValNoComma((drow("TotalEmpWithholdingTax"))), 2)
            'Loans
            txtemptotloan.Text = FormatNumber(ValNoComma((drow("TotalLoans"))), 2)
            'Adjustments
            txtTotalAdjustments.Text = FormatNumber(ValNoComma((drow("TotalAdjustments"))), 2)

            Dim totalAgencyFee = ValNoComma(drow("TotalAgencyFee"))
            txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

            Dim thirteenthMonthPay = ValNoComma(drow("ThirteenthMonthPay"))
            txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

            Dim totalNetSalary = ValNoComma(drow("TotalNetSalary")) + totalAgencyFee
            'Net
            txtnetsal.Text = FormatNumber(totalNetSalary, 2)

            Dim totalNetPay = totalNetSalary + thirteenthMonthPay
            txtTotalNetPay.Text = FormatNumber(totalNetPay, 2)

            txtPaidLeave.Text = FormatNumber(ValNoComma(drow("PaidLeaveAmount")), 2)

            For Each txtbx In txtbxField
                txtbx.Text = If(IsDBNull(drow(txtbx.AccessibleDescription)), 0.0, drow(txtbx.AccessibleDescription))
            Next

            Exit For
        Next

        paystubactual.Dispose()
        UpdateAdjustmentDetails(Convert.ToInt16(DirectCast(sender, TabPage).Tag))
    End Sub

    Private Sub TabPage1_Enter(sender As Object, e As EventArgs) Handles TabPage1.Enter 'DECLARED
        TabPage1.Text = TabPage1.Text.Trim
        TabPage1.Text = TabPage1.Text & Space(15)
        TabPage4.Text = TabPage4.Text.Trim
    End Sub

    Private Sub TabPage4_Enter1(sender As Object, e As EventArgs) Handles TabPage4.Enter 'UNDECLARED / ACTUAL
        TabPage4.Text = TabPage4.Text.Trim
        TabPage4.Text = TabPage4.Text & Space(15)
        TabPage1.Text = TabPage1.Text.Trim

        For Each txtbxctrl In TabPage4.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag

        Dim _params = New Object() {
            orgztnID,
            EmployeeRowID,
            paypFrom,
            paypTo
        }

        Dim n_SQLQueryToDatatable = New SQL("CALL VIEW_paystubitem_actual(?OrganizID, ?EmpRowID, ?pay_date_from, ?pay_date_to);", _params)

        Dim paystubactual As New DataTable

        paystubactual = n_SQLQueryToDatatable.GetFoundRows.Tables(0)

        'Dim psaItems As New DataTable

        Dim txtbxField = Panel6.Controls.OfType(Of TextBox).Where(Function(txbx) String.IsNullOrWhiteSpace(txbx.AccessibleDescription) = False)

        For Each drow As DataRow In paystubactual.Rows

            'psaItems = New SQL("CALL VIEW_paystubitemundeclared('" & ValNoComma(drow("RowID")) & "');").GetFoundRows.Tables(0)

            Dim strdouble = ValNoComma(drow("BasicPay")) 'BasicPay

            ' Basic Pay
            txtempbasicpay_U.Text = FormatNumber(ValNoComma(strdouble), 2)

            ' Regular hours
            txthrswork_U.Text = ValNoComma(drow("RegularHours"))

            ' Regular pay
            If drow("EmployeeType").ToString = "Fixed" Then

                txthrsworkamt_U.Text = FormatNumber(ValNoComma(strdouble), 2)

            ElseIf drow("EmployeeType").ToString = "Monthly" Then

                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "True" Then
                    thebasicpay = ValNoComma(drow("RegularPay"))
                Else
                    thebasicpay = ValNoComma(drow("RegularPay"))
                End If

                txthrsworkamt_U.Text = FormatNumber((thebasicpay - thelessamounts), 2)
            Else
                txthrsworkamt_U.Text = FormatNumber(ValNoComma((drow("RegularPay"))), 2)
            End If

            ' Over time
            txttotothrs_U.Text = ValNoComma(drow("OvertimeHours"))
            txttototamt_U.Text = FormatNumber(ValNoComma((drow("OvertimePay"))), 2)

            ' Night differential
            txttotnightdiffhrs_U.Text = ValNoComma(drow("NightDiffHours"))
            txttotnightdiffamt_U.Text = FormatNumber(ValNoComma((drow("NightDiffPay"))), 2)

            ' Night differential Overtime
            txttotnightdiffothrs_U.Text = ValNoComma(drow("NightDiffOvertimeHours"))
            txttotnightdiffotamt_U.Text = FormatNumber(ValNoComma((drow("NightDiffOvertimePay"))), 2)

            If drow("EmployeeType").ToString = "Fixed" Then
                lblSubtotal.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then

                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "True" Then
                    thebasicpay = ValNoComma(drow("RegularPay"))

                    lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction")) + ValNoComma(drow("AbsenceDeduction"))

                    Dim all_regular = (thebasicpay - thelessamounts)
                    lblSubtotal.Text = FormatNumber(
                        all_regular +
                        ValNoComma(drow("SpecialHolidayPay")) +
                        ValNoComma(drow("SpecialHolidayOTPay")) +
                        ValNoComma(drow("RegularHolidayPay")) +
                        ValNoComma(drow("RegularHolidayOTPay")) +
                        ValNoComma(drow("OvertimePay")) +
                        ValNoComma(drow("NightDiffPay")) +
                        ValNoComma(drow("NightDiffOvertimePay")),
                        2)
                End If
            Else
                lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
            End If

            'Absent
            txttotabsent_U.Text = FormatNumber(ValNoComma((drow("AbsentHours"))), 2)
            txttotabsentamt_U.Text = FormatNumber(ValNoComma((drow("AbsenceDeduction"))), 2)

            'Tardiness / late
            txttottardi_U.Text = ValNoComma(drow("LateHours"))
            txttottardiamt_U.Text = FormatNumber(ValNoComma((drow("LateDeduction"))), 2)

            'Undertime
            txttotut_U.Text = ValNoComma(drow("UndertimeHours"))
            txttotutamt_U.Text = FormatNumber(ValNoComma((drow("UndertimeDeduction"))), 2)

            Dim miscsubtotal = ValNoComma(drow("AbsenceDeduction")) + ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction"))
            lblsubtotmisc.Text = FormatNumber(ValNoComma((miscsubtotal)), 2)

            'Allowance
            txtemptotallow.Text = FormatNumber(ValNoComma((drow("TotalAllowance"))), 2)

            'Taxable Allowance
            txtTotTaxabAllow.Text = FormatNumber(ValNoComma((drow("TotalTaxableAllowance"))), 2)

            txtGrandTotalAllow.Text = FormatNumber(ValNoComma(drow("TotalAllowance")) + ValNoComma(drow("TotalTaxableAllowance")), 2)

            'Bonus
            txtemptotbon.Text = FormatNumber(ValNoComma((drow("TotalBonus"))), 2)
            'Gross
            txtgrosssal.Text = FormatNumber(ValNoComma((drow("TotalGrossSalary"))), 2)

            ' SSS
            txtempsss.Text = FormatNumber(ValNoComma((drow("TotalEmpSSS"))), 2)
            ' PhilHealth
            txtempphh.Text = FormatNumber(ValNoComma((drow("TotalEmpPhilhealth"))), 2)
            ' PAGIBIG
            txtemphdmf.Text = FormatNumber(ValNoComma((drow("TotalEmpHDMF"))), 2)

            ' Taxable salary
            txttaxabsal.Text = FormatNumber(ValNoComma((drow("TotalTaxableSalary"))), 2)
            ' Withholding taxS
            txtempwtax.Text = FormatNumber(ValNoComma((drow("TotalEmpWithholdingTax"))), 2)
            ' Loans
            txtemptotloan.Text = FormatNumber(ValNoComma((drow("TotalLoans"))), 2)
            ' Adjustments
            txtTotalAdjustments.Text = FormatNumber(ValNoComma((drow("TotalAdjustments"))), 2)

            ' Agency fee
            Dim totalAgencyFee = ValNoComma(drow("TotalAgencyFee"))
            txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

            Dim thirteenthMonthPay = ValNoComma(drow("ThirteenthMonthPay"))
            txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

            Dim totalNetSalary = ValNoComma(drow("TotalNetSalary")) + totalAgencyFee
            'Net
            txtnetsal.Text = FormatNumber(totalNetSalary, 2)

            Dim totalNetPay = totalNetSalary + thirteenthMonthPay
            txtTotalNetPay.Text = FormatNumber(totalNetPay, 2)

            txtPaidLeave.Text = FormatNumber(ValNoComma(drow("LeavePay")), 2)

            For Each txtbx In txtbxField
                txtbx.Text = If(IsDBNull(drow(txtbx.AccessibleDescription)), 0.0, drow(txtbx.AccessibleDescription))
            Next

            Exit For
        Next

        paystubactual.Dispose()

        UpdateAdjustmentDetails(Convert.ToInt16(DirectCast(sender, TabPage).Tag))

    End Sub

    Private Sub TabPage4_Enter(sender As Object, e As EventArgs) 'Handles TabPage4.Enter 'UNDECLARED
        TabPage4_Enter1(TabPage4, New EventArgs)
    End Sub

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

    Private Async Sub tsbtnDelEmpPayroll_Click(sender As Object, e As EventArgs) Handles DeleteToolStripDropDownButton.Click

        If currentEmployeeID = Nothing Then
        Else

            Dim toBeDeletedEmployeeID = currentEmployeeID
            Dim toBeDeletedPaypFrom = paypFrom
            Dim toBeDeletedPaypTo = paypTo

            DeleteToolStripDropDownButton.Enabled = False

            Dim prompt = MessageBox.Show("Do you want to delete the '" & CDate(toBeDeletedPaypFrom).ToShortDateString &
                                         "' to '" & CDate(toBeDeletedPaypTo).ToShortDateString &
                                         "' payroll of employee '" & toBeDeletedEmployeeID & "' ?",
                                         "Delete employee payroll",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            If prompt = Windows.Forms.DialogResult.Yes Then

                PayrollTools.DeletePaystub(dgvemployees.Tag, paypRowID)

                RefreshForm()

                Await TimeEntrySummaryForm.LoadPayPeriods()

                MessageBoxHelper.Information($"Paystub of employee {toBeDeletedEmployeeID} for payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}' was successfully deleted.")

            End If

            DeleteToolStripDropDownButton.Enabled = True

        End If

    End Sub

    Private Sub dgvemployees_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvemployees.RowsRemoved
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    Private Sub PrintAllPaySlip_Click(sender As Object, e As EventArgs) Handles DeclaredToolStripMenuItem2.Click, ActualToolStripMenuItem2.Click
        Dim IsActualFlag = Convert.ToInt16(DirectCast(sender, ToolStripMenuItem).Tag)

        Dim n_PrintAllPaySlipOfficialFormat As _
            New PrintAllPaySlipOfficialFormat(ValNoComma(paypRowID),
                                              IsActualFlag)
    End Sub

    Sub ProgressCounter(result As PayrollGeneration.Result)
        If result.Status = PayrollGeneration.ResultStatus.Success Then
            Interlocked.Increment(_successfulPaystubs)
        Else
            Interlocked.Increment(_failedPaystubs)
        End If

        Interlocked.Increment(_finishedPaystubs)
        _results.Add(result)

        Dim percentComplete As Integer = (_finishedPaystubs / _totalPaystubs) * 100
        MDIPrimaryForm.systemprogressbar.Value = percentComplete

        If _finishedPaystubs = _totalPaystubs Then

            PayrollTools.UpdateLoanSchedule(paypRowID)

            Dim dialog = New PayrollResultDialog(_results.ToList()) With {
                .Owner = Me
            }
            dialog.ShowDialog()

            Me.Enabled = True
            dgvpayper_SelectionChanged(dgvpayper, New EventArgs)
        End If
    End Sub

    Private Sub setProperInterfaceBaseOnCurrentSystemOwner()
        Static _bool As Boolean =
            (sys_ownr.CurrentSystemOwner = SystemOwner.Cinema2000)

        If _bool Then

            Dim str_empty As String = String.Empty

            TabPage1.Text = str_empty

            TabPage4.Text = str_empty

            AddHandler tabEarned.Selecting, AddressOf tabEarned_Selecting
        Else

        End If

    End Sub

    Private Sub tabEarned_Selecting(sender As Object, e As TabControlCancelEventArgs)

        Static _bool As Boolean =
            (sys_ownr.CurrentSystemOwner = SystemOwner.Cinema2000)

        e.Cancel = _bool

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

        Await UpdatePayrollStatus(open:=False)

    End Sub

    Private Async Sub ReopenPayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReopenPayrollToolStripMenuItem.Click

        Await UpdatePayrollStatus(open:=True)

    End Sub

    Private Async Function UpdatePayrollStatus(open As Boolean) As Task(Of Boolean)

        Dim payPeriodId = ObjectUtils.ToNullableInteger(paypRowID)

        If payPeriodId Is Nothing Then

            MessageBoxHelper.Warning("Please select a pay period first.")
            Return False
        End If

        Using context As New PayrollContext

            Dim payPeriod = Await context.PayPeriods.FirstOrDefaultAsync(Function(p) p.RowID.Value = payPeriodId)

            If payPeriod Is Nothing Then

                MessageBoxHelper.Warning("Pay period does not exists. Please refresh the form.")
                Return False
            End If

            'if the action is to reopen, check if this payperiod already has paystubs
            'and if there is an existing OPEN payperiod that also has paystubs (PROCESSING pay pay period)
            'Multiple OPEN or CLOSE pay periods are allowed
            'Multiple PROCESSING pay periods are NOT allowed
            Dim otherProcessingPayPeriod = Await context.Paystubs.
                    Include(Function(p) p.PayPeriod).
                    Where(Function(p) p.PayPeriod.RowID.Value <> payPeriod.RowID.Value).
                    Where(Function(p) p.PayPeriod.IsClosed = False).
                    Where(Function(p) p.PayPeriod.OrganizationID.Value = z_OrganizationID).
                    FirstOrDefaultAsync()

            If open = True AndAlso otherProcessingPayPeriod IsNot Nothing Then

                MessageBoxHelper.Warning("There is currently a pay period with ""PROCESSING"" status. Please finish that pay period first then close it to reopen the selected pay period.")
                Return False

            End If

            payPeriod.IsClosed = Not open

            Await context.SaveChangesAsync

        End Using

        RefreshForm()

        Await TimeEntrySummaryForm.LoadPayPeriods()

        If open Then

            MessageBoxHelper.Information("Pay period was reopened successfully.")
        Else

            MessageBoxHelper.Information("Pay period was closed successfully.")

        End If

        Return True

    End Function

    Private Sub RefreshForm()
        VIEW_payperiodofyear()
        dgvpayper_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub GeneratePayrollToolStripButton_Click(sender As Object, e As EventArgs) Handles GeneratePayrollToolStripButton.Click

        With selectPayPeriod
            .Show()
            .BringToFront()
            .dgvpaypers.Focus()
        End With

    End Sub

    Private Sub Include13thMonthPayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Include13thMonthPayToolStripMenuItem.Click
        Dim payPeriodSelector = New PayrollSummaDateSelection()

        If payPeriodSelector.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim dateFrom = payPeriodSelector.DateFrom
        Dim dateTo = payPeriodSelector.DateTo

        Dim realse = New ReleaseThirteenthMonthPay(dateFrom, dateTo, paypRowID)
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

        Dim cashOut = New CashOutUnusedLeave(dateFromId, dateToId, paypRowID)
        cashOut.Execute()
    End Sub

    Private Sub RegenerateToolStripButton_Click(sender As Object, e As EventArgs) Handles RegeneratePayrollToolStripMenuItem.Click
        'TODO regenerate payroll
    End Sub

    Private Async Sub DeletePayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeletePayrollToolStripMenuItem.Click
        DeletePayrollToolStripMenuItem.Enabled = False

        Dim toBeDeletedPaypFrom = paypFrom
        Dim toBeDeletedPaypTo = paypTo

        Dim prompt = MessageBox.Show($"Do you want to delete all paystubs of employees for payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}'?",
                                     "Delete all employee payroll",
                                     MessageBoxButtons.YesNoCancel,
                                     MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If prompt = Windows.Forms.DialogResult.Yes Then
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            Dim payperiodId As Integer? = Integer.Parse(paypRowID)

            Dim paystubIds As IList(Of Integer?)
            Using context = New PayrollContext
                paystubIds = Await context.Paystubs.Where(Function(p) p.PayPeriodID = payperiodId).
                    Select(Function(p) p.RowID).
                    ToListAsync()
            End Using

            For Each p In paystubIds
                Dim query = New ExecuteQuery($"CALL DEL_specificpaystub('{p}');")
            Next

            RefreshForm()

            Await TimeEntrySummaryForm.LoadPayPeriods()

            MessageBoxHelper.Information($"All paystubs for payroll '{CDate(toBeDeletedPaypFrom).ToShortDateString}' to '{CDate(toBeDeletedPaypTo).ToShortDateString}' was successfully deleted.")

        End If

        DeletePayrollToolStripMenuItem.Enabled = True
    End Sub

End Class