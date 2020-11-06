Option Strict On

Imports System.Collections.Concurrent
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Exceptions
Imports AccuPay.Data.Helpers.ProgressGenerator
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.CostCenterReportDataService
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Infrastructure.Reports
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

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

    Public Property IsActual As Boolean
    Public Property SelectedReportType As ReportType

    Private ReadOnly _reportColumns As IReadOnlyCollection(Of ExcelReportColumn) = GetReportColumns()

#Region "Column Keys"

    Private Const EmployeeIdKey As String = "EmployeeId"
    Private Const EmployeeNameKey As String = "EmployeeName"
    Private Const TotalDaysKey As String = "TotalDays"
    Private Const TotalHoursKey As String = "TotalHours"
    Private Const DailyRateKey As String = "DailyRate"
    Private Const HoulyRateKey As String = "HoulyRate"
    Private Const BasicPayKey As String = "BasicPay"
    Private Const OvertimeHoursKey As String = "OvertimeHours"
    Private Const OvertimePayKey As String = "OvertimePay"
    Private Const NightDiffHoursKey As String = "NightDiffHours"
    Private Const NightDiffPayKey As String = "NightDiffPay"
    Private Const NightDiffOvertimeHoursKey As String = "NightDiffOvertimeHours"
    Private Const NightDiffOvertimePayKey As String = "NightDiffOvertimePay"
    Private Const RestDayHoursKey As String = "RestDayHours"
    Private Const RestDayPayKey As String = "RestDayPay"
    Private Const RestDayOTHoursKey As String = "RestDayOTHours"
    Private Const RestDayOTPayKey As String = "RestDayOTPay"
    Private Const SpecialHolidayHoursKey As String = "SpecialHolidayHours"
    Private Const SpecialHolidayPayKey As String = "SpecialHolidayPay"
    Private Const SpecialHolidayOTHoursKey As String = "SpecialHolidayOTHours"
    Private Const SpecialHolidayOTPayKey As String = "SpecialHolidayOTPay"
    Private Const RegularHolidayHoursKey As String = "RegularHolidayHours"
    Private Const RegularHolidayPayKey As String = "RegularHolidayPay"
    Private Const RegularHolidayOTHoursKey As String = "RegularHolidayOTHours"
    Private Const RegularHolidayOTPayKey As String = "RegularHolidayOTPay"
    Private Const TotalAllowanceKey As String = "TotalAllowanceKey"
    Private Const GrossPayKey As String = "GrossPay"
    Private Const SSSAmountKey As String = "SSSAmount"
    Private Const ECAmountKey As String = "ECAmount"
    Private Const HDMFAmountKey As String = "HDMFAmount"
    Private Const PhilHealthAmountKey As String = "PhilHealthAmount"
    Private Const HMOAmountKey As String = "HMOAmount"
    Private Const ThirteenthMonthPayKey As String = "ThirteenthMonthPay"
    Private Const FiveDaySilpAmountKey As String = "FiveDaySilpAmount" '5 Day SILP (leave)
    Private Const NetPayKey As String = "NetPay"

