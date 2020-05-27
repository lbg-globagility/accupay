Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class PayRateForm
    Dim view_ID As Integer = Nothing

    Dim _now

    Private Sub Payrate_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadpayrate()

        enlistToCboBox("SELECT DisplayValue FROM listofval WHERE Type='Holiday Type' ORDER BY DisplayValue;", cbopaytype)

        With tbleditedpayrate
            .Columns.Add("currPayrateID")
            .Columns.Add("currPayrate")
            .Columns.Add("Dscrptn")
            .Columns.Add("PayType")
            .Columns.Add("OTRate")
            .Columns.Add("NightDiffRate")
            .Columns.Add("NightDiffOTRate")
            .Columns.Add("RestDayRate")
            .Columns.Add("RestDayOTRate")

            .Columns.Add("pratedate")

        End With

        cbomonth.Items.Clear()
        For i = 0 To 11
            Dim date_month = EXECQUER("SELECT DATE_FORMAT(DATE_ADD(MAKEDATE(YEAR(NOW()),1), INTERVAL " & i & " MONTH),'%M');")
            cbomonth.Items.Add(date_month)
        Next

        _now = EXECQUER("SELECT DATE_FORMAT(NOW(),'%Y-%m-%d');")

        TextBox5.Text = CDate(_now).Year
        cbomonth.Text = Format(CDate(_now), "MMMM")

        addhandlr(sender, e)

        view_ID = VIEW_privilege("Pay rate", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then

            tsbtnsavepayrate.Visible = 0
            dontUpdate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    tsbtnsavepayrate.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

        If dgvpayrate.RowCount <> 0 Then
            dgvisLlostfocus = 0
        Else
            dgvisLlostfocus = 1

        End If

    End Sub

    Dim dattab As New DataTable
    Dim defaultViewDate As String

    Sub loadpayrate(Optional prate_date As Object = Nothing)

        dgvpayrate.Rows.Clear()

        If prate_date = Nothing Then

            defaultViewDate = EXECQUER("SELECT DATE_FORMAT(NOW(),'%M, %Y');")

            Label1.Text = defaultViewDate

            dattab = retAsDatTbl("SELECT prate.RowID,DAY(prate.Date) 'dateday'" &
                                 ", DAYOFWEEK(prate.Date) 'dayofwk'" &
                                 ", DAY(LAST_DAY(prate.Date)) 'maxday'" &
                                 ",COALESCE(prate.PayType,'') 'PayType'" &
                                 ",COALESCE(prate.Description,'') 'Description'" &
                                 ",COALESCE(prate.PayRate,1) 'PayRate'" &
                                 ",COALESCE(prate.OvertimeRate,1) 'OvertimeRate'" &
                                 ",COALESCE(prate.NightDifferentialRate,1) 'NightDifferentialRate'" &
                                 ",COALESCE(prate.NightDifferentialOTRate,1) 'NightDifferentialOTRate'" &
                                 ",DATE_FORMAT(prate.Date,'%m-%d-%Y') 'Date'" &
                                 ",COALESCE(RestDayRate,1) 'RestDayRate'" &
                                 ",COALESCE(RestDayOvertimeRate,1) 'RestDayOvertimeRate'" &
                                 " FROM payrate prate" &
                                 " WHERE DATE_FORMAT(prate.Date,'%Y-%m')=DATE_FORMAT(CURDATE(),'%Y-%m')" &
                                 " AND prate.OrganizationID='" & orgztnID & "'" &
                                 " ORDER BY prate.Date;")
            'MONTH(prate.Date)=MONTH(NOW()) AND YEAR(prate.Date)=YEAR(NOW());
            'DISTINCT(DATE_FORMAT(DATE,'%m-%d-%Y')),
            dgvpayrate.Rows.Clear()

            Dim countofweek As Integer = 0

            Dim cindx As Integer = 0

            Dim iii = dattab.Compute("COUNT(dayofwk)", "dayofwk=7")
            countofweek = If(IsDBNull(iii), 0, iii)

            'For Each drow As DataRow In dattab.Rows
            '    If Val(drow("dayofwk")) = 7 Then
            '        countofweek += 1
            '    End If
            'Next

            For i = 0 To countofweek
                dgvpayrate.Rows.Add()
            Next

            For Each dgvr As DataGridViewRow In dgvpayrate.Rows
                For Each c As DataGridViewColumn In dgvpayrate.Columns
                    If dattab.Rows.Count <> cindx Then
                        If c.Index + 1 >= Val(dattab.Rows(cindx)("dayofwk")) Then
                            dgvpayrate.Item(c.Name, dgvr.Index).Value = cindx + 1
                            cindx += 1
                        End If
                    End If
                Next
            Next
        Else

            Dim querdate As Object = Format(CDate(prate_date), "yyyy-MM-dd")

            defaultViewDate = Format(CDate(prate_date), "MMMM, yyyy")
            'Label1.Text = Format(CDate(prate_date), "MMMM dd, yyyy")

            dattab = retAsDatTbl("SELECT prate.RowID" &
                                 ", DAY(prate.Date) 'dateday'" &
                                 ", DAYOFWEEK(prate.Date) 'dayofwk'" &
                                 ", DAY(LAST_DAY(prate.Date)) 'maxday'" &
                                 ", COALESCE(prate.PayType,'') 'PayType'" &
                                 ", COALESCE(prate.Description,'') 'Description'" &
                                 ", COALESCE(prate.PayRate,1) 'PayRate'" &
                                 ", COALESCE(prate.OvertimeRate,1) 'OvertimeRate'" &
                                 ", COALESCE(prate.NightDifferentialRate,1) 'NightDifferentialRate'" &
                                 ", COALESCE(prate.NightDifferentialOTRate,1) 'NightDifferentialOTRate'" &
                                 ", DATE_FORMAT(prate.Date,'%m-%d-%Y') 'Date'" &
                                 ", COALESCE(RestDayRate,1) 'RestDayRate'" &
                                 ", COALESCE(RestDayOvertimeRate,1) 'RestDayOvertimeRate'" &
                                 " FROM payrate prate" &
                                 " WHERE DATE_FORMAT(prate.Date,'%Y-%m')=DATE_FORMAT('" & querdate & "','%Y-%m')" &
                                 " AND prate.OrganizationID='" & orgztnID & "'" &
                                 " ORDER BY prate.Date;")
            'MONTH(prate.Date)=MONTH('" & querdate & "') AND YEAR(prate.Date)=YEAR('" & querdate & "');
            'DISTINCT(DATE_FORMAT(DATE,'%m-%d-%Y')),
            If dattab.Rows.Count <> 0 Then

                dgvpayrate.Rows.Clear()

                Dim the_date As String = StrReverse(getStrBetween(StrReverse(querdate.ToString), "", "-"))

                Dim countofweek As Integer = 0

                Dim cindx As Integer = 0

                Dim colmntosel, rowtosel As Integer

                Dim iii = dattab.Compute("COUNT(dayofwk)", "dayofwk=7")
                countofweek = If(IsDBNull(iii), 0, iii)

                'For Each drow As DataRow In dattab.Rows
                '    If Val(drow("dayofwk")) = 7 Then
                '        countofweek += 1
                '        Exit For
                '    End If
                'Next

                For i = 0 To countofweek
                    dgvpayrate.Rows.Add()
                Next

                For Each dgvr As DataGridViewRow In dgvpayrate.Rows
                    For Each c As DataGridViewColumn In dgvpayrate.Columns
                        If dattab.Rows.Count <> cindx Then
                            If c.Index + 1 >= Val(dattab.Rows(cindx)("dayofwk")) Then
                                dgvpayrate.Item(c.Name, dgvr.Index).Value = cindx + 1
                                cindx += 1
                                If the_date = cindx Then
                                    colmntosel = c.Index
                                    rowtosel = dgvr.Index
                                End If
                            End If
                        End If
                    Next
                Next

                dgvpayrate.Item(colmntosel, rowtosel).Selected = True

                Dim currColName As String = dgvpayrate.Columns(dgvpayrate.CurrentCell.ColumnIndex).Name
                ObjectFields(dgvpayrate, currColName)
            Else

                dgvpayrate.Rows.Clear()

            End If

        End If

        For Each dgvr As DataGridViewRow In dgvpayrate.Rows
            If dgvr.Cells("Column1").Value = Nothing _
           And dgvr.Cells("Column2").Value = Nothing _
           And dgvr.Cells("Column3").Value = Nothing _
           And dgvr.Cells("Column4").Value = Nothing _
           And dgvr.Cells("Column5").Value = Nothing _
           And dgvr.Cells("Column6").Value = Nothing _
           And dgvr.Cells("Column7").Value = Nothing Then

                dgvpayrate.Rows.Remove(dgvr)
                Exit For
            End If
        Next

    End Sub

#Region "my Old form load"

    'Dim dattab As New DataTable
    ''NOW()
    '    dattab = retAsDatTbl("SELECT DAYNAME(DATE_FORMAT(DATE_SUB(NOW(), INTERVAL DAY(NOW())-1 DAY),'%Y-%m-%d')) 'This month starts with this day'," & _
    '                         "DAY(LAST_DAY(NOW())) 'Last number of this month';")

    'Dim monthstartIn As Integer = Val(dattab.Rows(0)("This month starts with this day"))
    'Dim monthlastDay As Integer = Val(dattab.Rows(0)("Last number of this month"))

    'Dim weekcount As Integer = monthlastDay / 7

    ''1 = Sunday
    ''2 = Monday
    ''3 = Tuesday
    ''4 = Wednesday
    ''5 = Thursday
    ''6 = Friday
    ''7 = Saturday

    '    DataGridView1.Rows.Clear()
    'Dim daystarts As SByte = 0
    'Dim daycounting As Integer = 0
    '    For i = 0 To weekcount - 1
    'Dim r = DataGridView1.Rows.Add()
    '        If daystarts = 0 Then
    '            daystarts = 1
    '            For Each c As DataGridViewColumn In DataGridView1.Columns
    '                If c.Index >= monthstartIn - 1 Then
    '                    daycounting += 1
    '                    DataGridView1.Item(c.Name, i).Value = daycounting
    '                End If
    '            Next
    '        Else
    '            For Each c As DataGridViewColumn In DataGridView1.Columns
    '                daycounting += 1
    '                DataGridView1.Item(c.Name, i).Value = daycounting
    '            Next
    '        End If

    '        DataGridView1.Rows(r).Height = 100
    '    Next

    '    If daycounting < monthlastDay Then
    'Dim r = DataGridView1.Rows.Add()
    '        For Each c As DataGridViewColumn In DataGridView1.Columns
    '            If daycounting < monthlastDay Then
    '                daycounting += 1
    '                DataGridView1.Item(c.Name, r).Value = daycounting
    '            End If
    '        Next
    '    End If

#End Region

    Dim currPayrateID,
        currPayrate,
        Dscrptn,
        PayType,
        OTRate,
        NightDiffRate,
        NightDiffOTRate,
        RestRate,
        RestOTRate As String

    Sub addhandlr(ByVal sendr As Object, ByVal e As EventArgs)
        AddHandler dgvpayrate.CurrentCellChanged, AddressOf DataGridView1_CurrentCellChanged
        AddHandler dgvpayrate.RowLeave, AddressOf DataGridView1_RowLeave
        AddHandler dgvpayrate.SelectionChanged, AddressOf DataGridView1_SelectionChanged
        AddHandler dgvpayrate.CellLeave, AddressOf DataGridView1_CellLeave

        DataGridView1_CurrentCellChanged(sendr, e)
        DataGridView1_SelectionChanged(sendr, e)
    End Sub

    Sub rmvhandlr()
        RemoveHandler dgvpayrate.CurrentCellChanged, AddressOf DataGridView1_CurrentCellChanged
        RemoveHandler dgvpayrate.RowLeave, AddressOf DataGridView1_RowLeave
        RemoveHandler dgvpayrate.SelectionChanged, AddressOf DataGridView1_SelectionChanged
        RemoveHandler dgvpayrate.CellLeave, AddressOf DataGridView1_CellLeave
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayrate.CellContentClick

    End Sub

    Dim currColName As String

    Private Sub DataGridView1_CurrentCellChanged(sender As Object, e As EventArgs) 'Handles DataGridView1.CurrentCellChanged
        rmvhndlrTextChangeEditEvent()

        If dgvpayrate.RowCount <> 0 Then

            If dgvpayrate.CurrentCell IsNot Nothing Then

                dgvisLlostfocus = 1

                currColName = dgvpayrate.Columns(dgvpayrate.CurrentCell.ColumnIndex).Name

                'Label1.Text = defaultViewDate.Insert(defaultViewDate.IndexOf(","), _
                '                                 " " & dgvpayrate.CurrentRow.Cells(currColName).Value)

                dgvpayrate.Columns(currColName).Width = 230

                'hideObjFields()

                cbopaytype.Text = ""
                txtdescrptn.Text = ""
                txtpayrate.Text = ""
                txtotrate.Text = ""
                txtnightdiffrate.Text = ""
                txtnightdiffotrate.Text = ""
                txtrestrate.Text = ""
                txtrestotrate.Text = ""

                If dgvpayrate.CurrentRow.Cells(currColName).Value <> Nothing Then
                    Dim indx_day As String = If(dgvpayrate.CurrentRow.Cells(currColName).Value.ToString.Length = 1,
                                                "0" & dgvpayrate.CurrentRow.Cells(currColName).Value,
                                                dgvpayrate.CurrentRow.Cells(currColName).Value)

                    Dim selrow() As DataRow = dattab.Select("Date='" & monthindx & "-" & indx_day & "-" & TextBox5.Text & "'")

                    For Each drow In selrow
                        currPayrateID = drow("RowID")
                        currPayrate = drow("PayRate")
                        Dscrptn = drow("Description")
                        PayType = drow("PayType")
                        OTRate = drow("OvertimeRate")
                        NightDiffRate = drow("NightDifferentialRate")
                        NightDiffOTRate = drow("NightDifferentialOTRate")
                        RestRate = drow("RestDayRate")
                        RestOTRate = drow("RestDayOvertimeRate")

                        cbopaytype.Text = drow("PayType")
                        txtdescrptn.Text = drow("Description")
                        txtpayrate.Text = Val(drow("PayRate")) * 100
                        txtotrate.Text = Val(drow("OvertimeRate")) * 100
                        txtnightdiffrate.Text = Val(drow("NightDifferentialRate")) * 100
                        txtnightdiffotrate.Text = Val(drow("NightDifferentialOTRate")) * 100
                        txtrestrate.Text = Val(drow("RestDayRate")) * 100
                        txtrestotrate.Text = Val(drow("RestDayOvertimeRate")) * 100

                    Next

                End If

                If currColName = "Column7" Then
                    dgvpayrate.FirstDisplayedScrollingColumnIndex = dgvpayrate.Columns("Column7").Index
                End If

                If dgvisLlostfocus = 0 Then
                    ObjectFields(dgvpayrate, currColName)
                End If

            End If
        Else
            dgvisLlostfocus = 0

            cbopaytype.SelectedIndex = -1
            txtdescrptn.Text = Nothing
            txtpayrate.Text = Nothing
            txtotrate.Text = Nothing
            txtnightdiffrate.Text = Nothing
            txtnightdiffotrate.Text = Nothing
            Label1.Text = ""

            hideObjFields()
        End If

        addhndlrTextChangeEditEvent()
    End Sub

    Dim CellIndexLeave = 0

    Private Sub DataGridView1_CellLeave(sender As Object, e As DataGridViewCellEventArgs) 'Handles DataGridView1.CellLeave
        CellIndexLeave = -1

        If dgvpayrate.RowCount <> 0 Then

            CellIndexLeave = e.ColumnIndex
            dgvpayrate.Columns(e.ColumnIndex).Width = 95
        End If
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) 'Handles DataGridView1.SelectionChanged

        Static once As SByte = -1

        If dgvpayrate.RowCount <> 0 Then
            dgvisLlostfocus = 1

            dgvpayrate.CurrentRow.Height = 295

            If dgvpayrate.CurrentRow.Index = dgvpayrate.RowCount - 1 _
                And dgvpayrate.CurrentRow.Index = dgvpayrate.RowCount - 2 Then

                Panel2.AutoScrollPosition = New Point(0, 0)

                dgvpayrate.FirstDisplayedScrollingRowIndex = dgvpayrate.CurrentRow.Index

                once = -1

            ElseIf dgvpayrate.CurrentRow.Index >= dgvpayrate.RowCount - 2 Then

                Dim sameRIndx = dgvpayrate.CurrentRow.Index

                If once <> sameRIndx Then

                    once = sameRIndx

                    Panel2.AutoScrollPosition = New Point(0, Panel2.VerticalScroll.Maximum)

                    dgvpayrate.FirstDisplayedScrollingRowIndex = dgvpayrate.CurrentRow.Index - 1

                End If
            Else

                once = -1

            End If
        Else
            dgvisLlostfocus = 0
        End If
    End Sub

    Dim RowIndexLeave = 0

    Private Sub DataGridView1_RowLeave(sender As Object, e As DataGridViewCellEventArgs) 'Handles DataGridView1.RowLeave
        RowIndexLeave = -1

        If dgvpayrate.RowCount <> 0 Then
            RowIndexLeave = e.RowIndex
            dgvpayrate.Rows(e.RowIndex).Height = 95
        End If
    End Sub

    Sub ObjectFields(ByVal dgv As DataGridView,
                        ByVal colName As String,
                        Optional isVisb As SByte = 0)

        Try
            If dgv.CurrentRow.Cells(colName).Selected Then

                Dim rect As Rectangle = dgv.GetCellDisplayRectangle(dgv.CurrentRow.Cells(colName).ColumnIndex, dgv.CurrentRow.Cells(colName).RowIndex, True)

                '*********************Pay Type***************************
                Label11.Parent = dgv
                Label11.Location = New Point(rect.Right - cbopaytype.Width, rect.Top + 5)
                Label11.Visible = If(isVisb = 0, True, False)

                cbopaytype.Parent = dgv '                   'btn.Width
                cbopaytype.Location = New Point(rect.Right - cbopaytype.Width, rect.Top + 18)
                cbopaytype.Visible = If(isVisb = 0, True, False)

                '********************Description****************************
                Label12.Parent = dgv
                Label12.Location = New Point(rect.Right - txtdescrptn.Width, rect.Top + 43)
                Label12.Visible = If(isVisb = 0, True, False)

                txtdescrptn.Parent = dgv
                txtdescrptn.Location = New Point(rect.Right - txtdescrptn.Width, rect.Top + 56)
                txtdescrptn.Visible = If(isVisb = 0, True, False)
                '**********************Pay Rate**************************
                Label13.Parent = dgv
                Label13.Location = New Point(rect.Right - txtpayrate.Width, rect.Top + 87) '77
                Label13.Visible = If(isVisb = 0, True, False)

                txtpayrate.Parent = dgv
                txtpayrate.Location = New Point(rect.Right - txtpayrate.Width, rect.Top + 100) '90
                txtpayrate.Visible = If(isVisb = 0, True, False)
                '***********************Overtime Rate*************************
                Label14.Parent = dgv
                Label14.Location = New Point(rect.Right - txtotrate.Width, rect.Top + 121) '111
                Label14.Visible = If(isVisb = 0, True, False)

                txtotrate.Parent = dgv
                txtotrate.Location = New Point(rect.Right - txtotrate.Width, rect.Top + 134) '124
                txtotrate.Visible = If(isVisb = 0, True, False)
                '**********************Night differential rate**************************
                Label15.Parent = dgv
                Label15.Location = New Point(rect.Right - txtnightdiffrate.Width, rect.Top + 155) '145
                Label15.Visible = If(isVisb = 0, True, False)

                txtnightdiffrate.Parent = dgv
                txtnightdiffrate.Location = New Point(rect.Right - txtnightdiffrate.Width, rect.Top + 168) '158
                txtnightdiffrate.Visible = If(isVisb = 0, True, False)
                '************************Night differential overtime rate************************
                Label16.Parent = dgv
                Label16.Location = New Point(rect.Right - txtnightdiffotrate.Width, rect.Top + 189) '179
                Label16.Visible = If(isVisb = 0, True, False)

                txtnightdiffotrate.Parent = dgv
                txtnightdiffotrate.Location = New Point(rect.Right - txtnightdiffotrate.Width, rect.Top + 202) '192
                txtnightdiffotrate.Visible = If(isVisb = 0, True, False)
                '************************Rest day rate************************
                Label17.Parent = dgv
                Label17.Location = New Point(rect.Right - txtrestrate.Width, rect.Top + 223) '179
                Label17.Visible = If(isVisb = 0, True, False)

                txtrestrate.Parent = dgv
                txtrestrate.Location = New Point(rect.Right - txtrestrate.Width, rect.Top + 236) '192
                txtrestrate.Visible = If(isVisb = 0, True, False)
                '************************Rest day rate************************
                Label18.Parent = dgv
                Label18.Location = New Point(rect.Right - txtrestotrate.Width, rect.Top + 257) '179
                Label18.Visible = If(isVisb = 0, True, False)

                txtrestotrate.Parent = dgv
                txtrestotrate.Location = New Point(rect.Right - txtrestotrate.Width, rect.Top + 270) '192
                txtrestotrate.Visible = If(isVisb = 0, True, False)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Sub hideObjFields()
        cbopaytype.Visible = False
        txtdescrptn.Visible = False
        txtpayrate.Visible = False
        txtotrate.Visible = False
        txtnightdiffrate.Visible = False
        txtnightdiffotrate.Visible = False
        txtrestrate.Visible = False
        txtrestotrate.Visible = False

        Label11.Visible = False
        Label12.Visible = False
        Label13.Visible = False
        Label14.Visible = False
        Label15.Visible = False
        Label16.Visible = False
        Label17.Visible = False
        Label18.Visible = False
    End Sub

    Dim searchdate As Object

    Dim dgvisLlostfocus As SByte = 0

    Private Sub TextBox5_GotFocus(sender As Object, e As EventArgs) Handles TextBox5.GotFocus, cbomonth.GotFocus, Button2.GotFocus
        dgvisLlostfocus = 1
        DataGridView1_CurrentCellChanged(sender, e)
        DataGridView1_SelectionChanged(sender, e)
    End Sub

    Private Sub TextBox5_LostFocus(sender As Object, e As EventArgs) Handles TextBox5.LostFocus, cbomonth.LostFocus, Button2.LostFocus
        dgvisLlostfocus = 0
    End Sub

    Private Sub dgvpayrate_GotFocus(sender As Object, e As EventArgs) 'Handles dgvpayrate.GotFocus
        If dgvpayrate.RowCount <> 0 Then

            currColName = dgvpayrate.Columns(dgvpayrate.CurrentCell.ColumnIndex).Name

            If dgvisLlostfocus = 0 Then
                ObjectFields(dgvpayrate, currColName)
            End If
        Else
            hideObjFields()
        End If
    End Sub

    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        Dim e_KChar As String = Asc(e.KeyChar)
        e.Handled = TrapDecimKey(e_KChar)
    End Sub

    Private Sub TextBox5_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox5.KeyDown, cbomonth.KeyDown
        If e.KeyCode = Keys.Enter Then
            rmvhandlr()
            Button2_Click(sender, e)
        ElseIf e.KeyCode = Keys.Up Then
            TextBox5.Text = Val(TextBox5.Text) + 1
            TextBox5.SelectionStart = TextBox5.TextLength
        ElseIf e.KeyCode = Keys.Down Then
            TextBox5.Text = Val(TextBox5.Text) - 1
            TextBox5.SelectionStart = TextBox5.TextLength
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        rmvhandlr()

        For Each r As DataGridViewColumn In dgvpayrate.Columns
            r.Width = 95
        Next

        Try
            If Trim(cbomonth.Text) <> "" Then

                searchdate = cbomonth.Text & " " & TextBox5.Text
                searchdate = Format(CDate(searchdate), "MMMM yyyy")
                loadpayrate(searchdate)
            Else
                loadpayrate()
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        Finally
            addhandlr(sender, e)
        End Try
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Me.Close()
    End Sub

    Dim dontUpdate As SByte = 0

    Private Sub tsbtnsavepayrate_Click(sender As Object, e As EventArgs) Handles tsbtnsavepayrate.Click
        If dontUpdate = 1 Then
            Exit Sub
        End If
        rmvhandlr()

        Dim curr_rowindx, curr_colindx As Integer

        curr_rowindx = -1
        curr_colindx = -1

        If dgvpayrate.RowCount <> 0 Then

            Try
                curr_rowindx = dgvpayrate.CurrentRow.Index
                curr_colindx = dgvpayrate.CurrentCell.ColumnIndex
            Catch ex As Exception

                curr_rowindx = -1
                curr_colindx = -1

            End Try

        End If

        For Each drow As DataRow In tbleditedpayrate.Rows

            drow("currPayrate") = If(Val(drow("currPayrate")) = 0, 1, drow("currPayrate"))
            drow("OTRate") = If(Val(drow("OTRate")) = 0, 1, drow("OTRate"))
            drow("NightDiffRate") = If(Val(drow("NightDiffRate")) = 0, 1, drow("NightDiffRate"))
            drow("NightDiffOTRate") = If(Val(drow("NightDiffOTRate")) = 0, 1, drow("NightDiffOTRate"))
            drow("RestDayRate") = If(Val(drow("RestDayRate")) = 0, 1, drow("RestDayRate"))

            'USUALLY UPDATE FUNCTION

            INSUPD_payrate(drow("currPayrateID"),
                           drow("pratedate"),
                           drow("PayType"),
                           drow("Dscrptn"),
                           drow("currPayrate"),
                           drow("OTRate"),
                           drow("NightDiffRate"),
                           drow("NightDiffOTRate"),
                           drow("RestDayRate"),
                           drow("RestDayOTRate")) 'drow("RestDayRate")

        Next

        tbleditedpayrate.Rows.Clear()

        Button2_Click(sender, e)

        If curr_rowindx <> -1 Then
            curr_colindx = dgvpayrate.CurrentCell.ColumnIndex
            dgvpayrate.Item(curr_colindx, curr_rowindx).Selected = True
        End If

        addhandlr(sender, e)
    End Sub

    Sub INSUPD_payrate(Optional prate_RowID As Object = Nothing,
                       Optional prate_Date As Object = Nothing,
                       Optional prate_PayType As Object = Nothing,
                       Optional prate_Description As Object = Nothing,
                       Optional prate_PayRate As Object = Nothing,
                       Optional prate_OvertimeRate As Object = Nothing,
                       Optional prate_NightDifferentialRate As Object = Nothing,
                       Optional prate_NightDifferentialOTRate As Object = Nothing,
                       Optional RestDayRate As Object = Nothing,
                       Optional prate_RestDayOvertimeRate As Object = Nothing)

        Dim params(12, 2) As Object

        params(0, 0) = "prate_RowID"
        params(1, 0) = "prate_OrganizationID"
        params(2, 0) = "prate_CreatedBy"
        params(3, 0) = "prate_LastUpdBy"
        params(4, 0) = "prate_Date"
        params(5, 0) = "prate_PayType"
        params(6, 0) = "prate_Description"
        params(7, 0) = "prate_PayRate"
        params(8, 0) = "prate_OvertimeRate"
        params(9, 0) = "prate_NightDifferentialRate"
        params(10, 0) = "prate_NightDifferentialOTRate"
        params(11, 0) = "prate_RestDayRate"
        params(12, 0) = "prate_RestDayOvertimeRate"

        params(0, 1) = prate_RowID 'If(prate_RowID = Nothing, DBNull.Value, prate_RowID)
        params(1, 1) = orgztnID
        params(2, 1) = z_User 'CreatedBy
        params(3, 1) = z_User 'LastUpdBy
        params(4, 1) = prate_Date
        params(5, 1) = If(prate_PayType = Nothing, "Regular Day", prate_PayType)
        params(6, 1) = If(prate_Description = Nothing, "", prate_Description)
        params(7, 1) = If(prate_PayRate = Nothing, 1.0, prate_PayRate)
        params(8, 1) = If(prate_OvertimeRate = Nothing, 1.25, prate_OvertimeRate)
        params(9, 1) = If(prate_NightDifferentialRate = Nothing, 1.1, prate_NightDifferentialRate)
        params(10, 1) = If(prate_NightDifferentialOTRate = Nothing, 1.38, prate_NightDifferentialOTRate)
        params(11, 1) = If(RestDayRate = Nothing, 1.3, RestDayRate)
        params(12, 1) = If(prate_RestDayOvertimeRate = Nothing, 1.69, prate_RestDayOvertimeRate)

        EXEC_INSUPD_PROCEDURE(params,
                              "INSUPD_payrate",
                              "payrateID") 'voyager, face to face
    End Sub

    Sub addhndlrTextChangeEditEvent()
        AddHandler cbopaytype.TextChanged, AddressOf ComboBox1_TextChanged
        AddHandler txtdescrptn.TextChanged, AddressOf ComboBox1_TextChanged
        AddHandler txtpayrate.TextChanged, AddressOf ComboBox1_TextChanged
        AddHandler txtotrate.TextChanged, AddressOf ComboBox1_TextChanged
        AddHandler txtnightdiffrate.TextChanged, AddressOf ComboBox1_TextChanged
        AddHandler txtnightdiffotrate.TextChanged, AddressOf ComboBox1_TextChanged
        AddHandler txtrestrate.TextChanged, AddressOf ComboBox1_TextChanged
        AddHandler txtrestotrate.TextChanged, AddressOf ComboBox1_TextChanged
    End Sub

    Sub rmvhndlrTextChangeEditEvent()
        RemoveHandler cbopaytype.TextChanged, AddressOf ComboBox1_TextChanged
        RemoveHandler txtdescrptn.TextChanged, AddressOf ComboBox1_TextChanged
        RemoveHandler txtpayrate.TextChanged, AddressOf ComboBox1_TextChanged
        RemoveHandler txtotrate.TextChanged, AddressOf ComboBox1_TextChanged
        RemoveHandler txtnightdiffrate.TextChanged, AddressOf ComboBox1_TextChanged
        RemoveHandler txtnightdiffotrate.TextChanged, AddressOf ComboBox1_TextChanged
        RemoveHandler txtrestrate.TextChanged, AddressOf ComboBox1_TextChanged
        AddHandler txtrestotrate.TextChanged, AddressOf ComboBox1_TextChanged
    End Sub

    Dim editedpayrate As New AutoCompleteStringCollection

    Dim tbleditedpayrate As New DataTable

    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs) 'Handles ComboBox1.TextChanged, TextBox1.TextChanged, TextBox2.TextChanged, _
        If dgvpayrate.RowCount <> 0 Then

            If dgvpayrate.CurrentRow.Cells(dgvpayrate.CurrentCell.ColumnIndex).Value <> Nothing Then
                'For Each drow As DataRow In tbleditedpayrate.Rows
                '    If currPayrateID = drow("currPayrateID") Then

                '        tbleditedpayrate.Rows.Remove(drow)
                '        Exit For
                '    End If
                'Next

                tbleditedpayrate.Rows.Clear()

                Dim nRow As DataRow
                nRow = tbleditedpayrate.NewRow

                Dim indx_day As String = If(dgvpayrate.CurrentRow.Cells(currColName).Value.ToString.Length = 1,
                                            "0" & dgvpayrate.CurrentRow.Cells(currColName).Value,
                                            dgvpayrate.CurrentRow.Cells(currColName).Value)

                Dim selrow() As DataRow = dattab.Select("Date='" & monthindx & "-" & indx_day & "-" & TextBox5.Text & "'")

                For Each drow In selrow
                    currPayrateID = drow("RowID")
                    Exit For
                Next

                nRow("currPayrateID") = currPayrateID

                nRow("pratedate") = TextBox5.Text & "-" & monthindx & "-" & indx_day

                nRow("Dscrptn") = Trim(txtdescrptn.Text)
                nRow("PayType") = Trim(cbopaytype.Text)
                nRow("currPayrate") = (Val(txtpayrate.Text) / 100) ' + 1
                nRow("OTRate") = (Val(txtotrate.Text) / 100) ' + 1
                nRow("NightDiffRate") = (Val(txtnightdiffrate.Text) / 100) ' + 1
                nRow("NightDiffOTRate") = (Val(txtnightdiffotrate.Text) / 100) ' + 1
                nRow("RestDayRate") = (Val(txtrestrate.Text) / 100) ' + 1
                nRow("RestDayOTRate") = (Val(txtrestotrate.Text) / 100) ' + 1

                'Dim day_date = If(dgvpayrate.CurrentRow.Cells(currColName).Value.ToString.Length = 1, _
                '                "0" & dgvpayrate.CurrentRow.Cells(currColName).Value, _
                '                dgvpayrate.CurrentRow.Cells(currColName).Value)

                'nRow("pratedate") = CDate(_now).Year & "-" & _
                '                    CDate(_now).Month & "-" & _
                '                    day_date

                tbleditedpayrate.Rows.Add(nRow)
                'editedpayrate.Add(1)

            End If
        Else
        End If
    End Sub

    Private Sub txtpayrate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtpayrate.KeyPress, txtotrate.KeyPress,
                                                                                      txtnightdiffrate.KeyPress, txtnightdiffotrate.KeyPress,
                                                                                      txtrestrate.KeyPress, txtrestotrate.KeyPress
        Dim e_KChar As String = Asc(e.KeyChar)
        e.Handled = TrapDecimKey(e_KChar)
    End Sub

    Private Sub DataGridView1_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvpayrate.ColumnWidthChanged
        If dgvpayrate.RowCount <> 0 Then
            Dim currColName As String = dgvpayrate.Columns(dgvpayrate.CurrentCell.ColumnIndex).Name
            ObjectFields(dgvpayrate, currColName)
        End If
    End Sub

    Private Sub DataGridView1_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvpayrate.Scroll
        If dgvpayrate.RowCount <> 0 Then
            Dim currColName As String = dgvpayrate.Columns(dgvpayrate.CurrentCell.ColumnIndex).Name
            ObjectFields(dgvpayrate, currColName)
        End If
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        tbleditedpayrate.Rows.Clear()
    End Sub

    Private Sub Form8_ResizeEnd(sender As Object, e As EventArgs) 'Handles Me.ResizeEnd
        If dgvpayrate.RowCount <> 0 Then
            Dim currColName As String = dgvpayrate.Columns(dgvpayrate.CurrentCell.ColumnIndex).Name
            ObjectFields(dgvpayrate, currColName)

        End If
    End Sub

    Private Sub Label1_TextChanged(sender As Object, e As EventArgs) Handles Label1.TextChanged
        Dim lbl_Y = Label1.Location.Y
        Dim lbl_X = (Me.Width / 2) - (Label1.Width / 2)

        Label1.Location = New Point(lbl_X, lbl_Y)
    End Sub

    Private Async Sub cbopaytype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbopaytype.SelectedIndexChanged
        'Dim holidaypayrate = EXECQUER("SELECT DisplayValue FROM listofval WHERE ParentLIC='" & Trim(cbopaytype.Text) & "' AND Type='Pay rate';")

        'Dim ratevalues = Split(holidaypayrate, ",")

        Dim overtimeRateService = MainServiceProvider.GetRequiredService(Of OvertimeRateService)
        Dim overtimeRates = Await overtimeRateService.GetOvertimeRates()

        Dim overtimeRate As New OvertimeRate.RateGroup

        Select Case Trim(cbopaytype.Text)
            Case "Regular Day"
                overtimeRate = overtimeRates.RegularRateGroup
            Case "Special Non-Working Holiday"
                overtimeRate = overtimeRates.SpecialHolidayRateGroup
            Case "Regular Holiday"
                overtimeRate = overtimeRates.RegularHolidayRateGroup
            Case "Double Holiday"
                overtimeRate = overtimeRates.DoubleHolidayRateGroup
        End Select

        If overtimeRate Is Nothing Then

            txtpayrate.Text = 100

            txtotrate.Text = 100

            txtnightdiffrate.Text = 100

            txtnightdiffotrate.Text = 100

            txtrestrate.Text = 100

            txtrestotrate.Text = 100
        Else

            Try
                txtpayrate.Text = GetPayRate(overtimeRate.BasePay)
            Catch ex As Exception
                txtpayrate.Text = 100
            End Try

            Try
                txtotrate.Text = GetPayRate(overtimeRate.Overtime)
            Catch ex As Exception
                txtotrate.Text = 100
            End Try

            Try
                txtnightdiffrate.Text = GetPayRate(overtimeRate.NightDifferential)
            Catch ex As Exception
                txtnightdiffrate.Text = 100
            End Try

            Try
                txtnightdiffotrate.Text = GetPayRate(overtimeRate.NightDifferentialOvertime)
            Catch ex As Exception
                txtnightdiffotrate.Text = 100
            End Try

            Try
                txtrestrate.Text = GetPayRate(overtimeRate.RestDay)
            Catch ex As Exception
                txtrestrate.Text = 100
            End Try

            Try
                txtrestotrate.Text = GetPayRate(overtimeRate.RestDayOvertime)
            Catch ex As Exception
                txtrestotrate.Text = 100
            End Try
        End If
    End Sub

    Private Function GetPayRate(rate As Rate) As String

        Dim number As Decimal = rate.CurrentRate * 100

        Return number.ToString("#.#")

    End Function

    Private Sub txt_Leave(sender As Object, e As EventArgs) 'Handles txtpayrate.Leave, txtotrate.Leave, _
        Dim txtsender As TextBox = New TextBox
        txtsender = DirectCast(sender, TextBox)

        Dim sender_name As String = txtsender.Name

        If Val(txtsender.Text) < 100 Then
            If sender_name = "txtpayrate" Then
                txtpayrate.Text = 100
            ElseIf sender_name = "txtotrate" Then
                txtotrate.Text = 100
            ElseIf sender_name = "txtnightdiffrate" Then
                txtnightdiffrate.Text = 100
            ElseIf sender_name = "txtnightdiffotrate" Then
                txtnightdiffotrate.Text = 100
            End If
        End If
    End Sub

    Dim monthindx As String

    Private Sub cbomonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbomonth.SelectedIndexChanged, cbomonth.SelectedValueChanged
        rmvhandlr()
        monthindx = cbomonth.SelectedIndex + 1
        monthindx = If(monthindx.Length = 1, "0" & monthindx, monthindx)
        Button2_Click(sender, e)
    End Sub

    Private Sub dgvpayrate_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvpayrate.CellPainting
        If e.RowIndex = -1 Then
            GridDrawCustomHeaderColumns(dgvpayrate, e,
             My.Resources.ColumnBGStyle005,
             DGVHeaderImageAlignments.FillCell)
        End If
    End Sub

    Private Sub GridDrawCustomHeaderColumns(ByVal dgv As DataGridView,
     ByVal e As DataGridViewCellPaintingEventArgs, ByVal img As Image,
     ByVal Style As DGVHeaderImageAlignments)
        ' All of the graphical Processing is done here.
        Dim gr As Graphics = e.Graphics
        ' Fill the BackGround with the BackGroud Color of Headers.
        ' This step is necessary, for transparent images, or what's behind
        ' would be painted instead.
        gr.FillRectangle(
         New SolidBrush(dgv.ColumnHeadersDefaultCellStyle.BackColor),
         e.CellBounds)
        If img IsNot Nothing Then
            Select Case Style
                Case DGVHeaderImageAlignments.FillCell
                    gr.DrawImage(
                     img, e.CellBounds.X, e.CellBounds.Y,
                     e.CellBounds.Width, e.CellBounds.Height)
                Case DGVHeaderImageAlignments.SingleCentered
                    gr.DrawImage(img,
                     ((e.CellBounds.Width - img.Width) \ 2) + e.CellBounds.X,
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y,
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleLeft
                    gr.DrawImage(img, e.CellBounds.X,
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y,
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleRight
                    gr.DrawImage(img,
                     (e.CellBounds.Width - img.Width) + e.CellBounds.X,
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y,
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.Tile
                    ' ********************************************************
                    ' To correct: It sould display just a stripe of images,
                    ' long as the whole header, but centered in the header's
                    ' height.
                    ' This code WON'T WORK.
                    ' Any one got any better solution?
                    'Dim rect As New Rectangle(e.CellBounds.X, _
                    ' ((e.CellBounds.Height - img.Height) \ 2), _
                    ' e.ClipBounds.Width, _
                    ' ((e.CellBounds.Height \ 2 + img.Height \ 2)))
                    'Dim br As New TextureBrush(img, Drawing2D.WrapMode.Tile, _
                    ' rect)
                    ' ********************************************************
                    ' This one works... but poorly (the image is repeated
                    ' vertically, too).
                    Dim br As New TextureBrush(img, Drawing2D.WrapMode.Tile)
                    gr.FillRectangle(br, e.ClipBounds)
                Case Else
                    gr.DrawImage(
                     img, e.CellBounds.X, e.CellBounds.Y,
                     e.ClipBounds.Width, e.CellBounds.Height)
            End Select
        End If

        If e.Value Is Nothing Then
            e.Handled = True
            Return
        End If
        Using sf As New StringFormat
            With sf
                Select Case dgv.ColumnHeadersDefaultCellStyle.Alignment
                    Case DataGridViewContentAlignment.BottomCenter
                        .Alignment = StringAlignment.Center
                        .LineAlignment = StringAlignment.Far
                    Case DataGridViewContentAlignment.BottomLeft
                        .Alignment = StringAlignment.Near
                        .LineAlignment = StringAlignment.Far
                    Case DataGridViewContentAlignment.BottomRight
                        .Alignment = StringAlignment.Far
                        .LineAlignment = StringAlignment.Far
                    Case DataGridViewContentAlignment.MiddleCenter
                        .Alignment = StringAlignment.Center
                        .LineAlignment = StringAlignment.Center
                    Case DataGridViewContentAlignment.MiddleLeft
                        .Alignment = StringAlignment.Near
                        .LineAlignment = StringAlignment.Center
                    Case DataGridViewContentAlignment.MiddleRight
                        .Alignment = StringAlignment.Far
                        .LineAlignment = StringAlignment.Center
                    Case DataGridViewContentAlignment.TopCenter
                        .Alignment = StringAlignment.Center
                        .LineAlignment = StringAlignment.Near
                    Case DataGridViewContentAlignment.TopLeft
                        .Alignment = StringAlignment.Near
                        .LineAlignment = StringAlignment.Near
                    Case DataGridViewContentAlignment.TopRight
                        .Alignment = StringAlignment.Far
                        .LineAlignment = StringAlignment.Near
                End Select

                .HotkeyPrefix = Drawing.Text.HotkeyPrefix.None
                .Trimming = StringTrimming.None
            End With

            Dim newFont = New System.Drawing.Font("Segoe UI", 9.75!, FontStyle.Bold)

            Dim newForeColor = Color.FromArgb(0, 0, 0)

            With dgv.ColumnHeadersDefaultCellStyle
                gr.DrawString(e.Value.ToString, newFont,
                 New SolidBrush(newForeColor), e.CellBounds, sf)
            End With
        End Using
        e.Handled = True
    End Sub

    Private Sub dgvpayrate_Leave(sender As Object, e As EventArgs) Handles dgvpayrate.Leave
        If dgvpayrate.RowCount <> 0 Then
            If dgvisLlostfocus = 1 Then
                If CellIndexLeave = -1 Then
                Else
                    dgvpayrate.Columns(CellIndexLeave).Width = 230
                End If

                If RowIndexLeave = -1 Then
                Else
                    dgvpayrate.Rows(RowIndexLeave).Height = 295
                End If
            Else
                Dim dgvcellargs As New DataGridViewCellEventArgs(dgvpayrate.CurrentCell.ColumnIndex,
                                                                 dgvpayrate.CurrentRow.Index)

                DataGridView1_RowLeave(sender, dgvcellargs)
                DataGridView1_CellLeave(sender, dgvcellargs)
            End If
        Else
            dgvisLlostfocus = 0
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim numberone = 1

        If numberone = 1 Then
            Exit Sub
        End If

        Dim startyear = 2020

        For ii = 0 To 10

            Dim countofdays As Object = EXECQUER("SELECT DAYOFYEAR(DATE_SUB(LAST_DAY('2020-12-01'), INTERVAL " & ii & " YEAR));")

            Dim dateforyear As String = "'" & startyear & "-01-01'"

            For i = 1 To Val(countofdays)
                Dim prateDate As Object = EXECQUER("SELECT DATE_FORMAT(MAKEDATE(YEAR(" & dateforyear & ")," & i & "),'%Y-%m-%d');")

                INSUPD_payrate(, prateDate, , , 1.0, 1.25, 1.1, 1.21, 1.3, 1.69)

            Next

            startyear += 1

        Next

        MsgBox("Done inserting payrates.", MsgBoxStyle.Information, "Finish")
    End Sub

    Private Sub Panel2_Enter(sender As Object, e As EventArgs) Handles Panel2.Enter
        Static once As SByte = 0

        If once = 0 Then

            once = 1

            For Each objctrl As Control In Panel2.Controls
                If TypeOf objctrl Is TextBox Or
                    TypeOf objctrl Is ComboBox Then
                    OjbAssignNoContextMenu(objctrl)
                Else
                    Continue For
                End If
            Next

            cbopaytype.ContextMenu = New ContextMenu
        End If
    End Sub

End Class