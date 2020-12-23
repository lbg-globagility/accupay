<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EmployeeLoansForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EmployeeLoansForm))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.SearchTextBox = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.EmployeesDataGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.cemp_EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cemp_LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cemp_FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.lnkBonusPayment = New System.Windows.Forms.LinkLabel()
        Me.chkCompleteFilter = New System.Windows.Forms.CheckBox()
        Me.chkCancelledFilter = New System.Windows.Forms.CheckBox()
        Me.chkOnHoldFilter = New System.Windows.Forms.CheckBox()
        Me.chkInProgressFilter = New System.Windows.Forms.CheckBox()
        Me.LoanGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.pnlForm = New System.Windows.Forms.Panel()
        Me.DetailsTabControl = New System.Windows.Forms.TabControl()
        Me.DetailsTabLayout = New System.Windows.Forms.TabPage()
        Me.LoanUserControl1 = New LoanUserControl()
        Me.tbpHistory = New System.Windows.Forms.TabPage()
        Me.LoanHistoryGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_dateded = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn116 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeInfoTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.EmployeeNameTextBox = New System.Windows.Forms.TextBox()
        Me.EmployeeNumberTextBox = New System.Windows.Forms.TextBox()
        Me.EmployeePictureBox = New System.Windows.Forms.PictureBox()
        Me.ToolStrip12 = New System.Windows.Forms.ToolStrip()
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.DeleteToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CancelToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ImportToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.UserActivityToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.LoanListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ShowAllCheckBox = New System.Windows.Forms.CheckBox()
        Me.c_loanno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_loantype = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_totloanamt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_totballeft = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_dedamt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_DedPercent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_dedsched = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_dedeffectivedatefrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Comments = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlSearch.SuspendLayout()
        CType(Me.EmployeesDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMain.SuspendLayout()
        Me.Panel10.SuspendLayout()
        CType(Me.LoanGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlForm.SuspendLayout()
        Me.DetailsTabControl.SuspendLayout()
        Me.DetailsTabLayout.SuspendLayout()
        Me.tbpHistory.SuspendLayout()
        CType(Me.LoanHistoryGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EmployeeInfoTabLayout.SuspendLayout()
        CType(Me.EmployeePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip12.SuspendLayout()
        CType(Me.LoanListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblFormTitle
        '
        Me.lblFormTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(174, Byte), Integer), CType(CType(194, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblFormTitle.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblFormTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(1229, 24)
        Me.lblFormTitle.TabIndex = 0
        Me.lblFormTitle.Text = "Employee Loans"
        Me.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlSearch
        '
        Me.pnlSearch.BackColor = System.Drawing.Color.White
        Me.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSearch.Controls.Add(Me.SearchTextBox)
        Me.pnlSearch.Controls.Add(Me.lblSearch)
        Me.pnlSearch.Location = New System.Drawing.Point(8, 32)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(352, 56)
        Me.pnlSearch.TabIndex = 141
        '
        'SearchTextBox
        '
        Me.SearchTextBox.Location = New System.Drawing.Point(80, 16)
        Me.SearchTextBox.MaxLength = 50
        Me.SearchTextBox.Name = "SearchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(259, 22)
        Me.SearchTextBox.TabIndex = 64
        '
        'lblSearch
        '
        Me.lblSearch.Location = New System.Drawing.Point(8, 16)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(64, 25)
        Me.lblSearch.TabIndex = 62
        Me.lblSearch.Text = "Search"
        Me.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'EmployeesDataGridView
        '
        Me.EmployeesDataGridView.AllowUserToAddRows = False
        Me.EmployeesDataGridView.AllowUserToDeleteRows = False
        Me.EmployeesDataGridView.AllowUserToOrderColumns = True
        Me.EmployeesDataGridView.AllowUserToResizeRows = False
        Me.EmployeesDataGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EmployeesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.EmployeesDataGridView.BackgroundColor = System.Drawing.Color.White
        Me.EmployeesDataGridView.ColumnHeadersHeight = 34
        Me.EmployeesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.EmployeesDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cemp_EmployeeID, Me.cemp_LastName, Me.cemp_FirstName})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.EmployeesDataGridView.DefaultCellStyle = DataGridViewCellStyle1
        Me.EmployeesDataGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.EmployeesDataGridView.Location = New System.Drawing.Point(8, 120)
        Me.EmployeesDataGridView.MultiSelect = False
        Me.EmployeesDataGridView.Name = "EmployeesDataGridView"
        Me.EmployeesDataGridView.ReadOnly = True
        Me.EmployeesDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.EmployeesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.EmployeesDataGridView.Size = New System.Drawing.Size(352, 415)
        Me.EmployeesDataGridView.TabIndex = 140
        '
        'cemp_EmployeeID
        '
        Me.cemp_EmployeeID.DataPropertyName = "EmployeeNo"
        Me.cemp_EmployeeID.HeaderText = "Employee ID"
        Me.cemp_EmployeeID.Name = "cemp_EmployeeID"
        Me.cemp_EmployeeID.ReadOnly = True
        '
        'cemp_LastName
        '
        Me.cemp_LastName.DataPropertyName = "LastName"
        Me.cemp_LastName.HeaderText = "Last Name"
        Me.cemp_LastName.Name = "cemp_LastName"
        Me.cemp_LastName.ReadOnly = True
        '
        'cemp_FirstName
        '
        Me.cemp_FirstName.DataPropertyName = "FirstName"
        Me.cemp_FirstName.HeaderText = "First Name"
        Me.cemp_FirstName.Name = "cemp_FirstName"
        Me.cemp_FirstName.ReadOnly = True
        '
        'pnlMain
        '
        Me.pnlMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMain.AutoScroll = True
        Me.pnlMain.BackColor = System.Drawing.Color.White
        Me.pnlMain.Controls.Add(Me.Panel10)
        Me.pnlMain.Controls.Add(Me.ToolStrip12)
        Me.pnlMain.Location = New System.Drawing.Point(375, 32)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(842, 503)
        Me.pnlMain.TabIndex = 142
        '
        'Panel10
        '
        Me.Panel10.AutoScroll = True
        Me.Panel10.Controls.Add(Me.lnkBonusPayment)
        Me.Panel10.Controls.Add(Me.chkCompleteFilter)
        Me.Panel10.Controls.Add(Me.chkCancelledFilter)
        Me.Panel10.Controls.Add(Me.chkOnHoldFilter)
        Me.Panel10.Controls.Add(Me.chkInProgressFilter)
        Me.Panel10.Controls.Add(Me.LoanGridView)
        Me.Panel10.Controls.Add(Me.pnlForm)
        Me.Panel10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel10.Location = New System.Drawing.Point(0, 25)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(842, 478)
        Me.Panel10.TabIndex = 386
        '
        'lnkBonusPayment
        '
        Me.lnkBonusPayment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lnkBonusPayment.AutoSize = True
        Me.lnkBonusPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.lnkBonusPayment.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkBonusPayment.Location = New System.Drawing.Point(714, 407)
        Me.lnkBonusPayment.Name = "lnkBonusPayment"
        Me.lnkBonusPayment.Size = New System.Drawing.Size(97, 15)
        Me.lnkBonusPayment.TabIndex = 514
        Me.lnkBonusPayment.TabStop = True
        Me.lnkBonusPayment.Text = "pay using Bonus"
        Me.lnkBonusPayment.Visible = False
        '
        'chkCompleteFilter
        '
        Me.chkCompleteFilter.AutoSize = True
        Me.chkCompleteFilter.Location = New System.Drawing.Point(364, 405)
        Me.chkCompleteFilter.Name = "chkCompleteFilter"
        Me.chkCompleteFilter.Size = New System.Drawing.Size(102, 17)
        Me.chkCompleteFilter.TabIndex = 513
        Me.chkCompleteFilter.Text = "Complete (100)"
        Me.chkCompleteFilter.UseVisualStyleBackColor = True
        '
        'chkCancelledFilter
        '
        Me.chkCancelledFilter.AutoSize = True
        Me.chkCancelledFilter.Location = New System.Drawing.Point(253, 405)
        Me.chkCancelledFilter.Name = "chkCancelledFilter"
        Me.chkCancelledFilter.Size = New System.Drawing.Size(103, 17)
        Me.chkCancelledFilter.TabIndex = 512
        Me.chkCancelledFilter.Text = "Cancelled (100)"
        Me.chkCancelledFilter.UseVisualStyleBackColor = True
        '
        'chkOnHoldFilter
        '
        Me.chkOnHoldFilter.AutoSize = True
        Me.chkOnHoldFilter.Checked = True
        Me.chkOnHoldFilter.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOnHoldFilter.Location = New System.Drawing.Point(146, 405)
        Me.chkOnHoldFilter.Name = "chkOnHoldFilter"
        Me.chkOnHoldFilter.Size = New System.Drawing.Size(97, 17)
        Me.chkOnHoldFilter.TabIndex = 511
        Me.chkOnHoldFilter.Text = "On Hold (100)"
        Me.chkOnHoldFilter.UseVisualStyleBackColor = True
        '
        'chkInProgressFilter
        '
        Me.chkInProgressFilter.AutoSize = True
        Me.chkInProgressFilter.Checked = True
        Me.chkInProgressFilter.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkInProgressFilter.Location = New System.Drawing.Point(27, 405)
        Me.chkInProgressFilter.Name = "chkInProgressFilter"
        Me.chkInProgressFilter.Size = New System.Drawing.Size(110, 17)
        Me.chkInProgressFilter.TabIndex = 510
        Me.chkInProgressFilter.Text = "In Progress (100)"
        Me.chkInProgressFilter.UseVisualStyleBackColor = True
        '
        'LoanGridView
        '
        Me.LoanGridView.AllowUserToAddRows = False
        Me.LoanGridView.AllowUserToDeleteRows = False
        Me.LoanGridView.AllowUserToResizeRows = False
        Me.LoanGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LoanGridView.BackgroundColor = System.Drawing.Color.White
        Me.LoanGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.LoanGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_loanno, Me.c_loantype, Me.c_totloanamt, Me.c_totballeft, Me.c_dedamt, Me.c_DedPercent, Me.c_dedsched, Me.c_dedeffectivedatefrom, Me.Comments, Me.c_status})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.LoanGridView.DefaultCellStyle = DataGridViewCellStyle7
        Me.LoanGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.LoanGridView.Location = New System.Drawing.Point(28, 428)
        Me.LoanGridView.MultiSelect = False
        Me.LoanGridView.Name = "LoanGridView"
        Me.LoanGridView.ReadOnly = True
        Me.LoanGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.LoanGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.LoanGridView.Size = New System.Drawing.Size(781, 250)
        Me.LoanGridView.TabIndex = 366
        '
        'pnlForm
        '
        Me.pnlForm.Controls.Add(Me.DetailsTabControl)
        Me.pnlForm.Controls.Add(Me.EmployeeInfoTabLayout)
        Me.pnlForm.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlForm.Location = New System.Drawing.Point(0, 0)
        Me.pnlForm.Name = "pnlForm"
        Me.pnlForm.Size = New System.Drawing.Size(825, 379)
        Me.pnlForm.TabIndex = 509
        '
        'DetailsTabControl
        '
        Me.DetailsTabControl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DetailsTabControl.Controls.Add(Me.DetailsTabLayout)
        Me.DetailsTabControl.Controls.Add(Me.tbpHistory)
        Me.DetailsTabControl.Location = New System.Drawing.Point(0, 101)
        Me.DetailsTabControl.Name = "DetailsTabControl"
        Me.DetailsTabControl.SelectedIndex = 0
        Me.DetailsTabControl.Size = New System.Drawing.Size(825, 274)
        Me.DetailsTabControl.TabIndex = 5
        '
        'DetailsTabLayout
        '
        Me.DetailsTabLayout.Controls.Add(Me.LoanUserControl1)
        Me.DetailsTabLayout.Location = New System.Drawing.Point(4, 22)
        Me.DetailsTabLayout.Name = "DetailsTabLayout"
        Me.DetailsTabLayout.Padding = New System.Windows.Forms.Padding(3)
        Me.DetailsTabLayout.Size = New System.Drawing.Size(817, 248)
        Me.DetailsTabLayout.TabIndex = 0
        Me.DetailsTabLayout.Text = "Loan Details"
        Me.DetailsTabLayout.UseVisualStyleBackColor = True
        '
        'LoanUserControl1
        '
        Me.LoanUserControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LoanUserControl1.Location = New System.Drawing.Point(3, 3)
        Me.LoanUserControl1.Name = "LoanUserControl1"
        Me.LoanUserControl1.Size = New System.Drawing.Size(811, 242)
        Me.LoanUserControl1.TabIndex = 0
        '
        'tbpHistory
        '
        Me.tbpHistory.Controls.Add(Me.LoanHistoryGridView)
        Me.tbpHistory.Location = New System.Drawing.Point(4, 22)
        Me.tbpHistory.Name = "tbpHistory"
        Me.tbpHistory.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpHistory.Size = New System.Drawing.Size(834, 248)
        Me.tbpHistory.TabIndex = 1
        Me.tbpHistory.Text = "Loan History"
        Me.tbpHistory.UseVisualStyleBackColor = True
        '
        'LoanHistoryGridView
        '
        Me.LoanHistoryGridView.AllowUserToAddRows = False
        Me.LoanHistoryGridView.AllowUserToDeleteRows = False
        Me.LoanHistoryGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
        Me.LoanHistoryGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.LoanHistoryGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_dateded, Me.c_Amount, Me.DataGridViewTextBoxColumn116})
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.LoanHistoryGridView.DefaultCellStyle = DataGridViewCellStyle10
        Me.LoanHistoryGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.LoanHistoryGridView.Location = New System.Drawing.Point(24, 0)
        Me.LoanHistoryGridView.MultiSelect = False
        Me.LoanHistoryGridView.Name = "LoanHistoryGridView"
        Me.LoanHistoryGridView.ReadOnly = True
        Me.LoanHistoryGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.LoanHistoryGridView.Size = New System.Drawing.Size(764, 242)
        Me.LoanHistoryGridView.TabIndex = 325
        '
        'c_dateded
        '
        Me.c_dateded.DataPropertyName = "PayPeriodPayToDate"
        Me.c_dateded.HeaderText = "Deduction Date"
        Me.c_dateded.Name = "c_dateded"
        Me.c_dateded.ReadOnly = True
        Me.c_dateded.Width = 180
        '
        'c_Amount
        '
        Me.c_Amount.DataPropertyName = "Amount"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Format = "(#,###.00)"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.c_Amount.DefaultCellStyle = DataGridViewCellStyle8
        Me.c_Amount.HeaderText = "Amount"
        Me.c_Amount.Name = "c_Amount"
        Me.c_Amount.ReadOnly = True
        Me.c_Amount.Width = 180
        '
        'DataGridViewTextBoxColumn116
        '
        Me.DataGridViewTextBoxColumn116.DataPropertyName = "TotalBalance"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "N2"
        DataGridViewCellStyle9.NullValue = Nothing
        Me.DataGridViewTextBoxColumn116.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn116.HeaderText = "Balance"
        Me.DataGridViewTextBoxColumn116.Name = "DataGridViewTextBoxColumn116"
        Me.DataGridViewTextBoxColumn116.ReadOnly = True
        Me.DataGridViewTextBoxColumn116.Width = 180
        '
        'EmployeeInfoTabLayout
        '
        Me.EmployeeInfoTabLayout.BackColor = System.Drawing.Color.White
        Me.EmployeeInfoTabLayout.ColumnCount = 2
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121.0!))
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 861.0!))
        Me.EmployeeInfoTabLayout.Controls.Add(Me.EmployeeNameTextBox, 1, 0)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.EmployeeNumberTextBox, 1, 1)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.EmployeePictureBox, 0, 0)
        Me.EmployeeInfoTabLayout.Location = New System.Drawing.Point(0, 9)
        Me.EmployeeInfoTabLayout.Name = "EmployeeInfoTabLayout"
        Me.EmployeeInfoTabLayout.RowCount = 2
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.Size = New System.Drawing.Size(827, 88)
        Me.EmployeeInfoTabLayout.TabIndex = 3
        '
        'EmployeeNameTextBox
        '
        Me.EmployeeNameTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EmployeeNameTextBox.BackColor = System.Drawing.Color.White
        Me.EmployeeNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.EmployeeNameTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.EmployeeNameTextBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.EmployeeNameTextBox.Location = New System.Drawing.Point(124, 13)
        Me.EmployeeNameTextBox.MaxLength = 250
        Me.EmployeeNameTextBox.Name = "EmployeeNameTextBox"
        Me.EmployeeNameTextBox.ReadOnly = True
        Me.EmployeeNameTextBox.Size = New System.Drawing.Size(668, 28)
        Me.EmployeeNameTextBox.TabIndex = 381
        '
        'EmployeeNumberTextBox
        '
        Me.EmployeeNumberTextBox.BackColor = System.Drawing.Color.White
        Me.EmployeeNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.EmployeeNumberTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EmployeeNumberTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.EmployeeNumberTextBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.EmployeeNumberTextBox.Location = New System.Drawing.Point(124, 47)
        Me.EmployeeNumberTextBox.MaxLength = 50
        Me.EmployeeNumberTextBox.Multiline = True
        Me.EmployeeNumberTextBox.Name = "EmployeeNumberTextBox"
        Me.EmployeeNumberTextBox.ReadOnly = True
        Me.EmployeeNumberTextBox.Size = New System.Drawing.Size(855, 38)
        Me.EmployeeNumberTextBox.TabIndex = 380
        '
        'EmployeePictureBox
        '
        Me.EmployeePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.EmployeePictureBox.Location = New System.Drawing.Point(28, 3)
        Me.EmployeePictureBox.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.EmployeePictureBox.Name = "EmployeePictureBox"
        Me.EmployeeInfoTabLayout.SetRowSpan(Me.EmployeePictureBox, 2)
        Me.EmployeePictureBox.Size = New System.Drawing.Size(88, 82)
        Me.EmployeePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EmployeePictureBox.TabIndex = 382
        Me.EmployeePictureBox.TabStop = False
        '
        'ToolStrip12
        '
        Me.ToolStrip12.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip12.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip12.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.SaveToolStripButton, Me.DeleteToolStripButton, Me.CancelToolStripButton, Me.ImportToolStripButton, Me.ToolStripSeparator9, Me.btnClose, Me.UserActivityToolStripButton})
        Me.ToolStrip12.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip12.Name = "ToolStrip12"
        Me.ToolStrip12.Size = New System.Drawing.Size(842, 25)
        Me.ToolStrip12.TabIndex = 328
        Me.ToolStrip12.Text = "ToolStrip12"
        '
        'NewToolStripButton
        '
        Me.NewToolStripButton.Image = Global.AccuPay.My.Resources.Resources._new
        Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripButton.Name = "NewToolStripButton"
        Me.NewToolStripButton.Size = New System.Drawing.Size(51, 22)
        Me.NewToolStripButton.Text = "&New"
        '
        'SaveToolStripButton
        '
        Me.SaveToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripButton.Name = "SaveToolStripButton"
        Me.SaveToolStripButton.Size = New System.Drawing.Size(51, 22)
        Me.SaveToolStripButton.Text = "&Save"
        '
        'DeleteToolStripButton
        '
        Me.DeleteToolStripButton.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripButton.Name = "DeleteToolStripButton"
        Me.DeleteToolStripButton.Size = New System.Drawing.Size(60, 22)
        Me.DeleteToolStripButton.Text = "&Delete"
        '
        'CancelToolStripButton
        '
        Me.CancelToolStripButton.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.CancelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CancelToolStripButton.Name = "CancelToolStripButton"
        Me.CancelToolStripButton.Size = New System.Drawing.Size(63, 22)
        Me.CancelToolStripButton.Text = "&Cancel"
        '
        'ImportToolStripButton
        '
        Me.ImportToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Add
        Me.ImportToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ImportToolStripButton.Name = "ImportToolStripButton"
        Me.ImportToolStripButton.Size = New System.Drawing.Size(63, 22)
        Me.ImportToolStripButton.Text = "&Import"
        Me.ImportToolStripButton.ToolTipText = "Import loans"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "Close"
        '
        'UserActivityToolStripButton
        '
        Me.UserActivityToolStripButton.Image = CType(resources.GetObject("UserActivityToolStripButton.Image"), System.Drawing.Image)
        Me.UserActivityToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.UserActivityToolStripButton.Name = "UserActivityToolStripButton"
        Me.UserActivityToolStripButton.Size = New System.Drawing.Size(93, 22)
        Me.UserActivityToolStripButton.Text = "&User Activity"
        '
        'LoanListBindingSource
        '
        '
        'ShowAllCheckBox
        '
        Me.ShowAllCheckBox.AutoSize = True
        Me.ShowAllCheckBox.Location = New System.Drawing.Point(8, 97)
        Me.ShowAllCheckBox.Name = "ShowAllCheckBox"
        Me.ShowAllCheckBox.Size = New System.Drawing.Size(128, 17)
        Me.ShowAllCheckBox.TabIndex = 148
        Me.ShowAllCheckBox.Text = "Show All Employees"
        Me.ShowAllCheckBox.UseVisualStyleBackColor = True
        '
        'c_loanno
        '
        Me.c_loanno.DataPropertyName = "LoanNumber"
        Me.c_loanno.HeaderText = "Loan Number"
        Me.c_loanno.Name = "c_loanno"
        Me.c_loanno.ReadOnly = True
        Me.c_loanno.Width = 40
        '
        'c_loantype
        '
        Me.c_loantype.DataPropertyName = "LoanTypeName"
        Me.c_loantype.HeaderText = "Loan type"
        Me.c_loantype.Name = "c_loantype"
        Me.c_loantype.ReadOnly = True
        '
        'c_totloanamt
        '
        Me.c_totloanamt.DataPropertyName = "TotalLoanAmount"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.c_totloanamt.DefaultCellStyle = DataGridViewCellStyle2
        Me.c_totloanamt.HeaderText = "Total Loan Amount"
        Me.c_totloanamt.Name = "c_totloanamt"
        Me.c_totloanamt.ReadOnly = True
        '
        'c_totballeft
        '
        Me.c_totballeft.DataPropertyName = "TotalBalanceLeft"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "N2"
        Me.c_totballeft.DefaultCellStyle = DataGridViewCellStyle3
        Me.c_totballeft.HeaderText = "Total Balance Left"
        Me.c_totballeft.Name = "c_totballeft"
        Me.c_totballeft.ReadOnly = True
        '
        'c_dedamt
        '
        Me.c_dedamt.DataPropertyName = "DeductionAmount"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N2"
        Me.c_dedamt.DefaultCellStyle = DataGridViewCellStyle4
        Me.c_dedamt.HeaderText = "Deduction Amount"
        Me.c_dedamt.Name = "c_dedamt"
        Me.c_dedamt.ReadOnly = True
        '
        'c_DedPercent
        '
        Me.c_DedPercent.DataPropertyName = "InterestPercentage"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "N2"
        Me.c_DedPercent.DefaultCellStyle = DataGridViewCellStyle5
        Me.c_DedPercent.HeaderText = "Deduction Percentage"
        Me.c_DedPercent.Name = "c_DedPercent"
        Me.c_DedPercent.ReadOnly = True
        '
        'c_dedsched
        '
        Me.c_dedsched.DataPropertyName = "DeductionSchedule"
        Me.c_dedsched.HeaderText = "Deduction Schedule"
        Me.c_dedsched.Name = "c_dedsched"
        Me.c_dedsched.ReadOnly = True
        '
        'c_dedeffectivedatefrom
        '
        Me.c_dedeffectivedatefrom.DataPropertyName = "EffectiveFrom"
        DataGridViewCellStyle6.Format = "d"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.c_dedeffectivedatefrom.DefaultCellStyle = DataGridViewCellStyle6
        Me.c_dedeffectivedatefrom.HeaderText = "Deduction date from"
        Me.c_dedeffectivedatefrom.Name = "c_dedeffectivedatefrom"
        Me.c_dedeffectivedatefrom.ReadOnly = True
        '
        'Comments
        '
        Me.Comments.DataPropertyName = "Comments"
        Me.Comments.HeaderText = "Remarks"
        Me.Comments.Name = "Comments"
        Me.Comments.ReadOnly = True
        '
        'c_status
        '
        Me.c_status.DataPropertyName = "Status"
        Me.c_status.HeaderText = "Status"
        Me.c_status.Name = "c_status"
        Me.c_status.ReadOnly = True
        '
        'EmployeeLoansForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1229, 547)
        Me.Controls.Add(Me.ShowAllCheckBox)
        Me.Controls.Add(Me.pnlSearch)
        Me.Controls.Add(Me.lblFormTitle)
        Me.Controls.Add(Me.EmployeesDataGridView)
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "EmployeeLoansForm"
        Me.Text = "EmployeeLoansForm"
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.EmployeesDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        Me.Panel10.ResumeLayout(False)
        Me.Panel10.PerformLayout()
        CType(Me.LoanGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlForm.ResumeLayout(False)
        Me.DetailsTabControl.ResumeLayout(False)
        Me.DetailsTabLayout.ResumeLayout(False)
        Me.tbpHistory.ResumeLayout(False)
        CType(Me.LoanHistoryGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EmployeeInfoTabLayout.ResumeLayout(False)
        Me.EmployeeInfoTabLayout.PerformLayout()
        CType(Me.EmployeePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip12.ResumeLayout(False)
        Me.ToolStrip12.PerformLayout()
        CType(Me.LoanListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblFormTitle As Label
    Friend WithEvents pnlSearch As Panel
    Friend WithEvents SearchTextBox As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents EmployeesDataGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents pnlMain As Panel
    Friend WithEvents ToolStrip12 As ToolStrip
    Friend WithEvents NewToolStripButton As ToolStripButton
    Friend WithEvents SaveToolStripButton As ToolStripButton
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents DeleteToolStripButton As ToolStripButton
    Friend WithEvents CancelToolStripButton As ToolStripButton
    Friend WithEvents btnClose As ToolStripButton
    Friend WithEvents ImportToolStripButton As ToolStripButton
    Friend WithEvents Panel10 As Panel
    Friend WithEvents EmployeePictureBox As PictureBox
    Friend WithEvents LoanGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents EmployeeNameTextBox As TextBox
    Friend WithEvents EmployeeNumberTextBox As TextBox
    Friend WithEvents pnlForm As Panel
    Friend WithEvents EmployeeInfoTabLayout As TableLayoutPanel
    Friend WithEvents DetailsTabControl As TabControl
    Friend WithEvents DetailsTabLayout As TabPage
    Friend WithEvents tbpHistory As TabPage
    Friend WithEvents LoanHistoryGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents LoanListBindingSource As BindingSource
    Friend WithEvents chkCompleteFilter As CheckBox
    Friend WithEvents chkCancelledFilter As CheckBox
    Friend WithEvents chkOnHoldFilter As CheckBox
    Friend WithEvents chkInProgressFilter As CheckBox
    Friend WithEvents cemp_EmployeeID As DataGridViewTextBoxColumn
    Friend WithEvents cemp_LastName As DataGridViewTextBoxColumn
    Friend WithEvents cemp_FirstName As DataGridViewTextBoxColumn
    Friend WithEvents c_dateded As DataGridViewTextBoxColumn
    Friend WithEvents c_Amount As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn116 As DataGridViewTextBoxColumn
    Friend WithEvents ShowAllCheckBox As CheckBox
    Friend WithEvents UserActivityToolStripButton As ToolStripButton
    Friend WithEvents lnkBonusPayment As LinkLabel
    Friend WithEvents LoanUserControl1 As LoanUserControl
    Friend WithEvents c_loanno As DataGridViewTextBoxColumn
    Friend WithEvents c_loantype As DataGridViewTextBoxColumn
    Friend WithEvents c_totloanamt As DataGridViewTextBoxColumn
    Friend WithEvents c_totballeft As DataGridViewTextBoxColumn
    Friend WithEvents c_dedamt As DataGridViewTextBoxColumn
    Friend WithEvents c_DedPercent As DataGridViewTextBoxColumn
    Friend WithEvents c_dedsched As DataGridViewTextBoxColumn
    Friend WithEvents c_dedeffectivedatefrom As DataGridViewTextBoxColumn
    Friend WithEvents Comments As DataGridViewTextBoxColumn
    Friend WithEvents c_status As DataGridViewTextBoxColumn
End Class
