<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AlphaListRptVwr
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AlphaListRptVwr))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.pnlEmployee = New System.Windows.Forms.Panel()
        Me.txtFName = New System.Windows.Forms.TextBox()
        Me.txtEmpID = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.txtPaidOnHealth = New System.Windows.Forms.TextBox()
        Me.txtTaxabIncomePrevEmployer = New System.Windows.Forms.TextBox()
        Me.TextBox10 = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtPrevEmployerZipCode = New System.Windows.Forms.TextBox()
        Me.txtPrevEmployerAddress = New System.Windows.Forms.TextBox()
        Me.txtPrevEmployerName = New System.Windows.Forms.TextBox()
        Me.txtPrevEmployerTIN = New System.Windows.Forms.MaskedTextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkSecondEmployer = New System.Windows.Forms.CheckBox()
        Me.chkMainEmployer = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rdbYesMinimum = New System.Windows.Forms.RadioButton()
        Me.rdbNoMinimum = New System.Windows.Forms.RadioButton()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.rdbNo = New System.Windows.Forms.RadioButton()
        Me.rdbYes = New System.Windows.Forms.RadioButton()
        Me.txtMinWageMonth = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtForTheYear = New System.Windows.Forms.TextBox()
        Me.txtMinWageDay = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnDiscard = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.AutoCompleteTextBox1 = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.crvAlphaList = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnReload = New System.Windows.Forms.ToolStripButton()
        Me.bgwEmployeeID = New System.ComponentModel.BackgroundWorker()
        Me.bgReport = New System.ComponentModel.BackgroundWorker()
        Me.bgLoadAlphaList = New System.ComponentModel.BackgroundWorker()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.pnlEmployee.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.pnlEmployee)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.crvAlphaList)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ToolStrip1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1049, 552)
        Me.SplitContainer1.SplitterDistance = 450
        Me.SplitContainer1.SplitterWidth = 6
        Me.SplitContainer1.TabIndex = 0
        '
        'pnlEmployee
        '
        Me.pnlEmployee.AutoScroll = True
        Me.pnlEmployee.BackColor = System.Drawing.Color.White
        Me.pnlEmployee.Controls.Add(Me.txtFName)
        Me.pnlEmployee.Controls.Add(Me.txtEmpID)
        Me.pnlEmployee.Controls.Add(Me.Label17)
        Me.pnlEmployee.Controls.Add(Me.GroupBox4)
        Me.pnlEmployee.Controls.Add(Me.GroupBox3)
        Me.pnlEmployee.Controls.Add(Me.GroupBox2)
        Me.pnlEmployee.Controls.Add(Me.GroupBox1)
        Me.pnlEmployee.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlEmployee.Location = New System.Drawing.Point(0, 37)
        Me.pnlEmployee.Name = "pnlEmployee"
        Me.pnlEmployee.Size = New System.Drawing.Size(450, 478)
        Me.pnlEmployee.TabIndex = 0
        '
        'txtFName
        '
        Me.txtFName.BackColor = System.Drawing.Color.White
        Me.txtFName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFName.Font = New System.Drawing.Font("Nyala", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFName.Location = New System.Drawing.Point(12, 6)
        Me.txtFName.MaxLength = 250
        Me.txtFName.Name = "txtFName"
        Me.txtFName.ReadOnly = True
        Me.txtFName.Size = New System.Drawing.Size(516, 26)
        Me.txtFName.TabIndex = 100
        '
        'txtEmpID
        '
        Me.txtEmpID.BackColor = System.Drawing.Color.White
        Me.txtEmpID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpID.Font = New System.Drawing.Font("Nyala", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmpID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpID.Location = New System.Drawing.Point(12, 33)
        Me.txtEmpID.MaxLength = 50
        Me.txtEmpID.Name = "txtEmpID"
        Me.txtEmpID.ReadOnly = True
        Me.txtEmpID.Size = New System.Drawing.Size(668, 20)
        Me.txtEmpID.TabIndex = 101
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(9, 1039)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(45, 13)
        Me.Label17.TabIndex = 4
        Me.Label17.Text = "Label17"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtPaidOnHealth)
        Me.GroupBox4.Controls.Add(Me.txtTaxabIncomePrevEmployer)
        Me.GroupBox4.Controls.Add(Me.TextBox10)
        Me.GroupBox4.Controls.Add(Me.Label16)
        Me.GroupBox4.Controls.Add(Me.Label15)
        Me.GroupBox4.Controls.Add(Me.Label14)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 713)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(418, 160)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Part IVA   Summary"
        '
        'txtPaidOnHealth
        '
        Me.txtPaidOnHealth.Location = New System.Drawing.Point(100, 82)
        Me.txtPaidOnHealth.Name = "txtPaidOnHealth"
        Me.txtPaidOnHealth.Size = New System.Drawing.Size(100, 20)
        Me.txtPaidOnHealth.TabIndex = 5
        '
        'txtTaxabIncomePrevEmployer
        '
        Me.txtTaxabIncomePrevEmployer.Location = New System.Drawing.Point(100, 38)
        Me.txtTaxabIncomePrevEmployer.Name = "txtTaxabIncomePrevEmployer"
        Me.txtTaxabIncomePrevEmployer.Size = New System.Drawing.Size(100, 20)
        Me.txtTaxabIncomePrevEmployer.TabIndex = 4
        '
        'TextBox10
        '
        Me.TextBox10.Location = New System.Drawing.Point(100, 126)
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New System.Drawing.Size(299, 20)
        Me.TextBox10.TabIndex = 6
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(10, 110)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(116, 13)
        Me.Label16.TabIndex = 21
        Me.Label16.Text = "30B Previous Employer"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(10, 66)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(357, 13)
        Me.Label15.TabIndex = 20
        Me.Label15.Text = "27 Less: Premium Paid on Health, and/or Hospital Insurance (if applicable)"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(10, 22)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(306, 13)
        Me.Label14.TabIndex = 19
        Me.Label14.Text = "24 Add: Taxable Compensation Income from Previous Employer"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtPrevEmployerZipCode)
        Me.GroupBox3.Controls.Add(Me.txtPrevEmployerAddress)
        Me.GroupBox3.Controls.Add(Me.txtPrevEmployerName)
        Me.GroupBox3.Controls.Add(Me.txtPrevEmployerTIN)
        Me.GroupBox3.Controls.Add(Me.Label13)
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 477)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(418, 230)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Part III   Employer Information (Previous)"
        '
        'txtPrevEmployerZipCode
        '
        Me.txtPrevEmployerZipCode.Location = New System.Drawing.Point(100, 204)
        Me.txtPrevEmployerZipCode.Name = "txtPrevEmployerZipCode"
        Me.txtPrevEmployerZipCode.Size = New System.Drawing.Size(100, 20)
        Me.txtPrevEmployerZipCode.TabIndex = 3
        '
        'txtPrevEmployerAddress
        '
        Me.txtPrevEmployerAddress.Location = New System.Drawing.Point(100, 126)
        Me.txtPrevEmployerAddress.Multiline = True
        Me.txtPrevEmployerAddress.Name = "txtPrevEmployerAddress"
        Me.txtPrevEmployerAddress.Size = New System.Drawing.Size(299, 54)
        Me.txtPrevEmployerAddress.TabIndex = 2
        '
        'txtPrevEmployerName
        '
        Me.txtPrevEmployerName.Location = New System.Drawing.Point(100, 82)
        Me.txtPrevEmployerName.Name = "txtPrevEmployerName"
        Me.txtPrevEmployerName.Size = New System.Drawing.Size(299, 20)
        Me.txtPrevEmployerName.TabIndex = 1
        '
        'txtPrevEmployerTIN
        '
        Me.txtPrevEmployerTIN.Location = New System.Drawing.Point(100, 38)
        Me.txtPrevEmployerTIN.Mask = "000-000-000-0000"
        Me.txtPrevEmployerTIN.Name = "txtPrevEmployerTIN"
        Me.txtPrevEmployerTIN.Size = New System.Drawing.Size(100, 20)
        Me.txtPrevEmployerTIN.TabIndex = 0
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(10, 188)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(75, 13)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "20 A Zip Code"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(10, 110)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(114, 13)
        Me.Label12.TabIndex = 17
        Me.Label12.Text = "20 Registered Address"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(10, 66)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(103, 13)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "19 Employer's Name"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(10, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(149, 13)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "18 Taxpayer Identification No."
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.RadioButton3)
        Me.GroupBox2.Controls.Add(Me.RadioButton4)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.chkSecondEmployer)
        Me.GroupBox2.Controls.Add(Me.chkMainEmployer)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 371)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(418, 100)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Part II   Employer Information (Present)"
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Checked = True
        Me.RadioButton3.Location = New System.Drawing.Point(47, 20)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(94, 17)
        Me.RadioButton3.TabIndex = 0
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "Main Employer"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(184, 20)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(122, 17)
        Me.RadioButton4.TabIndex = 1
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Text = "Secondary Employer"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(10, 22)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(19, 13)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "17"
        '
        'chkSecondEmployer
        '
        Me.chkSecondEmployer.AutoSize = True
        Me.chkSecondEmployer.Location = New System.Drawing.Point(184, 43)
        Me.chkSecondEmployer.Name = "chkSecondEmployer"
        Me.chkSecondEmployer.Size = New System.Drawing.Size(123, 17)
        Me.chkSecondEmployer.TabIndex = 14
        Me.chkSecondEmployer.Text = "Secondary Employer"
        Me.chkSecondEmployer.UseVisualStyleBackColor = True
        Me.chkSecondEmployer.Visible = False
        '
        'chkMainEmployer
        '
        Me.chkMainEmployer.AutoSize = True
        Me.chkMainEmployer.Location = New System.Drawing.Point(47, 43)
        Me.chkMainEmployer.Name = "chkMainEmployer"
        Me.chkMainEmployer.Size = New System.Drawing.Size(95, 17)
        Me.chkMainEmployer.TabIndex = 13
        Me.chkMainEmployer.Text = "Main Employer"
        Me.chkMainEmployer.UseVisualStyleBackColor = True
        Me.chkMainEmployer.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Panel3)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.rdbNo)
        Me.GroupBox1.Controls.Add(Me.rdbYes)
        Me.GroupBox1.Controls.Add(Me.txtMinWageMonth)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.txtForTheYear)
        Me.GroupBox1.Controls.Add(Me.txtMinWageDay)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.TextBox4)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.TextBox3)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 59)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(419, 306)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Part I   Employee Information"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rdbYesMinimum)
        Me.Panel3.Controls.Add(Me.rdbNoMinimum)
        Me.Panel3.Location = New System.Drawing.Point(91, 243)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(130, 24)
        Me.Panel3.TabIndex = 9
        '
        'rdbYesMinimum
        '
        Me.rdbYesMinimum.AutoSize = True
        Me.rdbYesMinimum.Checked = True
        Me.rdbYesMinimum.Location = New System.Drawing.Point(0, 3)
        Me.rdbYesMinimum.Name = "rdbYesMinimum"
        Me.rdbYesMinimum.Size = New System.Drawing.Size(43, 17)
        Me.rdbYesMinimum.TabIndex = 9
        Me.rdbYesMinimum.TabStop = True
        Me.rdbYesMinimum.Text = "Yes"
        Me.rdbYesMinimum.UseVisualStyleBackColor = True
        '
        'rdbNoMinimum
        '
        Me.rdbNoMinimum.AutoSize = True
        Me.rdbNoMinimum.Location = New System.Drawing.Point(49, 3)
        Me.rdbNoMinimum.Name = "rdbNoMinimum"
        Me.rdbNoMinimum.Size = New System.Drawing.Size(39, 17)
        Me.rdbNoMinimum.TabIndex = 10
        Me.rdbNoMinimum.TabStop = True
        Me.rdbNoMinimum.Text = "No"
        Me.rdbNoMinimum.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(10, 214)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(302, 26)
        Me.Label18.TabIndex = 19
        Me.Label18.Text = "14 Minimum Wage Earner whose compensation is exempt from" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "     withholding tax an" & _
    "d not subject to income tax"
        '
        'rdbNo
        '
        Me.rdbNo.AutoSize = True
        Me.rdbNo.Location = New System.Drawing.Point(140, 116)
        Me.rdbNo.Name = "rdbNo"
        Me.rdbNo.Size = New System.Drawing.Size(39, 17)
        Me.rdbNo.TabIndex = 6
        Me.rdbNo.TabStop = True
        Me.rdbNo.Text = "No"
        Me.rdbNo.UseVisualStyleBackColor = True
        '
        'rdbYes
        '
        Me.rdbYes.AutoSize = True
        Me.rdbYes.Checked = True
        Me.rdbYes.Location = New System.Drawing.Point(91, 116)
        Me.rdbYes.Name = "rdbYes"
        Me.rdbYes.Size = New System.Drawing.Size(43, 17)
        Me.rdbYes.TabIndex = 5
        Me.rdbYes.TabStop = True
        Me.rdbYes.Text = "Yes"
        Me.rdbYes.UseVisualStyleBackColor = True
        '
        'txtMinWageMonth
        '
        Me.txtMinWageMonth.Location = New System.Drawing.Point(91, 191)
        Me.txtMinWageMonth.Name = "txtMinWageMonth"
        Me.txtMinWageMonth.Size = New System.Drawing.Size(100, 20)
        Me.txtMinWageMonth.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "1 For the Year"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(10, 175)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(211, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "13 Statutory Minimum Wage rate per month"
        '
        'txtForTheYear
        '
        Me.txtForTheYear.Location = New System.Drawing.Point(91, 38)
        Me.txtForTheYear.Name = "txtForTheYear"
        Me.txtForTheYear.Size = New System.Drawing.Size(100, 20)
        Me.txtForTheYear.TabIndex = 0
        '
        'txtMinWageDay
        '
        Me.txtMinWageDay.Location = New System.Drawing.Point(91, 152)
        Me.txtMinWageDay.Name = "txtMinWageDay"
        Me.txtMinWageDay.Size = New System.Drawing.Size(100, 20)
        Me.txtMinWageDay.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(82, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "2 For the Period"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(10, 136)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(199, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "12 Statutory Minimum Wage rate per day"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(91, 77)
        Me.TextBox1.MaxLength = 2
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(23, 20)
        Me.TextBox1.TabIndex = 1
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(120, 77)
        Me.TextBox2.MaxLength = 2
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(23, 20)
        Me.TextBox2.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 100)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(389, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "9A Is the wife claiming the additional exemption for qualified dependent children" & _
    " ?"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(237, 77)
        Me.TextBox4.MaxLength = 2
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(23, 20)
        Me.TextBox4.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic)
        Me.Label5.Location = New System.Drawing.Point(156, 84)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "MM/DD"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(208, 77)
        Me.TextBox3.MaxLength = 2
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(23, 20)
        Me.TextBox3.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(39, 84)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "MM/DD"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnDiscard)
        Me.Panel2.Controls.Add(Me.btnApply)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 515)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(450, 37)
        Me.Panel2.TabIndex = 2
        '
        'btnDiscard
        '
        Me.btnDiscard.Location = New System.Drawing.Point(229, 6)
        Me.btnDiscard.Name = "btnDiscard"
        Me.btnDiscard.Size = New System.Drawing.Size(95, 23)
        Me.btnDiscard.TabIndex = 1
        Me.btnDiscard.Text = "Discard changes"
        Me.btnDiscard.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(128, 6)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(95, 23)
        Me.btnApply.TabIndex = 0
        Me.btnApply.Text = "&Save changes"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.AutoCompleteTextBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(450, 37)
        Me.Panel1.TabIndex = 1
        '
        'AutoCompleteTextBox1
        '
        Me.AutoCompleteTextBox1.Enabled = False
        Me.AutoCompleteTextBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AutoCompleteTextBox1.Location = New System.Drawing.Point(168, 7)
        Me.AutoCompleteTextBox1.Name = "AutoCompleteTextBox1"
        Me.AutoCompleteTextBox1.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.AutoCompleteTextBox1.PopupOffset = New System.Drawing.Point(12, 0)
        Me.AutoCompleteTextBox1.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.AutoCompleteTextBox1.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.AutoCompleteTextBox1.PopupWidth = 300
        Me.AutoCompleteTextBox1.Size = New System.Drawing.Size(156, 25)
        Me.AutoCompleteTextBox1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(81, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Employee ID"
        '
        'btnSearch
        '
        Me.btnSearch.Image = CType(resources.GetObject("btnSearch.Image"), System.Drawing.Image)
        Me.btnSearch.Location = New System.Drawing.Point(330, 7)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(25, 25)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'crvAlphaList
        '
        Me.crvAlphaList.ActiveViewIndex = -1
        Me.crvAlphaList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crvAlphaList.CachedPageNumberPerDoc = 10
        Me.crvAlphaList.Cursor = System.Windows.Forms.Cursors.Default
        Me.crvAlphaList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.crvAlphaList.Location = New System.Drawing.Point(0, 25)
        Me.crvAlphaList.Name = "crvAlphaList"
        Me.crvAlphaList.Size = New System.Drawing.Size(593, 527)
        Me.crvAlphaList.TabIndex = 500
        Me.crvAlphaList.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnReload})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(593, 25)
        Me.ToolStrip1.TabIndex = 501
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnReload
        '
        Me.tsbtnReload.Enabled = False
        Me.tsbtnReload.Image = CType(resources.GetObject("tsbtnReload.Image"), System.Drawing.Image)
        Me.tsbtnReload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnReload.Name = "tsbtnReload"
        Me.tsbtnReload.Size = New System.Drawing.Size(133, 22)
        Me.tsbtnReload.Text = "&Reload changes (F5)"
        '
        'bgwEmployeeID
        '
        Me.bgwEmployeeID.WorkerReportsProgress = True
        Me.bgwEmployeeID.WorkerSupportsCancellation = True
        '
        'bgReport
        '
        Me.bgReport.WorkerReportsProgress = True
        Me.bgReport.WorkerSupportsCancellation = True
        '
        'bgLoadAlphaList
        '
        Me.bgLoadAlphaList.WorkerReportsProgress = True
        Me.bgLoadAlphaList.WorkerSupportsCancellation = True
        '
        'AlphaListRptVwr
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1049, 552)
        Me.Controls.Add(Me.SplitContainer1)
        Me.KeyPreview = True
        Me.Name = "AlphaListRptVwr"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.pnlEmployee.ResumeLayout(False)
        Me.pnlEmployee.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents crvAlphaList As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents pnlEmployee As System.Windows.Forms.Panel
    Friend WithEvents bgwEmployeeID As System.ComponentModel.BackgroundWorker
    Friend WithEvents AutoCompleteTextBox1 As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents bgReport As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtForTheYear As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtMinWageDay As System.Windows.Forms.TextBox
    Friend WithEvents txtMinWageMonth As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkSecondEmployer As System.Windows.Forms.CheckBox
    Friend WithEvents chkMainEmployer As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtPrevEmployerTIN As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtPrevEmployerName As System.Windows.Forms.TextBox
    Friend WithEvents txtPrevEmployerAddress As System.Windows.Forms.TextBox
    Friend WithEvents txtPrevEmployerZipCode As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnDiscard As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents rdbYes As System.Windows.Forms.RadioButton
    Friend WithEvents rdbNo As System.Windows.Forms.RadioButton
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents txtPaidOnHealth As System.Windows.Forms.TextBox
    Friend WithEvents txtTaxabIncomePrevEmployer As System.Windows.Forms.TextBox
    Friend WithEvents bgLoadAlphaList As System.ComponentModel.BackgroundWorker
    Friend WithEvents rdbNoMinimum As System.Windows.Forms.RadioButton
    Friend WithEvents rdbYesMinimum As System.Windows.Forms.RadioButton
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents txtFName As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpID As System.Windows.Forms.TextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnReload As System.Windows.Forms.ToolStripButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
End Class
