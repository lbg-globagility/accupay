Public Class EmployeeBonusControl

    Dim n_EmployeeRowID As Object = Nothing

    Dim n_BonusRowID As Object = Nothing

    Dim n_LoanStartDate As Object = Nothing

    Dim n_LoanEndDate As Object = Nothing

    Private ebonus_rowid_comment As New Dictionary(Of Object, String)

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

    Dim n_EmployeeBonusRowID As Object = Nothing

    ReadOnly Property EmployeeBonusRowID As Object
        Get
            Return n_EmployeeBonusRowID
        End Get
    End Property

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

        Return MyBase.ProcessCmdKey(msg, keyData)

    End Function

    Property BonusComments As Dictionary(Of Object, String)

        Get
            Return ebonus_rowid_comment

        End Get

        Set(value As Dictionary(Of Object, String))
            value = ebonus_rowid_comment

        End Set

    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)

    End Sub

    Dim view_IDBon As Integer = 0

    Dim dontUpdateBon As SByte = 0

    Private Sub EmployeeBonusControl_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        dgvempbon.EndEdit()

        Dim selected_bonus = dgvempbon.Rows.Cast(Of DataGridViewRow).Where(Function(c) c.Cells("SelectionBox").Value = True And c.IsNewRow = False)

        HasSelection = (selected_bonus.Count() > 0) 'Bonus_RowID

        If HasSelection Then
            For Each selrow In selected_bonus
                n_EmployeeBonusRowID = selrow.Cells("bon_RowID").Value
                Exit For
            Next
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
        Dim loan_referredbonus = dgvempbon.Rows.Cast(Of DataGridViewRow).Where(Function(c) ValNoComma(c.Cells(bon_RowID.Index).Value) = n_BonusRowID And c.IsNewRow = False)

        Dim bool_result As Boolean = (loan_referredbonus.Count > 0)

        SelectionBox.ReadOnly = bool_result

        For Each sdfsdf In loan_referredbonus
            sdfsdf.Cells("SelectionBox").Value = bool_result
            'sdfsdf.ReadOnly = True
        Next

    End Sub

    Private Sub dgvBonus_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempbon.CellContentClick

        If TypeOf (dgvempbon.Item(e.ColumnIndex, e.RowIndex)) Is DataGridViewCheckBoxCell Then

            With dgvempbon

                .CurrentCell = .Item("bon_Type", e.RowIndex)
                .CurrentCell = .Item("SelectionBox", e.RowIndex)

            End With

        End If

    End Sub

    Private Sub dgvBonus_SelectionChanged(sender As Object, e As EventArgs) Handles dgvempbon.SelectionChanged

    End Sub

    Dim isSelectionChanged As Boolean = False

    Dim SelectionValueBeforeEdit As Boolean = False

    Private Sub dgvBonus_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvempbon.CellBeginEdit

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
            isSelectionChanged = (SelectionValueBeforeEdit <> CBool(dgvempbon.Item(e.ColumnIndex, e.RowIndex).Value))
            'Dim rows_to_uncheck = dgvempbon.Rows.Cast(Of DataGridViewRow).Where(Function(c) c.Index <> e.RowIndex)

            'For Each dgvrow In rows_to_uncheck
            '    dgvrow.Cells("SelectionBox").Value = False
            'Next

        Else

            Dim _key As Object = dgvempbon.Item("bon_RowID", e.RowIndex).Value
            Dim _value As String =
                If(String.IsNullOrEmpty(dgvempbon.Item("columnRemarks", e.RowIndex).Value),
                   String.Empty,
                   dgvempbon.Item("columnRemarks", e.RowIndex).Value)

            If ebonus_rowid_comment.ContainsKey(_key) Then
                ebonus_rowid_comment.Remove(_key)
            End If

            ebonus_rowid_comment.Add(_key, _value.Trim)

        End If

        pnlBonPotentPayment.Enabled = IsThereBonusAppliedToLoan()

        n_EmployeeBonusRowID = Nothing

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

        dgvempbon.EndEdit()

        Dim sender_name = DirectCast(sender, Control).Name

        If sender_name = "Button1" _
            And dgvempbon.Rows.Cast(Of DataGridViewRow).Where(Function(c) c.Cells("SelectionBox").Value = True And c.IsNewRow = False).Count() = 1 _
            And isSelectionChanged Then

            'will update BonusID column of employeeloanschedule
            'base in the selected employeebonus.RowID

            'MsgBox(sender_name)

        ElseIf sender_name = "Button2" _
            And dgvempbon.Rows.Cast(Of DataGridViewRow).Where(Function(c) c.Cells("SelectionBox").Value = True And c.IsNewRow = False).Count() = 1 Then

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

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        dgvempbon.Rows.Add()
    End Sub

    Private Function IsThereBonusAppliedToLoan() As Boolean

        Dim dgvrow =
            dgvempbon.Rows.OfType(Of DataGridViewRow).Where(Function(dgvr) dgvr.Cells("SelectionBox").Value = True)

        Dim return_val As Boolean = (dgvrow.Count > 0)

        Return return_val

    End Function

End Class