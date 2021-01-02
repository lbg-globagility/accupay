Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Core.Services
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Attributes
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportTripTicketForm

#Region "VariableDeclarations"

    Private _filePath As String
    Private _okModels As List(Of TripTicketModel)
    Private _failModels As List(Of TripTicketModel)

    Private ReadOnly _employeeRepository As IEmployeeRepository
    Private ReadOnly _tripTicketRepository As ITripTicketRepository
    Private ReadOnly _routeRepository As IRouteRepository
    Private ReadOnly _vehicleRepository As IVehicleRepository

#End Region

    Sub New()

        InitializeComponent()

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _tripTicketRepository = MainServiceProvider.GetRequiredService(Of ITripTicketRepository)

        _routeRepository = MainServiceProvider.GetRequiredService(Of IRouteRepository)

        _vehicleRepository = MainServiceProvider.GetRequiredService(Of IVehicleRepository)

        DataGridView1.AutoGenerateColumns = False
        DataGridView2.AutoGenerateColumns = False

    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim browseFile = New OpenFileDialog With {
            .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls",
            .RestoreDirectory = True
        }

        If Not browseFile.ShowDialog() = DialogResult.OK Then Return

        Await FunctionUtils.TryCatchFunctionAsync("Load Trip Ticket Data",
            Async Function()
                Await SetFileDirectory(browseFile.FileName)
            End Function)
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.TripTicket)
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

#Region "Properties"

    Private Async Function SetFileDirectory(value As String) As Task
        _filePath = value

        Await FilePathChanged()
    End Function

#End Region

