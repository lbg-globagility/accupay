using System.IO;
using System.Threading.Tasks;
using static AccuPay.Core.Services.Imports.Salaries.SalaryImportParser;

namespace AccuPay.Core.Interfaces
{
    public interface ISalaryImportParser
    {
        string XlsxExtension { get; }

        Task<SalaryImportParserOutput> Parse(Stream importFile, int organizationId);
    }
}
