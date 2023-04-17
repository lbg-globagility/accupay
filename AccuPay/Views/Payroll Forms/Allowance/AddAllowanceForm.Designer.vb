<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AddAllowanceForm
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
        Me.txtEmployeeNumber = New System.Windows.Forms.TextBox()
        Me.pbEmployeePicture = New System.Windows.Forms.PictureBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnAddAndNew = New System.Windows.Forms.Button()
        Me.txtEmployeeFirstName = New System.Windows.Forms.TextBox()
        Me.btnAddAndClose = New System.Windows.Forms.Button()
        Me.EmployeeInfoTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.AllowanceDetailsTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlTxtLoanBalance = New System.Windows.Forms.Panel()
        Me.txtallowamt = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.dtpallowstartdate = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboallowfreq = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.plnCboLoanType = New System.Windows.Forms.Panel()
        Me.Label350 = New System.Windows.Forms.Label()
        Me.cboallowtype = New System.Windows.Forms.ComboBox()
        Me.lnklbaddallowtype = New System.Windows.Forms.LinkLabel()
        Me.Label156 = New System.Windows.Forms.Label()
        Me.Label167 = New System.Windows.Forms.Label()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.Label163 = New System.Windows.Forms.Label()
        Me.dtpallowenddate = New NullableDatePicker()
        Me.txtremarks = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.pbEmployeePicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EmployeeInfoTabLayout.SuspendLayout()
        Me.AllowanceDetailsTabLayout.SuspendLayout()
        Me.pnlTxtLoanBalance.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.plnCboLoanType.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtEmployeeNumber
        '
        Me.txtEmployeeNumber.BackColor = System.Drawing.Color.White
        Me.txtEmployeeNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeNumber.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmployeeNumber.Location = New System.Drawing.Point(124, 47)
        Me.txtEmployeeNumber.MaxLength = 50
        Me.txtEmployeeNumber.Multiline = True
        Me.txtEmployeeNumber.Name = "txtEmployeeNumber"
        Me.txtEmployeeNumber.ReadOnly = True
        Me.txtEmployeeNumber.Size = New System.Drawing.Size(393, 52)
        Me.txtEmployeeNumber.TabIndex = 380
        Me.txtEmployeeNumber.TabStop = False
        '
        'pbEmployeePicture
        '
        Me.pbEmployeePicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployeePicture.Location = New System.Drawing.Point(28, 3)
        Me.pbEmployeePicture.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.pbEmployeePicture.Name = "pbEmployeePicture"
        Me.EmployeeInfoTabLayout.SetRowSpan(Me.pbEmployeePicture, 2)
        Me.pbEmployeePicture.Size = New System.Drawing.Size(88, 82)
        Me.pbEmployeePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployeePicture.TabIndex = 382
        Me.pbEmployeePicture.TabStop = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(315, 367)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAddAndNew
        '
        Me.btnAddAndNew.Location = New System.Drawing.Point(232, 367)
        Me.btnAddAndNew.Name = "btnAddAndNew"
        Me.btnAddAndNew.Size = New System.Drawing.Size(75, 23)
        Me.btnAddAndNew.TabIndex = 12
        Me.btnAddAndNew.Text = "Add && &New"
        Me.btnAddAndNew.UseVisualStyleBackColor = True
        '
        'txtEmployeeFirstName
        '
        Me.txtEmployeeFirstName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtEmployeeFirstName.BackColor = System.Drawing.Color.White
        Me.txtEmployeeFirstName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeFirstName.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeFirstName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtEmployeeFirstName.Location = New System.Drawing.Point(124, 13)
        Me.txtEmployeeFirstName.MaxLength = 250
        Me.txtEmployeeFirstName.Name = "txtEmployeeFirstName"
        Me.txtEmployeeFirstName.ReadOnly = True
        Me.txtEmployeeFirstName.Size = New System.Drawing.Size(668, 28)
        Me.txtEmployeeFirstName.TabIndex = 381
        Me.txtEmployeeFirstName.TabStop = False
        '
        'btnAddAndClose
        '
        Me.btnAddAndClose.Location = New System.Drawing.Point(141, 367)
        Me.btnAddAndClose.Name = "btnAddAndClose"
        Me.btnAddAndClose.Size = New System.Drawing.Size(85, 23)
        Me.btnAddAndClose.TabIndex = 14
        Me.btnAddAndClose.Text = "&Add && Close"
        Me.btnAddAndClose.UseVisualStyleBackColor = True
        '
        'EmployeeInfoTabLayout
        '
        Me.EmployeeInfoTabLayout.BackColor = System.Drawing.Color.White
        Me.EmployeeInfoTabLayout.ColumnCount = 2
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121.0!))
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 861.0!))
        Me.EmployeeInfoTabLayout.Controls.Add(Me.txtEmployeeFirstName, 1, 0)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.txtEmployeeNumber, 1, 1)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.pbEmployeePicture, 0, 0)
        Me.EmployeeInfoTabLayout.Dock = System.Windows.Forms.DockStyle.Top
        Me.EmployeeInfoTabLayout.Location = New System.Drawing.Point(0, 0)
        Me.EmployeeInfoTabLayout.Name = "EmployeeInfoTabLayout"
        Me.EmployeeInfoTabLayout.RowCount = 2
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.Size = New System.Drawing.Size(557, 102)
        Me.EmployeeInfoTabLayout.TabIndex = 11
        '
        'AllowanceDetailsTabLayout
        '
        Me.AllowanceDetailsTabLayout.ColumnCount = 1
        Me.AllowanceDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.pnlTxtLoanBalance, 0, 9)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Panel2, 0, 5)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Panel1, 0, 3)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Label1, 0, 2)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.plnCboLoanType, 0, 1)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Label156, 0, 0)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Label167, 0, 4)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.lblEndDate, 0, 6)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Label163, 0, 8)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.dtpallowenddate, 0, 7)
        Me.AllowanceDetailsTabLayout.Location = New System.Drawing.Point(28, 108)
        Me.AllowanceDetailsTabLayout.Name = "AllowanceDetailsTabLayout"
        Me.AllowanceDetailsTabLayout.RowCount = 10
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.AllowanceDetailsTabLayout.Size = New System.Drawing.Size(257, 253)
        Me.AllowanceDetailsTabLayout.TabIndex = 0
        '
        'pnlTxtLoanBalance
        '
        Me.pnlTxtLoanBalance.Controls.Add(Me.txtallowamt)
        Me.pnlTxtLoanBalance.Controls.Add(Me.Label4)
        Me.pnlTxtLoanBalance.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTxtLoanBalance.Location = New System.Drawing.Point(0, 208)
        Me.pnlTxtLoanBalance.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlTxtLoanBalance.Name = "pnlTxtLoanBalance"
        Me.pnlTxtLoanBalance.Size = New System.Drawing.Size(257, 45)
        Me.pnlTxtLoanBalance.TabIndex = 382
        '
        'txtallowamt
        '
        Me.txtallowamt.Location = New System.Drawing.Point(20, 3)
        Me.txtallowamt.Name = "txtallowamt"
        Me.txtallowamt.ShortcutsEnabled = False
        Me.txtallowamt.Size = New System.Drawing.Size(195, 20)
        Me.txtallowamt.TabIndex = 384
        Me.txtallowamt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(14, 13)
        Me.Label4.TabIndex = 383
        Me.Label4.Text = "₱"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.dtpallowstartdate)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 112)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(257, 32)
        Me.Panel2.TabIndex = 373
        '
        'dtpallowstartdate
        '
        Me.dtpallowstartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpallowstartdate.Location = New System.Drawing.Point(20, 3)
        Me.dtpallowstartdate.Name = "dtpallowstartdate"
        Me.dtpallowstartdate.Size = New System.Drawing.Size(195, 20)
        Me.dtpallowstartdate.TabIndex = 508
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(3, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 507
        Me.Label3.Text = "*"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cboallowfreq)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 64)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(257, 32)
        Me.Panel1.TabIndex = 368
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(3, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(13, 13)
        Me.Label2.TabIndex = 507
        Me.Label2.Text = "*"
        '
        'cboallowfreq
        '
        Me.cboallowfreq.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboallowfreq.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboallowfreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboallowfreq.FormattingEnabled = True
        Me.cboallowfreq.Items.AddRange(New Object() {"One time", "Daily", "Semi-monthly", "Monthly"})
        Me.cboallowfreq.Location = New System.Drawing.Point(20, 2)
        Me.cboallowfreq.Name = "cboallowfreq"
        Me.cboallowfreq.Size = New System.Drawing.Size(195, 21)
        Me.cboallowfreq.TabIndex = 360
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 48)
        Me.Label1.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 367
        Me.Label1.Text = "Frequency"
        '
        'plnCboLoanType
        '
        Me.plnCboLoanType.Controls.Add(Me.Label350)
        Me.plnCboLoanType.Controls.Add(Me.cboallowtype)
        Me.plnCboLoanType.Controls.Add(Me.lnklbaddallowtype)
        Me.plnCboLoanType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plnCboLoanType.Location = New System.Drawing.Point(0, 16)
        Me.plnCboLoanType.Margin = New System.Windows.Forms.Padding(0)
        Me.plnCboLoanType.Name = "plnCboLoanType"
        Me.plnCboLoanType.Size = New System.Drawing.Size(257, 32)
        Me.plnCboLoanType.TabIndex = 366
        '
        'Label350
        '
        Me.Label350.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label350.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label350.Location = New System.Drawing.Point(3, 4)
        Me.Label350.Name = "Label350"
        Me.Label350.Size = New System.Drawing.Size(13, 13)
        Me.Label350.TabIndex = 507
        Me.Label350.Text = "*"
        '
        'cboallowtype
        '
        Me.cboallowtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboallowtype.FormattingEnabled = True
        Me.cboallowtype.Location = New System.Drawing.Point(20, 2)
        Me.cboallowtype.Name = "cboallowtype"
        Me.cboallowtype.Size = New System.Drawing.Size(195, 21)
        Me.cboallowtype.TabIndex = 378
        '
        'lnklbaddallowtype
        '
        Me.lnklbaddallowtype.AutoSize = True
        Me.lnklbaddallowtype.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklbaddallowtype.Location = New System.Drawing.Point(222, 6)
        Me.lnklbaddallowtype.Name = "lnklbaddallowtype"
        Me.lnklbaddallowtype.Size = New System.Drawing.Size(28, 15)
        Me.lnklbaddallowtype.TabIndex = 373
        Me.lnklbaddallowtype.TabStop = True
        Me.lnklbaddallowtype.Text = "Add"
        '
        'Label156
        '
        Me.Label156.AutoSize = True
        Me.Label156.Location = New System.Drawing.Point(20, 0)
        Me.Label156.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label156.Name = "Label156"
        Me.Label156.Size = New System.Drawing.Size(31, 13)
        Me.Label156.TabIndex = 365
        Me.Label156.Text = "Type"
        '
        'Label167
        '
        Me.Label167.AutoSize = True
        Me.Label167.Location = New System.Drawing.Point(20, 96)
        Me.Label167.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label167.Name = "Label167"
        Me.Label167.Size = New System.Drawing.Size(53, 13)
        Me.Label167.TabIndex = 372
        Me.Label167.Text = "Start date"
        '
        'lblEndDate
        '
        Me.lblEndDate.AutoSize = True
        Me.lblEndDate.Location = New System.Drawing.Point(20, 144)
        Me.lblEndDate.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.Size = New System.Drawing.Size(50, 13)
        Me.lblEndDate.TabIndex = 374
        Me.lblEndDate.Text = "End date"
        '
        'Label163
        '
        Me.Label163.AutoSize = True
        Me.Label163.Location = New System.Drawing.Point(20, 192)
        Me.Label163.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label163.Name = "Label163"
        Me.Label163.Size = New System.Drawing.Size(43, 13)
        Me.Label163.TabIndex = 381
        Me.Label163.Text = "Amount"
        '
        'dtpallowenddate
        '
        Me.dtpallowenddate.Location = New System.Drawing.Point(20, 160)
        Me.dtpallowenddate.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.dtpallowenddate.Name = "dtpallowenddate"
        Me.dtpallowenddate.Size = New System.Drawing.Size(195, 22)
        Me.dtpallowenddate.TabIndex = 380
        Me.dtpallowenddate.Value = New Date(2019, 5, 24, 10, 35, 13, 830)
        '
        'txtremarks
        '
        Me.txtremarks.Location = New System.Drawing.Point(291, 126)
        Me.txtremarks.MaxLength = 255
        Me.txtremarks.Multiline = True
        Me.txtremarks.Name = "txtremarks"
        Me.txtremarks.Size = New System.Drawing.Size(240, 164)
        Me.txtremarks.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(288, 110)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Remarks"
        '
        'AddAllowanceForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(557, 429)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtremarks)
        Me.Controls.Add(Me.AllowanceDetailsTabLayout)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAddAndNew)
        Me.Controls.Add(Me.btnAddAndClose)
        Me.Controls.Add(Me.EmployeeInfoTabLayout)
        Me.MaximizeBox = False
        Me.Name = "AddAllowanceForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Allowance"
        CType(Me.pbEmployeePicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EmployeeInfoTabLayout.ResumeLayout(False)
        Me.EmployeeInfoTabLayout.PerformLayout()
        Me.AllowanceDetailsTabLayout.ResumeLayout(False)
        Me.AllowanceDetailsTabLayout.PerformLayout()
        Me.pnlTxtLoanBalance.ResumeLayout(False)
        Me.pnlTxtLoanBalance.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.plnCboLoanType.ResumeLayout(False)
        Me.plnCboLoanType.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtEmployeeNumber As TextBox
    Friend WithEvents pbEmployeePicture As PictureBox
    Friend WithEvents EmployeeInfoTabLayout As TableLayoutPanel
    Friend WithEvents txtEmployeeFirstName As TextBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnAddAndNew As Button
    Friend WithEvents btnAddAndClose As Button
    Friend WithEvents AllowanceDetailsTabLayout As TableLayoutPanel
    Friend WithEvents Label163 As Label
    Friend WithEvents dtpallowenddate As NullableDatePicker
    Friend WithEvents lblEndDate As Label
    Friend WithEvents Label167 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents cboallowfreq As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents plnCboLoanType As Panel
    Friend WithEvents Label350 As Label
    Friend WithEvents cboallowtype As ComboBox
    Friend WithEvents lnklbaddallowtype As LinkLabel
    Friend WithEvents Label156 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents dtpallowstartdate As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents pnlTxtLoanBalance As Panel
    Friend WithEvents txtallowamt As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtremarks As TextBox
    Friend WithEvents Label5 As Label
End Class
