<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewRoleForm
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
        Me.RoleUserControl = New RoleUserControl()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.SaveButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'RoleUserControl1
        '
        Me.RoleUserControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RoleUserControl.Location = New System.Drawing.Point(0, 0)
        Me.RoleUserControl.Name = "RoleUserControl1"
        Me.RoleUserControl.Size = New System.Drawing.Size(846, 615)
        Me.RoleUserControl.TabIndex = 0
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(846, 25)
        Me.ToolStrip1.TabIndex = 160
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'SaveButton
        '
        Me.SaveButton.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(77, 22)
        Me.SaveButton.Text = "&Save Role"
        '
        'NewRoleForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(846, 615)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.RoleUserControl)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "NewRoleForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Role"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RoleUserControl As RoleUserControl
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents SaveButton As ToolStripButton
End Class
