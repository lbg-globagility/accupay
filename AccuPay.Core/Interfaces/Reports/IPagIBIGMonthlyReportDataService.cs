using System;
using System.Data;

namespace AccuPay.Core.Interfaces
{
    public interface IPagIBIGMonthlyReportDataService
    {
        DataTable GetData(int organizationId, DateTime date);
    }
}
