<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ResetLeaveCreditApplyPreviewForm
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.DataGridViewX1 = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.IsSelected = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.EmployeeNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StartDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateRegularized = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.VacationLeaveCredit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SickLeaveCredit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IsApplied = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.DataGridViewX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.DataGridViewX1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(800, 406)
        Me.Panel1.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnApply)
        Me.Panel2.Controls.Add(Me.Button2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 406)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(800, 44)
        Me.Panel2.TabIndex = 1
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(713, 6)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 31)
        Me.Button2.TabIndex = 0
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.Location = New System.Drawing.Point(527, 6)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(180, 31)
        Me.btnApply.TabIndex = 1
        Me.btnApply.Text = "Apply Leave Credits"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'DataGridViewX1
        '
        Me.DataGridViewX1.AllowUserToAddRows = False
        Me.DataGridViewX1.AllowUserToDeleteRows = False
        Me.DataGridViewX1.AllowUserToOrderColumns = True
        Me.DataGridViewX1.AllowUserToResizeRows = False
        Me.DataGridViewX1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridViewX1.ColumnHeadersHeight = 34
        Me.DataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridViewX1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IsSelected, Me.EmployeeNo, Me.LastName, Me.FirstName, Me.StartDate, Me.DateRegularized, Me.VacationLeaveCredit, Me.SickLeaveCredit, Me.IsApplied})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewX1.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewX1.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.DataGridViewX1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridViewX1.MultiSelect = False
        Me.DataGridViewX1.Name = "DataGridViewX1"
        Me.DataGridViewX1.ReadOnly = True
        Me.DataGridViewX1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewX1.Size = New System.Drawing.Size(800, 406)
        Me.DataGridViewX1.TabIndex = 140
        '
        'IsSelected
        '
        Me.IsSelected.DataPropertyName = "IsSelected"
        Me.IsSelected.HeaderText = ""
        Me.IsSelected.Name = "IsSelected"
        Me.IsSelected.ReadOnly = True
        Me.IsSelected.Width = 32
        '
        'EmployeeNo
        '
        Me.EmployeeNo.DataPropertyName = "EmployeeNo"
        Me.EmployeeNo.HeaderText = "Employee ID"
        Me.EmployeeNo.Name = "EmployeeNo"
        Me.EmployeeNo.ReadOnly = True
        '
        'LastName
        '
        Me.LastName.DataPropertyName = "LastName"
        Me.LastName.HeaderText = "Last Name"
        Me.LastName.Name = "LastName"
        Me.LastName.ReadOnly = True
        Me.LastName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.LastName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'FirstName
        '
        Me.FirstName.DataPropertyName = "FirstName"
        Me.FirstName.HeaderText = "First Name"
        Me.FirstName.Name = "FirstName"
        Me.FirstName.ReadOnly = True
        Me.FirstName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.FirstName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'StartDate
        '
        Me.StartDate.DataPropertyName = "StartDate"
        Me.StartDate.HeaderText = "Start Date"
        Me.StartDate.Name = "StartDate"
        Me.StartDate.ReadOnly = True
        '
        'DateRegularized
        '
        Me.DateRegularized.DataPropertyName = "DateRegularized"
        Me.DateRegularized.HeaderText = "Date Regularized"
        Me.DateRegularized.Name = "DateRegularized"
        Me.DateRegularized.ReadOnly = True
        '
        'VacationLeaveCredit
        '
        Me.VacationLeaveCredit.DataPropertyName = "VacationLeaveCredit"
        DataGridViewCellStyle1.Format = "N2"
        DataGridViewCellStyle1.NullValue = Nothing
        Me.VacationLeaveCredit.DefaultCellStyle = DataGridViewCellStyle1
        Me.VacationLeaveCredit.HeaderText = "Vacation Leave Credit"
        Me.VacationLeaveCredit.Name = "VacationLeaveCredit"
        Me.VacationLeaveCredit.ReadOnly = True
        '
        'SickLeaveCredit
        '
        Me.SickLeaveCredit.DataPropertyName = "SickLeaveCredit"
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.SickLeaveCredit.DefaultCellStyle = DataGridViewCellStyle2
        Me.SickLeaveCredit.HeaderText = "Sick Leave Credit"
        Me.SickLeaveCredit.Name = "SickLeaveCredit"
        Me.SickLeaveCredit.ReadOnly = True
        '
        'IsApplied
        '
        Me.IsApplied.DataPropertyName = "IsAppliedText"
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Green
        Me.IsApplied.DefaultCellStyle = DataGridViewCellStyle3
        Me.IsApplied.HeaderText = "Is Applied?"
        Me.IsApplied.Name = "IsApplied"
        Me.IsApplied.ReadOnly = True
        Me.IsApplied.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'ResetLeaveCreditApplyPreviewForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ResetLeaveCreditApplyPreviewForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        CType(Me.DataGridViewX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Button2 As Button
    Friend WithEvents btnApply As Button
    Friend WithEvents DataGridViewX1 As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents IsSelected As DataGridViewCheckBoxColumn
    Friend WithEvents EmployeeNo As DataGridViewTextBoxColumn
    Friend WithEvents LastName As DataGridViewTextBoxColumn
    Friend WithEvents FirstName As DataGridViewTextBoxColumn
    Friend WithEvents StartDate As DataGridViewTextBoxColumn
    Friend WithEvents DateRegularized As DataGridViewTextBoxColumn
    Friend WithEvents VacationLeaveCredit As DataGridViewTextBoxColumn
    Friend WithEvents SickLeaveCredit As DataGridViewTextBoxColumn
    Friend WithEvents IsApplied As DataGridViewTextBoxColumn
End Class
