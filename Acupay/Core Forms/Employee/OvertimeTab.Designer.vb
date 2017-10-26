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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewEmpOT = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveEmpOT = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnDeleteEmpOT = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton8 = New System.Windows.Forms.ToolStripButton()
        Me.OTImport = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton9 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton27 = New System.Windows.Forms.ToolStripButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.lnkLastOT = New System.Windows.Forms.LinkLabel()
        Me.Label339 = New System.Windows.Forms.Label()
        Me.Label338 = New System.Windows.Forms.Label()
        Me.lnkNxtOT = New System.Windows.Forms.LinkLabel()
        Me.lnkPrevOT = New System.Windows.Forms.LinkLabel()
        Me.Label186 = New System.Windows.Forms.Label()
        Me.lnkFirstOT = New System.Windows.Forms.LinkLabel()
        Me.Label205 = New System.Windows.Forms.Label()
        Me.cboStatusEmpOT = New System.Windows.Forms.ComboBox()
        Me.Label204 = New System.Windows.Forms.Label()
        Me.btndlEmpOTfile = New System.Windows.Forms.Button()
        Me.Label202 = New System.Windows.Forms.Label()
        Me.txtFNameEmpOT = New System.Windows.Forms.TextBox()
        Me.Label201 = New System.Windows.Forms.Label()
        Me.txtEmpIDEmpOT = New System.Windows.Forms.TextBox()
        Me.Label200 = New System.Windows.Forms.Label()
        Me.btnEmpOTtyp = New System.Windows.Forms.Button()
        Me.pbempEmpOT = New System.Windows.Forms.PictureBox()
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
        Me.txtreasonEmpOT = New System.Windows.Forms.TextBox()
        Me.txtcommentsEmpOT = New System.Windows.Forms.TextBox()
        Me.btnClearEmpOT = New System.Windows.Forms.Button()
        Me.btnBrowseEmpOT = New System.Windows.Forms.Button()
        Me.Label203 = New System.Windows.Forms.Label()
        Me.pbEmpPicEmpOT = New System.Windows.Forms.PictureBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpstartdateEmpOT = New System.Windows.Forms.DateTimePicker()
        Me.dtpendateEmpOT = New System.Windows.Forms.DateTimePicker()
        Me.txtstarttimeEmpOT = New System.Windows.Forms.TextBox()
        Me.txtendtimeEmpOT = New System.Windows.Forms.TextBox()
        Me.Label187 = New System.Windows.Forms.Label()
        Me.Label188 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.pbempEmpOT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvempOT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbEmpPicEmpOT, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.tsbtnNewEmpOT.Image = Global.Acupay.My.Resources.Resources._new
        Me.tsbtnNewEmpOT.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewEmpOT.Name = "tsbtnNewEmpOT"
        Me.tsbtnNewEmpOT.Size = New System.Drawing.Size(103, 22)
        Me.tsbtnNewEmpOT.Text = "&New Overtime"
        '
        'tsbtnSaveEmpOT
        '
        Me.tsbtnSaveEmpOT.Image = Global.Acupay.My.Resources.Resources.Save
        Me.tsbtnSaveEmpOT.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveEmpOT.Name = "tsbtnSaveEmpOT"
        Me.tsbtnSaveEmpOT.Size = New System.Drawing.Size(103, 22)
        Me.tsbtnSaveEmpOT.Text = "&Save Overtime"
        '
        'tsbtnDeleteEmpOT
        '
        Me.tsbtnDeleteEmpOT.Image = Global.Acupay.My.Resources.Resources.CLOSE_00
        Me.tsbtnDeleteEmpOT.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDeleteEmpOT.Name = "tsbtnDeleteEmpOT"
        Me.tsbtnDeleteEmpOT.Size = New System.Drawing.Size(112, 22)
        Me.tsbtnDeleteEmpOT.Text = "Delete Overtime"
        '
        'ToolStripButton8
        '
        Me.ToolStripButton8.Image = Global.Acupay.My.Resources.Resources.cancel1
        Me.ToolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton8.Name = "ToolStripButton8"
        Me.ToolStripButton8.Size = New System.Drawing.Size(63, 22)
        Me.ToolStripButton8.Text = "Cancel"
        '
        'OTImport
        '
        Me.OTImport.Image = Global.Acupay.My.Resources.Resources.Add
        Me.OTImport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OTImport.Name = "OTImport"
        Me.OTImport.Size = New System.Drawing.Size(115, 22)
        Me.OTImport.Text = "Import Overtime"
        '
        'ToolStripButton9
        '
        Me.ToolStripButton9.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton9.Image = Global.Acupay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton9.Name = "ToolStripButton9"
        Me.ToolStripButton9.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton9.Text = "Close"
        '
        'ToolStripButton27
        '
        Me.ToolStripButton27.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton27.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton27.Image = Global.Acupay.My.Resources.Resources.audit_trail_icon
        Me.ToolStripButton27.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton27.Name = "ToolStripButton27"
        Me.ToolStripButton27.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton27.Text = "ToolStripButton1"
        Me.ToolStripButton27.ToolTipText = "Show audit trails"
        '
        'Panel2
        '
        Me.Panel2.Location = New System.Drawing.Point(3, 28)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(830, 428)
        Me.Panel2.TabIndex = 2
        '
        'Panel5
        '
        Me.Panel5.AutoScroll = True
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
        Me.Panel5.Controls.Add(Me.Label205)
        Me.Panel5.Controls.Add(Me.cboStatusEmpOT)
        Me.Panel5.Controls.Add(Me.Label204)
        Me.Panel5.Controls.Add(Me.btndlEmpOTfile)
        Me.Panel5.Controls.Add(Me.Label202)
        Me.Panel5.Controls.Add(Me.txtFNameEmpOT)
        Me.Panel5.Controls.Add(Me.Label201)
        Me.Panel5.Controls.Add(Me.txtEmpIDEmpOT)
        Me.Panel5.Controls.Add(Me.Label200)
        Me.Panel5.Controls.Add(Me.btnEmpOTtyp)
        Me.Panel5.Controls.Add(Me.pbempEmpOT)
        Me.Panel5.Controls.Add(Me.Label187)
        Me.Panel5.Controls.Add(Me.dgvempOT)
        Me.Panel5.Controls.Add(Me.Label188)
        Me.Panel5.Controls.Add(Me.txtreasonEmpOT)
        Me.Panel5.Controls.Add(Me.txtcommentsEmpOT)
        Me.Panel5.Controls.Add(Me.btnClearEmpOT)
        Me.Panel5.Controls.Add(Me.btnBrowseEmpOT)
        Me.Panel5.Controls.Add(Me.Label203)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(0, 25)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(864, 489)
        Me.Panel5.TabIndex = 195
        '
        'lnkLastOT
        '
        Me.lnkLastOT.AutoSize = True
        Me.lnkLastOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkLastOT.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkLastOT.Location = New System.Drawing.Point(288, 561)
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
        Me.lnkNxtOT.Location = New System.Drawing.Point(243, 561)
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
        Me.Label186.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label186.Location = New System.Drawing.Point(607, 93)
        Me.Label186.Name = "Label186"
        Me.Label186.Size = New System.Drawing.Size(43, 15)
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
        'Label205
        '
        Me.Label205.AutoSize = True
        Me.Label205.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label205.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label205.Location = New System.Drawing.Point(649, 87)
        Me.Label205.Name = "Label205"
        Me.Label205.Size = New System.Drawing.Size(18, 24)
        Me.Label205.TabIndex = 193
        Me.Label205.Text = "*"
        '
        'cboStatusEmpOT
        '
        Me.cboStatusEmpOT.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboStatusEmpOT.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboStatusEmpOT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatusEmpOT.DropDownWidth = 150
        Me.cboStatusEmpOT.FormattingEnabled = True
        Me.cboStatusEmpOT.Location = New System.Drawing.Point(673, 90)
        Me.cboStatusEmpOT.Name = "cboStatusEmpOT"
        Me.cboStatusEmpOT.Size = New System.Drawing.Size(126, 21)
        Me.cboStatusEmpOT.TabIndex = 9
        '
        'Label204
        '
        Me.Label204.AutoSize = True
        Me.Label204.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label204.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label204.Location = New System.Drawing.Point(168, 137)
        Me.Label204.Name = "Label204"
        Me.Label204.Size = New System.Drawing.Size(18, 24)
        Me.Label204.TabIndex = 192
        Me.Label204.Text = "*"
        '
        'btndlEmpOTfile
        '
        Me.btndlEmpOTfile.Location = New System.Drawing.Point(742, 437)
        Me.btndlEmpOTfile.Name = "btndlEmpOTfile"
        Me.btndlEmpOTfile.Size = New System.Drawing.Size(95, 30)
        Me.btndlEmpOTfile.TabIndex = 182
        Me.btndlEmpOTfile.Text = "Download"
        Me.btndlEmpOTfile.UseVisualStyleBackColor = True
        '
        'Label202
        '
        Me.Label202.AutoSize = True
        Me.Label202.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label202.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label202.Location = New System.Drawing.Point(169, 190)
        Me.Label202.Name = "Label202"
        Me.Label202.Size = New System.Drawing.Size(18, 24)
        Me.Label202.TabIndex = 190
        Me.Label202.Text = "*"
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
        'Label201
        '
        Me.Label201.AutoSize = True
        Me.Label201.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label201.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label201.Location = New System.Drawing.Point(168, 163)
        Me.Label201.Name = "Label201"
        Me.Label201.Size = New System.Drawing.Size(18, 24)
        Me.Label201.TabIndex = 189
        Me.Label201.Text = "*"
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
        'Label200
        '
        Me.Label200.AutoSize = True
        Me.Label200.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label200.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label200.Location = New System.Drawing.Point(148, 88)
        Me.Label200.Name = "Label200"
        Me.Label200.Size = New System.Drawing.Size(18, 24)
        Me.Label200.TabIndex = 188
        Me.Label200.Text = "*"
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
        Me.pbempEmpOT.Location = New System.Drawing.Point(613, 228)
        Me.pbempEmpOT.Name = "pbempEmpOT"
        Me.pbempEmpOT.Size = New System.Drawing.Size(245, 191)
        Me.pbempEmpOT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbempEmpOT.TabIndex = 1
        Me.pbempEmpOT.TabStop = False
        '
        'dgvempOT
        '
        Me.dgvempOT.AllowUserToDeleteRows = False
        Me.dgvempOT.AllowUserToOrderColumns = True
        Me.dgvempOT.AllowUserToResizeRows = False
        Me.dgvempOT.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempOT.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvempOT.ColumnHeadersHeight = 38
        Me.dgvempOT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvempOT.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eot_RowID, Me.eot_Type, Me.eot_StartTime, Me.eot_EndTime, Me.eot_StartDate, Me.eot_EndDate, Me.eot_Status, Me.eot_Reason, Me.eot_Comment, Me.eot_Image, Me.eot_viewimage, Me.eot_attafilename, Me.eot_attafileextensn})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempOT.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgvempOT.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempOT.Location = New System.Drawing.Point(6, 228)
        Me.dgvempOT.MultiSelect = False
        Me.dgvempOT.Name = "dgvempOT"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempOT.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgvempOT.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvempOT.Size = New System.Drawing.Size(579, 331)
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
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eot_Reason.DefaultCellStyle = DataGridViewCellStyle2
        Me.eot_Reason.HeaderText = "Reason"
        Me.eot_Reason.Name = "eot_Reason"
        Me.eot_Reason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.eot_Reason.Width = 190
        '
        'eot_Comment
        '
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eot_Comment.DefaultCellStyle = DataGridViewCellStyle3
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
        'txtreasonEmpOT
        '
        Me.txtreasonEmpOT.BackColor = System.Drawing.Color.White
        Me.txtreasonEmpOT.Font = New System.Drawing.Font("Cambria", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtreasonEmpOT.Location = New System.Drawing.Point(395, 88)
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
        Me.txtcommentsEmpOT.Font = New System.Drawing.Font("Cambria", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtcommentsEmpOT.Location = New System.Drawing.Point(395, 156)
        Me.txtcommentsEmpOT.MaxLength = 2000
        Me.txtcommentsEmpOT.Multiline = True
        Me.txtcommentsEmpOT.Name = "txtcommentsEmpOT"
        Me.txtcommentsEmpOT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtcommentsEmpOT.Size = New System.Drawing.Size(190, 59)
        Me.txtcommentsEmpOT.TabIndex = 8
        '
        'btnClearEmpOT
        '
        Me.btnClearEmpOT.Location = New System.Drawing.Point(691, 473)
        Me.btnClearEmpOT.Name = "btnClearEmpOT"
        Me.btnClearEmpOT.Size = New System.Drawing.Size(95, 30)
        Me.btnClearEmpOT.TabIndex = 138
        Me.btnClearEmpOT.Text = "Clear"
        Me.btnClearEmpOT.UseVisualStyleBackColor = True
        '
        'btnBrowseEmpOT
        '
        Me.btnBrowseEmpOT.Location = New System.Drawing.Point(630, 437)
        Me.btnBrowseEmpOT.Name = "btnBrowseEmpOT"
        Me.btnBrowseEmpOT.Size = New System.Drawing.Size(95, 30)
        Me.btnBrowseEmpOT.TabIndex = 136
        Me.btnBrowseEmpOT.Text = "&Browse..."
        Me.btnBrowseEmpOT.UseVisualStyleBackColor = True
        '
        'Label203
        '
        Me.Label203.AutoSize = True
        Me.Label203.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label203.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label203.Location = New System.Drawing.Point(168, 112)
        Me.Label203.Name = "Label203"
        Me.Label203.Size = New System.Drawing.Size(18, 24)
        Me.Label203.TabIndex = 191
        Me.Label203.Text = "*"
        '
        'pbEmpPicEmpOT
        '
        Me.pbEmpPicEmpOT.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.pbEmpPicEmpOT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPicEmpOT.Location = New System.Drawing.Point(18, 6)
        Me.pbEmpPicEmpOT.Name = "pbEmpPicEmpOT"
        Me.pbEmpPicEmpOT.Size = New System.Drawing.Size(90, 80)
        Me.pbEmpPicEmpOT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPicEmpOT.TabIndex = 510
        Me.pbEmpPicEmpOT.TabStop = False
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Font = New System.Drawing.Font("Cambria", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(192, 90)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(100, 22)
        Me.ComboBox1.TabIndex = 511
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(115, 94)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 15)
        Me.Label1.TabIndex = 512
        Me.Label1.Text = "Type:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(115, 118)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 15)
        Me.Label2.TabIndex = 513
        Me.Label2.Text = "Start date:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(115, 142)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 15)
        Me.Label3.TabIndex = 514
        Me.Label3.Text = "End date:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(115, 167)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 15)
        Me.Label4.TabIndex = 515
        Me.Label4.Text = "Stat time:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(115, 193)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 15)
        Me.Label5.TabIndex = 516
        Me.Label5.Text = "End time:"
        '
        'dtpstartdateEmpOT
        '
        Me.dtpstartdateEmpOT.Font = New System.Drawing.Font("Cambria", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpstartdateEmpOT.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpstartdateEmpOT.Location = New System.Drawing.Point(193, 116)
        Me.dtpstartdateEmpOT.Name = "dtpstartdateEmpOT"
        Me.dtpstartdateEmpOT.Size = New System.Drawing.Size(99, 22)
        Me.dtpstartdateEmpOT.TabIndex = 517
        '
        'dtpendateEmpOT
        '
        Me.dtpendateEmpOT.Font = New System.Drawing.Font("Cambria", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpendateEmpOT.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpendateEmpOT.Location = New System.Drawing.Point(193, 142)
        Me.dtpendateEmpOT.Name = "dtpendateEmpOT"
        Me.dtpendateEmpOT.Size = New System.Drawing.Size(99, 22)
        Me.dtpendateEmpOT.TabIndex = 518
        '
        'txtstarttimeEmpOT
        '
        Me.txtstarttimeEmpOT.Font = New System.Drawing.Font("Cambria", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtstarttimeEmpOT.Location = New System.Drawing.Point(192, 166)
        Me.txtstarttimeEmpOT.Name = "txtstarttimeEmpOT"
        Me.txtstarttimeEmpOT.Size = New System.Drawing.Size(100, 22)
        Me.txtstarttimeEmpOT.TabIndex = 519
        '
        'txtendtimeEmpOT
        '
        Me.txtendtimeEmpOT.Font = New System.Drawing.Font("Cambria", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtendtimeEmpOT.Location = New System.Drawing.Point(192, 192)
        Me.txtendtimeEmpOT.Name = "txtendtimeEmpOT"
        Me.txtendtimeEmpOT.Size = New System.Drawing.Size(100, 22)
        Me.txtendtimeEmpOT.TabIndex = 520
        '
        'Label187
        '
        Me.Label187.AutoSize = True
        Me.Label187.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label187.Location = New System.Drawing.Point(320, 167)
        Me.Label187.Name = "Label187"
        Me.Label187.Size = New System.Drawing.Size(63, 15)
        Me.Label187.TabIndex = 141
        Me.Label187.Text = "Comment:"
        '
        'Label188
        '
        Me.Label188.AutoSize = True
        Me.Label188.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label188.Location = New System.Drawing.Point(320, 94)
        Me.Label188.Name = "Label188"
        Me.Label188.Size = New System.Drawing.Size(50, 15)
        Me.Label188.TabIndex = 141
        Me.Label188.Text = "Reason:"
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
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        CType(Me.pbempEmpOT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvempOT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbEmpPicEmpOT, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents Label205 As Label
    Friend WithEvents cboStatusEmpOT As ComboBox
    Friend WithEvents Label204 As Label
    Friend WithEvents btndlEmpOTfile As Button
    Friend WithEvents Label202 As Label
    Friend WithEvents txtFNameEmpOT As TextBox
    Friend WithEvents Label201 As Label
    Friend WithEvents txtEmpIDEmpOT As TextBox
    Friend WithEvents Label200 As Label
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
    Friend WithEvents Label203 As Label
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
End Class
