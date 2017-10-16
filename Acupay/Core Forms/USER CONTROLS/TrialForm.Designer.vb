<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TrialForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TrialForm))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher()
        Me.DataGridViewX2 = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.AutoCompleteTextBox1 = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.TreeViewAdv1 = New Aga.Controls.Tree.TreeViewAdv()
        Me.TreeColumn1 = New Aga.Controls.Tree.TreeColumn()
        Me.TreeColumn2 = New Aga.Controls.Tree.TreeColumn()
        Me.TreeColumn3 = New Aga.Controls.Tree.TreeColumn()
        Me.TreeColumn4 = New Aga.Controls.Tree.TreeColumn()
        Me.TreeColumn5 = New Aga.Controls.Tree.TreeColumn()
        Me.NodeTextBox1 = New Aga.Controls.Tree.NodeControls.NodeTextBox()
        Me.NodeTextBox2 = New Aga.Controls.Tree.NodeControls.NodeTextBox()
        Me.NodeTextBox3 = New Aga.Controls.Tree.NodeControls.NodeTextBox()
        Me.NodeTextBox4 = New Aga.Controls.Tree.NodeControls.NodeTextBox()
        Me.NodeTextBox5 = New Aga.Controls.Tree.NodeControls.NodeTextBox()
        Me.CustomDatePicker1 = New Acupay.CustomDatePicker()
        Me.DataGridViewX1 = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Column1 = New Acupay.DataGridViewTimeColumn()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewX2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.DataGridViewX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Location = New System.Drawing.Point(194, 91)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(534, 159)
        Me.SplitContainer1.SplitterDistance = 177
        Me.SplitContainer1.TabIndex = 0
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Size = New System.Drawing.Size(353, 159)
        Me.SplitContainer2.SplitterDistance = 174
        Me.SplitContainer2.TabIndex = 0
        '
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'DataGridViewX2
        '
        Me.DataGridViewX2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewX2.DefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewX2.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.DataGridViewX2.Location = New System.Drawing.Point(636, 314)
        Me.DataGridViewX2.Name = "DataGridViewX2"
        Me.DataGridViewX2.Size = New System.Drawing.Size(240, 150)
        Me.DataGridViewX2.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(0, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(0, 91)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(0, 120)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 202
        Me.Button3.Text = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'AutoCompleteTextBox1
        '
        Me.AutoCompleteTextBox1.Location = New System.Drawing.Point(416, 37)
        Me.AutoCompleteTextBox1.Name = "AutoCompleteTextBox1"
        Me.AutoCompleteTextBox1.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.AutoCompleteTextBox1.PopupOffset = New System.Drawing.Point(12, 0)
        Me.AutoCompleteTextBox1.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.AutoCompleteTextBox1.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.AutoCompleteTextBox1.PopupWidth = 300
        Me.AutoCompleteTextBox1.Size = New System.Drawing.Size(100, 20)
        Me.AutoCompleteTextBox1.TabIndex = 203
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton2})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(899, 25)
        Me.ToolStrip1.TabIndex = 0
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton2.Text = "ToolStripButton2"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'TreeViewAdv1
        '
        Me.TreeViewAdv1.BackColor = System.Drawing.SystemColors.Window
        Me.TreeViewAdv1.Columns.Add(Me.TreeColumn1)
        Me.TreeViewAdv1.Columns.Add(Me.TreeColumn2)
        Me.TreeViewAdv1.Columns.Add(Me.TreeColumn3)
        Me.TreeViewAdv1.Columns.Add(Me.TreeColumn4)
        Me.TreeViewAdv1.Columns.Add(Me.TreeColumn5)
        Me.TreeViewAdv1.DefaultToolTipProvider = Nothing
        Me.TreeViewAdv1.DragDropMarkColor = System.Drawing.Color.Black
        Me.TreeViewAdv1.LineColor = System.Drawing.SystemColors.ControlDark
        Me.TreeViewAdv1.Location = New System.Drawing.Point(36, 389)
        Me.TreeViewAdv1.Model = Nothing
        Me.TreeViewAdv1.Name = "TreeViewAdv1"
        Me.TreeViewAdv1.NodeControls.Add(Me.NodeTextBox1)
        Me.TreeViewAdv1.NodeControls.Add(Me.NodeTextBox2)
        Me.TreeViewAdv1.NodeControls.Add(Me.NodeTextBox3)
        Me.TreeViewAdv1.NodeControls.Add(Me.NodeTextBox4)
        Me.TreeViewAdv1.NodeControls.Add(Me.NodeTextBox5)
        Me.TreeViewAdv1.SelectedNode = Nothing
        Me.TreeViewAdv1.Size = New System.Drawing.Size(290, 71)
        Me.TreeViewAdv1.TabIndex = 204
        Me.TreeViewAdv1.Text = "TreeViewAdv1"
        '
        'TreeColumn1
        '
        Me.TreeColumn1.Header = ""
        Me.TreeColumn1.SortOrder = System.Windows.Forms.SortOrder.None
        Me.TreeColumn1.TooltipText = Nothing
        '
        'TreeColumn2
        '
        Me.TreeColumn2.Header = ""
        Me.TreeColumn2.SortOrder = System.Windows.Forms.SortOrder.None
        Me.TreeColumn2.TooltipText = Nothing
        '
        'TreeColumn3
        '
        Me.TreeColumn3.Header = ""
        Me.TreeColumn3.SortOrder = System.Windows.Forms.SortOrder.None
        Me.TreeColumn3.TooltipText = Nothing
        '
        'TreeColumn4
        '
        Me.TreeColumn4.Header = ""
        Me.TreeColumn4.SortOrder = System.Windows.Forms.SortOrder.None
        Me.TreeColumn4.TooltipText = Nothing
        '
        'TreeColumn5
        '
        Me.TreeColumn5.Header = ""
        Me.TreeColumn5.SortOrder = System.Windows.Forms.SortOrder.None
        Me.TreeColumn5.TooltipText = Nothing
        '
        'NodeTextBox1
        '
        Me.NodeTextBox1.IncrementalSearchEnabled = True
        Me.NodeTextBox1.LeftMargin = 3
        Me.NodeTextBox1.ParentColumn = Nothing
        '
        'NodeTextBox2
        '
        Me.NodeTextBox2.IncrementalSearchEnabled = True
        Me.NodeTextBox2.LeftMargin = 3
        Me.NodeTextBox2.ParentColumn = Nothing
        '
        'NodeTextBox3
        '
        Me.NodeTextBox3.IncrementalSearchEnabled = True
        Me.NodeTextBox3.LeftMargin = 3
        Me.NodeTextBox3.ParentColumn = Nothing
        '
        'NodeTextBox4
        '
        Me.NodeTextBox4.IncrementalSearchEnabled = True
        Me.NodeTextBox4.LeftMargin = 3
        Me.NodeTextBox4.ParentColumn = Nothing
        '
        'NodeTextBox5
        '
        Me.NodeTextBox5.IncrementalSearchEnabled = True
        Me.NodeTextBox5.LeftMargin = 3
        Me.NodeTextBox5.ParentColumn = Nothing
        '
        'CustomDatePicker1
        '
        Me.CustomDatePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.CustomDatePicker1.Location = New System.Drawing.Point(24, 314)
        Me.CustomDatePicker1.Name = "CustomDatePicker1"
        Me.CustomDatePicker1.Size = New System.Drawing.Size(117, 20)
        Me.CustomDatePicker1.TabIndex = 4
        Me.CustomDatePicker1.Tag = "2016-08-11"
        Me.CustomDatePicker1.Value = New Date(2016, 8, 11, 0, 0, 0, 0)
        '
        'DataGridViewX1
        '
        Me.DataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewX1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewX1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewX1.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.DataGridViewX1.Location = New System.Drawing.Point(375, 314)
        Me.DataGridViewX1.Name = "DataGridViewX1"
        Me.DataGridViewX1.Size = New System.Drawing.Size(240, 150)
        Me.DataGridViewX1.TabIndex = 1
        '
        'Column1
        '
        Me.Column1.HeaderText = "Column1"
        Me.Column1.Name = "Column1"
        '
        'TrialForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(899, 472)
        Me.Controls.Add(Me.TreeViewAdv1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.AutoCompleteTextBox1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.CustomDatePicker1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DataGridViewX2)
        Me.Controls.Add(Me.DataGridViewX1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "TrialForm"
        Me.Text = "TrialForm"
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewX2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.DataGridViewX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents DataGridViewX1 As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents FileSystemWatcher1 As System.IO.FileSystemWatcher
    Friend WithEvents Column1 As Acupay.DataGridViewTimeColumn
    Friend WithEvents DataGridViewX2 As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CustomDatePicker1 As Acupay.CustomDatePicker
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents AutoCompleteTextBox1 As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Private WithEvents TreeViewAdv1 As Aga.Controls.Tree.TreeViewAdv
    Friend WithEvents NodeTextBox1 As Aga.Controls.Tree.NodeControls.NodeTextBox
    Friend WithEvents NodeTextBox2 As Aga.Controls.Tree.NodeControls.NodeTextBox
    Friend WithEvents NodeTextBox3 As Aga.Controls.Tree.NodeControls.NodeTextBox
    Friend WithEvents NodeTextBox4 As Aga.Controls.Tree.NodeControls.NodeTextBox
    Friend WithEvents NodeTextBox5 As Aga.Controls.Tree.NodeControls.NodeTextBox
    Friend WithEvents TreeColumn1 As Aga.Controls.Tree.TreeColumn
    Friend WithEvents TreeColumn2 As Aga.Controls.Tree.TreeColumn
    Friend WithEvents TreeColumn3 As Aga.Controls.Tree.TreeColumn
    Friend WithEvents TreeColumn4 As Aga.Controls.Tree.TreeColumn
    Friend WithEvents TreeColumn5 As Aga.Controls.Tree.TreeColumn
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
End Class
