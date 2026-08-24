using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
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

namespace AccuPay.Infrastructure.Reports.Customize
{
    public class DailyAttendanceReport : ExcelFormatReport, IDailyAttendanceReport
    {
        private const string ClientName = "Access Offshoring Philippines Inc.";

        private const string ClientAddress =
            "1105 One San Miguel Avenue Bldg, San Miguel Ave Corner Shaw Blvd, Ortigas Center, Pasig City, Philippines";

        private const string TimeFormat = "h:mm AM/PM";
        private const string ElapsedTimeFormat = "[h]:mm";
        private const string MoneyFormat = "\"PHP\" #,##0.00";

        private static readonly Color HeaderFillColor = Color.FromArgb(217, 217, 217);
        private static readonly Color HighlightFillColor = Color.FromArgb(255, 255, 0);
        private static readonly Color TotalColumnFillColor = Color.FromArgb(250, 205, 205);

        private readonly IPayPeriodRepository _payPeriodRepository;
        private readonly IPaystubRepository _paystubRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IShiftRepository _shiftRepository;

        public DailyAttendanceReport(
            IPayPeriodRepository payPeriodRepository,
            IPaystubRepository paystubRepository,
            ISalaryRepository salaryRepository,
            ITimeLogRepository timeLogRepository,
            IShiftRepository shiftRepository)
        {
            _payPeriodRepository = payPeriodRepository;
            _paystubRepository = paystubRepository;
            _salaryRepository = salaryRepository;
            _timeLogRepository = timeLogRepository;
            _shiftRepository = shiftRepository;
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

            var datePeriod = new TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate);

            var paystubs = (await _paystubRepository.GetByPayPeriodFullPaystubAsync(payPeriodId))
                .Where(p => p.EmployeeID.HasValue && employeeIds.Contains(p.EmployeeID.Value))
                .OrderBy(p => p.Employee.LastName)
                .ThenBy(p => p.Employee.FirstName)
                .ToList();

            var salaryByEmployeeId = (await _salaryRepository.GetByMultipleEmployeeAsync(employeeIds, payPeriod.PayToDate))
                .Where(s => s.EmployeeID.HasValue)
                .ToDictionary(s => s.EmployeeID.Value);

            var timeLogsByEmployeeId = (await _timeLogRepository.GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(employeeIds, datePeriod))
                .Where(t => t.EmployeeID.HasValue)
                .GroupBy(t => t.EmployeeID.Value)
                .ToDictionary(g => g.Key, g => g.OrderBy(t => t.LogDate).ToList());

            var shiftsByEmployeeId = (await _shiftRepository.GetByEmployeeAndDatePeriodAsync(organizationId, employeeIds, datePeriod))
                .Where(s => s.EmployeeID.HasValue)
                .GroupBy(s => s.EmployeeID.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(s => s.DateSched.Date).ToDictionary(d => d.Key, d => d.First()));

            var usedSheetNames = new HashSet<string>();

            var newFile = new FileInfo(saveFilePath);

