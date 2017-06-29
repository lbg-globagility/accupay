Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient

Public Class TimeEntrySummary
    Public q_employee As String = "SELECT e.RowID," & _
            "e.EmployeeID 'Employee ID'," & _
            "e.FirstName 'First Name'," & _
            "e.MiddleName 'Middle Name'," & _
            "e.LastName 'Last Name'," & _
            "e.Surname," & _
            "e.Nickname," & _
            "e.MaritalStatus 'Marital Status'," & _
            "COALESCE(e.NoOfDependents,0) 'No. Of Dependents'," & _
            "DATE_FORMAT(e.Birthdate,'%m-%d-%Y') 'Birthdate'," & _
            "DATE_FORMAT(e.Startdate,'%m-%d-%Y') 'Startdate'," & _
            "e.JobTitle 'Job Title'," & _
            "COALESCE(pos.PositionName,'') 'Position'," & _
            "e.Salutation," & _
            "e.TINNo 'TIN'," & _
            "e.SSSNo 'SSS No.'," & _
            "e.HDMFNo 'PAGIBIG No.'," & _
            "e.PhilHealthNo 'PhilHealth No.'," & _
            "e.WorkPhone 'Work Phone No.'," & _
            "e.HomePhone 'Home Phone No.'," & _
            "e.MobilePhone 'Mobile Phone No.'," & _
            "e.HomeAddress 'Home address'," & _
            "e.EmailAddress 'Email address'," & _
            "IF(e.Gender='M','Male','Female') 'Gender'," & _
            "e.EmploymentStatus 'Employment Status'," & _
            "IFNULL(pf.PayFrequencyType,'') 'Pay Frequency'," & _
            "e.UndertimeOverride," & _
            "e.OvertimeOverride," & _
            "DATE_FORMAT(e.Created,'%m-%d-%Y') 'Creation Date'," & _
            "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by'," & _
            "COALESCE(DATE_FORMAT(e.LastUpd,'%m-%d-%Y'),'') 'Last Update'," & _
            "IFNULL((SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM user WHERE RowID=e.LastUpdBy),'') 'LastUpdate by'" & _
            ",COALESCE(pos.RowID,'') 'PositionID'" & _
            ",IFNULL(e.PayFrequencyID,'') 'PayFrequencyID'" & _
            ",e.EmployeeType" & _
            ",e.LeaveBalance" & _
            ",e.SickLeaveBalance" & _
            ",e.MaternityLeaveBalance" & _
            ",e.LeaveAllowance" & _
            ",e.SickLeaveAllowance" & _
            ",e.MaternityLeaveAllowance" & _
            ",e.LeavePerPayPeriod" & _
            ",e.SickLeavePerPayPeriod" & _
            ",e.MaternityLeavePerPayPeriod" & _
            ",COALESCE(fstat.RowID,3) 'fstatRowID'" & _
            " " & _
            "FROM employee e " & _
            "LEFT JOIN user u ON e.CreatedBy=u.RowID " & _
            "LEFT JOIN position pos ON e.PositionID=pos.RowID " & _
            "LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID " & _
            "LEFT JOIN filingstatus fstat ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents " & _
            "WHERE e.OrganizationID=" & orgztnID

    Public Simple As New AutoCompleteStringCollection

    Public currPayrateID, _
            currPayrate, _
            Dscrptn, _
            PayType, _
            OTRate, _
            NightDiffRate, _
            NightDiffOTRate, _
            RestDayRate, _
            RestDayOvertimeRate, _
            defaultViewDate, _
            TotalDayPay, _
            TotalDayPayNight, _
            u_nem As String

    Dim editedpayrate As New AutoCompleteStringCollection

    Dim tbleditedpayrate As New DataTable

    Dim tbleditedetent As New DataTable

    Dim dattab As New DataTable

    Dim view_ID As Integer = Nothing

    Public today_date As Object

    Dim DGVCellStyle As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()

    Protected Overrides Sub OnLoad(e As EventArgs)
        'DataGridViewCellStyle { BackColor=Color [Window], ForeColor=Color [ControlText]
        ', SelectionBackColor=Color [Highlight], SelectionForeColor=Color [HighlightText]
        ', Font=[Font: Name=Microsoft Sans Serif, Size=8.25, Units=3, GdiCharSet=0, GdiVerticalFont=False], WrapMode=False, Alignment=MiddleLeft }

        With DGVCellStyle
            .Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ForeColor = System.Drawing.Color.DimGray

        End With

        MyBase.OnLoad(e)
    End Sub

    Private Sub EmpTimeEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        new_conn.ConnectionString = db_connectinstring 'conn.ConnectionString

        today_date = EXECQUER("SELECT DATE_FORMAT(NOW(),'%m-%d-%Y');")

        curr_dd = Format(CDate(today_date), "dd")
        curr_mm = Format(CDate(today_date), "MM")
        curr_YYYY = Format(CDate(today_date), "yyyy")

        'viewID = VIEW_privilege(Me.Text)
        loademployees()

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

        End With

        u_nem = EXECQUER(USERNameStrPropr & 1)

        addhandlr()

        'dgvEmp_SelectionChanged(sender, e)

        'cbomonth.Items.Clear()
        'For i = 0 To 11
        '    Dim date_month = EXECQUER("SELECT DATE_FORMAT(DATE_ADD(MAKEDATE(YEAR(NOW()),1), INTERVAL " & i & " MONTH),'%M');")
        '    cbomonth.Items.Add(date_month)
        'Next

        '*****************************************

        'Dim args As ArgumentType = New ArgumentType()
        'args._a = 5
        'args._b = 6
        '' Start up the BackgroundWorker1.
        '' ... Pass argument object to it.
        'BackgroundWorker1.RunWorkerAsync
        txtshftEffFrom.Focus()

        view_ID = VIEW_privilege("Employee Time Entry", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then

            tsbtnNewtimeent.Visible = 0
            tsbtnSavetimeent.Visible = 0
            tsbtnrecalc.Visible = 0

            dontUpdate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    tsbtnNewtimeent.Visible = 0
                    tsbtnSavetimeent.Visible = 0
                    tsbtnrecalc.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        tsbtnSavetimeent.Visible = 0
                        tsbtnrecalc.Visible = 0
                    Else
                        tsbtnSavetimeent.Visible = 1
                        tsbtnrecalc.Visible = 1
                    End If

                    'If drow("Deleting").ToString = "N" Then
                    '    tsbtndel.Visible = 0
                    'Else
                    '    tsbtndel.Visible = 1
                    'End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

    End Sub

    Sub loadpayrate(Optional prate_date As Object = Nothing)

        If prate_date = Nothing Then

            defaultViewDate = EXECQUER("SELECT DATE_FORMAT(NOW(),'%M, %Y');")

            'Label1.Text = defaultViewDate

            dgvcalendar.Rows.Clear()

            dattab = retAsDatTbl("SELECT prate.RowID,DAY(prate.Date) 'dateday'" & _
                                 ", DAYOFWEEK(prate.Date) 'dayofwk'" & _
                                 ", DAY(LAST_DAY(prate.Date)) 'maxday'" & _
                                 ",COALESCE(prate.PayType,'') 'PayType'" & _
                                 ",COALESCE(prate.Description,'') 'Description'" & _
                                 ",COALESCE(prate.PayRate,1) 'PayRate'" & _
                                 ",COALESCE(prate.OvertimeRate,1) 'OvertimeRate'" & _
                                 ",COALESCE(prate.NightDifferentialRate,1) 'NightDifferentialRate'" & _
                                 ",COALESCE(prate.NightDifferentialOTRate,1) 'NightDifferentialOTRate'" & _
                                 ",COALESCE(prate.RestDayRate,1) 'RestDayRate'" & _
                                 ",COALESCE(prate.RestDayOvertimeRate,1) 'RestDayOvertimeRate'" & _
                                 ",DATE_FORMAT(prate.Date,'%m-%d-%Y') 'Date'" & _
                                 " FROM payrate prate" & _
                                 " WHERE prate.OrganizationID='" & orgztnID & "'" & _
                                 " AND MONTH(prate.Date)=MONTH(NOW())" & _
                                 " AND YEAR(prate.Date)=YEAR(NOW())" & _
                                 " AND DAY(prate.Date)<=(SELECT IF(DAY(NOW())<=15,15,DAY(LAST_DAY(NOW()))));")

            'DISTINCT(DATE_FORMAT(DATE,'%m-%d-%Y')),
            Dim countofweek As Integer = 0

            Dim cindx As Integer = 0

            countofweek = 0

            Dim countofweeks = dattab.Compute("COUNT(dayofwk)", "dayofwk=7")

            countofweek = countofweeks '.ToString)

            'For Each drow As DataRow In dattab.Rows
            '    If Val(drow("dayofwk")) = 7 Then
            '        countofweek += 1
            '    End If
            'Next

            For i = 0 To countofweek
                dgvcalendar.Rows.Add()
            Next

            For Each dgvr As DataGridViewRow In dgvcalendar.Rows
                For Each c As DataGridViewColumn In dgvcalendar.Columns
                    If dattab.Rows.Count <> cindx Then
                        If c.Index + 1 >= Val(dattab.Rows(cindx)("dayofwk")) Then
                            dgvcalendar.Item(c.Name, dgvr.Index).Value = dattab.Rows(cindx)("dateday") 'cindx + 1
                            cindx += 1
                        End If
                    End If
                Next
            Next

        Else

            Dim querdate As Object = Format(CDate(prate_date), "yyyy-MM-dd")

            defaultViewDate = Format(CDate(prate_date), "MMMM, yyyy")
            'Label1.Text = Format(CDate(prate_date), "MMMM dd, yyyy")

            If curr_dd - 15 <= 0 Then
                dattab = retAsDatTbl("SELECT prate.RowID,DAY(prate.Date) 'dateday', DAYOFWEEK(prate.Date) 'dayofwk', DAY(LAST_DAY(prate.Date)) 'maxday',COALESCE(prate.PayType,'') 'PayType',COALESCE(prate.Description,'') 'Description',COALESCE(prate.PayRate,1) 'PayRate',COALESCE(prate.OvertimeRate,1) 'OvertimeRate',COALESCE(prate.NightDifferentialRate,1) 'NightDifferentialRate',COALESCE(prate.NightDifferentialOTRate,1) 'NightDifferentialOTRate',COALESCE(prate.RestDayRate,1) 'RestDayRate',COALESCE(prate.RestDayOvertimeRate,1) 'RestDayOvertimeRate',DATE_FORMAT(prate.Date,'%m-%d-%Y') 'Date' FROM payrate prate WHERE MONTH(prate.Date)=MONTH('" & querdate & "') AND YEAR(prate.Date)=YEAR('" & querdate & "') AND DAY(prate.Date)<=15 AND prate.OrganizationID='" & orgztnID & "';")
            Else
                dattab = retAsDatTbl("SELECT prate.RowID,DAY(prate.Date) 'dateday', DAYOFWEEK(prate.Date) 'dayofwk', DAY(LAST_DAY(prate.Date)) 'maxday',COALESCE(prate.PayType,'') 'PayType',COALESCE(prate.Description,'') 'Description',COALESCE(prate.PayRate,1) 'PayRate',COALESCE(prate.OvertimeRate,1) 'OvertimeRate',COALESCE(prate.NightDifferentialRate,1) 'NightDifferentialRate',COALESCE(prate.NightDifferentialOTRate,1) 'NightDifferentialOTRate',COALESCE(prate.RestDayRate,1) 'RestDayRate',COALESCE(prate.RestDayOvertimeRate,1) 'RestDayOvertimeRate',DATE_FORMAT(prate.Date,'%m-%d-%Y') 'Date' FROM payrate prate WHERE MONTH(prate.Date)=MONTH('" & querdate & "') AND YEAR(prate.Date)=YEAR('" & querdate & "') AND DAY(prate.Date)>15 AND prate.OrganizationID='" & orgztnID & "';")
            End If
            'dattab = retAsDatTbl("SELECT prate.RowID,DAY(prate.Date) 'dateday', DAYOFWEEK(prate.Date) 'dayofwk', DAY(LAST_DAY(prate.Date)) 'maxday',COALESCE(prate.PayType,'') 'PayType',COALESCE(prate.Description,'') 'Description',COALESCE(prate.PayRate,1) 'PayRate',COALESCE(prate.OvertimeRate,1) 'OvertimeRate',COALESCE(prate.NightDifferentialRate,1) 'NightDifferentialRate',COALESCE(prate.NightDifferentialOTRate,1) 'NightDifferentialOTRate',DATE_FORMAT(prate.Date,'%m-%d-%Y') 'Date' FROM payrate prate WHERE MONTH(prate.Date)=MONTH('" & querdate & "') AND YEAR(prate.Date)=YEAR('" & querdate & "');")
            'DISTINCT(DATE_FORMAT(DATE,'%m-%d-%Y')),
            If dattab.Rows.Count <> 0 Then

                dgvcalendar.Rows.Clear()

                Dim the_date As String = curr_dd 'StrReverse(getStrBetween(StrReverse(querdate.ToString), "", "-"))

                Dim countofweek As Integer = 0

                Dim cindx As Integer = 0

                Dim colmntosel, rowtosel As Integer

                countofweek = 0

                Dim countofweeks = dattab.Compute("COUNT(dayofwk)", "dayofwk=7")

                countofweek = countofweeks '.ToString)

                'For Each drow As DataRow In dattab.Rows
                '    If Val(drow("dayofwk")) = 7 Then
                '        countofweek += 1
                '    End If
                'Next

                For i = 0 To countofweek
                    dgvcalendar.Rows.Add()
                Next

                For Each dgvr As DataGridViewRow In dgvcalendar.Rows
                    For Each c As DataGridViewColumn In dgvcalendar.Columns
                        If dattab.Rows.Count <> cindx Then
                            If c.Index + 1 >= Val(dattab.Rows(cindx)("dayofwk")) Then
                                dgvcalendar.Item(c.Name, dgvr.Index).Value = dattab.Rows(cindx)("dateday") 'cindx + 1

                                If the_date = dattab.Rows(cindx)("dateday") Then 'cindx 
                                    colmntosel = c.Index
                                    rowtosel = dgvr.Index
                                End If
                                cindx += 1
                            End If
                        End If
                    Next
                Next

                dgvcalendar.Item(colmntosel, rowtosel).Selected = True

                Dim currColName As String = dgvcalendar.Columns(dgvcalendar.CurrentCell.ColumnIndex).Name
                ObjectFields(dgvcalendar, currColName)

            Else

                dgvcalendar.Rows.Clear()

            End If

        End If

        'For Each drow As DataRow In dattab.Rows

        'Next
        For Each dgvr As DataGridViewRow In dgvcalendar.Rows
            If dgvr.Cells("Column1").Value = Nothing _
           And dgvr.Cells("Column2").Value = Nothing _
           And dgvr.Cells("Column3").Value = Nothing _
           And dgvr.Cells("Column4").Value = Nothing _
           And dgvr.Cells("Column5").Value = Nothing _
           And dgvr.Cells("Column6").Value = Nothing _
           And dgvr.Cells("Column7").Value = Nothing Then

                dgvcalendar.Rows.Remove(dgvr)
                Exit For
            End If
        Next

        Static n As SByte = 0
        If n = 0 Then
            n = 1

            'dattab = retAsDatTbl("SELECT prate.RowID,DAY(prate.Date) 'dateday', DAYOFWEEK(prate.Date) 'dayofwk', DAY(LAST_DAY(prate.Date)) 'maxday',COALESCE(prate.PayType,'') 'PayType',COALESCE(prate.Description,'') 'Description',COALESCE(prate.PayRate,1) 'PayRate',COALESCE(prate.OvertimeRate,1) 'OvertimeRate',COALESCE(prate.NightDifferentialRate,1) 'NightDifferentialRate',COALESCE(prate.NightDifferentialOTRate,1) 'NightDifferentialOTRate',DATE_FORMAT(prate.Date,'%m-%d-%Y') 'Date' FROM payrate prate WHERE MONTH(prate.Date)=MONTH(NOW()) AND YEAR(prate.Date)=YEAR(NOW());")
            'DISTINCT(DATE_FORMAT(DATE,'%m-%d-%Y')),
        End If

    End Sub

    Sub loademployees(Optional searchquery As String = Nothing)
        Dim str_fullsearch As String = ""
        If searchquery = Nothing Then
            ''    'Dim param(2, 2) As Object

            ''    'param(0, 0) = "e_OrganizationID"
            ''    'param(1, 0) = "pagination"

            ''    'param(0, 1) = orgztnID
            ''    'param(1, 1) = pagination

            ''    'filltable(dgvEmployi, "VIEW_employee", param, 1)
            ''    'filltable(dgvEmployi, q_employee)
            ''    'filltable(dgvEmployi, "VIEW_employee", "e_OrganizationID", 1, 1)   

            ''    dgvRowAdder(q_employee & " ORDER BY e.RowID DESC LIMIT " & pagination & ",100;", dgvEmployi)
            str_fullsearch = q_employee & " ORDER BY e.RowID DESC LIMIT " & pagination & "," & page_limiter & ";"
        Else 'q_employee & 
            ''    dgvRowAdder(q_employee & searchquery & " ORDER BY e.RowID DESC", dgvEmployi) ', Simple)
            str_fullsearch = q_employee & searchquery & " ORDER BY e.RowID DESC LIMIT " & pagination & "," & page_limiter & ";"
        End If
        Dim catchdt = New SQLQueryToDatatable(str_fullsearch).ResultTable
        dgvEmployi.Rows.Clear()
        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvEmployi.Rows.Add(row_array)
        Next
        Static x As SByte = 0

        If x = 0 Then
            x = 1

            With dgvEmployi
                .Columns("cemp_RowID").Visible = 0
                .Columns("cemp_UndertimeOverride").Visible = 0
                .Columns("cemp_OvertimeOverride").Visible = 0
                .Columns("cemp_PositionID").Visible = 0
                .Columns("cemp_PayFrequencyID").Visible = 0
                .Columns("cemp_LeavePerPayPeriod").Visible = 0
                .Columns("cemp_LeaveBalance").Visible = 0
                .Columns("cemp_LeaveAllowance").Visible = 0
                '.Columns("Image").Visible = 0
                .Columns("cemp_fstatRowID").Visible = 0

                If .RowCount <> 0 Then
                    .Item("cemp_EmployeeID", 0).Selected = 1
                End If

                'For Each r As DataGridViewRow In .Rows
                '    empcolcount = 0
                '    For Each c As DataGridViewColumn In .Columns
                '        If c.Visible Then
                '            If TypeOf r.Cells(c.Index).Value Is Byte() Then
                '                Simple.Add("")
                '            Else
                '                Simple.Add(CStr(r.Cells(c.Index).Value))
                '            End If
                '            empcolcount += 1
                '        End If
                '    Next
                'Next

                For Each r As DataGridViewRow In dgvEmployi.Rows
                    empcolcount = 0
                    For Each c As DataGridViewColumn In dgvEmployi.Columns
                        If c.Visible Then
                            If TypeOf r.Cells(c.Index).Value Is Byte() Then
                                Simple.Add("")
                            Else
                                Simple.Add(CStr(r.Cells(c.Index).Value))
                            End If
                            empcolcount += 1
                        End If
                    Next
                Next

            End With

            'txtSimple.AutoCompleteCustomSource = Simple
            'txtSimple.AutoCompleteMode = AutoCompleteMode.Suggest
            'txtSimple.AutoCompleteSource = AutoCompleteSource.CustomSource

            'For Each s As String In Simple
            '    ComboBox1.Items.Add(s)
            'Next

        End If

    End Sub

    Sub addhandlr()

        AddHandler dgvcalendar.CurrentCellChanged, AddressOf dgvcalendar_CurrentCellChanged
        'AddHandler dgvcalendar.RowLeave, AddressOf dgvcalendar_RowLeave
        'AddHandler dgvcalendar.SelectionChanged, AddressOf dgvcalendar_SelectionChanged
        'AddHandler dgvcalendar.CellLeave, AddressOf dgvcalendar_CellLeave

        ''dgvcalendar_CurrentCellChanged(sendr, e)
        ''dgvcalendar_SelectionChanged(sendr, e)
    End Sub

    Sub rmvhandlr()

        RemoveHandler dgvcalendar.CurrentCellChanged, AddressOf dgvcalendar_CurrentCellChanged
        'RemoveHandler dgvcalendar.RowLeave, AddressOf dgvcalendar_RowLeave
        'RemoveHandler dgvcalendar.SelectionChanged, AddressOf dgvcalendar_SelectionChanged
        'RemoveHandler dgvcalendar.CellLeave, AddressOf dgvcalendar_CellLeave

    End Sub

    Private Sub dgvcalendar_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvcalendar.CellContentClick

    End Sub

    Dim curr_CalendCol As String

    Dim prevdgvrowetentsemimon As New DataGridViewRow

    Sub dgvcalendar_CurrentCellChanged(sender As Object, e As EventArgs) 'Handles dgvcalendar.CurrentCellChanged
        rmvhndlrTextChangeEditEvent()

        Static Emp_ID As Integer = -1
        ''curr_mm'curr_YYYY

        txthrsworkd.Text = ""
        txtreghrsworkd.Text = ""
        txtregpay.Text = ""
        txthrsOT.Text = ""
        txtotpay.Text = ""
        txthrsUT.Text = ""
        txtutamount.Text = ""
        txthrslate.Text = ""
        txtlateamount.Text = ""
        txtnightdiff.Text = ""
        txtnightdiffpay.Text = ""
        txtnightdiffOT.Text = ""
        txtnightdiffotpay.Text = ""
        txttotdaypay.Text = ""
        txtTimeIn.Text = ""
        txtTimeOut.Text = ""

        chkrest.Checked = False

        prevdgvrowetentsemimon = Nothing

        daterecalc = Nothing

        Dim txtobjlist = Panel1.Controls.OfType(Of TextBox).Where(Function(t) Convert.ToString(t.Tag) = "Numeric")

        For Each txtobj As TextBox In txtobjlist
            txtobj.Clear()
            txtobj.Text = "0.00"
        Next

        Dim txtstringobjlist = Panel1.Controls.OfType(Of TextBox).Where(Function(t) Convert.ToString(t.Tag) = "String")

        For Each txtobj As TextBox In txtstringobjlist
            txtobj.Clear()
        Next

        If dgvcalendar.RowCount <> 0 And dgvEmployi.RowCount <> 0 Then

            Dim currColName As String = Nothing

            Try

                currColName = dgvcalendar.Columns(dgvcalendar.CurrentCell.ColumnIndex).Name
                curr_dd = dgvcalendar.CurrentRow.Cells(currColName).Value
            Catch ex As Exception
                currColName = "Column1"
            End Try

            'If dgvEmployi.RowCount <> 0 _
            '           And curr_dd <> Nothing _
            '           And dgvetentsemimon.RowCount = 0 Then

            '    VIEW_employeetimeentry_SEMIMON(dgvEmployi.CurrentRow.Cells("RowID").Value, _
            '                                   curr_YYYY & "-" & curr_mm & "-" & curr_dd)
            '    'employeetimeentry

            'End If

            curr_CalendCol = currColName
            'Label1.Text = defaultViewDate.Insert(defaultViewDate.IndexOf(","), _
            '                                 " " & dgvcalendar.CurrentRow.Cells(currColName).Value)


            'curr_mm = Format(CDate(DateTimePicker1.Value), "MM")
            'curr_YYYY = DateTimePicker1.Value.Year 'Format(CDate(DateTimePicker1.Value), "YYYY")

            'dgvEmployi_SelectionChanged(sender, e)

            hideObjFields()

            'If Not bgwpayrate.IsBusy Then
            '    bgwpayrate.RunWorkerAsync()
            'End If

            currPayrateID = Nothing
            Dscrptn = Nothing
            PayType = Nothing

            currPayrate = Nothing
            OTRate = Nothing

            NightDiffRate = Nothing
            NightDiffOTRate = Nothing

            RestDayRate = Nothing
            RestDayOvertimeRate = Nothing

            TotalDayPay = Nothing
            TotalDayPayNight = Nothing

            txt_PayType.Text = ""
            txt_PayRate.Text = ""
            txt_OvertimeRate.Text = ""
            txt_NightDifferentialRate.Text = ""
            txt_NightDifferentialOTRate.Text = ""
            txt_RestDayRate.Text = ""
            txt_RestDayOvertimeRate.Text = ""

            Try
                If dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value <> Nothing Then

                    Dim rowpayrate() As DataRow = dattab.Select("dateday='" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value & "'")

                    For Each drow In rowpayrate
                        currPayrateID = drow("RowID")
                        Dscrptn = drow("Description")
                        PayType = drow("PayType")

                        currPayrate = drow("PayRate")
                        OTRate = drow("OvertimeRate")

                        NightDiffRate = drow("NightDifferentialRate")
                        NightDiffOTRate = drow("NightDifferentialOTRate")

                        RestDayRate = drow("RestDayRate")
                        RestDayOvertimeRate = drow("RestDayOvertimeRate")

                        TotalDayPay = drow("PayRate") + drow("OvertimeRate")
                        TotalDayPayNight = drow("NightDifferentialRate") + drow("NightDifferentialOTRate")

                        txt_PayType.Text = Trim(drow("PayType"))

                        txt_PayRate.Text = FormatNumber(Val(drow("PayRate")), 2)

                        txt_OvertimeRate.Text = FormatNumber(Val(drow("OvertimeRate")), 2)

                        txt_NightDifferentialRate.Text = FormatNumber(Val(drow("NightDifferentialRate")), 2)

                        txt_NightDifferentialOTRate.Text = FormatNumber(Val(drow("NightDifferentialOTRate")), 2)

                        txt_RestDayRate.Text = FormatNumber(Val(drow("RestDayRate")), 2)

                        txt_RestDayOvertimeRate.Text = FormatNumber(Val(drow("RestDayOvertimeRate")), 2)

                        Exit For
                    Next

                End If
            Catch ex As Exception

                'MsgBox(getErrExcptn(ex, Me.Name))

                Dim rowpayrate() As DataRow = dattab.Select("dateday='" & etent_day & "'")

                For Each drow In rowpayrate
                    currPayrateID = drow("RowID")
                    Dscrptn = drow("Description")
                    PayType = drow("PayType")

                    currPayrate = drow("PayRate")
                    OTRate = drow("OvertimeRate")

                    NightDiffRate = drow("NightDifferentialRate")
                    NightDiffOTRate = drow("NightDifferentialOTRate")

                    RestDayRate = drow("RestDayRate")
                    RestDayOvertimeRate = drow("RestDayOvertimeRate")

                    TotalDayPay = drow("PayRate") + drow("OvertimeRate")
                    TotalDayPayNight = drow("NightDifferentialRate") + drow("NightDifferentialOTRate")

                    Exit For
                Next

            Finally

                Dim is_restday = Nothing

                Try
                    EXECQUER("SELECT COALESCE(RestDay,0) FROM employeeshift WHERE EmployeeID='" & dgvEmployi.CurrentRow.Cells("cemp_RowID").Value & _
                                                   "' AND OrganizationID=" & orgztnID & _
                                                   " AND '" & curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value & _
                                                   "' BETWEEN EffectiveFrom AND COALESCE(EffectiveTo,EffectiveFrom) AND COALESCE(RestDay,0)=1 LIMIT 1;")
                Catch ex As Exception
                    EXECQUER("SELECT COALESCE(RestDay,0) FROM employeeshift WHERE EmployeeID='" & dgvEmployi.CurrentRow.Cells("cemp_RowID").Value & _
                                                   "' AND OrganizationID=" & orgztnID & _
                                                   " AND '" & curr_YYYY & "-" & curr_mm & "-01" & _
                                                   "' BETWEEN EffectiveFrom AND COALESCE(EffectiveTo,EffectiveFrom) AND COALESCE(RestDay,0)=1 LIMIT 1;")
                End Try

                daterecalc = curr_YYYY & "-" & curr_mm & "-" & If(Trim(dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value).Length = 1, _
                                                                  "0" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value, _
                                                                  dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value)

                'ORDER BY DATEDIFF('" & curr_YYYY & "-" & curr_mm & "-" & etent_day & "',EffectiveFrom)

                chkrest.Checked = If(is_restday = "", 0, If(Trim(is_restday) = "1", True, False))

            End Try


            'For Each drow As DataRow In dattab.Rows
            '    If dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value = drow("dateday") Then

            '        currPayrateID = drow("RowID")
            '        Dscrptn = drow("Description")
            '        PayType = drow("PayType")

            '        currPayrate = drow("PayRate")
            '        OTRate = drow("OvertimeRate")

            '        NightDiffRate = drow("NightDifferentialRate")
            '        NightDiffOTRate = drow("NightDifferentialOTRate")

            '        TotalDayPay = drow("PayRate") + drow("OvertimeRate")
            '        TotalDayPayNight = drow("NightDifferentialRate") + drow("NightDifferentialOTRate")

            '        'cbopaytype.Text = drow("PayType")
            '        'txtdescrptn.Text = drow("Description")
            '        'txtpayrate.Text = Val(drow("PayRate")) * 100
            '        'txtotrate.Text = Val(drow("OvertimeRate")) * 100
            '        'txtnightdiffrate.Text = Val(drow("NightDifferentialRate")) * 100
            '        'txtnightdiffotrate.Text = Val(drow("NightDifferentialOTRate")) * 100

            '        Exit For
            '    End If
            'Next

            If dgvEmployi.RowCount <> 0 _
                       And curr_dd <> Nothing Then

                'If Emp_ID <> Val(dgvEmployi.CurrentRow.Cells("RowID").Value) Then
                '    Emp_ID = Val(dgvEmployi.CurrentRow.Cells("RowID").Value)

                'End If

                'VIEW_employeetimeentry_SEMIMON(dgvEmployi.CurrentRow.Cells("RowID").Value, _
                '                               curr_YYYY & "-" & curr_mm & "-" & curr_dd)

                Dim currsel_day As String = If(curr_dd.ToString.Length = 1, "0" & curr_dd, curr_dd)
                curr_dd = currsel_day
                'Dim selrow() As DataRow = employeetimeentry.Select("Date='" & curr_YYYY & "-" & curr_mm & "-" & currsel_day & "'")

                'For Each drow As DataRow In employeetimeentry.Rows

                '    txthrsworkd.Text = drow("RegularHoursWorked")
                '    txthrsOT.Text = drow("OvertimeHoursWorked")
                '    txthrsUT.Text = drow("UndertimeHours")
                '    'txthrsabsent.Text = drow("")
                '    txtnightdiff.Text = drow("NightDifferentialHours")
                '    txtnightdiffOT.Text = drow("NightDifferentialOTHours")
                '    txthrslate.Text = drow("HoursLate")
                '    Exit For
                'Next


                'txtempbasicpay.Text = EXECQUER("SELECT COALESCE(SUM(BasicPay),0)" & _
                '                               " FROM employeesalary" & _
                '                               " WHERE EmployeeID='" & theEmployeeRowID & "'" & _
                '                               " AND OrganizationID=" & orgztnID & _
                '                               " AND '" & curr_YYYY & "-" & curr_mm & "-" & currsel_day & "'" & _
                '                               " BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d')))" & _
                '                               " AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH)))" & _
                '                               " AND DATEDIFF('" & curr_YYYY & "-" & curr_mm & "-" & currsel_day & "',EffectiveDateFrom) >= 0" & _
                '                               " ORDER BY DATEDIFF(DATE_FORMAT('" & curr_YYYY & "-" & curr_mm & "-" & currsel_day & "','%Y-%m-%d'),EffectiveDateFrom)" & _
                '                               " LIMIT 1;")


                Try
                    txtempbasicpay.Text = 0
                    'Dim currentSalary = EXECQUER("SELECT COALESCE(SUM(BasicPay),0)" & _
                    '                               " FROM employeesalary" & _
                    '                               " WHERE EmployeeID='" & theEmployeeRowID & "'" & _
                    '                               " AND OrganizationID=" & orgztnID & _
                    '                               " AND '" & curr_YYYY & "-" & curr_mm & "-" & currsel_day & "' BETWEEN EffectiveDateFrom AND IFNULL(EffectiveDateTo,'" & curr_YYYY & "-" & curr_mm & "-" & currsel_day & "')" & _
                    '                               " ORDER BY RowID DESC" & _
                    '                               " LIMIT 1;") 'EffectiveDateFrom'" AND EffectiveDateTo IS NULL" & _
                    ''" & curr_YYYY & "-" & curr_mm & "

                    'txtempbasicpay.Text = FormatNumber(Val(currentSalary), 2).ToString.Replace(",", "")

                Catch ex As Exception
                    txtempbasicpay.Text = "0.00"
                End Try

                Dim sel_dgvrow = dgvetentsemimon.Rows.
                    OfType(Of DataGridViewRow).
                    Where(Function(d) d.Cells("etiment_Date").Value =
                              String.Concat(curr_YYYY, "-", curr_mm, "-", currsel_day))

                For Each dgvrow As DataGridViewRow In sel_dgvrow 'dgvetentsemimon.Rows
                    'If dgvrow.Cells("etiment_Date").Value = curr_YYYY & "-" & curr_mm & "-" & currsel_day Then
                    'End If

                    With dgvrow

                        Dim theRowID = .Cells("etiment_RowID").Value & " " & _
                            .Cells("etiment_EmpID").Value & " " & _
                            .Cells("etiment_Date").Value

                        txthrsworkd.Text = .Cells("etiment_tothrsworked").Value
                        txttotdaypay.Text = FormatNumber(Val(.Cells("etiment_TotDayPay").Value), 2)

                        txtreghrsworkd.Text = .Cells("etiment_RegHrsWork").Value
                        txtregpay.Text = FormatNumber(Val(.Cells("etiment_reghrsamt").Value), 2)

                        txthrsOT.Text = .Cells("etiment_OTHrsWork").Value
                        txtotpay.Text = FormatNumber(Val(.Cells("etiment_otpay").Value), 2)

                        txthrsUT.Text = .Cells("etiment_UTHrsWork").Value
                        txtutamount.Text = FormatNumber(Val(.Cells("etiment_utamt").Value), 2)

                        txthrslate.Text = .Cells("etiment_Hrslate").Value
                        txtlateamount.Text = FormatNumber(Val(.Cells("etiment_lateamt").Value), 2)

                        txtnightdiff.Text = .Cells("etiment_NightDiffHrs").Value
                        txtnightdiffpay.Text = FormatNumber(Val(.Cells("etiment_nightdiffpay").Value), 2)

                        txtnightdiffOT.Text = .Cells("etiment_NightDiffOTHrs").Value
                        txtnightdiffotpay.Text = FormatNumber(Val(.Cells("etiment_nightdiffotpay").Value), 2)

                        txtshftTimeFrom.Text = .Cells("ete_DutyStartTime").Value

                        txtshftTimeTo.Text = .Cells("ete_DutyEndTime").Value

                        txtshftEffFrom.Text = .Cells("ShiftEffectiveDateFrom").Value

                        txtshftEffTo.Text = .Cells("ShiftEffectiveDateTo").Value

                        chkrest.Checked = Convert.ToInt16(.Cells("IsDayOfRest").Value)

                        txtempbasicpay.Text = .Cells("esBasicPay").Value

                        txtTimeIn.Text = .Cells("etiment_TimeIn").Value
                        txtTimeOut.Text = .Cells("etiment_TimeOut").Value

                        prevdgvrowetentsemimon = dgvrow

                    End With

                    Exit For

                Next

            Else

                Dim currentSalary = EXECQUER("SELECT COALESCE(SUM(BasicPay),0)" & _
                                               " FROM employeesalary" & _
                                               " WHERE EmployeeID='" & theEmployeeRowID & "'" & _
                                               " AND OrganizationID=" & orgztnID & _
                                               " AND EffectiveDateTo IS NULL" & _
                                               " ORDER BY RowID DESC" & _
                                               " LIMIT 1;")

                'txtempbasicpay.Text = FormatNumber(Val(currentSalary), 2).ToString.Replace(",", "")

            End If

            If currColName = "Column7" Then
                dgvcalendar.FirstDisplayedScrollingColumnIndex = dgvcalendar.Columns("Column7").Index

            End If

            ObjectFields(dgvcalendar, currColName)

        Else
            cbopaytype.SelectedIndex = -1
            txtdescrptn.Text = Nothing
            txtpayrate.Text = Nothing
            txtotrate.Text = Nothing
            txtnightdiffrate.Text = Nothing
            txtnightdiffotrate.Text = Nothing

            employeetimeentry.Rows.Clear()

            txtempbasicpay.Text = ""
            'Label1.Text = ""
        End If
        'myEllipseButton(dgvcalendar, currColName, TextBox1)
        addhndlrTextChangeEditEvent()

    End Sub

    Private Sub dgvcalendar_CellLeave(sender As Object, e As DataGridViewCellEventArgs) 'Handles dgvcalendar.CellLeave
        If dgvcalendar.RowCount <> 0 Then
            dgvcalendar.Columns(e.ColumnIndex).Width = 95
        End If
    End Sub

    Private Sub dgvcalendar_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvcalendar.SelectionChanged
        If dgvcalendar.RowCount <> 0 Then
            dgvcalendar.CurrentRow.Height = 230

            Dim currColName As String = dgvcalendar.Columns(dgvcalendar.CurrentCell.ColumnIndex).Name

            'Label1.Text = defaultViewDate.Insert(defaultViewDate.IndexOf(","), _
            '                                 " " & dgvcalendar.CurrentRow.Cells(currColName).Value)

            dgvcalendar.Columns(currColName).Width = 230

            If dgvcalendar.CurrentRow.Index = dgvcalendar.RowCount - 1 Then
                dgvcalendar.FirstDisplayedScrollingRowIndex = dgvcalendar.CurrentRow.Index
            End If

        End If
    End Sub

    Private Sub dgvcalendar_RowLeave(sender As Object, e As DataGridViewCellEventArgs) 'Handles dgvcalendar.RowLeave
        If dgvcalendar.RowCount <> 0 Then
            dgvcalendar.Rows(e.RowIndex).Height = 95
        End If
    End Sub

    Private Sub dgvcalendar_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvcalendar.ColumnWidthChanged
        If dgvcalendar.RowCount <> 0 Then
            Dim currColName As String = dgvcalendar.Columns(dgvcalendar.CurrentCell.ColumnIndex).Name
            ObjectFields(dgvcalendar, currColName)
        End If
    End Sub

    Sub addhndlrTextChangeEditEvent()
        'AddHandler cbopaytype.TextChanged, AddressOf ComboBox1_TextChanged
        'AddHandler txtdescrptn.TextChanged, AddressOf ComboBox1_TextChanged
        'AddHandler txtpayrate.TextChanged, AddressOf ComboBox1_TextChanged
        'AddHandler txtotrate.TextChanged, AddressOf ComboBox1_TextChanged
        'AddHandler txtnightdiffrate.TextChanged, AddressOf ComboBox1_TextChanged
        'AddHandler txtnightdiffotrate.TextChanged, AddressOf ComboBox1_TextChanged

        'AddHandler txtpayrate.Leave, AddressOf txt_Leave
        'AddHandler txtotrate.Leave, AddressOf txt_Leave
        'AddHandler txtnightdiffrate.Leave, AddressOf txt_Leave
        'AddHandler txtnightdiffotrate.Leave, AddressOf txt_Leave

    End Sub

    Sub rmvhndlrTextChangeEditEvent()
        'RemoveHandler cbopaytype.TextChanged, AddressOf ComboBox1_TextChanged
        'RemoveHandler txtdescrptn.TextChanged, AddressOf ComboBox1_TextChanged
        'RemoveHandler txtpayrate.TextChanged, AddressOf ComboBox1_TextChanged
        'RemoveHandler txtotrate.TextChanged, AddressOf ComboBox1_TextChanged
        'RemoveHandler txtnightdiffrate.TextChanged, AddressOf ComboBox1_TextChanged
        'RemoveHandler txtnightdiffotrate.TextChanged, AddressOf ComboBox1_TextChanged

        'RemoveHandler txtpayrate.Leave, AddressOf txt_Leave
        'RemoveHandler txtotrate.Leave, AddressOf txt_Leave
        'RemoveHandler txtnightdiffrate.Leave, AddressOf txt_Leave
        'RemoveHandler txtnightdiffotrate.Leave, AddressOf txt_Leave

    End Sub

    Dim searchdate As Object

    Sub hideObjFields()

        'cbopaytype.Visible = False
        'txtdescrptn.Visible = False
        'txtpayrate.Visible = False
        'txtotrate.Visible = False
        'txtnightdiffrate.Visible = False
        'txtnightdiffotrate.Visible = False

        'Label11.Visible = False
        'Label12.Visible = False
        'Label13.Visible = False
        'Label14.Visible = False
        'Label15.Visible = False
        'Label16.Visible = False

    End Sub

    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs) 'Handles ComboBox1.TextChanged, TextBox1.TextChanged, TextBox2.TextChanged, _
        '                                                               'TextBox3.TextChanged, TextBox4.TextChanged, TextBox6.TextChanged

        'Static x As SByte = 0
        'If x = 0 Then
        '    x = 1
        '    With tbleditedpayrate
        '        .Columns.Add("currPayrateID")
        '        .Columns.Add("currPayrate")
        '        .Columns.Add("Dscrptn")
        '        .Columns.Add("PayType")
        '        .Columns.Add("OTRate")
        '        .Columns.Add("NightDiffRate")
        '        .Columns.Add("NightDiffOTRate")

        '    End With
        'End If

        If dgvcalendar.RowCount <> 0 Then

            If dgvcalendar.CurrentRow.Cells(dgvcalendar.CurrentCell.ColumnIndex).Value <> Nothing Then
                For Each drow As DataRow In tbleditedpayrate.Rows
                    If currPayrateID = drow("currPayrateID") Then

                        tbleditedpayrate.Rows.Remove(drow)
                        Exit For
                    End If
                Next

                Dim nRow As DataRow
                nRow = tbleditedpayrate.NewRow

                nRow("currPayrateID") = currPayrateID
                nRow("Dscrptn") = Trim(txtdescrptn.Text)
                nRow("PayType") = Trim(cbopaytype.Text)
                nRow("currPayrate") = (Val(txtpayrate.Text) / 100) ' + 1
                nRow("OTRate") = (Val(txtotrate.Text) / 100) ' + 1
                nRow("NightDiffRate") = (Val(txtnightdiffrate.Text) / 100) ' + 1
                nRow("NightDiffOTRate") = (Val(txtnightdiffotrate.Text) / 100) ' + 1

                tbleditedpayrate.Rows.Add(nRow)
                'editedpayrate.Add(1)

            End If

        Else


        End If

    End Sub

    Sub ObjectFields(ByVal dgv As DataGridView, _
                        ByVal colName As String, _
                        Optional isVisb As SByte = 0)
        ''cbopaytype, _
        ''                       txtdescrptn, _
        ''                       txtpayrate, _
        ''                       txtotrate, _
        ''                       txtnightdiffrate, _
        ''                       txtnightdiffotrate)
        'Try
        '    If dgv.CurrentRow.Cells(colName).Selected Then

        '        'If dgv.Columns(colName).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells Then
        '        '    Dim wid As Integer = dgv.Columns(colName).Width

        '        '    Dim x As Integer = dgv.Columns(colName).Width + 25
        '        '    dgv.Columns(colName).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        '        '    dgv.Columns(colName).Width = x
        '        'End If

        '        Dim rect As Rectangle = dgv.GetCellDisplayRectangle(dgv.CurrentRow.Cells(colName).ColumnIndex, dgv.CurrentRow.Cells(colName).RowIndex, True)

        '        '*********************Pay Type***************************
        '        Label11.Parent = dgv
        '        Label11.Location = New Point(rect.Right - cbopaytype.Width, rect.Top + 5)
        '        Label11.Visible = If(isVisb = 0, True, False)

        '        cbopaytype.Parent = dgv '                   'btn.Width
        '        cbopaytype.Location = New Point(rect.Right - cbopaytype.Width, rect.Top + 18)
        '        cbopaytype.Visible = If(isVisb = 0, True, False)

        '        '********************Description****************************
        '        Label12.Parent = dgv
        '        Label12.Location = New Point(rect.Right - txtdescrptn.Width, rect.Top + 43)
        '        Label12.Visible = If(isVisb = 0, True, False)

        '        txtdescrptn.Parent = dgv
        '        txtdescrptn.Location = New Point(rect.Right - txtdescrptn.Width, rect.Top + 56)
        '        txtdescrptn.Visible = If(isVisb = 0, True, False)
        '        '**********************Pay Rate**************************
        '        Label13.Parent = dgv
        '        Label13.Location = New Point(rect.Right - txtpayrate.Width, rect.Top + 87) '77
        '        Label13.Visible = If(isVisb = 0, True, False)

        '        txtpayrate.Parent = dgv
        '        txtpayrate.Location = New Point(rect.Right - txtpayrate.Width, rect.Top + 100) '90
        '        txtpayrate.Visible = If(isVisb = 0, True, False)
        '        '***********************Overtime Rate*************************
        '        Label14.Parent = dgv
        '        Label14.Location = New Point(rect.Right - txtotrate.Width, rect.Top + 121) '111
        '        Label14.Visible = If(isVisb = 0, True, False)

        '        txtotrate.Parent = dgv
        '        txtotrate.Location = New Point(rect.Right - txtotrate.Width, rect.Top + 134) '124
        '        txtotrate.Visible = If(isVisb = 0, True, False)
        '        '**********************Night differential rate**************************
        '        Label15.Parent = dgv
        '        Label15.Location = New Point(rect.Right - txtnightdiffrate.Width, rect.Top + 155) '145
        '        Label15.Visible = If(isVisb = 0, True, False)

        '        txtnightdiffrate.Parent = dgv
        '        txtnightdiffrate.Location = New Point(rect.Right - txtnightdiffrate.Width, rect.Top + 168) '158
        '        txtnightdiffrate.Visible = If(isVisb = 0, True, False)
        '        '************************Night differential overtime rate************************
        '        Label16.Parent = dgv
        '        Label16.Location = New Point(rect.Right - txtnightdiffotrate.Width, rect.Top + 189) '179
        '        Label16.Visible = If(isVisb = 0, True, False)

        '        txtnightdiffotrate.Parent = dgv
        '        txtnightdiffotrate.Location = New Point(rect.Right - txtnightdiffotrate.Width, rect.Top + 202) '192
        '        txtnightdiffotrate.Visible = If(isVisb = 0, True, False)

        '    Else
        '        hideObjFields()
        '    End If
        'Catch ex As Exception
        '    'MsgBox(ex.Message & " ERR_NO 77-10 : myEllipseButton")
        'End Try
    End Sub

    Private Sub dgvcalendar_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvcalendar.Scroll
        If dgvcalendar.RowCount <> 0 Then
            dgvcalendar.Focus()
            Dim currColName As String = dgvcalendar.Columns(dgvcalendar.CurrentCell.ColumnIndex).Name
            ObjectFields(dgvcalendar, currColName)

            'dgvcalendar.FirstDisplayedScrollingColumnIndex
        End If
    End Sub

    Private Sub DGVCalendar_Leave(sender As Object, e As EventArgs) Handles dtppayperiod.GotFocus, ComboBox7.GotFocus, ComboBox8.GotFocus, _
                                                                            ComboBox9.GotFocus, ComboBox10.GotFocus, TextBox2.GotFocus, _
                                                                            TextBox15.GotFocus, TextBox16.GotFocus, TextBox17.GotFocus, _
                                                                            txtSimple.GotFocus, btnRerfresh.GotFocus, Button3.GotFocus, _
                                                                            dgvEmployi.GotFocus
        hideObjFields()
    End Sub

    Dim curr_mm, _
        curr_dd, _
        curr_YYYY As Object

    Sub dtppayperiod_ValueChanged(sender As Object, e As EventArgs) Handles dtppayperiod.ValueChanged

        Dim i = 1

        If i = 1 Then
            Exit Sub
        End If

        rmvhandlr()

        curr_dd = Format(CDate(dtppayperiod.Value), "dd")
        curr_mm = Format(CDate(dtppayperiod.Value), "MM")
        curr_YYYY = dtppayperiod.Value.Year 'Format(CDate(DateTimePicker1.Value), "YYYY")

        'MsgBox(curr_dd & " curr_dd" & vbNewLine & _
        '       curr_mm & " curr_mm" & vbNewLine & _
        '       curr_YYYY & " curr_YYYY")

        hideObjFields()
        Try
            'If Trim(cbomonth.Text) <> "" Then
            '    'TextBox5.Text = Format(CDate(TextBox5.Text), "MMMM yyyy")

            '    'searchdate = cbomonth.Text & " " & TextBox5.Text
            'searchdate = Format(CDate(DateTimePicker1.Value), "MMMM yyyy")
            searchdate = curr_YYYY & "-" & curr_mm & "-" & curr_dd
            loadpayrate(searchdate)
            '    'loadpayrate(TextBox5.Text)
            'Else
            '    loadpayrate()
            'End If

            If dgvEmployi.RowCount <> 0 _
                       And curr_dd <> Nothing Then

                VIEW_employeetimeentry_SEMIMON(dgvEmployi.CurrentRow.Cells("cemp_RowID").Value, _
                                               curr_YYYY & "-" & curr_mm & "-" & curr_dd)
                'employeetimeentry

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        Finally
            hideObjFields()
            dgvEmployi_SelectionChanged(sender, e)
            addhandlr()
        End Try
    End Sub

    Private Sub dgvEmployi_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmployi.CellClick
        Static once As SByte = 0

        If once = 0 And load_tbpemptimeent = 0 Then
            once = 1
            dgvEmployi_SelectionChanged(sender, e)
            tbpemptimeent_Enter(sender, e)
        End If

    End Sub

    Public currEShiftID, _
            currShiftID, _
            currNightEShiftID, _
            currNightShiftID, _
            curr_TimeFromMILIT, _
            curr_TimeToMILIT, _
            curr_NightTimeFromMILIT, _
            curr_NightTimeToMILIT, _
            curr_RegHrsWork As Object

    Public timeoutlastval As Object

    Dim dt_esh As DataTable

    Private Sub dgvEmployi_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmployi.CellContentClick

    End Sub

    Dim theEmployeeRowID = ""

    Sub dgvEmployi_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvEmployi.SelectionChanged
        'dgvEmp
        listOfEditetimeent.Clear()

        daterecalc = Nothing

        If TabControl1.SelectedIndex = 1 Then
            Exit Sub
        End If

        Static employeeRowID As String = -1
        Static staticDate As Object = -1
        'listofEditRow.Clear()

        If dgvEmployi.RowCount <> 0 Then
            Dim fullname = ""
            Dim subdetails = ""

            With dgvEmployi.CurrentRow
                theEmployeeRowID = .Cells("cemp_RowID").Value

                fullname = .Cells("cemp_FirstName").Value

                'fullname = fullname & If(.Cells("cemp_MiddleName").Value = Nothing, _
                '                                         "", _
                '                                         " " & StrConv(Microsoft.VisualBasic.Left(.Cells("cemp_MiddleName").Value.ToString, 1), _
                'VbStrConv.ProperCase) & ".")



                Dim addtlWord = Nothing

                If .Cells("cemp_MiddleName").Value = Nothing Then

                Else

                    Dim midNameTwoWords = Split(.Cells("cemp_MiddleName").Value.ToString, " ")

                    addtlWord = " "

                    For Each s In midNameTwoWords

                        addtlWord &= (StrConv(Microsoft.VisualBasic.Left(s, 1), VbStrConv.ProperCase) & ".")
                    Next

                    fullname &= addtlWord

                End If


                fullname = fullname & " " & .Cells("cemp_LastName").Value

                fullname = fullname & If(.Cells("cemp_SurName").Value = Nothing, _
                                                         "", _
                                                         "-" & StrConv(.Cells("cemp_SurName").Value, _
                                                                       VbStrConv.ProperCase))

                'Microsoft.VisualBasic.Left(.Cells("cemp_SurName").Value, 1)

                'Dim positionName = EXECQUER("SELECT PositionName FROM position WHERE RowID='" & .Cells("cemp_PositionID").Value & "';")
                '
                subdetails = "ID# " & .Cells("cemp_EmployeeID").Value & _
                            If(.Cells("cemp_Position").Value = Nothing, _
                                                               "", _
                                                               ", " & .Cells("cemp_Position").Value) & _
                            If(.Cells("cemp_EmployeeType").Value = Nothing, _
                                                               "", _
                                                               ", " & .Cells("cemp_EmployeeType").Value & " salary")

            End With

            txtFName.Text = fullname
            txtEmpID.Text = subdetails
            txtempbasicpay.Text = ""

            'Val(dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value) <> 0
            'If curr_CalendCol <> Nothing _
            If dgvcalendar.RowCount <> 0 Then

                Try
                    curr_CalendCol = dgvcalendar.Columns(dgvcalendar.CurrentCell.ColumnIndex).Name
                    staticDate = curr_YYYY & "-" & curr_mm & "-" & curr_dd
                    'staticDate = curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value
                Catch ex As Exception
                    curr_CalendCol = "Column1"
                End Try

                Try
                    curr_dd = dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value
                Catch ex As Exception
                    For Each colmn As DataGridViewColumn In dgvcalendar.Columns
                        If dgvcalendar.CurrentRow.Cells(colmn.Name).Value <> Nothing Then
                            curr_dd = dgvcalendar.CurrentRow.Cells(colmn.Name).Value
                            Exit For
                        End If
                    Next
                Finally
                    If curr_dd = Nothing Then
                        For Each colmn As DataGridViewColumn In dgvcalendar.Columns
                            If dgvcalendar.CurrentRow.Cells(colmn.Name).Value <> Nothing Then
                                curr_dd = dgvcalendar.CurrentRow.Cells(colmn.Name).Value
                                curr_dd = If(curr_dd.ToString.Length = 1, "0" & curr_dd, curr_dd)
                                Exit For
                            End If
                        Next
                    End If
                End Try

                'If employeeRowID <> dgvEmployi.CurrentRow.Cells("RowID").Value Then
                '    And staticDate <> curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value Then

                employeeRowID = dgvEmployi.CurrentRow.Cells("cemp_RowID").Value

                dt_esh = New DataTable
                '",COALESCE(DATE_FORMAT(esh.EffectiveFrom,'%m-%d-%Y'),MAKEDATE(YEAR(NOW()),1)) 'EffFrom'" & _
                '",COALESCE(DATE_FORMAT(esh.EffectiveTo,'%m-%d-%Y'),DATE_FORMAT(MAKEDATE(YEAR(NOW()),DAYOFYEAR(DATE(CONCAT(YEAR(NOW()),'-12-',DAY(LAST_DAY(DATE(CONCAT(YEAR(NOW()),'-12-00')))))))),'%m-%d-%Y')) 'EffTo'" & _
                dt_esh = retAsDatTbl("SELECT " & _
                                     "esh.RowID 'esh_RowID'" & _
                                     ",esh.EffectiveFrom AS EffFrom" &
                                     ",esh.EffectiveTo AS EffTo" &
                                     ",COALESCE(sh.RowID,'') 'sh_RowID'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%r'),'') 'sh_TimeFrom'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeTo,'%r'),'') 'sh_TimeTo'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%H:%i:%s'),'') 'sh_TimeFromMILIT'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeTo,'%H:%i:%s'),'') 'sh_TimeToMILIT'" & _
                                     ",COALESCE(esh.NightShift,'') 'NightShift'" & _
                                     ",COALESCE((CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(IF(sh.TimeFrom>sh.TimeTo,ADDTIME(sh.TimeTo, '24:00:00'),sh.TimeTo),'%H:%i:%s'),TIME_FORMAT(sh.TimeFrom,'%H:%i:%s')),':',1) AS INT)) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(IF(sh.TimeFrom>sh.TimeTo,ADDTIME(sh.TimeTo, '24:00:00'),sh.TimeTo),'%H:%i:%s'),TIME_FORMAT(sh.TimeFrom,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),0) 'RegularHrsWork'" & _
                                     " FROM employeeshift esh" & _
                                     " LEFT JOIN shift sh ON sh.RowID=esh.ShiftID" & _
                                     " WHERE esh.EmployeeID=" & dgvEmployi.CurrentRow.Cells("cemp_RowID").Value & _
                                     " AND esh.OrganizationID=" & orgztnID & _
                                     " AND COALESCE(esh.RestDay,0)=0" & _
                                     " AND CAST('" & curr_YYYY & "-" & curr_mm & "-" & curr_dd & "' AS DATE)" & _
                                     " BETWEEN COALESCE(esh.EffectiveFrom,DATE_ADD(DATE('" & curr_YYYY & "-" & curr_mm & "-" & curr_dd & "'), INTERVAL -1 MONTH))" & _
                                     " AND COALESCE(esh.EffectiveTo,DATE_ADD(DATE('" & curr_YYYY & "-" & curr_mm & "-" & curr_dd & "'), INTERVAL 1 MONTH))" & _
                                     " ORDER BY esh.EffectiveFrom DESC;")
                'dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value
                '*******KUNG MAY GRACE TIME, THE HERE...**********
                'SELECT ADDTIME('08:00:00', '00:15:00');' '00:15:00' sample ng grace time
                '*************************************************

                '*******AT SAKA KUNG MERON BANG ALLOWABLE MINUTE(S) LATE**********
                '*************************************************

                txtshftEffFrom.Text = ""
                txtshftEffTo.Text = ""
                txtshftTimeFrom.Text = ""
                txtshftTimeTo.Text = ""
                chknight.Checked = 0
                chkrest.Checked = 0

                currEShiftID = Nothing
                currShiftID = Nothing

                curr_TimeFromMILIT = Nothing
                curr_TimeToMILIT = Nothing

                currNightEShiftID = Nothing
                currNightShiftID = Nothing

                curr_NightTimeFromMILIT = Nothing
                curr_NightTimeToMILIT = Nothing

                curr_RegHrsWork = Nothing

                For Each drow As DataRow In dt_esh.Rows

                    If Val(drow("NightShift")) = 0 Then 'DAY SHIFT

                        currEShiftID = drow("esh_RowID")
                        currShiftID = drow("sh_RowID")

                        txtshftEffFrom.Text = Format(CDate(drow("EffFrom")), machineShortDateFormat)
                        txtshftEffTo.Text = Format(CDate(drow("EffTo")), machineShortDateFormat)
                        txtshftTimeFrom.Text = drow("sh_TimeFrom")
                        txtshftTimeTo.Text = drow("sh_TimeTo")

                        curr_TimeFromMILIT = drow("sh_TimeFromMILIT")
                        curr_TimeToMILIT = drow("sh_TimeToMILIT")

                        curr_RegHrsWork = drow("RegularHrsWork")

                        'isNightShift = 0 'DAY SHIFT
                        chknight.Checked = 0

                    Else '                              'NIGHT SHIFT

                        currNightEShiftID = drow("esh_RowID")
                        currNightShiftID = drow("sh_RowID")

                        txtshftEffFrom.Text = drow("EffFrom")
                        txtshftEffTo.Text = drow("EffTo")
                        txtshftTimeFrom.Text = drow("sh_TimeFrom")
                        txtshftTimeTo.Text = drow("sh_TimeTo")

                        curr_NightTimeFromMILIT = drow("sh_TimeFromMILIT")
                        curr_NightTimeToMILIT = drow("sh_TimeToMILIT")

                        curr_RegHrsWork = drow("RegularHrsWork")

                        chknight.Checked = 1
                    End If

                    Exit For

                Next

                'employeetimeentry
                'End If

                'Dim dt_etent As New DataTable

                'Dim hrswrkd As Double

                'Static firstrow As SByte = 0

                'timeoutlastval = Nothing

                'txthrsworkd.Text = 0
                'txthrsOT.Text = 0
                'txthrsUT.Text = 0

                'txthrsabsent.Text = 0
                'txtnightdiff.Text = 0
                'txtnightdiffOT.Text = 0

                'hrswrkd = 0

                'cbolateabsent.SelectedIndex = -1
                ''curr_TimeFromMILIT'curr_TimeToMILIT
                ''curr_NightTimeFromMILIT'curr_NightTimeToMILIT

                ''*******KUNG MAY GRACE TIME, THE HERE...**********
                ''SELECT ADDTIME('08:00:00', '00:15:00');' '00:15:00' sample ng grace time
                ''*************************************************

                'dt_etent = retAsDatTbl("SELECT RowID" & _
                '                       ", COALESCE(TIME_FORMAT(TimeIn,'%H:%i:%s'),'') 'TimeIn'" & _
                '                       ", COALESCE(TIME_FORMAT(TimeOut,'%H:%i:%s'),'') 'TimeOut'" & _
                '                       ", COALESCE(DATE_FORMAT(DATE,'%m-%d-%Y'),'') 'Date'" & _
                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLate'" & _
                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_TimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_TimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUT'" & _
                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',-2),':',1) AS DECIMAL) / 60),'') 'OT'" & _
                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),0) 'HRS_workd'" & _
                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLateNight'" & _
                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUTNight'" & _
                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'OTNight'" & _
                '                       " FROM employeetimeentrydetails" & _
                '                       " WHERE EmployeeID=" & dgvEmployi.CurrentRow.Cells("RowID").Value & _
                '                       " AND DATE= DATE('" & curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value & "') " & _
                '                       " AND OrganizationID=" & orgztnID & _
                '                       " ORDER BY RowID ASC;") '" ORDER BY DATE_FORMAT(Created,'%H') ASC;")

                'Dim UTval, _
                '    UTNightval, _
                '    OTval, _
                '    OTNightval, _
                '    Lateval As Double

                'For Each drows As DataRow In dt_etent.Rows

                '    hrswrkd += Val(drows("HRS_workd")) 'Total Hour(s) worked

                '    If firstrow = 0 Then
                '        firstrow = 1

                '        'txthrsUT.Text = UTval.ToString 'Total Undertime hour(s) 

                '        If Val(drows("IsLateNight")) <= 0 Then
                '            drows("IsLateNight") = drows("IsLateNight").ToString.Replace("-", "")
                '            Lateval = Val(drows("IsLateNight"))
                '        Else

                '            If drows("IsLate").ToString.Contains("-") Then
                '                'cbolateabsent.SelectedIndex = 1
                '                'drows("IsLate") = drows("IsLate").ToString.Replace("-", "")
                '                'Lateval = Val(drows("IsLate"))
                '            Else
                '                'cbolateabsent.SelectedIndex = -1
                '            End If

                '            'drows("IsLate") = drows("IsLate").ToString.Replace("-", "")
                '            'UTval = Val(drows("IsLate"))
                '        End If

                '    End If
                '    '*****************DAY SHIFT****************
                '    If drows("IsUT").ToString.Contains("-") Then
                '        drows("IsUT") = drows("IsUT").ToString.Replace("-", "")
                '        UTval = Val(drows("IsUT"))
                '    End If

                '    If drows("OT").ToString.Contains("-") = False Then
                '        drows("OT") = drows("OT").ToString.Replace("-", "")
                '        OTval += Val(drows("OT"))
                '    End If
                '    '*****************DAY SHIFT****************

                '    '*****************NIGHT SHIFT****************
                '    If drows("IsUTNight").ToString.Contains("-") Then
                '        drows("IsUTNight") = drows("IsUTNight").ToString.Replace("-", "")
                '        UTNightval = Val(drows("IsUTNight"))
                '    End If

                '    If drows("OT").ToString.Contains("-") Then
                '        drows("OTNight") = drows("OTNight").ToString.Replace("-", "")
                '        OTNightval += Val(drows("OTNight"))
                '    End If
                '    '*****************NIGHT SHIFT****************

                'Next

                'firstrow = 0

                'txthrsworkd.Text = hrswrkd

                'If curr_TimeFromMILIT = Nothing _
                '                    And curr_TimeToMILIT = Nothing Then 'SHIFT IS NOT DEFINED

                '    txthrsworkd.Text = hrswrkd + OTval

                '    If curr_NightTimeFromMILIT <> Nothing _
                '                    And curr_NightTimeToMILIT <> Nothing Then 'NIGHT SHIFT

                '        txthrsworkd.Text = hrswrkd

                '        txtnightdiffOT.Text = OTNightval

                '        txthrsUT.Text = UTNightval

                '    End If
                'ElseIf curr_TimeFromMILIT <> Nothing _
                '                    And curr_TimeToMILIT <> Nothing Then 'DAY SHIFT

                '    txthrsworkd.Text = hrswrkd

                '    txthrsOT.Text = OTval

                '    txthrsUT.Text = UTval

                'End If

                ''txthrsUT.Text = If(OTval > 0, 0, UTval)

                ''********************
                'cboOverUnderTime.SelectedIndex = If(OTval > 0 Or OTNightval > 0, 0, If(dt_etent.Rows.Count = 0, -1, 1))
                ''DEPENDE PA ANG OVER TIME, 
                ''EXAMPLE KUNG MAY APPROVAL PA BA? 
                ''NO OVER TIME
                '',KUNG MAY ALLOWABLE HOUR(S) FOR OVER TIME
                ''********************

            End If

            If dgvEmployi.RowCount <> 0 _
                       And curr_dd <> Nothing Then

                VIEW_employeetimeentry_SEMIMON(dgvEmployi.CurrentRow.Cells("cemp_RowID").Value, _
                                               curr_YYYY & "-" & curr_mm & "-" & curr_dd)
                'employeetimeentry

                If employeetimeentry_absent.Rows.Count <> 0 Then

                    For Each dgvrow As DataGridViewRow In dgvcalendar.Rows

                        For Each dgvcol As DataGridViewColumn In dgvcalendar.Columns

                            If dgvrow.Cells(dgvcol.Index).Value <> Nothing Then

                                Dim sel_employeetimeentry_absent = employeetimeentry_absent.Select("DateDay = '" & dgvrow.Cells(dgvcol.Index).Value & "'")

                                If sel_employeetimeentry_absent.Count <> 0 Then

                                    dgvrow.Cells(dgvcol.Index).Style.BackColor = Color.FromArgb(255, 94, 0)

                                    dgvrow.Cells(dgvcol.Index).Style.ForeColor = Color.White

                                    dgvrow.Cells(dgvcol.Index).Style.SelectionBackColor = Color.FromArgb(255, 158, 0)

                                Else

                                    dgvrow.Cells(dgvcol.Index).Style.BackColor = Color.White

                                    dgvrow.Cells(dgvcol.Index).Style.ForeColor = Color.DimGray

                                    dgvrow.Cells(dgvcol.Index).Style.SelectionBackColor = System.Drawing.SystemColors.Highlight

                                End If

                                Continue For

                            Else

                                dgvrow.Cells(dgvcol.Index).Style.BackColor = Color.White

                                dgvrow.Cells(dgvcol.Index).Style.ForeColor = Color.DimGray

                                dgvrow.Cells(dgvcol.Index).Style.SelectionBackColor = System.Drawing.SystemColors.Highlight

                                Continue For

                            End If

                        Next

                    Next

                Else

                    For Each dgvrow As DataGridViewRow In dgvcalendar.Rows

                        For Each dgvcol As DataGridViewColumn In dgvcalendar.Columns

                            dgvrow.Cells(dgvcol.Index).Style.BackColor = Color.White

                            dgvrow.Cells(dgvcol.Index).Style.ForeColor = Color.DimGray

                            dgvrow.Cells(dgvcol.Index).Style.SelectionBackColor = System.Drawing.SystemColors.Highlight

                            'DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
                            'DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
                            'DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
                            'DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
                            'DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
                            'DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
                            'DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]

                        Next

                    Next
                    'DataGridViewCellStyle { BackColor=Color [Window], ForeColor=Color [ControlText], SelectionBackColor=Color [Highlight], SelectionForeColor=Color [HighlightText], Font=[Font: Name=Microsoft Sans Serif, Size=8.25, Units=3, GdiCharSet=0, GdiVerticalFont=False], WrapMode=False, Alignment=MiddleLeft }

                End If

            End If

            dgvcalendar_CurrentCellChanged(sender, e)

        Else

            theEmployeeRowID = Nothing

            txtFName.Text = ""
            txtEmpID.Text = ""
            txtempbasicpay.Text = ""

            currEShiftID = Nothing
            currShiftID = Nothing

            txtshftEffFrom.Text = ""
            txtshftEffTo.Text = ""
            txtshftTimeFrom.Text = ""
            txtshftTimeTo.Text = ""

            txthrsworkd.Text = 0
            txthrsOT.Text = 0
            txthrsUT.Text = 0
            'txthrsabsent.Text = 0
            txtnightdiff.Text = 0
            txtnightdiffOT.Text = 0

        End If

    End Sub

