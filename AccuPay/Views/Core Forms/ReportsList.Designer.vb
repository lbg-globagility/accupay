<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ReportsList
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lvMainMenu = New System.Windows.Forms.ListView()
        Me.SuspendLayout()
        '
        'lvMainMenu
        '
        Me.lvMainMenu.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvMainMenu.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvMainMenu.Location = New System.Drawing.Point(8, 8)
        Me.lvMainMenu.MultiSelect = False
        Me.lvMainMenu.Name = "lvMainMenu"
        Me.lvMainMenu.Size = New System.Drawing.Size(376, 416)
        Me.lvMainMenu.TabIndex = 1
        Me.lvMainMenu.UseCompatibleStateImageBehavior = False
        Me.lvMainMenu.View = System.Windows.Forms.View.List
        '
        'ReportsList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(697, 432)
        Me.Controls.Add(Me.lvMainMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "ReportsList"
        Me.Text = "ReportsList"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvMainMenu As System.Windows.Forms.ListView
End Class
