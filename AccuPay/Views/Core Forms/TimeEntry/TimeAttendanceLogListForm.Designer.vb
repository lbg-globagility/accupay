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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TimeAttendanceLogDataGrid = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblShift = New System.Windows.Forms.Label()
        Me.lblBreakTime = New System.Windows.Forms.Label()
        Me.ColumnTimeStamp = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TimeAttendanceLogDataGridLogType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.TimeAttendanceLogDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblBreakTime)
        Me.Panel1.Controls.Add(Me.lblShift)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(477, 41)
        Me.Panel1.TabIndex = 0
        '
        'TimeAttendanceLogDataGrid
        '
        Me.TimeAttendanceLogDataGrid.AllowUserToAddRows = False
        Me.TimeAttendanceLogDataGrid.AllowUserToDeleteRows = False
        Me.TimeAttendanceLogDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TimeAttendanceLogDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnTimeStamp, Me.TimeAttendanceLogDataGridLogType, Me.Column2})
        Me.TimeAttendanceLogDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TimeAttendanceLogDataGrid.Location = New System.Drawing.Point(0, 41)
        Me.TimeAttendanceLogDataGrid.Name = "TimeAttendanceLogDataGrid"
        Me.TimeAttendanceLogDataGrid.ReadOnly = True
        Me.TimeAttendanceLogDataGrid.Size = New System.Drawing.Size(477, 241)
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
        'lblShift
        '
        Me.lblShift.AutoSize = True
        Me.lblShift.Location = New System.Drawing.Point(3, 16)
        Me.lblShift.Name = "lblShift"
        Me.lblShift.Size = New System.Drawing.Size(30, 13)
        Me.lblShift.TabIndex = 0
        Me.lblShift.Text = "shift"
        '
        'lblBreakTime
        '
        Me.lblBreakTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBreakTime.Location = New System.Drawing.Point(231, 16)
        Me.lblBreakTime.Name = "lblBreakTime"
        Me.lblBreakTime.Size = New System.Drawing.Size(243, 13)
        Me.lblBreakTime.TabIndex = 1
        Me.lblBreakTime.Text = "break time"
        Me.lblBreakTime.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ColumnTimeStamp
        '
        Me.ColumnTimeStamp.DataPropertyName = "TimeStamp"
        Me.ColumnTimeStamp.HeaderText = "Timestamp"
        Me.ColumnTimeStamp.Name = "ColumnTimeStamp"
        Me.ColumnTimeStamp.ReadOnly = True
        Me.ColumnTimeStamp.Width = 200
        '
        'TimeAttendanceLogDataGridLogType
        '
        Me.TimeAttendanceLogDataGridLogType.DataPropertyName = "IsTimeInDescription"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.TimeAttendanceLogDataGridLogType.DefaultCellStyle = DataGridViewCellStyle1
        Me.TimeAttendanceLogDataGridLogType.HeaderText = ""
        Me.TimeAttendanceLogDataGridLogType.Name = "TimeAttendanceLogDataGridLogType"
        Me.TimeAttendanceLogDataGridLogType.ReadOnly = True
        Me.TimeAttendanceLogDataGridLogType.Width = 80
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "ImportNumber"
        Me.Column2.HeaderText = "Import ID"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 140
        '
        'TimeAttendanceLogListForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(477, 282)
        Me.Controls.Add(Me.TimeAttendanceLogDataGrid)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TimeAttendanceLogListForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Time Attendance Log List"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.TimeAttendanceLogDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents TimeAttendanceLogDataGrid As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents lblBreakTime As Label
    Friend WithEvents lblShift As Label
    Friend WithEvents ColumnTimeStamp As DataGridViewTextBoxColumn
    Friend WithEvents TimeAttendanceLogDataGridLogType As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
End Class
