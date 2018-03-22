<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LeaveTab
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LeaveTab))
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip5 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewLeave = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveLeave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel10 = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnDeletLeave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton17 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton20 = New System.Windows.Forms.ToolStripButton()
        Me.Panel15 = New System.Windows.Forms.Panel()
        Me.Label345 = New System.Windows.Forms.Label()
        Me.Label346 = New System.Windows.Forms.Label()
        Me.cboleavestatus = New System.Windows.Forms.ComboBox()
        Me.Label324 = New System.Windows.Forms.Label()
        Me.Label323 = New System.Windows.Forms.Label()
        Me.dtpendate = New System.Windows.Forms.DateTimePicker()
        Me.dtpstartdate = New System.Windows.Forms.DateTimePicker()
        Me.pbEmpPicLeave = New System.Windows.Forms.PictureBox()
        Me.cboleavetypes = New System.Windows.Forms.ComboBox()
        Me.Label199 = New System.Windows.Forms.Label()
        Me.TabControl4 = New System.Windows.Forms.TabControl()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.Label128 = New System.Windows.Forms.Label()
        Me.Label131 = New System.Windows.Forms.Label()
        Me.Label132 = New System.Windows.Forms.Label()
        Me.txtmlbalLeave = New System.Windows.Forms.TextBox()
        Me.Label105 = New System.Windows.Forms.Label()
        Me.txtvlbalLeave = New System.Windows.Forms.TextBox()
        Me.Label117 = New System.Windows.Forms.Label()
        Me.Label118 = New System.Windows.Forms.Label()
        Me.txtslbalLeave = New System.Windows.Forms.TextBox()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.Label133 = New System.Windows.Forms.Label()
        Me.Label136 = New System.Windows.Forms.Label()
        Me.Label137 = New System.Windows.Forms.Label()
        Me.txtmlallowleave = New System.Windows.Forms.TextBox()
        Me.txtvlallowLeave = New System.Windows.Forms.TextBox()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.txtslallowLeave = New System.Windows.Forms.TextBox()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label104 = New System.Windows.Forms.Label()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.Label127 = New System.Windows.Forms.Label()
        Me.Label126 = New System.Windows.Forms.Label()
        Me.Label123 = New System.Windows.Forms.Label()
        Me.txtmlpaypLeave = New System.Windows.Forms.TextBox()
        Me.Label119 = New System.Windows.Forms.Label()
        Me.txtvlpaypLeave = New System.Windows.Forms.TextBox()
        Me.Label121 = New System.Windows.Forms.Label()
        Me.Label122 = New System.Windows.Forms.Label()
        Me.txtslpaypLeave = New System.Windows.Forms.TextBox()
        Me.Label198 = New System.Windows.Forms.Label()
        Me.txtFNameLeave = New System.Windows.Forms.TextBox()
        Me.Label197 = New System.Windows.Forms.Label()
        Me.txtEmpIDLeave = New System.Windows.Forms.TextBox()
        Me.Label196 = New System.Windows.Forms.Label()
        Me.btnleavtyp = New System.Windows.Forms.Button()
        Me.Label195 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.btndlleavefile = New System.Windows.Forms.Button()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.pbempleave = New System.Windows.Forms.PictureBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.dgvempleave = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.elv_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.elv_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.elv_StartTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.elv_EndTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.elv_StartDate = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.elv_EndDate = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.elv_Reason = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.elv_Comment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.elv_Image = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.elv_viewimage = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.elv_attafilename = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.elv_attafileextensn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.elv_Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.txtstarttime = New System.Windows.Forms.TextBox()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.txtendtime = New System.Windows.Forms.TextBox()
        Me.txtcomments = New System.Windows.Forms.TextBox()
        Me.txtstartdate = New System.Windows.Forms.TextBox()
        Me.txtreason = New System.Windows.Forms.TextBox()
        Me.txtendate = New System.Windows.Forms.TextBox()
        Me.Label224 = New System.Windows.Forms.Label()
        Me.Label225 = New System.Windows.Forms.Label()
        Me.ToolStrip5.SuspendLayout()
        Me.Panel15.SuspendLayout()
        CType(Me.pbEmpPicLeave, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl4.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        CType(Me.pbempleave, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvempleave, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip5
        '
        Me.ToolStrip5.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip5.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip5.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewLeave, Me.tsbtnSaveLeave, Me.ToolStripButton3, Me.ToolStripLabel10, Me.tsbtnDeletLeave, Me.ToolStripButton4, Me.ToolStripButton17, Me.ToolStripButton20})
        Me.ToolStrip5.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip5.Name = "ToolStrip5"
        Me.ToolStrip5.Size = New System.Drawing.Size(856, 25)
        Me.ToolStrip5.TabIndex = 1
        Me.ToolStrip5.Text = "ToolStrip5"
        '
        'tsbtnNewLeave
        '
        Me.tsbtnNewLeave.Image = Global.AccuPay.My.Resources.Resources._new
        Me.tsbtnNewLeave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewLeave.Name = "tsbtnNewLeave"
        Me.tsbtnNewLeave.Size = New System.Drawing.Size(84, 22)
        Me.tsbtnNewLeave.Text = "&New Leave"
        '
        'tsbtnSaveLeave
        '
        Me.tsbtnSaveLeave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.tsbtnSaveLeave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveLeave.Name = "tsbtnSaveLeave"
        Me.tsbtnSaveLeave.Size = New System.Drawing.Size(84, 22)
        Me.tsbtnSaveLeave.Text = "&Save Leave"
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(63, 22)
        Me.ToolStripButton3.Text = "Cancel"
        '
        'ToolStripLabel10
        '
        Me.ToolStripLabel10.AutoSize = False
        Me.ToolStripLabel10.Name = "ToolStripLabel10"
        Me.ToolStripLabel10.Size = New System.Drawing.Size(89, 22)
        '
        'tsbtnDeletLeave
        '
        Me.tsbtnDeletLeave.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.tsbtnDeletLeave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDeletLeave.Name = "tsbtnDeletLeave"
        Me.tsbtnDeletLeave.Size = New System.Drawing.Size(93, 22)
        Me.tsbtnDeletLeave.Text = "Delete Leave"
        '
        'ToolStripButton4
        '
        Me.ToolStripButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton4.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton4.Name = "ToolStripButton4"
        Me.ToolStripButton4.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton4.Text = "Close"
        '
        'ToolStripButton17
        '
        Me.ToolStripButton17.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton17.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton17.Image = Global.AccuPay.My.Resources.Resources.audit_trail_icon
        Me.ToolStripButton17.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton17.Name = "ToolStripButton17"
        Me.ToolStripButton17.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton17.Text = "ToolStripButton1"
        Me.ToolStripButton17.ToolTipText = "Show audit trails"
        '
        'ToolStripButton20
        '
        Me.ToolStripButton20.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton20.Image = CType(resources.GetObject("ToolStripButton20.Image"), System.Drawing.Image)
        Me.ToolStripButton20.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton20.Name = "ToolStripButton20"
        Me.ToolStripButton20.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton20.Text = "ToolStripButton20"
        '
        'Panel15
        '
        Me.Panel15.AutoScroll = True
        Me.Panel15.BackColor = System.Drawing.Color.White
        Me.Panel15.Controls.Add(Me.Label345)
        Me.Panel15.Controls.Add(Me.Label346)
        Me.Panel15.Controls.Add(Me.cboleavestatus)
        Me.Panel15.Controls.Add(Me.Label324)
        Me.Panel15.Controls.Add(Me.Label323)
        Me.Panel15.Controls.Add(Me.dtpendate)
        Me.Panel15.Controls.Add(Me.dtpstartdate)
        Me.Panel15.Controls.Add(Me.pbEmpPicLeave)
        Me.Panel15.Controls.Add(Me.cboleavetypes)
        Me.Panel15.Controls.Add(Me.Label199)
        Me.Panel15.Controls.Add(Me.TabControl4)
        Me.Panel15.Controls.Add(Me.Label198)
        Me.Panel15.Controls.Add(Me.txtFNameLeave)
        Me.Panel15.Controls.Add(Me.Label197)
        Me.Panel15.Controls.Add(Me.txtEmpIDLeave)
        Me.Panel15.Controls.Add(Me.Label196)
        Me.Panel15.Controls.Add(Me.btnleavtyp)
        Me.Panel15.Controls.Add(Me.Label195)
        Me.Panel15.Controls.Add(Me.Label32)
        Me.Panel15.Controls.Add(Me.btndlleavefile)
        Me.Panel15.Controls.Add(Me.Label33)
        Me.Panel15.Controls.Add(Me.Button6)
        Me.Panel15.Controls.Add(Me.Label34)
        Me.Panel15.Controls.Add(Me.Button7)
        Me.Panel15.Controls.Add(Me.Label35)
        Me.Panel15.Controls.Add(Me.pbempleave)
        Me.Panel15.Controls.Add(Me.Label36)
        Me.Panel15.Controls.Add(Me.dgvempleave)
        Me.Panel15.Controls.Add(Me.Label37)
        Me.Panel15.Controls.Add(Me.txtstarttime)
        Me.Panel15.Controls.Add(Me.Label38)
        Me.Panel15.Controls.Add(Me.txtendtime)
        Me.Panel15.Controls.Add(Me.txtcomments)
        Me.Panel15.Controls.Add(Me.txtstartdate)
        Me.Panel15.Controls.Add(Me.txtreason)
        Me.Panel15.Controls.Add(Me.txtendate)
        Me.Panel15.Controls.Add(Me.Label224)
        Me.Panel15.Controls.Add(Me.Label225)
        Me.Panel15.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel15.Location = New System.Drawing.Point(0, 25)
        Me.Panel15.Name = "Panel15"
        Me.Panel15.Size = New System.Drawing.Size(856, 527)
        Me.Panel15.TabIndex = 189
        '
        'Label345
        '
        Me.Label345.AutoSize = True
        Me.Label345.Location = New System.Drawing.Point(29, 233)
        Me.Label345.Name = "Label345"
        Me.Label345.Size = New System.Drawing.Size(40, 13)
        Me.Label345.TabIndex = 507
        Me.Label345.Text = "Status:"
        '
        'Label346
        '
        Me.Label346.AutoSize = True
        Me.Label346.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label346.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label346.Location = New System.Drawing.Point(67, 225)
        Me.Label346.Name = "Label346"
        Me.Label346.Size = New System.Drawing.Size(18, 24)
        Me.Label346.TabIndex = 508
        Me.Label346.Text = "*"
        '
        'cboleavestatus
        '
        Me.cboleavestatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboleavestatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboleavestatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboleavestatus.DropDownWidth = 150
        Me.cboleavestatus.FormattingEnabled = True
        Me.cboleavestatus.Location = New System.Drawing.Point(120, 226)
        Me.cboleavestatus.Name = "cboleavestatus"
        Me.cboleavestatus.Size = New System.Drawing.Size(103, 21)
        Me.cboleavestatus.TabIndex = 7
        '
        'Label324
        '
        Me.Label324.AutoSize = True
        Me.Label324.ForeColor = System.Drawing.Color.White
        Me.Label324.Location = New System.Drawing.Point(29, 605)
        Me.Label324.Name = "Label324"
        Me.Label324.Size = New System.Drawing.Size(25, 13)
        Me.Label324.TabIndex = 505
        Me.Label324.Text = "___"
        '
        'Label323
        '
        Me.Label323.AutoSize = True
        Me.Label323.ForeColor = System.Drawing.Color.White
        Me.Label323.Location = New System.Drawing.Point(847, 435)
        Me.Label323.Name = "Label323"
        Me.Label323.Size = New System.Drawing.Size(25, 13)
        Me.Label323.TabIndex = 504
        Me.Label323.Text = "___"
        '
        'dtpendate
        '
        Me.dtpendate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpendate.Location = New System.Drawing.Point(120, 148)
        Me.dtpendate.Name = "dtpendate"
        Me.dtpendate.Size = New System.Drawing.Size(103, 20)
        Me.dtpendate.TabIndex = 4
        '
        'dtpstartdate
        '
        Me.dtpstartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpstartdate.Location = New System.Drawing.Point(120, 122)
        Me.dtpstartdate.Name = "dtpstartdate"
        Me.dtpstartdate.Size = New System.Drawing.Size(103, 20)
        Me.dtpstartdate.TabIndex = 3
        '
        'pbEmpPicLeave
        '
        Me.pbEmpPicLeave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPicLeave.Location = New System.Drawing.Point(32, 8)
        Me.pbEmpPicLeave.Name = "pbEmpPicLeave"
        Me.pbEmpPicLeave.Size = New System.Drawing.Size(89, 77)
        Me.pbEmpPicLeave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPicLeave.TabIndex = 180
        Me.pbEmpPicLeave.TabStop = False
        '
        'cboleavetypes
        '
        Me.cboleavetypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboleavetypes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboleavetypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboleavetypes.DropDownWidth = 150
        Me.cboleavetypes.FormattingEnabled = True
        Me.cboleavetypes.Location = New System.Drawing.Point(120, 93)
        Me.cboleavetypes.Name = "cboleavetypes"
        Me.cboleavetypes.Size = New System.Drawing.Size(103, 21)
        Me.cboleavetypes.TabIndex = 2
        '
        'Label199
        '
        Me.Label199.AutoSize = True
        Me.Label199.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label199.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label199.Location = New System.Drawing.Point(82, 146)
        Me.Label199.Name = "Label199"
        Me.Label199.Size = New System.Drawing.Size(18, 24)
        Me.Label199.TabIndex = 187
        Me.Label199.Text = "*"
        '
        'TabControl4
        '
        Me.TabControl4.Controls.Add(Me.TabPage7)
        Me.TabControl4.Controls.Add(Me.TabPage6)
        Me.TabControl4.Controls.Add(Me.TabPage8)
        Me.TabControl4.Location = New System.Drawing.Point(507, 93)
        Me.TabControl4.Name = "TabControl4"
        Me.TabControl4.SelectedIndex = 0
        Me.TabControl4.Size = New System.Drawing.Size(291, 148)
        Me.TabControl4.TabIndex = 181
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.Label128)
        Me.TabPage7.Controls.Add(Me.Label131)
        Me.TabPage7.Controls.Add(Me.Label132)
        Me.TabPage7.Controls.Add(Me.txtmlbalLeave)
        Me.TabPage7.Controls.Add(Me.Label105)
        Me.TabPage7.Controls.Add(Me.txtvlbalLeave)
        Me.TabPage7.Controls.Add(Me.Label117)
        Me.TabPage7.Controls.Add(Me.Label118)
        Me.TabPage7.Controls.Add(Me.txtslbalLeave)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7.Size = New System.Drawing.Size(283, 122)
        Me.TabPage7.TabIndex = 1
        Me.TabPage7.Text = "Leave balance"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'Label128
        '
        Me.Label128.AutoSize = True
        Me.Label128.Location = New System.Drawing.Point(160, 65)
        Me.Label128.Name = "Label128"
        Me.Label128.Size = New System.Drawing.Size(39, 13)
        Me.Label128.TabIndex = 160
        Me.Label128.Text = "hour(s)"
        '
        'Label131
        '
        Me.Label131.AutoSize = True
        Me.Label131.Location = New System.Drawing.Point(160, 39)
        Me.Label131.Name = "Label131"
        Me.Label131.Size = New System.Drawing.Size(39, 13)
        Me.Label131.TabIndex = 161
        Me.Label131.Text = "hour(s)"
        '
        'Label132
        '
        Me.Label132.AutoSize = True
        Me.Label132.Location = New System.Drawing.Point(160, 13)
        Me.Label132.Name = "Label132"
        Me.Label132.Size = New System.Drawing.Size(39, 13)
        Me.Label132.TabIndex = 162
        Me.Label132.Text = "hour(s)"
        '
        'txtmlbalLeave
        '
        Me.txtmlbalLeave.BackColor = System.Drawing.Color.White
        Me.txtmlbalLeave.Location = New System.Drawing.Point(71, 57)
        Me.txtmlbalLeave.MaxLength = 50
        Me.txtmlbalLeave.Name = "txtmlbalLeave"
        Me.txtmlbalLeave.ReadOnly = True
        Me.txtmlbalLeave.Size = New System.Drawing.Size(84, 20)
        Me.txtmlbalLeave.TabIndex = 149
        '
        'Label105
        '
        Me.Label105.AutoSize = True
        Me.Label105.Location = New System.Drawing.Point(14, 65)
        Me.Label105.Name = "Label105"
        Me.Label105.Size = New System.Drawing.Size(50, 13)
        Me.Label105.TabIndex = 152
        Me.Label105.Text = "Maternity"
        '
        'txtvlbalLeave
        '
        Me.txtvlbalLeave.BackColor = System.Drawing.Color.White
        Me.txtvlbalLeave.Location = New System.Drawing.Point(70, 5)
        Me.txtvlbalLeave.MaxLength = 50
        Me.txtvlbalLeave.Name = "txtvlbalLeave"
        Me.txtvlbalLeave.ReadOnly = True
        Me.txtvlbalLeave.Size = New System.Drawing.Size(84, 20)
        Me.txtvlbalLeave.TabIndex = 147
        '
        'Label117
        '
        Me.Label117.AutoSize = True
        Me.Label117.Location = New System.Drawing.Point(15, 40)
        Me.Label117.Name = "Label117"
        Me.Label117.Size = New System.Drawing.Size(28, 13)
        Me.Label117.TabIndex = 151
        Me.Label117.Text = "Sick"
        '
        'Label118
        '
        Me.Label118.AutoSize = True
        Me.Label118.Location = New System.Drawing.Point(15, 13)
        Me.Label118.Name = "Label118"
        Me.Label118.Size = New System.Drawing.Size(49, 13)
        Me.Label118.TabIndex = 150
        Me.Label118.Text = "Vacation"
        '
        'txtslbalLeave
        '
        Me.txtslbalLeave.BackColor = System.Drawing.Color.White
        Me.txtslbalLeave.Location = New System.Drawing.Point(70, 31)
        Me.txtslbalLeave.MaxLength = 50
        Me.txtslbalLeave.Name = "txtslbalLeave"
        Me.txtslbalLeave.ReadOnly = True
        Me.txtslbalLeave.Size = New System.Drawing.Size(84, 20)
        Me.txtslbalLeave.TabIndex = 148
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.Label133)
        Me.TabPage6.Controls.Add(Me.Label136)
        Me.TabPage6.Controls.Add(Me.Label137)
        Me.TabPage6.Controls.Add(Me.txtmlallowleave)
        Me.TabPage6.Controls.Add(Me.txtvlallowLeave)
        Me.TabPage6.Controls.Add(Me.Label46)
        Me.TabPage6.Controls.Add(Me.txtslallowLeave)
        Me.TabPage6.Controls.Add(Me.Label47)
        Me.TabPage6.Controls.Add(Me.Label104)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(283, 122)
        Me.TabPage6.TabIndex = 0
        Me.TabPage6.Text = "Leave allowance"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'Label133
        '
        Me.Label133.AutoSize = True
        Me.Label133.Location = New System.Drawing.Point(205, 65)
        Me.Label133.Name = "Label133"
        Me.Label133.Size = New System.Drawing.Size(39, 13)
        Me.Label133.TabIndex = 160
        Me.Label133.Text = "hour(s)"
        '
        'Label136
        '
        Me.Label136.AutoSize = True
        Me.Label136.Location = New System.Drawing.Point(205, 39)
        Me.Label136.Name = "Label136"
        Me.Label136.Size = New System.Drawing.Size(39, 13)
        Me.Label136.TabIndex = 161
        Me.Label136.Text = "hour(s)"
        '
        'Label137
        '
        Me.Label137.AutoSize = True
        Me.Label137.Location = New System.Drawing.Point(205, 13)
        Me.Label137.Name = "Label137"
        Me.Label137.Size = New System.Drawing.Size(39, 13)
        Me.Label137.TabIndex = 162
        Me.Label137.Text = "hour(s)"
        '
        'txtmlallowleave
        '
        Me.txtmlallowleave.BackColor = System.Drawing.Color.White
        Me.txtmlallowleave.Location = New System.Drawing.Point(115, 57)
        Me.txtmlallowleave.MaxLength = 50
        Me.txtmlallowleave.Name = "txtmlallowleave"
        Me.txtmlallowleave.ReadOnly = True
        Me.txtmlallowleave.Size = New System.Drawing.Size(84, 20)
        Me.txtmlallowleave.TabIndex = 28
        '
        'txtvlallowLeave
        '
        Me.txtvlallowLeave.BackColor = System.Drawing.Color.White
        Me.txtvlallowLeave.Location = New System.Drawing.Point(115, 5)
        Me.txtvlallowLeave.MaxLength = 50
        Me.txtvlallowLeave.Name = "txtvlallowLeave"
        Me.txtvlallowLeave.ReadOnly = True
        Me.txtvlallowLeave.Size = New System.Drawing.Size(84, 20)
        Me.txtvlallowLeave.TabIndex = 26
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Location = New System.Drawing.Point(14, 13)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(49, 13)
        Me.Label46.TabIndex = 142
        Me.Label46.Text = "Vacation"
        '
        'txtslallowLeave
        '
        Me.txtslallowLeave.BackColor = System.Drawing.Color.White
        Me.txtslallowLeave.Location = New System.Drawing.Point(115, 31)
        Me.txtslallowLeave.MaxLength = 50
        Me.txtslallowLeave.Name = "txtslallowLeave"
        Me.txtslallowLeave.ReadOnly = True
        Me.txtslallowLeave.Size = New System.Drawing.Size(84, 20)
        Me.txtslallowLeave.TabIndex = 27
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Location = New System.Drawing.Point(14, 39)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(28, 13)
        Me.Label47.TabIndex = 145
        Me.Label47.Text = "Sick"
        '
        'Label104
        '
        Me.Label104.AutoSize = True
        Me.Label104.Location = New System.Drawing.Point(14, 65)
        Me.Label104.Name = "Label104"
        Me.Label104.Size = New System.Drawing.Size(50, 13)
        Me.Label104.TabIndex = 146
        Me.Label104.Text = "Maternity"
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.Label127)
        Me.TabPage8.Controls.Add(Me.Label126)
        Me.TabPage8.Controls.Add(Me.Label123)
        Me.TabPage8.Controls.Add(Me.txtmlpaypLeave)
        Me.TabPage8.Controls.Add(Me.Label119)
        Me.TabPage8.Controls.Add(Me.txtvlpaypLeave)
        Me.TabPage8.Controls.Add(Me.Label121)
        Me.TabPage8.Controls.Add(Me.Label122)
        Me.TabPage8.Controls.Add(Me.txtslpaypLeave)
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage8.Size = New System.Drawing.Size(283, 122)
        Me.TabPage8.TabIndex = 2
        Me.TabPage8.Text = "Leave per pay period"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'Label127
        '
        Me.Label127.AutoSize = True
        Me.Label127.Location = New System.Drawing.Point(160, 65)
        Me.Label127.Name = "Label127"
        Me.Label127.Size = New System.Drawing.Size(39, 13)
        Me.Label127.TabIndex = 159
        Me.Label127.Text = "hour(s)"
        '
        'Label126
        '
        Me.Label126.AutoSize = True
        Me.Label126.Location = New System.Drawing.Point(160, 39)
        Me.Label126.Name = "Label126"
        Me.Label126.Size = New System.Drawing.Size(39, 13)
        Me.Label126.TabIndex = 159
        Me.Label126.Text = "hour(s)"
        '
        'Label123
        '
        Me.Label123.AutoSize = True
        Me.Label123.Location = New System.Drawing.Point(160, 13)
        Me.Label123.Name = "Label123"
        Me.Label123.Size = New System.Drawing.Size(39, 13)
        Me.Label123.TabIndex = 159
        Me.Label123.Text = "hour(s)"
        '
        'txtmlpaypLeave
        '
        Me.txtmlpaypLeave.BackColor = System.Drawing.Color.White
        Me.txtmlpaypLeave.Location = New System.Drawing.Point(71, 57)
        Me.txtmlpaypLeave.MaxLength = 50
        Me.txtmlpaypLeave.Name = "txtmlpaypLeave"
        Me.txtmlpaypLeave.ReadOnly = True
        Me.txtmlpaypLeave.Size = New System.Drawing.Size(84, 20)
        Me.txtmlpaypLeave.TabIndex = 155
        '
        'Label119
        '
        Me.Label119.AutoSize = True
        Me.Label119.Location = New System.Drawing.Point(14, 65)
        Me.Label119.Name = "Label119"
        Me.Label119.Size = New System.Drawing.Size(50, 13)
        Me.Label119.TabIndex = 158
        Me.Label119.Text = "Maternity"
        '
        'txtvlpaypLeave
        '
        Me.txtvlpaypLeave.BackColor = System.Drawing.Color.White
        Me.txtvlpaypLeave.Location = New System.Drawing.Point(70, 5)
        Me.txtvlpaypLeave.MaxLength = 50
        Me.txtvlpaypLeave.Name = "txtvlpaypLeave"
        Me.txtvlpaypLeave.ReadOnly = True
        Me.txtvlpaypLeave.Size = New System.Drawing.Size(84, 20)
        Me.txtvlpaypLeave.TabIndex = 153
        '
        'Label121
        '
        Me.Label121.AutoSize = True
        Me.Label121.Location = New System.Drawing.Point(36, 39)
        Me.Label121.Name = "Label121"
        Me.Label121.Size = New System.Drawing.Size(28, 13)
        Me.Label121.TabIndex = 157
        Me.Label121.Text = "Sick"
        '
        'Label122
        '
        Me.Label122.AutoSize = True
        Me.Label122.Location = New System.Drawing.Point(15, 13)
        Me.Label122.Name = "Label122"
        Me.Label122.Size = New System.Drawing.Size(49, 13)
        Me.Label122.TabIndex = 156
        Me.Label122.Text = "Vacation"
        '
        'txtslpaypLeave
        '
        Me.txtslpaypLeave.BackColor = System.Drawing.Color.White
        Me.txtslpaypLeave.Location = New System.Drawing.Point(70, 31)
        Me.txtslpaypLeave.MaxLength = 50
        Me.txtslpaypLeave.Name = "txtslpaypLeave"
        Me.txtslpaypLeave.ReadOnly = True
        Me.txtslpaypLeave.Size = New System.Drawing.Size(84, 20)
        Me.txtslpaypLeave.TabIndex = 154
        '
        'Label198
        '
        Me.Label198.AutoSize = True
        Me.Label198.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label198.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label198.Location = New System.Drawing.Point(82, 118)
        Me.Label198.Name = "Label198"
        Me.Label198.Size = New System.Drawing.Size(18, 24)
        Me.Label198.TabIndex = 186
        Me.Label198.Text = "*"
        '
        'txtFNameLeave
        '
        Me.txtFNameLeave.BackColor = System.Drawing.Color.White
        Me.txtFNameLeave.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFNameLeave.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFNameLeave.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFNameLeave.Location = New System.Drawing.Point(127, 22)
        Me.txtFNameLeave.MaxLength = 250
        Me.txtFNameLeave.Name = "txtFNameLeave"
        Me.txtFNameLeave.ReadOnly = True
        Me.txtFNameLeave.Size = New System.Drawing.Size(516, 28)
        Me.txtFNameLeave.TabIndex = 175
        '
        'Label197
        '
        Me.Label197.AutoSize = True
        Me.Label197.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label197.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label197.Location = New System.Drawing.Point(82, 195)
        Me.Label197.Name = "Label197"
        Me.Label197.Size = New System.Drawing.Size(18, 24)
        Me.Label197.TabIndex = 185
        Me.Label197.Text = "*"
        '
        'txtEmpIDLeave
        '
        Me.txtEmpIDLeave.BackColor = System.Drawing.Color.White
        Me.txtEmpIDLeave.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpIDLeave.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmpIDLeave.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpIDLeave.Location = New System.Drawing.Point(127, 49)
        Me.txtEmpIDLeave.MaxLength = 50
        Me.txtEmpIDLeave.Name = "txtEmpIDLeave"
        Me.txtEmpIDLeave.ReadOnly = True
        Me.txtEmpIDLeave.Size = New System.Drawing.Size(516, 22)
        Me.txtEmpIDLeave.TabIndex = 170
        '
        'Label196
        '
        Me.Label196.AutoSize = True
        Me.Label196.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label196.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label196.Location = New System.Drawing.Point(82, 172)
        Me.Label196.Name = "Label196"
        Me.Label196.Size = New System.Drawing.Size(18, 24)
        Me.Label196.TabIndex = 184
        Me.Label196.Text = "*"
        '
        'btnleavtyp
        '
        Me.btnleavtyp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnleavtyp.Location = New System.Drawing.Point(202, 296)
        Me.btnleavtyp.Name = "btnleavtyp"
        Me.btnleavtyp.Size = New System.Drawing.Size(21, 23)
        Me.btnleavtyp.TabIndex = 143
        Me.btnleavtyp.Text = "..."
        Me.btnleavtyp.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnleavtyp.UseVisualStyleBackColor = True
        Me.btnleavtyp.Visible = False
        '
        'Label195
        '
        Me.Label195.AutoSize = True
        Me.Label195.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label195.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label195.Location = New System.Drawing.Point(96, 94)
        Me.Label195.Name = "Label195"
        Me.Label195.Size = New System.Drawing.Size(18, 24)
        Me.Label195.TabIndex = 183
        Me.Label195.Text = "*"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(245, 175)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(51, 13)
        Me.Label32.TabIndex = 141
        Me.Label32.Text = "Comment"
        '
        'btndlleavefile
        '
        Me.btndlleavefile.Location = New System.Drawing.Point(649, 481)
        Me.btndlleavefile.Name = "btndlleavefile"
        Me.btndlleavefile.Size = New System.Drawing.Size(75, 21)
        Me.btndlleavefile.TabIndex = 182
        Me.btndlleavefile.Text = "Download"
        Me.btndlleavefile.UseVisualStyleBackColor = True
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(245, 101)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(44, 13)
        Me.Label33.TabIndex = 141
        Me.Label33.Text = "Reason"
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(766, 454)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(75, 21)
        Me.Button6.TabIndex = 138
        Me.Button6.Text = "Clear"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(29, 154)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(53, 13)
        Me.Label34.TabIndex = 141
        Me.Label34.Text = "End date:"
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(649, 454)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(75, 21)
        Me.Button7.TabIndex = 136
        Me.Button7.Text = "&Browse..."
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(29, 128)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(56, 13)
        Me.Label35.TabIndex = 141
        Me.Label35.Text = "Start date:"
        '
        'pbempleave
        '
        Me.pbempleave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbempleave.Location = New System.Drawing.Point(649, 257)
        Me.pbempleave.Name = "pbempleave"
        Me.pbempleave.Size = New System.Drawing.Size(192, 191)
        Me.pbempleave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbempleave.TabIndex = 1
        Me.pbempleave.TabStop = False
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(29, 102)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(63, 13)
        Me.Label36.TabIndex = 141
        Me.Label36.Text = "Leave type:"
        '
        'dgvempleave
        '
        Me.dgvempleave.AllowUserToDeleteRows = False
        Me.dgvempleave.AllowUserToOrderColumns = True
        Me.dgvempleave.AllowUserToResizeColumns = False
        Me.dgvempleave.AllowUserToResizeRows = False
        Me.dgvempleave.BackgroundColor = System.Drawing.Color.White
        Me.dgvempleave.ColumnHeadersHeight = 38
        Me.dgvempleave.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvempleave.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.elv_RowID, Me.elv_Type, Me.elv_StartTime, Me.elv_EndTime, Me.elv_StartDate, Me.elv_EndDate, Me.elv_Reason, Me.elv_Comment, Me.elv_Image, Me.elv_viewimage, Me.elv_attafilename, Me.elv_attafileextensn, Me.elv_Status})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempleave.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgvempleave.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempleave.Location = New System.Drawing.Point(32, 257)
        Me.dgvempleave.MultiSelect = False
        Me.dgvempleave.Name = "dgvempleave"
        Me.dgvempleave.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvempleave.Size = New System.Drawing.Size(593, 345)
        Me.dgvempleave.TabIndex = 0
        '
        'elv_RowID
        '
        Me.elv_RowID.HeaderText = "RowID"
        Me.elv_RowID.Name = "elv_RowID"
        Me.elv_RowID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.elv_RowID.Visible = False
        '
        'elv_Type
        '
        Me.elv_Type.HeaderText = "Leave type"
        Me.elv_Type.Name = "elv_Type"
        Me.elv_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.elv_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.elv_Type.Width = 150
        '
        'elv_StartTime
        '
        Me.elv_StartTime.HeaderText = "Start time"
        Me.elv_StartTime.Name = "elv_StartTime"
        Me.elv_StartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'elv_EndTime
        '
        Me.elv_EndTime.HeaderText = "End time"
        Me.elv_EndTime.Name = "elv_EndTime"
        Me.elv_EndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'elv_StartDate
        '
        '
        '
        '
        Me.elv_StartDate.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.elv_StartDate.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.elv_StartDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.elv_StartDate.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.elv_StartDate.HeaderText = "Start date"
        Me.elv_StartDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.elv_StartDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.elv_StartDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.elv_StartDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.elv_StartDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.elv_StartDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.elv_StartDate.MonthCalendar.DisplayMonth = New Date(2015, 5, 1, 0, 0, 0, 0)
        Me.elv_StartDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.elv_StartDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.elv_StartDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.elv_StartDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.elv_StartDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.elv_StartDate.Name = "elv_StartDate"
        Me.elv_StartDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.elv_StartDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'elv_EndDate
        '
        '
        '
        '
        Me.elv_EndDate.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.elv_EndDate.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.elv_EndDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.elv_EndDate.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.elv_EndDate.HeaderText = "End date"
        Me.elv_EndDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.elv_EndDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.elv_EndDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.elv_EndDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.elv_EndDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.elv_EndDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.elv_EndDate.MonthCalendar.DisplayMonth = New Date(2015, 5, 1, 0, 0, 0, 0)
        Me.elv_EndDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.elv_EndDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.elv_EndDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.elv_EndDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.elv_EndDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.elv_EndDate.Name = "elv_EndDate"
        Me.elv_EndDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.elv_EndDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'elv_Reason
        '
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.elv_Reason.DefaultCellStyle = DataGridViewCellStyle4
        Me.elv_Reason.HeaderText = "Reason"
        Me.elv_Reason.Name = "elv_Reason"
        Me.elv_Reason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.elv_Reason.Width = 190
        '
        'elv_Comment
        '
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.elv_Comment.DefaultCellStyle = DataGridViewCellStyle5
        Me.elv_Comment.HeaderText = "Comments"
        Me.elv_Comment.MaxInputLength = 499
        Me.elv_Comment.Name = "elv_Comment"
        Me.elv_Comment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.elv_Comment.Width = 190
        '
        'elv_Image
        '
        Me.elv_Image.HeaderText = "Image"
        Me.elv_Image.MaxInputLength = 1999
        Me.elv_Image.Name = "elv_Image"
        Me.elv_Image.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.elv_Image.Visible = False
        '
        'elv_viewimage
        '
        Me.elv_viewimage.HeaderText = ""
        Me.elv_viewimage.Name = "elv_viewimage"
        '
        'elv_attafilename
        '
        Me.elv_attafilename.HeaderText = "Attachment file name"
        Me.elv_attafilename.Name = "elv_attafilename"
        '
        'elv_attafileextensn
        '
        Me.elv_attafileextensn.HeaderText = "Attachment file extension"
        Me.elv_attafileextensn.Name = "elv_attafileextensn"
        '
        'elv_Status
        '
        Me.elv_Status.HeaderText = "Status"
        Me.elv_Status.Name = "elv_Status"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(29, 206)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(51, 13)
        Me.Label37.TabIndex = 140
        Me.Label37.Text = "End time:"
        '
        'txtstarttime
        '
        Me.txtstarttime.BackColor = System.Drawing.Color.White
        Me.txtstarttime.Location = New System.Drawing.Point(120, 174)
        Me.txtstarttime.Name = "txtstarttime"
        Me.txtstarttime.Size = New System.Drawing.Size(103, 20)
        Me.txtstarttime.TabIndex = 5
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(29, 180)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(54, 13)
        Me.Label38.TabIndex = 139
        Me.Label38.Text = "Start time:"
        '
        'txtendtime
        '
        Me.txtendtime.BackColor = System.Drawing.Color.White
        Me.txtendtime.Location = New System.Drawing.Point(120, 200)
        Me.txtendtime.Name = "txtendtime"
        Me.txtendtime.Size = New System.Drawing.Size(103, 20)
        Me.txtendtime.TabIndex = 6
        '
        'txtcomments
        '
        Me.txtcomments.BackColor = System.Drawing.Color.White
        Me.txtcomments.Location = New System.Drawing.Point(302, 162)
        Me.txtcomments.MaxLength = 2000
        Me.txtcomments.Multiline = True
        Me.txtcomments.Name = "txtcomments"
        Me.txtcomments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtcomments.Size = New System.Drawing.Size(190, 59)
        Me.txtcomments.TabIndex = 9
        '
        'txtstartdate
        '
        Me.txtstartdate.BackColor = System.Drawing.Color.White
        Me.txtstartdate.Location = New System.Drawing.Point(823, 126)
        Me.txtstartdate.Name = "txtstartdate"
        Me.txtstartdate.Size = New System.Drawing.Size(61, 20)
        Me.txtstartdate.TabIndex = 3
        Me.txtstartdate.Visible = False
        '
        'txtreason
        '
        Me.txtreason.BackColor = System.Drawing.Color.White
        Me.txtreason.Location = New System.Drawing.Point(302, 94)
        Me.txtreason.MaxLength = 500
        Me.txtreason.Multiline = True
        Me.txtreason.Name = "txtreason"
        Me.txtreason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtreason.Size = New System.Drawing.Size(190, 59)
        Me.txtreason.TabIndex = 8
        '
        'txtendate
        '
        Me.txtendate.BackColor = System.Drawing.Color.White
        Me.txtendate.Location = New System.Drawing.Point(823, 152)
        Me.txtendate.Name = "txtendate"
        Me.txtendate.Size = New System.Drawing.Size(61, 20)
        Me.txtendate.TabIndex = 4
        Me.txtendate.Visible = False
        '
        'Label224
        '
        Me.Label224.AutoSize = True
        Me.Label224.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label224.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label224.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label224.ImageIndex = 0
        Me.Label224.Location = New System.Drawing.Point(882, 129)
        Me.Label224.Name = "Label224"
        Me.Label224.Size = New System.Drawing.Size(19, 15)
        Me.Label224.TabIndex = 188
        Me.Label224.Text = "    "
        Me.Label224.Visible = False
        '
        'Label225
        '
        Me.Label225.AutoSize = True
        Me.Label225.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label225.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label225.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label225.ImageIndex = 0
        Me.Label225.Location = New System.Drawing.Point(204, 175)
        Me.Label225.Name = "Label225"
        Me.Label225.Size = New System.Drawing.Size(19, 15)
        Me.Label225.TabIndex = 189
        Me.Label225.Text = "    "
        '
        'LeaveTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel15)
        Me.Controls.Add(Me.ToolStrip5)
        Me.Name = "LeaveTab"
        Me.Size = New System.Drawing.Size(856, 552)
        Me.ToolStrip5.ResumeLayout(False)
        Me.ToolStrip5.PerformLayout()
        Me.Panel15.ResumeLayout(False)
        Me.Panel15.PerformLayout()
        CType(Me.pbEmpPicLeave, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl4.ResumeLayout(False)
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage7.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage8.PerformLayout()
        CType(Me.pbempleave, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvempleave, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip5 As ToolStrip
    Friend WithEvents tsbtnNewLeave As ToolStripButton
    Friend WithEvents tsbtnSaveLeave As ToolStripButton
    Friend WithEvents ToolStripButton3 As ToolStripButton
    Friend WithEvents ToolStripLabel10 As ToolStripLabel
    Friend WithEvents tsbtnDeletLeave As ToolStripButton
    Friend WithEvents ToolStripButton4 As ToolStripButton
    Friend WithEvents ToolStripButton17 As ToolStripButton
    Friend WithEvents ToolStripButton20 As ToolStripButton
    Friend WithEvents Panel15 As Panel
    Friend WithEvents Label345 As Label
    Friend WithEvents Label346 As Label
    Friend WithEvents cboleavestatus As ComboBox
    Friend WithEvents Label324 As Label
    Friend WithEvents Label323 As Label
    Friend WithEvents dtpendate As DateTimePicker
    Friend WithEvents dtpstartdate As DateTimePicker
    Friend WithEvents pbEmpPicLeave As PictureBox
    Friend WithEvents cboleavetypes As ComboBox
    Friend WithEvents Label199 As Label
    Friend WithEvents TabControl4 As TabControl
    Friend WithEvents TabPage7 As TabPage
    Friend WithEvents Label128 As Label
    Friend WithEvents Label131 As Label
    Friend WithEvents Label132 As Label
    Friend WithEvents txtmlbalLeave As TextBox
    Friend WithEvents Label105 As Label
    Friend WithEvents txtvlbalLeave As TextBox
    Friend WithEvents Label117 As Label
    Friend WithEvents Label118 As Label
    Friend WithEvents txtslbalLeave As TextBox
    Friend WithEvents TabPage6 As TabPage
    Friend WithEvents Label133 As Label
    Friend WithEvents Label136 As Label
    Friend WithEvents Label137 As Label
    Friend WithEvents txtmlallowleave As TextBox
    Friend WithEvents txtvlallowLeave As TextBox
    Friend WithEvents Label46 As Label
    Friend WithEvents txtslallowLeave As TextBox
    Friend WithEvents Label47 As Label
    Friend WithEvents Label104 As Label
    Friend WithEvents TabPage8 As TabPage
    Friend WithEvents Label127 As Label
    Friend WithEvents Label126 As Label
    Friend WithEvents Label123 As Label
    Friend WithEvents txtmlpaypLeave As TextBox
    Friend WithEvents Label119 As Label
    Friend WithEvents txtvlpaypLeave As TextBox
    Friend WithEvents Label121 As Label
    Friend WithEvents Label122 As Label
    Friend WithEvents txtslpaypLeave As TextBox
    Friend WithEvents Label198 As Label
    Friend WithEvents txtFNameLeave As TextBox
    Friend WithEvents Label197 As Label
    Friend WithEvents txtEmpIDLeave As TextBox
    Friend WithEvents Label196 As Label
    Friend WithEvents btnleavtyp As Button
    Friend WithEvents Label195 As Label
    Friend WithEvents Label32 As Label
    Friend WithEvents btndlleavefile As Button
    Friend WithEvents Label33 As Label
    Friend WithEvents Button6 As Button
    Friend WithEvents Label34 As Label
    Friend WithEvents Button7 As Button
    Friend WithEvents Label35 As Label
    Friend WithEvents pbempleave As PictureBox
    Friend WithEvents Label36 As Label
    Friend WithEvents dgvempleave As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents elv_RowID As DataGridViewTextBoxColumn
    Friend WithEvents elv_Type As DataGridViewTextBoxColumn
    Friend WithEvents elv_StartTime As DataGridViewTextBoxColumn
    Friend WithEvents elv_EndTime As DataGridViewTextBoxColumn
    Friend WithEvents elv_StartDate As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents elv_EndDate As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents elv_Reason As DataGridViewTextBoxColumn
    Friend WithEvents elv_Comment As DataGridViewTextBoxColumn
    Friend WithEvents elv_Image As DataGridViewTextBoxColumn
    Friend WithEvents elv_viewimage As DataGridViewButtonColumn
    Friend WithEvents elv_attafilename As DataGridViewTextBoxColumn
    Friend WithEvents elv_attafileextensn As DataGridViewTextBoxColumn
    Friend WithEvents elv_Status As DataGridViewTextBoxColumn
    Friend WithEvents Label37 As Label
    Friend WithEvents txtstarttime As TextBox
    Friend WithEvents Label38 As Label
    Friend WithEvents txtendtime As TextBox
    Friend WithEvents txtcomments As TextBox
    Friend WithEvents txtstartdate As TextBox
    Friend WithEvents txtreason As TextBox
    Friend WithEvents txtendate As TextBox
    Friend WithEvents Label224 As Label
    Friend WithEvents Label225 As Label
End Class
