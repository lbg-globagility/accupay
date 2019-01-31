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
        Me.dgvLogs = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.ParsedTabControl = New System.Windows.Forms.TabPage()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.ErrorsTabControl = New System.Windows.Forms.TabPage()
        Me.dgvErrors = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvLogs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.ParsedTabControl.SuspendLayout()
        Me.ErrorsTabControl.SuspendLayout()
        CType(Me.dgvErrors, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvLogs
        '
        Me.dgvLogs.AllowUserToAddRows = False
        Me.dgvLogs.AllowUserToDeleteRows = False
        Me.dgvLogs.AllowUserToOrderColumns = True
        Me.dgvLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvLogs.BackgroundColor = System.Drawing.Color.White
        Me.dgvLogs.ColumnHeadersHeight = 34
        Me.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvLogs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvLogs.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvLogs.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgvLogs.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvLogs.Location = New System.Drawing.Point(3, 24)
        Me.dgvLogs.MultiSelect = False
        Me.dgvLogs.Name = "dgvLogs"
        Me.dgvLogs.ReadOnly = True
        Me.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLogs.Size = New System.Drawing.Size(571, 316)
        Me.dgvLogs.TabIndex = 12
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "EmployeeNo"
        Me.Column1.FillWeight = 20.0!
        Me.Column1.HeaderText = "Employee ID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "DateTime"
        Me.Column2.FillWeight = 80.0!
        Me.Column2.HeaderText = "Timestamp"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnOK)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 378)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(585, 72)
        Me.Panel1.TabIndex = 13
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(417, 25)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 35)
        Me.btnOK.TabIndex = 10
        Me.btnOK.Text = "Save"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(498, 25)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 35)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.ParsedTabControl)
        Me.TabControl1.Controls.Add(Me.ErrorsTabControl)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(585, 378)
        Me.TabControl1.TabIndex = 14
        '
        'ParsedTabControl
        '
        Me.ParsedTabControl.Controls.Add(Me.dgvLogs)
        Me.ParsedTabControl.Controls.Add(Me.lblStatus)
        Me.ParsedTabControl.Location = New System.Drawing.Point(4, 22)
        Me.ParsedTabControl.Name = "ParsedTabControl"
        Me.ParsedTabControl.Padding = New System.Windows.Forms.Padding(3)
        Me.ParsedTabControl.Size = New System.Drawing.Size(577, 352)
        Me.ParsedTabControl.TabIndex = 0
        Me.ParsedTabControl.Text = "Ok"
        Me.ParsedTabControl.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Red
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblStatus.ForeColor = System.Drawing.Color.White
        Me.lblStatus.Location = New System.Drawing.Point(3, 3)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Padding = New System.Windows.Forms.Padding(4)
        Me.lblStatus.Size = New System.Drawing.Size(571, 21)
        Me.lblStatus.TabIndex = 13
        Me.lblStatus.Text = "There is 1 error. Failed error logs will not be saved."
        '
        'ErrorsTabControl
        '
        Me.ErrorsTabControl.Controls.Add(Me.dgvErrors)
        Me.ErrorsTabControl.Location = New System.Drawing.Point(4, 22)
        Me.ErrorsTabControl.Name = "ErrorsTabControl"
        Me.ErrorsTabControl.Padding = New System.Windows.Forms.Padding(3)
        Me.ErrorsTabControl.Size = New System.Drawing.Size(577, 352)
        Me.ErrorsTabControl.TabIndex = 1
        Me.ErrorsTabControl.Text = "Errors"
        Me.ErrorsTabControl.UseVisualStyleBackColor = True
        '
        'dgvErrors
        '
        Me.dgvErrors.AllowUserToAddRows = False
        Me.dgvErrors.AllowUserToDeleteRows = False
        Me.dgvErrors.AllowUserToOrderColumns = True
        Me.dgvErrors.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvErrors.BackgroundColor = System.Drawing.Color.White
        Me.dgvErrors.ColumnHeadersHeight = 34
        Me.dgvErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvErrors.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.Column3})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvErrors.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvErrors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvErrors.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvErrors.Location = New System.Drawing.Point(3, 3)
        Me.dgvErrors.MultiSelect = False
        Me.dgvErrors.Name = "dgvErrors"
        Me.dgvErrors.ReadOnly = True
        Me.dgvErrors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvErrors.Size = New System.Drawing.Size(571, 346)
        Me.dgvErrors.TabIndex = 13
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "LineNumber"
        Me.DataGridViewTextBoxColumn3.FillWeight = 20.0!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Line Number"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "Content"
        Me.DataGridViewTextBoxColumn4.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn4.HeaderText = "Line Content"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "Reason"
        Me.Column3.FillWeight = 40.0!
        Me.Column3.HeaderText = "Reason"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
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
        Me.DataGridViewTextBoxColumn2.FillWeight = 80.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Timestamp"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 422
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "Reason"
        Me.DataGridViewTextBoxColumn5.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn5.HeaderText = "Reason"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.Width = 211
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "Content"
        Me.DataGridViewTextBoxColumn6.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn6.HeaderText = "Line Content"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.Width = 211
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "Reason"
        Me.DataGridViewTextBoxColumn7.FillWeight = 40.0!
        Me.DataGridViewTextBoxColumn7.HeaderText = "Reason"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.Width = 211
        '
        'TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(585, 450)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import Preview"
        CType(Me.dgvLogs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.ParsedTabControl.ResumeLayout(False)
        Me.ErrorsTabControl.ResumeLayout(False)
        CType(Me.dgvErrors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvLogs As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents ParsedTabControl As TabPage
    Friend WithEvents ErrorsTabControl As TabPage
    Friend WithEvents dgvErrors As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents lblStatus As Label
End Class
