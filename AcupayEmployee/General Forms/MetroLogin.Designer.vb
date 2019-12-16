<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MetroLogin
    'Inherits System.Windows.Forms.Form
    Inherits MetroFramework.Forms.MetroForm

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
        Me.lnklblobf.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblobf.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblobf.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblobf.Location = New System.Drawing.Point(24, 96)
        Me.lnklblobf.Name = "lnklblobf"
        Me.lnklblobf.Size = New System.Drawing.Size(184, 24)
        Me.lnklblobf.TabIndex = 22
        Me.lnklblobf.TabStop = True
        Me.lnklblobf.Text = "New Official Business"
        '
        'lnklblovertime
        '
        Me.lnklblovertime.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblovertime.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblovertime.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblovertime.Location = New System.Drawing.Point(24, 56)
        Me.lnklblovertime.Name = "lnklblovertime"
        Me.lnklblovertime.Size = New System.Drawing.Size(184, 24)
        Me.lnklblovertime.TabIndex = 21
        Me.lnklblovertime.TabStop = True
        Me.lnklblovertime.Text = "New Overtime"
        '
        'lnklblleave
        '
        Me.lnklblleave.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnklblleave.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnklblleave.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnklblleave.Location = New System.Drawing.Point(24, 16)
        Me.lnklblleave.Name = "lnklblleave"
        Me.lnklblleave.Size = New System.Drawing.Size(184, 24)
        Me.lnklblleave.TabIndex = 20
        Me.lnklblleave.TabStop = True
        Me.lnklblleave.Text = "New Leave of Absence"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.LinkLabel1)
        Me.Panel1.Controls.Add(Me.lnklblobf)
        Me.Panel1.Controls.Add(Me.lnklblleave)
        Me.Panel1.Controls.Add(Me.lnklblovertime)
        Me.Panel1.Location = New System.Drawing.Point(8, 56)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(504, 192)
        Me.Panel1.TabIndex = 23
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel1.Location = New System.Drawing.Point(24, 136)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(184, 24)
        Me.LinkLabel1.TabIndex = 23
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "My time entry"
        '
        'MetroLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(520, 256)
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
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MetroStyleManager1 As MetroFramework.Components.MetroStyleManager
    Friend WithEvents lnklblobf As LinkLabel
    Friend WithEvents lnklblovertime As LinkLabel
    Friend WithEvents lnklblleave As LinkLabel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents LinkLabel1 As LinkLabel
End Class
