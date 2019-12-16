<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PFF_053Form
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
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.txtEmpID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtstname = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtprovincestatecountry = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtsubdivision = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtlotblockphasehouse = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtbuildingname = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtunitroomflr = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtbrgy = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtEmpbName = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtmunicipalitycity = New System.Windows.Forms.TextBox()
        Me.grpAddress = New System.Windows.Forms.GroupBox()
        Me.dgvemplist = New System.Windows.Forms.DataGridView()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtzipcode = New System.Windows.Forms.TextBox()
        Me.c_pagibigmidno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_acctno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_membershipprog = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_lname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_fname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_nameexit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_mname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_periodcovered = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_monthlycompensation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_eeshare = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_ershare = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_total = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_remarks = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grpAddress.SuspendLayout()
        CType(Me.dgvemplist, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(735, 12)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(132, 36)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'txtEmpID
        '
        Me.txtEmpID.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtEmpID.Location = New System.Drawing.Point(363, 28)
        Me.txtEmpID.Name = "txtEmpID"
        Me.txtEmpID.Size = New System.Drawing.Size(339, 20)
        Me.txtEmpID.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(363, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(185, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Pag-IBIG EMPLOYER'S ID NUMBER"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(604, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Subdivision"
        '
        'txtstname
        '
        Me.txtstname.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtstname.Location = New System.Drawing.Point(351, 89)
        Me.txtstname.Name = "txtstname"
        Me.txtstname.Size = New System.Drawing.Size(237, 20)
        Me.txtstname.TabIndex = 22
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(949, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Municipality/City"
        '
        'txtprovincestatecountry
        '
        Me.txtprovincestatecountry.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtprovincestatecountry.Location = New System.Drawing.Point(949, 89)
        Me.txtprovincestatecountry.Name = "txtprovincestatecountry"
        Me.txtprovincestatecountry.Size = New System.Drawing.Size(339, 20)
        Me.txtprovincestatecountry.TabIndex = 26
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(351, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Street Name"
        '
        'txtsubdivision
        '
        Me.txtsubdivision.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtsubdivision.Location = New System.Drawing.Point(604, 47)
        Me.txtsubdivision.Name = "txtsubdivision"
        Me.txtsubdivision.Size = New System.Drawing.Size(339, 20)
        Me.txtsubdivision.TabIndex = 23
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(351, 31)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(208, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Lot No., Block No., Phase No., House No."
        '
        'txtlotblockphasehouse
        '
        Me.txtlotblockphasehouse.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtlotblockphasehouse.Location = New System.Drawing.Point(351, 47)
        Me.txtlotblockphasehouse.Name = "txtlotblockphasehouse"
        Me.txtlotblockphasehouse.Size = New System.Drawing.Size(237, 20)
        Me.txtlotblockphasehouse.TabIndex = 21
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 73)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(75, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Building Name"
        '
        'txtbuildingname
        '
        Me.txtbuildingname.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtbuildingname.Location = New System.Drawing.Point(6, 89)
        Me.txtbuildingname.Name = "txtbuildingname"
        Me.txtbuildingname.Size = New System.Drawing.Size(339, 20)
        Me.txtbuildingname.TabIndex = 20
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 31)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(105, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Unit/Room No. Floor"
        '
        'txtunitroomflr
        '
        Me.txtunitroomflr.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtunitroomflr.Location = New System.Drawing.Point(6, 47)
        Me.txtunitroomflr.Name = "txtunitroomflr"
        Me.txtunitroomflr.Size = New System.Drawing.Size(339, 20)
        Me.txtunitroomflr.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(604, 73)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Barangay"
        '
        'txtbrgy
        '
        Me.txtbrgy.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtbrgy.Location = New System.Drawing.Point(604, 89)
        Me.txtbrgy.Name = "txtbrgy"
        Me.txtbrgy.Size = New System.Drawing.Size(339, 20)
        Me.txtbrgy.TabIndex = 24
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(18, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(159, 13)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "EMPLOYER/BUSINESS NAME"
        '
        'txtEmpbName
        '
        Me.txtEmpbName.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtEmpbName.Location = New System.Drawing.Point(18, 28)
        Me.txtEmpbName.Name = "txtEmpbName"
        Me.txtEmpbName.Size = New System.Drawing.Size(339, 20)
        Me.txtEmpbName.TabIndex = 17
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(949, 73)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(167, 13)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "Province/State/Country(if abroad)"
        '
        'txtmunicipalitycity
        '
        Me.txtmunicipalitycity.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtmunicipalitycity.Location = New System.Drawing.Point(949, 47)
        Me.txtmunicipalitycity.Name = "txtmunicipalitycity"
        Me.txtmunicipalitycity.Size = New System.Drawing.Size(339, 20)
        Me.txtmunicipalitycity.TabIndex = 25
        '
        'grpAddress
        '
        Me.grpAddress.Controls.Add(Me.Label11)
        Me.grpAddress.Controls.Add(Me.txtzipcode)
        Me.grpAddress.Controls.Add(Me.Label7)
        Me.grpAddress.Controls.Add(Me.Label10)
        Me.grpAddress.Controls.Add(Me.txtstname)
        Me.grpAddress.Controls.Add(Me.txtmunicipalitycity)
        Me.grpAddress.Controls.Add(Me.Label2)
        Me.grpAddress.Controls.Add(Me.txtprovincestatecountry)
        Me.grpAddress.Controls.Add(Me.Label3)
        Me.grpAddress.Controls.Add(Me.Label8)
        Me.grpAddress.Controls.Add(Me.txtsubdivision)
        Me.grpAddress.Controls.Add(Me.txtbrgy)
        Me.grpAddress.Controls.Add(Me.Label4)
        Me.grpAddress.Controls.Add(Me.txtlotblockphasehouse)
        Me.grpAddress.Controls.Add(Me.txtunitroomflr)
        Me.grpAddress.Controls.Add(Me.Label5)
        Me.grpAddress.Controls.Add(Me.Label6)
        Me.grpAddress.Controls.Add(Me.txtbuildingname)
        Me.grpAddress.Location = New System.Drawing.Point(18, 54)
        Me.grpAddress.Name = "grpAddress"
        Me.grpAddress.Size = New System.Drawing.Size(1299, 163)
        Me.grpAddress.TabIndex = 21
        Me.grpAddress.TabStop = False
        Me.grpAddress.Text = "EMPLOYER BUSINESS ADDRESS"
        '
        'dgvemplist
        '
        Me.dgvemplist.AllowUserToDeleteRows = False
        Me.dgvemplist.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvemplist.BackgroundColor = System.Drawing.SystemColors.Control
        Me.dgvemplist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvemplist.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_pagibigmidno, Me.c_acctno, Me.c_membershipprog, Me.c_lname, Me.c_fname, Me.c_nameexit, Me.c_mname, Me.c_periodcovered, Me.c_monthlycompensation, Me.c_eeshare, Me.c_ershare, Me.c_total, Me.c_remarks})
        Me.dgvemplist.Location = New System.Drawing.Point(18, 223)
        Me.dgvemplist.Name = "dgvemplist"
        Me.dgvemplist.Size = New System.Drawing.Size(1299, 247)
        Me.dgvemplist.TabIndex = 22
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(947, 113)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(52, 13)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "ZIP Code"
        '
        'txtzipcode
        '
        Me.txtzipcode.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtzipcode.Location = New System.Drawing.Point(947, 129)
        Me.txtzipcode.Name = "txtzipcode"
        Me.txtzipcode.Size = New System.Drawing.Size(339, 20)
        Me.txtzipcode.TabIndex = 26
        '
        'c_pagibigmidno
        '
        Me.c_pagibigmidno.HeaderText = "Pag-IBIG MID No./RTN"
        Me.c_pagibigmidno.Name = "c_pagibigmidno"
        '
        'c_acctno
        '
        Me.c_acctno.HeaderText = "Account No"
        Me.c_acctno.Name = "c_acctno"
        '
        'c_membershipprog
        '
        Me.c_membershipprog.HeaderText = "Membership Program"
        Me.c_membershipprog.Name = "c_membershipprog"
        '
        'c_lname
        '
        Me.c_lname.HeaderText = "Last Name"
        Me.c_lname.Name = "c_lname"
        '
        'c_fname
        '
        Me.c_fname.HeaderText = "First Name"
        Me.c_fname.Name = "c_fname"
        '
        'c_nameexit
        '
        Me.c_nameexit.HeaderText = "Name Ext(Jr., III, etc)"
        Me.c_nameexit.Name = "c_nameexit"
        Me.c_nameexit.Width = 95
        '
        'c_mname
        '
        Me.c_mname.HeaderText = "Middle Name"
        Me.c_mname.Name = "c_mname"
        '
        'c_periodcovered
        '
        Me.c_periodcovered.HeaderText = "Period Covered"
        Me.c_periodcovered.Name = "c_periodcovered"
        '
        'c_monthlycompensation
        '
        Me.c_monthlycompensation.HeaderText = "Monthly Compensation"
        Me.c_monthlycompensation.Name = "c_monthlycompensation"
        '
        'c_eeshare
        '
        Me.c_eeshare.HeaderText = "EE Share"
        Me.c_eeshare.Name = "c_eeshare"
        Me.c_eeshare.Width = 80
        '
        'c_ershare
        '
        Me.c_ershare.HeaderText = "ER Share"
        Me.c_ershare.Name = "c_ershare"
        Me.c_ershare.Width = 80
        '
        'c_total
        '
        Me.c_total.HeaderText = "TOTAL"
        Me.c_total.Name = "c_total"
        Me.c_total.Width = 80
        '
        'c_remarks
        '
        Me.c_remarks.HeaderText = "Remarks"
        Me.c_remarks.Name = "c_remarks"
        '
        'PFF_053Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1335, 482)
        Me.Controls.Add(Me.dgvemplist)
        Me.Controls.Add(Me.grpAddress)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtEmpbName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtEmpID)
        Me.Controls.Add(Me.btnPrint)
        Me.Name = "PFF_053Form"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PFF_053Form"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpAddress.ResumeLayout(False)
        Me.grpAddress.PerformLayout()
        CType(Me.dgvemplist, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents txtEmpID As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtstname As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtprovincestatecountry As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtsubdivision As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtlotblockphasehouse As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtbuildingname As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtunitroomflr As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtbrgy As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtEmpbName As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtmunicipalitycity As System.Windows.Forms.TextBox
    Friend WithEvents grpAddress As System.Windows.Forms.GroupBox
    Friend WithEvents dgvemplist As System.Windows.Forms.DataGridView
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtzipcode As System.Windows.Forms.TextBox
    Friend WithEvents c_pagibigmidno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_acctno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_membershipprog As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_lname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_fname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_nameexit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_mname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_periodcovered As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_monthlycompensation As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_eeshare As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_ershare As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_total As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_remarks As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
