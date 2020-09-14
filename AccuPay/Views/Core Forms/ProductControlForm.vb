Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection
Imports MySql.Data.MySqlClient

Public Class ProductControlForm

    Dim isShowAsDialog As Boolean = False

    Public Property IsSaved As Boolean

    Private ReadOnly _productDataService As ProductDataService

    Sub New()

        InitializeComponent()

        _productDataService = MainServiceProvider.GetRequiredService(Of ProductDataService)

    End Sub

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            isShowAsDialog = True

            .Text = someValue

            IsSaved = False

        End With

        Return Me.ShowDialog

    End Function

    Dim n_categname As String = String.Empty

    Property NameOfCategory As String

        Get
            Return n_categname
        End Get
        Set(value As String)
            n_categname = value
        End Set
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim ii = Status.Index

        MyBase.OnLoad(e)

    End Sub

    Private Sub ProdCtrlForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If ToolStripButton2.Enabled Then

            Dim haschangestoDB =
                EXECQUER("SELECT EXISTS(SELECT" &
                         " RowID" &
                         " FROM product" &
                         " WHERE OrganizationID='" & orgztnID & "'" &
                         " AND `Category`='" & n_categname & "'" &
                         " AND (DATE_FORMAT(Created, '%Y-%m-%d') = CURDATE() OR DATE_FORMAT(LastUpd, '%Y-%m-%d') = CURDATE())" &
                         " LIMIT 1);")

            If haschangestoDB = 1 Then
                Me.DialogResult = DialogResult.OK
            Else
                Me.DialogResult = DialogResult.Cancel

            End If
        Else
            e.Cancel = True

        End If

    End Sub

    Private Sub ProductControlForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ToolStripButton3_Click(sender, e)

    End Sub

    Function INS_product(Optional prod_rowID As Object = Nothing,
                         Optional p_Name As Object = Nothing,
                         Optional p_PartNo As Object = Nothing,
                         Optional p_CategName As Object = Nothing,
                         Optional p_Status As Object = "Active",
                         Optional p_IsFixed As Object = "0",
                         Optional p_IsIncludedIn13th As Object = "0") As Object

        'Dim _naw As Object = EXECQUER("SELECT DATE_FORMAT(NOW(),'%Y-%m-%d %h:%i:%s');")

        Dim returnvalue As Object = Nothing

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            Dim cmdquer As MySqlCommand

            cmdquer = New MySqlCommand("INSUPD_product", conn)

            conn.Open()

            With cmdquer

                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                'If Val(p_RowID) = 0 Then 'THIS WILL INSERT A PRODUCT (in this case, this is as Illness)

                'If r.IsNewRow = False Then

                .Parameters.Add("prod_RowID", MySqlDbType.Int32)

                .Parameters.AddWithValue("p_RowID", If(prod_rowID = Nothing, DBNull.Value, prod_rowID))
                .Parameters.AddWithValue("p_Name", p_Name)
                .Parameters.AddWithValue("p_OrganizationID", orgztnID) 'orgztnID
                .Parameters.AddWithValue("p_PartNo", p_PartNo)
                .Parameters.AddWithValue("p_LastUpd", DBNull.Value)
                .Parameters.AddWithValue("p_CreatedBy", z_User)
                .Parameters.AddWithValue("p_LastUpdBy", z_User)
                .Parameters.AddWithValue("p_Category", p_CategName)
                .Parameters.AddWithValue("p_CategoryID", DBNull.Value)
                .Parameters.AddWithValue("p_Status", p_Status)
                .Parameters.AddWithValue("p_UnitPrice", 0.0)
                .Parameters.AddWithValue("p_UnitOfMeasure", 0)
                .Parameters.AddWithValue("p_IsFixed", p_IsFixed)
                .Parameters.AddWithValue("p_IsIncludedIn13th", p_IsIncludedIn13th)

                .Parameters("prod_RowID").Direction = ParameterDirection.ReturnValue

                Dim datrd As MySqlDataReader

                datrd = .ExecuteReader()

                returnvalue = datrd(0)

                Me.IsSaved = True

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " INSUPD_product")
            returnvalue = Nothing
        Finally
            conn.Close()
        End Try

        Return returnvalue

    End Function

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        dgvproducts.EndEdit(True)

        For Each dgvrow As DataGridViewRow In dgvproducts.Rows
            If dgvrow.IsNewRow Then
                dgvproducts.Item("PartNo", dgvrow.Index).Selected = True
                Exit For
            End If
        Next

    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click

        ToolStripButton2.Enabled = False

        dgvproducts.EndEdit(True)

        Dim validRows = dgvproducts.Rows.OfType(Of DataGridViewRow).Where(Function(r) Not r.IsNewRow)

        For Each drow As DataGridViewRow In validRows

            Dim datastatus As Short = 0

            datastatus = Convert.ToInt16(drow.Cells("Status").Value)

            Dim allowanceTypeId = drow.Cells("RowID").Value

            Dim has_no_rowid = CBool(allowanceTypeId = Nothing)

            Dim returnval =
                    INS_product(If(has_no_rowid, Nothing, drow.Cells("RowID").Value),
                                drow.Cells("PartNo").Value,
                                drow.Cells("PartNo").Value,
                                n_categname,
                                datastatus,
                                Convert.ToInt16(drow.Cells("Fixed").Value),
                                Convert.ToInt16(drow.Cells("AllocateBelowSafetyFlag").Value))

            If has_no_rowid Then
                drow.Cells("RowID").Value = returnval
            End If

        Next

        ToolStripButton2.Enabled = True

        If ToolStripButton2.Enabled Then
            InfoBalloon("Changes made were successfully saved.", "Successfully saved", lblforballoon, 0, -69)
        End If

    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Static once As SByte = 0

        If once = 0 Then

            once = 1

            'Status.Items.Add("Yes")

            'Status.Items.Add("No")

        End If

        Dim selectAllProduct As New DataTable

        'selectAllProduct = retAsDatTbl("SELECT p.*, IF(p.`Status` = '0', 'No', 'Yes') AS IStatus" & _
        '                               " FROM product p" & _
        '                               " INNER JOIN category c ON c.OrganizationID='" & orgztnID & "' AND CategoryName='" & n_categname & "'" & _
        '                               " WHERE p.OrganizationID='" & orgztnID & "'" & _
        '                               " AND p.CategoryID=c.RowID" & _
        '                               " AND p.ActiveData='1';")

        Dim allowance_typequery As String =
            String.Concat("SELECT p.*, IF(p.`Status` = '0', 'No', 'Yes') `IStatus`",
                          " FROM product p",
                          " INNER JOIN category c ON c.RowID=p.CategoryID AND c.OrganizationID=p.OrganizationID AND CategoryName=?categ_name",
                          " WHERE p.OrganizationID=?og_id",
                          " AND p.CategoryID=c.RowID",
                          " AND p.ActiveData='1';")

        Dim sql As New SQL(allowance_typequery,
                           n_categname, orgztnID)

        selectAllProduct = sql.GetFoundRows.Tables(0)

        'dgvproducts.Rows.Clear()

        'For Each dcol As DataGridViewColumn In dgvproducts.Columns
        '    File.AppendAllText("D:\DOWNLOADS\New_Text Document.txt", dcol.Name & ",")
        'Next

        dgvproducts.Rows.Clear()

        For Each drow As DataRow In selectAllProduct.Rows

            dgvproducts.Rows.Add(drow("RowID"),
                                 Nothing,
                                 drow("Name"),
                                 drow("Description"),
                                 drow("PartNo"),
                                 drow("Category"),
                                 drow("CategoryID"),
                                 (CShort(drow("Status")) = 1),
                                 CBool(drow("Fixed")), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                                 CBool(drow("AllocateBelowSafetyFlag")))

            'RowID,SupplierID,ProdName,Description,PartNo,Category,CategoryID
            ',Status,Fixed,UnitPrice,VATPercent,FirstBillFlag,SecondBillFlag,ThirdBillFlag
            ',PDCFlag,MonthlyBIllFlag,PenaltyFlag,WithholdingTaxPercent,CostPrice,UnitOfMeasure
            ',SKU,LeadTime,BarCode,BusinessUnitID,LastRcvdFromShipmentDate,LastRcvdFromShipmentCount
            ',TotalShipmentCount,BookPageNo,BrandName,LastPurchaseDate,LastSoldDate,LastSoldCount,ReOrderPoint
            ',AllocateBelowSafetyFlag,Strength,UnitsBackordered,UnitsBackorderAsOf,DateLastInventoryCount,TaxVAT,WithholdingTax,

        Next

        selectAllProduct.Dispose()

    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click

    End Sub

    Dim isCellInEditMode = False

    Private Sub dgvpayper_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvproducts.CellBeginEdit
        isCellInEditMode = True
        'dgvproducts.Item(e.ColumnIndex, e.RowIndex).ContextMenuStrip = cmsBlank
    End Sub

    Private Sub dgvpayper_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvproducts.CellContentClick

    End Sub

    Private Sub dgvpayper_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvproducts.CellEndEdit
        isCellInEditMode = False
    End Sub

    'Const WM_LBUTTONDBLCLK = &H203
    'Const WM_RBUTTONDBLCLK = &H206
    'Const WM_MBUTTONDBLCLK = &H209

    'Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    '    Select Case m.Msg
    '        Case WM_RBUTTONDBLCLK, WM_MBUTTONDBLCLK : Return
    '            'Case WM_LBUTTONDBLCLK : Return
    '        Case Else
    '            Call MyBase.WndProc(m)
    '    End Select
    'End Sub

    Dim selected_rowindex As Integer = -1

    Private Sub dgvproducts_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvproducts.CellMouseDown

        selected_rowindex = -1

        If e.RowIndex > -1 And e.ColumnIndex > -1 Then

            dgvproducts.Item(e.ColumnIndex, e.RowIndex).ContextMenuStrip = New ContextMenuStrip

            If dgvproducts.Rows(e.RowIndex).IsNewRow Then
                ContextMenuStrip1.Hide()
            Else

                If e.Button = Windows.Forms.MouseButtons.Right Then

                    dgvproducts.EndEdit(True)

                    If ToolStripButton2.Visible _
                        And dgvproducts.IsCurrentCellInEditMode = False Then

                        For Each dgvrow As DataGridViewRow In dgvproducts.Rows
                            dgvrow.Selected = False
                        Next

                        dgvproducts.Focus()

                        selected_rowindex = e.RowIndex

                        dgvproducts.Item(e.ColumnIndex, selected_rowindex).Selected = True

                        ContextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Default)

                    End If

                End If

            End If
        Else
            ContextMenuStrip1.Hide()

        End If

    End Sub

    Private Sub dgvpayper_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvproducts.EditingControlShowing

        'If TypeOf e.Control Is TextBox Then
        'e.Control.ContextMenuStrip = cmsBlank
        'e.Control.ContextMenu = New ContextMenu
        'Else
        '    Label1.Text = "Is not text box"
        'End If

        'If dgvAdjustments.CurrentCell.ColumnIndex = DataGridViewTextBoxColumn66.Index Then

        '    With DirectCast(e.Control, TextBox)

        '        Dim n_DGVColKeyPressHandler As New DGVColKeyPressHandler

        '        AddHandler .KeyPress, AddressOf n_DGVColKeyPressHandler.KeyPressHandler

        '    End With

        '    'With DirectCast(e.Control, DataGridViewTextBoxEditingControl)

        'ElseIf TypeOf e.Control Is TextBox Then

        '    'DataGridViewTextBoxColumn66'DataGridViewTextBoxColumn64
        '    With DirectCast(e.Control, TextBox)

        '        Dim n_DGVColKeyPressHandler As New DGVColKeyPressHandler

        '        n_DGVColKeyPressHandler.DisposeKeyPressHandler()

        '        AddHandler .KeyPress, AddressOf Free_KeyPress

        '    End With

        'End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape And isCellInEditMode = False Then

            Me.Close()

            Return True
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub ToolStripButton2_EnabledChanged(sender As Object, e As EventArgs) Handles ToolStripButton2.EnabledChanged
        dgvproducts.ReadOnly = Not ToolStripButton2.Enabled

    End Sub

    Private Async Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click

        If selected_rowindex = -1 Then Return

        Dim allowanceName = dgvproducts.Item("PartNo", selected_rowindex).Value.ToString

        Dim prompt As DialogResult

        If n_categname = ProductConstant.ALLOWANCE_TYPE_CATEGORY AndAlso
            Await _productDataService.CheckIfAlreadyUsedInAllowancesAsync(allowanceName) Then

            MessageBoxHelper.Warning("Cannot delete an allowance item that is already used in a generated payroll.")
            Return
            'prompt = MessageBox.Show("This allowance type is already used in generated payrolls. Are you sure you want to delete this item?",
            '                        "Delete item",
            '                        MessageBoxButtons.YesNoCancel,
            '                        MessageBoxIcon.Warning,
            '                        MessageBoxDefaultButton.Button2)
        Else

            prompt = MessageBox.Show("Are you sure do you want to delete this item?",
                                    "Delete item",
                                    MessageBoxButtons.YesNoCancel,
                                    MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button2)

        End If

        If prompt = Windows.Forms.DialogResult.Yes Then
            dgvproducts.Enabled = False
            Dim n_ExecuteQuery As _
            New ExecuteQuery("UPDATE product" &
                             " SET LastUpd=CURRENT_TIMESTAMP()" &
                             ",LastUpdBy='" & z_User & "'" &
                             ",ActiveData='0'" &
                             " WHERE RowID='" & dgvproducts.Item("RowID", selected_rowindex).Value & "';")

            Dim removing_row = dgvproducts.Rows(selected_rowindex)

            dgvproducts.Rows.Remove(removing_row)

            'ToolStripButton3_Click(sender, e)

        End If
        dgvproducts.Enabled = True

    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

End Class