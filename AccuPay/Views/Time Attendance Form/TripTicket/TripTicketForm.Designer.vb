<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TripTicketForm
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TripTicketForm))
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlTripTicket = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.dgvTripTicketHelpers = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.dgvEmployeesName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTripTicketHelpersPosition = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvEmployeesPayment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTripTicketHelpersRemove = New DevComponents.DotNetBar.Controls.DataGridViewButtonXColumn()
        Me.cboEmployees = New System.Windows.Forms.ComboBox()
        Me.btnAddHelper = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblTruckType = New System.Windows.Forms.Label()
        Me.lblPlateNo = New System.Windows.Forms.Label()
        Me.txtDistance = New System.Windows.Forms.TextBox()
        Me.txtTruckType = New System.Windows.Forms.TextBox()
        Me.txtPlateNo = New System.Windows.Forms.TextBox()
        Me.cboVehicles = New System.Windows.Forms.ComboBox()
        Me.lnkRoutePayMatrix = New System.Windows.Forms.LinkLabel()
        Me.lblVehicle = New System.Windows.Forms.Label()
        Me.VehicleDialogLink = New System.Windows.Forms.LinkLabel()
        Me.cboRoutes = New System.Windows.Forms.ComboBox()
        Me.lblDistance = New System.Windows.Forms.Label()
        Me.lblRoute = New System.Windows.Forms.Label()
        Me.grpTripTicket = New System.Windows.Forms.GroupBox()
        Me.lblTripDate = New System.Windows.Forms.Label()
        Me.dtpTripDate = New System.Windows.Forms.DateTimePicker()
        Me.chkSpecialOperations = New System.Windows.Forms.CheckBox()
        Me.txtTicketNo = New System.Windows.Forms.TextBox()
        Me.lblTicketNo = New System.Windows.Forms.Label()
        Me.lblTimeFrom = New System.Windows.Forms.Label()
        Me.lblTimeTo = New System.Windows.Forms.Label()
        Me.lblTimeDispatched = New System.Windows.Forms.Label()
        Me.txtTimeDispatched = New System.Windows.Forms.TextBox()
        Me.txtTimeFrom = New System.Windows.Forms.TextBox()
        Me.txtTimeTo = New System.Windows.Forms.TextBox()
        Me.TripTicketMenubar = New System.Windows.Forms.ToolStrip()
        Me.btnNewTripTicket = New System.Windows.Forms.ToolStripButton()
        Me.btnSaveTripTicket = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnImportTripTicket = New System.Windows.Forms.ToolStripButton()
        Me.dgvTripTickets = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.dgvTripTicketsRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvTripTicketsTicketNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvTripTicketsTripDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvTripTicketsRoute = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlTripTicket.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvTripTicketHelpers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.grpTripTicket.SuspendLayout()
        Me.TripTicketMenubar.SuspendLayout()
        CType(Me.dgvTripTickets, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTripTicket
        '
        Me.pnlTripTicket.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTripTicket.Controls.Add(Me.GroupBox2)
        Me.pnlTripTicket.Controls.Add(Me.GroupBox1)
        Me.pnlTripTicket.Controls.Add(Me.grpTripTicket)
        Me.pnlTripTicket.Controls.Add(Me.TripTicketMenubar)
        Me.pnlTripTicket.Location = New System.Drawing.Point(411, 27)
        Me.pnlTripTicket.Name = "pnlTripTicket"
        Me.pnlTripTicket.Size = New System.Drawing.Size(842, 708)
        Me.pnlTripTicket.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.dgvTripTicketHelpers)
        Me.GroupBox2.Controls.Add(Me.cboEmployees)
        Me.GroupBox2.Controls.Add(Me.btnAddHelper)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 359)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(836, 341)
        Me.GroupBox2.TabIndex = 108
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Driver and Helpers"
        '
        'dgvTripTicketHelpers
        '
        Me.dgvTripTicketHelpers.AllowUserToAddRows = False
        Me.dgvTripTicketHelpers.AllowUserToDeleteRows = False
        Me.dgvTripTicketHelpers.AllowUserToOrderColumns = True
        Me.dgvTripTicketHelpers.AllowUserToResizeRows = False
        Me.dgvTripTicketHelpers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvTripTicketHelpers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTripTicketHelpers.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvTripTicketHelpers.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgvTripTicketHelpers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTripTicketHelpers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvEmployeesName, Me.colTripTicketHelpersPosition, Me.dgvEmployeesPayment, Me.colTripTicketHelpersRemove})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvTripTicketHelpers.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgvTripTicketHelpers.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvTripTicketHelpers.Location = New System.Drawing.Point(11, 48)
        Me.dgvTripTicketHelpers.MultiSelect = False
        Me.dgvTripTicketHelpers.Name = "dgvTripTicketHelpers"
        Me.dgvTripTicketHelpers.RowHeadersWidth = 25
        Me.dgvTripTicketHelpers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvTripTicketHelpers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTripTicketHelpers.Size = New System.Drawing.Size(819, 287)
        Me.dgvTripTicketHelpers.TabIndex = 102
        '
        'dgvEmployeesName
        '
        Me.dgvEmployeesName.DataPropertyName = "FullName"
        Me.dgvEmployeesName.HeaderText = "Name"
        Me.dgvEmployeesName.Name = "dgvEmployeesName"
        Me.dgvEmployeesName.ReadOnly = True
        '
        'colTripTicketHelpersPosition
        '
        Me.colTripTicketHelpersPosition.DataPropertyName = "PositionName"
        Me.colTripTicketHelpersPosition.HeaderText = "Position"
        Me.colTripTicketHelpersPosition.Name = "colTripTicketHelpersPosition"
        Me.colTripTicketHelpersPosition.ReadOnly = True
        '
        'dgvEmployeesPayment
        '
        Me.dgvEmployeesPayment.DataPropertyName = "NoOfTrips"
        Me.dgvEmployeesPayment.HeaderText = "No of Trips"
        Me.dgvEmployeesPayment.Name = "dgvEmployeesPayment"
        '
        'colTripTicketHelpersRemove
        '
        Me.colTripTicketHelpersRemove.HeaderText = "Remove"
        Me.colTripTicketHelpersRemove.Name = "colTripTicketHelpersRemove"
        Me.colTripTicketHelpersRemove.ReadOnly = True
        Me.colTripTicketHelpersRemove.Text = "Remove"
        Me.colTripTicketHelpersRemove.UseColumnTextForButtonValue = True
        '
        'cboEmployees
        '
        Me.cboEmployees.DisplayMember = "FullName"
        Me.cboEmployees.FormattingEnabled = True
        Me.cboEmployees.Location = New System.Drawing.Point(11, 19)
        Me.cboEmployees.Name = "cboEmployees"
        Me.cboEmployees.Size = New System.Drawing.Size(326, 21)
        Me.cboEmployees.TabIndex = 103
        Me.cboEmployees.ValueMember = "RowID"
        '
        'btnAddHelper
        '
        Me.btnAddHelper.Location = New System.Drawing.Point(343, 19)
        Me.btnAddHelper.Name = "btnAddHelper"
        Me.btnAddHelper.Size = New System.Drawing.Size(75, 23)
        Me.btnAddHelper.TabIndex = 104
        Me.btnAddHelper.Text = "Add"
        Me.btnAddHelper.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblTruckType)
        Me.GroupBox1.Controls.Add(Me.lblPlateNo)
        Me.GroupBox1.Controls.Add(Me.txtDistance)
        Me.GroupBox1.Controls.Add(Me.txtTruckType)
        Me.GroupBox1.Controls.Add(Me.txtPlateNo)
        Me.GroupBox1.Controls.Add(Me.cboVehicles)
        Me.GroupBox1.Controls.Add(Me.lnkRoutePayMatrix)
        Me.GroupBox1.Controls.Add(Me.lblVehicle)
        Me.GroupBox1.Controls.Add(Me.VehicleDialogLink)
        Me.GroupBox1.Controls.Add(Me.cboRoutes)
        Me.GroupBox1.Controls.Add(Me.lblDistance)
        Me.GroupBox1.Controls.Add(Me.lblRoute)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 202)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(836, 151)
        Me.GroupBox1.TabIndex = 107
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Vehicle and Route"
        '
        'lblTruckType
        '
        Me.lblTruckType.AutoSize = True
        Me.lblTruckType.Location = New System.Drawing.Point(45, 73)
        Me.lblTruckType.Name = "lblTruckType"
        Me.lblTruckType.Size = New System.Drawing.Size(62, 13)
        Me.lblTruckType.TabIndex = 109
        Me.lblTruckType.Text = "Truck Type"
        '
        'lblPlateNo
        '
        Me.lblPlateNo.AutoSize = True
        Me.lblPlateNo.Location = New System.Drawing.Point(45, 48)
        Me.lblPlateNo.Name = "lblPlateNo"
        Me.lblPlateNo.Size = New System.Drawing.Size(48, 13)
        Me.lblPlateNo.TabIndex = 108
        Me.lblPlateNo.Text = "Plate No"
        '
        'txtDistance
        '
        Me.txtDistance.Location = New System.Drawing.Point(161, 120)
        Me.txtDistance.Name = "txtDistance"
        Me.txtDistance.ReadOnly = True
        Me.txtDistance.Size = New System.Drawing.Size(198, 20)
        Me.txtDistance.TabIndex = 107
        '
        'txtTruckType
        '
        Me.txtTruckType.Location = New System.Drawing.Point(161, 70)
        Me.txtTruckType.Name = "txtTruckType"
        Me.txtTruckType.ReadOnly = True
        Me.txtTruckType.Size = New System.Drawing.Size(198, 20)
        Me.txtTruckType.TabIndex = 107
        '
        'txtPlateNo
        '
        Me.txtPlateNo.Location = New System.Drawing.Point(161, 45)
        Me.txtPlateNo.Name = "txtPlateNo"
        Me.txtPlateNo.ReadOnly = True
        Me.txtPlateNo.Size = New System.Drawing.Size(198, 20)
        Me.txtPlateNo.TabIndex = 107
        '
        'cboVehicles
        '
        Me.cboVehicles.DisplayMember = "BodyNo"
        Me.cboVehicles.FormattingEnabled = True
        Me.cboVehicles.Location = New System.Drawing.Point(161, 20)
        Me.cboVehicles.Name = "cboVehicles"
        Me.cboVehicles.Size = New System.Drawing.Size(198, 21)
        Me.cboVehicles.TabIndex = 8
        Me.cboVehicles.ValueMember = "RowID"
        '
        'lnkRoutePayMatrix
        '
        Me.lnkRoutePayMatrix.AutoSize = True
        Me.lnkRoutePayMatrix.Location = New System.Drawing.Point(374, 98)
        Me.lnkRoutePayMatrix.Name = "lnkRoutePayMatrix"
        Me.lnkRoutePayMatrix.Size = New System.Drawing.Size(71, 13)
        Me.lnkRoutePayMatrix.TabIndex = 106
        Me.lnkRoutePayMatrix.TabStop = True
        Me.lnkRoutePayMatrix.Text = "Routes Table"
        '
        'lblVehicle
        '
        Me.lblVehicle.AutoSize = True
        Me.lblVehicle.Location = New System.Drawing.Point(21, 23)
        Me.lblVehicle.Name = "lblVehicle"
        Me.lblVehicle.Size = New System.Drawing.Size(42, 13)
        Me.lblVehicle.TabIndex = 9
        Me.lblVehicle.Text = "Vehicle"
        '
        'VehicleDialogLink
        '
        Me.VehicleDialogLink.AutoSize = True
        Me.VehicleDialogLink.Location = New System.Drawing.Point(374, 23)
        Me.VehicleDialogLink.Name = "VehicleDialogLink"
        Me.VehicleDialogLink.Size = New System.Drawing.Size(69, 13)
        Me.VehicleDialogLink.TabIndex = 105
        Me.VehicleDialogLink.TabStop = True
        Me.VehicleDialogLink.Text = "Add Vehicles"
        '
        'cboRoutes
        '
        Me.cboRoutes.DisplayMember = "Description"
        Me.cboRoutes.FormattingEnabled = True
        Me.cboRoutes.Location = New System.Drawing.Point(161, 95)
        Me.cboRoutes.Name = "cboRoutes"
        Me.cboRoutes.Size = New System.Drawing.Size(198, 21)
        Me.cboRoutes.TabIndex = 32
        Me.cboRoutes.ValueMember = "RowID"
        '
        'lblDistance
        '
        Me.lblDistance.AutoSize = True
        Me.lblDistance.Location = New System.Drawing.Point(45, 123)
        Me.lblDistance.Name = "lblDistance"
        Me.lblDistance.Size = New System.Drawing.Size(85, 13)
        Me.lblDistance.TabIndex = 33
        Me.lblDistance.Text = "Distance (in KM)"
        '
        'lblRoute
        '
        Me.lblRoute.AutoSize = True
        Me.lblRoute.Location = New System.Drawing.Point(21, 98)
        Me.lblRoute.Name = "lblRoute"
        Me.lblRoute.Size = New System.Drawing.Size(36, 13)
        Me.lblRoute.TabIndex = 33
        Me.lblRoute.Text = "Route"
        '
        'grpTripTicket
        '
        Me.grpTripTicket.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpTripTicket.Controls.Add(Me.lblTripDate)
        Me.grpTripTicket.Controls.Add(Me.dtpTripDate)
        Me.grpTripTicket.Controls.Add(Me.chkSpecialOperations)
        Me.grpTripTicket.Controls.Add(Me.txtTicketNo)
        Me.grpTripTicket.Controls.Add(Me.lblTicketNo)
        Me.grpTripTicket.Controls.Add(Me.lblTimeFrom)
        Me.grpTripTicket.Controls.Add(Me.lblTimeTo)
        Me.grpTripTicket.Controls.Add(Me.lblTimeDispatched)
        Me.grpTripTicket.Controls.Add(Me.txtTimeDispatched)
        Me.grpTripTicket.Controls.Add(Me.txtTimeFrom)
        Me.grpTripTicket.Controls.Add(Me.txtTimeTo)
        Me.grpTripTicket.Location = New System.Drawing.Point(3, 28)
        Me.grpTripTicket.Name = "grpTripTicket"
        Me.grpTripTicket.Size = New System.Drawing.Size(836, 168)
        Me.grpTripTicket.TabIndex = 31
        Me.grpTripTicket.TabStop = False
        Me.grpTripTicket.Text = "Trip Ticket"
        '
        'lblTripDate
        '
        Me.lblTripDate.AutoSize = True
        Me.lblTripDate.Location = New System.Drawing.Point(21, 48)
        Me.lblTripDate.Name = "lblTripDate"
        Me.lblTripDate.Size = New System.Drawing.Size(51, 13)
        Me.lblTripDate.TabIndex = 10
        Me.lblTripDate.Text = "Trip Date"
        '
        'dtpTripDate
        '
        Me.dtpTripDate.CustomFormat = "MM/dd/yyyy"
        Me.dtpTripDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTripDate.Location = New System.Drawing.Point(161, 45)
        Me.dtpTripDate.Name = "dtpTripDate"
        Me.dtpTripDate.Size = New System.Drawing.Size(198, 20)
        Me.dtpTripDate.TabIndex = 9
        '
        'chkSpecialOperations
        '
        Me.chkSpecialOperations.AutoSize = True
        Me.chkSpecialOperations.Location = New System.Drawing.Point(161, 145)
        Me.chkSpecialOperations.Name = "chkSpecialOperations"
        Me.chkSpecialOperations.Size = New System.Drawing.Size(115, 17)
        Me.chkSpecialOperations.TabIndex = 8
        Me.chkSpecialOperations.Text = "Special Operations"
        Me.chkSpecialOperations.UseVisualStyleBackColor = True
        '
        'txtTicketNo
        '
        Me.txtTicketNo.Location = New System.Drawing.Point(161, 20)
        Me.txtTicketNo.Name = "txtTicketNo"
        Me.txtTicketNo.Size = New System.Drawing.Size(198, 20)
        Me.txtTicketNo.TabIndex = 4
        '
        'lblTicketNo
        '
        Me.lblTicketNo.AutoSize = True
        Me.lblTicketNo.Location = New System.Drawing.Point(21, 23)
        Me.lblTicketNo.Name = "lblTicketNo"
        Me.lblTicketNo.Size = New System.Drawing.Size(54, 13)
        Me.lblTicketNo.TabIndex = 0
        Me.lblTicketNo.Text = "Ticket No"
        '
        'lblTimeFrom
        '
        Me.lblTimeFrom.AutoSize = True
        Me.lblTimeFrom.Location = New System.Drawing.Point(21, 73)
        Me.lblTimeFrom.Name = "lblTimeFrom"
        Me.lblTimeFrom.Size = New System.Drawing.Size(56, 13)
        Me.lblTimeFrom.TabIndex = 1
        Me.lblTimeFrom.Text = "Time From"
        '
        'lblTimeTo
        '
        Me.lblTimeTo.AutoSize = True
        Me.lblTimeTo.Location = New System.Drawing.Point(21, 98)
        Me.lblTimeTo.Name = "lblTimeTo"
        Me.lblTimeTo.Size = New System.Drawing.Size(46, 13)
        Me.lblTimeTo.TabIndex = 2
        Me.lblTimeTo.Text = "Time To"
        '
        'lblTimeDispatched
        '
        Me.lblTimeDispatched.AutoSize = True
        Me.lblTimeDispatched.Location = New System.Drawing.Point(21, 123)
        Me.lblTimeDispatched.Name = "lblTimeDispatched"
        Me.lblTimeDispatched.Size = New System.Drawing.Size(87, 13)
        Me.lblTimeDispatched.TabIndex = 3
        Me.lblTimeDispatched.Text = "Time Dispatched"
        '
        'txtTimeDispatched
        '
        Me.txtTimeDispatched.Location = New System.Drawing.Point(161, 120)
        Me.txtTimeDispatched.Name = "txtTimeDispatched"
        Me.txtTimeDispatched.Size = New System.Drawing.Size(198, 20)
        Me.txtTimeDispatched.TabIndex = 7
        '
        'txtTimeFrom
        '
        Me.txtTimeFrom.Location = New System.Drawing.Point(161, 70)
        Me.txtTimeFrom.Name = "txtTimeFrom"
        Me.txtTimeFrom.Size = New System.Drawing.Size(198, 20)
        Me.txtTimeFrom.TabIndex = 5
        '
        'txtTimeTo
        '
        Me.txtTimeTo.Location = New System.Drawing.Point(161, 95)
        Me.txtTimeTo.Name = "txtTimeTo"
        Me.txtTimeTo.Size = New System.Drawing.Size(198, 20)
        Me.txtTimeTo.TabIndex = 6
        '
        'TripTicketMenubar
        '
        Me.TripTicketMenubar.BackColor = System.Drawing.Color.Transparent
        Me.TripTicketMenubar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.TripTicketMenubar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNewTripTicket, Me.btnSaveTripTicket, Me.btnCancel, Me.tsbtnClose, Me.btnImportTripTicket})
        Me.TripTicketMenubar.Location = New System.Drawing.Point(0, 0)
        Me.TripTicketMenubar.Name = "TripTicketMenubar"
        Me.TripTicketMenubar.Size = New System.Drawing.Size(842, 25)
        Me.TripTicketMenubar.TabIndex = 29
        Me.TripTicketMenubar.Text = "ToolStrip1"
        '
        'btnNewTripTicket
        '
        Me.btnNewTripTicket.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNewTripTicket.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNewTripTicket.Name = "btnNewTripTicket"
        Me.btnNewTripTicket.Size = New System.Drawing.Size(109, 22)
        Me.btnNewTripTicket.Text = "&New Trip Ticket"
        Me.btnNewTripTicket.ToolTipText = "New Trip Ticket"
        '
        'btnSaveTripTicket
        '
        Me.btnSaveTripTicket.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.btnSaveTripTicket.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSaveTripTicket.Name = "btnSaveTripTicket"
        Me.btnSaveTripTicket.Size = New System.Drawing.Size(109, 22)
        Me.btnSaveTripTicket.Text = "&Save Trip Ticket"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "Cancel"
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnClose.Name = "tsbtnClose"
        Me.tsbtnClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnClose.Text = "Close"
        '
        'btnImportTripTicket
        '
        Me.btnImportTripTicket.Image = CType(resources.GetObject("btnImportTripTicket.Image"), System.Drawing.Image)
        Me.btnImportTripTicket.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnImportTripTicket.Name = "btnImportTripTicket"
        Me.btnImportTripTicket.Size = New System.Drawing.Size(121, 22)
        Me.btnImportTripTicket.Text = "Import Trip Ticket"
        '
        'dgvTripTickets
        '
        Me.dgvTripTickets.AllowUserToAddRows = False
        Me.dgvTripTickets.AllowUserToDeleteRows = False
        Me.dgvTripTickets.AllowUserToOrderColumns = True
        Me.dgvTripTickets.AllowUserToResizeRows = False
        Me.dgvTripTickets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvTripTickets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTripTickets.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvTripTickets.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.dgvTripTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTripTickets.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvTripTicketsRowID, Me.dgvTripTicketsTicketNo, Me.dgvTripTicketsTripDate, Me.dgvTripTicketsRoute})
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvTripTickets.DefaultCellStyle = DataGridViewCellStyle8
        Me.dgvTripTickets.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvTripTickets.Location = New System.Drawing.Point(8, 32)
        Me.dgvTripTickets.MultiSelect = False
        Me.dgvTripTickets.Name = "dgvTripTickets"
        Me.dgvTripTickets.ReadOnly = True
        Me.dgvTripTickets.RowHeadersWidth = 25
        Me.dgvTripTickets.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvTripTickets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTripTickets.Size = New System.Drawing.Size(384, 664)
        Me.dgvTripTickets.TabIndex = 102
        '
        'dgvTripTicketsRowID
        '
        Me.dgvTripTicketsRowID.DataPropertyName = "RowID"
        Me.dgvTripTicketsRowID.HeaderText = "RowID"
        Me.dgvTripTicketsRowID.Name = "dgvTripTicketsRowID"
        Me.dgvTripTicketsRowID.ReadOnly = True
        '
        'dgvTripTicketsTicketNo
        '
        Me.dgvTripTicketsTicketNo.DataPropertyName = "TicketNo"
        Me.dgvTripTicketsTicketNo.HeaderText = "Ticket No"
        Me.dgvTripTicketsTicketNo.Name = "dgvTripTicketsTicketNo"
        Me.dgvTripTicketsTicketNo.ReadOnly = True
        '
        'dgvTripTicketsTripDate
        '
        Me.dgvTripTicketsTripDate.DataPropertyName = "TripDate"
        Me.dgvTripTicketsTripDate.HeaderText = "Date"
        Me.dgvTripTicketsTripDate.Name = "dgvTripTicketsTripDate"
        Me.dgvTripTicketsTripDate.ReadOnly = True
        '
        'dgvTripTicketsRoute
        '
        Me.dgvTripTicketsRoute.DataPropertyName = "RouteName"
        Me.dgvTripTicketsRoute.HeaderText = "Route"
        Me.dgvTripTicketsRoute.Name = "dgvTripTicketsRoute"
        Me.dgvTripTicketsRoute.ReadOnly = True
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(1265, 24)
        Me.Label16.TabIndex = 314
        Me.Label16.Text = "TRIP TICKETS"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.lblSearch)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Controls.Add(Me.dgvTripTickets)
        Me.Panel1.Location = New System.Drawing.Point(4, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(401, 708)
        Me.Panel1.TabIndex = 30
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(8, 8)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(41, 13)
        Me.lblSearch.TabIndex = 104
        Me.lblSearch.Text = "Search"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(64, 8)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(328, 20)
        Me.TextBox1.TabIndex = 103
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "RowID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 80
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "TicketNo"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Ticket No"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 80
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "TripDate"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Date"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 80
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "TicketNo"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Route"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 80
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "TripDate"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Date"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 80
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "TripDate"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Route"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 80
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "TripDate"
        Me.DataGridViewTextBoxColumn7.HeaderText = "Route"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.Width = 80
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "Route"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.Width = 80
        '
        'TripTicketForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1265, 739)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.pnlTripTicket)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "TripTicketForm"
        Me.Text = "RouteTripForm"
        Me.pnlTripTicket.ResumeLayout(False)
        Me.pnlTripTicket.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.dgvTripTicketHelpers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpTripTicket.ResumeLayout(False)
        Me.grpTripTicket.PerformLayout()
        Me.TripTicketMenubar.ResumeLayout(False)
        Me.TripTicketMenubar.PerformLayout()
        CType(Me.dgvTripTickets, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlTripTicket As System.Windows.Forms.Panel
    Friend WithEvents txtTimeDispatched As System.Windows.Forms.TextBox
    Friend WithEvents txtTimeTo As System.Windows.Forms.TextBox
    Friend WithEvents txtTimeFrom As System.Windows.Forms.TextBox
    Friend WithEvents txtTicketNo As System.Windows.Forms.TextBox
    Friend WithEvents lblTimeDispatched As System.Windows.Forms.Label
    Friend WithEvents lblTimeTo As System.Windows.Forms.Label
    Friend WithEvents lblTimeFrom As System.Windows.Forms.Label
    Friend WithEvents lblTicketNo As System.Windows.Forms.Label
    Friend WithEvents TripTicketMenubar As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNewTripTicket As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSaveTripTicket As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvTripTickets As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents cboVehicles As System.Windows.Forms.ComboBox
    Friend WithEvents lblVehicle As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents grpTripTicket As System.Windows.Forms.GroupBox
    Friend WithEvents lblRoute As System.Windows.Forms.Label
    Friend WithEvents cboRoutes As System.Windows.Forms.ComboBox
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvTripTicketHelpers As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents cboEmployees As System.Windows.Forms.ComboBox
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chkSpecialOperations As System.Windows.Forms.CheckBox
    Friend WithEvents lblTripDate As System.Windows.Forms.Label
    Friend WithEvents dtpTripDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnAddHelper As System.Windows.Forms.Button
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents VehicleDialogLink As System.Windows.Forms.LinkLabel
    Friend WithEvents dgvTripTicketsRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvTripTicketsTicketNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvTripTicketsTripDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvTripTicketsRoute As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lnkRoutePayMatrix As System.Windows.Forms.LinkLabel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtTruckType As System.Windows.Forms.TextBox
    Friend WithEvents txtPlateNo As System.Windows.Forms.TextBox
    Friend WithEvents lblPlateNo As System.Windows.Forms.Label
    Friend WithEvents lblTruckType As System.Windows.Forms.Label
    Friend WithEvents txtDistance As System.Windows.Forms.TextBox
    Friend WithEvents lblDistance As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents dgvEmployeesName As DataGridViewTextBoxColumn
    Friend WithEvents colTripTicketHelpersPosition As DataGridViewTextBoxColumn
    Friend WithEvents dgvEmployeesPayment As DataGridViewTextBoxColumn
    Friend WithEvents colTripTicketHelpersRemove As DevComponents.DotNetBar.Controls.DataGridViewButtonXColumn
    Friend WithEvents btnImportTripTicket As ToolStripButton
End Class
