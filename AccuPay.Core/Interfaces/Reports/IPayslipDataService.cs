using System.Data;

namespace AccuPay.Core.Interfaces
{
    public interface IPayslipDataService
    {
        DataTable GetCinema2000Data(int organizationId, int payPeriodId);

        DataTable GetDefaultData(int organizationId, int payPeriodId, bool isActual);

        DataTable GetGoldWingsData(int organizationId, int payPeriodId, bool isActual);
    }
}
