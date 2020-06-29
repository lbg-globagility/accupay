using AccuPay.CrystalReports.ThirteenthMonthSummaryReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class ThirteenthMonthSummaryReportCreator
    {
        private readonly ThirteenthMonthSummaryReportDataService _dataService;
        private ReportClass _reportDocument;

        public ThirteenthMonthSummaryReportCreator(ThirteenthMonthSummaryReportDataService dataService)
        {
            _dataService = dataService;
        }

        public ThirteenthMonthSummaryReportCreator CreateReportDocument(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            _reportDocument = new ThirteenthMonthSummary();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, dateFrom, dateTo));

            return this;
        }

        public ReportClass GetReportDocument()
        {
            return _reportDocument;
        }
    }
}
