using AccuPay.Core;
using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities;
using AccuPay.Utilities.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Infrastructure.Reports.ExcelReportColumn;

namespace AccuPay.Infrastructure.Reports
{
    public class PayrollSummaryReportBuilder : ExcelFormatReport, IPayrollSummaryReportBuilder
    {
        public string Name { get; set; } = "Payroll Summary";
        public bool IsHidden { get; set; } = false;

        private const string adjustmentColumn = "(Adj.)";

        private const string totalAdjustmentColumn = "Adj.";

        private const string EmployeeRowIDColumnName = "EmployeeRowID";

        private readonly IReadOnlyCollection<ExcelReportColumn> _reportColumns;

        private readonly ListOfValueCollection _settings;
        private readonly OrganizationRepository _organizationRepository;
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly PaystubDataService _paystubDataService;
        private readonly SystemOwnerService _systemOwnerService;
        private readonly PayrollSummaryExcelFormatReportDataService _reportDataService;

        public PayrollSummaryReportBuilder(
            OrganizationRepository organizationRepository,
            PayPeriodRepository payPeriodRepository,
            PaystubDataService paystubDataService,
            SystemOwnerService systemOwnerService,
            ListOfValueService listOfValueService,
            PayrollSummaryExcelFormatReportDataService reportDataService)
        {
            _organizationRepository = organizationRepository;
            _payPeriodRepository = payPeriodRepository;
            _paystubDataService = paystubDataService;
            _systemOwnerService = systemOwnerService;
            _reportDataService = reportDataService;
            _settings = listOfValueService.Create();

            _reportColumns = GetReportColumns();
        }

