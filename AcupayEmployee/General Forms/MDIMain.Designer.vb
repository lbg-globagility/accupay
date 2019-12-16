<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDIMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDIMain))
        Me.Mainmenu = New System.Windows.Forms.MenuStrip()
        Me.GeneralToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserPrevilegeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserAccountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogonToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AuditTrailToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListOfValueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OrganizationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangePasswordToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HRISToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EmployeeSalaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EmployeePreviousEmployersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PFF053FormToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tssTime = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TssUser = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel4 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Tssposition = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel5 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tssProcessingdate = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MainProcessBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Showmainbutton = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Mainbuttons = New System.Windows.Forms.ToolStrip()
        Me.cmdLogIn = New System.Windows.Forms.ToolStripButton()
        Me.cmdCreateUser = New System.Windows.Forms.ToolStripButton()
        Me.cmdChangePassword = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdClose = New System.Windows.Forms.ToolStripButton()
        Me.cmdLogOut = New System.Windows.Forms.ToolStripButton()
        Me.Mainmenu.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.Showmainbutton.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Mainbuttons.SuspendLayout()
        Me.SuspendLayout()
        '
        'Mainmenu
        '
        Me.Mainmenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Mainmenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.Mainmenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GeneralToolStripMenuItem, Me.HRISToolStripMenuItem, Me.ReportsToolStripMenuItem})
        Me.Mainmenu.Location = New System.Drawing.Point(0, 0)
        Me.Mainmenu.Name = "Mainmenu"
        Me.Mainmenu.Size = New System.Drawing.Size(1076, 24)
        Me.Mainmenu.TabIndex = 17
        Me.Mainmenu.Text = "MenuStrip"
        '
        'GeneralToolStripMenuItem
        '
        Me.GeneralToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UserPrevilegeToolStripMenuItem, Me.UserAccountToolStripMenuItem, Me.LogonToolStripMenuItem, Me.AuditTrailToolStripMenuItem, Me.ListOfValueToolStripMenuItem, Me.OrganizationToolStripMenuItem, Me.ChangePasswordToolStripMenuItem, Me.LogoutToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.GeneralToolStripMenuItem.Name = "GeneralToolStripMenuItem"
        Me.GeneralToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.GeneralToolStripMenuItem.Text = "General"
        '
        'UserPrevilegeToolStripMenuItem
        '
        Me.UserPrevilegeToolStripMenuItem.Name = "UserPrevilegeToolStripMenuItem"
        Me.UserPrevilegeToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.UserPrevilegeToolStripMenuItem.Text = "&User Previlege"
        '
        'UserAccountToolStripMenuItem
        '
        Me.UserAccountToolStripMenuItem.Image = Global.PayrollSystem.My.Resources.Resources.userid
        Me.UserAccountToolStripMenuItem.Name = "UserAccountToolStripMenuItem"
        Me.UserAccountToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.UserAccountToolStripMenuItem.Text = "&User Account"
        '
        'LogonToolStripMenuItem
        '
        Me.LogonToolStripMenuItem.Name = "LogonToolStripMenuItem"
        Me.LogonToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.LogonToolStripMenuItem.Text = "&Log-on"
        '
        'AuditTrailToolStripMenuItem
        '
        Me.AuditTrailToolStripMenuItem.Image = Global.PayrollSystem.My.Resources.Resources.audit_trail_icon
        Me.AuditTrailToolStripMenuItem.Name = "AuditTrailToolStripMenuItem"
        Me.AuditTrailToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.AuditTrailToolStripMenuItem.Text = "&Audit Trail"
        '
        'ListOfValueToolStripMenuItem
        '
        Me.ListOfValueToolStripMenuItem.Image = Global.PayrollSystem.My.Resources.Resources.documents7
        Me.ListOfValueToolStripMenuItem.Name = "ListOfValueToolStripMenuItem"
        Me.ListOfValueToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.ListOfValueToolStripMenuItem.Text = "&List of value"
        '
        'OrganizationToolStripMenuItem
        '
        Me.OrganizationToolStripMenuItem.Image = Global.PayrollSystem.My.Resources.Resources.wi0111_481
        Me.OrganizationToolStripMenuItem.Name = "OrganizationToolStripMenuItem"
        Me.OrganizationToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.OrganizationToolStripMenuItem.Text = "&Organization"
        '
        'ChangePasswordToolStripMenuItem
        '
        Me.ChangePasswordToolStripMenuItem.Name = "ChangePasswordToolStripMenuItem"
        Me.ChangePasswordToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.ChangePasswordToolStripMenuItem.Text = "&Change Password"
        '
        'LogoutToolStripMenuItem
        '
        Me.LogoutToolStripMenuItem.Name = "LogoutToolStripMenuItem"
        Me.LogoutToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.LogoutToolStripMenuItem.Text = "&Log-out"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        '
        'HRISToolStripMenuItem
        '
        Me.HRISToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EmployeeSalaryToolStripMenuItem, Me.EmployeePreviousEmployersToolStripMenuItem})
        Me.HRISToolStripMenuItem.Name = "HRISToolStripMenuItem"
        Me.HRISToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HRISToolStripMenuItem.Text = "HRIS"
        '
        'EmployeeSalaryToolStripMenuItem
        '
        Me.EmployeeSalaryToolStripMenuItem.Image = Global.PayrollSystem.My.Resources.Resources.money
        Me.EmployeeSalaryToolStripMenuItem.Name = "EmployeeSalaryToolStripMenuItem"
        Me.EmployeeSalaryToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.EmployeeSalaryToolStripMenuItem.Text = "Employee Salary"
        '
        'EmployeePreviousEmployersToolStripMenuItem
        '
        Me.EmployeePreviousEmployersToolStripMenuItem.Image = Global.PayrollSystem.My.Resources.Resources.User_Group_icon__1_
        Me.EmployeePreviousEmployersToolStripMenuItem.Name = "EmployeePreviousEmployersToolStripMenuItem"
        Me.EmployeePreviousEmployersToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.EmployeePreviousEmployersToolStripMenuItem.Text = "Employee Previous Employer's"
        '
        'ReportsToolStripMenuItem
        '
        Me.ReportsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PFF053FormToolStripMenuItem})
        Me.ReportsToolStripMenuItem.Name = "ReportsToolStripMenuItem"
        Me.ReportsToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.ReportsToolStripMenuItem.Text = "Reports"
        '
        'PFF053FormToolStripMenuItem
        '
        Me.PFF053FormToolStripMenuItem.Image = Global.PayrollSystem.My.Resources.Resources.My_Documents_icon
        Me.PFF053FormToolStripMenuItem.Name = "PFF053FormToolStripMenuItem"
        Me.PFF053FormToolStripMenuItem.Size = New System.Drawing.Size(147, 22)
        Me.PFF053FormToolStripMenuItem.Text = "PFF-053 Form"
        '
        'StatusStrip
        '
        Me.StatusStrip.AutoSize = False
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel, Me.tssTime, Me.ToolStripStatusLabel2, Me.TssUser, Me.ToolStripStatusLabel4, Me.Tssposition, Me.ToolStripStatusLabel5, Me.tssProcessingdate, Me.MainProcessBar, Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel3})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 621)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1076, 25)
        Me.StatusStrip.TabIndex = 21
        Me.StatusStrip.Text = "                                      "
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(38, 20)
        Me.ToolStripStatusLabel.Text = "Time:"
        '
        'tssTime
        '
        Me.tssTime.Name = "tssTime"
        Me.tssTime.Size = New System.Drawing.Size(127, 20)
        Me.tssTime.Text = "                                        "
        Me.tssTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(36, 20)
        Me.ToolStripStatusLabel2.Text = "User:"
        '
        'TssUser
        '
        Me.TssUser.AutoSize = False
        Me.TssUser.Name = "TssUser"
        Me.TssUser.Size = New System.Drawing.Size(200, 20)
        Me.TssUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel4
        '
        Me.ToolStripStatusLabel4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabel4.Name = "ToolStripStatusLabel4"
        Me.ToolStripStatusLabel4.Size = New System.Drawing.Size(55, 20)
        Me.ToolStripStatusLabel4.Text = "Position:"
        '
        'Tssposition
        '
        Me.Tssposition.AutoSize = False
        Me.Tssposition.Name = "Tssposition"
        Me.Tssposition.Size = New System.Drawing.Size(100, 20)
        Me.Tssposition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel5
        '
        Me.ToolStripStatusLabel5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabel5.Name = "ToolStripStatusLabel5"
        Me.ToolStripStatusLabel5.Size = New System.Drawing.Size(88, 20)
        Me.ToolStripStatusLabel5.Text = "Payroll Period:"
        Me.ToolStripStatusLabel5.Visible = False
        '
        'tssProcessingdate
        '
        Me.tssProcessingdate.AutoSize = False
        Me.tssProcessingdate.Name = "tssProcessingdate"
        Me.tssProcessingdate.Size = New System.Drawing.Size(240, 20)
        Me.tssProcessingdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MainProcessBar
        '
        Me.MainProcessBar.AutoSize = False
        Me.MainProcessBar.Maximum = 0
        Me.MainProcessBar.Name = "MainProcessBar"
        Me.MainProcessBar.Size = New System.Drawing.Size(150, 19)
        Me.MainProcessBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(103, 20)
        Me.ToolStripStatusLabel1.Text = "COMPANY NAME"
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(31, 15)
        Me.ToolStripStatusLabel3.Text = "v 1.0"
        '
        'Showmainbutton
        '
        Me.Showmainbutton.AutoSize = False
        Me.Showmainbutton.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Showmainbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Showmainbutton.Dock = System.Windows.Forms.DockStyle.Left
        Me.Showmainbutton.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.Showmainbutton.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.Showmainbutton.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton2})
        Me.Showmainbutton.Location = New System.Drawing.Point(0, 24)
        Me.Showmainbutton.Name = "Showmainbutton"
        Me.Showmainbutton.Size = New System.Drawing.Size(57, 597)
        Me.Showmainbutton.TabIndex = 22
        Me.Showmainbutton.Text = "ToolStrip1"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.Checked = True
        Me.ToolStripButton2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripButton2.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(55, 35)
        Me.ToolStripButton2.Tag = ""
        Me.ToolStripButton2.Text = "Menu"
        Me.ToolStripButton2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Mainbuttons)
        Me.Panel1.Location = New System.Drawing.Point(55, 24)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1019, 597)
        Me.Panel1.TabIndex = 23
        '
        'Mainbuttons
        '
        Me.Mainbuttons.AutoSize = False
        Me.Mainbuttons.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Mainbuttons.Dock = System.Windows.Forms.DockStyle.Left
        Me.Mainbuttons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.Mainbuttons.ImageScalingSize = New System.Drawing.Size(35, 35)
        Me.Mainbuttons.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmdLogIn, Me.cmdCreateUser, Me.cmdChangePassword, Me.ToolStripSeparator2, Me.cmdClose, Me.cmdLogOut})
        Me.Mainbuttons.Location = New System.Drawing.Point(0, 0)
        Me.Mainbuttons.Name = "Mainbuttons"
        Me.Mainbuttons.Size = New System.Drawing.Size(194, 595)
        Me.Mainbuttons.TabIndex = 21
        Me.Mainbuttons.Text = "ToolStrip2"
        '
        'cmdLogIn
        '
        Me.cmdLogIn.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLogIn.Image = CType(resources.GetObject("cmdLogIn.Image"), System.Drawing.Image)
        Me.cmdLogIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdLogIn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdLogIn.Name = "cmdLogIn"
        Me.cmdLogIn.Size = New System.Drawing.Size(192, 39)
        Me.cmdLogIn.Text = "&Log On"
        Me.cmdLogIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdCreateUser
        '
        Me.cmdCreateUser.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCreateUser.Image = CType(resources.GetObject("cmdCreateUser.Image"), System.Drawing.Image)
        Me.cmdCreateUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCreateUser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdCreateUser.Name = "cmdCreateUser"
        Me.cmdCreateUser.Size = New System.Drawing.Size(192, 39)
        Me.cmdCreateUser.Text = "&Users Account"
        Me.cmdCreateUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdChangePassword
        '
        Me.cmdChangePassword.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChangePassword.Image = CType(resources.GetObject("cmdChangePassword.Image"), System.Drawing.Image)
        Me.cmdChangePassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdChangePassword.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdChangePassword.Name = "cmdChangePassword"
        Me.cmdChangePassword.Size = New System.Drawing.Size(192, 39)
        Me.cmdChangePassword.Text = "&Change Password"
        Me.cmdChangePassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(192, 6)
        '
        'cmdClose
        '
        Me.cmdClose.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(192, 39)
        Me.cmdClose.Text = "&Exit"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdLogOut
        '
        Me.cmdLogOut.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLogOut.Image = CType(resources.GetObject("cmdLogOut.Image"), System.Drawing.Image)
        Me.cmdLogOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdLogOut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdLogOut.Name = "cmdLogOut"
        Me.cmdLogOut.Size = New System.Drawing.Size(192, 39)
        Me.cmdLogOut.Text = "&Log Out"
        Me.cmdLogOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MDIMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1076, 646)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Showmainbutton)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.Mainmenu)
        Me.IsMdiContainer = True
        Me.Name = "MDIMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Payroll System"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Mainmenu.ResumeLayout(False)
        Me.Mainmenu.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.Showmainbutton.ResumeLayout(False)
        Me.Showmainbutton.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Mainbuttons.ResumeLayout(False)
        Me.Mainbuttons.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Mainmenu As System.Windows.Forms.MenuStrip
    Friend WithEvents GeneralToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssTime As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents TssUser As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel4 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Tssposition As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel5 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssProcessingdate As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents MainProcessBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Showmainbutton As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Mainbuttons As System.Windows.Forms.ToolStrip
    Friend WithEvents cmdLogIn As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdCreateUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdChangePassword As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmdClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdLogOut As System.Windows.Forms.ToolStripButton
    Friend WithEvents UserPrevilegeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UserAccountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LogonToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListOfValueToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OrganizationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChangePasswordToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LogoutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HRISToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EmployeeSalaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AuditTrailToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EmployeePreviousEmployersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReportsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PFF053FormToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
