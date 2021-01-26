Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class selectPayPeriod

    Public Property PayPeriod As PayPeriod

    Private ReadOnly selectedButtonFont As New Font("Trebuchet MS", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType(0, Byte))

    Private ReadOnly unselectedButtonFont As New Font("Trebuchet MS", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))

    Dim m_PayFreqType As String = ""

    Private _currentYear As Integer = Date.Now.Year

    Private _currentlyWorkedOnPayPeriod As IPayPeriod

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Sub New()

        InitializeComponent()

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)
    End Sub

    Private Async Sub selectPayPeriod_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        linkPrev.Text = "← " & (_currentYear - 1)
        linkNxt.Text = (_currentYear + 1) & " →"

        _currentlyWorkedOnPayPeriod = Await _payPeriodRepository.GetCurrentPayPeriodAsync(
            organizationId:=z_OrganizationID,
            currentUserId:=z_User)

        Dim payfrqncy As New AutoCompleteStringCollection

        Dim sel_query = ""

        Dim hasAnEmployee = CInt(EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE OrganizationID=" & orgztnID & " LIMIT 1);"))

        If hasAnEmployee = 1 Then
            sel_query = "SELECT pp.PayFrequencyType FROM payfrequency pp INNER JOIN employee e ON e.PayFrequencyID=pp.RowID GROUP BY pp.RowID;"
        Else
            sel_query = "SELECT PayFrequencyType FROM payfrequency;"
        End If

        enlistTheLists(sel_query, payfrqncy)

        Dim first_sender As New ToolStripButton

        Dim indx = 0

        For Each strval As String In payfrqncy

            If strval.ToString.ToUpper = "WEEKLY" Then
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

            If m_PayFreqType = "" Then
                If indx = 0 Then
                    indx = 1
                    first_sender = new_tsbtn
                End If
            Else
                If m_PayFreqType = new_tsbtn.Text Then
                    first_sender = new_tsbtn
                End If
            End If

            AddHandler new_tsbtn.Click, AddressOf PayFreq_Changed

            new_tsbtn = Nothing

        Next

        tstrip.PerformLayout()

        If first_sender IsNot Nothing Then
            PayFreq_Changed(first_sender, New EventArgs)
        End If
    End Sub

    Dim quer_empPayFreq As String = ""

    Sub PayFreq_Changed(sender As Object, e As EventArgs)

        quer_empPayFreq = ""

        Dim senderObj As New ToolStripButton

        Dim prevObj As New ToolStripButton

        Dim once As SByte = 0

        senderObj = DirectCast(sender, ToolStripButton)

        If once = 0 Then

            once = 1

            prevObj = senderObj

            senderObj.BackColor = Color.FromArgb(194, 228, 255)

            senderObj.Font = selectedButtonFont

            quer_empPayFreq = senderObj.Text

            VIEW_payp(Nothing, senderObj.Text)

            dgvpaypers_SelectionChanged(sender, e)

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

        quer_empPayFreq = senderObj.Text

        VIEW_payp(Nothing, senderObj.Text)

        dgvpaypers_SelectionChanged(sender, e)

    End Sub

    Sub VIEW_payp(Optional year As Integer? = Nothing,
                  Optional PayFreqType As Object = Nothing)

        Dim paramDate As Object

        If year Is Nothing Then
            paramDate = DBNull.Value
        Else
            paramDate = year.ToString() & "-01-01"
        End If

        Dim params = New Object() {
            orgztnID,
            paramDate,
            0,
            PayFreqType
        }
        dgvpaypers.Rows.Clear()

        Dim sql As New SQL("CALL VIEW_payp(?og_rowid, ?param_date, ?isotherformat, ?payfreqtype);", params)
        Dim dt = sql.GetFoundRows.Tables(0)

        Dim index As Integer = 0
        Dim currentlyWorkedOnPayPeriodIndex As Integer = 0
        For Each drow As DataRow In dt.Rows

            If _currentlyWorkedOnPayPeriod IsNot Nothing AndAlso Nullable.Equals(drow(0), _currentlyWorkedOnPayPeriod?.RowID) Then

                currentlyWorkedOnPayPeriodIndex = index

            End If

            Dim row_array = drow.ItemArray
            dgvpaypers.Rows.Add(row_array)

            'Because UpdatePayPeriodStatusLabel() will not execute fully on the first insert of row of dgvpaypers
            If index = 0 Then UpdatePayPeriodStatusLabel(dgvpaypers.Rows(0))

            HighlightOpenPayPeriod(index, drow)

            index += 1

        Next

        If currentlyWorkedOnPayPeriodIndex > dgvpaypers.Rows.Count - 1 Then Return

        dgvpaypers.Rows(currentlyWorkedOnPayPeriodIndex).Selected = True

        dgvpaypers.Rows(currentlyWorkedOnPayPeriodIndex).Cells(Column15.Index).Selected = True

    End Sub

    Private Sub HighlightOpenPayPeriod(index As Integer, drow As DataRow)

        'TODO: URGENT, this form looks like it is still using the old way to determine
        'the closed and open pay period. Inspect this.

        Dim status = Enums(Of PayPeriodStatus).Parse(drow("Status").ToString())

        If status = PayPeriodStatus.Closed Then
            dgvpaypers.Rows(index).DefaultCellStyle.ForeColor = Color.Black

        ElseIf status = PayPeriodStatus.Open Then

            dgvpaypers.Rows(index).DefaultCellStyle.SelectionBackColor = Color.Green
            dgvpaypers.Rows(index).DefaultCellStyle.BackColor = Color.Yellow
        Else
            dgvpaypers.Rows(index).DefaultCellStyle.ForeColor = Color.Gray

        End If

    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        _currentYear += 1
        UpdatePages()
    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        _currentYear -= 1
        UpdatePages()
    End Sub

    Private Sub UpdatePages()
        Panel2.Enabled = False
        linkNxt.Text = (_currentYear + 1) & " →"
        linkPrev.Text = "← " & (_currentYear - 1)

        VIEW_payp(_currentYear, quer_empPayFreq)
        If dgvpaypers.RowCount <> 0 Then
            dgvpaypers_SelectionChanged(Nothing, Nothing)
        Else
            lblpapyperiodval.Text = ""
            ResetPayPeriodStatusLabel()
        End If
        Panel2.Enabled = True
    End Sub

    Private Async Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        dgvpaypers.EndEdit()

        If dgvpaypers.RowCount <> 0 Then

            Dim currentIndex = dgvpaypers.CurrentRow.Index

            With dgvpaypers.CurrentRow

                Dim payPeriodId = ObjectUtils.ToNullableInteger(.Cells("Column1").Value)

                Dim payPeriodService = MainServiceProvider.GetRequiredService(Of IPayPeriodDataService)
                Dim validate = Await payPeriodService.ValidatePayPeriodActionAsync(payPeriodId)

                If validate = FunctionResult.Failed Then

                    MessageBoxHelper.Warning(validate.Message)
                    Return
                End If

                PayPeriod = Await _payPeriodRepository.GetByIdAsync(payPeriodId.Value)
            End With

        End If

        DialogResult = DialogResult.OK
    End Sub

    Private Sub dgvpaypers_SelectionChanged(sender As Object, e As EventArgs) Handles dgvpaypers.SelectionChanged
        If dgvpaypers.RowCount <> 0 Then
            Dim currentRow = dgvpaypers.CurrentRow

            Dim dateFrom = Trim(currentRow.Cells("Column2").Value.ToString())
            Dim dateTo = Trim(currentRow.Cells("Column3").Value.ToString())

            lblpapyperiodval.Text = ": from " & dateFrom & " to " & dateTo

            UpdatePayPeriodStatusLabel(currentRow)
        Else
            lblpapyperiodval.Text = ""
            ResetPayPeriodStatusLabel()
        End If
    End Sub

    Private Sub UpdatePayPeriodStatusLabel(currentRow As DataGridViewRow)

        If currentRow Is Nothing Then Return

        Dim statusColumn = currentRow.Cells("StatusColumn").Value.ToString()
        If statusColumn Is Nothing Then Return

        Dim status = Enums(Of PayPeriodStatus).Parse(statusColumn)

        Select Case status
            Case PayPeriodStatus.Pending
                PayPeriodStatusLabel.Text = "PENDING"
                PayPeriodStatusLabel.BackColor = Color.Blue
                PayPeriodStatusLabel.ForeColor = Color.White
            Case PayPeriodStatus.Open
                PayPeriodStatusLabel.Text = "OPEN"
                PayPeriodStatusLabel.BackColor = Color.Green
                PayPeriodStatusLabel.ForeColor = Color.White
            Case PayPeriodStatus.Closed
                PayPeriodStatusLabel.Text = "CLOSED"
                PayPeriodStatusLabel.BackColor = Color.Gray
                PayPeriodStatusLabel.ForeColor = Color.Black
        End Select
    End Sub

    Private Sub ResetPayPeriodStatusLabel()
        PayPeriodStatusLabel.ForeColor = Color.Black
        PayPeriodStatusLabel.BackColor = Color.White
        PayPeriodStatusLabel.ResetText()
    End Sub

    Private Sub dgvpaypers_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvpaypers.RowsAdded
        Dim month_column_name As String = Column5.Name

        Dim is_even As Boolean = ((CInt(dgvpaypers.Item(month_column_name, e.RowIndex).Value) Mod 2) = 0)

        Dim row_bg_color As Color

        If is_even Then
            row_bg_color = Color.LightGray
        Else
            row_bg_color = Color.FromArgb(255, 255, 255)
        End If

        dgvpaypers.Rows(e.RowIndex).DefaultCellStyle.BackColor = row_bg_color
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Dim n_link As New LinkLabel.Link

        If keyData = Keys.Enter Then

            OkButton.PerformClick()

            Return True

        ElseIf keyData = Keys.Escape And dgvpaypers.IsCurrentCellInEditMode = False Then

            CloseButton.PerformClick()

            Return True

        ElseIf keyData = Keys.Left Then

            n_link.Name = "linkPrev"

            linkPrev_LinkClicked(linkPrev, New LinkLabelLinkClickedEventArgs(n_link))

            Return True

        ElseIf keyData = Keys.Right Then

            n_link.Name = "linkNxt"

            linkNxt_LinkClicked(linkNxt, New LinkLabelLinkClickedEventArgs(n_link))

            Return True
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If
    End Function

End Class