        private ReadOnlyCollection<ExcelReportColumn> GetReportColumns()
        {
            var allowanceColumnName = "Allowance";

            var reportColumns = new List<ExcelReportColumn>()
            {
                new ExcelReportColumn("Code", "DatCol2", ColumnType.Text),
                new ExcelReportColumn("Full Name", "DatCol3", ColumnType.Text),
                new ExcelReportColumn("Rate", "Rate"),
                new ExcelReportColumn("Basic Hours", "BasicHours"),
                new ExcelReportColumn("Basic Pay", "BasicPay"),
                new ExcelReportColumn("Reg Hrs", "RegularHours"),
                new ExcelReportColumn("Reg Pay", "RegularPay"),
                new ExcelReportColumn("OT Hrs", "OvertimeHours", optional: true),
                new ExcelReportColumn("OT Pay", "OvertimePay", optional: true),
                new ExcelReportColumn("ND Hrs", "NightDiffHours", optional: true),
                new ExcelReportColumn("ND Pay", "NightDiffPay", optional: true),
                new ExcelReportColumn("NDOT Hrs", "NightDiffOvertimeHours", optional: true),
                new ExcelReportColumn("NDOT Pay", "NightDiffOvertimePay", optional: true),
                new ExcelReportColumn("R.Day Hrs", "RestDayHours", optional: true),
                new ExcelReportColumn("R.Day Pay", "RestDayPay", optional: true),
                new ExcelReportColumn("R.DayOT Hrs", "RestDayOTHours", optional: true),
                new ExcelReportColumn("R.DayOT Pay", "RestDayOTPay", optional: true),
                new ExcelReportColumn("R.Day ND Hrs", "RestDayNightDiffHours", optional: true),
                new ExcelReportColumn("R.Day ND Pay", "RestDayNightDiffPay", optional: true),
                new ExcelReportColumn("R.Day NDOT Hrs", "RestDayNightDiffOTHours", optional: true),
                new ExcelReportColumn("R.Day NDOT Pay", "RestDayNightDiffOTPay", optional: true),
                new ExcelReportColumn("S.Hol Hrs", "SpecialHolidayHours", optional: true),
                new ExcelReportColumn("S.Hol Pay", "SpecialHolidayPay", optional: true),
                new ExcelReportColumn("S.HolOT Hrs", "SpecialHolidayOTHours", optional: true),
                new ExcelReportColumn("S.HolOT Pay", "SpecialHolidayOTPay", optional: true),
                new ExcelReportColumn("S.Hol ND Hrs", "SpecialHolidayNightDiffHours", optional: true),
                new ExcelReportColumn("S.Hol ND Pay", "SpecialHolidayNightDiffPay", optional: true),
                new ExcelReportColumn("S.Hol NDOT Hrs", "SpecialHolidayNightDiffOTHours", optional: true),
                new ExcelReportColumn("S.Hol NDOT Pay", "SpecialHolidayNightDiffOTPay", optional: true),
                new ExcelReportColumn("S.Hol R.Day Hrs", "SpecialHolidayRestDayHours", optional: true),
                new ExcelReportColumn("S.Hol R.Day Pay", "SpecialHolidayRestDayPay", optional: true),
                new ExcelReportColumn("S.Hol R.DayOT Hrs", "SpecialHolidayRestDayOTHours", optional: true),
                new ExcelReportColumn("S.Hol R.DayOT Pay", "SpecialHolidayRestDayOTPay", optional: true),
                new ExcelReportColumn("S.Hol R.Day ND Hrs", "SpecialHolidayRestDayNightDiffHours", optional: true),
                new ExcelReportColumn("S.Hol R.Day ND Pay", "SpecialHolidayRestDayNightDiffPay", optional: true),
                new ExcelReportColumn("S.Hol R.Day NDOT Hrs", "SpecialHolidayRestDayNightDiffOTHours", optional: true),
                new ExcelReportColumn("S.Hol R.Day NDOT Pay", "SpecialHolidayRestDayNightDiffOTPay", optional: true),
                new ExcelReportColumn("R.Hol Hrs", "RegularHolidayHours", optional: true),
                new ExcelReportColumn("R.Hol Pay", "RegularHolidayPay", optional: true),
                new ExcelReportColumn("R.HolOT Hrs", "RegularHolidayOTHours", optional: true),
                new ExcelReportColumn("R.HolOT Pay", "RegularHolidayOTPay", optional: true),
                new ExcelReportColumn("R.Hol ND Hrs", "RegularHolidayNightDiffHours", optional: true),
                new ExcelReportColumn("R.Hol ND Pay", "RegularHolidayNightDiffPay", optional: true),
                new ExcelReportColumn("R.Hol NDOT Hrs", "RegularHolidayNightDiffOTHours", optional: true),
                new ExcelReportColumn("R.Hol NDOT Pay", "RegularHolidayNightDiffOTPay", optional: true),
                new ExcelReportColumn("R.Hol R.Day Hrs", "RegularHolidayRestDayHours", optional: true),
                new ExcelReportColumn("R.Hol R.Day Pay", "RegularHolidayRestDayPay", optional: true),
                new ExcelReportColumn("R.Hol R.DayOT Hrs", "RegularHolidayRestDayOTHours", optional: true),
                new ExcelReportColumn("R.Hol R.DayOT Pay", "RegularHolidayRestDayOTPay", optional: true),
                new ExcelReportColumn("R.Hol R.Day ND Hrs", "RegularHolidayRestDayNightDiffHours", optional: true),
                new ExcelReportColumn("R.Hol R.Day ND Pay", "RegularHolidayRestDayNightDiffPay", optional: true),
                new ExcelReportColumn("R.Hol R.Day NDOT Hrs", "RegularHolidayRestDayNightDiffOTHours", optional: true),
                new ExcelReportColumn("R.Hol R.Day NDOT Pay", "RegularHolidayRestDayNightDiffOTPay", optional: true),
                new ExcelReportColumn("Leave Hrs", "LeaveHours", optional: true),
                new ExcelReportColumn("Leave Pay", "LeavePay", optional: true),
                new ExcelReportColumn("Late Hrs", "LateHours", optional: true),
                new ExcelReportColumn("Late Amt", "LateDeduction", optional: true),
                new ExcelReportColumn("UT Hrs", "UndertimeHours", optional: true),
                new ExcelReportColumn("UT Amt", "UndertimeDeduction", optional: true),
                new ExcelReportColumn("Absent Hrs", "AbsentHours", optional: true),
                new ExcelReportColumn("Absent Amt", "AbsentDeduction", optional: true),
                new ExcelReportColumn(allowanceColumnName, "TotalAllowance"),
                new ExcelReportColumn("Bonus", "TotalBonus", optional: true),
                new ExcelReportColumn("Gross", "GrossIncome"),
                new ExcelReportColumn("SSS", "SSS", optional: true),
                new ExcelReportColumn("Ph.Health", "PhilHealth", optional: true),
                new ExcelReportColumn("HDMF", "HDMF", optional: true),
                new ExcelReportColumn("Taxable", "TaxableIncome"),
                new ExcelReportColumn("W.Tax", "WithholdingTax"),
                new ExcelReportColumn("Loan", "TotalLoans"),
                new ExcelReportColumn("A.Fee", "AgencyFee", optional: true),
                new ExcelReportColumn(totalAdjustmentColumn, "TotalAdjustments", optional: true),
                new ExcelReportColumn("Net Pay", "NetPay"),
                new ExcelReportColumn("13th Month", "13thMonthPay"),
                new ExcelReportColumn("Total", "Total")
            };

            if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
            {
                var allowanceColumn = reportColumns
                    .Where(r => r.Name == allowanceColumnName)
                    .FirstOrDefault();

                if (allowanceColumn != null)
                    allowanceColumn.Name = "Ecola";
            }

            return new ReadOnlyCollection<ExcelReportColumn>(reportColumns);
        }

