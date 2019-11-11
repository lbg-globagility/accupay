<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmployeeLoanHistoryForm
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
        Me.DataGridViewX1 = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_empid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_empname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_emprowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtamount = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtremarks = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dateded = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbstatus = New System.Windows.Forms.ComboBox()
        Me.txtempid = New System.Windows.Forms.TextBox()
        Me.DataGridViewX2 = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_dateded = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Remarks = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_LoanID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txtEmpnameloan = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.DataGridViewX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.SuperTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuperTabControl1.SuspendLayout()
        Me.SuperTabControlPanel1.SuspendLayout()
        Me.SuperTabControlPanel2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.DataGridViewX2, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Label16.Size = New System.Drawing.Size(960, 24)
        Me.Label16.TabIndex = 313
        Me.Label16.Text = "Loan History"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DataGridViewX1
        '
        Me.DataGridViewX1.AllowUserToAddRows = False
        Me.DataGridViewX1.AllowUserToDeleteRows = False
        Me.DataGridViewX1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewX1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
        Me.DataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewX1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_empid, Me.c_empname, Me.c_emprowID})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewX1.DefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewX1.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.DataGridViewX1.Location = New System.Drawing.Point(3, 122)
        Me.DataGridViewX1.Name = "DataGridViewX1"
        Me.DataGridViewX1.ReadOnly = True
        Me.DataGridViewX1.Size = New System.Drawing.Size(307, 286)
        Me.DataGridViewX1.TabIndex = 314
        '
        'c_empid
        '
        Me.c_empid.HeaderText = "Employee ID"
        Me.c_empid.Name = "c_empid"
        Me.c_empid.ReadOnly = True
        '
        'c_empname
        '
        Me.c_empname.HeaderText = "Employee Name"
        Me.c_empname.Name = "c_empname"
        Me.c_empname.ReadOnly = True
        Me.c_empname.Width = 150
        '
        'c_emprowID
        '
        Me.c_emprowID.HeaderText = "RowID"
        Me.c_emprowID.Name = "c_emprowID"
        Me.c_emprowID.ReadOnly = True
        Me.c_emprowID.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.SuperTabControl1)
        Me.Panel1.Controls.Add(Me.DataGridViewX1)
        Me.Panel1.Location = New System.Drawing.Point(4, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(321, 413)
        Me.Panel1.TabIndex = 315
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
        Me.SuperTabControl1.Location = New System.Drawing.Point(3, 3)
        Me.SuperTabControl1.Name = "SuperTabControl1"
        Me.SuperTabControl1.ReorderTabsEnabled = True
        Me.SuperTabControl1.SelectedTabFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.SuperTabControl1.SelectedTabIndex = 0
        Me.SuperTabControl1.Size = New System.Drawing.Size(307, 113)
        Me.SuperTabControl1.TabFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SuperTabControl1.TabIndex = 323
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
        'Panel2
        '
        Me.Panel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.txtamount)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.txtremarks)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.dateded)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.cmbstatus)
        Me.Panel2.Controls.Add(Me.txtempid)
        Me.Panel2.Controls.Add(Me.DataGridViewX2)
        Me.Panel2.Controls.Add(Me.txtEmpnameloan)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.ToolStrip1)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Location = New System.Drawing.Point(331, 27)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(617, 413)
        Me.Panel2.TabIndex = 316
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(381, 73)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 13)
        Me.Label8.TabIndex = 335
        Me.Label8.Text = "Deduction Amount"
        '
        'txtamount
        '
        Me.txtamount.Location = New System.Drawing.Point(384, 90)
        Me.txtamount.Multiline = True
        Me.txtamount.Name = "txtamount"
        Me.txtamount.Size = New System.Drawing.Size(200, 21)
        Me.txtamount.TabIndex = 334
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(381, 32)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 333
        Me.Label6.Text = "Remarks"
        '
        'txtremarks
        '
        Me.txtremarks.Location = New System.Drawing.Point(384, 49)
        Me.txtremarks.Multiline = True
        Me.txtremarks.Name = "txtremarks"
        Me.txtremarks.Size = New System.Drawing.Size(200, 21)
        Me.txtremarks.TabIndex = 332
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(212, 71)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 13)
        Me.Label5.TabIndex = 331
        Me.Label5.Text = "Deduction Date"
        '
        'dateded
        '
        Me.dateded.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dateded.Location = New System.Drawing.Point(215, 87)
        Me.dateded.Name = "dateded"
        Me.dateded.Size = New System.Drawing.Size(163, 20)
        Me.dateded.TabIndex = 330
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(212, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 329
        Me.Label3.Text = "Status"
        '
        'cmbstatus
        '
        Me.cmbstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbstatus.FormattingEnabled = True
        Me.cmbstatus.Location = New System.Drawing.Point(215, 49)
        Me.cmbstatus.Name = "cmbstatus"
        Me.cmbstatus.Size = New System.Drawing.Size(163, 21)
        Me.cmbstatus.TabIndex = 328
        '
        'txtempid
        '
        Me.txtempid.Enabled = False
        Me.txtempid.Location = New System.Drawing.Point(9, 49)
        Me.txtempid.Multiline = True
        Me.txtempid.Name = "txtempid"
        Me.txtempid.Size = New System.Drawing.Size(200, 21)
        Me.txtempid.TabIndex = 153
        '
        'DataGridViewX2
        '
        Me.DataGridViewX2.AllowUserToAddRows = False
        Me.DataGridViewX2.AllowUserToDeleteRows = False
        Me.DataGridViewX2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewX2.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
        Me.DataGridViewX2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewX2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_dateded, Me.c_Amount, Me.c_Status, Me.c_Remarks, Me.c_LoanID})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewX2.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewX2.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.DataGridViewX2.Location = New System.Drawing.Point(3, 122)
        Me.DataGridViewX2.Name = "DataGridViewX2"
        Me.DataGridViewX2.ReadOnly = True
        Me.DataGridViewX2.Size = New System.Drawing.Size(609, 286)
        Me.DataGridViewX2.TabIndex = 324
        '
        'c_dateded
        '
        Me.c_dateded.HeaderText = "Deduction Date"
        Me.c_dateded.Name = "c_dateded"
        Me.c_dateded.ReadOnly = True
        Me.c_dateded.Width = 120
        '
        'c_Amount
        '
        Me.c_Amount.HeaderText = "Amount Deducted"
        Me.c_Amount.Name = "c_Amount"
        Me.c_Amount.ReadOnly = True
        Me.c_Amount.Width = 150
        '
        'c_Status
        '
        Me.c_Status.HeaderText = "Status"
        Me.c_Status.Name = "c_Status"
        Me.c_Status.ReadOnly = True
        '
        'c_Remarks
        '
        Me.c_Remarks.HeaderText = "Remarks"
        Me.c_Remarks.Name = "c_Remarks"
        Me.c_Remarks.ReadOnly = True
        Me.c_Remarks.Width = 150
        '
        'c_LoanID
        '
        Me.c_LoanID.HeaderText = "Loan ID"
        Me.c_LoanID.Name = "c_LoanID"
        Me.c_LoanID.ReadOnly = True
        Me.c_LoanID.Visible = False
        '
        'txtEmpnameloan
        '
        Me.txtEmpnameloan.Enabled = False
        Me.txtEmpnameloan.Location = New System.Drawing.Point(9, 90)
        Me.txtEmpnameloan.Multiline = True
        Me.txtEmpnameloan.Name = "txtEmpnameloan"
        Me.txtEmpnameloan.Size = New System.Drawing.Size(200, 21)
        Me.txtEmpnameloan.TabIndex = 152
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(6, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 151
        Me.Label1.Text = "Employee ID:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnClose, Me.btnAudittrail, Me.ToolStripButton1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(615, 25)
        Me.ToolStrip1.TabIndex = 327
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "&Close"
        '
        'btnAudittrail
        '
        Me.btnAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnAudittrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAudittrail.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.audit_trail_icon
        Me.btnAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAudittrail.Name = "btnAudittrail"
        Me.btnAudittrail.Size = New System.Drawing.Size(23, 22)
        Me.btnAudittrail.Text = "ToolStripButton1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.money1
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(75, 22)
        Me.ToolStripButton1.Text = "Pay Loan"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(6, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 150
        Me.Label2.Text = "Employee Name:"
        '
        'EmployeeLoanHistoryForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(960, 452)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label16)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "EmployeeLoanHistoryForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "EmployeeLoanHistoryForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridViewX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.SuperTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SuperTabControl1.ResumeLayout(False)
        Me.SuperTabControlPanel1.ResumeLayout(False)
        Me.SuperTabControlPanel1.PerformLayout()
        Me.SuperTabControlPanel2.ResumeLayout(False)
        Me.SuperTabControlPanel2.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.DataGridViewX2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents DataGridViewX1 As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents c_empid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_empname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_emprowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbempname As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtamount As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtremarks As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents dateded As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbstatus As System.Windows.Forms.ComboBox
    Friend WithEvents txtempid As System.Windows.Forms.TextBox
    Friend WithEvents DataGridViewX2 As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_dateded As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_Amount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_Status As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_Remarks As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_LoanID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txtEmpnameloan As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Private WithEvents SuperTabControl1 As DevComponents.DotNetBar.SuperTabControl
    Private WithEvents SuperTabControlPanel1 As DevComponents.DotNetBar.SuperTabControlPanel
    Private WithEvents SuperTabItem1 As DevComponents.DotNetBar.SuperTabItem
    Private WithEvents SuperTabControlPanel2 As DevComponents.DotNetBar.SuperTabControlPanel
    Private WithEvents SuperTabItem2 As DevComponents.DotNetBar.SuperTabItem
End Class
