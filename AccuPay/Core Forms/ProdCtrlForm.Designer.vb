<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProdCtrlForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProdCtrlForm))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.dgvproducts = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SupplierID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProdName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PartNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Category = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CategoryID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.UnitPrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.VATPercent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstBillFlag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SecondBillFlag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ThirdBillFlag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PDCFlag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MonthlyBIllFlag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PenaltyFlag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.WithholdingTaxPercent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CostPrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UnitOfMeasure = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SKU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LeadTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BarCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BusinessUnitID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastRcvdFromShipmentDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastRcvdFromShipmentCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalShipmentCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BookPageNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BrandName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastPurchaseDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastSoldDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastSoldCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReOrderPoint = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AllocateBelowSafetyFlag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Strength = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UnitsBackordered = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UnitsBackorderAsOf = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateLastInventoryCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TaxVAT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.WithholdingTax = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.COAId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblforballoon = New System.Windows.Forms.Label()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvproducts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripButton3, Me.ToolStripButton4})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(701, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(51, 22)
        Me.ToolStripButton1.Text = "&New"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(51, 22)
        Me.ToolStripButton2.Text = "&Save"
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.Image = CType(resources.GetObject("ToolStripButton3.Image"), System.Drawing.Image)
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(63, 22)
        Me.ToolStripButton3.Text = "Cancel"
        '
        'ToolStripButton4
        '
        Me.ToolStripButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton4.Image = CType(resources.GetObject("ToolStripButton4.Image"), System.Drawing.Image)
        Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton4.Name = "ToolStripButton4"
        Me.ToolStripButton4.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton4.Text = "Close"
        Me.ToolStripButton4.Visible = False
        '
        'dgvproducts
        '
        Me.dgvproducts.AllowUserToDeleteRows = False
        Me.dgvproducts.AllowUserToOrderColumns = True
        Me.dgvproducts.BackgroundColor = System.Drawing.Color.White
        Me.dgvproducts.ColumnHeadersHeight = 34
        Me.dgvproducts.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.RowID, Me.SupplierID, Me.ProdName, Me.Description, Me.PartNo, Me.Category, Me.CategoryID, Me.Status, Me.UnitPrice, Me.VATPercent, Me.FirstBillFlag, Me.SecondBillFlag, Me.ThirdBillFlag, Me.PDCFlag, Me.MonthlyBIllFlag, Me.PenaltyFlag, Me.WithholdingTaxPercent, Me.CostPrice, Me.UnitOfMeasure, Me.SKU, Me.LeadTime, Me.BarCode, Me.BusinessUnitID, Me.LastRcvdFromShipmentDate, Me.LastRcvdFromShipmentCount, Me.TotalShipmentCount, Me.BookPageNo, Me.BrandName, Me.LastPurchaseDate, Me.LastSoldDate, Me.LastSoldCount, Me.ReOrderPoint, Me.AllocateBelowSafetyFlag, Me.Strength, Me.UnitsBackordered, Me.UnitsBackorderAsOf, Me.DateLastInventoryCount, Me.TaxVAT, Me.WithholdingTax, Me.COAId})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvproducts.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvproducts.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvproducts.Location = New System.Drawing.Point(12, 75)
        Me.dgvproducts.MultiSelect = False
        Me.dgvproducts.Name = "dgvproducts"
        Me.dgvproducts.Size = New System.Drawing.Size(677, 308)
        Me.dgvproducts.TabIndex = 0
        '
        'RowID
        '
        Me.RowID.HeaderText = "RowID"
        Me.RowID.Name = "RowID"
        Me.RowID.ReadOnly = True
        Me.RowID.Visible = False
        '
        'SupplierID
        '
        Me.SupplierID.HeaderText = "SupplierID"
        Me.SupplierID.Name = "SupplierID"
        Me.SupplierID.Visible = False
        '
        'ProdName
        '
        Me.ProdName.HeaderText = "ProdName"
        Me.ProdName.Name = "ProdName"
        Me.ProdName.Visible = False
        '
        'Description
        '
        Me.Description.HeaderText = "Description"
        Me.Description.Name = "Description"
        Me.Description.Visible = False
        '
        'PartNo
        '
        Me.PartNo.HeaderText = "PartNo"
        Me.PartNo.Name = "PartNo"
        Me.PartNo.Width = 443
        '
        'Category
        '
        Me.Category.HeaderText = "Category"
        Me.Category.Name = "Category"
        Me.Category.Visible = False
        '
        'CategoryID
        '
        Me.CategoryID.HeaderText = "CategoryID"
        Me.CategoryID.Name = "CategoryID"
        Me.CategoryID.Visible = False
        '
        'Status
        '
        Me.Status.HeaderText = "Status"
        Me.Status.Name = "Status"
        Me.Status.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Status.Width = 190
        '
        'UnitPrice
        '
        Me.UnitPrice.HeaderText = "UnitPrice"
        Me.UnitPrice.Name = "UnitPrice"
        Me.UnitPrice.Visible = False
        '
        'VATPercent
        '
        Me.VATPercent.HeaderText = "VATPercent"
        Me.VATPercent.Name = "VATPercent"
        Me.VATPercent.Visible = False
        '
        'FirstBillFlag
        '
        Me.FirstBillFlag.HeaderText = "FirstBillFlag"
        Me.FirstBillFlag.Name = "FirstBillFlag"
        Me.FirstBillFlag.Visible = False
        '
        'SecondBillFlag
        '
        Me.SecondBillFlag.HeaderText = "SecondBillFlag"
        Me.SecondBillFlag.Name = "SecondBillFlag"
        Me.SecondBillFlag.Visible = False
        '
        'ThirdBillFlag
        '
        Me.ThirdBillFlag.HeaderText = "ThirdBillFlag"
        Me.ThirdBillFlag.Name = "ThirdBillFlag"
        Me.ThirdBillFlag.Visible = False
        '
        'PDCFlag
        '
        Me.PDCFlag.HeaderText = "PDCFlag"
        Me.PDCFlag.Name = "PDCFlag"
        Me.PDCFlag.Visible = False
        '
        'MonthlyBIllFlag
        '
        Me.MonthlyBIllFlag.HeaderText = "MonthlyBIllFlag"
        Me.MonthlyBIllFlag.Name = "MonthlyBIllFlag"
        Me.MonthlyBIllFlag.Visible = False
        '
        'PenaltyFlag
        '
        Me.PenaltyFlag.HeaderText = "PenaltyFlag"
        Me.PenaltyFlag.Name = "PenaltyFlag"
        Me.PenaltyFlag.Visible = False
        '
        'WithholdingTaxPercent
        '
        Me.WithholdingTaxPercent.HeaderText = "WithholdingTaxPercent"
        Me.WithholdingTaxPercent.Name = "WithholdingTaxPercent"
        Me.WithholdingTaxPercent.Visible = False
        '
        'CostPrice
        '
        Me.CostPrice.HeaderText = "CostPrice"
        Me.CostPrice.Name = "CostPrice"
        Me.CostPrice.Visible = False
        '
        'UnitOfMeasure
        '
        Me.UnitOfMeasure.HeaderText = "UnitOfMeasure"
        Me.UnitOfMeasure.Name = "UnitOfMeasure"
        Me.UnitOfMeasure.Visible = False
        '
        'SKU
        '
        Me.SKU.HeaderText = "SKU"
        Me.SKU.Name = "SKU"
        Me.SKU.Visible = False
        '
        'LeadTime
        '
        Me.LeadTime.HeaderText = "LeadTime"
        Me.LeadTime.Name = "LeadTime"
        Me.LeadTime.Visible = False
        '
        'BarCode
        '
        Me.BarCode.HeaderText = "BarCode"
        Me.BarCode.Name = "BarCode"
        Me.BarCode.Visible = False
        '
        'BusinessUnitID
        '
        Me.BusinessUnitID.HeaderText = "BusinessUnitID"
        Me.BusinessUnitID.Name = "BusinessUnitID"
        Me.BusinessUnitID.Visible = False
        '
        'LastRcvdFromShipmentDate
        '
        Me.LastRcvdFromShipmentDate.HeaderText = "LastRcvdFromShipmentDate"
        Me.LastRcvdFromShipmentDate.Name = "LastRcvdFromShipmentDate"
        Me.LastRcvdFromShipmentDate.Visible = False
        '
        'LastRcvdFromShipmentCount
        '
        Me.LastRcvdFromShipmentCount.HeaderText = "LastRcvdFromShipmentCount"
        Me.LastRcvdFromShipmentCount.Name = "LastRcvdFromShipmentCount"
        Me.LastRcvdFromShipmentCount.Visible = False
        '
        'TotalShipmentCount
        '
        Me.TotalShipmentCount.HeaderText = "TotalShipmentCount"
        Me.TotalShipmentCount.Name = "TotalShipmentCount"
        Me.TotalShipmentCount.Visible = False
        '
        'BookPageNo
        '
        Me.BookPageNo.HeaderText = "BookPageNo"
        Me.BookPageNo.Name = "BookPageNo"
        Me.BookPageNo.Visible = False
        '
        'BrandName
        '
        Me.BrandName.HeaderText = "BrandName"
        Me.BrandName.Name = "BrandName"
        Me.BrandName.Visible = False
        '
        'LastPurchaseDate
        '
        Me.LastPurchaseDate.HeaderText = "LastPurchaseDate"
        Me.LastPurchaseDate.Name = "LastPurchaseDate"
        Me.LastPurchaseDate.Visible = False
        '
        'LastSoldDate
        '
        Me.LastSoldDate.HeaderText = "LastSoldDate"
        Me.LastSoldDate.Name = "LastSoldDate"
        Me.LastSoldDate.Visible = False
        '
        'LastSoldCount
        '
        Me.LastSoldCount.HeaderText = "LastSoldCount"
        Me.LastSoldCount.Name = "LastSoldCount"
        Me.LastSoldCount.Visible = False
        '
        'ReOrderPoint
        '
        Me.ReOrderPoint.HeaderText = "ReOrderPoint"
        Me.ReOrderPoint.Name = "ReOrderPoint"
        Me.ReOrderPoint.Visible = False
        '
        'AllocateBelowSafetyFlag
        '
        Me.AllocateBelowSafetyFlag.HeaderText = "AllocateBelowSafetyFlag"
        Me.AllocateBelowSafetyFlag.Name = "AllocateBelowSafetyFlag"
        Me.AllocateBelowSafetyFlag.Visible = False
        '
        'Strength
        '
        Me.Strength.HeaderText = "Strength"
        Me.Strength.Name = "Strength"
        Me.Strength.Visible = False
        '
        'UnitsBackordered
        '
        Me.UnitsBackordered.HeaderText = "UnitsBackordered"
        Me.UnitsBackordered.Name = "UnitsBackordered"
        Me.UnitsBackordered.Visible = False
        '
        'UnitsBackorderAsOf
        '
        Me.UnitsBackorderAsOf.HeaderText = "UnitsBackorderAsOf"
        Me.UnitsBackorderAsOf.Name = "UnitsBackorderAsOf"
        Me.UnitsBackorderAsOf.Visible = False
        '
        'DateLastInventoryCount
        '
        Me.DateLastInventoryCount.HeaderText = "DateLastInventoryCount"
        Me.DateLastInventoryCount.Name = "DateLastInventoryCount"
        Me.DateLastInventoryCount.Visible = False
        '
        'TaxVAT
        '
        Me.TaxVAT.HeaderText = "TaxVAT"
        Me.TaxVAT.Name = "TaxVAT"
        Me.TaxVAT.Visible = False
        '
        'WithholdingTax
        '
        Me.WithholdingTax.HeaderText = "WithholdingTax"
        Me.WithholdingTax.Name = "WithholdingTax"
        Me.WithholdingTax.Visible = False
        '
        'COAId
        '
        Me.COAId.HeaderText = "COAId"
        Me.COAId.Name = "COAId"
        Me.COAId.Visible = False
        '
        'lblforballoon
        '
        Me.lblforballoon.AutoSize = True
        Me.lblforballoon.Location = New System.Drawing.Point(64, 11)
        Me.lblforballoon.Name = "lblforballoon"
        Me.lblforballoon.Size = New System.Drawing.Size(39, 13)
        Me.lblforballoon.TabIndex = 1
        Me.lblforballoon.Text = "Label1"
        '
        'ProdCtrlForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(701, 395)
        Me.Controls.Add(Me.dgvproducts)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.lblforballoon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ProdCtrlForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvproducts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvproducts As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents lblforballoon As System.Windows.Forms.Label
    Friend WithEvents RowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SupplierID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ProdName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Description As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PartNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Category As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CategoryID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Status As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents UnitPrice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents VATPercent As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FirstBillFlag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SecondBillFlag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ThirdBillFlag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PDCFlag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MonthlyBIllFlag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PenaltyFlag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents WithholdingTaxPercent As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostPrice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UnitOfMeasure As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SKU As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LeadTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BarCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BusinessUnitID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LastRcvdFromShipmentDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LastRcvdFromShipmentCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TotalShipmentCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BookPageNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BrandName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LastPurchaseDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LastSoldDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LastSoldCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ReOrderPoint As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AllocateBelowSafetyFlag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Strength As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UnitsBackordered As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UnitsBackorderAsOf As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DateLastInventoryCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TaxVAT As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents WithholdingTax As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents COAId As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
