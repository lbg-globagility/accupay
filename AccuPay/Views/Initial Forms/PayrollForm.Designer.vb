<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PayrollForm
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.PayrollToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BonusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WithholdingTaxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PaystubExperimentalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PanelPayroll = New System.Windows.Forms.Panel()
        Me.BenchmarkPaystubToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AllowanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.Color.Transparent
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AllowanceToolStripMenuItem, Me.LoanToolStripMenuItem, Me.PayrollToolStripMenuItem, Me.BonusToolStripMenuItem, Me.WithholdingTaxToolStripMenuItem, Me.PaystubExperimentalToolStripMenuItem, Me.BenchmarkPaystubToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1006, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'PayrollToolStripMenuItem
        '
        Me.PayrollToolStripMenuItem.Name = "PayrollToolStripMenuItem"
        Me.PayrollToolStripMenuItem.Size = New System.Drawing.Size(55, 20)
        Me.PayrollToolStripMenuItem.Text = "Payroll"
        '
        'BonusToolStripMenuItem
        '
        Me.BonusToolStripMenuItem.Name = "BonusToolStripMenuItem"
        Me.BonusToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.BonusToolStripMenuItem.Text = "Bonus"
        '
        'WithholdingTaxToolStripMenuItem
        '
        Me.WithholdingTaxToolStripMenuItem.Name = "WithholdingTaxToolStripMenuItem"
        Me.WithholdingTaxToolStripMenuItem.Size = New System.Drawing.Size(105, 20)
        Me.WithholdingTaxToolStripMenuItem.Text = "Withholding Tax"
        '
        'PaystubExperimentalToolStripMenuItem
        '
        Me.PaystubExperimentalToolStripMenuItem.Name = "PaystubExperimentalToolStripMenuItem"
        Me.PaystubExperimentalToolStripMenuItem.Size = New System.Drawing.Size(140, 20)
        Me.PaystubExperimentalToolStripMenuItem.Text = "Paystub (Experimental)"
        '
        'PanelPayroll
        '
        Me.PanelPayroll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelPayroll.Location = New System.Drawing.Point(0, 24)
        Me.PanelPayroll.Name = "PanelPayroll"
        Me.PanelPayroll.Size = New System.Drawing.Size(1006, 446)
        Me.PanelPayroll.TabIndex = 3
        '
        'AllowanceToolStripMenuItem
        '
        Me.AllowanceToolStripMenuItem.Name = "AllowanceToolStripMenuItem"
        Me.AllowanceToolStripMenuItem.Size = New System.Drawing.Size(74, 20)
        Me.AllowanceToolStripMenuItem.Text = "Allowance"
        '
        'LoanToolStripMenuItem
        '
        Me.LoanToolStripMenuItem.Name = "LoanToolStripMenuItem"
        Me.LoanToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.LoanToolStripMenuItem.Text = "Loans"
        '
        'BenchmarkPaystubToolStripMenuItem
        '
        Me.BenchmarkPaystubToolStripMenuItem.Name = "BenchmarkPaystubToolStripMenuItem"
        Me.BenchmarkPaystubToolStripMenuItem.Size = New System.Drawing.Size(66, 20)
        Me.BenchmarkPaystubToolStripMenuItem.Text = "Paystubs"
        '
        'PayrollForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1006, 470)
        Me.Controls.Add(Me.PanelPayroll)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "PayrollForm"
        Me.Text = "PayrollForm"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents PayrollToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PanelPayroll As System.Windows.Forms.Panel
    Friend WithEvents BonusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WithholdingTaxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PaystubExperimentalToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AllowanceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoanToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BenchmarkPaystubToolStripMenuItem As ToolStripMenuItem
End Class
