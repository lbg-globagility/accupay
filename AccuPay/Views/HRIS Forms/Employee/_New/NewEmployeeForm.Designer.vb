<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NewEmployeeForm
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
        Me.EmployeeDataGridView = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.SearchTextBox = New System.Windows.Forms.TextBox()
        Me.RefreshButton = New System.Windows.Forms.Button()
        Me.ActiveCheckBox = New System.Windows.Forms.CheckBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.PersonalInfoTab = New AccuPay.PersonalInfoTab()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.SalaryTab21 = New AccuPay.SalaryTab2()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.EmployeeDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'EmployeeDataGridView
        '
        Me.EmployeeDataGridView.AllowUserToAddRows = False
        Me.EmployeeDataGridView.AllowUserToDeleteRows = False
        Me.EmployeeDataGridView.AllowUserToResizeColumns = False
        Me.EmployeeDataGridView.AllowUserToResizeRows = False
        Me.EmployeeDataGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EmployeeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.EmployeeDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3})
        Me.EmployeeDataGridView.Location = New System.Drawing.Point(8, 104)
        Me.EmployeeDataGridView.MultiSelect = False
        Me.EmployeeDataGridView.Name = "EmployeeDataGridView"
        Me.EmployeeDataGridView.ReadOnly = True
        Me.EmployeeDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.EmployeeDataGridView.Size = New System.Drawing.Size(344, 448)
        Me.EmployeeDataGridView.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "EmployeeNo"
        Me.Column1.HeaderText = "Employee No"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "LastName"
        Me.Column2.HeaderText = "Last Name"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "FirstName"
        Me.Column3.HeaderText = "First Name"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.SearchTextBox)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(344, 56)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'SearchTextBox
        '
        Me.SearchTextBox.Location = New System.Drawing.Point(8, 24)
        Me.SearchTextBox.Name = "SearchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(324, 20)
        Me.SearchTextBox.TabIndex = 0
        '
        'RefreshButton
        '
        Me.RefreshButton.Location = New System.Drawing.Point(280, 72)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Size = New System.Drawing.Size(75, 24)
        Me.RefreshButton.TabIndex = 2
        Me.RefreshButton.Text = "Refresh"
        Me.RefreshButton.UseVisualStyleBackColor = True
        '
        'ActiveCheckBox
        '
        Me.ActiveCheckBox.Checked = True
        Me.ActiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ActiveCheckBox.Location = New System.Drawing.Point(8, 72)
        Me.ActiveCheckBox.Name = "ActiveCheckBox"
        Me.ActiveCheckBox.Size = New System.Drawing.Size(80, 24)
        Me.ActiveCheckBox.TabIndex = 3
        Me.ActiveCheckBox.Text = "Active only"
        Me.ActiveCheckBox.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(360, 8)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.Padding = New System.Drawing.Point(16, 8)
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(832, 544)
        Me.TabControl1.TabIndex = 4
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.PersonalInfoTab)
        Me.TabPage1.Location = New System.Drawing.Point(4, 35)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(824, 505)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Personal"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'PersonalInfoTab
        '
        Me.PersonalInfoTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PersonalInfoTab.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PersonalInfoTab.Location = New System.Drawing.Point(3, 3)
        Me.PersonalInfoTab.Name = "PersonalInfoTab"
        Me.PersonalInfoTab.Size = New System.Drawing.Size(818, 499)
        Me.PersonalInfoTab.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.SalaryTab21)
        Me.TabPage2.Location = New System.Drawing.Point(4, 35)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(824, 505)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Salaries"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'SalaryTab21
        '
        Me.SalaryTab21.AllowanceSalary = New Decimal(New Integer() {0, 0, 0, 0})
        Me.SalaryTab21.BackColor = System.Drawing.Color.White
        Me.SalaryTab21.BasicSalary = New Decimal(New Integer() {0, 0, 0, 0})
        Me.SalaryTab21.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SalaryTab21.Location = New System.Drawing.Point(0, 0)
        Me.SalaryTab21.Name = "SalaryTab21"
        Me.SalaryTab21.PagIBIG = New Decimal(New Integer() {0, 0, 0, 0})
        Me.SalaryTab21.PhilHealth = Nothing
        Me.SalaryTab21.Size = New System.Drawing.Size(824, 505)
        Me.SalaryTab21.Sss = Nothing
        Me.SalaryTab21.TabIndex = 0
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "EmployeeNo"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee No"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "LastName"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "FirstName"
        Me.DataGridViewTextBoxColumn3.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        '
        'NewEmployeeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 560)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.ActiveCheckBox)
        Me.Controls.Add(Me.RefreshButton)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.EmployeeDataGridView)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "NewEmployeeForm"
        Me.Text = "NewEmployeeForm"
        CType(Me.EmployeeDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents EmployeeDataGridView As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents SearchTextBox As TextBox
    Friend WithEvents RefreshButton As Button
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents ActiveCheckBox As CheckBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents PersonalInfoTab As PersonalInfoTab
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents SalaryTab21 As SalaryTab2
End Class
