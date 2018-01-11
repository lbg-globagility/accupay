Option Strict On
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class PayrollSummaryExcelFormatReportProvider
    Implements IReportProvider

    Private basic_alphabet() As String =
        New String() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                      "AA", "AB", "AC", "AD"}

    Private column_headers() As String =
        New String() {"Code",
                      "Full name",
                      "Rate",
                      "BasicPay",
                      "Hrs",
                      "Reg Pay",
                      "OTHrs",
                      "OT",
                      "Holiday",
                      "NDiff",
                      "NDiff OT",
                      "R.dayPay",
                      "Leave Pay",
                      "UT",
                      "Late",
                      "Absent",
                      "Allowance",
                      "Bonus",
                      "Gross",
                      "SSS",
                      "Ph.Health",
                      "HDMF",
                      "Taxable",
                      "W.Tax",
                      "Loan",
                      "A.fee",
                      "Adj.",
                      "Net",
                      "13th",
                      "Total"}


    Private cell_mapped_text_value() As String =
        New String() {"DatCol2",
                      "DatCol3"}

    Private cell_mapped_decim_value() As String =
        New String() {"DatCol43",
                      "DatCol21",
                      "DatCol41",
                      "DatCol47",
                      "DatCol44",
                      "DatCol37",
                      "DatCol36",
                      "DatCol35",
                      "DatCol38",
                      "DatCol46",
                      "DatCol48",
                      "DatCol34",
                      "DatCol33",
                      "DatCol32",
                      "DatCol31",
                      "DatCol30",
                      "DatCol22",
                      "DatCol25",
                      "DatCol27",
                      "DatCol28",
                      "DatCol24",
                      "DatCol26",
                      "DatCol29",
                      "DatCol39",
                      "DatCol45",
                      "DatCol23",
                      "DatCol40",
                      "DatCol42"}

    Private is_actual As Boolean = False

    Private preferred_font As Font =
        New System.Drawing.Font("Source Sans Pro", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Private preferred_excel_font As ExcelFont

    Private font_size As Single = 8

    Dim margin_size() As Decimal = New Decimal() {0.25D, 0.75D, 0.3D}

    Public Property Name As String = "" Implements IReportProvider.Name

    Property IsActual As Boolean
        Get
            Return is_actual
        End Get
        Set(value As Boolean)
            is_actual = value
        End Set
    End Property

    Public Sub Run() Implements IReportProvider.Run

        Static last_cell_column As String = basic_alphabet.Last

        Dim bool_result As Short = Convert.ToInt16(is_actual) 'Convert.ToInt16(SalaryActualDeclared)

        Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

        n_PayrollSummaDateSelection.ReportIndex = 6

        If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

            Dim excel_custom_format As Boolean = Convert.ToBoolean(ExcelOptionFormat())

            Dim parameters =
                New Object() {orgztnID,
                              n_PayrollSummaDateSelection.PayPeriodFromID,
                              n_PayrollSummaDateSelection.PayPeriodToID,
                              bool_result,
                              n_PayrollSummaDateSelection.cboStringParameter.Text,
                              excel_custom_format}

            Dim sql_print_employee_profiles As New SQL("CALL PAYROLLSUMMARY2(?og_rowid, ?min_pp_rowid, ?max_pp_rowid, ?is_actual, ?salaray_distrib, ?keep_in_onesheet);",
                                                       parameters)

            Static one_value As Integer = 1

            Try

                Dim ds As New DataSet

                'Dim dt As New DataTable

                'dt = sql_print_employee_profiles.GetFoundRows.Tables(0)
                ds = sql_print_employee_profiles.GetFoundRows

                If sql_print_employee_profiles.HasError Then

                    Throw sql_print_employee_profiles.ErrorException
                Else

                    Static report_name As String = "PayrollSummary"

                    Static temp_path As String = Path.GetTempPath()

                    Dim short_dates() As String =
                        New String() {CDate(n_PayrollSummaDateSelection.DateFrom).ToShortDateString,
                                      CDate(n_PayrollSummaDateSelection.DateTo).ToShortDateString}

                    Dim temp_file As String =
                        String.Concat(temp_path,
                                      orgNam,
                                      report_name, n_PayrollSummaDateSelection.cboStringParameter.Text.Replace(" ", ""), "Report",
                                      String.Concat(short_dates(0).Replace("/", "-"), "TO", short_dates(1).Replace("/", "-")),
                                      ".xlsx")

                    Dim date_range As String =
                        String.Concat("for the period of ", short_dates(0), " to ", short_dates(1))

                    Dim newFile = New FileInfo(temp_file)

                    If newFile.Exists Then
                        newFile.Delete()
                        newFile = New FileInfo(temp_file)
                    End If

                    'preferred_excel_font.Name = "Source Sans Pro Regular"
                    'preferred_excel_font.Name = preferred_font.Name

                    Dim tbl_withrows =
                            ds.Tables.OfType(Of DataTable).Where(Function(dt) dt.Rows.Count > 0)

                    Using excl_pkg = New ExcelPackage(newFile)

                        Dim ii = 0

                        For Each dtbl As DataTable In tbl_withrows

                            Dim worksheet As ExcelWorksheet =
                                    excl_pkg.Workbook.Worksheets.Add(String.Concat(report_name, ii))

                            worksheet.Cells.Style.Font.Size = font_size

                            Dim cell1 = worksheet.Cells(1, one_value)

                            cell1.Value = orgNam.ToUpper
                            cell1.Style.Font.Bold = True

                            Dim cell2 = worksheet.Cells(2, one_value)

                            cell2.Value = date_range

                            Dim row_indx As Integer = 5

                            Dim col_index As Integer = one_value

                            'For Each dtcol As DataColumn In dt.Columns
                            '    worksheet.Cells(row_indx, col_index).Value = dtcol.ColumnName
                            '    col_index += one_value
                            'Next

                            For Each str_header As String In column_headers
                                Dim cell_row5 = worksheet.Cells(row_indx, col_index)
                                cell_row5.Value = str_header
                                cell_row5.Style.Font.Bold = True

                                col_index += one_value
                            Next

                            row_indx += one_value

                            Dim details_start_rowindex = row_indx

                            Dim details_last_rowindex = 0

                            Dim last_cell_range As String = String.Empty

                            For Each dtrow As DataRow In dtbl.Rows

                                Dim cell3 = worksheet.Cells(3, one_value)

                                Dim division_name = dtrow("DatCol1").ToString

                                If division_name.Length > 0 Then

                                    cell3.Value =
                                        String.Concat("Division: ", division_name)

                                    worksheet.Name = division_name

                                End If

                                Dim row_array = dtrow.ItemArray

                                Dim i = 0

                                'For Each rowval In row_array

                                'Next

                                For Each cell_val As String In cell_mapped_text_value

                                    Dim excl_colrow As String =
                                            String.Concat(basic_alphabet(i),
                                                          row_indx)

                                    Dim _cells = worksheet.Cells(excl_colrow)

                                    _cells.Value = dtrow(cell_val)

                                    i += one_value

                                Next

                                '********************

                                For Each cell_val As String In cell_mapped_decim_value

                                    Dim excl_colrow As String =
                                            String.Concat(basic_alphabet(i),
                                                          row_indx)

                                    last_cell_range = basic_alphabet(i)

                                    Dim _cells = worksheet.Cells(excl_colrow)

                                    _cells.Value = dtrow(cell_val)
                                    _cells.Style.Numberformat.Format = "#,##0.00_);(#,##0.00)"

                                    _cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right

                                    i += one_value

                                Next

                                details_last_rowindex = row_indx

                                row_indx += one_value

                            Next

                            'last_cell_range = String.Concat(last_cell_range, (row_indx + 1))

                            Dim sum_cell_range = String.Join(":",
                                                                 String.Concat("C", row_indx),
                                                                 String.Concat(last_cell_column, row_indx))
                            ''FromRow, FromColumn, ToRow, ToColumn
                            'worksheet.Cells(sum_cell_range).Formula = String.Format("SUBTOTAL(9,{0})") ', New ExcelAddress(2, 3, 4, 3).Address)

                            worksheet.Cells(sum_cell_range).Formula =
                                    String.Format("SUM({0})",
                                                  New ExcelAddress(details_start_rowindex,
                                                                   3,
                                                                   details_last_rowindex,
                                                                   3).Address) 'column_headers.Count

                            worksheet.Cells(sum_cell_range).Style.Font.Bold = True
                            worksheet.Cells(sum_cell_range).Style.Numberformat.Format = "#,##0.00_);(#,##0.00)"

                            With worksheet.PrinterSettings

                                .Orientation = eOrientation.Landscape

                                .PaperSize = ePaperSize.Legal

                                .TopMargin = margin_size(1)
                                .BottomMargin = margin_size(1)
                                .LeftMargin = margin_size(0)
                                .RightMargin = margin_size(0)

                            End With

                            worksheet.Cells.AutoFitColumns()

                            worksheet.Cells("A1").AutoFitColumns(4.9, 5.3)

                            excl_pkg.Save()

                            ii += 1

                        Next

                    End Using

                    If tbl_withrows.Count > 0 Then

                        Process.Start(temp_file)

                    Else
                        MsgBox("No found record(s)", MsgBoxStyle.Information)

                    End If

                End If
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name))
            End Try

        End If

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

End Class

Friend Enum SalaryActualization As Short
    Declared = 0
    Actual = 1

End Enum

Friend Enum ExcelOption As Short
    SeparateEachDepartment = 0
    KeepAllInOneSheet = 1

End Enum