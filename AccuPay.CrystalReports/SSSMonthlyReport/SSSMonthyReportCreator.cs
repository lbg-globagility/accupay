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
using CrystalDecisions.CrystalReports.Engine;
using AccuPay.CrystalReports.SSSMonthlyReport;
using AccuPay.Data.Services;

namespace AccuPay.CrystalReports
{
    public class SSSMonthyReportCreator
    {
        private readonly SSSMonthlyReportDataService _dataService;
        private ReportClass _reportDocument;

        public SSSMonthyReportCreator(SSSMonthlyReportDataService dataService)
        {
            _dataService = dataService;
        }

        public SSSMonthyReportCreator CreateReportDocument(int organizationId, DateTime date)
        {
            _reportDocument = new SSS_Monthly_Report();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, date));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text2"];

            var date_from = date.ToString("MMMM  yyyy");
            objText.Text = "for the month of " + date_from;

            return this;
        }

        public ReportClass GetReportDocument()
        {
            return _reportDocument;
        }
    }
}