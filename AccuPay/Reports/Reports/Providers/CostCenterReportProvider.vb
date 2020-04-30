Option Strict On

Imports System.Collections.ObjectModel
Imports System.IO
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.CostCenterReportDataService
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Entity
Imports AccuPay.ExcelReportColumn
Imports AccuPay.Helpers
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class CostCenterReportProvider
    Inherits ExcelFormatReport
    Implements IReportProvider

    Public Property Name As String = "Cost Center Report" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _reportColumns As IReadOnlyCollection(Of ExcelReportColumn) = GetReportColumns()

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

    Private Shared Function GetReportColumns() As ReadOnlyCollection(Of ExcelReportColumn)

        Dim reportColumns = New List(Of ExcelReportColumn)({
                New ExcelReportColumn("NAME OF EMPLOYEES", EmployeeNameKey, ColumnType.Text),
                New ExcelReportColumn("NO. OF DAYS", TotalDaysKey),
                New ExcelReportColumn("NO. OF HOURS", TotalHoursKey),
                New ExcelReportColumn("RATE", DailyRateKey),
                New ExcelReportColumn("HOURLY", HoulyRateKey),
                New ExcelReportColumn("GROSS PAY", BasicPayKey),
                New ExcelReportColumn("NO. OF OT HOURS", OvertimeHoursKey),
                New ExcelReportColumn("OT PAY", OvertimePayKey),
                New ExcelReportColumn("NO. OF ND HOURS", NightDiffHoursKey),
                New ExcelReportColumn("ND PAY", NightDiffPayKey),
                New ExcelReportColumn("NO. OF NDOT HOURS", NightDiffOvertimeHoursKey),
                New ExcelReportColumn("NDOT PAY", NightDiffOvertimePayKey),
                New ExcelReportColumn("SP HOLIDAY HOURS", SpecialHolidayHoursKey),
                New ExcelReportColumn("SP HOLIDAY PAY", SpecialHolidayPayKey),
                New ExcelReportColumn("SP HOLIDAY OT HOURS", SpecialHolidayOTHoursKey),
                New ExcelReportColumn("SP HOLIDAY OT PAY", SpecialHolidayOTPayKey),
                New ExcelReportColumn("LEGAL HOLIDAY HOURS", RegularHolidayHoursKey),
                New ExcelReportColumn("LH HOLIDAY PAY", RegularHolidayPayKey),
                New ExcelReportColumn("LEGAL HOLIDAY OT HOURS", RegularHolidayOTHoursKey),
                New ExcelReportColumn("LH HOLIDAY OT PAY", RegularHolidayOTPayKey),
                New ExcelReportColumn("ALLOWANCE", TotalAllowanceKey),
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
            Dim selectMonthForm As New selectMonth

            If Not selectMonthForm.ShowDialog = Windows.Forms.DialogResult.OK Then
                Return
            End If
            Dim selectedMonth As Date = CDate(selectMonthForm.MonthValue).ToMinimumDateValue

            Dim selectedBranch = GetSelectedBranch()
            If selectedBranch Is Nothing Then Return

            Dim defaultFileName = GetDefaultFileName("Cost Center Report", selectedBranch, selectedMonth)

            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

            If saveFileDialogHelperOutPut.IsSuccess = False Then
                Return
            End If

            Dim newFile = saveFileDialogHelperOutPut.FileInfo

            Dim dataService As New CostCenterReportDataService(selectedMonth,
                                                                selectedBranch,
                                                                userId:=z_User)

            Dim payPeriodModels = dataService.GetData()

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

    Private Shared Function GetSelectedBranch() As Data.Entities.Branch

        Dim selectBranchDialog As New SelectBranchForm

        If Not selectBranchDialog.ShowDialog <> DialogResult.OK Then
            Return Nothing
        End If

        Return selectBranchDialog.SelectedBranch

    End Function

    Protected Shared Function GetDefaultFileName(reportName As String,
                                                 selectedBranch As Data.Entities.Branch,
                                                 selectedMonth As Date) As String
        Return String.Concat(selectedBranch.Name, " ",
                            reportName, " ",
                            "- ",
                            selectedMonth.ToString("MMMM"),
                            ".xlsx")
    End Function

    Private Sub GenerateExcel(payPeriodModels As List(Of PayPeriodModel),
                              newFile As FileInfo,
                              selectedBranch As Data.Entities.Branch)

        Using excel = New ExcelPackage(newFile)
            Dim subTotalRows = New List(Of Integer)

            Dim worksheet = excel.Workbook.Worksheets.Add("Sheet1")

            RenderWorksheet(worksheet, payPeriodModels, _reportColumns, selectedBranch)

            excel.Save()
        End Using
    End Sub

    Private Sub RenderWorksheet(worksheet As ExcelWorksheet,
                                payPeriods As ICollection(Of PayPeriodModel),
                                viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn),
                                selectedBranch As Data.Entities.Branch)
        Dim subTotalRows = New List(Of Integer)

        worksheet.Cells.Style.Font.Size = FontSize

        Dim sys_ownr As New SystemOwnerService()

        If sys_ownr.GetCurrentSystemOwner() = SystemOwnerService.Benchmark Then
            worksheet.Cells.Style.Font.Name = "Book Antiqua"
        End If

        Dim rowIndex = 1
        Dim organizationCell = worksheet.Cells(rowIndex, 1)
        organizationCell.Value = orgNam.ToUpper()
        organizationCell.Style.Font.Bold = True
        rowIndex += 1

        ' space after the title
        rowIndex += 1
        Dim lastCell = String.Empty

        For Each payPeriodModel In payPeriods
            Dim branchNameCell = worksheet.Cells(rowIndex, 1)
            branchNameCell.Value = selectedBranch.Name.ToUpper()
            branchNameCell.Style.Font.Bold = True
            rowIndex += 1
            Dim payPeriodDateCell = worksheet.Cells(rowIndex, 1)
            payPeriodDateCell.Value = GetPayPeriodDescription(payPeriodModel.PayPeriod)
            payPeriodDateCell.Style.Font.Bold = True
            rowIndex += 1

            RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns)
            rowIndex += 1

            Dim employeesStartIndex = rowIndex
            Dim employeesLastIndex = 0

            If payPeriodModel.Paystubs.Count > 0 Then
                RenderGroupedRows(worksheet,
                                  viewableReportColumns,
                                  subTotalRows,
                                  rowIndex,
                                  lastCell,
                                  payPeriodModel,
                                  employeesStartIndex,
                                  employeesLastIndex)

            End If

        Next

        worksheet.Cells.AutoFitColumns()
        worksheet.Cells("A1").AutoFitColumns(4.9, 5.3)

        rowIndex += 1

        If payPeriods.Count > 1 Then
            RenderGrandTotal(worksheet, rowIndex, lastCell, subTotalRows, "B"c)
        End If

        rowIndex += 1

        SetDefaultPrinterSettings(worksheet.PrinterSettings)
    End Sub

    Private Function GetPayPeriodDescription(payPeriod As TimePeriod) As String

        If payPeriod Is Nothing Then Return String.Empty

        If payPeriod.Start.Month = payPeriod.End.Month Then

            Return $"{payPeriod.Start.ToString("MMMM").ToUpper} {payPeriod.Start.Day} - {payPeriod.End.Day}"
        Else
            Return $"{payPeriod.Start.ToString("MMMM").ToUpper} {payPeriod.Start.Day} - {payPeriod.End.ToString("MMMM").ToUpper} {payPeriod.End.Day}"

        End If

    End Function

    Private Sub RenderGroupedRows(worksheet As ExcelWorksheet,
                                  viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn),
                                  subTotalRows As List(Of Integer),
                                  ByRef rowIndex As Integer,
                                  ByRef lastCell As String,
                                  payPeriodModel As PayPeriodModel,
                                  employeesStartIndex As Integer,
                                  ByRef employeesLastIndex As Integer)

        For Each paystub In payPeriodModel.Paystubs
            Dim letters = GenerateAlphabet.GetEnumerator()
            Dim propertyLookUp = paystub.LookUp

            For Each reportColumn In viewableReportColumns
                letters.MoveNext()
                Dim alphabet = letters.Current

                Dim column = $"{alphabet}{rowIndex}"

                Dim cell = worksheet.Cells(column)

                Dim value = propertyLookUp(reportColumn.Source)
                If reportColumn.Type = ColumnType.Numeric Then
                    cell.Value = CDec(value)
                Else
                    cell.Value = value

                End If

                If reportColumn.Type = ColumnType.Numeric Then
                    cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)"
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                End If
            Next

            lastCell = letters.Current
            employeesLastIndex = rowIndex
            rowIndex += 1
        Next

        Dim subTotalCellRange = $"B{rowIndex}:{lastCell}{rowIndex}"

        subTotalRows.Add(rowIndex)

        RenderSubTotal(worksheet,
                       subTotalCellRange,
                       employeesStartIndex,
                       employeesLastIndex,
                       formulaColumnStart:=2)

        rowIndex += 2
    End Sub

End Class