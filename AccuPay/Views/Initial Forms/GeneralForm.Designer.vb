<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GeneralForm
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.UserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OrganizationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BranchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserPrivilegeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GovernmentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SSSTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DutyShiftingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PayRateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AgencyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CalendarsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PanelGeneral = New System.Windows.Forms.Panel()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.Color.Transparent
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UserToolStripMenuItem, Me.OrganizationToolStripMenuItem, Me.BranchToolStripMenuItem, Me.UserPrivilegeToolStripMenuItem, Me.GovernmentToolStripMenuItem, Me.DutyShiftingToolStripMenuItem, Me.PayRateToolStripMenuItem, Me.AgencyToolStripMenuItem, Me.CalendarsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1006, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'UserToolStripMenuItem
        '
        Me.UserToolStripMenuItem.Name = "UserToolStripMenuItem"
        Me.UserToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.UserToolStripMenuItem.Text = "User"
        '
        'OrganizationToolStripMenuItem
        '
        Me.OrganizationToolStripMenuItem.Name = "OrganizationToolStripMenuItem"
        Me.OrganizationToolStripMenuItem.Size = New System.Drawing.Size(87, 20)
        Me.OrganizationToolStripMenuItem.Text = "Organization"
        '
        'BranchToolStripMenuItem
        '
        Me.BranchToolStripMenuItem.Name = "BranchToolStripMenuItem"
        Me.BranchToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.BranchToolStripMenuItem.Text = "Branch"
        '
        'UserPrivilegeToolStripMenuItem
        '
        Me.UserPrivilegeToolStripMenuItem.Name = "UserPrivilegeToolStripMenuItem"
        Me.UserPrivilegeToolStripMenuItem.Size = New System.Drawing.Size(90, 20)
        Me.UserPrivilegeToolStripMenuItem.Text = "User Privilege"
        '
        'GovernmentToolStripMenuItem
        '
        Me.GovernmentToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SSSTableToolStripMenuItem})
        Me.GovernmentToolStripMenuItem.Name = "GovernmentToolStripMenuItem"
        Me.GovernmentToolStripMenuItem.Size = New System.Drawing.Size(85, 20)
        Me.GovernmentToolStripMenuItem.Text = "Government"
        '
        'SSSTableToolStripMenuItem
        '
        Me.SSSTableToolStripMenuItem.Name = "SSSTableToolStripMenuItem"
        Me.SSSTableToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
        Me.SSSTableToolStripMenuItem.Text = "SSS table"
        '
        'DutyShiftingToolStripMenuItem
        '
        Me.DutyShiftingToolStripMenuItem.Name = "DutyShiftingToolStripMenuItem"
        Me.DutyShiftingToolStripMenuItem.Size = New System.Drawing.Size(87, 20)
        Me.DutyShiftingToolStripMenuItem.Text = "Duty shifting"
        Me.DutyShiftingToolStripMenuItem.Visible = False
        '
        'PayRateToolStripMenuItem
        '
        Me.PayRateToolStripMenuItem.Name = "PayRateToolStripMenuItem"
        Me.PayRateToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.PayRateToolStripMenuItem.Text = "Pay rate"
        '
        'AgencyToolStripMenuItem
        '
        Me.AgencyToolStripMenuItem.AccessibleDescription = "Hyundai;Goldwings;Fourlinq"
        Me.AgencyToolStripMenuItem.Name = "AgencyToolStripMenuItem"
        Me.AgencyToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.AgencyToolStripMenuItem.Text = "Agency"
        '
        'CalendarsToolStripMenuItem
        '
        Me.CalendarsToolStripMenuItem.Name = "CalendarsToolStripMenuItem"
        Me.CalendarsToolStripMenuItem.Size = New System.Drawing.Size(71, 20)
        Me.CalendarsToolStripMenuItem.Text = "Calendars"
        '
        'PanelGeneral
        '
        Me.PanelGeneral.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelGeneral.Location = New System.Drawing.Point(0, 24)
        Me.PanelGeneral.Name = "PanelGeneral"
        Me.PanelGeneral.Size = New System.Drawing.Size(1006, 446)
        Me.PanelGeneral.TabIndex = 2
        '
        'GeneralForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1006, 470)
        Me.Controls.Add(Me.PanelGeneral)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "GeneralForm"
        Me.Text = "GeneralForm"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents UserToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OrganizationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UserPrivilegeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PanelGeneral As System.Windows.Forms.Panel
    Friend WithEvents GovernmentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SSSTableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DutyShiftingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PayRateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AgencyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BranchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CalendarsToolStripMenuItem As ToolStripMenuItem
End Class
