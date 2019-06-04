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
        Me.TimeEntToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShiftScheduleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimeEntryLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SummaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OvertimeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PanelTimeAttend = New System.Windows.Forms.Panel()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.Color.Transparent
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TimeEntToolStripMenuItem, Me.ShiftScheduleToolStripMenuItem, Me.ToolStripMenuItem1, Me.TimeEntryLogsToolStripMenuItem, Me.SummaryToolStripMenuItem, Me.OvertimeToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1006, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'TimeEntToolStripMenuItem
        '
        Me.TimeEntToolStripMenuItem.Name = "TimeEntToolStripMenuItem"
        Me.TimeEntToolStripMenuItem.Size = New System.Drawing.Size(98, 20)
        Me.TimeEntToolStripMenuItem.Text = "Employee Shift"
        '
        'ShiftScheduleToolStripMenuItem
        '
        Me.ShiftScheduleToolStripMenuItem.Name = "ShiftScheduleToolStripMenuItem"
        Me.ShiftScheduleToolStripMenuItem.Size = New System.Drawing.Size(94, 20)
        Me.ShiftScheduleToolStripMenuItem.Text = "Shift Schedule"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(74, 20)
        Me.ToolStripMenuItem1.Text = "Time Logs"
        '
        'TimeEntryLogsToolStripMenuItem
        '
        Me.TimeEntryLogsToolStripMenuItem.Checked = True
        Me.TimeEntryLogsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TimeEntryLogsToolStripMenuItem.Name = "TimeEntryLogsToolStripMenuItem"
        Me.TimeEntryLogsToolStripMenuItem.Size = New System.Drawing.Size(101, 20)
        Me.TimeEntryLogsToolStripMenuItem.Text = "Time entry logs"
        Me.TimeEntryLogsToolStripMenuItem.Visible = False
        '
        'SummaryToolStripMenuItem
        '
        Me.SummaryToolStripMenuItem.Name = "SummaryToolStripMenuItem"
        Me.SummaryToolStripMenuItem.Size = New System.Drawing.Size(70, 20)
        Me.SummaryToolStripMenuItem.Text = "Summary"
        '
        'OvertimeToolStripMenuItem
        '
        Me.OvertimeToolStripMenuItem.Name = "OvertimeToolStripMenuItem"
        Me.OvertimeToolStripMenuItem.Size = New System.Drawing.Size(73, 20)
        Me.OvertimeToolStripMenuItem.Text = "Overtimes"
        Me.OvertimeToolStripMenuItem.Visible = False
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
    Friend WithEvents TimeEntToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PanelTimeAttend As System.Windows.Forms.Panel
    Friend WithEvents TimeEntryLogsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SummaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OvertimeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShiftScheduleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
End Class
