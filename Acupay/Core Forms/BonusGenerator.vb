Imports System.Threading

Public Class BonusGenerator

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        SplitContainer1.SplitterWidth = 6

        Dim listoftextbox = SplitContainer1.Panel2.Controls.OfType(Of TextBox)()

        For Each txtbx In listoftextbox
            AddHandler txtbx.TextChanged, AddressOf Amount_Formatter
        Next

        MyBase.OnLoad(e)

    End Sub

    Dim emp_page_limiter As Integer = 20

    Dim pagenumber As Object = 0

    Sub BonusGenerator()

    End Sub

    Dim Int_Year As Integer = New ExecuteQuery("SELECT YEAR(CURDATE());").Result

    Private Sub BonusGenerator_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        PayrollForm.listPayrollForm.Remove(Me.Name)

    End Sub

    Private Sub BonusGenerator_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'LoadPayPeriods(Int_Year)

        Paginator(lnklblFirst, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))

        'Navigate_PayPeriodYear(linkPrevs, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))

        lblYear_DoubleClick(lblYear, New EventArgs)

    End Sub

    Sub LoadPayPeriods(IntYear As Integer)

        RemoveHandler dgvPayPeriodList.SelectionChanged, AddressOf dgvPayPeriodList_SelectionChanged

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL VIEW_payperiodofyear('" & orgztnID & "','" & IntYear & "-01-01', 1);")

        Dim catchdt As New DataTable

        catchdt = n_SQLQueryToDatatable.ResultTable

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Dim i = 0

            For Each dcol As DataColumn In catchdt.Columns

                With dgvPayPeriodList.Columns(i)
                    .Name = dcol.ColumnName.Replace(" ", "_")
                    .HeaderText = dcol.ColumnName.Trim
                    .Visible = (dcol.ColumnName.Contains(Space(1)))

                End With

                i += 1

            Next

            Dim dgvVisibleRows = dgvPayPeriodList.Columns.Cast(Of DataGridViewColumn).Where(Function(ii) ii.Visible = True)

            Dim scrollbarwidth = 19

            Dim mincolwidth As Integer = (dgvPayPeriodList.Width - (dgvPayPeriodList.RowHeadersWidth + scrollbarwidth)) / dgvVisibleRows.Count

            For Each dgvcol In dgvVisibleRows
                dgvcol.Width = mincolwidth
                dgvcol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

        End If

        dgvPayPeriodList.Rows.Clear()

        For Each drow As DataRow In catchdt.Rows

            Dim row_array = drow.ItemArray

            dgvPayPeriodList.Rows.Add(row_array)

        Next

        dgvPayPeriodList_SelectionChanged(dgvPayPeriodList, New EventArgs)

        AddHandler dgvPayPeriodList.SelectionChanged, AddressOf dgvPayPeriodList_SelectionChanged

    End Sub

    Sub LoadEmployees(Optional Page_Number As Object = 0, Optional SearchString As String = "")

        RemoveHandler dgvEmployeeList.SelectionChanged, AddressOf dgvEmployeeList_SelectionChanged

        'Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL VIEW_employee_for_payroll('" & orgztnID & "','" & Page_Number & "','" & SearchString & "');")

        Dim n_SQLQueryToDatatable As New ReadSQLProcedureToDatatable("VIEW_employee_for_payroll", orgztnID, Page_Number, SearchString)

        Dim catchdt As New DataTable

        catchdt = n_SQLQueryToDatatable.ResultTable

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Dim i = 0

            For Each dcol As DataColumn In catchdt.Columns

                With dgvEmployeeList.Columns(i)
                    .Name = dcol.ColumnName.Replace(" ", "_")
                    .HeaderText = dcol.ColumnName.Trim
                    .Visible = (dcol.ColumnName.Contains(Space(1)))

                End With

                i += 1

            Next

        End If

        dgvEmployeeList.Rows.Clear()

        For Each drow As DataRow In catchdt.Rows

            Dim row_array = drow.ItemArray

            dgvEmployeeList.Rows.Add(row_array)

        Next

        MakeEmployeeSelect()

    End Sub

    Sub MakeEmployeeSelect()

        dgvEmployeeList_SelectionChanged(dgvEmployeeList, New EventArgs)

        AddHandler dgvEmployeeList.SelectionChanged, AddressOf dgvEmployeeList_SelectionChanged

    End Sub

    Private Sub Paginator(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblFirst.LinkClicked, lnklblPrev.LinkClicked,
                                                                                        lnklblNext.LinkClicked, lnklblLast.LinkClicked

        Dim sender_name = DirectCast(sender, LinkLabel).Name

        If sender_name = "lnklblFirst" Then
            pagenumber = 0
        ElseIf sender_name = "lnklblPrev" Then
            If (pagenumber - emp_page_limiter) < 0 Then
                pagenumber = 0
            Else
                pagenumber -= emp_page_limiter
            End If
        ElseIf sender_name = "lnklblNext" Then
            pagenumber += emp_page_limiter
        ElseIf sender_name = "lnklblLast" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 20 FROM employee WHERE OrganizationID=" & orgztnID & ";"))

            Dim remender = lastpage Mod 1

            pagenumber = (lastpage - remender) * 20

        End If

        LoadEmployees(pagenumber, tstxboxSearch.Text.Trim)

    End Sub

    Private Sub tsbtnSearch_Click(sender As Object, e As EventArgs) Handles tsbtnSearch.Click

        Paginator(lnklblFirst, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))

        'pagenumber = 0

        'LoadEmployees(pagenumber, tstxboxSearch.Text.Trim)

    End Sub

    Private Sub tstxboxSearch_Click(sender As Object, e As EventArgs) Handles tstxboxSearch.Click

    End Sub

    Private Sub tstxboxSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tstxboxSearch.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then
            tsbtnSearch_Click(tsbtnSearch, New EventArgs)
        End If

    End Sub

    Dim GeneratePayPeriodRowID As Object

    Dim GeneratePayFromDate As Object

    Dim GeneratePayToDate As Object

    Private Sub tsbtnBonusGen_Click(sender As Object, e As EventArgs) Handles tsbtnBonusGen.Click

        GeneratePayPeriodRowID = Nothing

        GeneratePayFromDate = Nothing

        GeneratePayToDate = Nothing

        Dim n_PayPeriodSelector As New PayPeriodSelector

        With n_PayPeriodSelector

            .FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog

            .MaximizeBox = False

            .MinimizeBox = False

            .StartPosition = FormStartPosition.CenterScreen

            If .ShowDialog() = Windows.Forms.DialogResult.OK Then

                GeneratePayPeriodRowID = .PayPeriodRowID

                Dim payperioddates As String = New ExecuteQuery("SELECT CONCAT_WS(',',pp.PayFromDate,pp.PayToDate) FROM payperiod pp WHERE pp.RowID='" & GeneratePayPeriodRowID & "';").Result

                Dim array_payperioddates = Split(payperioddates, ",")

                GeneratePayFromDate = array_payperioddates(array_payperioddates.GetLowerBound(0))

                GeneratePayToDate = array_payperioddates(array_payperioddates.GetUpperBound(0))

                If bgworkBonusPreparator.IsBusy = False _
                    And False = (GeneratePayPeriodRowID = Nothing) Then
                    tsProgressBar.Visible = True
                    bgworkBonusPreparator.RunWorkerAsync()
                End If

            End If

        End With

    End Sub

    Private Sub tsbntClose_Click(sender As Object, e As EventArgs) Handles tsbntClose.Click

        Me.Close()

    End Sub

    Dim validEmployeeList As New DataTable

    Private Sub bgworkBonusPreparator_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkBonusPreparator.DoWork

        'GeneratePayPeriodRowID, GeneratePayFromDate, GeneratePayToDate

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL EMPLOYEE_payrollgen('" & orgztnID & "', '" & GeneratePayFromDate & "', '" & GeneratePayToDate & "', NULL);")

        validEmployeeList = n_SQLQueryToDatatable.ResultTable

    End Sub

    Private Sub bgworkBonusPreparator_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkBonusPreparator.ProgressChanged

    End Sub

    Private Sub bgworkBonusPreparator_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkBonusPreparator.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox(getErrExcptn(e.Error, Me.Name))
        ElseIf e.Cancelled Then
            MsgBox("User aborts the preparation", MsgBoxStyle.Information, "")
        Else

            If bgworkBonusGenerate.IsBusy = False Then
                bgworkBonusGenerate.RunWorkerAsync()
            End If

        End If

    End Sub

    Private Sub bgworkBonusGenerate_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkBonusGenerate.DoWork
        'Create a variable for start time:
        'Dim TimerStart As DateTime
        'TimerStart = Now

        'computing starts here
        If e.Cancel = False Then

            Dim datacount = validEmployeeList.Rows.Count

            Dim i = 1

            For Each erow As DataRow In validEmployeeList.Rows

                'Thread.Sleep(1000)

                Dim n_ReadSQLFunction As _
                    New ReadSQLFunction("INSUPD_paystubbonus",
                                            "returnvalue",
                                        orgztnID,
                                        erow("RowID"),
                                        z_User,
                                        GeneratePayPeriodRowID,
                                        GeneratePayFromDate,
                                        GeneratePayToDate,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0)

                bgworkBonusGenerate.ReportProgress((i / datacount) * 100)

                i += 1

            Next

            'Dim TimeSpent As System.TimeSpan
            'TimeSpent = Now.Subtract(TimerStart)
            'MsgBox(TimeSpent.TotalSeconds & " seconds spent on this task")' TotalSeconds is a double data type

        End If

    End Sub

    Private Sub bgworkBonusGenerate_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkBonusGenerate.ProgressChanged

        tsProgressBar.Value = CType(e.ProgressPercentage, Integer)

        'tsProgressBar.Visible = (tsProgressBar.Value > 0)

    End Sub

    Private Sub bgworkBonusGenerate_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkBonusGenerate.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox(getErrExcptn(e.Error, Me.Name))
        ElseIf e.Cancelled Then
            MsgBox("User aborts the process", MsgBoxStyle.Information, "")
        Else

            MsgBox("Done generating", MsgBoxStyle.Information, "")

            lblYear_DoubleClick(lblYear, New EventArgs)

        End If

        tsProgressBar.Visible = False

    End Sub

    Private Sub tsProgressBar_Click(sender As Object, e As EventArgs) Handles tsProgressBar.Click

    End Sub

    Private Sub tsProgressBar_VisibleChanged(sender As Object, e As EventArgs) Handles tsProgressBar.VisibleChanged
        If tsProgressBar.Visible = False Then
            tsProgressBar.Value = 0
        End If

        Me.Enabled = (Not tsProgressBar.Visible)

    End Sub

    Private Sub dgvPayPeriodList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPayPeriodList.CellContentClick



    End Sub

    Private Sub dgvPayPeriodList_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvPayPeriodList.RowsRemoved

    End Sub

    Dim bonPayFromDate As Object = Nothing

    Dim bonPayToDate As Object = Nothing

    Private Sub dgvPayPeriodList_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvPayPeriodList.SelectionChanged

        dgvPayPeriodList.Tag = Nothing

        bonPayFromDate = Nothing

        bonPayToDate = Nothing

        If dgvPayPeriodList.RowCount > 0 Then
            'MsgBox("dgvPayPeriodList_SelectionChanged")
            With dgvPayPeriodList.CurrentRow

                dgvPayPeriodList.Tag = .Cells("ppRowID").Value
                
                bonPayFromDate = dgvPayPeriodList.Item(1, .Index).Value

                bonPayToDate = dgvPayPeriodList.Item(2, .Index).Value

            End With
        Else

        End If

        MakeEmployeeSelect()

    End Sub

    Private Sub dgvEmployeeList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmployeeList.CellContentClick

    End Sub

    Private Sub dgvEmployeeList_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvEmployeeList.RowsRemoved

    End Sub

    Private Sub dgvEmployeeList_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvEmployeeList.SelectionChanged

        ClearTextBox()

        If dgvEmployeeList.RowCount > 0 _
            And False = (dgvPayPeriodList.Tag = Nothing) Then

            With dgvEmployeeList.CurrentRow

                LOAD_paystubbonus(.Cells("RowID").Value,
                                  dgvPayPeriodList.Tag)

            End With
            'MsgBox("dgvEmployeeList_SelectionChanged")

        Else

        End If

    End Sub

    Dim PayStubBonusRowID As Object = Nothing

    Private Sub LOAD_paystubbonus(EmployeeRowID As Object,
                                  PayPeriod_RowID As Object)

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL VIEW_paystubbonus('" & orgztnID & "','" & EmployeeRowID & "','" & PayPeriod_RowID & "');")

        Dim catchdt As New DataTable

        catchdt = n_SQLQueryToDatatable.ResultTable

        For Each drow As DataRow In catchdt.Rows

            PayStubBonusRowID = drow("RowID")

            txtTotalLoan.Text = ValNoComma(drow("TotalLoans"))
            txtTotalBonus.Text = ValNoComma(drow("TotalNetSalary"))

        Next

        catchdt.Dispose()

    End Sub

    Private Sub ClearTextBox()

        PayStubBonusRowID = Nothing

        Dim listoftextbox = SplitContainer1.Panel2.Controls.OfType(Of TextBox)()

        For Each ctrl In listoftextbox
            ctrl.Text = 0
        Next

    End Sub

    Private Sub Navigate_PayPeriodYear(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrevs.LinkClicked, linkNxt.LinkClicked

        Dim sender_obj = DirectCast(sender, LinkLabel)

        Dim sender_name As String = sender_obj.Name

        Static Int_Years As Integer = (Int_Year + 1)
        If IsReturnToCurrentYear Then
            Int_Years = (Int_Year + 1)
        End If
        If sender_name = "linkPrevs" Then
            Int_Years -= 1
            IsReturnToCurrentYear = False
            Static once As SByte = 0
            If once = 0 Then
                once = 1
                Navigate_PayPeriodYear(linkPrevs, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))
                Exit Sub
            End If
        ElseIf sender_name = "linkNxt" Then
            Int_Years += 1
        End If

        'End If

        lblYear.Text = Int_Years

        linkPrevs.Text = "← " & (Int_Years - 1)

        linkNxt.Text = (Int_Years + 1) & " →"

        LoadPayPeriods(Int_Years)

    End Sub

    Private Sub Amount_Formatter(sender As Object, e As EventArgs) 'Handles txtTotalBonus.TextChanged

        Dim sender_obj = DirectCast(sender, TextBox)

        sender_obj.Text = FormatNumber(ValNoComma(sender_obj.Text), 2)

    End Sub

    Private Sub lblYear_Click(sender As Object, e As EventArgs) Handles lblYear.Click

    End Sub

    Dim IsReturnToCurrentYear As Boolean = False

    Private Sub lblYear_DoubleClick(sender As Object, e As EventArgs) Handles lblYear.DoubleClick

        LoadPayPeriods(Int_Year)

        lblYear.Text = Int_Year

        linkPrevs.Text = "← " & (Int_Year - 1)

        linkNxt.Text = (Int_Year + 1) & " →"

        'Navigate_PayPeriodYear(linkPrevs, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))

    End Sub

    Private Sub tsbtnDelEmpPayrollBonus_Click(sender As Object, e As EventArgs) Handles tsbtnDelEmpPayrollBonus.Click

        Dim hasNoRowSelected As Boolean = False

        Try

            hasNoRowSelected = (dgvEmployeeList.CurrentRow Is Nothing) _
                                Or (dgvPayPeriodList.Tag = Nothing)

        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))

        Finally

            If hasNoRowSelected = False Then

                'If dgvEmployeeList.RowCount > 0 Then

                '    If dgvEmployeeList.CurrentRow Is Nothing Then

                '    Else

                Dim prompt = MessageBox.Show("Are you sure you want to delete the computed bonus" & vbNewLine &
                                             "for the period of '" & bonPayFromDate & "' and '" & bonPayToDate & "' ?",
                                             "Delete the selected bonus",
                                             MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                If prompt = Windows.Forms.DialogResult.Yes Then

                    Dim dgvcurrRow = dgvEmployeeList.CurrentRow

                    Dim PayStubRow_ID = New ExecuteQuery("SELECT RowID FROM paystubbonus WHERE EmployeeID='" & dgvcurrRow.Cells("RowID").Value & "' AND OrganizationID='" & orgztnID & "' AND PayPeriodID='" & dgvPayPeriodList.Tag & "' LIMIT 1;").Result
                    PayStubRow_ID = ValNoComma(PayStubRow_ID)
                    Dim n_ExecuteQuery As _
                        New ExecuteQuery("CALL DEL_specificpaystubbonus('" & PayStubRow_ID & "');")

                    'dgvEmployeeList.Rows.Remove(dgvcurrRow)
                    Paginator(lnklblFirst, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))

                End If

                '    End If

                'End If

            End If

        End Try

    End Sub

    Private Sub lblYear_TextChanged(sender As Object, e As EventArgs) Handles lblYear.TextChanged
        'Static once As SByte = 0
        'If once = 0 Then
        '    once = 1
        'Else
        IsReturnToCurrentYear = (ValNoComma(lblYear.Text.Trim) = Int_Year)
        'End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class