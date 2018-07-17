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

    Private ReadOnly columns As IReadOnlyCollection(Of Column) = New ReadOnlyCollection(Of Column)({
        New Column("Code", "DatCol2", ColumnType.Text),
        New Column("Full Name", "DatCol3", ColumnType.Text),
        New Column("Rate", "Rate"),
        New Column("Basic Pay", "BasicPay"),
        New Column("Reg Hrs", "RegularHours"),
        New Column("Reg Pay", "RegularPay"),
        New Column("OT Hrs", "OvertimeHours"),
        New Column("OT Pay", "OvertimePay"),
        New Column("ND Hrs", "NightDiffHours"),
        New Column("ND Pay", "NightDiffPay"),
        New Column("NDOT Hrs", "NightDiffOvertimeHours"),
        New Column("NDOT Pay", "NightDiffOvertimePay"),
        New Column("R.Day Hrs", "RestDayHours"),
        New Column("R.Day Pay", "RestDayPay"),
        New Column("R.DayOT Hrs", "RestDayOTHours"),
        New Column("R.DayOT Pay", "RestDayOTPay"),
        New Column("S.Hol Hrs", "SpecialHolidayHours"),
        New Column("S.Hol Pay", "SpecialHolidayPay"),
        New Column("S.HolOT Hrs", "SpecialHolidayOTHours"),
        New Column("S.HolOT Pay", "SpecialHolidayOTPay"),
        New Column("R.Hol Hrs", "RegularHolidayHours"),
        New Column("R.Hol Pay", "RegularHolidayPay"),
        New Column("R.HolOT Hrs", "RegularHolidayOTHours"),
        New Column("R.HolOT Pay", "RegularHolidayOTPay"),
        New Column("Leave Hrs", "LeaveHours"),
        New Column("Leave Pay", "LeavePay"),
        New Column("Late Hrs", "LateHours"),
        New Column("Late Amt", "LateDeduction"),
        New Column("UT Hrs", "UndertimeHours"),
        New Column("UT Amt", "UndertimeDeduction"),
        New Column("Absent Hrs", "AbsentHours"),
        New Column("Absent Amt", "AbsentDeduction"),
        New Column("Allowance", "TotalAllowance"),
        New Column("Bonus", "TotalBonus"),
        New Column("Gross", "GrossIncome"),
        New Column("SSS", "SSS"),
        New Column("Ph.Health", "PhilHealth"),
        New Column("HDMF", "HDMF"),
        New Column("Taxable", "TaxableIncome"),
        New Column("W.Tax", "WithholdingTax"),
        New Column("Loan", "TotalLoans"),
        New Column("A.Fee", "AgencyFee"),
        New Column("Adj.", "TotalAdjustments"),
        New Column("Net", "NetPay"),
        New Column("13th Month", "13thMonthPay"),
        New Column("Total", "Total")
    })

    Private cell_mapped_text_value() As String =
        New String() {"DatCol2",
                      "DatCol3"}

    Private cell_mapped_decim_value() As String =
        New String() {"Rate",
                      "BasicPay",
                      "RegularHours",
                      "RegularPay",
                      "OvertimeHours",
                      "OvertimePay",
                      "NightDiffHours",
                      "NightDiffPay",
                      "NightDiffOvertimeHours",
                      "NightDiffOvertimePay",
                      "RestDayHours",
                      "RestDayPay",
                      "RestDayOTHours",
                      "RestDayOTPay",
                      "SpecialHolidayHours",
                      "SpecialHolidayPay",
                      "SpecialHolidayOTHours",
                      "SpecialHolidayOTPay",
                      "RegularHolidayHours",
                      "RegularHolidayPay",
                      "RegularHolidayOTHours",
                      "RegularHolidayOTPay",
                      "LeaveHours",
                      "LeavePay",
                      "LateHours",
                      "LateDeduction",
                      "UndertimeHours",
                      "UndertimeDeduction",
                      "AbsentHours",
                      "AbsentDeduction",
                      "TotalAllowance",
                      "TotalBonus",
                      "GrossIncome",
                      "SSS",
                      "PhilHealth",
                      "HDMF",
                      "TaxableIncome",
                      "WithholdingTax",
                      "TotalLoans",
                      "AgencyFee",
                      "TotalAdjustments",
                      "NetPay",
                      "13thMonthPay",
                      "Total"}

    Private ReadOnly preferred_font As Font = New System.Drawing.Font(
        "Source Sans Pro",
        8.25!,
        System.Drawing.FontStyle.Regular,
        System.Drawing.GraphicsUnit.Point,
        CType(0, Byte))

    Private Const FontSize As Single = 8

    Private ReadOnly margin_size() As Decimal = New Decimal() {0.25D, 0.75D, 0.3D}

    Private sys_ownr As New SystemOwner

    Private Property IsActual As Boolean

    Public Sub Run() Implements IReportProvider.Run
        Dim is_goldwings As Boolean = (sys_ownr.CurrentSystemOwner = SystemOwner.Goldwings)

        Static last_cell_column As String = basic_alphabet.Last

        Dim bool_result As Short = Convert.ToInt16(IsActual)

        Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

        n_PayrollSummaDateSelection.ReportIndex = 6

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

                    Dim row_indx As Integer = 5
                    Dim col_index As Integer = one_value

                    For Each column In columns
                        Dim headerCell = worksheet.Cells(row_indx, col_index)
                        headerCell.Value = column.Name
                        headerCell.Style.Font.Bold = True

                        col_index += 1
                    Next

                    row_indx += one_value

                    Dim details_start_rowindex = row_indx
                    Dim details_last_rowindex = 0

                    For Each employeeRow As DataRow In employeeTable.Rows
                        Dim cell3 = worksheet.Cells(3, one_value)
                        Dim division_name = employeeRow("DatCol1").ToString

                        If division_name.Length > 0 Then
                            cell3.Value = String.Concat("Division: ", division_name)

                            worksheet.Name = division_name
                        End If

                        Dim row_array = employeeRow.ItemArray

                        Dim i = 0
                        For Each cell_val As String In cell_mapped_text_value
                            Dim excl_colrow = String.Concat(basic_alphabet(i), row_indx)

                            Dim _cells = worksheet.Cells(excl_colrow)
                            _cells.Value = employeeRow(cell_val)

                            i += one_value
                        Next

                        For Each cell_val As String In cell_mapped_decim_value
                            Dim excl_colrow = String.Concat(basic_alphabet(i), row_indx)

                            Dim _cells = worksheet.Cells(excl_colrow)
                            _cells.Value = employeeRow(cell_val)
                            _cells.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)"
                            _cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right

                            i += one_value
                        Next

                        details_last_rowindex = row_indx
                        row_indx += one_value
                    Next

                    Dim sum_cell_range = String.Join(
                        ":",
                        String.Concat("C", row_indx),
                        String.Concat(last_cell_column, row_indx))

                    worksheet.Cells(sum_cell_range).Formula = String.Format(
                        "SUM({0})",
                        New ExcelAddress(
                            details_start_rowindex, 3,
                            details_last_rowindex, 3).Address)

                    worksheet.Cells(sum_cell_range).Style.Font.Bold = True
                    worksheet.Cells(sum_cell_range).Style.Numberformat.Format = "#,##0.00_);(#,##0.00)"

                    If is_goldwings Then
                        RenderSignatureFields(worksheet, row_indx)
                    End If

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

    Private Class Column
        Public Sub New(name As String, source As String, Optional type As ColumnType = Nothing)
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
