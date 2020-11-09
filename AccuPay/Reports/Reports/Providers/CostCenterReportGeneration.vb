Imports System.Collections.Concurrent
Imports System.Threading
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.CostCenterReportDataService

Public Class CostCenterReportGeneration
    Inherits ProgressGenerator

    Private Branches As IEnumerable(Of Branch)
    Private ReadOnly IsActual As Boolean

    Private _results As BlockingCollection(Of CostCenterReportGenerationResult)

    Public Sub New(branches As IEnumerable(Of Branch), isActual As Boolean)

        MyBase.New(branches.Count())

        Me.Branches = branches
        Me.IsActual = isActual

        _results = New BlockingCollection(Of CostCenterReportGenerationResult)()
    End Sub

    Public Sub Start(resources As CostCenterReportResources)

        For Each branch In Branches

            Dim payPeriodModels = GetCostCenterPayPeriodModels(branch, resources)

            Dim result = CostCenterReportGenerationResult.Success(branch, payPeriodModels)
            _results.Add(result)

            Interlocked.Increment(_finished)
        Next

        SetResults(_results.ToList())

    End Sub

    Private Function GetCostCenterPayPeriodModels(branch As Branch, resources As CostCenterReportResources) As List(Of PayPeriodModel)

        If branch Is Nothing Then Return Nothing

        Dim dataService As New CostCenterReportDataService()
        Dim payPeriodModels = dataService.GetData(
            resources,
            branch,
            userId:=z_User,
            isActual:=IsActual)
        Return payPeriodModels
    End Function

End Class
