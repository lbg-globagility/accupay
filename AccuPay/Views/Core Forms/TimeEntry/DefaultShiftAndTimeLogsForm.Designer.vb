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
        Me.CancelDialogButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.EmployeeDataGrid = New DevComponents.DotNetBar.Controls.DataGridViewX()
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
        Me.SuspendLayout()
        '
        'EmployeePanel
        '
        Me.EmployeePanel.Controls.Add(Me.EmployeeTreeView)
        Me.EmployeePanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.EmployeePanel.Location = New System.Drawing.Point(0, 0)
        Me.EmployeePanel.Name = "EmployeePanel"
        Me.EmployeePanel.Padding = New System.Windows.Forms.Padding(20, 10, 10, 0)
        Me.EmployeePanel.Size = New System.Drawing.Size(341, 761)
        Me.EmployeePanel.TabIndex = 0
        '
        'ActionPanel
        '
        Me.ActionPanel.Controls.Add(Me.CancelDialogButton)
        Me.ActionPanel.Controls.Add(Me.SaveButton)
        Me.ActionPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ActionPanel.Location = New System.Drawing.Point(341, 719)
        Me.ActionPanel.Name = "ActionPanel"
        Me.ActionPanel.Size = New System.Drawing.Size(667, 42)
        Me.ActionPanel.TabIndex = 1
        '
        'CancelDialogButton
        '
        Me.CancelDialogButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelDialogButton.Location = New System.Drawing.Point(559, 5)
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
        Me.SaveButton.Location = New System.Drawing.Point(454, 5)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(96, 32)
        Me.SaveButton.TabIndex = 20
        Me.SaveButton.Text = "&Save"
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
        Me.EmployeeDataGrid.Location = New System.Drawing.Point(341, 0)
        Me.EmployeeDataGrid.Name = "EmployeeDataGrid"
        Me.EmployeeDataGrid.ReadOnly = True
        Me.EmployeeDataGrid.Size = New System.Drawing.Size(667, 719)
        Me.EmployeeDataGrid.TabIndex = 2
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
        Me.EmployeeTreeView.OrganizationID = 0
        Me.EmployeeTreeView.Size = New System.Drawing.Size(311, 751)
        Me.EmployeeTreeView.TabIndex = 316
        '
        'DefaultShiftAndTimeLogsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 761)
        Me.Controls.Add(Me.EmployeeDataGrid)
        Me.Controls.Add(Me.ActionPanel)
        Me.Controls.Add(Me.EmployeePanel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "DefaultShiftAndTimeLogsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Default Shift and Time Logs Form"
        Me.EmployeePanel.ResumeLayout(False)
        Me.ActionPanel.ResumeLayout(False)
        CType(Me.EmployeeDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
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
End Class
