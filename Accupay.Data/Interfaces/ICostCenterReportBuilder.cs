using AccuPay.Data.Entities;
using AccuPay.Data.Services;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Interfaces
{
    public interface ICostCenterReportBuilder
    {
        void CreateReport(IEnumerable<IGrouping<Branch, CostCenterReportDataService.PayPeriodModel>> branchPaystubModels, string saveFilePath, int organizationId);
    }
}