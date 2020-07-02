using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AccuPay.Infrastructure.Reports
{
    public class ExcelFormatReport
    {
        private readonly decimal[] MarginSize = new decimal[] { 0.25M, 0.75M, 0.3M };

        protected const float FontSize = 8;

        protected void RenderColumnHeaders(ExcelWorksheet worksheet, int rowIndex, IReadOnlyCollection<ExcelReportColumn> reportColumns)
        {
            int columnIndex = 1;
            foreach (var column in reportColumns)
            {
                var headerCell = worksheet.Cells[rowIndex, columnIndex];
                headerCell.Value = column.Name;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));

                columnIndex += 1;
            }
        }

        protected IEnumerable<string> GenerateAlphabet()
        {
            var letter = "A";

            while (true)
            {
                yield return letter;

                var firstLetter = '\0';
                var currentLetter = '\0';

                var isMultiCharacter = letter.Length > 1;
                if (isMultiCharacter)
                {
                    firstLetter = letter[0];
                    currentLetter = letter[1];
                }
                else
                    currentLetter = letter[0];

                var currentLetterAsNumeric = char.GetNumericValue(currentLetter);
                if (currentLetterAsNumeric >= char.GetNumericValue('Z'))
                {
                    if (firstLetter == '\0')
                        firstLetter = 'A';
                    else
                        firstLetter++;
                    
                    letter = $"{firstLetter}A";
                }
                else
                    letter = $"{firstLetter}{currentLetter + 1}";
            }
        }

        //protected static PayrollSummaDateSelection GetPayrollSelector()
        //{
        //    var payrollSelector = new PayrollSummaDateSelection()
        //    {
        //        ReportIndex = 6
        //    };

        //    if (payrollSelector.ShowDialog() != DialogResult.OK)
        //        return null/* TODO Change to default(_) if this is not a reference type */;

        //    return payrollSelector;
        //}

        protected static void RenderSubTotal(ExcelWorksheet worksheet, string subTotalCellRange, int employeesStartIndex, int employeesLastIndex, int formulaColumnStart)
        {
            worksheet.Cells[subTotalCellRange].Formula = string.Format("SUM({0})", new ExcelAddress(employeesStartIndex, formulaColumnStart, employeesLastIndex, formulaColumnStart).Address);

            worksheet.Cells[subTotalCellRange].Style.Font.Bold = true;
            worksheet.Cells[subTotalCellRange].Style.Numberformat.Format = "#,##0.00_);(#,##0.00)";
            worksheet.Cells[subTotalCellRange].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        }

        protected static void RenderGrandTotal(ExcelWorksheet worksheet, int rowIndex, string lastCellColumn, IEnumerable<int> subTotalRows, char startingLetter)
        {
            var grandTotalRange = $"{startingLetter}{rowIndex}:{lastCellColumn}{rowIndex}";
            worksheet.Cells[grandTotalRange].Formula = string.Format("SUM({0})", string.Join(",", subTotalRows.Select(s => $"{startingLetter}{s}")));
            worksheet.Cells[grandTotalRange].Style.Border.Top.Style = ExcelBorderStyle.Double;
            worksheet.Cells[grandTotalRange].Style.Font.Bold = true;
            worksheet.Cells[grandTotalRange].Style.Numberformat.Format = "#,##0.00_);(#,##0.00)";
        }

        protected void SetDefaultPrinterSettings(ExcelPrinterSettings settings)
        {
            {
                var withBlock = settings;
                withBlock.Orientation = eOrientation.Landscape;
                withBlock.PaperSize = ePaperSize.Legal;
                withBlock.TopMargin = MarginSize[1];
                withBlock.BottomMargin = MarginSize[1];
                withBlock.LeftMargin = MarginSize[0];
                withBlock.RightMargin = MarginSize[0];
            }
        }
    }
}