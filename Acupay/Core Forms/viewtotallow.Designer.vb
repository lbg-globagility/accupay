<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class viewtotallow
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvempallowance = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.eall_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Start = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvempallowance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvempallowance
        '
        Me.dgvempallowance.AllowUserToAddRows = False
        Me.dgvempallowance.AllowUserToDeleteRows = False
        Me.dgvempallowance.AllowUserToOrderColumns = True
        Me.dgvempallowance.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempallowance.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvempallowance.ColumnHeadersHeight = 34
        Me.dgvempallowance.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eall_Type, Me.eall_Start, Me.eall_Amount})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempallowance.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgvempallowance.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempallowance.Location = New System.Drawing.Point(12, 12)
        Me.dgvempallowance.MultiSelect = False
        Me.dgvempallowance.Name = "dgvempallowance"
        Me.dgvempallowance.ReadOnly = True
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempallowance.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgvempallowance.Size = New System.Drawing.Size(784, 450)
        Me.dgvempallowance.TabIndex = 0
        '
        'eall_Type
        '
        Me.eall_Type.HeaderText = "Name"
        Me.eall_Type.Name = "eall_Type"
        Me.eall_Type.ReadOnly = True
        Me.eall_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eall_Type.Width = 247
        '
        'eall_Start
        '
        DataGridViewCellStyle2.Format = "d"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.eall_Start.DefaultCellStyle = DataGridViewCellStyle2
        Me.eall_Start.HeaderText = "Date"
        Me.eall_Start.Name = "eall_Start"
        Me.eall_Start.ReadOnly = True
        '
        'eall_Amount
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "#,###,##0.00;(#,###,##0.00);"""""""""
        DataGridViewCellStyle3.NullValue = Nothing
        Me.eall_Amount.DefaultCellStyle = DataGridViewCellStyle3
        Me.eall_Amount.HeaderText = "Amount"
        Me.eall_Amount.Name = "eall_Amount"
        Me.eall_Amount.ReadOnly = True
        Me.eall_Amount.Width = 247
        '
        'viewtotallow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(194, Byte), Integer), CType(CType(183, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(808, 474)
        Me.Controls.Add(Me.dgvempallowance)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "viewtotallow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Allowance"
        CType(Me.dgvempallowance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvempallowance As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents eall_Type As DataGridViewTextBoxColumn
    Friend WithEvents eall_Start As DataGridViewTextBoxColumn
    Friend WithEvents eall_Amount As DataGridViewTextBoxColumn
End Class
