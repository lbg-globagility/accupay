<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CertificationTab
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CertificationTab))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip20 = New System.Windows.Forms.ToolStrip()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnUserActivity = New System.Windows.Forms.ToolStripButton()
        Me.txtEmployeeID = New System.Windows.Forms.TextBox()
        Me.txtFullname = New System.Windows.Forms.TextBox()
        Me.pbEmployee = New System.Windows.Forms.PictureBox()
        Me.grpSalary = New System.Windows.Forms.GroupBox()
        Me.dtpExpirationDate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblComments = New System.Windows.Forms.Label()
        Me.lblDateExp = New System.Windows.Forms.Label()
        Me.lblDateIssued = New System.Windows.Forms.Label()
        Me.lblCertNo = New System.Windows.Forms.Label()
        Me.lblIssuingAuthority = New System.Windows.Forms.Label()
        Me.lblCertType = New System.Windows.Forms.Label()
        Me.txtComments = New System.Windows.Forms.TextBox()
        Me.dtpIssueDate = New System.Windows.Forms.DateTimePicker()
        Me.txtCertificationNo = New System.Windows.Forms.TextBox()
        Me.txtIssuingAuthority = New System.Windows.Forms.TextBox()
        Me.txtCertificationType = New System.Windows.Forms.TextBox()
        Me.dgvCertifications = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.CertificationType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IssuingAuthority = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CertificationNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IssueDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ExpirationDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Comments = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip20.SuspendLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSalary.SuspendLayout()
        CType(Me.dgvCertifications, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip20
        '
        Me.ToolStrip20.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip20.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip20.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnClose, Me.btnNew, Me.btnSave, Me.btnDelete, Me.btnCancel, Me.ToolStripSeparator1, Me.btnUserActivity})
        Me.ToolStrip20.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip20.Name = "ToolStrip20"
        Me.ToolStrip20.Size = New System.Drawing.Size(856, 25)
        Me.ToolStrip20.TabIndex = 166
        Me.ToolStrip20.Text = "ToolStrip20"
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
        Me.txtEmployeeID.Location = New System.Drawing.Point(105, 76)
        Me.txtEmployeeID.MaxLength = 50
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(516, 22)
        Me.txtEmployeeID.TabIndex = 351
        '
        'txtFullname
        '
        Me.txtFullname.BackColor = System.Drawing.Color.White
        Me.txtFullname.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullname.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullname.Location = New System.Drawing.Point(105, 36)
        Me.txtFullname.MaxLength = 250
        Me.txtFullname.Name = "txtFullname"
        Me.txtFullname.ReadOnly = True
        Me.txtFullname.Size = New System.Drawing.Size(668, 28)
        Me.txtFullname.TabIndex = 350
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(11, 36)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 88)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 349
        Me.pbEmployee.TabStop = False
        '
        'grpSalary
        '
        Me.grpSalary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSalary.Controls.Add(Me.dtpExpirationDate)
        Me.grpSalary.Controls.Add(Me.Label1)
        Me.grpSalary.Controls.Add(Me.Label3)
        Me.grpSalary.Controls.Add(Me.lblComments)
        Me.grpSalary.Controls.Add(Me.lblDateExp)
        Me.grpSalary.Controls.Add(Me.lblDateIssued)
        Me.grpSalary.Controls.Add(Me.lblCertNo)
        Me.grpSalary.Controls.Add(Me.lblIssuingAuthority)
        Me.grpSalary.Controls.Add(Me.lblCertType)
        Me.grpSalary.Controls.Add(Me.txtComments)
        Me.grpSalary.Controls.Add(Me.dtpIssueDate)
        Me.grpSalary.Controls.Add(Me.txtCertificationNo)
        Me.grpSalary.Controls.Add(Me.txtIssuingAuthority)
        Me.grpSalary.Controls.Add(Me.txtCertificationType)
        Me.grpSalary.Location = New System.Drawing.Point(11, 130)
        Me.grpSalary.Name = "grpSalary"
        Me.grpSalary.Size = New System.Drawing.Size(840, 167)
        Me.grpSalary.TabIndex = 352
        Me.grpSalary.TabStop = False
        Me.grpSalary.Text = "Certification"
        '
        'dtpExpirationDate
        '
        Me.dtpExpirationDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpExpirationDate.Location = New System.Drawing.Point(338, 84)
        Me.dtpExpirationDate.Name = "dtpExpirationDate"
        Me.dtpExpirationDate.ShowCheckBox = True
        Me.dtpExpirationDate.Size = New System.Drawing.Size(190, 20)
        Me.dtpExpirationDate.TabIndex = 512
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(75, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 510
        Me.Label1.Text = "*"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(319, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 509
        Me.Label3.Text = "*"
        '
        'lblComments
        '
        Me.lblComments.AutoSize = True
        Me.lblComments.Location = New System.Drawing.Point(569, 19)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.Size = New System.Drawing.Size(56, 13)
        Me.lblComments.TabIndex = 365
        Me.lblComments.Text = "Comments"
        '
        'lblDateExp
        '
        Me.lblDateExp.AutoSize = True
        Me.lblDateExp.Location = New System.Drawing.Point(335, 68)
        Me.lblDateExp.Name = "lblDateExp"
        Me.lblDateExp.Size = New System.Drawing.Size(91, 13)
        Me.lblDateExp.TabIndex = 364
        Me.lblDateExp.Text = "Date of Expiration"
        '
        'lblDateIssued
        '
        Me.lblDateIssued.AutoSize = True
        Me.lblDateIssued.Location = New System.Drawing.Point(335, 19)
        Me.lblDateIssued.Name = "lblDateIssued"
        Me.lblDateIssued.Size = New System.Drawing.Size(64, 13)
        Me.lblDateIssued.TabIndex = 363
        Me.lblDateIssued.Text = "Date Issued"
        '
        'lblCertNo
        '
        Me.lblCertNo.AutoSize = True
        Me.lblCertNo.Location = New System.Drawing.Point(91, 117)
        Me.lblCertNo.Name = "lblCertNo"
        Me.lblCertNo.Size = New System.Drawing.Size(82, 13)
        Me.lblCertNo.TabIndex = 362
        Me.lblCertNo.Text = "Certification No."
        '
        'lblIssuingAuthority
        '
        Me.lblIssuingAuthority.AutoSize = True
        Me.lblIssuingAuthority.Location = New System.Drawing.Point(91, 68)
        Me.lblIssuingAuthority.Name = "lblIssuingAuthority"
        Me.lblIssuingAuthority.Size = New System.Drawing.Size(84, 13)
        Me.lblIssuingAuthority.TabIndex = 361
        Me.lblIssuingAuthority.Text = "Issuing Authority"
        '
        'lblCertType
        '
        Me.lblCertType.AutoSize = True
        Me.lblCertType.Location = New System.Drawing.Point(91, 19)
        Me.lblCertType.Name = "lblCertType"
        Me.lblCertType.Size = New System.Drawing.Size(89, 13)
        Me.lblCertType.TabIndex = 360
        Me.lblCertType.Text = "Certification Type"
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(572, 35)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(190, 69)
        Me.txtComments.TabIndex = 359
        '
        'dtpIssueDate
        '
        Me.dtpIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpIssueDate.Location = New System.Drawing.Point(338, 35)
        Me.dtpIssueDate.Name = "dtpIssueDate"
        Me.dtpIssueDate.Size = New System.Drawing.Size(190, 20)
        Me.dtpIssueDate.TabIndex = 357
        '
        'txtCertificationNo
        '
        Me.txtCertificationNo.Location = New System.Drawing.Point(94, 133)
        Me.txtCertificationNo.Name = "txtCertificationNo"
        Me.txtCertificationNo.Size = New System.Drawing.Size(190, 20)
        Me.txtCertificationNo.TabIndex = 356
        '
        'txtIssuingAuthority
        '
        Me.txtIssuingAuthority.Location = New System.Drawing.Point(94, 84)
        Me.txtIssuingAuthority.Name = "txtIssuingAuthority"
        Me.txtIssuingAuthority.Size = New System.Drawing.Size(190, 20)
        Me.txtIssuingAuthority.TabIndex = 355
        '
        'txtCertificationType
        '
        Me.txtCertificationType.Location = New System.Drawing.Point(94, 35)
        Me.txtCertificationType.Name = "txtCertificationType"
        Me.txtCertificationType.Size = New System.Drawing.Size(190, 20)
        Me.txtCertificationType.TabIndex = 354
        '
        'dgvCertifications
        '
        Me.dgvCertifications.AllowUserToAddRows = False
        Me.dgvCertifications.AllowUserToDeleteRows = False
        Me.dgvCertifications.AllowUserToResizeRows = False
        Me.dgvCertifications.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvCertifications.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvCertifications.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvCertifications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCertifications.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CertificationType, Me.IssuingAuthority, Me.CertificationNo, Me.IssueDate, Me.ExpirationDate, Me.Comments})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvCertifications.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvCertifications.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvCertifications.Location = New System.Drawing.Point(11, 303)
        Me.dgvCertifications.MultiSelect = False
        Me.dgvCertifications.Name = "dgvCertifications"
        Me.dgvCertifications.ReadOnly = True
        Me.dgvCertifications.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvCertifications.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCertifications.Size = New System.Drawing.Size(840, 241)
        Me.dgvCertifications.TabIndex = 353
        '
        'CertificationType
        '
        Me.CertificationType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.CertificationType.DataPropertyName = "CertificationType"
        Me.CertificationType.HeaderText = "Certification Type"
        Me.CertificationType.MinimumWidth = 120
        Me.CertificationType.Name = "CertificationType"
        Me.CertificationType.ReadOnly = True
        Me.CertificationType.Width = 120
        '
        'IssuingAuthority
        '
        Me.IssuingAuthority.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.IssuingAuthority.DataPropertyName = "IssuingAuthority"
        Me.IssuingAuthority.HeaderText = "Issuing Authority"
        Me.IssuingAuthority.MinimumWidth = 120
        Me.IssuingAuthority.Name = "IssuingAuthority"
        Me.IssuingAuthority.ReadOnly = True
        Me.IssuingAuthority.Width = 120
        '
        'CertificationNo
        '
        Me.CertificationNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.CertificationNo.DataPropertyName = "CertificationNo"
        Me.CertificationNo.HeaderText = "Certification No."
        Me.CertificationNo.MinimumWidth = 100
        Me.CertificationNo.Name = "CertificationNo"
        Me.CertificationNo.ReadOnly = True
        '
        'IssueDate
        '
        Me.IssueDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.IssueDate.DataPropertyName = "IssueDate"
        Me.IssueDate.HeaderText = "Date Issued"
        Me.IssueDate.MinimumWidth = 35
        Me.IssueDate.Name = "IssueDate"
        Me.IssueDate.ReadOnly = True
        Me.IssueDate.Width = 82
        '
        'ExpirationDate
        '
        Me.ExpirationDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.ExpirationDate.DataPropertyName = "ExpirationDate"
        Me.ExpirationDate.HeaderText = "Date of Expiration"
        Me.ExpirationDate.MinimumWidth = 35
        Me.ExpirationDate.Name = "ExpirationDate"
        Me.ExpirationDate.ReadOnly = True
        Me.ExpirationDate.Width = 106
        '
        'Comments
        '
        Me.Comments.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.Comments.DataPropertyName = "Comments"
        Me.Comments.HeaderText = "Comments"
        Me.Comments.MinimumWidth = 100
        Me.Comments.Name = "Comments"
        Me.Comments.ReadOnly = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "CertificationType"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Certification Type"
        Me.DataGridViewTextBoxColumn1.MinimumWidth = 100
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "IssuingAuthority"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Issuing Authority"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 100
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "CertificationNo"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Certification No."
        Me.DataGridViewTextBoxColumn3.MinimumWidth = 100
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "IssueDate"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Date Issued"
        Me.DataGridViewTextBoxColumn4.MinimumWidth = 35
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "ExpirationDate"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Date of Expiration"
        Me.DataGridViewTextBoxColumn5.MinimumWidth = 35
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "Comments"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Comments"
        Me.DataGridViewTextBoxColumn6.MinimumWidth = 100
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        '
        'CertificationTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.dgvCertifications)
        Me.Controls.Add(Me.grpSalary)
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullname)
        Me.Controls.Add(Me.pbEmployee)
        Me.Controls.Add(Me.ToolStrip20)
        Me.Name = "CertificationTab"
        Me.Size = New System.Drawing.Size(856, 552)
        Me.ToolStrip20.ResumeLayout(False)
        Me.ToolStrip20.PerformLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSalary.ResumeLayout(False)
        Me.grpSalary.PerformLayout()
        CType(Me.dgvCertifications, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip20 As ToolStrip
    Friend WithEvents btnClose As ToolStripButton
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents btnSave As ToolStripButton
    Friend WithEvents btnCancel As ToolStripButton
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents btnUserActivity As ToolStripButton
    Friend WithEvents txtEmployeeID As TextBox
    Public WithEvents txtFullname As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents grpSalary As GroupBox
    Friend WithEvents dgvCertifications As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents CertificationType As DataGridViewTextBoxColumn
    Friend WithEvents IssuingAuthority As DataGridViewTextBoxColumn
    Friend WithEvents CertificationNo As DataGridViewTextBoxColumn
    Friend WithEvents IssueDate As DataGridViewTextBoxColumn
    Friend WithEvents ExpirationDate As DataGridViewTextBoxColumn
    Friend WithEvents Comments As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents txtCertificationNo As TextBox
    Friend WithEvents txtIssuingAuthority As TextBox
    Friend WithEvents txtCertificationType As TextBox
    Friend WithEvents dtpIssueDate As DateTimePicker
    Friend WithEvents lblComments As Label
    Friend WithEvents lblDateExp As Label
    Friend WithEvents lblDateIssued As Label
    Friend WithEvents lblCertNo As Label
    Friend WithEvents lblIssuingAuthority As Label
    Friend WithEvents lblCertType As Label
    Friend WithEvents txtComments As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents dtpExpirationDate As DateTimePicker
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
End Class
