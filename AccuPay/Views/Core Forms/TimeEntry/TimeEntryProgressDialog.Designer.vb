<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TimeEntryProgressDialog
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
        Me.components = New System.ComponentModel.Container()
        Me.CompletionProgressBar = New System.Windows.Forms.ProgressBar()
        Me.ProgressTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'CompletionProgressBar
        '
        Me.CompletionProgressBar.Location = New System.Drawing.Point(16, 72)
        Me.CompletionProgressBar.Name = "CompletionProgressBar"
        Me.CompletionProgressBar.Size = New System.Drawing.Size(352, 23)
        Me.CompletionProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.CompletionProgressBar.TabIndex = 0
        '
        'ProgressTimer
        '
        Me.ProgressTimer.Enabled = True
        Me.ProgressTimer.Interval = 500
        '
        'TimeEntryProgressDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 161)
        Me.ControlBox = False
        Me.Controls.Add(Me.CompletionProgressBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "TimeEntryProgressDialog"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Time Entry Progress Dialog"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CompletionProgressBar As ProgressBar
    Friend WithEvents ProgressTimer As Timer
End Class
