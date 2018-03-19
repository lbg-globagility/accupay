<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DiscipAction
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.txtname = New System.Windows.Forms.TextBox()
        Me.txtdesc = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.dgvFindingsList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_findingname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_findingdesc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.lblforballoon = New System.Windows.Forms.Label()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvFindingsList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtname
        '
        Me.txtname.Location = New System.Drawing.Point(10, 69)
        Me.txtname.MaxLength = 100
        Me.txtname.Name = "txtname"
        Me.txtname.Size = New System.Drawing.Size(356, 20)
        Me.txtname.TabIndex = 2
        '
        'txtdesc
        '
        Me.txtdesc.Location = New System.Drawing.Point(10, 108)
        Me.txtdesc.MaxLength = 500
        Me.txtdesc.Multiline = True
        Me.txtdesc.Name = "txtdesc"
        Me.txtdesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtdesc.Size = New System.Drawing.Size(356, 82)
        Me.txtdesc.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Action Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 92)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Action Description"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.btnDelete, Me.btnCancel})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(380, 25)
        Me.ToolStrip1.TabIndex = 13
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(89, 22)
        Me.btnNew.Text = "&New Action"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(89, 22)
        Me.btnSave.Text = "&Save Action"
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(98, 22)
        Me.btnDelete.Text = "&Delete Action"
        '
        'btnCancel
        '
        Me.btnCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'dgvFindingsList
        '
        Me.dgvFindingsList.AllowUserToAddRows = False
        Me.dgvFindingsList.AllowUserToDeleteRows = False
        Me.dgvFindingsList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvFindingsList.BackgroundColor = System.Drawing.Color.White
        Me.dgvFindingsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFindingsList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_findingname, Me.c_findingdesc, Me.c_rowid})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvFindingsList.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvFindingsList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvFindingsList.Location = New System.Drawing.Point(10, 196)
        Me.dgvFindingsList.Name = "dgvFindingsList"
        Me.dgvFindingsList.ReadOnly = True
        Me.dgvFindingsList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFindingsList.Size = New System.Drawing.Size(358, 169)
        Me.dgvFindingsList.TabIndex = 322
        '
        'c_findingname
        '
        Me.c_findingname.HeaderText = "Action Name"
        Me.c_findingname.Name = "c_findingname"
        Me.c_findingname.ReadOnly = True
        Me.c_findingname.Width = 157
        '
        'c_findingdesc
        '
        Me.c_findingdesc.HeaderText = "Action Description"
        Me.c_findingdesc.Name = "c_findingdesc"
        Me.c_findingdesc.ReadOnly = True
        Me.c_findingdesc.Width = 156
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "RowID"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.ReadOnly = True
        Me.c_rowid.Visible = False
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.Location = New System.Drawing.Point(53, 31)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(72, 13)
        Me.lblSaveMsg.TabIndex = 323
        Me.lblSaveMsg.Text = "Finding Name"
        Me.lblSaveMsg.Visible = False
        '
        'lblforballoon
        '
        Me.lblforballoon.AutoSize = True
        Me.lblforballoon.Location = New System.Drawing.Point(94, 32)
        Me.lblforballoon.Name = "lblforballoon"
        Me.lblforballoon.Size = New System.Drawing.Size(72, 13)
        Me.lblforballoon.TabIndex = 324
        Me.lblforballoon.Text = "Finding Name"
        Me.lblforballoon.Visible = False
        '
        'DiscipAction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(380, 377)
        Me.Controls.Add(Me.lblSaveMsg)
        Me.Controls.Add(Me.dgvFindingsList)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtdesc)
        Me.Controls.Add(Me.txtname)
        Me.Controls.Add(Me.lblforballoon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DiscipAction"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Actions for disciplinary"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvFindingsList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtname As System.Windows.Forms.TextBox
    Friend WithEvents txtdesc As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvFindingsList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
    Friend WithEvents c_findingname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_findingdesc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_rowid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblforballoon As System.Windows.Forms.Label
End Class
