Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class PayrollSummaryExcelFormatReportProvider
    Implements IReportProvider

    Public Property Name As String = "" Implements IReportProvider.Name

    Private Const adjustmentColumn As String = "(Adj.)"
    Private Const totalAdjustmentColumn As String = "Adj."

    Private ReadOnly _reportColumns As IReadOnlyCollection(Of ReportColumn) = New ReadOnlyCollection(Of ReportColumn)({
        New ReportColumn("Code", "DatCol2", ColumnType.Text),
        New ReportColumn("Full Name", "DatCol3", ColumnType.Text),
        New ReportColumn("Rate", "Rate"),
        New ReportColumn("Basic Hours", "BasicHours"),
        New ReportColumn("Basic Pay", "BasicPay"),
        New ReportColumn("Reg Hrs", "RegularHours"),
        New ReportColumn("Reg Pay", "RegularPay"),
        New ReportColumn("OT Hrs", "OvertimeHours", [optional]:=True),
        New ReportColumn("OT Pay", "OvertimePay", [optional]:=True),
        New ReportColumn("ND Hrs", "NightDiffHours", [optional]:=True),
        New ReportColumn("ND Pay", "NightDiffPay", [optional]:=True),
        New ReportColumn("NDOT Hrs", "NightDiffOvertimeHours", [optional]:=True),
        New ReportColumn("NDOT Pay", "NightDiffOvertimePay", [optional]:=True),
        New ReportColumn("R.Day Hrs", "RestDayHours", [optional]:=True),
        New ReportColumn("R.Day Pay", "RestDayPay", [optional]:=True),
        New ReportColumn("R.DayOT Hrs", "RestDayOTHours", [optional]:=True),
        New ReportColumn("R.DayOT Pay", "RestDayOTPay", [optional]:=True),
        New ReportColumn("S.Hol Hrs", "SpecialHolidayHours", [optional]:=True),
        New ReportColumn("S.Hol Pay", "SpecialHolidayPay", [optional]:=True),
        New ReportColumn("S.HolOT Hrs", "SpecialHolidayOTHours", [optional]:=True),
        New ReportColumn("S.HolOT Pay", "SpecialHolidayOTPay", [optional]:=True),
        New ReportColumn("R.Hol Hrs", "RegularHolidayHours", [optional]:=True),
        New ReportColumn("R.Hol Pay", "RegularHolidayPay", [optional]:=True),
        New ReportColumn("R.HolOT Hrs", "RegularHolidayOTHours", [optional]:=True),
        New ReportColumn("R.HolOT Pay", "RegularHolidayOTPay", [optional]:=True),
        New ReportColumn("Leave Hrs", "LeaveHours", [optional]:=True),
        New ReportColumn("Leave Pay", "LeavePay", [optional]:=True),
        New ReportColumn("Late Hrs", "LateHours", [optional]:=True),
        New ReportColumn("Late Amt", "LateDeduction", [optional]:=True),
        New ReportColumn("UT Hrs", "UndertimeHours", [optional]:=True),
        New ReportColumn("UT Amt", "UndertimeDeduction", [optional]:=True),
        New ReportColumn("Absent Hrs", "AbsentHours", [optional]:=True),
        New ReportColumn("Absent Amt", "AbsentDeduction", [optional]:=True),
        New ReportColumn("Allowance", "TotalAllowance"),
        New ReportColumn("Bonus", "TotalBonus", [optional]:=True),
        New ReportColumn("Gross", "GrossIncome"),
        New ReportColumn("SSS", "SSS", [optional]:=True),
        New ReportColumn("Ph.Health", "PhilHealth", [optional]:=True),
        New ReportColumn("HDMF", "HDMF", [optional]:=True),
        New ReportColumn("Taxable", "TaxableIncome"),
        New ReportColumn("W.Tax", "WithholdingTax"),
        New ReportColumn("Loan", "TotalLoans"),
        New ReportColumn("A.Fee", "AgencyFee", [optional]:=True),
        New ReportColumn(totalAdjustmentColumn, "TotalAdjustments", [optional]:=True),
        New ReportColumn("Net Pay", "NetPay"),
        New ReportColumn("13th Month", "13thMonthPay"),
        New ReportColumn("Total", "Total")
    })

    Private Const FontSize As Single = 8

    Private ReadOnly margin_size() As Decimal = New Decimal() {0.25D, 0.75D, 0.3D}

    Private Const EmployeeRowIDColumnName As String = "EmployeeRowID"

    Private _settings As ListOfValueCollection

    Public Property IsActual As Boolean

    Public Async Sub Run() Implements IReportProvider.Run
        Dim bool_result As Short = Convert.ToInt16(IsActual)

        Dim payrollSelector = New PayrollSummaDateSelection With {
            .ReportIndex = 6
        }

        If payrollSelector.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Using context As New PayrollContext

            _settings = New ListOfValueCollection(context.ListOfValues.ToList())

        End Using

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

            Dim defaultFileName As String =
                        String.Concat(orgNam,
                                      reportName, payrollSelector.cboStringParameter.Text.Replace(" ", ""), "Report",
                                      String.Concat(short_dates(0).Replace("/", "-"), "TO", short_dates(1).Replace("/", "-")),
                                      ".xlsx")

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

                    RenderWorksheet(worksheet, employeeGroups, short_dates, viewableReportColumns)
                Else
                    For Each employeeGroup In employeeGroups
                        Dim worksheet = excel.Workbook.Worksheets.Add(employeeGroup.DivisionName)

                        Dim currentGroup = New Collection(Of EmployeeGroup) From {
                            employeeGroup
                        }

                        RenderWorksheet(worksheet, currentGroup, short_dates, viewableReportColumns)
                    Next
                End If

                excel.Save()
            End Using

            Process.Start(newFile.FullName)
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try
    End Sub

    Private Sub RenderWorksheet(worksheet As ExcelWorksheet,
                                employeeGroups As ICollection(Of EmployeeGroup),
                                short_dates As String(),
                                viewableReportColumns As ICollection(Of ReportColumn))
        Dim subTotalRows = New List(Of Integer)

        worksheet.Cells.Style.Font.Size = FontSize

        Dim organizationCell = worksheet.Cells(1, 1)
        organizationCell.Value = orgNam.ToUpper()
        organizationCell.Style.Font.Bold = True

        Dim dateCell = worksheet.Cells(2, 1)
        Dim dateRange = $"For the period of {short_dates(0)} to {short_dates(1)})"
        dateCell.Value = dateRange

        Dim lastCell = String.Empty
        Dim rowIndex As Integer = 4

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

                Dim employeeId = employee(EmployeeRowIDColumnName)

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

            RenderSubTotal(worksheet, subTotalCellRange, employeesStartIndex, employeesLastIndex)

            rowIndex += 2
        Next

        worksheet.Cells.AutoFitColumns()
        worksheet.Cells("A1").AutoFitColumns(4.9, 5.3)

        rowIndex += 1

        If employeeGroups.Count > 1 Then
            RenderGrandTotal(worksheet, rowIndex, lastCell, subTotalRows)
        End If

        rowIndex += 1

        RenderSignatureFields(worksheet, rowIndex)
        SetDefaultPrinterSettings(worksheet.PrinterSettings)
    End Sub

    Private Function GetCellValue(employee As DataRow, sourceName As String) As Object
        If sourceName.EndsWith(adjustmentColumn) AndAlso GetPayrollSummaryPolicy() <> PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly Then

            If _adjustments Is Nothing Then Return 0

            Dim productName = GetAdjustmentColumnFromName(sourceName)
            Dim employeeId = ObjectUtils.ToNullableInteger(employee(EmployeeRowIDColumnName))

            Dim adjustment = _adjustments.
                    Where(Function(a) a.Product.PartNo = productName).
                    Where(Function(a) a.Paystub.EmployeeID.Value = employeeId.Value).
                    Sum(Function(a) a.PayAmount)

            Return adjustment

        End If

        Return employee(sourceName)
    End Function

    Private Async Function GetViewableReportColumns(allEmployees As ICollection(Of DataRow), hideEmptyColumns As Boolean, payrollSummaDateSelection As PayrollSummaDateSelection) As Task(Of ICollection(Of ReportColumn))

        Dim viewableReportColumns = New List(Of ReportColumn)
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

        If GetPayrollSummaryPolicy() <> PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly Then
            Await AddAdjustmentBreakdownColumns(allEmployees, viewableReportColumns, payrollSummaDateSelection)
        End If

        Return viewableReportColumns
    End Function

    Private Iterator Function GenerateAlphabet() As IEnumerable(Of String)
        Dim letter = "A"

        While True
            Yield letter

            Dim firstLetter = ""
            Dim currentLetter = ""

            Dim isMultiCharacter = letter.Length > 1
            If isMultiCharacter Then
                firstLetter = letter.Chars(0)
                currentLetter = letter.Chars(1)
            Else
                currentLetter = letter.Chars(0)
            End If

            Dim letterAsAscii = Asc(currentLetter)
            If letterAsAscii >= Asc("Z") Then
                If firstLetter = String.Empty Then
                    firstLetter = "A"
                Else
                    firstLetter = Chr(Asc(firstLetter) + 1)
                End If

                letter = $"{firstLetter}A"
            Else
                letter = $"{firstLetter}{Chr(letterAsAscii + 1)}"
            End If
        End While
    End Function

    Private Sub RenderColumnHeaders(worksheet As ExcelWorksheet, rowIndex As Integer, reportColum As ICollection(Of ReportColumn))
        Dim columnIndex As Integer = 1
        For Each column In reportColum
            Dim headerCell = worksheet.Cells(rowIndex, columnIndex)
            headerCell.Value = column.Name
            headerCell.Style.Font.Bold = True
            headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid
            headerCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217))

            columnIndex += 1
        Next
    End Sub

    Private Sub RenderSubTotal(worksheet As ExcelWorksheet, subTotalCellRange As String, employeesStartIndex As Integer, employeesLastIndex As Integer)
        worksheet.Cells(subTotalCellRange).Formula = String.Format(
                        "SUM({0})",
                        New ExcelAddress(employeesStartIndex, 3, employeesLastIndex, 3).Address)

        worksheet.Cells(subTotalCellRange).Style.Font.Bold = True
        worksheet.Cells(subTotalCellRange).Style.Numberformat.Format = "#,##0.00_);(#,##0.00)"
        worksheet.Cells(subTotalCellRange).Style.Border.Top.Style = ExcelBorderStyle.Thin
    End Sub

    Private Sub RenderGrandTotal(worksheet As ExcelWorksheet,
                                 rowIndex As Integer,
                                 lastCellColumn As String,
                                 subTotalRows As IEnumerable(Of Integer))
        Dim grandTotalRange = $"C{rowIndex}:{lastCellColumn}{rowIndex}"
        worksheet.Cells(grandTotalRange).Formula = String.Format("SUM({0})", String.Join(",", subTotalRows.Select(Function(s) $"C{s}")))
        worksheet.Cells(grandTotalRange).Style.Border.Top.Style = ExcelBorderStyle.Double
        worksheet.Cells(grandTotalRange).Style.Font.Bold = True
        worksheet.Cells(grandTotalRange).Style.Numberformat.Format = "#,##0.00_);(#,##0.00)"
    End Sub

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

    Private Sub SetDefaultPrinterSettings(settings As ExcelPrinterSettings)
        With settings
            .Orientation = eOrientation.Landscape
            .PaperSize = ePaperSize.Legal
            .TopMargin = margin_size(1)
            .BottomMargin = margin_size(1)
            .LeftMargin = margin_size(0)
            .RightMargin = margin_size(0)
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