        public async Task CreateReport(
            bool keepInOneSheet,
            bool hideEmptyColumns,
            int organizationId,
            int payPeriodFromId,
            int payPeriodToId,
            string salaryDistributionType,
            bool isActual,
            string saveFilePath)
        {
            var payPeriod = await GetSelectedPayPeriod(
                payPeriodFromId: payPeriodFromId,
                payPeriodToId: payPeriodToId);

            var organization = await _organizationRepository.GetByIdAsync(organizationId);

            var distributionTypes = new string[] {
                PayrollSummaryCategory.Cash.ToTrimmedLowerCase(),
                PayrollSummaryCategory.DirectDeposit.ToTrimmedLowerCase(),
                PayrollSummaryCategory.All.ToTrimmedLowerCase() };

            if (distributionTypes.Contains(salaryDistributionType.ToTrimmedLowerCase()) == false)
            {
                // this means that this will show all, no filters
                salaryDistributionType = null;
            }

            if (organization == null)
                throw new BusinessLogicException("Organization does not exists.");

            var employeeTable = _reportDataService.GetData(
                organizationId: organizationId,
                payPeriodFromId: payPeriodFromId,
                payPeriodToId: payPeriodToId,
                salaryDistributionType: salaryDistributionType,
                Convert.ToInt16(isActual),
                keepInOneSheet);

            var allEmployees = employeeTable.Rows.OfType<DataRow>().ToList();

            if (allEmployees.Count <= 0)
                throw new BusinessLogicException("No paystubs to show.");

            string reportName = "PayrollSummary";

            string[] short_dates = new string[] { payPeriod.DateFrom.ToShortDateString(), payPeriod.DateTo.ToShortDateString() };

            var newFile = new FileInfo(saveFilePath);

            var viewableReportColumns = await GetViewableReportColumns(allEmployees, hideEmptyColumns, organizationId, payPeriod, isActual);

            var employeeGroups = GroupEmployees(allEmployees);

            using (var excel = new ExcelPackage(newFile))
            {
                var subTotalRows = new List<int>();

                if (keepInOneSheet)
                {
                    var worksheet = excel.Workbook.Worksheets.Add(reportName);

                    RenderWorksheet(worksheet, employeeGroups, short_dates, viewableReportColumns, payPeriod, organization);
                }
                else
                    foreach (var employeeGroup in employeeGroups)
                    {
                        var worksheet = excel.Workbook.Worksheets.Add(employeeGroup.DivisionName);

                        var currentGroup = new Collection<EmployeeGroup>()
                        {
                            employeeGroup
                        };

                        RenderWorksheet(worksheet, currentGroup, short_dates, viewableReportColumns, payPeriod, organization);
                    }

                excel.Save();
            }
        }

        private async Task<SelectedPayPeriod> GetSelectedPayPeriod(int payPeriodFromId, int payPeriodToId)
        {
            PayPeriod payPeriodFrom = await _payPeriodRepository.GetByIdAsync(payPeriodFromId);
            PayPeriod payPeriodTo;

            if (payPeriodFrom == null)
                throw new Exception("PayPeriodFrom is required.");

            if (payPeriodFromId == payPeriodToId)
            {
                payPeriodTo = payPeriodFrom;
            }
            else
            {
                payPeriodTo = await _payPeriodRepository.GetByIdAsync(payPeriodToId);
            }

            if (payPeriodTo == null)
                throw new Exception("PayPeriodFrom is required.");

            return new SelectedPayPeriod(from: payPeriodFrom, to: payPeriodTo);
        }

