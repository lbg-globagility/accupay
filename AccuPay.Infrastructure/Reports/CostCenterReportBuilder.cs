using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
using AccuPay.Core.ValueObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using static AccuPay.Core.Services.CostCenterReportDataService;

namespace AccuPay.Infrastructure.Reports
{
    public class CostCenterReportBuilder : ExcelFormatReport, ICostCenterReportBuilder
    {
        private readonly IReadOnlyCollection<ExcelReportColumn> _reportColumns = GetReportColumns();
        private readonly SystemOwnerService _systemOwnerService;
        private readonly OrganizationRepository _organizationRepository;
        private const string EmployeeIdKey = "EmployeeId";
        private const string EmployeeNameKey = "EmployeeName";
        private const string TotalDaysKey = "TotalDays";
        private const string TotalHoursKey = "TotalHours";
        private const string DailyRateKey = "DailyRate";
        private const string HoulyRateKey = "HoulyRate";
        private const string BasicPayKey = "BasicPay";
        private const string OvertimeHoursKey = "OvertimeHours";
        private const string OvertimePayKey = "OvertimePay";
        private const string NightDiffHoursKey = "NightDiffHours";
        private const string NightDiffPayKey = "NightDiffPay";
        private const string NightDiffOvertimeHoursKey = "NightDiffOvertimeHours";
        private const string NightDiffOvertimePayKey = "NightDiffOvertimePay";
        private const string RestDayHoursKey = "RestDayHours";
        private const string RestDayPayKey = "RestDayPay";
        private const string RestDayOTHoursKey = "RestDayOTHours";
        private const string RestDayOTPayKey = "RestDayOTPay";
        private const string SpecialHolidayHoursKey = "SpecialHolidayHours";
        private const string SpecialHolidayPayKey = "SpecialHolidayPay";
        private const string SpecialHolidayOTHoursKey = "SpecialHolidayOTHours";
        private const string SpecialHolidayOTPayKey = "SpecialHolidayOTPay";
        private const string RegularHolidayHoursKey = "RegularHolidayHours";
        private const string RegularHolidayPayKey = "RegularHolidayPay";
        private const string RegularHolidayOTHoursKey = "RegularHolidayOTHours";
        private const string RegularHolidayOTPayKey = "RegularHolidayOTPay";
        private const string TotalAllowanceKey = "TotalAllowanceKey";
        private const string GrossPayKey = "GrossPay";
        private const string SSSAmountKey = "SSSAmount";
        private const string ECAmountKey = "ECAmount";
        private const string HDMFAmountKey = "HDMFAmount";
        private const string PhilHealthAmountKey = "PhilHealthAmount";
        private const string HMOAmountKey = "HMOAmount";
        private const string ThirteenthMonthPayKey = "ThirteenthMonthPay";
        private const string FiveDaySilpAmountKey = "FiveDaySilpAmount"; // 5 Day SILP (leave)
        private const string NetPayKey = "NetPay";

        public CostCenterReportBuilder(SystemOwnerService systemOwnerService, OrganizationRepository organizationRepository)
        {
            _systemOwnerService = systemOwnerService;
            _organizationRepository = organizationRepository;
        }

