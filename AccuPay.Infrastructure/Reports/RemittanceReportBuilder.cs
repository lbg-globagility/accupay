using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Reports;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Reports
{
    public class RemittanceReportBuilder : ExcelFormatReport, IRemittanceReportBuilder
    {
        private const int HeaderRowIndex = 5;
        private const string DateFormat = "MM/dd/yyyy";

        private readonly IRemittanceReportDataService _dataService;

        private readonly ColumnList<RemittanceReportRow> _payrollColumns = new ColumnList<RemittanceReportRow>()
        {
            { "NAME", r => r.EmployeeName },
            { "SSS Number", r => r.SssNumber },
            { "PHIC Number", r => r.PhilHealthNumber },
            { "Birthday", r => r.BirthDate },
            { "TOTAL\nBASIC", r => r.TotalBasic, true },
            { "TOTAL\nCOMP", r => r.TotalCompensation, true },
            { "SSS Premium", r => r.SssPremium, true },
            { "ECC", r => r.Ecc, true },
            { "Mandatory\nProvident Fund", r => r.MandatoryProvidentFund, true },
            { "TOTAL ER/EC\nSSS", r => r.TotalSss, true },
            { "Total SSS\nSalary Loan", r => r.SssSalaryLoan, true },
            { "Total SSS\nCalamity Loan", r => r.SssCalamityLoan, true },
            { "Total SSS\nEmergency Loan", r => r.SssEmergencyLoan, true },
            { "Total HDMF\nSalary Loan", r => r.HdmfSalaryLoan, true },
            { "Total HDMF\nCalamity Loan", r => r.HdmfCalamityLoan, true },
            { "Total HDMF\nMP2", r => r.HdmfMp2, true },
            { "TOTAL\nHDMF", r => r.TotalHdmf, true },
            { "Total\nTAX", r => r.TotalTax, true },
            { "TOTAL ER/EC\nPH", r => r.TotalPhilHealth, true },
        };

        private readonly ColumnList<RemittanceReportRow> _philHealthColumns = new ColumnList<RemittanceReportRow>()
        {
            { "NAME", r => r.EmployeeName },
            { "PHIC Number", r => r.PhilHealthNumber },
            { "Birthday", r => r.BirthDate },
            { "TOTAL\nBASIC", r => r.TotalBasic, true },
            { "", r => r.PhilHealthEmployeeShare, true },
            { "", r => r.PhilHealthEmployerShare, true },
            { "TOTAL ER/EC\nPH", r => r.TotalPhilHealth, true },
        };

        private readonly ColumnList<RemittanceReportRow> _sssColumns = new ColumnList<RemittanceReportRow>()
        {
            { "NAME", r => r.EmployeeName },
            { "SSS Number", r => r.SssNumber },
            { "TOTAL\nCOMP", r => r.TotalCompensation, true },
            { "SSS Premium", r => r.SssPremium, true },
            { "ECC", r => r.Ecc, true },
            { "Mandatory\nProvident Fund", r => r.MandatoryProvidentFund, true },
            { "TOTAL ER/EC\nSSS", r => r.TotalSss, true },
        };

        private readonly ColumnList<RemittanceLoanRow> _sssLoanColumns = new ColumnList<RemittanceLoanRow>()
        {
            { "NAME", r => r.EmployeeName },
            { "SSS Number", r => r.SssNumber },
            { "Loan date", r => r.LoanDate },
            { "Loan Amount", r => r.LoanAmount },
            { "Total SSS\nLoan", r => r.TotalDeduction, true },
            { "Remarks", r => r.Remarks },
        };

        private readonly ColumnList<RemittanceReportRow> _hdmfColumns = new ColumnList<RemittanceReportRow>()
        {
            { "PAGIBIG_NO", r => r.PagIbigNumber },
            { "EMP ID @ CMPY", r => r.EmployeeNumber },
            { "LASTNAME", r => r.LastName },
            { "FIRST_NAME", r => r.FirstName },
            { "MIDDLE_NAME", r => r.MiddleName },
            { "EE_AMT", r => r.HdmfEmployeeShare },
            { "ER_AMT", r => r.HdmfEmployerShare },
            { "TIN", r => r.TinNumber },
            { "BIRTHDATE", r => r.BirthDate },
        };

        public RemittanceReportBuilder(IRemittanceReportDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task CreateReport(int organizationId, DateTime dateMonth, string saveFilePath)
        {
            var rows = await _dataService.GetData(organizationId, dateMonth);

            if (!rows.Any())
                throw new Exception("No paystubs found for the selected month.");

            var sssSalaryLoanData = await _dataService.GetSssSalaryLoanData(organizationId, dateMonth);

            var newFile = new FileInfo(saveFilePath);
            if (newFile.Exists)
                newFile.Delete();

            using (var excel = new ExcelPackage(newFile))
            {
                var worksheets = excel.Workbook.Worksheets;
                var monthEnd = dateMonth.AddMonths(1).AddDays(-1);
                var monthTitle = dateMonth.ToString("MMMM yyyy").ToUpper();

                RenderSheet(
                    worksheets.Add(dateMonth.ToString("MMMyyyy")),
                    new[]
                    {
                        "PAYROLL SHEET",
                        $"Pay period: {dateMonth:MMMM} 1 - {monthEnd.Day}, {dateMonth.Year}",
                        "Payout Date:"
                    },
                    _payrollColumns,
                    rows.ToList(),
                    boldTitle: false);

                RenderSheet(
                    worksheets.Add("phic"),
                    new[] { "PHILHEALTH", monthTitle },
                    _philHealthColumns,
                    rows.Where(r => r.TotalPhilHealth != 0).ToList());

                RenderSheet(
                    worksheets.Add("sss"),
                    new[] { "SSS", monthTitle },
                    _sssColumns,
                    rows.Where(r => r.TotalSss != 0).ToList());

                RenderSssLoanSheet(worksheets.Add("sss loan"), monthTitle, dateMonth, sssSalaryLoanData);

                RenderHdmfSheet(worksheets.Add("hdmf"), rows.Where(r => r.TotalHdmf != 0).ToList());

                excel.Save();
            }
        }

        private void RenderSssLoanSheet(
            ExcelWorksheet worksheet,
            string monthTitle,
            DateTime dateMonth,
            RemittanceLoanData loanData)
        {
            var nextRowIndex = RenderSheet(
                worksheet,
                new[] { "SSS SALARY LOAN", monthTitle },
                _sssLoanColumns,
                loanData.Paid.ToList());

            if (!loanData.StoppedPayment.Any())
                return;

            var lastColumnIndex = _sssLoanColumns.Count + 1;
            var stopPaymentRowIndex = nextRowIndex + 1;
            var lastPaymentMonth = dateMonth.AddMonths(-1).ToString("MMMM yyyy").ToUpper();

            var stopPaymentHeader = worksheet.Cells[stopPaymentRowIndex, 1, stopPaymentRowIndex, lastColumnIndex];
            stopPaymentHeader.Merge = true;
            stopPaymentHeader.Value = $"STOP PAYMENT (LAST PAYMENT REMITTED - {lastPaymentMonth})";
            stopPaymentHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            stopPaymentHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
            stopPaymentHeader.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));
            SetBorder(stopPaymentHeader);

            RenderRows(worksheet, stopPaymentRowIndex + 1, _sssLoanColumns, loanData.StoppedPayment.ToList());
        }

        private void RenderHdmfSheet(ExcelWorksheet worksheet, IReadOnlyList<RemittanceReportRow> rows)
        {
            worksheet.Cells.Style.Font.Size = FontSize;

            for (int i = 0; i < _hdmfColumns.Count; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = _hdmfColumns[i].Header;
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                for (int i = 0; i < _hdmfColumns.Count; i++)
                {
                    SetCellValue(worksheet.Cells[rowIndex + 2, i + 1], _hdmfColumns[i].Value(rows[rowIndex]));
                }
            }

            worksheet.Cells.AutoFitColumns();
        }

        /// <returns>The row index after the totals row.</returns>
        private int RenderSheet<T>(
            ExcelWorksheet worksheet,
            IReadOnlyList<string> titleLines,
            ColumnList<T> columns,
            IReadOnlyList<T> rows,
            bool boldTitle = true)
        {
            worksheet.Cells.Style.Font.Size = FontSize;

            for (int i = 0; i < titleLines.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = titleLines[i];
                worksheet.Cells[i + 1, 1].Style.Font.Bold = boldTitle;
            }

            RenderHeaders(worksheet, columns);

            var firstRowIndex = HeaderRowIndex + 1;
            var lastRowIndex = RenderRows(worksheet, firstRowIndex, columns, rows);
            var totalRowIndex = lastRowIndex + 2;

            RenderTotals(worksheet, columns, totalRowIndex, firstRowIndex, lastRowIndex);

            worksheet.Cells.AutoFitColumns();
            worksheet.Column(1).Width = 5;
            for (int columnIndex = 2; columnIndex <= columns.Count + 1; columnIndex++)
            {
                if (worksheet.Column(columnIndex).Width < 12)
                    worksheet.Column(columnIndex).Width = 12;
            }

            return totalRowIndex + 1;
        }

        private void RenderHeaders<T>(ExcelWorksheet worksheet, ColumnList<T> columns)
        {
            var columnIndex = 2;
            foreach (var column in columns)
            {
                var cell = worksheet.Cells[HeaderRowIndex, columnIndex];
                cell.Value = column.Header;
                cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

                columnIndex += 1;
            }

            worksheet.Row(HeaderRowIndex).Height = 24;
            SetBorder(worksheet.Cells[HeaderRowIndex, 1, HeaderRowIndex, columns.Count + 1]);
        }

        /// <returns>The index of the last rendered row.</returns>
        private int RenderRows<T>(
            ExcelWorksheet worksheet,
            int firstRowIndex,
            ColumnList<T> columns,
            IReadOnlyList<T> rows)
        {
            if (!rows.Any())
                return firstRowIndex - 1;

            var rowIndex = firstRowIndex;
            var rowNumber = 1;

            foreach (var row in rows)
            {
                worksheet.Cells[rowIndex, 1].Value = rowNumber;

                var columnIndex = 2;
                foreach (var column in columns)
                {
                    SetCellValue(worksheet.Cells[rowIndex, columnIndex], column.Value(row));
                    columnIndex += 1;
                }

                rowIndex += 1;
                rowNumber += 1;
            }

            var lastRowIndex = rowIndex - 1;
            SetBorder(worksheet.Cells[firstRowIndex, 1, lastRowIndex, columns.Count + 1]);

            return lastRowIndex;
        }

        private void RenderTotals<T>(
            ExcelWorksheet worksheet,
            ColumnList<T> columns,
            int totalRowIndex,
            int firstRowIndex,
            int lastRowIndex)
        {
            var lastColumnIndex = columns.Count + 1;

            for (int i = 0; i < columns.Count; i++)
            {
                if (!columns[i].HasTotal)
                    continue;

                var columnIndex = i + 2;
                var cell = worksheet.Cells[totalRowIndex, columnIndex];
                var columnLetter = ExcelCellAddress.GetColumnLetter(columnIndex);

                if (lastRowIndex >= firstRowIndex)
                    cell.Formula = $"SUM({columnLetter}{firstRowIndex}:{columnLetter}{lastRowIndex})";
                else
                    cell.Value = 0;

                cell.Style.Numberformat.Format = NumberFormat;
            }

            var totalRow = worksheet.Cells[totalRowIndex, 1, totalRowIndex, lastColumnIndex];
            totalRow.Style.Font.Bold = true;
            totalRow.Style.Fill.PatternType = ExcelFillStyle.Solid;
            totalRow.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
        }

        private void SetCellValue(ExcelRange cell, object value)
        {
            cell.Value = value;

            if (value is decimal)
                cell.Style.Numberformat.Format = NumberFormat;
            else if (value is DateTime)
                cell.Style.Numberformat.Format = DateFormat;
        }

        private void SetBorder(ExcelRange range)
        {
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        private class ReportColumn<T>
        {
            public ReportColumn(string header, Func<T, object> value, bool hasTotal)
            {
                Header = header;
                Value = value;
                HasTotal = hasTotal;
            }

            public string Header { get; }

            public Func<T, object> Value { get; }

            public bool HasTotal { get; }
        }

        private class ColumnList<T> : List<ReportColumn<T>>
        {
            public void Add(string header, Func<T, object> value, bool hasTotal = false)
            {
                Add(new ReportColumn<T>(header, value, hasTotal));
            }
        }
    }
}
