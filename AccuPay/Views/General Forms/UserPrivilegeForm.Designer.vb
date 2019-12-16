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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.dgvPositionList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_Position = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grpGeneral = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.dgvGeneral = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_FormName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_add = New DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn()
        Me.c_save = New DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn()
        Me.c_edit = New DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn()
        Me.c_delete = New DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn()
        Me.GroupPanel2 = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.CheckBox5 = New System.Windows.Forms.CheckBox()
        Me.CheckBox6 = New System.Windows.Forms.CheckBox()
        Me.CheckBox7 = New System.Windows.Forms.CheckBox()
        Me.CheckBox8 = New System.Windows.Forms.CheckBox()
        Me.dgvHRIS = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewCheckBoxXColumn1 = New DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn()
        Me.DataGridViewCheckBoxXColumn2 = New DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn()
        Me.DataGridViewCheckBoxXColumn3 = New DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn()
        Me.DataGridViewCheckBoxXColumn4 = New DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvPositionList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpGeneral.SuspendLayout()
        CType(Me.dgvGeneral, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupPanel2.SuspendLayout()
        CType(Me.dgvHRIS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Silver
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(1181, 24)
        Me.Label8.TabIndex = 66
        Me.Label8.Text = "USER PRIVILEGE"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.ToolStripSeparator1, Me.ToolStripLabel1, Me.ToolStripSeparator3, Me.btnCancel, Me.btnClose})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1181, 25)
        Me.ToolStrip1.TabIndex = 309
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.GoldWingsPayrollSys.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(125, 22)
        Me.btnNew.Text = "&New User Privilege"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(125, 22)
        Me.btnSave.Text = "&Save User Privilege"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(50, 22)
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "&Close"
        '
        'dgvPositionList
        '
        Me.dgvPositionList.AllowUserToAddRows = False
        Me.dgvPositionList.AllowUserToDeleteRows = False
        Me.dgvPositionList.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvPositionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPositionList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_Position, Me.c_rowID})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPositionList.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvPositionList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvPositionList.Location = New System.Drawing.Point(12, 73)
        Me.dgvPositionList.Name = "dgvPositionList"
        Me.dgvPositionList.ReadOnly = True
        Me.dgvPositionList.Size = New System.Drawing.Size(245, 484)
        Me.dgvPositionList.TabIndex = 310
        '
        'c_Position
        '
        Me.c_Position.HeaderText = "Position Name"
        Me.c_Position.Name = "c_Position"
        Me.c_Position.ReadOnly = True
        Me.c_Position.Width = 200
        '
        'c_rowID
        '
        Me.c_rowID.HeaderText = "Row ID"
        Me.c_rowID.Name = "c_rowID"
        Me.c_rowID.ReadOnly = True
        Me.c_rowID.Visible = False
        '
        'grpGeneral
        '
        Me.grpGeneral.CanvasColor = System.Drawing.SystemColors.Control
        Me.grpGeneral.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Windows7
        Me.grpGeneral.Controls.Add(Me.Button2)
        Me.grpGeneral.Controls.Add(Me.Button1)
        Me.grpGeneral.Controls.Add(Me.CheckBox4)
        Me.grpGeneral.Controls.Add(Me.CheckBox3)
        Me.grpGeneral.Controls.Add(Me.CheckBox2)
        Me.grpGeneral.Controls.Add(Me.CheckBox1)
        Me.grpGeneral.Controls.Add(Me.dgvGeneral)
        Me.grpGeneral.Location = New System.Drawing.Point(263, 73)
        Me.grpGeneral.Name = "grpGeneral"
        Me.grpGeneral.Size = New System.Drawing.Size(443, 239)
        '
        '
        '
        Me.grpGeneral.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.grpGeneral.Style.BackColorGradientAngle = 90
        Me.grpGeneral.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.grpGeneral.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.grpGeneral.Style.BorderBottomWidth = 1
        Me.grpGeneral.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.grpGeneral.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.grpGeneral.Style.BorderLeftWidth = 1
        Me.grpGeneral.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.grpGeneral.Style.BorderRightWidth = 1
        Me.grpGeneral.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.grpGeneral.Style.BorderTopWidth = 1
        Me.grpGeneral.Style.Class = ""
        Me.grpGeneral.Style.CornerDiameter = 4
        Me.grpGeneral.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.grpGeneral.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.grpGeneral.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.grpGeneral.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.grpGeneral.StyleMouseDown.Class = ""
        Me.grpGeneral.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.grpGeneral.StyleMouseOver.Class = ""
        Me.grpGeneral.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.grpGeneral.TabIndex = 313
        Me.grpGeneral.Text = "General"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(351, 184)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 317
        Me.Button2.Text = "Select All"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(270, 184)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 316
        Me.Button1.Text = "Deselect All"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.Location = New System.Drawing.Point(305, 7)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(57, 17)
        Me.CheckBox4.TabIndex = 315
        Me.CheckBox4.Text = "Delete"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Location = New System.Drawing.Point(246, 7)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(61, 17)
        Me.CheckBox3.TabIndex = 314
        Me.CheckBox3.Text = "Update"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(195, 7)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(51, 17)
        Me.CheckBox2.TabIndex = 313
        Me.CheckBox2.Text = "Save"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(146, 7)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(45, 17)
        Me.CheckBox1.TabIndex = 312
        Me.CheckBox1.Text = "Add"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'dgvGeneral
        '
        Me.dgvGeneral.AllowUserToAddRows = False
        Me.dgvGeneral.AllowUserToDeleteRows = False
        Me.dgvGeneral.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvGeneral.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGeneral.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_FormName, Me.c_add, Me.c_save, Me.c_edit, Me.c_delete})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvGeneral.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvGeneral.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvGeneral.Location = New System.Drawing.Point(3, 30)
        Me.dgvGeneral.Name = "dgvGeneral"
        Me.dgvGeneral.Size = New System.Drawing.Size(423, 148)
        Me.dgvGeneral.TabIndex = 311
        '
        'c_FormName
        '
        Me.c_FormName.HeaderText = "Form Name"
        Me.c_FormName.Name = "c_FormName"
        '
        'c_add
        '
        Me.c_add.Checked = True
        Me.c_add.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.c_add.CheckValue = Nothing
        Me.c_add.HeaderText = "Add"
        Me.c_add.Name = "c_add"
        Me.c_add.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_add.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.c_add.Width = 50
        '
        'c_save
        '
        Me.c_save.Checked = True
        Me.c_save.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.c_save.CheckValue = "N"
        Me.c_save.HeaderText = "Save"
        Me.c_save.Name = "c_save"
        Me.c_save.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_save.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.c_save.Width = 50
        '
        'c_edit
        '
        Me.c_edit.Checked = True
        Me.c_edit.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.c_edit.CheckValue = "N"
        Me.c_edit.HeaderText = "Update"
        Me.c_edit.Name = "c_edit"
        Me.c_edit.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_edit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.c_edit.Width = 60
        '
        'c_delete
        '
        Me.c_delete.Checked = True
        Me.c_delete.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.c_delete.CheckValue = "N"
        Me.c_delete.HeaderText = "Delete"
        Me.c_delete.Name = "c_delete"
        Me.c_delete.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_delete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.c_delete.Width = 60
        '
        'GroupPanel2
        '
        Me.GroupPanel2.CanvasColor = System.Drawing.SystemColors.Control
        Me.GroupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Windows7
        Me.GroupPanel2.Controls.Add(Me.Button3)
        Me.GroupPanel2.Controls.Add(Me.Button4)
        Me.GroupPanel2.Controls.Add(Me.CheckBox5)
        Me.GroupPanel2.Controls.Add(Me.CheckBox6)
        Me.GroupPanel2.Controls.Add(Me.CheckBox7)
        Me.GroupPanel2.Controls.Add(Me.CheckBox8)
        Me.GroupPanel2.Controls.Add(Me.dgvHRIS)
        Me.GroupPanel2.Location = New System.Drawing.Point(263, 318)
        Me.GroupPanel2.Name = "GroupPanel2"
        Me.GroupPanel2.Size = New System.Drawing.Size(443, 239)
        '
        '
        '
        Me.GroupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.GroupPanel2.Style.BackColorGradientAngle = 90
        Me.GroupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.GroupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel2.Style.BorderBottomWidth = 1
        Me.GroupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.GroupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel2.Style.BorderLeftWidth = 1
        Me.GroupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel2.Style.BorderRightWidth = 1
        Me.GroupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel2.Style.BorderTopWidth = 1
        Me.GroupPanel2.Style.Class = ""
        Me.GroupPanel2.Style.CornerDiameter = 4
        Me.GroupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.GroupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.GroupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.GroupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.GroupPanel2.StyleMouseDown.Class = ""
        Me.GroupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.GroupPanel2.StyleMouseOver.Class = ""
        Me.GroupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel2.TabIndex = 318
        Me.GroupPanel2.Text = "HRIS"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(351, 184)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 317
        Me.Button3.Text = "Select All"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(270, 184)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 316
        Me.Button4.Text = "Deselect All"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'CheckBox5
        '
        Me.CheckBox5.AutoSize = True
        Me.CheckBox5.Location = New System.Drawing.Point(305, 7)
        Me.CheckBox5.Name = "CheckBox5"
        Me.CheckBox5.Size = New System.Drawing.Size(57, 17)
        Me.CheckBox5.TabIndex = 315
        Me.CheckBox5.Text = "Delete"
        Me.CheckBox5.UseVisualStyleBackColor = True
        '
        'CheckBox6
        '
        Me.CheckBox6.AutoSize = True
        Me.CheckBox6.Location = New System.Drawing.Point(246, 7)
        Me.CheckBox6.Name = "CheckBox6"
        Me.CheckBox6.Size = New System.Drawing.Size(61, 17)
        Me.CheckBox6.TabIndex = 314
        Me.CheckBox6.Text = "Update"
        Me.CheckBox6.UseVisualStyleBackColor = True
        '
        'CheckBox7
        '
        Me.CheckBox7.AutoSize = True
        Me.CheckBox7.Location = New System.Drawing.Point(195, 7)
        Me.CheckBox7.Name = "CheckBox7"
        Me.CheckBox7.Size = New System.Drawing.Size(51, 17)
        Me.CheckBox7.TabIndex = 313
        Me.CheckBox7.Text = "Save"
        Me.CheckBox7.UseVisualStyleBackColor = True
        '
        'CheckBox8
        '
        Me.CheckBox8.AutoSize = True
        Me.CheckBox8.Location = New System.Drawing.Point(146, 7)
        Me.CheckBox8.Name = "CheckBox8"
        Me.CheckBox8.Size = New System.Drawing.Size(45, 17)
        Me.CheckBox8.TabIndex = 312
        Me.CheckBox8.Text = "Add"
        Me.CheckBox8.UseVisualStyleBackColor = True
        '
        'dgvHRIS
        '
        Me.dgvHRIS.AllowUserToAddRows = False
        Me.dgvHRIS.AllowUserToDeleteRows = False
        Me.dgvHRIS.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvHRIS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvHRIS.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewCheckBoxXColumn1, Me.DataGridViewCheckBoxXColumn2, Me.DataGridViewCheckBoxXColumn3, Me.DataGridViewCheckBoxXColumn4})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvHRIS.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvHRIS.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvHRIS.Location = New System.Drawing.Point(3, 30)
        Me.dgvHRIS.Name = "dgvHRIS"
        Me.dgvHRIS.Size = New System.Drawing.Size(423, 148)
        Me.dgvHRIS.TabIndex = 311
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Form Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewCheckBoxXColumn1
        '
        Me.DataGridViewCheckBoxXColumn1.Checked = True
        Me.DataGridViewCheckBoxXColumn1.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.DataGridViewCheckBoxXColumn1.CheckValue = Nothing
        Me.DataGridViewCheckBoxXColumn1.HeaderText = "Add"
        Me.DataGridViewCheckBoxXColumn1.Name = "DataGridViewCheckBoxXColumn1"
        Me.DataGridViewCheckBoxXColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCheckBoxXColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxXColumn1.Width = 50
        '
        'DataGridViewCheckBoxXColumn2
        '
        Me.DataGridViewCheckBoxXColumn2.Checked = True
        Me.DataGridViewCheckBoxXColumn2.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.DataGridViewCheckBoxXColumn2.CheckValue = "N"
        Me.DataGridViewCheckBoxXColumn2.HeaderText = "Save"
        Me.DataGridViewCheckBoxXColumn2.Name = "DataGridViewCheckBoxXColumn2"
        Me.DataGridViewCheckBoxXColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCheckBoxXColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxXColumn2.Width = 50
        '
        'DataGridViewCheckBoxXColumn3
        '
        Me.DataGridViewCheckBoxXColumn3.Checked = True
        Me.DataGridViewCheckBoxXColumn3.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.DataGridViewCheckBoxXColumn3.CheckValue = "N"
        Me.DataGridViewCheckBoxXColumn3.HeaderText = "Update"
        Me.DataGridViewCheckBoxXColumn3.Name = "DataGridViewCheckBoxXColumn3"
        Me.DataGridViewCheckBoxXColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCheckBoxXColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxXColumn3.Width = 60
        '
        'DataGridViewCheckBoxXColumn4
        '
        Me.DataGridViewCheckBoxXColumn4.Checked = True
        Me.DataGridViewCheckBoxXColumn4.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.DataGridViewCheckBoxXColumn4.CheckValue = "N"
        Me.DataGridViewCheckBoxXColumn4.HeaderText = "Delete"
        Me.DataGridViewCheckBoxXColumn4.Name = "DataGridViewCheckBoxXColumn4"
        Me.DataGridViewCheckBoxXColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCheckBoxXColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxXColumn4.Width = 60
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.Location = New System.Drawing.Point(134, 57)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(39, 13)
        Me.lblSaveMsg.TabIndex = 319
        Me.lblSaveMsg.Text = "Label1"
        '
        'UserPrivilegeForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(1181, 565)
        Me.Controls.Add(Me.lblSaveMsg)
        Me.Controls.Add(Me.GroupPanel2)
        Me.Controls.Add(Me.grpGeneral)
        Me.Controls.Add(Me.dgvPositionList)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Label8)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "UserPrivilegeForm"
        Me.Text = "UserPreviligeForm"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvPositionList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpGeneral.ResumeLayout(False)
        Me.grpGeneral.PerformLayout()
        CType(Me.dgvGeneral, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupPanel2.ResumeLayout(False)
        Me.GroupPanel2.PerformLayout()
        CType(Me.dgvHRIS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvPositionList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_Position As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_rowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents dgvGeneral As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_FormName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_add As DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn
    Friend WithEvents c_save As DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn
    Friend WithEvents c_edit As DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn
    Friend WithEvents c_delete As DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents CheckBox5 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox6 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox7 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox8 As System.Windows.Forms.CheckBox
    Friend WithEvents dgvHRIS As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewCheckBoxXColumn1 As DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn
    Friend WithEvents DataGridViewCheckBoxXColumn2 As DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn
    Friend WithEvents DataGridViewCheckBoxXColumn3 As DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn
    Friend WithEvents DataGridViewCheckBoxXColumn4 As DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
    Private WithEvents grpGeneral As DevComponents.DotNetBar.Controls.GroupPanel
    Private WithEvents GroupPanel2 As DevComponents.DotNetBar.Controls.GroupPanel
End Class
