using System.IO;
using System.Threading.Tasks;
using static AccuPay.Core.Services.Imports.Employees.EmployeeImportParser;

namespace AccuPay.Core.Interfaces
{
    public interface IEmployeeImportParser
    {
        string XlsxExtension { get; }

        Task<EmployeeImportParserOutput> Parse(Stream importFile, int organizationId);

        Task<EmployeeImportParserOutput> Parse(string filePath, int organizationId);
    }
}
