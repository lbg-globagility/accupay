Imports System.Collections.ObjectModel
Imports System.IO
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class PayrollLedgerExcelFormatReportProvider
    Implements IReportProvider

    Private fromPeriodId, toPeriodId As Integer
    Private actualSwitch As Boolean
    Private dateFrom, dateTo As Date

    Public Property Name As String = "Payroll Ledger" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private basic_alphabet() As String =
        New String() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                      "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT"}

    Private ReadOnly reportColumns As IReadOnlyCollection(Of ReportColumn) = New ReadOnlyCollection(Of ReportColumn)({
        New ReportColumn("FROM", "From", ColumnType.Text),
        New ReportColumn("TO", "To", ColumnType.Text),
        New ReportColumn("Rate", "Rate"),
        New ReportColumn("Basic Pay", "BasicPay"),
        New ReportColumn("Reg Hrs", "RegularHours"),
        New ReportColumn("Reg Pay", "RegularPay"),
        New ReportColumn("OT Hrs", "OvertimeHours"),
        New ReportColumn("OT Pay", "OvertimePay"),
        New ReportColumn("ND Hrs", "NightDiffHours"),
        New ReportColumn("ND Pay", "NightDiffPay"),
        New ReportColumn("NDOT Hrs", "NightDiffOvertimeHours"),
        New ReportColumn("NDOT Pay", "NightDiffOvertimePay"),
        New ReportColumn("R.Day Hrs", "RestDayHours"),
        New ReportColumn("R.Day Pay", "RestDayPay"),
        New ReportColumn("R.DayOT Hrs", "RestDayOTHours"),
        New ReportColumn("R.DayOT Pay", "RestDayOTPay"),
        New ReportColumn("S.Hol Hrs", "SpecialHolidayHours"),
        New ReportColumn("S.Hol Pay", "SpecialHolidayPay"),
        New ReportColumn("S.HolOT Hrs", "SpecialHolidayOTHours"),
        New ReportColumn("S.HolOT Pay", "SpecialHolidayOTPay"),
        New ReportColumn("R.Hol Hrs", "RegularHolidayHours"),
        New ReportColumn("R.Hol Pay", "RegularHolidayPay"),
        New ReportColumn("R.HolOT Hrs", "RegularHolidayOTHours"),
        New ReportColumn("R.HolOT Pay", "RegularHolidayOTPay"),
        New ReportColumn("Leave Hrs", "LeaveHours"),
        New ReportColumn("Leave Pay", "LeavePay"),
        New ReportColumn("Late Hrs", "LateHours"),
        New ReportColumn("Late Amt", "LateDeduction"),
        New ReportColumn("UT Hrs", "UndertimeHours"),
        New ReportColumn("UT Amt", "UndertimeDeduction"),
        New ReportColumn("Absent Hrs", "AbsentHours"),
        New ReportColumn("Absent Amt", "AbsentDeduction"),
        New ReportColumn("Allowance", "TotalAllowance"),
        New ReportColumn("Bonus", "TotalBonus"),
        New ReportColumn("Gross", "GrossIncome"),
        New ReportColumn("SSS", "SSS"),
        New ReportColumn("Ph.Health", "PhilHealth"),
        New ReportColumn("HDMF", "HDMF"),
        New ReportColumn("Taxable", "TaxableIncome"),
        New ReportColumn("W.Tax", "WithholdingTax"),
        New ReportColumn("Loan", "TotalLoans"),
        New ReportColumn("A.Fee", "AgencyFee"),
        New ReportColumn("Adj.", "TotalAdjustments"),
        New ReportColumn("Net", "NetPay"),
        New ReportColumn("13th Month", "13thMonthPay"),
        New ReportColumn("Total", "Total")
    })

    Private ReadOnly preferred_font As Font = New Font(
        "Source Sans Pro",
        8.25!,
        FontStyle.Regular,
        GraphicsUnit.Point,
        CType(0, Byte))

    Private Const FontSize As Single = 8

    Private ReadOnly margin_size() As Decimal = New Decimal() {0.25D, 0.75D, 0.3D}

    Public Property IsActual As Boolean

    Public Sub New()

    End Sub

    Public Sub New(FromPayPeriodId As Integer, ToPayPeriodId As Integer, IsActual As Boolean, PayDateFrom As Date, PayDateTo As Date)
        fromPeriodId = FromPayPeriodId
        toPeriodId = ToPayPeriodId

        actualSwitch = IsActual

        dateFrom = PayDateFrom
        dateTo = PayDateTo
    End Sub

    Private Function ParameterAssignment() As Boolean
        Dim boolResult As Boolean = True

        Dim periodSelector As New PayrollSummaDateSelection()

        periodSelector.Panel3.Visible = False
        periodSelector.panelSalarySwitch.Visible = True
        periodSelector.Label5.Visible = False

        If periodSelector.ShowDialog = DialogResult.OK Then
            fromPeriodId = periodSelector.PayPeriodFromID
            toPeriodId = periodSelector.PayPeriodToID

            actualSwitch = periodSelector.IsActual

            dateFrom = periodSelector.DateFrom
            dateTo = periodSelector.DateTo
        Else
            boolResult = False
        End If

        Return boolResult
    End Function

    Public Sub Run() Implements IReportProvider.Run
        Static last_cell_column As String = basic_alphabet.Last

        If ParameterAssignment() = False Then
            Return
        End If

        Dim parameters =
                New Object() {orgztnID,
                              fromPeriodId,
                              toPeriodId,
                              actualSwitch,
                              DBNull.Value,
                              True}

        Dim sql_print_employee_profiles As New SQL(
            "CALL PAYROLLSUMMARY2(?og_rowid, ?min_pp_rowid, ?max_pp_rowid, ?is_actual, ?salaray_distrib, ?keep_in_onesheet);",
            parameters)

        Try
            Dim ds = sql_print_employee_profiles.GetFoundRows

            If sql_print_employee_profiles.HasError Then
                Throw sql_print_employee_profiles.ErrorException
            End If

            Static report_name As String = "PayrollLedger"
            Static temp_path As String = Path.GetTempPath()

            Dim short_dates() As String = New String() {
                CDate(dateFrom).ToShortDateString,
                CDate(dateTo).ToShortDateString}

            Dim temp_file As String =
                        String.Concat(temp_path,
                                      orgNam,
                                      "Report",
                                      String.Concat(short_dates(0).Replace("/", "-"), "TO", short_dates(1).Replace("/", "-")),
                                      ".xlsx")

            Dim date_range As String = String.Concat("For the period of ", short_dates(0), " to ", short_dates(1))

            Dim newFile = New FileInfo(temp_file)

            If newFile.Exists Then
                newFile.Delete()
                newFile = New FileInfo(temp_file)
            End If

            'Dim divisionsWithEmployees = ds.Tables.OfType(Of DataTable).Where(Function(dt) dt.Rows.Count > 0).FirstOrDefault
            Dim divisionsWithEmployees = ds.Tables.OfType(Of DataTable).FirstOrDefault

            Using excel = New ExcelPackage(newFile)
                Dim subTotalRows = New List(Of Integer)

                Dim worksheet = excel.Workbook.Worksheets.Add(report_name)
                worksheet.Cells.Style.Font.Size = FontSize

                Dim organizationCell = worksheet.Cells(1, 1)
                organizationCell.Value = orgNam.ToUpper
                organizationCell.Style.Font.Bold = True

                Dim dateCell = worksheet.Cells(2, 1)
                dateCell.Value = date_range

                Dim rowIndex As Integer = 3

                Dim empoyeeNo = "0"

                Dim rows = divisionsWithEmployees.Rows

                Dim employeeList = divisionsWithEmployees.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("DatCol2")).[Select](Function(grp) grp.First())

                For Each employeesInDivision As DataRow In employeeList

                    RenderColumnHeaders(worksheet, rowIndex)

                    rowIndex += 1

                    empoyeeNo = employeesInDivision("DatCol2").ToString

                    Dim payrolls = rows.OfType(Of DataRow).Where(Function(e) Equals(empoyeeNo, e("DatCol2")))
                    Dim firstEmployee = payrolls.FirstOrDefault()

                    Dim employeeNoCell = worksheet.Cells(rowIndex, 2)
                    Dim employeeNoCellHeader = worksheet.Cells(rowIndex, 1)
                    employeeNoCellHeader.Value = "EMPLOYEE ID"

                    employeeNoCell.Value = empoyeeNo
                    employeeNoCellHeader.Style.Font.Bold = True

                    rowIndex += 1

                    Dim empoyeeFullName = firstEmployee("DatCol3").ToString

                    Dim employeeNameCell = worksheet.Cells(rowIndex, 2)
                    Dim employeeNameCellHeader = worksheet.Cells(rowIndex, 1)
                    If empoyeeFullName.Length > 0 Then
                        employeeNameCell.Value = empoyeeFullName
                    End If
                    employeeNameCellHeader.Value = "EMPLOYEE NAME"
                    employeeNameCellHeader.Style.Font.Bold = True

                    rowIndex += 1

                    Dim employeesStartIndex = rowIndex
                    Dim employeesLastIndex = 0

                    For Each employeeRow As DataRow In payrolls
                        Dim letters = GenerateAlphabet.GetEnumerator()

                        For Each reportColumn In reportColumns
                            letters.MoveNext()
                            Dim alphabet = letters.Current

                            Dim column = $"{alphabet}{rowIndex}"

                            Dim cell = worksheet.Cells(column)
                            Dim sourceName = reportColumn.SourceName
                            cell.Value = employeeRow(sourceName)

                            If reportColumn.Type = ColumnType.Numeric Then
                                cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)"
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                            End If
                        Next

                        employeesLastIndex = rowIndex
                        rowIndex += 1
                    Next

                    Dim subTotalCellRange = String.Join(
                        ":",
                        String.Concat("C", rowIndex),
                        String.Concat(last_cell_column, rowIndex))

                    subTotalRows.Add(rowIndex)

                    RenderSubTotal(worksheet, subTotalCellRange, employeesStartIndex, employeesLastIndex)

                    rowIndex += 2
                Next

                worksheet.Cells.AutoFitColumns()
                worksheet.Cells("A1").AutoFitColumns(4.9, 12.28)

                rowIndex += 1

                RenderGrandTotal(worksheet, rowIndex, last_cell_column, subTotalRows)

                rowIndex += 1

                RenderSignatureFields(worksheet, rowIndex)
                SetDefaultPrinterSettings(worksheet.PrinterSettings)

                excel.Save()
            End Using

            If divisionsWithEmployees.Rows.Count > 0 Then
                Process.Start(temp_file)
            Else
                MsgBox("No found record(s)", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try
    End Sub

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
                If firstLetter = "" Then
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

    Private Sub RenderColumnHeaders(worksheet As ExcelWorksheet, rowIndex As Integer)
        Dim columnIndex As Integer = 1
        For Each column In reportColumns
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

    Private Sub RenderGrandTotal(worksheet As ExcelWorksheet, rowIndex As Integer, last_cell_column As String, subTotalRows As IEnumerable(Of Integer))
        Dim grandTotalRange = String.Join(":", String.Concat("C", rowIndex), String.Concat(last_cell_column, rowIndex))
        worksheet.Cells(grandTotalRange).Formula = String.Format("SUM({0})", String.Join(",", subTotalRows.Select(Function(s) $"C{s}")))
        worksheet.Cells(grandTotalRange).Style.Border.Top.Style = ExcelBorderStyle.Double
        worksheet.Cells(grandTotalRange).Style.Font.Bold = True
        worksheet.Cells(grandTotalRange).Style.Numberformat.Format = "#,##0.00_);(#,##0.00)"
    End Sub

    Private Sub RenderSignatureFields(worksheet As ExcelWorksheet, startIdx As Integer)
        Dim signatur_field_index As Integer = (startIdx + 1)

        With worksheet
            .Cells(String.Concat("A", signatur_field_index)).Value = "Prepared by: "
            .Cells(String.Join(":",
                                   String.Concat("A", signatur_field_index),
                                   String.Concat("B", signatur_field_index))).Merge = True

            signatur_field_index += 1
            .Cells(String.Concat("A", signatur_field_index)).Value = "Audited by: "
            .Cells(String.Join(":",
                                   String.Concat("A", signatur_field_index),
                                   String.Concat("B", signatur_field_index))).Merge = True

            signatur_field_index += 1
            .Cells(String.Concat("A", signatur_field_index)).Value = "Approved by: "
            .Cells(String.Join(":",
                                   String.Concat("A", signatur_field_index),
                                   String.Concat("B", signatur_field_index))).Merge = True
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

    Private Function NewLiner(Optional repetition As Integer = 1) As String

        Dim _result As String = String.Empty

        Dim i = 0

        While i < repetition

            _result =
                String.Concat(_result, Environment.NewLine)

            i += 1

        End While

        Return _result

    End Function

    Private Class ReportColumn

        Public Sub New(name As String, source As String, Optional type As ColumnType = ColumnType.Numeric)
            Me.Name = name
            Me.SourceName = source
            Me.Type = type
        End Sub

        Public Property Name As String
        Public Property Type As ColumnType
        Public Property SourceName As String
    End Class

    Private Enum ColumnType
        Text
        Numeric
    End Enum

    Private Enum SalaryActualization As Short
        Declared = 0
        Actual = 1
    End Enum

End Class