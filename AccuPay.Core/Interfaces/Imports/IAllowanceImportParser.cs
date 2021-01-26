using AccuPay.Core.Services.Imports.Allowances;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAllowanceImportParser
    {
        string XlsxExtension { get; }

        Task<AllowanceImportParserOutput> Parse(Stream importFile, int organizationId);
    }
}
