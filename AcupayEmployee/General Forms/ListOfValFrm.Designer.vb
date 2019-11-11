<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListOfValFrm
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListOfValFrm))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgUnknown = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.gbUnknown = New System.Windows.Forms.GroupBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.errProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.pbClose = New System.Windows.Forms.PictureBox()
        Me.msCancel = New System.Windows.Forms.ToolStripMenuItem()
        Me.msClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.msAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.msSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.msNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabDetails = New System.Windows.Forms.TabPage()
        Me.gbListOfVal = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboParentValue = New System.Windows.Forms.ComboBox()
        Me.cboLOVType = New System.Windows.Forms.ComboBox()
        Me.txtDisplayValue = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.txtComments = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboStatus = New System.Windows.Forms.ComboBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip()
        Me.cmdFirst = New System.Windows.Forms.ToolStripButton()
        Me.cmdPrev = New System.Windows.Forms.ToolStripButton()
        Me.cmdNext = New System.Windows.Forms.ToolStripButton()
        Me.cmdLast = New System.Windows.Forms.ToolStripButton()
        Me.tsRefresh = New System.Windows.Forms.ToolStripButton()
        Me.dgListOfVal = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.l_no = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.l_type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblsavemsg = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.msMenu = New System.Windows.Forms.MenuStrip()
        Me.gbListOfValList = New System.Windows.Forms.GroupBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
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
        Me.lv_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lv_systemaccountflg = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lv_no = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lv_displayvalue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lv_parentvalue = New EWSoftware.ListControls.DataGridViewControls.AutoCompleteTextBoxColumn()
        Me.lv_comments = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lv_status = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.lv_type = New EWSoftware.ListControls.DataGridViewControls.AutoCompleteTextBoxColumn()
        Me.lv_systemaccount = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        CType(Me.dgUnknown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbUnknown.SuspendLayout()
        CType(Me.errProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbClose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDetails.SuspendLayout()
        Me.gbListOfVal.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.ToolStrip3.SuspendLayout()
        CType(Me.dgListOfVal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.msMenu.SuspendLayout()
        Me.gbListOfValList.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgUnknown
        '
        Me.dgUnknown.AllowUserToAddRows = False
        Me.dgUnknown.AllowUserToDeleteRows = False
        Me.dgUnknown.AllowUserToOrderColumns = True
        Me.dgUnknown.AllowUserToResizeRows = False
        Me.dgUnknown.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgUnknown.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgUnknown.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgUnknown.ColumnHeadersHeight = 34
        Me.dgUnknown.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.lv_rowid, Me.lv_systemaccountflg, Me.lv_no, Me.lv_displayvalue, Me.lv_parentvalue, Me.lv_comments, Me.lv_status, Me.lv_type, Me.lv_systemaccount})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgUnknown.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgUnknown.GridColor = System.Drawing.Color.FromArgb(CType(CType(218, Byte), Integer), CType(CType(225, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.dgUnknown.Location = New System.Drawing.Point(13, 23)
        Me.dgUnknown.MultiSelect = False
        Me.dgUnknown.Name = "dgUnknown"
        Me.dgUnknown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgUnknown.Size = New System.Drawing.Size(775, 300)
        Me.dgUnknown.TabIndex = 241
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.BackColor = System.Drawing.Color.White
        Me.Label23.Font = New System.Drawing.Font("Cambria", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.Label23.Location = New System.Drawing.Point(6, -1)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(125, 17)
        Me.Label23.TabIndex = 240
        Me.Label23.Text = "List Of Value List:"
        '
        'gbUnknown
        '
        Me.gbUnknown.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbUnknown.Controls.Add(Me.dgUnknown)
        Me.gbUnknown.Controls.Add(Me.Label23)
        Me.gbUnknown.Location = New System.Drawing.Point(6, 133)
        Me.gbUnknown.Name = "gbUnknown"
        Me.gbUnknown.Size = New System.Drawing.Size(800, 336)
        Me.gbUnknown.TabIndex = 222
        Me.gbUnknown.TabStop = False
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Cambria", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1210, 28)
        Me.lblTitle.TabIndex = 285
        Me.lblTitle.Text = "List Of Values"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'errProvider
        '
        Me.errProvider.ContainerControl = Me
        '
        'pbClose
        '
        Me.pbClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.pbClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbClose.Image = CType(resources.GetObject("pbClose.Image"), System.Drawing.Image)
        Me.pbClose.Location = New System.Drawing.Point(1183, 5)
        Me.pbClose.Name = "pbClose"
        Me.pbClose.Size = New System.Drawing.Size(19, 19)
        Me.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbClose.TabIndex = 287
        Me.pbClose.TabStop = False
        '
        'msCancel
        '
        Me.msCancel.Image = CType(resources.GetObject("msCancel.Image"), System.Drawing.Image)
        Me.msCancel.Name = "msCancel"
        Me.msCancel.Size = New System.Drawing.Size(71, 20)
        Me.msCancel.Text = "&Cancel"
        '
        'msClear
        '
        Me.msClear.Image = CType(resources.GetObject("msClear.Image"), System.Drawing.Image)
        Me.msClear.Name = "msClear"
        Me.msClear.Size = New System.Drawing.Size(62, 20)
        Me.msClear.Text = "&Clear"
        '
        'msAdd
        '
        Me.msAdd.Image = CType(resources.GetObject("msAdd.Image"), System.Drawing.Image)
        Me.msAdd.Name = "msAdd"
        Me.msAdd.Size = New System.Drawing.Size(57, 20)
        Me.msAdd.Text = "&Add"
        '
        'msSave
        '
        Me.msSave.Image = CType(resources.GetObject("msSave.Image"), System.Drawing.Image)
        Me.msSave.Name = "msSave"
        Me.msSave.Size = New System.Drawing.Size(59, 20)
        Me.msSave.Text = "&Save"
        '
        'msNew
        '
        Me.msNew.Image = CType(resources.GetObject("msNew.Image"), System.Drawing.Image)
        Me.msNew.Name = "msNew"
        Me.msNew.Size = New System.Drawing.Size(59, 20)
        Me.msNew.Text = "&New"
        '
        'tabDetails
        '
        Me.tabDetails.AutoScroll = True
        Me.tabDetails.Controls.Add(Me.gbListOfVal)
        Me.tabDetails.Controls.Add(Me.gbUnknown)
        Me.tabDetails.Location = New System.Drawing.Point(4, 4)
        Me.tabDetails.Name = "tabDetails"
        Me.tabDetails.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDetails.Size = New System.Drawing.Size(813, 475)
        Me.tabDetails.TabIndex = 0
        Me.tabDetails.Text = "L.O.V. Details"
        Me.tabDetails.UseVisualStyleBackColor = True
        '
        'gbListOfVal
        '
        Me.gbListOfVal.Controls.Add(Me.Label3)
        Me.gbListOfVal.Controls.Add(Me.Label1)
        Me.gbListOfVal.Controls.Add(Me.cboParentValue)
        Me.gbListOfVal.Controls.Add(Me.cboLOVType)
        Me.gbListOfVal.Controls.Add(Me.txtDisplayValue)
        Me.gbListOfVal.Controls.Add(Me.Label25)
        Me.gbListOfVal.Controls.Add(Me.txtComments)
        Me.gbListOfVal.Controls.Add(Me.Label9)
        Me.gbListOfVal.Controls.Add(Me.Label6)
        Me.gbListOfVal.Controls.Add(Me.cboStatus)
        Me.gbListOfVal.Controls.Add(Me.Label18)
        Me.gbListOfVal.Controls.Add(Me.Label17)
        Me.gbListOfVal.Controls.Add(Me.Label4)
        Me.gbListOfVal.Controls.Add(Me.Label8)
        Me.gbListOfVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbListOfVal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbListOfVal.Location = New System.Drawing.Point(6, 6)
        Me.gbListOfVal.Name = "gbListOfVal"
        Me.gbListOfVal.Size = New System.Drawing.Size(736, 121)
        Me.gbListOfVal.TabIndex = 223
        Me.gbListOfVal.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(91, 51)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(16, 20)
        Me.Label3.TabIndex = 275
        Me.Label3.Text = "*"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(91, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(16, 20)
        Me.Label1.TabIndex = 274
        Me.Label1.Text = "*"
        '
        'cboParentValue
        '
        Me.cboParentValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboParentValue.FormattingEnabled = True
        Me.cboParentValue.Location = New System.Drawing.Point(110, 81)
        Me.cboParentValue.Name = "cboParentValue"
        Me.cboParentValue.Size = New System.Drawing.Size(229, 20)
        Me.cboParentValue.TabIndex = 3
        '
        'cboLOVType
        '
        Me.cboLOVType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLOVType.FormattingEnabled = True
        Me.cboLOVType.Location = New System.Drawing.Point(110, 30)
        Me.cboLOVType.Name = "cboLOVType"
        Me.cboLOVType.Size = New System.Drawing.Size(229, 21)
        Me.cboLOVType.TabIndex = 1
        '
        'txtDisplayValue
        '
        Me.txtDisplayValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDisplayValue.Location = New System.Drawing.Point(110, 55)
        Me.txtDisplayValue.Name = "txtDisplayValue"
        Me.txtDisplayValue.Size = New System.Drawing.Size(229, 20)
        Me.txtDisplayValue.TabIndex = 2
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label25.Location = New System.Drawing.Point(12, 58)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(74, 13)
        Me.Label25.TabIndex = 273
        Me.Label25.Text = "Display Value:"
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(458, 29)
        Me.txtComments.MaxLength = 500
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(244, 46)
        Me.txtComments.TabIndex = 4
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(375, 32)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(59, 13)
        Me.Label9.TabIndex = 270
        Me.Label9.Text = "Comments:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(12, 84)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 13)
        Me.Label6.TabIndex = 246
        Me.Label6.Text = "Parent Value:"
        '
        'cboStatus
        '
        Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatus.FormattingEnabled = True
        Me.cboStatus.Items.AddRange(New Object() {"Yes", "No"})
        Me.cboStatus.Location = New System.Drawing.Point(458, 80)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(244, 21)
        Me.cboStatus.TabIndex = 5
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.Color.White
        Me.Label18.Font = New System.Drawing.Font("Cambria", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.Label18.Location = New System.Drawing.Point(9, -1)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(184, 17)
        Me.Label18.TabIndex = 238
        Me.Label18.Text = "List Of Value Information:"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.Red
        Me.Label17.Location = New System.Drawing.Point(438, 79)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(16, 20)
        Me.Label17.TabIndex = 234
        Me.Label17.Text = "*"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(375, 84)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 232
        Me.Label4.Text = "Status:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(12, 32)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(34, 13)
        Me.Label8.TabIndex = 177
        Me.Label8.Text = "Type:"
        '
        'tabMain
        '
        Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.tabMain.Controls.Add(Me.tabDetails)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.tabMain.ItemSize = New System.Drawing.Size(61, 23)
        Me.tabMain.Location = New System.Drawing.Point(0, 24)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(821, 506)
        Me.tabMain.TabIndex = 195
        '
        'ToolStrip3
        '
        Me.ToolStrip3.AutoSize = False
        Me.ToolStrip3.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip3.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmdFirst, Me.cmdPrev, Me.cmdNext, Me.cmdLast, Me.tsRefresh})
        Me.ToolStrip3.Location = New System.Drawing.Point(3, 16)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Size = New System.Drawing.Size(360, 22)
        Me.ToolStrip3.TabIndex = 215
        Me.ToolStrip3.Text = "toolbar1"
        '
        'cmdFirst
        '
        Me.cmdFirst.BackColor = System.Drawing.Color.White
        Me.cmdFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdFirst.Image = CType(resources.GetObject("cmdFirst.Image"), System.Drawing.Image)
        Me.cmdFirst.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.cmdFirst.Name = "cmdFirst"
        Me.cmdFirst.Size = New System.Drawing.Size(24, 19)
        Me.cmdFirst.Text = "First"
        '
        'cmdPrev
        '
        Me.cmdPrev.BackColor = System.Drawing.Color.White
        Me.cmdPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdPrev.Image = CType(resources.GetObject("cmdPrev.Image"), System.Drawing.Image)
        Me.cmdPrev.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.cmdPrev.Name = "cmdPrev"
        Me.cmdPrev.Size = New System.Drawing.Size(24, 19)
        Me.cmdPrev.Text = "Previous"
        '
        'cmdNext
        '
        Me.cmdNext.BackColor = System.Drawing.Color.White
        Me.cmdNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdNext.Image = CType(resources.GetObject("cmdNext.Image"), System.Drawing.Image)
        Me.cmdNext.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(24, 19)
        Me.cmdNext.Text = "Next"
        '
        'cmdLast
        '
        Me.cmdLast.BackColor = System.Drawing.Color.White
        Me.cmdLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdLast.Image = CType(resources.GetObject("cmdLast.Image"), System.Drawing.Image)
        Me.cmdLast.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.cmdLast.Name = "cmdLast"
        Me.cmdLast.Size = New System.Drawing.Size(24, 19)
        Me.cmdLast.Text = "Last"
        '
        'tsRefresh
        '
        Me.tsRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsRefresh.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.tsRefresh.Image = CType(resources.GetObject("tsRefresh.Image"), System.Drawing.Image)
        Me.tsRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsRefresh.Name = "tsRefresh"
        Me.tsRefresh.Size = New System.Drawing.Size(70, 19)
        Me.tsRefresh.Text = "&Refresh"
        '
        'dgListOfVal
        '
        Me.dgListOfVal.AllowUserToAddRows = False
        Me.dgListOfVal.AllowUserToDeleteRows = False
        Me.dgListOfVal.AllowUserToOrderColumns = True
        Me.dgListOfVal.AllowUserToResizeRows = False
        Me.dgListOfVal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgListOfVal.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgListOfVal.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgListOfVal.ColumnHeadersHeight = 34
        Me.dgListOfVal.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.l_no, Me.l_type})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgListOfVal.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgListOfVal.GridColor = System.Drawing.Color.FromArgb(CType(CType(218, Byte), Integer), CType(CType(225, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.dgListOfVal.Location = New System.Drawing.Point(8, 42)
        Me.dgListOfVal.MultiSelect = False
        Me.dgListOfVal.Name = "dgListOfVal"
        Me.dgListOfVal.ReadOnly = True
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgListOfVal.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgListOfVal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgListOfVal.Size = New System.Drawing.Size(350, 467)
        Me.dgListOfVal.TabIndex = 217
        '
        'l_no
        '
        Me.l_no.HeaderText = "Seq. No."
        Me.l_no.Name = "l_no"
        Me.l_no.ReadOnly = True
        Me.l_no.Width = 50
        '
        'l_type
        '
        Me.l_type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.l_type.HeaderText = "Type"
        Me.l_type.Name = "l_type"
        Me.l_type.ReadOnly = True
        Me.l_type.Width = 56
        '
        'lblsavemsg
        '
        Me.lblsavemsg.AutoSize = True
        Me.lblsavemsg.Location = New System.Drawing.Point(77, 5)
        Me.lblsavemsg.Name = "lblsavemsg"
        Me.lblsavemsg.Size = New System.Drawing.Size(0, 13)
        Me.lblsavemsg.TabIndex = 193
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Blue
        Me.Label12.Font = New System.Drawing.Font("Cambria", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label12.Location = New System.Drawing.Point(6, 1)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(142, 17)
        Me.Label12.TabIndex = 220
        Me.Label12.Text = "List Of Value Types:"
        '
        'msMenu
        '
        Me.msMenu.BackColor = System.Drawing.Color.Transparent
        Me.msMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.msNew, Me.msSave, Me.msAdd, Me.msClear, Me.msCancel})
        Me.msMenu.Location = New System.Drawing.Point(0, 0)
        Me.msMenu.Name = "msMenu"
        Me.msMenu.Size = New System.Drawing.Size(821, 24)
        Me.msMenu.TabIndex = 317
        '
        'gbListOfValList
        '
        Me.gbListOfValList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbListOfValList.BackColor = System.Drawing.Color.Transparent
        Me.gbListOfValList.Controls.Add(Me.Label12)
        Me.gbListOfValList.Controls.Add(Me.dgListOfVal)
        Me.gbListOfValList.Controls.Add(Me.ToolStrip3)
        Me.gbListOfValList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbListOfValList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbListOfValList.Location = New System.Drawing.Point(7, 5)
        Me.gbListOfValList.Name = "gbListOfValList"
        Me.gbListOfValList.Size = New System.Drawing.Size(366, 518)
        Me.gbListOfValList.TabIndex = 1
        Me.gbListOfValList.TabStop = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 28)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.Color.Blue
        Me.SplitContainer1.Panel1.Controls.Add(Me.gbListOfValList)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.tabMain)
        Me.SplitContainer1.Panel2.Controls.Add(Me.msMenu)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblsavemsg)
        Me.SplitContainer1.Size = New System.Drawing.Size(1210, 534)
        Me.SplitContainer1.SplitterDistance = 381
        Me.SplitContainer1.TabIndex = 286
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "rowid"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Visible = False
        Me.DataGridViewTextBoxColumn1.Width = 50
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.DataGridViewTextBoxColumn2.HeaderText = "street2"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Visible = False
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "barangay"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Visible = False
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "country"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Visible = False
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "zipcode"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Visible = False
        Me.DataGridViewTextBoxColumn5.Width = 50
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "comments"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.Visible = False
        Me.DataGridViewTextBoxColumn6.Width = 150
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Seq. No."
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.Width = 50
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.DataGridViewTextBoxColumn8.HeaderText = "Street Address 1"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "City/Town"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "Status"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.Width = 80
        '
        'lv_rowid
        '
        Me.lv_rowid.HeaderText = "rowid"
        Me.lv_rowid.Name = "lv_rowid"
        Me.lv_rowid.Visible = False
        '
        'lv_systemaccountflg
        '
        Me.lv_systemaccountflg.HeaderText = "systemaccountflg"
        Me.lv_systemaccountflg.Name = "lv_systemaccountflg"
        Me.lv_systemaccountflg.Visible = False
        '
        'lv_no
        '
        Me.lv_no.HeaderText = "Seq. No."
        Me.lv_no.Name = "lv_no"
        Me.lv_no.ReadOnly = True
        Me.lv_no.Width = 50
        '
        'lv_displayvalue
        '
        Me.lv_displayvalue.HeaderText = "Display Value"
        Me.lv_displayvalue.Name = "lv_displayvalue"
        Me.lv_displayvalue.Width = 150
        '
        'lv_parentvalue
        '
        Me.lv_parentvalue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.lv_parentvalue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.lv_parentvalue.HeaderText = "Parent Value"
        Me.lv_parentvalue.Name = "lv_parentvalue"
        Me.lv_parentvalue.Width = 120
        '
        'lv_comments
        '
        Me.lv_comments.HeaderText = "Comments"
        Me.lv_comments.Name = "lv_comments"
        '
        'lv_status
        '
        Me.lv_status.HeaderText = "Active"
        Me.lv_status.Items.AddRange(New Object() {"Yes", "No"})
        Me.lv_status.Name = "lv_status"
        '
        'lv_type
        '
        Me.lv_type.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.lv_type.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.lv_type.HeaderText = "Type"
        Me.lv_type.Name = "lv_type"
        Me.lv_type.ReadOnly = True
        '
        'lv_systemaccount
        '
        Me.lv_systemaccount.HeaderText = "System Account"
        Me.lv_systemaccount.Name = "lv_systemaccount"
        Me.lv_systemaccount.ReadOnly = True
        Me.lv_systemaccount.Visible = False
        Me.lv_systemaccount.Width = 60
        '
        'ListOfValFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1210, 562)
        Me.Controls.Add(Me.pbClose)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.lblTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ListOfValFrm"
        CType(Me.dgUnknown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbUnknown.ResumeLayout(False)
        Me.gbUnknown.PerformLayout()
        CType(Me.errProvider, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbClose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDetails.ResumeLayout(False)
        Me.gbListOfVal.ResumeLayout(False)
        Me.gbListOfVal.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        CType(Me.dgListOfVal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.msMenu.ResumeLayout(False)
        Me.msMenu.PerformLayout()
        Me.gbListOfValList.ResumeLayout(False)
        Me.gbListOfValList.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgUnknown As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents gbUnknown As System.Windows.Forms.GroupBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents errProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents pbClose As System.Windows.Forms.PictureBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents gbListOfValList As System.Windows.Forms.GroupBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents dgListOfVal As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents ToolStrip3 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsRefresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabDetails As System.Windows.Forms.TabPage
    Friend WithEvents msMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents msNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents msSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents msAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents msClear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents msCancel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblsavemsg As System.Windows.Forms.Label
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents gbListOfVal As System.Windows.Forms.GroupBox
    Friend WithEvents txtDisplayValue As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cboStatus As System.Windows.Forms.ComboBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cboLOVType As System.Windows.Forms.ComboBox
    Friend WithEvents l_no As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents l_type As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cboParentValue As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdFirst As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdPrev As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdNext As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdLast As System.Windows.Forms.ToolStripButton
    Friend WithEvents lv_rowid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lv_systemaccountflg As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lv_no As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lv_displayvalue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lv_parentvalue As EWSoftware.ListControls.DataGridViewControls.AutoCompleteTextBoxColumn
    Friend WithEvents lv_comments As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lv_status As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents lv_type As EWSoftware.ListControls.DataGridViewControls.AutoCompleteTextBoxColumn
    Friend WithEvents lv_systemaccount As System.Windows.Forms.DataGridViewCheckBoxColumn
End Class
