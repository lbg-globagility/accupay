<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AddCertificationForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.txtEmployeeID = New System.Windows.Forms.TextBox()
        Me.txtFullName = New System.Windows.Forms.TextBox()
        Me.pbEmployee = New System.Windows.Forms.PictureBox()
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
        Me.AddAndCloseButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.AddAndNewButton = New System.Windows.Forms.Button()
        Me.dtpExpirationDate = New System.Windows.Forms.DateTimePicker()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtEmployeeID
        '
        Me.txtEmployeeID.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtEmployeeID.BackColor = System.Drawing.Color.White
        Me.txtEmployeeID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeID.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtEmployeeID.Location = New System.Drawing.Point(107, 56)
        Me.txtEmployeeID.MaxLength = 250
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(595, 28)
        Me.txtEmployeeID.TabIndex = 392
        '
        'txtFullName
        '
        Me.txtFullName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtFullName.BackColor = System.Drawing.Color.White
        Me.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullName.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullName.Location = New System.Drawing.Point(107, 20)
        Me.txtFullName.MaxLength = 250
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.ReadOnly = True
        Me.txtFullName.Size = New System.Drawing.Size(595, 28)
        Me.txtFullName.TabIndex = 390
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(13, 12)
        Me.pbEmployee.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 82)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 391
        Me.pbEmployee.TabStop = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(24, 131)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 524
        Me.Label1.Text = "*"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(261, 131)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 523
        Me.Label3.Text = "*"
        '
        'lblComments
        '
        Me.lblComments.AutoSize = True
        Me.lblComments.Location = New System.Drawing.Point(509, 113)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.Size = New System.Drawing.Size(61, 13)
        Me.lblComments.TabIndex = 522
        Me.lblComments.Text = "Comments"
        '
        'lblDateExp
        '
        Me.lblDateExp.AutoSize = True
        Me.lblDateExp.Location = New System.Drawing.Point(277, 162)
        Me.lblDateExp.Name = "lblDateExp"
        Me.lblDateExp.Size = New System.Drawing.Size(100, 13)
        Me.lblDateExp.TabIndex = 521
        Me.lblDateExp.Text = "Date of Expiration"
        '
        'lblDateIssued
        '
        Me.lblDateIssued.AutoSize = True
        Me.lblDateIssued.Location = New System.Drawing.Point(277, 113)
        Me.lblDateIssued.Name = "lblDateIssued"
        Me.lblDateIssued.Size = New System.Drawing.Size(67, 13)
        Me.lblDateIssued.TabIndex = 520
        Me.lblDateIssued.Text = "Date Issued"
        '
        'lblCertNo
        '
        Me.lblCertNo.AutoSize = True
        Me.lblCertNo.Location = New System.Drawing.Point(40, 211)
        Me.lblCertNo.Name = "lblCertNo"
        Me.lblCertNo.Size = New System.Drawing.Size(91, 13)
        Me.lblCertNo.TabIndex = 519
        Me.lblCertNo.Text = "Certification No."
        '
        'lblIssuingAuthority
        '
        Me.lblIssuingAuthority.AutoSize = True
        Me.lblIssuingAuthority.Location = New System.Drawing.Point(40, 162)
        Me.lblIssuingAuthority.Name = "lblIssuingAuthority"
        Me.lblIssuingAuthority.Size = New System.Drawing.Size(95, 13)
        Me.lblIssuingAuthority.TabIndex = 518
        Me.lblIssuingAuthority.Text = "Issuing Authority"
        '
        'lblCertType
        '
        Me.lblCertType.AutoSize = True
        Me.lblCertType.Location = New System.Drawing.Point(40, 113)
        Me.lblCertType.Name = "lblCertType"
        Me.lblCertType.Size = New System.Drawing.Size(96, 13)
        Me.lblCertType.TabIndex = 517
        Me.lblCertType.Text = "Certification Type"
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(512, 129)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(190, 69)
        Me.txtComments.TabIndex = 516
        '
        'dtpIssueDate
        '
        Me.dtpIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpIssueDate.Location = New System.Drawing.Point(280, 129)
        Me.dtpIssueDate.Name = "dtpIssueDate"
        Me.dtpIssueDate.Size = New System.Drawing.Size(190, 22)
        Me.dtpIssueDate.TabIndex = 515
        '
        'txtCertificationNo
        '
        Me.txtCertificationNo.Location = New System.Drawing.Point(43, 227)
        Me.txtCertificationNo.Name = "txtCertificationNo"
        Me.txtCertificationNo.Size = New System.Drawing.Size(190, 22)
        Me.txtCertificationNo.TabIndex = 514
        '
        'txtIssuingAuthority
        '
        Me.txtIssuingAuthority.Location = New System.Drawing.Point(43, 178)
        Me.txtIssuingAuthority.Name = "txtIssuingAuthority"
        Me.txtIssuingAuthority.Size = New System.Drawing.Size(190, 22)
        Me.txtIssuingAuthority.TabIndex = 513
        '
        'txtCertificationType
        '
        Me.txtCertificationType.Location = New System.Drawing.Point(43, 129)
        Me.txtCertificationType.Name = "txtCertificationType"
        Me.txtCertificationType.Size = New System.Drawing.Size(190, 22)
        Me.txtCertificationType.TabIndex = 512
        '
        'AddAndCloseButton
        '
        Me.AddAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndCloseButton.Location = New System.Drawing.Point(237, 286)
        Me.AddAndCloseButton.Name = "AddAndCloseButton"
        Me.AddAndCloseButton.Size = New System.Drawing.Size(85, 23)
        Me.AddAndCloseButton.TabIndex = 528
        Me.AddAndCloseButton.Text = "&Add && Close"
        Me.AddAndCloseButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.Location = New System.Drawing.Point(411, 286)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 527
        Me.CancelButton.Text = "&Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'AddAndNewButton
        '
        Me.AddAndNewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndNewButton.Location = New System.Drawing.Point(328, 286)
        Me.AddAndNewButton.Name = "AddAndNewButton"
        Me.AddAndNewButton.Size = New System.Drawing.Size(75, 23)
        Me.AddAndNewButton.TabIndex = 526
        Me.AddAndNewButton.Text = "Add && &New"
        Me.AddAndNewButton.UseVisualStyleBackColor = True
        '
        'dtpExpirationDate
        '
        Me.dtpExpirationDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpExpirationDate.Location = New System.Drawing.Point(280, 178)
        Me.dtpExpirationDate.Name = "dtpExpirationDate"
        Me.dtpExpirationDate.ShowCheckBox = True
        Me.dtpExpirationDate.Size = New System.Drawing.Size(190, 22)
        Me.dtpExpirationDate.TabIndex = 529
        '
        'AddCertificationForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(732, 321)
        Me.Controls.Add(Me.dtpExpirationDate)
        Me.Controls.Add(Me.AddAndCloseButton)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.AddAndNewButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblComments)
        Me.Controls.Add(Me.lblDateExp)
        Me.Controls.Add(Me.lblDateIssued)
        Me.Controls.Add(Me.lblCertNo)
        Me.Controls.Add(Me.lblIssuingAuthority)
        Me.Controls.Add(Me.lblCertType)
        Me.Controls.Add(Me.txtComments)
        Me.Controls.Add(Me.dtpIssueDate)
        Me.Controls.Add(Me.txtCertificationNo)
        Me.Controls.Add(Me.txtIssuingAuthority)
        Me.Controls.Add(Me.txtCertificationType)
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullName)
        Me.Controls.Add(Me.pbEmployee)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AddCertificationForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Certification"
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmployeeID As TextBox
    Friend WithEvents txtFullName As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblComments As Label
    Friend WithEvents lblDateExp As Label
    Friend WithEvents lblDateIssued As Label
    Friend WithEvents lblCertNo As Label
    Friend WithEvents lblIssuingAuthority As Label
    Friend WithEvents lblCertType As Label
    Friend WithEvents txtComments As TextBox
    Friend WithEvents dtpIssueDate As DateTimePicker
    Friend WithEvents txtCertificationNo As TextBox
    Friend WithEvents txtIssuingAuthority As TextBox
    Friend WithEvents txtCertificationType As TextBox
    Friend WithEvents AddAndCloseButton As Button
    Friend WithEvents CancelButton As Button
    Friend WithEvents AddAndNewButton As Button
    Friend WithEvents dtpExpirationDate As DateTimePicker
End Class
