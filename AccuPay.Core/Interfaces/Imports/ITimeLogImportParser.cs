using AccuPay.Core.Services.Imports;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccuPay.Infrastructure.Data.TimeLogImportParser;

namespace AccuPay.Core.Interfaces
{
    public interface ITimeLogImportParser
    {
        AllParsedLogOutput GetAllLogs(ITimeAttendanceHelper helper, IList<TimeLogImportModel> parsedErrors);

        Task<ITimeAttendanceHelper> GetHelper(List<TimeLogImportModel> logs, int organizationId, int userId);

        Task<TimeLogImportParserOutput> Parse(string importFile, int organizationId, int userId);
    }
}
