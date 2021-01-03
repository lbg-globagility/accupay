using AccuPay.Core.Services.Imports.Overtimes;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IOvertimeImportParser
    {
        string XlsxExtension { get; }

        Task<OvertimeImportParserOutput> Parse(Stream importFile, int organizationId);
    }
}
