Imports System.Threading.Tasks
Imports System.Threading

Public Class selectPayPeriod

    Dim m_PayFreqType = ""

    Property PayFreqType As String
        Get
            Return m_PayFreqType
        End Get
        Set(value As String)
            m_PayFreqType = value
        End Set
    End Property


    Public PriorPayPeriodID As String = String.Empty

    Public CurrentPayPeriodID As String = String.Empty

    Public NextPayPeriodID As String = String.Empty


    Dim yearnow = CDate(dbnow).Year

    Dim selectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Private Sub selectPayPeriod_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Dim orgpayfreqID = Nothing

    Private Sub selectPayPeriod_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"



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



        orgpayfreqID = EXECQUER("SELECT PayFrequencyID FROM organization WHERE RowID='" & orgztnID & "';")

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

            VIEW_payp(, senderObj.Text)

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

        VIEW_payp(, senderObj.Text)

        dgvpaypers_SelectionChanged(sender, e)

    End Sub

    Sub VIEW_payp(Optional param_Date As Object = Nothing, _
                  Optional PayFreqType As Object = Nothing)

        Dim params(3, 2) As Object

        params(0, 0) = "payp_OrganizationID"
        params(1, 0) = "param_Date"
        params(2, 0) = "isotherformat"
        params(3, 0) = "PayFreqType"

        params(0, 1) = orgztnID
        params(1, 1) = If(param_Date = Nothing, DBNull.Value, param_Date & "-01-01")
        params(2, 1) = "0"
        params(3, 1) = PayFreqType

        EXEC_VIEW_PROCEDURE(params, _
                            "VIEW_payp", _
                            dgvpaypers)

    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        Panel2.Enabled = False
        yearnow = yearnow + 1

        linkNxt.Text = (yearnow + 1) & " →"
        linkPrev.Text = "← " & (yearnow - 1)

        VIEW_payp(yearnow, quer_empPayFreq)

        If dgvpaypers.RowCount <> 0 Then
            dgvpaypers_SelectionChanged(sender, e)
        Else
            lblpapyperiodval.Text = ""
        End If
        Panel2.Enabled = True
    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        Panel2.Enabled = False
        yearnow = yearnow - 1

        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"

        VIEW_payp(yearnow, quer_empPayFreq)

        If dgvpaypers.RowCount <> 0 Then
            dgvpaypers_SelectionChanged(sender, e)
        Else
            lblpapyperiodval.Text = ""
        End If
        Panel2.Enabled = True

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        dgvpaypers.EndEdit()

        CurrentPayPeriodID = String.Empty

        PriorPayPeriodID = String.Empty

        NextPayPeriodID = String.Empty

        Dim id_value As Object = Nothing

        If dgvpaypers.RowCount <> 0 Then

            With dgvpaypers.CurrentRow

                id_value = dgvpaypers.Item("Column1", 0).Value

                PayStub.paypRowID = .Cells("Column1").Value

                CurrentPayPeriodID = .Cells("Column1").Value

                PayStub.Current_PayPeriodID = CurrentPayPeriodID

                Dim prior_index = .Index - 1

                If prior_index < 0 Then

                    PriorPayPeriodID = EXECQUER("SELECT pyp.RowID" & _
                                                " FROM payperiod pyp" & _
                                                " INNER JOIN payperiod pp ON pp.RowID='" & CurrentPayPeriodID & "'" & _
                                                " WHERE pyp.OrganizationID='" & orgztnID & "'" & _
                                                " AND pyp.TotalGrossSalary=pp.TotalGrossSalary" & _
                                                " AND pyp.RowID BETWEEN (" & CurrentPayPeriodID & " - 10)" & _
                                                " AND pp.RowID" & _
                                                " ORDER BY pyp.PayFromDate DESC,pyp.PayToDate DESC" & _
                                                " LIMIT 1,1;")

                Else
                    PriorPayPeriodID = dgvpaypers.Item("Column1", prior_index).Value

                End If

                PayStub.Prior_PayPeriodID = PriorPayPeriodID


                PayStub.paypSSSContribSched = dgvpaypers.Item("SSSContribSched", .Index).Value

                PayStub.paypPhHContribSched = dgvpaypers.Item("PhHContribSched", .Index).Value

                PayStub.paypHDMFContribSched = dgvpaypers.Item("HDMFContribSched", .Index).Value




                Dim next_index = .Index + 1

                If next_index < dgvpaypers.RowCount Then
                    NextPayPeriodID = dgvpaypers.Item("Column1", next_index).Value
                Else

                    NextPayPeriodID = EXECQUER("SELECT pyp.RowID" & _
                                                " FROM payperiod pyp" & _
                                                " INNER JOIN payperiod pp ON pp.RowID='" & CurrentPayPeriodID & "'" & _
                                                " WHERE pyp.OrganizationID='" & orgztnID & "'" & _
                                                " AND pyp.TotalGrossSalary=pp.TotalGrossSalary" & _
                                                " AND pyp.RowID BETWEEN pp.RowID" & _
                                                " AND (" & CurrentPayPeriodID & " + 10)" & _
                                                " ORDER BY pyp.PayFromDate,pyp.PayToDate" & _
                                                " LIMIT 1,1;")

                End If

                PayStub.Next_PayPeriodID = NextPayPeriodID

                'NextPayPeriodID


                PayStub.paypFrom = Format(CDate(.Cells("Column2").Value), "yyyy-MM-dd")

                PayStub.paypTo = Format(CDate(.Cells("Column3").Value), "yyyy-MM-dd")


                'Dim sel_yearDateFrom = CDate(PayStub.paypFrom).Year

                'Dim sel_yearDateTo = CDate(PayStub.paypTo).Year

                'Dim sel_year = If(sel_yearDateFrom > sel_yearDateTo, _
                '                  sel_yearDateFrom, _
                '                  sel_yearDateTo)


                PayStub.isEndOfMonth = Trim(.Cells("Column14").Value)

                PayStub.genpayselyear = Format(CDate(.Cells("Column2").Value), "yyyy")

                PayStub.numofweekdays = 0

                PayStub.numofweekends = 0

                Dim date_diff = DateDiff(DateInterval.Day, CDate(PayStub.paypFrom), CDate(PayStub.paypTo))

                For i = 0 To date_diff

                    Dim DayOfWeek = CDate(PayStub.paypFrom).AddDays(i)

                    If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                        PayStub.numofweekends += 1

                    ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                        PayStub.numofweekends += 1

                    Else
                        PayStub.numofweekdays += 1

                    End If

                Next

                PayStub.withthirteenthmonthpay = 0

                If CheckBox1.Checked Then 'Format(CDate(.Cells("Column3").Value), "MM") = "12"

                    Dim prompt = MessageBox.Show("Do you want to include the calculation of Thirteenth month pay ?", _
                                                 "Thirteenth month pay calculation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information)

                    If prompt = Windows.Forms.DialogResult.Yes Then

                        PayStub.withthirteenthmonthpay = 1

                    ElseIf prompt = Windows.Forms.DialogResult.No Then

                    ElseIf prompt = Windows.Forms.DialogResult.Cancel Then
                        Exit Sub
                    End If

                    'Else

                End If

            End With



            PayStub.VeryFirstPayPeriodIDOfThisYear = id_value

            Dim PayFreqRowID = EXECQUER("SELECT RowID FROM payfrequency WHERE PayFrequencyType='" & quer_empPayFreq & "';")
            
            PayStub.genpayroll(PayFreqRowID)

        End If

        Me.Hide()

        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Me.Close()

    End Sub

    Dim prior_value As Object = Nothing

    Private Sub dgvpaypers_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvpaypers.CellBeginEdit

        prior_value = ValNoComma(dgvpaypers.Item("PayPeriodMinWageValue", e.RowIndex).Value)

    End Sub

    Private Sub dgvpaypers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpaypers.CellContentClick

    End Sub

    Private Sub dgvpaypers_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpaypers.CellEndEdit

        If e.RowIndex > -1 _
            And e.ColumnIndex > -1 Then

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

        Else

        End If

    End Sub

    Private Sub dgvpaypers_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvpaypers.KeyDown

        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = 1
            Button1_Click(sender, e)
        End If

    End Sub

    Dim numofweekdays = 0

    Dim numofweekends = 0

    Dim paypFrom = Nothing

    Dim paypTo = Nothing

    Private Sub dgvpaypers_SelectionChanged(sender As Object, e As EventArgs) Handles dgvpaypers.SelectionChanged

        Static _year As Integer = 0

        If dgvpaypers.RowCount <> 0 Then

            With dgvpaypers.CurrentRow
                lblpapyperiodval.Text = ": from " & _
                    Trim(.Cells("Column2").Value) & _
                    " to " & _
                    Trim(.Cells("Column3").Value)

                'Column1
                'Column2
                'Column3

                paypFrom = Format(CDate(.Cells("Column2").Value), "yyyy-MM-dd")

                paypTo = Format(CDate(.Cells("Column3").Value), "yyyy-MM-dd")

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

                If _year <> yearnow Then

                    _year = yearnow

                    Dim n_ExecuteQuery As _
                            New ExecuteQuery("SELECT EXISTS(SELECT RowID" & _
                                            " FROM paystub" & _
                                            " WHERE OrganizationID='" & orgztnID & "'" & _
                                            " AND ThirteenthMonthInclusion='1'" & _
                                            " AND (YEAR(PayFromDate)='" & yearnow & "' OR YEAR(PayToDate)='" & yearnow & "')" & _
                                            " LIMIT 1);")

                    Dim bool_decision = Not Convert.ToBoolean(CInt(n_ExecuteQuery.Result))

                    'If n_ExecuteQuery.Result = "1" Then

                    '    bool_decision = Convert.ToBoolean(CInt(n_ExecuteQuery.Result))

                    'Else

                    '    bool_decision = Convert.ToBoolean(CInt(n_ExecuteQuery.Result))

                    'End If

                    CheckBox1.Checked = False

                    CheckBox1.Enabled = bool_decision

                End If

            End With
        Else

            _year = 0

            paypFrom = Nothing

            paypTo = Nothing

            numofweekends = 0

            numofweekdays = 0

            lblpapyperiodval.Text = ""

        End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        Dim n_link As New System.Windows.Forms.LinkLabel.Link

        If keyData = Keys.Escape _
            And dgvpaypers.IsCurrentCellInEditMode = False Then

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

    Private Sub Other_OKFunction(sender As Object, e As EventArgs)

        CurrentPayPeriodID = String.Empty

        PriorPayPeriodID = String.Empty

        NextPayPeriodID = String.Empty

        If dgvpaypers.RowCount <> 0 Then

            With dgvpaypers.CurrentRow

                'PayStub.paypRowID = .Cells("Column1").Value

                CurrentPayPeriodID = .Cells("Column1").Value

                'PayStub.Current_PayPeriodID = CurrentPayPeriodID

                Dim prior_index = .Index - 1

                If prior_index < 0 Then

                    PriorPayPeriodID = EXECQUER("SELECT pyp.RowID" & _
                                                " FROM payperiod pyp" & _
                                                " INNER JOIN payperiod pp ON pp.RowID='" & CurrentPayPeriodID & "'" & _
                                                " WHERE pyp.OrganizationID='" & orgztnID & "'" & _
                                                " AND pyp.TotalGrossSalary=pp.TotalGrossSalary" & _
                                                " AND pyp.RowID BETWEEN (" & CurrentPayPeriodID & " - 10)" & _
                                                " AND pp.RowID" & _
                                                " ORDER BY pyp.PayFromDate DESC,pyp.PayToDate DESC" & _
                                                " LIMIT 1,1;")

                Else
                    PriorPayPeriodID = dgvpaypers.Item("Column1", prior_index).Value

                End If

                'PayStub.Prior_PayPeriodID = PriorPayPeriodID


                'PriorPayPeriodID



                Dim next_index = .Index + 1

                If next_index < dgvpaypers.RowCount Then
                    NextPayPeriodID = dgvpaypers.Item("Column1", next_index).Value
                Else

                    NextPayPeriodID = EXECQUER("SELECT pyp.RowID" & _
                                                " FROM payperiod pyp" & _
                                                " INNER JOIN payperiod pp ON pp.RowID='" & CurrentPayPeriodID & "'" & _
                                                " WHERE pyp.OrganizationID='" & orgztnID & "'" & _
                                                " AND pyp.TotalGrossSalary=pp.TotalGrossSalary" & _
                                                " AND pyp.RowID BETWEEN pp.RowID" & _
                                                " AND (" & CurrentPayPeriodID & " + 10)" & _
                                                " ORDER BY pyp.PayFromDate,pyp.PayToDate" & _
                                                " LIMIT 1,1;")

                End If

                'PayStub.Next_PayPeriodID = NextPayPeriodID

                'NextPayPeriodID


                'PayStub.paypFrom = Format(CDate(.Cells("Column2").Value), "yyyy-MM-dd")

                'PayStub.paypTo = Format(CDate(.Cells("Column3").Value), "yyyy-MM-dd")


                'Dim sel_yearDateFrom = CDate('PayStub.paypFrom).Year

                'Dim sel_yearDateTo = CDate('PayStub.paypTo).Year

                'Dim sel_year = If(sel_yearDateFrom > sel_yearDateTo, _
                '                  sel_yearDateFrom, _
                '                  sel_yearDateTo)


                'PayStub.isEndOfMonth = Trim(.Cells("Column14").Value)

                'PayStub.genpayselyear = Format(CDate(.Cells("Column2").Value), "yyyy")

                'PayStub.numofweekdays = 0

                'PayStub.numofweekends = 0

                'Dim date_diff = DateDiff(DateInterval.Day, CDate('PayStub.paypFrom), CDate('PayStub.paypTo))

                'For i = 0 To date_diff

                '    Dim DayOfWeek = CDate('PayStub.paypFrom).AddDays(i)

                '    If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                '        'PayStub.numofweekends += 1

                '    ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                '        'PayStub.numofweekends += 1

                '    Else
                '        'PayStub.numofweekdays += 1

                '    End If

                'Next

                'PayStub.withthirteenthmonthpay = 0

            End With

            Dim PayFreqRowID = EXECQUER("SELECT RowID FROM payfrequency WHERE PayFrequencyType='" & quer_empPayFreq & "';")

            'PayStub.genpayroll(PayFreqRowID)

        End If

        Me.DialogResult = Windows.Forms.DialogResult.OK

        Me.Close()

    End Sub

End Class