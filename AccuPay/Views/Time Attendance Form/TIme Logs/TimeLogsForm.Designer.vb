<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TimeLogsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TimeLogsForm))
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.MainSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.EmployeeTreeView1 = New EmployeeTreeView()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.CloseToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ImportToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExportToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.UserActivityToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.FilterButton = New System.Windows.Forms.Button()
        Me.dtpDateFrom = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dtpDateTo = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.grid = New System.Windows.Forms.DataGridView()
        Me.ActionPanel = New System.Windows.Forms.Panel()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnDiscard = New System.Windows.Forms.Button()
        Me.btnDeleteAll = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.labelChangedCount = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.bgworkTypicalImport = New System.ComponentModel.BackgroundWorker()
        Me.bgworkImport = New System.ComponentModel.BackgroundWorker()
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
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEmployeeNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFullName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colShift = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDateIn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBranchID = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colTimeIn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDateOut = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDateOutDisplay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDecrement = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colIncrement = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colTimeOut = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colIsExisting = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colHasChanged = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDelete = New System.Windows.Forms.DataGridViewImageColumn()
        Me.colRestore = New System.Windows.Forms.DataGridViewImageColumn()
        CType(Me.MainSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainSplitContainer.Panel1.SuspendLayout()
        Me.MainSplitContainer.Panel2.SuspendLayout()
        Me.MainSplitContainer.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ActionPanel.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblFormTitle
        '
        Me.lblFormTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblFormTitle.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFormTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblFormTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(1255, 28)
        Me.lblFormTitle.TabIndex = 315
        Me.lblFormTitle.Text = "EMPLOYEE TIME ENTRY LOGS"
        Me.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MainSplitContainer
        '
        Me.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainSplitContainer.Location = New System.Drawing.Point(0, 28)
        Me.MainSplitContainer.Name = "MainSplitContainer"
        '
        'MainSplitContainer.Panel1
        '
        Me.MainSplitContainer.Panel1.Controls.Add(Me.EmployeeTreeView1)
        '
        'MainSplitContainer.Panel2
        '
        Me.MainSplitContainer.Panel2.Controls.Add(Me.ToolStrip2)
        Me.MainSplitContainer.Panel2.Controls.Add(Me.SplitContainer2)
        Me.MainSplitContainer.Size = New System.Drawing.Size(1255, 585)
        Me.MainSplitContainer.SplitterDistance = 352
        Me.MainSplitContainer.TabIndex = 316
        '
        'EmployeeTreeView1
        '
        Me.EmployeeTreeView1.BackColor = System.Drawing.Color.Transparent
        Me.EmployeeTreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EmployeeTreeView1.Location = New System.Drawing.Point(0, 0)
        Me.EmployeeTreeView1.Name = "EmployeeTreeView1"
        Me.EmployeeTreeView1.Size = New System.Drawing.Size(352, 585)
        Me.EmployeeTreeView1.TabIndex = 316
        '
        'ToolStrip2
        '
        Me.ToolStrip2.BackColor = System.Drawing.Color.White
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseToolStripButton, Me.ImportToolStripButton, Me.ToolStripSeparator1, Me.ExportToolStripButton, Me.UserActivityToolStripButton, Me.ToolStripProgressBar1})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(899, 25)
        Me.ToolStrip2.TabIndex = 350
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'CloseToolStripButton
        '
        Me.CloseToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.CloseToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.CloseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CloseToolStripButton.Name = "CloseToolStripButton"
        Me.CloseToolStripButton.Size = New System.Drawing.Size(56, 22)
        Me.CloseToolStripButton.Text = "Close"
        '
        'ImportToolStripButton
        '
        Me.ImportToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Add
        Me.ImportToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ImportToolStripButton.Name = "ImportToolStripButton"
        Me.ImportToolStripButton.Size = New System.Drawing.Size(63, 22)
        Me.ImportToolStripButton.Text = "&Import"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ExportToolStripButton
        '
        Me.ExportToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Document
        Me.ExportToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ExportToolStripButton.Name = "ExportToolStripButton"
        Me.ExportToolStripButton.Size = New System.Drawing.Size(60, 22)
        Me.ExportToolStripButton.Text = "&Export"
        '
        'UserActivityToolStripButton
        '
        Me.UserActivityToolStripButton.Image = CType(resources.GetObject("UserActivityToolStripButton.Image"), System.Drawing.Image)
        Me.UserActivityToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.UserActivityToolStripButton.Name = "UserActivityToolStripButton"
        Me.UserActivityToolStripButton.Size = New System.Drawing.Size(93, 22)
        Me.UserActivityToolStripButton.Text = "&User Activity"
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 22)
        Me.ToolStripProgressBar1.Visible = False
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.BackColor = System.Drawing.Color.White
        Me.SplitContainer2.Panel1.Controls.Add(Me.Panel6)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.grid)
        Me.SplitContainer2.Panel2.Controls.Add(Me.ActionPanel)
        Me.SplitContainer2.Size = New System.Drawing.Size(899, 585)
        Me.SplitContainer2.SplitterDistance = 138
        Me.SplitContainer2.TabIndex = 0
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.GroupBox1)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel6.Location = New System.Drawing.Point(0, 0)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(899, 138)
        Me.Panel6.TabIndex = 353
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.FilterButton)
        Me.GroupBox1.Controls.Add(Me.dtpDateFrom)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.dtpDateTo)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(364, 138)
        Me.GroupBox1.TabIndex = 349
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filter Dates"
        '
        'FilterButton
        '
        Me.FilterButton.Location = New System.Drawing.Point(66, 75)
        Me.FilterButton.Name = "FilterButton"
        Me.FilterButton.Size = New System.Drawing.Size(75, 23)
        Me.FilterButton.TabIndex = 348
        Me.FilterButton.Text = "&Filter"
        Me.FilterButton.UseVisualStyleBackColor = True
        '
        'dtpDateFrom
        '
        Me.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDateFrom.Location = New System.Drawing.Point(6, 47)
        Me.dtpDateFrom.Name = "dtpDateFrom"
        Me.dtpDateFrom.Size = New System.Drawing.Size(95, 22)
        Me.dtpDateFrom.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(104, 31)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(18, 13)
        Me.Label6.TabIndex = 346
        Me.Label6.Text = "To"
        '
        'dtpDateTo
        '
        Me.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDateTo.Location = New System.Drawing.Point(107, 47)
        Me.dtpDateTo.Name = "dtpDateTo"
        Me.dtpDateTo.Size = New System.Drawing.Size(95, 22)
        Me.dtpDateTo.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(3, 31)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(33, 13)
        Me.Label5.TabIndex = 345
        Me.Label5.Text = "From"
        '
        'grid
        '
        Me.grid.AllowUserToAddRows = False
        Me.grid.AllowUserToDeleteRows = False
        Me.grid.AllowUserToResizeRows = False
        Me.grid.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colRowID, Me.colEmployeeID, Me.colEmployeeNo, Me.colFullName, Me.colShift, Me.colDay, Me.colDateIn, Me.colBranchID, Me.colTimeIn, Me.colDateOut, Me.colDateOutDisplay, Me.colDecrement, Me.colIncrement, Me.colTimeOut, Me.colIsExisting, Me.colHasChanged, Me.colDelete, Me.colRestore})
        Me.grid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grid.Location = New System.Drawing.Point(0, 31)
        Me.grid.Name = "grid"
        Me.grid.RowHeadersWidth = 36
        Me.grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.grid.Size = New System.Drawing.Size(899, 412)
        Me.grid.TabIndex = 336
        '
        'ActionPanel
        '
        Me.ActionPanel.BackColor = System.Drawing.Color.White
        Me.ActionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ActionPanel.Controls.Add(Me.btnSave)
        Me.ActionPanel.Controls.Add(Me.btnDiscard)
        Me.ActionPanel.Controls.Add(Me.btnDeleteAll)
        Me.ActionPanel.Controls.Add(Me.Panel2)
        Me.ActionPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ActionPanel.Location = New System.Drawing.Point(0, 0)
        Me.ActionPanel.Name = "ActionPanel"
        Me.ActionPanel.Size = New System.Drawing.Size(899, 31)
        Me.ActionPanel.TabIndex = 337
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(810, 3)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnDiscard
        '
        Me.btnDiscard.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDiscard.Location = New System.Drawing.Point(712, 3)
        Me.btnDiscard.Name = "btnDiscard"
        Me.btnDiscard.Size = New System.Drawing.Size(92, 23)
        Me.btnDiscard.TabIndex = 1
        Me.btnDiscard.Text = "Un&do Changes"
        Me.btnDiscard.UseVisualStyleBackColor = True
        '
        'btnDeleteAll
        '
        Me.btnDeleteAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteAll.Location = New System.Drawing.Point(567, 3)
        Me.btnDeleteAll.Name = "btnDeleteAll"
        Me.btnDeleteAll.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteAll.TabIndex = 2
        Me.btnDeleteAll.Text = "Delete All"
        Me.btnDeleteAll.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(309, 29)
        Me.Panel2.TabIndex = 3
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.labelChangedCount)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 9)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(309, 20)
        Me.Panel3.TabIndex = 0
        Me.Panel3.Visible = False
        '
        'labelChangedCount
        '
        Me.labelChangedCount.AutoSize = True
        Me.labelChangedCount.Dock = System.Windows.Forms.DockStyle.Left
        Me.labelChangedCount.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelChangedCount.ForeColor = System.Drawing.Color.Green
        Me.labelChangedCount.Location = New System.Drawing.Point(103, 0)
        Me.labelChangedCount.Name = "labelChangedCount"
        Me.labelChangedCount.Size = New System.Drawing.Size(13, 13)
        Me.labelChangedCount.TabIndex = 0
        Me.labelChangedCount.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(103, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Unsaved changes :"
        '
        'bgworkTypicalImport
        '
        '
        'bgworkImport
        '
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "RowID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "EmployeeID"
        Me.DataGridViewTextBoxColumn2.FillWeight = 20.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Employee No"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 10
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn2.Visible = False
        Me.DataGridViewTextBoxColumn2.Width = 80
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "EmployeeNo"
        Me.DataGridViewTextBoxColumn3.FillWeight = 140.0!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Full Name"
        Me.DataGridViewTextBoxColumn3.MinimumWidth = 10
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "FullName"
        DataGridViewCellStyle8.Format = "ddd"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn4.FillWeight = 28.62944!
        Me.DataGridViewTextBoxColumn4.HeaderText = "Day"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "DateIn"
        DataGridViewCellStyle9.Format = "ddd"
        DataGridViewCellStyle9.NullValue = Nothing
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn5.FillWeight = 28.62944!
        Me.DataGridViewTextBoxColumn5.HeaderText = "Date In"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "TimeIn"
        DataGridViewCellStyle10.Format = "ddd"
        DataGridViewCellStyle10.NullValue = Nothing
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle10
        Me.DataGridViewTextBoxColumn6.FillWeight = 28.62944!
        Me.DataGridViewTextBoxColumn6.HeaderText = "Time In"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn6.Width = 66
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "TimeOut"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle11
        Me.DataGridViewTextBoxColumn7.FillWeight = 28.62944!
        Me.DataGridViewTextBoxColumn7.HeaderText = "Date Out"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn7.Visible = False
        Me.DataGridViewTextBoxColumn7.Width = 65
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "IsExisting"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn8.DefaultCellStyle = DataGridViewCellStyle12
        Me.DataGridViewTextBoxColumn8.FillWeight = 28.0!
        Me.DataGridViewTextBoxColumn8.HeaderText = "Time Out"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn8.Visible = False
        Me.DataGridViewTextBoxColumn8.Width = 66
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "HasChanged"
        Me.DataGridViewTextBoxColumn9.HeaderText = "IsExisting"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn9.Visible = False
        Me.DataGridViewTextBoxColumn9.Width = 34
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "TimeOut"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn10.DefaultCellStyle = DataGridViewCellStyle13
        Me.DataGridViewTextBoxColumn10.HeaderText = "HasChanged"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn10.Visible = False
        Me.DataGridViewTextBoxColumn10.Width = 34
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.DataPropertyName = "IsExisting"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn11.DefaultCellStyle = DataGridViewCellStyle14
        Me.DataGridViewTextBoxColumn11.HeaderText = "IsExisting"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.ReadOnly = True
        Me.DataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn11.Visible = False
        Me.DataGridViewTextBoxColumn11.Width = 59
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.DataPropertyName = "HasChanged"
        Me.DataGridViewTextBoxColumn12.HeaderText = "HasChanged"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.ReadOnly = True
        Me.DataGridViewTextBoxColumn12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn12.Visible = False
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.DataPropertyName = "HasChanged"
        Me.DataGridViewTextBoxColumn13.HeaderText = "HasChanged"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        Me.DataGridViewTextBoxColumn13.ReadOnly = True
        Me.DataGridViewTextBoxColumn13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn13.Visible = False
        '
        'colRowID
        '
        Me.colRowID.DataPropertyName = "RowID"
        Me.colRowID.HeaderText = "RowID"
        Me.colRowID.Name = "colRowID"
        Me.colRowID.ReadOnly = True
        Me.colRowID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colRowID.Visible = False
        '
        'colEmployeeID
        '
        Me.colEmployeeID.DataPropertyName = "EmployeeID"
        Me.colEmployeeID.HeaderText = "EmployeeID"
        Me.colEmployeeID.Name = "colEmployeeID"
        Me.colEmployeeID.ReadOnly = True
        Me.colEmployeeID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colEmployeeID.Visible = False
        '
        'colEmployeeNo
        '
        Me.colEmployeeNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colEmployeeNo.DataPropertyName = "EmployeeNo"
        Me.colEmployeeNo.FillWeight = 20.0!
        Me.colEmployeeNo.HeaderText = "Employee No"
        Me.colEmployeeNo.MinimumWidth = 10
        Me.colEmployeeNo.Name = "colEmployeeNo"
        Me.colEmployeeNo.ReadOnly = True
        Me.colEmployeeNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colEmployeeNo.Width = 80
        '
        'colFullName
        '
        Me.colFullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colFullName.DataPropertyName = "FullName"
        Me.colFullName.FillWeight = 50.0!
        Me.colFullName.HeaderText = "Full Name"
        Me.colFullName.Name = "colFullName"
        Me.colFullName.ReadOnly = True
        Me.colFullName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colFullName.Width = 64
        '
        'colShift
        '
        Me.colShift.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colShift.DataPropertyName = "ShiftText"
        Me.colShift.HeaderText = "Shift Schedule"
        Me.colShift.Name = "colShift"
        Me.colShift.ReadOnly = True
        Me.colShift.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colShift.Width = 87
        '
        'colDay
        '
        Me.colDay.DataPropertyName = "DateIn"
        DataGridViewCellStyle1.Format = "ddd"
        DataGridViewCellStyle1.NullValue = Nothing
        Me.colDay.DefaultCellStyle = DataGridViewCellStyle1
        Me.colDay.FillWeight = 28.62944!
        Me.colDay.HeaderText = "Day"
        Me.colDay.Name = "colDay"
        Me.colDay.ReadOnly = True
        Me.colDay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colDay.Width = 34
        '
        'colDateIn
        '
        Me.colDateIn.DataPropertyName = "DateIn"
        Me.colDateIn.FillWeight = 28.62944!
        Me.colDateIn.HeaderText = "Date In"
        Me.colDateIn.Name = "colDateIn"
        Me.colDateIn.ReadOnly = True
        Me.colDateIn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colDateIn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colDateIn.Width = 66
        '
        'colBranchID
        '
        Me.colBranchID.DataPropertyName = "BranchID"
        Me.colBranchID.DropDownWidth = 375
        Me.colBranchID.HeaderText = "Branch"
        Me.colBranchID.Name = "colBranchID"
        Me.colBranchID.Width = 250
        '
        'colTimeIn
        '
        Me.colTimeIn.DataPropertyName = "TimeIn"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colTimeIn.DefaultCellStyle = DataGridViewCellStyle2
        Me.colTimeIn.FillWeight = 28.0!
        Me.colTimeIn.HeaderText = "Time In"
        Me.colTimeIn.Name = "colTimeIn"
        Me.colTimeIn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colTimeIn.Width = 49
        '
        'colDateOut
        '
        Me.colDateOut.DataPropertyName = "DateOut"
        Me.colDateOut.HeaderText = "Date Out"
        Me.colDateOut.Name = "colDateOut"
        Me.colDateOut.ReadOnly = True
        Me.colDateOut.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colDateOut.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colDateOut.Visible = False
        Me.colDateOut.Width = 66
        '
        'colDateOutDisplay
        '
        Me.colDateOutDisplay.DataPropertyName = "DateOutDisplay"
        Me.colDateOutDisplay.HeaderText = "Date Out"
        Me.colDateOutDisplay.Name = "colDateOutDisplay"
        Me.colDateOutDisplay.ReadOnly = True
        Me.colDateOutDisplay.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colDateOutDisplay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colDateOutDisplay.Width = 66
        '
        'colDecrement
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.NullValue = "-"
        Me.colDecrement.DefaultCellStyle = DataGridViewCellStyle3
        Me.colDecrement.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.colDecrement.HeaderText = ""
        Me.colDecrement.Name = "colDecrement"
        Me.colDecrement.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colDecrement.Width = 23
        '
        'colIncrement
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.NullValue = "+"
        Me.colIncrement.DefaultCellStyle = DataGridViewCellStyle4
        Me.colIncrement.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.colIncrement.HeaderText = ""
        Me.colIncrement.Name = "colIncrement"
        Me.colIncrement.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colIncrement.Width = 23
        '
        'colTimeOut
        '
        Me.colTimeOut.DataPropertyName = "TimeOut"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colTimeOut.DefaultCellStyle = DataGridViewCellStyle5
        Me.colTimeOut.HeaderText = "Time Out"
        Me.colTimeOut.Name = "colTimeOut"
        Me.colTimeOut.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colTimeOut.Width = 59
        '
        'colIsExisting
        '
        Me.colIsExisting.DataPropertyName = "IsExisting"
        Me.colIsExisting.HeaderText = "IsExisting"
        Me.colIsExisting.Name = "colIsExisting"
        Me.colIsExisting.ReadOnly = True
        Me.colIsExisting.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colIsExisting.Visible = False
        '
        'colHasChanged
        '
        Me.colHasChanged.DataPropertyName = "HasChanged"
        Me.colHasChanged.HeaderText = "HasChanged"
        Me.colHasChanged.Name = "colHasChanged"
        Me.colHasChanged.ReadOnly = True
        Me.colHasChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colHasChanged.Visible = False
        '
        'colDelete
        '
        Me.colDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.NullValue = Nothing
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Transparent
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Transparent
        Me.colDelete.DefaultCellStyle = DataGridViewCellStyle6
        Me.colDelete.HeaderText = ""
        Me.colDelete.Image = Global.AccuPay.My.Resources.Resources.baseline_delete_forever_black_18dp
        Me.colDelete.Name = "colDelete"
        Me.colDelete.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colDelete.ToolTipText = "Delete?"
        Me.colDelete.Width = 5
        '
        'colRestore
        '
        Me.colRestore.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.NullValue = Nothing
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Transparent
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Transparent
        Me.colRestore.DefaultCellStyle = DataGridViewCellStyle7
        Me.colRestore.HeaderText = ""
        Me.colRestore.Image = Global.AccuPay.My.Resources.Resources.baseline_undo_black_18dp
        Me.colRestore.Name = "colRestore"
        Me.colRestore.Width = 5
        '
        'TimeLogsForm2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(165, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1255, 613)
        Me.Controls.Add(Me.MainSplitContainer)
        Me.Controls.Add(Me.lblFormTitle)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "TimeLogsForm2"
        Me.MainSplitContainer.Panel1.ResumeLayout(False)
        Me.MainSplitContainer.Panel2.ResumeLayout(False)
        Me.MainSplitContainer.Panel2.PerformLayout()
        CType(Me.MainSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainSplitContainer.ResumeLayout(False)
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ActionPanel.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblFormTitle As Label
    Friend WithEvents MainSplitContainer As SplitContainer
    Friend WithEvents EmployeeTreeView1 As EmployeeTreeView
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents grid As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents dtpDateFrom As DateTimePicker
    Friend WithEvents Label6 As Label
    Friend WithEvents dtpDateTo As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents ActionPanel As Panel
    Friend WithEvents btnSave As Button
    Friend WithEvents btnDeleteAll As Button
    Friend WithEvents btnDiscard As Button
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents labelChangedCount As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As DataGridViewTextBoxColumn
    Friend WithEvents Panel6 As Panel
    Friend WithEvents DataGridViewTextBoxColumn13 As DataGridViewTextBoxColumn
    Friend WithEvents bgworkTypicalImport As System.ComponentModel.BackgroundWorker
    Friend WithEvents bgworkImport As System.ComponentModel.BackgroundWorker
    Friend WithEvents FilterButton As Button
    Friend WithEvents ToolStrip2 As ToolStrip
    Friend WithEvents CloseToolStripButton As ToolStripButton
    Friend WithEvents ImportToolStripButton As ToolStripButton
    Friend WithEvents UserActivityToolStripButton As ToolStripButton
    Friend WithEvents ToolStripProgressBar1 As ToolStripProgressBar
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ExportToolStripButton As ToolStripButton
    Friend WithEvents colRowID As DataGridViewTextBoxColumn
    Friend WithEvents colEmployeeID As DataGridViewTextBoxColumn
    Friend WithEvents colEmployeeNo As DataGridViewTextBoxColumn
    Friend WithEvents colFullName As DataGridViewTextBoxColumn
    Friend WithEvents colShift As DataGridViewTextBoxColumn
    Friend WithEvents colDay As DataGridViewTextBoxColumn
    Friend WithEvents colDateIn As DataGridViewTextBoxColumn
    Friend WithEvents colBranchID As DataGridViewComboBoxColumn
    Friend WithEvents colTimeIn As DataGridViewTextBoxColumn
    Friend WithEvents colDateOut As DataGridViewTextBoxColumn
    Friend WithEvents colDateOutDisplay As DataGridViewTextBoxColumn
    Friend WithEvents colDecrement As DataGridViewButtonColumn
    Friend WithEvents colIncrement As DataGridViewButtonColumn
    Friend WithEvents colTimeOut As DataGridViewTextBoxColumn
    Friend WithEvents colIsExisting As DataGridViewTextBoxColumn
    Friend WithEvents colHasChanged As DataGridViewTextBoxColumn
    Friend WithEvents colDelete As DataGridViewImageColumn
    Friend WithEvents colRestore As DataGridViewImageColumn
End Class
