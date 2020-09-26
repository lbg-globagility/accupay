<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DisciplinaryActionTab
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DisciplinaryActionTab))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip8 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnPrintMemo = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnUserActivity = New System.Windows.Forms.ToolStripButton()
        Me.txtEmployeeID = New System.Windows.Forms.TextBox()
        Me.txtFullname = New System.Windows.Forms.TextBox()
        Me.pbEmployee = New System.Windows.Forms.PictureBox()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.cboAction = New System.Windows.Forms.ComboBox()
        Me.cboFinding = New System.Windows.Forms.ComboBox()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.lblAddFindingname = New System.Windows.Forms.LinkLabel()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.dtpEffectiveTo = New System.Windows.Forms.DateTimePicker()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.dtpEffectiveFrom = New System.Windows.Forms.DateTimePicker()
        Me.txtComments = New System.Windows.Forms.TextBox()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.dgvDisciplinaryList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_FindingName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_action = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_datefrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_dateto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_desc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_comment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip8.SuspendLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvDisciplinaryList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip8
        '
        Me.ToolStrip8.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip8.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip8.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.btnDelete, Me.btnCancel, Me.btnClose, Me.ToolStripSeparator1, Me.btnPrintMemo, Me.btnUserActivity})
        Me.ToolStrip8.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip8.Name = "ToolStrip8"
        Me.ToolStrip8.Size = New System.Drawing.Size(825, 25)
        Me.ToolStrip8.TabIndex = 327
        Me.ToolStrip8.Text = "ToolStrip8"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(51, 22)
        Me.btnNew.Text = "&New"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(51, 22)
        Me.btnSave.Text = "&Save"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'btnDelete
        '
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(60, 22)
        Me.btnDelete.Text = "&Delete"
        '
        'btnPrintMemo
        '
        Me.btnPrintMemo.Image = CType(resources.GetObject("btnPrintMemo.Image"), System.Drawing.Image)
        Me.btnPrintMemo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrintMemo.Name = "btnPrintMemo"
        Me.btnPrintMemo.Size = New System.Drawing.Size(90, 22)
        Me.btnPrintMemo.Text = "Print Memo"
        Me.btnPrintMemo.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "Close"
        '
        'btnUserActivity
        '
        Me.btnUserActivity.Image = CType(resources.GetObject("btnUserActivity.Image"), System.Drawing.Image)
        Me.btnUserActivity.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUserActivity.Name = "btnUserActivity"
        Me.btnUserActivity.Size = New System.Drawing.Size(93, 22)
        Me.btnUserActivity.Text = "User Activity"
        '
        'txtEmployeeID
        '
        Me.txtEmployeeID.BackColor = System.Drawing.Color.White
        Me.txtEmployeeID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeID.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmployeeID.Location = New System.Drawing.Point(117, 77)
        Me.txtEmployeeID.MaxLength = 50
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(689, 22)
        Me.txtEmployeeID.TabIndex = 354
        '
        'txtFullname
        '
        Me.txtFullname.BackColor = System.Drawing.Color.White
        Me.txtFullname.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullname.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullname.Location = New System.Drawing.Point(117, 37)
        Me.txtFullname.MaxLength = 250
        Me.txtFullname.Name = "txtFullname"
        Me.txtFullname.ReadOnly = True
        Me.txtFullname.Size = New System.Drawing.Size(668, 28)
        Me.txtFullname.TabIndex = 353
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(21, 37)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 88)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 352
        Me.pbEmployee.TabStop = False
        '
        'LinkLabel3
        '
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.Location = New System.Drawing.Point(163, 183)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(82, 13)
        Me.LinkLabel3.TabIndex = 519
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = "Add/Edit Action"
        '
        'cboAction
        '
        Me.cboAction.DisplayMember = "DisplayValue"
        Me.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAction.FormattingEnabled = True
        Me.cboAction.Location = New System.Drawing.Point(88, 199)
        Me.cboAction.MaxLength = 100
        Me.cboAction.Name = "cboAction"
        Me.cboAction.Size = New System.Drawing.Size(205, 21)
        Me.cboAction.TabIndex = 507
        '
        'cboFinding
        '
        Me.cboFinding.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboFinding.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboFinding.DisplayMember = "PartNo"
        Me.cboFinding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFinding.FormattingEnabled = True
        Me.cboFinding.Location = New System.Drawing.Point(88, 159)
        Me.cboFinding.Name = "cboFinding"
        Me.cboFinding.Size = New System.Drawing.Size(205, 21)
        Me.cboFinding.TabIndex = 506
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Location = New System.Drawing.Point(85, 222)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(101, 13)
        Me.Label44.TabIndex = 513
        Me.Label44.Text = "Effective Date From"
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(85, 143)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(72, 13)
        Me.Label43.TabIndex = 514
        Me.Label43.Text = "Finding Name"
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Location = New System.Drawing.Point(85, 261)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(91, 13)
        Me.Label45.TabIndex = 512
        Me.Label45.Text = "Effective Date To"
        '
        'lblAddFindingname
        '
        Me.lblAddFindingname.AutoSize = True
        Me.lblAddFindingname.Location = New System.Drawing.Point(163, 143)
        Me.lblAddFindingname.Name = "lblAddFindingname"
        Me.lblAddFindingname.Size = New System.Drawing.Size(117, 13)
        Me.lblAddFindingname.TabIndex = 518
        Me.lblAddFindingname.TabStop = True
        Me.lblAddFindingname.Text = "Add/Edit Finding Name"
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(85, 183)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(37, 13)
        Me.Label42.TabIndex = 515
        Me.Label42.Text = "Action"
        '
        'dtpEffectiveTo
        '
        Me.dtpEffectiveTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectiveTo.Location = New System.Drawing.Point(88, 278)
        Me.dtpEffectiveTo.Name = "dtpEffectiveTo"
        Me.dtpEffectiveTo.Size = New System.Drawing.Size(205, 20)
        Me.dtpEffectiveTo.TabIndex = 509
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(398, 161)
        Me.txtDescription.MaxLength = 2000
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(334, 59)
        Me.txtDescription.TabIndex = 510
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(395, 145)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(97, 13)
        Me.Label40.TabIndex = 517
        Me.Label40.Text = "Finding Description"
        '
        'dtpEffectiveFrom
        '
        Me.dtpEffectiveFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectiveFrom.Location = New System.Drawing.Point(88, 238)
        Me.dtpEffectiveFrom.Name = "dtpEffectiveFrom"
        Me.dtpEffectiveFrom.Size = New System.Drawing.Size(205, 20)
        Me.dtpEffectiveFrom.TabIndex = 508
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(398, 239)
        Me.txtComments.MaxLength = 500
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(334, 59)
        Me.txtComments.TabIndex = 511
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(395, 223)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(56, 13)
        Me.Label41.TabIndex = 516
        Me.Label41.Text = "Comments"
        '
        'dgvDisciplinaryList
        '
        Me.dgvDisciplinaryList.AllowUserToAddRows = False
        Me.dgvDisciplinaryList.AllowUserToDeleteRows = False
        Me.dgvDisciplinaryList.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDisciplinaryList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvDisciplinaryList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDisciplinaryList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_FindingName, Me.c_action, Me.c_datefrom, Me.c_dateto, Me.c_desc, Me.c_comment})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvDisciplinaryList.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgvDisciplinaryList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvDisciplinaryList.Location = New System.Drawing.Point(21, 304)
        Me.dgvDisciplinaryList.MultiSelect = False
        Me.dgvDisciplinaryList.Name = "dgvDisciplinaryList"
        Me.dgvDisciplinaryList.ReadOnly = True
        Me.dgvDisciplinaryList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvDisciplinaryList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDisciplinaryList.Size = New System.Drawing.Size(785, 325)
        Me.dgvDisciplinaryList.TabIndex = 520
        '
        'c_FindingName
        '
        Me.c_FindingName.DataPropertyName = "FindingName"
        Me.c_FindingName.HeaderText = "Finding Name"
        Me.c_FindingName.Name = "c_FindingName"
        Me.c_FindingName.ReadOnly = True
        Me.c_FindingName.Width = 117
        '
        'c_action
        '
        Me.c_action.DataPropertyName = "Action"
        Me.c_action.HeaderText = "Action"
        Me.c_action.Name = "c_action"
        Me.c_action.ReadOnly = True
        Me.c_action.Width = 117
        '
        'c_datefrom
        '
        Me.c_datefrom.DataPropertyName = "DateFrom"
        Me.c_datefrom.HeaderText = "Effective Date From"
        Me.c_datefrom.Name = "c_datefrom"
        Me.c_datefrom.ReadOnly = True
        Me.c_datefrom.Width = 117
        '
        'c_dateto
        '
        Me.c_dateto.DataPropertyName = "DateTo"
        Me.c_dateto.HeaderText = "Effective Date To"
        Me.c_dateto.Name = "c_dateto"
        Me.c_dateto.ReadOnly = True
        Me.c_dateto.Width = 117
        '
        'c_desc
        '
        Me.c_desc.DataPropertyName = "FindingDescription"
        Me.c_desc.HeaderText = "Finding Description"
        Me.c_desc.Name = "c_desc"
        Me.c_desc.ReadOnly = True
        Me.c_desc.Width = 117
        '
        'c_comment
        '
        Me.c_comment.DataPropertyName = "Comments"
        Me.c_comment.HeaderText = "Comments"
        Me.c_comment.Name = "c_comment"
        Me.c_comment.ReadOnly = True
        Me.c_comment.Width = 117
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label61.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label61.Location = New System.Drawing.Point(67, 161)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(18, 24)
        Me.Label61.TabIndex = 521
        Me.Label61.Text = "*"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(67, 201)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(18, 24)
        Me.Label1.TabIndex = 522
        Me.Label1.Text = "*"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "FindingName"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Finding Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 117
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "Action"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Action"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 117
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "DateFrom"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Effective Date From"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 117
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "DateTo"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Effective Date To"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 117
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "FindingDescription"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Finding Description"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.Width = 117
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "Comments"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Comments"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.Width = 117
        '
        'DisciplinaryActionTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label61)
        Me.Controls.Add(Me.dgvDisciplinaryList)
        Me.Controls.Add(Me.LinkLabel3)
        Me.Controls.Add(Me.cboAction)
        Me.Controls.Add(Me.cboFinding)
        Me.Controls.Add(Me.Label44)
        Me.Controls.Add(Me.Label43)
        Me.Controls.Add(Me.Label45)
        Me.Controls.Add(Me.lblAddFindingname)
        Me.Controls.Add(Me.Label42)
        Me.Controls.Add(Me.dtpEffectiveTo)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.Label40)
        Me.Controls.Add(Me.dtpEffectiveFrom)
        Me.Controls.Add(Me.txtComments)
        Me.Controls.Add(Me.Label41)
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullname)
        Me.Controls.Add(Me.pbEmployee)
        Me.Controls.Add(Me.ToolStrip8)
        Me.Name = "DisciplinaryActionTab"
        Me.Size = New System.Drawing.Size(825, 626)
        Me.ToolStrip8.ResumeLayout(False)
        Me.ToolStrip8.PerformLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvDisciplinaryList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip8 As ToolStrip
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents btnSave As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents btnPrintMemo As ToolStripButton
    Friend WithEvents btnCancel As ToolStripButton
    Friend WithEvents btnClose As ToolStripButton
    Friend WithEvents btnUserActivity As ToolStripButton
    Friend WithEvents txtEmployeeID As TextBox
    Public WithEvents txtFullname As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents LinkLabel3 As LinkLabel
    Friend WithEvents cboAction As ComboBox
    Friend WithEvents cboFinding As ComboBox
    Friend WithEvents Label44 As Label
    Friend WithEvents Label43 As Label
    Friend WithEvents Label45 As Label
    Friend WithEvents lblAddFindingname As LinkLabel
    Friend WithEvents Label42 As Label
    Friend WithEvents dtpEffectiveTo As DateTimePicker
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents Label40 As Label
    Friend WithEvents dtpEffectiveFrom As DateTimePicker
    Friend WithEvents txtComments As TextBox
    Friend WithEvents Label41 As Label
    Friend WithEvents dgvDisciplinaryList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label61 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents c_FindingName As DataGridViewTextBoxColumn
    Friend WithEvents c_action As DataGridViewTextBoxColumn
    Friend WithEvents c_datefrom As DataGridViewTextBoxColumn
    Friend WithEvents c_dateto As DataGridViewTextBoxColumn
    Friend WithEvents c_desc As DataGridViewTextBoxColumn
    Friend WithEvents c_comment As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
End Class
