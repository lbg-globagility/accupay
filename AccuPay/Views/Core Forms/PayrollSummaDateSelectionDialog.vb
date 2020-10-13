Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection
Imports MySql.Data.MySqlClient

Public Class PayrollSummaDateSelectionDialog

    Public Property ReportIndex As Integer

    Private _showLoanType As Boolean = False

    Private _loanTypeId As Integer?

    Dim yearnow As Integer = CDate(dbnow).Year

    Dim numofweekdays As Integer = 0

    Dim numofweekends As Integer = 0

    Dim paypFrom As Object = Nothing

    Dim paypTo As Object = Nothing

    Private _currentlyWorkedOnPayPeriod As IPayPeriod

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Sub New()

        InitializeComponent()

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

    End Sub

    Public ReadOnly Property PayPeriodFromID As Integer?
        Get
            Dim startPayPeriod = GetStartPayPeriod()
            If startPayPeriod Is Nothing Then
                Return Nothing
            Else
                Return startPayPeriod.RowID
            End If
        End Get
    End Property

    Public ReadOnly Property PayPeriodToID As Integer?
        Get
            Dim endPayPeriod = GetEndPayPeriod()
            If endPayPeriod Is Nothing Then
                Return Nothing
            Else
                Return endPayPeriod.RowID
            End If
        End Get
    End Property

    Public ReadOnly Property DateFrom As Date?
        Get
            Dim startPayPeriod = GetStartPayPeriod()
            If startPayPeriod Is Nothing Then
                Return Nothing
            Else
                Return startPayPeriod.PayFromDate
            End If
        End Get
    End Property

    Public ReadOnly Property DateTo As Date?
        Get
            Dim endPayPeriod = GetEndPayPeriod()
            If endPayPeriod Is Nothing Then
                Return Nothing
            Else
                Return endPayPeriod.PayToDate
            End If
        End Get
    End Property

    Public ReadOnly Property IsActual As Boolean
        Get
            Return RadioButton2.Checked
        End Get
    End Property

    Public Property ShowLoanType As Boolean
        Get
            Return _showLoanType
        End Get
        Set(value As Boolean)
            _showLoanType = value
        End Set
    End Property

    Public Property LoanTypeId As Integer?
        Get
            Return _loanTypeId
        End Get
        Set(value As Integer?)
            _loanTypeId = value
        End Set
    End Property

    Public Property PayPeriodID As Object

    Private _selectedPayPeriods As List(Of PayPeriod)

#Region "Loan Types"

    Protected Overrides Sub OnLoad(e As EventArgs)
        Dim boolResult = True

        Select Case ReportIndex

            Case 4 'Employee Loan Report

                cboStringParameter.Visible = boolResult
                TextBox1.Visible = boolResult
                Label360.Visible = boolResult
                Label5.Visible = boolResult

                enlistToCboBox("SELECT p.PartNo" &
                               " FROM product p" &
                               " INNER JOIN category c ON c.OrganizationID='" & orgztnID & "' AND c.CategoryName='Loan Type'" &
                               " WHERE p.CategoryID=c.RowID" &
                               " AND p.OrganizationID=" & orgztnID & ";",
                               cboxLoanType)

                cboStringParameter.DropDownStyle = ComboBoxStyle.DropDownList
                TextBox1.Text = "Loan Type"

            Case 6 'Payroll Summary Report
                Panel3.Visible = boolResult

                TextBox1.Visible = True
                Label360.Visible = True
                Label5.Visible = True

                With cboStringParameter
                    .Visible = True
                    .Items.Add(PayrollSummaryCategory.All)
                    .Items.Add(PayrollSummaryCategory.Cash)
                    .Items.Add(PayrollSummaryCategory.DirectDeposit)
                    .DropDownStyle = ComboBoxStyle.DropDownList
                End With

                TextBox1.Text = "Salary Distribution"
            Case Else
                Me.Height = 578
        End Select

        If _showLoanType Then
            LoadLoanTypesAsync()
            Panel5.Visible = _showLoanType

            AddHandler cboxLoanType.SelectedIndexChanged, AddressOf cboxLoanType_SelectedIndexChanged
        End If

        MyBase.OnLoad(e)
    End Sub

    Private Async Sub LoadLoanTypesAsync()
        Dim loanTypes = Await GetLoanTypes()

        cboxLoanType.DataSource = loanTypes.ToList()
    End Sub

    Private Async Function GetLoanTypes() As Task(Of ICollection(Of Product))
        'Select Case NULL `RowID`, 'All' `PartNo`
        '        UNION
        '    Select Case i.RowID, i.PartNo
        '            FROM() i
        Dim sql = <![CDATA[
                    SELECT p.PartNo, p.RowID
                    FROM category c
                    INNER JOIN product p ON p.CategoryID=c.RowID AND p.OrganizationID=c.OrganizationID AND p.ActiveData=TRUE
                    WHERE c.OrganizationID = @orgId
                    AND c.CategoryName=@leaveType
                    ORDER BY p.PartNo
                    ;
        ]]>.Value

        Dim products = New Collection(Of Product)
        Dim noRowId As Integer?

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            command.Parameters.AddWithValue("@orgId", CStr(z_OrganizationID))
            command.Parameters.AddWithValue("@leaveType", ProductConstant.LEAVE_TYPE_CATEGORY)

            Await connection.OpenAsync()

            Dim reader = Await command.ExecuteReaderAsync()

            products.Insert(0, New Product() With {.RowID = noRowId, .PartNo = "All"})
            While Await reader.ReadAsync()
                Dim partNo = reader.GetValue(Of String)("PartNo")
                Dim rowId = reader.GetValue(Of Integer)("RowID")

                Dim product = New Product() With {
                    .PartNo = partNo,
                    .RowID = rowId
                }

                products.Add(product)
            End While
        End Using

        Return products
    End Function

