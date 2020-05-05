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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectPayslipEmployeesForm))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RefreshEmailServiceButton = New System.Windows.Forms.Button()
        Me.RefreshEmailStatusButton = New System.Windows.Forms.Button()
        Me.PreviewButton = New System.Windows.Forms.Button()
        Me.PayslipTypePanel = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PayslipTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.CancelDialogButton = New System.Windows.Forms.Button()
        Me.SendEmailsButton = New System.Windows.Forms.Button()
        Me.EmployeesDataGrid = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.SelectedCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmailAddressColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmailStatusColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ErrorLogMessageColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblStatus = New System.Windows.Forms.Label()
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
        Me.Panel1.SuspendLayout()
        Me.PayslipTypePanel.SuspendLayout()
        CType(Me.EmployeesDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RefreshEmailServiceButton)
        Me.Panel1.Controls.Add(Me.RefreshEmailStatusButton)
        Me.Panel1.Controls.Add(Me.PreviewButton)
        Me.Panel1.Controls.Add(Me.PayslipTypePanel)
        Me.Panel1.Controls.Add(Me.CancelDialogButton)
        Me.Panel1.Controls.Add(Me.SendEmailsButton)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 413)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1084, 50)
        Me.Panel1.TabIndex = 0
        '
        'RefreshEmailServiceButton
        '
        Me.RefreshEmailServiceButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RefreshEmailServiceButton.BackColor = System.Drawing.Color.Red
        Me.RefreshEmailServiceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RefreshEmailServiceButton.ForeColor = System.Drawing.Color.White
        Me.RefreshEmailServiceButton.Location = New System.Drawing.Point(481, 15)
        Me.RefreshEmailServiceButton.Name = "RefreshEmailServiceButton"
        Me.RefreshEmailServiceButton.Size = New System.Drawing.Size(123, 23)
        Me.RefreshEmailServiceButton.TabIndex = 9
        Me.RefreshEmailServiceButton.Text = "Restart Email Service"
        Me.RefreshEmailServiceButton.UseVisualStyleBackColor = False
        '
        'RefreshEmailStatusButton
        '
        Me.RefreshEmailStatusButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RefreshEmailStatusButton.Location = New System.Drawing.Point(618, 15)
        Me.RefreshEmailStatusButton.Name = "RefreshEmailStatusButton"
        Me.RefreshEmailStatusButton.Size = New System.Drawing.Size(123, 23)
        Me.RefreshEmailStatusButton.TabIndex = 8
        Me.RefreshEmailStatusButton.Text = "&Refresh Email Status"
        Me.RefreshEmailStatusButton.UseVisualStyleBackColor = True
        '
        'PreviewButton
        '
        Me.PreviewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PreviewButton.Enabled = False
        Me.PreviewButton.Location = New System.Drawing.Point(865, 15)
        Me.PreviewButton.Name = "PreviewButton"
        Me.PreviewButton.Size = New System.Drawing.Size(96, 23)
        Me.PreviewButton.TabIndex = 7
        Me.PreviewButton.Text = "&Preview"
        Me.PreviewButton.UseVisualStyleBackColor = True
        '
        'PayslipTypePanel
        '
        Me.PayslipTypePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PayslipTypePanel.Controls.Add(Me.Label1)
        Me.PayslipTypePanel.Controls.Add(Me.PayslipTypeComboBox)
        Me.PayslipTypePanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.PayslipTypePanel.Location = New System.Drawing.Point(0, 0)
        Me.PayslipTypePanel.Name = "PayslipTypePanel"
        Me.PayslipTypePanel.Size = New System.Drawing.Size(456, 50)
        Me.PayslipTypePanel.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(242, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Payslip Type:"
        '
        'PayslipTypeComboBox
        '
        Me.PayslipTypeComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PayslipTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PayslipTypeComboBox.FormattingEnabled = True
        Me.PayslipTypeComboBox.ItemHeight = 13
        Me.PayslipTypeComboBox.Location = New System.Drawing.Point(315, 15)
        Me.PayslipTypeComboBox.Name = "PayslipTypeComboBox"
        Me.PayslipTypeComboBox.Size = New System.Drawing.Size(121, 21)
        Me.PayslipTypeComboBox.TabIndex = 5
        '
        'CancelFormButton
        '
        Me.CancelDialogButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelDialogButton.Location = New System.Drawing.Point(970, 15)
        Me.CancelDialogButton.Name = "CancelFormButton"
        Me.CancelDialogButton.Size = New System.Drawing.Size(96, 23)
        Me.CancelDialogButton.TabIndex = 3
        Me.CancelDialogButton.Text = "&Cancel"
        Me.CancelDialogButton.UseVisualStyleBackColor = True
        '
        'SendEmailsButton
        '
        Me.SendEmailsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SendEmailsButton.Enabled = False
        Me.SendEmailsButton.Location = New System.Drawing.Point(750, 15)
        Me.SendEmailsButton.Name = "SendEmailsButton"
        Me.SendEmailsButton.Size = New System.Drawing.Size(106, 23)
        Me.SendEmailsButton.TabIndex = 4
        Me.SendEmailsButton.Text = "&Send Emails"
        Me.SendEmailsButton.UseVisualStyleBackColor = True
        '
        'EmployeesDataGrid
        '
        Me.EmployeesDataGrid.AllowUserToAddRows = False
        Me.EmployeesDataGrid.AllowUserToDeleteRows = False
        Me.EmployeesDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.EmployeesDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.EmployeesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.EmployeesDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.SelectedCheckBoxColumn, Me.Column1, Me.Column6, Me.Column2, Me.Column3, Me.EmailAddressColumn, Me.Column4, Me.Column5, Me.Column7, Me.EmailStatusColumn, Me.ErrorLogMessageColumn})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.EmployeesDataGrid.DefaultCellStyle = DataGridViewCellStyle6
        Me.EmployeesDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EmployeesDataGrid.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.EmployeesDataGrid.Location = New System.Drawing.Point(0, 21)
        Me.EmployeesDataGrid.Name = "EmployeesDataGrid"
        Me.EmployeesDataGrid.Size = New System.Drawing.Size(1084, 392)
        Me.EmployeesDataGrid.TabIndex = 1
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
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle1
        Me.Column2.FillWeight = 36.74457!
        Me.Column2.HeaderText = "Middle Name"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "LastName"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column3.FillWeight = 36.74457!
        Me.Column3.HeaderText = "Last Name"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
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
        'ErrorLogMessageColumn
        '
        Me.ErrorLogMessageColumn.DataPropertyName = "ErrorLogMessage"
        Me.ErrorLogMessageColumn.HeaderText = "Last Error Log Message"
        Me.ErrorLogMessageColumn.Name = "ErrorLogMessageColumn"
        Me.ErrorLogMessageColumn.ReadOnly = True
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Black
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblStatus.ForeColor = System.Drawing.Color.White
        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Padding = New System.Windows.Forms.Padding(4)
        Me.lblStatus.Size = New System.Drawing.Size(1084, 21)
        Me.lblStatus.TabIndex = 14
        Me.lblStatus.Text = "Tick the checkbox of the employee that you want to include."
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
        Me.DataGridViewTextBoxColumn2.FillWeight = 150.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 152
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "MiddleName"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "MM/dd/yyyy"
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn3.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Middle Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 102
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "LastName"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Format = "MM/dd/yyyy"
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn4.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn4.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 101
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "EmployeeType"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "#,###,##0.00;(#,###,##0.00);"""""""""
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn5.FillWeight = 36.74457!
        Me.DataGridViewTextBoxColumn5.HeaderText = "Employee Type"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 101
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "PositionName"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle10.Format = "#,###,##0.00;(#,###,##0.00);"""""""""
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle10
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
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle11
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
        'SelectPayslipEmployeesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1084, 463)
        Me.Controls.Add(Me.EmployeesDataGrid)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SelectPayslipEmployeesForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select Employees Form"
        Me.Panel1.ResumeLayout(False)
        Me.PayslipTypePanel.ResumeLayout(False)
        Me.PayslipTypePanel.PerformLayout()
        CType(Me.EmployeesDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents CancelDialogButton As Button
    Friend WithEvents SendEmailsButton As Button
    Friend WithEvents PayslipTypePanel As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents PayslipTypeComboBox As ComboBox
    Friend WithEvents PreviewButton As Button
    Friend WithEvents EmployeesDataGrid As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents lblStatus As Label
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents RefreshEmailStatusButton As Button
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents SelectedCheckBoxColumn As DataGridViewCheckBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents EmailAddressColumn As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents EmailStatusColumn As DataGridViewTextBoxColumn
    Friend WithEvents ErrorLogMessageColumn As DataGridViewTextBoxColumn
    Friend WithEvents RefreshEmailServiceButton As Button
End Class
