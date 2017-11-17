Public Class PayrollSummaDateSelection

    Dim rpt_index = Nothing

    Public Property ReportIndex As Object
        Get
            Return rpt_index
        End Get
        Set(value As Object)
            rpt_index = value
        End Set
    End Property

    Dim me_DateFromID = Nothing

    Public Property DateFromID As Object
        Get
            Return me_DateFromID
        End Get
        Set(value As Object)
            me_DateFromID = value
        End Set
    End Property

    Dim me_DateToID = Nothing

    Public Property DateToID As Object
        Get
            Return me_DateToID
        End Get
        Set(value As Object)
            me_DateToID = value
        End Set
    End Property

    Dim me_DateFromstr = Nothing

    Public Property DateFromstr As Object
        Get
            Return me_DateFromstr
        End Get
        Set(value As Object)
            me_DateFromstr = value
        End Set
    End Property

    Dim me_DateTostr = Nothing

    Public Property DateTostr As Object
        Get
            Return me_DateTostr
        End Get
        Set(value As Object)
            me_DateTostr = value
        End Set
    End Property

    Dim me_PaypID = Nothing

    Public Property PayPeriodID As Object
        Get
            Return me_PaypID
        End Get
        Set(value As Object)
            me_PaypID = value
        End Set
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)
        Select Case rpt_index

            Case 4 'Employee Loan Report

                cboStringParameter.Visible = True
                TextBox1.Visible = True
                Label360.Visible = True
                Label5.Visible = True
                enlistToCboBox("SELECT p.PartNo" &
                               " FROM product p" &
                               " INNER JOIN category c ON c.OrganizationID='" & orgztnID & "' AND c.CategoryName='Loan Type'" &
                               " WHERE p.CategoryID=c.RowID" &
                               " AND p.OrganizationID=" & orgztnID & ";",
                               cboStringParameter)
                cboStringParameter.DropDownStyle = ComboBoxStyle.DropDownList
                TextBox1.Text = "Loan Type"

            Case 6 'Payroll Summary Report

                TextBox1.Visible = True
                Label360.Visible = True
                Label5.Visible = True

                With cboStringParameter
                    .Visible = True
                    .Items.Add("Cash")
                    .Items.Add("Direct Deposit")
                    .DropDownStyle = ComboBoxStyle.DropDownList
                End With

                TextBox1.Text = "Salary Distribution"
            Case Else
                Me.Height = 578
        End Select

        MyBase.OnLoad(e)
    End Sub

    '**********************
    Dim yearnow = CDate(dbnow).Year

    Private Sub PayrollSummaDateSelection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"
        Label3.Text = ""
        Label4.Text = ""
        TabPage1_Enter(TabControl1, New EventArgs)
    End Sub

    Sub VIEW_payp(Optional param_Date As Object = Nothing,
                  Optional PayFreqType As Object = Nothing)
        Dim params(3, 2) As Object

        params(0, 0) = "payp_OrganizationID"
        params(1, 0) = "param_Date"
        params(2, 0) = "isotherformat"
        params(3, 0) = "PayFreqType"

        params(0, 1) = orgztnID
        params(1, 1) = If(param_Date = Nothing, DBNull.Value, param_Date & "-01-01")
        params(2, 1) = "1"
        params(3, 1) = PayFreqType

        EXEC_VIEW_PROCEDURE(params,
                            "VIEW_payp",
                            dgvpayperiod)
    End Sub

    Private Sub dgvpayperiod_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayperiod.CellContentClick
        Dim colName = dgvpayperiod.Columns(e.ColumnIndex).Name

        If colName = "Column1" Then
            dgvpayperiod.Item("Column6", e.RowIndex).Selected = True
            dgvpayperiod.Item("Column1", e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub dgvpayperiod_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayperiod.CellEndEdit
        Static limittwo As SByte = 0
        Static previousindex As SByte = 0

        Label3.Text = ""
        Label4.Text = ""

        If dgvpayperiod.RowCount <> 0 Then
            Dim rowindx = e.RowIndex
            Dim colName = dgvpayperiod.Columns(e.ColumnIndex).Name

            If colName = "Column1" Then
                If dgvpayperiod.Item("Column1", rowindx).Value = True Then
                    limittwo += 1
                    Dim istwo = 0

                    For Each dgvrow As DataGridViewRow In dgvpayperiod.Rows
                        If dgvrow.Cells("Column1").Value = True Then
                            istwo += 1
                        End If
                    Next

                    If istwo = 2 Then
                        limittwo = 2
                    End If

                    If limittwo >= 3 Then
                        limittwo = 0
                        previousindex = -1
                        me_DateFromID = Nothing
                        me_DateToID = Nothing
                        me_DateFromstr = Nothing
                        me_DateTostr = Nothing

                        For Each dgvrow As DataGridViewRow In dgvpayperiod.Rows
                            dgvrow.Cells("Column1").Value = False
                        Next
                    ElseIf limittwo = 2 Then

                        If previousindex < rowindx Then
                            me_DateFromID = dgvpayperiod.Item("Column4", previousindex).Value
                            me_DateToID = dgvpayperiod.Item("Column4", rowindx).Value
                            me_DateFromstr = dgvpayperiod.Item("Column2", previousindex).Value
                            me_DateTostr = dgvpayperiod.Item("Column3", rowindx).Value
                        Else
                            me_DateFromID = dgvpayperiod.Item("Column4", rowindx).Value
                            me_DateToID = dgvpayperiod.Item("Column4", previousindex).Value
                            me_DateFromstr = dgvpayperiod.Item("Column2", rowindx).Value
                            me_DateTostr = dgvpayperiod.Item("Column3", previousindex).Value
                        End If
                    Else
                        If limittwo = 1 Then
                            me_DateFromID = dgvpayperiod.Item("Column4", rowindx).Value
                            me_DateToID = dgvpayperiod.Item("Column4", rowindx).Value
                            me_DateFromstr = dgvpayperiod.Item("Column2", rowindx).Value
                            me_DateTostr = dgvpayperiod.Item("Column3", rowindx).Value
                        End If

                        previousindex = rowindx
                    End If
                Else

                    If limittwo >= 2 Then
                        For Each dgvrow As DataGridViewRow In dgvpayperiod.Rows

                            If dgvrow.Cells("Column1").Value = True Then
                                me_DateFromID = dgvrow.Cells("Column4").Value
                                me_DateToID = dgvrow.Cells("Column4").Value
                                me_DateFromstr = dgvrow.Cells("Column2").Value
                                me_DateTostr = dgvrow.Cells("Column3").Value

                                Exit For
                            End If
                        Next

                        limittwo = 1
                    End If
                End If
            End If

            If me_DateFromstr = Nothing And
                me_DateFromstr = Nothing Then
            Else
                Label3.Text = Format(CDate(me_DateFromstr), "MMMM d, yyyy")
                Label4.Text = Format(CDate(me_DateTostr), "MMMM d, yyyy")
            End If
        Else
            limittwo = 0
            previousindex = 0
            me_DateFromID = Nothing
            me_DateToID = Nothing
            me_DateFromstr = Nothing
            me_DateTostr = Nothing
        End If
    End Sub

    Dim numofweekdays = 0

    Dim numofweekends = 0

    Dim paypFrom = Nothing

    Dim paypTo = Nothing

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

        linkNxt.Text = (yearnow + 1) & " →"
        linkPrev.Text = "← " & (yearnow - 1)

        Dim sel_tab_pg = TabControl1.SelectedTab
        VIEW_payp(yearnow, sel_tab_pg.Text.Trim)

        dgvpayperiod.EndEdit(True)

        If dgvpayperiod.RowCount <> 0 Then
        Else
            me_DateFromID = Nothing
            me_DateToID = Nothing
            me_DateFromstr = Nothing
            me_DateTostr = Nothing
        End If
    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        yearnow = yearnow - 1

        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"

        Dim sel_tab_pg = TabControl1.SelectedTab
        VIEW_payp(yearnow, sel_tab_pg.Text.Trim)

        dgvpayperiod.EndEdit(True)

        If dgvpayperiod.RowCount <> 0 Then
        Else
            me_DateFromID = Nothing
            me_DateToID = Nothing
            me_DateFromstr = Nothing
            me_DateTostr = Nothing
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        dgvpayperiod.EndEdit(True)

        If me_DateFromID = Nothing Then
            Exit Sub
        ElseIf cboStringParameter.Visible Then

            Select Case Convert.ToInt32(rpt_index)
                Case 6 'prints Payroll summary report

                    Dim n_ExecuteQuery As New _
                        ExecuteQuery(String.Concat("SELECT EXISTS(",
                                                   "SELECT tp.RowID",
                                                   " FROM thirteenthmonthpay tp",
                                                   " INNER JOIN paystub ps ON ps.RowID=tp.PaystubID AND ps.OrganizationID=tp.OrganizationID",
                                                   " AND ps.PayPeriodID=", ValNoComma(me_DateFromID),
                                                   " WHERE tp.OrganizationID=", orgztnID,
                                                   " LIMIT 1",
                                                   ") `Result`;"), 192)

                    Dim bool_result As Boolean = (Convert.ToInt16(n_ExecuteQuery.Result) = 0)

                    If Convert.ToInt16(n_ExecuteQuery.Result) = 0 Then
                        Dim param_array = New Object() {orgztnID, me_DateFromID, z_User}

                        Dim n_ExecSQLProcedure As New _
                            ExecSQLProcedure("RECOMPUTE_thirteenthmonthpay",
                                             param_array)
                    End If
            End Select

            If cboStringParameter.Text = String.Empty Then
                WarnBalloon("Please input " & TextBox1.Text, "Invalid " & TextBox1.Text, cboStringParameter, cboStringParameter.Width - 17, -69)

                Exit Sub
            End If
        End If

        If dgvpayperiod.RowCount <> 0 Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

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

    Dim DefaultFontStyle = New System.Drawing.Font("Segoe UI Semibold", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim faultFontStyle = New System.Drawing.Font("Segoe UI Semilight", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Private Sub TabPage1_Enter(sender As Object, e As EventArgs) Handles TabPage1.Enter
        VIEW_payp(, TabPage1.Text.Trim)
    End Sub

    Private Sub TabPage2_Enter(sender As Object, e As EventArgs) Handles TabPage2.Enter
        VIEW_payp(, TabPage2.Text.Trim)
    End Sub

End Class