#Region "FIRST dgvEmployi_SelectionChanged()"

    ''dgvEmp

    ''listofEditRow.Clear()

    '    If dgvEmployi.RowCount <> 0 Then
    ''Val(dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value) <> 0
    ''If curr_CalendCol <> Nothing _
    '        If dgvcalendar.RowCount <> 0 Then

    '            curr_CalendCol = dgvcalendar.Columns(dgvcalendar.CurrentCell.ColumnIndex).Name

    'Dim dt_esh As New DataTable

    '            dt_esh = retAsDatTbl("SELECT " & _
    '                                 "esh.RowID 'esh_RowID'" & _
    '                                 ",DATE_FORMAT(esh.EffectiveFrom,'%m-%d-%Y') 'EffFrom'" & _
    '                                 ",DATE_FORMAT(esh.EffectiveTo,'%m-%d-%Y') 'EffTo'" & _
    '                                 ",COALESCE(sh.RowID,'') 'sh_RowID'" & _
    '                                 ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%r'),'') 'sh_TimeFrom'" & _
    '                                 ",COALESCE(TIME_FORMAT(sh.TimeTo,'%r'),'') 'sh_TimeTo'" & _
    '                                 ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%H:%i:%s'),'') 'sh_TimeFromMILIT'" & _
    '                                 ",COALESCE(TIME_FORMAT(sh.TimeTo,'%H:%i:%s'),'') 'sh_TimeToMILIT'" & _
    '                                 ",COALESCE(esh.NightShift,'') 'NightShift'" & _
    '                                 " FROM employeeshift esh" & _
    '                                 " LEFT JOIN shift sh ON sh.RowID=esh.ShiftID" & _
    '                                 " WHERE esh.EmployeeID=" & dgvEmployi.CurrentRow.Cells("RowID").Value & _
    '                                 " AND esh.OrganizationID=" & orgztnID & _
    '                                 " AND CAST('" & curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value & "' AS DATE) BETWEEN esh.EffectiveFrom AND esh.EffectiveTo;")

    '            currEShiftID = ""
    '            currShiftID = ""

    '            txtshftEffFrom.Text = ""
    '            txtshftEffTo.Text = ""
    '            txtshftTimeFrom.Text = ""
    '            txtshftTimeTo.Text = ""

    '            curr_TimeFromMILIT = Nothing
    '            curr_TimeToMILIT = Nothing

    '            currNightEShiftID = Nothing
    '            currNightShiftID = Nothing

    '            curr_NightTimeFromMILIT = Nothing
    '            curr_NightTimeToMILIT = Nothing

    '            For Each drow As DataRow In dt_esh.Rows

    '                If Val(drow("NightShift")) = 0 Then

    '                    currEShiftID = drow("esh_RowID")
    '                    currShiftID = drow("sh_RowID")

    '                    txtshftEffFrom.Text = drow("EffFrom")
    '                    txtshftEffTo.Text = drow("EffTo")
    '                    txtshftTimeFrom.Text = drow("sh_TimeFrom")
    '                    txtshftTimeTo.Text = drow("sh_TimeTo")

    '                    curr_TimeFromMILIT = drow("sh_TimeFromMILIT")
    '                    curr_TimeToMILIT = drow("sh_TimeToMILIT")

    '                Else
    '                    currNightEShiftID = drow("esh_RowID")
    '                    currNightShiftID = drow("sh_RowID")

    '                    curr_NightTimeFromMILIT = drow("sh_TimeFromMILIT")
    '                    curr_NightTimeToMILIT = drow("sh_TimeToMILIT")

    '                End If

    '            Next

    'Dim dt_etent As New DataTable

    'Dim hrswrkd As Double

    'Static firstrow As SByte = 0

    '            timeoutlastval = Nothing

    '            txthrsworkd.Text = 0
    '            txthrsOT.Text = 0
    '            txthrsUT.Text = 0

    '            txthrsabsent.Text = 0
    '            txtnightdiff.Text = 0
    '            txtnightdiffOT.Text = 0

    '            hrswrkd = 0

    '            cbolateabsent.SelectedIndex = -1
    ''curr_TimeFromMILIT'curr_TimeToMILIT

    '            If curr_TimeFromMILIT <> Nothing And _
    '                                     curr_TimeToMILIT <> Nothing Then

    '                dt_etent = retAsDatTbl("SELECT " & _
    '                                       "TIME_FORMAT(TimeIn,'%H:%i:%s') 'TimeIn'" & _
    '                                       ",TIME_FORMAT(TimeOut,'%H:%i:%s') 'TimeOut'" & _
    '                                       ",DATE_FORMAT(DATE,'%m-%d-%Y') 'Date'" & _
    '                                       ",TIMEDIFF('" & curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')) 'IsLate'" & _
    '                                       ",TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_TimeToMILIT & "') 'IsUT'" & _
    '                                       ",SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) 'HR_workd'" & _
    '                                       ",SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) 'MIN_workd'" & _
    '                                       ",SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-1) 'SEC_workd'" & _
    '                                       " FROM employeetimeentrydetails" & _
    '                                       " WHERE EmployeeID=" & dgvEmployi.CurrentRow.Cells("RowID").Value & _
    '                                       " AND DATE= DATE('" & curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value & "') " & _
    '                                       " AND OrganizationID=" & orgztnID & _
    '                                       " ORDER BY TIME_FORMAT(TimeIn,'%H') ASC;")

    '                For Each drows As DataRow In dt_etent.Rows

    '                    hrswrkd += Val(drows("HR_workd")) 'Total Hour(s) worked
    '                    hrswrkd = hrswrkd + (Val(Val(drows("MIN_workd")) / 60)) 'Total Minute(s) worked

    '                    If firstrow = 0 Then
    '                        firstrow = 1
    '                        cbolateabsent.SelectedIndex = If(drows("IsLate").ToString.Contains("-"), _
    '                                                         1, _
    '                                                         -1)
    '                        txthrsUT.Text = 0

    'Dim f_loop As SByte = 0

    'Dim UTval As Double

    '                        drows("IsLate") = drows("IsLate").ToString.Replace("-", "")

    '                        For Each strtime In Split(drows("IsLate").ToString, ":")
    '                            If f_loop = 1 Then
    '                                UTval += (Val(Val(strtime) / 60)) 'Undertime minute(s) 
    '                            ElseIf f_loop = 0 Then
    '                                UTval += Val(strtime) 'Undertime hour(s) 
    '                            End If

    '                            f_loop += 1
    '                        Next

    '                        txthrsUT.Text = UTval.ToString 'Total Undertime hour(s) 

    '                    End If

    '                    timeoutlastval = drows("IsUT")

    '                Next

    '                txthrsworkd.Text = hrswrkd

    '                firstrow = 0

    '                cboOverUnderTime.SelectedIndex = -1

    '                If timeoutlastval <> Nothing Then
    '                    If timeoutlastval.ToString.Contains("-") Then 'this means undertime

    '                        txthrsOT.Text = ""
    '                        txthrsUT.Text = timeoutlastval.ToString
    '                        cboOverUnderTime.SelectedIndex = 1
    '                    Else '                                        'this means over time
    ''txthrsUT.Text = ""

    'Dim OTval As Double
    'Dim frstloop As SByte = 0
    '                        For Each intgr In Split(timeoutlastval.ToString, ":")
    '                            If frstloop = 1 Then
    '                                OTval += (Val(Val(intgr) / 60)) 'Overtime minute(s) worked
    ''this is the hour(s)
    '                            ElseIf frstloop = 0 Then
    '                                OTval += Val(intgr) 'Overtime hour(s) worked
    ''this is the minute(s) divided by 60 mins
    '                            End If
    ''MsgBox(intgr)

    '                            frstloop += 1
    '                        Next

    '                        cboOverUnderTime.SelectedIndex = 0

    '                        txthrsOT.Text = OTval 'Total Overtime hour(s) worked
    ''timeoutlastval.ToString
    '                    End If
    '                Else
    ''Label8.Text = "Is UT ?"
    '                End If

    '            End If

    '        End If

    '    Else

    '        currEShiftID = ""
    '        currShiftID = ""

    '        txtshftEffFrom.Text = ""
    '        txtshftEffTo.Text = ""
    '        txtshftTimeFrom.Text = ""
    '        txtshftTimeTo.Text = ""

    '        txthrsworkd.Text = 0
    '        txthrsOT.Text = 0
    '        txthrsUT.Text = 0
    '        txthrsabsent.Text = 0
    '        txtnightdiff.Text = 0
    '        txtnightdiffOT.Text = 0

    '    End If

