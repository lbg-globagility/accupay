<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EmployeeAllowanceForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EmployeeAllowanceForm))
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle29 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle30 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle31 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle32 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.EmployeeNumberTextBox = New System.Windows.Forms.TextBox()
        Me.EmployeeNameTextBox = New System.Windows.Forms.TextBox()
        Me.EmployeeInfoTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.EmployeePictureBox = New System.Windows.Forms.PictureBox()
        Me.ToolStrip12 = New System.Windows.Forms.ToolStrip()
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.CancelToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton25 = New System.Windows.Forms.ToolStripButton()
        Me.ImportToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.UserActivityToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.AllowancesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.pnlForm = New System.Windows.Forms.Panel()
        Me.DetailsTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlTxtLoanBalance = New System.Windows.Forms.Panel()
        Me.txtallowamt = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label163 = New System.Windows.Forms.Label()
        Me.dtpallowenddate = New AccuPay.NullableDatePicker()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.dtpallowstartdate = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label167 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboallowfreq = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.plnCboLoanType = New System.Windows.Forms.Panel()
        Me.Label350 = New System.Windows.Forms.Label()
        Me.cboallowtype = New System.Windows.Forms.ComboBox()
        Me.lnklbaddallowtype = New System.Windows.Forms.LinkLabel()
        Me.Label156 = New System.Windows.Forms.Label()
        Me.AllowanceGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.eall_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Frequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Start = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.eall_End = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.eall_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.allow_taxab = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_ProdID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.employeesDataGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.cemp_EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cemp_LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cemp_FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SearchTextBox = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.ShowAllCheckBox = New System.Windows.Forms.CheckBox()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn19 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn17 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn18 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeInfoTabLayout.SuspendLayout()
        CType(Me.EmployeePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip12.SuspendLayout()
        CType(Me.AllowancesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMain.SuspendLayout()
        Me.pnlForm.SuspendLayout()
        Me.DetailsTabLayout.SuspendLayout()
        Me.pnlTxtLoanBalance.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.plnCboLoanType.SuspendLayout()
        CType(Me.AllowanceGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.employeesDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'EmployeeNumberTextBox
        '
        Me.EmployeeNumberTextBox.BackColor = System.Drawing.Color.White
        Me.EmployeeNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.EmployeeNumberTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EmployeeNumberTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.EmployeeNumberTextBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.EmployeeNumberTextBox.Location = New System.Drawing.Point(124, 47)
        Me.EmployeeNumberTextBox.MaxLength = 50
        Me.EmployeeNumberTextBox.Multiline = True
        Me.EmployeeNumberTextBox.Name = "EmployeeNumberTextBox"
        Me.EmployeeNumberTextBox.ReadOnly = True
        Me.EmployeeNumberTextBox.Size = New System.Drawing.Size(855, 38)
        Me.EmployeeNumberTextBox.TabIndex = 380
        '
        'EmployeeNameTextBox
        '
        Me.EmployeeNameTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EmployeeNameTextBox.BackColor = System.Drawing.Color.White
        Me.EmployeeNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.EmployeeNameTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.EmployeeNameTextBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.EmployeeNameTextBox.Location = New System.Drawing.Point(124, 13)
        Me.EmployeeNameTextBox.MaxLength = 250
        Me.EmployeeNameTextBox.Name = "EmployeeNameTextBox"
        Me.EmployeeNameTextBox.ReadOnly = True
        Me.EmployeeNameTextBox.Size = New System.Drawing.Size(668, 28)
        Me.EmployeeNameTextBox.TabIndex = 381
        '
        'EmployeeInfoTabLayout
        '
        Me.EmployeeInfoTabLayout.BackColor = System.Drawing.Color.White
        Me.EmployeeInfoTabLayout.ColumnCount = 2
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121.0!))
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 861.0!))
        Me.EmployeeInfoTabLayout.Controls.Add(Me.EmployeeNameTextBox, 1, 0)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.EmployeeNumberTextBox, 1, 1)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.EmployeePictureBox, 0, 0)
        Me.EmployeeInfoTabLayout.Location = New System.Drawing.Point(0, 9)
        Me.EmployeeInfoTabLayout.Name = "EmployeeInfoTabLayout"
        Me.EmployeeInfoTabLayout.RowCount = 2
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.Size = New System.Drawing.Size(827, 88)
        Me.EmployeeInfoTabLayout.TabIndex = 3
        '
        'EmployeePictureBox
        '
        Me.EmployeePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.EmployeePictureBox.Location = New System.Drawing.Point(28, 3)
        Me.EmployeePictureBox.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.EmployeePictureBox.Name = "EmployeePictureBox"
        Me.EmployeeInfoTabLayout.SetRowSpan(Me.EmployeePictureBox, 2)
        Me.EmployeePictureBox.Size = New System.Drawing.Size(88, 82)
        Me.EmployeePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EmployeePictureBox.TabIndex = 382
        Me.EmployeePictureBox.TabStop = False
        '
        'ToolStrip12
        '
        Me.ToolStrip12.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip12.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip12.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.SaveToolStripButton, Me.ToolStripSeparator9, Me.DeleteToolStripButton, Me.ToolStripSeparator10, Me.CancelToolStripButton, Me.btnClose, Me.ToolStripButton25, Me.ImportToolStripButton, Me.UserActivityToolStripButton})
        Me.ToolStrip12.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip12.Name = "ToolStrip12"
        Me.ToolStrip12.Size = New System.Drawing.Size(842, 25)
        Me.ToolStrip12.TabIndex = 328
        Me.ToolStrip12.Text = "ToolStrip12"
        '
        'NewToolStripButton
        '
        Me.NewToolStripButton.Image = Global.AccuPay.My.Resources.Resources._new
        Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripButton.Name = "NewToolStripButton"
        Me.NewToolStripButton.Size = New System.Drawing.Size(51, 22)
        Me.NewToolStripButton.Text = "&New"
        '
        'SaveToolStripButton
        '
        Me.SaveToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripButton.Name = "SaveToolStripButton"
        Me.SaveToolStripButton.Size = New System.Drawing.Size(51, 22)
        Me.SaveToolStripButton.Text = "&Save"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
        '
        'DeleteToolStripButton_Click
        '
        Me.DeleteToolStripButton.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripButton.Name = "DeleteToolStripButton_Click"
        Me.DeleteToolStripButton.Size = New System.Drawing.Size(60, 22)
        Me.DeleteToolStripButton.Text = "&Delete"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 25)
        '
        'CancelToolStripButton
        '
        Me.CancelToolStripButton.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.CancelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CancelToolStripButton.Name = "CancelToolStripButton"
        Me.CancelToolStripButton.Size = New System.Drawing.Size(63, 22)
        Me.CancelToolStripButton.Text = "Cancel"
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "Close"
        '
        'ToolStripButton25
        '
        Me.ToolStripButton25.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton25.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton25.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton25.Name = "ToolStripButton25"
        Me.ToolStripButton25.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton25.Text = "ToolStripButton1"
        Me.ToolStripButton25.ToolTipText = "Show audit trails"
        '
        'ImportToolStripButton
        '
        Me.ImportToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Add
        Me.ImportToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ImportToolStripButton.Name = "ImportToolStripButton"
        Me.ImportToolStripButton.Size = New System.Drawing.Size(63, 22)
        Me.ImportToolStripButton.Text = "Import"
        Me.ImportToolStripButton.ToolTipText = "Import loans"
        '
        'UserActivityToolStripButton
        '
        Me.UserActivityToolStripButton.Image = CType(resources.GetObject("UserActivityToolStripButton.Image"), System.Drawing.Image)
        Me.UserActivityToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.UserActivityToolStripButton.Name = "UserActivityToolStripButton"
        Me.UserActivityToolStripButton.Size = New System.Drawing.Size(93, 22)
        Me.UserActivityToolStripButton.Text = "User Activity"
        '
        'AllowancesBindingSource
        '
        '
        'pnlMain
        '
        Me.pnlMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMain.BackColor = System.Drawing.Color.White
        Me.pnlMain.Controls.Add(Me.pnlForm)
        Me.pnlMain.Controls.Add(Me.ToolStrip12)
        Me.pnlMain.Location = New System.Drawing.Point(375, 33)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(842, 503)
        Me.pnlMain.TabIndex = 146
        '
        'pnlForm
        '
        Me.pnlForm.AutoScroll = True
        Me.pnlForm.BackColor = System.Drawing.Color.Transparent
        Me.pnlForm.Controls.Add(Me.DetailsTabLayout)
        Me.pnlForm.Controls.Add(Me.AllowanceGridView)
        Me.pnlForm.Controls.Add(Me.EmployeeInfoTabLayout)
        Me.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlForm.Location = New System.Drawing.Point(0, 25)
        Me.pnlForm.Name = "pnlForm"
        Me.pnlForm.Size = New System.Drawing.Size(842, 478)
        Me.pnlForm.TabIndex = 509
        '
        'AllowanceDetailsTabLayout
        '
        Me.DetailsTabLayout.ColumnCount = 1
        Me.DetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.DetailsTabLayout.Controls.Add(Me.pnlTxtLoanBalance, 0, 9)
        Me.DetailsTabLayout.Controls.Add(Me.Label163, 0, 8)
        Me.DetailsTabLayout.Controls.Add(Me.dtpallowenddate, 0, 7)
        Me.DetailsTabLayout.Controls.Add(Me.lblEndDate, 0, 6)
        Me.DetailsTabLayout.Controls.Add(Me.Panel2, 0, 5)
        Me.DetailsTabLayout.Controls.Add(Me.Label167, 0, 4)
        Me.DetailsTabLayout.Controls.Add(Me.Panel1, 0, 3)
        Me.DetailsTabLayout.Controls.Add(Me.Label1, 0, 2)
        Me.DetailsTabLayout.Controls.Add(Me.plnCboLoanType, 0, 1)
        Me.DetailsTabLayout.Controls.Add(Me.Label156, 0, 0)
        Me.DetailsTabLayout.Location = New System.Drawing.Point(8, 101)
        Me.DetailsTabLayout.Name = "AllowanceDetailsTabLayout"
        Me.DetailsTabLayout.RowCount = 10
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.DetailsTabLayout.Size = New System.Drawing.Size(265, 249)
        Me.DetailsTabLayout.TabIndex = 380
        '
        'pnlTxtLoanBalance
        '
        Me.pnlTxtLoanBalance.Controls.Add(Me.txtallowamt)
        Me.pnlTxtLoanBalance.Controls.Add(Me.Label4)
        Me.pnlTxtLoanBalance.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTxtLoanBalance.Location = New System.Drawing.Point(0, 208)
        Me.pnlTxtLoanBalance.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlTxtLoanBalance.Name = "pnlTxtLoanBalance"
        Me.pnlTxtLoanBalance.Size = New System.Drawing.Size(265, 41)
        Me.pnlTxtLoanBalance.TabIndex = 382
        '
        'txtallowamt
        '
        Me.txtallowamt.Location = New System.Drawing.Point(20, 3)
        Me.txtallowamt.Name = "txtallowamt"
        Me.txtallowamt.ShortcutsEnabled = False
        Me.txtallowamt.Size = New System.Drawing.Size(195, 22)
        Me.txtallowamt.TabIndex = 384
        Me.txtallowamt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(13, 13)
        Me.Label4.TabIndex = 383
        Me.Label4.Text = "₱"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label163
        '
        Me.Label163.AutoSize = True
        Me.Label163.Location = New System.Drawing.Point(20, 192)
        Me.Label163.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label163.Name = "Label163"
        Me.Label163.Size = New System.Drawing.Size(48, 13)
        Me.Label163.TabIndex = 381
        Me.Label163.Text = "Amount"
        '
        'dtpallowenddate
        '
        Me.dtpallowenddate.Location = New System.Drawing.Point(20, 160)
        Me.dtpallowenddate.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.dtpallowenddate.Name = "dtpallowenddate"
        Me.dtpallowenddate.Size = New System.Drawing.Size(195, 22)
        Me.dtpallowenddate.TabIndex = 380
        Me.dtpallowenddate.Value = New Date(2019, 5, 24, 10, 35, 13, 830)
        '
        'lblEndDate
        '
        Me.lblEndDate.AutoSize = True
        Me.lblEndDate.Location = New System.Drawing.Point(20, 144)
        Me.lblEndDate.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.Size = New System.Drawing.Size(53, 13)
        Me.lblEndDate.TabIndex = 374
        Me.lblEndDate.Text = "End date"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.dtpallowstartdate)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 112)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(265, 32)
        Me.Panel2.TabIndex = 373
        '
        'dtpallowstartdate
        '
        Me.dtpallowstartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpallowstartdate.Location = New System.Drawing.Point(20, 3)
        Me.dtpallowstartdate.Name = "dtpallowstartdate"
        Me.dtpallowstartdate.Size = New System.Drawing.Size(195, 22)
        Me.dtpallowstartdate.TabIndex = 508
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(3, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 507
        Me.Label3.Text = "*"
        '
        'Label167
        '
        Me.Label167.AutoSize = True
        Me.Label167.Location = New System.Drawing.Point(20, 96)
        Me.Label167.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label167.Name = "Label167"
        Me.Label167.Size = New System.Drawing.Size(57, 13)
        Me.Label167.TabIndex = 372
        Me.Label167.Text = "Start date"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cboallowfreq)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 64)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(265, 32)
        Me.Panel1.TabIndex = 368
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(3, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(13, 13)
        Me.Label2.TabIndex = 507
        Me.Label2.Text = "*"
        '
        'cboallowfreq
        '
        Me.cboallowfreq.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboallowfreq.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboallowfreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboallowfreq.FormattingEnabled = True
        Me.cboallowfreq.Location = New System.Drawing.Point(20, 2)
        Me.cboallowfreq.Name = "cboallowfreq"
        Me.cboallowfreq.Size = New System.Drawing.Size(195, 21)
        Me.cboallowfreq.TabIndex = 360
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 48)
        Me.Label1.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 367
        Me.Label1.Text = "Frequency"
        '
        'plnCboLoanType
        '
        Me.plnCboLoanType.Controls.Add(Me.Label350)
        Me.plnCboLoanType.Controls.Add(Me.cboallowtype)
        Me.plnCboLoanType.Controls.Add(Me.lnklbaddallowtype)
        Me.plnCboLoanType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plnCboLoanType.Location = New System.Drawing.Point(0, 16)
        Me.plnCboLoanType.Margin = New System.Windows.Forms.Padding(0)
        Me.plnCboLoanType.Name = "plnCboLoanType"
        Me.plnCboLoanType.Size = New System.Drawing.Size(265, 32)
        Me.plnCboLoanType.TabIndex = 366
        '
        'Label350
        '
        Me.Label350.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label350.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label350.Location = New System.Drawing.Point(3, 4)
        Me.Label350.Name = "Label350"
        Me.Label350.Size = New System.Drawing.Size(13, 13)
        Me.Label350.TabIndex = 507
        Me.Label350.Text = "*"
        '
        'cboallowtype
        '
        Me.cboallowtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboallowtype.FormattingEnabled = True
        Me.cboallowtype.Location = New System.Drawing.Point(20, 2)
        Me.cboallowtype.Name = "cboallowtype"
        Me.cboallowtype.Size = New System.Drawing.Size(195, 21)
        Me.cboallowtype.TabIndex = 378
        '
        'lnklbaddallowtype
        '
        Me.lnklbaddallowtype.AutoSize = True
        Me.lnklbaddallowtype.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklbaddallowtype.Location = New System.Drawing.Point(222, 6)
        Me.lnklbaddallowtype.Name = "lnklbaddallowtype"
        Me.lnklbaddallowtype.Size = New System.Drawing.Size(28, 15)
        Me.lnklbaddallowtype.TabIndex = 373
        Me.lnklbaddallowtype.TabStop = True
        Me.lnklbaddallowtype.Text = "Add"
        '
        'Label156
        '
        Me.Label156.AutoSize = True
        Me.Label156.Location = New System.Drawing.Point(20, 0)
        Me.Label156.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label156.Name = "Label156"
        Me.Label156.Size = New System.Drawing.Size(29, 13)
        Me.Label156.TabIndex = 365
        Me.Label156.Text = "Type"
        '
        'AllowanceGridView
        '
        Me.AllowanceGridView.AllowUserToAddRows = False
        Me.AllowanceGridView.AllowUserToDeleteRows = False
        Me.AllowanceGridView.AllowUserToOrderColumns = True
        Me.AllowanceGridView.AllowUserToResizeColumns = False
        Me.AllowanceGridView.AllowUserToResizeRows = False
        Me.AllowanceGridView.BackgroundColor = System.Drawing.Color.White
        Me.AllowanceGridView.ColumnHeadersHeight = 34
        Me.AllowanceGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eall_RowID, Me.eall_Type, Me.eall_Frequency, Me.eall_Start, Me.eall_End, Me.eall_Amount, Me.allow_taxab, Me.eall_ProdID})
        DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle18.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.AllowanceGridView.DefaultCellStyle = DataGridViewCellStyle18
        Me.AllowanceGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.AllowanceGridView.Location = New System.Drawing.Point(28, 350)
        Me.AllowanceGridView.MultiSelect = False
        Me.AllowanceGridView.Name = "AllowanceGridView"
        Me.AllowanceGridView.ReadOnly = True
        Me.AllowanceGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.AllowanceGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.AllowanceGridView.Size = New System.Drawing.Size(784, 250)
        Me.AllowanceGridView.TabIndex = 364
        '
        'eall_RowID
        '
        Me.eall_RowID.DataPropertyName = "RowID"
        Me.eall_RowID.HeaderText = "RowID"
        Me.eall_RowID.Name = "eall_RowID"
        Me.eall_RowID.ReadOnly = True
        Me.eall_RowID.Visible = False
        Me.eall_RowID.Width = 50
        '
        'eall_Type
        '
        Me.eall_Type.DataPropertyName = "Type"
        Me.eall_Type.HeaderText = "Type"
        Me.eall_Type.Name = "eall_Type"
        Me.eall_Type.ReadOnly = True
        Me.eall_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eall_Type.Width = 180
        '
        'eall_Frequency
        '
        Me.eall_Frequency.DataPropertyName = "AllowanceFrequency"
        Me.eall_Frequency.HeaderText = "Frequency"
        Me.eall_Frequency.Name = "eall_Frequency"
        Me.eall_Frequency.ReadOnly = True
        Me.eall_Frequency.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
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
        Me.eall_Start.DataPropertyName = "EffectiveStartDate"
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
        Me.eall_End.DataPropertyName = "EffectiveEndDate"
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
        'eall_Amount
        '
        Me.eall_Amount.DataPropertyName = "Amount"
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle17.Format = "N2"
        DataGridViewCellStyle17.NullValue = Nothing
        Me.eall_Amount.DefaultCellStyle = DataGridViewCellStyle17
        Me.eall_Amount.HeaderText = "Amount"
        Me.eall_Amount.Name = "eall_Amount"
        Me.eall_Amount.ReadOnly = True
        Me.eall_Amount.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eall_Amount.Width = 180
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
        'employeesDataGridView
        '
        Me.employeesDataGridView.AllowUserToAddRows = False
        Me.employeesDataGridView.AllowUserToDeleteRows = False
        Me.employeesDataGridView.AllowUserToOrderColumns = True
        Me.employeesDataGridView.AllowUserToResizeRows = False
        Me.employeesDataGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.employeesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.employeesDataGridView.BackgroundColor = System.Drawing.Color.White
        Me.employeesDataGridView.ColumnHeadersHeight = 34
        Me.employeesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.employeesDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cemp_EmployeeID, Me.cemp_LastName, Me.cemp_FirstName})
        DataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle19.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.employeesDataGridView.DefaultCellStyle = DataGridViewCellStyle19
        Me.employeesDataGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.employeesDataGridView.Location = New System.Drawing.Point(8, 120)
        Me.employeesDataGridView.MultiSelect = False
        Me.employeesDataGridView.Name = "employeesDataGridView"
        Me.employeesDataGridView.ReadOnly = True
        Me.employeesDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.employeesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.employeesDataGridView.Size = New System.Drawing.Size(352, 415)
        Me.employeesDataGridView.TabIndex = 144
        '
        'cemp_EmployeeID
        '
        Me.cemp_EmployeeID.DataPropertyName = "EmployeeNo"
        Me.cemp_EmployeeID.HeaderText = "Employee ID"
        Me.cemp_EmployeeID.Name = "cemp_EmployeeID"
        Me.cemp_EmployeeID.ReadOnly = True
        '
        'cemp_LastName
        '
        Me.cemp_LastName.DataPropertyName = "LastName"
        Me.cemp_LastName.HeaderText = "Last Name"
        Me.cemp_LastName.Name = "cemp_LastName"
        Me.cemp_LastName.ReadOnly = True
        '
        'cemp_FirstName
        '
        Me.cemp_FirstName.DataPropertyName = "FirstName"
        Me.cemp_FirstName.HeaderText = "First Name"
        Me.cemp_FirstName.Name = "cemp_FirstName"
        Me.cemp_FirstName.ReadOnly = True
        '
        'searchTextBox
        '
        Me.SearchTextBox.Location = New System.Drawing.Point(80, 16)
        Me.SearchTextBox.MaxLength = 50
        Me.SearchTextBox.Name = "searchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(259, 22)
        Me.SearchTextBox.TabIndex = 64
        '
        'lblSearch
        '
        Me.lblSearch.Location = New System.Drawing.Point(8, 16)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(64, 25)
        Me.lblSearch.TabIndex = 62
        Me.lblSearch.Text = "Search"
        Me.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlSearch
        '
        Me.pnlSearch.BackColor = System.Drawing.Color.White
        Me.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSearch.Controls.Add(Me.SearchTextBox)
        Me.pnlSearch.Controls.Add(Me.lblSearch)
        Me.pnlSearch.Location = New System.Drawing.Point(8, 32)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(352, 56)
        Me.pnlSearch.TabIndex = 145
        '
        'lblFormTitle
        '
        Me.lblFormTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(174, Byte), Integer), CType(CType(194, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblFormTitle.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblFormTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(1229, 24)
        Me.lblFormTitle.TabIndex = 143
        Me.lblFormTitle.Text = "Employee Allowance"
        Me.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbShowAll
        '
        Me.ShowAllCheckBox.AutoSize = True
        Me.ShowAllCheckBox.Location = New System.Drawing.Point(8, 97)
        Me.ShowAllCheckBox.Name = "cbShowAll"
        Me.ShowAllCheckBox.Size = New System.Drawing.Size(128, 17)
        Me.ShowAllCheckBox.TabIndex = 147
        Me.ShowAllCheckBox.Text = "Show All Employees"
        Me.ShowAllCheckBox.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "RowID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Visible = False
        Me.DataGridViewTextBoxColumn1.Width = 40
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "EmployeeID"
        DataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle20
        Me.DataGridViewTextBoxColumn2.HeaderText = "Employee ID"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 103
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "LastName"
        DataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle21
        Me.DataGridViewTextBoxColumn3.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 103
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "FirstName"
        DataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle22
        Me.DataGridViewTextBoxColumn4.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Visible = False
        Me.DataGridViewTextBoxColumn4.Width = 103
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "DeductionPercentage"
        DataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle23
        Me.DataGridViewTextBoxColumn5.HeaderText = "Loan Number"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn5.Width = 40
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "DeductionSchedule"
        DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle24
        Me.DataGridViewTextBoxColumn6.HeaderText = "Total Loan Amount"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn6.Visible = False
        Me.DataGridViewTextBoxColumn6.Width = 180
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "NoOfPayPeriod"
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle25
        Me.DataGridViewTextBoxColumn7.HeaderText = "Total Balance Left"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn7.Visible = False
        Me.DataGridViewTextBoxColumn7.Width = 180
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "LoanPayPeriodLeft"
        DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn8.DefaultCellStyle = DataGridViewCellStyle26
        Me.DataGridViewTextBoxColumn8.HeaderText = "Deduction Amount"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Visible = False
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "DedEffectiveDateFrom"
        DataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn9.DefaultCellStyle = DataGridViewCellStyle27
        Me.DataGridViewTextBoxColumn9.HeaderText = "Deduction Percentage"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Visible = False
        '
        'DataGridViewTextBoxColumn19
        '
        Me.DataGridViewTextBoxColumn19.DataPropertyName = "FirstName"
        Me.DataGridViewTextBoxColumn19.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn19.Name = "DataGridViewTextBoxColumn19"
        Me.DataGridViewTextBoxColumn19.ReadOnly = True
        Me.DataGridViewTextBoxColumn19.Width = 103
        '
        'DataGridViewTextBoxColumn17
        '
        Me.DataGridViewTextBoxColumn17.DataPropertyName = "LastName"
        Me.DataGridViewTextBoxColumn17.HeaderText = "Loan type"
        Me.DataGridViewTextBoxColumn17.Name = "DataGridViewTextBoxColumn17"
        Me.DataGridViewTextBoxColumn17.ReadOnly = True
        Me.DataGridViewTextBoxColumn17.Width = 103
        '
        'DataGridViewTextBoxColumn16
        '
        Me.DataGridViewTextBoxColumn16.DataPropertyName = "EmployeeID"
        DataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn16.DefaultCellStyle = DataGridViewCellStyle28
        Me.DataGridViewTextBoxColumn16.HeaderText = "Status"
        Me.DataGridViewTextBoxColumn16.Name = "DataGridViewTextBoxColumn16"
        Me.DataGridViewTextBoxColumn16.ReadOnly = True
        Me.DataGridViewTextBoxColumn16.Width = 103
        '
        'DataGridViewTextBoxColumn15
        '
        Me.DataGridViewTextBoxColumn15.DataPropertyName = "RowID"
        DataGridViewCellStyle29.Format = "N2"
        DataGridViewCellStyle29.NullValue = Nothing
        Me.DataGridViewTextBoxColumn15.DefaultCellStyle = DataGridViewCellStyle29
        Me.DataGridViewTextBoxColumn15.HeaderText = "RowiD"
        Me.DataGridViewTextBoxColumn15.Name = "DataGridViewTextBoxColumn15"
        Me.DataGridViewTextBoxColumn15.ReadOnly = True
        Me.DataGridViewTextBoxColumn15.Visible = False
        Me.DataGridViewTextBoxColumn15.Width = 103
        '
        'DataGridViewTextBoxColumn14
        '
        Me.DataGridViewTextBoxColumn14.DataPropertyName = "EmployeeID"
        DataGridViewCellStyle30.Format = "(0,000.00)"
        DataGridViewCellStyle30.NullValue = Nothing
        Me.DataGridViewTextBoxColumn14.DefaultCellStyle = DataGridViewCellStyle30
        Me.DataGridViewTextBoxColumn14.HeaderText = "Remarks"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        Me.DataGridViewTextBoxColumn14.ReadOnly = True
        Me.DataGridViewTextBoxColumn14.Visible = False
        Me.DataGridViewTextBoxColumn14.Width = 103
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.DataPropertyName = "LoanHasBonus"
        Me.DataGridViewTextBoxColumn13.HeaderText = "Deduction date from"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        Me.DataGridViewTextBoxColumn13.ReadOnly = True
        Me.DataGridViewTextBoxColumn13.Visible = False
        Me.DataGridViewTextBoxColumn13.Width = 103
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.DataPropertyName = "LoanType"
        DataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn12.DefaultCellStyle = DataGridViewCellStyle31
        Me.DataGridViewTextBoxColumn12.HeaderText = "No of pay period left"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.ReadOnly = True
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.DataPropertyName = "Status"
        DataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn11.DefaultCellStyle = DataGridViewCellStyle32
        Me.DataGridViewTextBoxColumn11.HeaderText = "No of pay period"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.ReadOnly = True
        Me.DataGridViewTextBoxColumn11.Visible = False
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "Deduction Schedule"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        '
        'DataGridViewTextBoxColumn18
        '
        Me.DataGridViewTextBoxColumn18.DataPropertyName = "FirstName"
        Me.DataGridViewTextBoxColumn18.HeaderText = "LoanHasBonus"
        Me.DataGridViewTextBoxColumn18.Name = "DataGridViewTextBoxColumn18"
        Me.DataGridViewTextBoxColumn18.ReadOnly = True
        Me.DataGridViewTextBoxColumn18.Visible = False
        Me.DataGridViewTextBoxColumn18.Width = 103
        '
        'EmployeeAllowanceForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1229, 547)
        Me.Controls.Add(Me.ShowAllCheckBox)
        Me.Controls.Add(Me.pnlMain)
        Me.Controls.Add(Me.employeesDataGridView)
        Me.Controls.Add(Me.pnlSearch)
        Me.Controls.Add(Me.lblFormTitle)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "EmployeeAllowanceForm"
        Me.Text = "EmployeeAllowanceForm"
        Me.EmployeeInfoTabLayout.ResumeLayout(False)
        Me.EmployeeInfoTabLayout.PerformLayout()
        CType(Me.EmployeePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip12.ResumeLayout(False)
        Me.ToolStrip12.PerformLayout()
        CType(Me.AllowancesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        Me.pnlForm.ResumeLayout(False)
        Me.DetailsTabLayout.ResumeLayout(False)
        Me.DetailsTabLayout.PerformLayout()
        Me.pnlTxtLoanBalance.ResumeLayout(False)
        Me.pnlTxtLoanBalance.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.plnCboLoanType.ResumeLayout(False)
        Me.plnCboLoanType.PerformLayout()
        CType(Me.AllowanceGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.employeesDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents EmployeeNumberTextBox As TextBox
    Friend WithEvents EmployeeNameTextBox As TextBox
    Friend WithEvents EmployeeInfoTabLayout As TableLayoutPanel
    Friend WithEvents EmployeePictureBox As PictureBox
    Friend WithEvents ToolStrip12 As ToolStrip
    Friend WithEvents NewToolStripButton As ToolStripButton
    Friend WithEvents SaveToolStripButton As ToolStripButton
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents DeleteToolStripButton As ToolStripButton
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents CancelToolStripButton As ToolStripButton
    Friend WithEvents btnClose As ToolStripButton
    Friend WithEvents ToolStripButton25 As ToolStripButton
    Friend WithEvents ImportToolStripButton As ToolStripButton
    Friend WithEvents DataGridViewTextBoxColumn19 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn17 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn16 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn15 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents AllowancesBindingSource As BindingSource
    Friend WithEvents pnlMain As Panel
    Friend WithEvents pnlForm As Panel
    Friend WithEvents cemp_FirstName As DataGridViewTextBoxColumn
    Friend WithEvents cemp_LastName As DataGridViewTextBoxColumn
    Friend WithEvents cemp_EmployeeID As DataGridViewTextBoxColumn
    Friend WithEvents employeesDataGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents SearchTextBox As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents pnlSearch As Panel
    Friend WithEvents lblFormTitle As Label
    Friend WithEvents DataGridViewTextBoxColumn18 As DataGridViewTextBoxColumn
    Friend WithEvents lnklbaddallowtype As LinkLabel
    Friend WithEvents AllowanceGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label156 As Label
    Friend WithEvents cboallowfreq As ComboBox
    Friend WithEvents eall_RowID As DataGridViewTextBoxColumn
    Friend WithEvents eall_Type As DataGridViewTextBoxColumn
    Friend WithEvents eall_Frequency As DataGridViewTextBoxColumn
    Friend WithEvents eall_Start As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents eall_End As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents eall_Amount As DataGridViewTextBoxColumn
    Friend WithEvents allow_taxab As DataGridViewTextBoxColumn
    Friend WithEvents eall_ProdID As DataGridViewTextBoxColumn
    Friend WithEvents ShowAllCheckBox As CheckBox
    Friend WithEvents cboallowtype As ComboBox
    Friend WithEvents DetailsTabLayout As TableLayoutPanel
    Friend WithEvents plnCboLoanType As Panel
    Friend WithEvents Label350 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label163 As Label
    Friend WithEvents dtpallowenddate As NullableDatePicker
    Friend WithEvents lblEndDate As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents dtpallowstartdate As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents Label167 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents pnlTxtLoanBalance As Panel
    Friend WithEvents txtallowamt As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents UserActivityToolStripButton As ToolStripButton
End Class
