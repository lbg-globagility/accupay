Option Strict On

Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class ExcelFormatReport

    Private ReadOnly MarginSize() As Decimal = New Decimal() {0.25D, 0.75D, 0.3D}

    Protected Const FontSize As Single = 8

    Protected Sub RenderColumnHeaders(worksheet As ExcelWorksheet,
                                      rowIndex As Integer,
                                      reportColumns As IReadOnlyCollection(Of ExcelReportColumn))
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

    Protected Iterator Function GenerateAlphabet() As IEnumerable(Of String)
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

    Protected Shared Function GetPayrollSelector() As PayrollSummaDateSelection

        Dim payrollSelector = New PayrollSummaDateSelection With {
            .ReportIndex = 6
        }

        If payrollSelector.ShowDialog() <> DialogResult.OK Then
            Return Nothing
        End If

        Return payrollSelector

    End Function

    Protected Shared Sub RenderSubTotal(worksheet As ExcelWorksheet, subTotalCellRange As String, employeesStartIndex As Integer, employeesLastIndex As Integer)
        worksheet.Cells(subTotalCellRange).Formula = String.Format(
                        "SUM({0})",
                        New ExcelAddress(employeesStartIndex, 3, employeesLastIndex, 3).Address)

        worksheet.Cells(subTotalCellRange).Style.Font.Bold = True
        worksheet.Cells(subTotalCellRange).Style.Numberformat.Format = "#,##0.00_);(#,##0.00)"
        worksheet.Cells(subTotalCellRange).Style.Border.Top.Style = ExcelBorderStyle.Thin
    End Sub

    Protected Shared Sub RenderGrandTotal(worksheet As ExcelWorksheet,
                                            grandTotalRange As String,
                                            subTotalRows As IEnumerable(Of Integer))
        worksheet.Cells(grandTotalRange).Formula = String.Format("SUM({0})", String.Join(",", subTotalRows.Select(Function(s) $"C{s}")))
        worksheet.Cells(grandTotalRange).Style.Border.Top.Style = ExcelBorderStyle.Double
        worksheet.Cells(grandTotalRange).Style.Font.Bold = True
        worksheet.Cells(grandTotalRange).Style.Numberformat.Format = "#,##0.00_);(#,##0.00)"
    End Sub

    Protected Sub SetDefaultPrinterSettings(settings As ExcelPrinterSettings)
        With settings
            .Orientation = eOrientation.Landscape
            .PaperSize = ePaperSize.Legal
            .TopMargin = MarginSize(1)
            .BottomMargin = MarginSize(1)
            .LeftMargin = MarginSize(0)
            .RightMargin = MarginSize(0)
        End With
    End Sub

End Class