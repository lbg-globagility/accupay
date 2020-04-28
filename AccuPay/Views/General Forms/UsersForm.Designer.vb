<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UsersForm
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Deleteg = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel5 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel6 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton12 = New System.Windows.Forms.ToolStripButton()
        Me.tsAuditTrail = New System.Windows.Forms.ToolStripButton()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.dgvPrivilege = New System.Windows.Forms.DataGridView()
        Me.Modules = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Addg = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Modifyg = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Readg = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.grpDetails = New System.Windows.Forms.GroupBox()
        Me.UserLevelComboBox = New System.Windows.Forms.ComboBox()
        Me.UserLevelLabel = New System.Windows.Forms.Label()
        Me.cboxposition = New System.Windows.Forms.ComboBox()
        Me.lblAddPosition = New System.Windows.Forms.LinkLabel()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtEmailAdd = New System.Windows.Forms.TextBox()
        Me.lblCompare = New System.Windows.Forms.Label()
        Me.lblRowID = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtConfirmPassword = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbPosition = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtMiddleName = New System.Windows.Forms.TextBox()
        Me.lblid = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFirstName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtLastName = New System.Windows.Forms.TextBox()
        Me.dgvUserList = New System.Windows.Forms.DataGridView()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.UserPrivilegeLabel = New System.Windows.Forms.Label()
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
        Me.c_userid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Position = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_lname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_fname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Mname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_emailadd = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserLevelDescriptionColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserLevelColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip3.SuspendLayout()
        CType(Me.dgvPrivilege, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDetails.SuspendLayout()
        CType(Me.dgvUserList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Deleteg
        '
        Me.Deleteg.HeaderText = "Delete"
        Me.Deleteg.Name = "Deleteg"
        Me.Deleteg.ReadOnly = True
        Me.Deleteg.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Deleteg.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Deleteg.Width = 50
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 22)
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(6, 22)
        '
        'ToolStripLabel5
        '
        Me.ToolStripLabel5.Name = "ToolStripLabel5"
        Me.ToolStripLabel5.Size = New System.Drawing.Size(0, 19)
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(6, 22)
        '
        'ToolStripLabel6
        '
        Me.ToolStripLabel6.Name = "ToolStripLabel6"
        Me.ToolStripLabel6.Size = New System.Drawing.Size(0, 19)
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(6, 22)
        '
        'ToolStrip3
        '
        Me.ToolStrip3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToolStrip3.AutoSize = False
        Me.ToolStrip3.BackColor = System.Drawing.Color.White
        Me.ToolStrip3.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.ToolStripSeparator11, Me.ToolStripLabel5, Me.btnSave, Me.ToolStripSeparator14, Me.ToolStripLabel6, Me.btnDelete, Me.ToolStripSeparator13, Me.btnCancel, Me.ToolStripSeparator15, Me.ToolStripButton12, Me.tsAuditTrail})
        Me.ToolStrip3.Location = New System.Drawing.Point(2, 24)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Size = New System.Drawing.Size(1122, 22)
        Me.ToolStrip3.TabIndex = 64
        Me.ToolStrip3.Text = "ToolStrip3"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(77, 19)
        Me.btnNew.Text = "&New User"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(77, 19)
        Me.btnSave.Text = "&Save User"
        '
        'btnDelete
        '
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(86, 19)
        Me.btnDelete.Text = "&Delete User"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 19)
        Me.btnCancel.Text = "&Cancel"
        '
        'ToolStripButton12
        '
        Me.ToolStripButton12.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton12.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton12.Name = "ToolStripButton12"
        Me.ToolStripButton12.Size = New System.Drawing.Size(56, 19)
        Me.ToolStripButton12.Text = "&Close"
        '
        'tsAuditTrail
        '
        Me.tsAuditTrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsAuditTrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsAuditTrail.Image = Global.AccuPay.My.Resources.Resources.audit_trail_icon
        Me.tsAuditTrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsAuditTrail.Name = "tsAuditTrail"
        Me.tsAuditTrail.Size = New System.Drawing.Size(23, 19)
        Me.tsAuditTrail.Text = "Audit Trail"
        Me.tsAuditTrail.ToolTipText = "Show audit trails"
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.Location = New System.Drawing.Point(90, 63)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(0, 13)
        Me.lblSaveMsg.TabIndex = 63
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(1124, 24)
        Me.Label8.TabIndex = 65
        Me.Label8.Text = "USER FORM"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvPrivilege
        '
        Me.dgvPrivilege.AllowUserToAddRows = False
        Me.dgvPrivilege.AllowUserToDeleteRows = False
        Me.dgvPrivilege.AllowUserToResizeRows = False
        Me.dgvPrivilege.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvPrivilege.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPrivilege.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvPrivilege.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPrivilege.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Modules, Me.Addg, Me.Modifyg, Me.Deleteg, Me.Readg})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrivilege.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvPrivilege.Location = New System.Drawing.Point(665, 243)
        Me.dgvPrivilege.Name = "dgvPrivilege"
        Me.dgvPrivilege.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPrivilege.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvPrivilege.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvPrivilege.Size = New System.Drawing.Size(420, 304)
        Me.dgvPrivilege.TabIndex = 1
        '
        'Modules
        '
        Me.Modules.HeaderText = "Module Name"
        Me.Modules.Name = "Modules"
        Me.Modules.ReadOnly = True
        Me.Modules.Width = 150
        '
        'Addg
        '
        Me.Addg.HeaderText = "Add"
        Me.Addg.Name = "Addg"
        Me.Addg.ReadOnly = True
        Me.Addg.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Addg.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Addg.Width = 50
        '
        'Modifyg
        '
        Me.Modifyg.HeaderText = "Modify"
        Me.Modifyg.Name = "Modifyg"
        Me.Modifyg.ReadOnly = True
        Me.Modifyg.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Modifyg.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Modifyg.Width = 50
        '
        'Readg
        '
        Me.Readg.HeaderText = "Read"
        Me.Readg.Name = "Readg"
        Me.Readg.ReadOnly = True
        Me.Readg.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Readg.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Readg.Width = 50
        '
        'grpDetails
        '
        Me.grpDetails.BackColor = System.Drawing.Color.White
        Me.grpDetails.Controls.Add(Me.UserLevelComboBox)
        Me.grpDetails.Controls.Add(Me.UserLevelLabel)
        Me.grpDetails.Controls.Add(Me.cboxposition)
        Me.grpDetails.Controls.Add(Me.lblAddPosition)
        Me.grpDetails.Controls.Add(Me.Label17)
        Me.grpDetails.Controls.Add(Me.Label16)
        Me.grpDetails.Controls.Add(Me.Label15)
        Me.grpDetails.Controls.Add(Me.Label14)
        Me.grpDetails.Controls.Add(Me.Label13)
        Me.grpDetails.Controls.Add(Me.Label12)
        Me.grpDetails.Controls.Add(Me.Label9)
        Me.grpDetails.Controls.Add(Me.txtEmailAdd)
        Me.grpDetails.Controls.Add(Me.lblCompare)
        Me.grpDetails.Controls.Add(Me.lblRowID)
        Me.grpDetails.Controls.Add(Me.Label4)
        Me.grpDetails.Controls.Add(Me.txtConfirmPassword)
        Me.grpDetails.Controls.Add(Me.Label5)
        Me.grpDetails.Controls.Add(Me.txtPassword)
        Me.grpDetails.Controls.Add(Me.Label6)
        Me.grpDetails.Controls.Add(Me.txtUserName)
        Me.grpDetails.Controls.Add(Me.Label7)
        Me.grpDetails.Controls.Add(Me.cmbPosition)
        Me.grpDetails.Controls.Add(Me.Label3)
        Me.grpDetails.Controls.Add(Me.txtMiddleName)
        Me.grpDetails.Controls.Add(Me.lblid)
        Me.grpDetails.Controls.Add(Me.Label2)
        Me.grpDetails.Controls.Add(Me.txtFirstName)
        Me.grpDetails.Controls.Add(Me.Label1)
        Me.grpDetails.Controls.Add(Me.txtLastName)
        Me.grpDetails.Location = New System.Drawing.Point(10, 49)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(790, 164)
        Me.grpDetails.TabIndex = 61
        Me.grpDetails.TabStop = False
        Me.grpDetails.Text = "Users Details"
        '
        'UserLevelComboBox
        '
        Me.UserLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.UserLevelComboBox.FormattingEnabled = True
        Me.UserLevelComboBox.Location = New System.Drawing.Point(256, 125)
        Me.UserLevelComboBox.Name = "UserLevelComboBox"
        Me.UserLevelComboBox.Size = New System.Drawing.Size(210, 21)
        Me.UserLevelComboBox.TabIndex = 44
        '
        'UserLevelLabel
        '
        Me.UserLevelLabel.AutoSize = True
        Me.UserLevelLabel.Location = New System.Drawing.Point(256, 109)
        Me.UserLevelLabel.Name = "UserLevelLabel"
        Me.UserLevelLabel.Size = New System.Drawing.Size(58, 13)
        Me.UserLevelLabel.TabIndex = 43
        Me.UserLevelLabel.Text = "User Level"
        '
        'cboxposition
        '
        Me.cboxposition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboxposition.FormattingEnabled = True
        Me.cboxposition.Location = New System.Drawing.Point(256, 77)
        Me.cboxposition.Name = "cboxposition"
        Me.cboxposition.Size = New System.Drawing.Size(210, 21)
        Me.cboxposition.TabIndex = 42
        '
        'lblAddPosition
        '
        Me.lblAddPosition.AutoSize = True
        Me.lblAddPosition.Location = New System.Drawing.Point(479, 85)
        Me.lblAddPosition.Name = "lblAddPosition"
        Me.lblAddPosition.Size = New System.Drawing.Size(66, 13)
        Me.lblAddPosition.TabIndex = 41
        Me.lblAddPosition.TabStop = True
        Me.lblAddPosition.Text = "Add Position"
        Me.lblAddPosition.Visible = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.Color.Red
        Me.Label17.Location = New System.Drawing.Point(637, 108)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(11, 13)
        Me.Label17.TabIndex = 40
        Me.Label17.Text = "*"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.Red
        Me.Label16.Location = New System.Drawing.Point(599, 61)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(11, 13)
        Me.Label16.TabIndex = 39
        Me.Label16.Text = "*"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.Color.Red
        Me.Label15.Location = New System.Drawing.Point(601, 22)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(11, 13)
        Me.Label15.TabIndex = 38
        Me.Label15.Text = "*"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ForeColor = System.Drawing.Color.Red
        Me.Label14.Location = New System.Drawing.Point(329, 62)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(11, 13)
        Me.Label14.TabIndex = 37
        Me.Label14.Text = "*"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Red
        Me.Label13.Location = New System.Drawing.Point(71, 61)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(11, 13)
        Me.Label13.TabIndex = 36
        Me.Label13.Text = "*"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Red
        Me.Label12.Location = New System.Drawing.Point(70, 24)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(11, 13)
        Me.Label12.TabIndex = 35
        Me.Label12.Text = "*"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(256, 22)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 13)
        Me.Label9.TabIndex = 34
        Me.Label9.Text = "Email Address"
        '
        'txtEmailAdd
        '
        Me.txtEmailAdd.Location = New System.Drawing.Point(256, 38)
        Me.txtEmailAdd.Name = "txtEmailAdd"
        Me.txtEmailAdd.Size = New System.Drawing.Size(210, 20)
        Me.txtEmailAdd.TabIndex = 20
        '
        'lblCompare
        '
        Me.lblCompare.AutoSize = True
        Me.lblCompare.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompare.Location = New System.Drawing.Point(13, 316)
        Me.lblCompare.Name = "lblCompare"
        Me.lblCompare.Size = New System.Drawing.Size(0, 18)
        Me.lblCompare.TabIndex = 32
        '
        'lblRowID
        '
        Me.lblRowID.AutoSize = True
        Me.lblRowID.Location = New System.Drawing.Point(77, 22)
        Me.lblRowID.Name = "lblRowID"
        Me.lblRowID.Size = New System.Drawing.Size(0, 13)
        Me.lblRowID.TabIndex = 31
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(547, 109)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 13)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "Confirm Password"
        '
        'txtConfirmPassword
        '
        Me.txtConfirmPassword.Location = New System.Drawing.Point(547, 125)
        Me.txtConfirmPassword.Name = "txtConfirmPassword"
        Me.txtConfirmPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtConfirmPassword.Size = New System.Drawing.Size(210, 20)
        Me.txtConfirmPassword.TabIndex = 24
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(547, 61)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 13)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "Password"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(547, 77)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtPassword.Size = New System.Drawing.Size(210, 20)
        Me.txtPassword.TabIndex = 23
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(547, 22)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(55, 13)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Username"
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(547, 38)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(210, 20)
        Me.txtUserName.TabIndex = 22
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(253, 61)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(75, 13)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "Position Name"
        '
        'cmbPosition
        '
        Me.cmbPosition.FormattingEnabled = True
        Me.cmbPosition.Location = New System.Drawing.Point(281, 104)
        Me.cmbPosition.Name = "cmbPosition"
        Me.cmbPosition.Size = New System.Drawing.Size(210, 21)
        Me.cmbPosition.TabIndex = 21
        Me.cmbPosition.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 109)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 13)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "Middle Name"
        '
        'txtMiddleName
        '
        Me.txtMiddleName.Location = New System.Drawing.Point(13, 125)
        Me.txtMiddleName.Name = "txtMiddleName"
        Me.txtMiddleName.Size = New System.Drawing.Size(210, 20)
        Me.txtMiddleName.TabIndex = 19
        '
        'lblid
        '
        Me.lblid.AutoSize = True
        Me.lblid.Location = New System.Drawing.Point(13, 38)
        Me.lblid.Name = "lblid"
        Me.lblid.Size = New System.Drawing.Size(0, 13)
        Me.lblid.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "First Name"
        '
        'txtFirstName
        '
        Me.txtFirstName.Location = New System.Drawing.Point(13, 77)
        Me.txtFirstName.Name = "txtFirstName"
        Me.txtFirstName.Size = New System.Drawing.Size(210, 20)
        Me.txtFirstName.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Last Name"
        '
        'txtLastName
        '
        Me.txtLastName.Location = New System.Drawing.Point(13, 38)
        Me.txtLastName.Name = "txtLastName"
        Me.txtLastName.Size = New System.Drawing.Size(210, 20)
        Me.txtLastName.TabIndex = 17
        '
        'dgvUserList
        '
        Me.dgvUserList.AllowUserToAddRows = False
        Me.dgvUserList.AllowUserToDeleteRows = False
        Me.dgvUserList.AllowUserToResizeRows = False
        Me.dgvUserList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvUserList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvUserList.BackgroundColor = System.Drawing.Color.White
        Me.dgvUserList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUserList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_userid, Me.c_Position, Me.c_lname, Me.c_fname, Me.c_Mname, Me.c_rowid, Me.c_emailadd, Me.UserLevelDescriptionColumn, Me.UserLevelColumn})
        Me.dgvUserList.Location = New System.Drawing.Point(12, 243)
        Me.dgvUserList.Name = "dgvUserList"
        Me.dgvUserList.ReadOnly = True
        Me.dgvUserList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvUserList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvUserList.Size = New System.Drawing.Size(647, 304)
        Me.dgvUserList.TabIndex = 62
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label10.Location = New System.Drawing.Point(8, 216)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(118, 24)
        Me.Label10.TabIndex = 35
        Me.Label10.Text = "USER LIST"
        '
        'UserPrivilegeLabel
        '
        Me.UserPrivilegeLabel.AutoSize = True
        Me.UserPrivilegeLabel.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.UserPrivilegeLabel.Location = New System.Drawing.Point(661, 216)
        Me.UserPrivilegeLabel.Name = "UserPrivilegeLabel"
        Me.UserPrivilegeLabel.Size = New System.Drawing.Size(182, 24)
        Me.UserPrivilegeLabel.TabIndex = 66
        Me.UserPrivilegeLabel.Text = "USER PRIVILEGE"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "User ID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 86
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Position"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 87
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 86
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 86
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "Middle Name"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 86
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Column1"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Visible = False
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Email Address"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Width = 87
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "User Level"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.Width = 86
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "Module Name"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Visible = False
        Me.DataGridViewTextBoxColumn9.Width = 150
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "Module Name"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.Width = 150
        '
        'c_userid
        '
        Me.c_userid.DataPropertyName = "UserID"
        Me.c_userid.HeaderText = "Username"
        Me.c_userid.Name = "c_userid"
        Me.c_userid.ReadOnly = True
        '
        'c_Position
        '
        Me.c_Position.DataPropertyName = "PositionName"
        Me.c_Position.HeaderText = "Position"
        Me.c_Position.Name = "c_Position"
        Me.c_Position.ReadOnly = True
        '
        'c_lname
        '
        Me.c_lname.DataPropertyName = "LastName"
        Me.c_lname.HeaderText = "Last Name"
        Me.c_lname.Name = "c_lname"
        Me.c_lname.ReadOnly = True
        '
        'c_fname
        '
        Me.c_fname.DataPropertyName = "FirstName"
        Me.c_fname.HeaderText = "First Name"
        Me.c_fname.Name = "c_fname"
        Me.c_fname.ReadOnly = True
        '
        'c_Mname
        '
        Me.c_Mname.DataPropertyName = "MiddleName"
        Me.c_Mname.HeaderText = "Middle Name"
        Me.c_Mname.Name = "c_Mname"
        Me.c_Mname.ReadOnly = True
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "Column1"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.ReadOnly = True
        Me.c_rowid.Visible = False
        '
        'c_emailadd
        '
        Me.c_emailadd.DataPropertyName = "EmailAddress"
        Me.c_emailadd.HeaderText = "Email Address"
        Me.c_emailadd.Name = "c_emailadd"
        Me.c_emailadd.ReadOnly = True
        '
        'UserLevelDescriptionColumn
        '
        Me.UserLevelDescriptionColumn.DataPropertyName = "UserLevelIndex"
        Me.UserLevelDescriptionColumn.HeaderText = "User Level"
        Me.UserLevelDescriptionColumn.Name = "UserLevelDescriptionColumn"
        Me.UserLevelDescriptionColumn.ReadOnly = True
        '
        'UserLevelColumn
        '
        Me.UserLevelColumn.DataPropertyName = "UserLevel"
        Me.UserLevelColumn.HeaderText = "User Level (Index)"
        Me.UserLevelColumn.Name = "UserLevelColumn"
        Me.UserLevelColumn.ReadOnly = True
        Me.UserLevelColumn.Visible = False
        '
        'UsersForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1124, 559)
        Me.ControlBox = False
        Me.Controls.Add(Me.UserPrivilegeLabel)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.dgvPrivilege)
        Me.Controls.Add(Me.ToolStrip3)
        Me.Controls.Add(Me.lblSaveMsg)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.grpDetails)
        Me.Controls.Add(Me.dgvUserList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.HelpButton = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UsersForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        CType(Me.dgvPrivilege, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDetails.ResumeLayout(False)
        Me.grpDetails.PerformLayout()
        CType(Me.dgvUserList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Deleteg As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel5 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel6 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStrip3 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton12 As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsAuditTrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents dgvPrivilege As System.Windows.Forms.DataGridView
    Friend WithEvents Modules As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Addg As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Modifyg As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Readg As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtEmailAdd As System.Windows.Forms.TextBox
    Friend WithEvents lblCompare As System.Windows.Forms.Label
    Friend WithEvents lblRowID As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtConfirmPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbPosition As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtMiddleName As System.Windows.Forms.TextBox
    Friend WithEvents lblid As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFirstName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtLastName As System.Windows.Forms.TextBox
    Friend WithEvents dgvUserList As System.Windows.Forms.DataGridView
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents UserPrivilegeLabel As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lblAddPosition As System.Windows.Forms.LinkLabel
    Friend WithEvents cboxposition As System.Windows.Forms.ComboBox
    Friend WithEvents UserLevelComboBox As ComboBox
    Friend WithEvents UserLevelLabel As Label
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
    Friend WithEvents c_userid As DataGridViewTextBoxColumn
    Friend WithEvents c_Position As DataGridViewTextBoxColumn
    Friend WithEvents c_lname As DataGridViewTextBoxColumn
    Friend WithEvents c_fname As DataGridViewTextBoxColumn
    Friend WithEvents c_Mname As DataGridViewTextBoxColumn
    Friend WithEvents c_rowid As DataGridViewTextBoxColumn
    Friend WithEvents c_emailadd As DataGridViewTextBoxColumn
    Friend WithEvents UserLevelDescriptionColumn As DataGridViewTextBoxColumn
    Friend WithEvents UserLevelColumn As DataGridViewTextBoxColumn
End Class
