Imports AccuPay.Data.Helpers
Imports Microsoft.EntityFrameworkCore

Public Class viewtotallow
    Dim categallowID As Object = Nothing

    Dim allowance_type As New AutoCompleteStringCollection
    Private employeeId As Object
    Private periodDateFrom As Object
    Private periodDateTo As Object

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
        LoadAllowanceTypesAsync()

        For Each cbox In Panel1.Controls.OfType(Of ComboBox)()
            cbox.SelectedIndex = -1
            AddHandler cbox.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
        Next

        AddHandler ComboBox1.DropDown, AddressOf ComboBox1_DropDown

    End Sub

    Sub VIEW_allowanceperday(Optional eallow_EmployeeID As Object = Nothing,
                               Optional datefrom As Object = Nothing,
                               Optional dateto As Object = Nothing,
                               Optional num_weekdays As Object = Nothing,
                               Optional AllowanceExcept As String = Nothing)

        employeeId = eallow_EmployeeID

        periodDateFrom = datefrom
        periodDateTo = dateto

        Dim params =
            New Object() {orgztnID,
            employeeId,
            periodDateFrom,
            periodDateTo,
            ComboBox2.Text,
            ComboBox1.Text.Trim()}

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
        VIEW_allowanceperday(employeeId, periodDateFrom, periodDateTo)

    End Sub

    Private Async Sub LoadAllowanceTypesAsync()
        ComboBox1.Items.Clear()
        Dim orgId = Convert.ToInt32(orgztnID)

        Using context = New PayrollContext
            Dim allowanceTypes = Await context.Products.
                Where(Function(p) Nullable.Equals(p.OrganizationID, orgId)).
                Where(Function(p) p.PartNo.Trim().Length > 0).
                Where(Function(p) Nullable.Equals(p.Category, ProductConstant.ALLOWANCE_TYPE_CATEGORY)).
                Where(Function(p) p.ActiveData).
                Select(Function(p) p.PartNo).
                OrderBy(Function(p) p.Trim()).
                ToListAsync

            ComboBox1.Items.AddRange(allowanceTypes.ToList.ToArray)
        End Using
    End Sub

    Private Sub ComboBox1_DropDown(sender As Object, e As EventArgs)

        Static cb_font As Font = ComboBox1.Font

        Dim grp As Graphics = ComboBox1.CreateGraphics()

        Dim vertScrollBarWidth As Integer = If(ComboBox1.Items.Count > ComboBox1.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0)

        Dim allowanceTypes = ComboBox1.Items().OfType(Of String).OrderByDescending(Function(s) s.Length).Take(1).FirstOrDefault

        Dim wiidth = CInt(grp.MeasureString(allowanceTypes, cb_font).Width) + vertScrollBarWidth

        ComboBox1.DropDownWidth = wiidth

        RemoveHandler ComboBox1.DropDown, AddressOf ComboBox1_DropDown
    End Sub

End Class