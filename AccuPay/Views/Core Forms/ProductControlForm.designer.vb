<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProductControlForm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProductControlForm))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
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
        Me.Status = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Fixed = New System.Windows.Forms.DataGridViewCheckBoxColumn()
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
        Me.AllocateBelowSafetyFlag = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Strength = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UnitsBackordered = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UnitsBackorderAsOf = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateLastInventoryCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TaxVAT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.WithholdingTax = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.COAId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblforballoon = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmsBlank = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvproducts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
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
        Me.dgvproducts.AllowUserToResizeRows = False
        Me.dgvproducts.BackgroundColor = System.Drawing.Color.White
        Me.dgvproducts.ColumnHeadersHeight = 34
        Me.dgvproducts.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.RowID, Me.SupplierID, Me.ProdName, Me.Description, Me.PartNo, Me.Category, Me.CategoryID, Me.Status, Me.Fixed, Me.UnitPrice, Me.VATPercent, Me.FirstBillFlag, Me.SecondBillFlag, Me.ThirdBillFlag, Me.PDCFlag, Me.MonthlyBIllFlag, Me.PenaltyFlag, Me.WithholdingTaxPercent, Me.CostPrice, Me.UnitOfMeasure, Me.SKU, Me.LeadTime, Me.BarCode, Me.BusinessUnitID, Me.LastRcvdFromShipmentDate, Me.LastRcvdFromShipmentCount, Me.TotalShipmentCount, Me.BookPageNo, Me.BrandName, Me.LastPurchaseDate, Me.LastSoldDate, Me.LastSoldCount, Me.ReOrderPoint, Me.AllocateBelowSafetyFlag, Me.Strength, Me.UnitsBackordered, Me.UnitsBackorderAsOf, Me.DateLastInventoryCount, Me.TaxVAT, Me.WithholdingTax, Me.COAId})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvproducts.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvproducts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvproducts.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvproducts.Location = New System.Drawing.Point(0, 38)
        Me.dgvproducts.MultiSelect = False
        Me.dgvproducts.Name = "dgvproducts"
        Me.dgvproducts.RowHeadersWidth = 25
        Me.dgvproducts.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvproducts.Size = New System.Drawing.Size(701, 357)
        Me.dgvproducts.TabIndex = 0
        '
        'RowID
        '
        Me.RowID.HeaderText = "RowID"
        Me.RowID.Name = "RowID"
        Me.RowID.ReadOnly = True
        Me.RowID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.RowID.Visible = False
        '
        'SupplierID
        '
        Me.SupplierID.HeaderText = "SupplierID"
        Me.SupplierID.Name = "SupplierID"
        Me.SupplierID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.SupplierID.Visible = False
        '
        'ProdName
        '
        Me.ProdName.HeaderText = "ProdName"
        Me.ProdName.Name = "ProdName"
        Me.ProdName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ProdName.Visible = False
        '
        'Description
        '
        Me.Description.HeaderText = "Description"
        Me.Description.Name = "Description"
        Me.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Description.Visible = False
        '
        'PartNo
        '
        Me.PartNo.HeaderText = "PartNo"
        Me.PartNo.Name = "PartNo"
        Me.PartNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.PartNo.Width = 159
        '
        'Category
        '
        Me.Category.HeaderText = "Category"
        Me.Category.Name = "Category"
        Me.Category.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Category.Visible = False
        '
        'CategoryID
        '
        Me.CategoryID.HeaderText = "CategoryID"
        Me.CategoryID.Name = "CategoryID"
        Me.CategoryID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.CategoryID.Visible = False
        '
        'Status
        '
        Me.Status.HeaderText = "Status"
        Me.Status.Name = "Status"
        Me.Status.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Status.Width = 141
        '
        'Fixed
        '
        Me.Fixed.HeaderText = "Is Fixed"
        Me.Fixed.Name = "Fixed"
        Me.Fixed.Width = 142
        '
        'UnitPrice
        '
        Me.UnitPrice.HeaderText = "UnitPrice"
        Me.UnitPrice.Name = "UnitPrice"
        Me.UnitPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.UnitPrice.Visible = False
        '
        'VATPercent
        '
        Me.VATPercent.HeaderText = "VATPercent"
        Me.VATPercent.Name = "VATPercent"
        Me.VATPercent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.VATPercent.Visible = False
        '
        'FirstBillFlag
        '
        Me.FirstBillFlag.HeaderText = "FirstBillFlag"
        Me.FirstBillFlag.Name = "FirstBillFlag"
        Me.FirstBillFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.FirstBillFlag.Visible = False
        '
        'SecondBillFlag
        '
        Me.SecondBillFlag.HeaderText = "SecondBillFlag"
        Me.SecondBillFlag.Name = "SecondBillFlag"
        Me.SecondBillFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.SecondBillFlag.Visible = False
        '
        'ThirdBillFlag
        '
        Me.ThirdBillFlag.HeaderText = "ThirdBillFlag"
        Me.ThirdBillFlag.Name = "ThirdBillFlag"
        Me.ThirdBillFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ThirdBillFlag.Visible = False
        '
        'PDCFlag
        '
        Me.PDCFlag.HeaderText = "PDCFlag"
        Me.PDCFlag.Name = "PDCFlag"
        Me.PDCFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.PDCFlag.Visible = False
        '
        'MonthlyBIllFlag
        '
        Me.MonthlyBIllFlag.HeaderText = "MonthlyBIllFlag"
        Me.MonthlyBIllFlag.Name = "MonthlyBIllFlag"
        Me.MonthlyBIllFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.MonthlyBIllFlag.Visible = False
        '
        'PenaltyFlag
        '
        Me.PenaltyFlag.HeaderText = "PenaltyFlag"
        Me.PenaltyFlag.Name = "PenaltyFlag"
        Me.PenaltyFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.PenaltyFlag.Visible = False
        '
        'WithholdingTaxPercent
        '
        Me.WithholdingTaxPercent.HeaderText = "WithholdingTaxPercent"
        Me.WithholdingTaxPercent.Name = "WithholdingTaxPercent"
        Me.WithholdingTaxPercent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.WithholdingTaxPercent.Visible = False
        '
        'CostPrice
        '
        Me.CostPrice.HeaderText = "CostPrice"
        Me.CostPrice.Name = "CostPrice"
        Me.CostPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.CostPrice.Visible = False
        '
        'UnitOfMeasure
        '
        Me.UnitOfMeasure.HeaderText = "UnitOfMeasure"
        Me.UnitOfMeasure.Name = "UnitOfMeasure"
        Me.UnitOfMeasure.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.UnitOfMeasure.Visible = False
        '
        'SKU
        '
        Me.SKU.HeaderText = "SKU"
        Me.SKU.Name = "SKU"
        Me.SKU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.SKU.Visible = False
        '
        'LeadTime
        '
        Me.LeadTime.HeaderText = "LeadTime"
        Me.LeadTime.Name = "LeadTime"
        Me.LeadTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.LeadTime.Visible = False
        '
        'BarCode
        '
        Me.BarCode.HeaderText = "BarCode"
        Me.BarCode.Name = "BarCode"
        Me.BarCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.BarCode.Visible = False
        '
        'BusinessUnitID
        '
        Me.BusinessUnitID.HeaderText = "BusinessUnitID"
        Me.BusinessUnitID.Name = "BusinessUnitID"
        Me.BusinessUnitID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.BusinessUnitID.Visible = False
        '
        'LastRcvdFromShipmentDate
        '
        Me.LastRcvdFromShipmentDate.HeaderText = "LastRcvdFromShipmentDate"
        Me.LastRcvdFromShipmentDate.Name = "LastRcvdFromShipmentDate"
        Me.LastRcvdFromShipmentDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.LastRcvdFromShipmentDate.Visible = False
        '
        'LastRcvdFromShipmentCount
        '
        Me.LastRcvdFromShipmentCount.HeaderText = "LastRcvdFromShipmentCount"
        Me.LastRcvdFromShipmentCount.Name = "LastRcvdFromShipmentCount"
        Me.LastRcvdFromShipmentCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.LastRcvdFromShipmentCount.Visible = False
        '
        'TotalShipmentCount
        '
        Me.TotalShipmentCount.HeaderText = "TotalShipmentCount"
        Me.TotalShipmentCount.Name = "TotalShipmentCount"
        Me.TotalShipmentCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.TotalShipmentCount.Visible = False
        '
        'BookPageNo
        '
        Me.BookPageNo.HeaderText = "BookPageNo"
        Me.BookPageNo.Name = "BookPageNo"
        Me.BookPageNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.BookPageNo.Visible = False
        '
        'BrandName
        '
        Me.BrandName.HeaderText = "BrandName"
        Me.BrandName.Name = "BrandName"
        Me.BrandName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.BrandName.Visible = False
        '
        'LastPurchaseDate
        '
        Me.LastPurchaseDate.HeaderText = "LastPurchaseDate"
        Me.LastPurchaseDate.Name = "LastPurchaseDate"
        Me.LastPurchaseDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.LastPurchaseDate.Visible = False
        '
        'LastSoldDate
        '
        Me.LastSoldDate.HeaderText = "LastSoldDate"
        Me.LastSoldDate.Name = "LastSoldDate"
        Me.LastSoldDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.LastSoldDate.Visible = False
        '
        'LastSoldCount
        '
        Me.LastSoldCount.HeaderText = "LastSoldCount"
        Me.LastSoldCount.Name = "LastSoldCount"
        Me.LastSoldCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.LastSoldCount.Visible = False
        '
        'ReOrderPoint
        '
        Me.ReOrderPoint.HeaderText = "ReOrderPoint"
        Me.ReOrderPoint.Name = "ReOrderPoint"
        Me.ReOrderPoint.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ReOrderPoint.Visible = False
        '
        'AllocateBelowSafetyFlag
        '
        Me.AllocateBelowSafetyFlag.HeaderText = "Included in 13th month pay"
        Me.AllocateBelowSafetyFlag.Name = "AllocateBelowSafetyFlag"
        Me.AllocateBelowSafetyFlag.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AllocateBelowSafetyFlag.Width = 141
        '
        'Strength
        '
        Me.Strength.HeaderText = "Strength"
        Me.Strength.Name = "Strength"
        Me.Strength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Strength.Visible = False
        '
        'UnitsBackordered
        '
        Me.UnitsBackordered.HeaderText = "UnitsBackordered"
        Me.UnitsBackordered.Name = "UnitsBackordered"
        Me.UnitsBackordered.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.UnitsBackordered.Visible = False
        '
        'UnitsBackorderAsOf
        '
        Me.UnitsBackorderAsOf.HeaderText = "UnitsBackorderAsOf"
        Me.UnitsBackorderAsOf.Name = "UnitsBackorderAsOf"
        Me.UnitsBackorderAsOf.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.UnitsBackorderAsOf.Visible = False
        '
        'DateLastInventoryCount
        '
        Me.DateLastInventoryCount.HeaderText = "DateLastInventoryCount"
        Me.DateLastInventoryCount.Name = "DateLastInventoryCount"
        Me.DateLastInventoryCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DateLastInventoryCount.Visible = False
        '
        'TaxVAT
        '
        Me.TaxVAT.HeaderText = "TaxVAT"
        Me.TaxVAT.Name = "TaxVAT"
        Me.TaxVAT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.TaxVAT.Visible = False
        '
        'WithholdingTax
        '
        Me.WithholdingTax.HeaderText = "WithholdingTax"
        Me.WithholdingTax.Name = "WithholdingTax"
        Me.WithholdingTax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.WithholdingTax.Visible = False
        '
        'COAId
        '
        Me.COAId.HeaderText = "COAId"
        Me.COAId.Name = "COAId"
        Me.COAId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
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
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(108, 26)
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Label1"
        Me.Label1.Visible = False
        '
        'cmsBlank
        '
        Me.cmsBlank.Name = "cmsBlank"
        Me.cmsBlank.Size = New System.Drawing.Size(61, 4)
        '
        'ProductControlForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(701, 395)
        Me.Controls.Add(Me.dgvproducts)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.lblforballoon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ProductControlForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvproducts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
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
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmsBlank As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RowID As DataGridViewTextBoxColumn
    Friend WithEvents SupplierID As DataGridViewTextBoxColumn
    Friend WithEvents ProdName As DataGridViewTextBoxColumn
    Friend WithEvents Description As DataGridViewTextBoxColumn
    Friend WithEvents PartNo As DataGridViewTextBoxColumn
    Friend WithEvents Category As DataGridViewTextBoxColumn
    Friend WithEvents CategoryID As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewCheckBoxColumn
    Friend WithEvents Fixed As DataGridViewCheckBoxColumn
    Friend WithEvents UnitPrice As DataGridViewTextBoxColumn
    Friend WithEvents VATPercent As DataGridViewTextBoxColumn
    Friend WithEvents FirstBillFlag As DataGridViewTextBoxColumn
    Friend WithEvents SecondBillFlag As DataGridViewTextBoxColumn
    Friend WithEvents ThirdBillFlag As DataGridViewTextBoxColumn
    Friend WithEvents PDCFlag As DataGridViewTextBoxColumn
    Friend WithEvents MonthlyBIllFlag As DataGridViewTextBoxColumn
    Friend WithEvents PenaltyFlag As DataGridViewTextBoxColumn
    Friend WithEvents WithholdingTaxPercent As DataGridViewTextBoxColumn
    Friend WithEvents CostPrice As DataGridViewTextBoxColumn
    Friend WithEvents UnitOfMeasure As DataGridViewTextBoxColumn
    Friend WithEvents SKU As DataGridViewTextBoxColumn
    Friend WithEvents LeadTime As DataGridViewTextBoxColumn
    Friend WithEvents BarCode As DataGridViewTextBoxColumn
    Friend WithEvents BusinessUnitID As DataGridViewTextBoxColumn
    Friend WithEvents LastRcvdFromShipmentDate As DataGridViewTextBoxColumn
    Friend WithEvents LastRcvdFromShipmentCount As DataGridViewTextBoxColumn
    Friend WithEvents TotalShipmentCount As DataGridViewTextBoxColumn
    Friend WithEvents BookPageNo As DataGridViewTextBoxColumn
    Friend WithEvents BrandName As DataGridViewTextBoxColumn
    Friend WithEvents LastPurchaseDate As DataGridViewTextBoxColumn
    Friend WithEvents LastSoldDate As DataGridViewTextBoxColumn
    Friend WithEvents LastSoldCount As DataGridViewTextBoxColumn
    Friend WithEvents ReOrderPoint As DataGridViewTextBoxColumn
    Friend WithEvents AllocateBelowSafetyFlag As DataGridViewCheckBoxColumn
    Friend WithEvents Strength As DataGridViewTextBoxColumn
    Friend WithEvents UnitsBackordered As DataGridViewTextBoxColumn
    Friend WithEvents UnitsBackorderAsOf As DataGridViewTextBoxColumn
    Friend WithEvents DateLastInventoryCount As DataGridViewTextBoxColumn
    Friend WithEvents TaxVAT As DataGridViewTextBoxColumn
    Friend WithEvents WithholdingTax As DataGridViewTextBoxColumn
    Friend WithEvents COAId As DataGridViewTextBoxColumn
End Class
