using System;
using System.Data;

namespace AccuPay.Core.Interfaces
{
    public interface IPhilHealthMonthlyReportDataService
    {
        DataTable GetData(int organizationId, DateTime date);
    }
}
