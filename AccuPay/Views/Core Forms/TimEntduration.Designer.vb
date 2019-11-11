<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TimEntduration
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
        Me.cbomonth = New System.Windows.Forms.ComboBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.txtYr = New System.Windows.Forms.TextBox()
        Me.dgvpayper = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.bgWork = New System.ComponentModel.BackgroundWorker()
        Me.progbar = New System.Windows.Forms.ProgressBar()
        Me.linkNxt = New System.Windows.Forms.LinkLabel()
        Me.linkPrev = New System.Windows.Forms.LinkLabel()
        Me.tstrip = New System.Windows.Forms.ToolStrip()
        Me.dgvpaypers = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.bgworkRECOMPUTE_employeeleave = New System.ComponentModel.BackgroundWorker()
        Me.cboxDivisions = New System.Windows.Forms.ComboBox()
        Me.Label124 = New System.Windows.Forms.Label()
        Me.lblgrouping = New System.Windows.Forms.Label()
        CType(Me.dgvpayper, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvpaypers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbomonth
        '
        Me.cbomonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbomonth.FormattingEnabled = True
        Me.cbomonth.Location = New System.Drawing.Point(12, 22)
        Me.cbomonth.Name = "cbomonth"
        Me.cbomonth.Size = New System.Drawing.Size(84, 21)
        Me.cbomonth.TabIndex = 0
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(146, 21)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(153, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Search month and year"
        Me.Button2.UseVisualStyleBackColor = True
        Me.Button2.Visible = False
        '
        'txtYr
        '
        Me.txtYr.Location = New System.Drawing.Point(102, 23)
        Me.txtYr.MaxLength = 4
        Me.txtYr.Name = "txtYr"
        Me.txtYr.Size = New System.Drawing.Size(38, 20)
        Me.txtYr.TabIndex = 1
        Me.txtYr.Visible = False
        '
        'dgvpayper
        '
        Me.dgvpayper.AllowUserToAddRows = False
        Me.dgvpayper.AllowUserToDeleteRows = False
        Me.dgvpayper.AllowUserToOrderColumns = True
        Me.dgvpayper.BackgroundColor = System.Drawing.Color.White
        Me.dgvpayper.ColumnHeadersHeight = 38
        Me.dgvpayper.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column10, Me.Column11, Me.Column12, Me.Column13, Me.Column14})
        Me.dgvpayper.Location = New System.Drawing.Point(543, 95)
        Me.dgvpayper.MultiSelect = False
        Me.dgvpayper.Name = "dgvpayper"
        Me.dgvpayper.ReadOnly = True
        Me.dgvpayper.RowTemplate.Height = 23
        Me.dgvpayper.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvpayper.Size = New System.Drawing.Size(411, 249)
        Me.dgvpayper.TabIndex = 3
        Me.dgvpayper.Visible = False
        '
        'Column1
        '
        Me.Column1.HeaderText = "Column1"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Visible = False
        Me.Column1.Width = 73
        '
        'Column2
        '
        Me.Column2.HeaderText = "Pay period from"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 184
        '
        'Column3
        '
        Me.Column3.HeaderText = "Pay period to"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 184
        '
        'Column4
        '
        Me.Column4.HeaderText = "Column4"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Visible = False
        Me.Column4.Width = 73
        '
        'Column5
        '
        Me.Column5.HeaderText = "Column5"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Visible = False
        Me.Column5.Width = 73
        '
        'Column6
        '
        Me.Column6.HeaderText = "Column6"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Visible = False
        Me.Column6.Width = 73
        '
        'Column7
        '
        Me.Column7.HeaderText = "Column7"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Visible = False
        Me.Column7.Width = 73
        '
        'Column8
        '
        Me.Column8.HeaderText = "Column8"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        Me.Column8.Visible = False
        Me.Column8.Width = 73
        '
        'Column9
        '
        Me.Column9.HeaderText = "Column9"
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        Me.Column9.Visible = False
        Me.Column9.Width = 73
        '
        'Column10
        '
        Me.Column10.HeaderText = "Column10"
        Me.Column10.Name = "Column10"
        Me.Column10.ReadOnly = True
        Me.Column10.Visible = False
        Me.Column10.Width = 79
        '
        'Column11
        '
        Me.Column11.HeaderText = "Column11"
        Me.Column11.Name = "Column11"
        Me.Column11.ReadOnly = True
        Me.Column11.Visible = False
        Me.Column11.Width = 79
        '
        'Column12
        '
        Me.Column12.HeaderText = "Column12"
        Me.Column12.Name = "Column12"
        Me.Column12.ReadOnly = True
        Me.Column12.Visible = False
        Me.Column12.Width = 79
        '
        'Column13
        '
        Me.Column13.HeaderText = "Column13"
        Me.Column13.Name = "Column13"
        Me.Column13.ReadOnly = True
        Me.Column13.Visible = False
        Me.Column13.Width = 79
        '
        'Column14
        '
        Me.Column14.HeaderText = "Column14"
        Me.Column14.Name = "Column14"
        Me.Column14.ReadOnly = True
        Me.Column14.Visible = False
        Me.Column14.Width = 79
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(396, 381)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(141, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Compute hour(s) worked"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'bgWork
        '
        Me.bgWork.WorkerReportsProgress = True
        Me.bgWork.WorkerSupportsCancellation = True
        '
        'progbar
        '
        Me.progbar.Location = New System.Drawing.Point(12, 381)
        Me.progbar.Name = "progbar"
        Me.progbar.Size = New System.Drawing.Size(100, 23)
        Me.progbar.TabIndex = 5
        Me.progbar.Visible = False
        '
        'linkNxt
        '
        Me.linkNxt.AutoSize = True
        Me.linkNxt.Dock = System.Windows.Forms.DockStyle.Right
        Me.linkNxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkNxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkNxt.Location = New System.Drawing.Point(372, 0)
        Me.linkNxt.Name = "linkNxt"
        Me.linkNxt.Size = New System.Drawing.Size(39, 15)
        Me.linkNxt.TabIndex = 7
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
        Me.linkPrev.TabIndex = 6
        Me.linkPrev.TabStop = True
        Me.linkPrev.Text = "<Prev"
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
        Me.tstrip.Location = New System.Drawing.Point(13, 95)
        Me.tstrip.Name = "tstrip"
        Me.tstrip.Size = New System.Drawing.Size(111, 249)
        Me.tstrip.TabIndex = 282
        Me.tstrip.Text = "ToolStrip1"
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
        Me.dgvpaypers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.DataGridViewTextBoxColumn6, Me.DataGridViewTextBoxColumn7, Me.DataGridViewTextBoxColumn8, Me.DataGridViewTextBoxColumn9, Me.DataGridViewTextBoxColumn10, Me.DataGridViewTextBoxColumn11, Me.DataGridViewTextBoxColumn12, Me.DataGridViewTextBoxColumn13, Me.DataGridViewTextBoxColumn14})
        Me.dgvpaypers.Location = New System.Drawing.Point(125, 95)
        Me.dgvpaypers.MultiSelect = False
        Me.dgvpaypers.Name = "dgvpaypers"
        Me.dgvpaypers.ReadOnly = True
        Me.dgvpaypers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvpaypers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvpaypers.Size = New System.Drawing.Size(411, 249)
        Me.dgvpaypers.TabIndex = 283
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Column1"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Pay period from"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn2.Width = 183
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Pay period to"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn3.Width = 183
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "Column4"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Visible = False
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "Column5"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Visible = False
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Column6"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Visible = False
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Column7"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Visible = False
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "Column8"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Visible = False
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "Column9"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Visible = False
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "Column10"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.Visible = False
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.HeaderText = "Column11"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.ReadOnly = True
        Me.DataGridViewTextBoxColumn11.Visible = False
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.HeaderText = "Column12"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.ReadOnly = True
        Me.DataGridViewTextBoxColumn12.Visible = False
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.HeaderText = "Column13"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        Me.DataGridViewTextBoxColumn13.ReadOnly = True
        Me.DataGridViewTextBoxColumn13.Visible = False
        '
        'DataGridViewTextBoxColumn14
        '
        Me.DataGridViewTextBoxColumn14.HeaderText = "Column14"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        Me.DataGridViewTextBoxColumn14.ReadOnly = True
        Me.DataGridViewTextBoxColumn14.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CheckBox1)
        Me.Panel1.Controls.Add(Me.cbomonth)
        Me.Panel1.Controls.Add(Me.dgvpaypers)
        Me.Panel1.Controls.Add(Me.txtYr)
        Me.Panel1.Controls.Add(Me.tstrip)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.dgvpayper)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(547, 375)
        Me.Panel1.TabIndex = 284
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.Location = New System.Drawing.Point(315, 27)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(232, 17)
        Me.CheckBox1.TabIndex = 286
        Me.CheckBox1.Text = "Generate only the Resigned employee(s)"
        Me.CheckBox1.UseVisualStyleBackColor = True
        Me.CheckBox1.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.linkNxt)
        Me.Panel2.Controls.Add(Me.linkPrev)
        Me.Panel2.Location = New System.Drawing.Point(125, 344)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(411, 15)
        Me.Panel2.TabIndex = 285
        '
        'bgworkRECOMPUTE_employeeleave
        '
        Me.bgworkRECOMPUTE_employeeleave.WorkerReportsProgress = True
        Me.bgworkRECOMPUTE_employeeleave.WorkerSupportsCancellation = True
        '
        'cboxDivisions
        '
        Me.cboxDivisions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboxDivisions.FormattingEnabled = True
        Me.cboxDivisions.Location = New System.Drawing.Point(243, 382)
        Me.cboxDivisions.Name = "cboxDivisions"
        Me.cboxDivisions.Size = New System.Drawing.Size(147, 21)
        Me.cboxDivisions.TabIndex = 285
        '
        'Label124
        '
        Me.Label124.AutoSize = True
        Me.Label124.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label124.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label124.Location = New System.Drawing.Point(218, 382)
        Me.Label124.Name = "Label124"
        Me.Label124.Size = New System.Drawing.Size(18, 24)
        Me.Label124.TabIndex = 286
        Me.Label124.Text = "*"
        '
        'lblgrouping
        '
        Me.lblgrouping.AutoSize = True
        Me.lblgrouping.Location = New System.Drawing.Point(186, 390)
        Me.lblgrouping.Name = "lblgrouping"
        Me.lblgrouping.Size = New System.Drawing.Size(36, 13)
        Me.lblgrouping.TabIndex = 287
        Me.lblgrouping.Text = "Group"
        '
        'TimEntduration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(547, 416)
        Me.Controls.Add(Me.lblgrouping)
        Me.Controls.Add(Me.cboxDivisions)
        Me.Controls.Add(Me.progbar)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label124)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TimEntduration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvpayper, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvpaypers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbomonth As System.Windows.Forms.ComboBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents txtYr As System.Windows.Forms.TextBox
    Friend WithEvents dgvpayper As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents bgWork As System.ComponentModel.BackgroundWorker
    Friend WithEvents progbar As System.Windows.Forms.ProgressBar
    Friend WithEvents linkNxt As System.Windows.Forms.LinkLabel
    Friend WithEvents linkPrev As System.Windows.Forms.LinkLabel
    Friend WithEvents tstrip As System.Windows.Forms.ToolStrip
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
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
    Friend WithEvents dgvpaypers As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents bgworkRECOMPUTE_employeeleave As System.ComponentModel.BackgroundWorker
    Friend WithEvents cboxDivisions As System.Windows.Forms.ComboBox
    Friend WithEvents Label124 As System.Windows.Forms.Label
    Friend WithEvents lblgrouping As System.Windows.Forms.Label
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
End Class
