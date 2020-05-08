<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PromotionTab
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PromotionTab))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip11 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel4 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton18 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton19 = New System.Windows.Forms.ToolStripButton()
        Me.btnUserActivity = New System.Windows.Forms.ToolStripButton()
        Me.txtReason = New System.Windows.Forms.TextBox()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label222 = New System.Windows.Forms.Label()
        Me.lblPeso = New System.Windows.Forms.Label()
        Me.lblCurrentSalary = New System.Windows.Forms.Label()
        Me.cboPositionTo = New System.Windows.Forms.ComboBox()
        Me.Label142 = New System.Windows.Forms.Label()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.lblPositionFrom = New System.Windows.Forms.TextBox()
        Me.Label84 = New System.Windows.Forms.Label()
        Me.cboCompensationChange = New System.Windows.Forms.ComboBox()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.txtNewSalary = New System.Windows.Forms.TextBox()
        Me.lblNewSalary = New System.Windows.Forms.Label()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.dtpEffectivityDate = New System.Windows.Forms.DateTimePicker()
        Me.txtEmployeeID = New System.Windows.Forms.TextBox()
        Me.txtFullname = New System.Windows.Forms.TextBox()
        Me.pbEmployee = New System.Windows.Forms.PictureBox()
        Me.dgvPromotions = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_PostionFrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_PositionTo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_EffectiveDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Compensation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_BasicPay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Reason = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblReqAsterisk = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolStrip11.SuspendLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvPromotions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip11
        '
        Me.ToolStrip11.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip11.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip11.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.ToolStripLabel4, Me.ToolStripSeparator7, Me.btnDelete, Me.ToolStripSeparator8, Me.btnCancel, Me.ToolStripButton18, Me.ToolStripButton19, Me.btnUserActivity})
        Me.ToolStrip11.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip11.Name = "ToolStrip11"
        Me.ToolStrip11.Size = New System.Drawing.Size(905, 25)
        Me.ToolStrip11.TabIndex = 330
        Me.ToolStrip11.Text = "ToolStrip11"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(111, 22)
        Me.btnNew.Text = "&New Promotion"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(111, 22)
        Me.btnSave.Text = "&Save Promotion"
        '
        'ToolStripLabel4
        '
        Me.ToolStripLabel4.AutoSize = False
        Me.ToolStripLabel4.Name = "ToolStripLabel4"
        Me.ToolStripLabel4.Size = New System.Drawing.Size(50, 22)
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'btnDelete
        '
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 22)
        Me.btnDelete.Text = "&Delete Promotion"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 25)
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'ToolStripButton18
        '
        Me.ToolStripButton18.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton18.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton18.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton18.Name = "ToolStripButton18"
        Me.ToolStripButton18.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton18.Text = "Close"
        '
        'ToolStripButton19
        '
        Me.ToolStripButton19.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton19.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton19.Image = Global.AccuPay.My.Resources.Resources.audit_trail_icon
        Me.ToolStripButton19.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton19.Name = "ToolStripButton19"
        Me.ToolStripButton19.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton19.Text = "ToolStripButton1"
        Me.ToolStripButton19.ToolTipText = "Show audit trails"
        '
        'btnUserActivity
        '
        Me.btnUserActivity.Image = CType(resources.GetObject("btnUserActivity.Image"), System.Drawing.Image)
        Me.btnUserActivity.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUserActivity.Name = "btnUserActivity"
        Me.btnUserActivity.Size = New System.Drawing.Size(93, 22)
        Me.btnUserActivity.Text = "User Activity"
        '
        'txtReason
        '
        Me.txtReason.Location = New System.Drawing.Point(519, 150)
        Me.txtReason.MaxLength = 2000
        Me.txtReason.Multiline = True
        Me.txtReason.Name = "txtReason"
        Me.txtReason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtReason.Size = New System.Drawing.Size(195, 122)
        Me.txtReason.TabIndex = 403
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(469, 158)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(44, 13)
        Me.Label57.TabIndex = 402
        Me.Label57.Text = "Reason"
        '
        'Label222
        '
        Me.Label222.AutoSize = True
        Me.Label222.Location = New System.Drawing.Point(210, 236)
        Me.Label222.Name = "Label222"
        Me.Label222.Size = New System.Drawing.Size(14, 13)
        Me.Label222.TabIndex = 401
        Me.Label222.Text = "₱"
        '
        'lblPeso
        '
        Me.lblPeso.AutoSize = True
        Me.lblPeso.Location = New System.Drawing.Point(210, 255)
        Me.lblPeso.Name = "lblPeso"
        Me.lblPeso.Size = New System.Drawing.Size(14, 13)
        Me.lblPeso.TabIndex = 400
        Me.lblPeso.Text = "₱"
        Me.lblPeso.Visible = False
        '
        'lblCurrentSalary
        '
        Me.lblCurrentSalary.AutoSize = True
        Me.lblCurrentSalary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentSalary.Location = New System.Drawing.Point(225, 236)
        Me.lblCurrentSalary.Name = "lblCurrentSalary"
        Me.lblCurrentSalary.Size = New System.Drawing.Size(14, 13)
        Me.lblCurrentSalary.TabIndex = 399
        Me.lblCurrentSalary.Text = "0"
        '
        'cboPositionTo
        '
        Me.cboPositionTo.DisplayMember = "Name"
        Me.cboPositionTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPositionTo.FormattingEnabled = True
        Me.cboPositionTo.Location = New System.Drawing.Point(228, 150)
        Me.cboPositionTo.Name = "cboPositionTo"
        Me.cboPositionTo.Size = New System.Drawing.Size(195, 21)
        Me.cboPositionTo.TabIndex = 388
        '
        'Label142
        '
        Me.Label142.AutoSize = True
        Me.Label142.Location = New System.Drawing.Point(94, 236)
        Me.Label142.Name = "Label142"
        Me.Label142.Size = New System.Drawing.Size(71, 13)
        Me.Label142.TabIndex = 398
        Me.Label142.Text = "Current salary"
        '
        'Label85
        '
        Me.Label85.AutoSize = True
        Me.Label85.Location = New System.Drawing.Point(94, 132)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(70, 13)
        Me.Label85.TabIndex = 392
        Me.Label85.Text = "Position From"
        '
        'lblPositionFrom
        '
        Me.lblPositionFrom.BackColor = System.Drawing.Color.White
        Me.lblPositionFrom.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblPositionFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblPositionFrom.ForeColor = System.Drawing.Color.Black
        Me.lblPositionFrom.Location = New System.Drawing.Point(228, 132)
        Me.lblPositionFrom.MaxLength = 50
        Me.lblPositionFrom.Name = "lblPositionFrom"
        Me.lblPositionFrom.ReadOnly = True
        Me.lblPositionFrom.Size = New System.Drawing.Size(516, 13)
        Me.lblPositionFrom.TabIndex = 397
        '
        'Label84
        '
        Me.Label84.AutoSize = True
        Me.Label84.Location = New System.Drawing.Point(94, 158)
        Me.Label84.Name = "Label84"
        Me.Label84.Size = New System.Drawing.Size(60, 13)
        Me.Label84.TabIndex = 393
        Me.Label84.Text = "Position To"
        '
        'cboCompensationChange
        '
        Me.cboCompensationChange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCompensationChange.FormattingEnabled = True
        Me.cboCompensationChange.Items.AddRange(New Object() {"Yes", "No"})
        Me.cboCompensationChange.Location = New System.Drawing.Point(228, 203)
        Me.cboCompensationChange.Name = "cboCompensationChange"
        Me.cboCompensationChange.Size = New System.Drawing.Size(195, 21)
        Me.cboCompensationChange.TabIndex = 390
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.Location = New System.Drawing.Point(94, 211)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(114, 13)
        Me.Label83.TabIndex = 394
        Me.Label83.Text = "Compensation Change"
        '
        'txtNewSalary
        '
        Me.txtNewSalary.Location = New System.Drawing.Point(228, 252)
        Me.txtNewSalary.Name = "txtNewSalary"
        Me.txtNewSalary.ShortcutsEnabled = False
        Me.txtNewSalary.Size = New System.Drawing.Size(195, 20)
        Me.txtNewSalary.TabIndex = 391
        '
        'lblNewSalary
        '
        Me.lblNewSalary.AutoSize = True
        Me.lblNewSalary.Location = New System.Drawing.Point(94, 260)
        Me.lblNewSalary.Name = "lblNewSalary"
        Me.lblNewSalary.Size = New System.Drawing.Size(59, 13)
        Me.lblNewSalary.TabIndex = 395
        Me.lblNewSalary.Text = "New salary"
        '
        'Label81
        '
        Me.Label81.AutoSize = True
        Me.Label81.Location = New System.Drawing.Point(94, 184)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(75, 13)
        Me.Label81.TabIndex = 396
        Me.Label81.Text = "Effective Date"
        '
        'dtpEffectivityDate
        '
        Me.dtpEffectivityDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectivityDate.Location = New System.Drawing.Point(228, 177)
        Me.dtpEffectivityDate.Name = "dtpEffectivityDate"
        Me.dtpEffectivityDate.Size = New System.Drawing.Size(195, 20)
        Me.dtpEffectivityDate.TabIndex = 389
        '
        'txtEmployeeID
        '
        Me.txtEmployeeID.BackColor = System.Drawing.Color.White
        Me.txtEmployeeID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeID.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmployeeID.Location = New System.Drawing.Point(111, 79)
        Me.txtEmployeeID.MaxLength = 50
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(784, 22)
        Me.txtEmployeeID.TabIndex = 406
        '
        'txtFullname
        '
        Me.txtFullname.BackColor = System.Drawing.Color.White
        Me.txtFullname.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullname.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullname.Location = New System.Drawing.Point(111, 39)
        Me.txtFullname.MaxLength = 250
        Me.txtFullname.Name = "txtFullname"
        Me.txtFullname.ReadOnly = True
        Me.txtFullname.Size = New System.Drawing.Size(784, 28)
        Me.txtFullname.TabIndex = 405
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(15, 39)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 88)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 404
        Me.pbEmployee.TabStop = False
        '
        'dgvPromotions
        '
        Me.dgvPromotions.AllowUserToAddRows = False
        Me.dgvPromotions.AllowUserToDeleteRows = False
        Me.dgvPromotions.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPromotions.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvPromotions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPromotions.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_PostionFrom, Me.c_PositionTo, Me.c_EffectiveDate, Me.c_Compensation, Me.c_BasicPay, Me.c_Reason})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPromotions.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvPromotions.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvPromotions.Location = New System.Drawing.Point(15, 278)
        Me.dgvPromotions.MultiSelect = False
        Me.dgvPromotions.Name = "dgvPromotions"
        Me.dgvPromotions.ReadOnly = True
        Me.dgvPromotions.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvPromotions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPromotions.Size = New System.Drawing.Size(880, 271)
        Me.dgvPromotions.TabIndex = 407
        '
        'c_PostionFrom
        '
        Me.c_PostionFrom.DataPropertyName = "PositionFrom"
        Me.c_PostionFrom.HeaderText = "Position From"
        Me.c_PostionFrom.Name = "c_PostionFrom"
        Me.c_PostionFrom.ReadOnly = True
        Me.c_PostionFrom.Width = 200
        '
        'c_PositionTo
        '
        Me.c_PositionTo.DataPropertyName = "PositionTo"
        Me.c_PositionTo.HeaderText = "Position To"
        Me.c_PositionTo.Name = "c_PositionTo"
        Me.c_PositionTo.ReadOnly = True
        Me.c_PositionTo.Width = 200
        '
        'c_EffectiveDate
        '
        Me.c_EffectiveDate.DataPropertyName = "EffectiveDate"
        Me.c_EffectiveDate.HeaderText = "Effective Date"
        Me.c_EffectiveDate.Name = "c_EffectiveDate"
        Me.c_EffectiveDate.ReadOnly = True
        '
        'c_Compensation
        '
        Me.c_Compensation.DataPropertyName = "CompensationToYesNo"
        Me.c_Compensation.HeaderText = "Compensation"
        Me.c_Compensation.Name = "c_Compensation"
        Me.c_Compensation.ReadOnly = True
        '
        'c_BasicPay
        '
        Me.c_BasicPay.DataPropertyName = "BasicPay"
        Me.c_BasicPay.HeaderText = "Basic Pay"
        Me.c_BasicPay.Name = "c_BasicPay"
        Me.c_BasicPay.ReadOnly = True
        Me.c_BasicPay.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'c_Reason
        '
        Me.c_Reason.DataPropertyName = "Reason"
        Me.c_Reason.HeaderText = "Reason"
        Me.c_Reason.Name = "c_Reason"
        Me.c_Reason.ReadOnly = True
        Me.c_Reason.Width = 200
        '
        'lblReqAsterisk
        '
        Me.lblReqAsterisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblReqAsterisk.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblReqAsterisk.Location = New System.Drawing.Point(195, 253)
        Me.lblReqAsterisk.Name = "lblReqAsterisk"
        Me.lblReqAsterisk.Size = New System.Drawing.Size(13, 13)
        Me.lblReqAsterisk.TabIndex = 515
        Me.lblReqAsterisk.Text = "*"
        Me.lblReqAsterisk.Visible = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(211, 153)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 513
        Me.Label3.Text = "*"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(211, 206)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 514
        Me.Label1.Text = "*"
        '
        'PromotionTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.lblReqAsterisk)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgvPromotions)
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullname)
        Me.Controls.Add(Me.pbEmployee)
        Me.Controls.Add(Me.txtReason)
        Me.Controls.Add(Me.Label57)
        Me.Controls.Add(Me.Label222)
        Me.Controls.Add(Me.lblPeso)
        Me.Controls.Add(Me.lblCurrentSalary)
        Me.Controls.Add(Me.cboPositionTo)
        Me.Controls.Add(Me.Label142)
        Me.Controls.Add(Me.Label85)
        Me.Controls.Add(Me.lblPositionFrom)
        Me.Controls.Add(Me.Label84)
        Me.Controls.Add(Me.cboCompensationChange)
        Me.Controls.Add(Me.Label83)
        Me.Controls.Add(Me.txtNewSalary)
        Me.Controls.Add(Me.lblNewSalary)
        Me.Controls.Add(Me.Label81)
        Me.Controls.Add(Me.dtpEffectivityDate)
        Me.Controls.Add(Me.ToolStrip11)
        Me.Name = "PromotionTab"
        Me.Size = New System.Drawing.Size(905, 552)
        Me.ToolStrip11.ResumeLayout(False)
        Me.ToolStrip11.PerformLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvPromotions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip11 As ToolStrip
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents btnSave As ToolStripButton
    Friend WithEvents ToolStripLabel4 As ToolStripLabel
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents btnCancel As ToolStripButton
    Friend WithEvents ToolStripButton18 As ToolStripButton
    Friend WithEvents ToolStripButton19 As ToolStripButton
    Friend WithEvents btnUserActivity As ToolStripButton
    Friend WithEvents txtReason As TextBox
    Friend WithEvents Label57 As Label
    Friend WithEvents Label222 As Label
    Friend WithEvents lblPeso As Label
    Friend WithEvents lblCurrentSalary As Label
    Friend WithEvents cboPositionTo As ComboBox
    Friend WithEvents Label142 As Label
    Friend WithEvents Label85 As Label
    Friend WithEvents lblPositionFrom As TextBox
    Friend WithEvents Label84 As Label
    Friend WithEvents cboCompensationChange As ComboBox
    Friend WithEvents Label83 As Label
    Friend WithEvents txtNewSalary As TextBox
    Friend WithEvents lblNewSalary As Label
    Friend WithEvents Label81 As Label
    Friend WithEvents dtpEffectivityDate As DateTimePicker
    Friend WithEvents txtEmployeeID As TextBox
    Public WithEvents txtFullname As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents dgvPromotions As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_PostionFrom As DataGridViewTextBoxColumn
    Friend WithEvents c_PositionTo As DataGridViewTextBoxColumn
    Friend WithEvents c_EffectiveDate As DataGridViewTextBoxColumn
    Friend WithEvents c_Compensation As DataGridViewTextBoxColumn
    Friend WithEvents c_BasicPay As DataGridViewTextBoxColumn
    Friend WithEvents c_Reason As DataGridViewTextBoxColumn
    Friend WithEvents lblReqAsterisk As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
End Class
