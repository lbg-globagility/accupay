<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EducationalBackgroundTab
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EducationalBackgroundTab))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip9 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnUserActivity = New System.Windows.Forms.ToolStripButton()
        Me.txtEmployeeID = New System.Windows.Forms.TextBox()
        Me.txtFullname = New System.Windows.Forms.TextBox()
        Me.pbEmployee = New System.Windows.Forms.PictureBox()
        Me.grpSalary = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblSchool = New System.Windows.Forms.Label()
        Me.txtSchool = New System.Windows.Forms.TextBox()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.lblRemarks = New System.Windows.Forms.Label()
        Me.lblDateTo = New System.Windows.Forms.Label()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.dtpDateTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpDateFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblMajor = New System.Windows.Forms.Label()
        Me.lblCourse = New System.Windows.Forms.Label()
        Me.lblDegree = New System.Windows.Forms.Label()
        Me.lblEducType = New System.Windows.Forms.Label()
        Me.txtMajor = New System.Windows.Forms.TextBox()
        Me.txtCourse = New System.Windows.Forms.TextBox()
        Me.txtDegree = New System.Windows.Forms.TextBox()
        Me.dgvEducBgs = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.EducationType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.School = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Degree = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Course = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Major = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateFrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateTo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Remarks = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip9.SuspendLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSalary.SuspendLayout()
        CType(Me.dgvEducBgs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip9
        '
        Me.ToolStrip9.AutoSize = False
        Me.ToolStrip9.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip9.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip9.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnClose, Me.btnSave, Me.btnDelete, Me.btnCancel, Me.ToolStripSeparator3, Me.btnUserActivity})
        Me.ToolStrip9.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip9.Name = "ToolStrip9"
        Me.ToolStrip9.Size = New System.Drawing.Size(856, 25)
        Me.ToolStrip9.TabIndex = 64
        Me.ToolStrip9.Text = "ToolStrip9"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(51, 22)
        Me.btnNew.Text = "&New"
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
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(51, 22)
        Me.btnSave.Text = "&Save"
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(60, 22)
        Me.btnDelete.Text = "&Delete"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'btnUserActivity
        '
        Me.btnUserActivity.Image = CType(resources.GetObject("btnUserActivity.Image"), System.Drawing.Image)
        Me.btnUserActivity.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUserActivity.Name = "btnUserActivity"
        Me.btnUserActivity.Size = New System.Drawing.Size(93, 22)
        Me.btnUserActivity.Text = "User Activity"
        '
        'txtEmployeeID
        '
        Me.txtEmployeeID.BackColor = System.Drawing.Color.White
        Me.txtEmployeeID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeID.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmployeeID.Location = New System.Drawing.Point(108, 78)
        Me.txtEmployeeID.MaxLength = 50
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(743, 22)
        Me.txtEmployeeID.TabIndex = 354
        '
        'txtFullname
        '
        Me.txtFullname.BackColor = System.Drawing.Color.White
        Me.txtFullname.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullname.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullname.Location = New System.Drawing.Point(108, 38)
        Me.txtFullname.MaxLength = 250
        Me.txtFullname.Name = "txtFullname"
        Me.txtFullname.ReadOnly = True
        Me.txtFullname.Size = New System.Drawing.Size(668, 28)
        Me.txtFullname.TabIndex = 353
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(12, 38)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 88)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 352
        Me.pbEmployee.TabStop = False
        '
        'grpSalary
        '
        Me.grpSalary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSalary.Controls.Add(Me.Label4)
        Me.grpSalary.Controls.Add(Me.Label2)
        Me.grpSalary.Controls.Add(Me.Label1)
        Me.grpSalary.Controls.Add(Me.Label3)
        Me.grpSalary.Controls.Add(Me.lblSchool)
        Me.grpSalary.Controls.Add(Me.txtSchool)
        Me.grpSalary.Controls.Add(Me.cboType)
        Me.grpSalary.Controls.Add(Me.lblRemarks)
        Me.grpSalary.Controls.Add(Me.lblDateTo)
        Me.grpSalary.Controls.Add(Me.lblDateFrom)
        Me.grpSalary.Controls.Add(Me.txtRemarks)
        Me.grpSalary.Controls.Add(Me.dtpDateTo)
        Me.grpSalary.Controls.Add(Me.dtpDateFrom)
        Me.grpSalary.Controls.Add(Me.lblMajor)
        Me.grpSalary.Controls.Add(Me.lblCourse)
        Me.grpSalary.Controls.Add(Me.lblDegree)
        Me.grpSalary.Controls.Add(Me.lblEducType)
        Me.grpSalary.Controls.Add(Me.txtMajor)
        Me.grpSalary.Controls.Add(Me.txtCourse)
        Me.grpSalary.Controls.Add(Me.txtDegree)
        Me.grpSalary.Location = New System.Drawing.Point(11, 132)
        Me.grpSalary.Name = "grpSalary"
        Me.grpSalary.Size = New System.Drawing.Size(840, 167)
        Me.grpSalary.TabIndex = 355
        Me.grpSalary.TabStop = False
        Me.grpSalary.Text = "Educational Background"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(532, 56)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(13, 13)
        Me.Label4.TabIndex = 512
        Me.Label4.Text = "*"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(532, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(13, 13)
        Me.Label2.TabIndex = 511
        Me.Label2.Text = "*"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(173, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 510
        Me.Label1.Text = "*"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(173, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 509
        Me.Label3.Text = "*"
        '
        'lblSchool
        '
        Me.lblSchool.AutoSize = True
        Me.lblSchool.Location = New System.Drawing.Point(78, 58)
        Me.lblSchool.Name = "lblSchool"
        Me.lblSchool.Size = New System.Drawing.Size(40, 13)
        Me.lblSchool.TabIndex = 17
        Me.lblSchool.Text = "School"
        '
        'txtSchool
        '
        Me.txtSchool.Location = New System.Drawing.Point(192, 55)
        Me.txtSchool.Name = "txtSchool"
        Me.txtSchool.Size = New System.Drawing.Size(190, 20)
        Me.txtSchool.TabIndex = 16
        '
        'cboType
        '
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.Items.AddRange(New Object() {"College", "High School", "Elementary", "Certification"})
        Me.cboType.Location = New System.Drawing.Point(192, 28)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(190, 21)
        Me.cboType.TabIndex = 15
        '
        'lblRemarks
        '
        Me.lblRemarks.AutoSize = True
        Me.lblRemarks.Location = New System.Drawing.Point(470, 84)
        Me.lblRemarks.Name = "lblRemarks"
        Me.lblRemarks.Size = New System.Drawing.Size(49, 13)
        Me.lblRemarks.TabIndex = 14
        Me.lblRemarks.Text = "Remarks"
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(470, 57)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(46, 13)
        Me.lblDateTo.TabIndex = 13
        Me.lblDateTo.Text = "Date To"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(470, 34)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(56, 13)
        Me.lblDateFrom.TabIndex = 12
        Me.lblDateFrom.Text = "Date From"
        '
        'txtRemarks
        '
        Me.txtRemarks.Location = New System.Drawing.Point(551, 81)
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.Size = New System.Drawing.Size(190, 46)
        Me.txtRemarks.TabIndex = 11
        '
        'dtpDateTo
        '
        Me.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDateTo.Location = New System.Drawing.Point(551, 55)
        Me.dtpDateTo.Name = "dtpDateTo"
        Me.dtpDateTo.Size = New System.Drawing.Size(190, 20)
        Me.dtpDateTo.TabIndex = 10
        '
        'dtpDateFrom
        '
        Me.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDateFrom.Location = New System.Drawing.Point(551, 29)
        Me.dtpDateFrom.Name = "dtpDateFrom"
        Me.dtpDateFrom.Size = New System.Drawing.Size(190, 20)
        Me.dtpDateFrom.TabIndex = 9
        '
        'lblMajor
        '
        Me.lblMajor.AutoSize = True
        Me.lblMajor.Location = New System.Drawing.Point(78, 136)
        Me.lblMajor.Name = "lblMajor"
        Me.lblMajor.Size = New System.Drawing.Size(33, 13)
        Me.lblMajor.TabIndex = 8
        Me.lblMajor.Text = "Major"
        '
        'lblCourse
        '
        Me.lblCourse.AutoSize = True
        Me.lblCourse.Location = New System.Drawing.Point(78, 110)
        Me.lblCourse.Name = "lblCourse"
        Me.lblCourse.Size = New System.Drawing.Size(40, 13)
        Me.lblCourse.TabIndex = 7
        Me.lblCourse.Text = "Course"
        '
        'lblDegree
        '
        Me.lblDegree.AutoSize = True
        Me.lblDegree.Location = New System.Drawing.Point(78, 84)
        Me.lblDegree.Name = "lblDegree"
        Me.lblDegree.Size = New System.Drawing.Size(42, 13)
        Me.lblDegree.TabIndex = 6
        Me.lblDegree.Text = "Degree"
        '
        'lblEducType
        '
        Me.lblEducType.AutoSize = True
        Me.lblEducType.Location = New System.Drawing.Point(78, 31)
        Me.lblEducType.Name = "lblEducType"
        Me.lblEducType.Size = New System.Drawing.Size(82, 13)
        Me.lblEducType.TabIndex = 5
        Me.lblEducType.Text = "Education Type"
        '
        'txtMajor
        '
        Me.txtMajor.Location = New System.Drawing.Point(192, 133)
        Me.txtMajor.Name = "txtMajor"
        Me.txtMajor.Size = New System.Drawing.Size(190, 20)
        Me.txtMajor.TabIndex = 4
        '
        'txtCourse
        '
        Me.txtCourse.Location = New System.Drawing.Point(192, 107)
        Me.txtCourse.Name = "txtCourse"
        Me.txtCourse.Size = New System.Drawing.Size(190, 20)
        Me.txtCourse.TabIndex = 3
        '
        'txtDegree
        '
        Me.txtDegree.Location = New System.Drawing.Point(192, 81)
        Me.txtDegree.Name = "txtDegree"
        Me.txtDegree.Size = New System.Drawing.Size(190, 20)
        Me.txtDegree.TabIndex = 2
        '
        'dgvEducBgs
        '
        Me.dgvEducBgs.AllowUserToAddRows = False
        Me.dgvEducBgs.AllowUserToDeleteRows = False
        Me.dgvEducBgs.AllowUserToResizeRows = False
        Me.dgvEducBgs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEducBgs.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvEducBgs.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEducBgs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEducBgs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.EducationType, Me.School, Me.Degree, Me.Course, Me.Major, Me.DateFrom, Me.DateTo, Me.Remarks})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEducBgs.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvEducBgs.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEducBgs.Location = New System.Drawing.Point(11, 305)
        Me.dgvEducBgs.MultiSelect = False
        Me.dgvEducBgs.Name = "dgvEducBgs"
        Me.dgvEducBgs.ReadOnly = True
        Me.dgvEducBgs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvEducBgs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEducBgs.Size = New System.Drawing.Size(840, 244)
        Me.dgvEducBgs.TabIndex = 356
        '
        'EducationType
        '
        Me.EducationType.DataPropertyName = "Type"
        Me.EducationType.HeaderText = "Education Type"
        Me.EducationType.Name = "EducationType"
        Me.EducationType.ReadOnly = True
        '
        'School
        '
        Me.School.DataPropertyName = "School"
        Me.School.HeaderText = "School"
        Me.School.Name = "School"
        Me.School.ReadOnly = True
        '
        'Degree
        '
        Me.Degree.DataPropertyName = "Degree"
        Me.Degree.HeaderText = "Degree"
        Me.Degree.Name = "Degree"
        Me.Degree.ReadOnly = True
        '
        'Course
        '
        Me.Course.DataPropertyName = "Course"
        Me.Course.HeaderText = "Course"
        Me.Course.Name = "Course"
        Me.Course.ReadOnly = True
        '
        'Major
        '
        Me.Major.DataPropertyName = "Major"
        Me.Major.HeaderText = "Major"
        Me.Major.Name = "Major"
        Me.Major.ReadOnly = True
        '
        'DateFrom
        '
        Me.DateFrom.DataPropertyName = "DateFrom"
        Me.DateFrom.HeaderText = "Date From"
        Me.DateFrom.Name = "DateFrom"
        Me.DateFrom.ReadOnly = True
        '
        'DateTo
        '
        Me.DateTo.DataPropertyName = "DateTo"
        Me.DateTo.HeaderText = "Date To"
        Me.DateTo.Name = "DateTo"
        Me.DateTo.ReadOnly = True
        '
        'Remarks
        '
        Me.Remarks.DataPropertyName = "Remarks"
        Me.Remarks.HeaderText = "Remarks"
        Me.Remarks.Name = "Remarks"
        Me.Remarks.ReadOnly = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Education Type"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "School"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Degree"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "Course"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "Major"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Date From"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Date To"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "Remarks"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        '
        'EducationalBackgroundTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.dgvEducBgs)
        Me.Controls.Add(Me.grpSalary)
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullname)
        Me.Controls.Add(Me.pbEmployee)
        Me.Controls.Add(Me.ToolStrip9)
        Me.Name = "EducationalBackgroundTab"
        Me.Size = New System.Drawing.Size(856, 552)
        Me.ToolStrip9.ResumeLayout(False)
        Me.ToolStrip9.PerformLayout()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSalary.ResumeLayout(False)
        Me.grpSalary.PerformLayout()
        CType(Me.dgvEducBgs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip9 As ToolStrip
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents btnClose As ToolStripButton
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents btnSave As ToolStripButton
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents btnCancel As ToolStripButton
    Friend WithEvents btnUserActivity As ToolStripButton
    Friend WithEvents txtEmployeeID As TextBox
    Public WithEvents txtFullname As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents grpSalary As GroupBox
    Friend WithEvents dgvEducBgs As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents lblMajor As Label
    Friend WithEvents lblCourse As Label
    Friend WithEvents lblDegree As Label
    Friend WithEvents lblEducType As Label
    Friend WithEvents txtMajor As TextBox
    Friend WithEvents txtCourse As TextBox
    Friend WithEvents txtDegree As TextBox
    Friend WithEvents dtpDateTo As DateTimePicker
    Friend WithEvents dtpDateFrom As DateTimePicker
    Friend WithEvents txtRemarks As TextBox
    Friend WithEvents lblRemarks As Label
    Friend WithEvents lblDateTo As Label
    Friend WithEvents lblDateFrom As Label
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents cboType As ComboBox
    Friend WithEvents lblSchool As Label
    Friend WithEvents txtSchool As TextBox
    Friend WithEvents EducationType As DataGridViewTextBoxColumn
    Friend WithEvents School As DataGridViewTextBoxColumn
    Friend WithEvents Degree As DataGridViewTextBoxColumn
    Friend WithEvents Course As DataGridViewTextBoxColumn
    Friend WithEvents Major As DataGridViewTextBoxColumn
    Friend WithEvents DateFrom As DataGridViewTextBoxColumn
    Friend WithEvents DateTo As DataGridViewTextBoxColumn
    Friend WithEvents Remarks As DataGridViewTextBoxColumn
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
End Class
