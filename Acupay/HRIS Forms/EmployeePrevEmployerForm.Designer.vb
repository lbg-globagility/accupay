<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmployeePrevEmployerForm
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dgvEmplist = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_EmpID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_empname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_LrowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.dgvListCompany = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_compname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_trade = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_contname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_mainphone = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_altphone = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_faxno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_emailaddr = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_altemailaddr = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_url = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_tinno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_jobtitle = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_jobfunction = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_orgtype = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_experience = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_compaddr = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grpDetails = New System.Windows.Forms.GroupBox()
        Me.txtExfromto = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCompAddr = New System.Windows.Forms.TextBox()
        Me.txtOrganizationType = New System.Windows.Forms.TextBox()
        Me.txtJobFunction = New System.Windows.Forms.TextBox()
        Me.txtJobTitle = New System.Windows.Forms.TextBox()
        Me.txtTinNo = New System.Windows.Forms.TextBox()
        Me.txtContactName = New System.Windows.Forms.TextBox()
        Me.txtUrl = New System.Windows.Forms.TextBox()
        Me.txtAltEmailAdd = New System.Windows.Forms.TextBox()
        Me.txtEmailAdd = New System.Windows.Forms.TextBox()
        Me.txtFaxNo = New System.Windows.Forms.TextBox()
        Me.txtAltPhone = New System.Windows.Forms.TextBox()
        Me.txtMainPhone = New System.Windows.Forms.TextBox()
        Me.txtTradeName = New System.Windows.Forms.TextBox()
        Me.txtCompanyName = New System.Windows.Forms.TextBox()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvEmplist, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvListCompany, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDetails.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.dgvEmplist)
        Me.Panel1.Controls.Add(Me.Label17)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Location = New System.Drawing.Point(3, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(325, 424)
        Me.Panel1.TabIndex = 309
        '
        'dgvEmplist
        '
        Me.dgvEmplist.AllowUserToAddRows = False
        Me.dgvEmplist.AllowUserToDeleteRows = False
        Me.dgvEmplist.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvEmplist.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvEmplist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEmplist.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_EmpID, Me.c_empname, Me.c_LrowID})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEmplist.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEmplist.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEmplist.Location = New System.Drawing.Point(3, 54)
        Me.dgvEmplist.Name = "dgvEmplist"
        Me.dgvEmplist.ReadOnly = True
        Me.dgvEmplist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmplist.Size = New System.Drawing.Size(316, 365)
        Me.dgvEmplist.TabIndex = 312
        '
        'c_EmpID
        '
        Me.c_EmpID.HeaderText = "Employee ID"
        Me.c_EmpID.Name = "c_EmpID"
        Me.c_EmpID.ReadOnly = True
        Me.c_EmpID.Width = 95
        '
        'c_empname
        '
        Me.c_empname.HeaderText = "Employee Name"
        Me.c_empname.Name = "c_empname"
        Me.c_empname.ReadOnly = True
        Me.c_empname.Width = 170
        '
        'c_LrowID
        '
        Me.c_LrowID.HeaderText = "Row ID"
        Me.c_LrowID.Name = "c_LrowID"
        Me.c_LrowID.ReadOnly = True
        Me.c_LrowID.Visible = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(6, 28)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(87, 13)
        Me.Label17.TabIndex = 34
        Me.Label17.Text = "Employee Name:"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(97, 28)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(223, 20)
        Me.TextBox1.TabIndex = 34
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.lblSaveMsg)
        Me.Panel2.Controls.Add(Me.dgvListCompany)
        Me.Panel2.Controls.Add(Me.grpDetails)
        Me.Panel2.Controls.Add(Me.ToolStrip1)
        Me.Panel2.Location = New System.Drawing.Point(333, 27)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(942, 424)
        Me.Panel2.TabIndex = 310
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.Location = New System.Drawing.Point(143, 28)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(0, 13)
        Me.lblSaveMsg.TabIndex = 40
        '
        'dgvListCompany
        '
        Me.dgvListCompany.AllowUserToAddRows = False
        Me.dgvListCompany.AllowUserToDeleteRows = False
        Me.dgvListCompany.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvListCompany.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvListCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvListCompany.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_compname, Me.c_trade, Me.c_contname, Me.c_mainphone, Me.c_altphone, Me.c_faxno, Me.c_emailaddr, Me.c_altemailaddr, Me.c_url, Me.c_tinno, Me.c_jobtitle, Me.c_jobfunction, Me.c_orgtype, Me.c_experience, Me.c_compaddr, Me.c_rowid})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvListCompany.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvListCompany.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvListCompany.Location = New System.Drawing.Point(7, 285)
        Me.dgvListCompany.Name = "dgvListCompany"
        Me.dgvListCompany.ReadOnly = True
        Me.dgvListCompany.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvListCompany.Size = New System.Drawing.Size(930, 134)
        Me.dgvListCompany.TabIndex = 319
        '
        'c_compname
        '
        Me.c_compname.HeaderText = "Company Name"
        Me.c_compname.Name = "c_compname"
        Me.c_compname.ReadOnly = True
        '
        'c_trade
        '
        Me.c_trade.HeaderText = "Trade Name"
        Me.c_trade.Name = "c_trade"
        Me.c_trade.ReadOnly = True
        '
        'c_contname
        '
        Me.c_contname.HeaderText = "Contact Name"
        Me.c_contname.Name = "c_contname"
        Me.c_contname.ReadOnly = True
        '
        'c_mainphone
        '
        Me.c_mainphone.HeaderText = "Main Phone"
        Me.c_mainphone.Name = "c_mainphone"
        Me.c_mainphone.ReadOnly = True
        '
        'c_altphone
        '
        Me.c_altphone.HeaderText = "Alt Phone"
        Me.c_altphone.Name = "c_altphone"
        Me.c_altphone.ReadOnly = True
        '
        'c_faxno
        '
        Me.c_faxno.HeaderText = "Fax No."
        Me.c_faxno.Name = "c_faxno"
        Me.c_faxno.ReadOnly = True
        '
        'c_emailaddr
        '
        Me.c_emailaddr.HeaderText = "Email Address"
        Me.c_emailaddr.Name = "c_emailaddr"
        Me.c_emailaddr.ReadOnly = True
        '
        'c_altemailaddr
        '
        Me.c_altemailaddr.HeaderText = "Alt Email Address"
        Me.c_altemailaddr.Name = "c_altemailaddr"
        Me.c_altemailaddr.ReadOnly = True
        '
        'c_url
        '
        Me.c_url.HeaderText = "URL"
        Me.c_url.Name = "c_url"
        Me.c_url.ReadOnly = True
        '
        'c_tinno
        '
        Me.c_tinno.HeaderText = "TIN No."
        Me.c_tinno.Name = "c_tinno"
        Me.c_tinno.ReadOnly = True
        '
        'c_jobtitle
        '
        Me.c_jobtitle.HeaderText = "Job Title"
        Me.c_jobtitle.Name = "c_jobtitle"
        Me.c_jobtitle.ReadOnly = True
        '
        'c_jobfunction
        '
        Me.c_jobfunction.HeaderText = "Job Function"
        Me.c_jobfunction.Name = "c_jobfunction"
        Me.c_jobfunction.ReadOnly = True
        '
        'c_orgtype
        '
        Me.c_orgtype.HeaderText = "Organization Type"
        Me.c_orgtype.Name = "c_orgtype"
        Me.c_orgtype.ReadOnly = True
        '
        'c_experience
        '
        Me.c_experience.HeaderText = "Experience From To date"
        Me.c_experience.Name = "c_experience"
        Me.c_experience.ReadOnly = True
        '
        'c_compaddr
        '
        Me.c_compaddr.HeaderText = "Company Address"
        Me.c_compaddr.Name = "c_compaddr"
        Me.c_compaddr.ReadOnly = True
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "RowiD"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.ReadOnly = True
        Me.c_rowid.Visible = False
        '
        'grpDetails
        '
        Me.grpDetails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDetails.Controls.Add(Me.txtExfromto)
        Me.grpDetails.Controls.Add(Me.Label23)
        Me.grpDetails.Controls.Add(Me.Label22)
        Me.grpDetails.Controls.Add(Me.Label21)
        Me.grpDetails.Controls.Add(Me.Label20)
        Me.grpDetails.Controls.Add(Me.Label19)
        Me.grpDetails.Controls.Add(Me.Label18)
        Me.grpDetails.Controls.Add(Me.Label11)
        Me.grpDetails.Controls.Add(Me.Label12)
        Me.grpDetails.Controls.Add(Me.Label13)
        Me.grpDetails.Controls.Add(Me.Label14)
        Me.grpDetails.Controls.Add(Me.Label15)
        Me.grpDetails.Controls.Add(Me.Label6)
        Me.grpDetails.Controls.Add(Me.Label7)
        Me.grpDetails.Controls.Add(Me.Label8)
        Me.grpDetails.Controls.Add(Me.Label9)
        Me.grpDetails.Controls.Add(Me.Label10)
        Me.grpDetails.Controls.Add(Me.Label5)
        Me.grpDetails.Controls.Add(Me.Label4)
        Me.grpDetails.Controls.Add(Me.Label3)
        Me.grpDetails.Controls.Add(Me.Label2)
        Me.grpDetails.Controls.Add(Me.Label1)
        Me.grpDetails.Controls.Add(Me.txtCompAddr)
        Me.grpDetails.Controls.Add(Me.txtOrganizationType)
        Me.grpDetails.Controls.Add(Me.txtJobFunction)
        Me.grpDetails.Controls.Add(Me.txtJobTitle)
        Me.grpDetails.Controls.Add(Me.txtTinNo)
        Me.grpDetails.Controls.Add(Me.txtContactName)
        Me.grpDetails.Controls.Add(Me.txtUrl)
        Me.grpDetails.Controls.Add(Me.txtAltEmailAdd)
        Me.grpDetails.Controls.Add(Me.txtEmailAdd)
        Me.grpDetails.Controls.Add(Me.txtFaxNo)
        Me.grpDetails.Controls.Add(Me.txtAltPhone)
        Me.grpDetails.Controls.Add(Me.txtMainPhone)
        Me.grpDetails.Controls.Add(Me.txtTradeName)
        Me.grpDetails.Controls.Add(Me.txtCompanyName)
        Me.grpDetails.Controls.Add(Me.DataGridView2)
        Me.grpDetails.Location = New System.Drawing.Point(7, 39)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(930, 240)
        Me.grpDetails.TabIndex = 309
        Me.grpDetails.TabStop = False
        Me.grpDetails.Text = "Employee Previous Employer Info"
        '
        'txtExfromto
        '
        Me.txtExfromto.Location = New System.Drawing.Point(733, 43)
        Me.txtExfromto.Name = "txtExfromto"
        Me.txtExfromto.Size = New System.Drawing.Size(185, 20)
        Me.txtExfromto.TabIndex = 14
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(824, 72)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(11, 13)
        Me.Label23.TabIndex = 39
        Me.Label23.Text = "*"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.ForeColor = System.Drawing.Color.Red
        Me.Label22.Location = New System.Drawing.Point(321, 186)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(11, 13)
        Me.Label22.TabIndex = 38
        Me.Label22.Text = "*"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.ForeColor = System.Drawing.Color.Red
        Me.Label21.Location = New System.Drawing.Point(347, 69)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(11, 13)
        Me.Label21.TabIndex = 37
        Me.Label21.Text = "*"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.ForeColor = System.Drawing.Color.Red
        Me.Label20.Location = New System.Drawing.Point(78, 147)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(11, 13)
        Me.Label20.TabIndex = 36
        Me.Label20.Text = "*"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Red
        Me.Label19.Location = New System.Drawing.Point(86, 107)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(11, 13)
        Me.Label19.TabIndex = 35
        Me.Label19.Text = "*"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.Color.Red
        Me.Label18.Location = New System.Drawing.Point(97, 27)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(11, 13)
        Me.Label18.TabIndex = 34
        Me.Label18.Text = "*"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(730, 69)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(95, 13)
        Me.Label11.TabIndex = 31
        Me.Label11.Text = "Company Address:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(730, 24)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(115, 13)
        Me.Label12.TabIndex = 30
        Me.Label12.Text = "Experience From Date:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(532, 186)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(93, 13)
        Me.Label13.TabIndex = 29
        Me.Label13.Text = "Organization Type"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(532, 66)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(71, 13)
        Me.Label14.TabIndex = 28
        Me.Label14.Text = "Job Function:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(532, 25)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(50, 13)
        Me.Label15.TabIndex = 27
        Me.Label15.Text = "Job Title:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(272, 185)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(48, 13)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "TIN No.:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 108)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(78, 13)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Contact Name:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(272, 146)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(32, 13)
        Me.Label8.TabIndex = 24
        Me.Label8.Text = "URL:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(272, 107)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(91, 13)
        Me.Label9.TabIndex = 23
        Me.Label9.Text = "Alt Email Address:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(272, 68)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(76, 13)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "Email Address:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(272, 29)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 13)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Fax No.:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 186)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Alt Phone:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 147)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Main Phone:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Trade Name:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Company Name:"
        '
        'txtCompAddr
        '
        Me.txtCompAddr.Location = New System.Drawing.Point(733, 91)
        Me.txtCompAddr.Multiline = True
        Me.txtCompAddr.Name = "txtCompAddr"
        Me.txtCompAddr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCompAddr.Size = New System.Drawing.Size(191, 91)
        Me.txtCompAddr.TabIndex = 15
        '
        'txtOrganizationType
        '
        Me.txtOrganizationType.Location = New System.Drawing.Point(532, 202)
        Me.txtOrganizationType.Name = "txtOrganizationType"
        Me.txtOrganizationType.Size = New System.Drawing.Size(185, 20)
        Me.txtOrganizationType.TabIndex = 13
        '
        'txtJobFunction
        '
        Me.txtJobFunction.Location = New System.Drawing.Point(532, 82)
        Me.txtJobFunction.Multiline = True
        Me.txtJobFunction.Name = "txtJobFunction"
        Me.txtJobFunction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtJobFunction.Size = New System.Drawing.Size(185, 100)
        Me.txtJobFunction.TabIndex = 12
        '
        'txtJobTitle
        '
        Me.txtJobTitle.Location = New System.Drawing.Point(532, 43)
        Me.txtJobTitle.Name = "txtJobTitle"
        Me.txtJobTitle.Size = New System.Drawing.Size(185, 20)
        Me.txtJobTitle.TabIndex = 11
        '
        'txtTinNo
        '
        Me.txtTinNo.Location = New System.Drawing.Point(275, 201)
        Me.txtTinNo.Name = "txtTinNo"
        Me.txtTinNo.Size = New System.Drawing.Size(236, 20)
        Me.txtTinNo.TabIndex = 10
        '
        'txtContactName
        '
        Me.txtContactName.Location = New System.Drawing.Point(11, 124)
        Me.txtContactName.Name = "txtContactName"
        Me.txtContactName.Size = New System.Drawing.Size(236, 20)
        Me.txtContactName.TabIndex = 3
        '
        'txtUrl
        '
        Me.txtUrl.Location = New System.Drawing.Point(272, 162)
        Me.txtUrl.Name = "txtUrl"
        Me.txtUrl.Size = New System.Drawing.Size(236, 20)
        Me.txtUrl.TabIndex = 9
        '
        'txtAltEmailAdd
        '
        Me.txtAltEmailAdd.Location = New System.Drawing.Point(272, 123)
        Me.txtAltEmailAdd.Name = "txtAltEmailAdd"
        Me.txtAltEmailAdd.Size = New System.Drawing.Size(236, 20)
        Me.txtAltEmailAdd.TabIndex = 8
        '
        'txtEmailAdd
        '
        Me.txtEmailAdd.Location = New System.Drawing.Point(272, 84)
        Me.txtEmailAdd.Name = "txtEmailAdd"
        Me.txtEmailAdd.Size = New System.Drawing.Size(236, 20)
        Me.txtEmailAdd.TabIndex = 7
        '
        'txtFaxNo
        '
        Me.txtFaxNo.Location = New System.Drawing.Point(272, 45)
        Me.txtFaxNo.Name = "txtFaxNo"
        Me.txtFaxNo.Size = New System.Drawing.Size(236, 20)
        Me.txtFaxNo.TabIndex = 6
        '
        'txtAltPhone
        '
        Me.txtAltPhone.Location = New System.Drawing.Point(11, 202)
        Me.txtAltPhone.Name = "txtAltPhone"
        Me.txtAltPhone.Size = New System.Drawing.Size(236, 20)
        Me.txtAltPhone.TabIndex = 5
        '
        'txtMainPhone
        '
        Me.txtMainPhone.Location = New System.Drawing.Point(11, 163)
        Me.txtMainPhone.Name = "txtMainPhone"
        Me.txtMainPhone.Size = New System.Drawing.Size(236, 20)
        Me.txtMainPhone.TabIndex = 4
        '
        'txtTradeName
        '
        Me.txtTradeName.Location = New System.Drawing.Point(11, 85)
        Me.txtTradeName.Name = "txtTradeName"
        Me.txtTradeName.Size = New System.Drawing.Size(236, 20)
        Me.txtTradeName.TabIndex = 3
        '
        'txtCompanyName
        '
        Me.txtCompanyName.Location = New System.Drawing.Point(11, 43)
        Me.txtCompanyName.Name = "txtCompanyName"
        Me.txtCompanyName.Size = New System.Drawing.Size(236, 20)
        Me.txtCompanyName.TabIndex = 2
        '
        'DataGridView2
        '
        Me.DataGridView2.BackgroundColor = System.Drawing.SystemColors.Control
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(6, 257)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(317, 110)
        Me.DataGridView2.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.ToolStripLabel1, Me.ToolStripSeparator1, Me.btnDelete, Me.ToolStripSeparator2, Me.btnCancel, Me.btnClose})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(940, 25)
        Me.ToolStrip1.TabIndex = 308
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.GoldWingsPayrollSys.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(130, 22)
        Me.btnNew.Text = "&New Prev Employer"
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(130, 22)
        Me.btnSave.Text = "&Save Prev Employer"
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
        Me.btnDelete.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(139, 22)
        Me.btnDelete.Text = "&Delete Prev Employer"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
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
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Silver
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Stencil", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(1287, 24)
        Me.Label16.TabIndex = 311
        Me.Label16.Text = "Employee Previous Employer's"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'EmployeePrevEmployerForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(1287, 454)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "EmployeePrevEmployerForm"
        Me.Text = "EmployeePrevEmployerForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvEmplist, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvListCompany, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDetails.ResumeLayout(False)
        Me.grpDetails.PerformLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents txtCompAddr As System.Windows.Forms.TextBox
    Friend WithEvents txtOrganizationType As System.Windows.Forms.TextBox
    Friend WithEvents txtJobFunction As System.Windows.Forms.TextBox
    Friend WithEvents txtJobTitle As System.Windows.Forms.TextBox
    Friend WithEvents txtTinNo As System.Windows.Forms.TextBox
    Friend WithEvents txtContactName As System.Windows.Forms.TextBox
    Friend WithEvents txtUrl As System.Windows.Forms.TextBox
    Friend WithEvents txtAltEmailAdd As System.Windows.Forms.TextBox
    Friend WithEvents txtEmailAdd As System.Windows.Forms.TextBox
    Friend WithEvents txtFaxNo As System.Windows.Forms.TextBox
    Friend WithEvents txtAltPhone As System.Windows.Forms.TextBox
    Friend WithEvents txtMainPhone As System.Windows.Forms.TextBox
    Friend WithEvents txtTradeName As System.Windows.Forms.TextBox
    Friend WithEvents txtCompanyName As System.Windows.Forms.TextBox
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtExfromto As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents dgvListCompany As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_compname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_trade As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_contname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_mainphone As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_altphone As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_faxno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_emailaddr As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_altemailaddr As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_url As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_tinno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_jobtitle As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_jobfunction As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_orgtype As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_experience As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_compaddr As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_rowid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvEmplist As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_EmpID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_empname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_LrowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
End Class
