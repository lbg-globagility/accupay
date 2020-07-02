using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using AccuPay.Data;
using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.ValueObjects;
using AccuPay.Desktop.Utilities;
using AccuPay.ExcelReportColumn;
using AccuPay.Helpers;
using AccuPay.Utilities;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace AccuPay.Infrastructure.Reports { 

public class PayrollSummaryExcelFormatReportProvider : ExcelFormatReport
{
    public string Name { get; set; } = "Payroll Summary";
    public bool IsHidden { get; set; } = false;

    private const string adjustmentColumn = "(Adj.)";

    private const string totalAdjustmentColumn = "Adj.";

    private readonly IReadOnlyCollection<ExcelReportColumn> _reportColumns = GetReportColumns();

    private const string EmployeeRowIDColumnName = "EmployeeRowID";

    private ListOfValueCollection _settings;

    private PayPeriodRepository _payPeriodRepository;

    private AdjustmentService _adjustmentService;

    public bool IsActual { get; set; }

    public PayrollSummaryExcelFormatReportProvider()
    {
        _payPeriodRepository = MainServiceProvider.GetRequiredService<PayPeriodRepository>;

        _adjustmentService = MainServiceProvider.GetRequiredService<AdjustmentService>;

        _settings = MainServiceProvider.GetRequiredService<ListOfValueService>.Create();
    }

    private static ReadOnlyCollection<ExcelReportColumn> GetReportColumns()
    {
        var allowanceColumnName = "Allowance";

        var reportColumns = new List<ExcelReportColumn>(
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
        });

        var systemOwnerService = MainServiceProvider.GetRequiredService<SystemOwnerService>;
        if (systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
        {
            var allowanceColumn = reportColumns.Where(r => r.Name == allowanceColumnName).FirstOrDefault;

            if (allowanceColumn != null)
                allowanceColumn.Name = "Ecola";
        }

        return new ReadOnlyCollection<ExcelReportColumn>(reportColumns);
    }

    public async void Run()
    {
        short bool_result = Convert.ToInt16(IsActual);

        var payrollSelector = GetPayrollSelector();
        if (payrollSelector == null)
            return;

        var keepInOneSheet = Convert.ToBoolean(ExcelOptionFormat());

        var parameters = new object[] {
            orgztnID,
            payrollSelector.PayPeriodFromID,
            payrollSelector.PayPeriodToID,
            bool_result,
            payrollSelector.cboStringParameter.Text,
            keepInOneSheet
        };

        var hideEmptyColumns = payrollSelector.chkHideEmptyColumns.Checked;

        try
        {
            var query = new SQL("CALL PAYROLLSUMMARY2(?og_rowid, ?min_pp_rowid, ?max_pp_rowid, ?is_actual, ?salaray_distrib, ?keep_in_onesheet);", parameters);

            var ds = query.GetFoundRows;
            if (query.HasError)
                throw query.ErrorException;

            var employeeTable = ds.Tables.OfType<DataTable>.FirstOrDefault();
            var allEmployees = employeeTable.Rows.OfType<DataRow>.ToList();

            if (allEmployees.Count <= 0)
                throw new Exception("No paystubs to show.");

            string reportName = "PayrollSummary";

            string[] short_dates = new string[] { (DateTime)payrollSelector.DateFrom.ToShortDateString, (DateTime)payrollSelector.DateTo.ToShortDateString };

            var defaultFileName = GetDefaultFileName(reportName, payrollSelector);

            var saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx");

            if (saveFileDialogHelperOutPut.IsSuccess == false)
                return;

            var newFile = saveFileDialogHelperOutPut.FileInfo;

            var viewableReportColumns = await GetViewableReportColumns(allEmployees, hideEmptyColumns, payrollSelector);

            var employeeGroups = GroupEmployees(allEmployees);

            using (var excel = new ExcelPackage(newFile))
            {
                var subTotalRows = new List<int>();

                if (keepInOneSheet)
                {
                    var worksheet = excel.Workbook.Worksheets.Add(reportName);

                    RenderWorksheet(worksheet, employeeGroups, short_dates, viewableReportColumns, payrollSelector);
                }
                else
                    foreach (var employeeGroup in employeeGroups)
                    {
                        var worksheet = excel.Workbook.Worksheets.Add(employeeGroup.DivisionName);

                        var currentGroup = new Collection<EmployeeGroup>()
                        {
                            employeeGroup
                        };

                        RenderWorksheet(worksheet, currentGroup, short_dates, viewableReportColumns, payrollSelector);
                    }

                excel.Save();
            }

            Process.Start(newFile.FullName);
        }
        catch (IOException ex)
        {
            MessageBoxHelper.ErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            MsgBox(getErrExcptn(ex, this.Name));
        }
    }

    private string GetDefaultFileName(string reportName, PayrollSummaDateSelection payrollSelector)
    {
        return string.Concat(orgNam, reportName, payrollSelector.cboStringParameter.Text.Replace(" ", ""), "Report", string.Concat(payrollSelector.DateFrom.Value.ToShortDateString().Replace("/", "-"), "TO", payrollSelector.DateTo.Value.ToShortDateString().Replace("/", "-")), ".xlsx");
    }

    private void RenderWorksheet(ExcelWorksheet worksheet, ICollection<EmployeeGroup> employeeGroups, string[] short_dates, IReadOnlyCollection<ExcelReportColumn> viewableReportColumns, PayrollSummaDateSelection PayrollSummaDateSelection)
    {
        var subTotalRows = new List<int>();

        worksheet.Cells.Style.Font.Size = FontSize;

        var systemOwnerService = MainServiceProvider.GetRequiredService<SystemOwnerService>;

        if (systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
            worksheet.Cells.Style.Font.Name = "Book Antiqua";

        var organizationCell = worksheet.Cells(1, 1);
        organizationCell.Value = orgNam.ToUpper();
        organizationCell.Style.Font.Bold = true;

        var attendancePeriodCell = worksheet.Cells(2, 1);
        var attendancePeriodDescription = $"For the period of {short_dates[0]} to {short_dates[1]}";
        attendancePeriodCell.Value = attendancePeriodDescription;

        var lastCell = string.Empty;
        int rowIndex = 4;

        if (ShowCoveredPeriod())
        {
            attendancePeriodCell.Value = $"Attendance Period: {short_dates[0]} to {short_dates[1]}";

            var payFromNextCutOff = _payPeriodRepository.GetNextPayPeriod(PayrollSummaDateSelection.PayPeriodFromID.Value);
            var payToNextCutOff = _payPeriodRepository.GetNextPayPeriod(PayrollSummaDateSelection.PayPeriodToID.Value);

            var payrollPeriodCell = worksheet.Cells(3, 1);
            var payrollPeriodDescription = $"Payroll Period: {payFromNextCutOff?.PayFromDate == null ? "" : payFromNextCutOff.PayFromDate.ToShortDateString} to {payToNextCutOff?.PayToDate == null ? "" : payToNextCutOff.PayToDate.ToShortDateString}";
            payrollPeriodCell.Value = payrollPeriodDescription;

            rowIndex = 5;
        }

        foreach (var employeeGroup in employeeGroups)
        {
            var divisionCell = worksheet.Cells(rowIndex, 1);
            divisionCell.Value = employeeGroup.DivisionName;
            divisionCell.Style.Font.Italic = true;

            rowIndex += 1;

            RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns);

            rowIndex += 1;

            var employeesStartIndex = rowIndex;
            var employeesLastIndex = 0;

            foreach (var employee in employeeGroup.Employees)
            {
                var letters = GenerateAlphabet.GetEnumerator();

                foreach (var reportColumn in viewableReportColumns)
                {
                    letters.MoveNext();
                    var alphabet = letters.Current;

                    var column = $"{alphabet}{rowIndex}";

                    var cell = worksheet.Cells(column);
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
        worksheet.Cells("A1").AutoFitColumns(4.9, 5.3);

        rowIndex += 1;

        if (employeeGroups.Count > 1)
            RenderGrandTotal(worksheet, rowIndex, lastCell, subTotalRows, 'C');

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
            var employeeId = ObjectUtils.ToNullableInteger(employee(EmployeeRowIDColumnName));

            var adjustment = _adjustments.Where(a => a.Product.PartNo == productName).Where(a => a.Paystub.EmployeeID.Value == employeeId.Value).Where(a => a.Paystub.RowID.Value == ObjectUtils.ToInteger(employee("PaystubId"))).Sum(a => a.Amount);

            return adjustment;
        }

        return employee(sourceName);
    }

    private async Task<IReadOnlyCollection<ExcelReportColumn>> GetViewableReportColumns(ICollection<DataRow> allEmployees, bool hideEmptyColumns, PayrollSummaDateSelection payrollSummaDateSelection)
    {
        var viewableReportColumns = new List<ExcelReportColumn>();
        foreach (var reportColumn in _reportColumns)
        {
            if (reportColumn.Optional && hideEmptyColumns)
            {
                var hasValue = allEmployees.Any(row => !IsDBNull(row(reportColumn.Source)) & !System.Convert.ToDouble(row(reportColumn.Source)) == 0);

                if (hasValue)
                    viewableReportColumns.Add(reportColumn);
            }
            else
                viewableReportColumns.Add(reportColumn);
        }

        if (GetPayrollSummaryAdjustmentBreakdownPolicy() != PayrollSummaryAdjustmentBreakdownPolicy.TotalOnly)
            await AddAdjustmentBreakdownColumns(allEmployees, viewableReportColumns, payrollSummaDateSelection);

        return viewableReportColumns;
    }

    private void RenderSignatureFields(ExcelWorksheet worksheet, int startIdx)
    {
        int index = (startIdx + 1);

        {
            var withBlock = worksheet;
            withBlock.Cells($"A{index}").Value = "Prepared by: ";
            withBlock.Cells($"A{index}:B{index}").Merge = true;

            index += 1;
            withBlock.Cells($"A{index}").Value = "Audited by: ";
            withBlock.Cells($"A{index}:B{index}").Merge = true;

            index += 1;
            withBlock.Cells($"A{index}").Value = "Approved by: ";
            withBlock.Cells($"A{index}:B{index}").Merge = true;
        }
    }

    private SalaryActualization SalaryActualDeclared()
    {
        SalaryActualization time_logformat;

        MessageBoxManager.OK = "Declared";

        MessageBoxManager.Cancel = "Actual";

        MessageBoxManager.Register();

        var custom_prompt = MessageBox.Show("", "", MessageBoxButtons.OKCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

        if (custom_prompt == Windows.Forms.DialogResult.OK)
            time_logformat = SalaryActualization.Declared;
        else if (custom_prompt == Windows.Forms.DialogResult.Cancel)
            time_logformat = SalaryActualization.Actual;

        MessageBoxManager.Unregister();

        return time_logformat;
    }

    private ExcelOption ExcelOptionFormat()
    {
        ExcelOption result_value;

        MessageBoxManager.OK = "(A)";

        MessageBoxManager.Cancel = "(B)";

        MessageBoxManager.Register();

        string message_content = string.Concat("Please select an option :", NewLiner(2), "A ) keep all in one sheet", NewLiner, "B ) separate sheet by department");

        var custom_prompt = MessageBox.Show(message_content, "Excel sheet format", MessageBoxButtons.OKCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

        if (custom_prompt == Windows.Forms.DialogResult.OK)
            result_value = ExcelOption.KeepAllInOneSheet;
        else
            result_value = ExcelOption.SeparateEachDepartment;

        MessageBoxManager.Unregister();

        return result_value;
    }

    private string NewLiner(int repetition = 1)
    {
        string _result = string.Empty;

        var i = 0;
        while (i < repetition)
        {
            _result = string.Concat(_result, Environment.NewLine);
            i += 1;
        }

        return _result;
    }

    private ICollection<EmployeeGroup> GroupEmployees(ICollection<DataRow> allEmployees)
    {
        var employeesByGroups = allEmployees.GroupBy(r => r("DivisionID").ToString());

        var groups = new Collection<EmployeeGroup>();

        foreach (var employeesByGroup in employeesByGroups)
        {
            var group = new EmployeeGroup()
            {
                Employees = employeesByGroup.ToList()
            };

            var employee = group.Employees.FirstOrDefault();
            group.DivisionName = employee("DatCol1").ToString();

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

    private async Task AddAdjustmentBreakdownColumns(ICollection<DataRow> allEmployees, List<ExcelReportColumn> viewableReportColumns, PayrollSummaDateSelection payrollSummaDateSelection)
    {
        var adjustments = await GetCurrentAdjustments(payrollSummaDateSelection, allEmployees);

        var groupedAdjustments = adjustments.GroupBy(a => a.ProductID).ToList;

        var totalAdjustmentReportColumn = viewableReportColumns.Where(r => r.Name == totalAdjustmentColumn).FirstOrDefault;
        var totalAdjustmentColumnIndex = viewableReportColumns.IndexOf(totalAdjustmentReportColumn);

        var counter = 1;

        // add breakdown columns
        foreach (var adjustment in groupedAdjustments)
        {
            var adjustmentName = GetAdjustmentName(adjustment(0).Product?.Name);

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

    public async Task<List<System.Data.IAdjustment>> GetCurrentAdjustments(PayrollSummaDateSelection payrollSummaDateSelection, ICollection<DataRow> allEmployees = null)
    {
        PayPeriod payPeriodFrom = null/* TODO Change to default(_) if this is not a reference type */;
        PayPeriod payPeriodTo = null/* TODO Change to default(_) if this is not a reference type */;

        if (payrollSummaDateSelection.PayPeriodFromID != null)
            payPeriodFrom = await _payPeriodRepository.GetByIdAsync(payrollSummaDateSelection.PayPeriodFromID.Value);

        if (payrollSummaDateSelection.PayPeriodToID != null)
            payPeriodTo = await _payPeriodRepository.GetByIdAsync(payrollSummaDateSelection.PayPeriodToID.Value);

        if (payPeriodFrom?.PayFromDate == null || payPeriodTo?.PayToDate == null)
            throw new ArgumentException("Cannot fetch pay period data.");

        var employeeIds = GetEmployeeIds(allEmployees);
        var datePeriod = new TimePeriod(payPeriodFrom.PayFromDate, payPeriodTo.PayToDate);

        _adjustments = (await _adjustmentService.GetByMultipleEmployeeAndDatePeriodAsync(z_OrganizationID, employeeIds, datePeriod)).ToList();

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

    private int[] GetEmployeeIds(ICollection<DataRow> allEmployees)
    {
        List<int> employeeIds = new List<int>();

        for (var index = 0; index <= allEmployees.Count - 1; index++)
        {
            var employeeId = ObjectUtils.ToNullableInteger(allEmployees(index)(EmployeeRowIDColumnName));

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
}

internal enum SalaryActualization : short
{
    Declared = 0,
    Actual = 1
}

internal enum ExcelOption : short
{
    SeparateEachDepartment = 0,
    KeepAllInOneSheet = 1
}
}