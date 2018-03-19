<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmployeeShiftMassUpdate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EmployeeShiftMassUpdate))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.dgvEmpList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.ShiftEncodingTypeDisplayValue = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.c_EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_EmployeeName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShiftEncodingType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.tslblSelectedEmployee = New System.Windows.Forms.ToolStripLabel()
        Me.tslblRowCountFound = New System.Windows.Forms.ToolStripLabel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpDateFrom = New System.Windows.Forms.DateTimePicker()
        Me.dtpDateTo = New System.Windows.Forms.DateTimePicker()
        Me.chkbxNewShiftByDay = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboshiftlist = New System.Windows.Forms.ComboBox()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtEmpSearchBox = New System.Windows.Forms.TextBox()
        Me.lblShiftEntry = New System.Windows.Forms.LinkLabel()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.chkrestday = New System.Windows.Forms.CheckBox()
        Me.chkNightShift = New System.Windows.Forms.CheckBox()
        Me.Label140 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.lblShiftID = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpTimeTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpTimeFrom = New System.Windows.Forms.DateTimePicker()
        Me.bgEmpShiftImport = New System.ComponentModel.BackgroundWorker()
        Me.cmsDeleteShift = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteSelectedShiftToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        CType(Me.dgvEmpList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.cmsDeleteShift.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(588, 24)
        Me.Label16.TabIndex = 313
        Me.Label16.Text = "MASS UPDATE : EMPLOYEE SHIFT ENTRY"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvEmpList
        '
        Me.dgvEmpList.AllowUserToAddRows = False
        Me.dgvEmpList.AllowUserToDeleteRows = False
        Me.dgvEmpList.AllowUserToResizeColumns = False
        Me.dgvEmpList.AllowUserToResizeRows = False
        Me.dgvEmpList.BackgroundColor = System.Drawing.Color.White
        Me.dgvEmpList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvEmpList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ShiftEncodingTypeDisplayValue, Me.c_EmployeeID, Me.c_EmployeeName, Me.c_ID, Me.ShiftEncodingType})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEmpList.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEmpList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEmpList.Location = New System.Drawing.Point(4, 208)
        Me.dgvEmpList.MultiSelect = False
        Me.dgvEmpList.Name = "dgvEmpList"
        Me.dgvEmpList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvEmpList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmpList.Size = New System.Drawing.Size(509, 262)
        Me.dgvEmpList.TabIndex = 8
        '
        'ShiftEncodingTypeDisplayValue
        '
        Me.ShiftEncodingTypeDisplayValue.HeaderText = ""
        Me.ShiftEncodingTypeDisplayValue.Name = "ShiftEncodingTypeDisplayValue"
        Me.ShiftEncodingTypeDisplayValue.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ShiftEncodingTypeDisplayValue.Width = 45
        '
        'c_EmployeeID
        '
        Me.c_EmployeeID.HeaderText = "Employee ID"
        Me.c_EmployeeID.Name = "c_EmployeeID"
        Me.c_EmployeeID.ReadOnly = True
        Me.c_EmployeeID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.c_EmployeeID.Width = 120
        '
        'c_EmployeeName
        '
        Me.c_EmployeeName.HeaderText = "Employee Name"
        Me.c_EmployeeName.Name = "c_EmployeeName"
        Me.c_EmployeeName.ReadOnly = True
        Me.c_EmployeeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.c_EmployeeName.Width = 284
        '
        'c_ID
        '
        Me.c_ID.HeaderText = "RowID"
        Me.c_ID.Name = "c_ID"
        Me.c_ID.ReadOnly = True
        Me.c_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.c_ID.Visible = False
        '
        'ShiftEncodingType
        '
        Me.ShiftEncodingType.HeaderText = "ShiftEncodingType"
        Me.ShiftEncodingType.Name = "ShiftEncodingType"
        Me.ShiftEncodingType.ReadOnly = True
        Me.ShiftEncodingType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ShiftEncodingType.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ToolStrip2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.dtpDateFrom)
        Me.Panel1.Controls.Add(Me.dgvEmpList)
        Me.Panel1.Controls.Add(Me.dtpDateTo)
        Me.Panel1.Controls.Add(Me.chkbxNewShiftByDay)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.cboshiftlist)
        Me.Panel1.Controls.Add(Me.Panel5)
        Me.Panel1.Controls.Add(Me.lblShiftEntry)
        Me.Panel1.Controls.Add(Me.lblSaveMsg)
        Me.Panel1.Controls.Add(Me.chkrestday)
        Me.Panel1.Controls.Add(Me.chkNightShift)
        Me.Panel1.Controls.Add(Me.Label140)
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Controls.Add(Me.lblShiftID)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.dtpTimeTo)
        Me.Panel1.Controls.Add(Me.dtpTimeFrom)
        Me.Panel1.Location = New System.Drawing.Point(4, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(581, 500)
        Me.Panel1.TabIndex = 317
        '
        'ToolStrip2
        '
        Me.ToolStrip2.BackColor = System.Drawing.Color.White
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslblSelectedEmployee, Me.tslblRowCountFound})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 473)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(579, 25)
        Me.ToolStrip2.TabIndex = 351
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'tslblSelectedEmployee
        '
        Me.tslblSelectedEmployee.AutoSize = False
        Me.tslblSelectedEmployee.Name = "tslblSelectedEmployee"
        Me.tslblSelectedEmployee.Size = New System.Drawing.Size(160, 22)
        Me.tslblSelectedEmployee.Text = "selected employee(s) : "
        Me.tslblSelectedEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tslblRowCountFound
        '
        Me.tslblRowCountFound.AutoSize = False
        Me.tslblRowCountFound.Name = "tslblRowCountFound"
        Me.tslblRowCountFound.Size = New System.Drawing.Size(160, 22)
        Me.tslblRowCountFound.Text = "record(s) found : "
        Me.tslblRowCountFound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(18, 29)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 13)
        Me.Label5.TabIndex = 337
        Me.Label5.Text = "Date From"
        '
        'dtpDateFrom
        '
        Me.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDateFrom.Location = New System.Drawing.Point(21, 45)
        Me.dtpDateFrom.Name = "dtpDateFrom"
        Me.dtpDateFrom.Size = New System.Drawing.Size(105, 20)
        Me.dtpDateFrom.TabIndex = 0
        '
        'dtpDateTo
        '
        Me.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDateTo.Location = New System.Drawing.Point(147, 45)
        Me.dtpDateTo.Name = "dtpDateTo"
        Me.dtpDateTo.Size = New System.Drawing.Size(105, 20)
        Me.dtpDateTo.TabIndex = 1
        '
        'chkbxNewShiftByDay
        '
        Me.chkbxNewShiftByDay.AutoSize = True
        Me.chkbxNewShiftByDay.Location = New System.Drawing.Point(440, 109)
        Me.chkbxNewShiftByDay.Name = "chkbxNewShiftByDay"
        Me.chkbxNewShiftByDay.Size = New System.Drawing.Size(129, 17)
        Me.chkbxNewShiftByDay.TabIndex = 350
        Me.chkbxNewShiftByDay.Text = "chkbxNewShiftByDay"
        Me.chkbxNewShiftByDay.UseVisualStyleBackColor = True
        Me.chkbxNewShiftByDay.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(144, 29)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(46, 13)
        Me.Label6.TabIndex = 338
        Me.Label6.Text = "Date To"
        '
        'cboshiftlist
        '
        Me.cboshiftlist.FormattingEnabled = True
        Me.cboshiftlist.Location = New System.Drawing.Point(21, 85)
        Me.cboshiftlist.Name = "cboshiftlist"
        Me.cboshiftlist.Size = New System.Drawing.Size(231, 21)
        Me.cboshiftlist.TabIndex = 4
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.White
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.btnSearch)
        Me.Panel5.Controls.Add(Me.Label9)
        Me.Panel5.Controls.Add(Me.txtEmpSearchBox)
        Me.Panel5.Location = New System.Drawing.Point(3, 157)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(325, 45)
        Me.Panel5.TabIndex = 323
        '
        'btnSearch
        '
        Me.btnSearch.Image = CType(resources.GetObject("btnSearch.Image"), System.Drawing.Image)
        Me.btnSearch.Location = New System.Drawing.Point(293, 8)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(25, 25)
        Me.btnSearch.TabIndex = 7
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 19)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(41, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Search"
        '
        'txtEmpSearchBox
        '
        Me.txtEmpSearchBox.Location = New System.Drawing.Point(54, 12)
        Me.txtEmpSearchBox.Name = "txtEmpSearchBox"
        Me.txtEmpSearchBox.Size = New System.Drawing.Size(233, 20)
        Me.txtEmpSearchBox.TabIndex = 6
        '
        'lblShiftEntry
        '
        Me.lblShiftEntry.AutoSize = True
        Me.lblShiftEntry.Location = New System.Drawing.Point(258, 93)
        Me.lblShiftEntry.Name = "lblShiftEntry"
        Me.lblShiftEntry.Size = New System.Drawing.Size(163, 13)
        Me.lblShiftEntry.TabIndex = 5
        Me.lblShiftEntry.TabStop = True
        Me.lblShiftEntry.Text = "Add Shift Entry/Select Shift Entry"
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.BackColor = System.Drawing.Color.Transparent
        Me.lblSaveMsg.Location = New System.Drawing.Point(364, 110)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(70, 13)
        Me.lblSaveMsg.TabIndex = 340
        Me.lblSaveMsg.Text = "Employee ID:"
        Me.lblSaveMsg.Visible = False
        '
        'chkrestday
        '
        Me.chkrestday.AutoSize = True
        Me.chkrestday.Location = New System.Drawing.Point(339, 49)
        Me.chkrestday.Name = "chkrestday"
        Me.chkrestday.Size = New System.Drawing.Size(68, 17)
        Me.chkrestday.TabIndex = 3
        Me.chkrestday.Text = "Rest day"
        Me.chkrestday.UseVisualStyleBackColor = True
        '
        'chkNightShift
        '
        Me.chkNightShift.AutoSize = True
        Me.chkNightShift.Location = New System.Drawing.Point(258, 49)
        Me.chkNightShift.Name = "chkNightShift"
        Me.chkNightShift.Size = New System.Drawing.Size(75, 17)
        Me.chkNightShift.TabIndex = 2
        Me.chkNightShift.Text = "Night Shift"
        Me.chkNightShift.UseVisualStyleBackColor = True
        '
        'Label140
        '
        Me.Label140.AutoSize = True
        Me.Label140.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label140.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label140.Location = New System.Drawing.Point(4, 83)
        Me.Label140.Name = "Label140"
        Me.Label140.Size = New System.Drawing.Size(19, 23)
        Me.Label140.TabIndex = 343
        Me.Label140.Text = "*"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnSave, Me.ToolStripLabel1, Me.btnCancel, Me.btnClose})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(579, 25)
        Me.ToolStrip1.TabIndex = 327
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(78, 22)
        Me.btnSave.Text = "&Save Shift"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(50, 22)
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "Cancel"
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "&Close"
        '
        'lblShiftID
        '
        Me.lblShiftID.AutoSize = True
        Me.lblShiftID.BackColor = System.Drawing.Color.Transparent
        Me.lblShiftID.Location = New System.Drawing.Point(524, 169)
        Me.lblShiftID.Name = "lblShiftID"
        Me.lblShiftID.Size = New System.Drawing.Size(13, 13)
        Me.lblShiftID.TabIndex = 342
        Me.lblShiftID.Text = "0"
        Me.lblShiftID.Visible = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(485, 129)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 339
        Me.Label8.Text = "Time To"
        Me.Label8.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(359, 129)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 336
        Me.Label3.Text = "Time From"
        Me.Label3.Visible = False
        '
        'dtpTimeTo
        '
        Me.dtpTimeTo.Enabled = False
        Me.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpTimeTo.Location = New System.Drawing.Point(488, 145)
        Me.dtpTimeTo.Name = "dtpTimeTo"
        Me.dtpTimeTo.ShowUpDown = True
        Me.dtpTimeTo.Size = New System.Drawing.Size(105, 20)
        Me.dtpTimeTo.TabIndex = 333
        Me.dtpTimeTo.Visible = False
        '
        'dtpTimeFrom
        '
        Me.dtpTimeFrom.Enabled = False
        Me.dtpTimeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpTimeFrom.Location = New System.Drawing.Point(362, 145)
        Me.dtpTimeFrom.Name = "dtpTimeFrom"
        Me.dtpTimeFrom.ShowUpDown = True
        Me.dtpTimeFrom.Size = New System.Drawing.Size(105, 20)
        Me.dtpTimeFrom.TabIndex = 332
        Me.dtpTimeFrom.Visible = False
        '
        'bgEmpShiftImport
        '
        Me.bgEmpShiftImport.WorkerReportsProgress = True
        Me.bgEmpShiftImport.WorkerSupportsCancellation = True
        '
        'cmsDeleteShift
        '
        Me.cmsDeleteShift.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteSelectedShiftToolStripMenuItem})
        Me.cmsDeleteShift.Name = "cmsDeleteShift"
        Me.cmsDeleteShift.Size = New System.Drawing.Size(180, 26)
        '
        'DeleteSelectedShiftToolStripMenuItem
        '
        Me.DeleteSelectedShiftToolStripMenuItem.Name = "DeleteSelectedShiftToolStripMenuItem"
        Me.DeleteSelectedShiftToolStripMenuItem.Size = New System.Drawing.Size(179, 22)
        Me.DeleteSelectedShiftToolStripMenuItem.Text = "Delete selected shift"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee ID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn1.Width = 80
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Employee Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn2.Width = 180
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn3.Visible = False
        '
        'DataGridViewTextBoxColumn4
        '
        DataGridViewCellStyle2.Format = "d"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn4.HeaderText = "ShiftEncodingType"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn4.Visible = False
        '
        'DataGridViewTextBoxColumn5
        '
        DataGridViewCellStyle3.Format = "d"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewTextBoxColumn5.HeaderText = "Shift Encoding Type"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 150
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Employee ID"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Employee Name"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Width = 150
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Visible = False
        Me.DataGridViewTextBoxColumn8.Width = 80
        '
        'DataGridViewTextBoxColumn9
        '
        DataGridViewCellStyle4.Format = "d"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.DataGridViewTextBoxColumn9.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewTextBoxColumn9.HeaderText = "Effective Date From"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Width = 180
        '
        'DataGridViewTextBoxColumn10
        '
        DataGridViewCellStyle5.Format = "d"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.DataGridViewTextBoxColumn10.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumn10.HeaderText = "Effective Date To"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.Visible = False
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.HeaderText = "Time From"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.ReadOnly = True
        Me.DataGridViewTextBoxColumn11.Visible = False
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.HeaderText = "Time To"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.ReadOnly = True
        Me.DataGridViewTextBoxColumn12.Width = 150
        '
        'EmployeeShiftMassUpdate
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(588, 530)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label16)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "EmployeeShiftMassUpdate"
        Me.Text = "ShiftEntryForm"
        CType(Me.dgvEmpList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.cmsDeleteShift.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents dgvEmpList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpDateTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpDateFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpTimeTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpTimeFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
    Friend WithEvents lblShiftEntry As System.Windows.Forms.LinkLabel
    Friend WithEvents lblShiftID As System.Windows.Forms.Label
    Friend WithEvents chkNightShift As System.Windows.Forms.CheckBox
    Friend WithEvents chkrestday As System.Windows.Forms.CheckBox
    Friend WithEvents cboshiftlist As System.Windows.Forms.ComboBox
    Friend WithEvents Label140 As System.Windows.Forms.Label
    Friend WithEvents bgEmpShiftImport As System.ComponentModel.BackgroundWorker
    Friend WithEvents chkbxNewShiftByDay As System.Windows.Forms.CheckBox
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
    Friend WithEvents DataGridViewTextBoxColumn11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtEmpSearchBox As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cmsDeleteShift As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DeleteSelectedShiftToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShiftEncodingTypeDisplayValue As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents c_EmployeeID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_EmployeeName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShiftEncodingType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents tslblSelectedEmployee As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslblRowCountFound As System.Windows.Forms.ToolStripLabel
End Class
