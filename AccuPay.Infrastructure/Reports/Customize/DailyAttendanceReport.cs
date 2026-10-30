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
        private const string ShortDateFormat = "d MMM yy";

        // first row of the two-row table header (row 11 holds the table title)
        private const int HeaderStartRow = 12;

        private static readonly Color HighlightFillColor = Color.FromArgb(255, 255, 0);
        private static readonly Color TotalColumnFillColor = Color.FromArgb(255, 153, 204);
        private static readonly Color TimeInFontColor = Color.FromArgb(197, 90, 17);
        private static readonly Color LunchTotalFontColor = Color.FromArgb(192, 0, 0);

        private readonly IPayPeriodRepository _payPeriodRepository;
        private readonly IPaystubRepository _paystubRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public DailyAttendanceReport(
            IPayPeriodRepository payPeriodRepository,
            IPaystubRepository paystubRepository,
            ISalaryRepository salaryRepository,
            ITimeLogRepository timeLogRepository,
            IShiftRepository shiftRepository,
            IOrganizationRepository organizationRepository)
        {
            _payPeriodRepository = payPeriodRepository;
            _paystubRepository = paystubRepository;
            _salaryRepository = salaryRepository;
            _timeLogRepository = timeLogRepository;
            _shiftRepository = shiftRepository;
            _organizationRepository = organizationRepository;
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

            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            var isContractor = organization?.IsContractor ?? false;

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

                    RenderEmployeeSheet(worksheet, employee, payPeriod, paystub, salary, employeeTimeLogs, employeeShiftsByDate, allowanceItems, isContractor);
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
            ICollection<AllowanceItem> allowanceItems,
            bool isContractor)
        {
            worksheet.Cells.Style.Font.Size = FontSize;

            SetColumnWidths(worksheet);

            RenderDateHeader(worksheet, payPeriod);

            RenderBilledToBlock(worksheet, payPeriod);

            RenderTableHeaders(worksheet);

            var timeLogsByDate = timeLogs.ToDictionary(t => t.LogDate.Date);

            var lastDataRow = RenderDailyRows(worksheet, payPeriod, timeLogsByDate, shiftsByDate);

            var monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary);
            var dailyRate = PayrollTools.GetDailyRate(salary, employee);
            var hourlyRate = PayrollTools.GetHourlyRateByDailyRate(salary, employee);

            var ratesStartRow = lastDataRow + 3;
            RenderRatesBlock(worksheet, ratesStartRow, monthlyRate, dailyRate, hourlyRate);

            var payablesStartRow = ratesStartRow + 4;
            RenderPayablesBlock(worksheet, payablesStartRow, paystub, timeLogs, allowanceItems, isContractor);

            SetDefaultPrinterSettings(worksheet.PrinterSettings);
            worksheet.PrinterSettings.Orientation = eOrientation.Portrait;
            worksheet.PrinterSettings.FitToPage = true;
            worksheet.PrinterSettings.FitToWidth = 1;
            worksheet.PrinterSettings.FitToHeight = 0;
            worksheet.PrinterSettings.RepeatRows = worksheet.Cells[$"{HeaderStartRow}:{HeaderStartRow + 1}"];
        }

        private void SetColumnWidths(ExcelWorksheet worksheet)
        {
           // worksheet.Column(1).Width = 9; // Date

            for (var column = 1; column <= 14; column++)
                worksheet.Column(column).Width = 7;
        }

        private void ApplyGrid(ExcelRange range)
        {
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            range.Style.Border.BorderAround(ExcelBorderStyle.Medium);
        }

        private void RenderDateHeader(ExcelWorksheet worksheet, PayPeriod payPeriod)
        {
            void RenderLabel(string address, string text)
            {
                worksheet.Cells[address].Value = text;
                worksheet.Cells[address].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }

            RenderLabel("K2", "DATE:");
            worksheet.Cells["L2:N2"].Merge = true;
            worksheet.Cells["L2:N2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["L2"].Value = payPeriod.PayToDate;
            worksheet.Cells["L2"].Style.Numberformat.Format = ShortDateFormat;
            worksheet.Cells["L2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            RenderLabel("K3", "Date");
            RenderLabel("K4", "Range");
            worksheet.Cells["L4:N4"].Merge = true;
            worksheet.Cells["L4:N4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["L4"].Value = $"{payPeriod.PayFromDate:d MMM yy} - {payPeriod.PayToDate:d MMM yy}";
        }

        private void RenderBilledToBlock(ExcelWorksheet worksheet, PayPeriod payPeriod)
        {
            worksheet.Cells["A6:N6"].Merge = true;
            var titleCell = worksheet.Cells["A6"];
            titleCell.Value = "DAILY ATTENDANCE REPORT (DAR)";
            titleCell.Style.Font.Bold = true;
            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ExcelRange RenderRow(int rowIndex, string label, object value)
            {
                worksheet.Cells[$"A{rowIndex}:B{rowIndex}"].Merge = true;
                worksheet.Cells[$"A{rowIndex}"].Value = label;
                worksheet.Cells[$"A{rowIndex}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                worksheet.Cells[$"C{rowIndex}:N{rowIndex}"].Merge = true;
                var valueCell = worksheet.Cells[$"C{rowIndex}"];
                valueCell.Value = value;
                valueCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                return valueCell;
            }

            RenderRow(7, "Billed To:", ClientName);
            RenderRow(8, "ADDRESS:", ClientAddress);
            RenderRow(9, "Date:", payPeriod.PayToDate).Style.Numberformat.Format = ShortDateFormat;

            var block = worksheet.Cells["A6:N9"];
            block.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            block.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            block.Style.Border.BorderAround(ExcelBorderStyle.Medium);
        }

        private void RenderTableHeaders(ExcelWorksheet worksheet)
        {
            var titleRow = HeaderStartRow - 1;
            var groupRow = HeaderStartRow;
            var subRow = HeaderStartRow + 1;

            worksheet.Cells[$"A{titleRow}:N{titleRow}"].Merge = true;
            var titleCell = worksheet.Cells[$"A{titleRow}"];
            titleCell.Value = "DAILY ATTENDANCE REPORT (DAR)";
            titleCell.Style.Font.Bold = true;
            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            void MergeVertical(string column, string text)
            {
                worksheet.Cells[$"{column}{groupRow}:{column}{subRow}"].Merge = true;
                var cell = worksheet.Cells[$"{column}{groupRow}"];
                cell.Value = text;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
            }

            void MergeHorizontal(string fromColumn, string toColumn, string text)
            {
                worksheet.Cells[$"{fromColumn}{groupRow}:{toColumn}{groupRow}"].Merge = true;
                worksheet.Cells[$"{fromColumn}{groupRow}"].Value = text;
            }

            MergeVertical("A", "Date");
            MergeHorizontal("B", "D", "Regular Hours");
            MergeHorizontal("E", "G", "Overtime Hours");
            MergeHorizontal("H", "I", "LEAVE (IN HOURS)");
            MergeVertical("J", "R E M A R K S");
            MergeVertical("K", "DETAILS");
            MergeHorizontal("L", "N", "Lunch Break");

            worksheet.Cells[$"B{subRow}"].Value = "IN";
            worksheet.Cells[$"C{subRow}"].Value = "OUT";
            worksheet.Cells[$"D{subRow}"].Value = "Total";
            worksheet.Cells[$"E{subRow}"].Value = "IN";
            worksheet.Cells[$"F{subRow}"].Value = "OUT";
            worksheet.Cells[$"G{subRow}"].Value = "Total";
            worksheet.Cells[$"H{subRow}"].Value = "With Pay";
            worksheet.Cells[$"I{subRow}"].Value = "Without Pay";
            worksheet.Cells[$"I{subRow}"].Style.WrapText = true;
            worksheet.Cells[$"L{subRow}"].Value = "OUT";
            worksheet.Cells[$"M{subRow}"].Value = "IN";
            worksheet.Cells[$"N{subRow}"].Value = "TOTAL";

            worksheet.Row(subRow).Height = 24;

            var headerRange = worksheet.Cells[$"A{titleRow}:N{subRow}"];
            headerRange.Style.Font.Bold = true;
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            var totalColumnHeader = worksheet.Cells[$"N{subRow}"];
            totalColumnHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
            totalColumnHeader.Style.Fill.BackgroundColor.SetColor(TotalColumnFillColor);
            totalColumnHeader.Style.Font.Color.SetColor(LunchTotalFontColor);
        }

        private int RenderDailyRows(
            ExcelWorksheet worksheet,
            PayPeriod payPeriod,
            IDictionary<DateTime, TimeLog> timeLogsByDate,
            IDictionary<DateTime, Shift> shiftsByDate)
        {
            var rowIndex = HeaderStartRow + 2;

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

            // trailing empty row
            var lastRow = rowIndex;

            ApplyGrid(worksheet.Cells[$"A{HeaderStartRow - 1}:N{lastRow}"]);

            return lastRow;
        }

        private void RenderDailyRow(ExcelWorksheet worksheet, int rowIndex, DateTime date, TimeLog timeLog, Shift shift)
        {
            var dateCell = worksheet.Cells[$"A{rowIndex}"];
            dateCell.Value = date;
            dateCell.Style.Numberformat.Format = "d MMM";
            dateCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            SetTimeCell(worksheet.Cells[$"B{rowIndex}"], timeLog?.TimeIn);
            worksheet.Cells[$"B{rowIndex}"].Style.Font.Color.SetColor(TimeInFontColor);
            SetTimeCell(worksheet.Cells[$"C{rowIndex}"], timeLog?.TimeOut);
            SetElapsedFormula(worksheet.Cells[$"D{rowIndex}"], $"IF(OR(B{rowIndex}=\"\",C{rowIndex}=\"\"),\"\",(C{rowIndex}-B{rowIndex})-N{rowIndex})");

            SetElapsedFormula(worksheet.Cells[$"G{rowIndex}"], $"IF(OR(E{rowIndex}=\"\",F{rowIndex}=\"\"),\"\",F{rowIndex}-E{rowIndex})");

            var (lunchOut, lunchIn) = GetLunchTimes(timeLog, shift);

            SetTimeCell(worksheet.Cells[$"L{rowIndex}"], lunchOut);
            SetTimeCell(worksheet.Cells[$"M{rowIndex}"], lunchIn);
            SetElapsedFormula(worksheet.Cells[$"N{rowIndex}"], $"IF(OR(L{rowIndex}=\"\",M{rowIndex}=\"\"),0,M{rowIndex}-L{rowIndex})");
            worksheet.Cells[$"N{rowIndex}"].Style.Font.Color.SetColor(LunchTotalFontColor);
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
        }

        private void RenderLabelCell(ExcelWorksheet worksheet, int rowIndex, string label)
        {
            worksheet.Cells[$"A{rowIndex}:B{rowIndex}"].Merge = true;
            var labelCell = worksheet.Cells[$"A{rowIndex}"];
            labelCell.Value = label;
            labelCell.Style.Font.Bold = true;
            labelCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        }

        private ExcelRange RenderMergedAmountCell(ExcelWorksheet worksheet, int rowIndex)
        {
            worksheet.Cells[$"D{rowIndex}:E{rowIndex}"].Merge = true;
            var amountCell = worksheet.Cells[$"D{rowIndex}"];
            amountCell.Style.Numberformat.Format = MoneyFormat;
            amountCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            return amountCell;
        }

        private void RenderSectionHeader(ExcelWorksheet worksheet, int rowIndex, string text)
        {
            worksheet.Cells[$"A{rowIndex}:E{rowIndex}"].Merge = true;
            var headerCell = worksheet.Cells[$"A{rowIndex}"];
            headerCell.Value = text;
            headerCell.Style.Font.Bold = true;
            headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private void RenderRatesBlock(ExcelWorksheet worksheet, int startRow, decimal monthlyRate, decimal dailyRate, decimal hourlyRate)
        {
            void RenderRateRow(int rowIndex, string label, decimal value, bool highlight)
            {
                RenderLabelCell(worksheet, rowIndex, label);

                var valueCell = RenderMergedAmountCell(worksheet, rowIndex);
                valueCell.Value = value;

                if (highlight)
                {
                    worksheet.Cells[$"C{rowIndex}:E{rowIndex}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"C{rowIndex}:E{rowIndex}"].Style.Fill.BackgroundColor.SetColor(HighlightFillColor);
                }
            }

            RenderRateRow(startRow, "Monthly Rate", monthlyRate, highlight: true);
            RenderRateRow(startRow + 1, "Daily Rate", dailyRate, highlight: false);
            RenderRateRow(startRow + 2, "Hourly Rate", hourlyRate, highlight: false);

            ApplyGrid(worksheet.Cells[$"A{startRow}:E{startRow + 2}"]);
        }

        private void RenderPayablesBlock(
            ExcelWorksheet worksheet,
            int startRow,
            Paystub paystub,
            IList<TimeLog> timeLogs,
            ICollection<AllowanceItem> allowanceItems,
            bool isContractor)
        {
            var daysWorked = timeLogs.Count(t => t.TimeIn.HasValue);

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

            var wfhAllowanceAmount = paystub.TotalNonTaxableAllowance;

            var rowIndex = startRow;

            RenderSectionHeader(worksheet, rowIndex, "PAYABLES");
            rowIndex++;

            var payableCells = new List<string>();
            var deductionCells = new List<string>();

            void RenderLineItem(string label, decimal count, string countFormat, decimal amount, List<string> cells)
            {
                RenderLabelCell(worksheet, rowIndex, label);
                worksheet.Cells[$"D{rowIndex}:E{rowIndex}"].Merge = true;
                worksheet.Cells[$"C{rowIndex}"].Value = count;
                worksheet.Cells[$"C{rowIndex}"].Style.Numberformat.Format = countFormat;

                worksheet.Cells[$"D{rowIndex}"].Value = amount;
                worksheet.Cells[$"D{rowIndex}"].Style.Numberformat.Format = MoneyFormat;
                worksheet.Cells[$"D{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cells.Add($"D{rowIndex}");
                rowIndex++;
            }

            void RenderAmountItem(string label, decimal amount, List<string> cells)
            {
                RenderLabelCell(worksheet, rowIndex, label);
                RenderMergedAmountCell(worksheet, rowIndex).Value = amount;

                cells.Add($"C{rowIndex}");
                rowIndex++;
            }

            RenderLineItem("# of Days Worked", daysWorked, "0", paystub.BasicPay, payableCells);
            RenderLineItem("Leave (hours) with Pay", paystub.LeaveHours, "0.00", paystub.LeavePay, payableCells);
            RenderLineItem("Regular Hours Overtime", totalOvertimeHours, "0.00", totalOvertimePay, payableCells);
            RenderAmountItem("WFH Allowance", wfhAllowanceAmount, payableCells);

            ApplyGrid(worksheet.Cells[$"A{startRow}:E{rowIndex - 1}"]);

            // empty separator row
            ApplyGrid(worksheet.Cells[$"A{rowIndex}:E{rowIndex}"]);
            rowIndex++;

            var deductionsStartRow = rowIndex;

            // Contractor employees are self-employed, so the contributions shown are the employer share.
            RenderAmountItem("SSS", isContractor ? paystub.SssEmployerShare : paystub.SssEmployeeShare, deductionCells);
            RenderAmountItem("PHIC", isContractor ? paystub.PhilHealthEmployerShare : paystub.PhilHealthEmployeeShare, deductionCells);
            RenderAmountItem("PAGIBIG", isContractor ? paystub.HdmfEmployerShare : paystub.HdmfEmployeeShare, deductionCells);
            RenderAmountItem("W/TAX", paystub.WithholdingTax, deductionCells);

            RenderSectionHeader(worksheet, rowIndex, "DEDUCTIONS");
            rowIndex++;

            RenderLineItem("Leave (hours) without Pay", paystub.AbsentHours, "0.00", paystub.AbsenceDeduction, deductionCells);

            RenderLabelCell(worksheet, rowIndex, "TOTAL DEDUCTIONS");
            var totalDeductionsCell = RenderMergedAmountCell(worksheet, rowIndex);
            totalDeductionsCell.Formula = string.Join("+", deductionCells);
            totalDeductionsCell.Style.Font.Bold = true;
            var totalDeductionsRow = rowIndex;
            rowIndex++;

            RenderLabelCell(worksheet, rowIndex, "Gross Amount");
            var grossAmountCell = RenderMergedAmountCell(worksheet, rowIndex);
            grossAmountCell.Value = paystub.NetPay;
            grossAmountCell.Style.Font.Bold = true;

            ApplyGrid(worksheet.Cells[$"A{deductionsStartRow}:E{rowIndex}"]);
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
