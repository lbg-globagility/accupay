<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PersonalProfileTab
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PersonalProfileTab))
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Last = New System.Windows.Forms.LinkLabel()
        Me.Nxt = New System.Windows.Forms.LinkLabel()
        Me.Prev = New System.Windows.Forms.LinkLabel()
        Me.First = New System.Windows.Forms.LinkLabel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewEmp = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveEmp = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnClose = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnImportEmployee = New System.Windows.Forms.ToolStripButton()
        Me.tsprogbarempimport = New System.Windows.Forms.ToolStripProgressBar()
        Me.Panel2.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.Last)
        Me.Panel2.Controls.Add(Me.Nxt)
        Me.Panel2.Controls.Add(Me.Prev)
        Me.Panel2.Controls.Add(Me.First)
        Me.Panel2.Location = New System.Drawing.Point(0, 29)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1250, 570)
        Me.Panel2.TabIndex = 1
        '
        'Last
        '
        Me.Last.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Last.AutoSize = True
        Me.Last.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Last.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Last.Location = New System.Drawing.Point(389, 494)
        Me.Last.Name = "Last"
        Me.Last.Size = New System.Drawing.Size(46, 18)
        Me.Last.TabIndex = 154
        Me.Last.TabStop = True
        Me.Last.Text = "Last>>"
        '
        'Nxt
        '
        Me.Nxt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Nxt.AutoSize = True
        Me.Nxt.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Nxt.Location = New System.Drawing.Point(344, 494)
        Me.Nxt.Name = "Nxt"
        Me.Nxt.Size = New System.Drawing.Size(45, 18)
        Me.Nxt.TabIndex = 153
        Me.Nxt.TabStop = True
        Me.Nxt.Text = "Next>"
        '
        'Prev
        '
        Me.Prev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Prev.AutoSize = True
        Me.Prev.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Prev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Prev.Location = New System.Drawing.Point(66, 494)
        Me.Prev.Name = "Prev"
        Me.Prev.Size = New System.Drawing.Size(43, 18)
        Me.Prev.TabIndex = 152
        Me.Prev.TabStop = True
        Me.Prev.Text = "<Prev"
        '
        'First
        '
        Me.First.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.First.AutoSize = True
        Me.First.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.First.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.First.Location = New System.Drawing.Point(16, 494)
        Me.First.Name = "First"
        Me.First.Size = New System.Drawing.Size(49, 18)
        Me.First.TabIndex = 151
        Me.First.TabStop = True
        Me.First.Text = "<<First"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewEmp, Me.tsbtnSaveEmp, Me.tsbtnCancel, Me.tsbtnClose, Me.tsbtnAudittrail, Me.tsbtnImportEmployee, Me.tsprogbarempimport})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1031, 25)
        Me.ToolStrip1.TabIndex = 29
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNewEmp
        '
        Me.tsbtnNewEmp.Image = Global.AccuPay.My.Resources.Resources._new
        Me.tsbtnNewEmp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewEmp.Name = "tsbtnNewEmp"
        Me.tsbtnNewEmp.Size = New System.Drawing.Size(106, 22)
        Me.tsbtnNewEmp.Text = "&New Employee"
        '
        'tsbtnSaveEmp
        '
        Me.tsbtnSaveEmp.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.tsbtnSaveEmp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveEmp.Name = "tsbtnSaveEmp"
        Me.tsbtnSaveEmp.Size = New System.Drawing.Size(106, 22)
        Me.tsbtnSaveEmp.Text = "&Save Employee"
        '
        'tsbtnCancel
        '
        Me.tsbtnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.tsbtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancel.Name = "tsbtnCancel"
        Me.tsbtnCancel.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancel.Text = "Cancel"
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnClose.Name = "tsbtnClose"
        Me.tsbtnClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnClose.Text = "Close"
        '
        'tsbtnAudittrail
        '
        Me.tsbtnAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnAudittrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnAudittrail.Image = Global.AccuPay.My.Resources.Resources.audit_trail_icon
        Me.tsbtnAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnAudittrail.Name = "tsbtnAudittrail"
        Me.tsbtnAudittrail.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnAudittrail.Text = "ToolStripButton1"
        Me.tsbtnAudittrail.ToolTipText = "Show audit trails"
        '
        'tsbtnImportEmployee
        '
        Me.tsbtnImportEmployee.Image = CType(resources.GetObject("tsbtnImportEmployee.Image"), System.Drawing.Image)
        Me.tsbtnImportEmployee.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnImportEmployee.Name = "tsbtnImportEmployee"
        Me.tsbtnImportEmployee.Size = New System.Drawing.Size(118, 22)
        Me.tsbtnImportEmployee.Text = "Import Employee"
        '
        'tsprogbarempimport
        '
        Me.tsprogbarempimport.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsprogbarempimport.Name = "tsprogbarempimport"
        Me.tsprogbarempimport.Size = New System.Drawing.Size(100, 22)
        Me.tsprogbarempimport.Visible = False
        '
        'PersonalProfileTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "PersonalProfileTab"
        Me.Size = New System.Drawing.Size(1031, 519)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Last As LinkLabel
    Friend WithEvents Nxt As LinkLabel
    Friend WithEvents Prev As LinkLabel
    Friend WithEvents First As LinkLabel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents tsbtnNewEmp As ToolStripButton
    Friend WithEvents tsbtnSaveEmp As ToolStripButton
    Friend WithEvents tsbtnCancel As ToolStripButton
    Friend WithEvents tsbtnClose As ToolStripButton
    Friend WithEvents tsbtnAudittrail As ToolStripButton
    Friend WithEvents tsbtnImportEmployee As ToolStripButton
    Friend WithEvents tsprogbarempimport As ToolStripProgressBar
End Class
