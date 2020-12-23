<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoanUserControl
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
        Me.LoanDetailsTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtLoanStatus = New System.Windows.Forms.TextBox()
        Me.cmbLoanStatus = New System.Windows.Forms.ComboBox()
        Me.pnlTxtLoanBalance = New System.Windows.Forms.Panel()
        Me.lblLoanBalancePesoSign = New System.Windows.Forms.Label()
        Me.txtLoanBalance = New System.Windows.Forms.TextBox()
        Me.lblLoanType = New System.Windows.Forms.Label()
        Me.plnCboLoanType = New System.Windows.Forms.Panel()
        Me.cboLoanType = New System.Windows.Forms.ComboBox()
        Me.Label350 = New System.Windows.Forms.Label()
        Me.lnlAddLoanType = New System.Windows.Forms.LinkLabel()
        Me.lblLoanNumber = New System.Windows.Forms.Label()
        Me.txtLoanNumber = New System.Windows.Forms.TextBox()
        Me.dtpDateFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.lblRemarks = New System.Windows.Forms.Label()
        Me.lblLoanBalance = New System.Windows.Forms.Label()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.lblTotalLoanAmount = New System.Windows.Forms.Label()
        Me.pnlTxtLoanAmount = New System.Windows.Forms.Panel()
        Me.Label220 = New System.Windows.Forms.Label()
        Me.txtTotalLoanAmount = New System.Windows.Forms.TextBox()
        Me.lblDeductionAmount = New System.Windows.Forms.Label()
        Me.pnlTxtDeductionAmount = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDeductionAmount = New System.Windows.Forms.TextBox()
        Me.lblLoanStatus = New System.Windows.Forms.Label()
        Me.lblLoanInterestPercentage = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.txtLoanInterestPercentage = New System.Windows.Forms.TextBox()
        Me.lblDeductionSchedule = New System.Windows.Forms.Label()
        Me.cmbDeductionSchedule = New System.Windows.Forms.ComboBox()
        Me.LoanDetailsTabLayout.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlTxtLoanBalance.SuspendLayout()
        Me.plnCboLoanType.SuspendLayout()
        Me.pnlTxtLoanAmount.SuspendLayout()
        Me.pnlTxtDeductionAmount.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'LoanDetailsTabLayout
        '
        Me.LoanDetailsTabLayout.ColumnCount = 3
        Me.LoanDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.LoanDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.LoanDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34!))
        Me.LoanDetailsTabLayout.Controls.Add(Me.Panel1, 1, 5)
        Me.LoanDetailsTabLayout.Controls.Add(Me.pnlTxtLoanBalance, 0, 7)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanType, 0, 0)
        Me.LoanDetailsTabLayout.Controls.Add(Me.plnCboLoanType, 0, 1)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanNumber, 0, 2)
        Me.LoanDetailsTabLayout.Controls.Add(Me.txtLoanNumber, 0, 3)
        Me.LoanDetailsTabLayout.Controls.Add(Me.dtpDateFrom, 1, 3)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblDateFrom, 1, 2)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblRemarks, 2, 0)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanBalance, 0, 6)
        Me.LoanDetailsTabLayout.Controls.Add(Me.txtRemarks, 2, 1)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblTotalLoanAmount, 0, 4)
        Me.LoanDetailsTabLayout.Controls.Add(Me.pnlTxtLoanAmount, 0, 5)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblDeductionAmount, 1, 0)
        Me.LoanDetailsTabLayout.Controls.Add(Me.pnlTxtDeductionAmount, 1, 1)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanStatus, 1, 4)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanInterestPercentage, 1, 8)
        Me.LoanDetailsTabLayout.Controls.Add(Me.Panel3, 1, 9)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblDeductionSchedule, 1, 6)
        Me.LoanDetailsTabLayout.Controls.Add(Me.cmbDeductionSchedule, 1, 7)
        Me.LoanDetailsTabLayout.Dock = System.Windows.Forms.DockStyle.Top
        Me.LoanDetailsTabLayout.Location = New System.Drawing.Point(0, 0)
        Me.LoanDetailsTabLayout.Name = "LoanDetailsTabLayout"
        Me.LoanDetailsTabLayout.RowCount = 10
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.LoanDetailsTabLayout.Size = New System.Drawing.Size(803, 247)
        Me.LoanDetailsTabLayout.TabIndex = 6
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtLoanStatus)
        Me.Panel1.Controls.Add(Me.cmbLoanStatus)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(270, 115)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(261, 26)
        Me.Panel1.TabIndex = 392
        '
        'txtLoanStatus
        '
        Me.txtLoanStatus.Enabled = False
        Me.txtLoanStatus.Location = New System.Drawing.Point(20, 2)
        Me.txtLoanStatus.Name = "txtLoanStatus"
        Me.txtLoanStatus.Size = New System.Drawing.Size(195, 20)
        Me.txtLoanStatus.TabIndex = 392
        '
        'cmbLoanStatus
        '
        Me.cmbLoanStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLoanStatus.FormattingEnabled = True
        Me.cmbLoanStatus.Location = New System.Drawing.Point(20, 2)
        Me.cmbLoanStatus.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.cmbLoanStatus.MaxLength = 50
        Me.cmbLoanStatus.Name = "cmbLoanStatus"
        Me.cmbLoanStatus.Size = New System.Drawing.Size(195, 21)
        Me.cmbLoanStatus.TabIndex = 361
        '
        'pnlTxtLoanBalance
        '
        Me.pnlTxtLoanBalance.Controls.Add(Me.lblLoanBalancePesoSign)
        Me.pnlTxtLoanBalance.Controls.Add(Me.txtLoanBalance)
        Me.pnlTxtLoanBalance.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTxtLoanBalance.Location = New System.Drawing.Point(0, 160)
        Me.pnlTxtLoanBalance.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlTxtLoanBalance.Name = "pnlTxtLoanBalance"
        Me.pnlTxtLoanBalance.Size = New System.Drawing.Size(267, 32)
        Me.pnlTxtLoanBalance.TabIndex = 356
        '
        'lblLoanBalancePesoSign
        '
        Me.lblLoanBalancePesoSign.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanBalancePesoSign.AutoSize = True
        Me.lblLoanBalancePesoSign.Location = New System.Drawing.Point(3, 4)
        Me.lblLoanBalancePesoSign.Name = "lblLoanBalancePesoSign"
        Me.lblLoanBalancePesoSign.Size = New System.Drawing.Size(14, 13)
        Me.lblLoanBalancePesoSign.TabIndex = 383
        Me.lblLoanBalancePesoSign.Text = "₱"
        Me.lblLoanBalancePesoSign.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLoanBalance
        '
        Me.txtLoanBalance.Enabled = False
        Me.txtLoanBalance.Location = New System.Drawing.Point(20, 2)
        Me.txtLoanBalance.Name = "txtLoanBalance"
        Me.txtLoanBalance.Size = New System.Drawing.Size(195, 20)
        Me.txtLoanBalance.TabIndex = 356
        '
        'lblLoanType
        '
        Me.lblLoanType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanType.AutoSize = True
        Me.lblLoanType.Location = New System.Drawing.Point(20, 3)
        Me.lblLoanType.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanType.Name = "lblLoanType"
        Me.lblLoanType.Size = New System.Drawing.Size(66, 13)
        Me.lblLoanType.TabIndex = 379
        Me.lblLoanType.Text = "Type of loan"
        '
        'plnCboLoanType
        '
        Me.plnCboLoanType.Controls.Add(Me.cboLoanType)
        Me.plnCboLoanType.Controls.Add(Me.Label350)
        Me.plnCboLoanType.Controls.Add(Me.lnlAddLoanType)
        Me.plnCboLoanType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plnCboLoanType.Location = New System.Drawing.Point(0, 16)
        Me.plnCboLoanType.Margin = New System.Windows.Forms.Padding(0)
        Me.plnCboLoanType.Name = "plnCboLoanType"
        Me.plnCboLoanType.Size = New System.Drawing.Size(267, 32)
        Me.plnCboLoanType.TabIndex = 353
        '
        'cboLoanType
        '
        Me.cboLoanType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLoanType.FormattingEnabled = True
        Me.cboLoanType.Location = New System.Drawing.Point(20, 2)
        Me.cboLoanType.Name = "cboLoanType"
        Me.cboLoanType.Size = New System.Drawing.Size(195, 21)
        Me.cboLoanType.TabIndex = 353
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
        'lnlAddLoanType
        '
        Me.lnlAddLoanType.AutoSize = True
        Me.lnlAddLoanType.Location = New System.Drawing.Point(222, 6)
        Me.lnlAddLoanType.Name = "lnlAddLoanType"
        Me.lnlAddLoanType.Size = New System.Drawing.Size(26, 13)
        Me.lnlAddLoanType.TabIndex = 354
        Me.lnlAddLoanType.TabStop = True
        Me.lnlAddLoanType.Text = "Add"
        '
        'lblLoanNumber
        '
        Me.lblLoanNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanNumber.AutoSize = True
        Me.lblLoanNumber.Location = New System.Drawing.Point(20, 51)
        Me.lblLoanNumber.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanNumber.Name = "lblLoanNumber"
        Me.lblLoanNumber.Size = New System.Drawing.Size(71, 13)
        Me.lblLoanNumber.TabIndex = 357
        Me.lblLoanNumber.Text = "Loan Number"
        '
        'txtLoanNumber
        '
        Me.txtLoanNumber.Location = New System.Drawing.Point(20, 67)
        Me.txtLoanNumber.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.txtLoanNumber.Name = "txtLoanNumber"
        Me.txtLoanNumber.Size = New System.Drawing.Size(195, 20)
        Me.txtLoanNumber.TabIndex = 354
        '
        'dtpDateFrom
        '
        Me.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDateFrom.Location = New System.Drawing.Point(287, 67)
        Me.dtpDateFrom.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.dtpDateFrom.Name = "dtpDateFrom"
        Me.dtpDateFrom.Size = New System.Drawing.Size(195, 20)
        Me.dtpDateFrom.TabIndex = 357
        '
        'lblDateFrom
        '
        Me.lblDateFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(287, 51)
        Me.lblDateFrom.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(55, 13)
        Me.lblDateFrom.TabIndex = 372
        Me.lblDateFrom.Text = "Start Date"
        '
        'lblRemarks
        '
        Me.lblRemarks.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblRemarks.AutoSize = True
        Me.lblRemarks.Location = New System.Drawing.Point(554, 3)
        Me.lblRemarks.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblRemarks.Name = "lblRemarks"
        Me.lblRemarks.Size = New System.Drawing.Size(49, 13)
        Me.lblRemarks.TabIndex = 370
        Me.lblRemarks.Text = "Remarks"
        '
        'lblLoanBalance
        '
        Me.lblLoanBalance.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanBalance.AutoSize = True
        Me.lblLoanBalance.Location = New System.Drawing.Point(20, 147)
        Me.lblLoanBalance.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanBalance.Name = "lblLoanBalance"
        Me.lblLoanBalance.Size = New System.Drawing.Size(94, 13)
        Me.lblLoanBalance.TabIndex = 365
        Me.lblLoanBalance.Text = "Total Balance Left"
        '
        'txtRemarks
        '
        Me.txtRemarks.Location = New System.Drawing.Point(554, 19)
        Me.txtRemarks.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.txtRemarks.MaxLength = 2000
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.LoanDetailsTabLayout.SetRowSpan(Me.txtRemarks, 7)
        Me.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRemarks.Size = New System.Drawing.Size(195, 170)
        Me.txtRemarks.TabIndex = 364
        '
        'lblTotalLoanAmount
        '
        Me.lblTotalLoanAmount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalLoanAmount.AutoSize = True
        Me.lblTotalLoanAmount.Location = New System.Drawing.Point(20, 99)
        Me.lblTotalLoanAmount.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblTotalLoanAmount.Name = "lblTotalLoanAmount"
        Me.lblTotalLoanAmount.Size = New System.Drawing.Size(97, 13)
        Me.lblTotalLoanAmount.TabIndex = 362
        Me.lblTotalLoanAmount.Text = "Total Loan Amount"
        '
        'pnlTxtLoanAmount
        '
        Me.pnlTxtLoanAmount.Controls.Add(Me.Label220)
        Me.pnlTxtLoanAmount.Controls.Add(Me.txtTotalLoanAmount)
        Me.pnlTxtLoanAmount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTxtLoanAmount.Location = New System.Drawing.Point(0, 112)
        Me.pnlTxtLoanAmount.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlTxtLoanAmount.Name = "pnlTxtLoanAmount"
        Me.pnlTxtLoanAmount.Size = New System.Drawing.Size(267, 32)
        Me.pnlTxtLoanAmount.TabIndex = 355
        '
        'Label220
        '
        Me.Label220.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label220.AutoSize = True
        Me.Label220.Location = New System.Drawing.Point(3, 4)
        Me.Label220.Name = "Label220"
        Me.Label220.Size = New System.Drawing.Size(14, 13)
        Me.Label220.TabIndex = 383
        Me.Label220.Text = "₱"
        Me.Label220.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTotalLoanAmount
        '
        Me.txtTotalLoanAmount.Location = New System.Drawing.Point(20, 2)
        Me.txtTotalLoanAmount.Name = "txtTotalLoanAmount"
        Me.txtTotalLoanAmount.ShortcutsEnabled = False
        Me.txtTotalLoanAmount.Size = New System.Drawing.Size(195, 20)
        Me.txtTotalLoanAmount.TabIndex = 355
        '
        'lblDeductionAmount
        '
        Me.lblDeductionAmount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDeductionAmount.AutoSize = True
        Me.lblDeductionAmount.Location = New System.Drawing.Point(287, 3)
        Me.lblDeductionAmount.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblDeductionAmount.Name = "lblDeductionAmount"
        Me.lblDeductionAmount.Size = New System.Drawing.Size(95, 13)
        Me.lblDeductionAmount.TabIndex = 368
        Me.lblDeductionAmount.Text = "Deduction Amount"
        '
        'pnlTxtDeductionAmount
        '
        Me.pnlTxtDeductionAmount.Controls.Add(Me.Label1)
        Me.pnlTxtDeductionAmount.Controls.Add(Me.txtDeductionAmount)
        Me.pnlTxtDeductionAmount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTxtDeductionAmount.Location = New System.Drawing.Point(267, 16)
        Me.pnlTxtDeductionAmount.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlTxtDeductionAmount.Name = "pnlTxtDeductionAmount"
        Me.pnlTxtDeductionAmount.Size = New System.Drawing.Size(267, 32)
        Me.pnlTxtDeductionAmount.TabIndex = 360
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(14, 13)
        Me.Label1.TabIndex = 383
        Me.Label1.Text = "₱"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDeductionAmount
        '
        Me.txtDeductionAmount.Location = New System.Drawing.Point(20, 2)
        Me.txtDeductionAmount.Name = "txtDeductionAmount"
        Me.txtDeductionAmount.ShortcutsEnabled = False
        Me.txtDeductionAmount.Size = New System.Drawing.Size(195, 20)
        Me.txtDeductionAmount.TabIndex = 360
        '
        'lblLoanStatus
        '
        Me.lblLoanStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanStatus.AutoSize = True
        Me.lblLoanStatus.Location = New System.Drawing.Point(287, 99)
        Me.lblLoanStatus.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanStatus.Name = "lblLoanStatus"
        Me.lblLoanStatus.Size = New System.Drawing.Size(37, 13)
        Me.lblLoanStatus.TabIndex = 371
        Me.lblLoanStatus.Text = "Status"
        '
        'lblLoanInterestPercentage
        '
        Me.lblLoanInterestPercentage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanInterestPercentage.AutoSize = True
        Me.lblLoanInterestPercentage.Location = New System.Drawing.Point(287, 195)
        Me.lblLoanInterestPercentage.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanInterestPercentage.Name = "lblLoanInterestPercentage"
        Me.lblLoanInterestPercentage.Size = New System.Drawing.Size(125, 13)
        Me.lblLoanInterestPercentage.TabIndex = 391
        Me.lblLoanInterestPercentage.Text = "Loan interest percentage"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.txtLoanInterestPercentage)
        Me.Panel3.Location = New System.Drawing.Point(267, 208)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(267, 32)
        Me.Panel3.TabIndex = 362
        '
        'txtLoanInterestPercentage
        '
        Me.txtLoanInterestPercentage.BackColor = System.Drawing.Color.White
        Me.txtLoanInterestPercentage.Location = New System.Drawing.Point(20, 2)
        Me.txtLoanInterestPercentage.Name = "txtLoanInterestPercentage"
        Me.txtLoanInterestPercentage.ShortcutsEnabled = False
        Me.txtLoanInterestPercentage.Size = New System.Drawing.Size(195, 20)
        Me.txtLoanInterestPercentage.TabIndex = 362
        '
        'lblDeductionSchedule
        '
        Me.lblDeductionSchedule.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDeductionSchedule.AutoSize = True
        Me.lblDeductionSchedule.Location = New System.Drawing.Point(287, 147)
        Me.lblDeductionSchedule.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblDeductionSchedule.Name = "lblDeductionSchedule"
        Me.lblDeductionSchedule.Size = New System.Drawing.Size(104, 13)
        Me.lblDeductionSchedule.TabIndex = 375
        Me.lblDeductionSchedule.Text = "Deduction Schedule"
        '
        'cmbDeductionSchedule
        '
        Me.cmbDeductionSchedule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDeductionSchedule.FormattingEnabled = True
        Me.cmbDeductionSchedule.Location = New System.Drawing.Point(287, 163)
        Me.cmbDeductionSchedule.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.cmbDeductionSchedule.Name = "cmbDeductionSchedule"
        Me.cmbDeductionSchedule.Size = New System.Drawing.Size(195, 21)
        Me.cmbDeductionSchedule.TabIndex = 363
        '
        'LoanUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LoanDetailsTabLayout)
        Me.Name = "LoanUserControl"
        Me.Size = New System.Drawing.Size(803, 247)
        Me.LoanDetailsTabLayout.ResumeLayout(False)
        Me.LoanDetailsTabLayout.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlTxtLoanBalance.ResumeLayout(False)
        Me.pnlTxtLoanBalance.PerformLayout()
        Me.plnCboLoanType.ResumeLayout(False)
        Me.plnCboLoanType.PerformLayout()
        Me.pnlTxtLoanAmount.ResumeLayout(False)
        Me.pnlTxtLoanAmount.PerformLayout()
        Me.pnlTxtDeductionAmount.ResumeLayout(False)
        Me.pnlTxtDeductionAmount.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LoanDetailsTabLayout As TableLayoutPanel
    Friend WithEvents pnlTxtLoanBalance As Panel
    Friend WithEvents lblLoanBalancePesoSign As Label
    Friend WithEvents txtLoanBalance As TextBox
    Friend WithEvents lblLoanType As Label
    Friend WithEvents plnCboLoanType As Panel
    Friend WithEvents cboLoanType As ComboBox
    Friend WithEvents Label350 As Label
    Friend WithEvents lnlAddLoanType As LinkLabel
    Friend WithEvents lblLoanNumber As Label
    Friend WithEvents txtLoanNumber As TextBox
    Friend WithEvents dtpDateFrom As DateTimePicker
    Friend WithEvents lblDateFrom As Label
    Friend WithEvents lblRemarks As Label
    Friend WithEvents lblLoanBalance As Label
    Friend WithEvents txtRemarks As TextBox
    Friend WithEvents lblTotalLoanAmount As Label
    Friend WithEvents pnlTxtLoanAmount As Panel
    Friend WithEvents Label220 As Label
    Friend WithEvents txtTotalLoanAmount As TextBox
    Friend WithEvents lblDeductionAmount As Label
    Friend WithEvents pnlTxtDeductionAmount As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents txtDeductionAmount As TextBox
    Friend WithEvents lblLoanStatus As Label
    Friend WithEvents lblLoanInterestPercentage As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents txtLoanInterestPercentage As TextBox
    Friend WithEvents lblDeductionSchedule As Label
    Friend WithEvents cmbDeductionSchedule As ComboBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents txtLoanStatus As TextBox
    Friend WithEvents cmbLoanStatus As ComboBox
End Class
