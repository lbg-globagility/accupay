using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
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
    /// <summary>
    /// Lists the employees paid in the time period whose bank is not BDO, since they are not
    /// covered by the BDO bank file. Employees without a bank are left out.
    /// </summary>
    public class NonBdoBankReportBuilder : ExcelFormatReport, INonBdoBankReportBuilder
    {
        private const int AddressColumnIndex = 2;
        private const int MaxAddressColumnWidth = 60;
        private const string TextFormat = "@";

        private static readonly string[] Headers = { "Name", "Address", "Bank", "Bank Address", "Account Number", "Pay" };

        private readonly IPaystubRepository _paystubRepository;

        public NonBdoBankReportBuilder(IPaystubRepository paystubRepository)
        {
            _paystubRepository = paystubRepository;
        }

        public async Task CreateReport(int organizationId, TimePeriod timePeriod, string saveFilePath)
        {
            var paystubs = await _paystubRepository
                .GetByTimePeriodWithThirteenthMonthPayAndEmployeeAsync(timePeriod, organizationId);

            var rows = paystubs
                .Where(p => !string.IsNullOrWhiteSpace(p.Employee.BankName) && !p.Employee.HasBdoBank)
                .GroupBy(p => p.EmployeeID)
                .Select(g => (Employee: g.First().Employee, Pay: g.Sum(p => p.NetPay)))
                .OrderBy(r => r.Employee.FullNameWithMiddleInitialLastNameFirst)
                .ToList();

            if (!rows.Any())
                throw new Exception("No employees with a non-BDO bank found for the selected pay periods.");

            var newFile = new FileInfo(saveFilePath);
            if (newFile.Exists)
                newFile.Delete();

            using (var excel = new ExcelPackage(newFile))
            {
                RenderSheet(excel.Workbook.Worksheets.Add("Non-BDO Banks"), rows);

                excel.Save();
            }
        }

        private void RenderSheet(ExcelWorksheet worksheet, IReadOnlyList<(Employee Employee, decimal Pay)> rows)
        {
            worksheet.Cells.Style.Font.Size = FontSize;

            for (int i = 0; i < Headers.Length; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = Headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
            }

            var rowIndex = 2;
            foreach (var (employee, pay) in rows)
            {
                worksheet.Cells[rowIndex, 1].Value = employee.FullNameWithMiddleInitialLastNameFirst;
                worksheet.Cells[rowIndex, 2].Value = employee.HomeAddress;
                worksheet.Cells[rowIndex, 3].Value = employee.BankName;
                // Column 4, Bank Address, stays blank because the system does not store it yet.

                // Kept as text so that the leading zeros of the account number are not lost.
                worksheet.Cells[rowIndex, 5].Style.Numberformat.Format = TextFormat;
                worksheet.Cells[rowIndex, 5].Value = employee.AtmNo;

                worksheet.Cells[rowIndex, 6].Value = pay;
                worksheet.Cells[rowIndex, 6].Style.Numberformat.Format = NumberFormat;

                rowIndex += 1;
            }

            worksheet.Cells.AutoFitColumns();

            var addressColumn = worksheet.Column(AddressColumnIndex);
            if (addressColumn.Width > MaxAddressColumnWidth)
            {
                addressColumn.Width = MaxAddressColumnWidth;
                addressColumn.Style.WrapText = true;
            }
        }
    }
}
