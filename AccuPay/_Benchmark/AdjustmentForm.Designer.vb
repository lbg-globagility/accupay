<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdjustmentForm
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
        Me.AdjustmentGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.cemp_EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cemp_LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DetailsGroupBox = New System.Windows.Forms.GroupBox()
        Me.AllowanceDetailsTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.DescriptionTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.plnCboLoanType = New System.Windows.Forms.Panel()
        Me.CodeTextBox = New System.Windows.Forms.TextBox()
        Me.Label350 = New System.Windows.Forms.Label()
        Me.Label156 = New System.Windows.Forms.Label()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.EditButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.AdjustmentGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DetailsGroupBox.SuspendLayout()
        Me.AllowanceDetailsTabLayout.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.plnCboLoanType.SuspendLayout()
        Me.SuspendLayout()
        '
        'AdjustmentGridView
        '
        Me.AdjustmentGridView.AllowUserToAddRows = False
        Me.AdjustmentGridView.AllowUserToDeleteRows = False
        Me.AdjustmentGridView.AllowUserToOrderColumns = True
        Me.AdjustmentGridView.AllowUserToResizeRows = False
        Me.AdjustmentGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AdjustmentGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.AdjustmentGridView.BackgroundColor = System.Drawing.Color.White
        Me.AdjustmentGridView.ColumnHeadersHeight = 34
        Me.AdjustmentGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.AdjustmentGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cemp_EmployeeID, Me.cemp_LastName})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.AdjustmentGridView.DefaultCellStyle = DataGridViewCellStyle1
        Me.AdjustmentGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.AdjustmentGridView.Location = New System.Drawing.Point(14, 12)
        Me.AdjustmentGridView.MultiSelect = False
        Me.AdjustmentGridView.Name = "AdjustmentGridView"
        Me.AdjustmentGridView.ReadOnly = True
        Me.AdjustmentGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.AdjustmentGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.AdjustmentGridView.Size = New System.Drawing.Size(352, 222)
        Me.AdjustmentGridView.TabIndex = 141
        '
        'cemp_EmployeeID
        '
        Me.cemp_EmployeeID.DataPropertyName = "Comments"
        Me.cemp_EmployeeID.FillWeight = 47.71573!
        Me.cemp_EmployeeID.HeaderText = "Code"
        Me.cemp_EmployeeID.Name = "cemp_EmployeeID"
        Me.cemp_EmployeeID.ReadOnly = True
        '
        'cemp_LastName
        '
        Me.cemp_LastName.DataPropertyName = "PartNo"
        Me.cemp_LastName.FillWeight = 152.2843!
        Me.cemp_LastName.HeaderText = "Description"
        Me.cemp_LastName.Name = "cemp_LastName"
        Me.cemp_LastName.ReadOnly = True
        '
        'DetailsGroupBox
        '
        Me.DetailsGroupBox.Controls.Add(Me.AllowanceDetailsTabLayout)
        Me.DetailsGroupBox.Controls.Add(Me.SaveButton)
        Me.DetailsGroupBox.Enabled = False
        Me.DetailsGroupBox.Location = New System.Drawing.Point(373, 12)
        Me.DetailsGroupBox.Name = "DetailsGroupBox"
        Me.DetailsGroupBox.Size = New System.Drawing.Size(300, 222)
        Me.DetailsGroupBox.TabIndex = 142
        Me.DetailsGroupBox.TabStop = False
        Me.DetailsGroupBox.Text = "Details"
        '
        'AllowanceDetailsTabLayout
        '
        Me.AllowanceDetailsTabLayout.ColumnCount = 1
        Me.AllowanceDetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Panel1, 0, 3)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Label1, 0, 2)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.plnCboLoanType, 0, 1)
        Me.AllowanceDetailsTabLayout.Controls.Add(Me.Label156, 0, 0)
        Me.AllowanceDetailsTabLayout.Location = New System.Drawing.Point(26, 36)
        Me.AllowanceDetailsTabLayout.Name = "AllowanceDetailsTabLayout"
        Me.AllowanceDetailsTabLayout.RowCount = 4
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.AllowanceDetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.AllowanceDetailsTabLayout.Size = New System.Drawing.Size(265, 99)
        Me.AllowanceDetailsTabLayout.TabIndex = 381
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.DescriptionTextBox)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 64)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(265, 35)
        Me.Panel1.TabIndex = 368
        '
        'DescriptionTextBox
        '
        Me.DescriptionTextBox.Location = New System.Drawing.Point(20, 2)
        Me.DescriptionTextBox.Name = "DescriptionTextBox"
        Me.DescriptionTextBox.Size = New System.Drawing.Size(192, 22)
        Me.DescriptionTextBox.TabIndex = 509
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(3, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(13, 13)
        Me.Label2.TabIndex = 507
        Me.Label2.Text = "*"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 48)
        Me.Label1.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 367
        Me.Label1.Text = "Description"
        '
        'plnCboLoanType
        '
        Me.plnCboLoanType.Controls.Add(Me.CodeTextBox)
        Me.plnCboLoanType.Controls.Add(Me.Label350)
        Me.plnCboLoanType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plnCboLoanType.Location = New System.Drawing.Point(0, 16)
        Me.plnCboLoanType.Margin = New System.Windows.Forms.Padding(0)
        Me.plnCboLoanType.Name = "plnCboLoanType"
        Me.plnCboLoanType.Size = New System.Drawing.Size(265, 32)
        Me.plnCboLoanType.TabIndex = 366
        '
        'CodeTextBox
        '
        Me.CodeTextBox.Location = New System.Drawing.Point(20, 2)
        Me.CodeTextBox.Name = "CodeTextBox"
        Me.CodeTextBox.Size = New System.Drawing.Size(192, 22)
        Me.CodeTextBox.TabIndex = 508
        '
        'Label350
        '
        Me.Label350.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label350.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label350.Location = New System.Drawing.Point(3, 4)
        Me.Label350.Name = "Label350"
        Me.Label350.Size = New System.Drawing.Size(13, 13)
        Me.Label350.TabIndex = 507
        Me.Label350.Text = "*"
        '
        'Label156
        '
        Me.Label156.AutoSize = True
        Me.Label156.Location = New System.Drawing.Point(20, 0)
        Me.Label156.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label156.Name = "Label156"
        Me.Label156.Size = New System.Drawing.Size(34, 13)
        Me.Label156.TabIndex = 365
        Me.Label156.Text = "Code"
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.Location = New System.Drawing.Point(142, 141)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(96, 26)
        Me.SaveButton.TabIndex = 146
        Me.SaveButton.Text = "&Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddButton.Location = New System.Drawing.Point(62, 240)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(96, 32)
        Me.AddButton.TabIndex = 143
        Me.AddButton.Text = "&Add"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'EditButton
        '
        Me.EditButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EditButton.Location = New System.Drawing.Point(166, 240)
        Me.EditButton.Name = "EditButton"
        Me.EditButton.Size = New System.Drawing.Size(96, 32)
        Me.EditButton.TabIndex = 144
        Me.EditButton.Text = "&Edit"
        Me.EditButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.Location = New System.Drawing.Point(270, 240)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(96, 32)
        Me.DeleteButton.TabIndex = 145
        Me.DeleteButton.Text = "&Delete"
        Me.DeleteButton.UseVisualStyleBackColor = True
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
        'AdjustmentForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(682, 276)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.EditButton)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.DetailsGroupBox)
        Me.Controls.Add(Me.AdjustmentGridView)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AdjustmentForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Adjustment Form"
        CType(Me.AdjustmentGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DetailsGroupBox.ResumeLayout(False)
        Me.AllowanceDetailsTabLayout.ResumeLayout(False)
        Me.AllowanceDetailsTabLayout.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.plnCboLoanType.ResumeLayout(False)
        Me.plnCboLoanType.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AdjustmentGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents cemp_EmployeeID As DataGridViewTextBoxColumn
    Friend WithEvents cemp_LastName As DataGridViewTextBoxColumn
    Friend WithEvents DetailsGroupBox As GroupBox
    Friend WithEvents AddButton As Button
    Friend WithEvents EditButton As Button
    Friend WithEvents DeleteButton As Button
    Friend WithEvents AllowanceDetailsTabLayout As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents DescriptionTextBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents plnCboLoanType As Panel
    Friend WithEvents CodeTextBox As TextBox
    Friend WithEvents Label350 As Label
    Friend WithEvents Label156 As Label
    Friend WithEvents SaveButton As Button
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
End Class