#End Region

    Private Async Sub PayrollSummaDateSelection_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        PayperiodsGridView.AutoGenerateColumns = False

        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"

        _currentlyWorkedOnPayPeriod = Await _payPeriodRepository.GetCurrentPayPeriodAsync(z_OrganizationID)

        DateFromLabel.Text = ""
        DateToLabel.Text = ""

        _selectedPayPeriods = New List(Of PayPeriod)
        Await LoadPayPeriods()
    End Sub

    Private Sub PayperiodsGridView_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles PayperiodsGridView.CellEndEdit
        UpdateDateDescription()

    End Sub

    Private Sub dgvpayperiod_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles PayperiodsGridView.CellContentClick

        If PayperiodsGridView.CurrentRow Is Nothing Then Return

        Dim selectedPayPeriod = CType(PayperiodsGridView.CurrentRow.DataBoundItem, PayPeriodWrapper)

        If selectedPayPeriod Is Nothing Then Return

        If e.ColumnIndex <> IsCheckedColumn.Index Then Return

        Dim checkBox As DataGridViewCheckBoxCell = CType(PayperiodsGridView.CurrentRow.Cells(IsCheckedColumn.Index), DataGridViewCheckBoxCell)
        Dim originalState = If(checkBox Is Nothing, False, (CType(checkBox.Value, Boolean)))

        If originalState = True Then

            _selectedPayPeriods.Remove(selectedPayPeriod.Model)
        Else

            'If the action is to check a cell and there currently two checked cells,
            'uncheck the earliest check cell so there will be always only 2 checked cells
            If _selectedPayPeriods.Count = 2 Then

                Dim earliestSelectedPayPeriod = _selectedPayPeriods(0)

                _selectedPayPeriods.Remove(earliestSelectedPayPeriod)

                Dim payPeriods = CType(PayperiodsGridView.DataSource, List(Of PayPeriodWrapper))
                Dim earliestSelectedPayPeriodWrapper = payPeriods.FirstOrDefault(Function(p) p.DateFrom = earliestSelectedPayPeriod.PayFromDate)

                If earliestSelectedPayPeriodWrapper IsNot Nothing Then

                    earliestSelectedPayPeriodWrapper.IsChecked = False
                End If

            End If

            _selectedPayPeriods.Add(selectedPayPeriod.Model)

        End If

        PayperiodsGridView.EndEdit()
        PayperiodsGridView.Refresh()
    End Sub

    Private Async Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        yearnow = yearnow + 1
        Await ChangePageLinks()
    End Sub

    Private Async Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        yearnow = yearnow - 1
        Await ChangePageLinks()
    End Sub

    Private Async Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        PayperiodsGridView.EndEdit()

        If cboStringParameter.Visible Then
            If cboStringParameter.Text = String.Empty Then
                WarnBalloon("Please select a " & TextBox1.Text, "Invalid " & TextBox1.Text, cboStringParameter, cboStringParameter.Width - 17, -69)
                Return
            End If
        End If

        If _selectedPayPeriods.Any() AndAlso DateFrom.HasValue AndAlso DateTo.HasValue Then

            Await CreatePayPeriodIfNotExists(GetStartPayPeriod())
            Await CreatePayPeriodIfNotExists(GetEndPayPeriod())

            If Not PayPeriodFromID.HasValue OrElse Not PayPeriodToID.HasValue Then

                'log: cannot create new pay period'
                MessageBoxHelper.Warning("Cannot get pay period data. Close and reopen the dialog then try again.")
                Return
            End If
        Else

            WarnBalloon("Please select a pay period. ", "Invalid Pay Period", linkPrev, 30, -470)
            Return
        End If

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Async Function CreatePayPeriodIfNotExists(currentPayPeriod As PayPeriod) As Task
        If currentPayPeriod.RowID Is Nothing Then

            Dim newlyCreatedId = Await CreateNewPayPeriod(currentPayPeriod)

            currentPayPeriod.RowID = newlyCreatedId

        End If
    End Function

    Private Shared Async Function CreateNewPayPeriod(startPayPeriod As PayPeriod) As Task(Of Integer?)
        Dim dataService = MainServiceProvider.GetRequiredService(Of PayPeriodDataService)
        Dim newPayPeriod = Await dataService.CreatesAsync(
            organizationId:=z_OrganizationID,
            month:=startPayPeriod.Month,
            year:=startPayPeriod.Year,
            isFirstHalf:=startPayPeriod.IsFirstHalf,
            createdByUserId:=z_User)

        Return newPayPeriod.RowID
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.Escape Then
            btnClose_Click(btnClose, New EventArgs)

            Return True
        Else
            Return MyBase.ProcessCmdKey(msg, keyData)
        End If
    End Function

    Private Sub ComboBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboStringParameter.KeyPress
        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 8 Then
            cboStringParameter.Text = String.Empty
            cboStringParameter.SelectedIndex = -1
        End If
    End Sub

    Private Sub cboxLoanType_SelectedIndexChanged(sender As Object, e As EventArgs)
        _loanTypeId = CInt(cboxLoanType.SelectedValue)
    End Sub

    Private Async Function LoadPayPeriods() As Task

        Dim payPeriods = (Await _payPeriodRepository.
            GetYearlyPayPeriodsAsync(
                organizationId:=z_OrganizationID,
                year:=Me.yearnow,
                currentUserId:=z_User)).
            Select(Function(p) New PayPeriodWrapper(p)).
            ToList()

        Dim focusedPayPeriod As PayPeriodWrapper = Nothing

        If _selectedPayPeriods.Any() Then

            Dim startPayPeriod = _selectedPayPeriods(0)
            focusedPayPeriod = CheckPayPeriodIfItIsInCurrentPage(startPayPeriod.PayFromDate, payPeriods)

            If _selectedPayPeriods.Count > 1 Then

                Dim endPayPeriod = _selectedPayPeriods(1)
                CheckPayPeriodIfItIsInCurrentPage(endPayPeriod.PayFromDate, payPeriods)
            End If
        Else
            focusedPayPeriod = CheckPayPeriodIfItIsInCurrentPage(_currentlyWorkedOnPayPeriod.PayFromDate, payPeriods)

            If focusedPayPeriod IsNot Nothing Then

                _selectedPayPeriods.Add(focusedPayPeriod.Model)
            End If

        End If

        PayperiodsGridView.DataSource = payPeriods
        FocusPayPeriod(focusedPayPeriod, payPeriods)
        UpdateDateDescription()

    End Function

    Private Function CheckPayPeriodIfItIsInCurrentPage(payPeriodPayFromDate As Date, payPeriods As List(Of PayPeriodWrapper)) As PayPeriodWrapper
        Dim currentPayPeriod = payPeriods.Where(Function(p) p.DateFrom = payPeriodPayFromDate).FirstOrDefault()

        If currentPayPeriod Is Nothing Then Return Nothing

        currentPayPeriod.IsChecked = True

        Return currentPayPeriod

    End Function

    Private Sub FocusPayPeriod(currentPayPeriod As PayPeriodWrapper, payPeriods As List(Of PayPeriodWrapper))

        If currentPayPeriod IsNot Nothing Then

            Dim currentPayPeriodWrapperIndex = payPeriods.IndexOf(currentPayPeriod)
            If currentPayPeriodWrapperIndex > PayperiodsGridView.Rows.Count - 1 Then Return

            Dim currentPayPeriodRow = PayperiodsGridView.Rows(currentPayPeriodWrapperIndex)

            If currentPayPeriodRow IsNot Nothing Then

                'currentPayPeriodRow.Selected = True
                currentPayPeriodRow.Cells(DateFromColumn.Index).Selected = True

            End If

        End If
    End Sub

    Private Function GetStartPayPeriod() As PayPeriod
        If _selectedPayPeriods Is Nothing OrElse Not _selectedPayPeriods.Any Then Return Nothing
        Return _selectedPayPeriods.OrderBy(Function(p) p.PayFromDate).FirstOrDefault()
    End Function

    Private Function GetEndPayPeriod() As PayPeriod
        If _selectedPayPeriods Is Nothing OrElse Not _selectedPayPeriods.Any Then Return Nothing
        Return _selectedPayPeriods.OrderByDescending(Function(p) p.PayFromDate).FirstOrDefault()
    End Function

    Private Sub UpdateDateDescription()

        DateFromLabel.Text = FormatDateString(DateFrom)
        DateToLabel.Text = FormatDateString(DateTo)
    End Sub

    Private Shared Function FormatDateString(dateInput As Date?) As String
        Return If(dateInput Is Nothing, String.Empty, dateInput.Value.ToString("MMMM d, yyyy"))
    End Function

    Private Async Function ChangePageLinks() As Task
        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"

        Dim sel_tab_pg = TabControl1.SelectedTab

        Await LoadPayPeriods()
    End Function

    Private Class PayPeriodWrapper

        Public ReadOnly Property RowID As Integer?
            Get
                Return Model.RowID
            End Get
        End Property

        Public ReadOnly Property DateFrom As Date
            Get
                Return Model.PayFromDate
            End Get
        End Property

        Public ReadOnly Property DateTo As Date
            Get
                Return Model.PayToDate
            End Get
        End Property

        Public ReadOnly Model As PayPeriod
        Public Property IsChecked As Boolean

        Sub New(payPeriod As PayPeriod)

            Model = payPeriod

        End Sub

    End Class

End Class