<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CalendarsForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CalendarsForm))
        Me.ToolStrip12 = New System.Windows.Forms.ToolStrip()
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.CancelToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CloseButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton25 = New System.Windows.Forms.ToolStripButton()
        Me.ImportToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.SearchTextBox = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.FormTitleLabel = New System.Windows.Forms.Label()
        Me.MainPanel = New System.Windows.Forms.Panel()
        Me.CalendarPanel = New System.Windows.Forms.Panel()
        Me.CalendarsDataGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Column_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LeaveListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripTextBox1 = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripComboBox1 = New System.Windows.Forms.ToolStripComboBox()
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
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.DayTypesToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStrip12.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        Me.MainPanel.SuspendLayout()
        CType(Me.CalendarsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LeaveListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip12
        '
        Me.ToolStrip12.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip12.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip12.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.SaveToolStripButton, Me.ToolStripSeparator9, Me.DeleteToolStripButton, Me.ToolStripSeparator10, Me.CancelToolStripButton, Me.CloseButton, Me.ToolStripButton25, Me.ImportToolStripButton, Me.ToolStripSeparator1, Me.DayTypesToolStripButton})
        Me.ToolStrip12.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip12.Name = "ToolStrip12"
        Me.ToolStrip12.Size = New System.Drawing.Size(842, 25)
        Me.ToolStrip12.TabIndex = 3
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
        'DeleteToolStripButton
        '
        Me.DeleteToolStripButton.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripButton.Name = "DeleteToolStripButton"
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
        'CloseButton
        '
        Me.CloseButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.CloseButton.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.CloseButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(56, 22)
        Me.CloseButton.Text = "Close"
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
        'pnlSearch
        '
        Me.pnlSearch.BackColor = System.Drawing.Color.White
        Me.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSearch.Controls.Add(Me.SearchTextBox)
        Me.pnlSearch.Controls.Add(Me.lblSearch)
        Me.pnlSearch.Location = New System.Drawing.Point(8, 32)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(352, 56)
        Me.pnlSearch.TabIndex = 0
        '
        'SearchTextBox
        '
        Me.SearchTextBox.Location = New System.Drawing.Point(80, 16)
        Me.SearchTextBox.MaxLength = 50
        Me.SearchTextBox.Name = "SearchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(259, 22)
        Me.SearchTextBox.TabIndex = 0
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
        'FormTitleLabel
        '
        Me.FormTitleLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(174, Byte), Integer), CType(CType(194, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.FormTitleLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.FormTitleLabel.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.FormTitleLabel.Location = New System.Drawing.Point(0, 0)
        Me.FormTitleLabel.Name = "FormTitleLabel"
        Me.FormTitleLabel.Size = New System.Drawing.Size(1229, 24)
        Me.FormTitleLabel.TabIndex = 152
        Me.FormTitleLabel.Text = "Calendars"
        Me.FormTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MainPanel
        '
        Me.MainPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainPanel.AutoScroll = True
        Me.MainPanel.BackColor = System.Drawing.Color.White
        Me.MainPanel.Controls.Add(Me.CalendarPanel)
        Me.MainPanel.Controls.Add(Me.ToolStrip12)
        Me.MainPanel.Location = New System.Drawing.Point(375, 33)
        Me.MainPanel.Name = "MainPanel"
        Me.MainPanel.Size = New System.Drawing.Size(842, 503)
        Me.MainPanel.TabIndex = 156
        '
        'CalendarPanel
        '
        Me.CalendarPanel.AutoScroll = True
        Me.CalendarPanel.BackColor = System.Drawing.Color.Transparent
        Me.CalendarPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CalendarPanel.Location = New System.Drawing.Point(0, 25)
        Me.CalendarPanel.Name = "CalendarPanel"
        Me.CalendarPanel.Size = New System.Drawing.Size(842, 478)
        Me.CalendarPanel.TabIndex = 509
        '
        'CalendarsDataGridView
        '
        Me.CalendarsDataGridView.AllowUserToAddRows = False
        Me.CalendarsDataGridView.AllowUserToDeleteRows = False
        Me.CalendarsDataGridView.AllowUserToOrderColumns = True
        Me.CalendarsDataGridView.AllowUserToResizeRows = False
        Me.CalendarsDataGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CalendarsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.CalendarsDataGridView.BackgroundColor = System.Drawing.Color.White
        Me.CalendarsDataGridView.ColumnHeadersHeight = 34
        Me.CalendarsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.CalendarsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column_Name})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.CalendarsDataGridView.DefaultCellStyle = DataGridViewCellStyle1
        Me.CalendarsDataGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.CalendarsDataGridView.Location = New System.Drawing.Point(8, 96)
        Me.CalendarsDataGridView.MultiSelect = False
        Me.CalendarsDataGridView.Name = "CalendarsDataGridView"
        Me.CalendarsDataGridView.ReadOnly = True
        Me.CalendarsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.CalendarsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.CalendarsDataGridView.Size = New System.Drawing.Size(352, 439)
        Me.CalendarsDataGridView.TabIndex = 2
        '
        'Column_Name
        '
        Me.Column_Name.DataPropertyName = "Name"
        Me.Column_Name.HeaderText = "Calendar"
        Me.Column_Name.Name = "Column_Name"
        Me.Column_Name.ReadOnly = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AToolStripMenuItem, Me.ToolStripTextBox1, Me.ToolStripComboBox1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(182, 78)
        '
        'AToolStripMenuItem
        '
        Me.AToolStripMenuItem.Name = "AToolStripMenuItem"
        Me.AToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.AToolStripMenuItem.Text = "A"
        '
        'ToolStripTextBox1
        '
        Me.ToolStripTextBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ToolStripTextBox1.Name = "ToolStripTextBox1"
        Me.ToolStripTextBox1.Size = New System.Drawing.Size(100, 23)
        '
        'ToolStripComboBox1
        '
        Me.ToolStripComboBox1.Name = "ToolStripComboBox1"
        Me.ToolStripComboBox1.Size = New System.Drawing.Size(121, 23)
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "EmployeeNo"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee ID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 103
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "LastName"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 103
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "FirstName"
        Me.DataGridViewTextBoxColumn3.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 103
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "RowID"
        Me.DataGridViewTextBoxColumn4.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Visible = False
        Me.DataGridViewTextBoxColumn4.Width = 50
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "Type"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Type"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn5.Width = 180
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "AllowanceFrequency"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Frequency"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn6.Width = 180
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "Amount"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn7.HeaderText = "Amount"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn7.Width = 180
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "OTEndDate"
        Me.DataGridViewTextBoxColumn8.HeaderText = "Taxable"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Visible = False
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "Status"
        Me.DataGridViewTextBoxColumn9.HeaderText = "ProductID"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Visible = False
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "Reason"
        Me.DataGridViewTextBoxColumn10.HeaderText = "Reason"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.DataPropertyName = "Comments"
        Me.DataGridViewTextBoxColumn11.HeaderText = "Comments"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'DayTypesToolStripButton
        '
        Me.DayTypesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.DayTypesToolStripButton.Image = CType(resources.GetObject("DayTypesToolStripButton.Image"), System.Drawing.Image)
        Me.DayTypesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DayTypesToolStripButton.Name = "DayTypesToolStripButton"
        Me.DayTypesToolStripButton.Size = New System.Drawing.Size(63, 22)
        Me.DayTypesToolStripButton.Text = "Day Types"
        '
        'CalendarsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1229, 547)
        Me.Controls.Add(Me.pnlSearch)
        Me.Controls.Add(Me.FormTitleLabel)
        Me.Controls.Add(Me.MainPanel)
        Me.Controls.Add(Me.CalendarsDataGridView)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "CalendarsForm"
        Me.Text = "EmployeeLeavesForm"
        Me.ToolStrip12.ResumeLayout(False)
        Me.ToolStrip12.PerformLayout()
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        Me.MainPanel.ResumeLayout(False)
        Me.MainPanel.PerformLayout()
        CType(Me.CalendarsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LeaveListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ContextMenuStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStrip12 As ToolStrip
    Friend WithEvents NewToolStripButton As ToolStripButton
    Friend WithEvents SaveToolStripButton As ToolStripButton
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents DeleteToolStripButton As ToolStripButton
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents CancelToolStripButton As ToolStripButton
    Friend WithEvents CloseButton As ToolStripButton
    Friend WithEvents ToolStripButton25 As ToolStripButton
    Friend WithEvents ImportToolStripButton As ToolStripButton
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents pnlSearch As Panel
    Friend WithEvents SearchTextBox As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents FormTitleLabel As Label
    Friend WithEvents MainPanel As Panel
    Friend WithEvents CalendarPanel As Panel
    Friend WithEvents CalendarsDataGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents LeaveListBindingSource As BindingSource
    Friend WithEvents Column_Name As DataGridViewTextBoxColumn
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents AToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripTextBox1 As ToolStripTextBox
    Friend WithEvents ToolStripComboBox1 As ToolStripComboBox
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents DayTypesToolStripButton As ToolStripButton
End Class
