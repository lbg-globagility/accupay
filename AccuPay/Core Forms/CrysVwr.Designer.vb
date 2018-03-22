<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CrysVwr
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
        Me.CrystalReportViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnExclExport = New System.Windows.Forms.Button()
        Me.cboxExportTypeOptn = New System.Windows.Forms.ComboBox()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CrystalReportViewer1
        '
        Me.CrystalReportViewer1.ActiveViewIndex = -1
        Me.CrystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CrystalReportViewer1.CachedPageNumberPerDoc = 10
        Me.CrystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default
        Me.CrystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CrystalReportViewer1.Location = New System.Drawing.Point(0, 32)
        Me.CrystalReportViewer1.Name = "CrystalReportViewer1"
        Me.CrystalReportViewer1.Size = New System.Drawing.Size(793, 384)
        Me.CrystalReportViewer1.TabIndex = 0
        Me.CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cboxExportTypeOptn)
        Me.Panel1.Controls.Add(Me.btnExclExport)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(793, 32)
        Me.Panel1.TabIndex = 1
        Me.Panel1.Visible = False
        '
        'btnExclExport
        '
        Me.btnExclExport.Location = New System.Drawing.Point(4, 5)
        Me.btnExclExport.Name = "btnExclExport"
        Me.btnExclExport.Size = New System.Drawing.Size(86, 23)
        Me.btnExclExport.TabIndex = 0
        Me.btnExclExport.Text = "Export to Excel"
        Me.btnExclExport.UseVisualStyleBackColor = True
        '
        'cboxExportTypeOptn
        '
        Me.cboxExportTypeOptn.FormattingEnabled = True
        Me.cboxExportTypeOptn.Location = New System.Drawing.Point(291, 5)
        Me.cboxExportTypeOptn.Name = "cboxExportTypeOptn"
        Me.cboxExportTypeOptn.Size = New System.Drawing.Size(121, 21)
        Me.cboxExportTypeOptn.TabIndex = 1
        '
        'CrysVwr
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(793, 416)
        Me.Controls.Add(Me.CrystalReportViewer1)
        Me.Controls.Add(Me.Panel1)
        Me.KeyPreview = True
        Me.Name = "CrysVwr"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CrystalReportViewer1 As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnExclExport As System.Windows.Forms.Button
    Friend WithEvents cboxExportTypeOptn As System.Windows.Forms.ComboBox
End Class
