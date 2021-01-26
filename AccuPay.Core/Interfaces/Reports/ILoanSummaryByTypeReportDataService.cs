using System;
using System.Data;

namespace AccuPay.Core.Interfaces
{
    public interface ILoanSummaryByTypeReportDataService
    {
        DataTable GetData(int organizationId, DateTime dateFrom, DateTime dateTo);
    }
}