#End Region

#Region "SECOND dgvEmployi_SelectionChanged()"

    'Private Sub dgvEmployi_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvEmployi.SelectionChanged
    '    'dgvEmp
    '    Static employeeRowID As String = -1
    '    Static staticDate As Object = -1
    '    'listofEditRow.Clear()

    '    If dgvEmployi.RowCount <> 0 Then
    '        'Val(dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value) <> 0
    '        'If curr_CalendCol <> Nothing _
    '        If dgvcalendar.RowCount <> 0 Then

    '            curr_CalendCol = dgvcalendar.Columns(dgvcalendar.CurrentCell.ColumnIndex).Name

    '            'If employeeRowID <> dgvEmployi.CurrentRow.Cells("RowID").Value Then
    '            '    And staticDate <> curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value Then

    '            employeeRowID = dgvEmployi.CurrentRow.Cells("RowID").Value
    '            staticDate = curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value

    '            dt_esh = New DataTable
    '            dt_esh = retAsDatTbl("SELECT " & _
    '                                 "esh.RowID 'esh_RowID'" & _
    '                                 ",COALESCE(DATE_FORMAT(esh.EffectiveFrom,'%m-%d-%Y'),'') 'EffFrom'" & _
    '                                 ",COALESCE(DATE_FORMAT(esh.EffectiveTo,'%m-%d-%Y'),'') 'EffTo'" & _
    '                                 ",COALESCE(sh.RowID,'') 'sh_RowID'" & _
    '                                 ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%r'),'') 'sh_TimeFrom'" & _
    '                                 ",COALESCE(TIME_FORMAT(sh.TimeTo,'%r'),'') 'sh_TimeTo'" & _
    '                                 ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%H:%i:%s'),'') 'sh_TimeFromMILIT'" & _
    '                                 ",COALESCE(TIME_FORMAT(sh.TimeTo,'%H:%i:%s'),'') 'sh_TimeToMILIT'" & _
    '                                 ",COALESCE(esh.NightShift,'') 'NightShift'" & _
    '                                 " FROM employeeshift esh" & _
    '                                 " LEFT JOIN shift sh ON sh.RowID=esh.ShiftID" & _
    '                                 " WHERE esh.EmployeeID=" & dgvEmployi.CurrentRow.Cells("RowID").Value & _
    '                                 " AND esh.OrganizationID=" & orgztnID & _
    '                                 " AND CAST('" & curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value & "' AS DATE) BETWEEN esh.EffectiveFrom AND esh.EffectiveTo;")

    '            '*******KUNG MAY GRACE TIME, THE HERE...**********
    '            'SELECT ADDTIME('08:00:00', '00:15:00');' '00:15:00' sample ng grace time
    '            '*************************************************
    '            currEShiftID = ""
    '            currShiftID = ""

    '            txtshftEffFrom.Text = ""
    '            txtshftEffTo.Text = ""
    '            txtshftTimeFrom.Text = ""
    '            txtshftTimeTo.Text = ""

    '            curr_TimeFromMILIT = Nothing
    '            curr_TimeToMILIT = Nothing

    '            currNightEShiftID = Nothing
    '            currNightShiftID = Nothing

    '            curr_NightTimeFromMILIT = Nothing
    '            curr_NightTimeToMILIT = Nothing

    '            For Each drow As DataRow In dt_esh.Rows

    '                If Val(drow("NightShift")) = 0 Then 'DAY SHIFT

    '                    currEShiftID = drow("esh_RowID")
    '                    currShiftID = drow("sh_RowID")

    '                    txtshftEffFrom.Text = drow("EffFrom")
    '                    txtshftEffTo.Text = drow("EffTo")
    '                    txtshftTimeFrom.Text = drow("sh_TimeFrom")
    '                    txtshftTimeTo.Text = drow("sh_TimeTo")

    '                    curr_TimeFromMILIT = drow("sh_TimeFromMILIT")
    '                    curr_TimeToMILIT = drow("sh_TimeToMILIT")

    '                    'isNightShift = 0 'DAY SHIFT

    '                Else '                              'NIGHT SHIFT

    '                    currNightEShiftID = drow("esh_RowID")
    '                    currNightShiftID = drow("sh_RowID")

    '                    txtshftEffFrom.Text = drow("EffFrom")
    '                    txtshftEffTo.Text = drow("EffTo")
    '                    txtshftTimeFrom.Text = drow("sh_TimeFrom")
    '                    txtshftTimeTo.Text = drow("sh_TimeTo")

    '                    curr_NightTimeFromMILIT = drow("sh_TimeFromMILIT")
    '                    curr_NightTimeToMILIT = drow("sh_TimeToMILIT")

    '                End If

    '            Next

    '            'End If

    '            Dim dt_etent As New DataTable

    '            Dim hrswrkd As Double

    '            Static firstrow As SByte = 0

    '            timeoutlastval = Nothing

    '            txthrsworkd.Text = 0
    '            txthrsOT.Text = 0
    '            txthrsUT.Text = 0

    '            txthrsabsent.Text = 0
    '            txtnightdiff.Text = 0
    '            txtnightdiffOT.Text = 0

    '            hrswrkd = 0

    '            cbolateabsent.SelectedIndex = -1
    '            'curr_TimeFromMILIT'curr_TimeToMILIT
    '            'curr_NightTimeFromMILIT'curr_NightTimeToMILIT

    '            '*******KUNG MAY GRACE TIME, THE HERE...**********
    '            'SELECT ADDTIME('08:00:00', '00:15:00');' '00:15:00' sample ng grace time
    '            '*************************************************

    '            dt_etent = retAsDatTbl("SELECT RowID" & _
    '                                   ", COALESCE(TIME_FORMAT(TimeIn,'%H:%i:%s'),'') 'TimeIn'" & _
    '                                   ", COALESCE(TIME_FORMAT(TimeOut,'%H:%i:%s'),'') 'TimeOut'" & _
    '                                   ", COALESCE(DATE_FORMAT(DATE,'%m-%d-%Y'),'') 'Date'" & _
    '                                   ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLate'" & _
    '                                   ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_TimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_TimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUT'" & _
    '                                   ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',-2),':',1) AS DECIMAL) / 60),'') 'OT'" & _
    '                                   ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),0) 'HRS_workd'" & _
    '                                   ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLateNight'" & _
    '                                   ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUTNight'" & _
    '                                   ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'OTNight'" & _
    '                                   " FROM employeetimeentrydetails" & _
    '                                   " WHERE EmployeeID=" & dgvEmployi.CurrentRow.Cells("RowID").Value & _
    '                                   " AND DATE= DATE('" & curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value & "') " & _
    '                                   " AND OrganizationID=" & orgztnID & _
    '                                   " ORDER BY RowID ASC;") '" ORDER BY DATE_FORMAT(Created,'%H') ASC;")

    '            Dim UTval, _
    '                UTNightval, _
    '                OTval, _
    '                OTNightval, _
    '                Lateval As Double

    '            For Each drows As DataRow In dt_etent.Rows

    '                hrswrkd += Val(drows("HRS_workd")) 'Total Hour(s) worked

    '                If firstrow = 0 Then
    '                    firstrow = 1

    '                    'txthrsUT.Text = UTval.ToString 'Total Undertime hour(s) 

    '                    If Val(drows("IsLateNight")) <= 0 Then
    '                        drows("IsLateNight") = drows("IsLateNight").ToString.Replace("-", "")
    '                        Lateval = Val(drows("IsLateNight"))
    '                    Else

    '                        If drows("IsLate").ToString.Contains("-") Then
    '                            'cbolateabsent.SelectedIndex = 1
    '                            'drows("IsLate") = drows("IsLate").ToString.Replace("-", "")
    '                            'Lateval = Val(drows("IsLate"))
    '                        Else
    '                            'cbolateabsent.SelectedIndex = -1
    '                        End If

    '                        'drows("IsLate") = drows("IsLate").ToString.Replace("-", "")
    '                        'UTval = Val(drows("IsLate"))
    '                    End If

    '                End If
    '                '*****************DAY SHIFT****************
    '                If drows("IsUT").ToString.Contains("-") Then
    '                    drows("IsUT") = drows("IsUT").ToString.Replace("-", "")
    '                    UTval = Val(drows("IsUT"))
    '                End If

    '                If drows("OT").ToString.Contains("-") = False Then
    '                    drows("OT") = drows("OT").ToString.Replace("-", "")
    '                    OTval += Val(drows("OT"))
    '                End If
    '                '*****************DAY SHIFT****************

    '                '*****************NIGHT SHIFT****************
    '                If drows("IsUTNight").ToString.Contains("-") Then
    '                    drows("IsUTNight") = drows("IsUTNight").ToString.Replace("-", "")
    '                    UTNightval = Val(drows("IsUTNight"))
    '                End If

    '                If drows("OT").ToString.Contains("-") Then
    '                    drows("OTNight") = drows("OTNight").ToString.Replace("-", "")
    '                    OTNightval += Val(drows("OTNight"))
    '                End If
    '                '*****************NIGHT SHIFT****************

    '            Next

    '            firstrow = 0

    '            txthrsworkd.Text = hrswrkd

    '            If curr_TimeFromMILIT = Nothing _
    '                                And curr_TimeToMILIT = Nothing Then 'SHIFT IS NOT DEFINED

    '                txthrsworkd.Text = hrswrkd + OTval

    '                If curr_NightTimeFromMILIT <> Nothing _
    '                                And curr_NightTimeToMILIT <> Nothing Then 'NIGHT SHIFT

    '                    txthrsworkd.Text = hrswrkd

    '                    txtnightdiffOT.Text = OTNightval

    '                    txthrsUT.Text = UTNightval

    '                End If
    '            ElseIf curr_TimeFromMILIT <> Nothing _
    '                                And curr_TimeToMILIT <> Nothing Then 'DAY SHIFT

    '                txthrsworkd.Text = hrswrkd

    '                txthrsOT.Text = OTval

    '                txthrsUT.Text = UTval

    '            End If

    '            'txthrsUT.Text = If(OTval > 0, 0, UTval)

    '            '********************
    '            cboOverUnderTime.SelectedIndex = If(OTval > 0 Or OTNightval > 0, 0, If(dt_etent.Rows.Count = 0, -1, 1))
    '            'DEPENDE PA ANG OVER TIME, 
    '            'EXAMPLE KUNG MAY APPROVAL PA BA? 
    '            'NO OVER TIME
    '            ',KUNG MAY ALLOWABLE HOUR(S) FOR OVER TIME
    '            '********************

    '        End If

    '    Else

    '        currEShiftID = ""
    '        currShiftID = ""

    '        txtshftEffFrom.Text = ""
    '        txtshftEffTo.Text = ""
    '        txtshftTimeFrom.Text = ""
    '        txtshftTimeTo.Text = ""

    '        txthrsworkd.Text = 0
    '        txthrsOT.Text = 0
    '        txthrsUT.Text = 0
    '        txthrsabsent.Text = 0
    '        txtnightdiff.Text = 0
    '        txtnightdiffOT.Text = 0

    '    End If

    'End Sub

