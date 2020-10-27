<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EmployeeTreeView
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.AccuPayEmployeeTreeView = New AccuPay.AccuPayTreeView()
        Me.chkActive = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBox1.Location = New System.Drawing.Point(0, 0)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(165, 20)
        Me.TextBox1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(165, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Search"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.AccuPayEmployeeTreeView)
        Me.Panel1.Controls.Add(Me.chkActive)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 13)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(165, 145)
        Me.Panel1.TabIndex = 5
        '
        'AccuPayEmployeeTreeView
        '
        Me.AccuPayEmployeeTreeView.CheckBoxes = True
        Me.AccuPayEmployeeTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccuPayEmployeeTreeView.Location = New System.Drawing.Point(0, 45)
        Me.AccuPayEmployeeTreeView.Name = "AccuPayEmployeeTreeView"
        Me.AccuPayEmployeeTreeView.Size = New System.Drawing.Size(165, 100)
        Me.AccuPayEmployeeTreeView.TabIndex = 1
        '
        'chkActive
        '
        Me.chkActive.AutoSize = True
        Me.chkActive.Dock = System.Windows.Forms.DockStyle.Top
        Me.chkActive.Location = New System.Drawing.Point(0, 28)
        Me.chkActive.Name = "chkActive"
        Me.chkActive.Size = New System.Drawing.Size(165, 17)
        Me.chkActive.TabIndex = 3
        Me.chkActive.Text = "Show all employees"
        Me.chkActive.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.TextBox1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(165, 28)
        Me.Panel2.TabIndex = 2
        '
        'EmployeeTreeView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Name = "EmployeeTreeView"
        Me.Size = New System.Drawing.Size(165, 158)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AccuPayEmployeeTreeView As AccuPayTreeView
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents chkActive As CheckBox
End Class
