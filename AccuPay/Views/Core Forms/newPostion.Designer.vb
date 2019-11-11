<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class newPostion
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
        Me.cboParentPosit = New System.Windows.Forms.ComboBox()
        Me.txtPositName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboDivis = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'cboParentPosit
        '
        Me.cboParentPosit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboParentPosit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboParentPosit.FormattingEnabled = True
        Me.cboParentPosit.Location = New System.Drawing.Point(136, 50)
        Me.cboParentPosit.MaxLength = 50
        Me.cboParentPosit.Name = "cboParentPosit"
        Me.cboParentPosit.Size = New System.Drawing.Size(168, 21)
        Me.cboParentPosit.TabIndex = 1
        '
        'txtPositName
        '
        Me.txtPositName.Location = New System.Drawing.Point(136, 77)
        Me.txtPositName.MaxLength = 50
        Me.txtPositName.Name = "txtPositName"
        Me.txtPositName.ShortcutsEnabled = False
        Me.txtPositName.Size = New System.Drawing.Size(168, 20)
        Me.txtPositName.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(29, 58)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(78, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Parent Position"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(29, 84)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Position Name"
        '
        'Button1
        '
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(173, 132)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label22.ForeColor = System.Drawing.Color.Red
        Me.Label22.Location = New System.Drawing.Point(101, 76)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(19, 23)
        Me.Label22.TabIndex = 106
        Me.Label22.Text = "*"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(254, 132)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(29, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 110
        Me.Label3.Text = "Division Name"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(99, 23)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(19, 23)
        Me.Label5.TabIndex = 109
        Me.Label5.Text = "*"
        '
        'cboDivis
        '
        Me.cboDivis.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboDivis.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboDivis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDivis.FormattingEnabled = True
        Me.cboDivis.Location = New System.Drawing.Point(136, 23)
        Me.cboDivis.MaxLength = 5
        Me.cboDivis.Name = "cboDivis"
        Me.cboDivis.Size = New System.Drawing.Size(168, 21)
        Me.cboDivis.TabIndex = 0
        '
        'newPostion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(341, 187)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cboDivis)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.cboParentPosit)
        Me.Controls.Add(Me.txtPositName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label22)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "newPostion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboParentPosit As System.Windows.Forms.ComboBox
    Friend WithEvents txtPositName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cboDivis As System.Windows.Forms.ComboBox
End Class
