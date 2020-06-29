using AccuPay.CrystalReports.Payslip;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace AccuPay.CrystalReportsWeb.Controllers
{
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        private readonly PayslipCreator _payslipCreator;

        public ReportsController(PayslipCreator organizationRepository)
        {
            this._payslipCreator = organizationRepository;
        }

        [Route("payslip/{payPeriodId}")]
        public HttpResponseMessage GetPayslip(int payPeriodId)
        {
            string pdfFullPath = Path.GetTempFileName();

            _payslipCreator
                .CreateReportDocument(
                    payPeriodId: payPeriodId)
                .GeneratePDF(pdfFullPath);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(File.OpenRead(pdfFullPath))
            };

            var contentType = "application/pdf";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return response;
        }

        [Route("test")]
        public string Reports()
        {
            return "sdf";
        }
    }
}