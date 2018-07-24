Option Strict On
Imports System.Collections.ObjectModel
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class PayrollSummaryExcelFormatReportProvider
    Implements IReportProvider

    Public Property Name As String = "" Implements IReportProvider.Name

    Private basic_alphabet() As String =
        New String() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                      "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT"}

    Private ReadOnly reportColumns As IReadOnlyCollection(Of ReportColumn) = New ReadOnlyCollection(Of ReportColumn)({
        New ReportColumn("Code", "DatCol2", ColumnType.Text),
        New ReportColumn("Full Name", "DatCol3", ColumnType.Text),
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

    Private ReadOnly preferred_font As Font = New System.Drawing.Font(
        "Source Sans Pro",
        8.25!,
        System.Drawing.FontStyle.Regular,
        System.Drawing.GraphicsUnit.Point,
        CType(0, Byte))

    Private Const FontSize As Single = 8

    Private ReadOnly margin_size() As Decimal = New Decimal() {0.25D, 0.75D, 0.3D}

    Private sys_ownr As New SystemOwner

    Public Property IsActual As Boolean

    Public Sub Run() Implements IReportProvider.Run
        Dim is_goldwings As Boolean = (sys_ownr.CurrentSystemOwner = SystemOwner.Goldwings)

        Static last_cell_column As String = basic_alphabet.Last

        Dim bool_result As Short = Convert.ToInt16(IsActual)

        Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection With {
            .ReportIndex = 6
        }

        If n_PayrollSummaDateSelection.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim excel_custom_format = Convert.ToBoolean(ExcelOptionFormat())

        Dim parameters =
                New Object() {orgztnID,
                              n_PayrollSummaDateSelection.PayPeriodFromID,
                              n_PayrollSummaDateSelection.PayPeriodToID,
                              bool_result,
                              n_PayrollSummaDateSelection.cboStringParameter.Text,
                              excel_custom_format}

        Dim sql_print_employee_profiles As New SQL(
            "CALL PAYROLLSUMMARY2(?og_rowid, ?min_pp_rowid, ?max_pp_rowid, ?is_actual, ?salaray_distrib, ?keep_in_onesheet);",
            parameters)

        Static one_value As Integer = 1

        Try
            Dim ds = sql_print_employee_profiles.GetFoundRows

            If sql_print_employee_profiles.HasError Then
                Throw sql_print_employee_profiles.ErrorException
            End If

            Static report_name As String = "PayrollSummary"
            Static temp_path As String = Path.GetTempPath()

            Dim short_dates() As String = New String() {
                CDate(n_PayrollSummaDateSelection.DateFrom).ToShortDateString,
                CDate(n_PayrollSummaDateSelection.DateTo).ToShortDateString}

            Dim temp_file As String =
                        String.Concat(temp_path,
                                      orgNam,
                                      report_name, n_PayrollSummaDateSelection.cboStringParameter.Text.Replace(" ", ""), "Report",
                                      String.Concat(short_dates(0).Replace("/", "-"), "TO", short_dates(1).Replace("/", "-")),
                                      ".xlsx")

            Dim date_range As String = String.Concat("For the period of ", short_dates(0), " to ", short_dates(1))

            Dim newFile = New FileInfo(temp_file)

            If newFile.Exists Then
                newFile.Delete()
                newFile = New FileInfo(temp_file)
            End If

            Dim tbl_withrows = ds.Tables.OfType(Of DataTable).Where(Function(dt) dt.Rows.Count > 0)

            Using excel = New ExcelPackage(newFile)
                Dim divisionNo = 0

                For Each employeeTable As DataTable In tbl_withrows
                    Dim worksheet = excel.Workbook.Worksheets.Add(String.Concat(report_name, divisionNo))

                    worksheet.Cells.Style.Font.Size = FontSize

                    Dim organizationCell = worksheet.Cells(1, one_value)
                    organizationCell.Value = orgNam.ToUpper
                    organizationCell.Style.Font.Bold = True

                    Dim dateCell = worksheet.Cells(2, one_value)
                    dateCell.Value = date_range

                    Dim rowIndex As Integer = 5
                    Dim columnIndex As Integer = one_value

                    For Each column In reportColumns
                        Dim headerCell = worksheet.Cells(rowIndex, columnIndex)
                        headerCell.Value = column.Name
                        headerCell.Style.Font.Bold = True

                        columnIndex += 1
                    Next

                    rowIndex += one_value

                    Dim details_start_rowindex = rowIndex
                    Dim details_last_rowindex = 0

                    For Each employeeRow As DataRow In employeeTable.Rows
                        Dim cell3 = worksheet.Cells(3, one_value)
                        Dim division_name = employeeRow("DatCol1").ToString

                        If division_name.Length > 0 Then
                            cell3.Value = String.Concat("Division: ", division_name)

                            worksheet.Name = division_name
                        End If

                        Dim row_array = employeeRow.ItemArray

                        Dim letters = GenerateAlphabet.GetEnumerator()
                        For Each reportColumn In reportColumns.Zip(basic_alphabet, Function(c, a) New With {.Column = c, .Alphabet = a})
                            letters.MoveNext()
                            Dim alphabet = letters.Current

                            Dim column = $"{alphabet}{rowIndex}"

                            Dim cell = worksheet.Cells(column)
                            Dim sourceName = reportColumn.Column.SourceName
                            cell.Value = employeeRow(sourceName)

                            If reportColumn.Column.Type = ColumnType.Numeric Then
                                cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)"
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                            End If
                        Next

                        details_last_rowindex = rowIndex
                        rowIndex += one_value
                    Next

                    Dim sum_cell_range = String.Join(
                        ":",
                        String.Concat("C", rowIndex),
                        String.Concat(last_cell_column, rowIndex))

                    worksheet.Cells(sum_cell_range).Formula = String.Format(
                        "SUM({0})",
                        New ExcelAddress(
                            details_start_rowindex, 3,
                            details_last_rowindex, 3).Address)

                    worksheet.Cells(sum_cell_range).Style.Font.Bold = True
                    worksheet.Cells(sum_cell_range).Style.Numberformat.Format = "#,##0.00_);(#,##0.00)"

                    RenderSignatureFields(worksheet, rowIndex)
                    SetDefaultPrinterSettings(worksheet.PrinterSettings)

                    worksheet.Cells.AutoFitColumns()
                    worksheet.Cells("A1").AutoFitColumns(4.9, 5.3)

                    divisionNo += 1
                Next

                excel.Save()
            End Using

            If tbl_withrows.Count > 0 Then
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

End Class

Friend Enum SalaryActualization As Short
    Declared = 0
    Actual = 1
End Enum

Friend Enum ExcelOption As Short
    SeparateEachDepartment = 0
    KeepAllInOneSheet = 1
End Enum
