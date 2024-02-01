<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LeaveResetForm
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
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvEmployees = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eRowId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eLastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eFirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eStartDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eDateRegularized = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eVacationLeaveAllowance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eVacationLeaveBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eSickLeaveAllowance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eSickLeaveBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eOthersLeaveAllowance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eOthersLeaveBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eParentalLeaveAllowance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eParentalLeaveBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvEmployees
        '
        Me.dgvEmployees.AllowUserToAddRows = False
        Me.dgvEmployees.AllowUserToDeleteRows = False
        Me.dgvEmployees.AllowUserToOrderColumns = True
        Me.dgvEmployees.AllowUserToResizeRows = False
        Me.dgvEmployees.BackgroundColor = System.Drawing.Color.White
        Me.dgvEmployees.ColumnHeadersHeight = 34
        Me.dgvEmployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvEmployees.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eRowId, Me.eId, Me.eLastName, Me.eFirstName, Me.eStartDate, Me.eDateRegularized, Me.eVacationLeaveAllowance, Me.eVacationLeaveBalance, Me.eSickLeaveAllowance, Me.eSickLeaveBalance, Me.eOthersLeaveAllowance, Me.eOthersLeaveBalance, Me.eParentalLeaveAllowance, Me.eParentalLeaveBalance})
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEmployees.DefaultCellStyle = DataGridViewCellStyle10
        Me.dgvEmployees.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEmployees.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEmployees.Location = New System.Drawing.Point(0, 0)
        Me.dgvEmployees.MultiSelect = False
        Me.dgvEmployees.Name = "dgvEmployees"
        Me.dgvEmployees.ReadOnly = True
        Me.dgvEmployees.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvEmployees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmployees.Size = New System.Drawing.Size(800, 415)
        Me.dgvEmployees.TabIndex = 141
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 415)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(800, 35)
        Me.Panel1.TabIndex = 142
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.LinkLabel1)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel3.Location = New System.Drawing.Point(297, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(332, 35)
        Me.Panel3.TabIndex = 3
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.SystemColors.ControlText
        Me.LinkLabel1.Location = New System.Drawing.Point(0, 0)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(332, 35)
        Me.LinkLabel1.TabIndex = 2
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "LinkLabel1"
        Me.LinkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.LinkLabel1.VisitedLinkColor = System.Drawing.SystemColors.ControlText
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnReset)
        Me.Panel2.Controls.Add(Me.btnClose)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel2.Location = New System.Drawing.Point(629, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(171, 35)
        Me.Panel2.TabIndex = 1
        '
        'btnReset
        '
        Me.btnReset.Location = New System.Drawing.Point(3, 6)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(75, 23)
        Me.btnReset.TabIndex = 0
        Me.btnReset.Text = "&Reset"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(84, 6)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "&Cancel"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "RowID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "EmployeeNo"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Employee ID"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 103
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "LastName"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 103
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "FirstName"
        Me.DataGridViewTextBoxColumn4.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 103
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "StartDate"
        Me.DataGridViewTextBoxColumn5.HeaderText = "StartDate"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "VacationLeaveAllowance"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Vacation Leave Allowance"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "VacationLeaveBalance"
        Me.DataGridViewTextBoxColumn7.HeaderText = "Vacation Leave Balance"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "SickLeaveAllowance"
        Me.DataGridViewTextBoxColumn8.HeaderText = "Sick Leave Allowance"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "SickLeaveBalance"
        Me.DataGridViewTextBoxColumn9.HeaderText = "Sick Leave Balance"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "OthersLeaveAllowance"
        Me.DataGridViewTextBoxColumn10.HeaderText = "Other sLeave Allowance"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.DataPropertyName = "OthersLeaveBalance"
        Me.DataGridViewTextBoxColumn11.HeaderText = "Others Leave Balance"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.DataPropertyName = "ParentalLeaveAllowance"
        Me.DataGridViewTextBoxColumn12.HeaderText = "Parental Leave Allowance"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.DataPropertyName = "ParentalLeaveBalance"
        Me.DataGridViewTextBoxColumn13.HeaderText = "Parental Leave Balance"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        '
        'eRowId
        '
        Me.eRowId.DataPropertyName = "RowID"
        Me.eRowId.HeaderText = "RowID"
        Me.eRowId.Name = "eRowId"
        Me.eRowId.ReadOnly = True
        Me.eRowId.Visible = False
        '
        'eId
        '
        Me.eId.DataPropertyName = "EmployeeNo"
        Me.eId.HeaderText = "Employee ID"
        Me.eId.Name = "eId"
        Me.eId.ReadOnly = True
        Me.eId.Width = 103
        '
        'eLastName
        '
        Me.eLastName.DataPropertyName = "LastName"
        Me.eLastName.HeaderText = "Last Name"
        Me.eLastName.Name = "eLastName"
        Me.eLastName.ReadOnly = True
        Me.eLastName.Width = 103
        '
        'eFirstName
        '
        Me.eFirstName.DataPropertyName = "FirstName"
        Me.eFirstName.HeaderText = "First Name"
        Me.eFirstName.Name = "eFirstName"
        Me.eFirstName.ReadOnly = True
        Me.eFirstName.Width = 103
        '
        'eStartDate
        '
        Me.eStartDate.DataPropertyName = "StartDate"
        Me.eStartDate.HeaderText = "Start Date"
        Me.eStartDate.Name = "eStartDate"
        Me.eStartDate.ReadOnly = True
        '
        'eDateRegularized
        '
        Me.eDateRegularized.DataPropertyName = "DateRegularized"
        DataGridViewCellStyle1.Format = "d"
        DataGridViewCellStyle1.NullValue = "—"
        Me.eDateRegularized.DefaultCellStyle = DataGridViewCellStyle1
        Me.eDateRegularized.HeaderText = "Date Regularized"
        Me.eDateRegularized.Name = "eDateRegularized"
        Me.eDateRegularized.ReadOnly = True
        '
        'eVacationLeaveAllowance
        '
        Me.eVacationLeaveAllowance.DataPropertyName = "VacationLeaveAllowance"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.eVacationLeaveAllowance.DefaultCellStyle = DataGridViewCellStyle2
        Me.eVacationLeaveAllowance.HeaderText = "Vacation Leave Allowance"
        Me.eVacationLeaveAllowance.Name = "eVacationLeaveAllowance"
        Me.eVacationLeaveAllowance.ReadOnly = True
        '
        'eVacationLeaveBalance
        '
        Me.eVacationLeaveBalance.DataPropertyName = "VacationLeaveBalance"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle3.Format = "N2"
        Me.eVacationLeaveBalance.DefaultCellStyle = DataGridViewCellStyle3
        Me.eVacationLeaveBalance.HeaderText = "Vacation Leave Balance"
        Me.eVacationLeaveBalance.Name = "eVacationLeaveBalance"
        Me.eVacationLeaveBalance.ReadOnly = True
        '
        'eSickLeaveAllowance
        '
        Me.eSickLeaveAllowance.DataPropertyName = "SickLeaveAllowance"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle4.Format = "N2"
        Me.eSickLeaveAllowance.DefaultCellStyle = DataGridViewCellStyle4
        Me.eSickLeaveAllowance.HeaderText = "Sick Leave Allowance"
        Me.eSickLeaveAllowance.Name = "eSickLeaveAllowance"
        Me.eSickLeaveAllowance.ReadOnly = True
        '
        'eSickLeaveBalance
        '
        Me.eSickLeaveBalance.DataPropertyName = "SickLeaveBalance"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle5.Format = "N2"
        Me.eSickLeaveBalance.DefaultCellStyle = DataGridViewCellStyle5
        Me.eSickLeaveBalance.HeaderText = "Sick Leave Balance"
        Me.eSickLeaveBalance.Name = "eSickLeaveBalance"
        Me.eSickLeaveBalance.ReadOnly = True
        '
        'eOthersLeaveAllowance
        '
        Me.eOthersLeaveAllowance.DataPropertyName = "OthersLeaveAllowance"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle6.Format = "N2"
        Me.eOthersLeaveAllowance.DefaultCellStyle = DataGridViewCellStyle6
        Me.eOthersLeaveAllowance.HeaderText = "Other sLeave Allowance"
        Me.eOthersLeaveAllowance.Name = "eOthersLeaveAllowance"
        Me.eOthersLeaveAllowance.ReadOnly = True
        '
        'eOthersLeaveBalance
        '
        Me.eOthersLeaveBalance.DataPropertyName = "OthersLeaveBalance"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle7.Format = "N2"
        Me.eOthersLeaveBalance.DefaultCellStyle = DataGridViewCellStyle7
        Me.eOthersLeaveBalance.HeaderText = "Others Leave Balance"
        Me.eOthersLeaveBalance.Name = "eOthersLeaveBalance"
        Me.eOthersLeaveBalance.ReadOnly = True
        '
        'eParentalLeaveAllowance
        '
        Me.eParentalLeaveAllowance.DataPropertyName = "ParentalLeaveAllowance"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle8.Format = "N2"
        Me.eParentalLeaveAllowance.DefaultCellStyle = DataGridViewCellStyle8
        Me.eParentalLeaveAllowance.HeaderText = "Parental Leave Allowance"
        Me.eParentalLeaveAllowance.Name = "eParentalLeaveAllowance"
        Me.eParentalLeaveAllowance.ReadOnly = True
        '
        'eParentalLeaveBalance
        '
        Me.eParentalLeaveBalance.DataPropertyName = "ParentalLeaveBalance"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle9.Format = "N2"
        Me.eParentalLeaveBalance.DefaultCellStyle = DataGridViewCellStyle9
        Me.eParentalLeaveBalance.HeaderText = "Parental Leave Balance"
        Me.eParentalLeaveBalance.Name = "eParentalLeaveBalance"
        Me.eParentalLeaveBalance.ReadOnly = True
        '
        'LeaveResetForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.dgvEmployees)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LeaveResetForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvEmployees, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvEmployees As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnClose As Button
    Friend WithEvents btnReset As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As DataGridViewTextBoxColumn
    Friend WithEvents eRowId As DataGridViewTextBoxColumn
    Friend WithEvents eId As DataGridViewTextBoxColumn
    Friend WithEvents eLastName As DataGridViewTextBoxColumn
    Friend WithEvents eFirstName As DataGridViewTextBoxColumn
    Friend WithEvents eStartDate As DataGridViewTextBoxColumn
    Friend WithEvents eDateRegularized As DataGridViewTextBoxColumn
    Friend WithEvents eVacationLeaveAllowance As DataGridViewTextBoxColumn
    Friend WithEvents eVacationLeaveBalance As DataGridViewTextBoxColumn
    Friend WithEvents eSickLeaveAllowance As DataGridViewTextBoxColumn
    Friend WithEvents eSickLeaveBalance As DataGridViewTextBoxColumn
    Friend WithEvents eOthersLeaveAllowance As DataGridViewTextBoxColumn
    Friend WithEvents eOthersLeaveBalance As DataGridViewTextBoxColumn
    Friend WithEvents eParentalLeaveAllowance As DataGridViewTextBoxColumn
    Friend WithEvents eParentalLeaveBalance As DataGridViewTextBoxColumn
End Class
