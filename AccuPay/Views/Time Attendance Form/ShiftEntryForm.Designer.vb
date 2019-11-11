<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ShiftEntryForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpTimeTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpTimeFrom = New System.Windows.Forms.DateTimePicker()
        Me.dgvshiftentry = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_timef = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_timet = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DivisorToDailyRate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.breaktimefrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.breaktimeto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IsHidden = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewShift = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveShift = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancelShift = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCloseShift = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkHidden = New System.Windows.Forms.CheckBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpBreakTimeTo = New System.Windows.Forms.DateTimePicker()
        Me.chkHasLunchBreak = New System.Windows.Forms.CheckBox()
        Me.dtpBreakTimeFrom = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDivisorToDailyRate = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvshiftentry, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(211, 85)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 343
        Me.Label8.Text = "Time To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(38, 85)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 342
        Me.Label3.Text = "Time From"
        '
        'dtpTimeTo
        '
        Me.dtpTimeTo.CustomFormat = "hh:mm tt"
        Me.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTimeTo.Location = New System.Drawing.Point(263, 78)
        Me.dtpTimeTo.Name = "dtpTimeTo"
        Me.dtpTimeTo.ShowUpDown = True
        Me.dtpTimeTo.Size = New System.Drawing.Size(105, 20)
        Me.dtpTimeTo.TabIndex = 341
        '
        'dtpTimeFrom
        '
        Me.dtpTimeFrom.CustomFormat = "hh:mm tt"
        Me.dtpTimeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTimeFrom.Location = New System.Drawing.Point(100, 78)
        Me.dtpTimeFrom.Name = "dtpTimeFrom"
        Me.dtpTimeFrom.ShowUpDown = True
        Me.dtpTimeFrom.Size = New System.Drawing.Size(105, 20)
        Me.dtpTimeFrom.TabIndex = 340
        '
        'dgvshiftentry
        '
        Me.dgvshiftentry.AllowUserToAddRows = False
        Me.dgvshiftentry.AllowUserToDeleteRows = False
        Me.dgvshiftentry.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvshiftentry.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvshiftentry.BackgroundColor = System.Drawing.Color.White
        Me.dgvshiftentry.ColumnHeadersHeight = 34
        Me.dgvshiftentry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvshiftentry.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_timef, Me.c_timet, Me.DivisorToDailyRate, Me.c_rowid, Me.breaktimefrom, Me.breaktimeto, Me.IsHidden})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvshiftentry.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvshiftentry.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvshiftentry.Location = New System.Drawing.Point(41, 133)
        Me.dgvshiftentry.Name = "dgvshiftentry"
        Me.dgvshiftentry.ReadOnly = True
        Me.dgvshiftentry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvshiftentry.Size = New System.Drawing.Size(383, 258)
        Me.dgvshiftentry.TabIndex = 345
        '
        'c_timef
        '
        Me.c_timef.HeaderText = "Time From"
        Me.c_timef.Name = "c_timef"
        Me.c_timef.ReadOnly = True
        '
        'c_timet
        '
        Me.c_timet.HeaderText = "Time To"
        Me.c_timet.Name = "c_timet"
        Me.c_timet.ReadOnly = True
        '
        'DivisorToDailyRate
        '
        Me.DivisorToDailyRate.HeaderText = "DivisorToDailyRate"
        Me.DivisorToDailyRate.Name = "DivisorToDailyRate"
        Me.DivisorToDailyRate.ReadOnly = True
        Me.DivisorToDailyRate.Visible = False
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "RowID"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.ReadOnly = True
        Me.c_rowid.Visible = False
        '
        'breaktimefrom
        '
        DataGridViewCellStyle1.Format = "t"
        DataGridViewCellStyle1.NullValue = Nothing
        Me.breaktimefrom.DefaultCellStyle = DataGridViewCellStyle1
        Me.breaktimefrom.HeaderText = "Breaktime From"
        Me.breaktimefrom.Name = "breaktimefrom"
        Me.breaktimefrom.ReadOnly = True
        '
        'breaktimeto
        '
        DataGridViewCellStyle2.Format = "t"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.breaktimeto.DefaultCellStyle = DataGridViewCellStyle2
        Me.breaktimeto.HeaderText = "Breaktime To"
        Me.breaktimeto.Name = "breaktimeto"
        Me.breaktimeto.ReadOnly = True
        '
        'IsHidden
        '
        Me.IsHidden.HeaderText = "Is Hidden"
        Me.IsHidden.Name = "IsHidden"
        Me.IsHidden.ReadOnly = True
        Me.IsHidden.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(268, 104)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 346
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        Me.btnSave.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(349, 104)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnDelete.TabIndex = 347
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        Me.btnDelete.Visible = False
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(187, 104)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(75, 23)
        Me.btnNew.TabIndex = 348
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        Me.btnNew.Visible = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewShift, Me.tsbtnSaveShift, Me.tsbtnCancelShift, Me.tsbtnCloseShift, Me.tsbtnAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(665, 25)
        Me.ToolStrip1.TabIndex = 349
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNewShift
        '
        Me.tsbtnNewShift.Image = Global.AccuPay.My.Resources.Resources._new
        Me.tsbtnNewShift.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewShift.Name = "tsbtnNewShift"
        Me.tsbtnNewShift.Size = New System.Drawing.Size(77, 22)
        Me.tsbtnNewShift.Text = "&New shift"
        '
        'tsbtnSaveShift
        '
        Me.tsbtnSaveShift.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.tsbtnSaveShift.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveShift.Name = "tsbtnSaveShift"
        Me.tsbtnSaveShift.Size = New System.Drawing.Size(77, 22)
        Me.tsbtnSaveShift.Text = "&Save shift"
        '
        'tsbtnCancelShift
        '
        Me.tsbtnCancelShift.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.tsbtnCancelShift.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancelShift.Name = "tsbtnCancelShift"
        Me.tsbtnCancelShift.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancelShift.Text = "Cancel"
        '
        'tsbtnCloseShift
        '
        Me.tsbtnCloseShift.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnCloseShift.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnCloseShift.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCloseShift.Name = "tsbtnCloseShift"
        Me.tsbtnCloseShift.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnCloseShift.Text = "Close"
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
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkHidden)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtDivisorToDailyRate)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Controls.Add(Me.dtpTimeFrom)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.dtpTimeTo)
        Me.Panel1.Controls.Add(Me.btnDelete)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.btnSave)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.dgvshiftentry)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 21)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(665, 403)
        Me.Panel1.TabIndex = 350
        '
        'chkHidden
        '
        Me.chkHidden.AutoSize = True
        Me.chkHidden.Location = New System.Drawing.Point(441, 115)
        Me.chkHidden.Name = "chkHidden"
        Me.chkHidden.Size = New System.Drawing.Size(60, 17)
        Me.chkHidden.TabIndex = 377
        Me.chkHidden.Text = "Hidden"
        Me.chkHidden.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(529, 282)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 376
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.dtpBreakTimeTo)
        Me.Panel2.Controls.Add(Me.chkHasLunchBreak)
        Me.Panel2.Controls.Add(Me.dtpBreakTimeFrom)
        Me.Panel2.Location = New System.Drawing.Point(438, 133)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(200, 100)
        Me.Panel2.TabIndex = 375
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(15, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 13)
        Me.Label6.TabIndex = 346
        Me.Label6.Text = "to"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 34)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(27, 13)
        Me.Label5.TabIndex = 345
        Me.Label5.Text = "from"
        '
        'dtpBreakTimeTo
        '
        Me.dtpBreakTimeTo.CustomFormat = "hh:mm tt"
        Me.dtpBreakTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpBreakTimeTo.Location = New System.Drawing.Point(48, 53)
        Me.dtpBreakTimeTo.Name = "dtpBreakTimeTo"
        Me.dtpBreakTimeTo.ShowUpDown = True
        Me.dtpBreakTimeTo.Size = New System.Drawing.Size(105, 20)
        Me.dtpBreakTimeTo.TabIndex = 344
        '
        'chkHasLunchBreak
        '
        Me.chkHasLunchBreak.AutoSize = True
        Me.chkHasLunchBreak.Checked = True
        Me.chkHasLunchBreak.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHasLunchBreak.Location = New System.Drawing.Point(3, 4)
        Me.chkHasLunchBreak.Name = "chkHasLunchBreak"
        Me.chkHasLunchBreak.Size = New System.Drawing.Size(104, 17)
        Me.chkHasLunchBreak.TabIndex = 342
        Me.chkHasLunchBreak.Text = "Has lunch break"
        Me.chkHasLunchBreak.UseVisualStyleBackColor = True
        '
        'dtpBreakTimeFrom
        '
        Me.dtpBreakTimeFrom.CustomFormat = "hh:mm tt"
        Me.dtpBreakTimeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpBreakTimeFrom.Location = New System.Drawing.Point(48, 27)
        Me.dtpBreakTimeFrom.Name = "dtpBreakTimeFrom"
        Me.dtpBreakTimeFrom.ShowUpDown = True
        Me.dtpBreakTimeFrom.Size = New System.Drawing.Size(105, 20)
        Me.dtpBreakTimeFrom.TabIndex = 343
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(437, 236)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(141, 13)
        Me.Label1.TabIndex = 371
        Me.Label1.Text = "Divisor for getting hourly rate"
        '
        'txtDivisorToDailyRate
        '
        Me.txtDivisorToDailyRate.Location = New System.Drawing.Point(437, 252)
        Me.txtDivisorToDailyRate.Name = "txtDivisorToDailyRate"
        Me.txtDivisorToDailyRate.Size = New System.Drawing.Size(75, 20)
        Me.txtDivisorToDailyRate.TabIndex = 345
        Me.txtDivisorToDailyRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(38, 115)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(92, 15)
        Me.Label4.TabIndex = 369
        Me.Label4.Text = "List of duty shift"
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(160, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(665, 21)
        Me.Label25.TabIndex = 351
        Me.Label25.Text = "SHIFTING"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Time From"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 149
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Time To"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 148
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "DivisorToDailyRate"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Visible = False
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Visible = False
        '
        'DataGridViewTextBoxColumn5
        '
        DataGridViewCellStyle4.Format = "t"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewTextBoxColumn5.HeaderText = "breaktimefrom"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Visible = False
        Me.DataGridViewTextBoxColumn5.Width = 85
        '
        'DataGridViewTextBoxColumn6
        '
        DataGridViewCellStyle5.Format = "t"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumn6.HeaderText = "breaktimeto"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Visible = False
        Me.DataGridViewTextBoxColumn6.Width = 85
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Is Hidden"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Visible = False
        '
        'ShiftEntryForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(665, 424)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label25)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShiftEntryForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvshiftentry, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpTimeTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpTimeFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents dgvshiftentry As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnNewShift As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSaveShift As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancelShift As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCloseShift As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDivisorToDailyRate As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents dtpBreakTimeTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkHasLunchBreak As System.Windows.Forms.CheckBox
    Friend WithEvents dtpBreakTimeFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents c_timef As DataGridViewTextBoxColumn
    Friend WithEvents c_timet As DataGridViewTextBoxColumn
    Friend WithEvents DivisorToDailyRate As DataGridViewTextBoxColumn
    Friend WithEvents c_rowid As DataGridViewTextBoxColumn
    Friend WithEvents breaktimefrom As DataGridViewTextBoxColumn
    Friend WithEvents breaktimeto As DataGridViewTextBoxColumn
    Friend WithEvents IsHidden As DataGridViewTextBoxColumn
    Friend WithEvents chkHidden As CheckBox
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
End Class
