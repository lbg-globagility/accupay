<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnRevalidate = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.ParsedTabControl = New System.Windows.Forms.TabPage()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.TimeAttendanceLogDataGrid = New System.Windows.Forms.DataGridView()
        Me.Column6 = New EWSoftware.ListControls.DataGridViewControls.AutoCompleteTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TimeAttendanceLogDataGridLogDate = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.TimeAttendanceLogDataGridDecrementLogDayButton = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.TimeAttendanceLogDataGridIncrementLogDayButton = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TimeAttendanceLogDataGridTimeInButton = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.TimeAttendanceLogDataGridTimeOutButton = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.ErrorsTabControl = New System.Windows.Forms.TabPage()
        Me.TimeAttendanceLogErrorsDataGrid = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtErrorSearch = New System.Windows.Forms.TextBox()
        Me.lblErrorSearch = New System.Windows.Forms.Label()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewDateColumn1 = New AccuPay.DataGridViewDateColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.ParsedTabControl.SuspendLayout()
        CType(Me.TimeAttendanceLogDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ErrorsTabControl.SuspendLayout()
        CType(Me.TimeAttendanceLogErrorsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnRevalidate)
        Me.Panel1.Controls.Add(Me.btnOK)
        Me.Panel1.Controls.Add(Me.btnClose)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 378)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(857, 72)
        Me.Panel1.TabIndex = 13
        '
        'btnRevalidate
        '
        Me.btnRevalidate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevalidate.BackColor = System.Drawing.Color.Yellow
        Me.btnRevalidate.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnRevalidate.Location = New System.Drawing.Point(15, 25)
        Me.btnRevalidate.Name = "btnRevalidate"
        Me.btnRevalidate.Size = New System.Drawing.Size(98, 35)
        Me.btnRevalidate.TabIndex = 12
        Me.btnRevalidate.Text = "&Revalidate Logs"
        Me.btnRevalidate.UseVisualStyleBackColor = False
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(689, 25)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 35)
        Me.btnOK.TabIndex = 10
        Me.btnOK.Text = "&Save"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(770, 25)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 35)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.ParsedTabControl)
        Me.TabControl1.Controls.Add(Me.ErrorsTabControl)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(857, 378)
        Me.TabControl1.TabIndex = 14
        '
        'ParsedTabControl
        '
        Me.ParsedTabControl.Controls.Add(Me.txtSearch)
        Me.ParsedTabControl.Controls.Add(Me.lblSearch)
        Me.ParsedTabControl.Controls.Add(Me.TimeAttendanceLogDataGrid)
        Me.ParsedTabControl.Controls.Add(Me.lblStatus)
        Me.ParsedTabControl.Location = New System.Drawing.Point(4, 22)
        Me.ParsedTabControl.Name = "ParsedTabControl"
        Me.ParsedTabControl.Padding = New System.Windows.Forms.Padding(3)
        Me.ParsedTabControl.Size = New System.Drawing.Size(849, 352)
        Me.ParsedTabControl.TabIndex = 0
        Me.ParsedTabControl.Text = "Ok"
        Me.ParsedTabControl.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(52, 32)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(394, 22)
        Me.txtSearch.TabIndex = 16
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(8, 36)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(41, 13)
        Me.lblSearch.TabIndex = 15
        Me.lblSearch.Text = "Search"
        '
        'TimeAttendanceLogDataGrid
        '
        Me.TimeAttendanceLogDataGrid.AllowUserToAddRows = False
        Me.TimeAttendanceLogDataGrid.AllowUserToDeleteRows = False
        Me.TimeAttendanceLogDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimeAttendanceLogDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TimeAttendanceLogDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column6, Me.Column7, Me.Column8, Me.TimeAttendanceLogDataGridLogDate, Me.TimeAttendanceLogDataGridDecrementLogDayButton, Me.TimeAttendanceLogDataGridIncrementLogDayButton, Me.Column1, Me.TimeAttendanceLogDataGridTimeInButton, Me.TimeAttendanceLogDataGridTimeOutButton})
        Me.TimeAttendanceLogDataGrid.Location = New System.Drawing.Point(8, 62)
        Me.TimeAttendanceLogDataGrid.Name = "TimeAttendanceLogDataGrid"
        Me.TimeAttendanceLogDataGrid.RowHeadersVisible = False
        Me.TimeAttendanceLogDataGrid.Size = New System.Drawing.Size(833, 284)
        Me.TimeAttendanceLogDataGrid.TabIndex = 14
        '
        'Column6
        '
        Me.Column6.DataPropertyName = "EmployeeFullName"
        Me.Column6.HeaderText = "Employee Name"
        Me.Column6.Name = "Column6"
        Me.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column6.Width = 180
        '
        'Column7
        '
        Me.Column7.DataPropertyName = "EmployeeNumber"
        Me.Column7.HeaderText = "Employee ID"
        Me.Column7.Name = "Column7"
        Me.Column7.Width = 110
        '
        'Column8
        '
        Me.Column8.DataPropertyName = "ShiftDescription"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column8.DefaultCellStyle = DataGridViewCellStyle1
        Me.Column8.HeaderText = "Shift"
        Me.Column8.Name = "Column8"
        Me.Column8.Width = 150
        '
        'TimeAttendanceLogDataGridLogDate
        '
        '
        '
        '
        Me.TimeAttendanceLogDataGridLogDate.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.TimeAttendanceLogDataGridLogDate.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.TimeAttendanceLogDataGridLogDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TimeAttendanceLogDataGridLogDate.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.TimeAttendanceLogDataGridLogDate.DataPropertyName = "LogDate"
        DataGridViewCellStyle2.Format = "MM/dd/yyyy"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.TimeAttendanceLogDataGridLogDate.DefaultCellStyle = DataGridViewCellStyle2
        Me.TimeAttendanceLogDataGridLogDate.HeaderText = "Work Day"
        Me.TimeAttendanceLogDataGridLogDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.DisplayMonth = New Date(2019, 3, 1, 0, 0, 0, 0)
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TimeAttendanceLogDataGridLogDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.TimeAttendanceLogDataGridLogDate.Name = "TimeAttendanceLogDataGridLogDate"
        Me.TimeAttendanceLogDataGridLogDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TimeAttendanceLogDataGridLogDate.Width = 80
        '
        'TimeAttendanceLogDataGridDecrementLogDayButton
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.DefaultCellStyle = DataGridViewCellStyle3
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.HeaderText = ""
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.Name = "TimeAttendanceLogDataGridDecrementLogDayButton"
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.Text = "-"
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.ToolTipText = "Decrement one day"
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.UseColumnTextForButtonValue = True
        Me.TimeAttendanceLogDataGridDecrementLogDayButton.Width = 25
        '
        'TimeAttendanceLogDataGridIncrementLogDayButton
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.DefaultCellStyle = DataGridViewCellStyle4
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.HeaderText = ""
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.Name = "TimeAttendanceLogDataGridIncrementLogDayButton"
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.Text = "+"
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.ToolTipText = "Increment one day"
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.UseColumnTextForButtonValue = True
        Me.TimeAttendanceLogDataGridIncrementLogDayButton.Width = 25
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "DateTime"
        DataGridViewCellStyle5.Format = "MM/dd/yyyy hh:mm tt"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column1.HeaderText = "Timestamp"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 120
        '
        'TimeAttendanceLogDataGridTimeInButton
        '
        Me.TimeAttendanceLogDataGridTimeInButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.TimeAttendanceLogDataGridTimeInButton.HeaderText = ""
        Me.TimeAttendanceLogDataGridTimeInButton.Name = "TimeAttendanceLogDataGridTimeInButton"
        Me.TimeAttendanceLogDataGridTimeInButton.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TimeAttendanceLogDataGridTimeInButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.TimeAttendanceLogDataGridTimeInButton.Text = "IN"
        Me.TimeAttendanceLogDataGridTimeInButton.UseColumnTextForButtonValue = True
        Me.TimeAttendanceLogDataGridTimeInButton.Width = 50
        '
        'TimeAttendanceLogDataGridTimeOutButton
        '
        Me.TimeAttendanceLogDataGridTimeOutButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.TimeAttendanceLogDataGridTimeOutButton.HeaderText = ""
        Me.TimeAttendanceLogDataGridTimeOutButton.Name = "TimeAttendanceLogDataGridTimeOutButton"
        Me.TimeAttendanceLogDataGridTimeOutButton.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TimeAttendanceLogDataGridTimeOutButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.TimeAttendanceLogDataGridTimeOutButton.Text = "OUT"
        Me.TimeAttendanceLogDataGridTimeOutButton.UseColumnTextForButtonValue = True
        Me.TimeAttendanceLogDataGridTimeOutButton.Width = 50
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Red
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblStatus.ForeColor = System.Drawing.Color.White
        Me.lblStatus.Location = New System.Drawing.Point(3, 3)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Padding = New System.Windows.Forms.Padding(4)
        Me.lblStatus.Size = New System.Drawing.Size(843, 21)
        Me.lblStatus.TabIndex = 13
        Me.lblStatus.Text = "There is 1 error. Failed error logs will not be saved."
        '
        'ErrorsTabControl
        '
        Me.ErrorsTabControl.Controls.Add(Me.TimeAttendanceLogErrorsDataGrid)
        Me.ErrorsTabControl.Controls.Add(Me.Panel2)
        Me.ErrorsTabControl.Location = New System.Drawing.Point(4, 22)
        Me.ErrorsTabControl.Name = "ErrorsTabControl"
        Me.ErrorsTabControl.Padding = New System.Windows.Forms.Padding(3)
        Me.ErrorsTabControl.Size = New System.Drawing.Size(849, 352)
        Me.ErrorsTabControl.TabIndex = 1
        Me.ErrorsTabControl.Text = "Errors"
        Me.ErrorsTabControl.UseVisualStyleBackColor = True
        '
        'TimeAttendanceLogErrorsDataGrid
        '
        Me.TimeAttendanceLogErrorsDataGrid.AllowUserToAddRows = False
        Me.TimeAttendanceLogErrorsDataGrid.AllowUserToDeleteRows = False
        Me.TimeAttendanceLogErrorsDataGrid.AllowUserToOrderColumns = True
        Me.TimeAttendanceLogErrorsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.TimeAttendanceLogErrorsDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.TimeAttendanceLogErrorsDataGrid.ColumnHeadersHeight = 34
        Me.TimeAttendanceLogErrorsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.TimeAttendanceLogErrorsDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.Column3})
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TimeAttendanceLogErrorsDataGrid.DefaultCellStyle = DataGridViewCellStyle8
        Me.TimeAttendanceLogErrorsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TimeAttendanceLogErrorsDataGrid.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.TimeAttendanceLogErrorsDataGrid.Location = New System.Drawing.Point(3, 44)
        Me.TimeAttendanceLogErrorsDataGrid.MultiSelect = False
        Me.TimeAttendanceLogErrorsDataGrid.Name = "TimeAttendanceLogErrorsDataGrid"
        Me.TimeAttendanceLogErrorsDataGrid.ReadOnly = True
        Me.TimeAttendanceLogErrorsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.TimeAttendanceLogErrorsDataGrid.Size = New System.Drawing.Size(843, 305)
        Me.TimeAttendanceLogErrorsDataGrid.TabIndex = 13
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "LineNumber"
        DataGridViewCellStyle6.Format = "MM/dd/yyyy hh:mm tt"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewTextBoxColumn3.FillWeight = 20.0!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Line Number"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "LineContent"
        DataGridViewCellStyle7.Format = "MM/dd/yyyy hh:mm tt"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn4.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn4.HeaderText = "Line Content"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "ErrorMessage"
        Me.Column3.FillWeight = 40.0!
        Me.Column3.HeaderText = "Reason"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.txtErrorSearch)
        Me.Panel2.Controls.Add(Me.lblErrorSearch)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(843, 41)
        Me.Panel2.TabIndex = 14
        '
        'txtErrorSearch
        '
        Me.txtErrorSearch.Location = New System.Drawing.Point(49, 7)
        Me.txtErrorSearch.Name = "txtErrorSearch"
        Me.txtErrorSearch.Size = New System.Drawing.Size(394, 22)
        Me.txtErrorSearch.TabIndex = 18
        '
        'lblErrorSearch
        '
        Me.lblErrorSearch.AutoSize = True
        Me.lblErrorSearch.Location = New System.Drawing.Point(5, 11)
        Me.lblErrorSearch.Name = "lblErrorSearch"
        Me.lblErrorSearch.Size = New System.Drawing.Size(41, 13)
        Me.lblErrorSearch.TabIndex = 17
        Me.lblErrorSearch.Text = "Search"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "EmployeeNo"
        Me.DataGridViewTextBoxColumn1.FillWeight = 20.0!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee ID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 106
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "DateTime"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn2.FillWeight = 80.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Timestamp"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 422
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "Reason"
        DataGridViewCellStyle10.Format = "MM/dd/yyyy hh:mm tt"
        DataGridViewCellStyle10.NullValue = Nothing
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle10
        Me.DataGridViewTextBoxColumn5.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn5.HeaderText = "Reason"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 211
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "Content"
        Me.DataGridViewTextBoxColumn6.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn6.HeaderText = "Line Content"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 211
        '
        'DataGridViewDateColumn1
        '
        Me.DataGridViewDateColumn1.DataPropertyName = "LogDate"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.Format = "M/d/yyyy"
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewDateColumn1.DefaultCellStyle = DataGridViewCellStyle11
        Me.DataGridViewDateColumn1.HeaderText = "Log Day"
        Me.DataGridViewDateColumn1.MaxInputLength = 11
        Me.DataGridViewDateColumn1.Name = "DataGridViewDateColumn1"
        Me.DataGridViewDateColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewDateColumn1.Width = 110
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "Reason"
        Me.DataGridViewTextBoxColumn7.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn7.HeaderText = "Reason"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Width = 211
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "DateTime"
        Me.DataGridViewTextBoxColumn8.FillWeight = 80.0!
        Me.DataGridViewTextBoxColumn8.HeaderText = "Timestamp"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Width = 211
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "Type"
        Me.DataGridViewTextBoxColumn9.FillWeight = 20.0!
        Me.DataGridViewTextBoxColumn9.HeaderText = "Type"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Width = 53
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "LineNumber"
        Me.DataGridViewTextBoxColumn10.FillWeight = 20.0!
        Me.DataGridViewTextBoxColumn10.HeaderText = "Line Number"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.Width = 113
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.DataPropertyName = "LineContent"
        Me.DataGridViewTextBoxColumn11.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn11.HeaderText = "Line Content"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.Width = 227
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.DataPropertyName = "ErrorMessage"
        Me.DataGridViewTextBoxColumn12.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn12.HeaderText = "Reason"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.Width = 226
        '
        'TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(857, 450)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import Preview"
        Me.Panel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.ParsedTabControl.ResumeLayout(False)
        Me.ParsedTabControl.PerformLayout()
        CType(Me.TimeAttendanceLogDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ErrorsTabControl.ResumeLayout(False)
        CType(Me.TimeAttendanceLogErrorsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnOK As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents ParsedTabControl As TabPage
    Friend WithEvents ErrorsTabControl As TabPage
    Friend WithEvents TimeAttendanceLogErrorsDataGrid As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents lblStatus As Label
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents TimeAttendanceLogDataGrid As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewDateColumn1 As DataGridViewDateColumn
    Friend WithEvents Column6 As EWSoftware.ListControls.DataGridViewControls.AutoCompleteTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents Column8 As DataGridViewTextBoxColumn
    Friend WithEvents TimeAttendanceLogDataGridLogDate As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents TimeAttendanceLogDataGridDecrementLogDayButton As DataGridViewButtonColumn
    Friend WithEvents TimeAttendanceLogDataGridIncrementLogDayButton As DataGridViewButtonColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents TimeAttendanceLogDataGridTimeInButton As DataGridViewButtonColumn
    Friend WithEvents TimeAttendanceLogDataGridTimeOutButton As DataGridViewButtonColumn
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtErrorSearch As TextBox
    Friend WithEvents lblErrorSearch As Label
    Friend WithEvents btnRevalidate As Button
End Class
