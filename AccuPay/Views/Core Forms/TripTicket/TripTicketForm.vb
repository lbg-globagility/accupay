Option Explicit On
Option Strict On

Imports Payroll.Routes

Public Class TripTicketForm

    Public Enum TripTicketFormContext
        NewlyCreated
        Selected
        NoneSelected
    End Enum

    Public Event TripTicketCreated()
    Public Event TripTicketSaved()
    Public Event TripTicketCancelChanges()
    Public Event EmployeeWasAdded(employeeID As Integer?)
    Public Event TripTicketSelected(tripTicketID As Integer?)
    Public Event RouteHasChanged(routeID As Integer?)

    Public Event SpecialOperationsToggled(isSpecialOperations As Boolean)

    Private controller As TripTicketController

    Public Property TripTicketsSource As BindingSource
    Public Property TripTicketHelpersSource As BindingSource

    Public ReadOnly Property TimeTo As TimeSpan
        Get
            Return TimeSpan.Parse(txtTimeTo.Text)
        End Get
    End Property

    Public ReadOnly Property TimeDispatched As TimeSpan
        Get
            Return TimeSpan.Parse(txtTimeDispatched.Text)
        End Get
    End Property

    Public ReadOnly Property TripDate As DateTime?
        Get
            Return dtpTripDate.Value
        End Get
    End Property

    Public ReadOnly Property VehicleID As Integer?
        Get
            Dim vehicle = DirectCast(cboVehicles.SelectedItem, Vehicle)
            Return If(vehicle Is Nothing, Nothing, vehicle.RowID)
        End Get
    End Property

    Public ReadOnly Property RouteID As Integer?
        Get
            Dim route = DirectCast(cboRoutes.SelectedItem, Route)
            Return If(route Is Nothing, Nothing, route.RowID)
        End Get
    End Property

    Public Sub ChangeViewContext(context As TripTicketFormContext)
        If context = TripTicketFormContext.NewlyCreated Then
            btnNewTripTicket.Enabled = False
            btnSaveTripTicket.Enabled = True
            btnCancel.Enabled = True
            grpTripTicket.Enabled = True
        ElseIf context = TripTicketFormContext.Selected Then
            btnNewTripTicket.Enabled = True
            btnSaveTripTicket.Enabled = True
            btnCancel.Enabled = True
            grpTripTicket.Enabled = True
        ElseIf context = TripTicketFormContext.NoneSelected Then
            btnNewTripTicket.Enabled = True
            btnSaveTripTicket.Enabled = False
            btnCancel.Enabled = False
            grpTripTicket.Enabled = False
        End If
    End Sub

    Public Sub DisplayTripTicket(tripTicket As TripTicket)
        txtTicketNo.DataBindings.Clear()
        txtTicketNo.DataBindings.Add("Text", controller, "TimeTo")

        txtTimeFrom.DataBindings.Clear()
        txtTimeFrom.DataBindings.Add("Text", controller, "TimeFrom")

        txtTimeTo.DataBindings.Clear()
        txtTimeTo.DataBindings.Add("Text", controller, "TimeTo")

        txtTimeDispatched.DataBindings.Clear()
        txtTimeDispatched.DataBindings.Add("Text", controller, "TimeDispatched")

        'txtTicketNo.Text = tripTicket.TicketNo
        'txtTimeFrom.Text = tripTicket.TimeFrom.ToString()
        'txtTimeTo.Text = tripTicket.TimeTo.ToString()
        'txtTimeDispatched.Text = tripTicket.TimeDispatched.ToString()

        ChangeSelectedVehicle(tripTicket.VehicleID)
        ChangeSelectedRoute(tripTicket.RouteID)
    End Sub

    Public Sub ClearDisplay()
        'txtTicketNo.Text = Nothing
        'txtTimeFrom.Text = Nothing
        'txtTimeTo.Text = Nothing
        'txtTimeDispatched.Text = Nothing
        cboVehicles.SelectedIndex = -1
        cboRoutes.SelectedIndex = -1
        cboEmployees.SelectedIndex = -1
    End Sub

    Private Sub Startup()
        InitializeComboBoxes()
        InitializeTripTickets()
        InitializeTripTicketHelpers()
        Me.controller = New TripTicketController(Me)
    End Sub

    Private Sub InitializeComboBoxes()
        cboVehicles.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboVehicles.AutoCompleteSource = AutoCompleteSource.ListItems

        cboRoutes.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboRoutes.AutoCompleteSource = AutoCompleteSource.ListItems

        cboEmployees.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboEmployees.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub InitializeTripTickets()
        TripTicketsSource = New BindingSource()
        dgvTripTickets.AutoGenerateColumns = False
        dgvTripTickets.DataSource = TripTicketsSource
    End Sub

    Private Sub InitializeTripTicketHelpers()
        TripTicketHelpersSource = New BindingSource()
        dgvTripTicketHelpers.AutoGenerateColumns = False
        dgvTripTicketHelpers.DataSource = TripTicketHelpersSource
    End Sub

    Private Sub ChangeSelectedVehicle(vehicleID As Integer?)
        If vehicleID.HasValue Then
            cboVehicles.SelectedValue = vehicleID
        Else
            cboVehicles.SelectedIndex = -1
        End If
    End Sub

    Private Sub ChangeSelectedRoute(routeID As Integer?)
        If routeID.HasValue Then
            cboRoutes.SelectedValue = routeID
        Else
            cboRoutes.SelectedIndex = -1
        End If
    End Sub

    Private Sub btnNewTripTicket_Click(sender As Object, e As EventArgs) Handles btnNewTripTicket.Click
        RaiseEvent TripTicketCreated()
    End Sub

    Private Sub btnSaveTripTicket_Click(sender As Object, e As EventArgs) Handles btnSaveTripTicket.Click
        RaiseEvent TripTicketSaved()
    End Sub

    Private Sub TripTicketForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Startup()
    End Sub

    Private Sub dgvTripTickets_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTripTickets.CellClick
        Dim tripTicket = DirectCast(dgvTripTickets.CurrentRow.DataBoundItem, TripTicket)

        RaiseEvent TripTicketSelected(tripTicket.RowID)
    End Sub

    Private Sub btnAddHelper_Click(sender As Object, e As EventArgs) Handles btnAddHelper.Click
        If cboEmployees.SelectedItem Is Nothing Then
            Return
        End If

        Dim employee = DirectCast(cboEmployees.SelectedItem, Routes.Employee)
        RaiseEvent EmployeeWasAdded(employee.RowID)

        cboEmployees.SelectedItem = Nothing
    End Sub

    Private Sub VehicleDialogLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles VehicleDialogLink.LinkClicked
        VehicleForm.ShowDialog()
    End Sub

    Private Sub chkSpecialOperations_CheckedChanged(sender As Object, e As EventArgs) Handles chkSpecialOperations.CheckedChanged
        RaiseEvent SpecialOperationsToggled(chkSpecialOperations.Checked)
    End Sub

    Private Sub dgvTripTicketHelpers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTripTicketHelpers.CellClick
        ' Implement remove
    End Sub

    Private Sub cboRoutes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRoutes.SelectedIndexChanged
        Dim route = DirectCast(cboRoutes.SelectedItem, Route)

        If route Is Nothing Then
            Return
        End If

        txtDistance.Text = CStr(route.Distance)

        RaiseEvent RouteHasChanged(route.RowID)
    End Sub

    Private Sub lnkRoutePayMatrix_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkRoutePayMatrix.LinkClicked
        RoutePayRateMatrixForm.ShowDialog()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ' Should clear form
    End Sub

    Private Sub cboVehicles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboVehicles.SelectedIndexChanged
        Dim vehicle = DirectCast(cboVehicles.SelectedItem, Vehicle)

        If vehicle Is Nothing Then
            Return
        End If

        txtPlateNo.Text = vehicle.PlateNo
        txtTruckType.Text = vehicle.TruckType
    End Sub

End Class