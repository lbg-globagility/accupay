Option Strict On

Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Threading.Tasks
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Entity
Imports AccuPay.ExcelReportColumn
Imports AccuPay.Helpers
Imports AccuPay.Utilities
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class PayrollSummaryExcelFormatReportProvider
    Inherits ExcelFormatReport
    Implements IReportProvider

    Public Property Name As String = "Payroll Summary" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private Const adjustmentColumn As String = "(Adj.)"

    Private Const totalAdjustmentColumn As String = "Adj."

    Private ReadOnly _reportColumns As IReadOnlyCollection(Of ExcelReportColumn) = GetReportColumns()

    Private Const EmployeeRowIDColumnName As String = "EmployeeRowID"

    Private _settings As ListOfValueCollection

    Private _payPeriodRepository As PayPeriodRepository

    Public Property IsActual As Boolean

    Sub New()
        _payPeriodRepository = New PayPeriodRepository()

        _settings = ListOfValueCollection.Create()
    End Sub

    Private Shared Function GetReportColumns() As ReadOnlyCollection(Of ExcelReportColumn)

        Dim allowanceColumnName = "Allowance"

        Dim reportColumns = New List(Of ExcelReportColumn)({
                New ExcelReportColumn("Code", "DatCol2", ColumnType.Text),
                New ExcelReportColumn("Full Name", "DatCol3", ColumnType.Text),
                New ExcelReportColumn("Rate", "Rate"),
                New ExcelReportColumn("Basic Hours", "BasicHours"),
                New ExcelReportColumn("Basic Pay", "BasicPay"),
                New ExcelReportColumn("Reg Hrs", "RegularHours"),
                New ExcelReportColumn("Reg Pay", "RegularPay"),
                New ExcelReportColumn("OT Hrs", "OvertimeHours", [optional]:=True),
                New ExcelReportColumn("OT Pay", "OvertimePay", [optional]:=True),
                New ExcelReportColumn("ND Hrs", "NightDiffHours", [optional]:=True),
                New ExcelReportColumn("ND Pay", "NightDiffPay", [optional]:=True),
                New ExcelReportColumn("NDOT Hrs", "NightDiffOvertimeHours", [optional]:=True),
                New ExcelReportColumn("NDOT Pay", "NightDiffOvertimePay", [optional]:=True),
                New ExcelReportColumn("R.Day Hrs", "RestDayHours", [optional]:=True),
                New ExcelReportColumn("R.Day Pay", "RestDayPay", [optional]:=True),
                New ExcelReportColumn("R.DayOT Hrs", "RestDayOTHours", [optional]:=True),
                New ExcelReportColumn("R.DayOT Pay", "RestDayOTPay", [optional]:=True),
                New ExcelReportColumn("R.Day ND Hrs", "RestDayNightDiffHours", [optional]:=True),
                New ExcelReportColumn("R.Day ND Pay", "RestDayNightDiffPay", [optional]:=True),
                New ExcelReportColumn("R.Day NDOT Hrs", "RestDayNightDiffOTHours", [optional]:=True),
                New ExcelReportColumn("R.Day NDOT Pay", "RestDayNightDiffOTPay", [optional]:=True),
                New ExcelReportColumn("S.Hol Hrs", "SpecialHolidayHours", [optional]:=True),
                New ExcelReportColumn("S.Hol Pay", "SpecialHolidayPay", [optional]:=True),
                New ExcelReportColumn("S.HolOT Hrs", "SpecialHolidayOTHours", [optional]:=True),
                New ExcelReportColumn("S.HolOT Pay", "SpecialHolidayOTPay", [optional]:=True),
                New ExcelReportColumn("S.Hol ND Hrs", "SpecialHolidayNightDiffHours", [optional]:=True),
                New ExcelReportColumn("S.Hol ND Pay", "SpecialHolidayNightDiffPay", [optional]:=True),
                New ExcelReportColumn("S.Hol NDOT Hrs", "SpecialHolidayNightDiffOTHours", [optional]:=True),
                New ExcelReportColumn("S.Hol NDOT Pay", "SpecialHolidayNightDiffOTPay", [optional]:=True),
                New ExcelReportColumn("S.Hol R.Day Hrs", "SpecialHolidayRestDayHours", [optional]:=True),
                New ExcelReportColumn("S.Hol R.Day Pay", "SpecialHolidayRestDayPay", [optional]:=True),
                New ExcelReportColumn("S.Hol R.DayOT Hrs", "SpecialHolidayRestDayOTHours", [optional]:=True),
                New ExcelReportColumn("S.Hol R.DayOT Pay", "SpecialHolidayRestDayOTPay", [optional]:=True),
                New ExcelReportColumn("S.Hol R.Day ND Hrs", "SpecialHolidayRestDayNightDiffHours", [optional]:=True),
                New ExcelReportColumn("S.Hol R.Day ND Pay", "SpecialHolidayRestDayNightDiffPay", [optional]:=True),
                New ExcelReportColumn("S.Hol R.Day NDOT Hrs", "SpecialHolidayRestDayNightDiffOTHours", [optional]:=True),
                New ExcelReportColumn("S.Hol R.Day NDOT Pay", "SpecialHolidayRestDayNightDiffOTPay", [optional]:=True),
                New ExcelReportColumn("R.Hol Hrs", "RegularHolidayHours", [optional]:=True),
                New ExcelReportColumn("R.Hol Pay", "RegularHolidayPay", [optional]:=True),
                New ExcelReportColumn("R.HolOT Hrs", "RegularHolidayOTHours", [optional]:=True),
                New ExcelReportColumn("R.HolOT Pay", "RegularHolidayOTPay", [optional]:=True),
                New ExcelReportColumn("R.Hol ND Hrs", "RegularHolidayNightDiffHours", [optional]:=True),
                New ExcelReportColumn("R.Hol ND Pay", "RegularHolidayNightDiffPay", [optional]:=True),
                New ExcelReportColumn("R.Hol NDOT Hrs", "RegularHolidayNightDiffOTHours", [optional]:=True),
                New ExcelReportColumn("R.Hol NDOT Pay", "RegularHolidayNightDiffOTPay", [optional]:=True),
                New ExcelReportColumn("R.Hol R.Day Hrs", "RegularHolidayRestDayHours", [optional]:=True),
                New ExcelReportColumn("R.Hol R.Day Pay", "RegularHolidayRestDayPay", [optional]:=True),
                New ExcelReportColumn("R.Hol R.DayOT Hrs", "RegularHolidayRestDayOTHours", [optional]:=True),
                New ExcelReportColumn("R.Hol R.DayOT Pay", "RegularHolidayRestDayOTPay", [optional]:=True),
                New ExcelReportColumn("R.Hol R.Day ND Hrs", "RegularHolidayRestDayNightDiffHours", [optional]:=True),
                New ExcelReportColumn("R.Hol R.Day ND Pay", "RegularHolidayRestDayNightDiffPay", [optional]:=True),
                New ExcelReportColumn("R.Hol R.Day NDOT Hrs", "RegularHolidayRestDayNightDiffOTHours", [optional]:=True),
                New ExcelReportColumn("R.Hol R.Day NDOT Pay", "RegularHolidayRestDayNightDiffOTPay", [optional]:=True),
                New ExcelReportColumn("Leave Hrs", "LeaveHours", [optional]:=True),
                New ExcelReportColumn("Leave Pay", "LeavePay", [optional]:=True),
                New ExcelReportColumn("Late Hrs", "LateHours", [optional]:=True),
                New ExcelReportColumn("Late Amt", "LateDeduction", [optional]:=True),
                New ExcelReportColumn("UT Hrs", "UndertimeHours", [optional]:=True),
                New ExcelReportColumn("UT Amt", "UndertimeDeduction", [optional]:=True),
                New ExcelReportColumn("Absent Hrs", "AbsentHours", [optional]:=True),
                New ExcelReportColumn("Absent Amt", "AbsentDeduction", [optional]:=True),
                New ExcelReportColumn(allowanceColumnName, "TotalAllowance"),
                New ExcelReportColumn("Bonus", "TotalBonus", [optional]:=True),
                New ExcelReportColumn("Gross", "GrossIncome"),
                New ExcelReportColumn("SSS", "SSS", [optional]:=True),
                New ExcelReportColumn("Ph.Health", "PhilHealth", [optional]:=True),
                New ExcelReportColumn("HDMF", "HDMF", [optional]:=True),
                New ExcelReportColumn("Taxable", "TaxableIncome"),
                New ExcelReportColumn("W.Tax", "WithholdingTax"),
                New ExcelReportColumn("Loan", "TotalLoans"),
                New ExcelReportColumn("A.Fee", "AgencyFee", [optional]:=True),
                New ExcelReportColumn(totalAdjustmentColumn, "TotalAdjustments", [optional]:=True),
                New ExcelReportColumn("Net Pay", "NetPay"),
                New ExcelReportColumn("13th Month", "13thMonthPay"),
                New ExcelReportColumn("Total", "Total")
            })

        Dim sys_ownr As New SystemOwnerService()

        If sys_ownr.GetCurrentSystemOwner() = SystemOwnerService.Benchmark Then

            Dim allowanceColumn = reportColumns.Where(Function(r) r.Name = allowanceColumnName).FirstOrDefault

            If allowanceColumn IsNot Nothing Then

                allowanceColumn.Name = "Ecola"

            End If

        End If

        Return New ReadOnlyCollection(Of ExcelReportColumn)(reportColumns)
    End Function

    Public Async Sub Run() Implements IReportProvider.Run
        Dim bool_result As Short = Convert.ToInt16(IsActual)

        Dim payrollSelector = GetPayrollSelector()
        If payrollSelector Is Nothing Then Return

        Dim keepInOneSheet = Convert.ToBoolean(ExcelOptionFormat())

        Dim parameters = New Object() {
            orgztnID,
            payrollSelector.PayPeriodFromID,
            payrollSelector.PayPeriodToID,
            bool_result,
            payrollSelector.cboStringParameter.Text,
            keepInOneSheet
        }

        Dim hideEmptyColumns = payrollSelector.chkHideEmptyColumns.Checked

        Try
            Dim query = New SQL(
                "CALL PAYROLLSUMMARY2(?og_rowid, ?min_pp_rowid, ?max_pp_rowid, ?is_actual, ?salaray_distrib, ?keep_in_onesheet);",
                parameters)

            Dim ds = query.GetFoundRows
            If query.HasError Then
                Throw query.ErrorException
            End If

            Dim employeeTable = ds.Tables.OfType(Of DataTable).FirstOrDefault()
            Dim allEmployees = employeeTable.Rows.OfType(Of DataRow).ToList()

            If allEmployees.Count <= 0 Then
                Throw New Exception("No paystubs to show.")
            End If

            Dim reportName As String = "PayrollSummary"

            Dim short_dates() As String = New String() {
                CDate(payrollSelector.DateFrom).ToShortDateString,
                CDate(payrollSelector.DateTo).ToShortDateString}

            Dim defaultFileName = GetDefaultFileName(reportName, payrollSelector)

            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

            If saveFileDialogHelperOutPut.IsSuccess = False Then
                Return
            End If

            Dim newFile = saveFileDialogHelperOutPut.FileInfo

            Dim viewableReportColumns = Await GetViewableReportColumns(allEmployees, hideEmptyColumns, payrollSelector)

            Dim employeeGroups = GroupEmployees(allEmployees)

            Using excel = New ExcelPackage(newFile)
                Dim subTotalRows = New List(Of Integer)

                If keepInOneSheet Then
                    Dim worksheet = excel.Workbook.Worksheets.Add(reportName)

                    RenderWorksheet(worksheet, employeeGroups, short_dates, viewableReportColumns, payrollSelector)
                Else
                    For Each employeeGroup In employeeGroups
                        Dim worksheet = excel.Workbook.Worksheets.Add(employeeGroup.DivisionName)

                        Dim currentGroup = New Collection(Of EmployeeGroup) From {
                            employeeGroup
                        }

                        RenderWorksheet(worksheet, currentGroup, short_dates, viewableReportColumns, payrollSelector)
                    Next
                End If

                excel.Save()
            End Using

            Process.Start(newFile.FullName)
        Catch ex As IOException

            MessageBoxHelper.ErrorMessage(ex.Message)
        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))
        End Try
    End Sub

    Private Function GetDefaultFileName(reportName As String,
                                                 payrollSelector As PayrollSummaDateSelection) As String
        Return String.Concat(orgNam,
                            reportName,
                            payrollSelector.cboStringParameter.Text.Replace(" ", ""),
                            "Report",
                            String.Concat(
                                payrollSelector.DateFrom.Value.
                                    ToShortDateString().Replace("/", "-"),
                                "TO",
                                payrollSelector.DateTo.Value.
                                    ToShortDateString().Replace("/", "-")),
                            ".xlsx")
    End Function

    Private Sub RenderWorksheet(worksheet As ExcelWorksheet,
                                employeeGroups As ICollection(Of EmployeeGroup),
                                short_dates As String(),
                                viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn),
                                PayrollSummaDateSelection As PayrollSummaDateSelection)
        Dim subTotalRows = New List(Of Integer)

        worksheet.Cells.Style.Font.Size = FontSize

        Dim sys_ownr As New SystemOwnerService()

        If sys_ownr.GetCurrentSystemOwner() = SystemOwnerService.Benchmark Then
            worksheet.Cells.Style.Font.Name = "Book Antiqua"
        End If

        Dim organizationCell = worksheet.Cells(1, 1)
        organizationCell.Value = orgNam.ToUpper()
        organizationCell.Style.Font.Bold = True

        Dim attendancePeriodCell = worksheet.Cells(2, 1)
        Dim attendancePeriodDescription = $"For the period of {short_dates(0)} to {short_dates(1)}"
        attendancePeriodCell.Value = attendancePeriodDescription

        Dim lastCell = String.Empty
        Dim rowIndex As Integer = 4

        If ShowCoveredPeriod() Then

            attendancePeriodCell.Value = $"Attendance Period: {short_dates(0)} to {short_dates(1)}"

            Dim payFromNextCutOff = Data.Helpers.PayrollTools.GetNextPayPeriod(PayrollSummaDateSelection.PayPeriodFromID.Value)
            Dim payToNextCutOff = Data.Helpers.PayrollTools.GetNextPayPeriod(PayrollSummaDateSelection.PayPeriodToID.Value)

            Dim payrollPeriodCell = worksheet.Cells(3, 1)
            Dim payrollPeriodDescription = $"Payroll Period: {If(payFromNextCutOff?.PayFromDate Is Nothing, "", payFromNextCutOff.PayFromDate.ToShortDateString)} to {If(payToNextCutOff?.PayToDate Is Nothing, "", payToNextCutOff.PayToDate.ToShortDateString)}"
            payrollPeriodCell.Value = payrollPeriodDescription

            rowIndex = 5

        End If

        For Each employeeGroup In employeeGroups
            Dim divisionCell = worksheet.Cells(rowIndex, 1)
            divisionCell.Value = employeeGroup.DivisionName
            divisionCell.Style.Font.Italic = True

            rowIndex += 1

            RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns)

            rowIndex += 1

            Dim employeesStartIndex = rowIndex
            Dim employeesLastIndex = 0

            For Each employee In employeeGroup.Employees
                Dim letters = GenerateAlphabet.GetEnumerator()

                For Each reportColumn In viewableReportColumns
                    letters.MoveNext()
                    Dim alphabet = letters.Current

                    Dim column = $"{alphabet}{rowIndex}"

                    Dim cell = worksheet.Cells(column)
                    Dim sourceName = reportColumn.Source
                    cell.Value = GetCellValue(employee, sourceName)

                    If reportColumn.Type = ColumnType.Numeric Then
                        cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)"
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                    End If
                Next

                lastCell = letters.Current

                employeesLastIndex = rowIndex
                rowIndex += 1
            Next

            Dim subTotalCellRange = $"C{rowIndex}:{lastCell}{rowIndex}"

            subTotalRows.Add(rowIndex)

            RenderSubTotal(worksheet,
                           subTotalCellRange,
                           employeesStartIndex,
                           employeesLastIndex,
                           formulaColumnStart:=3)

            rowIndex += 2
        Next

        worksheet.Cells.AutoFitColumns()
        worksheet.Cells("A1").AutoFitColumns(4.9, 5.3)

        rowIndex += 1

        If employeeGroups.Count > 1 Then
            RenderGrandTotal(worksheet, rowIndex, lastCell, subTotalRows, "C"c)
        End If

        rowIndex += 1

        RenderSignatureFields(worksheet, rowIndex)
        SetDefaultPrinterSettings(worksheet.PrinterSettings)
    End Sub

    Private Function GetCellValue(employee As DataRow, sourceName As String) As Object
        If sourceName.EndsWith(adjustmentColumn) AndAlso GetPayrollSummaryAdjustmentBreakdownPolicy() <> PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly Then

            If _adjustments Is Nothing Then Return 0

            Dim productName = GetAdjustmentColumnFromName(sourceName)
            Dim employeeId = ObjectUtils.ToNullableInteger(employee(EmployeeRowIDColumnName))

            Dim adjustment = _adjustments.
                    Where(Function(a) a.Product.PartNo = productName).
                    Where(Function(a) a.Paystub.EmployeeID.Value = employeeId.Value).
                    Where(Function(a) a.Paystub.RowID.Value = ObjectUtils.ToInteger(employee("PaystubId"))).
                    Sum(Function(a) a.PayAmount)

            Return adjustment

        End If

        Return employee(sourceName)
    End Function

    Private Async Function GetViewableReportColumns(allEmployees As ICollection(Of DataRow), hideEmptyColumns As Boolean, payrollSummaDateSelection As PayrollSummaDateSelection) As Task(Of IReadOnlyCollection(Of ExcelReportColumn))

        Dim viewableReportColumns = New List(Of ExcelReportColumn)
        For Each reportColumn In _reportColumns
            If reportColumn.Optional AndAlso hideEmptyColumns Then
                Dim hasValue = allEmployees.
                                    Any(Function(row) Not IsDBNull(row(reportColumn.Source)) And Not CDbl(row(reportColumn.Source)) = 0)

                If hasValue Then
                    viewableReportColumns.Add(reportColumn)
                End If
            Else
                viewableReportColumns.Add(reportColumn)
            End If
        Next

        If GetPayrollSummaryAdjustmentBreakdownPolicy() <> PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly Then
            Await AddAdjustmentBreakdownColumns(allEmployees, viewableReportColumns, payrollSummaDateSelection)
        End If

        Return viewableReportColumns
    End Function

    Private Sub RenderSignatureFields(worksheet As ExcelWorksheet, startIdx As Integer)
        Dim index As Integer = (startIdx + 1)

        With worksheet
            .Cells($"A{index}").Value = "Prepared by: "
            .Cells($"A{index}:B{index}").Merge = True

            index += 1
            .Cells($"A{index}").Value = "Audited by: "
            .Cells($"A{index}:B{index}").Merge = True

            index += 1
            .Cells($"A{index}").Value = "Approved by: "
            .Cells($"A{index}:B{index}").Merge = True
        End With
    End Sub

    Private Function SalaryActualDeclared() As SalaryActualization

        Dim time_logformat As SalaryActualization

        MessageBoxManager.OK = "Declared"

        MessageBoxManager.Cancel = "Actual"

        MessageBoxManager.Register()

        Dim custom_prompt =
            MessageBox.Show("",
                            "",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.None,
                            MessageBoxDefaultButton.Button2)

        If custom_prompt = Windows.Forms.DialogResult.OK Then
            time_logformat = SalaryActualization.Declared
        ElseIf custom_prompt = Windows.Forms.DialogResult.Cancel Then
            time_logformat = SalaryActualization.Actual
        End If

        MessageBoxManager.Unregister()

        Return time_logformat

    End Function

    Private Function ExcelOptionFormat() As ExcelOption

        Dim result_value As ExcelOption

        MessageBoxManager.OK = "(A)"

        MessageBoxManager.Cancel = "(B)"

        MessageBoxManager.Register()

        Dim message_content As String =
            String.Concat("Please select an option :", NewLiner(2),
                          "A ) keep all in one sheet", NewLiner,
                          "B ) separate sheet by department")

        Dim custom_prompt =
            MessageBox.Show(message_content,
                            "Excel sheet format",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.None,
                            MessageBoxDefaultButton.Button1)

        If custom_prompt = Windows.Forms.DialogResult.OK Then
            result_value = ExcelOption.KeepAllInOneSheet
        Else 'If custom_prompt = Windows.Forms.DialogResult.Cancel Then
            result_value = ExcelOption.SeparateEachDepartment
        End If

        MessageBoxManager.Unregister()

        Return result_value

    End Function

    Private Function NewLiner(Optional repetition As Integer = 1) As String
        Dim _result As String = String.Empty

        Dim i = 0
        While i < repetition
            _result = String.Concat(_result, Environment.NewLine)
            i += 1
        End While

        Return _result
    End Function

    Private Function GroupEmployees(allEmployees As ICollection(Of DataRow)) As ICollection(Of EmployeeGroup)
        Dim employeesByGroups = allEmployees.
            GroupBy(Function(r) r("DivisionID").ToString())

        Dim groups = New Collection(Of EmployeeGroup)

        For Each employeesByGroup In employeesByGroups
            Dim group = New EmployeeGroup() With {
                .Employees = employeesByGroup.ToList()
            }

            Dim employee = group.Employees.FirstOrDefault()
            group.DivisionName = employee("DatCol1").ToString()

            groups.Add(group)
        Next

        For Each groupsWithSameDivision In groups.GroupBy(Function(g) g.DivisionName)
            If groupsWithSameDivision.Count > 1 Then
                Dim index = 1
                For Each groupInSameDivision In groupsWithSameDivision
                    groupInSameDivision.DivisionName = $"{groupInSameDivision.DivisionName}-{index}"
                    index += 1
                Next
            End If
        Next

        Return groups
    End Function

    Private Function ShowCoveredPeriod() As Boolean
        Return _settings.GetBoolean("Payroll Summary Policy.ShowCoveredPeriod", False)
    End Function

