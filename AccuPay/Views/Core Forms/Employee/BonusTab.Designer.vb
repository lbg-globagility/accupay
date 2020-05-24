<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BonusTab
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BonusTab))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip20 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton11 = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnNewBon = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveBon = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancelBon = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel8 = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnDelBon = New System.Windows.Forms.ToolStripButton()
        Me.UserActivity = New System.Windows.Forms.ToolStripButton()
        Me.pbEmpPicBon = New System.Windows.Forms.PictureBox()
        Me.txtEmpIDBon = New System.Windows.Forms.TextBox()
        Me.txtFNameBon = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.dtpbonenddate = New System.Windows.Forms.DateTimePicker()
        Me.dtpbonstartdate = New System.Windows.Forms.DateTimePicker()
        Me.cbobontype = New System.Windows.Forms.ComboBox()
        Me.cbobonfreq = New System.Windows.Forms.ComboBox()
        Me.txtbonamt = New System.Windows.Forms.TextBox()
        Me.Label181 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.dgvempbon = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.bon_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_Frequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_Start = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.bon_End = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.bon_ProdID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip20.SuspendLayout()
        CType(Me.pbEmpPicBon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvempbon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip20
        '
        Me.ToolStrip20.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip20.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip20.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton11, Me.tsbtnNewBon, Me.tsbtnSaveBon, Me.tsbtnCancelBon, Me.ToolStripLabel8, Me.tsbtnDelBon, Me.UserActivity})
        Me.ToolStrip20.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip20.Name = "ToolStrip20"
        Me.ToolStrip20.Size = New System.Drawing.Size(856, 25)
        Me.ToolStrip20.TabIndex = 165
        Me.ToolStrip20.Text = "ToolStrip20"
        '
        'ToolStripButton11
        '
        Me.ToolStripButton11.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton11.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton11.Name = "ToolStripButton11"
        Me.ToolStripButton11.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton11.Text = "Close"
        '
        'tsbtnNewBon
        '
        Me.tsbtnNewBon.Image = Global.AccuPay.My.Resources.Resources._new
        Me.tsbtnNewBon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewBon.Name = "tsbtnNewBon"
        Me.tsbtnNewBon.Size = New System.Drawing.Size(87, 22)
        Me.tsbtnNewBon.Text = "&New Bonus"
        '
        'tsbtnSaveBon
        '
        Me.tsbtnSaveBon.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.tsbtnSaveBon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveBon.Name = "tsbtnSaveBon"
        Me.tsbtnSaveBon.Size = New System.Drawing.Size(87, 22)
        Me.tsbtnSaveBon.Text = "&Save Bonus"
        '
        'tsbtnCancelBon
        '
        Me.tsbtnCancelBon.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.tsbtnCancelBon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancelBon.Name = "tsbtnCancelBon"
        Me.tsbtnCancelBon.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancelBon.Text = "Cancel"
        '
        'ToolStripLabel8
        '
        Me.ToolStripLabel8.AutoSize = False
        Me.ToolStripLabel8.Name = "ToolStripLabel8"
        Me.ToolStripLabel8.Size = New System.Drawing.Size(50, 22)
        '
        'tsbtnDelBon
        '
        Me.tsbtnDelBon.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.tsbtnDelBon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDelBon.Name = "tsbtnDelBon"
        Me.tsbtnDelBon.Size = New System.Drawing.Size(96, 22)
        Me.tsbtnDelBon.Text = "Delete bonus"
        '
        'UserActivity
        '
        Me.UserActivity.Image = CType(resources.GetObject("UserActivity.Image"), System.Drawing.Image)
        Me.UserActivity.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.UserActivity.Name = "UserActivity"
        Me.UserActivity.Size = New System.Drawing.Size(93, 22)
        Me.UserActivity.Text = "User Activity"
        '
        'pbEmpPicBon
        '
        Me.pbEmpPicBon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPicBon.Location = New System.Drawing.Point(55, 50)
        Me.pbEmpPicBon.Name = "pbEmpPicBon"
        Me.pbEmpPicBon.Size = New System.Drawing.Size(89, 77)
        Me.pbEmpPicBon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPicBon.TabIndex = 341
        Me.pbEmpPicBon.TabStop = False
        '
        'txtEmpIDBon
        '
        Me.txtEmpIDBon.BackColor = System.Drawing.Color.White
        Me.txtEmpIDBon.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpIDBon.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmpIDBon.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpIDBon.Location = New System.Drawing.Point(171, 92)
        Me.txtEmpIDBon.MaxLength = 50
        Me.txtEmpIDBon.Name = "txtEmpIDBon"
        Me.txtEmpIDBon.ReadOnly = True
        Me.txtEmpIDBon.Size = New System.Drawing.Size(516, 22)
        Me.txtEmpIDBon.TabIndex = 342
        '
        'txtFNameBon
        '
        Me.txtFNameBon.BackColor = System.Drawing.Color.White
        Me.txtFNameBon.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFNameBon.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFNameBon.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFNameBon.Location = New System.Drawing.Point(171, 65)
        Me.txtFNameBon.MaxLength = 250
        Me.txtFNameBon.Name = "txtFNameBon"
        Me.txtFNameBon.ReadOnly = True
        Me.txtFNameBon.Size = New System.Drawing.Size(516, 28)
        Me.txtFNameBon.TabIndex = 343
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(71, 147)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 344
        Me.Label1.Text = "Type:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(71, 174)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 345
        Me.Label2.Text = "Frequency:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(71, 205)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 346
        Me.Label3.Text = "Start date:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(71, 231)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 347
        Me.Label4.Text = "End date:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(71, 254)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 13)
        Me.Label5.TabIndex = 348
        Me.Label5.Text = "Amount:"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.Location = New System.Drawing.Point(367, 144)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(28, 15)
        Me.LinkLabel1.TabIndex = 364
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Add"
        '
        'dtpbonenddate
        '
        Me.dtpbonenddate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpbonenddate.Location = New System.Drawing.Point(171, 222)
        Me.dtpbonenddate.Name = "dtpbonenddate"
        Me.dtpbonenddate.Size = New System.Drawing.Size(190, 20)
        Me.dtpbonenddate.TabIndex = 362
        '
        'dtpbonstartdate
        '
        Me.dtpbonstartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpbonstartdate.Location = New System.Drawing.Point(171, 196)
        Me.dtpbonstartdate.Name = "dtpbonstartdate"
        Me.dtpbonstartdate.Size = New System.Drawing.Size(190, 20)
        Me.dtpbonstartdate.TabIndex = 361
        '
        'cbobontype
        '
        Me.cbobontype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cbobontype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbobontype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbobontype.FormattingEnabled = True
        Me.cbobontype.Location = New System.Drawing.Point(171, 141)
        Me.cbobontype.Name = "cbobontype"
        Me.cbobontype.Size = New System.Drawing.Size(190, 21)
        Me.cbobontype.TabIndex = 359
        '
        'cbobonfreq
        '
        Me.cbobonfreq.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cbobonfreq.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbobonfreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbobonfreq.FormattingEnabled = True
        Me.cbobonfreq.Items.AddRange(New Object() {"One time"})
        Me.cbobonfreq.Location = New System.Drawing.Point(171, 168)
        Me.cbobonfreq.Name = "cbobonfreq"
        Me.cbobonfreq.Size = New System.Drawing.Size(190, 21)
        Me.cbobonfreq.TabIndex = 360
        '
        'txtbonamt
        '
        Me.txtbonamt.Location = New System.Drawing.Point(171, 248)
        Me.txtbonamt.Name = "txtbonamt"
        Me.txtbonamt.ShortcutsEnabled = False
        Me.txtbonamt.Size = New System.Drawing.Size(190, 20)
        Me.txtbonamt.TabIndex = 363
        Me.txtbonamt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label181
        '
        Me.Label181.AutoSize = True
        Me.Label181.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label181.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label181.Location = New System.Drawing.Point(106, 141)
        Me.Label181.Name = "Label181"
        Me.Label181.Size = New System.Drawing.Size(18, 24)
        Me.Label181.TabIndex = 365
        Me.Label181.Text = "*"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(134, 171)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(18, 24)
        Me.Label6.TabIndex = 366
        Me.Label6.Text = "*"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(134, 197)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(18, 24)
        Me.Label7.TabIndex = 367
        Me.Label7.Text = "*"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(134, 225)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(18, 24)
        Me.Label8.TabIndex = 368
        Me.Label8.Text = "*"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(113, 251)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(18, 24)
        Me.Label9.TabIndex = 369
        Me.Label9.Text = "*"
        '
        'dgvempbon
        '
        Me.dgvempbon.AllowUserToAddRows = False
        Me.dgvempbon.AllowUserToDeleteRows = False
        Me.dgvempbon.AllowUserToOrderColumns = True
        Me.dgvempbon.AllowUserToResizeRows = False
        Me.dgvempbon.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempbon.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvempbon.ColumnHeadersHeight = 34
        Me.dgvempbon.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.bon_RowID, Me.bon_Type, Me.bon_Amount, Me.bon_Frequency, Me.bon_Start, Me.bon_End, Me.bon_ProdID})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempbon.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvempbon.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempbon.Location = New System.Drawing.Point(27, 296)
        Me.dgvempbon.MultiSelect = False
        Me.dgvempbon.Name = "dgvempbon"
        Me.dgvempbon.ReadOnly = True
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempbon.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvempbon.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvempbon.Size = New System.Drawing.Size(782, 242)
        Me.dgvempbon.TabIndex = 370
        '
        'bon_RowID
        '
        Me.bon_RowID.DataPropertyName = "RowID"
        Me.bon_RowID.HeaderText = "RowID"
        Me.bon_RowID.Name = "bon_RowID"
        Me.bon_RowID.ReadOnly = True
        Me.bon_RowID.Visible = False
        Me.bon_RowID.Width = 50
        '
        'bon_Type
        '
        Me.bon_Type.DataPropertyName = "BonusType"
        Me.bon_Type.HeaderText = "Type"
        Me.bon_Type.Name = "bon_Type"
        Me.bon_Type.ReadOnly = True
        Me.bon_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.bon_Type.Width = 180
        '
        'bon_Amount
        '
        Me.bon_Amount.DataPropertyName = "BonusAmount"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.bon_Amount.DefaultCellStyle = DataGridViewCellStyle2
        Me.bon_Amount.HeaderText = "Amount"
        Me.bon_Amount.Name = "bon_Amount"
        Me.bon_Amount.ReadOnly = True
        Me.bon_Amount.Width = 180
        '
        'bon_Frequency
        '
        Me.bon_Frequency.DataPropertyName = "AllowanceFrequency"
        Me.bon_Frequency.HeaderText = "Frequency"
        Me.bon_Frequency.Name = "bon_Frequency"
        Me.bon_Frequency.ReadOnly = True
        Me.bon_Frequency.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.bon_Frequency.Width = 180
        '
        'bon_Start
        '
        '
        '
        '
        Me.bon_Start.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.bon_Start.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.bon_Start.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_Start.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.bon_Start.DataPropertyName = "EffectiveStartDate"
        Me.bon_Start.HeaderText = "Effective start date"
        Me.bon_Start.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.bon_Start.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.bon_Start.MonthCalendar.BackgroundStyle.Class = ""
        Me.bon_Start.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.bon_Start.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.bon_Start.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_Start.MonthCalendar.DisplayMonth = New Date(2015, 5, 1, 0, 0, 0, 0)
        Me.bon_Start.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.bon_Start.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.bon_Start.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.bon_Start.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_Start.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.bon_Start.Name = "bon_Start"
        Me.bon_Start.ReadOnly = True
        Me.bon_Start.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'bon_End
        '
        '
        '
        '
        Me.bon_End.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.bon_End.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.bon_End.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_End.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.bon_End.DataPropertyName = "EffectiveEndDate"
        Me.bon_End.HeaderText = "Effective end date"
        Me.bon_End.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.bon_End.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.bon_End.MonthCalendar.BackgroundStyle.Class = ""
        Me.bon_End.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.bon_End.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.bon_End.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_End.MonthCalendar.DisplayMonth = New Date(2015, 5, 1, 0, 0, 0, 0)
        Me.bon_End.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.bon_End.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.bon_End.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.bon_End.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_End.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.bon_End.Name = "bon_End"
        Me.bon_End.ReadOnly = True
        Me.bon_End.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'bon_ProdID
        '
        Me.bon_ProdID.DataPropertyName = "ProductID"
        Me.bon_ProdID.HeaderText = "ProductID"
        Me.bon_ProdID.Name = "bon_ProdID"
        Me.bon_ProdID.ReadOnly = True
        Me.bon_ProdID.Visible = False
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "RowID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Visible = False
        Me.DataGridViewTextBoxColumn1.Width = 50
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "BonusType"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Type"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn2.Width = 180
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "BonusAmount"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumn3.HeaderText = "Amount"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 180
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "AllowanceFrequency"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Frequency"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn4.Width = 180
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "ProductID"
        Me.DataGridViewTextBoxColumn5.HeaderText = "ProductID"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.Visible = False
        '
        'BonusTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.dgvempbon)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label181)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.dtpbonenddate)
        Me.Controls.Add(Me.dtpbonstartdate)
        Me.Controls.Add(Me.cbobontype)
        Me.Controls.Add(Me.cbobonfreq)
        Me.Controls.Add(Me.txtbonamt)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtEmpIDBon)
        Me.Controls.Add(Me.txtFNameBon)
        Me.Controls.Add(Me.pbEmpPicBon)
        Me.Controls.Add(Me.ToolStrip20)
        Me.Name = "BonusTab"
        Me.Size = New System.Drawing.Size(856, 552)
        Me.ToolStrip20.ResumeLayout(False)
        Me.ToolStrip20.PerformLayout()
        CType(Me.pbEmpPicBon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvempbon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip20 As ToolStrip
    Friend WithEvents ToolStripButton11 As ToolStripButton
    Friend WithEvents tsbtnNewBon As ToolStripButton
    Friend WithEvents tsbtnSaveBon As ToolStripButton
    Friend WithEvents tsbtnCancelBon As ToolStripButton
    Friend WithEvents ToolStripLabel8 As ToolStripLabel
    Friend WithEvents tsbtnDelBon As ToolStripButton
    Friend WithEvents pbEmpPicBon As PictureBox
    Friend WithEvents txtEmpIDBon As TextBox
    Friend WithEvents txtFNameBon As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents dtpbonenddate As DateTimePicker
    Friend WithEvents dtpbonstartdate As DateTimePicker
    Friend WithEvents cbobontype As ComboBox
    Friend WithEvents cbobonfreq As ComboBox
    Friend WithEvents txtbonamt As TextBox
    Friend WithEvents Label181 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents dgvempbon As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents bon_RowID As DataGridViewTextBoxColumn
    Friend WithEvents bon_Type As DataGridViewTextBoxColumn
    Friend WithEvents bon_Amount As DataGridViewTextBoxColumn
    Friend WithEvents bon_Frequency As DataGridViewTextBoxColumn
    Friend WithEvents bon_Start As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents bon_End As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents bon_ProdID As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents UserActivity As ToolStripButton
End Class
