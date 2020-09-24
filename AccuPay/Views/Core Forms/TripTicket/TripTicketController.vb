Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class TripTicketController
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Class TimeSpanParser

        Private Const Expression24HourWithColons = "^([\d]|0[\d]|1[\d]|2[0-3]):[0-5][\d]$"

        Private Const Expression24HourWithSpaces = "^([\d]|0[\d]|1[\d]|2[0-3])\s[0-5][\d]$"

        Private Const Expression24HourNoSeparator = "^([\d]|0[\d]|1[\d]|2[0-3])[0-5][\d]$"

        Public Sub Parse(value As String)

        End Sub

    End Class

    Private WithEvents View As TripTicketForm

    Private ReadOnly _tripTicketRepository As TripTicketRepository
    Private ReadOnly _vehicleRepository As VehicleRepository
    Private ReadOnly _routeRepository As RouteRepository
    Private ReadOnly _employeeRepository As EmployeeRepository
    Private ReadOnly _routeRateRepository As RouteRateRepository
    Private ReadOnly _listOfValueRepository As ListOfValueRepository

    Private _tripTicket As TripTicket
    Private _tripTickets As ICollection(Of TripTicket)
    Private _vehicles As ICollection(Of Vehicle)
    Private _routes As ICollection(Of Route)
    Private _employees As ICollection(Of Employee)
    Private _tripTicketHelpers As ICollection(Of TripTicketEmployee)
    Private standardMinimumWage As Decimal
    Private _routeRates As ICollection(Of RoutePayRate)

    Public Property TicketNo As String
        Get
            Return _tripTicket?.TicketNo
        End Get
        Set(value As String)
            _tripTicket.TicketNo = value
            NotifyPropertyChanged("TicketNo")
        End Set
    End Property

    Public Property TripDate As Date
        Get
            Return If(_tripTicket Is Nothing, DateTime.Today(), _tripTicket.Date.Value)
        End Get
        Set(value As Date)
            _tripTicket.Date = value
            NotifyPropertyChanged("TripDate")
        End Set
    End Property

    Public Property TimeFrom As String
        Get
            Return _tripTicket?.TimeFrom.ToString()
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then Return

            _tripTicket.TimeFrom = TimeSpan.Parse(value)
            NotifyPropertyChanged("TimeFrom")
        End Set
    End Property

    Public Property TimeTo As String
        Get
            Return _tripTicket?.TimeTo.ToString()
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then Return

            _tripTicket.TimeTo = TimeSpan.Parse(value)
            NotifyPropertyChanged("TimeTo")
        End Set
    End Property

    Public Property TimeDispatched As String
        Get
            Return If(_tripTicket Is Nothing, Nothing, _tripTicket.TimeDispatched.ToString())
        End Get
        Set(value As String)
            _tripTicket.TimeDispatched = TimeSpan.Parse(value)
            NotifyPropertyChanged("TimeDispatched")
        End Set
    End Property

    Public Property Route As Route
        Get
            Return _tripTicket?.Route
        End Get
        Set(value As Route)
            _tripTicket.Route = value
            NotifyPropertyChanged("Route")
            NotifyPropertyChanged("Distance")
        End Set
    End Property

    Public ReadOnly Property Distance As String
        Get
            If _tripTicket Is Nothing Then : Return Nothing : End If
            If _tripTicket.Route Is Nothing Then : Return Nothing : End If

            Return CStr(_tripTicket.Route.Distance)
        End Get
    End Property

    Public Property Vehicle As Vehicle
        Get
            Return _tripTicket?.Vehicle
        End Get
        Set(value As Vehicle)
            _tripTicket.Vehicle = value
            NotifyPropertyChanged("Vehicle")
            NotifyPropertyChanged("PlateNo")
        End Set
    End Property

    Public ReadOnly Property PlateNo As String
        Get
            If _tripTicket Is Nothing Then : Return Nothing : End If
            If _tripTicket.Vehicle Is Nothing Then : Return Nothing : End If

            Return _tripTicket.Vehicle.PlateNo
        End Get
    End Property

    Public ReadOnly Property TruckType As String
        Get
            If _tripTicket Is Nothing Then : Return Nothing : End If
            If _tripTicket.Vehicle Is Nothing Then : Return Nothing : End If

            Return _tripTicket.Vehicle.TruckType
        End Get
    End Property

    Public Sub New(view As TripTicketForm)
        Me.View = view

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
        _tripTicketRepository = MainServiceProvider.GetRequiredService(Of TripTicketRepository)
        _routeRepository = MainServiceProvider.GetRequiredService(Of RouteRepository)
        _routeRateRepository = MainServiceProvider.GetRequiredService(Of RouteRateRepository)
        _vehicleRepository = MainServiceProvider.GetRequiredService(Of VehicleRepository)
        _listOfValueRepository = MainServiceProvider.GetRequiredService(Of ListOfValueRepository)

        Start()
    End Sub

    Public Async Sub Start()
        Await LoadTripTickets()
        Await LoadVehicles()
        Await LoadRoutes()
        Await LoadEmployees()
        Await LoadMinimumWageRate()
        Await LoadRoutePayRates()
    End Sub

    Private Async Function LoadTripTicket(tripTicketID As Integer?) As Task
        _tripTicket = Await _tripTicketRepository.FindById(tripTicketID)
    End Function

    Private Async Function LoadTripTicketHelpers(tripTicketID As Integer?) As Task
        Dim tripTicketEmployees = Await _tripTicketRepository.GetTripTicketEmployees(tripTicketID)

        _tripTicketHelpers = tripTicketEmployees
    End Function

    Private Async Function LoadTripTickets() As Task
        Dim tripTickets = Await _tripTicketRepository.GetAll()

        _tripTickets = tripTickets
        View.TripTicketsSource.DataSource = tripTickets
    End Function

    Private Async Function LoadEmployees() As Task
        Dim employees = Await _employeeRepository.GetAllWithPositionAsync(z_OrganizationID)

        _employees = employees.ToList()
        View.cboEmployees.DataSource = _employees
        View.cboEmployees.SelectedItem = Nothing
    End Function

    Private Async Function LoadVehicles() As Task
        _vehicles = Await _vehicleRepository.GetAll()

        View.cboVehicles.DataSource = _vehicles
        View.cboVehicles.SelectedIndex = -1
    End Function

    Private Async Function LoadRoutes() As Task
        _routes = Await _routeRepository.GetAll()

        View.cboRoutes.DataSource = _routes
        View.cboRoutes.SelectedIndex = -1
    End Function

    Private Async Function LoadRoutePayRates() As Task
        Dim routeRates = Await _routeRateRepository.GetAllAsync()

        _routeRates = routeRates
    End Function

    Private Async Function LoadMinimumWageRate() As Task
        Dim listOfValue = (Await _listOfValueRepository.
            GetFilteredListOfValuesAsync(Function(t) t.Type = "Minimum Wage Rate")).
            FirstOrDefault()

        Me.standardMinimumWage = CDec(listOfValue.DisplayValue)
    End Function

    Private Sub NewTripTicket() Handles View.TripTicketCreated
        _tripTicket = New TripTicket()
        _tripTicketHelpers = New List(Of TripTicketEmployee)
        View.ClearDisplay()
        View.DisplayTripTicket(_tripTicket)
        View.TripTicketHelpersSource.DataSource = _tripTicketHelpers
    End Sub

    Private Sub CancelChanges() Handles View.TripTicketCancelChanges
        'Me.context.Entry(Me._tripTicket).Reload()
    End Sub

    Private Async Sub SelectTripTicket(tripTicketID As Integer?) Handles View.TripTicketSelected
        Await LoadTripTicket(tripTicketID)
        Await LoadTripTicketHelpers(tripTicketID)

        View.ClearDisplay()
        View.DisplayTripTicket(_tripTicket)
        View.TripTicketHelpersSource.DataSource = _tripTicketHelpers
    End Sub

    Private Sub AddTripTicketHelper(employeeID As Integer?) Handles View.EmployeeWasAdded
        Dim employee = _employees.Single(Function(e) CBool(e.RowID = employeeID))

        Dim tripTicketHelper = New TripTicketEmployee()
        tripTicketHelper.OrganizationID = z_OrganizationID
        tripTicketHelper.CreatedBy = z_User
        tripTicketHelper.Employee = employee
        tripTicketHelper.EmployeeID = employee.RowID

        If _tripTicket.IsSpecialOperations Then
            'tripTicketHelper.PaymentAmount = Me.standardMinimumWage
        Else
            ApplyPaymentFromPayRateMatrix(tripTicketHelper)
        End If

        View.TripTicketHelpersSource.Add(tripTicketHelper)
    End Sub

    Private Sub RemoveTripTicketHelper(employeeID As Integer?) Handles View.EmployeeWasRemoved
        Dim tripTicketEmployee = _tripTicketHelpers.FirstOrDefault(Function(e) CBool(e.EmployeeID = employeeID))

        View.TripTicketHelpersSource.Remove(tripTicketEmployee)
    End Sub

    ''' <summary>
    ''' Change payment policy according to whether special operations has been toggled.
    ''' </summary>
    Private Sub SwitchSpecialOperations(isSpecialOperations As Boolean) Handles View.SpecialOperationsToggled
        ' If IsSpecialOperations switch didn't actually change, don't do anything.
        If _tripTicket.IsSpecialOperations = isSpecialOperations Then
            Return
        End If

        _tripTicket.IsSpecialOperations = isSpecialOperations

        If _tripTicket.IsSpecialOperations Then
            GiveMinimumWagePayments()
        Else
            GivePayRateMatrixPayments()
        End If

        View.dgvTripTicketHelpers.Refresh()
    End Sub

    ''' <summary>
    ''' Set all helpers payment to the minimum wage rate.
    ''' </summary>
    Private Sub GiveMinimumWagePayments()
        For Each tripTicketHelper In _tripTicketHelpers
            'tripTicketHelper.PaymentAmount = Me.standardMinimumWage
        Next
    End Sub

    ''' <summary>
    ''' Give all helpers payment according to the pay rate matrix.
    ''' </summary>
    Private Sub GivePayRateMatrixPayments()
        For Each tripTicketHelper In _tripTicketHelpers
            ApplyPaymentFromPayRateMatrix(tripTicketHelper)
        Next
    End Sub

    Private Sub ApplyPaymentFromPayRateMatrix(tripTicketHelper As TripTicketEmployee)
        ' Select pay rate according to employee's position and current route.
        Dim routePayRate = (From r In _routeRates
                            Select r
                            Where r.PositionID = tripTicketHelper.Employee.PositionID And
                                r.RouteID = Me._tripTicket.RouteID
                            ).FirstOrDefault()

        ' If the routepayrate doesn't exist, don't pay anything.
        ' Otherwise, pay according to the amount set by the pay rate.
        If routePayRate Is Nothing Then
            'tripTicketHelper.PaymentAmount = 0
        Else
            'tripTicketHelper.PaymentAmount = routePayRate.Rate
        End If
    End Sub

    Private Sub UpdateRoute(routeID As Integer?) Handles View.RouteHasChanged
        If _tripTicket Is Nothing Then
            Return
        End If

        _tripTicket.RouteID = routeID
        GivePayRateMatrixPayments()

        View.dgvTripTicketHelpers.Refresh()
    End Sub

    Private Sub SaveTripTicket() Handles View.TripTicketSaved
        ApplyChanges()
        Persist()
    End Sub

    Private Sub ApplyChanges()
        'Me.tripTicket.TicketNo = view.TicketNo
        'Me.tripTicket.TimeFrom = view.TimeFrom
        'Me.tripTicket.TimeTo = view.TimeTo
        'Me.tripTicket.TimeDispatched = view.TimeDispatched
        _tripTicket.Date = View.TripDate
        _tripTicket.VehicleID = View.VehicleID
        _tripTicket.RouteID = View.RouteID
    End Sub

    Private Sub Persist()
        _tripTicket.Employees = _tripTicketHelpers
        _tripTicketRepository.Update(_tripTicket)
    End Sub

End Class