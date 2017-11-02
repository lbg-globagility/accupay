<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AllowanceTab
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip17 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton7 = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnNewAllowance = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveAllowance = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton10 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton26 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel7 = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnDelAllowance = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnimportallowance = New System.Windows.Forms.ToolStripButton()
        Me.pbEmpPicAllow = New System.Windows.Forms.PictureBox()
        Me.txtEmpIDAllow = New System.Windows.Forms.TextBox()
        Me.txtFNameAllow = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label164 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label219 = New System.Windows.Forms.Label()
        Me.dtpallowenddate = New System.Windows.Forms.DateTimePicker()
        Me.dtpallowstartdate = New System.Windows.Forms.DateTimePicker()
        Me.cboallowtype = New System.Windows.Forms.ComboBox()
        Me.cboallowfreq = New System.Windows.Forms.ComboBox()
        Me.txtallowamt = New System.Windows.Forms.TextBox()
        Me.lnklbaddallowtype = New System.Windows.Forms.LinkLabel()
        Me.dgvempallowance = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.eall_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Type = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.eall_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Frequency = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.eall_Start = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.eall_End = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.allow_taxab = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_ProdID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip17.SuspendLayout()
        CType(Me.pbEmpPicAllow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvempallowance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip17
        '
        Me.ToolStrip17.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip17.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip17.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton7, Me.tsbtnNewAllowance, Me.tsbtnSaveAllowance, Me.ToolStripButton10, Me.ToolStripButton26, Me.ToolStripLabel7, Me.tsbtnDelAllowance, Me.tsbtnimportallowance})
        Me.ToolStrip17.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip17.Name = "ToolStrip17"
        Me.ToolStrip17.Size = New System.Drawing.Size(856, 25)
        Me.ToolStrip17.TabIndex = 165
        Me.ToolStrip17.Text = "ToolStrip17"
        '
        'ToolStripButton7
        '
        Me.ToolStripButton7.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton7.Image = Global.Acupay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton7.Name = "ToolStripButton7"
        Me.ToolStripButton7.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton7.Text = "Close"
        '
        'tsbtnNewAllowance
        '
        Me.tsbtnNewAllowance.Image = Global.Acupay.My.Resources.Resources._new
        Me.tsbtnNewAllowance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewAllowance.Name = "tsbtnNewAllowance"
        Me.tsbtnNewAllowance.Size = New System.Drawing.Size(109, 22)
        Me.tsbtnNewAllowance.Text = "&New Allowance"
        '
        'tsbtnSaveAllowance
        '
        Me.tsbtnSaveAllowance.Image = Global.Acupay.My.Resources.Resources.Save
        Me.tsbtnSaveAllowance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveAllowance.Name = "tsbtnSaveAllowance"
        Me.tsbtnSaveAllowance.Size = New System.Drawing.Size(109, 22)
        Me.tsbtnSaveAllowance.Text = "&Save Allowance"
        '
        'ToolStripButton10
        '
        Me.ToolStripButton10.Image = Global.Acupay.My.Resources.Resources.cancel1
        Me.ToolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton10.Name = "ToolStripButton10"
        Me.ToolStripButton10.Size = New System.Drawing.Size(63, 22)
        Me.ToolStripButton10.Text = "Cancel"
        '
        'ToolStripButton26
        '
        Me.ToolStripButton26.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton26.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton26.Image = Global.Acupay.My.Resources.Resources.audit_trail_icon
        Me.ToolStripButton26.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton26.Name = "ToolStripButton26"
        Me.ToolStripButton26.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton26.Text = "ToolStripButton1"
        Me.ToolStripButton26.ToolTipText = "Show audit trails"
        '
        'ToolStripLabel7
        '
        Me.ToolStripLabel7.AutoSize = False
        Me.ToolStripLabel7.Name = "ToolStripLabel7"
        Me.ToolStripLabel7.Size = New System.Drawing.Size(50, 22)
        '
        'tsbtnDelAllowance
        '
        Me.tsbtnDelAllowance.Image = Global.Acupay.My.Resources.Resources.CLOSE_00
        Me.tsbtnDelAllowance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDelAllowance.Name = "tsbtnDelAllowance"
        Me.tsbtnDelAllowance.Size = New System.Drawing.Size(116, 22)
        Me.tsbtnDelAllowance.Text = "Delete allowance"
        '
        'tsbtnimportallowance
        '
        Me.tsbtnimportallowance.Image = Global.Acupay.My.Resources.Resources.Add
        Me.tsbtnimportallowance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnimportallowance.Name = "tsbtnimportallowance"
        Me.tsbtnimportallowance.Size = New System.Drawing.Size(119, 22)
        Me.tsbtnimportallowance.Text = "Import allowance"
        '
        'pbEmpPicAllow
        '
        Me.pbEmpPicAllow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPicAllow.Location = New System.Drawing.Point(36, 51)
        Me.pbEmpPicAllow.Name = "pbEmpPicAllow"
        Me.pbEmpPicAllow.Size = New System.Drawing.Size(89, 77)
        Me.pbEmpPicAllow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPicAllow.TabIndex = 341
        Me.pbEmpPicAllow.TabStop = False
        '
        'txtEmpIDAllow
        '
        Me.txtEmpIDAllow.BackColor = System.Drawing.Color.White
        Me.txtEmpIDAllow.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpIDAllow.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmpIDAllow.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpIDAllow.Location = New System.Drawing.Point(160, 90)
        Me.txtEmpIDAllow.MaxLength = 50
        Me.txtEmpIDAllow.Name = "txtEmpIDAllow"
        Me.txtEmpIDAllow.ReadOnly = True
        Me.txtEmpIDAllow.Size = New System.Drawing.Size(516, 22)
        Me.txtEmpIDAllow.TabIndex = 342
        '
        'txtFNameAllow
        '
        Me.txtFNameAllow.BackColor = System.Drawing.Color.White
        Me.txtFNameAllow.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFNameAllow.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFNameAllow.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFNameAllow.Location = New System.Drawing.Point(160, 63)
        Me.txtFNameAllow.MaxLength = 250
        Me.txtFNameAllow.Name = "txtFNameAllow"
        Me.txtFNameAllow.ReadOnly = True
        Me.txtFNameAllow.Size = New System.Drawing.Size(516, 28)
        Me.txtFNameAllow.TabIndex = 343
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(59, 155)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 344
        Me.Label1.Text = "Type:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(59, 182)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 345
        Me.Label2.Text = "Frequency:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(59, 213)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 346
        Me.Label3.Text = "Start date:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(62, 239)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 347
        Me.Label4.Text = "End date:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(62, 262)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 13)
        Me.Label5.TabIndex = 348
        Me.Label5.Text = "Amount:"
        '
        'Label164
        '
        Me.Label164.AutoSize = True
        Me.Label164.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label164.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label164.Location = New System.Drawing.Point(94, 147)
        Me.Label164.Name = "Label164"
        Me.Label164.Size = New System.Drawing.Size(18, 24)
        Me.Label164.TabIndex = 349
        Me.Label164.Text = "*"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(125, 174)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(18, 24)
        Me.Label6.TabIndex = 350
        Me.Label6.Text = "*"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(112, 207)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(18, 24)
        Me.Label7.TabIndex = 351
        Me.Label7.Text = "*"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(112, 239)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(18, 24)
        Me.Label8.TabIndex = 352
        Me.Label8.Text = "*"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(112, 262)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(18, 24)
        Me.Label9.TabIndex = 353
        Me.Label9.Text = "*"
        '
        'Label219
        '
        Me.Label219.AutoSize = True
        Me.Label219.Location = New System.Drawing.Point(142, 262)
        Me.Label219.Name = "Label219"
        Me.Label219.Size = New System.Drawing.Size(14, 13)
        Me.Label219.TabIndex = 364
        Me.Label219.Text = "₱"
        '
        'dtpallowenddate
        '
        Me.dtpallowenddate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpallowenddate.Location = New System.Drawing.Point(160, 233)
        Me.dtpallowenddate.Name = "dtpallowenddate"
        Me.dtpallowenddate.Size = New System.Drawing.Size(190, 20)
        Me.dtpallowenddate.TabIndex = 362
        '
        'dtpallowstartdate
        '
        Me.dtpallowstartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpallowstartdate.Location = New System.Drawing.Point(160, 207)
        Me.dtpallowstartdate.Name = "dtpallowstartdate"
        Me.dtpallowstartdate.Size = New System.Drawing.Size(190, 20)
        Me.dtpallowstartdate.TabIndex = 361
        '
        'cboallowtype
        '
        Me.cboallowtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboallowtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboallowtype.FormattingEnabled = True
        Me.cboallowtype.Location = New System.Drawing.Point(160, 152)
        Me.cboallowtype.Name = "cboallowtype"
        Me.cboallowtype.Size = New System.Drawing.Size(190, 21)
        Me.cboallowtype.TabIndex = 359
        '
        'cboallowfreq
        '
        Me.cboallowfreq.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboallowfreq.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboallowfreq.FormattingEnabled = True
        Me.cboallowfreq.Location = New System.Drawing.Point(160, 179)
        Me.cboallowfreq.Name = "cboallowfreq"
        Me.cboallowfreq.Size = New System.Drawing.Size(190, 21)
        Me.cboallowfreq.TabIndex = 360
        '
        'txtallowamt
        '
        Me.txtallowamt.Location = New System.Drawing.Point(160, 259)
        Me.txtallowamt.Name = "txtallowamt"
        Me.txtallowamt.ShortcutsEnabled = False
        Me.txtallowamt.Size = New System.Drawing.Size(190, 20)
        Me.txtallowamt.TabIndex = 363
        Me.txtallowamt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lnklbaddallowtype
        '
        Me.lnklbaddallowtype.AutoSize = True
        Me.lnklbaddallowtype.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklbaddallowtype.Location = New System.Drawing.Point(356, 153)
        Me.lnklbaddallowtype.Name = "lnklbaddallowtype"
        Me.lnklbaddallowtype.Size = New System.Drawing.Size(28, 15)
        Me.lnklbaddallowtype.TabIndex = 365
        Me.lnklbaddallowtype.TabStop = True
        Me.lnklbaddallowtype.Text = "Add"
        '
        'dgvempallowance
        '
        Me.dgvempallowance.AllowUserToDeleteRows = False
        Me.dgvempallowance.AllowUserToOrderColumns = True
        Me.dgvempallowance.AllowUserToResizeColumns = False
        Me.dgvempallowance.AllowUserToResizeRows = False
        Me.dgvempallowance.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempallowance.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvempallowance.ColumnHeadersHeight = 34
        Me.dgvempallowance.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eall_RowID, Me.eall_Type, Me.eall_Amount, Me.eall_Frequency, Me.eall_Start, Me.eall_End, Me.allow_taxab, Me.eall_ProdID})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempallowance.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvempallowance.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempallowance.Location = New System.Drawing.Point(30, 307)
        Me.dgvempallowance.MultiSelect = False
        Me.dgvempallowance.Name = "dgvempallowance"
        Me.dgvempallowance.ReadOnly = True
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempallowance.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvempallowance.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvempallowance.Size = New System.Drawing.Size(793, 225)
        Me.dgvempallowance.TabIndex = 366
        '
        'eall_RowID
        '
        Me.eall_RowID.HeaderText = "RowID"
        Me.eall_RowID.Name = "eall_RowID"
        Me.eall_RowID.ReadOnly = True
        Me.eall_RowID.Visible = False
        Me.eall_RowID.Width = 50
        '
        'eall_Type
        '
        Me.eall_Type.HeaderText = "Type"
        Me.eall_Type.Name = "eall_Type"
        Me.eall_Type.ReadOnly = True
        Me.eall_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eall_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.eall_Type.Width = 180
        '
        'eall_Amount
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.eall_Amount.DefaultCellStyle = DataGridViewCellStyle2
        Me.eall_Amount.HeaderText = "Amount"
        Me.eall_Amount.Name = "eall_Amount"
        Me.eall_Amount.ReadOnly = True
        Me.eall_Amount.Width = 180
        '
        'eall_Frequency
        '
        Me.eall_Frequency.HeaderText = "Frequency"
        Me.eall_Frequency.Name = "eall_Frequency"
        Me.eall_Frequency.ReadOnly = True
        Me.eall_Frequency.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eall_Frequency.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.eall_Frequency.Width = 180
        '
        'eall_Start
        '
        '
        '
        '
        Me.eall_Start.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.eall_Start.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.eall_Start.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eall_Start.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.eall_Start.HeaderText = "Effective start date"
        Me.eall_Start.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.eall_Start.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eall_Start.MonthCalendar.BackgroundStyle.Class = ""
        Me.eall_Start.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.eall_Start.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.eall_Start.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eall_Start.MonthCalendar.DisplayMonth = New Date(2015, 5, 1, 0, 0, 0, 0)
        Me.eall_Start.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.eall_Start.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eall_Start.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.eall_Start.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eall_Start.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.eall_Start.Name = "eall_Start"
        Me.eall_Start.ReadOnly = True
        Me.eall_Start.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'eall_End
        '
        '
        '
        '
        Me.eall_End.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.eall_End.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.eall_End.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eall_End.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.eall_End.HeaderText = "Effective end date"
        Me.eall_End.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.eall_End.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eall_End.MonthCalendar.BackgroundStyle.Class = ""
        Me.eall_End.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.eall_End.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.eall_End.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eall_End.MonthCalendar.DisplayMonth = New Date(2015, 5, 1, 0, 0, 0, 0)
        Me.eall_End.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.eall_End.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eall_End.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.eall_End.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eall_End.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.eall_End.Name = "eall_End"
        Me.eall_End.ReadOnly = True
        Me.eall_End.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'allow_taxab
        '
        Me.allow_taxab.HeaderText = "Taxable"
        Me.allow_taxab.Name = "allow_taxab"
        Me.allow_taxab.ReadOnly = True
        Me.allow_taxab.Visible = False
        '
        'eall_ProdID
        '
        Me.eall_ProdID.HeaderText = "ProductID"
        Me.eall_ProdID.Name = "eall_ProdID"
        Me.eall_ProdID.ReadOnly = True
        Me.eall_ProdID.Visible = False
        '
        'AllowanceTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.dgvempallowance)
        Me.Controls.Add(Me.lnklbaddallowtype)
        Me.Controls.Add(Me.Label219)
        Me.Controls.Add(Me.dtpallowenddate)
        Me.Controls.Add(Me.dtpallowstartdate)
        Me.Controls.Add(Me.cboallowtype)
        Me.Controls.Add(Me.cboallowfreq)
        Me.Controls.Add(Me.txtallowamt)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label164)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtEmpIDAllow)
        Me.Controls.Add(Me.txtFNameAllow)
        Me.Controls.Add(Me.pbEmpPicAllow)
        Me.Controls.Add(Me.ToolStrip17)
        Me.Name = "AllowanceTab"
        Me.Size = New System.Drawing.Size(856, 552)
        Me.ToolStrip17.ResumeLayout(False)
        Me.ToolStrip17.PerformLayout()
        CType(Me.pbEmpPicAllow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvempallowance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip17 As ToolStrip
    Friend WithEvents ToolStripButton7 As ToolStripButton
    Friend WithEvents tsbtnNewAllowance As ToolStripButton
    Friend WithEvents tsbtnSaveAllowance As ToolStripButton
    Friend WithEvents ToolStripButton10 As ToolStripButton
    Friend WithEvents ToolStripButton26 As ToolStripButton
    Friend WithEvents ToolStripLabel7 As ToolStripLabel
    Friend WithEvents tsbtnDelAllowance As ToolStripButton
    Friend WithEvents tsbtnimportallowance As ToolStripButton
    Friend WithEvents pbEmpPicAllow As PictureBox
    Friend WithEvents txtEmpIDAllow As TextBox
    Friend WithEvents txtFNameAllow As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label164 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label219 As Label
    Friend WithEvents dtpallowenddate As DateTimePicker
    Friend WithEvents dtpallowstartdate As DateTimePicker
    Friend WithEvents cboallowtype As ComboBox
    Friend WithEvents cboallowfreq As ComboBox
    Friend WithEvents txtallowamt As TextBox
    Friend WithEvents lnklbaddallowtype As LinkLabel
    Friend WithEvents dgvempallowance As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents eall_RowID As DataGridViewTextBoxColumn
    Friend WithEvents eall_Type As DataGridViewComboBoxColumn
    Friend WithEvents eall_Amount As DataGridViewTextBoxColumn
    Friend WithEvents eall_Frequency As DataGridViewComboBoxColumn
    Friend WithEvents eall_Start As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents eall_End As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents allow_taxab As DataGridViewTextBoxColumn
    Friend WithEvents eall_ProdID As DataGridViewTextBoxColumn
End Class
