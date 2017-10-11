<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EducationalBackgroundForm
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
        Me.dgvEmplist = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_EmpID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_empname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.cmbEducType = New System.Windows.Forms.ComboBox()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpto = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblSchool = New System.Windows.Forms.Label()
        Me.txtSchool = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtMinor = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtCourse = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtDegree = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.dgvEducback = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_EmplyeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_school = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_degree = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_course = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_minor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_EducationalType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Datefrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Dateto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Remarks = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_RowID1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SuperTabControl1 = New DevComponents.DotNetBar.SuperTabControl()
        Me.SuperTabControlPanel1 = New DevComponents.DotNetBar.SuperTabControlPanel()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbempname = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SuperTabItem1 = New DevComponents.DotNetBar.SuperTabItem()
        Me.SuperTabControlPanel2 = New DevComponents.DotNetBar.SuperTabControlPanel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.SuperTabItem2 = New DevComponents.DotNetBar.SuperTabItem()
        CType(Me.dgvEmplist, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvEducback, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SuperTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuperTabControl1.SuspendLayout()
        Me.SuperTabControlPanel1.SuspendLayout()
        Me.SuperTabControlPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvEmplist
        '
        Me.dgvEmplist.AllowUserToAddRows = False
        Me.dgvEmplist.AllowUserToDeleteRows = False
        Me.dgvEmplist.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvEmplist.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvEmplist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEmplist.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_EmpID, Me.c_empname, Me.c_rowID})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEmplist.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEmplist.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEmplist.Location = New System.Drawing.Point(5, 128)
        Me.dgvEmplist.Name = "dgvEmplist"
        Me.dgvEmplist.ReadOnly = True
        Me.dgvEmplist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmplist.Size = New System.Drawing.Size(354, 328)
        Me.dgvEmplist.TabIndex = 311
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
        Me.c_empname.Width = 200
        '
        'c_rowID
        '
        Me.c_rowID.HeaderText = "Row ID"
        Me.c_rowID.Name = "c_rowID"
        Me.c_rowID.ReadOnly = True
        Me.c_rowID.Visible = False
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Silver
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Stencil", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(1179, 24)
        Me.Label16.TabIndex = 313
        Me.Label16.Text = "Education Background"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Location = New System.Drawing.Point(364, 27)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmbEducType)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblSaveMsg)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtRemarks)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.dtpto)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.dtpFrom)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblSchool)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtSchool)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label8)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtMinor)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label9)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtCourse)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label10)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtDegree)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label11)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ToolStrip1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.dgvEducback)
        Me.SplitContainer1.Size = New System.Drawing.Size(803, 429)
        Me.SplitContainer1.SplitterDistance = 160
        Me.SplitContainer1.TabIndex = 315
        '
        'cmbEducType
        '
        Me.cmbEducType.FormattingEnabled = True
        Me.cmbEducType.Items.AddRange(New Object() {"College", "High School", "Elementary", "Certification"})
        Me.cmbEducType.Location = New System.Drawing.Point(101, 47)
        Me.cmbEducType.Name = "cmbEducType"
        Me.cmbEducType.Size = New System.Drawing.Size(193, 21)
        Me.cmbEducType.TabIndex = 143
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.BackColor = System.Drawing.Color.Transparent
        Me.lblSaveMsg.Location = New System.Drawing.Point(161, 25)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(0, 13)
        Me.lblSaveMsg.TabIndex = 154
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(536, 47)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(52, 13)
        Me.Label3.TabIndex = 153
        Me.Label3.Text = "Remarks:"
        '
        'txtRemarks
        '
        Me.txtRemarks.Location = New System.Drawing.Point(591, 47)
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRemarks.Size = New System.Drawing.Size(190, 97)
        Me.txtRemarks.TabIndex = 151
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(315, 102)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 13)
        Me.Label2.TabIndex = 152
        Me.Label2.Text = "Date To:"
        '
        'dtpto
        '
        Me.dtpto.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpto.Location = New System.Drawing.Point(380, 102)
        Me.dtpto.Name = "dtpto"
        Me.dtpto.Size = New System.Drawing.Size(151, 20)
        Me.dtpto.TabIndex = 150
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(315, 76)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 13)
        Me.Label1.TabIndex = 150
        Me.Label1.Text = "Date From:"
        '
        'dtpFrom
        '
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFrom.Location = New System.Drawing.Point(380, 76)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(151, 20)
        Me.dtpFrom.TabIndex = 149
        '
        'lblSchool
        '
        Me.lblSchool.AutoSize = True
        Me.lblSchool.BackColor = System.Drawing.Color.Transparent
        Me.lblSchool.Location = New System.Drawing.Point(15, 74)
        Me.lblSchool.Name = "lblSchool"
        Me.lblSchool.Size = New System.Drawing.Size(43, 13)
        Me.lblSchool.TabIndex = 148
        Me.lblSchool.Text = "School:"
        '
        'txtSchool
        '
        Me.txtSchool.Location = New System.Drawing.Point(101, 74)
        Me.txtSchool.Multiline = True
        Me.txtSchool.Name = "txtSchool"
        Me.txtSchool.Size = New System.Drawing.Size(193, 21)
        Me.txtSchool.TabIndex = 144
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(315, 47)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(36, 13)
        Me.Label8.TabIndex = 147
        Me.Label8.Text = "Minor:"
        '
        'txtMinor
        '
        Me.txtMinor.Location = New System.Drawing.Point(380, 47)
        Me.txtMinor.Multiline = True
        Me.txtMinor.Name = "txtMinor"
        Me.txtMinor.Size = New System.Drawing.Size(151, 21)
        Me.txtMinor.TabIndex = 147
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Location = New System.Drawing.Point(15, 128)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(43, 13)
        Me.Label9.TabIndex = 146
        Me.Label9.Text = "Course:"
        '
        'txtCourse
        '
        Me.txtCourse.Location = New System.Drawing.Point(101, 128)
        Me.txtCourse.Multiline = True
        Me.txtCourse.Name = "txtCourse"
        Me.txtCourse.Size = New System.Drawing.Size(193, 21)
        Me.txtCourse.TabIndex = 146
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Location = New System.Drawing.Point(15, 101)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(45, 13)
        Me.Label10.TabIndex = 145
        Me.Label10.Text = "Degree:"
        '
        'txtDegree
        '
        Me.txtDegree.Location = New System.Drawing.Point(101, 102)
        Me.txtDegree.Multiline = True
        Me.txtDegree.Name = "txtDegree"
        Me.txtDegree.Size = New System.Drawing.Size(193, 21)
        Me.txtDegree.TabIndex = 145
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Location = New System.Drawing.Point(15, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(85, 13)
        Me.Label11.TabIndex = 144
        Me.Label11.Text = "Education Type:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnClose, Me.ToolStripSeparator1, Me.btnSave, Me.ToolStripSeparator2, Me.ToolStripLabel1, Me.btnDelete, Me.btnCancel, Me.tsAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(801, 25)
        Me.ToolStrip1.TabIndex = 62
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.GoldWingsPayrollSys.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(147, 22)
        Me.btnNew.Text = "&New Educ Background"
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
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'btnSave
        '
        Me.btnSave.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(147, 22)
        Me.btnSave.Text = "&Save Educ Background"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(89, 22)
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.cancel
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(156, 22)
        Me.btnDelete.Text = "&Delete Educ Background"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'tsAudittrail
        '
        Me.tsAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsAudittrail.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.audit_trail_icon
        Me.tsAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsAudittrail.Name = "tsAudittrail"
        Me.tsAudittrail.Size = New System.Drawing.Size(23, 22)
        '
        'dgvEducback
        '
        Me.dgvEducback.AllowUserToAddRows = False
        Me.dgvEducback.AllowUserToDeleteRows = False
        Me.dgvEducback.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEducback.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvEducback.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEducback.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_EmplyeeID, Me.c_name, Me.c_school, Me.c_degree, Me.c_course, Me.c_minor, Me.c_EducationalType, Me.c_Datefrom, Me.c_Dateto, Me.c_Remarks, Me.c_RowID1})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEducback.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvEducback.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEducback.Location = New System.Drawing.Point(3, 3)
        Me.dgvEducback.Name = "dgvEducback"
        Me.dgvEducback.ReadOnly = True
        Me.dgvEducback.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEducback.Size = New System.Drawing.Size(795, 257)
        Me.dgvEducback.TabIndex = 318
        '
        'c_EmplyeeID
        '
        Me.c_EmplyeeID.HeaderText = "Employee ID"
        Me.c_EmplyeeID.Name = "c_EmplyeeID"
        Me.c_EmplyeeID.ReadOnly = True
        '
        'c_name
        '
        Me.c_name.HeaderText = "Name"
        Me.c_name.Name = "c_name"
        Me.c_name.ReadOnly = True
        '
        'c_school
        '
        Me.c_school.HeaderText = "School"
        Me.c_school.Name = "c_school"
        Me.c_school.ReadOnly = True
        '
        'c_degree
        '
        Me.c_degree.HeaderText = "Degree"
        Me.c_degree.Name = "c_degree"
        Me.c_degree.ReadOnly = True
        '
        'c_course
        '
        Me.c_course.HeaderText = "Course"
        Me.c_course.Name = "c_course"
        Me.c_course.ReadOnly = True
        '
        'c_minor
        '
        Me.c_minor.HeaderText = "Minor"
        Me.c_minor.Name = "c_minor"
        Me.c_minor.ReadOnly = True
        '
        'c_EducationalType
        '
        Me.c_EducationalType.HeaderText = "Educational Type"
        Me.c_EducationalType.Name = "c_EducationalType"
        Me.c_EducationalType.ReadOnly = True
        '
        'c_Datefrom
        '
        Me.c_Datefrom.HeaderText = "Date From"
        Me.c_Datefrom.Name = "c_Datefrom"
        Me.c_Datefrom.ReadOnly = True
        '
        'c_Dateto
        '
        Me.c_Dateto.HeaderText = "Date To"
        Me.c_Dateto.Name = "c_Dateto"
        Me.c_Dateto.ReadOnly = True
        '
        'c_Remarks
        '
        Me.c_Remarks.HeaderText = "Remarks"
        Me.c_Remarks.Name = "c_Remarks"
        Me.c_Remarks.ReadOnly = True
        '
        'c_RowID1
        '
        Me.c_RowID1.HeaderText = "Column1"
        Me.c_RowID1.Name = "c_RowID1"
        Me.c_RowID1.ReadOnly = True
        Me.c_RowID1.Visible = False
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
        Me.SuperTabControl1.Location = New System.Drawing.Point(4, 28)
        Me.SuperTabControl1.Name = "SuperTabControl1"
        Me.SuperTabControl1.ReorderTabsEnabled = True
        Me.SuperTabControl1.SelectedTabFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.SuperTabControl1.SelectedTabIndex = 0
        Me.SuperTabControl1.Size = New System.Drawing.Size(347, 95)
        Me.SuperTabControl1.TabFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SuperTabControl1.TabIndex = 316
        Me.SuperTabControl1.Tabs.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.SuperTabItem1, Me.SuperTabItem2})
        Me.SuperTabControl1.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.WinMediaPlayer12
        Me.SuperTabControl1.Text = "SuperTabControl1"
        '
        'SuperTabControlPanel1
        '
        Me.SuperTabControlPanel1.Controls.Add(Me.TextBox4)
        Me.SuperTabControlPanel1.Controls.Add(Me.TextBox2)
        Me.SuperTabControlPanel1.Controls.Add(Me.Label6)
        Me.SuperTabControlPanel1.Controls.Add(Me.cmbempname)
        Me.SuperTabControlPanel1.Controls.Add(Me.Label4)
        Me.SuperTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SuperTabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.SuperTabControlPanel1.Name = "SuperTabControlPanel1"
        Me.SuperTabControlPanel1.Size = New System.Drawing.Size(347, 72)
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
        Me.TextBox2.Location = New System.Drawing.Point(187, 40)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(151, 21)
        Me.TextBox2.TabIndex = 148
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(3, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 13)
        Me.Label6.TabIndex = 147
        Me.Label6.Text = "Employee ID:"
        '
        'cmbempname
        '
        Me.cmbempname.FormattingEnabled = True
        Me.cmbempname.Items.AddRange(New Object() {"starts with", "contains like", "is exactly", "does not contain", "is empty null", "is not empty"})
        Me.cmbempname.Location = New System.Drawing.Point(90, 40)
        Me.cmbempname.Name = "cmbempname"
        Me.cmbempname.Size = New System.Drawing.Size(92, 21)
        Me.cmbempname.TabIndex = 146
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(3, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(87, 13)
        Me.Label4.TabIndex = 144
        Me.Label4.Text = "Employee Name:"
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
        Me.SuperTabControlPanel2.Controls.Add(Me.Label5)
        Me.SuperTabControlPanel2.Controls.Add(Me.Label13)
        Me.SuperTabControlPanel2.Controls.Add(Me.TextBox5)
        Me.SuperTabControlPanel2.Controls.Add(Me.TextBox3)
        Me.SuperTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SuperTabControlPanel2.Location = New System.Drawing.Point(0, 0)
        Me.SuperTabControlPanel2.Name = "SuperTabControlPanel2"
        Me.SuperTabControlPanel2.Size = New System.Drawing.Size(347, 95)
        Me.SuperTabControlPanel2.TabIndex = 0
        Me.SuperTabControlPanel2.TabItem = Me.SuperTabItem2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(4, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 13)
        Me.Label5.TabIndex = 149
        Me.Label5.Text = "Employee ID:"
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
        Me.TextBox5.Size = New System.Drawing.Size(234, 21)
        Me.TextBox5.TabIndex = 146
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(97, 12)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(234, 21)
        Me.TextBox3.TabIndex = 145
        '
        'SuperTabItem2
        '
        Me.SuperTabItem2.AttachedControl = Me.SuperTabControlPanel2
        Me.SuperTabItem2.GlobalItem = False
        Me.SuperTabItem2.Name = "SuperTabItem2"
        Me.SuperTabItem2.Text = "Simple Search"
        '
        'EducationalBackgroundForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(1179, 458)
        Me.ControlBox = False
        Me.Controls.Add(Me.SuperTabControl1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.dgvEmplist)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "EducationalBackgroundForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvEmplist, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvEducback, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SuperTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SuperTabControl1.ResumeLayout(False)
        Me.SuperTabControlPanel1.ResumeLayout(False)
        Me.SuperTabControlPanel1.PerformLayout()
        Me.SuperTabControlPanel2.ResumeLayout(False)
        Me.SuperTabControlPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvEmplist As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvEducback As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents lblSchool As System.Windows.Forms.Label
    Friend WithEvents txtSchool As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtMinor As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtCourse As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtDegree As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtRemarks As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpto As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents c_EmplyeeID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_school As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_degree As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_course As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_minor As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_EducationalType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_Datefrom As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_Dateto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_Remarks As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_RowID1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbempname As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
    Friend WithEvents c_EmpID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_empname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_rowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cmbEducType As System.Windows.Forms.ComboBox
    Private WithEvents SuperTabControl1 As DevComponents.DotNetBar.SuperTabControl
    Private WithEvents SuperTabControlPanel2 As DevComponents.DotNetBar.SuperTabControlPanel
    Private WithEvents SuperTabItem2 As DevComponents.DotNetBar.SuperTabItem
    Private WithEvents SuperTabControlPanel1 As DevComponents.DotNetBar.SuperTabControlPanel
    Private WithEvents SuperTabItem1 As DevComponents.DotNetBar.SuperTabItem
End Class