            using (var excel = new ExcelPackage(newFile))
            {
                foreach (var paystub in paystubs)
                {
                    var employee = paystub.Employee;

                    salaryByEmployeeId.TryGetValue(employee.RowID.Value, out var salary);

                    if (!timeLogsByEmployeeId.TryGetValue(employee.RowID.Value, out var employeeTimeLogs))
                        employeeTimeLogs = new List<TimeLog>();

                    if (!shiftsByEmployeeId.TryGetValue(employee.RowID.Value, out var employeeShiftsByDate))
                        employeeShiftsByDate = new Dictionary<DateTime, Shift>();

                    var allowanceItems = await _paystubRepository.GetAllowanceItemsAsync(paystub.RowID.Value);

                    var sheetName = GetUniqueSheetName(employee.FullNameLastNameFirst, usedSheetNames);

                    var worksheet = excel.Workbook.Worksheets.Add(sheetName);

                    RenderEmployeeSheet(worksheet, employee, payPeriod, paystub, salary, employeeTimeLogs, employeeShiftsByDate, allowanceItems);
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
            IList<TimeLog> timeLogs,
            IDictionary<DateTime, Shift> shiftsByDate,
            ICollection<AllowanceItem> allowanceItems)
        {
            worksheet.Cells.Style.Font.Size = FontSize;

            SetColumnWidths(worksheet);

            RenderLetterHead(worksheet, employee);

            RenderBilledToBlock(worksheet, payPeriod);

            worksheet.Cells["A10:N10"].Merge = true;
            var sectionTitleCell = worksheet.Cells["A10"];
            sectionTitleCell.Value = "DAILY ATTENDANCE REPORT (DAR)";
            sectionTitleCell.Style.Font.Bold = true;
            sectionTitleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A10:N10"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A10:N10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            RenderTableHeaders(worksheet);

            var timeLogsByDate = timeLogs.ToDictionary(t => t.LogDate.Date);

            var lastDataRow = RenderDailyRows(worksheet, payPeriod, timeLogsByDate, shiftsByDate);

            var monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary);
            var dailyRate = PayrollTools.GetDailyRate(salary, employee);
            var hourlyRate = PayrollTools.GetHourlyRateByDailyRate(salary, employee);

            var ratesStartRow = lastDataRow + 2;
            RenderRatesBlock(worksheet, ratesStartRow, monthlyRate, dailyRate, hourlyRate);

            var payablesStartRow = ratesStartRow + 4;
            RenderPayablesBlock(worksheet, payablesStartRow, paystub, timeLogs, dailyRate, allowanceItems);

            SetDefaultPrinterSettings(worksheet.PrinterSettings);
            worksheet.PrinterSettings.RepeatRows = worksheet.Cells["11:12"];
        }

        private void SetColumnWidths(ExcelWorksheet worksheet)
        {
            worksheet.Column(1).Width = 10; // Date

            for (var column = 2; column <= 7; column++) // Regular/Overtime IN, OUT, Total
                worksheet.Column(column).Width = 8;

            worksheet.Column(8).Width = 9; // Leave With Pay
            worksheet.Column(9).Width = 9; // Leave Without Pay
            worksheet.Column(10).Width = 22; // Remarks
            worksheet.Column(11).Width = 22; // Details

            for (var column = 12; column <= 14; column++) // Lunch Break OUT, IN, Total
                worksheet.Column(column).Width = 8;
        }

