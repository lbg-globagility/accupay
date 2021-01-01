Option Strict On

Imports System.Collections.Concurrent
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Exceptions
Imports AccuPay.Core.Helpers.ProgressGenerator
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Repositories
Imports AccuPay.Core.Services
Imports AccuPay.Core.Services.CostCenterReportDataService
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Infrastructure.Reports
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

''' <summary>
''' This report will print all the employees from the selected branch and any employee
''' that has at least 1 time logs on the selected branch. This report only supports daily employees
''' as requested by the client and supporting monthly employees would require modification on the code.
''' </summary>
Public Class CostCenterReportProvider
    Inherits ExcelFormatReport
    Implements IReportProvider

    Public Enum ReportType
        All
        Branch
    End Enum

    Public Property Name As String = "Cost Center Report" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    'Passing the Owner Form is kind of hackish. Maybe replace this with
    'a better approach of disabling the parent form in the future.
    Public Property Owner As Form

    Public Property IsActual As Boolean
    Public Property SelectedReportType As ReportType

    Private ReadOnly _reportBuilder As ICostCenterReportBuilder

    Sub New()
        _reportBuilder = MainServiceProvider.GetRequiredService(Of ICostCenterReportBuilder)
    End Sub

    Public Sub Run() Implements IReportProvider.Run

        Try
            Dim selectedMonth = GetSelectedMonth()
            If selectedMonth Is Nothing Then Return

            Dim selectedBranch As Branch = Nothing

            If SelectedReportType = ReportType.Branch Then

                selectedBranch = GetSelectedBranch()
                If selectedBranch?.RowID Is Nothing Then Return

            End If

            Dim defaultFileName = GetDefaultFileName("Cost Center Report", selectedMonth.Value, selectedBranch)

            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

            If saveFileDialogHelperOutPut.IsSuccess = False Then
                Return
            End If

            Dim saveFilePath = saveFileDialogHelperOutPut.FileInfo.FullName

            GenerateReport(selectedMonth.Value, selectedBranch, saveFilePath)
        Catch ex As IOException

            MessageBoxHelper.ErrorMessage(ex.Message)
        Catch ex As Exception

            Debugger.Break()
            MessageBoxHelper.DefaultErrorMessage()

        End Try

    End Sub

    Private Sub GenerateReport(selectedMonth As Date, selectedBranch As Branch, saveFilePath As String)

        Dim allPayPeriodModels As New BlockingCollection(Of PayPeriodModel)

        If SelectedReportType = ReportType.Branch Then

            If selectedBranch Is Nothing Then

                MessageBoxHelper.ErrorMessage("Please select a valid branch.")

                Return
            End If

            GenerateSingleBranchReport(selectedMonth, selectedBranch, saveFilePath)
        Else
            GenerateMultipleBranchReport(selectedMonth, saveFilePath)

        End If
    End Sub

    Private Sub GenerateSingleBranchReport(selectedMonth As Date, selectedBranch As Branch, saveFilePath As String)
        GetResources(
            selectedMonth,
            Nothing,
            Sub(t)
                Dim dataService As New CostCenterReportDataService()
                Dim payPeriodModels = dataService.GetData(
                    t.Result,
                    selectedBranch,
                    userId:=z_User,
                    isActual:=IsActual)

                _reportBuilder.CreateReport(payPeriodModels.GroupBy(Function(p) p.Branch), saveFilePath, z_OrganizationID)

                Process.Start(saveFilePath)
            End Sub)
    End Sub

    Private Sub GenerateMultipleBranchReport(selectedMonth As Date, saveFilePath As String)
        Dim branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)
        Dim branches = branchRepository.GetAll()

        Dim generator As New CostCenterReportGeneration(branches, IsActual, additionalProgressCount:=1)
        Dim progressDialog = New ProgressDialog(generator, "Generating cost center report...")

        If Owner IsNot Nothing Then
            Owner.Enabled = False
        End If
        progressDialog.Show()

        generator.SetCurrentMessage("Loading resources...")
        GetResources(
            selectedMonth,
            progressDialog,
            Sub(resourcesTask)

                generator.IncreaseProgress("Finished loading resources.")

                Dim generationTask = Task.Run(
                    Sub()
                        generator.Start(resourcesTask.Result)
                    End Sub
                )

                RunGenerateExportTask(generationTask, saveFilePath, generator, progressDialog)
            End Sub)

    End Sub

    Private Sub RunGenerateExportTask(generationTask As Task, saveFilePath As String, generator As CostCenterReportGeneration, progressDialog As ProgressDialog)

        generationTask.ContinueWith(
            Sub() GenerationOnSuccess(generator.Results, progressDialog, saveFilePath),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        generationTask.ContinueWith(
            Sub(t As Task) GenerationOnError(t, progressDialog),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Sub GenerationOnSuccess(results As IReadOnlyCollection(Of IResult), progressDialog As ProgressDialog, saveFilePath As String)

        CloseProgressDialog(progressDialog)

        Dim saveResults = results.
            Select(Function(r) CType(r, CostCenterReportGenerationResult)).
            Where(Function(r) r.IsSuccess).
            SelectMany(Function(r) r.Model).
            GroupBy(Function(m) m.Branch).
            OrderBy(Function(b) b.Key.Name).
            ToList()

        _reportBuilder.CreateReport(saveResults, saveFilePath, z_OrganizationID)

        Process.Start(saveFilePath)
    End Sub

    Private Sub GenerationOnError(t As Task, progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        Const MessageTitle As String = "Generate Cost Center Report"

        If t.Exception?.InnerException.GetType() Is GetType(BusinessLogicException) Then

            MessageBoxHelper.ErrorMessage(t.Exception?.InnerException.Message, MessageTitle)
        Else
            Debugger.Break()
            MessageBoxHelper.ErrorMessage("Something went wrong while generating the cost center report. Please contact Globagility Inc. for assistance.", MessageTitle)
        End If

    End Sub

    Private Sub GetResources(selectedMonth As Date, progressDialog As ProgressDialog, callBackAfterLoadResources As Action(Of Task(Of CostCenterReportResources)))
        Dim resources = MainServiceProvider.GetRequiredService(Of CostCenterReportResources)

        Dim generationTask = Task.Run(
            Function()
                Dim resourcesTask = resources.Load(selectedMonth)
                resourcesTask.Wait()

                Return resources
            End Function)

        generationTask.ContinueWith(
            callBackAfterLoadResources,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        generationTask.ContinueWith(
            Sub(resourcesTask) LoadingResourcesOnError(resourcesTask, progressDialog),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Sub LoadingResourcesOnError(resourcesTask As Task(Of CostCenterReportResources), progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        MsgBox("Something went wrong while loading the cost center report resources. Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Resources")

    End Sub

    Private Sub CloseProgressDialog(progressDialog As ProgressDialog)

        If Owner IsNot Nothing Then
            Owner.Enabled = True
        End If

        If progressDialog IsNot Nothing Then

            progressDialog.Close()
            progressDialog.Dispose()

        End If
    End Sub

    Private Shared Function GetSelectedBranch() As Branch

        Dim selectBranchDialog As New SelectBranchForm

        If selectBranchDialog.ShowDialog <> DialogResult.OK Then
            Return Nothing
        End If

        Return selectBranchDialog.SelectedBranch

    End Function

    Private Function GetSelectedMonth() As Date?
        Dim selectMonthForm As New selectMonth

        If Not selectMonthForm.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return Nothing
        End If

        Return CDate(selectMonthForm.MonthValue).ToMinimumDateValue
    End Function

    Private Function GetDefaultFileName(
        reportName As String,
        selectedMonth As Date,
        Optional selectedBranch As Branch = Nothing) As String

        Return String.Concat(
            If(SelectedReportType = ReportType.Branch, $"{selectedBranch?.Name} ", "All - "),
            reportName, " ",
            "- ",
            selectedMonth.ToString("MMMM"),
            ".xlsx")
    End Function

End Class
