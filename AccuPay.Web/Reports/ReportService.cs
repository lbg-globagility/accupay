using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Web.Reports
{
    public class ReportService
    {
        private const string ExcelFileExtension = ".xlsx";

        private readonly ICurrentUser _currentUser;
        private readonly IPayrollSummaryReportBuilder _reportBuilder;

        public ReportService(ICurrentUser currentUser, IPayrollSummaryReportBuilder reportBuilder)
        {
            _currentUser = currentUser;
            _reportBuilder = reportBuilder;
        }

        public async Task<string> GetPayrollSummary(
            bool keepInOneSheet,
            bool hideEmptyColumns,
            int payPeriodFromId,
            int payPeriodToId,
            string salaryDistributionType,
            bool isActual)
        {
            var fileName = Path.GetTempFileName();

            var extension = Path.GetExtension(fileName);

            var excelFileName = ReplaceLastOccurrence(fileName, extension, ExcelFileExtension);

            await _reportBuilder.CreateReport(
                keepInOneSheet: keepInOneSheet,
                hideEmptyColumns: hideEmptyColumns,
                organizationId: _currentUser.OrganizationId,
                payPeriodFromId: payPeriodFromId,
                payPeriodToId: payPeriodToId,
                salaryDistributionType: salaryDistributionType,
                isActual: isActual,
                saveFilePath: excelFileName);

            return excelFileName;
        }

        public static string ReplaceLastOccurrence(string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);

            if (place == -1)
                return source;

            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }
    }
}
