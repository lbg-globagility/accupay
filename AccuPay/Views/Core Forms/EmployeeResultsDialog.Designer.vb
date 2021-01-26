<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmployeeResultsDialog
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
        Me.dgvSuccess = New System.Windows.Forms.DataGridView()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SuccessResultColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tbpSuccess = New System.Windows.Forms.TabPage()
        Me.tbpFailed = New System.Windows.Forms.TabPage()
        Me.dgvFailed = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FailedResultColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvSuccess, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.tbpSuccess.SuspendLayout()
        Me.tbpFailed.SuspendLayout()
        CType(Me.dgvFailed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvSuccess
        '
        Me.dgvSuccess.AllowUserToAddRows = False
        Me.dgvSuccess.AllowUserToDeleteRows = False
        Me.dgvSuccess.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvSuccess.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvSuccess.BackgroundColor = System.Drawing.Color.White
        Me.dgvSuccess.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvSuccess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSuccess.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column4, Me.Column1, Me.SuccessResultColumn, Me.Column3})
        Me.dgvSuccess.Location = New System.Drawing.Point(3, 3)
        Me.dgvSuccess.MultiSelect = False
        Me.dgvSuccess.Name = "dgvSuccess"
        Me.dgvSuccess.ReadOnly = True
        Me.dgvSuccess.RowHeadersVisible = False
        Me.dgvSuccess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSuccess.Size = New System.Drawing.Size(872, 304)
        Me.dgvSuccess.TabIndex = 0
        '
        'Column4
        '
        Me.Column4.DataPropertyName = "EmployeeNumber"
        Me.Column4.FillWeight = 15.0!
        Me.Column4.HeaderText = "Employee No"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "EmployeeFullName"
        Me.Column1.FillWeight = 25.0!
        Me.Column1.HeaderText = "Full Name"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'SuccessResultColumn
        '
        Me.SuccessResultColumn.FillWeight = 15.0!
        Me.SuccessResultColumn.HeaderText = "Result"
        Me.SuccessResultColumn.Name = "SuccessResultColumn"
        Me.SuccessResultColumn.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "Description"
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle1
        Me.Column3.FillWeight = 60.0!
        Me.Column3.HeaderText = "Description"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.FillWeight = 25.0!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 185
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.FillWeight = 15.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Result"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 111
        '
        'DataGridViewTextBoxColumn3
        '
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn3.FillWeight = 60.0!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 445
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(807, 390)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(83, 32)
        Me.btnOk.TabIndex = 5
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMessage.BackColor = System.Drawing.Color.White
        Me.lblMessage.Location = New System.Drawing.Point(8, 8)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(879, 40)
        Me.lblMessage.TabIndex = 6
        Me.lblMessage.Text = "<Message>"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.tbpSuccess)
        Me.TabControl1.Controls.Add(Me.tbpFailed)
        Me.TabControl1.Location = New System.Drawing.Point(8, 48)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(886, 336)
        Me.TabControl1.TabIndex = 7
        '
        'tbpSuccess
        '
        Me.tbpSuccess.Controls.Add(Me.dgvSuccess)
        Me.tbpSuccess.Location = New System.Drawing.Point(4, 22)
        Me.tbpSuccess.Name = "tbpSuccess"
        Me.tbpSuccess.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpSuccess.Size = New System.Drawing.Size(878, 310)
        Me.tbpSuccess.TabIndex = 0
        Me.tbpSuccess.Text = "Success"
        Me.tbpSuccess.UseVisualStyleBackColor = True
        '
        'tbpFailed
        '
        Me.tbpFailed.Controls.Add(Me.dgvFailed)
        Me.tbpFailed.Location = New System.Drawing.Point(4, 22)
        Me.tbpFailed.Name = "tbpFailed"
        Me.tbpFailed.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpFailed.Size = New System.Drawing.Size(878, 310)
        Me.tbpFailed.TabIndex = 1
        Me.tbpFailed.Text = "Failed"
        Me.tbpFailed.UseVisualStyleBackColor = True
        '
        'dgvFailed
        '
        Me.dgvFailed.AllowUserToAddRows = False
        Me.dgvFailed.AllowUserToDeleteRows = False
        Me.dgvFailed.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvFailed.BackgroundColor = System.Drawing.Color.White
        Me.dgvFailed.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvFailed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFailed.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.FailedResultColumn, Me.DataGridViewTextBoxColumn7})
        Me.dgvFailed.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvFailed.Location = New System.Drawing.Point(3, 3)
        Me.dgvFailed.MultiSelect = False
        Me.dgvFailed.Name = "dgvFailed"
        Me.dgvFailed.ReadOnly = True
        Me.dgvFailed.RowHeadersVisible = False
        Me.dgvFailed.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFailed.Size = New System.Drawing.Size(872, 304)
        Me.dgvFailed.TabIndex = 1
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "EmployeeNumber"
        Me.DataGridViewTextBoxColumn4.FillWeight = 15.0!
        Me.DataGridViewTextBoxColumn4.HeaderText = "Employee No"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "EmployeeFullName"
        Me.DataGridViewTextBoxColumn5.FillWeight = 25.0!
        Me.DataGridViewTextBoxColumn5.HeaderText = "Full Name"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        '
        'FailedResultColumn
        '
        Me.FailedResultColumn.FillWeight = 15.0!
        Me.FailedResultColumn.HeaderText = "Result"
        Me.FailedResultColumn.Name = "FailedResultColumn"
        Me.FailedResultColumn.ReadOnly = True
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "Description"
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewTextBoxColumn7.FillWeight = 60.0!
        Me.DataGridViewTextBoxColumn7.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        '
        'EmployeeResultsDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(902, 432)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnOk)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "EmployeeResultsDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "<Title>"
        CType(Me.dgvSuccess, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.tbpSuccess.ResumeLayout(False)
        Me.tbpFailed.ResumeLayout(False)
        CType(Me.dgvFailed, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvSuccess As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents btnOk As Button
    Friend WithEvents lblMessage As Label
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tbpSuccess As TabPage
    Friend WithEvents tbpFailed As TabPage
    Friend WithEvents dgvFailed As DataGridView
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents SuccessResultColumn As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents FailedResultColumn As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
End Class
