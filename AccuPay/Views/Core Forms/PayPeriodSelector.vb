Imports AccuPay.DB

Public Class PayPeriodSelector

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL `VIEW_payp`('" & orgztnID & "', '1900-01-01', '0', 'SEMI-MONTHLY');")

        Dim catchdt As New DataTable

        catchdt = n_SQLQueryToDatatable.ResultTable

        Dim i = 0

        For Each dcol As DataColumn In catchdt.Columns

            With dgvPayPeriodList.Columns(i)
                .Name = dcol.ColumnName.Replace(" ", "_")
                .HeaderText = dcol.ColumnName
                .Visible = (dcol.ColumnName.Contains(Space(1)))

            End With

            i += 1

        Next

        catchdt.Dispose()

        Dim dgvVisibleRows = dgvPayPeriodList.Columns.Cast(Of DataGridViewColumn).Where(Function(ii) ii.Visible = True)

        Dim scrollbarwidth = 19

        Dim mincolwidth As Integer = (dgvPayPeriodList.Width - (dgvPayPeriodList.RowHeadersWidth + scrollbarwidth)) / dgvVisibleRows.Count

        For Each dgvcol In dgvVisibleRows
            dgvcol.Width = mincolwidth
            dgvcol.SortMode = DataGridViewColumnSortMode.NotSortable
        Next

    End Sub

    Dim n_PayPeriodRowID As Object

    Property PayPeriodRowID As Object
        Get
            Return n_PayPeriodRowID
        End Get
        Set(value As Object)
            n_PayPeriodRowID = value
        End Set
    End Property

    Dim isShowDialog As Boolean = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            isShowDialog = True

            .Text = someValue.Trim

        End With

        Return Me.ShowDialog

    End Function

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            btnCancel_Click(btnCancel, New EventArgs)

            Return True
        Else
            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Dim ServerTodayDate = New ExecuteQuery("SELECT CURDATE();").Result

    Private Sub PayPeriodSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        LoadPayPeriods(MYSQLDateFormat(CDate(ServerTodayDate)))

        linkNxt.Text = (CDate(ServerTodayDate).Year + Convert.ToInt32(linkNxt.Tag))
        linkPrevs.Text = (CDate(ServerTodayDate).Year + Convert.ToInt32(linkPrevs.Tag))

    End Sub

    Private Sub LoadPayPeriods(PayPeriodYear As Object)

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL `VIEW_payp`('" & orgztnID & "', '" & PayPeriodYear & "', '0', 'SEMI-MONTHLY');")

        Dim catchdt As New DataTable

        catchdt = n_SQLQueryToDatatable.ResultTable

        dgvPayPeriodList.Rows.Clear()

        For Each drow As DataRow In catchdt.Rows

            Dim row_array = drow.ItemArray

            dgvPayPeriodList.Rows.Add(row_array)

        Next

        dgvPayPeriodList_SelectionChanged(dgvPayPeriodList, New EventArgs)

        AddHandler dgvPayPeriodList.SelectionChanged, AddressOf dgvPayPeriodList_SelectionChanged

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        If dgvPayPeriodList.Tag = Nothing Then
            btnCancel_Click(btnCancel, New EventArgs)
        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

    End Sub

    Private Sub dgvPayPeriodList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPayPeriodList.CellContentClick

    End Sub

    Private Sub dgvPayPeriodList_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgvPayPeriodList.CurrentCellChanged

    End Sub

    Private Sub dgvPayPeriodList_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvPayPeriodList.RowsRemoved
        RemoveHandler dgvPayPeriodList.SelectionChanged, AddressOf dgvPayPeriodList_SelectionChanged
    End Sub

    Private Sub dgvPayPeriodList_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvPayPeriodList.SelectionChanged

        If dgvPayPeriodList.RowCount > 0 Then

            With dgvPayPeriodList
                n_PayPeriodRowID = .CurrentRow.Cells("RowID").Value

            End With
        Else
            n_PayPeriodRowID = Nothing
        End If

        dgvPayPeriodList.Tag = n_PayPeriodRowID

    End Sub

    Private Sub YearChanged(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles _
        linkNxt.LinkClicked,
        linkPrevs.LinkClicked

        Dim curr_lnklbl As New LinkLabel
        curr_lnklbl = DirectCast(sender, LinkLabel)

        Dim _year As Integer = Convert.ToInt32(curr_lnklbl.Text)

        linkNxt.Text =
            (Convert.ToInt32(linkNxt.Text) + Convert.ToInt32(curr_lnklbl.Tag))

        linkPrevs.Text =
            (Convert.ToInt32(linkPrevs.Text) + Convert.ToInt32(curr_lnklbl.Tag))

        LoadPayPeriods(String.Concat(_year, "-01-01"))

    End Sub

End Class