using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Reports.Customize
{
    public class AccessOffshoringPayslip : ExcelFormatReport, IAccessOffshoringPayslip
    {
        private const string ClientName = "Access Offshoring Philippines Inc.";

        private const string ClientAddress =
            "1105 One San Miguel Avenue Bldg, San Miguel Ave Corner Shaw Blvd, Ortigas Center, Pasig City, Philippines";

        private const string DateFormat = "d-MMM-yy";
        private const string MoneyFormat = "\"PHP\" #,##0.00";

        private static readonly Color HeaderFillColor = Color.FromArgb(217, 217, 217);
        private static readonly Color HighlightFillColor = Color.FromArgb(255, 255, 0);
        private static readonly Color RateHighlightFillColor = Color.FromArgb(226, 239, 218);

        private readonly IPayPeriodRepository _payPeriodRepository;
        private readonly IPaystubRepository _paystubRepository;
        private readonly ISalaryRepository _salaryRepository;

        public AccessOffshoringPayslip(
            IPayPeriodRepository payPeriodRepository,
            IPaystubRepository paystubRepository,
            ISalaryRepository salaryRepository)
        {
            _payPeriodRepository = payPeriodRepository;
            _paystubRepository = paystubRepository;
            _salaryRepository = salaryRepository;
        }

        public async Task CreateReport(
            int organizationId,
            int payPeriodId,
            int[] employeeIds,
            string saveFilePath)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            if (payPeriod == null)
                throw new Exception("Pay period not found.");

            var paystubs = (await _paystubRepository.GetByPayPeriodFullPaystubAsync(payPeriodId))
                .Where(p => p.EmployeeID.HasValue && employeeIds.Contains(p.EmployeeID.Value))
                .OrderBy(p => p.Employee.LastName)
                .ThenBy(p => p.Employee.FirstName)
                .ToList();

            var salaryByEmployeeId = (await _salaryRepository.GetByMultipleEmployeeAsync(employeeIds, payPeriod.PayToDate))
                .Where(s => s.EmployeeID.HasValue)
                .ToDictionary(s => s.EmployeeID.Value);

            var usedSheetNames = new HashSet<string>();

            var newFile = new FileInfo(saveFilePath);

            using (var excel = new ExcelPackage(newFile))
            {
                foreach (var paystub in paystubs)
                {
                    var employee = paystub.Employee;

                    salaryByEmployeeId.TryGetValue(employee.RowID.Value, out var salary);

                    var allowanceItems = await _paystubRepository.GetAllowanceItemsAsync(paystub.RowID.Value);

                    var sheetName = GetUniqueSheetName(employee.FullNameLastNameFirst, usedSheetNames);

                    var worksheet = excel.Workbook.Worksheets.Add(sheetName);

                    RenderEmployeeSheet(worksheet, employee, payPeriod, paystub, salary, allowanceItems);
                }

                excel.Save();
            }
        }

        private void RenderEmployeeSheet(
            ExcelWorksheet worksheet,
            Employee employee,
            PayPeriod payPeriod,
            Paystub paystub,
            Salary salary,
            ICollection<AllowanceItem> allowanceItems)
        {
            worksheet.Cells.Style.Font.Size = FontSize;

            SetColumnWidths(worksheet);

            RenderLetterHead(worksheet, employee);

            RenderBilledToBlock(worksheet, payPeriod);

            var monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary);
            var dailyRate = PayrollTools.GetDailyRate(salary, employee);
            var hourlyRate = PayrollTools.GetHourlyRateByDailyRate(salary, employee);

            RenderRatesBlock(worksheet, startRow: 9, monthlyRate, dailyRate, hourlyRate);

            RenderInvoiceHeader(worksheet, rowIndex: 13, employee, payPeriod);

            var payrollStartRow = 15;
            var (lastPayrollRow, payrollRows) = RenderPayrollTable(worksheet, payrollStartRow, payPeriod, paystub, allowanceItems);

            var deductionStartRow = lastPayrollRow + 2;
            var totalDeductionRow = RenderDeductionBlock(worksheet, deductionStartRow, paystub);

            var totalPayableRow = totalDeductionRow + 2;
            RenderTotalPayable(worksheet, totalPayableRow, payrollRows, totalDeductionRow);

            var remitToStartRow = totalPayableRow + 3;
            RenderRemitToBlock(worksheet, remitToStartRow, employee);

            var noteRow = remitToStartRow + 5;
            RenderNoteAndSignature(worksheet, noteRow, employee, payPeriod);

            SetDefaultPrinterSettings(worksheet.PrinterSettings);
        }

        private void SetColumnWidths(ExcelWorksheet worksheet)
        {
            worksheet.Column(1).Width = 3; // spacer
            worksheet.Column(2).Width = 26; // Label
            worksheet.Column(3).Width = 12; // Secondary value (e.g. hours)
            worksheet.Column(4).Width = 16; // Amount
            worksheet.Column(5).Width = 24; // Remarks
        }

        private void RenderLetterHead(ExcelWorksheet worksheet, Employee employee)
        {
            var nameCell = worksheet.Cells["B2"];
            nameCell.Value = employee.FullName;
            nameCell.Style.Font.Bold = true;
            nameCell.Style.Font.Size = FontSize + 4;

            worksheet.Cells["B3"].Value = string.IsNullOrWhiteSpace(employee.HomeAddress) ? "<Address>" : employee.HomeAddress;
        }

        private void RenderBilledToBlock(ExcelWorksheet worksheet, PayPeriod payPeriod)
        {
            void RenderRow(int rowIndex, string label, string value)
            {
                worksheet.Cells[$"B{rowIndex}"].Value = label;
                worksheet.Cells[$"B{rowIndex}"].Style.Font.Bold = true;

                worksheet.Cells[$"D{rowIndex}:E{rowIndex}"].Merge = true;
                worksheet.Cells[$"D{rowIndex}"].Value = value;
            }

            RenderRow(6, "Billed To:", ClientName);
            RenderRow(7, "ADDRESS:", ClientAddress);
            RenderRow(8, "Date:", $"{payPeriod.PayFromDate:d-MMM-yy} to {payPeriod.PayToDate:d-MMM-yy}");

            worksheet.Cells["B6:E8"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["B6:E8"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        private void RenderRatesBlock(ExcelWorksheet worksheet, int startRow, decimal monthlyRate, decimal dailyRate, decimal hourlyRate)
        {
            void RenderRateRow(int rowIndex, string label, decimal value, bool highlight)
            {
                worksheet.Cells[$"B{rowIndex}:C{rowIndex}"].Merge = true;
                worksheet.Cells[$"B{rowIndex}"].Value = label;

                worksheet.Cells[$"D{rowIndex}:E{rowIndex}"].Merge = true;
                worksheet.Cells[$"D{rowIndex}"].Value = value;
                worksheet.Cells[$"D{rowIndex}"].Style.Numberformat.Format = MoneyFormat;

                if (highlight)
                {
                    worksheet.Cells[$"B{rowIndex}:E{rowIndex}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"B{rowIndex}:E{rowIndex}"].Style.Fill.BackgroundColor.SetColor(RateHighlightFillColor);
                }

                worksheet.Cells[$"B{rowIndex}:E{rowIndex}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"B{rowIndex}:E{rowIndex}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"B{rowIndex}:E{rowIndex}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"B{rowIndex}:E{rowIndex}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            RenderRateRow(startRow, "Monthly Rate", monthlyRate, highlight: true);
            RenderRateRow(startRow + 1, "Daily Rate", dailyRate, highlight: false);
            RenderRateRow(startRow + 2, "Hourly Rate", hourlyRate, highlight: false);
        }

        private void RenderInvoiceHeader(ExcelWorksheet worksheet, int rowIndex, Employee employee, PayPeriod payPeriod)
        {
            worksheet.Cells[$"B{rowIndex}:E{rowIndex}"].Merge = true;
            var titleCell = worksheet.Cells[$"B{rowIndex}"];
            titleCell.Value = $"INVOICE #{payPeriod.PayToDate:yyyy}-{employee.EmployeeNo}";
            titleCell.Style.Font.Bold = true;
            titleCell.Style.Font.Size = FontSize + 4;
            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private (int lastRow, List<int> payrollRows) RenderPayrollTable(
            ExcelWorksheet worksheet,
            int startRow,
            PayPeriod payPeriod,
            Paystub paystub,
            ICollection<AllowanceItem> allowanceItems)
        {
            var rowIndex = startRow;

            worksheet.Cells[$"B{rowIndex}:C{rowIndex}"].Merge = true;
            worksheet.Cells[$"B{rowIndex}"].Value = "PAYROLL";
            worksheet.Cells[$"D{rowIndex}"].Value = "AMOUNT";
            worksheet.Cells[$"E{rowIndex}"].Value = "REMARKS";

            var headerRange = worksheet.Cells[$"B{rowIndex}:E{rowIndex}"];
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(HeaderFillColor);
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rowIndex++;

            var payrollRows = new List<int>();

            void RenderLineItem(string label, decimal amount, bool highlight = false)
            {
                worksheet.Cells[$"B{rowIndex}:C{rowIndex}"].Merge = true;
                worksheet.Cells[$"B{rowIndex}"].Value = label;

                worksheet.Cells[$"D{rowIndex}"].Value = amount;
                worksheet.Cells[$"D{rowIndex}"].Style.Numberformat.Format = MoneyFormat;

                if (highlight)
                {
                    worksheet.Cells[$"B{rowIndex}:C{rowIndex}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"B{rowIndex}:C{rowIndex}"].Style.Fill.BackgroundColor.SetColor(HighlightFillColor);
                }

                payrollRows.Add(rowIndex);
                rowIndex++;
            }

            var wfhAllowanceAmount = allowanceItems?
                .Where(a => a.Allowance?.Product?.Name != null &&
                    a.Allowance.Product.Name.IndexOf("WFH", StringComparison.OrdinalIgnoreCase) >= 0)
                .Sum(a => a.Amount) ?? 0M;

            var sssTotal = paystub.SssEmployeeShare + paystub.SssEmployerShare;
            var philHealthTotal = paystub.PhilHealthEmployeeShare + paystub.PhilHealthEmployerShare;
            var hdmfTotal = paystub.HdmfEmployeeShare + paystub.HdmfEmployerShare;

            RenderLineItem($"{payPeriod.PayFromDate:MMM d} - {payPeriod.PayToDate:MMM d, yyyy}", paystub.TotalEarnings, highlight: true);
            RenderLineItem("WFH ALLOWANCE", wfhAllowanceAmount);
            RenderLineItem("SSS", sssTotal);
            RenderLineItem("PHIC", philHealthTotal);
            RenderLineItem("HDMF", hdmfTotal);

            var lastRow = rowIndex - 1;

            var tableRange = worksheet.Cells[$"B{startRow}:E{lastRow}"];
            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            return (lastRow, payrollRows);
        }

        private int RenderDeductionBlock(ExcelWorksheet worksheet, int startRow, Paystub paystub)
        {
            var rowIndex = startRow;

            worksheet.Cells[$"B{rowIndex}:E{rowIndex}"].Merge = true;
            worksheet.Cells[$"B{rowIndex}"].Value = "DEDUCTION";
            worksheet.Cells[$"B{rowIndex}"].Style.Font.Bold = true;
            rowIndex++;

            var deductionRows = new List<int>();

            worksheet.Cells[$"B{rowIndex}"].Value = "Unpaid Leaves";
            worksheet.Cells[$"C{rowIndex}"].Value = paystub.AbsentHours;
            worksheet.Cells[$"C{rowIndex}"].Style.Numberformat.Format = "0.00";
            worksheet.Cells[$"D{rowIndex}"].Value = paystub.AbsenceDeduction;
            worksheet.Cells[$"D{rowIndex}"].Style.Numberformat.Format = MoneyFormat;
            deductionRows.Add(rowIndex);
            rowIndex++;

            worksheet.Cells[$"B{rowIndex}:C{rowIndex}"].Merge = true;
            worksheet.Cells[$"B{rowIndex}"].Value = "TOTAL DEDUCTION";
            worksheet.Cells[$"B{rowIndex}"].Style.Font.Bold = true;

            var totalDeductionCell = worksheet.Cells[$"D{rowIndex}"];
            totalDeductionCell.Formula = string.Join("+", deductionRows.Select(r => $"D{r}"));
            totalDeductionCell.Style.Numberformat.Format = MoneyFormat;
            totalDeductionCell.Style.Font.Bold = true;
            totalDeductionCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;

            var totalDeductionRow = rowIndex;

            var blockRange = worksheet.Cells[$"B{startRow}:E{totalDeductionRow}"];
            blockRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            blockRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            blockRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            blockRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            return totalDeductionRow;
        }

        private void RenderTotalPayable(
            ExcelWorksheet worksheet,
            int rowIndex,
            List<int> payrollRows,
            int totalDeductionRow)
        {
            worksheet.Cells[$"B{rowIndex}:C{rowIndex}"].Merge = true;
            var labelCell = worksheet.Cells[$"B{rowIndex}"];
            labelCell.Value = "TOTAL AMOUNT PAYABLE";
            labelCell.Style.Font.Bold = true;
            labelCell.Style.Font.Size = FontSize + 2;
            labelCell.Style.Font.Color.SetColor(Color.Red);

            var totalCell = worksheet.Cells[$"D{rowIndex}"];
            totalCell.Formula = string.Join("+", payrollRows.Select(r => $"D{r}")) + $"-D{totalDeductionRow}";
            totalCell.Style.Numberformat.Format = MoneyFormat;
            totalCell.Style.Font.Bold = true;
            totalCell.Style.Font.Size = FontSize + 2;
            totalCell.Style.Font.Color.SetColor(Color.Red);
            totalCell.Style.Border.Top.Style = ExcelBorderStyle.Double;
        }

        private void RenderRemitToBlock(ExcelWorksheet worksheet, int startRow, Employee employee)
        {
            worksheet.Cells[$"B{startRow}:E{startRow}"].Merge = true;
            worksheet.Cells[$"B{startRow}"].Value = "PLS REMIT PAYMENT TO:";
            worksheet.Cells[$"B{startRow}"].Style.Font.Bold = true;

            void RenderRow(int rowIndex, string label, string value)
            {
                worksheet.Cells[$"B{rowIndex}"].Value = label;
                worksheet.Cells[$"B{rowIndex}"].Style.Font.Bold = true;

                worksheet.Cells[$"D{rowIndex}:E{rowIndex}"].Merge = true;
                worksheet.Cells[$"D{rowIndex}"].Value = value;
            }

            RenderRow(startRow + 1, "Account Name", employee.FullName);
            RenderRow(startRow + 2, "Account Number", employee.AtmNo);
            RenderRow(startRow + 3, "Branch:", employee.BankName);
        }

        private void RenderNoteAndSignature(ExcelWorksheet worksheet, int noteRow, Employee employee, PayPeriod payPeriod)
        {
            worksheet.Cells[$"B{noteRow}:E{noteRow}"].Merge = true;
            var noteCell = worksheet.Cells[$"B{noteRow}"];
            noteCell.Value = "***NOTE: My government contributions will be remitted directly by myself as self employed.";
            noteCell.Style.Font.Italic = true;

            var signatureRow = noteRow + 5;

            worksheet.Cells[$"D{signatureRow}"].Value = employee.FullName;
            worksheet.Cells[$"D{signatureRow}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;

            worksheet.Cells[$"E{signatureRow}"].Value = payPeriod.PayToDate;
            worksheet.Cells[$"E{signatureRow}"].Style.Numberformat.Format = DateFormat;
            worksheet.Cells[$"E{signatureRow}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;

            worksheet.Cells[$"D{signatureRow + 1}:E{signatureRow + 1}"].Merge = true;
            var signatureLabelCell = worksheet.Cells[$"D{signatureRow + 1}"];
            signatureLabelCell.Value = "SIGNATURE OVER PRINTED NAME/DATE";
            signatureLabelCell.Style.Font.Italic = true;
            signatureLabelCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private string GetUniqueSheetName(string preferredName, HashSet<string> usedSheetNames)
        {
            var sanitized = new string(preferredName
                .Select(c => "\\/?*[]:".IndexOf(c) >= 0 ? '-' : c)
                .ToArray());

            if (sanitized.Length > 31)
                sanitized = sanitized.Substring(0, 31);

            var candidate = sanitized;
            var suffix = 1;

            while (!usedSheetNames.Add(candidate))
            {
                var suffixText = $" ({++suffix})";
                candidate = sanitized.Substring(0, Math.Min(sanitized.Length, 31 - suffixText.Length)) + suffixText;
            }

            return candidate;
        }
    }
}
