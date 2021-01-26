<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddBonusTypeForm
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
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.Label164 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chktaxab = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'Button2
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.Location = New System.Drawing.Point(212, 98)
        Me.CloseButton.Name = "Button2"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 356
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.Location = New System.Drawing.Point(131, 98)
        Me.SaveButton.Name = "Button1"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 355
        Me.SaveButton.Text = "&Add"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'Label164
        '
        Me.Label164.AutoSize = True
        Me.Label164.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label164.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label164.Location = New System.Drawing.Point(93, 21)
        Me.Label164.Name = "Label164"
        Me.Label164.Size = New System.Drawing.Size(18, 24)
        Me.Label164.TabIndex = 354
        Me.Label164.Text = "*"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(117, 22)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(169, 22)
        Me.TextBox1.TabIndex = 353
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 352
        Me.Label1.Text = "Bonus name :"
        '
        'chktaxab
        '
        Me.chktaxab.AutoSize = True
        Me.chktaxab.Location = New System.Drawing.Point(117, 48)
        Me.chktaxab.Name = "chktaxab"
        Me.chktaxab.Size = New System.Drawing.Size(64, 17)
        Me.chktaxab.TabIndex = 357
        Me.chktaxab.Text = "Taxable"
        Me.chktaxab.UseVisualStyleBackColor = True
        Me.chktaxab.Visible = False
        '
        'AddBonusTypeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(319, 142)
        Me.Controls.Add(Me.chktaxab)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.Label164)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddBonusTypeForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "New Bonus Type"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents Label164 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chktaxab As System.Windows.Forms.CheckBox
End Class
