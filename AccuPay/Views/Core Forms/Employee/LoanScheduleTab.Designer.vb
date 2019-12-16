<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LoanScheduleTab
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip12 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel5 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton24 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton25 = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnImportLoans = New System.Windows.Forms.ToolStripButton()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.chkboxChargeToBonus = New System.Windows.Forms.CheckBox()
        Me.Label333 = New System.Windows.Forms.Label()
        Me.Label332 = New System.Windows.Forms.Label()
        Me.txtloaninterest = New System.Windows.Forms.TextBox()
        Me.Label314 = New System.Windows.Forms.Label()
        Me.Label231 = New System.Windows.Forms.Label()
        Me.rdbamount = New System.Windows.Forms.RadioButton()
        Me.rdbpercent = New System.Windows.Forms.RadioButton()
        Me.txtnoofpayperleft = New System.Windows.Forms.TextBox()
        Me.Label230 = New System.Windows.Forms.Label()
        Me.pbEmpPicLoan = New System.Windows.Forms.PictureBox()
        Me.Label221 = New System.Windows.Forms.Label()
        Me.Label220 = New System.Windows.Forms.Label()
        Me.dgvLoanList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.txtFNameLoan = New System.Windows.Forms.TextBox()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.txtEmpIDLoan = New System.Windows.Forms.TextBox()
        Me.Label100 = New System.Windows.Forms.Label()
        Me.lnklblloantype = New System.Windows.Forms.LinkLabel()
        Me.txtloannumber = New System.Windows.Forms.TextBox()
        Me.cboloantype = New System.Windows.Forms.ComboBox()
        Me.Label99 = New System.Windows.Forms.Label()
        Me.Label185 = New System.Windows.Forms.Label()
        Me.datefrom = New System.Windows.Forms.DateTimePicker()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.txtloanamt = New System.Windows.Forms.TextBox()
        Me.lblAdd = New System.Windows.Forms.LinkLabel()
        Me.Label98 = New System.Windows.Forms.Label()
        Me.cmbdedsched = New System.Windows.Forms.ComboBox()
        Me.txtbal = New System.Windows.Forms.TextBox()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.Label97 = New System.Windows.Forms.Label()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.txtdedpercent = New System.Windows.Forms.TextBox()
        Me.txtdedamt = New System.Windows.Forms.TextBox()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.Label96 = New System.Windows.Forms.Label()
        Me.dateto = New System.Windows.Forms.DateTimePicker()
        Me.txtnoofpayper = New System.Windows.Forms.TextBox()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.Label95 = New System.Windows.Forms.Label()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.Label349 = New System.Windows.Forms.Label()
        Me.Label350 = New System.Windows.Forms.Label()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn17 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn18 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_LoanNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_TotalLoanAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_TotalBalanceLeft = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_DeductionAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_DeductionPercentage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_DeductionSchedule = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_NoOfPayPeriod = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Comments = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip12.SuspendLayout()
        Me.Panel10.SuspendLayout()
        CType(Me.pbEmpPicLoan, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvLoanList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip12
        '
        Me.ToolStrip12.BackColor = System.Drawing.Color.White
        Me.ToolStrip12.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip12.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.ToolStripLabel5, Me.ToolStripSeparator9, Me.btnDelete, Me.ToolStripSeparator10, Me.btnCancel, Me.ToolStripButton24, Me.ToolStripButton25, Me.tsbtnImportLoans})
        Me.ToolStrip12.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip12.Name = "ToolStrip12"
        Me.ToolStrip12.Size = New System.Drawing.Size(1031, 25)
        Me.ToolStrip12.TabIndex = 328
        Me.ToolStrip12.Text = "ToolStrip12"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(115, 22)
        Me.btnNew.Text = "&New Loan Sched"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(115, 22)
        Me.btnSave.Text = "&Save Loan Sched"
        '
        'ToolStripLabel5
        '
        Me.ToolStripLabel5.AutoSize = False
        Me.ToolStripLabel5.Name = "ToolStripLabel5"
        Me.ToolStripLabel5.Size = New System.Drawing.Size(50, 22)
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
        '
        'btnDelete
        '
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(124, 22)
        Me.btnDelete.Text = "&Delete Loan Sched"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 25)
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "Cancel"
        '
        'ToolStripButton24
        '
        Me.ToolStripButton24.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton24.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton24.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton24.Name = "ToolStripButton24"
        Me.ToolStripButton24.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton24.Text = "Close"
        '
        'ToolStripButton25
        '
        Me.ToolStripButton25.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton25.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton25.Image = Global.AccuPay.My.Resources.Resources.audit_trail_icon
        Me.ToolStripButton25.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton25.Name = "ToolStripButton25"
        Me.ToolStripButton25.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton25.Text = "ToolStripButton1"
        Me.ToolStripButton25.ToolTipText = "Show audit trails"
        '
        'tsbtnImportLoans
        '
        Me.tsbtnImportLoans.Image = Global.AccuPay.My.Resources.Resources.Add
        Me.tsbtnImportLoans.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnImportLoans.Name = "tsbtnImportLoans"
        Me.tsbtnImportLoans.Size = New System.Drawing.Size(97, 22)
        Me.tsbtnImportLoans.Text = "Import Loans"
        Me.tsbtnImportLoans.ToolTipText = "Import loans"
        '
        'Panel10
        '
        Me.Panel10.AutoScroll = True
        Me.Panel10.BackColor = System.Drawing.Color.White
        Me.Panel10.Controls.Add(Me.chkboxChargeToBonus)
        Me.Panel10.Controls.Add(Me.Label333)
        Me.Panel10.Controls.Add(Me.Label332)
        Me.Panel10.Controls.Add(Me.txtloaninterest)
        Me.Panel10.Controls.Add(Me.Label314)
        Me.Panel10.Controls.Add(Me.Label231)
        Me.Panel10.Controls.Add(Me.rdbamount)
        Me.Panel10.Controls.Add(Me.rdbpercent)
        Me.Panel10.Controls.Add(Me.txtnoofpayperleft)
        Me.Panel10.Controls.Add(Me.Label230)
        Me.Panel10.Controls.Add(Me.pbEmpPicLoan)
        Me.Panel10.Controls.Add(Me.Label221)
        Me.Panel10.Controls.Add(Me.Label220)
        Me.Panel10.Controls.Add(Me.dgvLoanList)
        Me.Panel10.Controls.Add(Me.txtFNameLoan)
        Me.Panel10.Controls.Add(Me.TextBox7)
        Me.Panel10.Controls.Add(Me.txtEmpIDLoan)
        Me.Panel10.Controls.Add(Me.Label100)
        Me.Panel10.Controls.Add(Me.lnklblloantype)
        Me.Panel10.Controls.Add(Me.txtloannumber)
        Me.Panel10.Controls.Add(Me.cboloantype)
        Me.Panel10.Controls.Add(Me.Label99)
        Me.Panel10.Controls.Add(Me.Label185)
        Me.Panel10.Controls.Add(Me.datefrom)
        Me.Panel10.Controls.Add(Me.Label88)
        Me.Panel10.Controls.Add(Me.txtloanamt)
        Me.Panel10.Controls.Add(Me.lblAdd)
        Me.Panel10.Controls.Add(Me.Label98)
        Me.Panel10.Controls.Add(Me.cmbdedsched)
        Me.Panel10.Controls.Add(Me.txtbal)
        Me.Panel10.Controls.Add(Me.Label89)
        Me.Panel10.Controls.Add(Me.Label97)
        Me.Panel10.Controls.Add(Me.Label90)
        Me.Panel10.Controls.Add(Me.cmbStatus)
        Me.Panel10.Controls.Add(Me.txtdedpercent)
        Me.Panel10.Controls.Add(Me.txtdedamt)
        Me.Panel10.Controls.Add(Me.Label91)
        Me.Panel10.Controls.Add(Me.Label96)
        Me.Panel10.Controls.Add(Me.dateto)
        Me.Panel10.Controls.Add(Me.txtnoofpayper)
        Me.Panel10.Controls.Add(Me.Label92)
        Me.Panel10.Controls.Add(Me.Label95)
        Me.Panel10.Controls.Add(Me.Label93)
        Me.Panel10.Controls.Add(Me.TextBox6)
        Me.Panel10.Controls.Add(Me.Label94)
        Me.Panel10.Controls.Add(Me.Label349)
        Me.Panel10.Controls.Add(Me.Label350)
        Me.Panel10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel10.Location = New System.Drawing.Point(0, 25)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(1031, 494)
        Me.Panel10.TabIndex = 386
        '
        'chkboxChargeToBonus
        '
        Me.chkboxChargeToBonus.AutoSize = True
        Me.chkboxChargeToBonus.Location = New System.Drawing.Point(522, 265)
        Me.chkboxChargeToBonus.Name = "chkboxChargeToBonus"
        Me.chkboxChargeToBonus.Size = New System.Drawing.Size(96, 17)
        Me.chkboxChargeToBonus.TabIndex = 365
        Me.chkboxChargeToBonus.Text = "Apply in Bonus"
        Me.chkboxChargeToBonus.UseVisualStyleBackColor = True
        '
        'Label333
        '
        Me.Label333.AutoSize = True
        Me.Label333.ForeColor = System.Drawing.Color.White
        Me.Label333.Location = New System.Drawing.Point(862, 608)
        Me.Label333.Name = "Label333"
        Me.Label333.Size = New System.Drawing.Size(25, 13)
        Me.Label333.TabIndex = 505
        Me.Label333.Text = "___"
        '
        'Label332
        '
        Me.Label332.AutoSize = True
        Me.Label332.ForeColor = System.Drawing.Color.White
        Me.Label332.Location = New System.Drawing.Point(29, 624)
        Me.Label332.Name = "Label332"
        Me.Label332.Size = New System.Drawing.Size(25, 13)
        Me.Label332.TabIndex = 504
        Me.Label332.Text = "___"
        '
        'txtloaninterest
        '
        Me.txtloaninterest.BackColor = System.Drawing.Color.White
        Me.txtloaninterest.Location = New System.Drawing.Point(273, 260)
        Me.txtloaninterest.Name = "txtloaninterest"
        Me.txtloaninterest.ShortcutsEnabled = False
        Me.txtloaninterest.Size = New System.Drawing.Size(203, 20)
        Me.txtloaninterest.TabIndex = 362
        '
        'Label314
        '
        Me.Label314.AutoSize = True
        Me.Label314.Location = New System.Drawing.Point(272, 245)
        Me.Label314.Name = "Label314"
        Me.Label314.Size = New System.Drawing.Size(125, 13)
        Me.Label314.TabIndex = 391
        Me.Label314.Text = "Loan interest percentage"
        '
        'Label231
        '
        Me.Label231.AutoSize = True
        Me.Label231.Location = New System.Drawing.Point(14, 227)
        Me.Label231.Name = "Label231"
        Me.Label231.Size = New System.Drawing.Size(14, 13)
        Me.Label231.TabIndex = 389
        Me.Label231.Text = "₱"
        '
        'rdbamount
        '
        Me.rdbamount.AutoSize = True
        Me.rdbamount.Location = New System.Drawing.Point(273, 65)
        Me.rdbamount.Name = "rdbamount"
        Me.rdbamount.Size = New System.Drawing.Size(216, 17)
        Me.rdbamount.TabIndex = 358
        Me.rdbamount.Text = "Deduct a specific amount per pay period"
        Me.rdbamount.UseVisualStyleBackColor = True
        Me.rdbamount.Visible = False
        '
        'rdbpercent
        '
        Me.rdbpercent.AutoSize = True
        Me.rdbpercent.Checked = True
        Me.rdbpercent.Location = New System.Drawing.Point(761, 81)
        Me.rdbpercent.Name = "rdbpercent"
        Me.rdbpercent.Size = New System.Drawing.Size(246, 17)
        Me.rdbpercent.TabIndex = 357
        Me.rdbpercent.TabStop = True
        Me.rdbpercent.Text = "Deduct a percent of the net pay per pay period"
        Me.rdbpercent.UseVisualStyleBackColor = True
        Me.rdbpercent.Visible = False
        '
        'txtnoofpayperleft
        '
        Me.txtnoofpayperleft.BackColor = System.Drawing.Color.White
        Me.txtnoofpayperleft.Location = New System.Drawing.Point(273, 142)
        Me.txtnoofpayperleft.Name = "txtnoofpayperleft"
        Me.txtnoofpayperleft.ReadOnly = True
        Me.txtnoofpayperleft.Size = New System.Drawing.Size(203, 20)
        Me.txtnoofpayperleft.TabIndex = 359
        '
        'Label230
        '
        Me.Label230.AutoSize = True
        Me.Label230.Location = New System.Drawing.Point(272, 127)
        Me.Label230.Name = "Label230"
        Me.Label230.Size = New System.Drawing.Size(107, 13)
        Me.Label230.TabIndex = 386
        Me.Label230.Text = "No. of Pay Period left"
        '
        'pbEmpPicLoan
        '
        Me.pbEmpPicLoan.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPicLoan.Location = New System.Drawing.Point(11, 7)
        Me.pbEmpPicLoan.Name = "pbEmpPicLoan"
        Me.pbEmpPicLoan.Size = New System.Drawing.Size(89, 77)
        Me.pbEmpPicLoan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPicLoan.TabIndex = 382
        Me.pbEmpPicLoan.TabStop = False
        '
        'Label221
        '
        Me.Label221.AutoSize = True
        Me.Label221.Location = New System.Drawing.Point(256, 185)
        Me.Label221.Name = "Label221"
        Me.Label221.Size = New System.Drawing.Size(14, 13)
        Me.Label221.TabIndex = 384
        Me.Label221.Text = "₱"
        '
        'Label220
        '
        Me.Label220.AutoSize = True
        Me.Label220.Location = New System.Drawing.Point(12, 185)
        Me.Label220.Name = "Label220"
        Me.Label220.Size = New System.Drawing.Size(14, 13)
        Me.Label220.TabIndex = 383
        Me.Label220.Text = "₱"
        '
        'dgvLoanList
        '
        Me.dgvLoanList.AllowUserToAddRows = False
        Me.dgvLoanList.AllowUserToDeleteRows = False
        Me.dgvLoanList.AllowUserToOrderColumns = True
        Me.dgvLoanList.AllowUserToResizeRows = False
        Me.dgvLoanList.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvLoanList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvLoanList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLoanList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_LoanNumber, Me.c_TotalLoanAmount, Me.c_TotalBalanceLeft, Me.c_DeductionAmount, Me.c_DeductionPercentage, Me.c_DeductionSchedule, Me.c_NoOfPayPeriod, Me.c_Comments, Me.c_RowID, Me.c_Status})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvLoanList.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvLoanList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvLoanList.Location = New System.Drawing.Point(32, 287)
        Me.dgvLoanList.MultiSelect = False
        Me.dgvLoanList.Name = "dgvLoanList"
        Me.dgvLoanList.ReadOnly = True
        Me.dgvLoanList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvLoanList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLoanList.Size = New System.Drawing.Size(824, 334)
        Me.dgvLoanList.TabIndex = 366
        '
        'txtFNameLoan
        '
        Me.txtFNameLoan.BackColor = System.Drawing.Color.White
        Me.txtFNameLoan.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFNameLoan.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFNameLoan.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFNameLoan.Location = New System.Drawing.Point(127, 22)
        Me.txtFNameLoan.MaxLength = 250
        Me.txtFNameLoan.Name = "txtFNameLoan"
        Me.txtFNameLoan.ReadOnly = True
        Me.txtFNameLoan.Size = New System.Drawing.Size(668, 28)
        Me.txtFNameLoan.TabIndex = 381
        '
        'TextBox7
        '
        Me.TextBox7.Enabled = False
        Me.TextBox7.Location = New System.Drawing.Point(731, 260)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(203, 20)
        Me.TextBox7.TabIndex = 352
        Me.TextBox7.Visible = False
        '
        'txtEmpIDLoan
        '
        Me.txtEmpIDLoan.BackColor = System.Drawing.Color.White
        Me.txtEmpIDLoan.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpIDLoan.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmpIDLoan.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpIDLoan.Location = New System.Drawing.Point(127, 49)
        Me.txtEmpIDLoan.MaxLength = 50
        Me.txtEmpIDLoan.Name = "txtEmpIDLoan"
        Me.txtEmpIDLoan.ReadOnly = True
        Me.txtEmpIDLoan.Size = New System.Drawing.Size(516, 22)
        Me.txtEmpIDLoan.TabIndex = 380
        '
        'Label100
        '
        Me.Label100.AutoSize = True
        Me.Label100.BackColor = System.Drawing.Color.Transparent
        Me.Label100.Location = New System.Drawing.Point(728, 245)
        Me.Label100.Name = "Label100"
        Me.Label100.Size = New System.Drawing.Size(67, 13)
        Me.Label100.TabIndex = 354
        Me.Label100.Text = "Employee ID"
        Me.Label100.Visible = False
        '
        'lnklblloantype
        '
        Me.lnklblloantype.AutoSize = True
        Me.lnklblloantype.Location = New System.Drawing.Point(241, 111)
        Me.lnklblloantype.Name = "lnklblloantype"
        Me.lnklblloantype.Size = New System.Drawing.Size(26, 13)
        Me.lnklblloantype.TabIndex = 354
        Me.lnklblloantype.TabStop = True
        Me.lnklblloantype.Text = "Add"
        Me.lnklblloantype.Visible = False
        '
        'txtloannumber
        '
        Me.txtloannumber.Enabled = False
        Me.txtloannumber.Location = New System.Drawing.Point(32, 142)
        Me.txtloannumber.Name = "txtloannumber"
        Me.txtloannumber.Size = New System.Drawing.Size(203, 20)
        Me.txtloannumber.TabIndex = 354
        '
        'cboloantype
        '
        Me.cboloantype.FormattingEnabled = True
        Me.cboloantype.Location = New System.Drawing.Point(32, 103)
        Me.cboloantype.Name = "cboloantype"
        Me.cboloantype.Size = New System.Drawing.Size(204, 21)
        Me.cboloantype.TabIndex = 353
        '
        'Label99
        '
        Me.Label99.AutoSize = True
        Me.Label99.Location = New System.Drawing.Point(29, 127)
        Me.Label99.Name = "Label99"
        Me.Label99.Size = New System.Drawing.Size(71, 13)
        Me.Label99.TabIndex = 357
        Me.Label99.Text = "Loan Number"
        '
        'Label185
        '
        Me.Label185.AutoSize = True
        Me.Label185.Location = New System.Drawing.Point(30, 88)
        Me.Label185.Name = "Label185"
        Me.Label185.Size = New System.Drawing.Size(66, 13)
        Me.Label185.TabIndex = 379
        Me.Label185.Text = "Type of loan"
        '
        'datefrom
        '
        Me.datefrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.datefrom.Location = New System.Drawing.Point(32, 260)
        Me.datefrom.Name = "datefrom"
        Me.datefrom.Size = New System.Drawing.Size(203, 20)
        Me.datefrom.TabIndex = 357
        '
        'Label88
        '
        Me.Label88.AutoSize = True
        Me.Label88.Location = New System.Drawing.Point(103, 7)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(67, 13)
        Me.Label88.TabIndex = 377
        Me.Label88.Text = "Employee ID"
        Me.Label88.Visible = False
        '
        'txtloanamt
        '
        Me.txtloanamt.Location = New System.Drawing.Point(32, 181)
        Me.txtloanamt.Name = "txtloanamt"
        Me.txtloanamt.ShortcutsEnabled = False
        Me.txtloanamt.Size = New System.Drawing.Size(203, 20)
        Me.txtloanamt.TabIndex = 355
        '
        'lblAdd
        '
        Me.lblAdd.AutoSize = True
        Me.lblAdd.Location = New System.Drawing.Point(731, 210)
        Me.lblAdd.Name = "lblAdd"
        Me.lblAdd.Size = New System.Drawing.Size(26, 13)
        Me.lblAdd.TabIndex = 358
        Me.lblAdd.TabStop = True
        Me.lblAdd.Text = "Add"
        Me.lblAdd.Visible = False
        '
        'Label98
        '
        Me.Label98.AutoSize = True
        Me.Label98.Location = New System.Drawing.Point(29, 166)
        Me.Label98.Name = "Label98"
        Me.Label98.Size = New System.Drawing.Size(97, 13)
        Me.Label98.TabIndex = 362
        Me.Label98.Text = "Total Loan Amount"
        '
        'cmbdedsched
        '
        Me.cmbdedsched.FormattingEnabled = True
        Me.cmbdedsched.Location = New System.Drawing.Point(522, 238)
        Me.cmbdedsched.Name = "cmbdedsched"
        Me.cmbdedsched.Size = New System.Drawing.Size(204, 21)
        Me.cmbdedsched.TabIndex = 364
        '
        'txtbal
        '
        Me.txtbal.Enabled = False
        Me.txtbal.Location = New System.Drawing.Point(32, 220)
        Me.txtbal.Name = "txtbal"
        Me.txtbal.Size = New System.Drawing.Size(203, 20)
        Me.txtbal.TabIndex = 356
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Location = New System.Drawing.Point(519, 222)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(104, 13)
        Me.Label89.TabIndex = 375
        Me.Label89.Text = "Deduction Schedule"
        '
        'Label97
        '
        Me.Label97.AutoSize = True
        Me.Label97.Location = New System.Drawing.Point(29, 205)
        Me.Label97.Name = "Label97"
        Me.Label97.Size = New System.Drawing.Size(94, 13)
        Me.Label97.TabIndex = 365
        Me.Label97.Text = "Total Balance Left"
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Location = New System.Drawing.Point(758, 104)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(114, 13)
        Me.Label90.TabIndex = 374
        Me.Label90.Text = "Deduction Percentage"
        Me.Label90.Visible = False
        '
        'cmbStatus
        '
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Location = New System.Drawing.Point(273, 220)
        Me.cmbStatus.MaxLength = 50
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(204, 21)
        Me.cmbStatus.TabIndex = 361
        '
        'txtdedpercent
        '
        Me.txtdedpercent.Location = New System.Drawing.Point(761, 119)
        Me.txtdedpercent.Name = "txtdedpercent"
        Me.txtdedpercent.Size = New System.Drawing.Size(203, 20)
        Me.txtdedpercent.TabIndex = 359
        Me.txtdedpercent.Visible = False
        '
        'txtdedamt
        '
        Me.txtdedamt.Location = New System.Drawing.Point(274, 181)
        Me.txtdedamt.Name = "txtdedamt"
        Me.txtdedamt.ShortcutsEnabled = False
        Me.txtdedamt.Size = New System.Drawing.Size(203, 20)
        Me.txtdedamt.TabIndex = 360
        '
        'Label91
        '
        Me.Label91.AutoSize = True
        Me.Label91.Location = New System.Drawing.Point(758, 142)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(46, 13)
        Me.Label91.TabIndex = 373
        Me.Label91.Text = "Date To"
        Me.Label91.Visible = False
        '
        'Label96
        '
        Me.Label96.AutoSize = True
        Me.Label96.Location = New System.Drawing.Point(272, 167)
        Me.Label96.Name = "Label96"
        Me.Label96.Size = New System.Drawing.Size(95, 13)
        Me.Label96.TabIndex = 368
        Me.Label96.Text = "Deduction Amount"
        '
        'dateto
        '
        Me.dateto.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dateto.Location = New System.Drawing.Point(761, 158)
        Me.dateto.Name = "dateto"
        Me.dateto.Size = New System.Drawing.Size(203, 20)
        Me.dateto.TabIndex = 365
        Me.dateto.Visible = False
        '
        'txtnoofpayper
        '
        Me.txtnoofpayper.Location = New System.Drawing.Point(273, 103)
        Me.txtnoofpayper.Name = "txtnoofpayper"
        Me.txtnoofpayper.ShortcutsEnabled = False
        Me.txtnoofpayper.Size = New System.Drawing.Size(203, 20)
        Me.txtnoofpayper.TabIndex = 358
        '
        'Label92
        '
        Me.Label92.AutoSize = True
        Me.Label92.Location = New System.Drawing.Point(29, 244)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(56, 13)
        Me.Label92.TabIndex = 372
        Me.Label92.Text = "Date From"
        '
        'Label95
        '
        Me.Label95.AutoSize = True
        Me.Label95.Location = New System.Drawing.Point(270, 88)
        Me.Label95.Name = "Label95"
        Me.Label95.Size = New System.Drawing.Size(90, 13)
        Me.Label95.TabIndex = 369
        Me.Label95.Text = "No. of Pay Period"
        '
        'Label93
        '
        Me.Label93.AutoSize = True
        Me.Label93.Location = New System.Drawing.Point(272, 205)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(37, 13)
        Me.Label93.TabIndex = 371
        Me.Label93.Text = "Status"
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(522, 103)
        Me.TextBox6.MaxLength = 2000
        Me.TextBox6.Multiline = True
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox6.Size = New System.Drawing.Size(203, 116)
        Me.TextBox6.TabIndex = 363
        '
        'Label94
        '
        Me.Label94.AutoSize = True
        Me.Label94.Location = New System.Drawing.Point(519, 88)
        Me.Label94.Name = "Label94"
        Me.Label94.Size = New System.Drawing.Size(49, 13)
        Me.Label94.TabIndex = 370
        Me.Label94.Text = "Remarks"
        '
        'Label349
        '
        Me.Label349.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label349.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label349.Location = New System.Drawing.Point(618, 217)
        Me.Label349.Name = "Label349"
        Me.Label349.Size = New System.Drawing.Size(13, 13)
        Me.Label349.TabIndex = 506
        Me.Label349.Text = "*"
        '
        'Label350
        '
        Me.Label350.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label350.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label350.Location = New System.Drawing.Point(91, 83)
        Me.Label350.Name = "Label350"
        Me.Label350.Size = New System.Drawing.Size(13, 13)
        Me.Label350.TabIndex = 507
        Me.Label350.Text = "*"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Loan Number"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Total Loan Amount"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Total Balance Left"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "Deduction Amount"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "Deduction Percentage"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Deduction Schedule"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "No of pay period"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "No of pay period left"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "Deduction date from"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "Remarks"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.HeaderText = "RowiD"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.ReadOnly = True
        Me.DataGridViewTextBoxColumn11.Visible = False
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.HeaderText = "Status"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.ReadOnly = True
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.HeaderText = "Loan type"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        Me.DataGridViewTextBoxColumn13.ReadOnly = True
        '
        'DataGridViewTextBoxColumn14
        '
        Me.DataGridViewTextBoxColumn14.HeaderText = "LoanHasBonus"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        Me.DataGridViewTextBoxColumn14.ReadOnly = True
        Me.DataGridViewTextBoxColumn14.Visible = False
        '
        'DataGridViewTextBoxColumn15
        '
        Me.DataGridViewTextBoxColumn15.HeaderText = "Column14"
        Me.DataGridViewTextBoxColumn15.Name = "DataGridViewTextBoxColumn15"
        '
        'DataGridViewTextBoxColumn16
        '
        Me.DataGridViewTextBoxColumn16.HeaderText = "Column15"
        Me.DataGridViewTextBoxColumn16.Name = "DataGridViewTextBoxColumn16"
        '
        'DataGridViewTextBoxColumn17
        '
        Me.DataGridViewTextBoxColumn17.HeaderText = "Column16"
        Me.DataGridViewTextBoxColumn17.Name = "DataGridViewTextBoxColumn17"
        '
        'DataGridViewTextBoxColumn18
        '
        Me.DataGridViewTextBoxColumn18.HeaderText = "Column17"
        Me.DataGridViewTextBoxColumn18.Name = "DataGridViewTextBoxColumn18"
        '
        'c_LoanNumber
        '
        Me.c_LoanNumber.DataPropertyName = "LoanNumber"
        Me.c_LoanNumber.HeaderText = "LoanNumber"
        Me.c_LoanNumber.Name = "c_LoanNumber"
        Me.c_LoanNumber.ReadOnly = True
        '
        'c_TotalLoanAmount
        '
        Me.c_TotalLoanAmount.DataPropertyName = "TotalLoanAmount"
        Me.c_TotalLoanAmount.HeaderText = "Total Loan Amount"
        Me.c_TotalLoanAmount.Name = "c_TotalLoanAmount"
        Me.c_TotalLoanAmount.ReadOnly = True
        '
        'c_TotalBalanceLeft
        '
        Me.c_TotalBalanceLeft.DataPropertyName = "TotalBalanceLeft"
        Me.c_TotalBalanceLeft.HeaderText = "Total Balance Left"
        Me.c_TotalBalanceLeft.Name = "c_TotalBalanceLeft"
        Me.c_TotalBalanceLeft.ReadOnly = True
        '
        'c_DeductionAmount
        '
        Me.c_DeductionAmount.DataPropertyName = "DeductionAmount"
        Me.c_DeductionAmount.HeaderText = "Deduction Amount"
        Me.c_DeductionAmount.Name = "c_DeductionAmount"
        Me.c_DeductionAmount.ReadOnly = True
        '
        'c_DeductionPercentage
        '
        Me.c_DeductionPercentage.DataPropertyName = "DeductionPercentage"
        Me.c_DeductionPercentage.HeaderText = "Deduction Percentage"
        Me.c_DeductionPercentage.Name = "c_DeductionPercentage"
        Me.c_DeductionPercentage.ReadOnly = True
        '
        'c_DeductionSchedule
        '
        Me.c_DeductionSchedule.DataPropertyName = "DeductionSchedule"
        Me.c_DeductionSchedule.HeaderText = "Deduction Schedule"
        Me.c_DeductionSchedule.Name = "c_DeductionSchedule"
        Me.c_DeductionSchedule.ReadOnly = True
        '
        'c_NoOfPayPeriod
        '
        Me.c_NoOfPayPeriod.DataPropertyName = "NoOfPayPeriod"
        Me.c_NoOfPayPeriod.HeaderText = "No Of Pay Period"
        Me.c_NoOfPayPeriod.Name = "c_NoOfPayPeriod"
        Me.c_NoOfPayPeriod.ReadOnly = True
        '
        'c_Comments
        '
        Me.c_Comments.DataPropertyName = "Comments"
        Me.c_Comments.HeaderText = "Remarks"
        Me.c_Comments.Name = "c_Comments"
        Me.c_Comments.ReadOnly = True
        '
        'c_RowID
        '
        Me.c_RowID.DataPropertyName = "RowID"
        Me.c_RowID.HeaderText = "RowID"
        Me.c_RowID.Name = "c_RowID"
        Me.c_RowID.ReadOnly = True
        '
        'c_Status
        '
        Me.c_Status.DataPropertyName = "Status"
        Me.c_Status.HeaderText = "Status"
        Me.c_Status.Name = "c_Status"
        Me.c_Status.ReadOnly = True
        '
        'LoanScheduleTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel10)
        Me.Controls.Add(Me.ToolStrip12)
        Me.Name = "LoanScheduleTab"
        Me.Size = New System.Drawing.Size(1031, 519)
        Me.ToolStrip12.ResumeLayout(False)
        Me.ToolStrip12.PerformLayout()
        Me.Panel10.ResumeLayout(False)
        Me.Panel10.PerformLayout()
        CType(Me.pbEmpPicLoan, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvLoanList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip12 As ToolStrip
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents btnSave As ToolStripButton
    Friend WithEvents ToolStripLabel5 As ToolStripLabel
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents btnCancel As ToolStripButton
    Friend WithEvents ToolStripButton24 As ToolStripButton
    Friend WithEvents ToolStripButton25 As ToolStripButton
    Friend WithEvents tsbtnImportLoans As ToolStripButton
    Friend WithEvents Panel10 As Panel
    Friend WithEvents chkboxChargeToBonus As CheckBox
    Friend WithEvents Label333 As Label
    Friend WithEvents Label332 As Label
    Friend WithEvents txtloaninterest As TextBox
    Friend WithEvents Label314 As Label
    Friend WithEvents Label231 As Label
    Friend WithEvents rdbamount As RadioButton
    Friend WithEvents rdbpercent As RadioButton
    Friend WithEvents txtnoofpayperleft As TextBox
    Friend WithEvents Label230 As Label
    Friend WithEvents pbEmpPicLoan As PictureBox
    Friend WithEvents Label221 As Label
    Friend WithEvents Label220 As Label
    Friend WithEvents dgvLoanList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents txtFNameLoan As TextBox
    Friend WithEvents TextBox7 As TextBox
    Friend WithEvents txtEmpIDLoan As TextBox
    Friend WithEvents Label100 As Label
    Friend WithEvents lnklblloantype As LinkLabel
    Friend WithEvents txtloannumber As TextBox
    Friend WithEvents cboloantype As ComboBox
    Friend WithEvents Label99 As Label
    Friend WithEvents Label185 As Label
    Friend WithEvents datefrom As DateTimePicker
    Friend WithEvents Label88 As Label
    Friend WithEvents txtloanamt As TextBox
    Friend WithEvents lblAdd As LinkLabel
    Friend WithEvents Label98 As Label
    Friend WithEvents cmbdedsched As ComboBox
    Friend WithEvents txtbal As TextBox
    Friend WithEvents Label89 As Label
    Friend WithEvents Label97 As Label
    Friend WithEvents Label90 As Label
    Friend WithEvents cmbStatus As ComboBox
    Friend WithEvents txtdedpercent As TextBox
    Friend WithEvents txtdedamt As TextBox
    Friend WithEvents Label91 As Label
    Friend WithEvents Label96 As Label
    Friend WithEvents dateto As DateTimePicker
    Friend WithEvents txtnoofpayper As TextBox
    Friend WithEvents Label92 As Label
    Friend WithEvents Label95 As Label
    Friend WithEvents Label93 As Label
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents Label94 As Label
    Friend WithEvents Label349 As Label
    Friend WithEvents Label350 As Label
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As DataGridViewTextBoxColumn
    Friend WithEvents c_LoanNumber As DataGridViewTextBoxColumn
    Friend WithEvents c_TotalLoanAmount As DataGridViewTextBoxColumn
    Friend WithEvents c_TotalBalanceLeft As DataGridViewTextBoxColumn
    Friend WithEvents c_DeductionAmount As DataGridViewTextBoxColumn
    Friend WithEvents c_DeductionPercentage As DataGridViewTextBoxColumn
    Friend WithEvents c_DeductionSchedule As DataGridViewTextBoxColumn
    Friend WithEvents c_NoOfPayPeriod As DataGridViewTextBoxColumn
    Friend WithEvents c_Comments As DataGridViewTextBoxColumn
    Friend WithEvents c_RowID As DataGridViewTextBoxColumn
    Friend WithEvents c_Status As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn15 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn16 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn17 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn18 As DataGridViewTextBoxColumn
End Class
