<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TimeAttendForm
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.MassOvertimeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LeaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OfficialBusinessToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OvertimeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OldShiftToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShiftScheduleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimeLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TripTicketsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SummaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PanelTimeAttend = New System.Windows.Forms.Panel()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.Color.Transparent
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MassOvertimeToolStripMenuItem, Me.LeaveToolStripMenuItem, Me.OfficialBusinessToolStripMenuItem, Me.OvertimeToolStripMenuItem, Me.OldShiftToolStripMenuItem, Me.ShiftScheduleToolStripMenuItem, Me.TimeLogsToolStripMenuItem, Me.TripTicketsToolStripMenuItem, Me.SummaryToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1006, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'MassOvertimeToolStripMenuItem
        '
        Me.MassOvertimeToolStripMenuItem.Name = "MassOvertimeToolStripMenuItem"
        Me.MassOvertimeToolStripMenuItem.Size = New System.Drawing.Size(103, 20)
        Me.MassOvertimeToolStripMenuItem.Text = "Mass Overtimes"
        '
        'LeaveToolStripMenuItem
        '
        Me.LeaveToolStripMenuItem.Name = "LeaveToolStripMenuItem"
        Me.LeaveToolStripMenuItem.Size = New System.Drawing.Size(54, 20)
        Me.LeaveToolStripMenuItem.Text = "Leaves"
        '
        'OfficialBusinessToolStripMenuItem
        '
        Me.OfficialBusinessToolStripMenuItem.Name = "OfficialBusinessToolStripMenuItem"
        Me.OfficialBusinessToolStripMenuItem.Size = New System.Drawing.Size(105, 20)
        Me.OfficialBusinessToolStripMenuItem.Text = "Official Business"
        '
        'OvertimeToolStripMenuItem
        '
        Me.OvertimeToolStripMenuItem.Name = "OvertimeToolStripMenuItem"
        Me.OvertimeToolStripMenuItem.Size = New System.Drawing.Size(73, 20)
        Me.OvertimeToolStripMenuItem.Text = "Overtimes"
        '
        'OldShiftToolStripMenuItem
        '
        Me.OldShiftToolStripMenuItem.Name = "OldShiftToolStripMenuItem"
        Me.OldShiftToolStripMenuItem.Size = New System.Drawing.Size(98, 20)
        Me.OldShiftToolStripMenuItem.Text = "Employee Shift"
        '
        'ShiftScheduleToolStripMenuItem
        '
        Me.ShiftScheduleToolStripMenuItem.Name = "ShiftScheduleToolStripMenuItem"
        Me.ShiftScheduleToolStripMenuItem.Size = New System.Drawing.Size(94, 20)
        Me.ShiftScheduleToolStripMenuItem.Text = "Shift Schedule"
        '
        'TimeLogsToolStripMenuItem
        '
        Me.TimeLogsToolStripMenuItem.Name = "TimeLogsToolStripMenuItem"
        Me.TimeLogsToolStripMenuItem.Size = New System.Drawing.Size(73, 20)
        Me.TimeLogsToolStripMenuItem.Text = "Time Logs"
        '
        'TripTicketsToolStripMenuItem
        '
        Me.TripTicketsToolStripMenuItem.Name = "TripTicketsToolStripMenuItem"
        Me.TripTicketsToolStripMenuItem.Size = New System.Drawing.Size(77, 20)
        Me.TripTicketsToolStripMenuItem.Text = "Trip Tickets"
        '
        'SummaryToolStripMenuItem
        '
        Me.SummaryToolStripMenuItem.Name = "SummaryToolStripMenuItem"
        Me.SummaryToolStripMenuItem.Size = New System.Drawing.Size(70, 20)
        Me.SummaryToolStripMenuItem.Text = "Summary"
        '
        'PanelTimeAttend
        '
        Me.PanelTimeAttend.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelTimeAttend.Location = New System.Drawing.Point(0, 24)
        Me.PanelTimeAttend.Name = "PanelTimeAttend"
        Me.PanelTimeAttend.Size = New System.Drawing.Size(1006, 446)
        Me.PanelTimeAttend.TabIndex = 3
        '
        'TimeAttendForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1006, 470)
        Me.Controls.Add(Me.PanelTimeAttend)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "TimeAttendForm"
        Me.Text = "TimeAttendForm"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents OldShiftToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PanelTimeAttend As System.Windows.Forms.Panel
    Friend WithEvents SummaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MassOvertimeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShiftScheduleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TimeLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OfficialBusinessToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LeaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OvertimeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TripTicketsToolStripMenuItem As ToolStripMenuItem
End Class
