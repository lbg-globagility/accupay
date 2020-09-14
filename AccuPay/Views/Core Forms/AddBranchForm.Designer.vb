<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddBranchForm
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
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.EditButton = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.NameTextBox = New System.Windows.Forms.TextBox()
        Me.Label350 = New System.Windows.Forms.Label()
        Me.plnCboLoanType = New System.Windows.Forms.Panel()
        Me.Label156 = New System.Windows.Forms.Label()
        Me.AllowanceDetailsTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.CalendarPanel = New System.Windows.Forms.Panel()
        Me.CalendarComboBox = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DetailsGroupBox = New System.Windows.Forms.GroupBox()
        Me.BranchGrid = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.cemp_EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.plnCboLoanType.SuspendLayout()
        Me.AllowanceDetailsTabLayout.SuspendLayout()
        Me.CalendarPanel.SuspendLayout()
        Me.DetailsGroupBox.SuspendLayout()
        CType(Me.BranchGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.Location = New System.Drawing.Point(268, 236)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(96, 32)
        Me.DeleteButton.TabIndex = 150
        Me.DeleteButton.Text = "&Delete"
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'EditButton
        '
        Me.EditButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EditButton.Location = New System.Drawing.Point(164, 236)
        Me.EditButton.Name = "EditButton"
        Me.EditButton.Size = New System.Drawing.Size(96, 32)
        Me.EditButton.TabIndex = 149
        Me.EditButton.Text = "&Edit"
        Me.EditButton.UseVisualStyleBackColor = True
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddButton.Location = New System.Drawing.Point(60, 236)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(96, 32)
        Me.AddButton.TabIndex = 148
        Me.AddButton.Text = "&Add"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.Location = New System.Drawing.Point(141, 142)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(96, 26)
        Me.SaveButton.TabIndex = 146
        Me.SaveButton.Text = "&Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'NameTextBox
        '
        Me.NameTextBox.Location = New System.Drawing.Point(20, 2)
        Me.NameTextBox.Name = "NameTextBox"
        Me.NameTextBox.Size = New System.Drawing.Size(192, 22)
        Me.NameTextBox.TabIndex = 508
        '
        'Label350
        '
        Me.Label350.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label350.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label350.Location = New System.Drawing.Point(6, 4)
        Me.Label350.Name = "Label350"
        Me.Label350.Size = New System.Drawing.Size(14, 16)
        Me.Label350.TabIndex = 507
        Me.Label350.Text = "*"
        '
        'plnCboLoanType
        '
        Me.plnCboLoanType.Controls.Add(Me.NameTextBox)
        Me.plnCboLoanType.Controls.Add(Me.Label350)
        Me.plnCboLoanType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plnCboLoanType.Location = New System.Drawing.Point(0, 16)
        Me.plnCboLoanType.Margin = New System.Windows.Forms.Padding(0)
        Me.plnCboLoanType.Name = "plnCboLoanType"
        Me.plnCboLoanType.Size = New System.Drawing.Size(238, 32)
        Me.plnCboLoanType.TabIndex = 366
        '
        'Label156
        '
        Me.Label156.AutoSize = True
        Me.Label156.Location = New System.Drawing.Point(21, -3)
        Me.Label156.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label156.Name = "Label156"
        Me.Label156.Size = New System.Drawing.Size(53, 13)
        Me.Label156.TabIndex = 365
        Me.Label156.Text = "Calendar"
        '
        'AllowanceDetailsTabLayout
        '
        Me.AllowanceDetailsTabLayout.ColumnCount = 1
        Me.AllowanceDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.plnCboLoanType, 0, 1)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.CalendarPanel, 0, 2)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Label2, 0, 0)
        Me.AllowanceDetailsTabLayout.Location = New System.Drawing.Point(26, 36)
        Me.AllowanceDetailsTabLayout.Name = "AllowanceDetailsTabLayout"
        Me.AllowanceDetailsTabLayout.RowCount = 3
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.AllowanceDetailsTabLayout.Size = New System.Drawing.Size(238, 100)
        Me.AllowanceDetailsTabLayout.TabIndex = 381
        '
        'CalendarPanel
        '
        Me.CalendarPanel.Controls.Add(Me.CalendarComboBox)
        Me.CalendarPanel.Controls.Add(Me.Label1)
        Me.CalendarPanel.Controls.Add(Me.Label156)
        Me.CalendarPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CalendarPanel.Location = New System.Drawing.Point(3, 51)
        Me.CalendarPanel.Name = "CalendarPanel"
        Me.CalendarPanel.Size = New System.Drawing.Size(232, 46)
        Me.CalendarPanel.TabIndex = 367
        '
        'CalendarComboBox
        '
        Me.CalendarComboBox.DisplayMember = "Name"
        Me.CalendarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CalendarComboBox.FormattingEnabled = True
        Me.CalendarComboBox.Location = New System.Drawing.Point(16, 16)
        Me.CalendarComboBox.Name = "CalendarComboBox"
        Me.CalendarComboBox.Size = New System.Drawing.Size(192, 21)
        Me.CalendarComboBox.TabIndex = 367
        Me.CalendarComboBox.ValueMember = "RowID"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(3, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(16, 16)
        Me.Label1.TabIndex = 507
        Me.Label1.Text = "*"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 0)
        Me.Label2.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 13)
        Me.Label2.TabIndex = 365
        Me.Label2.Text = "Branch Name"
        '
        'DetailsGroupBox
        '
        Me.DetailsGroupBox.Controls.Add(Me.AllowanceDetailsTabLayout)
        Me.DetailsGroupBox.Controls.Add(Me.SaveButton)
        Me.DetailsGroupBox.Enabled = False
        Me.DetailsGroupBox.Location = New System.Drawing.Point(371, 8)
        Me.DetailsGroupBox.Name = "DetailsGroupBox"
        Me.DetailsGroupBox.Size = New System.Drawing.Size(285, 222)
        Me.DetailsGroupBox.TabIndex = 147
        Me.DetailsGroupBox.TabStop = False
        Me.DetailsGroupBox.Text = "Branch"
        '
        'BranchGridView
        '
        Me.BranchGrid.AllowUserToAddRows = False
        Me.BranchGrid.AllowUserToDeleteRows = False
        Me.BranchGrid.AllowUserToOrderColumns = True
        Me.BranchGrid.AllowUserToResizeRows = False
        Me.BranchGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BranchGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.BranchGrid.BackgroundColor = System.Drawing.Color.White
        Me.BranchGrid.ColumnHeadersHeight = 34
        Me.BranchGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.BranchGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cemp_EmployeeID})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.BranchGrid.DefaultCellStyle = DataGridViewCellStyle1
        Me.BranchGrid.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.BranchGrid.Location = New System.Drawing.Point(12, 8)
        Me.BranchGrid.MultiSelect = False
        Me.BranchGrid.Name = "BranchGridView"
        Me.BranchGrid.ReadOnly = True
        Me.BranchGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.BranchGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.BranchGrid.Size = New System.Drawing.Size(352, 222)
        Me.BranchGrid.TabIndex = 146
        '
        'cemp_EmployeeID
        '
        Me.cemp_EmployeeID.DataPropertyName = "Name"
        Me.cemp_EmployeeID.HeaderText = "Branches"
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
        'AddBranchForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(661, 276)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.EditButton)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.DetailsGroupBox)
        Me.Controls.Add(Me.BranchGrid)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AddBranchForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Branch Form"
        Me.plnCboLoanType.ResumeLayout(False)
        Me.plnCboLoanType.PerformLayout()
        Me.AllowanceDetailsTabLayout.ResumeLayout(False)
        Me.AllowanceDetailsTabLayout.PerformLayout()
        Me.CalendarPanel.ResumeLayout(False)
        Me.CalendarPanel.PerformLayout()
        Me.DetailsGroupBox.ResumeLayout(False)
        CType(Me.BranchGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DeleteButton As Button
    Friend WithEvents EditButton As Button
    Friend WithEvents AddButton As Button
    Friend WithEvents SaveButton As Button
    Friend WithEvents NameTextBox As TextBox
    Friend WithEvents Label350 As Label
    Friend WithEvents plnCboLoanType As Panel
    Friend WithEvents Label156 As Label
    Friend WithEvents AllowanceDetailsTabLayout As TableLayoutPanel
    Friend WithEvents DetailsGroupBox As GroupBox
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents BranchGrid As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents cemp_EmployeeID As DataGridViewTextBoxColumn
    Friend WithEvents CalendarPanel As Panel
    Friend WithEvents CalendarComboBox As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
End Class
