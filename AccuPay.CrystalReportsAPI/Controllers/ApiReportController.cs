using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;

namespace AccuPay.CrystalReportsAPI.Controllers
{
    public class ApiReportController : ApiController
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage PdfReportResult(string pdfFullPath)
        {
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