#Region "Adjustment Breakdown"

    Private _adjustmentQueried As Boolean
    Private _adjustments As List(Of IAdjustment)

    Private Function GetPayrollSummaryPolicy() As PayrollSummaryAdjustmentBreakdownPolicy
        Return _settings.GetEnum("Payroll Summary Policy.AdjustmentBreakdown", PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly)
    End Function

    Private Async Function AddAdjustmentBreakdownColumns(allEmployees As ICollection(Of DataRow), viewableReportColumns As List(Of ReportColumn), payrollSummaDateSelection As PayrollSummaDateSelection) As Task

        Dim adjustments = Await GetCurrentAdjustments(payrollSummaDateSelection, allEmployees)

        Dim groupedAdjustments = adjustments.GroupBy(Function(a) a.ProductID).ToList

        Dim totalAdjustmentReportColumn = viewableReportColumns.Where(Function(r) r.Name = totalAdjustmentColumn).FirstOrDefault
        Dim totalAdjustmentColumnIndex = viewableReportColumns.IndexOf(totalAdjustmentReportColumn)

        Dim counter = 1

        'add breakdown columns
        For Each adjustment In groupedAdjustments

            Dim adjustmentName = GetAdjustmentName(adjustment(0).Product?.Name)

            If String.IsNullOrWhiteSpace(adjustmentName) Then Continue For

            viewableReportColumns.Insert(totalAdjustmentColumnIndex + counter, New ReportColumn(adjustmentName, adjustmentName))

            counter += 1
        Next

        'remove total adjustments column
        viewableReportColumns.RemoveAt(totalAdjustmentColumnIndex)

        'add back the total adjustment column if it is not BreakdownOnly
        'this will put the column after the adjustment breakdown columns
        If GetPayrollSummaryPolicy() <> PayrollSummaryAdjustmentBreakdownPolicy.BreakdownOnly Then

            If GetPayrollSummaryPolicy() = PayrollSummaryAdjustmentBreakdownPolicy.Both Then

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

        If _adjustmentQueried AndAlso _adjustments IsNot Nothing Then

            Return _adjustments

        End If

        Using context As New PayrollContext

            Dim payPeriodFrom As New PayPeriod
            Dim payPeriodTo As New PayPeriod

            If payrollSummaDateSelection.PayPeriodFromID IsNot Nothing Then

                payPeriodFrom = Await context.PayPeriods.
                                Where(Function(p) p.RowID.Value = payrollSummaDateSelection.PayPeriodFromID.Value).
                                FirstOrDefaultAsync
            End If

            If payrollSummaDateSelection.PayPeriodToID IsNot Nothing Then

                payPeriodTo = Await context.PayPeriods.
                                Where(Function(p) p.RowID.Value = payrollSummaDateSelection.PayPeriodToID.Value).
                                FirstOrDefaultAsync
            End If

            If payPeriodFrom?.PayFromDate Is Nothing OrElse payPeriodTo?.PayToDate Is Nothing Then

                Throw New ArgumentException("Cannot fetch pay period data.")

            End If

            Dim employeeIds = GetEmployeeIds(allEmployees)

            Dim adjustmentQuery = GetBaseAdjustmentQuery(context.Adjustments.Where(Function(a) a.OrganizationID.Value = z_OrganizationID), payPeriodFrom.PayFromDate, payPeriodTo.PayToDate, employeeIds)
            Dim actualAdjustmentQuery = GetBaseAdjustmentQuery(context.ActualAdjustments.Where(Function(a) a.OrganizationID.Value = z_OrganizationID), payPeriodFrom.PayFromDate, payPeriodTo.PayToDate, employeeIds)

            If allEmployees Is Nothing Then

                adjustmentQuery.Where(Function(p) employeeIds.Contains(p.Paystub.EmployeeID))
                actualAdjustmentQuery.Where(Function(p) employeeIds.Contains(p.Paystub.EmployeeID))

            End If

            _adjustmentQueried = True

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

    Private Class ReportColumn

        Public Property Name As String
        Public Property Type As ColumnType
        Public Property Source As String
        Public Property [Optional] As Boolean

        Public Sub New(name As String,
                       source As String,
                       Optional type As ColumnType = ColumnType.Numeric,
                       Optional [optional] As Boolean = False)
            Me.Name = name
            Me.Source = source
            Me.Type = type
            Me.Optional = [optional]
        End Sub

    End Class

    Private Enum ColumnType
        Text
        Numeric
    End Enum

End Class

Friend Enum SalaryActualization As Short
    Declared = 0
    Actual = 1
End Enum

Friend Enum ExcelOption As Short
    SeparateEachDepartment = 0
    KeepAllInOneSheet = 1
End Enum