<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NewDivisionPositionForm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewDivisionPositionForm))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.DivisionPositionTreeView = New System.Windows.Forms.TreeView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.PositionTabPage = New System.Windows.Forms.TabPage()
        Me.PositionGroupBox = New System.Windows.Forms.GroupBox()
        Me.PositionUserControl1 = New PositionUserControl()
        Me.EmployeeDataGrid = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MiddleName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.SavePositionToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeletePositionToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CancelPositionToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CloseFormToolStripItem2 = New System.Windows.Forms.ToolStripButton()
        Me.UserActivityPositionToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.miniToolStrip = New System.Windows.Forms.ToolStrip()
        Me.DivisionTabPage = New System.Windows.Forms.TabPage()
        Me.DivisionUserControl1 = New DivisionUserControl()
        Me.DivisionLocationGroupBox = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel9 = New System.Windows.Forms.Panel()
        Me.Panel11 = New System.Windows.Forms.Panel()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.DivisionLocationTextBox = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.SaveDivisionToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteDivisionToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CancelDivisionToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CloseFormToolStripItem = New System.Windows.Forms.ToolStripButton()
        Me.UserActivityToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.FormsTabControl = New System.Windows.Forms.TabControl()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip()
        Me.SearchToolStripTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.AddDivisionLocationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddDivisionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddPositionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PositionTabPage.SuspendLayout()
        Me.PositionGroupBox.SuspendLayout()
        CType(Me.EmployeeDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip2.SuspendLayout()
        Me.DivisionTabPage.SuspendLayout()
        Me.DivisionLocationGroupBox.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.Panel11.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.FormsTabControl.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ToolStrip3.SuspendLayout()
        Me.SuspendLayout()
        '
        'DivisionPositionTreeView
        '
        Me.DivisionPositionTreeView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DivisionPositionTreeView.ImageIndex = 2
        Me.DivisionPositionTreeView.ImageList = Me.ImageList1
        Me.DivisionPositionTreeView.Location = New System.Drawing.Point(27, 70)
        Me.DivisionPositionTreeView.Name = "DivisionPositionTreeView"
        Me.DivisionPositionTreeView.SelectedImageIndex = 0
        Me.DivisionPositionTreeView.Size = New System.Drawing.Size(350, 454)
        Me.DivisionPositionTreeView.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Documents-icon-64.png")
        Me.ImageList1.Images.SetKeyName(1, "Icon_159.ico")
        Me.ImageList1.Images.SetKeyName(2, "groupofusers.png")
        '
        'lblFormTitle
        '
        Me.lblFormTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(174, Byte), Integer), CType(CType(194, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblFormTitle.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblFormTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(1229, 21)
        Me.lblFormTitle.TabIndex = 139
        Me.lblFormTitle.Text = "DIVISION"
        Me.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PositionTabPage
        '
        Me.PositionTabPage.Controls.Add(Me.PositionGroupBox)
        Me.PositionTabPage.Controls.Add(Me.EmployeeDataGrid)
        Me.PositionTabPage.Controls.Add(Me.Label2)
        Me.PositionTabPage.Controls.Add(Me.ToolStrip2)
        Me.PositionTabPage.Location = New System.Drawing.Point(4, 22)
        Me.PositionTabPage.Name = "PositionTabPage"
        Me.PositionTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.PositionTabPage.Size = New System.Drawing.Size(802, 460)
        Me.PositionTabPage.TabIndex = 1
        Me.PositionTabPage.Text = "Position"
        Me.PositionTabPage.UseVisualStyleBackColor = True
        '
        'PositionGroupBox
        '
        Me.PositionGroupBox.Controls.Add(Me.PositionUserControl1)
        Me.PositionGroupBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.PositionGroupBox.Location = New System.Drawing.Point(6, 42)
        Me.PositionGroupBox.Name = "PositionGroupBox"
        Me.PositionGroupBox.Size = New System.Drawing.Size(299, 172)
        Me.PositionGroupBox.TabIndex = 394
        Me.PositionGroupBox.TabStop = False
        Me.PositionGroupBox.Text = "Payroll Details"
        '
        'PositionUserControl1
        '
        Me.PositionUserControl1.Location = New System.Drawing.Point(0, 17)
        Me.PositionUserControl1.Margin = New System.Windows.Forms.Padding(4)
        Me.PositionUserControl1.Name = "PositionUserControl1"
        Me.PositionUserControl1.Size = New System.Drawing.Size(292, 148)
        Me.PositionUserControl1.TabIndex = 0
        '
        'EmployeeDataGrid
        '
        Me.EmployeeDataGrid.AllowUserToAddRows = False
        Me.EmployeeDataGrid.AllowUserToDeleteRows = False
        Me.EmployeeDataGrid.AllowUserToOrderColumns = True
        Me.EmployeeDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.EmployeeDataGrid.ColumnHeadersHeight = 34
        Me.EmployeeDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.EmployeeID, Me.LastName, Me.FirstName, Me.MiddleName})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.EmployeeDataGrid.DefaultCellStyle = DataGridViewCellStyle2
        Me.EmployeeDataGrid.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.EmployeeDataGrid.Location = New System.Drawing.Point(6, 242)
        Me.EmployeeDataGrid.MultiSelect = False
        Me.EmployeeDataGrid.Name = "EmployeeDataGrid"
        Me.EmployeeDataGrid.ReadOnly = True
        Me.EmployeeDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.EmployeeDataGrid.Size = New System.Drawing.Size(768, 212)
        Me.EmployeeDataGrid.TabIndex = 333
        '
        'EmployeeID
        '
        Me.EmployeeID.DataPropertyName = "EmployeeNo"
        Me.EmployeeID.HeaderText = "Employee ID"
        Me.EmployeeID.Name = "EmployeeID"
        Me.EmployeeID.ReadOnly = True
        '
        'LastName
        '
        Me.LastName.DataPropertyName = "LastName"
        Me.LastName.HeaderText = "Last Name"
        Me.LastName.Name = "LastName"
        Me.LastName.ReadOnly = True
        '
        'FirstName
        '
        Me.FirstName.DataPropertyName = "FirstName"
        Me.FirstName.HeaderText = "First Name"
        Me.FirstName.Name = "FirstName"
        Me.FirstName.ReadOnly = True
        '
        'MiddleName
        '
        Me.MiddleName.DataPropertyName = "MiddleName"
        Me.MiddleName.HeaderText = "Middle Name"
        Me.MiddleName.Name = "MiddleName"
        Me.MiddleName.ReadOnly = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(3, 223)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(123, 15)
        Me.Label2.TabIndex = 332
        Me.Label2.Text = "Assigned Employee(s)"
        '
        'ToolStrip2
        '
        Me.ToolStrip2.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SavePositionToolStripButton, Me.DeletePositionToolStripButton, Me.CancelPositionToolStripButton, Me.CloseFormToolStripItem2, Me.ToolStripSeparator3, Me.UserActivityPositionToolStripButton})
        Me.ToolStrip2.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(796, 25)
        Me.ToolStrip2.TabIndex = 330
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'SavePositionToolStripButton
        '
        Me.SavePositionToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.SavePositionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SavePositionToolStripButton.Name = "SavePositionToolStripButton"
        Me.SavePositionToolStripButton.Size = New System.Drawing.Size(51, 22)
        Me.SavePositionToolStripButton.Text = "&Save"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'DeletePositionToolStripButton
        '
        Me.DeletePositionToolStripButton.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.DeletePositionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeletePositionToolStripButton.Name = "DeletePositionToolStripButton"
        Me.DeletePositionToolStripButton.Size = New System.Drawing.Size(60, 22)
        Me.DeletePositionToolStripButton.Text = "&Delete"
        '
        'CancelPositionToolStripButton
        '
        Me.CancelPositionToolStripButton.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.CancelPositionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CancelPositionToolStripButton.Name = "CancelPositionToolStripButton"
        Me.CancelPositionToolStripButton.Size = New System.Drawing.Size(63, 22)
        Me.CancelPositionToolStripButton.Text = "Cancel"
        '
        'CloseFormToolStripItem2
        '
        Me.CloseFormToolStripItem2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.CloseFormToolStripItem2.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.CloseFormToolStripItem2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CloseFormToolStripItem2.Name = "CloseFormToolStripItem2"
        Me.CloseFormToolStripItem2.Size = New System.Drawing.Size(56, 22)
        Me.CloseFormToolStripItem2.Text = "Close"
        '
        'UserActivityPositionToolStripButton
        '
        Me.UserActivityPositionToolStripButton.Image = CType(resources.GetObject("UserActivityPositionToolStripButton.Image"), System.Drawing.Image)
        Me.UserActivityPositionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.UserActivityPositionToolStripButton.Name = "UserActivityPositionToolStripButton"
        Me.UserActivityPositionToolStripButton.Size = New System.Drawing.Size(93, 22)
        Me.UserActivityPositionToolStripButton.Text = "User Activity"
        '
        'miniToolStrip
        '
        Me.miniToolStrip.AccessibleName = "New item selection"
        Me.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown
        Me.miniToolStrip.AutoSize = False
        Me.miniToolStrip.BackColor = System.Drawing.Color.Transparent
        Me.miniToolStrip.CanOverflow = False
        Me.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.miniToolStrip.Location = New System.Drawing.Point(398, 3)
        Me.miniToolStrip.Name = "miniToolStrip"
        Me.miniToolStrip.Size = New System.Drawing.Size(796, 25)
        Me.miniToolStrip.TabIndex = 330
        '
        'DivisionTabPage
        '
        Me.DivisionTabPage.AutoScroll = True
        Me.DivisionTabPage.Controls.Add(Me.DivisionUserControl1)
        Me.DivisionTabPage.Controls.Add(Me.DivisionLocationGroupBox)
        Me.DivisionTabPage.Controls.Add(Me.ToolStrip1)
        Me.DivisionTabPage.Location = New System.Drawing.Point(4, 22)
        Me.DivisionTabPage.Name = "DivisionTabPage"
        Me.DivisionTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.DivisionTabPage.Size = New System.Drawing.Size(802, 460)
        Me.DivisionTabPage.TabIndex = 0
        Me.DivisionTabPage.Text = "Division"
        Me.DivisionTabPage.UseVisualStyleBackColor = True
        '
        'DivisionUserControl1
        '
        Me.DivisionUserControl1.Location = New System.Drawing.Point(6, 42)
        Me.DivisionUserControl1.Name = "DivisionUserControl1"
        Me.DivisionUserControl1.Size = New System.Drawing.Size(740, 600)
        Me.DivisionUserControl1.TabIndex = 396
        Me.DivisionUserControl1.Visible = False
        '
        'DivisionLocationGroupBox
        '
        Me.DivisionLocationGroupBox.Controls.Add(Me.TableLayoutPanel4)
        Me.DivisionLocationGroupBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.DivisionLocationGroupBox.Location = New System.Drawing.Point(6, 84)
        Me.DivisionLocationGroupBox.Name = "DivisionLocationGroupBox"
        Me.DivisionLocationGroupBox.Size = New System.Drawing.Size(240, 93)
        Me.DivisionLocationGroupBox.TabIndex = 395
        Me.DivisionLocationGroupBox.TabStop = False
        Me.DivisionLocationGroupBox.Text = "Division Location"
        Me.DivisionLocationGroupBox.Visible = False
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.Panel9, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.Label7, 0, 0)
        Me.TableLayoutPanel4.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(6, 21)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 2
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(230, 64)
        Me.TableLayoutPanel4.TabIndex = 332
        '
        'Panel9
        '
        Me.Panel9.Controls.Add(Me.Panel11)
        Me.Panel9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel9.Location = New System.Drawing.Point(0, 16)
        Me.Panel9.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(230, 48)
        Me.Panel9.TabIndex = 382
        '
        'Panel11
        '
        Me.Panel11.Controls.Add(Me.Label23)
        Me.Panel11.Controls.Add(Me.DivisionLocationTextBox)
        Me.Panel11.Location = New System.Drawing.Point(0, 0)
        Me.Panel11.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(230, 48)
        Me.Panel11.TabIndex = 385
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label23.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label23.Location = New System.Drawing.Point(3, 4)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(13, 13)
        Me.Label23.TabIndex = 507
        Me.Label23.Text = "*"
        '
        'DivisionLocationTextBox
        '
        Me.DivisionLocationTextBox.Location = New System.Drawing.Point(20, 3)
        Me.DivisionLocationTextBox.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.DivisionLocationTextBox.Name = "DivisionLocationTextBox"
        Me.DivisionLocationTextBox.Size = New System.Drawing.Size(195, 22)
        Me.DivisionLocationTextBox.TabIndex = 354
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 3)
        Me.Label7.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 13)
        Me.Label7.TabIndex = 363
        Me.Label7.Text = "Division Name"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveDivisionToolStripButton, Me.DeleteDivisionToolStripButton, Me.CancelDivisionToolStripButton, Me.CloseFormToolStripItem, Me.ToolStripSeparator1, Me.UserActivityToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(779, 25)
        Me.ToolStrip1.TabIndex = 329
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'SaveDivisionToolStripButton
        '
        Me.SaveDivisionToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.SaveDivisionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveDivisionToolStripButton.Name = "SaveDivisionToolStripButton"
        Me.SaveDivisionToolStripButton.Size = New System.Drawing.Size(51, 22)
        Me.SaveDivisionToolStripButton.Text = "&Save"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'DeleteDivisionToolStripButton
        '
        Me.DeleteDivisionToolStripButton.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.DeleteDivisionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteDivisionToolStripButton.Name = "DeleteDivisionToolStripButton"
        Me.DeleteDivisionToolStripButton.Size = New System.Drawing.Size(60, 22)
        Me.DeleteDivisionToolStripButton.Text = "&Delete"
        '
        'CancelDivisionToolStripButton
        '
        Me.CancelDivisionToolStripButton.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.CancelDivisionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CancelDivisionToolStripButton.Name = "CancelDivisionToolStripButton"
        Me.CancelDivisionToolStripButton.Size = New System.Drawing.Size(63, 22)
        Me.CancelDivisionToolStripButton.Text = "Cancel"
        '
        'CloseFormToolStripItem
        '
        Me.CloseFormToolStripItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.CloseFormToolStripItem.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.CloseFormToolStripItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CloseFormToolStripItem.Name = "CloseFormToolStripItem"
        Me.CloseFormToolStripItem.Size = New System.Drawing.Size(56, 22)
        Me.CloseFormToolStripItem.Text = "Close"
        '
        'UserActivityToolStripButton
        '
        Me.UserActivityToolStripButton.Image = CType(resources.GetObject("UserActivityToolStripButton.Image"), System.Drawing.Image)
        Me.UserActivityToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.UserActivityToolStripButton.Name = "UserActivityToolStripButton"
        Me.UserActivityToolStripButton.Size = New System.Drawing.Size(93, 22)
        Me.UserActivityToolStripButton.Text = "User Activity"
        '
        'FormsTabControl
        '
        Me.FormsTabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FormsTabControl.Controls.Add(Me.DivisionTabPage)
        Me.FormsTabControl.Controls.Add(Me.PositionTabPage)
        Me.FormsTabControl.Location = New System.Drawing.Point(406, 42)
        Me.FormsTabControl.Name = "FormsTabControl"
        Me.FormsTabControl.SelectedIndex = 0
        Me.FormsTabControl.Size = New System.Drawing.Size(810, 486)
        Me.FormsTabControl.TabIndex = 140
        Me.FormsTabControl.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ToolStrip3)
        Me.Panel1.Location = New System.Drawing.Point(27, 42)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(350, 29)
        Me.Panel1.TabIndex = 141
        '
        'ToolStrip3
        '
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SearchToolStripTextBox, Me.ToolStripDropDownButton1, Me.ToolStripLabel1})
        Me.ToolStrip3.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Size = New System.Drawing.Size(350, 25)
        Me.ToolStrip3.TabIndex = 0
        Me.ToolStrip3.Text = "ToolStrip3"
        '
        'SearchToolStripTextBox
        '
        Me.SearchToolStripTextBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.SearchToolStripTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SearchToolStripTextBox.Margin = New System.Windows.Forms.Padding(1, 0, 20, 0)
        Me.SearchToolStripTextBox.Name = "SearchToolStripTextBox"
        Me.SearchToolStripTextBox.Size = New System.Drawing.Size(200, 25)
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddDivisionLocationToolStripMenuItem, Me.AddDivisionToolStripMenuItem, Me.AddPositionToolStripMenuItem})
        Me.ToolStripDropDownButton1.Image = Global.AccuPay.My.Resources.Resources._new
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(58, 22)
        Me.ToolStripDropDownButton1.Text = "Add"
        '
        'AddDivisionLocationToolStripMenuItem
        '
        Me.AddDivisionLocationToolStripMenuItem.Name = "AddDivisionLocationToolStripMenuItem"
        Me.AddDivisionLocationToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.AddDivisionLocationToolStripMenuItem.Text = "Division Location"
        '
        'AddDivisionToolStripMenuItem
        '
        Me.AddDivisionToolStripMenuItem.Name = "AddDivisionToolStripMenuItem"
        Me.AddDivisionToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.AddDivisionToolStripMenuItem.Text = "Division"
        '
        'AddPositionToolStripMenuItem
        '
        Me.AddPositionToolStripMenuItem.Name = "AddPositionToolStripMenuItem"
        Me.AddPositionToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.AddPositionToolStripMenuItem.Text = "Position"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripLabel1.Image = Global.AccuPay.My.Resources.Resources.magnifier_zoom
        Me.ToolStripLabel1.Margin = New System.Windows.Forms.Padding(0, 1, 5, 2)
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(16, 22)
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "EmployeeNo"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee ID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "LastName"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "FirstName"
        Me.DataGridViewTextBoxColumn3.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "MiddleName"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Middle Name"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'NewDivisionPositionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1229, 547)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.FormsTabControl)
        Me.Controls.Add(Me.lblFormTitle)
        Me.Controls.Add(Me.DivisionPositionTreeView)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "NewDivisionPositionForm"
        Me.Text = "NewEmployeePositionForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.PositionTabPage.ResumeLayout(False)
        Me.PositionTabPage.PerformLayout()
        Me.PositionGroupBox.ResumeLayout(False)
        CType(Me.EmployeeDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.DivisionTabPage.ResumeLayout(False)
        Me.DivisionTabPage.PerformLayout()
        Me.DivisionLocationGroupBox.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.Panel9.ResumeLayout(False)
        Me.Panel11.ResumeLayout(False)
        Me.Panel11.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.FormsTabControl.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblFormTitle As Label
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents PositionTabPage As TabPage
    Friend WithEvents EmployeeDataGrid As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label2 As Label
    Friend WithEvents ToolStrip2 As ToolStrip
    Friend WithEvents SavePositionToolStripButton As ToolStripButton
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents DeletePositionToolStripButton As ToolStripButton
    Friend WithEvents CancelPositionToolStripButton As ToolStripButton
    Friend WithEvents CloseFormToolStripItem2 As ToolStripButton
    Friend WithEvents miniToolStrip As ToolStrip
    Friend WithEvents DivisionTabPage As TabPage
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents SaveDivisionToolStripButton As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents DeleteDivisionToolStripButton As ToolStripButton
    Friend WithEvents CancelDivisionToolStripButton As ToolStripButton
    Friend WithEvents CloseFormToolStripItem As ToolStripButton
    Friend WithEvents FormsTabControl As TabControl
    Friend WithEvents DivisionLocationGroupBox As GroupBox
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents Panel9 As Panel
    Friend WithEvents Panel11 As Panel
    Friend WithEvents Label23 As Label
    Friend WithEvents DivisionLocationTextBox As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents PositionGroupBox As GroupBox
    Friend WithEvents DivisionUserControl1 As DivisionUserControl
    Friend WithEvents PositionUserControl1 As PositionUserControl
    Friend WithEvents EmployeeID As DataGridViewTextBoxColumn
    Friend WithEvents LastName As DataGridViewTextBoxColumn
    Friend WithEvents FirstName As DataGridViewTextBoxColumn
    Friend WithEvents MiddleName As DataGridViewTextBoxColumn
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ToolStrip3 As ToolStrip
    Friend WithEvents SearchToolStripTextBox As ToolStripTextBox
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents AddDivisionLocationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddDivisionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddPositionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Protected Friend WithEvents DivisionPositionTreeView As TreeView
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents UserActivityToolStripButton As ToolStripButton
    Friend WithEvents UserActivityPositionToolStripButton As ToolStripButton
End Class
