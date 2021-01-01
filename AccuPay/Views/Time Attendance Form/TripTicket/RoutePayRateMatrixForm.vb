Option Explicit On
Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class RoutePayRateMatrixForm

    Private _routes As ICollection(Of Route)
    Private _routeRates As ICollection(Of RoutePayRate)
    Private _positions As ICollection(Of Position)

    Private ReadOnly _routeRepository As RouteRepository
    Private ReadOnly _routeRateRepository As RouteRateRepository
    Private ReadOnly _positionRepository As PositionRepository

    Private _selectedRoute As Route

    Public Sub New()
        InitializeComponent()

        _routeRepository = MainServiceProvider.GetRequiredService(Of RouteRepository)
        _routeRateRepository = MainServiceProvider.GetRequiredService(Of RouteRateRepository)
        _positionRepository = MainServiceProvider.GetRequiredService(Of PositionRepository)
    End Sub

    Private Sub RoutePayRateMatrixForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RouteDataGridView.AutoGenerateColumns = False
        RouteRatesDataGridView.AutoGenerateColumns = False

        Startup()
    End Sub

    Public Async Sub Startup()
        Await LoadRoutes()
        Await LoadPositions()

        RouteDataGridView.DataSource = _routes
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        RouteRatesDataGridView.EndEdit()

        Dim changedRouteRates = New Collection(Of RoutePayRate)

        For Each positionRow As DataGridViewRow In RouteRatesDataGridView.Rows
            Dim model = DirectCast(positionRow.DataBoundItem, RouteRateModel)

            Dim position = model.Position
            Dim routeRate = model.RouteRate

            Dim isRouteRateNotExist = routeRate Is Nothing
            Dim rateIsNotZero = model.Rate > 0D

            If isRouteRateNotExist And rateIsNotZero Then
                routeRate = New RoutePayRate() With {
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .RouteID = _selectedRoute.RowID,
                    .PositionID = position.RowID
                }
            End If

            If routeRate IsNot Nothing Then
                routeRate.Rate = model.Rate

                changedRouteRates.Add(routeRate)
            End If
        Next

        Await _routeRateRepository.SaveMany(changedRouteRates)
    End Sub

    Private Async Sub RouteDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles RouteDataGridView.SelectionChanged
        Dim route = DirectCast(RouteDataGridView.CurrentRow.DataBoundItem, Route)

        If route Is Nothing Then Return
        If route Is _selectedRoute Then Return

        _selectedRoute = route
        Await LoadRouteRates()

        Dim models = New Collection(Of RouteRateModel)

        For Each position In _positions
            Dim routeRate = _routeRates.FirstOrDefault(Function(t) t.PositionID.Value = position.RowID.Value)
            Dim model = New RouteRateModel(position, routeRate)

            models.Add(model)
        Next

        RouteRatesDataGridView.DataSource = models
    End Sub

    Private Async Function LoadPositions() As Task
        Me._positions = Await _positionRepository.GetAllAsync(z_OrganizationID)
    End Function

    Private Async Function LoadRoutes() As Task
        Me._routes = Await _routeRepository.GetAll()

    End Function

    Private Async Function LoadRouteRates() As Task
        If _selectedRoute Is Nothing Then Return

        Me._routeRates = Await _routeRateRepository.GetAll(_selectedRoute.RowID)
    End Function

    Private Sub NewButton_Click(sender As Object, e As EventArgs) Handles NewButton.Click

    End Sub

    Public Class RouteRateModel

        Public ReadOnly Property Position As Position

        Public ReadOnly Property RouteRate As RoutePayRate

        Public ReadOnly Property PositionName As String

        Public Property Rate As Decimal

        Public Sub New(position As Position, routeRate As RoutePayRate)
            Me.Position = position
            Me.RouteRate = routeRate
            PositionName = position.Name
            Rate = If(routeRate?.Rate, 0D)
        End Sub

    End Class

End Class