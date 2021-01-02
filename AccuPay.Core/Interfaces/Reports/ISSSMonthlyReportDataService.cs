using System;
using System.Data;

namespace AccuPay.Core.Interfaces
{
    public interface ISSSMonthlyReportDataService
    {
        DataTable GetData(int organizationId, DateTime dateMonth);
    }
}
