<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PBCOMSelection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PBCOMSelection))
        Me.PBComPayBtn = New System.Windows.Forms.Button()
        Me.ThirteenMonthPayBtn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'PBComPayBtn
        '
        Me.PBComPayBtn.Location = New System.Drawing.Point(128, 17)
        Me.PBComPayBtn.Name = "PBComPayBtn"
        Me.PBComPayBtn.Size = New System.Drawing.Size(179, 23)
        Me.PBComPayBtn.TabIndex = 0
        Me.PBComPayBtn.Text = "PBCOM Pay Report "
        Me.PBComPayBtn.UseVisualStyleBackColor = True
        '
        'ThirteenMonthPayBtn
        '
        Me.ThirteenMonthPayBtn.Location = New System.Drawing.Point(128, 48)
        Me.ThirteenMonthPayBtn.Name = "ThirteenMonthPayBtn"
        Me.ThirteenMonthPayBtn.Size = New System.Drawing.Size(179, 23)
        Me.ThirteenMonthPayBtn.TabIndex = 1
        Me.ThirteenMonthPayBtn.Text = "13th Month Pay Report"
        Me.ThirteenMonthPayBtn.UseVisualStyleBackColor = True
        '
        'PBCOMSelection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(420, 96)
        Me.Controls.Add(Me.ThirteenMonthPayBtn)
        Me.Controls.Add(Me.PBComPayBtn)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "PBCOMSelection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PBCOM Report Selection"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PBComPayBtn As Button
    Friend WithEvents ThirteenMonthPayBtn As Button
End Class
