<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewCalendarDialog
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
        Me.NameTextbox = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.CopyCalendarComboBox = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CreateButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'NameTextbox
        '
        Me.NameTextbox.Location = New System.Drawing.Point(8, 24)
        Me.NameTextbox.Name = "NameTextbox"
        Me.NameTextbox.Size = New System.Drawing.Size(272, 22)
        Me.NameTextbox.TabIndex = 0
        '
        'NameLabel
        '
        Me.NameLabel.Location = New System.Drawing.Point(8, 8)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(104, 16)
        Me.NameLabel.TabIndex = 1
        Me.NameLabel.Text = "Name"
        '
        'CopyCalendarComboBox
        '
        Me.CopyCalendarComboBox.DisplayMember = "Name"
        Me.CopyCalendarComboBox.FormattingEnabled = True
        Me.CopyCalendarComboBox.Location = New System.Drawing.Point(8, 72)
        Me.CopyCalendarComboBox.Name = "CopyCalendarComboBox"
        Me.CopyCalendarComboBox.Size = New System.Drawing.Size(272, 21)
        Me.CopyCalendarComboBox.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(160, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Calendar To Copy Days From"
        '
        'CreateButton
        '
        Me.CreateButton.Location = New System.Drawing.Point(96, 128)
        Me.CreateButton.Name = "CreateButton"
        Me.CreateButton.Size = New System.Drawing.Size(99, 24)
        Me.CreateButton.TabIndex = 4
        Me.CreateButton.Text = "Create Calendar"
        Me.CreateButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Location = New System.Drawing.Point(200, 128)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(80, 24)
        Me.CancelButton.TabIndex = 5
        Me.CancelButton.Text = "Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'NewCalendarDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(288, 164)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.CreateButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CopyCalendarComboBox)
        Me.Controls.Add(Me.NameLabel)
        Me.Controls.Add(Me.NameTextbox)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "NewCalendarDialog"
        Me.Text = "New Calendar"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents NameTextbox As TextBox
    Friend WithEvents NameLabel As Label
    Friend WithEvents CopyCalendarComboBox As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents CreateButton As Button
    Friend WithEvents CancelButton As Button
End Class
