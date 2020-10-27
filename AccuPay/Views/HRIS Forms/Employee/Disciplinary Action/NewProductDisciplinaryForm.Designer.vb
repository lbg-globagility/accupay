<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NewProductDisciplinaryForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblActionName = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.dgvFindings = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_findingname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_findingdesc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvFindings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.btnDelete, Me.btnCancel})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(447, 25)
        Me.ToolStrip1.TabIndex = 15
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(94, 22)
        Me.btnNew.Text = "&New Finding"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(94, 22)
        Me.btnSave.Text = "&Save Finding"
        '
        'btnDelete
        '
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(103, 22)
        Me.btnDelete.Text = "&Delete Finding"
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
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 332
        Me.Label2.Text = "Description"
        '
        'lblActionName
        '
        Me.lblActionName.AutoSize = True
        Me.lblActionName.Location = New System.Drawing.Point(14, 45)
        Me.lblActionName.Name = "lblActionName"
        Me.lblActionName.Size = New System.Drawing.Size(72, 13)
        Me.lblActionName.TabIndex = 331
        Me.lblActionName.Text = "Finding Name"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(12, 100)
        Me.txtDescription.MaxLength = 500
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(423, 82)
        Me.txtDescription.TabIndex = 330
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(12, 61)
        Me.txtName.MaxLength = 100
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(423, 20)
        Me.txtName.TabIndex = 329
        '
        'dgvFindings
        '
        Me.dgvFindings.AllowUserToAddRows = False
        Me.dgvFindings.AllowUserToDeleteRows = False
        Me.dgvFindings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvFindings.BackgroundColor = System.Drawing.Color.White
        Me.dgvFindings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFindings.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_findingname, Me.c_findingdesc, Me.c_rowid})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvFindings.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvFindings.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvFindings.Location = New System.Drawing.Point(12, 188)
        Me.dgvFindings.MultiSelect = False
        Me.dgvFindings.Name = "dgvFindings"
        Me.dgvFindings.ReadOnly = True
        Me.dgvFindings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFindings.Size = New System.Drawing.Size(425, 191)
        Me.dgvFindings.TabIndex = 333
        '
        'c_findingname
        '
        Me.c_findingname.DataPropertyName = "PartNo"
        Me.c_findingname.HeaderText = "Finding Name"
        Me.c_findingname.Name = "c_findingname"
        Me.c_findingname.ReadOnly = True
        Me.c_findingname.Width = 130
        '
        'c_findingdesc
        '
        Me.c_findingdesc.DataPropertyName = "Description"
        Me.c_findingdesc.HeaderText = "Description"
        Me.c_findingdesc.Name = "c_findingdesc"
        Me.c_findingdesc.ReadOnly = True
        Me.c_findingdesc.Width = 250
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "RowID"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.ReadOnly = True
        Me.c_rowid.Visible = False
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label61.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label61.Location = New System.Drawing.Point(83, 44)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(18, 24)
        Me.Label61.TabIndex = 522
        Me.Label61.Text = "*"
        '
        'NewProductDisciplinaryForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(447, 391)
        Me.Controls.Add(Me.dgvFindings)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblActionName)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Label61)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewProductDisciplinaryForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Findings for Disciplinary"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvFindings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents btnSave As ToolStripButton
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents btnCancel As ToolStripButton
    Friend WithEvents Label2 As Label
    Friend WithEvents lblActionName As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents txtName As TextBox
    Friend WithEvents dgvFindings As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label61 As Label
    Friend WithEvents c_findingname As DataGridViewTextBoxColumn
    Friend WithEvents c_findingdesc As DataGridViewTextBoxColumn
    Friend WithEvents c_rowid As DataGridViewTextBoxColumn
End Class
