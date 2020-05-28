<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DayTypesDialog
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.DetailsGroupBox = New System.Windows.Forms.GroupBox()
        Me.DayTypeControl = New AccuPay.DayTypeControl()
        Me.DayTypesGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.cemp_EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DetailsGroupBox.SuspendLayout()
        CType(Me.DayTypesGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(8, 336)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(96, 26)
        Me.SaveButton.TabIndex = 146
        Me.SaveButton.Text = "&Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'DetailsGroupBox
        '
        Me.DetailsGroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DetailsGroupBox.Controls.Add(Me.SaveButton)
        Me.DetailsGroupBox.Controls.Add(Me.DayTypeControl)
        Me.DetailsGroupBox.Location = New System.Drawing.Point(368, 8)
        Me.DetailsGroupBox.Name = "DetailsGroupBox"
        Me.DetailsGroupBox.Size = New System.Drawing.Size(376, 375)
        Me.DetailsGroupBox.TabIndex = 147
        Me.DetailsGroupBox.TabStop = False
        Me.DetailsGroupBox.Text = "Day Type"
        '
        'DayTypeControl
        '
        Me.DayTypeControl.DayType = Nothing
        Me.DayTypeControl.Dock = System.Windows.Forms.DockStyle.Top
        Me.DayTypeControl.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DayTypeControl.Location = New System.Drawing.Point(3, 18)
        Me.DayTypeControl.Name = "DayTypeControl"
        Me.DayTypeControl.Size = New System.Drawing.Size(370, 318)
        Me.DayTypeControl.TabIndex = 382
        '
        'DayTypesGridView
        '
        Me.DayTypesGridView.AllowUserToAddRows = False
        Me.DayTypesGridView.AllowUserToDeleteRows = False
        Me.DayTypesGridView.AllowUserToOrderColumns = True
        Me.DayTypesGridView.AllowUserToResizeRows = False
        Me.DayTypesGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DayTypesGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DayTypesGridView.BackgroundColor = System.Drawing.Color.White
        Me.DayTypesGridView.ColumnHeadersHeight = 34
        Me.DayTypesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DayTypesGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cemp_EmployeeID})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DayTypesGridView.DefaultCellStyle = DataGridViewCellStyle3
        Me.DayTypesGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.DayTypesGridView.Location = New System.Drawing.Point(8, 8)
        Me.DayTypesGridView.MultiSelect = False
        Me.DayTypesGridView.Name = "DayTypesGridView"
        Me.DayTypesGridView.ReadOnly = True
        Me.DayTypesGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DayTypesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DayTypesGridView.Size = New System.Drawing.Size(352, 375)
        Me.DayTypesGridView.TabIndex = 146
        '
        'cemp_EmployeeID
        '
        Me.cemp_EmployeeID.DataPropertyName = "Name"
        Me.cemp_EmployeeID.HeaderText = "Day Types"
        Me.cemp_EmployeeID.Name = "cemp_EmployeeID"
        Me.cemp_EmployeeID.ReadOnly = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "Comments"
        Me.DataGridViewTextBoxColumn1.FillWeight = 47.71573!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Code"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 74
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "PartNo"
        Me.DataGridViewTextBoxColumn2.FillWeight = 152.2843!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 235
        '
        'DayTypesDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(751, 389)
        Me.Controls.Add(Me.DetailsGroupBox)
        Me.Controls.Add(Me.DayTypesGridView)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "DayTypesDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Branch Form"
        Me.DetailsGroupBox.ResumeLayout(False)
        CType(Me.DayTypesGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents SaveButton As Button
    Friend WithEvents DetailsGroupBox As GroupBox
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DayTypesGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents cemp_EmployeeID As DataGridViewTextBoxColumn
    Friend WithEvents DayTypeControl As DayTypeControl
End Class
