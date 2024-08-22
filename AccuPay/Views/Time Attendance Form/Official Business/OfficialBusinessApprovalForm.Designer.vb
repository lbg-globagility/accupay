<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OfficialBusinessApprovalForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OfficialBusinessApprovalForm))
        Me.EmployeeSearchTextbox = New System.Windows.Forms.TextBox()
        Me.Leave = New System.Windows.Forms.Label()
        Me.OBDataGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Checked = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LeaveType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StartDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EndDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Reason = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Comment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ApproveSelectedBtn = New System.Windows.Forms.Button()
        Me.OBSelectAllCheckbox = New System.Windows.Forms.CheckBox()
        Me.OBRefreshBtn = New System.Windows.Forms.Button()
        Me.DataGridViewCheckBoxColumn1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.OBDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'EmployeeSearchTextbox
        '
        Me.EmployeeSearchTextbox.Location = New System.Drawing.Point(13, 28)
        Me.EmployeeSearchTextbox.Name = "EmployeeSearchTextbox"
        Me.EmployeeSearchTextbox.Size = New System.Drawing.Size(624, 20)
        Me.EmployeeSearchTextbox.TabIndex = 1
        '
        'Leave
        '
        Me.Leave.AutoSize = True
        Me.Leave.Location = New System.Drawing.Point(12, 8)
        Me.Leave.Name = "Leave"
        Me.Leave.Size = New System.Drawing.Size(121, 13)
        Me.Leave.TabIndex = 2
        Me.Leave.Text = "Search Employee Name"
        '
        'OBDataGridView
        '
        Me.OBDataGridView.AllowUserToAddRows = False
        Me.OBDataGridView.AllowUserToOrderColumns = True
        Me.OBDataGridView.AllowUserToResizeRows = False
        Me.OBDataGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.OBDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.OBDataGridView.BackgroundColor = System.Drawing.Color.White
        Me.OBDataGridView.ColumnHeadersHeight = 34
        Me.OBDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.OBDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Checked, Me.EmployeeID, Me.EmployeeName, Me.LeaveType, Me.StartDate, Me.EndDate, Me.Reason, Me.Comment, Me.Status, Me.RowID})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.OBDataGridView.DefaultCellStyle = DataGridViewCellStyle1
        Me.OBDataGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.OBDataGridView.Location = New System.Drawing.Point(13, 56)
        Me.OBDataGridView.MultiSelect = False
        Me.OBDataGridView.Name = "OBDataGridView"
        Me.OBDataGridView.ReadOnly = True
        Me.OBDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.OBDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.OBDataGridView.Size = New System.Drawing.Size(800, 326)
        Me.OBDataGridView.TabIndex = 4
        '
        'Checked
        '
        Me.Checked.DataPropertyName = "Checked"
        Me.Checked.FillWeight = 45.68528!
        Me.Checked.HeaderText = ""
        Me.Checked.Name = "Checked"
        Me.Checked.ReadOnly = True
        Me.Checked.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Checked.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'EmployeeID
        '
        Me.EmployeeID.DataPropertyName = "EmployeeID"
        Me.EmployeeID.FillWeight = 106.7893!
        Me.EmployeeID.HeaderText = "Employee ID"
        Me.EmployeeID.Name = "EmployeeID"
        Me.EmployeeID.ReadOnly = True
        '
        'EmployeeName
        '
        Me.EmployeeName.DataPropertyName = "EmployeeName"
        Me.EmployeeName.FillWeight = 106.7893!
        Me.EmployeeName.HeaderText = "Employee Name"
        Me.EmployeeName.Name = "EmployeeName"
        Me.EmployeeName.ReadOnly = True
        '
        'LeaveType
        '
        Me.LeaveType.DataPropertyName = "LeaveType"
        Me.LeaveType.FillWeight = 106.7893!
        Me.LeaveType.HeaderText = "LeaveType"
        Me.LeaveType.Name = "LeaveType"
        Me.LeaveType.ReadOnly = True
        '
        'StartDate
        '
        Me.StartDate.DataPropertyName = "StartDate"
        Me.StartDate.FillWeight = 106.7893!
        Me.StartDate.HeaderText = "Start Date"
        Me.StartDate.Name = "StartDate"
        Me.StartDate.ReadOnly = True
        '
        'EndDate
        '
        Me.EndDate.DataPropertyName = "EndDate"
        Me.EndDate.FillWeight = 106.7893!
        Me.EndDate.HeaderText = "End Date"
        Me.EndDate.Name = "EndDate"
        Me.EndDate.ReadOnly = True
        '
        'Reason
        '
        Me.Reason.DataPropertyName = "Reason"
        Me.Reason.FillWeight = 106.7893!
        Me.Reason.HeaderText = "Reason"
        Me.Reason.Name = "Reason"
        Me.Reason.ReadOnly = True
        '
        'Comment
        '
        Me.Comment.DataPropertyName = "Comment"
        Me.Comment.FillWeight = 106.7893!
        Me.Comment.HeaderText = "Comment"
        Me.Comment.Name = "Comment"
        Me.Comment.ReadOnly = True
        '
        'Status
        '
        Me.Status.DataPropertyName = "Status"
        Me.Status.FillWeight = 106.7893!
        Me.Status.HeaderText = "Status"
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        '
        'RowID
        '
        Me.RowID.DataPropertyName = "RowID"
        Me.RowID.HeaderText = "RowID"
        Me.RowID.Name = "RowID"
        Me.RowID.ReadOnly = True
        Me.RowID.Visible = False
        '
        'ApproveSelectedBtn
        '
        Me.ApproveSelectedBtn.Location = New System.Drawing.Point(701, 394)
        Me.ApproveSelectedBtn.Name = "ApproveSelectedBtn"
        Me.ApproveSelectedBtn.Size = New System.Drawing.Size(112, 23)
        Me.ApproveSelectedBtn.TabIndex = 5
        Me.ApproveSelectedBtn.Text = "Approve Selected"
        Me.ApproveSelectedBtn.UseVisualStyleBackColor = True
        '
        'OBSelectAllCheckbox
        '
        Me.OBSelectAllCheckbox.AutoSize = True
        Me.OBSelectAllCheckbox.Location = New System.Drawing.Point(656, 30)
        Me.OBSelectAllCheckbox.Name = "OBSelectAllCheckbox"
        Me.OBSelectAllCheckbox.Size = New System.Drawing.Size(70, 17)
        Me.OBSelectAllCheckbox.TabIndex = 6
        Me.OBSelectAllCheckbox.Text = "Select All"
        Me.OBSelectAllCheckbox.UseVisualStyleBackColor = True
        '
        'OBRefreshBtn
        '
        Me.OBRefreshBtn.Location = New System.Drawing.Point(738, 27)
        Me.OBRefreshBtn.Name = "OBRefreshBtn"
        Me.OBRefreshBtn.Size = New System.Drawing.Size(75, 23)
        Me.OBRefreshBtn.TabIndex = 7
        Me.OBRefreshBtn.Text = "Refresh"
        Me.OBRefreshBtn.UseVisualStyleBackColor = True
        '
        'DataGridViewCheckBoxColumn1
        '
        Me.DataGridViewCheckBoxColumn1.DataPropertyName = "Checked"
        Me.DataGridViewCheckBoxColumn1.FillWeight = 45.68528!
        Me.DataGridViewCheckBoxColumn1.HeaderText = ""
        Me.DataGridViewCheckBoxColumn1.Name = "DataGridViewCheckBoxColumn1"
        Me.DataGridViewCheckBoxColumn1.ReadOnly = True
        Me.DataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxColumn1.Width = 189
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "EmployeeID"
        Me.DataGridViewTextBoxColumn1.FillWeight = 106.7893!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee ID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 190
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "EmployeeName"
        Me.DataGridViewTextBoxColumn2.FillWeight = 106.7893!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Employee Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 189
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "StartDate"
        Me.DataGridViewTextBoxColumn3.FillWeight = 106.7893!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Start Date"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 189
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "StartDate"
        Me.DataGridViewTextBoxColumn4.FillWeight = 106.7893!
        Me.DataGridViewTextBoxColumn4.HeaderText = "Start Date"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 90
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "EndDate"
        Me.DataGridViewTextBoxColumn5.FillWeight = 106.7893!
        Me.DataGridViewTextBoxColumn5.HeaderText = "End Date"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.Width = 90
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "Reason"
        Me.DataGridViewTextBoxColumn6.FillWeight = 106.7893!
        Me.DataGridViewTextBoxColumn6.HeaderText = "Reason"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.Width = 89
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "Comment"
        Me.DataGridViewTextBoxColumn7.FillWeight = 106.7893!
        Me.DataGridViewTextBoxColumn7.HeaderText = "Comment"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.Width = 90
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "Status"
        Me.DataGridViewTextBoxColumn8.FillWeight = 106.7893!
        Me.DataGridViewTextBoxColumn8.HeaderText = "Status"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.Width = 90
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "RowID"
        Me.DataGridViewTextBoxColumn9.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.Visible = False
        '
        'OfficialBusinessApprovalForm
        '
        Me.ClientSize = New System.Drawing.Size(825, 429)
        Me.Controls.Add(Me.OBRefreshBtn)
        Me.Controls.Add(Me.OBSelectAllCheckbox)
        Me.Controls.Add(Me.ApproveSelectedBtn)
        Me.Controls.Add(Me.OBDataGridView)
        Me.Controls.Add(Me.Leave)
        Me.Controls.Add(Me.EmployeeSearchTextbox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "OfficialBusinessApprovalForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Official Business Approval"
        CType(Me.OBDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents EmployeeSearchTextbox As TextBox
    Friend WithEvents Leave As Label
    Friend WithEvents OBDataGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents ApproveSelectedBtn As Button
    Friend WithEvents DataGridViewCheckBoxColumn1 As DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents OBSelectAllCheckbox As CheckBox
    Friend WithEvents OBRefreshBtn As Button
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents Checked As DataGridViewCheckBoxColumn
    Friend WithEvents EmployeeID As DataGridViewTextBoxColumn
    Friend WithEvents EmployeeName As DataGridViewTextBoxColumn
    Friend WithEvents LeaveType As DataGridViewTextBoxColumn
    Friend WithEvents StartDate As DataGridViewTextBoxColumn
    Friend WithEvents EndDate As DataGridViewTextBoxColumn
    Friend WithEvents Reason As DataGridViewTextBoxColumn
    Friend WithEvents Comment As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewTextBoxColumn
    Friend WithEvents RowID As DataGridViewTextBoxColumn
End Class

