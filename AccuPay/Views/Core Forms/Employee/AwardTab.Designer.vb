<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AwardTab
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AwardTab))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip5 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnUserActivity = New System.Windows.Forms.ToolStripButton()
        Me.txtEmployeeID = New System.Windows.Forms.TextBox()
        Me.txtFullname = New System.Windows.Forms.TextBox()
        Me.pbEmployee = New System.Windows.Forms.PictureBox()
        Me.grpSalary = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblAwardDate = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblAwardType = New System.Windows.Forms.Label()
        Me.dtpAwardDate = New System.Windows.Forms.DateTimePicker()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtAwardType = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.dgvAwards = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_AwardType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Date = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip5.SuspendLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSalary.SuspendLayout()
        CType(Me.dgvAwards, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip5
        '
        Me.ToolStrip5.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip5.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip5.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.btnDelete, Me.btnCancel, Me.ToolStripSeparator1, Me.btnClose, Me.btnUserActivity})
        Me.ToolStrip5.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip5.Name = "ToolStrip5"
        Me.ToolStrip5.Size = New System.Drawing.Size(856, 25)
        Me.ToolStrip5.TabIndex = 3
        Me.ToolStrip5.Text = "ToolStrip5"
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
        'btnDelete
        '
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(60, 22)
        Me.btnDelete.Text = "&Delete"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
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
        Me.btnUserActivity.Text = "&User Activity"
        '
        'txtEmployeeID
        '
        Me.txtEmployeeID.BackColor = System.Drawing.Color.White
        Me.txtEmployeeID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeID.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmployeeID.Location = New System.Drawing.Point(104, 72)
        Me.txtEmployeeID.MaxLength = 50
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(516, 22)
        Me.txtEmployeeID.TabIndex = 348
        '
        'txtFullname
        '
        Me.txtFullname.BackColor = System.Drawing.Color.White
        Me.txtFullname.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullname.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullname.Location = New System.Drawing.Point(104, 32)
        Me.txtFullname.MaxLength = 250
        Me.txtFullname.Name = "txtFullname"
        Me.txtFullname.ReadOnly = True
        Me.txtFullname.Size = New System.Drawing.Size(668, 28)
        Me.txtFullname.TabIndex = 347
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(8, 32)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 88)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 346
        Me.pbEmployee.TabStop = False
        '
        'grpSalary
        '
        Me.grpSalary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSalary.Controls.Add(Me.Label3)
        Me.grpSalary.Controls.Add(Me.lblAwardDate)
        Me.grpSalary.Controls.Add(Me.lblDescription)
        Me.grpSalary.Controls.Add(Me.lblAwardType)
        Me.grpSalary.Controls.Add(Me.dtpAwardDate)
        Me.grpSalary.Controls.Add(Me.txtDescription)
        Me.grpSalary.Controls.Add(Me.txtAwardType)
        Me.grpSalary.Location = New System.Drawing.Point(8, 135)
        Me.grpSalary.Name = "grpSalary"
        Me.grpSalary.Size = New System.Drawing.Size(840, 179)
        Me.grpSalary.TabIndex = 349
        Me.grpSalary.TabStop = False
        Me.grpSalary.Text = "Award"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(183, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 508
        Me.Label3.Text = "*"
        '
        'lblAwardDate
        '
        Me.lblAwardDate.AutoSize = True
        Me.lblAwardDate.Location = New System.Drawing.Point(100, 139)
        Me.lblAwardDate.Name = "lblAwardDate"
        Me.lblAwardDate.Size = New System.Drawing.Size(63, 13)
        Me.lblAwardDate.TabIndex = 5
        Me.lblAwardDate.Text = "Award Date"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(100, 58)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(60, 13)
        Me.lblDescription.TabIndex = 4
        Me.lblDescription.Text = "Description"
        '
        'lblAwardType
        '
        Me.lblAwardType.AutoSize = True
        Me.lblAwardType.Location = New System.Drawing.Point(100, 32)
        Me.lblAwardType.Name = "lblAwardType"
        Me.lblAwardType.Size = New System.Drawing.Size(64, 13)
        Me.lblAwardType.TabIndex = 3
        Me.lblAwardType.Text = "Award Type"
        '
        'dtpAwardDate
        '
        Me.dtpAwardDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpAwardDate.Location = New System.Drawing.Point(202, 135)
        Me.dtpAwardDate.Name = "dtpAwardDate"
        Me.dtpAwardDate.Size = New System.Drawing.Size(190, 20)
        Me.dtpAwardDate.TabIndex = 2
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(202, 55)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(190, 74)
        Me.txtDescription.TabIndex = 1
        '
        'txtAwardType
        '
        Me.txtAwardType.Location = New System.Drawing.Point(202, 29)
        Me.txtAwardType.Name = "txtAwardType"
        Me.txtAwardType.Size = New System.Drawing.Size(190, 20)
        Me.txtAwardType.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(61, 4)
        '
        'dgvAwards
        '
        Me.dgvAwards.AllowUserToAddRows = False
        Me.dgvAwards.AllowUserToDeleteRows = False
        Me.dgvAwards.AllowUserToResizeRows = False
        Me.dgvAwards.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvAwards.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvAwards.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvAwards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAwards.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_AwardType, Me.c_Description, Me.c_Date})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAwards.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvAwards.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvAwards.Location = New System.Drawing.Point(8, 320)
        Me.dgvAwards.MultiSelect = False
        Me.dgvAwards.Name = "dgvAwards"
        Me.dgvAwards.ReadOnly = True
        Me.dgvAwards.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvAwards.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAwards.Size = New System.Drawing.Size(840, 229)
        Me.dgvAwards.TabIndex = 347
        '
        'c_AwardType
        '
        Me.c_AwardType.DataPropertyName = "AwardType"
        Me.c_AwardType.HeaderText = "Award Type"
        Me.c_AwardType.MinimumWidth = 150
        Me.c_AwardType.Name = "c_AwardType"
        Me.c_AwardType.ReadOnly = True
        Me.c_AwardType.Width = 150
        '
        'c_Description
        '
        Me.c_Description.DataPropertyName = "AwardDescription"
        Me.c_Description.HeaderText = "Description"
        Me.c_Description.MinimumWidth = 200
        Me.c_Description.Name = "c_Description"
        Me.c_Description.ReadOnly = True
        Me.c_Description.Width = 200
        '
        'c_Date
        '
        Me.c_Date.DataPropertyName = "AwardDate"
        Me.c_Date.HeaderText = "Date"
        Me.c_Date.Name = "c_Date"
        Me.c_Date.ReadOnly = True
        '
        'AwardTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.dgvAwards)
        Me.Controls.Add(Me.grpSalary)
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullname)
        Me.Controls.Add(Me.pbEmployee)
        Me.Controls.Add(Me.ToolStrip5)
        Me.Name = "AwardTab"
        Me.Size = New System.Drawing.Size(856, 552)
        Me.ToolStrip5.ResumeLayout(False)
        Me.ToolStrip5.PerformLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSalary.ResumeLayout(False)
        Me.grpSalary.PerformLayout()
        CType(Me.dgvAwards, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip5 As ToolStrip
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents btnSave As ToolStripButton
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents btnCancel As ToolStripButton
    Friend WithEvents btnClose As ToolStripButton
    Friend WithEvents btnUserActivity As ToolStripButton
    Friend WithEvents txtEmployeeID As TextBox
    Public WithEvents txtFullname As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents grpSalary As GroupBox
    Friend WithEvents txtAwardType As TextBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents dtpAwardDate As DateTimePicker
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents lblAwardType As Label
    Friend WithEvents lblDescription As Label
    Friend WithEvents lblAwardDate As Label
    Friend WithEvents dgvAwards As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_AwardType As DataGridViewTextBoxColumn
    Friend WithEvents c_Description As DataGridViewTextBoxColumn
    Friend WithEvents c_Date As DataGridViewTextBoxColumn
    Friend WithEvents Label3 As Label
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
End Class
