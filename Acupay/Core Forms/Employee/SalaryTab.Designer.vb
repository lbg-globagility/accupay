<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SalaryTab
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SalaryTab))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip5 = New System.Windows.Forms.ToolStrip()
        Me.btnNewSal = New System.Windows.Forms.ToolStripButton()
        Me.btnSaveSal = New System.Windows.Forms.ToolStripButton()
        Me.btnDelSal = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton30 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton31 = New System.Windows.Forms.ToolStripButton()
        Me.btnCancelSal = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnImportSalary = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripProgressBar2 = New System.Windows.Forms.ToolStripProgressBar()
        Me.pbEmpPicSal = New System.Windows.Forms.PictureBox()
        Me.grpbasicsalaryaddeduction = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtEmp_type = New System.Windows.Forms.TextBox()
        Me.dtpToSal = New System.Windows.Forms.DateTimePicker()
        Me.dptFromSal = New System.Windows.Forms.DateTimePicker()
        Me.txtpaytype = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtToComputeSal = New System.Windows.Forms.TextBox()
        Me.txtEmpDeclaSal = New System.Windows.Forms.TextBox()
        Me.txtBasicrateSal = New System.Windows.Forms.TextBox()
        Me.Label213 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtSSSSal = New System.Windows.Forms.TextBox()
        Me.txtPagibig = New System.Windows.Forms.TextBox()
        Me.txtPhilHealthSal = New System.Windows.Forms.TextBox()
        Me.Label217 = New System.Windows.Forms.Label()
        Me.Label216 = New System.Windows.Forms.Label()
        Me.Label215 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtTrueSal = New System.Windows.Forms.TextBox()
        Me.Label347 = New System.Windows.Forms.Label()
        Me.txtFNameSal = New System.Windows.Forms.TextBox()
        Me.txtEmpIDSal = New System.Windows.Forms.TextBox()
        Me.dgvemployeesalary = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_empID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_empName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_PayType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_maritalStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_noofdepd = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_filingstatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_BasicPaySal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_EmpSal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_BasicDailyPaySal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_BasicHourlyPaySal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_pagibig = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_philhealth = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_sss = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_fromdate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_todate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_RowIDSal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_TrueSal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_ToComputeSal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TrueHDMFAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip5.SuspendLayout()
        CType(Me.pbEmpPicSal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpbasicsalaryaddeduction.SuspendLayout()
        CType(Me.dgvemployeesalary, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip5
        '
        Me.ToolStrip5.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip5.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip5.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNewSal, Me.btnSaveSal, Me.btnDelSal, Me.ToolStripButton30, Me.ToolStripButton31, Me.btnCancelSal, Me.tsbtnImportSalary, Me.ToolStripProgressBar2})
        Me.ToolStrip5.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip5.Name = "ToolStrip5"
        Me.ToolStrip5.Size = New System.Drawing.Size(856, 25)
        Me.ToolStrip5.TabIndex = 2
        Me.ToolStrip5.Text = "ToolStrip5"
        '
        'btnNewSal
        '
        Me.btnNewSal.Image = Global.Acupay.My.Resources.Resources._new
        Me.btnNewSal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNewSal.Name = "btnNewSal"
        Me.btnNewSal.Size = New System.Drawing.Size(140, 22)
        Me.btnNewSal.Text = "&New Employee Salary"
        '
        'btnSaveSal
        '
        Me.btnSaveSal.Image = Global.Acupay.My.Resources.Resources.Save
        Me.btnSaveSal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSaveSal.Name = "btnSaveSal"
        Me.btnSaveSal.Size = New System.Drawing.Size(143, 22)
        Me.btnSaveSal.Text = "&Save Employee Salary "
        '
        'btnDelSal
        '
        Me.btnDelSal.Image = Global.Acupay.My.Resources.Resources.CLOSE_00
        Me.btnDelSal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelSal.Name = "btnDelSal"
        Me.btnDelSal.Size = New System.Drawing.Size(149, 22)
        Me.btnDelSal.Text = "Delete Employee Salary"
        '
        'ToolStripButton30
        '
        Me.ToolStripButton30.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton30.Image = Global.Acupay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton30.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton30.Name = "ToolStripButton30"
        Me.ToolStripButton30.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton30.Text = "Close"
        '
        'ToolStripButton31
        '
        Me.ToolStripButton31.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton31.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton31.Image = Global.Acupay.My.Resources.Resources.audit_trail_icon
        Me.ToolStripButton31.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton31.Name = "ToolStripButton31"
        Me.ToolStripButton31.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton31.Text = "ToolStripButton1"
        Me.ToolStripButton31.ToolTipText = "Show audit trails"
        '
        'btnCancelSal
        '
        Me.btnCancelSal.Image = Global.Acupay.My.Resources.Resources.cancel1
        Me.btnCancelSal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancelSal.Name = "btnCancelSal"
        Me.btnCancelSal.Size = New System.Drawing.Size(63, 22)
        Me.btnCancelSal.Text = "Cancel"
        '
        'tsbtnImportSalary
        '
        Me.tsbtnImportSalary.Image = CType(resources.GetObject("tsbtnImportSalary.Image"), System.Drawing.Image)
        Me.tsbtnImportSalary.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnImportSalary.Name = "tsbtnImportSalary"
        Me.tsbtnImportSalary.Size = New System.Drawing.Size(97, 22)
        Me.tsbtnImportSalary.Text = "Import Salary"
        '
        'ToolStripProgressBar2
        '
        Me.ToolStripProgressBar2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripProgressBar2.Name = "ToolStripProgressBar2"
        Me.ToolStripProgressBar2.Size = New System.Drawing.Size(100, 22)
        Me.ToolStripProgressBar2.Visible = False
        '
        'pbEmpPicSal
        '
        Me.pbEmpPicSal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPicSal.Location = New System.Drawing.Point(22, 48)
        Me.pbEmpPicSal.Name = "pbEmpPicSal"
        Me.pbEmpPicSal.Size = New System.Drawing.Size(89, 77)
        Me.pbEmpPicSal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPicSal.TabIndex = 182
        Me.pbEmpPicSal.TabStop = False
        '
        'grpbasicsalaryaddeduction
        '
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtTrueSal)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label347)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label10)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label14)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtSSSSal)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtPagibig)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtPhilHealthSal)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label217)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label216)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label215)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label13)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label213)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtToComputeSal)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtEmpDeclaSal)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtBasicrateSal)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label12)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label11)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label9)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label8)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label7)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label6)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label5)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtEmp_type)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.dtpToSal)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.dptFromSal)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtpaytype)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label4)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label3)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label2)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label1)
        Me.grpbasicsalaryaddeduction.Location = New System.Drawing.Point(22, 131)
        Me.grpbasicsalaryaddeduction.Name = "grpbasicsalaryaddeduction"
        Me.grpbasicsalaryaddeduction.Size = New System.Drawing.Size(811, 174)
        Me.grpbasicsalaryaddeduction.TabIndex = 183
        Me.grpbasicsalaryaddeduction.TabStop = False
        Me.grpbasicsalaryaddeduction.Text = "Employee Salary"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Pay Type:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Employee Type:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 95)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Effective Date From:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 121)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(94, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Effective Date To:"
        '
        'txtEmp_type
        '
        Me.txtEmp_type.BackColor = System.Drawing.Color.White
        Me.txtEmp_type.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmp_type.Location = New System.Drawing.Point(133, 63)
        Me.txtEmp_type.Name = "txtEmp_type"
        Me.txtEmp_type.ReadOnly = True
        Me.txtEmp_type.Size = New System.Drawing.Size(162, 20)
        Me.txtEmp_type.TabIndex = 60
        '
        'dtpToSal
        '
        Me.dtpToSal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToSal.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpToSal.Location = New System.Drawing.Point(133, 115)
        Me.dtpToSal.Name = "dtpToSal"
        Me.dtpToSal.Size = New System.Drawing.Size(162, 20)
        Me.dtpToSal.TabIndex = 58
        '
        'dptFromSal
        '
        Me.dptFromSal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dptFromSal.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dptFromSal.Location = New System.Drawing.Point(133, 89)
        Me.dptFromSal.Name = "dptFromSal"
        Me.dptFromSal.Size = New System.Drawing.Size(162, 20)
        Me.dptFromSal.TabIndex = 57
        '
        'txtpaytype
        '
        Me.txtpaytype.BackColor = System.Drawing.Color.White
        Me.txtpaytype.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtpaytype.Location = New System.Drawing.Point(133, 38)
        Me.txtpaytype.Name = "txtpaytype"
        Me.txtpaytype.ReadOnly = True
        Me.txtpaytype.Size = New System.Drawing.Size(162, 20)
        Me.txtpaytype.TabIndex = 59
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(303, 40)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 13)
        Me.Label5.TabIndex = 61
        Me.Label5.Text = "Declared Salary:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(303, 68)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(72, 13)
        Me.Label6.TabIndex = 62
        Me.Label6.Text = "Actual Salary:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(303, 94)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 13)
        Me.Label7.TabIndex = 63
        Me.Label7.Text = "Basic Pay:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(584, 40)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(58, 13)
        Me.Label8.TabIndex = 64
        Me.Label8.Text = "PhilHealth:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(584, 66)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(31, 13)
        Me.Label9.TabIndex = 65
        Me.Label9.Text = "SSS:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(584, 94)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 13)
        Me.Label11.TabIndex = 109
        Me.Label11.Text = "PAGIBIG:"
        '
        'txtToComputeSal
        '
        Me.txtToComputeSal.Location = New System.Drawing.Point(422, 64)
        Me.txtToComputeSal.Name = "txtToComputeSal"
        Me.txtToComputeSal.Size = New System.Drawing.Size(145, 20)
        Me.txtToComputeSal.TabIndex = 112
        '
        'txtEmpDeclaSal
        '
        Me.txtEmpDeclaSal.Location = New System.Drawing.Point(422, 38)
        Me.txtEmpDeclaSal.MaxLength = 12
        Me.txtEmpDeclaSal.Name = "txtEmpDeclaSal"
        Me.txtEmpDeclaSal.ShortcutsEnabled = False
        Me.txtEmpDeclaSal.Size = New System.Drawing.Size(145, 20)
        Me.txtEmpDeclaSal.TabIndex = 111
        '
        'txtBasicrateSal
        '
        Me.txtBasicrateSal.BackColor = System.Drawing.Color.White
        Me.txtBasicrateSal.Location = New System.Drawing.Point(422, 90)
        Me.txtBasicrateSal.MaxLength = 12
        Me.txtBasicrateSal.Name = "txtBasicrateSal"
        Me.txtBasicrateSal.ReadOnly = True
        Me.txtBasicrateSal.ShortcutsEnabled = False
        Me.txtBasicrateSal.Size = New System.Drawing.Size(145, 20)
        Me.txtBasicrateSal.TabIndex = 113
        '
        'Label213
        '
        Me.Label213.AutoSize = True
        Me.Label213.Location = New System.Drawing.Point(405, 40)
        Me.Label213.Name = "Label213"
        Me.Label213.Size = New System.Drawing.Size(14, 13)
        Me.Label213.TabIndex = 270
        Me.Label213.Text = "₱"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(405, 95)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(14, 13)
        Me.Label13.TabIndex = 271
        Me.Label13.Text = "₱"
        '
        'txtSSSSal
        '
        Me.txtSSSSal.BackColor = System.Drawing.Color.White
        Me.txtSSSSal.Location = New System.Drawing.Point(674, 64)
        Me.txtSSSSal.Name = "txtSSSSal"
        Me.txtSSSSal.Size = New System.Drawing.Size(131, 20)
        Me.txtSSSSal.TabIndex = 273
        '
        'txtPagibig
        '
        Me.txtPagibig.BackColor = System.Drawing.Color.White
        Me.txtPagibig.Location = New System.Drawing.Point(674, 90)
        Me.txtPagibig.Name = "txtPagibig"
        Me.txtPagibig.Size = New System.Drawing.Size(131, 20)
        Me.txtPagibig.TabIndex = 274
        '
        'txtPhilHealthSal
        '
        Me.txtPhilHealthSal.BackColor = System.Drawing.Color.White
        Me.txtPhilHealthSal.Location = New System.Drawing.Point(674, 38)
        Me.txtPhilHealthSal.Name = "txtPhilHealthSal"
        Me.txtPhilHealthSal.Size = New System.Drawing.Size(131, 20)
        Me.txtPhilHealthSal.TabIndex = 272
        '
        'Label217
        '
        Me.Label217.AutoSize = True
        Me.Label217.Location = New System.Drawing.Point(651, 93)
        Me.Label217.Name = "Label217"
        Me.Label217.Size = New System.Drawing.Size(14, 13)
        Me.Label217.TabIndex = 275
        Me.Label217.Text = "₱"
        '
        'Label216
        '
        Me.Label216.AutoSize = True
        Me.Label216.Location = New System.Drawing.Point(651, 67)
        Me.Label216.Name = "Label216"
        Me.Label216.Size = New System.Drawing.Size(14, 13)
        Me.Label216.TabIndex = 276
        Me.Label216.Text = "₱"
        '
        'Label215
        '
        Me.Label215.AutoSize = True
        Me.Label215.Location = New System.Drawing.Point(651, 41)
        Me.Label215.Name = "Label215"
        Me.Label215.Size = New System.Drawing.Size(14, 13)
        Me.Label215.TabIndex = 277
        Me.Label215.Text = "₱"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(385, 32)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(18, 24)
        Me.Label12.TabIndex = 110
        Me.Label12.Text = "*"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(118, 91)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(18, 24)
        Me.Label14.TabIndex = 278
        Me.Label14.Text = "*"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(108, 115)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(18, 24)
        Me.Label10.TabIndex = 279
        Me.Label10.Text = "*"
        '
        'txtTrueSal
        '
        Me.txtTrueSal.BackColor = System.Drawing.Color.White
        Me.txtTrueSal.Location = New System.Drawing.Point(461, 130)
        Me.txtTrueSal.MaxLength = 12
        Me.txtTrueSal.Name = "txtTrueSal"
        Me.txtTrueSal.ReadOnly = True
        Me.txtTrueSal.ShortcutsEnabled = False
        Me.txtTrueSal.Size = New System.Drawing.Size(176, 20)
        Me.txtTrueSal.TabIndex = 280
        Me.txtTrueSal.Visible = False
        '
        'Label347
        '
        Me.Label347.AutoSize = True
        Me.Label347.Location = New System.Drawing.Point(443, 133)
        Me.Label347.Name = "Label347"
        Me.Label347.Size = New System.Drawing.Size(14, 13)
        Me.Label347.TabIndex = 281
        Me.Label347.Text = "₱"
        Me.Label347.Visible = False
        '
        'txtFNameSal
        '
        Me.txtFNameSal.BackColor = System.Drawing.Color.White
        Me.txtFNameSal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFNameSal.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFNameSal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFNameSal.Location = New System.Drawing.Point(144, 58)
        Me.txtFNameSal.MaxLength = 250
        Me.txtFNameSal.Name = "txtFNameSal"
        Me.txtFNameSal.ReadOnly = True
        Me.txtFNameSal.Size = New System.Drawing.Size(668, 28)
        Me.txtFNameSal.TabIndex = 344
        '
        'txtEmpIDSal
        '
        Me.txtEmpIDSal.BackColor = System.Drawing.Color.White
        Me.txtEmpIDSal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpIDSal.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmpIDSal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpIDSal.Location = New System.Drawing.Point(143, 92)
        Me.txtEmpIDSal.MaxLength = 50
        Me.txtEmpIDSal.Name = "txtEmpIDSal"
        Me.txtEmpIDSal.ReadOnly = True
        Me.txtEmpIDSal.Size = New System.Drawing.Size(516, 22)
        Me.txtEmpIDSal.TabIndex = 345
        '
        'dgvemployeesalary
        '
        Me.dgvemployeesalary.AllowUserToAddRows = False
        Me.dgvemployeesalary.AllowUserToDeleteRows = False
        Me.dgvemployeesalary.AllowUserToResizeRows = False
        Me.dgvemployeesalary.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvemployeesalary.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvemployeesalary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvemployeesalary.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_empID, Me.c_empName, Me.c_PayType, Me.c_maritalStatus, Me.c_noofdepd, Me.c_filingstatus, Me.c_BasicPaySal, Me.c_EmpSal, Me.c_BasicDailyPaySal, Me.c_BasicHourlyPaySal, Me.c_pagibig, Me.c_philhealth, Me.c_sss, Me.c_fromdate, Me.c_todate, Me.c_RowIDSal, Me.c_TrueSal, Me.c_ToComputeSal, Me.TrueHDMFAmount})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvemployeesalary.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvemployeesalary.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvemployeesalary.Location = New System.Drawing.Point(11, 311)
        Me.dgvemployeesalary.MultiSelect = False
        Me.dgvemployeesalary.Name = "dgvemployeesalary"
        Me.dgvemployeesalary.ReadOnly = True
        Me.dgvemployeesalary.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvemployeesalary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvemployeesalary.Size = New System.Drawing.Size(832, 227)
        Me.dgvemployeesalary.TabIndex = 346
        '
        'c_empID
        '
        Me.c_empID.HeaderText = "Employee ID"
        Me.c_empID.Name = "c_empID"
        Me.c_empID.ReadOnly = True
        Me.c_empID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.c_empID.Visible = False
        '
        'c_empName
        '
        Me.c_empName.HeaderText = "Employee Name"
        Me.c_empName.Name = "c_empName"
        Me.c_empName.ReadOnly = True
        Me.c_empName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.c_empName.Visible = False
        '
        'c_PayType
        '
        Me.c_PayType.HeaderText = "Pay Type"
        Me.c_PayType.Name = "c_PayType"
        Me.c_PayType.ReadOnly = True
        Me.c_PayType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.c_PayType.Visible = False
        '
        'c_maritalStatus
        '
        Me.c_maritalStatus.HeaderText = "Marital Status"
        Me.c_maritalStatus.Name = "c_maritalStatus"
        Me.c_maritalStatus.ReadOnly = True
        Me.c_maritalStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_noofdepd
        '
        Me.c_noofdepd.HeaderText = "No Of Dependent"
        Me.c_noofdepd.Name = "c_noofdepd"
        Me.c_noofdepd.ReadOnly = True
        Me.c_noofdepd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_filingstatus
        '
        Me.c_filingstatus.HeaderText = "Filing Status"
        Me.c_filingstatus.Name = "c_filingstatus"
        Me.c_filingstatus.ReadOnly = True
        Me.c_filingstatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.c_filingstatus.Visible = False
        '
        'c_BasicPaySal
        '
        Me.c_BasicPaySal.HeaderText = "Basic Pay"
        Me.c_BasicPaySal.Name = "c_BasicPaySal"
        Me.c_BasicPaySal.ReadOnly = True
        Me.c_BasicPaySal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_EmpSal
        '
        Me.c_EmpSal.HeaderText = "Salary"
        Me.c_EmpSal.Name = "c_EmpSal"
        Me.c_EmpSal.ReadOnly = True
        Me.c_EmpSal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_BasicDailyPaySal
        '
        Me.c_BasicDailyPaySal.HeaderText = "Daily pay"
        Me.c_BasicDailyPaySal.Name = "c_BasicDailyPaySal"
        Me.c_BasicDailyPaySal.ReadOnly = True
        Me.c_BasicDailyPaySal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_BasicHourlyPaySal
        '
        Me.c_BasicHourlyPaySal.HeaderText = "Hourly pay"
        Me.c_BasicHourlyPaySal.Name = "c_BasicHourlyPaySal"
        Me.c_BasicHourlyPaySal.ReadOnly = True
        Me.c_BasicHourlyPaySal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_pagibig
        '
        Me.c_pagibig.HeaderText = "Pag-IBIG"
        Me.c_pagibig.Name = "c_pagibig"
        Me.c_pagibig.ReadOnly = True
        Me.c_pagibig.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_philhealth
        '
        Me.c_philhealth.HeaderText = "PhilHealth"
        Me.c_philhealth.Name = "c_philhealth"
        Me.c_philhealth.ReadOnly = True
        Me.c_philhealth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_sss
        '
        Me.c_sss.HeaderText = "SSS"
        Me.c_sss.Name = "c_sss"
        Me.c_sss.ReadOnly = True
        Me.c_sss.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_fromdate
        '
        Me.c_fromdate.HeaderText = "Effectivity Date From"
        Me.c_fromdate.Name = "c_fromdate"
        Me.c_fromdate.ReadOnly = True
        Me.c_fromdate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_todate
        '
        Me.c_todate.HeaderText = "Effectivity Date To"
        Me.c_todate.Name = "c_todate"
        Me.c_todate.ReadOnly = True
        Me.c_todate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'c_RowIDSal
        '
        Me.c_RowIDSal.HeaderText = "RowID"
        Me.c_RowIDSal.Name = "c_RowIDSal"
        Me.c_RowIDSal.ReadOnly = True
        Me.c_RowIDSal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.c_RowIDSal.Visible = False
        '
        'c_TrueSal
        '
        Me.c_TrueSal.HeaderText = "True Salary"
        Me.c_TrueSal.Name = "c_TrueSal"
        Me.c_TrueSal.ReadOnly = True
        Me.c_TrueSal.Visible = False
        '
        'c_ToComputeSal
        '
        Me.c_ToComputeSal.HeaderText = "DifferenceBetweenSal&TrueSal"
        Me.c_ToComputeSal.Name = "c_ToComputeSal"
        Me.c_ToComputeSal.ReadOnly = True
        Me.c_ToComputeSal.Visible = False
        '
        'TrueHDMFAmount
        '
        Me.TrueHDMFAmount.HeaderText = "TrueHDMFAmount"
        Me.TrueHDMFAmount.Name = "TrueHDMFAmount"
        Me.TrueHDMFAmount.ReadOnly = True
        Me.TrueHDMFAmount.Visible = False
        '
        'SalaryTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.dgvemployeesalary)
        Me.Controls.Add(Me.txtEmpIDSal)
        Me.Controls.Add(Me.txtFNameSal)
        Me.Controls.Add(Me.grpbasicsalaryaddeduction)
        Me.Controls.Add(Me.pbEmpPicSal)
        Me.Controls.Add(Me.ToolStrip5)
        Me.Name = "SalaryTab"
        Me.Size = New System.Drawing.Size(856, 552)
        Me.ToolStrip5.ResumeLayout(False)
        Me.ToolStrip5.PerformLayout()
        CType(Me.pbEmpPicSal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpbasicsalaryaddeduction.ResumeLayout(False)
        Me.grpbasicsalaryaddeduction.PerformLayout()
        CType(Me.dgvemployeesalary, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip5 As ToolStrip
    Friend WithEvents btnSaveSal As ToolStripButton
    Friend WithEvents btnCancelSal As ToolStripButton
    Friend WithEvents btnDelSal As ToolStripButton
    Friend WithEvents ToolStripButton30 As ToolStripButton
    Friend WithEvents ToolStripButton31 As ToolStripButton
    Friend WithEvents pbEmpPicSal As PictureBox
    Friend WithEvents btnNewSal As ToolStripButton
    Friend WithEvents tsbtnImportSalary As ToolStripButton
    Friend WithEvents ToolStripProgressBar2 As ToolStripProgressBar
    Friend WithEvents grpbasicsalaryaddeduction As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtEmp_type As TextBox
    Friend WithEvents dtpToSal As DateTimePicker
    Friend WithEvents dptFromSal As DateTimePicker
    Friend WithEvents txtpaytype As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents txtToComputeSal As TextBox
    Friend WithEvents txtEmpDeclaSal As TextBox
    Friend WithEvents txtBasicrateSal As TextBox
    Friend WithEvents Label213 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents txtSSSSal As TextBox
    Friend WithEvents txtPagibig As TextBox
    Friend WithEvents txtPhilHealthSal As TextBox
    Friend WithEvents Label217 As Label
    Friend WithEvents Label216 As Label
    Friend WithEvents Label215 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents txtTrueSal As TextBox
    Friend WithEvents Label347 As Label
    Public WithEvents txtFNameSal As TextBox
    Friend WithEvents txtEmpIDSal As TextBox
    Friend WithEvents dgvemployeesalary As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_empID As DataGridViewTextBoxColumn
    Friend WithEvents c_empName As DataGridViewTextBoxColumn
    Friend WithEvents c_PayType As DataGridViewTextBoxColumn
    Friend WithEvents c_maritalStatus As DataGridViewTextBoxColumn
    Friend WithEvents c_noofdepd As DataGridViewTextBoxColumn
    Friend WithEvents c_filingstatus As DataGridViewTextBoxColumn
    Friend WithEvents c_BasicPaySal As DataGridViewTextBoxColumn
    Friend WithEvents c_EmpSal As DataGridViewTextBoxColumn
    Friend WithEvents c_BasicDailyPaySal As DataGridViewTextBoxColumn
    Friend WithEvents c_BasicHourlyPaySal As DataGridViewTextBoxColumn
    Friend WithEvents c_pagibig As DataGridViewTextBoxColumn
    Friend WithEvents c_philhealth As DataGridViewTextBoxColumn
    Friend WithEvents c_sss As DataGridViewTextBoxColumn
    Friend WithEvents c_fromdate As DataGridViewTextBoxColumn
    Friend WithEvents c_todate As DataGridViewTextBoxColumn
    Friend WithEvents c_RowIDSal As DataGridViewTextBoxColumn
    Friend WithEvents c_TrueSal As DataGridViewTextBoxColumn
    Friend WithEvents c_ToComputeSal As DataGridViewTextBoxColumn
    Friend WithEvents TrueHDMFAmount As DataGridViewTextBoxColumn
End Class
