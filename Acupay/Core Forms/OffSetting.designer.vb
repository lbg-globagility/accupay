<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OffSetting
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
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvemployees = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.eRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eEmpID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eFName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eMidName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eLName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eGender = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eEmpStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ePayFreq = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ePosition = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ePositionID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ePayFreqID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eEmpType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eOffsetBal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eFullName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eDetails = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eImage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblOffSetBal = New System.Windows.Forms.Label()
        Me.txtFName = New System.Windows.Forms.TextBox()
        Me.txtEmpID = New System.Windows.Forms.TextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.dgvempoffset = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.eosRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eosType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eosStartTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eosEndTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eosStartDate = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.eosEndDate = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.eosStatus = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.eosReason = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eosComment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lnkLast = New System.Windows.Forms.LinkLabel()
        Me.lnkNxt = New System.Windows.Forms.LinkLabel()
        Me.lnkPrev = New System.Windows.Forms.LinkLabel()
        Me.lnkFirst = New System.Windows.Forms.LinkLabel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNew = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSave = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnDelete = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnClose = New System.Windows.Forms.ToolStripButton()
        Me.lblforballoon = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.AutoCompleteTextBox1 = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.Last = New System.Windows.Forms.LinkLabel()
        Me.Nxt = New System.Windows.Forms.LinkLabel()
        Me.Prev = New System.Windows.Forms.LinkLabel()
        Me.First = New System.Windows.Forms.LinkLabel()
        Me.bgwork = New System.ComponentModel.BackgroundWorker()
        Me.bgworkSearchAutoComplete = New System.ComponentModel.BackgroundWorker()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.dgvemployees, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.dgvempoffset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvemployees
        '
        Me.dgvemployees.AllowUserToAddRows = False
        Me.dgvemployees.AllowUserToDeleteRows = False
        Me.dgvemployees.AllowUserToOrderColumns = True
        Me.dgvemployees.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvemployees.BackgroundColor = System.Drawing.Color.White
        Me.dgvemployees.ColumnHeadersHeight = 38
        Me.dgvemployees.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eRowID, Me.eEmpID, Me.eFName, Me.eMidName, Me.eLName, Me.eGender, Me.eEmpStatus, Me.ePayFreq, Me.ePosition, Me.ePositionID, Me.ePayFreqID, Me.eEmpType, Me.eOffsetBal, Me.eFullName, Me.eDetails, Me.eImage})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvemployees.DefaultCellStyle = DataGridViewCellStyle7
        Me.dgvemployees.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvemployees.Location = New System.Drawing.Point(12, 166)
        Me.dgvemployees.MultiSelect = False
        Me.dgvemployees.Name = "dgvemployees"
        Me.dgvemployees.ReadOnly = True
        Me.dgvemployees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvemployees.ShowCellErrors = False
        Me.dgvemployees.ShowCellToolTips = False
        Me.dgvemployees.ShowEditingIcon = False
        Me.dgvemployees.ShowRowErrors = False
        Me.dgvemployees.Size = New System.Drawing.Size(324, 327)
        Me.dgvemployees.TabIndex = 323
        '
        'eRowID
        '
        Me.eRowID.HeaderText = "RowID"
        Me.eRowID.Name = "eRowID"
        Me.eRowID.ReadOnly = True
        Me.eRowID.Visible = False
        '
        'eEmpID
        '
        Me.eEmpID.HeaderText = "Employee ID"
        Me.eEmpID.Name = "eEmpID"
        Me.eEmpID.ReadOnly = True
        '
        'eFName
        '
        Me.eFName.HeaderText = "First Name"
        Me.eFName.Name = "eFName"
        Me.eFName.ReadOnly = True
        '
        'eMidName
        '
        Me.eMidName.HeaderText = "Middle Name"
        Me.eMidName.Name = "eMidName"
        Me.eMidName.ReadOnly = True
        '
        'eLName
        '
        Me.eLName.HeaderText = "Last Name"
        Me.eLName.Name = "eLName"
        Me.eLName.ReadOnly = True
        '
        'eGender
        '
        Me.eGender.HeaderText = "Gender"
        Me.eGender.Name = "eGender"
        Me.eGender.ReadOnly = True
        '
        'eEmpStatus
        '
        Me.eEmpStatus.HeaderText = "Employment Status"
        Me.eEmpStatus.Name = "eEmpStatus"
        Me.eEmpStatus.ReadOnly = True
        '
        'ePayFreq
        '
        Me.ePayFreq.HeaderText = "Pay Frequency"
        Me.ePayFreq.Name = "ePayFreq"
        Me.ePayFreq.ReadOnly = True
        '
        'ePosition
        '
        Me.ePosition.HeaderText = "Position"
        Me.ePosition.Name = "ePosition"
        Me.ePosition.ReadOnly = True
        '
        'ePositionID
        '
        Me.ePositionID.HeaderText = "PositionID"
        Me.ePositionID.Name = "ePositionID"
        Me.ePositionID.ReadOnly = True
        Me.ePositionID.Visible = False
        '
        'ePayFreqID
        '
        Me.ePayFreqID.HeaderText = "Pay Frequecny RowID"
        Me.ePayFreqID.Name = "ePayFreqID"
        Me.ePayFreqID.ReadOnly = True
        Me.ePayFreqID.Visible = False
        '
        'eEmpType
        '
        Me.eEmpType.HeaderText = "Employee Type"
        Me.eEmpType.Name = "eEmpType"
        Me.eEmpType.ReadOnly = True
        '
        'eOffsetBal
        '
        Me.eOffsetBal.HeaderText = "Offset Balance"
        Me.eOffsetBal.Name = "eOffsetBal"
        Me.eOffsetBal.ReadOnly = True
        '
        'eFullName
        '
        Me.eFullName.HeaderText = "eFullName"
        Me.eFullName.Name = "eFullName"
        Me.eFullName.ReadOnly = True
        Me.eFullName.Visible = False
        '
        'eDetails
        '
        Me.eDetails.HeaderText = "eDetails"
        Me.eDetails.Name = "eDetails"
        Me.eDetails.ReadOnly = True
        Me.eDetails.Visible = False
        '
        'eImage
        '
        Me.eImage.HeaderText = "Image"
        Me.eImage.Name = "eImage"
        Me.eImage.ReadOnly = True
        Me.eImage.Visible = False
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.TabControl1.ItemSize = New System.Drawing.Size(62, 25)
        Me.TabControl1.Location = New System.Drawing.Point(338, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(806, 505)
        Me.TabControl1.TabIndex = 324
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Panel2)
        Me.TabPage1.Controls.Add(Me.ToolStrip1)
        Me.TabPage1.Controls.Add(Me.lblforballoon)
        Me.TabPage1.Location = New System.Drawing.Point(4, 4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(798, 472)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "OFFSET            "
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblOffSetBal)
        Me.Panel2.Controls.Add(Me.txtFName)
        Me.Panel2.Controls.Add(Me.txtEmpID)
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.lnkLast)
        Me.Panel2.Controls.Add(Me.lnkNxt)
        Me.Panel2.Controls.Add(Me.lnkPrev)
        Me.Panel2.Controls.Add(Me.lnkFirst)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 28)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(792, 441)
        Me.Panel2.TabIndex = 1
        '
        'lblOffSetBal
        '
        Me.lblOffSetBal.Font = New System.Drawing.Font("Courier New", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOffSetBal.Location = New System.Drawing.Point(1, 89)
        Me.lblOffSetBal.Name = "lblOffSetBal"
        Me.lblOffSetBal.Size = New System.Drawing.Size(521, 23)
        Me.lblOffSetBal.TabIndex = 336
        Me.lblOffSetBal.Text = "Label1"
        '
        'txtFName
        '
        Me.txtFName.BackColor = System.Drawing.Color.White
        Me.txtFName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFName.Font = New System.Drawing.Font("Nyala", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFName.Location = New System.Drawing.Point(6, 18)
        Me.txtFName.MaxLength = 250
        Me.txtFName.Name = "txtFName"
        Me.txtFName.ReadOnly = True
        Me.txtFName.Size = New System.Drawing.Size(516, 26)
        Me.txtFName.TabIndex = 335
        '
        'txtEmpID
        '
        Me.txtEmpID.BackColor = System.Drawing.Color.White
        Me.txtEmpID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpID.Font = New System.Drawing.Font("Nyala", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmpID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpID.Location = New System.Drawing.Point(6, 50)
        Me.txtEmpID.MaxLength = 50
        Me.txtEmpID.Name = "txtEmpID"
        Me.txtEmpID.ReadOnly = True
        Me.txtEmpID.Size = New System.Drawing.Size(668, 20)
        Me.txtEmpID.TabIndex = 334
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.Controls.Add(Me.dgvempoffset)
        Me.Panel3.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel3.Location = New System.Drawing.Point(6, 131)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(783, 292)
        Me.Panel3.TabIndex = 333
        '
        'dgvempoffset
        '
        Me.dgvempoffset.AllowUserToDeleteRows = False
        Me.dgvempoffset.AllowUserToOrderColumns = True
        Me.dgvempoffset.BackgroundColor = System.Drawing.Color.White
        Me.dgvempoffset.ColumnHeadersHeight = 35
        Me.dgvempoffset.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eosRowID, Me.eosType, Me.eosStartTime, Me.eosEndTime, Me.eosStartDate, Me.eosEndDate, Me.eosStatus, Me.eosReason, Me.eosComment})
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempoffset.DefaultCellStyle = DataGridViewCellStyle8
        Me.dgvempoffset.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvempoffset.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempoffset.Location = New System.Drawing.Point(0, 0)
        Me.dgvempoffset.Name = "dgvempoffset"
        Me.dgvempoffset.ShowCellToolTips = False
        Me.dgvempoffset.Size = New System.Drawing.Size(783, 292)
        Me.dgvempoffset.TabIndex = 324
        '
        'eosRowID
        '
        Me.eosRowID.HeaderText = "RowID"
        Me.eosRowID.Name = "eosRowID"
        Me.eosRowID.Visible = False
        '
        'eosType
        '
        Me.eosType.HeaderText = "Type"
        Me.eosType.Name = "eosType"
        Me.eosType.Visible = False
        '
        'eosStartTime
        '
        Me.eosStartTime.HeaderText = "Start Time"
        Me.eosStartTime.Name = "eosStartTime"
        '
        'eosEndTime
        '
        Me.eosEndTime.HeaderText = "End Time"
        Me.eosEndTime.Name = "eosEndTime"
        '
        'eosStartDate
        '
        '
        '
        '
        Me.eosStartDate.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.eosStartDate.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.eosStartDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eosStartDate.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.eosStartDate.HeaderText = "Start Date"
        Me.eosStartDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.eosStartDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eosStartDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.eosStartDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.eosStartDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.eosStartDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eosStartDate.MonthCalendar.DisplayMonth = New Date(2016, 2, 1, 0, 0, 0, 0)
        Me.eosStartDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.eosStartDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eosStartDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.eosStartDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eosStartDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.eosStartDate.Name = "eosStartDate"
        Me.eosStartDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'eosEndDate
        '
        '
        '
        '
        Me.eosEndDate.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.eosEndDate.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.eosEndDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eosEndDate.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.eosEndDate.HeaderText = "End Date"
        Me.eosEndDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.eosEndDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eosEndDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.eosEndDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.eosEndDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.eosEndDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eosEndDate.MonthCalendar.DisplayMonth = New Date(2016, 2, 1, 0, 0, 0, 0)
        Me.eosEndDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.eosEndDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.eosEndDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.eosEndDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.eosEndDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.eosEndDate.Name = "eosEndDate"
        Me.eosEndDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'eosStatus
        '
        Me.eosStatus.HeaderText = "Status"
        Me.eosStatus.Name = "eosStatus"
        Me.eosStatus.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eosStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.eosStatus.Width = 150
        '
        'eosReason
        '
        Me.eosReason.HeaderText = "Reason"
        Me.eosReason.MaxInputLength = 500
        Me.eosReason.Name = "eosReason"
        Me.eosReason.Width = 250
        '
        'eosComment
        '
        Me.eosComment.HeaderText = "Comment"
        Me.eosComment.MaxInputLength = 2000
        Me.eosComment.Name = "eosComment"
        Me.eosComment.Width = 450
        '
        'lnkLast
        '
        Me.lnkLast.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lnkLast.AutoSize = True
        Me.lnkLast.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkLast.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkLast.Location = New System.Drawing.Point(266, 426)
        Me.lnkLast.Name = "lnkLast"
        Me.lnkLast.Size = New System.Drawing.Size(44, 15)
        Me.lnkLast.TabIndex = 332
        Me.lnkLast.TabStop = True
        Me.lnkLast.Text = "Last>>"
        '
        'lnkNxt
        '
        Me.lnkNxt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lnkNxt.AutoSize = True
        Me.lnkNxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkNxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkNxt.Location = New System.Drawing.Point(221, 426)
        Me.lnkNxt.Name = "lnkNxt"
        Me.lnkNxt.Size = New System.Drawing.Size(39, 15)
        Me.lnkNxt.TabIndex = 331
        Me.lnkNxt.TabStop = True
        Me.lnkNxt.Text = "Next>"
        '
        'lnkPrev
        '
        Me.lnkPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lnkPrev.AutoSize = True
        Me.lnkPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkPrev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkPrev.Location = New System.Drawing.Point(53, 426)
        Me.lnkPrev.Name = "lnkPrev"
        Me.lnkPrev.Size = New System.Drawing.Size(38, 15)
        Me.lnkPrev.TabIndex = 330
        Me.lnkPrev.TabStop = True
        Me.lnkPrev.Text = "<Prev"
        '
        'lnkFirst
        '
        Me.lnkFirst.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lnkFirst.AutoSize = True
        Me.lnkFirst.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkFirst.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkFirst.Location = New System.Drawing.Point(3, 426)
        Me.lnkFirst.Name = "lnkFirst"
        Me.lnkFirst.Size = New System.Drawing.Size(44, 15)
        Me.lnkFirst.TabIndex = 329
        Me.lnkFirst.TabStop = True
        Me.lnkFirst.Text = "<<First"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNew, Me.tsbtnSave, Me.tsbtnCancel, Me.ToolStripLabel1, Me.tsbtnDelete, Me.tsbtnClose})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(792, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNew
        '
        Me.tsbtnNew.Image = Global.Acupay.My.Resources.Resources._new
        Me.tsbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNew.Name = "tsbtnNew"
        Me.tsbtnNew.Size = New System.Drawing.Size(51, 22)
        Me.tsbtnNew.Text = "&New"
        '
        'tsbtnSave
        '
        Me.tsbtnSave.Image = Global.Acupay.My.Resources.Resources.Save
        Me.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSave.Name = "tsbtnSave"
        Me.tsbtnSave.Size = New System.Drawing.Size(51, 22)
        Me.tsbtnSave.Text = "&Save"
        '
        'tsbtnCancel
        '
        Me.tsbtnCancel.Image = Global.Acupay.My.Resources.Resources.cancel1
        Me.tsbtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancel.Name = "tsbtnCancel"
        Me.tsbtnCancel.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancel.Text = "Cancel"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(97, 22)
        Me.ToolStripLabel1.Text = "                              "
        '
        'tsbtnDelete
        '
        Me.tsbtnDelete.Image = Global.Acupay.My.Resources.Resources.CLOSE_00
        Me.tsbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDelete.Name = "tsbtnDelete"
        Me.tsbtnDelete.Size = New System.Drawing.Size(60, 22)
        Me.tsbtnDelete.Text = "Delete"
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnClose.Image = Global.Acupay.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnClose.Name = "tsbtnClose"
        Me.tsbtnClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnClose.Text = "Close"
        '
        'lblforballoon
        '
        Me.lblforballoon.AutoSize = True
        Me.lblforballoon.Location = New System.Drawing.Point(59, 8)
        Me.lblforballoon.Name = "lblforballoon"
        Me.lblforballoon.Size = New System.Drawing.Size(63, 13)
        Me.lblforballoon.TabIndex = 330
        Me.lblforballoon.Text = "lblforballoon"
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(1151, 21)
        Me.Label25.TabIndex = 325
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.AutoCompleteTextBox1)
        Me.Panel1.Controls.Add(Me.btnRefresh)
        Me.Panel1.Controls.Add(Me.Last)
        Me.Panel1.Controls.Add(Me.Nxt)
        Me.Panel1.Controls.Add(Me.Prev)
        Me.Panel1.Controls.Add(Me.First)
        Me.Panel1.Controls.Add(Me.TabControl1)
        Me.Panel1.Controls.Add(Me.dgvemployees)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 21)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1151, 520)
        Me.Panel1.TabIndex = 326
        '
        'AutoCompleteTextBox1
        '
        Me.AutoCompleteTextBox1.Enabled = False
        Me.AutoCompleteTextBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AutoCompleteTextBox1.Location = New System.Drawing.Point(12, 64)
        Me.AutoCompleteTextBox1.Name = "AutoCompleteTextBox1"
        Me.AutoCompleteTextBox1.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.AutoCompleteTextBox1.PopupOffset = New System.Drawing.Point(12, 0)
        Me.AutoCompleteTextBox1.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.AutoCompleteTextBox1.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.AutoCompleteTextBox1.PopupWidth = 300
        Me.AutoCompleteTextBox1.Size = New System.Drawing.Size(324, 25)
        Me.AutoCompleteTextBox1.TabIndex = 330
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(261, 114)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 329
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'Last
        '
        Me.Last.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Last.AutoSize = True
        Me.Last.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Last.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Last.Location = New System.Drawing.Point(292, 496)
        Me.Last.Name = "Last"
        Me.Last.Size = New System.Drawing.Size(44, 15)
        Me.Last.TabIndex = 328
        Me.Last.TabStop = True
        Me.Last.Text = "Last>>"
        '
        'Nxt
        '
        Me.Nxt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Nxt.AutoSize = True
        Me.Nxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Nxt.Location = New System.Drawing.Point(247, 496)
        Me.Nxt.Name = "Nxt"
        Me.Nxt.Size = New System.Drawing.Size(39, 15)
        Me.Nxt.TabIndex = 327
        Me.Nxt.TabStop = True
        Me.Nxt.Text = "Next>"
        '
        'Prev
        '
        Me.Prev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Prev.AutoSize = True
        Me.Prev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Prev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Prev.Location = New System.Drawing.Point(59, 496)
        Me.Prev.Name = "Prev"
        Me.Prev.Size = New System.Drawing.Size(38, 15)
        Me.Prev.TabIndex = 326
        Me.Prev.TabStop = True
        Me.Prev.Text = "<Prev"
        '
        'First
        '
        Me.First.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.First.AutoSize = True
        Me.First.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.First.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.First.Location = New System.Drawing.Point(9, 496)
        Me.First.Name = "First"
        Me.First.Size = New System.Drawing.Size(44, 15)
        Me.First.TabIndex = 325
        Me.First.TabStop = True
        Me.First.Text = "<<First"
        '
        'bgworkSearchAutoComplete
        '
        Me.bgworkSearchAutoComplete.WorkerReportsProgress = True
        Me.bgworkSearchAutoComplete.WorkerSupportsCancellation = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 331
        Me.Label1.Text = "Search box"
        '
        'OffSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1151, 541)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label25)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "OffSetting"
        Me.Text = "OffSetting"
        CType(Me.dgvemployees, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        CType(Me.dgvempoffset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvemployees As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Last As System.Windows.Forms.LinkLabel
    Friend WithEvents Nxt As System.Windows.Forms.LinkLabel
    Friend WithEvents Prev As System.Windows.Forms.LinkLabel
    Friend WithEvents First As System.Windows.Forms.LinkLabel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents tsbtnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents lnkLast As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkNxt As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkPrev As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkFirst As System.Windows.Forms.LinkLabel
    Friend WithEvents dgvempoffset As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents eosRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eosType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eosStartTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eosEndTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eosStartDate As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents eosEndDate As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents eosStatus As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents eosReason As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eosComment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bgwork As System.ComponentModel.BackgroundWorker
    Friend WithEvents tsbtnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents lblforballoon As System.Windows.Forms.Label
    Friend WithEvents txtFName As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpID As System.Windows.Forms.TextBox
    Friend WithEvents eRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eEmpID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eFName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eMidName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eLName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eGender As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eEmpStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ePayFreq As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ePosition As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ePositionID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ePayFreqID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eEmpType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eOffsetBal As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eFullName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eDetails As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eImage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblOffSetBal As System.Windows.Forms.Label
    Friend WithEvents AutoCompleteTextBox1 As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents bgworkSearchAutoComplete As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
