Imports AccuPay.Data.Repositories

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UserActivityForm
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserActivityForm))
        Me.UserActivityGrid = New System.Windows.Forms.DataGridView()
        Me.NameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ChangedEntityColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DescriptionColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateandTimeColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastLinkLabel = New System.Windows.Forms.LinkLabel()
        Me.NextLinkLabel = New System.Windows.Forms.LinkLabel()
        Me.PreviousLinkLabel = New System.Windows.Forms.LinkLabel()
        Me.FirstLinkLabel = New System.Windows.Forms.LinkLabel()
        Me.ChangedByLabel = New System.Windows.Forms.Label()
        Me.ChangedByComboBox = New System.Windows.Forms.ComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.FilterButton = New System.Windows.Forms.Button()
        Me.ToDatePicker = New NullableDatePicker()
        Me.ToLabel = New System.Windows.Forms.Label()
        Me.FromDatePicker = New NullableDatePicker()
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.DescriptionTextBox = New System.Windows.Forms.TextBox()
        Me.DescriptionLabel = New System.Windows.Forms.Label()
        Me.ChangedEntityComboBox = New System.Windows.Forms.ComboBox()
        Me.ChangedEntityLabel = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        CType(Me.UserActivityGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'UserActivityGrid
        '
        Me.UserActivityGrid.AllowUserToAddRows = False
        Me.UserActivityGrid.AllowUserToDeleteRows = False
        Me.UserActivityGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.UserActivityGrid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.UserActivityGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.UserActivityGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.UserActivityGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NameColumn, Me.ChangedEntityColumn, Me.DescriptionColumn, Me.DateandTimeColumn})
        Me.UserActivityGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserActivityGrid.Location = New System.Drawing.Point(0, 64)
        Me.UserActivityGrid.Name = "UserActivityGrid"
        Me.UserActivityGrid.ReadOnly = True
        Me.UserActivityGrid.Size = New System.Drawing.Size(1184, 422)
        Me.UserActivityGrid.TabIndex = 0
        '
        'NameColumn
        '
        Me.NameColumn.DataPropertyName = "ChangedBy"
        Me.NameColumn.FillWeight = 1.25!
        Me.NameColumn.HeaderText = "Changed By"
        Me.NameColumn.Name = "NameColumn"
        Me.NameColumn.ReadOnly = True
        '
        'ChangedEntityColumn
        '
        Me.ChangedEntityColumn.DataPropertyName = "ChangedEntity"
        DataGridViewCellStyle2.NullValue = "--"
        Me.ChangedEntityColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.ChangedEntityColumn.FillWeight = 1.55!
        Me.ChangedEntityColumn.HeaderText = "ChangedEntity"
        Me.ChangedEntityColumn.Name = "ChangedEntityColumn"
        Me.ChangedEntityColumn.ReadOnly = True
        '
        'DescriptionColumn
        '
        Me.DescriptionColumn.DataPropertyName = "Description"
        Me.DescriptionColumn.FillWeight = 6.1!
        Me.DescriptionColumn.HeaderText = "Description"
        Me.DescriptionColumn.Name = "DescriptionColumn"
        Me.DescriptionColumn.ReadOnly = True
        '
        'DateandTimeColumn
        '
        Me.DateandTimeColumn.DataPropertyName = "DateandTime"
        Me.DateandTimeColumn.FillWeight = 1.1!
        Me.DateandTimeColumn.HeaderText = "Date and Time"
        Me.DateandTimeColumn.Name = "DateandTimeColumn"
        Me.DateandTimeColumn.ReadOnly = True
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(1075, 30)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(96, 32)
        Me.CloseButton.TabIndex = 2
        Me.CloseButton.Text = "&Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "Name"
        Me.DataGridViewTextBoxColumn1.FillWeight = 3.0!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 60
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "Description"
        Me.DataGridViewTextBoxColumn2.FillWeight = 7.908267!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 85
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "DateandTime"
        Me.DataGridViewTextBoxColumn3.FillWeight = 3.0!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Date and Time"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 102
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "DateandTime"
        Me.DataGridViewTextBoxColumn4.FillWeight = 1.8!
        Me.DataGridViewTextBoxColumn4.HeaderText = "Date and Time"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 112
        '
        'LastLinkLabel
        '
        Me.LastLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LastLinkLabel.AutoSize = True
        Me.LastLinkLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LastLinkLabel.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LastLinkLabel.Location = New System.Drawing.Point(659, 10)
        Me.LastLinkLabel.Name = "LastLinkLabel"
        Me.LastLinkLabel.Size = New System.Drawing.Size(44, 15)
        Me.LastLinkLabel.TabIndex = 154
        Me.LastLinkLabel.TabStop = True
        Me.LastLinkLabel.Text = "Last>>"
        '
        'NextLinkLabel
        '
        Me.NextLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.NextLinkLabel.AutoSize = True
        Me.NextLinkLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NextLinkLabel.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.NextLinkLabel.Location = New System.Drawing.Point(614, 10)
        Me.NextLinkLabel.Name = "NextLinkLabel"
        Me.NextLinkLabel.Size = New System.Drawing.Size(39, 15)
        Me.NextLinkLabel.TabIndex = 153
        Me.NextLinkLabel.TabStop = True
        Me.NextLinkLabel.Text = "Next>"
        '
        'PreviousLinkLabel
        '
        Me.PreviousLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.PreviousLinkLabel.AutoSize = True
        Me.PreviousLinkLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PreviousLinkLabel.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PreviousLinkLabel.Location = New System.Drawing.Point(544, 10)
        Me.PreviousLinkLabel.Name = "PreviousLinkLabel"
        Me.PreviousLinkLabel.Size = New System.Drawing.Size(38, 15)
        Me.PreviousLinkLabel.TabIndex = 152
        Me.PreviousLinkLabel.TabStop = True
        Me.PreviousLinkLabel.Text = "<Prev"
        '
        'FirstLinkLabel
        '
        Me.FirstLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.FirstLinkLabel.AutoSize = True
        Me.FirstLinkLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FirstLinkLabel.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.FirstLinkLabel.Location = New System.Drawing.Point(495, 10)
        Me.FirstLinkLabel.Name = "FirstLinkLabel"
        Me.FirstLinkLabel.Size = New System.Drawing.Size(44, 15)
        Me.FirstLinkLabel.TabIndex = 151
        Me.FirstLinkLabel.TabStop = True
        Me.FirstLinkLabel.Text = "<<First"
        '
        'ChangedByLabel
        '
        Me.ChangedByLabel.AutoSize = True
        Me.ChangedByLabel.Location = New System.Drawing.Point(24, 23)
        Me.ChangedByLabel.Name = "ChangedByLabel"
        Me.ChangedByLabel.Size = New System.Drawing.Size(71, 13)
        Me.ChangedByLabel.TabIndex = 155
        Me.ChangedByLabel.Text = "Changed By:"
        '
        'ChangedByComboBox
        '
        Me.ChangedByComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ChangedByComboBox.FormattingEnabled = True
        Me.ChangedByComboBox.Location = New System.Drawing.Point(101, 20)
        Me.ChangedByComboBox.Name = "ChangedByComboBox"
        Me.ChangedByComboBox.Size = New System.Drawing.Size(121, 21)
        Me.ChangedByComboBox.TabIndex = 156
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.FilterButton)
        Me.Panel1.Controls.Add(Me.ToDatePicker)
        Me.Panel1.Controls.Add(Me.ToLabel)
        Me.Panel1.Controls.Add(Me.FromDatePicker)
        Me.Panel1.Controls.Add(Me.FromLabel)
        Me.Panel1.Controls.Add(Me.DescriptionTextBox)
        Me.Panel1.Controls.Add(Me.DescriptionLabel)
        Me.Panel1.Controls.Add(Me.ChangedEntityComboBox)
        Me.Panel1.Controls.Add(Me.ChangedEntityLabel)
        Me.Panel1.Controls.Add(Me.ChangedByComboBox)
        Me.Panel1.Controls.Add(Me.ChangedByLabel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1184, 64)
        Me.Panel1.TabIndex = 157
        '
        'FilterButton
        '
        Me.FilterButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilterButton.Location = New System.Drawing.Point(1075, 14)
        Me.FilterButton.Name = "FilterButton"
        Me.FilterButton.Size = New System.Drawing.Size(96, 32)
        Me.FilterButton.TabIndex = 155
        Me.FilterButton.Text = "&Filter"
        Me.FilterButton.UseVisualStyleBackColor = True
        '
        'ToDatePicker
        '
        Me.ToDatePicker.Location = New System.Drawing.Point(917, 20)
        Me.ToDatePicker.Margin = New System.Windows.Forms.Padding(0)
        Me.ToDatePicker.Name = "ToDatePicker"
        Me.ToDatePicker.Size = New System.Drawing.Size(100, 22)
        Me.ToDatePicker.TabIndex = 164
        Me.ToDatePicker.Value = New Date(2020, 11, 17, 16, 49, 4, 845)
        '
        'ToLabel
        '
        Me.ToLabel.AutoSize = True
        Me.ToLabel.Location = New System.Drawing.Point(890, 23)
        Me.ToLabel.Name = "ToLabel"
        Me.ToLabel.Size = New System.Drawing.Size(22, 13)
        Me.ToLabel.TabIndex = 163
        Me.ToLabel.Text = "To:"
        '
        'FromDatePicker
        '
        Me.FromDatePicker.Location = New System.Drawing.Point(780, 20)
        Me.FromDatePicker.Margin = New System.Windows.Forms.Padding(0)
        Me.FromDatePicker.Name = "FromDatePicker"
        Me.FromDatePicker.Size = New System.Drawing.Size(100, 22)
        Me.FromDatePicker.TabIndex = 162
        Me.FromDatePicker.Value = New Date(2020, 11, 17, 16, 49, 4, 845)
        '
        'FromLabel
        '
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(740, 23)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(36, 13)
        Me.FromLabel.TabIndex = 161
        Me.FromLabel.Text = "From:"
        '
        'DescriptionTextBox
        '
        Me.DescriptionTextBox.Location = New System.Drawing.Point(560, 20)
        Me.DescriptionTextBox.Name = "DescriptionTextBox"
        Me.DescriptionTextBox.Size = New System.Drawing.Size(164, 22)
        Me.DescriptionTextBox.TabIndex = 160
        '
        'DescriptionLabel
        '
        Me.DescriptionLabel.AutoSize = True
        Me.DescriptionLabel.Location = New System.Drawing.Point(485, 23)
        Me.DescriptionLabel.Name = "DescriptionLabel"
        Me.DescriptionLabel.Size = New System.Drawing.Size(69, 13)
        Me.DescriptionLabel.TabIndex = 159
        Me.DescriptionLabel.Text = "Description:"
        '
        'ChangedEntityComboBox
        '
        Me.ChangedEntityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ChangedEntityComboBox.FormattingEnabled = True
        Me.ChangedEntityComboBox.Location = New System.Drawing.Point(295, 20)
        Me.ChangedEntityComboBox.Name = "ChangedEntityComboBox"
        Me.ChangedEntityComboBox.Size = New System.Drawing.Size(177, 21)
        Me.ChangedEntityComboBox.TabIndex = 158
        '
        'ChangedEntityLabel
        '
        Me.ChangedEntityLabel.AutoSize = True
        Me.ChangedEntityLabel.Location = New System.Drawing.Point(234, 23)
        Me.ChangedEntityLabel.Name = "ChangedEntityLabel"
        Me.ChangedEntityLabel.Size = New System.Drawing.Size(59, 13)
        Me.ChangedEntityLabel.TabIndex = 157
        Me.ChangedEntityLabel.Text = "Employee:"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.FirstLinkLabel)
        Me.Panel2.Controls.Add(Me.PreviousLinkLabel)
        Me.Panel2.Controls.Add(Me.CloseButton)
        Me.Panel2.Controls.Add(Me.LastLinkLabel)
        Me.Panel2.Controls.Add(Me.NextLinkLabel)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 486)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1184, 75)
        Me.Panel2.TabIndex = 158
        '
        'UserActivityForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1184, 561)
        Me.Controls.Add(Me.UserActivityGrid)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "UserActivityForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "User Activity"
        CType(Me.UserActivityGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents UserActivityGrid As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents CloseButton As Button
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents NameColumn As DataGridViewTextBoxColumn
    Friend WithEvents ChangedEntityColumn As DataGridViewTextBoxColumn
    Friend WithEvents DescriptionColumn As DataGridViewTextBoxColumn
    Friend WithEvents DateandTimeColumn As DataGridViewTextBoxColumn
    Friend WithEvents LastLinkLabel As LinkLabel
    Friend WithEvents NextLinkLabel As LinkLabel
    Friend WithEvents PreviousLinkLabel As LinkLabel
    Friend WithEvents FirstLinkLabel As LinkLabel
    Friend WithEvents ChangedByLabel As Label
    Friend WithEvents ChangedByComboBox As ComboBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents DescriptionTextBox As TextBox
    Friend WithEvents DescriptionLabel As Label
    Friend WithEvents ChangedEntityComboBox As ComboBox
    Friend WithEvents ChangedEntityLabel As Label
    Friend WithEvents FilterButton As Button
    Friend WithEvents ToDatePicker As NullableDatePicker
    Friend WithEvents ToLabel As Label
    Friend WithEvents FromDatePicker As NullableDatePicker
    Friend WithEvents FromLabel As Label
End Class
