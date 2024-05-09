using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Reports.Customize
{
    public interface IRGIPayslip
    {
        Task CreateReport(
            int organizationId,
            int payPeriodId,
            int[] employeeIds,
            bool isActual,
            string saveFilePath);
    }
}
