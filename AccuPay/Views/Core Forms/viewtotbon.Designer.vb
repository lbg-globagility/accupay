<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class viewtotbon
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
        Me.dgvempbon = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.bon_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_Frequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_Start = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_End = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bonus_taxable = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_ProdID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvempbon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvempbon
        '
        Me.dgvempbon.AllowUserToAddRows = False
        Me.dgvempbon.AllowUserToDeleteRows = False
        Me.dgvempbon.AllowUserToOrderColumns = True
        Me.dgvempbon.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempbon.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvempbon.ColumnHeadersHeight = 34
        Me.dgvempbon.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.bon_RowID, Me.bon_Type, Me.bon_Amount, Me.bon_Frequency, Me.bon_Start, Me.bon_End, Me.bonus_taxable, Me.bon_ProdID})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempbon.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvempbon.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempbon.Location = New System.Drawing.Point(12, 12)
        Me.dgvempbon.MultiSelect = False
        Me.dgvempbon.Name = "dgvempbon"
        Me.dgvempbon.ReadOnly = True
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempbon.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvempbon.Size = New System.Drawing.Size(783, 450)
        Me.dgvempbon.TabIndex = 0
        '
        'bon_RowID
        '
        Me.bon_RowID.HeaderText = "RowID"
        Me.bon_RowID.Name = "bon_RowID"
        Me.bon_RowID.ReadOnly = True
        Me.bon_RowID.Visible = False
        Me.bon_RowID.Width = 50
        '
        'bon_Type
        '
        Me.bon_Type.HeaderText = "Type"
        Me.bon_Type.Name = "bon_Type"
        Me.bon_Type.ReadOnly = True
        Me.bon_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.bon_Type.Width = 247
        '
        'bon_Amount
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.bon_Amount.DefaultCellStyle = DataGridViewCellStyle2
        Me.bon_Amount.HeaderText = "Amount"
        Me.bon_Amount.Name = "bon_Amount"
        Me.bon_Amount.ReadOnly = True
        Me.bon_Amount.Width = 246
        '
        'bon_Frequency
        '
        Me.bon_Frequency.HeaderText = "Frequency"
        Me.bon_Frequency.Name = "bon_Frequency"
        Me.bon_Frequency.ReadOnly = True
        Me.bon_Frequency.Visible = False
        Me.bon_Frequency.Width = 180
        '
        'bon_Start
        '
        Me.bon_Start.HeaderText = "Effective start date"
        Me.bon_Start.Name = "bon_Start"
        Me.bon_Start.ReadOnly = True
        Me.bon_Start.Visible = False
        '
        'bon_End
        '
        Me.bon_End.HeaderText = "Effective end date"
        Me.bon_End.Name = "bon_End"
        Me.bon_End.ReadOnly = True
        Me.bon_End.Visible = False
        '
        'bonus_taxable
        '
        Me.bonus_taxable.HeaderText = "Taxable"
        Me.bonus_taxable.Name = "bonus_taxable"
        Me.bonus_taxable.ReadOnly = True
        Me.bonus_taxable.Width = 247
        '
        'bon_ProdID
        '
        Me.bon_ProdID.HeaderText = "ProductID"
        Me.bon_ProdID.Name = "bon_ProdID"
        Me.bon_ProdID.ReadOnly = True
        Me.bon_ProdID.Visible = False
        '
        'viewtotbon
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(219, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(808, 474)
        Me.Controls.Add(Me.dgvempbon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "viewtotbon"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Bonus"
        CType(Me.dgvempbon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvempbon As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents bon_RowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bon_Type As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bon_Amount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bon_Frequency As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bon_Start As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bon_End As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bonus_taxable As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bon_ProdID As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
