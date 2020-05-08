<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddPromotionForm
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
        Me.AddAndCloseButton = New System.Windows.Forms.Button()
        Me.CancelDialogButton = New System.Windows.Forms.Button()
        Me.AddAndNewButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblReqAsterisk = New System.Windows.Forms.Label()
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
        Me.txtEmployeeID.Location = New System.Drawing.Point(112, 63)
        Me.txtEmployeeID.MaxLength = 250
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(560, 28)
        Me.txtEmployeeID.TabIndex = 392
        '
        'txtFullName
        '
        Me.txtFullName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtFullName.BackColor = System.Drawing.Color.White
        Me.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullName.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullName.Location = New System.Drawing.Point(112, 27)
        Me.txtFullName.MaxLength = 250
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.ReadOnly = True
        Me.txtFullName.Size = New System.Drawing.Size(560, 28)
        Me.txtFullName.TabIndex = 390
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(18, 21)
        Me.pbEmployee.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 82)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 391
        Me.pbEmployee.TabStop = False
        '
        'txtReason
        '
        Me.txtReason.Location = New System.Drawing.Point(456, 137)
        Me.txtReason.MaxLength = 2000
        Me.txtReason.Multiline = True
        Me.txtReason.Name = "txtReason"
        Me.txtReason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtReason.Size = New System.Drawing.Size(195, 122)
        Me.txtReason.TabIndex = 419
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(406, 145)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(45, 13)
        Me.Label57.TabIndex = 418
        Me.Label57.Text = "Reason"
        '
        'Label222
        '
        Me.Label222.AutoSize = True
        Me.Label222.Location = New System.Drawing.Point(147, 223)
        Me.Label222.Name = "Label222"
        Me.Label222.Size = New System.Drawing.Size(13, 13)
        Me.Label222.TabIndex = 417
        Me.Label222.Text = "₱"
        '
        'lblPeso
        '
        Me.lblPeso.AutoSize = True
        Me.lblPeso.Location = New System.Drawing.Point(147, 242)
        Me.lblPeso.Name = "lblPeso"
        Me.lblPeso.Size = New System.Drawing.Size(13, 13)
        Me.lblPeso.TabIndex = 416
        Me.lblPeso.Text = "₱"
        Me.lblPeso.Visible = False
        '
        'lblCurrentSalary
        '
        Me.lblCurrentSalary.AutoSize = True
        Me.lblCurrentSalary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentSalary.Location = New System.Drawing.Point(162, 223)
        Me.lblCurrentSalary.Name = "lblCurrentSalary"
        Me.lblCurrentSalary.Size = New System.Drawing.Size(14, 13)
        Me.lblCurrentSalary.TabIndex = 415
        Me.lblCurrentSalary.Text = "0"
        '
        'cboPositionTo
        '
        Me.cboPositionTo.DisplayMember = "Name"
        Me.cboPositionTo.FormattingEnabled = True
        Me.cboPositionTo.Location = New System.Drawing.Point(165, 137)
        Me.cboPositionTo.Name = "cboPositionTo"
        Me.cboPositionTo.Size = New System.Drawing.Size(195, 21)
        Me.cboPositionTo.TabIndex = 404
        '
        'Label142
        '
        Me.Label142.AutoSize = True
        Me.Label142.Location = New System.Drawing.Point(25, 223)
        Me.Label142.Name = "Label142"
        Me.Label142.Size = New System.Drawing.Size(78, 13)
        Me.Label142.TabIndex = 414
        Me.Label142.Text = "Current salary"
        '
        'Label85
        '
        Me.Label85.AutoSize = True
        Me.Label85.Location = New System.Drawing.Point(25, 119)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(78, 13)
        Me.Label85.TabIndex = 408
        Me.Label85.Text = "Position From"
        '
        'lblPositionFrom
        '
        Me.lblPositionFrom.BackColor = System.Drawing.Color.White
        Me.lblPositionFrom.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblPositionFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblPositionFrom.ForeColor = System.Drawing.Color.Black
        Me.lblPositionFrom.Location = New System.Drawing.Point(165, 119)
        Me.lblPositionFrom.MaxLength = 50
        Me.lblPositionFrom.Name = "lblPositionFrom"
        Me.lblPositionFrom.ReadOnly = True
        Me.lblPositionFrom.Size = New System.Drawing.Size(516, 13)
        Me.lblPositionFrom.TabIndex = 413
        '
        'Label84
        '
        Me.Label84.AutoSize = True
        Me.Label84.Location = New System.Drawing.Point(25, 145)
        Me.Label84.Name = "Label84"
        Me.Label84.Size = New System.Drawing.Size(64, 13)
        Me.Label84.TabIndex = 409
        Me.Label84.Text = "Position To"
        '
        'cboCompensationChange
        '
        Me.cboCompensationChange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCompensationChange.FormattingEnabled = True
        Me.cboCompensationChange.Items.AddRange(New Object() {"Yes", "No"})
        Me.cboCompensationChange.Location = New System.Drawing.Point(165, 190)
        Me.cboCompensationChange.Name = "cboCompensationChange"
        Me.cboCompensationChange.Size = New System.Drawing.Size(195, 21)
        Me.cboCompensationChange.TabIndex = 406
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.Location = New System.Drawing.Point(25, 198)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(125, 13)
        Me.Label83.TabIndex = 410
        Me.Label83.Text = "Compensation Change"
        '
        'txtNewSalary
        '
        Me.txtNewSalary.Location = New System.Drawing.Point(165, 239)
        Me.txtNewSalary.Name = "txtNewSalary"
        Me.txtNewSalary.ShortcutsEnabled = False
        Me.txtNewSalary.Size = New System.Drawing.Size(195, 22)
        Me.txtNewSalary.TabIndex = 407
        Me.txtNewSalary.Visible = False
        '
        'lblNewSalary
        '
        Me.lblNewSalary.AutoSize = True
        Me.lblNewSalary.Location = New System.Drawing.Point(25, 247)
        Me.lblNewSalary.Name = "lblNewSalary"
        Me.lblNewSalary.Size = New System.Drawing.Size(62, 13)
        Me.lblNewSalary.TabIndex = 411
        Me.lblNewSalary.Text = "New salary"
        Me.lblNewSalary.Visible = False
        '
        'Label81
        '
        Me.Label81.AutoSize = True
        Me.Label81.Location = New System.Drawing.Point(25, 171)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(77, 13)
        Me.Label81.TabIndex = 412
        Me.Label81.Text = "Effective Date"
        '
        'dtpEffectivityDate
        '
        Me.dtpEffectivityDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectivityDate.Location = New System.Drawing.Point(165, 164)
        Me.dtpEffectivityDate.Name = "dtpEffectivityDate"
        Me.dtpEffectivityDate.Size = New System.Drawing.Size(195, 22)
        Me.dtpEffectivityDate.TabIndex = 405
        '
        'AddAndCloseButton
        '
        Me.AddAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndCloseButton.Location = New System.Drawing.Point(213, 286)
        Me.AddAndCloseButton.Name = "AddAndCloseButton"
        Me.AddAndCloseButton.Size = New System.Drawing.Size(85, 23)
        Me.AddAndCloseButton.TabIndex = 422
        Me.AddAndCloseButton.Text = "&Add && Close"
        Me.AddAndCloseButton.UseVisualStyleBackColor = True
        '
        'CancelDialogButton
        '
        Me.CancelDialogButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelDialogButton.Location = New System.Drawing.Point(387, 286)
        Me.CancelDialogButton.Name = "CancelDialogButton"
        Me.CancelDialogButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelDialogButton.TabIndex = 421
        Me.CancelDialogButton.Text = "&Cancel"
        Me.CancelDialogButton.UseVisualStyleBackColor = True
        '
        'AddAndNewButton
        '
        Me.AddAndNewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndNewButton.Location = New System.Drawing.Point(304, 286)
        Me.AddAndNewButton.Name = "AddAndNewButton"
        Me.AddAndNewButton.Size = New System.Drawing.Size(75, 23)
        Me.AddAndNewButton.TabIndex = 420
        Me.AddAndNewButton.Text = "Add && &New"
        Me.AddAndNewButton.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(147, 140)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 510
        Me.Label3.Text = "*"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(147, 193)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 511
        Me.Label1.Text = "*"
        '
        'lblReqAsterisk
        '
        Me.lblReqAsterisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblReqAsterisk.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblReqAsterisk.Location = New System.Drawing.Point(134, 241)
        Me.lblReqAsterisk.Name = "lblReqAsterisk"
        Me.lblReqAsterisk.Size = New System.Drawing.Size(13, 13)
        Me.lblReqAsterisk.TabIndex = 512
        Me.lblReqAsterisk.Text = "*"
        Me.lblReqAsterisk.Visible = False
        '
        'AddPromotionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(684, 321)
        Me.Controls.Add(Me.AddAndCloseButton)
        Me.Controls.Add(Me.CancelDialogButton)
        Me.Controls.Add(Me.AddAndNewButton)
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
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullName)
        Me.Controls.Add(Me.pbEmployee)
        Me.Controls.Add(Me.lblReqAsterisk)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AddPromotionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Promotion"
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmployeeID As TextBox
    Friend WithEvents txtFullName As TextBox
    Friend WithEvents pbEmployee As PictureBox
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
    Friend WithEvents AddAndCloseButton As Button
    Friend WithEvents CancelDialogButton As Button
    Friend WithEvents AddAndNewButton As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblReqAsterisk As Label
End Class
