<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BonusGenerator
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BonusGenerator))
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblYear = New System.Windows.Forms.Label()
        Me.linkPrevs = New System.Windows.Forms.LinkLabel()
        Me.linkNxt = New System.Windows.Forms.LinkLabel()
        Me.dgvPayPeriodList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Col1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.dgvEmployeeList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lnklblFirst = New System.Windows.Forms.LinkLabel()
        Me.lnklblPrev = New System.Windows.Forms.LinkLabel()
        Me.lnklblLast = New System.Windows.Forms.LinkLabel()
        Me.lnklblNext = New System.Windows.Forms.LinkLabel()
        Me.txtTotalBonus = New System.Windows.Forms.TextBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnBonusGen = New System.Windows.Forms.ToolStripButton()
        Me.tsbntClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.tstxboxSearch = New System.Windows.Forms.ToolStripTextBox()
        Me.tsbtnSearch = New System.Windows.Forms.ToolStripButton()
        Me.tsProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnDelEmpPayrollBonus = New System.Windows.Forms.ToolStripButton()
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
        Me.bgworkBonusGenerate = New System.ComponentModel.BackgroundWorker()
        Me.bgworkBonusPreparator = New System.ComponentModel.BackgroundWorker()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtTotalLoan = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.dgvPayPeriodList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dgvEmployeeList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(1218, 21)
        Me.Label25.TabIndex = 140
        Me.Label25.Text = "BONUS"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.dgvPayPeriodList)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 21)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(254, 465)
        Me.Panel1.TabIndex = 141
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.Controls.Add(Me.lblYear)
        Me.Panel4.Controls.Add(Me.linkPrevs)
        Me.Panel4.Controls.Add(Me.linkNxt)
        Me.Panel4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.Panel4.Location = New System.Drawing.Point(12, 445)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(236, 15)
        Me.Panel4.TabIndex = 285
        '
        'lblYear
        '
        Me.lblYear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.lblYear.ForeColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblYear.Location = New System.Drawing.Point(38, 0)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(159, 15)
        Me.lblYear.TabIndex = 3
        Me.lblYear.Text = "Label1"
        Me.lblYear.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'linkPrevs
        '
        Me.linkPrevs.AutoSize = True
        Me.linkPrevs.Dock = System.Windows.Forms.DockStyle.Left
        Me.linkPrevs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkPrevs.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.linkPrevs.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkPrevs.Location = New System.Drawing.Point(0, 0)
        Me.linkPrevs.Name = "linkPrevs"
        Me.linkPrevs.Size = New System.Drawing.Size(38, 15)
        Me.linkPrevs.TabIndex = 1
        Me.linkPrevs.TabStop = True
        Me.linkPrevs.Text = "<Prev"
        '
        'linkNxt
        '
        Me.linkNxt.AutoSize = True
        Me.linkNxt.Dock = System.Windows.Forms.DockStyle.Right
        Me.linkNxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkNxt.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.linkNxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkNxt.Location = New System.Drawing.Point(197, 0)
        Me.linkNxt.Name = "linkNxt"
        Me.linkNxt.Size = New System.Drawing.Size(39, 15)
        Me.linkNxt.TabIndex = 2
        Me.linkNxt.TabStop = True
        Me.linkNxt.Text = "Next>"
        '
        'dgvPayPeriodList
        '
        Me.dgvPayPeriodList.AllowUserToAddRows = False
        Me.dgvPayPeriodList.AllowUserToDeleteRows = False
        Me.dgvPayPeriodList.AllowUserToOrderColumns = True
        Me.dgvPayPeriodList.AllowUserToResizeRows = False
        Me.dgvPayPeriodList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPayPeriodList.BackgroundColor = System.Drawing.Color.White
        Me.dgvPayPeriodList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Col1, Me.Col2, Me.Col3, Me.Col4, Me.Col5, Me.Col6, Me.Col7, Me.Col8, Me.Col9, Me.Col10, Me.Col11, Me.Col12, Me.Col13, Me.Col14, Me.Col15, Me.Col16})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPayPeriodList.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvPayPeriodList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvPayPeriodList.Location = New System.Drawing.Point(12, 107)
        Me.dgvPayPeriodList.Name = "dgvPayPeriodList"
        Me.dgvPayPeriodList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvPayPeriodList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPayPeriodList.Size = New System.Drawing.Size(236, 332)
        Me.dgvPayPeriodList.TabIndex = 1
        '
        'Col1
        '
        Me.Col1.HeaderText = "Column1"
        Me.Col1.Name = "Col1"
        '
        'Col2
        '
        Me.Col2.HeaderText = "Column2"
        Me.Col2.Name = "Col2"
        '
        'Col3
        '
        Me.Col3.HeaderText = "Column3"
        Me.Col3.Name = "Col3"
        '
        'Col4
        '
        Me.Col4.HeaderText = "Column4"
        Me.Col4.Name = "Col4"
        '
        'Col5
        '
        Me.Col5.HeaderText = "Column5"
        Me.Col5.Name = "Col5"
        '
        'Col6
        '
        Me.Col6.HeaderText = "Column13"
        Me.Col6.Name = "Col6"
        '
        'Col7
        '
        Me.Col7.HeaderText = "Column14"
        Me.Col7.Name = "Col7"
        '
        'Col8
        '
        Me.Col8.HeaderText = "Column15"
        Me.Col8.Name = "Col8"
        '
        'Col9
        '
        Me.Col9.HeaderText = "Column16"
        Me.Col9.Name = "Col9"
        '
        'Col10
        '
        Me.Col10.HeaderText = "Column17"
        Me.Col10.Name = "Col10"
        '
        'Col11
        '
        Me.Col11.HeaderText = "Column18"
        Me.Col11.Name = "Col11"
        '
        'Col12
        '
        Me.Col12.HeaderText = "Column19"
        Me.Col12.Name = "Col12"
        '
        'Col13
        '
        Me.Col13.HeaderText = "Column20"
        Me.Col13.Name = "Col13"
        '
        'Col14
        '
        Me.Col14.HeaderText = "Column21"
        Me.Col14.Name = "Col14"
        '
        'Col15
        '
        Me.Col15.HeaderText = "Column22"
        Me.Col15.Name = "Col15"
        '
        'Col16
        '
        Me.Col16.HeaderText = "Column23"
        Me.Col16.Name = "Col16"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.SplitContainer1)
        Me.Panel2.Controls.Add(Me.ToolStrip1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(254, 21)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(964, 465)
        Me.Panel2.TabIndex = 142
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 25)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.dgvEmployeeList)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel3)
        Me.SplitContainer1.Panel1MinSize = 256
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTotalLoan)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTotalBonus)
        Me.SplitContainer1.Size = New System.Drawing.Size(964, 440)
        Me.SplitContainer1.SplitterDistance = 256
        Me.SplitContainer1.SplitterWidth = 7
        Me.SplitContainer1.TabIndex = 1
        '
        'dgvEmployeeList
        '
        Me.dgvEmployeeList.AllowUserToAddRows = False
        Me.dgvEmployeeList.AllowUserToDeleteRows = False
        Me.dgvEmployeeList.AllowUserToOrderColumns = True
        Me.dgvEmployeeList.AllowUserToResizeRows = False
        Me.dgvEmployeeList.BackgroundColor = System.Drawing.Color.White
        Me.dgvEmployeeList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvEmployeeList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column10, Me.Column11, Me.Column12})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEmployeeList.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvEmployeeList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEmployeeList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEmployeeList.Location = New System.Drawing.Point(0, 0)
        Me.dgvEmployeeList.MultiSelect = False
        Me.dgvEmployeeList.Name = "dgvEmployeeList"
        Me.dgvEmployeeList.ReadOnly = True
        Me.dgvEmployeeList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvEmployeeList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmployeeList.Size = New System.Drawing.Size(962, 239)
        Me.dgvEmployeeList.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = "Column1"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.HeaderText = "Column2"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.HeaderText = "Column3"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column4
        '
        Me.Column4.HeaderText = "Column4"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column5
        '
        Me.Column5.HeaderText = "Column5"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column6
        '
        Me.Column6.HeaderText = "Column6"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'Column7
        '
        Me.Column7.HeaderText = "Column7"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        '
        'Column8
        '
        Me.Column8.HeaderText = "Column8"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        '
        'Column9
        '
        Me.Column9.HeaderText = "Column9"
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        '
        'Column10
        '
        Me.Column10.HeaderText = "Column10"
        Me.Column10.Name = "Column10"
        Me.Column10.ReadOnly = True
        '
        'Column11
        '
        Me.Column11.HeaderText = "Column11"
        Me.Column11.Name = "Column11"
        Me.Column11.ReadOnly = True
        '
        'Column12
        '
        Me.Column12.HeaderText = "Column12"
        Me.Column12.Name = "Column12"
        Me.Column12.ReadOnly = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lnklblFirst)
        Me.Panel3.Controls.Add(Me.lnklblPrev)
        Me.Panel3.Controls.Add(Me.lnklblLast)
        Me.Panel3.Controls.Add(Me.lnklblNext)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 239)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(962, 15)
        Me.Panel3.TabIndex = 0
        '
        'lnklblFirst
        '
        Me.lnklblFirst.AutoSize = True
        Me.lnklblFirst.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblFirst.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblFirst.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblFirst.Location = New System.Drawing.Point(0, 0)
        Me.lnklblFirst.Name = "lnklblFirst"
        Me.lnklblFirst.Size = New System.Drawing.Size(44, 15)
        Me.lnklblFirst.TabIndex = 279
        Me.lnklblFirst.TabStop = True
        Me.lnklblFirst.Text = "<<First"
        '
        'lnklblPrev
        '
        Me.lnklblPrev.AutoSize = True
        Me.lnklblPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblPrev.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblPrev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblPrev.Location = New System.Drawing.Point(50, 0)
        Me.lnklblPrev.Name = "lnklblPrev"
        Me.lnklblPrev.Size = New System.Drawing.Size(38, 15)
        Me.lnklblPrev.TabIndex = 280
        Me.lnklblPrev.TabStop = True
        Me.lnklblPrev.Text = "<Prev"
        '
        'lnklblLast
        '
        Me.lnklblLast.AutoSize = True
        Me.lnklblLast.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblLast.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblLast.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblLast.Location = New System.Drawing.Point(139, 0)
        Me.lnklblLast.Name = "lnklblLast"
        Me.lnklblLast.Size = New System.Drawing.Size(44, 15)
        Me.lnklblLast.TabIndex = 282
        Me.lnklblLast.TabStop = True
        Me.lnklblLast.Text = "Last>>"
        '
        'lnklblNext
        '
        Me.lnklblNext.AutoSize = True
        Me.lnklblNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblNext.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblNext.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblNext.Location = New System.Drawing.Point(94, 0)
        Me.lnklblNext.Name = "lnklblNext"
        Me.lnklblNext.Size = New System.Drawing.Size(39, 15)
        Me.lnklblNext.TabIndex = 281
        Me.lnklblNext.TabStop = True
        Me.lnklblNext.Text = "Next>"
        '
        'txtTotalBonus
        '
        Me.txtTotalBonus.BackColor = System.Drawing.Color.White
        Me.txtTotalBonus.Location = New System.Drawing.Point(281, 87)
        Me.txtTotalBonus.Name = "txtTotalBonus"
        Me.txtTotalBonus.ReadOnly = True
        Me.txtTotalBonus.Size = New System.Drawing.Size(100, 20)
        Me.txtTotalBonus.TabIndex = 0
        Me.txtTotalBonus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnBonusGen, Me.tsbntClose, Me.ToolStripLabel1, Me.tstxboxSearch, Me.tsbtnSearch, Me.tsProgressBar, Me.ToolStripLabel2, Me.tsbtnDelEmpPayrollBonus})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(964, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnBonusGen
        '
        Me.tsbtnBonusGen.Image = CType(resources.GetObject("tsbtnBonusGen.Image"), System.Drawing.Image)
        Me.tsbtnBonusGen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnBonusGen.Name = "tsbtnBonusGen"
        Me.tsbtnBonusGen.Size = New System.Drawing.Size(111, 22)
        Me.tsbtnBonusGen.Text = "Ge&nerate period"
        '
        'tsbntClose
        '
        Me.tsbntClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbntClose.Image = Global.Acupay.My.Resources.Resources.Button_Delete_icon
        Me.tsbntClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbntClose.Name = "tsbntClose"
        Me.tsbntClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbntClose.Text = "Close"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(89, 22)
        '
        'tstxboxSearch
        '
        Me.tstxboxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tstxboxSearch.MaxLength = 20
        Me.tstxboxSearch.Name = "tstxboxSearch"
        Me.tstxboxSearch.Size = New System.Drawing.Size(150, 25)
        '
        'tsbtnSearch
        '
        Me.tsbtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnSearch.Image = Global.Acupay.My.Resources.Resources.magnifier_zoom
        Me.tsbtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSearch.Name = "tsbtnSearch"
        Me.tsbtnSearch.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnSearch.Text = "Search Employee"
        '
        'tsProgressBar
        '
        Me.tsProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsProgressBar.Name = "tsProgressBar"
        Me.tsProgressBar.Size = New System.Drawing.Size(100, 22)
        Me.tsProgressBar.Visible = False
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.AutoSize = False
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(89, 22)
        '
        'tsbtnDelEmpPayrollBonus
        '
        Me.tsbtnDelEmpPayrollBonus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnDelEmpPayrollBonus.Image = Global.Acupay.My.Resources.Resources.CLOSE_00
        Me.tsbtnDelEmpPayrollBonus.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDelEmpPayrollBonus.Name = "tsbtnDelEmpPayrollBonus"
        Me.tsbtnDelEmpPayrollBonus.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnDelEmpPayrollBonus.Text = "ToolStripButton1"
        Me.tsbtnDelEmpPayrollBonus.ToolTipText = "Deletes only the" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "selected bonus"
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Column1"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Column2"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "Column3"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "Column4"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "Column5"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.HeaderText = "Column6"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.HeaderText = "Column7"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.HeaderText = "Column8"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        '
        'DataGridViewTextBoxColumn14
        '
        Me.DataGridViewTextBoxColumn14.HeaderText = "Column9"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        '
        'DataGridViewTextBoxColumn15
        '
        Me.DataGridViewTextBoxColumn15.HeaderText = "Column10"
        Me.DataGridViewTextBoxColumn15.Name = "DataGridViewTextBoxColumn15"
        '
        'DataGridViewTextBoxColumn16
        '
        Me.DataGridViewTextBoxColumn16.HeaderText = "Column11"
        Me.DataGridViewTextBoxColumn16.Name = "DataGridViewTextBoxColumn16"
        '
        'DataGridViewTextBoxColumn17
        '
        Me.DataGridViewTextBoxColumn17.HeaderText = "Column12"
        Me.DataGridViewTextBoxColumn17.Name = "DataGridViewTextBoxColumn17"
        '
        'bgworkBonusGenerate
        '
        Me.bgworkBonusGenerate.WorkerReportsProgress = True
        Me.bgworkBonusGenerate.WorkerSupportsCancellation = True
        '
        'bgworkBonusPreparator
        '
        Me.bgworkBonusPreparator.WorkerReportsProgress = True
        Me.bgworkBonusPreparator.WorkerSupportsCancellation = True
        '
        'txtTotalLoan
        '
        Me.txtTotalLoan.BackColor = System.Drawing.Color.White
        Me.txtTotalLoan.Location = New System.Drawing.Point(281, 61)
        Me.txtTotalLoan.Name = "txtTotalLoan"
        Me.txtTotalLoan.ReadOnly = True
        Me.txtTotalLoan.Size = New System.Drawing.Size(100, 20)
        Me.txtTotalLoan.TabIndex = 0
        Me.txtTotalLoan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(97, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Total Bonus"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(97, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Total Loan"
        '
        'BonusGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1218, 486)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label25)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "BonusGenerator"
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        CType(Me.dgvPayPeriodList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dgvEmployeeList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnBonusGen As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbntClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents lnklblFirst As System.Windows.Forms.LinkLabel
    Friend WithEvents lnklblPrev As System.Windows.Forms.LinkLabel
    Friend WithEvents lnklblLast As System.Windows.Forms.LinkLabel
    Friend WithEvents lnklblNext As System.Windows.Forms.LinkLabel
    Friend WithEvents dgvEmployeeList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstxboxSearch As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tsbtnSearch As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvPayPeriodList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn15 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn16 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn17 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bgworkBonusGenerate As System.ComponentModel.BackgroundWorker
    Friend WithEvents tsProgressBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents Col1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col15 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col16 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bgworkBonusPreparator As System.ComponentModel.BackgroundWorker
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents linkNxt As System.Windows.Forms.LinkLabel
    Friend WithEvents linkPrevs As System.Windows.Forms.LinkLabel
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents txtTotalBonus As System.Windows.Forms.TextBox
    Friend WithEvents tsbtnDelEmpPayrollBonus As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtTotalLoan As TextBox
End Class