        private void RenderWorksheet(
            ExcelWorksheet worksheet,
            ICollection<EmployeeGroup> employeeGroups,
            string[] short_dates,
            IReadOnlyCollection<ExcelReportColumn> viewableReportColumns,
            SelectedPayPeriod payPeriod,
            Organization organization)
        {
            var subTotalRows = new List<int>();

            worksheet.Cells.Style.Font.Size = FontSize;

            if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
                worksheet.Cells.Style.Font.Name = "Book Antiqua";

            var organizationCell = worksheet.Cells[1, 1];
            organizationCell.Value = organization.Name.ToUpper();
            organizationCell.Style.Font.Bold = true;

            var attendancePeriodCell = worksheet.Cells[2, 1];
            var attendancePeriodDescription = $"For the period of {short_dates[0]} to {short_dates[1]}";
            attendancePeriodCell.Value = attendancePeriodDescription;

            var lastCell = string.Empty;
            int rowIndex = 4;

            if (ShowCoveredPeriod())
            {
                attendancePeriodCell.Value = $"Attendance Period: {short_dates[0]} to {short_dates[1]}";

                var payFromNextCutOff = _payPeriodRepository.GetNextPayPeriod(
                    payPeriodId: payPeriod.FromId,
                    organizationId: organization.RowID.Value);
                var payToNextCutOff = _payPeriodRepository.GetNextPayPeriod(
                    payPeriodId: payPeriod.ToId,
                    organizationId: organization.RowID.Value);

                var payrollPeriodCell = worksheet.Cells[3, 1];
                var payrollPeriodDescription = $"Payroll Period: {(payFromNextCutOff?.PayFromDate == null ? "" : payFromNextCutOff.PayFromDate.ToShortDateString())} to {(payToNextCutOff?.PayToDate == null ? "" : payToNextCutOff.PayToDate.ToShortDateString())}";
                payrollPeriodCell.Value = payrollPeriodDescription;

                rowIndex = 5;
            }

            foreach (var employeeGroup in employeeGroups)
            {
                var divisionCell = worksheet.Cells[rowIndex, 1];
                divisionCell.Value = employeeGroup.DivisionName;
                divisionCell.Style.Font.Italic = true;

                rowIndex += 1;

                RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns);

                rowIndex += 1;

                var employeesStartIndex = rowIndex;
                var employeesLastIndex = 0;

                foreach (var employee in employeeGroup.Employees)
                {
                    var letters = GenerateAlphabet().GetEnumerator();

                    foreach (var reportColumn in viewableReportColumns)
                    {
                        letters.MoveNext();

                        var alphabet = letters.Current;

                        var column = $"{alphabet}{rowIndex}";

                        var cell = worksheet.Cells[column];
                        var sourceName = reportColumn.Source;
                        cell.Value = GetCellValue(employee, sourceName);

                        if (reportColumn.Type == ColumnType.Numeric)
                        {
                            cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                    }

                    lastCell = letters.Current;

                    employeesLastIndex = rowIndex;
                    rowIndex += 1;
                }

                var subTotalCellRange = $"C{rowIndex}:{lastCell}{rowIndex}";

                subTotalRows.Add(rowIndex);

                RenderSubTotal(worksheet, subTotalCellRange, employeesStartIndex, employeesLastIndex, formulaColumnStart: 3);

                rowIndex += 2;
            }

            worksheet.Cells.AutoFitColumns();
            worksheet.Cells["A1"].AutoFitColumns(4.9, 5.3);

            rowIndex += 1;

            if (employeeGroups.Count > 1)
                RenderGrandTotal(worksheet, rowIndex, ThirdColumn, lastCell, subTotalRows);

            rowIndex += 1;

            RenderSignatureFields(worksheet, rowIndex);
            SetDefaultPrinterSettings(worksheet.PrinterSettings);
        }

