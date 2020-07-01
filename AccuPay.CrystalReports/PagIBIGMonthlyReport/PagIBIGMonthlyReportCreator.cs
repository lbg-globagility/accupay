using AccuPay.CrystalReports.PagIBIGMonthlyReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.CrystalReports
{
    public class PagIBIGMonthlyReportBuilder : BaseReportBuilder, IPdfGenerator
    {
        private readonly PagIBIGMonthlyReportDataService _dataService;

        public PagIBIGMonthlyReportBuilder(PagIBIGMonthlyReportDataService dataService)
        {
            _dataService = dataService;
        }

        public PagIBIGMonthlyReportBuilder CreateReportDocument(int organizationId, DateTime date)
        {

            _reportDocument = new Pagibig_Monthly_Report();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, date));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text2"];

            var date_from = date.ToString("MMMM  yyyy");
            objText.Text = "for the month of " + date_from;

            return this;
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }
    }
}
