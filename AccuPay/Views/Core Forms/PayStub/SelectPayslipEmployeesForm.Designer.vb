<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectPayslipEmployeesForm
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectPayslipEmployeesForm))
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ActionPanel = New System.Windows.Forms.Panel()
        Me.UncheckAllButton = New System.Windows.Forms.Button()
        Me.CloseDialogButton = New System.Windows.Forms.Button()
        Me.EmployeeGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.PreviewToolStripButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me.PreviewDeclaredToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PreviewActualToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SendEmailToolStripButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me.SendEmailDeclaredToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SendEmailActualToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ManageEmailToolStripDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ResetEmailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RefreshEmailStatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RefreshEmailServiceToolStripButton = New System.Windows.Forms.ToolStripButton()
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
        Me.SelectedCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmailAddressColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmailStatusColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PayslipTypeColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ResetEmailButtonColumn = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.ErrorLogMessageColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        Me.ActionPanel.SuspendLayout()
        CType(Me.EmployeeGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ActionPanel)
        Me.Panel1.Controls.Add(Me.CloseDialogButton)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 413)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1234, 50)
        Me.Panel1.TabIndex = 0
        '
        'ActionPanel
        '
        Me.ActionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ActionPanel.Controls.Add(Me.UncheckAllButton)
        Me.ActionPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.ActionPanel.Location = New System.Drawing.Point(0, 0)
        Me.ActionPanel.Name = "ActionPanel"
        Me.ActionPanel.Size = New System.Drawing.Size(456, 50)
        Me.ActionPanel.TabIndex = 6
        '
        'UncheckAllButton
        '
        Me.UncheckAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UncheckAllButton.Location = New System.Drawing.Point(11, 15)
        Me.UncheckAllButton.Name = "UncheckAllButton"
        Me.UncheckAllButton.Size = New System.Drawing.Size(103, 23)
        Me.UncheckAllButton.TabIndex = 9
        Me.UncheckAllButton.Text = "&Uncheck All"
        Me.UncheckAllButton.UseVisualStyleBackColor = True
        '
        'CloseDialogButton
        '
        Me.CloseDialogButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseDialogButton.Location = New System.Drawing.Point(1120, 15)
        Me.CloseDialogButton.Name = "CloseDialogButton"
        Me.CloseDialogButton.Size = New System.Drawing.Size(96, 23)
        Me.CloseDialogButton.TabIndex = 3
        Me.CloseDialogButton.Text = "&Close"
        Me.CloseDialogButton.UseVisualStyleBackColor = True
        '
        'EmployeeGridView
        '
        Me.EmployeeGridView.AllowUserToAddRows = False
        Me.EmployeeGridView.AllowUserToDeleteRows = False
        Me.EmployeeGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.EmployeeGridView.BackgroundColor = System.Drawing.Color.White
        Me.EmployeeGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.EmployeeGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.SelectedCheckBoxColumn, Me.Column1, Me.Column3, Me.Column6, Me.Column2, Me.EmailAddressColumn, Me.Column4, Me.Column5, Me.Column7, Me.EmailStatusColumn, Me.PayslipTypeColumn, Me.ResetEmailButtonColumn, Me.ErrorLogMessageColumn})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.EmployeeGridView.DefaultCellStyle = DataGridViewCellStyle6
        Me.EmployeeGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EmployeeGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.EmployeeGridView.Location = New System.Drawing.Point(0, 46)
        Me.EmployeeGridView.Name = "EmployeeGridView"
        Me.EmployeeGridView.Size = New System.Drawing.Size(1234, 367)
        Me.EmployeeGridView.TabIndex = 1
        '
        'StatusLabel
        '
        Me.StatusLabel.BackColor = System.Drawing.Color.Black
        Me.StatusLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.StatusLabel.ForeColor = System.Drawing.Color.White
        Me.StatusLabel.Location = New System.Drawing.Point(0, 25)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Padding = New System.Windows.Forms.Padding(4)
        Me.StatusLabel.Size = New System.Drawing.Size(1234, 21)
        Me.StatusLabel.TabIndex = 14
        Me.StatusLabel.Text = "Tick the checkbox of the employee that you want to include."
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PreviewToolStripButton, Me.SendEmailToolStripButton, Me.ManageEmailToolStripDropDownButton, Me.RefreshEmailServiceToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1234, 25)
        Me.ToolStrip1.TabIndex = 15
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'PreviewToolStripButton
        '
        Me.PreviewToolStripButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PreviewDeclaredToolStripMenuItem, Me.PreviewActualToolStripMenuItem})
        Me.PreviewToolStripButton.Image = CType(resources.GetObject("PreviewToolStripButton.Image"), System.Drawing.Image)
        Me.PreviewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PreviewToolStripButton.Name = "PreviewToolStripButton"
        Me.PreviewToolStripButton.Size = New System.Drawing.Size(77, 22)
        Me.PreviewToolStripButton.Text = "&Preview"
        '
        'PreviewDeclaredToolStripMenuItem
        '
        Me.PreviewDeclaredToolStripMenuItem.Name = "PreviewDeclaredToolStripMenuItem"
        Me.PreviewDeclaredToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.PreviewDeclaredToolStripMenuItem.Text = "Declared"
        '
        'PreviewActualToolStripMenuItem
        '
        Me.PreviewActualToolStripMenuItem.Name = "PreviewActualToolStripMenuItem"
        Me.PreviewActualToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.PreviewActualToolStripMenuItem.Text = "Actual"
        '
        'SendEmailToolStripButton
        '
        Me.SendEmailToolStripButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SendEmailDeclaredToolStripMenuItem, Me.SendEmailActualToolStripMenuItem})
        Me.SendEmailToolStripButton.Image = CType(resources.GetObject("SendEmailToolStripButton.Image"), System.Drawing.Image)
        Me.SendEmailToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SendEmailToolStripButton.Name = "SendEmailToolStripButton"
        Me.SendEmailToolStripButton.Size = New System.Drawing.Size(99, 22)
        Me.SendEmailToolStripButton.Text = "Send &Emails"
        '
        'SendEmailDeclaredToolStripMenuItem
        '
        Me.SendEmailDeclaredToolStripMenuItem.Name = "SendEmailDeclaredToolStripMenuItem"
        Me.SendEmailDeclaredToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.SendEmailDeclaredToolStripMenuItem.Text = "Declared"
        '
        'SendEmailActualToolStripMenuItem
        '
        Me.SendEmailActualToolStripMenuItem.Name = "SendEmailActualToolStripMenuItem"
        Me.SendEmailActualToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.SendEmailActualToolStripMenuItem.Text = "Actual"
        '
        'ManageEmailToolStripDropDownButton
        '
        Me.ManageEmailToolStripDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResetEmailsToolStripMenuItem, Me.RefreshEmailStatusToolStripMenuItem})
        Me.ManageEmailToolStripDropDownButton.Image = CType(resources.GetObject("ManageEmailToolStripDropDownButton.Image"), System.Drawing.Image)
        Me.ManageEmailToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ManageEmailToolStripDropDownButton.Name = "ManageEmailToolStripDropDownButton"
        Me.ManageEmailToolStripDropDownButton.Size = New System.Drawing.Size(116, 22)
        Me.ManageEmailToolStripDropDownButton.Text = "&Manage Emails"
        '
        'ResetEmailsToolStripMenuItem
        '
        Me.ResetEmailsToolStripMenuItem.Name = "ResetEmailsToolStripMenuItem"
        Me.ResetEmailsToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ResetEmailsToolStripMenuItem.Text = "Reset All"
        '
        'RefreshEmailStatusToolStripMenuItem
        '
        Me.RefreshEmailStatusToolStripMenuItem.Name = "RefreshEmailStatusToolStripMenuItem"
        Me.RefreshEmailStatusToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.RefreshEmailStatusToolStripMenuItem.Text = "Refresh Status"
        '
        'RefreshEmailServiceToolStripButton
        '
        Me.RefreshEmailServiceToolStripButton.Image = Global.AccuPay.My.Resources.Resources._1431339112_Settings
        Me.RefreshEmailServiceToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.RefreshEmailServiceToolStripButton.Name = "RefreshEmailServiceToolStripButton"
        Me.RefreshEmailServiceToolStripButton.Size = New System.Drawing.Size(135, 22)
        Me.RefreshEmailServiceToolStripButton.Text = "Restart Email Service"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "EmployeeNo"
        Me.DataGridViewTextBoxColumn1.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee Number"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 101
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "FirstName"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn2.FillWeight = 150.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 152
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "MiddleName"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Format = "MM/dd/yyyy"
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn3.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Middle Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 102
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "LastName"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "MM/dd/yyyy"
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn4.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn4.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 101
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "EmployeeType"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle10.Format = "#,###,##0.00;(#,###,##0.00);"""""""""
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle10
        Me.DataGridViewTextBoxColumn5.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn5.HeaderText = "Employee Type"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 101
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "PositionName"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle11.Format = "#,###,##0.00;(#,###,##0.00);"""""""""
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle11
        Me.DataGridViewTextBoxColumn6.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn6.HeaderText = "Position"
        Me.DataGridViewTextBoxColumn6.MinimumWidth = 120
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 120
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "DivisionName"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle12
        Me.DataGridViewTextBoxColumn7.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn7.HeaderText = "Division"
        Me.DataGridViewTextBoxColumn7.MinimumWidth = 120
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Width = 120
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "EmailStatus"
        Me.DataGridViewTextBoxColumn8.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn8.HeaderText = "Email Status"
        Me.DataGridViewTextBoxColumn8.MinimumWidth = 120
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Width = 120
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "ErrorLogMessage"
        Me.DataGridViewTextBoxColumn9.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn9.HeaderText = "Last Error Log Message"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Width = 192
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "ErrorLogMessage"
        Me.DataGridViewTextBoxColumn10.HeaderText = "Last Error Log Message"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        Me.DataGridViewTextBoxColumn10.Width = 177
        '
        'SelectedCheckBoxColumn
        '
        Me.SelectedCheckBoxColumn.DataPropertyName = "IsSelected"
        Me.SelectedCheckBoxColumn.FillWeight = 13.21778!
        Me.SelectedCheckBoxColumn.HeaderText = ""
        Me.SelectedCheckBoxColumn.Name = "SelectedCheckBoxColumn"
        Me.SelectedCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "EmployeeNumber"
        Me.Column1.FillWeight = 36.74457!
        Me.Column1.HeaderText = "Employee Number"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "LastName"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle1
        Me.Column3.FillWeight = 36.74457!
        Me.Column3.HeaderText = "Last Name"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column6
        '
        Me.Column6.DataPropertyName = "FirstName"
        Me.Column6.FillWeight = 55.11684!
        Me.Column6.HeaderText = "First Name"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "MiddleName"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column2.FillWeight = 36.74457!
        Me.Column2.HeaderText = "Middle Name"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'EmailAddressColumn
        '
        Me.EmailAddressColumn.DataPropertyName = "EmailAddress"
        DataGridViewCellStyle3.NullValue = "(No email address.)"
        Me.EmailAddressColumn.DefaultCellStyle = DataGridViewCellStyle3
        Me.EmailAddressColumn.HeaderText = "Email Address"
        Me.EmailAddressColumn.Name = "EmailAddressColumn"
        '
        'Column4
        '
        Me.Column4.DataPropertyName = "EmployeeType"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column4.FillWeight = 36.74457!
        Me.Column4.HeaderText = "Employee Type"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column5
        '
        Me.Column5.DataPropertyName = "PositionName"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column5.FillWeight = 36.74457!
        Me.Column5.HeaderText = "Position"
        Me.Column5.MinimumWidth = 120
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column7
        '
        Me.Column7.DataPropertyName = "DivisionName"
        Me.Column7.FillWeight = 36.74457!
        Me.Column7.HeaderText = "Division"
        Me.Column7.MinimumWidth = 120
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        '
        'EmailStatusColumn
        '
        Me.EmailStatusColumn.DataPropertyName = "EmailStatus"
        Me.EmailStatusColumn.FillWeight = 36.74457!
        Me.EmailStatusColumn.HeaderText = "Email Status"
        Me.EmailStatusColumn.Name = "EmailStatusColumn"
        Me.EmailStatusColumn.ReadOnly = True
        '
        'PayslipTypeColumn
        '
        Me.PayslipTypeColumn.DataPropertyName = "PayslipType"
        Me.PayslipTypeColumn.HeaderText = "Payslip Type"
        Me.PayslipTypeColumn.Name = "PayslipTypeColumn"
        '
        'ResetEmailButtonColumn
        '
        Me.ResetEmailButtonColumn.FillWeight = 50.0!
        Me.ResetEmailButtonColumn.HeaderText = ""
        Me.ResetEmailButtonColumn.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.ResetEmailButtonColumn.LinkColor = System.Drawing.Color.Red
        Me.ResetEmailButtonColumn.Name = "ResetEmailButtonColumn"
        Me.ResetEmailButtonColumn.Text = "Reset Email"
        Me.ResetEmailButtonColumn.UseColumnTextForLinkValue = True
        Me.ResetEmailButtonColumn.VisitedLinkColor = System.Drawing.Color.Red
        '
        'ErrorLogMessageColumn
        '
        Me.ErrorLogMessageColumn.DataPropertyName = "ErrorLogMessage"
        Me.ErrorLogMessageColumn.HeaderText = "Last Error Log Message"
        Me.ErrorLogMessageColumn.Name = "ErrorLogMessageColumn"
        Me.ErrorLogMessageColumn.ReadOnly = True
        '
        'SelectPayslipEmployeesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1234, 463)
        Me.Controls.Add(Me.EmployeeGridView)
        Me.Controls.Add(Me.StatusLabel)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SelectPayslipEmployeesForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select Employees Form"
        Me.Panel1.ResumeLayout(False)
        Me.ActionPanel.ResumeLayout(False)
        CType(Me.EmployeeGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents CloseDialogButton As Button
    Friend WithEvents ActionPanel As Panel
    Friend WithEvents EmployeeGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents StatusLabel As Label
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
    Friend WithEvents UncheckAllButton As Button
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents RefreshEmailServiceToolStripButton As ToolStripButton
    Friend WithEvents SendEmailToolStripButton As ToolStripDropDownButton
    Friend WithEvents PreviewToolStripButton As ToolStripDropDownButton
    Friend WithEvents PreviewDeclaredToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PreviewActualToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SendEmailDeclaredToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SendEmailActualToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ManageEmailToolStripDropDownButton As ToolStripDropDownButton
    Friend WithEvents ResetEmailsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RefreshEmailStatusToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectedCheckBoxColumn As DataGridViewCheckBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents EmailAddressColumn As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents EmailStatusColumn As DataGridViewTextBoxColumn
    Friend WithEvents PayslipTypeColumn As DataGridViewTextBoxColumn
    Friend WithEvents ResetEmailButtonColumn As DataGridViewLinkColumn
    Friend WithEvents ErrorLogMessageColumn As DataGridViewTextBoxColumn
End Class
