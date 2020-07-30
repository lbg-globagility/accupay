<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RoutePayRateMatrixForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RoutePayRateMatrixForm))
        Me.dgvTripTicketHelpers = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.dgvRoutePayRateMatrix = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.TripTicketMenubar = New System.Windows.Forms.ToolStrip()
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
        Me.colRoutePayRateRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRoutePayRatesPosition = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRoutePayRatesRate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvTripTicketHelpers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvRoutePayRateMatrix, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TripTicketMenubar.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvTripTicketHelpers
        '
        Me.dgvTripTicketHelpers.AllowUserToAddRows = False
        Me.dgvTripTicketHelpers.AllowUserToDeleteRows = False
        Me.dgvTripTicketHelpers.AllowUserToOrderColumns = True
        Me.dgvTripTicketHelpers.AllowUserToResizeRows = False
        Me.dgvTripTicketHelpers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvTripTicketHelpers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTripTicketHelpers.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvTripTicketHelpers.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvTripTicketHelpers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTripTicketHelpers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colRoutePayRateRowID, Me.colRoutePayRatesPosition, Me.colRoutePayRatesRate})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvTripTicketHelpers.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvTripTicketHelpers.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvTripTicketHelpers.Location = New System.Drawing.Point(554, 71)
        Me.dgvTripTicketHelpers.MultiSelect = False
        Me.dgvTripTicketHelpers.Name = "dgvTripTicketHelpers"
        Me.dgvTripTicketHelpers.ReadOnly = True
        Me.dgvTripTicketHelpers.RowHeadersWidth = 25
        Me.dgvTripTicketHelpers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvTripTicketHelpers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTripTicketHelpers.Size = New System.Drawing.Size(306, 475)
        Me.dgvTripTicketHelpers.TabIndex = 103
        '
        'dgvRoutePayRateMatrix
        '
        Me.dgvRoutePayRateMatrix.AllowUserToAddRows = False
        Me.dgvRoutePayRateMatrix.AllowUserToDeleteRows = False
        Me.dgvRoutePayRateMatrix.AllowUserToOrderColumns = True
        Me.dgvRoutePayRateMatrix.AllowUserToResizeRows = False
        Me.dgvRoutePayRateMatrix.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvRoutePayRateMatrix.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvRoutePayRateMatrix.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvRoutePayRateMatrix.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvRoutePayRateMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvRoutePayRateMatrix.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgvRoutePayRateMatrix.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvRoutePayRateMatrix.Location = New System.Drawing.Point(31, 124)
        Me.dgvRoutePayRateMatrix.MultiSelect = False
        Me.dgvRoutePayRateMatrix.Name = "dgvRoutePayRateMatrix"
        Me.dgvRoutePayRateMatrix.RowHeadersWidth = 25
        Me.dgvRoutePayRateMatrix.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvRoutePayRateMatrix.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvRoutePayRateMatrix.Size = New System.Drawing.Size(393, 354)
        Me.dgvRoutePayRateMatrix.TabIndex = 103
        '
        'TripTicketMenubar
        '
        Me.TripTicketMenubar.BackColor = System.Drawing.Color.Transparent
        Me.TripTicketMenubar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.TripTicketMenubar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnSave, Me.btnCancel, Me.tsbtnClose, Me.tsbtnAudittrail, Me.tsbtnImportEmployee, Me.tsprogbarempimport})
        Me.TripTicketMenubar.Location = New System.Drawing.Point(0, 0)
        Me.TripTicketMenubar.Name = "TripTicketMenubar"
        Me.TripTicketMenubar.Size = New System.Drawing.Size(882, 25)
        Me.TripTicketMenubar.TabIndex = 104
        Me.TripTicketMenubar.Text = "ToolStrip1"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(51, 22)
        Me.btnSave.Text = "&Save"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "Cancel"
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnClose.Name = "tsbtnClose"
        Me.tsbtnClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnClose.Text = "Close"
        '
        'tsbtnAudittrail
        '
        Me.tsbtnAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnAudittrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnAudittrail.Image = Global.AccuPay.My.Resources.Resources.audit_trail_icon
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
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 190
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Position"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 189
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Rate"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 190
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 366
        '
        'colRoutePayRateRowID
        '
        Me.colRoutePayRateRowID.HeaderText = "RowID"
        Me.colRoutePayRateRowID.Name = "colRoutePayRateRowID"
        Me.colRoutePayRateRowID.ReadOnly = True
        '
        'colRoutePayRatesPosition
        '
        Me.colRoutePayRatesPosition.HeaderText = "Position"
        Me.colRoutePayRatesPosition.Name = "colRoutePayRatesPosition"
        Me.colRoutePayRatesPosition.ReadOnly = True
        '
        'colRoutePayRatesRate
        '
        Me.colRoutePayRatesRate.HeaderText = "Rate"
        Me.colRoutePayRatesRate.Name = "colRoutePayRatesRate"
        Me.colRoutePayRatesRate.ReadOnly = True
        '
        'RoutePayRateMatrixForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(882, 582)
        Me.Controls.Add(Me.TripTicketMenubar)
        Me.Controls.Add(Me.dgvRoutePayRateMatrix)
        Me.Controls.Add(Me.dgvTripTicketHelpers)
        Me.Name = "RoutePayRateMatrixForm"
        Me.Text = "RoutePayRateMatrixForm"
        CType(Me.dgvTripTicketHelpers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvRoutePayRateMatrix, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TripTicketMenubar.ResumeLayout(False)
        Me.TripTicketMenubar.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvTripTicketHelpers As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents colRoutePayRateRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRoutePayRatesPosition As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRoutePayRatesRate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvRoutePayRateMatrix As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents TripTicketMenubar As System.Windows.Forms.ToolStrip
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnImportEmployee As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsprogbarempimport As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