        private void RenderLetterHead(ExcelWorksheet worksheet, Employee employee)
        {
            var nameCell = worksheet.Cells["A2"];
            nameCell.Value = employee.FullName;

            var dateLabelCell = worksheet.Cells["L2"];
            dateLabelCell.Value = "DATE:";
            dateLabelCell.Style.Font.Bold = true;

            var dateValueCell = worksheet.Cells["M2:N2"];
            dateValueCell.Merge = true;
            dateValueCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["M2"].Value = DateTime.Now;
            worksheet.Cells["M2"].Style.Numberformat.Format = "MMMM d, yyyy";

            worksheet.Cells["A3"].Value = string.IsNullOrWhiteSpace(employee.HomeAddress) ? "<Address>" : employee.HomeAddress;

            worksheet.Cells["A5:N5"].Merge = true;
            var titleCell = worksheet.Cells["A5"];
            titleCell.Value = "DAILY ATTENDANCE REPORT (DAR)";
            titleCell.Style.Font.Bold = true;
            titleCell.Style.Font.Size = 14;
            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private void RenderBilledToBlock(ExcelWorksheet worksheet, PayPeriod payPeriod)
        {
            worksheet.Cells["F6"].Value = "Billed To:";
            worksheet.Cells["F6"].Style.Font.Bold = true;
            worksheet.Cells["F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells["G6:N6"].Merge = true;
            worksheet.Cells["G6"].Value = ClientName;

            worksheet.Cells["F7"].Value = "ADDRESS:";
            worksheet.Cells["F7"].Style.Font.Bold = true;
            worksheet.Cells["F7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells["G7:N7"].Merge = true;
            worksheet.Cells["G7"].Value = ClientAddress;

            worksheet.Cells["F8"].Value = "Date:";
            worksheet.Cells["F8"].Style.Font.Bold = true;
            worksheet.Cells["F8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells["G8:N8"].Merge = true;
            worksheet.Cells["G8"].Value =
                $"{payPeriod.PayFromDate:d-MMM-yy} to {payPeriod.PayToDate:d-MMM-yy}";

            worksheet.Cells["A6:N8"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A6:N8"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        private void RenderTableHeaders(ExcelWorksheet worksheet)
        {
            void MergeVertical(string column, string text)
            {
                worksheet.Cells[$"{column}11:{column}12"].Merge = true;
                var cell = worksheet.Cells[$"{column}11"];
                cell.Value = text;
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            void MergeHorizontal(string range, string text)
            {
                worksheet.Cells[range].Merge = true;
                worksheet.Cells[range.Split(':')[0]].Value = text;
            }

            MergeVertical("A", "Date");
            MergeHorizontal("B11:D11", "Regular Hours");
            MergeHorizontal("E11:G11", "Overtime Hours");
            MergeHorizontal("H11:I11", "LEAVE (IN HOURS)");
            MergeVertical("J", "R E M A R K S");
            MergeVertical("K", "DETAILS");
            MergeHorizontal("L11:N11", "Lunch Break");

            worksheet.Cells["B12"].Value = "IN";
            worksheet.Cells["C12"].Value = "OUT";
            worksheet.Cells["D12"].Value = "Total";
            worksheet.Cells["E12"].Value = "IN";
            worksheet.Cells["F12"].Value = "OUT";
            worksheet.Cells["G12"].Value = "Total";
            worksheet.Cells["H12"].Value = "With Pay";
            worksheet.Cells["I12"].Value = "Without Pay";
            worksheet.Cells["L12"].Value = "OUT";
            worksheet.Cells["M12"].Value = "IN";
            worksheet.Cells["N12"].Value = "TOTAL";

            var headerRange = worksheet.Cells["A11:N12"];
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(HeaderFillColor);
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            var totalColumnHeader = worksheet.Cells["N11:N12"];
            totalColumnHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
            totalColumnHeader.Style.Fill.BackgroundColor.SetColor(TotalColumnFillColor);
        }

        private int RenderDailyRows(
            ExcelWorksheet worksheet,
            PayPeriod payPeriod,
            IDictionary<DateTime, TimeLog> timeLogsByDate,
            IDictionary<DateTime, Shift> shiftsByDate)
        {
            var rowIndex = 13;

            for (var date = payPeriod.PayFromDate.Date; date <= payPeriod.PayToDate.Date; date = date.AddDays(1))
            {
                RenderDailyRow(
                    worksheet,
                    rowIndex,
                    date,
                    timeLogsByDate.TryGetValue(date, out var log) ? log : null,
                    shiftsByDate.TryGetValue(date, out var shift) ? shift : null);

                rowIndex++;
            }

            var lastRow = rowIndex - 1;

            var dataRange = worksheet.Cells[$"A13:N{lastRow}"];
            dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            return lastRow;
        }

        private void RenderDailyRow(ExcelWorksheet worksheet, int rowIndex, DateTime date, TimeLog timeLog, Shift shift)
        {
            var dateCell = worksheet.Cells[$"A{rowIndex}"];
            dateCell.Value = date;
            dateCell.Style.Numberformat.Format = "d-MMM";

            SetTimeCell(worksheet.Cells[$"B{rowIndex}"], timeLog?.TimeIn);
            SetTimeCell(worksheet.Cells[$"C{rowIndex}"], timeLog?.TimeOut);
            SetElapsedFormula(worksheet.Cells[$"D{rowIndex}"], $"IF(OR(B{rowIndex}=\"\",C{rowIndex}=\"\"),0,(C{rowIndex}-B{rowIndex})-N{rowIndex})");

            SetElapsedFormula(worksheet.Cells[$"G{rowIndex}"], $"IF(OR(E{rowIndex}=\"\",F{rowIndex}=\"\"),0,F{rowIndex}-E{rowIndex})");

            var (lunchOut, lunchIn) = GetLunchTimes(timeLog, shift);

            SetTimeCell(worksheet.Cells[$"L{rowIndex}"], lunchOut);
            SetTimeCell(worksheet.Cells[$"M{rowIndex}"], lunchIn);
            SetElapsedFormula(worksheet.Cells[$"N{rowIndex}"], $"IF(OR(L{rowIndex}=\"\",M{rowIndex}=\"\"),0,M{rowIndex}-L{rowIndex})");
        }

        // An actual TimeLog lunch punch always wins, whether or not the shift requires one.
        // Only when there's no punch, and the shift doesn't require an explicit lunch punch
        // (RequiresLunchInOut = false) but does carve out a break, do we fall back to the
        // scheduled break window (BreakStartTime + BreakLength).
        private (TimeSpan? lunchOut, TimeSpan? lunchIn) GetLunchTimes(TimeLog timeLog, Shift shift)
        {
            if (timeLog?.LunchOut != null && timeLog?.LunchIn != null)
                return (timeLog.LunchOut, timeLog.LunchIn);

            if (shift != null && !shift.RequiresLunchInOut && shift.BreakLength > 0 && shift.BreakStartTime.HasValue)
            {
                var breakOut = shift.BreakStartTime.Value;
                var breakIn = breakOut.Add(TimeSpan.FromHours((double)shift.BreakLength));

                return (breakOut, breakIn);
            }

            return (timeLog?.LunchOut, timeLog?.LunchIn);
        }

        private void SetTimeCell(ExcelRange cell, TimeSpan? time)
        {
            if (time == null)
                return;

            cell.Value = time.Value.TotalDays;
            cell.Style.Numberformat.Format = TimeFormat;
        }

        private void SetElapsedFormula(ExcelRange cell, string formula)
        {
            cell.Formula = formula;
            cell.Style.Numberformat.Format = ElapsedTimeFormat;
            cell.Style.Font.Color.SetColor(Color.Blue);
        }

        private void RenderRatesBlock(ExcelWorksheet worksheet, int startRow, decimal monthlyRate, decimal dailyRate, decimal hourlyRate)
        {
            void RenderRateRow(int rowIndex, string label, decimal value, bool highlight)
            {
                worksheet.Cells[$"A{rowIndex}:B{rowIndex}"].Merge = true;
                worksheet.Cells[$"A{rowIndex}"].Value = label;

                var valueCell = worksheet.Cells[$"C{rowIndex}:D{rowIndex}"];
                valueCell.Merge = true;
                worksheet.Cells[$"C{rowIndex}"].Value = value;
                worksheet.Cells[$"C{rowIndex}"].Style.Numberformat.Format = MoneyFormat;

                if (highlight)
                {
                    worksheet.Cells[$"A{rowIndex}:D{rowIndex}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"A{rowIndex}:D{rowIndex}"].Style.Fill.BackgroundColor.SetColor(HighlightFillColor);
                }

                worksheet.Cells[$"A{rowIndex}:D{rowIndex}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"A{rowIndex}:D{rowIndex}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"A{rowIndex}:D{rowIndex}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"A{rowIndex}:D{rowIndex}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            RenderRateRow(startRow, "Monthly Rate", monthlyRate, highlight: true);
            RenderRateRow(startRow + 1, "Daily Rate", dailyRate, highlight: false);
            RenderRateRow(startRow + 2, "Hourly Rate", hourlyRate, highlight: false);
        }

        private void RenderPayablesBlock(
            ExcelWorksheet worksheet,
            int startRow,
            Paystub paystub,
            IList<TimeLog> timeLogs,
            decimal dailyRate,
            ICollection<AllowanceItem> allowanceItems)
        {
            var daysWorked = timeLogs.Count(t => t.TimeIn.HasValue);
            var daysWorkedPay = daysWorked * dailyRate;

            var totalOvertimeHours =
                paystub.OvertimeHours +
                paystub.RestDayOTHours +
                paystub.SpecialHolidayOTHours +
                paystub.RegularHolidayOTHours +
                paystub.SpecialHolidayRestDayOTHours +
                paystub.RegularHolidayRestDayOTHours;

            var totalOvertimePay =
                paystub.OvertimePay +
                paystub.RestDayOTPay +
                paystub.SpecialHolidayOTPay +
                paystub.RegularHolidayOTPay +
                paystub.SpecialHolidayRestDayOTPay +
                paystub.RegularHolidayRestDayOTPay;

            var wfhAllowanceAmount = allowanceItems?
                .Where(a => a.Allowance?.Product?.Name != null &&
                    a.Allowance.Product.Name.IndexOf("WFH", StringComparison.OrdinalIgnoreCase) >= 0)
                .Sum(a => a.Amount) ?? 0M;

            var rowIndex = startRow;

            worksheet.Cells[$"A{rowIndex}:D{rowIndex}"].Merge = true;
            var payablesHeaderCell = worksheet.Cells[$"A{rowIndex}"];
            payablesHeaderCell.Value = "PAYABLES";
            payablesHeaderCell.Style.Font.Bold = true;
            payablesHeaderCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            payablesHeaderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            payablesHeaderCell.Style.Fill.BackgroundColor.SetColor(HeaderFillColor);
            rowIndex++;

            var payableRows = new List<int>();

            void RenderLineItem(string label, decimal? hours, decimal amount)
            {
                worksheet.Cells[$"A{rowIndex}:B{rowIndex}"].Merge = true;
                worksheet.Cells[$"A{rowIndex}"].Value = label;

                if (hours.HasValue)
                {
                    worksheet.Cells[$"C{rowIndex}"].Value = hours.Value;
                    worksheet.Cells[$"C{rowIndex}"].Style.Numberformat.Format = "0.00";
                }

                worksheet.Cells[$"D{rowIndex}"].Value = amount;
                worksheet.Cells[$"D{rowIndex}"].Style.Numberformat.Format = MoneyFormat;

                rowIndex++;
            }

            RenderLineItem("# of Days Worked", daysWorked, daysWorkedPay);
            payableRows.Add(rowIndex - 1);

            RenderLineItem("Leave (hours) with Pay", paystub.LeaveHours, paystub.LeavePay);
            payableRows.Add(rowIndex - 1);

            RenderLineItem("Regular Hours Overtime", totalOvertimeHours, totalOvertimePay);
            payableRows.Add(rowIndex - 1);

            RenderLineItem("WFH Allowance", null, wfhAllowanceAmount);
            payableRows.Add(rowIndex - 1);

            worksheet.Cells[$"A{rowIndex}:D{rowIndex}"].Merge = true;
            var deductionsHeaderCell = worksheet.Cells[$"A{rowIndex}"];
            deductionsHeaderCell.Value = "DEDUCTIONS";
            deductionsHeaderCell.Style.Font.Bold = true;
            deductionsHeaderCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            deductionsHeaderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            deductionsHeaderCell.Style.Fill.BackgroundColor.SetColor(HeaderFillColor);
            rowIndex++;

            var deductionRows = new List<int>();

            RenderLineItem("Leave (hours) without pay", paystub.AbsentHours, paystub.AbsenceDeduction);
            deductionRows.Add(rowIndex - 1);

            worksheet.Cells[$"A{rowIndex}:C{rowIndex}"].Merge = true;
            worksheet.Cells[$"A{rowIndex}"].Value = "TOTAL DEDUCTIONS";
            worksheet.Cells[$"A{rowIndex}"].Style.Font.Bold = true;
            var totalDeductionsCell = worksheet.Cells[$"D{rowIndex}"];
            totalDeductionsCell.Formula = string.Join("+", deductionRows.Select(r => $"D{r}"));
            totalDeductionsCell.Style.Numberformat.Format = MoneyFormat;
            totalDeductionsCell.Style.Font.Bold = true;
            totalDeductionsCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            var totalDeductionsRow = rowIndex;
            rowIndex += 2;

            worksheet.Cells[$"A{rowIndex}:C{rowIndex}"].Merge = true;
            var totalPayableLabelCell = worksheet.Cells[$"A{rowIndex}"];
            totalPayableLabelCell.Value = "TOTAL AMOUNT PAYABLES:";
            totalPayableLabelCell.Style.Font.Bold = true;
            totalPayableLabelCell.Style.Font.Size = FontSize + 4;

            var totalPayableCell = worksheet.Cells[$"D{rowIndex}"];
            totalPayableCell.Formula = string.Join("+", payableRows.Select(r => $"D{r}")) + $"-D{totalDeductionsRow}";
            totalPayableCell.Style.Numberformat.Format = MoneyFormat;
            totalPayableCell.Style.Font.Bold = true;
            totalPayableCell.Style.Font.Size = FontSize + 4;
            totalPayableCell.Style.Border.Top.Style = ExcelBorderStyle.Double;
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
