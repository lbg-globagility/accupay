using AccuPay.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IBaseDataService
    {
        Task<bool> CheckIfDataIsWithinClosedPayPeriod(DateTime date, int organizationId, bool throwException = true);

        Task<bool> CheckIfDataIsWithinClosedPayPeriod(IEnumerable<DateTime> dates, int organizationId, bool throwException = true);

        bool CheckIfDataIsWithinClosedPayPeriod(IEnumerable<PayPeriod> payPeriods, bool throwException = true);
    }
}
