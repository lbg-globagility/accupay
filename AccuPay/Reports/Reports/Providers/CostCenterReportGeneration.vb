Option Strict On

Imports System.Collections.Concurrent
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Core.Services.CostCenterReportDataService

Public Class CostCenterReportGeneration
    Inherits ProgressGenerator

    Private Branches As IEnumerable(Of Branch)
    Private ReadOnly IsActual As Boolean

    Private _results As BlockingCollection(Of CostCenterReportGenerationResult)

    Public Sub New(branches As IEnumerable(Of Branch), isActual As Boolean, Optional additionalProgressCount As Integer = 0)

        MyBase.New(branches.Where(Function(b) b IsNot Nothing).Count() + additionalProgressCount)

        Me.Branches = branches.
            Where(Function(b) b IsNot Nothing).
            OrderBy(Function(b) b.Name)

        Me.IsActual = isActual

        _results = New BlockingCollection(Of CostCenterReportGenerationResult)()
    End Sub

    Public Sub Start(resources As ICostCenterReportResources)

        If resources Is Nothing Then Return

        For Each branch In Branches

            Dim payPeriodModels = GetCostCenterPayPeriodModels(branch, resources)

            Dim result = CostCenterReportGenerationResult.Success(branch, payPeriodModels)

            SetCurrentMessage($"Finished generating cost center report data for {branch.Name}.")

            _results.Add(result)

            IncreaseProgress()
        Next

        SetResults(_results.ToList())

    End Sub

    Private Function GetCostCenterPayPeriodModels(branch As Branch, resources As ICostCenterReportResources) As List(Of PayPeriodModel)

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
