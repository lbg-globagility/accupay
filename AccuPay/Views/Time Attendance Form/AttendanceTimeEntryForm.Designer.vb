<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AttendanceTimeEntryForm
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
        Me.Label16 = New System.Windows.Forms.Label()
        Me.dgvAttendanceList = New System.Windows.Forms.DataGridView()
        Me.dtFromDate = New System.Windows.Forms.DateTimePicker()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.dtDateTo = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.c_empid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_empname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_date = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_time = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvAttendanceList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Silver
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Stencil", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(686, 24)
        Me.Label16.TabIndex = 314
        Me.Label16.Text = "Attendance Time Entry"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvAttendanceList
        '
        Me.dgvAttendanceList.AllowUserToAddRows = False
        Me.dgvAttendanceList.AllowUserToDeleteRows = False
        Me.dgvAttendanceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAttendanceList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_empid, Me.c_empname, Me.c_date, Me.c_time})
        Me.dgvAttendanceList.Location = New System.Drawing.Point(12, 150)
        Me.dgvAttendanceList.Name = "dgvAttendanceList"
        Me.dgvAttendanceList.ReadOnly = True
        Me.dgvAttendanceList.Size = New System.Drawing.Size(492, 295)
        Me.dgvAttendanceList.TabIndex = 315
        '
        'dtFromDate
        '
        Me.dtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtFromDate.Location = New System.Drawing.Point(71, 28)
        Me.dtFromDate.Name = "dtFromDate"
        Me.dtFromDate.Size = New System.Drawing.Size(112, 20)
        Me.dtFromDate.TabIndex = 316
        '
        'btnImport
        '
        Me.btnImport.Location = New System.Drawing.Point(74, 62)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(112, 23)
        Me.btnImport.TabIndex = 317
        Me.btnImport.Text = "Import Attendance"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(273, 62)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 318
        Me.btnClear.Text = "&Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'dtDateTo
        '
        Me.dtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtDateTo.Location = New System.Drawing.Point(244, 28)
        Me.dtDateTo.Name = "dtDateTo"
        Me.dtDateTo.Size = New System.Drawing.Size(112, 20)
        Me.dtDateTo.TabIndex = 319
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.btnClear)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.btnImport)
        Me.GroupBox1.Controls.Add(Me.dtDateTo)
        Me.GroupBox1.Controls.Add(Me.dtFromDate)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 27)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(374, 91)
        Me.GroupBox1.TabIndex = 320
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filter"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 13)
        Me.Label1.TabIndex = 320
        Me.Label1.Text = "Date From:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(189, 34)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 13)
        Me.Label2.TabIndex = 321
        Me.Label2.Text = "Date To:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 123)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(173, 24)
        Me.Label3.TabIndex = 321
        Me.Label3.Text = "List of attendance"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(192, 62)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 322
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'c_empid
        '
        Me.c_empid.HeaderText = "Employee ID"
        Me.c_empid.Name = "c_empid"
        Me.c_empid.ReadOnly = True
        '
        'c_empname
        '
        Me.c_empname.HeaderText = "Employee Name"
        Me.c_empname.Name = "c_empname"
        Me.c_empname.ReadOnly = True
        '
        'c_date
        '
        Me.c_date.HeaderText = "Date"
        Me.c_date.Name = "c_date"
        Me.c_date.ReadOnly = True
        '
        'c_time
        '
        Me.c_time.HeaderText = "Time"
        Me.c_time.Name = "c_time"
        Me.c_time.ReadOnly = True
        '
        'AttendanceTimeEntryForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(686, 457)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.dgvAttendanceList)
        Me.Controls.Add(Me.Label16)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "AttendanceTimeEntryForm"
        Me.Text = "AttendanceTimeEntryForm"
        CType(Me.dgvAttendanceList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents dgvAttendanceList As System.Windows.Forms.DataGridView
    Friend WithEvents dtFromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents dtDateTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents c_empid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_empname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_date As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_time As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
