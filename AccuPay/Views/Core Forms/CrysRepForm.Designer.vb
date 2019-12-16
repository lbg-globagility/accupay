<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CrysRepForm
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
        Me.crysrepvwr = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'crysrepvwr
        '
        Me.crysrepvwr.ActiveViewIndex = -1
        Me.crysrepvwr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        'Me.crysrepvwr.CachedPageNumberPerDoc = 10
        Me.crysrepvwr.Cursor = System.Windows.Forms.Cursors.Default
        Me.crysrepvwr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.crysrepvwr.Location = New System.Drawing.Point(0, 0)
        Me.crysrepvwr.Name = "crysrepvwr"
        Me.crysrepvwr.Size = New System.Drawing.Size(919, 432)
        Me.crysrepvwr.TabIndex = 0
        '
        'CrysRepForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(919, 432)
        Me.Controls.Add(Me.crysrepvwr)
        Me.KeyPreview = True
        Me.Name = "CrysRepForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents crysrepvwr As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
