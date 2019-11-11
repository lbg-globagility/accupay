<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.RibbonControlHome = New DevComponents.DotNetBar.RibbonControl()
        Me.RibbonPanel2 = New DevComponents.DotNetBar.RibbonPanel()
        Me.btnDisciplinaryaction = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnLoadSched = New System.Windows.Forms.Button()
        Me.btntax = New System.Windows.Forms.Button()
        Me.hbtnDivision = New System.Windows.Forms.Button()
        Me.hbtnEducationbGround = New System.Windows.Forms.Button()
        Me.hbtnPrevEmployer = New System.Windows.Forms.Button()
        Me.RibbonPanel3 = New DevComponents.DotNetBar.RibbonPanel()
        Me.btnShiftEntry = New System.Windows.Forms.Button()
        Me.RibbonPanel1 = New DevComponents.DotNetBar.RibbonPanel()
        Me.btnUserPrevilige = New System.Windows.Forms.Button()
        Me.btnOrganization = New System.Windows.Forms.Button()
        Me.btnListOfval = New System.Windows.Forms.Button()
        Me.btnUsersform = New System.Windows.Forms.Button()
        Me.RibbonTabItem1 = New DevComponents.DotNetBar.RibbonTabItem()
        Me.RibbonTabItem2 = New DevComponents.DotNetBar.RibbonTabItem()
        Me.RibbonTabItem3 = New DevComponents.DotNetBar.RibbonTabItem()
        Me.ButtonItem1 = New DevComponents.DotNetBar.ButtonItem()
        Me.QatCustomizeItem1 = New DevComponents.DotNetBar.QatCustomizeItem()
        Me.StyleManager1 = New DevComponents.DotNetBar.StyleManager(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblTime = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblUser = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel5 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblPosition = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MainProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnTimeENtry = New System.Windows.Forms.Button()
        Me.hbtnEmpSalary = New System.Windows.Forms.Button()
        Me.HomeButton = New DevComponents.DotNetBar.Office2007StartButton()
        Me.btnTimeAttendance = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem3 = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem4 = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem10 = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem11 = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem12 = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem13 = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem14 = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem2 = New DevComponents.DotNetBar.ButtonItem()
        Me.hbtnUsers = New DevComponents.DotNetBar.ButtonItem()
        Me.hbtnListofval = New DevComponents.DotNetBar.ButtonItem()
        Me.hbtnorganization = New DevComponents.DotNetBar.ButtonItem()
        Me.hbtnaudittrail = New DevComponents.DotNetBar.ButtonItem()
        Me.hbtnChangePassword = New DevComponents.DotNetBar.ButtonItem()
        Me.btnbackup = New DevComponents.DotNetBar.ButtonItem()
        Me.hbtnexit = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem16 = New DevComponents.DotNetBar.ButtonItem()
        Me.GroupTimer = New System.Windows.Forms.Timer(Me.components)
        Me.RibbonControlHome.SuspendLayout()
        Me.RibbonPanel2.SuspendLayout()
        Me.RibbonPanel3.SuspendLayout()
        Me.RibbonPanel1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'RibbonControlHome
        '
        '
        '
        '
        Me.RibbonControlHome.BackgroundStyle.Class = ""
        Me.RibbonControlHome.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.RibbonControlHome.CaptionVisible = True
        Me.RibbonControlHome.Controls.Add(Me.RibbonPanel3)
        Me.RibbonControlHome.Controls.Add(Me.RibbonPanel2)
        Me.RibbonControlHome.Controls.Add(Me.RibbonPanel1)
        Me.RibbonControlHome.Dock = System.Windows.Forms.DockStyle.Top
        Me.RibbonControlHome.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RibbonControlHome.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.RibbonTabItem1, Me.RibbonTabItem2, Me.RibbonTabItem3})
        Me.RibbonControlHome.KeyTipsFont = New System.Drawing.Font("Symbol", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.RibbonControlHome.Location = New System.Drawing.Point(0, 0)
        Me.RibbonControlHome.Name = "RibbonControlHome"
        Me.RibbonControlHome.Padding = New System.Windows.Forms.Padding(0, 0, 0, 2)
        Me.RibbonControlHome.QuickToolbarItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.HomeButton, Me.ButtonItem1, Me.QatCustomizeItem1})
        Me.RibbonControlHome.Size = New System.Drawing.Size(837, 120)
        Me.RibbonControlHome.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.RibbonControlHome.SystemText.MaximizeRibbonText = "&Maximize the Ribbon"
        Me.RibbonControlHome.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon"
        Me.RibbonControlHome.SystemText.QatAddItemText = "&Add to Quick Access Toolbar"
        Me.RibbonControlHome.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>"
        Me.RibbonControlHome.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar..."
        Me.RibbonControlHome.SystemText.QatDialogAddButton = "&Add >>"
        Me.RibbonControlHome.SystemText.QatDialogCancelButton = "Cancel"
        Me.RibbonControlHome.SystemText.QatDialogCaption = "Customize Quick Access Toolbar"
        Me.RibbonControlHome.SystemText.QatDialogCategoriesLabel = "&Choose commands from:"
        Me.RibbonControlHome.SystemText.QatDialogOkButton = "OK"
        Me.RibbonControlHome.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon"
        Me.RibbonControlHome.SystemText.QatDialogRemoveButton = "&Remove"
        Me.RibbonControlHome.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon"
        Me.RibbonControlHome.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon"
        Me.RibbonControlHome.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar"
        Me.RibbonControlHome.TabGroupHeight = 14
        Me.RibbonControlHome.TabIndex = 0
        Me.RibbonControlHome.Text = "RibbonControl1"
        '
        'RibbonPanel2
        '
        Me.RibbonPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.RibbonPanel2.Controls.Add(Me.btnDisciplinaryaction)
        Me.RibbonPanel2.Controls.Add(Me.Button2)
        Me.RibbonPanel2.Controls.Add(Me.Button1)
        Me.RibbonPanel2.Controls.Add(Me.btnLoadSched)
        Me.RibbonPanel2.Controls.Add(Me.btntax)
        Me.RibbonPanel2.Controls.Add(Me.hbtnDivision)
        Me.RibbonPanel2.Controls.Add(Me.hbtnEducationbGround)
        Me.RibbonPanel2.Controls.Add(Me.hbtnPrevEmployer)
        Me.RibbonPanel2.Controls.Add(Me.hbtnEmpSalary)
        Me.RibbonPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RibbonPanel2.Location = New System.Drawing.Point(0, 0)
        Me.RibbonPanel2.Name = "RibbonPanel2"
        Me.RibbonPanel2.Padding = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.RibbonPanel2.Size = New System.Drawing.Size(837, 118)
        '
        '
        '
        Me.RibbonPanel2.Style.Class = ""
        Me.RibbonPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.RibbonPanel2.StyleMouseDown.Class = ""
        Me.RibbonPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.RibbonPanel2.StyleMouseOver.Class = ""
        Me.RibbonPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.RibbonPanel2.TabIndex = 2
        Me.RibbonPanel2.Visible = False
        '
        'btnDisciplinaryaction
        '
        Me.btnDisciplinaryaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDisciplinaryaction.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnDisciplinaryaction.Location = New System.Drawing.Point(638, 3)
        Me.btnDisciplinaryaction.Name = "btnDisciplinaryaction"
        Me.btnDisciplinaryaction.Size = New System.Drawing.Size(78, 53)
        Me.btnDisciplinaryaction.TabIndex = 11
        Me.btnDisciplinaryaction.Text = "Disciplinary Action"
        Me.btnDisciplinaryaction.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnDisciplinaryaction.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button2.Location = New System.Drawing.Point(554, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(78, 53)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Promotion"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button1.Location = New System.Drawing.Point(470, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(78, 53)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Load History"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnLoadSched
        '
        Me.btnLoadSched.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadSched.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnLoadSched.Location = New System.Drawing.Point(376, 3)
        Me.btnLoadSched.Name = "btnLoadSched"
        Me.btnLoadSched.Size = New System.Drawing.Size(88, 53)
        Me.btnLoadSched.TabIndex = 8
        Me.btnLoadSched.Text = "Load Schedule"
        Me.btnLoadSched.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnLoadSched.UseVisualStyleBackColor = True
        '
        'btntax
        '
        Me.btntax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btntax.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btntax.Location = New System.Drawing.Point(310, 3)
        Me.btntax.Name = "btntax"
        Me.btntax.Size = New System.Drawing.Size(60, 53)
        Me.btntax.TabIndex = 7
        Me.btntax.Text = "Tax"
        Me.btntax.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btntax.UseVisualStyleBackColor = True
        '
        'hbtnDivision
        '
        Me.hbtnDivision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.hbtnDivision.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.hbtnDivision.Location = New System.Drawing.Point(244, 3)
        Me.hbtnDivision.Name = "hbtnDivision"
        Me.hbtnDivision.Size = New System.Drawing.Size(60, 53)
        Me.hbtnDivision.TabIndex = 6
        Me.hbtnDivision.Text = "Division"
        Me.hbtnDivision.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.hbtnDivision.UseVisualStyleBackColor = True
        '
        'hbtnEducationbGround
        '
        Me.hbtnEducationbGround.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.hbtnEducationbGround.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.hbtnEducationbGround.Location = New System.Drawing.Point(156, 3)
        Me.hbtnEducationbGround.Name = "hbtnEducationbGround"
        Me.hbtnEducationbGround.Size = New System.Drawing.Size(82, 53)
        Me.hbtnEducationbGround.TabIndex = 5
        Me.hbtnEducationbGround.Text = "Educ " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Background"
        Me.hbtnEducationbGround.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.hbtnEducationbGround.UseVisualStyleBackColor = True
        '
        'hbtnPrevEmployer
        '
        Me.hbtnPrevEmployer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.hbtnPrevEmployer.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.hbtnPrevEmployer.Location = New System.Drawing.Point(84, 3)
        Me.hbtnPrevEmployer.Name = "hbtnPrevEmployer"
        Me.hbtnPrevEmployer.Size = New System.Drawing.Size(66, 53)
        Me.hbtnPrevEmployer.TabIndex = 4
        Me.hbtnPrevEmployer.Text = "Previous Employer"
        Me.hbtnPrevEmployer.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.hbtnPrevEmployer.UseVisualStyleBackColor = True
        '
        'RibbonPanel3
        '
        Me.RibbonPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.RibbonPanel3.Controls.Add(Me.btnTimeENtry)
        Me.RibbonPanel3.Controls.Add(Me.btnShiftEntry)
        Me.RibbonPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RibbonPanel3.Location = New System.Drawing.Point(0, 56)
        Me.RibbonPanel3.Name = "RibbonPanel3"
        Me.RibbonPanel3.Padding = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.RibbonPanel3.Size = New System.Drawing.Size(837, 62)
        '
        '
        '
        Me.RibbonPanel3.Style.Class = ""
        Me.RibbonPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.RibbonPanel3.StyleMouseDown.Class = ""
        Me.RibbonPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.RibbonPanel3.StyleMouseOver.Class = ""
        Me.RibbonPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.RibbonPanel3.TabIndex = 3
        '
        'btnShiftEntry
        '
        Me.btnShiftEntry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShiftEntry.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnShiftEntry.Location = New System.Drawing.Point(6, 0)
        Me.btnShiftEntry.Name = "btnShiftEntry"
        Me.btnShiftEntry.Size = New System.Drawing.Size(78, 53)
        Me.btnShiftEntry.TabIndex = 10
        Me.btnShiftEntry.Text = "Shift Entry"
        Me.btnShiftEntry.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnShiftEntry.UseVisualStyleBackColor = True
        '
        'RibbonPanel1
        '
        Me.RibbonPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.RibbonPanel1.Controls.Add(Me.btnUserPrevilige)
        Me.RibbonPanel1.Controls.Add(Me.btnOrganization)
        Me.RibbonPanel1.Controls.Add(Me.btnListOfval)
        Me.RibbonPanel1.Controls.Add(Me.btnUsersform)
        Me.RibbonPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RibbonPanel1.Location = New System.Drawing.Point(0, 56)
        Me.RibbonPanel1.Name = "RibbonPanel1"
        Me.RibbonPanel1.Padding = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.RibbonPanel1.Size = New System.Drawing.Size(837, 62)
        '
        '
        '
        Me.RibbonPanel1.Style.Class = ""
        Me.RibbonPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.RibbonPanel1.StyleMouseDown.Class = ""
        Me.RibbonPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.RibbonPanel1.StyleMouseOver.Class = ""
        Me.RibbonPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.RibbonPanel1.TabIndex = 1
        Me.RibbonPanel1.Visible = False
        '
        'btnUserPrevilige
        '
        Me.btnUserPrevilige.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUserPrevilige.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnUserPrevilige.Location = New System.Drawing.Point(259, 3)
        Me.btnUserPrevilige.Name = "btnUserPrevilige"
        Me.btnUserPrevilige.Size = New System.Drawing.Size(91, 53)
        Me.btnUserPrevilige.TabIndex = 8
        Me.btnUserPrevilige.Text = "User Previlige"
        Me.btnUserPrevilige.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnUserPrevilige.UseVisualStyleBackColor = True
        '
        'btnOrganization
        '
        Me.btnOrganization.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOrganization.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnOrganization.Location = New System.Drawing.Point(162, 3)
        Me.btnOrganization.Name = "btnOrganization"
        Me.btnOrganization.Size = New System.Drawing.Size(91, 53)
        Me.btnOrganization.TabIndex = 7
        Me.btnOrganization.Text = "Organization"
        Me.btnOrganization.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnOrganization.UseVisualStyleBackColor = True
        '
        'btnListOfval
        '
        Me.btnListOfval.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnListOfval.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnListOfval.Location = New System.Drawing.Point(84, 3)
        Me.btnListOfval.Name = "btnListOfval"
        Me.btnListOfval.Size = New System.Drawing.Size(72, 53)
        Me.btnListOfval.TabIndex = 6
        Me.btnListOfval.Text = "List of value"
        Me.btnListOfval.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnListOfval.UseVisualStyleBackColor = True
        '
        'btnUsersform
        '
        Me.btnUsersform.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUsersform.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnUsersform.Location = New System.Drawing.Point(12, 3)
        Me.btnUsersform.Name = "btnUsersform"
        Me.btnUsersform.Size = New System.Drawing.Size(66, 53)
        Me.btnUsersform.TabIndex = 5
        Me.btnUsersform.Text = "Users"
        Me.btnUsersform.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnUsersform.UseVisualStyleBackColor = True
        '
        'RibbonTabItem1
        '
        Me.RibbonTabItem1.FontBold = True
        Me.RibbonTabItem1.Name = "RibbonTabItem1"
        Me.RibbonTabItem1.Panel = Me.RibbonPanel1
        Me.RibbonTabItem1.Text = "&General"
        '
        'RibbonTabItem2
        '
        Me.RibbonTabItem2.FontBold = True
        Me.RibbonTabItem2.Name = "RibbonTabItem2"
        Me.RibbonTabItem2.Panel = Me.RibbonPanel2
        Me.RibbonTabItem2.Text = "&HRIS"
        '
        'RibbonTabItem3
        '
        Me.RibbonTabItem3.Checked = True
        Me.RibbonTabItem3.FontBold = True
        Me.RibbonTabItem3.Name = "RibbonTabItem3"
        Me.RibbonTabItem3.Panel = Me.RibbonPanel3
        Me.RibbonTabItem3.Text = "Time Attendance"
        '
        'ButtonItem1
        '
        Me.ButtonItem1.Name = "ButtonItem1"
        Me.ButtonItem1.Text = "Payroll System"
        '
        'QatCustomizeItem1
        '
        Me.QatCustomizeItem1.Name = "QatCustomizeItem1"
        Me.QatCustomizeItem1.Visible = False
        '
        'StyleManager1
        '
        Me.StyleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2007Black
        Me.StyleManager1.MetroColorParameters = New DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(26, Byte), Integer)))
        '
        'StatusStrip1
        '
        Me.StatusStrip1.BackColor = System.Drawing.Color.Transparent
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.lblTime, Me.ToolStripStatusLabel3, Me.lblUser, Me.ToolStripStatusLabel5, Me.lblPosition, Me.MainProgressBar})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 418)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(837, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(37, 17)
        Me.ToolStripStatusLabel1.Text = "Time:"
        '
        'lblTime
        '
        Me.lblTime.AutoSize = False
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(100, 17)
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(33, 17)
        Me.ToolStripStatusLabel3.Text = "User:"
        '
        'lblUser
        '
        Me.lblUser.AutoSize = False
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(121, 17)
        '
        'ToolStripStatusLabel5
        '
        Me.ToolStripStatusLabel5.Name = "ToolStripStatusLabel5"
        Me.ToolStripStatusLabel5.Size = New System.Drawing.Size(50, 17)
        Me.ToolStripStatusLabel5.Text = "Positon:"
        '
        'lblPosition
        '
        Me.lblPosition.AutoSize = False
        Me.lblPosition.Name = "lblPosition"
        Me.lblPosition.Size = New System.Drawing.Size(121, 17)
        '
        'MainProgressBar
        '
        Me.MainProgressBar.Name = "MainProgressBar"
        Me.MainProgressBar.Size = New System.Drawing.Size(120, 16)
        Me.MainProgressBar.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(3, 121)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(830, 294)
        Me.Panel1.TabIndex = 2
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1
        '
        'btnTimeENtry
        '
        Me.btnTimeENtry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTimeENtry.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnTimeENtry.Location = New System.Drawing.Point(90, 0)
        Me.btnTimeENtry.Name = "btnTimeENtry"
        Me.btnTimeENtry.Size = New System.Drawing.Size(76, 53)
        Me.btnTimeENtry.TabIndex = 11
        Me.btnTimeENtry.Text = "Time Entry"
        Me.btnTimeENtry.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnTimeENtry.UseVisualStyleBackColor = True
        '
        'hbtnEmpSalary
        '
        Me.hbtnEmpSalary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.hbtnEmpSalary.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Apps_preferences_system_time_icon
        Me.hbtnEmpSalary.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.hbtnEmpSalary.Location = New System.Drawing.Point(12, 3)
        Me.hbtnEmpSalary.Name = "hbtnEmpSalary"
        Me.hbtnEmpSalary.Size = New System.Drawing.Size(66, 53)
        Me.hbtnEmpSalary.TabIndex = 3
        Me.hbtnEmpSalary.Text = "Employee Salary"
        Me.hbtnEmpSalary.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.hbtnEmpSalary.UseVisualStyleBackColor = True
        '
        'HomeButton
        '
        Me.HomeButton.AutoExpandOnClick = True
        Me.HomeButton.CanCustomize = False
        Me.HomeButton.FontBold = True
        Me.HomeButton.HotTrackingStyle = DevComponents.DotNetBar.eHotTrackingStyle.Image
        Me.HomeButton.Image = CType(resources.GetObject("HomeButton.Image"), System.Drawing.Image)
        Me.HomeButton.ImagePaddingHorizontal = 2
        Me.HomeButton.ImagePaddingVertical = 2
        Me.HomeButton.Name = "HomeButton"
        Me.HomeButton.ShowSubItems = False
        Me.HomeButton.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnTimeAttendance, Me.ButtonItem3, Me.ButtonItem4, Me.ButtonItem10, Me.ButtonItem11, Me.ButtonItem12, Me.ButtonItem13, Me.ButtonItem14, Me.ButtonItem2, Me.btnbackup, Me.hbtnexit})
        Me.HomeButton.Text = "&Home"
        '
        'btnTimeAttendance
        '
        Me.btnTimeAttendance.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.Apps_preferences_system_time_icon__2_
        Me.btnTimeAttendance.Name = "btnTimeAttendance"
        Me.btnTimeAttendance.Text = "&Time Attendance"
        '
        'ButtonItem3
        '
        Me.ButtonItem3.Name = "ButtonItem3"
        Me.ButtonItem3.Text = "&HRIS"
        '
        'ButtonItem4
        '
        Me.ButtonItem4.Name = "ButtonItem4"
        Me.ButtonItem4.Text = "&Payroll"
        '
        'ButtonItem10
        '
        Me.ButtonItem10.Name = "ButtonItem10"
        Me.ButtonItem10.Text = "&Government"
        '
        'ButtonItem11
        '
        Me.ButtonItem11.Name = "ButtonItem11"
        Me.ButtonItem11.Text = "&Employee"
        '
        'ButtonItem12
        '
        Me.ButtonItem12.Name = "ButtonItem12"
        Me.ButtonItem12.Text = "&Approving Supervisor"
        '
        'ButtonItem13
        '
        Me.ButtonItem13.Name = "ButtonItem13"
        Me.ButtonItem13.Text = "&Immediate Supervisor"
        '
        'ButtonItem14
        '
        Me.ButtonItem14.Name = "ButtonItem14"
        Me.ButtonItem14.Text = "&Reports"
        '
        'ButtonItem2
        '
        Me.ButtonItem2.Name = "ButtonItem2"
        Me.ButtonItem2.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.hbtnUsers, Me.hbtnListofval, Me.hbtnorganization, Me.hbtnaudittrail, Me.hbtnChangePassword})
        Me.ButtonItem2.Text = "&General"
        '
        'hbtnUsers
        '
        Me.hbtnUsers.Name = "hbtnUsers"
        Me.hbtnUsers.Text = "Users"
        '
        'hbtnListofval
        '
        Me.hbtnListofval.Name = "hbtnListofval"
        Me.hbtnListofval.Text = "List of value"
        '
        'hbtnorganization
        '
        Me.hbtnorganization.Name = "hbtnorganization"
        Me.hbtnorganization.Text = "Organization"
        '
        'hbtnaudittrail
        '
        Me.hbtnaudittrail.Name = "hbtnaudittrail"
        Me.hbtnaudittrail.Text = "Audit Trail"
        '
        'hbtnChangePassword
        '
        Me.hbtnChangePassword.Name = "hbtnChangePassword"
        Me.hbtnChangePassword.Text = "Change Password"
        '
        'btnbackup
        '
        Me.btnbackup.Name = "btnbackup"
        Me.btnbackup.Text = "&Backup Database"
        '
        'hbtnexit
        '
        Me.hbtnexit.Name = "hbtnexit"
        Me.hbtnexit.Text = "&Exit"
        '
        'ButtonItem16
        '
        Me.ButtonItem16.Image = Global.GoldWingsPayrollSys.My.Resources.Resources.application_view_list_icon
        Me.ButtonItem16.Name = "ButtonItem16"
        Me.ButtonItem16.SubItemsExpandWidth = 14
        '
        'GroupTimer
        '
        Me.GroupTimer.Enabled = True
        Me.GroupTimer.Interval = 300
        '
        'MainForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(837, 440)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.RibbonControlHome)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.IsMdiContainer = True
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.RibbonControlHome.ResumeLayout(False)
        Me.RibbonControlHome.PerformLayout()
        Me.RibbonPanel2.ResumeLayout(False)
        Me.RibbonPanel3.ResumeLayout(False)
        Me.RibbonPanel1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RibbonControlHome As DevComponents.DotNetBar.RibbonControl
    Friend WithEvents RibbonPanel1 As DevComponents.DotNetBar.RibbonPanel
    Friend WithEvents RibbonPanel2 As DevComponents.DotNetBar.RibbonPanel
    Friend WithEvents RibbonTabItem1 As DevComponents.DotNetBar.RibbonTabItem
    Friend WithEvents RibbonTabItem2 As DevComponents.DotNetBar.RibbonTabItem
    Friend WithEvents HomeButton As DevComponents.DotNetBar.Office2007StartButton
    Friend WithEvents ButtonItem2 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents hbtnUsers As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents hbtnListofval As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents hbtnorganization As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents hbtnaudittrail As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents hbtnChangePassword As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItem3 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTimeAttendance As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItem1 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents QatCustomizeItem1 As DevComponents.DotNetBar.QatCustomizeItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButtonItem4 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItem10 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItem11 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItem12 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItem13 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItem14 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblTime As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblUser As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel5 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblPosition As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MainProgressBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents ButtonItem16 As DevComponents.DotNetBar.ButtonItem
    Private WithEvents StyleManager1 As DevComponents.DotNetBar.StyleManager
    Friend WithEvents hbtnPrevEmployer As System.Windows.Forms.Button
    Friend WithEvents hbtnEmpSalary As System.Windows.Forms.Button
    Friend WithEvents btnOrganization As System.Windows.Forms.Button
    Friend WithEvents btnListOfval As System.Windows.Forms.Button
    Friend WithEvents btnUsersform As System.Windows.Forms.Button
    Friend WithEvents hbtnEducationbGround As System.Windows.Forms.Button
    Friend WithEvents btnUserPrevilige As System.Windows.Forms.Button
    Friend WithEvents hbtnexit As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents hbtnDivision As System.Windows.Forms.Button
    Friend WithEvents btntax As System.Windows.Forms.Button
    Friend WithEvents btnLoadSched As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnbackup As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents RibbonPanel3 As DevComponents.DotNetBar.RibbonPanel
    Friend WithEvents btnShiftEntry As System.Windows.Forms.Button
    Friend WithEvents RibbonTabItem3 As DevComponents.DotNetBar.RibbonTabItem
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents btnDisciplinaryaction As System.Windows.Forms.Button
    Friend WithEvents btnTimeENtry As System.Windows.Forms.Button
    Friend WithEvents GroupTimer As System.Windows.Forms.Timer
End Class
