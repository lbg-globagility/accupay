<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SetOvertimeForm
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label129 = New System.Windows.Forms.Label()
        Me.TotalOvertimeHoursLabel = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.OvertimeComboBox = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.RemoveButton = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PercentageTextBox = New System.Windows.Forms.TextBox()
        Me.DaysRadioButton = New System.Windows.Forms.RadioButton()
        Me.InputTextBox = New System.Windows.Forms.TextBox()
        Me.HoursRadioButton = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TotalOvertimeAmountLabel = New System.Windows.Forms.TextBox()
        Me.BackButton = New System.Windows.Forms.Button()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OvertimeGridView = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.OvertimeGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label129
        '
        Me.Label129.BackColor = System.Drawing.Color.FromArgb(CType(CType(113, Byte), Integer), CType(CType(202, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.Label129.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label129.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label129.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label129.Location = New System.Drawing.Point(0, 0)
        Me.Label129.Name = "Label129"
        Me.Label129.Padding = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.Label129.Size = New System.Drawing.Size(539, 40)
        Me.Label129.TabIndex = 15
        Me.Label129.Text = "SET OVERTIME"
        Me.Label129.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalOvertimeHoursLabel
        '
        Me.TotalOvertimeHoursLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TotalOvertimeHoursLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TotalOvertimeHoursLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold)
        Me.TotalOvertimeHoursLabel.ForeColor = System.Drawing.Color.White
        Me.TotalOvertimeHoursLabel.Location = New System.Drawing.Point(0, 40)
        Me.TotalOvertimeHoursLabel.Name = "TotalOvertimeHoursLabel"
        Me.TotalOvertimeHoursLabel.Padding = New System.Windows.Forms.Padding(17, 0, 0, 0)
        Me.TotalOvertimeHoursLabel.Size = New System.Drawing.Size(539, 35)
        Me.TotalOvertimeHoursLabel.TabIndex = 16
        Me.TotalOvertimeHoursLabel.Text = "Total Overtime (Hours): 0"
        Me.TotalOvertimeHoursLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.OvertimeComboBox)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 75)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(539, 35)
        Me.Panel1.TabIndex = 17
        '
        'OvertimeComboBox
        '
        Me.OvertimeComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OvertimeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.OvertimeComboBox.DropDownHeight = 400
        Me.OvertimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.OvertimeComboBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OvertimeComboBox.FormattingEnabled = True
        Me.OvertimeComboBox.IntegralHeight = False
        Me.OvertimeComboBox.Location = New System.Drawing.Point(185, 3)
        Me.OvertimeComboBox.Name = "OvertimeComboBox"
        Me.OvertimeComboBox.Size = New System.Drawing.Size(342, 25)
        Me.OvertimeComboBox.TabIndex = 18
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(17, 7)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(162, 18)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "OVERTIME SELECT"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.RemoveButton)
        Me.Panel2.Controls.Add(Me.AddButton)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 110)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(539, 35)
        Me.Panel2.TabIndex = 19
        '
        'RemoveButton
        '
        Me.RemoveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RemoveButton.BackColor = System.Drawing.Color.Crimson
        Me.RemoveButton.FlatAppearance.BorderSize = 0
        Me.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RemoveButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RemoveButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.RemoveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.RemoveButton.ImageKey = "cross-delete-or-close-interface-symbol-white.png"
        Me.RemoveButton.Location = New System.Drawing.Point(284, 5)
        Me.RemoveButton.Name = "RemoveButton"
        Me.RemoveButton.Size = New System.Drawing.Size(243, 24)
        Me.RemoveButton.TabIndex = 20
        Me.RemoveButton.Text = "REMOVE"
        Me.RemoveButton.UseVisualStyleBackColor = False
        '
        'AddButton
        '
        Me.AddButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(113, Byte), Integer), CType(CType(202, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.AddButton.FlatAppearance.BorderSize = 0
        Me.AddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.AddButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.AddButton.ImageKey = "add-interface-circular-symbol-with-plus-sign.png"
        Me.AddButton.Location = New System.Drawing.Point(20, 5)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(235, 24)
        Me.AddButton.TabIndex = 20
        Me.AddButton.Text = "  ADD"
        Me.AddButton.UseVisualStyleBackColor = False
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.PercentageTextBox)
        Me.Panel3.Controls.Add(Me.DaysRadioButton)
        Me.Panel3.Controls.Add(Me.InputTextBox)
        Me.Panel3.Controls.Add(Me.HoursRadioButton)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 145)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(539, 35)
        Me.Panel3.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(305, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(121, 18)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "PERCENTAGE"
        '
        'PercentageTextBox
        '
        Me.PercentageTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PercentageTextBox.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.PercentageTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PercentageTextBox.Location = New System.Drawing.Point(427, 6)
        Me.PercentageTextBox.Name = "PercentageTextBox"
        Me.PercentageTextBox.ReadOnly = True
        Me.PercentageTextBox.Size = New System.Drawing.Size(100, 24)
        Me.PercentageTextBox.TabIndex = 21
        Me.PercentageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'DaysRadioButton
        '
        Me.DaysRadioButton.AutoSize = True
        Me.DaysRadioButton.BackColor = System.Drawing.Color.Transparent
        Me.DaysRadioButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DaysRadioButton.Location = New System.Drawing.Point(207, 7)
        Me.DaysRadioButton.Name = "DaysRadioButton"
        Me.DaysRadioButton.Size = New System.Drawing.Size(69, 22)
        Me.DaysRadioButton.TabIndex = 28
        Me.DaysRadioButton.TabStop = True
        Me.DaysRadioButton.Text = "DAY/S"
        Me.DaysRadioButton.UseVisualStyleBackColor = False
        '
        'InputTextBox
        '
        Me.InputTextBox.Enabled = False
        Me.InputTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputTextBox.Location = New System.Drawing.Point(20, 6)
        Me.InputTextBox.Name = "InputTextBox"
        Me.InputTextBox.Size = New System.Drawing.Size(100, 24)
        Me.InputTextBox.TabIndex = 22
        Me.InputTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'HoursRadioButton
        '
        Me.HoursRadioButton.AutoSize = True
        Me.HoursRadioButton.BackColor = System.Drawing.Color.Transparent
        Me.HoursRadioButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HoursRadioButton.Location = New System.Drawing.Point(123, 7)
        Me.HoursRadioButton.Name = "HoursRadioButton"
        Me.HoursRadioButton.Size = New System.Drawing.Size(85, 22)
        Me.HoursRadioButton.TabIndex = 27
        Me.HoursRadioButton.TabStop = True
        Me.HoursRadioButton.Text = "HOUR/S"
        Me.HoursRadioButton.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(17, 374)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(226, 18)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "TOTAL OVERTIME AMOUNT"
        '
        'TotalOvertimeAmountLabel
        '
        Me.TotalOvertimeAmountLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TotalOvertimeAmountLabel.BackColor = System.Drawing.SystemColors.Control
        Me.TotalOvertimeAmountLabel.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TotalOvertimeAmountLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TotalOvertimeAmountLabel.Location = New System.Drawing.Point(322, 375)
        Me.TotalOvertimeAmountLabel.Name = "TotalOvertimeAmountLabel"
        Me.TotalOvertimeAmountLabel.ReadOnly = True
        Me.TotalOvertimeAmountLabel.Size = New System.Drawing.Size(205, 17)
        Me.TotalOvertimeAmountLabel.TabIndex = 23
        Me.TotalOvertimeAmountLabel.Text = "0.00"
        Me.TotalOvertimeAmountLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'BackButton
        '
        Me.BackButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BackButton.BackColor = System.Drawing.Color.Crimson
        Me.BackButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BackButton.FlatAppearance.BorderSize = 0
        Me.BackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BackButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.BackButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BackButton.ImageKey = "the18.png"
        Me.BackButton.Location = New System.Drawing.Point(380, 400)
        Me.BackButton.Name = "BackButton"
        Me.BackButton.Size = New System.Drawing.Size(147, 29)
        Me.BackButton.TabIndex = 24
        Me.BackButton.Text = "   BACK TO MAIN"
        Me.BackButton.UseVisualStyleBackColor = False
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "Description"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 230
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "Hours"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Hours"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 125
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "Amount"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Amount"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 125
        '
        'OvertimeGridView
        '
        Me.OvertimeGridView.AllowUserToAddRows = False
        Me.OvertimeGridView.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.OvertimeGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.OvertimeGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.OvertimeGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.OvertimeGridView.DefaultCellStyle = DataGridViewCellStyle4
        Me.OvertimeGridView.Location = New System.Drawing.Point(20, 186)
        Me.OvertimeGridView.Name = "OvertimeGridView"
        Me.OvertimeGridView.ReadOnly = True
        Me.OvertimeGridView.Size = New System.Drawing.Size(507, 185)
        Me.OvertimeGridView.TabIndex = 25
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "Description"
        Me.Column1.HeaderText = "Description"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 220
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "Hours"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column2.HeaderText = "Hours"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 120
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "Amount"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column3.HeaderText = "Amount"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 120
        '
        'SetOvertimeForm
        '
        Me.AcceptButton = Me.AddButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BackButton
        Me.ClientSize = New System.Drawing.Size(539, 435)
        Me.Controls.Add(Me.OvertimeGridView)
        Me.Controls.Add(Me.BackButton)
        Me.Controls.Add(Me.TotalOvertimeAmountLabel)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TotalOvertimeHoursLabel)
        Me.Controls.Add(Me.Label129)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "SetOvertimeForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Set Overtime"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.OvertimeGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label129 As Label
    Friend WithEvents TotalOvertimeHoursLabel As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents OvertimeComboBox As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents RemoveButton As Button
    Friend WithEvents AddButton As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents PercentageTextBox As TextBox
    Friend WithEvents DaysRadioButton As RadioButton
    Friend WithEvents InputTextBox As TextBox
    Friend WithEvents HoursRadioButton As RadioButton
    Friend WithEvents Label4 As Label
    Friend WithEvents TotalOvertimeAmountLabel As TextBox
    Friend WithEvents BackButton As Button
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents OvertimeGridView As DataGridView
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
End Class
