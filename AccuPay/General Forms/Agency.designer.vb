<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Agency
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Agency))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dgvagency = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.CustomColoredTabControl1 = New AccuPay.CustomColoredTabControl()
        Me.tbpAgency = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtTIN = New System.Windows.Forms.TextBox()
        Me.btnSelectAddress = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtbxAddress = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtagencyfee = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtagencyname = New System.Windows.Forms.TextBox()
        Me.Label212 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnClose = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnNewAgency = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveAgency = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancel = New System.Windows.Forms.ToolStripButton()
        Me.tbpEmployee = New System.Windows.Forms.TabPage()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnEmpID = New System.Windows.Forms.Button()
        Me.dgvemployee = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.eRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MiddleName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DivisionID = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.PositionID = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Label223 = New System.Windows.Forms.Label()
        Me.LinkLabel4 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveEmp = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancelEmp = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tstxtsearch = New System.Windows.Forms.ToolStripTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.autcomptxtagency = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.Last = New System.Windows.Forms.LinkLabel()
        Me.Nxt = New System.Windows.Forms.LinkLabel()
        Me.Prev = New System.Windows.Forms.LinkLabel()
        Me.First = New System.Windows.Forms.LinkLabel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.bgworksearchbox = New System.ComponentModel.BackgroundWorker()
        Me.agRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.agName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.agFee = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.agAddress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.agAddressID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.agIsActive = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvagency, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CustomColoredTabControl1.SuspendLayout()
        Me.tbpAgency.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.tbpEmployee.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.dgvemployee, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(1021, 21)
        Me.Label25.TabIndex = 335
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.dgvagency)
        Me.Panel1.Controls.Add(Me.CustomColoredTabControl1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.autcomptxtagency)
        Me.Panel1.Controls.Add(Me.btnRefresh)
        Me.Panel1.Controls.Add(Me.Last)
        Me.Panel1.Controls.Add(Me.Nxt)
        Me.Panel1.Controls.Add(Me.Prev)
        Me.Panel1.Controls.Add(Me.First)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 21)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1021, 463)
        Me.Panel1.TabIndex = 336
        '
        'dgvagency
        '
        Me.dgvagency.AllowUserToAddRows = False
        Me.dgvagency.AllowUserToDeleteRows = False
        Me.dgvagency.AllowUserToOrderColumns = True
        Me.dgvagency.AllowUserToResizeRows = False
        Me.dgvagency.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvagency.BackgroundColor = System.Drawing.Color.White
        Me.dgvagency.ColumnHeadersHeight = 34
        Me.dgvagency.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.agRowID, Me.agName, Me.agFee, Me.agAddress, Me.agAddressID, Me.agIsActive})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvagency.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvagency.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvagency.Location = New System.Drawing.Point(15, 181)
        Me.dgvagency.MultiSelect = False
        Me.dgvagency.Name = "dgvagency"
        Me.dgvagency.ReadOnly = True
        Me.dgvagency.RowHeadersWidth = 25
        Me.dgvagency.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvagency.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvagency.Size = New System.Drawing.Size(321, 255)
        Me.dgvagency.TabIndex = 337
        '
        'CustomColoredTabControl1
        '
        Me.CustomColoredTabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.CustomColoredTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CustomColoredTabControl1.Controls.Add(Me.tbpAgency)
        Me.CustomColoredTabControl1.Controls.Add(Me.tbpEmployee)
        Me.CustomColoredTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.CustomColoredTabControl1.ItemSize = New System.Drawing.Size(152, 25)
        Me.CustomColoredTabControl1.Location = New System.Drawing.Point(345, -1)
        Me.CustomColoredTabControl1.Name = "CustomColoredTabControl1"
        Me.CustomColoredTabControl1.SelectedIndex = 0
        Me.CustomColoredTabControl1.Size = New System.Drawing.Size(676, 463)
        Me.CustomColoredTabControl1.TabIndex = 336
        '
        'tbpAgency
        '
        Me.tbpAgency.Controls.Add(Me.Panel2)
        Me.tbpAgency.Controls.Add(Me.ToolStrip1)
        Me.tbpAgency.Location = New System.Drawing.Point(4, 4)
        Me.tbpAgency.Name = "tbpAgency"
        Me.tbpAgency.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpAgency.Size = New System.Drawing.Size(668, 430)
        Me.tbpAgency.TabIndex = 0
        Me.tbpAgency.Text = "AGENCY               "
        Me.tbpAgency.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.CheckBox1)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.txtTIN)
        Me.Panel2.Controls.Add(Me.btnSelectAddress)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.txtbxAddress)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.txtagencyfee)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.txtagencyname)
        Me.Panel2.Controls.Add(Me.Label212)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 28)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(662, 399)
        Me.Panel2.TabIndex = 1
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(130, 39)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(56, 17)
        Me.CheckBox1.TabIndex = 370
        Me.CheckBox1.Text = "Active"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(21, 180)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(25, 13)
        Me.Label6.TabIndex = 369
        Me.Label6.Text = "TIN"
        '
        'txtTIN
        '
        Me.txtTIN.Location = New System.Drawing.Point(130, 173)
        Me.txtTIN.MaxLength = 15
        Me.txtTIN.Name = "txtTIN"
        Me.txtTIN.Size = New System.Drawing.Size(207, 20)
        Me.txtTIN.TabIndex = 368
        '
        'btnSelectAddress
        '
        Me.btnSelectAddress.Font = New System.Drawing.Font("Segoe UI Semibold", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectAddress.Location = New System.Drawing.Point(343, 148)
        Me.btnSelectAddress.Name = "btnSelectAddress"
        Me.btnSelectAddress.Size = New System.Drawing.Size(19, 19)
        Me.btnSelectAddress.TabIndex = 367
        Me.btnSelectAddress.Text = "..."
        Me.btnSelectAddress.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.ToolTip1.SetToolTip(Me.btnSelectAddress, "Select an address")
        Me.btnSelectAddress.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(21, 121)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 366
        Me.Label5.Text = "Address"
        '
        'txtbxAddress
        '
        Me.txtbxAddress.BackColor = System.Drawing.Color.White
        Me.txtbxAddress.Location = New System.Drawing.Point(130, 114)
        Me.txtbxAddress.Multiline = True
        Me.txtbxAddress.Name = "txtbxAddress"
        Me.txtbxAddress.ReadOnly = True
        Me.txtbxAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtbxAddress.Size = New System.Drawing.Size(207, 53)
        Me.txtbxAddress.TabIndex = 365
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 95)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 363
        Me.Label3.Text = "Agency fee"
        '
        'txtagencyfee
        '
        Me.txtagencyfee.Location = New System.Drawing.Point(130, 88)
        Me.txtagencyfee.MaxLength = 14
        Me.txtagencyfee.Name = "txtagencyfee"
        Me.txtagencyfee.Size = New System.Drawing.Size(207, 20)
        Me.txtagencyfee.TabIndex = 362
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(76, 85)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(19, 23)
        Me.Label4.TabIndex = 364
        Me.Label4.Text = "*"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Name"
        '
        'txtagencyname
        '
        Me.txtagencyname.Location = New System.Drawing.Point(130, 62)
        Me.txtagencyname.MaxLength = 100
        Me.txtagencyname.Name = "txtagencyname"
        Me.txtagencyname.Size = New System.Drawing.Size(207, 20)
        Me.txtagencyname.TabIndex = 0
        '
        'Label212
        '
        Me.Label212.AutoSize = True
        Me.Label212.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label212.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label212.Location = New System.Drawing.Point(50, 59)
        Me.Label212.Name = "Label212"
        Me.Label212.Size = New System.Drawing.Size(19, 23)
        Me.Label212.TabIndex = 361
        Me.Label212.Text = "*"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI Semilight", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(337, 95)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(103, 13)
        Me.Label7.TabIndex = 371
        Me.Label7.Text = "(daily fee - prorated)"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnClose, Me.tsbtnNewAgency, Me.tsbtnSaveAgency, Me.tsbtnCancel})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(662, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnClose.Name = "tsbtnClose"
        Me.tsbtnClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnClose.Text = "Close"
        '
        'tsbtnNewAgency
        '
        Me.tsbtnNewAgency.Image = CType(resources.GetObject("tsbtnNewAgency.Image"), System.Drawing.Image)
        Me.tsbtnNewAgency.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewAgency.Name = "tsbtnNewAgency"
        Me.tsbtnNewAgency.Size = New System.Drawing.Size(94, 22)
        Me.tsbtnNewAgency.Text = "&New Agency"
        '
        'tsbtnSaveAgency
        '
        Me.tsbtnSaveAgency.Image = CType(resources.GetObject("tsbtnSaveAgency.Image"), System.Drawing.Image)
        Me.tsbtnSaveAgency.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveAgency.Name = "tsbtnSaveAgency"
        Me.tsbtnSaveAgency.Size = New System.Drawing.Size(94, 22)
        Me.tsbtnSaveAgency.Text = "&Save Agency"
        '
        'tsbtnCancel
        '
        Me.tsbtnCancel.Image = CType(resources.GetObject("tsbtnCancel.Image"), System.Drawing.Image)
        Me.tsbtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancel.Name = "tsbtnCancel"
        Me.tsbtnCancel.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancel.Text = "Cancel"
        '
        'tbpEmployee
        '
        Me.tbpEmployee.Controls.Add(Me.Panel3)
        Me.tbpEmployee.Controls.Add(Me.ToolStrip2)
        Me.tbpEmployee.Location = New System.Drawing.Point(4, 4)
        Me.tbpEmployee.Name = "tbpEmployee"
        Me.tbpEmployee.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpEmployee.Size = New System.Drawing.Size(668, 430)
        Me.tbpEmployee.TabIndex = 1
        Me.tbpEmployee.Text = "EMPLOYEES               "
        Me.tbpEmployee.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.AutoScroll = True
        Me.Panel3.Controls.Add(Me.btnEmpID)
        Me.Panel3.Controls.Add(Me.dgvemployee)
        Me.Panel3.Controls.Add(Me.Label223)
        Me.Panel3.Controls.Add(Me.LinkLabel4)
        Me.Panel3.Controls.Add(Me.LinkLabel1)
        Me.Panel3.Controls.Add(Me.LinkLabel3)
        Me.Panel3.Controls.Add(Me.LinkLabel2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(3, 28)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(662, 399)
        Me.Panel3.TabIndex = 362
        '
        'btnEmpID
        '
        Me.btnEmpID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEmpID.Location = New System.Drawing.Point(203, 79)
        Me.btnEmpID.Name = "btnEmpID"
        Me.btnEmpID.Size = New System.Drawing.Size(21, 23)
        Me.btnEmpID.TabIndex = 362
        Me.btnEmpID.Text = "..."
        Me.btnEmpID.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.ToolTip1.SetToolTip(Me.btnEmpID, "Select from Employee(s)")
        Me.btnEmpID.UseVisualStyleBackColor = True
        Me.btnEmpID.Visible = False
        '
        'dgvemployee
        '
        Me.dgvemployee.AllowUserToResizeRows = False
        Me.dgvemployee.BackgroundColor = System.Drawing.Color.White
        Me.dgvemployee.ColumnHeadersHeight = 34
        Me.dgvemployee.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eRowID, Me.EmployeeID, Me.LastName, Me.FirstName, Me.MiddleName, Me.DivisionID, Me.PositionID})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvemployee.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvemployee.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvemployee.Location = New System.Drawing.Point(3, 61)
        Me.dgvemployee.MultiSelect = False
        Me.dgvemployee.Name = "dgvemployee"
        Me.dgvemployee.Size = New System.Drawing.Size(623, 419)
        Me.dgvemployee.TabIndex = 1
        '
        'eRowID
        '
        Me.eRowID.HeaderText = "RowID"
        Me.eRowID.Name = "eRowID"
        Me.eRowID.Visible = False
        '
        'EmployeeID
        '
        Me.EmployeeID.HeaderText = "Employee ID"
        Me.EmployeeID.Name = "EmployeeID"
        Me.EmployeeID.ReadOnly = True
        Me.EmployeeID.Width = 160
        '
        'LastName
        '
        Me.LastName.HeaderText = "Last Name"
        Me.LastName.Name = "LastName"
        Me.LastName.ReadOnly = True
        Me.LastName.Width = 140
        '
        'FirstName
        '
        Me.FirstName.HeaderText = "First Name"
        Me.FirstName.Name = "FirstName"
        Me.FirstName.ReadOnly = True
        Me.FirstName.Width = 140
        '
        'MiddleName
        '
        Me.MiddleName.HeaderText = "Middle Name"
        Me.MiddleName.Name = "MiddleName"
        Me.MiddleName.ReadOnly = True
        Me.MiddleName.Width = 140
        '
        'DivisionID
        '
        Me.DivisionID.HeaderText = "Group"
        Me.DivisionID.Name = "DivisionID"
        Me.DivisionID.ReadOnly = True
        Me.DivisionID.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DivisionID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DivisionID.Width = 150
        '
        'PositionID
        '
        Me.PositionID.HeaderText = "Position"
        Me.PositionID.Name = "PositionID"
        Me.PositionID.ReadOnly = True
        Me.PositionID.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.PositionID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.PositionID.Width = 150
        '
        'Label223
        '
        Me.Label223.AutoSize = True
        Me.Label223.ForeColor = System.Drawing.Color.White
        Me.Label223.Location = New System.Drawing.Point(710, 530)
        Me.Label223.Name = "Label223"
        Me.Label223.Size = New System.Drawing.Size(37, 13)
        Me.Label223.TabIndex = 361
        Me.Label223.Text = "_____"
        '
        'LinkLabel4
        '
        Me.LinkLabel4.AutoSize = True
        Me.LinkLabel4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel4.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel4.Location = New System.Drawing.Point(0, 483)
        Me.LinkLabel4.Name = "LinkLabel4"
        Me.LinkLabel4.Size = New System.Drawing.Size(44, 15)
        Me.LinkLabel4.TabIndex = 333
        Me.LinkLabel4.TabStop = True
        Me.LinkLabel4.Text = "<<First"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel1.Location = New System.Drawing.Point(283, 483)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(44, 15)
        Me.LinkLabel1.TabIndex = 336
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Last>>"
        '
        'LinkLabel3
        '
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel3.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel3.Location = New System.Drawing.Point(50, 483)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(38, 15)
        Me.LinkLabel3.TabIndex = 334
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = "<Prev"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel2.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel2.Location = New System.Drawing.Point(238, 483)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(39, 15)
        Me.LinkLabel2.TabIndex = 335
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "Next>"
        '
        'ToolStrip2
        '
        Me.ToolStrip2.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton4, Me.tsbtnSaveEmp, Me.tsbtnCancelEmp, Me.ToolStripLabel1, Me.ToolStripSeparator1, Me.tstxtsearch})
        Me.ToolStrip2.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(662, 25)
        Me.ToolStrip2.TabIndex = 363
        Me.ToolStrip2.Text = "ToolStrip2"
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
        'tsbtnSaveEmp
        '
        Me.tsbtnSaveEmp.Image = CType(resources.GetObject("tsbtnSaveEmp.Image"), System.Drawing.Image)
        Me.tsbtnSaveEmp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveEmp.Name = "tsbtnSaveEmp"
        Me.tsbtnSaveEmp.Size = New System.Drawing.Size(106, 22)
        Me.tsbtnSaveEmp.Text = "&Save Employee"
        '
        'tsbtnCancelEmp
        '
        Me.tsbtnCancelEmp.Image = CType(resources.GetObject("tsbtnCancelEmp.Image"), System.Drawing.Image)
        Me.tsbtnCancelEmp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancelEmp.Name = "tsbtnCancelEmp"
        Me.tsbtnCancelEmp.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancelEmp.Text = "Cancel"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(37, 22)
        Me.ToolStripLabel1.Text = "          "
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tstxtsearch
        '
        Me.tstxtsearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tstxtsearch.Name = "tstxtsearch"
        Me.tstxtsearch.Size = New System.Drawing.Size(150, 25)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 335
        Me.Label1.Text = "Search box"
        '
        'autcomptxtagency
        '
        Me.autcomptxtagency.Enabled = False
        Me.autcomptxtagency.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.autcomptxtagency.Location = New System.Drawing.Point(12, 64)
        Me.autcomptxtagency.Name = "autcomptxtagency"
        Me.autcomptxtagency.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.autcomptxtagency.PopupOffset = New System.Drawing.Point(12, 0)
        Me.autcomptxtagency.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.autcomptxtagency.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.autcomptxtagency.PopupWidth = 300
        Me.autcomptxtagency.Size = New System.Drawing.Size(324, 25)
        Me.autcomptxtagency.TabIndex = 334
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(261, 105)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 333
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'Last
        '
        Me.Last.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Last.AutoSize = True
        Me.Last.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Last.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Last.Location = New System.Drawing.Point(295, 439)
        Me.Last.Name = "Last"
        Me.Last.Size = New System.Drawing.Size(44, 15)
        Me.Last.TabIndex = 332
        Me.Last.TabStop = True
        Me.Last.Text = "Last>>"
        '
        'Nxt
        '
        Me.Nxt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Nxt.AutoSize = True
        Me.Nxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Nxt.Location = New System.Drawing.Point(250, 439)
        Me.Nxt.Name = "Nxt"
        Me.Nxt.Size = New System.Drawing.Size(39, 15)
        Me.Nxt.TabIndex = 331
        Me.Nxt.TabStop = True
        Me.Nxt.Text = "Next>"
        '
        'Prev
        '
        Me.Prev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Prev.AutoSize = True
        Me.Prev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Prev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Prev.Location = New System.Drawing.Point(62, 439)
        Me.Prev.Name = "Prev"
        Me.Prev.Size = New System.Drawing.Size(38, 15)
        Me.Prev.TabIndex = 330
        Me.Prev.TabStop = True
        Me.Prev.Text = "<Prev"
        '
        'First
        '
        Me.First.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.First.AutoSize = True
        Me.First.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.First.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.First.Location = New System.Drawing.Point(12, 439)
        Me.First.Name = "First"
        Me.First.Size = New System.Drawing.Size(44, 15)
        Me.First.TabIndex = 329
        Me.First.TabStop = True
        Me.First.Text = "<<First"
        '
        'bgworksearchbox
        '
        Me.bgworksearchbox.WorkerReportsProgress = True
        Me.bgworksearchbox.WorkerSupportsCancellation = True
        '
        'agRowID
        '
        Me.agRowID.HeaderText = "RowID"
        Me.agRowID.Name = "agRowID"
        Me.agRowID.ReadOnly = True
        Me.agRowID.Visible = False
        '
        'agName
        '
        Me.agName.HeaderText = "Name"
        Me.agName.Name = "agName"
        Me.agName.ReadOnly = True
        '
        'agFee
        '
        Me.agFee.HeaderText = "Fee"
        Me.agFee.Name = "agFee"
        Me.agFee.ReadOnly = True
        '
        'agAddress
        '
        Me.agAddress.HeaderText = "Address"
        Me.agAddress.Name = "agAddress"
        Me.agAddress.ReadOnly = True
        '
        'agAddressID
        '
        Me.agAddressID.HeaderText = "agAddressID"
        Me.agAddressID.Name = "agAddressID"
        Me.agAddressID.ReadOnly = True
        Me.agAddressID.Visible = False
        '
        'agIsActive
        '
        Me.agIsActive.HeaderText = "agIsActive"
        Me.agIsActive.Name = "agIsActive"
        Me.agIsActive.ReadOnly = True
        Me.agIsActive.Visible = False
        '
        'Agency
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1021, 484)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label25)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "Agency"
        Me.Text = "Agency"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvagency, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CustomColoredTabControl1.ResumeLayout(False)
        Me.tbpAgency.ResumeLayout(False)
        Me.tbpAgency.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.tbpEmployee.ResumeLayout(False)
        Me.tbpEmployee.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.dgvemployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Last As System.Windows.Forms.LinkLabel
    Friend WithEvents Nxt As System.Windows.Forms.LinkLabel
    Friend WithEvents Prev As System.Windows.Forms.LinkLabel
    Friend WithEvents First As System.Windows.Forms.LinkLabel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents autcomptxtagency As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents CustomColoredTabControl1 As Global.AccuPay.CustomColoredTabControl
    Friend WithEvents tbpAgency As System.Windows.Forms.TabPage
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents tsbtnNewAgency As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSaveAgency As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtagencyname As System.Windows.Forms.TextBox
    Friend WithEvents Label212 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtagencyfee As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtbxAddress As System.Windows.Forms.TextBox
    Friend WithEvents btnSelectAddress As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtTIN As System.Windows.Forms.TextBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents tbpEmployee As System.Windows.Forms.TabPage
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel3 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel4 As System.Windows.Forms.LinkLabel
    Friend WithEvents dgvemployee As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label223 As System.Windows.Forms.Label
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents tsbtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSaveEmp As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancelEmp As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEmpID As System.Windows.Forms.Button
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tstxtsearch As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents dgvagency As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents bgworksearchbox As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents eRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EmployeeID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LastName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FirstName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MiddleName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DivisionID As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents PositionID As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents agRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents agName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents agFee As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents agAddress As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents agAddressID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents agIsActive As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
