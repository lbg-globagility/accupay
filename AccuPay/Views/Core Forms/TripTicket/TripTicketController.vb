Option Explicit On
Option Strict On

Imports Payroll.Routes
Imports System.Data.Entity
Imports System.Transactions
Imports System.ComponentModel
Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports Microsoft.EntityFrameworkCore

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

    Private WithEvents view As TripTicketForm
    Private context As PayrollContext
    Private tripTicket As TripTicket
    Private tripTickets As IList(Of TripTicket)
    Private vehicles As IList(Of Vehicle)
    Private routes As IList(Of Route)
    Private employees As IList(Of Employee)
    Private tripTicketHelpers As IList(Of TripTicketEmployee)
    Private standardMinimumWage As Decimal
    Private routePayRates As IList(Of RoutePayRate)

    Public Property TicketNo As String
        Get
            Return tripTicket?.TicketNo
        End Get
        Set(value As String)
            tripTicket.TicketNo = value
            NotifyPropertyChanged("TicketNo")
        End Set
    End Property

    Public Property TripDate As Date
        Get
            Return If(tripTicket Is Nothing, DateTime.Today(), tripTicket.TripDate.Value)
        End Get
        Set(value As Date)
            tripTicket.TripDate = value
            NotifyPropertyChanged("TripDate")
        End Set
    End Property

    Public Property TimeFrom As String
        Get
            Return tripTicket?.TimeFrom.ToString()
        End Get
        Set(value As String)
            tripTicket.TimeFrom = TimeSpan.Parse(value)
            NotifyPropertyChanged("TimeFrom")
        End Set
    End Property

    Public Property TimeTo As String
        Get
            Return tripTicket?.TimeTo.ToString()
        End Get
        Set(value As String)
            tripTicket.TimeTo = TimeSpan.Parse(value)
            NotifyPropertyChanged("TimeTo")
        End Set
    End Property

    Public Property TimeDispatched As String
        Get
            Return If(tripTicket Is Nothing, Nothing, tripTicket.TimeDispatched.ToString())
        End Get
        Set(value As String)
            tripTicket.TimeDispatched = TimeSpan.Parse(value)
            NotifyPropertyChanged("TimeDispatched")
        End Set
    End Property

    Public Property Route As Route
        Get
            Return tripTicket?.Route
        End Get
        Set(value As Route)
            tripTicket.Route = value
            NotifyPropertyChanged("Route")
            NotifyPropertyChanged("Distance")
        End Set
    End Property

    Public ReadOnly Property Distance As String
        Get
            If tripTicket Is Nothing Then : Return Nothing : End If
            If tripTicket.Route Is Nothing Then : Return Nothing : End If

            Return CStr(tripTicket.Route.Distance)
        End Get
    End Property

    Public Property Vehicle As Vehicle
        Get
            Return tripTicket?.Vehicle
        End Get
        Set(value As Vehicle)
            tripTicket.Vehicle = value
            NotifyPropertyChanged("Vehicle")
            NotifyPropertyChanged("PlateNo")
        End Set
    End Property

    Public ReadOnly Property PlateNo As String
        Get
            If tripTicket Is Nothing Then : Return Nothing : End If
            If tripTicket.Vehicle Is Nothing Then : Return Nothing : End If

            Return tripTicket.Vehicle.PlateNo
        End Get
    End Property

    Public ReadOnly Property TruckType As String
        Get
            If tripTicket Is Nothing Then : Return Nothing : End If
            If tripTicket.Vehicle Is Nothing Then : Return Nothing : End If

            Return tripTicket.Vehicle.TruckType
        End Get
    End Property

    Public Sub New(view As TripTicketForm)
        Me.view = view
        Dim builder As DbContextOptionsBuilder = New DbContextOptionsBuilder()
        builder.UseMySql(mysql_conn_text)

        Me.context = New PayrollContext(builder.Options)

        Start()
    End Sub

    Public Sub Start()
        LoadTripTickets()
        LoadVehicles()
        LoadRoutes()
        LoadEmployees()
        LoadMinimumWageRate()
        LoadRoutePayRates()
    End Sub

    Private Sub LoadTripTicket(tripTicketID As Integer?)
        Dim query = From t In Me.context.TripTickets
                    Select t
                    Where t.RowID = tripTicketID

        Me.tripTicket = query.Single()
    End Sub

    Private Sub LoadTripTicketHelpers(tripTicketID As Integer?)
        Dim query = From t In Me.context.TripTicketEmployees.Include(
                        Function(t) t.Employee
                    )
                    Select t
                    Where t.TripTicketID = tripTicketID

        Me.tripTicketHelpers = query.ToList()
    End Sub

    Private Sub LoadTripTickets()
        Dim query = From t In Me.context.TripTickets
                    Select t

        Me.tripTickets = query.ToList()
        view.TripTicketsSource.DataSource = tripTickets
    End Sub

    Private Sub LoadEmployees()
        Dim query = From e In Me.context.Employees
                    Select e
                    Where e.OrganizationID = z_OrganizationID
                    Order By e.LastName, e.FirstName

        Me.employees = query.ToList()
        view.cboEmployees.DataSource = Me.employees
        view.cboEmployees.SelectedItem = Nothing
    End Sub

    Private Sub LoadVehicles()
        Dim query = From v In Me.context.Vehicles
                    Select v

        Me.vehicles = query.ToList()

        view.cboVehicles.DataSource = Me.vehicles
        view.cboVehicles.SelectedIndex = -1
    End Sub

    Private Sub LoadRoutes()
        Dim query = From r In Me.context.Routes
                    Select r

        Me.routes = query.ToList()

        view.cboRoutes.DataSource = Me.routes
        view.cboRoutes.SelectedIndex = -1
    End Sub

    Private Sub LoadRoutePayRates()
        Dim query = From p In Me.context.RoutePayRates
                    Select p

        Me.routePayRates = query.ToList()
    End Sub

    Private Sub LoadMinimumWageRate()
        Dim query = From l In Me.context.ListOfValues
                    Select l
                    Where l.Type = "Minimum Wage Rate"

        Dim listOfValue = query.Single()
        Me.standardMinimumWage = CDec(listOfValue.DisplayValue)
    End Sub

    Private Sub NewTripTicket() Handles view.TripTicketCreated
        Me.tripTicket = New TripTicket()
        Me.tripTicketHelpers = New List(Of TripTicketEmployee)
        view.ClearDisplay()
        view.DisplayTripTicket(Me.tripTicket)
        view.TripTicketHelpersSource.DataSource = Me.tripTicketHelpers
    End Sub

    Private Sub CancelChanges() Handles view.TripTicketCancelChanges
        Me.context.Entry(Me.tripTicket).Reload()
    End Sub

    Private Sub SelectTripTicket(tripTicketID As Integer?) Handles view.TripTicketSelected
        LoadTripTicket(tripTicketID)
        LoadTripTicketHelpers(tripTicketID)

        view.ClearDisplay()
        view.DisplayTripTicket(Me.tripTicket)
        view.TripTicketHelpersSource.DataSource = Me.tripTicketHelpers
    End Sub

    Private Sub AddTripTicketHelper(employeeID As Integer?) Handles view.EmployeeWasAdded
        Dim employee = Me.employees.Single(Function(e) CBool(e.RowID = employeeID))

        Dim tripTicketHelper = New TripTicketEmployee()
        tripTicketHelper.Employee = employee

        If Me.tripTicket.IsSpecialOperations Then
            'tripTicketHelper.PaymentAmount = Me.standardMinimumWage
        Else
            ApplyPaymentFromPayRateMatrix(tripTicketHelper)
        End If

        view.TripTicketHelpersSource.Add(tripTicketHelper)
    End Sub

    ''' <summary>
    ''' Change payment policy according to whether special operations has been toggled.
    ''' </summary>
    Private Sub SwitchSpecialOperations(isSpecialOperations As Boolean) Handles view.SpecialOperationsToggled
        ' If IsSpecialOperations switch didn't actually change, don't do anything.
        If Me.tripTicket.IsSpecialOperations = isSpecialOperations Then
            Return
        End If

        Me.tripTicket.IsSpecialOperations = isSpecialOperations

        If Me.tripTicket.IsSpecialOperations Then
            GiveMinimumWagePayments()
        Else
            GivePayRateMatrixPayments()
        End If

        view.dgvTripTicketHelpers.Refresh()
    End Sub

    ''' <summary>
    ''' Set all helpers payment to the minimum wage rate.
    ''' </summary>
    Private Sub GiveMinimumWagePayments()
        For Each tripTicketHelper In Me.tripTicketHelpers
            'tripTicketHelper.PaymentAmount = Me.standardMinimumWage
        Next
    End Sub

    ''' <summary>
    ''' Give all helpers payment according to the pay rate matrix.
    ''' </summary>
    Private Sub GivePayRateMatrixPayments()
        For Each tripTicketHelper In Me.tripTicketHelpers
            ApplyPaymentFromPayRateMatrix(tripTicketHelper)
        Next
    End Sub

    Private Sub ApplyPaymentFromPayRateMatrix(tripTicketHelper As TripTicketEmployee)
        ' Select pay rate according to employee's position and current route.
        Dim routePayRate = (From r In Me.routePayRates
                            Select r
                            Where r.PositionID = tripTicketHelper.Employee.PositionID And
                                r.RouteID = Me.tripTicket.RouteID
                            ).FirstOrDefault()

        ' If the routepayrate doesn't exist, don't pay anything.
        ' Otherwise, pay according to the amount set by the pay rate.
        If routePayRate Is Nothing Then
            'tripTicketHelper.PaymentAmount = 0
        Else
            'tripTicketHelper.PaymentAmount = routePayRate.Rate
        End If
    End Sub

    Private Sub UpdateRoute(routeID As Integer?) Handles view.RouteHasChanged
        If Me.tripTicket Is Nothing Then
            Return
        End If

        Me.tripTicket.RouteID = routeID
        GivePayRateMatrixPayments()

        view.dgvTripTicketHelpers.Refresh()
    End Sub

    Private Sub SaveTripTicket() Handles view.TripTicketSaved
        ApplyChanges()
        Persist()
    End Sub

    Private Sub ApplyChanges()
        'Me.tripTicket.TicketNo = view.TicketNo
        'Me.tripTicket.TimeFrom = view.TimeFrom
        'Me.tripTicket.TimeTo = view.TimeTo
        'Me.tripTicket.TimeDispatched = view.TimeDispatched
        Me.tripTicket.TripDate = view.TripDate
        Me.tripTicket.VehicleID = view.VehicleID
        Me.tripTicket.RouteID = view.RouteID
        Me.tripTicket.LastUpd = DateTime.Now()
    End Sub

    Private Sub Persist()
        Me.context.TripTickets.Add(Me.tripTicket)

        For Each tripTicketHelper In tripTicketHelpers
            tripTicketHelper.TripTicket = Me.tripTicket
            context.TripTicketEmployees.Add(tripTicketHelper)
        Next

        Me.context.SaveChanges()
    End Sub

End Class
