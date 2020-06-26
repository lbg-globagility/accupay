using AccuPay.CrystalReports.LoanSummaryByEmployeeReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.CrystalReports
{
    public class LoanSummaryByEmployeeReportCreator
    {
        private readonly LoanSummaryByEmployeeReportDataService _dataService;
        private ReportClass _reportDocument;

        public LoanSummaryByEmployeeReportCreator(LoanSummaryByEmployeeReportDataService dataService)
        {
            _dataService = dataService;
        }

        public LoanSummaryByEmployeeReportCreator CreateReportDocument(int organizationId, DateTime date_from, DateTime date_to)
        {
            _reportDocument = new LoanReports();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, date_from, date_to));

            TextObject objText = null/* TODO Change to default(_) if this is not a reference type */;

            objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["PeriodDate"];

            objText.Text = string.Concat("for the period of ", date_from.ToShortDateString(), " to ", date_to.ToShortDateString());

            objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["txtOrganizationName"];

            return this;
        }

        public ReportClass GetReportDocument()
        {
            return _reportDocument;
        }
    }
}
