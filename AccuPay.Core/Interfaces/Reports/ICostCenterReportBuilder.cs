using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Interfaces
{
    public interface ICostCenterReportBuilder
    {
        void CreateReport(IEnumerable<IGrouping<Branch, CostCenterReportDataService.PayPeriodModel>> branchPaystubModels, string saveFilePath, int organizationId);
    }
}