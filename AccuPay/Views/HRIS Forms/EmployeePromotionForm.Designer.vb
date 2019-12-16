<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmployeePromotionForm
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblphilhealth = New System.Windows.Forms.Label()
        Me.lblsss = New System.Windows.Forms.Label()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.dtpEffectivityDate = New System.Windows.Forms.DateTimePicker()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.dgvPromotionList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtbasicpay = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbflg = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtempname = New System.Windows.Forms.TextBox()
        Me.txtempid = New System.Windows.Forms.TextBox()
        Me.cmbto = New System.Windows.Forms.ComboBox()
        Me.cmbfrom = New System.Windows.Forms.ComboBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmbSalaryChanged = New System.Windows.Forms.ComboBox()
        Me.c_empID2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_empname2 = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_PostionFrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_positionto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_effecDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_compensation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_basicpay = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.Panel2.SuspendLayout()
        CType(Me.SuperTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuperTabControl1.SuspendLayout()
        Me.SuperTabControlPanel1.SuspendLayout()
        Me.SuperTabControlPanel2.SuspendLayout()
        CType(Me.dgvEmpList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvPromotionList, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Label16.Size = New System.Drawing.Size(1242, 24)
        Me.Label16.TabIndex = 314
        Me.Label16.Text = "Employee Promotions"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.SuperTabControl1)
        Me.Panel2.Controls.Add(Me.dgvEmpList)
        Me.Panel2.Location = New System.Drawing.Point(5, 27)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(321, 443)
        Me.Panel2.TabIndex = 317
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
        Me.dgvEmpList.Size = New System.Drawing.Size(307, 320)
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
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.cmbSalaryChanged)
        Me.Panel1.Controls.Add(Me.lblphilhealth)
        Me.Panel1.Controls.Add(Me.lblsss)
        Me.Panel1.Controls.Add(Me.lblSaveMsg)
        Me.Panel1.Controls.Add(Me.dtpEffectivityDate)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.dgvPromotionList)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.txtbasicpay)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.cmbflg)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtempname)
        Me.Panel1.Controls.Add(Me.txtempid)
        Me.Panel1.Controls.Add(Me.cmbto)
        Me.Panel1.Controls.Add(Me.cmbfrom)
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Location = New System.Drawing.Point(332, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(898, 443)
        Me.Panel1.TabIndex = 318
        '
        'lblphilhealth
        '
        Me.lblphilhealth.AutoSize = True
        Me.lblphilhealth.Location = New System.Drawing.Point(515, 125)
        Me.lblphilhealth.Name = "lblphilhealth"
        Me.lblphilhealth.Size = New System.Drawing.Size(53, 13)
        Me.lblphilhealth.TabIndex = 345
        Me.lblphilhealth.Text = "Basic pay"
        Me.lblphilhealth.Visible = False
        '
        'lblsss
        '
        Me.lblsss.AutoSize = True
        Me.lblsss.Location = New System.Drawing.Point(432, 125)
        Me.lblsss.Name = "lblsss"
        Me.lblsss.Size = New System.Drawing.Size(53, 13)
        Me.lblsss.TabIndex = 344
        Me.lblsss.Text = "Basic pay"
        Me.lblsss.Visible = False
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.Location = New System.Drawing.Point(122, 30)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(67, 13)
        Me.lblSaveMsg.TabIndex = 343
        Me.lblSaveMsg.Text = "Employee ID"
        Me.lblSaveMsg.Visible = False
        '
        'dtpEffectivityDate
        '
        Me.dtpEffectivityDate.Enabled = False
        Me.dtpEffectivityDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEffectivityDate.Location = New System.Drawing.Point(225, 141)
        Me.dtpEffectivityDate.Name = "dtpEffectivityDate"
        Me.dtpEffectivityDate.Size = New System.Drawing.Size(195, 20)
        Me.dtpEffectivityDate.TabIndex = 342
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(222, 125)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(75, 13)
        Me.Label9.TabIndex = 341
        Me.Label9.Text = "Effective Date"
        '
        'dgvPromotionList
        '
        Me.dgvPromotionList.AllowUserToAddRows = False
        Me.dgvPromotionList.AllowUserToDeleteRows = False
        Me.dgvPromotionList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPromotionList.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvPromotionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPromotionList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_empID2, Me.c_empname2, Me.c_rowid, Me.c_PostionFrom, Me.c_positionto, Me.c_effecDate, Me.c_compensation, Me.c_basicpay})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPromotionList.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvPromotionList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvPromotionList.Location = New System.Drawing.Point(3, 167)
        Me.dgvPromotionList.Name = "dgvPromotionList"
        Me.dgvPromotionList.ReadOnly = True
        Me.dgvPromotionList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPromotionList.Size = New System.Drawing.Size(890, 271)
        Me.dgvPromotionList.TabIndex = 323
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(432, 85)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 13)
        Me.Label8.TabIndex = 340
        Me.Label8.Text = "Basic pay"
        '
        'txtbasicpay
        '
        Me.txtbasicpay.Enabled = False
        Me.txtbasicpay.Location = New System.Drawing.Point(435, 101)
        Me.txtbasicpay.Name = "txtbasicpay"
        Me.txtbasicpay.Size = New System.Drawing.Size(195, 20)
        Me.txtbasicpay.TabIndex = 339
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(432, 45)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(114, 13)
        Me.Label6.TabIndex = 338
        Me.Label6.Text = "Compensation Change"
        '
        'cmbflg
        '
        Me.cmbflg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbflg.Enabled = False
        Me.cmbflg.FormattingEnabled = True
        Me.cmbflg.Items.AddRange(New Object() {"Yes", "No"})
        Me.cmbflg.Location = New System.Drawing.Point(435, 61)
        Me.cmbflg.Name = "cmbflg"
        Me.cmbflg.Size = New System.Drawing.Size(195, 21)
        Me.cmbflg.TabIndex = 337
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(222, 85)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 336
        Me.Label5.Text = "Position To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(222, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 13)
        Me.Label3.TabIndex = 335
        Me.Label3.Text = "Position From"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 334
        Me.Label2.Text = "Employee Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 333
        Me.Label1.Text = "Employee ID"
        '
        'txtempname
        '
        Me.txtempname.Location = New System.Drawing.Point(13, 101)
        Me.txtempname.Name = "txtempname"
        Me.txtempname.Size = New System.Drawing.Size(195, 20)
        Me.txtempname.TabIndex = 332
        '
        'txtempid
        '
        Me.txtempid.Location = New System.Drawing.Point(13, 62)
        Me.txtempid.Name = "txtempid"
        Me.txtempid.Size = New System.Drawing.Size(195, 20)
        Me.txtempid.TabIndex = 331
        '
        'cmbto
        '
        Me.cmbto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbto.Enabled = False
        Me.cmbto.FormattingEnabled = True
        Me.cmbto.Location = New System.Drawing.Point(225, 101)
        Me.cmbto.Name = "cmbto"
        Me.cmbto.Size = New System.Drawing.Size(195, 21)
        Me.cmbto.TabIndex = 330
        '
        'cmbfrom
        '
        Me.cmbfrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbfrom.Enabled = False
        Me.cmbfrom.FormattingEnabled = True
        Me.cmbfrom.Location = New System.Drawing.Point(225, 61)
        Me.cmbfrom.Name = "cmbfrom"
        Me.cmbfrom.Size = New System.Drawing.Size(195, 21)
        Me.cmbfrom.TabIndex = 329
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.ToolStripLabel1, Me.ToolStripSeparator1, Me.btnDelete, Me.ToolStripSeparator2, Me.btnCancel, Me.btnClose, Me.btnAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(896, 25)
        Me.ToolStrip1.TabIndex = 328
        Me.ToolStrip1.Text = "ToolStrip1"
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
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'cmbSalaryChanged
        '
        Me.cmbSalaryChanged.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSalaryChanged.FormattingEnabled = True
        Me.cmbSalaryChanged.Items.AddRange(New Object() {"Yes", "No"})
        Me.cmbSalaryChanged.Location = New System.Drawing.Point(636, 61)
        Me.cmbSalaryChanged.Name = "cmbSalaryChanged"
        Me.cmbSalaryChanged.Size = New System.Drawing.Size(228, 21)
        Me.cmbSalaryChanged.TabIndex = 346
        Me.cmbSalaryChanged.Visible = False
        '
        'c_empID2
        '
        Me.c_empID2.HeaderText = "Employee ID"
        Me.c_empID2.Name = "c_empID2"
        Me.c_empID2.ReadOnly = True
        '
        'c_empname2
        '
        Me.c_empname2.HeaderText = "Employee Name"
        Me.c_empname2.Name = "c_empname2"
        Me.c_empname2.ReadOnly = True
        Me.c_empname2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_empname2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.c_empname2.Width = 150
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "RowID"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.ReadOnly = True
        Me.c_rowid.Visible = False
        '
        'c_PostionFrom
        '
        Me.c_PostionFrom.HeaderText = "Position From"
        Me.c_PostionFrom.Name = "c_PostionFrom"
        Me.c_PostionFrom.ReadOnly = True
        '
        'c_positionto
        '
        Me.c_positionto.HeaderText = "Position To"
        Me.c_positionto.Name = "c_positionto"
        Me.c_positionto.ReadOnly = True
        '
        'c_effecDate
        '
        Me.c_effecDate.HeaderText = "Effective Date"
        Me.c_effecDate.Name = "c_effecDate"
        Me.c_effecDate.ReadOnly = True
        '
        'c_compensation
        '
        Me.c_compensation.HeaderText = "Compensation"
        Me.c_compensation.Name = "c_compensation"
        Me.c_compensation.ReadOnly = True
        '
        'c_basicpay
        '
        Me.c_basicpay.HeaderText = "Basic Pay"
        Me.c_basicpay.Name = "c_basicpay"
        Me.c_basicpay.ReadOnly = True
        Me.c_basicpay.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_basicpay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'btnNew
        '
        Me.btnNew.Image = Global.GoldWingsPayrollSys.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(111, 22)
        Me.btnNew.Text = "&New Promotion"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(111, 22)
        Me.btnSave.Text = "&Save Promotion"
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 22)
        Me.btnDelete.Text = "&Delete Promotion"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
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
        'EmployeePromotionForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(1242, 482)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label16)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "EmployeePromotionForm"
        Me.Text = "EmployeePromotionForm"
        Me.Panel2.ResumeLayout(False)
        CType(Me.SuperTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SuperTabControl1.ResumeLayout(False)
        Me.SuperTabControlPanel1.ResumeLayout(False)
        Me.SuperTabControlPanel1.PerformLayout()
        Me.SuperTabControlPanel2.ResumeLayout(False)
        Me.SuperTabControlPanel2.PerformLayout()
        CType(Me.dgvEmpList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvPromotionList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents SuperTabControl1 As DevComponents.DotNetBar.SuperTabControl
    Friend WithEvents SuperTabControlPanel1 As DevComponents.DotNetBar.SuperTabControlPanel
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbempname As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents SuperTabItem1 As DevComponents.DotNetBar.SuperTabItem
    Friend WithEvents SuperTabControlPanel2 As DevComponents.DotNetBar.SuperTabControlPanel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents SuperTabItem2 As DevComponents.DotNetBar.SuperTabItem
    Friend WithEvents dgvEmpList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_EmployeeID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_EmployeeName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dgvPromotionList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtbasicpay As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbflg As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtempname As System.Windows.Forms.TextBox
    Friend WithEvents txtempid As System.Windows.Forms.TextBox
    Friend WithEvents cmbto As System.Windows.Forms.ComboBox
    Friend WithEvents cmbfrom As System.Windows.Forms.ComboBox
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
    Friend WithEvents dtpEffectivityDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
    Friend WithEvents lblphilhealth As System.Windows.Forms.Label
    Friend WithEvents lblsss As System.Windows.Forms.Label
    Friend WithEvents cmbSalaryChanged As System.Windows.Forms.ComboBox
    Friend WithEvents c_empID2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_empname2 As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents c_rowid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_PostionFrom As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_positionto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_effecDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_compensation As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_basicpay As System.Windows.Forms.DataGridViewLinkColumn
End Class
