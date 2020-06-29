using AccuPay.CrystalReports.Payslip;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace AccuPay.CrystalReportsWeb.Controllers
{
    /// <summary>
    /// Reports API endpoints returning pdf files.
    /// </summary>
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        private readonly PayslipCreator _payslipCreator;

        /// <summary>
        /// sdf
        /// </summary>
        public ReportsController(PayslipCreator organizationRepository)
        {
            _payslipCreator = organizationRepository;
        }

        /// <summary>
        /// Download pdf of all the employee payslip per pay period.
        /// </summary>
        /// <param name="payPeriodId">The pay period ID. Organization ID is fetched from the payperiod object that is fetched using payPeriodId.</param>
        /// <returns></returns>
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
    }
}