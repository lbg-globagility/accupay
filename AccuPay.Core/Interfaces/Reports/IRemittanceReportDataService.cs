using AccuPay.Core.Services.Reports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IRemittanceReportDataService
    {
        Task<ICollection<RemittanceReportRow>> GetData(int organizationId, DateTime dateMonth);

        Task<RemittanceLoanData> GetSssSalaryLoanData(int organizationId, DateTime dateMonth);
    }
}
