using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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

                char? firstLetter = null;
                char currentLetter;

                var isMultiCharacter = letter.Length > 1;
                if (isMultiCharacter)
                {
                    firstLetter = letter[0];
                    currentLetter = letter[1];
                }
                else
                    currentLetter = letter[0];

                var currentLetterAsNumeric = Asc(currentLetter);
                if (currentLetterAsNumeric >= Asc('Z'))
                {
                    if (firstLetter == null)
                        firstLetter = 'A';
                    else
                    {
                        firstLetter++;
                    }

                    letter = $"{firstLetter}A";
                }
                else
                    letter = $"{firstLetter}{(char)(currentLetter + 1)}";
            }
        }

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

        protected static int Asc(char String)
        {
            int num;
            byte[] numArray;
            int num1 = Convert.ToInt32(String);
            if (num1 >= 128)
            {
                try
                {
                    Encoding fileIOEncoding = Encoding.Default;
                    char[] str = new char[] { String };
                    if (!fileIOEncoding.IsSingleByte)
                    {
                        numArray = new byte[2];
                        if (fileIOEncoding.GetBytes(str, 0, 1, numArray, 0) != 1)
                        {
                            if (BitConverter.IsLittleEndian)
                            {
                                byte num2 = numArray[0];
                                numArray[0] = numArray[1];
                                numArray[1] = num2;
                            }
                            num = BitConverter.ToInt16(numArray, 0);
                        }
                        else
                        {
                            num = numArray[0];
                        }
                    }
                    else
                    {
                        numArray = new byte[1];
                        fileIOEncoding.GetBytes(str, 0, 1, numArray, 0);
                        num = numArray[0];
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            else
            {
                num = num1;
            }
            return num;
        }
    }
}