#Region "Adjustment Breakdown"

    Private _adjustments As List(Of IAdjustment)

    Private Function GetPayrollSummaryAdjustmentBreakdownPolicy() As PayrollSummaryAdjustmentBreakdownPolicy
        Return _settings.GetEnum("Payroll Summary Policy.AdjustmentBreakdown", PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly)
    End Function

    Private Async Function AddAdjustmentBreakdownColumns(allEmployees As ICollection(Of DataRow), viewableReportColumns As List(Of ExcelReportColumn), payrollSummaDateSelection As PayrollSummaDateSelection) As Task

        Dim adjustments = Await GetCurrentAdjustments(payrollSummaDateSelection, allEmployees)

        Dim groupedAdjustments = adjustments.GroupBy(Function(a) a.ProductID).ToList

        Dim totalAdjustmentReportColumn = viewableReportColumns.Where(Function(r) r.Name = totalAdjustmentColumn).FirstOrDefault
        Dim totalAdjustmentColumnIndex = viewableReportColumns.IndexOf(totalAdjustmentReportColumn)

        Dim counter = 1

        'add breakdown columns
        For Each adjustment In groupedAdjustments

            Dim adjustmentName = GetAdjustmentName(adjustment(0).Product?.Name)

            If String.IsNullOrWhiteSpace(adjustmentName) Then Continue For

            viewableReportColumns.Insert(totalAdjustmentColumnIndex + counter, New ExcelReportColumn(adjustmentName, adjustmentName))

            counter += 1
        Next

        If totalAdjustmentColumnIndex >= 0 AndAlso totalAdjustmentColumnIndex < viewableReportColumns.Count Then

            'remove total adjustments column
            viewableReportColumns.RemoveAt(totalAdjustmentColumnIndex)

        End If

        'add back the total adjustment column if it is not BreakdownOnly
        'this will put the column after the adjustment breakdown columns
        If GetPayrollSummaryAdjustmentBreakdownPolicy() <> PayrollSummaryAdjustmentBreakdownPolicy.BreakdownOnly Then

            If GetPayrollSummaryAdjustmentBreakdownPolicy() = PayrollSummaryAdjustmentBreakdownPolicy.Both Then

                totalAdjustmentReportColumn.Name = "Total Adj."

            End If

            If counter > 1 Then
                'we have minus 1 here because we remove the original total adjustment column
                viewableReportColumns.Insert(totalAdjustmentColumnIndex + counter - 1, totalAdjustmentReportColumn)
            Else
                viewableReportColumns.Insert(totalAdjustmentColumnIndex, totalAdjustmentReportColumn)

            End If

        End If

    End Function

    Public Async Function GetCurrentAdjustments(
                            payrollSummaDateSelection As PayrollSummaDateSelection,
                            Optional allEmployees As ICollection(Of DataRow) = Nothing) _
                            As Task(Of List(Of IAdjustment))

        Dim payPeriodFrom As Data.Entities.PayPeriod = Nothing
        Dim payPeriodTo As Data.Entities.PayPeriod = Nothing

        If payrollSummaDateSelection.PayPeriodFromID IsNot Nothing Then

            payPeriodFrom = Await _payPeriodRepository.
                                GetByIdAsync(payrollSummaDateSelection.PayPeriodFromID.Value)
        End If

        If payrollSummaDateSelection.PayPeriodToID IsNot Nothing Then

            payPeriodTo = Await _payPeriodRepository.
                                GetByIdAsync(payrollSummaDateSelection.PayPeriodToID.Value)
        End If

        If payPeriodFrom?.PayFromDate Is Nothing OrElse payPeriodTo?.PayToDate Is Nothing Then

            Throw New ArgumentException("Cannot fetch pay period data.")

        End If

        Using context As New PayrollContext

            Dim employeeIds = GetEmployeeIds(allEmployees)

            Dim adjustmentQuery = GetBaseAdjustmentQuery(context.Adjustments.Where(Function(a) a.OrganizationID.Value = z_OrganizationID), payPeriodFrom.PayFromDate, payPeriodTo.PayToDate, employeeIds)
            Dim actualAdjustmentQuery = GetBaseAdjustmentQuery(context.ActualAdjustments.Where(Function(a) a.OrganizationID.Value = z_OrganizationID), payPeriodFrom.PayFromDate, payPeriodTo.PayToDate, employeeIds)

            If allEmployees Is Nothing Then

                adjustmentQuery.Where(Function(p) employeeIds.Contains(p.Paystub.EmployeeID))
                actualAdjustmentQuery.Where(Function(p) employeeIds.Contains(p.Paystub.EmployeeID))

            End If

            _adjustments = New List(Of IAdjustment)(Await adjustmentQuery.ToListAsync)
            _adjustments.AddRange(New List(Of IAdjustment)(Await actualAdjustmentQuery.ToListAsync))

            Return _adjustments

        End Using
    End Function

    Private Function GetBaseAdjustmentQuery(query As IQueryable(Of IAdjustment), PayFromDate As Date, PayToDate As Date, employeeIds As Decimal?()) As IQueryable(Of IAdjustment)

        Return query.Include(Function(p) p.Product).
                                Include(Function(p) p.Paystub).
                                Include(Function(p) p.Paystub.PayPeriod).
                                Where(Function(p) p.Paystub.PayPeriod.PayFromDate >= PayFromDate).
                                Where(Function(p) p.Paystub.PayPeriod.PayToDate <= PayToDate).
                                Where(Function(p) employeeIds.Contains(p.Paystub.EmployeeID))

    End Function

    Private Shared Function GetAdjustmentName(name As String) As String

        If String.IsNullOrWhiteSpace(name) Then Return Nothing

        Return $"{name} {adjustmentColumn}"

    End Function

    Private Shared Function GetAdjustmentColumnFromName(column As String) As String

        Return column.Replace($" {adjustmentColumn}", "")

    End Function

    Private Function GetEmployeeIds(allEmployees As ICollection(Of DataRow)) As Decimal?()

        Dim employeeIdsArray(allEmployees.Count - 1) As Decimal?

        For index = 0 To employeeIdsArray.Count - 1
            employeeIdsArray(index) = ObjectUtils.ToNullableDecimal(allEmployees(index)(EmployeeRowIDColumnName))
        Next

        Return employeeIdsArray
    End Function

#End Region

    Private Class EmployeeGroup

        Public Property DivisionName As String

        Public Property Employees As ICollection(Of DataRow)

    End Class

End Class

Friend Enum SalaryActualization As Short
    Declared = 0
    Actual = 1
End Enum

Friend Enum ExcelOption As Short
    SeparateEachDepartment = 0
    KeepAllInOneSheet = 1
End Enum