#End Region

    Private Sub tsbtnCloseempawar_Click(sender As Object, e As EventArgs) Handles tsbtnCloseempawar.Click
        If bgworkloademployees.IsBusy Then
            bgworkloademployees.CancelAsync()
        End If

        InfoBalloon(, , lblforballoon, , , 1)

        Me.Close()
    End Sub

    Sub INSUPD_payrate(Optional prate_RowID As Object = Nothing, _
                       Optional prate_Date As Object = Nothing, _
                       Optional prate_PayType As Object = Nothing, _
                       Optional prate_Description As Object = Nothing, _
                       Optional prate_PayRate As Object = Nothing, _
                       Optional prate_OvertimeRate As Object = Nothing, _
                       Optional prate_NightDifferentialRate As Object = Nothing, _
                       Optional prate_NightDifferentialOTRate As Object = Nothing)

        Dim params(10, 2) As Object

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

        params(0, 1) = If(prate_RowID = Nothing, DBNull.Value, prate_RowID)
        params(1, 1) = orgztnID
        params(2, 1) = z_User 'CreatedBy
        params(3, 1) = z_User 'LastUpdBy
        params(4, 1) = prate_Date
        params(5, 1) = If(prate_PayType = Nothing, DBNull.Value, prate_PayType)
        params(6, 1) = If(prate_Description = Nothing, DBNull.Value, prate_Description)
        params(7, 1) = If(prate_PayRate = Nothing, DBNull.Value, prate_PayRate)
        params(8, 1) = If(prate_OvertimeRate = Nothing, DBNull.Value, prate_OvertimeRate)
        params(9, 1) = If(prate_NightDifferentialRate = Nothing, DBNull.Value, prate_NightDifferentialRate)
        params(10, 1) = If(prate_NightDifferentialOTRate = Nothing, DBNull.Value, prate_NightDifferentialOTRate)

        EXEC_INSUPD_PROCEDURE(params, _
                              "INSUPD_payrate", _
                              "payrateID")

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        'dgvEmployi_SelectionChanged(sender, e)

        'For Each c As DataGridViewColumn In dgvetentsemimon.Columns
        '    IO.File.AppendAllText(IO.Path.GetTempPath() & "dgvetentsemimon.txt", c.Index + 1 & "&" & c.Name & "@" & c.HeaderText & Environment.NewLine)
        'Next

        'Dim str_length As Integer = If(("lambert").Length >= 50, 50, ("lambert").Length)

        'MsgBox(("lambert").Substring(0, str_length))

        'MsgBox(Format(Date.Parse("07:30:00"), "HH:mm:ss") & " curr_TimeFromMILIT" & vbNewLine & _
        '       Format(Date.Parse("13:00:00"), "HH:mm:ss") & " TimeIn" & vbNewLine & _
        '       DateDiff(DateInterval.Hour, _
        '            Date.Parse(Format(Date.Parse("07:35:00"), "HH:mm:ss")), _
        '            Date.Parse(Format(Date.Parse("07:30:00"), "HH:mm:ss"))).ToString)

        'For i = 0 To 4

        '    Dim thisyearnumofdays As Integer = _
        '        Val(EXECQUER("SELECT DAYOFYEAR(CAST(CONCAT(YEAR(DATE_ADD(NOW(), INTERVAL " & i & " YEAR)),'-', MONTH('1000-12-01'),'-', DAY(LAST_DAY('1000-12-03'))) AS DATE));"))

        '    For ii = 1 To thisyearnumofdays

        '        Dim thelastdateofyear As Object = _
        '            EXECQUER("SELECT DATE_FORMAT(CAST(CONCAT(YEAR(DATE_ADD(NOW(), INTERVAL " & i & " YEAR)),'-', MONTH('1000-12-01'),'-', DAY(LAST_DAY('1000-12-03'))) AS DATE),'%Y-%m-%d');")

        '        Dim datemade = EXECQUER("SELECT DATE_FORMAT(MAKEDATE(YEAR('" & thelastdateofyear & "')," & ii & "),'%Y-%m-%d');")

        '        INSUPD_payrate(, datemade, , , , , , )

        '    Next

        'Next

        If Button3.Image.Tag = 1 Then
            Button3.Image = Nothing
            Button3.Image = My.Resources.r_arrow
            Button3.Image.Tag = 0

            TabControl1.Show()
            dgvEmployi.Width = 350

            dgvEmployi_SelectionChanged(sender, e)
        Else
            Button3.Image = Nothing
            Button3.Image = My.Resources.l_arrow
            Button3.Image.Tag = 1

            TabControl1.Hide()
            Dim pointX As Integer = Width_resolution - (Width_resolution * 0.15)

            dgvEmployi.Width = pointX
        End If

    End Sub

    Dim dontUpdate As SByte = 0

    Private Sub tsbtnSavetimeent_Click(sender As Object, e As EventArgs) Handles tsbtnSavetimeent.Click
        If dgvEmployi.RowCount = 0 Then
            Exit Sub
        End If

        If dontUpdate = 1 Then
            listOfEditetimeent.Clear()
        End If

        lblmonthyear.Focus()

        txthrsworkd.HideSelection = True

        txthrsworkd.Focus()


        lblmonthyear.Focus()

        txttotdaypay.HideSelection = True

        txttotdaypay.Focus()


        lblmonthyear.Focus()

        txtreghrsworkd.HideSelection = True

        txtreghrsworkd.Focus()


        lblmonthyear.Focus()

        txtregpay.HideSelection = True

        txtregpay.Focus()


        lblmonthyear.Focus()

        txthrsOT.HideSelection = True

        txthrsOT.Focus()


        lblmonthyear.Focus()

        txtotpay.HideSelection = True

        txtotpay.Focus()


        lblmonthyear.Focus()

        txthrsUT.HideSelection = True

        txthrsUT.Focus()


        lblmonthyear.Focus()

        txtutamount.HideSelection = True

        txtutamount.Focus()


        lblmonthyear.Focus()

        txthrslate.HideSelection = True

        txthrslate.Focus()


        lblmonthyear.Focus()

        txtlateamount.HideSelection = True

        txtlateamount.Focus()


        lblmonthyear.Focus()

        txtnightdiff.HideSelection = True

        txtnightdiff.Focus()


        lblmonthyear.Focus()

        txtnightdiffpay.HideSelection = True

        txtnightdiffpay.Focus()


        lblmonthyear.Focus()

        txtnightdiffOT.HideSelection = True

        txtnightdiffOT.Focus()


        lblmonthyear.Focus()

        txtnightdiffotpay.HideSelection = True

        txtnightdiffotpay.Focus()


        lblmonthyear.Focus()


        RemoveHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged

        For Each dgvrow As DataGridViewRow In dgvetentsemimon.Rows
            With dgvrow
                If listOfEditetimeent.Contains(.Cells("etiment_RowID").Value) Then

                    INSUPD_employeetimeentry(.Cells("etiment_RowID").Value, _
                                             .Cells("etiment_Date").Value, _
                                             .Cells("etiment_EmpShiftID").Value, _
                                             .Cells("etiment_EmpID").Value, _
                                             .Cells("etiment_EmpSalID").Value, _
                                             If(dgvEmployi.CurrentRow.Cells("cemp_EmployeeType").Value = "Fixed", 1, 0), _
                                             .Cells("etiment_RegHrsWork").Value, _
                                             .Cells("etiment_OTHrsWork").Value, _
                                             .Cells("etiment_UTHrsWork").Value, _
                                             .Cells("etiment_NightDiffHrs").Value, _
                                             .Cells("etiment_NightDiffOTHrs").Value, _
                                             .Cells("etiment_Hrslate").Value, _
                                             .Cells("etiment_PayrateID").Value, _
                                             .Cells("etiment_VacLeavHrs").Value, _
                                             .Cells("etiment_SickLeavHrs").Value, _
                                             .Cells("etiment_TotDayPay").Value, _
                                             .Cells("etiment_RowID").Value, _
                                             .Cells("etiment_tothrsworked").Value, _
                                             .Cells("etiment_reghrsamt").Value, _
                                             .Cells("etiment_otpay").Value, _
                                             .Cells("etiment_utamt").Value, _
                                             .Cells("etiment_nightdiffpay").Value, _
                                             .Cells("etiment_nightdiffotpay").Value, _
                                             .Cells("etiment_lateamt").Value, _
                                             .Cells("etiment_MaternLeavHrs").Value, _
                                             .Cells("etiment_OtherLeavHrs").Value)

                Else

                End If

            End With
        Next

        listOfEditetimeent.Clear()

        AddHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged

        TimEntduration.Close()

        InfoBalloon("Changes made in Employee ID '" & dgvEmployi.CurrentRow.Cells("cemp_EmployeeID").Value & "' has successfully saved.", "Changes successfully save", lblforballoon, 0, -69)

        txthrsworkd.HideSelection = True

        txttotdaypay.HideSelection = True

        txtreghrsworkd.HideSelection = True

        txtregpay.HideSelection = True

        txthrsOT.HideSelection = True

        txtotpay.HideSelection = True

        txthrsUT.HideSelection = True

        txtutamount.HideSelection = True

        txthrslate.HideSelection = True

        txtlateamount.HideSelection = True

        txtnightdiff.HideSelection = True

        txtnightdiffpay.HideSelection = True

        txtnightdiffOT.HideSelection = True

        txtnightdiffotpay.HideSelection = True

        'lblforballoon
    End Sub

    '************
    'SEMI MONTHLY - PAY PERIOD
    '************

    Private Sub dgvEmployiSEMIMON1of2(sender As Object, e As EventArgs) 'Handles dgvEmployi.SelectionChanged
        'dgvEmp
        Static employeeRowID As String = -1
        Static staticDate As Object = -1
        'listofEditRow.Clear()

        If dgvEmployi.RowCount <> 0 Then
            'Val(dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value) <> 0
            'If curr_CalendCol <> Nothing _
            If dgvcalendar.RowCount <> 0 Then

                curr_CalendCol = dgvcalendar.Columns(dgvcalendar.CurrentCell.ColumnIndex).Name

                'If employeeRowID <> dgvEmployi.CurrentRow.Cells("RowID").Value Then
                '    And staticDate <> curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value Then

                employeeRowID = dgvEmployi.CurrentRow.Cells("cemp_RowID").Value
                staticDate = curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value

                dt_esh = New DataTable
                dt_esh = retAsDatTbl("SELECT " & _
                                     "esh.RowID 'esh_RowID'" & _
                                     ",DATE_FORMAT(esh.EffectiveFrom,'%m-%d-%Y') 'EffFrom'" & _
                                     ",DATE_FORMAT(esh.EffectiveTo,'%m-%d-%Y') 'EffTo'" & _
                                     ",COALESCE(sh.RowID,'') 'sh_RowID'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%r'),'') 'sh_TimeFrom'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeTo,'%r'),'') 'sh_TimeTo'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%H:%i:%s'),'') 'sh_TimeFromMILIT'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeTo,'%H:%i:%s'),'') 'sh_TimeToMILIT'" & _
                                     ",COALESCE(esh.NightShift,'') 'NightShift'" & _
                                     " FROM employeeshift esh" & _
                                     " LEFT JOIN shift sh ON sh.RowID=esh.ShiftID" & _
                                     " WHERE esh.EmployeeID=" & dgvEmployi.CurrentRow.Cells("cemp_RowID").Value & _
                                     " AND esh.OrganizationID=" & orgztnID & _
                                     " AND CAST('" & curr_YYYY & "-" & curr_mm & "-" & dgvcalendar.CurrentRow.Cells(curr_CalendCol).Value & "' AS DATE) BETWEEN esh.EffectiveFrom AND esh.EffectiveTo;")

                currEShiftID = ""
                currShiftID = ""

                txtshftEffFrom.Text = ""
                txtshftEffTo.Text = ""
                txtshftTimeFrom.Text = ""
                txtshftTimeTo.Text = ""

                curr_TimeFromMILIT = Nothing
                curr_TimeToMILIT = Nothing

                currNightEShiftID = Nothing
                currNightShiftID = Nothing

                curr_NightTimeFromMILIT = Nothing
                curr_NightTimeToMILIT = Nothing

                For Each drow As DataRow In dt_esh.Rows

                    If Val(drow("NightShift")) = 0 Then

                        currEShiftID = drow("esh_RowID")
                        currShiftID = drow("sh_RowID")

                        txtshftEffFrom.Text = drow("EffFrom")
                        txtshftEffTo.Text = drow("EffTo")
                        txtshftTimeFrom.Text = drow("sh_TimeFrom")
                        txtshftTimeTo.Text = drow("sh_TimeTo")

                        curr_TimeFromMILIT = drow("sh_TimeFromMILIT")
                        curr_TimeToMILIT = drow("sh_TimeToMILIT")

                    Else
                        currNightEShiftID = drow("esh_RowID")
                        currNightShiftID = drow("sh_RowID")

                        txtshftEffFrom.Text = drow("EffFrom")
                        txtshftEffTo.Text = drow("EffTo")
                        txtshftTimeFrom.Text = drow("sh_TimeFrom")
                        txtshftTimeTo.Text = drow("sh_TimeTo")

                        curr_NightTimeFromMILIT = drow("sh_TimeFromMILIT")
                        curr_NightTimeToMILIT = drow("sh_TimeToMILIT")

                    End If

                Next

                'End If

                Dim dt_etent As New DataTable

                Dim hrswrkd As Double

                Static firstrow As SByte = 0

                timeoutlastval = Nothing

                txthrsworkd.Text = 0
                txthrsOT.Text = 0
                txthrsUT.Text = 0

                'txthrsabsent.Text = 0
                txtnightdiff.Text = 0
                txtnightdiffOT.Text = 0

                hrswrkd = 0

                'cbolateabsent.SelectedIndex = -1
                'curr_TimeFromMILIT'curr_TimeToMILIT
                'curr_NightTimeFromMILIT'curr_NightTimeToMILIT

                '*******************************************************************************
                'THIS IS FOR EMPLOYEE WHO'S
                'PAY FREQUENCY IS 'DAILY'
                dt_etent = retAsDatTbl("SELECT " & _
                                       "COALESCE(TIME_FORMAT(TimeIn,'%H:%i:%s'),'') 'TimeIn'" & _
                                       ", COALESCE(TIME_FORMAT(TimeOut,'%H:%i:%s'),'') 'TimeOut'" & _
                                       ", COALESCE(DATE_FORMAT(DATE,'%m-%d-%Y'),'') 'Date'" & _
                                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLate'" & _
                                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_TimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_TimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUT'" & _
                                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',-2),':',1) AS DECIMAL) / 60),'') 'OT'" & _
                                       ", CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60) 'HRS_workd'" & _
                                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLateNight'" & _
                                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUTNight'" & _
                                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'OTNight'" & _
                                       " FROM employeetimeentrydetails" & _
                                       " WHERE EmployeeID=" & dgvEmployi.CurrentRow.Cells("cemp_RowID").Value & _
                                       " AND OrganizationID=" & orgztnID & _
                                       " AND DATE BETWEEN '" & curr_YYYY & "-" & curr_mm & "-15' AND LAST_DAY('" & curr_YYYY & "-" & curr_mm & "-00')" & _
                                       " ORDER BY DATE,TIME_FORMAT(TimeIn,'%H') ASC;")
                '*******************************************************************************

                Dim UTval, _
                    UTNightval, _
                    OTval, _
                    OTNightval, _
                    Lateval As Double
                UTNightval = 0 : OTNightval = 0
                For Each drows As DataRow In dt_etent.Rows

                    hrswrkd += Val(drows("HRS_workd")) 'Total Hour(s) worked

                    If firstrow = 0 Then
                        firstrow = 1

                        'txthrsUT.Text = UTval.ToString 'Total Undertime hour(s) 

                        If Val(drows("IsLateNight")) <= 0 Then
                            drows("IsLateNight") = drows("IsLateNight").ToString.Replace("-", "")
                            Lateval = Val(drows("IsLateNight"))
                        Else

                            If drows("IsLate").ToString.Contains("-") Then
                                'cbolateabsent.SelectedIndex = 1
                                'drows("IsLate") = drows("IsLate").ToString.Replace("-", "")
                                'Lateval = Val(drows("IsLate"))
                            Else
                                'cbolateabsent.SelectedIndex = -1
                            End If

                            'drows("IsLate") = drows("IsLate").ToString.Replace("-", "")
                            'UTval = Val(drows("IsLate"))
                        End If

                    End If

                    'If Val(drows("IsUTNight")) <= 0 Then
                    '    If drows("IsUTNight").ToString.Contains("-") Then
                    '        drows("IsUTNight") = drows("IsUTNight").ToString.Replace("-", "")
                    '        UTNightval = Val(drows("IsUTNight"))
                    '    End If

                    'Else
                    If drows("IsUT").ToString.Contains("-") Then
                        drows("IsUT") = drows("IsUT").ToString.Replace("-", "")
                        UTval = Val(drows("IsUT"))
                    End If

                    'End If

                    'If Val(drows("OTNight")) <= 0 Then
                    '    drows("OTNight") = drows("OTNight").ToString.Replace("-", "")
                    '    OTNightval += Val(drows("OTNight"))
                    'Else
                    If drows("OT").ToString.Contains("-") = False Then
                        drows("OT") = drows("OT").ToString.Replace("-", "")
                        OTval += Val(drows("OT"))
                    End If
                    'End If

                Next

                firstrow = 0

                txthrsworkd.Text = hrswrkd

                txthrsOT.Text = OTval

                txthrsUT.Text = If(OTval > 0, 0, UTval)

                'cboOverUnderTime.SelectedIndex = If(OTval > 0, 0, If(dt_etent.Rows.Count = 0, -1, 1))

            End If

        Else

            currEShiftID = ""
            currShiftID = ""

            txtshftEffFrom.Text = ""
            txtshftEffTo.Text = ""
            txtshftTimeFrom.Text = ""
            txtshftTimeTo.Text = ""

            txthrsworkd.Text = 0
            txthrsOT.Text = 0
            txthrsUT.Text = 0
            'txthrsabsent.Text = 0
            txtnightdiff.Text = 0
            txtnightdiffOT.Text = 0

        End If

    End Sub

#Region "INSERT payperiod SEMI MONTHLY"

    'Dim firstdate As Object = EXECQUER("SELECT DATE_FORMAT(MAKEDATE(YEAR(NOW()),1),'%Y-%m-%d');")

    '    For i = 0 To 11
    'Dim newdattab As New DataTable
    '        newdattab = retAsDatTbl("SELECT YEAR(ADDDATE('" & firstdate & "', INTERVAL " & i & " MONTH)) 'next_month_Y'" & _
    '                                ", MONTH(ADDDATE('" & firstdate & "', INTERVAL " & i & " MONTH)) 'next_month_MM'" & _
    '                                ", DAY(LAST_DAY(ADDDATE('" & firstdate & "', INTERVAL " & i & " MONTH))) 'this_month_lastday';")

    '        For Each drow As DataRow In newdattab.Rows
    'Dim payp_fromdate As String
    'Dim payp_todate As String

    '            payp_fromdate = drow("next_month_Y") & "-" & drow("next_month_MM") & "-1"
    '            payp_todate = drow("next_month_Y") & "-" & drow("next_month_MM") & "-15"

    '            INSUPD_payperiod_SEMI_MON(, payp_fromdate, _
    '                                        payp_todate)

    '            payp_fromdate = drow("next_month_Y") & "-" & drow("next_month_MM") & "-16"
    '            payp_todate = drow("next_month_Y") & "-" & drow("next_month_MM") & "-" & drow("this_month_lastday")

    '            INSUPD_payperiod_SEMI_MON(, payp_fromdate, _
    '                                        payp_todate)
    '        Next

    '    Next

