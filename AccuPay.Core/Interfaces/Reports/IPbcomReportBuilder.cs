using AccuPay.Core.ReportModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Reports
{
    public interface IPbcomReportBuilder
    {
        Task<List<PbcomReportModel>> GetData(DateTime startPeriod, DateTime endPeriod, int organizationId);
    }
}
