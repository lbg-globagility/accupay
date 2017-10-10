<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListOfValueForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListOfValueForm))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblRowID = New System.Windows.Forms.Label()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.txtParentLIC = New System.Windows.Forms.TextBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.grplistval = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtType = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtLIC = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDisplayval = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label212 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.dglistofval = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.dgvlistofvaltype = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel4 = New System.Windows.Forms.LinkLabel()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.bgworklistofvaltypes = New System.ComponentModel.BackgroundWorker()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnSearchNow = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.autcoListOfValType = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_display = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_lic = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_parentvalue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Active = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.c_description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_orderBy = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip1.SuspendLayout()
        Me.grplistval.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.dglistofval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvlistofvaltype, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblRowID
        '
        Me.lblRowID.AutoSize = True
        Me.lblRowID.Location = New System.Drawing.Point(11, 19)
        Me.lblRowID.Name = "lblRowID"
        Me.lblRowID.Size = New System.Drawing.Size(74, 13)
        Me.lblRowID.TabIndex = 21
        Me.lblRowID.Text = "Display Value:"
        '
        'cmbStatus
        '
        Me.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Items.AddRange(New Object() {"Yes", "No"})
        Me.cmbStatus.Location = New System.Drawing.Point(354, 48)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(145, 21)
        Me.cmbStatus.TabIndex = 12
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(354, 75)
        Me.txtDescription.MaxLength = 500
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(145, 45)
        Me.txtDescription.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(268, 75)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(63, 13)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Description:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(268, 49)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Status:"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(54, 5)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(356, 20)
        Me.txtSearch.TabIndex = 25
        '
        'txtParentLIC
        '
        Me.txtParentLIC.Location = New System.Drawing.Point(354, 23)
        Me.txtParentLIC.Name = "txtParentLIC"
        Me.txtParentLIC.Size = New System.Drawing.Size(145, 20)
        Me.txtParentLIC.TabIndex = 11
        '
        'ToolStrip1
        '
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnClose, Me.ToolStripSeparator1, Me.btnSave, Me.ToolStripSeparator2, Me.ToolStripLabel1, Me.btnDelete, Me.btnCancel, Me.tsAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(940, 25)
        Me.ToolStrip1.TabIndex = 60
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.GoldWingsPayrollSys.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(117, 22)
        Me.btnNew.Text = "&New List of value"
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "&Close"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(117, 22)
        Me.btnSave.Text = "&Save List of value"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(89, 22)
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(126, 22)
        Me.btnDelete.Text = "&Delete List of value"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'tsAudittrail
        '
        Me.tsAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsAudittrail.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.audit_trail_icon
        Me.tsAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsAudittrail.Name = "tsAudittrail"
        Me.tsAudittrail.Size = New System.Drawing.Size(23, 22)
        Me.tsAudittrail.ToolTipText = "Show audit trails"
        '
        'grplistval
        '
        Me.grplistval.Controls.Add(Me.lblRowID)
        Me.grplistval.Controls.Add(Me.cmbStatus)
        Me.grplistval.Controls.Add(Me.txtDescription)
        Me.grplistval.Controls.Add(Me.Label6)
        Me.grplistval.Controls.Add(Me.Label5)
        Me.grplistval.Controls.Add(Me.txtParentLIC)
        Me.grplistval.Controls.Add(Me.Label4)
        Me.grplistval.Controls.Add(Me.txtType)
        Me.grplistval.Controls.Add(Me.Label3)
        Me.grplistval.Controls.Add(Me.txtLIC)
        Me.grplistval.Controls.Add(Me.Label2)
        Me.grplistval.Controls.Add(Me.txtDisplayval)
        Me.grplistval.Controls.Add(Me.Label1)
        Me.grplistval.Controls.Add(Me.Label8)
        Me.grplistval.Controls.Add(Me.Label9)
        Me.grplistval.Controls.Add(Me.Label212)
        Me.grplistval.Enabled = False
        Me.grplistval.Location = New System.Drawing.Point(7, 27)
        Me.grplistval.Name = "grplistval"
        Me.grplistval.Size = New System.Drawing.Size(557, 131)
        Me.grplistval.TabIndex = 21
        Me.grplistval.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(268, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Parent Value:"
        '
        'txtType
        '
        Me.txtType.Location = New System.Drawing.Point(98, 72)
        Me.txtType.Name = "txtType"
        Me.txtType.Size = New System.Drawing.Size(145, 20)
        Me.txtType.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Type:"
        '
        'txtLIC
        '
        Me.txtLIC.Location = New System.Drawing.Point(98, 46)
        Me.txtLIC.Name = "txtLIC"
        Me.txtLIC.Size = New System.Drawing.Size(145, 20)
        Me.txtLIC.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "LIC:"
        '
        'txtDisplayval
        '
        Me.txtDisplayval.Location = New System.Drawing.Point(98, 20)
        Me.txtDisplayval.Name = "txtDisplayval"
        Me.txtDisplayval.Size = New System.Drawing.Size(145, 20)
        Me.txtDisplayval.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Display Value:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(81, 17)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(18, 24)
        Me.Label8.TabIndex = 361
        Me.Label8.Text = "*"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(337, 49)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(18, 24)
        Me.Label9.TabIndex = 362
        Me.Label9.Text = "*"
        '
        'Label212
        '
        Me.Label212.AutoSize = True
        Me.Label212.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label212.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label212.Location = New System.Drawing.Point(81, 71)
        Me.Label212.Name = "Label212"
        Me.Label212.Size = New System.Drawing.Size(18, 24)
        Me.Label212.TabIndex = 360
        Me.Label212.Text = "*"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(4, 12)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(44, 13)
        Me.Label7.TabIndex = 67
        Me.Label7.Text = "Search:"
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.lblSaveMsg)
        Me.Panel2.Controls.Add(Me.ToolStrip1)
        Me.Panel2.Controls.Add(Me.grplistval)
        Me.Panel2.Location = New System.Drawing.Point(252, 27)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(942, 166)
        Me.Panel2.TabIndex = 66
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.Location = New System.Drawing.Point(129, 27)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(0, 13)
        Me.lblSaveMsg.TabIndex = 22
        '
        'dglistofval
        '
        Me.dglistofval.AllowUserToAddRows = False
        Me.dglistofval.AllowUserToDeleteRows = False
        Me.dglistofval.AllowUserToResizeRows = False
        Me.dglistofval.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dglistofval.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dglistofval.BackgroundColor = System.Drawing.Color.White
        Me.dglistofval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dglistofval.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_rowid, Me.c_display, Me.c_lic, Me.c_Type, Me.c_parentvalue, Me.c_Active, Me.c_description, Me.c_orderBy})
        Me.dglistofval.Location = New System.Drawing.Point(7, 31)
        Me.dglistofval.Name = "dglistofval"
        Me.dglistofval.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dglistofval.Size = New System.Drawing.Size(927, 319)
        Me.dglistofval.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtSearch)
        Me.Panel1.Controls.Add(Me.dglistofval)
        Me.Panel1.Location = New System.Drawing.Point(252, 199)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(942, 359)
        Me.Panel1.TabIndex = 65
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.FromArgb(CType(CType(182, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(170, Byte), Integer))
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(1199, 24)
        Me.Label16.TabIndex = 312
        Me.Label16.Text = "LIST OF VALUE"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvlistofvaltype
        '
        Me.dgvlistofvaltype.AllowUserToAddRows = False
        Me.dgvlistofvaltype.AllowUserToDeleteRows = False
        Me.dgvlistofvaltype.AllowUserToOrderColumns = True
        Me.dgvlistofvaltype.AllowUserToResizeRows = False
        Me.dgvlistofvaltype.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvlistofvaltype.BackgroundColor = System.Drawing.Color.White
        Me.dgvlistofvaltype.ColumnHeadersHeight = 34
        Me.dgvlistofvaltype.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1})
        Me.dgvlistofvaltype.Location = New System.Drawing.Point(7, 179)
        Me.dgvlistofvaltype.MultiSelect = False
        Me.dgvlistofvaltype.Name = "dgvlistofvaltype"
        Me.dgvlistofvaltype.ReadOnly = True
        Me.dgvlistofvaltype.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvlistofvaltype.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvlistofvaltype.Size = New System.Drawing.Size(239, 361)
        Me.dgvlistofvaltype.TabIndex = 313
        '
        'Column1
        '
        Me.Column1.HeaderText = "Type"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 196
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel1.Location = New System.Drawing.Point(4, 543)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(44, 15)
        Me.LinkLabel1.TabIndex = 314
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "<<First"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel2.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel2.Location = New System.Drawing.Point(54, 543)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(38, 15)
        Me.LinkLabel2.TabIndex = 315
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "<Prev"
        '
        'LinkLabel3
        '
        Me.LinkLabel3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel3.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel3.Location = New System.Drawing.Point(206, 543)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(44, 15)
        Me.LinkLabel3.TabIndex = 317
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = "Last>>"
        '
        'LinkLabel4
        '
        Me.LinkLabel4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel4.AutoSize = True
        Me.LinkLabel4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel4.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel4.Location = New System.Drawing.Point(161, 543)
        Me.LinkLabel4.Name = "LinkLabel4"
        Me.LinkLabel4.Size = New System.Drawing.Size(39, 15)
        Me.LinkLabel4.TabIndex = 316
        Me.LinkLabel4.TabStop = True
        Me.LinkLabel4.Text = "Next>"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(171, 150)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 318
        Me.Button4.Text = "&Refresh"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'bgworklistofvaltypes
        '
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(1, 31)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(61, 13)
        Me.Label10.TabIndex = 391
        Me.Label10.Text = "Search box"
        '
        'btnSearchNow
        '
        Me.btnSearchNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearchNow.Location = New System.Drawing.Point(136, 91)
        Me.btnSearchNow.Name = "btnSearchNow"
        Me.btnSearchNow.Size = New System.Drawing.Size(21, 23)
        Me.btnSearchNow.TabIndex = 392
        Me.btnSearchNow.Text = "..."
        Me.btnSearchNow.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.ToolTip1.SetToolTip(Me.btnSearchNow, "Search now")
        Me.btnSearchNow.UseVisualStyleBackColor = True
        Me.btnSearchNow.Visible = False
        '
        'autcoListOfValType
        '
        Me.autcoListOfValType.Enabled = False
        Me.autcoListOfValType.Font = New System.Drawing.Font("Segoe UI Semilight", 9.75!)
        Me.autcoListOfValType.Location = New System.Drawing.Point(7, 47)
        Me.autcoListOfValType.Name = "autcoListOfValType"
        Me.autcoListOfValType.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.autcoListOfValType.PopupOffset = New System.Drawing.Point(12, 0)
        Me.autcoListOfValType.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.autcoListOfValType.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.autcoListOfValType.PopupWidth = 300
        Me.autcoListOfValType.Size = New System.Drawing.Size(239, 25)
        Me.autcoListOfValType.TabIndex = 393
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "Row ID"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.Visible = False
        '
        'c_display
        '
        Me.c_display.HeaderText = "Display Value"
        Me.c_display.Name = "c_display"
        '
        'c_lic
        '
        Me.c_lic.HeaderText = "LIC"
        Me.c_lic.Name = "c_lic"
        '
        'c_Type
        '
        Me.c_Type.HeaderText = "Type"
        Me.c_Type.Name = "c_Type"
        Me.c_Type.Visible = False
        '
        'c_parentvalue
        '
        Me.c_parentvalue.HeaderText = "Parent Value"
        Me.c_parentvalue.Name = "c_parentvalue"
        '
        'c_Active
        '
        Me.c_Active.HeaderText = "Active"
        Me.c_Active.Name = "c_Active"
        Me.c_Active.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_Active.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'c_description
        '
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_description.DefaultCellStyle = DataGridViewCellStyle1
        Me.c_description.FillWeight = 200.0!
        Me.c_description.HeaderText = "Description"
        Me.c_description.Name = "c_description"
        '
        'c_orderBy
        '
        Me.c_orderBy.HeaderText = "Order By"
        Me.c_orderBy.Name = "c_orderBy"
        Me.c_orderBy.Visible = False
        '
        'ListOfValueForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1199, 562)
        Me.Controls.Add(Me.autcoListOfValType)
        Me.Controls.Add(Me.btnSearchNow)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me.LinkLabel3)
        Me.Controls.Add(Me.LinkLabel4)
        Me.Controls.Add(Me.dgvlistofvaltype)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ListOfValueForm"
        Me.Text = "ListOfValueForm"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.grplistval.ResumeLayout(False)
        Me.grplistval.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dglistofval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvlistofvaltype, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblRowID As System.Windows.Forms.Label
    Friend WithEvents cmbStatus As System.Windows.Forms.ComboBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents txtParentLIC As System.Windows.Forms.TextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents grplistval As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtType As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtLIC As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDisplayval As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents dglistofval As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents dgvlistofvaltype As System.Windows.Forms.DataGridView
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel3 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel4 As System.Windows.Forms.LinkLabel
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label212 As System.Windows.Forms.Label
    Friend WithEvents bgworklistofvaltypes As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents btnSearchNow As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents autcoListOfValType As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents c_rowid As DataGridViewTextBoxColumn
    Friend WithEvents c_display As DataGridViewTextBoxColumn
    Friend WithEvents c_lic As DataGridViewTextBoxColumn
    Friend WithEvents c_Type As DataGridViewTextBoxColumn
    Friend WithEvents c_parentvalue As DataGridViewTextBoxColumn
    Friend WithEvents c_Active As DataGridViewCheckBoxColumn
    Friend WithEvents c_description As DataGridViewTextBoxColumn
    Friend WithEvents c_orderBy As DataGridViewTextBoxColumn
End Class
