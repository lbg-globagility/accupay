<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dutyshift
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
        Me.dgvshift = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.shf_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.shf_TimeFrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.shf_TimeTo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.mtxtTimeFrom = New System.Windows.Forms.MaskedTextBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewShift = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveShift = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.mtxtTimeTo = New System.Windows.Forms.MaskedTextBox()
        Me.Label212 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpTimeFrom = New System.Windows.Forms.DateTimePicker()
        Me.dtpTimeTo = New System.Windows.Forms.DateTimePicker()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.dgvshift, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvshift
        '
        Me.dgvshift.AllowUserToAddRows = False
        Me.dgvshift.AllowUserToDeleteRows = False
        Me.dgvshift.AllowUserToOrderColumns = True
        Me.dgvshift.BackgroundColor = System.Drawing.Color.White
        Me.dgvshift.ColumnHeadersHeight = 34
        Me.dgvshift.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvshift.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.shf_RowID, Me.shf_TimeFrom, Me.shf_TimeTo})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvshift.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvshift.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvshift.Location = New System.Drawing.Point(12, 118)
        Me.dgvshift.MultiSelect = False
        Me.dgvshift.Name = "dgvshift"
        Me.dgvshift.ReadOnly = True
        Me.dgvshift.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvshift.Size = New System.Drawing.Size(414, 169)
        Me.dgvshift.TabIndex = 321
        '
        'shf_RowID
        '
        Me.shf_RowID.HeaderText = "RowID"
        Me.shf_RowID.Name = "shf_RowID"
        Me.shf_RowID.ReadOnly = True
        Me.shf_RowID.Visible = False
        '
        'shf_TimeFrom
        '
        Me.shf_TimeFrom.HeaderText = "Time from"
        Me.shf_TimeFrom.Name = "shf_TimeFrom"
        Me.shf_TimeFrom.ReadOnly = True
        Me.shf_TimeFrom.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.shf_TimeFrom.Width = 186
        '
        'shf_TimeTo
        '
        Me.shf_TimeTo.HeaderText = "Time to"
        Me.shf_TimeTo.Name = "shf_TimeTo"
        Me.shf_TimeTo.ReadOnly = True
        Me.shf_TimeTo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.shf_TimeTo.Width = 185
        '
        'mtxtTimeFrom
        '
        Me.mtxtTimeFrom.Location = New System.Drawing.Point(305, 42)
        Me.mtxtTimeFrom.Name = "mtxtTimeFrom"
        Me.mtxtTimeFrom.Size = New System.Drawing.Size(100, 20)
        Me.mtxtTimeFrom.TabIndex = 329
        Me.mtxtTimeFrom.Visible = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewShift, Me.tsbtnSaveShift, Me.tsbtnCancel, Me.tsbtnAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(438, 25)
        Me.ToolStrip1.TabIndex = 330
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNewShift
        '
        Me.tsbtnNewShift.Image = Global.GoldWingsPayrollSys.My.Resources.Resources._new
        Me.tsbtnNewShift.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewShift.Name = "tsbtnNewShift"
        Me.tsbtnNewShift.Size = New System.Drawing.Size(75, 22)
        Me.tsbtnNewShift.Text = "&Add shift"
        '
        'tsbtnSaveShift
        '
        Me.tsbtnSaveShift.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Save
        Me.tsbtnSaveShift.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveShift.Name = "tsbtnSaveShift"
        Me.tsbtnSaveShift.Size = New System.Drawing.Size(91, 22)
        Me.tsbtnSaveShift.Text = "&Update shift"
        Me.tsbtnSaveShift.Visible = False
        '
        'tsbtnCancel
        '
        Me.tsbtnCancel.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.cancel1
        Me.tsbtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancel.Name = "tsbtnCancel"
        Me.tsbtnCancel.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancel.Text = "Cancel"
        Me.tsbtnCancel.Visible = False
        '
        'tsbtnAudittrail
        '
        Me.tsbtnAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnAudittrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnAudittrail.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.audit_trail_icon
        Me.tsbtnAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnAudittrail.Name = "tsbtnAudittrail"
        Me.tsbtnAudittrail.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnAudittrail.Text = "ToolStripButton1"
        Me.tsbtnAudittrail.ToolTipText = "Show audit trails"
        Me.tsbtnAudittrail.Visible = False
        '
        'mtxtTimeTo
        '
        Me.mtxtTimeTo.Location = New System.Drawing.Point(305, 68)
        Me.mtxtTimeTo.Name = "mtxtTimeTo"
        Me.mtxtTimeTo.Size = New System.Drawing.Size(100, 20)
        Me.mtxtTimeTo.TabIndex = 330
        Me.mtxtTimeTo.Visible = False
        '
        'Label212
        '
        Me.Label212.AutoSize = True
        Me.Label212.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label212.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label212.Location = New System.Drawing.Point(65, 42)
        Me.Label212.Name = "Label212"
        Me.Label212.Size = New System.Drawing.Size(18, 24)
        Me.Label212.TabIndex = 360
        Me.Label212.Text = "*"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 13)
        Me.Label1.TabIndex = 361
        Me.Label1.Text = "Time from :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 362
        Me.Label2.Text = "Time to :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(54, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(18, 24)
        Me.Label3.TabIndex = 363
        Me.Label3.Text = "*"
        '
        'dtpTimeFrom
        '
        Me.dtpTimeFrom.CustomFormat = "hh:mm tt"
        Me.dtpTimeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTimeFrom.Location = New System.Drawing.Point(89, 43)
        Me.dtpTimeFrom.Name = "dtpTimeFrom"
        Me.dtpTimeFrom.ShowUpDown = True
        Me.dtpTimeFrom.Size = New System.Drawing.Size(105, 20)
        Me.dtpTimeFrom.TabIndex = 364
        '
        'dtpTimeTo
        '
        Me.dtpTimeTo.CustomFormat = "hh:mm tt"
        Me.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTimeTo.Location = New System.Drawing.Point(89, 69)
        Me.dtpTimeTo.Name = "dtpTimeTo"
        Me.dtpTimeTo.ShowUpDown = True
        Me.dtpTimeTo.Size = New System.Drawing.Size(105, 20)
        Me.dtpTimeTo.TabIndex = 365
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(192, 293)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(114, 45)
        Me.Button1.TabIndex = 366
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(312, 293)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(114, 45)
        Me.Button2.TabIndex = 367
        Me.Button2.Text = "Close"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(9, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(92, 15)
        Me.Label4.TabIndex = 368
        Me.Label4.Text = "List of duty shift"
        Me.Label4.Visible = False
        '
        'dutyshift
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(438, 344)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.dtpTimeTo)
        Me.Controls.Add(Me.dtpTimeFrom)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label212)
        Me.Controls.Add(Me.mtxtTimeTo)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.mtxtTimeFrom)
        Me.Controls.Add(Me.dgvshift)
        Me.Controls.Add(Me.Label3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dutyshift"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "List of duty shift"
        CType(Me.dgvshift, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvshift As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents mtxtTimeFrom As System.Windows.Forms.MaskedTextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents mtxtTimeTo As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label212 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tsbtnNewShift As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSaveShift As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents dtpTimeFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpTimeTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents shf_RowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents shf_TimeFrom As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents shf_TimeTo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tsbtnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
