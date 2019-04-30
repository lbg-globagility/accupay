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
        Me.pnlTxtLoanBalance = New System.Windows.Forms.Panel()
        Me.dtpallowenddate = New System.Windows.Forms.DateTimePicker()
        Me.txtEmployeeNumber = New System.Windows.Forms.TextBox()
        Me.lblLoanType = New System.Windows.Forms.Label()
        Me.plnCboLoanType = New System.Windows.Forms.Panel()
        Me.cboAllowType = New System.Windows.Forms.ComboBox()
        Me.Label350 = New System.Windows.Forms.Label()
        Me.lnlAddAllowanceType = New System.Windows.Forms.LinkLabel()
        Me.pbEmployeePicture = New System.Windows.Forms.PictureBox()
        Me.dtpallowstartdate = New System.Windows.Forms.DateTimePicker()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.lblLoanBalance = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnAddAndNew = New System.Windows.Forms.Button()
        Me.txtEmployeeFirstName = New System.Windows.Forms.TextBox()
        Me.btnAddAndClose = New System.Windows.Forms.Button()
        Me.lblTotalLoanAmount = New System.Windows.Forms.Label()
        Me.pnlTxtLoanAmount = New System.Windows.Forms.Panel()
        Me.lblreqstartdate = New System.Windows.Forms.Label()
        Me.Label220 = New System.Windows.Forms.Label()
        Me.txtPeriodicAllowanceAmount = New System.Windows.Forms.TextBox()
        Me.LoanDetailsTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.lblLoanNumber = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cboAllowFreq = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.EmployeeInfoTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlTxtLoanBalance.SuspendLayout()
        Me.plnCboLoanType.SuspendLayout()
        CType(Me.pbEmployeePicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTxtLoanAmount.SuspendLayout()
        Me.LoanDetailsTabLayout.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.EmployeeInfoTabLayout.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTxtLoanBalance
        '
        Me.pnlTxtLoanBalance.Controls.Add(Me.dtpallowenddate)
        Me.pnlTxtLoanBalance.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTxtLoanBalance.Location = New System.Drawing.Point(0, 160)
        Me.pnlTxtLoanBalance.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlTxtLoanBalance.Name = "pnlTxtLoanBalance"
        Me.pnlTxtLoanBalance.Size = New System.Drawing.Size(566, 32)
        Me.pnlTxtLoanBalance.TabIndex = 356
        '
        'dtpallowenddate
        '
        Me.dtpallowenddate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpallowenddate.Location = New System.Drawing.Point(20, 4)
        Me.dtpallowenddate.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.dtpallowenddate.Name = "dtpallowenddate"
        Me.dtpallowenddate.ShowCheckBox = True
        Me.dtpallowenddate.Size = New System.Drawing.Size(195, 20)
        Me.dtpallowenddate.TabIndex = 381
        '
        'txtEmployeeNumber
        '
        Me.txtEmployeeNumber.BackColor = System.Drawing.Color.White
        Me.txtEmployeeNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeNumber.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmployeeNumber.Location = New System.Drawing.Point(124, 47)
        Me.txtEmployeeNumber.MaxLength = 50
        Me.txtEmployeeNumber.Name = "txtEmployeeNumber"
        Me.txtEmployeeNumber.ReadOnly = True
        Me.txtEmployeeNumber.Size = New System.Drawing.Size(516, 22)
        Me.txtEmployeeNumber.TabIndex = 380
        '
        'lblLoanType
        '
        Me.lblLoanType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanType.AutoSize = True
        Me.lblLoanType.Location = New System.Drawing.Point(20, 3)
        Me.lblLoanType.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanType.Name = "lblLoanType"
        Me.lblLoanType.Size = New System.Drawing.Size(94, 13)
        Me.lblLoanType.TabIndex = 379
        Me.lblLoanType.Text = "Type of allowance"
        '
        'plnCboLoanType
        '
        Me.plnCboLoanType.Controls.Add(Me.cboAllowType)
        Me.plnCboLoanType.Controls.Add(Me.Label350)
        Me.plnCboLoanType.Controls.Add(Me.lnlAddAllowanceType)
        Me.plnCboLoanType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plnCboLoanType.Location = New System.Drawing.Point(0, 16)
        Me.plnCboLoanType.Margin = New System.Windows.Forms.Padding(0)
        Me.plnCboLoanType.Name = "plnCboLoanType"
        Me.plnCboLoanType.Size = New System.Drawing.Size(566, 32)
        Me.plnCboLoanType.TabIndex = 353
        '
        'cboAllowType
        '
        Me.cboAllowType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAllowType.FormattingEnabled = True
        Me.cboAllowType.Location = New System.Drawing.Point(20, 2)
        Me.cboAllowType.Name = "cboAllowType"
        Me.cboAllowType.Size = New System.Drawing.Size(195, 21)
        Me.cboAllowType.TabIndex = 353
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
        'lnlAddAllowanceType
        '
        Me.lnlAddAllowanceType.AutoSize = True
        Me.lnlAddAllowanceType.Location = New System.Drawing.Point(222, 6)
        Me.lnlAddAllowanceType.Name = "lnlAddAllowanceType"
        Me.lnlAddAllowanceType.Size = New System.Drawing.Size(26, 13)
        Me.lnlAddAllowanceType.TabIndex = 354
        Me.lnlAddAllowanceType.TabStop = True
        Me.lnlAddAllowanceType.Text = "Add"
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
        'dtpallowstartdate
        '
        Me.dtpallowstartdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpallowstartdate.Location = New System.Drawing.Point(20, 4)
        Me.dtpallowstartdate.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.dtpallowstartdate.Name = "dtpallowstartdate"
        Me.dtpallowstartdate.Size = New System.Drawing.Size(195, 20)
        Me.dtpallowstartdate.TabIndex = 357
        '
        'lblDateFrom
        '
        Me.lblDateFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(20, 195)
        Me.lblDateFrom.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(43, 13)
        Me.lblDateFrom.TabIndex = 372
        Me.lblDateFrom.Text = "Amount"
        '
        'lblLoanBalance
        '
        Me.lblLoanBalance.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanBalance.AutoSize = True
        Me.lblLoanBalance.Location = New System.Drawing.Point(20, 147)
        Me.lblLoanBalance.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanBalance.Name = "lblLoanBalance"
        Me.lblLoanBalance.Size = New System.Drawing.Size(52, 13)
        Me.lblLoanBalance.TabIndex = 365
        Me.lblLoanBalance.Text = "End Date"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(452, 400)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAddAndNew
        '
        Me.btnAddAndNew.Location = New System.Drawing.Point(369, 400)
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
        '
        'btnAddAndClose
        '
        Me.btnAddAndClose.Location = New System.Drawing.Point(278, 400)
        Me.btnAddAndClose.Name = "btnAddAndClose"
        Me.btnAddAndClose.Size = New System.Drawing.Size(85, 23)
        Me.btnAddAndClose.TabIndex = 14
        Me.btnAddAndClose.Text = "&Add && Close"
        Me.btnAddAndClose.UseVisualStyleBackColor = True
        '
        'lblTotalLoanAmount
        '
        Me.lblTotalLoanAmount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalLoanAmount.AutoSize = True
        Me.lblTotalLoanAmount.Location = New System.Drawing.Point(20, 99)
        Me.lblTotalLoanAmount.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblTotalLoanAmount.Name = "lblTotalLoanAmount"
        Me.lblTotalLoanAmount.Size = New System.Drawing.Size(55, 13)
        Me.lblTotalLoanAmount.TabIndex = 362
        Me.lblTotalLoanAmount.Text = "Start Date"
        '
        'pnlTxtLoanAmount
        '
        Me.pnlTxtLoanAmount.Controls.Add(Me.lblreqstartdate)
        Me.pnlTxtLoanAmount.Controls.Add(Me.dtpallowstartdate)
        Me.pnlTxtLoanAmount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTxtLoanAmount.Location = New System.Drawing.Point(0, 112)
        Me.pnlTxtLoanAmount.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlTxtLoanAmount.Name = "pnlTxtLoanAmount"
        Me.pnlTxtLoanAmount.Size = New System.Drawing.Size(566, 32)
        Me.pnlTxtLoanAmount.TabIndex = 355
        '
        'lblreqstartdate
        '
        Me.lblreqstartdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblreqstartdate.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblreqstartdate.Location = New System.Drawing.Point(3, 2)
        Me.lblreqstartdate.Name = "lblreqstartdate"
        Me.lblreqstartdate.Size = New System.Drawing.Size(13, 13)
        Me.lblreqstartdate.TabIndex = 511
        Me.lblreqstartdate.Text = "*"
        '
        'Label220
        '
        Me.Label220.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label220.AutoSize = True
        Me.Label220.Location = New System.Drawing.Point(3, 7)
        Me.Label220.Name = "Label220"
        Me.Label220.Size = New System.Drawing.Size(14, 13)
        Me.Label220.TabIndex = 383
        Me.Label220.Text = "₱"
        Me.Label220.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPeriodicAllowanceAmount
        '
        Me.txtPeriodicAllowanceAmount.Location = New System.Drawing.Point(17, 3)
        Me.txtPeriodicAllowanceAmount.Name = "txtPeriodicAllowanceAmount"
        Me.txtPeriodicAllowanceAmount.ShortcutsEnabled = False
        Me.txtPeriodicAllowanceAmount.Size = New System.Drawing.Size(195, 20)
        Me.txtPeriodicAllowanceAmount.TabIndex = 355
        '
        'LoanDetailsTabLayout
        '
        Me.LoanDetailsTabLayout.ColumnCount = 1
        Me.LoanDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.LoanDetailsTabLayout.Controls.Add(Me.pnlTxtLoanBalance, 0, 7)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanType, 0, 0)
        Me.LoanDetailsTabLayout.Controls.Add(Me.plnCboLoanType, 0, 1)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanNumber, 0, 2)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblDateFrom, 0, 8)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanBalance, 0, 6)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblTotalLoanAmount, 0, 4)
        Me.LoanDetailsTabLayout.Controls.Add(Me.pnlTxtLoanAmount, 0, 5)
        Me.LoanDetailsTabLayout.Controls.Add(Me.Panel1, 0, 3)
        Me.LoanDetailsTabLayout.Controls.Add(Me.Panel2, 0, 9)
        Me.LoanDetailsTabLayout.Dock = System.Windows.Forms.DockStyle.Top
        Me.LoanDetailsTabLayout.Location = New System.Drawing.Point(0, 88)
        Me.LoanDetailsTabLayout.Name = "LoanDetailsTabLayout"
        Me.LoanDetailsTabLayout.RowCount = 11
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.LoanDetailsTabLayout.Size = New System.Drawing.Size(566, 247)
        Me.LoanDetailsTabLayout.TabIndex = 10
        '
        'lblLoanNumber
        '
        Me.lblLoanNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanNumber.AutoSize = True
        Me.lblLoanNumber.Location = New System.Drawing.Point(20, 51)
        Me.lblLoanNumber.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanNumber.Name = "lblLoanNumber"
        Me.lblLoanNumber.Size = New System.Drawing.Size(57, 13)
        Me.lblLoanNumber.TabIndex = 357
        Me.lblLoanNumber.Text = "Frequency"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cboAllowFreq)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 67)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(560, 28)
        Me.Panel1.TabIndex = 380
        '
        'cboAllowFreq
        '
        Me.cboAllowFreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAllowFreq.FormattingEnabled = True
        Me.cboAllowFreq.Items.AddRange(New Object() {"One time", "Daily", "Semi-monthly", "Monthly"})
        Me.cboAllowFreq.Location = New System.Drawing.Point(17, 4)
        Me.cboAllowFreq.Name = "cboAllowFreq"
        Me.cboAllowFreq.Size = New System.Drawing.Size(195, 21)
        Me.cboAllowFreq.TabIndex = 508
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(-1, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 510
        Me.Label1.Text = "*"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label220)
        Me.Panel2.Controls.Add(Me.txtPeriodicAllowanceAmount)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 211)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(560, 26)
        Me.Panel2.TabIndex = 384
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
        Me.EmployeeInfoTabLayout.Size = New System.Drawing.Size(566, 88)
        Me.EmployeeInfoTabLayout.TabIndex = 11
        '
        'AddAllowanceForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(566, 450)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAddAndNew)
        Me.Controls.Add(Me.btnAddAndClose)
        Me.Controls.Add(Me.LoanDetailsTabLayout)
        Me.Controls.Add(Me.EmployeeInfoTabLayout)
        Me.Name = "AddAllowanceForm"
        Me.Text = "New Allowance"
        Me.pnlTxtLoanBalance.ResumeLayout(False)
        Me.plnCboLoanType.ResumeLayout(False)
        Me.plnCboLoanType.PerformLayout()
        CType(Me.pbEmployeePicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTxtLoanAmount.ResumeLayout(False)
        Me.LoanDetailsTabLayout.ResumeLayout(False)
        Me.LoanDetailsTabLayout.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.EmployeeInfoTabLayout.ResumeLayout(False)
        Me.EmployeeInfoTabLayout.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlTxtLoanBalance As Panel
    Friend WithEvents txtEmployeeNumber As TextBox
    Friend WithEvents lblLoanType As Label
    Friend WithEvents plnCboLoanType As Panel
    Friend WithEvents pbEmployeePicture As PictureBox
    Friend WithEvents EmployeeInfoTabLayout As TableLayoutPanel
    Friend WithEvents txtEmployeeFirstName As TextBox
    Friend WithEvents dtpallowstartdate As DateTimePicker
    Friend WithEvents lblDateFrom As Label
    Friend WithEvents lblLoanBalance As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnAddAndNew As Button
    Friend WithEvents LoanDetailsTabLayout As TableLayoutPanel
    Friend WithEvents lblTotalLoanAmount As Label
    Friend WithEvents pnlTxtLoanAmount As Panel
    Friend WithEvents Label220 As Label
    Friend WithEvents txtPeriodicAllowanceAmount As TextBox
    Friend WithEvents btnAddAndClose As Button
    Friend WithEvents lblLoanNumber As Label
    Friend WithEvents cboAllowType As ComboBox
    Friend WithEvents Label350 As Label
    Friend WithEvents lnlAddAllowanceType As LinkLabel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents cboAllowFreq As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents dtpallowenddate As DateTimePicker
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblreqstartdate As Label
End Class
