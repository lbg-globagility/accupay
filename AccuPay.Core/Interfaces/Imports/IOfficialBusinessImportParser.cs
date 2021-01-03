using AccuPay.Core.Services.Imports.OfficialBusiness;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IOfficialBusinessImportParser
    {
        string XlsxExtension { get; }

        Task<OfficialBusinessImportParserOutput> Parse(Stream importFile, int organizationId);
    }
}
