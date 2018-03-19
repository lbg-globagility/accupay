Imports MySql.Data.MySqlClient

Public Class BlankTimeEntryLogs

    Dim n_pay_DateFrom = Nothing

    Dim n_pay_DateTo = Nothing

    Sub New(pay_DateFrom As Object,
            pay_DateTo As Object)

        n_pay_DateFrom = pay_DateFrom

        n_pay_DateTo = pay_DateTo

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

    End Sub

    Dim isShowAsDialog As Boolean = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            isShowAsDialog = True

            .Text = someValue

        End With

        Return Me.ShowDialog

    End Function

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("SELECT etd.RowID AS etdRowID" &
                                    ",e.RowID AS eRowID" &
                                    ",e.EmployeeID" &
                                    ",CONCAT(e.LastName,',',e.FirstName, IF(e.MiddleName='','',','),INITIALS(e.MiddleName,'. ','1')) 'Fullname'" &
                                    ",IF(esh.RowID IS NULL,'', CONCAT(TIME_FORMAT(sh.TimeFrom,'%l:%i %p'),' to ',TIME_FORMAT(sh.TimeTo,'%l:%i %p'))) AS EmpShift" &
                                    ",IFNULL(TIME_FORMAT(etd.TimeIn,'%l:%i %p'),'') AS TimeIn" &
                                    ",IFNULL(TIME_FORMAT(etd.TimeOut,'%l:%i %p'),'') AS TimeOut" &
                                    ",DATE_FORMAT(etd.`Date`,'%c/%e/%Y') AS Date" &
                                    ",IFNULL(etd.TimeScheduleType,'') AS TimeScheduleType" &
                                    ",IFNULL(etd.TimeEntryStatus,'') AS TimeEntryStatus" &
                                    ",DATE_FORMAT(etd.Created,@@datetime_format) AS Created" &
                                    " FROM employeetimeentrydetails etd" &
                                    " INNER JOIN employee e ON e.RowID=etd.EmployeeID AND e.OrganizationID=etd.OrganizationID" &
                                    " LEFT JOIN employeeshift esh ON esh.EmployeeID=e.RowID AND esh.OrganizationID=etd.OrganizationID AND etd.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo" &
                                    " LEFT JOIN shift sh ON sh.RowID=esh.ShiftID" &
                                    " WHERE etd.OrganizationID='" & orgztnID & "'" &
                                    " AND etd.EmployeeID IS NOT NULL" &
                                    " AND etd.`Date` BETWEEN '" & n_pay_DateFrom & "' AND '" & n_pay_DateTo & "'" &
                                    " AND (etd.TimeIn IS NULL OR etd.TimeOut IS NULL)" &
                                    " ORDER BY etd.`Date`;")

        '" LEFT JOIN (SELECT *" &
        '            " FROM employeeshift" &
        '            " WHERE OrganizationID='" & orgztnID & "'" &
        '            " AND (EffectiveFrom >= '" & n_pay_DateFrom & "' OR EffectiveTo >= '" & n_pay_DateFrom & "')" &
        '            " AND (EffectiveFrom <= '" & n_pay_DateTo & "' OR EffectiveTo <= '" & n_pay_DateTo & "')" &
        '            " ORDER BY EffectiveFrom DESC,EffectiveTo DESC" &
        '            " LIMIT 1) esh ON esh.EmployeeID=e.RowID" &

        Dim newdt As New DataTable

        newdt = n_SQLQueryToDatatable.ResultTable

        dgvetentdet.Rows.Clear()

        For Each drow As DataRow In newdt.Rows

            Dim rowArray = drow.ItemArray

            With dgvetentdet

                Dim rowindx = _
                    .Rows.Add(rowArray)

                If drow("TimeIn") = Nothing Then
                    .Item("Column3", rowindx).Style.BackColor = Color.FromArgb(255, 94, 0)
                    .Item("Column3", rowindx).Style.ForeColor = Color.White
                    .Item("Column3", rowindx).Style.SelectionBackColor = Color.FromArgb(255, 160, 0)
                End If

                If drow("TimeOut") = Nothing Then
                    .Item("Column4", rowindx).Style.BackColor = Color.FromArgb(255, 94, 0)
                    .Item("Column4", rowindx).Style.ForeColor = Color.White
                    .Item("Column4", rowindx).Style.SelectionBackColor = Color.FromArgb(255, 160, 0)
                End If

            End With

        Next

        'dgvetentdet

        MyBase.OnLoad(e)

    End Sub

    Private Sub dgvetentdet_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvetentdet.CellContentClick

    End Sub

    Dim haserrinput As SByte

    Dim listofEditRow As New AutoCompleteStringCollection

    Dim reset_static As SByte = -1

    Private Sub dgvetentdet_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvetentdet.CellEndEdit

        dgvetentdet.ShowCellErrors = True

        Dim colName As String = dgvetentdet.Columns(e.ColumnIndex).Name
        Dim rowindx = e.RowIndex

        Static num As Integer = If(reset_static = -1, _
                                   -1, _
                                   num)

        If dgvetentdet.RowCount <> 0 Then
            With dgvetentdet

                If Val(dgvetentdet.Item("Column1", e.RowIndex).Value) <> 0 Then
                    'If num <> Val(dgvetentdet.Item("Column1", e.RowIndex).Value) Then
                    '    num = Val(dgvetentdet.Item("Column1", e.RowIndex).Value)
                    listofEditRow.Add(dgvetentdet.Item("Column1", e.RowIndex).Value)
                    'End If
                Else

                End If

                'Column3'Column4'Column5

                If (colName = "Column5") Then

                    If Trim(dgvetentdet.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvetentdet.Item(colName, rowindx).Value)
                        Try
                            dgvetentdet.Item(colName, rowindx).Value = Format(CDate(dateobj), "M/dd/yyyy")

                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            haserrinput = 1
                            dgvetentdet.Item(colName, rowindx).ErrorText = "     Invalid date value"
                        End Try
                    Else
                        haserrinput = 0

                        dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                    End If

                ElseIf (colName = "Column3" Or colName = "Column4") Then

                    If Trim(dgvetentdet.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvetentdet.Item(colName, rowindx).Value).Replace(" ", ":")

                        Dim ampm As String = Nothing

                        Try
                            If dateobj.ToString.Contains("A") Or _
                        dateobj.ToString.Contains("P") Or _
                        dateobj.ToString.Contains("M") Then

                                ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                                ampm = Microsoft.VisualBasic.Left(ampm, 2) & "M"
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                                dateobj = dateobj.ToString.Replace(" ", ":")

                            End If
                            '    dateobj = getStrBetween(dateobj.ToString, "", " ")
                            '    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm")
                            '    dgvempleave.Item(colName, rowIndx).Value = valtime.ToShortTimeString
                            'Else
                            Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")
                            If ampm = Nothing Then
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToShortTimeString
                            Else
                                dgvetentdet.Item(colName, rowindx).Value = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                            End If
                            'End If
                            'valtime = DateTime.Parse(e.FormattedValue)
                            'valtime = valtime.ToShortTimeString
                            'Format(valtime, "hh:mm tt")
                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            Try
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, 5))
                                dateobj = dateobj.ToString.Replace(" ", ":")

                                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")
                                'valtime = DateTime.Parse(e.FormattedValue)
                                'valtime = valtime.ToShortTimeString
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToShortTimeString
                                'Format(valtime, "hh:mm tt")
                                haserrinput = 0

                                dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                            Catch ex_1 As Exception
                                haserrinput = 1
                                dgvetentdet.Item(colName, rowindx).ErrorText = "     Invalid time value"
                            End Try
                        End Try
                    Else
                        haserrinput = 0

                        dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                    End If
                    'Else 'Column6
                    '    haserrinput = 0
                    '    dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                End If

            End With
        End If

    End Sub

    Private Sub dgvetentdet_CellEndEdit1(sender As Object, e As DataGridViewCellEventArgs) 'Handles dgvetentdet.CellEndEdit

        dgvetentdet.ShowCellErrors = True

        Dim colName As String = dgvetentdet.Columns(e.ColumnIndex).Name
        Dim rowindx = e.RowIndex

        Static num As Integer = If(reset_static = -1, _
                                   -1, _
                                   num)

        If dgvetentdet.RowCount <> 0 Then
            With dgvetentdet

                If Val(dgvetentdet.Item("Column1", e.RowIndex).Value) <> 0 Then
                    'If num <> Val(dgvetentdet.Item("Column1", e.RowIndex).Value) Then
                    '    num = Val(dgvetentdet.Item("Column1", e.RowIndex).Value)
                    listofEditRow.Add(dgvetentdet.Item("Column1", e.RowIndex).Value)
                    'End If
                Else

                End If

                'Column3'Column4'Column5

                If (colName = "Column5") Then

                    If Trim(dgvetentdet.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvetentdet.Item(colName, rowindx).Value)
                        Try
                            dgvetentdet.Item(colName, rowindx).Value = Format(CDate(dateobj), "M/dd/yyyy")

                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        Catch ex As Exception
                            haserrinput = 1
                            dgvetentdet.Item(colName, rowindx).ErrorText = "     Invalid date value"
                        End Try
                    Else
                        haserrinput = 0

                        dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                    End If

                ElseIf (colName = "Column3" Or colName = "Column4") Then

                    If Trim(dgvetentdet.Item(colName, rowindx).Value) <> "" Then
                        Dim dateobj As Object = Trim(dgvetentdet.Item(colName, rowindx).Value).Replace(" ", ":")

                        Dim dateobj_len = dateobj.ToString.Length

                        Dim ampm As String = Nothing

                        Try
                            If dateobj.ToString.Contains("A") Or _
                                dateobj.ToString.Contains("P") Or _
                                dateobj.ToString.Contains("M") Then

                                ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))

                                dateobj_len -= ampm.Length

                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, dateobj_len)) 'dateobj.ToString.Substring(0, 4)
                                dateobj = dateobj.ToString.Replace(" ", ":")

                            End If
                            '    dateobj = getStrBetween(dateobj.ToString, "", " ")
                            '    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm")
                            '    dgvempleave.Item(colName, rowIndx).Value = valtime.ToLongTimeString
                            'Else

                            Dim modified_format = If(dateobj_len = 5, "h:m", "hh:mm:ss tt")

                            Dim valtime As DateTime = DateTime.Parse(dateobj).ToString(modified_format)
                            If ampm = Nothing Then
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToLongTimeString
                            Else
                                dgvetentdet.Item(colName, rowindx).Value = Trim(valtime.ToLongTimeString.Substring(0, (dateobj_len - 1))) & ampm
                            End If
                            'End If
                            'valtime = DateTime.Parse(e.FormattedValue)
                            'valtime = valtime.ToLongTimeString
                            'Format(valtime, "hh:mm tt")
                            haserrinput = 0

                            dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                            dgvetentdet.Item(colName, rowindx).Style.BackColor = Color.White
                            dgvetentdet.Item(colName, rowindx).Style.ForeColor = Color.Black
                            dgvetentdet.Item(colName, rowindx).Style.SelectionBackColor = Color.FromArgb(51, 153, 255)

                        Catch ex As Exception
                            Try
                                dateobj = dateobj.ToString.Replace(":", " ")
                                dateobj = Trim(dateobj.ToString.Substring(0, dateobj_len))
                                dateobj = dateobj.ToString.Replace(" ", ":")

                                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm:ss")
                                'valtime = DateTime.Parse(e.FormattedValue)
                                'valtime = valtime.ToLongTimeString
                                dgvetentdet.Item(colName, rowindx).Value = valtime.ToLongTimeString
                                'Format(valtime, "hh:mm tt")
                                haserrinput = 0

                                dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                                dgvetentdet.Item(colName, rowindx).Style.BackColor = Color.White
                                dgvetentdet.Item(colName, rowindx).Style.ForeColor = Color.Black
                                dgvetentdet.Item(colName, rowindx).Style.SelectionBackColor = Color.FromArgb(51, 153, 255)

                            Catch ex_1 As Exception
                                haserrinput = 1
                                dgvetentdet.Item(colName, rowindx).ErrorText = "     Invalid time value"
                            End Try
                        End Try
                    Else
                        haserrinput = 0

                        dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                        dgvetentdet.Item(colName, rowindx).Style.BackColor = Color.FromArgb(255, 94, 0)
                        dgvetentdet.Item(colName, rowindx).Style.ForeColor = Color.White
                        dgvetentdet.Item(colName, rowindx).Style.SelectionBackColor = Color.FromArgb(255, 160, 0) '255, 160, 0

                    End If
                    'Else 'Column6
                    '    haserrinput = 0
                    '    dgvetentdet.Item(colName, rowindx).ErrorText = Nothing
                End If

            End With

        End If

        'dgvetentdet.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells
        'dgvetentdet.AutoResizeRow(e.RowIndex)
        'dgvetentdet.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None

    End Sub

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click

        Dim r_indx, c_indx

        If dgvetentdet.RowCount > 1 Then

            r_indx = dgvetentdet.CurrentRow.Index
            c_indx = dgvetentdet.CurrentCell.ColumnIndex

            OnLoad(New EventArgs)

            If (dgvetentdet.RowCount - 2) >= r_indx Then
                dgvetentdet.Item(c_indx, r_indx).Selected = True
            End If
        Else
            OnLoad(New EventArgs)
        End If

    End Sub

    Dim newRowID = Nothing

    Dim dontUpdate As SByte = 0

    Private Sub tsbtnSave_Click(sender As Object, e As EventArgs) Handles tsbtnSave.Click

        dgvetentdet.EndEdit(True)

        If dontUpdate = 1 Then
            listofEditRow.Clear()
        End If

        If haserrinput = 1 Then
            'WarnBalloon("Please input a valid date or time.", "Invalid Date or Time", lblforballoon, 0, -69)
            Exit Sub

        End If

        ', _
        '                                    currtimestamp
        For Each dgrow As DataGridViewRow In dgvetentdet.Rows
            With dgrow
                If .IsNewRow = False Then
                    Dim RowID = Nothing

                    Dim time_i = If(.Cells("Column3").Value = Nothing, _
                                    Nothing, _
                                    Format(CDate(Trim(.Cells("Column3").Value)), "HH:mm:ss"))

                    Dim time_o = If(.Cells("Column4").Value = Nothing, _
                                    Nothing, _
                                    Format(CDate(Trim(.Cells("Column4").Value)), "HH:mm:ss"))

                    If listofEditRow.Contains(.Cells("Column1").Value) Then
                        Dim etent_date = Format(CDate(.Cells("Column5").Value), "yyyy-MM-dd")

                        RowID = .Cells("Column1").Value
                        INSUPD_employeetimeentrydetails(RowID, _
                                                        .Cells("Column2").Value, _
                                                        time_i, _
                                                        time_o, _
                                                        Trim(etent_date), _
                                                        .Cells("Column6").Value)
                    Else
                        If .Cells("Column1").Value = Nothing Then
                            newRowID = _
                            INSUPD_employeetimeentrydetails(, _
                                                            .Cells("Column2").Value, _
                                                            time_i, _
                                                            time_o, _
                                                            Format(CDate(.Cells("Column5").Value), "yyyy-MM-dd"), _
                                                            .Cells("Column6").Value, _
                                                            .Cells("DataCreated").Value)

                            .Cells("Column1").Value = newRowID
                        End If

                    End If

                    'Dim afsfa = CDate("").DayOfWeek

                    'If .Cells("Column1").Value = Nothing Then
                    '    .Cells("Column1").Value = newRowID
                    'End If

                End If
            End With

        Next

        listofEditRow.Clear()

        reset_static = -1

        'InfoBalloon("Successfully saved.", _
        '          "Successfully saved.", lblforballoon, 0, -69)

        tsbtnCancel_Click(sender, e)

    End Sub

    Function INSUPD_employeetimeentrydetails(Optional etentd_RowID As Object = Nothing, _
                                             Optional etentd_EmployeeID As Object = Nothing, _
                                             Optional etentd_TimeIn As Object = Nothing, _
                                             Optional etentd_TimeOut As Object = Nothing, _
                                             Optional etentd_Date As Object = Nothing, _
                                             Optional etentd_TimeScheduleType As Object = Nothing, _
                                             Optional etentd_Created As Object = Nothing, _
                                             Optional etentd_TimeEntryStatus As Object = Nothing,
                                             Optional EditAsUnique As String = "0") As Object
        Dim params(9, 2) As Object

        'params(0, 0) = "etentd_RowID"
        'params(1, 0) = "etentd_OrganizationID"
        'params(2, 0) = "etentd_CreatedBy"
        'params(3, 0) = "etentd_LastUpdBy"
        'params(4, 0) = "etentd_EmployeeID"
        'params(5, 0) = "etentd_TimeIn"
        'params(6, 0) = "etentd_TimeOut"
        'params(7, 0) = "etentd_Date"
        'params(8, 0) = "etentd_TimeScheduleType"

        'params(0, 1) = If(etentd_RowID = Nothing, DBNull.Value, etentd_RowID)
        'params(1, 1) = orgztnID
        'params(2, 1) = 2 'CreatedBy
        'params(3, 1) = 2 'LastUpdBy
        'params(4, 1) = If(etentd_EmployeeID = Nothing, DBNull.Value, etentd_EmployeeID)
        'params(5, 1) = If(etentd_TimeIn = Nothing, DBNull.Value, etentd_TimeIn)
        'params(6, 1) = If(etentd_TimeOut = Nothing, DBNull.Value, etentd_TimeOut)
        'params(7, 1) = etentd_Date
        'params(8, 1) = If(etentd_TimeScheduleType = Nothing, DBNull.Value, etentd_TimeScheduleType)

        'INSUPD_employeetimeentrydetails = _
        '    EXEC_INSUPD_PROCEDURE(params, _
        '                          "INSUPD_employeetimeentrydetails", _
        '                          "etentdID")

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("INSUPD_employeetimeentrydetails", conn)
            conn.Open()
            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("etentdID", MySqlDbType.Int32)

                'Dim rowid = If(etentd_RowID = Nothing, DBNull.Value, etentd_RowID)

                'MsgBox(rowid.ToString)

                .Parameters.AddWithValue("etentd_RowID", If(etentd_RowID = Nothing, DBNull.Value, etentd_RowID))
                .Parameters.AddWithValue("etentd_OrganizationID", orgztnID)
                .Parameters.AddWithValue("etentd_CreatedBy", z_User)
                .Parameters.AddWithValue("etentd_Created", etentd_Created)
                .Parameters.AddWithValue("etentd_LastUpdBy", z_User)
                .Parameters.AddWithValue("etentd_EmployeeID", If(etentd_EmployeeID = Nothing, DBNull.Value, etentd_EmployeeID))

                If IsDBNull(etentd_TimeIn) Then
                    .Parameters.AddWithValue("etentd_TimeIn", etentd_TimeIn)
                Else
                    .Parameters.AddWithValue("etentd_TimeIn", If(etentd_TimeIn = Nothing, DBNull.Value, etentd_TimeIn))
                End If

                If IsDBNull(etentd_TimeOut) Then
                    .Parameters.AddWithValue("etentd_TimeOut", etentd_TimeOut)
                Else
                    .Parameters.AddWithValue("etentd_TimeOut", If(etentd_TimeOut = Nothing, DBNull.Value, etentd_TimeOut))
                End If

                .Parameters.AddWithValue("etentd_Date", etentd_Date)

                .Parameters.AddWithValue("etentd_TimeScheduleType", If(etentd_TimeScheduleType = Nothing, String.Empty, Trim(etentd_TimeScheduleType)))

                .Parameters.AddWithValue("etentd_TimeEntryStatus", If(etentd_TimeEntryStatus = Nothing, String.Empty, etentd_TimeEntryStatus))
                
                .Parameters.AddWithValue("EditAsUnique", EditAsUnique)
                '##############################################
                Dim branc_code = New ExecuteQuery("SELECT b.BranchCode FROM branch b LEFT JOIN employeetimeentrydetails etd ON etd.ChargeToDivisionID=b.RowID AND etd.RowID='" & ValNoComma(etentd_RowID) & "' AND etd.OrganizationID=b.OrganizationID WHERE b.OrganizationID='" & orgztnID & "' LIMIT 1;").Result
                .Parameters.AddWithValue("Branch_Code", If(branc_code = Nothing, DBNull.Value, branc_code))
                Dim datetimelog_I = New ExecuteQuery("SELECT TimeStampIn FROM employeetimeentrydetails WHERE RowID='" & ValNoComma(etentd_RowID) & "';").Result
                .Parameters.AddWithValue("DateTimeLogIn", If(datetimelog_I = Nothing, DBNull.Value, datetimelog_I))
                Dim datetimelog_O = New ExecuteQuery("SELECT TimeStampOut FROM employeetimeentrydetails WHERE RowID='" & ValNoComma(etentd_RowID) & "';").Result
                .Parameters.AddWithValue("DateTimeLogOut", If(datetimelog_O = Nothing, DBNull.Value, datetimelog_O))
                '##############################################

                .Parameters("etentdID").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader() '.ExecuteScalar()

                INSUPD_employeetimeentrydetails = datread(0) 'Return value'CType(.ExecuteScalar(), Integer)

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " " & "INSUPD_employeetimeentrydetails", , "Error")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return Nothing
    End Function

    Dim view_ID = Nothing

    Private Sub BlankTimeEntryLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        view_ID = VIEW_privilege("Employee Time Entry logs", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then
            
            tsbtnSave.Visible = 0

            dontUpdate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    tsbtnSave.Visible = 0
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

    End Sub

    Private Sub btnOkay_Click(sender As Object, e As EventArgs) Handles btnOkay.Click

        dgvetentdet.EndEdit(True)

        If isShowAsDialog Then

            Me.DialogResult = Windows.Forms.DialogResult.OK

        Else

            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        End If

    End Sub

    Private Sub dgvetentdet_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvetentdet.RowsAdded

        Dim timelogissue As String = String.Empty

        If dgvetentdet.Item("Column3", e.RowIndex).Value.ToString.Trim.Length = 0 Then
            timelogissue = "Blank Time In (I)"
        End If

        If dgvetentdet.Item("Column4", e.RowIndex).Value.ToString.Trim.Length = 0 Then
            If timelogissue.Length > 0 Then
                timelogissue &= " and blank Time Out (O)"
            Else
                timelogissue = "Blank Time Out (O)"
            End If
        End If

        dgvetentdet.Item("TimeLogCase", e.RowIndex).Value = timelogissue

    End Sub

End Class