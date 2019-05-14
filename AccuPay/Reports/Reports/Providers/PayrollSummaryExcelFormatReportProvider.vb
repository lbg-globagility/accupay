Option Strict On

Imports System.Collections.ObjectModel
Imports System.IO
Imports AccuPay.Helpers
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class PayrollSummaryExcelFormatReportProvider
    Implements IReportProvider

    Public Property Name As String = "" Implements IReportProvider.Name

    Private basic_alphabet() As String =
        New String() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                      "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT"}

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
        New ReportColumn("Adj.", "TotalAdjustments", [optional]:=True),
        New ReportColumn("Net Pay", "NetPay"),
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

    Public Sub Run() Implements IReportProvider.Run
        Dim bool_result As Short = Convert.ToInt16(IsActual)

        Dim payrollSelector = New PayrollSummaDateSelection With {
            .ReportIndex = 6
        }

        If payrollSelector.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim keepInOneSheet = Convert.ToBoolean(ExcelOptionFormat())

        Dim parameters = New Object() {
            orgztnID,
            payrollSelector.PayPeriodFromID,
            payrollSelector.PayPeriodToID,
            bool_result,
            payrollSelector.cboStringParameter.Text,
            keepInOneSheet
        }

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



            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName)

            If saveFileDialogHelperOutPut.IsSuccess = False Then Return

            Dim newFile = saveFileDialogHelperOutPut.FileInfo

            Dim allEmployeesByDivision As IEnumerable(Of IGrouping(Of String, DataRow))

            Dim viewableReportColumns = New List(Of ReportColumn)
            For Each reportColumn In _reportColumns
                If reportColumn.Optional Then
                    Dim hasValue = allEmployees.
                        Any(Function(row) Not IsDBNull(row(reportColumn.Source)) And Not CDbl(row(reportColumn.Source)) = 0)

                    If hasValue Then
                        viewableReportColumns.Add(reportColumn)
                    End If
                Else
                    viewableReportColumns.Add(reportColumn)
                End If
            Next

            If keepInOneSheet Then
                allEmployeesByDivision = allEmployees.
                GroupBy(Function(r) String.Empty)
            Else
                allEmployeesByDivision = allEmployees.
                GroupBy(Function(r) r("DivisionID").ToString())
            End If

            Using excel = New ExcelPackage(newFile)
                Dim subTotalRows = New List(Of Integer)

                Dim worksheet = excel.Workbook.Worksheets.Add(reportName)
                worksheet.Cells.Style.Font.Size = FontSize

                Dim organizationCell = worksheet.Cells(1, 1)
                organizationCell.Value = orgNam.ToUpper
                organizationCell.Style.Font.Bold = True

                Dim dateCell = worksheet.Cells(2, 1)
                Dim dateRange = $"For the period of {short_dates(0)} to {short_dates(1)})"
                dateCell.Value = dateRange

                Dim lastCell = String.Empty
                Dim rowIndex As Integer = 4

                For Each employeesInDivision In allEmployeesByDivision
                    Dim divisionCell = worksheet.Cells(rowIndex, 1)
                    Dim firstEmployee = employeesInDivision.FirstOrDefault()
                    Dim divisionName = firstEmployee("DatCol1").ToString
                    If divisionName.Length > 0 Then
                        divisionCell.Value = divisionName
                        divisionCell.Style.Font.Italic = True
                    End If

                    rowIndex += 1

                    RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns)

                    rowIndex += 1

                    Dim employeesStartIndex = rowIndex
                    Dim employeesLastIndex = 0

                    For Each employeeRow In employeesInDivision
                        Dim letters = GenerateAlphabet.GetEnumerator()

                        For Each reportColumn In viewableReportColumns
                            letters.MoveNext()
                            Dim alphabet = letters.Current

                            Dim column = $"{alphabet}{rowIndex}"

                            Dim cell = worksheet.Cells(column)
                            Dim sourceName = reportColumn.Source
                            cell.Value = employeeRow(sourceName)

                            If reportColumn.Type = ColumnType.Numeric Then
                                cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)"
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                            End If
                        Next

                        lastCell = letters.Current

                        employeesLastIndex = rowIndex
                        rowIndex += 1
                    Next

                    Dim subTotalCellRange = String.Join(
                        ":",
                        String.Concat("C", rowIndex),
                        String.Concat(lastCell, rowIndex))

                    subTotalRows.Add(rowIndex)

                    RenderSubTotal(worksheet, subTotalCellRange, employeesStartIndex, employeesLastIndex)

                    rowIndex += 2
                Next

                worksheet.Cells.AutoFitColumns()
                worksheet.Cells("A1").AutoFitColumns(4.9, 5.3)

                rowIndex += 1

                RenderGrandTotal(worksheet, rowIndex, lastCell, subTotalRows)

                rowIndex += 1

                RenderSignatureFields(worksheet, rowIndex)
                SetDefaultPrinterSettings(worksheet.PrinterSettings)

                excel.Save()
            End Using

            Process.Start(newFile.FullName)
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

            _result =
                String.Concat(_result, Environment.NewLine)

            i += 1

        End While

        Return _result

    End Function

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
