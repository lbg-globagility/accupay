using AccuPay.Core.ReportModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPaystubPayslipModelDataService
    {
        Task<List<PaystubPayslipModel>> GetData(int organizationId, IPayPeriod payPeriod, bool isActual = false);
    }
}
