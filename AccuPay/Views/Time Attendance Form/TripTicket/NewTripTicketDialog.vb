Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class NewTripTicketDialog

    Private ReadOnly _tripTicketRepository As TripTicketRepository
    Private ReadOnly _vehicleRepository As VehicleRepository
    Private ReadOnly _routeRepository As RouteRepository

    Public Sub New()
        InitializeComponent()

        _tripTicketRepository = MainServiceProvider.GetRequiredService(Of TripTicketRepository)
        _vehicleRepository = MainServiceProvider.GetRequiredService(Of VehicleRepository)
        _routeRepository = MainServiceProvider.GetRequiredService(Of RouteRepository)
    End Sub

    Private Async Sub NewTripTicketDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadVehicles()
        Await LoadRoutes()
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        SaveButton.Enabled = False

        Dim tripTicket = New TripTicket() With {
            .CreatedBy = z_User,
            .OrganizationID = z_OrganizationID
        }

        tripTicket.TicketNo = TicketNoTextBox.Text
        tripTicket.Date = DatePicker.Value

        Dim timeFrom As TimeSpan
        If TimeSpan.TryParse(TimeFromTextBox.Text, timeFrom) Then
            tripTicket.TimeFrom = timeFrom
        End If

        Dim timeTo As TimeSpan
        If TimeSpan.TryParse(TimeToTextBox.Text, timeTo) Then
            tripTicket.TimeTo = timeTo
        End If

        tripTicket.IsSpecialOperations = chkSpecialOperations.Checked
        tripTicket.VehicleID = DirectCast(VehicleComboBox.SelectedValue, Integer?)
        tripTicket.RouteID = DirectCast(RouteComboBox.SelectedValue, Integer?)

        Await _tripTicketRepository.Create(tripTicket)

        SaveButton.Enabled = True

        DialogResult = DialogResult.OK
    End Sub

    Private Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Private Async Function LoadVehicles() As Task
        Dim vehicles = Await _vehicleRepository.GetAll()
        VehicleComboBox.DataSource = vehicles
        VehicleComboBox.SelectedIndex = -1
    End Function

    Private Async Function LoadRoutes() As Task
        Dim routes = Await _routeRepository.GetAll()
        RouteComboBox.DataSource = routes
        RouteComboBox.SelectedIndex = -1
    End Function

End Class
