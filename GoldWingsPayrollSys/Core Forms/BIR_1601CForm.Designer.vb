<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BIR_1601CForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BIR_1601CForm))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnDiscard = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.AutoCompleteTextBox1 = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.CrystalReportViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnReload = New System.Windows.Forms.ToolStripButton()
        Me.bgwEmployeeID = New System.ComponentModel.BackgroundWorker()
        Me.bgReport = New System.ComponentModel.BackgroundWorker()
        Me.bgLoadAlphaList = New System.ComponentModel.BackgroundWorker()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.CrystalReportViewer1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ToolStrip1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1138, 500)
        Me.SplitContainer1.SplitterDistance = 379
        Me.SplitContainer1.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 45)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(379, 410)
        Me.Panel2.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnDiscard)
        Me.Panel3.Controls.Add(Me.btnApply)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 455)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(379, 45)
        Me.Panel3.TabIndex = 2
        '
        'btnDiscard
        '
        Me.btnDiscard.Location = New System.Drawing.Point(192, 11)
        Me.btnDiscard.Name = "btnDiscard"
        Me.btnDiscard.Size = New System.Drawing.Size(95, 23)
        Me.btnDiscard.TabIndex = 3
        Me.btnDiscard.Text = "Discard changes"
        Me.btnDiscard.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(91, 11)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(95, 23)
        Me.btnApply.TabIndex = 2
        Me.btnApply.Text = "&Save changes"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.AutoCompleteTextBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(379, 45)
        Me.Panel1.TabIndex = 0
        '
        'AutoCompleteTextBox1
        '
        Me.AutoCompleteTextBox1.Enabled = False
        Me.AutoCompleteTextBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AutoCompleteTextBox1.Location = New System.Drawing.Point(133, 11)
        Me.AutoCompleteTextBox1.Name = "AutoCompleteTextBox1"
        Me.AutoCompleteTextBox1.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.AutoCompleteTextBox1.PopupOffset = New System.Drawing.Point(12, 0)
        Me.AutoCompleteTextBox1.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.AutoCompleteTextBox1.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.AutoCompleteTextBox1.PopupWidth = 300
        Me.AutoCompleteTextBox1.Size = New System.Drawing.Size(156, 25)
        Me.AutoCompleteTextBox1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(46, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Employee ID"
        '
        'btnSearch
        '
        Me.btnSearch.Image = CType(resources.GetObject("btnSearch.Image"), System.Drawing.Image)
        Me.btnSearch.Location = New System.Drawing.Point(295, 11)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(25, 25)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'CrystalReportViewer1
        '
        Me.CrystalReportViewer1.ActiveViewIndex = -1
        Me.CrystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CrystalReportViewer1.CachedPageNumberPerDoc = 10
        Me.CrystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default
        Me.CrystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CrystalReportViewer1.Location = New System.Drawing.Point(0, 25)
        Me.CrystalReportViewer1.Name = "CrystalReportViewer1"
        Me.CrystalReportViewer1.Size = New System.Drawing.Size(755, 475)
        Me.CrystalReportViewer1.TabIndex = 0
        Me.CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnReload})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(755, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnReload
        '
        Me.tsbtnReload.Enabled = False
        Me.tsbtnReload.Image = CType(resources.GetObject("tsbtnReload.Image"), System.Drawing.Image)
        Me.tsbtnReload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnReload.Name = "tsbtnReload"
        Me.tsbtnReload.Size = New System.Drawing.Size(133, 22)
        Me.tsbtnReload.Text = "&Reload changes (F5)"
        '
        'bgwEmployeeID
        '
        Me.bgwEmployeeID.WorkerReportsProgress = True
        Me.bgwEmployeeID.WorkerSupportsCancellation = True
        '
        'bgReport
        '
        Me.bgReport.WorkerReportsProgress = True
        Me.bgReport.WorkerSupportsCancellation = True
        '
        'bgLoadAlphaList
        '
        Me.bgLoadAlphaList.WorkerReportsProgress = True
        Me.bgLoadAlphaList.WorkerSupportsCancellation = True
        '
        'BIR_1601CForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1138, 500)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "BIR_1601CForm"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents CrystalReportViewer1 As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents AutoCompleteTextBox1 As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents tsbtnReload As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDiscard As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents bgwEmployeeID As System.ComponentModel.BackgroundWorker
    Friend WithEvents bgReport As System.ComponentModel.BackgroundWorker
    Friend WithEvents bgLoadAlphaList As System.ComponentModel.BackgroundWorker
End Class
