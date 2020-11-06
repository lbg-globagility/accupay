Option Strict On

Imports System.Collections.ObjectModel
Imports System.IO
Imports AccuPay.Data.Entities
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

    Public Property Name As String = "Cost Center Report" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Property IsActual As Boolean

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

            Dim selectedBranch = GetSelectedBranch()
            If selectedBranch?.RowID Is Nothing Then Return

            Dim defaultFileName = GetDefaultFileName("Cost Center Report", selectedBranch, selectedMonth.Value)

            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

            If saveFileDialogHelperOutPut.IsSuccess = False Then
                Return
            End If

            Dim newFile = saveFileDialogHelperOutPut.FileInfo

            Dim dataService = MainServiceProvider.GetRequiredService(Of CostCenterReportDataService)
            Dim payPeriodModels = dataService.GetData(
                selectedMonth.Value,
                selectedBranch,
                userId:=z_User,
                isActual:=IsActual)

            If payPeriodModels.Any = False OrElse payPeriodModels.Sum(Function(p) p.Paystubs.Count) = 0 Then

                MessageBoxHelper.ErrorMessage("No paystubs to show.")
                Return

            End If

            GenerateExcel(payPeriodModels, newFile, selectedBranch)

            Process.Start(newFile.FullName)
        Catch ex As IOException

            MessageBoxHelper.ErrorMessage(ex.Message)
        Catch ex As Exception

            Debugger.Break()
            MessageBoxHelper.DefaultErrorMessage()

        End Try

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

    Protected Shared Function GetDefaultFileName(
        reportName As String,
        selectedBranch As Branch,
        selectedMonth As Date) As String

        Return String.Concat(
            selectedBranch.Name, " ",
            reportName, " ",
            "- ",
            selectedMonth.ToString("MMMM"),
            ".xlsx")
    End Function

    Private Sub GenerateExcel(
        payPeriodModels As List(Of PayPeriodModel),
        newFile As FileInfo,
        selectedBranch As Branch)

        Using excel = New ExcelPackage(newFile)
            Dim subTotalRows = New List(Of Integer)

            Dim worksheet = excel.Workbook.Worksheets.Add("Sheet1")

            Dim viewableReportColumns = GetViewableReportColumns(payPeriodModels)

            RenderWorksheet(payPeriodModels, selectedBranch, worksheet, viewableReportColumns)

            SetDefaultPrinterSettings(worksheet.PrinterSettings)

            excel.Save()
        End Using
    End Sub

    Private Sub RenderWorksheet(
        payPeriodModels As List(Of PayPeriodModel),
        selectedBranch As Branch,
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
        For index = 1 To 3

            rowIndex = RenderBranchData(worksheet, payPeriodModels, viewableReportColumns, monthlyBranchSubTotalRows, selectedBranch, lastColumn, rowIndex)

        Next

        If monthlyBranchSubTotalRows.Count > 1 Then

            rowIndex += 3

            RenderGrandTotalLabel(worksheet, "GRAND TOTAL", rowIndex)
            RenderGrandTotal(worksheet, rowIndex, SecondColumn, lastColumn, monthlyBranchSubTotalRows, ExcelBorderStyle.Thick)
        End If

    End Sub

    Private Function GetViewableReportColumns(payPeriodModels As List(Of PayPeriodModel)) As IReadOnlyCollection(Of ExcelReportColumn)

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
        payPeriods As ICollection(Of PayPeriodModel),
        viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn),
        monthlyBranchSubTotalRows As List(Of Integer),
        selectedBranch As Branch,
        lastColumn As String,
        rowIndex As Integer) As Integer

        Dim branchName As String = selectedBranch.Name.ToUpper()
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
