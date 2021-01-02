Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Public Class VehicleForm

    Public Property VehiclesSource As BindingSource = New BindingSource()

    Private vehicles As IList(Of Vehicle)

    Private ReadOnly _vehicleRepository As IVehicleRepository

    Sub New()

        InitializeComponent()

        _vehicleRepository = MainServiceProvider.GetRequiredService(Of IVehicleRepository)
    End Sub

    Private Sub Startup()

        dgvVehicles.AutoGenerateColumns = False
        dgvVehicles.DataSource = VehiclesSource
        LoadVehicles()
    End Sub

    Private Sub LoadVehicles()

        Me.vehicles = _vehicleRepository.GetAll().ToList()
        VehiclesSource.DataSource = Me.vehicles
    End Sub

    Private Sub VehicleForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Startup()
    End Sub

    Private Sub RemoveVehicle(rowIndex As Integer)
        Dim vehicle = dgvVehicles.Rows(rowIndex).DataBoundItem

        Me.VehiclesSource.Remove(vehicle)
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        dgvVehicles.EndEdit()

        Dim newVehicles = Me.vehicles.Where(Function(vehicle) vehicle.RowID Is Nothing)

        For Each newVehicle In newVehicles
            newVehicle.OrganizationID = z_OrganizationID
            newVehicle.Created = DateTime.Now()
            newVehicle.CreatedBy = z_User
        Next

        For Each vehicle In Me.vehicles
            vehicle.LastUpd = DateTime.Now()
            vehicle.LastUpdBy = z_User
        Next

        Dim vehicleRepository = MainServiceProvider.GetRequiredService(Of IVehicleRepository)
        Await vehicleRepository.CreateMany(newVehicles)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        LoadVehicles()
    End Sub

    Private Sub dgvVehicles_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvVehicles.CellClick
        Dim vehiclesGrid = DirectCast(sender, DataGridView)
        Dim column = vehiclesGrid.Columns(e.ColumnIndex)

        If TypeOf column Is DataGridViewButtonColumn AndAlso Object.ReferenceEquals(column, colVehiclesRemove) Then
            RemoveVehicle(e.RowIndex)
        End If
    End Sub

End Class
