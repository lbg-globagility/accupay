<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CalendarDayControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.DayLabel = New System.Windows.Forms.Label()
        Me.DescriptionLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'DayLabel
        '
        Me.DayLabel.AutoSize = True
        Me.DayLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DayLabel.Location = New System.Drawing.Point(0, 0)
        Me.DayLabel.Name = "DayLabel"
        Me.DayLabel.Size = New System.Drawing.Size(42, 13)
        Me.DayLabel.TabIndex = 0
        Me.DayLabel.Text = "<Day>"
        '
        'DescriptionLabel
        '
        Me.DescriptionLabel.AutoSize = True
        Me.DescriptionLabel.Location = New System.Drawing.Point(0, 16)
        Me.DescriptionLabel.Name = "DescriptionLabel"
        Me.DescriptionLabel.Size = New System.Drawing.Size(82, 13)
        Me.DescriptionLabel.TabIndex = 1
        Me.DescriptionLabel.Text = "<Description>"
        '
        'CalendarDayControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.Controls.Add(Me.DescriptionLabel)
        Me.Controls.Add(Me.DayLabel)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(1)
        Me.Name = "CalendarDayControl"
        Me.Size = New System.Drawing.Size(64, 40)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DayLabel As Label
    Friend WithEvents DescriptionLabel As Label
End Class
