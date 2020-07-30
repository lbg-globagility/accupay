Option Explicit On
Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports Microsoft.EntityFrameworkCore

Public Class RoutePayRateMatrixForm
    Private routePayRateMatrix As DataTable
    Private context As PayrollContext

    Private routes As IList(Of Route)
    Private routePayRates As IList(Of RoutePayRate)
    Private positions As IList(Of Position)

    Private routePayRatesToDelete As IList(Of RoutePayRate)

    Public Sub Startup()
        Dim builder As DbContextOptionsBuilder = New DbContextOptionsBuilder()
        builder.UseMySql(mysql_conn_text)

        Me.context = New PayrollContext(builder.Options)

        LoadRoutePayRates()
        LoadRoutes()
        LoadPositions()

        routePayRatesToDelete = New List(Of RoutePayRate)

        InitializeRoutePayRateHeaders()
        FillInPositions()
        FillInExistingRoutePayRates()
    End Sub

    ''' <summary>
    ''' Load all existing company positions
    ''' </summary>
    Private Sub LoadPositions()
        Dim query = From p In Me.context.Positions
                    Select p
                    Where p.OrganizationID = z_OrganizationID

        Me.positions = query.ToList()
    End Sub

    ''' <summary>
    '''
    ''' </summary>
    Private Sub LoadRoutes()
        Dim query = From r In Me.context.Routes
                    Select r

        Me.routes = query.ToList()
    End Sub

    Private Sub LoadRoutePayRates()
        Dim query = From r In Me.context.RoutePayRates
                    Select r
                    Where r.OrganizationID = z_OrganizationID

        Me.routePayRates = query.ToList()
    End Sub

    Private Sub InitializeRoutePayRateHeaders()
        routePayRateMatrix = New DataTable()
        routePayRateMatrix.Columns.Add("PositionID").ColumnMapping = MappingType.Hidden
        routePayRateMatrix.Columns.Add("Position")

        For Each route In Me.routes
            Dim routeColumn = New DataColumn(route.Description, System.Type.GetType("System.Decimal"))
            routeColumn.ExtendedProperties.Add("RouteID", route.RowID)

            routePayRateMatrix.Columns.Add(routeColumn)
        Next

        dgvRoutePayRateMatrix.DataSource = routePayRateMatrix
    End Sub

    Private Sub FillInPositions()
        For Each position In Me.positions
            Dim row = routePayRateMatrix.NewRow()
            row(0) = position.RowID
            row(1) = position.Name

            routePayRateMatrix.Rows.Add(row)
        Next
    End Sub

    Private Sub FillInExistingRoutePayRates()
        For Each routePayRate In Me.routePayRates
            Dim row = FindRow(routePayRate.Position.Name)

            row(routePayRate.Route.Description) = routePayRate.Rate
        Next
    End Sub

    Private Function FindRow(positionName As String) As DataRow
        For Each row As DataRow In Me.routePayRateMatrix.Rows
            Dim rowPositionname = row("Position").ToString()
            If rowPositionname = positionName Then
                Return row
            End If
        Next

        Return Nothing
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        dgvRoutePayRateMatrix.EndEdit()

        For Each positionRow As DataRow In routePayRateMatrix.Rows
            Dim position = positionRow("Position").ToString()
            Dim positionID = ConvertToType(Of Integer?)(positionRow("PositionID"))

            For Each column As DataColumn In routePayRateMatrix.Columns
                If column.ToString = "Position" Or column.ToString = "PositionID" Then
                    Continue For
                End If

                Dim routeName = column.ToString()
                Dim routeID = ConvertToType(Of Integer?)(column.ExtendedProperties("RouteID"))
                Dim rate = ConvertToType(Of Decimal)(positionRow(routeName))

                Dim routePayRate = FindRoutePayRate(routeID, positionID)
                Dim isPayRateMissing = routePayRate Is Nothing
                Dim rateIsNonZero = rate > 0D

                If isPayRateMissing And rateIsNonZero Then
                    routePayRate = New RoutePayRate() With {
                        .OrganizationID = z_OrganizationID,
                        .RouteID = routeID,
                        .PositionID = positionID,
                        .Rate = rate
                    }

                    Me.routePayRates.Add(routePayRate)
                End If
            Next
        Next

        PersistToDatabase()
    End Sub

    Private Sub PersistToDatabase()
        For Each routePayRate In Me.routePayRates
            If routePayRate.RowID Is Nothing Then
                Me.context.RoutePayRates.Add(routePayRate)
            End If
        Next

        Me.context.SaveChanges()
    End Sub

    Private Function FindRoutePayRate(routeID As Integer?, positionID As Integer?) As RoutePayRate
        Return (From r In Me.routePayRates
                Select r
                Where r.RouteID = routeID And r.PositionID = positionID).FirstOrDefault()
    End Function

    Private Sub RoutePayRateMatrixForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Startup()
    End Sub

End Class