        private object GetCellValue(DataRow employee, string sourceName)
        {
            if (sourceName.EndsWith(adjustmentColumn) && GetPayrollSummaryAdjustmentBreakdownPolicy() != PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly)
            {
                if (_adjustments == null)
                    return 0;

                var productName = GetAdjustmentColumnFromName(sourceName);
                var employeeId = ObjectUtils.ToNullableInteger(employee[EmployeeRowIDColumnName]);

                var adjustment = _adjustments
                    .Where(a => a.Product.PartNo == productName)
                    .Where(a => a.Paystub.EmployeeID.Value == employeeId.Value)
                    .Where(a => a.Paystub.RowID.Value == ObjectUtils.ToInteger(employee["PaystubId"]))
                    .Sum(a => a.Amount);

                return adjustment;
            }

            return employee[sourceName];
        }

        private async Task<IReadOnlyCollection<ExcelReportColumn>> GetViewableReportColumns(
            ICollection<DataRow> allEmployees,
            bool hideEmptyColumns,
            int organizationId,
            SelectedPayPeriod payPeriod,
            bool isActual)
        {
            var viewableReportColumns = new List<ExcelReportColumn>();
            foreach (var reportColumn in _reportColumns)
            {
                if (reportColumn.Optional && hideEmptyColumns)
                {
                    var hasValueQuery = allEmployees
                        .Where(row => row[reportColumn.Source] != null);

                    if (reportColumn.Type == ColumnType.Numeric)
                    {
                        hasValueQuery = hasValueQuery
                            .Where(row => ObjectUtils.ToDecimal(row[reportColumn.Source]) != 0);
                    }

                    if (hasValueQuery.Any())
                        viewableReportColumns.Add(reportColumn);
                }
                else
                    viewableReportColumns.Add(reportColumn);
            }

            if (GetPayrollSummaryAdjustmentBreakdownPolicy() != PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly)
                await AddAdjustmentBreakdownColumns(allEmployees, viewableReportColumns, payPeriod, organizationId, isActual);

            return viewableReportColumns;
        }

        private void RenderSignatureFields(ExcelWorksheet worksheet, int startIdx)
        {
            int index = (startIdx + 1);

            {
                var withBlock = worksheet;
                withBlock.Cells[$"A{index}"].Value = "Prepared by: ";
                withBlock.Cells[$"A{index}:B{index}"].Merge = true;

                index += 1;
                withBlock.Cells[$"A{index}"].Value = "Audited by: ";
                withBlock.Cells[$"A{index}:B{index}"].Merge = true;

                index += 1;
                withBlock.Cells[$"A{index}"].Value = "Approved by: ";
                withBlock.Cells[$"A{index}:B{index}"].Merge = true;
            }
        }

        private ICollection<EmployeeGroup> GroupEmployees(ICollection<DataRow> allEmployees)
        {
            var employeesByGroups = allEmployees.GroupBy(r => r["DivisionID"].ToString());

            var groups = new Collection<EmployeeGroup>();

            foreach (var employeesByGroup in employeesByGroups)
            {
                var group = new EmployeeGroup()
                {
                    Employees = employeesByGroup.ToList()
                };

                var employee = group.Employees.FirstOrDefault();
                group.DivisionName = employee["DatCol1"].ToString();

                groups.Add(group);
            }

            foreach (var groupsWithSameDivision in groups.GroupBy(g => g.DivisionName))
            {
                if (groupsWithSameDivision.Count() > 1)
                {
                    var index = 1;
                    foreach (var groupInSameDivision in groupsWithSameDivision)
                    {
                        groupInSameDivision.DivisionName = $"{groupInSameDivision.DivisionName}-{index}";
                        index += 1;
                    }
                }
            }

            return groups;
        }

        private bool ShowCoveredPeriod()
        {
            return _settings.GetBoolean("Payroll Summary Policy.ShowCoveredPeriod", false);
        }

        private List<IAdjustment> _adjustments;

        private PayrollSummaryAdjustmentBreakdownPolicy GetPayrollSummaryAdjustmentBreakdownPolicy()
        {
            return _settings.GetEnum("Payroll Summary Policy.AdjustmentBreakdown", PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly);
        }

