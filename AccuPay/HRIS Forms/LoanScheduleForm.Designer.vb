<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoanScheduleForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.SuperTabControl1 = New DevComponents.DotNetBar.SuperTabControl()
        Me.SuperTabControlPanel1 = New DevComponents.DotNetBar.SuperTabControlPanel()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbempname = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.SuperTabItem1 = New DevComponents.DotNetBar.SuperTabItem()
        Me.SuperTabControlPanel2 = New DevComponents.DotNetBar.SuperTabControlPanel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.SuperTabItem2 = New DevComponents.DotNetBar.SuperTabItem()
        Me.dgvEmpList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_EmployeeName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvLoanList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_loanno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_totloanamt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_totballeft = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_dedamt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_DedPercent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_dedsched = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_noofpayperiod = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_remarks = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txtempid = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.lblAdd = New System.Windows.Forms.LinkLabel()
        Me.cmbdedsched = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtdedpercent = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.dateto = New System.Windows.Forms.DateTimePicker()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtremarks = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtnoofpayper = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtdedamt = New System.Windows.Forms.TextBox()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtbal = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtloanamt = New System.Windows.Forms.TextBox()
        Me.datefrom = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtloannumber = New System.Windows.Forms.TextBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.Panel2.SuspendLayout()
        CType(Me.SuperTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuperTabControl1.SuspendLayout()
        Me.SuperTabControlPanel1.SuspendLayout()
        Me.SuperTabControlPanel2.SuspendLayout()
        CType(Me.dgvEmpList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvLoanList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Silver
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Stencil", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(1053, 24)
        Me.Label16.TabIndex = 312
        Me.Label16.Text = "Loan Schedule"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.SuperTabControl1)
        Me.Panel2.Controls.Add(Me.dgvEmpList)
        Me.Panel2.Location = New System.Drawing.Point(4, 28)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(321, 425)
        Me.Panel2.TabIndex = 315
        '
        'SuperTabControl1
        '
        '
        '
        '
        '
        '
        '
        Me.SuperTabControl1.ControlBox.CloseBox.Name = ""
        '
        '
        '
        Me.SuperTabControl1.ControlBox.MenuBox.Name = ""
        Me.SuperTabControl1.ControlBox.Name = ""
        Me.SuperTabControl1.ControlBox.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.SuperTabControl1.ControlBox.MenuBox, Me.SuperTabControl1.ControlBox.CloseBox})
        Me.SuperTabControl1.Controls.Add(Me.SuperTabControlPanel1)
        Me.SuperTabControl1.Controls.Add(Me.SuperTabControlPanel2)
        Me.SuperTabControl1.Location = New System.Drawing.Point(7, 3)
        Me.SuperTabControl1.Name = "SuperTabControl1"
        Me.SuperTabControl1.ReorderTabsEnabled = True
        Me.SuperTabControl1.SelectedTabFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.SuperTabControl1.SelectedTabIndex = 0
        Me.SuperTabControl1.Size = New System.Drawing.Size(307, 113)
        Me.SuperTabControl1.TabFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SuperTabControl1.TabIndex = 322
        Me.SuperTabControl1.Tabs.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.SuperTabItem1, Me.SuperTabItem2})
        Me.SuperTabControl1.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.WinMediaPlayer12
        Me.SuperTabControl1.Text = "SuperTabControl1"
        '
        'SuperTabControlPanel1
        '
        Me.SuperTabControlPanel1.Controls.Add(Me.TextBox4)
        Me.SuperTabControlPanel1.Controls.Add(Me.TextBox2)
        Me.SuperTabControlPanel1.Controls.Add(Me.Label4)
        Me.SuperTabControlPanel1.Controls.Add(Me.cmbempname)
        Me.SuperTabControlPanel1.Controls.Add(Me.Label7)
        Me.SuperTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SuperTabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.SuperTabControlPanel1.Name = "SuperTabControlPanel1"
        Me.SuperTabControlPanel1.Size = New System.Drawing.Size(307, 90)
        Me.SuperTabControlPanel1.TabIndex = 1
        Me.SuperTabControlPanel1.TabItem = Me.SuperTabItem1
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(90, 12)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(92, 21)
        Me.TextBox4.TabIndex = 149
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(90, 65)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(200, 21)
        Me.TextBox2.TabIndex = 148
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(3, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 13)
        Me.Label4.TabIndex = 147
        Me.Label4.Text = "Employee ID:"
        '
        'cmbempname
        '
        Me.cmbempname.FormattingEnabled = True
        Me.cmbempname.Items.AddRange(New Object() {"starts with", "contains like", "is exactly", "does not contain", "is empty null", "is not empty"})
        Me.cmbempname.Location = New System.Drawing.Point(90, 40)
        Me.cmbempname.Name = "cmbempname"
        Me.cmbempname.Size = New System.Drawing.Size(200, 21)
        Me.cmbempname.TabIndex = 146
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(3, 43)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 13)
        Me.Label7.TabIndex = 144
        Me.Label7.Text = "Employee Name:"
        '
        'SuperTabItem1
        '
        Me.SuperTabItem1.AttachedControl = Me.SuperTabControlPanel1
        Me.SuperTabItem1.GlobalItem = False
        Me.SuperTabItem1.Name = "SuperTabItem1"
        Me.SuperTabItem1.Text = "Common Search"
        '
        'SuperTabControlPanel2
        '
        Me.SuperTabControlPanel2.Controls.Add(Me.Label10)
        Me.SuperTabControlPanel2.Controls.Add(Me.Label13)
        Me.SuperTabControlPanel2.Controls.Add(Me.TextBox5)
        Me.SuperTabControlPanel2.Controls.Add(Me.TextBox3)
        Me.SuperTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SuperTabControlPanel2.Location = New System.Drawing.Point(0, 0)
        Me.SuperTabControlPanel2.Name = "SuperTabControlPanel2"
        Me.SuperTabControlPanel2.Size = New System.Drawing.Size(307, 113)
        Me.SuperTabControlPanel2.TabIndex = 0
        Me.SuperTabControlPanel2.TabItem = Me.SuperTabItem2
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Location = New System.Drawing.Point(4, 12)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 13)
        Me.Label10.TabIndex = 149
        Me.Label10.Text = "Employee ID:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Location = New System.Drawing.Point(4, 43)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(87, 13)
        Me.Label13.TabIndex = 148
        Me.Label13.Text = "Employee Name:"
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(97, 40)
        Me.TextBox5.Multiline = True
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(207, 21)
        Me.TextBox5.TabIndex = 146
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(97, 12)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(207, 21)
        Me.TextBox3.TabIndex = 145
        '
        'SuperTabItem2
        '
        Me.SuperTabItem2.AttachedControl = Me.SuperTabControlPanel2
        Me.SuperTabItem2.GlobalItem = False
        Me.SuperTabItem2.Name = "SuperTabItem2"
        Me.SuperTabItem2.Text = "Simple Search"
        '
        'dgvEmpList
        '
        Me.dgvEmpList.AllowUserToAddRows = False
        Me.dgvEmpList.AllowUserToDeleteRows = False
        Me.dgvEmpList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEmpList.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvEmpList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEmpList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_EmployeeID, Me.c_EmployeeName, Me.c_ID})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEmpList.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEmpList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEmpList.Location = New System.Drawing.Point(7, 118)
        Me.dgvEmpList.Name = "dgvEmpList"
        Me.dgvEmpList.ReadOnly = True
        Me.dgvEmpList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmpList.Size = New System.Drawing.Size(307, 302)
        Me.dgvEmpList.TabIndex = 321
        '
        'c_EmployeeID
        '
        Me.c_EmployeeID.HeaderText = "Employee ID"
        Me.c_EmployeeID.Name = "c_EmployeeID"
        Me.c_EmployeeID.ReadOnly = True
        '
        'c_EmployeeName
        '
        Me.c_EmployeeName.HeaderText = "Employee Name"
        Me.c_EmployeeName.Name = "c_EmployeeName"
        Me.c_EmployeeName.ReadOnly = True
        Me.c_EmployeeName.Width = 150
        '
        'c_ID
        '
        Me.c_ID.HeaderText = "RowID"
        Me.c_ID.Name = "c_ID"
        Me.c_ID.ReadOnly = True
        Me.c_ID.Visible = False
        '
        'dgvLoanList
        '
        Me.dgvLoanList.AllowUserToAddRows = False
        Me.dgvLoanList.AllowUserToDeleteRows = False
        Me.dgvLoanList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvLoanList.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvLoanList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLoanList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_loanno, Me.c_totloanamt, Me.c_totballeft, Me.c_dedamt, Me.c_DedPercent, Me.c_dedsched, Me.c_noofpayperiod, Me.c_remarks, Me.c_rowid, Me.c_status})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvLoanList.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvLoanList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvLoanList.Location = New System.Drawing.Point(3, 244)
        Me.dgvLoanList.Name = "dgvLoanList"
        Me.dgvLoanList.ReadOnly = True
        Me.dgvLoanList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLoanList.Size = New System.Drawing.Size(703, 176)
        Me.dgvLoanList.TabIndex = 323
        '
        'c_loanno
        '
        Me.c_loanno.HeaderText = "Loan Number"
        Me.c_loanno.Name = "c_loanno"
        Me.c_loanno.ReadOnly = True
        '
        'c_totloanamt
        '
        Me.c_totloanamt.HeaderText = "Total Loan Amount"
        Me.c_totloanamt.Name = "c_totloanamt"
        Me.c_totloanamt.ReadOnly = True
        '
        'c_totballeft
        '
        Me.c_totballeft.HeaderText = "Total Balance Left"
        Me.c_totballeft.Name = "c_totballeft"
        Me.c_totballeft.ReadOnly = True
        '
        'c_dedamt
        '
        Me.c_dedamt.HeaderText = "Deduction Amount"
        Me.c_dedamt.Name = "c_dedamt"
        Me.c_dedamt.ReadOnly = True
        '
        'c_DedPercent
        '
        Me.c_DedPercent.HeaderText = "Deduction Percentage"
        Me.c_DedPercent.Name = "c_DedPercent"
        Me.c_DedPercent.ReadOnly = True
        '
        'c_dedsched
        '
        Me.c_dedsched.HeaderText = "Deduction Schedule"
        Me.c_dedsched.Name = "c_dedsched"
        Me.c_dedsched.ReadOnly = True
        '
        'c_noofpayperiod
        '
        Me.c_noofpayperiod.HeaderText = "No of pay period"
        Me.c_noofpayperiod.Name = "c_noofpayperiod"
        Me.c_noofpayperiod.ReadOnly = True
        '
        'c_remarks
        '
        Me.c_remarks.HeaderText = "Remarks"
        Me.c_remarks.Name = "c_remarks"
        Me.c_remarks.ReadOnly = True
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "RowiD"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.ReadOnly = True
        Me.c_rowid.Visible = False
        '
        'c_status
        '
        Me.c_status.HeaderText = "Status"
        Me.c_status.Name = "c_status"
        Me.c_status.ReadOnly = True
        '
        'txtempid
        '
        Me.txtempid.Enabled = False
        Me.txtempid.Location = New System.Drawing.Point(25, 62)
        Me.txtempid.Name = "txtempid"
        Me.txtempid.Size = New System.Drawing.Size(203, 20)
        Me.txtempid.TabIndex = 324
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 325
        Me.Label1.Text = "Employee ID"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblSaveMsg)
        Me.Panel1.Controls.Add(Me.lblAdd)
        Me.Panel1.Controls.Add(Me.cmbdedsched)
        Me.Panel1.Controls.Add(Me.Label17)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.txtdedpercent)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.dateto)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.txtremarks)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.txtnoofpayper)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.txtdedamt)
        Me.Panel1.Controls.Add(Me.cmbStatus)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.txtbal)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtloanamt)
        Me.Panel1.Controls.Add(Me.datefrom)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.txtloannumber)
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Controls.Add(Me.dgvLoanList)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtempid)
        Me.Panel1.Location = New System.Drawing.Point(330, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(711, 425)
        Me.Panel1.TabIndex = 326
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.Location = New System.Drawing.Point(122, 26)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(67, 13)
        Me.lblSaveMsg.TabIndex = 351
        Me.lblSaveMsg.Text = "Employee ID"
        Me.lblSaveMsg.Visible = False
        '
        'lblAdd
        '
        Me.lblAdd.AutoSize = True
        Me.lblAdd.Location = New System.Drawing.Point(356, 84)
        Me.lblAdd.Name = "lblAdd"
        Me.lblAdd.Size = New System.Drawing.Size(26, 13)
        Me.lblAdd.TabIndex = 350
        Me.lblAdd.TabStop = True
        Me.lblAdd.Text = "Add"
        '
        'cmbdedsched
        '
        Me.cmbdedsched.FormattingEnabled = True
        Me.cmbdedsched.Location = New System.Drawing.Point(249, 101)
        Me.cmbdedsched.Name = "cmbdedsched"
        Me.cmbdedsched.Size = New System.Drawing.Size(204, 21)
        Me.cmbdedsched.TabIndex = 330
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(246, 85)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(104, 13)
        Me.Label17.TabIndex = 348
        Me.Label17.Text = "Deduction Schedule"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(246, 46)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(114, 13)
        Me.Label15.TabIndex = 346
        Me.Label15.Text = "Deduction Percentage"
        '
        'txtdedpercent
        '
        Me.txtdedpercent.Location = New System.Drawing.Point(249, 62)
        Me.txtdedpercent.Name = "txtdedpercent"
        Me.txtdedpercent.Size = New System.Drawing.Size(203, 20)
        Me.txtdedpercent.TabIndex = 329
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(470, 84)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(46, 13)
        Me.Label14.TabIndex = 344
        Me.Label14.Text = "Date To"
        '
        'dateto
        '
        Me.dateto.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dateto.Location = New System.Drawing.Point(473, 101)
        Me.dateto.Name = "dateto"
        Me.dateto.Size = New System.Drawing.Size(203, 20)
        Me.dateto.TabIndex = 334
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(470, 45)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(56, 13)
        Me.Label12.TabIndex = 342
        Me.Label12.Text = "Date From"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(246, 164)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(37, 13)
        Me.Label11.TabIndex = 341
        Me.Label11.Text = "Status"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(470, 123)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 13)
        Me.Label9.TabIndex = 340
        Me.Label9.Text = "Remarks"
        '
        'txtremarks
        '
        Me.txtremarks.Location = New System.Drawing.Point(473, 139)
        Me.txtremarks.Multiline = True
        Me.txtremarks.Name = "txtremarks"
        Me.txtremarks.Size = New System.Drawing.Size(203, 58)
        Me.txtremarks.TabIndex = 335
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(246, 125)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(90, 13)
        Me.Label8.TabIndex = 338
        Me.Label8.Text = "No. of Pay Period"
        '
        'txtnoofpayper
        '
        Me.txtnoofpayper.Location = New System.Drawing.Point(249, 141)
        Me.txtnoofpayper.Name = "txtnoofpayper"
        Me.txtnoofpayper.Size = New System.Drawing.Size(203, 20)
        Me.txtnoofpayper.TabIndex = 331
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(22, 202)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(95, 13)
        Me.Label6.TabIndex = 336
        Me.Label6.Text = "Deduction Amount"
        '
        'txtdedamt
        '
        Me.txtdedamt.Location = New System.Drawing.Point(25, 218)
        Me.txtdedamt.Name = "txtdedamt"
        Me.txtdedamt.Size = New System.Drawing.Size(203, 20)
        Me.txtdedamt.TabIndex = 328
        '
        'cmbStatus
        '
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Items.AddRange(New Object() {"In Progress", "Cancelled"})
        Me.cmbStatus.Location = New System.Drawing.Point(249, 180)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(204, 21)
        Me.cmbStatus.TabIndex = 332
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(22, 163)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(94, 13)
        Me.Label5.TabIndex = 333
        Me.Label5.Text = "Total Balance Left"
        '
        'txtbal
        '
        Me.txtbal.Enabled = False
        Me.txtbal.Location = New System.Drawing.Point(25, 179)
        Me.txtbal.Name = "txtbal"
        Me.txtbal.Size = New System.Drawing.Size(203, 20)
        Me.txtbal.TabIndex = 327
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(22, 124)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 13)
        Me.Label3.TabIndex = 331
        Me.Label3.Text = "Total Loan Amount"
        '
        'txtloanamt
        '
        Me.txtloanamt.Location = New System.Drawing.Point(25, 140)
        Me.txtloanamt.Name = "txtloanamt"
        Me.txtloanamt.Size = New System.Drawing.Size(203, 20)
        Me.txtloanamt.TabIndex = 326
        '
        'datefrom
        '
        Me.datefrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.datefrom.Location = New System.Drawing.Point(473, 62)
        Me.datefrom.Name = "datefrom"
        Me.datefrom.Size = New System.Drawing.Size(203, 20)
        Me.datefrom.TabIndex = 333
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(22, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 328
        Me.Label2.Text = "Loan Number"
        '
        'txtloannumber
        '
        Me.txtloannumber.Enabled = False
        Me.txtloannumber.Location = New System.Drawing.Point(25, 101)
        Me.txtloannumber.Name = "txtloannumber"
        Me.txtloannumber.Size = New System.Drawing.Size(203, 20)
        Me.txtloannumber.TabIndex = 325
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.ToolStripLabel1, Me.ToolStripSeparator1, Me.btnDelete, Me.ToolStripSeparator2, Me.btnCancel, Me.btnClose, Me.btnAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(709, 25)
        Me.ToolStrip1.TabIndex = 326
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(115, 22)
        Me.btnNew.Text = "&New Load Sched"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(115, 22)
        Me.btnSave.Text = "&Save Load Sched"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(50, 22)
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(124, 22)
        Me.btnDelete.Text = "&Delete Load Sched"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "&Close"
        '
        'btnAudittrail
        '
        Me.btnAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnAudittrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAudittrail.Image = Global.AccuPay.My.Resources.Resources.audit_trail_icon
        Me.btnAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAudittrail.Name = "btnAudittrail"
        Me.btnAudittrail.Size = New System.Drawing.Size(23, 22)
        Me.btnAudittrail.Text = "ToolStripButton1"
        '
        'LoanScheduleForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1053, 457)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label16)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "LoanScheduleForm"
        Me.Text = "LoanScheduleForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        CType(Me.SuperTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SuperTabControl1.ResumeLayout(False)
        Me.SuperTabControlPanel1.ResumeLayout(False)
        Me.SuperTabControlPanel1.PerformLayout()
        Me.SuperTabControlPanel2.ResumeLayout(False)
        Me.SuperTabControlPanel2.PerformLayout()
        CType(Me.dgvEmpList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvLoanList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbempname As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents dgvEmpList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_EmployeeID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_EmployeeName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvLoanList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents txtempid As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtdedpercent As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents dateto As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtremarks As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtnoofpayper As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtdedamt As System.Windows.Forms.TextBox
    Friend WithEvents cmbStatus As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtbal As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtloanamt As System.Windows.Forms.TextBox
    Friend WithEvents datefrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtloannumber As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents cmbdedsched As System.Windows.Forms.ComboBox
    Friend WithEvents lblAdd As System.Windows.Forms.LinkLabel
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
    Friend WithEvents c_loanno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_totloanamt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_totballeft As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_dedamt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_DedPercent As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_dedsched As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_noofpayperiod As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_remarks As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_rowid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_status As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents SuperTabControl1 As DevComponents.DotNetBar.SuperTabControl
    Private WithEvents SuperTabControlPanel1 As DevComponents.DotNetBar.SuperTabControlPanel
    Private WithEvents SuperTabItem1 As DevComponents.DotNetBar.SuperTabItem
    Private WithEvents SuperTabControlPanel2 As DevComponents.DotNetBar.SuperTabControlPanel
    Private WithEvents SuperTabItem2 As DevComponents.DotNetBar.SuperTabItem
End Class
