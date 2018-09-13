Imports MySql.Data.MySqlClient
Imports System.IO

Public Class ProdCtrlForm

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

            Dim haschangestoDB = _
                EXECQUER("SELECT EXISTS(SELECT" & _
                         " RowID" & _
                         " FROM product" & _
                         " WHERE OrganizationID='" & orgztnID & "'" & _
                         " AND `Category`='" & n_categname & "'" & _
                         " AND (DATE_FORMAT(Created, '%Y-%m-%d') = CURDATE() OR DATE_FORMAT(LastUpd, '%Y-%m-%d') = CURDATE())" & _
                         " LIMIT 1);")

            If haschangestoDB = 1 Then
                Me.DialogResult = Windows.Forms.DialogResult.OK

            Else
                Me.DialogResult = Windows.Forms.DialogResult.Cancel

            End If

        Else
            e.Cancel = True

        End If

    End Sub

    Private Sub ProductControlForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ToolStripButton3_Click(sender, e)

    End Sub

    Function INS_product(Optional prod_rowID As Object = Nothing, _
                         Optional p_Name As Object = Nothing, _
                         Optional p_PartNo As Object = Nothing, _
                         Optional p_CategName As Object = Nothing, _
                         Optional p_Status As Object = "Active") As Object

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
                .Parameters.AddWithValue("p_CategoryID", DBNull.Value) 'KELANGAN MA-RETRIEVE KO UNG ROWID SA CATEGORY WHERE CATEGORYNAME = 'MEDICAL RECORD'
                .Parameters.AddWithValue("p_Status", p_Status)
                .Parameters.AddWithValue("p_UnitPrice", 0.0)
                .Parameters.AddWithValue("p_UnitOfMeasure", 0)
                .Parameters.AddWithValue("p_IsFixed", 0)
                .Parameters.AddWithValue("p_IsIncludedIn13th", 0)
                .Parameters("prod_RowID").Direction = ParameterDirection.ReturnValue

                Dim datrd As MySqlDataReader

                datrd = .ExecuteReader()

                returnvalue = datrd(0)

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


        For Each drow As DataGridViewRow In dgvproducts.Rows

            If drow.IsNewRow = False Then

                Dim datastatus = String.Empty

                If drow.Cells("Status").Value = "Yes" Then
                    datastatus = "1"
                Else
                    datastatus = "0"
                End If

                Dim returnval = _
                INS_product(drow.Cells("RowID").Value,
                            drow.Cells("PartNo").Value,
                            drow.Cells("PartNo").Value,
                            n_categname,
                            datastatus)

                If drow.Cells("RowID").Value = Nothing Then
                    drow.Cells("RowID").Value = returnval
                End If

            Else

                Continue For

            End If

        Next

        ToolStripButton2.Enabled = True

        If ToolStripButton2.Enabled Then
            InfoBalloon("Changes made were successfully savedS.", "Successfully saved", lblforballoon, 0, -69)
        End If

    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Status.Items.Add("Yes")

            Status.Items.Add("No")

        End If
        
        Dim selectAllProduct As New DataTable

        selectAllProduct = retAsDatTbl("SELECT p.*, IF(p.`Status` = '0', 'No', 'Yes') AS IStatus" & _
                                       " FROM product p" & _
                                       " INNER JOIN category c ON c.OrganizationID='" & orgztnID & "' AND CategoryName='" & n_categname & "'" & _
                                       " WHERE p.OrganizationID='" & orgztnID & "'" & _
                                       " AND p.CategoryID=c.RowID;")

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
                                 drow("IStatus"))

            'RowID,SupplierID,ProdName,Description,PartNo,Category,CategoryID
            ',Status,UnitPrice,VATPercent,FirstBillFlag,SecondBillFlag,ThirdBillFlag
            ',PDCFlag,MonthlyBIllFlag,PenaltyFlag,WithholdingTaxPercent,CostPrice,UnitOfMeasure
            ',SKU,LeadTime,BarCode,BusinessUnitID,LastRcvdFromShipmentDate,LastRcvdFromShipmentCount
            ',TotalShipmentCount,BookPageNo,BrandName,LastPurchaseDate,LastSoldDate,LastSoldCount,ReOrderPoint
            ',AllocateBelowSafetyFlag,Strength,UnitsBackordered,UnitsBackorderAsOf,DateLastInventoryCount,TaxVAT,WithholdingTax,COAId,

        Next

        selectAllProduct.Dispose()

    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click

    End Sub

    Dim isCellInEditMode = False

    Private Sub dgvpayper_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvproducts.CellBeginEdit
        isCellInEditMode = True
    End Sub

    Private Sub dgvpayper_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvproducts.CellContentClick

    End Sub

    Private Sub dgvpayper_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvproducts.CellEndEdit
        isCellInEditMode = False
    End Sub

    Private Sub dgvpayper_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvproducts.EditingControlShowing

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

End Class