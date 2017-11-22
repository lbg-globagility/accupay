<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JobPointsTab
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.dgvempallowance = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.txtEmpIDAllow = New System.Windows.Forms.TextBox()
        Me.txtFNameAllow = New System.Windows.Forms.TextBox()
        Me.pbEmpPicAllow = New System.Windows.Forms.PictureBox()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New Acupay.DataGridViewDateColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvempallowance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbEmpPicAllow, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvempallowance
        '
        Me.dgvempallowance.AllowUserToDeleteRows = False
        Me.dgvempallowance.AllowUserToOrderColumns = True
        Me.dgvempallowance.AllowUserToResizeColumns = False
        Me.dgvempallowance.AllowUserToResizeRows = False
        Me.dgvempallowance.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvempallowance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
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
        Me.dgvempallowance.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column4, Me.Column2, Me.Column3})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempallowance.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvempallowance.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempallowance.Location = New System.Drawing.Point(8, 104)
        Me.dgvempallowance.MultiSelect = False
        Me.dgvempallowance.Name = "dgvempallowance"
        Me.dgvempallowance.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvempallowance.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvempallowance.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvempallowance.Size = New System.Drawing.Size(848, 449)
        Me.dgvempallowance.TabIndex = 367
        '
        'txtEmpIDAllow
        '
        Me.txtEmpIDAllow.BackColor = System.Drawing.Color.White
        Me.txtEmpIDAllow.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpIDAllow.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmpIDAllow.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpIDAllow.Location = New System.Drawing.Point(104, 35)
        Me.txtEmpIDAllow.MaxLength = 50
        Me.txtEmpIDAllow.Name = "txtEmpIDAllow"
        Me.txtEmpIDAllow.ReadOnly = True
        Me.txtEmpIDAllow.Size = New System.Drawing.Size(520, 22)
        Me.txtEmpIDAllow.TabIndex = 369
        '
        'txtFNameAllow
        '
        Me.txtFNameAllow.BackColor = System.Drawing.Color.White
        Me.txtFNameAllow.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFNameAllow.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFNameAllow.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFNameAllow.Location = New System.Drawing.Point(104, 8)
        Me.txtFNameAllow.MaxLength = 250
        Me.txtFNameAllow.Name = "txtFNameAllow"
        Me.txtFNameAllow.ReadOnly = True
        Me.txtFNameAllow.Size = New System.Drawing.Size(520, 28)
        Me.txtFNameAllow.TabIndex = 370
        '
        'pbEmpPicAllow
        '
        Me.pbEmpPicAllow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPicAllow.Location = New System.Drawing.Point(8, 8)
        Me.pbEmpPicAllow.Name = "pbEmpPicAllow"
        Me.pbEmpPicAllow.Size = New System.Drawing.Size(88, 88)
        Me.pbEmpPicAllow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPicAllow.TabIndex = 368
        Me.pbEmpPicAllow.TabStop = False
        '
        'Column1
        '
        Me.Column1.HeaderText = "RowID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Visible = False
        '
        'Column4
        '
        Me.Column4.FillWeight = 20.0!
        Me.Column4.HeaderText = "Date"
        Me.Column4.MaxInputLength = 11
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'Column2
        '
        Me.Column2.FillWeight = 20.0!
        Me.Column2.HeaderText = "Points"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.FillWeight = 60.0!
        Me.Column3.HeaderText = "Comments"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'JobPointsTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtEmpIDAllow)
        Me.Controls.Add(Me.txtFNameAllow)
        Me.Controls.Add(Me.pbEmpPicAllow)
        Me.Controls.Add(Me.dgvempallowance)
        Me.Name = "JobPointsTab"
        Me.Size = New System.Drawing.Size(864, 560)
        CType(Me.dgvempallowance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbEmpPicAllow, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvempallowance As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents txtEmpIDAllow As TextBox
    Friend WithEvents txtFNameAllow As TextBox
    Friend WithEvents pbEmpPicAllow As PictureBox
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewDateColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
End Class
