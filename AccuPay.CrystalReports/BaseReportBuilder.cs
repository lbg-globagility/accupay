using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace AccuPay.CrystalReports
{
    public class BaseReportBuilder
    {
        protected ReportClass _reportDocument;

        protected string _pdfFile;

        public BaseReportBuilder()
        {
            _reportDocument = new ReportClass();
        }

        public ReportClass GetReportDocument()
        {
            return _reportDocument;
        }

        public string GetPDF()
        {
            return _pdfFile;
        }

        protected void ExportPDF(string pdfFullPath)
        {
            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = pdfFullPath;
            CrExportOptions = _reportDocument.ExportOptions;

            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
            CrExportOptions.FormatOptions = CrFormatTypeOptions;

            _reportDocument.Export();

            _pdfFile = pdfFullPath;
        }
    }
}