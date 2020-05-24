<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddAttachmentForm
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
        Me.EmployeeNumberTextbox = New System.Windows.Forms.TextBox()
        Me.EmployeeNameTextBox = New System.Windows.Forms.TextBox()
        Me.EmployeePictureBox = New System.Windows.Forms.PictureBox()
        Me.Label181 = New System.Windows.Forms.Label()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.txtFileExtension = New System.Windows.Forms.TextBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblFileName = New System.Windows.Forms.Label()
        Me.lblFileExtension = New System.Windows.Forms.Label()
        Me.AddAndCloseButton = New System.Windows.Forms.Button()
        Me.CancelDialogButton = New System.Windows.Forms.Button()
        Me.AddAndNewButton = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.EmployeePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'EmployeeNumberTextbox
        '
        Me.EmployeeNumberTextbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EmployeeNumberTextbox.BackColor = System.Drawing.Color.White
        Me.EmployeeNumberTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.EmployeeNumberTextbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.EmployeeNumberTextbox.ForeColor = System.Drawing.Color.Black
        Me.EmployeeNumberTextbox.Location = New System.Drawing.Point(107, 54)
        Me.EmployeeNumberTextbox.MaxLength = 250
        Me.EmployeeNumberTextbox.Name = "EmployeeNumberTextbox"
        Me.EmployeeNumberTextbox.ReadOnly = True
        Me.EmployeeNumberTextbox.Size = New System.Drawing.Size(537, 28)
        Me.EmployeeNumberTextbox.TabIndex = 389
        '
        'EmployeeNameTextBox
        '
        Me.EmployeeNameTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EmployeeNameTextBox.BackColor = System.Drawing.Color.White
        Me.EmployeeNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.EmployeeNameTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.EmployeeNameTextBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.EmployeeNameTextBox.Location = New System.Drawing.Point(107, 18)
        Me.EmployeeNameTextBox.MaxLength = 250
        Me.EmployeeNameTextBox.Name = "EmployeeNameTextBox"
        Me.EmployeeNameTextBox.ReadOnly = True
        Me.EmployeeNameTextBox.Size = New System.Drawing.Size(537, 28)
        Me.EmployeeNameTextBox.TabIndex = 387
        '
        'EmployeePictureBox
        '
        Me.EmployeePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.EmployeePictureBox.Location = New System.Drawing.Point(13, 12)
        Me.EmployeePictureBox.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.EmployeePictureBox.Name = "EmployeePictureBox"
        Me.EmployeePictureBox.Size = New System.Drawing.Size(88, 82)
        Me.EmployeePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EmployeePictureBox.TabIndex = 388
        Me.EmployeePictureBox.TabStop = False
        '
        'Label181
        '
        Me.Label181.AutoSize = True
        Me.Label181.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label181.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label181.Location = New System.Drawing.Point(85, 123)
        Me.Label181.Name = "Label181"
        Me.Label181.Size = New System.Drawing.Size(18, 24)
        Me.Label181.TabIndex = 401
        Me.Label181.Text = "*"
        '
        'cboType
        '
        Me.cboType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboType.DisplayMember = "DisplayValue"
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.Location = New System.Drawing.Point(109, 123)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(190, 21)
        Me.cboType.TabIndex = 400
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(106, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 399
        Me.Label1.Text = "Type:"
        '
        'txtFileName
        '
        Me.txtFileName.Enabled = False
        Me.txtFileName.Location = New System.Drawing.Point(109, 168)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(190, 22)
        Me.txtFileName.TabIndex = 402
        '
        'txtFileExtension
        '
        Me.txtFileExtension.Enabled = False
        Me.txtFileExtension.Location = New System.Drawing.Point(321, 168)
        Me.txtFileExtension.Name = "txtFileExtension"
        Me.txtFileExtension.Size = New System.Drawing.Size(190, 22)
        Me.txtFileExtension.TabIndex = 403
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(109, 196)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 404
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(224, 196)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 405
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.Location = New System.Drawing.Point(106, 152)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.Size = New System.Drawing.Size(60, 13)
        Me.lblFileName.TabIndex = 406
        Me.lblFileName.Text = "File Name:"
        '
        'lblFileExtension
        '
        Me.lblFileExtension.AutoSize = True
        Me.lblFileExtension.Location = New System.Drawing.Point(318, 152)
        Me.lblFileExtension.Name = "lblFileExtension"
        Me.lblFileExtension.Size = New System.Drawing.Size(81, 13)
        Me.lblFileExtension.TabIndex = 407
        Me.lblFileExtension.Text = "File Extension:"
        '
        'AddAndCloseButton
        '
        Me.AddAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndCloseButton.Location = New System.Drawing.Point(213, 253)
        Me.AddAndCloseButton.Name = "AddAndCloseButton"
        Me.AddAndCloseButton.Size = New System.Drawing.Size(85, 23)
        Me.AddAndCloseButton.TabIndex = 410
        Me.AddAndCloseButton.Text = "&Add && Close"
        Me.AddAndCloseButton.UseVisualStyleBackColor = True
        '
        'CancelDialogButton
        '
        Me.CancelDialogButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelDialogButton.Location = New System.Drawing.Point(387, 253)
        Me.CancelDialogButton.Name = "CancelDialogButton"
        Me.CancelDialogButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelDialogButton.TabIndex = 409
        Me.CancelDialogButton.Text = "&Cancel"
        Me.CancelDialogButton.UseVisualStyleBackColor = True
        '
        'AddAndNewButton
        '
        Me.AddAndNewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndNewButton.Location = New System.Drawing.Point(304, 253)
        Me.AddAndNewButton.Name = "AddAndNewButton"
        Me.AddAndNewButton.Size = New System.Drawing.Size(75, 23)
        Me.AddAndNewButton.TabIndex = 408
        Me.AddAndNewButton.Text = "Add && &New"
        Me.AddAndNewButton.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(85, 170)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 24)
        Me.Label2.TabIndex = 411
        Me.Label2.Text = "*"
        '
        'AddAttachmentForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(656, 288)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.AddAndCloseButton)
        Me.Controls.Add(Me.CancelDialogButton)
        Me.Controls.Add(Me.AddAndNewButton)
        Me.Controls.Add(Me.lblFileExtension)
        Me.Controls.Add(Me.lblFileName)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtFileExtension)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.Label181)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.EmployeeNumberTextbox)
        Me.Controls.Add(Me.EmployeeNameTextBox)
        Me.Controls.Add(Me.EmployeePictureBox)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AddAttachmentForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Attachment"
        CType(Me.EmployeePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents EmployeeNumberTextbox As TextBox
    Friend WithEvents EmployeeNameTextBox As TextBox
    Friend WithEvents EmployeePictureBox As PictureBox
    Friend WithEvents Label181 As Label
    Friend WithEvents cboType As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtFileName As TextBox
    Friend WithEvents txtFileExtension As TextBox
    Friend WithEvents btnBrowse As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents lblFileName As Label
    Friend WithEvents lblFileExtension As Label
    Friend WithEvents AddAndCloseButton As Button
    Friend WithEvents CancelDialogButton As Button
    Friend WithEvents AddAndNewButton As Button
    Friend WithEvents Label2 As Label
End Class