        private static ReadOnlyCollection<ExcelReportColumn> GetReportColumns()
        {
            var reportColumns = new List<ExcelReportColumn>()
            {
                new ExcelReportColumn("NAME OF EMPLOYEES", EmployeeNameKey, ExcelReportColumn.ColumnType.Text),
                new ExcelReportColumn("NO. OF DAYS", TotalDaysKey),
                new ExcelReportColumn("NO. OF HOURS", TotalHoursKey),
                new ExcelReportColumn("RATE", DailyRateKey),
                new ExcelReportColumn("HOURLY", HoulyRateKey),
                new ExcelReportColumn("GROSS PAY", BasicPayKey),
                new ExcelReportColumn("NO. OF OT HOURS", OvertimeHoursKey, optional: true),
                new ExcelReportColumn("OT PAY", OvertimePayKey, optional: true),
                new ExcelReportColumn("NO. OF ND HOURS", NightDiffHoursKey, optional: true),
                new ExcelReportColumn("ND PAY", NightDiffPayKey, optional: true),
                new ExcelReportColumn("NO. OF NDOT HOURS", NightDiffOvertimeHoursKey, optional: true),
                new ExcelReportColumn("NDOT PAY", NightDiffOvertimePayKey, optional: true),
                new ExcelReportColumn("REST DAY HOURS", RestDayHoursKey, optional: true),
                new ExcelReportColumn("REST DAY PAY", RestDayPayKey, optional: true),
                new ExcelReportColumn("REST DAY OT HOURS", RestDayOTHoursKey, optional: true),
                new ExcelReportColumn("REST DAY OT PAY", RestDayOTPayKey, optional: true),
                new ExcelReportColumn("SP HOLIDAY HOURS", SpecialHolidayHoursKey, optional: true),
                new ExcelReportColumn("SP HOLIDAY PAY", SpecialHolidayPayKey, optional: true),
                new ExcelReportColumn("SP HOLIDAY OT HOURS", SpecialHolidayOTHoursKey, optional: true),
                new ExcelReportColumn("SP HOLIDAY OT PAY", SpecialHolidayOTPayKey, optional: true),
                new ExcelReportColumn("LEGAL HOLIDAY HOURS", RegularHolidayHoursKey, optional: true),
                new ExcelReportColumn("LH HOLIDAY PAY", RegularHolidayPayKey, optional: true),
                new ExcelReportColumn("LEGAL HOLIDAY OT HOURS", RegularHolidayOTHoursKey, optional: true),
                new ExcelReportColumn("LH HOLIDAY OT PAY", RegularHolidayOTPayKey, optional: true),
                new ExcelReportColumn("ALLOWANCE", TotalAllowanceKey, optional: true),
                new ExcelReportColumn("TOTAL GROSS PAY", GrossPayKey),
                new ExcelReportColumn("SSS", SSSAmountKey),
                new ExcelReportColumn("EREC", ECAmountKey),
                new ExcelReportColumn("PAG-IBIG", HDMFAmountKey),
                new ExcelReportColumn("PHILHEALTH", PhilHealthAmountKey),
                new ExcelReportColumn("HMO", HMOAmountKey),
                new ExcelReportColumn("13TH MONTH PAY", ThirteenthMonthPayKey),
                new ExcelReportColumn("5 DAY SILP", FiveDaySilpAmountKey),
                new ExcelReportColumn("NET PAY", NetPayKey)
            };

            return new ReadOnlyCollection<ExcelReportColumn>(reportColumns);
        }

        public void CreateReport(IEnumerable<IGrouping<Branch, PayPeriodModel>> branchPaystubModels, string saveFilePath, int organizationId)
        {
            var newFile = new FileInfo(saveFilePath);

            var organization = _organizationRepository.GetById(organizationId);

            if (organization == null)
                throw new Exception("Organization is required.");

            using (var excel = new ExcelPackage(newFile))
            {
                var subTotalRows = new List<int>();

                var worksheet = excel.Workbook.Worksheets.Add("Sheet1");

                var viewableReportColumns = GetViewableReportColumns(branchPaystubModels);

                RenderWorksheet(branchPaystubModels, worksheet, viewableReportColumns, organization.Name);

                SetDefaultPrinterSettings(worksheet.PrinterSettings);

                excel.Save();
            }
        }

        private void RenderWorksheet(
            IEnumerable<IGrouping<Branch, PayPeriodModel>> branchPaystubModels,
            ExcelWorksheet worksheet,
            IReadOnlyCollection<ExcelReportColumn> viewableReportColumns,
            string orgNam)
        {
            int rowIndex = 1;
            string lastColumn = GetLastColumn(viewableReportColumns);

            worksheet.Cells.Style.Font.Size = FontSize;

            if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
                worksheet.Cells.Style.Font.Name = "Book Antiqua";

            var organizationCell = worksheet.Cells[rowIndex, 1];
            organizationCell.Value = orgNam.ToUpper();
            organizationCell.Style.Font.Bold = true;
            rowIndex += 1;

            var monthlyBranchSubTotalRows = new List<int>();

            foreach (var branchGroup in branchPaystubModels)
            {
                if (branchGroup.ToList().SelectMany(p => p.Paystubs).Any())
                    rowIndex = RenderBranchData(worksheet, branchGroup, viewableReportColumns, monthlyBranchSubTotalRows, lastColumn, rowIndex);
            }

            if (monthlyBranchSubTotalRows.Count > 1)
            {
                rowIndex += 3;

                RenderGrandTotalLabel(worksheet, "GRAND TOTAL", rowIndex);
                RenderGrandTotal(worksheet, rowIndex, SecondColumn, lastColumn, monthlyBranchSubTotalRows, ExcelBorderStyle.Thick);
            }
        }

        private IReadOnlyCollection<ExcelReportColumn> GetViewableReportColumns(IEnumerable<IGrouping<Branch, PayPeriodModel>> branchPaystubModels)
        {
            var payPeriodModels = branchPaystubModels.SelectMany(b => b.ToList()).ToList();

            List<ExcelReportColumn> viewableReportColumns = new List<ExcelReportColumn>();

            foreach (var column in _reportColumns)
            {
                if (column.Optional == false)
                {
                    viewableReportColumns.Add(column);
                    continue;
                }

                decimal totalValue = 0;

                payPeriodModels.ForEach(m =>
                {
                    m.Paystubs.ForEach(p =>
                    {
                        totalValue += Convert.ToDecimal(p.LookUp[column.Source]);
                    });
                });

                if (totalValue != 0)
                    viewableReportColumns.Add(column);
            }

            return viewableReportColumns;
        }

