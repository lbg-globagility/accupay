Imports AccuPay.Data.Entities
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services.CostCenterReportDataService

Public Class CostCenterReportGenerationResult
    Implements ProgressGenerator.IResult

    Private ReadOnly status As ResultStatus
    Public ReadOnly Property Branch As Branch
    Public ReadOnly Property Model As List(Of PayPeriodModel)

    Private Sub New(branch As Branch, model As List(Of PayPeriodModel), status As ResultStatus)
        Me.Branch = branch
        Me.Model = model
        Me.status = status
    End Sub

    Public ReadOnly Property IsSuccess As Boolean Implements ProgressGenerator.IResult.IsSuccess
        Get
            Return status = ResultStatus.Success
        End Get
    End Property

    Public ReadOnly Property IsError As Boolean Implements ProgressGenerator.IResult.IsError
        Get
            Return status = ResultStatus.Error
        End Get
    End Property

    Public Shared Function Success(branch As Branch, model As List(Of PayPeriodModel)) As CostCenterReportGenerationResult

        Return New CostCenterReportGenerationResult(branch, model, ResultStatus.Success)

    End Function

    Public Shared Function [Error]() As CostCenterReportGenerationResult

        Return New CostCenterReportGenerationResult(Nothing, Nothing, ResultStatus.Error)

    End Function

End Class
