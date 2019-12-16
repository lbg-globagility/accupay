<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BranchForm
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvbranch = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.brRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.brCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.brName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ChangeIndicator = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewBranch = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveBranch = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel9 = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnDelBranch = New System.Windows.Forms.ToolStripButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SearchPanel = New System.Windows.Forms.Panel()
        Me.txtSearchBox = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.FirstLink = New System.Windows.Forms.LinkLabel()
        Me.NextLink = New System.Windows.Forms.LinkLabel()
        Me.PrevLink = New System.Windows.Forms.LinkLabel()
        Me.LastLink = New System.Windows.Forms.LinkLabel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bgworksearchautocompleter = New System.ComponentModel.BackgroundWorker()
        CType(Me.dgvbranch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SearchPanel.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvbranch
        '
        Me.dgvbranch.AllowUserToDeleteRows = False
        Me.dgvbranch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvbranch.BackgroundColor = System.Drawing.Color.White
        Me.dgvbranch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvbranch.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.brRowID, Me.brCode, Me.brName, Me.ChangeIndicator})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvbranch.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvbranch.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvbranch.Location = New System.Drawing.Point(3, 54)
        Me.dgvbranch.MultiSelect = False
        Me.dgvbranch.Name = "dgvbranch"
        Me.dgvbranch.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvbranch.Size = New System.Drawing.Size(350, 340)
        Me.dgvbranch.TabIndex = 0
        '
        'brRowID
        '
        Me.brRowID.HeaderText = "RowID"
        Me.brRowID.Name = "brRowID"
        Me.brRowID.ReadOnly = True
        Me.brRowID.Visible = False
        '
        'brCode
        '
        Me.brCode.HeaderText = "Code"
        Me.brCode.MaxInputLength = 100
        Me.brCode.Name = "brCode"
        '
        'brName
        '
        Me.brName.HeaderText = "Name"
        Me.brName.MaxInputLength = 100
        Me.brName.Name = "brName"
        Me.brName.Width = 180
        '
        'ChangeIndicator
        '
        Me.ChangeIndicator.HeaderText = "HasChanges"
        Me.ChangeIndicator.Name = "ChangeIndicator"
        Me.ChangeIndicator.ReadOnly = True
        Me.ChangeIndicator.Visible = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewBranch, Me.tsbtnSaveBranch, Me.tsbtnCancel, Me.tsbtnClose, Me.ToolStripLabel9, Me.tsbtnDelBranch})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(515, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNewBranch
        '
        Me.tsbtnNewBranch.Image = Global.RBXPayrollSys.My.Resources.Resources._new
        Me.tsbtnNewBranch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewBranch.Name = "tsbtnNewBranch"
        Me.tsbtnNewBranch.Size = New System.Drawing.Size(91, 22)
        Me.tsbtnNewBranch.Text = "&New Branch"
        '
        'tsbtnSaveBranch
        '
        Me.tsbtnSaveBranch.Image = Global.RBXPayrollSys.My.Resources.Resources.Save
        Me.tsbtnSaveBranch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveBranch.Name = "tsbtnSaveBranch"
        Me.tsbtnSaveBranch.Size = New System.Drawing.Size(91, 22)
        Me.tsbtnSaveBranch.Text = "&Save Branch"
        '
        'tsbtnCancel
        '
        Me.tsbtnCancel.Image = Global.RBXPayrollSys.My.Resources.Resources.cancel1
        Me.tsbtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancel.Name = "tsbtnCancel"
        Me.tsbtnCancel.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancel.Text = "Cancel"
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnClose.Image = Global.RBXPayrollSys.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnClose.Name = "tsbtnClose"
        Me.tsbtnClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnClose.Text = "Close"
        '
        'ToolStripLabel9
        '
        Me.ToolStripLabel9.Name = "ToolStripLabel9"
        Me.ToolStripLabel9.Size = New System.Drawing.Size(52, 22)
        Me.ToolStripLabel9.Text = "               "
        '
        'tsbtnDelBranch
        '
        Me.tsbtnDelBranch.Image = Global.RBXPayrollSys.My.Resources.Resources.CLOSE_00
        Me.tsbtnDelBranch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDelBranch.Name = "tsbtnDelBranch"
        Me.tsbtnDelBranch.Size = New System.Drawing.Size(100, 22)
        Me.tsbtnDelBranch.Text = "Delete Branch"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.SearchPanel)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.dgvbranch)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(356, 427)
        Me.Panel1.TabIndex = 2
        '
        'SearchPanel
        '
        Me.SearchPanel.Controls.Add(Me.txtSearchBox)
        Me.SearchPanel.Controls.Add(Me.btnRefresh)
        Me.SearchPanel.Location = New System.Drawing.Point(12, 17)
        Me.SearchPanel.Name = "SearchPanel"
        Me.SearchPanel.Size = New System.Drawing.Size(228, 23)
        Me.SearchPanel.TabIndex = 204
        '
        'txtSearchBox
        '
        Me.txtSearchBox.Location = New System.Drawing.Point(0, 2)
        Me.txtSearchBox.Name = "txtSearchBox"
        Me.txtSearchBox.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSearchBox.PopupOffset = New System.Drawing.Point(12, 0)
        Me.txtSearchBox.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.txtSearchBox.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.txtSearchBox.PopupWidth = 300
        Me.txtSearchBox.Size = New System.Drawing.Size(150, 20)
        Me.txtSearchBox.TabIndex = 204
        '
        'btnRefresh
        '
        Me.btnRefresh.AutoEllipsis = True
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.Location = New System.Drawing.Point(156, 0)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(26, 23)
        Me.btnRefresh.TabIndex = 203
        Me.btnRefresh.Text = "Search"
        Me.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 400)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Label1"
        Me.Label1.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.FirstLink)
        Me.Panel2.Controls.Add(Me.NextLink)
        Me.Panel2.Controls.Add(Me.PrevLink)
        Me.Panel2.Controls.Add(Me.LastLink)
        Me.Panel2.Location = New System.Drawing.Point(89, 400)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(181, 27)
        Me.Panel2.TabIndex = 3
        '
        'FirstLink
        '
        Me.FirstLink.AutoSize = True
        Me.FirstLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FirstLink.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.FirstLink.Location = New System.Drawing.Point(0, 0)
        Me.FirstLink.Name = "FirstLink"
        Me.FirstLink.Size = New System.Drawing.Size(44, 15)
        Me.FirstLink.TabIndex = 279
        Me.FirstLink.TabStop = True
        Me.FirstLink.Text = "<<First"
        '
        'NextLink
        '
        Me.NextLink.AutoSize = True
        Me.NextLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NextLink.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.NextLink.Location = New System.Drawing.Point(95, 0)
        Me.NextLink.Name = "NextLink"
        Me.NextLink.Size = New System.Drawing.Size(39, 15)
        Me.NextLink.TabIndex = 281
        Me.NextLink.TabStop = True
        Me.NextLink.Text = "Next>"
        '
        'PrevLink
        '
        Me.PrevLink.AutoSize = True
        Me.PrevLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PrevLink.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PrevLink.Location = New System.Drawing.Point(50, 0)
        Me.PrevLink.Name = "PrevLink"
        Me.PrevLink.Size = New System.Drawing.Size(38, 15)
        Me.PrevLink.TabIndex = 280
        Me.PrevLink.TabStop = True
        Me.PrevLink.Text = "<Prev"
        '
        'LastLink
        '
        Me.LastLink.AutoSize = True
        Me.LastLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LastLink.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LastLink.Location = New System.Drawing.Point(140, 0)
        Me.LastLink.Name = "LastLink"
        Me.LastLink.Size = New System.Drawing.Size(44, 15)
        Me.LastLink.TabIndex = 282
        Me.LastLink.TabStop = True
        Me.LastLink.Text = "Last>>"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Branch Code"
        Me.DataGridViewTextBoxColumn2.MaxInputLength = 100
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Branch Name"
        Me.DataGridViewTextBoxColumn3.MaxInputLength = 100
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 180
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "HasChanges"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Visible = False
        '
        'bgworksearchautocompleter
        '
        Me.bgworksearchautocompleter.WorkerReportsProgress = True
        Me.bgworksearchautocompleter.WorkerSupportsCancellation = True
        '
        'BranchForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(515, 452)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.KeyPreview = True
        Me.Name = "BranchForm"
        CType(Me.dgvbranch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.SearchPanel.ResumeLayout(False)
        Me.SearchPanel.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvbranch As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents tsbtnNewBranch As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSaveBranch As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel9 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbtnDelBranch As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents FirstLink As System.Windows.Forms.LinkLabel
    Friend WithEvents PrevLink As System.Windows.Forms.LinkLabel
    Friend WithEvents LastLink As System.Windows.Forms.LinkLabel
    Friend WithEvents NextLink As System.Windows.Forms.LinkLabel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents bgworksearchautocompleter As System.ComponentModel.BackgroundWorker
    Friend WithEvents SearchPanel As System.Windows.Forms.Panel
    Friend WithEvents txtSearchBox As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents brRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents brCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents brName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ChangeIndicator As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
