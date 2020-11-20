<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BonusBreakdownDialog
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
        Me.BonusGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.BonusGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'BonusGridView
        '
        Me.BonusGridView.AllowUserToAddRows = False
        Me.BonusGridView.AllowUserToDeleteRows = False
        Me.BonusGridView.AllowUserToOrderColumns = True
        Me.BonusGridView.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.BonusGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.BonusGridView.ColumnHeadersHeight = 34
        Me.BonusGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column3, Me.Column2})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.BonusGridView.DefaultCellStyle = DataGridViewCellStyle3
        Me.BonusGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BonusGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.BonusGridView.Location = New System.Drawing.Point(0, 0)
        Me.BonusGridView.MultiSelect = False
        Me.BonusGridView.Name = "BonusGridView"
        Me.BonusGridView.ReadOnly = True
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.BonusGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.BonusGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.BonusGridView.Size = New System.Drawing.Size(484, 423)
        Me.BonusGridView.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.CloseButton)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 423)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(484, 51)
        Me.Panel2.TabIndex = 4
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(376, 9)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(96, 32)
        Me.CloseButton.TabIndex = 3
        Me.CloseButton.Text = "&Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "BonusType"
        Me.Column1.HeaderText = "Bonus Type"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 200
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "AllowanceFrequency"
        Me.Column3.HeaderText = "Frequency"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "Amount"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column2.HeaderText = "Amount"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'BonusBreakdownDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(219, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(484, 474)
        Me.Controls.Add(Me.BonusGridView)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BonusBreakdownDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Bonus Breakdown"
        CType(Me.BonusGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BonusGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Panel2 As Panel
    Friend WithEvents CloseButton As Button
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
End Class
