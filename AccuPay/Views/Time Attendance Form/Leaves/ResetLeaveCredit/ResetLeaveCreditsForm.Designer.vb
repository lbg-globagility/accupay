<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ResetLeaveCreditsForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ResetLeaveCreditsForm))
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.gridPeriods = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.gridEmployees = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnImport = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.btnUserActivities = New System.Windows.Forms.ToolStripButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewCheckBoxColumn1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewCheckBoxColumn2 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewCheckBoxColumn3 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.IsSelected = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.EmployeeNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StartDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateRegularized = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.VacationLeaveCredit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SickLeaveCredit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IsApplied = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.gridPeriods, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridPeriods
        '
        Me.gridPeriods.AllowUserToAddRows = False
        Me.gridPeriods.AllowUserToDeleteRows = False
        Me.gridPeriods.AllowUserToOrderColumns = True
        Me.gridPeriods.AllowUserToResizeColumns = False
        Me.gridPeriods.AllowUserToResizeRows = False
        Me.gridPeriods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gridPeriods.BackgroundColor = System.Drawing.Color.White
        Me.gridPeriods.ColumnHeadersHeight = 34
        Me.gridPeriods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridPeriods.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridPeriods.DefaultCellStyle = DataGridViewCellStyle1
        Me.gridPeriods.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.gridPeriods.Location = New System.Drawing.Point(12, 6)
        Me.gridPeriods.MultiSelect = False
        Me.gridPeriods.Name = "gridPeriods"
        Me.gridPeriods.ReadOnly = True
        Me.gridPeriods.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridPeriods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridPeriods.Size = New System.Drawing.Size(253, 325)
        Me.gridPeriods.TabIndex = 138
        '
        'gridEmployees
        '
        Me.gridEmployees.AllowUserToAddRows = False
        Me.gridEmployees.AllowUserToDeleteRows = False
        Me.gridEmployees.AllowUserToOrderColumns = True
        Me.gridEmployees.AllowUserToResizeRows = False
        Me.gridEmployees.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridEmployees.BackgroundColor = System.Drawing.Color.White
        Me.gridEmployees.ColumnHeadersHeight = 34
        Me.gridEmployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridEmployees.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IsSelected, Me.EmployeeNo, Me.LastName, Me.FirstName, Me.StartDate, Me.DateRegularized, Me.VacationLeaveCredit, Me.SickLeaveCredit, Me.IsApplied})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridEmployees.DefaultCellStyle = DataGridViewCellStyle7
        Me.gridEmployees.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.gridEmployees.Location = New System.Drawing.Point(271, 6)
        Me.gridEmployees.MultiSelect = False
        Me.gridEmployees.Name = "gridEmployees"
        Me.gridEmployees.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridEmployees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridEmployees.Size = New System.Drawing.Size(517, 325)
        Me.gridEmployees.TabIndex = 139
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.ToolStripLabel2, Me.btnDelete, Me.ToolStripLabel3, Me.btnCancel, Me.btnImport, Me.btnClose, Me.ToolStripLabel1, Me.btnUserActivities})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(800, 25)
        Me.ToolStrip1.TabIndex = 140
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(51, 22)
        Me.btnNew.Text = "&New"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(51, 22)
        Me.btnSave.Text = "&Save"
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(52, 22)
        Me.ToolStripLabel2.Text = "               "
        '
        'btnDelete
        '
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(60, 22)
        Me.btnDelete.Text = "&Delete"
        '
        'ToolStripLabel3
        '
        Me.ToolStripLabel3.Name = "ToolStripLabel3"
        Me.ToolStripLabel3.Size = New System.Drawing.Size(52, 22)
        Me.ToolStripLabel3.Text = "               "
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'btnImport
        '
        Me.btnImport.Image = Global.AccuPay.My.Resources.Resources.Add
        Me.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(63, 22)
        Me.btnImport.Text = "&Import"
        Me.btnImport.Visible = False
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
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(52, 22)
        Me.ToolStripLabel1.Text = "               "
        '
        'btnUserActivities
        '
        Me.btnUserActivities.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnUserActivities.Image = CType(resources.GetObject("btnUserActivities.Image"), System.Drawing.Image)
        Me.btnUserActivities.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUserActivities.Name = "btnUserActivities"
        Me.btnUserActivities.Size = New System.Drawing.Size(77, 22)
        Me.btnUserActivities.Text = "&User Activity"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.gridPeriods)
        Me.Panel1.Controls.Add(Me.gridEmployees)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 69)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(800, 337)
        Me.Panel1.TabIndex = 141
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnApply)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 406)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(800, 44)
        Me.Panel2.TabIndex = 142
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.Location = New System.Drawing.Point(608, 6)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(180, 31)
        Me.btnApply.TabIndex = 0
        Me.btnApply.Text = "Apply Leave Credits"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 25)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(800, 44)
        Me.Panel3.TabIndex = 143
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "PeriodDisplay"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Effectivity Start"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "PeriodText"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Month Period"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewCheckBoxColumn1
        '
        Me.DataGridViewCheckBoxColumn1.DataPropertyName = "IsSelected"
        Me.DataGridViewCheckBoxColumn1.HeaderText = ""
        Me.DataGridViewCheckBoxColumn1.Name = "DataGridViewCheckBoxColumn1"
        Me.DataGridViewCheckBoxColumn1.Width = 32
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "StartDate"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Start Date"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "VacationLeaveCredit"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Vacation Leave Credit"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "SickLeaveCredit"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Sick Leave Credit"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "IsApplied"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Applied"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 32
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "VacationLeaveCredit"
        DataGridViewCellStyle8.Format = "N2"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn7.HeaderText = "Vacation Leave Credit"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "SickLeaveCredit"
        DataGridViewCellStyle9.Format = "N2"
        DataGridViewCellStyle9.NullValue = Nothing
        Me.DataGridViewTextBoxColumn8.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn8.HeaderText = "Sick Leave Credit"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "SickLeaveCredit"
        DataGridViewCellStyle10.Format = "N2"
        DataGridViewCellStyle10.NullValue = Nothing
        Me.DataGridViewTextBoxColumn9.DefaultCellStyle = DataGridViewCellStyle10
        Me.DataGridViewTextBoxColumn9.HeaderText = "Sick Leave Credit"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "IsAppliedText"
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Wingdings", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.DataGridViewTextBoxColumn10.DefaultCellStyle = DataGridViewCellStyle11
        Me.DataGridViewTextBoxColumn10.HeaderText = "Applied"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "PeriodDisplay"
        Me.Column1.HeaderText = "Effectivity Start"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "PeriodText"
        Me.Column2.HeaderText = "Month Period"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'DataGridViewCheckBoxColumn2
        '
        Me.DataGridViewCheckBoxColumn2.DataPropertyName = "LastName"
        Me.DataGridViewCheckBoxColumn2.HeaderText = "Last Name"
        Me.DataGridViewCheckBoxColumn2.Name = "DataGridViewCheckBoxColumn2"
        Me.DataGridViewCheckBoxColumn2.ReadOnly = True
        Me.DataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCheckBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxColumn2.Width = 32
        '
        'DataGridViewCheckBoxColumn3
        '
        Me.DataGridViewCheckBoxColumn3.DataPropertyName = "FirstName"
        Me.DataGridViewCheckBoxColumn3.HeaderText = "First Name"
        Me.DataGridViewCheckBoxColumn3.Name = "DataGridViewCheckBoxColumn3"
        Me.DataGridViewCheckBoxColumn3.ReadOnly = True
        '
        'IsSelected
        '
        Me.IsSelected.DataPropertyName = "IsSelected"
        Me.IsSelected.HeaderText = ""
        Me.IsSelected.Name = "IsSelected"
        Me.IsSelected.Width = 32
        '
        'EmployeeNo
        '
        Me.EmployeeNo.DataPropertyName = "EmployeeNo"
        Me.EmployeeNo.HeaderText = "Employee ID"
        Me.EmployeeNo.Name = "EmployeeNo"
        Me.EmployeeNo.ReadOnly = True
        '
        'LastName
        '
        Me.LastName.DataPropertyName = "LastName"
        Me.LastName.HeaderText = "Last Name"
        Me.LastName.Name = "LastName"
        Me.LastName.ReadOnly = True
        Me.LastName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.LastName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'FirstName
        '
        Me.FirstName.DataPropertyName = "FirstName"
        Me.FirstName.HeaderText = "First Name"
        Me.FirstName.Name = "FirstName"
        Me.FirstName.ReadOnly = True
        Me.FirstName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.FirstName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'StartDate
        '
        Me.StartDate.DataPropertyName = "StartDate"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.StartDate.DefaultCellStyle = DataGridViewCellStyle2
        Me.StartDate.HeaderText = "Start Date"
        Me.StartDate.Name = "StartDate"
        Me.StartDate.ReadOnly = True
        '
        'DateRegularized
        '
        Me.DateRegularized.DataPropertyName = "DateRegularized"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.NullValue = "-"
        Me.DateRegularized.DefaultCellStyle = DataGridViewCellStyle3
        Me.DateRegularized.HeaderText = "Date Regularized"
        Me.DateRegularized.Name = "DateRegularized"
        Me.DateRegularized.ReadOnly = True
        '
        'VacationLeaveCredit
        '
        Me.VacationLeaveCredit.DataPropertyName = "VacationLeaveCredit"
        DataGridViewCellStyle4.Format = "N2"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.VacationLeaveCredit.DefaultCellStyle = DataGridViewCellStyle4
        Me.VacationLeaveCredit.HeaderText = "Vacation Leave Credit"
        Me.VacationLeaveCredit.Name = "VacationLeaveCredit"
        '
        'SickLeaveCredit
        '
        Me.SickLeaveCredit.DataPropertyName = "SickLeaveCredit"
        DataGridViewCellStyle5.Format = "N2"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.SickLeaveCredit.DefaultCellStyle = DataGridViewCellStyle5
        Me.SickLeaveCredit.HeaderText = "Sick Leave Credit"
        Me.SickLeaveCredit.Name = "SickLeaveCredit"
        '
        'IsApplied
        '
        Me.IsApplied.DataPropertyName = "IsAppliedText"
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Green
        Me.IsApplied.DefaultCellStyle = DataGridViewCellStyle6
        Me.IsApplied.HeaderText = "Is Applied?"
        Me.IsApplied.Name = "IsApplied"
        Me.IsApplied.ReadOnly = True
        Me.IsApplied.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'ResetLeaveCreditsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ResetLeaveCreditsForm"
        CType(Me.gridPeriods, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridEmployees, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents gridPeriods As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents gridEmployees As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents btnSave As ToolStripButton
    Friend WithEvents btnCancel As ToolStripButton
    Friend WithEvents btnImport As ToolStripButton
    Friend WithEvents btnClose As ToolStripButton
    Friend WithEvents btnUserActivities As ToolStripButton
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnApply As Button
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumn1 As DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumn2 As DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumn3 As DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents ToolStripLabel2 As ToolStripLabel
    Friend WithEvents ToolStripLabel3 As ToolStripLabel
    Friend WithEvents IsSelected As DataGridViewCheckBoxColumn
    Friend WithEvents EmployeeNo As DataGridViewTextBoxColumn
    Friend WithEvents LastName As DataGridViewTextBoxColumn
    Friend WithEvents FirstName As DataGridViewTextBoxColumn
    Friend WithEvents StartDate As DataGridViewTextBoxColumn
    Friend WithEvents DateRegularized As DataGridViewTextBoxColumn
    Friend WithEvents VacationLeaveCredit As DataGridViewTextBoxColumn
    Friend WithEvents SickLeaveCredit As DataGridViewTextBoxColumn
    Friend WithEvents IsApplied As DataGridViewTextBoxColumn
End Class