#Region "Functions"

    Public Async Function SaveAsync() As Task
        If Not _okModels.Any() Then
            Return
        End If

        Dim groupings = (From p In _okModels
                         Group By pp = New With {Key p.TripDate, Key p.TripLocation, Key p.Truck} Into ppp = Group
                         Select New TripTicketGrouping() With
                            {
                            .TripDate = pp.TripDate,
                            .TripLocation = pp.TripLocation,
                            .RouteID = ppp.FirstOrDefault.RouteID,
                            .Truck = pp.Truck,
                            .VehicleID = ppp.FirstOrDefault.VehicleID,
                            .RouteNotYetExists = ppp.FirstOrDefault.RouteNotYetExists,
                            .VehicleNotYetExists = ppp.FirstOrDefault.VehicleNotYetExists,
                            .Items = ppp.Select(Function(t) New TripTickeGroupItem(t)).ToList()
                            }).ToList()

        Dim tripTickets = New List(Of TripTicket)

        For Each model In groupings

            Dim newtripTicket = TripTicket.NewTripTicket(
                organizationId:=z_OrganizationID,
                userId:=z_User
            )

            AssignChanges(model, newtripTicket)

            tripTickets.Add(newtripTicket)
        Next

        Await SaveToDatabase(tripTickets)
    End Function

    Private Sub AssignChanges(model As TripTicketGrouping, newtripTicket As TripTicket)
        With model
            If .RouteNotYetExists Then
                Dim newRoute = Route.NewRoute(organizationId:=z_OrganizationID, userId:=z_User)
                newRoute.Description = model.TripLocation

                newtripTicket.Route = newRoute
            End If

            If .VehicleNotYetExists Then
                Dim newVehicle = Vehicle.NewVehicle(organizationId:=z_OrganizationID, userId:=z_User)
                newVehicle.PlateNo = model.Truck

                newtripTicket.Vehicle = newVehicle
            End If

            newtripTicket.Date = .TripDate

            newtripTicket.Employees = .Items.Select(Function(t) NewTripTicketEmployee(t)).ToList()

        End With
    End Sub

    Private Function NewTripTicketEmployee(t As TripTickeGroupItem) As TripTicketEmployee
        Dim tte = TripTicketEmployee.NewTripTicketEmployee(z_OrganizationID, z_User)
        tte.EmployeeID = t.EmployeeID
        tte.NoOfTrips = t.NoOfTrips

        Return tte
    End Function

    Private Async Function SaveToDatabase(tripTickets As List(Of TripTicket)) As Task
        If tripTickets.Any Then

            Dim service = MainServiceProvider.GetRequiredService(Of ITripTicketDataService)

            Dim routes = (From r In tripTickets
                          Group By rr = New With {Key r.RouteDescription} Into rrr = Group
                          Select rrr.FirstOrDefault.Route).
                          ToList()

            Dim vehicles = (From v In tripTickets
                            Group By vv = New With {Key v.VehiclePlateNo} Into vvv = Group
                            Select vvv.FirstOrDefault.Vehicle).
                            ToList()

            Await service.ImportAsync(
                tripTickets,
                routes,
                vehicles,
                organizationId:=z_OrganizationID,
                currentlyLoggedInUserId:=z_User)

        End If
    End Function

    Private Async Function FilePathChanged() As Task

        Dim parsedModels As New List(Of TripTicketModel)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
            Sub()
                parsedModels = ExcelService(Of TripTicketModel).
                            Read(_filePath, "Default").
                            ToList()

            End Sub)

        If parsedSuccessfully = False Then Return

        If parsedModels Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot read the template.")
            Return
        End If

        Dim employees = Await _employeeRepository.GetAllAsync(z_OrganizationID)

        Dim routes = Await _routeRepository.GetAllAsync(z_OrganizationID)

        Dim vehicles = Await _vehicleRepository.GetAllAsync(z_OrganizationID)

        Dim isEqualToLower =
            Function(dataText As String, parsedText As String)
                If String.IsNullOrWhiteSpace(parsedText) Then Return False
                Return StrConv(dataText, VbStrConv.Lowercase) = StrConv(parsedText, VbStrConv.Lowercase)
            End Function

        Dim employeeNos = (From p In parsedModels.Where(Function(e) Not String.IsNullOrWhiteSpace(e.EmployeeNo))
                           Group By pp = New With {Key p.EmployeeNo} Into ppp = Group
                           Select ppp.FirstOrDefault.EmployeeNo).
                           ToList()
        Dim employeeIDs = employees.Where(Function(e) employeeNos.Any(Function(eNo) isEqualToLower(e.EmployeeNo, eNo))).Select(Function(e) e.RowID.Value).ToList()

        Dim routeDesciptions = (From p In parsedModels.Where(Function(r) Not String.IsNullOrWhiteSpace(r.TripLocation))
                                Group By pp = New With {Key p.TripLocation} Into ppp = Group
                                Select ppp.FirstOrDefault.TripLocation).
                                ToList()

        Dim vehicleDescriptions = (From p In parsedModels.Where(Function(v) Not String.IsNullOrWhiteSpace(v.Truck))
                                   Group By pp = New With {Key p.Truck} Into ppp = Group
                                   Select ppp.FirstOrDefault.Truck).
                                   ToList()

        Dim tripDates = New TimePeriod(Now, Now)
        If parsedModels.Where(Function(p) p.TripDate.HasValue).Any Then
            Dim dates = parsedModels.Where(Function(p) p.TripDate.HasValue).Select(Function(p) p.TripDate.Value).ToList()
            Dim minDate = dates.Min()
            Dim maxDate = dates.Max()
            tripDates = New TimePeriod(minDate, maxDate)
        End If

        Dim organizationId = z_OrganizationID
        Dim tripiTickets = (Await _tripTicketRepository.GetByEmployeeIDsByRouteByVehicleBetweenDatesAsync(organizationId, routeDesciptions, vehicleDescriptions, tripDates)).ToList()

        For Each model In parsedModels
            Dim employee = employees.Where(Function(e) isEqualToLower(e.EmployeeNo, model.EmployeeNo)).FirstOrDefault
            Dim route = routes.Where(Function(r) isEqualToLower(r.Description, model.TripLocation)).FirstOrDefault
            Dim vehicle = vehicles.Where(Function(v) isEqualToLower(v.PlateNo, model.Truck)).FirstOrDefault
            Dim tripTicket = tripiTickets.
                Where(Function(t) t.RouteDescription = model.TripLocation).
                Where(Function(t) t.VehiclePlateNo = model.Truck).
                Where(Function(t) Nullable.Equals(t.Date, model.TripDate)).
                FirstOrDefault()

            model.Validate(employee, route, vehicle, tripTicket)
        Next

        _okModels = parsedModels.Where(Function(ee) Not ee.Invalid).ToList()
        _failModels = parsedModels.Where(Function(ee) ee.Invalid).ToList()

        DataGridView1.DataSource = _okModels
        DataGridView2.DataSource = _failModels

        SaveButton.Enabled = _okModels.Count > 0

        TabPage1.Text = $"Ok ({Me._okModels.Count})"
        TabPage2.Text = $"Failed ({Me._failModels.Count})"

        UpdateStatusLabel(_failModels.Count)

    End Function

    Private Sub UpdateStatusLabel(errorCount As Integer)
        If errorCount > 0 Then

            If errorCount = 1 Then
                lblStatus.Text = "There is 1 error."
            Else
                lblStatus.Text = $"There are {errorCount} errors."

            End If

            lblStatus.Text += " Failed records will not be saved."
            lblStatus.BackColor = Color.Red
        Else
            lblStatus.Text = "No errors found."
            lblStatus.BackColor = Color.Green
        End If
    End Sub