#End Region

    Private Shared Function GetReportColumns() As ReadOnlyCollection(Of ExcelReportColumn)

        Dim reportColumns = New List(Of ExcelReportColumn)({
            New ExcelReportColumn("NAME OF EMPLOYEES", EmployeeNameKey, ExcelReportColumn.ColumnType.Text),
            New ExcelReportColumn("NO. OF DAYS", TotalDaysKey),
            New ExcelReportColumn("NO. OF HOURS", TotalHoursKey),
            New ExcelReportColumn("RATE", DailyRateKey),
            New ExcelReportColumn("HOURLY", HoulyRateKey),
            New ExcelReportColumn("GROSS PAY", BasicPayKey),
            New ExcelReportColumn("NO. OF OT HOURS", OvertimeHoursKey, [optional]:=True),
            New ExcelReportColumn("OT PAY", OvertimePayKey, [optional]:=True),
            New ExcelReportColumn("NO. OF ND HOURS", NightDiffHoursKey, [optional]:=True),
            New ExcelReportColumn("ND PAY", NightDiffPayKey, [optional]:=True),
            New ExcelReportColumn("NO. OF NDOT HOURS", NightDiffOvertimeHoursKey, [optional]:=True),
            New ExcelReportColumn("NDOT PAY", NightDiffOvertimePayKey, [optional]:=True),
            New ExcelReportColumn("REST DAY HOURS", RestDayHoursKey, [optional]:=True),
            New ExcelReportColumn("REST DAY PAY", RestDayPayKey, [optional]:=True),
            New ExcelReportColumn("REST DAY OT HOURS", RestDayOTHoursKey, [optional]:=True),
            New ExcelReportColumn("REST DAY OT PAY", RestDayOTPayKey, [optional]:=True),
            New ExcelReportColumn("SP HOLIDAY HOURS", SpecialHolidayHoursKey, [optional]:=True),
            New ExcelReportColumn("SP HOLIDAY PAY", SpecialHolidayPayKey, [optional]:=True),
            New ExcelReportColumn("SP HOLIDAY OT HOURS", SpecialHolidayOTHoursKey, [optional]:=True),
            New ExcelReportColumn("SP HOLIDAY OT PAY", SpecialHolidayOTPayKey, [optional]:=True),
            New ExcelReportColumn("LEGAL HOLIDAY HOURS", RegularHolidayHoursKey, [optional]:=True),
            New ExcelReportColumn("LH HOLIDAY PAY", RegularHolidayPayKey, [optional]:=True),
            New ExcelReportColumn("LEGAL HOLIDAY OT HOURS", RegularHolidayOTHoursKey, [optional]:=True),
            New ExcelReportColumn("LH HOLIDAY OT PAY", RegularHolidayOTPayKey, [optional]:=True),
            New ExcelReportColumn("ALLOWANCE", TotalAllowanceKey, [optional]:=True),
            New ExcelReportColumn("TOTAL GROSS PAY", GrossPayKey),
            New ExcelReportColumn("SSS", SSSAmountKey),
            New ExcelReportColumn("EREC", ECAmountKey),
            New ExcelReportColumn("PAG-IBIG", HDMFAmountKey),
            New ExcelReportColumn("PHILHEALTH", PhilHealthAmountKey),
            New ExcelReportColumn("HMO", HMOAmountKey),
            New ExcelReportColumn("13TH MONTH PAY", ThirteenthMonthPayKey),
            New ExcelReportColumn("5 DAY SILP", FiveDaySilpAmountKey),
            New ExcelReportColumn("NET PAY", NetPayKey)
            })

        Return New ReadOnlyCollection(Of ExcelReportColumn)(reportColumns)
    End Function

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

            Dim newFile = saveFileDialogHelperOutPut.FileInfo

            GenerateReport(selectedMonth.Value, selectedBranch, newFile)

            'GenerateExcel(branchPaystubModels, newFile)
        Catch ex As IOException

            MessageBoxHelper.ErrorMessage(ex.Message)
        Catch ex As Exception

            Debugger.Break()
            MessageBoxHelper.DefaultErrorMessage()

        End Try

    End Sub

    Private Sub GenerateReport(selectedMonth As Date, selectedBranch As Branch, newFile As FileInfo)

        Dim allPayPeriodModels As New BlockingCollection(Of PayPeriodModel)

        If SelectedReportType = ReportType.Branch Then

            If selectedBranch Is Nothing Then

                MessageBoxHelper.ErrorMessage("Please select a valid branch.")

                Return
            End If

            GenerateSingleBranchReport(selectedMonth, selectedBranch, newFile)
        Else
            GenerateMultipleBranchReport(selectedMonth, newFile)

        End If
    End Sub

    Private Sub GenerateSingleBranchReport(selectedMonth As Date, selectedBranch As Branch, newFile As FileInfo)
        GetResources(
            selectedMonth,
            Sub(t)
                Dim dataService As New CostCenterReportDataService()
                Dim payPeriodModels = dataService.GetData(
                    t.Result,
                    selectedBranch,
                    userId:=z_User,
                    isActual:=IsActual)

                GenerateExcel(payPeriodModels.GroupBy(Function(p) p.Branch), newFile)
            End Sub)
    End Sub

    Private Sub GenerateMultipleBranchReport(selectedMonth As Date, newFile As FileInfo)
        Dim branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)
        Dim branches = branchRepository.GetAll()

        Dim generator As New CostCenterReportGeneration(branches, IsActual)

        Dim resources = MainServiceProvider.GetRequiredService(Of CostCenterReportResources)
        resources.Load(selectedMonth)

        GetResources(selectedMonth,
            Sub()
                Dim generationTask = Task.Run(
                    Sub()
                        generator.Start(resources)
                    End Sub
                )

                RunGenerateExportTask(newFile, generator, generationTask)
            End Sub)
    End Sub

    Private Sub RunGenerateExportTask(newFile As FileInfo, generator As CostCenterReportGeneration, generationTask As Task)
        Dim progressDialog = New ProgressDialog(generator, "Generating cost center report...")
        progressDialog.Show()

        generationTask.ContinueWith(
            Sub() GenerationOnSuccess(generator.Results, progressDialog, newFile),
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

    Private Sub GenerationOnSuccess(results As IReadOnlyCollection(Of IResult), progressDialog As ProgressDialog, newFile As FileInfo)

        progressDialog.Close()
        progressDialog.Dispose()

        Dim saveResults = results.
            Select(Function(r) CType(r, CostCenterReportGenerationResult)).
            Where(Function(r) r.IsSuccess).
            SelectMany(Function(r) r.Model).
            GroupBy(Function(m) m.Branch).
            OrderBy(Function(b) b.Key.Name).
            ToList()

        GenerateExcel(saveResults, newFile)
    End Sub

    Private Sub GenerationOnError(t As Task, progressDialog As ProgressDialog)

        progressDialog.Close()
        progressDialog.Dispose()

        Const MessageTitle As String = "Generate Cost Center Report"

        If t.Exception?.InnerException.GetType() Is GetType(BusinessLogicException) Then

            MessageBoxHelper.ErrorMessage(t.Exception?.InnerException.Message, MessageTitle)
        Else
            Debugger.Break()
            MessageBoxHelper.ErrorMessage("Something went wrong while generating the cost center report. Please contact Globagility Inc. for assistance.", MessageTitle)
        End If

    End Sub

    Private Sub GetResources(selectedMonth As Date, callBackAfterLoadResources As Action(Of Task(Of CostCenterReportResources)))
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
            AddressOf LoadingResourceOnError,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Sub LoadingResourceOnError(obj As Task(Of CostCenterReportResources))
        MsgBox("Something went wrong while loading the cost center report resources. Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Resources")
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

    Private Sub GenerateExcel(
        branchPaystubModels As IEnumerable(Of IGrouping(Of Branch, PayPeriodModel)),
        newFile As FileInfo)

        Using excel = New ExcelPackage(newFile)
            Dim subTotalRows = New List(Of Integer)

            Dim worksheet = excel.Workbook.Worksheets.Add("Sheet1")

            Dim viewableReportColumns = GetViewableReportColumns(branchPaystubModels)

            RenderWorksheet(branchPaystubModels, worksheet, viewableReportColumns)

            SetDefaultPrinterSettings(worksheet.PrinterSettings)

            excel.Save()
        End Using

        Process.Start(newFile.FullName)
    End Sub

    Private Sub RenderWorksheet(
        branchPaystubModels As IEnumerable(Of IGrouping(Of Branch, PayPeriodModel)),
        worksheet As ExcelWorksheet,
        viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn))

        Dim rowIndex As Integer = 1
        Dim lastColumn As String = GetLastColumn(viewableReportColumns)

        worksheet.Cells.Style.Font.Size = FontSize

        Dim systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        If systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark Then
            worksheet.Cells.Style.Font.Name = "Book Antiqua"
        End If

        Dim organizationCell = worksheet.Cells(rowIndex, 1)
        organizationCell.Value = orgNam.ToUpper()
        organizationCell.Style.Font.Bold = True
        rowIndex += 1

        Dim monthlyBranchSubTotalRows = New List(Of Integer)

        For Each branchGroup In branchPaystubModels

            If branchGroup.ToList().SelectMany(Function(p) p.Paystubs).Any() Then
                rowIndex = RenderBranchData(worksheet, branchGroup, viewableReportColumns, monthlyBranchSubTotalRows, lastColumn, rowIndex)

            End If

        Next

        If monthlyBranchSubTotalRows.Count > 1 Then

            rowIndex += 3

            RenderGrandTotalLabel(worksheet, "GRAND TOTAL", rowIndex)
            RenderGrandTotal(worksheet, rowIndex, SecondColumn, lastColumn, monthlyBranchSubTotalRows, ExcelBorderStyle.Thick)
        End If

    End Sub

    Private Function GetViewableReportColumns(branchPaystubModels As IEnumerable(Of IGrouping(Of Branch, PayPeriodModel))) As IReadOnlyCollection(Of ExcelReportColumn)

        Dim payPeriodModels = branchPaystubModels.
            SelectMany(Function(b) b.ToList()).
            ToList()

        Dim viewableReportColumns As New List(Of ExcelReportColumn)

        For Each column In _reportColumns
            If column.Optional = False Then

                viewableReportColumns.Add(column)
                Continue For
            End If

            Dim totalValue As Decimal = 0

            payPeriodModels.ForEach(
                Sub(m)
                    m.Paystubs.ForEach(
                    Sub(p)
                        totalValue += Convert.ToDecimal(p.LookUp(column.Source))
                    End Sub)
                End Sub)

            If totalValue <> 0 Then

                viewableReportColumns.Add(column)

            End If

        Next

        Return viewableReportColumns
    End Function

    Private Function RenderBranchData(
        worksheet As ExcelWorksheet,
        branchGroup As IGrouping(Of Branch, PayPeriodModel),
        viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn),
        monthlyBranchSubTotalRows As List(Of Integer),
        lastColumn As String,
        rowIndex As Integer) As Integer

        Dim branch As Branch = branchGroup.Key
        Dim branchName As String = branch.Name

        Dim payPeriods As ICollection(Of PayPeriodModel) = branchGroup.ToList()

        Dim branchSubTotalRows As New List(Of Integer)

        ' space after the title
        rowIndex += 1

        For Each payPeriodModel In payPeriods
            Dim branchNameCell As ExcelRange = worksheet.Cells(rowIndex, 1)
            branchNameCell.Value = GetPayPeriodDescription(payPeriodModel.PayPeriod)
            branchNameCell.Style.Font.Bold = True
            rowIndex += 1

            Dim payPeriodDateCell As ExcelRange = worksheet.Cells(rowIndex, 1)
            payPeriodDateCell.Value = branchName
            payPeriodDateCell.Style.Font.Bold = True
            rowIndex += 1

            RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns)
            rowIndex += 1

            If payPeriodModel.Paystubs.Count > 0 Then
                rowIndex = RenderGroupedRows(
                    worksheet,
                    viewableReportColumns,
                    branchSubTotalRows,
                    rowIndex,
                    lastColumn,
                    payPeriodModel)
            Else
                RenderZeroTotal(worksheet, rowIndex, SecondColumn, lastColumn, ExcelBorderStyle.Thin)

                rowIndex += 1
            End If

            rowIndex += 1

        Next

        worksheet.Cells.AutoFitColumns()
        worksheet.Cells("A1").AutoFitColumns(4.9, 5.3)

        rowIndex += 1

        If payPeriods.Count > 1 Then

            RenderGrandTotalLabel(worksheet, branchName, rowIndex)
            RenderGrandTotal(worksheet, rowIndex, SecondColumn, lastColumn, branchSubTotalRows)

            monthlyBranchSubTotalRows.Add(rowIndex)
        End If

        rowIndex += 1

        Return rowIndex
    End Function

    Private Sub RenderGrandTotalLabel(worksheet As ExcelWorksheet, branchName As String, rowIndex As Integer)
        Dim column = $"{FirstColumn}{rowIndex}"
        Dim cell = worksheet.Cells(column)
        cell.Style.Font.Bold = True

        cell.Value = branchName
    End Sub

    Private Function GetPayPeriodDescription(payPeriod As TimePeriod) As String

        If payPeriod Is Nothing Then Return String.Empty

        If payPeriod.Start.Month = payPeriod.End.Month Then

            Return $"{payPeriod.Start.ToString("MMMM").ToUpper} {payPeriod.Start.Day} - {payPeriod.End.Day}"
        Else
            Return $"{payPeriod.Start.ToString("MMMM").ToUpper} {payPeriod.Start.Day} - {payPeriod.End.ToString("MMMM").ToUpper} {payPeriod.End.Day}"

        End If

    End Function

    Private Function RenderGroupedRows(
        worksheet As ExcelWorksheet,
        viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn),
        subTotalRows As List(Of Integer),
        rowIndex As Integer,
        lastColumn As String,
        payPeriodModel As PayPeriodModel) As Integer

        Dim employeesStartRowIndex As Integer = rowIndex
        Dim employeesLastRowIndex As Integer = rowIndex

        For Each paystub In payPeriodModel.Paystubs
            Dim letters = GenerateAlphabet.GetEnumerator()
            Dim propertyLookUp = paystub.LookUp

            For Each reportColumn In viewableReportColumns
                letters.MoveNext()
                Dim alphabet = letters.Current

                Dim column = $"{alphabet}{rowIndex}"

                Dim cell = worksheet.Cells(column)

                Dim value = propertyLookUp(reportColumn.Source)
                If reportColumn.Type = ExcelReportColumn.ColumnType.Numeric Then
                    cell.Value = CDec(value)
                Else
                    cell.Value = value

                End If

                If reportColumn.Type = ExcelReportColumn.ColumnType.Numeric Then
                    cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)"
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                End If
            Next

            employeesLastRowIndex = rowIndex
            rowIndex += 1
        Next

        Dim subTotalCellRange = $"B{rowIndex}:{lastColumn}{rowIndex}"

        subTotalRows.Add(rowIndex)

        RenderSubTotal(
            worksheet,
            subTotalCellRange,
            employeesStartRowIndex,
            employeesLastRowIndex,
            formulaColumnStart:=2)

        rowIndex += 1

        Return rowIndex
    End Function

End Class
