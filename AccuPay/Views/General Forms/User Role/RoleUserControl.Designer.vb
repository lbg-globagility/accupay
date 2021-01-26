<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RoleUserControl
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.RoleNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.RolePermissionGrid = New System.Windows.Forms.DataGridView()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReadColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.CreateColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.UpdateColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DeleteColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.AllReadCheckBox = New System.Windows.Forms.CheckBox()
        Me.AllCreateCheckBox = New System.Windows.Forms.CheckBox()
        Me.AllDeleteCheckBox = New System.Windows.Forms.CheckBox()
        Me.AllUpdateCheckBox = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.RolePermissionGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.RolePermissionGrid)
        Me.Panel1.Controls.Add(Me.AllReadCheckBox)
        Me.Panel1.Controls.Add(Me.AllCreateCheckBox)
        Me.Panel1.Controls.Add(Me.AllDeleteCheckBox)
        Me.Panel1.Controls.Add(Me.AllUpdateCheckBox)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(908, 626)
        Me.Panel1.TabIndex = 7
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.RoleNameTextBox)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Location = New System.Drawing.Point(13, 29)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(806, 30)
        Me.Panel3.TabIndex = 372
        '
        'RoleNameTextBox
        '
        Me.RoleNameTextBox.Location = New System.Drawing.Point(83, 4)
        Me.RoleNameTextBox.Name = "RoleNameTextBox"
        Me.RoleNameTextBox.Size = New System.Drawing.Size(235, 20)
        Me.RoleNameTextBox.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Role Name:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(10, 70)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(131, 15)
        Me.Label4.TabIndex = 371
        Me.Label4.Text = "List of Role Permissions"
        '
        'RolePermissionGrid
        '
        Me.RolePermissionGrid.AllowUserToAddRows = False
        Me.RolePermissionGrid.AllowUserToDeleteRows = False
        Me.RolePermissionGrid.BackgroundColor = System.Drawing.Color.White
        Me.RolePermissionGrid.ColumnHeadersHeight = 38
        Me.RolePermissionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.RolePermissionGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column10, Me.ReadColumn, Me.CreateColumn, Me.UpdateColumn, Me.DeleteColumn})
        Me.RolePermissionGrid.Location = New System.Drawing.Point(13, 88)
        Me.RolePermissionGrid.MultiSelect = False
        Me.RolePermissionGrid.Name = "RolePermissionGrid"
        Me.RolePermissionGrid.Size = New System.Drawing.Size(806, 510)
        Me.RolePermissionGrid.TabIndex = 0
        '
        'Column10
        '
        Me.Column10.DataPropertyName = "DisplayName"
        Me.Column10.HeaderText = "Permission"
        Me.Column10.Name = "Column10"
        Me.Column10.Width = 250
        '
        'ReadColumn
        '
        Me.ReadColumn.DataPropertyName = "Read"
        Me.ReadColumn.HeaderText = "Read"
        Me.ReadColumn.Name = "ReadColumn"
        Me.ReadColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ReadColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.ReadColumn.Width = 127
        '
        'CreateColumn
        '
        Me.CreateColumn.DataPropertyName = "Create"
        Me.CreateColumn.HeaderText = "Create"
        Me.CreateColumn.Name = "CreateColumn"
        Me.CreateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CreateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.CreateColumn.Width = 127
        '
        'UpdateColumn
        '
        Me.UpdateColumn.DataPropertyName = "Update"
        Me.UpdateColumn.HeaderText = "Update"
        Me.UpdateColumn.Name = "UpdateColumn"
        Me.UpdateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.UpdateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.UpdateColumn.Width = 128
        '
        'DeleteColumn
        '
        Me.DeleteColumn.DataPropertyName = "Delete"
        Me.DeleteColumn.HeaderText = "Delete"
        Me.DeleteColumn.Name = "DeleteColumn"
        Me.DeleteColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DeleteColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DeleteColumn.Width = 127
        '
        'AllReadCheckBox
        '
        Me.AllReadCheckBox.AutoSize = True
        Me.AllReadCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AllReadCheckBox.Location = New System.Drawing.Point(352, 65)
        Me.AllReadCheckBox.Name = "AllReadCheckBox"
        Me.AllReadCheckBox.Size = New System.Drawing.Size(71, 17)
        Me.AllReadCheckBox.TabIndex = 5
        Me.AllReadCheckBox.Text = "All Reads"
        Me.AllReadCheckBox.UseVisualStyleBackColor = True
        '
        'AllCreateCheckBox
        '
        Me.AllCreateCheckBox.AutoSize = True
        Me.AllCreateCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AllCreateCheckBox.Location = New System.Drawing.Point(476, 65)
        Me.AllCreateCheckBox.Name = "AllCreateCheckBox"
        Me.AllCreateCheckBox.Size = New System.Drawing.Size(76, 17)
        Me.AllCreateCheckBox.TabIndex = 2
        Me.AllCreateCheckBox.Text = "All Creates"
        Me.AllCreateCheckBox.UseVisualStyleBackColor = True
        '
        'AllDeleteCheckBox
        '
        Me.AllDeleteCheckBox.AutoSize = True
        Me.AllDeleteCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AllDeleteCheckBox.Location = New System.Drawing.Point(719, 65)
        Me.AllDeleteCheckBox.Name = "AllDeleteCheckBox"
        Me.AllDeleteCheckBox.Size = New System.Drawing.Size(76, 17)
        Me.AllDeleteCheckBox.TabIndex = 4
        Me.AllDeleteCheckBox.Text = "All Deletes"
        Me.AllDeleteCheckBox.UseVisualStyleBackColor = True
        '
        'AllUpdateCheckBox
        '
        Me.AllUpdateCheckBox.AutoSize = True
        Me.AllUpdateCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AllUpdateCheckBox.Location = New System.Drawing.Point(606, 65)
        Me.AllUpdateCheckBox.Name = "AllUpdateCheckBox"
        Me.AllUpdateCheckBox.Size = New System.Drawing.Size(80, 17)
        Me.AllUpdateCheckBox.TabIndex = 3
        Me.AllUpdateCheckBox.Text = "All Updates"
        Me.AllUpdateCheckBox.UseVisualStyleBackColor = True
        '
        'RoleUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Name = "RoleUserControl"
        Me.Size = New System.Drawing.Size(908, 626)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.RolePermissionGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents RoleNameTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents RolePermissionGrid As DataGridView
    Friend WithEvents Column10 As DataGridViewTextBoxColumn
    Friend WithEvents ReadColumn As DataGridViewCheckBoxColumn
    Friend WithEvents CreateColumn As DataGridViewCheckBoxColumn
    Friend WithEvents UpdateColumn As DataGridViewCheckBoxColumn
    Friend WithEvents DeleteColumn As DataGridViewCheckBoxColumn
    Friend WithEvents AllReadCheckBox As CheckBox
    Friend WithEvents AllCreateCheckBox As CheckBox
    Friend WithEvents AllDeleteCheckBox As CheckBox
    Friend WithEvents AllUpdateCheckBox As CheckBox
End Class
