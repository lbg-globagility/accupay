<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UserUserControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.UserRoleGrid = New System.Windows.Forms.DataGridView()
        Me.UserPrivilegeLabel = New System.Windows.Forms.Label()
        Me.DetailsGroup = New System.Windows.Forms.GroupBox()
        Me.UserLevelComboBox = New System.Windows.Forms.ComboBox()
        Me.UserLevelLabel = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.EmailTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ConfirmPasswordTextBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.PasswordTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.UserNameTextBox = New System.Windows.Forms.TextBox()
        Me.lblid = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.FirstNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LastNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Modules = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RoleColumn = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.UserRoleGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DetailsGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.UserRoleGrid)
        Me.Panel1.Controls.Add(Me.UserPrivilegeLabel)
        Me.Panel1.Controls.Add(Me.DetailsGroup)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(787, 573)
        Me.Panel1.TabIndex = 163
        '
        'UserRoleGrid
        '
        Me.UserRoleGrid.AllowUserToAddRows = False
        Me.UserRoleGrid.AllowUserToDeleteRows = False
        Me.UserRoleGrid.AllowUserToResizeRows = False
        Me.UserRoleGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.UserRoleGrid.BackgroundColor = System.Drawing.Color.White
        Me.UserRoleGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.UserRoleGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Modules, Me.RoleColumn})
        Me.UserRoleGrid.Location = New System.Drawing.Point(8, 233)
        Me.UserRoleGrid.Name = "UserRoleGrid"
        Me.UserRoleGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.UserRoleGrid.Size = New System.Drawing.Size(769, 321)
        Me.UserRoleGrid.TabIndex = 160
        '
        'UserPrivilegeLabel
        '
        Me.UserPrivilegeLabel.AutoSize = True
        Me.UserPrivilegeLabel.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.UserPrivilegeLabel.Location = New System.Drawing.Point(8, 206)
        Me.UserPrivilegeLabel.Name = "UserPrivilegeLabel"
        Me.UserPrivilegeLabel.Size = New System.Drawing.Size(145, 24)
        Me.UserPrivilegeLabel.TabIndex = 161
        Me.UserPrivilegeLabel.Text = "USER ROLES"
        '
        'DetailsGroup
        '
        Me.DetailsGroup.BackColor = System.Drawing.Color.White
        Me.DetailsGroup.Controls.Add(Me.UserLevelComboBox)
        Me.DetailsGroup.Controls.Add(Me.UserLevelLabel)
        Me.DetailsGroup.Controls.Add(Me.Label9)
        Me.DetailsGroup.Controls.Add(Me.EmailTextBox)
        Me.DetailsGroup.Controls.Add(Me.Label4)
        Me.DetailsGroup.Controls.Add(Me.ConfirmPasswordTextBox)
        Me.DetailsGroup.Controls.Add(Me.Label5)
        Me.DetailsGroup.Controls.Add(Me.PasswordTextBox)
        Me.DetailsGroup.Controls.Add(Me.Label6)
        Me.DetailsGroup.Controls.Add(Me.UserNameTextBox)
        Me.DetailsGroup.Controls.Add(Me.lblid)
        Me.DetailsGroup.Controls.Add(Me.Label2)
        Me.DetailsGroup.Controls.Add(Me.FirstNameTextBox)
        Me.DetailsGroup.Controls.Add(Me.Label1)
        Me.DetailsGroup.Controls.Add(Me.LastNameTextBox)
        Me.DetailsGroup.Controls.Add(Me.Label17)
        Me.DetailsGroup.Controls.Add(Me.Label16)
        Me.DetailsGroup.Controls.Add(Me.Label15)
        Me.DetailsGroup.Controls.Add(Me.Label13)
        Me.DetailsGroup.Controls.Add(Me.Label12)
        Me.DetailsGroup.Location = New System.Drawing.Point(8, 18)
        Me.DetailsGroup.Name = "DetailsGroup"
        Me.DetailsGroup.Size = New System.Drawing.Size(769, 164)
        Me.DetailsGroup.TabIndex = 159
        Me.DetailsGroup.TabStop = False
        Me.DetailsGroup.Text = "Users Details"
        '
        'UserLevelComboBox
        '
        Me.UserLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.UserLevelComboBox.FormattingEnabled = True
        Me.UserLevelComboBox.Location = New System.Drawing.Point(13, 125)
        Me.UserLevelComboBox.Name = "UserLevelComboBox"
        Me.UserLevelComboBox.Size = New System.Drawing.Size(210, 21)
        Me.UserLevelComboBox.TabIndex = 44
        '
        'UserLevelLabel
        '
        Me.UserLevelLabel.AutoSize = True
        Me.UserLevelLabel.Location = New System.Drawing.Point(10, 109)
        Me.UserLevelLabel.Name = "UserLevelLabel"
        Me.UserLevelLabel.Size = New System.Drawing.Size(58, 13)
        Me.UserLevelLabel.TabIndex = 43
        Me.UserLevelLabel.Text = "User Level"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(253, 22)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 13)
        Me.Label9.TabIndex = 34
        Me.Label9.Text = "Email Address"
        '
        'EmailTextBox
        '
        Me.EmailTextBox.Location = New System.Drawing.Point(256, 38)
        Me.EmailTextBox.Name = "EmailTextBox"
        Me.EmailTextBox.Size = New System.Drawing.Size(210, 20)
        Me.EmailTextBox.TabIndex = 20
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(528, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 13)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "Confirm Password"
        '
        'ConfirmPasswordTextBox
        '
        Me.ConfirmPasswordTextBox.Location = New System.Drawing.Point(531, 82)
        Me.ConfirmPasswordTextBox.Name = "ConfirmPasswordTextBox"
        Me.ConfirmPasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.ConfirmPasswordTextBox.Size = New System.Drawing.Size(210, 20)
        Me.ConfirmPasswordTextBox.TabIndex = 24
        Me.ConfirmPasswordTextBox.Tag = "Required"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(528, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 13)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "Password"
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Location = New System.Drawing.Point(531, 38)
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.PasswordTextBox.Size = New System.Drawing.Size(210, 20)
        Me.PasswordTextBox.TabIndex = 23
        Me.PasswordTextBox.Tag = "Required"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(253, 66)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(55, 13)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Username"
        '
        'UserNameTextBox
        '
        Me.UserNameTextBox.Location = New System.Drawing.Point(256, 82)
        Me.UserNameTextBox.Name = "UserNameTextBox"
        Me.UserNameTextBox.Size = New System.Drawing.Size(210, 20)
        Me.UserNameTextBox.TabIndex = 22
        Me.UserNameTextBox.Tag = "Required"
        '
        'lblid
        '
        Me.lblid.AutoSize = True
        Me.lblid.Location = New System.Drawing.Point(13, 38)
        Me.lblid.Name = "lblid"
        Me.lblid.Size = New System.Drawing.Size(0, 13)
        Me.lblid.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "First Name"
        '
        'FirstNameTextBox
        '
        Me.FirstNameTextBox.Location = New System.Drawing.Point(13, 82)
        Me.FirstNameTextBox.Name = "FirstNameTextBox"
        Me.FirstNameTextBox.Size = New System.Drawing.Size(210, 20)
        Me.FirstNameTextBox.TabIndex = 18
        Me.FirstNameTextBox.Tag = "Required"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Last Name"
        '
        'LastNameTextBox
        '
        Me.LastNameTextBox.Location = New System.Drawing.Point(13, 38)
        Me.LastNameTextBox.Name = "LastNameTextBox"
        Me.LastNameTextBox.Size = New System.Drawing.Size(210, 20)
        Me.LastNameTextBox.TabIndex = 17
        Me.LastNameTextBox.Tag = "Required"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.Red
        Me.Label17.Location = New System.Drawing.Point(624, 59)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(17, 21)
        Me.Label17.TabIndex = 40
        Me.Label17.Text = "*"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.Red
        Me.Label16.Location = New System.Drawing.Point(580, 16)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(17, 21)
        Me.Label16.TabIndex = 39
        Me.Label16.Text = "*"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.Red
        Me.Label15.Location = New System.Drawing.Point(307, 62)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(17, 21)
        Me.Label15.TabIndex = 38
        Me.Label15.Text = "*"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Red
        Me.Label13.Location = New System.Drawing.Point(67, 60)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(17, 21)
        Me.Label13.TabIndex = 36
        Me.Label13.Text = "*"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Red
        Me.Label12.Location = New System.Drawing.Point(65, 18)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(17, 21)
        Me.Label12.TabIndex = 35
        Me.Label12.Text = "*"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = ""
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 150
        '
        'Modules
        '
        Me.Modules.DataPropertyName = "OrganizationName"
        Me.Modules.HeaderText = "Organization"
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
        'UserUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UserUserControl"
        Me.Size = New System.Drawing.Size(787, 573)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.UserRoleGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DetailsGroup.ResumeLayout(False)
        Me.DetailsGroup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents UserRoleGrid As DataGridView
    Friend WithEvents UserPrivilegeLabel As Label
    Friend WithEvents DetailsGroup As GroupBox
    Friend WithEvents UserLevelComboBox As ComboBox
    Friend WithEvents UserLevelLabel As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents EmailTextBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ConfirmPasswordTextBox As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents PasswordTextBox As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents UserNameTextBox As TextBox
    Friend WithEvents lblid As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents FirstNameTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents LastNameTextBox As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents Modules As DataGridViewTextBoxColumn
    Friend WithEvents RoleColumn As DataGridViewComboBoxColumn
End Class
