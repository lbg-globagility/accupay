<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class userprivil
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
        Me.Label25 = New System.Windows.Forms.Label()
        Me.dgvposit = New System.Windows.Forms.DataGridView()
        Me.Last = New System.Windows.Forms.LinkLabel()
        Me.Nxt = New System.Windows.Forms.LinkLabel()
        Me.Prev = New System.Windows.Forms.LinkLabel()
        Me.First = New System.Windows.Forms.LinkLabel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label223 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dgvpositview = New System.Windows.Forms.DataGridView()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column12 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column13 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column14 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.view_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewUserPrivil = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveUserPrivil = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancelUserPrivil = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCloseUserPrivil = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.lblforballoon = New System.Windows.Forms.Label()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvposit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvpositview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(1235, 21)
        Me.Label25.TabIndex = 108
        Me.Label25.Text = "USER PRIVILEGE"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvposit
        '
        Me.dgvposit.AllowUserToAddRows = False
        Me.dgvposit.AllowUserToDeleteRows = False
        Me.dgvposit.AllowUserToOrderColumns = True
        Me.dgvposit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvposit.BackgroundColor = System.Drawing.Color.White
        Me.dgvposit.ColumnHeadersHeight = 38
        Me.dgvposit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvposit.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8})
        Me.dgvposit.Location = New System.Drawing.Point(12, 166)
        Me.dgvposit.MultiSelect = False
        Me.dgvposit.Name = "dgvposit"
        Me.dgvposit.ReadOnly = True
        Me.dgvposit.Size = New System.Drawing.Size(306, 279)
        Me.dgvposit.TabIndex = 109
        '
        'Last
        '
        Me.Last.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Last.AutoSize = True
        Me.Last.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Last.Location = New System.Drawing.Point(274, 448)
        Me.Last.Name = "Last"
        Me.Last.Size = New System.Drawing.Size(44, 15)
        Me.Last.TabIndex = 154
        Me.Last.TabStop = True
        Me.Last.Text = "Last>>"
        '
        'Nxt
        '
        Me.Nxt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Nxt.AutoSize = True
        Me.Nxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nxt.Location = New System.Drawing.Point(229, 448)
        Me.Nxt.Name = "Nxt"
        Me.Nxt.Size = New System.Drawing.Size(39, 15)
        Me.Nxt.TabIndex = 153
        Me.Nxt.TabStop = True
        Me.Nxt.Text = "Next>"
        '
        'Prev
        '
        Me.Prev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Prev.AutoSize = True
        Me.Prev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Prev.Location = New System.Drawing.Point(59, 448)
        Me.Prev.Name = "Prev"
        Me.Prev.Size = New System.Drawing.Size(38, 15)
        Me.Prev.TabIndex = 152
        Me.Prev.TabStop = True
        Me.Prev.Text = "<Prev"
        '
        'First
        '
        Me.First.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.First.AutoSize = True
        Me.First.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.First.Location = New System.Drawing.Point(9, 448)
        Me.First.Name = "First"
        Me.First.Size = New System.Drawing.Size(44, 15)
        Me.First.TabIndex = 151
        Me.First.TabStop = True
        Me.First.Text = "<<First"
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.TabControl1.ItemSize = New System.Drawing.Size(62, 25)
        Me.TabControl1.Location = New System.Drawing.Point(324, 24)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(911, 439)
        Me.TabControl1.TabIndex = 155
        '
        'TabPage1
        '
        Me.TabPage1.AutoScroll = True
        Me.TabPage1.Controls.Add(Me.Panel1)
        Me.TabPage1.Controls.Add(Me.ToolStrip1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(903, 406)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "USER PRIVILEGE               "
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.Label223)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.dgvpositview)
        Me.Panel1.Controls.Add(Me.CheckBox4)
        Me.Panel1.Controls.Add(Me.CheckBox1)
        Me.Panel1.Controls.Add(Me.CheckBox3)
        Me.Panel1.Controls.Add(Me.CheckBox2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(897, 375)
        Me.Panel1.TabIndex = 6
        '
        'Label223
        '
        Me.Label223.AutoSize = True
        Me.Label223.ForeColor = System.Drawing.Color.White
        Me.Label223.Location = New System.Drawing.Point(772, 557)
        Me.Label223.Name = "Label223"
        Me.Label223.Size = New System.Drawing.Size(37, 13)
        Me.Label223.TabIndex = 372
        Me.Label223.Text = "_____"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(0, 26)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(110, 15)
        Me.Label4.TabIndex = 371
        Me.Label4.Text = "List of Form Names"
        '
        'dgvpositview
        '
        Me.dgvpositview.BackgroundColor = System.Drawing.Color.White
        Me.dgvpositview.ColumnHeadersHeight = 38
        Me.dgvpositview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvpositview.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column9, Me.Column10, Me.Column11, Me.Column12, Me.Column13, Me.Column14, Me.view_RowID})
        Me.dgvpositview.Location = New System.Drawing.Point(3, 44)
        Me.dgvpositview.MultiSelect = False
        Me.dgvpositview.Name = "dgvpositview"
        Me.dgvpositview.Size = New System.Drawing.Size(806, 510)
        Me.dgvpositview.TabIndex = 0
        '
        'Column9
        '
        Me.Column9.HeaderText = "RowID"
        Me.Column9.Name = "Column9"
        Me.Column9.Visible = False
        Me.Column9.Width = 127
        '
        'Column10
        '
        Me.Column10.HeaderText = "Form Name"
        Me.Column10.Name = "Column10"
        Me.Column10.Width = 250
        '
        'Column11
        '
        Me.Column11.HeaderText = "Creates"
        Me.Column11.Name = "Column11"
        Me.Column11.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column11.Width = 127
        '
        'Column12
        '
        Me.Column12.HeaderText = "Updates"
        Me.Column12.Name = "Column12"
        Me.Column12.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column12.Width = 128
        '
        'Column13
        '
        Me.Column13.HeaderText = "Deletes"
        Me.Column13.Name = "Column13"
        Me.Column13.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column13.Width = 127
        '
        'Column14
        '
        Me.Column14.HeaderText = "Read Only"
        Me.Column14.Name = "Column14"
        Me.Column14.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column14.Width = 127
        '
        'view_RowID
        '
        Me.view_RowID.HeaderText = "view_RowID"
        Me.view_RowID.Name = "view_RowID"
        Me.view_RowID.Visible = False
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox4.Location = New System.Drawing.Point(709, 21)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(90, 17)
        Me.CheckBox4.TabIndex = 5
        Me.CheckBox4.Text = "All Read Only"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox1.Location = New System.Drawing.Point(342, 21)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(76, 17)
        Me.CheckBox1.TabIndex = 2
        Me.CheckBox1.Text = "All Creates"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox3.Location = New System.Drawing.Point(596, 21)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(76, 17)
        Me.CheckBox3.TabIndex = 4
        Me.CheckBox3.Text = "All Deletes"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox2.Location = New System.Drawing.Point(466, 21)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(80, 17)
        Me.CheckBox2.TabIndex = 3
        Me.CheckBox2.Text = "All Updates"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewUserPrivil, Me.tsbtnSaveUserPrivil, Me.tsbtnCancelUserPrivil, Me.tsbtnCloseUserPrivil, Me.tsbtnAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(897, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNewUserPrivil
        '
        Me.tsbtnNewUserPrivil.Image = Global.GoldWingsPayrollSys.My.Resources.Resources._new
        Me.tsbtnNewUserPrivil.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewUserPrivil.Name = "tsbtnNewUserPrivil"
        Me.tsbtnNewUserPrivil.Size = New System.Drawing.Size(125, 22)
        Me.tsbtnNewUserPrivil.Text = "&New User Privilege"
        '
        'tsbtnSaveUserPrivil
        '
        Me.tsbtnSaveUserPrivil.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Save
        Me.tsbtnSaveUserPrivil.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveUserPrivil.Name = "tsbtnSaveUserPrivil"
        Me.tsbtnSaveUserPrivil.Size = New System.Drawing.Size(125, 22)
        Me.tsbtnSaveUserPrivil.Text = "&Save User Privilege"
        '
        'tsbtnCancelUserPrivil
        '
        Me.tsbtnCancelUserPrivil.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.cancel1
        Me.tsbtnCancelUserPrivil.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancelUserPrivil.Name = "tsbtnCancelUserPrivil"
        Me.tsbtnCancelUserPrivil.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancelUserPrivil.Text = "Cancel"
        '
        'tsbtnCloseUserPrivil
        '
        Me.tsbtnCloseUserPrivil.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnCloseUserPrivil.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnCloseUserPrivil.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCloseUserPrivil.Name = "tsbtnCloseUserPrivil"
        Me.tsbtnCloseUserPrivil.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnCloseUserPrivil.Text = "Close"
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
        '
        'lblforballoon
        '
        Me.lblforballoon.AutoSize = True
        Me.lblforballoon.Location = New System.Drawing.Point(468, 38)
        Me.lblforballoon.Name = "lblforballoon"
        Me.lblforballoon.Size = New System.Drawing.Size(39, 13)
        Me.lblforballoon.TabIndex = 156
        Me.lblforballoon.Text = "Label1"
        '
        'Column1
        '
        Me.Column1.HeaderText = "RowID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Visible = False
        '
        'Column2
        '
        Me.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Column2.HeaderText = "Position Name"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.HeaderText = "ParentPositionID"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Visible = False
        '
        'Column4
        '
        Me.Column4.HeaderText = "DivisionId"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Visible = False
        '
        'Column5
        '
        Me.Column5.HeaderText = "Created"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Visible = False
        '
        'Column6
        '
        Me.Column6.HeaderText = "CreatedBy"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Visible = False
        '
        'Column7
        '
        Me.Column7.HeaderText = "LastUpd"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Visible = False
        '
        'Column8
        '
        Me.Column8.HeaderText = "LastUpdBy"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        Me.Column8.Visible = False
        '
        'userprivil
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(183, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1235, 472)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Last)
        Me.Controls.Add(Me.Nxt)
        Me.Controls.Add(Me.Prev)
        Me.Controls.Add(Me.First)
        Me.Controls.Add(Me.dgvposit)
        Me.Controls.Add(Me.Label25)
        Me.Controls.Add(Me.lblforballoon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "userprivil"
        CType(Me.dgvposit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvpositview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents dgvposit As System.Windows.Forms.DataGridView
    Friend WithEvents Last As System.Windows.Forms.LinkLabel
    Friend WithEvents Nxt As System.Windows.Forms.LinkLabel
    Friend WithEvents Prev As System.Windows.Forms.LinkLabel
    Friend WithEvents First As System.Windows.Forms.LinkLabel
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents dgvpositview As System.Windows.Forms.DataGridView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnNewUserPrivil As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSaveUserPrivil As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancelUserPrivil As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCloseUserPrivil As System.Windows.Forms.ToolStripButton
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblforballoon As System.Windows.Forms.Label
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Column12 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Column13 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Column14 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents view_RowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tsbtnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label223 As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
