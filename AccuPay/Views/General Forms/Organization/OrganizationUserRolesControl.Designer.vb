<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OrganizationUserRolesControl
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
        Me.UserRoleGrid = New System.Windows.Forms.DataGridView()
        Me.Modules = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RoleColumn = New System.Windows.Forms.DataGridViewComboBoxColumn()
        CType(Me.UserRoleGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UserRoleGrid
        '
        Me.UserRoleGrid.AllowUserToAddRows = False
        Me.UserRoleGrid.AllowUserToDeleteRows = False
        Me.UserRoleGrid.AllowUserToResizeRows = False
        Me.UserRoleGrid.BackgroundColor = System.Drawing.Color.White
        Me.UserRoleGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.UserRoleGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Modules, Me.RoleColumn})
        Me.UserRoleGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserRoleGrid.Location = New System.Drawing.Point(0, 0)
        Me.UserRoleGrid.Name = "UserRoleGrid"
        Me.UserRoleGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.UserRoleGrid.Size = New System.Drawing.Size(745, 150)
        Me.UserRoleGrid.TabIndex = 382
        '
        'Modules
        '
        Me.Modules.DataPropertyName = "FullName"
        Me.Modules.HeaderText = "User"
        Me.Modules.Name = "Modules"
        Me.Modules.ReadOnly = True
        Me.Modules.Width = 450
        '
        'RoleColumn
        '
        Me.RoleColumn.DataPropertyName = "RoleId"
        Me.RoleColumn.HeaderText = "Role"
        Me.RoleColumn.Name = "RoleColumn"
        Me.RoleColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.RoleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.RoleColumn.Width = 250
        '
        'OrganizationUserRolesControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.UserRoleGrid)
        Me.Name = "OrganizationUserRolesControl"
        Me.Size = New System.Drawing.Size(745, 150)
        CType(Me.UserRoleGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents UserRoleGrid As DataGridView
    Friend WithEvents Modules As DataGridViewTextBoxColumn
    Friend WithEvents RoleColumn As DataGridViewComboBoxColumn
End Class
