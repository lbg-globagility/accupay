<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddSalaryForm
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
        Me.txtEmployeeID = New System.Windows.Forms.TextBox()
        Me.txtFullName = New System.Windows.Forms.TextBox()
        Me.pbEmployee = New System.Windows.Forms.PictureBox()
        Me.txtPayFrequency = New System.Windows.Forms.TextBox()
        Me.txtSalaryType = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.dtpEffectiveTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpEffectiveFrom = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblPayFrequency = New System.Windows.Forms.Label()
        Me.txtEcola = New System.Windows.Forms.TextBox()
        Me.lblEcolaPeroSign = New System.Windows.Forms.Label()
        Me.lblEcola = New System.Windows.Forms.Label()
        Me.txtTotalSalary = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblTotalSalaryPeroSign = New System.Windows.Forms.Label()
        Me.Label213 = New System.Windows.Forms.Label()
        Me.txtAllowance = New System.Windows.Forms.TextBox()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblTotalSalary = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.chkPayPhilHealth = New System.Windows.Forms.CheckBox()
        Me.ChkPagIbig = New System.Windows.Forms.CheckBox()
        Me.chkPaySSS = New System.Windows.Forms.CheckBox()
        Me.txtPagIbig = New System.Windows.Forms.TextBox()
        Me.txtPhilHealth = New System.Windows.Forms.TextBox()
        Me.Label217 = New System.Windows.Forms.Label()
        Me.Label215 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.AddAndCloseButton = New System.Windows.Forms.Button()
        Me.CancelDialogButton = New System.Windows.Forms.Button()
        Me.AddAndNewButton = New System.Windows.Forms.Button()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtEmployeeID
        '
        Me.txtEmployeeID.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtEmployeeID.BackColor = System.Drawing.Color.White
        Me.txtEmployeeID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeID.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeID.ForeColor = System.Drawing.Color.Black
        Me.txtEmployeeID.Location = New System.Drawing.Point(106, 56)
        Me.txtEmployeeID.MaxLength = 250
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(705, 28)
        Me.txtEmployeeID.TabIndex = 392
        '
        'txtFullName
        '
        Me.txtFullName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtFullName.BackColor = System.Drawing.Color.White
        Me.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullName.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullName.Location = New System.Drawing.Point(106, 20)
        Me.txtFullName.MaxLength = 250
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.ReadOnly = True
        Me.txtFullName.Size = New System.Drawing.Size(705, 28)
        Me.txtFullName.TabIndex = 390
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(12, 12)
        Me.pbEmployee.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 82)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 391
        Me.pbEmployee.TabStop = False
        '
        'txtPayFrequency
        '
        Me.txtPayFrequency.Location = New System.Drawing.Point(131, 117)
        Me.txtPayFrequency.Name = "txtPayFrequency"
        Me.txtPayFrequency.ReadOnly = True
        Me.txtPayFrequency.Size = New System.Drawing.Size(160, 22)
        Me.txtPayFrequency.TabIndex = 401
        '
        'txtSalaryType
        '
        Me.txtSalaryType.Location = New System.Drawing.Point(131, 141)
        Me.txtSalaryType.Name = "txtSalaryType"
        Me.txtSalaryType.ReadOnly = True
        Me.txtSalaryType.Size = New System.Drawing.Size(160, 22)
        Me.txtSalaryType.TabIndex = 400
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(115, 165)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(16, 16)
        Me.Label14.TabIndex = 399
        Me.Label14.Text = "*"
        '
        'dtpEffectiveTo
        '
        Me.dtpEffectiveTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEffectiveTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectiveTo.Location = New System.Drawing.Point(131, 189)
        Me.dtpEffectiveTo.Name = "dtpEffectiveTo"
        Me.dtpEffectiveTo.ShowCheckBox = True
        Me.dtpEffectiveTo.Size = New System.Drawing.Size(159, 20)
        Me.dtpEffectiveTo.TabIndex = 398
        Me.dtpEffectiveTo.Visible = False
        '
        'dtpEffectiveFrom
        '
        Me.dtpEffectiveFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEffectiveFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectiveFrom.Location = New System.Drawing.Point(131, 165)
        Me.dtpEffectiveFrom.Name = "dtpEffectiveFrom"
        Me.dtpEffectiveFrom.Size = New System.Drawing.Size(159, 20)
        Me.dtpEffectiveFrom.TabIndex = 397
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(13, 194)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(112, 16)
        Me.Label4.TabIndex = 396
        Me.Label4.Text = "Effective To"
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 170)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(112, 16)
        Me.Label3.TabIndex = 395
        Me.Label3.Text = "Effective From"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 146)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 16)
        Me.Label2.TabIndex = 394
        Me.Label2.Text = "Salary Type"
        '
        'lblPayFrequency
        '
        Me.lblPayFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayFrequency.Location = New System.Drawing.Point(13, 122)
        Me.lblPayFrequency.Name = "lblPayFrequency"
        Me.lblPayFrequency.Size = New System.Drawing.Size(112, 16)
        Me.lblPayFrequency.TabIndex = 393
        Me.lblPayFrequency.Text = "Pay Frequency"
        '
        'txtEcola
        '
        Me.txtEcola.Enabled = False
        Me.txtEcola.Location = New System.Drawing.Point(410, 192)
        Me.txtEcola.Name = "txtEcola"
        Me.txtEcola.Size = New System.Drawing.Size(160, 22)
        Me.txtEcola.TabIndex = 414
        Me.txtEcola.Visible = False
        '
        'lblEcolaPeroSign
        '
        Me.lblEcolaPeroSign.Enabled = False
        Me.lblEcolaPeroSign.Location = New System.Drawing.Point(394, 192)
        Me.lblEcolaPeroSign.Name = "lblEcolaPeroSign"
        Me.lblEcolaPeroSign.Size = New System.Drawing.Size(16, 16)
        Me.lblEcolaPeroSign.TabIndex = 413
        Me.lblEcolaPeroSign.Text = "₱"
        Me.lblEcolaPeroSign.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblEcolaPeroSign.Visible = False
        '
        'lblEcola
        '
        Me.lblEcola.Enabled = False
        Me.lblEcola.Location = New System.Drawing.Point(301, 192)
        Me.lblEcola.Name = "lblEcola"
        Me.lblEcola.Size = New System.Drawing.Size(88, 16)
        Me.lblEcola.TabIndex = 412
        Me.lblEcola.Text = "ECOLA"
        Me.lblEcola.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblEcola.Visible = False
        '
        'txtTotalSalary
        '
        Me.txtTotalSalary.Location = New System.Drawing.Point(410, 168)
        Me.txtTotalSalary.Name = "txtTotalSalary"
        Me.txtTotalSalary.ReadOnly = True
        Me.txtTotalSalary.Size = New System.Drawing.Size(160, 22)
        Me.txtTotalSalary.TabIndex = 411
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(394, 144)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(16, 16)
        Me.Label15.TabIndex = 409
        Me.Label15.Text = "₱"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalSalaryPeroSign
        '
        Me.lblTotalSalaryPeroSign.Location = New System.Drawing.Point(394, 168)
        Me.lblTotalSalaryPeroSign.Name = "lblTotalSalaryPeroSign"
        Me.lblTotalSalaryPeroSign.Size = New System.Drawing.Size(16, 16)
        Me.lblTotalSalaryPeroSign.TabIndex = 410
        Me.lblTotalSalaryPeroSign.Text = "₱"
        Me.lblTotalSalaryPeroSign.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label213
        '
        Me.Label213.Location = New System.Drawing.Point(394, 120)
        Me.Label213.Name = "Label213"
        Me.Label213.Size = New System.Drawing.Size(16, 16)
        Me.Label213.TabIndex = 408
        Me.Label213.Text = "₱"
        Me.Label213.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAllowance
        '
        Me.txtAllowance.Location = New System.Drawing.Point(410, 144)
        Me.txtAllowance.Name = "txtAllowance"
        Me.txtAllowance.Size = New System.Drawing.Size(160, 22)
        Me.txtAllowance.TabIndex = 407
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(410, 120)
        Me.txtAmount.MaxLength = 12
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.ShortcutsEnabled = False
        Me.txtAmount.Size = New System.Drawing.Size(160, 22)
        Me.txtAmount.TabIndex = 406
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(381, 121)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(18, 24)
        Me.Label12.TabIndex = 405
        Me.Label12.Text = "*"
        '
        'lblTotalSalary
        '
        Me.lblTotalSalary.Location = New System.Drawing.Point(298, 168)
        Me.lblTotalSalary.Name = "lblTotalSalary"
        Me.lblTotalSalary.Size = New System.Drawing.Size(88, 16)
        Me.lblTotalSalary.TabIndex = 404
        Me.lblTotalSalary.Text = "Total Salary"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(298, 144)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(88, 16)
        Me.Label6.TabIndex = 403
        Me.Label6.Text = "Allowance Salary"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(297, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(89, 16)
        Me.Label5.TabIndex = 402
        Me.Label5.Text = "Basic Salary:"
        '
        'chkPayPhilHealth
        '
        Me.chkPayPhilHealth.AutoSize = True
        Me.chkPayPhilHealth.Location = New System.Drawing.Point(670, 122)
        Me.chkPayPhilHealth.Name = "chkPayPhilHealth"
        Me.chkPayPhilHealth.Size = New System.Drawing.Size(15, 14)
        Me.chkPayPhilHealth.TabIndex = 422
        Me.chkPayPhilHealth.UseVisualStyleBackColor = True
        '
        'ChkPagIbig
        '
        Me.ChkPagIbig.Location = New System.Drawing.Point(670, 170)
        Me.ChkPagIbig.Name = "ChkPagIbig"
        Me.ChkPagIbig.Size = New System.Drawing.Size(16, 16)
        Me.ChkPagIbig.TabIndex = 423
        Me.ChkPagIbig.UseVisualStyleBackColor = True
        '
        'chkPaySSS
        '
        Me.chkPaySSS.AutoSize = True
        Me.chkPaySSS.Location = New System.Drawing.Point(670, 146)
        Me.chkPaySSS.Name = "chkPaySSS"
        Me.chkPaySSS.Size = New System.Drawing.Size(15, 14)
        Me.chkPaySSS.TabIndex = 424
        Me.chkPaySSS.UseVisualStyleBackColor = True
        '
        'txtPagIbig
        '
        Me.txtPagIbig.BackColor = System.Drawing.Color.White
        Me.txtPagIbig.Location = New System.Drawing.Point(710, 170)
        Me.txtPagIbig.Name = "txtPagIbig"
        Me.txtPagIbig.Size = New System.Drawing.Size(107, 22)
        Me.txtPagIbig.TabIndex = 419
        '
        'txtPhilHealth
        '
        Me.txtPhilHealth.BackColor = System.Drawing.Color.White
        Me.txtPhilHealth.Location = New System.Drawing.Point(710, 122)
        Me.txtPhilHealth.Name = "txtPhilHealth"
        Me.txtPhilHealth.Size = New System.Drawing.Size(107, 22)
        Me.txtPhilHealth.TabIndex = 418
        '
        'Label217
        '
        Me.Label217.Location = New System.Drawing.Point(694, 170)
        Me.Label217.Name = "Label217"
        Me.Label217.Size = New System.Drawing.Size(16, 16)
        Me.Label217.TabIndex = 420
        Me.Label217.Text = "₱"
        Me.Label217.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label215
        '
        Me.Label215.Location = New System.Drawing.Point(694, 122)
        Me.Label215.Name = "Label215"
        Me.Label215.Size = New System.Drawing.Size(16, 16)
        Me.Label215.TabIndex = 421
        Me.Label215.Text = "₱"
        Me.Label215.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(576, 170)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(88, 16)
        Me.Label11.TabIndex = 417
        Me.Label11.Text = "PAGIBIG (Auto)"
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(576, 146)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 16)
        Me.Label9.TabIndex = 416
        Me.Label9.Text = "SSS (Auto)"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(576, 122)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(103, 16)
        Me.Label8.TabIndex = 415
        Me.Label8.Text = "PhilHealth (Auto)"
        '
        'AddAndCloseButton
        '
        Me.AddAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndCloseButton.Location = New System.Drawing.Point(285, 254)
        Me.AddAndCloseButton.Name = "AddAndCloseButton"
        Me.AddAndCloseButton.Size = New System.Drawing.Size(85, 23)
        Me.AddAndCloseButton.TabIndex = 427
        Me.AddAndCloseButton.Text = "&Add && Close"
        Me.AddAndCloseButton.UseVisualStyleBackColor = True
        '
        'CancelDialogButton
        '
        Me.CancelDialogButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelDialogButton.Location = New System.Drawing.Point(459, 254)
        Me.CancelDialogButton.Name = "CancelDialogButton"
        Me.CancelDialogButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelDialogButton.TabIndex = 426
        Me.CancelDialogButton.Text = "&Cancel"
        Me.CancelDialogButton.UseVisualStyleBackColor = True
        '
        'AddAndNewButton
        '
        Me.AddAndNewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndNewButton.Location = New System.Drawing.Point(376, 254)
        Me.AddAndNewButton.Name = "AddAndNewButton"
        Me.AddAndNewButton.Size = New System.Drawing.Size(75, 23)
        Me.AddAndNewButton.TabIndex = 425
        Me.AddAndNewButton.Text = "Add && &New"
        Me.AddAndNewButton.UseVisualStyleBackColor = True
        '
        'AddSalaryForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(831, 289)
        Me.Controls.Add(Me.AddAndCloseButton)
        Me.Controls.Add(Me.CancelDialogButton)
        Me.Controls.Add(Me.AddAndNewButton)
        Me.Controls.Add(Me.chkPayPhilHealth)
        Me.Controls.Add(Me.ChkPagIbig)
        Me.Controls.Add(Me.chkPaySSS)
        Me.Controls.Add(Me.txtPagIbig)
        Me.Controls.Add(Me.txtPhilHealth)
        Me.Controls.Add(Me.Label217)
        Me.Controls.Add(Me.Label215)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtEcola)
        Me.Controls.Add(Me.lblEcolaPeroSign)
        Me.Controls.Add(Me.lblEcola)
        Me.Controls.Add(Me.txtTotalSalary)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.lblTotalSalaryPeroSign)
        Me.Controls.Add(Me.Label213)
        Me.Controls.Add(Me.txtAllowance)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.lblTotalSalary)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtPayFrequency)
        Me.Controls.Add(Me.txtSalaryType)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.dtpEffectiveTo)
        Me.Controls.Add(Me.dtpEffectiveFrom)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblPayFrequency)
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullName)
        Me.Controls.Add(Me.pbEmployee)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AddSalaryForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Salary"
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmployeeID As TextBox
    Friend WithEvents txtFullName As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents txtPayFrequency As TextBox
    Friend WithEvents txtSalaryType As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents dtpEffectiveTo As DateTimePicker
    Friend WithEvents dtpEffectiveFrom As DateTimePicker
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblPayFrequency As Label
    Friend WithEvents txtEcola As TextBox
    Friend WithEvents lblEcolaPeroSign As Label
    Friend WithEvents lblEcola As Label
    Friend WithEvents txtTotalSalary As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents lblTotalSalaryPeroSign As Label
    Friend WithEvents Label213 As Label
    Friend WithEvents txtAllowance As TextBox
    Friend WithEvents txtAmount As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents lblTotalSalary As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents chkPayPhilHealth As CheckBox
    Friend WithEvents ChkPagIbig As CheckBox
    Friend WithEvents chkPaySSS As CheckBox
    Friend WithEvents txtPagIbig As TextBox
    Friend WithEvents txtPhilHealth As TextBox
    Friend WithEvents Label217 As Label
    Friend WithEvents Label215 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents AddAndCloseButton As Button
    Friend WithEvents CancelDialogButton As Button
    Friend WithEvents AddAndNewButton As Button
End Class
