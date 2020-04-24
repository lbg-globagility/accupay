<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectBranchForm
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
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.BranchComboBox = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelButton.Location = New System.Drawing.Point(197, 67)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 35)
        Me.CancelButton.TabIndex = 6
        Me.CancelButton.Text = "Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'OkButton
        '
        Me.OkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OkButton.Location = New System.Drawing.Point(116, 67)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 35)
        Me.OkButton.TabIndex = 5
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'BranchComboBox
        '
        Me.BranchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.BranchComboBox.FormattingEnabled = True
        Me.BranchComboBox.Location = New System.Drawing.Point(12, 22)
        Me.BranchComboBox.Name = "BranchComboBox"
        Me.BranchComboBox.Size = New System.Drawing.Size(260, 21)
        Me.BranchComboBox.TabIndex = 7
        '
        'SelectBranchForm
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.CancelButton
        Me.ClientSize = New System.Drawing.Size(284, 113)
        Me.Controls.Add(Me.BranchComboBox)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.OkButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectBranchForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select Branch"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CancelButton As Button
    Friend WithEvents OkButton As Button
    Friend WithEvents BranchComboBox As ComboBox
End Class
