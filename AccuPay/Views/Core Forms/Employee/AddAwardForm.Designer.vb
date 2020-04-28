<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddAwardForm
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
        Me.lblAwardDate = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblAwardType = New System.Windows.Forms.Label()
        Me.dtpAwardDate = New System.Windows.Forms.DateTimePicker()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtAwardType = New System.Windows.Forms.TextBox()
        Me.AddAndCloseButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.AddAndNewButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
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
        Me.txtEmployeeID.Location = New System.Drawing.Point(108, 56)
        Me.txtEmployeeID.MaxLength = 250
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(560, 28)
        Me.txtEmployeeID.TabIndex = 389
        '
        'txtFullName
        '
        Me.txtFullName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtFullName.BackColor = System.Drawing.Color.White
        Me.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullName.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullName.Location = New System.Drawing.Point(108, 20)
        Me.txtFullName.MaxLength = 250
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.ReadOnly = True
        Me.txtFullName.Size = New System.Drawing.Size(560, 28)
        Me.txtFullName.TabIndex = 387
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(14, 12)
        Me.pbEmployee.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 82)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 388
        Me.pbEmployee.TabStop = False
        '
        'lblAwardDate
        '
        Me.lblAwardDate.AutoSize = True
        Me.lblAwardDate.Location = New System.Drawing.Point(36, 207)
        Me.lblAwardDate.Name = "lblAwardDate"
        Me.lblAwardDate.Size = New System.Drawing.Size(67, 13)
        Me.lblAwardDate.TabIndex = 395
        Me.lblAwardDate.Text = "Award Date"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(352, 151)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(66, 13)
        Me.lblDescription.TabIndex = 394
        Me.lblDescription.Text = "Description"
        '
        'lblAwardType
        '
        Me.lblAwardType.AutoSize = True
        Me.lblAwardType.Location = New System.Drawing.Point(36, 151)
        Me.lblAwardType.Name = "lblAwardType"
        Me.lblAwardType.Size = New System.Drawing.Size(66, 13)
        Me.lblAwardType.TabIndex = 393
        Me.lblAwardType.Text = "Award Type"
        '
        'dtpAwardDate
        '
        Me.dtpAwardDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpAwardDate.Location = New System.Drawing.Point(131, 200)
        Me.dtpAwardDate.Name = "dtpAwardDate"
        Me.dtpAwardDate.Size = New System.Drawing.Size(190, 22)
        Me.dtpAwardDate.TabIndex = 392
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(442, 148)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(190, 74)
        Me.txtDescription.TabIndex = 391
        '
        'txtAwardType
        '
        Me.txtAwardType.Location = New System.Drawing.Point(131, 148)
        Me.txtAwardType.Name = "txtAwardType"
        Me.txtAwardType.Size = New System.Drawing.Size(190, 22)
        Me.txtAwardType.TabIndex = 390
        '
        'AddAndCloseButton
        '
        Me.AddAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndCloseButton.Location = New System.Drawing.Point(213, 286)
        Me.AddAndCloseButton.Name = "AddAndCloseButton"
        Me.AddAndCloseButton.Size = New System.Drawing.Size(85, 23)
        Me.AddAndCloseButton.TabIndex = 408
        Me.AddAndCloseButton.Text = "&Add && Close"
        Me.AddAndCloseButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.Location = New System.Drawing.Point(387, 286)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 407
        Me.CancelButton.Text = "&Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'AddAndNewButton
        '
        Me.AddAndNewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndNewButton.Location = New System.Drawing.Point(304, 286)
        Me.AddAndNewButton.Name = "AddAndNewButton"
        Me.AddAndNewButton.Size = New System.Drawing.Size(75, 23)
        Me.AddAndNewButton.TabIndex = 406
        Me.AddAndNewButton.Text = "Add && &New"
        Me.AddAndNewButton.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(112, 151)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 509
        Me.Label3.Text = "*"
        '
        'AddAwardForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(684, 321)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.AddAndCloseButton)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.AddAndNewButton)
        Me.Controls.Add(Me.lblAwardDate)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.lblAwardType)
        Me.Controls.Add(Me.dtpAwardDate)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.txtAwardType)
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullName)
        Me.Controls.Add(Me.pbEmployee)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AddAwardForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Award"
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmployeeID As TextBox
    Friend WithEvents txtFullName As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents lblAwardDate As Label
    Friend WithEvents lblDescription As Label
    Friend WithEvents lblAwardType As Label
    Friend WithEvents dtpAwardDate As DateTimePicker
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents txtAwardType As TextBox
    Friend WithEvents AddAndCloseButton As Button
    Friend WithEvents CancelButton As Button
    Friend WithEvents AddAndNewButton As Button
    Friend WithEvents Label3 As Label
End Class