#End Region

    Private Class TripTicketModel
        Implements IExcelRowRecord

        Private _noEmployee As Boolean
        Private _noRoute As Boolean
        Private _noVehicle As Boolean
        Private _noTripDate As Boolean
        Private _noTripLocation As Boolean
        Private _noTruck As Boolean
        Private _noNumberOfTrips As Boolean
        Private _tripTicketAlreadyExists As Boolean

        <Ignore>
        Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

        <ColumnName("Employee ID")>
        Public Property EmployeeNo As String

        <Ignore>
        Public Property EmployeeFullName As String

        <Ignore>
        Public Property EmployeeID As Integer?

        <ColumnName("Trip Date")>
        Public Property TripDate As Date?

        <ColumnName("Trip Location")>
        Public Property TripLocation As String

        <Ignore>
        Public Property RouteID As Integer?

        <ColumnName("Truck")>
        Public Property Truck As String

        <Ignore>
        Public Property VehicleID As Integer?

        <ColumnName("No. of Trips")>
        Public Property NumberOfTrips As Integer?

        Friend Sub Validate(employee As Employee, route As Route, vehicle As Vehicle, tripTicket As TripTicket)
            _noEmployee = employee Is Nothing
            _noRoute = route Is Nothing
            _noVehicle = vehicle Is Nothing

            If Not _noEmployee Then
                EmployeeID = employee.RowID.Value
                EmployeeFullName = employee.FullNameLastNameFirst
            End If

            If Not _noRoute Then RouteID = route.RowID.Value

            If Not _noVehicle Then VehicleID = vehicle.RowID.Value

            _noTripDate = Not TripDate.HasValue

            _noTripLocation = String.IsNullOrWhiteSpace(TripLocation)

            _noTruck = String.IsNullOrWhiteSpace(Truck)

            _noNumberOfTrips = Not NumberOfTrips.HasValue

            If tripTicket IsNot Nothing And Not _noEmployee Then
                If tripTicket.Employees IsNot Nothing Then
                    Dim query = tripTicket.
                        Employees.
                        Where(Function(tte) Nullable.Equals(tte.EmployeeID, EmployeeID)).
                        Where(Function(tte) Nullable.Equals(tte.NoOfTrips, NumberOfTrips)).
                        ToList()

                    _tripTicketAlreadyExists = query.Any()
                End If
            End If
        End Sub

        Friend Function Invalid() As Boolean
            Return _noEmployee Or _noTripDate Or _noTripLocation Or _noTruck Or _noNumberOfTrips Or _tripTicketAlreadyExists
        End Function

        Friend Function RouteNotYetExists() As Boolean
            Return _noRoute
        End Function

        Friend Function VehicleNotYetExists() As Boolean
            Return _noVehicle
        End Function

        ReadOnly Property Remarks As String
            Get
                Dim errors = New List(Of String)
                If _noEmployee Then errors.Add("Employee doesn't exists")
                If _noTripDate Then errors.Add("Invalid Trip Date")
                If _noTripLocation Then errors.Add("Invalid Trip Location")
                If _noTruck Then errors.Add("Invalid Truck")
                If _noNumberOfTrips Then errors.Add("Invalid No. of trips")
                If _tripTicketAlreadyExists Then errors.Add("Duplicate Trip Ticket for employee #" & EmployeeNo)

                Return String.Join("; ", errors.ToArray())
            End Get
        End Property

    End Class

    Private Class TripTicketGroupModel
        Public Property TripDate As Date?
        Public Property TripLocation As String
        Public Property Truck As String
    End Class

    Private Class TripTicketGrouping

        Public Sub New()
        End Sub

        Public Property TripDate As Date?
        Public Property TripLocation As String
        Public Property RouteID As Integer?
        Public Property Truck As String
        Public Property VehicleID As Integer?
        Public Property NumberOfTrips As Integer?
        Public Property Items As ICollection(Of TripTickeGroupItem)
        Public Property RouteNotYetExists As Boolean
        Public Property VehicleNotYetExists As Boolean
    End Class

    Private Class TripTickeGroupItem
        Private _t As TripTicketModel

        Public Sub New(t As TripTicketModel)
            _t = t

            EmployeeNo = _t.EmployeeNo
            EmployeeID = _t.EmployeeID
            NoOfTrips = _t.NumberOfTrips.Value
        End Sub

        Public Property EmployeeNo As String
        Public Property EmployeeID As Integer?
        Public Property NoOfTrips As Integer
    End Class

End Class
