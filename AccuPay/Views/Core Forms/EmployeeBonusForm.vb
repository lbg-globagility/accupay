Public Class EmployeeBonusForm

    Dim n_EmployeeRowID As Object = Nothing

    Dim n_BonusRowID As Object = Nothing

    Dim n_LoanStartDate As Object = Nothing

    Dim n_LoanEndDate As Object = Nothing

    Sub New(Optional EmpRowID As Object = Nothing,
            Optional Bonus_RowID As Object = Nothing,
            Optional loan_startdate As Object = Nothing,
            Optional loan_enddate As Object = Nothing)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        n_EmployeeRowID = EmpRowID

        n_BonusRowID = Bonus_RowID

        n_LoanStartDate = loan_startdate

        n_LoanEndDate = loan_enddate

    End Sub

    Property EmployeeRowID As Object
        Get
            Return n_EmployeeRowID
        End Get
        Set(value As Object)
            n_EmployeeRowID = value
        End Set
    End Property

    Dim n_ShowAsDialog As Boolean = False

    Property ShowAsDialog As Boolean
        Get
            Return n_ShowAsDialog
        End Get
        Set(value As Boolean)
            n_ShowAsDialog = value
        End Set
    End Property

    Dim isShowDialog As Boolean = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            isShowDialog = True

            .Text = someValue.Trim

        End With

        Return Me.ShowDialog

    End Function

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If dgvpayperiod.Focused Then

            If dgvpayperiod.RowCount > 0 Then
                dgvpayperiod_CellContentClick(dgvpayperiod, New DataGridViewCellEventArgs(ppProperDateFrom.Index, dgvpayperiod.CurrentRow.Index))
            End If

            Return True
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)

    End Sub

    Dim view_IDBon As Integer = 0

    Dim dontUpdateBon As SByte = 0

    Private Sub EmployeeBonusControl_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        dgvempbon.EndEdit()

        HasSelection = (dgvempbon.Rows.Cast(Of DataGridViewRow).Where(Function(c) c.Cells("SelectionBox").Value = True And c.IsNewRow = False).Count() > 0) 'Bonus_RowID

        If HasSelection Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        End If

    End Sub

    Private Sub EmployeeBonusControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ToolStrip1.Visible = Not n_ShowAsDialog

        view_IDBon = VIEW_privilege("Employee Bonus", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_IDBon)

        If formuserprivilege.Count = 0 Then

            tsbtnNewBon.Visible = 0
            tsbtnSaveBon.Visible = 0
            tsbtnDelBon.Visible = False
            dontUpdateBon = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    tsbtnNewBon.Visible = 0
                    tsbtnSaveBon.Visible = 0
                    tsbtnDelBon.Visible = False
                    dontUpdateBon = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        tsbtnNewBon.Visible = 0
                    Else
                        tsbtnNewBon.Visible = 1
                    End If

                    If drow("Deleting").ToString = "N" Then
                        tsbtnDelBon.Visible = False
                    Else
                        tsbtnDelBon.Visible = True
                    End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdateBon = 1
                    Else
                        dontUpdateBon = 0
                    End If

                End If

            Next

        End If

        Dim categBonusID = EXECQUER("SELECT RowID FROM category WHERE OrganizationID=" & orgztnID & " AND CategoryName='" & "Bonus" & "' LIMIT 1;")

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT RowID,PartNo FROM product WHERE CategoryID='" & categBonusID & "' AND OrganizationID=" & orgztnID & ";")
        Dim catchdt As New DataTable
        catchdt = n_SQLQueryToDatatable.ResultTable
        bon_Type.ValueMember = catchdt.Columns(0).ColumnName
        bon_Type.DisplayMember = catchdt.Columns(1).ColumnName
        bon_Type.DataSource = catchdt

        n_SQLQueryToDatatable = New SQLQueryToDatatable("SELECT DisplayValue FROM listofval WHERE Type='Allowance Frequency' AND Active='Yes' AND OrderBy=3;")
        catchdt = New DataTable
        catchdt = n_SQLQueryToDatatable.ResultTable
        bon_Frequency.ValueMember = catchdt.Columns(0).ColumnName
        bon_Frequency.DisplayMember = catchdt.Columns(0).ColumnName
        bon_Frequency.DataSource = catchdt

        VIEW_employeebonus(n_EmployeeRowID)

    End Sub

    Sub VIEW_employeebonus(ByVal bon_EmployeeID As Object)
        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL VIEW_employeebonusforloan('" & bon_EmployeeID & "', '" & orgztnID & "'" &
                                    ", '" & n_BonusRowID & "', '" & n_LoanStartDate & "', '" & n_LoanEndDate & "');")
        Dim catchdt As New DataTable
        catchdt = n_SQLQueryToDatatable.ResultTable
        dgvempbon.Rows.Clear()
        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvempbon.Rows.Add(row_array)
        Next
        'If dgvempbon.RowCount > 0 THEN
        Dim fsdf = dgvempbon.Rows.Cast(Of DataGridViewRow).Where(Function(c) ValNoComma(c.Cells(bon_RowID.Index).Value) = n_BonusRowID And c.IsNewRow = False)

        For Each sdfsdf In fsdf
            sdfsdf.Cells("SelectionBox").Value = True
            sdfsdf.ReadOnly = True
        Next

    End Sub

    Private Sub dgvBonus_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempbon.CellContentClick

    End Sub

    Private Sub dgvBonus_SelectionChanged(sender As Object, e As EventArgs) Handles dgvempbon.SelectionChanged

    End Sub

    Dim isSelectionChanged As Boolean = False

    Dim SelectionValueBeforeEdit As Boolean = False

    Dim currentRowIsNew As Boolean = False

    Private Sub dgvBonus_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvempbon.CellBeginEdit
        currentRowIsNew = dgvempbon.Rows(e.RowIndex).IsNewRow
        If TypeOf (dgvempbon.Item(e.ColumnIndex, e.RowIndex)) Is DataGridViewComboBoxCell Then

        ElseIf TypeOf (dgvempbon.Item(e.ColumnIndex, e.RowIndex)) Is DataGridViewCheckBoxCell Then
            SelectionValueBeforeEdit = CBool(dgvempbon.Item(e.ColumnIndex, e.RowIndex).Value)
        Else

        End If

    End Sub

    Private Sub dgvBonus_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempbon.CellEndEdit

        If TypeOf (dgvempbon.Item(e.ColumnIndex, e.RowIndex)) Is DataGridViewComboBoxCell Then

            If bon_Type.Index = e.ColumnIndex Then
                dgvempbon.Item("bon_prodid", e.RowIndex).Value = DirectCast(dgvempbon.Item("bon_type", e.RowIndex), DataGridViewComboBoxCell).Value
            Else

            End If

        ElseIf TypeOf (dgvempbon.Item(e.ColumnIndex, e.RowIndex)) Is DataGridViewCheckBoxCell Then

            'If currentRowIsNew And ToolStrip1.Visible = False Then
            '    isSelectionChanged = False
            '    dgvempbon.Item(e.ColumnIndex, e.RowIndex).Value = isSelectionChanged
            'Else
            isSelectionChanged = (SelectionValueBeforeEdit <> CBool(dgvempbon.Item(e.ColumnIndex, e.RowIndex).Value))
            'End If
        Else

        End If

    End Sub

    Private Sub dgvempbon_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvempbon.EditingControlShowing

    End Sub

    Private Sub tsbtnNewBon_Click(sender As Object, e As EventArgs) Handles tsbtnNewBon.Click

    End Sub

    Private Sub tsbtnSaveBon_Click(sender As Object, e As EventArgs) Handles tsbtnSaveBon.Click

        dgvempbon.EndEdit()

    End Sub

    Private Sub tsbtnCancelBon_Click(sender As Object, e As EventArgs) Handles tsbtnCancelBon.Click

        tsbtnNewBon.Enabled = True

        VIEW_employeebonus(n_EmployeeRowID)

    End Sub

    Private Sub tsbtnDelBon_Click(sender As Object, e As EventArgs) Handles tsbtnDelBon.Click

        dgvempbon.EndEdit()

    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click, Button1.Click, Button2.Click

        Dim sender_name = DirectCast(sender, Control).Name

        If sender_name = "Button1" _
            And dgvempbon.Rows.Cast(Of DataGridViewRow).Where(Function(c) c.Cells("SelectionBox").Value = True And c.IsNewRow = False).Count() = 1 _
            And isSelectionChanged Then

            'will update BonusID column of employeeloanschedule
            'base in the selected employeebonus.RowID

            MsgBox(sender_name)

        End If

        Me.Close()

    End Sub

    Dim HasSelection As Boolean = False

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ToolStrip1.ItemClicked

    End Sub

    Private Sub ToolStrip1_VisibleChanged(sender As Object, e As EventArgs) Handles ToolStrip1.VisibleChanged
        Panel1.Visible = Not ToolStrip1.Visible
        dgvempbon.AllowUserToAddRows = ToolStrip1.Visible

    End Sub

    Private Sub dgvempbon_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvempbon.DataError

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Panel1_VisibleChanged(sender As Object, e As EventArgs) Handles Panel1.VisibleChanged

        dgvpayperiod.Visible = Panel1.Visible

    End Sub

    Private Sub dgvpayperiod_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayperiod.CellContentClick

        If e.ColumnIndex > -1 _
            And e.RowIndex > -1 Then
        Else

        End If

    End Sub

    Private Sub dgvpayperiod_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgvpayperiod.CurrentCellChanged

    End Sub

End Class