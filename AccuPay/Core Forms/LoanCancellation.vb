
Public Class LoanCancellation

    Dim ReferenceLoanID As Object = Nothing

    Dim Employee_RowID As Object = Nothing

    Sub New(ReferenceLoanRowID As Object)

        ReferenceLoanID = ReferenceLoanRowID

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        '***********************************

        'Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()

        'DataGridViewCellStyle8.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        'elsLoanType.DefaultCellStyle = DataGridViewCellStyle8
        'elsTotalLoanAmount.DefaultCellStyle = DataGridViewCellStyle8
        'elsTotalBalanceLeft.DefaultCellStyle = DataGridViewCellStyle8
        'elsDedEffectiveDateFrom.DefaultCellStyle = DataGridViewCellStyle8
        'elsDedEffectiveDateTo.DefaultCellStyle = DataGridViewCellStyle8
        'elsNoOfPayPeriod.DefaultCellStyle = DataGridViewCellStyle8
        'elsLoanPayPeriodLeft.DefaultCellStyle = DataGridViewCellStyle8
        'elsDeductionAmount.DefaultCellStyle = DataGridViewCellStyle8
        'elsStatus.DefaultCellStyle = DataGridViewCellStyle8
        'elsDeductionSchedule.DefaultCellStyle = DataGridViewCellStyle8

    End Sub

    Dim cancell_status = New ExecuteQuery("SELECT ii.COLUMN_COMMENT FROM information_schema.`COLUMNS` ii WHERE ii.TABLE_SCHEMA='" & sys_db & "' AND ii.COLUMN_NAME='Status' AND ii.TABLE_NAME='employeeloanschedule';").Result

    Dim status_last_index = Split(cancell_status, ",")

    Dim status_last_index_value As String = status_last_index(status_last_index.GetUpperBound(0))

    Protected Overrides Sub OnLoad(e As EventArgs)

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            status_last_index_value = Trim(status_last_index_value)

            Dim New_SQLQueryToDatatable As _
                New SQLQueryToDatatable("SELECT RowID,PartNo FROM product WHERE `Category`='Loan Type' AND OrganizationID='" & orgztnID & "';")

            Dim dt As New DataTable

            dt = New_SQLQueryToDatatable.ResultTable

            elsLoanType.ValueMember = dt.Columns(0).ColumnName

            elsLoanType.DisplayMember = dt.Columns(1).ColumnName

            elsLoanType.DataSource = dt

            'enlistTheLists("SELECT DisplayValue FROM listofval WHERE `Type`='Government deduction schedule' AND Active='Yes' AND OrderBy > 0 ORDER BY OrderBy;", loandeducsched)

            'elsDeductionSchedule.dat

            Dim New_SQLQueryToDatTble As _
                New SQLQueryToDatatable("SELECT DisplayValue,DisplayValue AS DeductionSchedule FROM listofval WHERE `Type`='Government deduction schedule' AND Active='Yes' AND OrderBy > 0 ORDER BY OrderBy;")

            Dim dt2 As New DataTable

            dt2 = New_SQLQueryToDatTble.ResultTable

            elsDeductionSchedule.ValueMember = dt2.Columns(0).ColumnName

            elsDeductionSchedule.DisplayMember = dt2.Columns(1).ColumnName

            elsDeductionSchedule.DataSource = dt2

        End If
        '",els.DedEffectiveDateTo" &
        Dim NewSQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL `VEIW_ref_employeeloanschedule`('" & ReferenceLoanID & "'" &
                                    ",'" & orgztnID & "');")

        cboloantype.Text = ""

        txtloannumber.Text = ""

        txtloanamt.Text = ""

        txtbal.Text = ""

        datefrom.Value = datefrom.MinDate

        dateto.Value = dateto.MinDate

        txtnoofpayper.Text = ""

        txtnoofpayperleft.Text = ""

        txtdedamt.Text = ""

        cmbStatus.Text = ""

        txtloaninterest.Text = ""

        txtRemarks.Text = ""

        cmbdedsched.Text = ""

        For Each drow As DataRow In NewSQLQueryToDatatable.ResultTable.Rows

            cboloantype.Text = drow("LoanType")

            txtloannumber.Text = drow("LoanNumber")

            txtloanamt.Text = drow("TotalLoanAmount")

            txtbal.Text = drow("TotalBalanceLeft")

            datefrom.Value = drow("DedEffectiveDateFrom")

            dateto.Value = drow("DedEffectiveDateTo")

            'elsDedEffectiveDateFrom

            txtnoofpayper.Text = drow("NoOfPayPeriod")

            txtnoofpayperleft.Text = drow("LoanPayPeriodLeft")

            txtdedamt.Text = drow("DeductionAmount")

            cmbStatus.Text = drow("Status")

            txtloaninterest.Text = drow("DeductionPercentage")

            txtRemarks.Text = drow("Comments")

            cmbdedsched.Text = drow("DeductionSchedule")

            Employee_RowID = drow("EmployeeID")

        Next

        '****************************************

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL VIEW_related_loan_cancelled('" & orgztnID & "','" & ReferenceLoanID & "');")

        Dim dtbl As New DataTable

        dtbl = n_SQLQueryToDatatable.ResultTable

        dgvLoanItem.Rows.Clear()

        For Each drow As DataRow In dtbl.Rows

            Dim row_array = drow.ItemArray

            Dim n =
                dgvLoanItem.Rows.Add(row_array)

            dgvLoanItem.Item("elsDedEffectiveDateFrom", n).Tag =
                MYSQLDateFormat(CDate(drow("DedEffectiveDateFrom")))

            dgvLoanItem.Item("elsDedEffectiveDateTo", n).Tag =
                MYSQLDateFormat(CDate(drow("DedEffectiveDateTo")))

        Next

        MyBase.OnLoad(e)

    End Sub

    Private Sub LoanCancellation_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        'Me.DialogResult = Windows.Forms.DialogResult.Cancel

    End Sub

    Private Sub LoanCancellation_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Dim isShowAsDialog As Boolean = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            isShowAsDialog = True

            .Text = someValue

        End With

        Return Me.ShowDialog

    End Function

    Private Sub tsbtnSaveLoan_Click(sender As Object, e As EventArgs) Handles tsbtnSaveLoan.Click

    End Sub

    Private Sub tsbtnSaveLoan_Click1(sender As Object, e As EventArgs)

        tsbtnSaveLoan.Enabled = False

        dgvLoanItem.EndEdit(True)

        If CBool(PanelEqual.Tag) = False Then

            Exit Sub

        End If

        Dim loan_typeRowID As Object = Nothing

        Dim hasErrors As Boolean = False

        Dim errorRowIndex As Integer = -1

        Dim errorColumnIndex As Integer = -1

        'Execute validations here
        For Each dgvrow As DataGridViewRow In dgvLoanItem.Rows

            With dgvrow

                If .IsNewRow = False Then

                    errorRowIndex = .Index

                    If .Cells("elsLoanType").Value = Nothing Then 'no loan type has selected

                        errorColumnIndex = elsLoanType.Index

                        hasErrors = True

                        Exit For

                    ElseIf ValNoComma(.Cells("elsTotalLoanAmount").Value) = 0 Then 'no value in total loan amount has inputted

                        errorColumnIndex = elsTotalLoanAmount.Index

                        hasErrors = True

                        Exit For

                    ElseIf .Cells("elsDedEffectiveDateFrom").Value = Nothing Then
                        'Or .Cells("elsDedEffectiveDateFrom").Tag = Nothing Then 'invalid date from

                        elsDedEffectiveDateFrom.Tag = Nothing

                        errorColumnIndex = elsDedEffectiveDateFrom.Index

                        hasErrors = True

                        Exit For

                    ElseIf .Cells("elsDedEffectiveDateTo").Value = Nothing Then
                        'Or .Cells("elsDedEffectiveDateTo").Tag = Nothing Then 'invalid date to

                        errorColumnIndex = elsDedEffectiveDateTo.Index

                        hasErrors = True

                        Exit For

                    ElseIf .Cells("elsStatus").Value Is Nothing Then 'invalid status

                        errorColumnIndex = elsStatus.Index

                        hasErrors = True

                        Exit For

                    ElseIf CStr(.Cells("elsStatus").Value).Length = 0 Then 'invalid status

                        errorColumnIndex = elsStatus.Index

                        hasErrors = True

                        Exit For

                    ElseIf .Cells("elsDeductionSchedule").Value Is Nothing Then 'invalid deduction sched

                        errorColumnIndex = elsDeductionSchedule.Index

                        hasErrors = True

                        Exit For

                    ElseIf CStr(.Cells("elsDeductionSchedule").Value).Length = 0 Then 'invalid deduction sched

                        errorColumnIndex = elsDeductionSchedule.Index

                        hasErrors = True

                        Exit For

                    Else

                    End If

                End If

            End With

            'elsRowID
            'elsLoanType
            'elsLoanNumber
            'elsTotalLoanAmount
            'elsTotalBalanceLeft
            'elsDedEffectiveDateFrom
            'elsDedEffectiveDateTo
            'elsNoOfPayPeriod
            'elsLoanPayPeriodLeft
            'elsDeductionAmount
            'elsStatus
            'elsDeductionPercentage
            'elsComments
            'elsDeductionSchedule

        Next



        If hasErrors = False Then 'valid data

            For Each dgvrow As DataGridViewRow In dgvLoanItem.Rows

                loan_typeRowID = Nothing

                If dgvrow.IsNewRow Then

                    Continue For

                Else

                    With dgvrow

                        'SelectedValue for the product RowID
                        loan_typeRowID = .Cells("elsLoanType").Value

                        If .Cells("elsRowID").Value = Nothing Then

                            'els_RowID,els_OrganizID,els_UserRowID,els_EmployeeID,els_LoanNumber
                            ',els_DedEffectiveDateFrom,els_DedEffectiveDateTo,els_TotalLoanAmount
                            ',els_DeductionSchedule,els_TotalBalanceLeft,els_DeductionAmount,els_Status
                            ',els_LoanTypeID,els_DeductionPercentage,els_NoOfPayPeriod,els_LoanPayPeriodLeft,els_Comments,els_ReferenceLoanID

                            .Cells("elsRowID").Value = _
                                New ReadSQLFunction("INSUPD_employeeloanschedule",
                                                        "returnvalue",
                                                    If(.Cells("elsRowID").Value = Nothing, DBNull.Value, .Cells("elsRowID").Value),
                                                    orgztnID,
                                                    z_User,
                                                    Employee_RowID,
                                                    .Cells("elsLoanNumber").Value,
                                                    .Cells("elsDedEffectiveDateFrom").Tag,
                                                    .Cells("elsDedEffectiveDateTo").Tag,
                                                    .Cells("elsTotalLoanAmount").Value,
                                                    .Cells("elsDeductionSchedule").Value,
                                                    .Cells("elsTotalBalanceLeft").Value,
                                                    .Cells("elsDeductionAmount").Value,
                                                    .Cells("elsStatus").Value,
                                                    If(loan_typeRowID = Nothing, DBNull.Value, loan_typeRowID),
                                                    ValNoComma(.Cells("elsDeductionPercentage").Value),
                                                    .Cells("elsNoOfPayPeriod").Value,
                                                    .Cells("elsLoanPayPeriodLeft").Value,
                                                    .Cells("elsComments").Value,
                                                    ReferenceLoanID).ReturnValue

                        Else

                            Dim n_ReadSQLFunction As _
                                New ReadSQLFunction("INSUPD_employeeloanschedule",
                                                        "returnvalue",
                                                    If(.Cells("elsRowID").Value = Nothing, DBNull.Value, .Cells("elsRowID").Value),
                                                    orgztnID,
                                                    z_User,
                                                    Employee_RowID,
                                                    .Cells("elsLoanNumber").Value,
                                                    .Cells("elsDedEffectiveDateFrom").Tag,
                                                    .Cells("elsDedEffectiveDateTo").Tag,
                                                    .Cells("elsTotalLoanAmount").Value,
                                                    .Cells("elsDeductionSchedule").Value,
                                                    .Cells("elsTotalBalanceLeft").Value,
                                                    .Cells("elsDeductionAmount").Value,
                                                    .Cells("elsStatus").Value,
                                                    If(loan_typeRowID = Nothing, DBNull.Value, loan_typeRowID),
                                                    ValNoComma(.Cells("elsDeductionPercentage").Value),
                                                    .Cells("elsNoOfPayPeriod").Value,
                                                    .Cells("elsLoanPayPeriodLeft").Value,
                                                    .Cells("elsComments").Value,
                                                    ReferenceLoanID)

                        End If

                    End With

                End If

            Next

            If dgvLoanItem.RowCount > 1 Then

                Dim n_ExecuteQuery As _
                    New ExecuteQuery("UPDATE employeeloanschedule" &
                                     " SET `Status`='Cancelled'" &
                                     ",LastUpd=CURRENT_TIMESTAMP()" &
                                     ",LastUpdBy='" & z_User & "'" &
                                     " WHERE RowID='" & ReferenceLoanID & "'" &
                                     " AND OrganizationID='" & orgztnID & "';")

                'SubstituteEndDate

            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK

        Else

            If errorRowIndex > -1 _
                And errorColumnIndex > -1 Then

                dgvLoanItem.Item(errorColumnIndex, errorRowIndex).Selected = True

            End If

        End If

        tsbtnSaveLoan.Enabled = True

    End Sub

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click

        Label2.Text = "0.00"

        OnLoad(e)

    End Sub

    Dim totalloanamount_before_edit As Double = ValNoComma(0)

    Private Sub dgvLoanItem_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvLoanItem.CellBeginEdit



        If elsTotalLoanAmount.Index = e.ColumnIndex Then

            totalloanamount_before_edit = ValNoComma(dgvLoanItem.Item("elsTotalLoanAmount",
                                                                      e.RowIndex).Value)

        Else
            totalloanamount_before_edit = 0
        End If

    End Sub

    Private Sub dgvLoanItem_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLoanItem.CellContentClick
        Dim fsdfs = elsLoanType.Index
    End Sub

    Private Sub dgvLoanItem_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLoanItem.CellEndEdit

        If e.RowIndex > -1 _
            And e.ColumnIndex > -1 Then

            'elsRowID
            'elsLoanType
            'elsLoanNumber
            'elsTotalLoanAmount
            'elsTotalBalanceLeft
            'elsDedEffectiveDateFrom
            'elsDedEffectiveDateTo
            'elsNoOfPayPeriod
            'elsLoanPayPeriodLeft
            'elsDeductionAmount
            'elsStatus
            'elsDeductionPercentage
            'elsComments
            'elsDeductionSchedule

            Dim anyvalue = dgvLoanItem.Item("elsDedEffectiveDateFrom", e.RowIndex).Tag

            If elsDedEffectiveDateFrom.Index = e.ColumnIndex _
                Or elsDeductionSchedule.Index = e.ColumnIndex _
                Or elsNoOfPayPeriod.Index = e.ColumnIndex Then

                If dgvLoanItem.Item("elsDeductionSchedule", e.RowIndex).Value = Nothing Then

                Else

                    'If dgvLoanItem.Item("elsDedEffectiveDateFrom", e.RowIndex).Tag = Nothing _
                    If dgvLoanItem.Item("elsDedEffectiveDateFrom", e.RowIndex).Value = Nothing Then

                    Else

                        Dim payperiod_to =
                            New ExecuteQuery("SELECT PAYTODATE_OF_NoOfPayPeriod('" & MYSQLDateFormat(CDate(dgvLoanItem.Item("elsDedEffectiveDateFrom", e.RowIndex).Value)) & "'" &
                                             "," & ValNoComma(dgvLoanItem.Item("elsNoOfPayPeriod", e.RowIndex).Value) & "" &
                                             "," & ValNoComma(Employee_RowID) &
                                             ",'" & CStr(dgvLoanItem.Item("elsDeductionSchedule", e.RowIndex).Value) & "');").Result

                        If IsDBNull(payperiod_to) Then

                            dgvLoanItem.Item("elsDedEffectiveDateTo", e.RowIndex).Value = Nothing

                            dgvLoanItem.Item("elsDedEffectiveDateTo", e.RowIndex).Tag = Nothing

                        ElseIf payperiod_to = Nothing Then

                            dgvLoanItem.Item("elsDedEffectiveDateTo", e.RowIndex).Value = Nothing

                            dgvLoanItem.Item("elsDedEffectiveDateTo", e.RowIndex).Tag = Nothing

                        Else

                            dgvLoanItem.Item("elsDedEffectiveDateTo", e.RowIndex).Value = CDate(payperiod_to).ToShortDateString

                            dgvLoanItem.Item("elsDedEffectiveDateTo", e.RowIndex).Tag = MYSQLDateFormat(CDate(payperiod_to))

                        End If

                        dgvLoanItem.Item("elsDedEffectiveDateFrom", e.RowIndex).Tag = _
                            MYSQLDateFormat(CDate(dgvLoanItem.Item("elsDedEffectiveDateFrom", e.RowIndex).Value))

                    End If

                End If

            ElseIf elsTotalLoanAmount.Index = e.ColumnIndex Then

                Label2.Text = ValNoComma(Label2.Text) - totalloanamount_before_edit

                Label2.Text = ValNoComma(Label2.Text) + ValNoComma(dgvLoanItem.Item("elsTotalLoanAmount", e.RowIndex).Value)

                dgvLoanItem.Item("elsTotalBalanceLeft", e.RowIndex).Value = ValNoComma(dgvLoanItem.Item("elsTotalLoanAmount", e.RowIndex).Value)


            ElseIf elsDeductionAmount.Index = e.ColumnIndex Then

                Dim int_result = ValNoComma(dgvLoanItem.Item("elsTotalLoanAmount", e.RowIndex).Value) _
                    / ValNoComma(dgvLoanItem.Item("elsDeductionAmount", e.RowIndex).Value)

                dgvLoanItem.Item("elsNoOfPayPeriod", e.RowIndex).Value = Math.Round(int_result, 0)

            Else

            End If

            If dgvLoanItem.Rows(e.RowIndex).IsNewRow Then

            Else

                If elsLoanPayPeriodLeft.Index <> e.RowIndex Then

                    dgvLoanItem.Item("elsLoanPayPeriodLeft", e.RowIndex).Value = ValNoComma(dgvLoanItem.Item("elsNoOfPayPeriod", e.RowIndex).Value)

                End If
                
                If elsDeductionAmount.Index <> e.RowIndex Then

                    dgvLoanItem.Item("elsDeductionAmount", e.RowIndex).Value = ValNoComma(dgvLoanItem.Item("elsTotalLoanAmount", e.RowIndex).Value) _
                        / ValNoComma(dgvLoanItem.Item("elsNoOfPayPeriod", e.RowIndex).Value)

                End If

                If dgvLoanItem.Item("elsStatus", e.RowIndex).Value = Nothing Then
                    dgvLoanItem.Item("elsStatus", e.RowIndex).Value = "In Progress"
                End If

                If e.RowIndex >= 1 Then

                    Try
                        dgvLoanItem.Item("elsDedEffectiveDateFrom", e.RowIndex).Value = CDate(dgvLoanItem.Item("elsDedEffectiveDateTo", (e.RowIndex - 1)).Value).AddDays(1)
                    Catch ex As Exception
                        dgvLoanItem.Item("elsDedEffectiveDateFrom", e.RowIndex).Value = Nothing
                    End Try

                End If

            End If

        Else

        End If

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Dim my_green = Color.FromArgb(37, 106, 46)

    Dim my_red = Color.FromArgb(255, 30, 30)

    Private Sub Label2_TextChanged(sender As Object, e As EventArgs) Handles Label2.TextChanged,
                                                                                txtbal.TextChanged

        Label3.Text = "/ " & txtbal.Text

        If ValNoComma(txtbal.Text) = ValNoComma(Label2.Text) Then
            Label2.ForeColor = my_green
            PanelEqual.Tag = True
        Else
            Label2.ForeColor = my_red
            PanelEqual.Tag = False
        End If

    End Sub

    Private Sub dgvLoanItem_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvLoanItem.DataError

        e.ThrowException = False

    End Sub

    Private Sub dgvLoanItem_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvLoanItem.RowsAdded

        Try

            If dgvLoanItem.Rows(e.RowIndex).IsNewRow = False Then

                Label2.Text = ValNoComma(Label2.Text) + ValNoComma(dgvLoanItem.Item("elsTotalLoanAmount", e.RowIndex).Value)

            End If

        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))

        End Try

    End Sub

    Private Sub dgvLoanItem_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvLoanItem.RowsRemoved

    End Sub

    Private Sub dgvLoanItem_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles dgvLoanItem.UserDeletedRow

    End Sub

    Private Sub dgvLoanItem_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles dgvLoanItem.UserDeletingRow

        Try

            If e.Row.IsNewRow = False Then
                'And dgvLoanItem.Rows(e.RowIndex).IsNewRow = False Then

                'Label2.Text = ValNoComma(Label2.Text) - ValNoComma(dgvLoanItem.Item("elsTotalLoanAmount", e.Row.Index).Value)
                Label2.Text = ValNoComma(Label2.Text) - ValNoComma(e.Row.Cells("elsTotalLoanAmount").Value)

            End If

        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))

        End Try

    End Sub

    Private Sub cmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus.SelectedIndexChanged

    End Sub

    Private Sub cmbStatus_TextChanged(sender As Object, e As EventArgs) Handles cmbStatus.TextChanged

        RemoveHandler tsbtnSaveLoan.Click, AddressOf tsbtnSaveLoan_Click1

        If cmbStatus.Text.Length > 0 Then

            If cmbStatus.Text = status_last_index_value Then

            Else

                AddHandler tsbtnSaveLoan.Click, AddressOf tsbtnSaveLoan_Click1

            End If

        End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            If dgvLoanItem.IsCurrentCellInEditMode Then

            Else

                If tsbtnSaveLoan.Enabled Then
                    Me.Close()
                End If
                
            End If

            Return Not dgvLoanItem.IsCurrentCellInEditMode

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

End Class