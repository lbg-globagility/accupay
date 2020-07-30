<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VehicleForm
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VehicleForm))
        Me.dgvVehicles = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.colVehiclesRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colVehiclesBodyNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colVehiclesPlateNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colVehiclesTruckType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colVehiclesRemove = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnClose = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnImportEmployee = New System.Windows.Forms.ToolStripButton()
        Me.tsprogbarempimport = New System.Windows.Forms.ToolStripProgressBar()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvVehicles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvVehicles
        '
        Me.dgvVehicles.AllowUserToDeleteRows = False
        Me.dgvVehicles.AllowUserToOrderColumns = True
        Me.dgvVehicles.AllowUserToResizeRows = False
        Me.dgvVehicles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvVehicles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvVehicles.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvVehicles.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgvVehicles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvVehicles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colVehiclesRowID, Me.colVehiclesBodyNo, Me.colVehiclesPlateNo, Me.colVehiclesTruckType, Me.colVehiclesRemove})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvVehicles.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgvVehicles.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvVehicles.Location = New System.Drawing.Point(12, 127)
        Me.dgvVehicles.MultiSelect = False
        Me.dgvVehicles.Name = "dgvVehicles"
        Me.dgvVehicles.RowHeadersWidth = 25
        Me.dgvVehicles.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvVehicles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvVehicles.Size = New System.Drawing.Size(848, 312)
        Me.dgvVehicles.TabIndex = 103
        '
        'colVehiclesRowID
        '
        Me.colVehiclesRowID.DataPropertyName = "RowID"
        Me.colVehiclesRowID.HeaderText = "RowID"
        Me.colVehiclesRowID.Name = "colVehiclesRowID"
        '
        'colVehiclesBodyNo
        '
        Me.colVehiclesBodyNo.DataPropertyName = "BodyNo"
        Me.colVehiclesBodyNo.HeaderText = "Body No"
        Me.colVehiclesBodyNo.Name = "colVehiclesBodyNo"
        '
        'colVehiclesPlateNo
        '
        Me.colVehiclesPlateNo.DataPropertyName = "PlateNo"
        Me.colVehiclesPlateNo.HeaderText = "Plate No"
        Me.colVehiclesPlateNo.Name = "colVehiclesPlateNo"
        '
        'colVehiclesTruckType
        '
        Me.colVehiclesTruckType.DataPropertyName = "TruckType"
        Me.colVehiclesTruckType.HeaderText = "Truck Type"
        Me.colVehiclesTruckType.Name = "colVehiclesTruckType"
        '
        'colVehiclesRemove
        '
        Me.colVehiclesRemove.HeaderText = ""
        Me.colVehiclesRemove.Name = "colVehiclesRemove"
        Me.colVehiclesRemove.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colVehiclesRemove.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.colVehiclesRemove.Text = "Remove"
        Me.colVehiclesRemove.UseColumnTextForButtonValue = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnSave, Me.btnCancel, Me.tsbtnClose, Me.tsbtnAudittrail, Me.tsbtnImportEmployee, Me.tsprogbarempimport})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(872, 25)
        Me.ToolStrip1.TabIndex = 104
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.Payroll.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(51, 22)
        Me.btnSave.Text = "&Save"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.Payroll.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "Cancel"
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnClose.Image = Global.Payroll.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnClose.Name = "tsbtnClose"
        Me.tsbtnClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnClose.Text = "Close"
        '
        'tsbtnAudittrail
        '
        Me.tsbtnAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnAudittrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnAudittrail.Image = Global.Payroll.My.Resources.Resources.audit_trail_icon
        Me.tsbtnAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnAudittrail.Name = "tsbtnAudittrail"
        Me.tsbtnAudittrail.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnAudittrail.Text = "ToolStripButton1"
        Me.tsbtnAudittrail.ToolTipText = "Show audit trails"
        '
        'tsbtnImportEmployee
        '
        Me.tsbtnImportEmployee.Image = CType(resources.GetObject("tsbtnImportEmployee.Image"), System.Drawing.Image)
        Me.tsbtnImportEmployee.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnImportEmployee.Name = "tsbtnImportEmployee"
        Me.tsbtnImportEmployee.Size = New System.Drawing.Size(118, 22)
        Me.tsbtnImportEmployee.Text = "Import Employee"
        '
        'tsprogbarempimport
        '
        Me.tsprogbarempimport.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsprogbarempimport.Name = "tsprogbarempimport"
        Me.tsprogbarempimport.Size = New System.Drawing.Size(100, 22)
        Me.tsprogbarempimport.Visible = False
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "RowID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 164
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "BodyNo"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Body No"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 164
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "PlateNo"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Plate No"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 165
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "TruckType"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Truck Type"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 164
        '
        'VehicleForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(872, 455)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.dgvVehicles)
        Me.Name = "VehicleForm"
        Me.Text = "VehicleForm"
        CType(Me.dgvVehicles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvVehicles As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnImportEmployee As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsprogbarempimport As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents colVehiclesRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colVehiclesBodyNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colVehiclesPlateNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colVehiclesTruckType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colVehiclesRemove As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
