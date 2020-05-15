<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class selectPayPeriod
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvpaypers = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SSSContribSched = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PhHContribSched = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.HDMFContribSched = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PayPeriodMinWageValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.linkNxt = New System.Windows.Forms.LinkLabel()
        Me.linkPrev = New System.Windows.Forms.LinkLabel()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblpapyperiodval = New System.Windows.Forms.Label()
        Me.tstrip = New System.Windows.Forms.ToolStrip()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.PayPeriodStatusLabel = New System.Windows.Forms.Label()
        CType(Me.dgvpaypers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvpaypers
        '
        Me.dgvpaypers.AllowUserToAddRows = False
        Me.dgvpaypers.AllowUserToDeleteRows = False
        Me.dgvpaypers.AllowUserToOrderColumns = True
        Me.dgvpaypers.AllowUserToResizeColumns = False
        Me.dgvpaypers.AllowUserToResizeRows = False
        Me.dgvpaypers.BackgroundColor = System.Drawing.Color.White
        Me.dgvpaypers.ColumnHeadersHeight = 38
        Me.dgvpaypers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvpaypers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column15, Me.Column16, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column10, Me.Column11, Me.Column12, Me.Column13, Me.Column14, Me.SSSContribSched, Me.PhHContribSched, Me.HDMFContribSched, Me.PayPeriodMinWageValue})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvpaypers.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvpaypers.Location = New System.Drawing.Point(122, 43)
        Me.dgvpaypers.MultiSelect = False
        Me.dgvpaypers.Name = "dgvpaypers"
        Me.dgvpaypers.ReadOnly = True
        Me.dgvpaypers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvpaypers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvpaypers.Size = New System.Drawing.Size(416, 373)
        Me.dgvpaypers.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = "Column1"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Visible = False
        '
        'Column2
        '
        Me.Column2.HeaderText = "Column2"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column2.Visible = False
        Me.Column2.Width = 178
        '
        'Column3
        '
        Me.Column3.HeaderText = "Column3"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column3.Visible = False
        Me.Column3.Width = 178
        '
        'Column15
        '
        Me.Column15.HeaderText = "Pay period from"
        Me.Column15.Name = "Column15"
        Me.Column15.ReadOnly = True
        Me.Column15.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column15.Width = 178
        '
        'Column16
        '
        Me.Column16.HeaderText = "Pay period to"
        Me.Column16.Name = "Column16"
        Me.Column16.ReadOnly = True
        Me.Column16.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column16.Width = 178
        '
        'Column4
        '
        Me.Column4.HeaderText = "Column4"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Visible = False
        '
        'Column5
        '
        Me.Column5.HeaderText = "Column5"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Visible = False
        '
        'Column6
        '
        Me.Column6.HeaderText = "Column6"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Visible = False
        '
        'Column7
        '
        Me.Column7.HeaderText = "Column7"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Visible = False
        '
        'Column8
        '
        Me.Column8.HeaderText = "Column8"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        Me.Column8.Visible = False
        '
        'Column9
        '
        Me.Column9.HeaderText = "Column9"
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        Me.Column9.Visible = False
        '
        'Column10
        '
        Me.Column10.HeaderText = "Column10"
        Me.Column10.Name = "Column10"
        Me.Column10.ReadOnly = True
        Me.Column10.Visible = False
        '
        'Column11
        '
        Me.Column11.HeaderText = "Column11"
        Me.Column11.Name = "Column11"
        Me.Column11.ReadOnly = True
        Me.Column11.Visible = False
        '
        'Column12
        '
        Me.Column12.HeaderText = "Column12"
        Me.Column12.Name = "Column12"
        Me.Column12.ReadOnly = True
        Me.Column12.Visible = False
        '
        'Column13
        '
        Me.Column13.HeaderText = "Column13"
        Me.Column13.Name = "Column13"
        Me.Column13.ReadOnly = True
        Me.Column13.Visible = False
        '
        'Column14
        '
        Me.Column14.HeaderText = "Column14"
        Me.Column14.Name = "Column14"
        Me.Column14.ReadOnly = True
        Me.Column14.Visible = False
        Me.Column14.Width = 124
        '
        'SSSContribSched
        '
        Me.SSSContribSched.HeaderText = "SSSContribSched"
        Me.SSSContribSched.Name = "SSSContribSched"
        Me.SSSContribSched.ReadOnly = True
        Me.SSSContribSched.Visible = False
        '
        'PhHContribSched
        '
        Me.PhHContribSched.HeaderText = "PhHContribSched"
        Me.PhHContribSched.Name = "PhHContribSched"
        Me.PhHContribSched.ReadOnly = True
        Me.PhHContribSched.Visible = False
        '
        'HDMFContribSched
        '
        Me.HDMFContribSched.HeaderText = "HDMFContribSched"
        Me.HDMFContribSched.Name = "HDMFContribSched"
        Me.HDMFContribSched.ReadOnly = True
        Me.HDMFContribSched.Visible = False
        '
        'PayPeriodMinWageValue
        '
        Me.PayPeriodMinWageValue.HeaderText = "Minimum Wage Amount"
        Me.PayPeriodMinWageValue.Name = "PayPeriodMinWageValue"
        Me.PayPeriodMinWageValue.ReadOnly = True
        Me.PayPeriodMinWageValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.PayPeriodMinWageValue.Visible = False
        Me.PayPeriodMinWageValue.Width = 124
        '
        'linkNxt
        '
        Me.linkNxt.AutoSize = True
        Me.linkNxt.Dock = System.Windows.Forms.DockStyle.Right
        Me.linkNxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkNxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkNxt.Location = New System.Drawing.Point(377, 0)
        Me.linkNxt.Name = "linkNxt"
        Me.linkNxt.Size = New System.Drawing.Size(39, 15)
        Me.linkNxt.TabIndex = 2
        Me.linkNxt.TabStop = True
        Me.linkNxt.Text = "Next>"
        '
        'linkPrev
        '
        Me.linkPrev.AutoSize = True
        Me.linkPrev.Dock = System.Windows.Forms.DockStyle.Left
        Me.linkPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkPrev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkPrev.Location = New System.Drawing.Point(0, 0)
        Me.linkPrev.Name = "linkPrev"
        Me.linkPrev.Size = New System.Drawing.Size(38, 15)
        Me.linkPrev.TabIndex = 1
        Me.linkPrev.TabStop = True
        Me.linkPrev.Text = "<Prev"
        '
        'OkButton
        '
        Me.OkButton.Location = New System.Drawing.Point(383, 461)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 35)
        Me.OkButton.TabIndex = 3
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.Location = New System.Drawing.Point(464, 461)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 35)
        Me.CloseButton.TabIndex = 4
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(119, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 15)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Pay period"
        '
        'lblpapyperiodval
        '
        Me.lblpapyperiodval.AutoSize = True
        Me.lblpapyperiodval.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblpapyperiodval.Location = New System.Drawing.Point(178, 25)
        Me.lblpapyperiodval.Name = "lblpapyperiodval"
        Me.lblpapyperiodval.Size = New System.Drawing.Size(32, 15)
        Me.lblpapyperiodval.TabIndex = 14
        Me.lblpapyperiodval.Text = "-----"
        '
        'tstrip
        '
        Me.tstrip.AutoSize = False
        Me.tstrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.tstrip.CanOverflow = False
        Me.tstrip.Dock = System.Windows.Forms.DockStyle.None
        Me.tstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tstrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tstrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.tstrip.Location = New System.Drawing.Point(10, 43)
        Me.tstrip.Name = "tstrip"
        Me.tstrip.Size = New System.Drawing.Size(111, 373)
        Me.tstrip.TabIndex = 281
        Me.tstrip.Text = "ToolStrip1"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.linkNxt)
        Me.Panel2.Controls.Add(Me.linkPrev)
        Me.Panel2.Location = New System.Drawing.Point(122, 416)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(416, 29)
        Me.Panel2.TabIndex = 283
        '
        'PayPeriodStatusLabel
        '
        Me.PayPeriodStatusLabel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.PayPeriodStatusLabel.Location = New System.Drawing.Point(452, 25)
        Me.PayPeriodStatusLabel.Name = "PayPeriodStatusLabel"
        Me.PayPeriodStatusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PayPeriodStatusLabel.Size = New System.Drawing.Size(86, 15)
        Me.PayPeriodStatusLabel.TabIndex = 284
        Me.PayPeriodStatusLabel.Text = "-----"
        Me.PayPeriodStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'selectPayPeriod
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(548, 508)
        Me.Controls.Add(Me.PayPeriodStatusLabel)
        Me.Controls.Add(Me.tstrip)
        Me.Controls.Add(Me.lblpapyperiodval)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.dgvpaypers)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "selectPayPeriod"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "List of pay period(s)"
        CType(Me.dgvpaypers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvpaypers As System.Windows.Forms.DataGridView
    Friend WithEvents linkNxt As System.Windows.Forms.LinkLabel
    Friend WithEvents linkPrev As System.Windows.Forms.LinkLabel
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblpapyperiodval As System.Windows.Forms.Label
    Friend WithEvents tstrip As System.Windows.Forms.ToolStrip
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column15 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column16 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SSSContribSched As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PhHContribSched As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents HDMFContribSched As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PayPeriodMinWageValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PayPeriodStatusLabel As Label
End Class
