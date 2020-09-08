<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserPrivilegeForm
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
        Me.RoleGrid = New System.Windows.Forms.DataGridView()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.RoleNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.SaveButton = New System.Windows.Forms.ToolStripButton()
        Me.CancelButton = New System.Windows.Forms.ToolStripButton()
        Me.CloseButton = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.RolePermissionGrid = New System.Windows.Forms.DataGridView()
        Me.AllReadCheckBox = New System.Windows.Forms.CheckBox()
        Me.AllCreateCheckBox = New System.Windows.Forms.CheckBox()
        Me.AllDeleteCheckBox = New System.Windows.Forms.CheckBox()
        Me.AllUpdateCheckBox = New System.Windows.Forms.CheckBox()
        Me.lblforballoon = New System.Windows.Forms.Label()
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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReadColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.CreateColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.UpdateColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DeleteColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        CType(Me.RoleGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.RolePermissionGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
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
        'RoleGrid
        '
        Me.RoleGrid.AllowUserToAddRows = False
        Me.RoleGrid.AllowUserToDeleteRows = False
        Me.RoleGrid.AllowUserToOrderColumns = True
        Me.RoleGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RoleGrid.BackgroundColor = System.Drawing.Color.White
        Me.RoleGrid.ColumnHeadersHeight = 38
        Me.RoleGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.RoleGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column2})
        Me.RoleGrid.Location = New System.Drawing.Point(12, 145)
        Me.RoleGrid.MultiSelect = False
        Me.RoleGrid.Name = "RoleGrid"
        Me.RoleGrid.ReadOnly = True
        Me.RoleGrid.Size = New System.Drawing.Size(306, 277)
        Me.RoleGrid.TabIndex = 109
        '
        'Column2
        '
        Me.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Column2.DataPropertyName = "DisplayName"
        Me.Column2.HeaderText = "Roles"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.RolePermissionGrid)
        Me.Panel1.Controls.Add(Me.AllReadCheckBox)
        Me.Panel1.Controls.Add(Me.AllCreateCheckBox)
        Me.Panel1.Controls.Add(Me.AllDeleteCheckBox)
        Me.Panel1.Controls.Add(Me.AllUpdateCheckBox)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(325, 21)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(910, 451)
        Me.Panel1.TabIndex = 6
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.RoleNameTextBox)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Location = New System.Drawing.Point(13, 29)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(806, 30)
        Me.Panel3.TabIndex = 372
        '
        'RoleNameTextBox
        '
        Me.RoleNameTextBox.Location = New System.Drawing.Point(83, 4)
        Me.RoleNameTextBox.Name = "RoleNameTextBox"
        Me.RoleNameTextBox.Size = New System.Drawing.Size(235, 20)
        Me.RoleNameTextBox.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Role Name:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveButton, Me.CancelButton, Me.CloseButton, Me.tsbtnAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(893, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'SaveButton
        '
        Me.SaveButton.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(125, 22)
        Me.SaveButton.Text = "&Save User Privilege"
        '
        'CancelButton
        '
        Me.CancelButton.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.CancelButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(63, 22)
        Me.CancelButton.Text = "Cancel"
        '
        'CloseButton
        '
        Me.CloseButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.CloseButton.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.CloseButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(56, 22)
        Me.CloseButton.Text = "Close"
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
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(10, 70)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(131, 15)
        Me.Label4.TabIndex = 371
        Me.Label4.Text = "List of Role Permissions"
        '
        'RolePermissionGrid
        '
        Me.RolePermissionGrid.AllowUserToAddRows = False
        Me.RolePermissionGrid.AllowUserToDeleteRows = False
        Me.RolePermissionGrid.BackgroundColor = System.Drawing.Color.White
        Me.RolePermissionGrid.ColumnHeadersHeight = 38
        Me.RolePermissionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.RolePermissionGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column10, Me.ReadColumn, Me.CreateColumn, Me.UpdateColumn, Me.DeleteColumn})
        Me.RolePermissionGrid.Location = New System.Drawing.Point(13, 88)
        Me.RolePermissionGrid.MultiSelect = False
        Me.RolePermissionGrid.Name = "RolePermissionGrid"
        Me.RolePermissionGrid.Size = New System.Drawing.Size(806, 510)
        Me.RolePermissionGrid.TabIndex = 0
        '
        'AllReadCheckBox
        '
        Me.AllReadCheckBox.AutoSize = True
        Me.AllReadCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AllReadCheckBox.Location = New System.Drawing.Point(352, 65)
        Me.AllReadCheckBox.Name = "AllReadCheckBox"
        Me.AllReadCheckBox.Size = New System.Drawing.Size(71, 17)
        Me.AllReadCheckBox.TabIndex = 5
        Me.AllReadCheckBox.Text = "All Reads"
        Me.AllReadCheckBox.UseVisualStyleBackColor = True
        '
        'AllCreateCheckBox
        '
        Me.AllCreateCheckBox.AutoSize = True
        Me.AllCreateCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AllCreateCheckBox.Location = New System.Drawing.Point(476, 65)
        Me.AllCreateCheckBox.Name = "AllCreateCheckBox"
        Me.AllCreateCheckBox.Size = New System.Drawing.Size(76, 17)
        Me.AllCreateCheckBox.TabIndex = 2
        Me.AllCreateCheckBox.Text = "All Creates"
        Me.AllCreateCheckBox.UseVisualStyleBackColor = True
        '
        'AllDeleteCheckBox
        '
        Me.AllDeleteCheckBox.AutoSize = True
        Me.AllDeleteCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AllDeleteCheckBox.Location = New System.Drawing.Point(719, 65)
        Me.AllDeleteCheckBox.Name = "AllDeleteCheckBox"
        Me.AllDeleteCheckBox.Size = New System.Drawing.Size(76, 17)
        Me.AllDeleteCheckBox.TabIndex = 4
        Me.AllDeleteCheckBox.Text = "All Deletes"
        Me.AllDeleteCheckBox.UseVisualStyleBackColor = True
        '
        'AllUpdateCheckBox
        '
        Me.AllUpdateCheckBox.AutoSize = True
        Me.AllUpdateCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AllUpdateCheckBox.Location = New System.Drawing.Point(606, 65)
        Me.AllUpdateCheckBox.Name = "AllUpdateCheckBox"
        Me.AllUpdateCheckBox.Size = New System.Drawing.Size(80, 17)
        Me.AllUpdateCheckBox.TabIndex = 3
        Me.AllUpdateCheckBox.Text = "All Updates"
        Me.AllUpdateCheckBox.UseVisualStyleBackColor = True
        '
        'lblforballoon
        '
        Me.lblforballoon.AutoSize = True
        Me.lblforballoon.Location = New System.Drawing.Point(468, 38)
        Me.lblforballoon.Name = "lblforballoon"
        Me.lblforballoon.Size = New System.Drawing.Size(79, 13)
        Me.lblforballoon.TabIndex = 156
        Me.lblforballoon.Text = "label for baloon"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn2.HeaderText = "Position Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "ParentPositionID"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Visible = False
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "DivisionId"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Visible = False
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "Created"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.Visible = False
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "CreatedBy"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.Visible = False
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "LastUpd"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.Visible = False
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "LastUpdBy"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.Visible = False
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.Visible = False
        Me.DataGridViewTextBoxColumn9.Width = 127
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "Form Name"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.Width = 250
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.HeaderText = "view_RowID"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.RoleGrid)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(0, 21)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(325, 451)
        Me.Panel2.TabIndex = 157
        '
        'Column10
        '
        Me.Column10.DataPropertyName = "DisplayName"
        Me.Column10.HeaderText = "Permission"
        Me.Column10.Name = "Column10"
        Me.Column10.Width = 250
        '
        'ReadColumn
        '
        Me.ReadColumn.DataPropertyName = "Read"
        Me.ReadColumn.HeaderText = "Read"
        Me.ReadColumn.Name = "ReadColumn"
        Me.ReadColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ReadColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.ReadColumn.Width = 127
        '
        'CreateColumn
        '
        Me.CreateColumn.DataPropertyName = "Create"
        Me.CreateColumn.HeaderText = "Create"
        Me.CreateColumn.Name = "CreateColumn"
        Me.CreateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CreateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.CreateColumn.Width = 127
        '
        'UpdateColumn
        '
        Me.UpdateColumn.DataPropertyName = "Update"
        Me.UpdateColumn.HeaderText = "Update"
        Me.UpdateColumn.Name = "UpdateColumn"
        Me.UpdateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.UpdateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.UpdateColumn.Width = 128
        '
        'DeleteColumn
        '
        Me.DeleteColumn.DataPropertyName = "Delete"
        Me.DeleteColumn.HeaderText = "Delete"
        Me.DeleteColumn.Name = "DeleteColumn"
        Me.DeleteColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DeleteColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DeleteColumn.Width = 127
        '
        'UserPrivilegeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(183, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1235, 472)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label25)
        Me.Controls.Add(Me.lblforballoon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UserPrivilegeForm"
        CType(Me.RoleGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.RolePermissionGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents RoleGrid As System.Windows.Forms.DataGridView
    Friend WithEvents RolePermissionGrid As System.Windows.Forms.DataGridView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents SaveButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents CancelButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents CloseButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents AllCreateCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AllUpdateCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AllDeleteCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AllReadCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblforballoon As System.Windows.Forms.Label
    Friend WithEvents tsbtnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents RoleNameTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column10 As DataGridViewTextBoxColumn
    Friend WithEvents ReadColumn As DataGridViewCheckBoxColumn
    Friend WithEvents CreateColumn As DataGridViewCheckBoxColumn
    Friend WithEvents UpdateColumn As DataGridViewCheckBoxColumn
    Friend WithEvents DeleteColumn As DataGridViewCheckBoxColumn
End Class
