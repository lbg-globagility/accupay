<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AllowanceBreakdownDialog
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.AllowanceGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.eall_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Start = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AllowanceFrequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.AllowanceGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'AllowanceGridView
        '
        Me.AllowanceGridView.AllowUserToAddRows = False
        Me.AllowanceGridView.AllowUserToDeleteRows = False
        Me.AllowanceGridView.AllowUserToResizeColumns = False
        Me.AllowanceGridView.AllowUserToResizeRows = False
        Me.AllowanceGridView.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AllowanceGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.AllowanceGridView.ColumnHeadersHeight = 34
        Me.AllowanceGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eall_Type, Me.eall_Start, Me.eall_Amount, Me.AllowanceFrequency})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.AllowanceGridView.DefaultCellStyle = DataGridViewCellStyle5
        Me.AllowanceGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AllowanceGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.AllowanceGridView.Location = New System.Drawing.Point(0, 0)
        Me.AllowanceGridView.MultiSelect = False
        Me.AllowanceGridView.Name = "AllowanceGridView"
        Me.AllowanceGridView.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AllowanceGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.AllowanceGridView.Size = New System.Drawing.Size(784, 373)
        Me.AllowanceGridView.TabIndex = 0
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn2.Width = 182
        '
        'DataGridViewTextBoxColumn3
        '
        DataGridViewCellStyle7.Format = "d"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn3.HeaderText = "Date"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 180
        '
        'DataGridViewTextBoxColumn4
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Format = "#,###,##0.00;(#,###,##0.00);"""""""""
        DataGridViewCellStyle8.NullValue = Nothing
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn4.HeaderText = "Amount"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Visible = False
        Me.DataGridViewTextBoxColumn4.Width = 181
        '
        'DataGridViewTextBoxColumn5
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "#,###,##0.00;(#,###,##0.00);"""""""""
        DataGridViewCellStyle9.NullValue = Nothing
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn5.HeaderText = "Allowance Frequency"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 181
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Allowance Frequency"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.Width = 181
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.CloseButton)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 373)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(784, 51)
        Me.Panel2.TabIndex = 2
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(676, 9)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(96, 32)
        Me.CloseButton.TabIndex = 3
        Me.CloseButton.Text = "&Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'eall_Type
        '
        Me.eall_Type.DataPropertyName = "AllowanceName"
        Me.eall_Type.HeaderText = "Name"
        Me.eall_Type.Name = "eall_Type"
        Me.eall_Type.ReadOnly = True
        Me.eall_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eall_Type.Width = 182
        '
        'eall_Start
        '
        Me.eall_Start.DataPropertyName = "TransactionDate"
        DataGridViewCellStyle2.Format = "d"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.eall_Start.DefaultCellStyle = DataGridViewCellStyle2
        Me.eall_Start.HeaderText = ""
        Me.eall_Start.Name = "eall_Start"
        Me.eall_Start.ReadOnly = True
        Me.eall_Start.Width = 180
        '
        'eall_Amount
        '
        Me.eall_Amount.DataPropertyName = "Amount"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "#,###,##0.0000;(#,###,##0.0000);-"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.eall_Amount.DefaultCellStyle = DataGridViewCellStyle3
        Me.eall_Amount.HeaderText = ""
        Me.eall_Amount.Name = "eall_Amount"
        Me.eall_Amount.ReadOnly = True
        Me.eall_Amount.Width = 181
        '
        'AllowanceFrequency
        '
        Me.AllowanceFrequency.DataPropertyName = "RunningTotal"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "#,###,##0.0000;(#,###,##0.0000);0.0000"
        Me.AllowanceFrequency.DefaultCellStyle = DataGridViewCellStyle4
        Me.AllowanceFrequency.HeaderText = ""
        Me.AllowanceFrequency.Name = "AllowanceFrequency"
        Me.AllowanceFrequency.ReadOnly = True
        Me.AllowanceFrequency.Width = 181
        '
        'AllowanceBreakdownDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(194, Byte), Integer), CType(CType(183, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(784, 424)
        Me.Controls.Add(Me.AllowanceGridView)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AllowanceBreakdownDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Allowance Breakdown"
        CType(Me.AllowanceGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AllowanceGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents Panel2 As Panel
    Friend WithEvents CloseButton As Button
    Friend WithEvents eall_Type As DataGridViewTextBoxColumn
    Friend WithEvents eall_Start As DataGridViewTextBoxColumn
    Friend WithEvents eall_Amount As DataGridViewTextBoxColumn
    Friend WithEvents AllowanceFrequency As DataGridViewTextBoxColumn
End Class
