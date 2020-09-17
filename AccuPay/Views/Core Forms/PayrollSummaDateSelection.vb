Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection
Imports MySql.Data.MySqlClient

Public Class PayrollSummaDateSelection

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
            Return If(_payPeriodFrom Is Nothing, _payPeriodTo?.RowID, _payPeriodFrom.RowID)
        End Get
    End Property

    Public ReadOnly Property PayPeriodToID As Integer?
        Get
            Return If(_payPeriodTo Is Nothing, _payPeriodFrom?.RowID, _payPeriodTo.RowID)
        End Get
    End Property

    Public ReadOnly Property DateFrom As Date?
        Get
            Return If(_payPeriodFrom Is Nothing, _payPeriodTo?.DateFrom, _payPeriodFrom.DateFrom)
        End Get
    End Property

    Public ReadOnly Property DateTo As Date?
        Get
            Return If(_payPeriodTo Is Nothing, _payPeriodFrom?.DateTo, _payPeriodTo.DateTo)
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

    Private _payPeriodFrom As PayPeriod

    Private _payPeriodTo As PayPeriod

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
                               cboStringParameter)

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

    Private Async Sub PayrollSummaDateSelection_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"

        _currentlyWorkedOnPayPeriod = Await _payPeriodRepository.GetCurrentPayPeriodAsync(z_OrganizationID)

        DateFromLabel.Text = ""
        DateToLabel.Text = ""
        SemiMonthlyTab_Enter(TabControl1, New EventArgs)
    End Sub

    Sub VIEW_payp(Optional param_Date As Object = Nothing,
                  Optional PayFreqType As Object = Nothing)

        If param_Date Is Nothing Then
            param_Date = DBNull.Value
        Else
            param_Date = param_Date.ToString() & "-01-01"
        End If

        Dim params = New Object() {
            orgztnID,
            param_Date,
            "1",
            PayFreqType
        }

        dgvpayperiod.Rows.Clear()

        Dim sql As New SQL("CALL VIEW_payp(?og_rowid, ?param_date, ?isotherformat, ?payfreqtype);", params)
        Dim dt = sql.GetFoundRows.Tables(0)

        Dim index As Integer = 0
        Dim currentlyWorkedOnPayPeriodIndex As Integer = 0
        For Each drow As DataRow In dt.Rows

            If _currentlyWorkedOnPayPeriod IsNot Nothing AndAlso Nullable.Equals(drow(5), _currentlyWorkedOnPayPeriod.RowID) Then

                currentlyWorkedOnPayPeriodIndex = index

            End If

            Dim row_array = drow.ItemArray

            dgvpayperiod.Rows.Add(row_array)

            index += 1
        Next

        If currentlyWorkedOnPayPeriodIndex > dgvpayperiod.Rows.Count - 1 Then Return

        dgvpayperiod.Rows(currentlyWorkedOnPayPeriodIndex).Selected = True

        dgvpayperiod.Rows(currentlyWorkedOnPayPeriodIndex).Cells(Column6.Index).Selected = True
    End Sub

    Private Sub dgvpayperiod_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayperiod.CellContentClick
        dgvpayperiod.EndEdit()
        DateFromLabel.Text = String.Empty
        DateToLabel.Text = String.Empty

        If dgvpayperiod.RowCount = 0 Then
            Return
        End If

        Dim rowIdx = e.RowIndex
        Dim colName = dgvpayperiod.Columns(e.ColumnIndex).Name

        If Not colName = "Column1" Then
            Return
        End If

        Dim payPeriod = New PayPeriod() With {
            .RowID = ConvertToType(Of Integer?)(dgvpayperiod.Item("Column4", rowIdx).Value),
            .DateFrom = CDate(dgvpayperiod.Item("Column2", rowIdx).Value),
            .DateTo = CDate(dgvpayperiod.Item("Column3", rowIdx).Value)
        }

        Dim checkBox = dgvpayperiod.Item("Column1", rowIdx)
        If CBool(checkBox.Value) = False Then
            DeselectPayPeriod(payPeriod)
        Else
            SelectPayPeriod(payPeriod, rowIdx)
        End If

        DateFromLabel.Text = If(DateFrom Is Nothing, String.Empty, CDate(DateFrom).ToString("MMMM d, yyyy"))
        DateToLabel.Text = If(DateTo Is Nothing, String.Empty, CDate(DateTo).ToString("MMMM d, yyyy"))
    End Sub

    Private Sub DeselectPayPeriod(payPeriod As PayPeriod)
        If _payPeriodFrom?.Equals(payPeriod) Then
            _payPeriodFrom = Nothing
        ElseIf _payPeriodTo?.Equals(payPeriod) Then
            _payPeriodTo = Nothing
        End If
    End Sub

    Private Sub SelectPayPeriod(payPeriod As PayPeriod, rowIdx As Integer)
        If _payPeriodFrom Is Nothing Then
            _payPeriodFrom = payPeriod
        ElseIf _payPeriodTo Is Nothing Then
            _payPeriodTo = payPeriod

            If _payPeriodFrom.DateFrom > _payPeriodTo.DateFrom Then
                Dim swap = _payPeriodTo
                _payPeriodTo = _payPeriodFrom
                _payPeriodFrom = swap
            End If
        Else
            For Each row As DataGridViewRow In dgvpayperiod.Rows
                row.Cells("Column1").Value = False
            Next
            dgvpayperiod.Item("Column1", rowIdx).Value = True
            _payPeriodFrom = payPeriod

            _payPeriodTo = Nothing
        End If
    End Sub

    Private Sub dgvpayperiod_SelectionChanged(sender As Object, e As EventArgs) Handles dgvpayperiod.SelectionChanged
        If dgvpayperiod.RowCount <> 0 Then
            paypFrom = dgvpayperiod.CurrentRow.Cells("Column2").Value
            paypTo = dgvpayperiod.CurrentRow.Cells("Column3").Value

            Dim date_diff = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

            numofweekdays = 0

            For i = 0 To date_diff
                Dim DayOfWeek = CDate(paypFrom).AddDays(i)

                If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                    numofweekends += 1
                ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                    numofweekends += 1
                Else
                    numofweekdays += 1
                End If
            Next
        Else
            paypFrom = Nothing
            paypTo = Nothing
            numofweekends = 0
            numofweekdays = 0
        End If
    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        yearnow = yearnow + 1
        ChangePageLinks()
    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        yearnow = yearnow - 1
        ChangePageLinks()
    End Sub

    Private Sub ChangePageLinks()
        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"

        Dim sel_tab_pg = TabControl1.SelectedTab
        VIEW_payp(yearnow, sel_tab_pg.Text.Trim)

        dgvpayperiod.EndEdit()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        dgvpayperiod.EndEdit()

        If PayPeriodFromID Is Nothing Then
            WarnBalloon("Please select a pay period. ", "Invalid Pay Period", linkPrev, 30, -470)
            Exit Sub
        ElseIf cboStringParameter.Visible Then
            If cboStringParameter.Text = String.Empty Then
                WarnBalloon("Please select a " & TextBox1.Text, "Invalid " & TextBox1.Text, cboStringParameter, cboStringParameter.Width - 17, -69)
                Exit Sub
            End If
        End If

        If dgvpayperiod.RowCount <> 0 Then
            Me.DialogResult = DialogResult.OK
        Else
            Me.DialogResult = DialogResult.Cancel
        End If
    End Sub

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

    Private Sub SemiMonthlyTab_Enter(sender As Object, e As EventArgs) Handles SemiMonthlyTab.Enter
        VIEW_payp(, SemiMonthlyTab.Text.Trim)
    End Sub

    Private Class PayPeriod

        Public Property RowID As Integer?

        Public Property DateFrom As Date

        Public Property DateTo As Date

        Public Overrides Function Equals(obj As Object) As Boolean
            If TypeOf obj IsNot PayPeriod Then
                Return False
            End If

            Return CBool(DirectCast(obj, PayPeriod)?.RowID = RowID)
        End Function

    End Class

    Private Sub cboxLoanType_SelectedIndexChanged(sender As Object, e As EventArgs)
        _loanTypeId = CInt(cboxLoanType.SelectedValue)
    End Sub

End Class