<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AddPositionForm
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
        Me.btnAddAndClose = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnAddAndNew = New System.Windows.Forms.Button()
        Me.PositionUserControl1 = New PositionUserControl()
        Me.SuspendLayout()
        '
        'btnAddAndClose
        '
        Me.btnAddAndClose.Location = New System.Drawing.Point(25, 160)
        Me.btnAddAndClose.Name = "btnAddAndClose"
        Me.btnAddAndClose.Size = New System.Drawing.Size(85, 23)
        Me.btnAddAndClose.TabIndex = 12
        Me.btnAddAndClose.Text = "&Add && Close"
        Me.btnAddAndClose.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(199, 160)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAddAndNew
        '
        Me.btnAddAndNew.Location = New System.Drawing.Point(116, 160)
        Me.btnAddAndNew.Name = "btnAddAndNew"
        Me.btnAddAndNew.Size = New System.Drawing.Size(75, 23)
        Me.btnAddAndNew.TabIndex = 10
        Me.btnAddAndNew.Text = "Add && &New"
        Me.btnAddAndNew.UseVisualStyleBackColor = True
        '
        'PositionUserControl1
        '
        Me.PositionUserControl1.Location = New System.Drawing.Point(34, 7)
        Me.PositionUserControl1.Name = "PositionUserControl1"
        Me.PositionUserControl1.Size = New System.Drawing.Size(232, 150)
        Me.PositionUserControl1.TabIndex = 6
        '
        'AddPositionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(298, 195)
        Me.Controls.Add(Me.btnAddAndClose)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAddAndNew)
        Me.Controls.Add(Me.PositionUserControl1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddPositionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Position"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PositionUserControl1 As PositionUserControl
    Friend WithEvents btnAddAndClose As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnAddAndNew As Button
End Class
