<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AuditTrailForm
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
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cbAction = New System.Windows.Forms.ComboBox()
        Me.cbUserID = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.c_viewname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ActionPerformed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.newval = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Oldval = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_changedrowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FieldChanged = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Date = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dgvAuditList = New System.Windows.Forms.DataGridView()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.ToolStrip2.SuspendLayout()
        CType(Me.dgvAuditList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(167, 208)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 23)
        Me.btnPrint.TabIndex = 89
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnShow
        '
        Me.btnShow.Location = New System.Drawing.Point(86, 208)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(75, 23)
        Me.btnShow.TabIndex = 88
        Me.btnShow.Text = "&Show"
        Me.btnShow.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(21, 165)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 87
        Me.Label5.Text = "Action:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(21, 138)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 13)
        Me.Label4.TabIndex = 86
        Me.Label4.Text = "User ID:"
        '
        'cbAction
        '
        Me.cbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAction.FormattingEnabled = True
        Me.cbAction.Items.AddRange(New Object() {"All", "Insert", "Update", "Delete"})
        Me.cbAction.Location = New System.Drawing.Point(86, 157)
        Me.cbAction.Name = "cbAction"
        Me.cbAction.Size = New System.Drawing.Size(208, 21)
        Me.cbAction.TabIndex = 85
        '
        'cbUserID
        '
        Me.cbUserID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbUserID.FormattingEnabled = True
        Me.cbUserID.Location = New System.Drawing.Point(86, 130)
        Me.cbUserID.Name = "cbUserID"
        Me.cbUserID.Size = New System.Drawing.Size(208, 21)
        Me.cbUserID.TabIndex = 84
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(197, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 83
        Me.Label3.Text = "To Date:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 13)
        Me.Label1.TabIndex = 82
        Me.Label1.Text = "From Date:"
        '
        'dtpTo
        '
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTo.Location = New System.Drawing.Point(252, 97)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(102, 20)
        Me.dtpTo.TabIndex = 81
        '
        'dtpFrom
        '
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFrom.Location = New System.Drawing.Point(86, 97)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(102, 20)
        Me.dtpFrom.TabIndex = 80
        '
        'GroupBox1
        '
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.GroupBox1.Location = New System.Drawing.Point(14, 60)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(358, 181)
        Me.GroupBox1.TabIndex = 91
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filter Option"
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToolStrip2.AutoSize = False
        Me.ToolStrip2.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnClose})
        Me.ToolStrip2.Location = New System.Drawing.Point(-2, 29)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(934, 25)
        Me.ToolStrip2.TabIndex = 78
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'c_viewname
        '
        Me.c_viewname.HeaderText = "View Name"
        Me.c_viewname.Name = "c_viewname"
        Me.c_viewname.ReadOnly = True
        Me.c_viewname.Width = 150
        '
        'ActionPerformed
        '
        Me.ActionPerformed.HeaderText = "Action Performed"
        Me.ActionPerformed.Name = "ActionPerformed"
        Me.ActionPerformed.ReadOnly = True
        Me.ActionPerformed.Width = 120
        '
        'newval
        '
        Me.newval.HeaderText = "New Value"
        Me.newval.Name = "newval"
        Me.newval.ReadOnly = True
        Me.newval.Width = 150
        '
        'Oldval
        '
        Me.Oldval.HeaderText = "Old Value"
        Me.Oldval.Name = "Oldval"
        Me.Oldval.ReadOnly = True
        Me.Oldval.Width = 150
        '
        'c_changedrowid
        '
        Me.c_changedrowid.HeaderText = "Changed RowID"
        Me.c_changedrowid.Name = "c_changedrowid"
        Me.c_changedrowid.ReadOnly = True
        '
        'FieldChanged
        '
        Me.FieldChanged.HeaderText = "Field Name"
        Me.FieldChanged.Name = "FieldChanged"
        Me.FieldChanged.ReadOnly = True
        Me.FieldChanged.Width = 200
        '
        'UserID
        '
        Me.UserID.HeaderText = "User ID"
        Me.UserID.Name = "UserID"
        Me.UserID.ReadOnly = True
        '
        'c_Date
        '
        Me.c_Date.HeaderText = "Date"
        Me.c_Date.Name = "c_Date"
        Me.c_Date.ReadOnly = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(14, 256)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(107, 16)
        Me.Label6.TabIndex = 90
        Me.Label6.Text = "Audit Trail List"
        '
        'dgvAuditList
        '
        Me.dgvAuditList.AllowUserToAddRows = False
        Me.dgvAuditList.AllowUserToDeleteRows = False
        Me.dgvAuditList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvAuditList.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvAuditList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAuditList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_Date, Me.UserID, Me.FieldChanged, Me.c_changedrowid, Me.Oldval, Me.newval, Me.ActionPerformed, Me.c_viewname})
        Me.dgvAuditList.Location = New System.Drawing.Point(14, 275)
        Me.dgvAuditList.Name = "dgvAuditList"
        Me.dgvAuditList.ReadOnly = True
        Me.dgvAuditList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAuditList.Size = New System.Drawing.Size(907, 139)
        Me.dgvAuditList.TabIndex = 77
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "&Close"
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label15.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label15.Font = New System.Drawing.Font("Stencil", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.Black
        Me.Label15.Location = New System.Drawing.Point(0, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(933, 28)
        Me.Label15.TabIndex = 92
        Me.Label15.Text = "Audit trail"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'AuditTrailForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(933, 426)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cbAction)
        Me.Controls.Add(Me.cbUserID)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dtpTo)
        Me.Controls.Add(Me.dtpFrom)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ToolStrip2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.dgvAuditList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "AuditTrailForm"
        Me.Text = "AuditTrailForm"
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        CType(Me.dgvAuditList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbAction As System.Windows.Forms.ComboBox
    Friend WithEvents cbUserID As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents c_viewname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActionPerformed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents newval As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Oldval As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_changedrowid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FieldChanged As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UserID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_Date As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dgvAuditList As System.Windows.Forms.DataGridView
    Friend WithEvents Label15 As System.Windows.Forms.Label
End Class
