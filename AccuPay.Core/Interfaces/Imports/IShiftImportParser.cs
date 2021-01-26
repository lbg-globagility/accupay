using AccuPay.Core.Services.Policies;
using System.IO;
using System.Threading.Tasks;
using static AccuPay.Core.Services.Imports.ShiftImportParser;

namespace AccuPay.Core.Interfaces
{
    public interface IShiftImportParser
    {
        string XlsxExtension { get; }

        Task<ShiftImportParserOutput> Parse(Stream importFile, int organizationId);

        Task<ShiftImportParserOutput> Parse(string filePath, int organizationId);

        void SetShiftBasedAutoOvertimePolicy(ShiftBasedAutomaticOvertimePolicy shiftBasedAutoOvertimePolicy);
    }
}
