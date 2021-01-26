using System.Data;
using System.Threading.Tasks;

namespace AccuPay.CrystalReports
{
    public interface IPayslipBuilder : IBaseReportBuilder
    {
        PayslipBuilder AddPdfPassword(string password);

        bool CheckIfEmployeeExists(int employeeId);

        Task<PayslipBuilder> CreateReportDocumentAsync(int payPeriodId, bool isActual = false, int[] employeeIds = null);

        BaseReportBuilder GeneratePDF(string pdfFullPath);

        BaseReportBuilder GeneratePDF(string saveFolderPath, string fileName);

        DataRow GetFirstEmployee();
    }
}