        private async Task AddAdjustmentBreakdownColumns(
            ICollection<DataRow> allEmployees,
            List<ExcelReportColumn> viewableReportColumns,
            SelectedPayPeriod payPeriod,
            int organizationId,
            bool isActual)
        {
            var adjustments = await GetCurrentAdjustments(payPeriod, organizationId, allEmployees, isActual);

            if (!adjustments.Any()) return;

            var groupedAdjustments = adjustments.GroupBy(a => a.ProductID).ToList();

            var totalAdjustmentReportColumn = viewableReportColumns.Where(r => r.Name == totalAdjustmentColumn).FirstOrDefault();
            var totalAdjustmentColumnIndex = viewableReportColumns.IndexOf(totalAdjustmentReportColumn);

            var counter = 1;

            // add breakdown columns
            foreach (var adjustment in groupedAdjustments)
            {
                var adjustmentName = GetAdjustmentName(adjustment.ToList()[0].Product?.Name);

                if (string.IsNullOrWhiteSpace(adjustmentName))
                    continue;

                viewableReportColumns.Insert(totalAdjustmentColumnIndex + counter, new ExcelReportColumn(adjustmentName, adjustmentName));

                counter += 1;
            }

            if (totalAdjustmentColumnIndex >= 0 && totalAdjustmentColumnIndex < viewableReportColumns.Count)

                // remove total adjustments column
                viewableReportColumns.RemoveAt(totalAdjustmentColumnIndex);

            // add back the total adjustment column if it is not BreakdownOnly
            // this will put the column after the adjustment breakdown columns
            if (GetPayrollSummaryAdjustmentBreakdownPolicy() != PayrollSummaryAdjustmentBreakdownPolicy.BreakdownOnly)
            {
                if (GetPayrollSummaryAdjustmentBreakdownPolicy() == PayrollSummaryAdjustmentBreakdownPolicy.Both)
                    totalAdjustmentReportColumn.Name = "Total Adj.";

                if (counter > 1)
                    // we have minus 1 here because we remove the original total adjustment column
                    viewableReportColumns.Insert(totalAdjustmentColumnIndex + counter - 1, totalAdjustmentReportColumn);
                else
                    viewableReportColumns.Insert(totalAdjustmentColumnIndex, totalAdjustmentReportColumn);
            }
        }

        private async Task<List<IAdjustment>> GetCurrentAdjustments(SelectedPayPeriod payPeriod, int organizationId, ICollection<DataRow> allEmployees = null, bool isActual = false)
        {
            if (payPeriod.DateFrom == null || payPeriod.DateTo == null)
                throw new ArgumentException("Cannot fetch pay period data.");

            var employeeIds = GetEmployeeIds(allEmployees.ToList());
            var datePeriod = new TimePeriod(payPeriod.DateFrom, payPeriod.DateTo);

            _adjustments = (await _paystubDataService.GetAdjustmentsByEmployeeAndDatePeriodAsync(
                organizationId,
                employeeIds,
                datePeriod,
                isActual
            )).ToList();

            return _adjustments;
        }

        private static string GetAdjustmentName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return $"{name} {adjustmentColumn}";
        }

        private static string GetAdjustmentColumnFromName(string column)
        {
            return column.Replace($" {adjustmentColumn}", "");
        }

        private int[] GetEmployeeIds(IList<DataRow> allEmployees)
        {
            List<int> employeeIds = new List<int>();

            for (var index = 0; index <= allEmployees.Count - 1; index++)
            {
                var employeeId = ObjectUtils.ToNullableInteger(allEmployees[index][EmployeeRowIDColumnName]);

                if (employeeId == null)
                    continue;

                employeeIds.Add(employeeId.Value);
            }

            return employeeIds.ToArray();
        }

        private class EmployeeGroup
        {
            public string DivisionName { get; set; }

            public ICollection<DataRow> Employees { get; set; }
        }

        private class SelectedPayPeriod
        {
            public PayPeriod From { get; set; }
            public PayPeriod To { get; set; }

            public int FromId => From.RowID.Value;
            public int ToId => To.RowID.Value;

            public DateTime DateFrom => From.PayFromDate;
            public DateTime DateTo => To.PayToDate;

            public SelectedPayPeriod(PayPeriod from, PayPeriod to)
            {
                From = from;
                To = to;
            }
        }
    }
}