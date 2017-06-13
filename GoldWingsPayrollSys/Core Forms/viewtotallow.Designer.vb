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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvempallowance = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.eall_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Frequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_Start = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_End = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.allow_taxable = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eall_ProdID = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.dgvempallowance.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eall_RowID, Me.eall_Type, Me.eall_Amount, Me.eall_Frequency, Me.eall_Start, Me.eall_End, Me.allow_taxable, Me.eall_ProdID})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempallowance.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvempallowance.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempallowance.Location = New System.Drawing.Point(12, 12)
        Me.dgvempallowance.MultiSelect = False
        Me.dgvempallowance.Name = "dgvempallowance"
        Me.dgvempallowance.ReadOnly = True
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempallowance.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvempallowance.Size = New System.Drawing.Size(784, 450)
        Me.dgvempallowance.TabIndex = 0
        '
        'eall_RowID
        '
        Me.eall_RowID.HeaderText = "RowID"
        Me.eall_RowID.Name = "eall_RowID"
        Me.eall_RowID.ReadOnly = True
        Me.eall_RowID.Visible = False
        Me.eall_RowID.Width = 50
        '
        'eall_Type
        '
        Me.eall_Type.HeaderText = "Type"
        Me.eall_Type.Name = "eall_Type"
        Me.eall_Type.ReadOnly = True
        Me.eall_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eall_Type.Width = 247
        '
        'eall_Amount
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.eall_Amount.DefaultCellStyle = DataGridViewCellStyle2
        Me.eall_Amount.HeaderText = "Amount"
        Me.eall_Amount.Name = "eall_Amount"
        Me.eall_Amount.ReadOnly = True
        Me.eall_Amount.Width = 247
        '
        'eall_Frequency
        '
        Me.eall_Frequency.HeaderText = "Frequency"
        Me.eall_Frequency.Name = "eall_Frequency"
        Me.eall_Frequency.ReadOnly = True
        Me.eall_Frequency.Visible = False
        Me.eall_Frequency.Width = 180
        '
        'eall_Start
        '
        Me.eall_Start.HeaderText = "Effective start date"
        Me.eall_Start.Name = "eall_Start"
        Me.eall_Start.ReadOnly = True
        Me.eall_Start.Visible = False
        '
        'eall_End
        '
        Me.eall_End.HeaderText = "Effective end date"
        Me.eall_End.Name = "eall_End"
        Me.eall_End.ReadOnly = True
        Me.eall_End.Visible = False
        '
        'allow_taxable
        '
        Me.allow_taxable.HeaderText = "Taxable"
        Me.allow_taxable.Name = "allow_taxable"
        Me.allow_taxable.ReadOnly = True
        Me.allow_taxable.Width = 247
        '
        'eall_ProdID
        '
        Me.eall_ProdID.HeaderText = "ProductID"
        Me.eall_ProdID.Name = "eall_ProdID"
        Me.eall_ProdID.ReadOnly = True
        Me.eall_ProdID.Visible = False
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
    Friend WithEvents eall_RowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eall_Type As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eall_Amount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eall_Frequency As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eall_Start As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eall_End As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents allow_taxable As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eall_ProdID As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
