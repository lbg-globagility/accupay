using System;
using System.Data;

namespace AccuPay.Core.Interfaces
{
    public interface ILoanSummaryByEmployeeReportDataService
    {
        DataTable GetData(int organizationId, DateTime date_from, DateTime date_to);
    }
}
