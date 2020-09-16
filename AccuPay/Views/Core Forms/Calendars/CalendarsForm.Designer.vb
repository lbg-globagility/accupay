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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip12 = New System.Windows.Forms.ToolStrip()
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.CancelToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CloseButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.DayTypesToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.FormTitleLabel = New System.Windows.Forms.Label()
        Me.MainPanel = New System.Windows.Forms.Panel()
        Me.CalendarLabel = New System.Windows.Forms.Label()
        Me.CalendarPanel = New System.Windows.Forms.Panel()
        Me.MonthSelectorControl = New AccuPay.CalendarMonthSelectorControl()
        Me.CalendarsDataGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Column_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.ToolStrip12.SuspendLayout()
        Me.MainPanel.SuspendLayout()
        CType(Me.CalendarsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip12
        '
        Me.ToolStrip12.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip12.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip12.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.SaveToolStripButton, Me.ToolStripSeparator9, Me.CancelToolStripButton, Me.CloseButton, Me.ToolStripSeparator1, Me.DeleteToolStripButton, Me.DayTypesToolStripButton})
        Me.ToolStrip12.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip12.Name = "ToolStrip12"
        Me.ToolStrip12.Size = New System.Drawing.Size(849, 25)
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
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'DeleteToolStripButton
        '
        Me.DeleteToolStripButton.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripButton.Name = "DeleteToolStripButton"
        Me.DeleteToolStripButton.Size = New System.Drawing.Size(60, 22)
        Me.DeleteToolStripButton.Text = "Delete"
        '
        'DayTypesToolStripButton
        '
        Me.DayTypesToolStripButton.Image = Global.AccuPay.My.Resources.Resources.application_view_list_icon
        Me.DayTypesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DayTypesToolStripButton.Name = "DayTypesToolStripButton"
        Me.DayTypesToolStripButton.Size = New System.Drawing.Size(79, 22)
        Me.DayTypesToolStripButton.Text = "Day Types"
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
        Me.MainPanel.Controls.Add(Me.CalendarLabel)
        Me.MainPanel.Controls.Add(Me.CalendarPanel)
        Me.MainPanel.Controls.Add(Me.MonthSelectorControl)
        Me.MainPanel.Controls.Add(Me.ToolStrip12)
        Me.MainPanel.Location = New System.Drawing.Point(368, 33)
        Me.MainPanel.Name = "MainPanel"
        Me.MainPanel.Size = New System.Drawing.Size(849, 503)
        Me.MainPanel.TabIndex = 156
        '
        'CalendarLabel
        '
        Me.CalendarLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CalendarLabel.BackColor = System.Drawing.Color.Transparent
        Me.CalendarLabel.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CalendarLabel.Location = New System.Drawing.Point(136, 24)
        Me.CalendarLabel.Name = "CalendarLabel"
        Me.CalendarLabel.Size = New System.Drawing.Size(712, 32)
        Me.CalendarLabel.TabIndex = 511
        Me.CalendarLabel.Text = "<CalendarLabel>"
        Me.CalendarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CalendarPanel
        '
        Me.CalendarPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CalendarPanel.AutoScroll = True
        Me.CalendarPanel.BackColor = System.Drawing.Color.Transparent
        Me.CalendarPanel.Location = New System.Drawing.Point(136, 56)
        Me.CalendarPanel.Name = "CalendarPanel"
        Me.CalendarPanel.Size = New System.Drawing.Size(712, 448)
        Me.CalendarPanel.TabIndex = 509
        '
        'MonthSelectorControl
        '
        Me.MonthSelectorControl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MonthSelectorControl.CalendarName = ""
        Me.MonthSelectorControl.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MonthSelectorControl.Location = New System.Drawing.Point(0, 24)
        Me.MonthSelectorControl.Margin = New System.Windows.Forms.Padding(0)
        Me.MonthSelectorControl.Name = "MonthSelectorControl"
        Me.MonthSelectorControl.Size = New System.Drawing.Size(136, 480)
        Me.MonthSelectorControl.TabIndex = 510
        Me.MonthSelectorControl.Year = 0
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
        Me.CalendarsDataGridView.Location = New System.Drawing.Point(8, 32)
        Me.CalendarsDataGridView.MultiSelect = False
        Me.CalendarsDataGridView.Name = "CalendarsDataGridView"
        Me.CalendarsDataGridView.ReadOnly = True
        Me.CalendarsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.CalendarsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.CalendarsDataGridView.Size = New System.Drawing.Size(352, 503)
        Me.CalendarsDataGridView.TabIndex = 2
        '
        'Column_Name
        '
        Me.Column_Name.DataPropertyName = "Name"
        Me.Column_Name.HeaderText = "Calendar"
        Me.Column_Name.Name = "Column_Name"
        Me.Column_Name.ReadOnly = True
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
        'CalendarsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1229, 547)
        Me.Controls.Add(Me.FormTitleLabel)
        Me.Controls.Add(Me.MainPanel)
        Me.Controls.Add(Me.CalendarsDataGridView)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "CalendarsForm"
        Me.Text = "EmployeeLeavesForm"
        Me.ToolStrip12.ResumeLayout(False)
        Me.ToolStrip12.PerformLayout()
        Me.MainPanel.ResumeLayout(False)
        Me.MainPanel.PerformLayout()
        CType(Me.CalendarsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStrip12 As ToolStrip
    Friend WithEvents NewToolStripButton As ToolStripButton
    Friend WithEvents SaveToolStripButton As ToolStripButton
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents CancelToolStripButton As ToolStripButton
    Friend WithEvents CloseButton As ToolStripButton
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents FormTitleLabel As Label
    Friend WithEvents MainPanel As Panel
    Friend WithEvents CalendarPanel As Panel
    Friend WithEvents CalendarsDataGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents Column_Name As DataGridViewTextBoxColumn
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents DayTypesToolStripButton As ToolStripButton
    Friend WithEvents MonthSelectorControl As CalendarMonthSelectorControl
    Friend WithEvents CalendarLabel As Label
    Friend WithEvents DeleteToolStripButton As ToolStripButton
End Class
