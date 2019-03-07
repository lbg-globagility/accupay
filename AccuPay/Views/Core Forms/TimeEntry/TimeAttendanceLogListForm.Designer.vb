<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TimeAttendanceLogListForm
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TimeAttendanceLogDataGrid = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TimeAttendanceLogDataGridLogType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.TimeAttendanceLogDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(365, 41)
        Me.Panel1.TabIndex = 0
        '
        'TimeAttendanceLogDataGrid
        '
        Me.TimeAttendanceLogDataGrid.AllowUserToAddRows = False
        Me.TimeAttendanceLogDataGrid.AllowUserToDeleteRows = False
        Me.TimeAttendanceLogDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TimeAttendanceLogDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.TimeAttendanceLogDataGridLogType})
        Me.TimeAttendanceLogDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TimeAttendanceLogDataGrid.Location = New System.Drawing.Point(0, 41)
        Me.TimeAttendanceLogDataGrid.Name = "TimeAttendanceLogDataGrid"
        Me.TimeAttendanceLogDataGrid.ReadOnly = True
        Me.TimeAttendanceLogDataGrid.Size = New System.Drawing.Size(365, 241)
        Me.TimeAttendanceLogDataGrid.TabIndex = 1
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Timestamp"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 200
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = ""
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "TimeStamp"
        Me.Column1.HeaderText = "Timestamp"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 200
        '
        'TimeAttendanceLogDataGridLogType
        '
        Me.TimeAttendanceLogDataGridLogType.DataPropertyName = "IsTimeInDescription"
        Me.TimeAttendanceLogDataGridLogType.HeaderText = ""
        Me.TimeAttendanceLogDataGridLogType.Name = "TimeAttendanceLogDataGridLogType"
        Me.TimeAttendanceLogDataGridLogType.ReadOnly = True
        '
        'TimeAttendanceLogListForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(365, 282)
        Me.Controls.Add(Me.TimeAttendanceLogDataGrid)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TimeAttendanceLogListForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Time Attendance Log List"
        CType(Me.TimeAttendanceLogDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents TimeAttendanceLogDataGrid As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents TimeAttendanceLogDataGridLogType As DataGridViewTextBoxColumn
End Class
