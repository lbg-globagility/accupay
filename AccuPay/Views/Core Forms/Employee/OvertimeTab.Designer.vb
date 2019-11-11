<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OvertimeTab
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.txtendtimeEmpOT = New System.Windows.Forms.TextBox()
        Me.txtstarttimeEmpOT = New System.Windows.Forms.TextBox()
        Me.dtpendateEmpOT = New System.Windows.Forms.DateTimePicker()
        Me.dtpstartdateEmpOT = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.pbEmpPicEmpOT = New System.Windows.Forms.PictureBox()
        Me.lnkLastOT = New System.Windows.Forms.LinkLabel()
        Me.Label339 = New System.Windows.Forms.Label()
        Me.Label338 = New System.Windows.Forms.Label()
        Me.lnkNxtOT = New System.Windows.Forms.LinkLabel()
        Me.lnkPrevOT = New System.Windows.Forms.LinkLabel()
        Me.Label186 = New System.Windows.Forms.Label()
        Me.lnkFirstOT = New System.Windows.Forms.LinkLabel()
        Me.cboStatusEmpOT = New System.Windows.Forms.ComboBox()
        Me.btndlEmpOTfile = New System.Windows.Forms.Button()
        Me.txtFNameEmpOT = New System.Windows.Forms.TextBox()
        Me.txtEmpIDEmpOT = New System.Windows.Forms.TextBox()
        Me.btnEmpOTtyp = New System.Windows.Forms.Button()
        Me.pbempEmpOT = New System.Windows.Forms.PictureBox()
        Me.Label187 = New System.Windows.Forms.Label()
        Me.dgvempOT = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.eot_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eot_Type = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.eot_StartTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eot_EndTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eot_StartDate = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.eot_EndDate = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.eot_Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eot_Reason = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eot_Comment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eot_Image = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eot_viewimage = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.eot_attafilename = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eot_attafileextensn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label188 = New System.Windows.Forms.Label()
        Me.txtreasonEmpOT = New System.Windows.Forms.TextBox()
        Me.txtcommentsEmpOT = New System.Windows.Forms.TextBox()
        Me.btnClearEmpOT = New System.Windows.Forms.Button()
        Me.btnBrowseEmpOT = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewEmpOT = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveEmpOT = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnDeleteEmpOT = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton8 = New System.Windows.Forms.ToolStripButton()
        Me.OTImport = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton9 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton27 = New System.Windows.Forms.ToolStripButton()
        Me.Label181 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.pbEmpPicEmpOT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbempEmpOT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvempOT, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel5)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(864, 514)
        Me.Panel1.TabIndex = 1
        '
        'Panel5
        '
        Me.Panel5.AutoScroll = True
        Me.Panel5.BackColor = System.Drawing.Color.White
        Me.Panel5.Controls.Add(Me.Label10)
        Me.Panel5.Controls.Add(Me.Label9)
        Me.Panel5.Controls.Add(Me.Label8)
        Me.Panel5.Controls.Add(Me.Label7)
        Me.Panel5.Controls.Add(Me.Label6)
        Me.Panel5.Controls.Add(Me.Label181)
        Me.Panel5.Controls.Add(Me.txtendtimeEmpOT)
        Me.Panel5.Controls.Add(Me.txtstarttimeEmpOT)
        Me.Panel5.Controls.Add(Me.dtpendateEmpOT)
        Me.Panel5.Controls.Add(Me.dtpstartdateEmpOT)
        Me.Panel5.Controls.Add(Me.Label5)
        Me.Panel5.Controls.Add(Me.Label4)
        Me.Panel5.Controls.Add(Me.Label3)
        Me.Panel5.Controls.Add(Me.Label2)
        Me.Panel5.Controls.Add(Me.Label1)
        Me.Panel5.Controls.Add(Me.ComboBox1)
        Me.Panel5.Controls.Add(Me.pbEmpPicEmpOT)
        Me.Panel5.Controls.Add(Me.lnkLastOT)
        Me.Panel5.Controls.Add(Me.Label339)
        Me.Panel5.Controls.Add(Me.Label338)
        Me.Panel5.Controls.Add(Me.lnkNxtOT)
        Me.Panel5.Controls.Add(Me.lnkPrevOT)
        Me.Panel5.Controls.Add(Me.Label186)
        Me.Panel5.Controls.Add(Me.lnkFirstOT)
        Me.Panel5.Controls.Add(Me.cboStatusEmpOT)
        Me.Panel5.Controls.Add(Me.btndlEmpOTfile)
        Me.Panel5.Controls.Add(Me.txtFNameEmpOT)
        Me.Panel5.Controls.Add(Me.txtEmpIDEmpOT)
        Me.Panel5.Controls.Add(Me.btnEmpOTtyp)
        Me.Panel5.Controls.Add(Me.pbempEmpOT)
        Me.Panel5.Controls.Add(Me.Label187)
        Me.Panel5.Controls.Add(Me.dgvempOT)
        Me.Panel5.Controls.Add(Me.Label188)
        Me.Panel5.Controls.Add(Me.txtreasonEmpOT)
        Me.Panel5.Controls.Add(Me.txtcommentsEmpOT)
        Me.Panel5.Controls.Add(Me.btnClearEmpOT)
        Me.Panel5.Controls.Add(Me.btnBrowseEmpOT)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(0, 25)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(864, 489)
        Me.Panel5.TabIndex = 195
        '
        'txtendtimeEmpOT
        '
        Me.txtendtimeEmpOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtendtimeEmpOT.Location = New System.Drawing.Point(127, 199)
        Me.txtendtimeEmpOT.Name = "txtendtimeEmpOT"
        Me.txtendtimeEmpOT.Size = New System.Drawing.Size(100, 20)
        Me.txtendtimeEmpOT.TabIndex = 520
        '
        'txtstarttimeEmpOT
        '
        Me.txtstarttimeEmpOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtstarttimeEmpOT.Location = New System.Drawing.Point(127, 173)
        Me.txtstarttimeEmpOT.Name = "txtstarttimeEmpOT"
        Me.txtstarttimeEmpOT.Size = New System.Drawing.Size(100, 20)
        Me.txtstarttimeEmpOT.TabIndex = 519
        '
        'dtpendateEmpOT
        '
        Me.dtpendateEmpOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpendateEmpOT.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpendateEmpOT.Location = New System.Drawing.Point(128, 149)
        Me.dtpendateEmpOT.Name = "dtpendateEmpOT"
        Me.dtpendateEmpOT.Size = New System.Drawing.Size(99, 20)
        Me.dtpendateEmpOT.TabIndex = 518
        '
        'dtpstartdateEmpOT
        '
        Me.dtpstartdateEmpOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpstartdateEmpOT.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpstartdateEmpOT.Location = New System.Drawing.Point(128, 123)
        Me.dtpstartdateEmpOT.Name = "dtpstartdateEmpOT"
        Me.dtpstartdateEmpOT.Size = New System.Drawing.Size(99, 20)
        Me.dtpstartdateEmpOT.TabIndex = 517
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(50, 200)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 13)
        Me.Label5.TabIndex = 516
        Me.Label5.Text = "End time:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(50, 174)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 515
        Me.Label4.Text = "Stat time:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(50, 149)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 514
        Me.Label3.Text = "End date:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(50, 125)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 513
        Me.Label2.Text = "Start date:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(50, 101)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 512
        Me.Label1.Text = "Type:"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(127, 97)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(100, 21)
        Me.ComboBox1.TabIndex = 511
        '
        'pbEmpPicEmpOT
        '
        Me.pbEmpPicEmpOT.BackColor = System.Drawing.Color.White
        Me.pbEmpPicEmpOT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPicEmpOT.Location = New System.Drawing.Point(18, 6)
        Me.pbEmpPicEmpOT.Name = "pbEmpPicEmpOT"
        Me.pbEmpPicEmpOT.Size = New System.Drawing.Size(90, 80)
        Me.pbEmpPicEmpOT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPicEmpOT.TabIndex = 510
        Me.pbEmpPicEmpOT.TabStop = False
        '
        'lnkLastOT
        '
        Me.lnkLastOT.AutoSize = True
        Me.lnkLastOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkLastOT.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkLastOT.Location = New System.Drawing.Point(476, 561)
        Me.lnkLastOT.Name = "lnkLastOT"
        Me.lnkLastOT.Size = New System.Drawing.Size(44, 15)
        Me.lnkLastOT.TabIndex = 509
        Me.lnkLastOT.TabStop = True
        Me.lnkLastOT.Text = "Last>>"
        '
        'Label339
        '
        Me.Label339.AutoSize = True
        Me.Label339.ForeColor = System.Drawing.Color.White
        Me.Label339.Location = New System.Drawing.Point(847, 406)
        Me.Label339.Name = "Label339"
        Me.Label339.Size = New System.Drawing.Size(25, 13)
        Me.Label339.TabIndex = 505
        Me.Label339.Text = "___"
        '
        'Label338
        '
        Me.Label338.AutoSize = True
        Me.Label338.ForeColor = System.Drawing.Color.White
        Me.Label338.Location = New System.Drawing.Point(29, 583)
        Me.Label338.Name = "Label338"
        Me.Label338.Size = New System.Drawing.Size(25, 13)
        Me.Label338.TabIndex = 504
        Me.Label338.Text = "___"
        '
        'lnkNxtOT
        '
        Me.lnkNxtOT.AutoSize = True
        Me.lnkNxtOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkNxtOT.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkNxtOT.Location = New System.Drawing.Point(431, 561)
        Me.lnkNxtOT.Name = "lnkNxtOT"
        Me.lnkNxtOT.Size = New System.Drawing.Size(39, 15)
        Me.lnkNxtOT.TabIndex = 508
        Me.lnkNxtOT.TabStop = True
        Me.lnkNxtOT.Text = "Next>"
        '
        'lnkPrevOT
        '
        Me.lnkPrevOT.AutoSize = True
        Me.lnkPrevOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkPrevOT.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkPrevOT.Location = New System.Drawing.Point(79, 561)
        Me.lnkPrevOT.Name = "lnkPrevOT"
        Me.lnkPrevOT.Size = New System.Drawing.Size(38, 15)
        Me.lnkPrevOT.TabIndex = 507
        Me.lnkPrevOT.TabStop = True
        Me.lnkPrevOT.Text = "<Prev"
        '
        'Label186
        '
        Me.Label186.AutoSize = True
        Me.Label186.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label186.Location = New System.Drawing.Point(539, 100)
        Me.Label186.Name = "Label186"
        Me.Label186.Size = New System.Drawing.Size(40, 13)
        Me.Label186.TabIndex = 184
        Me.Label186.Text = "Status:"
        '
        'lnkFirstOT
        '
        Me.lnkFirstOT.AutoSize = True
        Me.lnkFirstOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkFirstOT.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkFirstOT.Location = New System.Drawing.Point(29, 561)
        Me.lnkFirstOT.Name = "lnkFirstOT"
        Me.lnkFirstOT.Size = New System.Drawing.Size(44, 15)
        Me.lnkFirstOT.TabIndex = 506
        Me.lnkFirstOT.TabStop = True
        Me.lnkFirstOT.Text = "<<First"
        '
        'cboStatusEmpOT
        '
        Me.cboStatusEmpOT.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboStatusEmpOT.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboStatusEmpOT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatusEmpOT.DropDownWidth = 150
        Me.cboStatusEmpOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatusEmpOT.FormattingEnabled = True
        Me.cboStatusEmpOT.Location = New System.Drawing.Point(605, 97)
        Me.cboStatusEmpOT.Name = "cboStatusEmpOT"
        Me.cboStatusEmpOT.Size = New System.Drawing.Size(126, 21)
        Me.cboStatusEmpOT.TabIndex = 9
        '
        'btndlEmpOTfile
        '
        Me.btndlEmpOTfile.Location = New System.Drawing.Point(678, 451)
        Me.btndlEmpOTfile.Name = "btndlEmpOTfile"
        Me.btndlEmpOTfile.Size = New System.Drawing.Size(95, 30)
        Me.btndlEmpOTfile.TabIndex = 182
        Me.btndlEmpOTfile.Text = "Download"
        Me.btndlEmpOTfile.UseVisualStyleBackColor = True
        '
        'txtFNameEmpOT
        '
        Me.txtFNameEmpOT.BackColor = System.Drawing.Color.White
        Me.txtFNameEmpOT.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFNameEmpOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFNameEmpOT.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFNameEmpOT.Location = New System.Drawing.Point(127, 22)
        Me.txtFNameEmpOT.MaxLength = 250
        Me.txtFNameEmpOT.Name = "txtFNameEmpOT"
        Me.txtFNameEmpOT.ReadOnly = True
        Me.txtFNameEmpOT.Size = New System.Drawing.Size(516, 28)
        Me.txtFNameEmpOT.TabIndex = 175
        '
        'txtEmpIDEmpOT
        '
        Me.txtEmpIDEmpOT.BackColor = System.Drawing.Color.White
        Me.txtEmpIDEmpOT.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpIDEmpOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmpIDEmpOT.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpIDEmpOT.Location = New System.Drawing.Point(127, 46)
        Me.txtEmpIDEmpOT.MaxLength = 50
        Me.txtEmpIDEmpOT.Name = "txtEmpIDEmpOT"
        Me.txtEmpIDEmpOT.ReadOnly = True
        Me.txtEmpIDEmpOT.Size = New System.Drawing.Size(516, 22)
        Me.txtEmpIDEmpOT.TabIndex = 170
        '
        'btnEmpOTtyp
        '
        Me.btnEmpOTtyp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEmpOTtyp.Location = New System.Drawing.Point(202, 267)
        Me.btnEmpOTtyp.Name = "btnEmpOTtyp"
        Me.btnEmpOTtyp.Size = New System.Drawing.Size(21, 23)
        Me.btnEmpOTtyp.TabIndex = 143
        Me.btnEmpOTtyp.Text = "..."
        Me.btnEmpOTtyp.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnEmpOTtyp.UseVisualStyleBackColor = True
        Me.btnEmpOTtyp.Visible = False
        '
        'pbempEmpOT
        '
        Me.pbempEmpOT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbempEmpOT.Location = New System.Drawing.Point(549, 242)
        Me.pbempEmpOT.Name = "pbempEmpOT"
        Me.pbempEmpOT.Size = New System.Drawing.Size(245, 191)
        Me.pbempEmpOT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbempEmpOT.TabIndex = 1
        Me.pbempEmpOT.TabStop = False
        '
        'Label187
        '
        Me.Label187.AutoSize = True
        Me.Label187.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label187.Location = New System.Drawing.Point(255, 174)
        Me.Label187.Name = "Label187"
        Me.Label187.Size = New System.Drawing.Size(54, 13)
        Me.Label187.TabIndex = 141
        Me.Label187.Text = "Comment:"
        '
        'dgvempOT
        '
        Me.dgvempOT.AllowUserToDeleteRows = False
        Me.dgvempOT.AllowUserToOrderColumns = True
        Me.dgvempOT.AllowUserToResizeRows = False
        Me.dgvempOT.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempOT.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvempOT.ColumnHeadersHeight = 38
        Me.dgvempOT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvempOT.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eot_RowID, Me.eot_Type, Me.eot_StartTime, Me.eot_EndTime, Me.eot_StartDate, Me.eot_EndDate, Me.eot_Status, Me.eot_Reason, Me.eot_Comment, Me.eot_Image, Me.eot_viewimage, Me.eot_attafilename, Me.eot_attafileextensn})
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempOT.DefaultCellStyle = DataGridViewCellStyle9
        Me.dgvempOT.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempOT.Location = New System.Drawing.Point(27, 242)
        Me.dgvempOT.MultiSelect = False
        Me.dgvempOT.Name = "dgvempOT"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempOT.RowHeadersDefaultCellStyle = DataGridViewCellStyle10
        Me.dgvempOT.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvempOT.Size = New System.Drawing.Size(493, 316)
        Me.dgvempOT.TabIndex = 0
        '
        'eot_RowID
        '
        Me.eot_RowID.HeaderText = "RowID"
        Me.eot_RowID.Name = "eot_RowID"
        Me.eot_RowID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.eot_RowID.Visible = False
        '
        'eot_Type
        '
        Me.eot_Type.HeaderText = "Overtime type"
        Me.eot_Type.Name = "eot_Type"
        Me.eot_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eot_Type.Width = 150
        '
        'eot_StartTime
        '
        Me.eot_StartTime.HeaderText = "Start time"
        Me.eot_StartTime.Name = "eot_StartTime"
        Me.eot_StartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'eot_EndTime
        '
        Me.eot_EndTime.HeaderText = "End time"
        Me.eot_EndTime.Name = "eot_EndTime"
        Me.eot_EndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'eot_StartDate
        '
        '
        '
        '
        Me.eot_StartDate.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.eot_StartDate.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.eot_StartDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eot_StartDate.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.eot_StartDate.HeaderText = "Start date"
        Me.eot_StartDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.eot_StartDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eot_StartDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.eot_StartDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.eot_StartDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.eot_StartDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eot_StartDate.MonthCalendar.DisplayMonth = New Date(2015, 5, 1, 0, 0, 0, 0)
        Me.eot_StartDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.eot_StartDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eot_StartDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.eot_StartDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eot_StartDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.eot_StartDate.Name = "eot_StartDate"
        Me.eot_StartDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eot_StartDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'eot_EndDate
        '
        '
        '
        '
        Me.eot_EndDate.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.eot_EndDate.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.eot_EndDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eot_EndDate.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.eot_EndDate.HeaderText = "End date"
        Me.eot_EndDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.eot_EndDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eot_EndDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.eot_EndDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.eot_EndDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.eot_EndDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eot_EndDate.MonthCalendar.DisplayMonth = New Date(2015, 5, 1, 0, 0, 0, 0)
        Me.eot_EndDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.eot_EndDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eot_EndDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.eot_EndDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eot_EndDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.eot_EndDate.Name = "eot_EndDate"
        Me.eot_EndDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eot_EndDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'eot_Status
        '
        Me.eot_Status.HeaderText = "Status"
        Me.eot_Status.Name = "eot_Status"
        '
        'eot_Reason
        '
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eot_Reason.DefaultCellStyle = DataGridViewCellStyle7
        Me.eot_Reason.HeaderText = "Reason"
        Me.eot_Reason.Name = "eot_Reason"
        Me.eot_Reason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.eot_Reason.Width = 190
        '
        'eot_Comment
        '
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eot_Comment.DefaultCellStyle = DataGridViewCellStyle8
        Me.eot_Comment.HeaderText = "Comments"
        Me.eot_Comment.MaxInputLength = 499
        Me.eot_Comment.Name = "eot_Comment"
        Me.eot_Comment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.eot_Comment.Width = 190
        '
        'eot_Image
        '
        Me.eot_Image.HeaderText = "Image"
        Me.eot_Image.MaxInputLength = 1999
        Me.eot_Image.Name = "eot_Image"
        Me.eot_Image.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.eot_Image.Visible = False
        '
        'eot_viewimage
        '
        Me.eot_viewimage.HeaderText = ""
        Me.eot_viewimage.Name = "eot_viewimage"
        '
        'eot_attafilename
        '
        Me.eot_attafilename.HeaderText = "Attachment file name"
        Me.eot_attafilename.Name = "eot_attafilename"
        '
        'eot_attafileextensn
        '
        Me.eot_attafileextensn.HeaderText = "Attachment file extension"
        Me.eot_attafileextensn.Name = "eot_attafileextensn"
        '
        'Label188
        '
        Me.Label188.AutoSize = True
        Me.Label188.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label188.Location = New System.Drawing.Point(255, 101)
        Me.Label188.Name = "Label188"
        Me.Label188.Size = New System.Drawing.Size(47, 13)
        Me.Label188.TabIndex = 141
        Me.Label188.Text = "Reason:"
        '
        'txtreasonEmpOT
        '
        Me.txtreasonEmpOT.BackColor = System.Drawing.Color.White
        Me.txtreasonEmpOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtreasonEmpOT.Location = New System.Drawing.Point(330, 95)
        Me.txtreasonEmpOT.MaxLength = 500
        Me.txtreasonEmpOT.Multiline = True
        Me.txtreasonEmpOT.Name = "txtreasonEmpOT"
        Me.txtreasonEmpOT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtreasonEmpOT.Size = New System.Drawing.Size(190, 59)
        Me.txtreasonEmpOT.TabIndex = 7
        '
        'txtcommentsEmpOT
        '
        Me.txtcommentsEmpOT.BackColor = System.Drawing.Color.White
        Me.txtcommentsEmpOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtcommentsEmpOT.Location = New System.Drawing.Point(330, 163)
        Me.txtcommentsEmpOT.MaxLength = 2000
        Me.txtcommentsEmpOT.Multiline = True
        Me.txtcommentsEmpOT.Name = "txtcommentsEmpOT"
        Me.txtcommentsEmpOT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtcommentsEmpOT.Size = New System.Drawing.Size(190, 59)
        Me.txtcommentsEmpOT.TabIndex = 8
        '
        'btnClearEmpOT
        '
        Me.btnClearEmpOT.Location = New System.Drawing.Point(627, 487)
        Me.btnClearEmpOT.Name = "btnClearEmpOT"
        Me.btnClearEmpOT.Size = New System.Drawing.Size(95, 30)
        Me.btnClearEmpOT.TabIndex = 138
        Me.btnClearEmpOT.Text = "Clear"
        Me.btnClearEmpOT.UseVisualStyleBackColor = True
        '
        'btnBrowseEmpOT
        '
        Me.btnBrowseEmpOT.Location = New System.Drawing.Point(566, 451)
        Me.btnBrowseEmpOT.Name = "btnBrowseEmpOT"
        Me.btnBrowseEmpOT.Size = New System.Drawing.Size(95, 30)
        Me.btnBrowseEmpOT.TabIndex = 136
        Me.btnBrowseEmpOT.Text = "&Browse..."
        Me.btnBrowseEmpOT.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Location = New System.Drawing.Point(3, 28)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(830, 428)
        Me.Panel2.TabIndex = 2
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewEmpOT, Me.tsbtnSaveEmpOT, Me.tsbtnDeleteEmpOT, Me.ToolStripButton8, Me.OTImport, Me.ToolStripButton9, Me.ToolStripButton27})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(864, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNewEmpOT
        '
        Me.tsbtnNewEmpOT.Image = Global.AccuPay.My.Resources.Resources._new
        Me.tsbtnNewEmpOT.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewEmpOT.Name = "tsbtnNewEmpOT"
        Me.tsbtnNewEmpOT.Size = New System.Drawing.Size(103, 22)
        Me.tsbtnNewEmpOT.Text = "&New Overtime"
        '
        'tsbtnSaveEmpOT
        '
        Me.tsbtnSaveEmpOT.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.tsbtnSaveEmpOT.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveEmpOT.Name = "tsbtnSaveEmpOT"
        Me.tsbtnSaveEmpOT.Size = New System.Drawing.Size(103, 22)
        Me.tsbtnSaveEmpOT.Text = "&Save Overtime"
        '
        'tsbtnDeleteEmpOT
        '
        Me.tsbtnDeleteEmpOT.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.tsbtnDeleteEmpOT.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDeleteEmpOT.Name = "tsbtnDeleteEmpOT"
        Me.tsbtnDeleteEmpOT.Size = New System.Drawing.Size(112, 22)
        Me.tsbtnDeleteEmpOT.Text = "Delete Overtime"
        '
        'ToolStripButton8
        '
        Me.ToolStripButton8.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.ToolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton8.Name = "ToolStripButton8"
        Me.ToolStripButton8.Size = New System.Drawing.Size(63, 22)
        Me.ToolStripButton8.Text = "Cancel"
        '
        'OTImport
        '
        Me.OTImport.Image = Global.AccuPay.My.Resources.Resources.Add
        Me.OTImport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OTImport.Name = "OTImport"
        Me.OTImport.Size = New System.Drawing.Size(115, 22)
        Me.OTImport.Text = "Import Overtime"
        '
        'ToolStripButton9
        '
        Me.ToolStripButton9.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton9.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton9.Name = "ToolStripButton9"
        Me.ToolStripButton9.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton9.Text = "Close"
        '
        'ToolStripButton27
        '
        Me.ToolStripButton27.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton27.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton27.Image = Global.AccuPay.My.Resources.Resources.audit_trail_icon
        Me.ToolStripButton27.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton27.Name = "ToolStripButton27"
        Me.ToolStripButton27.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton27.Text = "ToolStripButton1"
        Me.ToolStripButton27.ToolTipText = "Show audit trails"
        '
        'Label181
        '
        Me.Label181.AutoSize = True
        Me.Label181.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label181.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label181.Location = New System.Drawing.Point(83, 97)
        Me.Label181.Name = "Label181"
        Me.Label181.Size = New System.Drawing.Size(18, 24)
        Me.Label181.TabIndex = 521
        Me.Label181.Text = "*"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(104, 119)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(18, 24)
        Me.Label6.TabIndex = 522
        Me.Label6.Text = "*"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(104, 145)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(18, 24)
        Me.Label7.TabIndex = 523
        Me.Label7.Text = "*"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(104, 168)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(18, 24)
        Me.Label8.TabIndex = 524
        Me.Label8.Text = "*"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(104, 198)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(18, 24)
        Me.Label9.TabIndex = 525
        Me.Label9.Text = "*"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(581, 94)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(18, 24)
        Me.Label10.TabIndex = 526
        Me.Label10.Text = "*"
        '
        'OvertimeTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Name = "OvertimeTab"
        Me.Size = New System.Drawing.Size(870, 520)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        CType(Me.pbEmpPicEmpOT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbempEmpOT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvempOT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents tsbtnNewEmpOT As ToolStripButton
    Friend WithEvents tsbtnSaveEmpOT As ToolStripButton
    Friend WithEvents tsbtnDeleteEmpOT As ToolStripButton
    Friend WithEvents ToolStripButton8 As ToolStripButton
    Friend WithEvents OTImport As ToolStripButton
    Friend WithEvents ToolStripButton9 As ToolStripButton
    Friend WithEvents ToolStripButton27 As ToolStripButton
    Friend WithEvents Panel5 As Panel
    Friend WithEvents pbEmpPicEmpOT As PictureBox
    Friend WithEvents lnkLastOT As LinkLabel
    Friend WithEvents Label339 As Label
    Friend WithEvents Label338 As Label
    Friend WithEvents lnkNxtOT As LinkLabel
    Friend WithEvents lnkPrevOT As LinkLabel
    Friend WithEvents Label186 As Label
    Friend WithEvents lnkFirstOT As LinkLabel
    Friend WithEvents cboStatusEmpOT As ComboBox
    Friend WithEvents btndlEmpOTfile As Button
    Friend WithEvents txtFNameEmpOT As TextBox
    Friend WithEvents txtEmpIDEmpOT As TextBox
    Friend WithEvents btnEmpOTtyp As Button
    Friend WithEvents pbempEmpOT As PictureBox
    Friend WithEvents dgvempOT As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents eot_RowID As DataGridViewTextBoxColumn
    Friend WithEvents eot_Type As DataGridViewComboBoxColumn
    Friend WithEvents eot_StartTime As DataGridViewTextBoxColumn
    Friend WithEvents eot_EndTime As DataGridViewTextBoxColumn
    Friend WithEvents eot_StartDate As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents eot_EndDate As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents eot_Status As DataGridViewTextBoxColumn
    Friend WithEvents eot_Reason As DataGridViewTextBoxColumn
    Friend WithEvents eot_Comment As DataGridViewTextBoxColumn
    Friend WithEvents eot_Image As DataGridViewTextBoxColumn
    Friend WithEvents eot_viewimage As DataGridViewButtonColumn
    Friend WithEvents eot_attafilename As DataGridViewTextBoxColumn
    Friend WithEvents eot_attafileextensn As DataGridViewTextBoxColumn
    Friend WithEvents txtreasonEmpOT As TextBox
    Friend WithEvents txtcommentsEmpOT As TextBox
    Friend WithEvents btnClearEmpOT As Button
    Friend WithEvents btnBrowseEmpOT As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents txtendtimeEmpOT As TextBox
    Friend WithEvents txtstarttimeEmpOT As TextBox
    Friend WithEvents dtpendateEmpOT As DateTimePicker
    Friend WithEvents dtpstartdateEmpOT As DateTimePicker
    Friend WithEvents Label187 As Label
    Friend WithEvents Label188 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label181 As Label
    Friend WithEvents Label10 As Label
End Class
