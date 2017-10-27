<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JobLevelForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tbpPosition = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CategoryNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.JobLevelDataGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.NewCategoryButton = New System.Windows.Forms.ToolStripButton()
        Me.SaveCategoryButton = New System.Windows.Forms.ToolStripButton()
        Me.DeleteCategoryButton = New System.Windows.Forms.ToolStripButton()
        Me.CancelCategoryButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.tv2 = New System.Windows.Forms.TreeView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TabControl1.SuspendLayout()
        Me.tbpPosition.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.JobLevelDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.tbpPosition)
        Me.TabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.TabControl1.ItemSize = New System.Drawing.Size(62, 25)
        Me.TabControl1.Location = New System.Drawing.Point(208, 8)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(808, 496)
        Me.TabControl1.TabIndex = 105
        '
        'tbpPosition
        '
        Me.tbpPosition.AutoScroll = True
        Me.tbpPosition.Controls.Add(Me.Panel1)
        Me.tbpPosition.Controls.Add(Me.ToolStrip1)
        Me.tbpPosition.Location = New System.Drawing.Point(4, 4)
        Me.tbpPosition.Name = "tbpPosition"
        Me.tbpPosition.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpPosition.Size = New System.Drawing.Size(800, 463)
        Me.tbpPosition.TabIndex = 0
        Me.tbpPosition.Text = "POSITION               "
        Me.tbpPosition.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.CategoryNameTextBox)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.JobLevelDataGridView)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(794, 432)
        Me.Panel1.TabIndex = 108
        '
        'CategoryNameTextBox
        '
        Me.CategoryNameTextBox.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CategoryNameTextBox.Location = New System.Drawing.Point(160, 16)
        Me.CategoryNameTextBox.Name = "CategoryNameTextBox"
        Me.CategoryNameTextBox.Size = New System.Drawing.Size(344, 23)
        Me.CategoryNameTextBox.TabIndex = 183
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(8, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(144, 20)
        Me.Label6.TabIndex = 182
        Me.Label6.Text = "Division Location"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'JobLevelDataGridView
        '
        Me.JobLevelDataGridView.AllowUserToAddRows = False
        Me.JobLevelDataGridView.AllowUserToDeleteRows = False
        Me.JobLevelDataGridView.AllowUserToOrderColumns = True
        Me.JobLevelDataGridView.BackgroundColor = System.Drawing.Color.White
        Me.JobLevelDataGridView.ColumnHeadersHeight = 34
        Me.JobLevelDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.JobLevelDataGridView.DefaultCellStyle = DataGridViewCellStyle1
        Me.JobLevelDataGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.JobLevelDataGridView.Location = New System.Drawing.Point(8, 48)
        Me.JobLevelDataGridView.MultiSelect = False
        Me.JobLevelDataGridView.Name = "JobLevelDataGridView"
        Me.JobLevelDataGridView.ReadOnly = True
        Me.JobLevelDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.JobLevelDataGridView.Size = New System.Drawing.Size(496, 376)
        Me.JobLevelDataGridView.TabIndex = 175
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewCategoryButton, Me.SaveCategoryButton, Me.DeleteCategoryButton, Me.CancelCategoryButton, Me.ToolStripButton4, Me.tsbtnAudittrail})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(794, 25)
        Me.ToolStrip1.TabIndex = 13
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'NewCategoryButton
        '
        Me.NewCategoryButton.Image = Global.Acupay.My.Resources.Resources._new
        Me.NewCategoryButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewCategoryButton.Name = "NewCategoryButton"
        Me.NewCategoryButton.Size = New System.Drawing.Size(102, 22)
        Me.NewCategoryButton.Text = "&New Category"
        '
        'SaveCategoryButton
        '
        Me.SaveCategoryButton.Image = Global.Acupay.My.Resources.Resources.Save
        Me.SaveCategoryButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveCategoryButton.Name = "SaveCategoryButton"
        Me.SaveCategoryButton.Size = New System.Drawing.Size(102, 22)
        Me.SaveCategoryButton.Text = "&Save Category"
        '
        'DeleteCategoryButton
        '
        Me.DeleteCategoryButton.Image = Global.Acupay.My.Resources.Resources.CLOSE_00
        Me.DeleteCategoryButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteCategoryButton.Name = "DeleteCategoryButton"
        Me.DeleteCategoryButton.Size = New System.Drawing.Size(111, 22)
        Me.DeleteCategoryButton.Text = "D&elete Category"
        '
        'CancelCategoryButton
        '
        Me.CancelCategoryButton.Image = Global.Acupay.My.Resources.Resources.cancel1
        Me.CancelCategoryButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CancelCategoryButton.Name = "CancelCategoryButton"
        Me.CancelCategoryButton.Size = New System.Drawing.Size(63, 22)
        Me.CancelCategoryButton.Text = "Cancel"
        '
        'ToolStripButton4
        '
        Me.ToolStripButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton4.Image = Global.Acupay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton4.Name = "ToolStripButton4"
        Me.ToolStripButton4.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton4.Text = "Close"
        '
        'tsbtnAudittrail
        '
        Me.tsbtnAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnAudittrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnAudittrail.Image = Global.Acupay.My.Resources.Resources.audit_trail_icon
        Me.tsbtnAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnAudittrail.Name = "tsbtnAudittrail"
        Me.tsbtnAudittrail.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnAudittrail.Text = "ToolStripButton1"
        Me.tsbtnAudittrail.ToolTipText = "Show audit trails"
        '
        'tv2
        '
        Me.tv2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tv2.ForeColor = System.Drawing.Color.Black
        Me.tv2.HideSelection = False
        Me.tv2.Location = New System.Drawing.Point(8, 8)
        Me.tv2.Name = "tv2"
        Me.tv2.Size = New System.Drawing.Size(192, 496)
        Me.tv2.TabIndex = 106
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "RowID"
        Me.Column1.HeaderText = "RowID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "Name"
        Me.Column2.HeaderText = "Name"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'JobLevelForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1024, 512)
        Me.Controls.Add(Me.tv2)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "JobLevelForm"
        Me.Text = "JobLevelForm"
        Me.TabControl1.ResumeLayout(False)
        Me.tbpPosition.ResumeLayout(False)
        Me.tbpPosition.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.JobLevelDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tbpPosition As TabPage
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents JobLevelDataGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents NewCategoryButton As ToolStripButton
    Friend WithEvents SaveCategoryButton As ToolStripButton
    Friend WithEvents DeleteCategoryButton As ToolStripButton
    Friend WithEvents CancelCategoryButton As ToolStripButton
    Friend WithEvents ToolStripButton4 As ToolStripButton
    Friend WithEvents tsbtnAudittrail As ToolStripButton
    Friend WithEvents tv2 As TreeView
    Friend WithEvents CategoryNameTextBox As TextBox
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
End Class