#End Region

    Sub INSUPD_payperiod(Optional payp_RowID As Object = Nothing, _
                                  Optional payp_PayFromDate As Object = Nothing, _
                                  Optional payp_PayToDate As Object = Nothing, _
                                  Optional payp_TotalGrossSalary As Object = Nothing, _
                                  Optional payp_TotalNetSalary As Object = Nothing, _
                                  Optional payp_TotalEmpSSS As Object = Nothing, _
                                  Optional payp_TotalEmpWithholdingTax As Object = Nothing, _
                                  Optional payp_TotalCompSSS As Object = Nothing, _
                                  Optional payp_TotalEmpPhilhealth As Object = Nothing, _
                                  Optional payp_TotalCompPhilhealth As Object = Nothing, _
                                  Optional payp_TotalEmpHDMF As Object = Nothing, _
                                  Optional payp_TotalCompHDMF As Object = Nothing)

        Dim params(14, 2) As Object

        params(0, 0) = "payp_RowID"
        params(1, 0) = "payp_OrganizationID"
        params(2, 0) = "payp_CreatedBy"
        params(3, 0) = "payp_LastUpdBy"
        params(4, 0) = "payp_PayFromDate"
        params(5, 0) = "payp_PayToDate"
        params(6, 0) = "payp_TotalGrossSalary"
        params(7, 0) = "payp_TotalNetSalary"
        params(8, 0) = "payp_TotalEmpSSS"
        params(9, 0) = "payp_TotalEmpWithholdingTax"
        params(10, 0) = "payp_TotalCompSSS"
        params(11, 0) = "payp_TotalEmpPhilhealth"
        params(12, 0) = "payp_TotalCompPhilhealth"
        params(13, 0) = "payp_TotalEmpHDMF"
        params(14, 0) = "payp_TotalCompHDMF"

        params(0, 1) = If(payp_RowID = Nothing, DBNull.Value, payp_RowID)
        params(1, 1) = orgztnID
        params(2, 1) = z_User 'CreatedBy
        params(3, 1) = z_User 'LastUpdBy
        params(4, 1) = payp_PayFromDate
        params(5, 1) = payp_PayToDate
        params(6, 1) = If(payp_TotalGrossSalary = Nothing, DBNull.Value, payp_TotalGrossSalary)
        params(7, 1) = If(payp_TotalNetSalary = Nothing, DBNull.Value, payp_TotalNetSalary)
        params(8, 1) = If(payp_TotalEmpSSS = Nothing, DBNull.Value, payp_TotalEmpSSS)
        params(9, 1) = If(payp_TotalEmpWithholdingTax = Nothing, DBNull.Value, payp_TotalEmpWithholdingTax)
        params(10, 1) = If(payp_TotalCompSSS = Nothing, DBNull.Value, payp_TotalCompSSS)
        params(11, 1) = If(payp_TotalEmpPhilhealth = Nothing, DBNull.Value, payp_TotalEmpPhilhealth)
        params(12, 1) = If(payp_TotalCompPhilhealth = Nothing, DBNull.Value, payp_TotalCompPhilhealth)
        params(13, 1) = If(payp_TotalEmpHDMF = Nothing, DBNull.Value, payp_TotalEmpHDMF)
        params(14, 1) = If(payp_TotalCompHDMF = Nothing, DBNull.Value, payp_TotalCompHDMF)

        EXEC_INSUPD_PROCEDURE(params, _
                              "INSUPD_payperiod", _
                              "payperiodID")

    End Sub

    Dim paypdattab As New DataTable

    Sub loadpayperiod()

        paypdattab = retAsDatTbl("SELECT RowID" & _
                                 ",COALESCE(DATE_FORMAT(PayFromDate,'%m-%d-%Y'),'') 'PayFromDate'" & _
                                 ",COALESCE(DATE_FORMAT(PayToDate,'%m-%d-%Y'),'') 'PayToDate'" & _
                                 ",COALESCE(TotalGrossSalary,0) 'TotalGrossSalary'" & _
                                 ",COALESCE(TotalNetSalary,0) 'TotalNetSalary'" & _
                                 ",COALESCE(TotalEmpSSS,0) 'TotalEmpSSS'" & _
                                 ",COALESCE(TotalEmpWithholdingTax,0) 'TotalEmpWithholdingTax'" & _
                                 ",COALESCE(TotalCompSSS,0) 'TotalCompSSS'" & _
                                 ",COALESCE(TotalEmpPhilhealth,0) 'TotalEmpPhilhealth'" & _
                                 ",COALESCE(TotalCompPhilhealth,0) 'TotalCompPhilhealth'" & _
                                 ",COALESCE(TotalEmpHDMF,0) 'TotalEmpHDMF'" & _
                                 ",COALESCE(TotalCompHDMF,0) 'TotalCompHDMF'" & _
                                 ",IF(DATE_FORMAT(NOW(),'%Y-%m-%d') BETWEEN PayFromDate AND PayToDate,'0',IF(DATE_FORMAT(NOW(),'%Y-%m-%d') > PayFromDate,'-1','1')) 'now_origin'" & _
                                 " FROM payperiod WHERE OrganizationID=" & orgztnID & _
                                 " AND YEAR(NOW()) IN (YEAR(PayFromDate),YEAR(PayToDate)) ORDER BY PayFromDate;")

        'cbopaypfrom.Items.Clear()
        'cbopaypto.Items.Clear()

        For Each drow As DataRow In paypdattab.Rows

            'cbopaypfrom.Items.Add(drow("PayFromDate").ToString)
            'cbopaypto.Items.Add(drow("PayToDate").ToString)

        Next

    End Sub

    Sub btnpaypPrev_btnpaypNxt_Click(sender As Object, e As EventArgs) Handles btnpaypNxt.Click, _
                                                                                       btnpaypPrev.Click

        RemoveHandler dtppayperiod.ValueChanged, AddressOf dtppayperiod_ValueChanged

        Dim btnpaypName As String

        btnpaypName = DirectCast(sender, Button).Name

        If btnpaypName = "btnpaypNxt" Then
            dtppayperiod.Value = CDate(dtppayperiod.Value).AddDays(1)
            'dtppayperiod.Value = Format(DateAdd(DateInterval.Day, 1, CDate(dtppayperiod.Value)), _
            '                            "MMMM-dd-yyyy")

            Dim payp_day As Integer = CDate(dtppayperiod.Value).Day

            If payp_day - 15 <= 0 Then
                dtppayperiod.Value = Format(CDate(dtppayperiod.Value), _
                                            "MMMM-15-yyyy")
            Else
                Dim payp_maxday As Integer = Date.DaysInMonth(CDate(dtppayperiod.Value).Year, _
                                                              CDate(dtppayperiod.Value).Month)

                dtppayperiod.Value = Format(CDate(dtppayperiod.Value), _
                                            "MMMM-" & payp_maxday & "-yyyy")
            End If
        Else

            'dtppayperiod.Value = Format(CDate(dtppayperiod.Value), "MMMM-1-yyyy")
            'dtppayperiod.Value = Format(DateAdd(DateInterval.Day, -1, CDate(dtppayperiod.Value)), _
            '                            "MMMM-dd-yyyy")

            Dim payp_day As Integer = CDate(dtppayperiod.Value).Day

            If payp_day - 15 <= 0 Then
                dtppayperiod.Value = CDate(dtppayperiod.Value).AddMonths(-1)

                Dim payp_maxday As Integer = Date.DaysInMonth(CDate(dtppayperiod.Value).Year, _
                                                              CDate(dtppayperiod.Value).Month)

                dtppayperiod.Value = Format(CDate(dtppayperiod.Value), _
                                            "MMMM-" & payp_maxday & "-yyyy")
            Else
                dtppayperiod.Value = Format(CDate(dtppayperiod.Value), _
                                            "MMMM-15-yyyy")
            End If
        End If

        AddHandler dtppayperiod.ValueChanged, AddressOf dtppayperiod_ValueChanged

        dtppayperiod_ValueChanged(sender, e)

    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        TabControlColor(TabControl1, e)
    End Sub

    Private Sub tsbtnNewtimeent_Click(sender As Object, e As EventArgs) Handles tsbtnNewtimeent.Click

        Dim n_TimEntduration As New TimEntduration

        With n_TimEntduration
            .ShowDialog()
            '.Show()
            '.BringToFront()
            'MDIPrimaryForm.Enabled = False
        End With

    End Sub

    Function INSUPD_employeetimeentry(Optional etent_RowID As Object = Nothing, _
                                     Optional etent_Date As Object = Nothing, _
                                     Optional etent_EmployeeShiftID As Object = Nothing, _
                                     Optional etent_EmployeeID As Object = Nothing, _
                                     Optional etent_EmployeeSalaryID As Object = Nothing, _
                                     Optional etent_EmployeeFixedSalaryFlag As Object = Nothing, _
                                     Optional etent_RegularHoursWorked As Object = Nothing, _
                                     Optional etent_OvertimeHoursWorked As Object = Nothing, _
                                     Optional etent_UndertimeHours As Object = Nothing, _
                                     Optional etent_NightDifferentialHours As Object = Nothing, _
                                     Optional etent_NightDifferentialOTHours As Object = Nothing, _
                                     Optional etent_HoursLate As Object = Nothing, _
                                     Optional etent_PayRateID As Object = Nothing, _
                                     Optional etent_VacationLeaveHours As Object = Nothing, _
                                     Optional etent_SickLeaveHours As Object = Nothing, _
                                     Optional etent_TotalDayPay As Object = Nothing, _
                                     Optional etent_IsNightShift As Object = Nothing, _
                                     Optional etent_TotalHoursWorked As Object = Nothing, _
                                     Optional etent_RegularHoursAmount As Object = Nothing, _
                                     Optional etent_OvertimeHoursAmount As Object = Nothing, _
                                     Optional etent_UndertimeHoursAmount As Object = Nothing, _
                                     Optional etent_NightDiffHoursAmount As Object = Nothing, _
                                     Optional etent_NightDiffOTHoursAmount As Object = Nothing, _
                                     Optional etent_HoursLateAmount As Object = Nothing, _
                                     Optional etent_MaternityLeaveHours As Object = Nothing, _
                                     Optional etent_OtherLeaveHours As Object = Nothing) As Object

        Dim params(28, 2) As Object

        params(0, 0) = "etent_RowID"
        params(1, 0) = "etent_OrganizationID"
        params(2, 0) = "etent_CreatedBy"
        params(3, 0) = "etent_LastUpdBy"
        params(4, 0) = "etent_Date"
        params(5, 0) = "etent_EmployeeShiftID"
        params(6, 0) = "etent_EmployeeID"
        params(7, 0) = "etent_EmployeeSalaryID"
        params(8, 0) = "etent_EmployeeFixedSalaryFlag"
        params(9, 0) = "etent_RegularHoursWorked"
        params(10, 0) = "etent_OvertimeHoursWorked"
        params(11, 0) = "etent_UndertimeHours"
        params(12, 0) = "etent_NightDifferentialHours"
        params(13, 0) = "etent_NightDifferentialOTHours"
        params(14, 0) = "etent_HoursLate"
        params(15, 0) = "etent_PayRateID"
        params(16, 0) = "etent_VacationLeaveHours"
        params(17, 0) = "etent_SickLeaveHours"
        params(18, 0) = "etent_TotalDayPay"
        params(19, 0) = "etent_IsNightShift"
        params(20, 0) = "etent_TotalHoursWorked"
        params(21, 0) = "etent_RegularHoursAmount"
        params(22, 0) = "etent_OvertimeHoursAmount"
        params(23, 0) = "etent_UndertimeHoursAmount"
        params(24, 0) = "etent_NightDiffHoursAmount"
        params(25, 0) = "etent_NightDiffOTHoursAmount"
        params(26, 0) = "etent_HoursLateAmount"
        params(27, 0) = "etent_MaternityLeaveHours"
        params(28, 0) = "etent_OtherLeaveHours"

        params(0, 1) = etent_RowID
        params(1, 1) = orgztnID
        params(2, 1) = z_User 'CreatedBy
        params(3, 1) = z_User 'LastUpdBy
        params(4, 1) = If(etent_Date = Nothing, DBNull.Value, etent_Date)
        params(5, 1) = If(etent_EmployeeShiftID = Nothing, DBNull.Value, etent_EmployeeShiftID)
        params(6, 1) = etent_EmployeeID
        params(7, 1) = If(etent_EmployeeSalaryID = Nothing, DBNull.Value, etent_EmployeeSalaryID)
        params(8, 1) = If(etent_EmployeeFixedSalaryFlag = Nothing, 0, 1)
        params(9, 1) = If(etent_RegularHoursWorked = Nothing, 0.0, etent_RegularHoursWorked)
        params(10, 1) = If(etent_OvertimeHoursWorked = Nothing, 0.0, etent_OvertimeHoursWorked)
        params(11, 1) = If(etent_UndertimeHours = Nothing, 0.0, etent_UndertimeHours)
        params(12, 1) = If(etent_NightDifferentialHours = Nothing, 0.0, etent_NightDifferentialHours)
        params(13, 1) = If(etent_NightDifferentialOTHours = Nothing, 0.0, etent_NightDifferentialOTHours)
        params(14, 1) = If(etent_HoursLate = Nothing, 0.0, etent_HoursLate)
        params(15, 1) = etent_PayRateID
        params(16, 1) = If(etent_VacationLeaveHours = Nothing, 0.0, etent_VacationLeaveHours)
        params(17, 1) = If(etent_SickLeaveHours = Nothing, 0.0, etent_SickLeaveHours)
        params(18, 1) = If(etent_TotalDayPay = Nothing, 0.0, etent_TotalDayPay)
        params(19, 1) = If(etent_IsNightShift = Nothing, 0, 1)
        params(20, 1) = If(etent_TotalHoursWorked = Nothing, 0.0, etent_TotalHoursWorked)
        params(21, 1) = If(etent_RegularHoursAmount = Nothing, 0.0, etent_RegularHoursAmount)
        params(22, 1) = If(etent_OvertimeHoursAmount = Nothing, 0.0, etent_OvertimeHoursAmount)
        params(23, 1) = If(etent_UndertimeHoursAmount = Nothing, 0.0, etent_UndertimeHoursAmount)
        params(24, 1) = If(etent_NightDiffHoursAmount = Nothing, 0.0, etent_NightDiffHoursAmount)
        params(25, 1) = If(etent_NightDiffOTHoursAmount = Nothing, 0.0, etent_NightDiffOTHoursAmount)
        params(26, 1) = If(etent_HoursLateAmount = Nothing, 0.0, etent_HoursLateAmount)
        params(27, 1) = If(etent_MaternityLeaveHours = Nothing, 0.0, etent_MaternityLeaveHours)
        params(28, 1) = If(etent_OtherLeaveHours = Nothing, 0.0, etent_OtherLeaveHours)

        INSUPD_employeetimeentry = _
            EXEC_INSUPD_PROCEDURE(params, _
                                  "INSUPD_employeetimeentry", _
                                  "etentID")

    End Function

    Dim pagination As Integer
    Dim page_limiter As Integer = 20
    Private Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked, _
        Nxt.LinkClicked, Last.LinkClicked

        RemoveHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "First" Then
            pagination = 0
        ElseIf sendrname = "Prev" Then
            If pagination - page_limiter < 0 Then
                pagination = 0
            Else : pagination -= page_limiter
            End If
        ElseIf sendrname = "Nxt" Then
            pagination += page_limiter
        ElseIf sendrname = "Last" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / " & page_limiter & " FROM employee WHERE OrganizationID=" & orgztnID & ";"))



            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * page_limiter

            'pagination += page_limiter

            If pagination - page_limiter < page_limiter Then
                'pagination = 0

            End If

            'pagination = If(lastpage - page_limiter >= page_limiter, _
            '                lastpage - page_limiter, _
            '                lastpage)

        End If



        If (Trim(TextBox2.Text) <> "" Or _
            Trim(TextBox15.Text) <> "" Or _
            Trim(TextBox16.Text) <> "" Or _
            Trim(TextBox17.Text) <> "") And _
            TabControl2.SelectedIndex = 0 Then

            btnRerfresh_Click(sender, e)

        ElseIf Trim(txtSimple.Text) <> "" And _
            TabControl2.SelectedIndex = 1 Then

            btnRerfresh_Click(sender, e)

        Else

            loademployees()

        End If

        AddHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged

        dgvEmployi_SelectionChanged(sender, e)

    End Sub

    Public employeetimeentry As New DataTable

    Dim employeetimeentry_absent As New DataTable

    Sub VIEW_employeetimeentry_SEMIMON(ByVal etent_EmployeeID As Object, _
                                       ByVal etent_date As Object)

        Static once As SByte = 0

        Dim dgvcatcher As New DataGridView

        Dim params(3, 2) As Object

        params(0, 0) = "etent_OrganizationID"
        params(1, 0) = "etent_EmployeeID"
        params(2, 0) = "etent_Date"

        params(0, 1) = orgztnID
        params(1, 1) = etent_EmployeeID

        params(2, 1) = CDate(etent_date)

        If once = 0 Then
            once = 1
            'With dgvcatcher
            '    .Columns.Add("etiment_RowID", "RowID")
            '    .Columns.Add("etiment_Org", "OrganizationID")
            '    .Columns.Add("etiment_Created", "Created")
            '    .Columns.Add("etiment_CreatedBy", "CreatedBy")

            '    .Columns.Add("etiment_LastUpd", "LastUpd")
            '    .Columns.Add("etiment_LastUpdBy", "LastUpdBy")
            '    .Columns.Add("etiment_Date", "Date")
            '    .Columns.Add("etiment_EmployeeShiftID", "EmployeeShiftID")

            '    .Columns.Add("etiment_EmployeeID", "EmployeeID")
            '    .Columns.Add("etiment_EmployeeSalaryID", "EmployeeSalaryID")
            '    .Columns.Add("etiment_EmployeeFixedSalaryFlag", "EmployeeFixedSalaryFlag")
            '    .Columns.Add("etiment_RegularHoursWorked", "RegularHoursWorked")

            '    .Columns.Add("etiment_OvertimeHoursWorked", "OvertimeHoursWorked")
            '    .Columns.Add("etiment_UndertimeHours", "UndertimeHours")
            '    .Columns.Add("etiment_NightDifferentialHours", "NightDifferentialHours")
            '    .Columns.Add("etiment_NightDifferentialOTHours", "NightDifferentialOTHours")

            '    .Columns.Add("etiment_Hourslate", "Hourslate")
            '    .Columns.Add("etiment_LateFlag", "LateFlag")
            '    .Columns.Add("etiment_PayrateID", "PayrateID")
            '    .Columns.Add("etiment_VacationLeaveHours", "VacationLeaveHours")

            '    .Columns.Add("etiment_SickLeaveHours", "SickLeaveHours")
            '    .Columns.Add("etiment_TotalDayPay", "TotalDayPay")

            'End With
        End If

        'filltable(dgvcatcher, _
        '          "VIEW_employeetimeentry_SEMIMON", _
        '          params, _
        '          1)

        EXEC_VIEW_PROCEDURE(params, _
                            "VIEW_employeetimeentry_SEMIMON", _
                            dgvetentsemimon)

        'retAsDatTbl()
        'employeetimeentry.Rows.Clear()

        'For Each dgvrow As DataGridViewRow In dgvcatcher.Rows
        '    MsgBox(dgvrow.Cells(0).Value.ToString)
        'Next

        'employeetimeentry = dgvcatcher.DataSource 'DirectCast(dgvcatcher.DataSource, DataTable)

        'employeetimeentry = dgvcatcher.DataSource

        'MsgBox(employeetimeentry.Rows.Count)

        employeetimeentry_absent.Dispose()

        employeetimeentry_absent = _
            New ReadSQLProcedureToDatatable("VIEW_employeetimeentry_this_month",
                                            orgztnID,
                                            etent_EmployeeID,
                                            etent_date).ResultTable

    End Sub

    Public employeesalary As New DataTable

    Sub VIEW_employeesalary(ByVal esal_EmployeeID As Object, _
                            ByVal esal_Date As Object)

        Dim dgvcatcher As New DataGridView

        Dim params(3, 2) As Object

        params(0, 0) = "esal_OrganizationID"
        params(1, 0) = "esal_EmployeeID"
        params(2, 0) = "esal_Date"

        params(0, 1) = orgztnID
        params(1, 1) = esal_EmployeeID
        params(2, 1) = CDate(esal_Date)

        filltable(dgvcatcher, _
                  "VIEW_employeesalary", _
                  params, _
                  1)

        employeesalary = dgvcatcher.DataSource

    End Sub

    Public me_close As SByte = 0

    Private Sub EmpTimeEntry_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        me_close = 1
        TimEntduration.Close()

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        InfoBalloon(, , lblforballoon, , , 1)

        WarnBalloon(, , lblforballoon, , , 1)

        showAuditTrail.Close()

        TimeAttendForm.listTimeAttendForm.Remove(Me.Name)

    End Sub

    Private Sub bgwpayrate_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) 'Handles bgwpayrate.DoWork
        backgroundworking = 1
    End Sub

    Private Sub bgwpayrate_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) 'Handles bgwpayrate.ProgressChanged

    End Sub

    Private Sub bgwpayrate_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) 'Handles bgwpayrate.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Background work cancelled.")
        Else
            'MessageBox.Show("Background work finish successfully.")
        End If
        backgroundworking = 0
    End Sub

    Dim tmpdate As String '= "01-01-" & CDate(today_date).Year

    Dim jan_lastday, _
        feb_lastday, _
        mar_lastday, _
        apr_lastday, _
        may_lastday, _
        jun_lastday, _
        jul_lastday, _
        aug_lastday, _
        sept_lastday, _
        oct_lastday, _
        nov_lastday, _
        dec_lastday As SByte

    Private Sub ToolStripButton15_Click(sender As Object, e As EventArgs) Handles tsbtnPrev.Click
        'tsbtnPrev.Checked = True
        'tsbtnNxt.Checked = False

        tmpdate = CInt(tmpdate) - 1

        Dim listoflastday = EXECQUER("SELECT CONCAT(DAY(LAST_DAY('" & tmpdate & "-01-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-02-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-03-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-04-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-05-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-06-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-07-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-08-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-09-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-10-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-11-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-12-01')));")

        Dim arrayoflastday = Split(listoflastday, ",")

        jan_lastday = arrayoflastday(0)

        wholejan.Text = "16-" & jan_lastday

        feb_lastday = arrayoflastday(1)

        wholefeb.Text = "16-" & feb_lastday

        mar_lastday = arrayoflastday(2)

        wholemar.Text = "16-" & mar_lastday

        apr_lastday = arrayoflastday(3)

        wholeapr.Text = "16-" & apr_lastday

        may_lastday = arrayoflastday(4)

        wholemay.Text = "16-" & may_lastday

        jun_lastday = arrayoflastday(5)

        wholejun.Text = "16-" & jun_lastday

        jul_lastday = arrayoflastday(6)

        wholejul.Text = "16-" & jul_lastday

        aug_lastday = arrayoflastday(7)

        wholeaug.Text = "16-" & aug_lastday

        sept_lastday = arrayoflastday(8)

        wholesep.Text = "16-" & sept_lastday

        oct_lastday = arrayoflastday(9)

        wholeoct.Text = "16-" & oct_lastday

        nov_lastday = arrayoflastday(10)

        wholenov.Text = "16-" & nov_lastday

        dec_lastday = arrayoflastday(11)

        wholedec.Text = "16-" & dec_lastday

        tsbtnPrev.Text = "← " & tmpdate - 1

        tsbtnNxt.Text = (tmpdate + 1) & " →"

        current_month(sel_month, sender, e)

    End Sub

    Private Sub tsbtnNxt_Click(sender As Object, e As EventArgs) Handles tsbtnNxt.Click
        'tsbtnNxt.Checked = True
        'tsbtnPrev.Checked = False

        tmpdate = tmpdate + 1

        Dim listoflastday = EXECQUER("SELECT CONCAT(DAY(LAST_DAY('" & tmpdate & "-01-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-02-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-03-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-04-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-05-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-06-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-07-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-08-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-09-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-10-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-11-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-12-01')));")

        Dim arrayoflastday = Split(listoflastday, ",")

        jan_lastday = arrayoflastday(0)

        wholejan.Text = "16-" & jan_lastday

        feb_lastday = arrayoflastday(1)

        wholefeb.Text = "16-" & feb_lastday

        mar_lastday = arrayoflastday(2)

        wholemar.Text = "16-" & mar_lastday

        apr_lastday = arrayoflastday(3)

        wholeapr.Text = "16-" & apr_lastday

        may_lastday = arrayoflastday(4)

        wholemay.Text = "16-" & may_lastday

        jun_lastday = arrayoflastday(5)

        wholejun.Text = "16-" & jun_lastday

        jul_lastday = arrayoflastday(6)

        wholejul.Text = "16-" & jul_lastday

        aug_lastday = arrayoflastday(7)

        wholeaug.Text = "16-" & aug_lastday

        sept_lastday = arrayoflastday(8)

        wholesep.Text = "16-" & sept_lastday

        oct_lastday = arrayoflastday(9)

        wholeoct.Text = "16-" & oct_lastday

        nov_lastday = arrayoflastday(10)

        wholenov.Text = "16-" & nov_lastday

        dec_lastday = arrayoflastday(11)

        wholedec.Text = "16-" & dec_lastday

        tsbtnNxt.Text = tmpdate + 1 & " →"

        tsbtnPrev.Text = "← " & (tmpdate - 1)

        current_month(sel_month, sender, e)

        'tmpdate = tmpdate - 2

        'tmpdate = tmpdate + 2

        'Dim sendr_name As String = CType(sender, Object).Name

        'If sendr_name = "tsbtnPrev" Then
        '    '←2015

        'ElseIf sendr_name = "tsbtnNxt" Then
        '    '2016→

        'End If

    End Sub

    Dim prevtsbtn As Object = Nothing

    Dim sel_month As String

    Dim tsdrpdwnbtn1 As ToolStripDropDownButton

    Dim etentsummapaypID As Integer = Nothing

    Private Sub tsbtnJan_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnJan.MouseEnter

        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        'tsbtnJan.ShowDropDown()
        tsbtnJan.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnJan_Click(sender As Object, e As EventArgs) Handles onehalfjan.Click  'tsbtnJan.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnJan)
        End If

        prevtsbtn = onehalfjan

        onehalfjan.Checked = True
        wholejan.Checked = False

        sel_month = "01"

        loaddateperiod()

    End Sub

    Private Sub wholejan_Click(sender As Object, e As EventArgs) Handles wholejan.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnJan)
        End If

        prevtsbtn = wholejan

        wholejan.Checked = True
        onehalfjan.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnFeb_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnFeb.MouseEnter

        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnFeb.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnFeb_Click(sender As Object, e As EventArgs) Handles onehalffeb.Click  'tsbtnFeb.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnFeb)
        End If

        prevtsbtn = onehalffeb

        onehalffeb.Checked = True
        wholefeb.Checked = False

        sel_month = "02"



        loaddateperiod()

    End Sub

    Private Sub wholefeb_Click(sender As Object, e As EventArgs) Handles wholefeb.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnFeb)
        End If

        prevtsbtn = wholefeb

        wholefeb.Checked = True
        onehalffeb.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnMar_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnMar.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnMar.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnMar_Click(sender As Object, e As EventArgs) Handles onehalfmar.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnMar)
        End If

        prevtsbtn = onehalfmar

        onehalfmar.Checked = True
        wholemar.Checked = False

        sel_month = "03"



        loaddateperiod()

    End Sub

    Private Sub wholemar_Click(sender As Object, e As EventArgs) Handles wholemar.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnMar)
        End If

        prevtsbtn = wholemar

        wholemar.Checked = True
        onehalfmar.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnApr_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnApr.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnApr.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnApr_Click(sender As Object, e As EventArgs) Handles onehalfapr.Click  'tsbtnApr.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnApr)
        End If

        prevtsbtn = onehalfapr

        onehalfapr.Checked = True
        wholeapr.Checked = False

        sel_month = "04"



        loaddateperiod()

    End Sub

    Private Sub wholeapr_Click(sender As Object, e As EventArgs) Handles wholeapr.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnApr)
        End If

        prevtsbtn = wholeapr

        wholeapr.Checked = True
        onehalfapr.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnMay_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnMay.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnMay.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnMay_Click(sender As Object, e As EventArgs) Handles onehalfmay.Click  'tsbtnMay.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnMay)
        End If

        prevtsbtn = onehalfmay

        onehalfmay.Checked = True
        wholemay.Checked = False

        sel_month = "05"



        loaddateperiod()

    End Sub

    Private Sub wholemay_Click(sender As Object, e As EventArgs) Handles wholemay.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnMay)
        End If

        prevtsbtn = wholemay

        wholemay.Checked = True
        onehalfmay.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnJun_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnJun.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnJun.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnJun_Click(sender As Object, e As EventArgs) Handles onehalfjun.Click  'tsbtnJun.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnJun)
        End If

        prevtsbtn = onehalfjun

        onehalfjun.Checked = True
        wholejun.Checked = False

        sel_month = "06"



        loaddateperiod()

    End Sub

    Private Sub wholejun_Click(sender As Object, e As EventArgs) Handles wholejun.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnJun)
        End If

        prevtsbtn = wholejun

        wholejun.Checked = True
        onehalfjun.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnJul_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnJul.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnJul.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnJul_Click(sender As Object, e As EventArgs) Handles onehalfjul.Click  'tsbtnJul.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnJul)
        End If

        prevtsbtn = onehalfjul

        onehalfjul.Checked = True
        wholejul.Checked = False

        sel_month = "07"



        loaddateperiod()

    End Sub

    Private Sub wholejul_Click(sender As Object, e As EventArgs) Handles wholejul.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnJul)
        End If

        prevtsbtn = wholejul

        wholejul.Checked = True
        onehalfjul.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnAug_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnAug.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnAug.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnAug_Click(sender As Object, e As EventArgs) Handles onehalfaug.Click  'tsbtnAug.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnAug)
        End If

        prevtsbtn = onehalfaug

        onehalfaug.Checked = True
        wholeaug.Checked = False

        sel_month = "08"



        loaddateperiod()

    End Sub

    Private Sub wholeaug_Click(sender As Object, e As EventArgs) Handles wholeaug.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnAug)
        End If

        prevtsbtn = wholeaug

        wholeaug.Checked = True
        onehalfaug.Checked = False


        loaddateperiod(1)

    End Sub

    Private Sub tsbtnSep_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnSep.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnSep.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnSep_Click(sender As Object, e As EventArgs) Handles onehalfsep.Click  'tsbtnSep.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnSep)
        End If

        prevtsbtn = onehalfsep

        onehalfsep.Checked = True
        wholesep.Checked = False

        sel_month = "09"



        loaddateperiod()

    End Sub

    Private Sub wholesep_Click(sender As Object, e As EventArgs) Handles wholesep.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnSep)
        End If

        prevtsbtn = wholesep

        wholesep.Checked = True
        onehalfsep.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnOct_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnOct.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnOct.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnOct_Click(sender As Object, e As EventArgs) Handles onehalfoct.Click  'tsbtnOct.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnOct)
        End If

        prevtsbtn = onehalfoct

        onehalfoct.Checked = True
        wholeoct.Checked = False

        sel_month = 10



        loaddateperiod()

    End Sub

    Private Sub wholeoct_Click(sender As Object, e As EventArgs) Handles wholeoct.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnOct)
        End If

        prevtsbtn = wholeoct

        wholeoct.Checked = True
        onehalfoct.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnNov_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnNov.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnNov.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnNov_Click(sender As Object, e As EventArgs) Handles onehalfnov.Click  'tsbtnNov.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnNov)
        End If

        prevtsbtn = onehalfnov

        onehalfnov.Checked = True
        wholenov.Checked = False

        sel_month = 11



        loaddateperiod()

    End Sub

    Private Sub wholenov_Click(sender As Object, e As EventArgs) Handles wholenov.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnNov)
        End If

        prevtsbtn = wholenov

        wholenov.Checked = True
        onehalfnov.Checked = False



        loaddateperiod(1)

    End Sub

    Private Sub tsbtnDec_MouseEnter(sender As Object, e As EventArgs) Handles tsbtnDec.MouseEnter
        tsdrpdwnbtn1 = DirectCast(sender, ToolStripDropDownButton)
        tsbtnDec.ShowDropDown()
        Timer1.Start()
    End Sub

    Private Sub tsbtnDec_Click(sender As Object, e As EventArgs) Handles onehalfdec.Click  'tsbtnDec.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnDec)
        End If

        prevtsbtn = onehalfdec

        onehalfdec.Checked = True
        wholedec.Checked = False

        sel_month = 12

        loaddateperiod()

    End Sub

    Private Sub wholedec_Click(sender As Object, e As EventArgs) Handles wholedec.Click

        If prevtsbtn IsNot Nothing Then
            unckhkpreobj(prevtsbtn, tsbtnDec)
        End If

        prevtsbtn = wholedec

        wholedec.Checked = True
        onehalfdec.Checked = False



        loaddateperiod(1)

    End Sub

    Sub unchkmenutimeent(Optional prev_tsbtn As Object = Nothing, _
                     Optional prev_tsdrpdwnmenitm As Object = Nothing)

        If TypeOf prev_tsbtn Is ToolStripMenuItem Then
            prev_tsbtn.Checked = False

        ElseIf TypeOf prev_tsbtn Is ToolStripButton Then
            prev_tsbtn.Checked = False

        End If

        Static static_objname As String = Nothing

        Static static_obj As Object = Nothing

        If prev_tsdrpdwnmenitm IsNot Nothing Then
            If static_objname <> DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).Name Then

                If static_obj IsNot Nothing Then

                    'DirectCast(static_obj, ToolStripDropDownItem).ForeColor = _
                    '        Color.Black

                    DirectCast(static_obj, ToolStripDropDownItem).BackColor = _
                        Color.Transparent

                End If

                static_obj = DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem)

                static_objname = DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).Name

                'DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).ForeColor = _
                '        Color.White

                DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).BackColor = _
                    Color.FromArgb(51, 192, 255)

                'MsgBox("If static_obj <> DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).Name Then")

            Else
                'MsgBox("If static_obj = prev_tsdrpdwnmenitm Then")
            End If


        End If

    End Sub

    Sub unckhkpreobj(Optional prev_tsbtn As Object = Nothing, _
                     Optional prev_tsdrpdwnmenitm As Object = Nothing)

        If TypeOf prev_tsbtn Is ToolStripMenuItem Then
            prev_tsbtn.Checked = False

        ElseIf TypeOf prev_tsbtn Is ToolStripButton Then
            prev_tsbtn.Checked = False

        End If

        Static static_objname As String = Nothing

        Static static_obj As Object = Nothing

        If prev_tsdrpdwnmenitm IsNot Nothing Then
            If static_objname <> DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).Name Then

                If static_obj IsNot Nothing Then

                    'DirectCast(static_obj, ToolStripDropDownItem).ForeColor = _
                    '        Color.Black

                    DirectCast(static_obj, ToolStripDropDownItem).BackColor = _
                        Color.Transparent

                End If

                static_obj = DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem)

                static_objname = DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).Name

                'DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).ForeColor = _
                '        Color.White

                DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).BackColor = _
                    Color.FromArgb(51, 192, 255)

                'MsgBox("If static_obj <> DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).Name Then")

            Else
                'MsgBox("If static_obj = prev_tsdrpdwnmenitm Then")
            End If


        End If

    End Sub

    Sub current_month(ByVal monthindex As SByte, _
                      sender As Object, _
                      e As EventArgs)

        Select Case monthindex
            Case 1
                prevtsbtn = onehalfjan
                tsbtnJan_Click(sender, e)

            Case 2
                prevtsbtn = onehalffeb
                tsbtnFeb_Click(sender, e)

            Case 3
                prevtsbtn = onehalfmar
                tsbtnMar_Click(sender, e)

            Case 4
                prevtsbtn = onehalfapr
                tsbtnApr_Click(sender, e)

            Case 5
                prevtsbtn = onehalfmay
                tsbtnMay_Click(sender, e)

            Case 6
                prevtsbtn = onehalfjun
                tsbtnJun_Click(sender, e)

            Case 7
                prevtsbtn = onehalfjul
                tsbtnJul_Click(sender, e)

            Case 8
                prevtsbtn = onehalfaug
                tsbtnAug_Click(sender, e)

            Case 9
                prevtsbtn = onehalfsep
                tsbtnSep_Click(sender, e)

            Case 10
                prevtsbtn = onehalfoct
                tsbtnOct_Click(sender, e)

            Case 11
                prevtsbtn = onehalfnov
                tsbtnNov_Click(sender, e)

            Case 12
                prevtsbtn = onehalfdec
                tsbtnDec_Click(sender, e)

        End Select

        Label3.Text = Format(CDate("2015-" & sel_month & "-01"), "MMMM") & " " & tmpdate

    End Sub



    Sub loaddateperiod(Optional second_payp As SByte = Nothing)

        If TabControl1.SelectedIndex = 0 Then

        ElseIf TabControl1.SelectedIndex = 1 Then

            dgvdatesummary.Focus()

            RemoveHandler dgvdatesummary.CurrentCellChanged, AddressOf dgvdatesummary_CurrentCellChanged
            RemoveHandler dgvdatesummary.SelectionChanged, AddressOf dgvdatesummary_SelectionChanged

            dgvdatesummary.Rows.Clear()

        End If

        Dim the_year As String = tmpdate

        'If tsbtnPrev.Checked = True Then
        '    the_year = StrReverse(getStrBetween(StrReverse(tsbtnPrev.Text), "", "←"))
        'Else
        '    the_year = getStrBetween(tsbtnNxt.Text, "", "→")
        'End If

        'MsgBox(the_year)

        Dim maxdatethismonth = EXECQUER("SELECT LAST_DAY('" & the_year & "-" & sel_month & "-01');")

        maxdatethismonth = Format(CDate(maxdatethismonth), "yyyy-MM-dd")

        'MsgBox(maxdatethismonth)

        'dattbl_dateperiod
        'Dim dayindx As String

        dattbl_dateperiod.Rows.Clear()

        Dim lastbound = CDate(maxdatethismonth).Day

        Dim firstbound = 1

        firstbound = If(second_payp = 0, 1, 16)

        lastbound = If(firstbound = 1, 15, lastbound)

        If TabControl1.SelectedIndex = 0 Then

        ElseIf TabControl1.SelectedIndex = 1 Then

            etentsummapaypID = Val(EXECQUER("SELECT RowID FROM payperiod WHERE PayFromDate='" & the_year & "-" & sel_month & "-" & If(firstbound.ToString.Length = 1, "0" & firstbound, firstbound) & _
                                            "' AND PayToDate='" & the_year & "-" & sel_month & "-" & If(lastbound.ToString.Length = 1, "0" & lastbound, lastbound) & _
                                            "' AND OrganizationID=" & orgztnID & " LIMIT 1;"))

        End If

        For i = firstbound To lastbound
            Dim newdrow As DataRow
            newdrow = dattbl_dateperiod.NewRow

            newdrow("col_day") = i

            'dayindx = If(i.ToString.Length = 1, "0" & i, i)

            newdrow("col_dayofweek") = CInt(CDate(the_year & "-" & sel_month & "-" & i).DayOfWeek)

            dattbl_dateperiod.Rows.Add(newdrow)

        Next

        'For Each drow As DataRow In dattbl_dateperiod.Rows
        '    MsgBox(drow("col_day") & vbNewLine & drow("col_dayofweek"))
        'Next

        Dim selrow = dattbl_dateperiod.Compute("COUNT(col_day)", "col_dayofweek = '6'")

        For ii = 0 To selrow
            dgvdatesummary.Rows.Add()
        Next

        Dim cindx As Integer = 0

        For Each dgvrow As DataGridViewRow In dgvdatesummary.Rows
            For Each c As DataGridViewColumn In dgvdatesummary.Columns
                If dattbl_dateperiod.Rows.Count <> cindx Then
                    If c.Index >= Val(dattbl_dateperiod.Rows(cindx)("col_dayofweek")) Then
                        dgvdatesummary.Item(c.Name, dgvrow.Index).Value = dattbl_dateperiod.Rows(cindx)("col_day")
                        cindx += 1
                    End If
                End If
            Next

        Next

        'tmpdate
        'sel_month 

        For Each dgvr As DataGridViewRow In dgvdatesummary.Rows
            If dgvr.Cells("col_sun").Value = Nothing _
           And dgvr.Cells("col_mon").Value = Nothing _
           And dgvr.Cells("col_tue").Value = Nothing _
           And dgvr.Cells("col_wed").Value = Nothing _
           And dgvr.Cells("col_thu").Value = Nothing _
           And dgvr.Cells("col_fri").Value = Nothing _
           And dgvr.Cells("col_sat").Value = Nothing Then

                dgvdatesummary.Rows.Remove(dgvr)
                Exit For
            End If
        Next

        Label3.Text = Format(CDate(maxdatethismonth), "MMMM") & " " & tmpdate

        If TabControl1.SelectedIndex = 0 Then

        ElseIf TabControl1.SelectedIndex = 1 Then

            dgvdatesummary.Focus()

            If dgvdatesummary.RowCount <> 0 Then
                dgvdatesummary.Item(0, 0).Selected = 1
            End If

            AddHandler dgvdatesummary.CurrentCellChanged, AddressOf dgvdatesummary_CurrentCellChanged
            AddHandler dgvdatesummary.SelectionChanged, AddressOf dgvdatesummary_SelectionChanged

            dgvdatesummary_SelectionChanged(dgvdatesummary, New EventArgs)

        End If

    End Sub

    Private Sub tbpemptimeentsumma_Click(sender As Object, e As EventArgs) Handles tbpemptimeentsumma.Click

    End Sub

    <DllImport("user32.dll")> Shared Function GetLastInputInfo(ByRef plii As LASTINPUTINFO) As Boolean
    End Function
    <StructLayout(LayoutKind.Sequential)> Structure LASTINPUTINFO
        <MarshalAs(UnmanagedType.U4)> Public cbSize As Integer
        <MarshalAs(UnmanagedType.U4)> Public dwTime As Integer
    End Structure
    Dim lastInputInf As New LASTINPUTINFO()
    Dim cycle As Integer = 0

    Dim dattbl_dateperiod As New DataTable

    Private Sub tbpemptimeentsumma_Enter(sender As Object, e As EventArgs) Handles tbpemptimeentsumma.Enter
        Static once As SByte = 0
        If once = 0 Then
            once = 1

            tmpdate = CDate(today_date).Year

            tsbtnPrev.Text = "← " & tmpdate - 1

            'tsbtnPrev.Checked = True

            tsbtnNxt.Text = tmpdate + 1 & " →"

            With dattbl_dateperiod
                '.Columns.Add("col_day", Type.GetType("System.Int32"))
                '.Columns.Add("col_dayofweek", Type.GetType("System.Int32"))

                .Columns.Add("col_day")
                .Columns.Add("col_dayofweek")

            End With

            Label3.Text = Format(CDate(today_date), "MMMM") & " " & tmpdate

            Dim listoflastday = EXECQUER("SELECT CONCAT(DAY(LAST_DAY('" & tmpdate & "-01-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-02-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-03-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-04-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-05-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-06-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-07-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-08-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-09-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-10-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-11-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-12-01')));")

            Dim arrayoflastday = Split(listoflastday, ",")

            jan_lastday = arrayoflastday(0)

            wholejan.Text = "16-" & jan_lastday

            feb_lastday = arrayoflastday(1)

            wholefeb.Text = "16-" & feb_lastday

            mar_lastday = arrayoflastday(2)

            wholemar.Text = "16-" & mar_lastday

            apr_lastday = arrayoflastday(3)

            wholeapr.Text = "16-" & apr_lastday

            may_lastday = arrayoflastday(4)

            wholemay.Text = "16-" & may_lastday

            jun_lastday = arrayoflastday(5)

            wholejun.Text = "16-" & jun_lastday

            jul_lastday = arrayoflastday(6)

            wholejul.Text = "16-" & jul_lastday

            aug_lastday = arrayoflastday(7)

            wholeaug.Text = "16-" & aug_lastday

            sept_lastday = arrayoflastday(8)

            wholesep.Text = "16-" & sept_lastday

            oct_lastday = arrayoflastday(9)

            wholeoct.Text = "16-" & oct_lastday

            nov_lastday = arrayoflastday(10)

            wholenov.Text = "16-" & nov_lastday

            dec_lastday = arrayoflastday(11)

            wholedec.Text = "16-" & dec_lastday

            current_month(CDate(today_date).Month, sender, e)

            dgvdatesummary.Focus()

            'If dgvdatesummary.RowCount <> 0 Then
            '    'dgvdatesummary.Item(0, 0).Selected = True
            '    'timeentrysummfields(dgvdatesummary, "col_sun")
            'End If

            'dgvdatesummary_SelectionChanged(sender, e)

            'lblpublish.Visible = 1
            'lblperfectatt.Visible = 1
            'lblmissin.Visible = 1
            'lblmissout.Visible = 1
            'lbldupin.Visible = 1
            'lbldupout.Visible = 1
            'lbllate.Visible = 1
            'lblundertime.Visible = 1
            'Label21.Visible = 1

            RemoveHandler dgvdatesummary.SelectionChanged, AddressOf dgvdatesummary_SelectionChanged

            RemoveHandler dgvdatesummary.CurrentCellChanged, AddressOf dgvdatesummary_CurrentCellChanged

            AddHandler dgvdatesummary.SelectionChanged, AddressOf dgvdatesummary_SelectionChanged

            AddHandler dgvdatesummary.CurrentCellChanged, AddressOf dgvdatesummary_CurrentCellChanged

            dgvdatesummary.Focus()

        End If

    End Sub

    Private Sub dgvdatesummary_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvdatesummary.RowsRemoved
        'MsgBox("dgvdatesummary_RowsRemoved")

    End Sub

    Dim dgvdatesummaryprevrow As Integer = -1
    Dim dgvdatesummaryprevcol As Integer = -1

    Private Sub dgvdatesummary_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvdatesummary.SelectionChanged

        Static samerowindx As Integer = -1

        If dgvdatesummary.RowCount <> 0 Then

            Dim column_name As String = dgvdatesummary.Columns(dgvdatesummary.CurrentCell.ColumnIndex).Name

            'If column_name = "col_sat" Then
            '    dgvdatesummary.FirstDisplayedScrollingColumnIndex = dgvdatesummary.Columns("col_sat").Index
            '    'dgvdatesummary.HorizontalScrollingOffset = 0
            'End If

            If dgvdatesummary.CurrentRow.Index = dgvdatesummary.RowCount - 1 Then
                If samerowindx <> (dgvdatesummary.RowCount - 1) Then
                    'samerowindx = dgvdatesummary.RowCount - 1
                    dgvdatesummary.FirstDisplayedScrollingRowIndex = dgvdatesummary.RowCount - 1 'dgvdatesummary.CurrentRow.Index
                End If
                'ElseIf dgvdatesummary.CurrentRow.Index = 0 Then
                '    dgvdatesummary.FirstDisplayedScrollingRowIndex = 0
            End If

            If dgvdatesummaryprevrow <> -1 And dgvdatesummaryprevcol <> -1 Then
                If samerowindx <> (dgvdatesummary.RowCount - 1) Then
                    'samerowindx = dgvdatesummary.RowCount - 1
                End If
                dgvdatesummary.Rows(dgvdatesummaryprevrow).Height = 100
                dgvdatesummary.Columns(dgvdatesummaryprevcol).Width = 100
            End If

            If dgvdatesummaryprevrow <> dgvdatesummary.CurrentRow.Index _
                Or dgvdatesummaryprevcol <> dgvdatesummary.Columns(column_name).Index Then

                dgvdatesummaryprevrow = dgvdatesummary.CurrentRow.Index
                dgvdatesummaryprevcol = dgvdatesummary.Columns(column_name).Index
            End If

            If samerowindx <> (dgvdatesummary.RowCount - 1) Then
                samerowindx = dgvdatesummary.RowCount - 1
            End If

            dgvdatesummary.CurrentRow.Height = 230
            dgvdatesummary.Columns(column_name).Width = 230

            timeentrysummfields(dgvdatesummary, _
                                column_name)

            If dgvdatesummary.CurrentRow.Cells(column_name).Value = Nothing Then

            Else

            End If

            'etentsummapaypID
        End If

    End Sub

    Private Sub dgvdatesummary_RowLeave(sender As Object, e As DataGridViewCellEventArgs) 'Handles dgvdatesummary.RowLeave
        If dgvdatesummary.RowCount <> 0 Then
            dgvdatesummary.Rows(e.RowIndex).Height = 100

            'lblpublish.Visible = False
            'lblperfectatt.Visible = False
            'lblmissin.Visible = False
            'lblmissout.Visible = False
            'lbldupin.Visible = False
            'lbldupout.Visible = False
            'lbllate.Visible = False
            'lblundertime.Visible = False
            'Label21.Visible = False

        End If

    End Sub

    Private Sub dgvdatesummary_CellLeave(sender As Object, e As DataGridViewCellEventArgs) 'Handles dgvdatesummary.CellLeave
        If dgvdatesummary.RowCount <> 0 Then
            dgvdatesummary.Columns(e.ColumnIndex).Width = 100

            lblpublish.Visible = False
            lblperfectatt.Visible = False
            lblmissin.Visible = False
            lblmissout.Visible = False
            lbldupin.Visible = False
            lbldupout.Visible = False
            lbllate.Visible = False
            lblundertime.Visible = False
            Label21.Visible = False
        End If

    End Sub

    Private Sub dgvdatesummary_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvdatesummary.Scroll

        If dgvdatesummary.RowCount <> 0 Then

            Dim column_name As String = dgvdatesummary.Columns(dgvdatesummary.CurrentCell.ColumnIndex).Name

            'timeentrysummfields(dgvdatesummary, _
            '                    column_name)

        End If

    End Sub

    Private Sub dgvdatesummary_CurrentCellChanged(sender As Object, e As EventArgs) 'Handles dgvdatesummary.CurrentCellChanged
        If dgvdatesummary.RowCount <> 0 Then

            'If dgvdatesummary.CurrentCell.Selected Then
            Dim column_name As String = Nothing
            Try
                column_name = dgvdatesummary.Columns(dgvdatesummary.CurrentCell.ColumnIndex).Name
            Catch ex As Exception
                column_name = "col_sun"
            End Try

            dgvdatesummary.Columns(column_name).Width = 230

            If column_name = "col_sat" Then
                dgvdatesummary.FirstDisplayedScrollingColumnIndex = dgvdatesummary.Columns("col_sat").Index
                'dgvdatesummary.HorizontalScrollingOffset = 0
            End If

            'timeentrysummfields(dgvdatesummary, _
            '                    column_name)

            'End If

            lblpublish.Text = "Published to Payroll :"
            lblperfectatt.Text = "Perfect attentdance :"
            lblmissin.Text = "Missing clock in :"
            lblmissout.Text = "Missing clock out :"
            lbldupin.Text = "Duplicate clock in :"
            lbldupout.Text = "Duplicate clock out :"
            lbllate.Text = "In late :"
            lblundertime.Text = "Out early :"

            If dgvdatesummary.CurrentRow.Cells(column_name).Value = Nothing Then

            Else
                Dim selday = If(dgvdatesummary.CurrentRow.Cells(column_name).Value.ToString.Length = 1, _
                                "0" & dgvdatesummary.CurrentRow.Cells(column_name).Value, _
                                dgvdatesummary.CurrentRow.Cells(column_name).Value)

                Dim querydate = tmpdate & "-" & sel_month & "-" & selday

                Dim etentsumma As New DataTable

                etentsumma = retAsDatTbl("SELECT" & _
" COALESCE((SELECT COUNT(EmployeeID) FROM paystub WHERE OrganizationID='" & orgztnID & "' AND '" & querydate & "' BETWEEN PayFromDate AND PayToDate),0) 'Published to Payroll'" & _
",COALESCE((SELECT COUNT(EmployeeID) FROM employeetimeentry WHERE OrganizationID='" & orgztnID & "' AND Date='" & querydate & "' AND (RegularHoursWorked!=UndertimeHours OR NightDifferentialHours!=UndertimeHours)),0) 'Perfect attendance'" & _
",COALESCE((SELECT COUNT(EmployeeID) FROM employeetimeentrydetails WHERE OrganizationID='" & orgztnID & "' AND Date='" & querydate & "' AND TimeEntryStatus='missing clock in'),0) 'Missing clock in'" & _
",COALESCE((SELECT COUNT(EmployeeID) FROM employeetimeentrydetails WHERE OrganizationID='" & orgztnID & "' AND Date='" & querydate & "' AND TimeEntryStatus='missing clock out'),0) 'Missing clock out'" & _
",0 'Duplicate clock in'" & _
",0 'Duplicate clock out'" & _
",COALESCE((SELECT COUNT(EmployeeID) FROM employeetimeentry WHERE OrganizationID='" & orgztnID & "' AND Date='" & querydate & "' AND COALESCE(HoursLate,0)>0),0) 'In late'" & _
",COALESCE((SELECT COUNT(EmployeeID) FROM employeetimeentry WHERE OrganizationID='" & orgztnID & "' AND Date='" & querydate & "' AND COALESCE(UndertimeHours,0)>0),0) 'Out early';")

                '",COALESCE((SELECT COUNT(EmployeeID) FROM employeetimeentry WHERE OrganizationID='" & orgztnID & "' AND Date='" & querydate & "' AND RegularHoursWorked!=UndertimeHours),0) 'Perfect attendance'" & _

                For Each drow In etentsumma.Rows
                    lblpublish.Text = "Published to Payroll : " & drow("Published to Payroll").ToString
                    lblperfectatt.Text = "Perfect attentdance : " & drow("Perfect attendance").ToString
                    lblmissin.Text = "Missing clock in : " & drow("Missing clock in").ToString
                    lblmissout.Text = "Missing clock out : " & drow("Missing clock out").ToString
                    lbldupin.Text = "Duplicate clock in : " & drow("Duplicate clock in").ToString
                    lbldupout.Text = "Duplicate clock out : " & drow("Duplicate clock out").ToString
                    lbllate.Text = "In late : " & drow("In late").ToString
                    lblundertime.Text = "Out early : " & drow("Out early").ToString
                    Exit For
                Next

            End If

        End If

    End Sub

    Private Sub dgvdatesummary_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvdatesummary.CellEnter
        If dgvdatesummary.RowCount <> 0 Then
            Dim column_name As String = Nothing
            Try
                column_name = dgvdatesummary.Columns(dgvdatesummary.CurrentCell.ColumnIndex).Name
            Catch ex As Exception
                column_name = "col_sun"
            End Try

            'timeentrysummfields(dgvdatesummary, _
            '                    column_name)

        End If
    End Sub

    Private Sub dgvdatesummary_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvdatesummary.CellContentClick

    End Sub

    Private Sub dgvcalendar_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvcalendar.CellPainting

        If e.RowIndex = -1 Then
            GridDrawCustomHeaderColumns(dgvcalendar, e, _
             My.Resources.ColumnBGStyle005, _
             DGVHeaderImageAlignments.FillCell)

        End If

    End Sub

    Private Sub dgvdatesummary_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvdatesummary.CellPainting

        If e.RowIndex = -1 Then
            GridDrawCustomHeaderColumns(dgvdatesummary, e, _
             My.Resources.ColumnBGStyle005, _
             DGVHeaderImageAlignments.FillCell)

        End If

    End Sub

    Private Sub GridDrawCustomHeaderColumns(ByVal dgv As DataGridView, _
     ByVal e As DataGridViewCellPaintingEventArgs, ByVal img As Image, _
     ByVal Style As DGVHeaderImageAlignments)

        ' All of the graphical Processing is done here.
        Dim gr As Graphics = e.Graphics
        ' Fill the BackGround with the BackGroud Color of Headers.
        ' This step is necessary, for transparent images, or what's behind
        ' would be painted instead.
        gr.FillRectangle( _
         New SolidBrush(dgv.ColumnHeadersDefaultCellStyle.BackColor), _
         e.CellBounds)
        If img IsNot Nothing Then
            Select Case Style
                Case DGVHeaderImageAlignments.FillCell
                    gr.DrawImage( _
                     img, e.CellBounds.X, e.CellBounds.Y, _
                     e.CellBounds.Width, e.CellBounds.Height)
                Case DGVHeaderImageAlignments.SingleCentered
                    gr.DrawImage(img, _
                     ((e.CellBounds.Width - img.Width) \ 2) + e.CellBounds.X, _
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y, _
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleLeft
                    gr.DrawImage(img, e.CellBounds.X, _
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y, _
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleRight
                    gr.DrawImage(img, _
                     (e.CellBounds.Width - img.Width) + e.CellBounds.X, _
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y, _
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
                    gr.DrawImage( _
                     img, e.CellBounds.X, e.CellBounds.Y, _
                     e.ClipBounds.Width, e.CellBounds.Height)
            End Select
        End If
        'e.PaintContent(e.CellBounds)
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
                ' This part could be handled...
                'Select Case dgv.ColumnHeadersDefaultCellStyle.WrapMode
                '	Case DataGridViewTriState.False
                '		.FormatFlags = StringFormatFlags.NoWrap
                '	Case DataGridViewTriState.NotSet
                '		.FormatFlags = StringFormatFlags.NoWrap
                '	Case DataGridViewTriState.True
                '		.FormatFlags = StringFormatFlags.FitBlackBox
                'End Select
                .HotkeyPrefix = Drawing.Text.HotkeyPrefix.None
                .Trimming = StringTrimming.None
            End With

            Dim newForeColor = Color.FromArgb(0, 0, 0)

            With dgv.ColumnHeadersDefaultCellStyle
                gr.DrawString(e.Value.ToString, .Font, _
                 New SolidBrush(newForeColor), e.CellBounds, sf)
            End With
        End Using
        e.Handled = True
    End Sub

    Sub timeentrysummfields(ByVal dgv As DataGridView, _
                        ByVal colName As String, _
                        Optional isVisb As SByte = 0)

        Static paypID As Object = Nothing

        'cbopaytype, _
        '                       txtdescrptn, _
        '                       txtpayrate, _
        '                       txtotrate, _
        '                       txtnightdiffrate, _
        '                       txtnightdiffotrate)

        Static name_column As String = -1

        'If name_column <> colName Then
        '    name_column = colName
        'Else
        '    Exit Sub
        'End If

        Try
            If dgv.CurrentRow.Cells(colName).Selected Then

                'If dgv.Columns(colName).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells Then
                '    Dim wid As Integer = dgv.Columns(colName).Width

                '    Dim x As Integer = dgv.Columns(colName).Width + 25
                '    dgv.Columns(colName).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                '    dgv.Columns(colName).Width = x
                'End If

                Dim rect As Rectangle = dgv.GetCellDisplayRectangle(dgv.CurrentRow.Cells(colName).ColumnIndex, dgv.CurrentRow.Cells(colName).RowIndex, True)

                '*********************Publish to Payroll***************************
                lblpublish.Parent = dgv '            '586               '230       '141              '25      
                lblpublish.Location = New Point(rect.Right - lblpublish.Width, rect.Top + lblpublish.Height)
                lblpublish.Visible = If(isVisb = 0, True, False)





                'cbopaytype.Parent = dgv '                   'btn.Width
                'cbopaytype.Location = New Point(rect.Right - cbopaytype.Width, rect.Top + 18)
                'cbopaytype.Visible = If(isVisb = 0, True, False)

                '********************Perfect attendance****************************
                lblperfectatt.Parent = dgv
                lblperfectatt.Location = New Point(rect.Right - lblperfectatt.Width, rect.Top + (lblpublish.Height * 2))
                lblperfectatt.Visible = If(isVisb = 0, True, False)

                'txtdescrptn.Parent = dgv
                'txtdescrptn.Location = New Point(rect.Right - txtdescrptn.Width, rect.Top + 56)
                'txtdescrptn.Visible = If(isVisb = 0, True, False)

                '**********************Missing clock in**************************
                lblmissin.Parent = dgv
                lblmissin.Location = New Point(rect.Right - lblmissin.Width, rect.Top + (lblperfectatt.Height * 3)) '77
                lblmissin.Visible = If(isVisb = 0, True, False)

                'txtpayrate.Parent = dgv
                'txtpayrate.Location = New Point(rect.Right - txtpayrate.Width, rect.Top + 100) '90
                'txtpayrate.Visible = If(isVisb = 0, True, False)

                '***********************Missing clock out*************************
                lblmissout.Parent = dgv
                lblmissout.Location = New Point(rect.Right - lblmissout.Width, rect.Top + (lblmissin.Height * 4)) '111
                lblmissout.Visible = If(isVisb = 0, True, False)

                'txtotrate.Parent = dgv
                'txtotrate.Location = New Point(rect.Right - txtotrate.Width, rect.Top + 134) '124
                'txtotrate.Visible = If(isVisb = 0, True, False)

                '**********************Duplicate clock in**************************
                lbldupin.Parent = dgv
                lbldupin.Location = New Point(rect.Right - lbldupin.Width, rect.Top + (lblmissout.Height * 5)) '145
                lbldupin.Visible = If(isVisb = 0, True, False)

                'txtnightdiffrate.Parent = dgv
                'txtnightdiffrate.Location = New Point(rect.Right - txtnightdiffrate.Width, rect.Top + 168) '158
                'txtnightdiffrate.Visible = If(isVisb = 0, True, False)

                '**********************Duplicate clock out**************************
                lbldupout.Parent = dgv
                lbldupout.Location = New Point(rect.Right - lbldupout.Width, rect.Top + (lbldupin.Height * 6)) '179
                lbldupout.Visible = If(isVisb = 0, True, False)

                'txtnightdiffotrate.Parent = dgv
                'txtnightdiffotrate.Location = New Point(rect.Right - txtnightdiffotrate.Width, rect.Top + 202) '192
                'txtnightdiffotrate.Visible = If(isVisb = 0, True, False)

                '**********************Count undertime**************************
                lblundertime.Parent = dgv
                lblundertime.Location = New Point(rect.Right - lblundertime.Width, rect.Top + (lbldupout.Height * 7)) '179
                lblundertime.Visible = If(isVisb = 0, True, False)

                '**********************Count late**************************
                lbllate.Parent = dgv
                lbllate.Location = New Point(rect.Right - lbllate.Width, rect.Top + (lblundertime.Height * 8)) '179
                lbllate.Visible = If(isVisb = 0, True, False)

                ' ''**********************Extra label**************************
                'Label21.Parent = dgv
                'Label21.Width = 230
                'Label21.Location = New Point(rect.Right - Label21.Width, rect.Top + (lbllate.Height * 9)) '179
                'Label21.Visible = If(isVisb = 0, True, False)

            Else
                'hideObjFields()
            End If
        Catch ex As Exception
            'MsgBox(ex.Message & " ERR_NO 77-10 : myEllipseButton")
        Finally
            'Dim pub_str = "Published to Payroll : "
            'Dim perf_atten = "Perfect attentdance : "
            'Dim miss_cl_in = "Missing clock in : "
            'Dim miss_cl_out = "Missing clock out : "
            'Dim dup_cl_in = "Duplicate clock in : "
            'Dim dup_cl_out = "Duplicate clock out : "
            'Dim in_late = "In late : "
            'Dim earlyout = "Out early : "

            'If paypID <> etentsummapaypID Then
            '    paypID = etentsummapaypID
            '    lblpublish.Text = pub_str & EXECQUER("SELECT COUNT(EmployeeID) FROM paystub WHERE PayPeriodID=" & etentsummapaypID & ";")

            'End If
            ''colName
            ''dgv
            ''sel_month
            ''tmpdate

            'If dgv.CurrentRow.Cells(colName).Value = Nothing Then
            '    lblmissin.Text = miss_cl_in & " 0"
            'Else
            '    Dim date_day = If(dgv.CurrentRow.Cells(colName).Value.ToString.Length = 1, "0" & dgv.CurrentRow.Cells(colName).Value, dgv.CurrentRow.Cells(colName).Value)

            '    lblmissin.Text = miss_cl_in & " " & EXECQUER("SELECT COUNT(RowID) FROM employeetimeentrydetails WHERE TimeEntryStatus='missing clock in' AND Date='" & _
            '                                           tmpdate & "-" & sel_month & "-" & date_day & "';")

            '    lblmissout.Text = miss_cl_in & " " & EXECQUER("SELECT COUNT(RowID) FROM employeetimeentrydetails WHERE TimeEntryStatus='missing clock out' AND Date='" & _
            '                                           tmpdate & "-" & sel_month & "-" & date_day & "';")

            'End If

        End Try
    End Sub

    Private Sub tbpemptimeent_Click(sender As Object, e As EventArgs) Handles tbpemptimeent.Click

    End Sub

    Dim etent_month As String = Nothing
    Dim etent_day As String = Nothing

    Dim load_tbpemptimeent As SByte = 0

    Sub tbpemptimeent_Enter(sender As Object, e As EventArgs) Handles tbpemptimeent.Enter

        Static once As SByte = 0
        If once = 0 And load_tbpemptimeent = 0 Then
            once = 1
            load_tbpemptimeent = 1

            tmpdate = CDate(today_date).Year

            prevyear.Text = "← " & tmpdate - 1

            'tsbtnPrev.Checked = True

            nxtyear.Text = tmpdate + 1 & " →"

            lblmonthyear.Text = Format(CDate(today_date), "MMMM") & " " & tmpdate

            etent_month = Format(CDate(today_date), "MM")

            etent_day = Format(CDate(today_date), "dd")

            curr_dd = etent_day
            curr_YYYY = tmpdate
            curr_mm = etent_month

            Dim listoflastday = EXECQUER("SELECT CONCAT(DAY(LAST_DAY('" & tmpdate & "-01-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-02-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-03-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-04-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-05-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-06-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-07-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-08-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-09-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-10-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-11-01'))" & _
                                         ",',',DAY(LAST_DAY('" & tmpdate & "-12-01')));")

            Dim arrayoflastday = Split(listoflastday, ",")

            secjan.Text = "16-" & arrayoflastday(0)

            secfeb.Text = "16-" & arrayoflastday(1)

            secmar.Text = "16-" & arrayoflastday(2)

            secapr.Text = "16-" & arrayoflastday(3)

            secmay.Text = "16-" & arrayoflastday(4)

            secjun.Text = "16-" & arrayoflastday(5)

            secjul.Text = "16-" & arrayoflastday(6)

            secaug.Text = "16-" & arrayoflastday(7)

            secsep.Text = "16-" & arrayoflastday(8)

            secoct.Text = "16-" & arrayoflastday(9)

            secnov.Text = "16-" & arrayoflastday(10)

            secdec.Text = "16-" & arrayoflastday(11)

            searchdate = tmpdate & "-" & etent_month & "-" & etent_day

            'loadpayrate() 'searchdate

            txtSimple.AutoCompleteCustomSource = Simple
            txtSimple.AutoCompleteMode = AutoCompleteMode.Suggest
            txtSimple.AutoCompleteSource = AutoCompleteSource.CustomSource

            For Each s As String In Simple
                ComboBox1.Items.Add(s)
            Next

            If etent_month = "01" Then
                drpdwnmenuitmbackcolor(janmonth)
                If etent_day <= 15 Then
                    firstjan_Click(sender, e)
                Else
                    secjan_Click(sender, e)
                End If

            ElseIf etent_month = "02" Then
                drpdwnmenuitmbackcolor(febmonth)
                If etent_day <= 15 Then
                    firstfeb_Click(sender, e)
                Else
                    secfeb_Click(sender, e)
                End If

            ElseIf etent_month = "03" Then
                drpdwnmenuitmbackcolor(marmonth)
                If etent_day <= 15 Then
                    firstmar_Click(sender, e)
                Else
                    secmar_Click(sender, e)
                End If

            ElseIf etent_month = "04" Then
                drpdwnmenuitmbackcolor(aprmonth)
                If etent_day <= 15 Then
                    firstapr_Click(sender, e)
                Else
                    secapr_Click(sender, e)
                End If

            ElseIf etent_month = "05" Then
                drpdwnmenuitmbackcolor(maymonth)
                If etent_day <= 15 Then
                    firstmay_Click(sender, e)
                Else
                    secmay_Click(sender, e)
                End If

            ElseIf etent_month = "06" Then
                drpdwnmenuitmbackcolor(junmonth)

                If etent_day <= 15 Then
                    firstjun_Click(sender, e)
                Else
                    secjun_Click(sender, e)
                End If

            ElseIf etent_month = "07" Then
                drpdwnmenuitmbackcolor(julmonth)
                If etent_day <= 15 Then
                    firstjul_Click(sender, e)
                Else
                    secjul_Click(sender, e)
                End If

            ElseIf etent_month = "08" Then
                drpdwnmenuitmbackcolor(augmonth)
                If etent_day <= 15 Then
                    firstaug_Click(sender, e)
                Else
                    secaug_Click(sender, e)
                End If

            ElseIf etent_month = "09" Then
                drpdwnmenuitmbackcolor(sepmonth)
                If etent_day <= 15 Then
                    firstsep_Click(sender, e)
                Else
                    secsep_Click(sender, e)
                End If

            ElseIf etent_month = "10" Then
                drpdwnmenuitmbackcolor(octmonth)
                If etent_day <= 15 Then
                    firstoct_Click(sender, e)
                Else
                    secoct_Click(sender, e)
                End If

            ElseIf etent_month = "11" Then
                drpdwnmenuitmbackcolor(novmonth)
                If etent_day <= 15 Then
                    firstnov_Click(sender, e)
                Else
                    secnov_Click(sender, e)
                End If

            ElseIf etent_month = "12" Then
                drpdwnmenuitmbackcolor(decmonth)
                If etent_day <= 15 Then
                    firstdec_Click(sender, e)
                Else
                    secdec_Click(sender, e)
                End If

            End If

            If bgworkloademployees.IsBusy Then
                bgworkloademployees.CancelAsync()
            End If

            bgworkloademployees.RunWorkerAsync()

        End If

    End Sub

    Sub drpdwnmenuitmbackcolor(Optional prev_tsdrpdwnmenitm As Object = Nothing)
        Static static_obj As Object = Nothing
        Static static_objname As String = Nothing

        If prev_tsdrpdwnmenitm IsNot Nothing Then
            If static_objname <> DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).Name Then

                If static_obj IsNot Nothing Then

                    'DirectCast(static_obj, ToolStripDropDownItem).ForeColor = _
                    '        Color.Black

                    DirectCast(static_obj, ToolStripDropDownItem).BackColor = _
                        Color.Transparent

                End If

                static_obj = DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem)

                static_objname = DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).Name

                'DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).ForeColor = _
                '        Color.White

                DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).BackColor = _
                    Color.FromArgb(51, 192, 255)

                'MsgBox("If static_obj <> DirectCast(prev_tsdrpdwnmenitm, ToolStripDropDownItem).Name Then")

            Else
                'MsgBox("If static_obj = prev_tsdrpdwnmenitm Then")
            End If


        End If
    End Sub

    Private Sub prevyear_Click(sender As Object, e As EventArgs) Handles prevyear.Click

        rmvhandlr()

        tmpdate = tmpdate - 1

        prevyear.Text = "← " & (tmpdate - 1)

        nxtyear.Text = (tmpdate + 1) & " →"

        If etent_month = 2 Then
            etent_day = EXECQUER("SELECT DATE_FORMAT(LAST_DAY('" & tmpdate & "-" & etent_month & "-01'),'%d');")
        ElseIf etent_month = "2" Then
            etent_day = EXECQUER("SELECT DATE_FORMAT(LAST_DAY('" & tmpdate & "-" & etent_month & "-01'),'%d');")
        ElseIf etent_month = "02" Then
            etent_day = EXECQUER("SELECT DATE_FORMAT(LAST_DAY('" & tmpdate & "-" & etent_month & "-01'),'%d');")
        End If

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        curr_YYYY = tmpdate

        Dim listoflastday = EXECQUER("SELECT CONCAT(DAY(LAST_DAY('" & tmpdate & "-01-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-02-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-03-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-04-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-05-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-06-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-07-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-08-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-09-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-10-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-11-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-12-01')));")

        Dim arrayoflastday = Split(listoflastday, ",")

        secjan.Text = "16-" & arrayoflastday(0)

        secfeb.Text = "16-" & arrayoflastday(1)

        secmar.Text = "16-" & arrayoflastday(2)

        secapr.Text = "16-" & arrayoflastday(3)

        secmay.Text = "16-" & arrayoflastday(4)

        secjun.Text = "16-" & arrayoflastday(5)

        secjul.Text = "16-" & arrayoflastday(6)

        secaug.Text = "16-" & arrayoflastday(7)

        secsep.Text = "16-" & arrayoflastday(8)

        secoct.Text = "16-" & arrayoflastday(9)

        secnov.Text = "16-" & arrayoflastday(10)

        secdec.Text = "16-" & arrayoflastday(11)

        'tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

    End Sub

    Private Sub nxtyear_Click(sender As Object, e As EventArgs) Handles nxtyear.Click

        rmvhandlr()

        tmpdate = Val(tmpdate) + 1

        nxtyear.Text = (tmpdate + 1) & " →"

        prevyear.Text = "← " & (tmpdate - 1)

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        curr_YYYY = tmpdate

        Dim listoflastday = EXECQUER("SELECT CONCAT(DAY(LAST_DAY('" & tmpdate & "-01-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-02-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-03-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-04-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-05-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-06-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-07-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-08-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-09-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-10-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-11-01'))" & _
                                     ",',',DAY(LAST_DAY('" & tmpdate & "-12-01')));")

        Dim arrayoflastday = Split(listoflastday, ",")

        secjan.Text = "16-" & arrayoflastday(0)

        secfeb.Text = "16-" & arrayoflastday(1)

        secmar.Text = "16-" & arrayoflastday(2)

        secapr.Text = "16-" & arrayoflastday(3)

        secmay.Text = "16-" & arrayoflastday(4)

        secjun.Text = "16-" & arrayoflastday(5)

        secjul.Text = "16-" & arrayoflastday(6)

        secaug.Text = "16-" & arrayoflastday(7)

        secsep.Text = "16-" & arrayoflastday(8)

        secoct.Text = "16-" & arrayoflastday(9)

        secnov.Text = "16-" & arrayoflastday(10)

        secdec.Text = "16-" & arrayoflastday(11)

        'tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

    End Sub

    Private Sub firstjan_Click(sender As Object, e As EventArgs) Handles firstjan.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, janmonth)
        End If

        prevtsbtn = firstjan

        firstjan.Checked = True
        secjan.Checked = False

        etent_month = "01"
        etent_day = StrReverse(getStrBetween(StrReverse(firstjan.Text), "", "-"))

        curr_dd = etent_day
        curr_YYYY = tmpdate
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(janmonth)
    End Sub

    Private Sub secjan_Click(sender As Object, e As EventArgs) Handles secjan.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, janmonth)
        End If

        prevtsbtn = secjan

        secjan.Checked = True
        firstjan.Checked = False

        etent_month = "01"
        etent_day = StrReverse(getStrBetween(StrReverse(secjan.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(janmonth)
    End Sub

    Private Sub firstfeb_Click(sender As Object, e As EventArgs) Handles firstfeb.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, febmonth)
        End If

        prevtsbtn = firstfeb

        firstfeb.Checked = True
        secfeb.Checked = False

        etent_month = "02"
        etent_day = StrReverse(getStrBetween(StrReverse(firstfeb.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(febmonth)
    End Sub

    Private Sub secfeb_Click(sender As Object, e As EventArgs) Handles secfeb.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, febmonth)
        End If

        prevtsbtn = secfeb

        secfeb.Checked = True
        firstfeb.Checked = False

        etent_month = "02"
        etent_day = StrReverse(getStrBetween(StrReverse(secfeb.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(febmonth)
    End Sub

    Private Sub firstmar_Click(sender As Object, e As EventArgs) Handles firstmar.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, marmonth)
        End If

        prevtsbtn = firstmar

        firstmar.Checked = True
        secmar.Checked = False

        etent_month = "03"
        etent_day = StrReverse(getStrBetween(StrReverse(firstmar.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(marmonth)
    End Sub

    Private Sub secmar_Click(sender As Object, e As EventArgs) Handles secmar.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, marmonth)
        End If

        prevtsbtn = secmar

        secmar.Checked = True
        firstmar.Checked = False

        etent_month = "03"
        etent_day = StrReverse(getStrBetween(StrReverse(secmar.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(marmonth)
    End Sub

    Private Sub firstapr_Click(sender As Object, e As EventArgs) Handles firstapr.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, aprmonth)
        End If

        prevtsbtn = firstapr

        firstapr.Checked = True
        secapr.Checked = False

        etent_month = "04"
        etent_day = StrReverse(getStrBetween(StrReverse(firstapr.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(aprmonth)
    End Sub

    Private Sub secapr_Click(sender As Object, e As EventArgs) Handles secapr.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, aprmonth)
        End If

        prevtsbtn = secapr

        secapr.Checked = True
        firstapr.Checked = False

        etent_month = "04"
        etent_day = StrReverse(getStrBetween(StrReverse(secapr.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(aprmonth)
    End Sub

    Private Sub firstmay_Click(sender As Object, e As EventArgs) Handles firstmay.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, maymonth)
        End If

        prevtsbtn = firstmay

        firstmay.Checked = True
        secmay.Checked = False

        etent_month = "05"
        etent_day = StrReverse(getStrBetween(StrReverse(firstmay.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(maymonth)
    End Sub

    Private Sub secmay_Click(sender As Object, e As EventArgs) Handles secmay.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, maymonth)
        End If

        prevtsbtn = secmay

        secmay.Checked = True
        firstmay.Checked = False

        etent_month = "05"
        etent_day = StrReverse(getStrBetween(StrReverse(secmay.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(maymonth)
    End Sub

    Private Sub firstjun_Click(sender As Object, e As EventArgs) Handles firstjun.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, junmonth)
        End If

        prevtsbtn = firstjun

        firstjun.Checked = True
        secjun.Checked = False

        etent_month = "06"
        etent_day = StrReverse(getStrBetween(StrReverse(firstjun.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(junmonth)

    End Sub

    Private Sub secjun_Click(sender As Object, e As EventArgs) Handles secjun.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, junmonth)
        End If

        prevtsbtn = secjun

        secjun.Checked = True
        firstjun.Checked = False

        etent_month = "06"
        etent_day = StrReverse(getStrBetween(StrReverse(secjun.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(junmonth)
    End Sub

    Private Sub firstjul_Click(sender As Object, e As EventArgs) Handles firstjul.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, julmonth)
        End If

        prevtsbtn = firstjul

        firstjul.Checked = True
        secjul.Checked = False

        etent_month = "07"
        etent_day = StrReverse(getStrBetween(StrReverse(firstjul.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(julmonth)
    End Sub

    Private Sub secjul_Click(sender As Object, e As EventArgs) Handles secjul.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, julmonth)
        End If

        prevtsbtn = secjul

        secjul.Checked = True
        firstjul.Checked = False

        etent_month = "07"
        etent_day = StrReverse(getStrBetween(StrReverse(secjul.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(julmonth)
    End Sub

    Private Sub firstaug_Click(sender As Object, e As EventArgs) Handles firstaug.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, augmonth)
        End If

        prevtsbtn = firstaug

        firstaug.Checked = True
        secaug.Checked = False

        etent_month = "08"
        etent_day = StrReverse(getStrBetween(StrReverse(firstaug.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(augmonth)
    End Sub

    Private Sub secaug_Click(sender As Object, e As EventArgs) Handles secaug.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, augmonth)
        End If

        prevtsbtn = secaug

        secaug.Checked = True
        firstaug.Checked = False

        etent_month = "08"
        etent_day = StrReverse(getStrBetween(StrReverse(secaug.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(augmonth)
    End Sub

    Private Sub firstsep_Click(sender As Object, e As EventArgs) Handles firstsep.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, sepmonth)
        End If

        prevtsbtn = firstsep

        firstsep.Checked = True
        secsep.Checked = False

        etent_month = "09"
        etent_day = StrReverse(getStrBetween(StrReverse(firstsep.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(sepmonth)
    End Sub

    Private Sub secsep_Click(sender As Object, e As EventArgs) Handles secsep.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, sepmonth)
        End If

        prevtsbtn = secsep

        secsep.Checked = True
        firstsep.Checked = False

        etent_month = "09"
        etent_day = StrReverse(getStrBetween(StrReverse(secsep.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(sepmonth)
    End Sub

    Private Sub firstoct_Click(sender As Object, e As EventArgs) Handles firstoct.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, octmonth)
        End If

        prevtsbtn = firstoct

        firstoct.Checked = True
        secoct.Checked = False

        etent_month = "10"
        etent_day = StrReverse(getStrBetween(StrReverse(firstoct.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(octmonth)
    End Sub

    Private Sub secoct_Click(sender As Object, e As EventArgs) Handles secoct.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, octmonth)
        End If

        prevtsbtn = secoct

        secoct.Checked = True
        firstoct.Checked = False

        etent_month = "10"
        etent_day = StrReverse(getStrBetween(StrReverse(secoct.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(octmonth)
    End Sub

    Private Sub firstnov_Click(sender As Object, e As EventArgs) Handles firstnov.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, novmonth)
        End If

        prevtsbtn = firstnov

        firstnov.Checked = True
        secnov.Checked = False

        etent_month = "11"
        etent_day = StrReverse(getStrBetween(StrReverse(firstnov.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(novmonth)
    End Sub

    Private Sub secnov_Click(sender As Object, e As EventArgs) Handles secnov.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, novmonth)
        End If

        prevtsbtn = secnov

        secnov.Checked = True
        firstnov.Checked = False

        etent_month = "11"
        etent_day = StrReverse(getStrBetween(StrReverse(secnov.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

        drpdwnmenuitmbackcolor(novmonth)
    End Sub

    Private Sub firstdec_Click(sender As Object, e As EventArgs) Handles firstdec.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, decmonth)
        End If

        prevtsbtn = firstdec

        firstdec.Checked = True
        secdec.Checked = False

        etent_month = "12"
        etent_day = StrReverse(getStrBetween(StrReverse(firstdec.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

    End Sub

    Private Sub secdec_Click(sender As Object, e As EventArgs) Handles secdec.Click

        rmvhandlr()

        If prevtsbtn IsNot Nothing Then
            unchkmenutimeent(prevtsbtn, decmonth)
        End If

        prevtsbtn = secdec

        secdec.Checked = True
        firstdec.Checked = False

        etent_month = "12"
        etent_day = StrReverse(getStrBetween(StrReverse(secdec.Text), "", "-"))

        curr_dd = etent_day
        tmpdate = curr_YYYY
        curr_mm = etent_month

        searchdate = tmpdate & "-" & etent_month & "-" & etent_day
        loadpayrate(searchdate)

        hideObjFields()
        dgvEmployi_SelectionChanged(sender, e)
        addhandlr()

        lblmonthyear.Text = Format(CDate(tmpdate & "-" & etent_month & "-" & etent_day), "MMMM") & " " & tmpdate

    End Sub

    Dim tsdrpdwnbtn0 As ToolStripDropDownButton

    Private Sub ToolStripDropDownButton_ShowDropDown(sender As Object, e As EventArgs) Handles janmonth.MouseEnter, febmonth.MouseEnter, marmonth.MouseEnter, aprmonth.MouseEnter, _
                                                                                    maymonth.MouseEnter, junmonth.MouseEnter, julmonth.MouseEnter, augmonth.MouseEnter, _
                                                                                    sepmonth.MouseEnter, octmonth.MouseEnter, novmonth.MouseEnter, decmonth.MouseEnter

        DirectCast(sender, ToolStripDropDownButton).ShowDropDown()
        tsdrpdwnbtn0 = DirectCast(sender, ToolStripDropDownButton)
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lastInputInf.cbSize = Marshal.SizeOf(lastInputInf)
        lastInputInf.dwTime = 0
        GetLastInputInfo(lastInputInf)
        'lblTime.Text = CInt((Environment.TickCount - lastInputInf.dwTime) / 1000).ToString
        If CInt((Environment.TickCount - lastInputInf.dwTime) / 1000) > 1 Then 'check if it has been 60 seconds
            'Do whatever commands here that you want to happen
            'if no user input is detected in last 60 seconds.
            Timer1.Stop()

            If TabControl1.SelectedIndex = 0 Then
                If tsdrpdwnbtn0 IsNot Nothing Then
                    tsdrpdwnbtn0.HideDropDown()
                End If
            ElseIf TabControl1.SelectedIndex = 1 Then
                If tsdrpdwnbtn1 IsNot Nothing Then
                    tsdrpdwnbtn1.HideDropDown()
                End If
            End If

        Else

        End If
    End Sub

    Private Sub bgworkloademployees_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkloademployees.DoWork
        backgroundworking = 1
    End Sub

    Private Sub bgworkloademployees_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkloademployees.ProgressChanged

    End Sub

    Private Sub bgworkloademployees_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkloademployees.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Error: " & vbNewLine & e.Error.Message)
            'MessageBox.Show
        ElseIf e.Cancelled Then
            MsgBox("Background work cancelled.", _
                   MsgBoxStyle.Exclamation)
        Else
            'MsgBox("Done!", _
            '       MsgBoxStyle.Information)

        End If
        backgroundworking = 0
    End Sub

    Private Sub txthrsworkd_KeyDown(sender As Object, e As KeyEventArgs) Handles txthrsworkd.KeyDown

        If e.Control AndAlso e.KeyCode = Keys.C Then
            txthrsworkd.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txthrsworkd.Paste()

        End If

    End Sub

    Private Sub txthrsworkd_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txthrsworkd.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txthrsworkd.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txthrsworkd.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txthrsworkd_TextChanged(sender As Object, e As EventArgs) Handles txthrsworkd.TextChanged

    End Sub

    Private Sub txthrsworkd_Leave(sender As Object, e As EventArgs) Handles txthrsworkd.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_tothrsworked").Value.ToString) <> Trim(txthrsworkd.Text) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)

                    dgvetentsemimon.Item("etiment_tothrsworked", prevdgvrowetentsemimon.Index).Value = Trim(txthrsworkd.Text)
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_tothrsworked") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try

    End Sub

    Private Sub txtreghrsworkd_KeyDown(sender As Object, e As KeyEventArgs) Handles txtreghrsworkd.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txtreghrsworkd.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txtreghrsworkd.Paste()

        End If

    End Sub

    Private Sub txtreghrsworkd_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtreghrsworkd.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtreghrsworkd.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtreghrsworkd.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtreghrsworkd_TextChanged(sender As Object, e As EventArgs) Handles txtreghrsworkd.TextChanged

    End Sub

    Private Sub txtreghrsworkd_Leave(sender As Object, e As EventArgs) Handles txtreghrsworkd.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_RegHrsWork").Value.ToString) <> Trim(txtreghrsworkd.Text) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_RegHrsWork", prevdgvrowetentsemimon.Index).Value = Trim(txtreghrsworkd.Text)
                End If
            End If
        Catch ex As Exception
            'Column named etiment_tothrsworked cannot be found. Parameter Name : columnName()
            If ex.Message.Contains("etiment_RegHrsWork") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txtregpay_KeyDown(sender As Object, e As KeyEventArgs) Handles txtregpay.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txtregpay.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txtregpay.Paste()

        End If

    End Sub

    Private Sub txtregpay_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtregpay.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtregpay.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtregpay.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtregpay_Leave(sender As Object, e As EventArgs) Handles txtregpay.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_reghrsamt").Value.ToString) <> Trim(txtregpay.Text.Replace(",", "")) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_reghrsamt", prevdgvrowetentsemimon.Index).Value = Trim(txtregpay.Text.Replace(",", ""))
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_reghrsamt") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txtregpay_TextChanged(sender As Object, e As EventArgs) Handles txtregpay.TextChanged

    End Sub

    Private Sub txthrsOT_KeyDown(sender As Object, e As KeyEventArgs) Handles txthrsOT.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txthrsOT.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txthrsOT.Paste()

        End If

    End Sub

    Private Sub txthrsOT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txthrsOT.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txthrsOT.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txthrsOT.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txthrsOT_Leave(sender As Object, e As EventArgs) Handles txthrsOT.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_OTHrsWork").Value.ToString) <> Trim(txthrsOT.Text) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_OTHrsWork", prevdgvrowetentsemimon.Index).Value = Trim(txthrsOT.Text)
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_OTHrsWork") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txthrsOT_TextChanged(sender As Object, e As EventArgs) Handles txthrsOT.TextChanged

    End Sub

    Private Sub txtotpay_KeyDown(sender As Object, e As KeyEventArgs) Handles txtotpay.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txtotpay.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txtotpay.Paste()

        End If

    End Sub

    Private Sub txtotpay_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtotpay.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtotpay.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtotpay.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtotpay_Leave(sender As Object, e As EventArgs) Handles txtotpay.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_otpay").Value.ToString) <> Trim(txtotpay.Text.Replace(",", "")) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_otpay", prevdgvrowetentsemimon.Index).Value = Trim(txtotpay.Text.Replace(",", ""))
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_otpay") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txtotpay_TextChanged(sender As Object, e As EventArgs) Handles txtotpay.TextChanged

    End Sub

    Private Sub txthrsUT_KeyDown(sender As Object, e As KeyEventArgs) Handles txthrsUT.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txthrsUT.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txthrsUT.Paste()

        End If

    End Sub

    Private Sub txthrsUT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txthrsUT.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txthrsUT.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txthrsUT.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txthrsUT_Leave(sender As Object, e As EventArgs) Handles txthrsUT.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_UTHrsWork").Value.ToString) <> Trim(txthrsUT.Text) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_UTHrsWork", prevdgvrowetentsemimon.Index).Value = Trim(txthrsUT.Text)
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_UTHrsWork") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txthrsUT_TextChanged(sender As Object, e As EventArgs) Handles txthrsUT.TextChanged

    End Sub

    Private Sub txtutamount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtutamount.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txtutamount.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txtutamount.Paste()

        End If

    End Sub

    Private Sub txtutamount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtutamount.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtutamount.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtutamount.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtutamount_Leave(sender As Object, e As EventArgs) Handles txtutamount.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_utamt").Value.ToString) <> Trim(txtutamount.Text.Replace(",", "")) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_utamt", prevdgvrowetentsemimon.Index).Value = Trim(txtutamount.Text.Replace(",", ""))
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_utamt") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txtutamount_TextChanged(sender As Object, e As EventArgs) Handles txtutamount.TextChanged

    End Sub

    Private Sub txthrslate_KeyDown(sender As Object, e As KeyEventArgs) Handles txthrslate.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txthrslate.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txthrslate.Paste()

        End If

    End Sub

    Private Sub txthrslate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txthrslate.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txthrslate.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txthrslate.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txthrslate_Leave(sender As Object, e As EventArgs) Handles txthrslate.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_Hrslate").Value.ToString) <> Trim(txthrslate.Text) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_Hrslate", prevdgvrowetentsemimon.Index).Value = Trim(txthrslate.Text)
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_Hrslate") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txthrslate_TextChanged(sender As Object, e As EventArgs) Handles txthrslate.TextChanged

    End Sub

    Private Sub txtlateamount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtlateamount.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txtlateamount.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txtlateamount.Paste()

        End If

    End Sub

    Private Sub txtlateamount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtlateamount.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtlateamount.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtlateamount.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtlateamount_Leave(sender As Object, e As EventArgs) Handles txtlateamount.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_lateamt").Value.ToString) <> Trim(txtlateamount.Text.Replace(",", "")) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_lateamt", prevdgvrowetentsemimon.Index).Value = Trim(txtlateamount.Text.Replace(",", ""))
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_lateamt") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txtlateamount_TextChanged(sender As Object, e As EventArgs) Handles txtlateamount.TextChanged

    End Sub

    Private Sub txtnightdiff_KeyDown(sender As Object, e As KeyEventArgs) Handles txtnightdiff.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txtnightdiff.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txtnightdiff.Paste()

        End If

    End Sub

    Private Sub txtnightdiff_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnightdiff.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtnightdiff.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtnightdiff.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtnightdiff_Leave(sender As Object, e As EventArgs) Handles txtnightdiff.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_NightDiffHrs").Value.ToString) <> Trim(txtnightdiff.Text) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_NightDiffHrs", prevdgvrowetentsemimon.Index).Value = Trim(txtnightdiff.Text)
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_NightDiffHrs") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txtnightdiff_TextChanged(sender As Object, e As EventArgs) Handles txtnightdiff.TextChanged

    End Sub

    Private Sub txtnightdiffpay_KeyDown(sender As Object, e As KeyEventArgs) Handles txtnightdiffpay.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txtnightdiffpay.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txtnightdiffpay.Paste()

        End If

    End Sub

    Private Sub txtnightdiffpay_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnightdiffpay.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtnightdiffpay.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtnightdiffpay.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtnightdiffpay_Leave(sender As Object, e As EventArgs) Handles txtnightdiffpay.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_nightdiffpay").Value.ToString) <> Trim(txtnightdiffpay.Text.Replace(",", "")) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_nightdiffpay", prevdgvrowetentsemimon.Index).Value = Trim(txtnightdiffpay.Text.Replace(",", ""))
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_nightdiffpay") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txtnightdiffpay_TextChanged(sender As Object, e As EventArgs) Handles txtnightdiffpay.TextChanged

    End Sub

    Private Sub txtnightdiffOT_KeyDown(sender As Object, e As KeyEventArgs) Handles txtnightdiffOT.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txtnightdiffOT.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txtnightdiffOT.Paste()

        End If

    End Sub

    Private Sub txtnightdiffOT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnightdiffOT.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtnightdiffOT.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtnightdiffOT.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtnightdiffOT_Leave(sender As Object, e As EventArgs) Handles txtnightdiffOT.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_NightDiffOTHrs").Value.ToString) <> Trim(txtnightdiffOT.Text) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_NightDiffOTHrs", prevdgvrowetentsemimon.Index).Value = Trim(txtnightdiffOT.Text)
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_NightDiffOTHrs") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txtnightdiffOT_TextChanged(sender As Object, e As EventArgs) Handles txtnightdiffOT.TextChanged

    End Sub

    Private Sub txtnightdiffotpay_KeyDown(sender As Object, e As KeyEventArgs) Handles txtnightdiffotpay.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txtnightdiffotpay.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txtnightdiffotpay.Paste()

        End If

    End Sub

    Private Sub txtnightdiffotpay_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnightdiffotpay.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtnightdiffotpay.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtnightdiffotpay.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtnightdiffotpay_Leave(sender As Object, e As EventArgs) Handles txtnightdiffotpay.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_nightdiffotpay").Value.ToString) <> Trim(txtnightdiffotpay.Text.Replace(",", "")) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_nightdiffotpay", prevdgvrowetentsemimon.Index).Value = Trim(txtnightdiffotpay.Text.Replace(",", ""))
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_nightdiffotpay") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txtnightdiffotpay_TextChanged(sender As Object, e As EventArgs) Handles txtnightdiffotpay.TextChanged

    End Sub

    Dim listOfEditetimeent As New List(Of String)

    Private Sub dgvetentsemimon_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvetentsemimon.CellContentClick

    End Sub

    Private Sub janmonth_Click(sender As Object, e As EventArgs) Handles janmonth.Click

    End Sub

    Dim curr_empRow As Integer = Nothing
    Dim curr_empColm As String = Nothing

    Sub btnRerfresh_Click(sender As Object, e As EventArgs) Handles btnRerfresh.Click


        RemoveHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged

        If dgvEmployi.RowCount <> 0 Then
            curr_empRow = dgvEmployi.CurrentRow.Index
            curr_empColm = dgvEmployi.Columns(dgvEmployi.CurrentCell.ColumnIndex).Name

            dgvEmployi.Item(curr_empColm, curr_empRow).Selected = True
        End If

        If TabControl2.SelectedIndex = 0 Then
            'Dim q_search = searchCommon(ComboBox7, TextBox2, _
            '                            ComboBox8, TextBox15, _
            '                            ComboBox9, TextBox16, _
            '                            ComboBox10, TextBox17)

            Dim dtemployee As New DataTable

            'If q_search = Nothing Then
            '    '    dgvRowAdder(q_employee & " ORDER BY e.RowID DESC LIMIT " & pagination & ",100;", dgvEmployi)
            '    dtemployee = New SQLQueryToDatatable(q_employee & " ORDER BY e.RowID DESC LIMIT " & pagination & "," & page_limiter & ";").ResultTable
            'Else
            '    '    loademployees(" AND " & q_search)
            '    dtemployee = New SQLQueryToDatatable(q_employee & " AND " & q_search & " ORDER BY e.RowID DESC LIMIT " & pagination & "," & page_limiter & ";").ResultTable
            'End If

            Dim param_array = New Object() {orgztnID,
                                            TextBox2.Text,
                                            TextBox15.Text,
                                            TextBox16.Text,
                                            pagination}

            Dim n_ReadSQLProcedureToDatatable As New  _
                ReadSQLProcedureToDatatable("SEARCH_employeetimeentry",
                                            param_array)

            dtemployee = n_ReadSQLProcedureToDatatable.ResultTable

            dgvEmployi.Rows.Clear()

            For Each drow As DataRow In dtemployee.Rows

                Dim rowArray = drow.ItemArray()

                dgvEmployi.Rows.Add(rowArray)

            Next

            dtemployee.Dispose()

        Else
            'If isKPressSimple = 1 Then
            '    If txtSimple.Text = "" Then
            '        dgvRowAdder(q_employee & " ORDER BY e.RowID DESC LIMIT " & pagination & ",100;", dgvEmp)
            '    Else
            '        searchEmpSimple()
            '    End If
            'Else
            If txtSimple.Text = "" Then
                dgvRowAdder(q_employee & " ORDER BY e.RowID DESC LIMIT " & pagination & ",100;", dgvEmployi)
            Else
                searchEmpSimple()
            End If



            '    'If colSearchSimple = -1 Then
            '    '    dgvRowAdder(q_employee & " ORDER BY e.RowID DESC", dgvEmp)
            '    'Else
            '    '    'dgvRowAdder(q_employee & q_empsearch & " ORDER BY e.RowID DESC", dgvEmp)
            '    'End If
            'End If

        End If

        If dgvEmployi.RowCount <> 0 Then
            If curr_empRow <= dgvEmployi.RowCount - 1 Then
                dgvEmployi.Item(curr_empColm, curr_empRow).Selected = True
            Else
                dgvEmployi.Item(curr_empColm, 0).Selected = True
            End If
            dgvEmployi_SelectionChanged(sender, e)
        End If

        AddHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged
        'RemoveHandler tsbtnNewEmp.Click, AddressOf tsbtnNewEmp_Click

    End Sub

    Function searchCommon(Optional cbox1 As ComboBox = Nothing, Optional search1 As Object = Nothing,
                          Optional cbox2 As ComboBox = Nothing, Optional search2 As Object = Nothing, _
                          Optional cbox3 As ComboBox = Nothing, Optional search3 As Object = Nothing,
                          Optional cbox4 As ComboBox = Nothing, Optional search4 As Object = Nothing, _
                          Optional cbox5 As ComboBox = Nothing, Optional search5 As Object = Nothing) As String

        '=============================================

        Dim _search1, _search2, _search3, _search4, _search5 As String ', ordate, credate

        Select Case cbox1.SelectedIndex
            Case 0
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID LIKE '" & search1.Text & "%'")
            Case 1
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID LIKE '%" & search1.Text & "%'")
            Case 2
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID = '" & search1.Text & "'")
            Case 3
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID NOT LIKE '%" & search1.Text & "%'")
            Case 4
                _search1 = " e.EmployeeID IS NULL"
            Case 5
                _search1 = " e.EmployeeID IS NOT NULL"
            Case Else
                '_search1 = ""
                _search1 = If(search1.Text = "", Nothing, " e.EmployeeID = '" & search1.Text & "'")
        End Select

        '=============================================

        Select Case cbox2.SelectedIndex
            Case 0
                _search2 = If(search2.Text = "", Nothing, " e.FirstName LIKE '" & search2.Text & "%'")
            Case 1
                _search2 = If(search2.Text = "", Nothing, " e.FirstName LIKE '%" & search2.Text & "%'")
            Case 2
                _search2 = If(search2.Text = "", Nothing, " e.FirstName = '" & search2.Text & "'")
            Case 3
                _search2 = If(search2.Text = "", Nothing, " e.FirstName NOT LIKE '%" & search2.Text & "%'")
            Case 4
                _search2 = " e.FirstName IS NULL"
            Case 5
                _search2 = " e.FirstName IS NOT NULL"
            Case Else
                '_search2 = ""
                _search2 = If(search2.Text = "", Nothing, " e.FirstName = '" & search2.Text & "'")
        End Select

        If _search1 <> "" And _search2 <> "" Then
            _search2 = " AND" & _search2
        End If

        '===============================================================

        Select Case cbox3.SelectedIndex
            Case 0
                _search3 = If(search3.Text = "", Nothing, " e.LastName LIKE '" & search3.Text & "%'")
            Case 1
                _search3 = If(search3.Text = "", Nothing, " e.LastName LIKE '%" & search3.Text & "%'")
            Case 2
                _search3 = If(search3.Text = "", Nothing, " e.LastName = '" & search3.Text & "'")
            Case 3
                _search3 = If(search3.Text = "", Nothing, " e.LastName NOT LIKE '%" & search3.Text & "%'")
            Case 4
                _search3 = " e.LastName IS NULL"
            Case 5
                _search3 = " e.LastName IS NOT NULL"
            Case Else
                '_search3 = ""
                _search3 = If(search3.Text = "", Nothing, " e.LastName = '" & search3.Text & "'")
        End Select

        If (_search1 <> "" Or _search2 <> "") And _search3 <> "" Then
            _search3 = " AND" & _search3
        End If

        '===============================================================
        If cbox4 Is Nothing Then
            _search4 = Nothing
        Else
            Select Case cbox4.SelectedIndex
                Case 0
                    _search4 = If(search4.Text = "", Nothing, " e.Surname LIKE '" & search4.Text & "%'")
                Case 1
                    _search4 = If(search4.Text = "", Nothing, " e.Surname LIKE '%" & search4.Text & "%'")
                Case 2
                    _search4 = If(search4.Text = "", Nothing, " e.Surname = '" & search4.Text & "'")
                Case 3
                    _search4 = If(search4.Text = "", Nothing, " e.Surname NOT LIKE '%" & search4.Text & "%'")
                Case 4
                    _search4 = " e.Surname IS NULL"
                Case 5
                    _search4 = " e.Surname IS NOT NULL"
                Case Else
                    '_search4 = ""
                    _search4 = If(search4.Text = "", Nothing, " e.Surname = '" & search4.Text & "'")
            End Select

            If (_search1 <> "" Or _search2 <> "" Or _search3 <> "") And _search4 <> "" Then
                _search4 = " AND" & _search4
            End If
        End If

        '===============================================================
        If cbox5 Is Nothing Then
            _search5 = Nothing
        Else
            Select Case cbox5.SelectedIndex
                Case 0
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName LIKE '" & search5.Text & "%'")
                Case 1
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName LIKE '%" & search5.Text & "%'")
                Case 2
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName = '" & search5.Text & "'")
                Case 3
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName NOT LIKE '%" & search5.Text & "%'")
                Case 4
                    _search5 = " e.MiddleName IS NULL"
                Case 5
                    _search5 = " e.MiddleName IS NOT NULL"
                Case Else
                    '_search5 = ""
                    _search5 = If(search5.Text = "", Nothing, " e.MiddleName = '" & search5.Text & "'")
            End Select

            If (_search1 <> "" Or _search2 <> "" Or _search3 <> "" Or _search4 <> "") And _search5 <> "" Then
                _search5 = " AND" & _search5
            End If
        End If

        Return _search1 & _search2 & _search3 & _search4 & _search5

    End Function

    Dim nameofcol As String = Nothing

    Sub searchEmpSimple() ' As String
        Static s As SByte
        'MsgBox(search_selIndx)
        'If search_selIndx = 0 Then
        '    Exit Sub
        'End If
        s = 0
        Select Case search_selIndx
            Case 0 : nameofcol = "e.EmployeeID='"
            Case 1 : nameofcol = "e.FirstName='"
            Case 2 : nameofcol = "e.MiddleName='"
            Case 3 : nameofcol = "e.LastName='"
            Case 4 : nameofcol = "e.Surname='"
            Case 5 : nameofcol = "e.Nickname='"
            Case 6 : nameofcol = "e.MaritalStatus='"
            Case 7 : nameofcol = "e.EmployeeID='" 'NoOfDependents
            Case 8 : nameofcol = "e.Birthdate='"
                s = 1
                'loademployees(q_employee & " AND " & colName & Format(CDate(txtSimple.Text), "yyyy-MM-dd") & "' ORDER BY e.RowID DESC")
                dgvRowAdder(q_employee & " AND " & nameofcol & Format(CDate(txtSimple.Text), "yyyy-MM-dd") & "' ORDER BY e.RowID DESC", dgvEmployi)

            Case 9 : nameofcol = "e.Startdate='"
                s = 1
                'loademployees(q_employee & " AND " & colName & Format(CDate(txtSimple.Text), "yyyy-MM-dd") & "' ORDER BY e.RowID DESC")
                dgvRowAdder(q_employee & " AND " & nameofcol & Format(CDate(txtSimple.Text), "yyyy-MM-dd") & "' ORDER BY e.RowID DESC", dgvEmployi)

            Case 10 : nameofcol = "e.JobTitle='"
            Case 11 : nameofcol = "pos.PositionName='" 'e.PositionID
            Case 12 : nameofcol = "e.Salutation='"
            Case 13 : nameofcol = "e.TINNo='"
            Case 14 : nameofcol = "e.SSSNo='"
            Case 15 : nameofcol = "e.HDMFNo='"
            Case 16 : nameofcol = "e.PhilHealthNo='"
            Case 17 : nameofcol = "e.WorkPhone='"
            Case 18 : nameofcol = "e.HomePhone='"
            Case 19 : nameofcol = "e.MobilePhone='" '19
            Case 20 : nameofcol = "e.HomeAddress='"
            Case 21 : nameofcol = "e.EmailAddress='"
            Case 22 : nameofcol = "e.Gender=LEFT('"
                s = 1
                'loademployees(q_employee & " AND " & colName & txtSimple.Text & "',1) ORDER BY e.RowID DESC")
                dgvRowAdder(q_employee & " AND " & nameofcol & txtSimple.Text & "',1) ORDER BY e.RowID DESC", dgvEmployi)

            Case 23 : nameofcol = "e.EmploymentStatus='"
            Case 24 : nameofcol = "pf.PayFrequencyType='"

            Case 25 : nameofcol = "DATE_FORMAT(e.Created,'%m-%d-%Y')='"
            Case 26 : nameofcol = "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))='" 'e.CreatedBy
            Case 27 : nameofcol = "DATE_FORMAT(e.LastUpd,'%m-%d-%Y')='"
            Case 28 : nameofcol = "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))='" 'e.CreatedBy

            Case 29 : nameofcol = "e.EmployeeType='"

            Case 30 : nameofcol = "e.EmployeeID='"

        End Select

        If s = 0 Then
            'loademployees(q_employee & " AND " & colName & txtSimple.Text & "' ORDER BY e.RowID DESC")
            dgvRowAdder(q_employee & " AND " & nameofcol & txtSimple.Text & "' ORDER BY e.RowID DESC", dgvEmployi)
        End If

    End Sub

    Dim new_conn As New MySqlConnection
    Dim new_cmd As New MySqlCommand

    Function computehrswork_employeetimeentry(Optional etent_EmployeeID As Object = Nothing, _
                                              Optional etent_Date As Object = Nothing, _
                                              Optional employee_startdate As Object = Nothing, _
                                              Optional deleteexistingfirst As Object = 0) As Object

        Dim n_ExecuteQuery As _
            New ExecuteQuery("SELECT GENERATE_employeetimeentry('" & etent_EmployeeID & "'" &
                             ",'" & orgztnID & "'" &
                             ",'" & etent_Date & "'" &
                             ",'" & z_User & "');")

        Return ValNoComma(n_ExecuteQuery.Result)

        '############################OLD CODE###############################

        'Try

        '    If deleteexistingfirst = 1 Then

        '        'EXECQUER("DELETE" & _
        '        '         " FROM employeetimeentry" & _
        '        '         " WHERE OrganizationID='" & orgztnID & "'" & _
        '        '         " AND EmployeeID='" & etent_EmployeeID & "'" & _
        '        '         " AND `Date`='" & etent_Date & "';" & _
        '        '         "ALTER TABLE employeetimeentry AUTO_INCREMENT = 0;")

        '    End If

        '    If new_conn.State = ConnectionState.Open Then : new_conn.Close() : End If

        '    new_cmd = New MySqlCommand("COMPUTE_employeetimeentry", new_conn)

        '    new_conn.Open()

        '    'Try

        '    '    new_conn.Open()

        '    'Catch ex As Exception

        '    '    'If conn.ConnectionString.Contains("password") = False Then
        '    '    '    conn.ConnectionString = db_connectinstring
        '    '    'End If

        '    '    new_conn.ConnectionString = db_connectinstring

        '    'Finally

        '    '    If new_conn.State = ConnectionState.Open Then : new_conn.Close() : End If

        '    '    new_conn.Open()

        '    '    'computehrswork_employeetimeentry(etent_EmployeeID, _
        '    '    '                                 etent_Date, _
        '    '    '                                 employee_startdate)
        '    'End Try

        '    With new_cmd
        '        .Parameters.Clear()

        '        .CommandType = CommandType.StoredProcedure

        '        .Parameters.Add("etentID", MySqlDbType.Int32)

        '        .Parameters.AddWithValue("etent_EmployeeID", etent_EmployeeID)

        '        .Parameters.AddWithValue("etent_OrganizationID", orgztnID)

        '        .Parameters.AddWithValue("etent_Date", etent_Date)

        '        .Parameters.AddWithValue("etent_CreatedBy", z_User)

        '        .Parameters.AddWithValue("etent_LastUpdBy", z_User)

        '        .Parameters.AddWithValue("EmployeeStartDate", Format(CDate(employee_startdate), "yyyy-MM-dd"))

        '        .Parameters("etentID").Direction = ParameterDirection.ReturnValue

        '        Dim datread As MySqlDataReader

        '        datread = .ExecuteReader()

        '        computehrswork_employeetimeentry = datread(0)

        '    End With
        'Catch ex As Exception
        '    MsgBox(ex.Message & " " & "COMPUTE_employeetimeentry", , "Error")
        'Finally
        '    new_conn.Close()
        '    new_cmd.Dispose()
        'End Try

    End Function

    'Function computehrswork_employeetimeentry(Optional etent_EmployeeID As Object = Nothing, _
    '                                          Optional etent_Date As Object = Nothing) As Object

    '    Static once As SByte = 0
    '    If once = 0 Then
    '        once = 1
    '    End If

    '    Try
    '        If new_conn.State = ConnectionState.Open Then : new_conn.Close() : End If

    '        new_cmd = New MySqlCommand("COMPUTE_employeetimeentry", new_conn)

    '        new_conn.Open()

    '        With new_cmd
    '            .Parameters.Clear()

    '            .CommandType = CommandType.StoredProcedure

    '            .Parameters.Add("etentID", MySqlDbType.Int32)

    '            .Parameters.AddWithValue("etent_EmployeeID", etent_EmployeeID)

    '            .Parameters.AddWithValue("etent_OrganizationID", orgztnID)

    '            .Parameters.AddWithValue("etent_Date", etent_Date)

    '            .Parameters.AddWithValue("etent_CreatedBy", z_User)

    '            .Parameters.AddWithValue("etent_LastUpdBy", z_User)

    '            .Parameters("etentID").Direction = ParameterDirection.ReturnValue

    '            Dim datread As MySqlDataReader

    '            datread = .ExecuteReader()

    '            computehrswork_employeetimeentry = datread(0)

    '        End With
    '    Catch ex As Exception
    '        MsgBox(ex.Message & " " & "COMPUTE_employeetimeentry", , "Error")
    '    Finally
    '        new_conn.Close()
    '        new_cmd.Dispose()
    '    End Try

    'End Function

    Dim daterecalc = Nothing

    Private Sub tsbtnrecalc_Click(sender As Object, e As EventArgs) Handles tsbtnrecalc.Click

        If curr_dd = Nothing Then

            WarnBalloon("Please select a day.", "No day is selected", lblforballoon, 108, -69)

        Else
            If dgvEmployi.RowCount <> 0 Then
                'MsgBox(tmpdate & "-" & curr_mm & "-" & curr_dd)

                Dim daterecalc = Trim(tmpdate & "-" & curr_mm & "-" & curr_dd)

                Dim the_date = Format(CDate(daterecalc), "MMM/dd/yyyy")

                Dim prompt = MessageBox.Show(the_date & " is the date to be recalculated for Employee # " & dgvEmployi.CurrentRow.Cells("cemp_EmployeeID").Value & vbNewLine & _
                                            "Proceed recalculating ?", _
                                            "Recalculate confirmation", _
                                            MessageBoxButtons.YesNo, _
                                            MessageBoxIcon.Question)

                'Dim prompt = MsgBox(the_date & " is the date to be recalculated for Employee # " & dgvEmployi.CurrentRow.Cells("cemp_EmployeeID").Value & vbNewLine & _
                '                    "Proceed recalculating ?", _
                '                    MsgBoxStyle.YesNo, "Recalculate confirmation")

                If prompt = MsgBoxResult.Yes Then

                    If daterecalc = Nothing Then

                    Else

                        RemoveHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged

                        RemoveHandler dgvcalendar.CurrentCellChanged, AddressOf dgvcalendar_CurrentCellChanged

                        computehrswork_employeetimeentry(dgvEmployi.CurrentRow.Cells("cemp_RowID").Value, _
                                                         daterecalc, _
                                                         dgvEmployi.CurrentRow.Cells("cemp_Startdate").Value)

                        'MsgBox("Recalcuation of day pay was done successfully.", _
                        '       MsgBoxStyle.Information, _
                        '       "Successful Recalculate day pay")

                        InfoBalloon("Recalcuation of day pay was done successfully.", _
                                    "Successful Recalculate day pay", _
                                    lblforballoon, 108, -69)

                        AddHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged

                        AddHandler dgvcalendar.CurrentCellChanged, AddressOf dgvcalendar_CurrentCellChanged

                    End If

                End If

            End If

        End If

    End Sub

    Private Sub txtSimple_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSimple.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnRerfresh_Click(sender, e)

        End If

    End Sub

    Private Sub EnterKeyPressSearch(sender As Object, e As KeyPressEventArgs) Handles ComboBox7.KeyPress, TextBox2.KeyPress, _
                                                                                      ComboBox8.KeyPress, TextBox15.KeyPress, _
                                                                                      ComboBox9.KeyPress, TextBox16.KeyPress, _
                                                                                      ComboBox10.KeyPress, TextBox17.KeyPress, _
                                                                                      txtSimple.KeyPress, ComboBox1.KeyPress

        RemoveHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged

        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 13 Then
            pagination = 0
            conn.Close()

            btnRerfresh_Click(sender, e)

        Else

            AddHandler dgvEmployi.SelectionChanged, AddressOf dgvEmployi_SelectionChanged

        End If

    End Sub

    Dim search_selIndx As Integer

    Dim empcolcount As Integer

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged, ComboBox1.SelectedValueChanged

        Dim _selIndx As Integer = ComboBox1.SelectedIndex

        'empcolcount = 29

        If _selIndx <= empcolcount Then
            search_selIndx = _selIndx
        Else
            Dim _num = _selIndx
            Do While _num > empcolcount
                _num -= empcolcount
            Loop
            search_selIndx = _num
        End If

        'Dim _selIndx As Integer = ComboBox1.SelectedIndex
        'Dim empcolcount As Integer = 32 'dgvEmp.Columns.Count '  - 1

        'If _selIndx <= empcolcount Then
        '    search_selIndx = _selIndx
        'Else
        '    Dim _num = _selIndx
        '    Do While _num > empcolcount
        '        _num = _num - empcolcount
        '    Loop
        '    search_selIndx = _num
        'End If

    End Sub

    Private Sub txtSimple_TextChanged(sender As Object, e As EventArgs) Handles txtSimple.TextChanged
        ComboBox1.Text = txtSimple.Text

    End Sub

    Private Sub txttotdaypay_KeyDown(sender As Object, e As KeyEventArgs) Handles txttotdaypay.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            txttotdaypay.Copy()

        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            txttotdaypay.Paste()

        End If

    End Sub

    Private Sub txttotdaypay_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txttotdaypay.KeyPress
        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txttotdaypay.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txttotdaypay.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txttotdaypay_Leave(sender As Object, e As EventArgs) Handles txttotdaypay.Leave
        Try
            If prevdgvrowetentsemimon IsNot Nothing Then
                'prevdgvrowetentsemimon.Index <> -1 Then

                If Trim(prevdgvrowetentsemimon.Cells("etiment_TotDayPay").Value.ToString) <> Trim(txttotdaypay.Text.Replace(",", "")) Then
                    listOfEditetimeent.Add(prevdgvrowetentsemimon.Cells("etiment_RowID").Value)
                    dgvetentsemimon.Item("etiment_TotDayPay", prevdgvrowetentsemimon.Index).Value = Trim(txttotdaypay.Text.Replace(",", ""))
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("etiment_TotDayPay") = False Then
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End If

        End Try
    End Sub

    Private Sub txttotdaypay_TextChanged(sender As Object, e As EventArgs) Handles txttotdaypay.TextChanged

    End Sub









    Private Sub lblpublish_Click(sender As Object, e As EventArgs) Handles lblpublish.Click  'Publish to Payroll

        If lblpublish.Text = "Published to Payroll :" Then

        Else

            MDIPrimaryForm.ToolStripButton5_Click(sender, e)

            PayrollForm.PayrollToolStripMenuItem_Click(sender, e)

        End If

    End Sub

    Private Sub lblperfectatt_Click(sender As Object, e As EventArgs) Handles lblperfectatt.Click  'Perfect attendance

        If lblperfectatt.Text = "Perfect attentdance :" Then

        Else

            'EmpTimeDetail

            MDIPrimaryForm.ToolStripButton3_Click(sender, e)

            TimeAttendForm.TimeEntryLogsToolStripMenuItem_Click(sender, e)

        End If

    End Sub

    Private Sub lblmissin_Click(sender As Object, e As EventArgs) Handles lblmissin.Click  'Missing clock in

        If lblmissin.Text = "Missing clock in :" Then

        Else

            'EmpTimeDetail

            MDIPrimaryForm.ToolStripButton3_Click(sender, e)

            TimeAttendForm.TimeEntryLogsToolStripMenuItem_Click(sender, e)

        End If

    End Sub

    Private Sub lblmissout_Click(sender As Object, e As EventArgs) Handles lblmissout.Click  'Missing clock out

        If lblmissout.Text = "Missing clock out :" Then

        Else

            'EmpTimeDetail

            MDIPrimaryForm.ToolStripButton3_Click(sender, e)

            TimeAttendForm.TimeEntryLogsToolStripMenuItem_Click(sender, e)

        End If

    End Sub

    Private Sub lbldupin_Click(sender As Object, e As EventArgs) Handles lbldupin.Click  'Duplicate clock in

        If lbldupin.Text = "Duplicate clock in :" Then

        Else

            'EmpTimeDetail

            MDIPrimaryForm.ToolStripButton3_Click(sender, e)

            TimeAttendForm.TimeEntryLogsToolStripMenuItem_Click(sender, e)

        End If

    End Sub

    Private Sub lbldupout_Click(sender As Object, e As EventArgs) Handles lbldupout.Click  'Duplicate clock out

        If lbldupout.Text = "Duplicate clock out :" Then

        Else

            'EmpTimeDetail

            MDIPrimaryForm.ToolStripButton3_Click(sender, e)

            TimeAttendForm.TimeEntryLogsToolStripMenuItem_Click(sender, e)

        End If

    End Sub

    Private Sub lbllate_Click(sender As Object, e As EventArgs) Handles lbllate.Click  'In late

        If lbllate.Text = "In late :" Then

        Else

            'EmpTimeDetail

            MDIPrimaryForm.ToolStripButton3_Click(sender, e)

            TimeAttendForm.TimeEntryLogsToolStripMenuItem_Click(sender, e)

        End If

    End Sub

    Private Sub lblundertime_Click(sender As Object, e As EventArgs) Handles lblundertime.Click  'Underime or out early

        If lblundertime.Text = "Out early :" Then

        Else

            'EmpTimeDetail

            MDIPrimaryForm.ToolStripButton3_Click(sender, e)

            TimeAttendForm.TimeEntryLogsToolStripMenuItem_Click(sender, e)

        End If

    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

        Dim numberone = 1

        If numberone = 1 Then

            Exit Sub
        End If

        Dim startyear = 2020

        For ii = 0 To 10

            For iii = 0 To 11
                Dim lastdayofmonth = EXECQUER("SELECT ADDDATE(LAST_DAY('" & startyear & "-01-01'), INTERVAL " & iii & " MONTH);")

                lastdayofmonth = Format(CDate(lastdayofmonth), "yyyy-MM-dd")

                Dim payp_fromdate As String = Nothing
                Dim payp_todate As String = Nothing

                '2020-01-01
                'MsgBox(("2020-01-02").ToString.Substring(0, 8))

                payp_fromdate = lastdayofmonth.ToString.Substring(0, 8) & "01"
                payp_todate = lastdayofmonth.ToString.Substring(0, 8) & "15"

                INSUPD_payperiod(, payp_fromdate, _
                                 payp_todate)

                payp_fromdate = lastdayofmonth.ToString.Substring(0, 8) & "16"
                payp_todate = lastdayofmonth

                INSUPD_payperiod(, payp_fromdate, _
                                 payp_todate)

            Next

            startyear += 1

        Next

        'Dim firstdate As Object = EXECQUER("SELECT DATE_FORMAT(MAKEDATE(YEAR('2020-01-01'),1),'%Y-%m-%d');")

        'For i = 0 To 11
        '    Dim newdattab As New DataTable
        '    newdattab = retAsDatTbl("SELECT YEAR(ADDDATE('" & firstdate & "', INTERVAL " & i & " MONTH)) 'next_month_Y'" & _
        '                            ", MONTH(ADDDATE('" & firstdate & "', INTERVAL " & i & " MONTH)) 'next_month_MM'" & _
        '                            ", DAY(LAST_DAY(ADDDATE('" & firstdate & "', INTERVAL " & i & " MONTH))) 'this_month_lastday';")

        '    For Each drow As DataRow In newdattab.Rows
        '        Dim payp_fromdate As String
        '        Dim payp_todate As String

        '        payp_fromdate = drow("next_month_Y") & "-" & drow("next_month_MM") & "-1"
        '        payp_todate = drow("next_month_Y") & "-" & drow("next_month_MM") & "-15"

        '        INSUPD_payperiod(, payp_fromdate, _
        '                         payp_todate)

        '        payp_fromdate = drow("next_month_Y") & "-" & drow("next_month_MM") & "-16"
        '        payp_todate = drow("next_month_Y") & "-" & drow("next_month_MM") & "-" & drow("this_month_lastday")

        '        INSUPD_payperiod(, payp_fromdate, _
        '                         payp_todate)
        '    Next

        'Next

    End Sub

    Private Sub tsbtnCancelempawar_Click(sender As Object, e As EventArgs) Handles tsbtnCancelempawar.Click
        If bgwpayrate.IsBusy Then

        Else
            dgvEmployi_SelectionChanged(sender, e)

        End If

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TabControl1_Selected(sender As Object, e As TabControlCancelEventArgs) Handles TabControl1.Selecting

        Dim bool_result As Boolean = (e.TabPageIndex = 1)

        'Console.WriteLine(bool_result)

        e.Cancel = bool_result

    End Sub

End Class