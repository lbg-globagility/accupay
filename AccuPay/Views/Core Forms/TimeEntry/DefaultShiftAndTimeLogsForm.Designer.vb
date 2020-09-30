<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DefaultShiftAndTimeLogsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DefaultShiftAndTimeLogsForm))
        Me.EmployeePanel = New System.Windows.Forms.Panel()
        Me.ActionPanel = New System.Windows.Forms.Panel()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.CancelDialogButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.EmployeeDataGrid = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.DefaultDetailsPanel = New System.Windows.Forms.Panel()
        Me.DefaultBreakLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DefaultBreakTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DefaultEndTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DefaultStartTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeNumberColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastNameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstNameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeTypeColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeTreeView = New AccuPay.EmployeeTreeView()
        Me.EmployeePanel.SuspendLayout()
        Me.ActionPanel.SuspendLayout()
        CType(Me.EmployeeDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DefaultDetailsPanel.SuspendLayout()
        CType(Me.DefaultBreakLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'EmployeePanel
        '
        Me.EmployeePanel.Controls.Add(Me.EmployeeTreeView)
        Me.EmployeePanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.EmployeePanel.Location = New System.Drawing.Point(0, 0)
        Me.EmployeePanel.Name = "EmployeePanel"
        Me.EmployeePanel.Padding = New System.Windows.Forms.Padding(20, 10, 10, 0)
        Me.EmployeePanel.Size = New System.Drawing.Size(323, 749)
        Me.EmployeePanel.TabIndex = 0
        '
        'ActionPanel
        '
        Me.ActionPanel.Controls.Add(Me.DeleteButton)
        Me.ActionPanel.Controls.Add(Me.CancelDialogButton)
        Me.ActionPanel.Controls.Add(Me.SaveButton)
        Me.ActionPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ActionPanel.Location = New System.Drawing.Point(323, 707)
        Me.ActionPanel.Name = "ActionPanel"
        Me.ActionPanel.Size = New System.Drawing.Size(822, 42)
        Me.ActionPanel.TabIndex = 1
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.BackColor = System.Drawing.Color.Red
        Me.DeleteButton.Enabled = False
        Me.DeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DeleteButton.ForeColor = System.Drawing.Color.White
        Me.DeleteButton.Location = New System.Drawing.Point(504, 5)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(96, 32)
        Me.DeleteButton.TabIndex = 22
        Me.DeleteButton.Text = "&Delete"
        Me.DeleteButton.UseVisualStyleBackColor = False
        '
        'CancelDialogButton
        '
        Me.CancelDialogButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelDialogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CancelDialogButton.Location = New System.Drawing.Point(714, 5)
        Me.CancelDialogButton.Name = "CancelDialogButton"
        Me.CancelDialogButton.Size = New System.Drawing.Size(96, 32)
        Me.CancelDialogButton.TabIndex = 21
        Me.CancelDialogButton.Text = "&Cancel"
        Me.CancelDialogButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.Enabled = False
        Me.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SaveButton.Location = New System.Drawing.Point(609, 5)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(96, 32)
        Me.SaveButton.TabIndex = 20
        Me.SaveButton.Text = "C&reate"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'EmployeeDataGrid
        '
        Me.EmployeeDataGrid.AllowUserToAddRows = False
        Me.EmployeeDataGrid.AllowUserToDeleteRows = False
        Me.EmployeeDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.EmployeeDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.EmployeeDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.EmployeeDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.EmployeeNumberColumn, Me.LastNameColumn, Me.FirstNameColumn, Me.EmployeeTypeColumn})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.EmployeeDataGrid.DefaultCellStyle = DataGridViewCellStyle1
        Me.EmployeeDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EmployeeDataGrid.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.EmployeeDataGrid.Location = New System.Drawing.Point(323, 32)
        Me.EmployeeDataGrid.Name = "EmployeeDataGrid"
        Me.EmployeeDataGrid.ReadOnly = True
        Me.EmployeeDataGrid.Size = New System.Drawing.Size(822, 675)
        Me.EmployeeDataGrid.TabIndex = 2
        '
        'DefaultDetailsPanel
        '
        Me.DefaultDetailsPanel.Controls.Add(Me.DefaultBreakLengthNumeric)
        Me.DefaultDetailsPanel.Controls.Add(Me.Label4)
        Me.DefaultDetailsPanel.Controls.Add(Me.DefaultBreakTimePicker)
        Me.DefaultDetailsPanel.Controls.Add(Me.Label3)
        Me.DefaultDetailsPanel.Controls.Add(Me.DefaultEndTimePicker)
        Me.DefaultDetailsPanel.Controls.Add(Me.Label2)
        Me.DefaultDetailsPanel.Controls.Add(Me.DefaultStartTimePicker)
        Me.DefaultDetailsPanel.Controls.Add(Me.Label1)
        Me.DefaultDetailsPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.DefaultDetailsPanel.Location = New System.Drawing.Point(323, 0)
        Me.DefaultDetailsPanel.Name = "DefaultDetailsPanel"
        Me.DefaultDetailsPanel.Size = New System.Drawing.Size(822, 32)
        Me.DefaultDetailsPanel.TabIndex = 3
        '
        'DefaultBreakLengthNumeric
        '
        Me.DefaultBreakLengthNumeric.Location = New System.Drawing.Point(820, 6)
        Me.DefaultBreakLengthNumeric.Name = "DefaultBreakLengthNumeric"
        Me.DefaultBreakLengthNumeric.Size = New System.Drawing.Size(50, 22)
        Me.DefaultBreakLengthNumeric.TabIndex = 8
        Me.DefaultBreakLengthNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(680, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(132, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Default Break Length:"
        '
        'DefaultBreakTimePicker
        '
        Me.DefaultBreakTimePicker.CustomFormat = "  hh:mm tt"
        Me.DefaultBreakTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DefaultBreakTimePicker.Location = New System.Drawing.Point(583, 5)
        Me.DefaultBreakTimePicker.Name = "DefaultBreakTimePicker"
        Me.DefaultBreakTimePicker.ShowUpDown = True
        Me.DefaultBreakTimePicker.Size = New System.Drawing.Size(88, 22)
        Me.DefaultBreakTimePicker.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(455, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Default Break Time:"
        '
        'DefaultEndTimePicker
        '
        Me.DefaultEndTimePicker.CustomFormat = "  hh:mm tt"
        Me.DefaultEndTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DefaultEndTimePicker.Location = New System.Drawing.Point(340, 5)
        Me.DefaultEndTimePicker.Name = "DefaultEndTimePicker"
        Me.DefaultEndTimePicker.ShowUpDown = True
        Me.DefaultEndTimePicker.Size = New System.Drawing.Size(88, 22)
        Me.DefaultEndTimePicker.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(230, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(102, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Default Time To:"
        '
        'DefaultStartTimePicker
        '
        Me.DefaultStartTimePicker.CustomFormat = "  hh:mm tt"
        Me.DefaultStartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DefaultStartTimePicker.Location = New System.Drawing.Point(124, 5)
        Me.DefaultStartTimePicker.Name = "DefaultStartTimePicker"
        Me.DefaultStartTimePicker.ShowUpDown = True
        Me.DefaultStartTimePicker.Size = New System.Drawing.Size(88, 22)
        Me.DefaultStartTimePicker.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(114, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Default Time From:"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "EmployeeNumber"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee No"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 139
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "LastName"
        Me.DataGridViewTextBoxColumn2.FillWeight = 150.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 208
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "FirstName"
        Me.DataGridViewTextBoxColumn3.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 138
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "EmployeeType"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Employee Type"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 139
        '
        'EmployeeNumberColumn
        '
        Me.EmployeeNumberColumn.DataPropertyName = "EmployeeNo"
        Me.EmployeeNumberColumn.HeaderText = "Employee No"
        Me.EmployeeNumberColumn.Name = "EmployeeNumberColumn"
        Me.EmployeeNumberColumn.ReadOnly = True
        '
        'LastNameColumn
        '
        Me.LastNameColumn.DataPropertyName = "LastName"
        Me.LastNameColumn.FillWeight = 150.0!
        Me.LastNameColumn.HeaderText = "Last Name"
        Me.LastNameColumn.Name = "LastNameColumn"
        Me.LastNameColumn.ReadOnly = True
        '
        'FirstNameColumn
        '
        Me.FirstNameColumn.DataPropertyName = "FirstName"
        Me.FirstNameColumn.HeaderText = "First Name"
        Me.FirstNameColumn.Name = "FirstNameColumn"
        Me.FirstNameColumn.ReadOnly = True
        '
        'EmployeeTypeColumn
        '
        Me.EmployeeTypeColumn.DataPropertyName = "EmployeeType"
        Me.EmployeeTypeColumn.HeaderText = "Employee Type"
        Me.EmployeeTypeColumn.Name = "EmployeeTypeColumn"
        Me.EmployeeTypeColumn.ReadOnly = True
        '
        'EmployeeTreeView
        '
        Me.EmployeeTreeView.BackColor = System.Drawing.Color.Transparent
        Me.EmployeeTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EmployeeTreeView.Location = New System.Drawing.Point(20, 10)
        Me.EmployeeTreeView.Name = "EmployeeTreeView"
        Me.EmployeeTreeView.Size = New System.Drawing.Size(293, 739)
        Me.EmployeeTreeView.TabIndex = 316
        '
        'DefaultShiftAndTimeLogsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1145, 749)
        Me.Controls.Add(Me.EmployeeDataGrid)
        Me.Controls.Add(Me.DefaultDetailsPanel)
        Me.Controls.Add(Me.ActionPanel)
        Me.Controls.Add(Me.EmployeePanel)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "DefaultShiftAndTimeLogsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Default Shift and Time Logs Form"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.EmployeePanel.ResumeLayout(False)
        Me.ActionPanel.ResumeLayout(False)
        CType(Me.EmployeeDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DefaultDetailsPanel.ResumeLayout(False)
        Me.DefaultDetailsPanel.PerformLayout()
        CType(Me.DefaultBreakLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents EmployeePanel As Panel
    Friend WithEvents EmployeeTreeView As EmployeeTreeView
    Friend WithEvents ActionPanel As Panel
    Friend WithEvents CancelDialogButton As Button
    Friend WithEvents SaveButton As Button
    Friend WithEvents EmployeeDataGrid As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents EmployeeNumberColumn As DataGridViewTextBoxColumn
    Friend WithEvents LastNameColumn As DataGridViewTextBoxColumn
    Friend WithEvents FirstNameColumn As DataGridViewTextBoxColumn
    Friend WithEvents EmployeeTypeColumn As DataGridViewTextBoxColumn
    Friend WithEvents DeleteButton As Button
    Friend WithEvents DefaultDetailsPanel As Panel
    Friend WithEvents DefaultStartTimePicker As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents DefaultEndTimePicker As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents DefaultBreakTimePicker As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents DefaultBreakLengthNumeric As NumericUpDown
End Class
