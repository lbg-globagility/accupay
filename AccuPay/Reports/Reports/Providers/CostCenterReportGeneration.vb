Imports System.Collections.Concurrent
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.CostCenterReportDataService
Imports Microsoft.Extensions.DependencyInjection

Public Class CostCenterReportGeneration
    Inherits ProgressGenerator

    Private SelectedMonth As Date?
    Private Branches As IEnumerable(Of Branch)
    Private ReadOnly IsActual As Boolean

    Private _results As BlockingCollection(Of CostCenterReportGenerationResult)

    Public Sub New(selectedMonth As Date?, branches As IEnumerable(Of Branch), isActual As Boolean)

        MyBase.New(branches.Count())

        Me.SelectedMonth = selectedMonth
        Me.Branches = branches
        Me.IsActual = isActual

        _results = New BlockingCollection(Of CostCenterReportGenerationResult)()
    End Sub

    Public Sub Start()

        'Parallel.ForEach(Branches,
        '    New ParallelOptions() With {.MaxDegreeOfParallelism = Environment.ProcessorCount},
        'Sub(branch)
        '    AddPayPeriodModels(branch)

        'End Sub)

        For Each branch In Branches

            Dim payPeriodModels = GetCostCenterPayPeriodModels(SelectedMonth, branch)

            Dim result = CostCenterReportGenerationResult.Success(branch, payPeriodModels)
            _results.Add(result)

            If result.IsError Then
                Console.WriteLine($"------------------------------------------------------------------------ ERROR - {branch.Name} ------------------------------------------------------------------------")
            End If

            Interlocked.Increment(_finished)
        Next

        SetResults(_results.ToList())

    End Sub

    Private Function GetCostCenterPayPeriodModels(selectedMonth As Date?, branch As Branch) As List(Of PayPeriodModel)

        If branch Is Nothing Then Return Nothing

        Dim dataService = MainServiceProvider.GetRequiredService(Of CostCenterReportDataService)
        Dim payPeriodModels = dataService.GetData(
            selectedMonth.Value,
            branch,
            userId:=z_User,
            isActual:=IsActual)
        Return payPeriodModels
    End Function

End Class
