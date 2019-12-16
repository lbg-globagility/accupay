<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmployeeSalaryForm
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
        Me.dgvEmployeeList = New System.Windows.Forms.DataGridView()
        Me.c_empid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Empname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grpbasicsalaryaddeduction = New System.Windows.Forms.GroupBox()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.dptFrom = New System.Windows.Forms.DateTimePicker()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.txtpaytype = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbMaritalStatus = New System.Windows.Forms.ComboBox()
        Me.cmbFilingStatus = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtSSS = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtPagibig = New System.Windows.Forms.TextBox()
        Me.txtBasicrate = New System.Windows.Forms.TextBox()
        Me.txtEmpname = New System.Windows.Forms.TextBox()
        Me.txtEmpID = New System.Windows.Forms.TextBox()
        Me.txtPhilHealth = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.lblSaveMSg = New System.Windows.Forms.Label()
        Me.dgvemployeesalary = New System.Windows.Forms.DataGridView()
        Me.c_empIDList = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_empNameList = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_paytype = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_filingStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_MaritalStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_NoOfDependents = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_basicpay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_PagIbig = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_philHealth = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_sssno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_effecdatefrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_effecDateto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtSearchEmpName = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtSearchEmpID = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnAudittrail = New System.Windows.Forms.ToolStripButton()
        CType(Me.dgvEmployeeList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpbasicsalaryaddeduction.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvemployeesalary, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvEmployeeList
        '
        Me.dgvEmployeeList.AllowUserToAddRows = False
        Me.dgvEmployeeList.AllowUserToDeleteRows = False
        Me.dgvEmployeeList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvEmployeeList.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvEmployeeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEmployeeList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_empid, Me.c_Empname})
        Me.dgvEmployeeList.Location = New System.Drawing.Point(3, 109)
        Me.dgvEmployeeList.Name = "dgvEmployeeList"
        Me.dgvEmployeeList.ReadOnly = True
        Me.dgvEmployeeList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmployeeList.Size = New System.Drawing.Size(311, 286)
        Me.dgvEmployeeList.TabIndex = 0
        '
        'c_empid
        '
        Me.c_empid.HeaderText = "Employee ID"
        Me.c_empid.Name = "c_empid"
        Me.c_empid.ReadOnly = True
        '
        'c_Empname
        '
        Me.c_Empname.HeaderText = "Employee Name"
        Me.c_Empname.Name = "c_Empname"
        Me.c_Empname.ReadOnly = True
        Me.c_Empname.Width = 150
        '
        'grpbasicsalaryaddeduction
        '
        Me.grpbasicsalaryaddeduction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.dtpTo)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.dptFrom)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label12)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.ComboBox1)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtpaytype)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label11)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label7)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label10)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.cmbMaritalStatus)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.cmbFilingStatus)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label8)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtSSS)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label6)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtPagibig)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtBasicrate)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtEmpname)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtEmpID)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.txtPhilHealth)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label9)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label3)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label5)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label2)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label1)
        Me.grpbasicsalaryaddeduction.Controls.Add(Me.Label19)
        Me.grpbasicsalaryaddeduction.Enabled = False
        Me.grpbasicsalaryaddeduction.Location = New System.Drawing.Point(5, 41)
        Me.grpbasicsalaryaddeduction.Name = "grpbasicsalaryaddeduction"
        Me.grpbasicsalaryaddeduction.Size = New System.Drawing.Size(815, 198)
        Me.grpbasicsalaryaddeduction.TabIndex = 1
        Me.grpbasicsalaryaddeduction.TabStop = False
        Me.grpbasicsalaryaddeduction.Text = "Employee Salary"
        '
        'dtpTo
        '
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTo.Location = New System.Drawing.Point(645, 71)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(154, 20)
        Me.dtpTo.TabIndex = 54
        '
        'dptFrom
        '
        Me.dptFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dptFrom.Location = New System.Drawing.Point(645, 32)
        Me.dptFrom.Name = "dptFrom"
        Me.dptFrom.Size = New System.Drawing.Size(154, 20)
        Me.dptFrom.TabIndex = 53
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(11, 136)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(90, 13)
        Me.Label12.TabIndex = 52
        Me.Label12.Text = "No of dependent:"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"0", "1", "2", "3", "4"})
        Me.ComboBox1.Location = New System.Drawing.Point(121, 134)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(176, 21)
        Me.ComboBox1.TabIndex = 51
        '
        'txtpaytype
        '
        Me.txtpaytype.Enabled = False
        Me.txtpaytype.Location = New System.Drawing.Point(121, 81)
        Me.txtpaytype.Name = "txtpaytype"
        Me.txtpaytype.Size = New System.Drawing.Size(176, 20)
        Me.txtpaytype.TabIndex = 49
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(10, 84)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(55, 13)
        Me.Label11.TabIndex = 48
        Me.Label11.Text = "Pay Type:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 109)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(74, 13)
        Me.Label7.TabIndex = 47
        Me.Label7.Text = "Marital Status:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(11, 164)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(67, 13)
        Me.Label10.TabIndex = 46
        Me.Label10.Text = "Filing Status:"
        '
        'cmbMaritalStatus
        '
        Me.cmbMaritalStatus.FormattingEnabled = True
        Me.cmbMaritalStatus.Location = New System.Drawing.Point(121, 107)
        Me.cmbMaritalStatus.Name = "cmbMaritalStatus"
        Me.cmbMaritalStatus.Size = New System.Drawing.Size(176, 21)
        Me.cmbMaritalStatus.TabIndex = 45
        '
        'cmbFilingStatus
        '
        Me.cmbFilingStatus.FormattingEnabled = True
        Me.cmbFilingStatus.Location = New System.Drawing.Point(121, 161)
        Me.cmbFilingStatus.Name = "cmbFilingStatus"
        Me.cmbFilingStatus.Size = New System.Drawing.Size(176, 21)
        Me.cmbFilingStatus.TabIndex = 44
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(642, 55)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(94, 13)
        Me.Label8.TabIndex = 40
        Me.Label8.Text = "Effective Date To:"
        '
        'txtSSS
        '
        Me.txtSSS.Enabled = False
        Me.txtSSS.Location = New System.Drawing.Point(420, 110)
        Me.txtSSS.Name = "txtSSS"
        Me.txtSSS.Size = New System.Drawing.Size(176, 20)
        Me.txtSSS.TabIndex = 39
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(642, 13)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(104, 13)
        Me.Label6.TabIndex = 33
        Me.Label6.Text = "Effective Date From:"
        '
        'txtPagibig
        '
        Me.txtPagibig.Location = New System.Drawing.Point(420, 56)
        Me.txtPagibig.Name = "txtPagibig"
        Me.txtPagibig.Size = New System.Drawing.Size(176, 20)
        Me.txtPagibig.TabIndex = 30
        '
        'txtBasicrate
        '
        Me.txtBasicrate.Location = New System.Drawing.Point(420, 30)
        Me.txtBasicrate.Name = "txtBasicrate"
        Me.txtBasicrate.Size = New System.Drawing.Size(176, 20)
        Me.txtBasicrate.TabIndex = 29
        '
        'txtEmpname
        '
        Me.txtEmpname.Enabled = False
        Me.txtEmpname.Location = New System.Drawing.Point(121, 55)
        Me.txtEmpname.Name = "txtEmpname"
        Me.txtEmpname.Size = New System.Drawing.Size(176, 20)
        Me.txtEmpname.TabIndex = 28
        '
        'txtEmpID
        '
        Me.txtEmpID.Location = New System.Drawing.Point(121, 29)
        Me.txtEmpID.Name = "txtEmpID"
        Me.txtEmpID.Size = New System.Drawing.Size(176, 20)
        Me.txtEmpID.TabIndex = 27
        '
        'txtPhilHealth
        '
        Me.txtPhilHealth.Enabled = False
        Me.txtPhilHealth.Location = New System.Drawing.Point(420, 82)
        Me.txtPhilHealth.Name = "txtPhilHealth"
        Me.txtPhilHealth.Size = New System.Drawing.Size(176, 20)
        Me.txtPhilHealth.TabIndex = 25
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(10, 32)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(70, 13)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Employee ID:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Employee Name:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(337, 117)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(28, 13)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "SSS"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(337, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "PHIL HEALTH"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(337, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "Pag-IBIG"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(337, 33)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(56, 13)
        Me.Label19.TabIndex = 7
        Me.Label19.Text = "Basic pay:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.ToolStripLabel1, Me.ToolStripSeparator1, Me.btnDelete, Me.ToolStripSeparator2, Me.btnCancel, Me.btnClose, Me.btnAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(831, 25)
        Me.ToolStrip1.TabIndex = 12
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
        'lblSaveMSg
        '
        Me.lblSaveMSg.AutoSize = True
        Me.lblSaveMSg.Location = New System.Drawing.Point(70, 25)
        Me.lblSaveMSg.Name = "lblSaveMSg"
        Me.lblSaveMSg.Size = New System.Drawing.Size(0, 13)
        Me.lblSaveMSg.TabIndex = 53
        '
        'dgvemployeesalary
        '
        Me.dgvemployeesalary.AllowUserToAddRows = False
        Me.dgvemployeesalary.AllowUserToDeleteRows = False
        Me.dgvemployeesalary.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvemployeesalary.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvemployeesalary.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenHorizontal
        Me.dgvemployeesalary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvemployeesalary.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_empIDList, Me.c_empNameList, Me.c_paytype, Me.c_filingStatus, Me.c_MaritalStatus, Me.c_NoOfDependents, Me.c_basicpay, Me.c_PagIbig, Me.c_philHealth, Me.c_sssno, Me.c_effecdatefrom, Me.c_effecDateto, Me.c_rowid})
        Me.dgvemployeesalary.Location = New System.Drawing.Point(5, 245)
        Me.dgvemployeesalary.Name = "dgvemployeesalary"
        Me.dgvemployeesalary.ReadOnly = True
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlDark
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Silver
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvemployeesalary.RowHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvemployeesalary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvemployeesalary.Size = New System.Drawing.Size(815, 150)
        Me.dgvemployeesalary.TabIndex = 54
        '
        'c_empIDList
        '
        Me.c_empIDList.HeaderText = "Employee ID"
        Me.c_empIDList.Name = "c_empIDList"
        Me.c_empIDList.ReadOnly = True
        '
        'c_empNameList
        '
        Me.c_empNameList.HeaderText = "Employee Name"
        Me.c_empNameList.Name = "c_empNameList"
        Me.c_empNameList.ReadOnly = True
        Me.c_empNameList.Width = 150
        '
        'c_paytype
        '
        Me.c_paytype.HeaderText = "Pay Type"
        Me.c_paytype.Name = "c_paytype"
        Me.c_paytype.ReadOnly = True
        '
        'c_filingStatus
        '
        Me.c_filingStatus.HeaderText = "Filing Status"
        Me.c_filingStatus.Name = "c_filingStatus"
        Me.c_filingStatus.ReadOnly = True
        '
        'c_MaritalStatus
        '
        Me.c_MaritalStatus.HeaderText = "Marital Status"
        Me.c_MaritalStatus.Name = "c_MaritalStatus"
        Me.c_MaritalStatus.ReadOnly = True
        '
        'c_NoOfDependents
        '
        Me.c_NoOfDependents.HeaderText = "No Of Dependents"
        Me.c_NoOfDependents.Name = "c_NoOfDependents"
        Me.c_NoOfDependents.ReadOnly = True
        '
        'c_basicpay
        '
        Me.c_basicpay.HeaderText = "Basic Pay"
        Me.c_basicpay.Name = "c_basicpay"
        Me.c_basicpay.ReadOnly = True
        '
        'c_PagIbig
        '
        Me.c_PagIbig.HeaderText = "Pag-IBIG"
        Me.c_PagIbig.Name = "c_PagIbig"
        Me.c_PagIbig.ReadOnly = True
        '
        'c_philHealth
        '
        Me.c_philHealth.HeaderText = "PhilHealth"
        Me.c_philHealth.Name = "c_philHealth"
        Me.c_philHealth.ReadOnly = True
        '
        'c_sssno
        '
        Me.c_sssno.HeaderText = "SSS"
        Me.c_sssno.Name = "c_sssno"
        Me.c_sssno.ReadOnly = True
        '
        'c_effecdatefrom
        '
        Me.c_effecdatefrom.HeaderText = "Effective Date From"
        Me.c_effecdatefrom.Name = "c_effecdatefrom"
        Me.c_effecdatefrom.ReadOnly = True
        '
        'c_effecDateto
        '
        Me.c_effecDateto.HeaderText = "Effective Date To"
        Me.c_effecDateto.Name = "c_effecDateto"
        Me.c_effecDateto.ReadOnly = True
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "RowID"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.ReadOnly = True
        Me.c_rowid.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Controls.Add(Me.lblSaveMSg)
        Me.Panel1.Controls.Add(Me.dgvemployeesalary)
        Me.Panel1.Controls.Add(Me.grpbasicsalaryaddeduction)
        Me.Panel1.Location = New System.Drawing.Point(322, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(833, 400)
        Me.Panel1.TabIndex = 55
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label14)
        Me.Panel2.Controls.Add(Me.txtSearchEmpName)
        Me.Panel2.Controls.Add(Me.Label13)
        Me.Panel2.Controls.Add(Me.txtSearchEmpID)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.dgvEmployeeList)
        Me.Panel2.Location = New System.Drawing.Point(1, 25)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(321, 400)
        Me.Panel2.TabIndex = 56
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(3, 2)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(66, 20)
        Me.Label14.TabIndex = 55
        Me.Label14.Text = "Search"
        '
        'txtSearchEmpName
        '
        Me.txtSearchEmpName.Location = New System.Drawing.Point(6, 83)
        Me.txtSearchEmpName.Name = "txtSearchEmpName"
        Me.txtSearchEmpName.Size = New System.Drawing.Size(280, 20)
        Me.txtSearchEmpName.TabIndex = 54
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(3, 67)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(87, 13)
        Me.Label13.TabIndex = 53
        Me.Label13.Text = "Employee Name:"
        '
        'txtSearchEmpID
        '
        Me.txtSearchEmpID.Location = New System.Drawing.Point(6, 44)
        Me.txtSearchEmpID.Name = "txtSearchEmpID"
        Me.txtSearchEmpID.Size = New System.Drawing.Size(280, 20)
        Me.txtSearchEmpID.TabIndex = 54
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 13)
        Me.Label4.TabIndex = 53
        Me.Label4.Text = "Employee ID:"
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Silver
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Stencil", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(1155, 24)
        Me.Label16.TabIndex = 312
        Me.Label16.Text = "Employee Salary"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnNew
        '
        Me.btnNew.Image = Global.PayrollSystem.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(112, 22)
        Me.btnNew.Text = "&New Emp Salary"
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Image = Global.PayrollSystem.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(112, 22)
        Me.btnSave.Text = "&Save Emp Salary"
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = Global.PayrollSystem.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(121, 22)
        Me.btnDelete.Text = "&Delete Emp Salary"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.PayrollSystem.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.PayrollSystem.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "&Close"
        '
        'btnAudittrail
        '
        Me.btnAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnAudittrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAudittrail.Image = Global.PayrollSystem.My.Resources.Resources.audit_trail_icon
        Me.btnAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAudittrail.Name = "btnAudittrail"
        Me.btnAudittrail.Size = New System.Drawing.Size(23, 22)
        Me.btnAudittrail.Text = "ToolStripButton1"
        '
        'EmployeeSalaryForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1155, 431)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EmployeeSalaryForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Salary Form"
        CType(Me.dgvEmployeeList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpbasicsalaryaddeduction.ResumeLayout(False)
        Me.grpbasicsalaryaddeduction.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvemployeesalary, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvEmployeeList As System.Windows.Forms.DataGridView
    Friend WithEvents grpbasicsalaryaddeduction As System.Windows.Forms.GroupBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents c_empid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_Empname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtSSS As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtPagibig As System.Windows.Forms.TextBox
    Friend WithEvents txtBasicrate As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpname As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpID As System.Windows.Forms.TextBox
    Friend WithEvents txtPhilHealth As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbMaritalStatus As System.Windows.Forms.ComboBox
    Friend WithEvents cmbFilingStatus As System.Windows.Forms.ComboBox
    Friend WithEvents txtpaytype As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblSaveMSg As System.Windows.Forms.Label
    Friend WithEvents dgvemployeesalary As System.Windows.Forms.DataGridView
    Friend WithEvents c_empIDList As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_empNameList As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_paytype As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_filingStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_MaritalStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_NoOfDependents As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_basicpay As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_PagIbig As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_philHealth As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_sssno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_effecdatefrom As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_effecDateto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_rowid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtSearchEmpName As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtSearchEmpID As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dptFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label16 As System.Windows.Forms.Label
End Class
