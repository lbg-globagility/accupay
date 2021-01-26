using AccuPay.Core.Services.Imports.Loans;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILoanImportParser
    {
        string XlsxExtension { get; }

        Task<LoanImportParserOutput> Parse(Stream importFile, int organizationId);
    }
}
