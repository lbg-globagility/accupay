using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ICinemaTardinessReportDataService
    {
        Task<List<ICinemaTardinessReportModel>> GetData(int organizationId, DateTime selectedDate, bool isLimitedReport);
    }
}
