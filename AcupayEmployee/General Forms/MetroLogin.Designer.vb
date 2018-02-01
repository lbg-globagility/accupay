<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MetroLogin
    'Inherits System.Windows.Forms.Form
    Inherits MetroFramework.Forms.MetroForm
    
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
        Me.components = New System.ComponentModel.Container()
        Me.MetroStyleManager1 = New MetroFramework.Components.MetroStyleManager(Me.components)
        Me.lnklblobf = New System.Windows.Forms.LinkLabel()
        Me.lnklblovertime = New System.Windows.Forms.LinkLabel()
        Me.lnklblleave = New System.Windows.Forms.LinkLabel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        CType(Me.MetroStyleManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MetroStyleManager1
        '
        Me.MetroStyleManager1.Owner = Me
        '
        'lnklblobf
        '
        Me.lnklblobf.AutoSize = True
        Me.lnklblobf.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblobf.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblobf.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblobf.Location = New System.Drawing.Point(0, 39)
        Me.lnklblobf.Name = "lnklblobf"
        Me.lnklblobf.Size = New System.Drawing.Size(126, 15)
        Me.lnklblobf.TabIndex = 22
        Me.lnklblobf.TabStop = True
        Me.lnklblobf.Text = "O&fficial Business filing"
        '
        'lnklblovertime
        '
        Me.lnklblovertime.AutoSize = True
        Me.lnklblovertime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblovertime.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblovertime.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblovertime.Location = New System.Drawing.Point(0, 24)
        Me.lnklblovertime.Name = "lnklblovertime"
        Me.lnklblovertime.Size = New System.Drawing.Size(85, 15)
        Me.lnklblovertime.TabIndex = 21
        Me.lnklblovertime.TabStop = True
        Me.lnklblovertime.Text = "Over&time filing"
        '
        'lnklblleave
        '
        Me.lnklblleave.AutoSize = True
        Me.lnklblleave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblleave.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblleave.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblleave.Location = New System.Drawing.Point(0, 9)
        Me.lnklblleave.Name = "lnklblleave"
        Me.lnklblleave.Size = New System.Drawing.Size(69, 15)
        Me.lnklblleave.TabIndex = 20
        Me.lnklblleave.TabStop = True
        Me.lnklblleave.Text = "Lea&ve filing"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.LinkLabel1)
        Me.Panel1.Controls.Add(Me.lnklblobf)
        Me.Panel1.Controls.Add(Me.lnklblleave)
        Me.Panel1.Controls.Add(Me.lnklblovertime)
        Me.Panel1.Location = New System.Drawing.Point(8, 56)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(520, 304)
        Me.Panel1.TabIndex = 23
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel1.Location = New System.Drawing.Point(132, 9)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(79, 15)
        Me.LinkLabel1.TabIndex = 23
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "My time entry"
        '
        'MetroLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(544, 371)
        Me.Controls.Add(Me.Panel1)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Movable = False
        Me.Name = "MetroLogin"
        Me.Resizable = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Welcome"
        CType(Me.MetroStyleManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MetroStyleManager1 As MetroFramework.Components.MetroStyleManager
    Friend WithEvents lnklblobf As LinkLabel
    Friend WithEvents lnklblovertime As LinkLabel
    Friend WithEvents lnklblleave As LinkLabel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents LinkLabel1 As LinkLabel
End Class
