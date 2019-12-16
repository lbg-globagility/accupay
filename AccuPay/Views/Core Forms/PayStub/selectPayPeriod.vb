Imports AccuPay.Data
Imports AccuPay.Entity
Imports AccuPay.Utils

Public Class selectPayPeriod

    Public Property PayPeriod As IPayPeriod
    Public Property GeneratePayroll As Boolean = True

    Private ReadOnly selectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Private ReadOnly unselectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim m_PayFreqType = ""

    Private _currentYear = Date.Now.Year

    Private _currentlyWorkedOnPayPeriod As IPayPeriod

    Private _payPeriodDataList As List(Of PayPeriodStatusData)

    Private Async Sub selectPayPeriod_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        linkPrev.Text = "← " & (_currentYear - 1)
        linkNxt.Text = (_currentYear + 1) & " →"

        _currentlyWorkedOnPayPeriod = Await PayrollTools.GetCurrentlyWorkedOnPayPeriodByCurrentYear()

        Dim payfrqncy As New AutoCompleteStringCollection

        Dim sel_query = ""

        Dim hasAnEmployee = EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE OrganizationID=" & orgztnID & " LIMIT 1);")

        If hasAnEmployee = 1 Then
            sel_query = "SELECT pp.PayFrequencyType FROM payfrequency pp INNER JOIN employee e ON e.PayFrequencyID=pp.RowID GROUP BY pp.RowID;"
        Else
            sel_query = "SELECT PayFrequencyType FROM payfrequency;"
        End If

        enlistTheLists(sel_query, payfrqncy)

        Dim first_sender As New ToolStripButton

        Dim indx = 0

        For Each strval In payfrqncy

            If strval.ToString.ToUpper = "WEEKLY" Then
                Continue For
            End If

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

    Dim quer_empPayFreq = ""

    Sub PayFreq_Changed(sender As Object, e As EventArgs)

        quer_empPayFreq = ""

        Dim senderObj As New ToolStripButton

        Static prevObj As New ToolStripButton

        Static once As SByte = 0

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

    Sub VIEW_payp(Optional param_Date As Object = Nothing,
                  Optional PayFreqType As Object = Nothing)
        Dim params = New Object() {
            orgztnID,
            If(param_Date = Nothing, DBNull.Value, param_Date & "-01-01"),
            0,
            PayFreqType
        }
        dgvpaypers.Rows.Clear()

        Dim sql As New SQL("CALL VIEW_payp(?og_rowid, ?param_date, ?isotherformat, ?payfreqtype);", params)
        Dim dt = sql.GetFoundRows.Tables(0)

        Dim payPeriodsWithPaystubCount = PayPeriodStatusData.GetPeriodsWithPaystubCount(PayFreqType)
        _payPeriodDataList = New List(Of PayPeriodStatusData)

        Dim index As Integer = 0
        Dim currentlyWorkedOnPayPeriodIndex As Integer = 0
        For Each drow As DataRow In dt.Rows

            If _currentlyWorkedOnPayPeriod IsNot Nothing AndAlso Nullable.Equals(drow(0), _currentlyWorkedOnPayPeriod.RowID) Then

                currentlyWorkedOnPayPeriodIndex = index

            End If

            Dim row_array = drow.ItemArray
            dgvpaypers.Rows.Add(row_array)

            'Because UpdatePayPeriodStatusLabel() will not execute fully on the first insert of row of dgvpaypers
            If index = 0 Then UpdatePayPeriodStatusLabel()

            Dim payPeriodData = CreatePayPeriodData(payPeriodsWithPaystubCount, index, drow)
            If payPeriodData IsNot Nothing Then
                _payPeriodDataList.Add(payPeriodData)
            End If

            index += 1

        Next

        If currentlyWorkedOnPayPeriodIndex > dgvpaypers.Rows.Count - 1 Then Return

        dgvpaypers.Rows(currentlyWorkedOnPayPeriodIndex).Selected = True

        dgvpaypers.Rows(currentlyWorkedOnPayPeriodIndex).Cells(Column15.Index).Selected = True

    End Sub

    Private Function CreatePayPeriodData(payPeriodsWithPaystubCount As List(Of PayPeriod), index As Integer, drow As DataRow) As PayPeriodStatusData
        Dim payPeriodData As New PayPeriodStatusData

        payPeriodData.Index = index
        payPeriodData.Status = PayPeriodStatusData.PayPeriodStatus.Open

        If drow("IsClosed") <> 0 Then
            'the payperiods here are closed
            dgvpaypers.Rows(index).DefaultCellStyle.ForeColor = Color.Black
            payPeriodData.Status = PayPeriodStatusData.PayPeriodStatus.Closed
        Else

            'check if this open payperiod is already modified
            If payPeriodsWithPaystubCount.Any(Function(p) p.RowID.Value = drow("RowID")) Then

                dgvpaypers.Rows(index).DefaultCellStyle.SelectionBackColor = Color.Green
                dgvpaypers.Rows(index).DefaultCellStyle.BackColor = Color.Yellow

                payPeriodData.Status = PayPeriodStatusData.PayPeriodStatus.Processing
            Else
                dgvpaypers.Rows(index).DefaultCellStyle.ForeColor = Color.Gray

            End If

        End If

        Return payPeriodData
    End Function

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

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        dgvpaypers.EndEdit()

        Dim id_value As Object = Nothing

        If dgvpaypers.RowCount <> 0 Then

            Dim currentIndex = dgvpaypers.CurrentRow.Index

            Dim currentPayPeriodData = _payPeriodDataList.FirstOrDefault(Function(p) p.Index = currentIndex)

            If currentPayPeriodData IsNot Nothing Then

            End If

            With dgvpaypers.CurrentRow

                If Await PayrollTools.
                    ValidatePayPeriodAction(ObjectUtils.ToNullableInteger(.Cells("Column1").Value)) = False Then Return

                id_value = dgvpaypers.Item("Column1", 0).Value

                PayStubForm.paypRowID = .Cells("Column1").Value
                PayStubForm.paypFrom = Format(CDate(.Cells("Column2").Value), "yyyy-MM-dd")
                PayStubForm.paypTo = Format(CDate(.Cells("Column3").Value), "yyyy-MM-dd")
                PayStubForm.isEndOfMonth = Trim(.Cells("Column14").Value)

                PayPeriod = New PayPeriod

                PayPeriod.RowID = .Cells("Column1").Value
                PayPeriod.PayFromDate = CDate(.Cells("Column2").Value)
                PayPeriod.PayToDate = CDate(.Cells("Column3").Value)
            End With

            Dim PayFreqRowID = EXECQUER("SELECT RowID FROM payfrequency WHERE PayFrequencyType='" & quer_empPayFreq & "';")

            If GeneratePayroll Then
                PayStubForm.genpayroll(PayFreqRowID)

            End If

        End If

        Me.Hide()
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Dim prior_value As Object = Nothing

    <Obsolete>
    Private Sub dgvpaypers_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvpaypers.CellBeginEdit
        prior_value = ValNoComma(dgvpaypers.Item("PayPeriodMinWageValue", e.RowIndex).Value)
    End Sub

    <Obsolete>
    Private Sub dgvpaypers_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpaypers.CellEndEdit
        If e.RowIndex > -1 And e.ColumnIndex > -1 Then

            Dim min_wage_val = ValNoComma(dgvpaypers.Item("PayPeriodMinWageValue", e.RowIndex).Value)

            If prior_value <> min_wage_val Then
                prior_value = ValNoComma(dgvpaypers.Item("PayPeriodMinWageValue", e.RowIndex).Value)

                Dim n_ExecuteQuery As _
                    New ExecuteQuery("UPDATE payperiod" &
                                     " SET" &
                                     " MinWageValue=" & min_wage_val & "" &
                                     ",LastUpd=CURRENT_TIMESTAMP()" &
                                     ",LastUpdBy='" & z_User & "'" &
                                     " WHERE RowID='" & dgvpaypers.Item("Column1", e.RowIndex).Value & "';")
            End If
        End If
    End Sub

    Private Sub dgvpaypers_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvpaypers.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            Button1_Click(sender, e)
        End If
    End Sub

    Private Sub dgvpaypers_SelectionChanged(sender As Object, e As EventArgs) Handles dgvpaypers.SelectionChanged
        If dgvpaypers.RowCount <> 0 Then
            Dim currentRow = dgvpaypers.CurrentRow

            Dim dateFrom = Trim(currentRow.Cells("Column2").Value)
            Dim dateTo = Trim(currentRow.Cells("Column3").Value)

            lblpapyperiodval.Text = ": from " & dateFrom & " to " & dateTo

            UpdatePayPeriodStatusLabel()
        Else
            lblpapyperiodval.Text = ""
            ResetPayPeriodStatusLabel()
        End If
    End Sub

    Private Sub UpdatePayPeriodStatusLabel()
        Dim currentPayPeriodData = _payPeriodDataList.
                                    FirstOrDefault(Function(p) p.Index = dgvpaypers.CurrentRow.Index)

        If currentPayPeriodData Is Nothing Then Return

        Select Case currentPayPeriodData.Status
            Case PayPeriodStatusData.PayPeriodStatus.Open
                PayPeriodStatusLabel.Text = "OPEN"
                PayPeriodStatusLabel.BackColor = Color.Blue
                PayPeriodStatusLabel.ForeColor = Color.White
            Case PayPeriodStatusData.PayPeriodStatus.Processing
                PayPeriodStatusLabel.Text = "PROCESSING"
                PayPeriodStatusLabel.BackColor = Color.Green
                PayPeriodStatusLabel.ForeColor = Color.White
            Case PayPeriodStatusData.PayPeriodStatus.Closed
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Dim n_link As New LinkLabel.Link

        If keyData = Keys.Escape And dgvpaypers.IsCurrentCellInEditMode = False Then

            Button2_Click(Button2, New EventArgs)

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

    Private Sub dgvpaypers_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvpaypers.RowsAdded
        Dim month_column_name As String = Column5.Name

        Dim is_even As Boolean = ((dgvpaypers.Item(month_column_name, e.RowIndex).Value Mod 2) = 0)

        Dim row_bg_color As Color

        If is_even Then
            row_bg_color = Color.LightGray
        Else
            row_bg_color = Color.FromArgb(255, 255, 255)
        End If

        dgvpaypers.Rows(e.RowIndex).DefaultCellStyle.BackColor = row_bg_color
    End Sub

End Class