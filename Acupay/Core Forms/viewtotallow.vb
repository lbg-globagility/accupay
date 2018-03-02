Public Class viewtotallow
    Dim categallowID As Object = Nothing

    Dim allowance_type As New AutoCompleteStringCollection
    Private Sub viewtotallow_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'categallowID = EXECQUER("SELECT RowID FROM category WHERE OrganizationID=" & orgztnID & " AND CategoryName='" & "Allowance Type" & "' LIMIT 1;")

        'If Val(categallowID) = 0 Then
        '    categallowID = INSUPD_category(, "Allowance Type")
        'End If

        'enlistTheLists("SELECT CONCAT(COALESCE(PartNo,''),'@',RowID) FROM product WHERE CategoryID='" & categallowID & "' AND OrganizationID=" & orgztnID & ";",
        '               allowance_type) 'cboallowtype

        'For Each strval In allowance_type
        '    'cboallowtype.Items.Add(getStrBetween(strval, "", "@"))
        '    eall_Type.Items.Add(getStrBetween(strval, "", "@"))
        'Next

    End Sub

    Sub VIEW_employeeallowance_indate(Optional eallow_EmployeeID As Object = Nothing,
                               Optional datefrom As Object = Nothing,
                               Optional dateto As Object = Nothing,
                               Optional num_weekdays As Object = Nothing,
                               Optional AllowanceExcept As String = Nothing)

        Static employee_rowid As Integer = eallow_EmployeeID

        Static from_date As Object = datefrom
        Static to_date As Object = dateto

        Dim params =
            New Object() {orgztnID,
            employee_rowid,
            from_date,
            to_date,
            ComboBox2.Text,
            ComboBox1.Text}

        Dim sql As New SQL("CALL VIEW_allowanceperday(?og_rowid, ?e_rowid, ?from_date, ?to_date, ?allowance_frequency, ?allowance_name);",
                           params)

        Try
            Dim dt As New DataTable
            dt = sql.GetFoundRows.Tables(0)

            If sql.HasError Then
                Throw sql.ErrorException
            Else
                dgvempallowance.Rows.Clear()
                For Each drow As DataRow In dt.Rows
                    Dim row_array = drow.ItemArray

                    dgvempallowance.Rows.Add(row_array)
                Next
            End If

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Name))
        Finally

            Static once As Boolean = True
            If once Then
                once = False

                Dim _types =
                (From row
                 In dgvempallowance.Rows.OfType(Of DataGridViewRow)
                 Group By val = Convert.ToString(row.Cells(eall_Type.Name).Value)
                     Into Group
                 Where val IsNot Nothing
                 Select val)

                'ComboBox1.DataSource = _types.ToList
                ComboBox1.Items.AddRange(_types.ToList.ToArray)

                'Dim _frequencies =
                '(From row
                ' In dgvempallowance.Rows.OfType(Of DataGridViewRow)
                ' Group By val = Convert.ToString(row.Cells(AllowanceFrequency.Name).Value)
                '     Into Group
                ' Where val IsNot Nothing
                ' Select val)

                'ComboBox2.DataSource = _frequencies.ToList

                For Each cbox In Panel1.Controls.OfType(Of ComboBox)()
                    cbox.SelectedIndex = -1
                    AddHandler cbox.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
                Next

            End If
        End Try

    End Sub

    Private Sub dgvempallowance_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempallowance.CellContentClick

    End Sub

    Private Sub dgvempallowance_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvempallowance.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

        VIEW_employeeallowance_indate()
    End Sub

End Class