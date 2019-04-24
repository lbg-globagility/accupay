<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddDivisionForm
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
        Me.DivisionUserControl1 = New AccuPay.DivisionUserControl()
        Me.btnAddAndClose = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnAddAndNew = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'DivisionUserControl1
        '
        Me.DivisionUserControl1.Location = New System.Drawing.Point(12, 12)
        Me.DivisionUserControl1.Name = "DivisionUserControl1"
        Me.DivisionUserControl1.Size = New System.Drawing.Size(740, 600)
        Me.DivisionUserControl1.TabIndex = 0
        '
        'btnAddAndClose
        '
        Me.btnAddAndClose.Location = New System.Drawing.Point(258, 642)
        Me.btnAddAndClose.Name = "btnAddAndClose"
        Me.btnAddAndClose.Size = New System.Drawing.Size(85, 23)
        Me.btnAddAndClose.TabIndex = 15
        Me.btnAddAndClose.Text = "&Add && Close"
        Me.btnAddAndClose.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(432, 642)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 14
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAddAndNew
        '
        Me.btnAddAndNew.Location = New System.Drawing.Point(349, 642)
        Me.btnAddAndNew.Name = "btnAddAndNew"
        Me.btnAddAndNew.Size = New System.Drawing.Size(75, 23)
        Me.btnAddAndNew.TabIndex = 13
        Me.btnAddAndNew.Text = "Add && &New"
        Me.btnAddAndNew.UseVisualStyleBackColor = True
        '
        'AddDivisionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(761, 682)
        Me.Controls.Add(Me.btnAddAndClose)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAddAndNew)
        Me.Controls.Add(Me.DivisionUserControl1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddDivisionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Division"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DivisionUserControl1 As DivisionUserControl
    Friend WithEvents btnAddAndClose As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnAddAndNew As Button
End Class
