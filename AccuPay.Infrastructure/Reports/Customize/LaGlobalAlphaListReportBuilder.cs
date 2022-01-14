using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Infrastructure.Reports.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static AccuPay.Infrastructure.Reports.ExcelReportColumn;

namespace AccuPay.Infrastructure.Reports.Customize
{
    public class LaGlobalAlphaListReportBuilder : ExcelFormatReport, ILaGlobalAlphaListReportBuilder
    {
        private readonly ILaGlobalAlphaListReportDataService _dataService;

        public IOrganizationRepository _organizationRepository { get; }

        private readonly IPayPeriodRepository _payPeriodRepository;
        private readonly IPaystubDataService _paystubDataService;
        private readonly ISystemOwnerService _systemOwnerService;
        private readonly IListOfValueService _listOfValueService;
        private readonly ReadOnlyCollection<ExcelReportColumn> _reportColumns;

        public LaGlobalAlphaListReportBuilder(ILaGlobalAlphaListReportDataService dataService,
            IOrganizationRepository organizationRepository,
            IPayPeriodRepository payPeriodRepository,
            IPaystubDataService paystubDataService,
            ISystemOwnerService systemOwnerService,
            IListOfValueService listOfValueService)
        {
            _dataService = dataService;
            _organizationRepository = organizationRepository;
            _payPeriodRepository = payPeriodRepository;
            _paystubDataService = paystubDataService;
            _systemOwnerService = systemOwnerService;
            _listOfValueService = listOfValueService;

            _reportColumns = GetReportColumns();
        }

        public async Task GenerateReportAsync(int organizationId,
            bool actualSwitch,
            int startPeriodId,
            int endPeriodId,
            string saveFilePath)
        {
            var data = await _dataService.GetData(organizationId: organizationId,
                actualSwitch: actualSwitch,
                startPeriodId: startPeriodId,
                endPeriodId: endPeriodId,
                saveFilePath: saveFilePath);

            var payPeriod = await GetSelectedPayPeriod(
                payPeriodFromId: startPeriodId,
                payPeriodToId: endPeriodId);

            var organization = await _organizationRepository.GetByIdAsync(organizationId);

            string workSheetName = "Sheet1";

            string[] shortDates = new string[] { payPeriod.DateFrom.ToShortDateString(),
                payPeriod.DateTo.ToShortDateString() };

            var newFile = new FileInfo(saveFilePath);

            using (var excel = new ExcelPackage(newFile))
            {
                var subTotalRows = new List<int>();

                var worksheet = excel.Workbook.Worksheets.Add(workSheetName);

                worksheet.Cells.Style.Font.Size = FontSize;

                var organizationCell = worksheet.Cells[1, 1];
                organizationCell.Value = organization.Name.ToUpper();
                organizationCell.Style.Font.Bold = true;

                var attendancePeriodCell = worksheet.Cells[2, 1];
                var attendancePeriodDescription = $"For the period of {shortDates[0]} to {shortDates[1]}";
                attendancePeriodCell.Value = attendancePeriodDescription;

                var lastCell = string.Empty;
                int rowIndex = 4;

                rowIndex += 1;

                RenderColumnHeaders(worksheet, rowIndex, _reportColumns);

                rowIndex += 1;

                var startIndex = rowIndex;
                var lastIndex = 0;

                foreach (var model in data)
                {
                    var letters = GenerateAlphabet().GetEnumerator();

                    foreach (var reportColumn in _reportColumns)
                    {
                        letters.MoveNext();

                        var alphabet = letters.Current;

                        var column = $"{alphabet}{rowIndex}";

                        var cell = worksheet.Cells[column];
                        var sourceName = reportColumn.Source;
                        cell.Value = GetPropertyValue(model, sourceName: sourceName);

                        if (reportColumn.Type == ColumnType.Numeric)
                        {
                            cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                    }

                    lastCell = letters.Current;

                    lastIndex = rowIndex;
                    rowIndex += 1;
                }

                var subTotalCellRange = $"G{rowIndex}:{lastCell}{rowIndex}";

                subTotalRows.Add(rowIndex);

                RenderSubTotal(worksheet, subTotalCellRange, startIndex, lastIndex, formulaColumnStart: 3);

                rowIndex += 2;

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

        private ReadOnlyCollection<ExcelReportColumn> GetReportColumns()
        {
            var reportColumns = new List<ExcelReportColumn>()
            {
                new ExcelReportColumn("Employee ID", "EmployeeId", ColumnType.Text),
                new ExcelReportColumn("Full Name", "Name", ColumnType.Text),
                new ExcelReportColumn("TIN", "TIN", ColumnType.Text),
                new ExcelReportColumn("Address", "Address", ColumnType.Text),
                new ExcelReportColumn("Birth Date", "BirthDate", ColumnType.Text),
                new ExcelReportColumn("Contact No", "ContactNo", ColumnType.Text),
                new ExcelReportColumn("Gross Salary", "Gross", ColumnType.Numeric),
                new ExcelReportColumn("SSS", "SSS", ColumnType.Numeric),
                new ExcelReportColumn("PhilHealth", "PhilHealth", ColumnType.Numeric),
                new ExcelReportColumn("HDMF", "HDMF", ColumnType.Numeric),
                new ExcelReportColumn("13th month pay", "ThirteenthMonthPay", ColumnType.Numeric)
            };

            return new ReadOnlyCollection<ExcelReportColumn>(reportColumns);
        }

        private Object GetPropertyValue(Object obj, string sourceName)
        {
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            var get = props.Where(p => p.Name == sourceName).FirstOrDefault();
            if (get == null) return null;
            return get.GetValue(obj);
        }
    }
}
