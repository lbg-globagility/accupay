using AccuPay.CrystalReports.TaxMonthlyReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.CrystalReports
{
    public class TaxMonthlyReportCreator
    {
        private readonly TaxMonthlyReportDataService _dataService;
        private ReportClass _reportDocument;

        public TaxMonthlyReportCreator(TaxMonthlyReportDataService dataService)
        {
            _dataService = dataService;
        }

        public TaxMonthlyReportCreator CreateReportDocument(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            _reportDocument = new Tax_Monthly_Report();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, dateFrom, dateTo));

            var objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text2"];
            objText.Text = "For the month of  " + dateFrom.ToString("MMMM yyyy");

            return this;
        }

        public ReportClass GetReportDocument()
        {
            return _reportDocument;
        }
    }
}
