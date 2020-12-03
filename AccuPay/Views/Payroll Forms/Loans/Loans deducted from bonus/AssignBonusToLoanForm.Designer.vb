<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AssignBonusToLoanForm
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.DetailsTabControl = New System.Windows.Forms.TabControl()
        Me.tbpDetails = New System.Windows.Forms.TabPage()
        Me.LoanDetailsTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.dtpDateFrom = New System.Windows.Forms.TextBox()
        Me.pnlTxtLoanBalance = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtLoanBalance = New System.Windows.Forms.TextBox()
        Me.lblLoanType = New System.Windows.Forms.Label()
        Me.plnCboLoanType = New System.Windows.Forms.Panel()
        Me.cboLoanType = New System.Windows.Forms.ComboBox()
        Me.lblLoanNumber = New System.Windows.Forms.Label()
        Me.txtLoanNumber = New System.Windows.Forms.TextBox()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.lblRemarks = New System.Windows.Forms.Label()
        Me.lblLoanBalance = New System.Windows.Forms.Label()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.lblTotalLoanAmount = New System.Windows.Forms.Label()
        Me.pnlTxtLoanAmount = New System.Windows.Forms.Panel()
        Me.Label220 = New System.Windows.Forms.Label()
        Me.txtTotalLoanAmount = New System.Windows.Forms.TextBox()
        Me.lblNumberOfPayPeriodLeft = New System.Windows.Forms.Label()
        Me.txtNumberOfPayPeriodLeft = New System.Windows.Forms.TextBox()
        Me.lblDeductionAmount = New System.Windows.Forms.Label()
        Me.pnlTxtDeductionAmount = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDeductionAmount = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtLoanStatus = New System.Windows.Forms.TextBox()
        Me.cmbLoanStatus = New System.Windows.Forms.ComboBox()
        Me.lblLoanStatus = New System.Windows.Forms.Label()
        Me.lblLoanInterestPercentage = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtLoanInterestPercentage = New System.Windows.Forms.TextBox()
        Me.lblNumberOfPayPeriod = New System.Windows.Forms.Label()
        Me.lblDeductionSchedule = New System.Windows.Forms.Label()
        Me.txtNumberOfPayPeriod = New System.Windows.Forms.TextBox()
        Me.cmbDeductionSchedule = New System.Windows.Forms.ComboBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.dgvBonuses = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.BonusId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BonusAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Frequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EffectiveDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EffectiveEndDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colIsFullAmount = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colAmountPayment = New DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblLoanPayPeriodLeft = New System.Windows.Forms.Label()
        Me.lblTotalBalanceLeft = New System.Windows.Forms.Label()
        Me.lblTotalAmountPayment = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.DetailsTabControl.SuspendLayout()
        Me.tbpDetails.SuspendLayout()
        Me.LoanDetailsTabLayout.SuspendLayout()
        Me.pnlTxtLoanBalance.SuspendLayout()
        Me.plnCboLoanType.SuspendLayout()
        Me.pnlTxtLoanAmount.SuspendLayout()
        Me.pnlTxtDeductionAmount.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dgvBonuses, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DetailsTabControl
        '
        Me.DetailsTabControl.Controls.Add(Me.tbpDetails)
        Me.DetailsTabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DetailsTabControl.Location = New System.Drawing.Point(0, 0)
        Me.DetailsTabControl.Name = "DetailsTabControl"
        Me.DetailsTabControl.SelectedIndex = 0
        Me.DetailsTabControl.Size = New System.Drawing.Size(753, 270)
        Me.DetailsTabControl.TabIndex = 6
        '
        'tbpDetails
        '
        Me.tbpDetails.Controls.Add(Me.LoanDetailsTabLayout)
        Me.tbpDetails.Location = New System.Drawing.Point(4, 22)
        Me.tbpDetails.Name = "tbpDetails"
        Me.tbpDetails.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpDetails.Size = New System.Drawing.Size(745, 244)
        Me.tbpDetails.TabIndex = 0
        Me.tbpDetails.Text = "Loan Details"
        Me.tbpDetails.UseVisualStyleBackColor = True
        '
        'LoanDetailsTabLayout
        '
        Me.LoanDetailsTabLayout.AutoScroll = True
        Me.LoanDetailsTabLayout.ColumnCount = 3
        Me.LoanDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.31678!))
        Me.LoanDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.24527!))
        Me.LoanDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.43795!))
        Me.LoanDetailsTabLayout.Controls.Add(Me.dtpDateFrom, 0, 9)
        Me.LoanDetailsTabLayout.Controls.Add(Me.pnlTxtLoanBalance, 0, 7)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanType, 0, 0)
        Me.LoanDetailsTabLayout.Controls.Add(Me.plnCboLoanType, 0, 1)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanNumber, 0, 2)
        Me.LoanDetailsTabLayout.Controls.Add(Me.txtLoanNumber, 0, 3)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblDateFrom, 0, 8)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblRemarks, 2, 0)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanBalance, 0, 6)
        Me.LoanDetailsTabLayout.Controls.Add(Me.txtRemarks, 2, 1)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblTotalLoanAmount, 0, 4)
        Me.LoanDetailsTabLayout.Controls.Add(Me.pnlTxtLoanAmount, 0, 5)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblNumberOfPayPeriodLeft, 1, 0)
        Me.LoanDetailsTabLayout.Controls.Add(Me.txtNumberOfPayPeriodLeft, 1, 1)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblDeductionAmount, 1, 2)
        Me.LoanDetailsTabLayout.Controls.Add(Me.pnlTxtDeductionAmount, 1, 3)
        Me.LoanDetailsTabLayout.Controls.Add(Me.Panel1, 1, 5)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanStatus, 1, 4)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblLoanInterestPercentage, 1, 6)
        Me.LoanDetailsTabLayout.Controls.Add(Me.Panel3, 1, 7)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblNumberOfPayPeriod, 2, 8)
        Me.LoanDetailsTabLayout.Controls.Add(Me.lblDeductionSchedule, 1, 8)
        Me.LoanDetailsTabLayout.Controls.Add(Me.txtNumberOfPayPeriod, 2, 9)
        Me.LoanDetailsTabLayout.Controls.Add(Me.cmbDeductionSchedule, 1, 9)
        Me.LoanDetailsTabLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LoanDetailsTabLayout.Location = New System.Drawing.Point(3, 3)
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
        Me.LoanDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.LoanDetailsTabLayout.Size = New System.Drawing.Size(739, 238)
        Me.LoanDetailsTabLayout.TabIndex = 4
        '
        'dtpDateFrom
        '
        Me.dtpDateFrom.BackColor = System.Drawing.Color.White
        Me.dtpDateFrom.Location = New System.Drawing.Point(20, 211)
        Me.dtpDateFrom.Margin = New System.Windows.Forms.Padding(20, 3, 3, 0)
        Me.dtpDateFrom.Name = "dtpDateFrom"
        Me.dtpDateFrom.ReadOnly = True
        Me.dtpDateFrom.Size = New System.Drawing.Size(195, 22)
        Me.dtpDateFrom.TabIndex = 0
        '
        'pnlTxtLoanBalance
        '
        Me.pnlTxtLoanBalance.Controls.Add(Me.Label3)
        Me.pnlTxtLoanBalance.Controls.Add(Me.txtLoanBalance)
        Me.pnlTxtLoanBalance.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTxtLoanBalance.Location = New System.Drawing.Point(0, 160)
        Me.pnlTxtLoanBalance.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlTxtLoanBalance.Name = "pnlTxtLoanBalance"
        Me.pnlTxtLoanBalance.Size = New System.Drawing.Size(246, 32)
        Me.pnlTxtLoanBalance.TabIndex = 356
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 383
        Me.Label3.Text = "₱"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLoanBalance
        '
        Me.txtLoanBalance.BackColor = System.Drawing.Color.White
        Me.txtLoanBalance.Location = New System.Drawing.Point(20, 2)
        Me.txtLoanBalance.Name = "txtLoanBalance"
        Me.txtLoanBalance.ReadOnly = True
        Me.txtLoanBalance.Size = New System.Drawing.Size(195, 22)
        Me.txtLoanBalance.TabIndex = 356
        '
        'lblLoanType
        '
        Me.lblLoanType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanType.AutoSize = True
        Me.lblLoanType.Location = New System.Drawing.Point(20, 3)
        Me.lblLoanType.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanType.Name = "lblLoanType"
        Me.lblLoanType.Size = New System.Drawing.Size(70, 13)
        Me.lblLoanType.TabIndex = 379
        Me.lblLoanType.Text = "Type of loan"
        '
        'plnCboLoanType
        '
        Me.plnCboLoanType.Controls.Add(Me.cboLoanType)
        Me.plnCboLoanType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plnCboLoanType.Location = New System.Drawing.Point(0, 16)
        Me.plnCboLoanType.Margin = New System.Windows.Forms.Padding(0)
        Me.plnCboLoanType.Name = "plnCboLoanType"
        Me.plnCboLoanType.Size = New System.Drawing.Size(246, 32)
        Me.plnCboLoanType.TabIndex = 353
        '
        'cboLoanType
        '
        Me.cboLoanType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cboLoanType.FormattingEnabled = True
        Me.cboLoanType.Location = New System.Drawing.Point(20, 2)
        Me.cboLoanType.Name = "cboLoanType"
        Me.cboLoanType.Size = New System.Drawing.Size(195, 21)
        Me.cboLoanType.TabIndex = 353
        '
        'lblLoanNumber
        '
        Me.lblLoanNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanNumber.AutoSize = True
        Me.lblLoanNumber.Location = New System.Drawing.Point(20, 51)
        Me.lblLoanNumber.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanNumber.Name = "lblLoanNumber"
        Me.lblLoanNumber.Size = New System.Drawing.Size(76, 13)
        Me.lblLoanNumber.TabIndex = 357
        Me.lblLoanNumber.Text = "Loan Number"
        '
        'txtLoanNumber
        '
        Me.txtLoanNumber.BackColor = System.Drawing.Color.White
        Me.txtLoanNumber.Location = New System.Drawing.Point(20, 67)
        Me.txtLoanNumber.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.txtLoanNumber.Name = "txtLoanNumber"
        Me.txtLoanNumber.ReadOnly = True
        Me.txtLoanNumber.Size = New System.Drawing.Size(195, 22)
        Me.txtLoanNumber.TabIndex = 354
        '
        'lblDateFrom
        '
        Me.lblDateFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(20, 195)
        Me.lblDateFrom.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(60, 13)
        Me.lblDateFrom.TabIndex = 372
        Me.lblDateFrom.Text = "Date From"
        '
        'lblRemarks
        '
        Me.lblRemarks.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblRemarks.AutoSize = True
        Me.lblRemarks.Location = New System.Drawing.Point(504, 3)
        Me.lblRemarks.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblRemarks.Name = "lblRemarks"
        Me.lblRemarks.Size = New System.Drawing.Size(50, 13)
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
        Me.lblLoanBalance.Size = New System.Drawing.Size(96, 13)
        Me.lblLoanBalance.TabIndex = 365
        Me.lblLoanBalance.Text = "Total Balance Left"
        '
        'txtRemarks
        '
        Me.txtRemarks.BackColor = System.Drawing.Color.White
        Me.txtRemarks.Location = New System.Drawing.Point(504, 19)
        Me.txtRemarks.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.txtRemarks.MaxLength = 2000
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.ReadOnly = True
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
        Me.lblTotalLoanAmount.Size = New System.Drawing.Size(104, 13)
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
        Me.pnlTxtLoanAmount.Size = New System.Drawing.Size(246, 32)
        Me.pnlTxtLoanAmount.TabIndex = 355
        '
        'Label220
        '
        Me.Label220.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label220.AutoSize = True
        Me.Label220.Location = New System.Drawing.Point(3, 4)
        Me.Label220.Name = "Label220"
        Me.Label220.Size = New System.Drawing.Size(13, 13)
        Me.Label220.TabIndex = 383
        Me.Label220.Text = "₱"
        Me.Label220.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTotalLoanAmount
        '
        Me.txtTotalLoanAmount.BackColor = System.Drawing.Color.White
        Me.txtTotalLoanAmount.Location = New System.Drawing.Point(20, 2)
        Me.txtTotalLoanAmount.Name = "txtTotalLoanAmount"
        Me.txtTotalLoanAmount.ReadOnly = True
        Me.txtTotalLoanAmount.ShortcutsEnabled = False
        Me.txtTotalLoanAmount.Size = New System.Drawing.Size(195, 22)
        Me.txtTotalLoanAmount.TabIndex = 355
        '
        'lblNumberOfPayPeriodLeft
        '
        Me.lblNumberOfPayPeriodLeft.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNumberOfPayPeriodLeft.AutoSize = True
        Me.lblNumberOfPayPeriodLeft.Location = New System.Drawing.Point(266, 3)
        Me.lblNumberOfPayPeriodLeft.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblNumberOfPayPeriodLeft.Name = "lblNumberOfPayPeriodLeft"
        Me.lblNumberOfPayPeriodLeft.Size = New System.Drawing.Size(115, 13)
        Me.lblNumberOfPayPeriodLeft.TabIndex = 386
        Me.lblNumberOfPayPeriodLeft.Text = "No. of Pay Period left"
        '
        'txtNumberOfPayPeriodLeft
        '
        Me.txtNumberOfPayPeriodLeft.BackColor = System.Drawing.Color.White
        Me.txtNumberOfPayPeriodLeft.Location = New System.Drawing.Point(266, 19)
        Me.txtNumberOfPayPeriodLeft.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.txtNumberOfPayPeriodLeft.Name = "txtNumberOfPayPeriodLeft"
        Me.txtNumberOfPayPeriodLeft.ReadOnly = True
        Me.txtNumberOfPayPeriodLeft.Size = New System.Drawing.Size(195, 22)
        Me.txtNumberOfPayPeriodLeft.TabIndex = 359
        '
        'lblDeductionAmount
        '
        Me.lblDeductionAmount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDeductionAmount.AutoSize = True
        Me.lblDeductionAmount.Location = New System.Drawing.Point(266, 51)
        Me.lblDeductionAmount.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblDeductionAmount.Name = "lblDeductionAmount"
        Me.lblDeductionAmount.Size = New System.Drawing.Size(105, 13)
        Me.lblDeductionAmount.TabIndex = 368
        Me.lblDeductionAmount.Text = "Deduction Amount"
        '
        'pnlTxtDeductionAmount
        '
        Me.pnlTxtDeductionAmount.Controls.Add(Me.Label1)
        Me.pnlTxtDeductionAmount.Controls.Add(Me.txtDeductionAmount)
        Me.pnlTxtDeductionAmount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTxtDeductionAmount.Location = New System.Drawing.Point(246, 64)
        Me.pnlTxtDeductionAmount.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlTxtDeductionAmount.Name = "pnlTxtDeductionAmount"
        Me.pnlTxtDeductionAmount.Size = New System.Drawing.Size(238, 32)
        Me.pnlTxtDeductionAmount.TabIndex = 360
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 383
        Me.Label1.Text = "₱"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDeductionAmount
        '
        Me.txtDeductionAmount.BackColor = System.Drawing.Color.White
        Me.txtDeductionAmount.Location = New System.Drawing.Point(20, 2)
        Me.txtDeductionAmount.Name = "txtDeductionAmount"
        Me.txtDeductionAmount.ReadOnly = True
        Me.txtDeductionAmount.ShortcutsEnabled = False
        Me.txtDeductionAmount.Size = New System.Drawing.Size(195, 22)
        Me.txtDeductionAmount.TabIndex = 360
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtLoanStatus)
        Me.Panel1.Controls.Add(Me.cmbLoanStatus)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(249, 115)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(232, 26)
        Me.Panel1.TabIndex = 361
        '
        'txtLoanStatus
        '
        Me.txtLoanStatus.BackColor = System.Drawing.Color.White
        Me.txtLoanStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoanStatus.Location = New System.Drawing.Point(20, 2)
        Me.txtLoanStatus.Name = "txtLoanStatus"
        Me.txtLoanStatus.ReadOnly = True
        Me.txtLoanStatus.Size = New System.Drawing.Size(195, 22)
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
        'lblLoanStatus
        '
        Me.lblLoanStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanStatus.AutoSize = True
        Me.lblLoanStatus.Location = New System.Drawing.Point(266, 99)
        Me.lblLoanStatus.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanStatus.Name = "lblLoanStatus"
        Me.lblLoanStatus.Size = New System.Drawing.Size(39, 13)
        Me.lblLoanStatus.TabIndex = 371
        Me.lblLoanStatus.Text = "Status"
        '
        'lblLoanInterestPercentage
        '
        Me.lblLoanInterestPercentage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoanInterestPercentage.AutoSize = True
        Me.lblLoanInterestPercentage.Location = New System.Drawing.Point(266, 147)
        Me.lblLoanInterestPercentage.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblLoanInterestPercentage.Name = "lblLoanInterestPercentage"
        Me.lblLoanInterestPercentage.Size = New System.Drawing.Size(135, 13)
        Me.lblLoanInterestPercentage.TabIndex = 391
        Me.lblLoanInterestPercentage.Text = "Loan interest percentage"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.txtLoanInterestPercentage)
        Me.Panel3.Location = New System.Drawing.Point(246, 160)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(223, 32)
        Me.Panel3.TabIndex = 362
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(13, 13)
        Me.Label2.TabIndex = 383
        Me.Label2.Text = "₱"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLoanInterestPercentage
        '
        Me.txtLoanInterestPercentage.BackColor = System.Drawing.Color.White
        Me.txtLoanInterestPercentage.Location = New System.Drawing.Point(20, 2)
        Me.txtLoanInterestPercentage.Name = "txtLoanInterestPercentage"
        Me.txtLoanInterestPercentage.ShortcutsEnabled = False
        Me.txtLoanInterestPercentage.Size = New System.Drawing.Size(195, 22)
        Me.txtLoanInterestPercentage.TabIndex = 362
        '
        'lblNumberOfPayPeriod
        '
        Me.lblNumberOfPayPeriod.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNumberOfPayPeriod.AutoSize = True
        Me.lblNumberOfPayPeriod.Location = New System.Drawing.Point(504, 195)
        Me.lblNumberOfPayPeriod.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblNumberOfPayPeriod.Name = "lblNumberOfPayPeriod"
        Me.lblNumberOfPayPeriod.Size = New System.Drawing.Size(95, 13)
        Me.lblNumberOfPayPeriod.TabIndex = 369
        Me.lblNumberOfPayPeriod.Text = "No. of Pay Period"
        '
        'lblDeductionSchedule
        '
        Me.lblDeductionSchedule.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDeductionSchedule.AutoSize = True
        Me.lblDeductionSchedule.Location = New System.Drawing.Point(266, 195)
        Me.lblDeductionSchedule.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblDeductionSchedule.Name = "lblDeductionSchedule"
        Me.lblDeductionSchedule.Size = New System.Drawing.Size(111, 13)
        Me.lblDeductionSchedule.TabIndex = 375
        Me.lblDeductionSchedule.Text = "Deduction Schedule"
        '
        'txtNumberOfPayPeriod
        '
        Me.txtNumberOfPayPeriod.BackColor = System.Drawing.Color.White
        Me.txtNumberOfPayPeriod.Location = New System.Drawing.Point(504, 211)
        Me.txtNumberOfPayPeriod.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.txtNumberOfPayPeriod.Name = "txtNumberOfPayPeriod"
        Me.txtNumberOfPayPeriod.ReadOnly = True
        Me.txtNumberOfPayPeriod.ShortcutsEnabled = False
        Me.txtNumberOfPayPeriod.Size = New System.Drawing.Size(195, 22)
        Me.txtNumberOfPayPeriod.TabIndex = 358
        '
        'cmbDeductionSchedule
        '
        Me.cmbDeductionSchedule.BackColor = System.Drawing.Color.White
        Me.cmbDeductionSchedule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbDeductionSchedule.Enabled = False
        Me.cmbDeductionSchedule.FormattingEnabled = True
        Me.cmbDeductionSchedule.Location = New System.Drawing.Point(266, 211)
        Me.cmbDeductionSchedule.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.cmbDeductionSchedule.Name = "cmbDeductionSchedule"
        Me.cmbDeductionSchedule.Size = New System.Drawing.Size(195, 21)
        Me.cmbDeductionSchedule.TabIndex = 363
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.DetailsTabControl)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.dgvBonuses)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel4)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel2)
        Me.SplitContainer1.Size = New System.Drawing.Size(753, 512)
        Me.SplitContainer1.SplitterDistance = 270
        Me.SplitContainer1.TabIndex = 7
        '
        'dgvBonuses
        '
        Me.dgvBonuses.AllowUserToAddRows = False
        Me.dgvBonuses.AllowUserToDeleteRows = False
        Me.dgvBonuses.BackgroundColor = System.Drawing.Color.White
        Me.dgvBonuses.ColumnHeadersHeight = 34
        Me.dgvBonuses.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.BonusId, Me.BonusAmount, Me.Column1, Me.Frequency, Me.EffectiveDate, Me.EffectiveEndDate, Me.colIsFullAmount, Me.colAmountPayment, Me.Column2})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvBonuses.DefaultCellStyle = DataGridViewCellStyle7
        Me.dgvBonuses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvBonuses.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvBonuses.Location = New System.Drawing.Point(0, 42)
        Me.dgvBonuses.Name = "dgvBonuses"
        Me.dgvBonuses.Size = New System.Drawing.Size(753, 160)
        Me.dgvBonuses.TabIndex = 370
        '
        'BonusId
        '
        Me.BonusId.DataPropertyName = "BonusId"
        Me.BonusId.HeaderText = "BonusId"
        Me.BonusId.Name = "BonusId"
        Me.BonusId.Visible = False
        '
        'BonusAmount
        '
        Me.BonusAmount.DataPropertyName = "BonusAmount"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.Format = "N2"
        DataGridViewCellStyle1.NullValue = Nothing
        Me.BonusAmount.DefaultCellStyle = DataGridViewCellStyle1
        Me.BonusAmount.HeaderText = "Bonus Amount"
        Me.BonusAmount.Name = "BonusAmount"
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "InclusiveCurrentBonusAmount"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "Sufficient Bonus Amount"
        Me.Column1.Name = "Column1"
        '
        'Frequency
        '
        Me.Frequency.DataPropertyName = "Frequency"
        Me.Frequency.HeaderText = "Frequency"
        Me.Frequency.Name = "Frequency"
        '
        'EffectiveDate
        '
        Me.EffectiveDate.DataPropertyName = "EffectiveStartDate"
        DataGridViewCellStyle3.Format = "d"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.EffectiveDate.DefaultCellStyle = DataGridViewCellStyle3
        Me.EffectiveDate.HeaderText = "Effective Start Date"
        Me.EffectiveDate.Name = "EffectiveDate"
        '
        'EffectiveEndDate
        '
        Me.EffectiveEndDate.DataPropertyName = "EffectiveEndDate"
        DataGridViewCellStyle4.Format = "d"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.EffectiveEndDate.DefaultCellStyle = DataGridViewCellStyle4
        Me.EffectiveEndDate.HeaderText = "Effective End Date"
        Me.EffectiveEndDate.Name = "EffectiveEndDate"
        '
        'colIsFullAmount
        '
        Me.colIsFullAmount.DataPropertyName = "IsFullPayment"
        Me.colIsFullAmount.HeaderText = "Use Max Valid Payment"
        Me.colIsFullAmount.Name = "colIsFullAmount"
        Me.colIsFullAmount.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colIsFullAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'colAmountPayment
        '
        '
        '
        '
        Me.colAmountPayment.BackgroundStyle.Class = "DataGridViewNumericBorder"
        Me.colAmountPayment.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.colAmountPayment.DataPropertyName = "AmountPayment"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "N2"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.colAmountPayment.DefaultCellStyle = DataGridViewCellStyle5
        Me.colAmountPayment.HeaderText = "Payment Amount"
        Me.colAmountPayment.Increment = 1.0R
        Me.colAmountPayment.MinValue = 0R
        Me.colAmountPayment.Name = "colAmountPayment"
        Me.colAmountPayment.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colAmountPayment.ShowUpDown = True
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "ValidPayment"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N2"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column2.HeaderText = "Valid Payment"
        Me.Column2.Name = "Column2"
        Me.Column2.Visible = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.btnCancel)
        Me.Panel4.Controls.Add(Me.btnSave)
        Me.Panel4.Controls.Add(Me.Label4)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 202)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(753, 36)
        Me.Panel4.TabIndex = 369
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(666, 7)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(585, 7)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(568, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 8)
        Me.Label4.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(753, 42)
        Me.Panel2.TabIndex = 368
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3343!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.33216!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.33216!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.00137!))
        Me.TableLayoutPanel1.Controls.Add(Me.lblLoanPayPeriodLeft, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label6, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label9, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblTotalBalanceLeft, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label8, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblTotalAmountPayment, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label7, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(753, 37)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Label9
        '
        Me.Label9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label9.Location = New System.Drawing.Point(444, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(184, 18)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "New Total Balance left"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'Label8
        '
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label8.Location = New System.Drawing.Point(254, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(184, 18)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Total Payment"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'Label6
        '
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label6.Location = New System.Drawing.Point(634, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(116, 18)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "No. of Pay Period left"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblLoanPayPeriodLeft
        '
        Me.lblLoanPayPeriodLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblLoanPayPeriodLeft.Location = New System.Drawing.Point(634, 18)
        Me.lblLoanPayPeriodLeft.Name = "lblLoanPayPeriodLeft"
        Me.lblLoanPayPeriodLeft.Size = New System.Drawing.Size(116, 19)
        Me.lblLoanPayPeriodLeft.TabIndex = 8
        Me.lblLoanPayPeriodLeft.Text = "Label5"
        Me.lblLoanPayPeriodLeft.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblTotalBalanceLeft
        '
        Me.lblTotalBalanceLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalBalanceLeft.Location = New System.Drawing.Point(444, 18)
        Me.lblTotalBalanceLeft.Name = "lblTotalBalanceLeft"
        Me.lblTotalBalanceLeft.Size = New System.Drawing.Size(184, 19)
        Me.lblTotalBalanceLeft.TabIndex = 9
        Me.lblTotalBalanceLeft.Text = "Label11"
        Me.lblTotalBalanceLeft.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblTotalAmountPayment
        '
        Me.lblTotalAmountPayment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalAmountPayment.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalAmountPayment.Location = New System.Drawing.Point(254, 18)
        Me.lblTotalAmountPayment.Name = "lblTotalAmountPayment"
        Me.lblTotalAmountPayment.Size = New System.Drawing.Size(184, 19)
        Me.lblTotalAmountPayment.TabIndex = 10
        Me.lblTotalAmountPayment.Text = "Label12"
        Me.lblTotalAmountPayment.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'ToolTip1
        '
        Me.ToolTip1.IsBalloon = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "BonusId"
        Me.DataGridViewTextBoxColumn1.HeaderText = "BonusId"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "BonusAmount"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Format = "N2"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn2.HeaderText = "Bonus Amount"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 133
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "ExclusiveCurrentBonusAmount"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "N2"
        DataGridViewCellStyle9.NullValue = Nothing
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn3.HeaderText = "Sufficient Bonus Amount"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 133
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "EffectiveDate"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Effective Date"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 132
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "AmountPayment"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Column2"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        '
        'Label5
        '
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label5.Location = New System.Drawing.Point(3, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(245, 18)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Deduction Amount + Payment from Bonus"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'Label7
        '
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label7.Location = New System.Drawing.Point(3, 18)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(245, 19)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Label7"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'AssignBonusToLoanForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(753, 512)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AssignBonusToLoanForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.DetailsTabControl.ResumeLayout(False)
        Me.tbpDetails.ResumeLayout(False)
        Me.LoanDetailsTabLayout.ResumeLayout(False)
        Me.LoanDetailsTabLayout.PerformLayout()
        Me.pnlTxtLoanBalance.ResumeLayout(False)
        Me.pnlTxtLoanBalance.PerformLayout()
        Me.plnCboLoanType.ResumeLayout(False)
        Me.pnlTxtLoanAmount.ResumeLayout(False)
        Me.pnlTxtLoanAmount.PerformLayout()
        Me.pnlTxtDeductionAmount.ResumeLayout(False)
        Me.pnlTxtDeductionAmount.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dgvBonuses, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DetailsTabControl As TabControl
    Friend WithEvents tbpDetails As TabPage
    Friend WithEvents LoanDetailsTabLayout As TableLayoutPanel
    Friend WithEvents pnlTxtLoanBalance As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents txtLoanBalance As TextBox
    Friend WithEvents lblLoanType As Label
    Friend WithEvents plnCboLoanType As Panel
    Friend WithEvents cboLoanType As ComboBox
    Friend WithEvents lblLoanNumber As Label
    Friend WithEvents txtLoanNumber As TextBox
    Friend WithEvents lblDateFrom As Label
    Friend WithEvents lblRemarks As Label
    Friend WithEvents lblLoanBalance As Label
    Friend WithEvents txtRemarks As TextBox
    Friend WithEvents lblTotalLoanAmount As Label
    Friend WithEvents pnlTxtLoanAmount As Panel
    Friend WithEvents Label220 As Label
    Friend WithEvents txtTotalLoanAmount As TextBox
    Friend WithEvents lblNumberOfPayPeriodLeft As Label
    Friend WithEvents txtNumberOfPayPeriodLeft As TextBox
    Friend WithEvents lblDeductionAmount As Label
    Friend WithEvents pnlTxtDeductionAmount As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents txtDeductionAmount As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents txtLoanStatus As TextBox
    Friend WithEvents cmbLoanStatus As ComboBox
    Friend WithEvents lblLoanStatus As Label
    Friend WithEvents lblLoanInterestPercentage As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents txtLoanInterestPercentage As TextBox
    Friend WithEvents lblNumberOfPayPeriod As Label
    Friend WithEvents lblDeductionSchedule As Label
    Friend WithEvents txtNumberOfPayPeriod As TextBox
    Friend WithEvents cmbDeductionSchedule As ComboBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents dtpDateFrom As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label6 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Label4 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents dgvBonuses As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents lblLoanPayPeriodLeft As Label
    Friend WithEvents lblTotalBalanceLeft As Label
    Friend WithEvents lblTotalAmountPayment As Label
    Friend WithEvents BonusId As DataGridViewTextBoxColumn
    Friend WithEvents BonusAmount As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Frequency As DataGridViewTextBoxColumn
    Friend WithEvents EffectiveDate As DataGridViewTextBoxColumn
    Friend WithEvents EffectiveEndDate As DataGridViewTextBoxColumn
    Friend WithEvents colIsFullAmount As DataGridViewCheckBoxColumn
    Friend WithEvents colAmountPayment As DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Label5 As Label
    Friend WithEvents Label7 As Label
End Class
