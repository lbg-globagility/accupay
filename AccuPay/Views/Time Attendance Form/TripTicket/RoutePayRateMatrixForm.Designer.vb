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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RoutePayRateMatrixForm))
        Me.RouteRatesDataGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.colRoutePayRatesPosition = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRoutePayRatesRate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RouteDataGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TripTicketMenubar = New System.Windows.Forms.ToolStrip()
        Me.NewButton = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.RouteRatesDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RouteDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TripTicketMenubar.SuspendLayout()
        Me.SuspendLayout()
        '
        'RouteRatesDataGridView
        '
        Me.RouteRatesDataGridView.AllowUserToAddRows = False
        Me.RouteRatesDataGridView.AllowUserToDeleteRows = False
        Me.RouteRatesDataGridView.AllowUserToOrderColumns = True
        Me.RouteRatesDataGridView.AllowUserToResizeRows = False
        Me.RouteRatesDataGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RouteRatesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.RouteRatesDataGridView.BackgroundColor = System.Drawing.Color.White
        Me.RouteRatesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.RouteRatesDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colRoutePayRatesPosition, Me.colRoutePayRatesRate})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.RouteRatesDataGridView.DefaultCellStyle = DataGridViewCellStyle1
        Me.RouteRatesDataGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.RouteRatesDataGridView.Location = New System.Drawing.Point(248, 64)
        Me.RouteRatesDataGridView.MultiSelect = False
        Me.RouteRatesDataGridView.Name = "RouteRatesDataGridView"
        Me.RouteRatesDataGridView.RowHeadersWidth = 25
        Me.RouteRatesDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.RouteRatesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.RouteRatesDataGridView.Size = New System.Drawing.Size(514, 352)
        Me.RouteRatesDataGridView.TabIndex = 103
        '
        'colRoutePayRatesPosition
        '
        Me.colRoutePayRatesPosition.DataPropertyName = "PositionName"
        Me.colRoutePayRatesPosition.HeaderText = "Position"
        Me.colRoutePayRatesPosition.Name = "colRoutePayRatesPosition"
        Me.colRoutePayRatesPosition.ReadOnly = True
        '
        'colRoutePayRatesRate
        '
        Me.colRoutePayRatesRate.DataPropertyName = "Rate"
        Me.colRoutePayRatesRate.HeaderText = "Rate"
        Me.colRoutePayRatesRate.Name = "colRoutePayRatesRate"
        '
        'RouteDataGridView
        '
        Me.RouteDataGridView.AllowUserToAddRows = False
        Me.RouteDataGridView.AllowUserToDeleteRows = False
        Me.RouteDataGridView.AllowUserToOrderColumns = True
        Me.RouteDataGridView.AllowUserToResizeRows = False
        Me.RouteDataGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RouteDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.RouteDataGridView.BackgroundColor = System.Drawing.Color.White
        Me.RouteDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.RouteDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.RouteDataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.RouteDataGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.RouteDataGridView.Location = New System.Drawing.Point(8, 28)
        Me.RouteDataGridView.MultiSelect = False
        Me.RouteDataGridView.Name = "RouteDataGridView"
        Me.RouteDataGridView.ReadOnly = True
        Me.RouteDataGridView.RowHeadersWidth = 25
        Me.RouteDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.RouteDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.RouteDataGridView.Size = New System.Drawing.Size(232, 388)
        Me.RouteDataGridView.TabIndex = 103
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "Description"
        Me.Column1.HeaderText = "Route"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'TripTicketMenubar
        '
        Me.TripTicketMenubar.BackColor = System.Drawing.Color.Transparent
        Me.TripTicketMenubar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.TripTicketMenubar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewButton, Me.btnSave, Me.btnCancel})
        Me.TripTicketMenubar.Location = New System.Drawing.Point(0, 0)
        Me.TripTicketMenubar.Name = "TripTicketMenubar"
        Me.TripTicketMenubar.Size = New System.Drawing.Size(769, 25)
        Me.TripTicketMenubar.TabIndex = 104
        Me.TripTicketMenubar.Text = "ToolStrip1"
        '
        'NewButton
        '
        Me.NewButton.Image = CType(resources.GetObject("NewButton.Image"), System.Drawing.Image)
        Me.NewButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewButton.Name = "NewButton"
        Me.NewButton.Size = New System.Drawing.Size(51, 22)
        Me.NewButton.Text = "New"
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
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "Description"
        Me.DataGridViewTextBoxColumn4.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 366
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(312, 32)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(152, 22)
        Me.TextBox1.TabIndex = 105
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(248, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 24)
        Me.Label1.TabIndex = 106
        Me.Label1.Text = "Name"
        '
        'RoutePayRateMatrixForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(769, 423)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.TripTicketMenubar)
        Me.Controls.Add(Me.RouteDataGridView)
        Me.Controls.Add(Me.RouteRatesDataGridView)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "RoutePayRateMatrixForm"
        Me.Text = "Route Matrix"
        CType(Me.RouteRatesDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RouteDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TripTicketMenubar.ResumeLayout(False)
        Me.TripTicketMenubar.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RouteRatesDataGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RouteDataGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents TripTicketMenubar As System.Windows.Forms.ToolStrip
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents colRoutePayRatesPosition As DataGridViewTextBoxColumn
    Friend WithEvents colRoutePayRatesRate As DataGridViewTextBoxColumn
    Friend WithEvents NewButton As ToolStripButton
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
End Class