        private int RenderBranchData(
            ExcelWorksheet worksheet,
            IGrouping<Branch, PayPeriodModel> branchGroup,
            IReadOnlyCollection<ExcelReportColumn> viewableReportColumns,
            List<int> monthlyBranchSubTotalRows,
            string lastColumn,
            int rowIndex)
        {
            Branch branch = branchGroup.Key;
            string branchName = branch.Name;

            ICollection<PayPeriodModel> payPeriods = branchGroup.ToList();

            List<int> branchSubTotalRows = new List<int>();

            // space after the title
            rowIndex += 1;

            foreach (var payPeriodModel in payPeriods)
            {
                ExcelRange branchNameCell = worksheet.Cells[rowIndex, 1];
                branchNameCell.Value = GetPayPeriodDescription(payPeriodModel.PayPeriod);
                branchNameCell.Style.Font.Bold = true;
                rowIndex += 1;

                ExcelRange payPeriodDateCell = worksheet.Cells[rowIndex, 1];
                payPeriodDateCell.Value = branchName;
                payPeriodDateCell.Style.Font.Bold = true;
                rowIndex += 1;

                RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns);
                rowIndex += 1;

                if (payPeriodModel.Paystubs.Count > 0)
                    rowIndex = RenderGroupedRows(worksheet, viewableReportColumns, branchSubTotalRows, rowIndex, lastColumn, payPeriodModel);
                else
                {
                    RenderZeroTotal(worksheet, rowIndex, SecondColumn, lastColumn, ExcelBorderStyle.Thin);

                    rowIndex += 1;
                }

                rowIndex += 1;
            }

            worksheet.Cells.AutoFitColumns();
            worksheet.Cells["A1"].AutoFitColumns(4.9, 5.3);

            rowIndex += 1;

            if (payPeriods.Count > 1)
            {
                RenderGrandTotalLabel(worksheet, branchName, rowIndex);
                RenderGrandTotal(worksheet, rowIndex, SecondColumn, lastColumn, branchSubTotalRows);

                monthlyBranchSubTotalRows.Add(rowIndex);
            }

            rowIndex += 1;

            return rowIndex;
        }

        private void RenderGrandTotalLabel(ExcelWorksheet worksheet, string branchName, int rowIndex)
        {
            var column = $"{FirstColumn}{rowIndex}";
            var cell = worksheet.Cells[column];
            cell.Style.Font.Bold = true;

            cell.Value = branchName;
        }

        private string GetPayPeriodDescription(TimePeriod payPeriod)
        {
            if (payPeriod == null)
                return string.Empty;

            if (payPeriod.Start.Month == payPeriod.End.Month)
                return $"{payPeriod.Start.ToString("MMMM").ToUpper()} {payPeriod.Start.Day} - {payPeriod.End.Day}";
            else
                return $"{payPeriod.Start.ToString("MMMM").ToUpper()} {payPeriod.Start.Day} - {payPeriod.End.ToString("MMMM").ToUpper()} {payPeriod.End.Day}";
        }

        private int RenderGroupedRows(
            ExcelWorksheet worksheet,
            IReadOnlyCollection<ExcelReportColumn> viewableReportColumns,
            List<int> subTotalRows,
            int rowIndex,
            string lastColumn,
            PayPeriodModel payPeriodModel)
        {
            int employeesStartRowIndex = rowIndex;
            int employeesLastRowIndex = rowIndex;

            foreach (var paystub in payPeriodModel.Paystubs)
            {
                var letters = GenerateAlphabet().GetEnumerator();
                var propertyLookUp = paystub.LookUp;

                foreach (var reportColumn in viewableReportColumns)
                {
                    letters.MoveNext();
                    var alphabet = letters.Current;

                    var column = $"{alphabet}{rowIndex}";

                    var cell = worksheet.Cells[column];

                    var value = propertyLookUp[reportColumn.Source];
                    if (reportColumn.Type == ExcelReportColumn.ColumnType.Numeric)
                        cell.Value = Convert.ToDecimal(value);
                    else
                        cell.Value = value;

                    if (reportColumn.Type == ExcelReportColumn.ColumnType.Numeric)
                    {
                        cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                }

                employeesLastRowIndex = rowIndex;
                rowIndex += 1;
            }

            var subTotalCellRange = $"B{rowIndex}:{lastColumn}{rowIndex}";

            subTotalRows.Add(rowIndex);

            RenderSubTotal(worksheet, subTotalCellRange, employeesStartRowIndex, employeesLastRowIndex, formulaColumnStart: 2);

            rowIndex += 1;

            return rowIndex;
        }
    }
}