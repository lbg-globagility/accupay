<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewTripTicketDialog
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
        Me.grpTripTicket = New System.Windows.Forms.GroupBox()
        Me.lblTripDate = New System.Windows.Forms.Label()
        Me.DatePicker = New System.Windows.Forms.DateTimePicker()
        Me.chkSpecialOperations = New System.Windows.Forms.CheckBox()
        Me.TicketNoTextBox = New System.Windows.Forms.TextBox()
        Me.lblTicketNo = New System.Windows.Forms.Label()
        Me.lblTimeFrom = New System.Windows.Forms.Label()
        Me.lblTimeTo = New System.Windows.Forms.Label()
        Me.lblTimeDispatched = New System.Windows.Forms.Label()
        Me.TimeDispatchedTextBox = New System.Windows.Forms.TextBox()
        Me.TimeFromTextBox = New System.Windows.Forms.TextBox()
        Me.TimeToTextBox = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblTruckType = New System.Windows.Forms.Label()
        Me.lblPlateNo = New System.Windows.Forms.Label()
        Me.txtDistance = New System.Windows.Forms.TextBox()
        Me.txtTruckType = New System.Windows.Forms.TextBox()
        Me.txtPlateNo = New System.Windows.Forms.TextBox()
        Me.VehicleComboBox = New System.Windows.Forms.ComboBox()
        Me.lnkRoutePayMatrix = New System.Windows.Forms.LinkLabel()
        Me.lblVehicle = New System.Windows.Forms.Label()
        Me.VehicleDialogLink = New System.Windows.Forms.LinkLabel()
        Me.RouteComboBox = New System.Windows.Forms.ComboBox()
        Me.lblDistance = New System.Windows.Forms.Label()
        Me.lblRoute = New System.Windows.Forms.Label()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.grpTripTicket.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpTripTicket
        '
        Me.grpTripTicket.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpTripTicket.Controls.Add(Me.lblTripDate)
        Me.grpTripTicket.Controls.Add(Me.DatePicker)
        Me.grpTripTicket.Controls.Add(Me.chkSpecialOperations)
        Me.grpTripTicket.Controls.Add(Me.TicketNoTextBox)
        Me.grpTripTicket.Controls.Add(Me.lblTicketNo)
        Me.grpTripTicket.Controls.Add(Me.lblTimeFrom)
        Me.grpTripTicket.Controls.Add(Me.lblTimeTo)
        Me.grpTripTicket.Controls.Add(Me.lblTimeDispatched)
        Me.grpTripTicket.Controls.Add(Me.TimeDispatchedTextBox)
        Me.grpTripTicket.Controls.Add(Me.TimeFromTextBox)
        Me.grpTripTicket.Controls.Add(Me.TimeToTextBox)
        Me.grpTripTicket.Location = New System.Drawing.Point(12, 12)
        Me.grpTripTicket.Name = "grpTripTicket"
        Me.grpTripTicket.Size = New System.Drawing.Size(481, 168)
        Me.grpTripTicket.TabIndex = 32
        Me.grpTripTicket.TabStop = False
        Me.grpTripTicket.Text = "Ticket Details"
        '
        'lblTripDate
        '
        Me.lblTripDate.AutoSize = True
        Me.lblTripDate.Location = New System.Drawing.Point(21, 48)
        Me.lblTripDate.Name = "lblTripDate"
        Me.lblTripDate.Size = New System.Drawing.Size(53, 13)
        Me.lblTripDate.TabIndex = 10
        Me.lblTripDate.Text = "Trip Date"
        '
        'DatePicker
        '
        Me.DatePicker.CustomFormat = "MM/dd/yyyy"
        Me.DatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DatePicker.Location = New System.Drawing.Point(161, 45)
        Me.DatePicker.Name = "DatePicker"
        Me.DatePicker.Size = New System.Drawing.Size(198, 22)
        Me.DatePicker.TabIndex = 9
        '
        'chkSpecialOperations
        '
        Me.chkSpecialOperations.AutoSize = True
        Me.chkSpecialOperations.Location = New System.Drawing.Point(161, 145)
        Me.chkSpecialOperations.Name = "chkSpecialOperations"
        Me.chkSpecialOperations.Size = New System.Drawing.Size(123, 17)
        Me.chkSpecialOperations.TabIndex = 8
        Me.chkSpecialOperations.Text = "Special Operations"
        Me.chkSpecialOperations.UseVisualStyleBackColor = True
        '
        'TicketNoTextBox
        '
        Me.TicketNoTextBox.Location = New System.Drawing.Point(161, 20)
        Me.TicketNoTextBox.Name = "TicketNoTextBox"
        Me.TicketNoTextBox.Size = New System.Drawing.Size(198, 22)
        Me.TicketNoTextBox.TabIndex = 4
        '
        'lblTicketNo
        '
        Me.lblTicketNo.AutoSize = True
        Me.lblTicketNo.Location = New System.Drawing.Point(21, 23)
        Me.lblTicketNo.Name = "lblTicketNo"
        Me.lblTicketNo.Size = New System.Drawing.Size(55, 13)
        Me.lblTicketNo.TabIndex = 0
        Me.lblTicketNo.Text = "Ticket No"
        '
        'lblTimeFrom
        '
        Me.lblTimeFrom.AutoSize = True
        Me.lblTimeFrom.Location = New System.Drawing.Point(21, 73)
        Me.lblTimeFrom.Name = "lblTimeFrom"
        Me.lblTimeFrom.Size = New System.Drawing.Size(60, 13)
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
        Me.lblTimeDispatched.Size = New System.Drawing.Size(92, 13)
        Me.lblTimeDispatched.TabIndex = 3
        Me.lblTimeDispatched.Text = "Time Dispatched"
        '
        'TimeDispatchedTextBox
        '
        Me.TimeDispatchedTextBox.Location = New System.Drawing.Point(161, 120)
        Me.TimeDispatchedTextBox.Name = "TimeDispatchedTextBox"
        Me.TimeDispatchedTextBox.Size = New System.Drawing.Size(198, 22)
        Me.TimeDispatchedTextBox.TabIndex = 7
        '
        'TimeFromTextBox
        '
        Me.TimeFromTextBox.Location = New System.Drawing.Point(161, 70)
        Me.TimeFromTextBox.Name = "TimeFromTextBox"
        Me.TimeFromTextBox.Size = New System.Drawing.Size(198, 22)
        Me.TimeFromTextBox.TabIndex = 5
        '
        'TimeToTextBox
        '
        Me.TimeToTextBox.Location = New System.Drawing.Point(161, 95)
        Me.TimeToTextBox.Name = "TimeToTextBox"
        Me.TimeToTextBox.Size = New System.Drawing.Size(198, 22)
        Me.TimeToTextBox.TabIndex = 6
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
        Me.GroupBox1.Controls.Add(Me.VehicleComboBox)
        Me.GroupBox1.Controls.Add(Me.lnkRoutePayMatrix)
        Me.GroupBox1.Controls.Add(Me.lblVehicle)
        Me.GroupBox1.Controls.Add(Me.VehicleDialogLink)
        Me.GroupBox1.Controls.Add(Me.RouteComboBox)
        Me.GroupBox1.Controls.Add(Me.lblDistance)
        Me.GroupBox1.Controls.Add(Me.lblRoute)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 186)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(481, 151)
        Me.GroupBox1.TabIndex = 108
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Vehicle and Route"
        '
        'lblTruckType
        '
        Me.lblTruckType.AutoSize = True
        Me.lblTruckType.Location = New System.Drawing.Point(45, 73)
        Me.lblTruckType.Name = "lblTruckType"
        Me.lblTruckType.Size = New System.Drawing.Size(60, 13)
        Me.lblTruckType.TabIndex = 109
        Me.lblTruckType.Text = "Truck Type"
        '
        'lblPlateNo
        '
        Me.lblPlateNo.AutoSize = True
        Me.lblPlateNo.Location = New System.Drawing.Point(45, 48)
        Me.lblPlateNo.Name = "lblPlateNo"
        Me.lblPlateNo.Size = New System.Drawing.Size(50, 13)
        Me.lblPlateNo.TabIndex = 108
        Me.lblPlateNo.Text = "Plate No"
        '
        'txtDistance
        '
        Me.txtDistance.Location = New System.Drawing.Point(161, 120)
        Me.txtDistance.Name = "txtDistance"
        Me.txtDistance.ReadOnly = True
        Me.txtDistance.Size = New System.Drawing.Size(198, 22)
        Me.txtDistance.TabIndex = 107
        '
        'txtTruckType
        '
        Me.txtTruckType.Location = New System.Drawing.Point(161, 70)
        Me.txtTruckType.Name = "txtTruckType"
        Me.txtTruckType.ReadOnly = True
        Me.txtTruckType.Size = New System.Drawing.Size(198, 22)
        Me.txtTruckType.TabIndex = 107
        '
        'txtPlateNo
        '
        Me.txtPlateNo.Location = New System.Drawing.Point(161, 45)
        Me.txtPlateNo.Name = "txtPlateNo"
        Me.txtPlateNo.ReadOnly = True
        Me.txtPlateNo.Size = New System.Drawing.Size(198, 22)
        Me.txtPlateNo.TabIndex = 107
        '
        'VehicleComboBox
        '
        Me.VehicleComboBox.DisplayMember = "PlateNo"
        Me.VehicleComboBox.FormattingEnabled = True
        Me.VehicleComboBox.Location = New System.Drawing.Point(161, 20)
        Me.VehicleComboBox.Name = "VehicleComboBox"
        Me.VehicleComboBox.Size = New System.Drawing.Size(198, 21)
        Me.VehicleComboBox.TabIndex = 8
        Me.VehicleComboBox.ValueMember = "RowID"
        '
        'lnkRoutePayMatrix
        '
        Me.lnkRoutePayMatrix.AutoSize = True
        Me.lnkRoutePayMatrix.Location = New System.Drawing.Point(374, 98)
        Me.lnkRoutePayMatrix.Name = "lnkRoutePayMatrix"
        Me.lnkRoutePayMatrix.Size = New System.Drawing.Size(73, 13)
        Me.lnkRoutePayMatrix.TabIndex = 106
        Me.lnkRoutePayMatrix.TabStop = True
        Me.lnkRoutePayMatrix.Text = "Routes Table"
        '
        'lblVehicle
        '
        Me.lblVehicle.AutoSize = True
        Me.lblVehicle.Location = New System.Drawing.Point(21, 23)
        Me.lblVehicle.Name = "lblVehicle"
        Me.lblVehicle.Size = New System.Drawing.Size(43, 13)
        Me.lblVehicle.TabIndex = 9
        Me.lblVehicle.Text = "Vehicle"
        '
        'VehicleDialogLink
        '
        Me.VehicleDialogLink.AutoSize = True
        Me.VehicleDialogLink.Location = New System.Drawing.Point(374, 23)
        Me.VehicleDialogLink.Name = "VehicleDialogLink"
        Me.VehicleDialogLink.Size = New System.Drawing.Size(72, 13)
        Me.VehicleDialogLink.TabIndex = 105
        Me.VehicleDialogLink.TabStop = True
        Me.VehicleDialogLink.Text = "Add Vehicles"
        '
        'RouteComboBox
        '
        Me.RouteComboBox.DisplayMember = "Description"
        Me.RouteComboBox.FormattingEnabled = True
        Me.RouteComboBox.Location = New System.Drawing.Point(161, 95)
        Me.RouteComboBox.Name = "RouteComboBox"
        Me.RouteComboBox.Size = New System.Drawing.Size(198, 21)
        Me.RouteComboBox.TabIndex = 32
        Me.RouteComboBox.ValueMember = "RowID"
        '
        'lblDistance
        '
        Me.lblDistance.AutoSize = True
        Me.lblDistance.Location = New System.Drawing.Point(45, 123)
        Me.lblDistance.Name = "lblDistance"
        Me.lblDistance.Size = New System.Drawing.Size(89, 13)
        Me.lblDistance.TabIndex = 33
        Me.lblDistance.Text = "Distance (in KM)"
        '
        'lblRoute
        '
        Me.lblRoute.AutoSize = True
        Me.lblRoute.Location = New System.Drawing.Point(21, 98)
        Me.lblRoute.Name = "lblRoute"
        Me.lblRoute.Size = New System.Drawing.Size(38, 13)
        Me.lblRoute.TabIndex = 33
        Me.lblRoute.Text = "Route"
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(307, 343)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(104, 32)
        Me.SaveButton.TabIndex = 109
        Me.SaveButton.Text = "Create Trip Ticket"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Location = New System.Drawing.Point(417, 343)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 32)
        Me.CancelButton.TabIndex = 110
        Me.CancelButton.Text = "Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'NewTripTicketDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(505, 381)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpTripTicket)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "NewTripTicketDialog"
        Me.Text = "New Trip Ticket"
        Me.grpTripTicket.ResumeLayout(False)
        Me.grpTripTicket.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpTripTicket As GroupBox
    Friend WithEvents lblTripDate As Label
    Friend WithEvents DatePicker As DateTimePicker
    Friend WithEvents chkSpecialOperations As CheckBox
    Friend WithEvents TicketNoTextBox As TextBox
    Friend WithEvents lblTicketNo As Label
    Friend WithEvents lblTimeFrom As Label
    Friend WithEvents lblTimeTo As Label
    Friend WithEvents lblTimeDispatched As Label
    Friend WithEvents TimeDispatchedTextBox As TextBox
    Friend WithEvents TimeFromTextBox As TextBox
    Friend WithEvents TimeToTextBox As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblTruckType As Label
    Friend WithEvents lblPlateNo As Label
    Friend WithEvents txtDistance As TextBox
    Friend WithEvents txtTruckType As TextBox
    Friend WithEvents txtPlateNo As TextBox
    Friend WithEvents VehicleComboBox As ComboBox
    Friend WithEvents lnkRoutePayMatrix As LinkLabel
    Friend WithEvents lblVehicle As Label
    Friend WithEvents VehicleDialogLink As LinkLabel
    Friend WithEvents RouteComboBox As ComboBox
    Friend WithEvents lblDistance As Label
    Friend WithEvents lblRoute As Label
    Friend WithEvents SaveButton As Button
    Friend WithEvents CancelButton As Button
End Class
