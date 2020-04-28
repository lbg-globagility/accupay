<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LaGlobalEmployeeReportForm
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
        Me.reportViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'reportViewer
        '
        Me.reportViewer.ActiveViewIndex = -1
        Me.reportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.reportViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.reportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.reportViewer.Location = New System.Drawing.Point(0, 0)
        Me.reportViewer.Name = "reportViewer"
        Me.reportViewer.Size = New System.Drawing.Size(800, 450)
        Me.reportViewer.TabIndex = 1
        Me.reportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.ParameterPanel
        '
        'LaGlobalEmployeeReportForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.reportViewer)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "LaGlobalEmployeeReportForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents reportViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
