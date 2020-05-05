<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddDisciplinaryAction
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.cboAction = New System.Windows.Forms.ComboBox()
        Me.cboFinding = New System.Windows.Forms.ComboBox()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.dtpEffectiveTo = New System.Windows.Forms.DateTimePicker()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.dtpEffectiveFrom = New System.Windows.Forms.DateTimePicker()
        Me.txtComments = New System.Windows.Forms.TextBox()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.lblAddFindingname = New System.Windows.Forms.LinkLabel()
        Me.AddAndCloseButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
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
        Me.txtEmployeeID.Location = New System.Drawing.Point(107, 54)
        Me.txtEmployeeID.MaxLength = 250
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(564, 28)
        Me.txtEmployeeID.TabIndex = 395
        '
        'txtFullName
        '
        Me.txtFullName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtFullName.BackColor = System.Drawing.Color.White
        Me.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullName.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullName.Location = New System.Drawing.Point(107, 18)
        Me.txtFullName.MaxLength = 250
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.ReadOnly = True
        Me.txtFullName.Size = New System.Drawing.Size(564, 28)
        Me.txtFullName.TabIndex = 393
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(13, 12)
        Me.pbEmployee.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 82)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 394
        Me.pbEmployee.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(39, 167)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(18, 24)
        Me.Label1.TabIndex = 538
        Me.Label1.Text = "*"
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label61.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label61.Location = New System.Drawing.Point(39, 127)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(18, 24)
        Me.Label61.TabIndex = 537
        Me.Label61.Text = "*"
        '
        'cboAction
        '
        Me.cboAction.DisplayMember = "DisplayValue"
        Me.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAction.FormattingEnabled = True
        Me.cboAction.Location = New System.Drawing.Point(59, 165)
        Me.cboAction.MaxLength = 100
        Me.cboAction.Name = "cboAction"
        Me.cboAction.Size = New System.Drawing.Size(205, 21)
        Me.cboAction.TabIndex = 524
        '
        'cboFinding
        '
        Me.cboFinding.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboFinding.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboFinding.DisplayMember = "PartNo"
        Me.cboFinding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFinding.FormattingEnabled = True
        Me.cboFinding.Location = New System.Drawing.Point(59, 125)
        Me.cboFinding.Name = "cboFinding"
        Me.cboFinding.Size = New System.Drawing.Size(205, 21)
        Me.cboFinding.TabIndex = 523
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Location = New System.Drawing.Point(56, 188)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(106, 13)
        Me.Label44.TabIndex = 530
        Me.Label44.Text = "Effective Date From"
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(56, 109)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(79, 13)
        Me.Label43.TabIndex = 531
        Me.Label43.Text = "Finding Name"
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Location = New System.Drawing.Point(56, 227)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(92, 13)
        Me.Label45.TabIndex = 529
        Me.Label45.Text = "Effective Date To"
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(56, 149)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(40, 13)
        Me.Label42.TabIndex = 532
        Me.Label42.Text = "Action"
        '
        'dtpEffectiveTo
        '
        Me.dtpEffectiveTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectiveTo.Location = New System.Drawing.Point(59, 244)
        Me.dtpEffectiveTo.Name = "dtpEffectiveTo"
        Me.dtpEffectiveTo.Size = New System.Drawing.Size(205, 22)
        Me.dtpEffectiveTo.TabIndex = 526
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(329, 127)
        Me.txtDescription.MaxLength = 2000
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(334, 59)
        Me.txtDescription.TabIndex = 527
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(326, 111)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(109, 13)
        Me.Label40.TabIndex = 534
        Me.Label40.Text = "Finding Description"
        '
        'dtpEffectiveFrom
        '
        Me.dtpEffectiveFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectiveFrom.Location = New System.Drawing.Point(59, 204)
        Me.dtpEffectiveFrom.Name = "dtpEffectiveFrom"
        Me.dtpEffectiveFrom.Size = New System.Drawing.Size(205, 22)
        Me.dtpEffectiveFrom.TabIndex = 525
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(329, 205)
        Me.txtComments.MaxLength = 500
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(334, 59)
        Me.txtComments.TabIndex = 528
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(326, 189)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(61, 13)
        Me.Label41.TabIndex = 533
        Me.Label41.Text = "Comments"
        '
        'LinkLabel3
        '
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.Location = New System.Drawing.Point(134, 149)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(88, 13)
        Me.LinkLabel3.TabIndex = 536
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = "Add/Edit Action"
        '
        'lblAddFindingname
        '
        Me.lblAddFindingname.AutoSize = True
        Me.lblAddFindingname.Location = New System.Drawing.Point(134, 109)
        Me.lblAddFindingname.Name = "lblAddFindingname"
        Me.lblAddFindingname.Size = New System.Drawing.Size(127, 13)
        Me.lblAddFindingname.TabIndex = 535
        Me.lblAddFindingname.TabStop = True
        Me.lblAddFindingname.Text = "Add/Edit Finding Name"
        '
        'AddAndCloseButton
        '
        Me.AddAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndCloseButton.Location = New System.Drawing.Point(227, 286)
        Me.AddAndCloseButton.Name = "AddAndCloseButton"
        Me.AddAndCloseButton.Size = New System.Drawing.Size(85, 23)
        Me.AddAndCloseButton.TabIndex = 541
        Me.AddAndCloseButton.Text = "&Add && Close"
        Me.AddAndCloseButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.Location = New System.Drawing.Point(401, 286)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 540
        Me.CancelButton.Text = "&Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'AddAndNewButton
        '
        Me.AddAndNewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddAndNewButton.Location = New System.Drawing.Point(318, 286)
        Me.AddAndNewButton.Name = "AddAndNewButton"
        Me.AddAndNewButton.Size = New System.Drawing.Size(75, 23)
        Me.AddAndNewButton.TabIndex = 539
        Me.AddAndNewButton.Text = "Add && &New"
        Me.AddAndNewButton.UseVisualStyleBackColor = True
        '
        'AddDisciplinaryAction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(711, 323)
        Me.Controls.Add(Me.AddAndCloseButton)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.AddAndNewButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label61)
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
        Me.Controls.Add(Me.txtFullName)
        Me.Controls.Add(Me.pbEmployee)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AddDisciplinaryAction"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Disciplinary Action"
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmployeeID As TextBox
    Friend WithEvents txtFullName As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label61 As Label
    Friend WithEvents cboAction As ComboBox
    Friend WithEvents cboFinding As ComboBox
    Friend WithEvents Label44 As Label
    Friend WithEvents Label43 As Label
    Friend WithEvents Label45 As Label
    Friend WithEvents Label42 As Label
    Friend WithEvents dtpEffectiveTo As DateTimePicker
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents Label40 As Label
    Friend WithEvents dtpEffectiveFrom As DateTimePicker
    Friend WithEvents txtComments As TextBox
    Friend WithEvents Label41 As Label
    Friend WithEvents LinkLabel3 As LinkLabel
    Friend WithEvents lblAddFindingname As LinkLabel
    Friend WithEvents AddAndCloseButton As Button
    Friend WithEvents CancelButton As Button
    Friend WithEvents AddAndNewButton As Button
End Class
