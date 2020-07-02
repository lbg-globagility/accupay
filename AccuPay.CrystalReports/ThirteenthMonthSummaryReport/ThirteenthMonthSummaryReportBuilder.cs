using AccuPay.CrystalReports.ThirteenthMonthSummaryReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class ThirteenthMonthSummaryReportBuilder : BaseReportBuilder, IPdfGenerator
    {
        private readonly ThirteenthMonthSummaryReportDataService _dataService;

        public ThirteenthMonthSummaryReportBuilder(ThirteenthMonthSummaryReportDataService dataService)
        {
            _dataService = dataService;
        }

        public ThirteenthMonthSummaryReportBuilder CreateReportDocument(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            _reportDocument = new ThirteenthMonthSummary();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, dateFrom, dateTo));

            return this;
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }
    }
}
