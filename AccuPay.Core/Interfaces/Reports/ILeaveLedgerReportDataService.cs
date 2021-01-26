using AccuPay.Core.ReportModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILeaveLedgerReportDataService
    {
        Task<List<LeaveLedgerReportModel>> GetData(int organizationId, DateTime startDate, DateTime endDate);
    }
}
